Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class ProductInformation_03
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim out_for_url As String = ""

    Dim UI_txtSerial As String = ""
    Dim UI_lblUserIDText As String = ""        '登入者
    Dim UI_lblRepairCenterValue As String = "" '維修中心
    Dim UI_lblAccountIDText As String = ""
    Dim UI_cboRepairCenter As String = ""

    Dim _RedirectURL As String = "Client_Worklist.aspx"
    Dim _Address As String = ""
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")

    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_Upload_FilePath")
    Dim Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    Dim _Requested_ExcelSample_VisualPath As String = ConfigurationSettings.AppSettings("Requested_ExcelSample_VisualPath")
    Dim _Requested_Default_FarFarcNO As String = ConfigurationSettings.AppSettings("Requested_Default_FarFarcNO")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _RepairEmailCC As String = ConfigurationSettings.AppSettings("RepairEmailCC")
    Dim _RequestNew_JP_BYTE_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_JP_BYTE_EmailCC")
    Dim _RequestNew_US_CL_MPLUS_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_US_CL_MPLUS_EmailCC")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function
    Public Function getoLanguageword(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Word)
    End Function

    Public Function getoLanguageCommand(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Command)
    End Function
    ''' <summary>
    ''' 新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True

            Response.Redirect("~/Default.aspx")
        End If

        If Me.IsPostBack = False Then

            If Not Request.Params("RMANO") Is Nothing And Not Request.Params("eumCommand") Is Nothing Then

                Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                Dim sRMANO As String = Request.Params("RMANO").ToString().Trim()
                Dim oRMA As New ctlRMA.Requested

                dtRMADetail = oRMA.QueryByRMADetail(sRMANO, "")
                Call ArrangementUpData(dtRMADetail)
                Session("_dtRMADetail") = dtRMADetail

                HiddenField_RMANO.Value = sRMANO

            End If

            Dim oExport As New ctlRMA.Export
            Dim oRequested As New ctlRMA.Requested
            Dim ctlReport As New ctlReport
            Dim sMessage As String = ""
            Dim retval As Boolean = True

            Try

                Call RMADetail_DataBind(1)
                Me.UI_txtSerial = ""

            Catch ex As Exception
                sMessage = ex.Message
                retval = False

            Finally

                If retval = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                End If
            End Try

            'List 
            setDefault()
            getModelData()

            UI_VisualPath.Value = _Requested_Upload_FilePath
            UI_WEBURL.Value = _WEBURL

        End If

    End Sub
    ''' <summary>
    ''' 取得Model 的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getModelData()
        Dim i As Integer = 0
        Dim oListItem As ListItem

        Dim oWarranty As New ctlWarranty
        Dim dtModel As New WarrantyDTO.vwPrdGroupDataTable
        dtModel = oWarranty.QueryPrdGroup("", "")

        Me.UI_cboModel.Items.Clear()
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "208", ctlLanguage.eumType.Tag)
        oListItem.Value = ""
        Me.UI_cboModel.Items.Add(oListItem)

        For i = 0 To dtModel.Rows.Count - 1
            Dim tGroupNO As String
            tGroupNO = dtModel.Rows(i).Item("GroupNo").ToString()
            '依需求單 : 因為部分EOL機種已過保修期 . 麻煩請協助將RMA系統客戶申請開單畫面由Model下拉式選單移除EOL(Model) 
            'MODI BY Angel ON 202160829
            If tGroupNO <> "1200" And tGroupNO <> "0520" And tGroupNO <> "07AC" And tGroupNO <> "0711" And tGroupNO <> "93AC" And tGroupNO <> "9300" And tGroupNO <> "94AC" And tGroupNO <> "9400" And tGroupNO <> "95AC" And tGroupNO <> "9500" Then
                oListItem = New ListItem
                oListItem.Text = dtModel.Rows(i).Item("GroupName").ToString().Trim()
                oListItem.Value = dtModel.Rows(i).Item("GroupNo").ToString().Trim()
                If tGroupNO = "102X" Then
                    oListItem.Text = "1023 / 1023 Reader"
                End If
                Me.UI_cboModel.Items.Add(oListItem)
            End If

        Next

    End Sub
    Private Sub setDefault()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), "OTHER", Me.UI_cboFailure, sText)

        Me.UI_cmdFileAdd.Text = _oLanguage.getText("RMA", "211", ctlLanguage.eumType.Command)

        Me.CancelBtn.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        Me.SavedBtn.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.AddBtn.Text = _oLanguage.getText("Warranty", "039", ctlLanguage.eumType.Tag)
        Me.Add_Btn.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Me.Product_InformationLabel.Text = _oLanguage.getText("RMA2", "025", ctlLanguage.eumType.Tag)
        Me.FaultLabel.Text = _oLanguage.getText("RMA2", "091", ctlLanguage.eumType.Tag)
        Me.FaultDLab.Text = _oLanguage.getText("RMA2", "092", ctlLanguage.eumType.Tag)
        Me.UploadTitleLabel.Text = _oLanguage.getText("RMA2", "020", ctlLanguage.eumType.Tag)
        Me.SelectFileLabel.Text = _oLanguage.getText("RMA2", "021", ctlLanguage.eumType.Tag)
        Me.CUSTOMER_Txt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "018", ctlLanguage.eumType.Tag))
        Me.Problem_DescriptionLabel.Text = _oLanguage.getText("RMA2", "024", ctlLanguage.eumType.Tag)
        Me.clear_btn.Text = _oLanguage.getText("RMA2", "022", ctlLanguage.eumType.Tag)
        Me.download_btn.Text = _oLanguage.getText("RMA2", "023", ctlLanguage.eumType.Tag)

        Me.AddBtn.Text = _oLanguage.getText("RMA2", "028", ctlLanguage.eumType.Tag)
        Me.ModalPopupExtender_understand_Label.Text = _oLanguage.getText("RMA2", "029", ctlLanguage.eumType.Tag)
        Me.CustomerProductNumberLabel.Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag)
        Me.Upload_File_SizeLabel.Text = _oLanguage.getText("RMA2", "060", ctlLanguage.eumType.Tag)
        Me.Upload_File_CSV_SizeLabel.Text = _oLanguage.getText("RMA2", "061", ctlLanguage.eumType.Tag)
        Me.SelectFileLabe.Text = _oLanguage.getText("RMA2", "063", ctlLanguage.eumType.Tag)

        Me.Product_Information_Lab.Text = _oLanguage.getText("RMA2", "056", ctlLanguage.eumType.Tag)
        Me.ContextLab.Text = _oLanguage.getText("RMA2", "057", ctlLanguage.eumType.Tag)
        Me.SerialLab.Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag)
        Me.SerialNumberTxt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "059", ctlLanguage.eumType.Tag))
        Me.UploadFileLabel.Text = _oLanguage.getText("RMA2", "020", ctlLanguage.eumType.Tag)
        Me.Sample_hyperlink.Text = _oLanguage.getText("RMA2", "062", ctlLanguage.eumType.Tag)
        Me.UI_Upload_CSV.Text = _oLanguage.getText("RMA2", "060", ctlLanguage.eumType.Tag)
        Me.UI_Upload_Size.Text = _oLanguage.getText("RMA2", "061", ctlLanguage.eumType.Tag)
        'Me.Warranty_Detail_Labe.Text = _oLanguage.getText("RMA2", "026", ctlLanguage.eumType.Tag)

        Me.UI_cmdCancel.Text = _oLanguage.getText("RMA", "017", ctlLanguage.eumType.Tag)
        Me.SpecificationProblemLab.Text = _oLanguage.getText("RMA2", "016", ctlLanguage.eumType.Tag)
        Me.UI_Serial_Number.Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag)
        Me.UI_Model_Lab.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)

        Me.Add_Rma_Btn.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.ProductInformation_02_Btn.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)


        Me.Save_lblTitleMsg.Text = _oLanguage.getText("Common", "013", ctlLanguage.eumType.Tag)
        Me.Save_html_Success.Text = _oLanguage.getText("RMA2", "119", ctlLanguage.eumType.Tag)
        Me.UI_butOK.Text = _oLanguage.getText("Common", "014", ctlLanguage.eumType.Command)
    End Sub
    ''' <summary>
    ''' 上傳資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdFileAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdFileAdd.Click

        If FileUpload1.PostedFile.ContentLength > 0 Then
            Dim path As String = UpLoadFileImg()
            path = path.Substring(0, path.Length - 1)

            If File_HiddenField.Value <> "" Then
                File_HiddenField.Value = File_HiddenField.Value & "," & path
            Else
                File_HiddenField.Value = path
            End If

            If html_File_.Value <> "" Then
                html_File_.Value = html_File_.Value & "," & path
            Else
                html_File_.Value = path
            End If


            If html_File_.Value <> "" Then

                Dim context As String = ""
                Dim FileName As String() = html_File_.Value.Split(",")
                For i = 0 To FileName.Count - 1
                    If FileName(i) <> "" Then
                        context += "<a href='/object/Customer/" & FileName(i) & "'  download  >" & FileName(i) & "</a>&nbsp;&nbsp;"
                    End If

                Next
                UploadLabel.Text = context

            Else
                UploadLabel.Text = "<a href='/object/Customer/" & path & "'  download  >" & path & "</a>&nbsp;&nbsp;"
            End If

        End If
        Me.ajModalProgress.Show()
    End Sub
    Protected Sub clear_btn_Click(sender As Object, e As EventArgs) Handles clear_btn.Click

        Me.File_HiddenField.Value = ""
        Me.html_File_.Value = ""
        Me.UploadLabel.Text = ""

        Me.ajModalProgress.Show()

    End Sub
    Protected Sub download_btn_Click(sender As Object, e As EventArgs) Handles download_btn.Click
        Try
            Dim UI_WEBURL_string As String = Me.UI_WEBURL.Value.Trim()
            Dim UI_VisualPath_string As String = Me.UI_VisualPath.Value.Trim()
            Dim html_File__string As String = Me.html_File_.Value.Trim()

            Dim FileName As String() = html_File__string.Split(",")
            Dim path As String = ConfigurationSettings.AppSettings("Requested_VisualPath") + FileName(0)

            Dim UI_WEBURL_string_D As String = Me.UI_WEBURL.Value.Trim()

            Dim sScript As String = "<script type=""text/javascript"">btnDownload(""" & path & """, """ & FileName(0) & """)</script>"
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "btnDownloadMsg", sScript)

            Me.ajModalProgress.Show()

        Catch ex As Exception
            Dim context As String = ex.Message
        End Try
    End Sub
    Private Sub RMADetail_DataBind(ByVal iPageIndex As Integer)
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        dtRMADetail = Session("_dtRMADetail")
        Dim oWarranty As New ctlWarranty
        Dim dtPur As New DataTable

        '新建Table
        Dim RMADetailDataTable_ As DataTable
        RMADetailDataTable_ = New DataTable("RMADetailDataTable")


        Dim SeqID_Look_column As DataColumn = New DataColumn("SeqID_Look")
        SeqID_Look_column.DataType = System.Type.GetType("System.String")

        Dim SeqID_column As DataColumn = New DataColumn("SeqID")
        SeqID_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_ID_column As DataColumn = New DataColumn("RMAD_ID")
        RMAD_ID_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_ISFILL_column As DataColumn = New DataColumn("RMAD_ISFILL")
        RMAD_ISFILL_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_WARRANTY_column As DataColumn = New DataColumn("RMAD_WARRANTY")
        RMAD_WARRANTY_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_CSTMP_column As DataColumn = New DataColumn("RMAD_CSTMP")
        RMAD_CSTMP_column.DataType = System.Type.GetType("System.String")

        Dim CWEndWarr_column As DataColumn = New DataColumn("CWEndWarr")
        CWEndWarr_column.DataType = System.Type.GetType("System.String")


        Dim RMAD_MODELNO_column As DataColumn = New DataColumn("RMAD_MODELNO")
        RMAD_MODELNO_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_SERIALNO_column As DataColumn = New DataColumn("RMAD_SERIALNO")
        RMAD_SERIALNO_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_sWARRANTY_column As DataColumn = New DataColumn("RMAD_sWARRANTY")
        RMAD_sWARRANTY_column.DataType = System.Type.GetType("System.String")

        Dim RMAD_MARK_column As DataColumn = New DataColumn("RMAD_MARK")
        RMAD_MARK_column.DataType = System.Type.GetType("System.String")

        Dim wats_warrnstart_column As DataColumn = New DataColumn("wats_warrnstart")
        wats_warrnstart_column.DataType = System.Type.GetType("System.String")

        Dim wats_warrnend_column As DataColumn = New DataColumn("wats_warrnend")
        wats_warrnend_column.DataType = System.Type.GetType("System.String")

        Dim wati_type_column As DataColumn = New DataColumn("wati_type")
        wati_type_column.DataType = System.Type.GetType("System.String")


        Dim wati_ver_column As DataColumn = New DataColumn("wati_ver")
        wati_ver_column.DataType = System.Type.GetType("System.String")

        Dim Warranty_Month_column As DataColumn = New DataColumn("Warranty_Month")
        Warranty_Month_column.DataType = System.Type.GetType("System.String")

        Dim Extra_Month_column As DataColumn = New DataColumn("Extra_Month")
        Extra_Month_column.DataType = System.Type.GetType("System.String")

        Dim SeqSN As DataColumn = New DataColumn("SeqSN")
        SeqSN.DataType = System.Type.GetType("System.String")

        Dim SWEndWarr As DataColumn = New DataColumn("SWEndWarr")
        SWEndWarr.DataType = System.Type.GetType("System.String")

        Dim CUSTOMER_PRODUCT_NUMBER As DataColumn = New DataColumn("CUSTOMER_PRODUCT_NUMBER")
        CUSTOMER_PRODUCT_NUMBER.DataType = System.Type.GetType("System.String")

        Dim WarrantyType As DataColumn = New DataColumn("WarrantyType")
        WarrantyType.DataType = System.Type.GetType("System.String")

        Dim WarrantyDate As DataColumn = New DataColumn("WarrantyDate")
        WarrantyDate.DataType = System.Type.GetType("System.String")

        Dim Warranty_Detail As DataColumn = New DataColumn("Warranty_Detail")
        Warranty_Detail.DataType = System.Type.GetType("System.String")


        Dim Warranty_Detail_Context As DataColumn = New DataColumn("Warranty_Detail_Context")
        Warranty_Detail_Context.DataType = System.Type.GetType("System.String")

        Dim CmdEdit As DataColumn = New DataColumn("CmdEdit")
        CmdEdit.DataType = System.Type.GetType("System.String")


        RMADetailDataTable_.Columns.Add(SeqID_Look_column)
        RMADetailDataTable_.Columns.Add(SeqID_column)
        RMADetailDataTable_.Columns.Add(RMAD_ID_column)
        RMADetailDataTable_.Columns.Add(RMAD_ISFILL_column)
        RMADetailDataTable_.Columns.Add(RMAD_WARRANTY_column)
        RMADetailDataTable_.Columns.Add(RMAD_CSTMP_column)
        RMADetailDataTable_.Columns.Add(CWEndWarr_column)
        RMADetailDataTable_.Columns.Add(RMAD_MODELNO_column)
        RMADetailDataTable_.Columns.Add(RMAD_SERIALNO_column)
        RMADetailDataTable_.Columns.Add(RMAD_sWARRANTY_column)
        RMADetailDataTable_.Columns.Add(RMAD_MARK_column)

        RMADetailDataTable_.Columns.Add(wats_warrnstart_column)
        RMADetailDataTable_.Columns.Add(wats_warrnend_column)
        RMADetailDataTable_.Columns.Add(wati_type_column)
        RMADetailDataTable_.Columns.Add(wati_ver_column)
        RMADetailDataTable_.Columns.Add(Warranty_Month_column)
        RMADetailDataTable_.Columns.Add(Extra_Month_column)
        RMADetailDataTable_.Columns.Add(SeqSN)

        RMADetailDataTable_.Columns.Add(SWEndWarr)
        RMADetailDataTable_.Columns.Add(CUSTOMER_PRODUCT_NUMBER)

        RMADetailDataTable_.Columns.Add(Warranty_Detail)
        RMADetailDataTable_.Columns.Add(CmdEdit)
        RMADetailDataTable_.Columns.Add(WarrantyType)
        RMADetailDataTable_.Columns.Add(WarrantyDate)
        RMADetailDataTable_.Columns.Add(Warranty_Detail_Context)

        For i = 0 To dtRMADetail.Rows.Count - 1
            Dim RMAD_SERIALNO As String = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()

            Dim RMADetail_Row As DataRow
            RMADetail_Row = RMADetailDataTable_.NewRow()

            '產品保固資料
            'Dim context As String = ""
            'Dim ctlReport As New ctlReport
            'Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
            'dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), RMAD_SERIALNO)

            'If dtReport.Rows.Count > 0 Then
            '    RMADetail_Row("WarrantyType") = dtReport.Rows(0)("WAR_TYPE").ToString().Trim()
            '    RMADetail_Row("WarrantyDate") = CDate(dtReport.Rows(0)("EXPORT_WARRANTY_DATE")).ToString("yyyy/MM/dd")
            'End If

            'RMA2
            RMADetail_Row("Warranty_Detail") = _oLanguage.getText("RMA2", "026", ctlLanguage.eumType.Tag)
            RMADetail_Row("CmdEdit") = _oLanguage.getText("RMA2", "027", ctlLanguage.eumType.Tag)

            RMADetail_Row("SeqID") = i.ToString().Trim()

            RMADetail_Row("RMAD_ID") = dtRMADetail.Rows(i)("RMAD_ID").ToString().Trim()
            RMADetail_Row("RMAD_ISFILL") = dtRMADetail.Rows(i)("RMAD_ISFILL").ToString().Trim()
            RMADetail_Row("RMAD_WARRANTY") = dtRMADetail.Rows(i)("RMAD_WARRANTY").ToString().Trim()
            RMADetail_Row("RMAD_CSTMP") = dtRMADetail.Rows(i)("RMAD_CSTMP").ToString().Trim()

            Dim oExport As New ctlRMA.Export
            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                RMADetail_Row("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If

            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                RMADetail_Row("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If

            RMADetail_Row("RMAD_MODELNO") = dtRMADetail.Rows(i)("RMAD_MODELNO").ToString().Trim()
            RMADetail_Row("RMAD_SERIALNO") = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()
            RMADetail_Row("RMAD_sWARRANTY") = dtRMADetail.Rows(i)("RMAD_sWARRANTY").ToString().Trim()
            RMADetail_Row("RMAD_MARK") = dtRMADetail.Rows(i)("RMAD_MARK").ToString().Trim()
            RMADetail_Row("CUSTOMER_PRODUCT_NUMBER") = dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()

            Dim RMAD_MODELNO_string As String = ""

            If dtRMADetail.Rows(i)("RMAD_MODELNO").ToString().Trim() = "XXXX" Then
                RMAD_MODELNO_string = "OTHER"
            Else
                RMAD_MODELNO_string = dtRMADetail.Rows(i)("RMAD_MODELNO").ToString().Trim()
            End If


            Dim dtWarrParts As DataTable = GetData(RMAD_SERIALNO, RMAD_MODELNO_string, Session("_RepairID").ToString())

            '零件保固
            Dim Component As String = ""

            For a = 0 To dtWarrParts.Rows.Count - 1
                RMADetail_Row("wati_ver") = dtWarrParts.Rows(a)("WAP_WID").ToString().Trim()
                Component += "<tr>"
                Component += "<td>" & a + 1 & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("PODate").ToString().Trim() & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("WAP_NAME").ToString().Trim() & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("WAP_MON").ToString().Trim() & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("WAP_EMON").ToString().Trim() & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("WAP_DESC").ToString().Trim() & "</td>"
                Component += "<td>" & dtWarrParts.Rows(a)("WarrEndDate").ToString().Trim() & "</td>"
                Component += "</tr>"
            Next
            RMADetail_Row("Warranty_Detail_Context") = Component

            Dim SERIALNO As String = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()
            dtPur = oWarranty.QueryWarrantyPO(SERIALNO)
            For a = 0 To dtPur.Rows.Count - 1
                RMADetail_Row("wats_warrnstart") = Convert.ToDateTime(dtPur.Rows(a)("wats_warrnstart").ToString().Trim()).ToString("yyyy/MM/dd")
                RMADetail_Row("wats_warrnend") = Convert.ToDateTime(dtPur.Rows(a)("wats_warrnend").ToString().Trim()).ToString("yyyy/MM/dd")

                Dim wats_Month As Long
                wats_Month = DateDiff(DateInterval.Month, CDate(dtPur.Rows(a)("wats_warrnstart").ToString().Trim()), CDate(dtPur.Rows(a)("wats_warrnend").ToString().Trim()))

                Dim exta_Month As Long
                exta_Month = DateDiff(DateInterval.Month, DateTime.Now, CDate(dtPur.Rows(a)("wats_warrnend").ToString().Trim()))

                If (exta_Month < 0) Then
                    RMADetail_Row("Extra_Month") = "0"
                Else
                    RMADetail_Row("Extra_Month") = exta_Month.ToString().Trim()
                End If

                RMADetail_Row("SeqSN") = (a + 1).ToString().Trim() & "."
                RMADetail_Row("Warranty_Month") = wats_Month.ToString().Trim()
                RMADetail_Row("wati_type") = dtPur.Rows(a)("wati_type").ToString().Trim()
                'RMADetail_Row("wati_ver") = dtPur.Rows(a)("wati_ver").ToString().Trim()

            Next


            RMADetailDataTable_.Rows.Add(RMADetail_Row)

        Next

        For i = 0 To dtRMADetail.Rows.Count - 1

        Next

        If (RMADetailDataTable_.Rows.Count > 0) Then
            Call ArrangementData(RMADetailDataTable_)

            Call ArrangementData_Change(dtRMADetail)

        End If


        Dim dvRMADetail As DataView = RMADetailDataTable_.DefaultView
        dvRMADetail.RowFilter = "RMAD_MARK=0"

        'List 放入資料
        Me.UI_dvRMAListView.DataSource = dvRMADetail
        Me.UI_dvRMAListView.DataBind()

    End Sub
    Public Function GetData(ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String) As DataTable
        If OperationCenter <> "CL_CHINA" Then
            OperationCenter = "CLHQ"
        End If

        'Response.Write("RMAD_SERIALNO " + RMAD_SERIALNO + "-RMAD_MODELNO " + RMAD_MODELNO)

        Dim oExport As New ctlRMA.Export
        Dim sEWEnd As String = oExport.getMaxWarranty(RMAD_SERIALNO, Session("_CustomerID").ToString(), Session("_RepairID").ToString())
        Dim sCWEnd As String = oExport.getWarrantyCW(RMAD_SERIALNO, "")
        Dim sSWEnd As String = oExport.getWarrantySW(RMAD_SERIALNO, "")
        Dim sWarDate As String = oExport.getWarrantyStart(RMAD_SERIALNO)
        Dim sWarVersion As String = String.Empty

        Dim sWar_id As String = ""
        Dim oWarranty As New ctlWarranty
        Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(RMAD_SERIALNO)
        If dtPur.Rows.Count > 0 Then
            sWar_id = dtPur.Rows(0)("wati_ver").ToString()
            'sWarDate = DateTime.Parse(dtPur.Rows(0)("waty_date").ToString()).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
            If sCWEnd.Trim() <> "" Then
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "CW", "", "Y", "WAR_VERSION")
            Else
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "EW", "", "Y", "WAR_VERSION")
            End If

            If dtEwVer.Rows.Count > 0 Then
                If dtEwVer.Rows(0)("WAR_VERSION").ToString().Trim().Equals("0") Then
                    sWar_id = dtEwVer.Rows(0)("WAR_ID").ToString()
                End If

                '20200217 wisely modify 抓訂單細目的版本
                sWarVersion = oWarranty.QueryWARVERSION(RMAD_SERIALNO)
                If sWarVersion <> String.Empty Then
                    Dim find_rows As DataRow() = dtEwVer.Select("WAR_VERSION='" + sWarVersion + "'")
                    If find_rows.Length > 0 Then
                        sWar_id = find_rows(0)("WAR_ID").ToString()
                    End If
                End If

            End If
        End If

        If sWarDate <> "" Then
            sWarDate = DateTime.Parse(sWarDate).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            sWar_id = "123$321"
        End If

        'lblTitle2.Text = sWar_id
        'Response.Write(sWar_id)
        'Response.End

        Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        'sWar_id = "9700CW00125"
        dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")
        dtWarrParts.Columns.Add("PODate")
        dtWarrParts.Columns.Add("WarrEndDate")

        Dim i As Integer
        For i = 0 To dtWarrParts.Rows.Count - 1
            dtWarrParts.Rows(i)("PODate") = sWarDate
            If sWarDate <> "" Then
                dtWarrParts.Rows(i)("WarrEndDate") = DateTime.Parse(sWarDate).AddMonths(Double.Parse(dtWarrParts.Rows(i)("WAP_MON").ToString()) + Double.Parse(dtWarrParts.Rows(i)("WAP_EMON").ToString())).AddDays(-1).ToString("yyyy/MM/dd")
            End If
        Next

        Return dtWarrParts
    End Function
    Function GetSnData(ByVal sn_no As String) As String

        Dim context As String = ""
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
        dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), sn_no)

        If dtReport.Rows.Count > 0 Then
            context = dtReport.Rows(0)("WAR_TYPE").ToString().Trim()
        End If

        Return context

    End Function
    Private Sub ArrangementUpData(ByVal dtRMADetail As DataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then
            dtRMADetail.Columns.Add("SeqID")
            dtRMADetail.Columns.Add("CWEndWarr")
            dtRMADetail.Columns.Add("SWEndWarr")
        End If

        Dim SeqID As Integer = 0

        For i = 0 To dtRMADetail.Rows.Count - 1

            dtRMADetail.Rows(i)("SeqID") = i + 1

            Dim oExport As New ctlRMA.Export
            Dim sEWEnd As String = ""

            If UI_cboRepairCenter = "" Then
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_lblRepairCenterValue)
            Else
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_cboRepairCenter)
            End If
            If sEWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(sEWEnd).ToShortDateString()
            End If

            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If

            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If
        Next
    End Sub
    Private Sub ArrangementData(ByVal dtRMADetail As DataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then
            dtRMADetail.Columns.Add("SeqID")
            dtRMADetail.Columns.Add("CWEndWarr")
            dtRMADetail.Columns.Add("SWEndWarr")
        End If

        Dim SeqID As Integer = 0

        For i = 0 To dtRMADetail.Rows.Count - 1

            If dtRMADetail.Rows(i)("RMAD_MARK") = 0 Then

                If dtRMADetail.Rows(i)("SeqID_Look") IsNot Nothing Then
                    dtRMADetail.Rows(i)("SeqID_Look") = SeqID + 1
                    SeqID = SeqID + 1
                End If
            End If
            dtRMADetail.Rows(i)("SeqID") = i + 1

            Dim oExport As New ctlRMA.Export
            Dim sEWEnd As String = ""

            If UI_cboRepairCenter = "" Then
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_lblRepairCenterValue)
            Else
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_cboRepairCenter)
            End If
            If sEWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(sEWEnd).ToShortDateString()
            End If

            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If

            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If
        Next
    End Sub
    Private Sub ArrangementData_Change(ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then
            dtRMADetail.Columns.Add("SeqID")
            dtRMADetail.Columns.Add("CWEndWarr")
            dtRMADetail.Columns.Add("SWEndWarr")
        End If

        For i = 0 To dtRMADetail.Rows.Count - 1
            dtRMADetail.Rows(i)("SeqID") = i + 1

            Dim oExport As New ctlRMA.Export
            Dim sEWEnd As String = ""

            If UI_cboRepairCenter = "" Then
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_lblRepairCenterValue)
            Else
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), UI_cboRepairCenter)
            End If
            If sEWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(sEWEnd).ToShortDateString()
            End If

            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If

            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If
        Next
    End Sub
    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As RmaDTO.RMADetailDataTable
        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim sWarranty As String = ""
        Dim oExport As New ctlRMA.Export

        If UI_txtSerial <> "" Then
            dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + UI_txtSerial.Trim().ToUpper() + "' AND RMAD_MARK<>'1'"
            If dtRMADetail.DefaultView.Count > 0 Then
                Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                Throw New ArgumentException(sErrorMsg)
            End If
        End If

        Try
            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""

            If UI_txtSerial.Trim() <> "" Then
                Dim sModelNo As String = oExport.getModelNo(UI_txtSerial.Trim().ToUpper(), UI_lblRepairCenterValue.ToString().Trim(), UI_lblAccountIDText.ToString().Trim())
                If sModelNo.Trim() = "" Then
                    dr.RMAD_MODELNO = "OTHER"
                Else
                    dr.RMAD_MODELNO = sModelNo
                End If
            Else
                'If UI_cboModel <> "" Then
                '    dr.RMAD_MODELNO = UI_cboModel
                'End If
            End If

            'Mod by Isaac 保固日期看抓全保、一般保的最大日期
            If UI_txtSerial.ToString().Trim() <> "" Then

                Dim WarrantyDate As String = ""
                If UI_cboRepairCenter = "" Then
                    WarrantyDate = oExport.getMaxWarranty(UI_txtSerial.ToUpper(), Session("_CustomerID").ToString(), UI_lblRepairCenterValue.ToString().Trim())
                Else
                    WarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.ToUpper(), Session("_CustomerID").ToString(), UI_cboRepairCenter)
                End If

                If WarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If

                If WarrantyDate.Trim() <> "" Then
                    dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If
            End If

            dr.RMAD_SERIALNO = Me.UI_txtSerial.ToString().Trim().ToUpper()
            dr.RMAD_CUSNAME = ""                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty          'Warranty 字串格式

            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = ""
            dr.RMAD_STATUS = 0

            If File_HiddenField.Value.ToString().Trim() <> "" Then

                dr.RMAD_UPLOADFILE = File_HiddenField.Value.ToString().Trim()

            End If

            dr.RMAD_FARFARCNO = Me.UI_cboFailureClass.SelectedItem.Value
            dr.RMAD_FARNO = Me.UI_cboFailure.SelectedItem.Value
            dr.RMAD_MODELNO = Me.UI_cboModel.SelectedItem.Value

            dr.RMAD_PARTSN = UI_txtSerialParts.Text.Trim().ToUpper()
            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_PRODUCTDESC = Message_Box.Text.Trim()
            dr.RMAD_PROBLEMDESC = Message_Box.Text.Trim()
            dr.CUSTOMER_PRODUCT_NUMBER = CUSTOMER_Txt.Text.Trim()
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try
        Return dtRMADetail
    End Function
    Protected Sub AddBtn_Click(sender As Object, e As EventArgs) Handles AddBtn.Click

        If Not Request.Params("RMANO") Is Nothing Then
            Dim _Crypto As New SecurityCrypt.Crypto
            Dim RMANO As String = _Crypto.Encrypt(Request.Params("RMANO").ToString().Trim(), "")

            Response.Redirect("ProductInformation_04.aspx?RMANO=" & RMANO)
        Else
            Response.Redirect("ProductInformation_04.aspx")
        End If

    End Sub
    Protected Sub UI_dvRMAListView_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles UI_dvRMAListView.ItemCommand

        If (e.Item.ItemType = ListViewItemType.DataItem) Then

            Dim thisDataItem As ListViewDataItem = e.Item
            Dim currentDataKey As DataKey = Me.UI_dvRMAListView.DataKeys(thisDataItem.DataItemIndex)
            Dim oExport As New ctlRMA.Export

            If e.CommandName = "cmdEdit" Then

                Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                dtRMADetail = Session("_dtRMADetail")
                Dim UI_RMADID As String = currentDataKey.Value

                CurrentDataKey_HiddenField.Value = UI_RMADID

                For a = 0 To dtRMADetail.Rows.Count - 1

                    If (UI_RMADID = a + 1) Then

                        Me.UI_txtSerial_UP.Text = dtRMADetail.Rows(a)("RMAD_SERIALNO").ToString().Trim()
                        '主要原因
                        Me.UI_cboFailureClass.SelectedIndex = Me.UI_cboFailureClass.Items.IndexOf(Me.UI_cboFailureClass.Items.FindByValue(dtRMADetail.Rows(a)("RMAD_FARFARCNO").ToString().Trim()))

                        '次要原因
                        'Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
                        'oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)

                        '次要原因
                        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)

                        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), dtRMADetail.Rows(a)("RMAD_FARFARCNO").ToString().Trim(), Me.UI_cboFailure, sFarcNameText)
                        Me.UI_cboFailure.SelectedIndex = Me.UI_cboFailure.Items.IndexOf(Me.UI_cboFailure.Items.FindByValue(dtRMADetail.Rows(a)("RMAD_FARNO").ToString().Trim()))
                        '機型


                        Me.UI_cboModel.SelectedIndex = Me.UI_cboModel.Items.IndexOf(Me.UI_cboModel.Items.FindByValue(dtRMADetail.Rows(a)("RMAD_MODELNO").ToString().Trim()))

                        '上傳路徑
                        If dtRMADetail.Rows(a)("RMAD_UPLOADFILE").ToString().Trim() <> "" Then
                            File_HiddenField.Value = dtRMADetail.Rows(a)("RMAD_UPLOADFILE").ToString().Trim()
                            html_File_.Value = dtRMADetail.Rows(a)("RMAD_UPLOADFILE").ToString().Trim()

                            Dim context As String = ""
                            Dim FileName As String() = dtRMADetail.Rows(a)("RMAD_UPLOADFILE").ToString().Trim().Split(",")
                            For i = 0 To FileName.Count - 1
                                If FileName(i) <> "" Then
                                    context += "<a href='/object/Customer/" & FileName(i) & "' >" & FileName(i) & "</a>&nbsp;&nbsp;"
                                End If

                            Next

                            UploadLabel.Text = context
                        End If
                        '備註
                        Message_Box.Text = dtRMADetail.Rows(a)("RMAD_PRODUCTDESC").ToString().Trim()
                        '客戶編號
                        Me.CUSTOMER_Txt.Text = dtRMADetail.Rows(a)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()


                    End If


                Next

                Me.ajModalProgress.Show()

            End If

            If e.CommandName = "cmdWarrDetail" Then

            End If

            If e.CommandName = "cmdDel" Then

                Dim UI_RMADID As String = currentDataKey.Value

                Call Delete(UI_RMADID)
            End If

        End If
        Dim index As Integer = Me.UI_cboFailureClass.SelectedIndex

        Dim indexs As Integer = 0
    End Sub

    ''' <summary>
    ''' 品項刪除
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal sID As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView


        Select Case Convert.ToInt16(Session("_eumCommand"))
            Case eumCommand.AddNew


            Case eumCommand.UPDATE

                For i = 0 To dtRMADetail.Count - 1

                    If (i = (Convert.ToInt32(sID) - 1)) Then

                        Dim ctAddress As New ctAddress

                        ctAddress.ProductInformation_03_Delete(dtRMADetail(i).RMAD_ID.ToString().Trim())

                    End If

                Next

        End Select

        dtRMADetail.Rows.RemoveAt(Convert.ToInt32(sID) - 1)

        Session("_dtRMADetail") = dtRMADetail

        RMADetail_DataBind(1)
    End Sub

    ''' <summary>
    ''' Excel檔案==新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Add_Rma_Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_Rma_Btn.Click

        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)
        Dim chk_CW As Boolean = False

        If SerialNumberTxt.Text.Trim() = "" Then
            'EXCEL
            '批次輸入
            Dim blnFlag As Boolean
            Dim sMessage As String = ""
            Dim i As Integer = 0

            Dim dt As New DataTable
            Dim oExport As New ctlRMA.Export
            Dim dtRMADetail As New RmaDTO.RMADetailDataTable


            Try
                Dim sFullFileName As String = UpLoadFile()

                '需檢查通過 才可以下一個階段 開始
                '判斷條件
                Dim lstWriteBits As List(Of String) = New List(Of String)()
                Dim lstWriteBits_New As List(Of String) = New List(Of String)()

                If sFullFileName <> "over" Then
                    If sFullFileName <> "format" Then

                        If sFullFileName = "" Then
                            Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                        End If

                        Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                        If txtFileName_Check = "" Then


                        Else




                            Dim dir As String = _Requested_Upload_FilePath
                            Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                            Dim TextLine As String
                            '客戶設定檔案
                            Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                            Dim ctlCustomer_Customer As New ctlCustomer.Customer
                            dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                            If System.IO.File.Exists(FILE_NAME) = True Then
                                Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)

                                Do While objReader.Peek() <> -1
                                    TextLine = objReader.ReadLine()
                                    Dim arrText() As String = TextLine.Split(",")
                                    Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                    lstWriteBits.Add(sSerial)

                                Loop
                                objReader.Close()
                            End If



                        End If
                    End If
                End If

                '過濾重複序號
                lstWriteBits_New = lstWriteBits.Distinct().ToList
                Dim Check_SN_List As List(Of String) = New List(Of String)()
                Check_SN_List = lstWriteBits.Intersect(lstWriteBits_New).ToList
                '需檢查通過 才可以下一個階段 結束
                Dim chk_sn_list As String = ""

                '比較數量是否減少
                If lstWriteBits.Count = lstWriteBits_New.Count Then
                    If sFullFileName <> "over" Then
                        If sFullFileName <> "format" Then

                            If sFullFileName = "" Then
                                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                            End If

                            Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                            If txtFileName_Check = "" Then
                                Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag) & "');  </script>"
                                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                            Else

                                Dim dir As String = _Requested_Upload_FilePath

                                Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                Dim TextLine As String

                                '客戶設定檔案
                                Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                If System.IO.File.Exists(FILE_NAME) = True Then
                                    Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                                    Do While objReader.Peek() <> -1
                                        TextLine = objReader.ReadLine()
                                        Dim arrText() As String = TextLine.Split(",")

                                        If arrText.Length > 0 Then
                                            If arrText(0).ToString().Trim().ToLower <> "Serial Number".ToLower() Then

                                                Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                                chk_sn_list = chk_sn_list & sSerial & ","
                                            End If
                                        End If

                                    Loop
                                    objReader.Close()
                                End If

                            End If

                        Else


                        End If
                    Else


                    End If
                Else
                    Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA2", "124", ctlLanguage.eumType.Tag) & "</br>" & Check_SN_List(1).ToString())
                End If

                Dim ctAddress As New ctAddress
                Dim ctAddress_dt As New DataTable
                chk_sn_list = chk_sn_list.Remove(chk_sn_list.Length - 1, 1)

                Dim chk_sn_list_Split() As String = Split(chk_sn_list, ",")
                Dim index_ As Integer = 0

                For i = 0 To chk_sn_list_Split.Length - 1

                    ctAddress_dt = ctAddress.chk_CW(chk_sn_list_Split(i).ToString().Trim())
                    If ctAddress_dt.Rows.Count > 0 Then
                        index_ = index_ + 1
                    Else

                    End If
                Next

                If index_ = chk_sn_list_Split.Length Then
                    chk_CW = True
                End If


            Catch ex As Exception

            Finally

            End Try
        Else
            '輸入框
            Dim ctAddress As New ctAddress
            Dim ctAddress_dt As New DataTable
            ctAddress_dt = ctAddress.chk_CW(SerialNumberTxt.Text.Trim())

            If ctAddress_dt.Rows.Count > 0 Then

                '批次輸入
                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim i As Integer = 0

                Dim dt As New DataTable
                Dim oExport As New ctlRMA.Export
                Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                chk_CW = True
            Else

            End If

        End If

        '只要不是終端客戶 都是不需要檢查CW
        If 1 = 1 Then
            Dim oRequested As New ctlRMA.Requested
            Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
            If (dt.Rows.Count > 0) Then
                chk_CW = True
            Else
                chk_CW = True
            End If
        End If

        If chk_CW = True Then
            '檢查通過
            Dim Check_SN_List As List(Of String) = New List(Of String)()

            Dim Check As Boolean = False
            Dim check_only_one As Boolean = False
            If SerialNumberTxt.Text.Trim() <> "" Then

                '檢查重複

                If 1 = 1 Then
                    Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                    dtRMADetail = Session("_dtRMADetail")

                    Dim lstWriteBits As List(Of String) = New List(Of String)()
                    Dim lstWriteBits_New As List(Of String) = New List(Of String)()

                    lstWriteBits.Add(SerialNumberTxt.Text.Trim())

                    For a = 0 To dtRMADetail.Rows.Count - 1
                        lstWriteBits.Add(dtRMADetail.Rows(a)("RMAD_SERIALNO").ToString().Trim())
                    Next

                    '過濾重複序號
                    lstWriteBits_New = lstWriteBits.Distinct().ToList
                    Check_SN_List = lstWriteBits.Intersect(lstWriteBits_New).ToList

                    If lstWriteBits.Count = lstWriteBits_New.Count Then
                        check_only_one = True
                    End If

                End If

                If check_only_one = True Then
                    Dim Requestedes As New ctlRMA.Requested
                    Dim dt As New DataTable
                    dt = Requestedes.QueryByRMA_SERIALNO(SerialNumberTxt.Text.Trim())
                    Dim index As Integer = Convert.ToInt32(dt.Rows(0)(0).ToString().Trim())
                    '24小時序號 關閉
                    index = 0
                    If index <= 0 Then

                        '逐筆輸入
                        Me.UI_txtSerial_UP.Text = Me.SerialNumberTxt.Text.Trim()
                        Me.UI_cboFailureClass.SelectedIndex = 0
                        Me.UI_cboFailure.SelectedIndex = 0
                        Me.UI_cboModel.SelectedIndex = 0
                        Me.File_HiddenField.Value = ""
                        Me.html_File_.Value = ""
                        Me.UploadLabel.Text = ""
                        Me.Message_Box.Text = ""
                        Me.CUSTOMER_Txt.Text = ""

                        '自動帶入型號 開始
                        Dim oExport As New ctlRMA.Export

                        Dim sModelNo As String = oExport.getModelNo(Me.SerialNumberTxt.Text.Trim().ToUpper())
                        If sModelNo.Trim() = "" Then
                            Me.UI_cboModel.SelectedIndex = 1


                        Else
                            Me.UI_cboModel.SelectedIndex = 1
                            For i = 0 To Me.UI_cboModel.Items.Count - 1
                                If Me.UI_cboModel.Items(i).Value.ToLower().Trim() = sModelNo.ToLower().Trim() Then
                                    Me.UI_cboModel.SelectedValue = sModelNo
                                    Exit For
                                End If
                            Next
                        End If

                        Me.ModalPopupExtender1.Hide()
                        Me.ajModalProgress.Show()
                        Check = True
                    Else

                        Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "113", ctlLanguage.eumType.Tag).Replace("XXXXXXXXXXXXX", SerialNumberTxt.Text.Trim()) & "');  </script>"
                        Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)
                        Check = False

                    End If

                Else

                End If

            Else
                '批次輸入

                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim i As Integer = 0

                Dim dt As New DataTable
                Dim oExport As New ctlRMA.Export
                Dim dtRMADetail As New RmaDTO.RMADetailDataTable

                Try
                    Dim sFullFileName As String = UpLoadFile()

                    Dim ck_sn_one_only As Boolean = False
                    '需檢查通過 才可以下一個階段 開始
                    '判斷條件
                    Dim lstWriteBits As List(Of String) = New List(Of String)()
                    Dim lstWriteBits_New As List(Of String) = New List(Of String)()

                    If sFullFileName <> "over" Then
                        If sFullFileName <> "format" Then

                            If sFullFileName = "" Then
                                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                            End If

                            Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                            If txtFileName_Check = "" Then


                            Else




                                Dim dir As String = _Requested_Upload_FilePath
                                Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                Dim TextLine As String
                                '客戶設定檔案
                                Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                If System.IO.File.Exists(FILE_NAME) = True Then
                                    Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                                    Dim index_open As Integer = 0
                                    Do While objReader.Peek() <> -1
                                        TextLine = objReader.ReadLine()
                                        Dim arrText() As String = TextLine.Split(",")
                                        Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No

                                        If index_open = 0 Then

                                        Else
                                            lstWriteBits.Add(sSerial)
                                        End If

                                        index_open = index_open + 1


                                    Loop
                                    objReader.Close()
                                End If



                            End If
                        End If
                    End If

                    Dim dtRMADetail_ As New RmaDTO.RMADetailDataTable
                    dtRMADetail_ = Session("_dtRMADetail")
                    For a = 0 To dtRMADetail_.Rows.Count - 1
                        lstWriteBits.Add(dtRMADetail_.Rows(a)("RMAD_SERIALNO").ToString().Trim())
                    Next

                    '過濾重複序號
                    lstWriteBits_New = lstWriteBits.Distinct().ToList
                    Check_SN_List = lstWriteBits.Intersect(lstWriteBits_New).ToList
                    If lstWriteBits.Count = lstWriteBits_New.Count Then
                        ck_sn_one_only = True
                        check_only_one = True
                    End If

                    '需檢查通過 才可以下一個階段 結束

                    '比較數量是否減少
                    If ck_sn_one_only Then

                        If sFullFileName <> "over" Then
                            If sFullFileName <> "format" Then

                                If sFullFileName = "" Then
                                    Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                                End If

                                Dim dir As String = _Requested_Upload_FilePath

                                Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                Dim TextLine As String

                                '客戶設定檔案
                                Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                If System.IO.File.Exists(FILE_NAME) = True Then
                                    Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                                    Do While objReader.Peek() <> -1
                                        TextLine = objReader.ReadLine()
                                        Dim arrText() As String = TextLine.Split(",")

                                        If arrText.Length > 0 Then
                                            If arrText(0).ToString().Trim().ToLower <> "Serial Number".ToLower() Then
                                                Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                                Dim sModel As String = arrText(1).ToString().Trim().ToUpper()            'Model No
                                                'Dim sProductDesc As String = arrText(2).ToString().Trim()      '產品敘述
                                                'Dim sCusName As String = arrText(3).ToString().Trim()         'Customer Product Name
                                                'Dim sFarFarcNo As String = arrText(4).ToString().Trim()       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
                                                'Dim sFarNo As String = arrText(5).ToString().Trim()           '關聯 FailureReasons.FAR_NO-->不良原因代碼
                                                'Dim sProblemDesc As String = arrText(6).ToString().Trim()     'Problem Description
                                                'Dim CUSTOMER_PRODUCT_NUMBER As String = arrText(7).ToString().Trim()

                                                '20240411  會儲存但不呈現 下依序-2-->excel sample中可否直接拉掉那兩欄，不然怕客戶填錯 開始
                                                Dim sProductDesc As String = ""      '產品敘述
                                                Dim sCusName As String = ""         'Customer Product Name
                                                Dim sFarFarcNo As String = arrText(2).ToString().Trim()       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
                                                Dim sFarNo As String = arrText(3).ToString().Trim()           '關聯 FailureReasons.FAR_NO-->不良原因代碼
                                                Dim sProblemDesc As String = arrText(4).ToString().Trim()     'Problem Description
                                                Dim CUSTOMER_PRODUCT_NUMBER As String = arrText(5).ToString().Trim()
                                                '20240411  會儲存但不呈現-->excel sample中可否直接拉掉那兩欄，不然怕客戶填錯 結束


                                                Dim sWarranty As String = ""        '保固日期
                                                Dim sPartSn As String = ""        '零件序號

                                                Dim sQuerySN As String = ""
                                                Dim ctlReport As New ctlReport
                                                Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable

                                                dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), sSerial)

                                                If dtReport.Rows.Count > 0 Then
                                                    sQuerySN = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
                                                End If

                                                If sQuerySN <> "" Then
                                                    If sQuerySN <> sSerial Then
                                                        sPartSn = sSerial '輸入的零件序號
                                                        sSerial = sQuerySN '查詢得到主機序號
                                                    End If
                                                End If


                                                '檢核 Serial No 是否有重複
                                                If sSerial.Trim() <> "" Then
                                                    dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + sSerial.Trim() + "' AND RMAD_MARK<>'1'"
                                                    If dtRMADetail.DefaultView.Count > 0 Then
                                                        Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                                                        Throw New ArgumentException(sErrorMsg)
                                                        'Continue Do
                                                    End If
                                                Else
                                                    Dim sErrorMsg As String = _oLanguage.getText("RMA", "108", ctlLanguage.eumType.Validator)
                                                    Throw New ArgumentException(sErrorMsg)
                                                End If



                                                'Warranty
                                                If sSerial.ToString().Trim() <> "" Then
                                                    'Dim sWarrantyDate As String = oExport.getWarranty(sSerial, Session("_CustomerID").ToString())
                                                    Dim sWarrantyDate As String = ""
                                                    If dtvwCustomer.Rows(0)("CU_ISCHOICE").ToString().Trim() = "1" Then
                                                        sWarrantyDate = oExport.getMaxWarranty(sSerial.ToUpper(), Session("_CustomerID").ToString(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim())
                                                    Else
                                                        sWarrantyDate = oExport.getMaxWarranty(sSerial.ToUpper(), Session("_CustomerID").ToString(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim())
                                                    End If

                                                    If sWarrantyDate.Trim() <> "" Then
                                                        sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                                                    End If
                                                Else
                                                    sWarranty = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                                                End If

                                                'ModelNo
                                                If sSerial.Trim() <> "" Then
                                                    'Dim sModelNo As String = oExport.getModelNo(sSerial.Trim())
                                                    Dim sModelNo As String = oExport.getModelNo(sSerial.Trim(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim(), Session("_CustomerID").ToString())
                                                    If sModelNo.Trim() = "" Then
                                                        sModel = "OTHER"
                                                    Else
                                                        sModel = sModelNo
                                                    End If
                                                Else
                                                    If sModel.Trim() = "" Then
                                                        sModel = "OTHER"
                                                    Else
                                                        Dim oModel As New ctlRMA.Model
                                                        If oModel.chkModel(sModel.Trim()) = False Then
                                                            sModel = "OTHER"
                                                        End If
                                                    End If
                                                End If

                                                'FarFarcNO
                                                If sFarFarcNo.Trim() = "" Then
                                                    sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                    sFarNo = -1

                                                Else
                                                    Dim oFailure As New ctlFailure

                                                    If sFarFarcNo.Trim() <> "" And sFarNo.Trim() <> "" Then
                                                        If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim(), sFarNo.Trim()) = False Then
                                                            sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                            sFarNo = -1
                                                        End If
                                                    Else

                                                        If sFarFarcNo.Trim() <> "" And sFarNo.Trim() = "" Then
                                                            If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim()) = False Then
                                                                sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                                sFarNo = -1
                                                            End If
                                                        End If
                                                    End If

                                                End If

                                                If IsNothing(Session("_dtRMADetail")) = True Then
                                                    dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn, CUSTOMER_PRODUCT_NUMBER)
                                                Else
                                                    dtRMADetail = Session("_dtRMADetail")
                                                    dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn, CUSTOMER_PRODUCT_NUMBER)
                                                End If

                                            End If
                                        End If
                                    Loop
                                    objReader.Close()
                                End If

                                If dtRMADetail.Rows.Count > 0 Then

                                    Session("_dtRMADetail") = dtRMADetail

                                End If


                                blnFlag = True
                                Call RMADetail_DataBind(1)
                                '直接新增維修單明細
                                'Response.Redirect("ProductInformation_03.aspx")
                                Check = True

                            Else
                                Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "118", ctlLanguage.eumType.Tag) & "');  </script>"
                                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                            End If
                        Else
                            Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "118", ctlLanguage.eumType.Tag) & "');  </script>"
                            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                        End If
                    Else
                        'Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA2", "124", ctlLanguage.eumType.Tag))
                    End If

                Catch ex As Exception
                    sMessage = ex.Message
                    blnFlag = False
                    Check = False
                Finally
                    If blnFlag = False Then
                        'Dim sScript As String = "<script type=""text/javascript"">alert(""" & sMessage & """)</script>"
                        'Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)
                    End If
                End Try

            End If

            If Check = True And check_only_one = True Then

                '新增並且整理
                Dim dtRMADetail_load As New RmaDTO.RMADetailDataTable

                If IsNothing(Session("_dtRMADetail")) = True Then
                    dtRMADetail_load = AddRMADetail(dtRMADetail_load)
                Else
                    dtRMADetail_load = Session("_dtRMADetail")
                End If

                Session("_dtRMADetail") = dtRMADetail_load
                Dim iPageIndex As Integer = dtRMADetail_load.Rows.Count
                Call RMADetail_DataBind(iPageIndex)
            Else

                If Check_SN_List.Count > 0 Then

                    Me.ucMessageOpen.showMessageByFailed(_oLanguage.getText("RMA2", "124", ctlLanguage.eumType.Tag) & "</br>" & Check_SN_List(0).ToString())
                    Me.ModalPopupExtender1.Show()

                End If

            End If

        Else
            Me.ucMessageOpen.showMessageByFailed(_oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator))
            Me.ModalPopupExtender1.Show()
            Me.SerialNumberTxt.Text = ""
        End If
    End Sub
    Private Function UpLoadFile() As String
        Dim retval As String = ""

        Try
            Dim txtFileName As String = Me.html_FileUpload.FileName

            '****** 取得檔名 **********
            Dim FileSplit() As String = Split(txtFileName, "\")
            Dim FileName As String = FileSplit(FileSplit.Length - 1)

            '****** 取得副檔名 **********
            Dim auxFileSplit() As String = Split(FileName, ".")
            Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
            Dim sFileNameChange As String = ""
            Dim oCommon As New Common
            sFileNameChange = oCommon.GetRandomizeNum()
            sFileNameChange = sFileNameChange & "." & auxFileName

            '***** 檔案(原檔名,亂數檔名) *****
            Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

            Me.html_FileUpload.SaveAs(_Requested_Upload_FilePath & sFileNameChange)
            'Me.html_FileUpload.MoveTo(_Requested_Upload_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
            retval = sFileNameChange

        Catch ex As Exception
            Throw ex
            retval = ""

        End Try

        Return retval
    End Function
    Private Function UpLoadFileImg() As String
        Dim retval As String = ""

        Try

            For i = 0 To Request.Files.Count - 1
                Dim postedFile As HttpPostedFile = Request.Files(i)

                If postedFile.ContentLength > 0 Then

                    Dim txtFileName As String = Me.FileUpload1.FileName

                    '****** 取得檔名 **********
                    Dim FileSplit() As String = Split(txtFileName, "\")
                    Dim FileName As String = FileSplit(FileSplit.Length - 1)

                    '****** 取得副檔名 **********
                    Dim auxFileSplit() As String = Split(FileName, ".")
                    Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
                    Dim sFileNameChange As String = ""
                    Dim oCommon As New Common
                    sFileNameChange = oCommon.GetRandomizeNum()
                    sFileNameChange = sFileNameChange & "." & auxFileName

                    '***** 檔案(原檔名,亂數檔名) *****
                    Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim


                    postedFile.SaveAs(Server.MapPath(Requested_VisualPath) & sFileNameChange)
                    'Me.html_FileUpload.MoveTo(_Requested_Upload_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
                    retval += sFileNameChange & ","
                End If


            Next

        Catch ex As Exception
            Throw ex
            retval = ""

        End Try

        Return retval
    End Function
    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <param name="sSerial"></param>
    ''' <param name="sModel"></param>
    ''' <param name="sCusName"></param>
    ''' <param name="sWarranty"></param>
    ''' <param name="sFarFarcNo"></param>
    ''' <param name="sFarNo"></param>
    ''' <param name="sProblemDesc"></param>
    ''' <param name="sProductDesc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Execl_AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable, ByVal sSerial As String, ByVal sModel As String, ByVal sCusName As String, ByVal sWarranty As String,
                                       ByVal sFarFarcNo As String, ByVal sFarNo As String, ByVal sProblemDesc As String, ByVal sProductDesc As String, ByVal sPartSN As String, ByVal CUSTOMER_PRODUCT_NUMBER As String) As RmaDTO.RMADetailDataTable

        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim oExport As New ctlRMA.Export

        Try
            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""
            dr.RMAD_MODELNO = sModel.Trim()
            dr.RMAD_SERIALNO = sSerial.Trim()
            dr.RMAD_CUSNAME = sCusName.Trim()                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty                       'Warranty 字串格式

            dr.RMAD_FARFARCNO = sFarFarcNo.Trim()
            dr.RMAD_FARNO = sFarNo.Trim()
            dr.RMAD_PARTSN = sPartSN.Trim()
            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = sProblemDesc.Trim()
            dr.RMAD_PROBLEMDESC = sProblemDesc.Trim()
            dr.RMAD_STATUS = 0

            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            If CUSTOMER_PRODUCT_NUMBER.Length > 20 Then
                dr.CUSTOMER_PRODUCT_NUMBER = CUSTOMER_PRODUCT_NUMBER.Trim().Substring(0, 20)
            Else
                dr.CUSTOMER_PRODUCT_NUMBER = CUSTOMER_PRODUCT_NUMBER.Trim()
            End If
            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMADetail
    End Function

#Region “已存入快取的資料,點選後帶入顯示區域”

    ''' <summary>
    ''' 新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ProductInformation_02_Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProductInformation_02_Btn.Click
        If html_File_.Value.Length < 125 Then
            '執行動作
            If CurrentDataKey_HiddenField.Value = "" Then
                '新增 開始
                Try
                    '控制1-5的資料
                    'Session("_dtRMADetail") = Nothing

                    Dim oExport As New ctlRMA.Export
                    Dim oRequested As New ctlRMA.Requested
                    Dim ctlReport As New ctlReport
                    Dim sMessage As String = ""
                    Dim retval As Boolean = True


                    '檢查序號是否有填寫
                    If UI_txtSerial_UP.Text = "" Then
                        Dim sErrorMsg As String = _oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator) 'Wait
                        Throw New ArgumentException(sErrorMsg)
                    End If

                    '判斷是否為零件序號
                    UI_txtSerialParts.Text = ""
                    Dim sProductSn As String = ""

                    Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
                    'dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.UI_txtSerial.Text.Trim().ToUpper())
                    dtReport = ctlReport.QueryRMAWarranty("", Me.UI_txtSerial_UP.Text.Trim().ToUpper())
                    If dtReport.Rows.Count > 0 Then
                        sProductSn = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
                    End If
                    'Throw New ArgumentException(sProductSn)
                    If sProductSn <> "" Then
                        If sProductSn <> Me.UI_txtSerial_UP.Text.Trim().ToUpper() Then
                            UI_txtSerialParts.Text = Me.UI_txtSerial_UP.Text.Trim().ToUpper()
                            Me.UI_txtSerial_UP.Text = sProductSn
                        Else
                            sProductSn = ""
                        End If
                    End If

                    '20180424 Isaac Add EndUser只能送修全保
                    'Dim bool_EndUser As Boolean = oRequested.IsEndUser(Session("_CustomerID").ToString().Trim())
                    Dim dt As DataTable = oRequested.IsEndUser(Session("_CustomerID").ToString().Trim())
                    'Dim dt As DataTable = oRequested.IsEndUser(UI_lblUserIDText.Text.ToString().Trim())
                    'If bool_EndUser Then
                    'If (dt.Rows.Count > 0) Then
                    '20220303 wisely Enduser 不可以送修零件
                    'If UI_txtSerialParts.Text.ToString() <> "" Then
                    'Me.UI_txtSerial_UP.Text = UI_txtSerialParts.Text
                    'Dim sErrorMsg As String = _oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator) 'Wait
                    'Throw New ArgumentException(sErrorMsg)
                    'End If
                    'End If

                    UI_txtSerial = UI_txtSerial_UP.Text.Trim()
                    Dim dtRMADetail As New RmaDTO.RMADetailDataTable

                    If IsNothing(Session("_dtRMADetail")) = True Then
                        dtRMADetail = AddRMADetail(dtRMADetail)
                    Else
                        dtRMADetail = Session("_dtRMADetail")
                        dtRMADetail = AddRMADetail(dtRMADetail)
                    End If

                    Session("_dtRMADetail") = dtRMADetail

                    Call RMADetail_DataBind(1)

                    Me.UI_txtSerial = ""
                    Me.SerialNumberTxt.Text = ""

                    Call RMADetail_DataBind(1)
                    '            Dim sScript As String = ""
                    '   sScript = "<script type=""text/javascript"">   window.location = 'ProductInformation_03.aspx'; </script>"
                    'Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                Catch ex As Exception


                Finally

                End Try
                '新增 結束
            Else
                '修改 開始
                Try

                    Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                    dtRMADetail = Session("_dtRMADetail")
                    Dim UI_RMADID As String = CurrentDataKey_HiddenField.Value

                    For a = 0 To dtRMADetail.Rows.Count - 1

                        If (UI_RMADID = a + 1) Then
                            dtRMADetail.Rows(a)("RMAD_SERIALNO") = Me.UI_txtSerial_UP.Text
                            '主要原因
                            dtRMADetail.Rows(a)("RMAD_FARFARCNO") = Me.UI_cboFailureClass.SelectedItem.Value
                            '次要原因
                            dtRMADetail.Rows(a)("RMAD_FARNO") = Me.UI_cboFailure.SelectedItem.Value
                            '機型
                            dtRMADetail.Rows(a)("RMAD_MODELNO") = Me.UI_cboModel.SelectedItem.Value
                            '上傳路徑
                            If File_HiddenField.Value.ToString().Trim() <> "" Then
                                dtRMADetail.Rows(a)("RMAD_UPLOADFILE") = File_HiddenField.Value.ToString().Trim()
                            End If
                            '備註
                            dtRMADetail.Rows(a)("RMAD_PRODUCTDESC") = Message_Box.Text.Trim()
                            '客戶
                            dtRMADetail.Rows(a)("CUSTOMER_PRODUCT_NUMBER") = CUSTOMER_Txt.Text.Trim()

                        End If
                    Next

                    Session("_dtRMADetail") = dtRMADetail
                    '快取序號清空
                    CurrentDataKey_HiddenField.Value = ""

                Catch ex As Exception

                Finally

                End Try
                '修改 結束
            End If
        Else

            Dim msg As String = _oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag)
            If Session("_LanguageID") = "002" Then

                Me.ucMessage.showMessageByFailed(msg & "數量" & "太多")
            Else
                Me.ucMessage.showMessageByFailed(msg & " Qty " & "too much")

            End If
            CurrentDataKey_HiddenField.Value = ""

        End If
    End Sub

    Protected Sub UI_cboFailureClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFailureClass.SelectedIndexChanged
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)
        Me.ajModalProgress.Show()
    End Sub

#End Region

    '取消RMA單
    Protected Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.ModalPopupExtender_understand.Show()
    End Sub

    Protected Sub UI_butOK_Click(sender As Object, e As EventArgs) Handles UI_butOK.Click

        Dim blnFlag As Boolean = False
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        out_for_url = "0"
        If chkRMA() = True Then
            Dim iStatus As Integer = 0      '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Save(iStatus)
        End If

    End Sub
    Protected Sub SaveImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles SaveImageBtn.Click



    End Sub

    ''' <summary>
    ''' 儲存之前的檢核 -- 品項
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chkRMA() As Boolean
        Dim retval As Boolean = True
        Dim sMessage As String = ""

        Try

            If Not Session("_dtRMADetail") Is Nothing Then
                Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
                Dim dvRMADetail As DataView = dtRMADetail.DefaultView
                dvRMADetail.RowFilter = "RMAD_MARK=0"
                If dvRMADetail.Count = 0 Then
                    sMessage = _oLanguage.getText("RMA", "194", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If

                '是否已填寫問題:0.否, 1.是
                dvRMADetail.RowFilter = "RMAD_ISFILL=0 and RMAD_MARK=0"
                If dvRMADetail.Count > 0 Then
                    sMessage = _oLanguage.getText("RMA", "195", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If
            End If

        Catch ex As Exception
            sMessage = ex.Message
            retval = False

        Finally

            If retval = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try

        Return retval
    End Function

    Private Sub Save(ByVal RMA_Status As Integer)
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim sRMAID As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oAdmin As New ctlAdmin

        '填資料
        Dim UI_lblRepairCenterValue_Text As String = ""
        Dim UI_PartsRequest As Boolean
        Try

            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    'RMA Table
                    dtRMA = Save_RMA(RMA_Status, False)
                Case eumCommand.UPDATE
                    dtRMA = Save_RMA(RMA_Status, True)
            End Select

            'RMADetail Table
            dtRMADetail = Save_RMADetail(RMA_Status)

            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    sRMAID = oRequested.SaveByAddNew(dtRMA, dtRMADetail)

                Case eumCommand.UPDATE

                    oRequested.SaveByEdit(HiddenField_RMANO.Value, dtRMA, dtRMADetail)

                    sRMAID = Me.UI_lblPreviousPage_RMAID.Text.Trim()
            End Select
            blnFlag = True

            '客戶編號
            Dim myctAddress As New ctAddress
            For i = 0 To dtRMADetail.Rows.Count - 1
                Dim RMAD_RMANO_string As String = sRMAID
                Dim RMAD_SERIALNO_string As String = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()
                Dim CUSTOMER_PRODUCT_NUMBER_string As String = ""
                If dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER") Is Nothing Then
                    CUSTOMER_PRODUCT_NUMBER_string = ""
                Else
                    CUSTOMER_PRODUCT_NUMBER_string = dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
                End If
                Try
                    myctAddress.Up_RMADETAIL_New(RMAD_RMANO_string, RMAD_SERIALNO_string, CUSTOMER_PRODUCT_NUMBER_string)
                Catch ex As Exception

                End Try

            Next

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If RMA_Status = 10 Then
                'addLog(DirectCast(dtRMA.Rows(0), DataService.RmaDTO.RMARow).RMA_NO, "User : " + Session("_UserID") + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
            Else
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Save ReqNew: " & sRMAID.Trim())
            End If

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)

            Else

                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " STATUS: " & RMA_Status)
                If RMA_Status = 10 Then
                    Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(sRMAID)
                    Dim _Crypto As New SecurityCrypt.Crypto
                    Dim sParm_01 As String = _Crypto.Encrypt(sRMAID.Trim, "")
                    Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
                    Dim sRedirectURL As String = "ProductInformation_05.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02

                    '20210721 wisely add NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
                    If (UI_lblRepairCenterValue_Text = "NZ_PB_TECH" OrElse UI_lblRepairCenterValue_Text = "AU_LAPTOP_KINGS") Then
                        bool_ISCW = False
                    End If

                    oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " endMail")
                    '客人送修, 按Submit時, 判斷若其中有一筆為全保, RMADETAIL.RMAD_ISCW = 1
                    '則pop window 顯示警示文字

                    If bool_ISCW = True And UI_PartsRequest = False Then
                        Me.ucMessage.showMessageBySuccess("<font color='red'>" & _oLanguage.getText("RMA", "411", ctlLanguage.eumType.Tag) & "</font>", ascx_ucMessage.eumTransferURL.Redirect, sRedirectURL)
                    Else
                        Response.Redirect(sRedirectURL)
                    End If
                Else

                    If out_for_url = "0" Then

                        ClientScript.RegisterStartupScript(GetType(String), "ShowInfoPage", "<script>  window.parent.location = 'Client_FlowCase01_Worklist.aspx';</script>")


                    Else

                        ClientScript.RegisterStartupScript(GetType(String), "ShowInfoPage", "<script>window.parent.Reload_windows();window.parent.Location_Reload_windows();</script>")

                    End If


                End If

            End If


        End Try



        If blnFlag = False Then
            Dim sMsg As String = "Repeat sent"
            'Me.ucMessage.showMessageByAlert(sMsg)
        Else


        End If



    End Sub

    Private Function Save_RMA(ByVal RMA_Status As Integer, ByVal bool_ISCW As Boolean) As RmaDTO.RMADataTable
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oGuid As Guid = Guid.NewGuid

        '填資料
        Dim UI_lblPreviousPage_RMAID_Text As String = ""
        Dim UI_lblPreviousPage_RMANO_Text As String = ""

        Dim UI_lblAccountIDText_Text As String = ""
        Dim UI_lblAccountNameText_Text As String = ""
        Dim UI_lblUserIDText_Text As String = ""
        Dim UI_txtApplicant_Text As String = ""
        Dim UI_txtTel_Text As String = ""
        Dim UI_txtCode_Text As String = ""
        Dim UI_lblRepairCenterText_Text As String = ""
        Dim UI_lblRepairCenterValue_Text As String = ""

        Dim UI_txtMail_Text As String = ""
        Dim UI_txtRemark_Text As String = ""
        Dim UI_cboRepairCenter As String = ""   '維修中心

        Dim UI_lblRepairCenterValue As String = ""   '替代維修中心
        Dim add As String = ""               '地址
        Dim UI_cboRepairCenter_ As Boolean  '有沒有維修中心

        Try

            '取資料
            Dim oCustomer As New ctlCustomer.Customer
            Dim oCustomerUser As New ctlCustomer.CustomerUser
            Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
            Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

            'oCustomer
            If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then

            Else
                dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
            End If

            Dim sTel As String = ""
            If dtCustome.Count > 0 Then
                Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
                UI_lblAccountIDText_Text = dr.CU_NO.ToString().Trim()
                UI_lblAccountNameText_Text = dr.CU_NAME.ToString().Trim()
                If dr.IsCU_CONTACTPERSONNull = False Then UI_txtApplicant_Text = dr.CU_CONTACTPERSON.ToString().Trim()
                UI_lblRepairCenterText_Text = dr.COMP_NAME.ToString().Trim()
                UI_lblRepairCenterValue_Text = dr.COMP_NO.ToString().Trim()
                Me.UI_lblRepairCenterValue_View.Value = dr.COMP_NO.ToString().Trim()
                UI_cboRepairCenter = dr.COMP_NO.ToString().Trim()
                UI_lblUserIDText_Text = Session("_UserID").ToString().Trim()

                '未知電話 地址 不可為空值
                UI_txtTel_Text = "unfilled"
                add = "unfilled"

                If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                    UI_cboRepairCenter_ = True
                Else
                    If dr.CU_ISCHOICE = "1" Then
                        UI_cboRepairCenter_ = True
                    Else
                        UI_cboRepairCenter_ = True
                    End If
                End If

                If dr.IsCU_TELNull = False Then sTel = dr.CU_TEL.ToString().Trim()
                If dr.IsCU_CONTACTPERSONNull = False Then UI_cboRepairCenter = dr.CU_CONTACTPERSON.ToString().Trim()
                If dr.IsCU_EMAILNull = False Then UI_txtMail_Text = dr.CU_EMAIL.ToString().Trim()

                '20211223 wisely 若為Enduer 不允許輸入EndUser 欄位
                Dim oRequested As New ctlRMA.Requested
                Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
                If (dt.Rows.Count > 0) Then

                Else

                End If

            End If


        Catch ex As Exception
            Throw ex

        End Try

        Try

            Dim dr As RmaDTO.RMARow = dtRMA.NewRMARow
            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    dr.RMA_ID = oGuid.ToString
                    dr.RMA_NO = ""

                Case eumCommand.UPDATE
                    dr.RMA_ID = UI_lblPreviousPage_RMAID_Text.Trim()
                    dr.RMA_NO = UI_lblPreviousPage_RMANO_Text.Trim()
            End Select

            dr.RMA_CUNO = UI_lblAccountIDText_Text.ToString().Trim()
            dr.RMA_ACCOUNTID = UI_lblUserIDText_Text.ToString().Trim()
            dr.RMA_APPLICANT = UI_txtApplicant_Text.ToString().Trim()
            dr.RMA_TEL = UI_txtTel_Text.ToString().Trim()
            dr.RMA_ADDRESS = add

            If UI_cboRepairCenter_ = False Then
                dr.RMA_COMPNO = UI_lblRepairCenterValue.ToString().Trim()
            Else
                dr.RMA_COMPNO = UI_cboRepairCenter
            End If
            '維修中心產生單號有關係
            dr.RMA_COMPNO = Me.UI_lblRepairCenterValue_View.Value

            dr.RMA_STATUS = RMA_Status
            dr.RMA_MAIL = UI_txtMail_Text
            dr.RMA_Remark = UI_txtRemark_Text

            dr.RMA_AD = Session("_UserID")
            dr.RMA_ADNAME = Session("_UserName")
            dr.RMA_CSTMP = Date.Now
            dr.RMA_LUAD = Session("_UserID")
            dr.RMA_LUADNAME = Session("_UserName")
            dr.RMA_LUSTMP = Date.Now
            dr.RMA_MARK = 0
            '未知
            dr.RMA_PARTSREQUEST = 0

            If bool_ISCW Then
                'End User 
                dr.RMA_EUCOMPANY = ""
                dr.RMA_EUTEL = ""
                dr.RMA_EUNAME = ""
                dr.RMA_EUMAIL = ""
                dr.RMA_EUADDRESS = ""
            Else
                dr.RMA_EUCOMPANY = ""
                dr.RMA_EUTEL = ""
                dr.RMA_EUNAME = ""
                dr.RMA_EUMAIL = ""
                dr.RMA_EUADDRESS = ""
            End If

            dtRMA.AddRMARow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMA
    End Function

    Private Function Save_RMADetail(ByVal RMA_Status As Integer) As RmaDTO.RMADetailDataTable
        Dim i As Integer = 0
        Dim dtTMP As New RmaDTO.RMADetailDataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oExport As New ctlRMA.Export

        Dim UI_cboRepairCenter As Boolean  '有沒有維修中心

        dtTMP = Session("_dtRMADetail")
        If dtTMP.Rows.Count = 0 Then

        Else

            Try
                Dim TXT As String = ""
                dtTMP = Session("_dtRMADetail")
                Dim SNTemp As String = ""
                For i = 0 To dtTMP.Rows.Count - 1
                    Dim UI_UI_SERIALNO As String = dtTMP.Rows(i)("RMAD_SERIALNO").ToString().Trim()

                    Dim UI_RMADID As String = dtTMP.Rows(i)("RMAD_SERIALNO").ToString().Trim()

                    If UI_UI_SERIALNO.ToString() <> "" Then
                        dtTMP.DefaultView.RowFilter = "RMAD_ID<>'" + UI_RMADID + "' AND RMAD_SERIALNO='" + UI_UI_SERIALNO + "' AND RMAD_MARK<>'1'"
                        If dtTMP.DefaultView.Count > 0 Then
                            'Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                        End If

                        If SNTemp.IndexOf(UI_UI_SERIALNO + "~@~;") >= 0 Then
                            'Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                        Else
                            dtTMP.DefaultView.RowFilter = "RMAD_ID='" + UI_RMADID.Trim + "'"
                            If dtTMP.DefaultView.Count > 0 Then
                                dtTMP.DefaultView(0)("RMAD_SERIALNO") = UI_UI_SERIALNO.Trim()
                            End If
                            SNTemp += UI_UI_SERIALNO.Trim() + "~@~;"
                        End If
                    Else
                        'Throw New ArgumentException(_oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator))
                    End If
                Next
                dtTMP.DefaultView.RowFilter = ""
                dtTMP.AcceptChanges()

                For i = 0 To dtTMP.Rows.Count - 1
                    Dim drTMP As RmaDTO.RMADetailRow = dtTMP.Rows(i)
                    Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow

                    Select Case Convert.ToInt16(Session("_eumCommand"))
                        Case eumCommand.AddNew
                            Dim oGuid As Guid = Guid.NewGuid
                            dr.RMAD_ID = oGuid.ToString
                            dr.RMAD_RMANO = ""

                        Case eumCommand.UPDATE
                            dr.RMAD_ID = drTMP.RMAD_ID
                            dr.RMAD_RMANO = drTMP.RMAD_RMANO
                            'dr.RMAD_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
                    End Select

                    dr.RMAD_SEQ = i + 1
                    If drTMP.IsRMAD_SERIALNONull = False Then dr.RMAD_SERIALNO = drTMP.RMAD_SERIALNO
                    If drTMP.IsRMAD_CUSNAMENull = False Then
                        dr.RMAD_CUSNAME = drTMP.RMAD_CUSNAME
                    Else
                        dr.RMAD_CUSNAME = ""
                    End If
                    If drTMP.IsRMAD_PARTSNNull = False Then

                        dr.RMAD_PARTSN = drTMP.RMAD_PARTSN
                    Else
                        dr.RMAD_PARTSN = ""
                    End If
                    '==================
                    'MODEL NO
                    '==================
                    If drTMP.IsRMAD_MODELNONull = False Then dr.RMAD_MODELNO = drTMP.RMAD_MODELNO
                    If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                        Dim sModelNo As String = oExport.getModelNo(drTMP.RMAD_SERIALNO.ToString().Trim())
                        If sModelNo.Trim() = "" Then
                            dr.RMAD_MODELNO = "OTHER"
                        Else
                            dr.RMAD_MODELNO = sModelNo
                        End If
                    End If


                    '==================
                    'WarrantyDate
                    '==================
                    Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                    If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then

                        Dim WarrantyDate As String = ""
                        If UI_cboRepairCenter = False Then
                            WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue_View.Value)
                        Else
                            WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue_View.Value)
                        End If

                        If WarrantyDate.Trim() <> "" Then
                            dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                        End If
                    End If

                    If drTMP.IsRMAD_FARFARCNONull = False Then
                        If drTMP.RMAD_FARFARCNO.Trim() <> "-1" Then
                            dr.RMAD_FARFARCNO = drTMP.RMAD_FARFARCNO
                        End If
                    End If

                    If drTMP.IsRMAD_FARNONull = False Then
                        If drTMP.RMAD_FARNO.Trim() <> "-1" Then
                            dr.RMAD_FARNO = drTMP.RMAD_FARNO
                        End If
                    End If

                    If drTMP.IsRMAD_UPLOADFILENull = False Then
                        dr.RMAD_UPLOADFILE = drTMP.RMAD_UPLOADFILE
                    Else
                        dr.RMAD_UPLOADFILE = ""
                    End If


                    If dtTMP.Rows(i)("RMAD_PRODUCTDESC") Is Nothing Then
                    Else
                        dr.RMAD_PROBLEMDESC = dtTMP.Rows(i)("RMAD_PRODUCTDESC").ToString().Trim()
                        dr.RMAD_PRODUCTDESC = dtTMP.Rows(i)("RMAD_PRODUCTDESC").ToString().Trim()
                    End If

                    If dtTMP.Rows(i)("CUSTOMER_PRODUCT_NUMBER") Is Nothing Then
                        dr.CUSTOMER_PRODUCT_NUMBER = ""
                    Else
                        dr.CUSTOMER_PRODUCT_NUMBER = dtTMP.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
                    End If



                    dr.RMAD_STATUS = RMA_Status
                    dr.RMAD_ISFILL = drTMP.RMAD_ISFILL
                    dr.RMAD_RECEVSTATUS = 0

                    dr.RMAD_AD = Session("_UserID")
                    dr.RMAD_ADNAME = Session("_UserName")
                    dr.RMAD_CSTMP = Date.Now
                    dr.RMAD_LUAD = Session("_UserID")
                    dr.RMAD_LUADNAME = Session("_UserName")
                    dr.RMAD_LUSTMP = Date.Now
                    dr.RMAD_MARK = drTMP.RMAD_MARK

                    dtRMADetail.AddRMADetailRow(dr)
                Next

            Catch ex As Exception
                Throw ex

            End Try

        End If

        Return dtRMADetail
    End Function

    ''' <summary>
    ''' 新增 RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_Head()
        Dim i As Integer = 0
        '' ''Dim oListItem As ListItem
        Dim dtAddress As New DataTable
        dtAddress.Columns.Add("CU_Address")

        '填資料
        Dim UI_lblPreviousPage_RMAID_Text As String = ""
        Dim UI_lblPreviousPage_RMANO_Text As String = ""

        Dim UI_txtAccountIDText As String = ""
        Dim UI_lblAccountIDText_Text As String = ""
        Dim UI_lblUserIDText_Text As String = ""
        Dim UI_txtApplicant_Text As String = ""
        Dim UI_txtTel_Text As String = ""
        Dim UI_txtCode_Text As String = ""
        Dim UI_txtAccountIDText_Text = ""
        Dim UI_txtMail_Text As String = ""
        Dim UI_txtRemark_Text As String = ""
        Dim UI_cboRepairCenter As String = ""   '維修中心

        Dim UI_lblRepairCenterValue As String = ""   '替代維修中心
        Dim add As String = ""               '地址
        Dim UI_cboRepairCenter_ As Boolean  '有沒有維修中心
        Dim UI_txtTel As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If UI_txtAccountIDText <> "" Then
                dtCustome = oCustomer.QueryByCompany(UI_lblAccountIDText_Text)
            End If
        Else
            dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
        End If

        Dim sTel As String = ""
        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            UI_txtAccountIDText = dr.CU_NO.ToString().Trim()
            UI_lblAccountIDText_Text = dr.CU_NAME.ToString().Trim()

            If dr.IsCU_TELNull = False Then sTel = dr.CU_TEL.ToString().Trim()
            If dr.IsCU_CONTACTPERSONNull = False Then UI_txtApplicant_Text = dr.CU_CONTACTPERSON.ToString().Trim()
            If dr.IsCU_EMAILNull = False Then UI_txtMail_Text = dr.CU_EMAIL.ToString().Trim()

            If dr.IsCU_ADDRESS1Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS1.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS2Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS2.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS3Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS3.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS4Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS4.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            '20211223 wisely 若為Enduer 不允許輸入EndUser 欄位
            Dim oRequested As New ctlRMA.Requested
            Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
            If (dt.Rows.Count > 0) Then

            Else

            End If

        End If


        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If UI_txtAccountIDText_Text.Trim <> "" Then
                dtCustomerUser = oCustomerUser.QueryUser(UI_txtAccountIDText_Text.Trim, "")
            End If
        Else
            dtCustomerUser = oCustomerUser.QueryUser(Session("_CustomerID").ToString(), Session("_UserID").ToString())
        End If

        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            UI_lblUserIDText_Text = dr.CUUS_ACCOUNTID.ToString().Trim()
            If dr.IsCUUS_TELNull = False Then sTel = dr.CUUS_TEL.ToString().Trim()

            If dr.IsCUUS_ADDRESSNull = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CUUS_ADDRESS.Trim()
                dtAddress.Rows.Add(oRow)

                Dim ADDRESSData_List As New List(Of String)
                'ADDRESSData_List = GetData(Me.UI_lblAccountIDText.Text.ToString().Trim())

                For t As Integer = 0 To ADDRESSData_List.Count - 1
                    Dim Address As String = ADDRESSData_List(t).ToString().Trim()
                    dtAddress.Rows.Add(Address)
                Next

            End If
        Next

        UI_txtTel = sTel.Trim()
        Me.ViewState("_dtAddress") = dtAddress

    End Sub


    ''' <summary>
    ''' 取消 RMA 單維修單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2011/08/04 START</remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click

        If HiddenField_RMANO.Value <> "" Then

            'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Dim sMessage As String = ""
            Dim blnFlag As Boolean = False

            Dim ctlClient As New ctlRMA.Client
            Dim dtClient As New RmaDTO.Client_CancelDataTable

            Try
                Dim dr As RmaDTO.Client_CancelRow = dtClient.NewClient_CancelRow
                dr.RMA_NO = HiddenField_RMANO.Value
                dr.RMA_CLIENTAD = Session("_UserID")
                dr.RMA_CLIENTADNAME = Session("_UserName")
                dr.RMA_CLIENTDATE = Date.Now

                dtClient.Rows.Add(dr)

                Call ctlClient.Client_Cancel(dtClient)
                blnFlag = True

            Catch ex As Exception
                sMessage = ex.Message
                blnFlag = False

            Finally

                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                Else
                    Dim sMsg As String = ""
                    sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                    Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, _RedirectURL)
                End If
            End Try


            ClientScript.RegisterStartupScript(GetType(String), "ShowInfoPage", "<script>window.parent.Reload_windows();window.parent.Location_Reload_windows();</script>")
        End If

    End Sub

End Class

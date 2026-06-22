Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ProductInformation_02
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    'ConfigurationSettings.AppSettings("Requested_VisualPath")
    'ConfigurationSettings.AppSettings("Requested_Upload_FilePath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True

            Response.Redirect("~/Default.aspx")
        End If

        If Me.IsPostBack = False Then

            Me.SerialNumberHiddenField.Value = Request.Params("SerialNumber")
            Me.UI_txtSerial.Text = Request.Params("SerialNumber")

            '第四步驟 讀取地址 還會使用到
            Call QueryData_Head()

            Call setDefault()

            UI_VisualPath.Value = _Requested_Upload_FilePath
            UI_WEBURL.Value = _WEBURL

            Me.AddBtn.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)

            Call getModelData()
        End If

    End Sub

    ''' <summary>
    ''' 初始化 的資料讀取
    ''' </summary>
    Private Sub QueryData_Head()
        Dim i As Integer = 0
        '' ''Dim oListItem As ListItem
        Dim dtAddress As New DataTable
        dtAddress.Columns.Add("CU_Address")

        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustome = oCustomer.QueryByCompany(Me.UI_txtAccountIDText.Text.Trim)
            End If
        Else
            dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
        End If

        Dim sTel As String = ""
        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            Me.UI_lblAccountIDText.Text = dr.CU_NO.ToString().Trim()
            Me.UI_lblAccountNameText.Text = dr.CU_NAME.ToString().Trim()

            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
            Me.UI_lblRepairCenterValue.Text = dr.COMP_NO.ToString().Trim()
            Me.UI_cboRepairCenter.SelectedValue = dr.COMP_NO.ToString().Trim()

            Me.UI_lblRepairCenterText.Visible = False
            Me.UI_cboRepairCenter.Visible = False
            If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                Me.UI_cboRepairCenter.Visible = True
            Else
                If dr.CU_ISCHOICE = "1" Then
                    Me.UI_cboRepairCenter.Visible = True
                Else
                    Me.UI_lblRepairCenterText.Visible = True
                End If
            End If

            If dr.IsCU_TELNull = False Then sTel = dr.CU_TEL.ToString().Trim()
            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_txtApplicant.Text = dr.CU_CONTACTPERSON.ToString().Trim()
            If dr.IsCU_EMAILNull = False Then Me.UI_txtMail.Text = dr.CU_EMAIL.ToString().Trim()

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

        End If

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustomerUser = oCustomerUser.QueryUser(Me.UI_txtAccountIDText.Text.Trim, "")
            End If
        Else
            dtCustomerUser = oCustomerUser.QueryUser(Session("_CustomerID").ToString(), Session("_UserID").ToString())
        End If

        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            Me.UI_lblUserIDText.Text = dr.CUUS_ACCOUNTID.ToString().Trim()
            If dr.IsCUUS_TELNull = False Then sTel = dr.CUUS_TEL.ToString().Trim()

            If dr.IsCUUS_ADDRESSNull = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CUUS_ADDRESS.Trim()
                dtAddress.Rows.Add(oRow)
            End If
        Next

    End Sub

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
                                       ByVal sFarFarcNo As String, ByVal sFarNo As String, ByVal sProblemDesc As String, ByVal sProductDesc As String, ByVal sPartSN As String) As RmaDTO.RMADetailDataTable

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
            dr.RMAD_WARRANTY = ""                               '20230906 自動帶入資料

            dr.RMAD_FARFARCNO = sFarFarcNo.Trim()
            dr.RMAD_FARNO = sFarNo.Trim()
            dr.RMAD_PARTSN = sPartSN.Trim()

            If File_HiddenField.Value.ToString().Trim() <> "" Then
                dr.RMAD_UPLOADFILE = File_HiddenField.Value.ToString().Trim()
            End If

            dr.RMAD_PRODUCTDESC = Message_Box.Text.Trim()
            dr.RMAD_PROBLEMDESC = Message_Box.Text.Trim()
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

            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMADetail
    End Function

    Private Function UpLoadFile() As String
        Dim retval As String = ""

        Try

            For i = 0 To Request.Files.Count - 1
                Dim postedFile As HttpPostedFile = Request.Files(i)

                If postedFile.ContentLength > 0 Then

                    Dim txtFileName As String = postedFile.FileName

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

                    postedFile.SaveAs(Server.MapPath(_Requested_Upload_FilePath) & sFileNameChange)
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

    Protected Sub FileUpload1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileUpload1.Disposed
        Dim contextes As String = FileUpload1.FileName.ToString().Trim()

        Me.CW_Show_OP.Text = contextes
    End Sub
    ''' <summary>
    ''' 上傳資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdFileAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdFileAdd.Click

        Dim contextes As String = FileUpload1.FileName.ToString().Trim()

        If FileUpload1.PostedFile.ContentLength > 0 Then


            Dim path As String = UpLoadFile()

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

    End Sub

    ''' <summary>
    ''' 新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AddBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBtn.Click

        If File_HiddenField.Value.Length < 125 Then

            '控制1-5的資料
            Session("_dtRMADetail") = Nothing

            Dim oExport As New ctlRMA.Export
            Dim oRequested As New ctlRMA.Requested
            Dim ctlReport As New ctlReport
            Dim sMessage As String = ""
            Dim retval As Boolean = True

            Try

                '檢查序號是否有填寫
                If UI_txtSerial.Text = "" Then
                    Dim sErrorMsg As String = _oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator) 'Wait
                    Throw New ArgumentException(sErrorMsg)
                End If

                '判斷是否為零件序號
                UI_txtSerialParts.Text = ""
                Dim sProductSn As String = ""

                Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
                'dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.UI_txtSerial.Text.Trim().ToUpper())
                dtReport = ctlReport.QueryRMAWarranty("", Me.UI_txtSerial.Text.Trim().ToUpper())
                If dtReport.Rows.Count > 0 Then
                    sProductSn = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
                End If
                'Throw New ArgumentException(sProductSn)
                If sProductSn <> "" Then
                    If sProductSn <> Me.UI_txtSerial.Text.Trim().ToUpper() Then
                        UI_txtSerialParts.Text = Me.UI_txtSerial.Text.Trim().ToUpper()
                        Me.UI_txtSerial.Text = sProductSn
                    Else
                        sProductSn = ""
                    End If
                End If

                Dim dt As DataTable = oRequested.IsEndUser(UI_lblUserIDText.Text.ToString().Trim())
                'If bool_EndUser Then
                'If (dt.Rows.Count > 0) Then
                '20220303 wisely Enduser 不可以送修零件
                'If UI_txtSerialParts.Text.ToString() <> "" Then
                'Me.UI_txtSerial.Text = UI_txtSerialParts.Text
                'Dim sErrorMsg As String = _oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator) 'Wait
                'Throw New ArgumentException(sErrorMsg)
                'End If

                'End If

                Dim dtRMADetail As New RmaDTO.RMADetailDataTable

                If IsNothing(Session("_dtRMADetail")) = True Then
                    dtRMADetail = AddRMADetail(dtRMADetail)
                Else
                    dtRMADetail = Session("_dtRMADetail")
                    dtRMADetail = AddRMADetail(dtRMADetail)
                End If

                Session("_dtRMADetail") = dtRMADetail
                Session("_eumCommand") = 1
                Dim sScript As String = "<script type=""text/javascript"">   window.location = 'ProductInformation_03.aspx';</script>"
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

            Catch ex As Exception
                sMessage = ex.Message
                retval = False
            Finally
                If retval = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                End If
            End Try

        Else
            '
            Dim msg As String = _oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag)
            If Session("_LanguageID") = "002" Then

                Me.ucMessage.showMessageByFailed(msg & "數量" & "太多")
            Else
                Me.ucMessage.showMessageByFailed(msg & " Qty " & "too much")

            End If


        End If

    End Sub

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As RmaDTO.RMADetailDataTable
        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim sWarranty As String = "" '_oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        Dim oExport As New ctlRMA.Export

        If Me.UI_txtSerial.Text.Trim() <> "" Then
            dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + Me.UI_txtSerial.Text.Trim().ToUpper() + "' AND RMAD_MARK<>'1'"
            If dtRMADetail.DefaultView.Count > 0 Then
                Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                'Throw New ArgumentException(sErrorMsg)
            End If
        End If

        Try

            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""

            '' ''dr.RMAD_MODELNO = Me.UI_txtModel.Text.Trim()
            'dr.RMAD_MODELNO = Me.UI_cboModel.SelectedValue
            If Me.UI_txtSerial.Text.Trim() <> "" Then
                Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim().ToUpper(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
                If sModelNo.Trim() = "" Then
                    dr.RMAD_MODELNO = "OTHER"
                Else
                    dr.RMAD_MODELNO = sModelNo
                End If
            Else
                If Me.UI_cboModel.SelectedValue.Trim() <> "" Then
                    dr.RMAD_MODELNO = Me.UI_cboModel.SelectedValue
                End If
            End If

            'Mod by Isaac 保固日期看抓全保、一般保的最大日期
            If Me.UI_txtSerial.Text.ToString().Trim() <> "" Then
                'Dim WarrantyDate As String = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString())
                Dim WarrantyDate As String = ""
                If Me.UI_cboRepairCenter.Visible = False Then
                    WarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                Else
                    WarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                End If

                If WarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If

                If WarrantyDate.Trim() <> "" Then
                    dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If
            End If

            dr.RMAD_SERIALNO = Me.UI_txtSerial.Text.ToString().Trim().ToUpper()
            dr.RMAD_CUSNAME = ""                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty          'Warranty 字串格式
            'If IsDate(sWarranty) = True Then
            'dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
            'End If

            If File_HiddenField.Value.ToString().Trim() <> "" Then

                dr.RMAD_UPLOADFILE = File_HiddenField.Value.ToString().Trim()

            End If

            dr.RMAD_FARFARCNO = Me.UI_cboFailureClass.SelectedItem.Value
            dr.RMAD_FARNO = Me.UI_cboFailure.SelectedItem.Value
            dr.RMAD_MODELNO = Me.UI_cboModel.SelectedItem.Value
            dr.RMAD_STATUS = 0
            dr.RMAD_PRODUCTDESC = Message_Box.Text.Trim()
            dr.RMAD_PROBLEMDESC = Message_Box.Text.Trim()

            dr.RMAD_PARTSN = UI_txtSerialParts.Text.Trim().ToUpper()
            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
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

    Private Sub setDefault()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)
        Me.UI_cmdFileAdd.Text = _oLanguage.getText("RMA", "211", ctlLanguage.eumType.Command)
        Me.CustomerProductNumberLab.Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag)
        Me.CUSTOMER_Txt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "018", ctlLanguage.eumType.Tag))

        Me.FaultLab.Text = _oLanguage.getText("RMA2", "091", ctlLanguage.eumType.Tag)
        Me.FaultDLab.Text = _oLanguage.getText("RMA2", "092", ctlLanguage.eumType.Tag)

        Me.UploadFileLab.Text = _oLanguage.getText("RMA2", "020", ctlLanguage.eumType.Tag)
        Me.Select_FileLab.Text = _oLanguage.getText("RMA2", "021", ctlLanguage.eumType.Tag)
        Me.clear_btn.Text = _oLanguage.getText("RMA2", "022", ctlLanguage.eumType.Tag)
        Me.download_btn.Text = _oLanguage.getText("RMA2", "023", ctlLanguage.eumType.Tag)
        Me.ProblemDescriptionLabe.Text = _oLanguage.getText("RMA2", "024", ctlLanguage.eumType.Tag)
        Me.UploadFileSizeLab.Text = _oLanguage.getText("RMA2", "061", ctlLanguage.eumType.Tag)
        Me.SpecificationProblemLab.Text = _oLanguage.getText("RMA2", "016", ctlLanguage.eumType.Tag)

        Me.UI_Serial_Number.Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag)
        Me.UI_Model_Lab.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_cboFailureClass.SelectedIndex = 0
        Me.UI_cboModel.SelectedIndex = 1

        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)

    End Sub

    ''' <summary>
    ''' 不良原因代碼(下拉式)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboFailureClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFailureClass.SelectedIndexChanged
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)
    End Sub

    ''' <summary>
    ''' 取得Model 的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getModelData()
        Try
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


            '自動帶入型號 開始

            Dim oExport As New ctlRMA.Export

            Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim().ToUpper())
            If sModelNo.Trim() = "" Then
                Me.UI_cboModel.SelectedValue = "Others"

            Else
                Me.UI_cboModel.SelectedValue = "Others"
                For i = 0 To Me.UI_cboModel.Items.Count - 1
                    If Me.UI_cboModel.Items(i).Value.ToLower().Trim() = sModelNo.ToLower().Trim() Then
                        Me.UI_cboModel.SelectedValue = sModelNo
                        Exit For
                    End If
                Next
            End If

            '自動帶入型號 結束

        Catch ex As Exception
            Dim oListItem As ListItem
            Me.UI_cboModel.Items.Clear()
            oListItem = New ListItem
            oListItem.Text = "Others"
            oListItem.Value = "XXXX"
            Me.UI_cboModel.Items.Add(oListItem)
        End Try

    End Sub

    Protected Sub clear_btn_Click(sender As Object, e As EventArgs) Handles clear_btn.Click

        Me.File_HiddenField.Value = ""
        Me.html_File_.Value = ""
        Me.UploadLabel.Text = ""
    End Sub

    Protected Sub download_btn_Click(sender As Object, e As EventArgs) Handles download_btn.Click
        Try

            Dim UI_WEBURL_string As String = Me.UI_WEBURL.Value.Trim()
            Dim UI_VisualPath_string As String = Me.UI_VisualPath.Value.Trim()
            Dim html_File__string As String = Me.html_File_.Value.Trim()

            Dim FileName As String() = html_File__string.Split(",")
            Dim path As String = UI_VisualPath_string + FileName(1)

            Dim UI_WEBURL_string_D As String = Me.UI_WEBURL.Value.Trim()

            Dim sScript As String = "<script type=""text/javascript"">btnDownload(""" & path & """, """ & FileName(1) & """)</script>"
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "btnDownloadMsg", sScript)
        Catch ex As Exception

        End Try
    End Sub

End Class

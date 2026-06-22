Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class RMARepair_UpLoad
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_FilePath As String = ConfigurationSettings.AppSettings("Repair_FilePath")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "3"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            Call setControls()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.ToString().Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.ToString().Trim()

                Call RepairHead()
                Call RepairDetail(0)
                Call RepairUpload(0)
            End If
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim sClientID As String = Me.UI_cmdUpload.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID


        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        'Attachment upload only 維修人員 才能上傳.及 才能刪除檔案
        If Session("_Role").ToString().IndexOf("2") <> -1 Then
            Me.UI_TRFileUpload.Visible = True
            Me.UI_TRUploadFileDesc.Visible = True
            Me.UI_dvRetailUpload.Columns(3).Visible = True

        Else
            Me.UI_dvRetailUpload.Columns(3).Visible = False
        End If

        Call setValidationMessage(Me.rfvhtmlFullFile)
        'Me.cv_FullFile.ErrorMessage = _oLanguage.getText("RMA", "120", ctlLanguage.eumType.Tag)


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "114", ctlLanguage.eumType.Tag)
        Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicantID.Text = _oLanguage.getText("RMA", "045", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblReportUpload.Text = _oLanguage.getText("RMA", "115", ctlLanguage.eumType.Tag)
        Me.UI_lblReportAttachment.Text = _oLanguage.getText("RMA", "116", ctlLanguage.eumType.Tag)
        Me.UI_lblProductTittle.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblDesc.Text = _oLanguage.getText("RMA", "120", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)

        Me.UI_UploadFileDesc.Text = _oLanguage.getText("RMA", "186", ctlLanguage.eumType.Tag)

        Me.UI_cmdUpload.Text = _oLanguage.getText("Common", "037", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "038", ctlLanguage.eumType.Command)

    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        sErrorMessage = _oLanguage.getText("RMA", "121", ctlLanguage.eumType.Validator)
        oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

    End Sub

#Region "RepairHead"
    Private Sub RepairHead()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable

        dtRepairHead = oRepair.QueryByRepairHead(Me.UI_lblPreviousPage_RMAID.Text)
        If dtRepairHead.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_HeadRow = dtRepairHead.Rows(0)
            Me.UI_lblRMANoText.Text = dr.RMA_NO.ToString().Trim()
            Me.UI_lblAccountIDText.Text = dr.RMA_CUNO.ToString().Trim()
            Me.UI_lblAccountNameText.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblApplicantIDText.Text = dr.RMA_ACCOUNTID.ToString().Trim()
            Me.UI_lblApplicantText.Text = dr.RMA_APPLICANT.ToString().Trim()
            Me.UI_lblTelText.Text = dr.RMA_TEL.ToString().Trim()
            Me.UI_lblAddressText.Text = dr.RMA_ADDRESS.ToString().Trim()
            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
            Me.UI_lblRepairCenterNO.Text = dr.RMA_COMPNO.ToString().Trim()
        End If
    End Sub
#End Region

#Region "RepairDetail"
    Private Sub RepairDetail(ByVal iPageIndex As Integer)
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairDetail As New RmaDTO.tmpRepair_DetailDataTable

        dtRepairDetail = oRepair.QueryAllByRMADetail(Session("_LanguageID").ToString().Trim(), Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim())
        Call ReplaceData(dtRepairDetail)

        Session("_dtRepairDetail") = dtRepairDetail
        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView

        Call RepairDetail_DataBind(dvRepairDetail, iPageIndex)
    End Sub

    Private Sub RepairDetail_DataBind(ByVal dvRepairDetail As DataView, ByVal iPageIndex As Integer)
        Me.UI_dvRetailDetail.PageSize = _PageSize
        Me.UI_dvRetailDetail.PageIndex = iPageIndex
        Me.UI_dvRetailDetail.DataSource = dvRepairDetail
        Me.UI_dvRetailDetail.DataBind()
    End Sub

    Private Sub ReplaceData(ByVal dtRepairDetail As RmaDTO.tmpRepair_DetailDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export

        dtRepairDetail.Columns.Add("Warranty")
        dtRepairDetail.Columns.Add("CWEndWarr")
        dtRepairDetail.Columns.Add("SWEndWarr")
        dtRepairDetail.Columns.Add("FarcName")
        For i = 0 To dtRepairDetail.Rows.Count - 1
            Dim dr As RmaDTO.tmpRepair_DetailRow = dtRepairDetail.Rows(i)
            Dim sFarcName As String = ""

            If dr.IsRMAR_FARCNONull = False And dr.IsRMAR_FARCNAMENull = False Then
                sFarcName = dr.RMAR_FARCNAME.ToString().Trim()
            Else
                If dr.IsRMAD_FARFARCNONull = False And dr.IsFARC_NAMENull = False Then
                    sFarcName = dr.FARC_NAME.ToString().Trim()
                End If
            End If
            dtRepairDetail.Rows(i).Item("FarcName") = sFarcName.ToString().Trim()

            '2021/05/05 置換ModelNO
            Dim sModelNo As String = oExport.getMModelNo(dtRepairDetail.Rows(i)("RMAD_MODELNO").ToString().Trim(), Me.UI_lblRepairCenterNO.Text, Me.UI_lblAccountIDText.Text.Trim())
            If sModelNo.Trim() <> "" Then
                dtRepairDetail.Rows(i)("RMAD_MODELNO") = sModelNo.Trim()
            End If

            '保固日期
            If dr.IsRMAD_WARRANTYNull = False Then
                dtRepairDetail.Rows(i).Item("Warranty") = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()

            Else
                dtRepairDetail.Rows(i).Item("Warranty") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRepairDetail.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtRepairDetail.Rows(i)("Warranty") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtRepairDetail.Rows(i)("Warranty") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select
            End If


            Dim sCWEnd As String = oExport.getWarrantyCW(dtRepairDetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRepairDetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If
            Dim sSWEnd As String = oExport.getWarrantySW(dtRepairDetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRepairDetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If

        Next
    End Sub

    Protected Sub UI_dvRetailDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRetailDetail.RowCommand

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_dvRetailDetail.Rows(iIndex)
            Dim RMAD_ID As Label = row.FindControl("UI_RMADID")
            Dim RMA_NO As String = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()

            '身分:1.客戶, 2.公司(維修中心)
            Select Case Convert.ToInt16(Session("_Identity"))
                Case 1
                    Me.ucClientDetail.show(RMAD_ID.Text.ToString().Trim(), RMA_NO.Trim(), True)
                Case 2
                    Me.ucRepairDetail.show(RMAD_ID.Text.ToString().Trim(), RMA_NO.Trim(), True)
            End Select
        End If

    End Sub

    Protected Sub UI_dvRetailDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRetailDetail.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
            'e.Row.Cells(4).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            'e.Row.Cells(5).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            'e.Row.Cells(6).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "119", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(4).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(4).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(4).ForeColor = Drawing.Color.Red
                End If
            End If

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If

    End Sub

    Protected Sub UI_dvRetailDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRetailDetail.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRepairDetail") Is Nothing Then
            Dim dtRepairDetail As RmaDTO.tmpRepair_DetailDataTable = Session("_dtRepairDetail")
            Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView
            Call RepairDetail_DataBind(dvRepairDetail, iPageIndex)

        Else
            Call RepairDetail(iPageIndex)
        End If
    End Sub

#End Region

#Region "RepairUpload"

    Private Sub RepairUpload(ByVal iPageIndex As Integer)
        Dim oRepairUpload As New ctlRMA.Repair
        Dim dtRepairUpload As New RmaDTO.RMAREPAIR_UPLOADDataTable

        dtRepairUpload = oRepairUpload.QueryByUpload(Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim())

        '沒有資料時, 控制 UI_lblReportAttachment.Text="Report Attachment" 標題, 不要出現
        Me.UI_TRReportAttachment.Visible = False
        If dtRepairUpload.Rows.Count > 0 Then
            Me.UI_TRReportAttachment.Visible = True
        End If


        Session("_dtRepairUpload") = dtRepairUpload
        Dim dvRepairUpload As DataView = dtRepairUpload.DefaultView

        Call RepairUpload_DataBind(dvRepairUpload, iPageIndex)
    End Sub

    Private Sub RepairUpload_DataBind(ByVal dvRepairUpload As DataView, ByVal iPageIndex As Integer)
        Me.UI_dvRetailUpload.PageSize = _PageSize
        Me.UI_dvRetailUpload.PageIndex = iPageIndex
        Me.UI_dvRetailUpload.DataSource = dvRepairUpload
        Me.UI_dvRetailUpload.DataBind()
    End Sub

    Protected Sub UI_dvRetailUpload_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRetailUpload.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = _oLanguage.getText("RMA", "117", ctlLanguage.eumType.Tag)
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "118", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "041", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RepairUpload As HyperLink = e.Row.FindControl("UI_RepairUpload")
            Dim UI_lblRepairUpload As Label = e.Row.FindControl("UI_lblRepairUpload")
            Dim sRepaurFile As String() = UI_lblRepairUpload.Text.ToString().Trim().Split(",")

            UI_RepairUpload.Text = sRepaurFile(0).ToString().Trim()
            UI_RepairUpload.NavigateUrl = "https://e-rma-admin.cipherlab.com.tw" & _Repair_VisualPath & sRepaurFile(1).ToString().Trim()
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If

    End Sub

    Protected Sub UI_dvRetailUpload_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRetailUpload.RowCommand

        If e.CommandName = "cmdDel" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRetailUpload.Rows(iIndex)

            Dim UI_RMARU_ID As Label = row.FindControl("UI_RMARU_ID")

            '刪除上傳檔案資料
            Dim oRepairUpload As New ctlRMA.Repair
            oRepairUpload.DeleteUpload(UI_RMARU_ID.Text.Trim())

            Call RepairUpload(0)
        End If

    End Sub

    Protected Sub UI_dvRetailUpload_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRetailUpload.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRepairUpload") Is Nothing Then
            Dim dtRepairUpload As RmaDTO.RMAREPAIR_UPLOADDataTable = Session("_dtRepairUpload")
            Dim dvRepairUpload As DataView = dtRepairUpload.DefaultView
            Call RepairUpload_DataBind(dvRepairUpload, iPageIndex)

        Else
            Call RepairUpload(iPageIndex)
        End If
    End Sub

#End Region

    ''' <summary>
    ''' 檔案上傳
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdUpload.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oRepairUpload As New ctlRMA.Repair
        Dim dtRepairUpload As New RmaDTO.RMAREPAIR_UPLOADDataTable
        Dim dr As RmaDTO.RMAREPAIR_UPLOADRow = dtRepairUpload.NewRMAREPAIR_UPLOADRow

        Dim oGuid As Guid = Guid.NewGuid

        Try

            Dim sFullFileName As String = UpLoadFile()
            If sFullFileName = "" Then
                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
            End If


            dr.RMARU_ID = oGuid.ToString().Trim()

            dr.RMARU_RMANO = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()
            dr.RMARU_UPLOADFILE = sFullFileName
            dr.RMARU_DESC = Me.UI_lblDescription.Text.ToString.Trim()

            dr.RMARU_AD = Session("_UserID")
            dr.RMARU_ADNAME = Session("_UserName")
            dr.RMARU_CSTMP = Date.Now
            dr.RMARU_LUAD = Session("_UserID")
            dr.RMARU_LUADNAME = Session("_UserName")
            dr.RMARU_LUSTMP = Date.Now

            dtRepairUpload.AddRMAREPAIR_UPLOADRow(dr)
            oRepairUpload.SaveUpload(dtRepairUpload)

            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Me.UI_lblDescription.Text = ""
                Call RepairUpload(0)
            End If
        End Try

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

            Me.html_FileUpload.MoveTo(_Repair_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
            retval = sFullFileName



            'If Me.html_FileUpload.HasFile = True Then
            'If Me.html_FileUpload.FileContent.Length <> 0 Then
            '    Dim txtFileName As String = Me.html_FileUpload.FileName

            '    '****** 取得檔名 **********
            '    Dim FileSplit() As String = Split(txtFileName, "\")
            '    Dim FileName As String = FileSplit(FileSplit.Length - 1)

            '    '****** 取得副檔名 **********
            '    Dim auxFileSplit() As String = Split(FileName, ".")
            '    Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
            '    Dim sFileNameChange As String = ""
            '    Dim oCommon As New Common
            '    sFileNameChange = oCommon.GetRandomizeNum()
            '    sFileNameChange = sFileNameChange & "." & auxFileName

            '    '***** 檔案(原檔名,亂數檔名) *****
            '    Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

            '    Dim filefullname As String = System.IO.Path.Combine(Request.PhysicalApplicationPath + "\object", txtFileName)
            '    Me.html_FileUpload.MoveTo(filefullname, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)

            '    'Me.html_FileUpload.SaveAs(_Repair_FilePath & sFileNameChange)
            '    retval = sFullFileName
            'End If
            'End If

        Catch ex As Exception
            Throw ex
            retval = ""

        End Try

        Return retval
    End Function

End Class

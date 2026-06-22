Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage

Partial Class Upload_List
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Public _TempExcelFolder As String = ConfigurationSettings.AppSettings("TempExcelFolder")
    Public _EmailAttach As String = ConfigurationSettings.AppSettings("EmailAttachFolder")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    'Dim _PageSize As String = "1"
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SerialNo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Status") = "-1"
            'Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            'Me.ViewState("edate") = Date.Now.ToShortDateString()
            Me.ViewState("fdate") = ""
            Me.ViewState("edate") = ""

            Session("_dtRmaList") = Nothing

            Call setControls()
            Call QueryData(0)
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())

        'Call oCommon.getStatus(UI_cboStatus)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Upload", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("Upload", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblCust.Text = _oLanguage.getText("Upload", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("Upload", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("Upload", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("Upload", "006", ctlLanguage.eumType.Tag)
        Me.UI_butImport.Text = _oLanguage.getText("Upload", "007", ctlLanguage.eumType.Tag)
        Me.UI_butReject.Text = _oLanguage.getText("Upload", "008", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("Upload", "009", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("Upload", "010", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("Upload", "011", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("Upload", "012", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(6).HeaderText = _oLanguage.getText("Upload", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("Upload", "014", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("Upload", "015", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oImport As New ctlRMA.Upload
        Dim dtRmaList As New ImpRmaDTO.dtImpRmaDataTable

        Dim sSerialNo As String = Me.ViewState("_SerialNo").ToString().Trim()
        Dim sCustomerID As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim Status As Integer = Convert.ToInt16(Me.ViewState("_Status"))
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtRmaList = oImport.QueryRmaImport(Session("_RepairCenter"), "", "", "", sSerialNo, sCustomerID, fdate, edate, "IRMA_NO desc")

        Call ArrangementData(dtRmaList)
        Session("_dtRmaList") = dtRmaList

        Dim dvReceiveList As DataView = dtRmaList.DefaultView
        Me.ViewState("_SortExpression") = "IRMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvReceiveList.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvReceiveList, iPageIndex)
    End Sub

    Private Sub RMA_DataBind(ByVal dvReceiveList As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvReceiveList
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRmaList As ImpRmaDTO.dtImpRmaDataTable)
        Dim i As Integer = 0

        If dtRmaList.Columns("SeqID") Is Nothing Then
            dtRmaList.Columns.Add("SeqID")
        End If

        For i = 0 To dtRmaList.Rows.Count - 1
            dtRmaList.Rows(i)("SeqID") = i + 1
        Next
    End Sub

    Protected Sub UI_dvRequest_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_dvRequest.PageIndexChanged

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvRequest.PageIndex * Me.UI_dvRequest.PageSize) + (e.Row.RowIndex + 1).ToString()
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

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRmaList") Is Nothing Then
            Dim dtReceiveList As ImpRmaDTO.dtImpRmaDataTable = Session("_dtRmaList")
            Dim dvReceiveList As DataView = dtReceiveList.DefaultView
            Call RMA_DataBind(dvReceiveList, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        Dim row As GridViewRow

        If e.CommandName = "cmdErrorMsg" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Me.ucImportErrorMsg1.show(UI_RMAID.Text.ToString().Trim(), True)
        End If

        If e.CommandName = "cmdDel" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANo As Label = row.FindControl("UI_RMANo")
            Dim UI_ERROR As Label = row.FindControl("UI_ERROR")
            Dim UI_FILE As Label = row.FindControl("UI_FILE")
            Call Delete(UI_RMAID.Text.ToString().Trim(), UI_RMANo.Text.ToString().Trim(), UI_ERROR.Text.ToString().Trim(), UI_FILE.Text.ToString().Trim())
            Call QueryData(0)
        End If

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANo As Label = row.FindControl("UI_RMANo")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANo.Text.Trim()
        End If

    End Sub

    Protected Sub UI_dvRequest_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvRequest.Sorting

        If Me.ViewState("_SortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_SortDirection") = "asc"
        Else
            If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_SortDirection") = "desc"
            Else
                Me.ViewState("_SortDirection") = "asc"
            End If
        End If
        Me.ViewState("_SortExpression") = e.SortExpression

        If IsNothing(Session("_dtRmaList")) = False Then
            Dim dtRMA_tmp As DataTable = Session("_dtRmaList")
            Dim dvDetail As DataView = dtRMA_tmp.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvDetail, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvRequest.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvRequest.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvRequest.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvRequest.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 刪除 RMA 單
    ''' </summary>
    ''' <param name="sRMAID"></param>
    ''' <param name="sRMANO"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal sRMAID As String, ByVal sRMANO As String, ByVal ErrorLog As String, ByVal FileName As String)
        Dim oReceived As New ctlRMA.Upload
        Dim dtReceiveList As New ImpRmaDTO.dtImpRmaDataTable

        oReceived.DeleteRMA_FromIMP_RMA(sRMAID, sRMANO, Session("_UserID").ToString(), Session("_UserName").ToString())
        SalesCancel_SendMail(sRMAID, sRMANO, ErrorLog, FileName)

    End Sub

    ''' <summary>
    ''' 轉入 RMA 單
    ''' </summary>
    ''' <param name="sRMAID"></param>
    ''' <param name="sRMANO"></param>
    ''' <remarks></remarks>
    Private Function Import(ByVal sRMAID As String, ByVal sRMANO As String) As String
        Dim sMessage As String = ""
        Dim sReturn As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim oReceived As New ctlRMA.Upload
            Dim dtReceiveList As New ImpRmaDTO.dtImpRmaDataTable

            sReturn = oReceived.ImportRMA(sRMAID, sRMANO, Session("_UserID").ToString(), Session("_UserName").ToString())

            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
            End If
        End Try
        Return sReturn
    End Function
    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_SerialNo") = Me.UI_txtSerialNumber.Text.Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCust.Text.Trim()

        Me.ViewState("_Status") = "-1"
        'Dim Status As Integer = Convert.ToInt16(Me.UI_cboStatus.SelectedValue)

        Me.ViewState("fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            Me.ViewState("edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

    '匯入資料
    Protected Sub UI_butImport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sMessage As String = ""
        Dim sReturn As String = ""
        Dim blnFlag As Boolean = False
        Dim bCheckFlag As Boolean = False
        Dim i As Integer = 0
        Try
            For i = 0 To Me.UI_dvRequest.Rows.Count - 1
                If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                    If UI_Check.Checked Then
                        bCheckFlag = True
                        Dim UI_RMAID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMAID")
                        Dim UI_RMANo As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMANo")
                        sReturn = sReturn + Import(UI_RMAID.Text.ToString().Trim(), UI_RMANo.Text.ToString().Trim()) + "</br>"
                    End If
                End If
            Next

            Call QueryData(0)
            blnFlag = True
            If bCheckFlag = False Then
                blnFlag = False
                sMessage = "Please select item~"
            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Me.ucMessage.showMessageByFailed(sReturn)
            End If
        End Try

    End Sub

    '批量刪除
    Protected Sub UI_butReject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim bCheckFlag As Boolean = False
        Dim i As Integer = 0
        Try
            For i = 0 To Me.UI_dvRequest.Rows.Count - 1
                If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                    If UI_Check.Checked Then
                        bCheckFlag = True
                        Dim UI_RMAID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMAID")
                        Dim UI_RMANo As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMANo")
                        Dim UI_ERROR As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_ERROR")
                        Dim UI_FILE As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_FILE")
                        Call Delete(UI_RMAID.Text.ToString().Trim(), UI_RMANo.Text.ToString().Trim(), UI_ERROR.Text.ToString().Trim(), UI_FILE.Text.ToString().Trim())
                    End If
                End If
            Next

            Call QueryData(0)
            blnFlag = True
            If bCheckFlag = False Then
                blnFlag = False
                sMessage = "Please select item~"
            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
            End If
        End Try

    End Sub

    Private Function SalesCancel_SendMail(ByVal RMA_ID As String, ByVal RMA_NO As String, ByVal ErrorLog As String, ByVal FileName As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try

            Dim oRepair As New ctlRMA.Repair
            Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable
            Dim sAccountIDText As String = ""
            Dim sApplicantIDText As String = ""
            Dim sApplicantText As String = ""
            Dim sRepairIDText As String = ""
            Dim sCopName As String = ""


            dtRepairHead = oRepair.QueryByIRepairHead(RMA_ID)
            If dtRepairHead.Rows.Count > 0 Then
                Dim dr As RmaDTO.vwRepair_HeadRow = dtRepairHead.Rows(0)

                sAccountIDText = dr.RMA_CUNO.ToString().Trim()
                sApplicantIDText = dr.RMA_ACCOUNTID.ToString().Trim()
                sApplicantText = dr.CU_NAME.ToString().Trim()
                sCopName = dr.COMP_NAME.ToString().Trim()
            End If

            'Dim oRMAStatus As New ctlRMA.RMAStatus
            'Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            'dtStatusPoint = oRMAStatus.QueryPointByDetail(RMA_ID)
            'If dtStatusPoint.Rows.Count > 0 Then
            '    Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)
            '    If dr.IsREPAIRQUOTED_ADNull = False Then sRepairIDText = dr.REPAIRQUOTED_AD.Trim()
            'End If

            dtCustomer = oCustomer.QueryUser(sAccountIDText, sApplicantIDText, "")
            If dtCustomer.Rows.Count > 0 Then
                Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                '================================================================================================================================================================================================================
                '業務報價確認 -->對象(顧客)
                '================================================================================================================================================================================================================
                Dim CU_NAME As String = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                Dim MailSales As String = ""
                Dim SalesName As String = ""
                If CU_SALESID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_SALESID, "")
                    MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                    SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim MailAssistant As String = ""
                Dim AssistantName As String = ""
                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim MailRepair As String = ""
                Dim RepairName As String = ""
                If sRepairIDText.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(sRepairIDText, "")
                    MailRepair = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    RepairName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim sSubject As String = _oLanguage.getText("Mail", "035", ctlLanguage.eumType.Mail)
                Dim sBody As String = _oLanguage.getText("Mail", "036", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                If MailUser.Trim() <> "" Then
                    sSubject = sSubject.Replace("[$RMA No$]", RMA_NO)

                    sSubject = sSubject.Replace("[$Customer User Name$]", sApplicantText)
                    sSubject = sSubject.Replace("customer /  sales", sCopName)

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                    End If

                    sBody = sBody.Replace("[$RMA No$]", RMA_NO)
                    sBody = sBody.Replace("[$Repair User Name$]", RepairName)
                    sBody = sBody.Replace("[$RMA Request No$]", RMA_NO)
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)
                    sBody = sBody & "</br>" + ErrorLog

                    Dim sFullFileName As String = ""
                    Try
                        sFullFileName = _EmailAttach + "\\" + FileName
                        If File.Exists(sFullFileName) = False Then
                            sFullFileName = ""
                        End If
                    Catch ex As Exception

                    End Try
                    Dim fAttach As New Collection
                    If sFullFileName <> "" Then fAttach.Add(sFullFileName)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = MailUser + "," + MailRepair + "," + MailSales + "," + MailAssistant
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC, fAttach)
                End If
            End If

        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
            End If
        End Try

        Return blnFlag
    End Function
End Class

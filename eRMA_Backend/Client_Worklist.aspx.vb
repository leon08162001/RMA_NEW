Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Client_Worklist
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _ClientID As String = ""
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_QuoteTotal") = 0
            Me.ViewState("_CurrencyCode") = ""

            Me.ViewState("_RMANo") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("edate") = Date.Now.ToShortDateString()

            'Session("_dtClientList") = Nothing
            Session("_dtRMA_tmp") = Nothing

            Call setDefault()

            Call QueryData()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        _ClientID = "ctl00_ContentPlaceHolder_UI_dvCustomer_ctl01_UI_CheckGroup"
        _ClientID = _ClientID & "," & Me.UI_cmdSearch.ClientID '& "," & Me.UI_cmdQuoting.ClientID & "," & Me.UI_cmdRepairing.ClientID

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblQuickTittle.Text = _oLanguage.getText("RMA", "171", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)

        Me.UI_dvCustomer.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(3).HeaderText = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(4).HeaderText = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(5).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(6).HeaderText = _oLanguage.getText("RMA", "214", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(7).HeaderText = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(8).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(9).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

        Dim dtRMA_tmp As New DataTable
        dtRMA_tmp.Columns.Add("RMA_NO")
        dtRMA_tmp.Columns.Add("RMA_ID")
        dtRMA_tmp.Columns.Add("RequestDate")
        dtRMA_tmp.Columns.Add("Applicant")
        dtRMA_tmp.Columns.Add("CurrencyCode")
        dtRMA_tmp.Columns.Add("QUOTE")
        dtRMA_tmp.Columns.Add("RMAD_STATUS")
        dtRMA_tmp.Columns.Add("RequestQty")
        dtRMA_tmp.Columns.Add("ShippedQty")
        dtRMA_tmp.Columns.Add("Remark")
        dtRMA_tmp.Columns.Add("RMAD_ID")
        dtRMA_tmp.Columns.Add("RMASQ_ID")
        Session("_dtRMA_tmp") = dtRMA_tmp
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString.Trim()

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

        Me.lblFooterDesc.Text = ""
        Me.lblQuoteTotal.Text = ""
        Call QueryData()
    End Sub

    Private Sub QueryData()
        Dim oClient As New ctlRMA.Client
        Dim dtClientList As New RmaDTO.vwClient_WorkListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = ""
        Dim sSerialNo As String = ""

        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim sCustomerName As String = ""
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'Session("_CustomerID")-->客戶編號
        dtClientList = oClient.QueryByWork(Session("_CustomerID").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerName, fdate, edate)


        '=======================================================================================================================================
        '無資料時, UI 的控制
        '=======================================================================================================================================
        Me.UI_cmdConfirm.Visible = True
        Me.UI_cmdCancel.Visible = True
        Me.UI_lblQuickTittle.Visible = True
        If dtClientList.Rows.Count = 0 Then
            Me.UI_cmdConfirm.Visible = False
            Me.UI_cmdCancel.Visible = False
            Me.UI_lblQuickTittle.Visible = False
        End If

        Call ArrangementData(dtClientList)

        Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
        Dim dvDetail As DataView = dtRMA_tmp.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvDetail)
    End Sub

    Private Sub RMA_DataBind(ByVal dvDetail As DataView)
        Dim i As Integer = 0
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())
        Me.UI_dvCustomer.DataSource = dvDetail
        Me.UI_dvCustomer.DataBind()


        Dim blnCheck As Boolean = False
        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            If Me.UI_dvCustomer.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim oCheck As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")
                If oCheck.Visible = True Then
                    blnCheck = True
                    Exit For
                End If
            End If
        Next

        If IsNothing(Me.UI_dvCustomer.HeaderRow) = False Then
            Dim oCheckGroup As CheckBox = Me.UI_dvCustomer.HeaderRow.FindControl("UI_CheckGroup")
            oCheckGroup.Visible = False
            If blnCheck = True Then
                oCheckGroup.Visible = True
            End If
        End If

    End Sub

    Private Sub ArrangementData(ByVal dtClientList As RmaDTO.vwClient_WorkListDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
        dtRMA_tmp.Rows.Clear()
        Dim dvRMA_tmp As DataView = dtRMA_tmp.DefaultView

        For i = 0 To dtClientList.Rows.Count - 1
            Dim dr As RmaDTO.vwClient_WorkListRow = dtClientList.Rows(i)
            Dim RMA_No As String = dr.RMA_NO.ToString().Trim()

            dvRMA_tmp.RowFilter = "RMA_NO='" & RMA_No & "'"
            If dvRMA_tmp.Count = 0 Then
                Dim drTmp As DataRow = dtRMA_tmp.NewRow
                drTmp("RMA_NO") = RMA_No
                drTmp("RMA_ID") = dr.RMA_ID.Trim()
                drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
                drTmp("Applicant") = dr.RMA_APPLICANT.Trim()
                drTmp("RMAD_STATUS") = dr.RMAD_STATUS
                If dr.IsRMA_RemarkNull = False Then drTmp("Remark") = dr.RMA_Remark.Trim
                dtRMA_tmp.Rows.Add(drTmp)
            End If
        Next
        dvRMA_tmp.RowFilter = ""


        Dim dvClientList As DataView = dtClientList.DefaultView
        For i = 0 To dvRMA_tmp.Count - 1
            Dim CurrencyCode As String = ""
            Dim RMAD_ID As String = ""
            Dim RMASQ_ID As String = ""

            Dim QUOTE As Double = 0
            Dim RequestQty As Integer = 0
            Dim ShippedQty As Integer = 0

            Dim RMA_No As String = dvRMA_tmp(i).Item("RMA_NO").ToString.Trim()
            dvClientList.RowFilter = "RMA_NO='" & RMA_No & "'"
            For j = 0 To dvClientList.Count - 1
                Dim dr As RmaDTO.vwClient_WorkListRow = dvClientList(j).Row
                If dr.IsRMASQ_CURRENCYCODENull = False Then CurrencyCode = dr.RMASQ_CURRENCYCODE.Trim()
                If dr.IsRMASQ_QUOTENull = False Then QUOTE = QUOTE + Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N")

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                If dr.RMAD_STATUS = "40" Then
                    If RMAD_ID.Trim <> "" Then RMAD_ID = RMAD_ID & ","
                    If RMASQ_ID.Trim <> "" Then RMASQ_ID = RMASQ_ID & ","
                    RMAD_ID = RMAD_ID & dr.RMAD_ID.Trim()
                    If dr.IsRMASQ_IDNull = False Then RMASQ_ID = RMASQ_ID & dr.RMASQ_ID.Trim()
                End If
            Next
            RequestQty = dvClientList.Count

            dvRMA_tmp(i)("CurrencyCode") = CurrencyCode
            If CurrencyCode.Trim <> "" Then
                dvRMA_tmp(i)("QUOTE") = QUOTE.ToString("N")
            End If
            dvRMA_tmp(i)("RequestQty") = RequestQty
            dvRMA_tmp(i)("ShippedQty") = ShippedQty
            dvRMA_tmp(i)("RMAD_ID") = RMAD_ID
            dvRMA_tmp(i)("RMASQ_ID") = RMASQ_ID
        Next
        dvRMA_tmp.RowFilter = ""

        Session("_dtRMA_tmp") = dtRMA_tmp
    End Sub

    Protected Sub UI_dvCustomer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvCustomer.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oCheck As CheckBox = e.Row.FindControl("UI_Check")
            _ClientID = _ClientID & "," & oCheck.ClientID

            Dim UI_RMASTATUS As Label = e.Row.FindControl("UI_RMASTATUS")
            Dim UI_imgDetail As ImageButton = e.Row.FindControl("UI_imgDetail")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")

            UI_imgDetail.Visible = False
            UI_cmdEdit.Visible = False
            If Convert.ToInt16(UI_RMASTATUS.Text.Trim) = 40 Then
                UI_imgDetail.Visible = True
                UI_Status.Text = _oLanguage.getText("Common", "064", ctlLanguage.eumType.Tag)
            End If
            If Convert.ToInt16(UI_RMASTATUS.Text.Trim) < 20 Then
                UI_cmdEdit.Visible = True
                oCheck.Visible = False
                UI_Status.Text = _oLanguage.getText("Common", "063", ctlLanguage.eumType.Tag)
            End If
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)


            '加總金額
            Dim UI_Quote As Label = e.Row.FindControl("UI_Quote")
            Dim UI_CurrencyCode As Label = e.Row.FindControl("UI_CurrencyCode")

            If UI_Quote.Text.Trim() <> "" Then
                Me.ViewState("_QuoteTotal") = Convert.ToDecimal(Me.ViewState("_QuoteTotal")) + Convert.ToDecimal(UI_Quote.Text.ToString().Trim())
                Me.ViewState("_CurrencyCode") = UI_CurrencyCode.Text.ToString().Trim()
            End If
        End If


        If e.Row.RowType = DataControlRowType.Footer Then
            Me.lblFooterDesc.Text = "Please click check box before confirm to repair center."

            '總金額
            Dim sQuote As String = ""
            sQuote = _oLanguage.getText("RMA", "172", ctlLanguage.eumType.Tag)
            sQuote = sQuote & " " & Me.ViewState("_CurrencyCode").ToString().Trim()
            sQuote = sQuote & "&nbsp;&nbsp;<u><b>" & Convert.ToDouble(Me.ViewState("_QuoteTotal").ToString().Trim()).ToString("N") & "</b></u>"
            If Convert.ToDouble(Me.ViewState("_QuoteTotal").ToString().Trim()) = 0 Then
                Me.lblQuoteTotal.Visible = False
            End If
            Me.lblQuoteTotal.Text = sQuote
        End If

    End Sub

    Protected Sub UI_dvCustomer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvCustomer.RowCommand

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvCustomer.Rows(iIndex)

            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            'Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
        End If

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvCustomer.Rows(iIndex)

            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
        End If

    End Sub

    Protected Sub UI_dvCustomer_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvCustomer.Sorting

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

        If IsNothing(Session("_dtRMA_tmp")) = False Then
            Dim dtDetail As DataTable = Session("_dtRMA_tmp")
            Dim dvDetail As DataView = dtDetail.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvDetail)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvCustomer.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvCustomer.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvCustomer.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvCustomer.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvCustomer.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvCustomer.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_checkGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False

        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            If Me.UI_dvCustomer.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")
                UI_Check.Checked = sender.Checked
                If sender.Checked = True Then
                    blnFlag = True
                End If
            End If
        Next

        Me.UI_cmdConfirm.Enabled = blnFlag
        Me.UI_cmdCancel.Enabled = blnFlag
    End Sub

    Protected Sub UI_check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False

        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            If Me.UI_dvCustomer.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")
                If UI_Check.Checked = True Then
                    blnFlag = True
                End If
            End If
        Next

        Me.UI_cmdConfirm.Enabled = blnFlag
        Me.UI_cmdCancel.Enabled = blnFlag
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If _ClientID.Trim() <> "" Then
            Me.ucProgressStatus.NotpostBackElement = _ClientID
        End If

    End Sub

    ''' <summary>
    ''' 修改RMAD狀態(50)-->客戶自行確認, 報價
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oCustomer As New ctlRMA.Client
        Dim dtConfirmed As New RmaDTO.ClientQuoted_ConfirmedDataTable

        Try
            For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
                If Me.UI_dvCustomer.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")

                    Dim UI_RMADID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMADID")
                    Dim UI_RMASQID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMASQID")

                    If UI_Check.Checked = True Then
                        If UI_RMADID.Text.Trim <> "" And UI_RMASQID.Text.Trim <> "" Then
                            Dim arrRMADID() As String = UI_RMADID.Text.Trim.Split(",")
                            Dim arrRMASQID() As String = UI_RMASQID.Text.Trim.Split(",")
                            If arrRMADID.Length = arrRMASQID.Length Then
                                For j = 0 To arrRMADID.Length - 1
                                    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.NewClientQuoted_ConfirmedRow

                                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                                    '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                                    dr.RMAD_ID = arrRMADID(j).ToString().Trim()
                                    dr.RMAD_STATUS = 50

                                    dr.RMASQ_ID = arrRMASQID(j).ToString().Trim()
                                    dr.RMASQ_CLIENTAD = Session("_UserID")
                                    dr.RMASQ_CLIENTADNAME = Session("_UserName")
                                    dr.RMASQ_CLIENTDATE = Date.Now
                                    dr.RMASQ_CLIENTCONFIRM = 1     '1.客戶自行確認, 2.業務帶客戶確認

                                    dtConfirmed.AddClientQuoted_ConfirmedRow(dr)
                                Next
                            End If
                        End If

                    End If
                End If
            Next

            oCustomer.ClientQuoted_Confirmed(dtConfirmed)


            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim isSendMail As Boolean = SendMail()

                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Client_Worklist.aspx")
            End If
        End Try
    End Sub

    Private Function SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0


        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try

            dtCustomer = oCustomer.QueryUser(Session("_CustomerID").ToString().Trim(), Session("_UserID").ToString().Trim(), "")
            If dtCustomer.Rows.Count > 0 Then

                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(業務和助理) 及 維修
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


                For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
                    If Me.UI_dvCustomer.Rows(i).RowType = DataControlRowType.DataRow Then
                        Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")
                        Dim RMA_NO As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMANO")

                        If UI_Check.Checked = True Then
                            Dim sSubject As String = _oLanguage.getText("Mail", "019", ctlLanguage.eumType.Mail)
                            Dim sBody As String = _oLanguage.getText("Mail", "020", ctlLanguage.eumType.Mail)
                            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                            If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                                Dim sMailTo As String = ""

                                sSubject = sSubject.Replace("[$RMA No$]", RMA_NO.Text.Trim())
                                sSubject = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())

                                If MailSales.Trim() = "" Then
                                    sBody = sBody.Replace("[$Sales Name$]", "")
                                    sBody = sBody.Replace("/", "")
                                Else
                                    sBody = sBody.Replace("[$Sales Name$]", SalesName)
                                    sMailTo = MailSales.Trim()
                                End If

                                If MailAssistant.Trim() = "" Then
                                    sBody = sBody.Replace("[$Assistant Name$]", "")
                                    sBody = sBody.Replace("/", "")
                                Else
                                    sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                                    If sMailTo.Trim <> "" Then
                                        sMailTo = sMailTo & ","
                                    End If
                                    sMailTo = sMailTo & MailAssistant.Trim()
                                End If

                                sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                                sBody = sBody.Replace("[$RMA No$]", RMA_NO.Text.Trim())
                                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                                If _isDebug = True Then
                                    sMailTo = ConfigurationManager.AppSettings("MailTo")
                                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                                End If
                                '對象(業務和助理)
                                blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)



                                '================================================================================================
                                '對象維修
                                '================================================================================================
                                Dim sSubject_Repair As String = _oLanguage.getText("Mail", "025", ctlLanguage.eumType.Mail)
                                sSubject_Repair = sSubject.Replace("[$RMA No$]", RMA_NO.Text.Trim())
                                sSubject_Repair = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())

                                Dim oRMA As New ctlRMA
                                Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                                Dim Repaire_Name As String = ""                    '維修人員

                                Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(RMA_NO.Text.Trim())
                                For j = 0 To arrRepaire.Count - 1
                                    Dim arrList() As String = arrRepaire(j)

                                    Repaire_Name = arrList(0).Trim()
                                    Repaire_EMAIL = arrList(1).Trim()

                                    Dim sBody_Repair As String = _oLanguage.getText("Mail", "026", ctlLanguage.eumType.Mail)
                                    If Repaire_Name.Trim <> "" Then
                                        sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", Repaire_Name)
                                    Else
                                        sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "")
                                    End If

                                    If Repaire_EMAIL.Trim <> "" Then
                                        sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
                                        sBody_Repair = sBody_Repair.Replace("[$RMA No$]", RMA_NO.Text.Trim())
                                        sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)
                                        sBody_Repair = sBody_Repair.Replace("[$item_Accept$]", "")
                                        sBody_Repair = sBody_Repair.Replace("[$item_Reject$]", "")

                                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                                        If _isDebug = True Then
                                            Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                                        End If
                                        blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_EMAIL, _MailCC)
                                    End If
                                Next
                            End If

                        End If
                    End If
                Next
            End If


        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 取消 RMA 單維修品項
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2011/08/04 START</remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click
        'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim ctlClient As New ctlRMA.Client

        Try
            Dim dtClient As New RmaDTO.Client_CancelDataTable

            For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMANO")
                Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")

                If UI_Check.Checked = True Then
                    Dim dr As RmaDTO.Client_CancelRow = dtClient.NewClient_CancelRow
                    dr.RMA_NO = UI_RMANO.Text.Trim()
                    dr.RMA_CLIENTAD = Session("_UserID")
                    dr.RMA_CLIENTADNAME = Session("_UserName")
                    dr.RMA_CLIENTDATE = Date.Now

                    dtClient.Rows.Add(dr)
                End If
            Next

            If dtClient.Rows.Count > 0 Then
                Call ctlClient.Client_Cancel(dtClient)
            End If

            For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMANO")
                Dim UI_RMAID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMAID")
                Dim UI_RMADID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_RMADID")
                Dim UI_Check As CheckBox = Me.UI_dvCustomer.Rows(i).FindControl("UI_Check")

                ''Loop to notice
                If UI_Check.Checked = True Then
                    SalesCancel_SendMail(UI_RMAID.Text.Trim(), UI_RMANO.Text.Trim(), UI_RMADID.Text.Trim())
                End If
            Next

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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Client_Worklist.aspx")
            End If
        End Try


    End Sub

    Private Function SalesCancel_SendMail(ByVal RMA_ID As String, ByVal RMA_NO As String, ByVal RMAD_ID As String) As Boolean
        Dim i As Integer = 0
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


            dtRepairHead = oRepair.QueryByRepairHead(RMA_ID)
            If dtRepairHead.Rows.Count > 0 Then
                Dim dr As RmaDTO.vwRepair_HeadRow = dtRepairHead.Rows(0)

                sAccountIDText = dr.RMA_CUNO.ToString().Trim()
                sApplicantIDText = dr.RMA_ACCOUNTID.ToString().Trim()
                sApplicantText = dr.RMA_APPLICANT.ToString().Trim()
            End If

            Dim oRMAStatus As New ctlRMA.RMAStatus
            Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            Dim arrRMADID As String() = RMAD_ID.ToString().Trim().Split(",")
            For i = 0 To arrRMADID.Length - 1
                dtStatusPoint = oRMAStatus.QueryPointByDetail(arrRMADID(i))
                If dtStatusPoint.Rows.Count > 0 Then
                    Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)
                    If dr.IsREPAIRQUOTED_ADNull = False Then sRepairIDText = dr.REPAIRQUOTED_AD.Trim()
                End If

                If sRepairIDText.Trim <> "" Then
                    Exit For
                End If
            Next


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
                    If dtAdmin.Rows.Count > 0 Then
                        MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                        SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If
                End If

                Dim MailAssistant As String = ""
                Dim AssistantName As String = ""
                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    If dtAdmin.Rows.Count > 0 Then
                        MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                        AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If
                End If

                Dim MailRepair As String = ""
                Dim RepairName As String = ""
                If sRepairIDText.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(sRepairIDText, "")
                    If dtAdmin.Rows.Count > 0 Then
                        MailRepair = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                        RepairName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If
                End If

                Dim sSubject As String = _oLanguage.getText("Mail", "035", ctlLanguage.eumType.Mail)
                Dim sBody As String = _oLanguage.getText("Mail", "036", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                If MailUser.Trim() <> "" Then
                    sSubject = sSubject.Replace("[$RMA No$]", RMA_NO)

                    sSubject = sSubject.Replace("[$Customer User Name$]", sApplicantText)

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

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = MailUser + "," + MailRepair + "," + MailSales + "," + MailAssistant
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC)
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

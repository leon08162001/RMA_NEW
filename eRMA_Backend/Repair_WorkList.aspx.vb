Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Repair_WorkList
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "3"
    Dim _ClientID As String = ""
    Dim _NavigateBack As Boolean = False

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim iBackIndex As Integer = 0




        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_SerialNo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_isQuery") = "0"

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
            '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Me.ViewState("_Status") = "20,30,50,60"

            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("edate") = Date.Now.ToShortDateString()

            Session("_dtRepairList") = Nothing

            Call setControls()

            If Not Session("_PreviousPage") Is Nothing Then
                Dim oHashtable As System.Collections.Hashtable = Session("_PreviousPage")
                Me.ViewState("_HistoryKey") = oHashtable
                iBackIndex = setNavigateData()
            End If
            Session("_PreviousPage") = Nothing

            Call QueryData(iBackIndex)
        End If

        Me.UI_cmdSubmit.Enabled = False
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        _ClientID = "ctl00_ContentPlaceHolder_UI_dvRepair_ctl01_UI_CheckGroup"
        _ClientID = _ClientID & "," & Me.UI_cmdSearch.ClientID & "," & Me.UI_cmdQuoting.ClientID & "," & Me.UI_cmdRepairing.ClientID


        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())


        Me.ViewState("_StatusList") = "-1,20,30,50,60"
        Dim arrList() As String = Me.ViewState("_StatusList").ToString.Split(",")
        oCommon.getItemStatus(Me.UI_cboStatus, arrList)


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "068", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblQuickSearch.Text = _oLanguage.getText("RMA", "069", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        'Me.UI_cmdCancel.Value = _oLanguage.getText("Common", "010", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)

        Me.UI_dvRepair.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRepair.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvRepair.Columns(3).HeaderText = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
        Me.UI_dvRepair.Columns(4).HeaderText = _oLanguage.getText("Report", "142", ctlLanguage.eumType.Tag)
        Me.UI_dvRepair.Columns(5).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRepair.Columns(6).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

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

        For i = 0 To Me.UI_dvRepair.Rows.Count - 1
            If Me.UI_dvRepair.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvRepair.Rows(i).FindControl("UI_Check")
                UI_Check.Checked = sender.Checked
                If sender.Checked = True Then
                    blnFlag = True
                End If
            End If
        Next

        Me.UI_cmdSubmit.Enabled = blnFlag
    End Sub

    Protected Sub UI_check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False

        For i = 0 To Me.UI_dvRepair.Rows.Count - 1
            If Me.UI_dvRepair.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvRepair.Rows(i).FindControl("UI_Check")
                If UI_Check.Checked = True Then
                    blnFlag = True
                End If
            End If
        Next

        Me.UI_cmdSubmit.Enabled = blnFlag
    End Sub

    ''' <summary>
    ''' Wait for Quoting
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdQuoting_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
        '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Me.ViewState("_RMANo") = ""
        Me.ViewState("_ModelNo") = ""
        Me.ViewState("_SerialNo") = ""
        Me.ViewState("_CustomerName") = ""
        Me.ViewState("_Status") = "20,30"
        Me.ViewState("fdate") = ""
        Me.ViewState("edate") = ""
        Me.ViewState("_isQuery") = "1"

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' Wait for Repairing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdRepairing_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
        '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Me.ViewState("_RMANo") = ""
        Me.ViewState("_ModelNo") = ""
        Me.ViewState("_SerialNo") = ""
        Me.ViewState("_CustomerName") = ""
        Me.ViewState("_Status") = "50,60"
        Me.ViewState("fdate") = ""
        Me.ViewState("edate") = ""
        Me.ViewState("_isQuery") = "2"

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_SerialNo") = Me.UI_txtSerialNumber.Text.Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.Trim()

        If Me.UI_cboStatus.SelectedValue = "-1" Then
            Me.ViewState("_Status") = Me.ViewState("_StatusList").ToString()
        Else
            Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue
        End If

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

        Me.ViewState("_isQuery") = "0"

        Call QueryData(0)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairList As New RmaDTO.vwRepair_WorkListGroupDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim sSerialNo As String = Me.ViewState("_SerialNo").ToString().Trim()

        Dim sCustomerID As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim arrStatus() As String = Me.ViewState("_Status").ToString.Trim().Split(",")
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'isQuery-->查詢方式:0.all，1.Quoting，2.Repairing
        Dim isQuery As Integer = Convert.ToInt16(Me.ViewState("_isQuery"))

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtRepairList = oRepair.QueryByWorkGroup(Session("_RepairCenter").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerID, arrStatus, fdate, edate, isQuery, "RMAD_RMANO asc")

        Call ArrangementData(dtRepairList)
        Session("_dtRepairList") = dtRepairList

        Dim dvRepairList As DataView = dtRepairList.DefaultView
        Me.ViewState("_SortExpression") = "RMAD_RMANO"
        Me.ViewState("_SortDirection") = "asc"
        dvRepairList.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvRepairList, iPageIndex)

    End Sub

    Private Sub RMA_DataBind(ByVal dvRepairList As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRepair.PageSize = _PageSize
        Me.UI_dvRepair.PageIndex = iPageIndex
        Me.UI_dvRepair.DataSource = dvRepairList
        Me.UI_dvRepair.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRepairList As RmaDTO.vwRepair_WorkListGroupDataTable)
        Dim i As Integer = 0

        If dtRepairList.Columns("SeqID") Is Nothing Then
            dtRepairList.Columns.Add("SeqID")
            dtRepairList.Columns.Add("Warranty")
            dtRepairList.Columns.Add("CURRENCYCODE_QUOTE")
            dtRepairList.Columns.Add("QUOTE")
            dtRepairList.Columns.Add("Status")
            dtRepairList.Columns.Add("Assign")
        End If

        For i = 0 To dtRepairList.Rows.Count - 1
            Dim dr As RmaDTO.vwRepair_WorkListGroupRow = dtRepairList.Rows(i)

            dtRepairList.Rows(i)("SeqID") = i + 1

            Dim sStatus As String = "Received:" + dtRepairList.Rows(i)("Received").ToString()
            sStatus += " / " + "WIP:" + dtRepairList.Rows(i)("WIP").ToString()
            sStatus += " / " + "Repairing:" + dtRepairList.Rows(i)("Repairing").ToString()
            sStatus += " / " + "Repaired:" + dtRepairList.Rows(i)("Repaired").ToString()
            dtRepairList.Rows(i)("Status") = sStatus
            'If Convert.ToInt16(dtRepairList.Rows(i)("RMAD_STATUS")) = 60 And dtRepairList.Rows(i)("RMAR_REPAIRAD").ToString().Trim() = "" Then
            'dtRepairList.Rows(i)("Status") = _oLanguage.getText("Common", "068", ctlLanguage.eumType.Status)
            'Else
            'dtRepairList.Rows(i)("Status") = oCommon.ConvertToItemStatusText(Convert.ToInt16(dtRepairList.Rows(i)("RMAD_STATUS")), dtRepairList.Rows(i)("RMAD_ID").ToString().Trim())
            'End If


            '保固日期
            Dim sWarranty As String = ""
            If dr.IsRMAD_WARRANTYNull = False Then
                sWarranty = Convert.ToDateTime(dtRepairList.Rows(i)("RMAD_WARRANTY").ToString()).ToShortDateString()
                dtRepairList.Rows(i)("Warranty") = sWarranty
            Else
                dtRepairList.Rows(i)("Warranty") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRepairList.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtRepairList.Rows(i)("Warranty") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtRepairList.Rows(i)("Warranty") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select
            End If


            '1.有被指派的維修中心 RMAR_COMPNO<>""
            '2.判斷RMA的維修中心, 跟要維修中心不一樣, 代表示此RMA品項被指派到其它維修中心
            dtRepairList.Rows(i)("Assign") = ""
            Dim RMA_COMPNO As String = dtRepairList.Rows(i)("RMA_COMPNO").ToString().Trim()
            Dim RMAR_COMPNO As String = dtRepairList.Rows(i)("RMAR_COMPNO").ToString().Trim()
            If RMA_COMPNO <> RMAR_COMPNO And RMAR_COMPNO <> "" Then
                dtRepairList.Rows(i)("Assign") = dtRepairList.Rows(i)("COMP_NAME")
            End If



            '1.RMAR_REPAIR_ISFILL:是否已填寫維修報價單:0.否, 1.是
            '2.必需要有報價金額
            '3.登入系統維修中心 跟 被指派維修中心 不一樣時
            dtRepairList.Rows(i)("CURRENCYCODE_QUOTE") = ""
            dtRepairList.Rows(i)("QUOTE") = ""
            Dim RMAR_REPAIR_ISFILL As String = dtRepairList.Rows(i)("RMAR_REPAIR_ISFILL").ToString().Trim()

            Dim RMARQ_CURRENCYCODE As String = dtRepairList.Rows(i)("RMARQ_CURRENCYCODE").ToString().Trim()
            Dim RMARQ_QUOTE As String = dtRepairList.Rows(i)("RMARQ_QUOTE").ToString().Trim()

            Dim RMARQ_ASSIGECURRENCYCODE As String = dtRepairList.Rows(i)("RMARQ_ASSIGECURRENCYCODE").ToString().Trim()
            Dim RMARQ_ASSIGEQUOTE As String = dtRepairList.Rows(i)("RMARQ_ASSIGEQUOTE").ToString().Trim()

            If Session("_RepairCenter").ToString().IndexOf(RMAR_COMPNO) <> -1 Then
                '登入系統維修中心 跟 被指派維修中心 一樣時
                If RMAR_REPAIR_ISFILL = "1" And RMARQ_QUOTE <> "" Then
                    dtRepairList.Rows(i)("CURRENCYCODE_QUOTE") = RMARQ_CURRENCYCODE & " "
                    dtRepairList.Rows(i)("QUOTE") = Convert.ToDouble(RMARQ_QUOTE).ToString("N")
                End If

            Else
                '登入系統維修中心 跟 被指派維修中心 不一樣時
                If RMAR_REPAIR_ISFILL = "1" And RMARQ_ASSIGEQUOTE <> "" Then
                    dtRepairList.Rows(i)("CURRENCYCODE_QUOTE") = RMARQ_ASSIGECURRENCYCODE & " "
                    dtRepairList.Rows(i)("QUOTE") = Convert.ToDouble(RMARQ_ASSIGEQUOTE).ToString("N")
                End If
            End If
        Next

    End Sub
    Protected Sub UI_dvRepair_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRepair.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvRepair.PageIndex * Me.UI_dvRepair.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim COMPNO As String = ""
            Dim oCheck As CheckBox = e.Row.FindControl("UI_Check")
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")
            Dim UI_Assign As Label = e.Row.FindControl("UI_Assign")
            Dim UI_RMA_COMPNO As Label = e.Row.FindControl("UI_RMA_COMPNO")
            Dim UI_RMAR_COMPNO As Label = e.Row.FindControl("UI_RMAR_COMPNO")
            Dim UI_RMAR_REPAIR_ISFILL As Label = e.Row.FindControl("UI_RMAR_REPAIR_ISFILL")
            Dim UI_QUOTE As Label = e.Row.FindControl("UI_QUOTE")

            Dim UI_ShowQuote As Label = e.Row.FindControl("UI_ShowQuote")
            Dim UI_ShowRepair As Label = e.Row.FindControl("UI_ShowRepair")

            _ClientID = _ClientID & "," & oCheck.ClientID

            Dim cmdQuoting As LinkButton = e.Row.FindControl("UI_Quoting")
            Dim cmdRepairing As LinkButton = e.Row.FindControl("UI_Repairing")

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(4).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(4).Text) < Convert.ToDateTime(e.Row.Cells(5).Text) Then
                    'e.Row.Cells(4).ForeColor = Drawing.Color.Red
                End If
            End If


            '控制 oCheck 出現時機
            '1.登入系統維修中心 跟 被指派維修中心 不一樣時
            '2.維修品項狀態為 60.Repaired -->CheckBox 就不要出現了
            '3.維修品項狀態為 50.Client Confirmed --->直接可以進 維修狀態, CheckBox 就不要出現了
            '4.維修品項狀態為 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed，此狀態維修人員尚無法做維修處理 -->CheckBox 就不要出現了
            '5.維修品項狀態為 20.Received -->CheckBox 就不要出現了
            If UI_RMAR_COMPNO.Text.Trim() <> "" Then
                COMPNO = UI_RMAR_COMPNO.Text.Trim()
            Else
                COMPNO = UI_RMA_COMPNO.Text.Trim()
            End If
            If Session("_RepairCenter").ToString().IndexOf(COMPNO) < 0 Or UI_Status.Text.Trim() = "20" Or UI_Status.Text.Trim() = "60" Or UI_Status.Text.Trim() = "50" _
                Or (Convert.ToInt16(UI_Status.Text.Trim()) >= 30 And Convert.ToInt16(UI_Status.Text.Trim()) <= 40) Then
                oCheck.Visible = False
            End If

            '有被指派維修中心 但 尚未報價的 CheckBox 就不要出現了
            If UI_RMAR_COMPNO.Text.Trim() <> "" Then
                If UI_RMA_COMPNO.Text.Trim() <> UI_RMAR_COMPNO.Text.Trim() And UI_RMAR_REPAIR_ISFILL.Text.Trim() <> "1" Then
                    oCheck.Visible = False
                End If
            End If


            '控制 維修報價 及 維修品項 出現時機
            'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '品項狀態變更為Sales confirmed，維修人員將不能更新報價
            Select Case UI_Status.Text.ToString().Trim()
                Case "20", "30", "35"
                    'cmdQuoting.Visible = True

                Case "50", "60"
                    'Client Confirmed-->就可以進維修 或 還未做 業務出貨通知(70) Shipped 之前可再進維修
                    'cmdRepairing.Visible = True
            End Select

            If UI_ShowQuote.Text.Trim().ToUpper.Equals("Y") Then
                cmdQuoting.Visible = True
            Else
                cmdQuoting.Visible = False
            End If

            If UI_ShowRepair.Text.Trim().ToUpper.Equals("Y") Then
                cmdRepairing.Visible = True
            Else
                cmdRepairing.Visible = False
            End If


            '控制 維修報價 出現時機
            '1.登入系統維修中心 跟 被指派維修中心 不一樣時
            '2.被指派維修中心 已填寫維修金額, 指派維修中心就不可以再修改(進入Quoting 項目)
            If Session("_RepairCenter").ToString().IndexOf(COMPNO) < 0 And UI_QUOTE.Text.Trim() <> "" Then
                'cmdQuoting.Visible = False
            End If


            '檢核不是自己要維修的品項 disable
            If Session("_RepairCenter").ToString().IndexOf(UI_RMAR_COMPNO.Text.Trim) < 0 Then
                'cmdRepairing.Visible = False
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

                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "DataControlPagerLinkButton".ToLower() Then
                    Dim oLinkButton As LinkButton = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    '_ClientID = _ClientID & "," & oLinkButton.ClientID
                    oLinkButton.Attributes.Add("onclick", "onProgress('Process')")
                End If

            Next
        End If

    End Sub

    Protected Sub UI_dvRepair_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRepair.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()
        If Not Session("_dtRepairList") Is Nothing Then
            Dim dtRepairList As RmaDTO.vwRepair_WorkListGroupDataTable = Session("_dtRepairList")
            Dim dvRepairList As DataView = dtRepairList.DefaultView
            Call RMA_DataBind(dvRepairList, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If

    End Sub

    Protected Sub UI_dvRepair_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRepair.RowCommand

        If e.CommandName = "cmdQuoting" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRepair.Rows(iIndex)

            'Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_ShowQuoteSN As Label = row.FindControl("UI_ShowQuoteSN")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim cmdQuoting As LinkButton = row.FindControl("UI_Quoting")

            '檢核是否還可以再維修報價
            Dim oRMA As New ctlRMA
            'If oRMA.isRepairQuoted(Session("_RepairCenter").ToString(), UI_ShowQuoteSN.Text.Trim()) = True Then
            'Me.UcRepairQuoted.RMADID = UI_ShowQuoteSN.Text.Trim()
            'Me.UcRepairQuoted.RMA_NO = UI_RMANO.Text.Trim()
            'Me.UcRepairQuoted.show = True

            Me.UI_lblPreviousPage_RMADID.Text = UI_ShowQuoteSN.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            'Else
            'Me.ucMessage.showMessageByFailed("此品項已無法再進行維修報價, 請確認...!!")
            'Dim iPageIndex As Integer = Me.UI_dvRepair.PageIndex
            'Call QueryData(iPageIndex)
            'End If

        End If

        If e.CommandName = "cmdRepairing" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRepair.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            'Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_ShowRepairSN As Label = row.FindControl("UI_ShowRepairSN")
            Me.UI_lblPreviousPage_RMADID.Text = UI_ShowRepairSN.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '1.品項狀態 大於 70.Shipped，維修人員就不可以再修改(進入Repairing)

            'Dim UI_Repairing As LinkButton = row.FindControl("UI_Repairing")
            'UI_Repairing.PostBackUrl = "RMARepair_Edit.aspx"
            'Server.Transfer("RMARepair_Edit.aspx")
            'PostBackUrl="~/RMARepair_Edit.aspx"
        End If


        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRepair.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
        End If


    End Sub

    Protected Sub UI_dvRepair_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvRepair.Sorting

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

        If IsNothing(Session("_dtRepairList")) = False Then
            Dim dtRepairList As RmaDTO.vwRepair_WorkListGroupDataTable = Session("_dtRepairList")
            Dim dvRepairList As DataView = dtRepairList.DefaultView
            dvRepairList.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvRepairList, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvRepair.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvRepair.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvRepair.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvRepair.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvRepair.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvRepair.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairStatus As New RmaDTO.tmpRepairStatusDataTable

        Try

            For i = 0 To Me.UI_dvRepair.Rows.Count - 1
                If Me.UI_dvRepair.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvRepair.Rows(i).FindControl("UI_Check")

                    Dim UI_RMANO As Label = Me.UI_dvRepair.Rows(i).FindControl("UI_RMANO")
                    Dim UI_RMADID As Label = Me.UI_dvRepair.Rows(i).FindControl("UI_RMADID")

                    If UI_Check.Checked = True Then
                        Dim dr As RmaDTO.tmpRepairStatusRow = dtRepairStatus.NewtmpRepairStatusRow

                        dr.RMAD_NO = UI_RMANO.Text.Trim()
                        dr.RMAD_ID = UI_RMADID.Text.Trim()

                        dr.Repair_AD = Session("_UserID")
                        dr.Repair_ADName = Session("_UserName")

                        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                        dr.Repair_Status = "60"

                        dtRepairStatus.AddtmpRepairStatusRow(dr)
                    End If
                End If
            Next

            oRepair.Save_RepairStatus(dtRepairStatus)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Repair_WorkList.aspx")
            End If
        End Try

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '========================================================================================================================================================================================================
        'STRAT
        '如果ucRepairQuoted.ascx 有錯誤發生時, 要處理的機制
        If Not Session("UCError") Is Nothing Then
            If Session("UCError").ToString.Trim() <> "" Then
                Me.ucMessage.showMessageByFailed(Session("UCError").ToString().Trim())
                Session("UCError") = ""
            End If
        End If

        If Convert.ToBoolean(Session("UCOK")) = True Then
            Dim iPageIndex As Integer = Me.UI_dvRepair.PageIndex
            Call QueryData(iPageIndex)

            Session("UCOK") = False
        End If

        If _ClientID.Trim() <> "" Then
            Me.ucProgressStatus.NotpostBackElement = _ClientID
        End If
        'END
        '========================================================================================================================================================================================================

        If _NavigateBack = False Then
            'Call AddHistoryPoint()
        End If


    End Sub

#Region "AddHistoryPoint"
    Private Sub AddHistoryPoint()
        Dim iPrevIndex As Integer = Me.UI_dvRepair.PageIndex

        Dim oHashtable As New System.Collections.Hashtable

        oHashtable("_CurrentPage") = iPrevIndex

        oHashtable("_RMANo") = Me.ViewState("_RMANo").ToString().Trim()
        oHashtable("_ModelNo") = Me.ViewState("_ModelNo").ToString()
        oHashtable("_SerialNo") = Me.ViewState("_SerialNo").ToString()
        oHashtable("_CustomerName") = Me.ViewState("_CustomerName").ToString()
        oHashtable("_Status") = Me.ViewState("_Status").ToString()
        oHashtable("fdate") = Me.ViewState("fdate").ToString()
        oHashtable("edate") = Me.ViewState("edate").ToString()

        For Each de As DictionaryEntry In oHashtable
            History.AddHistoryPoint(de.Key, de.Value)
        Next

        Me.ViewState("_HistoryKey") = oHashtable
    End Sub
#End Region

#Region "History_Navigate"
    Protected Sub History_Navigate(ByVal sender As Object, ByVal args As Microsoft.Web.Preview.UI.Controls.HistoryEventArgs) Handles History.Navigate
        Dim i As Integer = 0
        Dim iBackIndex As Integer = 0

        If args.State.Count > 0 Then
            _NavigateBack = True
            iBackIndex = setNavigateData(args)
        End If

        If Not Session("_dtRepairList") Is Nothing Then
            Dim dtRepairList As RmaDTO.vwRepair_WorkListGroupDataTable = Session("_dtRepairList")
            Dim dvRepairList As DataView = dtRepairList.DefaultView
            Call RMA_DataBind(dvRepairList, iBackIndex)

        Else
            Call QueryData(iBackIndex)
        End If

    End Sub
#End Region

    Private Function setNavigateData(ByVal args As Microsoft.Web.Preview.UI.Controls.HistoryEventArgs) As Integer
        Dim iBackIndex As Integer = 0
        If Not Me.ViewState("_HistoryKey") Is Nothing Then
            Dim oHashtable As System.Collections.Hashtable = Me.ViewState("_HistoryKey")

            If oHashtable.Count > 0 Then
                For Each de As DictionaryEntry In oHashtable
                    If de.Key.ToString.ToLower = "_CurrentPage".ToLower() Then
                        iBackIndex = args.State(de.Key.ToString)
                    Else
                        Me.ViewState(de.Key) = args.State(de.Key.ToString)
                    End If
                Next
            End If

        End If

        Return iBackIndex
    End Function

    Private Function setNavigateData() As Integer
        Dim iBackIndex As Integer = 0
        If Not Me.ViewState("_HistoryKey") Is Nothing Then
            Dim oHashtable As System.Collections.Hashtable = Me.ViewState("_HistoryKey")

            If oHashtable.Count > 0 Then
                For Each de As DictionaryEntry In oHashtable
                    If de.Key.ToString.ToLower = "_CurrentPage".ToLower() Then
                        iBackIndex = de.Value
                    Else
                        Me.ViewState(de.Key) = de.Value
                    End If
                Next
            End If

        End If

        Return iBackIndex
    End Function

#Region "getHistoryByList"
    Public ReadOnly Property getHistoryByList() As System.Collections.Hashtable
        Get
            Dim oHashtable As New System.Collections.Hashtable
            If Not Me.ViewState("_HistoryKey") Is Nothing Then
                oHashtable = Me.ViewState("_HistoryKey")
            End If

            Return oHashtable
        End Get
    End Property
#End Region

End Class

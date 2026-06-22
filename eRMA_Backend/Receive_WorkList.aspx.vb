Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Receive_WorkList
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    'Dim _PageSize As String = "1"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load





        If Me.IsPostBack = False Then



            Me.ViewState("_RMANo") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_SerialNo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Status") = "-1"
            'Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            'Me.ViewState("edate") = Date.Now.ToShortDateString()
            Me.ViewState("fdate") = ""
            Me.ViewState("edate") = ""

            Session("_dtReceiveList") = Nothing

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
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblQuickRMA.Text = _oLanguage.getText("RMA", "039", ctlLanguage.eumType.Tag)

        Me.UI_cmdQuery.Text = _oLanguage.getText("Common", "021", ctlLanguage.eumType.Command)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "040", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(6).HeaderText = _oLanguage.getText("RMA", "041", ctlLanguage.eumType.Tag)

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oReceived As New ctlRMA.Received
        Dim dtReceiveList As New RmaDTO.vwReceiveListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim sSerialNo As String = Me.ViewState("_SerialNo").ToString().Trim()

        Dim sCustomerID As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim Status As Integer = Convert.ToInt16(Me.ViewState("_Status"))
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtReceiveList = oReceived.QueryByWork(Session("_RepairCenter").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerID, Status, fdate, edate, "RMA_No desc")

        Call ArrangementData(dtReceiveList)
        Session("_dtReceiveList") = dtReceiveList

        Dim dvReceiveList As DataView = dtReceiveList.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
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

    Private Sub ArrangementData(ByVal dtReceiveList As RmaDTO.vwReceiveListDataTable)
        Dim i As Integer = 0

        If dtReceiveList.Columns("SeqID") Is Nothing Then
            dtReceiveList.Columns.Add("SeqID")
            dtReceiveList.Columns.Add("Status")
            dtReceiveList.Columns.Add("ReceivedTotal")
        End If

        For i = 0 To dtReceiveList.Rows.Count - 1
            dtReceiveList.Rows(i)("SeqID") = i + 1
            dtReceiveList.Rows(i)("Status") = oCommon.ConvertToStatusText(Convert.ToInt16(dtReceiveList.Rows(i)("RMA_STATUS")))
            dtReceiveList.Rows(i)("ReceivedTotal") = dtReceiveList.Rows(i)("RecvCount").ToString().Trim() & "  /  " & dtReceiveList.Rows(i)("TotalCount").ToString().Trim()
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

        If Not Session("_dtReceiveList") Is Nothing Then
            Dim dtReceiveList As RmaDTO.vwReceiveListDataTable = Session("_dtReceiveList")
            Dim dvReceiveList As DataView = dtReceiveList.DefaultView
            Call RMA_DataBind(dvReceiveList, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        Dim row As GridViewRow

        If e.CommandName = "cmdDetail" Then

        End If

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANo As Label = row.FindControl("UI_RMANo")
            Dim UI_CUNO As Label = row.FindControl("UI_CUNO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANo.Text.Trim()
            Me.UI_lblPreviousPage_CUNO.Text = UI_CUNO.Text.Trim()
        End If

        If e.CommandName = "cmdDel" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANo As Label = row.FindControl("UI_RMANo")
            Call Delete(UI_RMAID.Text.ToString().Trim(), UI_RMANo.Text.ToString().Trim())
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

        If IsNothing(Session("_dtReceiveList")) = False Then
            Dim dtRMA_tmp As DataTable = Session("_dtReceiveList")
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
    ''' <param name="sRMADID"></param>
    ''' <param name="sRMADNo"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal sRMADID As String, ByVal sRMADNo As String)
        Dim oReceived As New ctlRMA.Received
        Dim dtReceiveList As New RmaDTO.vwReceiveListDataTable
        Dim oAdmin As New ctlAdmin

        oReceived.DeleteRMA(sRMADID, sRMADNo, Session("_UserID").ToString(), Session("_UserName").ToString())

        oAdmin.addLog(sRMADNo.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMADNo.Trim() & " is deleted.")

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

    ''' <summary>
    ''' 快速查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdQuery.Click
        Me.ViewState("_RMANo") = Me.UI_txtQuickRMA.Text.Trim()
        Me.ViewState("_ModelNo") = ""
        Me.ViewState("_SerialNo") = ""
        Me.ViewState("_CustomerName") = ""

        Me.ViewState("_Status") = "-1"
        Me.ViewState("fdate") = ""
        Me.ViewState("edate") = ""

        Call QueryData(0)
    End Sub

End Class

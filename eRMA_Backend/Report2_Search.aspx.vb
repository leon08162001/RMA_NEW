Imports DataService
Imports DefLanguage

Partial Class Report2_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SerialNo") = ""

            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtReport") = Nothing

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
        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())

        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_lblSerialNo.Text = _oLanguage.getText("Report", "150", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.vwRpt_FrequencyDataTable

        Dim SerialNo As String = Me.ViewState("_SerialNo").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        dtReport = ctlReport.QueryFrequency(SerialNo, fdate, edate)

        Call Request_DataBind(dtReport, iPageIndex)
    End Sub

    Private Sub Request_DataBind(ByVal dtReport As ReportDTO.vwRpt_FrequencyDataTable, ByVal iPageIndex As Integer)

        Session("_dtReport") = dtReport

        Me.UI_dvReport.PageSize = _PageSize
        Me.UI_dvReport.PageIndex = iPageIndex
        Me.UI_dvReport.DataSource = dtReport.DefaultView
        Me.UI_dvReport.DataBind()
    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "159", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "179", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "180", ctlLanguage.eumType.Tag)
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_HrefDetail As LinkButton = e.Row.FindControl("UI_HrefDetail")
            UI_HrefDetail.Text = _oLanguage.getText("Report", "181", ctlLanguage.eumType.Tag)
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

    Protected Sub UI_dvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvReport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            Dim dtReport As ReportDTO.vwRpt_FrequencyDataTable = Session("_dtReport")
            Call Request_DataBind(dtReport, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_SerialNo") = Me.UI_txtSeriallNo.Text.Trim()

        Me.ViewState("_fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("_fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("_edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

    Protected Sub UI_dvReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvReport.RowCommand

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvReport.Rows(iIndex)
            Dim UI_SerialNo As Label = row.FindControl("UI_SerialNo")
            Dim UI_ModelNo As Label = row.FindControl("UI_ModelNo")

            Me.UI_lblPreviousPage_SerialNo.Text = UI_SerialNo.Text.ToString().Trim()
            Me.UI_lblPreviousPage_ModelNo.Text = UI_ModelNo.Text.ToString().Trim()

            Me.UI_lblFdate.Text = Me.ViewState("_fdate").ToString().Trim()
            Me.UI_lblEdate.Text = Me.ViewState("_edate").ToString().Trim()
        End If

    End Sub

End Class

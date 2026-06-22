Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Report5_Search
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_CompanyName") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_Warranty") = "-1"
            Me.ViewState("_Repair") = "-1"

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
        Dim TagText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(True, Me.UI_cboRepairCenter, TagText)
        oCommon.getWarranty(Me.UI_cboWarranty)

        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())

        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_lblCompanyName.Text = _oLanguage.getText("Report", "100", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Report", "102", ctlLanguage.eumType.Tag)
        Me.UI_lblWarranty.Text = _oLanguage.getText("Report", "103", ctlLanguage.eumType.Tag)
        Me.UI_lblDurationDate.Text = _oLanguage.getText("Report", "105", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.vwRpt_RCMonthlyByRepairerDataTable

        Dim ModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()

        '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
        Dim Warranty As String = Me.ViewState("_Warranty").ToString().Trim()
        Dim Repair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        dtReport = ctlReport.QueryRepairCenterMonthly(Session("_CustomerID").ToString().Trim(), ModelNo, Warranty, Repair, fdate, edate, "")
        dtReport.Columns.Add("isShow")    '1.Data Item  2. subTotal Item
        dtReport.Columns.Add("PerformedQTY_Text")

        Dim dvReport As DataView = dtReport.DefaultView
        Call ArrangementData(dvReport)


        Session("_dtReport") = dtReport
        Call Request_DataBind(dvReport, iPageIndex)
    End Sub

    Private Sub Request_DataBind(ByVal dvReport As DataView, ByVal iPageIndex As Integer)
        'Me.UI_dvReport.PageSize = _PageSize
        'Me.UI_dvReport.PageIndex = iPageIndex
        Me.UI_dvReport.DataSource = dvReport
        Me.UI_dvReport.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dvReport As DataView)
        Dim i As Integer = 0
        Dim subTotal_RequestQTY As Integer = 0
        Dim subTotal_CanceledQTY As Integer = 0
        Dim subTotal_ReceivedQTY As Integer = 0
        Dim subTotal_QuotedQTY As Integer = 0
        Dim subTotal_RepairedQTY As Integer = 0
        Dim subTotal_PerformedQTY As Integer = 0
        Dim subTotal_WarrantyQTY As Integer = 0
        Dim subTotal_WarrantyOutQTY As Integer = 0

        For i = 0 To dvReport.Count - 1
            dvReport(i)("isShow") = "1"
            'dvReport(i)("PerformedQTY_Text") = (Convert.ToInt16(dvReport(i)("PerformedQTY")) * 100).ToString() & "%"

            subTotal_RequestQTY = subTotal_RequestQTY + Convert.ToInt16(dvReport(i)("RequestQTY"))
            subTotal_CanceledQTY = subTotal_CanceledQTY + Convert.ToInt16(dvReport(i)("CanceledQTY"))
            subTotal_ReceivedQTY = subTotal_ReceivedQTY + Convert.ToInt16(dvReport(i)("ReceivedQTY"))
            subTotal_QuotedQTY = subTotal_QuotedQTY + Convert.ToInt16(dvReport(i)("QuotedQTY"))
            subTotal_RepairedQTY = subTotal_RepairedQTY + Convert.ToInt16(dvReport(i)("RepairedQTY"))
            subTotal_PerformedQTY = subTotal_PerformedQTY + Convert.ToInt16(dvReport(i)("PerformedQTY"))
            subTotal_WarrantyQTY = subTotal_WarrantyQTY + Convert.ToInt16(dvReport(i)("WarrantyQTY"))
            subTotal_WarrantyOutQTY = subTotal_WarrantyOutQTY + Convert.ToInt16(dvReport(i)("WarrantyOutQTY"))
        Next


        If dvReport.Count > 0 Then
            Dim dr As DataRow = dvReport.Table.NewRow
            dr("RMA_COMPNO") = "zzz"
            dr("COMP_NAME") = _oLanguage.getText("Report", "177", ctlLanguage.eumType.Tag)

            dr("RequestYM") = "zzz"

            dr("RMAR_REPAIRAD") = "zzz"
            dr("RMAR_REPAIRADNAME") = ""

            dr("RequestQTY") = subTotal_RequestQTY
            dr("CanceledQTY") = subTotal_CanceledQTY
            dr("ReceivedQTY") = subTotal_ReceivedQTY
            dr("QuotedQTY") = subTotal_QuotedQTY
            dr("RepairedQTY") = subTotal_RepairedQTY

            dr("PerformedQTY") = subTotal_RepairedQTY / (subTotal_RequestQTY - subTotal_CanceledQTY)
            'dr("PerformedQTY_Text") = (Convert.ToInt16(subTotal_PerformedQTY) * 100).ToString() & "%"

            dr("WarrantyQTY") = subTotal_WarrantyQTY
            dr("WarrantyOutQTY") = subTotal_WarrantyOutQTY

            dr("isShow") = 2
            dvReport.Table.Rows.Add(dr)
        End If

    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "118", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "145", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "122", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "169", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "170", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "171", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Report", "172", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("Report", "173", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("Report", "174", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("Report", "175", ctlLanguage.eumType.Tag)
            e.Row.Cells(11).Text = _oLanguage.getText("Report", "176", ctlLanguage.eumType.Tag)
        End If



        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_isShow As Label = e.Row.FindControl("UI_isShow")
            Select Case UI_isShow.Text.Trim()
                Case "2"
                    e.Row.BackColor = Drawing.Color.Silver
                    e.Row.Cells.Remove(e.Row.Cells(2))
                    e.Row.Cells.Remove(e.Row.Cells(0))
                    e.Row.Cells(0).ColumnSpan = 3
                    e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            End Select

        End If


        If e.Row.RowType = DataControlRowType.Pager Then
        End If

    End Sub

    'Protected Sub UI_dvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvReport.PageIndexChanging
    '    Dim iPageIndex As Integer = e.NewPageIndex.ToString()

    '    If Not Session("_dtReport") Is Nothing Then
    '        Dim dtReport As ReportDTO.vwRpt_RCMonthlyByRepairerDataTable = Session("_dtReport")
    '        Call Request_DataBind(dtReport.DefaultView, iPageIndex)

    '    Else
    '        Call QueryData(iPageIndex)
    '    End If
    'End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_CompanyName") = Me.UI_txtCompanyName.Text.Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_Warranty") = Me.UI_cboWarranty.SelectedValue.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()

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

End Class




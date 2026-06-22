Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Report9_Search
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_Warranty") = "-1"
            Me.ViewState("_Repair") = "-1"
            Me.ViewState("_Status") = "-1"

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

        '身分:1.客戶, 2.公司(維修中心)
        If Session("_Identity").ToString().Trim() = "2" Then
            oCommon.getRepairCenteryByDropDownList(True, Me.UI_cboRepairCenter, TagText)
        Else
            oCommon.getRepairCenteryByDropDownList(Session("_RepairID").ToString().Trim(), Me.UI_cboRepairCenter, TagText, False)
        End If
        oCommon.getWarranty(Me.UI_cboWarranty)
        oCommon.getItemStatus(Me.UI_cboStatus)

        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())

        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_lblModelNo.Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Report", "102", ctlLanguage.eumType.Tag)
        Me.UI_lblWarranty.Text = _oLanguage.getText("Report", "103", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Report", "104", ctlLanguage.eumType.Tag)
        Me.UI_lblDurationDate.Text = _oLanguage.getText("Report", "105", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.vwRtp_FailureDataTable

        Dim ModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()

        '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
        Dim Warranty As String = Me.ViewState("_Warranty").ToString().Trim()
        Dim Repair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        dtReport = ctlReport.QueryFailureReason(Session("_CustomerID").ToString().Trim(), ModelNo, Warranty, Repair, Status, fdate, edate, "RMAD_MODELNO asc, FAR_NO asc")
        dtReport.Columns.Add("SeqID")
        dtReport.Columns.Add("isShow")  '1.顯示Group第一筆, 2:不顯示, 3.顯示SubtotalQuanty
        Session("_dtReport") = dtReport
        Dim dvReport As DataView = dtReport.DefaultView

        Call ArrangementData(dvReport)
        Call Request_DataBind(dvReport, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dvReport As DataView)
        Dim i As Integer = 0
        Dim sGroupBY As String = Nothing
        Dim iSubtotalQuanty As Integer = 0
        Me.ViewState("_TotalQuanty") = 0

        Dim dtReport_Tmp As DataTable = dvReport.Table.Copy
        Dim dvReport_Tmp As DataView = dtReport_Tmp.DefaultView

        dvReport.Sort = "RMAD_MODELNO asc, FAR_NO asc"
        For i = 0 To dvReport.Count - 1
            Dim RMAD_MODELNO As String = dvReport(i)("RMAD_MODELNO").ToString().Trim()
            dvReport(i)("SeqID") = i + 1
            dvReport(i)("isShow") = 2
            If i = 0 Then
                sGroupBY = RMAD_MODELNO
                dvReport(i)("isShow") = 1
            Else
                If sGroupBY.ToString().ToLower() <> RMAD_MODELNO.ToLower() Then
                    dvReport(i)("isShow") = 1
                    sGroupBY = RMAD_MODELNO
                End If
            End If
        Next


        sGroupBY = Nothing
        dvReport_Tmp.Sort = "RMAD_MODELNO asc, FAR_NO asc"
        For i = 0 To dvReport_Tmp.Count - 1
            Dim RMAD_MODELNO As String = dvReport_Tmp(i)("RMAD_MODELNO").ToString().Trim()
            Dim iCount As Integer = Convert.ToInt16(dvReport_Tmp(i)("iCount").ToString().Trim())

            If i = 0 Then
                sGroupBY = RMAD_MODELNO
                iSubtotalQuanty = iCount

            Else
                If sGroupBY.ToString().ToLower() <> RMAD_MODELNO.ToLower() Then
                    Dim dr As DataRow = dvReport.Table.NewRow
                    dr("FAR_NO") = "zzz"
                    dr("RMAD_MODELNO") = sGroupBY.ToString.Trim()
                    dr("MODELNAME") = ""
                    dr("FAR_REASON") = _oLanguage.getText("Report", "141", ctlLanguage.eumType.Tag)
                    Me.ViewState("_TotalQuanty") = Convert.ToInt16(Me.ViewState("_TotalQuanty")) + Convert.ToInt16(iSubtotalQuanty)
                    dr("iCount") = Convert.ToInt16(iSubtotalQuanty)
                    dr("isShow") = 3
                    dvReport.Table.Rows.Add(dr)

                    sGroupBY = RMAD_MODELNO
                    iSubtotalQuanty = iCount
                Else
                    iSubtotalQuanty = Convert.ToInt16(iSubtotalQuanty) + iCount
                End If
            End If

            If i = dvReport_Tmp.Count - 1 Then
                Dim dr As DataRow = dvReport.Table.NewRow
                dr("FAR_NO") = "zzz"
                dr("RMAD_MODELNO") = sGroupBY.ToString.Trim()
                dr("MODELNAME") = ""
                dr("FAR_REASON") = _oLanguage.getText("Report", "141", ctlLanguage.eumType.Tag)
                Me.ViewState("_TotalQuanty") = Convert.ToInt16(Me.ViewState("_TotalQuanty")) + Convert.ToInt16(iSubtotalQuanty)
                dr("iCount") = Convert.ToInt16(iSubtotalQuanty)
                dr("isShow") = 3
                dvReport.Table.Rows.Add(dr)
            End If
        Next

    End Sub

    Private Sub Request_DataBind(ByVal dvReport As DataView, ByVal iPageIndex As Integer)

        'Me.UI_dvReport.PageSize = _PageSize
        'Me.UI_dvReport.PageIndex = iPageIndex
        dvReport.Sort = "RMAD_MODELNO asc, isShow asc"
        Me.UI_dvReport.DataSource = dvReport
        Me.UI_dvReport.DataBind()
    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "138", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "139", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "136", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "137", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "140", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_FAR_REASON As Label = e.Row.FindControl("UI_FAR_REASON")
            Dim UI_isShow As Label = e.Row.FindControl("UI_isShow")

            Select Case UI_isShow.Text.Trim()
                Case "2"
                    e.Row.Cells(1).Text = ""
                    e.Row.Cells(2).Text = ""

                Case "3"
                    e.Row.BackColor = Drawing.Color.Silver

                    e.Row.Cells.Remove(e.Row.Cells(0))
                    e.Row.Cells.Remove(e.Row.Cells(0))
                    e.Row.Cells.Remove(e.Row.Cells(0))
                    e.Row.Cells.Remove(e.Row.Cells(0))
                    e.Row.Cells(0).ColumnSpan = 5

                    e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Right
                    UI_FAR_REASON.Text = UI_FAR_REASON.Text.Trim() & "&nbsp;&nbsp;"
            End Select
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells.Remove(e.Row.Cells(0))
            e.Row.Cells.Remove(e.Row.Cells(0))
            e.Row.Cells.Remove(e.Row.Cells(0))
            e.Row.Cells.Remove(e.Row.Cells(0))
            e.Row.Cells(0).ColumnSpan = 5

            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Right

            Dim UI_Footer As Label = e.Row.FindControl("UI_Footer")
            Dim UI_Footer_TotalQuanty As Label = e.Row.FindControl("UI_Footer_TotalQuanty")

            UI_Footer.Text = _oLanguage.getText("Report", "142", ctlLanguage.eumType.Tag).ToString().Trim() & "&nbsp;&nbsp;"
            UI_Footer_TotalQuanty.Text = Me.ViewState("_TotalQuanty").ToString().Trim()
        End If

    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_Warranty") = Me.UI_cboWarranty.SelectedValue.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()

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

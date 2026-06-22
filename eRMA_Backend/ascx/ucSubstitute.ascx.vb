Imports System.Data
Imports DataService
Imports DefLanguage



Partial Class ascx_ucSubstitute
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _RepairBOM_No As String = ConfigurationSettings.AppSettings("RepairBOM_No")




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub


    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
    End Sub







    Private Sub QueryData(ByVal iSubstitute As Integer, ByVal bmb01 As String, ByVal bmb03 As String, ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtSubstitute As New ReportDTO.Rpt_SubstituteDataTable

        dtSubstitute = ctlReport.QuerySubstitute(iSubstitute, bmb01, bmb03, _RepairBOM_No)

        Call Report_DataBind(dtSubstitute, iPageIndex)
    End Sub





    Private Sub Report_DataBind(ByVal dtSubstitute As ReportDTO.Rpt_SubstituteDataTable, ByVal iPageIndex As Integer)
        Session("_dtSubstitute") = dtSubstitute

        Me.UI_dvSubstitute.PageSize = _PageSize
        Me.UI_dvSubstitute.PageIndex = iPageIndex
        Me.UI_dvSubstitute.DataSource = dtSubstitute.DefaultView
        Me.UI_dvSubstitute.DataBind()
    End Sub

    Protected Sub UI_dvSubstitute_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSubstitute.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            If Convert.ToInt16(Me.ViewState("_iSubstitute")) = 1 Then
                e.Row.Cells(1).Text = _oLanguage.getText("Report", "165", ctlLanguage.eumType.Tag)
            End If
            If Convert.ToInt16(Me.ViewState("_iSubstitute")) = 2 Then
                e.Row.Cells(1).Text = _oLanguage.getText("Report", "166", ctlLanguage.eumType.Tag)
            End If

            e.Row.Cells(2).Text = _oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvSubstitute.PageIndex * Me.UI_dvSubstitute.PageSize) + (e.Row.RowIndex + 1).ToString()
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


    Protected Sub UI_dvSubstitute_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSubstitute.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            Dim dtSubstitute As ReportDTO.Rpt_SubstituteDataTable = Session("_dtSubstitute")
            Call Report_DataBind(dtSubstitute, iPageIndex)

        Else

            Dim iSubstitute As Integer = Convert.ToInt16(Me.ViewState("_iSubstitute"))
            Dim bmb01 As String = Me.ViewState("_bmb01").ToString().Trim()
            Dim bmb03 As String = Me.ViewState("_bmb03").ToString().Trim()
            Call QueryData(iSubstitute, bmb01, bmb03, 0)
        End If

        Me.ajModalProgress.Show()
    End Sub








    ''' <summary>
    ''' 設定是否要顯示
    ''' </summary>
    ''' <param name="iSubstitute">"1"-->替代, "2"-->取代</param>
    ''' <param name="bmb01">上階料件</param>
    ''' <param name="bmb03">下階料件</param>
    ''' <remarks></remarks>
    Public Sub show(ByVal iSubstitute As Integer, ByVal bmb01 As String, ByVal bmb03 As String)
        Session("_dtSubstitute") = Nothing

        Me.ViewState("_iSubstitute") = iSubstitute
        Me.ViewState("_bmb01") = bmb01
        Me.ViewState("_bmb03") = bmb03

        Call QueryData(iSubstitute, bmb01, bmb03, 0)

        Me.ajModalProgress.Show()
    End Sub





End Class

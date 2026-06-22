Imports DataService
Imports DefLanguage

Partial Class FAQ_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtFAQ") = Nothing
            Call setControls()
            Call QueryData(0)
        End If
    End Sub
#End Region

#Region "setControls"
    Private Sub setControls()
        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(Me.UI_cboCategory1, Category1Text)

        Dim Category2Text As String = _oLanguage.getText("FAQ", "033", ctlLanguage.eumType.Tag)
        oCommon.getFQASubClassByDropDownList(Me.UI_cboCategory1.SelectedValue.ToString().Trim(), Me.UI_cboCategory2, Category2Text)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("FAQ", "032", ctlLanguage.eumType.Tag)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
    End Sub
#End Region

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oFAQ As New ctlFAQ.FAQ
        Dim dtFAQ As New FaqDTO.FAQDataTable

        Dim FAQC_ID As String = Me.UI_cboCategory1.SelectedValue.ToString().Trim()
        Dim FAQSC_ID As String = Me.UI_cboCategory2.SelectedValue.ToString().Trim()
        Dim Question As String = Me.UI_txtQuestion.Text.ToString().Trim()

        dtFAQ = oFAQ.QueryDisplay(FAQC_ID, FAQSC_ID, Question, "FAQ_LUSTMP desc")

        'Call ArrangementData(dtFAQ)

        Session("_dtFAQ") = dtFAQ

        Me.UI_dvFAQ.PageSize = _PageSize
        Me.UI_dvFAQ.PageIndex = iPageIndex
        Me.UI_dvFAQ.DataSource = dtFAQ.DefaultView
        Me.UI_dvFAQ.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtFAQ As FaqDTO.FAQDataTable)
        Dim i As Integer = 0

        If dtFAQ.Columns("ANSWER") Is Nothing Then
            dtFAQ.Columns.Add("ANSWER")
        End If

        For i = 0 To dtFAQ.Rows.Count - 1
            '            dtFAQ.Rows(i)("ANSWER") = dtFAQ.Rows(i)("FAQ_ANSWER").ToString().Replace(vbCrLf, "<br>")
        Next
    End Sub

    Protected Sub UI_dvFAQ_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvFAQ.RowDataBound
        Dim i As Integer = 0

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

    Protected Sub UI_dvFAQ_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvFAQ.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtFAQ") Is Nothing Then
            Dim dtFAQ As FaqDTO.FAQDataTable = Session("_dtFAQ")

            Me.UI_dvFAQ.PageSize = _PageSize
            Me.UI_dvFAQ.PageIndex = iPageIndex
            Me.UI_dvFAQ.DataSource = dtFAQ.DefaultView
            Me.UI_dvFAQ.DataBind()

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' FAQ大類下拉式
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboCategory1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboCategory1.SelectedIndexChanged
        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQASubClassByDropDownList(Me.UI_cboCategory1.SelectedValue.ToString().Trim(), Me.UI_cboCategory2, Category2Text)
    End Sub

End Class

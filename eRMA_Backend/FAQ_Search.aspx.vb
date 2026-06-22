Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class FAQ_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"
    Dim _oLanguage As New ctlLanguage

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
            Call QueryData(0)
        End If
    End Sub
#End Region

#Region "setDefault"
    Private Sub setDefault()

        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(Me.UI_cboCategory1, Category1Text)

        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQASubClassByDropDownList(Me.UI_cboCategory1.SelectedValue.ToString().Trim(), Me.UI_cboCategory2, Category2Text)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("FAQ", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQSearching.Text = _oLanguage.getText("FAQ", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblCategory.Text = _oLanguage.getText("FAQ", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblQuestion.Text = _oLanguage.getText("FAQ", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQTittle.Text = _oLanguage.getText("FAQ", "006", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Me.UI_dvFAQ.Columns(1).HeaderText = _oLanguage.getText("FAQ", "004", ctlLanguage.eumType.Tag)
        Me.UI_dvFAQ.Columns(2).HeaderText = _oLanguage.getText("FAQ", "007", ctlLanguage.eumType.Tag)
        Me.UI_dvFAQ.Columns(3).HeaderText = _oLanguage.getText("FAQ", "008", ctlLanguage.eumType.Tag)
        Me.UI_dvFAQ.Columns(4).HeaderText = _oLanguage.getText("FAQ", "009", ctlLanguage.eumType.Tag)

    End Sub
#End Region

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

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oFAQ As New ctlFAQ.FAQ
        Dim dtFAQ As New FaqDTO.FAQDataTable

        Dim FAQC_ID As String = Me.UI_cboCategory1.SelectedValue.ToString().Trim()
        Dim FAQSC_ID As String = Me.UI_cboCategory2.SelectedValue.ToString().Trim()
        Dim Question As String = Me.UI_txtQuestion.Text.ToString().Trim()

        dtFAQ = oFAQ.Query(FAQC_ID, FAQSC_ID, Question, "FAQ_LUSTMP desc")
        Call ArrangementData(dtFAQ)

        Session("_dtTMP") = dtFAQ
        Dim dvFAQ As DataView = dtFAQ.DefaultView
        Me.ViewState("_SortExpression") = "FAQ_ISSUEDATE"
        Me.ViewState("_SortDirection") = "desc"
        dvFAQ.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call FAQ_DataBind(dvFAQ, iPageIndex)
    End Sub

    Private Sub FAQ_DataBind(ByVal dvFAQ As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvFAQ.PageSize = _PageSize
        Me.UI_dvFAQ.PageIndex = iPageIndex
        Me.UI_dvFAQ.DataSource = dvFAQ
        Me.UI_dvFAQ.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtFAQ As FaqDTO.FAQDataTable)
        Dim i As Integer = 0

        If dtFAQ.Columns("SeqID") Is Nothing Then
            dtFAQ.Columns.Add("SeqID")
            dtFAQ.Columns.Add("QUESTION")
            dtFAQ.Columns.Add("ANSWER")
        End If

        For i = 0 To dtFAQ.Rows.Count - 1
            dtFAQ.Rows(i)("SeqID") = i + 1

            Dim FAQ_QUESTION As String = dtFAQ.Rows(i)("FAQ_QUESTION").ToString().Trim()
            Dim FAQ_ANSWER As String = dtFAQ.Rows(i)("FAQ_ANSWER").ToString().Trim()

            If FAQ_QUESTION.Length > 50 Then
                dtFAQ.Rows(i)("QUESTION") = dtFAQ.Rows(i)("FAQ_QUESTION").ToString().Substring(0, 50) & "..."
            Else
                dtFAQ.Rows(i)("QUESTION") = FAQ_QUESTION
            End If

            If FAQ_ANSWER.Length > 50 Then
                dtFAQ.Rows(i)("ANSWER") = dtFAQ.Rows(i)("FAQ_ANSWER").ToString().Substring(0, 50) & "..."
            Else
                dtFAQ.Rows(i)("ANSWER") = FAQ_ANSWER
            End If

        Next
    End Sub

    Protected Sub UI_dvFAQ_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvFAQ.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvFAQ.PageIndex * Me.UI_dvFAQ.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim imgEdit As Button = e.Row.FindControl("imgEdit")
            imgEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
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

    Protected Sub UI_dvFAQSubClass_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvFAQ.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_dvFAQ.Rows(iIndex)

            Dim FAQID As Label = row.FindControl("UI_FAQID")
            Me.UI_lblPreviousPage_FAQID.Text = FAQID.Text.Trim
        End If

    End Sub

    Protected Sub UI_dvFAQ_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvFAQ.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtTMP") Is Nothing Then
            Dim dtFAQ As FaqDTO.FAQDataTable = Session("_dtTMP")
            Dim dvFAQ As DataView = dtFAQ.DefaultView

            Call FAQ_DataBind(dvFAQ, iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvFAQ_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvFAQ.Sorting

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

        If IsNothing(Session("_dtTMP")) = False Then
            Dim dtFAQ As DataTable = Session("_dtTMP")
            Dim dvFAQ As DataView = dtFAQ.DefaultView
            dvFAQ.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call FAQ_DataBind(dvFAQ, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvFAQ.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvFAQ.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvFAQ.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvFAQ.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvFAQ.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvFAQ.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
    End Sub

End Class

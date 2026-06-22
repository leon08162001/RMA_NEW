Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class RepairCenter_List
    Inherits System.Web.UI.Page
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtCompany") = Nothing
            Call setDefault()
            Call QueryData(0)
        End If
    End Sub
#End Region

    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RepairCenter", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenterTittle.Text = _oLanguage.getText("RepairCenter", "002", ctlLanguage.eumType.Tag)

        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "011", ctlLanguage.eumType.Command)

        Me.UI_RepairCenter.Columns(1).HeaderText = _oLanguage.getText("RepairCenter", "003", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(2).HeaderText = _oLanguage.getText("RepairCenter", "004", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(3).HeaderText = _oLanguage.getText("RepairCenter", "005", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(4).HeaderText = _oLanguage.getText("RepairCenter", "006", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(5).HeaderText = _oLanguage.getText("RepairCenter", "044", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(6).HeaderText = _oLanguage.getText("RepairCenter", "007", ctlLanguage.eumType.Tag)
        Me.UI_RepairCenter.Columns(7).HeaderText = _oLanguage.getText("RepairCenter", "008", ctlLanguage.eumType.Tag)

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        dtCompany = oCompany.QueryAll("")
        Call ArrangementData(dtCompany)

        Session("_dtCompany") = dtCompany
        Dim dvCompany As DataView = dtCompany.DefaultView
        Me.ViewState("_SortExpression") = "COMP_NO"
        Me.ViewState("_SortDirection") = "asc"
        dvCompany.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RepairCenter_DataBind(dvCompany, iPageIndex)
    End Sub

    Private Sub RepairCenter_DataBind(ByVal dvCompany As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_RepairCenter.PageSize = _PageSize
        Me.UI_RepairCenter.PageIndex = iPageIndex
        Me.UI_RepairCenter.DataSource = dvCompany
        Me.UI_RepairCenter.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtCompany As CompanyDTO.CompanyDataTable)
        Dim i As Integer = 0
        Dim iValue As String = ""

        dtCompany.Columns.Add("Status")
        dtCompany.Columns.Add("SeqID")

        For i = 0 To dtCompany.Rows.Count - 1
            dtCompany.Rows(i)("SeqID") = i + 1

            '狀態
            iValue = dtCompany.Rows(i).Item("COMP_VISIBLE").ToString.Trim()
            If iValue.ToString.Trim() = "1" Then
                dtCompany.Rows(i).Item("Status") = _oLanguage.getText("RepairCenter", "019", ctlLanguage.eumType.Tag)
            Else
                dtCompany.Rows(i).Item("Status") = _oLanguage.getText("RepairCenter", "020", ctlLanguage.eumType.Tag)
            End If
        Next
    End Sub

    Protected Sub UI_RepairCenter_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_RepairCenter.RowCommand
        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_RepairCenter.Rows(iIndex)

            Dim UI_CompNo As Label = row.FindControl("UI_CompNo")
            Me.UI_lblPreviousPage_CompNo.Text = UI_CompNo.Text.Trim
        End If
    End Sub

    Protected Sub UI_RepairCenter_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_RepairCenter.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_RepairCenter.PageIndex * Me.UI_RepairCenter.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
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

    Protected Sub UI_RepairCenter_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_RepairCenter.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtCompany") Is Nothing Then
            Dim dtCompany As CompanyDTO.CompanyDataTable = Session("_dtCompany")
            Dim dvCompany As DataView = dtCompany.DefaultView

            Call RepairCenter_DataBind(dvCompany, iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_RepairCenter_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_RepairCenter.Sorting

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

        If IsNothing(Session("_dtCompany")) = False Then
            Dim dtCompany As DataTable = Session("_dtCompany")
            Dim dvCompany As DataView = dtCompany.DefaultView
            dvCompany.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RepairCenter_DataBind(dvCompany, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_RepairCenter.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_RepairCenter.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_RepairCenter.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_RepairCenter.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_RepairCenter.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_RepairCenter.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

End Class

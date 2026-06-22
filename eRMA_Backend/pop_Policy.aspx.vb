Imports DataService

Partial Class pop_Policy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call QueryData()
        End If

    End Sub

    Private Sub QueryData()
        Dim oPolicy As New ctlPolicy
        Dim dtPolicy As New PolicyDTO.PolicyDataTable

        'Session("_LanguageID") = "002"
        dtPolicy = oPolicy.Query(Session("_LanguageID").ToString(), Session("_Comp_Admin").ToString())

        If dtPolicy.Rows.Count > 0 Then
            Me.UI_lblPolicy.Text = dtPolicy.Rows(0).Item("POLICY_TEXT").ToString().Trim()
        End If


    End Sub

End Class

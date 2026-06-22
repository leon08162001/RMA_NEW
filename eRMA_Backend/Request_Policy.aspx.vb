Imports DefLanguage

Partial Class Request_Policy
    Inherits System.Web.UI.Page

    Dim _oLanguage As New ctlLanguage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Me.UI_lblTittle.Text = _oLanguage.getText("Policy", "006", ctlLanguage.eumType.Tag)

            Me.UI_cmdDisagree.Text = _oLanguage.getText("Common", "023", ctlLanguage.eumType.Command)
            Me.UI_cmdAgree.Text = _oLanguage.getText("Common", "024", ctlLanguage.eumType.Command)
        End If

        If Convert.ToBoolean(Session("_isPolicy")) = True Then
            Response.Redirect("Request_New.aspx")
        End If

    End Sub


    Protected Sub UI_cmdAgree_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAgree.Click
        Session("_isPolicy") = True     '紀錄是否有顯示過Policy
        Response.Redirect("Request_New.aspx")
    End Sub
End Class

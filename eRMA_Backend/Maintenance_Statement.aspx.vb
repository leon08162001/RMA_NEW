
Partial Class Maintenance_Statement
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Session("_Role").ToString().IndexOf("2") = -1 AndAlso Session("_Role").ToString().IndexOf("9") = -1) Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

End Class

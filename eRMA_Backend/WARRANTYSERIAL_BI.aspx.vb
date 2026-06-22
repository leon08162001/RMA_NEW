
Partial Class WARRANTYSERIAL_BI
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("A") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

End Class

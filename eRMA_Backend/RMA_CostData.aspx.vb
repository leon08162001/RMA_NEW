
Partial Class RMA_CostData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("C") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

End Class

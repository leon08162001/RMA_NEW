Partial Class _WarrantyStatusSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Panel2.Visible = True
            Panel1.Visible = False
        End If

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Account_Txt.Text.Trim() = "rtg" And Password_Txt.Text.Trim() = "rtg_rma0104" Then
            Panel1.Visible = True
            Panel2.Visible = False
        End If

    End Sub
End Class

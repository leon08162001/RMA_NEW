
Imports DefLanguage

Partial Class ascx_uc_Wait
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Me.loadingLabel.Text = _oLanguage.getText("RMA2", "065", ctlLanguage.eumType.Tag)
        End If

    End Sub


End Class

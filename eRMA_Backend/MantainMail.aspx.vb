Imports DataService
Imports DefLanguage

Partial Class MantainMail
    Inherits System.Web.UI.Page
    Dim _oLanguage As New ctlLanguage
    Dim oAdmin As New ctlAdmin
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            SetMailData()
            setControls()
        End If
    End Sub
#End Region

    Private Sub setControls()
        Me.UI_btnSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
    End Sub

    Private Sub SetMailData()
        Dim dtMailSet As New AccountDTO.MailSetDataTable
        dtMailSet = oAdmin.GetMailSet()
        dtMailSet.DefaultView.RowFilter = "MAIL_ID='Part'"
        If dtMailSet.DefaultView.Count > 0 Then
            UI_tbPartMail.Text = dtMailSet.DefaultView(0)("MAIL_ADDRESS")
        End If
    End Sub

    Protected Sub UI_btnSave_Click(sender As Object, e As EventArgs) Handles UI_btnSave.Click
        Dim dtMailSet As New AccountDTO.MailSetDataTable
        Dim drMailSet As AccountDTO.MailSetRow = dtMailSet.NewRow
        If UI_tbPartMail.Text.Trim() <> "" Then
            drMailSet.Mail_ID = "Part"
            drMailSet.Mail_Address = UI_tbPartMail.Text.Trim()
            dtMailSet.AddMailSetRow(drMailSet)
        End If
        oAdmin.SaveMailSet(dtMailSet)
    End Sub

End Class

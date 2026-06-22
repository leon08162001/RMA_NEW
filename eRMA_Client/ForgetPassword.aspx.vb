Imports DataService
Imports DefLanguage

Partial Class ForgetPassword
    Inherits System.Web.UI.Page
    Dim _oComm As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Me.UI_lblForgetTitle.Text = _oLanguage.getText("Common", "054", ctlLanguage.eumType.Tag)
            Me.UI_lblAccountID.Text = _oLanguage.getText("Common", "055", ctlLanguage.eumType.Tag)
            Me.UI_lblEmail.Text = _oLanguage.getText("Common", "056", ctlLanguage.eumType.Tag)

            Me.UI_cmdsubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        End If

    End Sub

    Protected Sub UI_cmdsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdsubmit.Click
        Dim blnFlag As Boolean = False

        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustomerUser As New CustomerDTO.VWCUSTOMERUSERDataTable

        Dim oMail As New ctlMail

        Try
            Dim sSubject As String = _oLanguage.getText("Mail", "003", ctlLanguage.eumType.Mail)
            Dim sBody As String = _oLanguage.getText("Mail", "004", ctlLanguage.eumType.Mail)
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            dtCustomerUser = oCustomerUser.QueryByForgetPassword(Me.UI_txtAccountID.Text.Trim(), Me.UI_txtMail.Text())

            If dtCustomerUser.Rows.Count > 0 Then
                sBody = sBody.Replace("[$Company Name$]", dtCustomerUser.Rows(0)("CU_NAME").ToString().Trim())
                sBody = sBody.Replace("[$Account ID$]", dtCustomerUser.Rows(0)("CUUS_ACCOUNTID").ToString().Trim())
                sBody = sBody.Replace("[$Password]", dtCustomerUser.Rows(0)("CUUS_PWD").ToString().Trim())
                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                Dim mailTo As String = Me.UI_txtMail.Text()
                If _isDebug = True Then
                    mailTo = ConfigurationManager.AppSettings("MailTo")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody, Me.UI_txtMail.Text())
            End If

        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "002", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)

            Else
                sMsg = _oLanguage.getText("Mail", "001", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Default.aspx")
            End If

        End Try

    End Sub

End Class

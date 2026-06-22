Imports DataService
Imports DefLanguage


Partial Class ascx_ucMessageSave
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _Message As String = ""
    Dim _RedirectURL As String = ""
    Dim _show As Boolean = False


    Enum eumTransferURL As Integer
        Redirect = 1
        Transfer = 2
    End Enum

	Public Function getoLanguage(ByVal FUNCTIONNAME As String,ByVal KEY_ As String)   As String
	Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
	End Function

    Public Function getoLanguageTag() As String
        Return _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.lblTitleMsg.Text = _oLanguage.getText("Common", "013", ctlLanguage.eumType.Tag)
            Me.UI_butOK.Text = _oLanguage.getText("Common", "014", ctlLanguage.eumType.Command)
            Me.UI_butClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_butAlert.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)

        End If
        'Me.Panel_OK.Visible = True
        'Me.UI_butOK.Visible = True
    End Sub



    ''' <summary>
    ''' 顯示成功訊息
    ''' </summary>
    ''' <param name="Message">提示訊息</param>
    ''' <param name="RedirectURL">按下確認後要導引到哪一頁</param>
    ''' <remarks></remarks>
    Public Sub showMessageBySuccess(ByVal Message As String, ByVal TransferURL As eumTransferURL, ByVal RedirectURL As String)
        _Message = Message
        Me.ViewState("_RedirectURL") = RedirectURL
        Me.ViewState("_TransferURL") = TransferURL
        _show = True

        Me.html_Success.Text = _Message
        Me.html_Failed.Text = ""

        Me.html_Success.Visible = True
        Me.html_Failed.Visible = False

        Me.Panel_OK.Style.Add("display", "block")
        Me.Panel_Cencel.Style.Add("display", "none")
        Me.Panel_Alert.Style.Add("display", "none")

        Me.ModalMessage.CancelControlID = "UI_butClose"

        Me.ModalMessage.Show()

    End Sub

    ''' <summary>
    ''' 顯示失敗訊息
    ''' </summary>
    ''' <param name="Message">提示訊息</param>
    ''' <remarks></remarks>
    Public Sub showMessageByFailed(ByVal Message As String)
        _Message = Message
        _show = True

        Me.html_Success.Text = ""
        Me.html_Failed.Text = _Message

        Me.html_Success.Visible = False
        Me.html_Failed.Visible = True

        Me.Panel_OK.Style.Add("display", "none")
        Me.Panel_Cencel.Style.Add("display", "block")
        Me.Panel_Alert.Style.Add("display", "none")

        Me.ModalMessage.CancelControlID = "UI_butClose"

        Me.ModalMessage.Show()
    End Sub



    ''' <summary>
    ''' 顯示成功訊息
    ''' </summary>
    ''' <param name="Message">提示訊息</param>
    ''' <remarks></remarks>
    Public Sub showMessageByAlert(ByVal Message As String)
        _Message = Message
        _show = True

        Me.html_Success.Text = _Message
        Me.html_Failed.Text = ""

        Me.html_Success.Visible = True
        Me.html_Failed.Visible = False

        Me.Panel_OK.Style.Add("display", "none")
        Me.Panel_Cencel.Style.Add("display", "none")
        Me.Panel_Alert.Style.Add("display", "block")

        Me.ModalMessage.CancelControlID = "UI_butAlert"

        Me.ModalMessage.Show()

    End Sub



    ''' <summary>
    ''' 按下確認後要導引到的頁面
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butOK.Click
        'Dim TransferURL As eumTransferURL = Me.ViewState("_TransferURL")

        'If Me.ViewState("_RedirectURL") IsNot Nothing Then
        '    If Me.ViewState("_RedirectURL") <> "" Then
        '        Select Case TransferURL
        '            Case eumTransferURL.Redirect
        '                Response.Redirect(Me.ViewState("_RedirectURL"))
        '            Case eumTransferURL.Transfer
        '                Server.Transfer(Me.ViewState("_RedirectURL"))
        '        End Select
        '    End If
        'End If


        'Me.ViewState("_RedirectURL") = ""
        'Me.ViewState("_TransferURL") = ""
    End Sub


    
End Class

Imports DataService
Imports DefLanguage


Partial Class ascx_ucMessage_UI_cmdBankTransfer
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _Message As String = ""
    Dim _RedirectURL As String = ""
    Dim _show As Boolean = False


    Enum eumTransferURL As Integer
        Redirect = 1
        Transfer = 2
    End Enum


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.lblTitleMsg.Text = _oLanguage.getText("Common", "013", ctlLanguage.eumType.Tag)
            Me.HyperLink1.Text = _oLanguage.getText("Common", "014", ctlLanguage.eumType.Command)
            Me.UI_butOK.Text = _oLanguage.getText("Common", "014", ctlLanguage.eumType.Command)
            Me.UI_butClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_butAlert.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        End If
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
    Public Sub showMessageByFailed(ByVal Message As String, ByVal ReportToPDF As String)

        HiddenField_Panel_OK.Value = ReportToPDF

        Dim strarr() As String = HiddenField_Panel_OK.Value.Split("\")

        HyperLink1.NavigateUrl = _Reoprt_FilePath & "/FILE/Report/" & strarr(strarr.Length - 1)

        _Message = Message
        _show = True

        Me.html_Success.Text = ""
        Me.html_Failed.Text = _Message

        Me.html_Success.Visible = False
        Me.html_Failed.Visible = True

        Me.Panel_OK.Style.Add("display", "block")
        Me.Panel_Cencel.Style.Add("display", "none")
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

    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("WEBURL")

    ''' <summary>
    ''' 按下確認後要導引到的頁面
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butOK.Click
        Dim strarr() As String
        strarr = HiddenField_Panel_OK.Value.Split("\")


        'Dim script As String = "<script>window.open('" & _Reoprt_FilePath & "FILE/Report/" & strarr(strarr.Length - 1) & "');</script>"

        Dim script As String = "<script>alert('" & _Reoprt_FilePath & "FILE/Report/" & strarr(strarr.Length - 1) & "');</script>"

        Response.Write(script)


    End Sub

    Protected Sub UI_butClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butClose.Click



    End Sub




End Class

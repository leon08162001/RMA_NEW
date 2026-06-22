Imports RMA_Common

Partial Class logout
    Inherits System.Web.UI.Page
    Dim _isDebug = ConfigurationManager.AppSettings("isDebug")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call LoginOut()
            LogHelper.WriteLog(Session("_ClientPort").ToString)
            '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 begin
            Dim _WEBURL As String = ConfigurationManager.AppSettings("WEBURL")
            Dim request As HttpRequest = Page.Request
            Dim scheme As String = request.Url.Scheme      ' http 或 https
            Dim host As String = request.Url.Host          ' 網域或 IP
            Dim port As Integer = request.Url.Port         ' port
            Dim url As String = If(HttpContext.Current.Request.IsLocal OrElse _isDebug = True, String.Format("{0}://{1}:{2}/{3}", scheme, host, Session("_ClientPort"), "Default.aspx"), _WEBURL)
            LogHelper.WriteLog(url)
            Response.Redirect(url)
            'Response.Redirect("https://e-rma.cipherlab.com.tw/")
            '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 end
        End If
    End Sub
    ''' <summary>
    ''' 登出清除Session
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoginOut()
        Session("_ucMessageRMA") = Nothing
        Session("_Identity") = ""
        Session("_CustomerID") = ""
        Session("_RepairName") = ""
        Session("_RepairTEL") = ""

        Session("_UserID") = ""
        Session("_UserName") = ""
        Session("_DeptNO") = ""
        Session("_isManager") = ""

        Session("_Role") = ""
        Session("_OperationRole") = ""
        Session("_OperationRoleName") = ""

        Session("_RepairCenter") = ""
        Session("_AuthorityLevel") = ""
        Session("_LanguageID") = "001"
        Session("_isPolicy") = False
        Session("_isTimeOut") = False
        Session("_dtLanguage") = Nothing
        Session("_isShowNotFound") = ""

        Session("_Comp_Admin") = ""

    End Sub

End Class

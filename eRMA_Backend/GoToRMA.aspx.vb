Partial Class GoToRMA 'JW/20150526/shaili
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    'Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"


#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '登入者ID 因為主要登入ID 由customeruser.cuus_accountid 為主 所以要抓 Session("_CustomerID")  為登入ID
        'Dim str_UserID As String = Session("_UserID").ToString().Trim()
        Dim str_UserID As String = Session("_CustomerID").Trim()
        Dim str_UserName As String = Session("_UserName").ToString().Trim()
        Dim str_UserRole As String = Session("_Role").ToString().Trim()
        'Dim str_act As String = "http://" + Request.Url.Authority + "/RMA/View/Account/Login.aspx?ID=" & str_UserID & "&name=" & str_UserName
        If str_UserID = String.Empty Then
            str_UserID = Session("_UserID").ToString().Trim()
        End If

        Dim str_act As String = "https://e-rma.cipherlab.com.tw/RMA/View/Account/Login.aspx?ID=" & str_UserID & "&name=" & str_UserName

        Me.Form.Action = str_act
        Response.Write("<script language=javascript>function func(){document.form1.submit();}</script>")

    End Sub
#End Region

End Class

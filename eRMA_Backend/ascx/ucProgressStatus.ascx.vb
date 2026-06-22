
Partial Class ascx_ucProgressStatus
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Me.IsPostBack = False Then
        '    Dim sScript As String = "<script type=""text/javascript"" src=""script/jsUpdateProgress.js""></script>"
        '    Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "UpdateProgress", sScript)
        'End If
    End Sub


    ''' <summary>
    ''' 用來表示要關聯不處理的 UI物件
    ''' </summary>
    ''' <value>傳入物件 ClientID</value>
    ''' <returns>回傳物件 ClientID</returns>
    ''' <remarks></remarks>
    Public Overridable Property NotpostBackElement() As String
        Get
            Return Me.NotpostBackElementID.Text.Trim()
        End Get

        Set(ByVal Value As String)
            Me.NotpostBackElementID.Text = Value.Trim()
        End Set
    End Property




End Class

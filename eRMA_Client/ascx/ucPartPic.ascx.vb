Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ascx_ucPartPic
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _PartsPic As String = ConfigurationSettings.AppSettings("PartsPic")




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
    End Sub




    '''' <summary>
    '''' 設定是否要顯示
    '''' </summary>
    '''' <param name="imgFile">圖檔</param>
    '''' <remarks></remarks>
    Public Sub show(ByVal imgFile As String)
        Me.UI_imgPic.ImageUrl = _WEBURL & _PartsPic & imgFile
        Me.ajModalProgress.Show()
    End Sub


End Class

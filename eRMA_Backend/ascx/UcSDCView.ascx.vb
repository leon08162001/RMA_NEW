Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_UcSDCView
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        'Me.UI_lblAddressTittle.Text = _oLanguage.getText("Warranty", "055", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)
    End Sub

    Public Sub show(ByVal RMAD_SERIALNO As String, ByVal isShow As Boolean)
        Dim oRMA As New ctlRMA

        Me.ViewState("_show") = isShow

        Dim dtSDCs As DataTable = oRMA.GetSDCData(RMAD_SERIALNO)
        Me.ViewState("dtWarrParts") = dtSDCs
        dgvSDC.DataSource = dtSDCs
        dgvSDC.DataBind()


        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub


End Class

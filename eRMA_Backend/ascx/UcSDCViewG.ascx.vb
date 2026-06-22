Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_UcSDCViewG
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If

        dgvSDC.ControlStyle.Font.Size = 8
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        'Me.UI_lblAddressTittle.Text = _oLanguage.getText("Warranty", "055", ctlLanguage.eumType.Tag)

        'Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)
    End Sub

    Public Sub show(ByVal RMAD_SERIALNO As String, ByVal uWith As Unit)
        Dim oRMA As New ctlRMA

        Dim dtSDCs As DataTable = oRMA.GetSDCData(RMAD_SERIALNO)
        dgvSDC.DataSource = dtSDCs
        dgvSDC.DataBind()

        '固定欄位寬度
        dgvSDC.Style.Add("table-layout", "fixed")

        If dtSDCs.Rows.Count > 0 Then
            Me.UI_panel.Visible = True
            Me.UI_panel.Width = uWith
        Else
            Me.UI_panel.Visible = False
        End If



    End Sub
    Protected Sub dgvSDC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgvSDC.RowDataBound
        e.Row.Attributes.Add("style", "height:10px")
    End Sub

End Class

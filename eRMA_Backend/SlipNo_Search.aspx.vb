Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class SlipNo_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"
    Dim _oLanguage As New ctlLanguage

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            lblErrorMsg.Text = ""
        End If
    End Sub
#End Region
    Private Sub QueryData()
        lblErrorMsg.Text = ""
        lblNo.Text = ""
        lblCust.Text = ""
        lblCount.Text = ""

        Dim sSlipNo As String = Me.txtSlipNo.Text.ToString().Trim()
        If sSlipNo.Length = 17 Then
            Dim oWarranty As New ctlWarranty
            Dim dtMData As New DataTable
            dtMData = oWarranty.QuerySlipNoDecrypt(sSlipNo)
            If dtMData.Rows.Count > 0 Then
                lblNo.Text = dtMData.Rows(0)("SLIP_NO").ToString().Trim()
                lblCust.Text = dtMData.Rows(0)("CUST_CODE").ToString().Trim()
                lblCount.Text = dtMData.Rows(0)("SLIP_COUNT").ToString().Trim()
            Else
                lblErrorMsg.Text = " "
            End If
        Else
            lblErrorMsg.Text = "Please input 17 number query key "
        End If
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData()
    End Sub

End Class

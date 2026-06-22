Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucImportErrorMsg
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_RMADID") = ""

            Call setControls()
        End If
    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblUploadData.Text = _oLanguage.getText("Upload", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblRmaNo.Text = _oLanguage.getText("Upload", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblRmaDate.Text = _oLanguage.getText("Upload", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("Upload", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Upload", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblErrMsg.Text = _oLanguage.getText("Upload", "013", ctlLanguage.eumType.Tag)
        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)
    End Sub



    Private Sub CleanData()
        Me.UI_txtRmaDate.Text = ""
        Me.UI_txtRmaNo.Text = ""
    End Sub



    Private Sub QueryDataUpload()
        Dim oRMA As New ctlRMA.Upload
        Dim dtClientDetail As New ImpRmaDTO.dtImpRmaDataTable

        dtClientDetail = oRMA.QueryRmaImport("", Me.ViewState("_RMAID").ToString(), "", "", "", "", "", "", "")

        If dtClientDetail.Rows.Count > 0 Then
            Dim dr As ImpRmaDTO.dtImpRmaRow = dtClientDetail.Rows(0)
            If dr.IsIRMA_NONull = False Then Me.UI_txtRmaNo.Text = dr.IRMA_NO.ToString().Trim()
            If dr.IsIRMA_CSTMPNull = False Then Me.UI_txtRmaDate.Text = dr.IRMA_CSTMP.ToString().Trim()
            If dr.Iscu_nameNull = False Then Me.UI_txtCustomer.Text = dr.cu_name.ToString().Trim()
            If dr.IsIRMA_STATUSNull = False Then Me.UI_txtStatus.Text = dr.IRMA_STATUS.ToString().Trim()
            If dr.IsIRMA_ERRORNull = False Then Me.UI_txtErrMsg.Text = dr.IRMA_ERROR.ToString().Trim()

        End If
    End Sub


    Public Sub show(ByVal sRMAID As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            If sRMAID.Trim <> "" Then
                Me.ViewState("_RMAID") = sRMAID

                Call CleanData()
                Call QueryDataUpload()
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub
End Class

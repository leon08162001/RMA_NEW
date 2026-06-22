Imports DataService
Imports DefLanguage

Partial Class Policy
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_eumCommand") = eumCommand.AddNew
            Call setControls()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim ClientID As String = Me.UI_cboPolicyDFLNO.ClientID
        Me.ucProgressStatus.NotpostBackElement = ClientID


        Me.UI_lblPolicyID.Text = ""
        Dim Category1Text As String = _oLanguage.getText("Policy", "003", ctlLanguage.eumType.Tag)
        oCommon.getDefLaguageByDropDownList(Me.UI_cboPolicyDFLNO, Category1Text)

        Call setValidationMessage(Me.rfv_txtPolicy)
        Call setValidationMessage(Me.cv_cboPolicyDFLNO)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Policy", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblPolicyDFLNO.Text = _oLanguage.getText("Policy", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblHTML.Text = _oLanguage.getText("Policy", "007", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Call UI_cboPolicyDFLNO_SelectedIndexChanged(Me.UI_cboPolicyDFLNO, System.EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "cv_cboPolicyDFLNO".ToLower()
                sErrorMessage = _oLanguage.getText("Policy", "004", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_txtPolicy".ToLower()
                sErrorMessage = _oLanguage.getText("Policy", "005", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    Protected Sub UI_cboPolicyDFLNO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboPolicyDFLNO.SelectedIndexChanged
        Dim oPolicy As New ctlPolicy
        Dim dtPolicy As New PolicyDTO.PolicyDataTable
        Dim sPolicyDEFLNO As String = Me.UI_cboPolicyDFLNO.SelectedValue.ToString().Trim()
        Dim sPolicyDEFL As String = Me.UI_cboPolicyDFLNO.SelectedItem.Text.ToString().Trim()
        dtPolicy = oPolicy.Query(sPolicyDEFLNO, sPolicyDEFL)

        If dtPolicy.Rows.Count > 0 Then
            Dim dr As PolicyDTO.PolicyRow = dtPolicy.Rows(0)

            Me.UI_lblPolicyID.Text = dr.POLICY_ID.ToString().Trim()
            Me.UI_txtPolicy.Text = dr.POLICY_TEXT.ToString()
            Me.ViewState("_eumCommand") = eumCommand.UPDATE
        Else
            Me.UI_lblPolicyID.Text = ""
            Me.UI_txtPolicy.Text = ""
            Me.ViewState("_eumCommand") = eumCommand.AddNew
        End If
    End Sub

    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oPolicy As New ctlPolicy
        Dim dtPolicy As New PolicyDTO.PolicyDataTable
        Dim oGuid As Guid = Guid.NewGuid

        Dim sPolicyDFLNO As String = Me.UI_cboPolicyDFLNO.SelectedValue.ToString().Trim()
        Dim sPolicyText As String = Me.UI_txtPolicy.Text.ToString().Trim()

        Try
            Dim sGUID As String = oGuid.ToString
            Dim dr As PolicyDTO.PolicyRow = dtPolicy.NewPolicyRow

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    dr.POLICY_ID = sGUID

                Case eumCommand.UPDATE
                    dr.POLICY_ID = Me.UI_lblPolicyID.Text.Trim()
            End Select

            dr.POLICY_TEXT = sPolicyText.ToString().Trim()

            dr.POLICY_AD = Session("_UserID")
            dr.POLICY_ADNAME = Session("_UserName")
            dr.POLICY_CSTMP = Date.Now
            dr.POLICY_LUAD = Session("_UserID")
            dr.POLICY_LUADNAME = Session("_UserName")
            dr.POLICY_LUSTMP = Date.Now
            dr.POLICY_DFLNO = sPolicyDFLNO.ToString().Trim()

            dtPolicy.AddPolicyRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oPolicy.SaveAdd(dtPolicy)

                Case eumCommand.UPDATE
                    oPolicy.SaveEdit(dtPolicy)
            End Select

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                    Case eumCommand.AddNew
                        sMsg = oCommon.getMessage(Common.enmMessage.AddOK)

                    Case eumCommand.UPDATE
                        sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                End Select
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try
    End Sub

End Class

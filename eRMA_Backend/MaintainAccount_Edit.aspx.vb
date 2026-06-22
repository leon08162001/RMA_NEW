Imports DataService
Imports DefLanguage

Partial Class MaintainAccount_Edit
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.UI_lblPreviousPage_adID.Text = Session("_UserID").ToString()
            Call setControls()
            Call QueryData()
        End If

    End Sub
#End Region

    Private Sub setControls()
        Me.rfvPassword.ErrorMessage = _oLanguage.getText("MaintainAccount", "031", ctlLanguage.eumType.Validator)
        Me.revUpperMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "033", ctlLanguage.eumType.Validator)
        Me.rfvMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "035", ctlLanguage.eumType.Validator)
        Me.revMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "033", ctlLanguage.eumType.Validator)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("MaintainAccount", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID1.Text = _oLanguage.getText("MaintainAccount", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblAuthorityLevel.Text = _oLanguage.getText("MaintainAccount", "014", ctlLanguage.eumType.Tag)
        Me.UI_lblName.Text = _oLanguage.getText("MaintainAccount", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblUpperMail.Text = _oLanguage.getText("MaintainAccount", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblPassword.Text = _oLanguage.getText("MaintainAccount", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("MaintainAccount", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblRole.Text = _oLanguage.getText("MaintainAccount", "020", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("MaintainAccount", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblLastEditor.Text = _oLanguage.getText("MaintainAccount", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblLastDate.Text = _oLanguage.getText("MaintainAccount", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblMail.Text = _oLanguage.getText("MaintainAccount", "011", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData()
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable
        Dim sAdmin As String = Me.UI_lblPreviousPage_adID.Text.ToString().Trim()

        dtAdmin = oAdmin.Query(sAdmin, "")

        Dim i As Integer = 0
        Dim sAuthorityLevel As String = ""
        Dim Status As String = ""
        Dim sRole As String = ""
        Dim arrRoles() As String
        Dim sRepairCenter As String = ""
        Dim sRepairName As String = ""
        'Dim arrRepairCenter() As String

        If dtAdmin.Count > 0 Then
            Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

            Me.UI_lblAccountID.Text = dr.AD_ID.ToString().Trim()
            Me.UI_lblNameText.Text = dr.AD_NAME.ToString().Trim()
            Me.UI_txtPassword.Text = dr.AD_PASSWORD.ToString().Trim()
            Me.UI_lblLastEditorText.Text = dr.AD_LUAD.ToString().Trim()
            Me.UI_lblLastDateText.Text = dr.AD_LUSTMP.ToShortDateString().ToString().Trim()

            If dr.IsAD_UPPERSUPERMIALNull = False Then Me.UI_txtUpperMail.Text = dr.AD_UPPERSUPERMIAL.ToString().Trim()
            If dr.IsAD_EMAILNull = False Then Me.UI_txtMail.Text = dr.AD_EMAIL.ToString().Trim()

            If dr.AD_AUTHORITYLEVEL.ToString().Trim() = "0" Then
                sAuthorityLevel = _oLanguage.getText("MaintainAccount", "016", ctlLanguage.eumType.Tag)
            Else
                sAuthorityLevel = _oLanguage.getText("MaintainAccount", "017", ctlLanguage.eumType.Tag)
            End If
            Me.UI_lblAuthorityLevelText.Text = sAuthorityLevel.Trim()

            '狀態:1.Open 、0.Close
            If dr.AD_VISIBLE.ToString().Trim() = "0" Then
                Status = _oLanguage.getText("MaintainAccount", "026", ctlLanguage.eumType.Tag)
            Else
                Status = _oLanguage.getText("MaintainAccount", "025", ctlLanguage.eumType.Tag)
            End If
            Me.UI_lblStatusText.Text = Status.Trim()

            '角色:1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            arrRoles = dr.AD_ROLE.ToString().Trim().Split(",")
            For i = 0 To arrRoles.Length - 1
                Select Case arrRoles(i).Trim()
                    Case "1"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "021", ctlLanguage.eumType.Tag)
                    Case "2"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "002", ctlLanguage.eumType.Tag)
                    Case "3"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "022", ctlLanguage.eumType.Tag)
                    Case "4"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "023", ctlLanguage.eumType.Tag)
                    Case "9"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "024", ctlLanguage.eumType.Tag)
                    Case "7"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "037", ctlLanguage.eumType.Tag)
                    Case "6"
                        sRole = sRole & "," & _oLanguage.getText("MaintainAccount", "038", ctlLanguage.eumType.Tag)
                End Select
            Next
            Me.UI_lblRoleText.Text = Mid(sRole, 2, Len(sRole))                      '除去前面的逗號


            '維修中心代碼
            Dim oCompany As New ctlCompany
            Dim dtCompany As New CompanyDTO.CompanyDataTable
            'arrRepairCenter = dr.AD_REPAIRCENTER.ToString().Trim().Split(",")
            'For i = 0 To arrRepairCenter.Length - 1
            '    sRepairCenter = sRepairCenter & ",'" & arrRepairCenter(i).ToLower().Trim() & "'"
            'Next
            'sRepairCenter = "(" & Mid(sRepairCenter, 2, Len(sRepairCenter)) & ")"   '除去前面的逗號

            dtCompany = oCompany.QueryByRepairName(dr.AD_REPAIRCENTER.ToString())
            For i = 0 To dtCompany.Rows.Count - 1
                sRepairName = sRepairName & "," & dtCompany.Rows(i).Item("COMP_NAME").ToString().Trim()
            Next
            Me.UI_lblRepairCenterText.Text = Mid(sRepairName, 2, Len(sRepairName))
        End If

    End Sub

    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Try
            '修改Admin-(密碼及Mail及管理者Mail)
            oAdmin.ChangePassword(Me.UI_lblAccountID.Text.ToString().Trim(), Me.UI_txtPassword.Text.ToString().Trim(),
                Me.UI_txtUpperMail.Text.ToString().Trim(), Me.UI_txtMail.Text.ToString().Trim(),
                Session("_UserID").ToString(), Session("_UserName").ToString())

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Default.aspx")
            End If
        End Try
    End Sub

End Class

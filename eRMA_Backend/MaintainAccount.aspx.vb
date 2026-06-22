Imports DataService
Imports DefLanguage

Partial Class MaintainAccount
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_adID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_adID")
                Me.UI_lblPreviousPage_adID.Text = UI_lblPreviousPage_adID.Text.ToString().Trim()

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.UI_lblPreviousPage_adID.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()
                Call QueryData()
            End If
        End If
    End Sub
#End Region

    Private Sub setControls()
        oCommon.getRepairCenteryByCheckBoxList(Me.UI_chkRepairCenter)
        Me.rfvAccountID.ErrorMessage = _oLanguage.getText("MaintainAccount", "029", ctlLanguage.eumType.Validator)
        Me.rfvName.ErrorMessage = _oLanguage.getText("MaintainAccount", "030", ctlLanguage.eumType.Validator)

        Me.rfvPassword.ErrorMessage = _oLanguage.getText("MaintainAccount", "031", ctlLanguage.eumType.Validator)
        Me.cvAuthorityLevel.ErrorMessage = _oLanguage.getText("MaintainAccount", "032", ctlLanguage.eumType.Validator)
        Me.revUpperMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "033", ctlLanguage.eumType.Validator)
        'Me.cvRole.ErrorMessage = _oLanguage.getText("MaintainAccount", "034", ctlLanguage.eumType.Validator)

        Me.rfvMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "035", ctlLanguage.eumType.Validator)
        Me.revMail.ErrorMessage = _oLanguage.getText("MaintainAccount", "033", ctlLanguage.eumType.Validator)


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("MaintainAccount", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID1.Text = _oLanguage.getText("MaintainAccount", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblAuthorityLevel.Text = _oLanguage.getText("MaintainAccount", "014", ctlLanguage.eumType.Tag)
        Me.UI_cboAuthorityLevel.Items(0).Text = _oLanguage.getText("MaintainAccount", "015", ctlLanguage.eumType.Tag)
        Me.UI_cboAuthorityLevel.Items(1).Text = _oLanguage.getText("MaintainAccount", "016", ctlLanguage.eumType.Tag)
        Me.UI_cboAuthorityLevel.Items(2).Text = _oLanguage.getText("MaintainAccount", "017", ctlLanguage.eumType.Tag)
        Me.UI_lblName.Text = _oLanguage.getText("MaintainAccount", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblUpperMail.Text = _oLanguage.getText("MaintainAccount", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblPassword.Text = _oLanguage.getText("MaintainAccount", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("MaintainAccount", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblRole.Text = _oLanguage.getText("MaintainAccount", "020", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(0).Text = _oLanguage.getText("MaintainAccount", "021", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(1).Text = _oLanguage.getText("MaintainAccount", "002", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(2).Text = _oLanguage.getText("MaintainAccount", "022", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(3).Text = _oLanguage.getText("MaintainAccount", "023", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(4).Text = _oLanguage.getText("MaintainAccount", "024", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(5).Text = _oLanguage.getText("MaintainAccount", "036", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(6).Text = _oLanguage.getText("MaintainAccount", "038", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(7).Text = _oLanguage.getText("MaintainAccount", "037", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(8).Text = _oLanguage.getText("MaintainAccount", "039", ctlLanguage.eumType.Tag)
        Me.UI_chkRole.Items(9).Text = _oLanguage.getText("MaintainAccount", "040", ctlLanguage.eumType.Tag)

        Me.UI_lblStatus.Text = _oLanguage.getText("MaintainAccount", "010", ctlLanguage.eumType.Tag)
        Me.UI_opgStatus.Items(0).Text = _oLanguage.getText("MaintainAccount", "025", ctlLanguage.eumType.Tag)
        Me.UI_opgStatus.Items(1).Text = _oLanguage.getText("MaintainAccount", "026", ctlLanguage.eumType.Tag)
        Me.UI_lblLastEditor.Text = _oLanguage.getText("MaintainAccount", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblLastDate.Text = _oLanguage.getText("MaintainAccount", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblMail.Text = _oLanguage.getText("MaintainAccount", "011", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Me.UI_lblAccountID.Visible = False
        Me.UI_txtAccountID.Visible = False
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Me.UI_txtAccountID.Visible = True
        Else
            Me.UI_lblAccountID.Visible = True
        End If

    End Sub

    Private Sub QueryData()
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Exit Sub
        End If

        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable
        Dim sAdmin As String = Me.UI_lblPreviousPage_adID.Text.ToString().Trim()

        dtAdmin = oAdmin.Query(sAdmin, "")

        Dim i As Integer = 0
        Dim arrRoles() As String
        Dim arrRepairCenter() As String
        If dtAdmin.Count > 0 Then
            Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

            Me.UI_lblAccountID.Text = dr.AD_ID.ToString().Trim()
            Me.UI_txtAccountID.Text = dr.AD_ID.ToString().Trim()
            Me.UI_txtName.Text = dr.AD_NAME.ToString().Trim()
            Me.UI_txtPassword.Text = dr.AD_PASSWORD.ToString().Trim()
            Me.UI_lblLastEditorText.Text = dr.AD_LUAD.ToString().Trim()
            Me.UI_lblLastDateText.Text = dr.AD_LUSTMP.ToShortDateString().ToString().Trim()
            Me.UI_cboAuthorityLevel.SelectedValue = dr.AD_AUTHORITYLEVEL.ToString().Trim()
            Me.UI_opgStatus.SelectedValue = dr.AD_VISIBLE.ToString().Trim()

            If dr.IsAD_UPPERSUPERMIALNull = False Then Me.UI_txtUpperMail.Text = dr.AD_UPPERSUPERMIAL.ToString().Trim()
            If dr.IsAD_EMAILNull = False Then Me.UI_txtMail.Text = dr.AD_EMAIL.ToString().Trim()

            '角色:1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            Dim chkRole As CheckBoxList = Me.UI_chkRole
            arrRoles = dr.AD_ROLE.ToString().Trim().Split(",")
            For i = 0 To arrRoles.Length - 1
                Select Case arrRoles(i).Trim()
                    Case "1"
                        chkRole.Items(0).Selected = True
                    Case "2"
                        chkRole.Items(1).Selected = True
                    Case "3"
                        chkRole.Items(2).Selected = True
                    Case "4"
                        chkRole.Items(3).Selected = True
                    Case "9"
                        chkRole.Items(4).Selected = True
                    Case "8"
                        chkRole.Items(5).Selected = True
                    Case "6"
                        chkRole.Items(6).Selected = True
                    Case "7"
                        chkRole.Items(7).Selected = True
                    Case "C"
                        chkRole.Items(8).Selected = True
                    Case "A"
                        chkRole.Items(9).Selected = True
                End Select
            Next

            '維修中心代碼
            Dim chkRepairCenter As CheckBoxList = Me.UI_chkRepairCenter
            Dim j As Integer = 0
            arrRepairCenter = dr.AD_REPAIRCENTER.ToString().Trim().Split(",")
            For i = 0 To arrRepairCenter.Length - 1
                For j = 0 To chkRepairCenter.Items.Count - 1
                    If chkRepairCenter.Items(j).Value.ToLower().Trim() = arrRepairCenter(i).ToLower().Trim() Then
                        chkRepairCenter.Items(j).Selected = True
                    End If
                Next
            Next
        End If

        Me.UI_TrAdminluad.Visible = True
        Me.UI_TrAdminlustmp.Visible = True
        Me.UI_txtAccountID.Enabled = False
    End Sub

    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim sRepair As String = ""
        Dim sRole As String = ""
        Dim sAdID As String = ""

        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                sAdID = Me.UI_txtAccountID.Text.ToString().Trim()

            Case eumCommand.UPDATE
                sAdID = Me.UI_txtAccountID.Text.ToString().Trim()
        End Select

        Try
            Dim dr As AccountDTO.ADMINRow = dtAdmin.NewADMINRow

            For i = 0 To Me.UI_chkRepairCenter.Items.Count - 1
                Dim oListItem As ListItem = Me.UI_chkRepairCenter.Items(i)

                If oListItem.Selected = True Then
                    If sRepair.Trim <> "" Then
                        sRepair = sRepair & ","
                    End If
                    sRepair = sRepair & oListItem.Value.Trim()
                End If
            Next

            For i = 0 To Me.UI_chkRole.Items.Count - 1
                Dim oListItem As ListItem = Me.UI_chkRole.Items(i)

                If oListItem.Selected = True Then
                    If sRole.Trim <> "" Then
                        sRole = sRole & ","
                    End If
                    sRole = sRole & oListItem.Value.Trim()
                End If
            Next
            If sRole.ToString().Trim() = "" Then        '角色(Role)要是都無勾選,值就為0
                sRole = "0"
            End If

            dr.AD_ID = sAdID.ToString().Trim()
            dr.AD_NAME = Me.UI_txtName.Text.ToString().Trim()

            dr.AD_PASSWORD = Me.UI_txtPassword.Text.ToString().Trim()

            dr.AD_REPAIRCENTER = sRepair.ToString().Trim()
            dr.AD_ROLE = sRole.ToString().Trim()
            dr.AD_VISIBLE = Convert.ToInt32(Me.UI_opgStatus.SelectedValue.ToString().Trim())
            dr.AD_AUTHORITYLEVEL = Convert.ToInt32(Me.UI_cboAuthorityLevel.SelectedValue.ToString().Trim())

            dr.AD_UPPERSUPERMIAL = Me.UI_txtUpperMail.Text.ToString().Trim()
            dr.AD_EMAIL = Me.UI_txtMail.Text.ToString().Trim()

            dr.AD_AD = Session("_UserID")
            dr.AD_ADNAME = Session("_UserName")
            dr.AD_CSTMP = Date.Now
            dr.AD_LUAD = Session("_UserID")
            dr.AD_LUADNAME = Session("_UserName")
            dr.AD_LUSTMP = Date.Now

            dtAdmin.AddADMINRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oAdmin.SaveAdd(dtAdmin)                 '新增Admin

                Case eumCommand.UPDATE
                    oAdmin.SaveEdit(dtAdmin)                '修改Admin

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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "MaintainAccount_Search.aspx")
            End If
        End Try
    End Sub

End Class

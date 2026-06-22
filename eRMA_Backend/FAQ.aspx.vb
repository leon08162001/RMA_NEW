Imports DataService
Imports DefLanguage

Partial Class FAQ
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _oLanguage As New ctlLanguage

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            '抓取上一頁的資料
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_FAQID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_FAQID")
                Me.UI_lblPreviousPage_FAQID.Text = UI_lblPreviousPage_FAQID.Text.ToString().Trim()

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.UI_lblPreviousPage_FAQID.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()
                Call QueryData()
            End If

        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim sClientID As String = Me.UI_cboCategory1.ClientID & "," & Me.UI_cmdAdd_Class.ClientID
        'Me.ucProgressStatus.NotpostBackElement = sClientID

        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(Me.UI_cboCategory1, Category1Text)
        oCommon.getFQASubClassByDropDownList(Me.UI_cboCategory1.SelectedValue.ToString().Trim(), Me.UI_cboCategory2, Category2Text)

        Me.UI_txtISSUEDATE.Text = Date.Now.ToShortDateString()

        Call setValidationMessage(Me.cv_cboCategory1)
        Call setValidationMessage(Me.cv_cboCategory2)
        Call setValidationMessage(Me.rfv_txtISSUEDATE)
        Call setValidationMessage(Me.cv_txtISSUEDATE)

        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                Me.UI_TrFAQluad.Visible = False
                Me.UI_TrFAQlustmp.Visible = False
                Me.UI_cmdDel.Visible = False

            Case eumCommand.UPDATE
                Me.UI_TrFAQluad.Visible = True
                Me.UI_TrFAQlustmp.Visible = True
                Me.UI_cmdDel.Visible = True
        End Select


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("FAQ", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCategory1.Text = _oLanguage.getText("FAQ", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblCategory2.Text = _oLanguage.getText("FAQ", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("FAQ", "020", ctlLanguage.eumType.Tag)
        Me.UI_opgVisible.Items(0).Text = _oLanguage.getText("FAQ", "013", ctlLanguage.eumType.Tag)
        Me.UI_opgVisible.Items(1).Text = _oLanguage.getText("FAQ", "014", ctlLanguage.eumType.Tag)
        Me.UI_lblISSUEDATE.Text = _oLanguage.getText("FAQ", "015", ctlLanguage.eumType.Tag)
        Me.UI_lblQuestion.Text = _oLanguage.getText("FAQ", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblAnswer.Text = _oLanguage.getText("FAQ", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQluad.Text = _oLanguage.getText("FAQ", "016", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQlustmp.Text = _oLanguage.getText("FAQ", "017", ctlLanguage.eumType.Tag)

        Me.UI_cmdAdd_Class.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        Me.UI_cmdBack1.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)





    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "cv_cboCategory1".ToLower()
                sErrorMessage = _oLanguage.getText("FAQ", "025", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cv_cboCategory2".ToLower()
                sErrorMessage = _oLanguage.getText("FAQ", "026", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_txtISSUEDATE".ToLower()
                sErrorMessage = _oLanguage.getText("FAQ", "027", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cv_txtISSUEDATE".ToLower()
                sErrorMessage = _oLanguage.getText("FAQ", "028", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
        End Select
    End Sub

    ''' <summary>
    ''' FAQ大類下拉式
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboCategory1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboCategory1.SelectedIndexChanged
        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQASubClassByDropDownList(Me.UI_cboCategory1.SelectedValue.ToString().Trim(), Me.UI_cboCategory2, Category2Text)
    End Sub

    ''' <summary>
    ''' 新增修改類別
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Class_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd_Class.Click
        'Me.ucFAQClass.show = True
    End Sub

#Region "QueryData"
    Private Sub QueryData()
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Exit Sub
        End If

        Dim oFAQ As New ctlFAQ.FAQ
        Dim dtFAQ As New FaqDTO.FAQDataTable
        Dim sFAQID As String = Me.UI_lblPreviousPage_FAQID.Text.ToString().Trim()
        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)

        dtFAQ = oFAQ.QueryByPrimaryKey(sFAQID.Trim())

        If dtFAQ.Rows.Count > 0 Then
            Dim dr As FaqDTO.FAQRow = dtFAQ.Rows(0)
            oCommon.getFQASubClassByDropDownList(dr.FAQ_FAQCID.ToString().Trim(), Me.UI_cboCategory2, Category2Text)

            Me.UI_cboCategory1.SelectedValue = dr.FAQ_FAQCID.ToString().Trim()
            Me.UI_cboCategory2.SelectedValue = dr.FAQ_FAQSCID.ToString().Trim()
            Me.UI_txtQuestion.Text = dr.FAQ_QUESTION.ToString().Trim()
            Me.UI_txtAnswer.Text = dr.FAQ_ANSWER.ToString().Trim()
            Me.UI_opgVisible.SelectedValue = dr.FAQ_VISIBLE.ToString().Trim()
            Me.UI_txtISSUEDATE.Text = dr.FAQ_ISSUEDATE.ToShortDateString().Trim()
            Me.UI_lblFAQluadText.Text = dr.FAQ_LUAD.ToString().Trim()
            Me.UI_lblFAQlustmpText.Text = dr.FAQ_LUSTMP.ToString()
        End If

    End Sub
#End Region

    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '讓 Progress 等候一下, 再開始處理資料
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Call Save()
    End Sub

    Private Sub Save()
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oFAQ As New ctlFAQ.FAQ
        Dim dtFAQ As New FaqDTO.FAQDataTable
        Dim oGuid As Guid = Guid.NewGuid

        Try
            Dim dr As FaqDTO.FAQRow = dtFAQ.NewFAQRow
            Dim sGUID As String = oGuid.ToString

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    dr.FAQ_ID = sGUID

                Case eumCommand.UPDATE
                    dr.FAQ_ID = Me.UI_lblPreviousPage_FAQID.Text.Trim()
            End Select

            dr.FAQ_FAQCID = Me.UI_cboCategory1.SelectedValue.ToString().Trim()
            dr.FAQ_FAQSCID = Me.UI_cboCategory2.SelectedValue.ToString().Trim()

            dr.FAQ_QUESTION = Me.UI_txtQuestion.Text.ToString().Trim()
            dr.FAQ_ANSWER = Me.UI_txtAnswer.Text.ToString().Trim()
            dr.FAQ_ISSUEDATE = Convert.ToDateTime(Me.UI_txtISSUEDATE.Text)
            dr.FAQ_VISIBLE = Convert.ToInt16(Me.UI_opgVisible.SelectedValue)

            dr.FAQ_AD = Session("_UserID")
            dr.FAQ_ADNAME = Session("_UserName")
            dr.FAQ_CSTMP = Date.Now
            dr.FAQ_LUAD = Session("_UserID")
            dr.FAQ_LUADNAME = Session("_UserName")
            dr.FAQ_LUSTMP = Date.Now
            dr.FAQ_MARK = 0

            dtFAQ.AddFAQRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oFAQ.SaveAdd(dtFAQ)

                Case eumCommand.UPDATE
                    oFAQ.SaveEdit(dtFAQ)
            End Select

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                'Me.ucMessage.showMessageByFailed(sMessage)
            Else

                Dim sMsg As String = ""
                Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                    Case eumCommand.AddNew
                        sMsg = oCommon.getMessage(Common.enmMessage.AddOK)

                    Case eumCommand.UPDATE
                        sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                End Select
                'Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "FAQ_Search.aspx")
            End If
        End Try

    End Sub

    Protected Sub UI_cmdDel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '讓 Progress 等候一下, 再開始處理資料
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Call Delete()

    End Sub

    Protected Sub Delete()
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFAQ As New ctlFAQ.FAQ

        Dim FAQ_ID As String = Me.UI_lblPreviousPage_FAQID.Text.ToString().Trim()

        Try
            oFAQ.Delete(FAQ_ID, Session("_UserID").ToString().Trim(), Session("_UserName").ToString().Trim())
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                'Me.ucMessage.showMessageByFailed(sMessage)
            Else
                'Me.ucMessage.showMessageBySuccess(oCommon.getMessage(Common.enmMessage.DelOK), ascx_ucMessage.eumTransferURL.Redirect, "FAQ_Search.aspx")
            End If
        End Try
    End Sub

End Class

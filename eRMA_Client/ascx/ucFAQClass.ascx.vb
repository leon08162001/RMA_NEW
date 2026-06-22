Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ascx_ucFAQClass
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSizeByFAQClass")
    Dim _Status0 As String = "Close"
    Dim _Status1 As String = "Open"

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setControls()
            Call QueryFAQClass(0)
            Call QueryFAQSubClass(0)
        End If
    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Call setValidationMessage(Me.rfv_txtFAQC_ClassName)
        Call setValidationMessage(Me.cv_drpCategoryName1)
        Call setValidationMessage(Me.rfv_txtFAQSC_ClassName)

        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(Me.UI_drpCategoryName1, Category1Text)



        '取得Tag Text
        Me.UI_lblFAQTittle.Text = _oLanguage.getText("FAQ", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQC_ClassName.Text = _oLanguage.getText("FAQ", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQC_Visible.Text = _oLanguage.getText("FAQ", "020", ctlLanguage.eumType.Tag)
        Me.UI_opgFAQC_Visible.Items(0).Text = _oLanguage.getText("FAQ", "013", ctlLanguage.eumType.Tag)
        Me.UI_opgFAQC_Visible.Items(1).Text = _oLanguage.getText("FAQ", "014", ctlLanguage.eumType.Tag)
        Me.UI_lblCategoryName1.Text = _oLanguage.getText("FAQ", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQSC_ClassName.Text = _oLanguage.getText("FAQ", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblFAQSC_Visible.Text = _oLanguage.getText("FAQ", "020", ctlLanguage.eumType.Tag)
        Me.UI_opgFAQSC_Visible.Items(0).Text = _oLanguage.getText("FAQ", "013", ctlLanguage.eumType.Tag)
        Me.UI_opgFAQSC_Visible.Items(1).Text = _oLanguage.getText("FAQ", "014", ctlLanguage.eumType.Tag)

        Me.UI_cmdAdd1.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd2.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_butClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
    End Sub



    Private Sub QueryFAQClass(ByVal iPageIndex As Integer)
        Dim oFAQ As New ctlFAQ.FAQClass()
        Dim dtFAQClass As New FaqDTO.FAQClassDataTable

        dtFAQClass = oFAQ.QueryAll()
        Call ArrangementFAQClass(dtFAQClass)

        Session("_dtFAQClass") = dtFAQClass

        Me.UI_dvFAQClass.PageSize = _PageSize
        Me.UI_dvFAQClass.PageIndex = iPageIndex
        Me.UI_dvFAQClass.DataSource = dtFAQClass.DefaultView
        Me.UI_dvFAQClass.DataBind()
    End Sub

    Private Sub ArrangementFAQClass(ByVal dtFAQClass As FaqDTO.FAQClassDataTable)
        Dim i As Integer = 0
        dtFAQClass.Columns.Add("Visible")

        For i = 0 To dtFAQClass.Rows.Count - 1
            dtFAQClass.Rows(i)("Visible") = _Status0
            If dtFAQClass.Rows(i)("FAQC_VISIBLE").ToString().Trim() = "1" Then
                dtFAQClass.Rows(i)("Visible") = _Status1
            End If
        Next

    End Sub

    Protected Sub UI_dvFAQClass_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvFAQClass.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("FAQ", "021", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("FAQ", "020", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("FAQ", "016", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("FAQ", "031", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("FAQ", "009", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgEdit As Button = e.Row.FindControl("imgEdit")
            Dim imgSave As Button = e.Row.FindControl("imgSave")
            Dim hidFAQC_VISIBLE As Label = e.Row.FindControl("hidFAQC_VISIBLE")
            Dim drpFAQC_VISIBLE As DropDownList = e.Row.FindControl("drpFAQC_VISIBLE")

            drpFAQC_VISIBLE.Items(0).Text = _oLanguage.getText("FAQ", "013", ctlLanguage.eumType.Tag)
            drpFAQC_VISIBLE.Items(1).Text = _oLanguage.getText("FAQ", "014", ctlLanguage.eumType.Tag)
            drpFAQC_VISIBLE.SelectedValue = hidFAQC_VISIBLE.Text.Trim()

            imgEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            imgSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If

    End Sub

    Protected Sub UI_dvFAQClass_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvFAQClass.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow
        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)

        If e.CommandName = "cmdEdit" Or e.CommandName = "cmdSave" Then
            row = Me.UI_dvFAQClass.Rows(iIndex)

            Dim lblFAQC_ID As Label = row.FindControl("lblFAQC_ID")
            Dim lblFAQC_CLASSNAME As Label = row.FindControl("lblFAQC_CLASSNAME")
            Dim txtFAQC_CLASSNAME As TextBox = row.FindControl("txtFAQC_CLASSNAME")

            Dim lblFAQC_VISIBLE As Label = row.FindControl("lblFAQC_VISIBLE")
            Dim drpFAQC_VISIBLE As DropDownList = row.FindControl("drpFAQC_VISIBLE")
            Dim hidFAQC_VISIBLE As Label = row.FindControl("hidFAQC_VISIBLE")

            Dim rfv_dvFAQClass_txtFAQC_CLASSNAME As RequiredFieldValidator = row.FindControl("rfv_dvFAQClass_txtFAQC_CLASSNAME")
            Dim vsFAQClass_Edit As ValidationSummary = row.FindControl("vsFAQClass_Edit")

            Dim imgEdit As Button = row.FindControl("imgEdit")
            Dim imgSave As Button = row.FindControl("imgSave")

            Select Case e.CommandName
                Case "cmdEdit"
                    lblFAQC_CLASSNAME.Visible = False
                    txtFAQC_CLASSNAME.Visible = True

                    lblFAQC_VISIBLE.Visible = False
                    drpFAQC_VISIBLE.Visible = True

                    imgEdit.Visible = False
                    imgSave.Visible = True

                    rfv_dvFAQClass_txtFAQC_CLASSNAME.ValidationGroup = "vsFAQClass_Edit_" & iIndex.ToString()
                    vsFAQClass_Edit.ValidationGroup = "vsFAQClass_Edit_" & iIndex.ToString()
                    imgSave.ValidationGroup = "vsFAQClass_Edit_" & iIndex.ToString()

                    Call setValidationMessage(rfv_dvFAQClass_txtFAQC_CLASSNAME)

                Case "cmdSave"
                    Call Save_FAQClass(lblFAQC_ID.Text.Trim(), txtFAQC_CLASSNAME.Text.Trim(), drpFAQC_VISIBLE.SelectedValue, eumCommand.UPDATE)
                    Call QueryFAQSubClass(Me.UI_dvFAQSubClass.PageIndex)
                    oCommon.getFQAClassByDropDownList(Me.UI_drpCategoryName1, Category1Text)
                    Call Parent_ReLoad()

                    lblFAQC_CLASSNAME.Visible = True
                    txtFAQC_CLASSNAME.Visible = False

                    lblFAQC_VISIBLE.Visible = True
                    drpFAQC_VISIBLE.Visible = False

                    imgEdit.Visible = True
                    imgSave.Visible = False

                    lblFAQC_CLASSNAME.Text = txtFAQC_CLASSNAME.Text.Trim()
                    lblFAQC_VISIBLE.Text = drpFAQC_VISIBLE.SelectedItem.Text.Trim()
                    hidFAQC_VISIBLE.Text = drpFAQC_VISIBLE.SelectedValue
            End Select
        End If

        Me.show = Me.show
    End Sub

    Protected Sub UI_dvFAQClass_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvFAQClass.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtFAQClass") Is Nothing Then
            Dim dtFAQClass As FaqDTO.FAQClassDataTable = Session("_dtFAQClass")

            Me.UI_dvFAQClass.PageIndex = iPageIndex
            Me.UI_dvFAQClass.DataSource = dtFAQClass.DefaultView
            Me.UI_dvFAQClass.DataBind()
        Else
            Call QueryFAQClass(iPageIndex)
        End If
        Me.show = True
    End Sub




    Private Sub QueryFAQSubClass(ByVal iPageIndex As Integer)
        Dim oFAQ As New ctlFAQ.FAQSubClass
        Dim dtFAQSubClass As New FaqDTO.vwFAQClassDataTable

        dtFAQSubClass = oFAQ.QueryAll()
        Call ArrangementFAQSubClass(dtFAQSubClass)

        Session("_dtFAQSubClass") = dtFAQSubClass

        Me.UI_dvFAQSubClass.PageSize = _PageSize
        Me.UI_dvFAQSubClass.PageIndex = iPageIndex
        Me.UI_dvFAQSubClass.DataSource = dtFAQSubClass.DefaultView
        Me.UI_dvFAQSubClass.DataBind()
    End Sub

    Private Sub ArrangementFAQSubClass(ByVal dtFAQSubClass As FaqDTO.vwFAQClassDataTable)
        Dim i As Integer = 0
        dtFAQSubClass.Columns.Add("Visible")

        For i = 0 To dtFAQSubClass.Rows.Count - 1
            dtFAQSubClass.Rows(i)("Visible") = _Status0
            If dtFAQSubClass.Rows(i)("FAQSC_VISIBLE").ToString().Trim() = "1" Then
                dtFAQSubClass.Rows(i)("Visible") = _Status1
            End If
        Next

    End Sub

    Protected Sub UI_UI_dvFAQSubClass_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvFAQSubClass.RowDataBound
        Dim i As Integer = 0
        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("FAQ", "021", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("FAQ", "024", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("FAQ", "020", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("FAQ", "016", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("FAQ", "031", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("FAQ", "009", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgEdit As Button = e.Row.FindControl("imgEdit")
            Dim imgSave As Button = e.Row.FindControl("imgSave")
            Dim hidFAQSC_VISIBLE As Label = e.Row.FindControl("hidFAQSC_VISIBLE")
            Dim drpFAQSC_VISIBLE As DropDownList = e.Row.FindControl("drpFAQSC_VISIBLE")
            drpFAQSC_VISIBLE.SelectedValue = hidFAQSC_VISIBLE.Text.Trim()

            Dim hidFAQSC_FAQCID As Label = e.Row.FindControl("hidFAQSC_FAQCID")
            Dim drpCategoryName1 As DropDownList = e.Row.FindControl("drpCategoryName1")

            drpFAQSC_VISIBLE.Items(0).Text = _oLanguage.getText("FAQ", "013", ctlLanguage.eumType.Tag)
            drpFAQSC_VISIBLE.Items(1).Text = _oLanguage.getText("FAQ", "014", ctlLanguage.eumType.Tag)
            oCommon.getFQAClassByDropDownList(drpCategoryName1, Category1Text)
            drpCategoryName1.SelectedValue = hidFAQSC_FAQCID.Text.Trim()
            drpCategoryName1.Items.RemoveAt(0)

            imgEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            imgSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If

    End Sub

    Protected Sub UI_dvFAQSubClass_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvFAQSubClass.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        If e.CommandName = "cmdEdit" Or e.CommandName = "cmdSave" Then
            row = Me.UI_dvFAQSubClass.Rows(iIndex)

            Dim lblFAQSC_ID As Label = row.FindControl("lblFAQSC_ID")

            Dim lblFAQC_CLASSNAME As Label = row.FindControl("lblFAQC_CLASSNAME")
            Dim hidFAQSC_FAQCID As Label = row.FindControl("hidFAQSC_FAQCID")
            Dim drpCategoryName1 As DropDownList = row.FindControl("drpCategoryName1")

            Dim lblFAQSC_CLASSNAME As Label = row.FindControl("lblFAQSC_CLASSNAME")
            Dim txtFAQSC_CLASSNAME As TextBox = row.FindControl("txtFAQSC_CLASSNAME")

            Dim lblFAQSC_VISIBLE As Label = row.FindControl("lblFAQSC_VISIBLE")
            Dim hidFAQSC_VISIBLE As Label = row.FindControl("hidFAQSC_VISIBLE")
            Dim drpFAQSC_VISIBLE As DropDownList = row.FindControl("drpFAQSC_VISIBLE")

            Dim rfv_dvFAQSubClass_txtFAQSC_CLASSNAME As RequiredFieldValidator = row.FindControl("rfv_dvFAQSubClass_txtFAQSC_CLASSNAME")
            Dim vsFAQSubClass_Edit As ValidationSummary = row.FindControl("vsFAQSubClass_Edit")

            Dim imgEdit As Button = row.FindControl("imgEdit")
            Dim imgSave As Button = row.FindControl("imgSave")

            Select Case e.CommandName
                Case "cmdEdit"
                    lblFAQC_CLASSNAME.Visible = False
                    drpCategoryName1.Visible = True

                    lblFAQSC_CLASSNAME.Visible = False
                    txtFAQSC_CLASSNAME.Visible = True

                    lblFAQSC_VISIBLE.Visible = False
                    drpFAQSC_VISIBLE.Visible = True

                    imgEdit.Visible = False
                    imgSave.Visible = True

                    rfv_dvFAQSubClass_txtFAQSC_CLASSNAME.ValidationGroup = "vsFAQSubClass_Edit_" & iIndex.ToString()
                    vsFAQSubClass_Edit.ValidationGroup = "vsFAQSubClass_Edit_" & iIndex.ToString()
                    imgSave.ValidationGroup = "vsFAQSubClass_Edit_" & iIndex.ToString()

                    Call setValidationMessage(rfv_dvFAQSubClass_txtFAQSC_CLASSNAME)

                Case "cmdSave"
                    Call Save_FAQSubClass(drpCategoryName1.SelectedValue, lblFAQSC_ID.Text.Trim(), txtFAQSC_CLASSNAME.Text.Trim(), drpFAQSC_VISIBLE.SelectedValue, eumCommand.UPDATE)
                    Call QueryFAQClass(Me.UI_dvFAQClass.PageIndex)
                    Call Parent_ReLoad()

                    lblFAQC_CLASSNAME.Visible = True
                    drpCategoryName1.Visible = False

                    lblFAQSC_CLASSNAME.Visible = True
                    txtFAQSC_CLASSNAME.Visible = False

                    lblFAQSC_VISIBLE.Visible = True
                    drpFAQSC_VISIBLE.Visible = False

                    imgEdit.Visible = True
                    imgSave.Visible = False

                    lblFAQC_CLASSNAME.Text = drpCategoryName1.SelectedItem.Text
                    hidFAQSC_FAQCID.Text = drpCategoryName1.SelectedValue

                    lblFAQSC_CLASSNAME.Text = txtFAQSC_CLASSNAME.Text.Trim()

                    lblFAQSC_VISIBLE.Text = drpFAQSC_VISIBLE.SelectedItem.Text
                    hidFAQSC_VISIBLE.Text = drpFAQSC_VISIBLE.SelectedValue
            End Select
        End If

        Me.show = Me.show
    End Sub

    Protected Sub UI_dvFAQSubClass_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvFAQSubClass.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtFAQSubClass") Is Nothing Then
            Dim dtFAQSubClass As FaqDTO.vwFAQClassDataTable = Session("_dtFAQSubClass")

            Me.UI_dvFAQSubClass.PageSize = _PageSize
            Me.UI_dvFAQSubClass.PageIndex = iPageIndex
            Me.UI_dvFAQSubClass.DataSource = dtFAQSubClass.DefaultView
            Me.UI_dvFAQSubClass.DataBind()
        Else
            Call QueryFAQSubClass(iPageIndex)
        End If
        Me.show = True
    End Sub





    ''' <summary>
    ''' 大類新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd1.Click
        Call Save_FAQClass("", Me.UI_txtFAQC_ClassName.Text.Trim(), UI_opgFAQC_Visible.SelectedValue, eumCommand.AddNew)

        Call QueryFAQClass(0)
        Call QueryFAQSubClass(Me.UI_dvFAQSubClass.PageIndex)

        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(Me.UI_drpCategoryName1, Category1Text)
        Call Parent_ReLoad()
        Me.show = Me.show
    End Sub

    ''' <summary>
    ''' 大類資料 新增or修改
    ''' </summary>
    ''' <param name="FAQC_ID">FAQ大類代碼</param>
    ''' <param name="FAQC_CLASSNAME">類別名稱</param>
    ''' <param name="FAQC_VISIBLE">是否顯示(1:顯示 , 0:不顯示)</param>
    ''' <param name="eumCommand">Command型態</param>
    ''' <remarks></remarks>
    Private Sub Save_FAQClass(ByVal FAQC_ID As String, ByVal FAQC_CLASSNAME As String, ByVal FAQC_VISIBLE As Integer, ByVal eumCommand As eumCommand)
        Dim oFAQClass As New ctlFAQ.FAQClass()
        Dim dtFAQ As New FaqDTO.FAQClassDataTable

        Try
            Dim dr As FaqDTO.FAQClassRow = dtFAQ.NewFAQClassRow

            dr.FAQC_ID = FAQC_ID
            dr.FAQC_CLASSNAME = FAQC_CLASSNAME
            dr.FAQC_TYPE = 1
            dr.FAQC_VISIBLE = FAQC_VISIBLE
            dr.FAQC_AD = Session("_UserID")
            dr.FAQC_ADNAME = Session("_UserName")
            dr.FAQC_CSTMP = Date.Now
            dr.FAQC_LUAD = Session("_UserID")
            dr.FAQC_LUADNAME = Session("_UserName")
            dr.FAQC_LUSTMP = Date.Now
            dr.FAQC_MARK = 0

            dtFAQ.AddFAQClassRow(dr)

            Select Case eumCommand
                Case eumCommand.AddNew
                    oFAQClass.SaveAdd(dtFAQ)

                Case eumCommand.UPDATE
                    oFAQClass.SaveEdit(dtFAQ)
            End Select

        Catch ex As Exception
            Throw ex

        End Try

    End Sub


    ''' <summary>
    ''' 小類新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd2.Click
        Call Save_FAQSubClass(Me.UI_drpCategoryName1.SelectedValue, "", Me.UI_txtFAQSC_ClassName.Text.Trim(), UI_opgFAQSC_Visible.SelectedValue, eumCommand.AddNew)

        Call QueryFAQClass(Me.UI_dvFAQClass.PageIndex)
        Call QueryFAQSubClass(0)
        Call Parent_ReLoad()
        Me.show = Me.show
    End Sub

    ''' <summary>
    ''' 小類資料 新增or修改
    ''' </summary>
    ''' <param name="FAQSC_FAQCID">FAQ大類代碼</param>
    ''' <param name="FAQSC_ID">FAQ小類代碼</param>
    ''' <param name="FAQC_CLASSNAME">類別名稱</param>
    ''' <param name="FAQC_VISIBLE">是否顯示(1:顯示 , 0:不顯示)</param>
    ''' <param name="eumCommand">Command型態</param>
    ''' <remarks></remarks>
    Private Sub Save_FAQSubClass(ByVal FAQSC_FAQCID As String, ByVal FAQSC_ID As String, ByVal FAQC_CLASSNAME As String, ByVal FAQC_VISIBLE As Integer, ByVal eumCommand As eumCommand)
        Dim oFAQSubClass As New ctlFAQ.FAQSubClass()
        Dim dtFAQ As New FaqDTO.FAQSubClassDataTable

        Try
            Dim dr As FaqDTO.FAQSubClassRow = dtFAQ.NewFAQSubClassRow

            dr.FAQSC_ID = FAQSC_ID
            dr.FAQSC_FAQCID = FAQSC_FAQCID

            dr.FAQSC_CLASSNAME = FAQC_CLASSNAME
            dr.FAQSC_VISIBLE = FAQC_VISIBLE
            dr.FAQSC_AD = Session("_UserID")
            dr.FAQSC_ADNAME = Session("_UserName")
            dr.FAQSC_CSTMP = Date.Now
            dr.FAQSC_LUAD = Session("_UserID")
            dr.FAQSC_LUADNAME = Session("_UserName")
            dr.FAQSC_LUSTMP = Date.Now
            dr.FAQSC_MARK = 0

            dtFAQ.AddFAQSubClassRow(dr)

            Select Case eumCommand
                Case eumCommand.AddNew
                    oFAQSubClass.SaveAdd(dtFAQ)

                Case eumCommand.UPDATE
                    oFAQSubClass.SaveEdit(dtFAQ)
            End Select

        Catch ex As Exception
            Throw ex

        End Try

    End Sub








    ''' <summary>
    ''' 重新再載入主頁的(FAQ.aspx)Category1及Category2
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Parent_ReLoad()
        Dim UI_cboCategory1 As DropDownList = Me.Parent.FindControl("UI_cboCategory1")
        Dim UI_cboCategory2 As DropDownList = Me.Parent.FindControl("UI_cboCategory2")

        Dim Category1Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        Dim Category2Text As String = _oLanguage.getText("FAQ", "005", ctlLanguage.eumType.Tag)
        oCommon.getFQAClassByDropDownList(UI_cboCategory1, Category1Text)
        oCommon.getFQASubClassByDropDownList(UI_cboCategory1.SelectedValue.ToString().Trim(), UI_cboCategory2, Category2Text)
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""
        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfv_dvFAQClass_txtFAQC_CLASSNAME".ToLower()       '按下大類修改 Validation 的錯誤訊息
                sErrorMessage = _oLanguage.getText("FAQ", "029", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_txtFAQC_ClassName".ToLower()                  '按下大類新增 Validation 的錯誤訊息
                sErrorMessage = _oLanguage.getText("FAQ", "029", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cv_drpCategoryName1".ToLower()                    '按下小類新增 Validation 的錯誤訊息
                sErrorMessage = _oLanguage.getText("FAQ", "025", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_txtFAQSC_ClassName".ToLower()                 '按下小類新增 Validation 的錯誤訊息
                sErrorMessage = _oLanguage.getText("FAQ", "030", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_dvFAQSubClass_txtFAQSC_CLASSNAME".ToLower()   '按下小類新增 Validation 的錯誤訊息
                sErrorMessage = _oLanguage.getText("FAQ", "030", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
        End Select
    End Sub

    ''' <summary>
    ''' 設定是否要顯示
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 Boolean</returns>
    ''' <remarks></remarks>
    Public Property show() As Boolean
        Get
            Dim retval As Boolean = False
            If Convert.ToBoolean(Me.ViewState("_show")) = True Then
                retval = True
            End If
            Return retval
        End Get

        Set(ByVal nNewValue As Boolean)
            If nNewValue = True Then
                Me.ajModalProgress.Show()
            Else
                Me.ajModalProgress.Hide()
            End If

            Me.ViewState("_show") = nNewValue
        End Set
    End Property






End Class

Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class RepairCenter
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call setDefault()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_CompNo As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_CompNo")
                Me.UI_lblPreviousPage_CompNo.Text = UI_lblPreviousPage_CompNo.Text.ToString().Trim()

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.UI_lblPreviousPage_CompNo.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()
                Call QueryCompany()
                Call QueryRepairBOM(0)
            End If
        End If



    End Sub
#End Region

    Private Sub setDefault()
        Session("_dtRepairBOM") = Nothing
        Me.ViewState("_eumCommandPart") = eumCommand.UPDATE        '紀錄目前是否再做 Part Information 新增

        Me.UI_txtPart.Text = ""
        Me.ViewState("_PartKeyWord") = Me.UI_txtPart.Text
    End Sub

    Private Sub setControls()
        Dim sClientID As String = Me.UI_cmdAdd.ClientID & "," & Me.UI_cmdSearch.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID

        '設定 Enter 鍵觸發
        Me.UI_txtPart.Attributes.Add("OnKeypress", "return clickButton(event,'" & Me.UI_cmdSearch.ClientID & "')")

        Dim sRepairText As String = _oLanguage.getText("RepairCenter", "024", ctlLanguage.eumType.Tag)
        oCommon.getCountryByDropDownList(Me.UI_cboCountry, sRepairText)
        oCommon.getCurrencyByDropDownList(Me.UI_cboCurrency, sRepairText)

        Call setValidationMessage()

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RepairCenter", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCompName.Text = _oLanguage.getText("RepairCenter", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblCompNo.Text = _oLanguage.getText("RepairCenter", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblCountry.Text = _oLanguage.getText("RepairCenter", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblCurrency.Text = _oLanguage.getText("RepairCenter", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborCost.Text = _oLanguage.getText("RepairCenter", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblManHour.Text = _oLanguage.getText("RepairCenter", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblApprovalAmount.Text = _oLanguage.getText("RepairCenter", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblLowestDiscount.Text = _oLanguage.getText("RepairCenter", "014", ctlLanguage.eumType.Tag)
        Me.UI_lblTEL.Text = _oLanguage.getText("RepairCenter", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RepairCenter", "016", ctlLanguage.eumType.Tag)
        Me.UI_lblExpress.Text = _oLanguage.getText("RepairCenter", "017", ctlLanguage.eumType.Tag)
        Me.UI_lblExpressURL.Text = _oLanguage.getText("RepairCenter", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblVisible.Text = _oLanguage.getText("RepairCenter", "007", ctlLanguage.eumType.Tag)
        Me.UI_opgVisible.Items(0).Text = _oLanguage.getText("RepairCenter", "019", ctlLanguage.eumType.Tag)
        Me.UI_opgVisible.Items(1).Text = _oLanguage.getText("RepairCenter", "020", ctlLanguage.eumType.Tag)
        Me.UI_lblRemark.Text = _oLanguage.getText("RepairCenter", "021", ctlLanguage.eumType.Tag)
        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RepairCenter", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblPart.Text = _oLanguage.getText("RepairCenter", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblStockManager.Text = _oLanguage.getText("RepairCenter", "044", ctlLanguage.eumType.Tag)


        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        Me.UI_cmdPartSubmit.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Call show_PartInformation()

    End Sub

    Private Sub show_PartInformation()
        Dim blnFlag As Boolean = False

        '控制是否顯示物件
        If Me.ViewState("_eumCommand") = eumCommand.UPDATE Then
            blnFlag = True
        End If

        Me.UI_lblInformationTittle.Visible = blnFlag
        'Me.UI_cmdAdd.Visible = blnFlag
        Me.UI_lblPart.Visible = blnFlag
        Me.UI_txtPart.Visible = blnFlag
        Me.UI_cmdSearch.Visible = blnFlag
        Me.UI_cmdCancel.Visible = blnFlag
        Me.UI_cmdPartSubmit.Visible = blnFlag
        Me.UI_trPart.Visible = blnFlag

    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setValidationMessage()
        Me.rfvCompName.ErrorMessage = _oLanguage.getText("RepairCenter", "029", ctlLanguage.eumType.Validator)
        Me.rfvCompNo.ErrorMessage = _oLanguage.getText("RepairCenter", "030", ctlLanguage.eumType.Validator)
        Me.cvCountry.ErrorMessage = _oLanguage.getText("RepairCenter", "031", ctlLanguage.eumType.Validator)
        Me.cvCurrency.ErrorMessage = _oLanguage.getText("RepairCenter", "032", ctlLanguage.eumType.Validator)

        Me.rfvLaborCost.ErrorMessage = _oLanguage.getText("RepairCenter", "033", ctlLanguage.eumType.Validator)
        Me.rvLaborCost.ErrorMessage = _oLanguage.getText("RepairCenter", "034", ctlLanguage.eumType.Validator)
        Me.rfvApprovalAmount.ErrorMessage = _oLanguage.getText("RepairCenter", "038", ctlLanguage.eumType.Validator)
        Me.rvApprovalAmount.ErrorMessage = _oLanguage.getText("RepairCenter", "039", ctlLanguage.eumType.Validator)
        Me.rfvLowestDiscount.ErrorMessage = _oLanguage.getText("RepairCenter", "040", ctlLanguage.eumType.Validator)
        Me.rvLowestDiscount.ErrorMessage = _oLanguage.getText("RepairCenter", "041", ctlLanguage.eumType.Validator)

    End Sub

    Private Sub QueryCompany()
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        dtCompany = oCompany.QueryByPrimaryKey(Me.UI_lblPreviousPage_CompNo.Text)
        If dtCompany.Rows.Count > 0 Then
            Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(0)

            Me.UI_lblCompNo1.Text = dr.COMP_NO.ToString().Trim()
            Me.UI_txtCompNo.Text = dr.COMP_NO.ToString().Trim()

            Me.UI_lblCompNo1.Visible = False
            Me.UI_txtCompNo.Visible = False
            If Me.ViewState("_eumCommand") = eumCommand.UPDATE Then
                Me.UI_lblCompNo1.Visible = True
            Else
                Me.UI_txtCompNo.Visible = True
            End If


            Me.UI_txtCompName.Text = dr.COMP_NAME.ToString().Trim()

            Me.UI_txtTEL.Text = ""
            If dr.IsCOMP_TELNull = False Then Me.UI_txtTEL.Text = dr.COMP_TEL.ToString().Trim()

            Me.UI_txtAddress.Text = ""
            If dr.IsCOMP_ADDRESSNull = False Then Me.UI_txtAddress.Text = dr.COMP_ADDRESS.ToString().Trim()

            Me.UI_cboCountry.SelectedValue = dr.COMP_COUNTRYID.Trim()
            Me.UI_cboCurrency.SelectedValue = dr.COMP_CURRENCYCODE.Trim()

            Me.UI_txtLaborCost.Text = dr.COMP_LABORCOST.ToString().Trim()
            Me.UI_txtApprovalAmount.Text = dr.COMP_APPROVALAMOUNT.ToString().Trim()
            Me.UI_txtLowestDiscount.Text = dr.COMP_LOWESTDISCOUNT.ToString().Trim()

            Me.UI_opgVisible.SelectedValue = dr.COMP_VISIBLE.ToString().Trim()

            Me.UI_txtExpress.Text = ""
            If dr.IsCOMP_EXPRESSCONull = False Then Me.UI_txtExpress.Text = dr.COMP_EXPRESSCO.ToString().Trim()

            Me.UI_txtExpressURL.Text = ""
            If dr.IsCOMP_EXPRESSURLNull = False Then Me.UI_txtExpressURL.Text = dr.COMP_EXPRESSURL.ToString().Trim()

            Me.UI_txtStockManager.Text = ""
            If dr.IsCOMP_STOCKMANAGERNull = False Then Me.UI_txtStockManager.Text = dr.COMP_STOCKMANAGER.ToString().Trim()

            Me.UI_txtRemark.Text = ""
            If dr.IsCOMP_REMARKNull = False Then Me.UI_txtRemark.Text = dr.COMP_REMARK.ToString().Trim()
        End If

    End Sub

    ''' <summary>
    ''' RepairCenter - 儲存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim oCompany As New ctlCompany

        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                blnFlag = oCompany.chkIsExist(Me.UI_txtCompNo.Text.Trim(), "")

            Case eumCommand.UPDATE
                blnFlag = oCompany.chkIsExist(Me.UI_txtCompNo.Text.Trim(), Me.UI_lblPreviousPage_CompNo.Text.Trim())
        End Select

        If blnFlag = True Then
            sMessage = _oLanguage.getText("RepairCenter", "042", ctlLanguage.eumType.Validator)
            Me.ucMessage.showMessageByFailed(sMessage)

        Else
            Call Save()
            Call setDefault()
            Call show_PartInformation()
            Call QueryRepairBOM(0)
        End If

    End Sub

    Private Sub Save()
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        Try
            Dim dr As CompanyDTO.CompanyRow = dtCompany.NewRow

            dr.COMP_NO = Me.UI_txtCompNo.Text.Trim()
            dr.COMP_NAME = Me.UI_txtCompName.Text.Trim()
            dr.COMP_INVOICE = ""

            dr.COMP_TEL = Me.UI_txtTEL.Text.Trim()
            dr.COMP_ADDRESS = Me.UI_txtAddress.Text.Trim()
            dr.COMP_COUNTRYID = Me.UI_cboCountry.SelectedValue.Trim()
            dr.COMP_CURRENCYCODE = Me.UI_cboCurrency.SelectedValue.Trim()
            dr.COMP_URL = ""

            dr.COMP_LABORCOST = Convert.ToDecimal(Me.UI_txtLaborCost.Text)
            dr.COMP_APPROVALAMOUNT = Convert.ToDecimal(Me.UI_txtApprovalAmount.Text)
            dr.COMP_LOWESTDISCOUNT = Convert.ToDecimal(Me.UI_txtLowestDiscount.Text)

            dr.COMP_ISROLE = "1"            '公司角色:1.維修中心
            dr.COMP_ISREPAIR = "1"          '是否附設維修中心:0.否, 1.是

            dr.COMP_VISIBLE = Convert.ToInt16(Me.UI_opgVisible.SelectedValue)
            dr.COMP_EXPRESSCO = Me.UI_txtExpress.Text.Trim()
            dr.COMP_EXPRESSURL = Me.UI_txtExpressURL.Text.Trim()
            dr.COMP_STOCKMANAGER = Me.UI_txtStockManager.Text.Trim()
            dr.COMP_REMARK = Me.UI_txtRemark.Text.Trim()

            dr.COMP_AD = Session("_UserID")
            dr.COMP_ADNAME = Session("_UserName")
            dr.COMP_CSTMP = Date.Now
            dr.COMP_LUAD = Session("_UserID")
            dr.COMP_LUADNAME = Session("_UserName")
            dr.COMP_LUSTMP = Date.Now

            dtCompany.AddCompanyRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oCompany.SaveAdd(dtCompany)

                Case eumCommand.UPDATE
                    oCompany.SaveEdit(dtCompany, Me.UI_lblPreviousPage_CompNo.Text)
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

                Me.ViewState("_eumCommand") = eumCommand.UPDATE
                Me.UI_lblPreviousPage_CompNo.Text = Me.UI_txtCompNo.Text.Trim()
            End If
        End Try




    End Sub

    Private Sub QueryRepairBOM(ByVal iPageIndex As Integer)
        Dim oRepairBOM As New ctlRMA.RepairBOM
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable

        Me.ViewState("_eumCommandPart") = eumCommand.UPDATE

        Dim Comp_No As String = Me.UI_lblPreviousPage_CompNo.Text.Trim
        Dim PartNo As String = Me.ViewState("_PartKeyWord").ToString().Trim()

        dtRepairBOM = oRepairBOM.QueryByPartNo(Comp_No, PartNo)
        Call RepairBOM_DataBind(dtRepairBOM, iPageIndex)
    End Sub

    Private Sub RepairBOM_DataBind(ByVal dtRepairBOM As RmaDTO.RepairBOMDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtRepairBOM)

        Session("_dtRepairBOM") = dtRepairBOM

        If Me.ViewState("_eumCommandPart") = eumCommand.UPDATE Then
            Me.UI_RepairBOM.AllowPaging = True
            Me.UI_RepairBOM.PageSize = _PageSize
            Me.UI_RepairBOM.PageIndex = iPageIndex
        Else
            Me.UI_RepairBOM.AllowPaging = False
        End If

        Me.UI_RepairBOM.DataSource = dtRepairBOM.DefaultView
        Me.UI_RepairBOM.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRepairBOM As RmaDTO.RepairBOMDataTable)
        Dim i As Integer = 0

        If dtRepairBOM.Columns("SeqID") Is Nothing Then
            dtRepairBOM.Columns.Add("SeqID")
        End If

        For i = 0 To dtRepairBOM.Rows.Count - 1
            dtRepairBOM.Rows(i)("SeqID") = i + 1
        Next
    End Sub

    Protected Sub UI_RepairBOM_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_RepairBOM.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        '刪除
        If e.CommandName = "cmdDel" Then
            Dim oRepairBOM As New ctlRMA.RepairBOM

            System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

            Call Keep_RepairBOM_Data()

            row = Me.UI_RepairBOM.Rows(iIndex)
            Dim UI_SeqID As Label = row.FindControl("UI_SeqID")
            Dim UI_Rpbom_CompNo As Label = row.FindControl("UI_Rpbom_CompNo")
            Dim UI_oldRpbomPartNo As Label = row.FindControl("UI_oldRpbomPartNo")

            Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable
            dtRepairBOM = Session("_dtRepairBOM")
            Dim dvRepairBOM As DataView = dtRepairBOM.DefaultView
            dvRepairBOM.RowFilter = "SeqID='" & UI_SeqID.Text.Trim() & "'"
            If dvRepairBOM.Count > 0 Then
                dvRepairBOM.Delete(0)
                If UI_oldRpbomPartNo.Text <> "" Then
                    oRepairBOM.Delete(UI_Rpbom_CompNo.Text, UI_oldRpbomPartNo.Text)
                End If
            End If

            dvRepairBOM.RowFilter = ""
            Session("_dtRepairBOM") = dtRepairBOM
            Call RepairBOM_DataBind(dtRepairBOM, Me.UI_RepairBOM.PageIndex)
        End If
    End Sub

    Protected Sub UI_RepairBOM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_RepairBOM.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RepairCenter", "023", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RepairCenter", "025", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RepairCenter", "026", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RepairCenter", "027", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RepairCenter", "028", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RpbomPartNo As TextBox = e.Row.FindControl("UI_RpbomPartNo")
            Dim UI_RpbomMateriaLcost As TextBox = e.Row.FindControl("UI_RpbomMateriaLcost")

            Dim rfv_RpbomPartNo As RequiredFieldValidator = e.Row.FindControl("rfv_RpbomPartNo")
            rfv_RpbomPartNo.ControlToValidate = UI_RpbomPartNo.ID
            rfv_RpbomPartNo.ErrorMessage = _oLanguage.getText("RepairCenter", "035", ctlLanguage.eumType.Validator)


            Dim rfv_RpbomMateriaLcost As RequiredFieldValidator = e.Row.FindControl("rfv_RpbomMateriaLcost")
            Dim rv_RpbomMateriaLcost As RangeValidator = e.Row.FindControl("rv_RpbomMateriaLcost")
            rfv_RpbomMateriaLcost.ControlToValidate = UI_RpbomMateriaLcost.ID
            rv_RpbomMateriaLcost.ControlToValidate = UI_RpbomMateriaLcost.ID
            rfv_RpbomMateriaLcost.ErrorMessage = _oLanguage.getText("RepairCenter", "036", ctlLanguage.eumType.Validator)
            rv_RpbomMateriaLcost.ErrorMessage = _oLanguage.getText("RepairCenter", "037", ctlLanguage.eumType.Validator)

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If

                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "DataControlPagerLinkButton".ToLower() Then
                    Dim oLinkButton As LinkButton = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLinkButton.Attributes.Add("onclick", "onProgress('Process')")
                End If

            Next
        End If

    End Sub

    Protected Sub UI_RepairBOM_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_RepairBOM.PageIndexChanging
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRepairBOM") Is Nothing Then
            Dim dtRepairBOM As RmaDTO.RepairBOMDataTable = Session("_dtRepairBOM")
            Call RepairBOM_DataBind(dtRepairBOM, iPageIndex)

        Else
            Call QueryRepairBOM(iPageIndex)
        End If
    End Sub

    ''' <summary>
    ''' 紀錄 UI_RepairBOM 裡的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Keep_RepairBOM_Data()
        Dim i As Integer = 0
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable
        dtRepairBOM = Session("_dtRepairBOM")

        Dim dvRepairBOM As DataView = dtRepairBOM.DefaultView
        For i = 0 To Me.UI_RepairBOM.Rows.Count - 1
            If Me.UI_RepairBOM.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_SeqID As Label = Me.UI_RepairBOM.Rows(i).FindControl("UI_SeqID")
                Dim UI_RpbomPartNo As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomPartNo")
                Dim UI_RpbomLocation As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomLocation")
                Dim UI_RpbomMateriaLcost As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomMateriaLcost")
                Dim UI_RpbomDesc As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomDesc")

                dvRepairBOM.RowFilter = "SeqID='" & UI_SeqID.Text.Trim() & "'"
                If dvRepairBOM.Count > 0 Then
                    Dim dr As RmaDTO.RepairBOMRow = dvRepairBOM(0).Row
                    dr.RPBOM_PARTNO = UI_RpbomPartNo.Text.Trim()
                    dr.RPBOM_LOCATION = UI_RpbomLocation.Text.Trim()
                    dr.RPBOM_DESC = UI_RpbomDesc.Text.Trim()
                    dr.RPBOM_MATERIALCOST = Convert.ToDecimal(UI_RpbomMateriaLcost.Text.Trim())
                End If
            End If
        Next
        dvRepairBOM.RowFilter = ""
        Session("_dtRepairBOM") = dtRepairBOM
    End Sub

    ''' <summary>
    ''' Part Information 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRepairBOM As New ctlRMA.RepairBOM
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable

        If Me.ViewState("_eumCommandPart") = eumCommand.AddNew Then
            Call Keep_RepairBOM_Data()
            dtRepairBOM = Session("_dtRepairBOM")
        Else
            Me.ViewState("_eumCommandPart") = eumCommand.AddNew
        End If


        Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM.NewRepairBOMRow
        dr.RPBOM_COMPNO = Me.UI_lblPreviousPage_CompNo.Text.Trim()
        dr.RPBOM_PARTNO = ""
        dr.RPBOM_oldPARTNO = ""
        dr.RPBOM_LOCATION = ""
        dr.RPBOM_DESC = ""
        dr.RPBOM_MATERIALCOST = 0

        dr.RPBOM_AD = Session("_UserID")
        dr.RPBOM_ADNAME = Session("_UserName")
        dr.RPBOM_CSTMP = Date.Now
        dr.RPBOM_LUAD = Session("_UserID")
        dr.RPBOM_LUADNAME = Session("_UserName")
        dr.RPBOM_LUSTMP = Date.Now

        dtRepairBOM.AddRepairBOMRow(dr)

        Call RepairBOM_DataBind(dtRepairBOM, 0)
    End Sub

    ''' <summary>
    ''' Part Information 取消
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call setDefault()
        Call setControls()
        Call QueryCompany()
        Call QueryRepairBOM(0)
    End Sub

    ''' <summary>
    ''' Part Information 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ViewState("_PartKeyWord") = Me.UI_txtPart.Text
        Call QueryRepairBOM(0)

        Dim oScriptManager As ScriptManager = Me.Master.FindControl("ScriptManager")
        oScriptManager.SetFocus(Me.UI_txtPart)
    End Sub

    ''' <summary>
    ''' Part Information 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPartSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim oRepairBOM As New ctlRMA.RepairBOM
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable

        Try

            For i = 0 To Me.UI_RepairBOM.Rows.Count - 1
                If Me.UI_RepairBOM.Rows(i).RowType = DataControlRowType.DataRow Then

                    Dim UI_oldRpbomPartNo As Label = Me.UI_RepairBOM.Rows(i).FindControl("UI_oldRpbomPartNo")
                    Dim UI_RpbomPartNo As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomPartNo")
                    Dim UI_RpbomLocation As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomLocation")
                    Dim UI_RpbomMateriaLcost As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomMateriaLcost")
                    Dim UI_RpbomDesc As TextBox = Me.UI_RepairBOM.Rows(i).FindControl("UI_RpbomDesc")

                    Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM.NewRepairBOMRow
                    dr.RPBOM_COMPNO = Me.UI_lblPreviousPage_CompNo.Text.Trim()
                    dr.RPBOM_PARTNO = UI_RpbomPartNo.Text.Trim()
                    dr.RPBOM_oldPARTNO = UI_oldRpbomPartNo.Text.Trim()
                    dr.RPBOM_LOCATION = UI_RpbomLocation.Text.Trim()
                    dr.RPBOM_DESC = UI_RpbomDesc.Text.Trim()
                    dr.RPBOM_MATERIALCOST = Convert.ToDecimal(UI_RpbomMateriaLcost.Text.Trim())

                    dr.RPBOM_AD = Session("_UserID")
                    dr.RPBOM_ADNAME = Session("_UserName")
                    dr.RPBOM_CSTMP = Date.Now
                    dr.RPBOM_LUAD = Session("_UserID")
                    dr.RPBOM_LUADNAME = Session("_UserName")
                    dr.RPBOM_LUSTMP = Date.Now

                    dtRepairBOM.AddRepairBOMRow(dr)
                End If
            Next
            oRepairBOM.Save(dtRepairBOM)
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Me.ucMessage.showMessageByAlert(oCommon.getMessage(Common.enmMessage.EditOK))
            End If

            Call setDefault()
            Call QueryRepairBOM(0)
        End Try

    End Sub

End Class

Imports System.Data
Imports System.Web.Services
Imports System.Xml         '140701 by cipherlab.fairy
Imports DataService
Imports DefLanguage

Partial Class Warranty_TypeSetting_add
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim sUploadPath As String = ConfigurationSettings.AppSettings("EmailAttachFolder")
    Dim sDiscount As String = ConfigurationSettings.AppSettings("DefaultDiscount")

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim lblPreviousPage_SwID As Label = oContentPlaceHolder.FindControl("lblPreviousPage_SwID")
                Dim lblPreviousPage_Type As Label = oContentPlaceHolder.FindControl("lblPreviousPage_Type")
                Me.lblSwID.Text = lblPreviousPage_SwID.Text.ToString().Trim()
                Dim sWar_id As String = Me.lblSwID.Text.ToString().Trim()

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.lblSwID.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()

                Dim oWarranty As New ctlWarranty
                Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
                dtProductGroup = oWarranty.QueryPrdGroup("", "")
                ViewState("_dtProductGroup") = dtProductGroup

                Dim dtWarrPartsType As New WarrantyDTO.WarrPartsTypeDataTable
                dtWarrPartsType = oWarranty.QueryWarrPartsType(Session("_LanguageID").ToString(), "", "")
                ViewState("dtWarrPartsType") = dtWarrPartsType
                cboPartsType.DataSource = dtWarrPartsType
                cboPartsType.DataTextField = "TYPE_NAME"
                cboPartsType.DataValueField = "TYPE_NO"
                cboPartsType.DataBind()

                'Spec Type
                Dim dtWarrSPECsType As New DataTable
                dtWarrSPECsType = oWarranty.QueryWarrSpecsType(Session("_LanguageID").ToString(), "", "")
                ViewState("dtWarrSpcsType") = dtWarrSPECsType
                cboSpecsType.DataSource = dtWarrSPECsType
                cboSpecsType.DataTextField = "SPEC_NAME"
                cboSpecsType.DataValueField = "SPEC_NO"
                cboSpecsType.DataBind()

                If sWar_id = "" Then
                    sWar_id = "$$"
                End If
                Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
                dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, "", "")
                ViewState("dtWarrParts") = dtWarrParts
                lstParts.DataSource = dtWarrParts
                lstParts.DataBind()
                Dim i As Integer = 0
                For i = 0 To dtWarrParts.Rows.Count - 1
                    cboPartsType.Items.Remove(cboPartsType.Items.FindByValue(dtWarrParts.Rows(i)("WAP_NAME").ToString()))
                Next

                'Spec
                Dim dtWarrSpecs As New DataTable
                dtWarrSpecs = oWarranty.QueryWarrSpecs(Session("_LanguageID").ToString(), sWar_id, "", "")
                ViewState("dtWarrSpecs") = dtWarrSpecs
                lstSpecs.DataSource = dtWarrSpecs
                lstSpecs.DataBind()
                For i = 0 To dtWarrSpecs.Rows.Count - 1
                    cboSpecsType.Items.Remove(cboSpecsType.Items.FindByValue(dtWarrSpecs.Rows(i)("WAP_NAME").ToString()))
                Next



                'Dim dtParts As New WarrantyDTO.vwPartsDataTable
                'dtParts = oWarranty.QueryParts("", "")
                'ViewState("_dtParts") = dtParts
                UI_txtDiscount.Text = sDiscount
                Call QueryData()

                If lblPreviousPage_Type.Text.Trim() = "C" Then
                    btnInValid.Visible = False
                End If
            End If
        End If
    End Sub

    'Private Sub setBattaryService(sRepairText As String)
    '    Dim oListItem As ListItem
    '    oListItem = New ListItem
    '    oListItem.Text = sRepairText
    '    oListItem.Value = "-1"
    '    UI_cboProjectType.Items.Add(oListItem)

    '    oListItem = New ListItem
    '    oListItem.Text = "Standard"
    '    oListItem.Value = "Standard"
    '    UI_cboProjectType.Items.Add(oListItem)

    '    oListItem = New ListItem
    '    oListItem.Text = "Premium"
    '    oListItem.Value = "Premium"
    '    UI_cboProjectType.Items.Add(oListItem)

    'End Sub

    Private Sub setControls()
        Me.rfvMonth.ErrorMessage = _oLanguage.getText("Warranty", "101", ctlLanguage.eumType.Validator)
        Me.rfvProductGroup.ErrorMessage = _oLanguage.getText("Warranty", "102", ctlLanguage.eumType.Validator)
        Me.rfvWarrName.ErrorMessage = _oLanguage.getText("Warranty", "103", ctlLanguage.eumType.Validator)
        Me.rfvDiscount.ErrorMessage = _oLanguage.getText("Warranty", "106", ctlLanguage.eumType.Validator)
        Me.rfvLongestYears.ErrorMessage = _oLanguage.getText("Warranty", "107", ctlLanguage.eumType.Validator)
        Me.rfvStandardYears.ErrorMessage = _oLanguage.getText("Warranty", "108", ctlLanguage.eumType.Validator)

        Me.rfvITEM_TYPE.ErrorMessage = _oLanguage.getText("Warranty", "109", ctlLanguage.eumType.Validator)
        Me.rfvPROGRAM_TYPE.ErrorMessage = _oLanguage.getText("Warranty", "110", ctlLanguage.eumType.Validator)
        Me.rfvPRICE_VER.ErrorMessage = _oLanguage.getText("Warranty", "111", ctlLanguage.eumType.Validator)

        Dim sRepairText As String = ""
        oCommon.getWarrsetTypeByDropDownList(Me.UI_cboWarrantyType, sRepairText, Me.ViewState("_eumCommand"))

        'Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getPriceVerByDropDownList(Me.UI_cboPriceVer, UI_cboWarrantyType.SelectedValue, sRepairText)

        Dim sProjectTypeText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getProgramTypeByDropDownList(Me.UI_cboProgramType, UI_cboWarrantyType.SelectedValue, sRepairText)
        'setBattaryService(sRepairText)

        oCommon.getItemTypeByDropDownList(Me.UI_cboItemType, UI_cboWarrantyType.SelectedValue, sRepairText)

        UI_lblItemtype.Text = _oLanguage.getText("Warranty", "082", ctlLanguage.eumType.Tag)
        UI_lblPriceVer.Text = _oLanguage.getText("Warranty", "083", ctlLanguage.eumType.Tag)
        UI_lblProgramType.Text = _oLanguage.getText("Warranty", "084", ctlLanguage.eumType.Tag)

        '取得Tag Text
        Call oCommon.getCostCenterByDropDownList(False, Me.UI_cboOperationCenter, "")
        'Call oCommon.getExtensionWarrantyTypeByDropDownList(False, Me.UI_cboWarrantyType, "")
        Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "062", ctlLanguage.eumType.Tag)
        Me.UI_lblSubTittle.Text = _oLanguage.getText("Warranty", "038", ctlLanguage.eumType.Tag)
        Me.UI_lblOperationCenter.Text = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyType.Text = _oLanguage.getText("Warranty", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblExtraMonths.Text = _oLanguage.getText("Warranty", "031", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyName.Text = _oLanguage.getText("Warranty", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblYears.Text = _oLanguage.getText("Warranty", "050", ctlLanguage.eumType.Tag)
        Me.UI_lblVersion.Text = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("Warranty", "036", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrSpecDesc.Text = _oLanguage.getText("Warranty", "112", ctlLanguage.eumType.Tag)
        Me.UI_lblWar_longyy.Text = _oLanguage.getText("Warranty", "051", ctlLanguage.eumType.Tag)
        Me.UI_lblDiscount.Text = "Discount Off" '_oLanguage.getText("Warranty", "049", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "023", ctlLanguage.eumType.Tag)

        Me.UI_cmdProductGroupPick.Text = _oLanguage.getText("Warranty", "040", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Warranty", "039", ctlLanguage.eumType.Tag)
        'Me.btnInvalidAll.Text = _oLanguage.getText("Warranty", "053", ctlLanguage.eumType.Tag)
        'Me.btnValidClient.Text = _oLanguage.getText("Warranty", "052", ctlLanguage.eumType.Tag)

        Me.btnSaveAll.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.btnSubmitAll.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)

        pnlFile.Visible = False
        pnParts.Visible = False
        UI_txtVersion.Enabled = False
    End Sub

    Private Sub QueryData()
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Exit Sub
        End If

        Dim oWarranty As New ctlWarranty
        Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
        Dim sWar_id As String = Me.lblSwID.Text.ToString().Trim()
        UI_cmdSave.Visible = False

        UI_cmdProductGroupPick.Visible = False
        lblProductGroup.Text = ""

        dtWarrSet = oWarranty.QueryWarrSet(sWar_id, "", "", "", "", "")
        Dim i As Integer = 0
        If dtWarrSet.Count > 0 Then
            Dim dr As WarrantyDTO.WARRSETRow = dtWarrSet.Rows(0)

            btnBackTop.Visible = False
            UI_cboOperationCenter.SelectedValue = dr.WAR_COMPNO.ToString().Trim()
            UI_cboWarrantyType.SelectedValue = dr.WAR_TYPE.ToString().Trim()

            UI_cboOperationCenter.Enabled = False
            UI_cboWarrantyType.Enabled = False
            UI_txtProductGroup.Enabled = False
            UI_txtVersion.Enabled = False

            Me.UI_cboItemType.Enabled = False
            Me.UI_cboProgramType.Enabled = False
            Me.UI_cboPriceVer.Enabled = False

            GetWarrsetData(UI_cboWarrantyType.SelectedValue)


            UI_ExtraMonths.Text = dr.WAR_EXTMM.ToString().Trim()
            UI_txtProductGroup.Text = dr.WAR_GROUP.ToString().Trim()

            Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
            dtProductGroup = oWarranty.QueryPrdGroup(UI_txtProductGroup.Text, "")
            If dtProductGroup.Rows.Count > 0 Then
                lblProductGroup.Text = dtProductGroup.Rows(0)("GroupName").ToString().Trim()
            End If


            UI_txtWarrantyName.Text = dr.WAR_NAME.ToString().Trim()
            UI_Years.Text = dr.WAR_STDYY.ToString().Trim()
            UI_YearsOld.Text = dr.WAR_STDYY.ToString().Trim()
            If Not dr.IsWAR_ExpdateNull Then
                txtExpDate.Text = dr.WAR_Expdate.ToString("yyyy/MM/dd")
            End If

            UI_txtVersion.Text = dr.WAR_VERSION.ToString("000").Trim()
            UI_txtWar_longyy.Text = dr.WAR_LONGYY.ToString().Trim()
            UI_txtWar_longyyOld.Text = dr.WAR_LONGYY.ToString().Trim()

            UI_Years.Enabled = False
            'UI_txtWar_longyy.Enabled = False

            UI_txtDiscount.Text = dr.WAR_DISCOUNT.ToString().Trim()
            If Not dr.IsWAR_DESCNull Then UI_txtDescription.Text = dr.WAR_DESC.ToString().Trim()
            If Not dr.IsWAR_SPEC_DESCNull Then UI_txtWarrSpecDesc.Text = dr.WAR_SPEC_DESC.ToString().Trim()
            If Not dr.IsWAR_CARD_CONTENTNull Then UI_txtWarCardContent.Text = dr.WAR_CARD_CONTENT.ToString().Trim() ' 加入Warranty Card Content欄位 by buck add 20260128

            btnSaveAll.Visible = False
            btnPartsAdd.Visible = False
            btnSpecsAdd.Visible = False
            btnSubmitAll.Visible = False
            btnInValid.Visible = False
            btnValid.Visible = False
            btnInvalidAll.Visible = False
            btnValidClient.Visible = False
            chkOKAll.Enabled = False
            chkCopyChina.Visible = False
            lblSwIDStatus.Text = dr.WAR_STATUS

            If dr.WAR_STATUS = 0 Then
                btnSaveAll.Visible = True
                btnPartsAdd.Visible = True
                btnSpecsAdd.Visible = True
                btnSubmitAll.Visible = True
                btnInValid.Visible = True
                btnInvalidAll.Visible = True
                btnValidClient.Visible = True
                chkOKAll.Enabled = True
                chkCopyChina.Visible = True
            Else
                Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
                Dim x As Integer
                For x = 0 To lstParts.Items.Count - 1
                    If lstParts.Items(x).ItemType = ListItemType.Item Or lstParts.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstPartlblSeq As Label = lstParts.Items(x).FindControl("lstPartlblSeq")
                        Dim lstPartlblPart As Label = lstParts.Items(x).FindControl("lstPartlblPart")
                        Dim lstParttxtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtMon")
                        Dim lstParttxtExtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtExtMon")
                        Dim lstParttxtMemo As TextBox = lstParts.Items(x).FindControl("lstParttxtMemo")

                        lstParttxtMon.Enabled = False
                        lstParttxtExtMon.Enabled = False
                        lstParttxtMemo.Enabled = False
                    End If
                Next

                For x = 0 To lstSpecs.Items.Count - 1
                    If lstSpecs.Items(x).ItemType = ListItemType.Item Or lstSpecs.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstSpectxtRule As TextBox = lstSpecs.Items(x).FindControl("lstSpectxtRule")
                        lstSpectxtRule.Enabled = False
                    End If
                Next
            End If

            If dr.WAR_STATUS = 1 Then
            End If

            If dr.WAR_STATUS = 2 Then
                btnValid.Visible = True
            End If

            If dr.WAR_STATUS = 3 Then
                'btnValid.Visible = True
            End If

            If dr.WAR_ISALL = 0 Then
                chkOKAll.Checked = False
                btnValidClient.Visible = True
            Else
                chkOKAll.Checked = True
                btnValidClient.Visible = False
            End If
            GetYeas()
            pnlFile.Visible = True
            pnParts.Visible = True

            'If dr.IsWAR_REPAIR_NONull() Then
            '    'Me.UI_lblRepairCenterValue.Text = ""
            '    Me.UI_cboPriceType.SelectedValue = ""
            '    'Me.UI_lblRepairCenterText.Text = ""
            'Else
            '    'Me.UI_lblRepairCenterValue.Text = dr.WAR_REPAIR_NO.ToString().Trim()
            '    Me.UI_cboPriceType.SelectedValue = dr.WAR_REPAIR_NO.ToString().Trim()
            '    'Me.UI_lblRepairCenterText.Text = Me.UI_cboPriceType.Items(Me.UI_cboPriceType.SelectedIndex).Text.Trim()
            'End If


            'If dr.IsWAR_BATTERY_SERIVCENull() Then
            '    Me.UI_cboProjectType.Text = ""
            'Else
            '    Me.UI_cboProjectType.SelectedValue = dr.WAR_BATTERY_SERIVCE.ToString().Trim()
            'End If

            Me.UI_cboItemType.SelectedValue = dr.WAR_ITEM_TYPE.ToString().Trim()
            Me.UI_cboProgramType.SelectedValue = dr.WAR_PROGRAM_TYPE.ToString().Trim()
            Me.UI_cboPriceVer.SelectedValue = dr.WAR_PRICE_VER.ToString().Trim()

            lbl_part_no.Text = UI_txtProductGroup.Text + UI_cboWarrantyType.Text + UI_cboProgramType.Text + UI_cboItemType.Text + UI_cboPriceVer.Text + UI_txtVersion.Text
        End If

        Dim dtWPrice As New WarrantyDTO.WPRICEDataTable
        dtWPrice = oWarranty.QueryWPrice(sWar_id, "", "")
        For i = 0 To dtWPrice.Rows.Count - 1
            Dim dr As WarrantyDTO.WPRICERow = dtWPrice.Rows(i)
            If dr.WP_STYY = 1 Then
                txtPrice10.Text = dr.WP_YY01.ToString()
                txtPrice11.Text = dr.WP_YY02.ToString()
                txtPrice12.Text = dr.WP_YY03.ToString()
                txtPrice13.Text = dr.WP_YY04.ToString()
                txtPrice14.Text = dr.WP_YY05.ToString()
                txtPrice15.Text = dr.WP_YY06.ToString()
                txtPrice16.Text = dr.WP_YY07.ToString()
                txtPrice17.Text = dr.WP_YY08.ToString()
                txtPrice18.Text = dr.WP_YY09.ToString()
                txtPrice19.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 2 Then
                txtPrice20.Text = dr.WP_YY01.ToString()
                txtPrice21.Text = dr.WP_YY02.ToString()
                txtPrice22.Text = dr.WP_YY03.ToString()
                txtPrice23.Text = dr.WP_YY04.ToString()
                txtPrice24.Text = dr.WP_YY05.ToString()
                txtPrice25.Text = dr.WP_YY06.ToString()
                txtPrice26.Text = dr.WP_YY07.ToString()
                txtPrice27.Text = dr.WP_YY08.ToString()
                txtPrice28.Text = dr.WP_YY09.ToString()
                txtPrice29.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 3 Then
                txtPrice30.Text = dr.WP_YY01.ToString()
                txtPrice31.Text = dr.WP_YY02.ToString()
                txtPrice32.Text = dr.WP_YY03.ToString()
                txtPrice33.Text = dr.WP_YY04.ToString()
                txtPrice34.Text = dr.WP_YY05.ToString()
                txtPrice35.Text = dr.WP_YY06.ToString()
                txtPrice36.Text = dr.WP_YY07.ToString()
                txtPrice37.Text = dr.WP_YY08.ToString()
                txtPrice38.Text = dr.WP_YY09.ToString()
                txtPrice39.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 4 Then
                txtPrice40.Text = dr.WP_YY01.ToString()
                txtPrice41.Text = dr.WP_YY02.ToString()
                txtPrice42.Text = dr.WP_YY03.ToString()
                txtPrice43.Text = dr.WP_YY04.ToString()
                txtPrice44.Text = dr.WP_YY05.ToString()
                txtPrice45.Text = dr.WP_YY06.ToString()
                txtPrice46.Text = dr.WP_YY07.ToString()
                txtPrice47.Text = dr.WP_YY08.ToString()
                txtPrice48.Text = dr.WP_YY09.ToString()
                txtPrice49.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 5 Then
                txtPrice50.Text = dr.WP_YY01.ToString()
                txtPrice51.Text = dr.WP_YY02.ToString()
                txtPrice52.Text = dr.WP_YY03.ToString()
                txtPrice53.Text = dr.WP_YY04.ToString()
                txtPrice54.Text = dr.WP_YY05.ToString()
                txtPrice55.Text = dr.WP_YY06.ToString()
                txtPrice56.Text = dr.WP_YY07.ToString()
                txtPrice57.Text = dr.WP_YY08.ToString()
                txtPrice58.Text = dr.WP_YY09.ToString()
                txtPrice59.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 6 Then
                txtPrice60.Text = dr.WP_YY01.ToString()
                txtPrice61.Text = dr.WP_YY02.ToString()
                txtPrice62.Text = dr.WP_YY03.ToString()
                txtPrice63.Text = dr.WP_YY04.ToString()
                txtPrice64.Text = dr.WP_YY05.ToString()
                txtPrice65.Text = dr.WP_YY06.ToString()
                txtPrice66.Text = dr.WP_YY07.ToString()
                txtPrice67.Text = dr.WP_YY08.ToString()
                txtPrice68.Text = dr.WP_YY09.ToString()
                txtPrice69.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 7 Then
                txtPrice70.Text = dr.WP_YY01.ToString()
                txtPrice71.Text = dr.WP_YY02.ToString()
                txtPrice72.Text = dr.WP_YY03.ToString()
                txtPrice73.Text = dr.WP_YY04.ToString()
                txtPrice74.Text = dr.WP_YY05.ToString()
                txtPrice75.Text = dr.WP_YY06.ToString()
                txtPrice76.Text = dr.WP_YY07.ToString()
                txtPrice77.Text = dr.WP_YY08.ToString()
                txtPrice78.Text = dr.WP_YY09.ToString()
                txtPrice79.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 8 Then
                txtPrice80.Text = dr.WP_YY01.ToString()
                txtPrice81.Text = dr.WP_YY02.ToString()
                txtPrice82.Text = dr.WP_YY03.ToString()
                txtPrice83.Text = dr.WP_YY04.ToString()
                txtPrice84.Text = dr.WP_YY05.ToString()
                txtPrice85.Text = dr.WP_YY06.ToString()
                txtPrice86.Text = dr.WP_YY07.ToString()
                txtPrice87.Text = dr.WP_YY08.ToString()
                txtPrice88.Text = dr.WP_YY09.ToString()
                txtPrice89.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 9 Then
                txtPrice90.Text = dr.WP_YY01.ToString()
                txtPrice91.Text = dr.WP_YY02.ToString()
                txtPrice92.Text = dr.WP_YY03.ToString()
                txtPrice93.Text = dr.WP_YY04.ToString()
                txtPrice94.Text = dr.WP_YY05.ToString()
                txtPrice95.Text = dr.WP_YY06.ToString()
                txtPrice96.Text = dr.WP_YY07.ToString()
                txtPrice97.Text = dr.WP_YY08.ToString()
                txtPrice98.Text = dr.WP_YY09.ToString()
                txtPrice99.Text = dr.WP_YY10.ToString()
            End If
            If dr.WP_STYY = 10 Then
                txtPrice100.Text = dr.WP_YY01.ToString()
                txtPrice101.Text = dr.WP_YY02.ToString()
                txtPrice102.Text = dr.WP_YY03.ToString()
                txtPrice103.Text = dr.WP_YY04.ToString()
                txtPrice104.Text = dr.WP_YY05.ToString()
                txtPrice105.Text = dr.WP_YY06.ToString()
                txtPrice106.Text = dr.WP_YY07.ToString()
                txtPrice107.Text = dr.WP_YY08.ToString()
                txtPrice108.Text = dr.WP_YY09.ToString()
                txtPrice109.Text = dr.WP_YY10.ToString()
            End If

            If txtPrice10.Text = "0" Then txtPrice10.Text = ""
            If txtPrice11.Text = "0" Then txtPrice11.Text = ""
            If txtPrice12.Text = "0" Then txtPrice12.Text = ""
            If txtPrice13.Text = "0" Then txtPrice13.Text = ""
            If txtPrice14.Text = "0" Then txtPrice14.Text = ""
            If txtPrice15.Text = "0" Then txtPrice15.Text = ""
            If txtPrice16.Text = "0" Then txtPrice16.Text = ""
            If txtPrice17.Text = "0" Then txtPrice17.Text = ""
            If txtPrice18.Text = "0" Then txtPrice18.Text = ""
            If txtPrice19.Text = "0" Then txtPrice19.Text = ""

            If txtPrice20.Text = "0" Then txtPrice20.Text = ""
            If txtPrice21.Text = "0" Then txtPrice21.Text = ""
            If txtPrice22.Text = "0" Then txtPrice22.Text = ""
            If txtPrice23.Text = "0" Then txtPrice23.Text = ""
            If txtPrice24.Text = "0" Then txtPrice24.Text = ""
            If txtPrice25.Text = "0" Then txtPrice25.Text = ""
            If txtPrice26.Text = "0" Then txtPrice26.Text = ""
            If txtPrice27.Text = "0" Then txtPrice27.Text = ""
            If txtPrice28.Text = "0" Then txtPrice28.Text = ""
            If txtPrice29.Text = "0" Then txtPrice29.Text = ""

            If txtPrice30.Text = "0" Then txtPrice30.Text = ""
            If txtPrice31.Text = "0" Then txtPrice31.Text = ""
            If txtPrice32.Text = "0" Then txtPrice32.Text = ""
            If txtPrice33.Text = "0" Then txtPrice33.Text = ""
            If txtPrice34.Text = "0" Then txtPrice34.Text = ""
            If txtPrice35.Text = "0" Then txtPrice35.Text = ""
            If txtPrice36.Text = "0" Then txtPrice36.Text = ""
            If txtPrice37.Text = "0" Then txtPrice37.Text = ""
            If txtPrice38.Text = "0" Then txtPrice38.Text = ""
            If txtPrice39.Text = "0" Then txtPrice39.Text = ""

            If txtPrice40.Text = "0" Then txtPrice40.Text = ""
            If txtPrice41.Text = "0" Then txtPrice41.Text = ""
            If txtPrice42.Text = "0" Then txtPrice42.Text = ""
            If txtPrice43.Text = "0" Then txtPrice43.Text = ""
            If txtPrice44.Text = "0" Then txtPrice44.Text = ""
            If txtPrice45.Text = "0" Then txtPrice45.Text = ""
            If txtPrice46.Text = "0" Then txtPrice46.Text = ""
            If txtPrice47.Text = "0" Then txtPrice47.Text = ""
            If txtPrice48.Text = "0" Then txtPrice48.Text = ""
            If txtPrice49.Text = "0" Then txtPrice49.Text = ""

            If txtPrice50.Text = "0" Then txtPrice50.Text = ""
            If txtPrice51.Text = "0" Then txtPrice51.Text = ""
            If txtPrice52.Text = "0" Then txtPrice52.Text = ""
            If txtPrice53.Text = "0" Then txtPrice53.Text = ""
            If txtPrice54.Text = "0" Then txtPrice54.Text = ""
            If txtPrice55.Text = "0" Then txtPrice55.Text = ""
            If txtPrice56.Text = "0" Then txtPrice56.Text = ""
            If txtPrice57.Text = "0" Then txtPrice57.Text = ""
            If txtPrice58.Text = "0" Then txtPrice58.Text = ""
            If txtPrice59.Text = "0" Then txtPrice59.Text = ""

            If txtPrice60.Text = "0" Then txtPrice60.Text = ""
            If txtPrice61.Text = "0" Then txtPrice61.Text = ""
            If txtPrice62.Text = "0" Then txtPrice62.Text = ""
            If txtPrice63.Text = "0" Then txtPrice63.Text = ""
            If txtPrice64.Text = "0" Then txtPrice64.Text = ""
            If txtPrice65.Text = "0" Then txtPrice65.Text = ""
            If txtPrice66.Text = "0" Then txtPrice66.Text = ""
            If txtPrice67.Text = "0" Then txtPrice67.Text = ""
            If txtPrice68.Text = "0" Then txtPrice68.Text = ""
            If txtPrice69.Text = "0" Then txtPrice69.Text = ""

            If txtPrice70.Text = "0" Then txtPrice70.Text = ""
            If txtPrice71.Text = "0" Then txtPrice71.Text = ""
            If txtPrice72.Text = "0" Then txtPrice72.Text = ""
            If txtPrice73.Text = "0" Then txtPrice73.Text = ""
            If txtPrice74.Text = "0" Then txtPrice74.Text = ""
            If txtPrice75.Text = "0" Then txtPrice75.Text = ""
            If txtPrice76.Text = "0" Then txtPrice76.Text = ""
            If txtPrice77.Text = "0" Then txtPrice77.Text = ""
            If txtPrice78.Text = "0" Then txtPrice78.Text = ""
            If txtPrice79.Text = "0" Then txtPrice79.Text = ""

            If txtPrice80.Text = "0" Then txtPrice80.Text = ""
            If txtPrice81.Text = "0" Then txtPrice81.Text = ""
            If txtPrice82.Text = "0" Then txtPrice82.Text = ""
            If txtPrice83.Text = "0" Then txtPrice83.Text = ""
            If txtPrice84.Text = "0" Then txtPrice84.Text = ""
            If txtPrice85.Text = "0" Then txtPrice85.Text = ""
            If txtPrice86.Text = "0" Then txtPrice86.Text = ""
            If txtPrice87.Text = "0" Then txtPrice87.Text = ""
            If txtPrice88.Text = "0" Then txtPrice88.Text = ""
            If txtPrice89.Text = "0" Then txtPrice89.Text = ""

            If txtPrice90.Text = "0" Then txtPrice90.Text = ""
            If txtPrice91.Text = "0" Then txtPrice91.Text = ""
            If txtPrice92.Text = "0" Then txtPrice92.Text = ""
            If txtPrice93.Text = "0" Then txtPrice93.Text = ""
            If txtPrice94.Text = "0" Then txtPrice94.Text = ""
            If txtPrice95.Text = "0" Then txtPrice95.Text = ""
            If txtPrice96.Text = "0" Then txtPrice96.Text = ""
            If txtPrice97.Text = "0" Then txtPrice97.Text = ""
            If txtPrice98.Text = "0" Then txtPrice98.Text = ""
            If txtPrice99.Text = "0" Then txtPrice99.Text = ""


            If txtPrice100.Text = "0" Then txtPrice100.Text = ""
            If txtPrice101.Text = "0" Then txtPrice101.Text = ""
            If txtPrice102.Text = "0" Then txtPrice102.Text = ""
            If txtPrice103.Text = "0" Then txtPrice103.Text = ""
            If txtPrice104.Text = "0" Then txtPrice104.Text = ""
            If txtPrice105.Text = "0" Then txtPrice105.Text = ""
            If txtPrice106.Text = "0" Then txtPrice106.Text = ""
            If txtPrice107.Text = "0" Then txtPrice107.Text = ""
            If txtPrice108.Text = "0" Then txtPrice108.Text = ""
            If txtPrice109.Text = "0" Then txtPrice109.Text = ""

        Next
        SetToolTip()



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
        Dim sWar_id As String = lblSwID.Text.ToString().Trim()

        Dim oWarranty As New ctlWarranty
        Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
        Try
            Dim dr As WarrantyDTO.WARRSETRow = dtWarrSet.NewWARRSETRow

            dr.WAR_ID = sWar_id.ToString().Trim()
            dr.WAR_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
            dr.WAR_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
            dr.WAR_GROUP = UI_txtProductGroup.Text.ToString().Trim()
            dr.WAR_TYPE = Me.UI_cboWarrantyType.SelectedValue.ToString().Trim()
            dr.WAR_DISCOUNT = Convert.ToInt32(Me.UI_txtDiscount.Text.ToString().Trim())
            dr.WAR_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
            dr.WAR_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
            dr.WAR_LONGYY = Convert.ToInt32(Me.UI_txtWar_longyy.Text.ToString().Trim())
            dr.WAR_DESC = Me.UI_txtDescription.Text.ToString().Trim()
            dr.WAR_SPEC_DESC = Me.UI_txtWarrSpecDesc.Text.ToString().Trim()
            If txtExpDate.Text.Trim() <> "" Then
                dr.WAR_Expdate = txtExpDate.Text
            End If
            dr.WAR_ISALL = 1
            dr.WAR_STATUS = 0

            dr.WAR_AD = Session("_UserID")
            dr.WAR_ADNAME = Session("_UserName")
            dr.WAR_CSTMP = Date.Now
            dr.WAR_LUAD = Session("_UserID")
            dr.WAR_LUADNAME = Session("_UserName")
            dr.WAR_LUSTMP = Date.Now
            dr.WAR_MARK = 0
            dr.WAR_COPYCN = 0

            'dr.WAR_REPAIR_NO = Me.UI_cboPriceType.SelectedValue.Trim()
            'If oCommon.CheckDBNull(dr.WAR_REPAIR_NO).ToString().Trim() = "-1" Then
            '    dr.WAR_REPAIR_NO = ""
            'Else
            '    dr.WAR_REPAIR_NO = Me.UI_cboPriceType.SelectedValue
            'End If

            'dr.WAR_BATTERY_SERIVCE = Me.UI_cboProjectType.SelectedValue.Trim()

            dr.WAR_ITEM_TYPE = Me.UI_cboItemType.SelectedValue.Trim()
            dr.WAR_PROGRAM_TYPE = Me.UI_cboProgramType.SelectedValue.Trim()
            dr.WAR_PRICE_VER = Me.UI_cboPriceVer.SelectedValue.Trim()

            dtWarrSet.AddWARRSETRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    sWar_id = oWarranty.SaveAddWarrSet(dtWarrSet)
                    lblSwID.Text = sWar_id
                    UI_cmdSave.Visible = False
                    UI_cmdProductGroupPick.Visible = False

                    chkOKAll.Checked = True
                    chkCopyChina.Checked = True

                    btnValidClient.Visible = False

                    GetYeas()
                    btnInValid.Visible = False

                    dtWarrSet = oWarranty.QueryWarrSet(sWar_id, "", "", "", "", "", "")
                    If dtWarrSet.Count > 0 Then
                        Dim dr1 As WarrantyDTO.WARRSETRow = dtWarrSet.Rows(0)
                        UI_txtVersion.Text = dr1.WAR_VERSION.ToString("000").Trim()
                    End If
                    SetToolTip()
                Case eumCommand.UPDATE
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

                pnlFile.Visible = True
                pnParts.Visible = True

            End If
        End Try
    End Sub

    Protected Sub UI_cmdProductGroupPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdProductGroupPick.Click
        Dim dtProductGroup As DataTable = Me.ViewState("_dtProductGroup")
        Me.ucProductGroup.show(dtProductGroup, True)
    End Sub

    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitAll.Click
        Dim sMessage As String = ""
        Dim tWAR_COMPNO As String = "" 'MODY BY Angel On 20160613 上海flow流程
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable

                '20170327 Isaac 增加檢核，必須有客戶(S)
                Dim dtWCLIENT As New WarrantyDTO.WCLIENTDataTable
                dtWCLIENT = oWarranty.QueryWClient(sWar_id, "", "")
                Dim x As Integer
                Dim sCustomer As String = ""
                For x = 0 To dtWCLIENT.Rows.Count - 1
                    sCustomer = sCustomer + dtWCLIENT.Rows(x)("WC_CLNO").ToString().Trim() + ","
                Next

                If sCustomer.Length > 0 Then
                    If sCustomer.Substring(sCustomer.Length - 1) = "," Then
                        sCustomer = sCustomer.Substring(0, sCustomer.Length - 1)
                    End If
                End If

                If chkOKAll.Checked = False And sCustomer = "" Then
                    Throw New Exception("Please Check Cusomer!")
                End If
                '20170327 Isaac 增加檢核，必須有客戶(E)

                dtWarrSet = oWarranty.QueryWarrSet(sWar_id, "", "", "", "", "", "")
                If dtWarrSet.Rows.Count > 0 Then
                    Dim dr As WarrantyDTO.WARRSETRow = dtWarrSet.Rows(0)
                    dr.WAR_ID = sWar_id
                    dr.WAR_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
                    dr.WAR_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                    tWAR_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                    dr.WAR_GROUP = UI_txtProductGroup.Text.ToString().Trim()
                    dr.WAR_TYPE = Me.UI_cboWarrantyType.SelectedValue.ToString().Trim()
                    dr.WAR_DISCOUNT = Convert.ToInt32(Me.UI_txtDiscount.Text.ToString().Trim())
                    dr.WAR_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
                    dr.WAR_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                    dr.WAR_LONGYY = Convert.ToInt32(Me.UI_txtWar_longyy.Text.ToString().Trim())
                    dr.WAR_DESC = Me.UI_txtDescription.Text.ToString().Trim()
                    dr.WAR_SPEC_DESC = Me.UI_txtWarrSpecDesc.Text.ToString().Trim()
                    dr.WAR_CARD_CONTENT = Me.UI_txtWarCardContent.Text.ToString().Trim() ' 加入Warranty Card Content欄位 by buck add 20260128
                    If txtExpDate.Text.Trim() <> "" Then
                        dr.WAR_Expdate = txtExpDate.Text
                    End If
                    If chkOKAll.Checked Then
                        dr.WAR_ISALL = 1
                    Else
                        dr.WAR_ISALL = 0
                    End If

                    dr.WAR_STATUS = 1
                    dr.WAR_LUAD = Session("_UserID")
                    dr.WAR_LUADNAME = Session("_UserName")
                    dr.WAR_LUSTMP = Date.Now
                    If chkCopyChina.Checked Then
                        dr.WAR_COPYCN = 1
                    Else
                        dr.WAR_COPYCN = 0
                    End If

                    ''dr.WAR_REPAIR_NO = Me.UI_cboPriceType.SelectedValue.Trim()
                    'If Me.UI_cboPriceType.SelectedValue.Trim() = "-1" Then
                    '    dr.WAR_REPAIR_NO = ""
                    'Else
                    '    dr.WAR_REPAIR_NO = Me.UI_cboPriceType.SelectedValue.Trim()
                    'End If

                    'dr.WAR_BATTERY_SERIVCE = Me.UI_cboProjectType.SelectedValue.Trim()

                    dr.WAR_ITEM_TYPE = Me.UI_cboItemType.SelectedValue.Trim()
                    dr.WAR_PROGRAM_TYPE = Me.UI_cboProgramType.SelectedValue.Trim()
                    dr.WAR_PRICE_VER = Me.UI_cboPriceVer.SelectedValue.Trim()

                End If

                Dim dtWPrice As New WarrantyDTO.WPRICEDataTable
                Dim drp As WarrantyDTO.WPRICERow = dtWPrice.NewWPRICERow

                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 1
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0

                If Not txtPrice10.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice10.Text.Trim())
                End If
                If Not txtPrice11.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice11.Text.Trim())
                End If
                If Not txtPrice12.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice12.Text.Trim())
                End If
                If Not txtPrice13.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice13.Text.Trim())
                End If
                If Not txtPrice14.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice14.Text.Trim())
                End If
                If Not txtPrice15.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice15.Text.Trim())
                End If
                If Not txtPrice16.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice16.Text.Trim())
                End If
                If Not txtPrice17.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice17.Text.Trim())
                End If
                If Not txtPrice18.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice18.Text.Trim())
                End If
                If Not txtPrice19.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice19.Text.Trim())
                End If

                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 2
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice20.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice20.Text.Trim())
                End If
                If Not txtPrice21.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice21.Text.Trim())
                End If
                If Not txtPrice22.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice22.Text.Trim())
                End If
                If Not txtPrice23.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice23.Text.Trim())
                End If
                If Not txtPrice24.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice24.Text.Trim())
                End If
                If Not txtPrice25.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice25.Text.Trim())
                End If
                If Not txtPrice26.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice26.Text.Trim())
                End If
                If Not txtPrice27.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice27.Text.Trim())
                End If
                If Not txtPrice28.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice28.Text.Trim())
                End If
                If Not txtPrice29.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice29.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 3
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice30.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice30.Text.Trim())
                End If
                If Not txtPrice31.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice31.Text.Trim())
                End If
                If Not txtPrice32.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice32.Text.Trim())
                End If
                If Not txtPrice33.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice33.Text.Trim())
                End If
                If Not txtPrice34.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice34.Text.Trim())
                End If
                If Not txtPrice35.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice35.Text.Trim())
                End If
                If Not txtPrice36.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice36.Text.Trim())
                End If
                If Not txtPrice37.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice37.Text.Trim())
                End If
                If Not txtPrice38.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice38.Text.Trim())
                End If
                If Not txtPrice39.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice39.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 4
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice40.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice40.Text.Trim())
                End If
                If Not txtPrice41.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice41.Text.Trim())
                End If
                If Not txtPrice42.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice42.Text.Trim())
                End If
                If Not txtPrice43.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice43.Text.Trim())
                End If
                If Not txtPrice44.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice44.Text.Trim())
                End If
                If Not txtPrice45.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice45.Text.Trim())
                End If
                If Not txtPrice46.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice46.Text.Trim())
                End If
                If Not txtPrice47.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice47.Text.Trim())
                End If
                If Not txtPrice48.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice48.Text.Trim())
                End If
                If Not txtPrice49.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice49.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 5
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice50.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice50.Text.Trim())
                End If
                If Not txtPrice51.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice51.Text.Trim())
                End If
                If Not txtPrice52.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice52.Text.Trim())
                End If
                If Not txtPrice53.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice53.Text.Trim())
                End If
                If Not txtPrice54.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice54.Text.Trim())
                End If
                If Not txtPrice55.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice55.Text.Trim())
                End If
                If Not txtPrice56.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice56.Text.Trim())
                End If
                If Not txtPrice57.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice57.Text.Trim())
                End If
                If Not txtPrice58.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice58.Text.Trim())
                End If
                If Not txtPrice59.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice59.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)


                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 6
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice60.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice60.Text.Trim())
                End If
                If Not txtPrice61.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice61.Text.Trim())
                End If
                If Not txtPrice62.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice62.Text.Trim())
                End If
                If Not txtPrice63.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice63.Text.Trim())
                End If
                If Not txtPrice64.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice64.Text.Trim())
                End If
                If Not txtPrice65.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice65.Text.Trim())
                End If
                If Not txtPrice66.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice66.Text.Trim())
                End If
                If Not txtPrice67.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice67.Text.Trim())
                End If
                If Not txtPrice68.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice68.Text.Trim())
                End If
                If Not txtPrice69.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice69.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 7
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice70.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice70.Text.Trim())
                End If
                If Not txtPrice71.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice71.Text.Trim())
                End If
                If Not txtPrice72.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice72.Text.Trim())
                End If
                If Not txtPrice73.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice73.Text.Trim())
                End If
                If Not txtPrice74.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice74.Text.Trim())
                End If
                If Not txtPrice75.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice75.Text.Trim())
                End If
                If Not txtPrice76.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice76.Text.Trim())
                End If
                If Not txtPrice77.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice77.Text.Trim())
                End If
                If Not txtPrice78.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice78.Text.Trim())
                End If
                If Not txtPrice79.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice79.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 8
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice80.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice80.Text.Trim())
                End If
                If Not txtPrice81.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice81.Text.Trim())
                End If
                If Not txtPrice82.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice82.Text.Trim())
                End If
                If Not txtPrice83.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice83.Text.Trim())
                End If
                If Not txtPrice84.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice84.Text.Trim())
                End If
                If Not txtPrice85.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice85.Text.Trim())
                End If
                If Not txtPrice86.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice86.Text.Trim())
                End If
                If Not txtPrice87.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice87.Text.Trim())
                End If
                If Not txtPrice88.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice88.Text.Trim())
                End If
                If Not txtPrice89.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice89.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 9
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice90.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice90.Text.Trim())
                End If
                If Not txtPrice91.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice91.Text.Trim())
                End If
                If Not txtPrice92.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice92.Text.Trim())
                End If
                If Not txtPrice93.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice93.Text.Trim())
                End If
                If Not txtPrice94.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice94.Text.Trim())
                End If
                If Not txtPrice95.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice95.Text.Trim())
                End If
                If Not txtPrice96.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice96.Text.Trim())
                End If
                If Not txtPrice97.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice97.Text.Trim())
                End If
                If Not txtPrice98.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice98.Text.Trim())
                End If
                If Not txtPrice99.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice99.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 10
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice100.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice100.Text.Trim())
                End If
                If Not txtPrice101.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice101.Text.Trim())
                End If
                If Not txtPrice102.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice102.Text.Trim())
                End If
                If Not txtPrice103.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice103.Text.Trim())
                End If
                If Not txtPrice104.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice104.Text.Trim())
                End If
                If Not txtPrice105.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice105.Text.Trim())
                End If
                If Not txtPrice106.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice106.Text.Trim())
                End If
                If Not txtPrice107.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice107.Text.Trim())
                End If
                If Not txtPrice108.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice108.Text.Trim())
                End If
                If Not txtPrice109.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice109.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                'Save WarrParts Data
                Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
                For x = 0 To lstParts.Items.Count - 1
                    If lstParts.Items(x).ItemType = ListItemType.Item Or lstParts.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstPartlblSeq As Label = lstParts.Items(x).FindControl("lstPartlblSeq")
                        Dim lstPartlblPart As Label = lstParts.Items(x).FindControl("lstPartlblPart")
                        Dim lstParttxtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtMon")
                        Dim lstParttxtExtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtExtMon")
                        Dim lstParttxtMemo As TextBox = lstParts.Items(x).FindControl("lstParttxtMemo")

                        If Convert.ToInt16(lstParttxtMon.Text) = 0 Then
                            Throw New Exception("Please input Warranty Month!")
                        End If

                        Dim drWp As WarrantyDTO.WarrPartsRow = dtWarrParts.NewWarrPartsRow
                        drWp.WAP_WID = sWar_id
                        drWp.WAP_SEQ = Convert.ToInt16(lstPartlblSeq.Text)
                        drWp.WAP_NAME = lstPartlblPart.Text
                        drWp.WAP_MON = Convert.ToInt16(lstParttxtMon.Text)
                        drWp.WAP_EMON = Convert.ToInt16(lstParttxtExtMon.Text)
                        drWp.WAP_DESC = lstParttxtMemo.Text
                        drWp.WAP_AD = Session("_UserID")
                        drWp.WAP_ADNAME = Session("_UserName")
                        drWp.WAP_CSTMP = Date.Now
                        drWp.WAP_LUAD = Session("_UserID")
                        drWp.WAP_LUADNAME = Session("_UserName")
                        drWp.WAP_LUSTMP = Date.Now
                        dtWarrParts.AddWarrPartsRow(drWp)
                    End If
                Next

                'Save WarrSpecs Data
                Dim dtWarrSpecs As New DataTable
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_WID", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_SEQ", GetType(Integer)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_NAME", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_Rule", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_AD", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_ADNAME", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_CSTMP", GetType(String)))

                For x = 0 To lstSpecs.Items.Count - 1
                    If lstSpecs.Items(x).ItemType = ListItemType.Item Or lstSpecs.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstSpeclblSeq As Label = lstSpecs.Items(x).FindControl("lstSpeclblSeq")
                        Dim lstSpeclblPart As Label = lstSpecs.Items(x).FindControl("lstSpeclblPart")
                        Dim lstSpectxtRule As TextBox = lstSpecs.Items(x).FindControl("lstSpectxtRule")

                        If lstSpectxtRule.Text = "" Then
                            Throw New Exception("Please input Warranty Spec Rule!")
                        End If

                        Dim drWp As DataRow = dtWarrSpecs.NewRow
                        drWp("WAP_WID") = sWar_id
                        drWp("WAP_SEQ") = Convert.ToInt16(lstSpeclblSeq.Text)
                        drWp("WAP_NAME") = lstSpeclblPart.Text
                        drWp("WAP_Rule") = Convert.ToInt16(lstSpectxtRule.Text)
                        drWp("WAP_AD") = Session("_UserID")
                        drWp("WAP_ADNAME") = Session("_UserName")
                        drWp("WAP_CSTMP") = Date.Now
                        dtWarrSpecs.Rows.Add(drWp)

                    End If
                Next

                oWarranty.SaveEditWarrSet(dtWarrSet)
                oWarranty.SaveAddWPrice(dtWPrice)
                oWarranty.SaveAddWarrParts(dtWarrParts)
                oWarranty.SaveAddWarrSpecs(dtWarrSpecs)
                oWarranty.SaveAddWarrSetPart(sWar_id)

                '2025/01/06 新增 電池保固 開始
                If UI_cboProgramType.SelectedItem.Value = "P" Or UI_cboProgramType.SelectedItem.Value = "S" Then
                    Dim WAP_WID As String = sWar_id
                    Dim WAP_SEQ As String = "0"
                    Dim WAP_NAME As String = "BI"

                    Dim WAP_RULE As String = ""

                    If UI_cboProgramType.SelectedItem.Value = "P" Then
                        WAP_RULE = "18"
                    Else
                        WAP_RULE = "36"
                    End If

                    Dim WAP_AD As String = Session("_UserID")
                    Dim WAP_ADNAME As String = Session("_UserName")
                    oWarranty.WARRSPECS_BI_Insert(WAP_WID, WAP_SEQ, WAP_NAME, WAP_RULE, WAP_AD, WAP_ADNAME)


                End If
                '2025/01/06 新增 電池保固 結束

                '140701 by cipherlab.fairy----(S)
                dtWarrSet = oWarranty.QueryWarrSet(sWar_id, "", "", "", "", "", "")
                Dim dr1 As WarrantyDTO.WARRSETRow = dtWarrSet.Rows(0)
                dr1.WAR_ID = sWar_id
                dr1.WAR_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
                dr1.WAR_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                dr1.WAR_GROUP = UI_txtProductGroup.Text.ToString().Trim()
                dr1.WAR_TYPE = Me.UI_cboWarrantyType.SelectedValue.ToString().Trim()
                dr1.WAR_DISCOUNT = Convert.ToInt32(Me.UI_txtDiscount.Text.ToString().Trim())
                dr1.WAR_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
                dr1.WAR_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                dr1.WAR_LONGYY = Convert.ToInt32(Me.UI_txtWar_longyy.Text.ToString().Trim())
                'Throw New Exception(dr1.WAR_VERSION)
                If txtExpDate.Text.Trim() <> "" Then
                    dr1.WAR_Expdate = txtExpDate.Text
                End If
                If chkOKAll.Checked Then
                    dr1.WAR_ISALL = 1
                Else
                    dr1.WAR_ISALL = 0
                End If
                dr1.WAR_LUAD = Session("_UserID")

                Dim FlowWS As New WorkflowService.WorkflowServiceService()
                Dim formOID As String = FlowWS.findFormOIDsOfProcess("WARRSetting")
                Dim formXML As New XmlDocument()
                formXML.LoadXml(FlowWS.getFormFieldTemplate(formOID))
                formXML.DocumentElement.SelectSingleNode("war_id").InnerText = sWar_id
                formXML.DocumentElement.SelectSingleNode("war_name").InnerText = dr1.WAR_NAME
                formXML.DocumentElement.SelectSingleNode("war_group").InnerText = dr1.WAR_GROUP
                formXML.DocumentElement.SelectSingleNode("war_type").InnerText = dr1.WAR_TYPE
                formXML.DocumentElement.SelectSingleNode("war_version").InnerText = dr1.WAR_VERSION
                formXML.DocumentElement.SelectSingleNode("war_discount").InnerText = dr1.WAR_DISCOUNT
                formXML.DocumentElement.SelectSingleNode("war_extmm").InnerText = dr1.WAR_EXTMM
                formXML.DocumentElement.SelectSingleNode("war_stdyy").InnerText = dr1.WAR_STDYY
                formXML.DocumentElement.SelectSingleNode("war_longyy").InnerText = dr1.WAR_LONGYY
                formXML.DocumentElement.SelectSingleNode("war_isall").InnerText = dr1.WAR_ISALL
                formXML.DocumentElement.SelectSingleNode("war_expdate").InnerText = dr1.WAR_Expdate
                formXML.DocumentElement.SelectSingleNode("war_compno").InnerText = dr1.WAR_COMPNO
                formXML.DocumentElement.SelectSingleNode("war_customer").InnerText = sCustomer '170327 add by Isaac
                formXML.DocumentElement.SelectSingleNode("war_copychina").InnerText = chkCopyChina.Checked.ToString() '170327 add by Isaac
                'Throw New Exception(FlowWS.getFormFieldTemplate(formOID))
                'Throw New Exception(dr1.WAR_COMPNO)
                '撈單身資料
                'Dim dtWPrice As New WarrantyDTO.WPRICEDataTable
                dtWPrice = oWarranty.QueryWPrice(sWar_id, dr1.WAR_COMPNO, "wp_styy")

                '設定datagrid
                Dim xnl As XmlNode
                xnl = formXML.DocumentElement.SelectSingleNode("ecws_grid/records")
                Dim xn As XmlNode
                xn = xnl.FirstChild.Clone()
                xnl.RemoveAll()
                xn.Attributes("id").InnerText = "ecws_grid_0"
                xn.SelectSingleNode("//item[@id='wp_styy']").InnerText = dtWPrice.Rows(0)("wp_styy").ToString
                xn.SelectSingleNode("//item[@id='wp_yy01']").InnerText = dtWPrice.Rows(0)("wp_yy01").ToString
                xn.SelectSingleNode("//item[@id='wp_yy02']").InnerText = dtWPrice.Rows(0)("wp_yy02").ToString
                xn.SelectSingleNode("//item[@id='wp_yy03']").InnerText = dtWPrice.Rows(0)("wp_yy03").ToString
                xn.SelectSingleNode("//item[@id='wp_yy04']").InnerText = dtWPrice.Rows(0)("wp_yy04").ToString
                xn.SelectSingleNode("//item[@id='wp_yy05']").InnerText = dtWPrice.Rows(0)("wp_yy05").ToString
                xn.SelectSingleNode("//item[@id='wp_yy06']").InnerText = dtWPrice.Rows(0)("wp_yy06").ToString
                xn.SelectSingleNode("//item[@id='wp_yy07']").InnerText = dtWPrice.Rows(0)("wp_yy07").ToString
                xn.SelectSingleNode("//item[@id='wp_yy08']").InnerText = dtWPrice.Rows(0)("wp_yy08").ToString
                xn.SelectSingleNode("//item[@id='wp_yy09']").InnerText = dtWPrice.Rows(0)("wp_yy09").ToString
                xn.SelectSingleNode("//item[@id='wp_yy10']").InnerText = dtWPrice.Rows(0)("wp_yy10").ToString
                xnl.AppendChild(xn)
                'Throw New Exception(dtWPrice.Rows.Count)
                Dim i As Integer = 0
                For i = 1 To dtWPrice.Rows.Count - 1 Step 1
                    xn = xnl.FirstChild.Clone()
                    xn.Attributes("id").InnerText = "ecws_grid" + i.ToString
                    xn.SelectSingleNode("//item[@id='wp_styy']").InnerText = dtWPrice.Rows(i)("wp_styy").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy01']").InnerText = dtWPrice.Rows(i)("wp_yy01").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy02']").InnerText = dtWPrice.Rows(i)("wp_yy02").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy03']").InnerText = dtWPrice.Rows(i)("wp_yy03").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy04']").InnerText = dtWPrice.Rows(i)("wp_yy04").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy05']").InnerText = dtWPrice.Rows(i)("wp_yy05").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy06']").InnerText = dtWPrice.Rows(i)("wp_yy06").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy07']").InnerText = dtWPrice.Rows(i)("wp_yy07").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy08']").InnerText = dtWPrice.Rows(i)("wp_yy08").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy09']").InnerText = dtWPrice.Rows(i)("wp_yy09").ToString
                    xn.SelectSingleNode("//item[@id='wp_yy10']").InnerText = dtWPrice.Rows(i)("wp_yy10").ToString
                    xnl.AppendChild(xn)
                Next
                'Throw New Exception(formXML.InnerXml)
                Dim strAD As String
                'strAD = "0231"
                strAD = dr1.WAR_AD
                Dim strDept As String = "30B20"
                Dim processID As String

                If tWAR_COMPNO = "CL_CHINA" Then
                    strDept = "C11B0"
                    processID = FlowWS.invokeProcess("SHA_WARRSetting", strAD, strDept, formOID, formXML.InnerXml, "WARRSetting")
                Else
                    processID = FlowWS.invokeProcess("WARRSetting", strAD, strDept, formOID, formXML.InnerXml, "WARRSetting")
                End If


                'processID = FlowWS.invokeProcess("WARRSetting", "0231", strDept, formOID, formXML.InnerXml, "WARRSetting")
                'Dim sMessage As String = processID
                '140701 by cipherlab.fairy----(E)
            End If
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_TypeSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
                dtWarrSet = oWarranty.QueryWarrSet(sWar_id, "", "", "", "", "", "")
                If dtWarrSet.Rows.Count > 0 Then
                    Dim dr As WarrantyDTO.WARRSETRow = dtWarrSet.Rows(0)

                    dr.WAR_ID = sWar_id
                    dr.WAR_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
                    dr.WAR_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                    dr.WAR_GROUP = UI_txtProductGroup.Text.ToString().Trim()
                    dr.WAR_TYPE = Me.UI_cboWarrantyType.SelectedValue.ToString().Trim()
                    dr.WAR_DISCOUNT = Convert.ToInt32(Me.UI_txtDiscount.Text.ToString().Trim())
                    dr.WAR_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
                    dr.WAR_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                    dr.WAR_LONGYY = Convert.ToInt32(Me.UI_txtWar_longyy.Text.ToString().Trim())
                    dr.WAR_DESC = Me.UI_txtDescription.Text.ToString().Trim()
                    dr.WAR_SPEC_DESC = Me.UI_txtWarrSpecDesc.Text.ToString().Trim()
                    dr.WAR_CARD_CONTENT = Me.UI_txtWarCardContent.Text.ToString().Trim() ' 加入Warranty Card Content欄位 by buck add 20260128
                    If txtExpDate.Text.Trim() <> "" Then
                        dr.WAR_Expdate = txtExpDate.Text
                    End If
                    If chkOKAll.Checked Then
                        dr.WAR_ISALL = 1
                    Else
                        dr.WAR_ISALL = 0
                    End If
                    dr.WAR_LUAD = Session("_UserID")
                    dr.WAR_LUADNAME = Session("_UserName")
                    dr.WAR_LUSTMP = Date.Now


                    'If Me.UI_cboPriceType.SelectedValue.Trim() = "-1" Then
                    '    dr.WAR_REPAIR_NO = ""
                    'Else
                    '    dr.WAR_REPAIR_NO = Me.UI_cboPriceType.SelectedValue.Trim()
                    'End If

                    dr.WAR_ITEM_TYPE = Me.UI_cboItemType.SelectedValue.Trim()
                    dr.WAR_PROGRAM_TYPE = Me.UI_cboProgramType.SelectedValue.Trim()
                    dr.WAR_PRICE_VER = Me.UI_cboPriceVer.SelectedValue.Trim()
                End If

                Dim dtWPrice As New WarrantyDTO.WPRICEDataTable
                Dim drp As WarrantyDTO.WPRICERow = dtWPrice.NewWPRICERow

                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 1
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice10.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice10.Text.Trim())
                End If
                If Not txtPrice11.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice11.Text.Trim())
                End If
                If Not txtPrice12.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice12.Text.Trim())
                End If
                If Not txtPrice13.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice13.Text.Trim())
                End If
                If Not txtPrice14.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice14.Text.Trim())
                End If
                If Not txtPrice15.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice15.Text.Trim())
                End If
                If Not txtPrice16.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice16.Text.Trim())
                End If
                If Not txtPrice17.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice17.Text.Trim())
                End If
                If Not txtPrice18.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice18.Text.Trim())
                End If
                If Not txtPrice19.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice19.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 2
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice20.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice20.Text.Trim())
                End If
                If Not txtPrice21.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice21.Text.Trim())
                End If
                If Not txtPrice22.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice22.Text.Trim())
                End If
                If Not txtPrice23.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice23.Text.Trim())
                End If
                If Not txtPrice24.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice24.Text.Trim())
                End If
                If Not txtPrice25.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice25.Text.Trim())
                End If
                If Not txtPrice26.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice26.Text.Trim())
                End If
                If Not txtPrice27.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice27.Text.Trim())
                End If
                If Not txtPrice28.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice28.Text.Trim())
                End If
                If Not txtPrice29.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice29.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 3
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice30.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice30.Text.Trim())
                End If
                If Not txtPrice31.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice31.Text.Trim())
                End If
                If Not txtPrice32.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice32.Text.Trim())
                End If
                If Not txtPrice33.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice33.Text.Trim())
                End If
                If Not txtPrice34.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice34.Text.Trim())
                End If
                If Not txtPrice35.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice35.Text.Trim())
                End If
                If Not txtPrice36.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice36.Text.Trim())
                End If
                If Not txtPrice37.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice37.Text.Trim())
                End If
                If Not txtPrice38.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice38.Text.Trim())
                End If
                If Not txtPrice39.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice39.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 4
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice40.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice40.Text.Trim())
                End If
                If Not txtPrice41.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice41.Text.Trim())
                End If
                If Not txtPrice42.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice42.Text.Trim())
                End If
                If Not txtPrice43.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice43.Text.Trim())
                End If
                If Not txtPrice44.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice44.Text.Trim())
                End If
                If Not txtPrice45.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice45.Text.Trim())
                End If
                If Not txtPrice46.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice46.Text.Trim())
                End If
                If Not txtPrice47.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice47.Text.Trim())
                End If
                If Not txtPrice48.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice48.Text.Trim())
                End If
                If Not txtPrice49.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice49.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 5
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice50.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice50.Text.Trim())
                End If
                If Not txtPrice51.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice51.Text.Trim())
                End If
                If Not txtPrice52.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice52.Text.Trim())
                End If
                If Not txtPrice53.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice53.Text.Trim())
                End If
                If Not txtPrice54.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice54.Text.Trim())
                End If
                If Not txtPrice55.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice55.Text.Trim())
                End If
                If Not txtPrice56.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice56.Text.Trim())
                End If
                If Not txtPrice57.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice57.Text.Trim())
                End If
                If Not txtPrice58.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice58.Text.Trim())
                End If
                If Not txtPrice59.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice59.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 6
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice60.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice60.Text.Trim())
                End If
                If Not txtPrice61.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice61.Text.Trim())
                End If
                If Not txtPrice62.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice62.Text.Trim())
                End If
                If Not txtPrice63.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice63.Text.Trim())
                End If
                If Not txtPrice64.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice64.Text.Trim())
                End If
                If Not txtPrice65.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice65.Text.Trim())
                End If
                If Not txtPrice66.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice66.Text.Trim())
                End If
                If Not txtPrice67.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice67.Text.Trim())
                End If
                If Not txtPrice68.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice68.Text.Trim())
                End If
                If Not txtPrice69.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice69.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 7
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice70.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice70.Text.Trim())
                End If
                If Not txtPrice71.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice71.Text.Trim())
                End If
                If Not txtPrice72.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice72.Text.Trim())
                End If
                If Not txtPrice73.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice73.Text.Trim())
                End If
                If Not txtPrice74.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice74.Text.Trim())
                End If
                If Not txtPrice75.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice75.Text.Trim())
                End If
                If Not txtPrice76.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice76.Text.Trim())
                End If
                If Not txtPrice77.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice77.Text.Trim())
                End If
                If Not txtPrice78.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice78.Text.Trim())
                End If
                If Not txtPrice79.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice79.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 8
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice80.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice80.Text.Trim())
                End If
                If Not txtPrice81.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice81.Text.Trim())
                End If
                If Not txtPrice82.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice82.Text.Trim())
                End If
                If Not txtPrice83.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice83.Text.Trim())
                End If
                If Not txtPrice84.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice84.Text.Trim())
                End If
                If Not txtPrice85.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice85.Text.Trim())
                End If
                If Not txtPrice86.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice86.Text.Trim())
                End If
                If Not txtPrice87.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice87.Text.Trim())
                End If
                If Not txtPrice88.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice88.Text.Trim())
                End If
                If Not txtPrice89.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice89.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 9
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice90.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice90.Text.Trim())
                End If
                If Not txtPrice91.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice91.Text.Trim())
                End If
                If Not txtPrice92.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice92.Text.Trim())
                End If
                If Not txtPrice93.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice93.Text.Trim())
                End If
                If Not txtPrice94.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice94.Text.Trim())
                End If
                If Not txtPrice95.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice95.Text.Trim())
                End If
                If Not txtPrice96.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice96.Text.Trim())
                End If
                If Not txtPrice97.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice97.Text.Trim())
                End If
                If Not txtPrice98.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice98.Text.Trim())
                End If
                If Not txtPrice99.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice99.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                drp = dtWPrice.NewWPRICERow
                drp.WP_ID = sWar_id
                drp.WP_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                drp.WP_STYY = 10
                drp.WP_YY01 = 0
                drp.WP_YY02 = 0
                drp.WP_YY03 = 0
                drp.WP_YY04 = 0
                drp.WP_YY05 = 0
                drp.WP_YY06 = 0
                drp.WP_YY07 = 0
                drp.WP_YY08 = 0
                drp.WP_YY09 = 0
                drp.WP_YY10 = 0
                If Not txtPrice100.Text.Trim().Equals("") Then
                    drp.WP_YY01 = Convert.ToDecimal(txtPrice100.Text.Trim())
                End If
                If Not txtPrice101.Text.Trim().Equals("") Then
                    drp.WP_YY02 = Convert.ToDecimal(txtPrice101.Text.Trim())
                End If
                If Not txtPrice102.Text.Trim().Equals("") Then
                    drp.WP_YY03 = Convert.ToDecimal(txtPrice102.Text.Trim())
                End If
                If Not txtPrice103.Text.Trim().Equals("") Then
                    drp.WP_YY04 = Convert.ToDecimal(txtPrice103.Text.Trim())
                End If
                If Not txtPrice104.Text.Trim().Equals("") Then
                    drp.WP_YY05 = Convert.ToDecimal(txtPrice104.Text.Trim())
                End If
                If Not txtPrice105.Text.Trim().Equals("") Then
                    drp.WP_YY06 = Convert.ToDecimal(txtPrice105.Text.Trim())
                End If
                If Not txtPrice106.Text.Trim().Equals("") Then
                    drp.WP_YY07 = Convert.ToDecimal(txtPrice106.Text.Trim())
                End If
                If Not txtPrice107.Text.Trim().Equals("") Then
                    drp.WP_YY08 = Convert.ToDecimal(txtPrice107.Text.Trim())
                End If
                If Not txtPrice108.Text.Trim().Equals("") Then
                    drp.WP_YY09 = Convert.ToDecimal(txtPrice108.Text.Trim())
                End If
                If Not txtPrice109.Text.Trim().Equals("") Then
                    drp.WP_YY10 = Convert.ToDecimal(txtPrice109.Text.Trim())
                End If
                drp.WP_AD = Session("_UserID")
                drp.WP_ADNAME = Session("_UserName")
                drp.WP_CSTMP = Date.Now
                drp.WP_LUAD = Session("_UserID")
                drp.WP_LUADNAME = Session("_UserName")
                drp.WP_LUSTMP = Date.Now
                drp.WP_MARK = 0
                dtWPrice.AddWPRICERow(drp)

                'Save WarrParts Data
                Dim x As Integer
                Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
                For x = 0 To lstParts.Items.Count - 1
                    If lstParts.Items(x).ItemType = ListItemType.Item Or lstParts.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstPartlblSeq As Label = lstParts.Items(x).FindControl("lstPartlblSeq")
                        Dim lstPartlblPart As Label = lstParts.Items(x).FindControl("lstPartlblPart")
                        Dim lstParttxtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtMon")
                        Dim lstParttxtExtMon As TextBox = lstParts.Items(x).FindControl("lstParttxtExtMon")
                        Dim lstParttxtMemo As TextBox = lstParts.Items(x).FindControl("lstParttxtMemo")

                        If Convert.ToInt16(lstParttxtMon.Text) = 0 Then
                            Throw New Exception("Please input Warranty Month!")
                        End If

                        Dim drWp As WarrantyDTO.WarrPartsRow = dtWarrParts.NewWarrPartsRow
                        drWp.WAP_WID = sWar_id
                        drWp.WAP_SEQ = Convert.ToInt16(lstPartlblSeq.Text)
                        drWp.WAP_NAME = lstPartlblPart.Text
                        drWp.WAP_MON = Convert.ToInt16(lstParttxtMon.Text)
                        drWp.WAP_EMON = Convert.ToInt16(lstParttxtExtMon.Text)
                        drWp.WAP_DESC = lstParttxtMemo.Text
                        drWp.WAP_AD = Session("_UserID")
                        drWp.WAP_ADNAME = Session("_UserName")
                        drWp.WAP_CSTMP = Date.Now
                        drWp.WAP_LUAD = Session("_UserID")
                        drWp.WAP_LUADNAME = Session("_UserName")
                        drWp.WAP_LUSTMP = Date.Now
                        dtWarrParts.AddWarrPartsRow(drWp)
                    End If
                Next

                'Save WarrSpecs Data
                Dim dtWarrSpecs As New DataTable
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_WID", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_SEQ", GetType(Integer)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_NAME", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_Rule", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_AD", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_ADNAME", GetType(String)))
                dtWarrSpecs.Columns.Add(New DataColumn("WAP_CSTMP", GetType(String)))

                For x = 0 To lstSpecs.Items.Count - 1
                    If lstSpecs.Items(x).ItemType = ListItemType.Item Or lstSpecs.Items(x).ItemType = ListItemType.AlternatingItem Then
                        Dim lstSpeclblSeq As Label = lstSpecs.Items(x).FindControl("lstSpeclblSeq")
                        Dim lstSpeclblPart As Label = lstSpecs.Items(x).FindControl("lstSpeclblPart")
                        Dim lstSpectxtRule As TextBox = lstSpecs.Items(x).FindControl("lstSpectxtRule")

                        If lstSpectxtRule.Text = "" Then
                            Throw New Exception("Please input Warranty Spec Rule!")
                        End If

                        Dim drWp As DataRow = dtWarrSpecs.NewRow
                        drWp("WAP_WID") = sWar_id
                        drWp("WAP_SEQ") = Convert.ToInt16(lstSpeclblSeq.Text)
                        drWp("WAP_NAME") = lstSpeclblPart.Text
                        drWp("WAP_Rule") = Convert.ToInt16(lstSpectxtRule.Text)
                        drWp("WAP_AD") = Session("_UserID")
                        drWp("WAP_ADNAME") = Session("_UserName")
                        drWp("WAP_CSTMP") = Date.Now
                        dtWarrSpecs.Rows.Add(drWp)
                    End If
                Next

                oWarranty.SaveEditWarrSet(dtWarrSet)
                oWarranty.SaveAddWPrice(dtWPrice)
                oWarranty.SaveAddWarrParts(dtWarrParts)
                oWarranty.SaveAddWarrSpecs(dtWarrSpecs)
                oWarranty.SaveAddWarrSetPart(sWar_id)
            End If
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_TypeSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub btnInvalidAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvalidAll.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                oWarranty.SaveInvalidWPrice(sWar_id, Session("_UserID"), Session("_UserName"))

                txtPrice10.Text = ""
                txtPrice11.Text = ""
                txtPrice12.Text = ""
                txtPrice13.Text = ""
                txtPrice14.Text = ""
                txtPrice15.Text = ""
                txtPrice16.Text = ""
                txtPrice17.Text = ""
                txtPrice18.Text = ""
                txtPrice19.Text = ""

                txtPrice20.Text = ""
                txtPrice21.Text = ""
                ''txtPrice22.Text = ""
                txtPrice23.Text = ""
                txtPrice24.Text = ""
                txtPrice25.Text = ""
                txtPrice26.Text = ""
                txtPrice27.Text = ""
                txtPrice28.Text = ""
                txtPrice29.Text = ""

                txtPrice30.Text = ""
                txtPrice31.Text = ""
                txtPrice32.Text = ""
                txtPrice33.Text = ""
                txtPrice34.Text = ""
                txtPrice35.Text = ""
                txtPrice36.Text = ""
                txtPrice37.Text = ""
                txtPrice38.Text = ""
                txtPrice39.Text = ""

                txtPrice40.Text = ""
                txtPrice41.Text = ""
                txtPrice42.Text = ""
                txtPrice43.Text = ""
                txtPrice44.Text = ""
                txtPrice45.Text = ""
                txtPrice46.Text = ""
                txtPrice47.Text = ""
                txtPrice48.Text = ""
                txtPrice49.Text = ""

                txtPrice50.Text = ""
                txtPrice51.Text = ""
                txtPrice52.Text = ""
                txtPrice53.Text = ""
                txtPrice54.Text = ""
                txtPrice55.Text = ""
                txtPrice56.Text = ""
                txtPrice57.Text = ""
                txtPrice58.Text = ""
                txtPrice59.Text = ""

                txtPrice60.Text = ""
                txtPrice61.Text = ""
                txtPrice62.Text = ""
                txtPrice63.Text = ""
                txtPrice64.Text = ""
                txtPrice65.Text = ""
                txtPrice66.Text = ""
                txtPrice67.Text = ""
                txtPrice68.Text = ""
                txtPrice69.Text = ""

                txtPrice70.Text = ""
                txtPrice71.Text = ""
                txtPrice72.Text = ""
                txtPrice73.Text = ""
                txtPrice74.Text = ""
                txtPrice75.Text = ""
                txtPrice76.Text = ""
                txtPrice77.Text = ""
                txtPrice78.Text = ""
                txtPrice79.Text = ""

                txtPrice80.Text = ""
                txtPrice81.Text = ""
                txtPrice82.Text = ""
                txtPrice83.Text = ""
                txtPrice84.Text = ""
                txtPrice85.Text = ""
                txtPrice86.Text = ""
                txtPrice87.Text = ""
                txtPrice88.Text = ""
                txtPrice89.Text = ""

                txtPrice90.Text = ""
                txtPrice91.Text = ""
                txtPrice92.Text = ""
                txtPrice93.Text = ""
                txtPrice94.Text = ""
                txtPrice95.Text = ""
                txtPrice96.Text = ""
                txtPrice97.Text = ""
                txtPrice98.Text = ""
                txtPrice99.Text = ""

                txtPrice100.Text = ""
                txtPrice101.Text = ""
                txtPrice102.Text = ""
                txtPrice103.Text = ""
                txtPrice104.Text = ""
                txtPrice105.Text = ""
                txtPrice106.Text = ""
                txtPrice107.Text = ""
                txtPrice108.Text = ""
                txtPrice109.Text = ""

            End If
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
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
    End Sub

    Protected Sub chkOKAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOKAll.CheckedChanged
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If chkOKAll.Checked Then
                btnValidClient.Visible = False
            Else
                btnValidClient.Visible = True
            End If
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    Protected Sub btnValidClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidClient.Click
        Me.ucValidCustomer.show(True, lblSwID.Text.ToString().Trim(), UI_cboOperationCenter.SelectedValue.Trim(), lblSwIDStatus.Text)
    End Sub

    Public Sub GetYeas()
        Dim nMin As Integer = Convert.ToInt32(UI_Years.Text)
        Dim nMax As Integer = Convert.ToInt32(UI_txtWar_longyy.Text)
        Dim oContentPlaceHolder As ContentPlaceHolder = Me.Master.FindControl("ContentPlaceHolder")
        Dim n1 As Integer = 0
        Dim n2 As Integer = 0
        For n1 = 1 To 10
            For n2 = 0 To 9
                Dim txt As TextBox = oContentPlaceHolder.FindControl("txtPrice" + n1.ToString("") + n2.ToString())
                If Not txt Is Nothing Then
                    txt.Style.Add("display", "none")
                End If
            Next
        Next

        For n1 = 1 To nMax
            For n2 = nMin - n1 + 1 To nMax - n1
                If n2 > -1 Then
                    Dim txt As TextBox = oContentPlaceHolder.FindControl("txtPrice" + n1.ToString("") + n2.ToString())
                    If Not txt Is Nothing Then
                        txt.Style.Add("display", "")
                    End If
                End If
            Next
        Next

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Warranty_TypeSetting.aspx")
    End Sub
    Protected Sub btnBackTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackTop.Click
        Response.Redirect("Warranty_TypeSetting.aspx")
    End Sub

    Protected Sub btnInValid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInValid.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                oWarranty.SaveInvalidWarrSet(sWar_id, Session("_UserID"), Session("_UserName"), 3)
            End If
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_TypeSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub btnValid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValid.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                oWarranty.SaveInvalidWarrSet(sWar_id, Session("_UserID"), Session("_UserName"), 0)
            End If
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_TypeSetting.aspx")
            End If
        End Try
    End Sub

    Private Sub SetToolTip()
        Dim sTip As String = UI_txtProductGroup.Text.Trim()
        Dim nStart As String = Convert.ToInt16(UI_Years.Text)
        Dim nYear As String = Convert.ToInt16(UI_Years.Text)
        Dim nCW As Integer = 0
        If UI_cboWarrantyType.SelectedIndex > -1 Then sTip = sTip & UI_cboWarrantyType.SelectedValue.Trim()
        If UI_cboProgramType.SelectedIndex > -1 Then sTip = sTip & UI_cboProgramType.SelectedValue.Trim()
        If UI_cboItemType.SelectedIndex > -1 Then sTip = sTip & UI_cboItemType.SelectedValue.Trim()
        If UI_cboPriceVer.SelectedIndex > -1 Then sTip = sTip & UI_cboPriceVer.SelectedValue.Trim()
        'sTip = sTip & "00"
        sTip = sTip & UI_txtVersion.Text.Trim().Substring(1, 2)

        nStart = 1

        '1
        If txtPrice10.Style("display") <> "none" Then
            txtPrice10.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice11.Style("display") <> "none" Then
            txtPrice11.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice12.Style("display") <> "none" Then
            txtPrice12.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice13.Style("display") <> "none" Then
            txtPrice13.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice14.Style("display") <> "none" Then
            txtPrice14.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice15.Style("display") <> "none" Then
            txtPrice15.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice16.Style("display") <> "none" Then
            txtPrice16.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice17.Style("display") <> "none" Then
            txtPrice17.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice18.Style("display") <> "none" Then
            txtPrice18.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If

        If txtPrice19.Style("display") <> "none" Then
            txtPrice19.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1

        '2
        If txtPrice20.Style("display") <> "none" Then
            txtPrice20.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice21.Style("display") <> "none" Then
            txtPrice21.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice22.Style("display") <> "none" Then
            txtPrice22.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice23.Style("display") <> "none" Then
            txtPrice23.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice24.Visible Then
            txtPrice24.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice25.Style("display") <> "none" Then
            txtPrice25.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice26.Style("display") <> "none" Then
            txtPrice26.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice27.Style("display") <> "none" Then
            txtPrice27.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice28.Style("display") <> "none" Then
            txtPrice28.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice29.Style("display") <> "none" Then
            txtPrice29.ToolTip = sTip & "1" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1

        '3
        If txtPrice30.Style("display") <> "none" Then
            txtPrice30.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice31.Style("display") <> "none" Then
            txtPrice31.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice32.Style("display") <> "none" Then
            txtPrice32.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice33.Style("display") <> "none" Then
            txtPrice33.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice34.Style("display") <> "none" Then
            txtPrice34.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice35.Style("display") <> "none" Then
            txtPrice35.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice36.Style("display") <> "none" Then
            txtPrice36.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice37.Style("display") <> "none" Then
            txtPrice37.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice38.Style("display") <> "none" Then
            txtPrice38.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice39.Style("display") <> "none" Then
            txtPrice39.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '4
        If txtPrice40.Style("display") <> "none" Then
            txtPrice40.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice41.Style("display") <> "none" Then
            txtPrice41.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice42.Style("display") <> "none" Then
            txtPrice42.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice43.Style("display") <> "none" Then
            txtPrice43.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice44.Style("display") <> "none" Then
            txtPrice44.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice45.Style("display") <> "none" Then
            txtPrice45.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice46.Style("display") <> "none" Then
            txtPrice46.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice47.Style("display") <> "none" Then
            txtPrice47.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice48.Style("display") <> "none" Then
            txtPrice48.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice49.Style("display") <> "none" Then
            txtPrice49.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '5
        If txtPrice50.Style("display") <> "none" Then
            txtPrice50.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice51.Style("display") <> "none" Then
            txtPrice51.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice52.Style("display") <> "none" Then
            txtPrice52.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice53.Style("display") <> "none" Then
            txtPrice53.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice54.Style("display") <> "none" Then
            txtPrice54.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice55.Style("display") <> "none" Then
            txtPrice39.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice56.Style("display") <> "none" Then
            txtPrice56.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice57.Style("display") <> "none" Then
            txtPrice57.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice58.Style("display") <> "none" Then
            txtPrice58.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice59.Style("display") <> "none" Then
            txtPrice59.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '6
        If txtPrice60.Style("display") <> "none" Then
            txtPrice60.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice61.Style("display") <> "none" Then
            txtPrice61.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice62.Style("display") <> "none" Then
            txtPrice62.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice63.Style("display") <> "none" Then
            txtPrice63.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice64.Style("display") <> "none" Then
            txtPrice64.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice65.Style("display") <> "none" Then
            txtPrice65.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice66.Style("display") <> "none" Then
            txtPrice66.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice67.Style("display") <> "none" Then
            txtPrice67.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice68.Style("display") <> "none" Then
            txtPrice68.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice69.Style("display") <> "none" Then
            txtPrice69.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '7
        If txtPrice70.Style("display") <> "none" Then
            txtPrice70.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice71.Style("display") <> "none" Then
            txtPrice71.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice72.Style("display") <> "none" Then
            txtPrice72.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice73.Style("display") <> "none" Then
            txtPrice73.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice74.Style("display") <> "none" Then
            txtPrice74.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice75.Style("display") <> "none" Then
            txtPrice75.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice76.Style("display") <> "none" Then
            txtPrice76.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice77.Style("display") <> "none" Then
            txtPrice77.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice78.Style("display") <> "none" Then
            txtPrice78.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice79.Style("display") <> "none" Then
            txtPrice79.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '8
        If txtPrice80.Style("display") <> "none" Then
            txtPrice80.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice81.Style("display") <> "none" Then
            txtPrice81.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice82.Style("display") <> "none" Then
            txtPrice82.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice83.Style("display") <> "none" Then
            txtPrice83.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice84.Style("display") <> "none" Then
            txtPrice84.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice85.Style("display") <> "none" Then
            txtPrice85.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice87.Style("display") <> "none" Then
            txtPrice87.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice88.Style("display") <> "none" Then
            txtPrice88.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice89.Style("display") <> "none" Then
            txtPrice89.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '9
        If txtPrice90.Style("display") <> "none" Then
            txtPrice90.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice91.Style("display") <> "none" Then
            txtPrice91.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice92.Style("display") <> "none" Then
            txtPrice92.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice93.Style("display") <> "none" Then
            txtPrice93.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice94.Style("display") <> "none" Then
            txtPrice94.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice95.Style("display") <> "none" Then
            txtPrice95.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice96.Style("display") <> "none" Then
            txtPrice96.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice97.Style("display") <> "none" Then
            txtPrice97.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice98.Style("display") <> "none" Then
            txtPrice98.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice99.Style("display") <> "none" Then
            txtPrice99.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

        nStart = 1
        '10
        If txtPrice100.Style("display") <> "none" Then
            txtPrice100.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice101.Style("display") <> "none" Then
            txtPrice101.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice102.Style("display") <> "none" Then
            txtPrice102.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice103.Style("display") <> "none" Then
            txtPrice103.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice104.Style("display") <> "none" Then
            txtPrice104.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice105.Style("display") <> "none" Then
            txtPrice105.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice106.Style("display") <> "none" Then
            txtPrice106.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice107.Style("display") <> "none" Then
            txtPrice107.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice108.Style("display") <> "none" Then
            txtPrice108.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If
        If txtPrice109.Style("display") <> "none" Then
            txtPrice109.ToolTip = sTip & "R" + (nStart).ToString()
            nStart += 1
        End If

    End Sub

    Protected Sub btnPartsAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPartsAdd.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If cboPartsType.SelectedIndex > -1 Then
            Dim sPartsNo = cboPartsType.SelectedValue.Trim()
            Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            dtWarrParts = ViewState("dtWarrParts")
            Dim nSeq As Integer = dtWarrParts.Rows.Count
            Dim sWar_id As String = Me.lblSwID.Text.ToString().Trim()

            Dim drWp As WarrantyDTO.WarrPartsRow = dtWarrParts.NewWarrPartsRow
            drWp.WAP_WID = sWar_id
            drWp.WAP_SEQ = nSeq
            drWp.WAP_NAME = sPartsNo
            drWp.WAP_MON = 0
            drWp.WAP_EMON = 0
            drWp.WAP_DESC = ""
            drWp.WAP_AD = Session("_UserID")
            drWp.WAP_ADNAME = Session("_UserName")
            drWp.WAP_CSTMP = Date.Now
            drWp.WAP_LUAD = Session("_UserID")
            drWp.WAP_LUADNAME = Session("_UserName")
            drWp.WAP_LUSTMP = Date.Now
            dtWarrParts.AddWarrPartsRow(drWp)

            ViewState("dtWarrParts") = dtWarrParts

            lstParts.DataSource = dtWarrParts
            lstParts.DataBind()

            cboPartsType.Items.RemoveAt(cboPartsType.SelectedIndex)
        End If

    End Sub

    Protected Sub btnSpecsAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSpecsAdd.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If cboSpecsType.SelectedIndex > -1 Then
            Dim sSpecsNo = cboSpecsType.SelectedValue.Trim()
            Dim dtWarrSpecs As New DataTable
            dtWarrSpecs = ViewState("dtWarrSpecs")
            Dim nSeq As Integer = dtWarrSpecs.Rows.Count
            Dim sWar_id As String = Me.lblSwID.Text.ToString().Trim()

            Dim drWp As DataRow = dtWarrSpecs.NewRow
            drWp("WAP_WID") = sWar_id
            drWp("WAP_SEQ") = nSeq
            drWp("WAP_NAME") = sSpecsNo
            drWp("WAP_Rule") = "0"
            drWp("WAP_AD") = Session("_UserID")
            drWp("WAP_ADNAME") = Session("_UserName")
            drWp("WAP_CSTMP") = Date.Now
            dtWarrSpecs.Rows.Add(drWp)

            ViewState("dtWarrSpecs") = dtWarrSpecs

            lstSpecs.DataSource = dtWarrSpecs
            lstSpecs.DataBind()

            cboSpecsType.Items.RemoveAt(cboSpecsType.SelectedIndex)
        End If

    End Sub

    Protected Sub UI_cboWarrantyType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UI_cboWarrantyType.SelectedIndexChanged
        GetWarrsetData(UI_cboWarrantyType.SelectedValue)
        lbl_part_no.Text = UI_txtProductGroup.Text + UI_cboWarrantyType.Text + UI_cboProgramType.Text + UI_cboItemType.Text + UI_cboPriceVer.Text + UI_txtVersion.Text
    End Sub

    Private Sub GetWarrsetData(WarrsetType As String)
        'Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        Dim sRepairText As String = ""

        oCommon.getPriceVerByDropDownList(Me.UI_cboPriceVer, WarrsetType, sRepairText)
        oCommon.getProgramTypeByDropDownList(Me.UI_cboProgramType, WarrsetType, sRepairText)
        oCommon.getItemTypeByDropDownList(Me.UI_cboItemType, WarrsetType, sRepairText)

        Dim sPart_nm As String = oCommon.GetWarrsetPartNM(UI_txtProductGroup.Text, UI_cboWarrantyType.Text, UI_cboProgramType.Text, UI_cboItemType.Text, UI_cboPriceVer.Text)

        UI_txtWarrantyName.Text = sPart_nm

    End Sub

    <WebMethod()> Public Shared Function GetWarrsetPartNM(ByVal War_Group As String, ByVal WARRSET_TYPE As String, ByVal Program_Type As String, ByVal Item_Type As String, ByVal Price_Ver As String) As String
        Dim oCommon As New Common

        Dim sPart_nm As String = oCommon.GetWarrsetPartNM(War_Group, WARRSET_TYPE, Program_Type, Item_Type, Price_Ver)
        Return sPart_nm

    End Function

End Class

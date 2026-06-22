Imports System.Data
Imports System.IO    '140701 by cipherlab.fairy
Imports System.Xml         '140701 by cipherlab.fairy
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel

Partial Class Warranty_SpecialSetting_add
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim sUploadPath As String = ConfigurationSettings.AppSettings("EmailAttachFolder")
    Public sUploadUrl As String = ConfigurationSettings.AppSettings("UploadUrl")
    Dim _RepairBOM_No As String = ConfigurationSettings.AppSettings("RepairBOM_No")

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim lblPreviousPage_SwID As Label = oContentPlaceHolder.FindControl("lblPreviousPage_SwID")
                Me.lblSwID.Text = lblPreviousPage_SwID.Text.ToString().Trim()
                pnDesc.Visible = False

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.lblSwID.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()

                Dim oWarranty As New ctlWarranty
                Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
                dtProductGroup = oWarranty.QueryPrdGroup("", "")
                ViewState("_dtProductGroup") = dtProductGroup

                'Dim dtParts As New WarrantyDTO.vwPartsDataTable
                'dtParts = oWarranty.QueryParts("", "")
                'ViewState("_dtParts") = dtParts

                Call QueryData()
            End If
        End If
    End Sub

    Private Sub setControls()
        Me.rfvMonth.ErrorMessage = _oLanguage.getText("Warranty", "101", ctlLanguage.eumType.Validator)
        Me.rfvProductGroup.ErrorMessage = _oLanguage.getText("Warranty", "102", ctlLanguage.eumType.Validator)
        Me.rfvWarrName.ErrorMessage = _oLanguage.getText("Warranty", "103", ctlLanguage.eumType.Validator)
        Me.rfvYears.ErrorMessage = _oLanguage.getText("Warranty", "104", ctlLanguage.eumType.Validator)
        Me.rfvPrice.ErrorMessage = _oLanguage.getText("Warranty", "105", ctlLanguage.eumType.Validator)


        '取得Tag Text
        Call oCommon.getCostCenterByDropDownList(False, Me.UI_cboOperationCenter, "")
        Call oCommon.getWarrantyTypeByDropDownList(False, Me.UI_cboWarrantyType, "")
        Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "035", ctlLanguage.eumType.Tag)
        Me.UI_lblSubTittle.Text = _oLanguage.getText("Warranty", "038", ctlLanguage.eumType.Tag)
        Me.UI_lblOperationCenter.Text = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyType.Text = _oLanguage.getText("Warranty", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblExtraMonths.Text = _oLanguage.getText("Warranty", "031", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyName.Text = _oLanguage.getText("Warranty", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblYears.Text = _oLanguage.getText("Warranty", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblVersion.Text = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("Warranty", "036", ctlLanguage.eumType.Tag) + "  :"
        Me.UI_lblUnitPrice.Text = _oLanguage.getText("Warranty", "037", ctlLanguage.eumType.Tag)
        Me.UI_cmdPartPick.Text = _oLanguage.getText("Warranty", "041", ctlLanguage.eumType.Tag)
        Me.UI_cmdProductGroupPick.Text = _oLanguage.getText("Warranty", "040", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Warranty", "039", ctlLanguage.eumType.Tag)
        Me.btnFileAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdSearchPart.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdToPart.Text = _oLanguage.getText("Warranty", "042", ctlLanguage.eumType.Tag)

        Me.UI_cmdDelete.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        Me.UI_cmdSavePart.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)

        lblPartsInform.Text = _oLanguage.getText("Warranty", "043", ctlLanguage.eumType.Tag)
        lblAttachments.Text = _oLanguage.getText("Warranty", "044", ctlLanguage.eumType.Tag)
        lblPartNoQuery.Text = _oLanguage.getText("Warranty", "045", ctlLanguage.eumType.Tag)
        lblPartDescQuery.Text = _oLanguage.getText("Warranty", "046", ctlLanguage.eumType.Tag)

        lblPartAllPart.Text = _oLanguage.getText("Warranty", "047", ctlLanguage.eumType.Tag)
        lblPartAllDesc.Text = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        lblPartOKPart.Text = _oLanguage.getText("Warranty", "047", ctlLanguage.eumType.Tag)
        lblPartOKDesc.Text = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        lblPartOKYears.Text = _oLanguage.getText("Warranty", "032", ctlLanguage.eumType.Tag)

        UI_cmdPartPick.Visible = False
        pnlFile.Visible = False
        UI_txtVersion.Enabled = False
    End Sub

    Private Sub QueryData()
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Exit Sub
        End If

        Dim oWarranty As New ctlWarranty
        Dim dtSwSet As New WarrantyDTO.SWSETDataTable
        Dim sSwID As String = Me.lblSwID.Text.ToString().Trim()
        UI_cmdSave.Visible = False
        UI_cmdPartPick.Visible = False
        UI_cmdProductGroupPick.Visible = False

        dtSwSet = oWarranty.QuerySWSet(sSwID, "", "", -1, "", "")
        Dim i As Integer = 0
        If dtSwSet.Count > 0 Then
            Dim dr As WarrantyDTO.SWSETRow = dtSwSet.Rows(0)

            UI_cboOperationCenter.SelectedValue = dr.SW_COMPNO.ToString().Trim()
            UI_cboWarrantyType.SelectedValue = dr.SW_TYPE01.ToString().Trim()
            UI_cboWarrantyTypeoth.SelectedValue = dr.SW_TYPE02.ToString().Trim()

            If dr.SW_TYPE01.ToString() = "1" Then
                UI_cboWarrantyTypeoth.Visible = True
                UI_lblDescription.Visible = True
                UI_txtDescription.Visible = True
                pnDesc.Visible = False
            Else
                UI_cboWarrantyTypeoth.Visible = False
                UI_lblDescription.Visible = False
                UI_txtDescription.Visible = False
                pnDesc.Visible = True
            End If

            UI_ExtraMonths.Text = dr.SW_EXTMM.ToString().Trim()
            UI_txtProductGroup.Text = dr.SW_GROUP.ToString().Trim()
            UI_txtWarrantyName.Text = dr.SW_NAME.ToString().Trim()
            UI_Years.Text = dr.SW_STDYY.ToString().Trim()
            UI_txtVersion.Text = dr.SW_VERSION.ToString("000").Trim()
            If Not dr.IsSW_DESCNull Then
                UI_txtDescription.Text = dr.SW_DESC.ToString().Trim()
                txtDescription.Text = dr.SW_DESC.ToString().Trim()
            End If
            If Not dr.IsSW_ExpdateNull Then
                txtExpDate.Text = dr.SW_Expdate.ToString("yyyy/MM/dd")
            End If
            UI_UnitPrice.Text = dr.SW_PRICE.ToString().Trim()

            UI_cboOperationCenter.Enabled = False
            UI_txtProductGroup.Enabled = False
            UI_txtVersion.Enabled = False
            UI_cboWarrantyType.Enabled = False
            UI_cboWarrantyTypeoth.Enabled = False
            UI_Years.Enabled = False
            UI_ExtraMonths.Enabled = False

            Dim dtSWFile As New WarrantyDTO.SWFILEDataTable
            dtSWFile = oWarranty.QuerySWFile(lblSwID.Text, "", "")
            lstFile.DataSource = dtSWFile
            lstFile.DataBind()

            btnFileAdd.Visible = False
            UI_cmdSave.Visible = False
            UI_cmdSearchPart.Visible = False
            UI_cmdDelete.Visible = False
            UI_cmdSavePart.Visible = False
            UI_cmdSubmit.Visible = False
            UI_cmdToPart.Visible = False
            btnInvalid.Visible = False
            btnValid.Visible = False
            chkOKAll.Enabled = False
            chkPartAll.Enabled = False

            If dr.SW_STATUS = 0 Then
                UI_cmdSearchPart.Visible = True
                btnFileAdd.Visible = True
                UI_cmdToPart.Visible = True
                UI_cmdSavePart.Visible = True
                chkOKAll.Enabled = True
                chkPartAll.Enabled = True

                UI_cmdDelete.Visible = True
                UI_cmdSubmit.Visible = True
                btnInvalid.Visible = True
            End If

            If dr.SW_STATUS = 1 Then
            End If

            If dr.SW_STATUS = 2 Then
                btnValid.Visible = True
            End If

            If dr.SW_STATUS = 3 Then
                'btnValid.Visible = True
            End If

            If UI_cboWarrantyType.SelectedValue <> 1 Then
                pnParts.Visible = False
                UI_cmdDelete.Visible = False
            Else
                pnParts.Visible = True
                UI_cmdDelete.Visible = True
            End If

        End If

        Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
        pnlFile.Visible = True
        dtSWParts = oWarranty.QuerySWParts(sSwID, "", 1, "")
        lstOKPartNo.DataSource = dtSWParts
        lstOKPartNo.DataBind()

        dtSWParts = oWarranty.QuerySWParts(sSwID, "", 0, "")
        lstAllPartNo.DataSource = dtSWParts
        lstAllPartNo.DataBind()
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
        Dim sSwID As String = lblSwID.Text.ToString().Trim()

        Dim oWarranty As New ctlWarranty
        Dim dtSWSet As New WarrantyDTO.SWSETDataTable
        Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable

        Try
            Dim dr As WarrantyDTO.SWSETRow = dtSWSet.NewSWSETRow

            dr.SW_ID = sSwID.ToString().Trim()
            dr.SW_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
            dr.SW_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
            dr.SW_GROUP = UI_txtProductGroup.Text.ToString().Trim()
            dr.SW_TYPE01 = Convert.ToInt32(Me.UI_cboWarrantyType.SelectedValue.ToString().Trim())
            dr.SW_TYPE02 = Convert.ToInt32(Me.UI_cboWarrantyTypeoth.SelectedValue.ToString().Trim())
            dr.SW_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
            dr.SW_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
            dr.SW_DESC = Me.UI_txtDescription.Text.ToString().Trim()
            dr.SW_PRICE = Convert.ToDouble(UI_UnitPrice.Text)
            dr.SW_STATUS = 0
            If txtExpDate.Text.Trim() <> "" Then
                dr.SW_Expdate = txtExpDate.Text
            End If

            dr.SW_AD = Session("_UserID")
            dr.SW_ADNAME = Session("_UserName")
            dr.SW_CSTMP = Date.Now
            dr.SW_LUAD = Session("_UserID")
            dr.SW_LUADNAME = Session("_UserName")
            dr.SW_LUSTMP = Date.Now
            dr.SW_MARK = 0
            dtSWSet.AddSWSETRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    sSwID = oWarranty.SaveAddSWSet(dtSWSet)
                    lblSwID.Text = sSwID

                    If Not Session("dtSelectPart") Is Nothing Then
                        Dim dt As DataTable = Session("dtSelectPart")
                        For i = 0 To dt.Rows.Count - 1
                            Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                            drp.SWP_ID = sSwID
                            drp.SWP_PARTNO = dt.Rows(i)("PartNo").ToString()
                            drp.SWP_DESC = dt.Rows(i)("PartDesc").ToString()
                            drp.SWP_YEAR = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                            drp.SWP_AD = Session("_UserID")
                            drp.SWP_ADNAME = Session("_UserName")
                            drp.SWP_CSTMP = Date.Now
                            drp.SWP_LUAD = Session("_UserID")
                            drp.SWP_LUADNAME = Session("_UserName")
                            drp.SWP_LUSTMP = Date.Now
                            drp.SWP_MARK = 0
                            drp.SWP_FLAG = 0
                            dtSWParts.AddSWPARTSRow(drp)
                        Next
                    End If

                    '根據機種抓材料資料
                    Dim ctlReport As New ctlReport
                    Dim sBomFGPartNo As String = ""
                    Dim dtReport As New ReportDTO.Rpt_BOMDataTable
                    '_RepairBOM_No
                    dtReport = ctlReport.QuerySpare_PrimalSN(Me.UI_txtProductGroup.Text, _RepairBOM_No, sBomFGPartNo)
                    'dtReport = ctlReport.QuerySpare_PrimalSN(Me.UI_txtProductGroup.Text, UI_cboOperationCenter.SelectedValue.Trim(), sBomFGPartNo)
                    For i = 0 To dtReport.Rows.Count - 1
                        Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                        drp.SWP_ID = sSwID
                        drp.SWP_PARTNO = dtReport.Rows(i)("bmb03").ToString()
                        drp.SWP_DESC = dtReport.Rows(i)("RPBOM_DESC").ToString()
                        drp.SWP_YEAR = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                        drp.SWP_AD = Session("_UserID")
                        drp.SWP_ADNAME = Session("_UserName")
                        drp.SWP_CSTMP = Date.Now
                        drp.SWP_LUAD = Session("_UserID")
                        drp.SWP_LUADNAME = Session("_UserName")
                        drp.SWP_LUSTMP = Date.Now
                        drp.SWP_MARK = 0
                        drp.SWP_FLAG = 0
                        dtSWParts.AddSWPARTSRow(drp)
                    Next

                    oWarranty.SaveSWParts(dtSWParts)

                    btnInvalid.Visible = False
                    UI_cmdSave.Visible = False
                    UI_cmdPartPick.Visible = False
                    UI_cmdProductGroupPick.Visible = False
                Case eumCommand.UPDATE
                    oWarranty.SaveAddSWSet(dtSWSet)

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
                UI_cboOperationCenter.Enabled = False
                UI_cboWarrantyType.Enabled = False
                UI_cboWarrantyTypeoth.Enabled = False
                UI_cmdPartPick.Visible = False
                UI_ExtraMonths.Enabled = False
                UI_txtProductGroup.Enabled = False
                UI_cmdProductGroupPick.Visible = False
                UI_txtWarrantyName.Enabled = False
                UI_txtVersion.Enabled = False
                UI_Years.Enabled = False
                UI_txtDescription.Enabled = False
                UI_UnitPrice.Enabled = False

                If UI_cboWarrantyType.SelectedValue <> 1 Then
                    pnParts.Visible = False
                    UI_cmdDelete.Visible = False
                End If

                dtSWParts = oWarranty.QuerySWParts(sSwID, "", 1, "")
                lstOKPartNo.DataSource = dtSWParts
                lstOKPartNo.DataBind()

                dtSWParts = oWarranty.QuerySWParts(sSwID, "", 0, "")
                lstAllPartNo.DataSource = dtSWParts
                lstAllPartNo.DataBind()
            End If
        End Try
    End Sub

    Protected Sub UI_cmdPartPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPartPick.Click
        Dim dtParts As DataTable = Me.ViewState("_dtParts")
        Me.ucParts.show(dtParts, True)
    End Sub

    Protected Sub UI_cmdProductGroupPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdProductGroupPick.Click
        Dim dtProductGroup As DataTable = Me.ViewState("_dtProductGroup")
        Me.ucProductGroup.show(dtProductGroup, True)
    End Sub

    Protected Sub UI_cboWarrantyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboWarrantyType.SelectedIndexChanged
        If UI_cboWarrantyType.SelectedValue.ToString() = "1" Then
            UI_cboWarrantyTypeoth.Visible = True
            UI_cmdPartPick.Visible = True
            pnDesc.Visible = False
            UI_lblDescription.Visible = True
            UI_txtDescription.Visible = True
        Else
            UI_cboWarrantyTypeoth.Visible = False
            UI_cmdPartPick.Visible = False
            pnDesc.Visible = True
            UI_lblDescription.Visible = False
            UI_txtDescription.Visible = False
        End If
    End Sub

    Protected Sub btnFileAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFileAdd.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim sFileName As String = ""
        Try
            If FileUp.PostedFile Is Nothing Then
                Throw New Exception("Please choose file first")
            Else
                Dim FileSplit() As String = FileUp.PostedFile.FileName.Split("\\")
                Dim FileName As String = FileSplit.GetValue(FileSplit.Length - 1).ToString()
                FileUp.PostedFile.SaveAs(sUploadPath + "\\" + FileName)
                sFileName = FileName

                Dim dtSWFile As New WarrantyDTO.SWFILEDataTable
                Dim drp As WarrantyDTO.SWFILERow = dtSWFile.NewSWFILERow
                drp.SWF_ID = lblSwID.Text
                drp.SWF_FILE = sFileName
                drp.SWF_MARK = 0
                drp.SWF_AD = Session("_UserID")
                drp.SWF_ADNAME = Session("_UserName")
                drp.SWF_CSTMP = Date.Now
                drp.SWF_LUAD = Session("_UserID")
                drp.SWF_LUADNAME = Session("_UserName")
                drp.SWF_LUSTMP = Date.Now
                dtSWFile.Rows.Add(drp)

                Dim oWarranty As New ctlWarranty
                oWarranty.SaveAddSWFile(dtSWFile)
                blnFlag = True

                dtSWFile = oWarranty.QuerySWFile(lblSwID.Text, "", "")
                lstFile.DataSource = dtSWFile
                lstFile.DataBind()

            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                'Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
    End Sub

    Public Function Excel2007ToDataSet(ByVal ExcelFileName As String) As DataSet
        Dim file As FileStream = New FileStream(ExcelFileName, FileMode.Open, FileAccess.Read)
        Dim hssfworkbook As HSSFWorkbook = New HSSFWorkbook(file)
        Dim sheet As HSSFSheet = hssfworkbook.GetSheetAt(0)
        Dim rows As System.Collections.IEnumerator = sheet.GetRowEnumerator()
        Dim dt As DataTable = New DataTable()
        Dim t As Integer = 0
        Dim i As Integer = 0
        While rows.MoveNext()
            Dim row As HSSFRow = rows.Current
            Dim cellCount As Integer = row.LastCellNum
            Dim dr As DataRow = dt.NewRow()
            For i = 0 To row.LastCellNum - 1
                Dim cell As HSSFCell = row.GetCell(i)
                If t = 0 Then
                    Dim column As DataColumn = New DataColumn(cell.ToString())
                    dt.Columns.Add(column)
                End If
                If cell Is Nothing Then
                    dr(i) = ""
                Else
                    dr(i) = cell.ToString()
                End If
            Next

            dt.Rows.Add(dr)
            t = t + 1
        End While


        Dim dataSet1 As DataSet = New DataSet()
        dataSet1.Tables.Add(dt)

        Return dataSet1

    End Function

    Protected Sub UI_cmdToPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdToPart.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
            Dim i As Integer = 0

            For i = 0 To lstAllPartNo.Items.Count - 1
                If lstAllPartNo.Items(i).ItemType = ListItemType.Item Or lstAllPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim chkExcelPart As CheckBox = lstAllPartNo.Items(i).FindControl("chkExcelPart")
                    Dim lblPartNo As Label = lstAllPartNo.Items(i).FindControl("lblPartNo")
                    Dim lblPartDesc As Label = lstAllPartNo.Items(i).FindControl("lblPartDesc")
                    If chkExcelPart.Checked Then
                        Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                        drp.SWP_YEAR = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                        drp.SWP_AD = Session("_UserID")
                        drp.SWP_ADNAME = Session("_UserName")
                        drp.SWP_CSTMP = Date.Now
                        drp.SWP_LUAD = Session("_UserID")
                        drp.SWP_LUADNAME = Session("_UserName")
                        drp.SWP_LUSTMP = Date.Now
                        drp.SWP_MARK = 0
                        drp.SWP_FLAG = 1
                        drp.SWP_ID = lblSwID.Text
                        drp.SWP_PARTNO = lblPartNo.Text
                        drp.SWP_DESC = lblPartDesc.Text
                        dtSWParts.Rows.Add(drp)
                    End If
                End If
            Next

            Dim oWarranty As New ctlWarranty
            oWarranty.SaveSWParts(dtSWParts)

            dtSWParts = oWarranty.QuerySWParts(lblSwID.Text, "", 1, "")
            lstOKPartNo.DataSource = dtSWParts
            lstOKPartNo.DataBind()

            dtSWParts = oWarranty.QuerySWParts(lblSwID.Text, "", 0, "")
            lstAllPartNo.DataSource = dtSWParts
            lstAllPartNo.DataBind()
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

    Protected Sub UI_cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdDelete.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
            Dim i As Integer = 0
            For i = 0 To lstOKPartNo.Items.Count - 1
                If lstOKPartNo.Items(i).ItemType = ListItemType.Item Or lstOKPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim chkOKPart As CheckBox = lstOKPartNo.Items(i).FindControl("chkOKPart")
                    Dim lblPartNo As Label = lstOKPartNo.Items(i).FindControl("lblPartNo")
                    If chkOKPart.Checked Then
                        Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                        drp.SWP_MARK = 1
                        drp.SWP_FLAG = 0
                        drp.SWP_ID = lblSwID.Text
                        drp.SWP_PARTNO = lblPartNo.Text
                        drp.SWP_LUAD = Session("_UserID")
                        drp.SWP_LUADNAME = Session("_UserName")
                        drp.SWP_LUSTMP = Date.Now
                        dtSWParts.Rows.Add(drp)
                    End If
                End If
            Next

            Dim oWarranty As New ctlWarranty
            oWarranty.SaveDelSWParts(dtSWParts)

            dtSWParts = oWarranty.QuerySWParts(lblSwID.Text, "", 1, "")
            lstOKPartNo.DataSource = dtSWParts
            lstOKPartNo.DataBind()

            dtSWParts = oWarranty.QuerySWParts(lblSwID.Text, "", 0, "")
            lstAllPartNo.DataSource = dtSWParts
            lstAllPartNo.DataBind()

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

    Protected Sub UI_cmdSavePart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSavePart.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim sSwID As String = lblSwID.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtSWSet As New WarrantyDTO.SWSETDataTable
            dtSWSet = oWarranty.QuerySWSet(sSwID, "", "", -1, "")
            If dtSWSet.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.SWSETRow = dtSWSet.Rows(0)

                dr.SW_ID = sSwID
                dr.SW_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
                dr.SW_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                dr.SW_GROUP = UI_txtProductGroup.Text.ToString().Trim()
                dr.SW_TYPE01 = Convert.ToInt32(Me.UI_cboWarrantyType.SelectedValue.ToString().Trim())
                dr.SW_TYPE02 = Convert.ToInt32(Me.UI_cboWarrantyTypeoth.SelectedValue.ToString().Trim())
                dr.SW_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
                dr.SW_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                If UI_cboWarrantyType.SelectedValue.ToString().Trim() = "1" Then
                    dr.SW_DESC = Me.UI_txtDescription.Text.ToString().Trim()
                Else
                    dr.SW_DESC = Me.txtDescription.Text.ToString().Trim()
                End If
                If txtExpDate.Text.Trim() <> "" Then
                    dr.SW_Expdate = txtExpDate.Text
                End If
                dr.SW_PRICE = Convert.ToDouble(UI_UnitPrice.Text)
                dr.SW_LUAD = Session("_UserID")
                dr.SW_LUADNAME = Session("_UserName")
                dr.SW_LUSTMP = Date.Now
                oWarranty.SaveEditSWSet(dtSWSet)
            End If

            Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
            Dim i As Integer = 0
            For i = 0 To lstOKPartNo.Items.Count - 1
                If lstOKPartNo.Items(i).ItemType = ListItemType.Item Or lstOKPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim txtYears As TextBox = lstOKPartNo.Items(i).FindControl("txtYears")
                    Dim lblPartNo As Label = lstOKPartNo.Items(i).FindControl("lblPartNo")

                    Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                    drp.SWP_ID = lblSwID.Text
                    drp.SWP_PARTNO = lblPartNo.Text
                    drp.SWP_YEAR = Convert.ToInt32(txtYears.Text.ToString().Trim())
                    drp.SWP_LUAD = Session("_UserID")
                    drp.SWP_LUADNAME = Session("_UserName")
                    drp.SWP_LUSTMP = Date.Now
                    dtSWParts.Rows.Add(drp)
                End If
            Next

            oWarranty.SaveEditSWParts(dtSWParts)

            dtSWParts = oWarranty.QuerySWParts(lblSwID.Text, "", 1, "")
            lstOKPartNo.DataSource = dtSWParts
            lstOKPartNo.DataBind()
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SpecialSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim sSwID As String = lblSwID.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtSWSet As New WarrantyDTO.SWSETDataTable
            dtSWSet = oWarranty.QuerySWSet(sSwID, "", "", -1, "")
            'Throw New Exception(dtSWSet.Rows.Count)     
            If dtSWSet.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.SWSETRow = dtSWSet.Rows(0)
                dr.SW_ID = sSwID
                dr.SW_NAME = Me.UI_txtWarrantyName.Text.ToString().Trim()
                dr.SW_COMPNO = UI_cboOperationCenter.SelectedValue.Trim()
                dr.SW_GROUP = UI_txtProductGroup.Text.ToString().Trim()
                dr.SW_TYPE01 = Convert.ToInt32(Me.UI_cboWarrantyType.SelectedValue.ToString().Trim())
                dr.SW_TYPE02 = Convert.ToInt32(Me.UI_cboWarrantyTypeoth.SelectedValue.ToString().Trim())
                dr.SW_EXTMM = Convert.ToInt32(Me.UI_ExtraMonths.Text.ToString().Trim())
                dr.SW_STDYY = Convert.ToInt32(Me.UI_Years.Text.ToString().Trim())
                If UI_cboWarrantyType.SelectedValue.ToString().Trim() = "1" Then
                    dr.SW_DESC = Me.UI_txtDescription.Text.ToString().Trim()
                Else
                    dr.SW_DESC = Me.txtDescription.Text.ToString().Trim()
                End If
                If txtExpDate.Text.Trim() <> "" Then
                    dr.SW_Expdate = txtExpDate.Text
                End If
                dr.SW_PRICE = Convert.ToDouble(UI_UnitPrice.Text)
                dr.SW_LUAD = Session("_UserID")
                dr.SW_LUADNAME = Session("_UserName")
                dr.SW_LUSTMP = Date.Now
                dr.SW_STATUS = 1
                oWarranty.SaveEditSWSet(dtSWSet)
            End If

            Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
            Dim i As Integer = 0
            For i = 0 To lstOKPartNo.Items.Count - 1
                If lstOKPartNo.Items(i).ItemType = ListItemType.Item Or lstOKPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim txtYears As TextBox = lstOKPartNo.Items(i).FindControl("txtYears")
                    Dim lblPartNo As Label = lstOKPartNo.Items(i).FindControl("lblPartNo")

                    Dim drp As WarrantyDTO.SWPARTSRow = dtSWParts.NewSWPARTSRow
                    drp.SWP_ID = lblSwID.Text
                    drp.SWP_PARTNO = lblPartNo.Text
                    drp.SWP_YEAR = Convert.ToInt32(txtYears.Text.ToString().Trim())
                    drp.SWP_LUAD = Session("_UserID")
                    drp.SWP_LUADNAME = Session("_UserName")
                    drp.SWP_LUSTMP = Date.Now
                    dtSWParts.Rows.Add(drp)
                End If
            Next
            oWarranty.SaveEditSWParts(dtSWParts)

            '140701 by cipherlab.fairy----(S)
            Dim FlowWS As New WorkflowService.WorkflowServiceService()
            Dim formOID As String = FlowWS.findFormOIDsOfProcess("SWSetting")
            Dim formXML As New XmlDocument()
            formXML.LoadXml(FlowWS.getFormFieldTemplate(formOID))
            Dim dr1 As WarrantyDTO.SWSETRow = dtSWSet.Rows(0)
            formXML.DocumentElement.SelectSingleNode("sw_id").InnerText = sSwID
            formXML.DocumentElement.SelectSingleNode("sw_name").InnerText = dr1.SW_NAME
            formXML.DocumentElement.SelectSingleNode("sw_group").InnerText = dr1.SW_GROUP
            'formXML.DocumentElement.SelectSingleNode("sw_type01").InnerText = dr1.SW_TYPE01
            'formXML.DocumentElement.SelectSingleNode("sw_type02").InnerText = dr1.SW_TYPE02
            formXML.DocumentElement.SelectSingleNode("sw_version").InnerText = dr1.SW_VERSION
            formXML.DocumentElement.SelectSingleNode("sw_extmm").InnerText = dr1.SW_EXTMM
            formXML.DocumentElement.SelectSingleNode("sw_stdyy").InnerText = dr1.SW_STDYY
            formXML.DocumentElement.SelectSingleNode("sw_desc").InnerText = dr1.SW_DESC
            formXML.DocumentElement.SelectSingleNode("sw_price").InnerText = dr1.SW_PRICE
            formXML.DocumentElement.SelectSingleNode("sw_expdate").InnerText = dr1.SW_Expdate
            formXML.DocumentElement.SelectSingleNode("sw_compno").InnerText = dr1.SW_COMPNO
            'Throw New Exception(FlowWS.getFormFieldTemplate(formOID))

            '撈單身資料
            Dim dtSWParts1 As New WarrantyDTO.SWPARTSDataTable
            dtSWParts1 = oWarranty.QuerySWParts(sSwID, "", 1, "")
            '設定datagrid
            Dim xnl As XmlNode
            xnl = formXML.DocumentElement.SelectSingleNode("sw_grid/records")
            Dim xn As XmlNode
            xn = xnl.FirstChild.Clone()
            xnl.RemoveAll()
            xn.Attributes("id").InnerText = "sw_grid_0"
            xn.SelectSingleNode("//item[@id='SWP_PARTNO']").InnerText = dtSWParts1.Rows(0)("SWP_PARTNO").ToString
            xn.SelectSingleNode("//item[@id='SWP_DESC']").InnerText = dtSWParts1.Rows(0)("SWP_DESC").ToString
            xn.SelectSingleNode("//item[@id='SWP_YEAR']").InnerText = dtSWParts1.Rows(0)("SWP_YEAR").ToString
            xn.SelectSingleNode("//item[@id='SWP_FLAG']").InnerText = dtSWParts1.Rows(0)("SWP_FLAG").ToString
            xnl.AppendChild(xn)
            Throw New Exception(dtSWParts1.Rows.Count)
            'Dim i As Integer = 0
            For i = 1 To dtSWParts1.Rows.Count - 1 Step 1
                xn = xnl.FirstChild.Clone()
                xn.Attributes("id").InnerText = "SW_grid" + i.ToString
                xn.SelectSingleNode("//item[@id='SWP_PARTNO']").InnerText = dtSWParts1.Rows(0)("SWP_PARTNO").ToString
                xn.SelectSingleNode("//item[@id='SWP_DESC']").InnerText = dtSWParts1.Rows(0)("SWP_DESC").ToString
                xn.SelectSingleNode("//item[@id='SWP_YEAR']").InnerText = dtSWParts1.Rows(0)("SWP_YEAR").ToString
                xn.SelectSingleNode("//item[@id='SWP_FLAG']").InnerText = dtSWParts1.Rows(0)("SWP_FLAG").ToString
                xnl.AppendChild(xn)
            Next
            'Throw New Exception(formXML.InnerXml)
            Dim strAD As String
            strAD = "0231"
            'strAD = dr1.WAR_AD
            Dim strDept As String = "30840"
            Dim processID As String
            processID = FlowWS.invokeProcess("SWSetting", strAD, strDept, formOID, formXML.InnerXml, "SWSetting")
            'Dim sMessage As String = processID
            '140701 by cipherlab.fairy----(E)

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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SpecialSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub chkPartAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPartAll.CheckedChanged
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim i As Integer = 0
            Dim chkPartAll As CheckBox = sender
            For i = 0 To lstAllPartNo.Items.Count - 1
                If lstAllPartNo.Items(i).ItemType = ListItemType.Item Or lstAllPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim chkExcelPart As CheckBox = lstAllPartNo.Items(i).FindControl("chkExcelPart")
                    chkExcelPart.Checked = chkPartAll.Checked
                End If
            Next
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

    Protected Sub chkOKAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOKAll.CheckedChanged
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim i As Integer = 0
            Dim chkOKAll As CheckBox = sender
            For i = 0 To lstOKPartNo.Items.Count - 1
                If lstOKPartNo.Items(i).ItemType = ListItemType.Item Or lstOKPartNo.Items(i).ItemType = ListItemType.AlternatingItem Then
                    Dim chkOKPart As CheckBox = lstOKPartNo.Items(i).FindControl("chkOKPart")
                    chkOKPart.Checked = chkOKAll.Checked
                End If
            Next
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

    Protected Sub UI_cmdSearchPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearchPart.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim oDSImport As New DataSet
            If Not ViewState("oDSImport") Is Nothing Then
                oDSImport = ViewState("oDSImport")
                oDSImport.Tables(0).DefaultView.RowFilter = "PartNo<>'' AND PartNo like '*" + txtQueryPartNo.Text.Trim() + "*' AND PartDesc like '*" + txtQueryPartDesc.Text.Trim() + "*'"
                oDSImport.Tables(0).DefaultView.Sort = "PartNo"
                lstAllPartNo.DataSource = oDSImport.Tables(0).DefaultView
                lstAllPartNo.DataBind()
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

    Protected Sub btnInvalid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvalid.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            If lblSwID.Text.ToString().Trim() <> "" Then
                Dim sWar_id As String = lblSwID.Text.ToString().Trim()
                Dim oWarranty As New ctlWarranty
                oWarranty.SaveInvalidSWSet(sWar_id, Session("_UserID"), Session("_UserName"), 3)
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SpecialSetting.aspx")
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
                oWarranty.SaveInvalidSWSet(sWar_id, Session("_UserID"), Session("_UserName"), 0)
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
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SpecialSetting.aspx")
            End If
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Warranty_SpecialSetting.aspx")
    End Sub
End Class

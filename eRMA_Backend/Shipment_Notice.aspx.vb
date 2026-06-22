Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Shipment_Notice
    Inherits System.Web.UI.Page

    Dim _Crypto As New SecurityCrypt.Crypto
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = "Shipment_" & oCommon.GetRandomizeNum() & ".pdf"

    Dim i As Integer = 0

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then

            Call setDefault()

            Session("_dtRMAShipping") = Nothing

            Me.ViewState("_eumCommand") = eumCommand.AddNew
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMASMID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMASMID")

                Me.UI_lblPreviousPage_RMASMID.Text = UI_lblPreviousPage_RMASMID.Text.ToString().Trim()
                If Me.UI_lblPreviousPage_RMASMID.Text.Trim() <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE

                    Call QueryData_RMAShipping()        'show表頭
                    Call QueryData_RMAShippingDetail()  'show表身
                End If
            End If

        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Me.UI_lblNoticeText.Text = _oLanguage.getText("RMA", "218", ctlLanguage.eumType.Tag)
        Me.UI_lblDateText.Text = Date.Now.ToShortDateString()

        'Dim sClientID As String = "ctl00_ContentPlaceHolder_UI_dvSales_ctl01_UI_CheckGroup"
        'sClientID = sClientID & "," & Me.UI_cmdSearch.ClientID '& "," & Me.UI_cmdQuoting.ClientID & "," & Me.UI_cmdRepairing.ClientID
        'Me.ucProgressStatus.NotpostBackElement = sClientID

        Call setValidationMessage(Me.rfvtxtCustomer)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "147", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "148", ctlLanguage.eumType.Tag)
        Me.UI_lblNotice.Text = _oLanguage.getText("RMA", "144", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingOrders.Text = _oLanguage.getText("RMA", "149", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingNumber.Text = _oLanguage.getText("RMA", "150", ctlLanguage.eumType.Tag)
        Me.UI_lblMemo.Text = _oLanguage.getText("RMA", "151", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "152", ctlLanguage.eumType.Tag)
        Me.UI_opgShippingOrders.Items(0).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_opgShippingOrders.Items(1).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)

        Me.UI_cmdCustomerSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdShippingSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "044", ctlLanguage.eumType.Command)
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfvtxtCustomer".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "181", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_SaleLaborCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "074", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_SaleMaterialCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "075", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_SaleAmount".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "135", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rv_SaleLaborCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "076", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rv_SaleMaterialCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "077", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rv_SaleAmount".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "136", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    ''' <summary>
    ''' 顯示單頭資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShipping()
        Dim oShipping As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.ShipmentDataTable
        Dim sShipmentID As String = Me.UI_lblPreviousPage_RMASMID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByRMA_Shipping("", sShipmentID, Session("_UserID").ToString(), "", "", "", "", "", "")

        If dtShipping.Rows.Count > 0 Then
            Dim dr As RmaDTO.ShipmentRow = dtShipping.Rows(0)

            Me.UI_lblNoticeText.Text = dr.RMASM_PACKINGNO.ToString().Trim()
            Me.UI_lblDateText.Text = Convert.ToDateTime(dr.RMASM_CSTMP.ToString.Trim()).ToShortDateString()
            Me.UI_txtCustomer.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblCustomerID.Text = dr.RMASM_CUNO.ToString().Trim()
            Me.UI_lblCurrencyCode.Text = dr.RMARSM_CURRENCYCODE.ToString().Trim()
            Me.UI_lblCurrencyRate.Text = dr.RMARSM_CURRENCYRATE.ToString().Trim()
            Me.UI_opgShippingOrders.SelectedValue = dr.RMASM_ISSHIP.ToString().Trim()

            If dr.IsRMASM_SHIPNONull = False Then Me.UI_txtShippingNumber.Text = dr.RMASM_SHIPNO.ToString().Trim()
            If dr.IsRMASM_SHIPMEMONull = False Then Me.UI_txtMemo.Text = dr.RMASM_SHIPMEMO.ToString().Trim()

            Me.UI_lblIsSumbit.Text = dr.RMASM_ISSUBMIT.ToString().Trim()
            If dr.RMASM_ISSUBMIT = 1 Then
                Me.UI_cmdSave.Visible = False
            End If
        End If

        Me.UI_cmdCustomerSearch.Enabled = False
        Me.UI_txtCustomer.Enabled = False
    End Sub

    ''' <summary>
    ''' 顯示單身資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShippingDetail()
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
        Dim sShipmentID As String = Me.UI_lblPreviousPage_RMASMID.Text.ToString().Trim()

        dtShipping = oShipment.QueryByRMA_ShipmentDetail(sShipmentID)

        Call Shipping_DataBind(dtShipping)
    End Sub

    Private Sub Shipping_DataBind(ByVal dtShipping As RmaDTO.Shipment_DetailDataTable)
        Session("_dtRMAShipping") = dtShipping
        Me.UI_dvShipping.ShowFooter = False
        Me.UI_cmdSave.Enabled = False
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdPrint.Enabled = False
        Me.UI_cmdCustomerSearch.Enabled = True
        Me.UI_txtCustomer.Enabled = True

        Dim dvShipping As DataView = dtShipping.DefaultView()
        dvShipping.RowFilter = "RMASMD_oldMark='0' OR RMASMD_oldMark='1'"       '只秀新增及修改的資料

        If dvShipping.Count > 0 Then
            Me.UI_dvShipping.ShowFooter = True
            Me.UI_cmdSave.Enabled = True
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdPrint.Enabled = True
            Me.UI_cmdCustomerSearch.Enabled = False
            Me.UI_txtCustomer.Enabled = False
        End If

        '修改資料時可能刪除全部資料
        'If Convert.ToInt16(Me.ViewState("_eumCommand")) = eumCommand.UPDATE Then
        '    Me.UI_cmdSave.Enabled = True
        '    Me.UI_cmdSubmit.Enabled = True
        '    Me.UI_cmdPrint.Enabled = True
        'End If

        Me.UI_dvShipping.DataSource = dtShipping.DefaultView()
        Me.UI_dvShipping.DataBind()
    End Sub

    Protected Sub UI_dvShipping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvShipping.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHNo As Label = e.Item.FindControl("lblHNo")
            Dim lblHRMA As Label = e.Item.FindControl("lblHRMA")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")
            Dim lblHModel As Label = e.Item.FindControl("lblHModel")
            Dim lblHLabor As Label = e.Item.FindControl("lblHLabor")
            Dim lblHMaterial As Label = e.Item.FindControl("lblHMaterial")
            Dim lblHAmount As Label = e.Item.FindControl("lblHAmount")
            Dim lblHDelete As Label = e.Item.FindControl("lblHDelete")
            Dim lblHDetail As Label = e.Item.FindControl("lblHDetail")

            lblHNo.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            lblHRMA.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            lblHModel.Text = _oLanguage.getText("RMA", "020", ctlLanguage.eumType.Tag)
            lblHLabor.Text = _oLanguage.getText("RMA", "178", ctlLanguage.eumType.Tag)
            lblHMaterial.Text = _oLanguage.getText("RMA", "059", ctlLanguage.eumType.Tag)
            lblHAmount.Text = _oLanguage.getText("RMA", "180", ctlLanguage.eumType.Tag)

            lblHDelete.Text = _oLanguage.getText("RMA", "017", ctlLanguage.eumType.Tag)
            lblHDetail.Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

            Me.UI_lblLaborTotal.Text = "0"
            Me.UI_lblMaterialTotal.Text = "0"
            Me.UI_lblQuotedTotal.Text = "0"
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblLabor As Label = e.Item.FindControl("lblLabor")
            Dim lblQuoted As Label = e.Item.FindControl("lblQuoted")
            lblLabor.Text = _oLanguage.getText("RMA", "094", ctlLanguage.eumType.Tag)
            lblQuoted.Text = _oLanguage.getText("RMA", "179", ctlLanguage.eumType.Tag)

            '==========================================================================================================================================================
            '檢核機制
            '==========================================================================================================================================================
            Dim txtSaleLabor As TextBox = e.Item.FindControl("txtSaleLabor")
            Dim txtSaleMaterial As TextBox = e.Item.FindControl("txtSaleMaterial")
            Dim txtSaleQuoted As TextBox = e.Item.FindControl("txtSaleQuoted")

            Dim rfv_SaleLaborCost As RequiredFieldValidator = e.Item.FindControl("rfv_SaleLaborCost")
            Dim rfv_SaleMaterialCost As RequiredFieldValidator = e.Item.FindControl("rfv_SaleMaterialCost")
            Dim rfv_SaleAmount As RequiredFieldValidator = e.Item.FindControl("rfv_SaleAmount")
            Dim rv_SaleLaborCost As RangeValidator = e.Item.FindControl("rv_SaleLaborCost")
            Dim rv_SaleMaterialCost As RangeValidator = e.Item.FindControl("rv_SaleMaterialCost")
            Dim rv_SaleAmount As RangeValidator = e.Item.FindControl("rv_SaleAmount")

            rfv_SaleLaborCost.ControlToValidate = txtSaleLabor.ID
            rfv_SaleMaterialCost.ControlToValidate = txtSaleMaterial.ID
            rfv_SaleAmount.ControlToValidate = txtSaleQuoted.ID
            rv_SaleLaborCost.ControlToValidate = txtSaleLabor.ID
            rv_SaleMaterialCost.ControlToValidate = txtSaleMaterial.ID
            rv_SaleAmount.ControlToValidate = txtSaleQuoted.ID

            Call setValidationMessage(rfv_SaleLaborCost)
            Call setValidationMessage(rfv_SaleMaterialCost)
            Call setValidationMessage(rfv_SaleAmount)
            Call setValidationMessage(rv_SaleLaborCost)
            Call setValidationMessage(rv_SaleMaterialCost)
            Call setValidationMessage(rv_SaleAmount)

            '==========================================================================================================================================================
            '表尾的資料(總金額)
            '==========================================================================================================================================================
            If Me.UI_lblLaborTotal.Text.ToString().Trim() <> "" And txtSaleLabor.Text.ToString().Trim() <> "" Then
                Me.UI_lblLaborTotal.Text = Convert.ToDouble(Me.UI_lblLaborTotal.Text.ToString().Trim()) + Convert.ToDouble(txtSaleLabor.Text.ToString().Trim())
            End If
            If Me.UI_lblMaterialTotal.Text.ToString().Trim() <> "" And txtSaleMaterial.Text.ToString().Trim() <> "" Then
                Me.UI_lblMaterialTotal.Text = Convert.ToDouble(Me.UI_lblMaterialTotal.Text.ToString().Trim()) + Convert.ToDouble(txtSaleMaterial.Text.ToString().Trim())
            End If
            If Me.UI_lblQuotedTotal.Text.ToString().Trim() <> "" And txtSaleQuoted.Text.ToString().Trim() <> "" Then
                Me.UI_lblQuotedTotal.Text = Convert.ToDouble(Me.UI_lblQuotedTotal.Text.ToString().Trim()) + Convert.ToDouble(txtSaleQuoted.Text.ToString().Trim())
            End If

            txtSaleLabor.Attributes.Add("onkeyup", "calTotalAMT()")
            txtSaleMaterial.Attributes.Add("onkeyup", "calTotalAMT()")

        End If

        If e.Item.ItemType = ListItemType.Footer Then
            Dim lblFLabor As Label = e.Item.FindControl("lblFLabor")
            Dim lblFMaterial As Label = e.Item.FindControl("lblFMaterial")
            Dim lblFAmount As Label = e.Item.FindControl("lblFAmount")
            Dim lblFLaborTotal As Label = e.Item.FindControl("lblFLaborTotal")
            Dim lblFMaterialTotal As Label = e.Item.FindControl("lblFMaterialTotal")
            Dim lblFQuotedTotal As Label = e.Item.FindControl("lblFQuotedTotal")
            Dim lblFCurrnecyCode As Label = e.Item.FindControl("lblFCurrnecyCode")

            lblFLabor.Text = _oLanguage.getText("RMA", "178", ctlLanguage.eumType.Tag)
            lblFMaterial.Text = _oLanguage.getText("RMA", "059", ctlLanguage.eumType.Tag)
            lblFAmount.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)

            lblFLaborTotal.Text = Convert.ToDecimal(Me.UI_lblLaborTotal.Text.ToString().Trim()).ToString("N")
            lblFMaterialTotal.Text = Convert.ToDecimal(Me.UI_lblMaterialTotal.Text.ToString().Trim()).ToString("N")
            lblFQuotedTotal.Text = Convert.ToDecimal(Me.UI_lblQuotedTotal.Text.ToString().Trim()).ToString("N")
            lblFCurrnecyCode.Text = Me.UI_lblCurrencyCode.Text.ToString().Trim()
        End If

    End Sub

    Protected Sub UI_dvShipping_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles UI_dvShipping.ItemCommand
        If e.CommandName = "cmdDel" Then
            Dim lblRMADID As Label = e.Item.FindControl("lblRMADID")

            Call Delete(lblRMADID.Text.ToString())
        End If

        If e.CommandName = "cmdDetail" Then
            Dim lblRMANO As Label = e.Item.FindControl("lblRMANO")
            Dim lblRMADID As Label = e.Item.FindControl("lblRMADID")
            Me.ucRepairDetail.show(lblRMADID.Text.ToString().Trim(), lblRMANO.Text.Trim(), True)
        End If

    End Sub

    ''' <summary>
    ''' 品項刪除
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks></remarks>
    ''' Visible欄位狀態說明
    ''' 0:新增資料==>資料庫無此比資料
    ''' 1:修改資料==>資料庫有此比資料
    ''' 2:刪除資料==>要是資料狀態為0就直接刪除,資料狀態為1就改狀態為2
    ''' 9:虛擬資料==>作用是秀出畫面表頭
    Private Sub Delete(ByVal sID As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim dtShipping As RmaDTO.Shipment_DetailDataTable = Session("_dtRMAShipping")
        Dim dvShipping As DataView = dtShipping.DefaultView

        dvShipping.RowFilter = "RMASMD_RMADID='" & sID.ToString().Trim() & "'"
        If dvShipping.Count > 0 Then
            Dim sVisible As String = dvShipping.Item(0)("RMASMD_oldMark").ToString().Trim()
            If sVisible.Trim() = "1" Then
                dvShipping(0)("RMASMD_oldMark") = "2"
            Else
                dvShipping.Item(0).Delete()
            End If

        End If
        dvShipping.RowFilter = ""

        Call Shipping_DataBind(dtShipping)
    End Sub

    ''' <summary>
    ''' ShippingCustomer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdCustomerSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCustomerSearch.Click
        Me.ucCustomer_pick.show("", True)
    End Sub

    ''' <summary>
    ''' ShippingSerial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdShippingSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdShippingSearch.Click
        Dim sCuID As String = Me.UI_lblCustomerID.Text.Trim()
        Call keepDetail()
        Me.ucRMASerial_Pick.show(sCuID, True)
    End Sub

    ''' <summary>
    ''' 儲存 UI 單身資料到 DataTable
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub keepDetail()
        Dim dtShipping As New RmaDTO.Shipment_DetailDataTable

        If IsNothing(Session("_dtRMAShipping")) = False Then
            dtShipping = Session("_dtRMAShipping")
            Dim dvShipping As DataView = dtShipping.DefaultView

            For i = 0 To Me.UI_dvShipping.Items.Count - 1
                If Me.UI_dvShipping.Items(i).ItemType = ListItemType.AlternatingItem Or Me.UI_dvShipping.Items(i).ItemType = ListItemType.Item Then
                    Dim lblRMADID As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMADID")
                    Dim txtSaleLabor As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleLabor")
                    Dim txtSaleMaterial As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleMaterial")
                    Dim txtSaleQuoted As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleQuoted")

                    dvShipping.RowFilter = "RMASMD_RMADID='" & lblRMADID.Text.ToString().Trim() & "'"
                    If dvShipping.Count > 0 Then
                        dvShipping(0)("RMARSD_LABORCOST") = txtSaleLabor.Text.Trim()
                        dvShipping(0)("RMARSD_MATERIALCOST") = txtSaleMaterial.Text.Trim()
                        dvShipping(0)("RMARSD_QUOTE") = txtSaleQuoted.Text.Trim()
                    End If
                End If
            Next

            dvShipping.RowFilter = ""
            Session("_dtRMAShipping") = dtShipping
        End If
    End Sub

    ''' <summary>
    ''' Save
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call Save("Save")
    End Sub

    ''' <summary>
    ''' Submit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call Save("Submit")
    End Sub

    ''' <summary>
    ''' Print
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call Save("Print")
    End Sub

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="sType">sType:不為Save就需修改RMA狀態</param>
    ''' <remarks></remarks>
    Private Sub Save(ByVal sType As String)
        Dim blnFlag As Boolean = False
        Dim isPrint As Boolean = False
        Dim isSumbit As Boolean = False
        Dim isBossConfirm As Boolean = False
        Dim isSendMail As Boolean = True

        Dim sMessage As String = ""
        Dim RMASM_ID As String = ""
        Dim curDateTime As Date = Date.Now

        Dim oShipping As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.ShipmentDataTable
        Dim dtShippingDetail As New RmaDTO.Shipment_DetailDataTable
        Dim sKey As String = Me.UI_lblPreviousPage_RMASMID.Text.Trim()

        Try
            If sType.ToLower().Trim() = "Print".ToLower().Trim() Then
                isPrint = True
                RMASM_ID = Me.UI_lblPreviousPage_RMASMID.Text

                If Me.UI_lblIsSumbit.Text.Trim() <> "1" Then
                    sType = "Save"
                End If
            End If

            If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                isSumbit = True
            End If

            If sType.ToLower().Trim() = "Save".ToLower().Trim() Or sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                'ShippingDetail Table
                'isBossConfirm: = True -->檢核是否低於折扣金額, 是的話須要送主管審核
                dtShippingDetail = Save_ShippingDetail(isBossConfirm, curDateTime)
                'Shipping Table
                dtShipping = Save_Shipping(sKey, isSumbit, isBossConfirm, curDateTime)

                Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                    Case eumCommand.AddNew
                        RMASM_ID = oShipping.SaveByAddNew(sType, dtShipping, dtShippingDetail)

                    Case eumCommand.UPDATE
                        oShipping.SaveByEdit(sType, sKey, dtShipping, dtShippingDetail)
                        RMASM_ID = Me.UI_lblPreviousPage_RMASMID.Text
                End Select
            End If
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                If isSumbit = True Then
                    Me.ViewState("_AttachmentFile") = ""
                    Call getAttachmentFile(RMASM_ID)
                    Call SendMail_Notice(RMASM_ID, curDateTime)
                End If

                If isPrint = True Then
                    Response.Redirect("Shipment_Print.aspx?RMASM_ID=" & _Crypto.Encrypt(RMASM_ID.Trim, ""))
                Else
                    If isBossConfirm = True And isSumbit = True Then
                        isSendMail = SendMail(RMASM_ID)
                    End If

                    If isSendMail = True Then
                        Dim sMsg As String = ""
                        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                            Case eumCommand.AddNew
                                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)

                            Case eumCommand.UPDATE
                                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                        End Select
                        Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Shipment_List.aspx")
                    End If
                End If

            End If
        End Try
    End Sub

    Private Function Save_Shipping(ByVal sKey As String, ByVal isSumbit As Boolean, ByVal isBossConfirm As Boolean, ByVal curDateTime As Date) As RmaDTO.ShipmentDataTable
        Dim dtShipping As New RmaDTO.ShipmentDataTable
        Dim dr As RmaDTO.ShipmentRow = dtShipping.NewShipmentRow

        Dim oGuid As Guid = Guid.NewGuid
        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                dr.RMASM_ID = oGuid.ToString().Trim()               '系統自動產生唯一識別碼

            Case eumCommand.UPDATE
                dr.RMASM_ID = sKey.Trim()                           '系統自動產生唯一識別碼

        End Select

        Try

            dr.RMASM_PACKINGNO = oGuid.ToString().Trim()        'RMA Shipment 編號
            dr.RMASM_CUNO = Me.UI_lblCustomerID.Text.ToString().Trim()
            dr.RMASM_ISSHIP = Convert.ToInt32(Me.UI_opgShippingOrders.SelectedValue.ToString().Trim())
            dr.RMASM_SHIPNO = Me.UI_txtShippingNumber.Text.ToString().Trim()
            dr.RMASM_SHIPMEMO = Me.UI_txtMemo.Text.ToString().Trim()

            dr.RMASM_AD = Session("_UserID")
            dr.RMASM_ADNAME = Session("_UserName")
            dr.RMASM_CSTMP = curDateTime
            dr.RMASM_LUAD = Session("_UserID")
            dr.RMASM_LUADNAME = Session("_UserName")
            dr.RMASM_LUSTMP = curDateTime

            dr.RMARSM_LABORCOST = Convert.ToDecimal(Me.UI_lblLaborTotal.Text.ToString().Trim())
            dr.RMARSM_MATERIALCOST = Convert.ToDecimal(Me.UI_lblMaterialTotal.Text.ToString().Trim())
            dr.RMARSM_QUOTE = Convert.ToDecimal(Me.UI_lblQuotedTotal.Text.ToString().Trim())
            dr.RMARSM_CURRENCYCODE = Me.UI_lblCurrencyCode.Text.ToString().Trim()
            dr.RMARSM_CURRENCYRATE = Convert.ToDecimal(Me.UI_lblCurrencyRate.Text.ToString.Trim())

            dr.RMASM_ISBOSSCONFIRM = 0                  '是否需主管審核:0.否, 1.是, 2.已審核
            If isBossConfirm = True Then
                dr.RMASM_ISBOSSCONFIRM = 1
            End If

            '是否已Submit: 0.否, 1.是 
            dr.RMASM_ISSUBMIT = 0
            If isSumbit = True Then
                dr.RMASM_ISSUBMIT = 1
            End If

            dtShipping.AddShipmentRow(dr)

        Catch ex As Exception
            Throw ex
        End Try

        Return dtShipping
    End Function

    Private Function Save_ShippingDetail(ByRef isBossConfirm As Boolean, ByVal curDateTime As Date) As RmaDTO.Shipment_DetailDataTable
        Dim i As Integer = 0
        Dim oRMA As New ctlRMA
        Dim dtShippingDetail As New RmaDTO.Shipment_DetailDataTable

        Try
            For i = 0 To Me.UI_dvShipping.Items.Count - 1
                Dim dr As RmaDTO.Shipment_DetailRow = dtShippingDetail.NewShipment_DetailRow

                Dim lblRMASMDRMASMID As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMASMDRMASMID")
                Dim lblRMASMDID As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMASMDID")
                Dim RMANO As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMANO")
                Dim RMADID As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMADID")
                Dim CurrncyCode As Label = Me.UI_dvShipping.Items(i).FindControl("lblCurrncyCode")
                Dim CurrncyRate As Label = Me.UI_dvShipping.Items(i).FindControl("lblCurrncyRate")
                Dim SerialNo As Label = Me.UI_dvShipping.Items(i).FindControl("lblSerialNo")
                Dim ModelNo As Label = Me.UI_dvShipping.Items(i).FindControl("lblModelNo")
                Dim SaleLabor As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleLabor")
                Dim SaleMaterial As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleMaterial")
                Dim SaleQuoted As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtSaleQuoted")
                Dim Mark As Label = Me.UI_dvShipping.Items(i).FindControl("lblMark")

                Dim oGuid As Guid = Guid.NewGuid

                dr.RMASMD_RMASMID = lblRMASMDRMASMID.Text.ToString().Trim()
                dr.RMASMD_ID = lblRMASMDID.Text.ToString().Trim()
                dr.RMASMD_RMANO = RMANO.Text.ToString().Trim()
                dr.RMASMD_RMADID = RMADID.Text.ToString().Trim()
                dr.RMASMD_MODELNO = ModelNo.Text.ToString().Trim()
                dr.RMASMD_SERIALNO = SerialNo.Text.ToString().Trim()
                'dr.RMASMD_PARTNO = ""

                dr.RMARSD_LABORCOST = Convert.ToDouble(SaleLabor.Text.ToString().Trim())
                dr.RMARSD_MATERIALCOST = Convert.ToDouble(SaleMaterial.Text.ToString().Trim())
                dr.RMARSD_QUOTE = Convert.ToDouble(SaleQuoted.Text.ToString().Trim())

                dr.RMARSD_CURRENCYCODE = Me.UI_lblCurrencyCode.Text.ToString().Trim()
                dr.RMARSD_CURRENCYRATE = Convert.ToDecimal(Me.UI_lblCurrencyRate.Text.ToString().Trim())

                dr.RMASMD_LOWESTDISCOUNT = oRMA.getLowestDisCount(RMANO.Text.ToString().Trim())

                '1.最低折扣>0, 計算 維修後最低折扣金額
                '2.如果業務報價金額<=維修後最低折扣金額, 需要寄送mail 給主管審核
                If dr.RMASMD_LOWESTDISCOUNT > 0 And isBossConfirm = False Then
                    Dim txtRepairQuoted As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtRepairQuoted")
                    Dim iRepairQuoted As Double = Convert.ToDouble(txtRepairQuoted.Text.Trim())
                    Dim iDisCountAMT As Double = iRepairQuoted * (dr.RMASMD_LOWESTDISCOUNT * 0.1)  '維修後折扣金額
                    If dr.RMARSD_QUOTE <= iDisCountAMT Then
                        isBossConfirm = True
                    End If
                End If


                dr.RMASMD_AD = Session("_UserID")
                dr.RMASMD_ADNAME = Session("_UserName")
                dr.RMASMD_CSTMP = curDateTime
                dr.RMASMD_LUAD = Session("_UserID")
                dr.RMASMD_LUADNAME = Session("_UserName")
                dr.RMASMD_LUSTMP = curDateTime
                dr.RMASMD_oldMark = Mark.Text.ToString().Trim()

                dtShippingDetail.AddShipment_DetailRow(dr)
            Next

        Catch ex As Exception
            Throw ex

        End Try

        Return dtShippingDetail
    End Function

    ''' <summary>
    ''' 檢核是否低於折扣金額, 是的話須要送Mail給主管
    ''' </summary>
    ''' <param name="RMASM_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendMail(ByVal RMASM_ID As String) As Boolean
        Dim blnFlag As Boolean = False

        Dim oAdmin As New ctlAdmin

        Dim oMail As New ctlMail

        Try
            RMASM_ID = _Crypto.Encrypt(RMASM_ID, "")

            Dim sSubject As String = _oLanguage.getText("Mail", "006", ctlLanguage.eumType.Mail)
            Dim sBody As String = _oLanguage.getText("Mail", "007", ctlLanguage.eumType.Mail)
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            Dim Mail As String = oAdmin.getUpperSuperMaill(Session("_UserID").ToString().Trim())
            If Mail <> "" Then
                sSubject = sSubject.Replace("[$Customer's Name$]", Me.UI_txtCustomer.Text)
                sSubject = sSubject.Replace("[$Sales Name(Submit person)$]", Session("_UserName").ToString().Trim())
                sSubject = sSubject.Replace("[$Quoted submit date$]", Date.Now.ToString())

                sBody = sBody.Replace("[$Customer Name$]", Me.UI_txtCustomer.Text)
                sBody = sBody.Replace("[$Sales Name$]", Session("_UserName").ToString().Trim())
                sBody = sBody.Replace("[$Sales Confirm Date$]", Date.Now.ToString())
                sBody = sBody.Replace("[$Supervisor login url$]", _WEBURL & "BossDefault.aspx?ID=" & RMASM_ID)
                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Mail = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody, Mail, _MailCC)
            End If


        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 業務確認出貨通知--寄送Mail(對象:出貨人員)
    ''' </summary>
    ''' <param name="RMASM_ID"></param>
    ''' <param name="curDateTime"></param>
    ''' <remarks></remarks>
    Private Sub SendMail_Notice(ByVal RMASM_ID As String, ByVal curDateTime As Date)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oMail As New ctlMail

        Try
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
            Dim SalesName As String = Session("_UserName").ToString().Trim()

            Dim oShipment As New ctlRMA.Shipment
            Dim NoticeNo As String = oShipment.getNoticeNo(RMASM_ID.Trim())



            '==============================================================================================================================
            '寄送Mail(對象:出貨人員)
            '==============================================================================================================================
            Dim Shipment_EMAIL As String = ""                   '出貨人員 EMail
            Dim Shipment_Name As String = ""                    '出貨人員

            Dim arrShipping As ArrayList = oShipment.getShippingMail(RMASM_ID.Trim())
            For i = 0 To arrShipping.Count - 1
                Dim arrList() As String = arrShipping(i)
                If Shipment_Name.Trim() <> "" Then
                    Shipment_Name = Shipment_Name & ","
                End If
                If Shipment_EMAIL.Trim() <> "" Then
                    Shipment_EMAIL = Shipment_EMAIL & ","
                End If

                Shipment_Name = Shipment_Name & arrList(0).Trim()
                Shipment_EMAIL = Shipment_EMAIL & arrList(1).Trim()
            Next

            If Shipment_EMAIL.Trim() <> "" Then
                Dim sSubject_Repaire As String = _oLanguage.getText("Mail", "015", ctlLanguage.eumType.Mail)
                Dim sBody_Repaire As String = _oLanguage.getText("Mail", "016", ctlLanguage.eumType.Mail)

                sSubject_Repaire = sSubject_Repaire.Replace("[$Sales Name$]", SalesName.Trim())
                sSubject_Repaire = sSubject_Repaire.Replace("[$Customer Name$]", Me.UI_txtCustomer.Text.Trim())
                sSubject_Repaire = sSubject_Repaire.Replace("[$Notice No$]", NoticeNo)

                sBody_Repaire = sBody_Repaire.Replace("[$Repaire Person name$]", Shipment_Name)
                sBody_Repaire = sBody_Repaire.Replace("[$Notice No.$]", NoticeNo.Trim())
                sBody_Repaire = sBody_Repaire.Replace("[$Sales Name$]", SalesName.Trim())
                sBody_Repaire = sBody_Repaire.Replace("[$submit date$]", curDateTime)

                sBody_Repaire = sBody_Repaire.Replace("[$Email URL$]", sEmailURL)

                Dim oAttachmentFile As New Collection
                oAttachmentFile.Add(Me.ViewState("_AttachmentFile").ToString())

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Shipment_EMAIL = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject_Repaire, sBody_Repaire, Shipment_EMAIL.Trim(), _MailCC, oAttachmentFile)
                If blnFlag = False Then
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim sMsg As String = ex.Message

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 業務確認出貨通知--寄送Mail(對象:申請人(顧客))-->已不用
    ''' 業務確認出貨通知--寄送Mail(對象:維修人員)-->已不用
    ''' </summary>
    ''' <param name="RMASM_ID"></param>
    ''' <remarks></remarks>
    Private Sub SendMail_Notice_已不用(ByVal RMASM_ID As String, ByVal curDateTime As Date)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim Applicant_Mail As String = ""
        Dim RMADetail As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim oRequested As New ctlRMA.Requested
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oMail As New ctlMail

        Try
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            Dim SalesName As String = Session("_UserName").ToString().Trim()
            '==============================================================================================================================
            '寄送Mail(對象:申請人(顧客))
            '==============================================================================================================================
            'Dim CU_NO As String = Me.UI_lblCustomerID.Text.ToString().Trim()        '客戶編號
            'Dim CU_EMAIL As String = oCustomer.getMail(CU_NO)

            Dim RMAD_ID As String = ""
            For i = 0 To Me.UI_dvShipping.Items.Count - 1
                Dim RMADID As Label = Me.UI_dvShipping.Items(i).FindControl("lblRMADID")

                If RMAD_ID.Trim() <> "" Then
                    RMAD_ID = RMAD_ID & ","
                End If
                RMAD_ID = RMAD_ID & RMADID.Text.Trim()
            Next

            '取得要寄送的對象EMail Applicant 及 品項內容
            Dim arrRMADetail As ArrayList = oRequested.getApplicantMail(RMAD_ID.Trim())
            For i = 0 To arrRMADetail.Count - 1
                Dim arrList() As String = arrRMADetail(i)

                If Applicant_Mail.ToLower().IndexOf(arrList(2).Trim().ToLower()) = -1 Then
                    If Applicant_Mail.Trim <> "" Then
                        Applicant_Mail = Applicant_Mail & ","
                    End If
                    Applicant_Mail = Applicant_Mail & arrList(2).Trim()
                End If


                If RMADetail.Trim <> "" Then
                    RMADetail = RMADetail & vbCrLf
                End If
                RMADetail = RMADetail & (i + 1).ToString & ". " & arrList(3).Trim() & "/" & arrList(4).Trim()
            Next

            If Applicant_Mail.Trim() <> "" And RMADetail.Trim() <> "" Then
                Dim sSubject As String = _oLanguage.getText("Mail", "013", ctlLanguage.eumType.Mail)
                Dim sBody As String = _oLanguage.getText("Mail", "014", ctlLanguage.eumType.Mail)

                sBody = sBody.Replace("[$Customer User Name$]", Me.UI_txtCustomer.Text.Trim())
                sBody = sBody.Replace("[$RMA Detail$]", RMADetail.Trim())
                sBody = sBody.Replace("[$sales name$]", SalesName.Trim())
                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Applicant_Mail = ConfigurationManager.AppSettings("MailTo")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody, Applicant_Mail.Trim())
                If blnFlag = False Then
                    Exit Try
                End If
            End If



            '==============================================================================================================================
            '寄送Mail(對象:維修人員)
            '==============================================================================================================================
            Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
            Dim Repaire_Name As String = ""                    '維修人員
            Dim oShipment As New ctlRMA.Shipment

            Dim NoticeNo As String = oShipment.getNoticeNo(RMASM_ID.Trim())
            Dim arrRepaire As ArrayList = oShipment.getRepaireMail(RMASM_ID.Trim())

            For i = 0 To arrRepaire.Count - 1
                Dim arrList() As String = arrRepaire(i)
                If Repaire_Name.Trim() <> "" Then
                    Repaire_Name = Repaire_Name & ","
                End If
                If Repaire_EMAIL.Trim() <> "" Then
                    Repaire_EMAIL = Repaire_EMAIL & ","
                End If

                Repaire_Name = Repaire_Name & arrList(0).Trim()
                Repaire_EMAIL = Repaire_EMAIL & arrList(1).Trim()
            Next


            If Repaire_EMAIL.Trim() <> "" Then
                Dim sSubject_Repaire As String = _oLanguage.getText("Mail", "015", ctlLanguage.eumType.Mail)
                Dim sBody_Repaire As String = _oLanguage.getText("Mail", "016", ctlLanguage.eumType.Mail)

                sSubject_Repaire = sSubject_Repaire.Replace("[$Sales Name$]", SalesName.Trim())
                sSubject_Repaire = sSubject_Repaire.Replace("[$Customer Name$]", Me.UI_txtCustomer.Text.Trim())
                sSubject_Repaire = sSubject_Repaire.Replace("[$Notice No$]", NoticeNo)

                sBody_Repaire = sBody_Repaire.Replace("[$Repaire Person name$]", Repaire_Name)
                sBody_Repaire = sBody_Repaire.Replace("[$Notice No.$]", NoticeNo.Trim())
                sBody_Repaire = sBody_Repaire.Replace("[$Sales Name$]", SalesName.Trim())
                sBody_Repaire = sBody_Repaire.Replace("[$submit date$]", curDateTime)

                sBody_Repaire = sBody_Repaire.Replace("[$Email URL$]", sEmailURL)

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                End If
                blnFlag = oMail.SendMail(sSubject_Repaire, sBody_Repaire, Repaire_EMAIL.Trim())
                If blnFlag = False Then
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim sMsg As String = ex.Message

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

    End Sub

    Private Sub getAttachmentFile(ByVal RMASM_ID As String)
        Dim i As Integer = 0
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShippingReport As New RmaDTO.ShippingSaleRoportDataTable
        Dim dtReport As New RmaDTO.ShippingSaleRoportDataTable

        dtShippingReport = oShipment.QueryByReport(RMASM_ID)

        For i = 0 To dtShippingReport.Rows.Count - 1
            Dim dr As RmaDTO.ShippingSaleRoportRow = dtShippingReport.Rows(i)
            Dim drReport As RmaDTO.ShippingSaleRoportRow = dtReport.NewRow

            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()

            If dr.IsRMAD_WARRANTYNull = False Then
                drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()

            Else
                drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)

                'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                If dr.IsRMAD_ISWARRANTYNull = False Then
                    Select Case dr.RMAD_ISWARRANTY
                        Case 0
                            drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                        Case 1
                            drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                    End Select
                End If
            End If



            If dr.IsRMAD_CSTMPNull = False Then drReport.RMAD_CSTMP = Convert.ToDateTime(dr.RMAD_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsRMASM_CSTMPNull = False Then drReport.RMASM_CSTMP = Convert.ToDateTime(dr.RMASM_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsRMASMD_RMANONull = False Then drReport.RMASMD_RMANO = dr.RMASMD_RMANO.ToString().Trim()
            If dr.IsRMASMD_SERIALNONull = False Then drReport.RMASMD_SERIALNO = dr.RMASMD_SERIALNO.ToString().Trim()
            If dr.IsRMASMD_MODELNONull = False Then drReport.RMASMD_MODELNO = dr.RMASMD_MODELNO.ToString().Trim()

            drReport.SeqID = i + 1

            dtReport.AddShippingSaleRoportRow(drReport)
        Next

        Call Print(dtReport)
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.ShippingSaleRoportDataTable)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptShipment.rpt"))
        ReportDoc.SetDataSource(oReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

    'Public Sub ExportSetup()
    '    If Not System.IO.Directory.Exists(_Reoprt_FilePath) Then
    '        System.IO.Directory.CreateDirectory(_Reoprt_FilePath)
    '    End If

    '    myDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    myExportOptions = ReportDoc.ExportOptions
    '    ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
    'End Sub

    'Public Sub ConfigureExportToPdf()
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF

    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = Me.ViewState("_AttachmentFile").ToString().Trim()
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    'End Sub

End Class

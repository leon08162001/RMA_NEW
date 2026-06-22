Imports System.Data
Imports System.Data.OracleClient
Imports System.Web.Script.Services
Imports System.Web.Services
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports Newtonsoft.Json
Imports RMA_Common

Partial Class ShippingNotice
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")
    Dim _FinanceEmail As String = ConfigurationSettings.AppSettings("FinanceEmail")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

    Shared conNumberWordLess20 As String() = {"", "ONE", "TWO", "THREE", "FOUR", "FIVE",
     "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
     "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
     "EIGHTEEN", "NINETEEN"}

    Shared conNumberWordTen As String() = {"", "", "TWENTY", "THIRTY", "FORTY", "FIFTY",
     "SIXTY", "SEVENTY", "EIGHTY", "NINETY"}

    Enum eumCommand As Integer
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.UI_lblPreviousPage_RMASHID.Text = ""
            Me.UI_lblPreviousPage_SHIPPINGNO.Text = ""
            Me.UI_lblPreviousPage_RMANO.Text = ""

            Session("_dtShippingDetail") = Nothing
            Call chkFlowCase01()
            Call setDefault()

            Dim hsSelectID As New Hashtable
            Me.ViewState("hsSelectID") = hsSelectID
            Dim hDetail As New Hashtable
            Me.ViewState("hDetail") = hDetail

            Me.ViewState("_eumCommand") = eumCommand.AddNew

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMASHID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMASHID")

                Me.UI_lblPreviousPage_RMASHID.Text = UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

                Call QueryShippingCustomer()

                QueryShippingCustomerEU()
                QueryShippingCustomerDetail()

                '如果修改的話，需要帶默認客戶，Wait....
                If Me.UI_lblPreviousPage_RMASHID.Text.Trim() <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE

                    Call QueryData_RMAShipping()        'show表頭
                    Call QueryData_RMAShippingDetail()  'show表身
                End If
                Call QueryDataSerial(0)
            End If

            If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
                Me.UI_cmdSave.Enabled = False
                Me.UI_cmdSubmit.Enabled = False
                Me.UI_cmdSave.Attributes.Add("onclick", "return chkTrackingNo();")
                Me.UI_cmdSubmit.Attributes.Add("onclick", "return chkTrackingNo();")

                'Me.UI_cmdSave.Attributes.Add("disabled", "true")
                'Me.UI_cmdSubmit.Attributes.Add("disabled", "true")
            End If


            Call setFlowCase01()
        End If

        Dim dtShippingDetail As New RmaDTO.tmpShipping_DetailDataTable
        Dim curDateTime As Date = Date.Now
        If IsNothing(Session("_dtShippingDetail")) = False Then
            dtShippingDetail = Session("_dtShippingDetail")
        End If

        If Session("_UserID").ToString.ToUpper = "ADMIN" Or Session("_UserID").ToString.ToUpper = "P0001" Then
            Button1.Visible = True
            Button2.Visible = True
        Else
            Button1.Visible = False
            Button2.Visible = True
        End If

        'Call SendMail_Notice(Session("_dtShippingDetail"), curDateTime, 'ARBA-160100047')
        'Call Print_Inovice("SHP-2016010097", "CLHQ", "ARBA-160100047")
        'Call Print_AD("SHP-2016010097", "CLHQ", "ARBA-160100047")
        'Call SendMailNoticeBySales("ARMA-2015030071", "SHP-2016010097", "ARBA-160100047")

    End Sub
#End Region

    Private Sub chkFlowCase01()
        Dim i As Integer = 0

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "01"
                Exit For
            End If
        Next

    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01()

        If Me.UI_flowCase.Text = "01" Then
            Me.uiTR_ShippingOrders.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Dim sClientID As String = Me.UI_cboFrom.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID

        Me.UI_lblShippingText.Text = _oLanguage.getText("RMA", "218", ctlLanguage.eumType.Tag)
        Me.UI_lblDateText.Text = Date.Now.ToShortDateString()

        'UI_cboFrom-->維修點Session("_RepairCenter")
        Dim TagText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(Session("_RepairCenter"), Me.UI_cboFrom, TagText, False)
        Call setCompany(Me.UI_cboFrom.SelectedValue.Trim())

        Call setValidationMessage(Me.rfv_txtTracking)
        Call setValidationMessage(Me.rfv_txtCustomer)
        Call setValidationMessage(Me.rfv_txtCustomerAdd)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "154", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "155", ctlLanguage.eumType.Tag)
        Me.UI_lblShipping.Text = _oLanguage.getText("RMA", "156", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        Me.UI_lblPacking.Text = _oLanguage.getText("RMA", "157", ctlLanguage.eumType.Tag)
        Me.UI_lblPackingEU.Text = _oLanguage.getText("Transfer", "038", ctlLanguage.eumType.Word)
        'Me.UI_lblShipped.Text = _oLanguage.getText("RMA", "158", ctlLanguage.eumType.Tag)
        Me.UI_lblFrom.Text = _oLanguage.getText("RMA", "159", ctlLanguage.eumType.Tag)
        Me.UI_lblTo.Text = _oLanguage.getText("RMA", "160", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblExpress.Text = _oLanguage.getText("RMA", "161", ctlLanguage.eumType.Tag)
        Me.UI_lblTracking.Text = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)
        Me.UI_lblMemo.Text = _oLanguage.getText("RMA", "151", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "162", ctlLanguage.eumType.Tag)
        Me.UI_lblAddShippingTittle.Text = _oLanguage.getText("RMA", "162", ctlLanguage.eumType.Tag)

        Me.UI_lblShippingOrders.Text = _oLanguage.getText("RMA", "149", ctlLanguage.eumType.Tag)
        Me.UI_opgShippingOrders.Items(0).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_opgShippingOrders.Items(1).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingNumber.Text = _oLanguage.getText("RMA", "150", ctlLanguage.eumType.Tag)

        'Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "010", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "044", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfv_txtPacking".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "200", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfv_txtTracking".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "201", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
                lblMsgTracking.Text = sErrorMessage.ToString().Trim()

            Case "rfv_txtCustomer".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "181", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
            Case "rfv_txtCustomerAdd".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "181", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    ''' <summary>
    ''' 查詢客戶下拉選單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryShippingCustomerEU()
        'Dim CompNo As String = Me.UI_cboFrom.SelectedValue.ToString().Trim()
        Dim oShipment As New ctlRMA.Shipment

        'Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
        Dim dtCustomer As New DataTable
        Dim i As Integer = 0

        'dtCustomer = oShipping.QueryByShipping_Customer(CompNo, "")
        'dtCustomer = oShipment.QueryByShipment_Customer(Session("_UserID").ToString(), "")
        dtCustomer = oShipment.QueryByShipment_CustomerEU(Me.cboCustomer.SelectedValue.Trim())
        Me.ViewState("CustomerEU") = dtCustomer

        Dim dvCustomer As DataView = dtCustomer.DefaultView

        Me.cboCustomerEU.Items.Clear()
        Dim oListItem = New ListItem
        oListItem.Text = ""
        oListItem.Value = ""
        Me.cboCustomerEU.Items.Add(oListItem)

        For i = 0 To dvCustomer.Count - 1
            Dim oListItem1 = New ListItem
            oListItem1.Text = dvCustomer(i)("CU_NAME").ToString().Trim()
            oListItem1.Value = dvCustomer(i)("CU_NO").ToString().Trim()
            Me.cboCustomerEU.Items.Add(oListItem1)
        Next

        'cboCustomerEU.DataSource = dvCustomer
        'cboCustomerEU.DataTextField = "CU_NAME"
        'cboCustomerEU.DataValueField = "CU_NO"
        'cboCustomerEU.DataBind()

    End Sub

    ''' <summary>
    ''' 查詢客戶下拉選單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryShippingCustomer()

        'Dim CompNo As String = Me.UI_cboFrom.SelectedValue.ToString().Trim()
        Dim oShipment As New ctlRMA.Shipment

        'Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
        Dim dtCustomer As New DataTable

        'dtCustomer = oShipping.QueryByShipping_Customer(CompNo, "")
        'dtCustomer = oShipment.QueryByShipment_Customer(Session("_UserID").ToString(), "")
        dtCustomer = oShipment.QueryByShipment_Customer(Session("_RepairCenter").ToString())

        Dim dvCustomer As DataView = dtCustomer.DefaultView


        cboCustomer.DataSource = dvCustomer
        cboCustomer.DataTextField = "CU_NAME"
        cboCustomer.DataValueField = "CU_NO"
        cboCustomer.DataBind()
    End Sub

    ''' <summary>
    ''' 顯示選擇客戶的相關信息
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryShippingCustomerDetail()
        'Dim CustNo As String = Me.UI_cboFrom.SelectedValue.ToString().Trim()
        Dim CustNo As String = "$$"
        If cboCustomer.SelectedIndex > -1 Then
            Me.UI_txtCustomer.Text = Me.cboCustomer.SelectedItem.Text.Trim()
            CustNo = Me.cboCustomer.SelectedValue.Trim()
        End If
        Call setFrom(CustNo)
        Call setCustomer(CustNo)
    End Sub

    ''' <summary>
    ''' 修改的時候顯示Shipping單頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShipping()
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShipment As New RmaDTO.ShipmentDataTable
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByRMA_Shipping(sShippingID)

        If dtShipping.Rows.Count > 0 Then
            Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.Rows(0)

            Me.UI_lblPreviousPage_SHIPPINGNO.Text = dr.RMASH_SHIPPINGNO.ToString().Trim()

            Me.UI_lblShippingText.Text = dr.RMASH_SHIPPINGNO.ToString().Trim()
            Me.UI_lblDateText.Text = Convert.ToDateTime(dr.RMASH_CSTMP.ToString().Trim()).ToShortDateString()
            'Me.cboCustomer.SelectedValue = dr.RMASH_CUNO.ToString().Trim()
            Me.cboCustomer.SelectedValue = cboCustomer.Items.FindByValue(dr.RMASH_CUNO.ToString().Trim()).Value

            '抓Enduser
            QueryShippingCustomerEU()

            'test
            'Me.UI_lblPreviousPage_SHIPPINGNO.Text = dr.RMASH_PACKINGLIST.ToString().Trim() & "/test"

            Me.cboCustomerEU.SelectedValue = dr.RMASH_PACKINGLIST.ToString().Trim()
            Me.cboCustomerEU.Enabled = False
            Me.cboCustomer.Enabled = False

            Me.UI_cboFrom.SelectedValue = dr.RMASH_FROM.ToString().Trim()
            Me.UI_txtCustomer.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblCustomerID.Text = dr.RMASH_CUNO.ToString().Trim()
            Me.UI_txtTracking.Text = dr.RMASH_TRACKINGNO.ToString().Trim()
            dtShipment = oShipment.QueryShipmentByShipingID(dr.RMASH_ID)
            If dtShipment.Rows.Count > 0 Then
                Me.UI_opgShippingOrders.SelectedValue = dtShipment.Rows(0)("RMASM_ISSHIP").ToString().Trim()
                Me.UI_txtShippingNumber.Text = dtShipment.Rows(0)("RMASM_SHIPNO").ToString().Trim()
            End If

            '客戶地址
            'Call setCustomer(dr.RMASH_CUNO.ToString().Trim())
            '抓地址
            Call QueryShippingCustomerDetail()
            If dr.RMASH_PACKINGLIST.ToString().Trim() <> dr.RMASH_CUNO.ToString().Trim() Then
                Me.UI_txtCustomer.Text = dr.RMASH_PACKINGLIST.ToString().Trim()
                Call QueryShippingCustomerDetailEU()
            End If

            If dr.IsRMASH_ADDRESSNull = False Then Me.UI_cboAddress.SelectedValue = dr.RMASH_ADDRESS.ToString().Trim()
            If dr.IsRMASH_EXPRESSCONull = False Then Me.UI_cboExpress.SelectedValue = dr.RMASH_EXPRESSCO.ToString().Trim()
            If dr.IsRMASH_MEMONull = False Then Me.UI_txtMemo.Text = dr.RMASH_MEMO.ToString().Trim()

            If Me.cboCustomer.SelectedValue <> UI_txtCustomer.Text Then
                Me.UI_cboAddress.Enabled = False
            End If

            '快遞公司
            Call setCompany(dr.RMASH_FROM.ToString().Trim())
        End If

        Me.UI_txtCustomer.Enabled = False
        Me.UI_cboFrom.Enabled = False
    End Sub

    ''' <summary>
    ''' 顯示單身資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShippingDetail()
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.tmpShipping_DetailDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByRMA_ShippingDetail(sShippingID)

        Call Shipping_DataBind(dtShipping)
    End Sub

    ''' <summary>
    ''' 查詢可以出貨的Serial Number
    ''' </summary>
    ''' <param name="iPageIndex"></param>
    ''' <remarks></remarks>
    Private Sub QueryDataSerial(ByVal iPageIndex As Integer)
        Dim oSerial As New ctlRMA.Shipment
        Dim _dtRMAShipping As New RmaDTO.Shipment_DetailDataTable
        Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
        Dim dtSerialTmp As New RmaDTO.Shipment_DetailDataTable
        Dim sCuID As String = ""
        If cboCustomer.SelectedIndex > -1 Then
            sCuID = cboCustomer.SelectedValue.Trim()
        End If
        Dim dvSerial As DataView
        Dim _dvRMAShipping As DataView

        '排除已選的資料()
        Dim i As Integer = 0
        Dim sRMADID As String = ""
        If IsNothing(Session("_dtRMAShipping")) = False Then
            _dtRMAShipping = Session("_dtRMAShipping")
            _dvRMAShipping = _dtRMAShipping.DefaultView
            For i = 0 To _dvRMAShipping.Count - 1
                If sRMADID.Trim <> "" Then
                    sRMADID = sRMADID & " AND "
                End If
                sRMADID = sRMADID & "RMASMD_RMADID <> '" & _dvRMAShipping.Item(i)("RMASMD_RMADID").ToString().Trim() & "'"
            Next
        End If


        dtSerial = oSerial.QueryCustomerByRMADetail(sCuID, cboCustomerEU.SelectedValue.Trim())
        '考虑之前的资料
        Dim hDetail As Hashtable = ViewState("hDetail")
        Dim Item As DictionaryEntry
        For Each Item In hDetail
            Dim sUI_RMADID As String = Item.Key.ToString()
            dtSerialTmp = oSerial.QueryCustomerByRMADetailID(sUI_RMADID)
            If dtSerialTmp.Rows.Count > 0 Then
                dtSerial.Rows.Add(dtSerialTmp.Rows(0).ItemArray())
            End If
        Next
        dtSerial.AcceptChanges()

        dtSerial.DefaultView.Sort = " RMASMD_RMANO desc"
        If Not dtSerial.Rows.Count > 0 Then
            dtSerial = AddSerial01(dtSerial)
            dvSerial = dtSerial.DefaultView
            dvSerial.RowFilter = "RMASMD_oldMark = '9'"
        Else
            If sRMADID.Trim() <> "" Then        '排除已選的資料
                dvSerial = dtSerial.DefaultView
                dvSerial.RowFilter = sRMADID.ToString().Trim()
            Else
                dvSerial = dtSerial.DefaultView
            End If
        End If

        dvSerial.Sort = "RMASMD_RMANO asc, RMASMD_SERIALNO asc"
        Session("_dtSerial") = dtSerial             '將所有資料(dtSerial)暫存到_dtSerial(暫存資料)
        'Me.UI_dvSerial.PageSize = _PageSize
        Me.UI_dvSerial.PageSize = 100000
        Me.UI_dvSerial.PageIndex = iPageIndex
        Me.UI_dvSerial.DataSource = dvSerial
        Me.UI_dvSerial.DataBind()
        'dvSerial.RowFilter = ""
    End Sub

    Private Sub Shipping_DataBind(ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
        Session("_dtShippingDetail") = dtShippingDetail
        Me.UI_cmdPrint.Enabled = False
        Me.UI_cmdSave.Enabled = False
        Me.UI_cmdSubmit.Enabled = False

        Dim hDetail As Hashtable = ViewState("hDetail")
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        Dim oShipment As New ctlRMA.Shipment

        Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView()
        dvShippingDetail.RowFilter = "RMASHD_oldMark='0' OR RMASHD_oldMark='1'"       '只秀新增及修改的資料
        If dvShippingDetail.Count > 0 Then
            Dim i As Integer = 0
            For i = 0 To dvShippingDetail.Count - 1
                Dim sRMASM_ID = dvShippingDetail(i)("RMASM_ID").ToString().Trim()
                Dim sRMASMD_RMADID = dvShippingDetail(i)("RMASMD_RMADID").ToString().Trim()
                hDetail.Remove(sRMASMD_RMADID)
                hDetail.Add(sRMASMD_RMADID, sRMASMD_RMADID)

                hsSelectID.Remove(sRMASMD_RMADID)
                hsSelectID.Add(sRMASMD_RMADID, sRMASMD_RMADID)
            Next

            Me.UI_cmdPrint.Enabled = True
            Me.UI_cmdSave.Enabled = True
            Me.UI_cmdSubmit.Enabled = True
        End If

        'Me.UI_dvShippingList.DataSource = dtShippingDetail.DefaultView()
        'Me.UI_dvShippingList.DataBind()
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

        Dim dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable = Session("_dtShippingDetail")
        Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView

        dvShippingDetail.RowFilter = "RMASHD_ID='" & sID.ToString().Trim() & "'"
        If dvShippingDetail.Count > 0 Then
            Dim sMark As String = dvShippingDetail.Item(0)("RMASHD_oldMark").ToString().Trim()
            If sMark.Trim() = "1" Then
                dvShippingDetail(0)("RMASHD_oldMark") = "2"
            Else
                dvShippingDetail.Item(0).Delete()
            End If

        End If
        dvShippingDetail.RowFilter = ""

        Call Shipping_DataBind(dtShippingDetail)
    End Sub

    ''' <summary>
    ''' 選維修點
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFrom.SelectedIndexChanged
        Dim cboFrom As DropDownList = sender
        'Me.UI_lblAddressText.Text = ""
        Me.UI_urlExpress.Text = ""
        Me.UI_urlExpress.NavigateUrl = ""
        Me.UI_cboExpress.Items.Clear()
        'Me.UI_cboExpress.SelectedValue = "-1"

        Call setCompany(cboFrom.SelectedValue.Trim())
    End Sub

    ''' <summary>
    ''' 快遞公司
    ''' </summary>
    ''' <param name="CompNo"></param>
    ''' <remarks></remarks>
    Private Sub setCompany(ByVal CompNo As String)
        Dim i As Integer = 0
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable
        dtCompany = oCompany.QueryByPrimaryKey(CompNo.Trim())

        If dtCompany.Rows.Count > 0 Then
            Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(0)
            Dim arrExpress() As String

            If dr.IsCOMP_EXPRESSCONull = False Then
                arrExpress = Convert.ToString(dr.COMP_EXPRESSCO.ToString().Trim()).Split(",")

                Dim oListItem As ListItem
                Me.UI_cboExpress.Items.Clear()
                oListItem = New ListItem
                For i = 0 To arrExpress.Length - 1
                    oListItem = New ListItem
                    oListItem.Text = arrExpress(i).ToString().Trim()
                    oListItem.Value = arrExpress(i).ToString().Trim()
                    Me.UI_cboExpress.Items.Add(oListItem)
                Next

                Me.UI_cboExpress.Dispose()
            End If

            'If dr.IsCOMP_EXPRESSURLNull = False Then Me.UI_urlExpress.Text = dr.COMP_EXPRESSURL.ToString().Trim()
            'If dr.IsCOMP_EXPRESSURLNull = False Then Me.UI_urlExpress.NavigateUrl = dr.COMP_EXPRESSURL.ToString().Trim()
        End If
    End Sub

    ''' <summary>
    ''' 增加Shipping Pack 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd.Click
        Me.UI_cboFrom.Enabled = False
        Me.UI_txtCustomer.Enabled = False
        'Me.UI_cboAddress.Enabled = False
        Dim sCuID As String = UI_txtCustomer.Text.Trim()
    End Sub

    ''' <summary>
    ''' 客戶地址
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <remarks></remarks>
    Private Sub setCustomer(ByVal CustomerID As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        Me.UI_cboAddress.Items.Clear()
        dtCustome = oCustomer.QueryByCompany(CustomerID)

        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)

            If dr.IsCU_ADDRESS1Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS1.Trim()
                oListItem.Value = dr.CU_ADDRESS1.Trim()
                Me.UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS2Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS2.Trim()
                oListItem.Value = dr.CU_ADDRESS2.Trim()
                Me.UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS3Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS3.Trim()
                oListItem.Value = dr.CU_ADDRESS3.Trim()
                Me.UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS4Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS4.Trim()
                oListItem.Value = dr.CU_ADDRESS4.Trim()
                Me.UI_cboAddress.Items.Add(oListItem)
            End If
        End If

        dtCustomerUser = oCustomerUser.Query(CustomerID)
        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            If dr.IsCUUS_ADDRESSNull = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CUUS_ADDRESS.Trim()
                oListItem.Value = dr.CUUS_ADDRESS.Trim()
                Me.UI_cboAddress.Items.Add(oListItem)
            End If
        Next
        Me.UI_cboAddress.Dispose()
    End Sub

    ''' <summary>
    ''' 設定維修點-Customer下拉連動
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <remarks></remarks>
    Private Sub setFrom(ByVal CustomerID As String)
        '修改顯示全部的維修中心，且預設當前客戶的維修中心 by buck modify 20251113 begin
        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustome As CustomerDTO.VWCUSTOMERDataTable = oCustomer.QueryByCompany(CustomerID)
        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            UI_cboFrom.SelectedValue = dr.COMP_NO.Trim()
        End If

        'Dim i As Integer = 0
        'Dim oListItem As ListItem
        'Dim oCustomer As New ctlCustomer.Customer
        'Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable

        'Me.UI_cboFrom.Items.Clear()
        'dtCustome = oCustomer.QueryByCompany(CustomerID)

        'If dtCustome.Count > 0 Then
        '    Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)

        '    oListItem = New ListItem
        '    oListItem.Text = dr.COMP_NAME.Trim()
        '    oListItem.Value = dr.COMP_NO.Trim()
        '    Me.UI_cboFrom.Items.Add(oListItem)
        'End If
        '修改顯示全部的維修中心，且預設當前客戶的維修中心 by buck modify 20251113 end
    End Sub

    ''' <summary>
    ''' 判斷,如果存在於charge quoted並且 approve<>2 的時候, 不可以出貨
    ''' </summary>
    ''' <remarks></remarks>
    Private Function chkChargeQuoted_Approve() As Boolean
        Dim sMessage As String = ""
        Dim i As Integer = 0
        Dim retval As Boolean = False

        Dim ctlChargeQuoted As New ctlChargeQuoted()

        Try

            Dim hsSelectID As Hashtable = ViewState("hsSelectID")
            Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
            dtSerial = Session("_dtSerial")
            For i = 0 To dtSerial.Rows.Count - 1
                Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()

                If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                    If ctlChargeQuoted.chkChargeQuoted_Approve(RMASMD_RMANO) = True Then
                        sMessage = _oLanguage.getText("RMA", "450", ctlLanguage.eumType.Validator) & "<br>" & "[RMA NO:" & RMASMD_RMANO & "]"
                        Throw New Exception(sMessage.Trim())
                    End If
                End If
            Next
            retval = True

        Catch ex As Exception
            retval = False
            Me.ucMessage.showMessageByFailed(sMessage)

        Finally
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 暫存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Me.UI_lblPreviousPage_RMANO.Text = ""
        Call CheckedItem()
        If chkChargeQuoted_Approve() = True Then
            Call Save("Save")
        End If

    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Me.UI_lblPreviousPage_RMANO.Text = ""
        Call CheckedItem()

        If chkChargeQuoted_Approve() = True Then
            Call Save("Submit")
        End If
    End Sub

    ''' <summary>
    ''' Print
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Me.UI_lblPreviousPage_RMANO.Text = ""
        Call Save("Print")
    End Sub

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="sType">sType:不為Save就需修改RMA狀態</param>
    ''' <remarks></remarks>
    Private Sub Save(ByVal sType As String)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim sRMASHID As String = ""
        Dim retvalAR As String = ""

        Dim isPrint As Boolean = False
        Dim isSumbit As Boolean = False
        Dim curDateTime As Date = Date.Now

        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        Dim dtShippingDetail As New RmaDTO.tmpShipping_DetailDataTable
        Dim sKey As String = Me.UI_lblPreviousPage_RMASHID.Text.Trim()
        Dim RMASH_SHIPPINGNO As String = ""
        Dim retMessage As String = ""
        Dim oCompany As New ctlCompany

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Try
            If sType.ToLower().Trim() = "Print".ToLower().Trim() Then
                isPrint = True
            End If

            If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                isSumbit = True
            End If

            Dim sCurrencyCode As String = QueryByCompany_Currency(Me.UI_cboFrom.SelectedValue.ToString().Trim())
            Dim sStock As String = oCompany.QueryByStock(Me.UI_cboFrom.SelectedValue.ToString().Trim())
            If sStock = "N" Then
                retMessage = "ok"
            End If

            oConn.BeginTransaction()


            '=============================================================================================================================================================================================================================================================================================================
            '檢核庫存量是否足夠
            '=============================================================================================================================================================================================================================================================================================================
            'Throw New Exception(Me.UI_flowCase.Text)  'fairy
            'Throw New Exception(Me.UI_cboFrom.SelectedValue.ToString().Trim() )
            If isSumbit = True Then
                Dim dtRMASHIPDTL As New RmaDTO.Shipment_DetailDataTable
                Dim hsSelectID_ALL As Hashtable = ViewState("hsSelectID")
                Dim tCount As Integer = 0
                dtRMASHIPDTL = Session("_dtSerial")

                For i = 0 To dtRMASHIPDTL.Rows.Count - 1

                    Dim RMASMD_RMADID As String = dtRMASHIPDTL.Rows(i)("RMASMD_RMADID").ToString().Trim()
                    If hsSelectID_ALL.ContainsKey(RMASMD_RMADID) Then
                        tCount = tCount + 1
                    End If
                    '    Throw New Exception("Shipping Detail must less than 10")
                Next
                'If (tCount > 10) Then
                '    Throw New Exception("Shipping Detail must less than 10")
                'End If

                If Me.UI_flowCase.Text = "01" AndAlso sStock = "Y" Then
                    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                    dtSerial = Session("_dtSerial")
                    For i = 0 To dtSerial.Rows.Count - 1
                        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                            retMessage = runSP_SHP_RMA_STOCK(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                            If retMessage.Trim() <> "ok" Then
                                Throw New Exception(retMessage + " for " + RMASMD_SERIALNO)
                                Exit For
                            End If
                        End If
                    Next
                End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "CEAT" Then   'fairy add CEAT
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                'End If



                ''If Me.UI_flowCase.Text = "02" And Session("_RepairCenter").ToString().Trim().ToLower().IndexOf("CL_CHINA".ToLower()) <> -1 Then   'fairy
                'If Me.UI_cboFrom.SelectedValue.ToString().Trim() = "CL_CHINA" Then   'fairy
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable

                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_CN(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                'End If

                '20170726 AU不扣庫存 mark by Isaac
                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "AU" Then   '150825 add by MaggieChen:AU
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        Dim b As Boolean
                '        If RMASMD_RMANO = "SRMA-2017050008" Then
                '            b = False
                '        ElseIf RMASMD_RMANO = "SRMA-2017050007" Then
                '            b = False
                '        Else
                '            b = True
                '        End If

                '        If b Then
                '            If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '                Dim retMessage As String = runSP_SHP_RMA_INV_AU(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '                If retMessage.Trim() <> "OK" Then
                '                    Throw New Exception(retMessage.Trim())
                '                End If
                '            End If
                '        End If
                '    Next
                'End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "AUS" Then   '20160120 add by Angel:AUS
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_AUS(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "JP_BYTE" Then   '20190919 add by Angel:BYTE
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_BYTE(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If


                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "AU_LAPTOP_KINGS" Then   '20210420 add by Wisely
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_AU_LAPTOP_KINGS(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "NZ_PB_TECH" Then   '20190919 add by Angel:BYTE
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_NZ_PB_TECH(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "UK_FALA" Then   '20210420 add by Wisely
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_UK_FALA(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If


                ''Mplus
                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "US_CL_MPLUS" Then   '20190919 add by Angel:BYTE
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_US_CL_MPLUS(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If

                'If Me.UI_flowCase.Text = "01" And Me.UI_cboFrom.SelectedValue.ToString().Trim() = "JP_BYTE_MPLUS" Then   '20190919 add by Angel:BYTE
                '    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                '    Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
                '    dtSerial = Session("_dtSerial")
                '    For i = 0 To dtSerial.Rows.Count - 1
                '        Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
                '        Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
                '        Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
                '        'Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

                '        If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '            Dim retMessage As String = runSP_SHP_RMA_INV_JP_BYTE_MPLUS(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)
                '            If retMessage.Trim() <> "OK" Then
                '                Throw New Exception(retMessage.Trim())
                '            End If
                '        End If
                '    Next
                '    ' End If
                'End If


            End If

            '--只是保險 若庫存有問題 確定不往下做-----------
            'If retMessage <> "ok" Then
            '    oConn.Rollback()
            '    blnFlag = False
            '    addLog(retMessage)
            '    ScriptManager.RegisterStartupScript(UI_cmdSubmit, UI_cmdSubmit.GetType(), "alert", "alert('" + retMessage + "');", True)
            '    Return
            'End If

            'Shipment(Table)
            Call DeleteRMA_SHIPMENT_DETAIL(oConn)
            Call SaveShipmentAll(oConn, curDateTime)

            dtShipping = Save_Shipping(sKey, curDateTime)
            If IsNothing(Session("_dtShippingDetail")) = False Then
                dtShippingDetail = Session("_dtShippingDetail")
            End If

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    sRMASHID = oShipping.SaveByAddNew(oConn, sType, dtShipping, dtShippingDetail, isSumbit, Me.cboCustomer.SelectedValue.Trim(), RMASH_SHIPPINGNO, sCurrencyCode, Session("_UserID"))

                    Me.UI_lblPreviousPage_SHIPPINGNO.Text = RMASH_SHIPPINGNO

                Case eumCommand.UPDATE
                    'RMASH_SHIPPINGNO = Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim()
                    oShipping.SaveByEdit(oConn, sType, sKey, dtShipping, dtShippingDetail, isSumbit, Me.cboCustomer.SelectedValue.Trim(),
                    Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim(), sCurrencyCode, Session("_UserID"))

                    sRMASHID = sKey
            End Select
            oConn.Commit()

            If isSumbit = True Then
                '如果維修中心是CEAT, CLHQ, CL_CHINA,AU才會呼叫runSP_SHP_INS_AR
                'Dim isRun_SPAR As Boolean = False
                'Dim sRepairNo As String = "CEAT,CLHQ,CL_CHINA,AU,JP_BYTE,JP_BYTE_MPLUS,AU_LAPTOP_KINGS,NZ_PB_TECH,US_CL_MPLUS"
                'Dim arrRepairNo() As String = sRepairNo.Split(",")
                'For i = 0 To arrRepairNo.Length - 1
                '    If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo(i).ToString().Trim()) <> -1 Then
                '        isRun_SPAR = True
                '        Exit For
                '    End If
                'Next

                Dim isRun_SPAR As Boolean = True

                If isRun_SPAR = True Then
                    '取得單上的 維修中心
                    Dim sRepairCenter As String = ""
                    For i = 0 To dtShipping.Rows.Count - 1
                        Dim sRMASH_COMPNO As String = dtShipping.Rows(i)("RMASH_COMPNO").ToString().Trim()
                        If sRepairCenter.IndexOf(sRMASH_COMPNO) = -1 Then
                            If sRepairCenter.Trim() <> "" Then
                                sRepairCenter = sRepairCenter + ","
                            End If
                            sRepairCenter = sRepairCenter + sRMASH_COMPNO
                            Session("_RepairCenter1") = sRepairCenter         'fairy
                        End If
                    Next
                    retvalAR = oShipping.runSP_SHP_INS_AR(Me.cboCustomer.SelectedValue.Trim(), Me.UI_lblPreviousPage_SHIPPINGNO.Text, sCurrencyCode, Session("_UserID"), sRepairCenter)
                    addLog(retvalAR)
                End If
                'Throw New Exception(retvalAR)
            End If

            blnFlag = True

        Catch ex As Exception
            oConn.Rollback()
            addLog(ex.Message)
            sMessage = ex.Message + " at " + ex.Source + " " + ex.StackTrace
            blnFlag = False
            'Throw New Exception(ex.Message, ex)
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                oConn.Close()
                oConn.Dispose()

                If isSumbit = True Then
                    'Me.UI_lblPreviousPage_RMANO.Text = "FRMA-2015030027,FRMA-2015030026"
                    ' Dim sRepairCenter1  As String = ""

                    'Dim ctlShipping As New ctlRMA.Shipping
                    'Dim dtShipping_Head As New RmaDTO.RMA_ShippingDataTable
                    'dtShipping_Head = ctlShipping.QueryByRMA_Shipping(sRMASHID)
                    'If dtShipping_Head.Rows.Count > 0 Then
                    '    Dim dr As RmaDTO.RMA_ShippingRow = dtShipping_Head.Rows(0)
                    '    If dr.IsRMASH_ARNONull = False Then
                    '        sRMASH_ARNO = dr.RMASH_ARNO.Trim()
                    '    End If

                    '    If dr.IsRMASH_INVNONull = False Then
                    '        sRMASH_INVNO = dr.RMASH_INVNO.Trim()
                    '    End If
                    'End If

                    'Call Print_Inovice(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())	'fairy
                    'Call Print_AD(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())	'fairy

                    Dim arrAR() As String = retvalAR.Trim().Split(",")
                    Dim sRMA_ARNumber As String = ""

                    If arrAR(0).ToLower() = "ok" Then
                        addLog("submit Ok")
                        If arrAR.Length > 1 Then
                            For i = 1 To arrAR.Length - 1
                                Me.ViewState("_AttachmentFile_01") = ""
                                Me.ViewState("_AttachmentFile_02") = ""

                                Dim sRMA_ARNO As String = arrAR(i).ToString().Trim()
                                sRMA_ARNumber = sRMA_ARNO
                                addLog("Finished AR:" & sRMA_ARNO)
                                'Dim arrAR2() As String = sAR_INV.Trim().Split("@")
                                'Dim sRMA_ARNO As String = arrAR2(0).ToString().Trim()
                                'Dim sRMA_INVNO As String = arrAR2(1).ToString().Trim()
                                Try
                                    addLog("InvoiceShip:" & Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())
                                    Call Print_Inovice(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim(), Session("_RepairCenter1").ToString().Trim(), sRMA_ARNO)   'fairy
                                Catch ex As Exception
                                    addLog("Error Invoice")
                                End Try
                                Try
                                    addLog("ADCenter:" & Session("_RepairCenter1").ToString().Trim())
                                    Call Print_AD(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim(), Session("_RepairCenter1").ToString().Trim(), sRMA_ARNO)    'fairy
                                Catch ex As Exception
                                    addLog("Error AD")
                                End Try

                                'RMA單號 + Shipping單號 + AR.NO
                                'addLog("SendMailNoticeBySales")
                                Call SendMailNoticeBySales(Me.UI_lblPreviousPage_RMANO.Text.Trim(), Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim(), sRMA_ARNO)
                                'RMA單號 + Shipping單號 + AR.NO + InvoiceNo
                                'Call SendMailNoticeBySales(Me.UI_lblPreviousPage_RMANO.Text.Trim(), Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim(), sRMA_ARNO, sRMA_INVNO)
                            Next
                        End If
                    End If

                    Me.ViewState("_AttachmentFile_03") = ""
                    'Add by Isaac
                    Dim bool_ISCW As Boolean = oShipping.chkShpISCWarranty(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())

                    '20210721 wisely add NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
                    If (UI_cboFrom.SelectedValue.ToString().Trim() = "NZ_PB_TECH" OrElse UI_cboFrom.SelectedValue.ToString().Trim() = "AU_LAPTOP_KINGS") Then
                        bool_ISCW = False
                    End If

                    '20210622 wisely 若是BU046 且有輸入 Enduser 資料 依全保方式處理
                    If Me.cboCustomer.SelectedValue.Trim() = "BU046" Then
                        bool_ISCW = True
                    End If

                    'by Ryan, 針對 CA010 20240321 一定要賣他CW 開始
                    If Me.cboCustomer.SelectedValue.Trim() = "CA010" Then
                        bool_ISCW = True
                    End If
                    'by Ryan 針對 CA010 20240321 一定要賣他CW 結束

                    '全保要寄 packing list
                    If bool_ISCW Then
                        Try
                            addLog("Ship Packing List:" & Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())
                            Call Print_PackingList(Me.UI_lblPreviousPage_SHIPPINGNO.Text.Trim())
                        Catch ex As Exception
                            addLog("Error Packing List")
                        End Try

                    End If

                    Call SendMail_Notice(dtShippingDetail, curDateTime, sRMA_ARNumber)

                End If

                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                Else
                    If isPrint = True Then
                        Response.Redirect("Shipping_Print.aspx?sRMASHID=" & _Crypto.Encrypt(sRMASHID.Trim, ""))
                    Else
                        Dim sMsg As String = ""
                        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                            Case eumCommand.AddNew
                                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)

                            Case eumCommand.UPDATE
                                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                        End Select

                        If sType.ToLower().Trim() = "Save".ToLower().Trim() Then
                            Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Shipping_List.aspx")
                        Else
                            Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Transfer, "Shipping_List.aspx")
                            'Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Transfer, "ShippingNotice_Print.aspx")
                        End If

                    End If
                End If
            End If
        End Try
    End Sub

    Private Sub addLog(ByVal LogValue As String)
        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_LOG"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
            oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try
    End Sub

    Private Function SaveShipmentAll(ByVal oConn As ICAT_OracleDAO.Connection, ByVal curDateTime As Date) As String
        Dim sReturn As String = ""
        Dim i As Integer = 0
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
        dtSerial = Session("_dtSerial")

        Dim dtShippingDetail As New RmaDTO.tmpShipping_DetailDataTable
        If IsNothing(Session("_dtShippingDetail")) = False Then
            dtShippingDetail = Session("_dtShippingDetail")
        End If


        For i = 0 To dtSerial.Rows.Count - 1
            Dim RMASMD_RMADID As String = dtSerial.Rows(i)("RMASMD_RMADID").ToString().Trim()
            Dim RMASMD_RMANO As String = dtSerial.Rows(i)("RMASMD_RMANO").ToString().Trim()
            Dim RMASMD_SERIALNO As String = dtSerial.Rows(i)("RMASMD_SERIALNO").ToString().Trim()
            Dim RMASMD_MODELNO As String = dtSerial.Rows(i)("RMASMD_MODELNO").ToString().Trim()

            If hsSelectID.ContainsKey(RMASMD_RMADID) Then
                '---------------------------------------------------------------------------------------------------
                Dim ctlShipment As New ctlRMA.Shipment
                Dim dtShipment_Qry As New RmaDTO.ShipmentDataTable
                Dim dtShipment As New RmaDTO.ShipmentDataTable
                Dim dtShipmentDetail As New RmaDTO.Shipment_DetailDataTable
                Dim RMASM_ID As String = ""
                '---------------------------------------------------------------------------------------------------

                Dim RMASM_PACKINGNO As String = ""
                dtShipmentDetail = SaveShipmentDetail(True, curDateTime, RMASMD_RMADID)
                dtShipment = SaveShipment("", True, True, curDateTime, RMASMD_RMADID)

                RMASM_ID = ctlShipment.SaveByAddNew_Shipping(oConn, "", dtShipment, dtShipmentDetail, RMASM_PACKINGNO)

                Dim dr As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.NewtmpShipping_DetailRow
                Dim oGuid As Guid = Guid.NewGuid

                dr.RMASHD_ID = oGuid.ToString()
                dr.RMASHD_RMASHID = oGuid.ToString()
                dr.RMASHD_CTNNO = ""
                dr.RMASHD_DESCRIPTION = ""

                'dtShipment_Qry = ctlShipment.QueryShipmentByShipmentID(RMASM_ID)
                'If dtShipment.Rows.Count > 0 Then
                '    dr.RMASHD_SHIPMENTNO = dtShipment_Qry.Rows(0)("RMASM_PACKINGNO").ToString()
                '    dr.RMASHD_RMANO = dtShipment_Qry.Rows(0)("RMASMD_RMANO").ToString()
                '    dr.RMASHD_RMASMPACKINGNO = dtShipment_Qry.Rows(0)("RMASM_PACKINGNO").ToString()
                'Else
                '    dr.RMASHD_SHIPMENTNO = ""
                'End If

                dr.RMASHD_SHIPMENTNO = RMASM_PACKINGNO
                dr.RMASHD_RMASMPACKINGNO = RMASM_PACKINGNO

                If dtShipmentDetail.Rows.Count > 0 Then
                    dr.RMASHD_RMANO = dtShipmentDetail.Rows(0)("RMASMD_RMANO").ToString()
                End If

                dr.RMASHD_QUANTITY = 1
                dr.RMASHD_NETWEIGHT = 0
                dr.RMASHD_GROSSWEIGH = 0
                dr.RMASHD_MEASUREMENT = 0
                dr.RMASHD_AD = Session("_UserID")
                dr.RMASHD_ADNAME = Session("_UserName")
                dr.RMASHD_CSTMP = Date.Now
                dr.RMASHD_LUAD = Session("_UserID")
                dr.RMASHD_LUADNAME = Session("_UserName")
                dr.RMASHD_LUSTMP = Date.Now
                dr.RMASHD_RMANO = RMASMD_RMANO
                'dr.RMASHD_RMASMPACKINGNO = ""
                dr.RMASHD_oldMark = "0"

                If Me.UI_lblPreviousPage_RMANO.Text.IndexOf(RMASMD_RMANO) = -1 Then
                    If Me.UI_lblPreviousPage_RMANO.Text.Trim() <> "" Then
                        Me.UI_lblPreviousPage_RMANO.Text = Me.UI_lblPreviousPage_RMANO.Text + ","
                    End If

                    Me.UI_lblPreviousPage_RMANO.Text = Me.UI_lblPreviousPage_RMANO.Text & RMASMD_RMANO
                End If
                dtShippingDetail.AddtmpShipping_DetailRow(dr)
            End If
        Next

        Session("_dtShippingDetail") = dtShippingDetail
        Return sReturn
    End Function

    Private Function Save_Shipping(ByVal sKey As String, ByVal curDateTime As Date) As RmaDTO.RMA_ShippingDataTable
        Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.NewRMA_ShippingRow

        Dim oGuid As Guid = Guid.NewGuid
        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                dr.RMASH_ID = oGuid.ToString().Trim()               '系統自動產生唯一識別碼

            Case eumCommand.UPDATE
                dr.RMASH_ID = sKey.Trim()                           '系統自動產生唯一識別碼

        End Select

        Try
            dr.RMASH_SHIPPINGNO = oGuid.ToString().Trim()        'Shipping No
            If cboCustomerEU.SelectedValue.Trim() = "" Then
                dr.RMASH_PACKINGLIST = cboCustomer.SelectedValue.Trim()
            Else
                dr.RMASH_PACKINGLIST = cboCustomerEU.SelectedValue.Trim()
            End If
            'dr.RMASH_SHIPPINGBY()
            dr.RMASH_FROM = Me.UI_cboFrom.SelectedValue.ToString().Trim()
            dr.RMASH_CUNO = Me.cboCustomer.SelectedValue.Trim()
            dr.RMASH_ADDRESS = Me.UI_cboAddress.SelectedItem.Text.ToString().Trim()
            dr.RMASH_EXPRESSCO = Me.UI_cboExpress.SelectedValue.Trim()
            dr.RMASH_TRACKINGNO = Me.UI_txtTracking.Text.ToString().Trim()
            dr.RMASH_MEMO = Me.UI_txtMemo.Text.ToString().Trim()

            dr.RMASH_AD = Session("_UserID")
            dr.RMASH_ADNAME = Session("_UserName")
            dr.RMASH_CSTMP = curDateTime
            dr.RMASH_LUAD = Session("_UserID")
            dr.RMASH_LUADNAME = Session("_UserName")
            dr.RMASH_LUSTMP = curDateTime

            dr.RMASH_COMPNO = Me.UI_cboFrom.SelectedValue.ToString().Trim()

            dtShipping.AddRMA_ShippingRow(dr)

        Catch ex As Exception
            Throw ex
        End Try

        Return dtShipping
    End Function

    Private Sub DeleteRMA_SHIPMENT_DETAIL(ByVal oConn As ICAT_OracleDAO.Connection)
        Dim ctlShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.tmpShipping_DetailDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = ctlShipping.QueryByRMA_ShippingDetail(sShippingID)

        Dim oShipment As New ctlRMA.Shipment
        Dim i As Integer = 0
        For i = 0 To dtShipping.Rows.Count - 1
            Dim sRMASM_ID = dtShipping.Rows(i)("RMASM_ID").ToString().Trim()
            Dim sRMASMD_RMADID = dtShipping.Rows(i)("RMASMD_RMADID").ToString().Trim()
            oShipment.ShipmentDelete(oConn, sRMASM_ID)
        Next
    End Sub

    ''' <summary>
    ''' 注意
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <param name="isSumbit"></param>
    ''' <param name="isBossConfirm"></param>
    ''' <param name="curDateTime"></param>
    ''' <param name="RMAD_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveShipment(ByVal sKey As String, ByVal isSumbit As Boolean, ByVal isBossConfirm As Boolean, ByVal curDateTime As Date, ByVal RMAD_ID As String) As RmaDTO.ShipmentDataTable
        Dim dtShipment As New RmaDTO.ShipmentDataTable
        Dim dr As RmaDTO.ShipmentRow = dtShipment.NewShipmentRow

        Dim oGuid As Guid = Guid.NewGuid
        dr.RMASM_ID = oGuid.ToString().Trim()               '系統自動產生唯一識別碼
        Try

            dr.RMASM_PACKINGNO = oGuid.ToString().Trim()        'RMA Shipment 編號
            dr.RMASM_CUNO = Me.cboCustomer.SelectedValue.Trim()
            dr.RMASM_ISSHIP = Convert.ToInt32(Me.UI_opgShippingOrders.SelectedValue.ToString().Trim())
            dr.RMASM_SHIPNO = Me.UI_txtShippingNumber.Text.ToString().Trim()
            dr.RMASM_SHIPMEMO = Me.UI_txtMemo.Text.ToString().Trim()

            dr.RMASM_AD = Session("_UserID")
            dr.RMASM_ADNAME = Session("_UserName")
            dr.RMASM_CSTMP = curDateTime
            dr.RMASM_LUAD = Session("_UserID")
            dr.RMASM_LUADNAME = Session("_UserName")
            dr.RMASM_LUSTMP = curDateTime

            Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
            Dim oSerial As New ctlRMA.Shipment

            dtSerial = oSerial.QueryCustomerByRMADetailID_Save(RMAD_ID)
            If dtSerial.Rows.Count > 0 Then

                If dtSerial.Rows(0)("RMAD_STATUS").ToString().Trim() = "91" Then
                    dr.RMARSM_LABORCOST = 0
                    dr.RMARSM_MATERIALCOST = 0
                    dr.RMARSM_QUOTE = 0
                    dr.RMARSM_CURRENCYCODE = " "
                    dr.RMARSM_CURRENCYRATE = 0

                Else

                    dr.RMARSM_LABORCOST = 0
                    If dtSerial.Rows(0)("RMARSD_LABORCOST").ToString().Trim() <> "" Then
                        dr.RMARSM_LABORCOST = dtSerial.Rows(0)("RMARSD_LABORCOST").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldLABORCOST").ToString().Trim() <> "" Then
                            dr.RMARSM_LABORCOST = dtSerial.Rows(0)("RMARSD_oldLABORCOST").ToString()
                        End If
                    End If

                    dr.RMARSM_MATERIALCOST = 0
                    If dtSerial.Rows(0)("RMARSD_MATERIALCOST").ToString().Trim() <> "" Then
                        dr.RMARSM_MATERIALCOST = dtSerial.Rows(0)("RMARSD_MATERIALCOST").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldMATERIALCOST").ToString().Trim() <> "" Then
                            dr.RMARSM_MATERIALCOST = dtSerial.Rows(0)("RMARSD_oldMATERIALCOST").ToString()
                        End If
                    End If

                    dr.RMARSM_QUOTE = 0
                    If dtSerial.Rows(0)("RMARSD_QUOTE").ToString().Trim() <> "" Then
                        dr.RMARSM_QUOTE = dtSerial.Rows(0)("RMARSD_QUOTE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldQUOTE").ToString().Trim() <> "" Then
                            dr.RMARSM_QUOTE = dtSerial.Rows(0)("RMARSD_oldQUOTE").ToString()
                        End If
                    End If

                    dr.RMARSM_CURRENCYCODE = " "
                    If dtSerial.Rows(0)("RMARSD_CURRENCYCODE").ToString().Trim() <> "" Then
                        dr.RMARSM_CURRENCYCODE = dtSerial.Rows(0)("RMARSD_CURRENCYCODE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldCURRENCYCODE").ToString().Trim() <> "" Then
                            dr.RMARSM_CURRENCYCODE = dtSerial.Rows(0)("RMARSD_oldCURRENCYCODE").ToString()
                        End If
                    End If


                    dr.RMARSM_CURRENCYRATE = 0
                    If dtSerial.Rows(0)("RMARSD_CURRENCYRATE").ToString().Trim() <> "" Then
                        dr.RMARSM_CURRENCYRATE = dtSerial.Rows(0)("RMARSD_CURRENCYRATE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldCURRENCYRATE").ToString().Trim() <> "" Then
                            dr.RMARSM_CURRENCYRATE = dtSerial.Rows(0)("RMARSD_oldCURRENCYRATE").ToString()
                        End If
                    End If

                End If

            End If

            dr.RMASM_ISBOSSCONFIRM = 0                  '是否需主管審核:0.否, 1.是, 2.已審核
            If isBossConfirm = True Then
                dr.RMASM_ISBOSSCONFIRM = 1
            End If

            '是否已Submit: 0.否, 1.是 
            dr.RMASM_ISSUBMIT = 0
            If isSumbit = True Then
                dr.RMASM_ISSUBMIT = 1
            End If
            dr.RMASM_ISSHIP = Me.UI_opgShippingOrders.SelectedValue
            dr.RMASM_SHIPNO = Me.UI_txtShippingNumber.Text

            dtShipment.AddShipmentRow(dr)

        Catch ex As Exception
            Throw ex
        End Try

        Return dtShipment
    End Function

    ''' <summary>
    ''' 注意
    ''' </summary>
    ''' <param name="isBossConfirm"></param>
    ''' <param name="curDateTime"></param>
    ''' <param name="RMAD_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveShipmentDetail(ByRef isBossConfirm As Boolean, ByVal curDateTime As Date, ByVal RMAD_ID As String) As RmaDTO.Shipment_DetailDataTable
        Dim i As Integer = 0
        Dim oRMA As New ctlRMA
        Dim dtShipmentDetail As New RmaDTO.Shipment_DetailDataTable

        Try
            Dim dr As RmaDTO.Shipment_DetailRow = dtShipmentDetail.NewShipment_DetailRow

            Dim oGuid As Guid = Guid.NewGuid

            dr.RMASMD_RMASMID = ""
            dr.RMASMD_ID = oGuid.ToString()

            Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
            Dim oSerial As New ctlRMA.Shipment

            dtSerial = oSerial.QueryCustomerByRMADetailID_Save(RMAD_ID)
            If dtSerial.Rows.Count > 0 Then
                dr.RMASMD_RMANO = dtSerial.Rows(0)("RMASMD_RMANO").ToString()
                dr.RMASMD_RMADID = dtSerial.Rows(0)("RMASMD_RMADID").ToString()

                dr.RMASMD_MODELNO = dtSerial.Rows(0)("RMASMD_MODELNO").ToString()
                dr.RMASMD_SERIALNO = dtSerial.Rows(0)("RMASMD_SERIALNO").ToString()

                If dtSerial.Rows(0)("RMAD_STATUS").ToString().Trim() = "91" Then
                    dr.RMARSD_LABORCOST = 0
                    dr.RMARSD_MATERIALCOST = 0
                    dr.RMARSD_QUOTE = 0
                    dr.RMARSD_CURRENCYCODE = " "
                    dr.RMARSD_CURRENCYRATE = 0
                    dr.RMASMD_LOWESTDISCOUNT = 0

                Else
                    dr.RMARSD_LABORCOST = 0
                    If dtSerial.Rows(0)("RMARSD_LABORCOST").ToString().Trim() <> "" Then
                        dr.RMARSD_LABORCOST = dtSerial.Rows(0)("RMARSD_LABORCOST").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldLABORCOST").ToString().Trim() <> "" Then
                            dr.RMARSD_LABORCOST = dtSerial.Rows(0)("RMARSD_oldLABORCOST").ToString()
                        End If
                    End If

                    dr.RMARSD_MATERIALCOST = 0
                    If dtSerial.Rows(0)("RMARSD_MATERIALCOST").ToString().Trim() <> "" Then
                        dr.RMARSD_MATERIALCOST = dtSerial.Rows(0)("RMARSD_MATERIALCOST").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldMATERIALCOST").ToString().Trim() <> "" Then
                            dr.RMARSD_MATERIALCOST = dtSerial.Rows(0)("RMARSD_oldMATERIALCOST").ToString()
                        End If
                    End If

                    dr.RMARSD_QUOTE = 0
                    If dtSerial.Rows(0)("RMARSD_QUOTE").ToString().Trim() <> "" Then
                        dr.RMARSD_QUOTE = dtSerial.Rows(0)("RMARSD_QUOTE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldQUOTE").ToString().Trim() <> "" Then
                            dr.RMARSD_QUOTE = dtSerial.Rows(0)("RMARSD_oldQUOTE").ToString()
                        End If
                    End If

                    dr.RMARSD_CURRENCYCODE = " "
                    If dtSerial.Rows(0)("RMARSD_CURRENCYCODE").ToString().Trim() <> "" Then
                        dr.RMARSD_CURRENCYCODE = dtSerial.Rows(0)("RMARSD_CURRENCYCODE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldCURRENCYCODE").ToString().Trim() <> "" Then
                            dr.RMARSD_CURRENCYCODE = dtSerial.Rows(0)("RMARSD_oldCURRENCYCODE").ToString()
                        End If
                    End If

                    dr.RMARSD_CURRENCYRATE = 0
                    If dtSerial.Rows(0)("RMARSD_CURRENCYRATE").ToString().Trim() <> "" Then
                        dr.RMARSD_CURRENCYRATE = dtSerial.Rows(0)("RMARSD_CURRENCYRATE").ToString()
                    Else
                        If dtSerial.Rows(0)("RMARSD_oldCURRENCYRATE").ToString().Trim() <> "" Then
                            dr.RMARSD_CURRENCYRATE = dtSerial.Rows(0)("RMARSD_oldCURRENCYRATE").ToString()
                        End If
                    End If

                    If dtSerial.Rows(0)("RMASMD_LOWESTDISCOUNT").ToString().Equals("") Then
                        dr.RMASMD_LOWESTDISCOUNT = 0
                    Else
                        dr.RMASMD_LOWESTDISCOUNT = dtSerial.Rows(0)("RMASMD_LOWESTDISCOUNT").ToString()
                    End If
                End If


                '1.最低折扣>0, 計算 維修後最低折扣金額
                '2.如果業務報價金額<=維修後最低折扣金額, 需要寄送mail 給主管審核
                'If dr.RMASMD_LOWESTDISCOUNT > 0 And isBossConfirm = False Then
                'Dim txtRepairQuoted As TextBox = Me.UI_dvShipping.Items(i).FindControl("txtRepairQuoted")
                'Dim iRepairQuoted As Double = Convert.ToDouble(txtRepairQuoted.Text.Trim())
                'Dim iDisCountAMT As Double = iRepairQuoted * (dr.RMASMD_LOWESTDISCOUNT * 0.1)  '維修後折扣金額
                'If dr.RMARSD_QUOTE <= iDisCountAMT Then
                'isBossConfirm = True
                'End If
                'End If
            End If

            dr.RMASMD_AD = Session("_UserID")
            dr.RMASMD_ADNAME = Session("_UserName")
            dr.RMASMD_CSTMP = curDateTime
            dr.RMASMD_LUAD = Session("_UserID")
            dr.RMASMD_LUADNAME = Session("_UserName")
            dr.RMASMD_LUSTMP = curDateTime
            dr.RMASMD_oldMark = "0"

            dtShipmentDetail.AddShipment_DetailRow(dr)
        Catch ex As Exception
            Throw ex

        End Try

        Return dtShipmentDetail
    End Function

    ''' <summary>
    ''' 出貨通知--寄送Mail(對象:申請人(顧客))
    ''' </summary>
    ''' <param name="dtShippingDetail"></param>
    ''' <remarks></remarks>
    Private Sub SendMail_Notice(ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal curDateTime As Date, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim blnFlag As Boolean = False
        Dim RMADetail As String = ""
        Dim Applicant_Mail As String = ""
        Dim Applicant_Name As String = ""
        Dim RMANo As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim oRequested As New ctlRMA.Requested
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
        Dim oMail As New ctlMail
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
        Dim ctlRMA As New ctlRMA
        Dim RMA_INVNO As String = ""
        '取ARNO與InvoiceNo
        Try

            dtSales = ctlRMA.getSalesMail_ARNO(sRMA_ARNO)
            For i = 0 To dtSales.Rows.Count - 1
                RMA_INVNO = dtSales.Rows(0)("RMA_INVNO").ToString().Trim()
            Next
        Catch ex As Exception
            blnFlag = False
        Finally

        End Try



        Try
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
            Dim SalesName As String = Session("_UserName").ToString().Trim()
            '==============================================================================================================================
            '寄送Mail(對象:申請人(顧客))
            '==============================================================================================================================
            'Dim CU_NO As String = Me.UI_lblCustomerID.Text.ToString().Trim()        '客戶編號
            'Dim CU_EMAIL As String = oCustomer.getMail(CU_NO)


            Dim RMAD_ID As String = ""
            For i = 0 To dtShippingDetail.Rows.Count - 1
                Dim drShippingDetail As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)
                Dim PackingNo As String = drShippingDetail.RMASHD_RMASMPACKINGNO.ToString().Trim()
                RMANo = drShippingDetail.RMASHD_RMANO.ToString().Trim()

                dtShipment = oShipping.QueryByRMADID(PackingNo, RMANo)
                For j = 0 To dtShipment.Rows.Count - 1
                    Dim drShipment As RmaDTO.Shipment_DetailRow = dtShipment.Rows(j)
                    Dim RMADID As String = drShipment.RMASMD_RMADID.ToString().Trim()    '取RMAD_ID

                    If RMAD_ID.Trim() <> "" Then
                        RMAD_ID = RMAD_ID & ","
                    End If
                    RMAD_ID = RMAD_ID & RMADID.Trim()
                Next
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

                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.cboCustomer.SelectedValue.Trim())

                Dim sSubject As String = _oLanguage.getMailText("Mail", "017", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "018", ctlLanguage.eumType.Mail, LanguageID)

                sSubject = sSubject.Replace("[$INVNO$]", RMA_INVNO)
                sSubject = sSubject.Replace("[$RMANO$]", RMANo.Trim())
                sSubject = sSubject.Replace("[$Tracking no$]", Me.UI_txtTracking.Text.Trim())
                sSubject = sSubject.Replace("[$Shipped Submit Date$]", curDateTime)

                sBody = sBody.Replace("[$RMA Request No$]", RMANo.Trim())
                sBody = sBody.Replace("[$Applicant Name$]", Me.UI_txtCustomer.Text.Trim())
                sBody = sBody.Replace("[$Tracking No.$]", Me.UI_txtTracking.Text.Trim())
                sBody = sBody.Replace("[$RMA Detail$]", RMADetail.Trim())
                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                'Applicant_Mail = "ted@icat-tech.com.tw"
                Dim oAttachmentFile As New Collection
                If Me.ViewState("_AttachmentFile_03").ToString().Trim() <> "" Then
                    oAttachmentFile.Add(Me.ViewState("_AttachmentFile_03").ToString())
                    Dim strEndUserMail As String = ""
                    Try
                        strEndUserMail = oRequested.getEndUserMail(RMAD_ID.Trim())
                        addLog("Send mail PACKING LIST" & strEndUserMail)
                    Catch ex As Exception
                        addLog("Send Mail PACKING LIST ERROR" & ex.Message)
                    End Try
                    If strEndUserMail <> "" Then
                        Applicant_Mail = Applicant_Mail & "," & strEndUserMail
                    End If

                    'Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                    'Dim Repaire_Name As String = ""                    '維修人員
                    '取得維修人員Mail
                    Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RepairCenter(Me.UI_cboFrom.SelectedValue.Trim())
                    For j = 0 To arrRepaire.Count - 1
                        Dim arrList() As String = arrRepaire(j)
                        Applicant_Mail += "," + arrList(1).Trim()
                    Next
                End If
                'Applicant_Mail = "isaac.yeh@cipherlab.com.tw"
                'Applicant_Mail += "," + "ShippingManagement@cipherlab.com.tw"
                If _isDebug = True Then
                    Applicant_Mail = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                '20230720 提示訊息

                'EndUser Paypal 付款
                Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblCustomerID.Text.ToString().Trim(), "X0091")
                If (dt.Rows.Count > 0) Then
                    sBody = "</br>" + "  This is RMA from End User, Please shipping without waiting  "
                Else

                End If

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Applicant_Mail = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody, Applicant_Mail.Trim(), _MailCC, oAttachmentFile)
                If blnFlag = False Then
                    Exit Try
                End If

            End If


        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 出貨通知--寄送Mail(對象:MIS) 測試用/補印用
    ''' </summary>
    ''' <param name="dtShippingDetail"></param>
    ''' <remarks></remarks>
    Private Sub SendMail_NoticeByMIS(ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal curDateTime As Date, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim blnFlag As Boolean = False
        Dim RMADetail As String = ""
        Dim Applicant_Mail As String = ""
        Dim Applicant_Mail_MIS As String = ""
        Dim Applicant_Name As String = ""
        Dim RMANo As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim oRequested As New ctlRMA.Requested
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
        Dim oMail As New ctlMail
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
        Dim ctlRMA As New ctlRMA
        Dim RMA_INVNO As String = ""
        '取ARNO與InvoiceNo
        Try

            dtSales = ctlRMA.getSalesMail_ARNO(sRMA_ARNO)
            For i = 0 To dtSales.Rows.Count - 1
                RMA_INVNO = dtSales.Rows(0)("RMA_INVNO").ToString().Trim()
            Next
        Catch ex As Exception
            blnFlag = False
        Finally

        End Try

        Try
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
            Dim SalesName As String = Session("_UserName").ToString().Trim()
            '==============================================================================================================================
            '寄送Mail(對象:申請人(顧客))
            '==============================================================================================================================
            'Dim CU_NO As String = Me.UI_lblCustomerID.Text.ToString().Trim()        '客戶編號
            'Dim CU_EMAIL As String = oCustomer.getMail(CU_NO)


            Dim RMAD_ID As String = ""
            For i = 0 To dtShippingDetail.Rows.Count - 1
                Dim drShippingDetail As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)
                Dim PackingNo As String = drShippingDetail.RMASHD_RMASMPACKINGNO.ToString().Trim()
                RMANo = drShippingDetail.RMASHD_RMANO.ToString().Trim()

                dtShipment = oShipping.QueryByRMADID(PackingNo, RMANo)
                For j = 0 To dtShipment.Rows.Count - 1
                    Dim drShipment As RmaDTO.Shipment_DetailRow = dtShipment.Rows(j)
                    Dim RMADID As String = drShipment.RMASMD_RMADID.ToString().Trim()    '取RMAD_ID

                    If RMAD_ID.Trim() <> "" Then
                        RMAD_ID = RMAD_ID & ","
                    End If
                    RMAD_ID = RMAD_ID & RMADID.Trim()
                Next
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

                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.cboCustomer.SelectedValue.Trim())

                Dim sSubject As String = _oLanguage.getMailText("Mail", "017", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "018", ctlLanguage.eumType.Mail, LanguageID)

                sSubject = sSubject.Replace("[$INVNO$]", RMA_INVNO)
                sSubject = sSubject.Replace("[$RMANO$]", RMANo.Trim())
                sSubject = sSubject.Replace("[$Tracking no$]", Me.UI_txtTracking.Text.Trim())
                sSubject = sSubject.Replace("[$Shipped Submit Date$]", curDateTime)

                sBody = sBody.Replace("[$RMA Request No$]", RMANo.Trim())
                sBody = sBody.Replace("[$Applicant Name$]", Me.UI_txtCustomer.Text.Trim())
                sBody = sBody.Replace("[$Tracking No.$]", Me.UI_txtTracking.Text.Trim())
                sBody = sBody.Replace("[$RMA Detail$]", RMADetail.Trim())
                sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                'Applicant_Mail = "ted@icat-tech.com.tw"
                Dim oAttachmentFile As New Collection
                If Me.ViewState("_AttachmentFile_03").ToString().Trim() <> "" Then
                    oAttachmentFile.Add(Me.ViewState("_AttachmentFile_03").ToString())
                    Dim strEndUserMail As String = ""
                    Try
                        strEndUserMail = oRequested.getEndUserMail(RMAD_ID.Trim())
                        addLog("Send mail PACKING LLSIT" & strEndUserMail)
                    Catch ex As Exception
                        addLog("Send Mail PACKING LLSIT ERROR" & ex.Message)
                    End Try
                    If strEndUserMail <> "" Then
                        Applicant_Mail = Applicant_Mail & "," & strEndUserMail
                    End If

                    'Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                    'Dim Repaire_Name As String = ""                    '維修人員
                    '取得維修人員Mail
                    Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RepairCenter(Me.UI_cboFrom.SelectedValue.Trim())
                    For j = 0 To arrRepaire.Count - 1
                        Dim arrList() As String = arrRepaire(j)
                        Applicant_Mail += "," + arrList(1).Trim()
                    Next
                End If

                '20230720 提示訊息             
                'EndUser Paypal 付款
                Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblCustomerID.Text.ToString().Trim(), "X0091")
                If (dt.Rows.Count > 0) Then
                    sBody = "</br>" + "  This is RMA from End User, Please shipping without waiting  "
                Else

                End If

                'Test用
                'Applicant_Mail_MIS = "hugh.wang@cipherlab.com.tw"
                'Applicant_Mail += "," + "ShippingManagement@cipherlab.com.tw"
                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Applicant_Mail_MIS = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody & "\n Actual Mail To:" & Applicant_Mail, Applicant_Mail_MIS.Trim(), _MailCC, oAttachmentFile)

                If blnFlag = False Then
                    Exit Try
                End If

            End If


        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 出貨通知--寄送Mail(對象:業務, 助理)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendMailNoticeBySales(ByVal sRMANo As String, ByVal sShippingNo As String, ByVal sRMA_ARNO As String) As Boolean
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim ctlRMA As New ctlRMA
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
        Dim oMail As New ctlMail

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.cboCustomer.SelectedValue.Trim())

        Try
            addLog("SendMailNoticeBySales:" & sRMANo)
            dtSales = ctlRMA.getSalesMail_ARNO(sRMA_ARNO)
            For i = 0 To dtSales.Rows.Count - 1

                Dim sSubject As String = _oLanguage.getMailText("Mail", "422", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "423", ctlLanguage.eumType.Mail, LanguageID)

                'sSubject = sSubject.Replace("[$ShippingNo$]", sShippingNo)

                Dim RMA_ARNO As String = dtSales.Rows(0)("RMA_ARNO").ToString().Trim()
                Dim RMA_INVNO As String = dtSales.Rows(0)("RMA_INVNO").ToString().Trim()

                Dim SalesID As String = dtSales.Rows(0)("SalesID").ToString().Trim()
                Dim SalesEmail As String = dtSales.Rows(0)("SalesEmail").ToString().Trim()
                Dim SalesName As String = dtSales.Rows(0)("SalesName").ToString().Trim()

                Dim AssistantID As String = dtSales.Rows(0)("AssistantID").ToString().Trim()
                Dim AssistantEmail As String = dtSales.Rows(0)("AssistantEmail").ToString().Trim()
                Dim AssistantName As String = dtSales.Rows(0)("AssistantName").ToString().Trim()
                Dim sCU_Name As String = dtSales.Rows(0)("CU_NAME").ToString().Trim()
                Dim sCU_FINANCEEMAIL As String = dtSales.Rows(0)("CU_FINANCEEMAIL").ToString().Trim()

                ' addLog("SalesID:" & SalesID)
                ' addLog("SalesM:" & SalesEmail)
                ' addLog("AssM:" & AssistantEmail)
                '修改寄送信件主旨 : [RMA System notice]ARMA-150900001,A/R NO:ARBA-150900013, INVOICE(AXLA-151100121)
                sSubject = sSubject.Replace("[$RMANo$]", sRMANo)
                sSubject = sSubject.Replace("[$ARNo$]", RMA_ARNO)
                sSubject = sSubject.Replace("[$INVNo$]", RMA_INVNO)
                sSubject = sSubject.Replace("[$Customer's Name$]", sCU_Name)


                If SalesEmail.Trim() <> "" Or AssistantEmail <> "" Then
                    Dim mailTo As String = ""
                    If SalesEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & SalesEmail.Trim()
                    End If
                    If AssistantEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & AssistantEmail.Trim()
                    End If

                    If sCU_FINANCEEMAIL.Trim() <> "" Then
                        'Dim sFINANCEEMAILList As String() = sCU_FINANCEEMAIL.Split(",")
                        'For Each sMail In sFINANCEEMAILList
                        '    If sMail.Trim() <> "" Then
                        '        mailTo += sMail & ","
                        '    End If

                        'Next
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo += sCU_FINANCEEMAIL
                    End If

                    Dim sName As String = ""
                    If SalesName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & SalesName.Trim()
                    End If
                    If AssistantName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & AssistantName.Trim()
                    End If
                    sBody = sBody.Replace("[$sales Name$]", sName)

                    sBody = sBody.Replace("[$Receivable NO$]", RMA_ARNO)
                    sBody = sBody.Replace("[$Invoice NO$]", RMA_INVNO)

                    Dim oAttachmentFile As New Collection
                    If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
                    End If
                    If Me.ViewState("_AttachmentFile_02").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_02").ToString())
                    End If

                    If _FinanceEmail.Trim <> "" Then
                        mailTo = mailTo & "," & _FinanceEmail.Trim()
                    End If
                    'mailTo = "isaac.yeh@cipherlab.com.tw"
                    'mailTo = "ted@icat-tech.com.tw"
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC, oAttachmentFile)
                    'addLog(i&"-SalesID:" & SalesID)
                End If
            Next


        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 出貨通知--寄送Mail(對象:MIS人員), 補印用
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendMailNoticeByMIS(ByVal sRMANo As String, ByVal sShippingNo As String, ByVal sRMA_ARNO As String) As Boolean
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim ctlRMA As New ctlRMA
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
        Dim oMail As New ctlMail

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.cboCustomer.SelectedValue.Trim())

        Try
            addLog("SendMailNoticeBySales:" & sRMANo)
            dtSales = ctlRMA.getSalesMail_ARNO(sRMA_ARNO)
            For i = 0 To dtSales.Rows.Count - 1

                Dim sSubject As String = _oLanguage.getMailText("Mail", "422", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "423", ctlLanguage.eumType.Mail, LanguageID)

                'sSubject = sSubject.Replace("[$ShippingNo$]", sShippingNo)

                Dim RMA_ARNO As String = dtSales.Rows(0)("RMA_ARNO").ToString().Trim()
                Dim RMA_INVNO As String = dtSales.Rows(0)("RMA_INVNO").ToString().Trim()

                Dim SalesID As String = dtSales.Rows(0)("SalesID").ToString().Trim()
                Dim SalesEmail As String = dtSales.Rows(0)("SalesEmail").ToString().Trim()
                Dim SalesName As String = dtSales.Rows(0)("SalesName").ToString().Trim()

                Dim AssistantID As String = dtSales.Rows(0)("AssistantID").ToString().Trim()
                Dim AssistantEmail As String = dtSales.Rows(0)("AssistantEmail").ToString().Trim()
                Dim AssistantName As String = dtSales.Rows(0)("AssistantName").ToString().Trim()
                Dim sCU_Name As String = dtSales.Rows(0)("CU_NAME").ToString().Trim()
                Dim sCU_FINANCEEMAIL As String = dtSales.Rows(0)("CU_FINANCEEMAIL").ToString().Trim()

                ' addLog("SalesID:" & SalesID)
                ' addLog("SalesM:" & SalesEmail)
                ' addLog("AssM:" & AssistantEmail)
                '修改寄送信件主旨 : [RMA System notice]ARMA-150900001,A/R NO:ARBA-150900013, INVOICE(AXLA-151100121)
                sSubject = sSubject.Replace("[$RMANo$]", sRMANo)
                sSubject = sSubject.Replace("[$ARNo$]", RMA_ARNO)
                sSubject = sSubject.Replace("[$INVNo$]", RMA_INVNO)
                sSubject = sSubject.Replace("[$Customer's Name$]", sCU_Name)


                If SalesEmail.Trim() <> "" Or AssistantEmail <> "" Then
                    Dim mailTo As String = ""
                    Dim mailToMIS As String = ""

                    If SalesEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & SalesEmail.Trim()
                    End If
                    If AssistantEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & AssistantEmail.Trim()
                    End If

                    If sCU_FINANCEEMAIL.Trim() <> "" Then
                        'Dim sFINANCEEMAILList As String() = sCU_FINANCEEMAIL.Split(",")
                        'For Each sMail In sFINANCEEMAILList
                        '    If sMail.Trim() <> "" Then
                        '        mailTo += sMail & ","
                        '    End If

                        'Next
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo += sCU_FINANCEEMAIL
                    End If

                    Dim sName As String = ""
                    If SalesName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & SalesName.Trim()
                    End If
                    If AssistantName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & AssistantName.Trim()
                    End If
                    sBody = sBody.Replace("[$sales Name$]", sName)

                    sBody = sBody.Replace("[$Receivable NO$]", RMA_ARNO)
                    sBody = sBody.Replace("[$Invoice NO$]", RMA_INVNO)

                    Dim oAttachmentFile As New Collection
                    If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
                    End If
                    If Me.ViewState("_AttachmentFile_02").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_02").ToString())
                    End If

                    If _FinanceEmail.Trim <> "" Then
                        mailTo = mailTo & "," & _FinanceEmail.Trim()
                    End If
                    'mailToMIS = "hugh.wang@cipherlab.com.tw"
                    'mailTo = "ted@icat-tech.com.tw"
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        mailToMIS = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody & "Actual Mail TO:" & mailTo, mailToMIS, _MailCC, oAttachmentFile)
                    'addLog(i&"-SalesID:" & SalesID)

                End If
            Next


        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 顯示選擇EndUser客戶的相關信息
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryShippingCustomerDetailEU()
        Dim DtCustomerEU As DataTable
        Dim i As Integer

        If Me.ViewState("CustomerEU") IsNot Nothing Then

            Me.UI_txtCustomer.Text = Me.cboCustomerEU.SelectedItem.Text.Trim()

            DtCustomerEU = Me.ViewState("CustomerEU")
            Dim dvCustomerEU As DataView = DtCustomerEU.DefaultView

            UI_cboAddress.Items.Clear()
            '修改寫法如果有單引號字元會出錯 by buck modify 2025.08.20
            'Mail:FRMA-2025080008 無法結案
            Dim aryCustomerEU = DtCustomerEU.AsEnumerable.ToList().Where(Function(x) x.Field(Of String)("CU_NO").Replace("'", "^") = cboCustomerEU.SelectedValue.Trim().Replace("'", "^"))

            If aryCustomerEU.Count() > 0 Then
                For Each item In aryCustomerEU
                    Me.UI_cboAddress.Items.AddRange({New ListItem(item.Field(Of String)("RMA_EUADDRESS").Trim(), item.Field(Of String)("RMA_EUADDRESS").Trim())})
                Next
            End If

            'dvCustomerEU.RowFilter = "CU_NO = '" + cboCustomerEU.SelectedValue.Trim() + "'"
            'If dvCustomerEU.Count > 0 Then
            '    For i = 0 To dvCustomerEU.Count - 1
            '        Dim oListItem1 = New ListItem
            '        oListItem1.Text = dvCustomerEU(i)("RMA_EUADDRESS").ToString().Trim()
            '        oListItem1.Value = dvCustomerEU(i)("RMA_EUADDRESS").ToString().Trim()
            '        Me.UI_cboAddress.Items.Add(oListItem1)
            '    Next
            'End If
        End If
    End Sub

    Protected Sub cboCustomerEU_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomerEU.SelectedIndexChanged
        Call QueryShippingCustomerDetailEU() 'show客戶其他資料
        Call QueryDataSerial(0) '查詢此可以可以出貨的Serial Number
        '若EndUser為空 則重抓Partner地址
        If cboCustomerEU.SelectedValue.Trim() = "" Then
            Call QueryShippingCustomerDetail() 'show客戶其他資料
        End If
    End Sub

    Protected Sub cboCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Call QueryShippingCustomerDetail() 'show客戶其他資料
        Call QueryDataSerial(0) '查詢此可以可以出貨的Serial Number
        QueryShippingCustomerEU()
    End Sub

    Protected Sub UI_dvSerial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSerial.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "021", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_Mark As Label = e.Row.FindControl("UI_Mark")
            Dim UI_RMADID As Label = e.Row.FindControl("UI_RMADID")
            Dim UI_Check As CheckBox = e.Row.FindControl("UI_Check")
            Dim hsSelectID As Hashtable = ViewState("hsSelectID")
            Dim UI_RMADPARTSN As Label = e.Row.FindControl("UI_RMADPARTSN")
            Dim UI_RMAD_SERIALNO As Label = e.Row.FindControl("UI_RMAD_SERIALNO")

            UI_Check.Attributes.Add("onclick", "JScheckItem();")

            If UI_RMADPARTSN.Text <> "" Then
                UI_RMAD_SERIALNO.Text = UI_RMADPARTSN.Text
            End If

            If UI_Mark.Text.Trim = "9" Then
                e.Row.Visible = False
            Else
                If hsSelectID.ContainsKey(UI_RMADID.Text.Trim()) Then
                    UI_Check.Checked = True
                End If
            End If
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

    Protected Sub UI_dvSerial_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSerial.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryDataSerial(iPageIndex)
        'Me.ajModalProgress.Show()
    End Sub

    ''' <summary>
    ''' 無資料時建立一筆虛擬資料,作用是秀出表頭
    ''' </summary>
    ''' <param name="dtSerial"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial01(ByVal dtSerial As RmaDTO.Shipment_DetailDataTable) As RmaDTO.Shipment_DetailDataTable
        Dim drSerial As RmaDTO.Shipment_DetailRow = dtSerial.NewShipment_DetailRow
        Dim oGuid As Guid = Guid.NewGuid
        Try
            drSerial.RMASMD_oldMark = "9"
            drSerial.RMASMD_LOWESTDISCOUNT = 0

            dtSerial.AddShipment_DetailRow(drSerial)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtSerial
    End Function

    ''' <summary>
    ''' 新增選取的資料到dtSerial
    ''' </summary>
    ''' <param name="_dvSerial"></param>
    ''' <param name="dtSerial"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial02(ByVal _dvSerial As DataView, ByVal dtSerial As RmaDTO.Shipment_DetailDataTable) As RmaDTO.Shipment_DetailDataTable
        Dim drSerial As RmaDTO.Shipment_DetailRow = dtSerial.NewShipment_DetailRow

        Try
            drSerial.RMASMD_RMASMID = _dvSerial.Item(0)("RMASMD_RMASMID").ToString().Trim()
            drSerial.RMASMD_ID = _dvSerial.Item(0)("RMASMD_ID").ToString().Trim()
            drSerial.RMASMD_RMANO = _dvSerial.Item(0)("RMASMD_RMANO").ToString().Trim()
            drSerial.RMASMD_RMADID = _dvSerial.Item(0)("RMASMD_RMADID").ToString().Trim()
            drSerial.RMASMD_SERIALNO = _dvSerial.Item(0)("RMASMD_SERIALNO").ToString().Trim()
            drSerial.RMASMD_PARTNO = _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim()

            If _dvSerial.Item(0)("RMASMD_MODELNO").ToString().Trim() <> "" Then
                drSerial.RMASMD_MODELNO = _dvSerial.Item(0)("RMASMD_MODELNO").ToString().Trim()
            End If

            If _dvSerial.Item(0)("RMARSD_LABORCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_LABORCOST = _dvSerial.Item(0)("RMARSD_LABORCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_MATERIALCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_MATERIALCOST = _dvSerial.Item(0)("RMARSD_MATERIALCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_QUOTE").ToString().Trim() <> "" Then
                drSerial.RMARSD_QUOTE = _dvSerial.Item(0)("RMARSD_QUOTE").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim() <> "" Then
                drSerial.RMASMD_PARTNO = _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim()
            End If

            If _dvSerial.Item(0)("RMARSD_CURRENCYCODE").ToString().Trim() <> "" Then
                drSerial.RMARSD_CURRENCYCODE = _dvSerial.Item(0)("RMARSD_CURRENCYCODE").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_CURRENCYRATE").ToString().Trim() <> "" Then
                drSerial.RMARSD_CURRENCYRATE = _dvSerial.Item(0)("RMARSD_CURRENCYRATE").ToString().Trim()
            End If


            If _dvSerial.Item(0)("RMASMD_oldRMAID").ToString().Trim() <> "" Then
                drSerial.RMASMD_oldRMAID = _dvSerial.Item(0)("RMASMD_oldRMAID").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldLABORCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldLABORCOST = _dvSerial.Item(0)("RMARSD_oldLABORCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldMATERIALCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldMATERIALCOST = _dvSerial.Item(0)("RMARSD_oldMATERIALCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldQUOTE").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldQUOTE = _dvSerial.Item(0)("RMARSD_oldQUOTE").ToString().Trim()
            End If

            drSerial.RMASMD_LOWESTDISCOUNT = _dvSerial.Item(0)("RMASMD_LOWESTDISCOUNT").ToString().Trim()


            drSerial.RMASMD_oldMark = "0"

            dtSerial.AddShipment_DetailRow(drSerial)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtSerial
    End Function

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_CheckGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdSave.Enabled = False
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_Mark As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_Mark")
                Dim UI_RMADID As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_RMADID")
                hsSelectID.Remove(UI_RMADID.Text.Trim())
                UI_Check.Checked = sender.Checked

                If UI_Check.Checked = True And UI_Mark.Text.Trim() <> "9" Then
                    hsSelectID.Add(UI_RMADID.Text.Trim(), UI_RMADID.Text.Trim())
                End If
            End If
        Next
        If hsSelectID.Count > 0 Then
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdSave.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' 單一勾選項目
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdSave.Enabled = False
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_RMADID As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_RMADID")
                hsSelectID.Remove(UI_RMADID.Text.Trim())

                If UI_Check.Checked = True Then
                    hsSelectID.Add(UI_RMADID.Text.Trim(), UI_RMADID.Text.Trim())
                End If
            End If
        Next
        If hsSelectID.Count > 0 Then
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdSave.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' 勾選項目
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckedItem()
        Dim i As Integer = 0
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdSave.Enabled = False
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_RMADID As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_RMADID")
                hsSelectID.Remove(UI_RMADID.Text.Trim())

                If UI_Check.Checked = True Then
                    hsSelectID.Add(UI_RMADID.Text.Trim(), UI_RMADID.Text.Trim())
                End If
            End If
        Next
        If hsSelectID.Count > 0 Then
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdSave.Enabled = True
        End If
    End Sub

    Protected Sub UI_linkRMANO_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim linkRMANO As LinkButton = sender

        '檢核是否有RMANO已勾選
        Dim isChecked As Boolean = False
        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_linkRMANO As LinkButton = Me.UI_dvSerial.Rows(i).FindControl("UI_linkRMANO")

                If UI_linkRMANO.Text.Trim.ToLower() = linkRMANO.Text.ToLower() And UI_Check.Checked = True Then
                    isChecked = True
                    Exit For
                End If
            End If
        Next


        '處理RMANO 勾選或反勾選
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdSave.Enabled = False
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_RMADID As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_RMADID")
                Dim UI_linkRMANO As LinkButton = Me.UI_dvSerial.Rows(i).FindControl("UI_linkRMANO")

                hsSelectID.Remove(UI_RMADID.Text.Trim())
                If UI_linkRMANO.Text.Trim.ToLower() = linkRMANO.Text.ToLower() Then
                    UI_Check.Checked = Not isChecked
                End If


                If UI_Check.Checked = True Then
                    hsSelectID.Add(UI_RMADID.Text.Trim(), UI_RMADID.Text.Trim())
                End If
            End If
        Next
        If hsSelectID.Count > 0 Then
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdSave.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' 取得 指派的維修中心 相關的幣別資料
    ''' </summary>
    ''' <param name="COMP_NO">公司代碼</param>
    ''' <remarks></remarks>
    Private Function QueryByCompany_Currency(ByVal COMP_NO As String) As String
        Dim CURRENCY_CODE As String = ""
        Dim blnFlag_Cal As Boolean = False
        Dim iTotalManAmt As Decimal = 0
        Dim iMaterial As Decimal = 0

        Dim oCompany As New ctlCompany
        Dim dtCurrency As New CompanyDTO.vwCompany_CurrencyDataTable

        dtCurrency = oCompany.QueryByCurrency(COMP_NO)
        If dtCurrency.Rows.Count > 0 Then
            Dim dr As CompanyDTO.vwCompany_CurrencyRow = dtCurrency.Rows(0)
            CURRENCY_CODE = dr.CURRENCY_CODE.Trim()              '指派的維修中心-幣別
            ''''Me.UI_txtAssigeCurrencyRate.Text = dr.CURRENCY_RATE.ToString()          '指派的維修中心-兌美金匯率
        End If

        Return CURRENCY_CODE
    End Function

#Region "異動庫存"
    Public Function runSP_SHP_RMA_STOCK(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
        Dim retval As String = ""

        Dim oCommand As OracleCommand = oConn.Command
        Try

            oCommand.CommandText = "TX_RMA_OUTSTOCK"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
            oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
            oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
            oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
            oCommand.Parameters("RES").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("RES").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

    'Public Function runSP_SHP_RMA_INV(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function


    'Public Function runSP_SHP_RMA_INV_CN(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_CN"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function
    ''新增AUS的維修中心倉庫扣帳 MODI BY ANGEL ON 20160120
    'Public Function runSP_SHP_RMA_INV_AU(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_AU"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function


    ''新增AUS的維修中心倉庫扣帳 MODI BY ANGEL ON 20160120
    'Public Function runSP_SHP_RMA_INV_AUS(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_AUS"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function


    ''新增BYTE的維修中心倉庫扣帳 MODI BY ANGEL ON 20190919
    'Public Function runSP_SHP_RMA_INV_BYTE(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_BYTE"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function

    'Public Function runSP_SHP_RMA_INV_JP_BYTE_MPLUS(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_JP_BYTE_MPLUS"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function

    'Public Function runSP_SHP_RMA_INV_AU_LAPTOP_KINGS(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_AU_LAPTOP_KINGS"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function

    'Public Function runSP_SHP_RMA_INV_NZ_PB_TECH(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_NZ_PB_TECH"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function


    'Public Function runSP_SHP_RMA_INV_UK_FALA(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_UK_FALA"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function

    'Public Function runSP_SHP_RMA_INV_US_CL_MPLUS(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMA_NO_IN As String, ByVal RMASMD_RMADID As String, ByVal RMASMD_SERIALNO As String) As String
    '    Dim retval As String = ""

    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try

    '        oCommand.CommandText = "TX_RMA_INV_US_CL_MPLUS"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure

    '        oCommand.Parameters.Add("RMA_NO_IN", OracleType.NVarChar).Value = RMA_NO_IN
    '        oCommand.Parameters("RMA_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RMAD_ID_IN", OracleType.NVarChar).Value = RMASMD_RMADID
    '        oCommand.Parameters("RMAD_ID_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("SERIAL_NO_IN", OracleType.NVarChar).Value = RMASMD_SERIALNO
    '        oCommand.Parameters("SERIAL_NO_IN").Direction = ParameterDirection.Input

    '        oCommand.Parameters.Add("RES", OracleType.NVarChar, 200)
    '        oCommand.Parameters("RES").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()

    '        retval = oCommand.Parameters("RES").Value

    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try

    '    Return retval
    'End Function
#End Region

#Region "產生報表"
    Private Sub Print_Inovice(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable

        Dim iRMARQ_QUOTE As Double = 0.0
        'Throw New Exception(sRepairCenter.Trim())    'fairy
        dtRptINVOICE = ctlReport.qryRPTInovice(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("RMAD_SEQ") = (i + 1).ToString()
            iRMARQ_QUOTE = iRMARQ_QUOTE + Convert.ToDouble(dtRptINVOICE.Rows(i)("RMARQ_QUOTE"))
        Next

        Dim SayTotal As String = GetNumberWord(iRMARQ_QUOTE)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("SayTotal") = SayTotal
        Next

        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptINVOICE)

        Dim LanguageID As String = ""
        If dtRptINVOICE.Rows.Count > 0 Then
            Dim oLoginInfo As New ctlLoginInfo
            LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptINVOICE.Rows(0)("rmad_rmano").ToString())
        End If

        Call Print_Inovice(oDsReport, LanguageID)
    End Sub

    Private Sub Print_Inovice(ByVal oDsReport As DataSet, ByVal sLanguageID As String)
        Dim sReportToPDF_Invoice As String = "Invoice_" & oCommon.GetRandomizeNum() & ".pdf"
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sLanguageID = "003" Then
            ReportDoc.Load(Server.MapPath("Report\rptInovice_jp.rpt"))
        Else
            ReportDoc.Load(Server.MapPath("Report\rptInovice.rpt"))
        End If
        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF_Invoice)
        Me.ViewState("_AttachmentFile_01") = _Reoprt_FilePath & sReportToPDF_Invoice
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_01") = ConfigureExportToPdf(sReportToPDF_Invoice)
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

    Private Sub Print_AD(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptAD As New RmaDTO.RPTADDataTable

        dtRptAD = ctlReport.qryRPTAD(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
        For i = 0 To dtRptAD.Rows.Count - 1
            dtRptAD.Rows(i)("SEQ") = (i + 1).ToString()
        Next

        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptAD)

        Dim LanguageID As String = ""
        If dtRptAD.Rows.Count > 0 Then
            Dim oLoginInfo As New ctlLoginInfo
            LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptAD.Rows(0)("RMA單號").ToString())
        End If

        Call Print_AD(oDsReport, LanguageID)
    End Sub

    Private Sub Print_AD(ByVal oDsReport As DataSet, ByVal sLanguageID As String)
        Dim sReportToPDF_AD As String = "AD_" & oCommon.GetRandomizeNum() & ".pdf"
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sLanguageID = "003" Then
            ReportDoc.Load(Server.MapPath("Report\rptAR_jp.rpt"))
        Else
            ReportDoc.Load(Server.MapPath("Report\rptAR.rpt"))
        End If
        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF_AD)
        Me.ViewState("_AttachmentFile_02") = _Reoprt_FilePath & sReportToPDF_AD
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_02") = ConfigureExportToPdf(sReportToPDF_AD)
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

    'Public Function ConfigureExportToPdf(ByVal sReportToPDF As String) As String
    '    Dim retval As String = _Reoprt_FilePath & sReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & sReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    ' '' ''Dim sScript As String = ""
    '    ' '' ''sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    ' '' ''sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    '' '' ''sScript = sScript & "window.location.href='Shipping_List.aspx';" & vbCrLf
    '    ' '' ''sScript = sScript & "</script>" & vbCrLf
    '    ' '' ''Response.Write(sScript)

    '    Return retval
    'End Function

    Private Sub Print_PackingList(ByVal ShippingNo As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable
        Dim EndUser As Boolean = False

        Try
            dtRequest = ctlReport.qryShpRmaNo(Session("_LanguageID").ToString().Trim(), ShippingNo)
            For i = 0 To dtRequest.Rows.Count - 1
                Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
                Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow

                If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
                If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
                If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
                If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
                If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
                If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
                If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
                If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
                If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
                If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
                If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
                If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
                If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
                If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
                If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
                If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

                If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
                If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
                If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
                If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

                If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
                If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
                If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
                If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
                If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()

                If dr.IsRMASH_CSTMPNull = False Then drReport.RMASH_CSTMP = dr.RMASH_CSTMP.ToString().Trim()
                If dr.IsRMASH_SHIPPINGNONull = False Then drReport.RMASH_SHIPPINGNO = dr.RMASH_SHIPPINGNO.ToString().Trim()
                If dr.IsRMASH_TRACKINGNONull = False Then drReport.RMASH_TRACKINGNO = dr.RMASH_TRACKINGNO.ToString().Trim()

                drReport.SeqID = i + 1
                If Not (dr.IsRMA_EUCOMPANYNull) Then
                    EndUser = True
                End If
                dtReport.AddRequestReportRow(drReport)
            Next
            Dim oDsReport As New DataSet
            oDsReport.Tables.Add(dtReport)

            Dim LanguageID As String = ""
            If dtReport.Rows.Count > 0 Then
                Dim oLoginInfo As New ctlLoginInfo
                LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtReport.Rows(0)("RMA_NO").ToString())
            End If

            Call Print_PackingList(oDsReport, EndUser, LanguageID)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Private Sub Print_PackingList(ByVal oDsReport As DataSet, ByVal EndUser As Boolean, ByVal sLanguageID As String)
        Dim sReportToPDF As String = "PackingList_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        'If (EndUser) Then
        '    ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp_EndUser.rpt"))
        'Else
        '    ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp.rpt"))
        'End If
        If (EndUser) Then
            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp_EndUser_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp_EndUser.rpt"))
            End If
        Else
            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp.rpt"))
            End If
        End If

        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF)
        Me.ViewState("_AttachmentFile_03") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_03") = ConfigureExportToPdf(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        ReportDoc.Close()
    End Sub
#End Region

    Public Shared Function GetNumberWord(ByVal dValue As Double) As String
        Dim sValue As String = dValue.ToString("#.00")
        Dim sValueBeforeDot As String = sValue.Substring(0, sValue.IndexOf("."))
        Dim sValueAfterDot As String = sValue.Substring(sValue.IndexOf(".") + 1, sValue.Length - sValue.IndexOf(".") - 1)
        Dim sResult As String
        If sValueAfterDot = "00" Then
            sResult = Get2NumberWord(sValueBeforeDot) & " ONLY"
        Else
            sResult = Get2NumberWord(sValueBeforeDot) & " AND CENTS " & Get2NumberWord(sValueAfterDot) & " ONLY"
        End If
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = "SAY US DOLLAR " & sResult
        Return (sResult)
    End Function

    Protected Shared Function Get2NumberWord(ByVal sNumber As String) As String
        If sNumber = "" Then
            sNumber = "0"
        End If
        Dim iLength As Integer = sNumber.Length
        Dim iNumber As Integer = Convert.ToInt32(sNumber)
        If iLength < 3 AndAlso iLength > 0 Then
            If iNumber = 0 Then
                Return ("")
            ElseIf iNumber < 20 Then
                Return (conNumberWordLess20(iNumber))
            Else
                Dim iTenNumber As Integer, iOneNumber As Integer
                iTenNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 1))
                iOneNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 1, 1))
                Return (conNumberWordTen(iTenNumber) + "-" + conNumberWordLess20(iOneNumber))
            End If
        ElseIf iLength = 3 Then
            Dim iHundredNumber As Integer, iBelowHundredNumber As Integer
            iHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 1))
            iBelowHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 2))
            If iBelowHundredNumber = 0 Then
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED")
            ElseIf iHundredNumber = 0 Then
                Return ("AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            Else
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            End If
        ElseIf iLength > 3 And iLength < 7 Then
            Dim iThousandNumber As Integer, iBelowThousandNumber As Integer
            iThousandNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 3))
            iBelowThousandNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 3))
            Return (Get2NumberWord(iThousandNumber.ToString()) & " THOUNSAND " & IIf(iBelowThousandNumber < 100 And iBelowThousandNumber > 0, "AND ", "") & Get2NumberWord(iBelowThousandNumber.ToString()))
        ElseIf iLength > 6 AndAlso iLength < 10 Then
            Dim iMillionNumber As Integer, iBelowMillionNumber As Integer
            iMillionNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 6))
            iBelowMillionNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 6, 6))
            Return (Get2NumberWord(iMillionNumber.ToString()) & " MILLION " & IIf(iBelowMillionNumber < 100000 And iBelowMillionNumber > 0, "AND ", "") & Get2NumberWord(iBelowMillionNumber.ToString()))
        Else
            Return ("")
        End If

    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim retvalAR As String = ""
        Dim sRMA_ARNO As String = ""
        Dim sShippingNO As String = "SHP-2025110039"
        Dim sRepairCenter As String = "CLHQ"
        Dim sCurrencyCode As String = "USD"
        Dim sCustomer As String = "DT728"
        Dim sRMA_NO As String = "ARMA-2025100089"
        Dim sUserID As String = "Howard.Chen"

        Dim oShipping As New ctlRMA.Shipping

        '分二次執行 一次先產生AR  再產生報表
        '產生AR----------------------------------------------------------------------------------------------------
        retvalAR = oShipping.runSP_SHP_INS_AR(sCustomer, sShippingNO, sCurrencyCode, sUserID, sRepairCenter)
        addLog(retvalAR)
        '-----------------------------------------------------------------------------------------------------------

        ''產生報表----------------------------------------------------------------------------------------------------
        'Me.ViewState("_AttachmentFile_01") = ""
        'Me.ViewState("_AttachmentFile_02") = ""
        'Try
        '    addLog("InvoiceShip:" & sShippingNO)
        '    Call Print_Inovice(sShippingNO, sRepairCenter, sRMA_ARNO)   'fairy
        'Catch ex As Exception
        '    addLog("Error Invoice")
        'End Try
        'Try
        '    addLog("ADCenter:" & sRepairCenter)
        '    Call Print_AD(sShippingNO, sRepairCenter.Trim(), sRMA_ARNO)    'fairy
        'Catch ex As Exception
        '    addLog("Error AD")
        'End Try

        ''RMA單號 + Shipping單號 + AR.NO
        'addLog("SendMailNoticeBySales")
        'Call SendMailNoticeBySales(sRMA_NO, sShippingNO, sRMA_ARNO)
        ''----------------------------------------------------------------------------------------------------------------

        ''扣庫存---------------------------------------------------------------------------------------------------------
        'Dim oConn As New ICAT_OracleDAO.Connection
        'oConn.Open()
        'Dim RMASMD_RMADID As String = "f6fd56e5-ccad-482e-bf17-0266002ba591"
        'Dim RMASMD_RMANO As String = "FRMA-2021040032"
        'Dim RMASMD_SERIALNO As String = "FJ120CA000209"
        ''Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)

        ''已統一 共用一個Function
        'Dim retMessage As String = runSP_SHP_RMA_STOCK(oConn, RMASMD_RMANO, RMASMD_RMADID, RMASMD_SERIALNO)

        'If retMessage.Trim() <> "OK" Then
        '    Throw New Exception(retMessage.Trim())
        'End If
        '---------------------------------------------------------------------------------------------------------------------

        ''Test Message- 搭配Updatepanel 無效-----------------------------------------------------------------------------------------------------

        '''獲取彈出對話框的按鈕
        ''Dim button As Button = CType(sender, Button)
        ''ScriptManager.RegisterStartupScript(button, button.GetType(), "alert", "alert('添加信息成功！');", True)

        ''Throw New Exception(RMASMD_RMANO & "," & RMASMD_RMADID & "," & RMASMD_SERIALNO)
        '-----------------------------------------------------------------------------------------------------------------------
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'add by hugh 20240709, 用於重送AR/INVOICE (Batch)
        '需先取得以下資訊
        'Dim retvalAR As String = "ARBA-240700054,ARBA-240700054,ARBA-240400084,ARBA-240700053,ARBA-240400090,ARBA-240400081,ARBA-240400080,ARBA-240400079,ARBA-240400089,ARBA-240400088,ARBA-240700056,ARBA-240400087,ARBA-240400083,ARBA-240400082,ARBA-240400086,ARBA-240400085,ARBA-240400078,ARBA-240700055"
        'Dim retvalRMA As String = "CAULKR-2024020003,CAULKR-2024020003,CAULKR-2024020018,CAULKR-2024020022,CAULKR-2024030001,CAULKR-2024030002,CAULKR-2024030003,CAULKR-2024030005,CAULKR-2024030008,CAULKR-2024030009,CAULKR-2024030010,CAULKR-2024030011,CAULKR-2024030012,CAULKR-2024030013,CAULKR-2024030014,CAULKR-2024030015,CAULKR-2024030016,CAULKR-2024030025"

        '再寄送
        Dim sRepairCenter As String = "CLHQ"
        Dim sShippingNO As String = "SHP-2025110039"
        Dim retvalAR = "ARBA-251100119"
        Dim retvalRMA As String = "ARMA-2025100089"
        Dim sARNo As String = ""

        Dim arrAR() As String = retvalAR.Trim().Split(",")
        Dim arrRMA() As String = retvalRMA.Trim().Split(",")

        Try
            For i = 0 To arrAR.Length - 1

                Me.ViewState("_AttachmentFile_01") = ""
                Me.ViewState("_AttachmentFile_02") = ""

                Dim sRMA_ARNO As String = arrAR(i).ToString().Trim()
                Dim sRMA_RMANO As String = arrRMA(i).ToString().Trim()

                sARNo = sRMA_ARNO

                Try
                    Call Print_Inovice(sShippingNO, sRepairCenter, sRMA_ARNO)   'fairy
                Catch ex As Exception
                    addLog("Error Invoice Gen")
                End Try
                Try
                    Call Print_AD(sShippingNO, sRepairCenter, sRMA_ARNO)    'fairy
                Catch ex As Exception
                    addLog("Error AD Gen")
                End Try

                'Call SendMailNoticeByMIS('ARMA-2015030071', 'SHP-2016010097', 'ARBA-160100047')
                'Call SendMailNoticeByMIS(sRMA_RMANO, shippingNo, sRMA_ARNO)
                Call SendMailNoticeBySales(sRMA_RMANO, sShippingNO, sRMA_ARNO)  '補寄正式信

                addLog("Resent AR/INV Mail:" & sShippingNO & "," & sRepairCenter & "," & sRMA_ARNO & "," & sRMA_RMANO)
            Next


            'by Hugh, 重寄 packing list
            '注意, 寄送前需先選擇對應RMA No. 查詢, 新增後, 挑選正式的 Customer --> 因為會帶到 Mail Body
            'Try					
            '	Call Print_PackingList(shippingNo)
            'Catch ex As Exception
            '	addLog("Error Packing List Gen")
            'End Try

            'Dim oShipping As New ctlRMA.Shipping
            'Dim dtShipping As New RmaDTO.tmpShipping_DetailDataTable    
            'Dim curDateTime As Date = Date.Now			

            'shippingNo 的Id			
            'dtShipping = oShipping.QueryByRMA_ShippingDetail("b5a52541-14f3-46f0-897b-e5fa00e3d0fb")
            'Call Shipping_DataBind(dtShipping)

            'Call SendMail_NoticeByMIS(dtShipping, curDateTime, sARNo)

            'addLog("Resent PackingList(CW) Mail:" & shippingNo & "," & repairCenter & "," &  sARNo)						

        Catch ex As Exception
            addLog("Error Resent AR/INV:" & ex.Message)
            Throw ex
        End Try

    End Sub

#Region "AJAX一鍵重送AR"
    Public Class csResponse
        Public Property Status As Boolean
        Public Property ErrorMessage As String
    End Class
    Public Class Result
        Public Property Bool As Boolean
        Public Property Lang As String
        Public Property TempFilePath As String
        Public Property Msg As String
        Public Property PathData As TypeData
    End Class

    Public Class TypeData
        Public Property INVO_FilePath As String
        Public Property AR_FilePath As String
    End Class

    <WebMethod(EnableSession:=True)>
    Public Shared Function CreateAR_INVOICE2(ByVal RMA_NO As String, ByVal RepairCenter As String) As String

        Try
            If String.IsNullOrWhiteSpace(RMA_NO) Then

                Return JsonConvert.SerializeObject(New With {
                    .Status = False,
                    .Msg = "RMA_NO不可為空"
                })

            End If

            Dim oCompany As New ctlCompany
            Dim oShipping As New ctlRMA.Shipping
            Dim oShipment As New ctlRMA.Shipment
            Dim oRMAStatus As New ctlRMA.RMAStatus

            Dim queryMonth As Integer = Convert.ToInt32(ConfigurationManager.AppSettings("QueryDate_Month"))

            Dim dtShipping As DataTable = oShipping.QueryByRMA_ShippingList(RepairCenter, "", RMA_NO, "", "", "", Date.Now.AddMonths(queryMonth).ToShortDateString(), Date.Now.ToShortDateString())
            If dtShipping Is Nothing OrElse dtShipping.Rows.Count = 0 Then
                Return JsonConvert.SerializeObject(New With {
                    .Status = False,
                    .Msg = "查無Shipping資料"
                })

            End If

            Dim RMASH_ID As String = dtShipping.Rows(0).Item("RMASH_ID").ToString()
            Dim dtCompany = oCompany.QueryAll()
            Dim dtShippingM = oShipping.QueryByRMA_Shipping(RMASH_ID)
            Dim dtShippingD = oShipping.QueryByRMA_ShippingDetail(RMASH_ID)
            Dim dtShipment = oShipment.Query_ShippingToShipmentDetail(RMASH_ID)

            If dtShipment Is Nothing OrElse dtShipment.Rows.Count = 0 Then
                Return JsonConvert.SerializeObject(New With {
                    .Status = False,
                    .Msg = "查無Shipment資料"
                })

            End If

            Dim dtStatusPoint = oRMAStatus.QueryPointByDetail(dtShippingD.Rows(0).Item("RMASMD_RMADID").ToString())
            Dim sFrom As String = dtShippingM.Rows(0).Item("RMASH_FROM").ToString()
            Dim sRepairCenter As String = dtCompany.AsEnumerable().Where(Function(x) x("COMP_NO").ToString() = sFrom).Select(Function(y) y("COMP_NO").ToString()).FirstOrDefault()
            'Dim sRepairCenter As String = ""
            'For Each row As DataRow In dtCompany.Rows
            '    If row("COMP_NO").ToString() = sFrom Then
            '        sRepairCenter = row("COMP_NO").ToString()
            '        Exit For
            '    End If
            'Next

            Dim sShippingNO As String = dtShippingM.Rows(0).Item("RMASH_SHIPPINGNO").ToString()
            Dim sCustomer As String = dtShippingM.Rows(0).Item("RMASH_CUNO").ToString()
            Dim sCurrencyCode As String = dtShipment.Rows(0).Item("RMARSD_CURRENCYCODE").ToString()
            Dim sUserID As String = dtStatusPoint.Rows(0).Item("CLOSE_AD").ToString()
            Dim arno As String = dtShipment.AsEnumerable().Select(Function(x) x("RMA_ARNO").ToString()).FirstOrDefault()
            'Dim arno As String = ""
            'If dtShipment.Rows.Count > 0 Then
            '    arno = dtShipment.Rows(0).Item("RMA_ARNO").ToString()
            'End If

            ' 尚未產生AR
            If String.IsNullOrWhiteSpace(arno) Then

                Dim retvalAR = oShipping.runSP_SHP_INS_AR(sCustomer, sShippingNO, sCurrencyCode, sUserID, sRepairCenter)

            End If

            ' 重新撈資料
            dtShipment = oShipment.Query_ShippingToShipmentDetail(RMASH_ID)

            Dim dict As New Dictionary(Of String, String)
            For Each row As DataRow In dtShipment.Rows

                Dim rmano As String = row("RMASMD_RMANO").ToString()
                Dim arNo2 As String = row("RMA_ARNO").ToString()

                If Not dict.ContainsKey(rmano) Then
                    dict.Add(rmano, arNo2)
                End If

            Next

            For Each item In dict

                Dim sRMA_RMANO As String = item.Key
                Dim sRMA_ARNO As String = item.Value

                Dim result = Print_Inovice_ForAjax(sShippingNO, sRepairCenter, sRMA_ARNO)

                If result.Bool = True Then
                    result = Print_AD_ForAjax(sShippingNO, sRepairCenter, sRMA_ARNO)
                End If

            Next
        Catch ex As Exception
            Return JsonConvert.SerializeObject(New With {
                .Status = False,
                .Msg = ex.ToString()
            })

        End Try

        Return JsonConvert.SerializeObject(New With {
            .Status = True,
            .Msg = ""
        })

    End Function

    '''' <summary>
    '''' 暫時使用一鍵重送AR，未加入判斷防呆，錯誤直接alert訊息
    '''' </summary>
    '''' <param name="RMA_NO"></param>
    '''' <param name="RepairCenter"></param>
    '''' <returns></returns>
    '<Web.Services.WebMethod(EnableSession:=True)>
    'Public Shared Function CreateAR_INVOICE(ByVal RMA_NO As String, ByVal RepairCenter As String) As String
    '    Dim response As New csResponse()
    '    Dim result As New Result()
    '    Dim oCompany As New ctlCompany
    '    Dim oShipping As New ctlRMA.Shipping
    '    Dim oShipment As New ctlRMA.Shipment
    '    Dim oRMAStatus As New ctlRMA.RMAStatus
    '    Dim dtShipping As DataTable
    '    Dim _QueryDate_Month As String = ConfigurationManager.AppSettings("QueryDate_Month")

    '    Dim sRMA_NO As String = RMA_NO
    '    Try
    '        If RMA_NO <> "" Then
    '            dtShipping = oShipping.QueryByRMA_ShippingList(RepairCenter, "", sRMA_NO, "", "", "", Date.Now.AddMonths(_QueryDate_Month).ToShortDateString(), Date.Now.ToShortDateString())
    '            Dim RMASH_ID As String = dtShipping.Rows(0).Item("RMASH_ID").ToString()

    '            Dim dtCompany = oCompany.QueryAll()
    '            Dim dtShippingM = oShipping.QueryByRMA_Shipping(RMASH_ID)
    '            Dim dtShippingD = oShipping.QueryByRMA_ShippingDetail(RMASH_ID)
    '            Dim dtShipment = oShipment.Query_ShippingToShipmentDetail(RMASH_ID)
    '            Dim dtStatusPoint = oRMAStatus.QueryPointByDetail(dtShippingD.Rows(0).Item("RMASMD_RMADID").ToString())

    '            Dim sFrom As String = dtShippingM.Rows(0).Item("RMASH_FROM").ToString()
    '            Dim sRepairCenter = dtCompany.Where(Function(x) x.COMP_NO = sFrom).Select(Function(y) y.COMP_NO).First()
    '            Dim sShippingNO = dtShippingM.Rows(0).Item("RMASH_SHIPPINGNO").ToString()
    '            Dim sCustomer = dtShippingM.Rows(0).Item("RMASH_CUNO").ToString()
    '            Dim sCurrencyCode = dtShipment.Rows(0).Item("RMARSD_CURRENCYCODE").ToString()
    '            Dim sUserID = dtStatusPoint.Rows(0).Item("CLOSE_AD").ToString()

    '            If dtShipment.Rows.Count > 0 Then
    '                Dim arno As String = dtShipment.AsEnumerable().Select(Function(x) x.Field(Of String)("RMA_ARNO")).First()
    '                If arno = "" Then
    '                    '分二次執行 一次先產生AR  再產生報表
    '                    '產生AR----------------------------------------------------------------------------------------------------
    '                    Dim retvalAR = oShipping.runSP_SHP_INS_AR(sCustomer, sShippingNO, sCurrencyCode, sUserID, sRepairCenter)
    '                    'addLog(retvalAR)
    '                    '-----------------------------------------------------------------------------------------------------------
    '                End If
    '                '再寄送----------------------------------------------------------------------------------------------------
    '                Dim dict As New Dictionary(Of String, String)
    '                Dim sShiooingNo As String = ""
    '                '產生AR後一定要重新撈ARNO資料。如果之前產生過，就跳過產生AR步驟往下執行。
    '                dtShipment = oShipment.Query_ShippingToShipmentDetail(RMASH_ID)
    '                dtShipment.ToList().ForEach(Sub(item)
    '                                                dict.Add(item.RMASMD_RMANO, item.RMA_ARNO)
    '                                            End Sub)
    '                Dim sARNo As String = ""
    '                Dim retvalRMA As String = String.Join(",", dict.ToList().Select(Function(x) x.Key).ToList())
    '                Dim retvalAR1 As String = String.Join(",", dict.ToList().Select(Function(x) x.Value).ToList())

    '                Dim INVO_FilePath As String, AR_FilePath As String = ""
    '                dict.ToList().ForEach(Sub(x)
    '                                          Dim sRMA_RMANO = x.Key
    '                                          Dim sRMA_ARNO = x.Value
    '                                          result = Print_Inovice_ForAjax(sShippingNO, sRepairCenter, sRMA_ARNO)
    '                                          If result.Bool = True Then
    '                                              result = Print_AD_ForAjax(sShippingNO, sRepairCenter, sRMA_ARNO)
    '                                              If result.Bool = True Then
    '                                                  'Call SendMailNoticeForAJAX(sRMA_RMANO, sShippingNO, sRMA_ARNO, result)  '補寄正式信
    '                                              End If
    '                                          End If

    '                                          'addLog("Resent AR/INV Mail:" & sShippingNO & "," & sRepairCenter & "," & sRMA_ARNO & "," & RMA_NO)

    '                                      End Sub)


    '                response.Status = True
    '                response.ErrorMessage = ""

    '            End If

    '        End If
    '    Catch ex As Exception
    '        response.Status = False
    '        response.ErrorMessage = ex.Message
    '    End Try

    '    Return JsonConvert.SerializeObject(New With {
    '        .Status = response.Status,
    '        .Msg = response.ErrorMessage
    '    })
    'End Function

    Private Shared Function Print_Inovice_ForAjax(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String) As Result
        Dim oDsReport As New DataSet
        Dim LanguageID As String = ""
        Dim result As New Result()
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable

        Try
            Dim i As Integer = 0
            Dim ctlReport As New ctlReport

            Dim iRMARQ_QUOTE As Double = 0.0
            'Throw New Exception(sRepairCenter.Trim())    'fairy
            dtRptINVOICE = ctlReport.qryRPTInovice(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
            For i = 0 To dtRptINVOICE.Rows.Count - 1
                dtRptINVOICE.Rows(i)("RMAD_SEQ") = (i + 1).ToString()
                iRMARQ_QUOTE = iRMARQ_QUOTE + Convert.ToDouble(dtRptINVOICE.Rows(i)("RMARQ_QUOTE"))
            Next

            Dim SayTotal As String = GetNumberWord(iRMARQ_QUOTE)
            For i = 0 To dtRptINVOICE.Rows.Count - 1
                dtRptINVOICE.Rows(i)("SayTotal") = SayTotal
            Next

            oDsReport.Tables.Add(dtRptINVOICE)

            If dtRptINVOICE.Rows.Count > 0 Then
                Dim oLoginInfo As New ctlLoginInfo
                LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptINVOICE.Rows(0)("rmad_rmano").ToString())
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return Print_PDF("Invoice", dtRptINVOICE, LanguageID)
    End Function

    Private Shared Function Print_AD_ForAjax(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String) As Result
        Dim oDsReport As New DataSet
        Dim LanguageID As String = ""
        Dim result As New Result()
        Dim dtRptAD As New RmaDTO.RPTADDataTable

        Try
            Dim i As Integer = 0
            Dim ctlReport As New ctlReport

            dtRptAD = ctlReport.qryRPTAD(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
            For i = 0 To dtRptAD.Rows.Count - 1
                dtRptAD.Rows(i)("SEQ") = (i + 1).ToString()
            Next

            oDsReport.Tables.Add(dtRptAD)

            If dtRptAD.Rows.Count > 0 Then
                Dim oLoginInfo As New ctlLoginInfo
                LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptAD.Rows(0)("RMA單號").ToString())
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return Print_PDF("AD", dtRptAD, LanguageID)
    End Function

    Private Shared Function Print_PDF(type As String, dtReport As DataTable, sLanguageID As String) As Result
        Dim result As New Result()
        Dim com As New Common
        Dim comms As New Commons
        Dim rptDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim sReportToPDF As String = ""
        Dim ds As DataSet
        ds.Tables.Add(dtReport)
        Try
            sReportToPDF = If(type = "Invoice", "Invoice_" & com.GetRandomizeNum() & ".pdf", "AD_" & com.GetRandomizeNum() & ".pdf")
            Dim path As String = String.Format("Report\rpt{0}{1}.rpt", If(type = "Invoice", "Inovice", "AR"), If(sLanguageID = "003", "_jp", ""))
            'rptDoc.Load(HttpContext.Current.Server.MapPath(path))
            Dim Server_MapPath_rpt = HttpContext.Current.Server.MapPath(path)

            '修改Export PDF共用函式 by buck modify 20250828 begin
            'com.ExportSetup(rptDoc, sReportToPDF)

            comms.ExportSetup_New(Server_MapPath_rpt, ConfigurationManager.AppSettings("Report_FilePath") & sReportToPDF, ds)
            'Me.ViewState("_AttachmentFile_01") = ConfigurationSettings.AppSettings("Report_FilePath") & sReportToPDF_Invoice
            'ExportSetup()
            'Me.ViewState("_AttachmentFile_01") = ConfigureExportToPdf(sReportToPDF_Invoice)
            '修改Export PDF共用函式 by buck modify 20250828 end
        Catch ex As Exception

            result.Bool = False
            result.Msg = ex.Message
            result.TempFilePath = ""
        Finally
            If result.Msg IsNot Nothing OrElse result.Msg = "" Then
                result.Bool = True
                result.TempFilePath = ConfigurationSettings.AppSettings("Report_FilePath") & sReportToPDF
                result.PathData = New TypeData()
                If type = "Invoice" Then
                    result.PathData.INVO_FilePath = result.TempFilePath
                Else
                    result.PathData.AR_FilePath = result.TempFilePath
                End If
            End If
        End Try
        result.Lang = sLanguageID
        rptDoc.Close()
        Return result
    End Function

    ''' <summary>
    ''' 出貨通知--寄送Mail(對象:業務, 助理)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function SendMailNoticeForAJAX(sRMANo As String, sShippingNo As String, sRMA_ARNO As String, result As Result) As Boolean
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim _MailCC As String = ""
        Dim _FinanceEmail As String = ConfigurationSettings.AppSettings("FinanceEmail")

        Dim ctlRMA As New ctlRMA
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
        Dim oMail As New ctlMail

        Dim oLang As New ctlLanguage
        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", result.Lang)

        Try
            'addLog("SendMailNoticeBySales:" & sRMANo)
            dtSales = ctlRMA.getSalesMail_ARNO(sRMA_ARNO)
            For i = 0 To dtSales.Rows.Count - 1

                Dim sSubject As String = oLang.getMailText("Mail", "422", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = oLang.getMailText("Mail", "423", ctlLanguage.eumType.Mail, LanguageID)

                'sSubject = sSubject.Replace("[$ShippingNo$]", sShippingNo)

                Dim RMA_ARNO As String = dtSales.Rows(0)("RMA_ARNO").ToString().Trim()
                Dim RMA_INVNO As String = dtSales.Rows(0)("RMA_INVNO").ToString().Trim()

                Dim SalesID As String = dtSales.Rows(0)("SalesID").ToString().Trim()
                Dim SalesEmail As String = dtSales.Rows(0)("SalesEmail").ToString().Trim()
                Dim SalesName As String = dtSales.Rows(0)("SalesName").ToString().Trim()

                Dim AssistantID As String = dtSales.Rows(0)("AssistantID").ToString().Trim()
                Dim AssistantEmail As String = dtSales.Rows(0)("AssistantEmail").ToString().Trim()
                Dim AssistantName As String = dtSales.Rows(0)("AssistantName").ToString().Trim()
                Dim sCU_Name As String = dtSales.Rows(0)("CU_NAME").ToString().Trim()
                Dim sCU_FINANCEEMAIL As String = dtSales.Rows(0)("CU_FINANCEEMAIL").ToString().Trim()

                ' addLog("SalesID:" & SalesID)
                ' addLog("SalesM:" & SalesEmail)
                ' addLog("AssM:" & AssistantEmail)
                '修改寄送信件主旨 : [RMA System notice]ARMA-150900001,A/R NO:ARBA-150900013, INVOICE(AXLA-151100121)
                sSubject = sSubject.Replace("[$RMANo$]", sRMANo)
                sSubject = sSubject.Replace("[$ARNo$]", RMA_ARNO)
                sSubject = sSubject.Replace("[$INVNo$]", RMA_INVNO)
                sSubject = sSubject.Replace("[$Customer's Name$]", sCU_Name)


                If SalesEmail.Trim() <> "" Or AssistantEmail <> "" Then
                    Dim mailTo As String = ""
                    If SalesEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & SalesEmail.Trim()
                    End If
                    If AssistantEmail.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo = mailTo & AssistantEmail.Trim()
                    End If

                    If sCU_FINANCEEMAIL.Trim() <> "" Then
                        If mailTo.Trim() <> "" Then
                            mailTo = mailTo & ","
                        End If
                        mailTo += sCU_FINANCEEMAIL
                    End If

                    Dim sName As String = ""
                    If SalesName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & SalesName.Trim()
                    End If
                    If AssistantName.Trim() <> "" Then
                        If sName.Trim() <> "" Then
                            sName = sName & " / "
                        End If
                        sName = sName & AssistantName.Trim()
                    End If
                    sBody = sBody.Replace("[$sales Name$]", sName)

                    sBody = sBody.Replace("[$Receivable NO$]", RMA_ARNO)
                    sBody = sBody.Replace("[$Invoice NO$]", RMA_INVNO)

                    Dim oAttachmentFile As New Collection
                    If result.PathData.INVO_FilePath <> "" Then
                        oAttachmentFile.Add(result.PathData.INVO_FilePath)
                    End If
                    If result.PathData.AR_FilePath <> "" Then
                        oAttachmentFile.Add(result.PathData.AR_FilePath)
                    End If

                    If _FinanceEmail.Trim <> "" Then
                        mailTo = mailTo & "," & _FinanceEmail.Trim()
                    End If

                    'mailTo = "isaac.yeh@cipherlab.com.tw"
                    'mailTo = "ted@icat-tech.com.tw"
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If ConfigurationManager.AppSettings("isDebug") = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC, oAttachmentFile)
                    'addLog(i&"-SalesID:" & SalesID)
                End If
            Next


        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = oLang.getText("Mail", "008", ctlLanguage.eumType.Tag)
                'Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function
#End Region

End Class

Imports System.Data
Imports DataService
Imports DefLanguage
Imports RMA_Model

Partial Class Request_Detail
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_FilePath As String = ConfigurationSettings.AppSettings("Repair_FilePath")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")

#Region "電池保固"
    Private Sub Check_Standard_Battery(ByVal RMAD_ID As String, ByVal WATS_SN As String)

        Dim myctlWarranty As New ctlWarranty
        Dim dt As New DataTable
        dt = myctlWarranty.Check_Standard_Battery(RMAD_ID)

        Dim dt_ As New DataTable
        dt_ = myctlWarranty.WARRANTYSERIAL_BI_Repaired_QTY(WATS_SN)

        Dim index As Integer = 0

        For i = 0 To dt.Rows.Count - 1
            index = index + Convert.ToInt32(dt.Rows(i)(0).ToString().Trim())
        Next

        Dim index_ As Integer = 0

        For i = 0 To dt_.Rows.Count - 1
            If Not dt_.Rows(i)("RMAD_RMANO_18") Is DBNull.Value Then
                If dt_.Rows(i)("RMAD_RMANO_18").ToString().Trim() = "OK" Then
                    index_ = index_ + 1
                End If
            End If
        Next

        RMAD_RMANO.Visible = False
        txtRMAD_RMANO_QTY.Visible = False
        LabRMAD_RMANO_QTY.Visible = False

        If index = 0 Then

        Else

            RMAD_RMANO.Visible = True
            txtRMAD_RMANO_QTY.Visible = True
            LabRMAD_RMANO_QTY.Visible = True

            If Session("_LanguageID").ToString() = "002" Then
                txtRMAD_RMANO_QTY.Text = ":使用數量:" + index.ToString().Trim()
                RMAD_RMANO.Text = "電池保險申請"

            ElseIf Session("_LanguageID").ToString() = "003" Then
                txtRMAD_RMANO_QTY.Text = ":使用数量:" + index.ToString().Trim()
                RMAD_RMANO.Text = "バッテリー保険適用"



            Else
                txtRMAD_RMANO_QTY.Text = ":Using Q'ty:" + index.ToString().Trim()
                RMAD_RMANO.Text = "Apply Battery Insurance"
            End If



            If Session("_LanguageID").ToString() = "002" Then
                LabRMAD_RMANO_QTY.Text = " 剩餘數量:" + index_.ToString().Trim()

            ElseIf Session("_LanguageID").ToString() = "003" Then
                LabRMAD_RMANO_QTY.Text = " 残りの数量:" + index_.ToString().Trim()

            Else
                LabRMAD_RMANO_QTY.Text = " Remaining Q'ty:" + index_.ToString().Trim()
            End If

        End If

    End Sub
#End Region

#Region "Total_Loss"

    Public Function Apply_Total_Loss_Insurance(ByVal SERIALNO As String) As DataTable

        Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable

        Try

            Dim ctlWarranty_ As New ctlWarranty
            '提供序號
            EXPORT_EXPORT_ORDERNUMBER = ctlWarranty_.EXPORT_EXPORT_ORDERNUMBER(SERIALNO)

        Catch ex As Exception
            Throw ex
        Finally

        End Try

        Return EXPORT_EXPORT_ORDERNUMBER
    End Function
    Private Sub Check_AXMT410_AXMT400(ByVal UI_lblRMANoText_String As String, ByVal UI_lblShowSerial_String As String)

        '判斷保險種類
        If 1 = 1 Then

            '確認是否需要秀出 Apply Total Loss Insurance 
            '提供序號
            Insurance_Label.Visible = False
            UI_Apply_Total_Loss_Insurance.Visible = False
            Apply_Label.Visible = False

            Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
            EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial_String)

            If Not EXPORT_EXPORT_ORDERNUMBER Is Nothing Then

                If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                    '有的話帶入db資料
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True

                    Dim ctlWarranty_ As New ctlWarranty

                    Dim dt As New DataTable
                    dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)

                    If Not dt Is Nothing Then

                        If dt.Rows.Count > 0 Then
                            UI_Apply_Total_Loss_Insurance.Text = "Yes"
                        Else

                        End If

                    Else

                    End If

                End If

            Else

            End If

        End If

        Dim Order_No As String = ""
        Dim Total_Loss_Insurance As String = ""

        '判斷保險可用數量
        If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

            Dim ID As String = ""
            Dim Project_No As String = ""
            Dim Project_Qty As String = ""
            Dim Order_Qty As String = ""
            Dim RMAD_RMANO As String = ""
            Dim RMAD_SEQ As String = ""
            Dim RMAD_SERIALNO As String = ""

            If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

                Dim ctlWarranty_ As New ctlWarranty
                '提供序號
                Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial_String)


                '新增RMA加購保固判斷
                Dim Select_WARRANTYSERIAL_DataTable As Boolean = ctlWarranty_.Select_WARRANTYSERIAL(UI_lblShowSerial_String)

                If Select_WARRANTYSERIAL_DataTable Then

                    If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                        Dim Select_WARRANTYITEM_DataTable As New DataTable
                        Select_WARRANTYITEM_DataTable = ctlWarranty_.Select_WARRANTYITEM(UI_lblShowSerial_String)
                        'RMA
                        Dim pList As New List(Of Product)
                        Dim Product_D As New Product

                        '專案編號
                        Product_D.Project_No = ""
                        '專案數量
                        Product_D.Project_Qty = ""
                        '訂單編號
                        Product_D.Order_No = Select_WARRANTYITEM_DataTable.Rows(0)("WATI_WATYNO").ToString.Trim() & "-" & Select_WARRANTYITEM_DataTable.Rows(0)("WATI_SEQ").ToString.Trim()
                        '專案數量
                        Product_D.Order_Qty = Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()
                        '全損保險
                        Product_D.Total_Loss_Insurance = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString() & "%"
                        '可更換數量
                        Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        '已更換數量
                        Product_D.Quantity_replaced = ""

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = UI_lblRMANoText_String
                        RMAD_SEQ = Product_D.Replaceable_quantity
                        RMAD_SERIALNO = UI_lblShowSerial_String


                    End If

                Else

                    Dim strarr() As String
                    strarr = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("EXPORT_ORDERNUMBER").ToString().Trim().Split("-")

                    '表格
                    Dim EXPORT_axmt410_axmt400 As New DataTable
                    EXPORT_axmt410_axmt400 = ctlWarranty_.EXPORT_axmt410_axmt400(strarr(0) & "-" & strarr(1), strarr(2))


                    If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                        Dim pList As New List(Of Product)
                        Dim Product_D As New Product

                        '專案編號
                        Product_D.Project_No = EXPORT_axmt410_axmt400.Rows(0)("Project_No").ToString() & "-" & strarr(2)
                        '專案數量
                        Product_D.Project_Qty = EXPORT_axmt410_axmt400.Rows(0)("Project_Qty").ToString()
                        '訂單編號
                        Product_D.Order_No = EXPORT_axmt410_axmt400.Rows(0)("Order_No").ToString() & "-" & strarr(2)
                        '專案數量
                        Product_D.Order_Qty = EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()
                        '全損保險
                        Product_D.Total_Loss_Insurance = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString() & "%"
                        '可更換數量
                        Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        '已更換數量
                        Product_D.Quantity_replaced = ""
                        pList.Add(Product_D)

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = UI_lblRMANoText_String
                        RMAD_SEQ = ""
                        RMAD_SERIALNO = UI_lblShowSerial_String



                    End If
                End If

            End If

        End If

        If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

            Dim ctlWarranty_ As New ctlWarranty
            Dim Check_axmt410_axmt400_DataTable As New DataTable
            Check_axmt410_axmt400_DataTable = ctlWarranty_.Check_axmt410_axmt400(Order_No)

            If Math.Floor(Convert.ToDecimal(Total_Loss_Insurance)) >= Convert.ToInt32(Check_axmt410_axmt400_DataTable.Rows(0)("ORDER_NO").ToString()) + 1 Then

            Else

                Insurance_Label.Visible = False
                UI_Apply_Total_Loss_Insurance.Visible = False
                Apply_Label.Visible = False


                '單獨資料顯示區塊
                Dim dt As New DataTable
                dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)

                For i = 0 To dt.Rows.Count - 1
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True
                Next
            End If
        End If

        '確認這單是否使用過全損保險
        If 1 = 1 Then

            Dim ctlWarranty_ As New ctlWarranty
            Dim dt As New DataTable
            dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)

            If Not dt Is Nothing Then

                If dt.Rows.Count > 0 Then
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Text = "Yes"
                Else

                End If

            Else

            End If

        End If

    End Sub
    Public Class Product
        '專案編號
        Public Property Project_No As String
        '專案數量
        Public Property Project_Qty As String
        '訂單編號
        Public Property Order_No As String
        '專案數量
        Public Property Order_Qty As String
        '全損保險
        Public Property Total_Loss_Insurance As String
        '可更換數量
        Public Property Replaceable_quantity As String
        '已更換數量
        Public Property Quantity_replaced As String
    End Class

#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtRepairDetail") = Nothing

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Me.UI_lblPreviousPage_RMADID.Text = UI_lblPreviousPage_RMADID.Text.Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()

                Call chkFlowCase01()
                Call setControls()
                Call QueryDataByHead()
                Call RepairUpload()
                Call QueryDataByDetail()
                Call QueryDataByStatusPoint()
                Call setFlowCase01()
                Call QuerySDC()
                Call QueryBI_Recode() '需求新增:BI保固 By buck Add 20250902

                Try
                    Call Check_AXMT410_AXMT400(UI_lblPreviousPage_RMANO.Text.Trim, UI_lblSerialText.Text.Trim())
                    Call Check_Standard_Battery(Me.UI_lblPreviousPage_RMADID.Text, Me.UI_lblSerialText.Text.Trim())
                Catch ex As Exception
                    RMAD_RMANO.Visible = False
                    txtRMAD_RMANO_QTY.Visible = False
                    LabRMAD_RMANO_QTY.Visible = False

                    Insurance_Label.Visible = False
                    UI_Apply_Total_Loss_Insurance.Visible = False
                    Apply_Label.Visible = False

                Finally


                End Try




            End If
        End If
    End Sub
#End Region

    Private Sub QuerySDC()
        Dim SDCGrid As DataGrid = New DataGrid()
        SDCGrid = Me.UcSDCViewG.FindControl("UcSDCViewG")
        Me.UcSDCViewG.show(UI_lblSerialText.Text.Trim(), Me.UI_dvRepairDetail.Width)

    End Sub

    '需求新增:BI保固 By buck Add 20250902 begin
    Private Sub QueryBI_Recode()

        Dim ctlWarranty As New ctlWarranty
        Dim dtWarrantyBI_List As New DataTable
        Dim lsWarrantyBI As List(Of DataRow)
        Dim oClient As New ctlRMA.Client
        Dim dtRMAData As DataTable = TryCast(Session("_dtRequest"), DataTable)
        Dim sCU_NO = dtRMAData.AsEnumerable().Select(Function(x) If(x.IsNull("CU_NO"), Nothing, x.Field(Of String)("CU_NO"))).FirstOrDefault()

        dtWarrantyBI_List = ctlWarranty.WARRANTY_BI_List()
        lsWarrantyBI = dtWarrantyBI_List.AsEnumerable.Where(Function(x) x.Field(Of String)("BI_CUNO") = sCU_NO).ToList()
        UI_lblApplyBI_TD.Visible = False
        UI_lblApplyBIText_TD.Visible = False
        uiTxt_ApplyBatteryInsurance.Text = "0"

        If Not lsWarrantyBI Is Nothing And lsWarrantyBI.Count > 0 Then
            UI_lblApplyBI_TD.Visible = True
            UI_lblApplyBIText_TD.Visible = True
            Dim dcBATT_QTY As Decimal = lsWarrantyBI.Select(Function(x) Convert.ToDecimal(x("BI_BATTERYQTY"))).FirstOrDefault() '購買總數量

            Dim sApply_Bi As String = dtRMAData.AsEnumerable().Select(Function(x) x.Field(Of String)("RMAD_APPLY_BI")).FirstOrDefault() '是否購買保固

            Dim dtBatExpend = ctlWarranty.WARRANTY_BatExpend_Record(New RMADetailReq With {.RMAD_SERIALNO = UI_lblSerialText.Text.Trim()})
            Dim dcUSEQTY As Decimal = dtBatExpend.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x("BE_USEQTY"))) '使用數量
            If dcUSEQTY = 0 Or sApply_Bi = Nothing Or sApply_Bi = 0 Then
                UI_lblApplyBI_TD.Visible = False
                UI_lblApplyBIText_TD.Visible = False
                uiTxt_ApplyBatteryInsurance.Text = "0"
            End If
            uiTxt_ApplyBatteryInsurance.Text = dcBATT_QTY - dcUSEQTY
        End If

    End Sub
    'Private Sub QueryWarrantyBI()

    '    Dim ctlWarranty As New ctlWarranty
    '    Dim dt As New DataTable
    '    dt = ctlWarranty.WARRANTY_BI(UI_lblShowSerial.Text.Trim())
    '    'dt = ctlWarranty.WARRANTY_BI("FZ12450011516")
    '    UI_BI_Row.Visible = False
    '    fdt_Warranty.Visible = False
    '    UI_opgImproPerusage.SelectedValue = "0"

    '    If Not dt Is Nothing And dt.Rows.Count > 0 Then
    '        UI_BI_Row.Visible = True
    '        fdt_Warranty.Visible = True

    '        Dim sApply_Bi As String = dt.AsEnumerable().Select(Function(x) x.Field(Of String)("RMAD_APPLY_BI")).FirstOrDefault()
    '        UI_opgApplyBatteryInsurance.SelectedValue = If(Not sApply_Bi Is Nothing, sApply_Bi, "0")

    '        Dim total As Decimal = dt.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x("BI_BATTERYQTY")))
    '        If total = 0 Then
    '            UI_BI_Row.Visible = False
    '            fdt_Warranty.Visible = False
    '            UI_opgImproPerusage.SelectedValue = "0"
    '        End If
    '        uiTxt_ApplyBatteryInsurance.Text += total.ToString()
    '    End If

    '    Me.UI_dvBATRECORD.PageSize = _PageSize
    '    Me.UI_dvBATRECORD.DataSource = dt
    '    Me.UI_dvBATRECORD.DataBind()
    'End Sub
    '需求新增:BI保固 By buck Add 20250902 end

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

    Private Sub setControls()

        '取得Tag Text
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "111", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)

        Me.UI_lblLaborHourCost.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)

        Me.UI_lblLaborHourText.Text = _oLanguage.getText("RMA", "057", ctlLanguage.eumType.Tag)
        Me.UI_lblQuote.Text = _oLanguage.getText("RMA", "113", ctlLanguage.eumType.Tag)
        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblProblemDesc.Text = _oLanguage.getText("RMA", "025", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairDesc.Text = _oLanguage.getText("RMA", "053", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairMemo.Text = _oLanguage.getText("RMA", "054", ctlLanguage.eumType.Tag)
        Me.UI_lblReportAttachment.Text = _oLanguage.getText("RMA", "182", ctlLanguage.eumType.Tag)

        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "082", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborCost.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
        Me.UI_lblMaterialCost.Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalText.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)
        Me.UI_lblStatusHistory.Text = _oLanguage.getText("RMA", "183", ctlLanguage.eumType.Tag)

        Me.UI_lblReceived.Text = _oLanguage.getText("RMA", "090", ctlLanguage.eumType.Tag)
        Me.UI_lblRepair.Text = _oLanguage.getText("RMA", "091", ctlLanguage.eumType.Tag)
        Me.UI_lblSales.Text = _oLanguage.getText("RMA", "092", ctlLanguage.eumType.Tag)
        Me.UI_lblClient.Text = _oLanguage.getText("RMA", "093", ctlLanguage.eumType.Tag)
        Me.UI_lblRepaired.Text = _oLanguage.getText("RMA", "094", ctlLanguage.eumType.Tag)
        Me.UI_lblClose.Text = _oLanguage.getText("RMA", "095", ctlLanguage.eumType.Tag)
        Me.UI_lblCancel.Text = _oLanguage.getText("RMA", "041", ctlLanguage.eumType.Tag)
        Me.UI_lblApprover.Text = _oLanguage.getText("RMA", "096", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        '需求新增:BI保固 By buck Add 20250902 begin
        Me.UI_lblApplyBatteryInsurance.Text = _oLanguage.getText("RMA", "222", ctlLanguage.eumType.Tag)
        Me.uiTxt_ApplyBatteryInsurance.Text = _oLanguage.getText("RMA", "223", ctlLanguage.eumType.Tag)
        '需求新增:BI保固 By buck Add 20250902 end
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryDataByHead()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailureReasons As New FailureDTO.vwFailureReasonsDataTable

        Dim oRepairQuoting As New ctlRMA.Repair_Quoting
        Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable

        Dim oRepair As New ctlRMA.Repair
        Dim dtRepair As New RmaDTO.RMARepairDataTable
        Dim oRepair_SALESQUOTED As New ctlRMA.Sale
        Dim dtRepair_SALESQUOTED As New RmaDTO.RMASALE_QUOTEDDataTable
        Dim dtRepair_RMAQUOTED As New RmaDTO.RMARepair_QuotedDataTable

        dtRepairQuoting = oRepairQuoting.Query(Me.UI_lblPreviousPage_RMADID.Text, "")
        dtRepair_SALESQUOTED = oRepair_SALESQUOTED.QueryBySaleQuoted(Me.UI_lblPreviousPage_RMADID.Text)
        dtRepair_RMAQUOTED = oRepairQuoting.QueryByRepairQuoted(Me.UI_lblPreviousPage_RMADID.Text)

        If dtRepairQuoting.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_QuotingRow = dtRepairQuoting.Rows(0)

            Dim RMA_COMPNO As String = dr.RMA_COMPNO.Trim()
            Dim RMAR_COMPNO As String = ""
            If dr.IsRMAR_COMPNONull = False Then RMAR_COMPNO = dr.RMAR_COMPNO.Trim()

            If dr.IsRMAD_SERIALNONull = False Then Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.Trim()

            Me.UI_lblRMANoText.Text = dr.RMA_NO.ToString().Trim()
            Me.UI_lblCustomerText.Text = dr.CU_NAME.Trim()

            '維修報價工時
            'If dr.IsRMARQ_LABORHOURNull = False Then Me.UI_lblLaborHourvalue.Text = dr.RMARQ_LABORHOUR.ToString().Trim()
            If dr.IsRMARQ_LABORHOURNull = False And dr.IsRMARQ_LABORPRICENull = False Then
                Me.UI_lblLaborHourvalue.Text = dr.RMARQ_LABORHOUR * dr.RMARQ_LABORPRICE
                'Me.UI_lblLaborHourvalue.Text = dr.RMARQ_ASSIGLABORCOST
            End If

            If Session("_RepairCenter").ToString().IndexOf(RMAR_COMPNO) <> -1 Then
                '更入系統維修中心 跟 被指派維修中心 一樣時

                If dr.IsRMAR_REPAIR_ISFILLNull = False And dr.IsRMARQ_QUOTENull = False Then
                    If dr.RMAR_REPAIR_ISFILL = 1 Then
                        Me.UI_lblSymbol.Visible = True
                        Me.UI_lblLaborHourText.Visible = True


                        '2013/11/15 mark
                        '1.先依維修單的金額為主
                        '2.若維修單無資料,再取報價單金額
                        'If dr.IsRMAR_QUOTENull = False Then
                        '    Me.UI_lblQuoteCode.Text = dr.RMAR_CURRENCYCODE.Trim()
                        '    Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMAR_QUOTE).ToString("N")
                        'Else
                        '    Me.UI_lblQuoteCode.Text = dr.RMARQ_CURRENCYCODE.Trim()
                        '    Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_QUOTE).ToString("N")
                        'End If
                        'If dr.IsRMARQ_CURRENCYCODENull = False Then Me.UI_lblHourCode.Text = dr.RMARQ_CURRENCYCODE.ToString().Trim()
                        'If dr.IsRMARQ_LABORPRICENull = False Then Me.UI_lblHourQuotedText.Text = dr.RMARQ_LABORPRICE.ToString().Trim()

                        '2013/11/15 修改
                        '已維修報價單金額為主
                        If dr.IsRMARQ_QUOTENull = False Then
                            Me.UI_lblQuoteCode.Text = dr.RMARQ_CURRENCYCODE.Trim()
                            Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_QUOTE).ToString("N")
                        End If

                        'MODI BY Angel ON 20160226 增加業務報價與維修中心報價 
                        If dtRepair_SALESQUOTED.Rows.Count > 0 Then
                            Dim dr_Sales As RmaDTO.RMASALE_QUOTEDRow = dtRepair_SALESQUOTED.Rows(0)
                            Me.UI_lblSalesQuoteCode.Text = dr.RMARQ_CURRENCYCODE.Trim()
                            Me.UI_lblSalesQuoteText.Text = Convert.ToDouble(dr_Sales.RMASQ_QUOTE).ToString("N")
                        End If

                        If dtRepair_RMAQUOTED.Rows.Count > 0 Then
                            Dim dr_Rma As RmaDTO.RMARepair_QuotedRow = dtRepair_RMAQUOTED.Rows(0)
                            Me.UI_lblRMAQuoteCode.Text = dr.RMARQ_CURRENCYCODE.Trim()
                            Me.UI_lblRMAQuoteText.Text = Convert.ToDouble(dr_Rma.RMARQ_QUOTE).ToString("N")
                        End If

                        If dr.IsRMARQ_CURRENCYCODENull = False Then Me.UI_lblHourCode.Text = dr.RMARQ_CURRENCYCODE.ToString().Trim()
                        If dr.IsRMARQ_LABORPRICENull = False Then Me.UI_lblHourQuotedText.Text = dr.RMARQ_LABORPRICE.ToString().Trim()

                    End If
                End If

            Else
                '更入系統維修中心 跟 被指派維修中心 不一樣時
                If dr.IsRMAR_REPAIR_ISFILLNull = False And dr.IsRMARQ_ASSIGEQUOTENull = False Then
                    If dr.RMAR_REPAIR_ISFILL = 1 Then
                        Me.UI_lblSymbol.Visible = True
                        Me.UI_lblLaborHourText.Visible = True

                        '2013/11/15 mark
                        '1.先依維修單的金額為主
                        '2.若維修單無資料,再取報價單金額
                        'If dr.IsRMAR_QUOTENull = False Then
                        '    Me.UI_lblQuoteCode.Text = dr.RMAR_ASSIGECURRENCYCODE.Trim()
                        '    Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMAR_ASSIGEQUOTE).ToString("N")
                        'Else
                        '    Me.UI_lblQuoteCode.Text = dr.RMARQ_ASSIGECURRENCYCODE.Trim()
                        '    Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_ASSIGEQUOTE).ToString("N")
                        'End If
                        'If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then Me.UI_lblHourCode.Text = dr.RMARQ_ASSIGECURRENCYCODE.ToString().Trim()
                        'If dr.IsRMARQ_ASSIGLABORCOSTNull = False Then Me.UI_lblHourQuotedText.Text = dr.RMARQ_ASSIGLABORCOST.ToString().Trim()

                        '2013/11/15 修改
                        '已維修報價單金額為主
                        If dr.IsRMARQ_ASSIGEQUOTENull = False Then
                            Me.UI_lblQuoteCode.Text = dr.RMARQ_ASSIGECURRENCYCODE.Trim()
                            Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_ASSIGEQUOTE).ToString("N")
                        End If

                        If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then Me.UI_lblHourCode.Text = dr.RMARQ_ASSIGECURRENCYCODE.ToString().Trim()
                        If dr.IsRMARQ_ASSIGLABORCOSTNull = False Then Me.UI_lblHourQuotedText.Text = dr.RMARQ_ASSIGLABORCOST.ToString().Trim()

                    End If
                End If
            End If

            '不良原因敘述
            If dr.IsRMAR_FARCNONull = False And dr.IsRMAR_FARNONull = False Then
                dtFailureReasons = oFailure.QueryByFailure(Session("_LanguageID"), dr.RMAR_FARCNO, dr.RMAR_FARNO)
                If dtFailureReasons.Rows.Count > 0 Then
                    Me.UI_lblFARCNO.Text = dr.RMAR_FARCNO.ToString().Trim()
                    Me.UI_lblFARCNO.Text = dr.RMAR_FARNO.ToString().Trim()
                    Me.UI_lblFailureText.Text = dtFailureReasons.Rows(0)("FAR_REASON").ToString.Trim()
                End If

            Else
                If dr.IsRMAD_FARFARCNONull = False And dr.IsRMAD_FARNONull = False Then
                    dtFailureReasons = oFailure.QueryByFailure(Session("_LanguageID"), dr.RMAD_FARFARCNO, dr.RMAD_FARNO)
                    If dtFailureReasons.Rows.Count > 0 Then
                        Me.UI_lblFARCNO.Text = dr.RMAD_FARFARCNO.ToString().Trim()
                        Me.UI_lblFARCNO.Text = dr.RMAD_FARNO.ToString().Trim()
                        Me.UI_lblFailureText.Text = dtFailureReasons.Rows(0)("FAR_REASON").ToString.Trim()
                    End If
                End If
            End If


            'Problem Description
            If dr.IsRMAR_PROBLEMDESCNull = False Then
                Me.UI_lblProblemDescText.Text = dr.RMAR_PROBLEMDESC.Trim().Replace(vbLf, "<br>")
            Else
                If dr.IsRMAD_PROBLEMDESCNull = False Then
                    Me.UI_lblProblemDescText.Text = dr.RMAD_PROBLEMDESC.Trim().Replace(vbLf, "<br>")
                End If
            End If

            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_lblRepairDescText.Text = dr.RMAR_REPAIRDESC.Trim().Replace(vbLf, "<br>")
            If dr.IsRMAR_REPAIRMEMONull = False Then Me.UI_lblRepairMemoText.Text = dr.RMAR_REPAIRMEMO.Trim().Replace(vbLf, "<br>")
            '
            If dr.IsRMAD_APPLY_BINull = True OrElse dr.RMAD_APPLY_BI = "0" Then
                Me.UI_lblApplyBatteryInsuranceText.Text = "N"
            ElseIf dr.IsRMAD_APPLY_BINull = False And dr.RMAD_APPLY_BI = "1" Then
                Me.UI_lblApplyBatteryInsuranceText.Text = "Y"
            End If

            '===============================================================================================================================
            '總金額
            dtRepair = oRepair.QueryByRepair(Me.UI_lblPreviousPage_RMADID.Text)
            If dtRepair.Rows.Count > 0 Then
                Dim drRepair As RmaDTO.RMARepairRow = dtRepair.Rows(0)
                Me.oTable.Visible = True    '顯示

                If Session("_RepairCenter").ToString().IndexOf(RMAR_COMPNO) <> -1 Then
                    '登入系統維修中心 跟 被指派維修中心 一樣時

                    Me.UI_Status.Text = "0"     '0:被指派,1:指派
                    If drRepair.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblCurrencySymbol1.Text = drRepair.RMAR_CURRENCYCODE.ToString().Trim() '幣別
                    If drRepair.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblCurrencySymbol2.Text = drRepair.RMAR_CURRENCYCODE.ToString().Trim() '幣別
                    If drRepair.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblCurrencyCode1.Text = drRepair.RMAR_CURRENCYCODE.ToString().Trim() '幣別

                    If drRepair.IsRMAR_LABORCOSTNull = False Then Me.UI_lblLaborTotal.Text = Convert.ToDouble(drRepair.RMAR_LABORCOST.ToString()).ToString("N")
                    If drRepair.IsRMAR_MATERIALCOSTNull = False Then Me.UI_lblMaterialTotal.Text = Convert.ToDouble(drRepair.RMAR_MATERIALCOST.ToString()).ToString("N")
                    If drRepair.IsRMAR_QUOTENull = False Then Me.UI_lblTotal.Text = Convert.ToDouble(drRepair.RMAR_QUOTE.ToString()).ToString("N")
                Else
                    '登入系統維修中心 跟 被指派維修中心 不一樣時

                    Me.UI_Status.Text = "1"     '0:被指派,1:指派
                    If drRepair.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblCurrencySymbol1.Text = drRepair.RMAR_ASSIGECURRENCYCODE.ToString().Trim() '幣別
                    If drRepair.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblCurrencySymbol2.Text = drRepair.RMAR_ASSIGECURRENCYCODE.ToString().Trim() '幣別
                    If drRepair.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblCurrencyCode1.Text = drRepair.RMAR_ASSIGECURRENCYCODE.ToString().Trim() '幣別

                    If drRepair.IsRMAR_ASSIGELABORCOSTNull = False Then Me.UI_lblLaborTotal.Text = Convert.ToDouble(drRepair.RMAR_ASSIGELABORCOST.ToString()).ToString("N")
                    If drRepair.IsRMAR_ASSIGEMATERIALCOSTNull = False Then Me.UI_lblMaterialTotal.Text = Convert.ToDouble(drRepair.RMAR_ASSIGEMATERIALCOST.ToString()).ToString("N")
                    If drRepair.IsRMAR_ASSIGEQUOTENull = False Then Me.UI_lblTotal.Text = Convert.ToDouble(drRepair.RMAR_ASSIGEQUOTE.ToString()).ToString("N")
                End If
            End If


            If dr.RMAD_STATUS = 91 Then
                ' Me.UI_lblLaborHourvalue.Text = "0"
                'Me.UI_lblQuoteText.Text = "0"
            End If
        End If

    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01()
        Me.UI_lblLaborHourCost.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborHourText.Text = ""
        Me.UI_lblSymbol.Visible = False
        Me.UI_lblHourQuotedText.Visible = False

        '權限範圍等級:0.By Center、1.All
        If Me.UI_flowCase.Text = "01" And Session("_AuthorityLevel").ToString().Trim() = "0" Then
            Me.uiTR_LaborHourCost.Visible = False
        End If


        Me.UI_lblLaborCost.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        If Me.UI_flowCase.Text = "01" And Session("_AuthorityLevel").ToString().Trim() = "0" Then
            Me.UI_lblLaborCost.Visible = False
            Me.UI_lblLaborCost_Delimited.Visible = False
            Me.UI_lblCurrencySymbol1.Visible = False
            Me.UI_lblLaborTotal.Visible = False

            Me.UI_lblMaterialCost.Visible = False
            Me.UI_lblMaterialCost_Delimited.Visible = False
            Me.UI_lblCurrencySymbol2.Visible = False
            Me.UI_lblMaterialTotal.Visible = False

            Me.UI_lblTotalText.Visible = False
            Me.UI_lblTotalText_Delimited.Visible = False
            Me.UI_lblCurrencyCode1.Visible = False
            Me.UI_lblTotal.Visible = False
        End If

    End Sub

#Region "RepairUpload"

    Private Sub RepairUpload()
        Dim oRepairUpload As New ctlRMA.Repair
        Dim dtRepairUpload As New RmaDTO.RMAREPAIR_UPLOADDataTable
        Dim dvRepairUpload As DataView

        dtRepairUpload = oRepairUpload.QueryByUpload(Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim())

        If Not dtRepairUpload.Rows.Count > 0 Then
            dtRepairUpload = AddSerial(dtRepairUpload)
            dvRepairUpload = dtRepairUpload.DefaultView
            dvRepairUpload.RowFilter = "RMARU_ID = '9'"
        Else
            dvRepairUpload = dtRepairUpload.DefaultView()
        End If

        Me.UI_dvRetailUpload.DataSource = dvRepairUpload
        Me.UI_dvRetailUpload.DataBind()
    End Sub

    ''' <summary>
    ''' 無資料時建立一筆虛擬資料,作用是秀出表頭
    ''' </summary>
    ''' <param name="dtRepairUpload"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial(ByVal dtRepairUpload As RmaDTO.RMAREPAIR_UPLOADDataTable) As RmaDTO.RMAREPAIR_UPLOADDataTable
        Dim drRepairUpload As RmaDTO.RMAREPAIR_UPLOADRow = dtRepairUpload.NewRMAREPAIR_UPLOADRow
        Dim oGuid As Guid = Guid.NewGuid
        Try
            drRepairUpload.RMARU_ID = "9"
            drRepairUpload.RMARU_RMANO = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()
            drRepairUpload.RMARU_UPLOADFILE = "111"
            drRepairUpload.RMARU_DESC = ""
            drRepairUpload.RMARU_AD = Session("_UserID")
            drRepairUpload.RMARU_ADNAME = Session("_UserName")
            drRepairUpload.RMARU_CSTMP = Date.Now
            drRepairUpload.RMARU_LUAD = Session("_UserID")
            drRepairUpload.RMARU_LUADNAME = Session("_UserName")
            drRepairUpload.RMARU_LUSTMP = Date.Now

            dtRepairUpload.AddRMAREPAIR_UPLOADRow(drRepairUpload)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRepairUpload
    End Function

    Protected Sub UI_dvRetailUpload_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRetailUpload.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = _oLanguage.getText("RMA", "117", ctlLanguage.eumType.Tag)
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "118", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim UI_RMARUID As Label = e.Row.FindControl("UI_RMARUID")
            If UI_RMARUID.Text.Trim = "9" Then
                e.Row.Visible = False
            Else
                Dim UI_RepairUpload As HyperLink = e.Row.FindControl("UI_RepairUpload")
                Dim UI_lblRepairUpload As Label = e.Row.FindControl("UI_lblRepairUpload")
                Dim sRepaurFile As String() = UI_lblRepairUpload.Text.ToString().Trim().Split(",")

                UI_RepairUpload.Text = sRepaurFile(0).ToString().Trim()
                'UI_RepairUpload.NavigateUrl = _WEBURL & _Repair_VisualPath & sRepaurFile(1).ToString().Trim()
                UI_RepairUpload.NavigateUrl = "https://e-rma-admin.cipherlab.com.tw" & _Repair_VisualPath & sRepaurFile(1).ToString().Trim()

            End If

        End If
    End Sub

#End Region

#Region "QueryDataByDetail"

    Private Sub QueryDataByDetail()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable

        dtRepairDetail = oRepair.QueryByDetail(Me.UI_lblPreviousPage_RMADID.Text)

        Call RepairDetail_DataBind(dtRepairDetail, 0)
        Call setFlowCase01_UI_dvRepairDetail()
    End Sub

    Private Sub RepairDetail_DataBind(ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairDetail") = dtRepairDetail

        Call ArrangementData(dtRepairDetail)
        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARED_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable)
        Dim oDuty As New ctlDefective
        Dim dtDuty As New DefectiveDTO.DefectiveDataTable
        Dim dvDuty As DataView
        dtDuty = oDuty.QueryAll(Session("_LanguageID"))
        dvDuty = dtDuty.DefaultView

        Dim i As Integer = 0
        Dim Status As String = Me.UI_Status.Text.Trim() '0:被指派,1:指派
        If dtRepairDetail.Columns("SeqID") Is Nothing Then
            dtRepairDetail.Columns.Add("SeqID")
            dtRepairDetail.Columns.Add("DEFECTIVE")
            dtRepairDetail.Columns.Add("PRICE")
        End If

        For i = 0 To dtRepairDetail.Rows.Count - 1
            Dim dr As RmaDTO.RMARepair_DetailRow = dtRepairDetail.Rows(i)
            Dim sDefectiveNo As String = ""
            dtRepairDetail.Rows(i)("SeqID") = i + 1

            'Defective Reason 關聯 Defective.Defective_No-->Defective代碼 
            If dr.IsRMARED_DEFECTIVENull = False Then
                sDefectiveNo = dr.RMARED_DEFECTIVE.ToString().Trim()
                dvDuty.RowFilter = "DEFECTIVE_NO='" & sDefectiveNo.Trim() & "'"
                sDefectiveNo = dvDuty.Item(0)("DEFECTIVE_NAME").ToString().Trim()
            End If
            dtRepairDetail.Rows(i)("DEFECTIVE") = sDefectiveNo.Trim()

            dvDuty.RowFilter = ""

            'Price
            '0:被指派,1:指派
            If Status.Trim() = "0" Then
                dtRepairDetail.Rows(i)("PRICE") = dr.RMARED_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARED_PRICE.ToString()).ToString("N")
            Else
                dtRepairDetail.Rows(i)("PRICE") = dr.RMARED_ASSIGECURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARED_ASSIGEPRICE.ToString()).ToString("N")
            End If

            'Option
            '1.不修, 2.要修
            If dtRepairDetail.Rows(i)("RMARED_OPTION") = "1" Then
                dtRepairDetail.Rows(i)("RMARED_OPTION") = "Reject"
            ElseIf dtRepairDetail.Rows(i)("RMARED_OPTION") = "2" Then
                dtRepairDetail.Rows(i)("RMARED_OPTION") = "Confirmed"
            End If
        Next

    End Sub

    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHNewPart As Label = e.Item.FindControl("lblHNewPart")
            Dim lblHNewSerial As Label = e.Item.FindControl("lblHNewSerial")
            Dim lblHDuty As Label = e.Item.FindControl("lblHDuty")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHLocation As Label = e.Item.FindControl("lblHLocation")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblHPrice As Label = e.Item.FindControl("lblHPrice")
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")
            Dim lblHOption As Label = e.Item.FindControl("lblHOption")

            lblHNewPart.Text = _oLanguage.getText("RMA", "184", ctlLanguage.eumType.Tag)
            lblHNewSerial.Text = _oLanguage.getText("RMA", "185", ctlLanguage.eumType.Tag)
            lblHDuty.Text = _oLanguage.getText("RMA", "051", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHLocation.Text = _oLanguage.getText("RMA", "084", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblHPrice.Text = _oLanguage.getText("RMA", "104", ctlLanguage.eumType.Tag)
            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "098", ctlLanguage.eumType.Tag)
            lblHOption.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        End If

        If e.Item.ItemType = ListItemType.Item Then
            '加總
            'Dim lblMaterialCost As Label = e.Item.FindControl("lblMaterialCost")
            'Me.UI_lblMaterial.Text = Convert.ToDecimal(Me.UI_lblMaterial.Text.Trim()) + Convert.ToDecimal(lblMaterialCost.Text.Trim())
        End If

    End Sub

    Private Sub setFlowCase01_UI_dvRepairDetail()
        Dim i As Integer = 0

        For i = 0 To UI_dvRepairDetail.Controls.Count - 1
            'oTableHeader
            Dim oTableHeader As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableHeader")
            If IsNothing(oTableHeader) = False Then

                If Me.UI_flowCase.Text = "01" Then
                    oTableHeader.Rows(0).Cells(6).Visible = False    'price filed
                End If
            End If

            'oTableHeader
            Dim oTableRow As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableRow")
            If IsNothing(oTableRow) = False Then
                If Me.UI_flowCase.Text = "01" Then
                    oTableRow.Rows(0).Cells(6).Visible = False    'price filed
                End If
            End If
        Next

    End Sub

#End Region

    Private Sub QueryDataByStatusPoint()
        Dim oRMAStatus As New ctlRMA.RMAStatus
        Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable

        dtStatusPoint = oRMAStatus.QueryPointByDetail(Me.UI_lblPreviousPage_RMADID.Text)
        If dtStatusPoint.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)

            If dr.IsRECEIVED_ADNull = False Then Me.UI_lblReceivedUser.Text = dr.RECEIVED_AD.Trim()
            If dr.IsRECEIVED_DATENull = False Then Me.UI_lblReceivedDate.Text = Convert.ToDateTime(dr.RECEIVED_DATE).ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsREPAIRQUOTED_ADNull = False Then Me.UI_lblRepairQuotedUser.Text = dr.REPAIRQUOTED_AD.Trim()
            If dr.IsREPAIRQUOTED_DATENull = False Then Me.UI_lblRepairQuotedDate.Text = Convert.ToDateTime(dr.REPAIRQUOTED_DATE).ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsSALES_ADNull = False Then Me.UI_lblSalesUser.Text = dr.SALES_AD.Trim()
            If dr.IsSALES_DATENull = False Then Me.UI_lblSalesDate.Text = Convert.ToDateTime(dr.SALES_DATE).ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLIENT_CONFIRMNull = False Then
                '1.客戶自行確認, 2.業務帶客戶確認
                If dr.CLIENT_CONFIRM = 1 Then
                    If dr.IsCLIENT_ADNull = False Then Me.UI_lblClientUser.Text = dr.CLIENT_AD.Trim()
                    If dr.IsCLIENT_DATENull = False Then Me.UI_lblClientDate.Text = Convert.ToDateTime(dr.CLIENT_DATE).ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    If dr.IsSALES_ADNull = False Then Me.UI_lblClientUser.Text = dr.SALES_AD.Trim()
                    If dr.IsSALES_DATENull = False Then Me.UI_lblClientDate.Text = Convert.ToDateTime(dr.SALES_DATE).ToString("yyyy/MM/dd HH:mm:ss")
                End If
            End If

            If dr.IsREPAIRED_ADNull = False Then Me.UI_lblRepairedUser.Text = dr.REPAIRED_AD.Trim()
            If dr.IsREPAIRED_DATENull = False Then Me.UI_lblRepairedDate.Text = Convert.ToDateTime(dr.REPAIRED_DATE).ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLOSE_ADNull = False Then Me.UI_lblCloseUser.Text = dr.CLOSE_AD.Trim()
            If dr.IsCLOSE_DATENull = False Then Me.UI_lblCloseDate.Text = Convert.ToDateTime(dr.CLOSE_DATE).ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCANCEL_ADNull = False Then Me.UI_lblCancelUser.Text = dr.CANCEL_AD.Trim()
            If dr.IsCANCEL_DATENull = False Then Me.UI_lblCancelDate.Text = Convert.ToDateTime(dr.CANCEL_DATE).ToString("yyyy/MM/dd HH:mm:ss")

        End If

    End Sub

    Protected Sub UI_dvRetailUpload_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles UI_dvRetailUpload.PageIndexChanging
        UI_dvRetailUpload.PageIndex = e.NewPageIndex
        'UI_dvRetailUpload.DataBind()
        RepairUpload()
    End Sub
End Class

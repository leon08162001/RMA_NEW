Imports System.Data
Imports DataService
Imports DefLanguage
Imports RMA_Model

Partial Class Report3_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim helper As New Utility.LanguageHelper()

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtReport") = Nothing

            pnlVersion.Visible = False
            Call setControls()
            Call QuerySDC()

            '顯示 total loss
            total_loss_Panel.Visible = False

        End If
    End Sub
#End Region

    Private Sub QuerySDC()
        Dim SDCGrid As DataGrid = New DataGrid()
        SDCGrid = Me.UcSDCViewG.FindControl("UcSDCViewG")
        Me.UcSDCViewG.show(UI_txtProduct_SerialNo.Text.Trim(), Me.UI_gvReport.Width)

    End Sub

    'RMAQuoting搬動到產品序號保固頁面 by Buck 20260310 begin
    Private Sub QueryWarranty()
        Dim WarrantyGrid As DataGrid = New DataGrid()
        WarrantyGrid = Me.UcWarrantyView.FindControl("UcWarrantyView")
        Me.UcWarrantyView.show(UI_txtProduct_SerialNo.Text.Trim(), Me.lstParts.Width)

    End Sub
    'RMAQuoting搬動到產品序號保固頁面 by Buck 20260310 end

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "003", ctlLanguage.eumType.Tag)

        Me.UI_lblProduct_SerialNo.Text = _oLanguage.getText("Report", "153", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)


        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.lblColumn.Text = _oLanguage.getText("RMA", "382", ctlLanguage.eumType.Tag)
        Me.lblVerName.Text = _oLanguage.getText("RMA", "383", ctlLanguage.eumType.Tag)
        Me.lblVerBefore.Text = _oLanguage.getText("RMA", "384", ctlLanguage.eumType.Tag)
        Me.lblVerAfter.Text = _oLanguage.getText("RMA", "385", ctlLanguage.eumType.Tag)

        Me.lblC01.Text = _oLanguage.getText("RMA", "386", ctlLanguage.eumType.Tag)
        Me.lblC02.Text = _oLanguage.getText("RMA", "387", ctlLanguage.eumType.Tag)
        Me.lblC03.Text = _oLanguage.getText("RMA", "388", ctlLanguage.eumType.Tag)
        Me.lblC04.Text = _oLanguage.getText("RMA", "389", ctlLanguage.eumType.Tag)
        Me.lblC05.Text = _oLanguage.getText("RMA", "390", ctlLanguage.eumType.Tag)
        Me.lblC06.Text = _oLanguage.getText("RMA", "391", ctlLanguage.eumType.Tag)
        Me.lblC07.Text = _oLanguage.getText("RMA", "392", ctlLanguage.eumType.Tag)
        Me.lblC08.Text = _oLanguage.getText("RMA", "393", ctlLanguage.eumType.Tag)
        Me.lblC09.Text = _oLanguage.getText("RMA", "394", ctlLanguage.eumType.Tag)
        Me.lblC10.Text = _oLanguage.getText("RMA", "395", ctlLanguage.eumType.Tag)
        Me.lblC11.Text = _oLanguage.getText("RMA", "396", ctlLanguage.eumType.Tag)
        Me.lblC12.Text = _oLanguage.getText("RMA", "397", ctlLanguage.eumType.Tag)
        Me.lblC13.Text = _oLanguage.getText("RMA", "398", ctlLanguage.eumType.Tag)
        Me.lblC14.Text = _oLanguage.getText("RMA", "399", ctlLanguage.eumType.Tag)
        Me.lblC15.Text = _oLanguage.getText("RMA", "400", ctlLanguage.eumType.Tag)
        Me.lblC16.Text = _oLanguage.getText("RMA", "401", ctlLanguage.eumType.Tag)
        Me.lblC17.Text = _oLanguage.getText("RMA", "402", ctlLanguage.eumType.Tag)
        Me.lblC18.Text = _oLanguage.getText("RMA", "403", ctlLanguage.eumType.Tag)
        Me.lblVerChange.Text = _oLanguage.getText("RMA", "301", ctlLanguage.eumType.Tag)

        '修正寫法語言檔改成json By buck Add 20251007 end
        helper.ApplyGridHeader(UI_gvWARRANTY_BI, "Report3_Search", "WARRANTY_BI", Session("_LanguageID").ToString())
        helper.ApplyGridHeader(UI_gvReport, "Report3_Search", "Report", Session("_LanguageID").ToString())
        helper.ApplyGridHeader(GridView_EXPORT_axmt410_axmt400, "Report3_Search", "TotalLoss", Session("_LanguageID").ToString())
        '修正寫法語言檔改成json By buck Add 20251007 end
    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
        Call QuerySDC()
        'Call QueryWARRANTYSERIAL_BI()
        Call QueryWARRANTY_BI() '需求新增:BI保固 By buck Add 20250902
        Call QueryWarranty() 'RMAQuoting搬動到產品序號保固頁面 by Buck 20260310
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

    Public Class WARRANTYSERIAL_BI_D

        Public Property WATS_WATYNO As String  'WRMA單號
        Public Property WATS_WATYSEQ As String '項次
        Public Property Total As String '總可耗用量
        Public Property Consumed_Amount As String '已耗用量
        '專案編號

    End Class

    '需求新增:BI保固 By buck Add 20250902 begin
    Private Sub QueryWARRANTY_BI()

        Dim oWarranty As New ctlWarranty
        Dim dcUSEQTY As Decimal = 0

        'GH12440002156
        'UI_txtProduct_SerialNo.Text.Trim
        Dim dtBI_Detail As DataTable = oWarranty.WARRANTY_BI_Detail_List(New RMADetailReq With {.RMAD_SERIALNO = UI_txtProduct_SerialNo.Text.Trim}) '電池購買保固明細
        Dim dtBI_Record As DataTable = oWarranty.WARRANTY_BatExpend_Record(New RMADetailReq With {.RMAD_SERIALNO = UI_txtProduct_SerialNo.Text.Trim}) '使用紀錄
        If dtBI_Record.Rows.Count > 0 Then
            Dim sWATY_CUST As String = dtBI_Detail.AsEnumerable.Select(Function(x) x.Field(Of String)("WATY_CUST")).FirstOrDefault() '客戶代碼
            Dim sORDER_NO As String = dtBI_Detail.AsEnumerable.Select(Function(x) x.Field(Of String)("ORDER_NO")).FirstOrDefault() '訂單編號
            dcUSEQTY = dtBI_Record.AsEnumerable.Where(Function(x) x.Field(Of String)("WATY_CUST") = sWATY_CUST AndAlso
                                                                    x.Field(Of String)("BE_ORDERNO") = sORDER_NO).
                                                                Sum(Function(x) Convert.ToDecimal(x("BE_USEQTY")))
        End If

        For Each x In dtBI_Detail.AsEnumerable
            x.SetField("Qty_replaced", dcUSEQTY)
        Next

        UI_gvWARRANTY_BI.DataSource = dtBI_Detail
        UI_gvWARRANTY_BI.DataBind()

    End Sub
    '需求新增:BI保固 By buck Add 20250902 end

    Private Sub QueryData(ByVal iPageIndex As Integer)

        '初始化
        If 1 = 1 Then
            Dim pList As New List(Of Product)
            Dim Product_D As New Product

            GridView_EXPORT_axmt410_axmt400.DataSource = pList
            GridView_EXPORT_axmt410_axmt400.DataBind()
        End If

        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable

        dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.UI_txtProduct_SerialNo.Text.Trim())
        Dim sSnNo As String = ""
        If dtReport.Rows.Count > 0 Then
            sSnNo = dtReport.Rows(0)("EXPORT_SERIALNO").ToString().Trim()
        End If
        Call dvReport_DataBind(dtReport, iPageIndex)
        'Call GetVersigonData(Me.UI_txtProduct_SerialNo.Text.Trim()) '優化效能 by buck modify 20260323

        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = oExport.getModelNo(sSnNo)

        If sSnNo <> "" Then
            Dim dtWarrParts As DataTable = GetData(sSnNo, sModelNo, "")
            Me.ViewState("dtWarrParts") = dtWarrParts
            lstParts.DataSource = dtWarrParts
            lstParts.DataBind()
        End If

        Try
            total_loss_Panel.Visible = False


            Dim EXPORT_WAR_ID As String = dtReport.Rows(0)("EXPORT_WAR_ID").ToString().Trim()
            If EXPORT_WAR_ID <> "" Then

                sSnNo = dtReport.Rows(0)("EXPORT_SERIALNO").ToString().Trim()

                '20230808

                Dim Warranty_TypeSetting_add_oWarranty As New ctlWarranty
                Dim Warranty_TypeSetting_add_dtWarrSpecs As New DataTable
                Warranty_TypeSetting_add_dtWarrSpecs = Warranty_TypeSetting_add_oWarranty.QueryWarrSpecs(Session("_LanguageID").ToString(), EXPORT_WAR_ID, "", "")
                Warranty_TypeSetting_add_lstSpecs.DataSource = Warranty_TypeSetting_add_dtWarrSpecs
                Warranty_TypeSetting_add_lstSpecs.DataBind()

                ' 原資料跳出exception所以不會往下執行，邏輯判斷WarrSpecs的WAP_NAME欄位有Losstop%才顯示，並且修正做法需要判斷是否有資料 by buck modify 20260327
                If Warranty_TypeSetting_add_dtWarrSpecs IsNot Nothing AndAlso Warranty_TypeSetting_add_dtWarrSpecs.Rows.Count > 0 Then

                    Dim hasLosstop = Warranty_TypeSetting_add_dtWarrSpecs.AsEnumerable().Any(Function(x) x.Field(Of String)("WAP_NAME").Trim().StartsWith("Losstop"))
                    If hasLosstop Then

                        Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                        EXPORT_EXPORT_ORDERNUMBER = Warranty_TypeSetting_add_oWarranty.EXPORT_EXPORT_ORDERNUMBER(UI_txtProduct_SerialNo.Text.Trim())

                        Dim Select_WARRANTYSERIAL_DataTable As Boolean = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYSERIAL(UI_txtProduct_SerialNo.Text.Trim())

                        If Select_WARRANTYSERIAL_DataTable Then
                            Dim Select_WARRANTYITEM_DataTable As New DataTable
                            Select_WARRANTYITEM_DataTable = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYITEM(UI_txtProduct_SerialNo.Text.Trim())
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

                            Dim SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 As New DataTable
                            SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 = Warranty_TypeSetting_add_oWarranty.SELECT_axmt410_axmt400_RMA(Product_D.Order_No, Product_D.Order_Qty, Product_D.Replaceable_quantity)

                            '已更換數量

                            If Not SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 Is Nothing Then

                                Product_D.Quantity_replaced = SELECT_axmt410_axmt400_SELECT_axmt410_axmt400.Rows.Count

                            Else

                                Product_D.Quantity_replaced = "0"

                            End If



                            pList.Add(Product_D)
                            GridView_EXPORT_axmt410_axmt400.DataSource = pList
                            GridView_EXPORT_axmt410_axmt400.DataBind()

                        Else
                            'tiptop
                            Dim strarr() As String
                            strarr = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("EXPORT_ORDERNUMBER").ToString().Trim().Split("-")

                            '表格
                            Dim EXPORT_axmt410_axmt400 As New DataTable
                            EXPORT_axmt410_axmt400 = Warranty_TypeSetting_add_oWarranty.EXPORT_axmt410_axmt400(strarr(0) & "-" & strarr(1), strarr(2))

                            Dim orderQty As Integer = 0
                            If EXPORT_axmt410_axmt400.Rows.Count > 0 Then
                                orderQty = Convert.ToInt32(If(IsDBNull(EXPORT_axmt410_axmt400.Rows(0)("Order_Qty")), "0", EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()))
                            End If

                            Dim lossTopStr As String = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString().Trim()
                            Dim lossTop As Integer = 0 ' 預設值
                            If Not String.IsNullOrEmpty(lossTopStr) Then
                                lossTop = Convert.ToInt32(lossTopStr)
                            End If

                            Dim pList As New List(Of Product)
                            Dim Product_D As New Product

                            '專案編號
                            Product_D.Project_No = EXPORT_axmt410_axmt400.Rows(0)("Project_No").ToString() & "-" & strarr(2)
                            '專案數量
                            Product_D.Project_Qty = EXPORT_axmt410_axmt400.Rows(0)("Project_Qty").ToString()
                            '訂單編號
                            Product_D.Order_No = EXPORT_axmt410_axmt400.Rows(0)("Order_No").ToString() & "-" & strarr(2)
                            '專案數量
                            Product_D.Order_Qty = orderQty
                            '全損保險
                            Product_D.Total_Loss_Insurance = lossTop & "%"
                            '可更換數量
                            Product_D.Replaceable_quantity = orderQty * lossTop / 100

                            Dim SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 As New DataTable
                            SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 = Warranty_TypeSetting_add_oWarranty.SELECT_axmt410_axmt400(Product_D.Project_No, Product_D.Project_Qty, Product_D.Order_No, Product_D.Order_Qty, Product_D.Replaceable_quantity)

                            '已更換數量

                            If Not SELECT_axmt410_axmt400_SELECT_axmt410_axmt400 Is Nothing Then

                                Product_D.Quantity_replaced = SELECT_axmt410_axmt400_SELECT_axmt410_axmt400.Rows.Count

                            Else

                                Product_D.Quantity_replaced = "0"

                            End If

                            pList.Add(Product_D)


                            GridView_EXPORT_axmt410_axmt400.DataSource = pList
                            GridView_EXPORT_axmt410_axmt400.DataBind()


                        End If


                        'total loss
                        total_loss_Panel.Visible = True
                        Dim ctlExtend_count As New ctlExtend
                        Dim myDataTable As New DataTable
                        myDataTable = ctlExtend_count.QryRMA_RMADETAIL(sSnNo)

                        Dim total As String = myDataTable.AsEnumerable().Select(Function(x) If(Not IsDBNull(x("TOTAL")), Convert.ToDecimal(x("TOTAL")), 0)).FirstOrDefault()
                        Dim Exceed As String = ""

                        Exceed = myDataTable.Rows.Count.ToString().Trim()

                        'For Each item As DataRow In myDataTable.Rows
                        '    total = item("TOTAL").ToString().Trim()
                        'Next

                        Shipping_Lab.Text = total
                        Repair_Lab.Text = Exceed

                        Dim percentage As Double = (Convert.ToInt32(Exceed) / Convert.ToInt32(total)) * 100

                        Exceed_Lab.Text = percentage.ToString() & "%"

                    End If
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub dvReport_DataBind(ByVal dtReport As ReportDTO.Rpt_RMAWarrantyDataTable, ByVal iPageIndex As Integer)
        Session("_dtReport") = dtReport

        Me.UI_gvReport.DataSource = dtReport.DefaultView
        Me.UI_gvReport.DataBind()
    End Sub

    Protected Sub UI_CheckVer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Call GetVersigonData(Me.UI_txtProduct_SerialNo.Text.Trim()) '優化效能 by buck modify 20260323
        If UI_CheckVer.Checked Then
            pnlVersion.Visible = True
        Else
            pnlVersion.Visible = False
        End If
    End Sub
    '需求新增:BI保固 By buck Add 20250902 begin
    Protected Sub UI_gvWARRANTY_BI_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_gvWARRANTY_BI.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            '多語系調整 by buck modify 20251201 being
            'Dim mapping As New Dictionary(Of Integer, String) From {
            '    {0, "OrderNo"},
            '    {1, "OrderQty"},
            '    {2, "BIYear"},
            '    {3, "ReplaceableQty"},
            '    {4, "QtyReplaced"}
            '}
            'Dim Model As New LanguageReq() With {
            '    .TargetControl = e.Row,
            '    .NamespaceName = "Report3_Search",
            '    .Category = "UI",
            '    .ClassName = "GRID",
            '    .Section = "WARRANTY_BI",
            '    .mapping = mapping,
            '    .LangCode = Session("_LanguageID").ToString()
            '}
            'helper.ApplyLanguage(Model)
            'helper.Apply(Me.Page, Session("_LanguageID").ToString())
            'helper.ApplyGridHeader(UI_gvWARRANTY_BI, "Report3_Search", "WARRANTY_BI", Session("_LanguageID").ToString())
            '多語系調整 by buck modify 20251201 end
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then

            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim lnk As HyperLink = CType(e.Row.FindControl("lnkOrder"), HyperLink)

            Dim orderNo As String = drv("Order_No").ToString()
            Dim serialNo As String = UI_txtProduct_SerialNo.Text.Trim()

            lnk.NavigateUrl = String.Format("Warranty_SerialSearch.aspx?OrderNo={0}&SerialNo={1}", orderNo, Server.UrlEncode(serialNo))

        End If
    End Sub
    ''需求新增:BI保固 By buck Add 20250902 end

    Protected Sub GridView_EXPORT_axmt410_axmt400_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView_EXPORT_axmt410_axmt400.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            '多語系調整 by buck modify 20251201 being
            '修正寫法語言檔改成json By buck Add 20251007 begin
            'Dim mapping As New Dictionary(Of Integer, String) From {
            '    {0, "ProjectNo"},
            '    {1, "ProjectQty"},
            '    {2, "OrderNo"},
            '    {3, "OrderQty"},
            '    {4, "TotalLossInsurance"},
            '    {5, "ReplaceableQty"},
            '    {6, "QtyReplaced"}
            '}
            'Dim Model As New LanguageReq() With {
            '    .TargetControl = e.Row,
            '    .NamespaceName = "Report3_Search",
            '    .Category = "UI",
            '    .ClassName = "GRID",
            '    .Section = "TotalLoss",
            '    .mapping = mapping,
            '    .LangCode = Session("_LanguageID").ToString()
            '}
            'helper.ApplyLanguage(Model)
            'helper.Apply(Me.Page, Session("_LanguageID").ToString())
            'helper.ApplyGridHeader(GridView_EXPORT_axmt410_axmt400, "Report3_Search", "TotalLoss", Session("_LanguageID").ToString())
            'e.Row.Cells(0).Text = _oLanguage.getText("TotalLoss", "001", ctlLanguage.eumType.Tag)
            'e.Row.Cells(1).Text = _oLanguage.getText("TotalLoss", "002", ctlLanguage.eumType.Tag)
            'e.Row.Cells(2).Text = _oLanguage.getText("TotalLoss", "003", ctlLanguage.eumType.Tag)
            'e.Row.Cells(3).Text = _oLanguage.getText("TotalLoss", "004", ctlLanguage.eumType.Tag)
            'e.Row.Cells(4).Text = _oLanguage.getText("TotalLoss", "005", ctlLanguage.eumType.Tag)
            'e.Row.Cells(5).Text = _oLanguage.getText("TotalLoss", "006", ctlLanguage.eumType.Tag)
            'e.Row.Cells(6).Text = _oLanguage.getText("TotalLoss", "007", ctlLanguage.eumType.Tag)
            '修正寫法語言檔改成json By buck Add 20251007 end
            '多語系調整 by buck modify 20251201 end
        End If
    End Sub

    Protected Sub UI_gvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_gvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            '多語系調整 by buck modify 20251201 being
            '修正寫法語言檔改成json By buck Add 20251007 begin
            'Dim mapping As New Dictionary(Of Integer, String) From {
            '    {1, "PartsNo"},
            '    {2, "SerialNo"},
            '    {3, "CustomerName"},
            '    {4, "Shippeddate"},
            '    {5, "Warrantydate"},
            '    {6, "CWWarrantydate"}
            '}
            'Dim Model As New LanguageReq() With {
            '    .TargetControl = e.Row,
            '    .NamespaceName = "Report3_Search",
            '    .Category = "UI",
            '    .ClassName = "GRID",
            '    .Section = "Report",
            '    .mapping = mapping,
            '    .LangCode = Session("_LanguageID").ToString()
            '}
            'helper.ApplyLanguage(Model)
            'helper.Apply(Me.Page, Session("_LanguageID").ToString())
            'helper.ApplyGridHeader(UI_gvReport, "Report3_Search", "Report", Session("_LanguageID").ToString())
            'e.Row.Cells(1).Text = _oLanguage.getText("Report", "158", ctlLanguage.eumType.Tag)
            'e.Row.Cells(2).Text = _oLanguage.getText("Report", "159", ctlLanguage.eumType.Tag)
            'e.Row.Cells(3).Text = _oLanguage.getText("Report", "144", ctlLanguage.eumType.Tag)
            'e.Row.Cells(4).Text = _oLanguage.getText("Report", "155", ctlLanguage.eumType.Tag)
            'e.Row.Cells(5).Text = _oLanguage.getText("Report", "156", ctlLanguage.eumType.Tag)
            'e.Row.Cells(6).Text = _oLanguage.getText("Report", "192", ctlLanguage.eumType.Tag)
            '修正寫法語言檔改成json By buck Add 20251007 end
            '多語系調整 by buck modify 20251201 end
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            'UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_isDetail As Label = e.Row.FindControl("UI_isDetail")
            If UI_isDetail.Text.Trim = "2" Then
                'e.Row.Cells(1).Visible = False
                e.Row.Cells(2).Visible = False
                e.Row.Cells(3).Visible = False
                e.Row.Cells(4).Visible = False
                e.Row.Cells(5).Visible = False
            End If

            If UI_isDetail.Text.Trim = "1" Then
                e.Row.BackColor = Drawing.Color.Silver
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

    Protected Sub UI_gvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_gvReport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            Dim dtReport As ReportDTO.Rpt_RMAWarrantyDataTable = Session("_dtReport")
            Call dvReport_DataBind(dtReport, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Private Sub GetVersigonData(ByVal SerialNumber As String)
        Dim sMessage As String = ""
        Try
            ResetValue()
            If SerialNumber <> "" Then
                'Get All Column Name
                Dim oClient As New ctlRMA.Client
                Dim dtExport As New RmaDTO.dtExportDataTable
                Dim sPartNo As String = ""
                dtExport = oClient.QryExport(SerialNumber)
                If dtExport.Rows.Count > 0 Then
                    sPartNo = dtExport.Rows(0)("export_partno").ToString().Trim()
                    Dim sSerialNo As String = dtExport.Rows(0)("e_sr").ToString().Trim()
                    'Current Version
                    txtA01.Text = dtExport.Rows(0)("e_01").ToString().Trim()
                    lblOA01.Text = dtExport.Rows(0)("m_01").ToString().Trim()
                    txtA02.Text = dtExport.Rows(0)("e_02").ToString().Trim()
                    lblOA02.Text = dtExport.Rows(0)("m_02").ToString().Trim()
                    txtA03.Text = dtExport.Rows(0)("e_03").ToString().Trim()
                    lblOA03.Text = dtExport.Rows(0)("m_03").ToString().Trim()
                    txtA04.Text = dtExport.Rows(0)("e_04").ToString().Trim()
                    lblOA04.Text = dtExport.Rows(0)("m_04").ToString().Trim()
                    txtA05.Text = dtExport.Rows(0)("e_05").ToString().Trim()
                    lblOA05.Text = dtExport.Rows(0)("m_05").ToString().Trim()
                    txtA06.Text = dtExport.Rows(0)("e_06").ToString().Trim()
                    lblOA06.Text = dtExport.Rows(0)("m_06").ToString().Trim()
                    txtA07.Text = dtExport.Rows(0)("e_07").ToString().Trim()
                    lblOA07.Text = dtExport.Rows(0)("m_07").ToString().Trim()
                    txtA08.Text = dtExport.Rows(0)("e_08").ToString().Trim()
                    lblOA08.Text = dtExport.Rows(0)("m_08").ToString().Trim()
                    txtA09.Text = dtExport.Rows(0)("e_09").ToString().Trim()
                    lblOA09.Text = dtExport.Rows(0)("m_09").ToString().Trim()
                    txtA10.Text = dtExport.Rows(0)("e_10").ToString().Trim()
                    lblOA10.Text = dtExport.Rows(0)("m_10").ToString().Trim()
                    txtA11.Text = dtExport.Rows(0)("e_11").ToString().Trim()
                    lblOA11.Text = dtExport.Rows(0)("m_11").ToString().Trim()
                    txtA12.Text = dtExport.Rows(0)("e_12").ToString().Trim()
                    lblOA12.Text = dtExport.Rows(0)("m_12").ToString().Trim()

                    txtB01.Text = dtExport.Rows(0)("e_13").ToString().Trim()
                    lblOB01.Text = dtExport.Rows(0)("m_13").ToString().Trim()
                    txtB02.Text = dtExport.Rows(0)("e_14").ToString().Trim()
                    lblOB02.Text = dtExport.Rows(0)("m_14").ToString().Trim()
                    txtB03.Text = dtExport.Rows(0)("e_15").ToString().Trim()
                    lblOB03.Text = dtExport.Rows(0)("m_15").ToString().Trim()
                    txtB04.Text = dtExport.Rows(0)("e_16").ToString().Trim()
                    lblOB04.Text = dtExport.Rows(0)("m_16").ToString().Trim()
                    txtB05.Text = dtExport.Rows(0)("e_17").ToString().Trim()
                    lblOB05.Text = dtExport.Rows(0)("m_17").ToString().Trim()
                    txtB06.Text = dtExport.Rows(0)("e_18").ToString().Trim()
                    lblOB06.Text = dtExport.Rows(0)("m_18").ToString().Trim()

                    lblA01.Text = dtExport.Rows(0)("n_01").ToString().Trim()
                    lblA02.Text = dtExport.Rows(0)("n_02").ToString().Trim()
                    lblA03.Text = dtExport.Rows(0)("n_03").ToString().Trim()
                    lblA04.Text = dtExport.Rows(0)("n_04").ToString().Trim()
                    lblA05.Text = dtExport.Rows(0)("n_05").ToString().Trim()
                    lblA06.Text = dtExport.Rows(0)("n_06").ToString().Trim()
                    lblA07.Text = dtExport.Rows(0)("n_07").ToString().Trim()
                    lblA08.Text = dtExport.Rows(0)("n_08").ToString().Trim()
                    lblA09.Text = dtExport.Rows(0)("n_09").ToString().Trim()
                    lblA10.Text = dtExport.Rows(0)("n_10").ToString().Trim()
                    lblA11.Text = dtExport.Rows(0)("n_11").ToString().Trim()
                    lblA12.Text = dtExport.Rows(0)("n_12").ToString().Trim()
                    lblB01.Text = dtExport.Rows(0)("n_13").ToString().Trim()
                    lblB02.Text = dtExport.Rows(0)("n_14").ToString().Trim()
                    lblB03.Text = dtExport.Rows(0)("n_15").ToString().Trim()
                    lblB04.Text = dtExport.Rows(0)("n_16").ToString().Trim()
                    lblB05.Text = dtExport.Rows(0)("n_17").ToString().Trim()
                    lblB06.Text = dtExport.Rows(0)("n_18").ToString().Trim()

                End If
                'Get Current Version
                If sPartNo <> Nothing Then
                    Dim dtTcOae As New RmaDTO.dtTcOaeFileDataTable
                    dtTcOae = oClient.QryTcOaeFile(sPartNo)
                    If dtTcOae.Rows.Count > 0 Then
                        txtQA01.Text = dtTcOae.Rows(0)("FD_01").ToString().Trim()
                        lblNA01.Text = dtTcOae.Rows(0)("m_01").ToString().Trim()

                        txtQA02.Text = dtTcOae.Rows(0)("FD_02").ToString().Trim()
                        lblNA02.Text = dtTcOae.Rows(0)("m_02").ToString().Trim()

                        txtQA03.Text = dtTcOae.Rows(0)("FD_03").ToString().Trim()
                        lblNA03.Text = dtTcOae.Rows(0)("m_03").ToString().Trim()

                        txtQA04.Text = dtTcOae.Rows(0)("FD_04").ToString().Trim()
                        lblNA04.Text = dtTcOae.Rows(0)("m_04").ToString().Trim()

                        txtQA05.Text = dtTcOae.Rows(0)("FD_05").ToString().Trim()
                        lblNA05.Text = dtTcOae.Rows(0)("m_05").ToString().Trim()

                        txtQA06.Text = dtTcOae.Rows(0)("FD_06").ToString().Trim()
                        lblNA06.Text = dtTcOae.Rows(0)("m_06").ToString().Trim()

                        txtQA07.Text = dtTcOae.Rows(0)("FD_07").ToString().Trim()
                        lblNA07.Text = dtTcOae.Rows(0)("m_07").ToString().Trim()

                        txtQA08.Text = dtTcOae.Rows(0)("FD_08").ToString().Trim()
                        lblNA08.Text = dtTcOae.Rows(0)("m_08").ToString().Trim()

                        txtQA09.Text = dtTcOae.Rows(0)("FD_09").ToString().Trim()
                        lblNA09.Text = dtTcOae.Rows(0)("m_09").ToString().Trim()

                        txtQA10.Text = dtTcOae.Rows(0)("FD_10").ToString().Trim()
                        lblNA10.Text = dtTcOae.Rows(0)("m_10").ToString().Trim()

                        txtQA11.Text = dtTcOae.Rows(0)("FD_11").ToString().Trim()
                        lblNA11.Text = dtTcOae.Rows(0)("m_11").ToString().Trim()

                        txtQA12.Text = dtTcOae.Rows(0)("FD_12").ToString().Trim()
                        lblNA12.Text = dtTcOae.Rows(0)("m_12").ToString().Trim()

                        txtQB01.Text = dtTcOae.Rows(0)("FD_13").ToString().Trim()
                        lblNB01.Text = dtTcOae.Rows(0)("m_13").ToString().Trim()

                        txtQB02.Text = dtTcOae.Rows(0)("FD_14").ToString().Trim()
                        lblNB02.Text = dtTcOae.Rows(0)("m_14").ToString().Trim()

                        txtQB03.Text = dtTcOae.Rows(0)("FD_15").ToString().Trim()
                        lblNB03.Text = dtTcOae.Rows(0)("m_15").ToString().Trim()

                        txtQB04.Text = dtTcOae.Rows(0)("FD_16").ToString().Trim()
                        lblNB04.Text = dtTcOae.Rows(0)("m_16").ToString().Trim()

                        txtQB05.Text = dtTcOae.Rows(0)("FD_17").ToString().Trim()
                        lblNB05.Text = dtTcOae.Rows(0)("m_17").ToString().Trim()

                        txtQB06.Text = dtTcOae.Rows(0)("FD_18").ToString().Trim()
                        lblNB06.Text = dtTcOae.Rows(0)("m_18").ToString().Trim()

                        If txtQA01.Text <> txtA01.Text Then
                            lblA01.ForeColor = Drawing.Color.Red
                            lblA01.Font.Bold = True
                        End If

                        If txtQA02.Text <> txtA02.Text Then
                            lblA02.ForeColor = Drawing.Color.Red
                            lblA02.Font.Bold = True
                        End If

                        If txtQA03.Text <> txtA03.Text Then
                            lblA03.ForeColor = Drawing.Color.Red
                            lblA03.Font.Bold = True
                        End If

                        If txtQA04.Text <> txtA04.Text Then
                            lblA04.ForeColor = Drawing.Color.Red
                            lblA04.Font.Bold = True
                        End If

                        If txtQA05.Text <> txtA05.Text Then
                            lblA05.ForeColor = Drawing.Color.Red
                            lblA05.Font.Bold = True
                        End If

                        If txtQA06.Text <> txtA06.Text Then
                            lblA06.ForeColor = Drawing.Color.Red
                            lblA06.Font.Bold = True
                        End If

                        If txtQA07.Text <> txtA07.Text Then
                            lblA07.ForeColor = Drawing.Color.Red
                            lblA07.Font.Bold = True
                        End If

                        If txtQA08.Text <> txtA08.Text Then
                            lblA08.ForeColor = Drawing.Color.Red
                            lblA08.Font.Bold = True
                        End If

                        If txtQA09.Text <> txtA09.Text Then
                            lblA09.ForeColor = Drawing.Color.Red
                            lblA09.Font.Bold = True
                        End If

                        If txtQA10.Text <> txtA10.Text Then
                            lblA10.ForeColor = Drawing.Color.Red
                            lblA10.Font.Bold = True
                        End If

                        If txtQA11.Text <> txtA11.Text Then
                            lblA11.ForeColor = Drawing.Color.Red
                            lblA11.Font.Bold = True
                        End If

                        If txtQA12.Text <> txtA12.Text Then
                            lblA12.ForeColor = Drawing.Color.Red
                            lblA12.Font.Bold = True
                        End If

                        If txtQB01.Text <> txtB01.Text Then
                            lblB01.ForeColor = Drawing.Color.Red
                            lblB01.Font.Bold = True
                        End If

                        If txtQB02.Text <> txtB02.Text Then
                            lblB02.ForeColor = Drawing.Color.Red
                            lblB02.Font.Bold = True
                        End If

                        If txtQB03.Text <> txtB03.Text Then
                            lblB03.ForeColor = Drawing.Color.Red
                            lblB03.Font.Bold = True
                        End If

                        If txtQB04.Text <> txtB04.Text Then
                            lblB04.ForeColor = Drawing.Color.Red
                            lblB04.Font.Bold = True
                        End If

                        If txtQB05.Text <> txtB05.Text Then
                            lblB05.ForeColor = Drawing.Color.Red
                            lblB05.Font.Bold = True
                        End If

                        If txtQB06.Text <> txtB06.Text Then
                            lblB06.ForeColor = Drawing.Color.Red
                            lblB06.Font.Bold = True
                        End If

                    End If
                End If


            End If

        Catch ex As Exception
            sMessage = ex.Message
            'Me.ucMessage.showMessageByFailed(sMessage)
        Finally
            'Me.ucMessage.showMessageByFailed(sMessage)
        End Try
    End Sub

    Private Sub ResetValue()
        lblA01.Text = ""
        lblA02.Text = ""
        lblA03.Text = ""
        lblA04.Text = ""
        lblA05.Text = ""
        lblA06.Text = ""
        lblA07.Text = ""
        lblA08.Text = ""
        lblA09.Text = ""
        lblA10.Text = ""
        lblA11.Text = ""
        lblA12.Text = ""
        lblB01.Text = ""
        lblB02.Text = ""
        lblB03.Text = ""
        lblB04.Text = ""
        lblB05.Text = ""
        lblB06.Text = ""

        txtA01.Text = ""
        txtA02.Text = ""
        txtA03.Text = ""
        txtA04.Text = ""
        txtA05.Text = ""
        txtA06.Text = ""
        txtA07.Text = ""
        txtA08.Text = ""
        txtA09.Text = ""
        txtA10.Text = ""
        txtA11.Text = ""
        txtA12.Text = ""
        txtB01.Text = ""
        txtB02.Text = ""
        txtB03.Text = ""
        txtB04.Text = ""
        txtB05.Text = ""
        txtB06.Text = ""

        lblOA01.Text = ""
        lblOA02.Text = ""
        lblOA03.Text = ""
        lblOA04.Text = ""
        lblOA05.Text = ""
        lblOA06.Text = ""
        lblOA07.Text = ""
        lblOA08.Text = ""
        lblOA09.Text = ""
        lblOA10.Text = ""
        lblOB01.Text = ""
        lblOB02.Text = ""
        lblOB03.Text = ""
        lblOB04.Text = ""
        lblOB05.Text = ""
        lblOB06.Text = ""

        txtQA01.Text = ""
        txtQA02.Text = ""
        txtQA03.Text = ""
        txtQA04.Text = ""
        txtQA05.Text = ""
        txtQA06.Text = ""
        txtQA07.Text = ""
        txtQA08.Text = ""
        txtQA09.Text = ""
        txtQA10.Text = ""
        txtQA11.Text = ""
        txtQA12.Text = ""
        txtQB01.Text = ""
        txtQB02.Text = ""
        txtQB03.Text = ""
        txtQB04.Text = ""
        txtQB05.Text = ""
        txtQB06.Text = ""

        lblNA01.Text = ""
        lblNA02.Text = ""
        lblNA03.Text = ""
        lblNA04.Text = ""
        lblNA05.Text = ""
        lblNA06.Text = ""
        lblNA07.Text = ""
        lblNA08.Text = ""
        lblNA09.Text = ""
        lblNA10.Text = ""
        lblNB01.Text = ""
        lblNB02.Text = ""
        lblNB03.Text = ""
        lblNB04.Text = ""
        lblNB05.Text = ""
        lblNB06.Text = ""

        lblA01.ForeColor = Drawing.Color.Black
        lblA01.Font.Bold = False

        lblA02.ForeColor = Drawing.Color.Black
        lblA02.Font.Bold = False

        lblA03.ForeColor = Drawing.Color.Black
        lblA03.Font.Bold = False

        lblA04.ForeColor = Drawing.Color.Black
        lblA04.Font.Bold = False

        lblA05.ForeColor = Drawing.Color.Black
        lblA05.Font.Bold = False

        lblA06.ForeColor = Drawing.Color.Black
        lblA06.Font.Bold = False

        lblA07.ForeColor = Drawing.Color.Black
        lblA07.Font.Bold = False

        lblA08.ForeColor = Drawing.Color.Black
        lblA08.Font.Bold = False

        lblA09.ForeColor = Drawing.Color.Black
        lblA09.Font.Bold = False

        lblA10.ForeColor = Drawing.Color.Black
        lblA10.Font.Bold = False

        lblA11.ForeColor = Drawing.Color.Black
        lblA11.Font.Bold = False

        lblA12.ForeColor = Drawing.Color.Black
        lblA12.Font.Bold = False

        lblB01.ForeColor = Drawing.Color.Black
        lblB01.Font.Bold = False

        lblB02.ForeColor = Drawing.Color.Black
        lblB02.Font.Bold = False

        lblB03.ForeColor = Drawing.Color.Black
        lblB03.Font.Bold = False

        lblB04.ForeColor = Drawing.Color.Black
        lblB04.Font.Bold = False

        lblB05.ForeColor = Drawing.Color.Black
        lblB05.Font.Bold = False

        lblB06.ForeColor = Drawing.Color.Black
        lblB06.Font.Bold = False
    End Sub

    Public Function GetData(ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String) As DataTable
        If OperationCenter <> "CL_CHINA" Then
            OperationCenter = "CLHQ"
        End If

        Dim oExport As New ctlRMA.Export
        'Dim sEWEnd As String = oExport.getWarranty(RMAD_SERIALNO, "")
        'Dim sEWEnd As String = oExport.getMaxWarranty(RMAD_SERIALNO, "", Session("_RepairID").ToString().Trim()) '優化效能 by buck modify 20260323
        Dim sCWEnd As String = oExport.getWarrantyCW(RMAD_SERIALNO, "")
        'Dim sSWEnd As String = oExport.getWarrantySW(RMAD_SERIALNO, "") '優化效能 by buck modify 20260323
        Dim sWarDate As String = oExport.getWarrantyStart(RMAD_SERIALNO)
        'Dim sWarVersion As String = String.Empty '優化效能 by buck modify 20260323

        Dim sWar_id As String = ""
        Dim oWarranty As New ctlWarranty
        Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(RMAD_SERIALNO)
        If dtPur.Rows.Count > 0 Then
            sWar_id = dtPur.Rows(0)("wati_ver").ToString()
            '2024/12/23 開會決議 只有出工廠時的標準保才有零件年限 加購沒有
            'sWarDate = DateTime.Parse(dtPur.Rows(0)("waty_date").ToString()).ToString("yyyy/MM/dd")
        Else

            Dim octlWarranty As New ctlRMA.Export
            Dim EXPORT_WAR_ID As String = octlWarranty.getEXPORT(RMAD_SERIALNO)
            sWar_id = EXPORT_WAR_ID

        End If

        If sWar_id = "" Then
            '優化效能 by buck modify 20260323 begin
            Dim dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, If(sCWEnd.Trim() <> "", "CW", "EW"), "", "Y", "WAR_VERSION")
            'Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
            'If sCWEnd.Trim() <> "" Then
            '    dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "CW", "", "Y", "WAR_VERSION")
            'Else
            '    dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "EW", "", "Y", "WAR_VERSION")
            'End If
            '優化效能 by buck modify 20260323 end

            If dtEwVer.Rows.Count > 0 Then
                If dtEwVer.Rows(0)("WAR_VERSION").ToString().Trim().Equals("0") Then
                    sWar_id = dtEwVer.Rows(0)("WAR_ID").ToString()
                End If

                '20200217 wisely modify 抓訂單細目的版本
                Dim sWarVersion = oWarranty.QueryWARVERSION(RMAD_SERIALNO) '優化效能 by buck modify 20260323
                If sWarVersion <> String.Empty Then
                    Dim find_rows As DataRow() = dtEwVer.Select("WAR_VERSION='" + sWarVersion + "'")
                    If find_rows.Length > 0 Then
                        sWar_id = find_rows(0)("WAR_ID").ToString()
                    End If
                End If
            End If

        End If
        If sWarDate <> "" Then
            sWarDate = DateTime.Parse(sWarDate).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            sWar_id = "123$321"
        End If

        '優化效能 by buck modify 20260323 begin
        ' 取得資料
        Dim dtWarrParts As WarrantyDTO.WarrPartsDataTable = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")

        ' 先增加欄位
        If Not dtWarrParts.Columns.Contains("PODate") Then dtWarrParts.Columns.Add("PODate")
        If Not dtWarrParts.Columns.Contains("WarrEndDate") Then dtWarrParts.Columns.Add("WarrEndDate")

        Dim startDate As DateTime
        Dim hasValidDate As Boolean = DateTime.TryParse(sWarDate, startDate)

        dtWarrParts.AsEnumerable().ToList().
            ForEach(Sub(row)
                        row("PODate") = sWarDate

                        If hasValidDate Then
                            Dim totalMonths As Double = Convert.ToDouble(If(IsDBNull(row("WAP_MON")), 0, row("WAP_MON"))) +
                                    Convert.ToDouble(If(IsDBNull(row("WAP_EMON")), 0, row("WAP_EMON")))

                            row("WarrEndDate") = startDate.AddMonths(CInt(totalMonths)).AddDays(-1).ToString("yyyy/MM/dd")
                        End If
                    End Sub)

        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        ''sWar_id = "9700CW00125"
        'dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")
        'dtWarrParts.Columns.Add("PODate")
        'dtWarrParts.Columns.Add("WarrEndDate")

        'Dim i As Integer
        'For i = 0 To dtWarrParts.Rows.Count - 1
        '    dtWarrParts.Rows(i)("PODate") = sWarDate
        '    If sWarDate <> "" Then
        '        dtWarrParts.Rows(i)("WarrEndDate") = DateTime.Parse(sWarDate).AddMonths(Double.Parse(dtWarrParts.Rows(i)("WAP_MON").ToString()) + Double.Parse(dtWarrParts.Rows(i)("WAP_EMON").ToString())).AddDays(-1).ToString("yyyy/MM/dd")
        '    End If
        'Next
        '優化效能 by buck modify 20260323 end
        'Response.Write(sWar_id + "-" + dtWarrParts.Rows.Count.ToString())

        Return dtWarrParts
    End Function

End Class

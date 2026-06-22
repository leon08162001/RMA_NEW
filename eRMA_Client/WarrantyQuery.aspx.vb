Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class WarrantyQuery
    Inherits System.Web.UI.Page

    Dim _oLanguage As New ctlLanguage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then

            pnlVersion.Visible = False
            found_Panel.Visible = False
            Call setDefault()

        End If
    End Sub

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function

    Public Function getoLanguageword(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Word)
    End Function

    Private Sub setDefault()
        Me.Warranty_Query_Label.Text = _oLanguage.getText("Report", "003", ctlLanguage.eumType.Tag)
        Me.SearchBtn.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.sRMANo_Txt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag))
        Me.UI_lblPriceList.Text = _oLanguage.getText("Warranty", "064", ctlLanguage.eumType.Tag)
        Me.lblPurchasing.Text = _oLanguage.getText("Warranty", "065", ctlLanguage.eumType.Tag)

        Me.lblAccessoriesInformation.Text = _oLanguage.getText("RMA2", "101", ctlLanguage.eumType.Tag)
        Me.lblVerChange.Text = _oLanguage.getText("RMA2", "102", ctlLanguage.eumType.Tag)
        Me.lblInformation.Text = _oLanguage.getText("RMA2", "103", ctlLanguage.eumType.Tag)
        Me.UI_lblPriceList.Visible = False
        Me.lblPurchasing.Visible = False
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
        dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.sRMANo_Txt.Text.Trim())

        Try

            '新建Table
            Dim RMADetailDataTable_ As DataTable
            RMADetailDataTable_ = New DataTable("RMADetailDataTable")

            Dim EXPORT_CUSTOMERNAME_column As DataColumn = New DataColumn("EXPORT_CUSTOMERNAME")
            EXPORT_CUSTOMERNAME_column.DataType = System.Type.GetType("System.String")

            Dim EXPORT_PARTNO_column As DataColumn = New DataColumn("EXPORT_PARTNO")
            EXPORT_PARTNO_column.DataType = System.Type.GetType("System.String")

            Dim EXPORT_SHIPPING_TIME_column As DataColumn = New DataColumn("EXPORT_SHIPPING_TIME")
            EXPORT_SHIPPING_TIME_column.DataType = System.Type.GetType("System.String")

            Dim CW_EDATE_column As DataColumn = New DataColumn("CW_EDATE")
            CW_EDATE_column.DataType = System.Type.GetType("System.String")

            Dim EXPORT_SERIALNO_column As DataColumn = New DataColumn("EXPORT_SERIALNO")
            EXPORT_SERIALNO_column.DataType = System.Type.GetType("System.String")

            Dim EXPORT_WARRANTY_DATE_column As DataColumn = New DataColumn("EXPORT_WARRANTY_DATE")
            EXPORT_WARRANTY_DATE_column.DataType = System.Type.GetType("System.String")

            Dim WAR_TYPE_column As DataColumn = New DataColumn("WAR_TYPE")
            WAR_TYPE_column.DataType = System.Type.GetType("System.String")


            Dim Context_column As DataColumn = New DataColumn("Context")
            Context_column.DataType = System.Type.GetType("System.String")

            RMADetailDataTable_.Columns.Add(EXPORT_CUSTOMERNAME_column)
            RMADetailDataTable_.Columns.Add(EXPORT_PARTNO_column)
            RMADetailDataTable_.Columns.Add(EXPORT_SHIPPING_TIME_column)
            RMADetailDataTable_.Columns.Add(CW_EDATE_column)
            RMADetailDataTable_.Columns.Add(EXPORT_SERIALNO_column)
            RMADetailDataTable_.Columns.Add(EXPORT_WARRANTY_DATE_column)
            RMADetailDataTable_.Columns.Add(WAR_TYPE_column)
            RMADetailDataTable_.Columns.Add(Context_column)


            Dim RMADetail_Row As DataRow
            RMADetail_Row = RMADetailDataTable_.NewRow()
            Dim Context As String = ""
            For i = 0 To dtReport.Rows.Count - 1

                If i = 0 Then

                    'RMA2
                    If dtReport.Rows(i)("EXPORT_CUSTOMERNAME") Is DBNull.Value Then

                    Else

                        RMADetail_Row("EXPORT_CUSTOMERNAME") = dtReport.Rows(i)("EXPORT_CUSTOMERNAME").ToString().Trim()
                    End If

                    If dtReport.Rows(i)("EXPORT_PARTNO") Is DBNull.Value Then
                    Else
                        RMADetail_Row("EXPORT_PARTNO") = dtReport.Rows(i)("EXPORT_PARTNO").ToString().Trim()
                    End If

                    If dtReport.Rows(i)("EXPORT_SHIPPING_TIME") Is DBNull.Value Then
                    Else
                        RMADetail_Row("EXPORT_SHIPPING_TIME") = dtReport.Rows(i)("EXPORT_SHIPPING_TIME").ToString().Trim()
                    End If

                    If dtReport.Rows(i)("CW_EDATE") Is DBNull.Value Then
                    Else
                        RMADetail_Row("CW_EDATE") = dtReport.Rows(i)("CW_EDATE").ToString().Trim()
                    End If

                    If dtReport.Rows(i)("EXPORT_SERIALNO") Is DBNull.Value Then
                    Else
                        RMADetail_Row("EXPORT_SERIALNO") = dtReport.Rows(i)("EXPORT_SERIALNO").ToString().Trim()
                    End If
                    If dtReport.Rows(i)("EXPORT_WARRANTY_DATE") Is DBNull.Value Then
                    Else
                        RMADetail_Row("EXPORT_WARRANTY_DATE") = dtReport.Rows(i)("EXPORT_WARRANTY_DATE").ToString().Trim()
                    End If
                    If dtReport.Rows(i)("WAR_TYPE") Is DBNull.Value Then
                    Else
                        RMADetail_Row("WAR_TYPE") = dtReport.Rows(i)("WAR_TYPE").ToString().Trim()
                    End If
                End If

                If i = 1 Then
                End If

                If i > 1 Then

                    Context += "<tr>"
                    Context += "<td>"
                    If dtReport.Rows(i)("EXPORT_PARTNO") Is DBNull.Value Then
                    Else
                        Context += dtReport.Rows(i)("EXPORT_PARTNO").ToString().Trim()
                    End If
                    Context += "</td>"
                    Context += "<td>"
                    If dtReport.Rows(i)("EXPORT_SERIALNO") Is DBNull.Value Then
                    Else
                        Context += dtReport.Rows(i)("EXPORT_SERIALNO").ToString().Trim()
                    End If
                    Context += "</td>"
                    Context += "</tr>"

                End If

            Next
            RMADetail_Row("Context") = Context
            RMADetailDataTable_.Rows.Add(RMADetail_Row)

            If dtReport.Rows.Count > 0 Then
                Me.UI_dvRMAListView.DataSource = RMADetailDataTable_
                Me.UI_dvRMAListView.DataBind()
                Me.UI_NotFound_Label.Text = ""
                Me.UI_NotFound_Label.Visible = False
                Me.UI_lblPriceList.Visible = True
                Me.lblPurchasing.Visible = True

                Dim RMADetailDataTable_New As New DataTable
                Me.dgvPurchasing.DataSource = RMADetailDataTable_New
                Me.dgvPurchasing.DataBind()

                Call dgvPurchasing_QueryData(0)
                Me.AccessoriesInformationPanel.Visible = True
                Me.ermatablecomponentsPanel.Visible = True

            Else
                Dim RMADetailDataTable_New As New DataTable
                Me.UI_dvRMAListView.DataSource = RMADetailDataTable_New
                Me.UI_dvRMAListView.DataBind()

                Me.dgvPurchasing.DataSource = RMADetailDataTable_New
                Me.dgvPurchasing.DataBind()


                Me.UI_NotFound_Label.Text = _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag)
                Me.ermatablecomponentsPanel.Visible = False
                Me.UI_NotFound_Label.Visible = True

                Me.UI_lblPriceList.Visible = False
                Me.lblPurchasing.Visible = False

                Me.AccessoriesInformationPanel.Visible = False
            End If



        Catch ex As Exception

        End Try

    End Sub

    Private Sub dgvPurchasing_QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable

        Try

            '新建Table
            Dim RMADetailDataTable_ As DataTable
            RMADetailDataTable_ = New DataTable("RMADetailDataTable")

            Dim SerialNo_column As DataColumn = New DataColumn("SerialNo")
            SerialNo_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(SerialNo_column)

            Dim PurchaseDate_column As DataColumn = New DataColumn("PurchaseDate")
            PurchaseDate_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(PurchaseDate_column)

            Dim WarrantyCode_column As DataColumn = New DataColumn("WarrantyCode")
            WarrantyCode_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(WarrantyCode_column)

            Dim StartDate_column As DataColumn = New DataColumn("StartDate")
            StartDate_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(StartDate_column)

            Dim EndDate_column As DataColumn = New DataColumn("EndDate")
            EndDate_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(EndDate_column)

            Dim Model_column As DataColumn = New DataColumn("Model")
            Model_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(Model_column)


            Dim SKU_column As DataColumn = New DataColumn("SKU")
            SKU_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(SKU_column)

            Dim Description_column As DataColumn = New DataColumn("Description")
            Description_column.DataType = System.Type.GetType("System.String")
            RMADetailDataTable_.Columns.Add(Description_column)

            Dim RMADetail_Row As DataRow
            RMADetail_Row = RMADetailDataTable_.NewRow()
            Dim Context As String = ""


            '序號 => 保固 by Hugh 20231103, 排序抓最新一筆 RMA加購保固
            Dim dtImport As DataTable = Session("_dtPurchasing")


            If dtImport.Rows(0)("SerialNo").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("SerialNo") = dtImport.Rows(0)("SerialNo").ToString().Trim()
            End If

            If dtImport.Rows(0)("PurchaseDate").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("PurchaseDate") = dtImport.Rows(0)("PurchaseDate").ToString().Trim()
            End If


            If dtImport.Rows(0)("WarrantyCode").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("WarrantyCode") = dtImport.Rows(0)("WarrantyCode").ToString().Trim()
            End If


            If dtImport.Rows(0)("StartDate").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("StartDate") = dtImport.Rows(0)("StartDate").ToString().Trim()
            End If


            If dtImport.Rows(0)("EndDate").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("EndDate") = dtImport.Rows(0)("EndDate").ToString().Trim()
            End If

            If dtImport.Rows(0)("Model").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("Model") = dtImport.Rows(0)("Model").ToString().Trim()
            End If

            If dtImport.Rows(0)("SKU").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("SKU") = dtImport.Rows(0)("SKU").ToString().Trim()
            End If

            If dtImport.Rows(0)("Description").ToString().Trim() Is DBNull.Value Then

            Else
                RMADetail_Row("Description") = dtImport.Rows(0)("Description").ToString().Trim()
            End If

            RMADetailDataTable_.Rows.Add(RMADetail_Row)

            If dtImport.Rows.Count > 0 Then
                Me.dgvPurchasing.DataSource = RMADetailDataTable_
                Me.dgvPurchasing.DataBind()

            Else
                Dim RMADetailDataTable_New As New DataTable
                Me.dgvPurchasing.DataSource = RMADetailDataTable_New
                Me.dgvPurchasing.DataBind()

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetDataTable()
        Dim dtImport As New DataTable
        dtImport.Columns.Add("Seq")
        dtImport.Columns.Add("SerialNo")
        dtImport.Columns.Add("CWStart")
        dtImport.Columns.Add("CWEnd")
        dtImport.Columns.Add("EWStart")
        dtImport.Columns.Add("EWEnd")
        dtImport.Columns.Add("WAR_TYPE")
        dtImport.Columns.Add("WAR_PROGRAM_TYPE")
        dtImport.Columns.Add("WAR_ITEM_TYPE")
        dtImport.Columns.Add("WAR_PRICE_VER")
        dtImport.Columns.Add("Model")
        dtImport.Columns.Add("SKU")
        dtImport.Columns.Add("Description")
        dtImport.Columns.Add("OrderNo")
        dtImport.Columns.Add("Customer")
        dtImport.Columns.Add("OrderDate")
        dtImport.Columns.Add("ShipNo")
        dtImport.Columns.Add("DeliverDate")
        Session("_dtImport") = dtImport

        Dim dtPurchasing As New DataTable
        dtPurchasing.Columns.Add("Seq")
        dtPurchasing.Columns.Add("SerialNo")
        dtPurchasing.Columns.Add("PurchaseDate")
        dtPurchasing.Columns.Add("WarrantyCode")
        dtPurchasing.Columns.Add("StartDate")
        dtPurchasing.Columns.Add("EndDate")
        dtPurchasing.Columns.Add("Model")
        dtPurchasing.Columns.Add("SKU")
        dtPurchasing.Columns.Add("Description")
        Session("_dtPurchasing") = dtPurchasing

    End Sub

    Private Sub ArrangementImportData(ByVal dtTmpImport As DataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim iPurCount As Integer = 0
        Dim sRMANo As String = ""

        Dim dtImport As DataTable = Session("_dtImport")
        dtImport.Rows.Clear()

        Dim dtPurchasing As DataTable = Session("_dtPurchasing")
        dtPurchasing.Rows.Clear()

        Dim oWarranty As New ctlWarranty
        For i = 0 To dtTmpImport.Rows.Count - 1
            If dtTmpImport.Rows(i)(0).ToString().Trim() <> "" Then
                Dim sSerailNo As String = dtTmpImport.Rows(i)(0).ToString().Trim()
                iCount = iCount + 1
                dtImport.NewRow()
                dtImport.Rows.Add(New Object() {})
                dtImport.Rows(dtImport.Rows.Count - 1)("Seq") = iCount
                dtImport.Rows(dtImport.Rows.Count - 1)("SerialNo") = sSerailNo

                Dim dtExport As DataTable = oWarranty.QueryExport(sSerailNo)
                If dtExport.Rows.Count > 0 Then
                    If dtExport.Rows(0)("cw_sdate").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("CWStart") = Convert.ToDateTime(dtExport.Rows(0)("cw_sdate").ToString()).ToString("yyyy/MM/dd")
                    End If
                    If dtExport.Rows(0)("cw_edate").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("CWEnd") = Convert.ToDateTime(dtExport.Rows(0)("cw_edate").ToString()).ToString("yyyy/MM/dd")
                    End If
                    If dtExport.Rows(0)("ew_sdate").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("EWStart") = Convert.ToDateTime(dtExport.Rows(0)("ew_sdate").ToString()).ToString("yyyy/MM/dd")
                    End If
                    If dtExport.Rows(0)("EXPORT_WARRANTY_DATE").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("EWEnd") = Convert.ToDateTime(dtExport.Rows(0)("EXPORT_WARRANTY_DATE").ToString()).ToString("yyyy/MM/dd")
                    End If
                    If dtExport.Rows(0)("WAR_TYPE").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("WAR_TYPE") = dtExport.Rows(0)("WAR_TYPE").ToString()
                    End If
                    If dtExport.Rows(0)("WAR_PROGRAM_TYPE").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("WAR_PROGRAM_TYPE") = dtExport.Rows(0)("WAR_PROGRAM_TYPE").ToString()
                    End If
                    If dtExport.Rows(0)("WAR_ITEM_TYPE").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("WAR_ITEM_TYPE") = dtExport.Rows(0)("WAR_ITEM_TYPE").ToString()
                    End If
                    If dtExport.Rows(0)("WAR_PRICE_VER").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("WAR_PRICE_VER") = dtExport.Rows(0)("WAR_PRICE_VER").ToString()
                    End If

                    dtImport.Rows(dtImport.Rows.Count - 1)("Model") = dtExport.Rows(0)("export_modelno").ToString()
                    dtImport.Rows(dtImport.Rows.Count - 1)("SKU") = dtExport.Rows(0)("export_partno").ToString()
                    dtImport.Rows(dtImport.Rows.Count - 1)("Description") = ""
                    dtImport.Rows(dtImport.Rows.Count - 1)("OrderNo") = ""
                    dtImport.Rows(dtImport.Rows.Count - 1)("Customer") = dtExport.Rows(0)("export_custno").ToString()
                    If dtExport.Rows(0)("export_shipping_time").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("OrderDate") = Convert.ToDateTime(dtExport.Rows(0)("export_shipping_time").ToString()).ToString("yyyy/MM/dd")
                    End If

                    dtImport.Rows(dtImport.Rows.Count - 1)("ShipNo") = dtExport.Rows(0)("EXPORT_ORDERNUMBER").ToString()
                    If dtExport.Rows(0)("export_shipping_time").ToString() <> "" Then
                        dtImport.Rows(dtImport.Rows.Count - 1)("DeliverDate") = Convert.ToDateTime(dtExport.Rows(0)("export_shipping_time").ToString()).ToString("yyyy/MM/dd")
                    End If
                End If

                Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(sSerailNo)
                If dtPur.Rows.Count > 0 Then
                    Dim k As Integer
                    For k = 0 To dtPur.Rows.Count - 1
                        iPurCount = iPurCount + 1
                        dtPurchasing.NewRow()
                        dtPurchasing.Rows.Add(New Object() {})
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Seq") = iPurCount
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SerialNo") = sSerailNo

                        If dtPur.Rows(k)("waty_date").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("PurchaseDate") = Convert.ToDateTime(dtPur.Rows(k)("waty_date").ToString()).ToString("yyyy/MM/dd")
                        End If

                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("WarrantyCode") = dtPur.Rows(k)("wati_type").ToString()

                        If dtPur.Rows(k)("wats_warrnstart").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("StartDate") = Convert.ToDateTime(dtPur.Rows(k)("wats_warrnstart").ToString()).ToString("yyyy/MM/dd")
                        End If

                        If dtPur.Rows(k)("wats_warrnend").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("EndDate") = Convert.ToDateTime(dtPur.Rows(k)("wats_warrnend").ToString()).ToString("yyyy/MM/dd")
                        End If

                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Model") = dtPur.Rows(k)("wati_model").ToString()
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SKU") = dtPur.Rows(k)("wati_skuno").ToString()
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Description") = dtPur.Rows(k)("wati_name").ToString()
                    Next
                Else
                    'iPurCount = iPurCount + 1
                    'dtPurchasing.NewRow()
                    'dtPurchasing.Rows.Add(New Object() {})
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Seq") = iPurCount
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SerialNo") = sSerailNo
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("PurchaseDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("WarrantyCode") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("StartDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("EndDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Model") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SKU") = ""
                End If
            End If
        Next

        Session("_dtImport") = dtImport
        Session("_dtPurchasing") = dtPurchasing
    End Sub

    Protected Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Call SetDataTable()

        ViewState("_SKU") = ""
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("SN")
        Dim dr As DataRow = dt.NewRow()
        dr(0) = sRMANo_Txt.Text.Trim()
        dt.Rows.Add(dr)

        ArrangementImportData(dt)

        Call QueryData(0)
        Call QueryData_Report3_Search(0)
    End Sub

    Protected Sub UI_CheckVer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If UI_CheckVer.Checked Then
            pnlVersion.Visible = True
        Else
            pnlVersion.Visible = False
        End If
    End Sub

    ' 老闆 對 PM 對 其他 附全責阿 開始
    Private Sub QueryData_Report3_Search(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable

        dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.sRMANo_Txt.Text.Trim())
        Dim sSnNo As String = ""
        If dtReport.Rows.Count > 0 Then
            sSnNo = dtReport.Rows(0)("EXPORT_SERIALNO").ToString().Trim()
        End If

        Call GetVersigonData(Me.sRMANo_Txt.Text.Trim())

        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = oExport.getModelNo(sSnNo)

        If sSnNo <> "" Then
            Dim dtWarrParts As DataTable = GetData(sSnNo, sModelNo, "")
            Me.ViewState("dtWarrParts") = dtWarrParts
            lstParts.DataSource = dtWarrParts
            lstParts.DataBind()
        End If


        Try

            Dim EXPORT_WAR_ID As String = dtReport.Rows(0)("EXPORT_WAR_ID").ToString().Trim()
            If EXPORT_WAR_ID <> "" Then

                sSnNo = dtReport.Rows(0)("EXPORT_SERIALNO").ToString().Trim()

                '20230808

                Dim Warranty_TypeSetting_add_oWarranty As New ctlWarranty
                Dim Warranty_TypeSetting_add_dtWarrSpecs As New DataTable
                Warranty_TypeSetting_add_dtWarrSpecs = Warranty_TypeSetting_add_oWarranty.QueryWarrSpecs(Session("_LanguageID").ToString(), EXPORT_WAR_ID, "", "")
                'Warranty_TypeSetting_add_lstSpecs.DataSource = Warranty_TypeSetting_add_dtWarrSpecs
                'Warranty_TypeSetting_add_lstSpecs.DataBind()

            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Function GetData(ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String) As DataTable
        If OperationCenter <> "CL_CHINA" Then
            OperationCenter = "CLHQ"
        End If

        Dim oExport As New ctlRMA.Export
        'Dim sEWEnd As String = oExport.getWarranty(RMAD_SERIALNO, "")
        Dim sEWEnd As String = oExport.getMaxWarranty(RMAD_SERIALNO, "", Session("_RepairID").ToString().Trim())
        Dim sCWEnd As String = oExport.getWarrantyCW(RMAD_SERIALNO, "")
        Dim sSWEnd As String = oExport.getWarrantySW(RMAD_SERIALNO, "")
        Dim sWarDate As String = oExport.getWarrantyStart(RMAD_SERIALNO)
        Dim sWarVersion As String = String.Empty

        Dim sWar_id As String = ""
        Dim oWarranty As New ctlWarranty
        Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(RMAD_SERIALNO)
        If dtPur.Rows.Count > 0 Then
            sWar_id = dtPur.Rows(0)("wati_ver").ToString()
            '2024/12/23 開會決議 只有出工廠時的標準保才有零件年限 加購沒有
            'sWarDate = DateTime.Parse(dtPur.Rows(0)("waty_date").ToString()).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
            If sCWEnd.Trim() <> "" Then
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "CW", "", "Y", "WAR_VERSION")
            Else
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "EW", "", "Y", "WAR_VERSION")
            End If

            If dtEwVer.Rows.Count > 0 Then
                If dtEwVer.Rows(0)("WAR_VERSION").ToString().Trim().Equals("0") Then
                    sWar_id = dtEwVer.Rows(0)("WAR_ID").ToString()
                End If

                '20200217 wisely modify 抓訂單細目的版本
                sWarVersion = oWarranty.QueryWARVERSION(RMAD_SERIALNO)
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

        Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        'sWar_id = "9700CW00125"
        dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")
        dtWarrParts.Columns.Add("PODate")
        dtWarrParts.Columns.Add("WarrEndDate")

        Dim i As Integer
        For i = 0 To dtWarrParts.Rows.Count - 1
            dtWarrParts.Rows(i)("PODate") = sWarDate
            If sWarDate <> "" Then
                dtWarrParts.Rows(i)("WarrEndDate") = DateTime.Parse(sWarDate).AddMonths(Double.Parse(dtWarrParts.Rows(i)("WAP_MON").ToString()) + Double.Parse(dtWarrParts.Rows(i)("WAP_EMON").ToString())).AddDays(-1).ToString("yyyy/MM/dd")
            End If
        Next
        'Response.Write(sWar_id + "-" + dtWarrParts.Rows.Count.ToString())

        Return dtWarrParts
    End Function

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

    ' 老闆 對 PM 對 其他 附全責阿 簡單說 他當PM 專案他負責 你要負嗎 不負閉嘴 結束

End Class

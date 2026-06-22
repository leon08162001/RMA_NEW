Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel

Partial Class Warranty_SerialSearch
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim sUploadPath As String = ConfigurationSettings.AppSettings("EmailAttachFolder")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SKU") = ""

            Me.ViewState("_ImpSortExpression") = "SerialNo"
            Me.ViewState("_ImpSortDirection") = "asc"

            Me.ViewState("_PurSortExpression") = "SerialNo"
            Me.ViewState("_PurSortDirection") = "asc"

            Session("_dtImport") = Nothing
            Session("_dtPurchasing") = Nothing

            Dim oWarranty As New ctlWarranty
            Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
            dtProductGroup = oWarranty.QueryPrdGroup("", "")
            ViewState("_dtProductGroup") = dtProductGroup

            Call setControls()
            Call SetDataTable()
            Call QueryData(0)
            '需求新增:BI保固 By buck Add 20250902 begin
            Dim prevUrl As String = String.Empty
            If Request.UrlReferrer IsNot Nothing Then
                prevUrl = Request.UrlReferrer.AbsolutePath.ToLower()
            End If

            If prevUrl.Contains("report3_search.aspx") Then
                txtSKU.Text = Request.QueryString("SerialNo")
                If Not String.IsNullOrEmpty(txtSKU.Text) Then
                    UI_cmdSearch_Click(Nothing, EventArgs.Empty)
                End If
            End If
            '需求新增:BI保固 By buck Add 20250902 end
        End If
    End Sub

    Private Sub setControls()

        '取得Tag Text
        Me.lblTittle.Text = _oLanguage.getText("Warranty", "063", ctlLanguage.eumType.Tag)
        Me.UI_lblPriceList.Text = _oLanguage.getText("Warranty", "064", ctlLanguage.eumType.Tag)
        Me.lblPurchasing.Text = _oLanguage.getText("Warranty", "065", ctlLanguage.eumType.Tag)
        Me.lblImport.Text = _oLanguage.getText("Warranty", "066", ctlLanguage.eumType.Tag)
        'Me.lblSKU.Text = _oLanguage.getText("Warranty", "073", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.btnImport.Text = _oLanguage.getText("RMA", "211", ctlLanguage.eumType.Command)

        'Me.dgvImport.Columns(1).HeaderText = _oLanguage.getText("Warranty", "067", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(2).HeaderText = _oLanguage.getText("Warranty", "068", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(3).HeaderText = _oLanguage.getText("Warranty", "069", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(4).HeaderText = _oLanguage.getText("Warranty", "070", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(5).HeaderText = _oLanguage.getText("Warranty", "071", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(6).HeaderText = _oLanguage.getText("Warranty", "072", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(7).HeaderText = _oLanguage.getText("Warranty", "073", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(8).HeaderText = _oLanguage.getText("Warranty", "074", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(9).HeaderText = _oLanguage.getText("Warranty", "075", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(10).HeaderText = _oLanguage.getText("Warranty", "076", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(11).HeaderText = _oLanguage.getText("Warranty", "077", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(12).HeaderText = _oLanguage.getText("Warranty", "078", ctlLanguage.eumType.Tag)
        'Me.dgvImport.Columns(13).HeaderText = _oLanguage.getText("Warranty", "079", ctlLanguage.eumType.Tag)

        Me.dgvPurchasing.Columns(1).HeaderText = _oLanguage.getText("Warranty", "067", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(2).HeaderText = _oLanguage.getText("Warranty", "080", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(3).HeaderText = _oLanguage.getText("Warranty", "081", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(4).HeaderText = _oLanguage.getText("Warranty", "069", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(5).HeaderText = _oLanguage.getText("Warranty", "070", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(6).HeaderText = _oLanguage.getText("Warranty", "072", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(7).HeaderText = _oLanguage.getText("Warranty", "073", ctlLanguage.eumType.Tag)
        Me.dgvPurchasing.Columns(8).HeaderText = _oLanguage.getText("Warranty", "074", ctlLanguage.eumType.Tag)

        Dim dtTmp As New DataTable
        dtTmp.Columns.Add("SEQID")
        dtTmp.Columns.Add("war_id")
        dtTmp.Columns.Add("OperationCenter")
        dtTmp.Columns.Add("ProductGroup")
        dtTmp.Columns.Add("WarrantyType")
        dtTmp.Columns.Add("Version")
        dtTmp.Columns.Add("WarrantyName")
        dtTmp.Columns.Add("ExtraMonths")
        dtTmp.Columns.Add("StdYears")
        dtTmp.Columns.Add("LongestYears")
        dtTmp.Columns.Add("Discount")
        dtTmp.Columns.Add("Status")

        Session("_dtTmp") = dtTmp
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
        dtImport.Columns.Add("CustomerName")
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

    Private Sub QueryDataImport(ByVal iPageIndex As Integer)
        Dim dtImport As DataTable = Session("_dtImport")
        Dim dvImport As DataView = dtImport.DefaultView

        Call RMA_DataBindImport(dvImport, iPageIndex)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim dtImport As DataTable = Session("_dtImport")
        Dim dtPurchasing As DataTable = Session("_dtPurchasing")

        Dim sSKU As String = ViewState("_SKU")

        If sSKU <> "" Then
            dtImport.DefaultView.RowFilter = "SerialNo='" & sSKU & "'"
        Else
            dtImport.DefaultView.RowFilter = ""
        End If
        Dim dvImport As DataView = dtImport.DefaultView
        Call RMA_DataBindImport(dvImport, iPageIndex)
        If sSKU <> "" Then
            dtPurchasing.DefaultView.RowFilter = "SerialNo='" & sSKU & "'"
        Else
            dtPurchasing.DefaultView.RowFilter = ""
        End If
        Dim dvPurchasing As DataView = dtPurchasing.DefaultView
        Call RMA_DataBindPur(dvPurchasing, iPageIndex)
    End Sub

    Private Sub RMA_DataBindImport(ByVal dvImport As DataView, ByVal iPageIndex As Integer)
        dvImport.Sort = Me.ViewState("_ImpSortExpression").ToString() & " " & Me.ViewState("_ImpSortDirection").ToString()
        Call CreateImpKeyPoint(Me.ViewState("_ImpSortExpression").ToString(), Me.ViewState("_ImpSortDirection").ToString())

        Me.dgvImport.PageSize = _PageSize
        Me.dgvImport.PageIndex = iPageIndex
        Me.dgvImport.DataSource = dvImport
        Me.dgvImport.DataBind()
    End Sub

    Private Sub RMA_DataBindPur(ByVal dvPurchasing As DataView, ByVal iPageIndex As Integer)
        dvPurchasing.Sort = Me.ViewState("_PurSortExpression").ToString() & " " & Me.ViewState("_PurSortDirection").ToString()
        Call CreatePurKeyPoint(Me.ViewState("_PurSortExpression").ToString(), Me.ViewState("_PurSortDirection").ToString())

        Me.dgvPurchasing.PageSize = _PageSize
        Me.dgvPurchasing.PageIndex = iPageIndex
        Me.dgvPurchasing.DataSource = dvPurchasing
        Me.dgvPurchasing.DataBind()
    End Sub

    Private Sub CreateImpKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.dgvImport.Columns.Count - 1
            Dim sHeaderText As String = Me.dgvImport.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.dgvImport.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_ImpSortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.dgvImport.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.dgvImport.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.dgvImport.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Private Sub CreatePurKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.dgvPurchasing.Columns.Count - 1
            Dim sHeaderText As String = Me.dgvPurchasing.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.dgvPurchasing.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_PurSortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.dgvPurchasing.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.dgvPurchasing.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.dgvPurchasing.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
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

                ViewState("strPath") = sUploadPath + "\\" + sFileName
                Dim oDSImport As DataSet = Excel2007ToDataSet(ViewState("strPath").ToString())
                ArrangementImportData(oDSImport.Tables(0))
                QueryData(0)
                blnFlag = True
            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
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
                    dtImport.Rows(dtImport.Rows.Count - 1)("CustomerName") = dtExport.Rows(0)("EXPORT_CUSTOMERNAME").ToString()
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

    Protected Sub dgvImport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvImport.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.dgvImport.PageIndex * Me.dgvImport.PageSize) + (e.Row.RowIndex + 1).ToString()
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

    Protected Sub dgvImport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvImport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtImport") Is Nothing Then
            Dim dtImport As DataTable = Session("_dtImport")
            Dim dvImport As DataView = dtImport.DefaultView
            Call RMA_DataBindImport(dvImport, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub dgvImport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvImport.Sorting

        If Me.ViewState("_ImpSortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_ImpSortDirection") = "asc"
        Else
            If Me.ViewState("_ImpSortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_ImpSortDirection") = "desc"
            Else
                Me.ViewState("_ImpSortDirection") = "asc"
            End If
        End If
        Me.ViewState("_ImpSortExpression") = e.SortExpression

        If IsNothing(Session("_dtImport")) = False Then
            Dim dtImport As DataTable = Session("_dtImport")
            Dim dvImport As DataView = dtImport.DefaultView
            Call RMA_DataBindImport(dvImport, 0)
        End If
    End Sub

    Protected Sub dgvPurchasing_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvPurchasing.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.dgvPurchasing.PageIndex * Me.dgvPurchasing.PageSize) + (e.Row.RowIndex + 1).ToString()
            Dim WAR_SPEC_DESC As Label = e.Row.FindControl("WAR_SPEC_DESC")

            Dim ctlWarranty_ As New ctlWarranty
            Dim WARRSET_WAR_SPEC_DESC_DataTable As New DataTable
            WARRSET_WAR_SPEC_DESC_DataTable = ctlWarranty_.Select_WARRSET_WAR_SPEC_DESC(e.Row.Cells(1).Text)
            If Not WARRSET_WAR_SPEC_DESC_DataTable.Rows(0)("WAR_SPEC_DESC") Is DBNull.Value Then
                WAR_SPEC_DESC.Text = WARRSET_WAR_SPEC_DESC_DataTable.Rows(0)("WAR_SPEC_DESC").ToString()
            Else
                WAR_SPEC_DESC.Text = ""

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

    Protected Sub dgvPurchasing_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvPurchasing.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtPurchasing") Is Nothing Then
            Dim dtPurchasing As DataTable = Session("_dtPurchasing")
            Dim dvPurchasing As DataView = dtPurchasing.DefaultView
            Call RMA_DataBindPur(dvPurchasing, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub dgvPurchasing_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvPurchasing.Sorting

        If Me.ViewState("_PurSortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_PurSortDirection") = "asc"
        Else
            If Me.ViewState("_PurSortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_PurSortDirection") = "desc"
            Else
                Me.ViewState("_PurSortDirection") = "asc"
            End If
        End If
        Me.ViewState("_PurSortExpression") = e.SortExpression

        If IsNothing(Session("_dtPurchasing")) = False Then
            Dim dtPurchasing As DataTable = Session("_dtPurchasing")
            Dim dvPurchasing As DataView = dtPurchasing.DefaultView
            Call RMA_DataBindPur(dvPurchasing, 0)
        End If
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click

        ViewState("_SKU") = ""
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("SN")
        Dim dr As DataRow = dt.NewRow()
        dr(0) = txtSKU.Text.Trim()
        dt.Rows.Add(dr)

        ArrangementImportData(dt)
        QueryData(0)
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
            Dim sSN As String = ""
            Dim cellCount As Integer = row.LastCellNum
            For i = 0 To 0
                Dim cell As HSSFCell = row.GetCell(i)
                If t = 0 Then
                    Dim column As DataColumn = New DataColumn("SN")
                    dt.Columns.Add(column)
                End If
                If cell Is Nothing Then
                    sSN = ""
                Else
                    sSN = cell.ToString()
                End If
            Next

            If sSN <> "" Then
                dt.DefaultView.RowFilter = "SN='" & sSN & "'"
                If dt.DefaultView.Count > 0 Then
                Else
                    Dim dr As DataRow = dt.NewRow()
                    dr(0) = sSN
                    dt.Rows.Add(dr)
                    t = t + 1
                End If
            End If
        End While


        Dim dataSet1 As DataSet = New DataSet()
        dataSet1.Tables.Add(dt)

        Return dataSet1

    End Function

    Protected Sub btnExportWarranty1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportWarranty1.Click
        If Session("_dtImport") Is Nothing Then
            Exit Sub
        End If

        Response.Redirect("/ashx/ExportWarrantyExcel.ashx")
    End Sub

    Protected Sub btnExportWarranty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportWarranty.Click
        If Not Session("_dtImport") Is Nothing Then

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""
            Dim hssfworkbook As New HSSFWorkbook
            Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
            Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat


            Try
                Dim dtImport As DataTable = Session("_dtImport")
                Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")
                Dim iCount As Integer = 0

                If dtImport.Rows.Count > 0 Then
                    Dim row As HSSFRow = sheet1.CreateRow(0)
                    row.Height = 20 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    ' 修正匯出EXCEL按鈕沒反映問題 by buck modify 20260204 begin
                    'row.CreateCell(iCount).SetCellValue("")
                    row.CreateCell(iCount).SetCellValue(" ")
                    ' 修正匯出EXCEL按鈕沒反映問題 by buck modify 20260204 end
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("SerialNo")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("CW Start")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("CW End")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("EW Start")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("EW End")
                    row.GetCell(iCount).CellStyle = style

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue("SW Start")
                    'row.GetCell(iCount).CellStyle = style

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue("SW End")
                    'row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("WAR_TYPE")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("WAR_PROGRAM_TYPE")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("WAR_ITEM_TYPE")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("WAR_PRICE_VER")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Model")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("SKU")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Customer")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("ShipNo")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("DeliverDate")
                    row.GetCell(iCount).CellStyle = style
                End If


                For i = 0 To dtImport.Rows.Count - 1
                    Dim row As HSSFRow = sheet1.CreateRow(i + 1)
                    row.Height = 15 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue((i + 1).ToString())
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("SerialNo").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("CWStart").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("CWEnd").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("EWStart").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("EWEnd").ToString().Trim())

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("SWStart").ToString().Trim())

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("SWEnd").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("WAR_TYPE").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("WAR_PROGRAM_TYPE").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("WAR_ITEM_TYPE").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("WAR_PRICE_VER").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("Model").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("SKU").ToString().Trim())

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("Description").ToString().Trim())

                    'iCount = iCount + 1
                    ' row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("OrderNo").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("Customer").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("ShipNo").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("OrderDate").ToString().Trim())

                    ' iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dtImport.Rows(i)("DeliverDate").ToString().Trim())
                Next

                For i = 0 To 40
                    sheet1.AutoSizeColumn(i)
                    sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
                Next

                ' 修正匯出EXCEL按鈕沒反映問題 by buck modify 20260204 begin
                Dim filename As String = oCommon.GetRandomizeNum & ".xls"

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
                Using ms As New MemoryStream()
                    hssfworkbook.Write(ms)
                    Response.BinaryWrite(ms.ToArray())
                End Using
                Response.Flush()

                'Dim filename As String = oCommon.GetRandomizeNum & ".xls"

                'Response.ContentType = "application/vnd.ms-excel"
                'Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
                'Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
                'Response.End()
                ' 修正匯出EXCEL按鈕沒反映問題 by buck modify 20260204 end

                blnFlag = True
            Catch ex As Exception
                blnFlag = False
                sMessage = ex.Message

            Finally
            End Try

        End If

    End Sub

    Protected Sub btnPurWarranty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPurWarranty.Click

        If Not Session("_dtPurchasing") Is Nothing Then

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""
            Dim hssfworkbook As New HSSFWorkbook
            Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
            Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat


            Try
                Dim dtPurchasing As DataTable = Session("_dtPurchasing")
                Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")
                Dim iCount As Integer = 0

                If dtPurchasing.Rows.Count > 0 Then
                    Dim row As HSSFRow = sheet1.CreateRow(0)
                    row.Height = 20 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue("")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("SerialNo")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("PurchaseDate")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("WarrantyCode")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("StartDate")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("EndDate")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Model")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("SKU")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Description")
                    row.GetCell(iCount).CellStyle = style
                End If


                For i = 0 To dtPurchasing.Rows.Count - 1
                    Dim row As HSSFRow = sheet1.CreateRow(i + 1)
                    row.Height = 15 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue((i + 1).ToString())
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("SerialNo").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("PurchaseDate").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("WarrantyCode").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("StartDate").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("EndDate").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("Model").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("SKU").ToString().Trim())

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dtPurchasing.Rows(i)("Description").ToString().Trim())
                Next

                For i = 0 To 40
                    sheet1.AutoSizeColumn(i)
                    sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
                Next


                Dim filename As String = oCommon.GetRandomizeNum & ".xls"

                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
                Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
                Response.End()

                blnFlag = True
            Catch ex As Exception
                blnFlag = False
                sMessage = ex.Message

            Finally
            End Try

        End If

    End Sub

    Private Function WriteToStream(ByVal hssfworkbook As HSSFWorkbook) As MemoryStream
        Dim file As MemoryStream = New MemoryStream
        hssfworkbook.Write(file)
        Return file
    End Function

End Class


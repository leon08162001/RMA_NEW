Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel
Imports NPOI.HSSF.Util

Partial Class Report10_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = 1
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.ui_jascript.Text = ""
        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_CompanyName") = ""
            Me.ViewState("_Repair") = "-1"
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtReport") = Nothing

            Call setControls()
            Call QueryData(0)
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim TagText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        '身分:1.客戶, 2.公司(維修中心)
        If Session("_Identity").ToString().Trim() = "2" Then
            oCommon.getRepairCenteryByDropDownList(Session("_RepairCenter").ToString().Trim(), Me.UI_cboRepairCenter, TagText, True)
        Else
            oCommon.getRepairCenteryByDropDownList(Session("_RepairID").ToString().Trim(), Me.UI_cboRepairCenter, TagText, False)
        End If


        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))
        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag)
        Me.UI_lblCompanyName.Text = _oLanguage.getText("Report", "100", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Report", "102", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingDate.Text = _oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdExport.Text = _oLanguage.getText("Common", "079", ctlLanguage.eumType.Command)
        Me.UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)



        '身分:1.客戶, 2.公司(維修中心)
        Me.UI_lblCompanyName.Visible = False
        Me.UI_lblCompanyName_Tag.Visible = False
        Me.UI_txtCompanyName.Visible = False
        If Session("_Identity").ToString().Trim() = "2" Then
            Me.UI_lblCompanyName.Visible = True
            Me.UI_lblCompanyName_Tag.Visible = True
            Me.UI_txtCompanyName.Visible = True
        End If


        Me.UI_dvReport.Columns(2).HeaderText = _oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(3).HeaderText = _oLanguage.getText("Report", "107", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(4).HeaderText = _oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(5).HeaderText = _oLanguage.getText("Report", "186", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(6).HeaderText = _oLanguage.getText("Report", "187", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(7).HeaderText = _oLanguage.getText("Report", "188", ctlLanguage.eumType.Tag)
        Me.UI_dvReport.Columns(8).HeaderText = _oLanguage.getText("Report", "189", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_BillDataDataTable

        Dim RMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim ModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim CompanyName As String = Me.ViewState("_CompanyName").ToString().Trim()

        Dim Repair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        '身分:1.客戶, 2.公司(維修中心)
        If Session("_Identity").ToString().Trim() = "2" Then
            dtReport = ctlReport.QueryRMABill(Session("_CustomerID").ToString().Trim(), CompanyName, RMANo, ModelNo, Repair, fdate, edate, Session("_RepairCenter"))
        Else
            dtReport = ctlReport.QueryRMABill(Session("_CustomerID").ToString().Trim(), CompanyName, RMANo, ModelNo, Repair, fdate, edate, Session("_RepairCenter"))
        End If

        'Me.UI_cmdExport.Visible = False
        'If dtReport.Rows.Count > 0 Then
        '    Me.UI_cmdExport.Visible = True
        'End If

        Call ArrangementData(dtReport)
        Session("_dtReport") = dtReport

        Dim dvReport As DataView = dtReport.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvReport.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Report_DataBind(dtReport.DefaultView, iPageIndex)

    End Sub

    Private Sub Report_DataBind(ByVal dvReport As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvReport.PageSize = _PageSize
        Me.UI_dvReport.PageIndex = iPageIndex
        Me.UI_dvReport.DataSource = dvReport
        Me.UI_dvReport.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtReport As ReportDTO.Rpt_BillDataDataTable)
        Dim i As Integer = 0

        If dtReport.Columns("TotalCount") Is Nothing Then
            dtReport.Columns.Add("BillDate")
            dtReport.Columns.Add("TotalCount")
        End If

        For i = 0 To dtReport.Rows.Count - 1
            If dtReport.Rows(i)("bill_date").ToString().Trim() <> "" Then
                dtReport.Rows(i)("BillDate") = Convert.ToDateTime(dtReport.Rows(i)("bill_date")).ToString("yyyy/MM/dd HH:mm:ss")
            End If
            dtReport.Rows(i)("TotalCount") = dtReport.Rows(i)("shippingCount").ToString().Trim() & " / " & dtReport.Rows(i)("ReceivedCount").ToString().Trim()
        Next

    End Sub

    Protected Sub UI_dvReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvReport.RowCommand
        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvReport.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_lblRMA As Label = row.FindControl("UI_lblRMA")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.ToString().Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_lblRMA.Text.ToString().Trim()
        End If

    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()
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

    Protected Sub UI_dvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvReport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            Dim dtReport As ReportDTO.Rpt_BillDataDataTable = Session("_dtReport")
            Dim dvReport As DataView = dtReport.DefaultView
            Call Report_DataBind(dvReport, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvReport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvReport.Sorting

        If Me.ViewState("_SortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_SortDirection") = "asc"
        Else
            If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_SortDirection") = "desc"
            Else
                Me.ViewState("_SortDirection") = "asc"
            End If
        End If
        Me.ViewState("_SortExpression") = e.SortExpression

        If IsNothing(Session("_dtReport")) = False Then
            Dim dtReport As DataTable = Session("_dtReport")
            Dim dvReport As DataView = dtReport.DefaultView
            dvReport.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Report_DataBind(dvReport, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvReport.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvReport.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvReport.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvReport.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvReport.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvReport.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_CompanyName") = Me.UI_txtCompanyName.Text.Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()

        Me.ViewState("_fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("_fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("_edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' 匯出並寫入 對帳單檔
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdExport.Click
        Dim i As Integer = 0
        Dim sRMANo As String = ""

        For i = 0 To Me.UI_dvReport.Rows.Count - 1
            Dim UI_Check As CheckBox = Me.UI_dvReport.Rows(i).FindControl("UI_Check")
            Dim UI_RMAID As Label = Me.UI_dvReport.Rows(i).FindControl("UI_RMAID")
            Dim UI_lblRMA As Label = Me.UI_dvReport.Rows(i).FindControl("UI_lblRMA")

            If UI_Check.Checked = True Then
                If sRMANo.Trim <> "" Then
                    sRMANo = sRMANo & ","
                End If
                sRMANo = sRMANo & UI_lblRMA.Text.Trim()
            End If
        Next


        If sRMANo.Trim <> "" Then
            Dim ctlRMA As New ctlRMA
            Dim ctlReport As New ctlReport

            'save RMA_BILLDATA
            Dim arrRMANo() As String = sRMANo.Split(",")
            Call ctlRMA.Save_BillData(arrRMANo, Session("_UserID").ToString().Trim(), Session("_UserName").ToString().Trim())


            'Dim dtReportGroup As New ReportDTO.vwRpt_ShippingDetailDataTable
            'dtReportGroup.Columns.Add("isWarrantyText")
            'dtReportGroup.Columns.Add("WarrantyDateText")

            Dim dtReport As New ReportDTO.vwRpt_ShippingDetailDataTable

            Dim RMANo As String = sRMANo
            Dim ModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
            Dim CompanyName As String = Me.ViewState("_CompanyName").ToString().Trim()

            Dim Repair As String = Me.ViewState("_Repair").ToString().Trim()
            Dim fdate As String = Me.ViewState("_fdate")
            Dim edate As String = Me.ViewState("_edate")

            '身分:1.客戶, 2.公司(維修中心)
            If Session("_Identity").ToString().Trim() = "2" Then
                dtReport = ctlReport.QueryRMAShippingByBill(Session("_CustomerID").ToString().Trim(), CompanyName, RMANo, ModelNo, Repair, fdate, edate, Session("_RepairCenter"))
            Else
                dtReport = ctlReport.QueryRMAShippingByBill(Session("_CustomerID").ToString().Trim(), CompanyName, RMANo, ModelNo, Repair, fdate, edate, Session("_RepairCenter"))
            End If

            dtReport.Columns.Add("isWarrantyText")
            dtReport.Columns.Add("WarrantyDateText")

            For i = 0 To dtReport.Rows.Count - 1
                Dim WarrantyText As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)

                dtReport.Rows(i)("isWarrantyText") = WarrantyText
                If dtReport.Rows(i)("RMAD_ISWARRANTY").ToString().Trim() <> "" Then
                    Select Case dtReport.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                        Case "0"
                            dtReport.Rows(i)("isWarrantyText") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                        Case "1"
                            dtReport.Rows(i)("isWarrantyText") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                    End Select
                End If

                dtReport.Rows(i)("WarrantyDateText") = WarrantyText
                If dtReport.Rows(i)("RMAD_WARRANTY").ToString().Trim() <> "" Then
                    dtReport.Rows(i)("WarrantyDateText") = Convert.ToDateTime(dtReport.Rows(i)("RMAD_WARRANTY").ToString().Trim()).ToShortDateString()
                End If
            Next

            QueryData(Me.UI_dvReport.PageIndex)

            Call RunExport(dtReport)
        End If


    End Sub

    Private Sub RunExport(ByVal dtReport As DataTable)
        Dim i As Integer = 0

        '=============================================================================================================================================================================================v
        '整理匯出的資料
        '=============================================================================================================================================================================================v
        Dim dtExport As New DataTable
        dtExport.Columns.Add("RMA_NO")
        dtExport.Columns.Add("EFYes_QTY")
        dtExport.Columns.Add("EKYes_QTY")
        dtExport.Columns.Add("subTotalAmount_1")

        dtExport.Columns.Add("EFNo_QTY")
        dtExport.Columns.Add("EKNo_QTY")
        dtExport.Columns.Add("subTotalAmount_2")

        dtExport.Columns.Add("TotalQTY")
        dtExport.Columns.Add("TotalAmount")


        Dim dvExport As DataView = dtExport.DefaultView
        For i = 0 To dtReport.Rows.Count - 1
            Dim Flag_Add As Boolean = False
            Dim dr As DataRow
            Dim RMA_NO As String = dtReport.Rows(i)("RMA_NO").ToString().Trim()
            Dim RMAD_SERIALNO As String = dtReport.Rows(i)("RMAD_SERIALNO").ToString().Trim()
            Dim RMAD_ISWARRANTY As String = dtReport.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()

            dvExport.RowFilter = "RMA_NO='" & RMA_NO & "'"
            If dvExport.Count = 0 Then
                Flag_Add = True
                dr = dtExport.NewRow
                dr("RMA_NO") = RMA_NO
                dr("EFYes_QTY") = 0
                dr("EKYes_QTY") = 0
                dr("subTotalAmount_1") = 0

                dr("EFNo_QTY") = 0
                dr("EKNo_QTY") = 0
                dr("subTotalAmount_2") = 0

                dr("TotalQTY") = 0
                dr("TotalAmount") = 0

                If RMAD_ISWARRANTY = "1" Then
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EF".ToLower() Then
                        dr("EFYes_QTY") = "1"
                    End If
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EK".ToLower() Then
                        dr("EKYes_QTY") = "1"
                    End If

                Else
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EF".ToLower() Then
                        dr("EFNo_QTY") = "1"
                    End If
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EK".ToLower() Then
                        dr("EKNo_QTY") = "1"
                    End If
                End If


            Else
                dr = dvExport(0).Row

                If RMAD_ISWARRANTY = "1" Then
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EF".ToLower() Then
                        dr("EFYes_QTY") = Convert.ToInt32(dr("EFYes_QTY")) + 1
                    End If
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EK".ToLower() Then
                        dr("EKYes_QTY") = Convert.ToInt32(dr("EKYes_QTY")) + 1
                    End If

                Else
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EF".ToLower() Then
                        dr("EFNo_QTY") = Convert.ToInt32(dr("EFNo_QTY")) + 1
                    End If
                    If RMAD_SERIALNO.Substring(0, 2).ToLower() = "EK".ToLower() Then
                        dr("EKNo_QTY") = Convert.ToInt32(dr("EKNo_QTY")) + 1
                    End If
                End If
            End If

            dr("subTotalAmount_1") = (Convert.ToInt32(dr("EFYes_QTY")) * 25) + (Convert.ToInt32(dr("EKYes_QTY")) * 15)
            dr("subTotalAmount_2") = (Convert.ToInt32(dr("EFNo_QTY")) * 25) + (Convert.ToInt32(dr("EKNo_QTY")) * 15)
            dr("TotalQTY") = Convert.ToInt32(dr("EFYes_QTY")) + Convert.ToInt32(dr("EKYes_QTY")) + Convert.ToInt32(dr("EFNo_QTY")) + Convert.ToInt32(dr("EKNo_QTY"))
            dr("TotalAmount") = Convert.ToInt32(dr("subTotalAmount_1")) + Convert.ToInt32(dr("subTotalAmount_2"))

            If Flag_Add = True Then
                dtExport.Rows.Add(dr)
            End If
        Next
        dvExport.RowFilter = ""

        Call Export_Mothod2(dtExport)
        'Call Export_Mothod1(dtReport)

    End Sub

    ''' <summary>
    ''' 匯出
    ''' </summary>
    ''' <param name="dtExport"></param>
    ''' <remarks></remarks>
    Private Sub Export_Mothod2(ByVal dtExport As DataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim hssfworkbook As New HSSFWorkbook
        Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
        Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat

        Try
            Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")

            '==========================================================================================================================================================
            '設定個欄位的寬度
            '==========================================================================================================================================================
            Dim iCount As Integer = 0

            Dim row1 As HSSFRow = sheet1.CreateRow(0)
            Dim row2 As HSSFRow = sheet1.CreateRow(1)
            row1.Height = 20 * 20

            style = hssfworkbook.CreateCellStyle()
            style.Alignment = CellHorizontalAlignment.CENTER
            style.VerticalAlignment = CellVerticalAlignment.CENTER

            iCount = 0
            row1.CreateCell(iCount).SetCellValue("")
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, 0, 1, 0))

            'RMA NO
            iCount = iCount + 1
            row1.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag))
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, 1, 1, 1))

            'Warranty Yes
            iCount = iCount + 1
            row1.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "011", ctlLanguage.eumType.Tag))
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, iCount, 0, iCount + 2))

            'EF
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "013", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style

            'EF
            iCount = iCount + 1
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "014", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style

            'subTotal Amount 1
            iCount = iCount + 1
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "015", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style


            'Warranty Yes
            iCount = iCount + 1
            row1.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "012", ctlLanguage.eumType.Tag))
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, iCount, 0, iCount + 2))

            'EF
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "013", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style

            'EF
            iCount = iCount + 1
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "014", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style

            'subTotal Amount 1
            iCount = iCount + 1
            row2.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "015", ctlLanguage.eumType.Tag))
            row2.GetCell(iCount).CellStyle = style


            iCount = iCount + 1
            row1.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "016", ctlLanguage.eumType.Tag))
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, iCount, 1, iCount))

            iCount = iCount + 1
            row1.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "017", ctlLanguage.eumType.Tag))
            row1.GetCell(iCount).CellStyle = style
            sheet1.AddMergedRegion(New Region(0, iCount, 1, iCount))


            Dim iRow As Integer = 1
            For i = 0 To dtExport.Rows.Count - 1
                iRow = iRow + 1
                Dim dr As DataRow = dtExport.Rows(i)
                row1 = sheet1.CreateRow(iRow)
                'row.Height = 15 * 20

                style = hssfworkbook.CreateCellStyle()
                style.Alignment = CellHorizontalAlignment.CENTER
                style.VerticalAlignment = CellVerticalAlignment.CENTER
                style.WrapText = True

                iCount = 0
                row1.CreateCell(iCount).SetCellValue((i + 1).ToString())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMA_NO").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("EFYes_QTY").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("EKYes_QTY").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("subTotalAmount_1").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style


                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("EFNo_QTY").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("EKNo_QTY").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("subTotalAmount_2").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style


                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("TotalQTY").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row1.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("TotalAmount").ToString().Trim())
                row1.GetCell(iCount).CellStyle = style
            Next


            For i = 0 To 9

                Select Case i
                    Case 0
                        sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (2 * 256))

                    Case 1
                        sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (15 * 256))

                    Case Else
                        sheet1.AutoSizeColumn(i)
                        sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
                End Select
            Next


            Dim filename As String = oCommon.GetRandomizeNum & ".xls"
            Dim file As FileStream = New FileStream(Server.MapPath("object/Report/" & filename), FileMode.Create)
            hssfworkbook.Write(file)
            file.Close()

            'Response.ContentType = "application/vnd.ms-excel"
            'Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
            'Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
            'Response.End()

            Me.ui_frame.Visible = False
            Dim sScript As String = ""
            sScript = sScript & "<script language=""javascript"">" & vbCrLf
            sScript = sScript & "window.open('" & "object/Report/" & filename & "','winRpt','');" & vbCrLf
            'sScript = sScript & "document.getElementById('" & Me.ui_frame.ClientID & "').src='object/Report/" & filename & "';" & vbCrLf
            'sScript = sScript & "window.location.href='DownLoadFile.aspx?filename=" & filename & "';" & vbCrLf
            sScript = sScript & "</script>" & vbCrLf
            'Response.Write(sScript)

            Me.ui_jascript.Text = sScript


            blnFlag = True

        Catch ex As Exception
            blnFlag = False
            sMessage = ex.Message

        Finally
            'If blnFlag = False Then
            '    sMessage = sMessage & "匯出學員資料有問題, 請洽系統人員...!!"
            '    Me.ucMessage.showMessageByFailed(sMessage)
            'End If
        End Try

    End Sub

    ''' <summary>
    ''' 匯出
    ''' </summary>
    ''' <param name="dtExport"></param>
    ''' <remarks></remarks>
    Private Sub Export_Mothod1(ByVal dtExport As DataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim hssfworkbook As New HSSFWorkbook
        Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
        Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat

        Try
            Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")

            '==========================================================================================================================================================
            '設定個欄位的寬度
            '==========================================================================================================================================================
            Dim iCount As Integer = 0

            Dim row As HSSFRow = sheet1.CreateRow(0)
            row.Height = 20 * 20

            style = hssfworkbook.CreateCellStyle()
            style.Alignment = CellHorizontalAlignment.CENTER
            style.VerticalAlignment = CellVerticalAlignment.CENTER

            iCount = 0
            row.CreateCell(iCount).SetCellValue("")
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "186", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "150", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "103", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "112", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style

            iCount = iCount + 1
            row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "190", ctlLanguage.eumType.Tag))
            row.GetCell(iCount).CellStyle = style



            For i = 0 To dtExport.Rows.Count - 1
                Dim dr As DataRow = dtExport.Rows(i)
                row = sheet1.CreateRow(i + 1)
                'row.Height = 15 * 20

                style = hssfworkbook.CreateCellStyle()
                style.Alignment = CellHorizontalAlignment.CENTER
                style.VerticalAlignment = CellVerticalAlignment.CENTER
                style.WrapText = True

                iCount = 0
                row.CreateCell(iCount).SetCellValue((i + 1).ToString())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMA_NO").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("cu_name").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMAD_SERIALNO").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMAD_MODELNO").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("isWarrantyText").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("WarrantyDateText").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                'row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("ShippedDate").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("ShippedDate").ToString().Trim()).ToString("yyyy/MM/dd"))
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMARED_NPARTNO").ToString().Trim())
                row.GetCell(iCount).CellStyle = style

                iCount = iCount + 1
                row.CreateCell(iCount).SetCellValue(dtExport.Rows(i)("RMAR_REPAIRADNAME").ToString().Trim())
                row.GetCell(iCount).CellStyle = style
            Next



            For i = 0 To 9
                sheet1.AutoSizeColumn(i)
                sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
            Next


            Dim filename As String = oCommon.GetRandomizeNum & ".xls"
            Dim file As FileStream = New FileStream(Server.MapPath("object/Report/" & filename), FileMode.Create)
            hssfworkbook.Write(file)
            file.Close()


            'Response.ContentType = "application/vnd.ms-excel"
            'Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
            'Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
            'Response.End()

            Dim sScript As String = ""
            sScript = sScript & "<script language=""javascript"">" & vbCrLf
            sScript = sScript & "window.open('" & "object/Report/" & filename & "','','');" & vbCrLf
            sScript = sScript & "</script>" & vbCrLf
            Response.Write(sScript)


            blnFlag = True

        Catch ex As Exception
            blnFlag = False
            sMessage = ex.Message

        Finally
            'If blnFlag = False Then
            '    sMessage = sMessage & "匯出學員資料有問題, 請洽系統人員...!!"
            '    Me.ucMessage.showMessageByFailed(sMessage)
            'End If
        End Try

    End Sub

    Private Function WriteToStream(ByVal hssfworkbook As HSSFWorkbook) As MemoryStream
        'Write the stream data of workbook to the root directory
        Dim file As MemoryStream = New MemoryStream
        hssfworkbook.Write(file)
        Return file
    End Function

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    ''' <summary>
    ''' 刪除 對帳單檔
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdDel.Click
        Dim i As Integer = 0
        Dim sRMANo As String = ""


        For i = 0 To Me.UI_dvReport.Rows.Count - 1
            Dim UI_Check As CheckBox = Me.UI_dvReport.Rows(i).FindControl("UI_Check")
            Dim UI_RMAID As Label = Me.UI_dvReport.Rows(i).FindControl("UI_RMAID")
            Dim UI_lblRMA As Label = Me.UI_dvReport.Rows(i).FindControl("UI_lblRMA")

            If UI_Check.Checked = True Then
                If sRMANo.Trim <> "" Then
                    sRMANo = sRMANo & ","
                End If
                sRMANo = sRMANo & UI_lblRMA.Text.Trim()
            End If
        Next



        If sRMANo.Trim <> "" Then
            Dim ctlRMA As New ctlRMA

            'Clear RMA_BILLDATA
            Dim arrRMANo() As String = sRMANo.Split(",")
            Call ctlRMA.Clear_BillData(arrRMANo, Session("_UserID").ToString().Trim(), Session("_UserName").ToString().Trim())
            QueryData(Me.UI_dvReport.PageIndex)
        End If



    End Sub

End Class

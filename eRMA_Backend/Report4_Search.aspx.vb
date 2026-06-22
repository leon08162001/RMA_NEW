Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel

Partial Class Report4_Search
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    'Dim _QueryDate_Month As String="-3"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_CompanyName") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_Warranty") = "-1"
            Me.ViewState("_Repair") = "-1"
            Me.ViewState("_Status") = "-1"

            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Me.ViewState("_SDfdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_SDedate") = Date.Now.ToShortDateString()



            Session("_dtReport") = Nothing

            Call setControls()
            'Call QueryData(0)
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


        oCommon.getWarranty(Me.UI_cboWarranty)
        oCommon.getItemStatus(Me.UI_cboStatus)


        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))

        Dim SDfdate As DateTime = Convert.ToDateTime(Me.ViewState("_SDfdate"))
        Dim SDedate As DateTime = Convert.ToDateTime(Me.ViewState("_SDedate"))

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())

        Call oCommon.getYear_DropDownList(Me.UI_cboSDBYear, SDfdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboSDEYear, SDedate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboSDBMonth, SDfdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboSDEMonth, SDedate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboSDBDay, SDfdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboSDEDay, SDedate.Day.ToString())

        UI_cboBYear.SelectedValue = -1
        UI_cboEYear.SelectedValue = -1
        UI_cboBMonth.SelectedValue = -1
        UI_cboEMonth.SelectedValue = -1
        UI_cboBDay.SelectedValue = -1
        UI_cboEDay.SelectedValue = -1
        UI_cboSDBYear.SelectedValue = -1
        UI_cboSDEYear.SelectedValue = -1
        UI_cboSDBMonth.SelectedValue = -1
        UI_cboSDEMonth.SelectedValue = -1
        UI_cboSDBDay.SelectedValue = -1
        UI_cboSDEDay.SelectedValue = -1

        Me.ViewState("_fdate") = ""
        Me.ViewState("_edate") = ""

        Me.ViewState("_SDfdate") = ""
        Me.ViewState("_SDedate") = ""

        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_lblCompanyName.Text = _oLanguage.getText("Report", "100", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Report", "102", ctlLanguage.eumType.Tag)
        Me.UI_lblWarranty.Text = _oLanguage.getText("Report", "103", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Report", "104", ctlLanguage.eumType.Tag)
        Me.UI_lblDurationDate.Text = _oLanguage.getText("Report", "105", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingDate.Text = _oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdExport.Text = _oLanguage.getText("Common", "076", ctlLanguage.eumType.Command)

        Me.UI_lblRmaNo.Text = _oLanguage.getText("Report", "202", ctlLanguage.eumType.Tag)

        '身分:1.客戶, 2.公司(維修中心)
        Me.UI_lblCompanyName.Visible = False
        Me.UI_lblCompanyName_Tag.Visible = False
        Me.UI_txtCompanyName.Visible = False
        Me.UI_lblRmaNo.Visible = False
        Me.UI_lblRmaNo_Tag.Visible = False
        Me.UI_txtRmaNo.Visible = False
        If Session("_Identity").ToString().Trim() = "2" Then
            Me.UI_lblCompanyName.Visible = True
            Me.UI_lblCompanyName_Tag.Visible = True
            Me.UI_txtCompanyName.Visible = True
            Me.UI_lblRmaNo.Visible = True
            Me.UI_lblRmaNo_Tag.Visible = True
            Me.UI_txtRmaNo.Visible = True
        End If


    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.VWRPT_RMADETAILDataTable

        Dim CompanyName As String = Me.ViewState("_CompanyName").ToString().Trim()
        Dim ModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim sRmaNo As String = Me.ViewState("_RmaNo").ToString().Trim()

        '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
        Dim Warranty As String = Me.ViewState("_Warranty").ToString().Trim()
        Dim Repair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")
        Dim SDfdate As String = Me.ViewState("_SDfdate")
        Dim SDedate As String = Me.ViewState("_SDedate")


        '身分:1.客戶, 2.公司(維修中心)
        If Session("_Identity").ToString().Trim() = "2" Then
            dtReport = ctlReport.QueryRMADetail(sRmaNo, Session("_CustomerID").ToString().Trim(), CompanyName, ModelNo, Warranty, Repair, Status, fdate, edate, SDfdate, SDedate, Session("_RepairCenter"), Session("_LanguageID"))
        Else
            dtReport = ctlReport.QueryRMADetail(sRmaNo, Session("_CustomerID").ToString().Trim(), CompanyName, ModelNo, Warranty, Repair, Status, fdate, edate, SDfdate, SDedate, Session("_RepairID"), Session("_LanguageID"))
        End If


        Me.UI_cmdExport.Visible = False
        If dtReport.Rows.Count > 0 Then
            Me.UI_cmdExport.Visible = True
        End If

        Call ArrangementData(dtReport)
        Session("_dtReport") = dtReport
        Call Request_DataBind(iPageIndex)

    End Sub

    Private Sub Request_DataBind(ByVal iPageIndex As Integer)
        Me.UI_dvReport.PageSize = _PageSize
        Me.UI_dvReport.PageIndex = iPageIndex
        Me.UI_dvReport.DataSource = Session("_dtReport")
        Me.UI_dvReport.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtReport As ReportDTO.VWRPT_RMADETAILDataTable)
        Dim i As Integer = 0

        If dtReport.Columns("SeqID") Is Nothing Then
            dtReport.Columns.Add("SeqID")
            dtReport.Columns.Add("RequestDate")
            dtReport.Columns.Add("RequestYear")
            dtReport.Columns.Add("RequestMonth")
            dtReport.Columns.Add("ShippingYear")
            dtReport.Columns.Add("ShippingMonth")
            dtReport.Columns.Add("Warranty")
            dtReport.Columns.Add("sIsWarranty")
            dtReport.Columns.Add("Status")
            dtReport.Columns.Add("IMPROPERUSAGE_Text")

            dtReport.Columns.Add("sRMARQ_ASSIGLABORCOST")
            dtReport.Columns.Add("sRMARQ_ASSIGMATERIALCOST")

            dtReport.Columns.Add("sRMASQ_LABORCOST")
            dtReport.Columns.Add("sRMASQ_MATERIALCOST")
            dtReport.Columns.Add("sRMASQ_QUOTE")

            dtReport.Columns.Add("sRMAR_LABORHOUR")
            dtReport.Columns.Add("sRMAR_LABORPRICE")
            dtReport.Columns.Add("sRMAR_MATERIALCOST")
            dtReport.Columns.Add("sRMAR_QUOTE")

            dtReport.Columns.Add("sRMARSD_LABORCOST")
            dtReport.Columns.Add("sRMARSD_MATERIALCOST")
            dtReport.Columns.Add("sRMARSD_QUOTE")
            dtReport.Columns.Add("sRMAD_ISCW")
            'dtReport.Columns.Add("EXPORT_SHIPPING_DATE")

            dtReport.Columns.Add("sRMACQ_DISCOUNT")
            dtReport.Columns.Add("sRMACQSN_DISCOUNTAMOUNT")
        End If


        For i = 0 To dtReport.Rows.Count - 1
            Dim dr As ReportDTO.VWRPT_RMADETAILRow = dtReport.Rows(i)
            dtReport.Rows(i)("SeqID") = i + 1

            '全保否
            If dr.IsRMAD_ISCWNull = False Then
                If dtReport.Rows(i)("RMAD_ISCW").ToString().Trim() = "1" Then
                    dtReport.Rows(i)("sRMAD_ISCW") = "Y"
                Else
                    dtReport.Rows(i)("sRMAD_ISCW") = ""
                End If
            Else
                dtReport.Rows(i)("sRMAD_ISCW") = ""
            End If

            '保固日期
            If dr.IsRMAD_WARRANTYNull = False Then
                dtReport.Rows(i)("Warranty") = Convert.ToDateTime(dtReport.Rows(i)("RMAD_WARRANTY").ToString()).ToShortDateString()
            Else
                dtReport.Rows(i)("Warranty") = _oLanguage.getText("Report", "052", ctlLanguage.eumType.Tag)
                'Select Case dtReport.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                '    Case "0"
                '        dtReport.Rows(i)("Warranty") = _oLanguage.getText("Report", "051", ctlLanguage.eumType.Tag)
                '    Case "1"
                '        dtReport.Rows(i)("Warranty") = _oLanguage.getText("Report", "050", ctlLanguage.eumType.Tag)
                'End Select
            End If

            If dr.IsRMAD_ISWARRANTYNull = False Then
                dtReport.Rows(i)("sIsWarranty") = _oLanguage.getText("Report", "051", ctlLanguage.eumType.Tag)
                If dtReport.Rows(i)("RMAD_ISWARRANTY").ToString().Trim() = "1" Then
                    dtReport.Rows(i)("sIsWarranty") = _oLanguage.getText("Report", "050", ctlLanguage.eumType.Tag)
                End If
            End If


            If dtReport.Rows(i)("RMARQ_IMPROPERUSAGE").ToString() = "0" Then
                '非正常使用: 0.No, 1.Yes
                dtReport.Rows(i)("IMPROPERUSAGE_Text") = _oLanguage.getText("Report", "051", ctlLanguage.eumType.Tag)
            Else
                ' If dr.RMARQ_IMPROPERUSAGE = 1 Then
                '  dtReport.Rows(i)("IMPROPERUSAGE_Text") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                dtReport.Rows(i)("IMPROPERUSAGE_Text") = _oLanguage.getText("Report", "050", ctlLanguage.eumType.Tag)
                'End If
            End If


            '申請日期
            dtReport.Rows(i)("RequestDate") = Convert.ToDateTime(dtReport.Rows(i)("RMAD_CSTMP").ToString()).ToShortDateString()
            dtReport.Rows(i)("RequestYear") = Convert.ToDateTime(dtReport.Rows(i)("RMAD_CSTMP").ToString()).Year.ToString()
            dtReport.Rows(i)("RequestMonth") = Convert.ToDateTime(dtReport.Rows(i)("RMAD_CSTMP").ToString()).Year.ToString() + "/" + Convert.ToDateTime(dtReport.Rows(i)("RMAD_CSTMP").ToString()).Month.ToString()
            '出貨日期
            If IsDBNull(dtReport.Rows(i)("ShippingDate")) = False Then
                dtReport.Rows(i)("ShippingYear") = Convert.ToDateTime(dtReport.Rows(i)("ShippingDate").ToString()).Year.ToString()
                dtReport.Rows(i)("ShippingMonth") = Convert.ToDateTime(dtReport.Rows(i)("ShippingDate").ToString()).Year.ToString() + "/" + Convert.ToDateTime(dtReport.Rows(i)("ShippingDate").ToString()).Month.ToString()
            End If
            If dtReport.Rows(i)("EXPORT_SHIPPING_DATE").ToString() <> "" Then
                dtReport.Rows(i)("EXPORT_SHIPPING_DATE") = Convert.ToDateTime(dtReport.Rows(i)("EXPORT_SHIPPING_DATE").ToString()).ToShortDateString()
            End If
            dtReport.Rows(i)("Status") = oCommon.ConvertToItemStatusText(dtReport.Rows(i)("RMAD_STATUS").ToString().Trim(), dtReport.Rows(i)("RMAD_ID").ToString().Trim())

            If dtReport.Rows(i)("RMARQ_ASSIGLABORCOST").ToString().Trim <> "" Then
                '                dtReport.Rows(i)("sRMARQ_ASSIGLABORCOST") = dtReport.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARQ_ASSIGLABORCOST")).ToString("N")
                dtReport.Rows(i)("sRMARQ_ASSIGLABORCOST") = Convert.ToDouble(dtReport.Rows(i)("RMARQ_ASSIGLABORCOST")).ToString("N")
            End If
            If dtReport.Rows(i)("RMARQ_ASSIGMATERIALCOST").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMARQ_ASSIGMATERIALCOST") = dtReport.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARQ_ASSIGMATERIALCOST")).ToString("N")
                dtReport.Rows(i)("sRMARQ_ASSIGMATERIALCOST") = Convert.ToDouble(dtReport.Rows(i)("RMARQ_ASSIGMATERIALCOST")).ToString("N")
            End If

            If dtReport.Rows(i)("RMASQ_LABORCOST").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMASQ_LABORCOST") = dtReport.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMASQ_LABORCOST")).ToString("N")
                dtReport.Rows(i)("sRMASQ_LABORCOST") = Convert.ToDouble(dtReport.Rows(i)("RMASQ_LABORCOST")).ToString("N")
            End If
            If dtReport.Rows(i)("RMASQ_MATERIALCOST").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMASQ_MATERIALCOST") = dtReport.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMASQ_MATERIALCOST")).ToString("N")
                dtReport.Rows(i)("sRMASQ_MATERIALCOST") = Convert.ToDouble(dtReport.Rows(i)("RMASQ_MATERIALCOST")).ToString("N")
            End If
            If dtReport.Rows(i)("RMASQ_QUOTE").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMASQ_QUOTE") = dtReport.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMASQ_QUOTE")).ToString("N")
                dtReport.Rows(i)("sRMASQ_QUOTE") = Convert.ToDouble(dtReport.Rows(i)("RMASQ_QUOTE")).ToString("N")
            End If


            If dtReport.Rows(i)("RMAR_LABORHOUR").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMAR_LABORHOUR") = dtReport.Rows(i)("RMAR_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMAR_LABORHOUR")).ToString("N")
                dtReport.Rows(i)("sRMAR_LABORHOUR") = dtReport.Rows(i)("RMAR_LABORHOUR").ToString().Trim
            End If
            If dtReport.Rows(i)("RMAR_LABORPRICE").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMAR_LABORPRICE") = dtReport.Rows(i)("RMAR_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMAR_LABORPRICE")).ToString("N")
                dtReport.Rows(i)("sRMAR_LABORPRICE") = Convert.ToDouble(dtReport.Rows(i)("RMAR_LABORPRICE")).ToString("N")
            End If
            If dtReport.Rows(i)("RMAR_MATERIALCOST").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMAR_MATERIALCOST") = dtReport.Rows(i)("RMAR_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMAR_MATERIALCOST")).ToString("N")
                dtReport.Rows(i)("sRMAR_MATERIALCOST") = Convert.ToDouble(dtReport.Rows(i)("RMAR_MATERIALCOST")).ToString("N")
            End If
            If dtReport.Rows(i)("RMAR_QUOTE").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMAR_QUOTE") = dtReport.Rows(i)("RMAR_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMAR_QUOTE")).ToString("N")
                dtReport.Rows(i)("sRMAR_QUOTE") = Convert.ToDouble(dtReport.Rows(i)("RMAR_QUOTE")).ToString("N")
            End If


            If dtReport.Rows(i)("RMARSD_LABORCOST").ToString().Trim <> "" Then
                dtReport.Rows(i)("sRMARSD_LABORCOST") = Convert.ToDouble(dtReport.Rows(i)("RMARSD_LABORCOST")).ToString("N")
            End If

            If dtReport.Rows(i)("RMARSD_MATERIALCOST").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMARSD_MATERIALCOST") = dtReport.Rows(i)("RMARSD_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARSD_MATERIALCOST")).ToString("N")
                dtReport.Rows(i)("sRMARSD_MATERIALCOST") = Convert.ToDouble(dtReport.Rows(i)("RMARSD_MATERIALCOST")).ToString("N")
            End If

            If dtReport.Rows(i)("RMARSD_QUOTE").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMARSD_QUOTE") = dtReport.Rows(i)("RMARSD_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARSD_QUOTE")).ToString("N")
                dtReport.Rows(i)("sRMARSD_QUOTE") = Convert.ToDouble(dtReport.Rows(i)("RMARSD_QUOTE")).ToString("N")
            End If

            If dtReport.Rows(i)("RMACQ_DISCOUNT").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMARSD_QUOTE") = dtReport.Rows(i)("RMARSD_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARSD_QUOTE")).ToString("N")
                dtReport.Rows(i)("sRMACQ_DISCOUNT") = Convert.ToDouble(dtReport.Rows(i)("RMACQ_DISCOUNT")).ToString("N")
            End If

            If dtReport.Rows(i)("RMACQSN_DISCOUNTAMOUNT").ToString().Trim <> "" Then
                'dtReport.Rows(i)("sRMARSD_QUOTE") = dtReport.Rows(i)("RMARSD_CURRENCYCODE").ToString().Trim & " " & Convert.ToDouble(dtReport.Rows(i)("RMARSD_QUOTE")).ToString("N")
                dtReport.Rows(i)("sRMACQSN_DISCOUNTAMOUNT") = Convert.ToDouble(dtReport.Rows(i)("RMACQSN_DISCOUNTAMOUNT")).ToString("N")
            End If

        Next

    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "200", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "201", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "107", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "196", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "197", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Report", "108", ctlLanguage.eumType.Tag)

            e.Row.Cells(8).Text = _oLanguage.getText("Report", "159", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("Report", "109", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("Report", "110", ctlLanguage.eumType.Tag)
            e.Row.Cells(11).Text = _oLanguage.getText("Report", "178", ctlLanguage.eumType.Tag)
            e.Row.Cells(12).Text = _oLanguage.getText("Report", "182", ctlLanguage.eumType.Tag)

            e.Row.Cells(13).Text = _oLanguage.getText("Report", "111", ctlLanguage.eumType.Tag)

            e.Row.Cells(14).Text = _oLanguage.getText("Report", "193", ctlLanguage.eumType.Tag)

            e.Row.Cells(15).Text = _oLanguage.getText("Report", "112", ctlLanguage.eumType.Tag)
            e.Row.Cells(16).Text = _oLanguage.getText("Report", "183", ctlLanguage.eumType.Tag)
            e.Row.Cells(17).Text = "EW/CW" '_oLanguage.getText("Report", "183", ctlLanguage.eumType.Tag)
            e.Row.Cells(18).Text = _oLanguage.getText("Report", "184", ctlLanguage.eumType.Tag)

            e.Row.Cells(19).Text = _oLanguage.getText("Report", "113", ctlLanguage.eumType.Tag)
            e.Row.Cells(20).Text = _oLanguage.getText("Report", "114", ctlLanguage.eumType.Tag)

            e.Row.Cells(21).Text = _oLanguage.getText("Report", "194", ctlLanguage.eumType.Tag)

            e.Row.Cells(22).Text = _oLanguage.getText("Report", "115", ctlLanguage.eumType.Tag)
            e.Row.Cells(23).Text = _oLanguage.getText("Report", "116", ctlLanguage.eumType.Tag)
            e.Row.Cells(24).Text = _oLanguage.getText("Report", "117", ctlLanguage.eumType.Tag)

            e.Row.Cells(25).Text = _oLanguage.getText("Report", "195", ctlLanguage.eumType.Tag)

            e.Row.Cells(26).Text = _oLanguage.getText("Report", "130", ctlLanguage.eumType.Tag)
            e.Row.Cells(27).Text = _oLanguage.getText("Report", "131", ctlLanguage.eumType.Tag)
            e.Row.Cells(28).Text = _oLanguage.getText("Report", "132", ctlLanguage.eumType.Tag)
            e.Row.Cells(29).Text = _oLanguage.getText("Report", "133", ctlLanguage.eumType.Tag)

            e.Row.Cells(30).Text = _oLanguage.getText("Report", "134", ctlLanguage.eumType.Tag)
            e.Row.Cells(31).Text = _oLanguage.getText("Report", "185", ctlLanguage.eumType.Tag)
            e.Row.Cells(32).Text = _oLanguage.getText("Report", "135", ctlLanguage.eumType.Tag)

            e.Row.Cells(33).Text = _oLanguage.getText("Report", "118", ctlLanguage.eumType.Tag)
            e.Row.Cells(34).Text = _oLanguage.getText("Report", "119", ctlLanguage.eumType.Tag)
            e.Row.Cells(35).Text = _oLanguage.getText("Report", "120", ctlLanguage.eumType.Tag)
            e.Row.Cells(36).Text = _oLanguage.getText("Report", "121", ctlLanguage.eumType.Tag)

            e.Row.Cells(37).Text = _oLanguage.getText("Report", "122", ctlLanguage.eumType.Tag)
            e.Row.Cells(38).Text = _oLanguage.getText("Report", "123", ctlLanguage.eumType.Tag)
            e.Row.Cells(39).Text = _oLanguage.getText("Report", "124", ctlLanguage.eumType.Tag)

            e.Row.Cells(40).Text = _oLanguage.getText("Report", "125", ctlLanguage.eumType.Tag)
            e.Row.Cells(41).Text = _oLanguage.getText("Report", "126", ctlLanguage.eumType.Tag)
            e.Row.Cells(42).Text = _oLanguage.getText("Report", "127", ctlLanguage.eumType.Tag)

            e.Row.Cells(43).Text = _oLanguage.getText("Report", "128", ctlLanguage.eumType.Tag)
            e.Row.Cells(44).Text = _oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag)
            e.Row.Cells(45).Text = _oLanguage.getText("Report", "198", ctlLanguage.eumType.Tag)
            e.Row.Cells(46).Text = _oLanguage.getText("Report", "199", ctlLanguage.eumType.Tag)
            e.Row.Cells(47).Text = _oLanguage.getText("Report", "203", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()


            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(5).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(9).Text) < Convert.ToDateTime(e.Row.Cells(4).Text) Then
                    e.Row.Cells(5).ForeColor = Drawing.Color.Red
                End If
            End If

            '隱藏不給客戶看
            If Session("_Identity").ToString().Trim() = "1" Then
                Me.tbReport.Width = "2100px"
                Me.UI_dvReport.Columns(16).Visible = False
                Me.UI_dvReport.Columns(17).Visible = False
                Me.UI_dvReport.Columns(18).Visible = False
                Me.UI_dvReport.Columns(19).Visible = False
                Me.UI_dvReport.Columns(20).Visible = False
                Me.UI_dvReport.Columns(21).Visible = False
                Me.UI_dvReport.Columns(22).Visible = False
                Me.UI_dvReport.Columns(23).Visible = False
                Me.UI_dvReport.Columns(24).Visible = False
                Me.UI_dvReport.Columns(25).Visible = False
                Me.UI_dvReport.Columns(26).Visible = False
                Me.UI_dvReport.Columns(27).Visible = False
                Me.UI_dvReport.Columns(28).Visible = False
                Me.UI_dvReport.Columns(29).Visible = False
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

    Protected Sub UI_dvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvReport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            'Dim dtReport As ReportDTO.VWRPT_RMADETAILDataTable = Session("_dtReport")
            'Call Request_DataBind(dtReport, iPageIndex)
            Call Request_DataBind(iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_CompanyName") = Me.UI_txtCompanyName.Text.Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_RmaNo") = Me.UI_txtRmaNo.Text.Trim()
        Me.ViewState("_Warranty") = Me.UI_cboWarranty.SelectedValue.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()

        Me.ViewState("_fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("_fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            'Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("_edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            'Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Me.ViewState("_SDfdate") = ""
        If Me.UI_cboSDBYear.SelectedValue <> -1 And Me.UI_cboSDBMonth.SelectedValue <> -1 And Me.UI_cboSDBDay.SelectedValue <> -1 Then
            Me.ViewState("_SDfdate") = Me.UI_cboSDBYear.SelectedValue & "/" & Me.UI_cboSDBMonth.SelectedValue & "/" & Me.UI_cboSDBDay.SelectedValue
        Else
            'Me.ViewState("_SDfdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_SDedate") = ""
        If Me.UI_cboSDEYear.SelectedValue <> -1 And Me.UI_cboSDEMonth.SelectedValue <> -1 And Me.UI_cboSDEDay.SelectedValue <> -1 Then
            Me.ViewState("_SDedate") = Me.UI_cboSDEYear.SelectedValue & "/" & Me.UI_cboSDEMonth.SelectedValue & "/" & Me.UI_cboSDEDay.SelectedValue
        Else
            'Me.ViewState("_SDedate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' 匯出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdExport.Click

        If Not Session("_dtReport") Is Nothing Then

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""
            Dim hssfworkbook As New HSSFWorkbook
            Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
            Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat

            Try
                Dim dtReport As ReportDTO.VWRPT_RMADETAILDataTable = Session("_dtReport")
                Dim dvReport As DataView = dtReport.DefaultView

                Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")

                '==========================================================================================================================================================
                '設定個欄位的寬度
                '==========================================================================================================================================================
                Dim iCount As Integer = 0

                If dvReport.Count > 0 Then
                    Dim row As HSSFRow = sheet1.CreateRow(0)
                    row.Height = 20 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue("Item")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "200", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "201", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "106", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "107", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    'Request Year
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Request Year".Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    'Request Month
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Request Month".Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "108", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "159", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "109", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "110", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "178", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "182", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "111", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "193", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "112", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "183", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    'EW/CW
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("EW/CW".Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "184", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    If Session("_Identity").ToString().Trim() <> "1" Then
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "113", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "114", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "194", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "115", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "116", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "117", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "195", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "130", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "131", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "132", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "133", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "134", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "185", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "135", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                        row.GetCell(iCount).CellStyle = style
                    End If

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "118", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "119", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "120", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "121", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "122", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "123", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "124", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "125", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "126", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "127", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "128", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "129", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "198", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "199", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "203", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue("EXPORT_SHIPPING_DATE")
                    'row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("Qty")
                    row.GetCell(iCount).CellStyle = style
                End If


                For i = 0 To dvReport.Count - 1
                    Dim dr As DataRow = dvReport(i).Row
                    Dim row As HSSFRow = sheet1.CreateRow(i + 1)
                    row.Height = 15 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue((i + 1).ToString())
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMA_COMPNO").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("COUNTRY_NAME").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMA_NO").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RequestDate").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RequestYear").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RequestMonth").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("CU_NAME").ToString().Trim())


                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMAD_SERIALNO").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMAD_MODELNO").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMAD_PRODUCTDESC").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMAR_PROBLEMDESC").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RMAR_REPAIRDESC").ToString().Trim())


                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("FAR_REASON").ToString().Trim())

                    iCount = iCount + 1
                    If dr("EXPORT_SHIPPING_DATE").ToString().Trim() <> "" Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("EXPORT_SHIPPING_DATE").ToString().Trim()).ToString("yyyy/MM/dd"))
                    Else
                        row.CreateCell(iCount).SetCellValue("")
                    End If

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("Warranty").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("sIsWarranty").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("sRMAD_ISCW").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("IMPROPERUSAGE_Text").ToString().Trim())


                    If Session("_Identity").ToString().Trim() <> "1" Then
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMARQ_ASSIGLABORCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMARQ_ASSIGMATERIALCOST").ToString().Trim())

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMACQSN_DISCOUNTAMOUNT").ToString().Trim())

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMASQ_LABORCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMASQ_MATERIALCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMASQ_QUOTE").ToString().Trim())

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMACQ_DISCOUNT").ToString().Trim())

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMAR_LABORHOUR").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMAR_LABORPRICE").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMAR_MATERIALCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMAR_QUOTE").ToString().Trim())

                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMARSD_LABORCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMARSD_MATERIALCOST").ToString().Trim())
                        iCount = iCount + 1
                        row.CreateCell(iCount).SetCellValue(dr("sRMARSD_QUOTE").ToString().Trim())
                    End If

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("COMP_NAME").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("Status").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("Received_Name").ToString().Trim())
                    iCount = iCount + 1
                    If IsDBNull(dr("Received_Date")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("Received_Date").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If


                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("Repaired_Name").ToString().Trim())
                    iCount = iCount + 1
                    If IsDBNull(dr("RepairQuoted_Date")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("RepairQuoted_Date").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If
                    iCount = iCount + 1
                    If IsDBNull(dr("Repaired_Date")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("Repaired_Date").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If


                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("Sales_Name").ToString().Trim())
                    iCount = iCount + 1
                    If IsDBNull(dr("Sales_Date")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("Sales_Date").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If
                    iCount = iCount + 1
                    If IsDBNull(dr("Client_Date")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("Client_Date").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If


                    iCount = iCount + 1
                    If IsDBNull(dr("NoticedDate")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("NoticedDate").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If
                    iCount = iCount + 1
                    If IsDBNull(dr("ShippingDate")) = False Then
                        row.CreateCell(iCount).SetCellValue(Convert.ToDateTime(dr("ShippingDate").ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss"))
                    End If

                    iCount = iCount + 1
                    If IsDBNull(dr("ShippingYear")) = False Then
                        row.CreateCell(iCount).SetCellValue(dr("ShippingYear").ToString().Trim())
                    End If

                    iCount = iCount + 1
                    If IsDBNull(dr("ShippingMonth")) = False Then
                        row.CreateCell(iCount).SetCellValue(dr("ShippingMonth").ToString().Trim())
                    End If

                    iCount = iCount + 1
                    If IsDBNull(dr("EXPORT_PARTNO")) = False Then
                        row.CreateCell(iCount).SetCellValue(dr("EXPORT_PARTNO").ToString().Trim())
                    End If

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue("1")
                Next



                For i = 0 To 44
                    sheet1.AutoSizeColumn(i)
                    sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
                Next


                Dim filename As String = oCommon.GetRandomizeNum & ".xls"
                'Dim file As FileStream = New FileStream(Server.MapPath("object\" & filename), FileMode.Create)
                'hssfworkbook.Write(file)
                'file.Close()


                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
                Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
                Response.End()

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

        End If

    End Sub

    Private Function WriteToStream(ByVal hssfworkbook As HSSFWorkbook) As MemoryStream
        'Write the stream data of workbook to the root directory
        Dim file As MemoryStream = New MemoryStream
        hssfworkbook.Write(file)
        Return file
    End Function

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

End Class

Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Repair_List
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

    Dim _rptRequestFormToPDF As String = "RequestForm_" & oCommon.GetRandomizeNum() & ".pdf"
    Dim _rptClientQuotationToPDF As String = "RepairList_" & oCommon.GetRandomizeNum() & ".pdf"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Repair") = "-1"
            Me.ViewState("_Status") = "-1"
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtSale_Status") = Nothing

            If IsNothing(Request.QueryString("RMANO")) = False Then
                Dim RMANO As String = Request.QueryString("RMANO").Trim()
                If RMANO <> "" Then
                    Me.ViewState("_RMANo") = _Crypto.Decrypt(RMANO, "")
                End If
            End If

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
        oCommon.getRepairCenteryByDropDownList(Session("_RepairCenter").ToString().Trim(), Me.UI_cboRepairCenter, TagText, True)

        oCommon.getStatus(Me.UI_cboStatus)

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())


        '取得Tag Text   
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items(0).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(6).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "271", ctlLanguage.eumType.Tag)


        Dim dtRMASales_tmp As New DataTable
        dtRMASales_tmp.Columns.Add("SEQID")
        dtRMASales_tmp.Columns.Add("RMA_NO")
        dtRMASales_tmp.Columns.Add("RMA_ID")
        dtRMASales_tmp.Columns.Add("RequestDate")
        dtRMASales_tmp.Columns.Add("Applicant")
        dtRMASales_tmp.Columns.Add("CUNAME")

        dtRMASales_tmp.Columns.Add("RepairCode")
        dtRMASales_tmp.Columns.Add("RepairQuoted")

        dtRMASales_tmp.Columns.Add("SaleCode")
        dtRMASales_tmp.Columns.Add("SaleQuoted")

        dtRMASales_tmp.Columns.Add("Status")
        dtRMASales_tmp.Columns.Add("isPrintQuotedFRBH")

        Session("_dtSale_Status") = dtRMASales_tmp
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sCustomerName As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim sRepair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim sStatus As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        Dim SQLText As String = ""

        'Session("_RepairCenter")-->只能看登入者維修點資料
        dtRequest = oClient.QueryRMAByRepairList(sRMANo, sCustomerName, sRepair, sStatus, fdate, edate, Session("_RepairCenter"), "RMA_No desc")


        Call ArrangementData(dtRequest)

        Dim dtRMASales_tmp As DataTable = Session("_dtSale_Status")
        Dim dvRMASales_tmp As DataView = dtRMASales_tmp.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvRMASales_tmp.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Request_DataBind(dvRMASales_tmp, iPageIndex)
    End Sub

    Private Sub Request_DataBind(ByVal dvRMASales_tmp As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRMASales_tmp
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0


        Dim dtRMASales_tmp As DataTable = Session("_dtSale_Status")
        dtRMASales_tmp.Rows.Clear()
        Dim dvRMASales_tmp As DataView = dtRMASales_tmp.DefaultView
        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.tmpRequest_ListRow = dtRequest.Rows(i)
            Dim RMA_No As String = dr.RMA_NO.ToString().Trim()

            dvRMASales_tmp.RowFilter = "RMA_NO='" & RMA_No & "'"
            If dvRMASales_tmp.Count = 0 Then
                iCount = iCount + 1
                Dim drTmp As DataRow = dtRMASales_tmp.NewRow
                drTmp("SEQID") = iCount
                drTmp("RMA_NO") = RMA_No
                drTmp("RMA_ID") = dr.RMA_ID.Trim()
                drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
                drTmp("Applicant") = dr.RMA_APPLICANT.Trim()
                drTmp("CUNAME") = dr.CU_NAME.Trim()
                drTmp("Status") = oCommon.ConvertToStatusText(dr.RMA_STATUS)
                drTmp("isPrintQuotedFRBH") = "0"
                dtRMASales_tmp.Rows.Add(drTmp)
            End If
        Next
        dvRMASales_tmp.RowFilter = ""



        '================================================================================================================================================================================================================================
        '處理金額
        '================================================================================================================================================================================================================================
        For i = 0 To dvRMASales_tmp.Count - 1
            Dim RMA_No As String = dvRMASales_tmp(i)("RMA_No").ToString().Trim()

            Dim dvRequest As DataView = dtRequest.DefaultView
            dvRequest.RowFilter = "RMA_NO='" & RMA_No & "'"
            For j = 0 To dvRequest.Count - 1
                Dim dr As RmaDTO.tmpRequest_ListRow = dvRequest.Item(j).Row

                If dr.IsRMAR_REPAIRADNull = False Then
                    If dr.RMAR_REPAIRAD.Trim() <> "" Then
                        dvRMASales_tmp(i)("isPrintQuotedFRBH") = "1"
                    End If
                End If


                '維修總金額
                '1.先依維修單的總金額為主
                '2.若維修單無資料,再取報價單總金額
                If dr.IsRMAR_QUOTENull = False Then
                    dvRMASales_tmp(i)("RepairCode") = dr.RMAR_CURRENCYCODE.Trim()
                    If dvRMASales_tmp(i)("RepairQuoted").ToString.Trim() = "" Then
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(dr.RMAR_QUOTE).ToString("N")
                    Else
                        Dim RepairQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("RepairQuoted")) + Convert.ToDouble(dr.RMAR_QUOTE)
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(RepairQuoted).ToString("N")
                    End If

                ElseIf dr.IsRMARQ_QUOTENull = False Then
                    dvRMASales_tmp(i)("RepairCode") = dr.RMARQ_CURRENCYCODE.Trim()
                    If dvRMASales_tmp(i)("RepairQuoted").ToString.Trim() = "" Then
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(dr.RMARQ_QUOTE).ToString("N")
                    Else
                        Dim RepairQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("RepairQuoted")) + Convert.ToDouble(dr.RMARQ_QUOTE)
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(RepairQuoted).ToString("N")
                    End If
                End If



                '業務總金額
                '1.先依業務出貨單的總金額為主
                '2.若業務出貨單無資料,再取業務報價單總金額
                If dr.IsRMARSD_QUOTENull = False Then
                    dvRMASales_tmp(i)("SaleCode") = dr.RMARSD_CURRENCYCODE.Trim()
                    If dvRMASales_tmp(i)("SaleQuoted").ToString.Trim() = "" Then
                        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(dr.RMARSD_QUOTE).ToString("N")
                    Else
                        Dim SaleQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("SaleQuoted")) + Convert.ToDouble(dr.RMARSD_QUOTE)
                        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(SaleQuoted).ToString("N")
                    End If

                    'ElseIf dr.IsRMASQ_QUOTENull = False Then
                    '    dvRMASales_tmp(i)("SaleCode") = dr.RMASQ_CURRENCYCODE.Trim()
                    '    If dvRMASales_tmp(i)("SaleQuoted").ToString.Trim() = "" Then
                    '        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N")
                    '    Else
                    '        Dim SaleQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("SaleQuoted")) + Convert.ToDouble(dr.RMASQ_QUOTE)
                    '        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(SaleQuoted).ToString("N")
                    '    End If
                End If


            Next
        Next

        dvRMASales_tmp.RowFilter = ""
        Session("_dtSale_Status") = dtRMASales_tmp
    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvRequest.PageIndex * Me.UI_dvRequest.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdPrintQuotedFRBH As Button = e.Row.FindControl("UI_cmdPrintQuotedFRBH")
            UI_cmdPrintQuotedFRBH.Text = _oLanguage.getText("Common", "420", ctlLanguage.eumType.Command)

            Dim UI_isPrintQuotedFRBH As Label = e.Row.FindControl("UI_isPrintQuotedFRBH")
            UI_cmdPrintQuotedFRBH.Visible = False
            If UI_isPrintQuotedFRBH.Text.Trim() = "1" Then
                UI_cmdPrintQuotedFRBH.Visible = True
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

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()
        If Not Session("_dtSale_Status") Is Nothing Then
            Dim dtRMASales_tmp As DataTable = Session("_dtSale_Status")
            Dim dvRMASales_tmp As DataView = dtRMASales_tmp.DefaultView
            Call Request_DataBind(dvRMASales_tmp, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand

        'If e.CommandName = "cmdDetail" Then
        '    Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

        '    Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
        '    Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
        '    Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

        '    Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.ToString().Trim()
        '    Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.ToString().Trim()
        'End If


        'If e.CommandName = "cmdEdit" Then
        '    Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

        '    Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
        '    Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
        '    Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

        '    Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.ToString().Trim()
        '    Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.ToString().Trim()
        'End If

        If e.CommandName = "cmdPrintQuotedFRBH" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Call RunPrint_RepairList(UI_RMANO.Text.Trim())
        End If

    End Sub

    Protected Sub UI_dvRequest_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvRequest.Sorting

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

        If IsNothing(Session("_dtSale_Status")) = False Then
            Dim dtRMASales_tmp As DataTable = Session("_dtSale_Status")
            Dim dvRMASales_tmp As DataView = dtRMASales_tmp.DefaultView
            dvRMASales_tmp.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Request_DataBind(dvRMASales_tmp, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvRequest.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvRequest.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvRequest.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvRequest.Columns(i).HeaderText = sHeaderText
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
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()

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

#Region "Client Quotation"

    Protected Sub RunPrint_RepairList(ByVal sRMANO As String)
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtRptRepairList_Head As New RmaDTO.vwRptRepairList_HeadDataTable
        Dim dtRptRepairList_SN As New RmaDTO.vwRptRepairList_SNDataTable
        Dim dtRptRepairList_Part As New RmaDTO.vwRptRepairList_PartDataTable

        dtRptRepairList_Head = ctlClient.QryRpt_RepairList_Head(sRMANO)
        dtRptRepairList_SN = ctlClient.QryRpt_RepairList_SN(sRMANO)
        dtRptRepairList_Part = ctlClient.QryRpt_RepairList_Part(sRMANO)


        Dim dvPart As DataView = dtRptRepairList_Part.DefaultView
        For i = 0 To dtRptRepairList_SN.Rows.Count - 1
            Dim j As Integer = 0
            Dim drSN As RmaDTO.vwRptRepairList_SNRow = dtRptRepairList_SN.Rows(i)
            drSN.SEQSN = (i + 1).ToString()

            '非正常使用: 0.No, 1.Yes
            Dim RMARQ_IMPROPERUSAGE_Text As String = "N"
            If drSN.RMARQ_IMPROPERUSAGE = 1 Then
                RMARQ_IMPROPERUSAGE_Text = "Y"
            End If
            drSN.RMARQ_IMPROPERUSAGE_TEXT = RMARQ_IMPROPERUSAGE_Text


            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            Dim RMAD_ISWARRANTY_Text As String = "N"
            If drSN.IsRMAD_ISWARRANTYNull = False Then
                If drSN.RMAD_ISWARRANTY = 1 Then
                    RMAD_ISWARRANTY_Text = "Y"
                End If
            End If
            drSN.RMAD_ISWARRANTY_TEXT = RMAD_ISWARRANTY_Text

            '是否要維修: 1.Accept, 2.Reject
            Dim RMARQ_Reject As String = ""
            If drSN.IsRMARQ_ACCEPTNull = False Then
                If drSN.RMARQ_ACCEPT = 2 Then
                    RMARQ_Reject = "Y"
                End If
            End If
            drSN.RMARQ_ACCEPT_TEXT = RMARQ_Reject

            dvPart.RowFilter = "RMAD_ID='" & drSN.RMAD_ID.Trim() & "'"
            For j = 0 To dvPart.Count - 1
                dvPart(j)("SEQPART") = (j + 1).ToString()
            Next
        Next
        dvPart.RowFilter = ""


        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptRepairList_Head)
        oDsReport.Tables.Add(dtRptRepairList_SN)
        oDsReport.Tables.Add(dtRptRepairList_Part)

        Call Print_ClientQuotation(oDsReport)
    End Sub

    Private Sub Print_ClientQuotation(ByVal oDsReport As DataSet)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptRepairList.rpt"))
        ReportDoc.SetDataSource(oDsReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _rptClientQuotationToPDF)
        oCommon.OpenPdf(Me, _rptClientQuotationToPDF)
        'ExportSetup()
        'ConfigureExportToPdf(_rptClientQuotationToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        'Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

#End Region

    'Public Sub ExportSetup()
    '    If Not System.IO.Directory.Exists(_Reoprt_FilePath) Then
    '        System.IO.Directory.CreateDirectory(_Reoprt_FilePath)
    '    End If

    '    myDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    myExportOptions = ReportDoc.ExportOptions
    '    ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
    'End Sub

    'Public Sub ConfigureExportToPdf(ByVal ReportToPDF As String)
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & ReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Dim sScript As String = ""
    '    sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & ReportToPDF & "','','');" & vbCrLf
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

End Class

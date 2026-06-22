Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Client_Status_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

    Dim _rptRequestFormToPDF As String = "RequestForm_" & oCommon.GetRandomizeNum() & ".pdf"
    Dim _rptClientQuotationToPDF As String = "ClientQuotation_" & oCommon.GetRandomizeNum() & ".pdf"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtRMA_tmp") = Nothing

            Me.ViewState("_RMANo") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

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
        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())

        Call oCommon.getQuery_ClientStatus(UI_cboStatus)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)

        Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(6).HeaderText = _oLanguage.getText("RMA", "215", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "214", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(9).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(10).HeaderText = _oLanguage.getText("RMA", "217", ctlLanguage.eumType.Tag)


        Dim dtRMA_tmp As New DataTable
        dtRMA_tmp.Columns.Add("SeqID")
        dtRMA_tmp.Columns.Add("RMA_NO")
        dtRMA_tmp.Columns.Add("RMA_ID")
        dtRMA_tmp.Columns.Add("RequestDate")
        dtRMA_tmp.Columns.Add("Applicant")
        dtRMA_tmp.Columns.Add("CurrencyCode")
        dtRMA_tmp.Columns.Add("QUOTE")
        dtRMA_tmp.Columns.Add("RMAD_STATUS")
        dtRMA_tmp.Columns.Add("PrintQuotedFRBH")

        dtRMA_tmp.Columns.Add("RequestQty")
        dtRMA_tmp.Columns.Add("ProcessingQty")
        dtRMA_tmp.Columns.Add("ShippedQty")
        dtRMA_tmp.Columns.Add("Status")
        dtRMA_tmp.Columns.Add("Remark")
        Session("_dtRMA_tmp") = dtRMA_tmp

    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()

        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString.Trim()

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

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oRMA As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        dtClientDetail = oRMA.QueryStatus(Session("_LanguageID").ToString().Trim(), Session("_CustomerID").ToString(), Session("_UserID").ToString(), sRMANo, Status, fdate, edate, "RMA_NO desc")

        Call ArrangementData(dtClientDetail)

        Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
        Dim dvDetail As DataView = dtRMA_tmp.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvDetail, iPageIndex)
    End Sub

    Private Sub RMA_DataBind(ByVal dvRMA_tmp As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRMA_tmp
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtClientList As RmaDTO.tmpClientDetailDataTable)
        Dim iSeq As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
        dtRMA_tmp.Rows.Clear()
        Dim dvRMA_tmp As DataView = dtRMA_tmp.DefaultView

        For i = 0 To dtClientList.Rows.Count - 1
            Dim dr As RmaDTO.tmpClientDetailRow = dtClientList.Rows(i)
            Dim RMA_No As String = dr.RMA_NO.ToString().Trim()

            dvRMA_tmp.RowFilter = "RMA_NO='" & RMA_No & "'"
            If dvRMA_tmp.Count = 0 Then
                Dim drTmp As DataRow = dtRMA_tmp.NewRow
                iSeq = iSeq + 1
                drTmp("SeqID") = iSeq
                drTmp("RMA_NO") = RMA_No
                drTmp("RMA_ID") = dr.RMA_ID.Trim()
                drTmp("RequestDate") = dr.RMAD_CSTMP.ToShortDateString
                drTmp("Applicant") = dr.RMA_APPLICANT.Trim()
                drTmp("RMAD_STATUS") = dr.RMAD_STATUS
                drTmp("PrintQuotedFRBH") = "0"

                If (Convert.ToInt16(dr.RMAD_STATUS) >= 50 And Convert.ToInt16(dr.RMAD_STATUS) <= 91) Then
                    drTmp("PrintQuotedFRBH") = "1"
                End If

                If dr.IsRMA_RemarkNull = False Then drTmp("Remark") = dr.RMA_Remark.Trim
                dtRMA_tmp.Rows.Add(drTmp)
            Else
                If (Convert.ToInt16(dr.RMAD_STATUS) >= 50 And Convert.ToInt16(dr.RMAD_STATUS) <= 91) Then
                    dvRMA_tmp(0)("PrintQuotedFRBH") = "1"
                End If
            End If
        Next
        dvRMA_tmp.RowFilter = ""


        'If (Convert.ToInt16(UI_RMAD_STATUS.Text.Trim) >= 50 And Convert.ToInt16(UI_RMAD_STATUS.Text.Trim) <= 90) And UI_Quote.Text.Trim() <> "" Then
        '    UI_cmdPrintQuotedFRBH.Visible = True
        'End If


        Dim dvClientList As DataView = dtClientList.DefaultView
        For i = 0 To dvRMA_tmp.Count - 1
            Dim CurrencyCode As String = ""
            Dim QUOTE As Double = 0
            Dim ProcessingQty As Integer = 0
            Dim ShippedQty As Integer = 0

            Dim RMA_No As String = dvRMA_tmp(i).Item("RMA_NO").ToString.Trim()
            dvClientList.RowFilter = "RMA_NO='" & RMA_No & "'"
            For j = 0 To dvClientList.Count - 1
                Dim dr As RmaDTO.tmpClientDetailRow = dvClientList(j).Row

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                'RMARQ_ACCEPT -->是否要維修: 1.Accept, 2.Reject
                '2015/10/21 Fairy要求-->要列出rmad_status=91資料
                If dr.RMAD_STATUS <> 91 Then
                End If
                ' If dr.IsRMARSD_QUOTENull = False Then
                '幣別
                'If dr.IsRMASQ_CURRENCYCODENull = False Then
                'CurrencyCode = dr.RMASQ_CURRENCYCODE.Trim()
                ' QUOTE = QUOTE + Convert.ToDouble(dr.RMARSD_QUOTE).ToString("N").Trim()
                '  End If
                ' Else
                'If dr.IsRMASQ_QUOTENull = False Then
                '幣別
                'If dr.IsRMASQ_CURRENCYCODENull = False Then
                'CurrencyCode = dr.RMASQ_CURRENCYCODE.Trim()
                'QUOTE = QUOTE + Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
                ' End If
                ' End If
                'End If
                'MODI BY Angel On 20151221 因為ShipmentDetail的金額記錄不正確,所以改取RMASALE_Quoted的金額
                If dr.IsRMASQ_QUOTENull = False Then
                    '幣別
                    If dr.IsRMASQ_CURRENCYCODENull = False Then
                        CurrencyCode = dr.RMASQ_CURRENCYCODE.Trim()
                        QUOTE = QUOTE + Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
                    End If
                End If

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                If Convert.ToInt16(dr.RMAD_STATUS) > 10 And Convert.ToInt16(dr.RMAD_STATUS) < 90 Then
                    ProcessingQty = ProcessingQty + 1
                End If

                If Convert.ToInt16(dr.RMAD_STATUS) = 90 Then
                    ShippedQty = ShippedQty + 1
                End If
            Next


            dvRMA_tmp(i)("CurrencyCode") = CurrencyCode
            If CurrencyCode.Trim <> "" Then
                dvRMA_tmp(i)("QUOTE") = Convert.ToDouble(QUOTE).ToString("N").Trim()
            End If

            dvRMA_tmp(i)("RequestQty") = dvClientList.Count
            dvRMA_tmp(i)("ProcessingQty") = ProcessingQty
            dvRMA_tmp(i)("ShippedQty") = ShippedQty
        Next
        dvRMA_tmp.RowFilter = ""


        Session("_dtRMA_tmp") = dtRMA_tmp
    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvRequest.PageIndex * Me.UI_dvRequest.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdPrintForm As Button = e.Row.FindControl("UI_cmdPrintForm")
            UI_cmdPrintForm.Text = _oLanguage.getText("Common", "419", ctlLanguage.eumType.Command)

            '20220912 wisely add 若還沒有收貨 不允許user 列印
            Dim UI_RMAD_STATUS As Label = e.Row.FindControl("UI_RMAD_STATUS")
            If UI_RMAD_STATUS.Text = "0" Then
                UI_cmdPrintForm.Visible = False
            Else
                UI_cmdPrintForm.Visible = True
            End If


            Dim UI_cmdPrintQuotedFRBH As Button = e.Row.FindControl("UI_cmdPrintQuotedFRBH")
            UI_cmdPrintQuotedFRBH.Text = _oLanguage.getText("Common", "420", ctlLanguage.eumType.Command)
            UI_cmdPrintQuotedFRBH.Visible = False

            Dim UI_cmdPrintQuotedFRBH_TW As Button = e.Row.FindControl("UI_cmdPrintQuotedFRBH_TW")
            If UI_cmdPrintQuotedFRBH_TW IsNot Nothing Then
                UI_cmdPrintQuotedFRBH_TW.Text = _oLanguage.getText("Common", "420", ctlLanguage.eumType.Command)
                UI_cmdPrintQuotedFRBH_TW.Visible = False
            End If
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Dim UI_Quote As Label = e.Row.FindControl("UI_Quote")
            Dim UI_PrintQuotedFRBH As Label = e.Row.FindControl("UI_PrintQuotedFRBH")
            If UI_PrintQuotedFRBH.Text = "1" And UI_Quote.Text.Trim() <> "" Then
                UI_cmdPrintQuotedFRBH.Visible = True
            End If

            If UI_RMAD_STATUS.Text = "40" Or UI_RMAD_STATUS.Text = "50" Or UI_RMAD_STATUS.Text = "60" Or UI_RMAD_STATUS.Text = "70" Or UI_RMAD_STATUS.Text = "90" Then
                UI_cmdPrintQuotedFRBH.Visible = True
            End If

            If Session("_RepairID") = "CLHQ" Then
                UI_cmdPrintQuotedFRBH.Visible = False
                If UI_cmdPrintQuotedFRBH_TW IsNot Nothing Then
                    UI_cmdPrintQuotedFRBH_TW.Visible = True
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

        '20230711 0 不能列印
        If e.Row.RowType = DataControlRowType.DataRow Then

            If ("0" = "0") Then

                Dim UI_Quote As Label = e.Row.FindControl("UI_Quote")
                Dim UI_cmdPrintForm As Button = e.Row.FindControl("UI_cmdPrintForm")
                Dim UI_cmdPrintQuotedFRBH As Button = e.Row.FindControl("UI_cmdPrintQuotedFRBH")

                If (UI_Quote.Text.Trim() = "0") Then

                    UI_cmdPrintForm.Visible = False
                    UI_cmdPrintQuotedFRBH.Visible = False

                End If
            End If
        End If

    End Sub

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRMA_tmp") Is Nothing Then
            Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
            Dim dvDetail As DataView = dtRMA_tmp.DefaultView
            Call RMA_DataBind(dvDetail, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
        End If


        If e.CommandName = "cmdPrintForm" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")

            '20230711 個別
            Dim oRequested As New ctlRMA.Requested
            Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(UI_RMAID.Text.Trim())
            Dim _Crypto As New SecurityCrypt.Crypto
            Dim sParm_01 As String = _Crypto.Encrypt(UI_RMAID.Text.Trim(), "")
            Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
            Dim sRedirectURL As String = "Request_Print.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02
            Response.Redirect(sRedirectURL)

            Call RunPrint_RequestForm(UI_RMAID.Text.Trim())
        End If

        If e.CommandName = "cmdPrintQuotedFRBH" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Call RunPrint_ClientQuotation(UI_RMANO.Text.Trim())
        End If

        If e.CommandName = "cmdPrintQuotedFRBH_TW" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Call RunPrint_ClientQuotation_TW(UI_RMANO.Text.Trim())
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

        If IsNothing(Session("_dtRMA_tmp")) = False Then
            Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
            Dim dvDetail As DataView = dtRMA_tmp.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvDetail, 0)
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

#Region "Request Form"

    Protected Sub RunPrint_RequestForm(ByVal RMAID As String)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""
        Dim bIsCW As Boolean = False

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", RMAID)

        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), RMAID)

        'dtRequest = oRMARequest.getReport(Session("_LanguageID").ToString().Trim(), RMAID)

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

            '20210513 置換Model
            'If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            sModelNo = oExport.getMModelNo(dr.RMAD_MODELNO.ToString().Trim(), dr.RMA_COMPNO.ToString().Trim(), dr.RMA_ACCOUNTID.ToString().Trim())
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = sModelNo

            If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
            If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
            If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
            If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
            If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()

            If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
            If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
            If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
            If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
            If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()

            drReport.SeqID = i + 1

            If dr.IsRMAD_ISCWNull = False Then
                If dr.RMAD_ISCW.ToString().Trim() = "1" Then
                    bIsCW = True
                End If
            End If

            '20221205 wisely add 若為全保送修 沒有寫Enduser住址 用自己的
            'Dim oRequested As New ctlRMA.Requested
            'Dim dt As DataTable = oRequested.IsEndUser(dr.RMA_CUNO.ToString().Trim())
            If (bIsCW AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                'drReport.RMA_EUMAIL = dt.Rows(0)("cu_email").ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If

            dtReport.AddRequestReportRow(drReport)
        Next

        Call Print_RequestForm(dtReport, bIsCW, LanguageID)
    End Sub

    Private Sub Print_RequestForm(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal bIsCW As Boolean, ByVal LanguageID As String)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        'If LanguageID = "003" Then
        '    ReportDoc.Load(Server.MapPath("Report\rptRequest_jp.rpt"))
        'Else
        '    ReportDoc.Load(Server.MapPath("Report\rptRequest.rpt"))
        'End If
        'ReportDoc.SetDataSource(oReport)

        If (bIsCW) Then
            If (LanguageID = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            End If
        Else
            If LanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest.rpt"))
            End If
        End If
        ReportDoc.SetDataSource(oReport)


        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _rptRequestFormToPDF)
        oCommon.OpenPdf(Me, _rptRequestFormToPDF)
        'ExportSetup()
        'ConfigureExportToPdf(_rptRequestFormToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

#End Region

#Region "Client Quotation"

    Protected Sub RunPrint_ClientQuotation_TW(ByVal sRMANO As String)
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
        Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
        Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable

        dtClient_SalesQuoted_Head = ctlClient.QryClient_SalesQuoted_Head(sRMANO)
        dtClient_SalesQuoted_SN = ctlClient.QryClient_SalesQuoted_SN_001(sRMANO)
        dtClient_SalesQuoted_Part = ctlClient.QryClient_SalesQuoted_Part(sRMANO)

        Dim sRMA_COMPNO As String = ""
        If dtClient_SalesQuoted_Head.Rows.Count > 0 Then
            sRMA_COMPNO = dtClient_SalesQuoted_Head.Rows(0)("RMA_COMPNO").ToString().Trim()
        End If


        Dim dvPart As DataView = dtClient_SalesQuoted_Part.DefaultView
        For i = 0 To dtClient_SalesQuoted_SN.Rows.Count - 1
            Dim j As Integer = 0
            Dim drSN As RmaDTO.VWRPTCLIENT_SALESQUOTED_SNRow = dtClient_SalesQuoted_SN.Rows(i)
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
        oDsReport.Tables.Add(dtClient_SalesQuoted_Head)
        oDsReport.Tables.Add(dtClient_SalesQuoted_SN)
        oDsReport.Tables.Add(dtClient_SalesQuoted_Part)

        Call Print_ClientQuotation(oDsReport, sRMANO, sRMA_COMPNO)
    End Sub
    Protected Sub RunPrint_ClientQuotation(ByVal sRMANO As String)
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
        Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
        Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable

        dtClient_SalesQuoted_Head = ctlClient.QryClient_SalesQuoted_Head(sRMANO)
        dtClient_SalesQuoted_SN = ctlClient.QryClient_SalesQuoted_SN_001(sRMANO)
        dtClient_SalesQuoted_Part = ctlClient.QryClient_SalesQuoted_Part(sRMANO)

        Dim sRMA_COMPNO As String = ""
        If dtClient_SalesQuoted_Head.Rows.Count > 0 Then
            sRMA_COMPNO = dtClient_SalesQuoted_Head.Rows(0)("RMA_COMPNO").ToString().Trim()
        End If


        Dim dvPart As DataView = dtClient_SalesQuoted_Part.DefaultView
        For i = 0 To dtClient_SalesQuoted_SN.Rows.Count - 1
            Dim j As Integer = 0
            Dim drSN As RmaDTO.VWRPTCLIENT_SALESQUOTED_SNRow = dtClient_SalesQuoted_SN.Rows(i)
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
        oDsReport.Tables.Add(dtClient_SalesQuoted_Head)
        oDsReport.Tables.Add(dtClient_SalesQuoted_SN)
        oDsReport.Tables.Add(dtClient_SalesQuoted_Part)

        Call Print_ClientQuotation(oDsReport, sRMANO, sRMA_COMPNO)
    End Sub

    Private Sub Print_ClientQuotation(ByVal oDsReport As DataSet, ByVal sRMANO As String, ByVal sRMA_COMPNO As String)
        Dim ctlChargeQuoted As New ctlChargeQuoted

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument

        '取得客戶語系
        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMANO)


        If ctlChargeQuoted.chkIsExist(sRMANO) = True Then
            If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
                ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted_CHINA.rpt"))
            Else
                If LanguageID = "003" Then
                    ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted_jp.rpt"))
                Else
                    ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted.rpt"))
                End If

            End If
        Else
            If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
                ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt"))
            Else
                If LanguageID = "003" Then
                    ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_jp.rpt"))
                Else

                    '新增paypal 打印文件特別備註 2023/7/5 ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                    Dim oRequested As New ctlRMA.Requested
                    Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
                    If (dt.Rows.Count > 0) Then
                        ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_X0091.rpt"))
                    Else
                        ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                    End If

                End If

            End If
        End If
        'ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt"))

        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _rptClientQuotationToPDF)
        oCommon.OpenPdf(Me, _rptClientQuotationToPDF)
        'ExportSetup()
        'ConfigureExportToPdf(_rptClientQuotationToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
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

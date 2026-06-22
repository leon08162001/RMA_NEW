Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports RMA_Common

Partial Class Client_Status_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _Comms As New Commons
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

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function

    Public Function Select_RMA_STATUS(ByVal RMA_NO As String) As String
        Dim myctAddress As New ctAddress
        Dim dt As New DataTable
        dt = myctAddress.Select_RMA(RMA_NO)


        Return dt.Rows(0)("RMA_STATUS").ToString().Trim()



    End Function

    Public Function getoLanguageword(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Word)
    End Function

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

            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"


        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()


        'Call oCommon.getQuery_ClientStatus(UI_cboStatus)
        Me.UI_cboStatus.Items.Clear()

        Dim oListItem As ListItem

        oListItem = New ListItem
        oListItem.Value = "-1"
        oListItem.Text = _oLanguage.getText("RMA2", "064", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA2", "108", ctlLanguage.eumType.Tag)
        oListItem.Value = "0"
        UI_cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA2", "109", ctlLanguage.eumType.Tag)
        oListItem.Value = "10"
        UI_cboStatus.Items.Add(oListItem)

        'Processing
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA2", "110", ctlLanguage.eumType.Tag)
        oListItem.Value = "20"
        UI_cboStatus.Items.Add(oListItem)

        'Closed
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA2", "112", ctlLanguage.eumType.Tag)
        oListItem.Value = "90"
        UI_cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA2", "111", ctlLanguage.eumType.Tag)
        oListItem.Value = "91"
        UI_cboStatus.Items.Add(oListItem)


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.ResetBtn.Text = _oLanguage.getText("RMA2", "009", ctlLanguage.eumType.Tag)

        Me.lblInformation.Text = _oLanguage.getText("RMA2", "103", ctlLanguage.eumType.Tag)

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

        'RMA2		
        Me.RMA_NoLab.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.Request_DateLab.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.Contact_PersonLab.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.Original_ChargeLab.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.ContentLab.Text = _oLanguage.getText("RMA2", "052", ctlLanguage.eumType.Tag)
        Me.PrintLab.Text = _oLanguage.getText("RMA2", "053", ctlLanguage.eumType.Tag)
        Me.UI_txtRMANo.Attributes.Add("placeholder", _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag))
        Me.Status_Lab.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)


        '20231222
        Me.UI_cboStatus_Change.Items.Clear()

        Dim myUI_cboStatus_001 As New ListItem
        myUI_cboStatus_001.Value = 0
        myUI_cboStatus_001.Text = _oLanguage.getText("RMA2", "064", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus_Change.Items.Add(myUI_cboStatus_001)

        Dim myUI_cboStatus_002 As New ListItem
        myUI_cboStatus_002.Value = 1
        myUI_cboStatus_002.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus_Change.Items.Add(myUI_cboStatus_002)


        Dim myUI_cboStatus_003 As New ListItem
        myUI_cboStatus_003.Value = 2
        myUI_cboStatus_003.Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus_Change.Items.Add(myUI_cboStatus_003)

    End Sub

    Private Sub RMA_DataBind(ByVal dvRMA_tmp As DataView, ByVal iPageIndex As Integer)

        Me.count_Lab.Text = dvRMA_tmp.Count.ToString()
        Me.UI_dvRMAListView.DataSource = dvRMA_tmp
        Me.UI_dvRMAListView.DataBind()

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)

        Dim oRMA As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        If UI_cboStatus_Change.SelectedIndex = 0 Then
            dtClientDetail = oRMA.QueryStatus_ALL(Session("_LanguageID").ToString().Trim(), Session("_CustomerID").ToString(), Session("_UserID").ToString(), sRMANo, Status, fdate, edate, "RMA_NO desc")

        ElseIf UI_cboStatus_Change.SelectedIndex = 1 Then
            dtClientDetail = oRMA.QueryStatus(Session("_LanguageID").ToString().Trim(), Session("_CustomerID").ToString(), Session("_UserID").ToString(), sRMANo, Status, fdate, edate, "RMA_NO desc")

        ElseIf UI_cboStatus_Change.SelectedIndex = 2 Then
            dtClientDetail = oRMA.QueryStatus_Customer_Number(Session("_LanguageID").ToString().Trim(), Session("_CustomerID").ToString(), Session("_UserID").ToString(), sRMANo, Status, fdate, edate, "RMA_NO desc")
        End If


        Call ArrangementData(dtClientDetail)

        Dim dtRMA_tmp As DataTable = Session("_dtRMA_tmp")
        Dim dvDetail As DataView = dtRMA_tmp.DefaultView
        'Me.ViewState("_SortExpression") = "RMA_NO"
        'Me.ViewState("_SortDirection") = "desc"
        'dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
        Session("Client_Status_List") = dtRMA_tmp
        Call RMA_DataBind(dvDetail, iPageIndex)

        Dim myDecimal As Decimal = dtRMA_tmp.Rows.Count / _PageSize
        Me.BookMark_Label.Value = "0"

        If dtRMA_tmp.Rows.Count = 0 Then
            Current_Page_Label.Text = "0"
        Else
            Current_Page_Label.Text = "1"
        End If

        Total_Page_Label.Text = Math.Ceiling(myDecimal).ToString()

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
                drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
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

    Protected Sub UI_dvRMAListView_PagePropertiesChanging(sender As Object, e As PagePropertiesChangingEventArgs)
        TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        Dim dvDetail As DataView = dvRMA_tmp.DefaultView
        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        'count_Lab.Text = dvDetail.Count.ToString()
        Me.UI_dvRMAListView.DataSource = dvDetail
        Me.UI_dvRMAListView.DataBind()

    End Sub

    Private Sub UI_dvRMAListView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles UI_dvRMAListView.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then




            Dim UI_RequestedProcessedLabel As Label = TryCast(e.Item.FindControl("RequestedProcessedLabel"), Label)
            UI_RequestedProcessedLabel.Text = _oLanguage.getText("RMA2", "108", ctlLanguage.eumType.Tag)

            Dim NotProcessedLabel_LB As Label = TryCast(e.Item.FindControl("NotProcessedLabel"), Label)
            NotProcessedLabel_LB.Text = _oLanguage.getText("RMA2", "109", ctlLanguage.eumType.Tag)

            Dim ProcessingLabel_LB As Label = TryCast(e.Item.FindControl("ProcessingLabel"), Label)
            ProcessingLabel_LB.Text = _oLanguage.getText("RMA2", "110", ctlLanguage.eumType.Tag)

            Dim ClosedLabel_LB As Label = TryCast(e.Item.FindControl("ClosedLabel"), Label)
            ClosedLabel_LB.Text = _oLanguage.getText("RMA2", "111", ctlLanguage.eumType.Tag)

            Dim CanceledLabel_LB As Label = TryCast(e.Item.FindControl("CanceledLabel"), Label)
            CanceledLabel_LB.Text = _oLanguage.getText("RMA2", "112", ctlLanguage.eumType.Tag)

            Dim UI_cmdPrintForm_Btn As Button = TryCast(e.Item.FindControl("UI_cmdPrintForm"), Button)
            Dim UI_cmdPrintQuotedFRBH_Btn As Button = TryCast(e.Item.FindControl("UI_cmdPrintQuotedFRBH"), Button)

            Dim UI_cmdPrintForm_LB As Button = TryCast(e.Item.FindControl("UI_cmdPrintForm"), Button)
            Dim UI_cmdPrintQuotedFRBH_LB As Button = TryCast(e.Item.FindControl("UI_cmdPrintQuotedFRBH"), Button)

            Dim UI_RMAD_STATUS_HiddenField As HiddenField = TryCast(e.Item.FindControl("UI_RMAD_STATUS_HiddenField"), HiddenField)
            Dim UI_Quote_HiddenField As HiddenField = TryCast(e.Item.FindControl("UI_Quote_HiddenField"), HiddenField)

            Dim UI_RMAD_STATUS_ES_HiddenField As HiddenField = TryCast(e.Item.FindControl("UI_RMAD_STATUS_ES_HiddenField"), HiddenField)

            Dim UI_cmdDetails As Button = TryCast(e.Item.FindControl("UI_cmdDetail"), Button)
            UI_cmdDetails.Text = _oLanguage.getText("Common", "025", ctlLanguage.eumType.Command)

            'RMA2
            Dim UI_RemarkLab As Label = TryCast(e.Item.FindControl("RemarkLab"), Label)
            UI_RemarkLab.Text = _oLanguage.getText("RMA2", "051", ctlLanguage.eumType.Tag)
            UI_cmdPrintForm_LB.Text = _oLanguage.getText("RMA2", "049", ctlLanguage.eumType.Tag)
            UI_cmdPrintQuotedFRBH_Btn.Text = _oLanguage.getText("RMA2", "050", ctlLanguage.eumType.Tag)
            UI_cmdPrintForm_Btn.Visible = False
            UI_cmdPrintQuotedFRBH_Btn.Visible = False

            If UI_RMAD_STATUS_HiddenField.Value.Trim() = "0" Then
                UI_cmdPrintForm_Btn.Visible = False
            Else
                UI_cmdPrintForm_Btn.Visible = True
            End If

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            If UI_cmdPrintQuotedFRBH_Btn.Text = "1" And UI_Quote_HiddenField.Value.Trim() <> "" Then
                UI_cmdPrintQuotedFRBH_Btn.Visible = True
            End If


            If UI_RMAD_STATUS_HiddenField.Value.Trim() = "40" Or UI_RMAD_STATUS_HiddenField.Value.Trim() = "50" Or UI_RMAD_STATUS_HiddenField.Value.Trim() = "60" Or UI_RMAD_STATUS_HiddenField.Value.Trim() = "70" Or UI_RMAD_STATUS_HiddenField.Value.Trim() = "90" Or UI_RMAD_STATUS_HiddenField.Value.Trim() = "91" Then
                UI_cmdPrintQuotedFRBH_Btn.Visible = True
            End If


            '狀態
            If UI_RMAD_STATUS_HiddenField.Value.Trim() = "" Then

            ElseIf UI_RMAD_STATUS_HiddenField.Value.Trim() = "0" Then
                UI_RequestedProcessedLabel.Visible = True
                NotProcessedLabel_LB.Visible = False
                ProcessingLabel_LB.Visible = False
                ClosedLabel_LB.Visible = False
                CanceledLabel_LB.Visible = False

            ElseIf UI_RMAD_STATUS_HiddenField.Value.Trim() = "10" Then

                UI_RequestedProcessedLabel.Visible = False
                NotProcessedLabel_LB.Visible = True
                ProcessingLabel_LB.Visible = False
                ClosedLabel_LB.Visible = False
                CanceledLabel_LB.Visible = False

            ElseIf UI_RMAD_STATUS_HiddenField.Value.Trim() = "20" Then
                UI_RequestedProcessedLabel.Visible = False
                NotProcessedLabel_LB.Visible = False
                ProcessingLabel_LB.Visible = True
                ClosedLabel_LB.Visible = False
                CanceledLabel_LB.Visible = False


            ElseIf UI_RMAD_STATUS_HiddenField.Value.Trim() = "90" Then

                UI_RequestedProcessedLabel.Visible = False
                NotProcessedLabel_LB.Visible = False
                ProcessingLabel_LB.Visible = False
                ClosedLabel_LB.Visible = False
                CanceledLabel_LB.Visible = True

            ElseIf UI_RMAD_STATUS_HiddenField.Value.Trim() = "91" Then

                UI_RequestedProcessedLabel.Visible = False
                NotProcessedLabel_LB.Visible = False
                ProcessingLabel_LB.Visible = False
                ClosedLabel_LB.Visible = True
                CanceledLabel_LB.Visible = False

            End If
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoted, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled';

            If UI_RMAD_STATUS_ES_HiddenField.Value.Trim() = "10" Or UI_RMAD_STATUS_ES_HiddenField.Value.Trim() = "20" Or UI_RMAD_STATUS_ES_HiddenField.Value.Trim() = "0" Then
                UI_cmdPrintQuotedFRBH_Btn.Visible = False
            Else
                UI_cmdPrintQuotedFRBH_Btn.Visible = True

            End If

            Dim UI_Quote As HiddenField = TryCast(e.Item.FindControl("UI_Quote"), HiddenField)
            If (UI_Quote.Value.Trim() = "") Then
                UI_cmdPrintQuotedFRBH_Btn.Visible = False
            End If


        End If
    End Sub

    Protected Sub UI_dvRMAListView_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles UI_dvRMAListView.ItemCommand

        If e.CommandName = "cmdDetail" Then

            Me.UI_lblPreviousPage_RMANO.Text = e.CommandArgument
            Session("Client_Status_Item_new_UI_lblPreviousPage_RMAID") = e.CommandArgument
            Session("Client_Status_Item_new_UI_lblPreviousPage_RMANO") = e.CommandArgument
            Dim _SecurityCrypt As New SecurityCrypt.Crypto


            Dim theHeight As Integer = 0
            If (Request.Cookies("windowhigh") IsNot Nothing) Then

                theHeight = Convert.ToInt32(Request.Cookies("windowhigh").Value)
            End If
            If theHeight >= 768 Then
                theHeight = 768

            Else
                theHeight = theHeight * 0.9
            End If

            Dim theWidth As Integer = 0
            If (Request.Cookies("windowWidth") IsNot Nothing) Then

                theWidth = Convert.ToInt32(Request.Cookies("windowWidth").Value)
            End If

            If theWidth >= 1300 Then
                theWidth = 1300


            Else
                theWidth = theWidth - 25
            End If


            Me.windowLab.Text = " <iframe   src='Client_Status_New_Item.aspx?RMANO=" & _SecurityCrypt.Encrypt(Me.UI_lblPreviousPage_RMANO.Text, "") & "' class='UI_Add_RMA_panel_iframe_Small' style='width:" & theWidth & "px;height:" & theHeight & "px;background:#fff;' ></iframe>  "
            UI_Up_RMA_panel_ModalPopupExtender.Show()

        End If

        If e.CommandName = "cmdPrintForm" Then

            Dim oRequested As New ctlRMA.Requested
            Dim myRMADataTable As New RmaDTO.RMADataTable

            Dim UI_RMAID As New Label
            UI_RMAID.Text = e.CommandArgument
            Dim context As String() = UI_RMAID.Text.ToString().Split("|")
            myRMADataTable = oRequested.QueryByRMAHead(context(1).ToString().Trim())

            If myRMADataTable.Rows(0)("RMA_EUADDRESS").ToString().Trim() <> "" Then

                Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(context(0).ToString().Trim())
                Dim _Crypto As New SecurityCrypt.Crypto
                Dim sParm_01 As String = _Crypto.Encrypt(context(0).ToString().Trim(), "")
                Dim sParm_02 As String = _Crypto.Encrypt(True, "")
                Dim sRedirectURL As String = "ProductInformation_05_002.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02

                Response.Redirect(sRedirectURL)

            Else

                Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(context(0).ToString())
                Dim _Crypto As New SecurityCrypt.Crypto
                Dim sParm_01 As String = _Crypto.Encrypt(context(0).ToString(), "")
                Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
                Dim sRedirectURL As String = "ProductInformation_06.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02

                ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", "ShowInfoPage('" & sRedirectURL & "')", True)

            End If



        End If

        If e.CommandName = "cmdPrintQuotedFRBH" Then
            Call RunPrint_ClientQuotation(e.CommandArgument)
        End If

    End Sub

#Region "Client Quotation"

    Protected Sub RunPrint_ClientQuotation(ByVal sRMANO As String)
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
        Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
        Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable

        dtClient_SalesQuoted_Head = ctlClient.QryClient_SalesQuoted_Head(sRMANO)
        dtClient_SalesQuoted_SN = ctlClient.QryClient_SalesQuoted_SN(sRMANO)
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

        Try
            dtClient_SalesQuoted_Head.Rows(0)("RMACQ_DISCOUNT") = dtClient_SalesQuoted_Head.Rows(0)("RMACQ_QUOTE_ORIGINAL") - dtClient_SalesQuoted_Head.Rows(0)("RMACQ_CHARGEQUOTE")
        Catch
        End Try


        oDsReport.Tables.Add(dtClient_SalesQuoted_SN)
        oDsReport.Tables.Add(dtClient_SalesQuoted_Part)

        Call Print_ClientQuotation(oDsReport, sRMANO, sRMA_COMPNO)
    End Sub

    Private Sub Print_ClientQuotation(ByVal oDsReport As DataSet, ByVal sRMANO As String, ByVal sRMA_COMPNO As String)
        Dim ctlChargeQuoted As New ctlChargeQuoted

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        'ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument

        '取得客戶語系
        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMANO)
        Dim Server_MapPath_rpt As String = ""

        If ctlChargeQuoted.chkIsExist(sRMANO) = True Then
            If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
                Server_MapPath_rpt = Server.MapPath("Report\rptClient_ChargeQuoted_CHINA.rpt")
                'ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted_CHINA.rpt"))
            Else
                If LanguageID = "003" Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptClient_ChargeQuoted_jp.rpt")
                    'ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted_jp.rpt"))
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptClient_ChargeQuoted.rpt")
                    'ReportDoc.Load(Server.MapPath("Report\rptClient_ChargeQuoted.rpt"))
                End If

            End If
        Else
            If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
                Server_MapPath_rpt = Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt")
                'ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt"))
            Else
                If LanguageID = "003" Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptClient_SalesQuoted_jp.rpt")
                    'ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_jp.rpt"))
                Else

                    '新增paypal 打印文件特別備註 2023/7/5 ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                    Dim oRequested As New ctlRMA.Requested
                    Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
                    If (dt.Rows.Count > 0) Then
                        Server_MapPath_rpt = Server.MapPath("Report\rptClient_SalesQuoted_X0091.rpt")
                        'ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_X0091.rpt"))
                    Else
                        Server_MapPath_rpt = Server.MapPath("Report\rptClient_SalesQuoted.rpt")
                        'ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                    End If

                End If

            End If
        End If

        'ReportDoc.SetDataSource(oDsReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        '修改Export函式。因本機開發環境vs2022所以Crystal Report需用v13，並且環境無法新舊版共存。
        '所以將舊版封裝成exe檔案，用呼叫exe方式呼叫舊版元件 by buck modify 20251106 begin
        Dim exportPath As String = IO.Path.Combine(_Reoprt_FilePath, _rptClientQuotationToPDF)
        _Comms.ExportSetup_New(Server_MapPath_rpt, exportPath, oDsReport)
        '_Comms.ExportSetup_New(Server_MapPath_rpt, exportPath, oDsReport.Tables(0).Rows)
        'oCommon.ExportSetup(ReportDoc, _rptClientQuotationToPDF)
        _Comms.OpenPdf(Me, _rptClientQuotationToPDF)
        'ExportSetup()
        'ConfigureExportToPdf(_rptClientQuotationToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        'Me.CrystalReportViewer1.Visible = False
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

    'Public Sub ConfigureExportToPdf(ByVal ReportToPDF As String)
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & ReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", "ShowInfoPage('" & _WEBURL & _Report_VisualPath & ReportToPDF & "')", True)

    'End Sub
#End Region

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        'RMA 前端日曆選項故障 by buck modify 20250925 begin
        Me.txtStart.Text = hfStartDate.Value
        Me.txtEnd.Text = hfEndDate.Value
        'RMA 前端日曆選項故障 by buck modify 20250925 end

        ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", " HideProgressBar();", True)

        If Me.UI_txtRMANo.Text.Trim() = "Please enter RMA Number" Then
            Me.UI_txtRMANo.Text = ""
        End If

        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString.Trim()

        Me.ViewState("_fdate") = ""
        If txtStart.Text.Trim() <> "" Then
            Me.ViewState("_fdate") = txtStart.Text.Trim()
        Else
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_edate") = ""
        If txtEnd.Text.Trim() <> "" Then
            Me.ViewState("_edate") = txtEnd.Text.Trim()
        Else
            Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

#Region "分頁器"
    Protected Sub first_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles first_ImageBtn.Click
        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(0, 10, False)

        Dim dvDetail As DataView = dvRMA_tmp.DefaultView

        Me.UI_dvRMAListView.DataSource = dvDetail
        Me.UI_dvRMAListView.DataBind()

        Me.BookMark_Label.Value = "0"
        Current_Page_Label.Text = "1"
    End Sub
    Protected Sub previous_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles previous_ImageBtn.Click

        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        Dim myDecimal As Decimal = dvRMA_tmp.Rows.Count / _PageSize
        Dim UI_dvRMAListView_index As Integer   '當前頁面

        If Convert.ToInt32(Me.BookMark_Label.Value.Trim()) > 0 Then
            UI_dvRMAListView_index = Convert.ToInt32(Me.BookMark_Label.Value.Trim()) - 1
            Me.BookMark_Label.Value = (Convert.ToInt32(Me.BookMark_Label.Value.Trim()) - 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
        End If

        TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(UI_dvRMAListView_index * 10, 10, False)
        Dim dvDetail As DataView = dvRMA_tmp.DefaultView

        Me.UI_dvRMAListView.DataSource = dvDetail
        Me.UI_dvRMAListView.DataBind()

    End Sub
    Protected Sub next_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles next_ImageBtn.Click
        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        Dim myDecimal As Decimal = dvRMA_tmp.Rows.Count / _PageSize
        Dim UI_dvRMAListView_index As Integer   '當前頁面

        If Convert.ToInt32(Me.BookMark_Label.Value.Trim()) < Math.Ceiling(myDecimal) - 1 Then
            UI_dvRMAListView_index = Convert.ToInt32(Me.BookMark_Label.Value.Trim()) + 1
            Me.BookMark_Label.Value = (Convert.ToInt32(Me.BookMark_Label.Value.Trim()) + 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) + 1).ToString()
            TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(UI_dvRMAListView_index * 10, 10, False)
            Dim dvDetail As DataView = dvRMA_tmp.DefaultView
            Me.UI_dvRMAListView.DataSource = dvDetail
            Me.UI_dvRMAListView.DataBind()
        End If

    End Sub
    Protected Sub last_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles last_ImageBtn.Click
        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        Dim myDecimal As Decimal = Math.Ceiling(dvRMA_tmp.Rows.Count / _PageSize)
        Dim UI_dvRMAListView_index As Integer   '當前頁面
        UI_dvRMAListView_index = Math.Floor(myDecimal) - 1
        TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(UI_dvRMAListView_index * 10, 10, False)

        Dim dvDetail As DataView = dvRMA_tmp.DefaultView
        Me.UI_dvRMAListView.DataSource = dvDetail
        Me.UI_dvRMAListView.DataBind()

        Me.BookMark_Label.Value = "0"
        Current_Page_Label.Text = "1"

        Me.BookMark_Label.Value = UI_dvRMAListView_index
        Current_Page_Label.Text = (UI_dvRMAListView_index + 1).ToString()
    End Sub
    Protected Sub Current_Page_Label_TextChanged(sender As Object, e As EventArgs) Handles Current_Page_Label.TextChanged
        Dim dvRMA_tmp As New DataTable
        dvRMA_tmp = Session("Client_Status_List")

        Dim myDecimal As Decimal = dvRMA_tmp.Rows.Count / _PageSize
        Dim UI_dvRMAListView_index As Integer   '當前頁面

        If IsNumeric(Current_Page_Label.Text.Trim()) Then

            If Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1 <= Math.Ceiling(myDecimal) - 1 And Convert.ToInt32(Current_Page_Label.Text.Trim()) > 0 Then
                UI_dvRMAListView_index = Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1
                Me.BookMark_Label.Value = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
                Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim())).ToString()

                TryCast(UI_dvRMAListView.FindControl("DataPager1"), DataPager).SetPageProperties(UI_dvRMAListView_index * 10, 10, False)

                Dim dvDetail As DataView = dvRMA_tmp.DefaultView
                Me.UI_dvRMAListView.DataSource = dvDetail
                Me.UI_dvRMAListView.DataBind()

            Else
                Current_Page_Label.Text = ""

            End If

        Else
            Current_Page_Label.Text = ""

        End If
    End Sub
#End Region

#Region ""

    Protected Sub RMA_NoLab_Click(sender As Object, e As EventArgs) Handles RMA_NoLab.Click
        If Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("RMA_NO") Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("RMA_NO") Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If
    End Sub
    Protected Sub RMA_NoLab_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles RMA_NoLab_ImageBtn.Click

        If Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.RMA_NoLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If

    End Sub

    Protected Sub Request_DateLab_Click(sender As Object, e As EventArgs) Handles Request_DateLab.Click
        If Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If
    End Sub
    Protected Sub Request_DateLab_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles Request_DateLab_ImageBtn.Click

        If Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDateTime(p.Item("RequestDate")) Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Request_DateLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If

    End Sub

    Protected Sub Contact_PersonLab_Click(sender As Object, e As EventArgs) Handles Contact_PersonLab.Click
        If Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("Applicant") Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("Applicant") Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If
    End Sub
    Protected Sub Contact_PersonLab_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles Contact_PersonLab_ImageBtn.Click
        If Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("Applicant") Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By p.Item("Applicant") Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Contact_PersonLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If
    End Sub

    Protected Sub Original_ChargeLab_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles Original_ChargeLab_ImageBtn.Click

        If Me.Original_ChargeLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png" Then
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDecimal(
            Not IsDBNull(p.Item("QUOTE"))) Ascending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)

            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Original_ChargeLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_asc_.png"
        Else
            Dim dvRMA_tmp As New DataTable
            dvRMA_tmp = Session("Client_Status_List")
            Dim DataTableNew As DataTable = New DataTable
            DataTableNew = dvRMA_tmp.Clone
            Dim orderedby = From p As DataRow In dvRMA_tmp.Rows Order By Convert.ToDecimal(
            Not IsDBNull(p.Item("QUOTE"))) Descending
            orderedby.CopyToDataTable(DataTableNew, LoadOption.OverwriteChanges)
            Session("Client_Status_List") = DataTableNew
            Me.UI_dvRMAListView.DataSource = DataTableNew
            Me.UI_dvRMAListView.DataBind()
            Me.Original_ChargeLab_ImageBtn.ImageUrl = "../CipherPG/Content/DataTables/images/sort_desc_.png"
        End If

    End Sub

#End Region

End Class

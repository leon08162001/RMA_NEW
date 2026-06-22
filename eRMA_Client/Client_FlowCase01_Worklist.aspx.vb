Imports System.Data
Imports System.Data.OracleClient
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports DataService
Imports DefLanguage
Imports Newtonsoft.Json


Partial Class Client_FlowCase01_Worklist
    Inherits System.Web.UI.Page

    Private Shared ReadOnly client As Net.Http.HttpClient = New Net.Http.HttpClient()

    Public open_window As String
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _ClientID As String = ""
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    'For Paypal
    Dim _Paypal_CLIENT_ID = ConfigurationSettings.AppSettings("CLIENT_ID")
    Dim _Paypal_CLIENT_SECRET = ConfigurationSettings.AppSettings("CLIENT_SECRET")
    Dim _Paypal_Token_Url = ConfigurationSettings.AppSettings("Paypal_Token_Url")
    Dim _Paypal_CreatePayment_Url = ConfigurationSettings.AppSettings("Paypal_CreatePayment_Url")
    Dim _Paypal_Return_Url = ConfigurationSettings.AppSettings("Paypal_Return_Url")
    Dim _Paypal_Cancel_Url = ConfigurationSettings.AppSettings("Paypal_Cancel_Url")

    Dim _ReportToPDF As String = "ClientQuotation_" & oCommon.GetRandomizeNum() & ".pdf"
    Private myExportOptions As ExportOptions
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Private ReportDoc As New ReportDocument

#Region "Class 顯示詳細資料"
    Public Class Client_FlowCase01_Worklist_Item_New_Json

        Private sRMANO As String
        Public Property RMANO() As String
            Get
                Return sRMANO
            End Get
            Set(ByVal value As String)
                sRMANO = value
            End Set
        End Property

        Private sRMADID As String
        Public Property RMADID() As String
            Get
                Return sRMADID
            End Get
            Set(ByVal value As String)
                sRMADID = value
            End Set
        End Property

        Private sRMA_ID As String
        Public Property RMA_ID() As String
            Get
                Return sRMA_ID
            End Get
            Set(ByVal value As String)
                sRMA_ID = value
            End Set
        End Property

        Private sRMAD_STATUS As String
        Public Property RMAD_STATUS() As String
            Get
                Return sRMAD_STATUS
            End Get
            Set(ByVal value As String)
                sRMAD_STATUS = value
            End Set
        End Property

    End Class
#End Region

#Region "產生 維修報價 檔案"
    Private Sub qryRepairQuotaionRPT(ByVal RMA_NO As String)

        Dim RMA_AHEAD As DataTable = PreparePayTable()

        Dim sAmt As Double = 0
        Dim item_Accept As String = ""
        Dim item_Reject As String = ""

        If 1 = 1 Then

            'Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable

            Dim item_Accept_id As String = ""
            Dim item_Reject_id As String = ""
            Dim CurrencyNO As String = "USD"

            Dim ctlRMA As New ctlRMA.Repair_Quoting

            'Me.ucMessage.showMessageByFailed("Not yet open!")
            'Throw New Exception("Not yet open!")
            'Return

            '結算費用
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANO")
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    Dim dr As DataRow = RMA_AHEAD.NewRow()
                    dr("RMA_NO") = Me.RMA_NO
                    dr("RMAD_ID") = UI_RMADID.Text.Trim()
                    dr("RMA_SERIALNO") = UI_SERIALNO.Text.Trim()

                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        dr("RMA_ACCEPT") = 1
                        dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        item_Accept += UI_SERIALNO.Text.Trim() + ","
                        item_Accept_id += UI_RMADID.Text.Trim() + ","
                    Else
                        If UI_Check_Reject.Checked = True Then
                            dr("RMA_ACCEPT") = 2
                            dr("RMA_AMT") = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            sAmt += Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            'If item_Reject.Trim() <> "" Then
                            '    item_Reject = item_Reject & ","
                            'End If
                            item_Reject = item_Reject & UI_SERIALNO.Text.Trim() + ","
                            item_Reject_id = item_Reject_id & UI_RMADID.Text.Trim() + ","

                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                            dr("RMA_ACCEPT") = 1
                            dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                            item_Accept_id += UI_RMADID.Text.Trim() + ","
                        End If
                    End If

                    dr("USER_ID") = Session("_UserID")
                    dr("USER_NAME") = Session("_UserName")
                    RMA_AHEAD.Rows.Add(dr)
                End If
            Next


        End If

        If 1 = 1 Then

            Dim i As Integer = 0
            Dim ctlClient As New ctlRMA.Client

            Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
            Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
            Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable
            Dim sRMANO As String = RMA_NO

            dtClient_SalesQuoted_Head = ctlClient.QryClient_SalesQuoted_Head(sRMANO)

            dtClient_SalesQuoted_Head(0).RMASQ_QUOTE = sAmt.ToString()
            dtClient_SalesQuoted_Head(0).RMACQ_CHARGEQUOTE = sAmt.ToString()

            If item_Accept = "" Then
                dtClient_SalesQuoted_Head(0).ACCEPTCOUNT = 0
            Else
                dtClient_SalesQuoted_Head(0).ACCEPTCOUNT = (item_Accept.Split(",").Length - 1)
            End If

            If item_Reject = "" Then
                dtClient_SalesQuoted_Head(0).REJECTCOUNT = 0
            Else
                dtClient_SalesQuoted_Head(0).REJECTCOUNT = (item_Reject.Split(",").Length - 1)
            End If

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

                    End If
                End If
                drSN.RMARQ_ACCEPT_TEXT = RMARQ_Reject
                If item_Accept = "" Then

                Else
                    Dim strarr() As String
                    strarr = item_Accept.Split(",")
                    For a = 0 To strarr.Length - 1
                        If drSN.RMAD_SERIALNO = strarr(a) Then
                            RMARQ_Reject = "Y"
                        End If
                    Next
                End If

                If RMARQ_Reject = "Y" Then

                Else
                    drSN.RMASQ_QUOTE = drSN.RMASQ_LABORCOST


                    'For t = 0 To dtClient_SalesQuoted_Part.Rows.Count - 1

                    '    If dtClient_SalesQuoted_Part.Rows(t)("RMAD_ID") = drSN.RMAD_ID.Trim() Then
                    '        dtClient_SalesQuoted_Part.Rows(t).Delete()
                    '    End If

                    'Next

                End If


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

            Dim oLoginInfo As New ctlLoginInfo
            Dim LanguageID As String = oLoginInfo.getLanguageIDRMANO("Customer", sRMANO)

            Call Print(oDsReport, sRMA_COMPNO, LanguageID)

        End If

    End Sub

    Private Sub Print(ByVal oDsReport As DataSet, ByVal sRMA_COMPNO As String, ByVal sLanguageID As String)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
            ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt"))
        Else
            If sLanguageID = "003" Then
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

        ReportDoc.SetDataSource(oDsReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF
        'ExportSetup()
        'ConfigureExportToPdf()
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

    'Public Sub ConfigureExportToPdf()
    '    Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = Me.ViewState("_AttachmentFile").ToString().Trim()
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()
    'End Sub

#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then


            Session("_dtRMAClient_FlowCase01") = Nothing

            Dim oRequested As New ctlRMA.Requested

            Me.ViewState("_RMANo") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            If Request.QueryString.AllKeys.Length > 0 Then
                If Request.QueryString("id") <> Nothing Then
                    Dim sTransID = Request.QueryString("id").ToString()
                    Dim Repairdt As DataTable = oRequested.GetRepairData(sTransID)
                    Dim dvRepairdt As DataView = Repairdt.DefaultView
                    Dim item_Accept As String = ""
                    Dim item_Reject As String = ""
                    Dim MailRMA_NO As String = ""
                    dvRepairdt.RowFilter = "RMA_ACCEPT = 1"
                    For i = 0 To dvRepairdt.Count - 1
                        item_Accept += dvRepairdt(i).Item("RMA_SERIALNO").ToString.Trim() + ","
                        MailRMA_NO = dvRepairdt(i).Item("RMA_NO").ToString.Trim()
                    Next
                    dvRepairdt.RowFilter = "RMA_ACCEPT = 2"
                    For i = 0 To dvRepairdt.Count - 1
                        item_Reject += dvRepairdt(i).Item("RMA_SERIALNO").ToString.Trim() + ","
                        MailRMA_NO = dvRepairdt(i).Item("RMA_NO").ToString.Trim()
                    Next
                    If item_Accept.Length > 0 Then
                        item_Accept = Mid(item_Accept, 1, item_Accept.Length - 1)
                    End If
                    If item_Reject.Length > 0 Then
                        item_Reject = Mid(item_Reject, 1, item_Reject.Length - 1)
                    End If
                    If Repairdt.Rows.Count > 0 Then
                        Dim isSendMail As Boolean = SendMail(MailRMA_NO.Trim(), item_Accept.Trim(), item_Reject.Trim())
                    End If
                End If

            End If

            Call clearFiled()
            Call setDefault()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")

                Me.RMA_NO = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.RMA_ID = UI_lblPreviousPage_RMAID.Text.Trim()

            End If

            Call QueryData_workList()
            Call showRequestDetail()

            'EndUser Paypal 付款
            Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
            If (dt.Rows.Count > 0) Then
                UI_cmdConfirm.Visible = False
                UI_cmdPaypal.Visible = True
                UI_client_cmdPaypal.Visible = False
                UI_cmdBankTransfer.Visible = True
            Else
                UI_cmdConfirm.Visible = True
                UI_cmdPaypal.Visible = False
                UI_client_cmdPaypal.Visible = False
                UI_cmdBankTransfer.Visible = False
            End If


        End If

        If Me.UI_txtRMANo.Text.Trim() = "" Then

        End If

    End Sub
#End Region

    Public Function QueryCUSTOMER_PRODUCT_NUMBER(ByVal RMAD_RMANO As String) As String

        Dim Context As String = ""

        Dim ctAddress_List As New ctAddress
        RMAD_RMANO = ctAddress_List.Get_CUSTOMER_PRODUCT_NUMBER(RMAD_RMANO)

        If RMAD_RMANO <> "" Then
            Context = RMAD_RMANO
        End If

        Return Context

    End Function

    Private Sub clearFiled()
        Me.RMA_ID = ""
        Me.RMA_NO = ""
        Me.RMASQ_ID = ""

        Me.UI_RMANo.Text = ""
        Me.UI_RequestDate.Text = ""
        Dim RMA_COMPNO As String = ""
        Me.UI_RepairCenter.Text = ""
        Me.UI_Applicant.Text = ""

        Me.UI_Total_ServiceCharge.Text = "0"
        Me.UI_Total_MaterialCost.Text = "0"
        Me.UI_Total_TotalAmount.Text = "0"

        Me.UI_lblPreviousPage_RMANO.Text = ""
        Me.UI_lblPreviousPage_RMAID.Text = ""
        Me.UI_lblPreviousPage_RMADID.Text = ""

        Me.UIPanel_RMADetail.Visible = False
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        _ClientID = "ctl00_ContentPlaceHolder_UI_dvCustomer_ctl01_UI_CheckGroup"
        _ClientID = _ClientID & "," & Me.UI_cmdSearch.ClientID

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())
        'RMA2
        Me.lblInformation.Text = _oLanguage.getText("RMA2", "103", ctlLanguage.eumType.Tag)
        Me.ResetBtn.Text = _oLanguage.getText("RMA2", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA2", "010", ctlLanguage.eumType.Tag)
        Me.UI_txtRMANo.Attributes.Add("placeholder", _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command))
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblQuickTittle.Text = _oLanguage.getText("RMA", "171", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdPreview.Text = _oLanguage.getText("Common", "414", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)

        Me.UI_dvCustomer.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(3).HeaderText = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(4).HeaderText = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        'Me.UI_dvCustomer.Columns(5).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag)

        Me.UI_dvCustomer.Columns(5).HeaderText = _oLanguage.getText("RMA2", "070", ctlLanguage.eumType.Tag)

        Me.UI_dvCustomer.Columns(6).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag).Replace("RMA", "")

        Me.UI_dvCustomer.Columns(7).HeaderText = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(8).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        'Me.UI_dvCustomer.Columns(9).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(9).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvCustomer.Columns(10).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle_Detail.Text = _oLanguage.getText("Transfer", "032", ctlLanguage.eumType.Word)

        Session("_isShowNotFound") = _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag)

        Dim dtRMAClient As New DataTable
        dtRMAClient.Columns.Add("SeqNo")
        dtRMAClient.Columns.Add("RMA_NO")
        dtRMAClient.Columns.Add("RMA_ID")
        dtRMAClient.Columns.Add("RequestDate")
        dtRMAClient.Columns.Add("Applicant")
        dtRMAClient.Columns.Add("CurrencyCode")
        dtRMAClient.Columns.Add("QUOTE")

        '20230817
        dtRMAClient.Columns.Add("Net_Charge_QUOTE")

        dtRMAClient.Columns.Add("RMAD_STATUS")
        dtRMAClient.Columns.Add("RequestQty")
        dtRMAClient.Columns.Add("ShippedQty")
        dtRMAClient.Columns.Add("Remark")
        dtRMAClient.Columns.Add("RMAD_ID")
        dtRMAClient.Columns.Add("RMASQ_ID")
        Session("_dtRMAClient_FlowCase01") = dtRMAClient

        Me.QuotationLabel.Text = _oLanguage.getText("RMA2", "036", ctlLanguage.eumType.Tag)
        Me.Quotation_titile_Label.Text = _oLanguage.getText("RMA2", "037", ctlLanguage.eumType.Tag)

        '20231222
        Me.UI_cboStatus.Items.Clear()

        Dim myUI_cboStatus_001 As New ListItem
        myUI_cboStatus_001.Value = 0
        myUI_cboStatus_001.Text = _oLanguage.getText("RMA2", "064", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items.Add(myUI_cboStatus_001)

        Dim myUI_cboStatus_002 As New ListItem
        myUI_cboStatus_002.Value = 1
        myUI_cboStatus_002.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items.Add(myUI_cboStatus_002)


        Dim myUI_cboStatus_003 As New ListItem
        myUI_cboStatus_003.Value = 2
        myUI_cboStatus_003.Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items.Add(myUI_cboStatus_003)

        'paypal
        html_Failed.Text = _oLanguage.getText("RMA2", "121", ctlLanguage.eumType.Tag)
        Me.lblTitleMsg.Text = _oLanguage.getText("Common", "013", ctlLanguage.eumType.Tag)
        Me.UI_butClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_butAlert.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)


    End Sub

    Protected Sub UI_butClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butClose.Click

        Dim sScript As String = ""
        sScript = sScript & "<script language=""javascript"">"
        sScript = sScript & "window.open('" & Me.ViewState("_AttachmentFile").ToString() & "','','');"
        sScript = sScript & "</script>"
        Response.Write(sScript)

    End Sub

    Private Sub QueryData_workList()
        Dim oClient As New ctlRMA.Client
        Dim dtClientList As New RmaDTO.vwClient_WorkListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = ""
        Dim sSerialNo As String = ""

        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim sCustomerName As String = ""
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        'Session("_CustomerID")-->客戶編號
        If Me.UI_cboStatus.SelectedItem.Value = 0 Then
            dtClientList = oClient.QueryByWorkProductAll(Session("_CustomerID").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerName, fdate, edate)
        End If

        If Me.UI_cboStatus.SelectedItem.Value = 1 Then
            dtClientList = oClient.QueryByWork(Session("_CustomerID").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerName, fdate, edate)
        End If


        If Me.UI_cboStatus.SelectedItem.Value = 2 Then
            dtClientList = oClient.QueryByWorkCUSTOMER_PRODUCT_NUMBER(Session("_CustomerID").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerName, fdate, edate)
        End If
        '=======================================================================================================================================
        '無資料時, UI 的控制
        '=======================================================================================================================================
        Me.UI_lblQuickTittle.Visible = True
        If dtClientList.Rows.Count = 0 Then
            Me.UI_lblQuickTittle.Visible = False
        End If

        Call ArrangementData_workList(dtClientList)

        Dim dtRMAClient As DataTable = Session("_dtRMAClient_FlowCase01")
        Dim dvRMAClient As DataView = dtRMAClient.DefaultView

        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvRMAClient.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvRMAClient)
    End Sub

    Private Sub ArrangementData_workList(ByVal dtClientList As RmaDTO.vwClient_WorkListDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim dtRMAClient As DataTable = Session("_dtRMAClient_FlowCase01")
        dtRMAClient.Rows.Clear()
        Dim dvRMAClient As DataView = dtRMAClient.DefaultView

        For i = 0 To dtClientList.Rows.Count - 1
            Dim dr As RmaDTO.vwClient_WorkListRow = dtClientList.Rows(i)
            Dim RMA_No As String = dr.RMA_NO.ToString().Trim()

            dvRMAClient.RowFilter = "RMA_NO='" & RMA_No & "'"
            If dvRMAClient.Count = 0 Then
                Dim drTmp As DataRow = dtRMAClient.NewRow
                drTmp("RMA_NO") = RMA_No
                drTmp("RMA_ID") = dr.RMA_ID.Trim()
                drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
                drTmp("Applicant") = dr.RMA_APPLICANT.Trim()
                drTmp("RMAD_STATUS") = dr.RMAD_STATUS
                If dr.IsRMA_RemarkNull = False Then drTmp("Remark") = dr.RMA_Remark.Trim
                dtRMAClient.Rows.Add(drTmp)
            End If
        Next
        dvRMAClient.RowFilter = ""


        Dim dvClientList As DataView = dtClientList.DefaultView
        For i = 0 To dvRMAClient.Count - 1
            Dim CurrencyCode As String = ""
            Dim RMAD_ID As String = ""
            Dim RMASQ_ID As String = ""


            Dim Net_Charge_QUOTE As Double = 0

            Dim QUOTE As Double = 0
            Dim RequestQty As Integer = 0
            Dim ShippedQty As Integer = 0

            Dim RMA_No As String = dvRMAClient(i).Item("RMA_NO").ToString.Trim()
            dvClientList.RowFilter = "RMA_NO='" & RMA_No & "'"
            For j = 0 To dvClientList.Count - 1
                Dim dr As RmaDTO.vwClient_WorkListRow = dvClientList(j).Row
                If dr.IsRMASQ_CURRENCYCODENull = False Then CurrencyCode = dr.RMASQ_CURRENCYCODE.Trim()
                If dr.IsRMASQ_QUOTENull = False Then QUOTE = QUOTE + Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N")

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                If dr.RMAD_STATUS = "40" Then
                    If RMAD_ID.Trim <> "" Then RMAD_ID = RMAD_ID & ","
                    If RMASQ_ID.Trim <> "" Then RMASQ_ID = RMASQ_ID & ","
                    RMAD_ID = RMAD_ID & dr.RMAD_ID.Trim()
                    If dr.IsRMASQ_IDNull = False Then RMASQ_ID = RMASQ_ID & dr.RMASQ_ID.Trim()
                End If


            Next
            RequestQty = dvClientList.Count

            dvRMAClient(i)("CurrencyCode") = CurrencyCode
            If CurrencyCode.Trim <> "" Then
                dvRMAClient(i)("QUOTE") = QUOTE.ToString("N")
            End If

            '20230818 顯示客戶付款總金額
            If QUOTE = 0 Then
                dvRMAClient(i)("Net_Charge_QUOTE") = ""

            Else
                dvRMAClient(i)("Net_Charge_QUOTE") = QUOTE.ToString("N")
            End If



            Dim oClientes As New ctlRMA.Client
            Dim VWRPTCLIENT_SALESQUOTED_HEADDataTablees As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable

            VWRPTCLIENT_SALESQUOTED_HEADDataTablees = oClientes.QryClient_SalesQuoted_Head(dvRMAClient(i).Item("RMA_NO").ToString.Trim())





            For j = 0 To VWRPTCLIENT_SALESQUOTED_HEADDataTablees.Count - 1

                If VWRPTCLIENT_SALESQUOTED_HEADDataTablees.Rows(j)("RMACQ_CHARGEQUOTE") Is Nothing Then

                Else

                    If VWRPTCLIENT_SALESQUOTED_HEADDataTablees.Rows(j)("RMACQ_CHARGEQUOTE").ToString().Trim() <> "" Then
                        dvRMAClient(i)("Net_Charge_QUOTE") = VWRPTCLIENT_SALESQUOTED_HEADDataTablees.Rows(j)("RMACQ_CHARGEQUOTE")
                    End If

                End If
            Next


            '20230818 顯示客戶付款總金額

            dvRMAClient(i)("RequestQty") = RequestQty
            dvRMAClient(i)("ShippedQty") = ShippedQty
            dvRMAClient(i)("RMAD_ID") = RMAD_ID
            dvRMAClient(i)("RMASQ_ID") = RMASQ_ID
        Next
        dvRMAClient.RowFilter = ""

        Session("_dtRMAClient_FlowCase01") = dtRMAClient
    End Sub

    Private Sub RMA_DataBind(ByVal dvDetail As DataView)
        Dim i As Integer = 0

        For i = 0 To dvDetail.Count - 1
            dvDetail(i)("SeqNo") = (i + 1).ToString()
        Next

        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())
        Me.Count_Lab_.Text = dvDetail.Count.ToString()
        Me.UI_dvCustomer.DataSource = dvDetail
        Me.UI_dvCustomer.DataBind()
    End Sub

    Protected Sub UI_dvCustomer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvCustomer.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMAID As Label = e.Row.FindControl("UI_RMAID")
            Dim UI_RMASQID As Label = e.Row.FindControl("UI_RMASQID")

            Dim UI_RMASTATUS As Label = e.Row.FindControl("UI_RMASTATUS")
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")

            Dim UI_imgDetail As Button = e.Row.FindControl("UI_imgDetail")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_imgDetail.Text = _oLanguage.getText("RMA2", "036", ctlLanguage.eumType.Tag)
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_imgDetail.Visible = False
            UI_cmdEdit.Visible = False
            If UI_RMAID.Text.Trim() = Me.RMA_ID Then
                Me.RMASQ_ID = UI_RMASQID.Text.Trim()
                e.Row.Style("background-color") = "Pink"
            End If

            If Convert.ToInt16(UI_RMASTATUS.Text.Trim) = 40 Then
                UI_imgDetail.Visible = True
                UI_Status.Text = _oLanguage.getText("RMA2", "107", ctlLanguage.eumType.Tag)
            End If
            If Convert.ToInt16(UI_RMASTATUS.Text.Trim) < 20 Then
                UI_cmdEdit.Visible = True
                UI_Status.Text = _oLanguage.getText("RMA2", "106", ctlLanguage.eumType.Tag)
            End If

            If e.Row.RowIndex Mod 2 = 0 Then
                e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255)
            Else
                e.Row.BackColor = System.Drawing.Color.FromArgb(244, 244, 246)
            End If

        End If

    End Sub

    Protected Sub UI_dvCustomer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvCustomer.RowCommand
        Dim i As Integer = 0

        If e.CommandName = "cmdDetail" Then
            For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
                Dim Gridrow As GridViewRow = Me.UI_dvCustomer.Rows(i)
                Gridrow.Style("background-color") = ""
            Next

            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvCustomer.Rows(iIndex)
            row.Style("background-color") = ""

            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMASQID As Label = row.FindControl("UI_RMASQID")

            Me.RMA_ID = UI_RMAID.Text.Trim.Trim()
            Me.RMA_NO = UI_RMANO.Text.Trim()
            Me.RMASQ_ID = UI_RMASQID.Text.Trim()

            Call showRequestDetail()
            Me.ajModalProgress.Show()
        End If

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvCustomer.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()

            Dim theHeight As Integer = 0
            If (Request.Cookies("windowhigh") IsNot Nothing) Then

                theHeight = Convert.ToInt32(Request.Cookies("windowhigh").Value)
            End If

            If theHeight >= 768 Then
                theHeight = 768

            Else
                theHeight = theHeight - 25
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

            Session("_eumCommand") = 2

            Me.windowLab.Text = " <iframe id='ProductInformation_03_iframe' src='ProductInformation_03.aspx?RMANO=" & UI_RMANO.Text & "&eumCommand=UPDATE'  style='width:" & theWidth & "px;height:" & theHeight & "px;border-radius: 10px;border:none;' ></iframe>   "
            Me.UI_Up_RMA_panel_ModalPopupExtender.Show()
        End If

    End Sub

    Protected Sub UI_dvCustomer_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvCustomer.Sorting

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

        If IsNothing(Session("_dtRMAClient_FlowCase01")) = False Then
            Dim dtDetail As DataTable = Session("_dtRMAClient_FlowCase01")
            Dim dvDetail As DataView = dtDetail.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvDetail)
        End If
    End Sub

#Region "RMA Detail"
    Private Sub showRequestDetail()
        Session("_dtClientDetail_FlowCase01") = Nothing

        Call setControls_Detail()
        Call QueryDataHead()
        Call QueryDataByDetail(0)

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls_Detail()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo_Detail.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate_Detail.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        'Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        'Me.UI_lblRequestedTittle_Detail.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryDataHead()
        Dim oRMA As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oCompany As New ctlCompany

        dtRMA = oRMA.QueryByRMAHead(Me.RMA_NO)
        If dtRMA.Rows.Count > 0 Then
            Me.UIPanel_RMADetail.Visible = True

            Dim dr As RmaDTO.RMARow = dtRMA.Rows(0)
            Me.UI_RMANo.Text = dr.RMA_NO.Trim()
            Me.UI_RequestDate.Text = dr.RMA_CSTMP.ToShortDateString()

            Dim RMA_COMPNO As String = dr.RMA_COMPNO.Trim
            Me.UI_RepairCenter.Text = oCompany.getRepairName(RMA_COMPNO)

            Me.UI_Applicant.Text = dr.RMA_APPLICANT.Trim()
            'If dr.IsRMA_RemarkNull = False Then Me.UI_Remark.Text = dr.RMA_Remark
        End If
    End Sub

    Private Sub QueryDataByDetail(ByVal iPageIndex As Integer)
        Me.UI_Total_ServiceCharge.Text = "0"
        Me.UI_Total_MaterialCost.Text = "0"
        Me.UI_Total_TotalAmount.Text = "0"

        Dim oRMA As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
        dtClientDetail = oRMA.QueryByClient(Session("_LanguageID").ToString().Trim(), Me.RMA_NO)

        Call RMADetail_DataBind(dtClientDetail, iPageIndex)
    End Sub

    Private Sub RMADetail_DataBind(ByVal dtClientDetail As RmaDTO.tmpClientDetailDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData_Detail(dtClientDetail)

        Dim dvClientDetail As DataView = dtClientDetail.DefaultView
        dvClientDetail.Sort = "RMAD_SERIALNO"
        Session("_dtClientDetail_FlowCase01") = dtClientDetail

        'Me.UI_dvRequestDetail.PageSize = _PageSize
        'Me.UI_dvRequestDetail.PageIndex = iPageIndex
        Me.UI_dvRequestDetail.DataSource = dvClientDetail
        Me.UI_dvRequestDetail.DataBind()
    End Sub

    Private Sub ArrangementData_Detail(ByVal dtClientDetail As RmaDTO.tmpClientDetailDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""

        If dtClientDetail.Columns("SeqID") Is Nothing Then
            dtClientDetail.Columns.Add("SeqID")
            dtClientDetail.Columns.Add("Status")
            dtClientDetail.Columns.Add("WARRANTY")
            dtClientDetail.Columns.Add("IMPROPERUSAGE")
            dtClientDetail.Columns.Add("FailureReason")
            dtClientDetail.Columns.Add("LaborCost")
            dtClientDetail.Columns.Add("MaterialCost")
            dtClientDetail.Columns.Add("TotalAmount")
        End If

        For i = 0 To dtClientDetail.Rows.Count - 1
            Dim dr As RmaDTO.tmpClientDetailRow = dtClientDetail.Rows(i)

            dtClientDetail.Rows(i)("SeqID") = i + 1
            dtClientDetail.Rows(i)("Status") = oCommon.ConvertToItemStatusText(Convert.ToInt16(dr.RMAD_STATUS), dr.RMAD_ID.Trim())

            'If dtClientDetail.Rows(i)("RMAD_WARRANTY").ToString().Trim() = "" Then
            '    dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            '    'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            '    Select Case dtClientDetail.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
            '        Case "0"
            '            dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            '        Case "1"
            '            dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            '    End Select

            'Else
            '    dtClientDetail.Rows(i)("WARRANTY") = Convert.ToDateTime(dr.RMAD_WARRANTY).ToShortDateString()
            'End If

            '2021/05/06 轉換Model
            sModelNo = oExport.getMModelNo(dtClientDetail.Rows(i)("RMAD_MODELNO").ToString().Trim(), dr.RMARQ_COMPNO.ToString().Trim(), dr.RMA_ACCOUNTID.ToString().Trim())

            If sModelNo.Trim() <> "" Then
                dtClientDetail.Rows(i)("RMAD_MODELNO") = sModelNo.Trim()
            End If


            dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            Select Case dtClientDetail.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                Case "0"
                    dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                Case "1"
                    dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End Select


            '是否人為因素
            dtClientDetail.Rows(i)("IMPROPERUSAGE") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            Select Case dtClientDetail.Rows(i)("RMARQ_IMPROPERUSAGE").ToString().Trim()
                Case "0"
                    dtClientDetail.Rows(i)("IMPROPERUSAGE") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                Case "1"
                    dtClientDetail.Rows(i)("IMPROPERUSAGE") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End Select



            If dr.IsFARC_NAME2Null = False Then
                dtClientDetail.Rows(i)("FailureReason") = dr.FARC_NAME2.ToString().Trim()
            Else
                If dr.IsFARC_NAME1Null = False Then dtClientDetail.Rows(i)("FailureReason") = dr.FARC_NAME1.ToString().Trim()
            End If


            If dr.IsRMARSD_QUOTENull = False Then
                '幣別
                If dr.IsRMASQ_CURRENCYCODENull = False Then
                    Dim sCurrencyCode As String = ""
                    'Dim sCurrencyCode As String = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "

                    'Labor Cost
                    If dr.IsRMARSD_LABORCOSTNull = False Then dtClientDetail.Rows(i)("LaborCost") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_LABORCOST).ToString("N").Trim()
                    'Material Cost
                    If dr.IsRMARSD_MATERIALCOSTNull = False Then dtClientDetail.Rows(i)("MaterialCost") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_MATERIALCOST).ToString("N").Trim()
                    'Total Amount
                    dtClientDetail.Rows(i)("TotalAmount") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_QUOTE).ToString("N").Trim()
                End If
            Else
                If dr.IsRMASQ_QUOTENull = False Then
                    '幣別
                    If dr.IsRMASQ_CURRENCYCODENull = False Then
                        Dim sCurrencyCode As String = ""
                        'Dim sCurrencyCode As String = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "

                        'Labor Cost
                        If dr.IsRMASQ_LABORCOSTNull = False Then dtClientDetail.Rows(i)("LaborCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_LABORCOST).ToString("N").Trim()
                        'Material Cost
                        If dr.IsRMASQ_MATERIALCOSTNull = False Then dtClientDetail.Rows(i)("MaterialCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_MATERIALCOST).ToString("N").Trim()
                        'Total Amount
                        dtClientDetail.Rows(i)("TotalAmount") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
                    End If
                End If

            End If
        Next

    End Sub

    Protected Sub UI_dvRequestDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequestDetail.RowDataBound

        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            Dim UI_lblAccept As Label = e.Row.FindControl("UI_lblAccept")
            Dim UI_lblReject As Label = e.Row.FindControl("UI_lblReject")

            UI_lblAccept.Text = _oLanguage.getText("Transfer", "034", ctlLanguage.eumType.Word)
            UI_lblReject.Text = _oLanguage.getText("Transfer", "035", ctlLanguage.eumType.Word)

            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA2", "038", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

            'e.Row.Cells(8).Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
            'e.Row.Cells(9).Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA2", "039", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "040", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
            e.Row.Cells(11).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

            e.Row.Cells(12).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            Dim UI_RMAD_STATUS As Label = e.Row.FindControl("UI_RMAD_STATUS")

            If UI_RMADWARRANTY.Text.Trim() <> "" Then
                Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                Dim RMAD_CSTMP As DateTime = Convert.ToDateTime(UI_RMADCSTMP.Text.Trim())
                If RMAD_WARRANTY < RMAD_CSTMP Then
                    'e.Row.Cells(3).ForeColor = Drawing.Color.Red
                End If
            End If


            'Accept:
            '1. in Warranty 為 "Y", 無checkbox, 但畫面是勾選的, 為必修
            '2. in Warranty 為 "N", 畫面有checkbox, 讓客戶決定是否要修
            Dim UI_RMAD_ISWARRANTY As Label = e.Row.FindControl("UI_RMAD_ISWARRANTY")
            Dim UI_RMARQ_IMPROPERUSAGE As Label = e.Row.FindControl("UI_RMARQ_IMPROPERUSAGE")

            Dim UI_Check_Accept As CheckBox = e.Row.FindControl("UI_Check_Accept")
            Dim UI_Check_Reject As CheckBox = e.Row.FindControl("UI_Check_Reject")
            Dim UI_Accept As Label = e.Row.FindControl("UI_Accept")
            Dim UI_Reject As Label = e.Row.FindControl("UI_Reject")

            UI_Check_Accept.Checked = True
            UI_Check_Reject.Checked = False

            UI_Accept.Visible = False
            UI_Check_Accept.Visible = True
            If UI_RMAD_ISWARRANTY.Text = "1" Then
                UI_Accept.Visible = True
                UI_Check_Accept.Visible = False
                UI_Check_Reject.Visible = False
                '增加若同意時，直接設為勾選
                UI_Check_Accept.Checked = True
            End If
            If UI_RMAD_ISWARRANTY.Text = "1" And UI_RMARQ_IMPROPERUSAGE.Text = "1" Then
                UI_Check_Reject.Visible = True
                UI_Check_Accept.Visible = True
                UI_Check_Accept.Checked = False
                UI_Accept.Visible = False
            End If


            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            If UI_RMAD_STATUS.Text.Trim() = "60" Then
                UI_Accept.Visible = True
                UI_Check_Accept.Visible = False
                UI_Check_Reject.Visible = False
            End If

            Dim UI_ServiceCharge As Label = e.Row.FindControl("UI_ServiceCharge")
            Dim UI_MaterialCost As Label = e.Row.FindControl("UI_MaterialCost")
            Dim UI_TotalAmount As Label = e.Row.FindControl("UI_TotalAmount")

            If Me.UI_Total_ServiceCharge.Text.Trim() <> "" And UI_ServiceCharge.Text.Trim() <> "" Then
                Me.UI_Total_ServiceCharge.Text = (Convert.ToDecimal(Me.UI_Total_ServiceCharge.Text) + Convert.ToDecimal(UI_ServiceCharge.Text)).ToString()
            End If

            If Me.UI_Total_MaterialCost.Text.Trim() <> "" And UI_MaterialCost.Text.Trim() <> "" Then
                Me.UI_Total_MaterialCost.Text = (Convert.ToDecimal(Me.UI_Total_MaterialCost.Text) + Convert.ToDecimal(UI_MaterialCost.Text)).ToString()
            End If

            If Me.UI_Total_TotalAmount.Text <> "" And UI_TotalAmount.Text.Trim() <> "" Then
                Me.UI_Total_TotalAmount.Text = (Convert.ToDecimal(Me.UI_Total_TotalAmount.Text) + Convert.ToDecimal(UI_TotalAmount.Text)).ToString()
            End If

            '20201029 wisely add  保固內若有要收費 讓使用者選擇Accept Or Reject
            If UI_TotalAmount.Text <> "" Then

                If UI_RMAD_ISWARRANTY.Text = "1" And Convert.ToDecimal(UI_TotalAmount.Text) > 0 Then
                    UI_Accept.Visible = False
                    UI_Check_Accept.Visible = True
                    UI_Check_Reject.Visible = True
                    '增加若同意時，直接設為勾選
                    UI_Check_Accept.Checked = True
                End If
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMANO As Label = e.Row.FindControl("UI_RMANO")
            Dim UI_SERIALNO As Label = e.Row.FindControl("UI_SERIALNO")
            Dim UI_Test_Report As Button = e.Row.FindControl("UI_Test_Report")

            Dim myctAddress As New ctAddress
            Dim myDataTable As DataTable = myctAddress.Select_Count_CheckingReport(UI_RMANO.Text.Trim(), UI_SERIALNO.Text.Trim())

            If myDataTable.Rows(0)("A") Is Nothing Then

            Else

                If Convert.ToInt32(myDataTable.Rows(0)("A").ToString().Trim()) > 0 Then
                    UI_Test_Report.Visible = True
                Else
                    UI_Test_Report.Visible = False
                End If

            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMANO As Button = e.Row.FindControl("UI_cmdDetail")
            UI_RMANO.Text = _oLanguage.getText("RMA2", "043", ctlLanguage.eumType.Tag)

            Dim UI_Test_Report As Button = e.Row.FindControl("UI_Test_Report")
            UI_Test_Report.Text = _oLanguage.getText("RMA2", "943", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim UI_Footer_ServiceCharge As Label = e.Row.FindControl("UI_Footer_ServiceCharge")
            Dim UI_Footer_MaterialCost As Label = e.Row.FindControl("UI_Footer_MaterialCost")
            Dim UI_Footer_TotalAmount As Label = e.Row.FindControl("UI_Footer_TotalAmount")

            '算總價格
            'UI_Footer_ServiceCharge.Text = Convert.ToDecimal(Me.UI_Total_ServiceCharge.Text).ToString("N")
            'UI_Footer_MaterialCost.Text = Convert.ToDecimal(Me.UI_Total_MaterialCost.Text).ToString("N")
            'UI_Footer_TotalAmount.Text = Convert.ToDecimal(Me.UI_Total_TotalAmount.Text).ToString("N")

            Me.ServiceChargeLabel.Text = _oLanguage.getText("RMA2", "044", ctlLanguage.eumType.Tag) & ":"
            Me.UI_Total_ServiceCharge.Text = Convert.ToDecimal(Me.UI_Total_ServiceCharge.Text).ToString("N")
            Me.MaterialLabel.Text = _oLanguage.getText("RMA2", "040", ctlLanguage.eumType.Tag) & ":"

            Me.UI_Total_MaterialCost.Text = Convert.ToDecimal(Me.UI_Total_MaterialCost.Text).ToString("N")
            Me.TotalAmount.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag) & ":" & Convert.ToDecimal(Me.UI_Total_TotalAmount.Text).ToString("N")
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

        '價錢如果為0不能取消維修 開始
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim UI_TotalAmount_Agree_ As Label = e.Row.FindControl("UI_TotalAmount_Agree")
            Dim UI_Check_Accept As CheckBox = e.Row.FindControl("UI_Check_Accept")
            Dim UI_Check_Reject As CheckBox = e.Row.FindControl("UI_Check_Reject")
            Dim UI_Accept As Label = e.Row.FindControl("UI_Accept")
            Dim UI_Reject As Label = e.Row.FindControl("UI_Reject")


            If UI_TotalAmount_Agree_.Text = "0.00" Then
                UI_Accept.Visible = True
                UI_Check_Accept.Visible = False
                UI_Check_Reject.Visible = False
            End If

        End If
        '價錢如果為0不能取消維修 結束
    End Sub

    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2011/08/04 START</remarks>
    Protected Sub UI_checkGroup_Accept_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Dim check As Integer = 0
            Dim UI_CheckGroup_Accept As CheckBox = sender.parent.FindControl("UI_CheckGroup_Accept")
            Dim UI_CheckGroup_Reject As CheckBox = sender.parent.FindControl("UI_CheckGroup_Reject")
            UI_CheckGroup_Reject.Checked = False

            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                If UI_Accept_.Visible = True Then
                    check = check + 1
                End If

            Next

            If check = Me.UI_dvRequestDetail.Rows.Count Then



            Else

                Dim i As Integer = 0
                Dim UI_Total_MaterialCost_text As Double = 0

                For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                    If Me.UI_dvRequestDetail.Rows(i).RowType = DataControlRowType.DataRow Then
                        Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                        Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                        Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
                        Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
                        Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")
                        Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
                        Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
                        Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")
                        Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")

                        If UI_Accept_.Visible = False Then
                            UI_Check_Accept.Checked = sender.Checked
                            If UI_Check_Accept.Checked = False Then
                                UI_Check_Reject.Checked = True
                            Else
                                UI_Check_Reject.Checked = False
                            End If

                            If UI_Check_Accept.Checked = True Then

                                '變換顯示 取消 開始
                                UI_TotalAmount_Agree.Visible = True
                                UI_TotalAmount.Visible = False
                                UI_TotalAmount_Cancel.Visible = False
                                UI_MaterialCost_Agree.Visible = True
                                UI_MaterialCost.Visible = False
                                UI_MaterialCost_Cancel.Visible = False
                                '變換顯示 取消 結束

                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())

                                End If

                            Else

                                '變換顯示 取消 開始
                                UI_TotalAmount_Agree.Visible = False
                                UI_TotalAmount.Visible = True
                                UI_TotalAmount_Cancel.Visible = True
                                UI_MaterialCost_Agree.Visible = False
                                UI_MaterialCost.Visible = True
                                UI_MaterialCost_Cancel.Visible = True

                                If UI_MaterialCost_Cancel.Text.Trim() = "" Then
                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                Else
                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())

                                End If


                                '變換顯示 取消 結束

                            End If

                        Else


                            If UI_Check_Accept.Checked = True Then



                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())

                                End If

                            Else



                                If UI_MaterialCost_Cancel.Text.Trim() = "" Then
                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                Else
                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())

                                End If



                            End If

                        End If
                    End If




                Next
                Me.UI_Total_MaterialCost.Text = UI_Total_MaterialCost_text.ToString()

                Call GetTotalAmount()

            End If

        Catch ex As Exception


        Finally

        End Try

        Me.ajModalProgress.Show()

    End Sub

    Protected Sub UI_check_Accept_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0

        Dim UI_Check_Accept As CheckBox = sender.parent.FindControl("UI_Check_Accept")
        Dim UI_Check_Reject As CheckBox = sender.parent.FindControl("UI_Check_Reject")
        If sender.Checked = True Then
            UI_Check_Reject.Checked = False
        End If

        If UI_Check_Accept.Checked = False Then
            'UI_Check_Reject.Checked = True
        End If

        Dim UI_Total_MaterialCost_text As Double = 0

        Dim UI_CheckGroup_Accept As CheckBox = Me.UI_dvRequestDetail.HeaderRow.FindControl("UI_CheckGroup_Accept")
        UI_CheckGroup_Accept.Checked = False
        Dim UI_CheckGroup_Reject As CheckBox = Me.UI_dvRequestDetail.HeaderRow.FindControl("UI_CheckGroup_Reject")
        UI_CheckGroup_Reject.Checked = False

        For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1

            If Me.UI_dvRequestDetail.Rows(i).RowType = DataControlRowType.DataRow Then

                Dim UI_Check_Accept_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Check_Reject_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
                Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
                Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")

                Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
                Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
                Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")

                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                Dim UI_ServiceCharge_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Reject")
                Dim UI_ServiceCharge_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Cancel")
                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                If UI_Check_Accept.Text.Trim() = UI_Check_Accept_.Text.Trim() Then

                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                    UI_ServiceCharge_Agree.Visible = True
                    UI_ServiceCharge.Visible = False
                    UI_ServiceCharge_Cancel.Visible = False
                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                    '變換顯示 取消 開始
                    UI_TotalAmount_Agree.Visible = True
                    UI_TotalAmount.Visible = False
                    UI_TotalAmount_Cancel.Visible = False

                    UI_MaterialCost_Agree.Visible = True
                    UI_MaterialCost.Visible = False
                    UI_MaterialCost_Cancel.Visible = False
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())
                    '變換顯示 取消 結束
                End If

            End If
        Next
        '材料費總和
        For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
            Dim UI_Check_Accept_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
            Dim UI_Check_Reject_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

            Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
            Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
            Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")

            Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
            Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
            Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")
            If UI_MaterialCost_Cancel.Visible = True Then


                UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())


            Else

                If UI_MaterialCost_Agree.Text.Trim() = "" Then
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                Else
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())
                End If


            End If
        Next
        Me.UI_Total_MaterialCost.Text = UI_Total_MaterialCost_text.ToString()
        Call GetTotalAmount()
        Me.ajModalProgress.Show()
    End Sub
    Protected Sub UI_checkGroup_Reject_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            Dim check As Integer = 0
            Dim UI_CheckGroup_Accept As CheckBox = sender.parent.FindControl("UI_CheckGroup_Accept")
            Dim UI_CheckGroup_Reject As CheckBox = sender.parent.FindControl("UI_CheckGroup_Reject")
            UI_CheckGroup_Accept.Checked = False


            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                If UI_Accept_.Visible = True Then
                    check = check + 1
                End If

            Next

            If check = Me.UI_dvRequestDetail.Rows.Count Then



            Else

                Dim i As Integer = 0
                Dim UI_Total_MaterialCost_text As Double = 0
                For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1

                    If Me.UI_dvRequestDetail.Rows(i).RowType = DataControlRowType.DataRow Then
                        Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                        Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                        Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
                        Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
                        Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")

                        Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
                        Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
                        Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")
                        Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                        If UI_Accept_.Visible = False Then
                            UI_Check_Reject.Checked = sender.Checked

                            If UI_Check_Reject.Checked = False Then
                                UI_Check_Accept.Checked = True
                            Else
                                UI_Check_Accept.Checked = False
                            End If

                            If UI_Check_Reject.Checked = True Then

                                '變換顯示 取消 開始
                                UI_TotalAmount_Agree.Visible = False
                                UI_TotalAmount.Visible = True
                                UI_TotalAmount_Cancel.Visible = True
                                UI_MaterialCost_Agree.Visible = False
                                UI_MaterialCost.Visible = True
                                UI_MaterialCost_Cancel.Visible = True
                                '變換顯示 取消 結束

                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_MaterialCost_Cancel.Visible = False

                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())

                                End If


                            Else

                                '變換顯示 取消 開始
                                UI_TotalAmount_Agree.Visible = True
                                UI_TotalAmount.Visible = False
                                UI_TotalAmount_Cancel.Visible = False
                                UI_MaterialCost_Agree.Visible = True
                                UI_MaterialCost.Visible = False
                                UI_MaterialCost_Cancel.Visible = False
                                '變換顯示 取消 結束
                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                    UI_MaterialCost_Cancel.Visible = False


                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())

                                End If

                            End If
                        Else
                            If UI_Check_Reject.Checked = True Then


                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_MaterialCost_Cancel.Visible = False

                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())

                                End If


                            Else


                                If UI_MaterialCost_Agree.Text.Trim() = "" Then

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                                    UI_MaterialCost_Cancel.Visible = False


                                Else

                                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())

                                End If

                            End If

                        End If





                    End If
                Next
                Me.UI_Total_MaterialCost.Text = UI_Total_MaterialCost_text.ToString()
                Call GetTotalAmount()

            End If
        Catch ex As Exception

        Finally

        End Try

        Me.ajModalProgress.Show()

    End Sub
    Protected Sub UI_check_Reject_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
        Dim dt As DataTable = TryCast(Session("_dtClientDetail_FlowCase01"), DataTable)
        Dim isDiscounted As Boolean = dt.AsEnumerable().Select(Function(r) r("CU_SERVICE_CHG_DISCOUNT") = 1).FirstOrDefault()
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
        Dim UI_Check_Accept As CheckBox = sender.parent.FindControl("UI_Check_Accept")
        Dim UI_Check_Reject As CheckBox = sender.parent.FindControl("UI_Check_Reject")
        If sender.Checked = True Then
            UI_Check_Accept.Checked = False
        End If

        If UI_Check_Reject.Checked = False Then
            'UI_Check_Accept.Checked = True
        End If

        Dim UI_CheckGroup_Accept As CheckBox = Me.UI_dvRequestDetail.HeaderRow.FindControl("UI_CheckGroup_Accept")
        UI_CheckGroup_Accept.Checked = False
        Dim UI_CheckGroup_Reject As CheckBox = Me.UI_dvRequestDetail.HeaderRow.FindControl("UI_CheckGroup_Reject")
        UI_CheckGroup_Reject.Checked = False

        Dim UI_Total_MaterialCost_text As Double = 0
        For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1

            If Me.UI_dvRequestDetail.Rows(i).RowType = DataControlRowType.DataRow Then

                Dim UI_Check_Accept_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Check_Reject_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
                Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
                Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")

                Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
                Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
                Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")
                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                Dim UI_ServiceCharge_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Reject")
                Dim UI_ServiceCharge_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Cancel")
                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                If UI_Check_Reject.Text.Trim() = UI_Check_Reject_.Text.Trim() Then
                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                    If Not isDiscounted Then
                        UI_ServiceCharge_Agree.Visible = False
                        UI_ServiceCharge.Visible = True
                        UI_ServiceCharge_Cancel.Visible = True
                    End If
                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                    '變換顯示 取消 開始
                    UI_TotalAmount_Agree.Visible = False
                    UI_TotalAmount.Visible = True
                    UI_TotalAmount_Cancel.Visible = True

                    UI_MaterialCost_Agree.Visible = False
                    UI_MaterialCost.Visible = True
                    UI_MaterialCost_Cancel.Visible = True
                    '變換顯示 取消 結束
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())

                End If

            End If
        Next
        '材料費總和
        For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
            Dim UI_Check_Accept_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
            Dim UI_Check_Reject_ As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

            Dim UI_TotalAmount_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
            Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")
            Dim UI_TotalAmount_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Cancel")

            Dim UI_MaterialCost_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Agree")
            Dim UI_MaterialCost As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost")
            Dim UI_MaterialCost_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_MaterialCost_Cancel")
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
            Dim UI_ServiceCharge_Agree As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
            Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Reject")
            Dim UI_ServiceCharge_Cancel As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Cancel")
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
            If UI_MaterialCost_Cancel.Visible = True Then


                UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Cancel.Text.Trim())


            Else

                If UI_MaterialCost_Agree.Text.Trim() = "" Then
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + 0
                Else
                    UI_Total_MaterialCost_text = UI_Total_MaterialCost_text + Convert.ToDouble(UI_MaterialCost_Agree.Text.Trim())
                End If

            End If
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
            If Not isDiscounted Then
                Dim cancelAmount As Decimal = 0, ServiceCharge As Decimal = 0
                Decimal.TryParse(UI_TotalAmount_Cancel.Text, cancelAmount)
                Decimal.TryParse(UI_ServiceCharge_Agree.Text, ServiceCharge)

                If cancelAmount > 0D Then
                    UI_TotalAmount_Cancel.Text = (cancelAmount - ServiceCharge).ToString("0.##")
                End If
            End If
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
        Next

        Me.UI_Total_MaterialCost.Text = UI_Total_MaterialCost_text.ToString()
        Call GetTotalAmount()
        Me.ajModalProgress.Show()

    End Sub

    Protected Function GetTotalAmount()

        Dim item_Accept As String = ""
        Dim item_Reject As String = ""

        Dim ctlClient As New ctlRMA.Client
        Dim sAmt As Double = 0, sServAmt As Double = 0 '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
        Dim dt As DataTable = TryCast(Session("_dtClientDetail_FlowCase01"), DataTable)
        Dim isDiscounted As Boolean = dt.AsEnumerable().Select(Function(r) r("CU_SERVICE_CHG_DISCOUNT") = 1).FirstOrDefault()
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

        '20210323 若有收費的Enduser 要先繳費
        For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
            Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
            Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
            Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
            Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
            Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
            Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
            Dim UI_TotalAmount As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount")

            If UI_RMAD_STATUS.Text.Trim() = "40" Then
                '是否要維修: 1.Accept, 2.Reject
                Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                If UI_Accept_.Visible = False Then
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then

                        If UI_TotalAmount.Text <> "" Then
                            sServAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Reject"), Label).Text) '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                        End If

                    Else
                        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                        If Not isDiscounted Then
                            If Not UI_Check_Reject.Checked Then
                                sServAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge_Reject"), Label).Text)
                                sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            End If
                        Else
                            If UI_Check_Reject.Checked = True Then

                                sAmt += Convert.ToDouble(UI_ServiceCharge.Text.Trim())


                            Else

                                sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)

                            End If
                        End If
                        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
                    End If
                End If
            End If
        Next
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
        If Not isDiscounted Then
            UI_Total_ServiceCharge.Text = sServAmt.ToString("0.##")
        End If
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
        TotalAmount.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag) & ":" & sAmt.ToString().Trim()

    End Function

#End Region

    Protected Function PreparePayTable() As DataTable
        Dim RMA_AHEAD As New DataTable
        RMA_AHEAD.Columns.Add("RMA_NO")
        RMA_AHEAD.Columns.Add("RMA_SERIALNO")
        RMA_AHEAD.Columns.Add("RMAD_ID")
        RMA_AHEAD.Columns.Add("RMA_ACCEPT")
        RMA_AHEAD.Columns.Add("RMA_AMT")
        RMA_AHEAD.Columns.Add("USER_ID")
        RMA_AHEAD.Columns.Add("USER_NAME")
        Return RMA_AHEAD
    End Function

    '20230718 bank transfer
    Private Function SendMailEndUser_Accounting(ByVal sRMA As String, ByVal item_Accept As String, sAmt As Double, ByVal sRMA_context As String, ByVal RMASQ_ID As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim oMail As New ctlMail
        Dim sMsg As String = ""
        Dim sSubject As String = "請協助確認End User是否付款 RMA NO." + sRMA
        Dim sBody As String = "SN : " + item_Accept + Environment.NewLine + " AMT : " + sAmt.ToString()
        Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
        Dim sMailTo As String = "sunny@cipherlab.com.tw,Miko@cipherlab.com.tw,"

        Dim CU_SALESID As String
        Dim CU_ASSISTANTID As String
        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Try

            Dim MailSales As String = ""
            Dim SalesName As String = ""
            dtCustomer = oCustomer.QueryUser(Session("_CustomerID").ToString().Trim(), Session("_UserID").ToString().Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                CU_SALESID = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                CU_ASSISTANTID = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()
            Else
                'MODI BY ANGEL 避免customer異常無法寄送mail
                Dim ctlRMA As New ctlRMA
                Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
                dtSales = ctlRMA.getSalesMail_ARNO(sRMA)
                CU_SALESID = dtSales.Rows(0)("SalesID").ToString().Trim()
                CU_ASSISTANTID = dtSales.Rows(0)("AssistantID").ToString().Trim()
            End If

            If CU_SALESID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_SALESID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim() + ","
            End If

            If CU_ASSISTANTID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()
            End If

            addLog("ctMail EndUserMail : " & sRMA)

            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            If _isDebug = True Then
                sMailTo = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)

            '會計收信
            qryRepairQuotaionRPT(sRMA)
            Dim oAttachmentFile As New Collection
            oAttachmentFile.Add(Me.ViewState("_AttachmentFile").ToString())
            Dim old_sBody As String = sBody

            Dim RMASQ_CLIENTAD As String = "0016"
            Dim RMASQ_CLIENTADNAME As String = "Fanny Tsai (蔡素蘭)"
            Dim acc_mail As String = "Fanny@cipherlab.com.tw"
            'acc_mail = "ryan.lee@cipherlab.com.tw"

            Dim context_OP As String = "&RMASQ_ID=" & sRMA & "&RMASQ_CLIENTAD=" & RMASQ_CLIENTAD & "&RMASQ_CLIENTADNAME=" & RMASQ_CLIENTADNAME & "&Customer=" & Session("_CustomerID").ToString().Trim() & "&sAmt=" & sAmt
            sBody = old_sBody & ""
            sBody = sBody & "<br>" & "<br>" & "您好，"
            sBody = sBody & "<br>"
            sBody = sBody & "<br>" & "End user針對維修報價選擇銀行帳戶匯款，請於近期留意入帳訊息。"
            sBody = sBody & "<br>" & "單號及款項請對照附件報價單。"

            sBody = sBody & "<br>" & "若確認款項已入帳，請點選以下連結，系統會自動通知維修人員進行維修。"
            sBody = sBody & "<br>" & "<span style='color: red;'>**請注意，點選後即為確認入帳不可更改，若發現入帳款項與報價單上不符，請聯絡RMA部門，我們會再通知End user進行後續處理</span>"
            sBody = sBody & "<br>" & "_全球維護服務部 _gms_dept@cipherlab.com.tw "
            sBody = sBody & "<br>"
            sBody = sBody & "</br>" & "<a href='https://e-rma.cipherlab.com.tw/SendMailEndUser_Accounting.aspx?SendMailEndUser_Accounting=" & sRMA_context & context_OP & "'>確認付款</a>"

            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            If _isDebug = True Then
                acc_mail = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, acc_mail, _MailCC, oAttachmentFile)

            RMASQ_CLIENTAD = "0044"
            RMASQ_CLIENTADNAME = "Vivian Chang (張幼親)"
            acc_mail = "Vivian@cipherlab.com.tw"
            'acc_mail = "ryan.lee@cipherlab.com.tw"

            context_OP = "&RMASQ_ID=" & sRMA & "&RMASQ_CLIENTAD=" & RMASQ_CLIENTAD & "&RMASQ_CLIENTADNAME=" & RMASQ_CLIENTADNAME & "&Customer=" & Session("_CustomerID").ToString().Trim() & "&sAmt=" & sAmt
            sBody = old_sBody & ""
            sBody = sBody & "<br>" & "<br>" & "您好，"
            sBody = sBody & "<br>"
            sBody = sBody & "<br>" & "End user針對維修報價選擇銀行帳戶匯款，請於近期留意入帳訊息。"
            sBody = sBody & "<br>" & "單號及款項請對照附件報價單。"

            sBody = sBody & "<br>" & "若確認款項已入帳，請點選以下連結，系統會自動通知維修人員進行維修。"
            sBody = sBody & "<br>" & "<span style='color: red;'>**請注意，點選後即為確認入帳不可更改，若發現入帳款項與報價單上不符，請聯絡RMA部門，我們會再通知End user進行後續處理</span>"
            sBody = sBody & "<br>" & "_全球維護服務部 _gms_dept@cipherlab.com.tw "
            sBody = sBody & "<br>"
            sBody = sBody & "</br>" & "<a href='https://e-rma.cipherlab.com.tw/SendMailEndUser_Accounting.aspx?SendMailEndUser_Accounting=" & sRMA_context & context_OP & "'>確認付款</a>"

            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            If _isDebug = True Then
                acc_mail = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, acc_mail, _MailCC, oAttachmentFile)

            RMASQ_CLIENTAD = "1312"
            RMASQ_CLIENTADNAME = "Teresa Cheng (鄭伊萍)"
            acc_mail = "Teresa.Cheng@cipherlab.com.tw"
            'acc_mail = "ryan.lee@cipherlab.com.tw"


            context_OP = "&RMASQ_ID=" & sRMA & "&RMASQ_CLIENTAD=" & RMASQ_CLIENTAD & "&RMASQ_CLIENTADNAME=" & RMASQ_CLIENTADNAME & "&Customer=" & Session("_CustomerID").ToString().Trim() & "&sAmt=" & sAmt
            sBody = old_sBody & ""
            sBody = sBody & "<br>" & "<br>" & "您好，"
            sBody = sBody & "<br>"
            sBody = sBody & "<br>" & "End user針對維修報價選擇銀行帳戶匯款，請於近期留意入帳訊息。"
            sBody = sBody & "<br>" & "單號及款項請對照附件報價單。"

            sBody = sBody & "<br>" & "若確認款項已入帳，請點選以下連結，系統會自動通知維修人員進行維修。"
            sBody = sBody & "<br>" & "<span style='color: red;'>**請注意，點選後即為確認入帳不可更改，若發現入帳款項與報價單上不符，請聯絡RMA部門，我們會再通知End user進行後續處理</span>"
            sBody = sBody & "<br>" & "_全球維護服務部 _gms_dept@cipherlab.com.tw "
            sBody = sBody & "<br>"
            sBody = sBody & "</br>" & "<a href='https://e-rma.cipherlab.com.tw/SendMailEndUser_Accounting.aspx?SendMailEndUser_Accounting=" & sRMA_context & context_OP & "'>確認付款</a>"

            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 begin
            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            'Dim mailTo As String = acc_mail & "," & "ryan.lee@cipherlab.com.tw" & "," & "rocio.peng@cipherlab.com.tw"
            Dim mailTo As String = acc_mail & "," & "rocio.peng@cipherlab.com.tw"
            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 end
            If _isDebug = True Then
                mailTo = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC, oAttachmentFile)

        Catch ex As Exception

            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

    End Function

    '2024/12/12 寄信給維修工程師 開始
    Private Sub SetEmailForRepaired(ByVal sRMA As String, ByVal item_Accept As String, ByVal item_Reject As String)

        Dim RMADataTable_ = New RmaDTO.RMADataTable
        Dim RMA_Head = New ctlRMA.Requested
        RMADataTable_ = RMA_Head.QueryByRMAHead(sRMA)
        '查客戶資料
        Dim Customer_ As New ctlCustomer.Customer
        Dim CustomerDataTable_ As New CustomerDTO.CustomerDataTable
        CustomerDataTable_ = Customer_.QueryByPrimaryKey(RMADataTable_.Rows(0)("RMA_CUNO").ToString().Trim())

        '客戶名稱
        Dim RMARQ_LUADNAME As String = Session("_UserName")
        '客戶名稱
        Dim RMASQ_CLIENTADNAME As String = Session("_UserName")
        '客戶ID
        Dim _UserID As String = Session("_UserID")
        Dim RMARQ_ACCEPT_1 As String = item_Accept
        Dim RMARQ_ACCEPT_2 As String = item_Reject

        Dim blnFlag As Boolean
        blnFlag = False
        Dim oMail As New ctlMail
        Dim sMsg As String = ""
        Dim sSubject As String = "RMA NO." & sRMA & " ’s payment has been confirmed by " & RMARQ_LUADNAME
        Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
        Dim sMailTo As String = "sunny@cipherlab.com.tw,Miko@cipherlab.com.tw,"

        Dim CU_SALESID As String
        Dim CU_ASSISTANTID As String
        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable


        Dim oRMA As New ctlRMA
        Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
        Dim Repaire_Name As String = ""                    '維修人員

        Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(sRMA)

        For j = 0 To arrRepaire.Count - 1
            Dim arrList() As String = arrRepaire(j)
            Repaire_Name = arrList(0).Trim()
            Repaire_EMAIL = arrList(1).Trim()
            sMailTo += Repaire_EMAIL + ","
        Next

        '存入paypal平台log 開始
        Dim CREATEUSER_ID As String = ""    '客戶ID
        Dim CREATEUSER_Name As String = "" '客戶名稱
        For u = 0 To CustomerDataTable_.Rows.Count - 1
            CREATEUSER_ID = CustomerDataTable_.Rows(u)("CU_NO").ToString()
            CREATEUSER_Name = CustomerDataTable_.Rows(u)("CU_NAME").ToString()
        Next

        Dim oRMA_ As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable

        Dim sBody As String = ""
        sBody += "Dear " & Repaire_Name & "," & "</br>"
        sBody += "This notice is your customer " & RMARQ_LUADNAME
        sBody += " payment has been confirmed by " & RMASQ_CLIENTADNAME & "(" & _UserID & ")" & " and get into repair status." & "</br>"
        sBody += "The RMA No. " & sRMA & " is getting into the repair status." & "</br></br>"

        If RMARQ_ACCEPT_1.Length > 0 Then
            sBody += "Accept Item: " & RMARQ_ACCEPT_1.Remove(RMARQ_ACCEPT_1.Length - 1, 1) & "</br></br>"
        Else
            sBody += "Accept Item:  </br></br>"
        End If

        If RMARQ_ACCEPT_2.Length > 0 Then
            sBody += "Cancel Item: " & RMARQ_ACCEPT_2.Remove(RMARQ_ACCEPT_2.Length - 1, 1) & "</br></br>"
        Else
            sBody += "Cancel Item:  </br></br>"
        End If

        sBody += "To check the application status, or to see detailed application information, please visit " & " web: <a href='http://e-rma.cipherlab.com.tw'>http://e-rma.cipherlab.com.tw</a> and status query page." & "</br>"
        sBody += "Thank you for using the RMA system." & "</br>"

        sBody += "== This is an automated system notification, please do not reply. ==" & "</br>"
        sBody += "== For any system operation issues, please contact Cipherlab E-RMA Administrator: e-rma@cipherlab.com.tw ==" & "</br></br>"

        sBody += "Best Regards," & "</br>"
        sBody += "Cipherlab E-RMA Service Center" & "</br>"

        ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 begin
        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
        'Dim mailTo As String = sMailTo & "ryan.lee@cipherlab.com.tw"
        Dim mailTo As String = sMailTo
        ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 end
        If _isDebug = True Then
            mailTo = ConfigurationManager.AppSettings("MailTo")
            _MailCC = ConfigurationManager.AppSettings("MailCC")
        End If
        blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC)
    End Sub
    '2024/12/12 寄信給維修工程師 結束

    Protected Sub UI_cmdBankTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdBankTransfer.Click
        '判斷0元
        Dim check_state As Boolean = True


        If 1 = 1 Then
            Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable
            '寄信內容產生
            If 1 = 1 Then
                '寄信內容產生

                Dim i As Integer = 0
                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim item_Accept As String = ""
                Dim item_Reject As String = ""

                Dim ctlRMA As New ctlRMA.Repair_Quoting
                Dim ctlRMAStatus As New ctlRMA.RMAStatus
                Dim ctlClient As New ctlRMA.Client
                Dim sAmt As Double = 0
                Dim dtEndUser As DataTable
                Dim ctlRMAR As New ctlRMA.Requested

                Try
                    For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                        Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                        Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                        Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                        Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                        Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                        Dim UI_PARTSN As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_PARTSN")

                        Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                        Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                        Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                        If UI_RMAD_STATUS.Text.Trim() = "40" Then
                            Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.NewtmpRMAREPAIR_QUOTED_ClientRow
                            dr.RMARQ_ID = UI_RMARQ_ID.Text.Trim()
                            dr.RMARQ_RMADID = UI_RMADID.Text.Trim()

                            '是否要維修: 1.Accept, 2.Reject
                            If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                                dr.RMARQ_ACCEPT = 1
                                If item_Accept.Trim() <> "" Then
                                    item_Accept = item_Accept & ","
                                End If
                                item_Accept = item_Accept & UI_SERIALNO.Text.Trim()

                            Else
                                If UI_Check_Reject.Checked = True Then
                                    dr.RMARQ_ACCEPT = 2
                                    dr.RMARQ_LABORHOUR = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                    dr.RMARQ_QUOTE = Convert.ToDouble(UI_ServiceCharge.Text.Trim())

                                    If item_Reject.Trim() <> "" Then
                                        item_Reject = item_Reject & ","
                                    End If
                                    item_Reject = item_Reject & UI_SERIALNO.Text.Trim()
                                Else
                                    'Accept與Reject都沒勾選時，視同Accept
                                    dr.RMARQ_ACCEPT = 1
                                    If item_Accept.Trim() <> "" Then
                                        item_Accept = item_Accept & ","
                                    End If
                                    item_Accept = item_Accept & UI_SERIALNO.Text.Trim()
                                End If
                            End If

                            dr.RMARQ_LUAD = Session("_UserID")
                            dr.RMARQ_LUADNAME = Session("_UserName")
                            dr.RMARQ_LUSTMP = Date.Now

                            dtRepairQuoted_Client.Rows.Add(dr)
                        End If
                    Next



                    blnFlag = True

                Catch ex As Exception
                    sMessage = ex.Message
                    blnFlag = False

                Finally

                    If blnFlag = False Then
                        Me.ucMessage.showMessageByFailed(sMessage)

                    Else

                    End If

                End Try

                '寄信內容產生
            End If
            '寄信內容產生
            '寄信
            If 1 = 1 Then

                Dim item_Reject As String = ""
                Dim item_Accept_id As String = ""
                Dim item_Accept As String = ""
                Dim item_Reject_id As String = ""
                Dim CurrencyNO As String = "USD"
                Dim RMA_AHEAD As DataTable = PreparePayTable()
                Dim sAmt As Double = 0

                '結算費用
                For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                    Dim UI_RMANO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANO")
                    Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                    Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                    Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                    Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                    Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                    Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                    Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")

                    If UI_RMAD_STATUS.Text.Trim() = "40" Then
                        Dim dr As DataRow = RMA_AHEAD.NewRow()
                        dr("RMA_NO") = Me.RMA_NO
                        dr("RMAD_ID") = UI_RMADID.Text.Trim()
                        dr("RMA_SERIALNO") = UI_SERIALNO.Text.Trim()

                        '是否要維修: 1.Accept, 2.Reject
                        If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                            dr("RMA_ACCEPT") = 1
                            dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                            item_Accept_id += UI_RMADID.Text.Trim() + ","
                        Else
                            If UI_Check_Reject.Checked = True Then
                                dr("RMA_ACCEPT") = 2
                                dr("RMA_AMT") = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                sAmt += Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                'If item_Reject.Trim() <> "" Then
                                '    item_Reject = item_Reject & ","
                                'End If
                                item_Reject = item_Reject & UI_SERIALNO.Text.Trim() + ","
                                item_Reject_id = item_Reject_id & UI_RMADID.Text.Trim() + ","

                            Else
                                'Accept與Reject都沒勾選時，視同Accept
                                'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                                dr("RMA_ACCEPT") = 1
                                dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                                sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                                item_Accept += UI_SERIALNO.Text.Trim() + ","
                                item_Accept_id += UI_RMADID.Text.Trim() + ","
                            End If
                        End If

                        dr("USER_ID") = Session("_UserID")
                        dr("USER_NAME") = Session("_UserName")
                        RMA_AHEAD.Rows.Add(dr)
                    End If
                Next


                Dim dtEndUser As DataTable
                Dim ctlRMAR As New ctlRMA.Requested

                dtEndUser = ctlRMAR.IsEndUser(Session("_UserID"))
                'dtEndUser = ctlRMAR.IsEndUser("0000041")

                '如果是enduser
                If (dtEndUser.Rows.Count > 0) Then
                    '若有費用產生且尚未付款
                    If (sAmt > 0 AndAlso dtEndUser(0)("EU_GP").ToString().Trim() <> "Y") Then

                        Dim str_json As String = ""
                        str_json = JsonConvert.SerializeObject(dtRepairQuoted_Client)


                        '20230721 發信給會計
                        Dim RMASQ_ID As String = Me.RMASQ_ID
                        SendMailEndUser_Accounting(RMA_NO, item_Accept, sAmt, str_json, RMASQ_ID)
                        '20230721 發信給會計
                        Me.ucMessage1.showMessageByFailed("<span id='ucMessage_html_Failed' style=''>" & _oLanguage.getText("RMA2", "120", ctlLanguage.eumType.Tag).Replace("*", "</br></br>*") & "</span>", Me.ViewState("_AttachmentFile").ToString().Trim())

                        Exit Sub
                    Else
                        check_state = False
                    End If
                End If

                '寄信
            End If

        End If
        '0元直接到下一步驟
        If check_state = True Then


        Else

            '取消維修 後並無檢測費用 退貨給終端客戶 開始
            Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable
            Dim ctlClient As New ctlRMA.Client
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""
            Dim RMANo As String = ""
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMANo As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANo")
                RMANo = UI_RMANo.Text

                Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_PARTSN As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_PARTSN")

                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.NewtmpRMAREPAIR_QUOTED_ClientRow
                    dr.RMARQ_ID = UI_RMARQ_ID.Text.Trim()
                    dr.RMARQ_RMADID = UI_RMADID.Text.Trim()



                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        dr.RMARQ_ACCEPT = 1
                        If item_Accept.Trim() <> "" Then
                            item_Accept = item_Accept & ","
                        End If
                        item_Accept = item_Accept & UI_SERIALNO.Text.Trim()

                    Else
                        If UI_Check_Reject.Checked = True Then
                            dr.RMARQ_ACCEPT = 2
                            dr.RMARQ_LABORHOUR = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            dr.RMARQ_QUOTE = Convert.ToDouble(UI_ServiceCharge.Text.Trim())

                            If item_Reject.Trim() <> "" Then
                                item_Reject = item_Reject & ","
                            End If
                            item_Reject = item_Reject & UI_SERIALNO.Text.Trim()
                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            dr.RMARQ_ACCEPT = 1
                            If item_Accept.Trim() <> "" Then
                                item_Accept = item_Accept & ","
                            End If
                            item_Accept = item_Accept & UI_SERIALNO.Text.Trim()
                        End If
                    End If

                    dr.RMARQ_LUAD = Session("_UserID")
                    dr.RMARQ_LUADNAME = Session("_UserName")
                    dr.RMARQ_LUSTMP = Date.Now

                    dtRepairQuoted_Client.Rows.Add(dr)
                End If
            Next

            Dim dvRMAREPAIR_QUOTED_Client As DataView = dtRepairQuoted_Client.DefaultView()
            '=============================================================================================================================================================================================================================
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            'Reject 的要 cancel
            '=============================================================================================================================================================================================================================
            'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
            Dim dtReject As New RmaDTO.ClientQuoted_ConfirmedDataTable
            dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=2"
            For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtReject.NewClientQuoted_ConfirmedRow
                Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()
                Dim RMARQ_QUOTE As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_QUOTE").ToString().Trim()
                Dim iServiceCharge As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_LABORHOUR").ToString().Trim()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                dr.RMAD_ID = RMARQ_RMADID.Trim()
                'dr.RMASQ_QUOTE = Convert.ToDouble(iServiceCharge)
                dr.RMASQ_QUOTE = Convert.ToDouble(RMARQ_QUOTE)
                dr.RMAD_STATUS = 91

                dr.RMASQ_ID = Me.RMASQ_ID
                dr.RMASQ_CLIENTAD = Session("_UserID")
                dr.RMASQ_CLIENTADNAME = Session("_UserName")
                dr.RMASQ_CLIENTDATE = Date.Now

                dtReject.AddClientQuoted_ConfirmedRow(dr)
            Next
            If dtReject.Rows.Count > 0 Then
                ctlClient.ClientQuoted_Confirmed(dtReject)
                Call SetEmailForRepaired(RMANo, item_Accept, item_Reject)
            End If


            '=============================================================================================================================================================================================================================
            '修改RMAD狀態(50)-->客戶自行確認, 報價
            '=============================================================================================================================================================================================================================
            Dim dtConfirmed As New RmaDTO.ClientQuoted_ConfirmedDataTable
            dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=1"
            For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.NewClientQuoted_ConfirmedRow
                Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                dr.RMAD_ID = RMARQ_RMADID.Trim()
                dr.RMAD_STATUS = 50

                dr.RMASQ_ID = Me.RMASQ_ID
                dr.RMASQ_CLIENTAD = Session("_UserID")
                dr.RMASQ_CLIENTADNAME = Session("_UserName")
                dr.RMASQ_CLIENTDATE = Date.Now
                dr.RMASQ_CLIENTCONFIRM = 1     '1.客戶自行確認, 2.業務帶客戶確認

                dtConfirmed.AddClientQuoted_ConfirmedRow(dr)
            Next
            If dtConfirmed.Rows.Count > 0 Then
                ctlClient.ClientQuoted_Confirmed(dtConfirmed)
                Call SetEmailForRepaired(RMANo, item_Accept, item_Reject)
            End If

            '取消維修 後並無檢測費用 退貨給終端客戶 結束
            '重新整理畫面
            Response.Redirect("Client_FlowCase01_Worklist.aspx")

        End If

    End Sub

    '20230718 PayPal Return
    Private Function PayPal_Return(ByVal sRMA As String, ByVal item_Accept As String, sAmt As Double, ByVal sRMA_context As String, ByVal RMASQ_ID As String) As String
        Dim Return_Url As String = ""
        Dim blnFlag As Boolean = False
        Dim oMail As New ctlMail
        Dim sMsg As String = ""
        Dim sSubject As String = "請協助確認End User是否付款 RMA NO." + sRMA
        Dim sBody As String = "SN : " + item_Accept + Environment.NewLine + " AMT : " + sAmt.ToString()
        Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
        Dim sMailTo As String = "sunny@cipherlab.com.tw,Miko@cipherlab.com.tw,"

        Dim CU_SALESID As String
        Dim CU_ASSISTANTID As String
        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Try

            Dim MailSales As String = ""
            Dim SalesName As String = ""
            dtCustomer = oCustomer.QueryUser(Session("_CustomerID").ToString().Trim(), Session("_UserID").ToString().Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                CU_SALESID = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                CU_ASSISTANTID = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()
            Else
                'MODI BY ANGEL 避免customer異常無法寄送mail
                Dim ctlRMA As New ctlRMA
                Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
                dtSales = ctlRMA.getSalesMail_ARNO(sRMA)
                CU_SALESID = dtSales.Rows(0)("SalesID").ToString().Trim()
                CU_ASSISTANTID = dtSales.Rows(0)("AssistantID").ToString().Trim()
            End If

            If CU_SALESID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_SALESID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim() + ","
            End If

            If CU_ASSISTANTID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()
            End If

            'addLog("ctMail MailAS : " & MailAssistant.Trim())
            addLog("ctMail EndUserMail : " & sRMA)

            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            If _isDebug = True Then
                sMailTo = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)
            Dim old_sBody As String = sBody

            Dim RMASQ_CLIENTAD As String = "admin"
            Dim RMASQ_CLIENTADNAME As String = "admin"

            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 begin
            'Dim acc_mail As String = "ryan.lee@cipherlab.com.tw"
            'acc_mail  = "ryan.lee@cipherlab.com.tw"
            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 end

            Dim context_OP As String = "&RMASQ_ID=" & sRMA & "&RMASQ_CLIENTAD=" & RMASQ_CLIENTAD & "&RMASQ_CLIENTADNAME=" & RMASQ_CLIENTADNAME & "&Customer=" & Session("_CustomerID").ToString().Trim()
            Return_Url = "https://e-rma.cipherlab.com.tw/SendMailEndUser_Accounting.aspx?SendMailEndUser_Accounting=" & sRMA_context & context_OP

        Catch ex As Exception

            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return Return_Url

    End Function

    Protected Sub UI_client_cmdPaypal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_client_cmdPaypal.Click

        Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable

        '寄信內容產生
        If 1 = 1 Then
            '寄信內容產生

            Dim i As Integer = 0
            Dim blnFlag As Boolean
            Dim sMessage As String = ""
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""

            Dim ctlRMA As New ctlRMA.Repair_Quoting
            Dim ctlRMAStatus As New ctlRMA.RMAStatus
            Dim ctlClient As New ctlRMA.Client
            Dim sAmt As Double = 0
            Dim dtEndUser As DataTable
            Dim ctlRMAR As New ctlRMA.Requested

            Try
                For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                    Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                    Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                    Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                    Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                    Dim UI_PARTSN As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_PARTSN")

                    Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                    Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                    Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                    If UI_RMAD_STATUS.Text.Trim() = "40" Then
                        Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.NewtmpRMAREPAIR_QUOTED_ClientRow
                        dr.RMARQ_ID = UI_RMARQ_ID.Text.Trim()
                        dr.RMARQ_RMADID = UI_RMADID.Text.Trim()



                        '是否要維修: 1.Accept, 2.Reject
                        If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                            dr.RMARQ_ACCEPT = 1
                            If item_Accept.Trim() <> "" Then
                                item_Accept = item_Accept & ","
                            End If
                            item_Accept = item_Accept & UI_SERIALNO.Text.Trim()

                        Else
                            If UI_Check_Reject.Checked = True Then
                                dr.RMARQ_ACCEPT = 2
                                dr.RMARQ_LABORHOUR = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                dr.RMARQ_QUOTE = Convert.ToDouble(UI_ServiceCharge.Text.Trim())

                                If item_Reject.Trim() <> "" Then
                                    item_Reject = item_Reject & ","
                                End If
                                item_Reject = item_Reject & UI_SERIALNO.Text.Trim()
                            Else
                                'Accept與Reject都沒勾選時，視同Accept
                                dr.RMARQ_ACCEPT = 1
                                If item_Accept.Trim() <> "" Then
                                    item_Accept = item_Accept & ","
                                End If
                                item_Accept = item_Accept & UI_SERIALNO.Text.Trim()
                            End If
                        End If

                        dr.RMARQ_LUAD = Session("_UserID")
                        dr.RMARQ_LUADNAME = Session("_UserName")
                        dr.RMARQ_LUSTMP = DateTime.UtcNow

                        dtRepairQuoted_Client.Rows.Add(dr)
                    End If
                Next



                blnFlag = True

            Catch ex As Exception
                sMessage = ex.Message
                blnFlag = False

            Finally

                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)

                Else

                End If

            End Try

            '寄信內容產生
        End If
        '寄信內容產生

        '寄信
        If 1 = 1 Then

            Dim item_Reject As String = ""
            Dim item_Accept_id As String = ""
            Dim item_Accept As String = ""
            Dim item_Reject_id As String = ""
            Dim CurrencyNO As String = "USD"
            Dim RMA_AHEAD As DataTable = PreparePayTable()
            Dim sAmt As Double = 0

            '結算費用
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANO")
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    Dim dr As DataRow = RMA_AHEAD.NewRow()
                    dr("RMA_NO") = Me.RMA_NO
                    dr("RMAD_ID") = UI_RMADID.Text.Trim()
                    dr("RMA_SERIALNO") = UI_SERIALNO.Text.Trim()

                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        dr("RMA_ACCEPT") = 1
                        dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        item_Accept += UI_SERIALNO.Text.Trim() + ","
                        item_Accept_id += UI_RMADID.Text.Trim() + ","
                    Else
                        If UI_Check_Reject.Checked = True Then
                            dr("RMA_ACCEPT") = 2
                            dr("RMA_AMT") = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            sAmt += Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            'If item_Reject.Trim() <> "" Then
                            '    item_Reject = item_Reject & ","
                            'End If
                            item_Reject = item_Reject & UI_SERIALNO.Text.Trim() + ","
                            item_Reject_id = item_Reject_id & UI_RMADID.Text.Trim() + ","

                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                            dr("RMA_ACCEPT") = 1
                            dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                            item_Accept_id += UI_RMADID.Text.Trim() + ","
                        End If
                    End If

                    dr("USER_ID") = Session("_UserID")
                    dr("USER_NAME") = Session("_UserName")
                    RMA_AHEAD.Rows.Add(dr)
                End If
            Next


            Dim dtEndUser As DataTable
            Dim ctlRMAR As New ctlRMA.Requested

            dtEndUser = ctlRMAR.IsEndUser(Session("_UserID"))
            'dtEndUser = ctlRMAR.IsEndUser("0000041")

            '如果是enduser
            If (dtEndUser.Rows.Count > 0) Then
                '若有費用產生且尚未付款
                If (sAmt > 0 AndAlso dtEndUser(0)("EU_GP").ToString().Trim() <> "Y") Then
                    'Me.ucMessage.showMessageByFailed("Please make the payment within 3 days. </br> Once we have confirmed your payment, </br> we will proceed to repair. </br> *PLEASE BE ADVISED THAT YOU WILL </br> BE CHARGED A 20% PROCESSING AND ADMINISTRATIVE </br> FEE BY CIPHERLAB IF YOU REQUEST A REFUND.")

                    Dim str_json As String = ""
                    str_json = JsonConvert.SerializeObject(dtRepairQuoted_Client)

                    Dim RMASQ_ID As String = Me.RMASQ_ID
                    Dim PayPal_Return_String As String = PayPal_Return(RMA_NO, item_Accept, sAmt, str_json, RMASQ_ID)

                    '正式區 面對客戶 既然你的有問題 只好先拿掉 這邊只能放 沒問題的
                    Response.Redirect("PayPalCheckout.html?sAmt=" & sAmt & "|" & PayPal_Return_String)

                    Exit Sub
                End If
            End If

            '寄信
        End If

    End Sub
    Protected Sub UI_cmdPaypal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPaypal.Click




    End Sub

    Protected Sub UI_butAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butAlert.Click

        Dim check_state As Boolean = True

        If "0" = "0" Then

            Dim sAmt As Double = 0
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""

            '20210323 若有收費的Enduser 要先繳費
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        item_Accept += UI_SERIALNO.Text.Trim() + ","
                    Else
                        If UI_Check_Reject.Checked = True Then

                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                        End If
                    End If

                End If
            Next

            If sAmt = 0 Then

                check_state = False

            End If

        End If

        If check_state = True Then


            '如果有任何費用產生需使用線上付款 開始

            Dim sAmt As Double = 0
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""
            Dim item_Accept_id As String = ""
            Dim item_Reject_id As String = ""
            Dim CurrencyNO As String = "USD"
            Dim RMA_AHEAD As DataTable = PreparePayTable()
            Dim ctlRMA As New ctlRMA.Repair_Quoting

            '結算費用
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANO")
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    Dim dr As DataRow = RMA_AHEAD.NewRow()
                    dr("RMA_NO") = Me.RMA_NO
                    dr("RMAD_ID") = UI_RMADID.Text.Trim()
                    dr("RMA_SERIALNO") = UI_SERIALNO.Text.Trim()

                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        dr("RMA_ACCEPT") = 1
                        dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        item_Accept += UI_SERIALNO.Text.Trim() + ","
                        item_Accept_id += UI_RMADID.Text.Trim() + ","
                    Else
                        If UI_Check_Reject.Checked = True Then
                            dr("RMA_ACCEPT") = 2
                            dr("RMA_AMT") = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            sAmt += Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            'If item_Reject.Trim() <> "" Then
                            '    item_Reject = item_Reject & ","
                            'End If
                            item_Reject = item_Reject & UI_SERIALNO.Text.Trim() + ","
                            item_Reject_id = item_Reject_id & UI_RMADID.Text.Trim() + ","

                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                            dr("RMA_ACCEPT") = 1
                            dr("RMA_AMT") = CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                            item_Accept_id += UI_RMADID.Text.Trim() + ","
                        End If
                    End If

                    dr("USER_ID") = Session("_UserID")
                    dr("USER_NAME") = Session("_UserName")
                    RMA_AHEAD.Rows.Add(dr)
                End If
            Next

            ctlRMA.Update_RepairQuoted_Client_Payment(RMA_AHEAD)

            Dim orderRequest As PaypalSelfOrderRequest = New PaypalSelfOrderRequest() With {
           .PayType = "RMA",
           .CancelUrl = _Paypal_Return_Url,
           .ReturnUrl = _Paypal_Cancel_Url,
           .Description = UI_RMANo.Text,
           .CustomId = Session("_UserID") + "-" + Session("_UserName"),
           .CurrencyCode = CurrencyNO,
           .Value = sAmt
         }


            'GetToken
            Dim Token As String = ""
            Dim loginUser As String = JsonConvert.SerializeObject(New LoginData With {.UserName = _Paypal_CLIENT_ID, .Password = _Paypal_CLIENT_SECRET})

            Dim TokenContent = New Net.Http.StringContent(loginUser, Encoding.UTF8, "application/json")
            TokenContent.Headers.ContentType = New Net.Http.Headers.MediaTypeHeaderValue("application/json")
            client.DefaultRequestHeaders.Clear()
            'Define request data format
            client.DefaultRequestHeaders.Accept.Add(New Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
            Try
                Dim Tokenresponser = client.PostAsync(_Paypal_Token_Url, TokenContent).Result

                Tokenresponser.EnsureSuccessStatusCode() '用来抛异常的

                If Tokenresponser.StatusCode.ToString() = "OK" Then
                    Dim r As String = Tokenresponser.Content.ReadAsStringAsync().Result.ToString()

                    Dim ResResout As ResponseResult(Of TokenData) = JsonConvert.DeserializeObject(Of ResponseResult(Of TokenData))(r)
                    Token = ResResout.Data.token

                End If

            Catch ex As HttpRequestException
                Throw New Exception(ex.Message.ToString())
            End Try

            Dim payload As String = JsonConvert.SerializeObject(orderRequest)
            Dim Content = New Net.Http.StringContent(payload, Encoding.UTF8, "application/json")
            Content.Headers.ContentType = New Net.Http.Headers.MediaTypeHeaderValue("application/json")

            client.DefaultRequestHeaders.Clear()
            client.DefaultRequestHeaders.Accept.Add(New Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))

            '加入Token
            client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", Token)

            Try


                Dim responser = client.PostAsync(_Paypal_CreatePayment_Url, Content).Result

                responser.EnsureSuccessStatusCode()

                If responser.StatusCode.ToString() = "OK" Then
                    Dim r As String = responser.Content.ReadAsStringAsync().Result.ToString()

                    Dim ResResout As ResponseResult(Of String) = JsonConvert.DeserializeObject(Of ResponseResult(Of String))(r)

                    Response.Redirect(ResResout.Data.ToString())
                End If

            Catch ex As Net.Http.HttpRequestException

                Throw New Exception(ex.Message.ToString())
            End Try

            '如果有任何費用產生需使用線上付款 結束
        Else

            '取消維修 後並無檢測費用 退貨給終端客戶 開始
            Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable
            Dim ctlClient As New ctlRMA.Client
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""
            Dim RMANo As String = ""
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMANO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMANO")
                RMANo = UI_RMANO.Text
                Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_PARTSN As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_PARTSN")

                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.NewtmpRMAREPAIR_QUOTED_ClientRow
                    dr.RMARQ_ID = UI_RMARQ_ID.Text.Trim()
                    dr.RMARQ_RMADID = UI_RMADID.Text.Trim()



                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        dr.RMARQ_ACCEPT = 1
                        If item_Accept.Trim() <> "" Then
                            item_Accept = item_Accept & ","
                        End If
                        item_Accept = item_Accept & UI_SERIALNO.Text.Trim()

                    Else
                        If UI_Check_Reject.Checked = True Then
                            dr.RMARQ_ACCEPT = 2
                            dr.RMARQ_LABORHOUR = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                            dr.RMARQ_QUOTE = Convert.ToDouble(UI_ServiceCharge.Text.Trim())

                            If item_Reject.Trim() <> "" Then
                                item_Reject = item_Reject & ","
                            End If
                            item_Reject = item_Reject & UI_SERIALNO.Text.Trim()
                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            dr.RMARQ_ACCEPT = 1
                            If item_Accept.Trim() <> "" Then
                                item_Accept = item_Accept & ","
                            End If
                            item_Accept = item_Accept & UI_SERIALNO.Text.Trim()
                        End If
                    End If

                    dr.RMARQ_LUAD = Session("_UserID")
                    dr.RMARQ_LUADNAME = Session("_UserName")
                    dr.RMARQ_LUSTMP = Date.Now

                    dtRepairQuoted_Client.Rows.Add(dr)
                End If
            Next

            Dim dvRMAREPAIR_QUOTED_Client As DataView = dtRepairQuoted_Client.DefaultView()
            '=============================================================================================================================================================================================================================
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            'Reject 的要 cancel
            '=============================================================================================================================================================================================================================
            'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
            Dim dtReject As New RmaDTO.ClientQuoted_ConfirmedDataTable
            dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=2"
            For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtReject.NewClientQuoted_ConfirmedRow
                Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()
                Dim RMARQ_QUOTE As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_QUOTE").ToString().Trim()
                Dim iServiceCharge As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_LABORHOUR").ToString().Trim()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                dr.RMAD_ID = RMARQ_RMADID.Trim()
                'dr.RMASQ_QUOTE = Convert.ToDouble(iServiceCharge)
                dr.RMASQ_QUOTE = Convert.ToDouble(RMARQ_QUOTE)
                dr.RMAD_STATUS = 91

                dr.RMASQ_ID = Me.RMASQ_ID
                dr.RMASQ_CLIENTAD = Session("_UserID")
                dr.RMASQ_CLIENTADNAME = Session("_UserName")
                dr.RMASQ_CLIENTDATE = Date.Now

                dtReject.AddClientQuoted_ConfirmedRow(dr)
            Next
            If dtReject.Rows.Count > 0 Then
                ctlClient.ClientQuoted_Confirmed(dtReject)
                Call SetEmailForRepaired(RMANo, item_Accept, item_Reject)
            End If


            '=============================================================================================================================================================================================================================
            '修改RMAD狀態(50)-->客戶自行確認, 報價
            '=============================================================================================================================================================================================================================
            Dim dtConfirmed As New RmaDTO.ClientQuoted_ConfirmedDataTable
            dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=1"
            For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.NewClientQuoted_ConfirmedRow
                Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                dr.RMAD_ID = RMARQ_RMADID.Trim()
                dr.RMAD_STATUS = 50

                dr.RMASQ_ID = Me.RMASQ_ID
                dr.RMASQ_CLIENTAD = Session("_UserID")
                dr.RMASQ_CLIENTADNAME = Session("_UserName")
                dr.RMASQ_CLIENTDATE = Date.Now
                dr.RMASQ_CLIENTCONFIRM = 1     '1.客戶自行確認, 2.業務帶客戶確認

                dtConfirmed.AddClientQuoted_ConfirmedRow(dr)
            Next
            If dtConfirmed.Rows.Count > 0 Then
                ctlClient.ClientQuoted_Confirmed(dtConfirmed)
                Call SetEmailForRepaired(RMANo, item_Accept, item_Reject)
            End If

            '取消維修 後並無檢測費用 退貨給終端客戶 結束
            '重新整理畫面
            Response.Redirect("Client_FlowCase01_Worklist.aspx")



        End If


    End Sub

    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
        Dim dt As DataTable = TryCast(Session("_dtClientDetail_FlowCase01"), DataTable)
        Dim isDiscounted As Boolean = dt.AsEnumerable().Select(Function(r) r("CU_SERVICE_CHG_DISCOUNT") = 1).FirstOrDefault()
        '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

        '防堵折讓到負數
        If 1 = 1 Then
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1

                Dim UI_Accept_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")

                Dim UI_TotalAmount_Agree_ As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount_Agree")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Reject As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Reject")


                If UI_TotalAmount_Agree_.Text = "0.00" Then
                    UI_Accept.Visible = False
                    UI_Check_Accept.Visible = True
                    UI_Check_Reject.Visible = True
                End If


            Next

        End If


        Dim ctlChargeQuoted As New ctlChargeQuoted()
        If ctlChargeQuoted.chkChargeQuoted_Approve(UI_RMANo.Text.Trim()) = True Then
            Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA2", "128", ctlLanguage.eumType.Tag) & "<br>" & "[RMA NO:" & UI_RMANo.Text.Trim() & "]")

        Else

            Dim i As Integer = 0
            Dim blnFlag As Boolean
            Dim sMessage As String = ""
            Dim item_Accept As String = ""
            Dim item_Reject As String = ""

            Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable
            Dim ctlRMA As New ctlRMA.Repair_Quoting
            Dim ctlRMAStatus As New ctlRMA.RMAStatus
            Dim ctlClient As New ctlRMA.Client
            Dim sAmt As Double = 0
            Dim dtEndUser As DataTable
            Dim ctlRMAR As New ctlRMA.Requested


            '20210323 若有收費的Enduser 要先繳費
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")
                Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")

                If UI_RMAD_STATUS.Text.Trim() = "40" Then
                    '是否要維修: 1.Accept, 2.Reject
                    If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                        sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                        item_Accept += UI_SERIALNO.Text.Trim() + ","
                    Else
                        If UI_Check_Reject.Checked = True Then

                        Else
                            'Accept與Reject都沒勾選時，視同Accept
                            'sAmt += CDbl(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount").ToString())
                            sAmt += CDbl(TryCast(Me.UI_dvRequestDetail.Rows(i).FindControl("UI_TotalAmount"), Label).Text)
                            item_Accept += UI_SERIALNO.Text.Trim() + ","
                        End If
                    End If

                End If
            Next

            dtEndUser = ctlRMAR.IsEndUser(Session("_UserID"))
            'dtEndUser = ctlRMAR.IsEndUser("0000041")

            '如果是enduser
            If (dtEndUser.Rows.Count > 0) Then
                '若有費用產生且尚未付款
                If (sAmt > 0 AndAlso dtEndUser(0)("EU_GP").ToString().Trim() <> "Y") Then
                    Me.ucMessage.showMessageByFailed("You have to pay first!")
                    SendMailEndUser(RMA_NO, item_Accept, sAmt)
                    Exit Sub
                End If
            End If
            item_Accept = ""

            Try
                For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1
                    Dim UI_RMARQ_ID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMARQ_ID")
                    Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")
                    Dim UI_RMAD_STATUS As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMAD_STATUS")
                    Dim UI_ServiceCharge As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_ServiceCharge")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_SERIALNO")
                    Dim UI_PARTSN As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_PARTSN")

                    Dim UI_Check_Accept As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Accept")
                    Dim UI_Accept As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Accept")
                    Dim UI_Check_Reject As CheckBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_Check_Reject")

                    If UI_RMAD_STATUS.Text.Trim() = "40" Then
                        Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.NewtmpRMAREPAIR_QUOTED_ClientRow
                        dr.RMARQ_ID = UI_RMARQ_ID.Text.Trim()
                        dr.RMARQ_RMADID = UI_RMADID.Text.Trim()



                        '是否要維修: 1.Accept, 2.Reject
                        If (UI_Check_Accept.Checked = True Or UI_Accept.Visible = True) And UI_Check_Reject.Checked = False Then
                            dr.RMARQ_ACCEPT = 1
                            If item_Accept.Trim() <> "" Then
                                item_Accept = item_Accept & ","
                            End If
                            item_Accept = item_Accept & UI_SERIALNO.Text.Trim()

                        Else
                            If UI_Check_Reject.Checked = True Then
                                dr.RMARQ_ACCEPT = 2
                                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                                'dr.RMARQ_LABORHOUR = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                'dr.RMARQ_QUOTE = Convert.ToDouble(UI_ServiceCharge.Text.Trim())
                                dr.RMARQ_LABORHOUR = Convert.ToDouble(If(Not isDiscounted, "0", UI_ServiceCharge.Text.Trim()))
                                dr.RMARQ_QUOTE = Convert.ToDouble(If(Not isDiscounted, "0", UI_ServiceCharge.Text.Trim()))
                                '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                                If item_Reject.Trim() <> "" Then
                                    item_Reject = item_Reject & ","
                                End If
                                item_Reject = item_Reject & UI_SERIALNO.Text.Trim()
                            Else
                                'Accept與Reject都沒勾選時，視同Accept
                                dr.RMARQ_ACCEPT = 1
                                If item_Accept.Trim() <> "" Then
                                    item_Accept = item_Accept & ","
                                End If
                                item_Accept = item_Accept & UI_SERIALNO.Text.Trim()
                            End If
                        End If

                        dr.RMARQ_LUAD = Session("_UserID")
                        dr.RMARQ_LUADNAME = Session("_UserName")
                        dr.RMARQ_LUSTMP = Date.Now

                        dtRepairQuoted_Client.Rows.Add(dr)
                    End If
                Next
                ctlRMA.Update_RepairQuoted_Client(dtRepairQuoted_Client)


                Dim dvRMAREPAIR_QUOTED_Client As DataView = dtRepairQuoted_Client.DefaultView()
                '=============================================================================================================================================================================================================================
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                'Reject 的要 cancel
                '=============================================================================================================================================================================================================================
                'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                Dim dtReject As New RmaDTO.ClientQuoted_ConfirmedDataTable
                dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=2"
                For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtReject.NewClientQuoted_ConfirmedRow
                    Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()
                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
                    'Dim RMARQ_QUOTE As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_QUOTE").ToString().Trim()
                    'Dim iServiceCharge As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_LABORHOUR").ToString().Trim()
                    Dim RMARQ_QUOTE As String = If(Not isDiscounted, "0", dvRMAREPAIR_QUOTED_Client(i)("RMARQ_QUOTE").ToString().Trim())
                    Dim iServiceCharge As String = If(Not isDiscounted, "0", dvRMAREPAIR_QUOTED_Client(i)("RMARQ_LABORHOUR").ToString().Trim())
                    '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end

                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                    '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    dr.RMAD_ID = RMARQ_RMADID.Trim()
                    'dr.RMASQ_QUOTE = Convert.ToDouble(iServiceCharge)
                    dr.RMASQ_LABORCOST = Convert.ToDouble(iServiceCharge) '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427
                    dr.RMASQ_QUOTE = Convert.ToDouble(RMARQ_QUOTE)
                    dr.RMAD_STATUS = 91

                    dr.RMASQ_ID = Me.RMASQ_ID
                    dr.RMASQ_CLIENTAD = Session("_UserID")
                    dr.RMASQ_CLIENTADNAME = Session("_UserName")
                    dr.RMASQ_CLIENTDATE = Date.Now

                    dtReject.AddClientQuoted_ConfirmedRow(dr)
                Next
                If dtReject.Rows.Count > 0 Then
                    ctlClient.ClientQuoted_Confirmed(dtReject)
                End If


                '=============================================================================================================================================================================================================================
                '修改RMAD狀態(50)-->客戶自行確認, 報價
                '=============================================================================================================================================================================================================================
                Dim dtConfirmed As New RmaDTO.ClientQuoted_ConfirmedDataTable
                dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=1"
                For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.NewClientQuoted_ConfirmedRow
                    Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()

                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                    '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    dr.RMAD_ID = RMARQ_RMADID.Trim()
                    dr.RMAD_STATUS = 50

                    dr.RMASQ_ID = Me.RMASQ_ID
                    dr.RMASQ_CLIENTAD = Session("_UserID")
                    dr.RMASQ_CLIENTADNAME = Session("_UserName")
                    dr.RMASQ_CLIENTDATE = Date.Now
                    dr.RMASQ_CLIENTCONFIRM = 1     '1.客戶自行確認, 2.業務帶客戶確認

                    dtConfirmed.AddClientQuoted_ConfirmedRow(dr)
                Next
                If dtConfirmed.Rows.Count > 0 Then
                    ctlClient.ClientQuoted_Confirmed(dtConfirmed)
                End If


                blnFlag = True

            Catch ex As Exception
                sMessage = ex.Message
                blnFlag = False

            Finally

                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)

                Else
                    'Call clearFiled()
                    'Call QueryData_workList()
                    'Call showRequestDetail()
                    Dim isSendMail As Boolean = SendMail(Me.RMA_NO.Trim(), item_Accept.Trim(), item_Reject.Trim())

                    Call UI_cmdPreview_Click(Me.UI_cmdPreview, System.EventArgs.Empty)
                End If

            End Try

        End If
    End Sub

    Protected Sub UI_cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPreview.Click
        Me.UI_lblPreviousPage_RMANO.Text = Me.RMA_NO
        Me.UI_lblPreviousPage_RMAID.Text = Me.RMA_ID
        Server.Transfer("Client_FlowCase01_Print.aspx")
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        'RMA 前端日曆選項故障 by buck modify 20250925 begin
        Me.txtStart.Text = hfStartDate.Value
        Me.txtEnd.Text = hfEndDate.Value
        'RMA 前端日曆選項故障 by buck modify 20250925 end
        If Me.UI_txtRMANo.Text.Trim() = "Please enter Any Search" Then
            Me.UI_txtRMANo.Text = ""
        End If

        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()

        'RMA 前端日曆選項故障 by buck modify 20250925
        Me.ViewState("_fdate") = ""
        If Me.txtStart.Text <> "" Then
            Me.ViewState("_fdate") = Me.txtStart.Text.Trim()
        Else
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        'RMA 前端日曆選項故障 by buck modify 20250925
        Me.ViewState("_edate") = ""
        If Me.txtEnd.Text <> "" Then
            Me.ViewState("_edate") = Me.txtEnd.Text.Trim()
        Else
            Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData_workList()
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvCustomer.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvCustomer.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvCustomer.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvCustomer.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvCustomer.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvCustomer.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 設定RMADID
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMADID</returns>
    ''' <remarks></remarks>
    Public Property RMA_ID() As String
        Get
            Return Me.ViewState("_RMA_ID").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMA_ID") = nNewValue
        End Set
    End Property

    ''' <summary>
    ''' 設定RMA_NO
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMA_NO</returns>
    ''' <remarks></remarks>
    Public Property RMA_NO() As String
        Get
            Return Me.ViewState("_RMA_NO").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMA_NO") = nNewValue
        End Set
    End Property

    ''' <summary>
    '''  設定 RMASQ_ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RMASQ_ID() As String
        Get
            Return Me.ViewState("_RMASQ_ID").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMASQ_ID") = nNewValue
        End Set
    End Property

    Private Function SendMailEndUser(ByVal sRMA As String, ByVal item_Accept As String, sAmt As Double) As Boolean
        Dim blnFlag As Boolean = False
        Dim oMail As New ctlMail
        Dim sMsg As String = ""
        Dim sSubject As String = "End User 尚未付款 RMA NO." + sRMA
        Dim sBody As String = "SN : " + item_Accept + Environment.NewLine + " AMT : " + sAmt.ToString()
        Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
        Dim sMailTo As String = "sunny@cipherlab.com.tw,Miko@cipherlab.com.tw,"

        Dim CU_SALESID As String
        Dim CU_ASSISTANTID As String
        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Try

            Dim MailSales As String = ""
            Dim SalesName As String = ""
            dtCustomer = oCustomer.QueryUser(Session("_CustomerID").ToString().Trim(), Session("_UserID").ToString().Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                CU_SALESID = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                CU_ASSISTANTID = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()
            Else
                'MODI BY ANGEL 避免customer異常無法寄送mail
                Dim ctlRMA As New ctlRMA
                Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
                dtSales = ctlRMA.getSalesMail_ARNO(sRMA)
                CU_SALESID = dtSales.Rows(0)("SalesID").ToString().Trim()
                CU_ASSISTANTID = dtSales.Rows(0)("AssistantID").ToString().Trim()
            End If

            If CU_SALESID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_SALESID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim() + ","
            End If

            If CU_ASSISTANTID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                sMailTo += dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()
            End If

            'addLog("ctMail MailAS : " & MailAssistant.Trim())
            addLog("ctMail EndUserMail : " & sRMA)


            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
            If _isDebug = True Then
                sMailTo = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)

            '會計收信
            sBody = sBody & "</br>" & "<a href='http://e-rma.cipherlab.com.tw:9090/'>確認付款</a>"
            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703

            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 begin
            'Dim mailTo As String = "ryan.lee@cipherlab.com.tw" & "," & "rocio.peng@cipherlab.com.tw"
            Dim mailTo As String = "rocio.peng@cipherlab.com.tw"
            ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 end
            If _isDebug = True Then
                mailTo = ConfigurationManager.AppSettings("MailTo")
                _MailCC = ConfigurationManager.AppSettings("MailCC")
            End If
            blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC)

        Catch ex As Exception

            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

    End Function

    Private Function SendMail(ByVal sRMA As String, ByVal item_Accept As String, ByVal item_Reject As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0


        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail
        Dim CU_NAME As String
        Dim CU_SALESID As String
        Dim CU_ASSISTANTID As String

        Try
            addLog("ctMail start : " & sRMA)
            addLog("ctMail Cust " & Session("_CustomerID").ToString().Trim())
            addLog("ctMail User " & Session("_UserID").ToString().Trim())
            dtCustomer = oCustomer.QueryUser(Session("_CustomerID").ToString().Trim(), Session("_UserID").ToString().Trim(), "")
            If dtCustomer.Rows.Count > 0 Then

                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(業務和助理) 及 維修
                '================================================================================================================================================================================================================
                CU_NAME = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                CU_SALESID = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                CU_ASSISTANTID = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()
            Else
                'MODI BY ANGEL 避免customer異常無法寄送mail
                Dim ctlRMA As New ctlRMA
                Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
                dtSales = ctlRMA.getSalesMail_ARNO(sRMA)
                CU_SALESID = dtSales.Rows(0)("SalesID").ToString().Trim()
                CU_ASSISTANTID = dtSales.Rows(0)("AssistantID").ToString().Trim()
                CU_NAME = ""
            End If

            ' addLog("ctMail ASID : " & CU_ASSISTANTID)

            Dim MailSales As String = ""
            Dim SalesName As String = ""
            If CU_SALESID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_SALESID, "")
                MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
            End If

            Dim MailAssistant As String = ""
            Dim AssistantName As String = ""
            If CU_ASSISTANTID.Trim() <> "" Then
                dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
            End If

            Dim oLoginInfo As New ctlLoginInfo
            Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Session("_UserID").ToString().Trim())

            Dim sSubject As String = _oLanguage.getMailText("Mail", "019", ctlLanguage.eumType.Mail, LanguageID)
            Dim sBody As String = _oLanguage.getMailText("Mail", "020", ctlLanguage.eumType.Mail, LanguageID)
            Dim sEmailURL As String = _oLanguage.getMailText("Mail", "005", ctlLanguage.eumType.Mail, LanguageID)

            Dim ctAddress_ As New ctAddress
            Dim dt As New DataTable
            dt = ctAddress_.Select_RMA(sRMA.Trim())

            If Not dt.Rows(0)("RMA_EUADDRESS") Is Nothing Then
                Client_FlowCase01_getRequestForm(dt.Rows(0)("RMA_ID"), True) '附件檔產生
            Else
                Client_FlowCase01_getRequestForm(dt.Rows(0)("RMA_ID"), False) '附件檔產生
            End If

            Dim Client_FlowCase01_oAttachmentFile As New Collection
            If Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01").ToString().Trim() <> "" Then
                Client_FlowCase01_oAttachmentFile.Add(Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01").ToString())
            End If

            addLog("ctMail MailAS : " & MailAssistant.Trim())

            If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                ' addLog("ctMail MailToSales : " & sRMA)
                '---150903 edit by MaggieChen---begin
                Dim sMailTo As String = ""
                If item_Accept = "" Then
                    'nothing
                Else
                    addLog("ctMail AcceptMail : " & sRMA)
                    sSubject = sSubject.Replace("[$RMA No$]", sRMA & " SN : " & item_Accept)
                    sSubject = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                        sMailTo = MailSales.Trim()
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                        If sMailTo.Trim <> "" Then
                            sMailTo = sMailTo & ","
                        End If
                        sMailTo = sMailTo & MailAssistant.Trim()
                    End If

                    sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                    sBody = sBody.Replace("[$RMA No$]", sRMA)
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        sMailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    '對象(業務和助理)
                    blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)

                End If

                '---150903 edit by MaggieChen---end


                Dim sSubject_Reject As String = _oLanguage.getMailText("Mail", "035", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody_Reject As String = _oLanguage.getMailText("RMA2", "130", ctlLanguage.eumType.Tag, LanguageID)
                Dim sMailTo_Reject As String = ""
                Dim sRepairUName_Reject As String = ""
                If item_Reject = "" Then
                    'nothing
                Else
                    addLog("ctMail RejectMail : " & sRMA)
                    sSubject_Reject = sSubject_Reject.Replace("[$RMA No$]", sRMA & " SN: " & item_Reject)
                    sSubject_Reject = sSubject_Reject.Replace("[$Customer's Name$]", CU_NAME.Trim())
                    sSubject_Reject = sSubject_Reject.Replace("[$Customer User Name$]", CU_NAME.Trim())
                    If MailSales.Trim() = "" Then
                        'Nothing
                    Else
                        sRepairUName_Reject = SalesName
                        sMailTo_Reject = MailSales.Trim()
                    End If

                    If MailAssistant.Trim() = "" Then
                        'Nothing
                    Else
                        sRepairUName_Reject = sRepairUName_Reject & "/" & AssistantName
                        If sMailTo_Reject.Trim <> "" Then
                            sMailTo_Reject = sMailTo_Reject & ","
                        End If
                        sMailTo_Reject = sMailTo_Reject & MailAssistant.Trim()
                    End If

                    sBody_Reject = sBody_Reject.Replace("[$RMA No$]", sRMA & " SN: " & item_Reject)
                    sBody_Reject = sBody_Reject.Replace("[$Repair User Name$]", sRepairUName_Reject)
                    sBody_Reject = sBody_Reject.Replace("[$RMA Request No$]", sRMA)
                    sBody_Reject = sBody_Reject.Replace("[$Email URL$]", sEmailURL)

                    Dim dt_ As New DataTable
                    dt_ = ctAddress_.Select_RMA(sRMA)
                    sBody_Reject = sBody_Reject.Replace("[$RMA REMARK$]", dt_.Rows(0)("RMA_REMARK").ToString().Trim())

                    addLog("BS : " & sRMA & " to : " & sSubject_Reject)
                    'sSubject_Reject = "新竹物流股份有限公司 RMA No: ARMA-2016100009 SN: R31164D011125 has been Canceled"

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        sMailTo_Reject = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject_Reject, sBody_Reject, sMailTo_Reject, _MailCC, Client_FlowCase01_oAttachmentFile)
                    addLog("ctMail RejectMail AS : " & sRMA)
                End If
            End If



            '是否要維修: 1.Accept, 2.Reject

            '================================================================================================
            '對象維修
            '================================================================================================
            Dim sSubject_Repair As String = _oLanguage.getMailText("Mail", "025", ctlLanguage.eumType.Mail, LanguageID)
            sSubject_Repair = sSubject_Repair.Replace("[$RMA No$]", sRMA)
            sSubject_Repair = sSubject_Repair.Replace("[$Customer's Name$]", CU_NAME.Trim())

            Dim oRMA As New ctlRMA
            Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
            Dim Repaire_Name As String = ""                    '維修人員
            Dim Repaire_EMAIL_List As String = ""              '維修人員 EMail List

            Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(sRMA)

            addLog("ctMail RMAMAIL : " & sRMA)

            'modi by hugh 20250429, 改成一次寄, 不要一封一封
            For j = 0 To arrRepaire.Count - 1
                Dim arrList() As String = arrRepaire(j)

                Repaire_Name = arrList(0).Trim()
                Repaire_EMAIL = arrList(1).Trim()

                Repaire_EMAIL_List = Repaire_EMAIL_List & "," & Repaire_EMAIL
            Next

            Dim sBody_Repair As String = _oLanguage.getMailText("Mail", "026", ctlLanguage.eumType.Mail, LanguageID)
            sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "RMA Repairers")

            If Repaire_EMAIL_List.Trim <> "" Then
                sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
                sBody_Repair = sBody_Repair.Replace("[$RMA No$]", sRMA)

                sBody_Repair = sBody_Repair.Replace("[$item_Accept$]", item_Accept)
                sBody_Repair = sBody_Repair.Replace("[$item_Reject$]", item_Reject)

                sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Repaire_EMAIL_List = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_EMAIL_List, _MailCC)
            End If

            ' For j = 0 To arrRepaire.Count - 1
            ' Dim arrList() As String = arrRepaire(j)

            ' Repaire_Name = arrList(0).Trim()
            ' Repaire_EMAIL = arrList(1).Trim()

            ' Dim sBody_Repair As String = _oLanguage.getMailText("Mail", "026", ctlLanguage.eumType.Mail, LanguageID)
            ' If Repaire_Name.Trim <> "" Then
            ' sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", Repaire_Name)
            ' Else
            ' sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "")
            ' End If

            ' If Repaire_EMAIL.Trim <> "" Then
            ' sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
            ' sBody_Repair = sBody_Repair.Replace("[$RMA No$]", sRMA)

            ' sBody_Repair = sBody_Repair.Replace("[$item_Accept$]", item_Accept)
            ' sBody_Repair = sBody_Repair.Replace("[$item_Reject$]", item_Reject)

            ' sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)
            ' blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_EMAIL, _MailCC)
            ' End If
            ' Next

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

#Region "退貨也要寄信給客戶負責的業務"

    Private Sub Client_FlowCase01_getRequestForm(ByVal sRMAID As String, ByVal EndUser As Boolean)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)


        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)

        EndUser = oRMARequest.chkISCWarrantyFee(sRMAID)

        'dtRequest = oRMARequest.getReport(Session("_LanguageID").ToString().Trim(), sRMAID)

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
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
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


            '20240308 客戶編號 開始
            Dim myctAddress As New ctAddress
            Dim CUSTOMER_PRODUCT_NUMBER As String = myctAddress.Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(dr.RMA_NO.ToString(), dr.RMAD_SERIALNO.ToString())
            drReport.RMAD_PRODUCTDESC = CUSTOMER_PRODUCT_NUMBER
            '20240308 客戶編號 結束

            '20221205 wisely add 若為Enduser送修 
            'Dim oRequested As New ctlRMA.Requested
            'Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblAccountIDText.Text.Trim())
            If (EndUser AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                'drReport.RMA_EUMAIL = dt.Rows(0)("cu_email").ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If


            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next


        Call Client_FlowCase01_Print(dtReport, EndUser, LanguageID)
    End Sub

    Private Sub Client_FlowCase01_Print(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal LanguageID As String)
        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        '取得客戶的語系
        Dim sCust As String = LanguageID
        Dim oLoginInfo As New ctlLoginInfo
        Dim sLanguageID As String = oLoginInfo.getLanguageID("Customer", sCust)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument

        '這裡SendMail兩個地方呼叫(Page_Loag、UI_cmdConfirm.Click)，不確定改了之後Page_Load有甚麼影響，Page_Load稍微看一下註解寫(EndUser要維修資訊但不是抓Repair資料)
        '改成跟開單一樣，開單的rpt只有這段，沒有分語系和EndUser以外 by buck modify 2025.08.15 
        '問題來源E-mail主旨:台灣富士通股份有限公司 Your RMA No: ARMA-2025070142 SN: FJ1233A000749 has been Canceled.
        ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
        'If (EndUser) Then
        '    If (sLanguageID = "003") Then
        '        ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
        '    Else
        '        ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
        '    End If
        'Else
        '    If (sLanguageID = "003") Then
        '        ReportDoc.Load(Server.MapPath("Report\rptRequest_02_jp.rpt"))
        '    Else
        '        ReportDoc.Load(Server.MapPath("Report\rptRequest_02.rpt"))
        '    End If

        'End If

        ReportDoc.SetDataSource(oReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF)
        Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01") = ConfigureExportToPdf_(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        ReportDoc.Close()
    End Sub

    'Public Function ConfigureExportToPdf_(ByVal sReportToPDF As String) As String
    '    Dim retval As String = _Reoprt_FilePath & sReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & sReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Return retval
    'End Function

#End Region

    Private Function addLog(ByVal LogValue As String)
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
    End Function

    Protected Sub UI_dvRequestDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequestDetail.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        If e.CommandName = "cmdDetail" Then
            row = Me.UI_dvRequestDetail.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMAD_STATUS As Label = row.FindControl("UI_RMAD_STATUS")

            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            Me.UI_lblPreviousPage_RMADID.Text = UI_RMADID.Text.Trim()
            Me.UI_lblPreviousPage_RMAID.Text = Me.RMA_ID
            Me.UI_lblRMAD_STATUS.Text = UI_RMAD_STATUS.Text.Trim()

            Dim Client_FlowCase01_Worklist_Item_New_Json_D As New Client_FlowCase01_Worklist_Item_New_Json()
            Client_FlowCase01_Worklist_Item_New_Json_D.RMANO = UI_RMANO.Text.Trim()
            Client_FlowCase01_Worklist_Item_New_Json_D.RMADID = UI_RMADID.Text.Trim()
            Client_FlowCase01_Worklist_Item_New_Json_D.RMA_ID = Me.RMA_ID
            Client_FlowCase01_Worklist_Item_New_Json_D.RMAD_STATUS = UI_RMAD_STATUS.Text.Trim()
            Dim Obj As String = JsonConvert.SerializeObject(Client_FlowCase01_Worklist_Item_New_Json_D)
            Obj = HttpUtility.UrlEncode(Obj)

            Me.windowLab.Text = " <iframe   src='Client_FlowCase01_Worklist_Item_New.aspx?DATA=" & Obj & "'   class='WaitingforProcessing_20240220'  ></iframe>  "
            Me.UI_Up_RMA_panel_ModalPopupExtender.Show()
        End If

        If e.CommandName = "Test_Report" Then
            row = Me.UI_dvRequestDetail.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMADID As Label = row.FindControl("UI_SERIALNO")


            Dim url As String = "CheckingReportList.aspx?UI_RMANO=" & UI_RMANO.Text.Trim() & "&UI_lblSerialText=" & UI_RMADID.Text.Trim()

            Dim queryString As String = url
            Dim newWin As String = "window.open('" & queryString & "','_blank');"
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", newWin, True)

        End If

    End Sub

    Protected Sub ResetBtn_Click(sender As Object, e As EventArgs) Handles ResetBtn.Click

        Me.UI_txtRMANo.Text = ""
        Me.UI_cboStatus.SelectedIndex = 0
        'RMA 前端日曆選項故障 by buck modify 20250925 begin
        txtStart.Text = ""
        txtEnd.Text = ""
        Me.ViewState("_fdate") = txtStart.Text
        Me.ViewState("_edate") = txtEnd.Text
        'RMA 前端日曆選項故障 by buck modify 20250925 end
        Call QueryData_workList()
    End Sub
End Class

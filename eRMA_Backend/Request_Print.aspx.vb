Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Request_Print
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0

        If Me.IsPostBack = False Then
            Me.UI_RedirectURL.Text = "Client_Worklist.aspx"

            '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
            Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
            For i = 0 To arrRepairNo_flowCase01.Length - 1
                If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                    Me.UI_RedirectURL.Text = "Client_FlowCase01_Worklist.aspx"
                    Exit For
                End If
            Next


            '用客戶申請時表單的維修中心判斷是否要執行 flow case 02
            Dim arrRepairNo_flowCase02() As String = _RepairNo_flowCase02.Trim().Split(",")
            For i = 0 To arrRepairNo_flowCase02.Length - 1
                If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase02(i).ToString().Trim()) <> -1 Then
                    Me.UI_RedirectURL.Text = "Client_FlowCase01_Worklist.aspx"
                    Exit For
                End If
            Next



            Dim _Crypto As New SecurityCrypt.Crypto
            Dim sRMAID As String = _Crypto.Decrypt(Request("sRMAID").Trim(), "")
            _Crypto = Nothing

            Me.UI_lblPreviousPage_RMAID.Text = sRMAID.Trim()

            Me.UI_Tittle.Text = _oLanguage.getText("RMA", "170", ctlLanguage.eumType.Tag)
            Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "045", ctlLanguage.eumType.Command)
        End If
    End Sub

    Protected Sub UI_cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdClose.Click
        Response.Redirect(Me.UI_RedirectURL.Text)
    End Sub

    ''' <summary>
    ''' Report-報表
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable
        Dim oExport As New ctlRMA.Export
        Dim sRMAID As String = Me.UI_lblPreviousPage_RMAID.Text.ToString().Trim()
        Dim EndUser As Boolean = False

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)

        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)
        'dtRequest = oRMARequest.getReport(Session("_LanguageID").ToString().Trim(), sRMAID)

        'Dim bool_ISCW As Boolean = oRMARequest.chkISCWarranty(sRMAID)
        Dim bool_ISCW As Boolean = oRMARequest.chkISCWarrantyFee(sRMAID)
        Dim bool_Parts As Boolean = False
        If dtRequest.Rows(0)("RMA_PARTSREQUEST").ToString() = "1" Then '1代表有寄PARTS MAIL
            bool_Parts = True
        End If

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

            '修改時間 20230817
            Dim RMAD_CSTMP As String = ""
            Dim oClientes As New ctlRMA.Client
            Dim tmpRequest_ListDataTable As New RmaDTO.tmpRequest_ListDataTable
            tmpRequest_ListDataTable = oClientes.Query(dr.RMA_NO.ToString().Trim(), "", "", "", "-1", "-1", "", "", "")
            For a = 0 To tmpRequest_ListDataTable.Rows.Count - 1
                RMAD_CSTMP = tmpRequest_ListDataTable.Rows(a).Item("RMAD_CSTMP").ToString()
            Next

            If RMAD_CSTMP = "" Then
                If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
            Else
                If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(RMAD_CSTMP.Trim()).ToShortDateString()
            End If
            '修改時間 20230817




            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
            If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
            'If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = oExport.getMModelNo(dr.RMAD_MODELNO.ToString().Trim(), HttpContext.Current.Session("_RepairID").ToString.Trim(), dr.RMA_CUNO.ToString().Trim())

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

            '20210617 wisely 若是BU046 且有輸入 Enduser 資料 依全保方式處理
            If dr.IsRMA_CUNONull = False Then
                If dr.RMA_CUNO.ToString().Trim() = "BU046" Then
                    bool_ISCW = True
                End If
            End If


            If (bool_ISCW) Then
                If Not (dr.IsRMA_EUCOMPANYNull) Then
                    EndUser = True
                End If
            End If
            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next

        '20210721 wisely add NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
        If (Session("_RepairID").ToString() = "NZ_PB_TECH" OrElse Session("_RepairID").ToString() = "AU_LAPTOP_KINGS") Then
            EndUser = False
        End If

        Call Print(dtReport, EndUser, bool_Parts, LanguageID)
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal PartsRequest As Boolean, ByVal sLanguageID As String)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If (PartsRequest) Then
            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts.rpt"))
            End If

        ElseIf (EndUser) Then


            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))

            End If


        Else
            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_02_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_02.rpt"))
            End If

        End If


        '不同國家 Start
        Dim myctlCustomer As New ctlCustomer.CustomerUser
        Dim myvwCustomerDataTable As New CustomerDTO.VWCUSTOMERUSERDataTable
        myvwCustomerDataTable = myctlCustomer.QueryUser(Session("_CustomerID"))

        For i = 0 To myvwCustomerDataTable.Rows.Count - 1
            Dim CU_COUNTRYID As String
            CU_COUNTRYID = myvwCustomerDataTable.Rows(i).Item("CU_COUNTRYID").ToString()

            If CU_COUNTRYID = "AU" Then

                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser_AU.rpt"))
            End If

            If CU_COUNTRYID = "NZ" Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser_AU.rpt"))
            End If


        Next
        '不同國家 End

        ReportDoc.SetDataSource(oReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
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
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & _ReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Dim sScript As String = ""
    '    sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    sScript = sScript & "window.location.href='" & Me.UI_RedirectURL.Text & "';" & vbCrLf
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

End Class

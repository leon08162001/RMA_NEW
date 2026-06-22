Imports System.Data
Imports System.IO
Imports CrystalDecisions.Shared 'MaggieChen
Imports DataService
Imports DefLanguage 'MaggieChen

Partial Class ShippingDetail
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = 3

    '所有產出報表的位置  MaggieChen ---begin---    
    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

    Shared conNumberWordLess20 As String() = {"", "ONE", "TWO", "THREE", "FOUR", "FIVE",
     "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
     "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
     "EIGHTEEN", "NINETEEN"}

    Shared conNumberWordTen As String() = {"", "", "TWENTY", "THIRTY", "FORTY", "FIFTY",
     "SIXTY", "SEVENTY", "EIGHTY", "NINETY"}
    'MaggieChen ---end---

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtShippingDetail") = Nothing
            Session("_dtShipment") = Nothing

            Call setDefault()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMASHID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMASHID")

                Me.UI_lblPreviousPage_RMASHID.Text = UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

                Call QueryData_RMAShipping()            'show表頭
                Call QueryData_RMAShippingDetail()      'show表身

                Call QueryData_RMAShippingDetail_Serial(0)      'show表身
            End If
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Me.UI_lblRMA_ARNO.Text = ""

        Me.UI_lblDateText.Text = Date.Now.ToShortDateString()

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "154", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "155", ctlLanguage.eumType.Tag)
        Me.UI_lblShipping.Text = _oLanguage.getText("RMA", "156", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        Me.UI_lblPacking.Text = _oLanguage.getText("RMA", "157", ctlLanguage.eumType.Tag)
        Me.UI_lblFrom.Text = _oLanguage.getText("RMA", "159", ctlLanguage.eumType.Tag)
        Me.UI_lblTo.Text = _oLanguage.getText("RMA", "160", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblExpress.Text = _oLanguage.getText("RMA", "161", ctlLanguage.eumType.Tag)
        Me.UI_lblTracking.Text = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)
        Me.UI_lblMemo.Text = _oLanguage.getText("RMA", "151", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "162", ctlLanguage.eumType.Tag)
        Me.UI_lblAddShippingTittle.Text = _oLanguage.getText("RMA", "162", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        'Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "044", ctlLanguage.eumType.Command)
    End Sub

    ''' <summary>
    ''' 顯示單頭資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShipping()
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByRMA_Shipping(sShippingID)

        If dtShipping.Rows.Count > 0 Then
            Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.Rows(0)
            Me.UI_lblShippingText.Text = dr.RMASH_SHIPPINGNO.ToString().Trim()
            Me.UI_lblDateText.Text = Convert.ToDateTime(dr.RMASH_CSTMP.ToString().Trim()).ToShortDateString()
            Me.UI_lblPackingtTxt.Text = dr.RMASH_PACKINGLIST.ToString().Trim()
            Me.UI_lblFromText.Text = dr.RMASH_FROM.ToString().Trim()
            Me.UI_lblCustomer.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblTrackingText.Text = dr.RMASH_TRACKINGNO.ToString().Trim()

            Me.UI_lblRMASH_COMPNO.Text = dr.RMASH_COMPNO.ToString().Trim()

            If dr.IsRMASH_ADDRESSNull = False Then Me.UI_lblAddressText.Text = dr.RMASH_ADDRESS.ToString().Trim()
            If dr.IsRMASH_EXPRESSCONull = False Then Me.UI_lblExpressText.Text = dr.RMASH_EXPRESSCO.ToString().Trim()
            If dr.IsRMASH_MEMONull = False Then Me.UI_lblMemoText.Text = dr.RMASH_MEMO.ToString().Trim()
        End If
    End Sub

    ''' <summary>
    ''' 顯示 Serial 單身資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShippingDetail_Serial(ByVal iPageIndex As Integer)
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipment = oShipment.Query_ShippingToShipmentDetail(sShippingID)

        ArrangementData(dtShipment)

        Dim dvShipment As DataView = dtShipment.DefaultView
        dvShipment.Sort = "RMASMD_RMANO asc, RMASMD_SERIALNO asc"
        Session("_dtShipment") = dtShipment
        Call Shipment_DataBind(dvShipment, iPageIndex)


        If Me.UI_lblRMA_ARNO.Text.Trim() = "" Then
            Me.UI_cmdAD.Visible = False
            Me.UI_cmdINVOICE.Visible = False
        End If

    End Sub

    Private Sub ArrangementData(ByVal dtShipment As RmaDTO.Shipment_DetailDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export

        For i = 0 To dtShipment.Rows.Count - 1
            '2021/05/06 轉換Model
            Dim sModelNo As String = oExport.getMModelNo(dtShipment.Rows(i)("RMASMD_MODELNO").ToString().Trim(), dtShipment.Rows(i)("RMA_COMPNO").ToString().Trim(), dtShipment.Rows(i)("RMA_ACCOUNTID").ToString().Trim())

            If sModelNo.Trim() <> "" Then
                dtShipment.Rows(i)("RMASMD_MODELNO") = sModelNo.Trim()
            End If
        Next

    End Sub

    Private Sub Shipment_DataBind(ByVal dvShipment As DataView, ByVal iPageIndex As Integer)
        Me.UI_dvSerial.PageSize = _PageSize * 10
        Me.UI_dvSerial.PageIndex = iPageIndex
        Me.UI_dvSerial.DataSource = dvShipment
        Me.UI_dvSerial.DataBind()
    End Sub

    Protected Sub UI_dvSerial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSerial.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "021", ctlLanguage.eumType.Tag)
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

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMA_ARNO As Label = e.Row.FindControl("UI_RMA_ARNO")

            If Me.UI_lblRMA_ARNO.Text.Trim().IndexOf(UI_RMA_ARNO.Text.Trim()) = -1 Then
                If Me.UI_lblRMA_ARNO.Text.Trim() <> "" Then
                    Me.UI_lblRMA_ARNO.Text = Me.UI_lblRMA_ARNO.Text.Trim() & ","
                End If
                Me.UI_lblRMA_ARNO.Text = Me.UI_lblRMA_ARNO.Text & UI_RMA_ARNO.Text.Trim()
            End If
        End If

    End Sub

    Protected Sub UI_dvSerial_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSerial.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtShipment") Is Nothing Then
            Dim dtShipment As RmaDTO.Shipment_DetailDataTable = Session("_dtShipment")
            Dim dvShipment As DataView = dtShipment.DefaultView
            Call Shipment_DataBind(dvShipment, iPageIndex)

        Else
            Call QueryData_RMAShippingDetail_Serial(iPageIndex)
        End If
    End Sub

    ''' <summary>
    ''' 顯示單身資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShippingDetail()
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.tmpShipping_DetailDataTable
        Dim sShippingID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByRMA_ShippingDetail(sShippingID)

        Me.UI_dvShippingList.DataSource = dtShipping.DefaultView()
        Me.UI_dvShippingList.DataBind()
    End Sub

    Protected Sub UI_dvShippingList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvShippingList.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = _oLanguage.getText("RMA", "163", ctlLanguage.eumType.Tag)
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "168", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "169", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "165", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "166", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "167", ctlLanguage.eumType.Tag)
        End If
    End Sub

    ''' <summary>
    ''' Print
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAD.Click
        'Response.Redirect("Shipping_Print.aspx?sRMASHID=" & _Crypto.Encrypt(Me.UI_lblPreviousPage_RMASHID.Text.Trim, ""))
        Dim i As Integer = 0
        Me.ViewState("_AttachmentFile_AD") = ""

        If Me.UI_lblRMA_ARNO.Text.Trim() <> "" Then
            Dim arrAR() As String = Me.UI_lblRMA_ARNO.Text.Trim().Split(",")
            For i = 0 To arrAR.Length - 1
                Dim sRMA_ARNO As String = arrAR(i).ToString().Trim()
                Call Print_AD(Me.UI_lblShippingText.Text.Trim(), Me.UI_lblRMASH_COMPNO.Text, sRMA_ARNO)   'fairy
            Next
        End If

        Dim zipFileName As String = "AD_" & oCommon.GetRandomizeNum() & ".zip"
        Dim sAttachmentFile As String = Me.ViewState("_AttachmentFile_AD").ToString().Trim()
        If sAttachmentFile.Trim() <> "" Then
            Dim arrFilepath() As String = sAttachmentFile.Split(",")
            If arrFilepath.Length > 0 Then
                Dim zip As New Ionic.Zip.ZipFile(System.Text.Encoding.[Default])
                For i = 0 To arrFilepath.Length - 1
                    zip.AddFile(arrFilepath(i).ToString().Trim(), "")
                Next
                zip.Save(_Reoprt_FilePath + zipFileName)
                zip.Dispose()
            End If

            Dim filepath As String = _Reoprt_FilePath + zipFileName
            Dim fi As New FileInfo(filepath)
            Response.Clear()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & zipFileName)
            Page.Response.AddHeader("Content-Length", fi.Length.ToString())
            Page.Response.ContentType = "application/octet-stream"
            Page.Response.Filter.Close()
            Page.Response.WriteFile(fi.FullName)
            Page.Response.[End]()

        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdINVOICE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdINVOICE.Click
        Dim i As Integer = 0
        Me.ViewState("_AttachmentFile_INVOICE") = ""

        If Me.UI_lblRMA_ARNO.Text.Trim() <> "" Then
            Dim arrAR() As String = Me.UI_lblRMA_ARNO.Text.Trim().Split(",")
            For i = 0 To arrAR.Length - 1
                Dim sRMA_ARNO As String = arrAR(i).ToString().Trim()
                Call Print_Inovice(Me.UI_lblShippingText.Text.Trim(), Me.UI_lblRMASH_COMPNO.Text, sRMA_ARNO)   'fairy
            Next
        End If

        Dim zipFileName As String = "Inovice_" & oCommon.GetRandomizeNum() & ".zip"
        Dim sAttachmentFile As String = Me.ViewState("_AttachmentFile_INVOICE").ToString().Trim()
        If sAttachmentFile.Trim() <> "" Then
            Dim arrFilepath() As String = sAttachmentFile.Split(",")
            If arrFilepath.Length > 0 Then
                Dim zip As New Ionic.Zip.ZipFile(System.Text.Encoding.[Default])
                For i = 0 To arrFilepath.Length - 1
                    zip.AddFile(arrFilepath(i).ToString().Trim(), "")
                Next
                zip.Save(_Reoprt_FilePath + zipFileName)
                zip.Dispose()
            End If

            Dim filepath As String = _Reoprt_FilePath + zipFileName
            Dim fi As New FileInfo(filepath)
            Response.Clear()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & zipFileName)
            Page.Response.AddHeader("Content-Length", fi.Length.ToString())
            Page.Response.ContentType = "application/octet-stream"
            Page.Response.Filter.Close()
            Page.Response.WriteFile(fi.FullName)
            Page.Response.[End]()

        End If

    End Sub

#Region "產生報表"

    Private Sub Print_Inovice(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable

        Dim iRMARQ_QUOTE As Double = 0.0
        'Throw New Exception(sRepairCenter.Trim())    'fairy
        dtRptINVOICE = ctlReport.qryRPTInovice(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("RMAD_SEQ") = (i + 1).ToString()
            iRMARQ_QUOTE = iRMARQ_QUOTE + Convert.ToDouble(dtRptINVOICE.Rows(i)("RMARQ_QUOTE"))
        Next

        Dim SayTotal As String = GetNumberWord(iRMARQ_QUOTE)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("SayTotal") = SayTotal
        Next

        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptINVOICE)

        Dim LanguageID As String = ""
        If dtRptINVOICE.Rows.Count > 0 Then
            Dim oLoginInfo As New ctlLoginInfo
            LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptINVOICE.Rows(0)("rmad_rmano").ToString())
        End If

        Call Print_Inovice(oDsReport, LanguageID)
    End Sub

    Private Sub Print_Inovice(ByVal oDsReport As DataSet, ByVal sLanguageID As String)
        Dim sReportToPDF_Invoice As String = "Invoice_" & oCommon.GetRandomizeNum() & ".pdf"
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sLanguageID = "003" Then
            ReportDoc.Load(Server.MapPath("Report\rptInovice_jp.rpt"))
        Else
            ReportDoc.Load(Server.MapPath("Report\rptInovice.rpt"))
        End If
        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF_Invoice)
        'ExportSetup()
        'Dim retvalFilePDF As String = ConfigureExportToPdf(sReportToPDF_Invoice)

        If Me.ViewState("_AttachmentFile_INVOICE").ToString().Trim() <> "" Then
            Me.ViewState("_AttachmentFile_INVOICE") = Me.ViewState("_AttachmentFile_INVOICE").ToString().Trim() & ","
        End If
        Me.ViewState("_AttachmentFile_INVOICE") = Me.ViewState("_AttachmentFile_INVOICE").ToString().Trim() + _Reoprt_FilePath & sReportToPDF_Invoice
        'Me.ViewState("_AttachmentFile_INVOICE") = Me.ViewState("_AttachmentFile_INVOICE").ToString().Trim() + retvalFilePDF
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

    Private Sub Print_AD(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptAD As New RmaDTO.RPTADDataTable

        dtRptAD = ctlReport.qryRPTAD(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
        For i = 0 To dtRptAD.Rows.Count - 1
            dtRptAD.Rows(i)("SEQ") = (i + 1).ToString()
        Next

        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptAD)

        Dim LanguageID As String = ""
        If dtRptAD.Rows.Count > 0 Then
            Dim oLoginInfo As New ctlLoginInfo
            LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptAD.Rows(0)("RMA單號").ToString())
        End If


        Call Print_AD(oDsReport, LanguageID)
    End Sub

    Private Sub Print_AD(ByVal oDsReport As DataSet, ByVal sLanguageID As String)
        Dim sReportToPDF_AD As String = "AD_" & oCommon.GetRandomizeNum() & ".pdf"
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sLanguageID = "003" Then
            ReportDoc.Load(Server.MapPath("Report\rptAR_jp.rpt"))
        Else
            ReportDoc.Load(Server.MapPath("Report\rptAR.rpt"))
        End If

        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF_AD)
        'ExportSetup()
        'Dim retvalFilePDF As String = ConfigureExportToPdf(sReportToPDF_AD)

        If Me.ViewState("_AttachmentFile_AD").ToString().Trim() <> "" Then
            Me.ViewState("_AttachmentFile_AD") = Me.ViewState("_AttachmentFile_AD").ToString().Trim() & ","
        End If
        Me.ViewState("_AttachmentFile_AD") = Me.ViewState("_AttachmentFile_AD").ToString().Trim() + _Reoprt_FilePath & sReportToPDF_AD
        'Me.ViewState("_AttachmentFile_AD") = Me.ViewState("_AttachmentFile_AD").ToString().Trim() + retvalFilePDF
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

    'Public Function ConfigureExportToPdf(ByVal sReportToPDF As String) As String
    '    Dim retval As String = _Reoprt_FilePath & sReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & sReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    ' '' ''Dim sScript As String = ""
    '    ' '' ''sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    ' '' ''sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    '' '' ''sScript = sScript & "window.location.href='Shipping_List.aspx';" & vbCrLf
    '    ' '' ''sScript = sScript & "</script>" & vbCrLf
    '    ' '' ''Response.Write(sScript)

    '    Return retval
    'End Function

#End Region

    Public Shared Function GetNumberWord(ByVal dValue As Double) As String
        Dim sValue As String = dValue.ToString("#.00")
        Dim sValueBeforeDot As String = sValue.Substring(0, sValue.IndexOf("."))
        Dim sValueAfterDot As String = sValue.Substring(sValue.IndexOf(".") + 1, sValue.Length - sValue.IndexOf(".") - 1)
        Dim sResult As String
        If sValueAfterDot = "00" Then
            sResult = Get2NumberWord(sValueBeforeDot) & " ONLY"
        Else
            sResult = Get2NumberWord(sValueBeforeDot) & " AND CENTS " & Get2NumberWord(sValueAfterDot) & " ONLY"
        End If
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = "SAY US DOLLAR " & sResult
        Return (sResult)
    End Function

    Protected Shared Function Get2NumberWord(ByVal sNumber As String) As String
        If sNumber = "" Then
            sNumber = "0"
        End If
        Dim iLength As Integer = sNumber.Length
        Dim iNumber As Integer = Convert.ToInt32(sNumber)
        If iLength < 3 AndAlso iLength > 0 Then
            If iNumber = 0 Then
                Return ("")
            ElseIf iNumber < 20 Then
                Return (conNumberWordLess20(iNumber))
            Else
                Dim iTenNumber As Integer, iOneNumber As Integer
                iTenNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 1))
                iOneNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 1, 1))
                Return (conNumberWordTen(iTenNumber) + "-" + conNumberWordLess20(iOneNumber))
            End If
        ElseIf iLength = 3 Then
            Dim iHundredNumber As Integer, iBelowHundredNumber As Integer
            iHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 1))
            iBelowHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 2))
            If iBelowHundredNumber = 0 Then
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED")
            ElseIf iHundredNumber = 0 Then
                Return ("AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            Else
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            End If
        ElseIf iLength > 3 And iLength < 7 Then
            Dim iThousandNumber As Integer, iBelowThousandNumber As Integer
            iThousandNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 3))
            iBelowThousandNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 3))
            Return (Get2NumberWord(iThousandNumber.ToString()) & " THOUNSAND " & IIf(iBelowThousandNumber < 100 And iBelowThousandNumber > 0, "AND ", "") & Get2NumberWord(iBelowThousandNumber.ToString()))
        ElseIf iLength > 6 AndAlso iLength < 10 Then
            Dim iMillionNumber As Integer, iBelowMillionNumber As Integer
            iMillionNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 6))
            iBelowMillionNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 6, 6))
            Return (Get2NumberWord(iMillionNumber.ToString()) & " MILLION " & IIf(iBelowMillionNumber < 100000 And iBelowMillionNumber > 0, "AND ", "") & Get2NumberWord(iBelowMillionNumber.ToString()))
        Else
            Return ("")
        End If

    End Function

End Class

Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Shipping_Print
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = "Shipping_" & oCommon.GetRandomizeNum() & ".pdf"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim sRMASHID As String = _Crypto.Decrypt(Request("sRMASHID").Trim(), "")
            Me.UI_lblPreviousPage_RMASHID.Text = sRMASHID.Trim()

            Me.UI_Tittle.Text = _oLanguage.getText("RMA", "170", ctlLanguage.eumType.Tag)
            Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "045", ctlLanguage.eumType.Command)
        End If
    End Sub

    Protected Sub UI_cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdClose.Click
        Response.Redirect("Shipping_List.aspx")
    End Sub

    ''' <summary>
    ''' Report-報表
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        Dim i As Integer = 0
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShipping As New RmaDTO.ShippingReportDataTable
        Dim dtReport As New RmaDTO.ShippingReportDataTable
        Dim sRMASHID As String = Me.UI_lblPreviousPage_RMASHID.Text.ToString().Trim()

        dtShipping = oShipping.QueryByReport(sRMASHID)

        For i = 0 To dtShipping.Rows.Count - 1
            Dim dr As RmaDTO.ShippingReportRow = dtShipping.Rows(i)
            Dim drReport As RmaDTO.ShippingReportRow = dtReport.NewShippingReportRow

            If dr.IsRMASH_SHIPPINGNONull = False Then drReport.RMASH_SHIPPINGNO = dr.RMASH_SHIPPINGNO.ToString().Trim()
            If dr.IsRMASH_PACKINGLISTNull = False Then drReport.RMASH_PACKINGLIST = dr.RMASH_PACKINGLIST.ToString().Trim()
            If dr.IsRMASH_FROMNull = False Then drReport.RMASH_FROM = dr.RMASH_FROM.ToString().Trim()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCU_TELNull = False Then drReport.CU_TEL = dr.CU_TEL.ToString().Trim()

            If dr.IsRMASH_ADDRESSNull = False Then drReport.RMASH_ADDRESS = dr.RMASH_ADDRESS.ToString().Trim()
            If dr.IsRMASH_CSTMPNull = False Then drReport.RMASH_CSTMP = Convert.ToDateTime(dr.RMASH_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsRMASH_LUADNAMENull = False Then drReport.RMASH_LUADNAME = dr.RMASH_LUADNAME.ToString().Trim()

            If dr.IsRMASHD_RMASHIDNull = False Then drReport.RMASHD_RMASHID = dr.RMASHD_RMASHID.ToString().Trim()
            If dr.IsRMASHD_CTNNONull = False Then drReport.RMASHD_CTNNO = dr.RMASHD_CTNNO.ToString().Trim()
            If dr.IsRMASHD_DESCRIPTIONNull = False Then drReport.RMASHD_DESCRIPTION = dr.RMASHD_DESCRIPTION.ToString().Trim()
            If dr.IsRMASHD_QUANTITYNull = False Then drReport.RMASHD_QUANTITY = dr.RMASHD_QUANTITY.ToString().Trim()
            If dr.IsRMASHD_NETWEIGHTNull = False Then drReport.RMASHD_NETWEIGHT = dr.RMASHD_NETWEIGHT.ToString().Trim()
            If dr.IsRMASHD_GROSSWEIGHNull = False Then drReport.RMASHD_GROSSWEIGH = dr.RMASHD_GROSSWEIGH.ToString().Trim()
            If dr.IsRMASHD_MEASUREMENTNull = False Then drReport.RMASHD_MEASUREMENT = dr.RMASHD_MEASUREMENT.ToString().Trim()
            drReport.SeqID = i + 1

            dtReport.AddShippingReportRow(drReport)
        Next

        Call Print(dtReport)
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.ShippingReportDataTable)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptShipping.rpt"))
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
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

End Class

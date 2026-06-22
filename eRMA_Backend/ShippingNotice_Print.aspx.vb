Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class ShippingNotice_Print
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = "SHP_" & oCommon.GetRandomizeNum() & ".pdf"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Me.UI_lblPreviousPage_RMANO.Text = "FRMA-2015030006, ARMA-2015030008"
            Me.UI_lblPreviousPage_ShippingNo.Text = "SHP-2015030003"

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_ShippingNo As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_ShippingNo")

                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.UI_lblPreviousPage_ShippingNo.Text = UI_lblPreviousPage_ShippingNo.Text.Trim()
            End If

            Me.UI_Tittle.Text = _oLanguage.getText("RMA", "416", ctlLanguage.eumType.Tag)
            Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_cmdPrint_Inovice.Text = _oLanguage.getText("Common", "417", ctlLanguage.eumType.Command)
            Me.UI_cmdPrint_AD.Text = _oLanguage.getText("Common", "418", ctlLanguage.eumType.Command)
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
    Protected Sub UI_cmdPrint_Inovice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint_Inovice.Click
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable

        dtRptINVOICE = ctlReport.qryRPTInovice(Me.UI_lblPreviousPage_RMANO.Text, Session("_RepairCenter1").ToString().Trim(), Me.UI_lblPreviousPage_RMANO.Text)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("RMAD_SEQ") = (i + 1).ToString()
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
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

    Protected Sub UI_cmdPrint_AD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint_AD.Click
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptAD As New RmaDTO.RPTADDataTable

        dtRptAD = ctlReport.qryRPTAD(Me.UI_lblPreviousPage_ShippingNo.Text, Session("_RepairCenter1").ToString().Trim(), Me.UI_lblPreviousPage_RMANO.Text)
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
    '    'sScript = sScript & "window.location.href='Shipping_List.aspx';" & vbCrLf
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

End Class

Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Client_FlowCase01_Print
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
    Dim _ReportToPDF As String = "ClientQuotation_" & oCommon.GetRandomizeNum() & ".pdf"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then


            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()

                Me.UI_Tittle.Text = _oLanguage.getText("RMA2", "128", ctlLanguage.eumType.Tag)
                Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
                Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "045", ctlLanguage.eumType.Command)
            End If


            Call UI_cmdPrint_Click(Me.UI_cmdPrint, System.EventArgs.Empty)
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", "window.location = 'Client_FlowCase01_Worklist.aspx';", True)

            'Me.UI_lblPreviousPage_RMANO.Text = "FRMA-2015060002"
        End If
    End Sub

    Protected Sub UI_cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdClose.Click
        Response.Redirect("Client_FlowCase01_Worklist.aspx")
    End Sub

    ''' <summary>
    ''' Report-報表
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
        Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
        Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable

        Dim sRMANO As String = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()
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
        oDsReport.Tables.Add(dtClient_SalesQuoted_SN)
        oDsReport.Tables.Add(dtClient_SalesQuoted_Part)

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMANO("Customer", sRMANO)


        Call Print(oDsReport, sRMA_COMPNO, LanguageID)
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
                ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
            End If
        End If
        ReportDoc.SetDataSource(oDsReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        ' Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
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
    '    'sScript = sScript & "window.location.href='Client_FlowCase01_Worklist.aspx';" & vbCrLf
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

End Class

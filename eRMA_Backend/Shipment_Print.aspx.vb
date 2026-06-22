Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Shipment_Print
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
    Dim _ReportToPDF As String = "Shipment_" & oCommon.GetRandomizeNum() & ".pdf"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim sRMAID As String = _Crypto.Decrypt(Request("RMASM_ID").Trim(), "")
            Me.UI_lblPreviousPage_RMASM_ID.Text = sRMAID.Trim()

            Me.UI_Tittle.Text = _oLanguage.getText("RMA", "170", ctlLanguage.eumType.Tag)
            Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
            Me.UI_cmdPrint.Text = _oLanguage.getText("Common", "045", ctlLanguage.eumType.Command)
        End If
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.ShippingSaleRoportDataTable)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptShipment.rpt"))
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

    Protected Sub UI_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPrint.Click
        Dim i As Integer = 0
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShippingReport As New RmaDTO.ShippingSaleRoportDataTable
        Dim dtReport As New RmaDTO.ShippingSaleRoportDataTable

        dtShippingReport = oShipment.QueryByReport(Me.UI_lblPreviousPage_RMASM_ID.Text.Trim())

        For i = 0 To dtShippingReport.Rows.Count - 1
            Dim dr As RmaDTO.ShippingSaleRoportRow = dtShippingReport.Rows(i)
            Dim drReport As RmaDTO.ShippingSaleRoportRow = dtReport.NewRow

            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()

            If dr.IsRMAD_WARRANTYNull = False Then
                drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()

            Else
                drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)

                'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                If dr.IsRMAD_ISWARRANTYNull = False Then
                    Select Case dr.RMAD_ISWARRANTY
                        Case 0
                            drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                        Case 1
                            drReport.RMAD_WARRANTY = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                    End Select
                End If
            End If



            If dr.IsRMAD_CSTMPNull = False Then drReport.RMAD_CSTMP = Convert.ToDateTime(dr.RMAD_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsRMASM_CSTMPNull = False Then drReport.RMASM_CSTMP = Convert.ToDateTime(dr.RMASM_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsRMASMD_RMANONull = False Then drReport.RMASMD_RMANO = dr.RMASMD_RMANO.ToString().Trim()
            If dr.IsRMASMD_SERIALNONull = False Then drReport.RMASMD_SERIALNO = dr.RMASMD_SERIALNO.ToString().Trim()
            If dr.IsRMASMD_MODELNONull = False Then drReport.RMASMD_MODELNO = dr.RMASMD_MODELNO.ToString().Trim()

            drReport.SeqID = i + 1

            dtReport.AddShippingSaleRoportRow(drReport)
        Next

        Call Print(dtReport)
    End Sub

    Protected Sub UI_cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdClose.Click
        Response.Redirect("Shipment_List.aspx")

    End Sub

End Class

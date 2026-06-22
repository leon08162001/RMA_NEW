<%@ WebHandler Language="VB" Class="GetMaintenanceStatementHandler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Newtonsoft.Json
Imports DataService
Imports System.Data

Public Class GetMaintenanceStatementHandler : Implements IHttpHandler

    Public Class MaintenanceStatement_Params
        Public Property repair_no As String
        Public Property sDate As String
        Public Property eDate As String
    End Class


    Public Class MaintenanceStatement
        Public Property TrackingNo As String

        Public Property RMA_COMPNO As String

        Public Property Shipped_Date As String

        Public Property Shipped_Year As String

        Public Property Shipped_Day As String

        Public Property Shipped_Month As String

        Public Property RMA_CUNO As String

        Public Property CU_NAME As String

        Public Property CU_COUNTRYID As String

        Public Property COUNTRY_NAME As String

        Public Property RMA_NO As String

        Public Property SERIAL_NO As String

        Public Property WARRANTY_TYPE As String

        Public Property Warranty_Kind As String

        Public Property FINAL_STATUS As String


        Public Property RMAD_RECVAD As String
    End Class


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim GetStr As String
        Dim sParams As MaintenanceStatement_Params

        Using reader As StreamReader = New StreamReader(context.Request.InputStream, Encoding.UTF8)
            'sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(reader.ReadToEnd())
            GetStr = reader.ReadToEnd()
            Dim decodeStr As String = HttpUtility.UrlDecode(GetStr)
            sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(GetStr)
        End Using

        Dim _MaintenanceStatementData As String = JsonConvert.SerializeObject(GetMaintenanceStatementData(sParams.repair_no, sParams.sDate, sParams.eDate))

        If _MaintenanceStatementData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_MaintenanceStatementData)

    End Sub


    Function GetMaintenanceStatementData(ByVal repair_no As String, ByVal sDate As String, ByVal eDate As String) As Object
        Dim oReport As New ctlReport
        Dim dMaintenanceStatementData As DataTable = oReport.qryMaintenanceStatement(repair_no, sDate, eDate)

        Dim Get_RMA_COMPNO_RMAR_REPAIRADNAMEData As DataTable = oReport.Get_RMA_COMPNO_RMAR_REPAIRADNAME(repair_no)

        For Each row As DataRow In dMaintenanceStatementData.Rows
            Dim Get_RMA_COMPNO_RMAR_REPAIRADNAMEData_grouped = Get_RMA_COMPNO_RMAR_REPAIRADNAMEData.AsEnumerable().Where(Function(dr) dr("RMAD_RMANO").ToString = row("RMA_NO") And dr("RMAD_SERIALNO").ToString = row("serial_no"))

            For Each item In Get_RMA_COMPNO_RMAR_REPAIRADNAMEData_grouped
                row("CU_COUNTRYID") = row("CU_COUNTRYID") & "|" & item("RMAR_REPAIRADNAME")
            Next
        Next row

        Return dMaintenanceStatementData

    End Function


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
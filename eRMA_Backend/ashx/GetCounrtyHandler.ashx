<%@ WebHandler Language="VB" Class="GetCounrtyHandler" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.IO


Public Class GetCounrtyHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim Repair_no As String = ""

        Using reader As StreamReader = New StreamReader(context.Request.InputStream, Encoding.UTF8)
            'sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(reader.ReadToEnd())
            Dim GetStr As String
            GetStr = reader.ReadToEnd()
            Dim Obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(GetStr)
            If Obj IsNot Nothing Then
                Repair_no = Obj.Item("Repair_No").ToString
            End If
            'Dim decodeStr As String = HttpUtility.UrlDecode(GetStr)
            'sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(GetStr)
        End Using

        Dim oExtend As New ctlExtend
        Dim _CounrtyData As String = JsonConvert.SerializeObject(oExtend.QryCounrtyData(Repair_no))

        If _CounrtyData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_CounrtyData)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
<%@ WebHandler Language="VB" Class="GetAlertRMADataHandlerRead" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data
Imports System.IO
Imports DefLanguage
Public Class GetAlertRMADataHandlerRead : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState


    Public Class SnData
        Public Property id As String
    End Class

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim oClient As New ctlRMA.Client
        Dim _Crypto As New SecurityCrypt.Crypto




        '接受前臺傳的num
        Dim Sn_no As String

        Using reader As StreamReader = New StreamReader(context.Request.InputStream)
            Sn_no = reader.ReadToEnd()
        End Using
        Dim p As SnData = JsonConvert.DeserializeObject(Of SnData)(Sn_no)

        Dim Sn_no_List As String() = p.id.Split("_")

        'oClient.Query_AlertRead(_Crypto.Decrypt(Sn_no_List(0), "").Trim(), _Crypto.Decrypt(Sn_no_List(1), "").Trim())

        context.Response.ContentType = "text/plain"
        context.Response.Write("")

    End Sub




    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
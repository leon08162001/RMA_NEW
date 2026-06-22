<%@ WebHandler Language="VB" Class="GetSnDataHandler" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data
Imports System.IO

Public Class GetSnDataHandler : Implements IHttpHandler

    Public Class test
        Public Property a As String
        Public Property b As String
    End Class

    Public Class SnData
        Public Property sn As String
    End Class


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'context.Response.ContentType = "text/plain"
        context.Response.ContentType = "application/json"
        'context.Response.ContentType = "text/json"

        'string array = context.Request.QueryString["array"] ?? string.Empty;
        'List<User> _User = JsonConvert.DeserializeObject<List<User>>(array)

        '接受前臺傳的num
        Dim Sn_no As String

        'Dim inputStream As Stream = context.Request.InputStream
        'Dim encoding As Encoding = context.Request.ContentEncoding
        'Dim streamReader As StreamReader = New StreamReader(inputStream, encoding)
        'Sn_no = streamReader.ReadToEnd()

        'For i As Integer = 0 To 300000000

        'Next

        Using reader As StreamReader = New StreamReader(context.Request.InputStream)
            Sn_no = reader.ReadToEnd()
        End Using

        If (Sn_no = "") Then
            context.Response.Write("")
            Return
        End If

        Dim p As SnData = JsonConvert.DeserializeObject(Of SnData)(Sn_no)

        Dim _SnData As String = JsonConvert.SerializeObject(GetSnData(p.sn))

        If _SnData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_SnData)

        'context.Response.Write(GetSnData(Sn_no))
    End Sub


    Function GetSnData(ByVal sn_no As String) As Object


        Dim oWarranty As New ctlWarranty
        Dim dSnData As DataTable = oWarranty.GetWarrantData(sn_no)



        Dim octAddress As New ctAddress
        Dim dtExport_ As New RmaDTO.ExportDataTable

        Dim dSnData_ As DataTable
        Dim RSION As String = oWarranty.QueryWARVERSION(sn_no)

        If RSION = "" Then
            dSnData.Rows(0)(4) = dSnData.Rows(1)(4)
            dSnData.Rows(0)(5) = dSnData.Rows(1)(5)
        End If

        Return dSnData

        'Dim data As List(Of test) = New List(Of test)()
        'data.Add(New test With {
        '    .a = "1",
        '    .b = "aaa"
        '})
        'data.Add(New test With {
        '    .a = "2",
        '    .b = "bbb"
        '})

        'Return data

    End Function



    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
<%@ WebHandler Language="VB" Class="GetWarrantyTypeHandler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Newtonsoft.Json
Imports DataService
Imports System.Data

Public Class GetWarrantyTypeHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim oExtend As New ctlExtend
        Dim _WarrantyTypeData As String = JsonConvert.SerializeObject(oExtend.GetWarrantyTypeData())

        If _WarrantyTypeData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_WarrantyTypeData)

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
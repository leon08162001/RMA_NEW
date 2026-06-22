<%@ WebHandler Language="VB" Class="GetWARRANTYSERIAL_BIHandler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Newtonsoft.Json
Imports DataService
Imports System.Data

Public Class GetWARRANTYSERIAL_BIHandler : Implements IHttpHandler

    Partial Public Class WARRANTYSERIAL_BI_
        Public Property RMAD_RMANO As String
    End Class

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim GetStr As String

        Using reader As StreamReader = New StreamReader(context.Request.InputStream, Encoding.UTF8)
            GetStr = reader.ReadToEnd()
        End Using




        'DataTable Insert Data
        Dim _RMA_CostData As String = JsonConvert.SerializeObject(GetWARRANTYSERIAL_BI(context.Request.Params("RMAD_RMANO")))


        If _RMA_CostData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_RMA_CostData)
    End Sub

    Function GetWARRANTYSERIAL_BI(ByVal RMAD_RMANO As String) As Object
        Dim octlWarranty As New ctlWarranty
        Dim dt As New DataTable
        If RMAD_RMANO <> "" Then
            dt = octlWarranty.Query_WATS_ALL(RMAD_RMANO)
        End If

        If RMAD_RMANO = "" Then
            dt = octlWarranty.Query_WATS_ALL()

        End If

        Return dt

    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
<%@ WebHandler Language="VB" Class="ExportPdfHandler" %>

Imports System
Imports System.Web
Imports System.IO

Public Class ExportPdfHandler
    Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim filePath As String = ConfigurationManager.AppSettings("Report_FilePath")
        Dim fileName As String = context.Request.QueryString("file")
        If String.IsNullOrEmpty(fileName) Then
            context.Response.StatusCode = 400
            context.Response.Write("Missing file parameter")
            Return
        End If

        ' 後端實體路徑，直接指定後端專案資料夾
        Dim pdfPath As String = Path.Combine(filePath, fileName)

        If Not File.Exists(pdfPath) Then
            context.Response.StatusCode = 404
            context.Response.Write("PDF not found")
            Return
        End If

        context.Response.ContentType = "application/pdf"
        context.Response.AddHeader("Content-Disposition", "inline; filename=" & fileName)
        context.Response.WriteFile(pdfPath)
        context.Response.Write("PDF Path: " & pdfPath)
        context.Response.End()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
<%@ WebHandler Language="VB" Class="GetCompanyHandler" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data


Public Class GetCompanyHandler : Implements IHttpHandler, IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        '登入者有開維修中心權限
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable
        Dim sAdmin As String = context.Session("_UserID").ToString()
        dtAdmin = oAdmin.Query(sAdmin, "")
        Dim oExtend As New ctlExtend
        Dim _CompanyData As String = ""

        '舊 維修中心
        Dim dt As DataTable = New DataTable
        dt = oExtend.QryCompanyData()

        '新 維修中心
        Dim dt_ As DataTable = New DataTable
        dt_ = oExtend.QryCompanyData()

        If dtAdmin.Count > 0 Then


            Dim AD_REPAIRCENTER As String() = dtAdmin(0)("AD_REPAIRCENTER").ToString().Split(",")

            For i = 0 To dt.Rows.Count - 1

                Dim index As Integer = 0

                '第一層過濾
                For a = 0 To AD_REPAIRCENTER.Length - 1
                    If (dt.Rows(i)(0).ToString() = AD_REPAIRCENTER(a).ToString()) Then
                        index = index + 1
                    End If
                Next

                If index > 0 Then

                Else
                    Dim row As DataRow = dt_.AsEnumerable().SingleOrDefault(Function(r) r("COMP_NO") = dt.Rows(i)(0).ToString())
                    If Not row Is Nothing Then
                        dt_.Rows.Remove(row)
                    End If
                End If

            Next

            _CompanyData = JsonConvert.SerializeObject(dt_)

        End If

        If _CompanyData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_CompanyData)

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
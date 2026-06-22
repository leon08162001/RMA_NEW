Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class SearchDuplicateSN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("A") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetDuplicateSNData(Wa_Cust As String, Wa_SN As String) As Object

        Dim oExtend As New ctlExtend

        Dim _DuplicateSNData As String = JsonConvert.SerializeObject(oExtend.QryDuplicateSNData(Wa_Cust, Wa_SN))

        Return _DuplicateSNData

    End Function

End Class

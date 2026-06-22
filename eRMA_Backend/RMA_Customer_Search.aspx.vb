Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class RMA_Customer_Search
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("A") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetCustomerData() As Object

        Dim oExtend As New ctlExtend

        Dim _CustomerData As String = JsonConvert.SerializeObject(oExtend.QryCustomerData())

        Return _CustomerData

    End Function

End Class

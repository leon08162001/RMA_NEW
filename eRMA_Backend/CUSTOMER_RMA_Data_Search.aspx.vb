
Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class CUSTOMER_RMA_Data_Search
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("C") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetRMAData(sDate As String, eDate As String, rma_no As String) As Object

        Dim oExtend As New ctlExtend

        Dim _RMAData As String = JsonConvert.SerializeObject(oExtend.QryRMAData(sDate, eDate, rma_no))

        Return _RMAData

    End Function
End Class

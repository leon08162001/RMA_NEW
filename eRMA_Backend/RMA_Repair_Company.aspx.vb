Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class RMA_Repair_Company
    Inherits System.Web.UI.Page

    Partial Public Class Company
        Public Property company_no As String
        Public Property country_id As String
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("C") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetCompanyCountryData(Repair_no As String) As String

        Dim oExtend As New ctlExtend

        Dim _CompanyCountryData As String = JsonConvert.SerializeObject(oExtend.QryCompanyCountryData(Repair_no))

        Return _CompanyCountryData

    End Function

    <WebMethod>
    Public Shared Function SaveCompanyCountry(objComp As Company) As String

        Dim oExtend As New ctlExtend

        Dim sErr As String = oExtend.InsertCompanyCountry(objComp.company_no, objComp.country_id, HttpContext.Current.Session("_UserID").ToString())

        Return sErr

    End Function

    <WebMethod>
    Public Shared Function DeleteData(company_no As String, country_id As String) As String

        Dim oExtend As New ctlExtend

        Dim sErr As String = oExtend.DelCompanyCountry(company_no, country_id)

        Return sErr

    End Function

End Class

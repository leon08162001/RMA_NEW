Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class SALES_RELATE
    Inherits System.Web.UI.Page

    Partial Public Class Sale_Relate
        Public Property sales_id As String
        Public Property head_id As String
        Public Property asst_id As String
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("A") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetSalesRelateData() As Object

        Dim oExtend As New ctlExtend

        Dim _SalesRelateData As String = JsonConvert.SerializeObject(oExtend.QrySALES_RELATEData(""))

        Return _SalesRelateData

    End Function

    <WebMethod>
    Public Shared Function EditData(sale_id As String) As String

        Dim oExtend As New ctlExtend

        Dim _SalesRelateData As String = JsonConvert.SerializeObject(oExtend.QrySALES_RELATEData(sale_id))

        Return _SalesRelateData

    End Function

    <WebMethod>
    Public Shared Function DeleteData(sale_id As String) As String

        Dim oExtend As New ctlExtend

        Dim sErr As String = oExtend.DelSALES_RELATE(sale_id)

        Return sErr

    End Function

    <WebMethod>
    Public Shared Function UpdateData(objSale As Sale_Relate) As String

        Dim oExtend As New ctlExtend

        Dim sErr As String = oExtend.UpdateSALES_RELATE(objSale.sales_id, objSale.head_id, objSale.asst_id)

        Return sErr

    End Function

    <WebMethod>
    Public Shared Function SaveSalesRelate(objSale As Sale_Relate) As String

        Dim oExtend As New ctlExtend

        Dim sErr As String = oExtend.InsertSALES_RELATE(objSale.sales_id, objSale.head_id, objSale.asst_id)

        Return sErr

    End Function

End Class

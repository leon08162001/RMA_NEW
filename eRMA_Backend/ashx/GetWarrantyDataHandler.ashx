<%@ WebHandler Language="VB" Class="GetWarrantyDataHandler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Newtonsoft.Json
Imports DataService
Imports System.Data

Public Class GetWarrantyDataHandler : Implements IHttpHandler

    Partial Public Class Warranty_Data_Params
        Public Property Wa_sDate As String
        Public Property Wa_eDate As String
        Public Property Wa_Cust As String
        Public Property Wa_InvNo As String
        Public Property Wa_SaleID As String
        Public Property Wa_Type As String
        Public Property Wa_Model As String
        Public Property Wa_SN As String
    End Class

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim GetStr As String
        Dim sParams As Warranty_Data_Params

        Using reader As StreamReader = New StreamReader(context.Request.InputStream, Encoding.UTF8)
            'sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(reader.ReadToEnd())
            GetStr = reader.ReadToEnd()
            Dim decodeStr As String = HttpUtility.UrlDecode(GetStr)
            sParams = JsonConvert.DeserializeObject(Of Warranty_Data_Params)(GetStr)
        End Using

        Dim _RMA_CostData As String = JsonConvert.SerializeObject(GetWarrantyData(sParams.Wa_sDate, sParams.Wa_eDate,
                                                                                  sParams.Wa_Cust, sParams.Wa_InvNo, sParams.Wa_SaleID,
                                                                                  sParams.Wa_Type, sParams.Wa_Model, sParams.Wa_SN))


        If _RMA_CostData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_RMA_CostData)
    End Sub

    Function GetWarrantyData(ByVal sDate As String, ByVal eDate As String, ByVal sCust As String,
                             ByVal sInvNo As String, ByVal sSaleID As String, ByVal sType As String,
                             ByVal sModel As String, ByVal sSN As String) As Object
        Dim oExtend As New ctlExtend
        Dim dRMA_CostData As DataTable = oExtend.QryWarrantyData(sDate, eDate, sCust, sInvNo, sSaleID, sType, sModel, sSN)

        Return dRMA_CostData

    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
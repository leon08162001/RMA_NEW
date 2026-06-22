Imports System.Linq
Imports DefLanguage
Imports ICAT_OracleDAO
Imports RMA_Model

Public Class ctlCipherlab
    Public Class Quote
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得新零件報價
        ''' </summary>
        ''' <param name="CipherlabReq">CipherlabReq</param>
        ''' <returns>傳回SkuPrice</returns>
        ''' <remarks></remarks>
        Public Function getNewSkuPriceFromERP(cipherlabReq As CipherlabReq) As Decimal
            Dim dcSkuPrice As Decimal = 0
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = String.Format("select CIPHERLAB.fn_get_sku_new_price_erp('{0}', '{1}') AS New_sku_price FROM dual",
                                           cipherlabReq.CU_NO, cipherlabReq.Part_No)

                dt = oQuery.ExecuteDT(sSQL)

                If dt.Rows.Count > 0 Then
                    Dim val = dt.Rows(0)("New_sku_price")

                    dcSkuPrice = If(val IsNot Nothing AndAlso Not IsDBNull(val) AndAlso Decimal.TryParse(val.ToString(), Nothing), Convert.ToDecimal(val), 0D)
                End If

            Catch ex As Exception
                Throw
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dcSkuPrice
        End Function


    End Class

End Class

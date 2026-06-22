Imports System.Data.OracleClient
Imports ICAT_OracleDAO

Public Class ctlExchangeRate

    ''' <summary>
    ''' 取得幣別資料
    ''' </summary>
    ''' <param name="OrderBY">排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 CurrencyDataTable</remarks>
    Public Function QueryAll(Optional ByVal OrderBY As String = "") As CurrencyDTO.CurrencyDataTable

        Dim dt As New DataTable
        Dim dtExchange As New CurrencyDTO.CurrencyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " CURRENCY_CODE asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            Dim sSQL As String = "SELECT * FROM CURRENCY " & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtExchange)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtExchange
    End Function

    ''' <summary>
    ''' 取得幣別資料
    ''' </summary>
    ''' <param name="Visible">是否顯示(1:顯示 , 0:不顯示, "":全部)</param>
    ''' <param name="OrderBY">排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 CurrencyDataTable</remarks>
    Public Function Query(ByVal Visible As String, Optional ByVal OrderBY As String = "") As CurrencyDTO.CurrencyDataTable

        Dim dt As New DataTable
        Dim dtExchange As New CurrencyDTO.CurrencyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " CURRENCY_CODE asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If Visible.ToString().Trim() <> "" Then
                oQuery.addWHERE("CURRENCY_VISIBLE", ":CURRENCY_VISIBLE", Visible, OracleType.Int16)
                sCondition = sCondition & " AND CURRENCY_VISIBLE=:CURRENCY_VISIBLE"
            End If

            Dim sSQL As String = "SELECT * FROM CURRENCY WHERE 1=1 " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtExchange)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtExchange
    End Function

    ''' <summary>
    ''' 幣別資料 - 修改
    ''' </summary>
    ''' <param name="dtExchange">傳入CurrencyDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtExchange As CurrencyDTO.CurrencyDataTable)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            For i = 0 To dtExchange.Rows.Count - 1
                Dim dr As CurrencyDTO.CurrencyRow = dtExchange.Rows(i)

                oExecute.addParameter("CURRENCY_RATE", dr.CURRENCY_RATE.ToString().Trim(), OracleType.Double)
                oExecute.addParameter("CURRENCY_VISIBLE", dr.CURRENCY_VISIBLE.ToString().Trim(), OracleType.Int16)

                'oExecute.addParameter("CURRENCY_SYMBOL", dr.CURRENCY_SYMBOL.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("CURRENCY_AD", dr.CURRENCY_AD.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("CURRENCY_ADNAME", dr.CURRENCY_ADNAME.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("CURRENCY_CSTMP", dr.CURRENCY_CSTMP, OracleType.DateTime)

                oExecute.addParameter("CURRENCY_LUAD", dr.CURRENCY_LUAD, OracleType.NVarChar)
                oExecute.addParameter("CURRENCY_LUADNAME", dr.CURRENCY_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("CURRENCY_LUSTMP", dr.CURRENCY_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("CURRENCY_CODE", dr.CURRENCY_CODE.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("CURRENCY", Execute.eumCommandType.UPDATE)
            Next
            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

End Class

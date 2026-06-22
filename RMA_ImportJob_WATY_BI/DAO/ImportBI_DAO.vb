Imports System.Data.OracleClient
Imports System.IO
Imports ICAT_OracleDAO
Imports Oracle.ManagedDataAccess

Public Class ImportBI_DAO

    ''' <summary>
    ''' 將 List WarrantyBIRsp 資料匯入 RMA 資料庫
    ''' </summary>
    Public Function InsertData(LsWatyBIRsp As List(Of WarrantyBIRsp), Optional writer As StreamWriter = Nothing) As Integer

        Dim connection As New Connection
        Dim oExecute As New Execute(connection)
        Dim successCount As Integer = 0
        If LsWatyBIRsp Is Nothing OrElse LsWatyBIRsp.Count = 0 Then Return 0
        connection.Open()

        Dim uniqueKeys As String() = {"BI_SOURCE", "BI_ORDERNO", "BI_CUNO", "BI_ORDERSEQ"}
        ' 🔹 取得資料庫現有唯一鍵組合
        Dim existingDict As New Dictionary(Of String, String)
        Dim selectColumns As String = String.Join(", ", uniqueKeys)
        Dim sql As String = String.Format("SELECT BI_ID, {0} FROM WARRANTY_BI", selectColumns)

        Dim cmdCheck As OracleCommand = connection.Command
        cmdCheck.CommandText = sql
        Using reader As OracleDataReader = cmdCheck.ExecuteReader()
            While reader.Read()
                Dim keyParts As New List(Of String)
                For i As Integer = 1 To uniqueKeys.Length
                    keyParts.Add(reader.GetValue(i).ToString().Trim())
                Next
                Dim key = String.Join("|", keyParts)
                existingDict(key) = reader.GetString(0).Trim()
            End While
        End Using


        For Each itm As WarrantyBIRsp In LsWatyBIRsp
            connection.BeginTransaction()
            Try
                Dim cmd As OracleCommand = connection.Command
                cmd.Parameters.Clear()

                ' 🔹 產生來源唯一鍵
                Dim keyParts As New List(Of String)
                For Each col As String In uniqueKeys
                    Dim prop = itm.GetType().GetProperty(col)
                    If prop Is Nothing Then Throw New Exception($"來源資料沒有欄位 {col}")
                    keyParts.Add(prop.GetValue(itm)?.ToString()?.Trim())
                Next
                Dim srcKey = String.Join("|", keyParts)

                ' 🔹 判斷是否存在
                Dim isUpdate As Boolean = existingDict.ContainsKey(srcKey)
                Dim finalBI_ID As String = If(isUpdate, existingDict(srcKey), Guid.NewGuid().ToString())

                ' 🔹 設定 SQL
                If isUpdate Then
                    cmd.CommandText = "
                        UPDATE WARRANTY_BI SET
                            BI_SOURCE = :BI_SOURCE,
                            BI_WATY_DATE = :BI_WATY_DATE,
                            BI_CUNO = :BI_CUNO,
                            BI_CUNAME = :BI_CUNAME,
                            BI_PRODUCTNO = :BI_PRODUCTNO,
                            BI_ORDERQTY = :BI_ORDERQTY,
                            BI_WATY_TYPE = :BI_WATY_TYPE,
                            BI_WATY_VER = :BI_WATY_VER,
                            BI_WATYNO = :BI_WATYNO,
                            BI_BATTERYQTY = :BI_BATTERYQTY,
                            BI_WATY_SDATE = :BI_WATY_SDATE,
                            BI_WATY_EDATE = :BI_WATY_EDATE,
                            BI_LUAD = :BI_LUAD,
                            BI_LUADNAME = :BI_LUADNAME,
                            BI_LUSTMP = :BI_LUSTMP
                        WHERE BI_ID = :BI_ID
                    "

                    ' ✅ 加入 UPDATE 所需參數
                    cmd.Parameters.Add(New OracleParameter(":BI_SOURCE", itm.BI_SOURCE))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_DATE", New Common().ToOracleDate(itm.BI_WATY_DATE)))
                    cmd.Parameters.Add(New OracleParameter(":BI_CUNO", itm.BI_CUNO))
                    cmd.Parameters.Add(New OracleParameter(":BI_CUNAME", itm.BI_CUNAME))
                    cmd.Parameters.Add(New OracleParameter(":BI_PRODUCTNO", itm.BI_PRODUCTNO))
                    cmd.Parameters.Add(New OracleParameter(":BI_ORDERQTY", If(Decimal.TryParse(itm.BI_ORDERQTY.ToString(), Nothing), CDec(itm.BI_ORDERQTY), 0D)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_TYPE", itm.BI_WATY_TYPE))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_VER", itm.BI_WATY_VER))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATYNO", If(String.IsNullOrWhiteSpace(itm.BI_WATYNO), DBNull.Value, itm.BI_WATYNO)))
                    cmd.Parameters.Add(New OracleParameter(":BI_BATTERYQTY", If(Decimal.TryParse(itm.BI_BATTERYQTY.ToString(), Nothing), CDec(itm.BI_BATTERYQTY), 0D)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_SDATE", New Common().ToOracleDate(itm.BI_WATY_SDATE)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_EDATE", New Common().ToOracleDate(itm.BI_WATY_EDATE)))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUAD", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUADNAME", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUSTMP", DateTime.Now))
                    cmd.Parameters.Add(New OracleParameter(":BI_ID", finalBI_ID))
                Else
                    cmd.CommandText = "
                        INSERT INTO WARRANTY_BI (
                            BI_ID, BI_SOURCE, BI_ORDERNO, BI_WATY_DATE, BI_CUNO, BI_CUNAME,
                            BI_ORDERSEQ, BI_PRODUCTNO, BI_ORDERQTY, BI_WATY_TYPE, BI_WATY_VER,
                            BI_WATYNO, BI_BATTERYQTY, BI_WATY_SDATE, BI_WATY_EDATE,
                            BI_AD, BI_ADNAME, BI_CSTMP, BI_LUAD, BI_LUADNAME, BI_LUSTMP
                        ) VALUES (
                            :BI_ID, :BI_SOURCE, :BI_ORDERNO, :BI_WATY_DATE, :BI_CUNO, :BI_CUNAME,
                            :BI_ORDERSEQ, :BI_PRODUCTNO, :BI_ORDERQTY, :BI_WATY_TYPE, :BI_WATY_VER,
                            :BI_WATYNO, :BI_BATTERYQTY, :BI_WATY_SDATE, :BI_WATY_EDATE,
                            :BI_AD, :BI_ADNAME, :BI_CSTMP, :BI_LUAD, :BI_LUADNAME, :BI_LUSTMP
                        )
                    "
                    ' 🔹 參數設定 (日期、數字欄位安全處理)
                    cmd.Parameters.Add(New OracleParameter(":BI_ID", finalBI_ID))
                    cmd.Parameters.Add(New OracleParameter(":BI_SOURCE", itm.BI_SOURCE))
                    cmd.Parameters.Add(New OracleParameter(":BI_ORDERNO", itm.BI_ORDERNO))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_DATE", New Common().ToOracleDate(itm.BI_WATY_DATE)))
                    cmd.Parameters.Add(New OracleParameter(":BI_CUNO", itm.BI_CUNO))
                    cmd.Parameters.Add(New OracleParameter(":BI_CUNAME", itm.BI_CUNAME))
                    cmd.Parameters.Add(New OracleParameter(":BI_ORDERSEQ", itm.BI_ORDERSEQ))
                    cmd.Parameters.Add(New OracleParameter(":BI_PRODUCTNO", itm.BI_PRODUCTNO))
                    cmd.Parameters.Add(New OracleParameter(":BI_ORDERQTY", If(Decimal.TryParse(itm.BI_ORDERQTY.ToString(), Nothing), CDec(itm.BI_ORDERQTY), 0D)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_TYPE", itm.BI_WATY_TYPE))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_VER", itm.BI_WATY_VER))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATYNO", If(String.IsNullOrWhiteSpace(itm.BI_WATYNO), DBNull.Value, itm.BI_WATYNO)))
                    cmd.Parameters.Add(New OracleParameter(":BI_BATTERYQTY", If(Decimal.TryParse(itm.BI_BATTERYQTY.ToString(), Nothing), CDec(itm.BI_BATTERYQTY), 0D)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_SDATE", New Common().ToOracleDate(itm.BI_WATY_SDATE)))
                    cmd.Parameters.Add(New OracleParameter(":BI_WATY_EDATE", New Common().ToOracleDate(itm.BI_WATY_EDATE)))

                    cmd.Parameters.Add(New OracleParameter(":BI_AD", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_ADNAME", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_CSTMP", DateTime.Now))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUAD", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUADNAME", "system"))
                    cmd.Parameters.Add(New OracleParameter(":BI_LUSTMP", DateTime.Now))
                End If

                cmd.ExecuteNonQuery()
                connection.Commit()
                successCount += 1

                ' 🔹 log 新增或更新
                If writer IsNot Nothing Then
                    writer.WriteLine(If(isUpdate, $"🟢 更新 BI_ID={finalBI_ID}", $"🔸 新增 BI_ID={finalBI_ID}"))
                End If

            Catch ex As Exception
                connection.Rollback()
                If writer IsNot Nothing Then writer.WriteLine($"❌ 匯入錯誤: {ex.Message}")
            End Try
        Next

        Return successCount
    End Function

End Class

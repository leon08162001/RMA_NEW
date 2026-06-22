Imports System.Globalization

Public Class Common
    ''' <summary>
    ''' Datatable 轉型 Model
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    Public Function DataTableToModel(Of T As New)(dt As DataTable) As T
        Dim obj As New T()

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return obj
        End If

        Dim row As DataRow = dt.Rows(0)
        Dim props = GetType(T).GetProperties()
        Dim columnNames = dt.Columns.Cast(Of DataColumn)().Select(Function(c) c.ColumnName.ToLower()).ToList()

        For Each prop In props
            Dim colIndex = columnNames.IndexOf(prop.Name.ToLower())
            If colIndex >= 0 Then
                Dim value = row(dt.Columns(colIndex))
                If value IsNot DBNull.Value Then
                    Try
                        prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType))
                    Catch
                        ' 若型別不合或轉換失敗就略過
                    End Try
                End If
            End If
        Next

        Return obj
    End Function

    Public Function DataTableToModelList(Of T As New)(dt As DataTable) As List(Of T)
        Dim list As New List(Of T)
        For Each row As DataRow In dt.Rows
            Dim model As New T()
            For Each prop In GetType(T).GetProperties()
                If dt.Columns.Contains(prop.Name) AndAlso Not IsDBNull(row(prop.Name)) Then
                    prop.SetValue(model, Convert.ChangeType(row(prop.Name), prop.PropertyType), Nothing)
                End If
            Next
            list.Add(model)
        Next
        Return list
    End Function

    ''' <summary>
    ''' 將任意型別（DateTime 或 String）安全轉換成 Oracle DATE 參數值。
    ''' </summary>
    Public Function ToOracleDate(value As Object) As Object
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return DBNull.Value
        End If

        ' 若本身就是 DateTime
        If TypeOf value Is Date OrElse TypeOf value Is DateTime Then
            Return CType(value, DateTime)
        End If

        ' 若是字串，嘗試解析多種日期格式
        Dim s As String = value.ToString().Trim()
        If s = "" Then Return DBNull.Value

        Dim dt As DateTime
        Dim formats() As String = {
            "M/d/yyyy h:mm:ss tt",
            "M/d/yyyy h:mm tt",
            "yyyy/MM/dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy/MM/dd",
            "yyyy-MM-dd"
        }

        If DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, dt) Then
            Return dt
        Else
            Return DBNull.Value
        End If
    End Function

End Class

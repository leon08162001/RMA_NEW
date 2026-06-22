Module Common

    ''' <summary>
    ''' 由 DataTable 轉換到 另一各 DTO DataTable 
    ''' </summary>
    ''' <param name="dtSource">來源資料表</param>
    ''' <param name="dtTarget">目的地資料表</param>
    ''' <remarks></remarks>
    Public Sub TransferDataTable(ByVal dtSource As DataTable, ByRef dtTarget As DataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0

        For i = 0 To dtSource.Rows.Count - 1
            Dim dr As DataRow = dtTarget.NewRow

            For j = 0 To dtSource.Columns.Count - 1
                Dim sColumnName As String = dtSource.Columns(j).ColumnName
                If IsNothing(dtTarget.Columns(sColumnName)) = False Then
                    dr(sColumnName) = dtSource.Rows(i)(sColumnName)
                End If
            Next
            dtTarget.Rows.Add(dr)
        Next

    End Sub

    ''' <summary>
    ''' 由 DataTable 轉換到 另一各 DTO DataTable 
    ''' </summary>
    ''' <param name="dtSource">來源資料表</param>
    ''' <param name="dtTarget">目的地資料表</param>
    ''' <param name="HasExceColumn">指定dtSource某各欄位 對應到dtTarget 的某各欄位</param>
    ''' <remarks></remarks>
    Public Sub TransferDataTable(ByVal dtSource As DataTable, ByRef dtTarget As DataTable, ByVal HasExceColumn As Hashtable)
        Dim i As Integer = 0
        Dim j As Integer = 0

        For i = 0 To dtSource.Rows.Count - 1
            Dim dr As DataRow = dtTarget.NewRow

            For j = 0 To dtSource.Columns.Count - 1
                Dim sColumnName As String = dtSource.Columns(j).ColumnName

                If IsNothing(dtTarget.Columns(sColumnName)) = False Then
                    dr(sColumnName) = dtSource.Rows(i)(sColumnName)

                    For Each de As DictionaryEntry In HasExceColumn
                        If de.Key.ToString().Trim().ToLower() = sColumnName.Trim().ToLower() Then
                            dr(de.Value.ToString().Trim()) = dtSource.Rows(i)(sColumnName)
                        End If
                    Next
                End If
            Next

            dtTarget.Rows.Add(dr)
        Next

    End Sub

    ''' <summary>
    ''' 由 DataTable 轉換到 另一各 DTO DataTable 
    ''' </summary>
    ''' <param name="dtSource">來源資料表</param>
    ''' <param name="dtTarget">目的地資料表</param>
    ''' <param name="HasExceColumn">指定dtSource某各欄位 對應到dtTarget 的某各欄位</param>
    ''' <param name="HasExceColumn_Decrypt">指定dtSource某各欄位 解密後 對應到dtTarget 的某各欄位</param>
    ''' <remarks></remarks>
    Public Sub TransferDataTable(ByVal dtSource As DataTable, ByRef dtTarget As DataTable, ByVal HasExceColumn As Hashtable, ByVal HasExceColumn_Decrypt As Hashtable)
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim i As Integer = 0
        Dim j As Integer = 0


        For i = 0 To dtSource.Rows.Count - 1
            Dim dr As DataRow = dtTarget.NewRow

            For j = 0 To dtSource.Columns.Count - 1
                Dim sColumnName As String = dtSource.Columns(j).ColumnName

                If IsNothing(dtTarget.Columns(sColumnName)) = False Then
                    dr(sColumnName) = dtSource.Rows(i)(sColumnName)

                    If HasExceColumn Is Nothing = False Then
                        For Each de As DictionaryEntry In HasExceColumn
                            If de.Key.ToString().Trim().ToLower() = sColumnName.Trim().ToLower() Then
                                dr(de.Value.ToString().Trim()) = dtSource.Rows(i)(sColumnName)
                            End If
                        Next
                    End If

                    If HasExceColumn_Decrypt Is Nothing = False Then
                        For Each de As DictionaryEntry In HasExceColumn_Decrypt
                            If de.Key.ToString().Trim().ToLower() = sColumnName.Trim().ToLower() Then
                                dr(de.Value.ToString().Trim()) = oCrypto.Decrypt(dtSource.Rows(i)(sColumnName), "")
                            End If
                        Next
                    End If
                End If

            Next
            dtTarget.Rows.Add(dr)
        Next

    End Sub

    ''' <summary>
    ''' 由 DataTable 轉換到 另一各 DTO DataTable 
    ''' </summary>
    ''' <param name="dtSource">來源資料表</param>
    ''' <param name="dtTarget">目的地資料表</param>
    ''' <remarks></remarks>
    Public Sub TransferData(ByVal dtSource As DataTable, ByRef dtTarget As DataTable)
        Dim reader As DataTableReader
        reader = dtSource.CreateDataReader
        dtTarget.Load(reader)
    End Sub

End Module


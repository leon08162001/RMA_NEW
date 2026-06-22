Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Newtonsoft.Json

Module Export
    Sub Main(args As String())
        Try
            If args.Length < 3 Then
                LogHelper.WriteLog("使用方式：RMA_ReportHost.exe <rptPath> <exportPath> <jsonPath>")
                Return
            End If

            Dim rptPath As String = args(0)
            Dim exportPath As String = args(1)
            Dim jsonPath As String = args(2)

            LogHelper.WriteLog($"🔹 報表路徑: {rptPath}")
            LogHelper.WriteLog($"🔹 輸出PDF: {exportPath}")
            LogHelper.WriteLog($"🔹 JSON資料: {jsonPath}")

            ' 🧩 Step 1 讀取 JSON
            Dim jsonText As String = File.ReadAllText(jsonPath)
            Dim ds As DataSet = JsonConvert.DeserializeObject(Of DataSet)(jsonText)
            LogHelper.WriteLog($"✅ 載入資料筆數: {ds.Tables.Count}")

            For i As Integer = 0 To ds.Tables.Count - 1
                If String.IsNullOrWhiteSpace(ds.Tables(i).TableName) Then
                    ds.Tables(i).TableName = "Table" & i
                End If
                LogHelper.WriteLog($"📄 {ds.Tables(i).TableName} Rows={ds.Tables(i).Rows.Count}")
            Next

            ' 🧩 Step 2 刪除暫存json檔
            If File.Exists(jsonPath) Then
                Try
                    File.Delete(jsonPath)
                    LogHelper.WriteLog("✅ 已刪除：" & jsonPath)
                Catch ex As Exception
                    LogHelper.WriteLog("⚠️ 無法刪除檔案：" & ex.Message)
                End Try
            End If

            ' 載入報表
            Dim rpt As New ReportDocument()
            rpt.Load(rptPath)

            ' 🧩 Step 3. 取得報表的 Table 名稱與欄位
            If rpt.Database.Tables.Count = 0 Then
                Throw New Exception("報表中沒有任何資料表 (請檢查 .rpt 的 Database Expert 設定)")
            End If

            Dim crTable = rpt.Database.Tables(0)
            LogHelper.WriteLog($"📄 報表資料表名稱: {crTable.Name}")

            LogHelper.WriteLog("📋 Crystal Report 欄位列表:")
            For Each field In crTable.Fields
                LogHelper.WriteLog($"    - {field.Name} ({field.ValueType})")
            Next

            ' 🧩 Step 4. 綁定資料來源
            rpt.SetDataSource(ds)

            ' 🧩 Step 5. 匯出 PDF
            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, exportPath)
            LogHelper.WriteLog($"✅ 匯出完成: {exportPath}")

        Catch ex As Exception
            LogHelper.WriteLog("❌ 發生錯誤：" & ex.ToString())
        End Try
    End Sub
End Module
Imports System.Configuration
Imports System.IO

Module ExecJOB
    Sub Main()
        Dim logPath As String = ConfigurationManager.AppSettings("logPath")
        If Not Directory.Exists(logPath) Then Directory.CreateDirectory(logPath)
        Dim logFile As String = Path.Combine(logPath, String.Format("ImportLog_{0:yyyyMMdd}.txt", DateTime.Now))

        Using writer As New StreamWriter(logFile, True)
            writer.WriteLine(String.Format("[{0:yyyy/MM/dd HH:mm:ss}] ▶ 程式啟動", DateTime.Now))


            Try
                ' 初始化 DAO
                Dim queryDAO As New QueryWatyBI_DAO()
                Dim importDAO As New ImportBI_DAO()

                ' 🧩 Step 1 撈資料
                writer.WriteLine("🔸開始撈取來源資料...")
                Dim LsWatyBIRsp As List(Of WarrantyBIRsp) = queryDAO.GetSourceData()
                writer.WriteLine(String.Format("🔹共撈取 {0} 筆資料", LsWatyBIRsp.Count))

                If LsWatyBIRsp.Count = 0 Then
                    writer.WriteLine("⚠ 無資料可匯入，結束作業。")
                    Return
                End If

                ' 🧩 Step 2 匯入 RMA
                writer.WriteLine("🔸開始匯入 RMA 資料庫...")
                Dim successCount As Integer = importDAO.InsertData(LsWatyBIRsp, writer)
                writer.WriteLine(String.Format("✅ 匯入完成，共成功 {0} 筆。", successCount))

            Catch ex As Exception
                writer.WriteLine(String.Format("❗ 系統錯誤：{0}", ex.Message))
            Finally
                writer.WriteLine(String.Format("[{0:yyyy/MM/dd HH:mm:ss}] ▶ 程式結束", DateTime.Now))
            End Try
        End Using
    End Sub
End Module

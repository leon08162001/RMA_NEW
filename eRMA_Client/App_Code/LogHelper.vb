Imports System.IO
Public Class LogHelper

    ''' <summary>
    ''' 寫入 Log 訊息
    ''' </summary>
    Public Shared Sub WriteLog(message As String)
        Try
            ' 使用虛擬路徑對應到實體路徑
            Dim logFolder As String = HttpContext.Current.Server.MapPath("~/App_Data/RMALogs")

            ' 資料夾不存在就建立
            If Not Directory.Exists(logFolder) Then
                Directory.CreateDirectory(logFolder)
            End If

            ' 每日產生一個檔案，例如 20250923.log
            Dim logFile As String = DateTime.Now.ToString("yyyyMMdd") & ".log"
            Dim fullPath As String = Path.Combine(logFolder, logFile)

            ' 舊版 VB.NET 字串串接
            Dim logMessage As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " | " & message

            ' 寫入檔案
            Dim sw As StreamWriter = Nothing
            Try
                sw = New StreamWriter(fullPath, True, Encoding.UTF8)
                sw.WriteLine(logMessage)
            Finally
                If sw IsNot Nothing Then sw.Close()
            End Try

        Catch ex As Exception
            ' Log 失敗不影響程式
            System.Diagnostics.Debug.WriteLine("寫入 Log 失敗：" & ex.Message)
        End Try
    End Sub
End Class
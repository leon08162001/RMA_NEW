<%@ WebHandler Language="VB" Class="ExportWarrantyExcel" %>

Imports System.IO
Imports System.Data
Imports NPOI.HSSF.UserModel

Public Class ExportWarrantyExcel
    Implements IHttpHandler
    Implements IRequiresSessionState

    Public Sub ProcessRequest(context As HttpContext) _
        Implements IHttpHandler.ProcessRequest

        Dim dtImport As DataTable =
            TryCast(context.Session("_dtImport"), DataTable)

        If dtImport Is Nothing OrElse dtImport.Rows.Count = 0 Then
            context.Response.End()
            Return
        End If

        Dim wb As New HSSFWorkbook()
        Dim sheet As HSSFSheet = wb.CreateSheet("Sheet1")

        Dim headerStyle As HSSFCellStyle = wb.CreateCellStyle()
        headerStyle.Alignment = CellHorizontalAlignment.CENTER
        headerStyle.VerticalAlignment = CellVerticalAlignment.CENTER

        ' ===== 標題列 =====
        Dim headerRow As HSSFRow = sheet.CreateRow(0)
        headerRow.Height = 20 * 20

        Dim col As Integer = 0
        headerRow.CreateCell(col).SetCellValue(" ")
        headerRow.GetCell(col).CellStyle = headerStyle

        col += 1 : headerRow.CreateCell(col).SetCellValue("SerialNo")
        col += 1 : headerRow.CreateCell(col).SetCellValue("CW Start")
        col += 1 : headerRow.CreateCell(col).SetCellValue("CW End")
        col += 1 : headerRow.CreateCell(col).SetCellValue("EW Start")
        col += 1 : headerRow.CreateCell(col).SetCellValue("EW End")
        col += 1 : headerRow.CreateCell(col).SetCellValue("WAR_TYPE")
        col += 1 : headerRow.CreateCell(col).SetCellValue("WAR_PROGRAM_TYPE")
        col += 1 : headerRow.CreateCell(col).SetCellValue("WAR_ITEM_TYPE")
        col += 1 : headerRow.CreateCell(col).SetCellValue("WAR_PRICE_VER")
        col += 1 : headerRow.CreateCell(col).SetCellValue("Model")
        col += 1 : headerRow.CreateCell(col).SetCellValue("SKU")
        col += 1 : headerRow.CreateCell(col).SetCellValue("Customer")
        col += 1 : headerRow.CreateCell(col).SetCellValue("CustomerName")
        col += 1 : headerRow.CreateCell(col).SetCellValue("ShipNo")
        col += 1 : headerRow.CreateCell(col).SetCellValue("DeliverDate")

        For i As Integer = 0 To col
            headerRow.GetCell(i).CellStyle = headerStyle
        Next

        ' ===== 資料列 =====
        For r As Integer = 0 To dtImport.Rows.Count - 1
            Dim row As HSSFRow = sheet.CreateRow(r + 1)
            row.Height = 15 * 20

            Dim c As Integer = 0
            row.CreateCell(c).SetCellValue((r + 1).ToString())

            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("SerialNo").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("CWStart").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("CWEnd").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("EWStart").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("EWEnd").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("WAR_TYPE").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("WAR_PROGRAM_TYPE").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("WAR_ITEM_TYPE").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("WAR_PRICE_VER").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("Model").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("SKU").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("Customer").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("CustomerName").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("ShipNo").ToString().Trim())
            c += 1 : row.CreateCell(c).SetCellValue(dtImport.Rows(r)("OrderDate").ToString().Trim())
        Next

        ' ===== 欄寬 =====
        For i As Integer = 0 To col
            sheet.AutoSizeColumn(i)
            sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + (5 * 256))
        Next

        ' ===== 輸出 =====
        Dim filename As String = DateTime.Now.ToString("yyyyMMddHHmmss") & ".xls"
        Dim response = context.Response

        response.Clear()
        response.Buffer = True
        response.ContentType = "application/vnd.ms-excel"
        response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))

        Using ms As New MemoryStream()
            wb.Write(ms)
            response.BinaryWrite(ms.ToArray())
        End Using

        response.Flush()
        context.ApplicationInstance.CompleteRequest()
    End Sub

    Public ReadOnly Property IsReusable As Boolean _
        Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

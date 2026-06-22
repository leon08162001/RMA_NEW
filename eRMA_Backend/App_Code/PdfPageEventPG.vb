Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class PdfPageEventPG
    Inherits PdfPageEventHelper

    Private _ShipNO As String = "*WARMA-1201000001*"
    Private _CustomerNO As String = "DT1231321"
    Private _PrintDate As String = "2022/05/05"
    Private _SwID As String = "WARMA-1201000001"
    Private _Addr As String = "Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C."
    Private _Tel As String = "Tel: +886 2 86471166 Fax: +886 2 87323300 e - mail: salesadmin @cipherlab.com.tw"
    Private sFontPath As String = ConfigurationManager.AppSettings("FontPath")
    Private sLogoPath As String = ConfigurationManager.AppSettings("LogoPath")
    Private _WaterMarkPic As String = ConfigurationManager.AppSettings("WaterMarkPic")
    Private _HeaderText As String = ""
    Private _bPDFPage As Integer
    Private font8 As Font
    Private font10 As Font
    Private font14 As Font
    Private bfBar24 As Font
    Private presentFontSize As Integer = 12
    Private pageSize As Rectangle = iTextSharp.text.PageSize.A4
    Private headerTemplate As PdfTemplate
    Private footerTemplate As PdfTemplate
    Private bf As BaseFont = Nothing
    Private bfBar As BaseFont = Nothing
    Private fontDetail As Font = Nothing
    Private _Title As String
    Private _HeaderRight As String
    Private _HeaderLeft As String

    Public Property bPDFPage As Integer
        Get
            Return _bPDFPage
        End Get
        Set(ByVal value As Integer)
            _bPDFPage = value
        End Set
    End Property
    Public Property SwID As String
        Get
            Return _SwID
        End Get
        Set(ByVal value As String)
            _SwID = value
        End Set
    End Property
    Public Property Addr As String
        Get
            Return _Addr
        End Get
        Set(ByVal value As String)
            _Addr = value
        End Set
    End Property
    Public Property Tel As String
        Get
            Return _Tel
        End Get
        Set(ByVal value As String)
            _Tel = value
        End Set
    End Property
    Public Property ShipNO As String
        Get
            Return _ShipNO
        End Get
        Set(ByVal value As String)
            _ShipNO = value
        End Set
    End Property
    Public Property CustomerNO As String
        Get
            Return _CustomerNO
        End Get
        Set(ByVal value As String)
            _CustomerNO = value
        End Set
    End Property
    Public Property PrintDate As String
        Get
            Return _PrintDate
        End Get
        Set(ByVal value As String)
            _PrintDate = value
        End Set
    End Property
    Public Property eaderText As String
        Get
            Return _HeaderText
        End Get
        Set(ByVal value As String)
            _HeaderText = value
        End Set
    End Property
    Public Property Title As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property
    Public Property HeaderLeft As String
        Get
            Return _HeaderLeft
        End Get
        Set(ByVal value As String)
            _HeaderLeft = value
        End Set
    End Property

    Public Property HeaderRight As String
        Get
            Return _HeaderRight
        End Get
        Set(ByVal value As String)
            _HeaderRight = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal yeMei As String, ByVal presentFontSize As Integer, ByVal pageSize As Rectangle)
        Me._HeaderText = yeMei
        Me.presentFontSize = presentFontSize
        Me.pageSize = pageSize
    End Sub

    Public Sub SetHeader(ByVal header As String)
        Me._HeaderText = header
    End Sub

    Public Sub SetPresentFontSize(ByVal presentFontSize As Integer)
        Me.presentFontSize = presentFontSize
    End Sub

    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        footerTemplate = writer.DirectContent.CreateTemplate(50, 50)
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)

        Try
            Dim img As Image = Image.GetInstance(_WaterMarkPic)
            img.Alignment = Image.UNDERLYING
            img.SetAbsolutePosition(50, 120)
            document.Add(img)

        Catch e As BadElementException
        Catch e As IOException
        Catch e As DocumentException
        End Try

    End Sub

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        Try

            If bf Is Nothing Then
                bf = BaseFont.CreateFont(sFontPath & "\msjhbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
                bfBar = BaseFont.CreateFont(sFontPath & "\FRE3OF9X.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
                font8 = New Font(bf, 8, 0)
                font10 = New Font(bf, 10, 0)
                font14 = New Font(bf, 14, 0)
                bfBar24 = New Font(bfBar, 24, 0)
            End If

            If fontDetail Is Nothing Then
                fontDetail = New Font(bf, presentFontSize, Font.NORMAL)
            End If

        Catch e As DocumentException
        Catch e As IOException
        End Try

        Dim pageSize As Rectangle = document.PageSize  '595*842

        'If _HeaderText <> String.Empty AndAlso bPDFPage >= writer.PageNumber Then
        If _HeaderText <> String.Empty Then
            writer.DirectContent.BeginText()
            writer.DirectContent.SetFontAndSize(bf, 8)
            writer.DirectContent.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40))
            writer.DirectContent.ShowTextAligned(2, _PrintDate, 550, 810, 0)
            writer.DirectContent.EndText()
            Dim headerTable As PdfPTable = New PdfPTable(2)
            headerTable.SetWidths(New Integer() {1, 3})
            headerTable.TotalWidth = 528
            headerTable.LockedWidth = True
            headerTable.DefaultCell.FixedHeight = 40
            headerTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER
            headerTable.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY
            headerTable.DefaultCell.Border = PdfPCell.NO_BORDER
            Dim Jjpg As Image = Image.GetInstance(sLogoPath & "\PdfLogo.jpg")
            Jjpg.Alignment = Image.ALIGN_TOP
            Jjpg.ScalePercent(70.0F)
            headerTable.AddCell(Jjpg)
            Dim headerTableCell As PdfPCell = New PdfPCell()
            headerTableCell.PaddingBottom = 15
            headerTableCell.PaddingLeft = 60
            headerTableCell.Border = Rectangle.BOTTOM_BORDER
            headerTableCell.BorderColor = BaseColor.LIGHT_GRAY
            headerTableCell.AddElement(New Phrase("CipherLab Co., Ltd.", New Font(Font.FontFamily.HELVETICA, 16)))
            headerTableCell.AddElement(New Phrase("      Service Advantage", New Font(Font.FontFamily.HELVETICA, 12)))
            headerTableCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerTableCell.VerticalAlignment = Element.ALIGN_CENTER
            headerTableCell.Border = PdfPCell.NO_BORDER
            headerTable.AddCell(headerTableCell)
            Dim BarCodeTable As PdfPTable = New PdfPTable(1)
            BarCodeTable.TotalWidth = 528
            BarCodeTable.LockedWidth = True
            BarCodeTable.DefaultCell.Border = PdfPCell.NO_BORDER
            Dim BarcodeCell As PdfPCell = New PdfPCell(New Paragraph("*" + ShipNO + "*", bfBar24))
            BarcodeCell.VerticalAlignment = Element.ALIGN_MIDDLE
            BarcodeCell.HorizontalAlignment = Element.ALIGN_RIGHT
            BarcodeCell.Border = PdfPCell.NO_BORDER
            BarCodeTable.AddCell(BarcodeCell)
            Dim t00 As PdfPTable = New PdfPTable(2)
            t00.TotalWidth = 528
            t00.LockedWidth = True
            t00.DefaultCell.BorderWidth = 0
            t00.SetWidths(New Single() {1.0F, 1.0F})
            Dim sStr As String = "CUSTOMER: " + CustomerNO
            Dim cell As PdfPCell = New PdfPCell(New Phrase(sStr, font8))
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            cell.Border = PdfPCell.BOTTOM_BORDER
            cell.FixedHeight = 20.0F
            t00.AddCell(cell)
            sStr = "No: " + SwID
            cell = New PdfPCell(New Phrase(sStr, font8))
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            cell.HorizontalAlignment = 2
            cell.Border = PdfPCell.BOTTOM_BORDER
            cell.FixedHeight = 20.0F
            t00.AddCell(cell)
            headerTable.WriteSelectedRows(0, -1, document.Left + 10, document.PageSize.Height - 36, writer.DirectContent)
            BarCodeTable.WriteSelectedRows(0, -1, document.Left + 10, document.PageSize.Height - 90, writer.DirectContent)
            t00.WriteSelectedRows(0, -1, document.Left + 10, document.PageSize.Height - headerTable.CalculateHeights() - 60, writer.DirectContent)


            'Dim pageS As Integer = writer.PageNumber
            'Dim foot1 As String = "第 " & pageS & " 頁 /共"
            'Dim footer As Phrase = New Phrase(foot1, fontDetail)
            'Dim len As Single = bf.GetWidthPoint(foot1, presentFontSize)
            Dim cb As PdfContentByte = writer.DirectContent
            'ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, footer, (document.RightMargin + document.Right + document.LeftMargin - document.Left - len) / 2.0F + 20.0F, document.Bottom - 20, 0)
            'cb.AddTemplate(footerTemplate, (document.RightMargin + document.Right + document.LeftMargin - document.Left) / 2.0F + 20.0F, document.Bottom - 20)

            cb.SetLineWidth(1.0F)
            cb.SetLineDash(2, 2, 0)
            cb.MoveTo(20, document.PageSize.GetBottom(40))
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(40))
            cb.Stroke()

            cb.BeginText()
            cb.SetFontAndSize(bf, 8)
            cb.SetTextMatrix(document.PageSize.GetRight(575), document.PageSize.GetBottom(30))
            cb.ShowText(_Addr)
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(bf, 8)
            cb.SetTextMatrix(document.PageSize.GetRight(575), document.PageSize.GetBottom(20))
            cb.ShowText(_Tel)
            cb.EndText()
        End If

    End Sub

    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)

        'footerTemplate.BeginText()
        'footerTemplate.SetFontAndSize(bf, presentFontSize)
        'Dim foot2 As String = " " & (writer.PageNumber) & " 頁"
        'footerTemplate.ShowText(foot2)
        'footerTemplate.EndText()
        'footerTemplate.ClosePath()
    End Sub

End Class


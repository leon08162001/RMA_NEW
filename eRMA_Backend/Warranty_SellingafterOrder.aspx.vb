Imports System.Data
Imports System.Data.OracleClient
Imports System.IO
Imports DataService
Imports DefLanguage
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class Warranty_SellingafterOrder
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

    '所有產出報表的位置
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _pdfSample As String = ConfigurationSettings.AppSettings("Requested_ExcelSample_VisualPath")
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim sFontPath As String = ConfigurationSettings.AppSettings("FontPath")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SortExpression") = "waty_no"
            Me.ViewState("_SortDirection") = "desc"

            Me.ViewState("_OperationCenter") = ""
            Me.ViewState("_SearchStart") = ""
            Me.ViewState("_SearchEnd") = ""
            Me.ViewState("_WarrantyNo") = ""
            Me.ViewState("_ERPNo") = ""
            Me.ViewState("_Customer") = ""

            Session("_dtTmp") = Nothing

            Dim oWarranty As New ctlWarranty
            Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
            dtProductGroup = oWarranty.QueryPrdGroup("", "")
            ViewState("_dtProductGroup") = dtProductGroup

            Call setControls()
            Call QueryData(0)

            If Session("_UserID").ToString.ToUpper = "ADMIN" Then
                Services.Visible = True
            Else
                Services.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()

        Call oCommon.getCostCenterByDropDownList(False, Me.UI_CboOperationCenter, "==SELECT==")

        '取得Tag Text
        'Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "021", ctlLanguage.eumType.Tag)
        Me.lblOperationCenter.Text = "Operation Center"
        Me.lblSearchPeriod.Text = "Search Period:"
        Me.lblWarrantyNo.Text = "Warranty No"
        Me.lblErpNo.Text = " ERP No."
        Me.lblCustomer.Text = "Customer:"

        Me.cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        'Me.UI_cmdProductGroupPick.Text = _oLanguage.getText("Warranty", "040", ctlLanguage.eumType.Tag)


        'Me.UI_dvSales.Columns(1).HeaderText = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(2).HeaderText = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(3).HeaderText = _oLanguage.getText("Warranty", "030", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(4).HeaderText = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(5).HeaderText = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(6).HeaderText = _oLanguage.getText("Warranty", "050", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(7).HeaderText = _oLanguage.getText("Warranty", "051", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(8).HeaderText = _oLanguage.getText("Warranty", "049", ctlLanguage.eumType.Tag)
        'Me.UI_dvSales.Columns(9).HeaderText = _oLanguage.getText("Warranty", "034", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.WARRANTYORDDataTable

        Dim sOperationCenter As String = Me.ViewState("_OperationCenter").ToString().Trim()
        Dim sSearchStart As String = Me.ViewState("_SearchStart").ToString().Trim()
        Dim sSearchEnd As String = Me.ViewState("_SearchEnd").ToString().Trim()
        Dim sWarrantyNo As String = Me.ViewState("_WarrantyNo").ToString().Trim()
        Dim sERPNo As String = Me.ViewState("_ERPNo").ToString().Trim()
        Dim sCustomer As String = Me.ViewState("_Customer").ToString().Trim()

        dtData = oWarranty.QueryWorder(sWarrantyNo, sOperationCenter, sCustomer, sSearchStart, sSearchEnd, sERPNo, "")

        Call ArrangementData(dtData)

        Dim dtTmp As DataTable = Session("_dtTmp")
        Dim dvTmp As DataView = dtTmp.DefaultView

        Call RMA_DataBind(dvTmp, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dtData As WarrantyDTO.WARRANTYORDDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim sRMANo As String = ""

        For i = 0 To dtData.Rows.Count - 1
            Dim dr As WarrantyDTO.WARRANTYORDRow = dtData.Rows(i)
            iCount = iCount + 1
            dr.SEQID = iCount
            dr("SEQID") = iCount
            If dr.ISConfirm = "N" Then
                dr.FlowName = "Open"
            Else
                If dr.WATY_FLOW = "F" Then
                    dr.FlowName = "Flow"
                Else
                    dr.FlowName = "Confirmed"
                End If
            End If

        Next

        Session("_dtTmp") = dtData
    End Sub

    Private Sub RMA_DataBind(ByVal dvTmp As DataView, ByVal iPageIndex As Integer)
        dvTmp.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvSales.PageSize = _PageSize
        Me.UI_dvSales.PageIndex = iPageIndex
        Me.UI_dvSales.DataSource = dvTmp
        Me.UI_dvSales.DataBind()
    End Sub

    Protected Sub UI_dvSales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSales.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            Dim lblConfirm As Label = e.Row.FindControl("lblConfirm")
            Dim lblFlow As Label = e.Row.FindControl("lblFlow")

            UI_SeqID.Text = (Me.UI_dvSales.PageIndex * Me.UI_dvSales.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdDele As Button = e.Row.FindControl("UI_cmdDele")

            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            If lblConfirm.Text.Trim() = "N" Then
                UI_cmdEdit.Text = "Edit"
                UI_cmdDele.Visible = True
            Else
                If lblFlow.Text.Trim() = "N" Then
                    UI_cmdEdit.Text = "Detail"
                    UI_cmdDele.Visible = False
                Else
                    UI_cmdEdit.Text = "Detail"
                    UI_cmdDele.Visible = False
                End If
            End If
        End If


        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If

    End Sub

    Protected Sub UI_dvSales_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSales.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtTmp") Is Nothing Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand

        If e.CommandName = "cmdEdit" Or e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim lblSwID As Label = row.FindControl("lblSwID")
            Dim lblCompno As Label = row.FindControl("lblCompno")

            Me.lblPreviousPage_SwID.Text = lblSwID.Text.Trim
            Me.lblPreviousPage_Compno.Text = lblCompno.Text.Trim
        End If
        If e.CommandName = "cmdDel" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim lblSwID As Label = row.FindControl("lblSwID")

            Dim sMessage As String = ""
            Dim blnFlag As Boolean = False
            Try
                Dim oWarranty As New ctlWarranty
                Dim dtWorder As New WarrantyDTO.WARRANTYORDDataTable
                Dim dr As WarrantyDTO.WARRANTYORDRow = dtWorder.NewWARRANTYORDRow
                dr.WATY_NO = lblSwID.Text
                dr.ISConfirm = "Y"
                dr.WATY_LUAD = Session("_UserID")
                dr.WATY_LUADNAME = Session("_UserName")
                dr.WATY_LUSTMP = Date.Now
                dr.WATY_MARK = 1
                dtWorder.AddWARRANTYORDRow(dr)
                oWarranty.SaveEditWarrantyOrd(dtWorder, False)
                blnFlag = True
            Catch ex As Exception
                sMessage = ex.Message
                blnFlag = False
            Finally
                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                Else
                    Call QueryData(0)
                End If
            End Try

        End If

    End Sub

    Protected Sub UI_dvSales_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvSales.Sorting

        If Me.ViewState("_SortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_SortDirection") = "asc"
        Else
            If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_SortDirection") = "desc"
            Else
                Me.ViewState("_SortDirection") = "asc"
            End If
        End If
        Me.ViewState("_SortExpression") = e.SortExpression

        If IsNothing(Session("_dtTmp")) = False Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvSales.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvSales.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvSales.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvSales.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If UI_CboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.UI_CboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        Me.ViewState("_SearchStart") = txtStart.Text
        Me.ViewState("_SearchEnd") = txtEnd.Text
        Me.ViewState("_WarrantyNo") = txtWarrantyNo.Text
        Me.ViewState("_ERPNo") = txtErpNo.Text
        Me.ViewState("_Customer") = txtCustomer.Text

        Call QueryData(0)

    End Sub
    'Protected Sub UI_cmdProductGroupPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdProductGroupPick.Click
    '    Dim dtProductGroup As DataTable = Me.ViewState("_dtProductGroup")
    '    Me.ucProductGroup.show(dtProductGroup, True)
    'End Sub
    Protected Sub Services_Click(sender As Object, e As EventArgs) Handles Services.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim doc As Document = New Document(PageSize.A4, 20, 20, 140, 40)
        Try
            Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtMData As New DataTable
            Dim iLine As Integer = 10
            dtMData = oWarranty.QueryServicesPrint(sSwID)

            If dtMData.Rows.Count > 0 Then

                Dim ms As New MemoryStream()
                'Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
                Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.Create))
                Dim PageEventHandler As PdfPageEventPG = New PdfPageEventPG()

                writer.CloseStream = False
                writer.PageEvent = PageEventHandler
                PageEventHandler.SetHeader("This Is Hearder")
                writer.PdfVersion = PdfWriter.VERSION_1_7
                writer.SetPdfVersion(PdfName.VERSION)

                Dim sStr As String = dtMData.Rows(0)("SYSDATE").ToString()
                If sStr <> "" Then
                    sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                    PageEventHandler.PrintDate = sStr
                End If

                PageEventHandler.SwID = sSwID
                PageEventHandler.ShipNO = dtMData.Rows(0)("WarrantyCard_no").ToString()
                PageEventHandler.CustomerNO = dtMData.Rows(0)("OCC18").ToString()
                PageEventHandler.Addr = "Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C."
                PageEventHandler.Tel = "Tel: +886 2 86471166  Fax: +886 2 87323300   e - mail: salesadmin @cipherlab.com.tw"

                doc.Open()

                Dim cb As PdfContentByte = writer.DirectContent
                Dim ct As ColumnText = New ColumnText(cb)

                Dim Bfont As BaseFont = BaseFont.CreateFont(sFontPath + "\msjh.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
                Dim font9 As Font = New Font(Bfont, 9, 0)

                'Phrase helloPhrase = New Phrase();            
                'Font helloFont = New iTextSharp.text.Font(chBaseFont, 14);

                Dim t01 As PdfPTable = New PdfPTable(4)
                t01.TotalWidth = 523
                t01.LockedWidth = True
                t01.DefaultCell.BorderWidth = 0
                t01.SetWidths(New Integer() {1, 2, 1, 2})

                sStr = "ITEM NO."
                'Dim TitleCell As PdfPCell = New PdfPCell(New Phrase(sStr, New Font(Font.FontFamily.HELVETICA, iFontSize)))
                Dim TitleCell As PdfPCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.Border = PdfPCell.BOTTOM_BORDER
                TitleCell.FixedHeight = 20.0F
                t01.AddCell(TitleCell)

                sStr = "DESCRIPTION" '+ Chr(13) + "aaaAAA123"
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(TitleCell)

                sStr = "Q’TY"
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.HorizontalAlignment = Element.ALIGN_CENTER
                TitleCell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(TitleCell)

                sStr = "WARRANTY DETAIL"
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(TitleCell)

                Dim t02 As PdfPTable = New PdfPTable(4) 'SerialNO
                Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
                dtData = oWarranty.QueryWarrantyItem(sSwID, -1, "")

                Dim MVPdf As Boolean = False
                Dim BatteryPdf As Boolean = False

                Dim i As Integer = 0
                For i = 0 To dtMData.Rows.Count - 1
                    Dim dr As DataRow = dtMData.Rows(i)

                    If (dr("WarrantyCard_type").ToString() = "PB" Or dr("WarrantyCard_type").ToString() = "EB") Then
                        BatteryPdf = True
                    End If
                    If dr("WarrantyCard_type").ToString() = "MV" Then
                        MVPdf = True
                    End If

                    Dim dtWarrantySerial As New DataTable
                    dtWarrantySerial = oWarranty.QueryServicesSN(dr("WarrantyCard_NO").ToString(), dr("WarrantyCard_SKUNO").ToString())

                    'SKU
                    sStr = dr("WarrantyCard_SKUNO").ToString()
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    TitleCell.BorderWidth = 0
                    TitleCell.FixedHeight = 18.0F
                    t01.AddCell(TitleCell)

                    'DESC
                    sStr = runFnGetImc04_1(dr("WarrantyCard_SKUNO").ToString(), dtMData.Rows(0)("OCC18").ToString()).Replace(",", Environment.NewLine)
                    '  If Not dr.IsWATI_SKUDESCNull Then
                    'sStr = dr.WATI_SKUDESC
                    'End If
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    TitleCell.BorderWidth = 0
                    t01.AddCell(TitleCell)

                    'sStr = runFnGetImc04_1(dr("WarrantyCard_SKUNO").ToString(), dtMData.Rows(0)("OCC18").ToString()).Replace(",", Environment.NewLine)
                    ''  If Not dr.IsWATI_SKUDESCNull Then
                    ''sStr = dr.WATI_SKUDESC
                    ''End If
                    'cell = New PdfPCell(New Phrase(sStr, ftStd))
                    'cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    'cell.BorderWidth = 0
                    't01.AddCell(cell)

                    sStr = dr("WarrantyCard_QTY").ToString()
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.HorizontalAlignment = 1
                    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    TitleCell.BorderWidth = 0
                    t01.AddCell(TitleCell)

                    'Name
                    sStr = dr("WarrantyCard_NAME").ToString()
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    TitleCell.BorderWidth = 0
                    t01.AddCell(TitleCell)

                    '空白一行 
                    sStr = " "
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    TitleCell.HorizontalAlignment = 1
                    TitleCell.BorderWidth = 0
                    TitleCell.Colspan = 4
                    TitleCell.FixedHeight = 10.0F
                    t01.AddCell(TitleCell)

                    Dim j As Integer = 0
                    For j = 0 To dtWarrantySerial.Rows.Count - 1
                        Dim drs As DataRow = dtWarrantySerial.Rows(j)

                        sStr = drs("WarrantyCard_SN").ToString
                        TitleCell = New PdfPCell(New Phrase(sStr, font9))
                        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                        TitleCell.BorderWidth = 0
                        t02.AddCell(TitleCell)

                    Next

                    For j = 1 To 4 - (dtWarrantySerial.Rows.Count Mod 4)
                        sStr = " "
                        TitleCell = New PdfPCell(New Phrase(sStr, font9))
                        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                        TitleCell.BorderWidth = 0
                        t02.AddCell(TitleCell)
                    Next


                    '空白一行 
                    sStr = " "
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                    TitleCell.BorderWidth = 0
                    TitleCell.Colspan = 4
                    TitleCell.FixedHeight = 10.0F
                    t02.AddCell(TitleCell)

                    '特殊簽呈
                    'If Not dr.IsWAR_SPEC_DESCNull Then
                    '    sStr = " "
                    '    sStr = dr.WAR_SPEC_DESC
                    '    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    '    TitleCell.VerticalAlignment = Element.ALIGN_TOP
                    '    TitleCell.BorderWidth = 0
                    '    TitleCell.Colspan = 4
                    '    t02.AddCell(TitleCell)
                    'End If

                    '空白一行 
                    sStr = " "
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                    TitleCell.BorderWidth = 0
                    TitleCell.Colspan = 4
                    TitleCell.FixedHeight = 25.0F
                    t02.AddCell(TitleCell)

                    doc.Add(t01)
                    doc.Add(t02)
                    t01.FlushContent()
                    t02.FlushContent()

                Next


                writer.CloseStream = True

                doc.Close()
                writer.Close()

                '合併PDF
                Dim PDFFiles As List(Of String) = New List(Of String)()
                PDFFiles.Add(_Reoprt_FilePath + sSwID + ".pdf")
                PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_Warranty_Policy_EN.pdf")
                PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_Warranty_Policy_TC.pdf")
                If BatteryPdf Then
                    PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Battery_Insurance_Policy_EN.pdf")
                    PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Battery_Insurance_Policy_TC.pdf")
                End If
                If MVPdf Then
                    PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_of_Material_Warranty_Policy_EN.pdf")
                    PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_of_Material_Warranty_Policy_TC.pdf")
                End If

                Dim sFileName As String = sSwID + "N.pdf"

                iTextSharpPdfMerge(PDFFiles, _Reoprt_FilePath + "\\" + sFileName)

                If File.Exists(_Reoprt_FilePath + sSwID + ".pdf") Then
                    File.Delete(_Reoprt_FilePath + sSwID + ".pdf")
                End If

                Dim sScript As String = ""
                sScript = sScript & "<script language=""javascript"">" & vbCrLf
                sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & sSwID & "N.pdf" & "','','');" & vbCrLf
                sScript = sScript & "</script>" & vbCrLf
                Response.Write(sScript)

            End If

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            doc.Close()
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    Private Sub iTextSharpPdfMerge(ByVal inFiles As List(Of String), ByVal outFile As String)
        Using stream = New FileStream(outFile, FileMode.Create)

            Using doc = New Document()

                Using pdf = New PdfCopy(doc, stream)
                    doc.Open()
                    For Each file As String In inFiles
                        Dim reader = New PdfReader(file)

                        For i As Integer = 0 To reader.NumberOfPages - 1
                            Dim page = pdf.GetImportedPage(reader, i + 1)
                            pdf.AddPage(page)
                        Next

                        pdf.FreeReader(reader)
                        reader.Close()
                    Next

                End Using
            End Using
        End Using
    End Sub

    Private Function PdfTable() As PdfPTable
        Dim t01 As New PdfPTable(5)
        t01.TotalWidth = 523
        t01.LockedWidth = True
        t01.DefaultCell.BorderWidth = 0
        t01.SetWidths(New Integer() {2, 3, 1, 2, 4})

        Dim sStr As String = ""
        Dim bf As BaseFont = BaseFont.CreateFont(sFontPath & "\msjhbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        Dim ftStd As New iTextSharp.text.Font(bf, 8, 0)
        Dim cell As New PdfPCell(New Phrase(sStr, ftStd))

        sStr = "ITEM NO."
        cell = New PdfPCell(New Phrase(sStr, ftStd))
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.FixedHeight = 20.0F
        t01.AddCell(cell)

        sStr = "DESCRIPTION"
        cell = New PdfPCell(New Phrase(sStr, ftStd))
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(cell)

        sStr = "Q’TY"
        cell = New PdfPCell(New Phrase(sStr, ftStd))
        cell.HorizontalAlignment = 1
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(cell)

        sStr = "SERIAL NO."
        cell = New PdfPCell(New Phrase(sStr, ftStd))
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(cell)

        sStr = "WARRANTY DETAIL"
        cell = New PdfPCell(New Phrase(sStr, ftStd))
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(cell)

        Return t01
    End Function

    Private Function PdfFooter(cb As PdfContentByte) As PdfTemplate
        Dim f_cn As BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)

        Dim tmpFooter As PdfTemplate = cb.CreateTemplate(580, 70)
        'Move to the bottom left corner of the template
        tmpFooter.MoveTo(1, 1)
        ' Place the footer content
        tmpFooter.Stroke()
        ' Begin writing the footer
        tmpFooter.BeginText()
        ' Set the font And size
        tmpFooter.SetFontAndSize(f_cn, 12)
        ' Write out details from the payee table
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C.", 0, 50, 0)
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tel: +886 2 27303066      Fax: +886 2 87323300      e-mail: salesadmin@cipherlab.com.tw", 0, 37, 0)

        ' End text
        tmpFooter.EndText() '
        ' Stamp a line above the page footer
        cb.SetLineWidth(0F) '
        cb.MoveTo(30, 60)
        cb.LineTo(570, 60)
        cb.Stroke()

        Return tmpFooter

    End Function

    '取得額外規格於PDF上顯示 MODI BY ANGEL ON 20160118
    Private Function runFnGetImc04_1(ByVal SKUNO_IN As String, ByVal CUSTID_IN As String) As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        ' Throw New Exception(SKUNO_IN)
        Try

            oCommand.CommandText = "FnGetImc04_1"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("vima01", OracleType.NVarChar).Value = SKUNO_IN
            oCommand.Parameters("vima01").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vocc01", OracleType.NVarChar).Value = CUSTID_IN
            oCommand.Parameters("vocc01").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 4000)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

    Public Sub WatermarkAdd(ByVal inputPath As String, ByVal outputPath As String, ByVal watermarkPath As String)
        Try
            Dim pdfReader As New PdfReader(inputPath)
            Dim numberOfPages As Integer = pdfReader.NumberOfPages
            Dim outputStream As New FileStream(outputPath, FileMode.Create)
            Dim pdfStamper As New PdfStamper(pdfReader, outputStream)
            Dim waterMarkContent As PdfContentByte

            Dim watermarkimagepath As String = watermarkPath
            Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(watermarkimagepath)

            image.SetAbsolutePosition(50, 120)
            For i As Integer = 1 To numberOfPages
                If i = 1 Then
                    waterMarkContent = pdfStamper.GetUnderContent(i)
                    waterMarkContent.AddImage(image)
                End If
            Next
            pdfStamper.Close()
            pdfReader.Close()
        Catch ex As Exception
            'WriteLog.Log(ex.ToString())
            Throw ex
        End Try
    End Sub

    Protected Sub ImgBtnStart_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnStart.Click
        Calend_Start.Visible = True
    End Sub
    Protected Sub ImgBtnEnd_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnEnd.Click
        Calend_End.Visible = True
    End Sub
    Protected Sub Calend_Start_SelectionChanged(sender As Object, e As EventArgs) Handles Calend_Start.SelectionChanged
        txtStart.Text = Calend_Start.SelectedDate.ToString("yyyy/MM/dd")
        Calend_Start.Visible = False
    End Sub
    Protected Sub Calend_End_SelectionChanged(sender As Object, e As EventArgs) Handles Calend_End.SelectionChanged
        txtEnd.Text = Calend_End.SelectedDate.ToString("yyyy/MM/dd")
        Calend_End.Visible = False
    End Sub

End Class


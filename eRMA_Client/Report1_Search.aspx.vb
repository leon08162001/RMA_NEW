Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel

Partial Class Report1_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _RepairBOM_No As String = ConfigurationSettings.AppSettings("RepairBOM_No")

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function

    Public Function getoLanguageword(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Word)
    End Function

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            UI_cmdExport.Visible = False
            UI_ExportWithPic.Visible = False
            Session("_dtReport") = Nothing

            Call setControls()
            Call Query_PrimalModelNo()      '原型機種
            Me.right_component.Visible = False
            Me.erma_infomation_number_count_lab.Visible = False
        End If
        If Session("_UserID").ToString().ToUpper() = "ADMIN" Then
            UI_ExportAll.Visible = True
        Else
            UI_ExportAll.Visible = False
        End If

        '客製化 th 顯示框
        'Me.Report_101_lab.Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
        'Me.Report_152_lab.Text = _oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag)
        'Me.Report_161_lab.Text = _oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag)
        'Me.Report_191_lab.Text = _oLanguage.getText("Report", "191", ctlLanguage.eumType.Tag)
        'Me.Report_162_lab.Text = _oLanguage.getText("Report", "162", ctlLanguage.eumType.Tag)
        'Me.Report_163_lab.Text = _oLanguage.getText("Report", "163", ctlLanguage.eumType.Tag)
        'Me.Report_164_lab.Text = _oLanguage.getText("Report", "164", ctlLanguage.eumType.Tag)
        'Me.UI_lblUpperPartsNo3.Text = ""



    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "001", ctlLanguage.eumType.Tag)

        '初始化
        Me.UI_cboPrimalModelNo_Panel.Visible = False
        Me.UI_txtProduct_SerialNo_Panel.Visible = False
        Me.UI_txtPartsNo_Panel.Visible = False
        Me.UI_cboPrimalModelNo_Panel.Visible = True

        Down.Items.Clear()

        Dim UI_lblPrimalModelNo_ListItem As New ListItem
        Dim UI_lblProduct_SerialNo_ListItem As New ListItem
        Dim UI_lblPartsNo_ListItem As New ListItem

        UI_lblPrimalModelNo_ListItem.Value = 0
        UI_lblPrimalModelNo_ListItem.Text = _oLanguage.getText("Report", "149", ctlLanguage.eumType.Tag)

        UI_lblProduct_SerialNo_ListItem.Value = 1
        UI_lblProduct_SerialNo_ListItem.Text = _oLanguage.getText("Report", "151", ctlLanguage.eumType.Tag)

        UI_lblPartsNo_ListItem.Value = 2
        UI_lblPartsNo_ListItem.Text = _oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag)
        Me.lblInformation.Text = _oLanguage.getText("RMA2", "103", ctlLanguage.eumType.Tag)

        Down.Items.Add(UI_lblPrimalModelNo_ListItem)
        Down.Items.Add(UI_lblProduct_SerialNo_ListItem)
        Down.Items.Add(UI_lblPartsNo_ListItem)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdReset.Text = _oLanguage.getText("RMA2", "009", ctlLanguage.eumType.Tag)
        Me.UI_txtProduct_SerialNo.Attributes.Add("placeholder", _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag))
        Me.UI_txtPartsNo.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "006", ctlLanguage.eumType.Tag))
    End Sub

    ''' <summary>
    ''' 匯出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdExport.Click

        If Not Session("_dtReport") Is Nothing Then

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""
            Dim hssfworkbook As New HSSFWorkbook
            Dim style As HSSFCellStyle = hssfworkbook.CreateCellStyle()
            Dim format As HSSFDataFormat = hssfworkbook.CreateDataFormat


            Try
                Dim dtReport As ReportDTO.Rpt_BOMDataTable = Session("_dtReport")
                Dim dvReport As DataView = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile").DefaultView
                Dim sheet1 As HSSFSheet = hssfworkbook.CreateSheet("Sheet1")
                Dim iCount As Integer = 0

                If dvReport.Count > 0 Then
                    Dim row As HSSFRow = sheet1.CreateRow(0)
                    row.Height = 20 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue("")
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "191", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    row.GetCell(iCount).CellStyle = style

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "162", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    'row.GetCell(iCount).CellStyle = style

                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(_oLanguage.getText("Report", "163", ctlLanguage.eumType.Tag).Replace("<br>", " "))
                    'row.GetCell(iCount).CellStyle = style
                End If


                For i = 0 To dvReport.Count - 1
                    Dim dr As DataRow = dvReport(i).Row
                    Dim row As HSSFRow = sheet1.CreateRow(i + 1)
                    row.Height = 15 * 20

                    style = hssfworkbook.CreateCellStyle()
                    style.Alignment = CellHorizontalAlignment.CENTER
                    style.VerticalAlignment = CellVerticalAlignment.CENTER

                    iCount = 0
                    row.CreateCell(iCount).SetCellValue((i + 1).ToString())
                    row.GetCell(iCount).CellStyle = style

                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("EXPORT_MODELNO").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("bmb03").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RPBOM_DESC").ToString().Trim())
                    iCount = iCount + 1
                    row.CreateCell(iCount).SetCellValue(dr("RPBOM_MATERIALCOST").ToString().Trim())
                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dr("Substitute1").ToString().Trim())
                    'iCount = iCount + 1
                    'row.CreateCell(iCount).SetCellValue(dr("Substitute2").ToString().Trim())
                Next

                For i = 0 To 40
                    sheet1.AutoSizeColumn(i)
                    sheet1.SetColumnWidth(i, sheet1.GetColumnWidth(i) + (5 * 256))
                Next


                Dim filename As String = oCommon.GetRandomizeNum & ".xls"

                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename))
                Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer())
                Response.End()

                blnFlag = True
            Catch ex As Exception
                blnFlag = False
                sMessage = ex.Message

            Finally
            End Try

        End If

    End Sub

    Private Function WriteToStream(ByVal hssfworkbook As HSSFWorkbook) As MemoryStream
        'Write the stream data of workbook to the root directory
        Dim file As MemoryStream = New MemoryStream
        hssfworkbook.Write(file)
        Return file
    End Function

    Private Sub Query_PrimalModelNo()
        Dim TagText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        '原型機種 
        oCommon.getQuery_PrimalModelNoByDropDownList(Me.UI_cboPrimalModelNo, TagText, Session("_UserID").ToString())

        '移除舊機種
        Dim lsPart As List(Of String) = New List(Of String)
        lsPart.Add("9700")
        lsPart.Add("9700 Cradle")
        lsPart.Add("9700 4 Slot Battery Charger")
        lsPart.Add("9700 4 Slot Cradle")
        lsPart.Add("1862")
        lsPart.Add("9400 GANG Battery Charger 6V3.3A")
        lsPart.Add("RK25")
        lsPart.Add("RK25 Cradle")
        lsPart.Add("RK25 Ethernet Cradle")
        lsPart.Add("RK25 4 Slot Charger")
        lsPart.Add("RS35")
        lsPart.Add("RS35 4 Slot Battery charger")
        lsPart.Add("RS35 Cradle")

        For i = 0 To Me.UI_cboPrimalModelNo.Items.Count - 1

            Dim result = From v In lsPart Where v = Me.UI_cboPrimalModelNo.Items(i).Value

            If result.Count > 0 Then
                Me.UI_cboPrimalModelNo.Items(i).Enabled = False
            End If

        Next

    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click

        Me.right_component.Visible = True

        If Down.SelectedItem.Value = "0" Then

            Call Query_PrimalSN(0)

        End If

        'Product Serial No Search
        If Down.SelectedItem.Value = "1" Then

            Call Query_ProductSerialNo(0)
        End If


        'Part’s No Search
        If Down.SelectedItem.Value = "2" Then

            Call Query_PartNo(0)
        End If


        If Not Session("_dtReport") Is Nothing Then

            Dim dtReport As New ReportDTO.Rpt_BOMDataTable
            dtReport = Session("_dtReport")
            Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
            Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize

            Me.Count_Lab_.Text = myDataTable.Rows.Count.ToString()

            Me.BookMark_Label.Text = "0"

            If myDataTable.Rows.Count = 0 Then
                Current_Page_Label.Text = "0"
            Else

                Current_Page_Label.Text = "1"
            End If


            Total_Page_Label.Text = Math.Ceiling(myDecimal).ToString()

            'If Me.UI_radSelect1.Checked = True Then
            '    UI_ExportWithPic.Visible = True
            'Else
            '    UI_ExportWithPic.Visible = False
            'End If
            'UI_cmdExport.Visible = True
        End If

        Me.erma_infomation_number_count_lab.Visible = True

    End Sub
    ''' <summary>
    ''' 原型機種 Search
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Query_PrimalSN(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim retval As Boolean = False
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable

        Try
            Dim sPartNo As String = ""

            dtReport = ctlReport.QuerySpare_PrimalSN(Me.UI_cboPrimalModelNo.SelectedValue, _RepairBOM_No, sPartNo)

            Call Report_DataBind(dtReport, iPageIndex)
            retval = True

        Catch ex As Exception

        Finally
            If retval = False Then
                Dim _SQLCollection As New Collection
                _SQLCollection = ctlReport.getSQL()
                For i = 1 To _SQLCollection.Count
                    'Response.Write(_SQLCollection(i) & "<br>")
                Next

            End If

        End Try

    End Sub
    ''' <summary>
    ''' Product Serial No Search
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Query_ProductSerialNo(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable

        Dim sPartNo As String = ""
        dtReport = ctlReport.QuerySpare_ProductSerialNo(Me.UI_txtProduct_SerialNo.Text, _RepairBOM_No, sPartNo)

        'If dtReport.Rows.Count > 0 Then
        '    Me.UI_lblUpperPartsNo3.Text = _oLanguage.getText("Report", "168", ctlLanguage.eumType.Tag) & ":" & sPartNo
        'End If


        Call Report_DataBind(dtReport, iPageIndex)
    End Sub
    ''' <summary>
    ''' Part’s No Search
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Query_PartNo(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable

        dtReport = ctlReport.QuerySpare_PartsNo(Me.UI_txtPartsNo.Text, _RepairBOM_No, Date.Now.ToShortDateString())

        Call Report_DataBind(dtReport, iPageIndex)
    End Sub
    Private Sub Report_DataBind(ByVal dtReport As ReportDTO.Rpt_BOMDataTable, ByVal iPageIndex As Integer)
        Session("_dtReport") = dtReport
        'Dim a As HSSFWorkbook
        Me.UI_dvReport.PageSize = _PageSize
        Me.UI_dvReport.PageIndex = iPageIndex


        Me.UI_dvReport.DataSource = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        'Me.UI_dvReport.DataSource = dtReport.DefaultView
        Me.UI_dvReport.DataBind()
    End Sub
    Protected Sub Down_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Down.SelectedIndexChanged

        '初始化
        Me.UI_cboPrimalModelNo_Panel.Visible = False
        'Me.UI_txtSerialNo_Panel.Visible = False
        Me.UI_txtProduct_SerialNo_Panel.Visible = False
        Me.UI_txtPartsNo_Panel.Visible = False

        '顯示資料
        If Me.Down.SelectedItem.Value = "0" Then

            Me.UI_cboPrimalModelNo_Panel.Visible = True

            'ElseIf Me.Down.SelectedItem.Value = "1" Then

            '    Me.UI_txtSerialNo_Panel.Visible = True

        ElseIf Me.Down.SelectedItem.Value = "1" Then


            Me.UI_txtProduct_SerialNo_Panel.Visible = True



        ElseIf Me.Down.SelectedItem.Value = "2" Then

            Me.UI_txtPartsNo_Panel.Visible = True

        End If

    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound


        '多語系轉換
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "101", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "191", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "162", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "163", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Report", "164", ctlLanguage.eumType.Tag)

            e.Row.Cells(1).Text = _oLanguage.getText("Report", "164", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "164", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "191", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "162", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Report", "163", ctlLanguage.eumType.Tag)




            'e.Row.Cells(1).Text = ""
            'e.Row.Cells(2).Text = ""
            'e.Row.Cells(3).Text = ""
            'e.Row.Cells(4).Text = ""
            'e.Row.Cells(5).Text = ""
            'e.Row.Cells(6).Text = ""
            'e.Row.Cells(7).Text = ""

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()


            Dim UI_Substitute1 As Label = e.Row.FindControl("UI_Substitute1")
            Dim UI_Substitute2 As Label = e.Row.FindControl("UI_Substitute2")
            Dim UI_cmdSubstitute1 As Button = e.Row.FindControl("UI_cmdSubstitute1")
            Dim UI_cmdSubstitute2 As Button = e.Row.FindControl("UI_cmdSubstitute2")

            '"1"-->替代, "2"-->取代
            UI_cmdSubstitute1.Text = _oLanguage.getText("Common", "074", ctlLanguage.eumType.Command)
            UI_cmdSubstitute2.Text = _oLanguage.getText("Common", "073", ctlLanguage.eumType.Command)
            UI_cmdSubstitute1.Visible = False
            UI_cmdSubstitute2.Visible = False

            If UI_Substitute1.Text.Trim = "1" Then
                UI_cmdSubstitute1.Visible = True
            End If
            If UI_Substitute2.Text.Trim = "2" Then
                UI_cmdSubstitute2.Visible = True
            End If


            Dim UI_imgFile As Label = e.Row.FindControl("UI_imgFile")
            Dim UI_cmdPic As Button = e.Row.FindControl("UI_cmdPic")
            UI_cmdPic.Text = _oLanguage.getText("Common", "075", ctlLanguage.eumType.Command)
            UI_cmdPic.Visible = False
            If UI_imgFile.Text.Trim <> "" Then
                UI_cmdPic.Visible = True
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

    Protected Sub UI_dvReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvReport.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        If e.CommandName = "cmdSubstitute1" Then
            row = Me.UI_dvReport.Rows(iIndex)
            Dim UI_bmb01 As Label = row.FindControl("UI_bmb01")     '上階料件
            Dim UI_bmb03 As Label = row.FindControl("UI_bmb03")     '下階料件
            Me.ucSubstitute.show(1, UI_bmb03.Text.Trim(), UI_bmb03.Text.Trim())
        End If


        If e.CommandName = "cmdSubstitute2" Then
            row = Me.UI_dvReport.Rows(iIndex)
            Dim UI_bmb01 As Label = row.FindControl("UI_bmb01")
            Dim UI_bmb03 As Label = row.FindControl("UI_bmb03")
            Me.ucSubstitute.show(2, UI_bmb03.Text.Trim(), UI_bmb03.Text.Trim())
        End If

        If e.CommandName = "cmdPic" Then
            row = Me.UI_dvReport.Rows(iIndex)
            Dim UI_imgFile As Label = row.FindControl("UI_imgFile")
            Me.ucPartPic.show(UI_imgFile.Text.Trim())
        End If

    End Sub

    Protected Sub UI_cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdReset.Click
        Response.Redirect("Report1_Search.aspx")
    End Sub

#Region "分頁器"
    Protected Sub first_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles first_ImageBtn.Click
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable
        dtReport = Session("_dtReport")
        Me.UI_dvReport.PageSize = _PageSize
        Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        Me.UI_dvReport.PageIndex = 0
        Me.UI_dvReport.DataSource = myDataTable
        Me.UI_dvReport.DataBind()
        BookMark_Label.Text = "0"
        Current_Page_Label.Text = "1"
    End Sub
    Protected Sub previous_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles previous_ImageBtn.Click
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable
        dtReport = Session("_dtReport")
        Me.UI_dvReport.PageSize = _PageSize
        Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        If Convert.ToInt32(BookMark_Label.Text.Trim()) > 0 Then
            Me.UI_dvReport.PageIndex = Convert.ToInt32(BookMark_Label.Text.Trim()) - 1
            BookMark_Label.Text = (Convert.ToInt32(BookMark_Label.Text.Trim()) - 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
        End If
        Me.UI_dvReport.DataSource = myDataTable
        Me.UI_dvReport.DataBind()
    End Sub
    Protected Sub next_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles next_ImageBtn.Click
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable
        dtReport = Session("_dtReport")
        Me.UI_dvReport.PageSize = _PageSize
        Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        If Convert.ToInt32(BookMark_Label.Text.Trim()) < Math.Ceiling(myDecimal) - 1 Then
            Me.UI_dvReport.PageIndex = Convert.ToInt32(BookMark_Label.Text.Trim()) + 1
            BookMark_Label.Text = (Convert.ToInt32(BookMark_Label.Text.Trim()) + 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) + 1).ToString()
        End If
        Me.UI_dvReport.DataSource = myDataTable
        Me.UI_dvReport.DataBind()
    End Sub
    Protected Sub last_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles last_ImageBtn.Click
        Dim dtReport As New ReportDTO.Rpt_BOMDataTable
        dtReport = Session("_dtReport")
        Me.UI_dvReport.PageSize = _PageSize
        Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        Me.UI_dvReport.PageIndex = Math.Ceiling(myDecimal)
        Me.UI_dvReport.DataSource = myDataTable
        Me.UI_dvReport.DataBind()
        BookMark_Label.Text = Me.UI_dvReport.PageIndex.ToString()
        Current_Page_Label.Text = (Me.UI_dvReport.PageIndex + 1).ToString()
    End Sub
    Protected Sub Current_Page_Label_TextChanged(sender As Object, e As EventArgs) Handles Current_Page_Label.TextChanged

        Dim dtReport As New ReportDTO.Rpt_BOMDataTable
        dtReport = Session("_dtReport")
        Me.UI_dvReport.PageSize = _PageSize
        Dim myDataTable As DataTable = dtReport.DefaultView.ToTable(True, "EXPORT_MODELNO", "bmb03", "RPBOM_DESC", "RPBOM_MATERIALCOST", "Substitute1", "Substitute2", "imgfile")
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize

        If IsNumeric(Current_Page_Label.Text.Trim()) Then

            If Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1 <= Math.Ceiling(myDecimal) - 1 And Convert.ToInt32(Current_Page_Label.Text.Trim()) > 0 Then
                Me.UI_dvReport.PageIndex = Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1
                BookMark_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
                Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim())).ToString()

                Me.UI_dvReport.DataSource = myDataTable
                Me.UI_dvReport.DataBind()

            Else
                Current_Page_Label.Text = ""

            End If

        Else
            Current_Page_Label.Text = ""

        End If


    End Sub
#End Region

End Class

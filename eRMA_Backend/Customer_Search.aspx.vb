Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
'202307140929 exe TO excel
Imports NPOI.HSSF.UserModel

Partial Class Customer_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "3"



    '20230714 09:27 產生excel

    Protected Sub Excel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Excel_btn.Click

        '取保固單明細 20230714
        Dim oCustomer As New ctlCustomer.Customer
        Dim WARRANTYITEMDataTable_CELL As New DataTable
        WARRANTYITEMDataTable_CELL = oCustomer.QueryALL()

        Dim MS_Date_yyyyMMddhhmmss As String = DateTime.Now.ToString("yyyyMMddhhmmss")
        Dim folderPath As String = Server.MapPath("~/FILE/Sample/")

        If Not WARRANTYITEMDataTable_CELL Is Nothing Then
            ' 增加下載客戶資訊，精簡寫法 by buck modify 20250827 begin
            Dim workbook As HSSFWorkbook = New HSSFWorkbook()
            Dim sheet1 As HSSFSheet = workbook.CreateSheet("sheet1")
            Dim row As HSSFRow

            Dim i As Decimal = 0
            WARRANTYITEMDataTable_CELL.AsEnumerable.ToList().
                ForEach(Sub(item)
                            row = sheet1.CreateRow(i)

                            row.CreateCell(0).SetCellValue(If(i = 0, "CU_NO", item.Field(Of String)("CU_NO")))
                            row.CreateCell(1).SetCellValue(If(i = 0, "CU_NAME", item.Field(Of String)("CU_NAME")))
                            row.CreateCell(2).SetCellValue(If(i = 0, "CU_COUNTRYID", item.Field(Of String)("CU_COUNTRYID")))
                            row.CreateCell(3).SetCellValue(If(i = 0, "CU_COMPNO", item.Field(Of String)("CU_COMPNO")))
                            row.CreateCell(4).SetCellValue(If(i = 0, "CU_SALESID", item.Field(Of String)("CU_SALESID")))
                            row.CreateCell(5).SetCellValue(If(i = 0, "CU_SALES_EMAIL", item.Field(Of String)("CU_SALES_EMAIL"))) '將對應業務、業管email加入現有報表 by buck modify 20251030
                            row.CreateCell(6).SetCellValue(If(i = 0, "CU_ASSISTANTID", item.Field(Of String)("CU_ASSISTANTID")))
                            row.CreateCell(7).SetCellValue(If(i = 0, "CU_ASSISTANT_EMAIL", item.Field(Of String)("CU_ASSISTANT_EMAIL"))) '將對應業務、業管email加入現有報表 by buck modify 20251030
                            row.CreateCell(8).SetCellValue(If(i = 0, "CU_STATUS", If(item.IsNull("CU_SERVICE_CHG"), 0, item.Field(Of Decimal)("CU_STATUS"))))
                            row.CreateCell(9).SetCellValue(If(i = 0, "CU_CONTACTPERSON", item.Field(Of String)("CU_CONTACTPERSON")))
                            row.CreateCell(10).SetCellValue(If(i = 0, "CU_TEL", item.Field(Of String)("CU_TEL")))
                            row.CreateCell(11).SetCellValue(If(i = 0, "CU_SERVICE_CHG", If(item.IsNull("CU_SERVICE_CHG"), 0, item.Field(Of Decimal)("CU_SERVICE_CHG"))))
                            row.CreateCell(12).SetCellValue(If(i = 0, "CU_DISCOUNT_OFF", If(item.IsNull("CU_DISCOUNT_OFF"), 0, item.Field(Of Decimal)("CU_DISCOUNT_OFF"))))
                            row.CreateCell(13).SetCellValue(If(i = 0, "CU_ADDRESS1", item.Field(Of String)("CU_ADDRESS1")))
                            row.CreateCell(14).SetCellValue(If(i = 0, "CU_ADDRESS2", item.Field(Of String)("CU_ADDRESS2")))
                            row.CreateCell(15).SetCellValue(If(i = 0, "CU_ADDRESS3", item.Field(Of String)("CU_ADDRESS3")))
                            row.CreateCell(16).SetCellValue(If(i = 0, "CU_ADDRESS4", item.Field(Of String)("CU_ADDRESS4")))
                            row.CreateCell(17).SetCellValue(If(i = 0, "CU_EMAIL", item.Field(Of String)("CU_EMAIL")))
                            row.CreateCell(18).SetCellValue(If(i = 0, "CU_FINANCEEMAIL", item.Field(Of String)("CU_FINANCEEMAIL")))
                            row.CreateCell(19).SetCellValue(If(i = 0, "CU_ISCHOICE", item.Field(Of String)("CU_ISCHOICE")))
                            row.CreateCell(20).SetCellValue(If(i = 0, "CU_TIPTOP_ID", item.Field(Of String)("CU_TIPTOP_ID")))

                            i += 1
                        End Sub)

            ' 增加下載客戶資訊，精簡寫法 by buck modify 20250827 end
            Dim MS As New FileStream(Path.Combine(folderPath, MS_Date_yyyyMMddhhmmss & ".xls"), FileMode.Create)
            workbook.Write(MS)
            MS.Close()
            Download_Lab.Text = "<a href ='" & "FILE/Sample/" & MS_Date_yyyyMMddhhmmss & ".xls" & "' download>下載" & MS_Date_yyyyMMddhhmmss & ".xls" & "</a>"
        Else
            Download_Lab.Text = "請再次點擊Searh"
        End If

    End Sub

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtCustomer") = Nothing

            Call setControls()
            '  Call QueryData(0)
        End If
    End Sub
#End Region

    Private Sub setControls()
        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(False, Me.UI_cboRepairCenter, sRepairText)

        oCommon.getCountryByDropDownList(Me.UI_cboCountry, sRepairText)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Customer", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerSearching.Text = _oLanguage.getText("Customer", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerID.Text = _oLanguage.getText("Customer", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerName.Text = _oLanguage.getText("Customer", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Customer", "005", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items(0).Text = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items(1).Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items(2).Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Customer", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerTittle.Text = _oLanguage.getText("Customer", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblCountry.Text = _oLanguage.getText("Customer", "013", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Me.UI_Customer.Columns(1).HeaderText = _oLanguage.getText("Customer", "003", ctlLanguage.eumType.Tag)
        Me.UI_Customer.Columns(2).HeaderText = _oLanguage.getText("Customer", "004", ctlLanguage.eumType.Tag)
        Me.UI_Customer.Columns(3).HeaderText = _oLanguage.getText("Customer", "011", ctlLanguage.eumType.Tag)
        Me.UI_Customer.Columns(4).HeaderText = _oLanguage.getText("Customer", "009", ctlLanguage.eumType.Tag)
        Me.UI_Customer.Columns(5).HeaderText = _oLanguage.getText("Customer", "005", ctlLanguage.eumType.Tag)
        Me.UI_Customer.Columns(6).HeaderText = _oLanguage.getText("Customer", "012", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable

        Dim sCustomerID As String = Me.UI_txtCustomerID.Text.ToString().Trim()
        Dim sCustomerNam As String = Me.UI_txtCustomerName.Text.ToString().Trim()
        Dim sStatus As String = Me.UI_cboStatus.SelectedValue.ToString().Trim()
        Dim sRepairCenter As String = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Dim sCountry As String = Me.UI_cboCountry.SelectedValue.ToString().Trim()

        dtCustomer = oCustomer.Query(sCustomerID, sCustomerNam, sStatus, sRepairCenter, sCountry)
        Call ArrangementData(dtCustomer)

        Session("_dtCustomer") = dtCustomer
        Dim dvCustomer As DataView = dtCustomer.DefaultView
        Me.ViewState("_SortExpression") = "CU_NO"
        Me.ViewState("_SortDirection") = "asc"
        dvCustomer.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Customer_DataBind(dvCustomer, iPageIndex)

    End Sub

    Private Sub Customer_DataBind(ByVal dvCustomer As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_Customer.PageSize = _PageSize
        Me.UI_Customer.PageIndex = iPageIndex
        Me.UI_Customer.DataSource = dvCustomer
        Me.UI_Customer.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtCustomer As CustomerDTO.VWCUSTOMERDataTable)
        Dim i As Integer = 0

        If dtCustomer.Columns("SeqID") Is Nothing Then
            dtCustomer.Columns.Add("SeqID")
        End If

        If dtCustomer.Columns("Status") Is Nothing Then
            dtCustomer.Columns.Add("Status")
        End If

        For i = 0 To dtCustomer.Rows.Count - 1
            dtCustomer.Rows(i)("SeqID") = i + 1

            '狀態
            Dim sStatus As String = dtCustomer.Rows(i).Item("CU_STATUS").ToString.Trim()
            If sStatus.ToString.Trim() = "1" Then
                'Open
                dtCustomer.Rows(i).Item("Status") = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
            Else
                'Close
                dtCustomer.Rows(i).Item("Status") = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)
            End If
        Next
    End Sub

    Protected Sub UI_Customer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_Customer.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_Customer.PageIndex * Me.UI_Customer.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
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

    Protected Sub UI_Customer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_Customer.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow
            row = Me.UI_Customer.Rows(iIndex)

            Dim UI_CuNo As Label = row.FindControl("UI_CuNo")
            Me.UI_lblPreviousPage_CuNo.Text = UI_CuNo.Text.Trim
        End If

    End Sub

    Protected Sub UI_Customer_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_Customer.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtCustomer") Is Nothing Then
            Dim dtCustomer As CustomerDTO.VWCUSTOMERDataTable = Session("_dtCustomer")
            Dim dvCustomer As DataView = dtCustomer.DefaultView
            Customer_DataBind(dvCustomer, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_Customer_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_Customer.Sorting

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

        If IsNothing(Session("_dtCustomer")) = False Then
            Dim dtCustomer As DataTable = Session("_dtCustomer")

            Dim dvCustomer As DataView = dtCustomer.DefaultView
            dvCustomer.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Customer_DataBind(dvCustomer, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_Customer.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_Customer.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_Customer.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_Customer.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_Customer.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_Customer.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
    End Sub

End Class

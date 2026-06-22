Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Sales_WorkList
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Me.IsPostBack = False Then
            Me.ViewState("_SortExpression") = "RMA_NO"
            Me.ViewState("_SortDirection") = "desc"

            Me.ViewState("_RMANo") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_SerialNo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("edate") = Date.Now.ToShortDateString()

            Session("_dtRMASales_tmp") = Nothing

            Call setControls()
            Call QueryData(0)
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_dvSales.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(3).HeaderText = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(4).HeaderText = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(5).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(6).HeaderText = _oLanguage.getText("RMA", "216", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(7).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(8).HeaderText = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(9).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

        Dim dtRMASales_tmp As New DataTable
        dtRMASales_tmp.Columns.Add("SEQID")
        dtRMASales_tmp.Columns.Add("RMA_NO")
        dtRMASales_tmp.Columns.Add("RMA_ID")
        dtRMASales_tmp.Columns.Add("RequestDate")
        dtRMASales_tmp.Columns.Add("Applicant")
        dtRMASales_tmp.Columns.Add("CUNAME")
        dtRMASales_tmp.Columns.Add("RequestQTY")
        dtRMASales_tmp.Columns.Add("ReceivedQTY")

        dtRMASales_tmp.Columns.Add("RepairCode")
        dtRMASales_tmp.Columns.Add("RepairQuoted")

        dtRMASales_tmp.Columns.Add("SaleCode")
        dtRMASales_tmp.Columns.Add("SaleQuoted")

        Session("_dtRMASales_tmp") = dtRMASales_tmp
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oSale As New ctlRMA.Sale
        Dim dtSaleList As New RmaDTO.vwSale_WorkListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim sSerialNo As String = Me.ViewState("_SerialNo").ToString().Trim()

        Dim sCustomerID As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtSaleList = oSale.QueryByWork(Session("_RepairCenter").ToString().Trim(), Session("_UserID").ToString().Trim(), sRMANo, sModelNo, sSerialNo, sCustomerID, fdate, edate)

        Call ArrangementData(dtSaleList)

        Dim dtRMASales_tmp As DataTable = Session("_dtRMASales_tmp")
        Dim dvSaleList As DataView = dtRMASales_tmp.DefaultView

        Call RMA_DataBind(dvSaleList, iPageIndex)
    End Sub

    Private Sub RMA_DataBind(ByVal dvSaleList As DataView, ByVal iPageIndex As Integer)
        dvSaleList.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvSales.PageSize = _PageSize
        Me.UI_dvSales.PageIndex = iPageIndex
        Me.UI_dvSales.DataSource = dvSaleList
        Me.UI_dvSales.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtSaleList As RmaDTO.vwSale_WorkListDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim sRMANo As String = ""

        Dim dtRMASales_tmp As DataTable = Session("_dtRMASales_tmp")
        dtRMASales_tmp.Rows.Clear()
        Dim dvRMASales_tmp As DataView = dtRMASales_tmp.DefaultView
        For i = 0 To dtSaleList.Rows.Count - 1
            Dim dr As RmaDTO.vwSale_WorkListRow = dtSaleList.Rows(i)
            Dim RMA_No As String = dr.RMA_NO.ToString().Trim()

            If dr.RMA_COMPNO.Trim() <> "CEAT" Then
                dvRMASales_tmp.RowFilter = "RMA_NO='" & RMA_No & "'"
                If dvRMASales_tmp.Count = 0 Then
                    iCount = iCount + 1
                    Dim drTmp As DataRow = dtRMASales_tmp.NewRow
                    If sRMANo.Trim <> "" Then
                        sRMANo = sRMANo & ","
                    End If
                    sRMANo = sRMANo & RMA_No

                    drTmp("SEQID") = iCount
                    drTmp("RMA_NO") = RMA_No
                    drTmp("RMA_ID") = dr.RMA_ID.Trim()
                    drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
                    drTmp("Applicant") = dr.RMA_APPLICANT.Trim()
                    drTmp("CUNAME") = dr.CU_NAME.Trim()
                    dtRMASales_tmp.Rows.Add(drTmp)
                End If
            End If

        Next
        dvRMASales_tmp.RowFilter = ""


        '================================================================================================================================================================================================================================
        '處理金額
        '================================================================================================================================================================================================================================
        For i = 0 To dvRMASales_tmp.Count - 1
            Dim RMA_No As String = dvRMASales_tmp(i)("RMA_No").ToString().Trim()

            Dim dvSaleList As DataView = dtSaleList.DefaultView
            dvSaleList.RowFilter = "RMA_NO='" & RMA_No & "'"
            For j = 0 To dvSaleList.Count - 1
                '1.RMAR_REPAIR_ISFILL:是否已填寫維修報價單:0.否, 1.是
                '2.必需要有報價金額
                '3.更入系統維修中心 跟 被指派維修中心 不一樣時
                Dim RMAR_REPAIR_ISFILL As String = dvSaleList(j)("RMAR_REPAIR_ISFILL").ToString().Trim()


                '維修報價
                Dim RMARQ_ASSIGECURRENCYCODE As String = dvSaleList(j)("RMARQ_ASSIGECURRENCYCODE").ToString().Trim()
                Dim RMARQ_ASSIGEQUOTE As String = dvSaleList(j)("RMARQ_ASSIGEQUOTE").ToString().Trim()
                If RMAR_REPAIR_ISFILL = "1" And RMARQ_ASSIGEQUOTE <> "" Then
                    dvRMASales_tmp(i)("RepairCode") = RMARQ_ASSIGECURRENCYCODE
                    If dvRMASales_tmp(i)("RepairQuoted").ToString.Trim() = "" Then
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(RMARQ_ASSIGEQUOTE).ToString("N")
                    Else
                        Dim RepairQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("RepairQuoted")) + Convert.ToDouble(RMARQ_ASSIGEQUOTE)
                        dvRMASales_tmp(i)("RepairQuoted") = Convert.ToDouble(RepairQuoted).ToString("N")
                    End If
                End If


                '業務報價
                Dim RMASQ_QUOTE As String = dvSaleList(j)("RMASQ_QUOTE").ToString().Trim()
                If RMASQ_QUOTE.Trim() <> "" Then
                    dvRMASales_tmp(i)("SaleCode") = dvSaleList(j)("RMASQ_CURRENCYCODE").ToString().Trim()
                    If dvRMASales_tmp(i)("SaleQuoted").ToString.Trim() = "" Then
                        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(RMASQ_QUOTE).ToString("N")
                    Else
                        Dim SaleQuoted As String = Convert.ToDouble(dvRMASales_tmp(i)("SaleQuoted")) + Convert.ToDouble(RMASQ_QUOTE)
                        dvRMASales_tmp(i)("SaleQuoted") = Convert.ToDouble(SaleQuoted).ToString("N")
                    End If
                End If

            Next
        Next
        dvRMASales_tmp.RowFilter = ""


        '================================================================================================================================================================================================================================
        '處理 RequestQTY 及 ReceivedQTY
        '================================================================================================================================================================================================================================
        If sRMANo.Trim <> "" Then
            Dim dtRMACount As New RmaDTO.RMACountDataTable
            Dim arrRMANo() As String = sRMANo.Split(",")
            Dim oRMA As New ctlRMA

            dtRMACount = oRMA.QueryCountByPlural(arrRMANo)
            Dim dvRMACount As DataView = dtRMACount.DefaultView

            For i = 0 To dvRMASales_tmp.Count - 1
                Dim RMA_No As String = dvRMASales_tmp(i)("RMA_No").ToString().Trim()

                dvRMACount.RowFilter = "RMA_No='" & RMA_No & "'"
                If dvRMACount.Count > 0 Then
                    dvRMASales_tmp(i)("RequestQTY") = dvRMACount(0)("RequestQTY").ToString().Trim()
                    dvRMASales_tmp(i)("ReceivedQTY") = dvRMACount(0)("ReceivedQTY").ToString().Trim()
                End If
            Next
        End If


        dvRMASales_tmp.RowFilter = ""
        Session("_dtRMASales_tmp") = dtRMASales_tmp
    End Sub

    Protected Sub UI_dvSales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSales.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvSales.PageIndex * Me.UI_dvSales.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "047", ctlLanguage.eumType.Command)
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

        If Not Session("_dtRMASales_tmp") Is Nothing Then
            Dim dtRMASales_tmp As DataTable = Session("_dtRMASales_tmp")
            Dim dvSaleList As DataView = dtRMASales_tmp.DefaultView
            Call RMA_DataBind(dvSaleList, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand

        If e.CommandName = "cmdDetail" Or e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim
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

        If IsNothing(Session("_dtRMASales_tmp")) = False Then
            Dim dtRMASales_tmp As DataTable = Session("_dtRMASales_tmp")
            Dim dvSaleList As DataView = dtRMASales_tmp.DefaultView
            'dvSaleList.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvSaleList, 0)
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

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_ModelNo") = ""
        Me.ViewState("_SerialNo") = ""
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.Trim()

        Me.ViewState("fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            Me.ViewState("edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

End Class

Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Sale_WorkList
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_SerialNo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("edate") = Date.Now.ToShortDateString()

            Session("_dtSaleList") = Nothing

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
        Dim sClientID As String = "ctl00_ContentPlaceHolder_UI_dvSales_ctl01_UI_CheckGroup"
        sClientID = sClientID & "," & Me.UI_cmdSearch.ClientID '& "," & Me.UI_cmdQuoting.ClientID & "," & Me.UI_cmdRepairing.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID


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
        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        'Me.UI_cmdCancel.Value = _oLanguage.getText("Common", "010", ctlLanguage.eumType.Command)
        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "040", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "041", ctlLanguage.eumType.Command)


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

        Call RMA_DataBind(dtSaleList, iPageIndex)


        '=======================================================================================================================================
        '無資料時, UI 的控制
        '=======================================================================================================================================
        Me.UI_cmdSubmit.Visible = False
        Me.UI_cmdConfirm.Visible = False
        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdConfirm.Enabled = False

        For i = 0 To Me.UI_dvSales.Rows.Count - 1
            If Me.UI_dvSales.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
                If UI_Check.Visible = True Then
                    Me.UI_cmdSubmit.Visible = True
                    Me.UI_cmdConfirm.Visible = True
                    Exit For
                End If
            End If
        Next

    End Sub

    Private Sub RMA_DataBind(ByVal dtSaleList As RmaDTO.vwSale_WorkListDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtSaleList)
        Session("_dtSaleList") = dtSaleList

        Dim dvSaleList As DataView = dtSaleList.DefaultView

        '排除維修中心尚未報價的部份
        dvSaleList.RowFilter = "QUOTE<>''"

        Me.UI_dvSales.PageSize = _PageSize
        Me.UI_dvSales.PageIndex = iPageIndex
        Me.UI_dvSales.DataSource = dvSaleList
        Me.UI_dvSales.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtSaleList As RmaDTO.vwSale_WorkListDataTable)
        Dim i As Integer = 0

        If dtSaleList.Columns("SeqID") Is Nothing Then
            dtSaleList.Columns.Add("SeqID")
            dtSaleList.Columns.Add("QUOTE")
            dtSaleList.Columns.Add("Status")
            dtSaleList.Columns.Add("Assign")

            dtSaleList.Columns.Add("SaleCode")
            dtSaleList.Columns.Add("SaleQuoted")
        End If

        For i = 0 To dtSaleList.Rows.Count - 1
            dtSaleList.Rows(i)("SeqID") = i + 1

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            dtSaleList.Rows(i)("Status") = oCommon.ConvertToItemStatusText(Convert.ToInt16(dtSaleList.Rows(i)("RMAD_STATUS")), dtSaleList.Rows(i)("RMAD_ID").ToString().Trim())

            '1.RMAR_REPAIR_ISFILL:是否已填寫維修報價單:0.否, 1.是
            '2.必需要有報價金額
            '3.更入系統維修中心 跟 被指派維修中心 不一樣時
            dtSaleList.Rows(i)("QUOTE") = ""
            Dim RMAR_REPAIR_ISFILL As String = dtSaleList.Rows(i)("RMAR_REPAIR_ISFILL").ToString().Trim()

            Dim RMARQ_CURRENCYCODE As String = dtSaleList.Rows(i)("RMARQ_CURRENCYCODE").ToString().Trim()
            Dim RMARQ_QUOTE As String = dtSaleList.Rows(i)("RMARQ_QUOTE").ToString().Trim()

            Dim RMARQ_ASSIGECURRENCYCODE As String = dtSaleList.Rows(i)("RMARQ_ASSIGECURRENCYCODE").ToString().Trim()
            Dim RMARQ_ASSIGEQUOTE As String = dtSaleList.Rows(i)("RMARQ_ASSIGEQUOTE").ToString().Trim()


            '登入系統維修中心 跟 被指派維修中心 不一樣時
            If RMARQ_ASSIGEQUOTE <> "" Then
                RMARQ_ASSIGEQUOTE = Convert.ToDouble(RMARQ_ASSIGEQUOTE).ToString("N")
            End If
            If RMAR_REPAIR_ISFILL = "1" And RMARQ_ASSIGEQUOTE <> "" Then
                dtSaleList.Rows(i)("QUOTE") = RMARQ_ASSIGECURRENCYCODE & " " & RMARQ_ASSIGEQUOTE
                dtSaleList.Rows(i)("SaleCode") = RMARQ_ASSIGECURRENCYCODE
            End If

            '業務報價
            dtSaleList.Rows(i)("SaleQuoted") = ""
            Dim RMASQ_QUOTE As String = dtSaleList.Rows(i)("RMASQ_QUOTE").ToString().Trim()
            If RMASQ_QUOTE.Trim() <> "" Then
                dtSaleList.Rows(i)("SaleQuoted") = Convert.ToDouble(RMASQ_QUOTE).ToString("N")
            End If
        Next

    End Sub

    Protected Sub UI_dvSales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSales.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim COMPNO As String = ""
            Dim oCheck As CheckBox = e.Row.FindControl("UI_Check")
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")
            Dim UI_RMA_COMPNO As Label = e.Row.FindControl("UI_RMA_COMPNO")
            Dim UI_RMAR_COMPNO As Label = e.Row.FindControl("UI_RMAR_COMPNO")


            Dim UI_SaleCode As Label = e.Row.FindControl("UI_SaleCode")
            Dim UI_RMASQID As Label = e.Row.FindControl("UI_RMASQID")


            Dim UI_SaleQuoted As Label = e.Row.FindControl("UI_SaleQuoted")
            Dim rfvSaleQuote As RequiredFieldValidator = e.Row.FindControl("rfvSaleQuote")
            Dim rvSaleQuote As RangeValidator = e.Row.FindControl("rvSaleQuote")

            If UI_RMASQID.Text.Trim = "" Then
                UI_SaleCode.Visible = False
                UI_SaleQuoted.Visible = False
            End If
            'rfvSaleQuote.ControlToValidate = UI_SaleQuoted.ID
            'rvSaleQuote.ControlToValidate = UI_SaleQuoted.ID
            'rfvSaleQuote.ErrorMessage = _oLanguage.getText("RMA", "135", ctlLanguage.eumType.Validator)
            'rvSaleQuote.ErrorMessage = _oLanguage.getText("RMA", "136", ctlLanguage.eumType.Validator)


            '控制 oCheck 出現時機
            '1.品項狀態 35.Sale Quoting, CheckBox 就要出現了
            If Convert.ToInt16(UI_Status.Text.Trim()) = 35 Then
                oCheck.Visible = True
            End If

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

        If Not Session("_dtSaleList") Is Nothing Then
            Dim dtSaleList As RmaDTO.vwSale_WorkListDataTable = Session("_dtSaleList")
            Call RMA_DataBind(dtSaleList, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

        If e.CommandName = "cmdEdit" Then
            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")

            Me.UcSalesDetail1.show(UI_RMAID.Text.Trim(), UI_RMADID.Text.ToString().Trim(), UI_RMANO.Text.ToString().Trim(), True)
        End If

        If e.CommandName = "cmdDetail" Then
            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim
        End If

    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.Trim()
        Me.ViewState("_SerialNo") = Me.UI_txtSerialNumber.Text.Trim()
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

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_CheckGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        For i = 0 To Me.UI_dvSales.Rows.Count - 1
            If Me.UI_dvSales.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
                UI_Check.Checked = sender.Checked
            End If
        Next

        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdConfirm.Enabled = False
        If sender.Checked = True Then
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdConfirm.Enabled = True
        End If

    End Sub

    ''' <summary>
    ''' 項目勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0

        Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdConfirm.Enabled = False
        For i = 0 To Me.UI_dvSales.Rows.Count - 1
            If Me.UI_dvSales.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
                If UI_Check.Checked = True Then
                    Me.UI_cmdSubmit.Enabled = True
                    Me.UI_cmdConfirm.Enabled = True
                    Exit For
                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' 儲存- 已不使用了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        'System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        'Dim i As Integer = 0
        'Dim sMessage As String = ""
        'Dim blnFlag As Boolean = False
        'Dim oSale As New ctlRMA.Sale
        'Dim dtRMADetail As New RmaDTO.tmpSaleStatusDataTable

        'Try
        '    For i = 0 To Me.UI_dvSales.Rows.Count - 1

        '        If Me.UI_dvSales.Rows(i).RowType = DataControlRowType.DataRow Then
        '            Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
        '            Dim UI_RMASQID As Label = Me.UI_dvSales.Rows(i).FindControl("UI_RMASQID")
        '            Dim UI_SaleQuoted As TextBox = Me.UI_dvSales.Rows(i).FindControl("UI_SaleQuoted")

        '            If UI_SaleQuoted.Text.Trim() <> "" Then
        '                Dim drRMADetail As RmaDTO.tmpSaleStatusRow = dtRMADetail.NewtmpSaleStatusRow

        '                drRMADetail.RMASQ_ID = UI_RMASQID.Text.ToString().Trim()
        '                drRMADetail.RMASQ_QUOTE = Convert.ToDecimal(UI_SaleQuoted.Text.ToString().Trim())
        '                dtRMADetail.AddtmpSaleStatusRow(drRMADetail)
        '            End If

        '        End If
        '    Next
        '    oSale.Save_RMADetailStatus(dtRMADetail)

        '    blnFlag = True
        'Catch ex As Exception
        '    sMessage = ex.Message
        '    blnFlag = False

        'Finally
        '    If blnFlag = False Then
        '        Me.ucMessage.showMessageByFailed(sMessage)
        '    Else
        '        Dim sMsg As String = ""
        '        sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
        '        Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Sale_WorkList.aspx")
        '    End If
        'End Try

    End Sub

    ''' <summary>
    ''' Submit to Customer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Call RMADetail_Save(40)
    End Sub

    ''' <summary>
    ''' Confirm by Sales
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click
        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Call RMADetail_Save(50)
    End Sub

    ''' <summary>
    ''' 業務報價確認 及 業務幫客戶確認
    ''' </summary>
    ''' <param name="iStatus">RMAD_STATUS</param>
    ''' <remarks></remarks>
    Private Sub RMADetail_Save(ByVal iStatus As Integer)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oSale As New ctlRMA.Sale
        Dim dtConfirmed As New RmaDTO.SalesQuoted_ConfirmedDataTable

        Try
            For i = 0 To Me.UI_dvSales.Rows.Count - 1
                If Me.UI_dvSales.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
                    Dim UI_RMADID As Label = Me.UI_dvSales.Rows(i).FindControl("UI_RMADID")
                    Dim UI_RMASQID As Label = Me.UI_dvSales.Rows(i).FindControl("UI_RMASQID")

                    If UI_Check.Checked = True Then
                        Dim dr As RmaDTO.SalesQuoted_ConfirmedRow = dtConfirmed.NewSalesQuoted_ConfirmedRow

                        dr.RMASQ_ID = UI_RMASQID.Text.ToString().Trim()
                        dr.RMAD_ID = UI_RMADID.Text.ToString().Trim()

                        dr.RMASQ_SALEAD = Session("_UserID")
                        dr.RMASQ_SALEADNAME = Session("_UserName")
                        dr.RMASQ_SALEDATE = Date.Now
                        dr.RMAD_STATUS = iStatus

                        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                        'RMASQ_CLIENTCONFIRM (1.客戶確認, 2.業務帶客戶確認)-->如果幫客戶確認, CLIENTCONFIRM=2
                        If iStatus = 50 Then
                            dr.RMASQ_CLIENTCONFIRM = 2
                        End If

                        dtConfirmed.AddSalesQuoted_ConfirmedRow(dr)
                    End If

                End If
            Next
            oSale.SalesQuoted_Confirmed(dtConfirmed)

            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Sale_WorkList.aspx")
            End If
        End Try
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        '如果ucRepairQuoted.ascx 有錯誤發生時, 要處理的機制
        If Session("UCError").ToString.Trim() <> "" Then
            Me.ucMessage.showMessageByFailed(Session("UCError").ToString().Trim())
            Session("UCError") = ""
        End If

        If Convert.ToBoolean(Session("UCOK")) = True Then
            Dim iPageIndex As Integer = Me.UI_dvSales.PageIndex
            Call QueryData(iPageIndex)

            Session("UCOK") = False
        End If

    End Sub

End Class

Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Sales_WorkList_Item
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "1"
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    'Hugh 
    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions
    Private fTotalQuote As Double

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = ""
    'Hugh

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            Session("_dtSaleList") = Nothing
            Call setControls()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.ToString().Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.ToString().Trim()

                Call QueryHead()
                Call QueryDetail(0)
            End If
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "114", ctlLanguage.eumType.Tag)
        Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)

        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicantID.Text = _oLanguage.getText("RMA", "045", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)


        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "040", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "041", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)


        'Hugh 2011-7-25
        Me.UI_QuoteDownload.Text = _oLanguage.getText("Common", "H_001", ctlLanguage.eumType.Command)
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '如果ucRepairQuoted.ascx 有錯誤發生時, 要處理的機制
        If Session("UCError").ToString.Trim() <> "" Then
            Me.ucMessage.showMessageByFailed(Session("UCError").ToString().Trim())
            Session("UCError") = ""
        End If

        If Convert.ToBoolean(Session("UCOK")) = True Then
            Call QueryDetail(Me.UI_dvSales.PageIndex)
            Session("UCOK") = False
        End If
    End Sub

#Region "QueryHead"
    Private Sub QueryHead()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable

        dtRepairHead = oRepair.QueryByRepairHead(Me.UI_lblPreviousPage_RMAID.Text)
        If dtRepairHead.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_HeadRow = dtRepairHead.Rows(0)

            Me.UI_lblRMANoText.Text = Me.UI_lblPreviousPage_RMANO.Text.Trim
            Me.UI_lblAccountIDText.Text = dr.RMA_CUNO.ToString().Trim()
            Me.UI_lblAccountNameText.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblApplicantIDText.Text = dr.RMA_ACCOUNTID.ToString().Trim()
            Me.UI_lblApplicantText.Text = dr.RMA_APPLICANT.ToString().Trim()
            Me.UI_lblTelText.Text = dr.RMA_TEL.ToString().Trim()
            Me.UI_lblAddressText.Text = dr.RMA_ADDRESS.ToString().Trim()
            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
        End If
    End Sub
#End Region

    Private Sub QueryDetail(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oSale As New ctlRMA.Sale
        Dim dtSaleList As New RmaDTO.vwSale_WorkListDataTable

        Dim sRMANo As String = Me.UI_lblPreviousPage_RMANO.Text.Trim

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtSaleList = oSale.QueryByDetail(Session("_RepairCenter").ToString().Trim(), Session("_UserID").ToString().Trim(), sRMANo, "RMAD_CSTMP asc")

        Call ArrangementData(dtSaleList)
        Session("_dtSaleList") = dtSaleList
        Dim dvSaleList As DataView = dtSaleList.DefaultView

        Call RMA_DataBind(dvSaleList, iPageIndex)



        '=======================================================================================================================================
        '按鈕, UI 的控制
        '=======================================================================================================================================
        Dim isEnabledButton As Boolean = False
        For i = 0 To dvSaleList.Count - 1
            Dim RMAD_STATUS As String = dvSaleList(i)("RMAD_STATUS").ToString().Trim()
            If Convert.ToInt16(RMAD_STATUS) <> 91 Then
                If Convert.ToInt16(RMAD_STATUS) = 35 Or Convert.ToInt16(RMAD_STATUS) = 40 Then
                    isEnabledButton = True
                Else
                    isEnabledButton = False
                    Exit For
                End If
            End If
        Next


        '========================================================================================================================================================================================================================================================================================
        '2011/09/16 START
        '調整項目:
        '1.業務報價時, 該RMA單有任一筆item, 經過Submit by Sales 後, 就可以維修.
        '2.業務報價時, 該RMA單可針對任一筆item做 Submit by Sales 或 Submit by Customer.
        '原Code
        '========================================================================================================================================================================================================================================================================================
        'Me.UI_cmdSubmit.Enabled = isEnabledButton
        'Me.UI_cmdConfirm.Enabled = isEnabledButton
        '========================================================================================================================================================================================================================================================================================
        '2011/09/16 END
        '========================================================================================================================================================================================================================================================================================



        'Hugh 2011-7-25
        Me.UI_QuoteDownload.Enabled = isEnabledButton

        '只開放China下載
        If UI_lblRMANoText.Text.Trim().IndexOf("CRMA") >= 0 Then
            Me.UI_QuoteDownload.Visible = True
        Else
            Me.UI_QuoteDownload.Visible = False
        End If


    End Sub

    Private Sub RMA_DataBind(ByVal dvSaleList As DataView, ByVal iPageIndex As Integer)

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

            dtSaleList.Columns.Add("RepairCode")
            dtSaleList.Columns.Add("RepairQuoted")

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
            Dim RMARQ_ASSIGECURRENCYCODE As String = dtSaleList.Rows(i)("RMARQ_ASSIGECURRENCYCODE").ToString().Trim()
            Dim RMARQ_ASSIGEQUOTE As String = dtSaleList.Rows(i)("RMARQ_ASSIGEQUOTE").ToString().Trim()


            '登入系統維修中心 跟 被指派維修中心 不一樣時
            If RMAR_REPAIR_ISFILL = "1" And RMARQ_ASSIGEQUOTE <> "" Then
                dtSaleList.Rows(i)("RepairCode") = RMARQ_ASSIGECURRENCYCODE
                dtSaleList.Rows(i)("RepairQuoted") = Convert.ToDouble(RMARQ_ASSIGEQUOTE).ToString("N")
            End If


            '業務報價
            dtSaleList.Rows(i)("SaleQuoted") = ""
            Dim RMASQ_QUOTE As String = dtSaleList.Rows(i)("RMASQ_QUOTE").ToString().Trim()
            If RMASQ_QUOTE.Trim() <> "" Then
                dtSaleList.Rows(i)("SaleCode") = dtSaleList.Rows(i)("RMASQ_CURRENCYCODE").ToString().Trim()
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
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")

            UI_cmdEdit.Text = _oLanguage.getText("Common", "047", ctlLanguage.eumType.Command)
            UI_cmdEdit.Visible = False
            If Convert.ToInt16(UI_Status.Text.Trim) = 30 Or Convert.ToInt16(UI_Status.Text.Trim) = 35 Or Convert.ToInt16(UI_Status.Text.Trim) = 40 Then
                UI_cmdEdit.Visible = True
            End If


            '========================================================================================================================================================================================================================================================================================
            '2011/09/16 START
            '調整項目:
            '1.業務報價時, 該RMA單有任一筆item, 經過Submit by Sales 後, 就可以維修.
            '2.業務報價時, 該RMA單可針對任一筆item做 Submit by Sales 或 Submit by Customer.
            '========================================================================================================================================================================================================================================================================================
            Dim UI_Check As CheckBox = e.Row.FindControl("UI_Check")
            UI_Check.Visible = False
            If Convert.ToInt16(UI_Status.Text.Trim) = 35 Or Convert.ToInt16(UI_Status.Text.Trim) = 40 Then
                UI_Check.Visible = True
                Dim UI_CheckGroup As CheckBox = Me.UI_dvSales.HeaderRow.FindControl("UI_CheckGroup")
                UI_CheckGroup.Visible = True
            End If
            '========================================================================================================================================================================================================================================================================================
            '2011/09/16 END
            '========================================================================================================================================================================================================================================================================================
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

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")

            Me.UcSalesDetail1.show(UI_RMAID.Text.Trim(), UI_RMADID.Text.ToString().Trim(), UI_RMANO.Text.ToString().Trim(), True)
        End If

    End Sub

    Protected Sub UI_dvSales_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSales.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtShipping") Is Nothing Then
            Dim dtSaleList As RmaDTO.vwSale_WorkListDataTable = Session("_dtSaleList")
            Dim dvSaleList As DataView = dtSaleList.DefaultView
            Call RMA_DataBind(dvSaleList, iPageIndex)

        Else
            Call QueryDetail(iPageIndex)
        End If
    End Sub

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 2011/09/16 START
    ''' 調整項目:
    ''' 1.業務報價時, 該RMA單有任一筆item, 經過Submit by Sales 後, 就可以維修.
    ''' 2.業務報價時, 該RMA單可針對任一筆item做 Submit by Sales 或 Submit by Customer.
    ''' </remarks>
    Protected Sub UI_checkGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim isEnabledButton As Boolean = False

        Dim UI_CheckGroup As CheckBox = Me.UI_dvSales.HeaderRow.FindControl("UI_CheckGroup")

        For i = 0 To Me.UI_dvSales.Rows.Count - 1
            Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
            UI_Check.Checked = False
            If UI_Check.Visible = True Then
                UI_Check.Checked = UI_CheckGroup.Checked
                If UI_Check.Checked = True Then
                    isEnabledButton = True
                End If
            End If
        Next

        Me.UI_cmdSubmit.Enabled = isEnabledButton
        Me.UI_cmdConfirm.Enabled = isEnabledButton
    End Sub

    ''' <summary>
    ''' 項目勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 2011/09/16 START
    ''' 調整項目:
    ''' 1.業務報價時, 該RMA單有任一筆item, 經過Submit by Sales 後, 就可以維修.
    ''' 2.業務報價時, 該RMA單可針對任一筆item做 Submit by Sales 或 Submit by Customer.
    ''' </remarks>
    Protected Sub UI_Check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim isEnabledButton As Boolean = False

        For i = 0 To Me.UI_dvSales.Rows.Count - 1
            Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
            If UI_Check.Checked = True Then
                isEnabledButton = True
            End If
        Next

        Me.UI_cmdSubmit.Enabled = isEnabledButton
        Me.UI_cmdConfirm.Enabled = isEnabledButton
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
            Dim dtSaleList As RmaDTO.vwSale_WorkListDataTable = Session("_dtSaleList")

            '========================================================================================================================================================================================================================================================================================
            '2011/09/16 START
            '調整項目:
            '1.業務報價時, 該RMA單有任一筆item, 經過Submit by Sales 後, 就可以維修.
            '2.業務報價時, 該RMA單可針對任一筆item做 Submit by Sales 或 Submit by Customer.
            '原Code
            '========================================================================================================================================================================================================================================================================================
            'For i = 0 To dtSaleList.Rows.Count - 1
            '    Dim RMAD_STATUS As String = dtSaleList.Rows(i)("RMAD_STATUS").ToString().Trim()
            '    Dim RMAD_ID As String = dtSaleList.Rows(i)("RMAD_ID").ToString().Trim()
            '    Dim RMASQ_ID As String = dtSaleList.Rows(i)("RMASQ_ID").ToString().Trim()

            '    If Convert.ToInt16(RMAD_STATUS) = 35 Or Convert.ToInt16(RMAD_STATUS) = 40 Then
            '        Dim dr As RmaDTO.SalesQuoted_ConfirmedRow = dtConfirmed.NewSalesQuoted_ConfirmedRow

            '        dr.RMASQ_ID = RMASQ_ID
            '        dr.RMAD_ID = RMAD_ID

            '        dr.RMASQ_SALEAD = Session("_UserID")
            '        dr.RMASQ_SALEADNAME = Session("_UserName")
            '        dr.RMASQ_SALEDATE = Date.Now
            '        dr.RMAD_STATUS = iStatus

            '        'RMASQ_CLIENTCONFIRM (1.客戶確認, 2.業務帶客戶確認)-->如果幫客戶確認, CLIENTCONFIRM=2
            '        If iStatus = 50 Then
            '            dr.RMASQ_CLIENTCONFIRM = 2
            '        End If

            '        dtConfirmed.AddSalesQuoted_ConfirmedRow(dr)
            '    End If
            'Next


            '========================================================================================================================================================================================================================================================================================
            '改成
            '========================================================================================================================================================================================================================================================================================
            For i = 0 To dtSaleList.Rows.Count - 1
                Dim RMAD_STATUS As String = dtSaleList.Rows(i)("RMAD_STATUS").ToString().Trim()
                Dim RMAD_ID As String = dtSaleList.Rows(i)("RMAD_ID").ToString().Trim()
                Dim RMASQ_ID As String = dtSaleList.Rows(i)("RMASQ_ID").ToString().Trim()
            Next


            For i = 0 To Me.UI_dvSales.Rows.Count - 1
                Dim UI_Check As CheckBox = Me.UI_dvSales.Rows(i).FindControl("UI_Check")
                Dim UI_Status As Label = Me.UI_dvSales.Rows(i).FindControl("UI_Status")
                Dim UI_RMADID As Label = Me.UI_dvSales.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMASQID As Label = Me.UI_dvSales.Rows(i).FindControl("UI_RMASQID")

                If (Convert.ToInt16(UI_Status.Text) = 35 Or Convert.ToInt16(UI_Status.Text) = 40) And UI_Check.Checked = True Then
                    Dim dr As RmaDTO.SalesQuoted_ConfirmedRow = dtConfirmed.NewSalesQuoted_ConfirmedRow

                    dr.RMASQ_ID = UI_RMASQID.Text.Trim()
                    dr.RMAD_ID = UI_RMADID.Text.Trim()

                    dr.RMASQ_SALEAD = Session("_UserID")
                    dr.RMASQ_SALEADNAME = Session("_UserName")
                    dr.RMASQ_SALEDATE = Date.Now
                    dr.RMAD_STATUS = iStatus

                    'RMASQ_CLIENTCONFIRM (1.客戶確認, 2.業務帶客戶確認)-->如果幫客戶確認, CLIENTCONFIRM=2
                    If iStatus = 50 Then
                        dr.RMASQ_CLIENTCONFIRM = 2
                    End If

                    dtConfirmed.AddSalesQuoted_ConfirmedRow(dr)
                End If
            Next
            '========================================================================================================================================================================================================================================================================================
            '2011/09/16 END
            '========================================================================================================================================================================================================================================================================================

            oSale.SalesQuoted_Confirmed(dtConfirmed)

            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                If iStatus = 40 Then        '40.Sales Confirmed
                    Dim isSendMail As Boolean = SalesConfirmed_SendMail()
                End If

                If iStatus = 50 Then        '50.Confirm by Sales -->業務帶客戶確認
                    Dim isSendMail As Boolean = ConfirmbySales_SendMail()
                End If

                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Sales_WorkList.aspx")
            End If
        End Try
    End Sub

    Private Function SalesConfirmed_SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try

            dtCustomer = oCustomer.QueryUser(Me.UI_lblAccountIDText.Text.Trim(), Me.UI_lblApplicantIDText.Text.Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                '================================================================================================================================================================================================================
                '業務報價確認 -->對象(顧客)
                '================================================================================================================================================================================================================
                Dim CU_NAME As String = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                Dim MailSales As String = ""
                Dim SalesName As String = ""
                If CU_SALESID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_SALESID, "")
                    MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                    SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim MailAssistant As String = ""
                Dim AssistantName As String = ""
                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim sSubject As String = _oLanguage.getText("Mail", "023", ctlLanguage.eumType.Mail)
                Dim sBody As String = _oLanguage.getText("Mail", "024", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                If MailUser.Trim() <> "" Then
                    sSubject = sSubject.Replace("[$RMA No$]", Me.UI_lblRMANoText.Text.Trim())

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                    End If

                    sBody = sBody.Replace("[$Customer User Name$]", Me.UI_lblApplicantText.Text.Trim())
                    sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                    sBody = sBody.Replace("[$RMA Request No$]", Me.UI_lblRMANoText.Text.Trim())
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        MailUser = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, MailUser, _MailCC)
                End If
            End If

        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function

    Private Function ConfirmbySales_SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim j As Integer = 0

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try
            Dim sSubject As String = _oLanguage.getText("Mail", "027", ctlLanguage.eumType.Mail)
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            Dim MailSales As String = ""
            Dim SalesName As String = ""
            Dim MailAssistant As String = ""
            Dim AssistantName As String = ""


            dtCustomer = oCustomer.QueryUser(Me.UI_lblAccountIDText.Text.Trim(), Me.UI_lblApplicantIDText.Text.Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                '================================================================================================================================================================================================================
                '業務報價直接幫客戶確認  -->對象(顧客) + 維修
                '================================================================================================================================================================================================================
                Dim CU_NAME As String = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                If CU_SALESID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_SALESID, "")
                    MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                    SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim sBody As String = _oLanguage.getText("Mail", "028", ctlLanguage.eumType.Mail)
                If MailUser.Trim() <> "" Then
                    sSubject = sSubject.Replace("[$RMA No$]", Me.UI_lblRMANoText.Text.Trim())

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                    End If

                    sBody = sBody.Replace("[$Customer User Name$]", Me.UI_lblApplicantText.Text.Trim())
                    sBody = sBody.Replace("[$RMA Request No$]", Me.UI_lblRMANoText.Text.Trim())
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        MailUser = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, MailUser, _MailCC)
                End If
            End If



            '================================================================================================
            '對象維修
            '================================================================================================
            Dim sSubject_Repair As String = _oLanguage.getText("Mail", "029", ctlLanguage.eumType.Mail)
            sSubject_Repair = sSubject.Replace("[$RMA No$]", Me.UI_lblRMANoText.Text.Trim())

            Dim oRMA As New ctlRMA
            Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
            Dim Repaire_Name As String = ""                    '維修人員

            Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(Me.UI_lblRMANoText.Text.Trim())
            For j = 0 To arrRepaire.Count - 1
                Dim arrList() As String = arrRepaire(j)

                Repaire_Name = arrList(0).Trim()
                Repaire_EMAIL = arrList(1).Trim()

                Dim sBody_Repair As String = _oLanguage.getText("Mail", "030", ctlLanguage.eumType.Mail)
                If Repaire_Name.Trim <> "" Then
                    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", Repaire_Name)
                Else
                    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "")
                End If

                If MailSales.Trim() = "" Then
                    sBody_Repair = sBody_Repair.Replace("[$Sales Name$]", "")
                    sBody_Repair = sBody_Repair.Replace("/", "")
                Else
                    sBody_Repair = sBody_Repair.Replace("[$Sales Name$]", SalesName)
                End If

                If MailAssistant.Trim() = "" Then
                    sBody_Repair = sBody_Repair.Replace("[$Assistant Name$]", "")
                    sBody_Repair = sBody_Repair.Replace("/", "")
                Else
                    sBody_Repair = sBody_Repair.Replace("[$Assistant Name$]", AssistantName)
                End If

                If Repaire_EMAIL.Trim <> "" Then
                    sBody_Repair = sBody_Repair.Replace("[$RMA Request No$]", Me.UI_lblRMANoText.Text.Trim())
                    sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_EMAIL, _MailCC)
                End If
            Next





        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function

    '===================================================================================================================
    'Start 新增報價單下載功能 by Hugh 2011-07-21 
    '===================================================================================================================

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim UI_RMAID As String = UI_lblRMANoText.Text.Trim()

        _ReportToPDF = UI_RMAID & "_Quote_" & oCommon.GetRandomizeNum() & ".pdf"

        Call RunPrint(UI_RMAID)

    End Sub

    Protected Sub RunPrint(ByVal RMAID As String)
        Dim i As Integer = 0
        Dim iTotalQuote As Double = 0
        Dim oRMARequest As New ctlCipherExtend.Quote
        Dim dtReport As New CipherExtendDTO.QuotaReportDataTable

        dtReport = oRMARequest.getQuoteReportData(Session("_LanguageID").ToString().Trim(), RMAID)

        For i = 0 To dtReport.Rows.Count - 1
            fTotalQuote = fTotalQuote + CType(dtReport.Rows(i)("IT_Quote").ToString(), Double)
        Next

        Call Print(dtReport)

    End Sub

    Private Sub Print(ByVal dtReport As CipherExtendDTO.QuotaReportDataTable)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptQuote.rpt"))
        ReportDoc.SetDataSource(oReport)

        ReportDoc.SetParameterValue("Repairer", Session("_UserName"))
        ReportDoc.SetParameterValue("TotalQuote", fTotalQuote)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()

    End Sub

    'Public Sub ExportSetup()
    '    If Not System.IO.Directory.Exists(_Reoprt_FilePath) Then
    '        System.IO.Directory.CreateDirectory(_Reoprt_FilePath)
    '    End If

    '    myDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    myExportOptions = ReportDoc.ExportOptions
    '    ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
    'End Sub

    'Public Sub ConfigureExportToPdf()
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & _ReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Dim szFileName As String = Server.MapPath("~\FILE\Quote\" & _ReportToPDF)

    '    Dim sScript As String = ""
    '    sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');"

    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Success", sScript, True)
    'End Sub

    '===================================================================================================================
    'End 新增報價單下載功能 by Hugh 2011-07-21 
    '===================================================================================================================

End Class

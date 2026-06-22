Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class HQQuote_item
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _Customer_ExceptionCharge As String = ConfigurationSettings.AppSettings("Customer_ExceptionCharge")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtHQRequest") = Nothing

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_CUNO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_CUNO")
                Dim UI_lblPreviousPage_COMPNO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_COMPNO")

                Me.RMA_NO = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.CU_NO = UI_lblPreviousPage_CUNO.Text.Trim()
                Me.COMP_NO = UI_lblPreviousPage_COMPNO.Text.Trim()

                Call setDefault()

                Call QueryData_Detail(0)
            End If

        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Me.UI_lblProductTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)

        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "082", ctlLanguage.eumType.Tag)

        Me.uiLbl_Repair_Manpower.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_Parts.Text = _oLanguage.getText("RMA", "087", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_TotalText.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)

        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)

    End Sub

#Region "Requested Information"

    Private Sub QueryData_Detail(ByVal iPageIndex As Integer)
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

        Me.UI_cmdSubmit.Visible = True

        dtRequest = oClient.Query02(Me.RMA_NO, "", "", "", "", "-1", "", "91", Me.COMP_NO, "RMAD_RMANO desc,RMAD_SERIALNO")
        If dtRequest.Rows.Count > 0 Then
            Me.RMAD_ID = dtRequest.Rows(0)("RMAD_ID").ToString().Trim()
            Call QueryData_Head()
        End If

        Call QueryByRepairQuotedDetail()

        Call ArrangementData(dtRequest)
        Session("_dtHQRequest") = dtRequest
        Dim dvRequest As DataView = dtRequest.DefaultView
        Me.ViewState("_SortExpression") = "RMAD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvRequest.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Request_DataBind(dvRequest, iPageIndex)
    End Sub

    Private Sub QueryData_Head()
        Me.uiTxt_Repair_ManHour.Text = ""
        Me.uiLbl_CURRENCYCODE.Text = ""
        Me.uiLbl_Repair_PartsTotal.Text = ""
        Me.uiLbl_Repair_Total.Text = ""

        Dim oRepairQuoting As New ctlRMA.Repair_Quoting
        Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable
        dtRepairQuoting = oRepairQuoting.Query(Me.RMAD_ID, "91")
        If dtRepairQuoting.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_QuotingRow = dtRepairQuoting.Rows(0)

            '==========================================================================================================================================================================================================================================
            '費用
            '==========================================================================================================================================================================================================================================
            If dr.IsRMARQ_LABORHOURNull = False Then
                Me.uiTxt_Repair_ManHour.Text = dr.RMARQ_LABORHOUR.ToString().Trim()
            End If
            If dr.IsRMARQ_CURRENCYCODENull = False Then
                Me.uiLbl_CURRENCYCODE.Text = dr.RMARQ_CURRENCYCODE.Trim()
            End If

            Call QueryCustomer(dr.RMA_CUNO)

            Me.UI_RMAD_ISWARRANTY.Text = "0"
            If dr.IsRMAD_ISWARRANTYNull = False Then
                Me.UI_RMAD_ISWARRANTY.Text = dr.RMAD_ISWARRANTY.ToString()
            End If



            Me.UI_RMAD_ISCW.Text = "0"
            If dr.IsRMAD_ISCWNull = False Then
                Me.UI_RMAD_ISCW.Text = dr.RMAD_ISCW.ToString()
            End If

            'If dr.IsRMARQ_MATERIALCOSTNull = False Then
            '    Me.uiLbl_Repair_PartsTotal.Text = dr.RMARQ_MATERIALCOST.ToString().Trim()
            'End If
            'If dr.IsRMARQ_QUOTENull = False Then
            '    Me.uiLbl_Repair_Total.Text = dr.RMARQ_QUOTE.ToString().Trim()
            'End If
        End If

    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0

        If dtRequest.Columns("SeqID") Is Nothing Then
            dtRequest.Columns.Add("SeqID")
            dtRequest.Columns.Add("Warranty")
            dtRequest.Columns.Add("CWEndWarr")
            dtRequest.Columns.Add("SWEndWarr")
            dtRequest.Columns.Add("RequestDate")
            dtRequest.Columns.Add("Quoted")
            dtRequest.Columns.Add("Amount")
            dtRequest.Columns.Add("Status")
            dtRequest.Columns.Add("Assign")
        End If

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.tmpRequest_ListRow = dtRequest.Rows(i)
            dtRequest.Rows(i)("SeqID") = i + 1

            '保固日期
            If dr.IsRMAD_WARRANTYNull = False Then
                dtRequest.Rows(i)("Warranty") = Convert.ToDateTime(dtRequest.Rows(i)("RMAD_WARRANTY").ToString()).ToShortDateString()
            Else
                dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRequest.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select
            End If

            Dim oExport As New ctlRMA.Export
            Dim sCWEnd As String = oExport.getWarrantyCW(dtRequest.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRequest.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If
            Dim sSWEnd As String = oExport.getWarrantySW(dtRequest.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRequest.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If

            'Assign
            dtRequest.Rows(i)("Assign") = ""
            Dim RMA_COMPNO As String = dtRequest.Rows(i)("RMA_COMPNO").ToString().Trim()
            Dim RMAR_COMPNO As String = dtRequest.Rows(i)("RMAR_COMPNO").ToString().Trim()
            If RMA_COMPNO <> RMAR_COMPNO And RMAR_COMPNO <> "" Then
                dtRequest.Rows(i)("Assign") = dtRequest.Rows(i)("COMP_NAME")
            End If

            '申請日期
            dtRequest.Rows(i)("RequestDate") = Convert.ToDateTime(dtRequest.Rows(i)("RMAD_CSTMP").ToString()).ToShortDateString()

            '維修總金額
            '1.先依維修單的總金額為主
            '2.若維修單無資料,再取報價單總金額
            Dim sQuoted As String = ""
            If Me.RMAD_ID = dr.RMAD_ID Then
                If dr.IsRMAR_QUOTENull = False Then
                    sQuoted = dr.RMAR_CURRENCYCODE.ToString().Trim() & " " & Me.uiLbl_Repair_Total.Text.Trim()
                ElseIf dr.IsRMARQ_QUOTENull = False Then
                    sQuoted = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " " & Me.uiLbl_Repair_Total.Text.Trim()
                End If

            Else
                If dr.IsRMAR_QUOTENull = False Then
                    sQuoted = dr.RMAR_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMAR_QUOTE.ToString()).ToString("N")
                ElseIf dr.IsRMARQ_QUOTENull = False Then
                    sQuoted = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARQ_QUOTE.ToString()).ToString("N")
                End If
            End If
            dtRequest.Rows(i)("Quoted") = sQuoted.Trim()





            '業務總金額
            '1.先依業務出貨單的總金額為主
            '2.若業務出貨單無資料,再取業務報價單總金額
            Dim sAmount As String = ""
            If dr.IsRMARSD_QUOTENull = False Then
                sAmount = dr.RMARSD_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARSD_QUOTE.ToString()).ToString("N")
            ElseIf dr.IsRMASQ_QUOTENull = False Then
                sAmount = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMASQ_QUOTE.ToString()).ToString("N")
            End If
            dtRequest.Rows(i)("Amount") = sAmount.Trim()


            '如果 RMAD_STATUS=60 and 尚未有填維修單, 單身狀態顯示為 (Repairing)
            If dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim() = "60" And dtRequest.Rows(i)("RMAR_REPAIRAD").ToString().Trim() = "" Then
                dtRequest.Rows(i)("Status") = _oLanguage.getText("Common", "068", ctlLanguage.eumType.Status)
            Else
                dtRequest.Rows(i)("Status") = oCommon.ConvertToItemStatusText(dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim(), dtRequest.Rows(i)("RMAD_ID").ToString().Trim())
            End If

            If Convert.ToInt16(dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim()) < 30 Then
                Me.UI_cmdSubmit.Visible = False
            End If
        Next

    End Sub

    Private Sub Request_DataBind(ByVal dvRequest As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRequest
        Me.UI_dvRequest.DataBind()
    End Sub

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtHQRequest") Is Nothing Then
            Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtHQRequest")
            Dim dvReceiveList As DataView = dtReceiveList.DefaultView
            Call Request_DataBind(dvReceiveList, iPageIndex)

        Else
            Call QueryData_Detail(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            Dim UI_RMAR_COMPNO As Label = e.Row.FindControl("UI_RMAR_COMPNO")
            Dim UI_RMADID As Label = e.Row.FindControl("UI_RMADID")
            Dim UI_RMAD_SERIALNO As LinkButton = e.Row.FindControl("UI_RMAD_SERIALNO")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")

            If UI_RMADID.Text.Trim().Equals(Me.RMAD_ID) Then
                e.Row.BackColor = Drawing.Color.Pink
            End If

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(4).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(4).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(4).ForeColor = Drawing.Color.Red
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

                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "DataControlPagerLinkButton".ToLower() Then
                    Dim oLinkButton As LinkButton = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLinkButton.Attributes.Add("onclick", "onProgress('Process')")
                End If

            Next
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand

        If e.CommandName = "cmdChangeSn" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)

            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            'Dim cmdQuoting As LinkButton = row.FindControl("UI_Quoting")

            Me.RMAD_ID = UI_RMADID.Text.Trim()
            Me.RMA_NO = UI_RMANO.Text.Trim()

            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            dtRequest = Session("_dtHQRequest")
            Dim dvRequest As DataView = dtRequest.DefaultView

            Call QueryData_Head()
            Call Request_DataBind(dvRequest, UI_dvRequest.PageIndex)
            Call QueryByRepairQuotedDetail()
        End If
    End Sub

#End Region

#Region "Replace Component"
    Private Sub QueryByRepairQuotedDetail()
        'Response.Write(Me.RMAD_ID)

        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRepairQuotedDetail_HQ As New RmaDTO.RMAREPAIR_QUOTED_DETAIL_HQDataTable

        dtRepairQuotedDetail_HQ = oRepairQuoted.QueryByRepairQuotedDetail_HQ(Me.RMAD_ID)

        Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail_HQ, 0)
    End Sub

    Private Sub RepairQuotedDetail_DataBind(ByVal dtRepairQuotedDetail_HQ As RmaDTO.RMAREPAIR_QUOTED_DETAIL_HQDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairQuotedDetail_HQ") = dtRepairQuotedDetail_HQ
        Dim dvRepairDetail_HQ As DataView = dtRepairQuotedDetail_HQ.DefaultView()
        dvRepairDetail_HQ.RowFilter = "RMARQD_MARK=0"

        Call RepairQuotedDetail_CalTotalAmt(dvRepairDetail_HQ)

        Me.UI_dvRepairDetail.DataSource = dtRepairQuotedDetail_HQ.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub

    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblWaive As Label = e.Item.FindControl("lblWaive")
            Dim lblOption As Label = e.Item.FindControl("lblOption")
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHLocation As Label = e.Item.FindControl("lblHLocation")
            Dim lblHImproper As Label = e.Item.FindControl("lblHImproper")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblHPrice As Label = e.Item.FindControl("lblHPrice")

            lblWaive.Text = _oLanguage.getText("RMA", "405", ctlLanguage.eumType.Tag)
            lblOption.Text = _oLanguage.getText("RMA", "406", ctlLanguage.eumType.Tag)
            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHLocation.Text = _oLanguage.getText("RMA", "100", ctlLanguage.eumType.Tag)
            lblHImproper.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblHPrice.Text = _oLanguage.getText("RMA", "104", ctlLanguage.eumType.Tag)
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chhWaive As CheckBox = e.Item.FindControl("chhWaive")
            Dim UI_lblRMARQD_WAIVE As Label = e.Item.FindControl("UI_lblRMARQD_WAIVE")
            If chhWaive.Checked = True Then
                UI_lblRMARQD_WAIVE.Text = "V"
            End If

            Dim chkOption As CheckBox = e.Item.FindControl("chkOption")
            Dim UI_lblRMARQD_OPTION As Label = e.Item.FindControl("UI_lblRMARQD_OPTION")
            If chkOption.Checked = True Then
                UI_lblRMARQD_OPTION.Text = "V"
            End If


            Dim lblIMPROPERUSAGE As Label = e.Item.FindControl("lblIMPROPERUSAGE")
            Dim UI_RMARQD_IMPROPERUSAGE As Label = e.Item.FindControl("UI_RMARQD_IMPROPERUSAGE")

            UI_RMARQD_IMPROPERUSAGE.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            If lblIMPROPERUSAGE.Text.Trim() = "1" Then
                UI_RMARQD_IMPROPERUSAGE.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End If
        End If

    End Sub

    Private Sub RepairQuotedDetail_CalTotalAmt(ByVal dvRepairDetail_HQ As DataView)
        Dim i As Integer = 0
        Dim RMARQD_PRICE As Double = 0
        Dim iTotalParts As Double = 0

        For i = 0 To dvRepairDetail_HQ.Count - 1
            'isAbnormal: 0.正常, 1.異常
            If Convert.ToInt16(dvRepairDetail_HQ(i)("isAbnormal")) = 1 Then
                Dim RMARQD_QTY As Double = Convert.ToDouble(dvRepairDetail_HQ(i)("RMARQD_QTY"))
                Dim RMARQD_MATERIALCOST As Double = Convert.ToDouble(dvRepairDetail_HQ(i)("RMARQD_MATERIALCOST"))
                Dim RMARQD_WAIVE As Double = Convert.ToDouble(dvRepairDetail_HQ(i)("RMARQD_WAIVE"))
                Dim RMARQD_IMPROPERUSAGE As Integer = Convert.ToInt16(dvRepairDetail_HQ(i)("RMARQD_IMPROPERUSAGE"))
                RMARQD_PRICE = PartsRule_Exception(RMARQD_QTY, RMARQD_MATERIALCOST, RMARQD_WAIVE, RMARQD_IMPROPERUSAGE)
                dvRepairDetail_HQ(i)("RMARQD_PRICE") = RMARQD_PRICE
            Else
                RMARQD_PRICE = Convert.ToDouble(dvRepairDetail_HQ(i)("RMARQD_PRICE"))
            End If

            iTotalParts = iTotalParts + RMARQD_PRICE
        Next

        If Me.uiTxt_Repair_ManHour.Text.Trim() <> "" Then
            Me.uiLbl_Repair_PartsTotal.Text = iTotalParts.ToString()
            Me.uiLbl_Repair_Total.Text = Convert.ToDecimal(Me.uiTxt_Repair_ManHour.Text) + iTotalParts
        End If
    End Sub

#End Region

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessageFailed As String = ""
        Dim sMessageOK As String = ""

        Dim dtRepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
        Dim dtRepairQuoted_Detail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable

        Dim ctlRMA As New ctlRMA.Repair_Quoting
        Dim ctlRepairBOM As New ctlRMA.RepairBOM

        Try

            '================================================================================================================================================================================================
            '重新補齊 RMA單裡的 維修報價 零件檔 規格及單價
            '================================================================================================================================================================================================
            dtRepairQuoted = ctlRMA.qryRepair_Quoted(Me.RMA_NO)
            For i = 0 To dtRepairQuoted.Rows.Count - 1
                Dim drHead As RmaDTO.RMARepair_QuotedRow = dtRepairQuoted.Rows(i)
                Dim RMARQ_ID As String = drHead.RMARQ_ID.Trim()
                Dim RMARQ_RMADID As String = drHead.RMARQ_RMADID.Trim()

                Dim TotalPrice_Parts As Double = 0
                dtRepairQuoted_Detail = ctlRMA.qryRepair_Quoted_Detail(RMARQ_RMADID)
                For j = 0 To dtRepairQuoted_Detail.Rows.Count - 1
                    Dim drDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuoted_Detail.Rows(j)

                    Dim RPBOM_MATERIALCOST As Double = 0        '零件單價
                    Dim RPBOM_DESC As String = ""               '零件規格
                    Dim sPartsNo As String = drDetail.RMARQD_NPARTNO.Trim()
                    Dim sLocation As String = ""

                    Dim RMARQD_PRICE As Double = drDetail.RMARQD_PRICE

                    Dim RMARQD_DESC As String = ""
                    If drDetail.IsRMARQD_DESCNull = False Then
                        RMARQD_DESC = drDetail.RMARQD_DESC.Trim()
                    End If
                    Dim RMARQD_MATERIALCOST As Double = drDetail.RMARQD_MATERIALCOST


                    If RMARQD_DESC.Trim() = "" Or RMARQD_MATERIALCOST = 0 Then
                        dtRepairBOM = ctlRepairBOM.Query(Me.COMP_NO, sPartsNo, sLocation)
                        If dtRepairBOM.Rows.Count > 0 Then
                            RPBOM_MATERIALCOST = Convert.ToDouble(dtRepairBOM.Rows(0)("RPBOM_MATERIALCOST"))
                            RPBOM_DESC = dtRepairBOM.Rows(0)("RPBOM_DESC").ToString().Trim()

                            If drDetail.IsRMARQD_DESCNull = True Then
                                drDetail.RMARQD_DESC = RPBOM_DESC
                            Else
                                If drDetail.RMARQD_DESC.Trim() = "" Then
                                    drDetail.RMARQD_DESC = RPBOM_DESC
                                End If
                            End If

                            drDetail.RMARQD_MATERIALCOST = RPBOM_MATERIALCOST

                            RMARQD_PRICE = PartsRule_Exception(drDetail.RMARQD_QTY, drDetail.RMARQD_MATERIALCOST, drDetail.RMARQD_WAIVE, drDetail.RMARQD_IMPROPERUSAGE)
                            drDetail.RMARQD_PRICE = RMARQD_PRICE
                            drDetail.RMARQD_ASSIGEPRICE = oCommon.ConvertCurrency(drDetail.RMARQD_CURRENCYRATE, drDetail.RMARQD_PRICE, drDetail.RMARQD_ASSIGECURRENCYRATE)
                        End If
                    End If

                    TotalPrice_Parts = TotalPrice_Parts + drDetail.RMARQD_PRICE
                Next


                'update  RMAREPAIR_QUOTED
                Dim RMARQ_LABORHOUR As Double = drHead.RMARQ_LABORHOUR  '工時
                Dim RMARQ_LABORPRICE As Double = drHead.RMARQ_LABORPRICE  '工時單價
                Dim RMARQ_MATERIALCOST As Double = TotalPrice_Parts  '零件費用
                Dim RMARQ_QUOTE As Double = (RMARQ_LABORHOUR * RMARQ_LABORPRICE) + RMARQ_MATERIALCOST '費用加總(報價)

                Dim RMARQ_ASSIGLABORCOST As Double = oCommon.ConvertCurrency(drHead.RMARQ_CURRENCYRATE, (RMARQ_LABORHOUR * RMARQ_LABORPRICE), drHead.RMARQ_ASSIGECURRENCYRATE)  '轉換成 指派的維修中心 --> 工時費用
                Dim RMARQ_ASSIGMATERIALCOST As Double = oCommon.ConvertCurrency(drHead.RMARQ_CURRENCYRATE, RMARQ_MATERIALCOST, drHead.RMARQ_ASSIGECURRENCYRATE)                 '轉換成 指派的維修中心 --> 零件費用
                Dim RMARQ_ASSIGEQUOTE As Double = oCommon.ConvertCurrency(drHead.RMARQ_CURRENCYRATE, RMARQ_QUOTE, drHead.RMARQ_ASSIGECURRENCYRATE)                              '轉換成 指派的維修中心 --> 費用加總(報價)

                'update  RMAREPAIR_QUOTED_DETAIL
                ctlRMA.Edit_RepairQuotedDetail(RMARQ_ID, RMARQ_MATERIALCOST, RMARQ_QUOTE, RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, dtRepairQuoted_Detail)
            Next



            If ctlRMA.FlowCase01_CHKQuotedItem_Abnormal(Me.RMA_NO) = True Then
                Throw New Exception(_oLanguage.getText("RMA", "409", ctlLanguage.eumType.Tag))
            Else
                Dim ctlSale As New ctlRMA.Sale

                dtRepairQuoted = ctlRMA.qryRepair_Quoted(Me.RMA_NO)
                For i = 0 To dtRepairQuoted.Rows.Count - 1
                    Dim drRepair_Quoted As RmaDTO.RMARepair_QuotedRow = dtRepairQuoted.Rows(i)

                    Dim dtSALE_QUOTED As New RmaDTO.RMASALE_QUOTEDDataTable
                    Dim drSALE_QUOTED As RmaDTO.RMASALE_QUOTEDRow = dtSALE_QUOTED.NewRMASALE_QUOTEDRow
                    drSALE_QUOTED.RMASQ_ID = Guid.NewGuid().ToString()
                    drSALE_QUOTED.RMASQ_RMADID = drRepair_Quoted.RMARQ_RMADID

                    drSALE_QUOTED.RMASQ_LABORCOST = drRepair_Quoted.RMARQ_LABORHOUR * drRepair_Quoted.RMARQ_LABORPRICE
                    drSALE_QUOTED.RMASQ_MATERIALCOST = Math.Round(drRepair_Quoted.RMARQ_MATERIALCOST, 2)
                    drSALE_QUOTED.RMASQ_QUOTE = drRepair_Quoted.RMARQ_QUOTE

                    drSALE_QUOTED.RMASQ_CURRENCYCODE = drRepair_Quoted.RMARQ_CURRENCYCODE
                    drSALE_QUOTED.RMASQ_CURRENCYRATE = drRepair_Quoted.RMARQ_CURRENCYRATE
                    drSALE_QUOTED.RMASQ_DESC = ""

                    drSALE_QUOTED.RMASQ_AD = Session("_UserID")
                    drSALE_QUOTED.RMASQ_ADNAME = Session("_UserName")
                    drSALE_QUOTED.RMASQ_CSTMP = Date.Now
                    drSALE_QUOTED.RMASQ_LUAD = Session("_UserID")
                    drSALE_QUOTED.RMASQ_LUADNAME = Session("_UserName")
                    drSALE_QUOTED.RMASQ_LUSTMP = Date.Now

                    drSALE_QUOTED.RMASQ_SALEAD = Session("_UserID")
                    drSALE_QUOTED.RMASQ_SALEADNAME = Session("_UserName")
                    drSALE_QUOTED.RMASQ_SALEDATE = Date.Now

                    'drSALE_QUOTED.RMASQ_CLIENTCONFIRM = 1       '1.客戶確認, 2.業務帶客戶確認
                    dtSALE_QUOTED.AddRMASALE_QUOTEDRow(drSALE_QUOTED)

                    ctlSale.SaveAdd_SalesConfirmed(dtSALE_QUOTED)               '業務報價 並 確認
                Next

                Confirmby_SendMail()
            End If

            ctlRMA.LessApprovAmountToRepairHQ(dtRepairQuoted)
            blnFlag = True

        Catch ex As Exception
            sMessageFailed = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessageFailed)
            Else
                sMessageOK = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageBySuccess(sMessageOK, ascx_ucMessage.eumTransferURL.Redirect, "HQQuote_List.aspx")
            End If
        End Try



    End Sub

#Region "相關金額計算"

    ''' <summary>
    ''' 報價零件金額 - 例外規格計算
    ''' </summary>
    ''' <param name="RMARQD_MATERIALCOST"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PartsRule_Exception(ByVal RMARQD_QTY As Double, ByVal RMARQD_MATERIALCOST As Double, ByVal RMARQD_WAIVE As Integer, ByVal RMARQD_IMPROPERUSAGE As Integer) As Double
        Dim i As Integer = 0
        Dim RMARQD_PRICE As Double = 0
        Dim CU_DISCOUNT_OFF As Double = 1

        If Me.UI_CU_DISCOUNT_OFF.Text.Trim <> "" Then
            CU_DISCOUNT_OFF = Convert.ToDouble(Me.UI_CU_DISCOUNT_OFF.Text)
        End If

        '計算折扣後的零件金額
        If RMARQD_QTY > 0 And RMARQD_MATERIALCOST > 0 Then
            RMARQD_PRICE = Math.Round((RMARQD_QTY * RMARQD_MATERIALCOST) * CU_DISCOUNT_OFF, 2)
        End If

        '1. IF ISWARRANTY=Y OR ISCW=Y THEN RMARQD_PRICE = 0
        If RMARQD_IMPROPERUSAGE = 0 Then
            If Me.UI_RMAD_ISWARRANTY.Text = "1" Or Me.UI_RMAD_ISCW.Text = "1" Then
                RMARQD_PRICE = 0
            End If
        End If
        ''2. 客戶編號為'Ni.'開頭的, RMARQD_PRICE = 0
        Dim arrCustomer() As String = _Customer_ExceptionCharge.Trim().Split(",")
        For i = 0 To arrCustomer.Length - 1
            If Me.CU_NO.IndexOf(arrCustomer(i).ToString().Trim()) <> -1 Then
                RMARQD_PRICE = 0
                Exit For
            End If
        Next

        'waive：表示此零件是我方吸收必修，維修收費價格會是0；
        If RMARQD_WAIVE = 1 Then
            RMARQD_PRICE = 0
        End If

        Return RMARQD_PRICE
    End Function

#End Region

    Private Function Confirmby_SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim j As Integer = 0

        Dim ctlAdmin As New ctlAdmin
        Dim ctlRMA As New ctlRMA
        Dim ctlRequested As New ctlRMA.Requested
        Dim ctlCustomer As New ctlCustomer.CustomerUser

        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim dtRMAHead As New RmaDTO.RMADataTable
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim ctlMail As New ctlMail

        Try
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
            Dim sSubject As String = _oLanguage.getText("Mail", "411", ctlLanguage.eumType.Mail)
            Dim sBody As String = _oLanguage.getText("Mail", "412", ctlLanguage.eumType.Mail)


            Dim MailSales As String = ""
            Dim SalesName As String = ""
            Dim MailAssistant As String = ""
            Dim AssistantName As String = ""

            dtRMAHead = ctlRequested.QueryByRMAHead(Me.RMA_NO)
            If dtRMAHead.Rows.Count > 0 Then
                Dim dr As RmaDTO.RMARow = dtRMAHead.Rows(0)

                Dim RMA_CUNO As String = dr.RMA_CUNO.ToString().Trim()
                Dim RMA_ACCOUNTID As String = dr.RMA_ACCOUNTID.ToString().Trim()
                Dim RMA_APPLICANT As String = dr.RMA_APPLICANT.ToString().Trim()
                Dim MailUser As String = dr.RMA_MAIL.ToString().Trim()      '客戶mail

                dtCustomer = ctlCustomer.QueryUser(RMA_CUNO, RMA_ACCOUNTID, "")
                If dtCustomer.Rows.Count > 0 Then
                    'Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                    '================================================================================================================================================================================================================
                    '業務報價直接幫客戶確認  -->對象(顧客) + 維修
                    '================================================================================================================================================================================================================
                    Dim CU_NAME As String = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                    Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                    Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                    If CU_SALESID.Trim() <> "" Then
                        dtAdmin = ctlAdmin.Query(CU_SALESID, "")
                        If dtAdmin.Rows.Count > 0 Then
                            MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                            SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                        End If
                    End If

                    If CU_ASSISTANTID.Trim() <> "" Then
                        dtAdmin = ctlAdmin.Query(CU_ASSISTANTID, "")
                        If dtAdmin.Rows.Count > 0 Then
                            MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                            AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                        End If
                    End If

                    If MailUser.Trim() <> "" Then
                        sSubject = sSubject.Replace("[$RMA No$]", Me.RMA_NO)

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

                        sBody = sBody.Replace("[$Customer User Name$]", RMA_APPLICANT)
                        sBody = sBody.Replace("[$RMA Request No$]", Me.RMA_NO)
                        sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            MailUser = ConfigurationManager.AppSettings("MailTo")
                        End If
                        blnFlag = ctlMail.SendMail(sSubject, sBody, MailUser, _MailCC)
                    End If
                End If



                '================================================================================================
                '對象維修
                '================================================================================================
                Dim sSubject_Repair As String = _oLanguage.getText("Mail", "411", ctlLanguage.eumType.Mail)
                sSubject_Repair = sSubject.Replace("[$RMA No$]", Me.RMA_NO)

                Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                Dim Repaire_Name As String = ""                    '維修人員

                Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RMA(Me.RMA_NO)
                For j = 0 To arrRepaire.Count - 1
                    Dim arrList() As String = arrRepaire(j)

                    Repaire_Name = arrList(0).Trim()
                    Repaire_EMAIL = arrList(1).Trim()

                    Dim sBody_Repair As String = _oLanguage.getText("Mail", "413", ctlLanguage.eumType.Mail)
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
                        sBody_Repair = sBody_Repair.Replace("[$RMA Request No$]", Me.RMA_NO)
                        sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = ctlMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_EMAIL, _MailCC)
                    End If
                Next
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

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvRequest.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvRequest.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvRequest.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvRequest.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 取得客戶 Discount 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryCustomer(ByVal RMA_CUNO As String)
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable

        Me.UI_CU_DISCOUNT_OFF.Text = "1"

        Dim ctlCustomer As New ctlCustomer.Customer
        dtCustomer = ctlCustomer.QueryByCompany(RMA_CUNO)
        If dtCustomer.Count > 0 Then
            Dim item As CustomerDTO.VWCUSTOMERRow = dtCustomer.Rows(0)
            If item.IsCU_DISCOUNT_OFFNull = False Then
                Dim CU_DISCOUNT_OFF As Double = 0
                If item.IsCU_DISCOUNT_OFFNull = False Then
                    CU_DISCOUNT_OFF = item.CU_DISCOUNT_OFF
                End If

                Dim CU_SERVICE_CHG As Double = 0
                If item.IsCU_SERVICE_CHGNull = False Then
                    CU_SERVICE_CHG = item.CU_SERVICE_CHG
                End If

                'Me.UI_lblDiscountText.Text = CU_DISCOUNT_OFF.ToString() + " OFF"
                Me.UI_CU_DISCOUNT_OFF.Text = (100 - CU_DISCOUNT_OFF) / 100
            End If
        End If

    End Sub

    ''' <summary>
    ''' 設定RMADID
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMADID</returns>
    ''' <remarks></remarks>
    Public Property RMAD_ID() As String
        Get
            Return Me.ViewState("_RMAD_ID").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMAD_ID") = nNewValue
        End Set
    End Property

    ''' <summary>
    ''' 設定RMA_NO
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMA_NO</returns>
    ''' <remarks></remarks>
    Public Property RMA_NO() As String
        Get
            Return Me.ViewState("_RMA_NO").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMA_NO") = nNewValue
        End Set
    End Property

    ''' <summary>
    ''' 設定CU_NO
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 CU_NO</returns>
    ''' <remarks></remarks>
    Public Property CU_NO() As String
        Get
            Return Me.ViewState("_CU_NO").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_CU_NO") = nNewValue
        End Set
    End Property

    ''' <summary>
    ''' 設定COMP_NO
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 COMP_NO</returns>
    ''' <remarks></remarks>
    Public Property COMP_NO() As String
        Get
            Return Me.ViewState("_COMP_NO").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_COMP_NO") = nNewValue
        End Set
    End Property

End Class

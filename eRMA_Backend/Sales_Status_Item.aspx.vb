Imports DataService
Imports DefLanguage

Partial Class Sales_Status_Item
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call setControls()
            Session("_dtRequest") = Nothing

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

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
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
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

        Dim sRMANo As String = Me.UI_lblPreviousPage_RMANO.Text.Trim
        Dim sCustomerName As String = ""
        Dim sModelNo As String = ""
        Dim sSerial As String = ""
        Dim sRepair As String = "-1"
        Dim sStatus As String = "-1"
        Dim fdate As String = ""
        Dim edate As String = ""

        'Session("_RepairCenter")-->只能看登入者維修點資料
        dtRequest = oClient.Query(sRMANo, sCustomerName, sModelNo, sSerial, sRepair, sStatus, fdate, edate, Session("_RepairCenter"))

        Call ArrangementData(dtRequest)
        Session("_dtRequest") = dtRequest
        Call Request_DataBind(iPageIndex)
    End Sub

    Private Sub Request_DataBind(ByVal iPageIndex As Integer)
        Dim dtRequest As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dtRequest.DefaultView
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0

        If dtRequest.Columns("SeqID") Is Nothing Then
            dtRequest.Columns.Add("SeqID")
            dtRequest.Columns.Add("Warranty")
            dtRequest.Columns.Add("RequestDate")
            dtRequest.Columns.Add("Quoted")
            dtRequest.Columns.Add("Amount")
            dtRequest.Columns.Add("Status")
        End If

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.tmpRequest_ListRow = dtRequest.Rows(i)
            dtRequest.Rows(i)("SeqID") = i + 1

            '依客戶送修什麼 就秀什麼
            If Not dr.IsRMAD_PARTSNNull Then
                dtRequest.Rows(i)("RMAD_SERIALNO") = dtRequest.Rows(i)("RMAD_PARTSN")
            End If

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


            '申請日期
            dtRequest.Rows(i)("RequestDate") = Convert.ToDateTime(dtRequest.Rows(i)("RMAD_CSTMP").ToString()).ToShortDateString()

            '維修總金額
            '1.先依維修單的總金額為主
            '2.若維修單無資料,再取報價單總金額
            Dim sQuoted As String = ""
            If dr.IsRMAR_QUOTENull = False Then
                sQuoted = dr.RMAR_CURRENCYCODE.ToString().Trim() & "  " & Convert.ToDouble(dr.RMAR_QUOTE.ToString()).ToString("N")
            ElseIf dr.IsRMARQ_QUOTENull = False Then
                sQuoted = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARQ_QUOTE.ToString()).ToString("N")
            End If





            dtRequest.Rows(i)("Quoted") = sQuoted.Trim()

            '業務總金額
            '1.先依業務出貨單的總金額為主
            '2.若業務出貨單無資料,再取業務報價單總金額
            Dim sAmount As String = ""
            'If dr.IsRMARSD_QUOTENull = False Then
            '    sAmount = dr.RMARSD_CURRENCYCODE.ToString().Trim() & "  " & Convert.ToDouble(dr.RMARSD_QUOTE.ToString()).ToString("N")
            'ElseIf dr.IsRMASQ_QUOTENull = False Then
            '    sAmount = dr.RMASQ_CURRENCYCODE.ToString().Trim() & "  " & Convert.ToDouble(dr.RMASQ_QUOTE.ToString()).ToString("N")
            'End If

            '業務總金額
            '取業務報價單總金額 , 出貨單總金額已無使用 MODI BY Angel
            If dr.IsRMASQ_QUOTENull = False Then
                sAmount = dr.RMASQ_CURRENCYCODE.ToString().Trim() & "  " & Convert.ToDouble(dr.RMASQ_QUOTE.ToString()).ToString("N")
            Else
                sAmount = " 0.00 "
            End If

            dtRequest.Rows(i)("Amount") = sAmount.Trim()

            'dtRequest.Rows(i)("Status") = oCommon.ConvertToItemStatusText(dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim(), dtRequest.Rows(i)("RMAD_ID").ToString().Trim())

            '如果 RMAD_STATUS=60 and 尚未有填維修單, 單身狀態顯示為 (Repairing)
            If dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim() = "60" And dtRequest.Rows(i)("RMAR_REPAIRAD").ToString().Trim() = "" Then
                dtRequest.Rows(i)("Status") = _oLanguage.getText("Common", "068", ctlLanguage.eumType.Status)
            Else
                dtRequest.Rows(i)("Status") = oCommon.ConvertToItemStatusText(dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim(), dtRequest.Rows(i)("RMAD_ID").ToString().Trim())
            End If
        Next

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "141", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)

            e.Row.Cells(10).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_imgEdit As ImageButton = e.Row.FindControl("UI_imgEdit")

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(3).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(3).Text) < Convert.ToDateTime(e.Row.Cells(4).Text) Then
                    e.Row.Cells(3).ForeColor = Drawing.Color.Red
                End If
            End If

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            If Convert.ToInt16(UI_RMADSTATUS.Text.Trim()) < 20 Then
                UI_imgEdit.Visible = False
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

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRequest") Is Nothing Then
            Call Request_DataBind(iPageIndex)

        Else
            Call QueryDetail(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

        If e.CommandName = "cmdEdit" Then
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMADID.Text = UI_RMADID.Text.ToString().Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.ToString().Trim()
        End If

    End Sub

End Class

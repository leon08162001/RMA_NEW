Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Request_Search_CheckingReport
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")

    Dim i As Integer = 0

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_ModelNo") = ""
            Me.ViewState("_Serial") = ""
            Me.ViewState("_Repair") = "-1"
            Me.ViewState("_Status") = "-1"

            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtRequest") = Nothing

            If IsNothing(Request.QueryString("RMANO")) = False Then
                Dim RMANO As String = Request.QueryString("RMANO").Trim()
                If RMANO <> "" Then
                    Me.ViewState("_RMANo") = _Crypto.Decrypt(RMANO, "")
                End If
            End If

            Call chkFlowCase01()
            Call setControls()
            ' Call QueryData(0)
        End If
    End Sub
#End Region

    Private Sub chkFlowCase01()
        Dim i As Integer = 0

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "01"
                Exit For
            End If
        Next

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim TagText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(Session("_RepairCenter").ToString().Trim(), Me.UI_cboRepairCenter, TagText, True)
        oCommon.getItemStatus(Me.UI_cboStatus)

        Dim fdate As DateTime = Convert.ToDateTime(Me.ViewState("_fdate"))
        Dim edate As DateTime = Convert.ToDateTime(Me.ViewState("_edate"))

        Call oCommon.getYear_DropDownList(Me.UI_cboBYear, fdate.Year.ToString())
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, edate.Year.ToString())

        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth, fdate.Month.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, edate.Month.ToString())

        Call oCommon.getDay_DropDownList(Me.UI_cboBDay, fdate.Day.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, edate.Day.ToString())


        '取得Tag Text   
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_cboStatus.Items(0).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        'Me.UI_cboStatus.Items(1).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(2).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(3).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(4).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(5).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(6).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(7).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(8).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'Me.UI_cboStatus.Items(9).Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(6).HeaderText = _oLanguage.getText("RMA", "036", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(9).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(10).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)


    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sCustomerName As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim sModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
        Dim sSerial As String = Me.ViewState("_Serial").ToString().Trim()
        Dim sRepair As String = Me.ViewState("_Repair").ToString().Trim()
        Dim sStatus As String = Me.ViewState("_Status").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")


        'Session("_RepairCenter")-->只能看登入者維修點資料
        Dim RepairCenter As String = ""
        For i = 0 To Me.UI_cboRepairCenter.Items.Count - 1
            If Me.UI_cboRepairCenter.Items(i).Value <> "-1" Then
                If RepairCenter <> "" Then
                    RepairCenter = RepairCenter & ","
                End If
                RepairCenter = RepairCenter & Me.UI_cboRepairCenter.Items(i).Value
            End If
        Next
        dtRequest = oClient.Query(sRMANo, sCustomerName, sModelNo, sSerial, sRepair, sStatus, fdate, edate, RepairCenter, "RMAD_RMANO desc")

        Call ArrangementData(dtRequest)
        Session("_dtRequest") = dtRequest
        Dim dvRequest As DataView = dtRequest.DefaultView
        Me.ViewState("_SortExpression") = "RMAD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvRequest.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Request_DataBind(dvRequest, iPageIndex)

    End Sub

    Private Sub Request_DataBind(ByVal dvRequest As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRequest
        Me.UI_dvRequest.DataBind()

        Call setFlowCase01_UI_dvRequest()
    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export

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

            '202105004 轉換model
            Dim sModelNo As String = oExport.getMModelNo(dtRequest.Rows(i)("RMAD_MODELNO").ToString().Trim(), dtRequest.Rows(i)("RMA_COMPNO").ToString().Trim(), dtRequest.Rows(i)("RMA_ACCOUNTID").ToString().Trim())
            dtRequest.Rows(i)("RMAD_MODELNO") = sModelNo

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
                sQuoted = dr.RMAR_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMAR_QUOTE.ToString()).ToString("N")
            ElseIf dr.IsRMARQ_QUOTENull = False Then
                sQuoted = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARQ_QUOTE.ToString()).ToString("N")
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
        Next

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0



        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvRequest.PageIndex * Me.UI_dvRequest.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_imgEdit As ImageButton = e.Row.FindControl("UI_imgEdit")

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(3).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(3).Text) < Convert.ToDateTime(e.Row.Cells(4).Text) Then
                    e.Row.Cells(3).ForeColor = Drawing.Color.Red
                End If
            End If

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            Dim UI_RECEVSTATUS As Label = e.Row.FindControl("UI_RECEVSTATUS")
            Dim UI_RMADNO As LinkButton = e.Row.FindControl("UI_RMADNO")
            Dim UI_RMADNOText As Label = e.Row.FindControl("UI_RMADNOText")
            Dim UI_RMADPARTSN As Label = e.Row.FindControl("UI_RMADPARTSN")
            Dim UI_RMAD_SERIALNO As Label = e.Row.FindControl("UI_RMAD_SERIALNO")

            If UI_RMADPARTSN.Text <> "" Then
                UI_RMAD_SERIALNO.Text = UI_RMADPARTSN.Text
            End If

            If Convert.ToInt16(UI_RMADSTATUS.Text.Trim()) < 20 Or UI_RECEVSTATUS.Text.Trim() = "2" Then
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
            Dim dtRequest As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            Dim dvRequest As DataView = dtRequest.DefaultView
            Call Request_DataBind(dvRequest, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.ToString().Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.ToString().Trim()
        End If

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

            Me.UI_lblPreviousPage_RMADID.Text = UI_RMADID.Text.ToString().Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.ToString().Trim()
        End If

    End Sub

    Protected Sub UI_dvRequest_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvRequest.Sorting

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

        If IsNothing(Session("_dtRequest")) = False Then
            Dim dtRMA_tmp As DataTable = Session("_dtRequest")
            Dim dvDetail As DataView = dtRMA_tmp.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Request_DataBind(dvDetail, 0)
        End If
    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01_UI_dvRequest()
        Dim i As Integer = 0

        If Me.UI_dvRequest.Rows.Count > 0 Then
            If Me.UI_flowCase.Text = "01" Then
                Me.UI_dvRequest.HeaderRow.Cells(7).Visible = False
                Me.UI_dvRequest.HeaderRow.Cells(8).Visible = False
            End If

            For i = 0 To Me.UI_dvRequest.Rows.Count - 1
                If Me.UI_flowCase.Text = "01" Then
                    Me.UI_dvRequest.Rows(i).Cells(7).Visible = False
                    Me.UI_dvRequest.Rows(i).Cells(8).Visible = False
                End If
            Next
        End If

    End Sub

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
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.ToString().Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.ToString().Trim()
        Me.ViewState("_Serial") = Me.UI_txtSerialNumber.Text.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()

        Me.ViewState("_fdate") = ""
        If Me.UI_cboBYear.SelectedValue <> -1 And Me.UI_cboBMonth.SelectedValue <> -1 And Me.UI_cboBDay.SelectedValue <> -1 Then
            Me.ViewState("_fdate") = Me.UI_cboBYear.SelectedValue & "/" & Me.UI_cboBMonth.SelectedValue & "/" & Me.UI_cboBDay.SelectedValue
        Else
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
        End If

        Me.ViewState("_edate") = ""
        If Me.UI_cboEYear.SelectedValue <> -1 And Me.UI_cboEMonth.SelectedValue <> -1 And Me.UI_cboEDay.SelectedValue <> -1 Then
            Me.ViewState("_edate") = Me.UI_cboEYear.SelectedValue & "/" & Me.UI_cboEMonth.SelectedValue & "/" & Me.UI_cboEDay.SelectedValue
        Else
            Me.ViewState("_edate") = Date.Now.ToShortDateString()
        End If

        Call QueryData(0)
    End Sub

End Class

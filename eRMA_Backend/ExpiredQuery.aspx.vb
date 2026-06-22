Imports System.Data
Imports DataService
Imports DefLanguage
Imports RMA_Common
Imports RMA_Model
Imports RMA_Model.Enums

Partial Class ExpiredQuery
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _Coms As New Commons
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim helper As New Utility.LanguageHelper()

    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

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

        helper.Apply(Me, Session("_LanguageID").ToString())
        helper.ApplyGridHeader(UI_dvRequest, "ExpiredQuery", "Request", Session("_LanguageID").ToString())
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
        Dim sExpireStatus As String = Me.ViewState("_ExpireStatus")

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
        dtRequest = oClient.ExpiredQuery(sRMANo, sCustomerName, sModelNo, sSerial, sRepair, fdate, edate, RepairCenter, sExpireStatus, "RMAD_RMANO desc")

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
            dtRequest.Columns.Add("gvQuoted")
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
            dtRequest.Rows(i)("gvQuoted") = sQuoted.Trim()

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
            'Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            'UI_SeqID.Text = (Me.UI_dvRequest.PageIndex * Me.UI_dvRequest.PageSize) + (e.Row.RowIndex + 1).ToString()

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
    Protected Sub UI_btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_btnSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.ToString().Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.ToString().Trim()
        Me.ViewState("_Serial") = Me.UI_txtSerialNumber.Text.ToString().Trim()
        Me.ViewState("_Repair") = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()
        Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()
        Me.ViewState("_ExpireStatus") = Me.UI_cboExpireStatus.SelectedValue

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

    ''' <summary>
    ''' 按下拒絕
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <Obsolete>
    Protected Sub UI_btnReject_Click(ByVal sender As Object, ByVal e As EventArgs) Handles UI_btnReject.Click
        Dim currentPage As Integer = UI_dvRequest.PageIndex
        Dim selectedNos As New List(Of String)

        For Each row As GridViewRow In UI_dvRequest.Rows
            Dim chk As CheckBox = CType(row.FindControl("UI_RMAcheck"), CheckBox)
            If chk IsNot Nothing AndAlso chk.Checked Then
                If row.RowIndex < UI_dvRequest.DataKeys.Count Then

                    Dim rmano As String = UI_dvRequest.DataKeys(row.RowIndex).Values("RMAD_ID").ToString()

                    selectedNos.Add(rmano)
                End If
            End If
        Next

        If selectedNos.Count = 0 Then
            _Coms.AlertMessage(Me, "請至少選擇一項！")
            Exit Sub
        End If

        Dim dt As DataTable = CType(Session("_dtRequest"), DataTable)
        Dim rejectList As List(Of DataRow) = dt.AsEnumerable().
                                                Where(Function(dr) selectedNos.Contains(dr("RMAD_ID").ToString())).
                                                ToList()

        Dim result = RejectOrder(rejectList)
        If Not result.IsSuccess Then
            _Coms.AlertMessage(Me, result.Message)
        Else
            SendMail(rejectList)
        End If

        UI_dvRequest.DataSource = dt
        If currentPage < UI_dvRequest.PageCount Then
            UI_dvRequest.PageIndex = currentPage
        End If
        UI_dvRequest.DataBind()
    End Sub

    <Obsolete>
    Private Function RejectOrder(rejectList As List(Of DataRow)) As Result
        Dim result As New Result
        Dim ctlRMA As New ctlRMA.Repair_Quoting
        Dim ctlClient As New ctlRMA.Client

        Try
            Dim inClause As String = String.Join(",", rejectList.Select(Function(x) "'" & x("RMAD_RMANO").ToString() & "'"))
            Dim rejectData = ctlClient.QueryRejectListData(Session("_LanguageID").ToString(), inClause)

            Dim RMADetailReqList As New List(Of RMADetailReq)
            Dim RepairQuotedReqList As New List(Of RMARepairQuotedReq)
            For Each dr In rejectList
                Dim RMADetailReq As New RMADetailReq()

                Dim sRMADID = dr("RMAD_ID").ToString()
                Dim sServiceCharge = rejectData.AsEnumerable().Where(Function(x) x("RMAD_ID").ToString() = sRMADID).Select(Function(r) r("RMASQ_LABORCOST").ToString()).First()

                RMADetailReq.RMAD_ID = sRMADID
                RMADetailReq.RMAD_STATUS = RMADstatus.Canceled
                RMADetailReq.RMAD_LUAD = dr("CU_NO").ToString()
                RMADetailReq.RMAD_LUADNAME = dr("CU_NO").ToString()
                RMADetailReq.RMAD_LUSTMP = Date.Now
                RMADetailReq.RMAD_REJAD = Session("_UserID")
                RMADetailReq.RMAD_REJADNAME = Session("_UserName")
                RMADetailReq.RMAD_REJSTMP = Date.Now

                RMADetailReq.RepairQuoted = New List(Of RMARepairQuotedReq) From {
                    New RMARepairQuotedReq With {
                        .RMARQ_RMADID = sRMADID,
                        .RMARQ_ID = rejectData.AsEnumerable().Where(Function(x) x("RMAD_ID").ToString() = sRMADID).Select(Function(r) r("RMARQ_ID").ToString()).First(),
                        .RMARQ_ACCEPT = 2,
                        .RMARQ_QUOTE = If(String.IsNullOrWhiteSpace(sServiceCharge), 0, Convert.ToDouble(sServiceCharge))
                    }
                }

                RMADetailReq.SaleQuoted = New List(Of RMASaleQuotedReq) From {
                    New RMASaleQuotedReq With {
                        .RMASQ_QUOTE = If(String.IsNullOrWhiteSpace(sServiceCharge), 0, Convert.ToDouble(sServiceCharge))
                    }
                }

                RMADetailReqList.Add(RMADetailReq)
            Next

            If rejectList.Count > 0 Then
                result = ctlRMA.UpdateRMAQuotedRejectData(RMADetailReqList)
            End If

        Catch ex As Exception
            result.IsSuccess = False
            result.Message = ex.Message
        End Try

        Return result
    End Function

    Public Function SendMail(rejectList As List(Of DataRow)) As Result
        Dim result As New Result
        Dim sMsg As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try
            ' 使用 LINQ 將資料依 RMA 編號分組
            Dim grouped = rejectList.GroupBy(Function(r) r("RMAD_RMANO").ToString()) _
                                .ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            ' 每個 RMA 只發送一次信件
            For Each kvp In grouped
                Dim sRMA As String = kvp.Key
                Dim items As List(Of DataRow) = kvp.Value

                ' 取得第一筆資料來抓客戶資訊、業務、助理
                Dim drFirst As DataRow = items.First()
                Dim item_Accept As String = ""
                Dim item_Reject As String = String.Join(",", items.Select(Function(x) x("RMAD_SERIALNO").ToString()))

                Dim sCU_ID As String = drFirst("CU_NO").ToString()
                Dim sUserID As String = drFirst("RMA_ACCOUNTID").ToString()

                ' 取得客戶資訊
                dtCustomer = oCustomer.QueryUser(sCU_ID, sUserID, "")
                Dim CU_NAME As String = ""
                Dim CU_SALESID As String = ""
                Dim CU_ASSISTANTID As String = ""

                If dtCustomer.Rows.Count > 0 Then
                    CU_NAME = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                    CU_SALESID = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                    CU_ASSISTANTID = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()
                Else
                    Dim ctlRMA As New ctlRMA
                    Dim dtSales As New RmaDTO.VWSALESBYRMADataTable
                    dtSales = ctlRMA.getSalesMail_ARNO(sRMA)
                    CU_SALESID = dtSales.Rows(0)("SalesID").ToString().Trim()
                    CU_ASSISTANTID = dtSales.Rows(0)("AssistantID").ToString().Trim()
                End If

                ' 取得業務與助理郵件
                Dim MailSales As String = "", SalesName As String = ""
                If Not String.IsNullOrEmpty(CU_SALESID) Then
                    dtAdmin = oAdmin.Query(CU_SALESID, "")
                    MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()
                    SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()
                End If

                Dim MailAssistant As String = "", AssistantName As String = ""
                If Not String.IsNullOrEmpty(CU_ASSISTANTID) Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()
                    AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()
                End If

                ' 取得語言設定
                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", sUserID)

                Dim sEmailURL As String = _oLanguage.getMailText("Mail", "005", ctlLanguage.eumType.Mail, LanguageID)

                ' 產生附件
                Dim ctAddress_ As New ctAddress
                Dim dt As DataTable = ctAddress_.Select_RMA(sRMA)
                Client_FlowCase01_getRequestForm(dt)
                Dim Client_FlowCase01_oAttachmentFile As New Collection
                If Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01").ToString().Trim() <> "" Then
                    Client_FlowCase01_oAttachmentFile.Add(Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01").ToString())
                End If

                ' 發送 Reject 郵件
                If Not String.IsNullOrEmpty(MailSales) Or Not String.IsNullOrEmpty(MailAssistant) Then
                    Dim sSubject_Reject As String = _oLanguage.getMailText("Mail", "035", ctlLanguage.eumType.Mail, LanguageID)
                    Dim sBody_Reject As String = _oLanguage.getMailText("Mail", "036", ctlLanguage.eumType.Mail, LanguageID)
                    Dim sMailTo_Reject As String = ""
                    Dim sRepairUName_Reject As String = ""

                    sSubject_Reject = sSubject_Reject.Replace("[$RMA No$]", sRMA & " SN: " & item_Reject)
                    sSubject_Reject = sSubject_Reject.Replace("[$Customer's Name$]", CU_NAME.Trim())
                    sSubject_Reject = sSubject_Reject.Replace("[$Customer User Name$]", CU_NAME.Trim())

                    If Not String.IsNullOrEmpty(MailSales) Then
                        sRepairUName_Reject = SalesName
                        sMailTo_Reject = MailSales
                    End If
                    If Not String.IsNullOrEmpty(MailAssistant) Then
                        sRepairUName_Reject = If(sRepairUName_Reject = "", "", sRepairUName_Reject & "/") & AssistantName
                        If Not String.IsNullOrEmpty(sMailTo_Reject) Then sMailTo_Reject &= ","
                        sMailTo_Reject &= MailAssistant
                    End If

                    sBody_Reject = sBody_Reject.Replace("[$RMA No$]", sRMA & " SN: " & item_Reject)
                    sBody_Reject = sBody_Reject.Replace("[$Repair User Name$]", sRepairUName_Reject)
                    sBody_Reject = sBody_Reject.Replace("[$RMA Request No$]", sRMA)
                    sBody_Reject = sBody_Reject.Replace("[$Email URL$]", sEmailURL)
                    sBody_Reject = sBody_Reject.Replace("[$RMA REMARK$]", dt.Rows(0)("RMA_REMARK").ToString().Trim())

                    If _isDebug Then
                        sMailTo_Reject = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If

                    result = oMail.SendMailReBool(sSubject_Reject, sBody_Reject, sMailTo_Reject, _MailCC, Client_FlowCase01_oAttachmentFile)
                End If

                ' 發送 Repair 郵件
                Dim sSubject_Repair As String = _oLanguage.getMailText("Mail", "025", ctlLanguage.eumType.Mail, LanguageID)
                sSubject_Repair = sSubject_Repair.Replace("[$RMA No$]", sRMA)
                sSubject_Repair = sSubject_Repair.Replace("[$Customer's Name$]", CU_NAME.Trim())

                Dim oRMA As New ctlRMA
                Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(sRMA)
                Dim Repaire_EMAIL_List As String = String.Join(",", arrRepaire.Cast(Of String()).Select(Function(a) a(1).Trim()))

                Dim sBody_Repair As String = _oLanguage.getMailText("Mail", "026", ctlLanguage.eumType.Mail, LanguageID)
                sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "RMA Repairers")
                If Not String.IsNullOrEmpty(Repaire_EMAIL_List) Then
                    sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
                    sBody_Repair = sBody_Repair.Replace("[$RMA No$]", sRMA)
                    sBody_Repair = sBody_Repair.Replace("[$item_Accept$]", item_Accept)
                    sBody_Repair = sBody_Repair.Replace("[$item_Reject$]", item_Reject)
                    sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                    If _isDebug Then
                        Repaire_EMAIL_List = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If

                    result = oMail.SendMailReBool(sSubject_Repair, sBody_Repair, Repaire_EMAIL_List, _MailCC)
                End If
            Next

        Catch ex As Exception
            result.IsSuccess = False
            sMsg = result.Message

        Finally
            If Not result.IsSuccess Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", String.Format("alert('{0}');", sMsg), True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Reject 完成');", True)
            End If
        End Try

        Return result
    End Function

    Private Sub Client_FlowCase01_getRequestForm(dt As DataTable)
        Dim sRMAID As String = dt.Rows(0)("RMA_ID")
        Dim EndUser As Boolean = If(Not dt.Rows(0)("RMA_EUADDRESS") Is Nothing, True, False)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)


        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)

        EndUser = oRMARequest.chkISCWarrantyFee(sRMAID)

        'dtRequest = oRMARequest.getReport(Session("_LanguageID").ToString().Trim(), sRMAID)

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
            Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow

            If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
            If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
            If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
            If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
            If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
            If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
            If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
            If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

            If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
            If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
            If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
            If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
            If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()


            '20240308 客戶編號 開始
            Dim myctAddress As New ctAddress
            Dim CUSTOMER_PRODUCT_NUMBER As String = myctAddress.Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(dr.RMA_NO.ToString(), dr.RMAD_SERIALNO.ToString())
            drReport.RMAD_PRODUCTDESC = CUSTOMER_PRODUCT_NUMBER
            '20240308 客戶編號 結束

            '20221205 wisely add 若為Enduser送修 
            'Dim oRequested As New ctlRMA.Requested
            'Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblAccountIDText.Text.Trim())
            If (EndUser AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                'drReport.RMA_EUMAIL = dt.Rows(0)("cu_email").ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If


            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next

        Dim ds As New DataSet
        ds.Tables.Add(dtReport)

        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"
        Dim Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser.rpt")
        Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01") = ConfigurationManager.AppSettings("Report_FilePath") & sReportToPDF
        _Coms.ExportSetup_New(Server_MapPath_rpt, Me.ViewState("Client_FlowCase01_Worklist_AttachmentFile_01"), ds)
        'Call Client_FlowCase01_Print(dtReport, EndUser, LanguageID)
    End Sub

End Class

Imports DataService
Imports DefLanguage
Imports Newtonsoft.Json

Partial Class Client_Client_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LogHelper.WriteLog("Page_Load")
        Try
            If Me.IsPostBack = False Then
                Session("_dtRMA") = Nothing

                Me.ViewState("_RMANo") = ""
                Me.ViewState("_ModelNo") = ""
                Me.ViewState("_Serial") = ""
                Me.ViewState("_Status") = "-1"
                Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
                Me.ViewState("_edate") = Date.Now.ToShortDateString()

                If IsNothing(Request.QueryString("RMANO")) = False Then
                    Dim RMANO As String = Request.QueryString("RMANO").Trim()
                    If RMANO <> "" Then
                        Me.ViewState("_RMANo") = _Crypto.Decrypt(RMANO, "")
                    End If
                End If


                Call setControls()
                Call QueryData(0)
            End If
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try

    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        LogHelper.WriteLog("setControls")
        Try
            Call oCommon.getItemStatus(UI_cboStatus)
            '取得Tag Text
            Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
            Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
            Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
            Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
            Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
            Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
            Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
            Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        LogHelper.WriteLog("QueryData")
        Try
            Dim oRMA As New ctlRMA.Client
            Dim dtRMA As New RmaDTO.tmpClientListDataTable

            Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
            Dim sModelNo As String = Me.ViewState("_ModelNo").ToString().Trim()
            Dim sSerialNo As String = Me.ViewState("_Serial").ToString().Trim()
            Dim Status As String = Me.ViewState("_Status").ToString().Trim()
            Dim fdate As String = Me.ViewState("_fdate")
            Dim edate As String = Me.ViewState("_edate")

            dtRMA = oRMA.QueryByClientLst(Session("_CustomerID").ToString(), Session("_UserID").ToString(), sRMANo, sModelNo, sSerialNo, Status, fdate, edate)

            Call RMA_DataBind(dtRMA, iPageIndex)
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Private Sub RMA_DataBind(ByVal dtRMA As RmaDTO.tmpClientListDataTable, ByVal iPageIndex As Integer)
        LogHelper.WriteLog("RMA_DataBind")
        Try
            Call ArrangementData(dtRMA)
            Session("_dtRMA") = dtRMA

            Dim myDecimal As Decimal = dtRMA.Rows.Count / _PageSize
            Me.BookMark_Label.Text = "0"
            If dtRMA.Rows.Count = 0 Then
                Current_Page_Label.Text = "0"
            Else
                Current_Page_Label.Text = "1"
            End If
            Total_Page_Label.Text = Math.Ceiling(myDecimal).ToString()

            Me.UI_dvRequest.PageSize = _PageSize
            Me.UI_dvRequest.PageIndex = iPageIndex
            Me.UI_dvRequest.DataSource = dtRMA.DefaultView
            Me.UI_dvRequest.DataBind()
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Private Sub ArrangementData(ByVal dtRMA As RmaDTO.tmpClientListDataTable)
        LogHelper.WriteLog("ArrangementData")
        Try
            Dim i As Integer = 0

            'If dtRMA.Columns("SeqID") Is Nothing Then
            '    dtRMA.Columns.Add("SeqID")
            dtRMA.Columns.Add("Status")
            dtRMA.Columns.Add("WARRANTY")
            dtRMA.Columns.Add("Amount")
            'End If

            For i = 0 To dtRMA.Rows.Count - 1
                Dim dr As RmaDTO.tmpClientListRow = dtRMA.Rows(i)

                'dtRMA.Rows(i)("SeqID") = i + 1
                dtRMA.Rows(i)("Status") = oCommon.ConvertToItemStatusText(Convert.ToInt16(dr.RMAD_STATUS), dr.RMAD_ID.Trim())

                If dtRMA.Rows(i)("RMAD_WARRANTY").ToString().Trim() = "" Then
                    dtRMA.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                    'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                    Select Case dtRMA.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                        Case "0"
                            dtRMA.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                        Case "1"
                            dtRMA.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                    End Select

                Else
                    dtRMA.Rows(i)("WARRANTY") = Convert.ToDateTime(dr.RMAD_WARRANTY).ToShortDateString()
                End If

                'If dr.IsRMARSD_QUOTENull = False Then
                '    dtRMA.Rows(i)("Amount") = dr.RMARSD_QUOTE.ToString("N") 'RMA_SHIPMENTDETAIL.RMARSD_QUOTE
                'Else
                '    If dr.IsRMASQ_QUOTENull = False Then dtRMA.Rows(i)("Amount") = dr.RMASQ_QUOTE.ToString("N") 'RMASALE_QUOTED.RMASQ_QUOTE
                'End If
                If dr.IsRMASQ_QUOTENull = False Then dtRMA.Rows(i)("Amount") = dr.RMASQ_QUOTE.ToString("N") 'RMASALE_QUOTED.RMASQ_QUOTE
            Next

        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        LogHelper.WriteLog("UI_dvRequest_RowDataBound")
        Try
            Dim i As Integer = 0
            If e.Row.RowType = DataControlRowType.Header Then
                '修改Client端上方Search會出現錯誤bug by buck modify 20250923 begin
                'e.Row.Cells(1).Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
                'e.Row.Cells(2).Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag)
                'e.Row.Cells(3).Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag)
                'e.Row.Cells(4).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
                'e.Row.Cells(5).Text = _oLanguage.getText("RMA2", "104", ctlLanguage.eumType.Tag)
                'e.Row.Cells(6).Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
                'e.Row.Cells(7).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
                'e.Row.Cells(8).Text = _oLanguage.getText("RMA", "089", ctlLanguage.eumType.Tag)
                'e.Row.Cells(9).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

                e.Row.Cells(1).Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)  'RMA 單號
                e.Row.Cells(2).Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag) '產品序號
                e.Row.Cells(3).Text = _oLanguage.getText("RMA2", "017", ctlLanguage.eumType.Tag) '產品編號
                e.Row.Cells(4).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)  '型號
                e.Row.Cells(5).Text = _oLanguage.getText("RMA2", "104", ctlLanguage.eumType.Tag) '保固日期
                e.Row.Cells(6).Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)  '需求建立日期
                e.Row.Cells(7).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)  '業務報價總和
                e.Row.Cells(8).Text = _oLanguage.getText("RMA", "089", ctlLanguage.eumType.Tag)  '狀態
                e.Row.Cells(9).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)  '內容
                '修改Client端上方Search會出現錯誤bug by buck modify 20250923 end
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
                Dim UI_RMASTATUS As Label = e.Row.FindControl("UI_RMASTATUS")
                Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
                Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")

                Dim UI_cmdDetail As Button = e.Row.FindControl("UI_cmdDetail")
                Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")

                If UI_RMADWARRANTY.Text.Trim() <> "" Then
                    Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                    Dim RMAD_CSTMP As DateTime = Convert.ToDateTime(UI_RMADCSTMP.Text.Trim())
                    If RMAD_WARRANTY < RMAD_CSTMP Then
                        e.Row.Cells(5).ForeColor = Drawing.Color.Red
                    End If
                End If


                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                If Convert.ToInt16(UI_RMASTATUS.Text) > 10 Then
                    UI_cmdEdit.Visible = False
                    UI_cmdDetail.Visible = True
                Else
                    UI_cmdEdit.Visible = True
                    UI_cmdDetail.Visible = False
                End If

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                'If Convert.ToInt16(UI_RMADSTATUS.Text) > 10 Then
                '    UI_cmdEdit.Visible = False
                '    UI_cmdDetail.Visible = True
                'Else
                '    UI_cmdEdit.Visible = True
                '    UI_cmdDetail.Visible = False
                'End If

                UI_cmdEdit.Text = _oLanguage.getText("RMA2", "043", ctlLanguage.eumType.Tag)
                UI_cmdDetail.Text = _oLanguage.getText("RMA2", "043", ctlLanguage.eumType.Tag)
            End If

            '修改Client端上方Search會出現錯誤bug by buck modify 20250923 begin
            '語系轉換 開始
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim statusText As String = e.Row.Cells(8).Text.Trim()
                Dim uiRECEVSTATUS As Label = e.Row.FindControl("UI_RECEVSTATUS")

                Dim statusMap As New Dictionary(Of String, String)
                statusMap.Add("10", _oLanguage.getText("RMA2", "072", ctlLanguage.eumType.Tag)) 'Requested
                statusMap.Add("20", _oLanguage.getText("RMA2", "073", ctlLanguage.eumType.Tag)) 'Received
                statusMap.Add("30", _oLanguage.getText("RMA2", "074", ctlLanguage.eumType.Tag)) 'Repair Quoted
                statusMap.Add("35", _oLanguage.getText("RMA2", "075", ctlLanguage.eumType.Tag)) 'Sales Quoting
                statusMap.Add("40", _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)) 'Sales Confirmed
                statusMap.Add("50", _oLanguage.getText("RMA2", "099", ctlLanguage.eumType.Tag)) 'Client Confirmed
                statusMap.Add("60", _oLanguage.getText("RMA2", "078", ctlLanguage.eumType.Tag)) 'Repaired
                statusMap.Add("70", _oLanguage.getText("RMA2", "079", ctlLanguage.eumType.Tag)) 'Shipped
                statusMap.Add("90", _oLanguage.getText("RMA2", "080", ctlLanguage.eumType.Tag)) 'Closed
                statusMap.Add("91", If(uiRECEVSTATUS.Text = "1",
                                    _oLanguage.getText("RMA2", "081", ctlLanguage.eumType.Tag),
                                    _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command))) 'Canceled OR Deleted

                If statusMap.ContainsKey(statusText) Then
                    e.Row.Cells(8).Text = statusMap(statusText)
                End If

                e.Row.Visible = If(statusText = "91" And uiRECEVSTATUS.Text = "2", False, True) '刪除狀態既不顯示
            End If
            'If e.Row.RowType = DataControlRowType.DataRow Then

            '    If e.Row.Cells(7).Text = "Requested" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "072", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Received" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "073", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Repair Quoted" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "074", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Sales Quoted" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "075", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Sales Confirmed" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Client Confirmed" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "099", ctlLanguage.eumType.Tag)
            '    End If


            '    If e.Row.Cells(7).Text = "Repaired" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "078", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Customer Confirmed" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "077", ctlLanguage.eumType.Tag)
            '    End If


            '    If e.Row.Cells(7).Text = "Shipped" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "079", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Closed" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "080", ctlLanguage.eumType.Tag)
            '    End If

            '    If e.Row.Cells(7).Text = "Canceled" Then
            '        e.Row.Cells(7).Text = _oLanguage.getText("RMA2", "081", ctlLanguage.eumType.Tag)


            '        If e.Row.Cells(7).Text = "Deleted" Then
            '            e.Row.Cells(7).Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
            '            e.Row.Visible = False
            '        End If

            '        If e.Row.Cells(7).Text = "Delete" Then

            '            e.Row.Visible = False
            '        End If
            '    Else


            '    End If

            'End If
            '語系轉換 結束

            'If e.Row.RowType = DataControlRowType.Pager Then
            '    Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            '    For i = 0 To iLoop - 1
            '        If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
            '            'Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
            '            'oLabel.ForeColor = Drawing.Color.Red
            '            'oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
            '        End If
            '    Next
            'End If

            'If e.Row.RowType = DataControlRowType.DataRow Then

            '    If e.Row.Cells(8).Text = "Deleted" Then
            '        e.Row.Cells(8).Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)

            '    End If

            '    Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            '    If UI_RMADSTATUS.Text = "40" Then
            '        e.Row.Cells(8).Text = _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)
            '    End If

            '    If UI_RMADSTATUS.Text = "30" Then
            '        e.Row.Cells(8).Text = _oLanguage.getText("RMA2", "074", ctlLanguage.eumType.Tag)
            '    End If

            'End If
            '修改Client端上方Search會出現錯誤bug by buck modify 20250923 end

        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        LogHelper.WriteLog("UI_dvRequest_PageIndexChanging")
        Try
            Dim iPageIndex As Integer = e.NewPageIndex.ToString()

            If Not Session("_dtRMA") Is Nothing Then
                Dim dtRMA As RmaDTO.tmpClientListDataTable = Session("_dtRMA")
                Call RMA_DataBind(dtRMA, iPageIndex)

            Else
                Call QueryData(iPageIndex)
            End If
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        LogHelper.WriteLog("UI_dvRequest_RowCommand")
        Try

            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            If e.CommandName = "cmdEdit" Then
                row = Me.UI_dvRequest.Rows(iIndex)

                Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
                Dim UI_RMANO As Label = row.FindControl("UI_RMANO")

                UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
                UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            End If

            If e.CommandName = "cmdDetail" Then
                row = Me.UI_dvRequest.Rows(iIndex)
                Dim UI_RMAID As Label = row.FindControl("UI_RMAID")
                Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
                Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
                Dim UI_RMAD_STATUS As Label = row.FindControl("UI_RMADSTATUS")

                Dim Client_FlowCase01_Worklist_Item_New_Json_D As New Client_FlowCase01_Worklist_Item_New_Json()
                Client_FlowCase01_Worklist_Item_New_Json_D.RMANO = UI_RMANO.Text.Trim()
                Client_FlowCase01_Worklist_Item_New_Json_D.RMADID = UI_RMADID.Text.Trim()
                Client_FlowCase01_Worklist_Item_New_Json_D.RMA_ID = UI_RMAID.Text.Trim()
                Client_FlowCase01_Worklist_Item_New_Json_D.RMAD_STATUS = UI_RMAD_STATUS.Text.Trim()
                Dim Obj As String = JsonConvert.SerializeObject(Client_FlowCase01_Worklist_Item_New_Json_D)
                Obj = HttpUtility.UrlEncode(Obj)

                Dim theHeight As Integer = 0
                If (Request.Cookies("windowhigh") IsNot Nothing) Then

                    theHeight = Convert.ToInt32(Request.Cookies("windowhigh").Value)
                End If

                If theHeight >= 768 Then
                    theHeight = 768

                Else
                    theHeight = theHeight - 25
                End If

                Dim theWidth As Integer = 0
                If (Request.Cookies("windowWidth") IsNot Nothing) Then

                    theWidth = Convert.ToInt32(Request.Cookies("windowWidth").Value)
                End If

                If theWidth >= 1300 Then
                    theWidth = 1300

                Else
                    theWidth = theWidth - 25
                End If


                'Me.windowLab.Text = " <iframe class=''  src='Client_FlowCase01_Worklist_Item_New.aspx?DATA=" & Obj & "'  style='width:" & theWidth  & "px;height:" & theHeight  & "px;border:none;border-radius:10px;' ></iframe>  "
                'Me.UI_Up_RMA_panel_ModalPopupExtender.Show()
                row = Me.UI_dvRequest.Rows(iIndex)


                Me.ucClientDetail.show(UI_RMADID.Text.Trim(), UI_RMANO.Text.Trim(), True)
            End If

        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

#Region "Class 顯示詳細資料"
    Public Class Client_FlowCase01_Worklist_Item_New_Json

        Private sRMANO As String
        Public Property RMANO() As String
            Get
                Return sRMANO
            End Get
            Set(ByVal value As String)
                sRMANO = value
            End Set
        End Property

        Private sRMADID As String
        Public Property RMADID() As String
            Get
                Return sRMADID
            End Get
            Set(ByVal value As String)
                sRMADID = value
            End Set
        End Property

        Private sRMA_ID As String
        Public Property RMA_ID() As String
            Get
                Return sRMA_ID
            End Get
            Set(ByVal value As String)
                sRMA_ID = value
            End Set
        End Property

        Private sRMAD_STATUS As String
        Public Property RMAD_STATUS() As String
            Get
                Return sRMAD_STATUS
            End Get
            Set(ByVal value As String)
                sRMAD_STATUS = value
            End Set
        End Property

    End Class
#End Region

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        LogHelper.WriteLog("UI_dvRequest_RowCommand")
        'RMA 前端日曆選項故障 by buck modify 20250925 begin
        Me.txtStart.Text = hfStartDate.Value
        Me.txtEnd.Text = hfEndDate.Value
        'RMA 前端日曆選項故障 by buck modify 20250925 end
        Try

            Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()
            Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.ToString().Trim()
            Me.ViewState("_Serial") = Me.UI_txtSerialNumber.Text.ToString().Trim()
            Me.ViewState("_Status") = Me.UI_cboStatus.SelectedValue.ToString().Trim()



            Me.ViewState("_fdate") = ""
            If Me.txtStart.Text <> "" Then
                Me.ViewState("_fdate") = Me.txtStart.Text.Trim()
            Else
                Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            End If

            Me.ViewState("_edate") = ""
            If Me.txtEnd.Text <> "" Then
                Me.ViewState("_edate") = Me.txtEnd.Text.Trim()
            Else
                Me.ViewState("_edate") = Date.Now.ToShortDateString()
            End If


            Call QueryData(0)

            If Not Session("_dtRMA") Is Nothing Then

                Dim myDataTable As New RmaDTO.tmpClientListDataTable
                myDataTable = Session("_dtRMA")
                Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize

                BookMark_Label.Text = "0"

                If myDataTable.Rows.Count = 0 Then
                    Current_Page_Label.Text = "0"
                Else
                    Current_Page_Label.Text = "1"
                End If


                Total_Page_Label.Text = Math.Ceiling(myDecimal).ToString()
            End If

        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

#Region "頁"
    Protected Sub first_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles first_ImageBtn.Click
        Dim myDataTable As New RmaDTO.tmpClientListDataTable
        myDataTable = Session("_dtRMA")
        Me.UI_dvRequest.PageSize = _PageSize
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        Me.UI_dvRequest.PageIndex = 0
        Me.UI_dvRequest.DataSource = myDataTable
        Me.UI_dvRequest.DataBind()
        BookMark_Label.Text = "0"
        Current_Page_Label.Text = "1"
    End Sub
    Protected Sub previous_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles previous_ImageBtn.Click
        Dim myDataTable As New RmaDTO.tmpClientListDataTable
        myDataTable = Session("_dtRMA")
        Me.UI_dvRequest.PageSize = _PageSize
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        If Convert.ToInt32(BookMark_Label.Text.Trim()) > 0 Then
            Me.UI_dvRequest.PageIndex = Convert.ToInt32(BookMark_Label.Text.Trim()) - 1
            BookMark_Label.Text = (Convert.ToInt32(BookMark_Label.Text.Trim()) - 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
        End If
        Me.UI_dvRequest.DataSource = myDataTable
        Me.UI_dvRequest.DataBind()
    End Sub
    Protected Sub next_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles next_ImageBtn.Click
        Dim myDataTable As New RmaDTO.tmpClientListDataTable
        myDataTable = Session("_dtRMA")
        Me.UI_dvRequest.PageSize = _PageSize
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        If Convert.ToInt32(BookMark_Label.Text.Trim()) < Math.Ceiling(myDecimal) - 1 Then
            Me.UI_dvRequest.PageIndex = Convert.ToInt32(BookMark_Label.Text.Trim()) + 1
            BookMark_Label.Text = (Convert.ToInt32(BookMark_Label.Text.Trim()) + 1).ToString()
            Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) + 1).ToString()
        End If
        Me.UI_dvRequest.DataSource = myDataTable
        Me.UI_dvRequest.DataBind()
    End Sub
    Protected Sub last_ImageBtn_Click(sender As Object, e As ImageClickEventArgs) Handles last_ImageBtn.Click
        Dim myDataTable As New RmaDTO.tmpClientListDataTable
        myDataTable = Session("_dtRMA")
        Me.UI_dvRequest.PageSize = _PageSize
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize
        Me.UI_dvRequest.PageIndex = Math.Ceiling(myDecimal)
        Me.UI_dvRequest.DataSource = myDataTable
        Me.UI_dvRequest.DataBind()
        BookMark_Label.Text = Me.UI_dvRequest.PageIndex.ToString()
        Current_Page_Label.Text = (Me.UI_dvRequest.PageIndex + 1).ToString()
    End Sub
    Protected Sub Current_Page_Label_TextChanged(sender As Object, e As EventArgs) Handles Current_Page_Label.TextChanged

        Dim myDataTable As New RmaDTO.tmpClientListDataTable
        myDataTable = Session("_dtRMA")
        Me.UI_dvRequest.PageSize = _PageSize
        Dim myDecimal As Decimal = myDataTable.Rows.Count / _PageSize

        If IsNumeric(Current_Page_Label.Text.Trim()) Then

            If Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1 <= Math.Ceiling(myDecimal) - 1 And Convert.ToInt32(Current_Page_Label.Text.Trim()) > 0 Then
                Me.UI_dvRequest.PageIndex = Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1
                BookMark_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim()) - 1).ToString()
                Current_Page_Label.Text = (Convert.ToInt32(Current_Page_Label.Text.Trim())).ToString()

                Me.UI_dvRequest.DataSource = myDataTable
                Me.UI_dvRequest.DataBind()

            Else
                Current_Page_Label.Text = ""

            End If

        Else
            Current_Page_Label.Text = ""

        End If


    End Sub
#End Region

End Class

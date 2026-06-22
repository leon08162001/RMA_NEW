Imports DataService
Imports DefLanguage

Partial Class Client_Client_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
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
    End Sub

    Private Sub RMA_DataBind(ByVal dtRMA As RmaDTO.tmpClientListDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtRMA)
        Session("_dtRMA") = dtRMA

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dtRMA.DefaultView
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRMA As RmaDTO.tmpClientListDataTable)
        Dim i As Integer = 0

        If dtRMA.Columns("SeqID") Is Nothing Then
            dtRMA.Columns.Add("SeqID")
            dtRMA.Columns.Add("Status")
            dtRMA.Columns.Add("WARRANTY")
            dtRMA.Columns.Add("Amount")
        End If

        For i = 0 To dtRMA.Rows.Count - 1
            Dim dr As RmaDTO.tmpClientListRow = dtRMA.Rows(i)

            dtRMA.Rows(i)("SeqID") = i + 1
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

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "037", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
            Dim UI_RMASTATUS As Label = e.Row.FindControl("UI_RMASTATUS")
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")

            Dim UI_cmdDetail As ImageButton = e.Row.FindControl("UI_cmdDetail")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")

            If UI_RMADWARRANTY.Text.Trim() <> "" Then
                Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                Dim RMAD_CSTMP As DateTime = Convert.ToDateTime(UI_RMADCSTMP.Text.Trim())
                If RMAD_WARRANTY < RMAD_CSTMP Then
                    e.Row.Cells(3).ForeColor = Drawing.Color.Red
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

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRMA") Is Nothing Then
            Dim dtRMA As RmaDTO.tmpClientListDataTable = Session("_dtRMA")
            Call RMA_DataBind(dtRMA, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
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
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")

            Me.ucClientDetail.show(UI_RMADID.Text.Trim(), UI_RMANO.Text.Trim(), True)
        End If

    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click

        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.ToString().Trim()
        Me.ViewState("_ModelNo") = Me.UI_txtModelNo.Text.ToString().Trim()
        Me.ViewState("_Serial") = Me.UI_txtSerialNumber.Text.ToString().Trim()
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

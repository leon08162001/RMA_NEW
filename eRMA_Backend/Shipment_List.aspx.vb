Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Shipment_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim i As Integer = 0

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_RMANo") = ""
            Me.ViewState("_Notice") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Tracking") = ""
            Me.ViewState("_Serial") = ""
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtShipment") = Nothing

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
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "146", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "143", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblNotice.Text = _oLanguage.getText("RMA", "144", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblTracking.Text = _oLanguage.getText("RMA", "138", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblNoticeDate.Text = _oLanguage.getText("RMA", "139", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "142", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdShippingAdd.Text = _oLanguage.getText("Common", "043", ctlLanguage.eumType.Command)

        Me.UI_dvShipping.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(2).HeaderText = _oLanguage.getText("RMA", "145", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(3).HeaderText = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(4).HeaderText = _oLanguage.getText("RMA", "139", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(5).HeaderText = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(6).HeaderText = _oLanguage.getText("RMA", "141", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(7).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShipment As New RmaDTO.ShipmentDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString.Trim
        Dim sNotice As String = Me.ViewState("_Notice").ToString().Trim()
        Dim sCustomerName As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim sSerial As String = Me.ViewState("_Serial").ToString().Trim()
        Dim sTracking As String = Me.ViewState("_Tracking").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        'Session("_UserID")-->業務ID,業務助理ID
        dtShipment = oShipment.QueryByRMA_Shipping(sRMANo, "", Session("_UserID"), sNotice, sTracking, sCustomerName, sSerial, fdate, edate)

        Call ArrangementData(dtShipment)
        Session("_dtShipment") = dtShipment
        Dim dvShipment As DataView = dtShipment.DefaultView
        Me.ViewState("_SortExpression") = "RMASMD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvShipment.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Shipment_DataBind(dvShipment, iPageIndex)
    End Sub

    Private Sub Shipment_DataBind(ByVal dvShipment As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvShipping.PageSize = _PageSize
        Me.UI_dvShipping.PageIndex = iPageIndex
        Me.UI_dvShipping.DataSource = dvShipment
        Me.UI_dvShipping.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtShipment As RmaDTO.ShipmentDataTable)
        Dim i As Integer = 0
        dtShipment.Columns.Add("NoticeDate")
        dtShipment.Columns.Add("TrackingNo")
        dtShipment.Columns.Add("ShippedDate")

        For i = 0 To dtShipment.Rows.Count - 1
            Dim dr As RmaDTO.ShipmentRow = dtShipment.Rows(i)

            dtShipment.Rows(i)("NoticeDate") = Convert.ToDateTime(dr.RMASM_CSTMP.ToString()).ToShortDateString()

            'If dr.IsRMASH_SHIPPINGNONull = False Then dtShipment.Rows(i)("TrackingNo") = dr.RMASH_TRACKINGNO.ToString()
            'If dr.IsRMASH_CSTMPNull = False Then dtShipment.Rows(i)("ShippedDate") = Convert.ToDateTime(dr.RMASH_CSTMP.ToString()).ToShortDateString()
        Next

    End Sub

    Protected Sub UI_dvShipping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvShipping.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvShipping.PageIndex * Me.UI_dvShipping.PageSize) + (e.Row.RowIndex + 1).ToString()
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

    Protected Sub UI_dvShipping_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvShipping.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtShipment") Is Nothing Then
            Dim dtShipment As RmaDTO.ShipmentDataTable = Session("_dtShipment")
            Dim dvShipment As DataView = dtShipment.DefaultView
            Call Shipment_DataBind(dvShipment, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvShipping_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvShipping.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvShipping.Rows(iIndex)
            Dim UI_RMASMID As Label = row.FindControl("UI_RMASMID")

            Me.UI_lblPreviousPage_RMASMID.Text = UI_RMASMID.Text.ToString().Trim()
        End If

    End Sub

    Protected Sub UI_dvShipping_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvShipping.Sorting

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

        If IsNothing(Session("_dtShipment")) = False Then
            Dim dtRMA_tmp As DataTable = Session("_dtShipment")
            Dim dvDetail As DataView = dtRMA_tmp.DefaultView
            dvDetail.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Shipment_DataBind(dvDetail, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvShipping.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvShipping.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvShipping.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvShipping.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvShipping.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvShipping.Columns(i).HeaderText = sHeaderText
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
        Me.ViewState("_Notice") = Me.UI_txtNotice.Text.Trim()
        Me.ViewState("_CustomerName") = Me.UI_txtCustomer.Text.Trim()
        Me.ViewState("_Tracking") = Me.UI_txtTracking.Text.Trim()
        Me.ViewState("_Serial") = Me.UI_txtSerialNumber.Text.Trim()

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

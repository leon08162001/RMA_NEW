Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports DefLanguage
Imports ICAT_OracleDAO

Partial Class Shipping_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Me.IsPostBack = False Then
            Me.ViewState("_RMA") = ""
            Me.ViewState("_CustomerName") = ""
            Me.ViewState("_Tracking") = ""
            Me.ViewState("_Serial") = ""
            Me.ViewState("_fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("_edate") = Date.Now.ToShortDateString()

            Session("_dtShipping") = Nothing

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
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "137", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMA.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblTracking.Text = _oLanguage.getText("RMA", "138", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "142", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdShippingAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Me.UI_dvShipping.Columns(1).HeaderText = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(2).HeaderText = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(3).HeaderText = _oLanguage.getText("RMA", "139", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(4).HeaderText = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(5).HeaderText = _oLanguage.getText("RMA", "141", ctlLanguage.eumType.Tag)
        Me.UI_dvShipping.Columns(6).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oShipping As New ctlRMA.Shipping
        'Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        Dim dtShipping As New DataTable

        Dim sRMANO As String = Me.ViewState("_RMA").ToString().Trim()
        Dim sCustomerName As String = Me.ViewState("_CustomerName").ToString().Trim()
        Dim sSerial As String = Me.ViewState("_Serial").ToString().Trim()
        Dim sTracking As String = Me.ViewState("_Tracking").ToString().Trim()
        Dim fdate As String = Me.ViewState("_fdate")
        Dim edate As String = Me.ViewState("_edate")

        dtShipping = oShipping.QueryByRMA_ShippingList(Session("_RepairCenter").ToString().Trim(), "", sRMANO, sTracking, sCustomerName, sSerial, fdate, edate)

        'Response.Write("dtShipping=" & dtShipping.Rows.Count)
        'Response.End()
        'Call ArrangementData(dtShipping)

        Session("_dtShipping") = dtShipping
        Dim dvShipping As DataView = dtShipping.DefaultView
        Me.ViewState("_SortExpression") = "RMASHD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvShipping.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Shipping_DataBind(dvShipping, iPageIndex)
    End Sub

    Private Sub Shipping_DataBind(ByVal dvShipping As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvShipping.PageSize = _PageSize
        Me.UI_dvShipping.PageIndex = iPageIndex
        Me.UI_dvShipping.DataSource = dvShipping
        Me.UI_dvShipping.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtShipping As RmaDTO.RMA_ShippingDataTable)
        Dim i As Integer = 0

        'dtShipping.Columns.Add("NoticeDate")
        'dtShipping.Columns.Add("ShippedDate")
        'dtShipping.Columns.Add("TrackingNo")

        'For i = 0 To dtShipping.Rows.Count - 1
        '    Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.Rows(i)
        '    dtShipping.Rows(i)("NoticeDate") = Convert.ToDateTime(dr.RMASM_CSTMP.ToString()).ToShortDateString()
        '    dtShipping.Rows(i)("ShippedDate") = Convert.ToDateTime(dr.RMASH_CSTMP.ToString()).ToShortDateString()
        '    If dr.IsRMASH_TRACKINGNONull = False Then dtShipping.Rows(i)("TrackingNo") = dr.RMASH_TRACKINGNO.ToString()
        'Next


    End Sub

    Protected Sub UI_dvShipping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvShipping.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvShipping.PageIndex * Me.UI_dvShipping.PageSize) + (e.Row.RowIndex + 1).ToString()
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_isEdit As Label = e.Row.FindControl("UI_isEdit")
            Dim UI_imgEdit As ImageButton = e.Row.FindControl("UI_imgEdit")
            Dim UI_imgDetail As ImageButton = e.Row.FindControl("UI_imgDetail")

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdDetail As Button = e.Row.FindControl("UI_cmdDetail")

            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_cmdDetail.Text = _oLanguage.getText("Common", "049", ctlLanguage.eumType.Command)


            UI_cmdEdit.Visible = False
            UI_cmdDetail.Visible = False
            If UI_isEdit.Text.Trim() = "0" Then
                '清單頁
                UI_cmdEdit.Visible = False
                UI_cmdDetail.Visible = True
            End If

            If UI_isEdit.Text.Trim() = "1" Then
                '修改頁
                UI_cmdEdit.Visible = True
                UI_cmdDetail.Visible = False
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

    Protected Sub UI_dvShipping_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvShipping.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtShipping") Is Nothing Then
            'Dim dtShipping As RmaDTO.RMA_ShippingDataTable = Session("_dtShipping")
            Dim dtShipping As DataTable = Session("_dtShipping")
            Dim dvShipping As DataView = dtShipping.DefaultView
            Call Shipping_DataBind(dvShipping, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvShipping_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvShipping.RowCommand

        If e.CommandName = "cmdEdit" Or e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvShipping.Rows(iIndex)
            Dim UI_RMASHID As Label = row.FindControl("UI_RMASHID")

            Me.UI_lblPreviousPage_RMASHID.Text = UI_RMASHID.Text.ToString().Trim()
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

        If IsNothing(Session("_dtShipping")) = False Then
            Dim dtShipping As DataTable = Session("_dtShipping")
            Dim dvShipping As DataView = dtShipping.DefaultView
            dvShipping.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Shipping_DataBind(dvShipping, 0)
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
        Me.ViewState("_RMA") = Me.UI_txtRMA.Text.Trim()
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

    Public Function runloopproc(ByVal Inval As Integer) As String

        Dim retval As String = ""

        Dim oConn As New Connection
        'Dim oExecute As New Execute(oConn)
        'Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            ' oConn.BeginTransaction()

            '=============================================================================================================================================================================================================================================================
            'Call SP -->SP_SHP_INS_AR(傳入SHP單號,幣別,執行人員代號)
            '=============================================================================================================================================================================================================================================================
            Dim oCommand As OracleCommand = oConn.Command()
            'oCommand.CommandText = "SP_SHP_INS_AR"
            'oCommand.CommandType = System.Data.CommandType.StoredProcedure
            'oCommand.Parameters.Add("vCUSTNO", OracleType.NVarChar).Value = CustNo
            'oCommand.Parameters.Add("vSHPNO", OracleType.NVarChar).Value = RMASH_SHIPPINGNO
            'oCommand.Parameters.Add("vCurr", OracleType.NVarChar).Value = CurrencyCode
            'oCommand.Parameters.Add("vUserNo", OracleType.NVarChar).Value = sUserAD
            'oCommand.Parameters.Add("vCompno", OracleType.NVarChar).Value = sRepairCenter
            'oCommand.ExecuteNonQuery()

            oCommand.CommandText = "loopproc"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("inval", OracleType.Number).Value = Inval
            oCommand.Parameters("inval").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.VarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.Parameters("vResult").Size = 200

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

            'oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    Private Sub addLog(ByVal LogValue As String)
        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_LOG"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
            oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try
    End Sub

End Class

Imports DataService
Imports System.Data
Imports DefLanguage

Partial Class ChargeQuoted_List
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")



#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Session("_dtChargeQuoted_List") = Nothing

            Me.ViewState("_RMANo") = ""
            Me.ViewState("_Status") = "-1"
            Me.ViewState("fdate") = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Me.ViewState("edate") = Date.Now.ToShortDateString()

            Call clearFiled()
            Call setDefault()
            Call QueryData(0)
        End If
    End Sub
#End Region


    Private Sub clearFiled()
        Me.UI_lblPreviousPage_RMANO.Text = ""
        Me.UI_lblPreviousPage_RMAID.Text = ""

    End Sub




    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Call oCommon.getYear_DropDownList(Me.UI_cboBYear)
        Call oCommon.getYear_DropDownList(Me.UI_cboEYear, Date.Now.Year.ToString())
        Call oCommon.getMonth_DropDownList(Me.UI_cboBMonth)
        Call oCommon.getMonth_DropDownList(Me.UI_cboEMonth, Date.Now.Month.ToString())
        Call oCommon.getDay_DropDownList(Me.UI_cboBDay)
        Call oCommon.getDay_DropDownList(Me.UI_cboEDay, Date.Now.Day.ToString())

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "427", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblQuickTittle.Text = _oLanguage.getText("RMA", "171", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)


        Me.UI_dgList.Columns(1).HeaderText = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(2).HeaderText = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(3).HeaderText = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(4).HeaderText = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(5).HeaderText = _oLanguage.getText("RMA", "213", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(6).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dgList.Columns(7).HeaderText = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)

        Dim dtChargeQuoted As New DataTable
        dtChargeQuoted.Columns.Add("SeqNo")
        dtChargeQuoted.Columns.Add("RMA_ID")
        dtChargeQuoted.Columns.Add("RMA_NO")
        dtChargeQuoted.Columns.Add("RequestDate")
        dtChargeQuoted.Columns.Add("Applicant")
        dtChargeQuoted.Columns.Add("CurrencyCode")
        dtChargeQuoted.Columns.Add("QUOTE")
        dtChargeQuoted.Columns.Add("RMA_STATUS")
        dtChargeQuoted.Columns.Add("RMA_STATUSText")
        dtChargeQuoted.Columns.Add("QTY")
        Session("_dtChargeQuoted_List") = dtChargeQuoted
    End Sub




    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_RMANo") = Me.UI_txtRMANo.Text.Trim()
        Me.ViewState("_Status") = "-1"

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

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlCharge As New ctlChargeQuoted
        Dim dtChargeQuotedList As New ChargeQuotedDTO.vwChargeQuotedListDataTable

        Dim sRMANo As String = Me.ViewState("_RMANo").ToString().Trim()
        Dim sModelNo As String = ""
        Dim sSerialNo As String = ""

        Dim Status As String = Me.ViewState("_Status").ToString().Trim()
        Dim sCustomerName As String = ""
        Dim fdate As String = Me.ViewState("fdate")
        Dim edate As String = Me.ViewState("edate")

        'Session("_RepairCenter")-->維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)
        dtChargeQuotedList = ctlCharge.QueryByChargeQuotedList(Session("_RepairCenter").ToString().Trim(), Session("_UserID").ToString().Trim(), sRMANo, fdate, edate)


        '=======================================================================================================================================
        '無資料時, UI 的控制
        '=======================================================================================================================================
        Me.UI_lblQuickTittle.Visible = True
        If dtChargeQuotedList.Rows.Count = 0 Then
            Me.UI_lblQuickTittle.Visible = False
        End If

        Call ArrangementData(dtChargeQuotedList)

        Dim dtChargeQuoted As DataTable = Session("_dtChargeQuoted_List")
        Dim dvChargeQuoted As DataView = dtChargeQuoted.DefaultView

        Me.ViewState("_SortExpression") = "RMA_NO"
        Me.ViewState("_SortDirection") = "desc"
        dvChargeQuoted.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call RMA_DataBind(dvChargeQuoted, iPageIndex)
    End Sub



    Private Sub ArrangementData(ByVal dtChargeQuotedList As ChargeQuotedDTO.vwCHARGEQUOTEDLISTDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim oDataTable As DataTable = Session("_dtChargeQuoted_List")
        oDataTable.Rows.Clear()

        For i = 0 To dtChargeQuotedList.Rows.Count - 1
            Dim dr As ChargeQuotedDTO.vwCHARGEQUOTEDLISTRow = dtChargeQuotedList.Rows(i)

            'Dim RMACQ_DISCOUNT As String = dr.RMACQ_DISCOUNT.ToString().Trim()
            'Dim RMACQ_APPROVAL As String = dr.RMACQ_APPROVAL.ToString().Trim()

            Dim drTmp As DataRow = oDataTable.NewRow
            drTmp("SeqNo") = (i + 1).ToString()
            drTmp("RMA_ID") = dr.RMA_ID.ToString().Trim()
            drTmp("RMA_NO") = dr.RMA_NO.ToString().Trim()
            drTmp("RequestDate") = dr.RMA_CSTMP.ToShortDateString
            drTmp("Applicant") = dr.CU_NAME.ToString().Trim()


            If dr.IsRMACQ_CHARGEQUOTENull = False Then
                drTmp("CurrencyCode") = dr.RMACQ_CURRENCYCODE.Trim()
                drTmp("QUOTE") = Convert.ToDouble(dr.RMACQ_CHARGEQUOTE).ToString("N")

            Else
                If dr.IsRMASQ_QUOTENull = False Then
                    drTmp("CurrencyCode") = dr.RMASQ_CURRENCYCODE.Trim()
                    drTmp("QUOTE") = Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N")
                End If
            End If

            drTmp("RMA_STATUS") = dr.RMA_STATUS.ToString().Trim()
            drTmp("RMA_STATUSText") = oCommon.ConvertToStatusText(dr.RMA_STATUS.ToString().Trim())
            drTmp("QTY") = dr.QTY.ToString().Trim()

            oDataTable.Rows.Add(drTmp)
        Next

        Session("_dtChargeQuoted_List") = oDataTable
    End Sub

    Private Sub RMA_DataBind(ByVal dvChargeQuoted As DataView, ByVal iPageIndex As Integer)
        Dim i As Integer = 0

        For i = 0 To dvChargeQuoted.Count - 1
            dvChargeQuoted(i)("SeqNo") = (i + 1).ToString()
        Next

        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dgList.PageSize = _PageSize
        Me.UI_dgList.PageIndex = iPageIndex
        Me.UI_dgList.DataSource = dvChargeQuoted
        Me.UI_dgList.DataBind()

    End Sub


    Protected Sub UI_dgList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dgList.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
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

    Protected Sub UI_dgList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dgList.RowCommand
        Dim i As Integer = 0

        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dgList.Rows(iIndex)
            Dim UI_RMAID As Label = row.FindControl("UI_RMA_ID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMA_NO")

            Me.UI_lblPreviousPage_RMAID.Text = UI_RMAID.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()
            Server.Transfer("~/ChargeQuoted_Item.aspx")
        End If

    End Sub

    Protected Sub UI_dgList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dgList.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If IsNothing(Session("_dtChargeQuoted_List")) = False Then
            Dim dtChargeQuoted As DataTable = Session("_dtChargeQuoted_List")
            Dim dvChargeQuoted As DataView = dtChargeQuoted.DefaultView
            dvChargeQuoted.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvChargeQuoted, iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If

    End Sub


    Protected Sub UI_dgList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dgList.Sorting

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

        If IsNothing(Session("_dtChargeQuoted_List")) = False Then
            Dim dtChargeQuoted As DataTable = Session("_dtChargeQuoted_List")
            Dim dvChargeQuoted As DataView = dtChargeQuoted.DefaultView
            dvChargeQuoted.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call RMA_DataBind(dvChargeQuoted, 0)
        End If
    End Sub


    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dgList.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dgList.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dgList.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dgList.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dgList.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dgList.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

End Class

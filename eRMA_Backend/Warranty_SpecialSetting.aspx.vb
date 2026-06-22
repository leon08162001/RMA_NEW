Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Warranty_SpecialSetting
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SortExpression") = "Version"
            Me.ViewState("_SortDirection") = "desc"

            Me.ViewState("_OperationCenter") = ""
            Me.ViewState("_ProductGroup") = ""
            Me.ViewState("_WarrantyType") = ""
            Session("_dtTmp") = Nothing

            Dim oWarranty As New ctlWarranty
            Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
            dtProductGroup = oWarranty.QueryPrdGroup("", "")
            ViewState("_dtProductGroup") = dtProductGroup

            Call setControls()
            Call QueryData(0)
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()

        Call oCommon.getCostCenterByDropDownList(False, Me.UI_CboOperationCenter, "All")
        Call oCommon.getWarrantyTypeByDropDownList(False, Me.UI_CboWarrantyType, "All")

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "021", ctlLanguage.eumType.Tag)
        Me.UI_lblOperationCenter.Text = _oLanguage.getText("Warranty", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyType.Text = _oLanguage.getText("Warranty", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblPriceList.Text = _oLanguage.getText("Warranty", "061", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdProductGroupPick.Text = _oLanguage.getText("Warranty", "040", ctlLanguage.eumType.Tag)


        Me.UI_dvSales.Columns(1).HeaderText = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(2).HeaderText = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(3).HeaderText = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(4).HeaderText = _oLanguage.getText("Warranty", "029", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(5).HeaderText = _oLanguage.getText("Warranty", "030", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(6).HeaderText = _oLanguage.getText("Warranty", "031", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(7).HeaderText = _oLanguage.getText("Warranty", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(8).HeaderText = _oLanguage.getText("Warranty", "033", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(9).HeaderText = _oLanguage.getText("Warranty", "034", ctlLanguage.eumType.Tag)

        Dim dtTmp As New DataTable
        dtTmp.Columns.Add("SEQID")
        dtTmp.Columns.Add("sw_id")
        dtTmp.Columns.Add("OperationCenter")
        dtTmp.Columns.Add("ProductGroup")
        dtTmp.Columns.Add("Version")
        dtTmp.Columns.Add("WarrantyName")
        dtTmp.Columns.Add("WarrantyType")
        dtTmp.Columns.Add("ExtraMonths")
        dtTmp.Columns.Add("Years")
        dtTmp.Columns.Add("Price")
        dtTmp.Columns.Add("COMP_NAME")
        dtTmp.Columns.Add("Status")

        Session("_dtTmp") = dtTmp
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.SWSETDataTable

        Dim sOperationCenter As String = Me.ViewState("_OperationCenter").ToString().Trim()
        Dim sProductGroup As String = Me.ViewState("_ProductGroup").ToString().Trim()
        Dim sWarrantyType As String = Me.ViewState("_WarrantyType").ToString().Trim()
        Dim nWarrantyType As Integer = -1
        If sWarrantyType <> "" Then
            nWarrantyType = Convert.ToInt32(sWarrantyType)
        End If

        dtData = oWarranty.QuerySWSet("", sOperationCenter, sProductGroup, nWarrantyType, "", "")

        Call ArrangementData(dtData)

        Dim dtTmp As DataTable = Session("_dtTmp")
        Dim dvTmp As DataView = dtTmp.DefaultView

        Call RMA_DataBind(dvTmp, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dtData As WarrantyDTO.SWSETDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim sRMANo As String = ""

        Dim dtTmp As DataTable = Session("_dtTmp")
        dtTmp.Rows.Clear()
        Dim dvTmp As DataView = dtTmp.DefaultView
        For i = 0 To dtData.Rows.Count - 1
            Dim dr As WarrantyDTO.SWSETRow = dtData.Rows(i)
            Dim sw_id As String = dr.SW_ID.ToString().Trim()

            iCount = iCount + 1
            Dim drTmp As DataRow = dtTmp.NewRow
            drTmp("SEQID") = iCount
            drTmp("sw_id") = dr.SW_ID
            drTmp("OperationCenter") = dr.SW_COMPNO
            drTmp("ProductGroup") = dr.SW_GROUP
            drTmp("Version") = dr.SW_VERSION.ToString("000")
            drTmp("WarrantyName") = dr.SW_NAME
            drTmp("COMP_NAME") = dr.COMP_NAME
            If dr.SW_TYPE01 = 1 Then
                drTmp("WarrantyType") = "Component"
            ElseIf dr.SW_TYPE01 = 2 Then
                drTmp("WarrantyType") = "SW"
            Else
                drTmp("WarrantyType") = "Additional Service"
            End If

            drTmp("ExtraMonths") = dr.SW_EXTMM
            drTmp("Years") = dr.SW_STDYY
            drTmp("Price") = dr.SW_PRICE
            If dr.SW_STATUS = 1 Then
                drTmp("Status") = "Flow"
            ElseIf dr.SW_STATUS = 2 Then
                drTmp("Status") = "Confirmed"
            ElseIf dr.SW_STATUS = 3 Then
                drTmp("Status") = "Invaild"
            Else
                drTmp("Status") = "Open"
            End If
            dtTmp.Rows.Add(drTmp)
        Next

        Session("_dtTmp") = dtTmp
    End Sub

    Private Sub RMA_DataBind(ByVal dvTmp As DataView, ByVal iPageIndex As Integer)
        dvTmp.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvSales.PageSize = _PageSize
        Me.UI_dvSales.PageIndex = iPageIndex
        Me.UI_dvSales.DataSource = dvTmp
        Me.UI_dvSales.DataBind()
    End Sub

    Protected Sub UI_dvSales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSales.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")

            UI_SeqID.Text = (Me.UI_dvSales.PageIndex * Me.UI_dvSales.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)

            If lblStatus.Text.Trim() <> "Open" Then
                UI_cmdEdit.Text = "Detail"
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
    Protected Sub UI_dvSales_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSales.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtTmp") Is Nothing Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand

        If e.CommandName = "cmdDetail" Or e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim lblSwID As Label = row.FindControl("lblSwID")

            Me.lblPreviousPage_SwID.Text = lblSwID.Text.Trim
        End If

    End Sub

    Protected Sub UI_dvSales_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvSales.Sorting

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

        If IsNothing(Session("_dtTmp")) = False Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvSales.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvSales.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvSales.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvSales.Columns(i).HeaderText = sHeaderText
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
        If UI_CboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.UI_CboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        If UI_CboWarrantyType.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_WarrantyType") = Me.UI_CboWarrantyType.SelectedValue.Trim()
        Else
            Me.ViewState("_WarrantyType") = "-1"
        End If
        Me.ViewState("_ProductGroup") = Me.UI_txtProductGroup.Text.Trim()

        Call QueryData(0)
    End Sub
    Protected Sub UI_cmdProductGroupPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdProductGroupPick.Click
        Dim dtProductGroup As DataTable = Me.ViewState("_dtProductGroup")
        Me.ucProductGroup.show(dtProductGroup, True)
    End Sub
End Class


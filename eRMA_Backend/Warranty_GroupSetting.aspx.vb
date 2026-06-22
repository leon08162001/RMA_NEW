Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Warranty_GroupSetting
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SortExpression") = "GROUP_NO"
            Me.ViewState("_SortDirection") = "ASC"

            Session("_dtTmp") = Nothing

            Dim oWarranty As New ctlWarranty
            Dim dtWarrGroup As New WarrantyDTO.WarrGroupTypeDataTable
            dtWarrGroup = oWarranty.QueryWarrGroup("", "")
            ViewState("_dtWarrGroup") = dtWarrGroup

            Call setControls()
            Call QueryData(0)
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Dim dtTmp As New DataTable
        dtTmp.Columns.Add("SEQID")
        dtTmp.Columns.Add("GROUP_NO")
        dtTmp.Columns.Add("GROUP_NAME")
        dtTmp.Columns.Add("GROUP_AD")
        dtTmp.Columns.Add("GROUP_ADNAME")
        dtTmp.Columns.Add("GROUP_CSTMP")
        dtTmp.Columns.Add("GROUP_LUAD")
        dtTmp.Columns.Add("GROUP_LUADNAME")
        dtTmp.Columns.Add("GROUP_LUSTMP")

        Session("_dtTmp") = dtTmp
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.WarrGroupTypeDataTable
        dtData = oWarranty.QueryWarrGroup("", "")

        Call ArrangementData(dtData)

        Dim dtTmp As DataTable = Session("_dtTmp")
        Dim dvTmp As DataView = dtTmp.DefaultView

        Call RMA_DataBind(dvTmp, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dtData As WarrantyDTO.WarrGroupTypeDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim sRMANo As String = ""

        Dim dtTmp As DataTable = Session("_dtTmp")
        dtTmp.Rows.Clear()
        Dim dvTmp As DataView = dtTmp.DefaultView
        For i = 0 To dtData.Rows.Count - 1
            Dim dr As WarrantyDTO.WarrGroupTypeRow = dtData.Rows(i)
            Dim GROUP_NO As String = dr.GROUP_NO.ToString().Trim()

            iCount = iCount + 1
            Dim drTmp As DataRow = dtTmp.NewRow
            drTmp("SEQID") = iCount
            drTmp("GROUP_NO") = dr.GROUP_NO
            drTmp("GROUP_NAME") = dr.GROUP_NAME
            drTmp("GROUP_AD") = dr.GROUP_AD
            drTmp("GROUP_ADNAME") = dr.GROUP_ADNAME
            drTmp("GROUP_CSTMP") = dr.GROUP_CSTMP
            drTmp("GROUP_LUAD") = dr.GROUP_LUAD
            drTmp("GROUP_LUADNAME") = dr.GROUP_LUADNAME
            drTmp("GROUP_LUSTMP") = dr.GROUP_LUSTMP

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
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SEQID")

            UI_SeqID.Text = (Me.UI_dvSales.PageIndex * Me.UI_dvSales.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
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
            Dim lblGroupNo As Label = row.FindControl("lblGroupNo")

            Me.lblPreviousPage_GroupNo.Text = lblGroupNo.Text.Trim
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
End Class


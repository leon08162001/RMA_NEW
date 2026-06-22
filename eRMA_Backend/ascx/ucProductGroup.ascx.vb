Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucProductGroup
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    'Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _PageSize As String = "10"




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub


    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        'Me.UI_lblAddressTittle.Text = _oLanguage.getText("Warranty", "055", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub


    Private Sub QueryData(ByVal dtAdress As DataTable, ByVal iPageIndex As Integer)
        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = iPageIndex
        Me.UI_dvAddress.DataSource = dtAdress.DefaultView
        Me.UI_dvAddress.DataBind()
    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Dim oWarranty As New ctlWarranty
        Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
        dtProductGroup = oWarranty.QueryPrdGroup(txtModel.Text, "")
        ViewState("_dtAdress") = dtProductGroup

        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = 0
        Me.UI_dvAddress.DataSource = dtProductGroup.DefaultView
        Me.UI_dvAddress.DataBind()
        Me.ajModalProgress.Show()
    End Sub


    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvAddress.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = "Model" ' _oLanguage.getText("Warranty", "056", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = "Description" '_oLanguage.getText("Warranty", "057", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim raoAddress As RadioButton = e.Row.FindControl("raoAddress")
            '先清除
            raoAddress.Checked = False
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



    Protected Sub UI_dvAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvAddress.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Dim dtAdress As DataTable = Me.ViewState("_dtAdress")
        Call QueryData(dtAdress, iPageIndex)
        Me.ajModalProgress.Show()
    End Sub



    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0

        For i = 0 To Me.UI_dvAddress.Rows.Count - 1
            Dim raoAddress As RadioButton = Me.UI_dvAddress.Rows(i).FindControl("raoAddress")
            Dim UI_CUAddress As Label = Me.UI_dvAddress.Rows(i).FindControl("UI_CUAddress")

            If raoAddress.Checked = True Then
                '20220725 wisely 組品名
                Dim UI_txtWarrantyName As TextBox = Me.Parent.FindControl("UI_txtWarrantyName")
                Dim UI_cboWarrantyType As DropDownList = Me.Parent.FindControl("UI_cboWarrantyType")
                Dim UI_cboProgramType As DropDownList = Me.Parent.FindControl("UI_cboProgramType")
                Dim UI_cboItemType As DropDownList = Me.Parent.FindControl("UI_cboItemType")
                Dim UI_cboPriceVer As DropDownList = Me.Parent.FindControl("UI_cboPriceVer")


                Dim UI_txtAddress As TextBox = Me.Parent.FindControl("UI_txtProductGroup")
                Dim lbl_part_no As Label = Me.Parent.FindControl("lbl_part_no")

                UI_txtAddress.Text = UI_CUAddress.Text.Trim()

                If lbl_part_no IsNot Nothing Then
                    lbl_part_no.Text = Replace(lbl_part_no.Text, UI_txtAddress.Text, UI_CUAddress.Text.Trim())
                    'UI_txtWarrantyName.Text = UI_txtAddress.Text +
                    '                          If(UI_cboWarrantyType.SelectedIndex >= 0, UI_cboWarrantyType.Items(UI_cboWarrantyType.SelectedIndex).Text, "") +
                    '                          If(UI_cboProgramType.SelectedIndex >= 0, UI_cboProgramType.Items(UI_cboProgramType.SelectedIndex).Text, "") +
                    '                          If(UI_cboItemType.SelectedIndex >= 0, UI_cboItemType.Items(UI_cboItemType.SelectedIndex).Text, "") +
                    '                          If(UI_cboPriceVer.SelectedIndex >= 0, UI_cboPriceVer.Items(UI_cboPriceVer.SelectedIndex).Text, "")

                    UI_txtWarrantyName.Text = oCommon.GetWarrsetPartNM(UI_txtAddress.Text, UI_cboWarrantyType.Text, UI_cboProgramType.Text, UI_cboItemType.Text, UI_cboPriceVer.Text)
                End If
            End If

        Next

    End Sub




    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="dtAdress"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal dtAdress As DataTable, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Me.ViewState("_dtAdress") = dtAdress

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Call QueryData(dtAdress, 0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub


End Class

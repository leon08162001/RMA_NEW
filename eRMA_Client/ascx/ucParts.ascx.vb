Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucParts
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = "5"




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
        Me.UI_lblAddressTittle.Text = _oLanguage.getText("Warranty", "058", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub


    Private Sub QueryData(ByVal dtAdress As DataTable, ByVal iPageIndex As Integer)
        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = iPageIndex
        Me.UI_dvAddress.DataSource = dtAdress.DefaultView
        Me.UI_dvAddress.DataBind()
    End Sub




    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvAddress.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Warranty", "059", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Warranty", "060", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkPartNo As CheckBox = e.Row.FindControl("chkPartNo")
            '先清除
            chkPartNo.Checked = False
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
        Dim sPartNoAll As String = ""
        Dim dtSelectPart As New DataTable
        dtSelectPart.Columns.Add("PartNo")
        dtSelectPart.Columns.Add("PartDesc")


        For i = 0 To Me.UI_dvAddress.Rows.Count - 1
            Dim chkPartNo As CheckBox = Me.UI_dvAddress.Rows(i).FindControl("chkPartNo")
            Dim txtPartNo As Label = Me.UI_dvAddress.Rows(i).FindControl("txtPartNo")
            Dim txtPartDesc As Label = Me.UI_dvAddress.Rows(i).FindControl("txtPartDesc")

            If chkPartNo.Checked = True Then
                dtSelectPart.NewRow()
                dtSelectPart.Rows.Add(New Object() {txtPartNo.Text.Trim(), txtPartDesc.Text().Trim()})
            End If
        Next

        Session("dtSelectPart") = dtSelectPart
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

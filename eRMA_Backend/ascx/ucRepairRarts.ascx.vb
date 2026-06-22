Imports System.Data
Imports DataService
Imports DefLanguage



Partial Class ascx_ucRepairRarts
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _RepairBOM_No As String = ConfigurationSettings.AppSettings("RepairBOM_No")




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
        Me.UI_lblTitle.Text = _oLanguage.getText("RMA", "221", ctlLanguage.eumType.Tag)
        Me.UI_lblKeyword.Text = _oLanguage.getText("RMA", "174", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub



    ''' <summary>
    ''' KeyWord 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)

        Me.ajModalProgress.Show()
    End Sub



    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim dtRepairBom As New RmaDTO.RepairBOMDataTable
        Dim oRepairBOM As New ctlRMA.RepairBOM

        dtRepairBom = oRepairBOM.QueryByRepairPart(_RepairBOM_No, Me.UI_txtKeyword.Text.Trim(), "")
        Dim dvRepairBom As DataView = dtRepairBom.DefaultView


        Me.UI_dvRepairRart.PageSize = _PageSize
        Me.UI_dvRepairRart.PageIndex = iPageIndex
        Me.UI_dvRepairRart.DataSource = dvRepairBom
        Me.UI_dvRepairRart.DataBind()
    End Sub



    Protected Sub UI_dvRepairRart_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRepairRart.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "152", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "161", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_Radio As RadioButton = e.Row.FindControl("UI_Radio")
            UI_Radio.Attributes.Add("onclick", "setRadio(this)")
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

    Protected Sub UI_dvRepairRart_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRepairRart.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryData(iPageIndex)
        Me.ajModalProgress.Show()
    End Sub
















    ''' <summary>
    ''' 設定是否要顯示
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 Boolean</returns>
    ''' <remarks></remarks>
    Public Property show() As Boolean
        Get
            Dim retval As Boolean = False
            If Convert.ToBoolean(Me.ViewState("_show")) = True Then
                retval = True
            End If
            Return retval
        End Get

        Set(ByVal nNewValue As Boolean)
            If nNewValue = True Then
                Me.ajModalProgress.Show()
            Else
                Me.ajModalProgress.Hide()
            End If

            Me.ViewState("_show") = nNewValue
            Call QueryData(0)

        End Set
    End Property





    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0

        For i = 0 To Me.UI_dvRepairRart.Rows.Count - 1
            Dim UI_Radio As RadioButton = Me.UI_dvRepairRart.Rows(i).FindControl("UI_Radio")
            Dim UI_PartNo As Label = Me.UI_dvRepairRart.Rows(i).FindControl("UI_PartNo")

            If UI_Radio.Checked = True Then
                Dim UI_txtPartsNo As TextBox = Me.Parent.FindControl("UI_txtPartsNo")
                UI_txtPartsNo.Text = UI_PartNo.Text.Trim()
                Session("_RepairRart_Submit") = True
            End If
        Next

    End Sub



End Class

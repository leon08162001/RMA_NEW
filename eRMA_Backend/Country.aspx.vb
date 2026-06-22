Imports DataService
Imports System.Data
Imports DefLanguage

Partial Class Country
    Inherits System.Web.UI.Page
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtCountry") = Nothing
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
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Country", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCountryTittle.Text = _oLanguage.getText("Country", "002", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Me.UI_Country.Columns(1).HeaderText = _oLanguage.getText("Country", "003", ctlLanguage.eumType.Tag)
        Me.UI_Country.Columns(2).HeaderText = _oLanguage.getText("Country", "004", ctlLanguage.eumType.Tag)
        Me.UI_Country.Columns(3).HeaderText = _oLanguage.getText("Country", "005", ctlLanguage.eumType.Tag)
        Me.UI_Country.Columns(4).HeaderText = _oLanguage.getText("Country", "006", ctlLanguage.eumType.Tag)

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oCountry As New ctlCountry
        Dim dtCountry As New CountryDTO.CountryDataTable

        dtCountry = oCountry.QueryAll()
        Session("_dtCountry") = dtCountry

        Dim dvCountry As DataView = dtCountry.DefaultView
        Me.ViewState("_SortExpression") = "COUNTRY_NAME"
        Me.ViewState("_SortDirection") = "asc"
        dvCountry.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Country_DataBind(dvCountry, iPageIndex)
    End Sub

    Private Sub Country_DataBind(ByVal dvCountry As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_Country.PageSize = _PageSize
        Me.UI_Country.PageIndex = iPageIndex
        Me.UI_Country.DataSource = dvCountry
        Me.UI_Country.DataBind()
    End Sub

    Protected Sub UI_Country_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_Country.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtCountry") Is Nothing Then
            Dim dtCountry As CountryDTO.CountryDataTable = Session("_dtCountry")
            Dim dvCountry As DataView = dtCountry.DefaultView

            Call Country_DataBind(dvCountry, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If

    End Sub

    Protected Sub UI_Country_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_Country.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_Country.PageIndex * Me.UI_Country.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_CountryVisible As DropDownList = e.Row.FindControl("UI_CountryVisible")
            UI_CountryVisible.Items(0).Text = _oLanguage.getText("Country", "007", ctlLanguage.eumType.Tag)
            UI_CountryVisible.Items(1).Text = _oLanguage.getText("Country", "008", ctlLanguage.eumType.Tag)


            Dim UI_Visible As Label = e.Row.FindControl("UI_Visible")
            Dim oDropDownList As DropDownList = e.Row.FindControl("UI_CountryVisible")

            oDropDownList.SelectedValue = UI_Visible.Text.Trim()

            Dim UI_CountryName As TextBox = e.Row.FindControl("UI_CountryName")
            Dim rfv_CountryName As RequiredFieldValidator = e.Row.FindControl("rfv_CountryName")
            rfv_CountryName.ControlToValidate = UI_CountryName.ID
            Call setValidationMessage(rfv_CountryName)
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

    Protected Sub UI_Country_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_Country.Sorting

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

        If IsNothing(Session("_dtCountry")) = False Then
            Dim dtCountry As DataTable = Session("_dtCountry")
            Dim dvCountry As DataView = dtCountry.DefaultView
            dvCountry.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Country_DataBind(dvCountry, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_Country.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_Country.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_Country.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_Country.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_Country.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_Country.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""
        sErrorMessage = _oLanguage.getText("Country", "009", ctlLanguage.eumType.Validator)
        oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
    End Sub

    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oGridView As GridView = Me.UI_Country
        Dim oCommon As New Common
        Dim oCountry As New ctlCountry
        Dim dtCountry As New CountryDTO.CountryDataTable

        Try
            Dim i As Integer = 0
            For i = 0 To oGridView.Rows.Count - 1
                Dim dr As CountryDTO.CountryRow = dtCountry.NewCountryRow

                Dim UI_CountryID As Label = oGridView.Rows(i).Cells(0).Controls(1).FindControl("UI_CountryID")
                Dim UI_CountryName As TextBox = oGridView.Rows(i).Cells(1).Controls(0).FindControl("UI_CountryName")
                Dim UI_CountryVisible As DropDownList = oGridView.Rows(i).Cells(4).FindControl("UI_CountryVisible")

                dr.COUNTRY_ID = UI_CountryID.Text.ToString().Trim()
                dr.COUNTRY_NAME = UI_CountryName.Text.ToString.Trim()
                dr.COUNTRY_VISIBLE = Convert.ToInt32(UI_CountryVisible.SelectedValue.ToString().Trim())
                dr.COUNTRY_AD = Session("_UserID").ToString().Trim()                '資料建立人帳號
                dr.COUNTRY_ADNAME = Session("_UserName").ToString().Trim()          '資料建立人姓名
                dr.COUNTRY_CSTMP = Date.Now                                         '資料建立時間
                dr.COUNTRY_LUAD = Session("_UserID").ToString().Trim()              '最後修改人
                dr.COUNTRY_LUADNAME = Session("_UserName").ToString().Trim()        '最後修改日期
                dr.COUNTRY_LUSTMP = Date.Now                                        '最後修改時間

                dtCountry.AddCountryRow(dr)
            Next

            oCountry.SaveEdit(dtCountry)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
    End Sub

End Class

Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ExchangeRate
    Inherits System.Web.UI.Page
    Dim _oLanguage As New ctlLanguage

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtExchange") = Nothing
            Call setControls()
            Call QueryData()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("ExchangeRate", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblExchangeTittle.Text = _oLanguage.getText("ExchangeRate", "002", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Me.UI_ExchangeRate.Columns(1).HeaderText = _oLanguage.getText("ExchangeRate", "003", ctlLanguage.eumType.Tag)
        Me.UI_ExchangeRate.Columns(2).HeaderText = _oLanguage.getText("ExchangeRate", "004", ctlLanguage.eumType.Tag)
        Me.UI_ExchangeRate.Columns(3).HeaderText = _oLanguage.getText("ExchangeRate", "005", ctlLanguage.eumType.Tag)
        Me.UI_ExchangeRate.Columns(4).HeaderText = _oLanguage.getText("ExchangeRate", "006", ctlLanguage.eumType.Tag)
        Me.UI_ExchangeRate.Columns(5).HeaderText = _oLanguage.getText("ExchangeRate", "007", ctlLanguage.eumType.Tag)
        Me.UI_ExchangeRate.Columns(6).HeaderText = _oLanguage.getText("ExchangeRate", "008", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData()
        Dim oExchange As New ctlExchangeRate
        Dim dtExchange As New CurrencyDTO.CurrencyDataTable

        dtExchange = oExchange.QueryAll()
        Session("_dtExchange") = dtExchange

        Dim dvExchange As DataView = dtExchange.DefaultView
        Me.ViewState("_SortExpression") = "CURRENCY_CODE"
        Me.ViewState("_SortDirection") = "asc"
        dvExchange.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Exchange_DataBind(dvExchange)
    End Sub

    Private Sub Exchange_DataBind(ByVal dvExchange As DataView)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_ExchangeRate.DataSource = dvExchange
        Me.UI_ExchangeRate.DataBind()
    End Sub

    Protected Sub UI_ExchangeRate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_ExchangeRate.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (e.Row.RowIndex + 1).ToString()

            Dim UI_CurrencyVisible As DropDownList = e.Row.FindControl("UI_CurrencyVisible")
            UI_CurrencyVisible.Items(0).Text = _oLanguage.getText("ExchangeRate", "009", ctlLanguage.eumType.Tag)
            UI_CurrencyVisible.Items(1).Text = _oLanguage.getText("ExchangeRate", "010", ctlLanguage.eumType.Tag)

            Dim UI_Visible As Label = e.Row.FindControl("UI_Visible")
            Dim oDropDownList As DropDownList = e.Row.FindControl("UI_CurrencyVisible")

            oDropDownList.SelectedValue = UI_Visible.Text.Trim()

            Dim UI_CurrencyRate As TextBox = e.Row.FindControl("UI_CurrencyRate")
            Dim rfv_CurrencyRate As RequiredFieldValidator = e.Row.FindControl("rfv_CurrencyRate")
            Dim cv_CurrencyRate As CompareValidator = e.Row.FindControl("cv_CurrencyRate")

            rfv_CurrencyRate.ControlToValidate = UI_CurrencyRate.ID
            cv_CurrencyRate.ControlToValidate = UI_CurrencyRate.ID

            rfv_CurrencyRate.ErrorMessage = _oLanguage.getText("ExchangeRate", "011", ctlLanguage.eumType.Validator)
            cv_CurrencyRate.ErrorMessage = _oLanguage.getText("ExchangeRate", "011", ctlLanguage.eumType.Validator)

        End If
    End Sub

    Protected Sub UI_ExchangeRate_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_ExchangeRate.Sorting

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

        If IsNothing(Session("_dtExchange")) = False Then
            Dim dtExchange As DataTable = Session("_dtExchange")
            Dim dvExchange As DataView = dtExchange.DefaultView
            dvExchange.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Exchange_DataBind(dvExchange)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_ExchangeRate.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_ExchangeRate.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_ExchangeRate.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_ExchangeRate.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_ExchangeRate.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_ExchangeRate.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oGridView As GridView = Me.UI_ExchangeRate
        Dim oCommon As New Common
        Dim oExchange As New ctlExchangeRate
        Dim dtExchange As New CurrencyDTO.CurrencyDataTable

        Try
            Dim i As Integer = 0
            For i = 0 To oGridView.Rows.Count - 1
                Dim dr As CurrencyDTO.CurrencyRow = dtExchange.NewCurrencyRow

                Dim UI_CurrencyCode As Label = oGridView.Rows(i).Cells(2).FindControl("UI_CurrencyCode")
                Dim UI_CurrencyRate As TextBox = oGridView.Rows(i).Cells(3).FindControl("UI_CurrencyRate")
                Dim UI_CurrencyVisible As DropDownList = oGridView.Rows(i).Cells(6).FindControl("UI_CurrencyVisible")

                dr.CURRENCY_CODE = UI_CurrencyCode.Text.ToString().Trim()
                dr.CURRENCY_RATE = Convert.ToDecimal(UI_CurrencyRate.Text.ToString.Trim())
                dr.CURRENCY_VISIBLE = Convert.ToInt32(UI_CurrencyVisible.SelectedValue.ToString().Trim())
                dr.CURRENCY_SYMBOL = ""                                              '幣別符號(ex:$,€)

                dr.CURRENCY_AD = Session("_UserID").ToString().Trim()                '資料建立人帳號
                dr.CURRENCY_ADNAME = Session("_UserName").ToString().Trim()          '資料建立人姓名
                dr.CURRENCY_CSTMP = Date.Now                                         '資料建立時間
                dr.CURRENCY_LUAD = Session("_UserID").ToString().Trim()              '最後修改人
                dr.CURRENCY_LUADNAME = Session("_UserName").ToString().Trim()        '最後修改日期
                dr.CURRENCY_LUSTMP = Date.Now                                        '最後修改時間

                dtExchange.AddCurrencyRow(dr)
            Next

            oExchange.SaveEdit(dtExchange)

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

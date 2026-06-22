Imports System.Data
Imports DataService
Imports DefLanguage



Partial Class ascx_ucCustomer_pick
    Inherits System.Web.UI.UserControl


    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_isCustomer") = False
            Me.ViewState("_isShipping") = False             '是否為 Shipping 機制
            Call setDefault()
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblShippingCustomer.Text = _oLanguage.getText("RMA", "173", ctlLanguage.eumType.Tag)
        Me.UI_lblKeyword.Text = _oLanguage.getText("RMA", "174", ctlLanguage.eumType.Tag)


        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)


        Me.UI_cmdSubmitShipment.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmitShipping.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdNewRequest.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub




    Private Sub QueryData(ByVal iPageIndex As Integer)
        If Convert.ToBoolean(Me.ViewState("_isCustomer")) = True Then
            Call QueryData_Customer(iPageIndex)
        Else
            Call QueryData_ShippingCustomer(iPageIndex)
        End If
    End Sub





    Private Sub QueryData_Customer(ByVal iPageIndex As Integer)
        Dim dvCustomer As DataView
        Dim oDataColumn As DataColumn
        Dim sCuName As String = Me.UI_txtKeyword.Text.ToString().Trim()

        If Convert.ToBoolean(Me.ViewState("_isCustomer")) = True Then
            Dim dtCustomer As New CustomerDTO.vwCustomerDataTable
            Dim oCustomer As New ctlCustomer.Customer
            dtCustomer = oCustomer.QueryKeyWord(sCuName.Trim())

            oDataColumn = New DataColumn
            oDataColumn.ColumnName = "CURRENCY_CODE"
            oDataColumn.DataType = System.Type.GetType("System.String")
            dtCustomer.Columns.Add(oDataColumn)

            oDataColumn = New DataColumn
            oDataColumn.ColumnName = "CURRENCY_RATE"
            oDataColumn.DataType = System.Type.GetType("System.String")
            dtCustomer.Columns.Add(oDataColumn)

            oDataColumn = New DataColumn
            oDataColumn.ColumnName = "Visible"
            oDataColumn.DataType = System.Type.GetType("System.String")
            oDataColumn.DefaultValue = "1"
            dtCustomer.Columns.Add(oDataColumn)

            dvCustomer = dtCustomer.DefaultView


            Me.UI_dvCustomer.PageSize = _PageSize
            Me.UI_dvCustomer.PageIndex = iPageIndex
            Me.UI_dvCustomer.DataSource = dvCustomer
            Me.UI_dvCustomer.DataBind()

        End If

    End Sub


    Private Sub QueryData_ShippingCustomer(ByVal iPageIndex As Integer)
        Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
        Dim sCuName As String = Me.UI_txtKeyword.Text.ToString().Trim()
        Dim sCompNo As String = Me.UI_lblCompNo.Text.ToString().Trim()
        Dim dvCustomer As DataView

        If Convert.ToBoolean(Me.ViewState("_isShipping")) = True Then
            '取得要 Shipping 的客戶, 只能挑選 維修中心 負責的客戶
            Dim oShipping As New ctlRMA.Shipping
            dtCustomer = oShipping.QueryByShipping_Customer(sCompNo, sCuName)

        Else
            '取得要 Shipment 的客戶, 只能挑選 業務 負責的客戶
            Dim oShipment As New ctlRMA.Shipment
            dtCustomer = oShipment.QueryByShipment_Customer(Session("_UserID").ToString(), sCuName)
        End If


        If dtCustomer.Rows.Count > 0 Then
            dvCustomer = dtCustomer.DefaultView
        Else
            dtCustomer = AddCustomer(dtCustomer)
            dvCustomer = dtCustomer.DefaultView
            dvCustomer.RowFilter = "Visible = '0'"
        End If

        Me.UI_dvCustomer.PageSize = _PageSize
        Me.UI_dvCustomer.PageIndex = iPageIndex
        Me.UI_dvCustomer.DataSource = dvCustomer
        Me.UI_dvCustomer.DataBind()
    End Sub





    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)

        Me.ajModalProgress.Show()
    End Sub




    Protected Sub UI_dvCustomer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvCustomer.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "175", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "176", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_Visible As Label = e.Row.FindControl("UI_Visible")
            If UI_Visible.Text.Trim = "0" Then
                e.Row.Visible = False
            End If

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

    Protected Sub UI_dvCustomer_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvCustomer.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryData(iPageIndex)
        Me.ajModalProgress.Show()
    End Sub



    ''' <summary>
    ''' 無客戶資料時建立一筆虛擬資料,作用是秀出表頭
    ''' </summary>
    ''' <param name="dtCustomer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddCustomer(ByVal dtCustomer As RmaDTO.tmpCustomerBySaleDataTable) As RmaDTO.tmpCustomerBySaleDataTable
        Dim drCustomer As RmaDTO.tmpCustomerBySaleRow = dtCustomer.NewRow

        Try
            drCustomer.CU_NO = ""
            drCustomer.CU_NAME = ""
            drCustomer.CU_SALESID = ""
            drCustomer.CU_ASSISTANTID = ""
            drCustomer.CURRENCY_CODE = ""
            drCustomer.CURRENCY_RATE = ""
            drCustomer.Visible = "0"
            drCustomer.CU_ADDRESS1 = ""

            dtCustomer.AddtmpCustomerBySaleRow(drCustomer)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtCustomer
    End Function





    ''' <summary>
    ''' Shipment-Submit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmitShipment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmitShipment.Click
        Dim i As Integer = 0

        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            Dim UI_Radio As RadioButton = Me.UI_dvCustomer.Rows(i).FindControl("UI_Radio")
            Dim UI_CustomerID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CustomerID")
            Dim UI_CustomerName As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CustomerName")
            Dim UI_CurrencyCode As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CurrencyCode")
            Dim UI_CurrencyRate As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CurrencyRate")
            Dim UI_CuAddress1 As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CuAddress1")      '新加的欄位

            If UI_Radio.Checked = True Then
                Dim CustomerName As TextBox = Me.Parent.FindControl("UI_txtCustomer")
                Dim CustomerID As Label = Me.Parent.FindControl("UI_lblCustomerID")
                Dim CurrencyCode As Label = Me.Parent.FindControl("UI_lblCurrencyCode")
                Dim CurrencyRate As Label = Me.Parent.FindControl("UI_lblCurrencyRate")
                Dim AddressText As Label = Me.Parent.FindControl("UI_lblAddressText")

                CustomerName.Text = UI_CustomerName.Text.ToString().Trim()

                CustomerID.Text = UI_CustomerID.Text.ToString().Trim()
                CurrencyCode.Text = UI_CurrencyCode.Text.ToString().Trim()
                CurrencyRate.Text = UI_CurrencyRate.Text.ToString().Trim()

                If IsNothing(AddressText) = False Then
                    AddressText.Text = UI_CuAddress1.Text.ToString().Trim()
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Shipping-Submit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmitShipping_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmitShipping.Click
        Dim i As Integer = 0

        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            Dim UI_Radio As RadioButton = Me.UI_dvCustomer.Rows(i).FindControl("UI_Radio")
            Dim UI_CustomerID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CustomerID")
            Dim UI_CustomerName As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CustomerName")
            Dim UI_CurrencyCode As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CurrencyCode")
            Dim UI_CurrencyRate As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CurrencyRate")

            If UI_Radio.Checked = True Then
                Dim UI_txtPacking As TextBox = Me.Parent.FindControl("UI_txtPacking")
                Dim CustomerName As TextBox = Me.Parent.FindControl("UI_txtCustomer")

                Dim CustomerID As Label = Me.Parent.FindControl("UI_lblCustomerID")
                Dim CurrencyCode As Label = Me.Parent.FindControl("UI_lblCurrencyCode")
                Dim CurrencyRate As Label = Me.Parent.FindControl("UI_lblCurrencyRate")

                UI_txtPacking.Text = UI_CustomerName.Text.ToString().Trim()
                CustomerName.Text = UI_CustomerName.Text.ToString().Trim()

                CustomerID.Text = UI_CustomerID.Text.ToString().Trim()
                CurrencyCode.Text = UI_CurrencyCode.Text.ToString().Trim()
                CurrencyRate.Text = UI_CurrencyRate.Text.ToString().Trim()

                '客戶地址
                Call setCustomer(UI_CustomerID.Text.ToString().Trim())
            End If
        Next
    End Sub

    ''' <summary>
    ''' New Request 挑選客戶
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdNewRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdNewRequest.Click
        Dim i As Integer = 0

        Dim UI_lblAccountIDText As Label = Me.Parent.FindControl("UI_lblAccountIDText")
        Dim UI_lblAccountNameText As Label = Me.Parent.FindControl("UI_lblAccountNameText")
        Dim UI_lblRepairCenterText As Label = Me.Parent.FindControl("UI_lblRepairCenterText")
        Dim UI_lblRepairCenterValue As Label = Me.Parent.FindControl("UI_lblRepairCenterValue")
        Dim UI_txtApplicant As TextBox = Me.Parent.FindControl("UI_txtApplicant")
        Dim UI_txtMail As TextBox = Me.Parent.FindControl("UI_txtMail")
        Dim UI_lblUserIDText As Label = Me.Parent.FindControl("UI_lblUserIDText")
        Dim UI_txtTel As TextBox = Me.Parent.FindControl("UI_txtTel")

        Dim UI_txtAccountIDText As TextBox = Me.Parent.FindControl("UI_txtAccountIDText")

        UI_lblAccountIDText.Text = ""
        UI_lblAccountNameText.Text = ""
        UI_lblRepairCenterText.Text = ""
        UI_lblRepairCenterValue.Text = ""
        UI_txtApplicant.Text = ""
        UI_txtMail.Text = ""
        UI_lblUserIDText.Text = ""
        UI_txtTel.Text = ""
        UI_txtAccountIDText.Text = ""

        For i = 0 To Me.UI_dvCustomer.Rows.Count - 1
            Dim UI_Radio As RadioButton = Me.UI_dvCustomer.Rows(i).FindControl("UI_Radio")
            If UI_Radio.Checked = True Then
                Dim UI_CustomerID As Label = Me.UI_dvCustomer.Rows(i).FindControl("UI_CustomerID")
                UI_txtAccountIDText.Text = UI_CustomerID.Text.Trim()
                Session("_PickCostomer_Submit") = True
                Exit For
            End If
        Next



    End Sub














    ''' <summary>
    ''' 客戶地址
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <remarks></remarks>
    Private Sub setCustomer(ByVal CustomerID As String)
        Dim UI_cboAddress As DropDownList = Me.Parent.FindControl("UI_cboAddress")
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.vwCustomerDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        UI_cboAddress.Items.Clear()
        dtCustome = oCustomer.QueryByCompany(CustomerID)

        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.vwCustomerRow = dtCustome.Rows(0)

            If dr.IsCU_ADDRESS1Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS1.Trim()
                oListItem.Value = dr.CU_ADDRESS1.Trim()
                UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS2Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS2.Trim()
                oListItem.Value = dr.CU_ADDRESS2.Trim()
                UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS3Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS3.Trim()
                oListItem.Value = dr.CU_ADDRESS3.Trim()
                UI_cboAddress.Items.Add(oListItem)
            End If

            If dr.IsCU_ADDRESS4Null = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CU_ADDRESS4.Trim()
                oListItem.Value = dr.CU_ADDRESS4.Trim()
                UI_cboAddress.Items.Add(oListItem)
            End If
        End If

        dtCustomerUser = oCustomerUser.Query(CustomerID)
        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            If dr.IsCUUS_ADDRESSNull = False Then
                oListItem = New ListItem
                oListItem.Text = dr.CUUS_ADDRESS.Trim()
                oListItem.Value = dr.CUUS_ADDRESS.Trim()
                UI_cboAddress.Items.Add(oListItem)
            End If
        Next

        UI_cboAddress.Dispose()
    End Sub



    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="sCuName"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sCuName As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Me.ViewState("_isShipping") = False

        Me.UI_cmdSubmitShipping.Visible = False
        Me.UI_cmdSubmitShipment.Visible = False
        Me.UI_cmdNewRequest.Visible = False

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.UI_txtKeyword.Text = sCuName.Trim()
            Me.UI_cmdSubmitShipment.Visible = True

            Call QueryData(0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="sCuName"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sCuName As String, ByVal isShow As Boolean, ByVal CompNo As String)
        Me.ViewState("_show") = isShow
        Me.ViewState("_isShipping") = True

        Me.UI_cmdSubmitShipping.Visible = False
        Me.UI_cmdSubmitShipment.Visible = False
        Me.UI_cmdNewRequest.Visible = False

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.UI_txtKeyword.Text = sCuName.Trim()
            Me.UI_lblCompNo.Text = CompNo.Trim()        '新加的條件
            Me.UI_cmdSubmitShipping.Visible = True

            Call QueryData(0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub



    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub showByNewRequest(ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Me.ViewState("_isCustomer") = True

        Me.UI_cmdSubmitShipping.Visible = False
        Me.UI_cmdSubmitShipment.Visible = False
        Me.UI_cmdNewRequest.Visible = False

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.UI_cmdNewRequest.Visible = True

            Call QueryData_Customer(0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub










End Class

Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucWarrantyOrderCust
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




    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvAddress.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Warranty", "056", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Warranty", "057", ctlLanguage.eumType.Tag)
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

        Dim dtCustomer As DataTable = Me.ViewState("_dtCustomer")
        Call QueryData(dtCustomer, iPageIndex)
        Me.ajModalProgress.Show()
    End Sub



    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0

        For i = 0 To Me.UI_dvAddress.Rows.Count - 1
            Dim raoAddress As RadioButton = Me.UI_dvAddress.Rows(i).FindControl("raoAddress")
            Dim UI_CUNo As Label = Me.UI_dvAddress.Rows(i).FindControl("UI_CUNo")
            Dim UI_CUName As Label = Me.UI_dvAddress.Rows(i).FindControl("UI_CUName")
            Dim lblCurrency As Label = Me.UI_dvAddress.Rows(i).FindControl("lblCurrency")
            Dim lblOrderType As Label = Me.UI_dvAddress.Rows(i).FindControl("lblOrderType")
            Dim lblSalesID As Label = Me.UI_dvAddress.Rows(i).FindControl("lblSalesID")
            Dim lblSalesName As Label = Me.UI_dvAddress.Rows(i).FindControl("lblSalesName")

            If raoAddress.Checked = True Then
                Dim txtCustomer As TextBox = Me.Parent.FindControl("txtCustomer")
                Dim txtCustomerName As TextBox = Me.Parent.FindControl("txtCustomerName")
                Dim cboWarrantyType As DropDownList = Me.Parent.FindControl("cboWarrantyType")
                Dim txtCurrency As TextBox = Me.Parent.FindControl("txtCurrency")
                Dim txtSales As TextBox = Me.Parent.FindControl("txtSales")
                Dim txtSalesName As TextBox = Me.Parent.FindControl("txtSalesName")

                txtCustomer.Text = UI_CUNo.Text.Trim()
                txtCustomerName.Text = UI_CUName.Text.Trim()

                'txtCurrency.Enabled = True
                txtCurrency.Text = lblCurrency.Text.Trim()
                'txtCurrency.Enabled = False

                txtSales.Text = lblSalesID.Text
                txtSalesName.Text = lblSalesName.Text

                cboWarrantyType.Enabled = True
                If lblOrderType.Text.Equals("LOC") Then
                    cboWarrantyType.SelectedValue = 1
                Else
                    cboWarrantyType.SelectedValue = 2
                End If
                cboWarrantyType.Enabled = False

            End If
        Next

    End Sub




    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="dtCustomer"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal isShow As Boolean, ByVal pOperationCenter As String)
        Me.ViewState("_show") = isShow
        Dim tOperationCenter As String = ""
        If (InStr(1, pOperationCenter, "CL_CHINA") >= 1) Then
            tOperationCenter = "CL_CHINA"
        Else
            tOperationCenter = "CLHQ"
        End If
        Me.ViewState("_OperationCenter") = tOperationCenter
        'Throw New "AAA" & tOperationCenter
        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Dim oWarranty As New ctlWarranty
            Dim dtCustomer As New WarrantyDTO.OCC_FILEDataTable
            'Response.Write(tOperationCenter)
            dtCustomer = oWarranty.QueryCustomer("", "", "", tOperationCenter, "", "")

            ViewState("_dtCustomer") = dtCustomer

            QueryData(dtCustomer, 0)
            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Dim oWarranty As New ctlWarranty
        Dim dtCustomer As New WarrantyDTO.OCC_FILEDataTable
        Dim sOperationCenter As String = Me.ViewState("_OperationCenter").ToString().Trim()
        dtCustomer = oWarranty.QueryCustomer("", txtCustomer.Text, "", sOperationCenter, "", "")
        ViewState("_dtCustomer") = dtCustomer

        QueryData(dtCustomer, 0)
        Me.ajModalProgress.Show()
    End Sub

End Class

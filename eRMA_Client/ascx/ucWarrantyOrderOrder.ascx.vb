Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucWarrantyOrderOrder
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
            'e.Row.Cells(1).Text = _oLanguage.getText("Warranty", "056", ctlLanguage.eumType.Tag)
            'e.Row.Cells(2).Text = _oLanguage.getText("Warranty", "057", ctlLanguage.eumType.Tag)
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
            Dim lblOrderNo As Label = Me.UI_dvAddress.Rows(i).FindControl("lblOrderNo")
            Dim lblOrderSeq As Label = Me.UI_dvAddress.Rows(i).FindControl("lblOrderSeq")
            Dim lblProduct As Label = Me.UI_dvAddress.Rows(i).FindControl("lblProduct")
            Dim lblDesc As Label = Me.UI_dvAddress.Rows(i).FindControl("lblDesc")

            If raoAddress.Checked = True Then
                Dim txtCustomer As TextBox = Me.Parent.FindControl("txtCustomer")
                Dim txtOrderNo As TextBox = Me.Parent.FindControl("txtOrderNo")
                Dim txtOrderSeq As TextBox = Me.Parent.FindControl("txtOrderSeq")
                Dim txtSKU As TextBox = Me.Parent.FindControl("txtSKU")
                Dim txtDescription As Label = Me.Parent.FindControl("txtDescription")
                Dim txtModel As Label = Me.Parent.FindControl("txtModel")
                Dim cboVersion As DropDownList = Me.Parent.FindControl("cboVersion")
                Dim cboOperationCenter As DropDownList = Me.Parent.FindControl("cboOperationCenter")
                Dim rdoType As DropDownList = Me.Parent.FindControl("rdoType")
                Dim btnSKUSel As Button = Me.Parent.FindControl("btnSKUSel")
                Dim txtPurchaseYear As TextBox = Me.Parent.FindControl("txtPurchaseYear")

                txtOrderNo.Text = lblOrderNo.Text.Trim()
                txtOrderSeq.Text = lblOrderSeq.Text.Trim()
                txtSKU.Text = lblProduct.Text.Trim()
                txtSKU.Enabled = False
                btnSKUSel.Visible = False
                txtDescription.Text = lblDesc.Text.Trim()
                txtPurchaseYear.Enabled = True

                Dim oWarranty As New ctlWarranty
                Dim dt As DataTable = oWarranty.QueryPrdGroupByPartNo(txtSKU.Text, "")
                If dt.Rows.Count > 0 Then
                    txtModel.Text = dt.Rows(0)("tc_oae020").ToString().Trim()
                End If


                'If rdoType.SelectedValue.ToString().Trim() = "CW" Then
                '    Dim dtCwVer As New WarrantyDTO.WARRSETDataTable
                '    dtCwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), "CW", txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
                '    dtCwVer.DefaultView.RowFilter = "WAR_STATUS=2"
                '    cboVersion.DataTextField = "war_name"
                '    cboVersion.DataValueField = "war_id"
                '    cboVersion.DataSource = dtCwVer.DefaultView
                '    cboVersion.DataBind()
                'End If
                'If rdoType.SelectedValue.ToString().Trim() = "EW" Then
                '    Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
                '    dtEwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), "EW", txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
                '    dtEwVer.DefaultView.RowFilter = "WAR_STATUS=2"
                '    cboVersion.DataTextField = "war_name"
                '    cboVersion.DataValueField = "war_id"
                '    cboVersion.DataSource = dtEwVer.DefaultView
                '    cboVersion.DataBind()
                'End If

                Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
                dtEwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), rdoType.SelectedValue.ToString().Trim(), txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
                dtEwVer.DefaultView.RowFilter = "WAR_STATUS=2"
                cboVersion.DataTextField = "war_name"
                cboVersion.DataValueField = "war_id"
                cboVersion.DataSource = dtEwVer.DefaultView
                cboVersion.DataBind()

                If rdoType.SelectedValue.ToString().Trim() = "SW" Then
                    Dim dtSwVer As New WarrantyDTO.SWSETDataTable
                    dtSwVer = oWarranty.QuerySWSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), -1, "Y", "")
                    dtSwVer.DefaultView.RowFilter = "SW_STATUS=2"
                    cboVersion.DataTextField = "sw_name"
                    cboVersion.DataValueField = "sw_id"
                    cboVersion.DataSource = dtSwVer.DefaultView
                    cboVersion.DataBind()
                    SetYear()
                End If

            End If
        Next

    End Sub


    Public Sub SetYear()
        Dim cboVersion As DropDownList = Me.Parent.FindControl("cboVersion")
        Dim txtPurchaseYear As TextBox = Me.Parent.FindControl("txtPurchaseYear")
        Dim sSWID As String = ""
        If cboVersion.SelectedIndex > -1 Then
            sSWID = cboVersion.SelectedValue.Trim()

            Dim dtSwVer As New WarrantyDTO.SWSETDataTable
            Dim oWarranty As New ctlWarranty
            dtSwVer = oWarranty.QuerySWSet(sSWID, "", "", -1, "")
            If dtSwVer.Rows.Count > 0 Then
                txtPurchaseYear.Text = dtSwVer.Rows(0)("SW_STDYY").ToString()
                txtPurchaseYear.Enabled = False
            End If
        End If
    End Sub


    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="sCustNo"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sCustNo As String, ByVal isShow As Boolean, ByVal sCompNo As String)
        Me.ViewState("_show") = isShow
        Me.ViewState("_sCustNo") = sCustNo
        Me.ViewState("_sCompNo") = sCompNo
        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Dim oCustomer As New ctlCustomer.Customer
            Dim dtOrder As New DataTable
            If sCompNo = "CL_CHINA" Then
                dtOrder = oCustomer.QryOrderSH(sCustNo, "", "", "")
            Else
                dtOrder = oCustomer.QryOrder(sCustNo, "", "", "")
            End If
            ViewState("_dtCustomer") = dtOrder
            Call QueryData(dtOrder, 0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Dim oCustomer As New ctlCustomer.Customer
        Dim dtOrder As New DataTable
        Dim sCompNo As String = Me.ViewState("_sCompNo")
        If sCompNo = "CL_CHINA" Then
            dtOrder = oCustomer.QryOrderSH(ViewState("_sCustNo"), txtOrder.Text.Trim(), "", txtItem.Text.Trim())
        Else
            dtOrder = oCustomer.QryOrder(ViewState("_sCustNo"), txtOrder.Text.Trim(), "", txtItem.Text.Trim())
        End If

        ViewState("_dtCustomer") = dtOrder
        Call QueryData(dtOrder, 0)
        Me.ajModalProgress.Show()
    End Sub
End Class

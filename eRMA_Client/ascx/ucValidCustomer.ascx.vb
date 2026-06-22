Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucValidCustomer
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
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
        lblCustomer.Text = _oLanguage.getText("Common", "065", ctlLanguage.eumType.Tag)
        lblSales.Text = _oLanguage.getText("Warranty", "054", ctlLanguage.eumType.Tag)

        'Me.UI_lblAddressTittle.Text = _oLanguage.getText("RMA", "206", ctlLanguage.eumType.Tag)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)

    End Sub
    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvAddress.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "143", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "144", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Warranty", "054", ctlLanguage.eumType.Tag)

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkPartNo As CheckBox = e.Row.FindControl("chkPartNo")
            Dim txtCuNo As Label = e.Row.FindControl("txtCuNo")
            chkPartNo.Checked = False

            Dim dtWCLIENT As New WarrantyDTO.WCLIENTDataTable
            dtWCLIENT = ViewState("_dtWCLIENT")
            dtWCLIENT.DefaultView.RowFilter = "WC_CLNO='" + txtCuNo.Text.Trim() + "' AND WC_MARK=0"
            If dtWCLIENT.DefaultView.Count > 0 Then
                chkPartNo.Checked = True
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

    Protected Sub UI_dvAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvAddress.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Dim dtAdress As DataTable = Me.ViewState("_dtAdress")
        Call QueryData(dtAdress, iPageIndex)
        Me.ajModalProgress.Show()
    End Sub
    Private Sub QueryData(ByVal dtAdress As DataTable, ByVal iPageIndex As Integer)
        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = iPageIndex
        Me.UI_dvAddress.DataSource = dtAdress.DefaultView
        Me.UI_dvAddress.DataBind()
    End Sub

    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtWCLIENT As New WarrantyDTO.WCLIENTDataTable

        For i = 0 To Me.UI_dvAddress.Rows.Count - 1
            Dim chkPartNo As CheckBox = Me.UI_dvAddress.Rows(i).FindControl("chkPartNo")
            Dim txtCuNo As Label = Me.UI_dvAddress.Rows(i).FindControl("txtCuNo")
            Dim txtName As Label = Me.UI_dvAddress.Rows(i).FindControl("txtName")

            If chkPartNo.Checked = True Then
                Dim dr As WarrantyDTO.WCLIENTRow = dtWCLIENT.NewWCLIENTRow
                dr.WC_CLID = ViewState("_WC_CLID")
                dr.WC_CLNO = txtCuNo.Text
                dr.WC_CLNAME = txtName.Text
                dr.WC_AD = Session("_UserID")
                dr.WC_ADNAME = Session("_UserName")
                dr.WC_CSTMP = Date.Now
                dr.WC_LUAD = Session("_UserID")
                dr.WC_LUADNAME = Session("_UserName")
                dr.WC_LUSTMP = Date.Now
                dr.WC_MARK = 0
                dtWCLIENT.Rows.Add(dr)
            End If
        Next
        oWarranty.SaveAddWClient(dtWCLIENT)
    End Sub
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Dim oWarranty As New ctlWarranty
        Dim dtCustomer As New WarrantyDTO.OCC_FILEDataTable

        Dim sOperationCenter As String = Me.ViewState("_COMP_NO").ToString()
        Dim sCustomerNam As String = txtCustomer.Text.ToString().Trim()
        Dim sSales As String = txtSales.Text.ToString().Trim()

        dtCustomer = oWarranty.QueryCustomer("", sCustomerNam, "", sOperationCenter, sSales, "")
        ViewState("_dtAdress") = dtCustomer

        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = 0
        Me.UI_dvAddress.DataSource = dtCustomer.DefaultView
        Me.UI_dvAddress.DataBind()
        Me.ajModalProgress.Show()
    End Sub

    Protected Sub chkOKAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim chkPartAll As CheckBox = sender
        For i = 0 To UI_dvAddress.Rows.Count - 1
            If UI_dvAddress.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim chkPartNo As CheckBox = UI_dvAddress.Rows(i).FindControl("chkPartNo")
                chkPartNo.Checked = chkPartAll.Checked
            End If
        Next
        Me.ajModalProgress.Show()
    End Sub
    Public Sub show(ByVal isShow As Boolean, ByVal WC_CLID As String, ByVal COMP_NO As String, ByVal Status As String)
        Me.ViewState("_show") = isShow
        Me.ViewState("_WC_CLID") = WC_CLID
        Me.ViewState("_COMP_NO") = COMP_NO
        If Status <> "0" Or Status = "" Then
            UI_cmdSubmit.Visible = False
        Else
            UI_cmdSubmit.Visible = True
        End If
        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.ajModalProgress.Show()
            'UI_dvAddress.DataSource = Nothing
            'UI_dvAddress.DataBind()
            lbl_status.Text = Status
            Dim oWarranty As New ctlWarranty
            Dim dtWCLIENT As New WarrantyDTO.WCLIENTDataTable
            dtWCLIENT = oWarranty.QueryWClient(WC_CLID, "", "")
            ViewState("_dtWCLIENT") = dtWCLIENT

            Dim dtCustomer As New WarrantyDTO.OCC_FILEDataTable
            Dim i As Integer
            For i = 0 To dtWCLIENT.Rows.Count - 1
                Dim sTmpCust As String = dtWCLIENT.Rows(i)("WC_CLNO").ToString().Trim()
                dtCustomer.Merge(oWarranty.QueryCustomer(sTmpCust, "", "", "", "", ""))
            Next

            UI_dvAddress.DataSource = dtCustomer
            UI_dvAddress.DataBind()
        Else
            Me.ajModalProgress.Hide()
        End If


    End Sub


End Class

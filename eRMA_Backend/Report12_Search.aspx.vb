Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports DefLanguage

Partial Class Report12_Search
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtReport") = Nothing

            Call setControls()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "H_014", ctlLanguage.eumType.Tag)

        Me.UI_lblProduct_SerialNo.Text = _oLanguage.getText("Report", "154", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
    End Sub

    Private Function QueryPartsDeliveryData(ByVal custId As String, ByVal partsQId As String) As DataTable

        Dim oConn As New OracleConnection

        oConn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString3").ConnectionString


        Dim dtReport As New DataTable
        Dim sSQL As New StringBuilder

        sSQL.Append(" SELECT    ")
        sSQL.Append(" B.ITEM_SN Parts_SN, ")
        sSQL.Append(" A.PRD_ID  Parts_NO, ")
        sSQL.Append(" to_char(A.SHIP_TIME,'YYYY/MM/DD') Shipped_Time, ")
        sSQL.Append(" C.OGA03 Customer_ID, ")
        sSQL.Append(" C.OGA032 Customer_Name, ")
        sSQL.Append(" A.ORDER_ID Order_ID, ")
        sSQL.Append(" A.NOTIFY_ID Shipping_Notice_ID ")
        sSQL.Append(" from  EXTSYS.SPP_ITEM A,  EXTSYS.SPP_ITEM_SN B, CIPHERLAB.OGA_FILE C, rma.customer D ")
        sSQL.Append(" where(A.ITEM_ID = B.ITEM_ID) ")
        sSQL.Append(" and A.NOTIFY_ID = C.OGA01 ")
        sSQL.Append(" and A.ACTI='Y' ")
        sSQL.Append(" and C.OGA03 = D.CU_TIPTOP_ID")

        sSQL.Append(" and B.ITEM_SN = '")
        sSQL.Append(partsQId)
        sSQL.Append("'")

        If custId.Trim() <> "" Then
            sSQL.Append(" and D.CU_TIPTOP_ID = '")
            sSQL.Append(custId)
            sSQL.Append("'")
        End If

        Dim oDataAdapter As New OracleDataAdapter(sSQL.ToString(), oConn)

        Try
            oDataAdapter.Fill(dtReport)
            Return dtReport

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim ctlReport As New ctlReport
        Dim dtReport As New DataTable

        dtReport = QueryPartsDeliveryData(Session("_CustomerID").ToString().Trim(), Me.UI_txtParts_SerialNo.Text.Trim())

        Call dvReport_DataBind(dtReport, iPageIndex)
    End Sub

    Private Sub dvReport_DataBind(ByVal dtReport As DataTable, ByVal iPageIndex As Integer)
        Session("_dtReport") = dtReport

        'Me.UI_dvReport.PageSize = _PageSize
        'Me.UI_dvReport.PageIndex = iPageIndex
        Me.UI_dvReport.DataSource = dtReport.DefaultView
        Me.UI_dvReport.DataBind()
    End Sub

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = _oLanguage.getText("Report", "H_015", ctlLanguage.eumType.Tag)
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "H_016", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "H_017", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "H_018", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "H_019", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "H_020", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "H_021", ctlLanguage.eumType.Tag)
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

    Protected Sub UI_dvReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvReport.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtReport") Is Nothing Then
            Dim dtReport As ReportDTO.Rpt_RMAWarrantyDataTable = Session("_dtReport")
            Call dvReport_DataBind(dtReport, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

End Class

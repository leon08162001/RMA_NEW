Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports DefLanguage

Partial Class Report11_Search
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
        Me.UI_lblTittle.Text = _oLanguage.getText("Report", "H_001", ctlLanguage.eumType.Tag)
        Me.UI_lblProduct_SerialNo.Text = _oLanguage.getText("Report", "H_002", ctlLanguage.eumType.Tag)
        Me.UI_lblReportTittle.Text = _oLanguage.getText("Report", "030", ctlLanguage.eumType.Tag)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        '身分:1.客戶, 2.公司(維修中心)
        'New Request
        If Session("_Identity") = "1" Then
            '抓取客戶的tiptop代號
            Dim oConn As New OracleConnection
            oConn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString4").ConnectionString


            Dim dtReport As New DataTable
            Dim sSQL As New StringBuilder

            sSQL.Append(" select CU_TIPTOP_ID from CUSTOMER where CU_NO='")
            sSQL.Append(Session("_UserID").ToString())
            sSQL.Append("'")

            Try
                Dim oDataAdapter As New OracleDataAdapter(sSQL.ToString(), oConn)

                oDataAdapter.Fill(dtReport)

                Me.UI_txtProduct_SerialNo.Text = dtReport.Rows(0)(0).ToString()
                Me.UI_txtProduct_SerialNo.Enabled = False

                If dtReport.Rows(0)(0).ToString() > "" Then
                    Call UI_cmdSearch_Click(Nothing, Nothing)
                Else
                    Me.UcMessage.showMessageByAlert("Can't Find Your System Customer ID.<br> Please Contact Your Sales!!")
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


        Else
            '可輸入代號查詢
            Me.UI_txtProduct_SerialNo.Enabled = True
        End If

    End Sub

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

        dtReport = QueryOrderDeliveryData(Session("_CustomerID").ToString().Trim(), Me.UI_txtProduct_SerialNo.Text.Trim())

        Call dvReport_DataBind(dtReport, iPageIndex)
    End Sub

    Private Sub dvReport_DataBind(ByVal dtReport As DataTable, ByVal iPageIndex As Integer)

        Session("_dtReport") = dtReport

        'Me.UI_dvReport.PageSize = _PageSize
        'Me.UI_dvReport.PageIndex = iPageIndex

        Me.UI_dvReport.DataSource = dtReport.DefaultView
        Me.UI_dvReport.DataBind()

    End Sub

    Private Function QueryOrderDeliveryData(ByVal custId As String, ByVal custQId As String) As DataTable

        Dim oConn As New OracleConnection

        oConn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString3").ConnectionString


        Dim dtReport As New DataTable
        Dim sSQL As New StringBuilder

        sSQL.Append(" select oeb01 as ""Order No."", oeb03 as ""Item"", oeb04 as ""Product No."" , imc04 as ""Desc"", to_char(oea02,'YYYY/MM/DD') as ""Ord. dt"", to_char(oeb16,'YYYY/MM/DD') as ""E.D.D"" , to_char(oga02_notice,'YYYY/MM/DD') as ""Plan Ship dt"", to_char(oga02_out,'YYYY/MM/DD') as ""Ship dt"", out_cnt as ""Del. Qty."" , oeb12-a as ""Undel. Qty."" , oea10 as ""Customer Ord. No."" from oeb_file join oea_file on oea01 = oeb01 ")
        sSQL.Append(" left join oao_file on oao04=1 and oao01=oeb01 and oao03=oeb03 left join(select ogb31,ogb32,max(oga02) as oga02_notice from oga_file, ogb_file where oga01=ogb01 and ")
        sSQL.Append(" ogaconf='Y' and oga01 like 'AXGA%' group by  ogb31,ogb32) not1 on   not1.ogb31=oeb01 and oeb03=not1.ogb32 ")
        sSQL.Append(" left join(select ogb31,ogb32,max(oga02) as oga02_out from oga_file, ogb_file ")
        sSQL.Append(" where oga01=ogb01 and ogaconf='Y' and oga01 like 'AXKA%'  ")
        sSQL.Append(" group by  ogb31,ogb32) out1 on   out1.ogb31=oeb01 and oeb03=out1.ogb32 ")
        sSQL.Append(" left join (select ogb31,ogb32,oga02,sum(ogb12) as out_cnt from oga_file, ogb_file ")
        sSQL.Append(" where oga01=ogb01 and ogaconf='Y' and oga01 like 'AXKA%' and ogb12>0 ")
        sSQL.Append(" group by  ogb31,ogb32,oga02) out2 on out2.ogb31=oeb01 and oeb03=out2.ogb32 and out2.oga02=out1.oga02_out ")
        sSQL.Append(" left join ")
        sSQL.Append(" (select oeb01 as ogb31 ,oeb03 as ogb32,nvl(ogb12,0) as a ")
        sSQL.Append(" from oeb_file join oea_file on oea01 = oeb01 ")
        sSQL.Append(" left join (select ogb31,ogb32, sum(ogb12) as ogb12  from ogb_file ,oga_file b ")
        sSQL.Append(" where ogaconf='Y' and oga09<>'1' and ogapost = 'Y' and oga01=ogb01 ")
        sSQL.Append(" group by ogb31,ogb32) ogb1 on ogb1.ogb31=oeb01 and oeb03=ogb1.ogb32 ")
        sSQL.Append(" where oeaconf='Y' ")
        sSQL.Append(" and oea00<>'0') ogb2 on   ogb2.ogb31=oeb01 and ogb2.ogb32=oeb03 join ima_file on ima01=oeb04 ")
        sSQL.Append(" left join imc_file on imc02='0000' and imc03=1 and ima01=imc01 ")
        sSQL.Append(" where oeaconf='Y' ")
        sSQL.Append(" and oea00<>'0' and (oeb12-nvl(a,0)>0 or (out_cnt>0  and oga02_out>=sysdate-30))")
        sSQL.Append(" and oea03='")
        sSQL.Append(custQId)
        sSQL.Append("'")

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

    Protected Sub UI_dvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvReport.RowDataBound

        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = _oLanguage.getText("Report", "H_003", ctlLanguage.eumType.Tag)
            e.Row.Cells(1).Text = _oLanguage.getText("Report", "H_004", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Report", "H_005", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Report", "H_006", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Report", "H_007", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Report", "H_008", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Report", "H_009", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Report", "H_010", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("Report", "H_011", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("Report", "H_012", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("Report", "H_013", ctlLanguage.eumType.Tag)

        End If

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
        '    'UI_SeqID.Text = (Me.UI_dvReport.PageIndex * Me.UI_dvReport.PageSize) + (e.Row.RowIndex + 1).ToString()

        '    Dim UI_isDetail As Label = e.Row.FindControl("UI_isDetail")
        '    If UI_isDetail.Text.Trim = "2" Then
        '        'e.Row.Cells(1).Visible = False
        '        e.Row.Cells(2).Visible = False
        '        e.Row.Cells(3).Visible = False
        '        e.Row.Cells(4).Visible = False
        '        e.Row.Cells(5).Visible = False
        '    End If

        '    If UI_isDetail.Text.Trim = "1" Then
        '        e.Row.BackColor = Drawing.Color.Silver
        '    End If

        'End If


        'If e.Row.RowType = DataControlRowType.Pager Then
        '    Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
        '    For i = 0 To iLoop - 1
        '        If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
        '            Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
        '            oLabel.ForeColor = Drawing.Color.Red
        '            oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
        '        End If
        '    Next
        'End If
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

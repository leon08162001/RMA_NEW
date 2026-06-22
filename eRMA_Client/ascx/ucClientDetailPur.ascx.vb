Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucClientDetailPur
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_RMADID") = ""
            Me.ViewState("_RMANO") = ""

            Call setControls()
        End If
    End Sub
    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)
    End Sub

    Private Sub CreatePurKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.dgvPurchasing.Columns.Count - 1
            Dim sHeaderText As String = Me.dgvPurchasing.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.dgvPurchasing.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_PurSortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.dgvPurchasing.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.dgvPurchasing.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.dgvPurchasing.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Private Sub ArrangementData(ByVal dtTmpImport As DataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim iPurCount As Integer = 0
        Dim sRMANo As String = ""
        Dim dtPurchasing As DataTable = Session("_dtPurchasing")
        dtPurchasing.Rows.Clear()
        Dim oWarranty As New ctlWarranty
        For i = 0 To dtTmpImport.Rows.Count - 1
		
            If dtTmpImport.Rows(i)(0).ToString().Trim() <> "" Then
			'Throw new Exception(dtTmpImport.Rows(i)(0).ToString().Trim())
                Dim sSerailNo As String = dtTmpImport.Rows(i)(0).ToString().Trim()
                iCount = iCount + 1
                Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(sSerailNo)
                If dtPur.Rows.Count > 0 Then
                    Dim k As Integer
                    For k = 0 To dtPur.Rows.Count - 1
                        iPurCount = iPurCount + 1
                        dtPurchasing.NewRow()
                        dtPurchasing.Rows.Add(New Object() {})
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Seq") = iPurCount
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SerialNo") = sSerailNo

                        If dtPur.Rows(k)("waty_date").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("PurchaseDate") = Convert.ToDateTime(dtPur.Rows(k)("waty_date").ToString()).ToString("yyyy/MM/dd")
                        End If

                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("WarrantyCode") = dtPur.Rows(k)("wati_type").ToString()

                        If dtPur.Rows(k)("wats_warrnstart").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("StartDate") = Convert.ToDateTime(dtPur.Rows(k)("wats_warrnstart").ToString()).ToString("yyyy/MM/dd")
                        End If

                        If dtPur.Rows(k)("wats_warrnend").ToString() <> "" Then
                            dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("EndDate") = Convert.ToDateTime(dtPur.Rows(k)("wats_warrnend").ToString()).ToString("yyyy/MM/dd")
                        End If

                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Model") = dtPur.Rows(k)("wati_skuno").ToString()
                        dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SKU") = dtPur.Rows(k)("wati_model").ToString()
                    Next
                Else
                    'iPurCount = iPurCount + 1
                    'dtPurchasing.NewRow()
                    'dtPurchasing.Rows.Add(New Object() {})
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Seq") = iPurCount
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SerialNo") = sSerailNo
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("PurchaseDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("WarrantyCode") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("StartDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("EndDate") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("Model") = ""
                    'dtPurchasing.Rows(dtPurchasing.Rows.Count - 1)("SKU") = ""
                End If
            End If
        Next

        Session("_dtPurchasing") = dtPurchasing
    End Sub

    Protected Sub dgvPurchasing_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvPurchasing.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.dgvPurchasing.PageIndex * Me.dgvPurchasing.PageSize) + (e.Row.RowIndex + 1).ToString()
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

    Protected Sub dgvPurchasing_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvPurchasing.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtPurchasing") Is Nothing Then
            Dim dtPurchasing As DataTable = Session("_dtPurchasing")
            Dim dvPurchasing As DataView = dtPurchasing.DefaultView
            Call RMA_DataBindPur(dvPurchasing, iPageIndex)

        Else
            Call QueryDataPur(iPageIndex)
        End If
    End Sub
    Private Sub RMA_DataBindPur(ByVal dvPurchasing As DataView, ByVal iPageIndex As Integer)
        dvPurchasing.Sort = Me.ViewState("_PurSortExpression").ToString() & " " & Me.ViewState("_PurSortDirection").ToString()
        Call CreatePurKeyPoint(Me.ViewState("_PurSortExpression").ToString(), Me.ViewState("_PurSortDirection").ToString())

        Me.dgvPurchasing.PageSize = 15
        Me.dgvPurchasing.PageIndex = iPageIndex
        Me.dgvPurchasing.DataSource = dvPurchasing
        Me.dgvPurchasing.DataBind()
    End Sub

    Protected Sub dgvPurchasing_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvPurchasing.Sorting

        If Me.ViewState("_PurSortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_PurSortDirection") = "asc"
        Else
            If Me.ViewState("_PurSortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_PurSortDirection") = "desc"
            Else
                Me.ViewState("_PurSortDirection") = "asc"
            End If
        End If
        Me.ViewState("_PurSortExpression") = e.SortExpression

        If IsNothing(Session("_dtPurchasing")) = False Then
            Dim dtPurchasing As DataTable = Session("_dtPurchasing")
            Dim dvPurchasing As DataView = dtPurchasing.DefaultView
            Call RMA_DataBindPur(dvPurchasing, 0)
        End If
    End Sub

    Private Sub QueryDataPur(ByVal iPageIndex As Integer)
        Dim dtPurchasing As DataTable = Session("_dtPurchasing")
        Dim dvPurchasing As DataView = dtPurchasing.DefaultView
        Call RMA_DataBindPur(dvPurchasing, iPageIndex)
    End Sub

    Private Sub SetDataTable()
        Dim dtPurchasing As New DataTable
        dtPurchasing.Columns.Add("Seq")
        dtPurchasing.Columns.Add("SerialNo")
        dtPurchasing.Columns.Add("PurchaseDate")
        dtPurchasing.Columns.Add("WarrantyCode")
        dtPurchasing.Columns.Add("StartDate")
        dtPurchasing.Columns.Add("EndDate")
        dtPurchasing.Columns.Add("Model")
        dtPurchasing.Columns.Add("SKU")
        dtPurchasing.Columns.Add("Description")
        Session("_dtPurchasing") = dtPurchasing

    End Sub

    Public Sub show(ByVal sRMADID As String, ByVal sRMANO As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            If sRMADID.Trim <> "" And sRMANO.Trim <> "" Then
				
                Me.ViewState("_RMADID") = sRMADID
                Me.ViewState("_RMANO") = sRMANO

                Me.ViewState("_PurSortExpression") = "SerialNo"
                Me.ViewState("_PurSortDirection") = "asc"
                Session("_dtPurchasing") = Nothing
                SetDataTable()

                Dim dt As DataTable = New DataTable()
                dt.Columns.Add("SN")
                Dim oRMA As New ctlRMA.Client
                Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
                dtClientDetail = oRMA.QueryByClientDetail(Session("_LanguageID").ToString().Trim(), Me.ViewState("_RMADID").ToString())
                If dtClientDetail.Rows.Count > 0 Then
				
                    Dim dr As RmaDTO.tmpClientDetailRow = dtClientDetail.Rows(0)
                    If dr.IsRMAD_SERIALNONull = False Then					
                        Dim drs As DataRow = dt.NewRow()
                        drs(0) = dr.RMAD_SERIALNO.ToString().Trim()
                        dt.Rows.Add(drs)
                    End If
                End If


                ArrangementData(dt)
                QueryDataPur(0)
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub



End Class

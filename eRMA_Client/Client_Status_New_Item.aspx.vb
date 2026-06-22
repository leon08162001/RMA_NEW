Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Client_Status_New_Item
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _SecurityCrypt As New SecurityCrypt.Crypto
    Dim WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtClientDetail") = Nothing

            Dim RMANO As String = _SecurityCrypt.Decrypt(Request.Params("RMANO").ToString().Trim(), "")

            Me.UI_lblPreviousPage_RMAID.Text = RMANO
            Me.UI_lblPreviousPage_RMANO.Text = RMANO

            Call setControls()
            Call QueryData()
            Call QueryDataByDetail(0)

        End If
    End Sub
#End Region

    Public Function QueryCUSTOMER_PRODUCT_NUMBER(ByVal RMAD_RMANO As String) As String

        Dim Context As String = ""

        Dim ctAddress_List As New ctAddress
        RMAD_RMANO = ctAddress_List.Get_CUSTOMER_PRODUCT_NUMBER(RMAD_RMANO)

        If RMAD_RMANO <> "" Then
            Context = RMAD_RMANO
        End If

        Return Context

    End Function

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "067", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)

        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestInformation.Text = _oLanguage.getText("RMA2", "054", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        Me.UI_SavedBtn_.Text = _oLanguage.getText("RMA2", "055", ctlLanguage.eumType.Tag)

    End Sub

    Private Sub QueryData()
        Dim oRMA As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oCompany As New ctlCompany

        Dim sRMANo As String = Me.UI_lblPreviousPage_RMANO.Text

        dtRMA = oRMA.QueryByRMAHead(sRMANo)
        If dtRMA.Rows.Count > 0 Then
            Dim dr As RmaDTO.RMARow = dtRMA.Rows(0)
            Me.UI_RMANo.Text = dr.RMA_NO.Trim()
            Me.UI_RequestDate.Text = dr.RMA_CSTMP.ToShortDateString()

            Dim RMA_COMPNO As String = dr.RMA_COMPNO.Trim
            Me.UI_RepairCenter.Text = oCompany.getRepairName(RMA_COMPNO)

            Me.UI_Applicant.Text = dr.RMA_APPLICANT.Trim()
            If dr.IsRMA_RemarkNull = False Then Me.UI_Remark.Text = dr.RMA_Remark
        End If
    End Sub

    Private Sub QueryDataByDetail(ByVal iPageIndex As Integer)
        Dim oRMA As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable

        dtClientDetail = oRMA.QueryByClient(Session("_LanguageID").ToString().Trim(), Me.UI_lblPreviousPage_RMANO.Text)

        Call RMA_DataBind(dtClientDetail, iPageIndex)
    End Sub

    Private Sub RMA_DataBind(ByVal dtClientDetail As RmaDTO.tmpClientDetailDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtClientDetail)

        Session("_dtClientDetail") = dtClientDetail

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dtClientDetail.DefaultView
        Me.UI_dvRequest.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtClientDetail As RmaDTO.tmpClientDetailDataTable)
        Dim i As Integer = 0

        If dtClientDetail.Columns("SeqID") Is Nothing Then
            dtClientDetail.Columns.Add("SeqID")
            dtClientDetail.Columns.Add("Status")
            dtClientDetail.Columns.Add("WARRANTY")
            dtClientDetail.Columns.Add("FailureReason")
            dtClientDetail.Columns.Add("LaborCost")
            dtClientDetail.Columns.Add("MaterialCost")
            dtClientDetail.Columns.Add("TotalAmount")
        End If

        For i = 0 To dtClientDetail.Rows.Count - 1
            Dim dr As RmaDTO.tmpClientDetailRow = dtClientDetail.Rows(i)

            dtClientDetail.Rows(i)("SeqID") = i + 1
            dtClientDetail.Rows(i)("Status") = oCommon.ConvertToItemStatusText(Convert.ToInt16(dr.RMAD_STATUS), dr.RMAD_ID.Trim())

            If dtClientDetail.Rows(i)("RMAD_WARRANTY").ToString().Trim() = "" Then
                dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                Select Case dtClientDetail.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtClientDetail.Rows(i)("WARRANTY") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select

            Else
                dtClientDetail.Rows(i)("WARRANTY") = Convert.ToDateTime(dr.RMAD_WARRANTY).ToShortDateString()
            End If

            If dr.IsFARC_NAME2Null = False Then
                dtClientDetail.Rows(i)("FailureReason") = dr.FARC_NAME2.ToString().Trim()
            Else
                If dr.IsFARC_NAME1Null = False Then dtClientDetail.Rows(i)("FailureReason") = dr.FARC_NAME1.ToString().Trim()
            End If


            ' If dr.IsRMARSD_QUOTENull = False Then
            ' '幣別
            ' If dr.IsRMASQ_CURRENCYCODENull = False Then
            ' Dim sCurrencyCode As String = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "

            ' 'Labor Cost
            ' If dr.IsRMARSD_LABORCOSTNull = False Then dtClientDetail.Rows(i)("LaborCost") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_LABORCOST).ToString("N").Trim()
            ' 'Material Cost
            ' If dr.IsRMARSD_MATERIALCOSTNull = False Then dtClientDetail.Rows(i)("MaterialCost") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_MATERIALCOST).ToString("N").Trim()
            ' 'Total Amount
            ' dtClientDetail.Rows(i)("TotalAmount") = sCurrencyCode & Convert.ToDouble(dr.RMARSD_QUOTE).ToString("N").Trim()
            ' End If
            ' Else
            ' If dr.IsRMASQ_QUOTENull = False Then
            ' '幣別
            ' If dr.IsRMASQ_CURRENCYCODENull = False Then
            ' Dim sCurrencyCode As String = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "

            ' 'Labor Cost
            ' If dr.IsRMASQ_LABORCOSTNull = False Then dtClientDetail.Rows(i)("LaborCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_LABORCOST).ToString("N").Trim()
            ' 'Material Cost
            ' If dr.IsRMASQ_MATERIALCOSTNull = False Then dtClientDetail.Rows(i)("MaterialCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_MATERIALCOST).ToString("N").Trim()
            ' 'Total Amount
            ' dtClientDetail.Rows(i)("TotalAmount") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
            ' End If
            ' End If

            ' End If
            'MODI BY Angel On 20151221 因為ShipmentDetail的金額記錄不正確,所以改取RMASALE_Quoted的金額
            If dr.IsRMASQ_QUOTENull = False Then
                '幣別
                If dr.IsRMASQ_CURRENCYCODENull = False Then
                    Dim sCurrencyCode As String = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "

                    'Labor Cost
                    If dr.IsRMASQ_LABORCOSTNull = False Then dtClientDetail.Rows(i)("LaborCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_LABORCOST).ToString("N").Trim()
                    'Material Cost
                    If dr.IsRMASQ_MATERIALCOSTNull = False Then dtClientDetail.Rows(i)("MaterialCost") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_MATERIALCOST).ToString("N").Trim()
                    'Total Amount
                    dtClientDetail.Rows(i)("TotalAmount") = sCurrencyCode & Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
                End If
            End If

        Next

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA2", "038", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("RMA", "141", ctlLanguage.eumType.Tag)
            e.Row.Cells(11).Text = _oLanguage.getText("RMA", "140", ctlLanguage.eumType.Tag)

            e.Row.Cells(12).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            Dim UI_cmdDetail_Btn As Button = e.Row.FindControl("UI_cmdDetail")

            UI_cmdDetail_Btn.Text = _oLanguage.getText("RMA2", "043", ctlLanguage.eumType.Tag)

            If UI_RMADWARRANTY.Text.Trim() <> "" Then
                Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                Dim RMAD_CSTMP As DateTime = Convert.ToDateTime(UI_RMADCSTMP.Text.Trim())
                If RMAD_WARRANTY < RMAD_CSTMP Then
                    e.Row.Cells(4).ForeColor = Drawing.Color.Red
                End If
            End If
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.Cells(9).Text = "Requested" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "072", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Received" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "073", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Repair Quoted" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "075", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Sales Quoted" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Sales Confirmed" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Client Confirmed" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "099", ctlLanguage.eumType.Tag)
            End If


            If e.Row.Cells(9).Text = "Repaired" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "078", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Customer Confirmed" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "077", ctlLanguage.eumType.Tag)
            End If


            If e.Row.Cells(9).Text = "Shipped" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "079", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Closed" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "080", ctlLanguage.eumType.Tag)
            End If

            If e.Row.Cells(9).Text = "Canceled" Then
                e.Row.Cells(9).Text = _oLanguage.getText("RMA2", "081", ctlLanguage.eumType.Tag)
            End If




            Dim UI_RMANO As Label = e.Row.FindControl("UI_RMANO")
            Dim UI_SERIALNO As Label = e.Row.FindControl("UI_SERIALNO")
            Dim UI_Test_Report As Button = e.Row.FindControl("UI_Test_Report")

            UI_Test_Report.Text = _oLanguage.getText("RMA2", "943", ctlLanguage.eumType.Tag)

            Dim myctAddress As New ctAddress
            Dim myDataTable As DataTable = myctAddress.Select_Count_CheckingReport(UI_RMANO.Text.Trim(), UI_SERIALNO.Text.Trim())

            If myDataTable.Rows(0)("A") Is Nothing Then

            Else

                If myDataTable.Rows(0)("A").ToString().Trim() = "1" Then
                    UI_Test_Report.Visible = True
                Else
                    UI_Test_Report.Visible = False
                End If

            End If

            '目前沒上線
            UI_Test_Report.Visible = False


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

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtClientDetail") Is Nothing Then
            Dim dtClientDetail As RmaDTO.tmpClientDetailDataTable = Session("_dtClientDetail")
            Call RMA_DataBind(dtClientDetail, iPageIndex)
        Else
            Call QueryDataByDetail(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        If e.CommandName = "cmdDetail" Then
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Me.ucClientDetail.show(UI_RMADID.Text.Trim(), UI_RMANO.Text.Trim(), True)
        End If

        If e.CommandName = "Test_Report" Then
            row = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMADID As Label = row.FindControl("UI_SERIALNO")


            Dim url As String = "CheckingReportList.aspx?UI_RMANO=" & UI_RMANO.Text.Trim() & "&UI_lblSerialText=" & UI_RMADID.Text.Trim()

            Dim queryString As String = url
            Dim newWin As String = "window.open('" & queryString & "','_blank');"
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "ShowInfoPage", newWin, True)

        End If

    End Sub

End Class

Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports RMA_Model

Partial Class RMARepair_Edit
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")

    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")
    Dim _RepairOKEmail As String = ConfigurationSettings.AppSettings("RepairOKEmail")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim sSelectNo As String = ""
    Dim sSerialNo As String = ""

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Session("_RepairRart_Submit") = False
            Session("_dtRepairDetail") = Nothing
            Session("_PreviousPage") = Nothing
            Dim hsSelectID As New Hashtable
            Me.ViewState("hsSelectID") = hsSelectID
            Me.ViewState("hSerialNumber") = ""

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Me.UI_lblPreviousPage_RMADID.Text = UI_lblPreviousPage_RMADID.Text.Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()

                Dim oHashtable As System.Collections.Hashtable = Me.PreviousPage.getHistoryByList
                Me.ViewState("_HistoryKey") = oHashtable
                pnlVersion.Visible = False

                Call setControls()
                Call QueryDataByHead()

                Call QueryDataByDetail()
                Call QueryDataByStatusPoint()
                Call QueryData(0)
                Call QuerySDC()

                ViewState("hSerialNumber") = UI_lblSerialText.Text
                GetVersigonData(UI_lblSerialText.Text)

                Call setFlowCase()
            End If
        End If
    End Sub
#End Region
    Private Sub QuerySDC()
        Dim SDCGrid As DataGrid = New DataGrid()
        SDCGrid = Me.UcSDCViewG.FindControl("UcSDCViewG")
        Me.UcSDCViewG.show(UI_lblSerialText.Text.Trim(), Me.UI_dvRepairDetail.Width)

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Me.IsPostBack = True Then
            'Repair Bom 查回來後新增
            If Session("_RepairRart_Submit") = True Then
                Call Me.UI_cmdSearch_Click(Me.UI_cmdSearch, System.EventArgs.Empty)
                Me.UI_txtPartsNo.Text = ""
            End If
            Session("_RepairRart_Submit") = False
        End If

    End Sub

    Private Sub chkFlowCase01()
        Dim i As Integer = 0

        '判斷是否要執行 flow case 01
        'Dim arrRepairCenter() As String = Session("_RepairCenter").ToString().Trim().Split(",")
        'For i = 0 To arrRepairCenter.Length - 1
        '    If _RepairNo_flowCase01.Trim().IndexOf(arrRepairCenter(i).ToString().Trim()) <> -1 Then
        '        Me.UI_flowCase.Text = "01"
        '        Exit For
        '    End If
        'Next

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If Me.UI_lblCompNO.Text.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "01"
                Exit For
            End If
        Next


        '用客戶申請時表單的維修中心判斷是否要執行 flow case 02
        Dim arrRepairNo_flowCase02() As String = _RepairNo_flowCase02.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase02.Length - 1
            If Me.UI_lblCompNO.Text.Trim().IndexOf(arrRepairNo_flowCase02(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "02"
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase()
        'Manpower Hour
        Me.UI_lblManpower.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.UI_lblHour.Text = Me.UI_lblCurrencyCode.Text
        Me.UI_lblHour.Text = ""
        Me.UI_lblHour_Delimited.Text = ""
        Me.UI_txtManHour.Style("display") = "none"      'Service Charge 金額

        Me.UI_txtLABORPrice.Text = "1"      '人工每小時單價, 已用不到了, 預設是 1
        Me.UI_txtLABORPrice.Style("display") = "none"
        'Me.UI_lblLaborCost.Style("display") = "none"    '人工維修費用(Service Charge 金額 * 人工每小時單價)

        'Parts
        Me.UI_txtPartsTotal.Style("display") = "none"    'parts price

        'Total amount
        Me.UI_lblCurrencyCode1.Visible = False

        If Me.UI_flowCase.Text = "01" Then
            Me.uiTR_EstimatedAmount.Visible = False

            Me.UI_lblManpower.Visible = False
            Me.UI_lblManpower_Delimited.Visible = False

            Me.UI_lblLaborCost.Style("display") = "none"    '人工維修費用(Service Charge 金額 * 人工每小時單價)

            'Parts
            Me.UI_lblParts.Style("display") = "none"
            Me.uiLbl_Parts_Delimited.Style("display") = "none"
            Me.UI_lblPartsTotal.Style("display") = "none"    'parts price

            'Total amount
            Me.UI_lblTotalText.Style("display") = "none"
            Me.UI_lblTotalText_Delimited.Style("display") = "none"
            Me.UI_lblTotal.Style("display") = "none"         'Total amount
        End If


        If Me.UI_flowCase.Text = "02" Then
            Me.uiTR_EstimatedAmount.Visible = True

            Me.UI_lblManpower.Visible = True
            Me.UI_lblManpower_Delimited.Visible = True

            Me.UI_lblLaborCost.Style("display") = ""    '人工維修費用(Service Charge 金額 * 人工每小時單價)

            'Parts
            Me.UI_lblParts.Style("display") = ""
            Me.uiLbl_Parts_Delimited.Style("display") = ""
            Me.UI_lblPartsTotal.Style("display") = ""    'parts price

            'Total amount
            Me.UI_lblTotalText.Style("display") = ""
            Me.UI_lblTotalText_Delimited.Style("display") = ""
            Me.UI_lblTotal.Style("display") = ""         'Total amount
        End If

    End Sub

    Private Sub setControls()
        Dim sClientID As String = Me.UI_cmdSearch.ClientID & "," & Me.UI_cmdParts_Search.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID

        'Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        'oCommon.getDutyByDropDownList(Me.UI_cboDuty, sText)
        'oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)
        'oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)

        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
        ''Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
        ''Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
        ''Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)

        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(9).HeaderText = _oLanguage.getText("RMA", "071", ctlLanguage.eumType.Tag)
        Me.UI_lblProductTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        'Me.UI_cmdApply.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)
        Me.UI_cmdApply.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdVerApply.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "111", ctlLanguage.eumType.Tag)

        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        'Me.UI_lblDuty.Text = _oLanguage.getText("RMA", "051", ctlLanguage.eumType.Tag)
        'Me.UI_lblLaborHour.Text = _oLanguage.getText("RMA", "112", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborHour.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborHourText.Text = _oLanguage.getText("RMA", "057", ctlLanguage.eumType.Tag)
        Me.UI_lblQuote.Text = _oLanguage.getText("RMA", "113", ctlLanguage.eumType.Tag)
        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

        Me.UI_lblProductDesc.Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
        Me.UI_lblProblemDesc.Text = _oLanguage.getText("RMA", "025", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairDesc.Text = _oLanguage.getText("RMA", "053", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairMemo.Text = _oLanguage.getText("RMA", "054", ctlLanguage.eumType.Tag)


        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "082", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblPartsNo.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
        Me.UI_lblLocation.Text = _oLanguage.getText("RMA", "084", ctlLanguage.eumType.Tag)
        Me.UI_lblCurrency.Text = _oLanguage.getText("RMA", "085", ctlLanguage.eumType.Tag)

        Me.UI_lblManpower.Text = _oLanguage.getText("RMA", "086", ctlLanguage.eumType.Tag)
        Me.UI_lblParts.Text = _oLanguage.getText("RMA", "087", ctlLanguage.eumType.Tag)
        Me.UI_lblHour.Text = _oLanguage.getText("RMA", "057", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalText.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)
        Me.UI_lblStatusUpdate.Text = _oLanguage.getText("RMA", "089", ctlLanguage.eumType.Tag)

        Me.UI_lblReceived.Text = _oLanguage.getText("RMA", "090", ctlLanguage.eumType.Tag)
        Me.UI_lblRepair.Text = _oLanguage.getText("RMA", "091", ctlLanguage.eumType.Tag)
        Me.UI_lblSales.Text = _oLanguage.getText("RMA", "092", ctlLanguage.eumType.Tag)
        Me.UI_lblClient.Text = _oLanguage.getText("RMA", "093", ctlLanguage.eumType.Tag)
        Me.UI_lblRepaired.Text = _oLanguage.getText("RMA", "094", ctlLanguage.eumType.Tag)
        Me.UI_lblClose.Text = _oLanguage.getText("RMA", "095", ctlLanguage.eumType.Tag)
        Me.UI_lblCancel.Text = _oLanguage.getText("RMA", "041", ctlLanguage.eumType.Tag)
        Me.UI_lblApprover.Text = _oLanguage.getText("RMA", "096", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdParts_Search.Text = _oLanguage.getText("Common", "078", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdSendMail.Text = _oLanguage.getText("Common", "083", ctlLanguage.eumType.Command)


        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_txtManHour.Attributes.Add("onkeyup", "cal_TotalAMT()")


        Me.UI_lblQuickSearch.Text = _oLanguage.getText("RMA", "069", ctlLanguage.eumType.Tag)
        Me.UI_lblQcSn.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.btnQuickSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.btnQA01.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA02.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA03.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA04.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA05.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA06.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA07.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA08.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA09.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA10.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA11.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQA12.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)

        Me.btnQB01.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQB02.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQB03.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQB04.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQB05.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)
        Me.btnQB06.Text = _oLanguage.getText("RMA", "324", ctlLanguage.eumType.Tag)

        Me.lblVerChange.Text = _oLanguage.getText("RMA", "301", ctlLanguage.eumType.Tag)
        Me.lblColumn.Text = _oLanguage.getText("RMA", "302", ctlLanguage.eumType.Tag)
        Me.lblVerName.Text = _oLanguage.getText("RMA", "303", ctlLanguage.eumType.Tag)
        Me.lblVerBefore.Text = _oLanguage.getText("RMA", "304", ctlLanguage.eumType.Tag)
        Me.lblVerAfter.Text = _oLanguage.getText("RMA", "305", ctlLanguage.eumType.Tag)

        Me.lblC01.Text = _oLanguage.getText("RMA", "306", ctlLanguage.eumType.Tag)
        Me.lblC02.Text = _oLanguage.getText("RMA", "307", ctlLanguage.eumType.Tag)
        Me.lblC03.Text = _oLanguage.getText("RMA", "308", ctlLanguage.eumType.Tag)
        Me.lblC04.Text = _oLanguage.getText("RMA", "309", ctlLanguage.eumType.Tag)
        Me.lblC05.Text = _oLanguage.getText("RMA", "310", ctlLanguage.eumType.Tag)
        Me.lblC06.Text = _oLanguage.getText("RMA", "311", ctlLanguage.eumType.Tag)
        Me.lblC07.Text = _oLanguage.getText("RMA", "312", ctlLanguage.eumType.Tag)
        Me.lblC08.Text = _oLanguage.getText("RMA", "313", ctlLanguage.eumType.Tag)
        Me.lblC09.Text = _oLanguage.getText("RMA", "314", ctlLanguage.eumType.Tag)
        Me.lblC10.Text = _oLanguage.getText("RMA", "315", ctlLanguage.eumType.Tag)
        Me.lblC11.Text = _oLanguage.getText("RMA", "316", ctlLanguage.eumType.Tag)
        Me.lblC12.Text = _oLanguage.getText("RMA", "317", ctlLanguage.eumType.Tag)
        Me.lblC13.Text = _oLanguage.getText("RMA", "318", ctlLanguage.eumType.Tag)
        Me.lblC14.Text = _oLanguage.getText("RMA", "319", ctlLanguage.eumType.Tag)
        Me.lblC15.Text = _oLanguage.getText("RMA", "320", ctlLanguage.eumType.Tag)
        Me.lblC16.Text = _oLanguage.getText("RMA", "321", ctlLanguage.eumType.Tag)
        Me.lblC17.Text = _oLanguage.getText("RMA", "322", ctlLanguage.eumType.Tag)
        Me.lblC18.Text = _oLanguage.getText("RMA", "323", ctlLanguage.eumType.Tag)
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfvNewPart".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "107", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvNewSerial".ToLower()
                'sErrorMessage = _oLanguage.getText("RMA", "108", ctlLanguage.eumType.Validator)
                'oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvQty".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "109", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvPrice".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "110", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    ''' <summary>
    ''' 取得 指派的維修中心 相關的幣別資料
    ''' </summary>
    ''' <param name="COMP_NO">公司代碼</param>
    ''' <remarks></remarks>
    Private Sub QueryByCompany_Currency(ByVal COMP_NO As String)
        Dim blnFlag_Cal As Boolean = False
        Dim iTotalManAmt As Decimal = 0
        Dim iMaterial As Decimal = 0

        Dim oCompany As New ctlCompany
        Dim dtCurrency As New CompanyDTO.vwCompany_CurrencyDataTable

        dtCurrency = oCompany.QueryByCurrency(COMP_NO)
        If dtCurrency.Rows.Count > 0 Then
            Dim dr As CompanyDTO.vwCompany_CurrencyRow = dtCurrency.Rows(0)
            Me.UI_lblCurrencyCode.Text = dr.CURRENCY_CODE.Trim()
            Me.UI_lblCurrencyRate.Text = dr.CURRENCY_RATE.ToString()

            Me.UI_lblCurrencyCode1.Text = dr.CURRENCY_CODE.Trim()
        End If

    End Sub

    ''' <summary>
    ''' 取得 原本維修中心 相關的幣別資料
    ''' </summary>
    ''' <param name="COMP_NO">公司代碼</param>
    ''' <remarks></remarks>
    Private Sub QueryByEssCompany_Currency(ByVal COMP_NO As String)
        Dim blnFlag_Cal As Boolean = False
        Dim iTotalManAmt As Decimal = 0
        Dim iMaterial As Decimal = 0

        Dim oCompany As New ctlCompany
        Dim dtCurrency As New CompanyDTO.vwCompany_CurrencyDataTable

        dtCurrency = oCompany.QueryByCurrency(COMP_NO)
        If dtCurrency.Rows.Count > 0 Then
            Dim dr As CompanyDTO.vwCompany_CurrencyRow = dtCurrency.Rows(0)
            Me.UI_lblEssCurrencyCode.Text = dr.CURRENCY_CODE.Trim()
            Me.UI_lblEssCurrencyRate.Text = dr.CURRENCY_RATE.ToString()
        End If

    End Sub

    Private Sub QueryDataByStatusPoint()
        Dim oRMAStatus As New ctlRMA.RMAStatus
        Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable

        Me.UI_lblReceivedUser.Text = ""
        Me.UI_lblReceivedDate.Text = ""
        Me.UI_lblRepairQuotedUser.Text = ""
        Me.UI_lblRepairQuotedDate.Text = ""
        Me.UI_lblSalesUser.Text = ""
        Me.UI_lblSalesDate.Text = ""
        Me.UI_lblClientUser.Text = ""
        Me.UI_lblClientDate.Text = ""
        Me.UI_lblRepairedUser.Text = ""
        Me.UI_lblRepairedDate.Text = ""
        Me.UI_lblCloseUser.Text = ""
        Me.UI_lblCloseDate.Text = ""
        Me.UI_lblCancelUser.Text = ""
        Me.UI_lblCancelDate.Text = ""

        dtStatusPoint = oRMAStatus.QueryPointByDetail(Me.UI_lblPreviousPage_RMADID.Text)
        If dtStatusPoint.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)

            If dr.IsRECEIVED_ADNull = False Then Me.UI_lblReceivedUser.Text = dr.RECEIVED_AD.Trim()
            If dr.IsRECEIVED_DATENull = False Then Me.UI_lblReceivedDate.Text = dr.RECEIVED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsREPAIRQUOTED_ADNull = False Then Me.UI_lblRepairQuotedUser.Text = dr.REPAIRQUOTED_AD.Trim()
            If dr.IsREPAIRQUOTED_DATENull = False Then Me.UI_lblRepairQuotedDate.Text = dr.REPAIRQUOTED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsSALES_ADNull = False Then Me.UI_lblSalesUser.Text = dr.SALES_AD.Trim()
            If dr.IsSALES_DATENull = False Then Me.UI_lblSalesDate.Text = dr.SALES_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLIENT_CONFIRMNull = False Then
                '1.客戶自行確認, 2.業務帶客戶確認
                If dr.CLIENT_CONFIRM = 1 Then
                    If dr.IsCLIENT_ADNull = False Then Me.UI_lblClientUser.Text = dr.CLIENT_AD.Trim()
                    If dr.IsCLIENT_DATENull = False Then Me.UI_lblClientDate.Text = dr.CLIENT_DATE.ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    If dr.IsSALES_ADNull = False Then Me.UI_lblClientUser.Text = dr.SALES_AD.Trim()
                    If dr.IsSALES_DATENull = False Then Me.UI_lblClientDate.Text = dr.SALES_DATE.ToString("yyyy/MM/dd HH:mm:ss")
                End If
            End If

            If dr.IsREPAIRED_ADNull = False Then Me.UI_lblRepairedUser.Text = dr.REPAIRED_AD.Trim()
            If dr.IsREPAIRED_DATENull = False Then Me.UI_lblRepairedDate.Text = dr.REPAIRED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLOSE_ADNull = False Then Me.UI_lblCloseUser.Text = dr.CLOSE_AD.Trim()
            If dr.IsCLOSE_DATENull = False Then Me.UI_lblCloseDate.Text = dr.CLOSE_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCANCEL_ADNull = False Then Me.UI_lblCancelUser.Text = dr.CANCEL_AD.Trim()
            If dr.IsCANCEL_DATENull = False Then Me.UI_lblCancelDate.Text = dr.CANCEL_DATE.ToString("yyyy/MM/dd HH:mm:ss")

        End If

    End Sub

    Private Sub QueryDataByHead()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        Me.UI_lblPartsTotal.Text = ""
        Me.UI_txtPartsTotal.Text = ""
        Me.UI_lblTotal.Text = ""
        Me.UI_lblProductDescText.Text = ""

        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailureReasons As New FailureDTO.vwFailureReasonsDataTable

        Dim oRepairQuoting As New ctlRMA.Repair_Quoting
        Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable

        dtRepairQuoting = oRepairQuoting.Query(Me.UI_lblPreviousPage_RMADID.Text, "")
        If dtRepairQuoting.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_QuotingRow = dtRepairQuoting.Rows(0)

            Me.UI_RMANO.Text = dr.RMA_NO.Trim()

            UI_lblRMAID.Text = dr.RMA_ID.Trim()
            Dim RMA_COMPNO As String = dr.RMA_COMPNO.Trim()
            Dim RMAR_COMPNO As String = ""
            If dr.IsRMAR_COMPNONull = False Then RMAR_COMPNO = dr.RMAR_COMPNO.Trim()

            Me.UI_lblRMACUNO.Text = dr.RMA_CUNO.ToString().Trim()
            Me.UI_lblRMAACCOUNTID.Text = dr.RMA_ACCOUNTID.ToString().Trim()

            Dim oExport As New ctlRMA.Export
            Dim sModelNo As String = oExport.getMModelNo(dr.RMAD_MODELNO.Trim(), RMA_COMPNO, Me.UI_lblRMAACCOUNTID.Text)

            'If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelNoText.Text = dr.RMAD_MODELNO.Trim()
            If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelNoText.Text = sModelNo

            If dr.IsRMAD_SERIALNONull = False Then
                Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.Trim()
                UI_lblShowSerial.Text = dr.RMAD_SERIALNO.Trim()
            End If
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                UI_lblShowSerial.Text = dr.RMAD_PARTSN.Trim()
            End If
            Me.UI_lblCustomerText.Text = dr.CU_NAME.Trim()


            '維修報價工時
            If dr.IsRMARQ_LABORHOURNull = False Then
                Me.UI_lblLaborHourvalue.Text = dr.RMARQ_LABORHOUR * dr.RMARQ_LABORPRICE
                'Me.UI_lblLaborHourvalue.Text = dr.RMARQ_ASSIGLABORCOST
            End If

            If Session("_RepairCenter").ToString().IndexOf(RMAR_COMPNO) <> -1 Then
                '更入系統維修中心 跟 被指派維修中心 一樣時
                If dr.IsRMAR_REPAIR_ISFILLNull = False And dr.IsRMARQ_QUOTENull = False Then
                    If dr.RMAR_REPAIR_ISFILL = 1 Then
                        Me.UI_lblQuoteCode.Text = dr.RMARQ_CURRENCYCODE.Trim()
                        Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_QUOTE).ToString("N")
                    End If
                End If

            Else
                '更入系統維修中心 跟 被指派維修中心 不一樣時
                If dr.IsRMAR_REPAIR_ISFILLNull = False And dr.IsRMARQ_ASSIGEQUOTENull = False Then
                    If dr.RMAR_REPAIR_ISFILL = 1 Then
                        Me.UI_lblQuoteCode.Text = dr.RMARQ_ASSIGECURRENCYCODE.Trim()
                        Me.UI_lblQuoteText.Text = Convert.ToDouble(dr.RMARQ_ASSIGEQUOTE).ToString("N")
                    End If
                End If
            End If

            '不良原因敘述
            If dr.IsRMAR_FARCNONull = False And dr.IsRMAR_FARNONull = False Then
                dtFailureReasons = oFailure.QueryByFailure(Session("_LanguageID"), dr.RMAR_FARCNO, dr.RMAR_FARNO)
                If dtFailureReasons.Rows.Count > 0 Then
                    Me.UI_lblFARCNO.Text = dr.RMAR_FARCNO.ToString().Trim()
                    Me.UI_lblFARNO.Text = dr.RMAR_FARNO.ToString().Trim()
                    Me.UI_lblFailureText.Text = dtFailureReasons.Rows(0)("FAR_REASON").ToString.Trim()
                End If

            Else
                If dr.IsRMAD_FARFARCNONull = False And dr.IsRMAD_FARNONull = False Then
                    dtFailureReasons = oFailure.QueryByFailure(Session("_LanguageID"), dr.RMAD_FARFARCNO, dr.RMAD_FARNO)
                    If dtFailureReasons.Rows.Count > 0 Then
                        Me.UI_lblFARCNO.Text = dr.RMAD_FARFARCNO.ToString().Trim()
                        Me.UI_lblFARNO.Text = dr.RMAD_FARNO.ToString().Trim()
                        Me.UI_lblFailureText.Text = dtFailureReasons.Rows(0)("FAR_REASON").ToString.Trim()
                    End If
                End If
            End If


            If dr.IsRMAD_PRODUCTDESCNull = False Then Me.UI_lblProductDescText.Text = dr.RMAD_PRODUCTDESC.Trim()

            'Problem Description
            If dr.IsRMAR_PROBLEMDESCNull = False Then
                Me.UI_lblProblemDescText.Text = dr.RMAR_PROBLEMDESC.Trim()
            Else
                If dr.IsRMAD_PROBLEMDESCNull = False Then
                    Me.UI_lblProblemDescText.Text = dr.RMAD_PROBLEMDESC.Trim()
                End If
            End If


            Me.UI_txtRepairDesc.Text = ""
            Me.UI_txtRepairMemo.Text = ""
            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_txtRepairDesc.Text = dr.RMAR_REPAIRDESC.Trim()
            If dr.IsRMAR_REPAIRMEMONull = False Then Me.UI_txtRepairMemo.Text = dr.RMAR_REPAIRMEMO.Trim()


            '幣別
            Dim COMPNO As String = dr.RMA_COMPNO
            Me.UI_lblEssCompNO.Text = dr.RMA_COMPNO.Trim()

            If dr.IsRMAR_COMPNONull = False Then
                COMPNO = dr.RMAR_COMPNO
            End If

            Me.UI_lblCompNO.Text = COMPNO                               '紀錄是哪各維修中心

            Call chkFlowCase01()
            Call QueryByCompany_Currency(COMPNO)                        '取得 指派的維修中心 相關的幣別資料
            Call QueryByEssCompany_Currency(Me.UI_lblEssCompNO.Text)    '取得 原本維修中心 相關的幣別資料

            '取得維修中心 工時單價
            If Me.UI_flowCase.Text = "01" Or Me.UI_flowCase.Text = "02" Then
                Me.UI_txtLABORPrice.Text = "1"
            Else
                Dim oCompany As New ctlCompany
                Me.UI_txtLABORPrice.Text = oCompany.getLaborCost(COMPNO)
            End If

        End If


        '================================================================================================================================================
        '取得維修單頭資料
        '================================================================================================================================================
        Dim oRepair As New ctlRMA.Repair
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable

        dtRMARepair = oRepair.QueryByRepair(Me.UI_lblPreviousPage_RMADID.Text)
        If dtRMARepair.Rows.Count > 0 Then
            Me.UI_txtManHour.Text = dtRMARepair.Rows(0)("RMAR_LABORHOUR").ToString().Trim()

            If dtRMARepair.Rows(0)("RMAR_LABORPRICE").ToString().Trim() <> "" Then
                Me.UI_txtLABORPrice.Text = dtRMARepair.Rows(0)("RMAR_LABORPRICE").ToString().Trim()
            End If

            Me.UI_lblLaborCost.Text = dtRMARepair.Rows(0)("RMAR_LABORCOST").ToString().Trim()
        End If

    End Sub

    Private Sub QueryDataByDetail()
        Dim i As Integer = 0
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable

        dtRepairDetail = oRepair.QueryByDetail(Me.UI_lblPreviousPage_RMADID.Text)
        If dtRepairDetail.Count = 0 Then
            Dim oRepairQuoted As New ctlRMA.Repair_Quoting
            Dim dtRepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
            Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

            dtRepairQuoted = oRepairQuoted.QueryByRepairQuoted(Me.UI_lblPreviousPage_RMADID.Text)
            If dtRepairQuoted.Rows.Count > 0 Then
                Dim dr As RmaDTO.RMARepair_QuotedRow = dtRepairQuoted.Rows(0)
                Me.UI_txtManHour.Text = Convert.ToDecimal(dr.RMARQ_LABORHOUR)
            End If

            dtRepairQuotedDetail = oRepairQuoted.QueryByRepairQuotedDetail(Me.UI_lblPreviousPage_RMADID.Text)
            For i = 0 To dtRepairQuotedDetail.Rows.Count - 1
                Dim item As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuotedDetail.Rows(i)

                Dim dr As RmaDTO.RMARepair_DetailRow = dtRepairDetail.NewRMARepair_DetailRow

                dr.RMARED_ID = item.RMARQD_ID
                dr.RMARED_RMADID = item.RMARQD_RMADID
                dr.RMARED_NPARTNO = item.RMARQD_NPARTNO

                If item.IsRMARQD_NSERIALNONull = False Then dr.RMARED_NSERIALNO = item.RMARQD_NSERIALNO
                If item.IsRMARQD_NWARRANTYNull = False Then dr.RMARED_NWARRANTY = item.RMARQD_NWARRANTY
                If item.IsRMARQD_OPARTNONull = False Then dr.RMARED_OPARTNO = item.RMARQD_OPARTNO
                If item.IsRMARQD_OSERIALNONull = False Then dr.RMARED_OSERIALNO = item.RMARQD_OSERIALNO
                If item.IsRMARQD_OWARRANTYNull = False Then dr.RMARED_OWARRANTY = item.RMARQD_OWARRANTY
                If item.IsRMARQD_DESCNull = False Then dr.RMARED_DESC = item.RMARQD_DESC
                If item.IsRMARQD_LOCATIONNull = False Then dr.RMARED_LOCATION = item.RMARQD_LOCATION

                dr.RMARED_IMPROPERUSAGE = item.RMARQD_IMPROPERUSAGE
                If item.IsRMARQD_DEFECTIVENull = False Then dr.RMARED_DEFECTIVE = item.RMARQD_DEFECTIVE

                dr.RMARED_QTY = item.RMARQD_QTY
                dr.RMARED_MATERIALCOST = item.RMARQD_MATERIALCOST
                dr.RMARED_PRICE = item.RMARQD_PRICE
                dr.RMARED_CURRENCYCODE = item.RMARQD_CURRENCYCODE
                dr.RMARED_CURRENCYRATE = item.RMARQD_CURRENCYRATE
                dr.RMARED_ASSIGEPRICE = item.RMARQD_ASSIGEPRICE
                dr.RMARED_ASSIGECURRENCYCODE = item.RMARQD_ASSIGECURRENCYCODE
                dr.RMARED_ASSIGECURRENCYRATE = item.RMARQD_ASSIGECURRENCYRATE
                dr.RMARED_AD = item.RMARQD_AD
                dr.RMARED_ADNAME = item.RMARQD_ADNAME
                dr.RMARED_CSTMP = item.RMARQD_CSTMP
                dr.RMARED_LUAD = item.RMARQD_LUAD
                dr.RMARED_LUADNAME = item.RMARQD_LUADNAME
                dr.RMARED_LUSTMP = item.RMARQD_LUSTMP
                dr.RMARED_MARK = item.RMARQD_MARK

                dr.RMARED_WAIVE = item.RMARQD_WAIVE
                dr.RMARED_OPTION = item.RMARQD_OPTIONCLIENT     '客戶可選擇是否要修-->1.不修, 2.要修
                dr.RMARED_ISSOURCE = 2   '來源: 1.自行建立, 2.RMAREPAIR_QUOTED_DETAIL 匯入

                dtRepairDetail.AddRMARepair_DetailRow(dr)

            Next

        End If

        Call RepairDetail_DataBind(dtRepairDetail, 0)
    End Sub

    Private Sub RepairDetail_DataBind(ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairDetail") = dtRepairDetail
        Call CalTotalAmt()

        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARED_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()

        Call setFlowCase_UI_dvRepairDetail()
    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面, 維修零件控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase_UI_dvRepairDetail()
        Dim i As Integer = 0

        For i = 0 To UI_dvRepairDetail.Controls.Count - 1
            'oTableHeader
            Dim oTableHeader As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableHeader")
            If IsNothing(oTableHeader) = False Then
                oTableHeader.Rows(0).Cells(1).Visible = False   'Waive filed
                oTableHeader.Rows(0).Cells(2).Visible = False    'Option filed

                If Me.UI_flowCase.Text = "01" Then
                    oTableHeader.Rows(0).Cells(1).Visible = False   'Waive filed
                    oTableHeader.Rows(0).Cells(2).Visible = False    'Option filed
                    oTableHeader.Rows(0).Cells(10).Visible = False    'price filed
                End If

                If Me.UI_flowCase.Text = "02" Then
                    oTableHeader.Rows(0).Cells(1).Visible = False   'Waive filed
                    oTableHeader.Rows(0).Cells(2).Visible = False    'Option filed
                    oTableHeader.Rows(0).Cells(10).Visible = True    'price filed
                End If
            End If

            'oTableRow
            Dim oTableRow As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableRow")
            If IsNothing(oTableRow) = False Then
                oTableRow.Rows(0).Cells(1).Visible = False   'Waive filed
                oTableRow.Rows(0).Cells(2).Visible = False    'Option filed

                If Me.UI_flowCase.Text = "01" Then
                    Dim txtDescription As TextBox = oTableRow.Rows(0).FindControl("txtDescription")
                    'txtDescription.Enabled = False
                    oTableRow.Rows(0).Cells(1).Visible = False   'Waive filed
                    oTableRow.Rows(0).Cells(2).Visible = False    'Option filed
                    oTableRow.Rows(0).Cells(10).Visible = False    'price filed
                End If

                If Me.UI_flowCase.Text = "02" Then
                    Dim txtDescription As TextBox = oTableRow.Rows(0).FindControl("txtDescription")
                    'txtDescription.Enabled = False
                    oTableRow.Rows(0).Cells(1).Visible = False   'Waive filed
                    oTableRow.Rows(0).Cells(2).Visible = False    'Option filed
                    oTableRow.Rows(0).Cells(10).Visible = True    'price filed
                End If
            End If
        Next
    End Sub

    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblWaive As Label = e.Item.FindControl("lblWaive")
            Dim lblOption As Label = e.Item.FindControl("lblOption")

            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHLocation As Label = e.Item.FindControl("lblHLocation")
            Dim lblHImproper As Label = e.Item.FindControl("lblHImproper")
            Dim lblHReason As Label = e.Item.FindControl("lblHReason")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblHPrice As Label = e.Item.FindControl("lblHPrice")
            Dim lblHDel As Label = e.Item.FindControl("lblHDel")

            lblWaive.Text = _oLanguage.getText("RMA", "405", ctlLanguage.eumType.Tag)
            lblOption.Text = _oLanguage.getText("RMA", "406", ctlLanguage.eumType.Tag)
            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "098", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHLocation.Text = _oLanguage.getText("RMA", "100", ctlLanguage.eumType.Tag)
            lblHImproper.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
            lblHReason.Text = _oLanguage.getText("RMA", "102", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblHPrice.Text = _oLanguage.getText("RMA", "104", ctlLanguage.eumType.Tag)
            lblHDel.Text = _oLanguage.getText("RMA", "017", ctlLanguage.eumType.Tag)

            'Me.UI_lblPartsTotal.Text = "0"
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chhWaive As CheckBox = e.Item.FindControl("chhWaive")
            Dim UI_lblWaive As Label = e.Item.FindControl("UI_lblWaive")
            chhWaive.Visible = False
            If chhWaive.Checked = True Then
                UI_lblWaive.Text = "V"
            End If

            '客戶可選擇是否要修-->1.不修, 2.要修
            Dim UI_RMARED_OPTION As Label = e.Item.FindControl("UI_RMARED_OPTION")
            Dim chkOption As CheckBox = e.Item.FindControl("chkOption")
            Dim UI_lblOption As Label = e.Item.FindControl("UI_lblOption")

            chkOption.Visible = False
            chkOption.Checked = False
            If UI_RMARED_OPTION.Text.Trim() = "2" Then
                chkOption.Checked = True
            End If
            If chkOption.Checked = True Then
                UI_lblOption.Text = "V"
            End If


            Dim lblNew As Label = e.Item.FindControl("lblNew")
            Dim lblOld As Label = e.Item.FindControl("lblOld")
            lblNew.Text = _oLanguage.getText("RMA", "105", ctlLanguage.eumType.Tag) + "&nbsp;&nbsp;"
            lblOld.Text = _oLanguage.getText("RMA", "106", ctlLanguage.eumType.Tag) + "&nbsp;&nbsp;&nbsp;&nbsp;"

            Dim lblIMPROPERUSAGE As Label = e.Item.FindControl("lblIMPROPERUSAGE")
            Dim sImproper As DropDownList = e.Item.FindControl("cboImproper")
            sImproper.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            sImproper.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            sImproper.SelectedValue = lblIMPROPERUSAGE.Text.Trim()


            Dim UI_cboDefective As DropDownList = e.Item.FindControl("UI_cboDefective")
            Dim lblDEFECTIVE As Label = e.Item.FindControl("lblDEFECTIVE")
            oCommon.getDefectiveByDropDownList(Session("_LanguageID"), UI_cboDefective)
            UI_cboDefective.SelectedValue = lblDEFECTIVE.Text.Trim()



            '==========================================================================================================================================================
            '計算金額
            '==========================================================================================================================================================
            Dim txtQty As TextBox = e.Item.FindControl("txtQty")
            txtQty.Attributes.Add("onkeyup", "cal_subTotalAMT()")


            '==========================================================================================================================================================
            '檢核機制
            '==========================================================================================================================================================
            Dim rfvNewPart As RequiredFieldValidator = e.Item.FindControl("rfvNewPart")
            Dim rfvNewSerial As RequiredFieldValidator = e.Item.FindControl("rfvNewSerial")
            Dim rvQty As RangeValidator = e.Item.FindControl("rvQty")
            'Dim rvPrice As RangeValidator = e.Item.FindControl("rvPrice")
            Dim txtNewPart As TextBox = e.Item.FindControl("txtNewPart")
            Dim txtNewSerial As TextBox = e.Item.FindControl("txtNewSerial")

            rfvNewPart.ControlToValidate = txtNewPart.ID
            'rfvNewSerial.ControlToValidate = txtNewSerial.ID
            rvQty.ControlToValidate = txtQty.ID

            Call setValidationMessage(rfvNewPart)
            'Call setValidationMessage(rfvNewSerial)
            Call setValidationMessage(rvQty)
            'Call setValidationMessage(rvPrice)


            Dim txtDescription As TextBox = e.Item.FindControl("txtDescription")
            Dim txtLocation As TextBox = e.Item.FindControl("txtLocation")
            Dim cboImproper As DropDownList = e.Item.FindControl("cboImproper")

            '來源: 1.自行建立, 2.RMAREPAIR_QUOTED_DETAIL 匯入
            Dim UI_RMARED_ISSOURCE As TextBox = e.Item.FindControl("UI_RMARED_ISSOURCE")
            UI_RMARED_ISSOURCE.Style("display") = "none"
            If UI_RMARED_ISSOURCE.Text.Trim() = "1" Then
                UI_lblOption.Visible = False
            End If

            If UI_RMARED_ISSOURCE.Text.Trim() = "2" Then
                Dim oTable As System.Web.UI.WebControls.Table = e.Item.Controls(1)

                'new Part No
                Dim UI_RMARED_NPARTNO As Label = e.Item.FindControl("UI_RMARED_NPARTNO")
                txtNewPart.Visible = False
                UI_RMARED_NPARTNO.Visible = True

                'old Part No
                Dim txtOldPart As TextBox = e.Item.FindControl("txtOldPart")
                Dim UI_RMARED_OPARTNO As Label = e.Item.FindControl("UI_RMARED_OPARTNO")
                txtOldPart.Visible = False
                UI_RMARED_OPARTNO.Visible = True
                If UI_RMARED_OPARTNO.Text.Trim() = "" Then
                    UI_RMARED_OPARTNO.Text = "&nbsp;"
                End If

                'new Serial No
                Dim UI_RMARED_NSERIALNO As Label = e.Item.FindControl("UI_RMARED_NSERIALNO")

                'old Serial No
                Dim txtOldSerial As TextBox = e.Item.FindControl("txtOldSerial")
                Dim UI_RMARED_OSERIALNO As Label = e.Item.FindControl("UI_RMARED_OSERIALNO")


                'Description
                Dim UI_RMARED_DESC As Label = e.Item.FindControl("UI_RMARED_DESC")
                txtDescription.Visible = False
                UI_RMARED_DESC.Visible = True

                'SMT Location
                Dim UI_RMARED_LOCATION As Label = e.Item.FindControl("UI_RMARED_LOCATION")
                'txtLocation.Visible = False
                'UI_RMARED_LOCATION.Visible = True

                'Improper Usage
                Dim UI_RMARED_IMPROPERUSAGE As Label = e.Item.FindControl("UI_RMARED_IMPROPERUSAGE")
                cboImproper.Visible = False
                UI_RMARED_IMPROPERUSAGE.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                If lblIMPROPERUSAGE.Text.Trim() = "1" Then
                    UI_RMARED_IMPROPERUSAGE.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End If
                UI_RMARED_IMPROPERUSAGE.Visible = True

                'Defect Reason
                Dim UI_Defective As Label = e.Item.FindControl("UI_Defective")
                'UI_cboDefective.Visible = False
                'UI_Defective.Text = UI_cboDefective.Items(UI_cboDefective.SelectedIndex).Text
                'UI_Defective.Visible = True

                'QTY
                Dim UI_RMARED_QTY As Label = e.Item.FindControl("UI_RMARED_QTY")
                txtQty.Style("display") = "none"
                UI_RMARED_QTY.Visible = True

                'delete
                Dim imgDel As ImageButton = e.Item.FindControl("imgDel")
                imgDel.Visible = True


                '客戶可選擇是否要修-->1.不修, 2.要修
                '灰色表示客人取消的
                If UI_RMARED_OPTION.Text.Trim() = "1" Then
                    oTable.Rows(0).BackColor = Drawing.Color.Gray
                    oTable.Rows(1).BackColor = Drawing.Color.Gray

                    lblNew.Visible = False
                    lblOld.Visible = False

                    'new Serial No
                    txtNewSerial.Visible = False
                    UI_RMARED_NSERIALNO.Visible = True

                    'old Serial No
                    txtOldSerial.Visible = False
                    UI_RMARED_OSERIALNO.Visible = True

                    'SMT Location
                    txtLocation.Visible = False
                    UI_RMARED_LOCATION.Visible = True

                    'Defect Reason
                    UI_cboDefective.Visible = False
                    UI_Defective.Text = UI_cboDefective.Items(UI_cboDefective.SelectedIndex).Text
                    UI_Defective.Visible = True

                    imgDel.Visible = False
                End If
            End If

            'MaterialCost
            Dim UI_txtMaterialCost As TextBox = e.Item.FindControl("UI_txtMaterialCost")
            UI_txtMaterialCost.Style("display") = "none"

            'Price
            Dim UI_txtRMARED_PRICE As TextBox = e.Item.FindControl("UI_txtRMARED_PRICE")
            UI_txtRMARED_PRICE.Style("display") = "none"

        End If
    End Sub

    Protected Sub UI_dvRepairDetail_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles UI_dvRepairDetail.ItemCommand
        If e.CommandName = "cmdDel" Then
            Dim lblRMAREDID As Label = e.Item.FindControl("lblRMAREDID")

            Call Delete(lblRMAREDID.Text.ToString())
        End If
    End Sub

    ''' <summary>
    ''' 計算 維修相關 金額
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalTotalAmt()
        Dim blnFlag_Cal As Boolean = False
        Dim i As Integer = 0
        Dim iTotalParts As Double = 0
        Dim iTotalAmt As Double = 0

        Dim dtRepairDetail As RmaDTO.RMARepair_DetailDataTable = Session("_dtRepairDetail")
        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView

        dvRepairDetail.RowFilter = "RMARED_MARK=0"
        For i = 0 To dvRepairDetail.Count - 1
            Dim dr As RmaDTO.RMARepair_DetailRow = dvRepairDetail(i).Row()

            '來源: 1.自行建立, 2.RMAREPAIR_QUOTED_DETAIL 匯入
            If dr.RMARED_ISSOURCE = 2 Then
                iTotalParts = iTotalParts + dr.RMARED_PRICE
            Else
                iTotalParts = iTotalParts + (dr.RMARED_QTY * dr.RMARED_MATERIALCOST)
            End If
        Next

        'Man Hour 已改 Service Charge
        Dim iTotalManAmt As Double = 0
        If Me.UI_txtManHour.Text.Trim() <> "" And IsNumeric(Me.UI_txtManHour.Text.Trim()) = True Then
            'Man Total amt
            iTotalManAmt = Convert.ToDouble(Me.UI_txtManHour.Text.Trim()) * Convert.ToDouble(Me.UI_txtLABORPrice.Text)

            iTotalManAmt = Math.Round(iTotalManAmt, 2)
            Me.UI_lblLaborCost.Text = iTotalManAmt.ToString()
            blnFlag_Cal = True
        End If

        Me.UI_lblPartsTotal.Text = iTotalParts
        Me.UI_txtPartsTotal.Text = iTotalParts

        iTotalAmt = iTotalManAmt + iTotalParts
        iTotalAmt = Math.Round(iTotalAmt, 2)
        Me.UI_lblTotal.Text = iTotalAmt

        dvRepairDetail.RowFilter = ""
    End Sub

    ''' <summary>
    ''' 將畫面DataList的值儲存至暫存Table(_dtRepairDetail)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Keep_RepairDetail_Data()
        Dim i As Integer = 0
        Dim dtRepairDetail As RmaDTO.RMARepair_DetailDataTable = Session("_dtRepairDetail")
        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView

        For i = 0 To Me.UI_dvRepairDetail.Items.Count - 1
            Dim lblRMAREDID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMAREDID")
            Dim txtNewPart As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtNewPart")
            Dim txtNewSerial As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtNewSerial")
            Dim txtOldPart As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtOldPart")
            Dim txtOldSerial As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtOldSerial")
            Dim txtDescription As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtDescription")
            Dim txtLocation As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtLocation")
            Dim cboImproper As DropDownList = Me.UI_dvRepairDetail.Items(i).FindControl("cboImproper")
            Dim cboDefective As DropDownList = Me.UI_dvRepairDetail.Items(i).FindControl("UI_cboDefective")
            Dim txtQty As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("txtQty")
            Dim UI_txtMaterialCost As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_txtMaterialCost")
            Dim UI_txtRMARED_PRICE As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_txtRMARED_PRICE")


            Dim UI_RMARED_WAIVE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARED_WAIVE")
            Dim UI_RMARED_OPTION As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARED_OPTION")
            Dim UI_RMARED_ISSOURCE As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARED_ISSOURCE")

            '來源: 1.自行建立, 2.RMAREPAIR_QUOTED_DETAIL 匯入
            '如果來源 是匯入的不要再處理, 會照成報價資料 跟 維修資料不一至

            dvRepairDetail.RowFilter = "RMARED_ID='" & lblRMAREDID.Text.Trim() & "'"
            If dvRepairDetail.Count > 0 Then
                Dim dr As RmaDTO.RMARepair_DetailRow = dvRepairDetail(0).Row

                dr.RMARED_NSERIALNO = txtNewSerial.Text.Trim()      'new Serial No
                dr.RMARED_OSERIALNO = txtOldSerial.Text.Trim()      'old Serial No
                dr.RMARED_LOCATION = txtLocation.Text.Trim()        'SMT Location
                dr.RMARED_DEFECTIVE = cboDefective.SelectedValue    'Defect Reason

                If dr.RMARED_ISSOURCE = 1 Then
                    dr.RMARED_NPARTNO = txtNewPart.Text.Trim()  'new Part No
                    dr.RMARED_OPARTNO = txtOldPart.Text.Trim()  'old Part No

                    dr.RMARED_DESC = txtDescription.Text.Trim()         'Description
                    dr.RMARED_LOCATION = txtLocation.Text.Trim()        'SMT Location
                    dr.RMARED_IMPROPERUSAGE = cboImproper.SelectedValue 'Improper Usage

                    dr.RMARED_QTY = txtQty.Text.Trim()
                    dr.RMARED_MATERIALCOST = UI_txtMaterialCost.Text.Trim()

                    If IsNumeric(dr.RMARED_QTY) = True And IsNumeric(dr.RMARED_MATERIALCOST) = True Then
                        dr.RMARED_PRICE = Math.Round(dr.RMARED_QTY, 2) * Math.Round(dr.RMARED_MATERIALCOST, 2)
                    End If
                End If

                dr.RMARED_WAIVE = Convert.ToInt16(UI_RMARED_WAIVE.Text.Trim)
                dr.RMARED_OPTION = Convert.ToInt16(UI_RMARED_OPTION.Text.Trim)
                dr.RMARED_ISSOURCE = Convert.ToInt16(UI_RMARED_ISSOURCE.Text.Trim)
            End If
        Next

        Session("_dtRepairDetail") = dtRepairDetail
    End Sub

    ''' <summary>
    ''' 增加零件項目(查詢零件)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call Keep_RepairDetail_Data()

        Dim i As Integer = 0
        Dim MaterialCost As Double = 0
        Dim sDescription As String = ""

        Dim oRepairBOM As New ctlRMA.RepairBOM
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable
        Dim dtRepairDetail As RmaDTO.RMARepair_DetailDataTable = Session("_dtRepairDetail")

        Dim sPartsNo As String = Me.UI_txtPartsNo.Text.ToString().Trim()
        Dim sLocation As String = Me.UI_txtLocation.Text.ToString().Trim()

        dtRepairBOM = oRepairBOM.Query(Me.UI_lblCompNO.Text.Trim(), sPartsNo, sLocation)
        If dtRepairBOM.Rows.Count > 0 Then
            MaterialCost = Convert.ToDouble(dtRepairBOM.Rows(0)("RPBOM_MATERIALCOST"))
            sDescription = dtRepairBOM.Rows(0)("RPBOM_DESC").ToString().Trim()
        End If

        dtRepairDetail = addRepairDetail(sPartsNo, sLocation, MaterialCost, sDescription, dtRepairDetail)
        Session("_dtRepairDetail") = dtRepairDetail
        Call RepairDetail_DataBind(dtRepairDetail, 0)

        Me.UI_txtModel.Text = ""
        Me.UI_txtPartsNo.Text = ""
        Me.UI_txtLocation.Text = ""
    End Sub

    ''' <summary>
    ''' 增加零件項目
    ''' </summary>
    ''' <param name="PartsNo">PartsNo</param>
    ''' <param name="Location">Location</param>
    ''' <param name="MaterialCost">MaterialCost</param>
    ''' <param name="sDescription">sDescription</param>
    ''' <param name="dtRepairDetail">RMARepair_DetailDataTable</param>
    ''' <returns>傳回RMARepair_DetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function addRepairDetail(ByVal PartsNo As String, ByVal Location As String, ByVal MaterialCost As Double, ByVal sDescription As String, ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable) As RmaDTO.RMARepair_DetailDataTable
        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Dim dr As RmaDTO.RMARepair_DetailRow = dtRepairDetail.NewRMARepair_DetailRow

        dr.RMARED_ID = sGUID.Trim()
        dr.RMARED_oldID = ""
        dr.RMARED_RMADID = Me.UI_lblPreviousPage_RMADID.Text.ToString().Trim()

        dr.RMARED_NPARTNO = PartsNo.Trim()
        dr.RMARED_NSERIALNO = ""

        dr.RMARED_OPARTNO = ""
        dr.RMARED_OSERIALNO = ""

        dr.RMARED_DESC = sDescription.Trim()
        dr.RMARED_LOCATION = Location
        dr.RMARED_IMPROPERUSAGE = 0                 '非正常使用 : 0.No, 1.Yes
        dr.RMARED_DEFECTIVE = ""

        dr.RMARED_QTY = 1
        dr.RMARED_MATERIALCOST = MaterialCost
        dr.RMARED_PRICE = MaterialCost

        dr.RMARED_AD = Session("_UserID")
        dr.RMARED_ADNAME = Session("_UserName")
        dr.RMARED_CSTMP = Date.Now
        dr.RMARED_LUAD = Session("_UserID")
        dr.RMARED_LUADNAME = Session("_UserName")
        dr.RMARED_LUSTMP = Date.Now
        dr.RMARED_MARK = 0

        dr.RMARED_CURRENCYCODE = Me.UI_lblCurrencyCode.Text.Trim()
        dr.RMARED_CURRENCYRATE() = Me.UI_lblCurrencyRate.Text.Trim()

        dr.RMARED_ASSIGECURRENCYCODE = Me.UI_lblEssCurrencyCode.Text.Trim()
        dr.RMARED_ASSIGECURRENCYRATE = Me.UI_lblEssCurrencyRate.Text.Trim()
        dr.RMARED_ASSIGEPRICE = MaterialCost

        dr.RMARED_WAIVE = 0
        dr.RMARED_OPTION = 2
        dr.RMARED_ISSOURCE = 1

        dtRepairDetail.AddRMARepair_DetailRow(dr)

        Return dtRepairDetail
    End Function

    Private Sub Delete(ByVal RMARED_ID As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Call Keep_RepairDetail_Data()

        Dim dtRepairDetail As RmaDTO.RMARepair_DetailDataTable = Session("_dtRepairDetail")
        Dim dvRepair As DataView = dtRepairDetail.DefaultView

        dvRepair.RowFilter = "RMARED_ID='" & RMARED_ID.ToString().Trim() & "'"
        If dvRepair.Count > 0 Then
            dvRepair.Item(0)("RMARED_MARK") = "1"
        End If
        dvRepair.RowFilter = ""

        Session("_dtRepairDetail") = dtRepairDetail
        Call RepairDetail_DataBind(dtRepairDetail, 0)
    End Sub

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <Obsolete>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oRepair As New ctlRMA.Repair
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable
        Dim Item As DictionaryEntry
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        Dim bIncluedTop As Boolean = False
        Dim bSaveSn As Boolean = False

        Try
            Call Keep_RepairDetail_Data()
            Call CalTotalAmt()

            Dim sRecord As String = UI_lblPreviousPage_RMADID.Text.Trim()
            Dim bIsIncludeTop As Boolean = False
            For Each Item In hsSelectID
                bSaveSn = False
                Dim sUI_RMADID As String = Item.Key.ToString()
                Dim sUI_SERIALNO As String = Item.Value.ToString()
                If sRecord = sUI_RMADID.Trim Then
                    bIncluedTop = True
                    bSaveSn = True
                End If
                UI_lblPreviousPage_RMADID.Text = sUI_RMADID.Trim()

                Dim iTotalPrice As Double = 0
                dtRepairDetail = Save_RMARepairDetail(iTotalPrice, sUI_RMADID.Trim(), bSaveSn)
                dtRMARepair = Save_RMARepair(iTotalPrice, sRecord)
                oRepair.Save(dtRMARepair, dtRepairDetail, True)
            Next

            UI_lblPreviousPage_RMADID.Text = sRecord
            '如果要保存的项目不在所选择的清单中
            If bIncluedTop = False Then
                Dim iTotalPrice As Double = 0
                dtRepairDetail = Save_RMARepairDetail(iTotalPrice, sRecord, True)
                dtRMARepair = Save_RMARepair(iTotalPrice, sRecord)
                oRepair.Save(dtRMARepair, dtRepairDetail, True)
            End If
            blnFlag = True
            'BI保固回壓WarrantyBIExpen by buck add 20260102 begin
            'BI保固回壓WarrantyBIExpen修正 by buck modify 20260120 begin
            Dim dtRMAData As DataTable = TryCast(Session("_dtRequest"), DataTable)
            Dim watyBIExpendReqList As New List(Of WarrantyBIExpendReq)
            Dim ctlWarranty As New ctlWarranty
            Dim oReq As New ctlRMA.Requested
            Dim dtWarrantyBI_List = ctlWarranty.WARRANTY_BI_List()

            For Each dr As DataRow In dtRMAData.AsEnumerable()
                Dim SerialNO = dr.Field(Of String)("RMAD_SERIALNO")
                Dim sRMAD_ID = dr.Field(Of String)("RMAD_ID")
                Dim sRMA_NO = dr.Field(Of String)("RMAD_RMANO")
                Dim dtExport As RmaDTO.ExportDataTable = oReq.QueryByExport(SerialNO)

                dtRepairDetail.ToList.ForEach(Sub(x)
                                                  If x.RMARED_RMADID = sRMAD_ID And x.RMARED_DESC.Contains("BAT") Then
                                                      Dim OrderNum As String = dtExport.AsEnumerable().Select(Function(r) r("EXPORT_ORDERNUMBER")).FirstOrDefault()
                                                      If Not OrderNum Is Nothing Then
                                                          Dim idx As Integer = OrderNum.LastIndexOf("-"c)
                                                          Dim lsWarrantyBI = dtWarrantyBI_List.AsEnumerable.Where(Function(y) y.Field(Of String)("BI_ORDERNO") = OrderNum.Substring(0, idx)).ToList()

                                                          If lsWarrantyBI.Count > 0 Then
                                                              watyBIExpendReqList.Add(New WarrantyBIExpendReq With {
                                                                  .BE_ID = Guid.NewGuid.ToString("N").Substring(0, 20),
                                                                  .BE_ORDERNO = OrderNum.Substring(0, idx),
                                                                  .BE_TYPE = lsWarrantyBI.Select(Function(r) r.Field(Of String)("BI_SOURCE")).FirstOrDefault(),
                                                                  .BE_REFNO = sRMA_NO,
                                                                  .BE_ORDERSEQ = OrderNum.Substring(idx + 1),
                                                                  .BE_PRODSERIAL = SerialNO,
                                                                  .BE_BATSERIAL_OLD = x.RMARED_OSERIALNO,
                                                                  .BE_BATSERIAL_NEW = x.RMARED_NSERIALNO,
                                                                  .BE_USEQTY = "1",
                                                                  .BE_AD = Session("_UserID"),
                                                                  .BE_ADNAME = Session("_UserName"),
                                                                  .BE_CSTMP = Date.Now,
                                                                  .BE_LUAD = Session("_UserID"),
                                                                  .BE_LUADNAME = Session("_UserName"),
                                                                  .BE_LUSTMP = Date.Now
                                                              })
                                                          End If
                                                      End If
                                                  End If
                                              End Sub)

            Next
            If watyBIExpendReqList.Count > 0 Then
                Dim result As Result = ctlWarranty.InsertWarrantyBIExpen(watyBIExpendReqList)
                blnFlag = Result.IsSuccess
                If Not Result.IsSuccess Then
                    Throw New Exception(Result.Message)
                End If
            End If
            'BI保固回壓WarrantyBIExpen修正 by buck modify 20260120 end
            'BI保固回壓WarrantyBIExpen by buck add 20260102 end
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            Dim flgSendMail As Boolean = False
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim oHashtable As New System.Collections.Hashtable
                oHashtable = Me.ViewState("_HistoryKey")
                Session("_PreviousPage") = oHashtable

                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Transfer, "")
                flgSendMail = True
            End If

            Call QueryDataByHead()
            Call QueryDataByDetail()
            Call QueryDataByStatusPoint()
            Call QueryData(UI_dvRequest.PageIndex)

            If flgSendMail = True Then
                '寄送 整張完修 Mail
                Dim dtRequest As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
                dtRequest.DefaultView.RowFilter = "CONVERT(RMAD_STATUS, 'System.Int32')<=60 AND RMAR_REPAIRAD is null"
                If dtRequest.DefaultView.Count() = 0 Then
                    'Throw New Exception("寄送 整張完修 Mail")
                    Dim isSendMail As Boolean = SendMail("")
                End If
                dtRequest.DefaultView.RowFilter = ""
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 寄送mail
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSendMail.Click
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim bIncludeTop As Boolean = False
        Dim sSERIALNO As String = ""
        Dim blnFlag_isRepairQuoted As Boolean = False

        Dim Item As DictionaryEntry
        Dim oRepairQuoted As New ctlRMA.Repair_Quoting

        Try
            Dim dtRequest As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            'For i = 0 To dtRequest.Rows.Count - 1
            '    Dim sUI_RMADID As String = dtRequest.Rows(i)("RMAD_ID").ToString()
            '    Dim sUI_SERIALNO As String = dtRequest.Rows(i)("RMAD_SERIALNO").ToString()
            '    Dim sUI_RMADSTATUS As String = dtRequest.Rows(i)("RMAD_STATUS").ToString()
            '    Dim RMAR_REPAIRAD As String = dtRequest.Rows(i)("RMAR_REPAIRAD").ToString().Trim()
            '    Dim ooxx As String = ""
            'Next

            dtRequest.DefaultView.RowFilter = "CONVERT(RMAD_STATUS, 'System.Int32')<=60 AND RMAR_REPAIRAD is null"
            '檢核是否已全部完修, 是, 則寄送 整張報價 Mail
            If dtRequest.DefaultView.Count() > 0 Then
                '寄送 有勾選單項次維修 Mail
                Dim sRecord As String = UI_lblPreviousPage_RMADID.Text.Trim()
                Dim hsSelectID As Hashtable = ViewState("hsSelectID")

                sSERIALNO = sSERIALNO & Me.UI_lblSerialText.Text.Trim()
                For Each Item In hsSelectID
                    Dim sUI_RMADID As String = Item.Key.ToString()
                    Dim sUI_SERIALNO As String = Item.Value.ToString()

                    If sRecord = sUI_RMADID Then
                        bIncludeTop = True
                    Else
                        If sSERIALNO.Trim <> "" Then
                            sSERIALNO = sSERIALNO & ","
                        End If
                        sSERIALNO = sSERIALNO & sUI_SERIALNO
                    End If
                Next


                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 
                '35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                Dim arrSERIALNO() As String = sSERIALNO.Split(",")
                For i = 0 To dtRequest.Rows.Count - 1
                    Dim sUI_RMADID As String = dtRequest.Rows(i)("RMAD_ID").ToString()
                    Dim sUI_SERIALNO As String = dtRequest.Rows(i)("RMAD_SERIALNO").ToString()
                    Dim sUI_RMADSTATUS As String = dtRequest.Rows(i)("RMAD_STATUS").ToString()
                    Dim RMAR_REPAIRAD As String = dtRequest.Rows(i)("RMAR_REPAIRAD").ToString().Trim()

                    For j = 0 To arrSERIALNO.Length - 1
                        '如果 RMAD_STATUS=60 and 尚未有填維修單, 單身狀態顯示為 (Repairing)
                        If sUI_SERIALNO.Trim() = arrSERIALNO(j).Trim() And (sUI_RMADSTATUS <> 60 Or RMAR_REPAIRAD = "") Then
                            Dim validatorMsg As String = _oLanguage.getText("RMA", "239", ctlLanguage.eumType.Validator)
                            validatorMsg = validatorMsg.Replace("[$item$]", sUI_SERIALNO.Trim())
                            Throw New Exception(validatorMsg)
                        End If
                    Next
                Next

                Dim iSeq As Integer = 0
                Dim SerialItem As String = ""
                For j = 0 To arrSERIALNO.Length - 1
                    iSeq = iSeq + 1
                    SerialItem = SerialItem & "items " & iSeq.ToString() & ". " & arrSERIALNO(j).Trim() + "\n"
                Next
                'Throw New Exception(SerialItem)

                Dim isSendMail As Boolean = SendMail(SerialItem)
                blnFlag = True

            Else
                '寄送 整張完修 Mail
                'Throw New Exception("寄送 整張完修 Mail")
                Dim isSendMail As Boolean = SendMail("")
                blnFlag = True
            End If
            dtRequest.DefaultView.RowFilter = ""

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.MailOK)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

    End Sub

    'RMA Report 開始
    Private Sub getRequestForm(ByVal sRMAID As String)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)
        Dim EndUser As Boolean = False

        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)

        EndUser = oRMARequest.chkISCWarrantyFee(sRMAID)

        Dim UI_txtAccountIDText As String = ""

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
            Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow
            '廖先生來 我造幹
            UI_txtAccountIDText = dr.RMA_CUNO.ToString().Trim()

            If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
            If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
            If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
            If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
            If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
            If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
            If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
            If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

            If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
            If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
            If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
            If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
            If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()

            If (EndUser AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If

            Try
                '20240308 客戶編號 開始
                Dim myctAddress As New ctlWarranty
                Dim CUSTOMER_PRODUCT_NUMBER As String = myctAddress.Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(dr.RMA_NO.ToString(), dr.RMAD_SERIALNO.ToString())
                drReport.RMAD_PRODUCTDESC = CUSTOMER_PRODUCT_NUMBER
                '20240308 客戶編號 結束
            Catch

            End Try


            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next

        Dim UI_PartsRequest As Boolean = False '2024/07/04 Report檔

        Call Print(dtReport, EndUser, LanguageID, UI_txtAccountIDText, UI_PartsRequest)
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal LanguageID As String, ByVal UI_txtAccountIDText As String, ByVal UI_PartsRequest As Boolean)


        '2024/11/15 檢查系統上RMA RMA_EUCOMPANY 終端客戶是否有填入值 開始
        Try

            If dtReport.Rows(0)("RMA_EUCOMPANY") Is DBNull.Value Then
                EndUser = False

            Else
                EndUser = True
            End If

        Catch

        End Try

        '2024/11/15 檢查系統上RMA RMA_EUCOMPANY 終端客戶是否有填入值 結束

        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        '取得客戶的語系
        Dim sCust As String = UI_txtAccountIDText
        Dim oLoginInfo As New ctlLoginInfo
        Dim sLanguageID_ As String = oLoginInfo.getLanguageID("Customer", sCust)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument

        If (UI_PartsRequest) Then
            If (sLanguageID_ = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts.rpt"))
            End If

        ElseIf (EndUser) Then
            If (sLanguageID_ = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            End If
        Else
            If (sLanguageID_ = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_02_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_02.rpt"))
            End If

        End If

        ReportDoc.SetDataSource(oReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF)
        Me.ViewState("_AttachmentFile_001") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_001") = ConfigureExportToPdf(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        ReportDoc.Close()
    End Sub
    'RMA Report 結束

    Private Function SendMail(ByVal sSERIALNO As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try
            '================================================================================================================================================================================================================
            '維修中心維修確認-->對象(客戶 及 業務和助理)  若有Paypal付款 要寄給Shipping 及 Miko Jean
            '================================================================================================================================================================================================================
            dtCustomer = oCustomer.QueryUser(Me.UI_lblRMACUNO.Text.Trim(), Me.UI_lblRMAACCOUNTID.Text.Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                Dim mailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                Dim CU_NAME As String = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()


                Dim MailSales As String = ""
                Dim SalesName As String = ""
                If CU_SALESID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_SALESID, "")
                    MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                    SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim MailAssistant As String = ""
                Dim AssistantName As String = ""
                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.UI_lblRMAACCOUNTID.Text.Trim())

                Dim sSubject As String = _oLanguage.getMailText("Mail", "033", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody_sales As String = _oLanguage.getMailText("Mail", "034", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody_customer As String = _oLanguage.getMailText("Mail", "034", ctlLanguage.eumType.Mail, LanguageID)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                    Dim sMailTo As String = ""
                    Dim sDearName As String = ""

                    sSubject = sSubject.Replace("[$Name$]", CU_NAME.Trim())
                    sSubject = sSubject.Replace("[$Customer User Name$]", CU_NAME.Trim())
                    sSubject = sSubject.Replace("[$RMA No$]", Me.UI_lblPreviousPage_RMANO.Text.Trim())

                    If MailSales.Trim() <> "" Then
                        sDearName = sDearName & SalesName
                        sMailTo = MailSales.Trim()
                    End If

                    If MailAssistant.Trim() <> "" Then
                        If sDearName <> "" Then
                            sDearName = sDearName & "/"
                        End If
                        sDearName = sDearName & AssistantName

                        If sMailTo <> "" Then
                            sMailTo = sMailTo & ","
                        End If
                        sMailTo = sMailTo & MailAssistant.Trim()
                    End If

                    'Enduser 加發給Shipping 及 Miko Jean
                    Dim dt As DataTable = oRequested.IsEndUser(dtCustomer.Rows(0)("CU_NO").ToString().Trim())
                    If (dt.Rows.Count > 0) Then
                        If (Me.UI_lblCompNO.Text = "CLHQ") Then

                            'Shipping
                            'Dim oRMA As New ctlRMA
                            'Dim ShippingMail As String = ""
                            'Dim arrShipping As ArrayList = oRMA.getShippingMail_RMA(Me.UI_lblPreviousPage_RMANO.Text.Trim())
                            'For i = 0 To arrShipping.Count - 1
                            'Dim arrList() As String = arrShipping(i)
                            'ShippingMail += arrList(1).Trim() + ","
                            'Next
                            'If sMailTo <> "" Then
                            'sMailTo = sMailTo & ","
                            'End If
                            'sMailTo = sMailTo & Mid(ShippingMail.Trim(), 1, Len(ShippingMail.Trim()) - 1)

                            'Miko 及 Jean
                            sMailTo = sMailTo + "," + _RepairOKEmail.Trim() + "," + "rocio.peng@cipherlab.com.tw" + "," + "Lynna.Wu@cipherlab.com.tw" + "," + "Vivian.Shen@cipherlab.com.tw" + "," + "May.Chiou@cipherlab.com.tw"
                        End If
                    End If

                    Call getRequestForm(UI_lblRMAID.Text.Trim())
                    Dim Requested_ As New ctlRMA.Requested
                    Dim Note As String = "</br><span>" & "RMA Note:  \n " & Requested_.QueryByRMA_REMARK(UI_lblPreviousPage_RMANO.Text.Trim()) & "</span> \n </br>"
                    Dim oAttachmentFile As New Collection
                    If Me.ViewState("_AttachmentFile_001").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_001").ToString())
                    End If

                    '對象(業務和助理)
                    sBody_sales = sBody_sales.Replace("[$Name$]", sDearName)
                    sBody_sales = sBody_sales.Replace("[$Customer User Name$]", sDearName)
                    sBody_sales = sBody_sales.Replace("[$Note$]", Note)

                    sBody_sales = sBody_sales.Replace("[$RMA No$]", Me.UI_lblPreviousPage_RMANO.Text.Trim())
                    sBody_sales = sBody_sales.Replace("[$Email URL$]", sEmailURL)
                    sBody_sales = sBody_sales.Replace("[$item$]", sSERIALNO)
                    'sBody_sales = sBody_sales.Replace("[$item$]", sSERIALNO & " \n " & Note & " \n ")

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = sMailTo
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody_sales, mailTo, _MailCC, oAttachmentFile)


                    '對象(客戶)
                    sBody_customer = sBody_customer.Replace("[$Name$]", CU_NAME)
                    sBody_customer = sBody_customer.Replace("[$Customer User Name$]", CU_NAME)
                    sBody_customer = sBody_customer.Replace("[$RMA No$]", Me.UI_lblPreviousPage_RMANO.Text.Trim())
                    sBody_customer = sBody_customer.Replace("[$Note$]", Note)
                    sBody_customer = sBody_customer.Replace("[$Email URL$]", sEmailURL)
                    sBody_customer = sBody_customer.Replace("[$item$]", sSERIALNO)
                    sBody_customer = sBody_customer.Replace("[$item$]", sSERIALNO)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        mailUser = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody_customer, mailUser, _MailCC)

                    '日本多一封要寄給維修人員
                    'Print_Notice(Me.UI_lblPreviousPage_RMANO.Text.Trim())
                    'SendMailNotices(sSubject, sBody_customer)

                End If


            End If

        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                'sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                'Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return blnFlag

    End Function

#Region "Print Notice"

    Private Sub SendMailNotices(sSubject As String, sBody_customer As String)
        Dim oMail As New ctlMail
        Dim oAttachmentFile As New Collection
        Dim ctlRMA As New ctlRMA
        Dim blnFlag As Boolean

        Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
        Dim Repaire_Name As String = ""                    '維修人員
        '取得維修人員Mail
        Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RepairCenter(Me.UI_lblCompNO.Text)

        If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
            oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())

            For j = 0 To arrRepaire.Count - 1
                Dim arrList() As String = arrRepaire(j)
                Repaire_Name = arrList(0).Trim()
                Repaire_EMAIL = arrList(1).Trim()

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject, sBody_customer, Repaire_EMAIL, _MailCC, oAttachmentFile)
            Next
        End If

    End Sub

    Private Sub Print_Notice(ByVal sRMANO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable
        Dim EndUser As Boolean = False

        Try
            dtRequest = ctlReport.qryNotices(sRMANO)
            For i = 0 To dtRequest.Rows.Count - 1
                Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
                Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow

                If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
                If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
                If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
                If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
                If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
                If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
                If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
                If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
                If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
                If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
                If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
                'If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
                'If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
                'If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
                'If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
                'If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

                'If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
                If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
                If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
                'If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

                If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
                If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
                If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
                If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
                If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUCOMPANY.ToString().Trim()

                'If dr.IsRMASH_CSTMPNull = False Then drReport.RMASH_CSTMP = dr.RMASH_CSTMP.ToString().Trim()
                'If dr.IsRMASH_SHIPPINGNONull = False Then drReport.RMASH_SHIPPINGNO = dr.RMASH_SHIPPINGNO.ToString().Trim()
                'If dr.IsRMASH_TRACKINGNONull = False Then drReport.RMASH_TRACKINGNO = dr.RMASH_TRACKINGNO.ToString().Trim()

                drReport.SeqID = i + 1

                dtReport.AddRequestReportRow(drReport)
            Next
            Dim oDsReport As New DataSet
            oDsReport.Tables.Add(dtReport)
            Call Print_PackingList(oDsReport)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Private Sub Print_PackingList(ByVal oDsReport As DataSet)
        Dim sReportToPDF As String = "JPNotices_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\rptRequest_Shp_jp1.rpt"))
        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF)
        Me.ViewState("_AttachmentFile_01") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_01") = ConfigureExportToPdf(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        ReportDoc.Close()
    End Sub

    'Public Sub ExportSetup()
    '    If Not System.IO.Directory.Exists(_Reoprt_FilePath) Then
    '        System.IO.Directory.CreateDirectory(_Reoprt_FilePath)
    '    End If

    '    myDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    myExportOptions = ReportDoc.ExportOptions
    '    ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
    'End Sub

    'Public Function ConfigureExportToPdf(ByVal sReportToPDF As String) As String
    '    Dim retval As String = _Reoprt_FilePath & sReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & sReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    ' '' ''Dim sScript As String = ""
    '    ' '' ''sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    ' '' ''sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    '' '' ''sScript = sScript & "window.location.href='Shipping_List.aspx';" & vbCrLf
    '    ' '' ''sScript = sScript & "</script>" & vbCrLf
    '    ' '' ''Response.Write(sScript)

    '    Return retval
    'End Function

#End Region

    Private Function Save_RMARepair(ByVal iTotalPrice As Double, ByVal sTop_RMADID As String) As RmaDTO.RMARepairDataTable
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Try
            Dim dr As RmaDTO.RMARepairRow = dtRMARepair.NewRMARepairRow

            dr.RMAR_ID = sGUID
            dr.RMAR_RMADID = Me.UI_lblPreviousPage_RMADID.Text

            dr.RMAR_COMPNO = Me.UI_lblEssCompNO.Text.Trim()
            If Me.UI_lblCompNO.Text.Trim() <> "" Then
                dr.RMAR_COMPNO = Me.UI_lblCompNO.Text.Trim()
            End If

            dr.RMAR_FARCNO = Me.UI_lblFARCNO.Text.Trim()
            dr.RMAR_FARNO = Me.UI_lblFARNO.Text.Trim()

            'If Me.UI_lblProblemDescText.Text.Trim <> "" Then dr.RMAR_PROBLEMDESC = Me.UI_lblProblemDescText.Text.Trim()

            If dr.RMAR_RMADID.Trim = sTop_RMADID.Trim() Then
                If Me.UI_txtRepairDesc.Text.Trim() <> "" Then dr.RMAR_REPAIRDESC = Me.UI_txtRepairDesc.Text.Trim()
                If Me.UI_txtRepairMemo.Text.Trim() <> "" Then dr.RMAR_REPAIRMEMO = Me.UI_txtRepairMemo.Text.Trim()
            End If

            dr.RMAR_REPAIR_ISFILL = "0"                         '是否已填寫維修報價單:0.否, 1.是

            dr.RMAR_CURRENCYCODE = Me.UI_lblCurrencyCode.Text.Trim()
            dr.RMAR_CURRENCYRATE = Convert.ToDouble(Me.UI_lblCurrencyRate.Text.Trim())

            dr.RMAR_ASSIGECURRENCYCODE = Me.UI_lblEssCurrencyCode.Text.Trim()
            dr.RMAR_ASSIGECURRENCYRATE = Convert.ToDouble(Me.UI_lblEssCurrencyRate.Text.Trim())

            Dim iTotalManAmt As Double = 0
            If Me.UI_txtManHour.Text.Trim() = "" Then Me.UI_txtManHour.Text = "0"

            If Me.UI_txtManHour.Text.Trim() <> "" And Me.UI_txtLABORPrice.Text.Trim <> "" Then
                If IsNumeric(Me.UI_txtManHour.Text.Trim()) = True And IsNumeric(Me.UI_txtLABORPrice.Text.Trim()) = True Then
                    dr.RMAR_LABORHOUR = Convert.ToDouble(Me.UI_txtManHour.Text.Trim())
                    dr.RMAR_LABORPRICE = Convert.ToDouble(Me.UI_txtLABORPrice.Text)

                    iTotalManAmt = dr.RMAR_LABORHOUR * dr.RMAR_LABORPRICE
                    dr.RMAR_LABORCOST = Math.Round(iTotalManAmt, 2)
                    dr.RMAR_ASSIGELABORCOST = oCommon.ConvertCurrency(dr.RMAR_CURRENCYRATE, dr.RMAR_LABORCOST, dr.RMAR_ASSIGECURRENCYRATE)
                End If
            End If

            dr.RMAR_MATERIALCOST = iTotalPrice
            dr.RMAR_ASSIGEMATERIALCOST = oCommon.ConvertCurrency(dr.RMAR_CURRENCYRATE, dr.RMAR_MATERIALCOST, dr.RMAR_ASSIGECURRENCYRATE)

            dr.RMAR_QUOTE = Math.Round(iTotalManAmt + iTotalPrice, 2)
            dr.RMAR_ASSIGEQUOTE = oCommon.ConvertCurrency(dr.RMAR_CURRENCYRATE, dr.RMAR_QUOTE, dr.RMAR_ASSIGECURRENCYRATE)

            dr.RMAR_AD = Session("_UserID")
            dr.RMAR_ADNAME = Session("_UserName")
            dr.RMAR_CSTMP = Date.Now
            dr.RMAR_LUAD = Session("_UserID")
            dr.RMAR_LUADNAME = Session("_UserName")
            dr.RMAR_LUSTMP = Date.Now

            dr.RMAR_REPAIRAD = Session("_UserID")
            dr.RMAR_REPAIRADNAME = Session("_UserName")
            dr.RMAR_REPAIRDATE = Date.Now

            dtRMARepair.AddRMARepairRow(dr)

        Catch ex As Exception
            Throw ex
        End Try

        Return dtRMARepair
    End Function

    Private Function Save_RMARepairDetail(ByRef iTotalPrice As Double, ByRef UI_RMADID As String, ByRef bSavePartSn As Boolean) As RmaDTO.RMARepair_DetailDataTable
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export
        Dim dtSession As RmaDTO.RMARepair_DetailDataTable = Session("_dtRepairDetail")
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable

        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Try
            Dim COMPNO As String = Me.UI_lblEssCompNO.Text.Trim()
            If Me.UI_lblCompNO.Text.Trim() <> "" Then
                COMPNO = Me.UI_lblCompNO.Text.Trim()
            End If

            'oDataView.RowFilter = "RMARED_MARK=0"
            For i = 0 To dtSession.Rows.Count - 1
                Dim drSession As RmaDTO.RMARepair_DetailRow = dtSession.Rows(i)

                Dim dr As RmaDTO.RMARepair_DetailRow = dtRepairDetail.NewRMARepair_DetailRow

                dr.RMARED_ID = drSession.RMARED_ID
                'dr.RMARED_oldID = drSession.RMARED_oldID
                'dr.RMARED_RMADID = drSession.RMARED_RMADID
                dr.RMARED_RMADID = UI_RMADID
                dr.RMARED_NSERIALNO = ""

                dr.RMARED_NPARTNO = drSession.RMARED_NPARTNO
                dr.RMARED_NSERIALNO = ""
                Dim RMARED_NSERIALNO As String = ""
                If drSession.IsRMARED_NSERIALNONull = False Then
                    If bSavePartSn Then '只有被選中的保存 SN 其他不保存
                        RMARED_NSERIALNO = drSession.RMARED_NSERIALNO
                        dr.RMARED_NSERIALNO = RMARED_NSERIALNO
                    End If
                End If

                'Dim Warranty As String = oExport.getWarranty(dr.RMARED_NPARTNO, RMARED_NSERIALNO, COMPNO)
                'Dim Warranty As String = oExport.getMaxWarranty(dr.RMARED_NPARTNO, RMARED_NSERIALNO, "", COMPNO)
                'If Warranty <> "" Then
                '    dr.RMARED_NWARRANTY = Convert.ToDateTime(Warranty)
                'End If


                Dim RMARED_oPartNo As String = ""
                Dim RMARED_oSerialNo As String = ""
                If drSession.IsRMARED_OPARTNONull = False Then
                    RMARED_oPartNo = drSession.RMARED_OPARTNO
                    dr.RMARED_OPARTNO = drSession.RMARED_OPARTNO
                End If

                If drSession.IsRMARED_OSERIALNONull = False Then
                    RMARED_oSerialNo = drSession.RMARED_OSERIALNO
                    dr.RMARED_OSERIALNO = drSession.RMARED_OSERIALNO
                End If

                'If RMARED_oPartNo.Trim() <> "" Or RMARED_oSerialNo.Trim <> "" Then
                '    'Warranty = oExport.getWarranty(RMARED_oPartNo, RMARED_oSerialNo, COMPNO)
                '    Warranty = oExport.getMaxWarranty(RMARED_oPartNo, RMARED_oSerialNo, "", COMPNO)
                '    If Warranty <> "" Then
                '        dr.RMARED_OWARRANTY = Convert.ToDateTime(Warranty)
                '    End If
                'End If


                If drSession.IsRMARED_DESCNull = False Then dr.RMARED_DESC = drSession.RMARED_DESC
                If drSession.IsRMARED_LOCATIONNull = False Then dr.RMARED_LOCATION = drSession.RMARED_LOCATION
                dr.RMARED_IMPROPERUSAGE = drSession.RMARED_IMPROPERUSAGE
                If drSession.IsRMARED_DEFECTIVENull = False Then dr.RMARED_DEFECTIVE = drSession.RMARED_DEFECTIVE

                dr.RMARED_QTY = drSession.RMARED_QTY
                dr.RMARED_MATERIALCOST = drSession.RMARED_MATERIALCOST
                dr.RMARED_PRICE = drSession.RMARED_PRICE
                dr.RMARED_CURRENCYCODE = Me.UI_lblCurrencyCode.Text.Trim()
                dr.RMARED_CURRENCYRATE = Convert.ToDouble(Me.UI_lblCurrencyRate.Text.Trim())

                If drSession.RMARED_MARK = 0 Then
                    iTotalPrice = iTotalPrice + dr.RMARED_PRICE
                End If


                '指派的維修中心 - 幣別代號(ex:NTD , USD)
                dr.RMARED_ASSIGECURRENCYCODE = Me.UI_lblEssCurrencyCode.Text.Trim()
                '指派的維修中心 - 兌美金匯率
                dr.RMARED_ASSIGECURRENCYRATE = Convert.ToDouble(Me.UI_lblEssCurrencyRate.Text.Trim())
                '轉換成 指派的維修中心 --> Price
                dr.RMARED_ASSIGEPRICE = oCommon.ConvertCurrency(dr.RMARED_CURRENCYRATE, dr.RMARED_PRICE, dr.RMARED_ASSIGECURRENCYRATE)


                dr.RMARED_AD = Session("_UserID")
                dr.RMARED_ADNAME = Session("_UserName")
                dr.RMARED_CSTMP = Date.Now
                dr.RMARED_LUAD = Session("_UserID")
                dr.RMARED_LUADNAME = Session("_UserName")
                dr.RMARED_LUSTMP = Date.Now
                dr.RMARED_MARK = drSession.RMARED_MARK

                dr.RMARED_WAIVE = drSession.RMARED_WAIVE
                dr.RMARED_OPTION = drSession.RMARED_OPTION
                dr.RMARED_ISSOURCE = drSession.RMARED_ISSOURCE

                dtRepairDetail.AddRMARepair_DetailRow(dr)
            Next

        Catch ex As Exception
            Throw ex
        End Try

        Return dtRepairDetail
    End Function

#Region "Requested Information"

    ''' <summary>
    ''' 查詢 RepairBom Part's No
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdParts_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ucRepairRarts.show = True
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable


        '2011/08/04 START
        '原code:
        'dtRequest = oClient.Query01(UI_lblPreviousPage_RMANO.Text.ToLower(), "", "", "", "", "-1", "", "91", Session("_RepairCenter"), "RMAD_RMANO desc,RMAD_SERIALNO")

        '改成:
        dtRequest = oClient.Query01(UI_lblPreviousPage_RMANO.Text.ToLower(), "", "", "", "", "-1", "", "", Session("_RepairCenter"), "RMAD_RMANO desc,RMAD_SERIALNO")
        '2011/08/04 END


        Call ArrangementData(dtRequest)
        Session("_dtRequest") = dtRequest
        Dim dvRequest As DataView = dtRequest.DefaultView
        Me.ViewState("_SortExpression") = "RMAD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvRequest.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Request_DataBind(dvRequest, iPageIndex)
    End Sub

    Private Sub Request_DataBind(ByVal dvRequest As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRequest
        Me.UI_dvRequest.DataBind()

        Call setFlowCase_UI_dvRequest()
    End Sub

    Private Sub setFlowCase_UI_dvRequest()
        If Me.UI_flowCase.Text = "01" Then
            Me.UI_dvRequest.Columns(7).Visible = False      'Estimated Amount
        End If

        If Me.UI_flowCase.Text = "02" Then
            Me.UI_dvRequest.Columns(7).Visible = True       'Estimated Amount
        End If
    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""

        If dtRequest.Columns("SeqID") Is Nothing Then
            dtRequest.Columns.Add("SeqID")
            dtRequest.Columns.Add("Warranty")
            dtRequest.Columns.Add("CWEndWarr")
            dtRequest.Columns.Add("SWEndWarr")
            dtRequest.Columns.Add("RequestDate")
            dtRequest.Columns.Add("Quoted")
            dtRequest.Columns.Add("Amount")
            dtRequest.Columns.Add("Status")
            dtRequest.Columns.Add("Assign")
        End If

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.tmpRequest_ListRow = dtRequest.Rows(i)
            dtRequest.Rows(i)("SeqID") = i + 1

            '2021/05/06 轉換Model
            sModelNo = oExport.getMModelNo(dtRequest.Rows(i)("RMAD_MODELNO").ToString().Trim(), dr.RMA_COMPNO.ToString().Trim(), Me.UI_lblRMAACCOUNTID.Text.ToString())

            If sModelNo.Trim() <> "" Then
                dtRequest.Rows(i)("RMAD_MODELNO") = sModelNo.Trim()
            End If

            '保固日期
            If dr.IsRMAD_WARRANTYNull = False Then
                dtRequest.Rows(i)("Warranty") = Convert.ToDateTime(dtRequest.Rows(i)("RMAD_WARRANTY").ToString()).ToShortDateString()
            Else
                dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRequest.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtRequest.Rows(i)("Warranty") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select
            End If

            'Dim oExport As New ctlRMA.Export
            Dim sCWEnd As String = oExport.getWarrantyCW(dtRequest.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRequest.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If
            Dim sSWEnd As String = oExport.getWarrantySW(dtRequest.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRequest.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If
            'Assign
            dtRequest.Rows(i)("Assign") = ""
            Dim RMA_COMPNO As String = dtRequest.Rows(i)("RMA_COMPNO").ToString().Trim()
            Dim RMAR_COMPNO As String = dtRequest.Rows(i)("RMAR_COMPNO").ToString().Trim()
            If RMA_COMPNO <> RMAR_COMPNO And RMAR_COMPNO <> "" Then
                dtRequest.Rows(i)("Assign") = dtRequest.Rows(i)("COMP_NAME")
            End If

            '申請日期
            dtRequest.Rows(i)("RequestDate") = Convert.ToDateTime(dtRequest.Rows(i)("RMAD_CSTMP").ToString()).ToShortDateString()

            '維修總金額
            '1.先依維修單的總金額為主
            '2.若維修單無資料,再取報價單總金額
            Dim sQuoted As String = ""
            If dr.IsRMAR_QUOTENull = False Then
                sQuoted = dr.RMAR_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMAR_QUOTE.ToString()).ToString("N")
            ElseIf dr.IsRMARQ_QUOTENull = False Then
                sQuoted = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARQ_QUOTE.ToString()).ToString("N")
            End If
            dtRequest.Rows(i)("Quoted") = sQuoted.Trim()

            '業務總金額
            '1.先依業務出貨單的總金額為主
            '2.若業務出貨單無資料,再取業務報價單總金額
            Dim sAmount As String = ""
            If dr.IsRMARSD_QUOTENull = False Then
                sAmount = dr.RMARSD_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMARSD_QUOTE.ToString()).ToString("N")
            ElseIf dr.IsRMASQ_QUOTENull = False Then
                sAmount = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " " & Convert.ToDouble(dr.RMASQ_QUOTE.ToString()).ToString("N")
            End If
            dtRequest.Rows(i)("Amount") = sAmount.Trim()

            '如果 RMAD_STATUS=60 and 尚未有填維修單, 單身狀態顯示為 (Repairing)
            If dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim() = "60" And dtRequest.Rows(i)("RMAR_REPAIRAD").ToString().Trim() = "" Then
                dtRequest.Rows(i)("Status") = _oLanguage.getText("Common", "068", ctlLanguage.eumType.Status)
            Else
                dtRequest.Rows(i)("Status") = oCommon.ConvertToItemStatusText(dtRequest.Rows(i)("RMAD_STATUS").ToString().Trim(), dtRequest.Rows(i)("RMAD_ID").ToString().Trim())
            End If
        Next

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            Dim UI_RMAR_COMPNO As Label = e.Row.FindControl("UI_RMAR_COMPNO")
            Dim UI_Check As CheckBox = e.Row.FindControl("UI_Check")
            Dim UI_RMAD_SERIALNO As LinkButton = e.Row.FindControl("UI_RMAD_SERIALNO")
            Dim hsSelectID As Hashtable = ViewState("hsSelectID")
            Dim UI_RMADID As Label = e.Row.FindControl("UI_RMADID")
            Dim UI_Status As Label = e.Row.FindControl("UI_Status")
            Dim RMAR_REPAIRAD As Label = e.Row.FindControl("RMAR_REPAIRAD")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            Dim UI_RMADPARTSN As Label = e.Row.FindControl("UI_RMADPARTSN")

            If UI_RMAD_SERIALNO.Text.Trim().Equals(UI_lblSerialText.Text.Trim()) Then
                e.Row.BackColor = Drawing.Color.Pink
            End If

            If UI_RMADPARTSN.Text <> "" Then
                UI_RMAD_SERIALNO.Text = UI_RMADPARTSN.Text
            End If

            If ((UI_RMADSTATUS.Text.Trim() = "50" Or UI_RMADSTATUS.Text.Trim() = "60")) Then
                UI_RMAD_SERIALNO.Enabled = True

                'UI_Check.Visible = True
                If hsSelectID.ContainsKey(UI_RMADID.Text) Then
                    UI_Check.Checked = True
                End If

                If UI_RMAR_COMPNO.Text.Trim <> "" Then
                    Dim sRepairCenter As String = Session("_RepairCenter")
                    Dim sInRepairCenter As String = ""
                    Dim arrRepair() As String = sRepairCenter.Split(",")
                    For i = 0 To arrRepair.Length - 1
                        If sInRepairCenter <> "" Then
                            sInRepairCenter = sInRepairCenter + ","
                        End If
                        sInRepairCenter = sInRepairCenter + "'" + arrRepair(i).Trim() + "'"
                    Next

                    If sInRepairCenter.IndexOf("'" + UI_RMAR_COMPNO.Text.Trim() + "'") < 0 Then
                        UI_Check.Visible = False
                        UI_RMAD_SERIALNO.Enabled = False
                    End If
                End If
            Else
                UI_Check.Visible = False
                UI_RMAD_SERIALNO.Enabled = False
            End If


            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(3).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(3).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(3).ForeColor = Drawing.Color.Red
                End If
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

                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "DataControlPagerLinkButton".ToLower() Then
                    Dim oLinkButton As LinkButton = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLinkButton.Attributes.Add("onclick", "onProgress('Process')")
                End If

            Next
        End If
    End Sub

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRequest") Is Nothing Then
            Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            Dim dvReceiveList As DataView = dtReceiveList.DefaultView
            Call Request_DataBind(dvReceiveList, iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand
        Keep_RepairDetail_Data()
        If e.CommandName = "cmdChangeSn" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)

            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim UI_RMAD_SERIALNO As LinkButton = row.FindControl("UI_RMAD_SERIALNO")

            Me.UI_lblPreviousPage_RMADID.Text = UI_RMADID.Text.Trim()
            Me.UI_lblPreviousPage_RMANO.Text = UI_RMANO.Text.Trim()

            Call QueryDataByHead()
            Call QueryDataByDetail()
            Call QueryDataByStatusPoint()
            Call QueryData(UI_dvRequest.PageIndex)
            Call QuerySDC()
            CalTotalAmt()
            ViewState("hSerialNumber") = UI_RMAD_SERIALNO.Text
            GetVersigonData(UI_RMAD_SERIALNO.Text)
            pnlVersion.Visible = False
            UI_CheckVer.Checked = False
        End If
        If e.CommandName = "cmdDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Me.ucClientDetailPur.show(UI_RMADID.Text.ToString().Trim(), UI_RMANO.Text.Trim(), True)
        End If

        If e.CommandName = "cmdDetail_img" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Me.ucClientDetail.show(UI_RMADID.Text.Trim(), UI_RMANO.Text.Trim(), True)
        End If

        If e.CommandName = "cmdSWDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Me.ucSpecialSetting.show(UI_RMADID.Text.ToString().Trim(), True)
        End If
    End Sub

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_checkGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Keep_RepairDetail_Data()
        Dim i As Integer = 0
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        '保存选择资料
        If Not Session("_dtRequest") Is Nothing Then
            Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            For i = 0 To dtReceiveList.Rows.Count - 1
                Dim sUI_RMADID As String = dtReceiveList.Rows(i)("RMAD_ID").ToString()
                Dim sUI_SERIALNO As String = dtReceiveList.Rows(i)("RMAD_SERIALNO").ToString()
                Dim sUI_RMADSTATUS As String = dtReceiveList.Rows(i)("RMAD_STATUS").ToString()
                Dim sUI_Status As String = dtReceiveList.Rows(i)("Status").ToString()
                Dim sRMAR_COMPNO As String = dtReceiveList.Rows(i)("RMAR_COMPNO").ToString()
                If ((sUI_RMADSTATUS.Trim() = "50" Or sUI_RMADSTATUS.Trim() = "60")) Then
                    'If sUI_Status.Trim() = _oLanguage.getText("Common", "068", ctlLanguage.eumType.Status) Then
                    If IsNothing(hsSelectID) = False Then
                        hsSelectID.Remove(sUI_RMADID)
                    End If
                    If sender.Checked Then
                        hsSelectID.Add(sUI_RMADID, sUI_SERIALNO)
                    End If

                    If sRMAR_COMPNO.Trim <> "" Then
                        Dim sRepairCenter As String = Session("_RepairCenter")
                        Dim sInRepairCenter As String = ""
                        Dim arrRepair() As String = sRepairCenter.Split(",")
                        Dim j As Integer = 0
                        For j = 0 To arrRepair.Length - 1
                            If sInRepairCenter <> "" Then
                                sInRepairCenter = sInRepairCenter + ","
                            End If
                            sInRepairCenter = sInRepairCenter + "'" + arrRepair(j).Trim() + "'"
                        Next

                        If sInRepairCenter.IndexOf("'" + sRMAR_COMPNO + "'") < 0 Then
                            hsSelectID.Remove(sUI_RMADID)
                        End If
                    End If
                End If
            Next
        End If
        ViewState("hsSelectID") = hsSelectID

        For i = 0 To Me.UI_dvRequest.Rows.Count - 1
            If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                If UI_Check.Visible Then
                    UI_Check.Checked = sender.Checked
                End If
            End If
        Next
        'Bind First Serial Number Version
        CalTotalAmt()
    End Sub

    Protected Sub UI_Check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Keep_RepairDetail_Data()
        Dim SerialNumber As String = ""
        Dim i As Integer = 0
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        For i = 0 To Me.UI_dvRequest.Rows.Count - 1
            If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                If CType(sender, CheckBox) Is UI_Check Then
                    Dim UI_RMADID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMADID")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")
                    If IsNothing(hsSelectID) = False Then
                        hsSelectID.Remove(UI_RMADID.Text.Trim())
                    End If

                    If UI_Check.Checked Then
                        SerialNumber = UI_SERIALNO.Text.Trim()
                        hsSelectID.Add(UI_RMADID.Text.Trim(), UI_SERIALNO.Text.Trim())
                    End If
                End If
            End If
        Next
        ViewState("hsSelectID") = hsSelectID
        'Bind All This Serial Number Version No
        'If SerialNumber <> "" Then
        'GetVersigonData(SerialNumber)
        'Else
        'GetVersigonData(UI_lblSerialText.Text.Trim())
        'End If
        CalTotalAmt()
    End Sub

#End Region







    Protected Sub UI_cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim nSerialNumber As Integer = 1
        Dim sSerialNumber As String = ""

        Dim oRepair As New ctlRMA.Repair
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable
        Dim Item As DictionaryEntry
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")
        Dim bSaveSn As Boolean = False

        Try
            Call Keep_RepairDetail_Data()
            Call CalTotalAmt()
            Dim bIncludeTop As Boolean = False

            Dim sRecord As String = UI_lblPreviousPage_RMADID.Text.Trim()
            Dim bIsIncludeTop As Boolean = False
            sSerialNumber = "S/N:" + UI_lblSerialText.Text.Trim()
            For Each Item In hsSelectID
                bSaveSn = False
                Dim sUI_RMADID As String = Item.Key.ToString()
                Dim sUI_SERIALNO As String = Item.Value.ToString()
                If sRecord = sUI_RMADID.Trim Then
                    bIncludeTop = True
                    bSaveSn = True
                Else
                    sSerialNumber += ";" + sUI_SERIALNO
                    nSerialNumber += 1
                End If
                UI_lblPreviousPage_RMADID.Text = sUI_RMADID.Trim()

                Dim iTotalPrice As Double = 0
                dtRepairDetail = Save_RMARepairDetail(iTotalPrice, sUI_RMADID.Trim(), bSaveSn)
                dtRMARepair = Save_RMARepair(iTotalPrice, sRecord)
                oRepair.Save(dtRMARepair, dtRepairDetail, False)
            Next


            '如果选择的项目不含此SN
            UI_lblPreviousPage_RMADID.Text = sRecord
            If bIncludeTop = False Then
                Dim iTotalPrice As Double = 0
                dtRepairDetail = Save_RMARepairDetail(iTotalPrice, sRecord, True)
                dtRMARepair = Save_RMARepair(iTotalPrice, sRecord)
                oRepair.Save(dtRMARepair, dtRepairDetail, False)
            End If

            blnFlag = True


        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                'Dim oHashtable As New System.Collections.Hashtable
                'oHashtable = Me.ViewState("_HistoryKey")
                'Session("_PreviousPage") = oHashtable

                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.ProcessOK)
                'Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Transfer, "Repair_WorkList.aspx")
                Me.ucMessage.showMessageByFailed(sMsg)
                Call QueryDataByHead()
                Call QueryDataByDetail()
                Call QueryDataByStatusPoint()
                Call QueryData(UI_dvRequest.PageIndex)
            End If

        End Try
    End Sub

    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click
        Response.Redirect("Repair_WorkList.aspx")
    End Sub

    Protected Sub btnQuickSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            dtRequest = Session("_dtRequest")
            Dim sQuickSn As String = UI_txtSN.Text.Trim()
            dtRequest.DefaultView.RowFilter = "RMAD_SERIALNO='" + sQuickSn + "'"
            If dtRequest.DefaultView.Count > 0 Then
                sMessage = _oLanguage.getText("RMA", "236", ctlLanguage.eumType.Validator)
                Dim sUI_RMADID As String = dtRequest.DefaultView(0)("RMAD_ID").ToString()
                Dim sUI_RMANO As String = dtRequest.DefaultView(0)("RMAD_RMANO").ToString()
                Dim sUI_RMADSTATUS As String = dtRequest.DefaultView(0)("RMAD_STATUS").ToString()
                Dim sUI_RMAR_COMPNO As String = dtRequest.DefaultView(0)("RMAR_COMPNO").ToString()
                Dim bOK As Boolean = False
                If sUI_RMADSTATUS.Trim() = "50" Or sUI_RMADSTATUS.Trim() = "60" Then
                    bOK = True
                    blnFlag = True

                    If sUI_RMAR_COMPNO.Trim <> "" Then
                        Dim sRepairCenter As String = Session("_RepairCenter")
                        Dim sInRepairCenter As String = ""
                        Dim arrRepair() As String = sRepairCenter.Split(",")
                        Dim i As Integer = 0
                        For i = 0 To arrRepair.Length - 1
                            If sInRepairCenter <> "" Then
                                sInRepairCenter = sInRepairCenter + ","
                            End If
                            sInRepairCenter = sInRepairCenter + "'" + arrRepair(i).Trim() + "'"
                        Next

                        If sInRepairCenter.IndexOf("'" + sUI_RMAR_COMPNO.Trim() + "'") < 0 Then
                            bOK = False
                            blnFlag = False
                        End If
                    End If
                End If

                If bOK Then
                    Me.UI_lblPreviousPage_RMADID.Text = sUI_RMADID.Trim()
                    Me.UI_lblPreviousPage_RMANO.Text = sUI_RMANO.Trim()

                    Call QueryDataByHead()
                    Call QueryDataByDetail()
                    Call QueryDataByStatusPoint()
                    Call QueryData(UI_dvRequest.PageIndex)
                    CalTotalAmt()
                    UI_cmdSubmit.Enabled = True
                    UI_cmdApply.Enabled = True
                End If
            Else
                Throw New ArgumentException(_oLanguage.getText("RMA", "235", ctlLanguage.eumType.Validator))
            End If
        Catch ex As Exception
            sMessage = ex.Message
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    Protected Sub UI_txtSN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        btnQuickSearch_Click(sender, e)
    End Sub






    Protected Sub btnQA01_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "01"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA02_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "02"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA03_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "03"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA04_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "04"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA05_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "05"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA06_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "06"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA07_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "07"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA08_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "08"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA09_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "09"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA10_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "10"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA11_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "11"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQA12_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "12"
        BindPopUpPartNo()
    End Sub

    Protected Sub btnQB01_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "13"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQB02_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "14"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQB03_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "15"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQB04_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "16"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQB05_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "17"
        BindPopUpPartNo()
    End Sub
    Protected Sub btnQB06_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        sSelectNo = "18"
        BindPopUpPartNo()
    End Sub




    Private Sub BindPopUpPartNo()
        Dim sMessage As String = ""
        Try
            ViewState("sSelectNo") = sSelectNo
            If ViewState("sSerialNo") <> Nothing Then
                Dim sSerialNo As String = ViewState("sSerialNo").ToString().Trim()
                Dim oClient As New ctlRMA.Client
                Dim dtVerItem As New RmaDTO.dtSrVerBankDataTable
                dtVerItem = oClient.QrySrVerBank(sSerialNo)
                lstPartNoSelect.DataSource = dtVerItem
                lstPartNoSelect.DataBind()
                mdlPupSelectPart.Show()
            End If
        Catch ex As Exception
            sMessage = ex.Message
            Me.ucMessage.showMessageByFailed(sMessage)
        Finally
        End Try
    End Sub

    Private Sub GetVersigonData(ByVal sDefaultSerialNumber As String)
        Dim sMessage As String = ""
        Try
            ResetValue()
            Dim hsSelectID As Hashtable = ViewState("hsSelectID")
            Dim sUI_SERIALNO As String = sDefaultSerialNumber
            'Get Serial Number
            If sUI_SERIALNO = "" Then
                Dim Item As DictionaryEntry
                For Each Item In hsSelectID
                    sUI_SERIALNO = Item.Value.ToString()
                Next
            End If

            If sUI_SERIALNO <> "" Then
                'Get All Column Name
                Dim oClient As New ctlRMA.Client
                Dim dtExport As New RmaDTO.dtExportDataTable
                Dim sPartNo As String = ""
                dtExport = oClient.QryExport(sUI_SERIALNO)
                If dtExport.Rows.Count > 0 Then
                    sPartNo = dtExport.Rows(0)("export_partno").ToString().Trim()
                    Dim sSerialNo As String = dtExport.Rows(0)("e_sr").ToString().Trim()
                    ViewState("sSerialNo") = sSerialNo
                    'Current Version
                    txtA01.Text = dtExport.Rows(0)("e_01").ToString().Trim()
                    lblOA01.Text = dtExport.Rows(0)("m_01").ToString().Trim()
                    txtA02.Text = dtExport.Rows(0)("e_02").ToString().Trim()
                    lblOA02.Text = dtExport.Rows(0)("m_02").ToString().Trim()
                    txtA03.Text = dtExport.Rows(0)("e_03").ToString().Trim()
                    lblOA03.Text = dtExport.Rows(0)("m_03").ToString().Trim()
                    txtA04.Text = dtExport.Rows(0)("e_04").ToString().Trim()
                    lblOA04.Text = dtExport.Rows(0)("m_04").ToString().Trim()
                    txtA05.Text = dtExport.Rows(0)("e_05").ToString().Trim()
                    lblOA05.Text = dtExport.Rows(0)("m_05").ToString().Trim()
                    txtA06.Text = dtExport.Rows(0)("e_06").ToString().Trim()
                    lblOA06.Text = dtExport.Rows(0)("m_06").ToString().Trim()
                    txtA07.Text = dtExport.Rows(0)("e_07").ToString().Trim()
                    lblOA07.Text = dtExport.Rows(0)("m_07").ToString().Trim()
                    txtA08.Text = dtExport.Rows(0)("e_08").ToString().Trim()
                    lblOA08.Text = dtExport.Rows(0)("m_08").ToString().Trim()
                    txtA09.Text = dtExport.Rows(0)("e_09").ToString().Trim()
                    lblOA09.Text = dtExport.Rows(0)("m_09").ToString().Trim()
                    txtA10.Text = dtExport.Rows(0)("e_10").ToString().Trim()
                    lblOA10.Text = dtExport.Rows(0)("m_10").ToString().Trim()
                    txtA11.Text = dtExport.Rows(0)("e_11").ToString().Trim()
                    lblOA11.Text = dtExport.Rows(0)("m_11").ToString().Trim()
                    txtA12.Text = dtExport.Rows(0)("e_12").ToString().Trim()
                    lblOA12.Text = dtExport.Rows(0)("m_12").ToString().Trim()

                    txtB01.Text = dtExport.Rows(0)("e_13").ToString().Trim()
                    lblOB01.Text = dtExport.Rows(0)("m_13").ToString().Trim()
                    txtB02.Text = dtExport.Rows(0)("e_14").ToString().Trim()
                    lblOB02.Text = dtExport.Rows(0)("m_14").ToString().Trim()
                    txtB03.Text = dtExport.Rows(0)("e_15").ToString().Trim()
                    lblOB03.Text = dtExport.Rows(0)("m_15").ToString().Trim()
                    txtB04.Text = dtExport.Rows(0)("e_16").ToString().Trim()
                    lblOB04.Text = dtExport.Rows(0)("m_16").ToString().Trim()
                    txtB05.Text = dtExport.Rows(0)("e_17").ToString().Trim()
                    lblOB05.Text = dtExport.Rows(0)("m_17").ToString().Trim()
                    txtB06.Text = dtExport.Rows(0)("e_18").ToString().Trim()
                    lblOB06.Text = dtExport.Rows(0)("m_18").ToString().Trim()

                    lblA01.Text = dtExport.Rows(0)("n_01").ToString().Trim()
                    lblA02.Text = dtExport.Rows(0)("n_02").ToString().Trim()
                    lblA03.Text = dtExport.Rows(0)("n_03").ToString().Trim()
                    lblA04.Text = dtExport.Rows(0)("n_04").ToString().Trim()
                    lblA05.Text = dtExport.Rows(0)("n_05").ToString().Trim()
                    lblA06.Text = dtExport.Rows(0)("n_06").ToString().Trim()
                    lblA07.Text = dtExport.Rows(0)("n_07").ToString().Trim()
                    lblA08.Text = dtExport.Rows(0)("n_08").ToString().Trim()
                    lblA09.Text = dtExport.Rows(0)("n_09").ToString().Trim()
                    lblA10.Text = dtExport.Rows(0)("n_10").ToString().Trim()
                    lblA11.Text = dtExport.Rows(0)("n_11").ToString().Trim()
                    lblA12.Text = dtExport.Rows(0)("n_12").ToString().Trim()
                    lblB01.Text = dtExport.Rows(0)("n_13").ToString().Trim()
                    lblB02.Text = dtExport.Rows(0)("n_14").ToString().Trim()
                    lblB03.Text = dtExport.Rows(0)("n_15").ToString().Trim()
                    lblB04.Text = dtExport.Rows(0)("n_16").ToString().Trim()
                    lblB05.Text = dtExport.Rows(0)("n_17").ToString().Trim()
                    lblB06.Text = dtExport.Rows(0)("n_18").ToString().Trim()

                End If
                'Get Current Version
                If sPartNo <> Nothing Then
                    Dim dtTcOae As New RmaDTO.dtTcOaeFileDataTable
                    dtTcOae = oClient.QryTcOaeFile(sPartNo)
                    If dtTcOae.Rows.Count > 0 Then
                        txtQA01.Text = dtTcOae.Rows(0)("FD_01").ToString().Trim()
                        lblNA01.Text = dtTcOae.Rows(0)("m_01").ToString().Trim()

                        txtQA02.Text = dtTcOae.Rows(0)("FD_02").ToString().Trim()
                        lblNA02.Text = dtTcOae.Rows(0)("m_02").ToString().Trim()

                        txtQA03.Text = dtTcOae.Rows(0)("FD_03").ToString().Trim()
                        lblNA03.Text = dtTcOae.Rows(0)("m_03").ToString().Trim()

                        txtQA04.Text = dtTcOae.Rows(0)("FD_04").ToString().Trim()
                        lblNA04.Text = dtTcOae.Rows(0)("m_04").ToString().Trim()

                        txtQA05.Text = dtTcOae.Rows(0)("FD_05").ToString().Trim()
                        lblNA05.Text = dtTcOae.Rows(0)("m_05").ToString().Trim()

                        txtQA06.Text = dtTcOae.Rows(0)("FD_06").ToString().Trim()
                        lblNA06.Text = dtTcOae.Rows(0)("m_06").ToString().Trim()

                        txtQA07.Text = dtTcOae.Rows(0)("FD_07").ToString().Trim()
                        lblNA07.Text = dtTcOae.Rows(0)("m_07").ToString().Trim()

                        txtQA08.Text = dtTcOae.Rows(0)("FD_08").ToString().Trim()
                        lblNA08.Text = dtTcOae.Rows(0)("m_08").ToString().Trim()

                        txtQA09.Text = dtTcOae.Rows(0)("FD_09").ToString().Trim()
                        lblNA09.Text = dtTcOae.Rows(0)("m_09").ToString().Trim()

                        txtQA10.Text = dtTcOae.Rows(0)("FD_10").ToString().Trim()
                        lblNA10.Text = dtTcOae.Rows(0)("m_10").ToString().Trim()

                        txtQA11.Text = dtTcOae.Rows(0)("FD_11").ToString().Trim()
                        lblNA11.Text = dtTcOae.Rows(0)("m_11").ToString().Trim()

                        txtQA12.Text = dtTcOae.Rows(0)("FD_12").ToString().Trim()
                        lblNA12.Text = dtTcOae.Rows(0)("m_12").ToString().Trim()

                        txtQB01.Text = dtTcOae.Rows(0)("FD_13").ToString().Trim()
                        lblNB01.Text = dtTcOae.Rows(0)("m_13").ToString().Trim()

                        txtQB02.Text = dtTcOae.Rows(0)("FD_14").ToString().Trim()
                        lblNB02.Text = dtTcOae.Rows(0)("m_14").ToString().Trim()

                        txtQB03.Text = dtTcOae.Rows(0)("FD_15").ToString().Trim()
                        lblNB03.Text = dtTcOae.Rows(0)("m_15").ToString().Trim()

                        txtQB04.Text = dtTcOae.Rows(0)("FD_16").ToString().Trim()
                        lblNB04.Text = dtTcOae.Rows(0)("m_16").ToString().Trim()

                        txtQB05.Text = dtTcOae.Rows(0)("FD_17").ToString().Trim()
                        lblNB05.Text = dtTcOae.Rows(0)("m_17").ToString().Trim()

                        txtQB06.Text = dtTcOae.Rows(0)("FD_18").ToString().Trim()
                        lblNB06.Text = dtTcOae.Rows(0)("m_18").ToString().Trim()

                        If txtQA01.Text <> txtA01.Text Then
                            lblA01.ForeColor = Drawing.Color.Red
                            lblA01.Font.Bold = True
                        End If

                        If txtQA02.Text <> txtA02.Text Then
                            lblA02.ForeColor = Drawing.Color.Red
                            lblA02.Font.Bold = True
                        End If

                        If txtQA03.Text <> txtA03.Text Then
                            lblA03.ForeColor = Drawing.Color.Red
                            lblA03.Font.Bold = True
                        End If

                        If txtQA04.Text <> txtA04.Text Then
                            lblA04.ForeColor = Drawing.Color.Red
                            lblA04.Font.Bold = True
                        End If

                        If txtQA05.Text <> txtA05.Text Then
                            lblA05.ForeColor = Drawing.Color.Red
                            lblA05.Font.Bold = True
                        End If

                        If txtQA06.Text <> txtA06.Text Then
                            lblA06.ForeColor = Drawing.Color.Red
                            lblA06.Font.Bold = True
                        End If

                        If txtQA07.Text <> txtA07.Text Then
                            lblA07.ForeColor = Drawing.Color.Red
                            lblA07.Font.Bold = True
                        End If

                        If txtQA08.Text <> txtA08.Text Then
                            lblA08.ForeColor = Drawing.Color.Red
                            lblA08.Font.Bold = True
                        End If

                        If txtQA09.Text <> txtA09.Text Then
                            lblA09.ForeColor = Drawing.Color.Red
                            lblA09.Font.Bold = True
                        End If

                        If txtQA10.Text <> txtA10.Text Then
                            lblA10.ForeColor = Drawing.Color.Red
                            lblA10.Font.Bold = True
                        End If

                        If txtQA11.Text <> txtA11.Text Then
                            lblA11.ForeColor = Drawing.Color.Red
                            lblA11.Font.Bold = True
                        End If

                        If txtQA12.Text <> txtA12.Text Then
                            lblA12.ForeColor = Drawing.Color.Red
                            lblA12.Font.Bold = True
                        End If

                        If txtQB01.Text <> txtB01.Text Then
                            lblB01.ForeColor = Drawing.Color.Red
                            lblB01.Font.Bold = True
                        End If

                        If txtQB02.Text <> txtB02.Text Then
                            lblB02.ForeColor = Drawing.Color.Red
                            lblB02.Font.Bold = True
                        End If

                        If txtQB03.Text <> txtB03.Text Then
                            lblB03.ForeColor = Drawing.Color.Red
                            lblB03.Font.Bold = True
                        End If

                        If txtQB04.Text <> txtB04.Text Then
                            lblB04.ForeColor = Drawing.Color.Red
                            lblB04.Font.Bold = True
                        End If

                        If txtQB05.Text <> txtB05.Text Then
                            lblB05.ForeColor = Drawing.Color.Red
                            lblB05.Font.Bold = True
                        End If

                        If txtQB06.Text <> txtB06.Text Then
                            lblB06.ForeColor = Drawing.Color.Red
                            lblB06.Font.Bold = True
                        End If

                    End If
                End If

                'If txtA01.Text <> txtQA01.Text Or txtA02.Text <> txtQA02.Text Or txtA03.Text <> txtQA03.Text Or txtA04.Text <> txtQA04.Text Or txtA05.Text <> txtQA05.Text Or txtA06.Text <> txtQA06.Text Or txtA07.Text <> txtQA07.Text Or txtA08.Text <> txtQA08.Text Or txtA09.Text <> txtQA09.Text Or txtA10.Text <> txtQA10.Text Or txtA11.Text <> txtQA11.Text Or txtA12.Text <> txtQA12.Text Or txtB01.Text <> txtQB01.Text Or txtB02.Text <> txtQB02.Text Or txtB03.Text <> txtQB03.Text Or txtB04.Text <> txtQB04.Text Or txtB05.Text <> txtQB05.Text Or txtB06.Text <> txtQB06.Text Then
                ' UI_cmdVerApply.Visible = True
                'Else
                '    UI_cmdVerApply.Visible = False
                'End If


            End If

        Catch ex As Exception
            sMessage = ex.Message
            Me.ucMessage.showMessageByFailed(sMessage)
        Finally
            'Me.ucMessage.showMessageByFailed(sMessage)
        End Try
    End Sub

    Protected Sub UI_cmdVerApply_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If UI_CheckVer.Checked Then
                'If txtA01.Text <> txtQA01.Text Or txtA02.Text <> txtQA02.Text Or txtA03.Text <> txtQA03.Text Or txtA04.Text <> txtQA04.Text Or txtA05.Text <> txtQA05.Text Or txtA06.Text <> txtQA06.Text Or txtA07.Text <> txtQA07.Text Or txtA08.Text <> txtQA08.Text Or txtA09.Text <> txtQA09.Text Or txtA10.Text <> txtQA10.Text Or txtA11.Text <> txtQA11.Text Or txtA12.Text <> txtQA12.Text Or txtB01.Text <> txtQB01.Text Or txtB02.Text <> txtQB02.Text Or txtB03.Text <> txtQB03.Text Or txtB04.Text <> txtQB04.Text Or txtB05.Text <> txtQB05.Text Or txtB06.Text <> txtQB06.Text Then
                Dim oClient As New ctlRMA.Client
                Dim dtExport As New RmaDTO.dtExportUpdateDataTable
                Dim dtRmaVerLog As New RmaDTO.dtRmaVerLogInsertDataTable

                dtExport = SaveExportVer()
                dtRmaVerLog = SaveRmaVer()
                oClient.Update_ExportVer(dtExport, dtRmaVerLog)
                GetVersigonData(ViewState("hSerialNumber").ToString())
                'End If
            End If
        Catch ex As Exception
            Dim sMessage As String = ex.Message
            Me.ucMessage.showMessageByFailed(sMessage)
        End Try
    End Sub

    Private Function SaveExportVer() As RmaDTO.dtExportUpdateDataTable
        Dim oExport As New ctlRMA.Export
        Dim dtExport As New RmaDTO.dtExportUpdateDataTable
        Try
            Dim hsSelectIDTmp As New Hashtable
            Dim i As Integer = 0
            For i = 0 To Me.UI_dvRequest.Rows.Count - 1
                If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")
                    If UI_Check.Visible And UI_Check.Checked Then
                        hsSelectIDTmp.Add(UI_SERIALNO.Text, UI_SERIALNO.Text)
                    End If
                End If
            Next

            Dim sUI_SERIALNO As String = ""

            If ViewState("hSerialNumber").ToString() <> "" Then
                Dim bFind As Boolean = False
                Dim ItemTmp As DictionaryEntry
                For Each ItemTmp In hsSelectIDTmp
                    If ItemTmp.Value.ToString().Trim() = ViewState("hSerialNumber").ToString().Trim() Then
                        bFind = True
                    End If
                Next
                If bFind = False Then
                    hsSelectIDTmp.Add(ViewState("hSerialNumber").ToString(), ViewState("hSerialNumber").ToString())
                End If
            End If

            'Get Serial Number
            Dim Item As DictionaryEntry
            For Each Item In hsSelectIDTmp
                sUI_SERIALNO = Item.Value.ToString()
                Dim drExport As RmaDTO.dtExportUpdateRow = dtExport.NewdtExportUpdateRow()
                drExport.export_serialno = sUI_SERIALNO
                drExport.EXPORT_FD_01 = txtQA01.Text.Trim()
                drExport.EXPORT_FD_02 = txtQA02.Text.Trim()
                drExport.EXPORT_FD_03 = txtQA03.Text.Trim()
                drExport.EXPORT_FD_04 = txtQA04.Text.Trim()
                drExport.EXPORT_FD_05 = txtQA05.Text.Trim()
                drExport.EXPORT_FD_06 = txtQA06.Text.Trim()
                drExport.EXPORT_FD_07 = txtQA07.Text.Trim()
                drExport.EXPORT_FD_08 = txtQA08.Text.Trim()
                drExport.EXPORT_FD_09 = txtQA09.Text.Trim()
                drExport.EXPORT_FD_10 = txtQA10.Text.Trim()
                drExport.EXPORT_FD_11 = txtQA11.Text.Trim()
                drExport.EXPORT_FD_12 = txtQA12.Text.Trim()
                drExport.EXPORT_FD_13 = txtQB01.Text.Trim()
                drExport.EXPORT_FD_14 = txtQB02.Text.Trim()
                drExport.EXPORT_FD_15 = txtQB03.Text.Trim()
                drExport.EXPORT_FD_16 = txtQB04.Text.Trim()
                drExport.EXPORT_FD_17 = txtQB05.Text.Trim()
                drExport.EXPORT_FD_18 = txtQB06.Text.Trim()
                dtExport.AdddtExportUpdateRow(drExport)
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return dtExport
    End Function

    Private Function SaveRmaVer() As RmaDTO.dtRmaVerLogInsertDataTable
        Dim oExport As New ctlRMA.Export
        Dim dtRmaVerLog As New RmaDTO.dtRmaVerLogInsertDataTable
        Try
            Dim hsSelectIDTmp As New Hashtable
            Dim i As Integer = 0
            For i = 0 To Me.UI_dvRequest.Rows.Count - 1
                If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")
                    Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")
                    If UI_Check.Visible And UI_Check.Checked Then
                        hsSelectIDTmp.Add(UI_SERIALNO.Text, UI_SERIALNO.Text)
                    End If
                End If
            Next
            Dim sUI_SERIALNO As String = ""

            If ViewState("hSerialNumber").ToString() <> "" Then
                Dim bFind As Boolean = False
                Dim ItemTmp As DictionaryEntry
                For Each ItemTmp In hsSelectIDTmp
                    If ItemTmp.Value.ToString().Trim() = ViewState("hSerialNumber").ToString().Trim() Then
                        bFind = True
                    End If
                Next
                If bFind = False Then
                    hsSelectIDTmp.Add(ViewState("hSerialNumber").ToString(), ViewState("hSerialNumber").ToString())
                End If
            End If

            Dim oClient As New ctlRMA.Client
            Dim dtExport As New RmaDTO.dtExportDataTable
            'Get Serial Number
            Dim Item As DictionaryEntry
            Dim sSerialNo As String = ""
            For Each Item In hsSelectIDTmp
                sUI_SERIALNO = Item.Value.ToString()
                Dim drLog As RmaDTO.dtRmaVerLogInsertRow = dtRmaVerLog.NewdtRmaVerLogInsertRow()

                Dim sPartNo As String = ""
                dtExport = oClient.QryExport(sUI_SERIALNO)
                If dtExport.Rows.Count > 0 Then
                    sPartNo = dtExport.Rows(0)("export_partno").ToString().Trim()
                    If sSerialNo <> "" Then
                        If sSerialNo <> dtExport.Rows(0)("e_sr").ToString().Trim() Then
                            Throw New Exception(_oLanguage.getText("RMA", "325", ctlLanguage.eumType.Tag))
                        End If
                    Else
                        sSerialNo = dtExport.Rows(0)("e_sr").ToString().Trim()
                    End If

                    drLog.RMAVL_OLD_FD_01 = dtExport.Rows(0)("e_01").ToString().Trim()
                    drLog.RMAVL_OLD_FD_02 = dtExport.Rows(0)("e_02").ToString().Trim()
                    drLog.RMAVL_OLD_FD_03 = dtExport.Rows(0)("e_03").ToString().Trim()
                    drLog.RMAVL_OLD_FD_04 = dtExport.Rows(0)("e_04").ToString().Trim()
                    drLog.RMAVL_OLD_FD_05 = dtExport.Rows(0)("e_05").ToString().Trim()
                    drLog.RMAVL_OLD_FD_06 = dtExport.Rows(0)("e_06").ToString().Trim()
                    drLog.RMAVL_OLD_FD_07 = dtExport.Rows(0)("e_07").ToString().Trim()
                    drLog.RMAVL_OLD_FD_08 = dtExport.Rows(0)("e_08").ToString().Trim()
                    drLog.RMAVL_OLD_FD_09 = dtExport.Rows(0)("e_09").ToString().Trim()
                    drLog.RMAVL_OLD_FD_10 = dtExport.Rows(0)("e_10").ToString().Trim()
                    drLog.RMAVL_OLD_FD_11 = dtExport.Rows(0)("e_11").ToString().Trim()
                    drLog.RMAVL_OLD_FD_12 = dtExport.Rows(0)("e_12").ToString().Trim()
                    drLog.RMAVL_OLD_FD_13 = dtExport.Rows(0)("e_13").ToString().Trim()
                    drLog.RMAVL_OLD_FD_14 = dtExport.Rows(0)("e_14").ToString().Trim()
                    drLog.RMAVL_OLD_FD_15 = dtExport.Rows(0)("e_15").ToString().Trim()
                    drLog.RMAVL_OLD_FD_16 = dtExport.Rows(0)("e_16").ToString().Trim()
                    drLog.RMAVL_OLD_FD_17 = dtExport.Rows(0)("e_17").ToString().Trim()
                    drLog.RMAVL_OLD_FD_18 = dtExport.Rows(0)("e_18").ToString().Trim()
                End If

                drLog.RMAVL_SERIALNO = sUI_SERIALNO
                drLog.RMAVL_DATETIME = DateTime.Now
                drLog.RMAVL_PNO = sPartNo
                drLog.RMAVL_SR = sSerialNo

                drLog.RMAVL_NEW_FD_01 = txtQA01.Text.Trim()
                drLog.RMAVL_NEW_FD_02 = txtQA02.Text.Trim()
                drLog.RMAVL_NEW_FD_03 = txtQA03.Text.Trim()
                drLog.RMAVL_NEW_FD_04 = txtQA04.Text.Trim()
                drLog.RMAVL_NEW_FD_05 = txtQA05.Text.Trim()
                drLog.RMAVL_NEW_FD_06 = txtQA06.Text.Trim()
                drLog.RMAVL_NEW_FD_07 = txtQA07.Text.Trim()
                drLog.RMAVL_NEW_FD_08 = txtQA08.Text.Trim()
                drLog.RMAVL_NEW_FD_09 = txtQA09.Text.Trim()
                drLog.RMAVL_NEW_FD_10 = txtQA10.Text.Trim()
                drLog.RMAVL_NEW_FD_11 = txtQA11.Text.Trim()
                drLog.RMAVL_NEW_FD_12 = txtQA12.Text.Trim()
                drLog.RMAVL_NEW_FD_13 = txtQB01.Text.Trim()
                drLog.RMAVL_NEW_FD_14 = txtQB02.Text.Trim()
                drLog.RMAVL_NEW_FD_15 = txtQB03.Text.Trim()
                drLog.RMAVL_NEW_FD_16 = txtQB04.Text.Trim()
                drLog.RMAVL_NEW_FD_17 = txtQB05.Text.Trim()
                drLog.RMAVL_NEW_FD_18 = txtQB06.Text.Trim()
                drLog.RMAVL_AD = Session("_UserID")
                drLog.RMAVL_ADNAME = Session("_UserName")

                dtRmaVerLog.AdddtRmaVerLogInsertRow(drLog)
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return dtRmaVerLog
    End Function

    Private Sub ResetValue()
        lblA01.Text = ""
        lblA02.Text = ""
        lblA03.Text = ""
        lblA04.Text = ""
        lblA05.Text = ""
        lblA06.Text = ""
        lblA07.Text = ""
        lblA08.Text = ""
        lblA09.Text = ""
        lblA10.Text = ""
        lblA11.Text = ""
        lblA12.Text = ""
        lblB01.Text = ""
        lblB02.Text = ""
        lblB03.Text = ""
        lblB04.Text = ""
        lblB05.Text = ""
        lblB06.Text = ""

        txtA01.Text = ""
        txtA02.Text = ""
        txtA03.Text = ""
        txtA04.Text = ""
        txtA05.Text = ""
        txtA06.Text = ""
        txtA07.Text = ""
        txtA08.Text = ""
        txtA09.Text = ""
        txtA10.Text = ""
        txtA11.Text = ""
        txtA12.Text = ""
        txtB01.Text = ""
        txtB02.Text = ""
        txtB03.Text = ""
        txtB04.Text = ""
        txtB05.Text = ""
        txtB06.Text = ""

        lblOA01.Text = ""
        lblOA02.Text = ""
        lblOA03.Text = ""
        lblOA04.Text = ""
        lblOA05.Text = ""
        lblOA06.Text = ""
        lblOA07.Text = ""
        lblOA08.Text = ""
        lblOA09.Text = ""
        lblOA10.Text = ""
        lblOB01.Text = ""
        lblOB02.Text = ""
        lblOB03.Text = ""
        lblOB04.Text = ""
        lblOB05.Text = ""
        lblOB06.Text = ""

        txtQA01.Text = ""
        txtQA02.Text = ""
        txtQA03.Text = ""
        txtQA04.Text = ""
        txtQA05.Text = ""
        txtQA06.Text = ""
        txtQA07.Text = ""
        txtQA08.Text = ""
        txtQA09.Text = ""
        txtQA10.Text = ""
        txtQA11.Text = ""
        txtQA12.Text = ""
        txtQB01.Text = ""
        txtQB02.Text = ""
        txtQB03.Text = ""
        txtQB04.Text = ""
        txtQB05.Text = ""
        txtQB06.Text = ""

        lblNA01.Text = ""
        lblNA02.Text = ""
        lblNA03.Text = ""
        lblNA04.Text = ""
        lblNA05.Text = ""
        lblNA06.Text = ""
        lblNA07.Text = ""
        lblNA08.Text = ""
        lblNA09.Text = ""
        lblNA10.Text = ""
        lblNB01.Text = ""
        lblNB02.Text = ""
        lblNB03.Text = ""
        lblNB04.Text = ""
        lblNB05.Text = ""
        lblNB06.Text = ""

        lblA01.ForeColor = Drawing.Color.Black
        lblA01.Font.Bold = False

        lblA02.ForeColor = Drawing.Color.Black
        lblA02.Font.Bold = False

        lblA03.ForeColor = Drawing.Color.Black
        lblA03.Font.Bold = False

        lblA04.ForeColor = Drawing.Color.Black
        lblA04.Font.Bold = False

        lblA05.ForeColor = Drawing.Color.Black
        lblA05.Font.Bold = False

        lblA06.ForeColor = Drawing.Color.Black
        lblA06.Font.Bold = False

        lblA07.ForeColor = Drawing.Color.Black
        lblA07.Font.Bold = False

        lblA08.ForeColor = Drawing.Color.Black
        lblA08.Font.Bold = False

        lblA09.ForeColor = Drawing.Color.Black
        lblA09.Font.Bold = False

        lblA10.ForeColor = Drawing.Color.Black
        lblA10.Font.Bold = False

        lblA11.ForeColor = Drawing.Color.Black
        lblA11.Font.Bold = False

        lblA12.ForeColor = Drawing.Color.Black
        lblA12.Font.Bold = False

        lblB01.ForeColor = Drawing.Color.Black
        lblB01.Font.Bold = False

        lblB02.ForeColor = Drawing.Color.Black
        lblB02.Font.Bold = False

        lblB03.ForeColor = Drawing.Color.Black
        lblB03.Font.Bold = False

        lblB04.ForeColor = Drawing.Color.Black
        lblB04.Font.Bold = False

        lblB05.ForeColor = Drawing.Color.Black
        lblB05.Font.Bold = False

        lblB06.ForeColor = Drawing.Color.Black
        lblB06.Font.Bold = False
    End Sub

    Protected Sub UI_CheckVer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If UI_CheckVer.Checked Then
            pnlVersion.Visible = True
        Else
            pnlVersion.Visible = False
        End If
    End Sub

    Public Sub CheckedchkPopPartNoSelectChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sPartNo As String = ""
        Dim sPartName As String = ""
        If ViewState("sSelectNo") <> Nothing Then
            sSelectNo = ViewState("sSelectNo").ToString().Trim()
        End If

        Dim oItem As DataListItem
        For Each oItem In lstPartNoSelect.Items
            Dim chk As RadioButton = DirectCast(oItem.FindControl("chkPopPartNoSelect"), RadioButton)
            If chk.Checked Then
                sPartNo = (DirectCast(oItem.FindControl("lblPartNo"), HtmlInputHidden)).Value.Trim()
                sPartName = (DirectCast(oItem.FindControl("lblPartName"), HtmlInputHidden)).Value.Trim()
            End If
        Next

        Select Case sSelectNo
            Case "01"
                txtQA01.Text = sPartNo
                lblNA01.Text = sPartName
            Case "02"
                txtQA02.Text = sPartNo
                lblNA02.Text = sPartName
            Case "03"
                txtQA03.Text = sPartNo
                lblNA03.Text = sPartName
            Case "04"
                txtQA04.Text = sPartNo
                lblNA04.Text = sPartName
            Case "05"
                txtQA05.Text = sPartNo
                lblNA05.Text = sPartName
            Case "06"
                txtQA06.Text = sPartNo
                lblNA06.Text = sPartName
            Case "07"
                txtQA07.Text = sPartNo
                lblNA07.Text = sPartName
            Case "08"
                txtQA08.Text = sPartNo
                lblNA08.Text = sPartName
            Case "09"
                txtQA09.Text = sPartNo
                lblNA09.Text = sPartName
            Case "10"
                txtQA10.Text = sPartNo
                lblNA10.Text = sPartName
            Case "11"
                txtQA11.Text = sPartNo
                lblNA11.Text = sPartName
            Case "12"
                txtQA12.Text = sPartNo
                lblNA12.Text = sPartName
            Case "13"
                txtQB01.Text = sPartNo
                lblNB01.Text = sPartName
            Case "14"
                txtQB02.Text = sPartNo
                lblNB02.Text = sPartName
            Case "15"
                txtQB03.Text = sPartNo
                lblNB03.Text = sPartName
            Case "16"
                txtQB04.Text = sPartNo
                lblNB04.Text = sPartName
            Case "17"
                txtQB05.Text = sPartNo
                lblNB05.Text = sPartName
            Case "18"
                txtQB06.Text = sPartNo
                lblNB06.Text = sPartName
        End Select

        mdlPupSelectPart.Hide()
    End Sub

    Protected Sub btnPopSelectPartClose_Click(ByVal sender As Object, ByVal e As EventArgs)
        mdlPupSelectPart.Hide()
    End Sub




    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvRequest.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvRequest.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvRequest.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvRequest.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvRequest.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub



End Class

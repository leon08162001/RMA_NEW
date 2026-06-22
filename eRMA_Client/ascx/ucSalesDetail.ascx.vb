Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucSalesDetail
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Enum eumCommand As Integer
        AddNew = 1
        UPDATE = 2
    End Enum


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_RMADID") = ""
            Me.ViewState("_RMANO") = ""
            Me.ViewState("_SourceCompNo") = ""  'RMA 單申請的 維修中心

            Call setDefault()
        End If
    End Sub


    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Call setValidationMessage(Me.rfvSaleLaborCost)
        Call setValidationMessage(Me.rfvSaleMaterialCost)
        'Call setValidationMessage(Me.rfvSaleQuote)
        Call setValidationMessage(Me.rvSaleLaborCost)
        Call setValidationMessage(Me.rvSaleMaterialCost)
        'Call setValidationMessage(Me.rvSaleQuote)

        '取得Tag Text
        Me.UI_lblChargeInformation.Text = _oLanguage.getText("RMA", "130", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

        Me.UI_lblProductDesc.Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
        Me.UI_lblProblemDesc.Text = _oLanguage.getText("RMA", "122", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
        Me.UI_lblDiscount.Text = _oLanguage.getText("RMA", "240", ctlLanguage.eumType.Tag)

        Me.UI_lblCustomerFile.Text = _oLanguage.getText("RMA", "123", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborCost.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
        Me.UI_lblMaterialCost.Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalAmount.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.UI_lblTittel.Text = _oLanguage.getText("RMA", "128", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairedCharge.Text = _oLanguage.getText("RMA", "131", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairedQuote.Text = _oLanguage.getText("RMA", "132", ctlLanguage.eumType.Tag)
        Me.UI_lblSalesQuote.Text = _oLanguage.getText("RMA", "133", ctlLanguage.eumType.Tag)
        Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        Me.UI_lblImproperUsage.Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "010", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)

        Me.UI_txtSaleLaborCost.Attributes.Add("onkeyup", "calTotalAMT()")
        Me.UI_txtSaleMaterialCost.Attributes.Add("onkeyup", "calTotalAMT()")
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfvSaleLaborCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "074", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvSaleMaterialCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "075", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvSaleQuote".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "135", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvSaleLaborCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "076", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvSaleMaterialCost".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "077", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvSaleQuote".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "136", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    Private Sub ClearUIText()
        Me.UI_lblSerialText.Text = ""
        Me.UI_lblModelText.Text = ""
        Me.UI_lblFailureText.Text = ""

        Me.UI_lblProductDescText.Text = ""
        Me.UI_lblProblemDescText.Text = ""
        Me.UI_lblDescriptionText.Text = ""

        Me.UI_lblImproperUsageText.Text = ""
        Me.UI_DownloadFile.Text = ""
        Me.UI_DownloadFile.NavigateUrl = ""

        Me.UI_lblLaborCostText.Text = ""
        Me.UI_lblMaterialCostText.Text = ""
        Me.UI_lblTotalAmountText.Text = ""

        Me.UI_lblSaleLaborCost_Code.Text = ""
        Me.UI_lblSaleMaterialCost_Code.Text = ""
        Me.UI_lblSaleQuote_Code.Text = ""
        Me.UI_CurrencyCode.Text = ""

        Me.UI_lblRMASQID.Text = ""
        Me.UI_txtSaleLaborCost.Text = ""
        Me.UI_txtSaleMaterialCost.Text = ""
        Me.UI_txtSaleQuote.Text = ""
        Me.UI_txtRemark.Text = ""
    End Sub




    Private Sub QueryByRepairQuotedDetail()
        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        dtRepairQuotedDetail = oRepairQuoted.QueryByRepairQuotedDetail(Me.ViewState("_RMADID").ToString())
        Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail, 0)

    End Sub



    Private Sub RepairQuotedDetail_DataBind(ByVal dtRepairDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairQuotedDetail") = dtRepairDetail
        'Call RepairQuotedDetail_CalTotalAmt()

        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARQD_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub


    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHLocation As Label = e.Item.FindControl("lblHLocation")
            Dim lblHImproper As Label = e.Item.FindControl("lblHImproper")
            Dim lblHReason As Label = e.Item.FindControl("lblHReason")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblHPrice As Label = e.Item.FindControl("lblHPrice")

            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "098", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHLocation.Text = _oLanguage.getText("RMA", "100", ctlLanguage.eumType.Tag)
            lblHImproper.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
            lblHReason.Text = _oLanguage.getText("RMA", "102", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblHPrice.Text = _oLanguage.getText("RMA", "104", ctlLanguage.eumType.Tag)
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblNew As Label = e.Item.FindControl("lblNew")
            Dim lblOld As Label = e.Item.FindControl("lblOld")
            lblNew.Text = _oLanguage.getText("RMA", "105", ctlLanguage.eumType.Tag)
            lblOld.Text = _oLanguage.getText("RMA", "106", ctlLanguage.eumType.Tag)

            ' '' ''Dim lblIMPROPERUSAGE As Label = e.Item.FindControl("lblIMPROPERUSAGE")
            ' '' ''Dim sImproper As DropDownList = e.Item.FindControl("cboImproper")
            ' '' ''sImproper.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            ' '' ''sImproper.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            ' '' ''sImproper.SelectedValue = lblIMPROPERUSAGE.Text.Trim()

            ' '' ''Dim UI_cboDefective As DropDownList = e.Item.FindControl("UI_cboDefective")
            ' '' ''Dim lblDEFECTIVE As Label = e.Item.FindControl("lblDEFECTIVE")
            ' '' ''oCommon.getDefectiveByDropDownList(Session("_LanguageID"), UI_cboDefective)
            ' '' ''UI_cboDefective.SelectedValue = lblDEFECTIVE.Text.Trim()

            '' '' ''==========================================================================================================================================================
            '' '' ''計算金額
            '' '' ''==========================================================================================================================================================
            ' '' ''Dim txtQty As TextBox = e.Item.FindControl("txtQty")
            ' '' ''txtQty.Attributes.Add("onkeyup", "cal_subTotalAMT()")


            '' '' ''==========================================================================================================================================================
            '' '' ''檢核機制
            '' '' ''==========================================================================================================================================================
            ' '' ''Dim rfvNewPart As RequiredFieldValidator = e.Item.FindControl("rfvNewPart")
            ' '' ''Dim rfvNewSerial As RequiredFieldValidator = e.Item.FindControl("rfvNewSerial")
            ' '' ''Dim rvQty As RangeValidator = e.Item.FindControl("rvQty")
            '' '' ''Dim rvPrice As RangeValidator = e.Item.FindControl("rvPrice")
            ' '' ''Dim txtNewPart As TextBox = e.Item.FindControl("txtNewPart")
            ' '' ''Dim txtNewSerial As TextBox = e.Item.FindControl("txtNewSerial")
            '' '' ''Dim txtPrice As TextBox = e.Item.FindControl("txtPrice")

            ' '' ''rfvNewPart.ControlToValidate = txtNewPart.ID
            '' '' ''rfvNewSerial.ControlToValidate = txtNewSerial.ID
            ' '' ''rvQty.ControlToValidate = txtQty.ID
            '' '' ''rvPrice.ControlToValidate = txtPrice.ID

            ' '' ''Call setValidationMessage(rfvNewPart)
            '' '' ''Call setValidationMessage(rfvNewSerial)
            ' '' ''Call setValidationMessage(rvQty)
            '' '' ''Call setValidationMessage(rvPrice)


            '' '' ''    Me.UI_lblPartsTotal.Text = Convert.ToInt32(Me.UI_lblPartsTotal.Text.ToString().Trim()) + Convert.ToInt32(txtPrice.Text.ToString().Trim())
            '' '' ''    Me.UI_lblTotal.Text = Convert.ToInt32(Me.UI_lblPartsTotal.Text.ToString().Trim()) + Convert.ToInt32(Me.UI_lblLaborCost.Text.ToString().Trim())
        End If
    End Sub






    ''' <summary>
    ''' 業務維修報價資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryDataSale()
        Dim oSale As New ctlRMA.Sale
        Dim dtSale As New RmaDTO.RMASale_QuotedDataTable

        dtSale = oSale.QueryBySaleQuoted(Me.ViewState("_RMADID").ToString())

        Me.ViewState("_eumCommand") = eumCommand.AddNew

        If dtSale.Rows.Count > 0 Then
            Dim dr As RmaDTO.RMASALE_QUOTEDRow = dtSale.Rows(0)
            Me.ViewState("_eumCommand") = eumCommand.UPDATE

            Me.UI_lblRMASQID.Text = dr.RMASQ_ID.ToString().Trim()

            Me.UI_txtSaleLaborCost.Text = Convert.ToDecimal(dr.RMASQ_LABORCOST.ToString().Trim())
            Me.UI_txtSaleMaterialCost.Text = Convert.ToDecimal(dr.RMASQ_MATERIALCOST.ToString().Trim())
            Me.UI_txtSaleQuote.Text = Convert.ToDecimal(dr.RMASQ_QUOTE.ToString().Trim())

            '業務報價幣別
            Me.UI_lblSaleLaborCost_Code.Text = dr.RMASQ_CURRENCYCODE.Trim() & "&nbsp;&nbsp;"
            Me.UI_lblSaleMaterialCost_Code.Text = dr.RMASQ_CURRENCYCODE.Trim() & "&nbsp;&nbsp;"
            Me.UI_lblSaleQuote_Code.Text = dr.RMASQ_CURRENCYCODE.Trim() & "&nbsp;&nbsp;"
            Me.UI_CurrencyCode.Text = dr.RMASQ_CURRENCYCODE.Trim()

            '兌美金匯率
            Me.UI_CurrencyRate.Text = dr.RMASQ_CURRENCYRATE.ToString().Trim()

            If dr.IsRMASQ_DESCNull = False Then Me.UI_txtRemark.Text = dr.RMASQ_DESC.ToString().Trim()

        Else
            '===============================================================================================================================================
            '取得 業務報價 幣別, 及取得目前的匯率
            '===============================================================================================================================================
            Dim oCompany As New ctlCompany
            Dim dtCurrency As New CompanyDTO.vwCompany_CurrencyDataTable

            dtCurrency = oCompany.QueryByCurrency(Me.ViewState("_SourceCompNo").ToString())
            If dtCurrency.Rows.Count > 0 Then
                Dim dr As CompanyDTO.vwCompany_CurrencyRow = dtCurrency.Rows(0)

                '業務報價幣別
                Me.UI_lblSaleLaborCost_Code.Text = dr.CURRENCY_CODE.Trim() & "&nbsp;&nbsp;"
                Me.UI_lblSaleMaterialCost_Code.Text = dr.CURRENCY_CODE.Trim() & "&nbsp;&nbsp;"
                Me.UI_lblSaleQuote_Code.Text = dr.CURRENCY_CODE.Trim() & "&nbsp;&nbsp;"
                Me.UI_CurrencyCode.Text = dr.CURRENCY_CODE.Trim()

                '兌美金匯率
                Me.UI_CurrencyRate.Text = dr.CURRENCY_RATE.ToString().Trim()
            End If
        End If

    End Sub


    ''' <summary>
    ''' 品項資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryDataRepair()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepair As New RmaDTO.vwRepair_DetailDataTable

        Me.ViewState("_SourceCompNo") = ""
        dtRepair = oRepair.QueryByRMARepairDetail(Session("_LanguageID").ToString().Trim(), Me.ViewState("_RMADID").ToString())

        If dtRepair.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_DetailRow = dtRepair.Rows(0)
            Dim sArrRepaurFile As String = ""
            Dim sCurrencyCode As String = ""

            'RMA 單申請的 維修中心
            Me.ViewState("_SourceCompNo") = dr.RMA_COMPNO.Trim()

            If dr.IsRMAD_SERIALNONull = False Then Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelText.Text = dr.RMAD_MODELNO.ToString().Trim()

            If dr.IsFARC_NAME2Null = False Then
                Me.UI_lblFailureText.Text = dr.IsFARC_NAME2Null.ToString().Trim()
            Else
                If dr.IsFARC_NAME1Null = False Then Me.UI_lblFailureText.Text = dr.FARC_NAME1.ToString().Trim()
            End If

            If dr.IsRMAD_PRODUCTDESCNull = False Then Me.UI_lblProductDescText.Text = dr.RMAD_PRODUCTDESC.Trim()
            If dr.IsRMAR_PROBLEMDESCNull = False Then Me.UI_lblProblemDescText.Text = dr.RMAR_PROBLEMDESC.ToString().Trim()
            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_lblDescriptionText.Text = dr.RMAR_REPAIRDESC.ToString().Trim()

            If dr.IsRMARQ_IMPROPERUSAGENull = False Then
                '非正常使用: 0.No, 1.Yes
                Me.UI_lblImproperUsageText.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                If dr.RMARQ_IMPROPERUSAGE = 1 Then
                    Me.UI_lblImproperUsageText.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End If
            End If


            '===============================================================================================================================================
            '客戶的折扣
            '===============================================================================================================================================
            Me.UI_txtCU_DISCOUNT_OFF.Text = "1"
            If dr.IsRMA_CUNONull = False Then
                If dr.RMA_CUNO.Trim() <> "" Then
                    Dim dtCustomer As New CustomerDTO.vwCustomerDataTable

                    Dim ctlCustomer As New ctlCustomer.Customer
                    dtCustomer = ctlCustomer.QueryByCompany(dr.RMA_CUNO.Trim())
                    If dtCustomer.Count > 0 Then
                        Dim item As CustomerDTO.vwCustomerRow = dtCustomer.Rows(0)
                        If item.IsCU_DISCOUNT_OFFNull = False Then
                            Dim CU_DISCOUNT_OFF As Double = item.CU_DISCOUNT_OFF
                            Me.UI_lblDiscountText.Text = CU_DISCOUNT_OFF.ToString() + " OFF"
                            Me.UI_txtCU_DISCOUNT_OFF.Text = (100 - CU_DISCOUNT_OFF) / 100
                        End If
                    End If
                End If
            End If



            '===============================================================================================================================================
            '客戶上傳的檔案
            '===============================================================================================================================================
            If dr.IsRMAD_UPLOADFILENull = False Then sArrRepaurFile = dr.RMAD_UPLOADFILE.ToString().Trim()
            If sArrRepaurFile.Trim() <> "" Then
                Dim sRepaurFile As String() = sArrRepaurFile.ToString().Trim().Split(",")
                Me.UI_DownloadFile.Text = sRepaurFile(0).ToString().Trim()
                Me.UI_DownloadFile.NavigateUrl = _WEBURL & _Requested_VisualPath & sRepaurFile(1).ToString().Trim()
            End If


            '===============================================================================================================================================
            '金額轉換
            '維修報價
            '===============================================================================================================================================
            '幣別
            If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then sCurrencyCode = dr.RMARQ_ASSIGECURRENCYCODE.ToString().Trim()

            If dr.IsRMARQ_ASSIGLABORCOSTNull = False And dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then
                Dim iLABORCost As Double = 0
                iLABORCost = Math.Round(dr.RMARQ_ASSIGLABORCOST, 2)
                Me.UI_lblLaborCostText.Text = sCurrencyCode & "&nbsp;&nbsp;" & iLABORCost.ToString()
                Me.UI_txtLaborCost.Text = iLABORCost.ToString()
            End If

            '零件費用
            If dr.IsRMARQ_ASSIGMATERIALCOSTNull = False And dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then
                Dim iMaterialCost As Double = 0
                iMaterialCost = Math.Round(dr.RMARQ_ASSIGMATERIALCOST, 2)
                Me.UI_lblMaterialCostText.Text = sCurrencyCode & "&nbsp;&nbsp;" & iMaterialCost.ToString()
                Me.UI_txtMaterialCost.Text = iMaterialCost.ToString()
            End If

            '維修總價
            If dr.IsRMARQ_ASSIGEQUOTENull = False And dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then
                Dim iQUOTE As Double = 0
                iQUOTE = Math.Round(dr.RMARQ_ASSIGEQUOTE, 2)
                Me.UI_lblTotalAmountText.Text = sCurrencyCode & "&nbsp;&nbsp;" & iQUOTE.ToString()
            End If

        End If
    End Sub



#Region "取得維修上傳檔案資料"
    Private Sub QueryDataRepairUpload()
        Dim oRepairUpload As New ctlRMA.Repair
        Dim dtRepairUpload As New RmaDTO.tmpRepairUploadDataTable

        dtRepairUpload = oRepairUpload.QueryByUpload_Group(Me.ViewState("_RMANO").ToString())

        Me.UI_panReportFile.Visible = False
        If dtRepairUpload.Rows.Count > 0 Then
            Me.UI_panReportFile.Visible = True
        End If

        Me.UI_dvRepairUpload.DataSource = dtRepairUpload
        Me.UI_dvRepairUpload.DataBind()
    End Sub

    Protected Sub UI_dvRepairUpload_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairUpload.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim sArrRepaurFile1 As String = ""
            Dim sArrRepaurFile2 As String = ""
            Dim sArrRepaurFile3 As String = ""
            Dim UI_lblSeqID1 As Label = e.Item.FindControl("UI_lblSeqID1")
            Dim UI_lblSeqID2 As Label = e.Item.FindControl("UI_lblSeqID2")
            Dim UI_lblSeqID3 As Label = e.Item.FindControl("UI_lblSeqID3")
            Dim UI_SeqID1 As Label = e.Item.FindControl("UI_SeqID1")
            Dim UI_SeqID2 As Label = e.Item.FindControl("UI_SeqID2")
            Dim UI_SeqID3 As Label = e.Item.FindControl("UI_SeqID3")

            Dim UI_lblRepair1 As Label = e.Item.FindControl("UI_lblRepair1")
            Dim UI_lblRepair2 As Label = e.Item.FindControl("UI_lblRepair2")
            Dim UI_lblRepair3 As Label = e.Item.FindControl("UI_lblRepair3")

            Dim UI_DownloadRepair1 As HyperLink = e.Item.FindControl("UI_DownloadRepair1")
            Dim UI_DownloadRepair2 As HyperLink = e.Item.FindControl("UI_DownloadRepair2")
            Dim UI_DownloadRepair3 As HyperLink = e.Item.FindControl("UI_DownloadRepair3")
            Dim UI_UPLOADFILE1 As Label = e.Item.FindControl("UI_UPLOADFILE1")
            Dim UI_UPLOADFILE2 As Label = e.Item.FindControl("UI_UPLOADFILE2")
            Dim UI_UPLOADFILE3 As Label = e.Item.FindControl("UI_UPLOADFILE3")

            sArrRepaurFile1 = UI_UPLOADFILE1.Text.ToString().Trim()
            sArrRepaurFile2 = UI_UPLOADFILE2.Text.ToString().Trim()
            sArrRepaurFile3 = UI_UPLOADFILE3.Text.ToString().Trim()

            If sArrRepaurFile1.Trim() <> "" Then


                UI_SeqID1.Text = "(" & UI_lblSeqID1.Text.Trim() & ")."
                Dim sRepaurFile1 As String() = sArrRepaurFile1.ToString().Trim().Split(",")
                UI_DownloadRepair1.Text = sRepaurFile1(0).ToString().Trim()
                UI_DownloadRepair1.NavigateUrl = _WEBURL & _Repair_VisualPath & sRepaurFile1(1).ToString().Trim()
                UI_lblRepair1.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID1.Visible = True
                UI_lblRepair1.Visible = True
                UI_DownloadRepair1.Visible = True
            End If

            If sArrRepaurFile2.Trim() <> "" Then
                UI_SeqID2.Text = "(" & UI_lblSeqID2.Text.Trim() & ")."
                Dim sRepaurFile2 As String() = sArrRepaurFile2.ToString().Trim().Split(",")
                UI_DownloadRepair2.Text = sRepaurFile2(0).ToString().Trim()
                UI_DownloadRepair2.NavigateUrl = _WEBURL & _Repair_VisualPath & sRepaurFile2(1).ToString().Trim()
                UI_lblRepair2.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID2.Visible = True
                UI_lblRepair2.Visible = True
                UI_DownloadRepair2.Visible = True
            End If

            If sArrRepaurFile3.Trim() <> "" Then
                UI_SeqID3.Text = "(" & UI_lblSeqID3.Text.Trim() & ")."
                Dim sRepaurFile3 As String() = sArrRepaurFile3.ToString().Trim().Split(",")
                UI_DownloadRepair3.Text = sArrRepaurFile3(0).ToString().Trim()
                UI_DownloadRepair3.NavigateUrl = _WEBURL & _Repair_VisualPath & sArrRepaurFile3(1).ToString().Trim()
                UI_lblRepair3.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID3.Visible = True
                UI_lblRepair3.Visible = True
                UI_DownloadRepair3.Visible = True
            End If

        End If
    End Sub

#End Region





    ''' <summary>
    ''' 取消 RMA 單維修品項
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click
        'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oRMAStatus As New ctlRMA.RMAStatus


        Try
            Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
            Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow

            drStatus.RMAD_ID = Me.ViewState("_RMADID").ToString()

            drStatus.RMAD_AD = Session("_UserID")
            drStatus.RMAD_ADNAME = Session("_UserName")
            drStatus.RMAD_DATE = Date.Now

            drStatus.RMAD_STATUS = 91
            dtStatus.AddRMADetailStatusRow(drStatus)

            Call oRMAStatus.ChangeStatus(dtStatus)
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Session("UCError") = sMessage
            Else
                Dim isSendMail As Boolean = SalesCancel_SendMail()
                Session("UCOK") = True
            End If
        End Try


    End Sub


    Private Function SalesCancel_SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try

            Dim oRepair As New ctlRMA.Repair
            Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable
            Dim sAccountIDText As String = ""
            Dim sApplicantIDText As String = ""
            Dim sApplicantText As String = ""
            Dim sRepairIDText As String = ""


            dtRepairHead = oRepair.QueryByRepairHead(ViewState("_RMAID").ToString())
            If dtRepairHead.Rows.Count > 0 Then
                Dim dr As RmaDTO.vwRepair_HeadRow = dtRepairHead.Rows(0)

                sAccountIDText = dr.RMA_CUNO.ToString().Trim()
                sApplicantIDText = dr.RMA_ACCOUNTID.ToString().Trim()
                sApplicantText = dr.RMA_APPLICANT.ToString().Trim()
            End If

            Dim oRMAStatus As New ctlRMA.RMAStatus
            Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            dtStatusPoint = oRMAStatus.QueryPointByDetail(ViewState("_RMADID").ToString())
            If dtStatusPoint.Rows.Count > 0 Then
                Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)
                If dr.IsREPAIRQUOTED_ADNull = False Then sRepairIDText = dr.REPAIRQUOTED_AD.Trim()
            End If

            dtCustomer = oCustomer.QueryUser(sAccountIDText, sApplicantIDText, "")
            If dtCustomer.Rows.Count > 0 Then
                Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()

                '================================================================================================================================================================================================================
                '業務報價確認 -->對象(顧客)
                '================================================================================================================================================================================================================
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

                Dim MailRepair As String = ""
                Dim RepairName As String = ""
                If sRepairIDText.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(sRepairIDText, "")
                    MailRepair = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                    RepairName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                End If

                Dim sSubject As String = _oLanguage.getText("Mail", "035", ctlLanguage.eumType.Mail)
                Dim sBody As String = _oLanguage.getText("Mail", "036", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                If MailUser.Trim() <> "" Then
                    sSubject = sSubject.Replace("[$RMA No$]", ViewState("_RMANO").ToString() + " S/N: " + UI_lblSerialText.Text)
                    sSubject = sSubject.Replace("[$Customer User Name$]", sApplicantText)

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                    End If

                    sBody = sBody.Replace("[$RMA No$]", ViewState("_RMANO").ToString() + " S/N: " + UI_lblSerialText.Text)
                    sBody = sBody.Replace("[$Repair User Name$]", RepairName)
                    sBody = sBody.Replace("[$RMA Request No$]", ViewState("_RMANO").ToString())
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = MailUser + "," + MailRepair + "," + MailSales + "," + MailAssistant
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC)
                End If
            End If

        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Session("UCError") = sMsg
            End If
        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim sRMASQID As String = ""

        Dim oGuid As Guid = Guid.NewGuid
        Dim oSale As New ctlRMA.Sale
        Dim dtSale As New RmaDTO.RMASALE_QUOTEDDataTable

        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                sRMASQID = oGuid.ToString().Trim()

            Case eumCommand.UPDATE
                sRMASQID = Me.UI_lblRMASQID.Text.ToString().Trim()
        End Select

        Dim CU_DISCOUNT_OFF As Double = Convert.ToDouble(Me.UI_txtCU_DISCOUNT_OFF.Text)

        Try
            Dim dr As RmaDTO.RMASALE_QUOTEDRow = dtSale.NewRMASALE_QUOTEDRow

            dr.RMASQ_ID = sRMASQID.ToString().Trim()
            dr.RMASQ_RMADID = Me.ViewState("_RMADID").ToString()

            dr.RMASQ_LABORCOST = Math.Round(Convert.ToDouble(Me.UI_txtSaleLaborCost.Text.ToString().Trim()), 2)
            dr.RMASQ_MATERIALCOST = Math.Round(Convert.ToDouble(Me.UI_txtSaleMaterialCost.Text.ToString().Trim()), 2)

            dr.RMASQ_QUOTE = Math.Round((dr.RMASQ_LABORCOST + dr.RMASQ_MATERIALCOST) * CU_DISCOUNT_OFF, 2)

            dr.RMASQ_CURRENCYCODE = Me.UI_CurrencyCode.Text.ToString().Trim()
            dr.RMASQ_CURRENCYRATE = Convert.ToDecimal(Me.UI_CurrencyRate.Text.ToString().Trim())

            dr.RMASQ_DESC = Me.UI_txtRemark.Text.ToString().Trim()

            dr.RMASQ_AD = Session("_UserID")
            dr.RMASQ_ADNAME = Session("_UserName")
            dr.RMASQ_CSTMP = Date.Now
            dr.RMASQ_LUAD = Session("_UserID")
            dr.RMASQ_LUADNAME = Session("_UserName")
            dr.RMASQ_LUSTMP = Date.Now

            dtSale.AddRMASALE_QUOTEDRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oSale.SaveAdd(dtSale)               '新增Sale

                Case eumCommand.UPDATE
                    oSale.SaveEdit(dtSale)              '修改Sale
            End Select


            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Session("UCError") = sMessage
            Else
                Session("UCOK") = True
            End If
        End Try

    End Sub





    ''' <summary>
    ''' 設定是否要顯示
    ''' </summary>
    ''' <param name="sRMADID">傳入RMAD_ID</param>
    ''' <param name="sRMANO">傳入RMA_NO</param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sRMAID As String, ByVal sRMADID As String, ByVal sRMANO As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.ViewState("_RMAID") = sRMAID
            If sRMADID.Trim <> "" And sRMANO.Trim <> "" Then
                Me.ViewState("_RMADID") = sRMADID
                Me.ViewState("_RMANO") = sRMANO
                Me.ViewState("_RMAID") = sRMAID

                Call ClearUIText()
                Call QueryByRepairQuotedDetail()
                Call QueryDataRepair()
                Call QueryDataSale()
                Call calTotalAMT()

                'Call QueryDataRepairUpload()
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub



    Private Sub calTotalAMT()
        Me.UI_txtSaleLaborCost.Text = Me.UI_txtLaborCost.Text

        If Me.UI_txtCU_DISCOUNT_OFF.Text.Trim() <> "" Or Me.UI_txtMaterialCost.Text.Trim() <> "" Then
            Dim CU_DISCOUNT_OFF As Double = Convert.ToDouble(Me.UI_txtCU_DISCOUNT_OFF.Text)
            Me.UI_txtSaleMaterialCost.Text = Math.Round(Convert.ToDouble(Me.UI_txtMaterialCost.Text.Trim()) * CU_DISCOUNT_OFF, 2)
        End If

        If Me.UI_txtSaleLaborCost.Text.Trim() <> "" Or Me.UI_txtSaleMaterialCost.Text.Trim() <> "" Then
            Dim LaborCost As Double = Convert.ToDouble(Me.UI_txtSaleLaborCost.Text.Trim())
            Dim MaterialCost As Double = Convert.ToDouble(Me.UI_txtSaleMaterialCost.Text.Trim())

            'Me.UI_txtSaleQuote.Text = Math.Round((LaborCost + MaterialCost) * CU_DISCOUNT_OFF, 2)
            Me.UI_txtSaleQuote.Text = Math.Round((LaborCost + MaterialCost), 2)
        End If
    End Sub



End Class

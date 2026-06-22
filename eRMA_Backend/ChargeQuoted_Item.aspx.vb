Imports System.Data
Imports System.Data.OracleClient    '150622 by cipherlab.MaggieChen
Imports System.Xml         '150622 by cipherlab.MaggieChen
Imports DataService
Imports DefLanguage

Partial Class ChargeQuoted_Item
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            'Session("_dtChargeQuoted_List") = Nothing

            If Not Me.PreviousPage Is Nothing Then
                Call clearFiled()

                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")

                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.Trim()

                Call setDefault()

                Call QueryChargeQuotedHead(Me.UI_lblPreviousPage_RMANO.Text)
                Call QueryChargeQuotedSN(Me.UI_lblPreviousPage_RMANO.Text, 0)

            End If
        End If

    End Sub
#End Region

    Private Sub clearFiled()
        Me.UI_lblPreviousPage_RMANO.Text = ""
        Me.UI_lblPreviousPage_RMAID.Text = ""
        Me.UI_lblPreviousPage_RMADID.Text = ""

        Me.hid_RMASQ_CURRENCYCODE.Text = ""
        Me.hid_RMASQ_CURRENCYRATE.Text = ""
        Me.hid_RMACQ_QUOTE_ORIGINAL.Text = ""
        Me.hid_RMACQ_CURRENCYCODE_ORIGINAL.Text = ""
        Me.hid_RMACQ_CURRENCYRATE_ORIGINAL.Text = ""


        Me.UI_Total_ServiceCharge.Text = "0"
        Me.UI_Total_MaterialCost.Text = "0"
        Me.UI_Total_TotalAmount.Text = "0"
        Me.UI_Total_DISCOUNTAMOUNT.Text = "0"
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Dim ctlCharge As New ctlChargeQuoted

        Me.uiTag_Tittle.Text = _oLanguage.getText("RMA", "429", ctlLanguage.eumType.Tag)
        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)

        Me.uiTag_RMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.uiTag_RMAStauts.Text = _oLanguage.getText("RMA", "430", ctlLanguage.eumType.Tag)
        Me.uiTag_AccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.uiTag_RequestDate.Text = _oLanguage.getText("RMA", "430", ctlLanguage.eumType.Tag)
        Me.uiTag_lblRepairCenter.Text = _oLanguage.getText("RMA", "039", ctlLanguage.eumType.Tag)

        Me.uiTag_TotalAmount.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)
        Me.uiTag_SalesName.Text = _oLanguage.getText("RMA", "431", ctlLanguage.eumType.Tag)
        Me.uiTag_DiscountOff.Text = _oLanguage.getText("RMA", "432", ctlLanguage.eumType.Tag)
        Me.uiTag_Approval.Text = _oLanguage.getText("RMA", "433", ctlLanguage.eumType.Tag)
        Me.uiTag_ActualAmount.Text = _oLanguage.getText("RMA", "434", ctlLanguage.eumType.Tag)


        Me.uiTxt_RMACQ_DISCOUNT.Visible = False
        Me.uiTxt_RMACQ_DISCOUNT.Attributes.Add("onkeyup", "calActualAmount()")

        Me.UI_cmdModify_DiscountOff.Text = _oLanguage.getText("Common", "435", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm_DiscountOff.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdApply_DiscountOff.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)

        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmitFlow.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdApply.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)


        Me.UI_cmdModify_DiscountOff.Visible = False
        Me.UI_cmdConfirm_DiscountOff.Visible = False
        Me.UI_cmdApply_DiscountOff.Visible = False

        Me.UI_cmdSubmit.Visible = False
        Me.UI_cmdSubmitFlow.Visible = False
        Me.UI_cmdApply.Visible = False


        If ctlCharge.chkIsExistSN(Me.UI_lblPreviousPage_RMANO.Text) = True Then
            Me.UI_cmdSubmit.Visible = True
            Me.UI_cmdApply.Visible = True
        Else
            If ctlCharge.chkIsExist(Me.UI_lblPreviousPage_RMANO.Text) = True Then
                Me.UI_cmdModify_DiscountOff.Visible = True
            Else
                Me.UI_cmdModify_DiscountOff.Visible = True
                Me.UI_cmdSubmit.Visible = True
                Me.UI_cmdApply.Visible = False
            End If
        End If

        If ctlCharge.chkIsExistPART(Me.UI_lblPreviousPage_RMANO.Text) = True Then
            Me.UI_cmdModify_DiscountOff.Visible = False
            Me.UI_cmdSubmit.Visible = False
            Me.UI_cmdSubmitFlow.Visible = True
            Me.UI_cmdApply.Visible = False
        End If

        'Me.uiTxt_TotalAmount.Style("display") = "none"

        Dim sScript As String = ""
        sScript = sScript & "<script type=""text/javascript"">" & vbCrLf
        sScript = sScript & "var _discountMessage=""" & _oLanguage.getText("RMA", "454", ctlLanguage.eumType.Validator) & """;" & vbCrLf
        sScript = sScript & "</script>" & vbCrLf
        Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "ChargeQuotedMsg", sScript)

    End Sub

#Region "Query"

    Private Sub QueryChargeQuotedHead(ByVal sRMANo As String)

        Dim ctlCharge As New ctlChargeQuoted
        Dim dtChargeQuotedHead As New ChargeQuotedDTO.vwChargeQuotedHeadDataTable

        dtChargeQuotedHead = ctlCharge.QueryByChargeQuotedHead(sRMANo)
        If dtChargeQuotedHead.Rows.Count > 0 Then
            Dim dr As ChargeQuotedDTO.vwChargeQuotedHeadRow = dtChargeQuotedHead.Rows(0)

            Me.uiLbl_RMANo.Text = dr.RMA_NO.ToString().Trim()
            Me.uiLbl_RMAStauts.Text = oCommon.ConvertToStatusText(dr.RMA_STATUS)
            ' Me.uiLbl_AccountName.Text = dr.RMA_APPLICANT.ToString().Trim()
            Me.uiLbl_AccountName.Text = dr.CU_NAME.ToString().Trim()
            Me.uiLbl_RequestDate.Text = dr.RMA_CSTMP.ToShortDateString()
            Me.uiLbl_RepairCenter.Text = dr.COMP_NAME.Trim()

            Try
                Me.uiLbl_SalesName.Text = dr.RMASQ_SALEADNAME.ToString().Trim()
            Catch ex As Exception
                Me.uiLbl_SalesName.Text = ""
            End Try

            Try
                Me.hid_CU_TIPTOP_ID.Text = dr.CU_TIPTOP_ID.ToString().Trim()
            Catch ex As Exception
                Me.hid_CU_TIPTOP_ID.Text = ""
            End Try

            '150702 add by MaggieChen ---begin---    


            If DBNull.Value.Equals(dr.RMASQ_SALEAD) Then
                Me.hid_RMASQ_SALEAD.Text = ""
            Else
                Me.hid_RMASQ_SALEAD.Text = dr.RMASQ_SALEAD.ToString().Trim()
            End If

            If DBNull.Value.Equals(dr.COMP_NO) Then
                Me.hid_COMP_NO.Text = ""
            Else
                Me.hid_COMP_NO.Text = dr.COMP_NO.ToString().Trim()
            End If


            '150702 add by MaggieChen ---end---    



            'approve 是否審核通過: 0.新增, 1.送簽中, 2.已確認
            If dr.IsRMACQ_APPROVALNull = False Then
                Select Case dr.RMACQ_APPROVAL.ToString()
                    Case "1"
                        Me.UI_cmdModify_DiscountOff.Visible = False
                        Me.UI_cmdConfirm_DiscountOff.Visible = False
                        Me.UI_cmdApply_DiscountOff.Visible = False

                        Me.UI_cmdSubmit.Visible = False
                        Me.UI_cmdSubmitFlow.Visible = False
                        Me.UI_cmdApply.Visible = False
                        Me.uiLbl_RMACQ_APPROVAL.Text = _oLanguage.getText("RMA", "452", ctlLanguage.eumType.Tag)

                    Case "2"
                        Me.UI_cmdModify_DiscountOff.Visible = False
                        Me.UI_cmdConfirm_DiscountOff.Visible = False
                        Me.UI_cmdApply_DiscountOff.Visible = False

                        Me.UI_cmdSubmit.Visible = False
                        Me.UI_cmdSubmitFlow.Visible = False
                        Me.UI_cmdApply.Visible = False
                        Me.uiLbl_RMACQ_APPROVAL.Text = _oLanguage.getText("RMA", "453", ctlLanguage.eumType.Tag)

                    Case Else
                        Me.uiLbl_RMACQ_APPROVAL.Text = _oLanguage.getText("RMA", "451", ctlLanguage.eumType.Tag)
                End Select

                'If dr.RMACQ_APPROVAL.ToString() = "1" Then
                '    Me.uiLbl_RMACQ_APPROVAL.Text = _oLanguage.getText("RMA", "436", ctlLanguage.eumType.Tag)
                'End If
                'If dr.RMACQ_APPROVAL.ToString() = "0" Then
                '    Me.uiLbl_RMACQ_APPROVAL.Text = _oLanguage.getText("RMA", "437", ctlLanguage.eumType.Tag)
                'End If
            End If



            Dim iRMASQ_QUOTE As Decimal = 0
            iRMASQ_QUOTE = Convert.ToDecimal(dr.RMASQ_QUOTE)
            If dr.IsRMACQ_SALEQUOTENull = False Then
                iRMASQ_QUOTE = Convert.ToDecimal(dr.RMACQ_SALEQUOTE)
            End If
            Me.uiLbl_RMACQ_SALEQUOTE.Text = iRMASQ_QUOTE.ToString("N")
            Me.uiTxt_RMACQ_SALEQUOTE.Text = iRMASQ_QUOTE.ToString()


            Dim iRMACQ_DISCOUNT As Decimal = 0
            If dr.IsRMACQ_DISCOUNTNull = False Then
                iRMACQ_DISCOUNT = Convert.ToDecimal(dr.RMACQ_DISCOUNT)
            End If
            Me.uiLbl_RMACQ_DISCOUNT.Text = iRMACQ_DISCOUNT.ToString("N")
            Me.uiTxt_RMACQ_DISCOUNT.Text = iRMACQ_DISCOUNT

            Me.uiLbl_ActualAmount.Text = (iRMASQ_QUOTE - iRMACQ_DISCOUNT).ToString()

            Me.hid_RMASQ_CURRENCYCODE.Text = dr.RMASQ_CURRENCYCODE.Trim()
            Me.hid_RMASQ_CURRENCYRATE.Text = dr.RMASQ_CURRENCYRATE.ToString()


            '紀錄原始資料
            '總金額(原始)
            Dim RMACQ_QUOTE_ORIGINAL As Decimal = dr.RMASQ_QUOTE
            If dr.IsRMACQ_QUOTE_ORIGINALNull = False Then
                RMACQ_QUOTE_ORIGINAL = dr.RMACQ_QUOTE_ORIGINAL
            End If
            Me.hid_RMACQ_QUOTE_ORIGINAL.Text = RMACQ_QUOTE_ORIGINAL.ToString()



            '幣別代號(原始)
            Dim RMACQ_CURRENCYCODE_ORIGINAL As String = dr.RMASQ_CURRENCYCODE.ToString()
            If dr.IsRMACQ_CURRENCYCODE_ORIGINALNull = False Then
                RMACQ_CURRENCYCODE_ORIGINAL = dr.RMACQ_CURRENCYCODE_ORIGINAL.Trim()
            End If
            Me.hid_RMACQ_CURRENCYCODE_ORIGINAL.Text = RMACQ_CURRENCYCODE_ORIGINAL

            '兌美金匯率(原始)
            Dim RMACQ_CURRENCYRATE_ORIGINAL As Decimal = dr.RMASQ_CURRENCYRATE
            If dr.IsRMACQ_CURRENCYRATE_ORIGINALNull = False = False Then
                RMACQ_CURRENCYRATE_ORIGINAL = dr.RMASQ_CURRENCYRATE
            End If
            Me.hid_RMACQ_CURRENCYRATE_ORIGINAL.Text = RMACQ_CURRENCYRATE_ORIGINAL.ToString()

        End If


    End Sub

    Private Sub QueryChargeQuotedSN(ByVal sRMANo As String, ByVal iPageIndex As Integer)

        Dim ctlCharge As New ctlChargeQuoted
        Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable

        dtChargeQuotedSN = ctlCharge.QueryByChargeQuotedSN(Session("_LanguageID").ToString().Trim(), sRMANo, "")
        Call QuotedSN_DataBind(dtChargeQuotedSN, iPageIndex)

    End Sub

    Private Sub QuotedSN_DataBind(ByVal dtChargeQuotedSN As ChargeQuotedDTO.vwChargeQuotedSNDataTable, ByVal iPageIndex As Integer)

        Dim dvChargeQuotedSN As DataView = dtChargeQuotedSN.DefaultView
        dvChargeQuotedSN.Sort = "RMAD_SERIALNO"
        Session("_dtChargeQuotedSN") = dtChargeQuotedSN

        Me.UI_dvRequestDetail.DataSource = dvChargeQuotedSN
        Me.UI_dvRequestDetail.DataBind()
    End Sub

    Protected Sub UI_dvRequestDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequestDetail.RowDataBound

        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

            e.Row.Cells(6).Text = _oLanguage.getText("RMA", "440", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("RMA", "441", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)

            e.Row.Cells(9).Text = _oLanguage.getText("RMA", "438", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rvDISCOUNTAMOUNT As RangeValidator = e.Row.FindControl("rvDISCOUNTAMOUNT")
            rvDISCOUNTAMOUNT.ErrorMessage = _oLanguage.getText("RMA", "442", ctlLanguage.eumType.Validator)

            Dim UI_TotalAmount_Text As Label = e.Row.FindControl("UI_TotalAmount_Text")
            Dim UITxt_RMACQSN_DISCOUNTAMOUNT As TextBox = e.Row.FindControl("UITxt_RMACQSN_DISCOUNTAMOUNT")

            Dim cvAfterDiscount As CustomValidator = e.Row.FindControl("cvAfterDiscount")
            cvAfterDiscount.Attributes.Add("Control01", UI_TotalAmount_Text.ClientID)
            cvAfterDiscount.Attributes.Add("Control02", UITxt_RMACQSN_DISCOUNTAMOUNT.ClientID)
            cvAfterDiscount.ErrorMessage = _oLanguage.getText("RMA", "454", ctlLanguage.eumType.Validator)


            'Warranty
            Dim UI_RMAD_ISWARRANTY_Text As Label = e.Row.FindControl("UI_RMAD_ISWARRANTY_Text")
            Dim UI_RMAD_ISWARRANTY As Label = e.Row.FindControl("UI_RMAD_ISWARRANTY")

            UI_RMAD_ISWARRANTY_Text.Text = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            Select Case UI_RMAD_ISWARRANTY.Text.Trim()
                Case "0"
                    UI_RMAD_ISWARRANTY_Text.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                Case "1"
                    UI_RMAD_ISWARRANTY_Text.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End Select

            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            If UI_RMADWARRANTY.Text.Trim() <> "" Then
                Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                Dim RMAD_CSTMP As DateTime = Convert.ToDateTime(UI_RMADCSTMP.Text.Trim())
                If RMAD_WARRANTY < RMAD_CSTMP Then
                    e.Row.Cells(3).ForeColor = Drawing.Color.Red
                End If
            End If


            'Improper Usage
            Dim UI_RMARQ_IMPROPERUSAGE_Text As Label = e.Row.FindControl("UI_RMARQ_IMPROPERUSAGE_Text")
            Dim UI_RMARQ_IMPROPERUSAGE As Label = e.Row.FindControl("UI_RMARQ_IMPROPERUSAGE")

            UI_RMARQ_IMPROPERUSAGE_Text.Text = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            Select Case UI_RMARQ_IMPROPERUSAGE.Text.Trim()
                Case "0"
                    UI_RMARQ_IMPROPERUSAGE_Text.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                Case "1"
                    UI_RMARQ_IMPROPERUSAGE_Text.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End Select


            'Improper Usage
            Dim UI_FailureReason_Text As Label = e.Row.FindControl("UI_FailureReason_Text")
            Dim UI_FARC_NAME1 As Label = e.Row.FindControl("UI_FARC_NAME1")
            Dim UI_FARC_NAME2 As Label = e.Row.FindControl("UI_FARC_NAME2")

            If UI_FARC_NAME2.Text.Trim() <> "" Then
                UI_FailureReason_Text.Text = UI_FARC_NAME2.Text.Trim()
            Else
                If UI_FARC_NAME1.Text.Trim() <> "" Then
                    UI_FailureReason_Text.Text = UI_FARC_NAME1.Text.Trim()
                End If
            End If


            'ServiceCharge
            Dim UI_ServiceCharge_Text As Label = e.Row.FindControl("UI_ServiceCharge_Text")
            Dim UI_RMASQ_LABORCOST As Label = e.Row.FindControl("UI_RMASQ_LABORCOST")
            Dim UI_RMACQSN_LABORCOST As Label = e.Row.FindControl("UI_RMACQSN_LABORCOST")

            Dim iServiceCharge As Decimal = Convert.ToDouble(UI_RMASQ_LABORCOST.Text.Trim())
            If UI_RMACQSN_LABORCOST.Text.Trim() <> "" Then
                iServiceCharge = Convert.ToDouble(UI_RMACQSN_LABORCOST.Text.Trim())
            End If
            UI_ServiceCharge_Text.Text = iServiceCharge.ToString("N")
            Me.UI_Total_ServiceCharge.Text = Convert.ToDecimal(Me.UI_Total_ServiceCharge.Text) + iServiceCharge

            'Material
            Dim UI_MaterialCost_Text As Label = e.Row.FindControl("UI_MaterialCost_Text")
            Dim UI_RMASQ_MATERIALCOST As Label = e.Row.FindControl("UI_RMASQ_MATERIALCOST")
            Dim UI_RMACQSN_MATERIALCOST As Label = e.Row.FindControl("UI_RMACQSN_MATERIALCOST")

            Dim iMaterialCost As Decimal = Convert.ToDouble(UI_RMASQ_MATERIALCOST.Text.Trim())
            If UI_RMACQSN_MATERIALCOST.Text.Trim() <> "" Then
                iMaterialCost = Convert.ToDouble(UI_RMACQSN_MATERIALCOST.Text.Trim())
            End If
            UI_MaterialCost_Text.Text = iMaterialCost.ToString("N")
            Me.UI_Total_MaterialCost.Text = Convert.ToDecimal(Me.UI_Total_MaterialCost.Text) + iMaterialCost


            'Total Amount
            'Dim UI_TotalAmount_Text As Label = e.Row.FindControl("UI_TotalAmount_Text")
            Dim UI_RMASQ_QUOTE As Label = e.Row.FindControl("UI_RMASQ_QUOTE")
            Dim UI_RMACQSN_QUOTE As Label = e.Row.FindControl("UI_RMACQSN_QUOTE")

            Dim iTotalAmount As Decimal = Convert.ToDouble(UI_RMASQ_QUOTE.Text.Trim())
            If UI_RMACQSN_QUOTE.Text.Trim() <> "" Then
                iTotalAmount = Convert.ToDouble(UI_RMACQSN_QUOTE.Text.Trim())
            End If
            UI_TotalAmount_Text.Text = iTotalAmount.ToString("N")
            Me.UI_Total_TotalAmount.Text = Convert.ToDecimal(Me.UI_Total_TotalAmount.Text) + iTotalAmount


            'After Discount
            Dim UI_RMACQSN_DISCOUNTAMOUNT As Label = e.Row.FindControl("UI_RMACQSN_DISCOUNTAMOUNT")
            Dim UI_RMACQSN_DISCOUNTAMOUNT_Text As Label = e.Row.FindControl("UI_RMACQSN_DISCOUNTAMOUNT_Text")
            'Dim UITxt_RMACQSN_DISCOUNTAMOUNT As TextBox = e.Row.FindControl("UITxt_RMACQSN_DISCOUNTAMOUNT")

            UITxt_RMACQSN_DISCOUNTAMOUNT.Attributes.Add("onkeyup", "calTotal_AfterDiscount()")

            UI_RMACQSN_DISCOUNTAMOUNT.Visible = False
            UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = True

            Dim iDISCOUNTAMOUNT As Decimal = iTotalAmount
            If UI_RMACQSN_DISCOUNTAMOUNT.Text.Trim <> "" Then
                iDISCOUNTAMOUNT = Convert.ToDouble(UI_RMACQSN_DISCOUNTAMOUNT.Text.Trim())
                iDISCOUNTAMOUNT = Convert.ToDouble(UI_RMACQSN_DISCOUNTAMOUNT.Text.Trim())
                UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = True

            Else
                'If Convert.ToDouble(UI_RMASQ_QUOTE.Text.Trim()) = 0 Then
                '    UI_RMACQSN_DISCOUNTAMOUNT_Text.Visible = True
                'Else
                '    UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = True
                'End If
            End If
            If Convert.ToInt16(UI_RMAD_ISWARRANTY.Text.Trim()) = 1 Then '不限制項在保固內不可依項目折讓 MODI BY Angel ON 20160310
                UI_RMACQSN_DISCOUNTAMOUNT_Text.Visible = False
                UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = True

            Else
                UI_RMACQSN_DISCOUNTAMOUNT_Text.Visible = False
                UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = True

            End If


            UI_RMACQSN_DISCOUNTAMOUNT_Text.Text = iDISCOUNTAMOUNT.ToString("N")
            UITxt_RMACQSN_DISCOUNTAMOUNT.Text = iDISCOUNTAMOUNT.ToString("#.##")

            Me.UI_Total_DISCOUNTAMOUNT.Text = Convert.ToDecimal(Me.UI_Total_DISCOUNTAMOUNT.Text) + iDISCOUNTAMOUNT

            If Me.UI_cmdSubmit.Visible = False Then
                UI_RMACQSN_DISCOUNTAMOUNT_Text.Visible = True
                UITxt_RMACQSN_DISCOUNTAMOUNT.Visible = False

            End If


            '是否要維修: 1.Accept, 2.Reject
            Dim UI_cmdDetail As ImageButton = e.Row.FindControl("UI_cmdDetail")
            Dim UI_RMAD_STATUS As Label = e.Row.FindControl("UI_RMAD_STATUS")
            Dim UI_RMARQ_ACCEPT As Label = e.Row.FindControl("UI_RMARQ_ACCEPT")
            If UI_RMARQ_ACCEPT.Text.Trim() <> "1" Or UI_RMAD_STATUS.Text.Trim() = "91" Then
                UI_cmdDetail.Visible = False
            End If

        End If


        If e.Row.RowType = DataControlRowType.Footer Then
            Dim UI_Footer_ServiceCharge As Label = e.Row.FindControl("UI_Footer_ServiceCharge")
            Dim UI_Footer_MaterialCost As Label = e.Row.FindControl("UI_Footer_MaterialCost")
            Dim UI_Footer_TotalAmount As Label = e.Row.FindControl("UI_Footer_TotalAmount")
            Dim UI_Footer_DISCOUNTAMOUNT As Label = e.Row.FindControl("UI_Footer_DISCOUNTAMOUNT")

            UI_Footer_ServiceCharge.Text = Convert.ToDecimal(Me.UI_Total_ServiceCharge.Text).ToString("N")
            UI_Footer_MaterialCost.Text = Convert.ToDecimal(Me.UI_Total_MaterialCost.Text).ToString("N")
            UI_Footer_TotalAmount.Text = Convert.ToDecimal(Me.UI_Total_TotalAmount.Text).ToString("N")
            UI_Footer_DISCOUNTAMOUNT.Text = Convert.ToDecimal(Me.UI_Total_DISCOUNTAMOUNT.Text).ToString("N")
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

    Protected Sub UI_dvRequestDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequestDetail.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtChargeQuotedSN") Is Nothing Then
            Dim dtChargeQuotedSN As ChargeQuotedDTO.vwChargeQuotedSNDataTable = Session("_dtChargeQuotedSN")
            Call QuotedSN_DataBind(dtChargeQuotedSN, iPageIndex)

        Else
            Call QueryChargeQuotedSN(Me.UI_lblPreviousPage_RMANO.Text, iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRequestDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequestDetail.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        If e.CommandName = "cmdDetail" Then
            row = Me.UI_dvRequestDetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")

            Me.UI_lblPreviousPage_RMADID.Text = UI_RMADID.Text.Trim()
        End If

    End Sub

#End Region

#Region "Click"
    ''' <summary>
    ''' 修改 Discount Off
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdModify_DiscountOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdModify_DiscountOff.Click
        Me.uiLbl_RMACQ_DISCOUNT.Visible = False
        Me.uiTxt_RMACQ_DISCOUNT.Visible = True

        Me.UI_cmdModify_DiscountOff.Visible = False
        Me.UI_cmdConfirm_DiscountOff.Visible = True
        Me.UI_cmdApply_DiscountOff.Visible = False

        Me.UI_cmdSubmit.Visible = False
        Me.UI_cmdSubmitFlow.Visible = False
        Me.UI_cmdApply.Visible = False
    End Sub

    Protected Sub UI_cmdApply_DiscountOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdApply_DiscountOff.Click
        Me.uiLbl_RMACQ_DISCOUNT.Visible = True
        Me.uiTxt_RMACQ_DISCOUNT.Visible = False
        Me.uiLbl_RMACQ_DISCOUNT.Text = Me.uiTxt_RMACQ_DISCOUNT.Text

        Me.UI_cmdModify_DiscountOff.Visible = True
        Me.UI_cmdConfirm_DiscountOff.Visible = False
        Me.UI_cmdApply_DiscountOff.Visible = False

        saveChargeQUOTED(False, False)

    End Sub

    ''' <summary>
    ''' 儲存 RMACharge_QUOTED 及 RMASALE_QUOTED_CLOG, RMAREPAIR_QUOTED_CLOG, RMAREPAIR_QUOTED_DETAIL_CLOG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirm_DiscountOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm_DiscountOff.Click
        Me.uiLbl_RMACQ_DISCOUNT.Visible = True
        Me.uiTxt_RMACQ_DISCOUNT.Visible = False
        Me.uiLbl_RMACQ_DISCOUNT.Text = Me.uiTxt_RMACQ_DISCOUNT.Text

        Me.UI_cmdModify_DiscountOff.Visible = True
        Me.UI_cmdConfirm_DiscountOff.Visible = False
        Me.UI_cmdApply_DiscountOff.Visible = False

        saveChargeQUOTED(False, True)

    End Sub

    Protected Sub UI_cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdApply.Click

        saveChargeQUOTED(True, False)
        Call QueryChargeQuotedHead(Me.UI_lblPreviousPage_RMANO.Text)

    End Sub

    ''' <summary>
    ''' 儲存 RMACharge_QUOTED 及 RMACHARGE_QUOTED_SN, RMASALE_QUOTED_CLOG, RMAREPAIR_QUOTED_CLOG, RMAREPAIR_QUOTED_DETAIL_CLOG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click

        saveChargeQUOTED(True, True)

    End Sub

    ''' <summary>
    ''' 儲存 RMACharge_QUOTED 及 RMACHARGE_QUOTED_SN, RMASALE_QUOTED_CLOG, RMAREPAIR_QUOTED_CLOG, RMAREPAIR_QUOTED_DETAIL_CLOG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmitFlow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmitFlow.Click
        saveFlow()
        Response.Redirect("ChargeQuoted_List.aspx")
    End Sub

#End Region

#Region "save"

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="isChargeQUOTED_SN">判斷是否是修改 SN</param>
    ''' <remarks></remarks>
    Private Sub saveChargeQUOTED(ByVal isChargeQUOTED_SN As Boolean, ByVal isConfirm As Boolean)
        Dim blnFlag As Boolean
        Dim sMessage As String = ""

        Dim ctlChargeQuoted As New ctlChargeQuoted()
        Dim dtRMACharge_QUOTED As New ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable
        Dim dtChargeQUOTED_SN As New ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable
        Dim dtRMACHARGE_QUOTED_PART As New ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable

        Dim RMACQ_SALEQUOTE As Decimal = Convert.ToDecimal(Me.uiTxt_RMACQ_SALEQUOTE.Text)
        Dim RMACQ_DISCOUNT As Decimal = Convert.ToDecimal(Me.uiTxt_RMACQ_DISCOUNT.Text)

        Try
            If isChargeQUOTED_SN = True Then
                'dtChargeQUOTED_SN Table
                Dim total_DISCOUNTAMOUNT As Decimal = 0
                dtChargeQUOTED_SN = Save_RMACHARGE_QUOTED_SN(total_DISCOUNTAMOUNT)

                RMACQ_SALEQUOTE = total_DISCOUNTAMOUNT
                RMACQ_DISCOUNT = 0
            End If

            'RMACQ_SALEQUOTE Table
            dtRMACharge_QUOTED = Save_RMACharge_QUOTED(RMACQ_SALEQUOTE, RMACQ_DISCOUNT)

            ctlChargeQuoted.saveChargeQUOTED(dtRMACharge_QUOTED, dtChargeQUOTED_SN, dtRMACHARGE_QUOTED_PART)

            If isConfirm = True Then
                saveFlow()
            End If

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)

            Else
                If isConfirm = True Then
                    Response.Redirect("ChargeQuoted_List.aspx")
                End If
                'Server.Transfer("ChargeQuoted_List.aspx")
            End If

        End Try


    End Sub

    Private Function Save_RMACharge_QUOTED(ByVal RMACQ_SALEQUOTE As Decimal, ByVal RMACQ_DISCOUNT As Decimal) As ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable
        Dim dtRMACharge_QUOTED As New ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable

        Try
            Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTEDRow = dtRMACharge_QUOTED.NewRMACHARGE_QUOTEDRow
            dr.RMACQ_ID = Guid.NewGuid.ToString()

            dr.RMACQ_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
            dr.RMACQ_CHARGEDATE = Date.Now

            dr.RMACQ_SALEQUOTE = RMACQ_SALEQUOTE
            dr.RMACQ_DISCOUNT = RMACQ_DISCOUNT
            dr.RMACQ_CHARGEQUOTE = RMACQ_SALEQUOTE - RMACQ_DISCOUNT

            dr.RMACQ_CURRENCYCODE = Me.hid_RMASQ_CURRENCYCODE.Text
            dr.RMACQ_CURRENCYRATE = Convert.ToDecimal(Me.hid_RMASQ_CURRENCYRATE.Text)

            dr.RMACQ_QUOTE_ORIGINAL = Me.hid_RMACQ_QUOTE_ORIGINAL.Text
            dr.RMACQ_CURRENCYCODE_ORIGINAL = Me.hid_RMACQ_CURRENCYCODE_ORIGINAL.Text
            dr.RMACQ_CURRENCYRATE_ORIGINAL = Me.hid_RMACQ_CURRENCYRATE_ORIGINAL.Text

            dr.RMACQ_APPROVAL = 0

            dr.RMACQ_AD = Session("_UserID")
            dr.RMACQ_ADNAME = Session("_UserName")
            dr.RMACQ_CSTMP = Date.Now
            dr.RMACQ_LUAD = Session("_UserID")
            dr.RMACQ_LUADNAME = Session("_UserName")
            dr.RMACQ_LUSTMP = Date.Now


            dtRMACharge_QUOTED.AddRMACHARGE_QUOTEDRow(dr)

        Catch ex As Exception
            Throw ex
        End Try


        Return dtRMACharge_QUOTED
    End Function

    Private Function Save_RMACHARGE_QUOTED_SN(ByRef total_DISCOUNTAMOUNT As Decimal) As ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable
        Dim i As Integer = 0

        Dim dtChargeQUOTED_SN As New ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable

        Try
            For i = 0 To Me.UI_dvRequestDetail.Rows.Count - 1

                If Me.UI_dvRequestDetail.Rows.Item(i).RowType = DataControlRowType.DataRow Then
                    Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTED_SNRow = dtChargeQUOTED_SN.NewRMACHARGE_QUOTED_SNRow

                    Dim UI_RMADID As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMADID")


                    dr.RMACQSN_ID = Guid.NewGuid.ToString()
                    dr.RMACQSN_RMADID = UI_RMADID.Text.Trim()


                    '工時費用 或 Service Charge
                    Dim UI_RMASQ_LABORCOST As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMASQ_LABORCOST")
                    Dim UI_RMACQSN_LABORCOST As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMACQSN_LABORCOST")
                    dr.RMACQSN_LABORCOST = Convert.ToDecimal(UI_RMASQ_LABORCOST.Text)

                    '零件費用
                    Dim UI_RMASQ_MATERIALCOST As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMASQ_MATERIALCOST")
                    Dim UI_RMACQSN_MATERIALCOST As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMACQSN_MATERIALCOST")
                    dr.RMACQSN_MATERIALCOST = Convert.ToDecimal(UI_RMASQ_MATERIALCOST.Text)

                    '費用加總
                    Dim UI_RMASQ_QUOTE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMASQ_QUOTE")
                    Dim UI_RMACQSN_QUOTE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMACQSN_QUOTE")
                    dr.RMACQSN_QUOTE = Convert.ToDecimal(UI_RMASQ_QUOTE.Text)

                    'After Discount 
                    Dim UITxt_RMACQSN_DISCOUNTAMOUNT As TextBox = Me.UI_dvRequestDetail.Rows(i).FindControl("UITxt_RMACQSN_DISCOUNTAMOUNT")
                    dr.RMACQSN_DISCOUNTAMOUNT = Convert.ToDecimal(UITxt_RMACQSN_DISCOUNTAMOUNT.Text)
                    total_DISCOUNTAMOUNT = total_DISCOUNTAMOUNT + dr.RMACQSN_DISCOUNTAMOUNT

                    '幣別代號(ex:NTD , USD)
                    'UI_RMASQ_CURRENCYCODE
                    Dim UI_RMASQ_CURRENCYCODE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMASQ_CURRENCYCODE")
                    Dim UI_RMACQSN_CURRENCYCODE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMACQSN_CURRENCYCODE")
                    dr.RMACQSN_CURRENCYCODE = UI_RMASQ_CURRENCYCODE.Text.Trim()

                    '兌美金匯率
                    Dim UI_RMASQ_CURRENCYRATE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMASQ_CURRENCYRATE")
                    Dim UI_RMACQSN_CURRENCYRATE As Label = Me.UI_dvRequestDetail.Rows(i).FindControl("UI_RMACQSN_CURRENCYRATE")
                    dr.RMACQSN_CURRENCYRATE = Convert.ToDecimal(UI_RMASQ_CURRENCYRATE.Text)






                    '工時費用 或 Service Charge(原始)
                    Dim RMACQSN_LABORCOST_ORIGINAL As Decimal = Convert.ToDecimal(UI_RMASQ_LABORCOST.Text)
                    If UI_RMACQSN_LABORCOST.Text.Trim <> "" Then
                        RMACQSN_LABORCOST_ORIGINAL = Convert.ToDecimal(UI_RMACQSN_LABORCOST.Text)
                    End If
                    dr.RMACQSN_LABORCOST_ORIGINAL = RMACQSN_LABORCOST_ORIGINAL


                    '零件費用(原始)
                    Dim RMACQSN_MATERIALCOST_ORIGINAL As Decimal = Convert.ToDecimal(UI_RMASQ_MATERIALCOST.Text)
                    If UI_RMACQSN_MATERIALCOST.Text.Trim <> "" Then
                        RMACQSN_MATERIALCOST_ORIGINAL = Convert.ToDecimal(UI_RMACQSN_MATERIALCOST.Text)
                    End If
                    dr.RMACQSN_MATERIALCOST_ORIGINAL = RMACQSN_MATERIALCOST_ORIGINAL


                    '費用加總(原始)
                    Dim RMACQSN_QUOTE_ORIGINAL As Decimal = Convert.ToDecimal(UI_RMASQ_QUOTE.Text)
                    If UI_RMACQSN_QUOTE.Text.Trim <> "" Then
                        RMACQSN_QUOTE_ORIGINAL = Convert.ToDecimal(UI_RMACQSN_QUOTE.Text)
                    End If
                    dr.RMACQSN_QUOTE_ORIGINAL = RMACQSN_QUOTE_ORIGINAL


                    '幣別代號(ex:NTD , USD)(原始)
                    dr.RMACQSN_CURRENCYCODE_ORIGINAL = UI_RMASQ_CURRENCYCODE.Text.Trim()
                    If UI_RMACQSN_CURRENCYCODE.Text.Trim <> "" Then
                        dr.RMACQSN_CURRENCYCODE_ORIGINAL = UI_RMACQSN_CURRENCYCODE.Text.Trim()
                    End If

                    '兌美金匯率(原始)
                    Dim RMACQSN_CURRENCYRATE_ORIGINAL As Decimal = Convert.ToDecimal(UI_RMASQ_CURRENCYRATE.Text)
                    If UI_RMACQSN_CURRENCYRATE.Text.Trim <> "" Then
                        RMACQSN_CURRENCYRATE_ORIGINAL = Convert.ToDecimal(UI_RMACQSN_CURRENCYRATE.Text)
                    End If
                    dr.RMACQSN_CURRENCYRATE_ORIGINAL = RMACQSN_CURRENCYRATE_ORIGINAL


                    dr.RMACQSN_AD = Session("_UserID")
                    dr.RMACQSN_ADNAME = Session("_UserName")
                    dr.RMACQSN_CSTMP = Date.Now
                    dr.RMACQSN_LUAD = Session("_UserID")
                    dr.RMACQSN_LUADNAME = Session("_UserName")
                    dr.RMACQSN_LUSTMP = Date.Now

                    dtChargeQUOTED_SN.AddRMACHARGE_QUOTED_SNRow(dr)
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try


        Return dtChargeQUOTED_SN
    End Function
    'MODI BY Angel ON 20151217 增加上海走折讓流程
    Public Sub saveFlow()
        Dim i As Integer = 0
        Dim ctlChargeQuoted As New ctlChargeQuoted()

        Try

            '如果維修中心是CEAT, CLHQ, CL_CHINA才會呼叫runSP_SHP_INS_AR
            'Dim isRun_SP_INS_AR22 As Boolean = False
            'Dim sRepairNo As String = "CEAT,CLHQ,CL_CHINA"
            'Dim arrRepairNo() As String = sRepairNo.Split(",")
            'For i = 0 To arrRepairNo.Length - 1
            '    If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo(i).ToString().Trim()) <> -1 Then
            '        isRun_SP_INS_AR22 = True
            '        Exit For
            '    End If
            'Next

            'If isRun_SP_INS_AR22 = True Then
            '    Dim retval As String = runSP_INS_AR22()
            'End If

            '150622 by Cipherlab.MaggieChen:RMA折讓單 ----Begin----
            Dim FlowWS As New WorkflowService.WorkflowServiceService()
            Dim formOID As String = FlowWS.findFormOIDsOfProcess("RMACharge_QUOTED")
            Dim formXML As New XmlDocument()

            formXML.LoadXml(FlowWS.getFormFieldTemplate(formOID))
            formXML.DocumentElement.SelectSingleNode("rmano").InnerText = Me.uiLbl_RMANo.Text
            formXML.DocumentElement.SelectSingleNode("rmastauts").InnerText = Me.uiLbl_RMAStauts.Text
            formXML.DocumentElement.SelectSingleNode("accountname").InnerText = Me.uiLbl_AccountName.Text
            formXML.DocumentElement.SelectSingleNode("requestdate").InnerText = Me.uiLbl_RequestDate.Text
            formXML.DocumentElement.SelectSingleNode("repaircenter").InnerText = Me.uiLbl_RepairCenter.Text
            formXML.DocumentElement.SelectSingleNode("rmacq_salequote").InnerText = Me.uiLbl_RMACQ_SALEQUOTE.Text
            formXML.DocumentElement.SelectSingleNode("rma_salesname").InnerText = Me.uiLbl_SalesName.Text
            formXML.DocumentElement.SelectSingleNode("rmacq_discount").InnerText = Me.uiLbl_RMACQ_DISCOUNT.Text
            formXML.DocumentElement.SelectSingleNode("rmacq_approval").InnerText = Me.uiLbl_RMACQ_APPROVAL.Text
            formXML.DocumentElement.SelectSingleNode("actualamount").InnerText = Me.uiLbl_ActualAmount.Text
            'formXML.DocumentElement.SelectSingleNode("salesid").InnerText = Me.hid_RMASQ_SALEAD.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("custno").InnerText = Me.hid_CU_TIPTOP_ID.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("userno").InnerText = Session("_UserID").ToString().Trim()
            formXML.DocumentElement.SelectSingleNode("compno").InnerText = Me.hid_COMP_NO.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("total_servicecharge").InnerText = Me.UI_Total_ServiceCharge.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("total_materialcost").InnerText = Me.UI_Total_MaterialCost.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("total_totalamount").InnerText = Me.UI_Total_TotalAmount.Text.Trim()
            formXML.DocumentElement.SelectSingleNode("total_discountamount").InnerText = Convert.ToDecimal(Me.UI_Total_TotalAmount.Text.Trim()) - Convert.ToDecimal(Me.UI_Total_DISCOUNTAMOUNT.Text.Trim())

            'Throw New Exception(formOID)
            'Throw New Exception(formXML.DocumentElement.SelectSingleNode("repaircenter").InnerText & "," & formXML.DocumentElement.SelectSingleNode("salesid").InnerText )
            'Throw New Exception(formXML.DocumentElement.SelectSingleNode("total_discountamount").InnerText)
            'Throw New Exception(formXML.DocumentElement.SelectSingleNode("total_discountamount").InnerText & "," & formXML.DocumentElement.SelectSingleNode("userno").InnerText)

            'DataSet(oDS)  '撈單身資料
            Dim dtData As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
            'If Session("_RepairCenter").ToString().Trim() = "CL_CHINA" Then
            If Session("_Comp_Admin").ToString().Trim() = "China" Then
                dtData = ctlChargeQuoted.QueryBySHChargeQuotedSN(Session("_LanguageID").ToString().Trim(), Me.uiLbl_RMANo.Text, "")
            ElseIf Session("_Comp_Admin").ToString().Trim() = "Mplus" Then
                dtData = ctlChargeQuoted.QueryByBondChargeQuotedSN(Session("_LanguageID").ToString().Trim(), Me.uiLbl_RMANo.Text, "")
            Else
                dtData = ctlChargeQuoted.QueryByChargeQuotedSN(Session("_LanguageID").ToString().Trim(), Me.uiLbl_RMANo.Text, "")
            End If

            '設定datagrid
            Dim xnl As XmlNode
            xnl = formXML.DocumentElement.SelectSingleNode("rcq_grid/records")
            Dim xn As XmlNode
            xn = xnl.FirstChild.Clone()
            xnl.RemoveAll()
            'xn.Attributes("id").InnerText = "ramcqgrid_0"
            xn.SelectSingleNode("//item[@id='rmad_serialno']").InnerText = dtData.Rows(0)("rmad_serialno").ToString
            xn.SelectSingleNode("//item[@id='rmad_modelno']").InnerText = dtData.Rows(0)("rmad_modelno").ToString
            xn.SelectSingleNode("//item[@id='rmad_iswarranty']").InnerText = dtData.Rows(0)("rmad_iswarranty").ToString
            xn.SelectSingleNode("//item[@id='rmarq_improperusage']").InnerText = dtData.Rows(0)("rmarq_improperusage").ToString
            If dtData.Rows(0)("farc_name2").ToString <> "" Then
                xn.SelectSingleNode("//item[@id='farc_name']").InnerText = dtData.Rows(0)("farc_name2").ToString
            Else
                If dtData.Rows(0)("farc_name1").ToString <> "" Then
                    xn.SelectSingleNode("//item[@id='farc_name']").InnerText = dtData.Rows(0)("farc_name1").ToString
                End If
            End If
            xn.SelectSingleNode("//item[@id='rmasq_laborcost']").InnerText = dtData.Rows(0)("rmasq_laborcost").ToString
            xn.SelectSingleNode("//item[@id='rmasq_materialcost']").InnerText = dtData.Rows(0)("rmasq_materialcost").ToString
            xn.SelectSingleNode("//item[@id='rmasq_quote']").InnerText = dtData.Rows(0)("rmacqsn_quote").ToString
            xn.SelectSingleNode("//item[@id='rmacqsn_discountamount']").InnerText = dtData.Rows(0)("rmacqsn_discountamount").ToString
            xnl.AppendChild(xn)

            'Throw New Exception(dvChargeQuotedSN.Rows.Count)
            For i = 1 To dtData.Rows.Count - 1 Step 1
                xn = xnl.FirstChild.Clone()
                'xn.Attributes("id").InnerText = "ramcqgrid_" + i.ToString
                xn.SelectSingleNode("//item[@id='rmad_serialno']").InnerText = dtData.Rows(i)("rmad_serialno").ToString
                xn.SelectSingleNode("//item[@id='rmad_modelno']").InnerText = dtData.Rows(i)("rmad_modelno").ToString
                xn.SelectSingleNode("//item[@id='rmad_iswarranty']").InnerText = dtData.Rows(i)("rmad_iswarranty").ToString
                xn.SelectSingleNode("//item[@id='rmarq_improperusage']").InnerText = dtData.Rows(i)("rmarq_improperusage").ToString
                If dtData.Rows(0)("farc_name2").ToString <> "" Then
                    xn.SelectSingleNode("//item[@id='farc_name']").InnerText = dtData.Rows(i)("farc_name2").ToString
                Else
                    If dtData.Rows(0)("farc_name1").ToString <> "" Then
                        xn.SelectSingleNode("//item[@id='farc_name']").InnerText = dtData.Rows(i)("farc_name1").ToString
                    End If
                End If
                xn.SelectSingleNode("//item[@id='rmasq_laborcost']").InnerText = dtData.Rows(i)("rmasq_laborcost").ToString
                xn.SelectSingleNode("//item[@id='rmasq_materialcost']").InnerText = dtData.Rows(i)("rmasq_materialcost").ToString
                xn.SelectSingleNode("//item[@id='rmasq_quote']").InnerText = dtData.Rows(i)("rmacqsn_quote").ToString
                xn.SelectSingleNode("//item[@id='rmacqsn_discountamount']").InnerText = dtData.Rows(i)("rmacqsn_discountamount").ToString
                xnl.AppendChild(xn)
            Next

            'Throw New Exception(dtData.Rows(1)("rmasq_quote").ToString & "," & dtData.Rows(1)("rmacqsn_discountamount").ToString & "," & dtData.Rows(1)("rmarq_improperusage").ToString)
            'Throw New Exception(dtData.Rows(1)("rmacqsn_quote").ToString & "," & dtData.Rows(1)("rmacqsn_discountamount").ToString)
            'Throw New Exception(formXML.DocumentElement.SelectSingleNode("total_discountamount").InnerText)
            'If Session("_RepairCenter").ToString().Trim() = "CL_CHINA" Then
            If Session("_Comp_Admin").ToString().Trim() = "China" Then

                'Dim retval As String = runSP_INS_AR22()
                Dim strAD As String = dtData.Rows(0)("CU_SALESID").ToString
                Dim strDept As String = dtData.Rows(0)("GEN03").ToString
                Dim processID As String
                formXML.DocumentElement.SelectSingleNode("salesid").InnerText = strAD
                processID = FlowWS.invokeProcess("SH_RMACharge_QUOTED", strAD, strDept, formOID, formXML.InnerXml, "RMA折讓單" + Me.uiLbl_RMANo.Text)

                '塞入Flow回傳的流程序號及更改狀態(UPDATE RMACHARGE_QUOTED SET RMACQ_APPROVAL=1,RMACQ_EF_ID=sRMACQ_EF_ID)
                Dim sRMACQ_RMANO As String = Me.uiLbl_RMANo.Text
                Dim sRMACQ_EF_ID As String = processID
                Dim retMessage As String = runSP_UPD_RMACQ(sRMACQ_RMANO, 1, sRMACQ_EF_ID)
                If retMessage.Trim() <> "OK" Then
                    Throw New Exception(retMessage.Trim())
                End If
                '150622 by cipherlab.MaggieChen ----End----

            ElseIf Session("_Comp_Admin").ToString().Trim() = "Mplus" Then

                Dim strAD As String = dtData.Rows(0)("CU_SALESID").ToString
                Dim strDept As String = dtData.Rows(0)("GEN03").ToString
                Dim processID As String
                formXML.DocumentElement.SelectSingleNode("salesid").InnerText = strAD


                processID = FlowWS.invokeProcess("BON_RMACharge_QUOTED", strAD, strDept, formOID, formXML.InnerXml, "RMA折讓單" + Me.uiLbl_RMANo.Text)


                '塞入Flow回傳的流程序號及更改狀態(UPDATE RMACHARGE_QUOTED SET RMACQ_APPROVAL=1,RMACQ_EF_ID=sRMACQ_EF_ID)
                Dim sRMACQ_RMANO As String = Me.uiLbl_RMANo.Text
                Dim sRMACQ_EF_ID As String = processID
                Dim retMessage As String = runSP_UPD_RMACQ(sRMACQ_RMANO, 1, sRMACQ_EF_ID)
                If retMessage.Trim() <> "OK" Then
                    Throw New Exception(retMessage.Trim())
                End If

            Else

                Dim strAD As String = dtData.Rows(0)("CU_SALESID").ToString
                Dim strDept As String = dtData.Rows(0)("GEN03").ToString
                Dim processID As String
                formXML.DocumentElement.SelectSingleNode("salesid").InnerText = strAD


                processID = FlowWS.invokeProcess("RMACharge_QUOTED", strAD, strDept, formOID, formXML.InnerXml, "RMA折讓單" + Me.uiLbl_RMANo.Text)


                '塞入Flow回傳的流程序號及更改狀態(UPDATE RMACHARGE_QUOTED SET RMACQ_APPROVAL=1,RMACQ_EF_ID=sRMACQ_EF_ID)
                Dim sRMACQ_RMANO As String = Me.uiLbl_RMANo.Text
                Dim sRMACQ_EF_ID As String = processID
                Dim retMessage As String = runSP_UPD_RMACQ(sRMACQ_RMANO, 1, sRMACQ_EF_ID)
                If retMessage.Trim() <> "OK" Then
                    Throw New Exception(retMessage.Trim())
                End If
                '150622 by cipherlab.MaggieChen ----End----
            End If

        Catch ex As Exception
            Throw ex

        Finally

        End Try

    End Sub

    Public Function runSP_INS_AR22() As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        Try

            oCommand.CommandText = "SP_INS_AR22"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("vCUSTNO", OracleType.NVarChar).Value = Me.hid_CU_TIPTOP_ID.Text.Trim()
            oCommand.Parameters("vCUSTNO").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vRMANO", OracleType.NVarChar).Value = Me.uiLbl_RMANo.Text.Trim()
            oCommand.Parameters("vRMANO").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vUserNo", OracleType.NVarChar).Value = Session("_UserID").ToString().Trim()
            oCommand.Parameters("vUserNo").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vCompno", OracleType.NVarChar).Value = Session("_RepairCenter").ToString().Trim()
            oCommand.Parameters("vCompno").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

    Public Function runSP_UPD_RMACQ(ByVal RMACQ_RMANO_IN As String, ByVal RMACQ_APPROVAL_IN As Integer, ByVal RMACQ_EF_ID_IN As String) As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        Try

            oCommand.CommandText = "SP_UPD_RMACQ"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("RMACQ_RMANO_IN", OracleType.NVarChar).Value = RMACQ_RMANO_IN
            oCommand.Parameters("RMACQ_RMANO_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RMACQ_APPROVAL_IN", OracleType.Int16).Value = RMACQ_APPROVAL_IN
            oCommand.Parameters("RMACQ_APPROVAL_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RMACQ_EF_ID_IN", OracleType.NVarChar).Value = RMACQ_EF_ID_IN
            oCommand.Parameters("RMACQ_EF_ID_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

#End Region

End Class

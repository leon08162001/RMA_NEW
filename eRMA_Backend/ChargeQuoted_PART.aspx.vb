Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports DefLanguage


Partial Class ChargeQuoted_PART
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
                clearFiled()

                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")

                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.Trim()
                Me.UI_lblPreviousPage_RMADID.Text = UI_lblPreviousPage_RMADID.Text.Trim()

                Call setDefault()

                Call QueryChargeQuotedSN(Me.UI_lblPreviousPage_RMANO.Text, Me.UI_lblPreviousPage_RMADID.Text, 0)
                Call QueryChargeQuotedPART(Me.UI_lblPreviousPage_RMADID.Text, 0)
            End If
        End If

    End Sub
#End Region

    Private Sub clearFiled()
        Me.uiLbl_ApprovalStatus.Text = ""

        Me.UI_lblPreviousPage_RMANO.Text = ""
        Me.UI_lblPreviousPage_RMAID.Text = ""
        Me.UI_lblPreviousPage_RMADID.Text = ""
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "429", ctlLanguage.eumType.Tag)

        Me.uiTag_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
        Me.uiTag_lblRequestDate.Text = _oLanguage.getText("RMA", "033", ctlLanguage.eumType.Tag)
        Me.uiTag_RepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.uiTag_Applicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.uiTag_SerialNumber.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.uiTag_RenewDate.Text = _oLanguage.getText("RMA", "443", ctlLanguage.eumType.Tag)

        Me.uiTag_ApprovalStatus.Text = _oLanguage.getText("RMA", "444", ctlLanguage.eumType.Tag)
        Me.uiTag_ApprovalDate.Text = _oLanguage.getText("RMA", "445", ctlLanguage.eumType.Tag)

        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)


        Me.lbl_ServiceCharge.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.uiLbl_MaterialCharge.Text = _oLanguage.getText("RMA", "448", ctlLanguage.eumType.Tag)
        Me.uiLbl_TotalCharge.Text = _oLanguage.getText("RMA", "449", ctlLanguage.eumType.Tag)


        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdApply.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryChargeQuotedSN(ByVal sRMANo As String, ByVal sRMAD_ID As String, ByVal iPageIndex As Integer)

        Dim ctlCharge As New ctlChargeQuoted
        Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
        dtChargeQuotedSN = ctlCharge.QueryByChargeQuotedSN(Session("_LanguageID").ToString().Trim(), sRMANo, sRMAD_ID)

        If dtChargeQuotedSN.Rows.Count > 0 Then
            Dim dr As ChargeQuotedDTO.vwChargeQuotedSNRow = dtChargeQuotedSN.Rows(0)

            Me.UI_RMANo.Text = dr.RMA_NO.ToString().Trim()
            Me.UI_RequestDate.Text = dr.RMA_CSTMP.ToShortDateString()
            Me.UI_RepairCenter.Text = dr.COMP_NAME.Trim()
            Me.UI_Applicant.Text = dr.RMA_APPLICANT.Trim()
            Me.uiLbl_SerialNumber.Text = dr.RMAD_SERIALNO.Trim()

            Try
                Me.hid_CU_TIPTOP_ID.Text = dr.CU_TIPTOP_ID.ToString().Trim()
            Catch ex As Exception
                Me.hid_CU_TIPTOP_ID.Text = ""
            End Try



            Dim RenewDate As DateTime = Date.Now
            If dr.IsRMACQ_CHARGEDATENull = False Then
                RenewDate = dr.RMACQ_CHARGEDATE
            End If
            Me.uiLbl_RenewDate.Text = RenewDate.ToShortDateString()


            '是否審核通過: 0.否, 1.是,
            'approve 是否審核通過: 0.新增, 1.送簽中, 2.已確認
            Me.UI_cmdApply.Visible = False
            Me.UI_cmdConfirm.Visible = False
            If dr.IsRMACQ_APPROVALNull = False Then
                Select Case dr.RMACQ_APPROVAL.ToString()
                    Case "1"
                        Me.uiLbl_ApprovalStatus_Text.Text = _oLanguage.getText("RMA", "452", ctlLanguage.eumType.Tag)
                    Case "2"
                        Me.uiLbl_ApprovalStatus_Text.Text = _oLanguage.getText("RMA", "453", ctlLanguage.eumType.Tag)
                    Case Else
                        Me.uiLbl_ApprovalStatus_Text.Text = _oLanguage.getText("RMA", "451", ctlLanguage.eumType.Tag)
                End Select

                Me.uiLbl_ApprovalStatus.Text = dr.RMACQ_APPROVAL.ToString()
            End If


            If dr.IsRMACQ_APPROVALDATENull = False Then
                Me.uiLbl_ApprovalDate.Text = dr.RMACQ_APPROVALDATE.ToShortDateString()
            End If

            'Service Charge
            Dim RMACQSN_LABORCOST As Decimal = dr.RMASQ_LABORCOST
            If dr.IsRMACQSN_LABORCOSTNull = False Then
                RMACQSN_LABORCOST = dr.RMACQSN_LABORCOST
            End If
            Me.UI_RMACQSN_LABORCOST.Text = RMACQSN_LABORCOST.ToString("N")

            'Material Charge
            Dim RMACQSN_MATERIALCOST As Decimal = dr.RMASQ_MATERIALCOST
            If dr.IsRMACQSN_MATERIALCOSTNull = False Then
                RMACQSN_MATERIALCOST = dr.RMACQSN_MATERIALCOST
            End If
            Me.UI_RMACQSN_MATERIALCOST.Text = RMACQSN_MATERIALCOST.ToString("N")

            'Total Charge 
            Dim RMACQSN_QUOTE As Decimal = dr.RMASQ_QUOTE
            If dr.IsRMACQSN_MATERIALCOSTNull = False Then
                RMACQSN_QUOTE = dr.RMACQSN_QUOTE
            End If
            Me.UI_RMACQSN_QUOTE.Text = RMACQSN_QUOTE.ToString("N")

        End If

    End Sub

    Private Sub QueryChargeQuotedPART(ByVal sRMAD_ID As String, ByVal iPageIndex As Integer)

        Dim ctlCharge As New ctlChargeQuoted
        Dim dtChargeQuotedPART As New ChargeQuotedDTO.vwChargeQuotedPARTDataTable
        dtChargeQuotedPART = ctlCharge.QueryByChargeQuotedPART(Me.UI_lblPreviousPage_RMANO.Text, sRMAD_ID)

        If dtChargeQuotedPART.Rows.Count = 0 Then
            Me.UI_cmdApply.Visible = False
            Me.UI_cmdConfirm.Visible = False
        Else

            If ctlCharge.chkIsExist(Me.UI_lblPreviousPage_RMANO.Text) = False And ctlCharge.chkIsExistPART(Me.UI_lblPreviousPage_RMANO.Text) = False Then
                Me.UI_cmdApply.Visible = True
                Me.UI_cmdConfirm.Visible = True
            Else
                Me.UI_cmdApply.Visible = False
                Me.UI_cmdConfirm.Visible = False

                If Me.uiLbl_ApprovalStatus.Text = "0" Then
                    Me.UI_cmdApply.Visible = True
                    Me.UI_cmdConfirm.Visible = True
                End If
            End If

        End If

        '20190710 Isaac Mod Confirm都回主頁確認
        UI_cmdConfirm.Visible = False

        ChargeQuotedPART_DataBind(dtChargeQuotedPART, 0)
    End Sub

    Private Sub ChargeQuotedPART_DataBind(ByVal dtChargeQuotedPART As ChargeQuotedDTO.vwChargeQuotedPARTDataTable, ByVal iPageIndex As Integer)
        Session("_dtChargeQuotedPART") = dtChargeQuotedPART

        Me.UI_dvRepairDetail.DataSource = dtChargeQuotedPART.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub

    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblOriginalCharge As Label = e.Item.FindControl("lblOriginalCharge")
            Dim lblRenewCharge As Label = e.Item.FindControl("lblRenewCharge")

            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblOriginalCharge.Text = _oLanguage.getText("RMA", "446", ctlLanguage.eumType.Tag)
            lblRenewCharge.Text = _oLanguage.getText("RMA", "447", ctlLanguage.eumType.Tag)
        End If

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim UI_RMARQD_QTY As Label = e.Item.FindControl("UI_RMARQD_QTY")
            Dim UI_RMARQD_MATERIALCOST As Label = e.Item.FindControl("UI_RMARQD_MATERIALCOST")
            Dim UI_RMARQD_PRICE As Label = e.Item.FindControl("UI_RMARQD_PRICE")
            Dim UI_RMARQD_CURRENCYCODE As Label = e.Item.FindControl("UI_RMARQD_CURRENCYCODE")
            Dim UI_RMARQD_CURRENCYRATE As Label = e.Item.FindControl("UI_RMARQD_CURRENCYRATE")

            Dim RMARQD_QTY As Decimal = Convert.ToDecimal(UI_RMARQD_QTY.Text.Trim())
            Dim RMARQD_MATERIALCOST As Decimal = Convert.ToDecimal(UI_RMARQD_MATERIALCOST.Text.Trim())
            Dim RMARQD_PRICE As Decimal = Convert.ToDecimal(UI_RMARQD_PRICE.Text.Trim())
            Dim RMARQD_CURRENCYCODE As String = UI_RMARQD_CURRENCYCODE.Text.Trim()
            Dim RMARQD_CURRENCYRATE As Decimal = Convert.ToDecimal(UI_RMARQD_CURRENCYRATE.Text.Trim())




            Dim UI_RMACQPT_QTY As Label = e.Item.FindControl("UI_RMACQPT_QTY")
            Dim UI_RMACQPT_MATERIALCOST As Label = e.Item.FindControl("UI_RMACQPT_MATERIALCOST")
            Dim UI_RMACQPT_PRICE As Label = e.Item.FindControl("UI_RMACQPT_PRICE")
            Dim UI_RMACQPT_CURRENCYCODE As Label = e.Item.FindControl("UI_RMACQPT_CURRENCYCODE")
            Dim UI_RMACQPT_CURRENCYRATE As Label = e.Item.FindControl("UI_RMACQPT_CURRENCYRATE")

            If UI_RMACQPT_QTY.Text.Trim() = "" Then
                UI_RMACQPT_QTY.Text = RMARQD_QTY.ToString("N")
            End If
            If UI_RMACQPT_MATERIALCOST.Text.Trim() = "" Then
                UI_RMACQPT_MATERIALCOST.Text = RMARQD_MATERIALCOST.ToString("N")
            End If
            If UI_RMACQPT_PRICE.Text.Trim() = "" Then
                UI_RMACQPT_PRICE.Text = RMARQD_PRICE.ToString("N")
            End If
            If UI_RMACQPT_CURRENCYCODE.Text.Trim = "" Then
                UI_RMACQPT_CURRENCYCODE.Text = RMARQD_CURRENCYCODE
            End If
            If UI_RMACQPT_CURRENCYRATE.Text.Trim() = "" Then
                UI_RMACQPT_CURRENCYRATE.Text = RMARQD_CURRENCYRATE.ToString("N")
            End If




            Dim UI_RMACQPT_QTY_ORIGINAL As Label = e.Item.FindControl("UI_RMACQPT_QTY_ORIGINAL")
            Dim UI_RMACQPT_MATERIALCOST_ORIGINAL As Label = e.Item.FindControl("UI_RMACQPT_MATERIALCOST_ORIGINAL")
            Dim UI_RMACQPT_PRICE_ORIGINAL As Label = e.Item.FindControl("UI_RMACQPT_PRICE_ORIGINAL")
            Dim UI_RMACQPT_CURRENCYCODE_ORIGINAL As Label = e.Item.FindControl("UI_RMACQPT_CURRENCYCODE_ORIGINAL")
            Dim UI_RMACQPT_CURRENCYRATE_ORIGINAL As Label = e.Item.FindControl("UI_RMACQPT_CURRENCYRATE_ORIGINAL")

            If UI_RMACQPT_QTY_ORIGINAL.Text.Trim() = "" Then
                UI_RMACQPT_QTY_ORIGINAL.Text = RMARQD_QTY.ToString("N")
            End If
            If UI_RMACQPT_MATERIALCOST_ORIGINAL.Text.Trim() = "" Then
                UI_RMACQPT_MATERIALCOST_ORIGINAL.Text = RMARQD_MATERIALCOST.ToString("N")
            End If
            If UI_RMACQPT_PRICE_ORIGINAL.Text.Trim() = "" Then
                UI_RMACQPT_PRICE_ORIGINAL.Text = RMARQD_PRICE.ToString("N")
            End If
            If UI_RMACQPT_CURRENCYCODE_ORIGINAL.Text.Trim = "" Then
                UI_RMACQPT_CURRENCYCODE_ORIGINAL.Text = RMARQD_CURRENCYCODE.Trim()
            End If
            If UI_RMACQPT_CURRENCYRATE_ORIGINAL.Text.Trim() = "" Then
                UI_RMACQPT_CURRENCYRATE_ORIGINAL.Text = RMARQD_CURRENCYRATE.ToString("N")
            End If



            'lblRMARQD_QTY
            Dim lblRMARQD_QTY As Label = e.Item.FindControl("lblRMARQD_QTY")
            lblRMARQD_QTY.Text = UI_RMACQPT_QTY.Text

            'lblRMARQD_PRICE
            Dim lblRMARQD_PRICE As Label = e.Item.FindControl("lblRMARQD_PRICE")
            lblRMARQD_PRICE.Text = UI_RMACQPT_PRICE.Text



            'Renew Charge
            Dim UILbl_RMACQPT_RECHARGE_PRICE As Label = e.Item.FindControl("UILbl_RMACQPT_RECHARGE_PRICE")
            Dim UI_RMACQPT_RECHARGE_PRICE As TextBox = e.Item.FindControl("UI_RMACQPT_RECHARGE_PRICE")
            If UI_RMACQPT_RECHARGE_PRICE.Text.Trim() = "" Then
                UILbl_RMACQPT_RECHARGE_PRICE.Text = RMARQD_PRICE.ToString("N")
                UI_RMACQPT_RECHARGE_PRICE.Text = RMARQD_PRICE.ToString("N")

                UILbl_RMACQPT_RECHARGE_PRICE.Visible = False
                UI_RMACQPT_RECHARGE_PRICE.Visible = False
                If Convert.ToDecimal(UI_RMACQPT_PRICE.Text) = 0 Then
                    UILbl_RMACQPT_RECHARGE_PRICE.Visible = True
                Else
                    UI_RMACQPT_RECHARGE_PRICE.Visible = True
                End If
            End If

            If Me.UI_cmdApply.Visible = False And Me.UI_cmdConfirm.Visible = False Then
                UILbl_RMACQPT_RECHARGE_PRICE.Visible = True
                UI_RMACQPT_RECHARGE_PRICE.Visible = False
            End If


            Dim cvAfterDiscount As CustomValidator = e.Item.FindControl("cvAfterDiscount")
            cvAfterDiscount.Attributes.Add("Control01", lblRMARQD_PRICE.ClientID)
            cvAfterDiscount.Attributes.Add("Control02", UI_RMACQPT_RECHARGE_PRICE.ClientID)
            cvAfterDiscount.ErrorMessage = _oLanguage.getText("RMA", "454", ctlLanguage.eumType.Validator)

        End If

    End Sub

    ''' <summary>
    ''' 儲存 RMACharge_QUOTED 及 RMACHARGE_QUOTED_SN, RMASALE_QUOTED_CLOG, RMAREPAIR_QUOTED_CLOG, RMAREPAIR_QUOTED_DETAIL_CLOG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdApply.Click

        saveChargeQUOTED(False)

    End Sub

    ''' <summary>
    ''' 儲存 RMACharge_QUOTED 及 RMACHARGE_QUOTED_SN, RMASALE_QUOTED_CLOG, RMAREPAIR_QUOTED_CLOG, RMAREPAIR_QUOTED_DETAIL_CLOG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click

        saveChargeQUOTED(True)

    End Sub

#Region "save"

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub saveChargeQUOTED(ByVal isConfirm As Boolean)
        Dim i As Integer = 0
        Dim blnFlag As Boolean
        Dim sMessage As String = ""

        Dim ctlChargeQuoted As New ctlChargeQuoted()
        Dim dtRMACharge_QUOTED As New ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable
        Dim dtChargeQUOTED_SN As New ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable
        Dim dtRMACHARGE_QUOTED_PART As New ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable

        Dim total_DISCOUNTAMOUNT As Decimal = 0
        Dim RMACQ_DISCOUNT As Decimal = 0

        Try

            'RMACHARGE_QUOTED_PART
            Dim dtChargeQuotedPART As New ChargeQuotedDTO.vwChargeQuotedPARTDataTable
            dtChargeQuotedPART = ctlChargeQuoted.QueryByChargeQuotedPART(Me.UI_lblPreviousPage_RMANO.Text, "")
            dtRMACHARGE_QUOTED_PART = Save_RMACHARGE_QUOTED_PART(dtChargeQuotedPART)


            'RMACHARGE_QUOTED_SN 
            Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
            dtChargeQuotedSN = ctlChargeQuoted.QueryByChargeQuotedSN(Session("_LanguageID").ToString().Trim(), Me.UI_lblPreviousPage_RMANO.Text, "")
            dtChargeQUOTED_SN = Save_RMACHARGE_QUOTED_SN(dtChargeQuotedSN, dtRMACHARGE_QUOTED_PART, total_DISCOUNTAMOUNT)


            'RMACHARGE_QUOTED
            Dim dtChargeQuotedHead As New ChargeQuotedDTO.vwChargeQuotedHeadDataTable
            dtChargeQuotedHead = ctlChargeQuoted.QueryByChargeQuotedHead(Me.UI_lblPreviousPage_RMANO.Text)
            dtRMACharge_QUOTED = Save_RMACharge_QUOTED(dtChargeQuotedHead, total_DISCOUNTAMOUNT, RMACQ_DISCOUNT)

            ctlChargeQuoted.saveChargeQUOTED(dtRMACharge_QUOTED, dtChargeQUOTED_SN, dtRMACHARGE_QUOTED_PART)

            If isConfirm = True Then
                '    runSP_INS_AR22()
            End If

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Server.Transfer("ChargeQuoted_Item.aspx")
                'Response.Redirect("ChargeQuoted_List.aspx")
            End If

        End Try


    End Sub

    Private Function Save_RMACHARGE_QUOTED_PART(ByVal dtChargeQuotedPART As ChargeQuotedDTO.vwChargeQuotedPARTDataTable) As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable
        Dim i As Integer = 0

        Dim dtChargeQUOTED_PART As New ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable
        Dim myView As DataView = dtChargeQUOTED_PART.DefaultView

        Try

            For i = 0 To dtChargeQuotedPART.Rows.Count - 1
                Dim drPART As ChargeQuotedDTO.vwChargeQuotedPARTRow = dtChargeQuotedPART.Rows(i)

                Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTRow = dtChargeQUOTED_PART.NewRMACHARGE_QUOTED_PARTRow
                dr.RMACQPT_ID = Guid.NewGuid.ToString()
                dr.RMACQPT_RMARQD_ID = drPART.RMARQD_ID
                dr.RMACQPT_RMADID = drPART.RMARQD_RMADID

                dr.RMACQPT_QTY = Convert.ToDecimal(drPART.RMARQD_QTY)
                If drPART.IsRMACQPT_QTYNull = False Then
                    dr.RMACQPT_QTY = Convert.ToDecimal(drPART.RMACQPT_QTY)
                End If

                dr.RMACQPT_MATERIALCOST = Convert.ToDecimal(drPART.RMARQD_MATERIALCOST)
                If drPART.IsRMACQPT_MATERIALCOSTNull = False Then
                    dr.RMACQPT_MATERIALCOST = Convert.ToDecimal(drPART.RMACQPT_MATERIALCOST)
                End If

                dr.RMACQPT_PRICE = Convert.ToDecimal(drPART.RMARQD_PRICE)
                If drPART.IsRMACQPT_PRICENull = False Then
                    dr.RMACQPT_PRICE = Convert.ToDecimal(drPART.RMACQPT_PRICE)
                End If

                dr.RMACQPT_CURRENCYCODE = drPART.RMARQD_CURRENCYCODE
                If drPART.IsRMACQPT_CURRENCYCODENull = False Then
                    dr.RMACQPT_CURRENCYCODE = drPART.RMACQPT_CURRENCYCODE
                End If

                dr.RMACQPT_CURRENCYRATE = Convert.ToDecimal(drPART.RMARQD_CURRENCYRATE)
                If drPART.IsRMACQPT_CURRENCYRATENull = False Then
                    dr.RMACQPT_CURRENCYRATE = drPART.RMACQPT_CURRENCYRATE
                End If

                dr.RMACQPT_QTY_ORIGINAL = Convert.ToDecimal(drPART.RMARQD_QTY)
                If drPART.IsRMACQPT_QTY_ORIGINALNull = False Then
                    dr.RMACQPT_QTY_ORIGINAL = drPART.RMACQPT_QTY_ORIGINAL
                End If

                dr.RMACQPT_MATERIALCOST_ORIGINAL = Convert.ToDecimal(drPART.RMARQD_MATERIALCOST)
                If drPART.IsRMACQPT_MATERIALCOST_ORIGINALNull = False Then
                    dr.RMACQPT_MATERIALCOST_ORIGINAL = drPART.RMACQPT_MATERIALCOST_ORIGINAL
                End If

                dr.RMACQPT_PRICE_ORIGINAL = Convert.ToDecimal(drPART.RMARQD_PRICE)
                If drPART.IsRMACQPT_PRICE_ORIGINALNull = False Then
                    dr.RMACQPT_PRICE_ORIGINAL = drPART.RMACQPT_PRICE_ORIGINAL
                End If

                dr.RMACQPT_CURRENCYCODE_ORIGINAL = drPART.RMARQD_CURRENCYCODE
                If drPART.IsRMACQPT_CURRENCYCODE_ORIGINALNull = False Then
                    dr.RMACQPT_CURRENCYCODE_ORIGINAL = drPART.RMACQPT_CURRENCYCODE_ORIGINAL
                End If

                dr.RMACQPT_CURRENCYRATE_ORIGINAL = Convert.ToDecimal(drPART.RMARQD_CURRENCYRATE)
                If drPART.IsRMACQPT_CURRENCYRATE_ORIGINALNull = False Then
                    dr.RMACQPT_CURRENCYRATE_ORIGINAL = drPART.RMACQPT_CURRENCYRATE_ORIGINAL
                End If


                dr.RMACQPT_RECHARGE_PRICE = Convert.ToDecimal(drPART.RMARQD_PRICE)
                If drPART.IsRMACQPT_RECHARGE_PRICENull = False Then
                    dr.RMACQPT_RECHARGE_PRICE = drPART.RMACQPT_RECHARGE_PRICE
                End If


                dr.RMACQPT_AD = Session("_UserID")
                dr.RMACQPT_ADNAME = Session("_UserName")
                dr.RMACQPT_CSTMP = Date.Now
                dr.RMACQPT_LUAD = Session("_UserID")
                dr.RMACQPT_LUADNAME = Session("_UserName")
                dr.RMACQPT_LUSTMP = Date.Now


                dtChargeQUOTED_PART.AddRMACHARGE_QUOTED_PARTRow(dr)
            Next


            For i = 0 To Me.UI_dvRepairDetail.Items.Count - 1
                If Me.UI_dvRepairDetail.Items(i).ItemType = ListItemType.Item Or Me.UI_dvRepairDetail.Items(i).ItemType = ListItemType.AlternatingItem Then

                    Dim UI_RMARQD_ID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_ID")
                    Dim UI_RMARQD_RMADID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_RMADID")

                    myView.RowFilter = "RMACQPT_RMARQD_ID='" & UI_RMARQD_ID.Text.Trim() & "' AND RMACQPT_RMADID='" & UI_RMARQD_RMADID.Text.Trim() & "'"
                    If myView.Count > 0 Then
                        Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTRow = myView(0).Row

                        'Dim UI_RMACQPT_QTY As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_QTY")
                        'Dim UI_RMACQPT_MATERIALCOST As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_MATERIALCOST")
                        'Dim UI_RMACQPT_PRICE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_PRICE")
                        'Dim UI_RMACQPT_CURRENCYCODE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_CURRENCYCODE")
                        'Dim UI_RMACQPT_CURRENCYRATE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_CURRENCYRATE")

                        'dr.RMACQPT_QTY = Convert.ToDecimal(UI_RMACQPT_QTY.Text)
                        'dr.RMACQPT_MATERIALCOST = Convert.ToDecimal(UI_RMACQPT_MATERIALCOST.Text)
                        'dr.RMACQPT_PRICE = Convert.ToDecimal(UI_RMACQPT_PRICE.Text)
                        'dr.RMACQPT_CURRENCYCODE = UI_RMACQPT_CURRENCYCODE.Text.Trim()
                        'dr.RMACQPT_CURRENCYRATE = Convert.ToDecimal(UI_RMACQPT_CURRENCYRATE.Text)


                        'Dim UI_RMACQPT_QTY_ORIGINAL As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_QTY_ORIGINAL")
                        'Dim UI_RMACQPT_MATERIALCOST_ORIGINAL As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_MATERIALCOST_ORIGINAL")
                        'Dim UI_RMACQPT_PRICE_ORIGINAL As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_PRICE_ORIGINAL")
                        'Dim UI_RMACQPT_CURRENCYCODE_ORIGINAL As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_CURRENCYCODE_ORIGINAL")
                        'Dim UI_RMACQPT_CURRENCYRATE_ORIGINAL As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_CURRENCYRATE_ORIGINAL")

                        'dr.RMACQPT_QTY_ORIGINAL = Convert.ToDecimal(UI_RMACQPT_QTY_ORIGINAL.Text)
                        'dr.RMACQPT_MATERIALCOST_ORIGINAL = Convert.ToDecimal(UI_RMACQPT_MATERIALCOST_ORIGINAL.Text)
                        'dr.RMACQPT_PRICE_ORIGINAL = Convert.ToDecimal(UI_RMACQPT_PRICE_ORIGINAL.Text)
                        'dr.RMACQPT_CURRENCYCODE_ORIGINAL = UI_RMACQPT_CURRENCYCODE_ORIGINAL.Text.Trim()
                        'dr.RMACQPT_CURRENCYRATE_ORIGINAL = Convert.ToDecimal(UI_RMACQPT_CURRENCYRATE_ORIGINAL.Text)

                        Dim UI_RMACQPT_RECHARGE_PRICE As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMACQPT_RECHARGE_PRICE")
                        dr.RMACQPT_RECHARGE_PRICE = Convert.ToDecimal(UI_RMACQPT_RECHARGE_PRICE.Text)
                    End If

                End If
            Next

            myView.RowFilter = ""

        Catch ex As Exception
            Throw ex
        End Try


        Return dtChargeQUOTED_PART
    End Function

    Private Function Save_RMACHARGE_QUOTED_SN(ByVal dtChargeQuotedSN As ChargeQuotedDTO.vwChargeQuotedSNDataTable, ByVal dtRMACHARGE_QUOTED_PART As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable,
        ByRef total_DISCOUNTAMOUNT As Decimal) As ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable

        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim dtChargeQUOTED_SN As New ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable
        Dim myView As DataView = dtRMACHARGE_QUOTED_PART.DefaultView


        Try
            For i = 0 To dtChargeQuotedSN.Rows.Count - 1
                Dim drSN As ChargeQuotedDTO.vwChargeQuotedSNRow = dtChargeQuotedSN.Rows(i)
                Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTED_SNRow = dtChargeQUOTED_SN.NewRMACHARGE_QUOTED_SNRow

                dr.RMACQSN_ID = Guid.NewGuid.ToString()
                dr.RMACQSN_RMADID = drSN.RMAD_ID


                dr.RMACQSN_LABORCOST = drSN.RMASQ_LABORCOST
                If drSN.IsRMACQSN_LABORCOSTNull = False Then
                    dr.RMACQSN_LABORCOST = drSN.RMACQSN_LABORCOST
                End If

                dr.RMACQSN_MATERIALCOST = drSN.RMASQ_MATERIALCOST
                If drSN.IsRMACQSN_MATERIALCOSTNull = False Then
                    dr.RMACQSN_MATERIALCOST = drSN.RMACQSN_MATERIALCOST
                End If

                dr.RMACQSN_QUOTE = drSN.RMASQ_QUOTE
                If drSN.IsRMACQSN_QUOTENull = False Then
                    dr.RMACQSN_QUOTE = drSN.RMACQSN_QUOTE
                End If

                Dim Charge_MaterialCost As Decimal = 0
                myView.RowFilter = "RMACQPT_RMADID='" & dr.RMACQSN_RMADID & "'"
                If myView.Count > 0 Then
                    For j = 0 To myView.Count - 1
                        Charge_MaterialCost = Charge_MaterialCost + Convert.ToDecimal(myView(j)("RMACQPT_RECHARGE_PRICE").ToString())
                    Next
                    dr.Charge_MaterialCost = Charge_MaterialCost
                End If

                dr.RMACQSN_DISCOUNTAMOUNT = drSN.RMASQ_QUOTE
                If drSN.IsRMACQSN_DISCOUNTAMOUNTNull = False Then
                    dr.RMACQSN_DISCOUNTAMOUNT = drSN.RMACQSN_DISCOUNTAMOUNT
                End If
                If dr.IsCharge_MaterialCostNull = False Then
                    dr.RMACQSN_DISCOUNTAMOUNT = (dr.RMACQSN_LABORCOST + dr.RMACQSN_MATERIALCOST) - (dr.RMACQSN_MATERIALCOST - dr.Charge_MaterialCost)
                End If
                total_DISCOUNTAMOUNT = total_DISCOUNTAMOUNT + dr.RMACQSN_DISCOUNTAMOUNT


                dr.RMACQSN_CURRENCYCODE = drSN.RMASQ_CURRENCYCODE
                If drSN.IsRMACQSN_CURRENCYCODENull = False Then
                    dr.RMACQSN_CURRENCYCODE = drSN.RMACQSN_CURRENCYCODE
                End If

                dr.RMACQSN_CURRENCYRATE = drSN.RMASQ_CURRENCYRATE
                If drSN.IsRMACQSN_CURRENCYRATENull = False Then
                    dr.RMACQSN_CURRENCYRATE = drSN.RMACQSN_CURRENCYRATE
                End If


                dr.RMACQSN_LABORCOST_ORIGINAL = drSN.RMASQ_LABORCOST
                dr.RMACQSN_MATERIALCOST_ORIGINAL = drSN.RMASQ_MATERIALCOST
                dr.RMACQSN_QUOTE_ORIGINAL = drSN.RMASQ_QUOTE
                dr.RMACQSN_CURRENCYCODE_ORIGINAL = drSN.RMASQ_CURRENCYCODE
                dr.RMACQSN_CURRENCYRATE_ORIGINAL = drSN.RMASQ_CURRENCYRATE



                dr.RMACQSN_QUOTE_ORIGINAL = drSN.RMASQ_QUOTE
                dr.RMACQSN_CURRENCYCODE_ORIGINAL = drSN.RMASQ_CURRENCYCODE
                dr.RMACQSN_CURRENCYRATE_ORIGINAL = drSN.RMASQ_CURRENCYRATE

                dr.RMACQSN_AD = Session("_UserID")
                dr.RMACQSN_ADNAME = Session("_UserName")
                dr.RMACQSN_CSTMP = Date.Now
                dr.RMACQSN_LUAD = Session("_UserID")
                dr.RMACQSN_LUADNAME = Session("_UserName")
                dr.RMACQSN_LUSTMP = Date.Now

                dtChargeQUOTED_SN.AddRMACHARGE_QUOTED_SNRow(dr)
            Next


            myView.RowFilter = ""

        Catch ex As Exception
            Throw ex
        End Try


        Return dtChargeQUOTED_SN
    End Function

    Private Function Save_RMACharge_QUOTED(ByVal dtChargeQuotedHead As ChargeQuotedDTO.vwChargeQuotedHeadDataTable, ByVal RMACQ_SALEQUOTE As Decimal, ByVal RMACQ_DISCOUNT As Decimal) As ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable
        Dim dtRMACharge_QUOTED As New ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable

        Try

            If dtChargeQuotedHead.Rows.Count > 0 Then
                Dim drHead As ChargeQuotedDTO.vwChargeQuotedHeadRow = dtChargeQuotedHead.Rows(0)

                Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTEDRow = dtRMACharge_QUOTED.NewRMACHARGE_QUOTEDRow
                dr.RMACQ_ID = Guid.NewGuid.ToString()

                dr.RMACQ_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
                dr.RMACQ_CHARGEDATE = Date.Now

                dr.RMACQ_SALEQUOTE = RMACQ_SALEQUOTE
                dr.RMACQ_DISCOUNT = RMACQ_DISCOUNT
                dr.RMACQ_CHARGEQUOTE = RMACQ_SALEQUOTE - RMACQ_DISCOUNT

                dr.RMACQ_CURRENCYCODE = drHead.RMASQ_CURRENCYCODE
                dr.RMACQ_CURRENCYRATE = drHead.RMASQ_CURRENCYRATE

                dr.RMACQ_QUOTE_ORIGINAL = drHead.RMASQ_QUOTE
                dr.RMACQ_CURRENCYCODE_ORIGINAL = drHead.RMASQ_CURRENCYCODE
                dr.RMACQ_CURRENCYRATE_ORIGINAL = drHead.RMASQ_CURRENCYRATE

                dr.RMACQ_APPROVAL = 0

                dr.RMACQ_AD = Session("_UserID")
                dr.RMACQ_ADNAME = Session("_UserName")
                dr.RMACQ_CSTMP = Date.Now
                dr.RMACQ_LUAD = Session("_UserID")
                dr.RMACQ_LUADNAME = Session("_UserName")
                dr.RMACQ_LUSTMP = Date.Now

                dtRMACharge_QUOTED.AddRMACHARGE_QUOTEDRow(dr)
            End If

        Catch ex As Exception
            Throw ex
        End Try


        Return dtRMACharge_QUOTED
    End Function

    Public Function runSP_INS_AR22() As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        Try

            'response.write (Me.hid_CU_TIPTOP_ID.Text.Trim() & "<br>")
            'response.write (Me.UI_RMANo.Text.Trim() & "<br>")
            'response.write (Session("_UserID").ToString().Trim() & "<br>")

            oCommand.CommandText = "SP_INS_AR22"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("vCUSTNO", OracleType.NVarChar).Value = Me.hid_CU_TIPTOP_ID.Text.Trim()
            oCommand.Parameters("vCUSTNO").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vRMANO", OracleType.NVarChar).Value = Me.UI_RMANo.Text.Trim()
            oCommand.Parameters("vRMANO").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vUserNo", OracleType.NVarChar).Value = Session("_UserID").ToString().Trim()
            oCommand.Parameters("vUserNo").Direction = ParameterDirection.Input
            oCommand.ExecuteNonQuery()

            oCommand.Parameters.Add("vCompno", OracleType.NVarChar).Value = Session("_UserID").ToString().Trim()
            oCommand.Parameters("vCompno").Direction = ParameterDirection.Input
            oCommand.ExecuteNonQuery()


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

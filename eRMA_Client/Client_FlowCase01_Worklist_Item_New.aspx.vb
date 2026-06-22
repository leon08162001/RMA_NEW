Imports System.Data
Imports DataService
Imports DefLanguage
Imports Newtonsoft.Json

Partial Class Client_FlowCase01_Worklist_Item_New
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _Customer_ExceptionCharge As String = ConfigurationSettings.AppSettings("Customer_ExceptionCharge")

#Region "Class 顯示詳細資料"

    Public Class Client_FlowCase01_Worklist_Item_New_Json
        Private sRMANO As String

        Public Property RMANO() As String
            Get
                Return sRMANO
            End Get
            Set(ByVal value As String)
                sRMANO = value
            End Set
        End Property

        Private sRMADID As String

        Public Property RMADID() As String
            Get
                Return sRMADID
            End Get
            Set(ByVal value As String)
                sRMADID = value
            End Set
        End Property

        Private sRMA_ID As String

        Public Property RMA_ID() As String
            Get
                Return sRMA_ID
            End Get
            Set(ByVal value As String)
                sRMA_ID = value
            End Set
        End Property


        Private sRMAD_STATUS As String

        Public Property RMAD_STATUS() As String
            Get
                Return sRMAD_STATUS
            End Get
            Set(ByVal value As String)
                sRMAD_STATUS = value
            End Set
        End Property

    End Class
#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtRMAClient_FlowCase01_Part") = Nothing

            If 1 = 1 Then

                Dim Obj As String = Request.Params("DATA").ToString().Trim()
                Obj = HttpUtility.UrlDecode(Obj)

                Dim Client_FlowCase01_Worklist_Item_New_Json_D As New Client_FlowCase01_Worklist_Item_New_Json()
                Client_FlowCase01_Worklist_Item_New_Json_D = JsonConvert.DeserializeObject(Of Client_FlowCase01_Worklist_Item_New_Json)(Obj)
                Me.UI_lblPreviousPage_RMANO.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMANO.ToString().Trim()
                Me.UI_lblPreviousPage_RMAID.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMA_ID.ToString().Trim()
                Me.UI_lblPreviousPage_RMADID.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMADID.ToString().Trim()
                Me.UI_lblRMAD_STATUS.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMAD_STATUS.ToString().Trim()

                Call setControls()
                Call QueryData()

                Call QueryByRepairQuoted()
                Call QueryByRepairQuotedDetail()

                Me.UI_cmdConfirm.Visible = False


            End If

        End If
    End Sub
#End Region

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
        'Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)

        Me.UI_lblRequestedTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)

        Me.lbl_Manpower.Text = _oLanguage.getText("RMA2", "039", ctlLanguage.eumType.Tag)
        Me.uiLbl_Parts.Text = _oLanguage.getText("RMA", "087", ctlLanguage.eumType.Tag)
        Me.uiLbl_TotalAmountText.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)

        Me.UI_btnDetail.Text = _oLanguage.getText("Transfer", "037", ctlLanguage.eumType.Word)

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
            'If dr.IsRMA_RemarkNull = False Then Me.UI_Remark.Text = dr.RMA_Remark
        End If
    End Sub
    Private Sub QueryByRepairQuoted()
        Me.UI_RMARQ_LABORHOUR.Text = ""
        'Me.uiLbl_Repair_PartsTotal.Text = ""
        'Me.uiLbl_Repair_Total.Text = ""


        Dim oRepairQuoting As New ctlRMA.Repair_Quoting
        Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable
        dtRepairQuoting = oRepairQuoting.Query(Me.UI_lblPreviousPage_RMADID.Text, "91")
        If dtRepairQuoting.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_QuotingRow = dtRepairQuoting.Rows(0)

            Me.UI_RMARQ_ID.Text = dr.RMARQ_ID.Trim()
            Me.UI_RMA_CUNO.Text = dr.RMA_CUNO.Trim()

            QueryCustomer(Me.UI_RMA_CUNO.Text)

            Me.UI_RMAD_ISWARRANTY.Text = "0"
            If dr.IsRMAD_ISWARRANTYNull = False Then
                Me.UI_RMAD_ISWARRANTY.Text = dr.RMAD_ISWARRANTY.ToString()
            End If

            'MODY BY Angel ON 20160815 增加是否有人為破壞
            Me.UI_RMARQ_IMPROPERUSAGE.Text = "0"
            If dr.IsRMARQ_IMPROPERUSAGENull = False Then
                Me.UI_RMARQ_IMPROPERUSAGE.Text = dr.RMARQ_IMPROPERUSAGE.ToString()
            End If

            Me.UI_RMAD_ISCW.Text = "0"
            If dr.IsRMAD_ISCWNull = False Then
                Me.UI_RMAD_ISCW.Text = dr.RMAD_ISCW.ToString()
            End If



            '==========================================================================================================================================================================================================================================
            '費用
            '==========================================================================================================================================================================================================================================
            If dr.IsRMARQ_LABORHOURNull = False Then
                Me.UI_RMARQ_LABORHOUR.Text = dr.RMARQ_LABORHOUR.ToString().Trim()       '工時
            End If
            If dr.IsRMARQ_LABORPRICENull = False Then
                Me.UI_RMARQ_LABORPRICE.Text = dr.RMARQ_LABORPRICE.ToString().Trim()     '工時單價
            End If

            If dr.IsRMARQ_MATERIALCOSTNull = False Then
                Me.UI_RMARQ_MATERIALCOST.Text = dr.RMARQ_MATERIALCOST.ToString().Trim() '零件費用
            End If

            If dr.IsRMARQ_QUOTENull = False Then
                Me.UI_RMARQ_QUOTE.Text = dr.RMARQ_QUOTE.ToString().Trim()            '費用加總(報價)
            End If

            If dr.IsRMARQ_CURRENCYRATENull = False Then
                Me.UI_RMARQ_CURRENCYRATE.Text = dr.RMARQ_CURRENCYRATE.ToString().Trim()            '被指派的維修中心 - 兌美金匯率
            End If
            If dr.IsRMARQ_ASSIGECURRENCYRATENull = False Then
                Me.UI_RMARQ_ASSIGECURRENCYRATE.Text = dr.RMARQ_ASSIGECURRENCYRATE.ToString().Trim()            '指派的維修中心 - 兌美金匯率
            End If

        End If

    End Sub

#Region "Replace Component"
    Private Sub QueryByRepairQuotedDetail()
        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        dtRepairQuotedDetail = oRepairQuoted.QueryByRepairQuotedDetail_Client(Me.UI_lblPreviousPage_RMADID.Text)

        Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail, 0)
    End Sub

    Private Sub RepairQuotedDetail_DataBind(ByVal dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairQuotedDetail_Client") = dtRepairQuotedDetail
        Dim dvRepairDetail As DataView = dtRepairQuotedDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARQD_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairQuotedDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub

    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblWaive As Label = e.Item.FindControl("lblWaive")
            Dim lblOption As Label = e.Item.FindControl("lblOption")
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHLocation As Label = e.Item.FindControl("lblHLocation")
            Dim lblHImproper As Label = e.Item.FindControl("lblHImproper")
            Dim lblHQty As Label = e.Item.FindControl("lblHQty")
            Dim lblHPrice As Label = e.Item.FindControl("lblHPrice")
            lblWaive.Text = _oLanguage.getText("RMA", "405", ctlLanguage.eumType.Tag)
            lblOption.Text = _oLanguage.getText("RMA", "406", ctlLanguage.eumType.Tag)
            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHLocation.Text = _oLanguage.getText("RMA", "100", ctlLanguage.eumType.Tag)
            lblHImproper.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
            lblHQty.Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
            lblHPrice.Text = _oLanguage.getText("RMA", "104", ctlLanguage.eumType.Tag)
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'e.Item.BackColor = Drawing.Color.Pink
            'If chhWaive.Checked = True Then
            'End If

            'Dim lblNew As Label = e.Item.FindControl("lblNew")
            'lblNew.Text = _oLanguage.getText("RMA", "105", ctlLanguage.eumType.Tag)

            'RMARQD_WAIVE --> 表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
            'RMARQD_OPTION -->  維修中心定義此為可換或可不換料件，即為option的意思，客戶確認時歸為兩區，前區為不可勾選區（必修），後區為可勾選區，客戶可選擇不修。
            'RMARQD_OPTIONCLIENT --> 客戶可選擇是否要修-->1.不修, 2.要修
            Dim chhWaive As CheckBox = e.Item.FindControl("chhWaive")
            Dim lblRMARQD_PRICE_Label As Label = e.Item.FindControl("lblRMARQD_PRICE")
            Dim UI_lblRMARQD_PRICE_Label As Label = e.Item.FindControl("UI_lblRMARQD_PRICE")
            Dim UI_lblRMARQD_PRICE_Cancel_Label As Label = e.Item.FindControl("UI_lblRMARQD_PRICE_Cancel")



            'If chhWaive.Checked = True Then
            '    lblRMARQD_PRICE_Label.Visible = True
            '    UI_lblRMARQD_PRICE_Label.Visible = False
            '    UI_lblRMARQD_PRICE_Cancel_Label.Visible = False
            'Else
            '    lblRMARQD_PRICE_Label.Visible = False
            '    UI_lblRMARQD_PRICE_Label.Visible = True
            '    UI_lblRMARQD_PRICE_Cancel_Label.Visible = True
            'End If

            Dim UI_RMARQD_OPTION As Label = e.Item.FindControl("UI_RMARQD_OPTION")
            Dim UI_RMARQD_OPTIONCLIENT As Label = e.Item.FindControl("UI_RMARQD_OPTIONCLIENT")
            Dim UI_chkOptionClent As CheckBox = e.Item.FindControl("UI_chkOptionClent")
            '顯示 RMARQD_OPTIONCLIENT 要修不修 開始
            'UI_chkOptionClent.Checked = False
            UI_chkOptionClent.Checked = False
            If UI_RMARQD_OPTIONCLIENT.Text = "2" Then
                UI_chkOptionClent.Checked = True
            End If

            '顯示 RMARQD_OPTIONCLIENT 要修不修 結束
            If chhWaive.Checked = True Then
                Dim oTable As System.Web.UI.WebControls.Table = e.Item.Controls(1)
                oTable.Rows(0).BackColor = Drawing.ColorTranslator.FromHtml("#e3e3e3")
                UI_chkOptionClent.Visible = False
            End If
            If UI_RMARQD_OPTION.Text.Trim() = 0 Then
                UI_chkOptionClent.Visible = False
            End If

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            If Me.UI_lblRMAD_STATUS.Text = "60" Then
                UI_chkOptionClent.Visible = False
            End If



            Dim lblIMPROPERUSAGE As Label = e.Item.FindControl("lblIMPROPERUSAGE")
            Dim UI_Improper As Label = e.Item.FindControl("UI_Improper")
            UI_Improper.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            If lblIMPROPERUSAGE.Text.Trim() = "1" Then
                UI_Improper.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            End If

            'Dim UI_cboDefective As DropDownList = e.Item.FindControl("UI_cboDefective")
            'Dim lblDEFECTIVE As Label = e.Item.FindControl("lblDEFECTIVE")
            'oCommon.getDefectiveByDropDownList(Session("_LanguageID"), UI_cboDefective)
            'UI_cboDefective.SelectedValue = lblDEFECTIVE.Text.Trim()
        End If

    End Sub

#End Region
    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click
        Dim i As Integer = 0
        Dim blnFlag As Boolean
        Dim sMessage As String = ""

        Dim dtRepairQuoted_Detail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
        Dim ctlRMA As New ctlRMA.Repair_Quoting

        Try
            Dim TotalPrice_Parts As Double = 0

            For i = 0 To Me.UI_dvRepairDetail.Items.Count - 1
                Dim lblRMARQDID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQDID")
                Dim lblRMARQD_RMADID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_RMADID")
                Dim lblRMARQD_NPARTNO As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_NPARTNO")
                Dim lblIMPROPERUSAGE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblIMPROPERUSAGE")

                Dim lblRMARQD_QTY As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_QTY")
                Dim UI_txtMaterialCost As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_txtMaterialCost")
                Dim lblRMARQD_PRICE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_PRICE")

                Dim lblRMARQD_CURRENCYCODE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CURRENCYCODE")
                Dim lblRMARQD_CURRENCYRATE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CURRENCYRATE")

                Dim lblRMARQD_ASSIGECURRENCYCODE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ASSIGECURRENCYCODE")
                Dim lblRMARQD_ASSIGECURRENCYRATE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ASSIGECURRENCYRATE")
                Dim lblRMARQD_AD As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_AD")
                Dim lblRMARQD_ADNAME As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ADNAME")
                Dim lblRMARQD_CSTMP As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CSTMP")


                Dim UI_RMARQD_WAIVE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_WAIVE")
                Dim UI_RMARQD_ACC As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_ACC")
                Dim UI_chkOptionClent As CheckBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_chkOptionClent")

                Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuoted_Detail.NewRMAREPAIR_QUOTED_DETAILRow
                dr.RMARQD_ID = lblRMARQDID.Text.Trim()
                dr.RMARQD_RMADID = lblRMARQD_RMADID.Text.Trim()
                dr.RMARQD_NPARTNO = lblRMARQD_NPARTNO.Text.Trim()

                dr.RMARQD_IMPROPERUSAGE = Convert.ToInt16(lblIMPROPERUSAGE.Text.Trim())

                ' '' '' ''RMARQD_WAIVE: 表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                ' '' '' ''不修重新計算 RMARQD_PRICE
                '' '' ''Dim RMARQD_PRICE As Double = 0
                '' '' ''If Convert.ToInt16(UI_RMARQD_WAIVE.Text.Trim()) = 1 Then
                '' '' ''    RMARQD_PRICE = 0
                '' '' ''Else
                '' '' ''    Dim RMARQD_OPTIONCLIENT As Integer = 1
                '' '' ''    If UI_chkOptionClent.Checked = True Then
                '' '' ''        RMARQD_PRICE = Convert.ToDecimal(lblRMARQD_QTY.Text.Trim()) * Convert.ToDecimal(UI_txtMaterialCost.Text.Trim())
                '' '' ''        RMARQD_OPTIONCLIENT = 2
                '' '' ''    End If
                '' '' ''    'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
                '' '' ''    dr.RMARQD_OPTIONCLIENT = RMARQD_OPTIONCLIENT
                '' '' ''End If


                'RMARQD_WAIVE: 表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                dr.RMARQD_WAIVE = 0
                If UI_RMARQD_WAIVE.Text.Trim() = "1" Then
                    dr.RMARQD_WAIVE = 1
                End If

                dr.RMARQD_ACC = 0
                If UI_RMARQD_ACC.Text.Trim() = "1" Then
                    dr.RMARQD_ACC = 1
                End If

                dr.RMARQD_QTY = Convert.ToDecimal(lblRMARQD_QTY.Text.Trim())
                dr.RMARQD_MATERIALCOST = Convert.ToDecimal(UI_txtMaterialCost.Text.Trim())

                'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
                Dim RMARQD_PRICE As Double = 0
                Dim RMARQD_OPTIONCLIENT As Integer = 2
                If UI_chkOptionClent.Checked = False Then
                    RMARQD_PRICE = 0
                    RMARQD_OPTIONCLIENT = 1
                End If
                If UI_chkOptionClent.Checked = True Then
                    RMARQD_PRICE = PartsRule_Exception(dr.RMARQD_QTY, dr.RMARQD_MATERIALCOST, dr.RMARQD_WAIVE, dr.RMARQD_ACC)

                    RMARQD_OPTIONCLIENT = 2
                End If

                dr.RMARQD_PRICE = RMARQD_PRICE
                dr.RMARQD_OPTIONCLIENT = RMARQD_OPTIONCLIENT


                dr.RMARQD_CURRENCYCODE = lblRMARQD_CURRENCYCODE.Text.Trim()
                dr.RMARQD_CURRENCYRATE = Convert.ToDecimal(lblRMARQD_CURRENCYRATE.Text.Trim())

                dr.RMARQD_ASSIGECURRENCYCODE = lblRMARQD_ASSIGECURRENCYCODE.Text.Trim()
                dr.RMARQD_ASSIGECURRENCYRATE = Convert.ToDecimal(lblRMARQD_ASSIGECURRENCYRATE.Text.Trim())
                dr.RMARQD_ASSIGEPRICE = oCommon.ConvertCurrency(dr.RMARQD_CURRENCYRATE, dr.RMARQD_PRICE, dr.RMARQD_ASSIGECURRENCYRATE)

                dr.RMARQD_AD = lblRMARQD_AD.Text.Trim()
                dr.RMARQD_ADNAME = lblRMARQD_ADNAME.Text.Trim()
                dr.RMARQD_CSTMP = Convert.ToDateTime(lblRMARQD_CSTMP.Text.Trim())

                dr.RMARQD_LUAD = Session("_UserID")
                dr.RMARQD_LUADNAME = Session("_UserName")
                dr.RMARQD_LUSTMP = Date.Now
                dr.RMARQD_MARK = 0

                TotalPrice_Parts = TotalPrice_Parts + dr.RMARQD_PRICE
                dtRepairQuoted_Detail.Rows.Add(dr)
            Next


            'update  RMAREPAIR_QUOTED
            Dim RMARQ_LABORHOUR As Double = Convert.ToDecimal(Me.UI_RMARQ_LABORHOUR.Text)       '工時
            Dim RMARQ_LABORPRICE As Double = Convert.ToDecimal(Me.UI_RMARQ_LABORPRICE.Text)     '工時單價
            Dim RMARQ_MATERIALCOST As Double = TotalPrice_Parts  '零件費用
            Dim RMARQ_QUOTE As Double = (RMARQ_LABORHOUR * RMARQ_LABORPRICE) + RMARQ_MATERIALCOST '費用加總(報價)

            Dim RMARQ_CURRENCYRATE As Double = Convert.ToDecimal(Me.UI_RMARQ_CURRENCYRATE.Text)
            Dim RMARQ_ASSIGECURRENCYRATE As Double = Convert.ToDecimal(Me.UI_RMARQ_ASSIGECURRENCYRATE.Text)

            Dim RMARQ_ASSIGLABORCOST As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, (RMARQ_LABORHOUR * RMARQ_LABORPRICE), RMARQ_ASSIGECURRENCYRATE)  '轉換成 指派的維修中心 --> 工時費用
            Dim RMARQ_ASSIGMATERIALCOST As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, RMARQ_MATERIALCOST, RMARQ_ASSIGECURRENCYRATE)                 '轉換成 指派的維修中心 --> 零件費用
            Dim RMARQ_ASSIGEQUOTE As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, RMARQ_QUOTE, RMARQ_ASSIGECURRENCYRATE)                              '轉換成 指派的維修中心 --> 費用加總(報價)

            'update  RMAREPAIR_QUOTED_DETAIL
            ctlRMA.Edit_RepairQuotedDetail(Me.UI_RMARQ_ID.Text.Trim(), RMARQ_MATERIALCOST, RMARQ_QUOTE, RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, dtRepairQuoted_Detail)


            'update RMASALE_QUOTED
            Dim ctlSale As New ctlRMA.Sale
            Dim RMASQ_LABORCOST As Decimal = RMARQ_LABORHOUR * RMARQ_LABORPRICE
            ctlSale.Edit_SalesQuoted(Me.UI_lblPreviousPage_RMADID.Text.Trim(), RMASQ_LABORCOST, RMARQ_MATERIALCOST, RMARQ_QUOTE, Session("_UserID").ToString(), Session("_UserName").ToString(), Date.Now)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                Dim sScript As String = "<script type=""text/javascript""> window.parent.Open_Client_FlowCase01_Worklist();</script>"
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "OpenMsg", sScript)

                'Server.Transfer("Client_FlowCase01_Worklist.aspx")
            End If

        End Try


    End Sub
#Region "相關金額計算"
    ''' <summary>
    ''' 取得客戶 Discount 及 Service Charge
    ''' </summary>
    ''' <param name="CU_NO"></param>
    ''' <remarks></remarks>
    Private Sub QueryCustomer(ByVal CU_NO As String)
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable

        Me.UI_CU_DISCOUNT_OFF.Text = "1"

        Dim ctlCustomer As New ctlCustomer.Customer
        dtCustomer = ctlCustomer.QueryByCompany(CU_NO)
        If dtCustomer.Count > 0 Then
            Dim item As CustomerDTO.VWCUSTOMERRow = dtCustomer.Rows(0)
            If item.IsCU_DISCOUNT_OFFNull = False Then
                Dim CU_DISCOUNT_OFF As Double = 0
                If item.IsCU_DISCOUNT_OFFNull = False Then
                    CU_DISCOUNT_OFF = item.CU_DISCOUNT_OFF
                End If

                Dim CU_SERVICE_CHG As Double = 0
                If item.IsCU_SERVICE_CHGNull = False Then
                    CU_SERVICE_CHG = item.CU_SERVICE_CHG
                End If

                'Me.UI_lblDiscountText.Text = CU_DISCOUNT_OFF.ToString() + " OFF"
                Me.UI_CU_DISCOUNT_OFF.Text = (100 - CU_DISCOUNT_OFF) / 100
            End If
        End If
    End Sub

    ''' <summary>
    ''' 報價零件金額 - 例外規格計算
    ''' </summary>
    ''' <param name="RMARQD_MATERIALCOST"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PartsRule_Exception(ByVal RMARQD_QTY As Double, ByVal RMARQD_MATERIALCOST As Double, ByVal RMARQD_WAIVE As Integer, ByVal RMARQD_Acc As Integer) As Double
        Dim i As Integer = 0
        Dim RMARQD_PRICE As Double = 0
        Dim CU_DISCOUNT_OFF As Double = 1

        If Me.UI_CU_DISCOUNT_OFF.Text.Trim <> "" Then
            CU_DISCOUNT_OFF = Convert.ToDouble(Me.UI_CU_DISCOUNT_OFF.Text)
        End If

        '計算折扣後的零件金額
        If RMARQD_QTY > 0 And RMARQD_MATERIALCOST > 0 Then
            RMARQD_PRICE = Math.Round((RMARQD_QTY * RMARQD_MATERIALCOST) * CU_DISCOUNT_OFF, 2)
        End If

        '1. IF ISWARRANTY=Y OR ISCW=Y THEN RMARQD_PRICE = 0 
        'MODY BY Angel ON 20160815 增加是否有人為破壞
        If (Me.UI_RMAD_ISWARRANTY.Text = "1" Or Me.UI_RMAD_ISCW.Text = "1") And Me.UI_RMARQ_IMPROPERUSAGE.Text = "0" And RMARQD_Acc = "0" Then
            RMARQD_PRICE = 0
        End If

        ''2. 客戶編號為'Ni.'開頭的, RMARQD_PRICE = 0
        Dim arrCustomer() As String = _Customer_ExceptionCharge.Trim().Split(",")
        For i = 0 To arrCustomer.Length - 1
            If Me.UI_RMA_CUNO.Text.IndexOf(arrCustomer(i).ToString().Trim()) <> -1 Then
                RMARQD_PRICE = 0
                Exit For
            End If
        Next

        'waive：表示此零件是我方吸收必修，維修收費價格會是0；
        If RMARQD_WAIVE = 1 Then
            RMARQD_PRICE = 0
        End If

        Return RMARQD_PRICE
    End Function

#End Region
    Protected Sub UI_btnDetail_Click1(sender As Object, e As System.EventArgs) Handles UI_btnDetail.Click
        Dim UI_RMANO As String = Me.UI_lblPreviousPage_RMANO.Text.Trim()
        Dim UI_RMADID As String = Me.UI_lblPreviousPage_RMADID.Text.Trim()
        Me.ucClientDetail.show(UI_RMADID, UI_RMANO, True)
    End Sub
    Protected Sub UI_chkOptionClent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim i As Integer = 0
        Dim blnFlag As Boolean
        Dim sMessage As String = ""

        Dim dtRepairQuoted_Detail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
        Dim ctlRMA As New ctlRMA.Repair_Quoting

        Try
            Dim TotalPrice_Parts As Double = 0

            For i = 0 To Me.UI_dvRepairDetail.Items.Count - 1
                Dim lblRMARQDID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQDID")
                Dim lblRMARQD_RMADID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_RMADID")
                Dim lblRMARQD_NPARTNO As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_NPARTNO")
                Dim lblIMPROPERUSAGE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblIMPROPERUSAGE")

                Dim lblRMARQD_QTY As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_QTY")
                Dim UI_txtMaterialCost As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_txtMaterialCost")
                Dim lblRMARQD_PRICE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_PRICE")

                Dim lblRMARQD_CURRENCYCODE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CURRENCYCODE")
                Dim lblRMARQD_CURRENCYRATE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CURRENCYRATE")

                Dim lblRMARQD_ASSIGECURRENCYCODE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ASSIGECURRENCYCODE")
                Dim lblRMARQD_ASSIGECURRENCYRATE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ASSIGECURRENCYRATE")
                Dim lblRMARQD_AD As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_AD")
                Dim lblRMARQD_ADNAME As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_ADNAME")
                Dim lblRMARQD_CSTMP As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQD_CSTMP")


                Dim UI_RMARQD_WAIVE As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_WAIVE")
                Dim UI_RMARQD_ACC As Label = Me.UI_dvRepairDetail.Items(i).FindControl("UI_RMARQD_ACC")
                Dim UI_chkOptionClent As CheckBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_chkOptionClent")

                Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuoted_Detail.NewRMAREPAIR_QUOTED_DETAILRow
                dr.RMARQD_ID = lblRMARQDID.Text.Trim()
                dr.RMARQD_RMADID = lblRMARQD_RMADID.Text.Trim()
                dr.RMARQD_NPARTNO = lblRMARQD_NPARTNO.Text.Trim()

                dr.RMARQD_IMPROPERUSAGE = Convert.ToInt16(lblIMPROPERUSAGE.Text.Trim())

                ' '' '' ''RMARQD_WAIVE: 表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                ' '' '' ''不修重新計算 RMARQD_PRICE
                '' '' ''Dim RMARQD_PRICE As Double = 0
                '' '' ''If Convert.ToInt16(UI_RMARQD_WAIVE.Text.Trim()) = 1 Then
                '' '' ''    RMARQD_PRICE = 0
                '' '' ''Else
                '' '' ''    Dim RMARQD_OPTIONCLIENT As Integer = 1
                '' '' ''    If UI_chkOptionClent.Checked = True Then
                '' '' ''        RMARQD_PRICE = Convert.ToDecimal(lblRMARQD_QTY.Text.Trim()) * Convert.ToDecimal(UI_txtMaterialCost.Text.Trim())
                '' '' ''        RMARQD_OPTIONCLIENT = 2
                '' '' ''    End If
                '' '' ''    'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
                '' '' ''    dr.RMARQD_OPTIONCLIENT = RMARQD_OPTIONCLIENT
                '' '' ''End If


                'RMARQD_WAIVE: 表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                dr.RMARQD_WAIVE = 0
                If UI_RMARQD_WAIVE.Text.Trim() = "1" Then
                    dr.RMARQD_WAIVE = 1
                End If

                dr.RMARQD_ACC = 0
                If UI_RMARQD_ACC.Text.Trim() = "1" Then
                    dr.RMARQD_ACC = 1
                End If

                dr.RMARQD_QTY = Convert.ToDecimal(lblRMARQD_QTY.Text.Trim())
                dr.RMARQD_MATERIALCOST = Convert.ToDecimal(UI_txtMaterialCost.Text.Trim())




                'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
                Dim RMARQD_PRICE As Double = 0
                Dim RMARQD_OPTIONCLIENT As Integer = 2
                If UI_chkOptionClent.Checked = False Then
                    RMARQD_PRICE = 0
                    RMARQD_OPTIONCLIENT = 1
                End If
                If UI_chkOptionClent.Checked = True Then
                    RMARQD_PRICE = PartsRule_Exception(dr.RMARQD_QTY, dr.RMARQD_MATERIALCOST, dr.RMARQD_WAIVE, dr.RMARQD_ACC)

                    RMARQD_OPTIONCLIENT = 2
                End If

                dr.RMARQD_PRICE = RMARQD_PRICE
                dr.RMARQD_OPTIONCLIENT = RMARQD_OPTIONCLIENT


                dr.RMARQD_CURRENCYCODE = lblRMARQD_CURRENCYCODE.Text.Trim()
                dr.RMARQD_CURRENCYRATE = Convert.ToDecimal(lblRMARQD_CURRENCYRATE.Text.Trim())

                dr.RMARQD_ASSIGECURRENCYCODE = lblRMARQD_ASSIGECURRENCYCODE.Text.Trim()
                dr.RMARQD_ASSIGECURRENCYRATE = Convert.ToDecimal(lblRMARQD_ASSIGECURRENCYRATE.Text.Trim())
                dr.RMARQD_ASSIGEPRICE = oCommon.ConvertCurrency(dr.RMARQD_CURRENCYRATE, dr.RMARQD_PRICE, dr.RMARQD_ASSIGECURRENCYRATE)

                dr.RMARQD_AD = lblRMARQD_AD.Text.Trim()
                dr.RMARQD_ADNAME = lblRMARQD_ADNAME.Text.Trim()
                dr.RMARQD_CSTMP = Convert.ToDateTime(lblRMARQD_CSTMP.Text.Trim())

                dr.RMARQD_LUAD = Session("_UserID")
                dr.RMARQD_LUADNAME = Session("_UserName")
                dr.RMARQD_LUSTMP = Date.Now
                dr.RMARQD_MARK = 0

                TotalPrice_Parts = TotalPrice_Parts + dr.RMARQD_PRICE
                dtRepairQuoted_Detail.Rows.Add(dr)
            Next


            'update  RMAREPAIR_QUOTED
            Dim RMARQ_LABORHOUR As Double = Convert.ToDecimal(Me.UI_RMARQ_LABORHOUR.Text)       '工時
            Dim RMARQ_LABORPRICE As Double = Convert.ToDecimal(Me.UI_RMARQ_LABORPRICE.Text)     '工時單價
            Dim RMARQ_MATERIALCOST As Double = TotalPrice_Parts  '零件費用
            Dim RMARQ_QUOTE As Double = (RMARQ_LABORHOUR * RMARQ_LABORPRICE) + RMARQ_MATERIALCOST '費用加總(報價)

            Dim RMARQ_CURRENCYRATE As Double = Convert.ToDecimal(Me.UI_RMARQ_CURRENCYRATE.Text)
            Dim RMARQ_ASSIGECURRENCYRATE As Double = Convert.ToDecimal(Me.UI_RMARQ_ASSIGECURRENCYRATE.Text)

            Dim RMARQ_ASSIGLABORCOST As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, (RMARQ_LABORHOUR * RMARQ_LABORPRICE), RMARQ_ASSIGECURRENCYRATE)  '轉換成 指派的維修中心 --> 工時費用
            Dim RMARQ_ASSIGMATERIALCOST As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, RMARQ_MATERIALCOST, RMARQ_ASSIGECURRENCYRATE)                 '轉換成 指派的維修中心 --> 零件費用
            Dim RMARQ_ASSIGEQUOTE As Double = oCommon.ConvertCurrency(RMARQ_CURRENCYRATE, RMARQ_QUOTE, RMARQ_ASSIGECURRENCYRATE)                              '轉換成 指派的維修中心 --> 費用加總(報價)

            'update  RMAREPAIR_QUOTED_DETAIL
            ctlRMA.Edit_RepairQuotedDetail(Me.UI_RMARQ_ID.Text.Trim(), RMARQ_MATERIALCOST, RMARQ_QUOTE, RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, dtRepairQuoted_Detail)


            'update RMASALE_QUOTED
            Dim ctlSale As New ctlRMA.Sale
            Dim RMASQ_LABORCOST As Decimal = RMARQ_LABORHOUR * RMARQ_LABORPRICE
            ctlSale.Edit_SalesQuoted(Me.UI_lblPreviousPage_RMADID.Text.Trim(), RMASQ_LABORCOST, RMARQ_MATERIALCOST, RMARQ_QUOTE, Session("_UserID").ToString(), Session("_UserName").ToString(), Date.Now)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                Dim Obj As String = Request.Params("DATA").ToString().Trim()
                Obj = HttpUtility.UrlDecode(Obj)

                Dim Client_FlowCase01_Worklist_Item_New_Json_D As New Client_FlowCase01_Worklist_Item_New_Json()
                Client_FlowCase01_Worklist_Item_New_Json_D = JsonConvert.DeserializeObject(Of Client_FlowCase01_Worklist_Item_New_Json)(Obj)
                Me.UI_lblPreviousPage_RMANO.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMANO.ToString().Trim()
                Me.UI_lblPreviousPage_RMAID.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMA_ID.ToString().Trim()
                Me.UI_lblPreviousPage_RMADID.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMADID.ToString().Trim()
                Me.UI_lblRMAD_STATUS.Text = Client_FlowCase01_Worklist_Item_New_Json_D.RMAD_STATUS.ToString().Trim()

                Call setControls()
                Call QueryData()

                Call QueryByRepairQuoted()
                Call QueryByRepairQuotedDetail()

            End If

        End Try

    End Sub

    Public Function GetPrice(ByVal Price As String, ByVal RMARQD_OPTIONCLIENT As String, ByVal RMARQD_OPTION As String, ByVal index As String) As String

        Dim myRMAREPAIR_QUOTED_DETAILDataTable As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
        myRMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail_Client")
        Dim index_ As Integer = Convert.ToInt32(index)

        Dim RMARQD_QTY As String = myRMAREPAIR_QUOTED_DETAILDataTable.Rows(index_)("RMARQD_QTY").ToString().Trim()
        Dim RMARQD_MATERIALCOST As String = myRMAREPAIR_QUOTED_DETAILDataTable.Rows(index_)("RMARQD_MATERIALCOST").ToString().Trim()
        Dim RMARQD_WAIVE As String = myRMAREPAIR_QUOTED_DETAILDataTable.Rows(index_)("RMARQD_WAIVE").ToString().Trim()
        Dim RMARQD_ACC As String = myRMAREPAIR_QUOTED_DETAILDataTable.Rows(index_)("RMARQD_ACC").ToString().Trim()
        Dim RMARQD_PRICE = PartsRule_Exception(RMARQD_QTY, RMARQD_MATERIALCOST, RMARQD_WAIVE, RMARQD_ACC)

        If RMARQD_OPTIONCLIENT = "2" Then
            Price = "<p>" & Price & "</p>"
        Else

            If RMARQD_OPTION = "1" Then
                Price = " <p style='color:red;text-decoration:line-through;display:inline;'>" & RMARQD_PRICE & "</p>" & "  &nbsp; &nbsp; <p style='display:inline;' >0</p>"
            Else
                Price = "<p>" & Price & "</p>"
            End If

        End If

        Return Price

    End Function

End Class

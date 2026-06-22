Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ascx_ucRepairDetail
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_RMADID") = ""
            Me.ViewState("_RMANO") = ""

            Call chkFlowCase01()
            Call setControls()
            Call setFlowCase01()
        End If
    End Sub




    Private Sub chkFlowCase01()
        Dim i As Integer = 0

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "01"
                Exit For
            End If
        Next

    End Sub





    ''' <summary>
    ''' 設定flow case 01 的畫面控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01()

        If Me.UI_flowCase.Text = "01" Then
            Me.uiTR_Charge.Visible = False
            Me.uiTR_beforeCost.Visible = False
            Me.uiTR_afterCost.Visible = False
        End If

    End Sub




    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblRepairDetail.Text = _oLanguage.getText("RMA", "111", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

        Me.UI_lblProductDesc.Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
        Me.UI_lblProblemDesc.Text = _oLanguage.getText("RMA", "122", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)

        Me.UI_lblCustomerFile.Text = _oLanguage.getText("RMA", "123", ctlLanguage.eumType.Tag)
        Me.UI_lblCharge.Text = _oLanguage.getText("RMA", "124", ctlLanguage.eumType.Tag)

        Me.UI_lblTittel.Text = _oLanguage.getText("RMA", "128", ctlLanguage.eumType.Tag)
        Me.UI_lblImproperUsage.Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)

        Me.UI_lblbefore.Text = _oLanguage.getText("RMA", "192", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborCost_Before.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
        Me.UI_lblMaterialCost_Before.Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalAmount_Before.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)

        Me.UI_lblAfter.Text = _oLanguage.getText("RMA", "193", ctlLanguage.eumType.Tag)
        Me.UI_lblLaborCost_After.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)
        Me.UI_lblMaterialCost_After.Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalAmount_After.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)

        Me.UI_lblRepairedTitle.Text = _oLanguage.getText("RMA", "202", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)

    End Sub



    Private Sub CleanData()
        Me.UI_lblSerialText.Text = ""
        Me.UI_lblModelText.Text = ""
        Me.UI_lblFailureText.Text = ""

        Me.UI_lblProductDescText.Text = ""
        Me.UI_lblProblemDescText.Text = ""
        Me.UI_lblDescriptionText.Text = ""

        Me.UI_lblLaborCostText_Before.Text = ""
        Me.UI_lblMaterialCostText_Before.Text = ""
        Me.UI_lblTotalAmountText_Before.Text = ""

        Me.UI_lblLaborCostText_After.Text = ""
        Me.UI_lblMaterialCostText_After.Text = ""
        Me.UI_lblTotalAmountText_After.Text = ""

        Me.UI_DownloadFile.Text = ""
        Me.UI_DownloadFile.NavigateUrl = ""

        'Me.UI_dvRepairUpload.Items.c
    End Sub



    Private Sub QueryDataRepair()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepair As New RmaDTO.vwRepair_DetailDataTable
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""

        dtRepair = oRepair.QueryByRMARepairDetail(Session("_LanguageID").ToString().Trim(), Me.ViewState("_RMADID").ToString())

        If dtRepair.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_DetailRow = dtRepair.Rows(0)

            Dim sArrRepaurFile As String = ""
            Dim sCurrencyCode As String = ""


            If dr.IsRMAD_SERIALNONull = False Then Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.ToString().Trim()

            sModelNo = oExport.getMModelNo(dr.RMAD_MODELNO.ToString().Trim(), dr.RMA_COMPNO, dr.RMA_CUNO)

            'If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelText.Text = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelText.Text = sModelNo

            If dr.IsFARC_NAME2Null = False Then
                Me.UI_lblFailureText.Text = dr.FARC_NAME2.ToString().Trim()
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

            Dim RMA_COMPNO As String = ""
            If dr.IsRMAR_COMPNONull = False Then
                RMA_COMPNO = dr.RMAR_COMPNO
            Else
                RMA_COMPNO = dr.RMA_COMPNO
            End If



            If Session("_RepairCenter").ToString().IndexOf(RMA_COMPNO) <> -1 Then
                '登入系統維修中心 跟 被指派維修中心 一樣時
                '====================================================================================================================================================================================
                '維修報價
                '====================================================================================================================================================================================
                If dr.IsRMARQ_CURRENCYCODENull = False Then sCurrencyCode = dr.RMARQ_CURRENCYCODE.ToString().Trim() & " "
                If dr.IsRMARQ_LABORHOURNull = False And dr.IsRMARQ_LABORPRICENull = False And dr.IsRMARQ_CURRENCYCODENull = False Then
                    Dim iLABORCost As Double = 0
                    iLABORCost = Math.Round(dr.RMARQ_LABORHOUR * dr.RMARQ_LABORPRICE, 2)        '工時*單價
                    Me.UI_lblLaborCostText_Before.Text = sCurrencyCode & iLABORCost.ToString("N").Trim()
                End If
                If dr.IsRMARQ_MATERIALCOSTNull = False And dr.IsRMARQ_CURRENCYCODENull = False Then Me.UI_lblMaterialCostText_Before.Text = sCurrencyCode & Convert.ToDouble(dr.RMARQ_MATERIALCOST).ToString("N").Trim()
                If dr.IsRMARQ_QUOTENull = False Or dr.IsRMARQ_CURRENCYCODENull = False Then Me.UI_lblTotalAmountText_Before.Text = sCurrencyCode & Convert.ToDouble(dr.RMARQ_QUOTE).ToString("N").Trim()


                '====================================================================================================================================================================================
                '維修費用
                '====================================================================================================================================================================================
                If dr.IsRMAR_CURRENCYCODENull = False Then sCurrencyCode = dr.RMAR_CURRENCYCODE.ToString().Trim() & " "
                If dr.IsRMAR_LABORCOSTNull = False And dr.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblLaborCostText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_LABORCOST).ToString("N").Trim()
                If dr.IsRMAR_MATERIALCOSTNull = False And dr.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblMaterialCostText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_MATERIALCOST).ToString("N").Trim()
                If dr.IsRMAR_QUOTENull = False Or dr.IsRMAR_CURRENCYCODENull = False Then Me.UI_lblTotalAmountText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_QUOTE).ToString("N").Trim()

            Else
                '====================================================================================================================================================================================
                '維修報價
                '====================================================================================================================================================================================
                If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then sCurrencyCode = dr.RMARQ_ASSIGECURRENCYCODE.ToString().Trim() & " "
                If dr.IsRMARQ_ASSIGLABORCOSTNull = False And dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then Me.UI_lblLaborCostText_Before.Text = sCurrencyCode & Convert.ToDouble(dr.RMARQ_ASSIGLABORCOST).ToString("N").Trim()
                If dr.IsRMARQ_ASSIGMATERIALCOSTNull = False And dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then Me.UI_lblMaterialCostText_Before.Text = sCurrencyCode & Convert.ToDouble(dr.RMARQ_ASSIGMATERIALCOST).ToString("N").Trim()
                If dr.IsRMARQ_ASSIGEQUOTENull = False Or dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then Me.UI_lblTotalAmountText_Before.Text = sCurrencyCode & Convert.ToDouble(dr.RMARQ_ASSIGEQUOTE).ToString("N").Trim()


                '====================================================================================================================================================================================
                '維修費用
                '====================================================================================================================================================================================
                If dr.IsRMAR_ASSIGECURRENCYCODENull = False Then sCurrencyCode = dr.RMAR_ASSIGECURRENCYCODE.ToString().Trim() & " "
                If dr.IsRMAR_ASSIGELABORCOSTNull = False And dr.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblLaborCostText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_ASSIGELABORCOST).ToString("N").Trim()
                If dr.IsRMAR_ASSIGEMATERIALCOSTNull = False And dr.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblMaterialCostText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_ASSIGEMATERIALCOST).ToString("N").Trim()
                If dr.IsRMAR_ASSIGEQUOTENull = False Or dr.IsRMAR_ASSIGECURRENCYCODENull = False Then Me.UI_lblTotalAmountText_After.Text = sCurrencyCode & Convert.ToDouble(dr.RMAR_ASSIGEQUOTE).ToString("N").Trim()
            End If

            If dr.IsRMAD_UPLOADFILENull = False Then sArrRepaurFile = dr.RMAD_UPLOADFILE.ToString().Trim()
            If sArrRepaurFile.Trim() <> "" Then
                'Dim sRepaurFile As String() = sArrRepaurFile.ToString().Trim().Split(",")
                'Me.UI_DownloadFile.Text = sRepaurFile(0).ToString().Trim()
                'Me.UI_DownloadFile.NavigateUrl = _WEBURL & _Requested_VisualPath & sRepaurFile(1).ToString().Trim()
			    Dim UI_Downloadlbl_String As String = ""
				
				Dim sRepaurFile As String() = sArrRepaurFile.ToString().Trim().Split(",")
                For i = 0 To sRepaurFile.Count - 1

                          Dim sRepaurFileString As String() = sRepaurFile(i).ToString().Trim().Split(".")

                    If sRepaurFileString(0).Length = 17 Then
					 	UI_Downloadlbl_String +="<a href='"& _WEBURL & _Requested_VisualPath & sRepaurFile(i).ToString().Trim()  &"'   target='_blank' >"  & sRepaurFile(i).ToString().Trim() & "</a>&nbsp;&nbsp;"
					
					
                        Me.UI_DownloadFile.Text = sRepaurFile(i).ToString().Trim()
                        Me.UI_DownloadFile.NavigateUrl = _WEBURL & _Requested_VisualPath & sRepaurFile(i).ToString().Trim()
                    End If

                Next
				
					  Me.UI_Downloadlbl.Text = UI_Downloadlbl_String
				
				
            End If


        End If
    End Sub


#Region "QueryDataRepairUpload:維修報告"

    Private Sub QueryDataRepairUpload()
        Dim oRepairUpload As New ctlRMA.Repair
        Dim dtRepairUpload As New RmaDTO.tmpRepairUploadDataTable

        dtRepairUpload = oRepairUpload.QueryByUpload_Group(Me.ViewState("_RMANO").ToString())

        Me.UI_panReport.Visible = False
        If dtRepairUpload.Rows.Count > 0 Then
            Me.UI_panReport.Visible = True
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
                'UI_lblRepair1.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID1.Visible = True
                UI_lblRepair1.Visible = True
                UI_DownloadRepair1.Visible = True
            End If

            If sArrRepaurFile2.Trim() <> "" Then
                UI_SeqID2.Text = "(" & UI_lblSeqID2.Text.Trim() & ")."
                Dim sRepaurFile2 As String() = sArrRepaurFile2.ToString().Trim().Split(",")
                UI_DownloadRepair2.Text = sRepaurFile2(0).ToString().Trim()
                UI_DownloadRepair2.NavigateUrl = _WEBURL & _Repair_VisualPath & sRepaurFile2(1).ToString().Trim()
                'UI_lblRepair2.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID2.Visible = True
                UI_lblRepair2.Visible = True
                UI_DownloadRepair2.Visible = True
            End If

            If sArrRepaurFile3.Trim() <> "" Then
                UI_SeqID3.Text = "(" & UI_lblSeqID3.Text.Trim() & ")."
                Dim sRepaurFile3 As String() = sArrRepaurFile3.ToString().Trim().Split(",")
                UI_DownloadRepair3.Text = sArrRepaurFile3(0).ToString().Trim()
                UI_DownloadRepair3.NavigateUrl = _WEBURL & _Repair_VisualPath & sArrRepaurFile3(1).ToString().Trim()
                'UI_lblRepair3.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

                UI_SeqID3.Visible = True
                UI_lblRepair3.Visible = True
                UI_DownloadRepair3.Visible = True
            End If

        End If
    End Sub


#End Region



#Region "QueryDataByDetail:維修品項"

    Private Sub QueryDataByDetail()
        Dim oRepair As New ctlRMA.Repair
        Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable

        dtRepairDetail = oRepair.QueryByDetail(Me.ViewState("_RMADID").ToString())

        Me.UI_panRepaired.Visible = False
        If dtRepairDetail.Rows.Count > 0 Then
            Me.UI_panRepaired.Visible = True
        End If

        Call RepairDetail_DataBind(dtRepairDetail, 0)

    End Sub

    Private Sub RepairDetail_DataBind(ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtRepairDetail)

        Session("_dtRepairDetail") = dtRepairDetail

        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARED_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable)
        Dim i As Integer = 0

        If dtRepairDetail.Columns("SeqID") Is Nothing Then
            dtRepairDetail.Columns.Add("SeqID")
        End If

        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARED_MARK=0"

        For i = 0 To dvRepairDetail.Count - 1
            Dim dr As RmaDTO.RMARepair_DetailRow = dvRepairDetail(i).Row()
            dvRepairDetail(i)("SeqID") = i + 1
        Next

    End Sub

    Protected Sub UI_dvRepairDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRepairDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "103", ctlLanguage.eumType.Tag)
        End If

    End Sub

#End Region


    Private Sub RMA_DataBindPur(ByVal dvPurchasing As DataView, ByVal iPageIndex As Integer)
        dvPurchasing.Sort = Me.ViewState("_PurSortExpression").ToString() & " " & Me.ViewState("_PurSortDirection").ToString()
        Call CreatePurKeyPoint(Me.ViewState("_PurSortExpression").ToString(), Me.ViewState("_PurSortDirection").ToString())

        Me.dgvPurchasing.PageSize = 15
        Me.dgvPurchasing.PageIndex = iPageIndex
        Me.dgvPurchasing.DataSource = dvPurchasing
        Me.dgvPurchasing.DataBind()
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

                Call CleanData()
                Call QueryDataRepair()
                Call QueryDataRepairUpload()
                Call QueryDataByDetail()

                Me.ViewState("_PurSortExpression") = "SerialNo"
                Me.ViewState("_PurSortDirection") = "asc"
                Session("_dtPurchasing") = Nothing
                SetDataTable()
                Dim dt As DataTable = New DataTable()
                dt.Columns.Add("SN")
                Dim dr As DataRow = dt.NewRow()
                dr(0) = UI_lblSerialText.Text
                dt.Rows.Add(dr)

                ArrangementData(dt)
                QueryDataPur(0)
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub









End Class

Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports DataService
Imports DefLanguage



Partial Class ascx_ucClientDetail
    Inherits System.Web.UI.UserControl

    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")


#Region "電池保固"
    Private Sub Check_Standard_Battery(ByVal RMAD_ID As String, ByVal WATS_SN As String)

        Dim myctlWarranty As New ctlWarranty
        Dim dt As New DataTable
        dt = myctlWarranty.Check_Standard_Battery(RMAD_ID)

        Dim dt_ As New DataTable
        dt_ = myctlWarranty.WARRANTYSERIAL_BI_Repaired_QTY(WATS_SN)

        Dim index As Integer = 0

        For i = 0 To dt.Rows.Count - 1
            index = index + Convert.ToInt32(dt.Rows(i)(0).ToString().Trim())
        Next

        Dim index_ As Integer = 0

        For i = 0 To dt_.Rows.Count - 1
            If Not dt_.Rows(i)("RMAD_RMANO_18") Is DBNull.Value Then
                If dt_.Rows(i)("RMAD_RMANO_18").ToString().Trim() = "OK" Then
                    index_ = index_ + 1
                End If
            End If
        Next

        RMAD_RMANO.Visible = False
        txtRMAD_RMANO_QTY.Visible = False
        LabRMAD_RMANO_QTY.Visible = False
        labAmount.Visible = False
        If index = 0 Then

        Else

            RMAD_RMANO.Visible = True
            txtRMAD_RMANO_QTY.Visible = True
            LabRMAD_RMANO_QTY.Visible = True
            labAmount.Visible = True
            If Session("_LanguageID").ToString() = "002" Then
                txtRMAD_RMANO_QTY.Text = ": &nbsp; &nbsp; 目前使用" + index.ToString().Trim() + "數量"
                RMAD_RMANO.Text = "電池保險申請:"
                labAmount.Text = "YES"

            ElseIf Session("_LanguageID").ToString() = "003" Then
                txtRMAD_RMANO_QTY.Text = ": &nbsp; &nbsp; 使用" + index.ToString().Trim() + "数量"
                RMAD_RMANO.Text = "バッテリー保険適用:"
                labAmount.Text = "はい"

            Else
                txtRMAD_RMANO_QTY.Text = ": &nbsp; &nbsp; usage" + index.ToString().Trim() + " quantity"
                RMAD_RMANO.Text = "Apply Battery </br> Insurance:"
                labAmount.Text = "YES"
            End If



            If Session("_LanguageID").ToString() = "002" Then
                LabRMAD_RMANO_QTY.Text = "&nbsp; &nbsp; 目前剩餘:" + index_.ToString().Trim() + "數量"
            Else
                LabRMAD_RMANO_QTY.Text = "&nbsp; &nbsp; Remaining:" + index_.ToString().Trim() + " quantity"
            End If

        End If

    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_RMADID") = ""
            Me.ViewState("_RMANO") = ""

            Call setControls()
        End If
    End Sub


    '提供序號
    Public Function Apply_Total_Loss_Insurance(ByVal SERIALNO As String) As DataTable

        Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable

        Try

            Dim ctlWarranty_ As New ctlWarranty
            '提供序號
            EXPORT_EXPORT_ORDERNUMBER = ctlWarranty_.EXPORT_EXPORT_ORDERNUMBER(SERIALNO)

        Catch ex As Exception
            Throw ex
        Finally

        End Try

        Return EXPORT_EXPORT_ORDERNUMBER
    End Function

    Private Sub Check_AXMT410_AXMT400(ByVal UI_lblRMANoText_String As String, ByVal UI_lblShowSerial_String As String)

        '判斷保險種類
        If 1 = 1 Then

            '確認是否需要秀出 Apply Total Loss Insurance 
            '提供序號
            Insurance_Label.Visible = False
            UI_Apply_Total_Loss_Insurance.Visible = False
            Apply_Label.Visible = False

            Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
            EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial_String)

            If Not EXPORT_EXPORT_ORDERNUMBER Is Nothing Then

                If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                    '有的話帶入db資料
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True

                    Dim ctlWarranty_ As New ctlWarranty


                    Dim dt As New DataTable
                    dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)



                    If Not dt Is Nothing Then

                        If dt.Rows.Count > 0 Then
                            UI_Apply_Total_Loss_Insurance.Text = "Yes"
                        Else

                        End If

                    Else

                    End If


                End If


            Else


            End If

        End If

        Dim Order_No As String = ""
        Dim Total_Loss_Insurance As String = ""
        '判斷保險可用數量
        If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

            Dim ID As String = ""
            Dim Project_No As String = ""
            Dim Project_Qty As String = ""
            Dim Order_Qty As String = ""
            Dim RMAD_RMANO As String = ""
            Dim RMAD_SEQ As String = ""
            Dim RMAD_SERIALNO As String = ""

            If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

                Dim ctlWarranty_ As New ctlWarranty
                '提供序號
                Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial_String)


                '新增RMA加購保固判斷
                Dim Select_WARRANTYSERIAL_DataTable As Boolean = ctlWarranty_.Select_WARRANTYSERIAL(UI_lblShowSerial_String)

                If Select_WARRANTYSERIAL_DataTable Then

                    If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                        Dim Select_WARRANTYITEM_DataTable As New DataTable
                        Select_WARRANTYITEM_DataTable = ctlWarranty_.Select_WARRANTYITEM(UI_lblShowSerial_String)
                        'RMA
                        Dim pList As New List(Of Product)
                        Dim Product_D As New Product

                        '專案編號
                        Product_D.Project_No = ""
                        '專案數量
                        Product_D.Project_Qty = ""
                        '訂單編號
                        Product_D.Order_No = Select_WARRANTYITEM_DataTable.Rows(0)("WATI_WATYNO").ToString.Trim() & "-" & Select_WARRANTYITEM_DataTable.Rows(0)("WATI_SEQ").ToString.Trim()
                        '專案數量
                        Product_D.Order_Qty = Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()
                        '全損保險
                        Product_D.Total_Loss_Insurance = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString() & "%"
                        '可更換數量
                        Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        '已更換數量
                        Product_D.Quantity_replaced = ""

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = UI_lblRMANoText_String
                        RMAD_SEQ = Product_D.Replaceable_quantity
                        RMAD_SERIALNO = UI_lblShowSerial_String


                    End If

                Else

                    Dim strarr() As String
                    strarr = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("EXPORT_ORDERNUMBER").ToString().Trim().Split("-")

                    '表格
                    Dim EXPORT_axmt410_axmt400 As New DataTable
                    EXPORT_axmt410_axmt400 = ctlWarranty_.EXPORT_axmt410_axmt400(strarr(0) & "-" & strarr(1), strarr(2))


                    If Not EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop") Is DBNull.Value Then

                        Dim pList As New List(Of Product)
                        Dim Product_D As New Product

                        '專案編號
                        Product_D.Project_No = EXPORT_axmt410_axmt400.Rows(0)("Project_No").ToString() & "-" & strarr(2)
                        '專案數量
                        Product_D.Project_Qty = EXPORT_axmt410_axmt400.Rows(0)("Project_Qty").ToString()
                        '訂單編號
                        Product_D.Order_No = EXPORT_axmt410_axmt400.Rows(0)("Order_No").ToString() & "-" & strarr(2)
                        '專案數量
                        Product_D.Order_Qty = EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()
                        '全損保險
                        Product_D.Total_Loss_Insurance = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString() & "%"
                        '可更換數量
                        Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        '已更換數量
                        Product_D.Quantity_replaced = ""
                        pList.Add(Product_D)

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = UI_lblRMANoText_String
                        RMAD_SEQ = ""
                        RMAD_SERIALNO = UI_lblShowSerial_String



                    End If
                End If

            End If

        End If

        If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

            Dim ctlWarranty_ As New ctlWarranty
            Dim Check_axmt410_axmt400_DataTable As New DataTable
            Check_axmt410_axmt400_DataTable = ctlWarranty_.Check_axmt410_axmt400(Order_No)

            If Math.Floor(Convert.ToDecimal(Total_Loss_Insurance)) >= Convert.ToInt32(Check_axmt410_axmt400_DataTable.Rows(0)("ORDER_NO").ToString()) + 1 Then

            Else

                Insurance_Label.Visible = False
                UI_Apply_Total_Loss_Insurance.Visible = False
                Apply_Label.Visible = False


                '單獨資料顯示區塊
                Dim dt As New DataTable
                dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)

                For i = 0 To dt.Rows.Count - 1
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True
                Next
            End If
        End If

        '確認這單是否使用過全損保險
        If 1 = 1 Then

            Dim ctlWarranty_ As New ctlWarranty
            Dim dt As New DataTable
            dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(UI_lblRMANoText_String, UI_lblShowSerial_String)

            If Not dt Is Nothing Then

                If dt.Rows.Count > 0 Then
                    Insurance_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Visible = True
                    Apply_Label.Visible = True
                    UI_Apply_Total_Loss_Insurance.Text = "Yes"
                Else

                End If

            Else

            End If

        End If

    End Sub

    Public Class Product
        '專案編號
        Public Property Project_No As String
        '專案數量
        Public Property Project_Qty As String
        '訂單編號
        Public Property Order_No As String
        '專案數量
        Public Property Order_Qty As String
        '全損保險
        Public Property Total_Loss_Insurance As String
        '可更換數量
        Public Property Replaceable_quantity As String
        '已更換數量
        Public Property Quantity_replaced As String

    End Class

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

        Me.UI_lblLaborCost.Text = _oLanguage.getText("RMA", "125", ctlLanguage.eumType.Tag)     'Labor Cost
        Me.UI_lblLaborCost.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)     'Service Charge


        Me.UI_lblMaterialCost.Text = _oLanguage.getText("RMA", "126", ctlLanguage.eumType.Tag)
        Me.UI_lblTotalAmount.Text = _oLanguage.getText("RMA", "127", ctlLanguage.eumType.Tag)
        Me.UI_lblTittel.Text = _oLanguage.getText("RMA", "128", ctlLanguage.eumType.Tag)

        Me.UI_lblRepairedTitle.Text = _oLanguage.getText("RMA", "202", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "039", ctlLanguage.eumType.Command)
        Me.PurchasingRecordsLab.Text = _oLanguage.getText("RMA2", "046", ctlLanguage.eumType.Tag)
        Me.UI_cmdClose.Text = _oLanguage.getText("RMA2", "047", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairDetail.Text = _oLanguage.getText("RMA2", "048", ctlLanguage.eumType.Tag)

        Me.UI_lblApply_BI.Text = _oLanguage.getText("RMA", "222", ctlLanguage.eumType.Tag)
    End Sub



    Private Sub CleanData()
        Me.UI_lblSerialText.Text = ""
        Me.UI_lblModelText.Text = ""
        Me.UI_lblFailureText.Text = ""

        Me.UI_lblProductDescText.Text = ""
        Me.UI_lblProblemDescText.Text = ""
        Me.UI_lblDescriptionText.Text = ""

        Me.UI_lblLaborCostText.Text = ""
        Me.UI_lblMaterialCostText.Text = ""
        Me.UI_lblTotalAmountText.Text = ""
        Me.UI_DownloadFile.Text = ""
        Me.UI_DownloadFile.NavigateUrl = ""

    End Sub



    Private Sub QueryDataRepair()
        Dim oRMA As New ctlRMA.Client
        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""

        dtClientDetail = oRMA.QueryByClientDetail(Session("_LanguageID").ToString().Trim(), Me.ViewState("_RMADID").ToString())

        If dtClientDetail.Rows.Count > 0 Then
            Dim dr As RmaDTO.tmpClientDetailRow = dtClientDetail.Rows(0)
            Dim sArrRepaurFile As String = ""
            Dim sCurrencyCode As String = ""

            If dr.IsRMAD_SERIALNONull = False Then Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.ToString().Trim()
            'If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelText.Text = dr.RMAD_MODELNO.ToString().Trim()

            sModelNo = oExport.getMModelNo(dr.RMAD_MODELNO.ToString().Trim(), dr.RMARQ_COMPNO.ToString().Trim(), dr.RMA_ACCOUNTID.ToString().Trim())

            If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelText.Text = sModelNo

            If dr.IsFARC_NAME2Null = False Then
                Me.UI_lblFailureText.Text = dr.FARC_NAME2.ToString().Trim()
            Else
                If dr.IsFARC_NAME1Null = False Then Me.UI_lblFailureText.Text = dr.FARC_NAME1.ToString().Trim()
            End If

            If dr.IsRMAD_PRODUCTDESCNull = False Then Me.UI_lblProductDescText.Text = dr.RMAD_PRODUCTDESC.Trim()

            If dr.IsRMAR_PROBLEMDESCNull = False Then
                Me.UI_lblProblemDescText.Text = dr.RMAR_PROBLEMDESC.ToString().Trim()
            Else
                If dr.IsRMAD_PROBLEMDESCNull = False Then Me.UI_lblProblemDescText.Text = dr.RMAD_PROBLEMDESC.ToString().Trim()
            End If

            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_lblDescriptionText.Text = dr.RMAR_REPAIRDESC.ToString().Trim()

            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_lblDescriptionText.Text = dr.RMAR_REPAIRDESC.ToString().Trim()
            '需求新增:BI保固 By buck Add 20250902 begin
            Me.UI_lblApply_BIText.Text = If(dr.IsRMAD_APPLY_BINull = True OrElse dr.RMAD_APPLY_BI.Trim() = "0",
                                                _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag),
                                            _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag))
            'Me.UI_trApply_BI.Visible = If(dr.IsRMAD_APPLY_BINull = False, True, False)
            '需求新增:BI保固 By buck Add 20250902 end
            'If dr.IsRMARSD_QUOTENull = False Then
            '    '幣別
            '    If dr.IsRMASQ_CURRENCYCODENull = False Then sCurrencyCode = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "
            '    'Labor Cost
            '    If dr.IsRMARSD_LABORCOSTNull = False Then Me.UI_lblLaborCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMARSD_LABORCOST).ToString("N").Trim()
            '    'Material Cost
            '    If dr.IsRMARSD_MATERIALCOSTNull = False Then Me.UI_lblMaterialCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMARSD_MATERIALCOST).ToString("N").Trim()
            '    'Total Amount
            '    Me.UI_lblTotalAmountText.Text = sCurrencyCode & Convert.ToDouble(dr.RMARSD_QUOTE).ToString("N").Trim()

            'Else
            '    If dr.IsRMASQ_QUOTENull = False Then
            '        '幣別
            '        If dr.IsRMASQ_CURRENCYCODENull = False Then sCurrencyCode = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "
            '        'Labor Cost
            '        If dr.IsRMASQ_LABORCOSTNull = False Then Me.UI_lblLaborCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_LABORCOST).ToString("N").Trim()
            '        'Material Cost
            '        If dr.IsRMASQ_MATERIALCOSTNull = False Then Me.UI_lblMaterialCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_MATERIALCOST).ToString("N").Trim()
            '        'Total Amount
            '        Me.UI_lblTotalAmountText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
            '    End If
            'End If

            If dr.IsRMASQ_QUOTENull = False Then
                    '幣別
                    If dr.IsRMASQ_CURRENCYCODENull = False Then sCurrencyCode = dr.RMASQ_CURRENCYCODE.ToString().Trim() & " "
                    'Labor Cost
                    If dr.IsRMASQ_LABORCOSTNull = False Then Me.UI_lblLaborCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_LABORCOST).ToString("N").Trim()
                    'Material Cost
                    If dr.IsRMASQ_MATERIALCOSTNull = False Then Me.UI_lblMaterialCostText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_MATERIALCOST).ToString("N").Trim()
                    'Total Amount
                    Me.UI_lblTotalAmountText.Text = sCurrencyCode & Convert.ToDouble(dr.RMASQ_QUOTE).ToString("N").Trim()
                End If

                If dr.RMAD_STATUS = 91 Then
                    Me.UI_lblLaborCostText.Text = sCurrencyCode & "0"
                    Me.UI_lblMaterialCostText.Text = sCurrencyCode & "0"
                    Me.UI_lblTotalAmountText.Text = sCurrencyCode & "0"
                End If

                If dr.IsRMAD_UPLOADFILENull = False Then sArrRepaurFile = dr.RMAD_UPLOADFILE.ToString().Trim()

                If Not IsDBNull(dr("RMAD_UPLOADFILE")) Then
                    Dim sRepaurFile As String() = sArrRepaurFile.ToString().Trim().Split(",")


                    Dim UI_Downloadlbl_String As String = ""

                    For i = 0 To sRepaurFile.Count - 1

                        Dim sRepaurFileString As String() = sRepaurFile(i).ToString().Trim().Split(".")

                        If sRepaurFileString(0).Length = 17 Then

                            UI_Downloadlbl_String += "<a href='" & _WEBURL & _Requested_VisualPath & sRepaurFile(i).ToString().Trim() & "'  target='_blank'  style='color:#326B9B;'  >" & sRepaurFile(i).ToString().Trim() & "</a>&nbsp;&nbsp;"

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
                UI_DownloadRepair3.Text = sRepaurFile3(0).ToString().Trim()
                UI_DownloadRepair3.NavigateUrl = _WEBURL & _Repair_VisualPath & sRepaurFile3(1).ToString().Trim()
                UI_lblRepair3.Text = _oLanguage.getText("RMA", "129", ctlLanguage.eumType.Tag)

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
                Try
                    Call Check_AXMT410_AXMT400(sRMANO.Trim, UI_lblSerialText.Text.Trim())
                    Call Check_Standard_Battery(sRMADID, UI_lblSerialText.Text.Trim())
                Catch ex As Exception
                    RMAD_RMANO.Visible = False
                    txtRMAD_RMANO_QTY.Visible = False
                    LabRMAD_RMANO_QTY.Visible = False

                    Insurance_Label.Visible = False
                    UI_Apply_Total_Loss_Insurance.Visible = False
                    Apply_Label.Visible = False

                Finally

                End Try

            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub



End Class

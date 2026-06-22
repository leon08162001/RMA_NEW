Imports System.Data
Imports System.Data.OracleClient
Imports Common
'Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports RMA_Common
Imports RMA_Model


Partial Class RMAQuoting
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim comms As New Commons
    Dim _oLanguage As New ctlLanguage
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")

    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")
    Dim _Customer_ExceptionCharge As String = ConfigurationSettings.AppSettings("Customer_ExceptionCharge")


    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim _ReportToPDF As String = "ClientQuotation_" & oCommon.GetRandomizeNum() & ".pdf"


    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Private Sub Check_BI()
        '2025/01/06 新增 電池保固 開始
        Dim ctlWarranty_WARRANTYSERIAL_BI As New ctlWarranty
        Dim myDataTable As New DataTable
        myDataTable = ctlWarranty_WARRANTYSERIAL_BI.WARRANTYSERIAL_BI_Repaired_QTY(UI_lblShowSerial.Text.Trim())
        LabRMAD_RMANO_UI.Visible = False
        RMAD_RMANO.Visible = False
        txtRMAD_RMANO_QTY.Visible = False
        LabRMAD_RMANO_QTY.Visible = False
        rblBI.Visible = False
        rblBI.SelectedValue = 0
        plBI.Visible = False

        txtRMAD_RMANO_QTY.Text = "0"

        Dim index As Integer = 0
        'For i = 0 To myDataTable.Rows.Count - 1
        'If myDataTable.Rows(i)("RMAD_RMANO_18").ToString() = "OK" Then
        'index = index + 1
        'End If
        'Next
        index = myDataTable.AsEnumerable.ToList().Where(Function(dr) dr("RMAD_RMANO_18") = "OK").Count()

        If index > 0 Then
            rblBI.Visible = True
            rblBI.SelectedValue = 1
            plBI.Visible = True


            LabRMAD_RMANO_UI.Visible = True
            RMAD_RMANO.Visible = True
            txtRMAD_RMANO_QTY.Visible = True
            LabRMAD_RMANO_QTY.Visible = True

            If Session("_LanguageID").ToString() = "002" Then
                LabRMAD_RMANO_QTY.Text = "目前剩餘數量:" + index.ToString().Trim()
                RMAD_RMANO.Text = "電池保險申請"
            ElseIf Session("_LanguageID").ToString() = "003" Then
                LabRMAD_RMANO_QTY.Text = "残りの数量:" + index.ToString().Trim()
                RMAD_RMANO.Text = "バッテリー保険適用"
            Else
                LabRMAD_RMANO_QTY.Text = "Remaining quantity:" + index.ToString().Trim()
                RMAD_RMANO.Text = "Apply Battery Insurance"
            End If

        End If
        '2025/01/06 新增 電池保固 結束
    End Sub

    Private Sub UpData_BI(ByVal Bi_Count As Integer)
        '2025/01/06 新增 電池保固 開始
        Dim ctlWarranty_WARRANTYSERIAL_BI As New ctlWarranty
        Dim myDataTable As New DataTable
        myDataTable = ctlWarranty_WARRANTYSERIAL_BI.WARRANTYSERIAL_BI_Repaired_QTY(UI_lblShowSerial.Text.Trim())
        '先清空
        ctlWarranty_WARRANTYSERIAL_BI.Up_Repaired_WATS_SN_WARRANTYSERIAL_BI(RMADID())

        For i = 0 To myDataTable.Rows.Count - 1
            Dim WATS_SN As String = myDataTable.Rows(i)("WATS_SN").ToString().Trim()
            Dim item As String = myDataTable.Rows(i)("item").ToString().Trim()

            If Bi_Count > i Then
                ctlWarranty_WARRANTYSERIAL_BI.Up_Repaired_RMAD_RMANO_WARRANTYSERIAL_BI(WATS_SN, RMADID(), item)
            End If
        Next
        '2025/01/06 新增 電池保固 結束
    End Sub

    '需求新增:BI保固 By buck Add 20250902 begin
    '需求新增:BI保固BUG修正 By buck Add 20251217 begin
    Private Sub QueryBIQty()

        Dim ctlWarranty As New ctlWarranty
        Dim dt As DataTable = TryCast(Session("_dtRequest"), DataTable)
        Dim sCUNO = dt.AsEnumerable.Select(Function(x) x.Field(Of String)("CU_NO")).First()
        Dim dtWarrantyBI = ctlWarranty.WARRANTY_BI_List()
        Dim lsWarrantyBI = dtWarrantyBI.AsEnumerable.Where(Function(x) x.Field(Of String)("WATY_CUST") = sCUNO).ToList()

        UI_BI_Row.Visible = False
        UI_lblApplyBatteryInsurance.Visible = False
        UI_opgApplyBatteryInsurance.Visible = False
        uiTxt_ApplyBatteryInsurance.Visible = False

        If Not lsWarrantyBI Is Nothing And lsWarrantyBI.Count > 0 Then
            UI_BI_Row.Visible = True
            UI_lblApplyBatteryInsurance.Visible = True
            UI_opgApplyBatteryInsurance.Visible = True
            uiTxt_ApplyBatteryInsurance.Visible = True

            Dim total As Decimal = lsWarrantyBI.AsEnumerable().Sum(Function(x) x.Field(Of Decimal)("BI_BATTERYQTY"))
            uiTxt_ApplyBatteryInsurance.Text = " Remaining Q’ty:" + total.ToString()
        End If

    End Sub
    Private Sub QueryWarrantyBI()

        Dim dtRMAData As DataTable = TryCast(Session("_dtRequest"), DataTable)
        Dim sCU_NO = dtRMAData.AsEnumerable().Select(Function(x) If(x.IsNull("CU_NO"), Nothing, x.Field(Of String)("CU_NO"))).FirstOrDefault()
        Dim ctlWarranty As New ctlWarranty
        Dim dtWarrantyBI As DataTable = ctlWarranty.WARRANTY_BI_List()
        Dim dtBatExpend As DataTable = ctlWarranty.WARRANTY_BatExpend_Record(New RMADetailReq With {.RMAD_SERIALNO = UI_lblSerialText.Text.Trim()})
        Dim lsWarrantyBI As List(Of DataRow) = dtWarrantyBI.AsEnumerable.Where(Function(x) x.Field(Of String)("BI_CUNO") = sCU_NO).ToList()
        'Dim lsWarrantyBI As List(Of DataRow) = dtWarrantyBI.AsEnumerable.Where(Function(x) x.Field(Of String)("WATY_CUST") = sCU_NO).ToList()
        UI_BI_Row.Visible = False
        fdt_Warranty.Visible = False
        UI_lblWarrantyBI.Visible = False
        UI_opgApplyBatteryInsurance.SelectedValue = " Remaining Q’ty:0"

        If Not lsWarrantyBI Is Nothing And lsWarrantyBI.Count > 0 Then

            'Dim dcBATT_QTY As Decimal = lsWarrantyBI.AsEnumerable().Sum(Function(x) x.Field(Of Decimal)("BATT_QTY")) '購買總數量
            Dim dcBATT_QTY As Decimal = lsWarrantyBI.AsEnumerable().Sum(Function(x) x.Field(Of Decimal)("BI_BATTERYQTY")) '購買總數量 
            Dim dcUSEQTY As Decimal = dtBatExpend.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x("BE_USEQTY"))) '使用數量
            If (dcBATT_QTY - dcUSEQTY) = 0 Then
                UI_BI_Row.Visible = False
                fdt_Warranty.Visible = False
                UI_lblWarrantyBI.Visible = False
            Else
                UI_BI_Row.Visible = True
                fdt_Warranty.Visible = True
                UI_lblWarrantyBI.Visible = True
                uiTxt_ApplyBatteryInsurance.Text = " Remaining Q’ty:" + (dcBATT_QTY - dcUSEQTY).ToString()
            End If

        End If

        Me.UI_dvBATRECORD.PageSize = _PageSize
        Me.UI_dvBATRECORD.DataSource = dtBatExpend
        Me.UI_dvBATRECORD.DataBind()
    End Sub
    '需求新增:BI保固BUG修正 By buck Add 20251217 end
    '需求新增:BI保固 By buck Add 20250902 end

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0

        If Me.IsPostBack = False Then
            Session("_RepairRart_Submit") = False

            Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
            Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")
            Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

            Session("_dtRepairQuotedDetail") = Nothing

            Me.RMADID = UI_lblPreviousPage_RMADID.Text.Trim()
            Me.RMA_NO = UI_lblPreviousPage_RMANO.Text.Trim()
            Me.UI_Customer_ExceptionCharge.Text = _Customer_ExceptionCharge

            Call setDefault()
            Dim hsSelectID As New Hashtable
            Me.ViewState("hsSelectID") = hsSelectID

            Call ClearObjValue()
            Call QueryData_Head()
            Call chkFlowCase()

            Call QueryData_Detail(0)
            Call QueryByRepairQuotedDetail()
            Call setFlowCase01()
            Call QueryData_WarrParts()
            Call QuerySDC()
            Call QueryWarranty()
            Call Check_BI()
            '需求新增:BI保固 By buck Add 20250902 begin
            'Call QueryBIQty()
            Call QueryWarrantyBI()
            '需求新增:BI保固 By buck Add 20250902 end
            ' qryRepairQuotaionRPT()
            Call Check_AXMT410_AXMT400(UI_lblShowSerial.Text.Trim())
        End If

        '新Total Loss  2024 10月上線 舊的廢棄
        If Me.UI_lblShowSerial.Text.Trim() <> "" Then
            'QueryData(Me.UI_lblShowSerial.Text.Trim())
        End If

    End Sub

    Private Sub QuerySDC()
        Dim SDCGrid As DataGrid = New DataGrid()
        SDCGrid = Me.UcSDCViewG.FindControl("UcSDCViewG")
        Me.UcSDCViewG.show(UI_lblSerialText.Text.Trim(), Me.lstParts.Width)

    End Sub

    Private Sub QueryWarranty()
        Dim WarrantyGrid As DataGrid = New DataGrid()
        WarrantyGrid = Me.UcWarrantyView.FindControl("UcWarrantyView")
        Me.UcWarrantyView.show(UI_lblSerialText.Text.Trim(), Me.lstParts.Width)

    End Sub

    Private Sub chkFlowCase()
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
            If Me.UI_lblRepairNo.Text.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "01"
                Exit For
            End If
        Next

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 02
        Dim arrRepairNo_flowCase02() As String = _RepairNo_flowCase02.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase02.Length - 1
            If Me.UI_lblRepairNo.Text.Trim().IndexOf(arrRepairNo_flowCase02(i).ToString().Trim()) <> -1 Then
                Me.UI_flowCase.Text = "02"
                Exit For
            End If
        Next

    End Sub

#Region "Requested Information"

    Private Sub QueryData_Detail(ByVal iPageIndex As Integer)
        Dim oClient As New ctlRMA.Client
        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

        dtRequest = oClient.Query01(RMA_NO.ToLower(), "", "", "", "", "-1", "", "91", Session("_RepairCenter"), "RMAD_RMANO desc,RMAD_SERIALNO")

        Call ArrangementData(dtRequest)
        Session("_dtRequest") = dtRequest
        Dim dvRequest As DataView = dtRequest.DefaultView
        Me.ViewState("_SortExpression") = "RMAD_RMANO"
        Me.ViewState("_SortDirection") = "desc"
        dvRequest.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Request_DataBind(dvRequest, iPageIndex)
    End Sub

    Private Sub QueryData_WarrParts()

        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = oExport.getCModelNo(UI_lblModelNoText.Text.Trim(), Me.UI_lblRepairNo.Text, Me.UI_ACCOUNTID.Text.Trim())

        'Dim dtWarrParts As DataTable = GetData(UI_lblSerialText.Text, UI_lblModelNoText.Text, UI_lblRepairNo.Text)
        Dim dtWarrParts As DataTable = GetData(UI_lblSerialText.Text, sModelNo, UI_lblRepairNo.Text)
        Me.ViewState("dtWarrParts") = dtWarrParts
        lstParts.DataSource = dtWarrParts
        lstParts.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRequest As RmaDTO.tmpRequest_ListDataTable)
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export

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


            Dim sModelNo As String = oExport.getMModelNo(dtRequest.Rows(i)("RMAD_MODELNO").ToString().Trim(), Me.UI_lblRepairNo.Text, Me.UI_ACCOUNTID.Text.Trim())

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

    Private Sub Request_DataBind(ByVal dvRequest As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvRequest.PageSize = _PageSize
        Me.UI_dvRequest.PageIndex = iPageIndex
        Me.UI_dvRequest.DataSource = dvRequest
        Me.UI_dvRequest.DataBind()

        Call setFlowCase01_UI_dvRequest()

    End Sub

    Protected Sub UI_dvRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRequest.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADSTATUS As Label = e.Row.FindControl("UI_RMADSTATUS")
            Dim UI_RMAR_COMPNO As Label = e.Row.FindControl("UI_RMAR_COMPNO")
            Dim UI_RMADID As Label = e.Row.FindControl("UI_RMADID")
            Dim UI_Check As CheckBox = e.Row.FindControl("UI_Check")
            Dim UI_RMAD_SERIALNO As LinkButton = e.Row.FindControl("UI_RMAD_SERIALNO")
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            Dim UI_RMADPARTSN As Label = e.Row.FindControl("UI_RMADPARTSN")

            Dim hsSelectID As Hashtable = ViewState("hsSelectID")

            If UI_RMADID.Text.Trim().Equals(RMADID) Then
                e.Row.BackColor = Drawing.Color.Pink
            End If

            If UI_RMADPARTSN.Text <> "" Then
                UI_RMAD_SERIALNO.Text = UI_RMADPARTSN.Text
            End If

            If UI_RMADSTATUS.Text.Trim() = "20" Or UI_RMADSTATUS.Text.Trim() = "30" Then
                UI_RMAD_SERIALNO.Enabled = True

                'ted 2015/3/12 start
                'UI_Check.Visible = True
                'If hsSelectID.ContainsKey(UI_RMADID.Text) Then
                '    UI_Check.Checked = True
                'End If
                'ted 2015/3/12 end


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

    Private Sub setFlowCase01_UI_dvRequest()
        If Me.UI_flowCase.Text = "01" Then
            Me.UI_dvRequest.Columns(7).Visible = False
        End If

        If Me.UI_flowCase.Text = "02" Then
            Me.UI_dvRequest.Columns(7).Visible = True
        End If

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

#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        oCommon.getRepairCenteryByDropDownList(True, Me.UI_cboAssignRepair, sText)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sText)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)
        Me.UI_dvRequest.Columns(1).HeaderText = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(2).HeaderText = _oLanguage.getText("RMA", "035", ctlLanguage.eumType.Tag)
        'Me.UI_dvRequest.Columns(3).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
        'Me.UI_dvRequest.Columns(4).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
        'Me.UI_dvRequest.Columns(5).HeaderText = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)

        Me.UI_dvRequest.Columns(7).HeaderText = _oLanguage.getText("RMA", "070", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(8).HeaderText = _oLanguage.getText("RMA", "032", ctlLanguage.eumType.Tag)
        Me.UI_dvRequest.Columns(9).HeaderText = _oLanguage.getText("RMA", "071", ctlLanguage.eumType.Tag)


        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "082", ctlLanguage.eumType.Tag)
        Me.UI_lblProductTittle.Text = _oLanguage.getText("RMA", "034", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyBI.Text = _oLanguage.getText("RMA", "224", ctlLanguage.eumType.Tag)

        'Me.UI_cmdApply.Text = _oLanguage.getText("Common", "081", ctlLanguage.eumType.Tag)
        Me.UI_cmdApply.Text = _oLanguage.getText("Common", "017", ctlLanguage.eumType.Command)

        Call setValidationMessage(Me.rfvSerialNo)
        Call setValidationMessage(Me.rfvProblemDesc)
        Call setValidationMessage(Me.rfvRepairDesc)
        Call setValidationMessage(Me.cvFailureClass)
        Call setValidationMessage(Me.cvFailure)
        Call setValidationMessage(Me.cvRepair)
        Call setValidationMessage(Me.rfvPartNo)


        '取得Tag Text
        Me.UI_QuoteInformation.Text = _oLanguage.getText("RMA", "063", ctlLanguage.eumType.Tag)

        Me.UI_lblModelNo.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRepair.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblAssign.Text = _oLanguage.getText("RMA", "052", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureClass.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

        Me.UI_lblProductDesc.Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
        Me.UI_lblProblemDesc.Text = _oLanguage.getText("RMA", "025", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairDesc.Text = _oLanguage.getText("RMA", "053", ctlLanguage.eumType.Tag)

        Me.UI_lblRepairMemo.Text = _oLanguage.getText("RMA", "054", ctlLanguage.eumType.Tag)

        Me.UI_lblEditor.Text = _oLanguage.getText("RMA", "060", ctlLanguage.eumType.Tag)
        'Me.UI_lblEditorText.Text = _oLanguage.getText("RMA", "061", ctlLanguage.eumType.Tag)
        Me.UI_lblImproperUsage.Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)
        Me.UI_opgImproPerusage.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_opgImproPerusage.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_lblApplyBatteryInsurance.Text = _oLanguage.getText("RMA", "222", ctlLanguage.eumType.Tag)
        Me.UI_opgApplyBatteryInsurance.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_opgApplyBatteryInsurance.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.uiTxt_ApplyBatteryInsurance.Text = _oLanguage.getText("RMA", "223", ctlLanguage.eumType.Tag)

        Me.UI_lblWarranty.Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)

        'Me.UI_radWarranty.Items(0).Text = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)

        Me.UI_chkWarranty_C0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_C1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)

        Me.UI_chkWarranty_S0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_S1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)


        Me.UI_chkWarranty_0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_1.ClientID & ",true)")
        Me.UI_chkWarranty_1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_0.ClientID & ",true)")

        Me.UI_chkWarranty_C0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_C1.ClientID & ",false)")
        Me.UI_chkWarranty_C1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_C0.ClientID & ",false)")

        Me.UI_chkWarranty_S0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_S1.ClientID & ",false)")
        Me.UI_chkWarranty_S1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_S0.ClientID & ",false)")

        Me.UI_opgImproPerusage.Attributes.Add("onclick", "opgImproPerusage()")
        'Me.UI_opgImproPerusage.Items(0).Attributes.Add("onclick", "opgImproPerusage()")
        'Me.UI_opgImproPerusage.Items(1).Attributes.Add("onclick", "opgImproPerusage()")

        Me.UI_lblQuickSearch.Text = _oLanguage.getText("RMA", "069", ctlLanguage.eumType.Tag)
        Me.UI_lblQcSn.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)


        'Replace Component
        Me.uiLbl_Repair_Model.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_PartsNo.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_Location.Text = _oLanguage.getText("RMA", "084", ctlLanguage.eumType.Tag)

        Me.uiLbl_Repair_Manpower.Text = _oLanguage.getText("RMA", "086", ctlLanguage.eumType.Tag)
        Me.uiTag_Repair_Parts.Text = _oLanguage.getText("RMA", "087", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_Hour.Text = _oLanguage.getText("RMA", "057", ctlLanguage.eumType.Tag)
        Me.uiTag_Repair_TotalText.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)
        Me.uiTag_Repair_Currency.Text = _oLanguage.getText("RMA", "085", ctlLanguage.eumType.Tag)

        'Me.uiTxt_Repair_ManHour.Attributes.Add("onkeyup", "cal_TotalAMT()")

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdParts_Search.Text = _oLanguage.getText("Common", "078", ctlLanguage.eumType.Command)

        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.UI_cmdSendMail.Text = _oLanguage.getText("Common", "083", ctlLanguage.eumType.Command)
        Me.btnQuickSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.Insurance_Label.Text = _oLanguage.getText("TotalLoss", "008", ctlLanguage.eumType.Tag)
        Me.UI_Apply_Total_Loss_Insurance.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_Apply_Total_Loss_Insurance.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)

        Me.rblBI.Items(0).Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.rblBI.Items(1).Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)

    End Sub

    Public Function getoLanguage(ByVal FUNCTIONNAME As String, ByVal KEY_ As String) As String
        Return _oLanguage.getText(FUNCTIONNAME, KEY_, ctlLanguage.eumType.Tag)
    End Function

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""

        Select Case oValidator.ID.ToString().Trim().ToLower()
            Case "rfvSerialNo".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "108", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvProblemDesc".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "072", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvRepairDesc".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "073", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvManHour".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "076", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rfvMaterial".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "077", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvManHour".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "076", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "rvMaterial".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "077", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cvFailureClass".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "078", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cvFailure".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "079", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

            Case "cvRepair".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "050", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

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

            Case "rfvPartNo".ToLower()
                sErrorMessage = _oLanguage.getText("RMA", "107", ctlLanguage.eumType.Validator)
                oValidator.ErrorMessage = sErrorMessage.ToString().Trim()

        End Select
    End Sub

    ''' <summary>
    ''' 不良原因代碼(下拉式)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboFailureClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFailureClass.SelectedIndexChanged
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)
    End Sub

    ''' <summary>
    ''' 變更維修中心
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboAssignRepair_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboAssignRepair.SelectedIndexChanged
        Call ClearQuote()
        Call QueryAssigeByCompany_Currency(Me.UI_cboAssignRepair.SelectedValue)

        '若改維修中心, Repair Desc 允許不輸入, Quote 可以不用填寫
        Me.rfvRepairDesc.Visible = True

        Me.UI_isRepairQuoted.Text = "1"
        If Me.UI_lblRepairNo.Text.Trim() <> Me.UI_cboAssignRepair.SelectedValue Then
            Me.UI_isRepairQuoted.Text = "0"
            Me.rfvRepairDesc.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' 清除畫面上資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearObjValue()
        Me.UI_lblRepairText.Text = ""
        Me.UI_lblRepairNo.Text = ""

        Me.UI_cboFailureClass.SelectedValue = "-1"
        Me.UI_cboFailure.SelectedValue = "-1"

        Me.UI_cboAssignRepair.SelectedValue = "-1"

        Me.UI_chkWarranty_0.Checked = False
        Me.UI_chkWarranty_1.Checked = False
        Me.UI_chkWarranty_C0.Checked = False
        Me.UI_chkWarranty_C1.Checked = False
        Me.UI_chkWarranty_S0.Checked = False
        Me.UI_chkWarranty_S1.Checked = False
        Me.UI_opgImproPerusage.SelectedValue = "0"
        Me.uiTxt_RMAD_ISCW.Text = ""

        Me.UI_txtProductDesc.Text = ""
        Me.UI_txtProblemDesc.Text = ""
        Me.UI_txtRepairDesc.Text = ""
        Me.UI_txtRepairMemo.Text = ""

        Me.UI_isRepairQuoted.Text = "1"

        Me.UI_txtAssigeCurrencyRate.Text = ""   '被指派的維修中心-幣別
        Me.UI_txtAssigeCurrencyCode.Text = ""   '被指派的維修中心-兌美金匯率

        Me.UI_flowCase.Text = ""                '紀錄執行哪個 flow case, 空白代表原流程

        Call ClearQuote()
    End Sub

    ''' <summary>
    ''' 清除畫面上 相關的費用 資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearQuote()
        'Me.UI_txtLABORPRICE.Text = ""           '取的維修中心-工時單價
        'Me.UI_txtTotalManAmt.Text = ""          '工時總金額
        'Me.UI_txtTotalQuote.Text = ""           '報價金額
        Me.UI_txtCurrencyRate.Text = ""         '幣別
        Me.UI_txtCurrencyCode.Text = ""         '兌美金匯率
    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01()
        'Manpower Hour
        Me.uiLbl_Repair_Manpower.Text = _oLanguage.getText("RMA", "404", ctlLanguage.eumType.Tag)
        Me.uiLbl_Repair_Hour.Text = Me.uiLbl_Repair_CurrencyCode.Text       '幣別
        Me.uiLbl_Repair_Hour.Text = ""
        Me.uiLbl_Repair_Hour_Delimited.Text = ""

        Me.UI_lblServiceCharge.Style("display") = "none"
        Me.uiTxt_Repair_ManHour.Style("display") = "none"
        Me.uiTxt_Repair_LABORPrice.Style("display") = "none"

        Me.uiTxt_Repair_PartsTotal.Style("display") = "none"    'parts price
        Me.fdt_Warranty.Visible = False '需求新增:BI保固 By buck Add 20250902
        UI_lblWarrantyBI.Visible = False
        If Me.UI_flowCase.Text = "01" Then
            Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "421", ctlLanguage.eumType.Tag)

            Me.UI_cboAssignRepair.Enabled = False

            Me.UI_chkWarranty_1.Enabled = False
            Me.UI_chkWarranty_0.Enabled = False

            Me.UI_chkWarranty_C1.Enabled = False
            Me.UI_chkWarranty_C0.Enabled = False

            Me.UI_chkWarranty_S1.Enabled = False
            Me.UI_chkWarranty_S0.Enabled = False

            'Service Charge
            Me.uiLbl_Repair_Manpower.Style("display") = "none"
            Me.uiLbl_Repair_Manpower_Delimited.Style("display") = "none"
            Me.uiLbl_Repair_LaborCost.Style("display") = "none"

            'Parts
            Me.uiTag_Repair_Parts.Style("display") = "none"
            Me.uiTag_Repair_Parts_Delimited.Style("display") = "none"
            Me.uiLbl_Repair_PartsTotal.Style("display") = "none"    'parts price

            'Total amount
            Me.uiTag_Repair_TotalText.Style("display") = "none"
            Me.uiTag_Repair_TotalText_Delimited.Style("display") = "none"
            Me.uiLbl_Repair_Total.Style("display") = "none"         'Total amount

            Me.UI_cmdSendMail.Visible = False
            Me.UI_cmdParts_Search.Visible = False
        End If


        If Me.UI_flowCase.Text = "02" Then
            Me.uiTxt_Repair_ManHour.Style("display") = ""
            Me.uiLbl_Repair_LaborCost.Style("display") = "none"

            Me.uiTxt_Repair_ManHour.Attributes.Add("onkeyup", "cal_TotalAMT()")

            Me.UI_cmdSendMail.Visible = False
            Me.UI_cmdParts_Search.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' 設定flow case 01 的畫面, 維修零件控制
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFlowCase01_UI_dvRepairDetail()
        Dim i As Integer = 0

        For i = 0 To UI_dvRepairDetail.Controls.Count - 1
            'oTableHeader
            Dim oTableHeader As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableHeader")
            If IsNothing(oTableHeader) = False Then
                oTableHeader.Rows(0).Cells(2).Visible = False   'Waive filed
                oTableHeader.Rows(0).Cells(3).Visible = False    'Option filed


                If Me.UI_flowCase.Text = "01" Then
                    oTableHeader.Rows(0).Cells(2).Visible = True   'Waive filed
                    oTableHeader.Rows(0).Cells(3).Visible = True    'Option filed
                    oTableHeader.Rows(0).Cells(11).Visible = False    'price filed
                End If

                If Me.UI_flowCase.Text = "02" Then
                    oTableHeader.Rows(0).Cells(2).Visible = True   'Waive filed
                    oTableHeader.Rows(0).Cells(3).Visible = True    'Option filed
                    oTableHeader.Rows(0).Cells(11).Visible = True    'price filed
                End If
            End If


            'oTableRow
            Dim oTableRow As Table = Me.UI_dvRepairDetail.Controls(i).FindControl("oTableRow")
            If IsNothing(oTableRow) = False Then
                oTableRow.Rows(0).Cells(2).Visible = False   'Waive filed
                oTableRow.Rows(0).Cells(3).Visible = False    'Option filed

                If Me.UI_flowCase.Text = "01" Then
                    Dim txtDescription As TextBox = oTableRow.Rows(0).FindControl("txtDescription")
                    'txtDescription.Enabled = False
                    oTableRow.Rows(0).Cells(2).Visible = True   'Waive filed
                    oTableRow.Rows(0).Cells(3).Visible = True    'Option filed
                    oTableRow.Rows(0).Cells(11).Visible = False    'price filed
                End If

                If Me.UI_flowCase.Text = "02" Then
                    oTableRow.Rows(0).Cells(2).Visible = True   'Waive filed
                    oTableRow.Rows(0).Cells(3).Visible = True    'Option filed
                    oTableRow.Rows(0).Cells(11).Visible = True    'price filed
                End If

            End If
        Next
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
            Me.UI_txtAssigeCurrencyCode.Text = dr.CURRENCY_CODE.Trim()              '指派的維修中心-幣別
            Me.UI_txtAssigeCurrencyRate.Text = dr.CURRENCY_RATE.ToString()          '指派的維修中心-兌美金匯率
        End If
    End Sub

    ''' <summary>
    ''' 取得 被指派的維修中心 相關的幣別資料
    ''' </summary>
    ''' <param name="COMP_NO">公司代碼</param>
    ''' <remarks></remarks>
    Private Sub QueryAssigeByCompany_Currency(ByVal COMP_NO As String)
        Dim blnFlag_Cal As Boolean = False
        Dim iTotalManAmt As Decimal = 0
        Dim iMaterial As Decimal = 0

        Dim oCompany As New ctlCompany
        Dim dtCurrency As New CompanyDTO.vwCompany_CurrencyDataTable

        dtCurrency = oCompany.QueryByCurrency(COMP_NO)
        If dtCurrency.Rows.Count > 0 Then
            Dim dr As CompanyDTO.vwCompany_CurrencyRow = dtCurrency.Rows(0)

            'Me.UI_txtLABORPRICE.Text = dr.COMP_LABORCOST.ToString()           '取的維修中心-工時單價
            Me.UI_txtCurrencyCode.Text = dr.CURRENCY_CODE.Trim()              '被指派的維修中心-幣別
            Me.UI_txtCurrencyRate.Text = dr.CURRENCY_RATE.ToString()          '被指派的維修中心-兌美金匯率

            Me.uiTxt_Repair_LABORPrice.Text = dr.COMP_LABORCOST.ToString()    '取得維修中心 工時單價
            Me.uiLbl_Repair_CurrencyCode.Text = dr.CURRENCY_CODE.Trim()

            '======================================================================================================================================================
            'START
            '2015/3/10 沒有人工費用了, 都改成客戶 Service chargee  概念
            'Me.UI_txtLABORPRICE.Text = "1"
            Me.uiTxt_Repair_LABORPrice.Text = "1"
            'END
            '======================================================================================================================================================

            '幣別
            ' '' '' ''Me.UI_lblCurrencySymbol.Text = dr.CURRENCY_CODE.Trim() & ":"
            Call setFlowCase01()
        End If
    End Sub

    ''' <summary>
    ''' 取得客戶 Discount 及 Service Charge
    ''' </summary>
    ''' <param name="CU_NO"></param>
    ''' <remarks></remarks>
    Private Sub QueryCustomer(ByVal CU_NO As String)
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable

        Me.UI_CU_DISCOUNT_OFF.Text = "1"
        Me.UI_CU_SERVICE_CHG.Text = "0"

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
                Me.UI_CU_SERVICE_CHG.Text = CU_SERVICE_CHG
            End If
        End If

        Me.UI_lblServiceCharge.Text = ServiceChargeRule_Exception(Me.UI_CU_SERVICE_CHG.Text.Trim()).ToString()
        Me.uiTxt_Repair_ManHour.Text = Me.UI_lblServiceCharge.Text
    End Sub

    Private Sub QueryData_Head()
        Dim i As Integer = 0
        Dim isWarranty_Flg As Boolean = False       '是否要保固日期內
        Dim ImproPerusage_Flg As Boolean = False    'Improper Usage 
        Dim oExport As New ctlRMA.Export

        Me.UI_RMANo.Text = ""
        Me.UI_CUNO.Text = ""
        Me.UI_ACCOUNTID.Text = ""
        Me.UI_CUName.Text = ""
        Me.UI_lblModelNoText.Text = ""
        Me.UI_lblSerialText.Text = ""
        Me.UI_lblRepairNo.Text = ""
        Me.UI_lblRepairText.Text = ""
        Me.UI_txtProductDesc.Text = ""
        Me.UI_txtProblemDesc.Text = ""
        Me.UI_txtProblemDesc.Text = ""
        Me.UI_txtRepairDesc.Text = ""
        Me.UI_txtRepairMemo.Text = ""
        Me.UI_lblEditName.Text = ""
        Me.UI_lblLuStmp.Text = ""

        '==============================================================================================================================
        '費用
        '==============================================================================================================================
        Me.UI_lblServiceCharge.Text = "0"
        Me.uiTxt_Repair_ManHour.Text = "0"


        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        Dim oRepairQuoting As New ctlRMA.Repair_Quoting
        Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable

        dtRepairQuoting = oRepairQuoting.Query(Me.ViewState("_RMADID").ToString().Trim(), "91")
        If dtRepairQuoting.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwRepair_QuotingRow = dtRepairQuoting.Rows(0)

            Me.UI_RMANo.Text = dr.RMA_NO.Trim()
            Me.UI_RMA_APPLICANT.Text = dr.RMA_APPLICANT.Trim()
            Me.UI_RMA_MAIL.Text = dr.RMA_MAIL.Trim()
            Me.UI_CUNO.Text = dr.RMA_CUNO.Trim()
            Me.UI_ACCOUNTID.Text = dr.RMA_ACCOUNTID.Trim()
            Me.UI_CUName.Text = dr.CU_NAME.Trim()

            Dim sModelNo As String = oExport.getMModelNo(dr.RMAD_MODELNO.Trim(), dr.RMA_COMPNO.Trim(), Me.UI_ACCOUNTID.Text.Trim())

            If dr.IsRMAD_MODELNONull = False Then Me.UI_lblModelNoText.Text = sModelNo  'dr.RMAD_MODELNO.Trim()
            If dr.IsRMAD_SERIALNONull = False Then
                Me.UI_lblSerialText.Text = dr.RMAD_SERIALNO.Trim()
                UI_lblShowSerial.Text = dr.RMAD_SERIALNO.Trim()
            End If
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                UI_lblShowSerial.Text = dr.RMAD_PARTSN.Trim()
            End If
            '原本維修中心
            Me.UI_lblRepairNo.Text = dr.RMA_COMPNO.Trim()
            Me.UI_lblRepairText.Text = dr.COMP_NAME.Trim()

            Call chkFlowCase()
            Call QueryCustomer(Me.UI_CUNO.Text)                 '取得 客戶相關資料
            Call QueryByCompany_Currency(dr.RMA_COMPNO)         '取得 指派的維修中心 相關的幣別資料

            If dr.IsRMAR_FARCNONull = False Then
                For i = 0 To Me.UI_cboFailureClass.Items.Count - 1
                    If Me.UI_cboFailureClass.Items(i).Value.ToLower() = dr.RMAR_FARCNO.ToLower().Trim() Then
                        Me.UI_cboFailureClass.SelectedValue = dr.RMAR_FARCNO.Trim()
                        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)
                        Exit For
                    End If
                Next

            Else
                If dr.IsRMAD_FARFARCNONull = False Then
                    For i = 0 To Me.UI_cboFailureClass.Items.Count - 1
                        If Me.UI_cboFailureClass.Items(i).Value.ToLower() = dr.RMAD_FARFARCNO.ToLower().Trim() Then
                            Me.UI_cboFailureClass.SelectedValue = dr.RMAD_FARFARCNO.Trim()
                            oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sText)
                            Exit For
                        End If
                    Next
                End If
            End If


            If dr.IsRMAR_FARNONull = False Then
                For i = 0 To Me.UI_cboFailure.Items.Count - 1
                    If Me.UI_cboFailure.Items(i).Value.ToLower() = dr.RMAR_FARNO.ToLower().Trim() Then
                        Me.UI_cboFailure.SelectedValue = dr.RMAR_FARNO.Trim()
                        Exit For
                    End If
                Next

            Else
                If dr.IsRMAD_FARNONull = False Then
                    For i = 0 To Me.UI_cboFailure.Items.Count - 1
                        If Me.UI_cboFailure.Items(i).Value.ToLower() = dr.RMAD_FARNO.ToLower().Trim() Then
                            Me.UI_cboFailure.SelectedValue = dr.RMAD_FARNO.Trim()
                            Exit For
                        End If
                    Next
                End If
            End If


            Me.UI_chkWarranty_0.Checked = False
            Me.UI_chkWarranty_1.Checked = False

            If dr.IsRMAD_ISWARRANTYNull = True Then
                '維修報價時若是in warranty ,  Me.UI_chkWarranty_1.Checked = True , 反之 Me.UI_chkWarranty_0.Checked = True
                If dr.IsRMAD_WARRANTYNull = True Then
                    Me.UI_chkWarranty_0.Checked = True
                Else
                    Dim sWarranty As String = Convert.ToDateTime(dr.RMAD_WARRANTY).ToShortDateString()
                    Dim RMA_CSTMP As String = Convert.ToDateTime(dr.RMA_CSTMP).ToShortDateString()

                    '若申請日期超過保固日期, Me.UI_chkWarranty_0.Checked = True
                    If IsDate(sWarranty) = True Then
                        If Convert.ToDateTime(sWarranty) < Convert.ToDateTime(RMA_CSTMP) Then
                            Me.UI_chkWarranty_0.Checked = True
                        Else
                            Me.UI_chkWarranty_1.Checked = True
                            isWarranty_Flg = True
                        End If
                    End If

                End If

            Else
                Select Case dr.RMAD_ISWARRANTY
                    Case 0
                        Me.UI_chkWarranty_0.Checked = True
                    Case 1
                        Me.UI_chkWarranty_1.Checked = True
                        isWarranty_Flg = True
                End Select
            End If
            'Me.UI_chkWarranty_1.Checked = True

            Me.UI_chkWarranty_C0.Checked = False
            Me.UI_chkWarranty_C1.Checked = False
            If dr.IsRMAD_ISCWNull = True Then
                '維修報價時若是in cw warranty ,  Me.UI_chkWarranty_C1.Checked = True , 反之 Me.UI_chkWarranty_C0.Checked = True
                Dim oWarranty As New ctlWarranty
                Dim dtExport As DataTable = oWarranty.QueryExport(UI_lblSerialText.Text)
                If dtExport.Rows.Count > 0 Then
                    Dim dtCWStart As DateTime = Now.AddDays(1)
                    Dim dtCWEnd As DateTime = Now
                    If dtExport.Rows(0)("CW_SDATE").ToString().Trim() <> "" Then
                        dtCWStart = Convert.ToDateTime(dtExport.Rows(0)("CW_SDATE").ToString())
                    End If
                    If dtExport.Rows(0)("CW_EDATE").ToString().Trim() <> "" Then
                        dtCWEnd = Convert.ToDateTime(dtExport.Rows(0)("CW_EDATE").ToString())
                    End If
                    If DateTime.Now > dtCWStart And DateTime.Now < dtCWEnd Then
                        UI_chkWarranty_C1.Checked = True
                    Else
                        UI_chkWarranty_C0.Checked = True
                    End If
                Else
                    UI_chkWarranty_C0.Checked = True
                End If
            Else
                Select Case dr.RMAD_ISCW
                    Case 0
                        Me.UI_chkWarranty_C0.Checked = True
                    Case 1
                        Me.UI_chkWarranty_C1.Checked = True
                End Select
            End If

            Me.UI_chkWarranty_S0.Checked = False
            Me.UI_chkWarranty_S1.Checked = False
            If dr.IsRMAD_ISSWNull = True Then
                '維修報價時若是in sw warranty ,  Me.UI_chkWarranty_S1.Checked = True , 反之 Me.UI_chkWarranty_S0.Checked = True
                Dim oWarranty As New ctlWarranty
                Dim dtExport As DataTable = oWarranty.QueryExport(UI_lblSerialText.Text)
                If dtExport.Rows.Count > 0 Then
                    Dim dtSWStart As DateTime = Now.AddDays(1)
                    Dim dtSWEnd As DateTime = Now
                    If dtExport.Rows(0)("SW_SDATE").ToString().Trim() <> "" Then
                        dtSWStart = Convert.ToDateTime(dtExport.Rows(0)("SW_SDATE").ToString())
                    End If
                    If dtExport.Rows(0)("SW_EDATE").ToString().Trim() <> "" Then
                        dtSWEnd = Convert.ToDateTime(dtExport.Rows(0)("SW_EDATE").ToString())
                    End If
                    If DateTime.Now > dtSWStart And DateTime.Now < dtSWEnd Then
                        UI_chkWarranty_S1.Checked = True
                    Else
                        UI_chkWarranty_S0.Checked = True
                    End If
                Else
                    UI_chkWarranty_S0.Checked = True
                End If
            Else
                Select Case dr.RMAD_ISSW
                    Case 0
                        Me.UI_chkWarranty_S0.Checked = True
                    Case 1
                        Me.UI_chkWarranty_S1.Checked = True
                End Select
            End If


            '判斷維修中心是否有 Assign
            If dr.IsRMAR_COMPNONull = False Then
                Me.UI_cboAssignRepair.SelectedValue = dr.RMAR_COMPNO.Trim()

                '如果有被指派其它維修中心報價 且 已報價了, 被指派的功能必須鎖起來
                If dr.RMA_COMPNO.Trim <> dr.RMAR_COMPNO.Trim() And dr.IsRMARQ_QUOTENull = False Then
                    Me.UI_cboAssignRepair.Enabled = False
                End If

                ' ''如果有被指派其它維修中心報價, 被指派的功能必須鎖起來
                ''If dr.RMA_COMPNO.Trim <> dr.RMAR_COMPNO.Trim() Then
                ''    Me.UI_cboAssignRepair.Enabled = False
                ''End If

            Else
                Me.UI_cboAssignRepair.SelectedValue = dr.RMA_COMPNO.Trim()
            End If

            Me.UI_opgImproPerusage.SelectedValue = 0
            If dr.IsRMARQ_IMPROPERUSAGENull = False Then
                Me.UI_opgImproPerusage.SelectedValue = dr.RMARQ_IMPROPERUSAGE
            End If
            If Me.UI_opgImproPerusage.SelectedValue = 0 Then
                ImproPerusage_Flg = True
            End If
            '需求新增:BI保固 By buck Add 20250902 begin
            Me.UI_opgApplyBatteryInsurance.SelectedValue = 0
            If dr.IsRMAD_APPLY_BINull = False Then
                Me.UI_opgApplyBatteryInsurance.SelectedValue = dr.RMAD_APPLY_BI
            End If
            '需求新增:BI保固 By buck Add 20250902 end
            Me.uiTxt_RMAD_ISCW.Text = "0"
            If dr.IsRMAD_ISCWNull = False Then
                Me.uiTxt_RMAD_ISCW.Text = dr.RMAD_ISCW.ToString().Trim()
            End If
            'Me.uiTxt_RMAD_ISCW.Text = "1"

            If dr.IsRMAD_PRODUCTDESCNull = False Then Me.UI_txtProductDesc.Text = dr.RMAD_PRODUCTDESC.Trim()

            If dr.IsRMAR_PROBLEMDESCNull = False Then
                Me.UI_txtProblemDesc.Text = dr.RMAR_PROBLEMDESC.Trim()
            Else
                If dr.IsRMAD_PROBLEMDESCNull = False Then Me.UI_txtProblemDesc.Text = dr.RMAD_PROBLEMDESC.Trim()
            End If

            If dr.IsRMAR_REPAIRDESCNull = False Then Me.UI_txtRepairDesc.Text = dr.RMAR_REPAIRDESC.Trim()
            If dr.IsRMAR_REPAIRMEMONull = False Then Me.UI_txtRepairMemo.Text = dr.RMAR_REPAIRMEMO.Trim()

            If dr.IsRMARQ_LUADNAMENull = False Then Me.UI_lblEditName.Text = dr.RMARQ_LUADNAME.Trim()
            If dr.IsRMARQ_LUSTMPNull = False Then Me.UI_lblLuStmp.Text = dr.RMARQ_LUSTMP.ToString("yyyy/MM/dd HH:mm:ss")

            '人工維修費用(Service Charge 金額 * 人工每小時單價-->目前已不使用此規格
            'RMARQ_LABORPRICE : 人工每小時單價, 已用不到了, 預設是 1
            'RMARQ_LABORHOUR  : Service Charge 金額
            If dr.IsRMARQ_LABORHOURNull = False And dr.IsRMARQ_LABORPRICENull = False Then
                Dim sServiceCharge As String = ServiceChargeRule_Exception(dr.RMARQ_LABORHOUR * dr.RMARQ_LABORPRICE).ToString()

                Me.UI_lblServiceCharge.Text = sServiceCharge
                Me.uiTxt_Repair_ManHour.Text = Me.UI_lblServiceCharge.Text

                Me.uiTxt_Repair_LABORPrice.Text = dr.RMARQ_LABORPRICE.ToString()            ' 人工每小時單價, 已用不到了, 預設是 1
                Me.uiLbl_Repair_LaborCost.Text = Me.UI_lblServiceCharge.Text
            End If

            '報價零件加總金額
            If dr.IsRMARQ_MATERIALCOSTNull = False Then
                Me.uiLbl_Repair_PartsTotal.Text = dr.RMARQ_MATERIALCOST.ToString()
                Me.uiTxt_Repair_PartsTotal.Text = dr.RMARQ_MATERIALCOST.ToString()
            End If

            '總金額 (Service Charge + 報價零件加總金額)
            If dr.IsRMARQ_QUOTENull = False Then
                Me.uiLbl_Repair_Total.Text = dr.RMARQ_QUOTE.ToString()
            End If


            '==========================================================================================================================================================================================================================================
            '控制是否可填寫 維修報價項目
            '1.登入系統維修中心 跟 被指派維修中心不一致, 不可填寫維修報價項目
            '指派維修中心 與 被指派維修中心 是否一致, 不一致, 不可填寫維修報價
            '==========================================================================================================================================================================================================================================
            Me.UI_isRepairQuoted.Text = "1"
            If Session("_RepairCenter").ToString().IndexOf(Me.UI_cboAssignRepair.SelectedValue) < 0 Then
                Me.UI_isRepairQuoted.Text = "0"
            End If
        End If


        Call QueryAssigeByCompany_Currency(Me.UI_cboAssignRepair.SelectedValue)
        If Me.UI_lblRepairNo.Text.Trim() <> Me.UI_cboAssignRepair.SelectedValue Then
            Me.UI_cboAssignRepair.Enabled = False
        Else
            Me.UI_cboAssignRepair.Enabled = True
        End If

    End Sub

    Private Function Save_RMARepair() As RmaDTO.RMARepairDataTable
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Try
            Dim dr As RmaDTO.RMARepairRow = dtRMARepair.NewRMARepairRow

            dr.RMAR_ID = sGUID
            dr.RMAR_RMADID = Me.ViewState("_RMADID").ToString().Trim()
            dr.RMAR_COMPNO = Me.UI_cboAssignRepair.SelectedValue

            dr.RMAR_FARCNO = Me.UI_cboFailureClass.SelectedValue
            dr.RMAR_FARNO = Me.UI_cboFailure.SelectedValue

            If Me.UI_txtProblemDesc.Text.Trim <> "" Then dr.RMAR_PROBLEMDESC = Me.UI_txtProblemDesc.Text.Trim()
            If Me.UI_txtRepairDesc.Text.Trim() <> "" Then dr.RMAR_REPAIRDESC = Me.UI_txtRepairDesc.Text.Trim()
            If Me.UI_txtRepairMemo.Text.Trim() <> "" Then dr.RMAR_REPAIRMEMO = Me.UI_txtRepairMemo.Text.Trim()


            '是否已填寫維修報價單:0.否, 1.是
            dr.RMAR_REPAIR_ISFILL = "0"
            If Me.UI_isRepairQuoted.Text = "1" Then
                dr.RMAR_REPAIR_ISFILL = "1"
            End If

            dr.RMAR_AD = Session("_UserID")
            dr.RMAR_ADNAME = Session("_UserName")
            dr.RMAR_CSTMP = Date.Now
            dr.RMAR_LUAD = Session("_UserID")
            dr.RMAR_LUADNAME = Session("_UserName")
            dr.RMAR_LUSTMP = Date.Now

            dtRMARepair.AddRMARepairRow(dr)

        Catch ex As Exception
            Throw ex
        End Try

        Return dtRMARepair
    End Function

    Private Function Save_RepairQuoted() As RmaDTO.RMARepair_QuotedDataTable
        Dim blnFlag_Cal As Boolean = False
        Dim dtRMARepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString
        Dim iTotalManAmt As Double = 0

        Try
            Dim dr As RmaDTO.RMARepair_QuotedRow = dtRMARepairQuoted.NewRMARepair_QuotedRow

            dr.RMARQ_ID = sGUID
            dr.RMARQ_RMADID = Me.ViewState("_RMADID").ToString().Trim()
            dr.RMARQ_COMPNO = Me.UI_cboAssignRepair.SelectedValue
            dr.RMARQ_IMPROPERUSAGE = Me.UI_opgImproPerusage.SelectedValue
            'ted 2015/03/11 start
            'If Me.UI_lblTotalQuote.Text.Trim() <> "" And Me.UI_TRQuote.Visible = True Then
            '    dr.RMARQ_QUOTE = Convert.ToDouble(Me.UI_lblTotalQuote.Text.Trim())
            '    dr.RMARQ_CURRENCYCODE = Me.UI_txtCurrencyCode.Text.Trim()
            '    dr.RMARQ_CURRENCYRATE = Me.UI_txtCurrencyRate.Text.Trim()

            '    dr.RMARQ_ASSIGECURRENCYCODE = Me.UI_txtAssigeCurrencyCode.Text.Trim()
            '    dr.RMARQ_ASSIGECURRENCYRATE = Me.UI_txtAssigeCurrencyRate.Text.Trim()
            '    dr.RMARQ_ASSIGEQUOTE = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, dr.RMARQ_QUOTE, dr.RMARQ_ASSIGECURRENCYRATE)
            'End If
            'ted 2015/03/11 end

            dr.SetRMARQ_LABORHOURNull()
            dr.SetRMARQ_LABORPRICENull()
            dr.SetRMARQ_MATERIALCOSTNull()
            dr.SetRMARQ_QUOTENull()
            dr.SetRMARQ_CURRENCYCODENull()
            dr.SetRMARQ_CURRENCYRATENull()

            dr.SetRMARQ_ASSIGECURRENCYCODENull()
            dr.SetRMARQ_ASSIGECURRENCYRATENull()
            dr.SetRMARQ_ASSIGLABORCOSTNull()
            dr.SetRMARQ_ASSIGMATERIALCOSTNull()
            dr.SetRMARQ_ASSIGEQUOTENull()

            If Me.UI_isRepairQuoted.Text.Trim() = "1" Then
                '工時 Service Charge
                Dim RMARQ_LABORHOUR As Double = 0
                If Me.uiTxt_Repair_ManHour.Text.Trim <> "" Then
                    RMARQ_LABORHOUR = Math.Round(Convert.ToDouble(Me.uiTxt_Repair_ManHour.Text.Trim()), 2)
                End If
                dr.RMARQ_LABORHOUR = RMARQ_LABORHOUR


                '工時單價
                Dim RMARQ_LABORPRICE As Double = 0
                If Me.uiTxt_Repair_LABORPrice.Text.Trim <> "" Then
                    RMARQ_LABORPRICE = Convert.ToDouble(Me.uiTxt_Repair_LABORPrice.Text.Trim())
                End If
                dr.RMARQ_LABORPRICE = RMARQ_LABORPRICE


                '零件費用
                Dim RMARQ_MATERIALCOST As Double = 0
                If Me.uiTxt_Repair_PartsTotal.Text.Trim() <> "" Then
                    RMARQ_MATERIALCOST = Convert.ToDouble(Me.uiTxt_Repair_PartsTotal.Text.Trim())    '零件費用
                End If
                dr.RMARQ_MATERIALCOST = RMARQ_MATERIALCOST

                '費用加總(報價)
                Dim RMARQ_QUOTE As Double = 0
                If Me.uiLbl_Repair_Total.Text.Trim <> "" Then
                    RMARQ_QUOTE = Convert.ToDouble(Me.uiLbl_Repair_Total.Text.Trim())
                End If
                dr.RMARQ_QUOTE = RMARQ_QUOTE

                '被指派的維修中心 - 幣別代號(ex:NTD , USD)
                dr.RMARQ_CURRENCYCODE = Me.UI_txtCurrencyCode.Text.Trim()

                '被指派的維修中心 - 兌美金匯率
                dr.RMARQ_CURRENCYRATE = Me.UI_txtCurrencyRate.Text.Trim()

                '指派的維修中心 - 幣別代號(ex:NTD , USD)
                dr.RMARQ_ASSIGECURRENCYCODE = Me.UI_txtAssigeCurrencyCode.Text.Trim()

                '指派的維修中心 - 兌美金匯率
                dr.RMARQ_ASSIGECURRENCYRATE = Me.UI_txtAssigeCurrencyRate.Text.Trim()

                '轉換成 指派的維修中心 --> 工時費用
                Dim RMARQ_ASSIGLABORCOST As Double = Math.Round(RMARQ_LABORHOUR * RMARQ_LABORPRICE, 2)
                dr.RMARQ_ASSIGLABORCOST = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, RMARQ_ASSIGLABORCOST, dr.RMARQ_ASSIGECURRENCYRATE)

                '轉換成 指派的維修中心 --> 零件費用
                dr.RMARQ_ASSIGMATERIALCOST = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, dr.RMARQ_MATERIALCOST, dr.RMARQ_ASSIGECURRENCYRATE)

                '轉換成 指派的維修中心 --> 費用加總(報價)
                dr.RMARQ_ASSIGEQUOTE = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, dr.RMARQ_QUOTE, dr.RMARQ_ASSIGECURRENCYRATE)
            End If

            'ted 2015/03/11 start
            'If Me.UI_txtManHour.Text.Trim() <> "" And Me.UI_TRQuote.Visible = True Then
            '    dr.RMARQ_LABORHOUR = Math.Round(Convert.ToDouble(Me.UI_txtManHour.Text.Trim()), 2)         '工時
            '    If Me.UI_txtLABORPRICE.Text.Trim() <> "" Then
            '        dr.RMARQ_LABORPRICE = Convert.ToDouble(Me.UI_txtLABORPRICE.Text.Trim()) '工時單價

            '        iTotalManAmt = Convert.ToDouble(dr.RMARQ_LABORHOUR) * Convert.ToDouble(dr.RMARQ_LABORPRICE)
            '        iTotalManAmt = Math.Round(iTotalManAmt, 2)

            '        dr.RMARQ_ASSIGLABORCOST = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, iTotalManAmt, dr.RMARQ_ASSIGECURRENCYRATE)
            '    End If
            'End If
            'ted 2015/03/11 end


            'ted 2015/03/11 start
            'If Me.UI_txtMaterial.Text.Trim() <> "" And Me.UI_TRQuote.Visible = True Then
            '    dr.RMARQ_MATERIALCOST = Convert.ToDouble(Me.UI_txtMaterial.Text.Trim())    '零件費用
            '    dr.RMARQ_ASSIGMATERIALCOST = oCommon.ConvertCurrency(dr.RMARQ_CURRENCYRATE, dr.RMARQ_MATERIALCOST, dr.RMARQ_ASSIGECURRENCYRATE)
            'End If
            'ted 2015/03/11 end


            dr.RMARQ_ACCEPT = 1                     '是否要維修: 1.Accept, 2.Reject

            dr.RMARQ_AD = Session("_UserID")
            dr.RMARQ_ADNAME = Session("_UserName")
            dr.RMARQ_CSTMP = Date.Now
            dr.RMARQ_LUAD = Session("_UserID")
            dr.RMARQ_LUADNAME = Session("_UserName")
            dr.RMARQ_LUSTMP = Date.Now
            dr.RMARQ_EXPIRED_DATE = Date.Now.AddMonths(1) '新增訂單逾期日 by buck modify 20251118

            dtRMARepairQuoted.AddRMARepair_QuotedRow(dr)

        Catch ex As Exception
            Throw ex
        End Try


        Return dtRMARepairQuoted
    End Function

    Private Function Save_RepairQuotedDetail(ByRef UI_RMADID As String, ByRef bSavePartSn As Boolean) As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
        Dim i As Integer = 0
        'Dim iTotalPrice As Double = 0

        Dim oExport As New ctlRMA.Export
        Dim dtSession As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Try
            Dim COMPNO As String = Me.UI_cboAssignRepair.SelectedValue

            'oDataView.RowFilter = "RMARQD_MARK=0"
            For i = 0 To dtSession.Rows.Count - 1
                Dim drSession As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtSession.Rows(i)
                Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuotedDetail.NewRMAREPAIR_QUOTED_DETAILRow

                dr.RMARQD_ID = drSession.RMARQD_ID
                'dr.RMARQD_oldID = drSession.RMARQD_oldID
                'dr.RMARQD_RMADID = drSession.RMARQD_RMADID
                dr.RMARQD_RMADID = UI_RMADID
                dr.RMARQD_NSERIALNO = ""

                dr.RMARQD_NPARTNO = drSession.RMARQD_NPARTNO
                dr.RMARQD_NSERIALNO = ""
                Dim RMARQD_NSERIALNO As String = ""
                If drSession.IsRMARQD_NSERIALNONull = False Then
                    If bSavePartSn Then '只有被選中的保存 SN 其他不保存
                        RMARQD_NSERIALNO = drSession.RMARQD_NSERIALNO
                        dr.RMARQD_NSERIALNO = RMARQD_NSERIALNO
                    End If
                End If

                'Dim Warranty As String = oExport.getWarranty(dr.RMARQD_NPARTNO, RMARQD_NSERIALNO, COMPNO)
                Dim Warranty As String = oExport.getMaxWarranty(dr.RMARQD_NPARTNO, RMARQD_NSERIALNO, "", COMPNO)
                If Warranty <> "" Then
                    dr.RMARQD_NWARRANTY = Convert.ToDateTime(Warranty)
                End If


                Dim RMARQD_oPartNo As String = ""
                Dim RMARQD_oSerialNo As String = ""
                If drSession.IsRMARQD_OPARTNONull = False Then
                    RMARQD_oPartNo = drSession.RMARQD_OPARTNO
                    dr.RMARQD_OPARTNO = drSession.RMARQD_OPARTNO
                End If

                If drSession.IsRMARQD_OSERIALNONull = False Then
                    RMARQD_oSerialNo = drSession.RMARQD_OSERIALNO
                    dr.RMARQD_OSERIALNO = drSession.RMARQD_OSERIALNO
                End If

                If RMARQD_oPartNo.Trim() <> "" Or RMARQD_oSerialNo.Trim <> "" Then
                    'Warranty = oExport.getWarranty(RMARQD_oPartNo, RMARQD_oSerialNo, COMPNO)
                    Warranty = oExport.getMaxWarranty(RMARQD_oPartNo, RMARQD_oSerialNo, "", COMPNO)
                    If Warranty <> "" Then
                        dr.RMARQD_OWARRANTY = Convert.ToDateTime(Warranty)
                    End If
                End If


                If drSession.IsRMARQD_DESCNull = False Then dr.RMARQD_DESC = drSession.RMARQD_DESC
                If drSession.IsRMARQD_LOCATIONNull = False Then dr.RMARQD_LOCATION = drSession.RMARQD_LOCATION
                dr.RMARQD_IMPROPERUSAGE = drSession.RMARQD_IMPROPERUSAGE
                If drSession.IsRMARQD_DEFECTIVENull = False Then dr.RMARQD_DEFECTIVE = drSession.RMARQD_DEFECTIVE

                dr.RMARQD_QTY = drSession.RMARQD_QTY
                dr.RMARQD_MATERIALCOST = drSession.RMARQD_MATERIALCOST
                dr.RMARQD_PRICE = drSession.RMARQD_PRICE

                dr.RMARQD_CURRENCYCODE = Me.UI_txtCurrencyCode.Text.Trim()
                dr.RMARQD_CURRENCYRATE = Convert.ToDouble(Me.UI_txtCurrencyRate.Text.Trim())

                dr.RMARQD_ASSIGECURRENCYCODE = Me.UI_txtAssigeCurrencyCode.Text.Trim()                     '指派的維修中心 - 幣別代號(ex:NTD , USD)
                dr.RMARQD_ASSIGECURRENCYRATE = Convert.ToDouble(Me.UI_txtAssigeCurrencyRate.Text.Trim())   '指派的維修中心 - 兌美金匯率

                '轉換成 指派的維修中心 --> Price
                dr.RMARQD_ASSIGEPRICE = oCommon.ConvertCurrency(dr.RMARQD_CURRENCYRATE, dr.RMARQD_PRICE, dr.RMARQD_ASSIGECURRENCYRATE)

                dr.RMARQD_ACC = drSession.RMARQD_ACC        '表示保内零件到期收零件费用；
                dr.RMARQD_WAIVE = drSession.RMARQD_WAIVE        '表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                dr.RMARQD_OPTION = drSession.RMARQD_OPTION      '如果RMARQD_WAIVE=0, RMARQD_OPTION=1 讓客戶決定是否要修
                dr.RMARQD_OPTIONCLIENT = 2                      'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修

                dr.RMARQD_AD = Session("_UserID")
                dr.RMARQD_ADNAME = Session("_UserName")
                dr.RMARQD_CSTMP = Date.Now
                dr.RMARQD_LUAD = Session("_UserID")
                dr.RMARQD_LUADNAME = Session("_UserName")
                dr.RMARQD_LUSTMP = Date.Now
                dr.RMARQD_MARK = drSession.RMARQD_MARK

                dtRepairQuotedDetail.AddRMAREPAIR_QUOTED_DETAILRow(dr)
            Next

        Catch ex As Exception
            Throw ex
        End Try

        Return dtRepairQuotedDetail
    End Function

    ''' <summary>
    ''' 檢核 item 是否都正常
    ''' </summary>
    ''' <param name="sRMANo"></param>
    ''' <param name="isSalesConfirmed">判斷是否有執行 SalesConfirmed</param>
    ''' <returns>正常傳True, 反之False</returns>
    ''' <remarks></remarks>
    Private Function CHKQuotedItem_isNormal(ByVal sRMANo As String, ByRef isSalesConfirmed As Boolean) As Boolean
        Dim retval As Boolean = False
        Dim i As Integer = 0
        Dim ctlRMA As New ctlRMA.Repair_Quoting

        Try
            '================================================================================================================================================================================================
            '如果是 flow case 01 的狀況
            '檢核 RMA單裡的 維修報價 零件檔 規格及單價 是否有無值狀況.
            '檢核 都有值, 直接送到客戶端, 等待客戶確認
            '================================================================================================================================================================================================
            If Me.UI_flowCase.Text = "01" Or Me.UI_flowCase.Text = "02" Then
                If ctlRMA.FlowCase01_CHKQuotedItem_Abnormal(sRMANo) = False Then
                    Dim ctlSale As New ctlRMA.Sale
                    Dim dtRepairQuoted As New RmaDTO.RMARepair_QuotedDataTable

                    dtRepairQuoted = ctlRMA.qryRepair_Quoted(sRMANo)
                    For i = 0 To dtRepairQuoted.Rows.Count - 1
                        Dim drRepair_Quoted As RmaDTO.RMARepair_QuotedRow = dtRepairQuoted.Rows(i)

                        Dim dtSALE_QUOTED As New RmaDTO.RMASALE_QUOTEDDataTable
                        Dim drSALE_QUOTED As RmaDTO.RMASALE_QUOTEDRow = dtSALE_QUOTED.NewRMASALE_QUOTEDRow

                        Dim retvalRate As Double
                        Dim oConn As New ICAT_OracleDAO.Connection
                        oConn.Open()
                        Dim oCommand As OracleCommand = oConn.Command
                        Try
                            oCommand.CommandText = "SP_RATE_GET"
                            oCommand.CommandType = System.Data.CommandType.StoredProcedure
                            oCommand.Parameters.Add("vRMADID", OracleType.NVarChar).Value = drRepair_Quoted.RMARQ_RMADID
                            oCommand.Parameters("vRMADID").Direction = ParameterDirection.Input
                            oCommand.Parameters.Add("vResult", OracleType.Number)
                            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
                            oCommand.ExecuteNonQuery()
                            retvalRate = oCommand.Parameters("vResult").Value
                            oCommand.Parameters.Clear()
                            oCommand.CommandText = ""
                            oCommand.CommandType = CommandType.Text
                        Catch ex As Exception
                            Throw ex

                        Finally
                            oCommand.Dispose()
                        End Try


                        drSALE_QUOTED.RMASQ_ID = Guid.NewGuid().ToString()
                        drSALE_QUOTED.RMASQ_RMADID = drRepair_Quoted.RMARQ_RMADID

                        drSALE_QUOTED.RMASQ_LABORCOST = drRepair_Quoted.RMARQ_LABORHOUR * drRepair_Quoted.RMARQ_LABORPRICE
                        drSALE_QUOTED.RMASQ_MATERIALCOST = Math.Round(drRepair_Quoted.RMARQ_MATERIALCOST, 2)
                        drSALE_QUOTED.RMASQ_QUOTE = drRepair_Quoted.RMARQ_QUOTE

                        drSALE_QUOTED.RMASQ_CURRENCYCODE = drRepair_Quoted.RMARQ_CURRENCYCODE
                        drSALE_QUOTED.RMASQ_CURRENCYRATE = retvalRate
                        drSALE_QUOTED.RMASQ_DESC = ""

                        drSALE_QUOTED.RMASQ_AD = Session("_UserID")
                        drSALE_QUOTED.RMASQ_ADNAME = Session("_UserName")
                        drSALE_QUOTED.RMASQ_CSTMP = Date.Now
                        drSALE_QUOTED.RMASQ_LUAD = Session("_UserID")
                        drSALE_QUOTED.RMASQ_LUADNAME = Session("_UserName")
                        drSALE_QUOTED.RMASQ_LUSTMP = Date.Now

                        drSALE_QUOTED.RMASQ_SALEAD = Session("_UserID")
                        drSALE_QUOTED.RMASQ_SALEADNAME = Session("_UserName")
                        drSALE_QUOTED.RMASQ_SALEDATE = Date.Now

                        'drSALE_QUOTED.RMASQ_CLIENTCONFIRM = 1       '1.客戶確認, 2.業務帶客戶確認
                        dtSALE_QUOTED.AddRMASALE_QUOTEDRow(drSALE_QUOTED)

                        ctlSale.SaveAdd_SalesConfirmed(dtSALE_QUOTED)               '業務報價 並 確認
                        isSalesConfirmed = True
                    Next

                    retval = True
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 如果是 flow case 01 的請發通知給HQ 維修人員
    ''' RMA報價中，若其中有一筆零件規格空白或單價為0時，按confirm，顯示零件異常訊息並Mail HQ RMA人員。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    Private Function FlowCase01_Notice(ByVal dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable, ByVal sRMANo As String, ByVal sModelNo As String, ByVal sSERIALNO As String) As Boolean
        Dim j As Integer = 0
        Dim blnFlag As Boolean = False
        Dim isSendMail As Boolean = False
        Dim isSendMail_HQ As Boolean = False        '紀錄是否要寄給HQ人員
        Dim sMsg As String = ""

        Dim ctlRMA As New ctlRMA
        Dim ctlRMAR As New ctlRMA.Requested
        Dim oMail As New ctlMail
        Dim dt As DataTable

        Try
            If Me.UI_flowCase.Text = "01" Then
                isSendMail = True
            End If

            'dt = ctlRMAR.IsEndUser(Me.UI_CUNO.Text)
            ''If (dt.Rows.Count > 0 AndAlso dt(0)("EU_GP").ToString().Trim() <> "Y") Then
            ''    isSendMail = True
            ''    isSendMail_HQ = True
            ''End If

            '如果是enduser
            'If (dt.Rows.Count > 0) Then
            '    Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView
            '    dvRepairQuotedDetail.RowFilter = "RMARQD_MATERIALCOST > 0 AND RMARQD_MARK=0"
            '    '若有費用產生且尚未付款
            '    If (dvRepairQuotedDetail.Count > 0 AndAlso dt(0)("EU_GP").ToString().Trim() <> "Y") Then
            '        isSendMail = True
            '        isSendMail_HQ = True
            '    End If
            'End If

            If Me.UI_flowCase.Text = "01" Or Me.UI_flowCase.Text = "02" Then
                Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView
                dvRepairQuotedDetail.RowFilter = "(RMARQD_DESC='' OR RMARQD_MATERIALCOST=0) AND RMARQD_MARK=0"
                If dvRepairQuotedDetail.Count > 0 Then
                    isSendMail_HQ = True
                End If

                ' RMA報價中，若其中有一筆零件規格空白或單價為0時，按confirm，顯示零件異常訊息並Mail HQ RMA人員。
                If isSendMail_HQ = True And isSendMail = True Then

                    Dim oLoginInfo As New ctlLoginInfo
                    Dim LanguageID As String = oLoginInfo.getLanguageIDRMANO("Customer", sRMANo.Trim())


                    Dim sSubject_HQRepair As String = _oLanguage.getMailText("Mail", "407", ctlLanguage.eumType.Mail, LanguageID)
                    sSubject_HQRepair = sSubject_HQRepair.Replace("[$RMA No$]", sRMANo.Trim())
                    sSubject_HQRepair = sSubject_HQRepair.Replace("[$Serial No$]", sSERIALNO.Trim())

                    Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                    Dim Repaire_Name As String = ""                    '維修人員
                    Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RepairCenter(_HQRepairNo)

                    For j = 0 To arrRepaire.Count - 1
                        Dim arrList() As String = arrRepaire(j)
                        Repaire_Name = arrList(0).Trim()
                        Repaire_EMAIL = arrList(1).Trim()

                        Dim sBody_HQRepair As String = _oLanguage.getMailText("Mail", "408", ctlLanguage.eumType.Mail, LanguageID)

                        If Repaire_Name.Trim <> "" Then
                            sBody_HQRepair = sBody_HQRepair.Replace("[$HQRMA_Name$]", Repaire_Name)
                        Else
                            sBody_HQRepair = sBody_HQRepair.Replace("[$HQRMA_Name$]", "")
                        End If

                        If Repaire_EMAIL.Trim <> "" Then
                            sBody_HQRepair = sBody_HQRepair.Replace("[$RMA No$]", sRMANo.Trim())
                            sBody_HQRepair = sBody_HQRepair.Replace("[$Serial No$]", sSERIALNO.Trim())
                            sBody_HQRepair = sBody_HQRepair.Replace("[$Model No$]", sModelNo.Trim())

                            '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                            If _isDebug = True Then
                                Repaire_EMAIL = ConfigurationManager.AppSettings("MailTo")
                                _MailCC = ConfigurationManager.AppSettings("MailCC")
                            End If
                            'Repaire_EMAIL = "ted@icat-tech.com.tw"
                            blnFlag = oMail.SendMail(sSubject_HQRepair, sBody_HQRepair, Repaire_EMAIL, _MailCC)
                        End If
                    Next
                End If
            End If


        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If isSendMail_HQ = True Then
                'sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                'Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return isSendMail_HQ
    End Function

    ''' <summary>
    ''' 更換序號
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_lblSerialText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_lblSerialText.TextChanged

        Dim oExport As New ctlRMA.Export
        If Me.UI_lblSerialText.Text.Trim() <> "" Then
            Dim sModelNo As String = oExport.getModelNo(Me.UI_lblSerialText.Text.Trim())
            If sModelNo.Trim() = "" Then
                Me.UI_lblModelNoText.Text = "OTHER"
            Else
                Me.UI_lblModelNoText.Text = sModelNo
            End If
        End If

    End Sub

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_checkGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        UI_cmdApply.Enabled = True
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")


        '勾选选择资料
        For i = 0 To Me.UI_dvRequest.Rows.Count - 1
            If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_RMADID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMADID")
                Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")
                Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")

                If UI_Check.Visible = True Then
                    UI_Check.Checked = sender.Checked
                Else
                    If IsNothing(hsSelectID) = False Then
                        hsSelectID.Remove(UI_RMADID.Text.Trim())
                    End If
                End If
            End If
        Next



        '保存选择资料
        If Not Session("_dtRequest") Is Nothing Then
            Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            For i = 0 To dtReceiveList.Rows.Count - 1
                Dim sUI_RMADID As String = dtReceiveList.Rows(i)("RMAD_ID").ToString()
                Dim sUI_SERIALNO As String = dtReceiveList.Rows(i)("RMAD_SERIALNO").ToString()
                Dim sUI_RMADSTATUS As String = dtReceiveList.Rows(i)("RMAD_STATUS").ToString()
                Dim sRMAR_COMPNO As String = dtReceiveList.Rows(i)("RMAR_COMPNO").ToString()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 
                '35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                If sUI_RMADSTATUS.Trim() = "20" Or sUI_RMADSTATUS.Trim() = "30" Then
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



    End Sub

    Protected Sub UI_Check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        UI_cmdApply.Enabled = True
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")       ' 其他頁有被勾選的項目也會被記錄

        For i = 0 To Me.UI_dvRequest.Rows.Count - 1
            If Me.UI_dvRequest.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvRequest.Rows(i).FindControl("UI_Check")

                'mark 2011/01/20 修改
                'If CType(sender, CheckBox) Is UI_Check Then
                '    Dim UI_RMADID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMADID")
                '    Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")

                '    If IsNothing(hsSelectID) = False Then
                '        hsSelectID.Remove(UI_RMADID.Text.Trim())
                '    End If

                '    If UI_Check.Checked Then
                '        hsSelectID.Add(UI_RMADID.Text.Trim(), UI_SERIALNO.Text.Trim())
                '    End If
                'End If

                Dim UI_RMADID As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_RMADID")
                Dim UI_SERIALNO As Label = Me.UI_dvRequest.Rows(i).FindControl("UI_SERIALNO")
                If UI_Check.Checked Then
                    If hsSelectID.ContainsKey(UI_RMADID.Text.Trim()) = False Then
                        hsSelectID.Add(UI_RMADID.Text.Trim(), UI_SERIALNO.Text.Trim())
                    End If
                Else
                    If IsNothing(hsSelectID) = False Then
                        hsSelectID.Remove(UI_RMADID.Text.Trim())
                    End If
                End If
                'mark 2011/01/20 修改 end
            End If
        Next
        ViewState("hsSelectID") = hsSelectID
    End Sub

    Protected Sub UI_dvRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRequest.RowCommand

        If e.CommandName = "cmdChangeSn" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)

            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMANO As Label = row.FindControl("UI_RMANO")
            Dim cmdQuoting As LinkButton = row.FindControl("UI_Quoting")
            RMADID = UI_RMADID.Text.Trim()
            RMA_NO = UI_RMANO.Text.Trim()
            Me.UI_cmdSubmit.Enabled = True
            Me.UI_cmdApply.Enabled = True

            Call QueryData_Head()
            Call QueryData_Detail(UI_dvRequest.PageIndex)
            Call QueryByRepairQuotedDetail()
            Call QueryData_WarrParts()
            Call QuerySDC()
            Call QueryWarranty()

            Call Check_AXMT410_AXMT400(UI_lblShowSerial.Text.Trim())

            UI_cmdSubmit.Enabled = True
            UI_cmdApply.Enabled = True
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

        'If e.CommandName = "cmdSDC" Then
        '    Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        '    Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
        '    Dim UI_RMAD_SERIALNO As LinkButton = row.FindControl("UI_RMAD_SERIALNO")
        '    Me.UcSDCView.show(UI_RMAD_SERIALNO.Text.ToString().Trim(), True)
        'End If
        If e.CommandName = "cmdSWDetail" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow = Me.UI_dvRequest.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Me.ucSpecialSetting.show(UI_RMADID.Text.ToString().Trim(), True)
        End If
    End Sub

    Protected Sub UI_dvRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRequest.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryData_Head()
        If Not Session("_dtRequest") Is Nothing Then
            Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")
            Dim dvReceiveList As DataView = dtReceiveList.DefaultView
            Call Request_DataBind(dvReceiveList, iPageIndex)
        Else
            Call QueryData_Detail(iPageIndex)
        End If

    End Sub

#Region "Click"

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim blnFlag As Boolean = False
        Dim isWarranty As String = ""
        Dim isCWarranty As String = ""
        Dim isSWarranty As String = ""
        Dim blnFlag_isRepairQuoted As Boolean = False
        Dim blnQuotedItem_isNormal As Boolean = False
        Dim isSalesConfirmed As Boolean = False         '判斷是否有執行 SalesConfirmed

        Dim sMessageFailed As String = ""
        Dim sMessageAbnormal As String = ""
        Dim sMessageOK As String = ""

        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim dtRMARepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        Dim bSaveSn As Boolean = False
        Dim bIncludeTop As Boolean = False
        Dim Item As DictionaryEntry
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        'If (UI_lblRepairNo.Text.Trim() = "CLHQ" AndAlso uiTxt_Repair_ManHour.Text.Trim() = "0") Then
        '    Me.ucMessage.showMessageByFailed(_oLanguage.getText("Transfer", "047", ctlLanguage.eumType.Word)) 'Service Charge cannot be 0
        '    Exit Sub
        'End If

        'Throw New Exception("Me.UI_isRepairQuoted.Text : " & Me.UI_isRepairQuoted.Text)
        Try
            '2011/01/27 START
            '' ''Dim oExport As New ctlRMA.Export
            '' ''If Me.UI_lblSerialText.Text.Trim() <> "" Then
            '' ''    Dim sModelNo As String = oExport.getModelNo(Me.UI_lblSerialText.Text.Trim())
            '' ''    If sModelNo.Trim() = "" Then
            '' ''        Me.UI_lblModelNoText.Text = "OTHER"
            '' ''    Else
            '' ''        Me.UI_lblModelNoText.Text = sModelNo
            '' ''    End If
            '' ''End If
            '2011/01/27 END

            If Me.UI_lblRepairNo.Text.Trim() <> Me.UI_cboAssignRepair.SelectedValue Then
                Me.UI_cmdSubmit.Enabled = False
                Me.UI_cmdApply.Enabled = False
            End If


            '''' ted 2015/1/20 START
            Call Keep_RepairQuotedDetail_Data()
            Call RepairQuotedDetail_CalTotalAmt()
            '''' ted 2015/1/20 END

            If Me.UI_isRepairQuoted.Text = "1" Then
                blnFlag_isRepairQuoted = True
            End If

            Dim sRecord As String = ViewState("_RMADID").ToString().Trim()

            'Check
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            dtRequest = Session("_dtRequest")
            dtRequest.DefaultView.RowFilter = "RMAD_ID<>'" + sRecord + "'"

            Dim j As Integer = 0
            For j = 0 To dtRequest.DefaultView.Count - 1
                Dim sUI_SERIALNO1 As String = dtRequest.DefaultView(j)("RMAD_SERIALNO").ToString().Trim()
                If sUI_SERIALNO1 = UI_lblSerialText.Text.Trim() Then
                    dtRequest.DefaultView.RowFilter = ""
                    Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                End If
            Next
            dtRequest.DefaultView.RowFilter = ""

            'Save Data
            For Each Item In hsSelectID
                Dim sUI_RMADID As String = Item.Key.ToString()
                Dim sUI_SERIALNO As String = Item.Value.ToString()

                bSaveSn = False
                If sRecord = sUI_RMADID Then
                    bIncludeTop = True
                    bSaveSn = True
                End If

                ViewState("_RMADID") = sUI_RMADID
                dtRMARepair = Save_RMARepair()
                dtRMARepairQuoted = Save_RepairQuoted()                                     '維修報價檔
                dtRepairQuotedDetail = Save_RepairQuotedDetail(sUI_RMADID.Trim(), bSaveSn)  '維修報價零件檔

                If Me.UI_chkWarranty_0.Checked = True Then
                    isWarranty = "0"
                End If
                If Me.UI_chkWarranty_1.Checked = True Then
                    isWarranty = "1"
                End If
                If Me.UI_chkWarranty_C0.Checked = True Then
                    isCWarranty = "0"
                End If
                If Me.UI_chkWarranty_C1.Checked = True Then
                    isCWarranty = "1"
                End If
                If Me.UI_chkWarranty_S0.Checked = True Then
                    isSWarranty = "0"
                End If
                If Me.UI_chkWarranty_S1.Checked = True Then
                    isSWarranty = "1"
                End If

                '2011/01/27 START
                '' ''Dim sModelNo_Text As String = oExport.getModelNo(sUI_SERIALNO)
                '' ''If sModelNo_Text.Trim() = "" Then
                '' ''    sModelNo_Text = "OTHER"
                '' ''End If

                Dim sModelNo_Text As String = ""
                If bIncludeTop = True Then
                    sModelNo_Text = Me.UI_lblModelNoText.Text.Trim()
                End If

                oRepairQuoted.Save(dtRMARepair, dtRMARepairQuoted, dtRepairQuotedDetail, blnFlag_isRepairQuoted, isWarranty, isCWarranty, isSWarranty, sModelNo_Text, sUI_SERIALNO, bIncludeTop, Me.UI_flowCase.Text, Me.UI_RMANo.Text.Trim(), Me.UI_opgApplyBatteryInsurance.SelectedValue)

                '如果是 flow case 01 的請發通知給HQ 維修人員
                'RMA報價中，若其中有一筆零件規格空白或單價為0時，按confirm，顯示零件異常訊息並Mail HQ RMA人員。
                If FlowCase01_Notice(dtRepairQuotedDetail, Me.UI_RMANo.Text.Trim(), sModelNo_Text, sUI_SERIALNO) = True Then
                    sMessageAbnormal = _oLanguage.getText("RMA", "409", ctlLanguage.eumType.Tag)
                End If

                '2011/01/27 END
            Next


            ViewState("_RMADID") = sRecord
            '如果选择中没有包含此项目
            If bIncludeTop = False Then
                'RMARepair Table
                '如果 Me.UI_TRQuote.Visible = True 不管有沒有填寫金額, 都算是已填寫 維修報價單 狀態(RMAR_REPAIR_ISFILL=1)
                dtRMARepair = Save_RMARepair()
                dtRMARepairQuoted = Save_RepairQuoted()                         '維修報價檔
                dtRepairQuotedDetail = Save_RepairQuotedDetail(sRecord, True)   '維修報價零件檔

                If Me.UI_chkWarranty_0.Checked = True Then
                    isWarranty = "0"
                End If
                If Me.UI_chkWarranty_1.Checked = True Then
                    isWarranty = "1"
                End If
                If Me.UI_chkWarranty_C0.Checked = True Then
                    isCWarranty = "0"
                End If
                If Me.UI_chkWarranty_C1.Checked = True Then
                    isCWarranty = "1"
                End If
                If Me.UI_chkWarranty_S0.Checked = True Then
                    isSWarranty = "0"
                End If
                If Me.UI_chkWarranty_S1.Checked = True Then
                    isSWarranty = "1"
                End If

                oRepairQuoted.Save(dtRMARepair, dtRMARepairQuoted, dtRepairQuotedDetail, blnFlag_isRepairQuoted, isWarranty, isCWarranty, isSWarranty, Me.UI_lblModelNoText.Text, Me.UI_lblSerialText.Text, True, Me.UI_flowCase.Text, Me.UI_RMANo.Text.Trim(), Me.UI_opgApplyBatteryInsurance.SelectedValue)

                '如果是 flow case 01 的請發通知給HQ 維修人員
                'RMA報價中，若其中有一筆零件規格空白或單價為0時，按confirm，顯示零件異常訊息並Mail HQ RMA人員。
                If FlowCase01_Notice(dtRepairQuotedDetail, Me.UI_RMANo.Text.Trim(), Me.UI_lblModelNoText.Text.Trim(), Me.UI_lblSerialText.Text.Trim()) = True Then
                    sMessageAbnormal = _oLanguage.getText("RMA", "409", ctlLanguage.eumType.Tag)
                End If
            End If

            '================================================================================================================================================================================================
            '如果是 flow case 01 的狀況
            '檢核 RMA單裡的 維修報價 零件檔 規格及單價 是否有無值狀況.
            '檢核 都有值, 直接送到客戶端, 等待客戶確認
            '================================================================================================================================================================================================
            If sMessageAbnormal.Trim() = "" Then
                blnQuotedItem_isNormal = CHKQuotedItem_isNormal(Me.UI_RMANo.Text.Trim(), isSalesConfirmed)
            End If

            blnFlag = True


        Catch ex As Exception
            sMessageFailed = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessageFailed)
            Else
                sMessageOK = oCommon.getMessage(Common.enmMessage.ProcessOK)
                If sMessageAbnormal.Trim() = "" Then
                    If blnQuotedItem_isNormal = True Then
                        If isSalesConfirmed = True Then
                            Me.ViewState("_AttachmentFile") = ""
                            qryRepairQuotaionRPT()
                            SalesConfirmed_SendMail()
                        End If
                        Me.ucMessage.showMessageBySuccess(sMessageOK, ascx_ucMessage.eumTransferURL.Redirect, "Repair_WorkList.aspx")

                    Else
                        Me.ucMessage.showMessageByAlert(sMessageOK)
                    End If

                Else
                    Me.ucMessage.showMessageByFailed(sMessageAbnormal)
                End If


                Dim hsSelectClear As New Hashtable
                ViewState("hsSelectID") = hsSelectClear

                Call QueryData_Head()
                Call QueryData_Detail(UI_dvRequest.PageIndex)
                Call QueryByRepairQuotedDetail()
                Call QueryData_WarrParts()

                '可以再把勾勾選項勾起來
                'Dim oCheckBox As New CheckBox
                'UI_Check_CheckedChanged(oCheckBox, System.EventArgs.Empty)

                Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
                dtRequest = Session("_dtRequest")
                dtRequest.DefaultView.RowFilter = "RMAD_ID='" + ViewState("_RMADID").ToString() + "' AND RMAD_STATUS='60'"
                If dtRequest.DefaultView.Count > 0 Then
                    UI_cmdSubmit.Enabled = False
                    UI_cmdApply.Enabled = False
                End If
                dtRequest.DefaultView.RowFilter = ""


            End If

            Dim ID As String = ""
            Dim Project_No As String = ""
            Dim Project_Qty As String = ""
            Dim Order_No As String = ""
            Dim Order_Qty As String = ""
            Dim Total_Loss_Insurance As String = ""
            Dim RMAD_RMANO As String = ""
            Dim RMAD_SEQ As String = ""
            Dim RMAD_SERIALNO As String = ""


            If Insurance_Label.Visible = True And UI_Apply_Total_Loss_Insurance.Visible = True And Apply_Label.Visible = True Then

                Dim ctlWarranty_ As New ctlWarranty
                '提供序號
                Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial.Text.Trim())


                '新增RMA加購保固判斷
                Dim Select_WARRANTYSERIAL_DataTable As Boolean = ctlWarranty_.Select_WARRANTYSERIAL(UI_lblShowSerial.Text.Trim())

                If Select_WARRANTYSERIAL_DataTable Then

                    If EXPORT_EXPORT_ORDERNUMBER.Rows.Count > 0 Then

                        Dim Select_WARRANTYITEM_DataTable As New DataTable
                        Select_WARRANTYITEM_DataTable = ctlWarranty_.Select_WARRANTYITEM(UI_lblShowSerial.Text.Trim())
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
                        Try
                            '可更換數量
                            Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        Catch

                        End Try
                        '已更換數量
                        Product_D.Quantity_replaced = ""

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = Me.UI_RMANo.Text.Trim()
                        RMAD_SEQ = Product_D.Replaceable_quantity
                        RMAD_SERIALNO = Me.UI_lblShowSerial.Text.Trim()

                        If UI_Apply_Total_Loss_Insurance.SelectedItem.Value = "1" Then
                            ctlWarranty_.DELETE_axmt410_axmt400(RMAD_RMANO, RMAD_SERIALNO)
                            ctlWarranty_.Insert_axmt410_axmt400(ID, Project_No, Project_Qty, Order_No, Order_Qty, Total_Loss_Insurance, RMAD_RMANO, RMAD_SEQ, RMAD_SERIALNO)
                        Else
                            ctlWarranty_.DELETE_axmt410_axmt400(RMAD_RMANO, RMAD_SERIALNO)
                        End If
                    End If

                Else

                    Dim strarr() As String
                    strarr = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("EXPORT_ORDERNUMBER").ToString().Trim().Split("-")

                    '表格
                    Dim EXPORT_axmt410_axmt400 As New DataTable
                    EXPORT_axmt410_axmt400 = ctlWarranty_.EXPORT_axmt410_axmt400(strarr(0) & "-" & strarr(1), strarr(2))


                    If EXPORT_EXPORT_ORDERNUMBER.Rows.Count > 0 Then

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
                        Try
                            '可更換數量
                            Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                        Catch ex As Exception

                        End Try

                        '已更換數量
                        Product_D.Quantity_replaced = ""
                        pList.Add(Product_D)

                        ID = System.Guid.NewGuid.ToString()
                        Project_No = Product_D.Project_No
                        Project_Qty = Product_D.Project_Qty
                        Order_No = Product_D.Order_No
                        Order_Qty = Product_D.Order_Qty
                        Total_Loss_Insurance = Product_D.Replaceable_quantity
                        RMAD_RMANO = Me.UI_RMANo.Text.Trim()
                        RMAD_SEQ = ""
                        RMAD_SERIALNO = Me.UI_lblShowSerial.Text.Trim()

                        If UI_Apply_Total_Loss_Insurance.SelectedItem.Value = "1" Then
                            ctlWarranty_.DELETE_axmt410_axmt400(RMAD_RMANO, RMAD_SERIALNO)
                            ctlWarranty_.Insert_axmt410_axmt400(ID, Project_No, Project_Qty, Order_No, Order_Qty, Total_Loss_Insurance, RMAD_RMANO, RMAD_SEQ, RMAD_SERIALNO)
                        Else
                            ctlWarranty_.DELETE_axmt410_axmt400(RMAD_RMANO, RMAD_SERIALNO)
                        End If

                    End If
                End If

            End If



        End Try


        Try

            If rblBI.SelectedValue = 1 And plBI.Visible = True Then


                If txtRMAD_RMANO_QTY.Text.Trim() <> "0" And RMAD_RMANO.Visible = True And txtRMAD_RMANO_QTY.Visible = True And LabRMAD_RMANO_QTY.Visible = True Then

                    If txtRMAD_RMANO_QTY.Text.Trim() <> "" Then

                        If IsNumeric(txtRMAD_RMANO_QTY.Text.Trim()) Then

                            If Convert.ToInt32(txtRMAD_RMANO_QTY.Text.Trim()) > 0 Then

                                Call UpData_BI(Convert.ToInt32(txtRMAD_RMANO_QTY.Text.Trim()))

                            End If

                        End If

                    End If

                End If

            End If
        Catch ex As Exception

        Finally

        End Try

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

    Private Sub Check_AXMT410_AXMT400(ByVal UI_lblShowSerial_String As String)
        Try
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

                    If EXPORT_EXPORT_ORDERNUMBER.Rows.Count > 0 Then

                        '有的話帶入db資料
                        Insurance_Label.Visible = True
                        UI_Apply_Total_Loss_Insurance.Visible = True
                        Apply_Label.Visible = True

                        Dim ctlWarranty_ As New ctlWarranty


                        Dim dt As New DataTable
                        dt = ctlWarranty_.select_Project_No_RMAD_SERIALNO(RMA_NO.Trim(), UI_lblShowSerial_String)

                        Dim Index As Int32 = 1


                        If Not dt Is Nothing Then

                            If dt.Rows.Count > 0 Then
                                Index = 0
                            Else

                            End If

                        Else

                        End If

                        UI_Apply_Total_Loss_Insurance.SelectedIndex = Index

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
                    EXPORT_EXPORT_ORDERNUMBER = Apply_Total_Loss_Insurance(UI_lblShowSerial.Text.Trim())


                    '新增RMA加購保固判斷
                    Dim Select_WARRANTYSERIAL_DataTable As Boolean = ctlWarranty_.Select_WARRANTYSERIAL(UI_lblShowSerial.Text.Trim())

                    If Select_WARRANTYSERIAL_DataTable Then

                        If EXPORT_EXPORT_ORDERNUMBER.Rows.Count > 0 Then

                            Dim Select_WARRANTYITEM_DataTable As New DataTable
                            Select_WARRANTYITEM_DataTable = ctlWarranty_.Select_WARRANTYITEM(UI_lblShowSerial.Text.Trim())
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
                            Try

                                '可更換數量
                                Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(Select_WARRANTYITEM_DataTable.Rows(0)("WATI_QTY").ToString.Trim()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                            Catch

                            End Try

                            '已更換數量
                            Product_D.Quantity_replaced = ""

                            ID = System.Guid.NewGuid.ToString()
                            Project_No = Product_D.Project_No
                            Project_Qty = Product_D.Project_Qty
                            Order_No = Product_D.Order_No
                            Order_Qty = Product_D.Order_Qty
                            Total_Loss_Insurance = Product_D.Replaceable_quantity
                            RMAD_RMANO = Me.UI_RMANo.Text.Trim()
                            RMAD_SEQ = Product_D.Replaceable_quantity
                            RMAD_SERIALNO = Me.UI_lblShowSerial.Text.Trim()

                        End If

                    Else

                        Dim strarr() As String
                        strarr = EXPORT_EXPORT_ORDERNUMBER.Rows(0)("EXPORT_ORDERNUMBER").ToString().Trim().Split("-")
                        If strarr.Length = 3 Then
                            '表格
                            Dim EXPORT_axmt410_axmt400 As New DataTable
                            EXPORT_axmt410_axmt400 = ctlWarranty_.EXPORT_axmt410_axmt400(strarr(0) & "-" & strarr(1), strarr(2))


                            If EXPORT_EXPORT_ORDERNUMBER.Rows.Count > 0 Then

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
                                Try
                                    '可更換數量
                                    Product_D.Replaceable_quantity = Convert.ToString(Convert.ToInt32(EXPORT_axmt410_axmt400.Rows(0)("Order_Qty").ToString()) * Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(0)("Losstop").ToString()) / 100)
                                Catch ex As Exception

                                End Try
                                '已更換數量
                                Product_D.Quantity_replaced = ""
                                pList.Add(Product_D)

                                ID = System.Guid.NewGuid.ToString()
                                Project_No = Product_D.Project_No
                                Project_Qty = Product_D.Project_Qty
                                Order_No = Product_D.Order_No
                                Order_Qty = Product_D.Order_Qty
                                Total_Loss_Insurance = Product_D.Replaceable_quantity
                                RMAD_RMANO = Me.UI_RMANo.Text.Trim()
                                RMAD_SEQ = ""
                                RMAD_SERIALNO = Me.UI_lblShowSerial.Text.Trim()


                            End If
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

                End If

            End If

        Catch ex As Exception

            Insurance_Label.Visible = False
            UI_Apply_Total_Loss_Insurance.Visible = False
            Apply_Label.Visible = False

        Finally

        End Try


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
            If Me.UI_isRepairQuoted.Text = "1" Then
                blnFlag_isRepairQuoted = True
            End If

            '是否已填寫維修報價單:0.否, 1.是
            If blnFlag_isRepairQuoted = True Then
                '檢核是否已全部報完價, 是, 則寄送 整張報價 Mail
                If oRepairQuoted.CHKAllItem_isRepairQuoted(Me.UI_RMANo.Text.Trim) = True Then
                    '寄送 整張報價 Mail
                    Dim isSendMail As Boolean = SendMail("")
                    blnFlag = True

                Else
                    Dim dtReceiveList As RmaDTO.tmpRequest_ListDataTable = Session("_dtRequest")

                    '寄送 有勾選單項次報價 Mail
                    Dim hsSelectID As Hashtable = ViewState("hsSelectID")
                    Dim sRecord As String = ViewState("_RMADID").ToString().Trim()
                    dtReceiveList.DefaultView.RowFilter = "RMAD_ID='" + sRecord + "'"
                    sSERIALNO = sSERIALNO & dtReceiveList.DefaultView(0)("RMAD_SERIALNO").ToString().Trim()
                    dtReceiveList.DefaultView.RowFilter = ""

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
                    For i = 0 To dtReceiveList.Rows.Count - 1
                        Dim sUI_RMADID As String = dtReceiveList.Rows(i)("RMAD_ID").ToString()
                        Dim sUI_SERIALNO As String = dtReceiveList.Rows(i)("RMAD_SERIALNO").ToString()
                        Dim sUI_RMADSTATUS As String = dtReceiveList.Rows(i)("RMAD_STATUS").ToString()

                        For j = 0 To arrSERIALNO.Length - 1
                            If sUI_SERIALNO.Trim() = arrSERIALNO(j).Trim() And sUI_RMADSTATUS = "20" Then
                                Dim validatorMsg As String = _oLanguage.getText("RMA", "238", ctlLanguage.eumType.Validator)
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

                    Dim isSendMail As Boolean = SendMail(SerialItem)
                    blnFlag = True
                End If
            End If



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

    Protected Sub UI_cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdApply.Click
        Dim blnFlag As Boolean = False
        Dim isWarranty As String = ""
        Dim isCWarranty As String = ""
        Dim isSWarranty As String = ""

        Dim bSaveSn As Boolean = False
        Dim blnFlag_isRepairQuoted As Boolean = False
        Dim sMessage As String = ""
        Dim Item As DictionaryEntry
        Dim hsSelectID As Hashtable = ViewState("hsSelectID")

        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRMARepair As New RmaDTO.RMARepairDataTable
        Dim dtRMARepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        'If (UI_lblRepairNo.Text.Trim() = "CLHQ" AndAlso uiTxt_Repair_ManHour.Text.Trim() = "0") Then
        '    Me.ucMessage.showMessageByFailed(_oLanguage.getText("Transfer", "047", ctlLanguage.eumType.Word)) 'Service Charge cannot be 0
        '    Exit Sub
        'End If

        '2011/01/27 STRAT
        ' '' ''Dim oExport As New ctlRMA.Export
        ' '' ''If Me.UI_lblSerialText.Text.Trim() <> "" Then
        ' '' ''    Dim sModelNo As String = oExport.getModelNo(Me.UI_lblSerialText.Text.Trim())
        ' '' ''    If sModelNo.Trim() = "" Then
        ' '' ''        Me.UI_lblModelNoText.Text = "OTHER"
        ' '' ''    Else
        ' '' ''        Me.UI_lblModelNoText.Text = sModelNo
        ' '' ''    End If
        ' '' ''End If
        '2011/01/27 END


        Try
            If Me.UI_lblRepairNo.Text.Trim() <> Me.UI_cboAssignRepair.SelectedValue Then
                Me.UI_cmdSubmit.Enabled = False
                Me.UI_cmdApply.Enabled = False
            End If

            '''' ted 2015/1/20 START
            Call Keep_RepairQuotedDetail_Data()
            Call RepairQuotedDetail_CalTotalAmt()
            '''' ted 2015/1/20 END


            If Me.UI_isRepairQuoted.Text = "1" Then
                blnFlag_isRepairQuoted = True
            End If

            Dim bIsIncludeTop As Boolean = False
            Dim sRecord As String = ViewState("_RMADID").ToString().Trim()

            'Check
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            dtRequest = Session("_dtRequest")
            dtRequest.DefaultView.RowFilter = "RMAD_ID<>'" + sRecord + "'"
            Dim j As Integer = 0
            For j = 0 To dtRequest.DefaultView.Count - 1
                Dim sUI_SERIALNO1 As String = dtRequest.DefaultView(j)("RMAD_SERIALNO").ToString().Trim()
                If sUI_SERIALNO1 = UI_lblSerialText.Text.Trim() Then
                    dtRequest.DefaultView.RowFilter = ""
                    Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                End If
            Next
            dtRequest.DefaultView.RowFilter = ""


            'Save
            For Each Item In hsSelectID
                Dim sUI_RMADID As String = Item.Key.ToString()
                Dim sUI_SERIALNO As String = Item.Value.ToString()

                bSaveSn = False
                If sRecord = sUI_RMADID Then
                    bIsIncludeTop = True
                    bSaveSn = True
                End If

                ViewState("_RMADID") = sUI_RMADID
                dtRMARepair = Save_RMARepair()
                dtRMARepairQuoted = Save_RepairQuoted()                                     '維修報價檔
                dtRepairQuotedDetail = Save_RepairQuotedDetail(sUI_RMADID.Trim(), bSaveSn)  '維修報價零件檔


                If Me.UI_chkWarranty_0.Checked = True Then
                    isWarranty = "0"
                End If
                If Me.UI_chkWarranty_1.Checked = True Then
                    isWarranty = "1"
                End If
                If Me.UI_chkWarranty_C0.Checked = True Then
                    isCWarranty = "0"
                End If
                If Me.UI_chkWarranty_C1.Checked = True Then
                    isCWarranty = "1"
                End If
                If Me.UI_chkWarranty_S0.Checked = True Then
                    isSWarranty = "0"
                End If
                If Me.UI_chkWarranty_S1.Checked = True Then
                    isSWarranty = "1"
                End If

                '2011/01/27 START
                '' ''Dim sModelNo_Text As String = oExport.getModelNo(sUI_SERIALNO)
                '' ''If sModelNo_Text.Trim() = "" Then
                '' ''    sModelNo_Text = "OTHER"
                '' ''End If
                Dim sModelNo_Text As String = ""
                If bIsIncludeTop = True Then
                    sModelNo_Text = Me.UI_lblModelNoText.Text.Trim()
                End If

                oRepairQuoted.Save(dtRMARepair, dtRMARepairQuoted, dtRepairQuotedDetail, False, isWarranty, isCWarranty, isSWarranty, sModelNo_Text, sUI_SERIALNO, bIsIncludeTop, Me.UI_flowCase.Text, Me.UI_RMANo.Text.Trim(), Me.UI_opgApplyBatteryInsurance.SelectedValue)
                '2011/01/27 END
            Next

            ViewState("_RMADID") = sRecord
            '如果最上方没有被选择，则重新Apply一次
            If bIsIncludeTop = False Then
                dtRMARepair = Save_RMARepair()
                dtRMARepairQuoted = Save_RepairQuoted()                         '維修報價檔
                dtRepairQuotedDetail = Save_RepairQuotedDetail(sRecord, True)   '維修報價零件檔


                If Me.UI_chkWarranty_0.Checked = True Then
                    isWarranty = "0"
                End If
                If Me.UI_chkWarranty_1.Checked = True Then
                    isWarranty = "1"
                End If
                If Me.UI_chkWarranty_C0.Checked = True Then
                    isCWarranty = "0"
                End If
                If Me.UI_chkWarranty_C1.Checked = True Then
                    isCWarranty = "1"
                End If
                If Me.UI_chkWarranty_S0.Checked = True Then
                    isSWarranty = "0"
                End If
                If Me.UI_chkWarranty_S1.Checked = True Then
                    isSWarranty = "1"
                End If

                oRepairQuoted.Save(dtRMARepair, dtRMARepairQuoted, dtRepairQuotedDetail, False, isWarranty, isCWarranty, isSWarranty, Me.UI_lblModelNoText.Text, UI_lblSerialText.Text, True, Me.UI_flowCase.Text, Me.UI_RMANo.Text.Trim(), Me.UI_opgApplyBatteryInsurance.SelectedValue)
            End If

            '需求新增:BI保固 By buck Add 20250902 begin
            Dim dtRMAData = TryCast(Session("_dtRequest"), DataTable)
            If dtRMAData IsNot Nothing Then
                Dim RMADReq As RMADetailReq = comms.DataTableToModel(Of RMADetailReq)(dtRMAData)
                RMADReq.RMAD_APPLY_BI = Me.UI_opgApplyBatteryInsurance.SelectedValue
                Dim oWarrantyBI As New ctlRMA.WarrantyBI
                oWarrantyBI.UpdRMADetail_Apply_BI(RMADReq)
            End If
            '需求新增:BI保固 By buck Add 20250902 end
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.ProcessOK)
                Me.ucMessage.showMessageByFailed(sMsg)

                Dim hsSelectClear As New Hashtable
                ViewState("hsSelectID") = hsSelectClear

                Call QueryData_Head()
                Call QueryData_Detail(UI_dvRequest.PageIndex)
                Call QueryByRepairQuotedDetail()
                Call QueryData_WarrParts()

            End If
        End Try
    End Sub

    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click
        Dim hsSelectID As New Hashtable
        Me.ViewState("hsSelectID") = hsSelectID
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
                If sUI_RMADSTATUS.Trim() = "20" Or sUI_RMADSTATUS.Trim() = "30" Then
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
                    RMADID = sUI_RMADID.Trim()
                    RMA_NO = sUI_RMANO.Trim()
                    Call QueryData_Head()
                    Call QueryData_Detail(UI_dvRequest.PageIndex)
                    Call QueryByRepairQuotedDetail()
                    Call QueryData_WarrParts()

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
            dtCustomer = oCustomer.QueryUser(Me.UI_CUNO.Text, Me.UI_ACCOUNTID.Text, "")

            If dtCustomer.Rows.Count > 0 Then
                '================================================================================================================================================================================================================
                '維修中心報價確認-->對象(業務和助理)
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

                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.UI_ACCOUNTID.Text.Trim())

                Dim sSubject As String = _oLanguage.getMailText("Mail", "021", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "022", ctlLanguage.eumType.Mail, LanguageID)
                Dim sEmailURL As String = _oLanguage.getMailText("Mail", "005", ctlLanguage.eumType.Mail, LanguageID)

                If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                    Dim sMailTo As String = ""

                    sSubject = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())
                    sSubject = sSubject.Replace("[$RMA No$]", Me.UI_RMANo.Text.Trim())

                    If MailSales.Trim() = "" Then
                        sBody = sBody.Replace("[$Sales Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Sales Name$]", SalesName)
                        sMailTo = MailSales.Trim()
                    End If

                    If MailAssistant.Trim() = "" Then
                        sBody = sBody.Replace("[$Assistant Name$]", "")
                        sBody = sBody.Replace("/", "")
                    Else
                        sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                        If sMailTo.Trim <> "" Then
                            sMailTo = sMailTo & ","
                        End If
                        sMailTo = sMailTo & MailAssistant.Trim()
                    End If

                    sBody = sBody.Replace("[$RMA No$]", Me.UI_RMANo.Text.Trim())
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)
                    sBody = sBody.Replace("[$item$]", sSERIALNO)

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        sMailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)
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

#End Region

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Me.IsPostBack = True Then
            'Repair Bom 查回來後新增
            If Session("_RepairRart_Submit") = True Then
                Call Me.UI_cmdSearch_Click(Me.UI_cmdSearch, System.EventArgs.Empty)
                Me.uiTxt_Repair_PartsNo.Text = ""
            End If
            Session("_RepairRart_Submit") = False
        End If

    End Sub

    ''' <summary>
    ''' 查詢 RepairBom Part's No
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdParts_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ucRepairRarts.show = True
    End Sub

#Region "Part Replace Component"

    ''' <summary>
    ''' 增加零件項目(查詢零件)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call Keep_RepairQuotedDetail_Data()

        Dim i As Integer = 0
        Dim MaterialCost As Double = 0
        Dim sDescription As String = ""

        Dim ctlCipher As New ctlCipherlab.Quote
        Dim oRepairBOM As New ctlRMA.RepairBOM
        Dim dtRepairBOM As New RmaDTO.RepairBOMDataTable
        Dim dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dtRMAData As DataTable = TryCast(Session("_dtRequest"), DataTable)
        Dim sCU_NO = dtRMAData.AsEnumerable().Select(Function(x) If(x.IsNull("CU_NO"), "", x.Field(Of String)("CU_NO"))).FirstOrDefault()

        Dim sPartsNo As String = Me.uiTxt_Repair_PartsNo.Text.ToString().Trim()
        Dim sLocation As String = Me.uiTxt_Repair_Location.Text.ToString().Trim()

        dtRepairBOM = oRepairBOM.Query(Me.UI_lblRepairNo.Text.Trim(), sPartsNo, sLocation)
        If dtRepairBOM.Rows.Count > 0 Then
            'RMA報價價格調整 by buck modify 20251013
            'MaterialCost = Convert.ToDouble(dtRepairBOM.Rows(0)("RPBOM_MATERIALCOST"))
            sDescription = dtRepairBOM.Rows(0)("RPBOM_DESC").ToString().Trim()
        End If
        'RMA報價價格調整 by buck modify 20251013
        MaterialCost = ctlCipher.getNewSkuPriceFromERP(New CipherlabReq With {.CU_NO = sCU_NO, .Part_No = sPartsNo})

        If Me.UI_flowCase.Text = "02" And (sDescription.Trim() = "" Or MaterialCost = 0) Then
            Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA", "428", ctlLanguage.eumType.Tag))

        Else
            dtRepairQuotedDetail = addRepairDetail(sPartsNo, sLocation, MaterialCost, sDescription, dtRepairQuotedDetail)
            Session("_dtRepairQuotedDetail") = dtRepairQuotedDetail
            Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail, 0)
        End If


        Me.uiTxt_Repair_Model.Text = ""
        Me.uiTxt_Repair_PartsNo.Text = ""
        Me.uiTxt_Repair_Location.Text = ""
    End Sub

    ''' <summary>
    ''' 增加零件項目
    ''' </summary>
    ''' <param name="PartsNo">PartsNo</param>
    ''' <param name="Location">Location</param>
    ''' <param name="MaterialCost">MaterialCost</param>
    ''' <param name="sDescription">sDescription</param>
    ''' <param name="dtRepairQuotedDetail">RMAREPAIR_QUOTED_DETAILDataTable</param>
    ''' <returns>傳回RMARepair_DetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function addRepairDetail(ByVal PartsNo As String, ByVal Location As String, ByVal MaterialCost As Double, ByVal sDescription As String, ByVal dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable) As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        Dim oGuid As Guid = Guid.NewGuid
        Dim sGUID As String = oGuid.ToString

        Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuotedDetail.NewRMAREPAIR_QUOTED_DETAILRow

        dr.RMARQD_ID = sGUID.Trim()
        dr.RMARQD_oldID = ""
        dr.RMARQD_RMADID = Me.RMADID

        dr.RMARQD_NPARTNO = PartsNo.Trim()
        dr.RMARQD_NSERIALNO = ""

        dr.RMARQD_OPARTNO = ""
        dr.RMARQD_OSERIALNO = ""

        dr.RMARQD_DESC = sDescription.Trim()
        dr.RMARQD_LOCATION = Location
        dr.RMARQD_IMPROPERUSAGE = 0                 '非正常使用 : 0.No, 1.Yes
        dr.RMARQD_DEFECTIVE = ""

        dr.RMARQD_WAIVE = 0
        dr.RMARQD_ACC = 0
        dr.RMARQD_OPTION = 0

        dr.RMARQD_QTY = 1
        dr.RMARQD_MATERIALCOST = MaterialCost
        dr.RMARQD_PRICE = PartsRule_Exception(dr.RMARQD_QTY, dr.RMARQD_MATERIALCOST, dr.RMARQD_WAIVE, dr.RMARQD_ACC)


        dr.RMARQD_AD = Session("_UserID")
        dr.RMARQD_ADNAME = Session("_UserName")
        dr.RMARQD_CSTMP = Date.Now
        dr.RMARQD_LUAD = Session("_UserID")
        dr.RMARQD_LUADNAME = Session("_UserName")
        dr.RMARQD_LUSTMP = Date.Now
        dr.RMARQD_MARK = 0

        dr.RMARQD_CURRENCYCODE = Me.UI_txtAssigeCurrencyCode.Text.Trim()
        dr.RMARQD_CURRENCYRATE() = Me.UI_txtAssigeCurrencyRate.Text.Trim()

        dr.RMARQD_ASSIGECURRENCYCODE = Me.UI_txtCurrencyCode.Text.Trim()
        dr.RMARQD_ASSIGECURRENCYRATE = Me.UI_txtAssigeCurrencyRate.Text.Trim()
        dr.RMARQD_ASSIGEPRICE = oCommon.ConvertCurrency(dr.RMARQD_CURRENCYRATE, dr.RMARQD_PRICE, dr.RMARQD_ASSIGECURRENCYRATE)

        dtRepairQuotedDetail.AddRMAREPAIR_QUOTED_DETAILRow(dr)

        Return dtRepairQuotedDetail
    End Function

    Private Sub QueryByRepairQuotedDetail()
        Dim oRepairQuoted As New ctlRMA.Repair_Quoting
        Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable

        dtRepairQuotedDetail = oRepairQuoted.QueryByRepairQuotedDetail(Me.RMADID)
        Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail, 0)

    End Sub

    Private Sub RepairQuotedDetail_DataBind(ByVal dtRepairDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairQuotedDetail") = dtRepairDetail
        Call RepairQuotedDetail_CalTotalAmt()

        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView()
        dvRepairDetail.RowFilter = "RMARQD_MARK=0"

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.DataBind()

        Call setFlowCase01_UI_dvRepairDetail()
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

            'Dim lblWaive_Cell As TableHeaderCell = lblWaive.Parent
            'lblWaive_Cell.Visible = False
        End If


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chkParts As CheckBox = e.Item.FindControl("chkParts")
            Dim chhWaive As CheckBox = e.Item.FindControl("chhWaive")
            'chhWaive.Attributes.Add("onchange", "cal_subTotalAMT()")

            chkParts.Attributes.Add("onclick", "cal_subTotalAMT()")
            chhWaive.Attributes.Add("onclick", "cal_subTotalAMT()")
            '1. IF ISWARRANTY=Y OR ISCW=Y THEN RMARQD_PRICE = 0
            If (Me.UI_chkWarranty_1.Checked = True Or Me.UI_chkWarranty_C1.Checked = True) And Me.UI_opgImproPerusage.SelectedValue = 1 Then
                chkParts.Enabled = True
            Else
                chkParts.Enabled = False
            End If



            Dim lblNew As Label = e.Item.FindControl("lblNew")
            'Dim lblOld As Label = e.Item.FindControl("lblOld")
            lblNew.Text = _oLanguage.getText("RMA", "105", ctlLanguage.eumType.Tag)
            'lblOld.Text = _oLanguage.getText("RMA", "106", ctlLanguage.eumType.Tag)

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
            'Dim txtPrice As TextBox = e.Item.FindControl("txtPrice")

            rfvNewPart.ControlToValidate = txtNewPart.ID
            'rfvNewSerial.ControlToValidate = txtNewSerial.ID
            rvQty.ControlToValidate = txtQty.ID
            'rvPrice.ControlToValidate = txtPrice.ID

            Call setValidationMessage(rfvNewPart)
            'Call setValidationMessage(rfvNewSerial)
            Call setValidationMessage(rvQty)
            'Call setValidationMessage(rvPrice)


            '    Me.UI_lblPartsTotal.Text = Convert.ToInt32(Me.UI_lblPartsTotal.Text.ToString().Trim()) + Convert.ToInt32(txtPrice.Text.ToString().Trim())
            '    Me.UI_lblTotal.Text = Convert.ToInt32(Me.UI_lblPartsTotal.Text.ToString().Trim()) + Convert.ToInt32(Me.UI_lblLaborCost.Text.ToString().Trim())
        End If
    End Sub

    Protected Sub UI_dvRepairDetail_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles UI_dvRepairDetail.ItemCommand
        If e.CommandName = "cmdDel" Then
            Dim lblRMARQDID As Label = e.Item.FindControl("lblRMARQDID")

            Call Delete(lblRMARQDID.Text.ToString())
        End If
    End Sub

#End Region

#Region "相關金額計算"

    ''' <summary>
    ''' mplus 特殊報價重置金額 - 例外規格計算
    ''' </summary>
    ''' <param name="dtRepairQuotedDetail"></param>
    ''' <remarks></remarks>
    Private Sub ReCalcReset_RepairQuotedDetail_Data(dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable)

        If Me.UI_cboAssignRepair.SelectedValue <> "JP_BYTE_MPLUS" Then Exit Sub

        Dim i As Integer = 0
        'Dim dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView

        '20211216 若只有重置料號 BQB15MA000001 固定 100, 有其它維修料號 150
        dvRepairQuotedDetail.RowFilter = "RMARQD_NPARTNO='BQB15MA000001'"
        If dvRepairQuotedDetail.Count > 0 Then
            Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dvRepairQuotedDetail(0).Row
            If dtRepairQuotedDetail.Count = 1 Then
                dr.RMARQD_MATERIALCOST = 100
                dr.RMARQD_PRICE = 100
            Else
                dr.RMARQD_MATERIALCOST = 150
                dr.RMARQD_PRICE = 150
            End If
        End If

    End Sub

    ''' <summary>
    ''' 報價零件金額 - 例外規格計算
    ''' </summary>
    ''' <param name="RMARQD_MATERIALCOST"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PartsRule_Exception(ByVal RMARQD_QTY As Double, ByVal RMARQD_MATERIALCOST As Double, ByVal RMARQD_WAIVE As Integer, ByVal RMARQD_ACC As Integer) As Double
        Dim i As Integer = 0
        Dim RMARQD_PRICE As Double = 0
        Dim CU_DISCOUNT_OFF As Double = 1

        If Me.UI_CU_DISCOUNT_OFF.Text.Trim <> "" Then
            CU_DISCOUNT_OFF = Convert.ToDouble(Me.UI_CU_DISCOUNT_OFF.Text)
        End If

        '計算折扣後的零件金額
        If RMARQD_QTY > 0 And RMARQD_MATERIALCOST > 0 Then
            RMARQD_PRICE = Math.Round((RMARQD_QTY * RMARQD_MATERIALCOST) * CU_DISCOUNT_OFF, 2, MidpointRounding.AwayFromZero)
            addLog("Regular Price= " & RMARQD_PRICE)
        End If

        '保內(一般/CW), 只有勾"人為", 就視同保外. service charge + 物料, 都要收錢
        '1. IF ISWARRANTY=Y OR ISCW=Y THEN RMARQD_PRICE = 0
        If (Me.UI_chkWarranty_1.Checked = True Or Me.UI_chkWarranty_C1.Checked = True) And Me.UI_opgImproPerusage.SelectedValue = 0 Then
            '2017-08-29 Jack Modify
            '保內(一般/CW), 非"人為",切零件不需要收费，金额才给0 
            If RMARQD_ACC = 0 Then
                RMARQD_PRICE = 0
                If (Me.UI_chkWarranty_1.Checked = True) Then
                    addLog("Warranty for  UI_chkWarranty_1 ")
                End If
                If (Me.UI_chkWarranty_C1.Checked = True) Then
                    addLog("Warranty for UI_chkWarranty_C1 ")
                End If
            End If
        End If

        '2. 客戶編號為'Ni.'開頭的, RMARQD_PRICE = 0
        Dim arrCustomer() As String = Me.UI_Customer_ExceptionCharge.Text.Trim().Split(",")
        For i = 0 To arrCustomer.Length - 1
            If Me.UI_CUNO.Text.Trim().IndexOf(arrCustomer(i).ToString().Trim()) <> -1 Then
                RMARQD_PRICE = 0
                addLog("Warranty for Ni ")
                Exit For
            End If
        Next

        'waive：表示此零件是我方吸收必修，維修收費價格會是0；
        If RMARQD_WAIVE = 1 Then
            RMARQD_PRICE = 0
            addLog("Warranty for RMARQD_WAIVE ")
        End If

        Return RMARQD_PRICE
    End Function


    ''' <summary>
    ''' 報價Service Charge - 例外規格計算
    ''' </summary>
    ''' <param name="sServiceCharge"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ServiceChargeRule_Exception(ByVal sServiceCharge As String) As Double
        Dim i As Integer = 0
        Dim iServiceCharge As Double = 0

        If sServiceCharge.Trim() <> "" Then
            If IsNumeric(sServiceCharge) = True Then
                iServiceCharge = Convert.ToDouble(sServiceCharge)
            End If
        End If

        '保內(一般/CW), 只有勾"人為", 就視同保外. service charge + 物料, 都要收錢
        '1. IF ISWARRANTY=Y OR ISCW=Y THEN  SERVICE CHARGE = 0  
        If (Me.UI_chkWarranty_1.Checked = True Or Me.UI_chkWarranty_C1.Checked = True) And Me.UI_opgImproPerusage.SelectedValue = 0 Then
            iServiceCharge = 0
        End If

        '2. 客戶編號為'Ni.'開頭的, SERVICE CHARGE = 0  
        Dim arrCustomer() As String = Me.UI_Customer_ExceptionCharge.Text.Trim().Split(",")
        For i = 0 To arrCustomer.Length - 1
            If Me.UI_CUNO.Text.Trim().IndexOf(arrCustomer(i).ToString().Trim()) <> -1 Then
                iServiceCharge = 0
                Exit For
            End If
        Next

        Return iServiceCharge
    End Function

    ''' <summary>
    ''' 計算 維修相關 金額
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RepairQuotedDetail_CalTotalAmt()
        Dim blnFlag_Cal As Boolean = False
        Dim i As Integer = 0
        Dim iTotalParts As Double = 0
        'Dim iTotalAmt As Double = 0

        Dim dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView

        dvRepairQuotedDetail.RowFilter = "RMARQD_MARK=0"
        For i = 0 To dvRepairQuotedDetail.Count - 1
            Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dvRepairQuotedDetail(i).Row()

            If dr.RMARQD_PRICE > 0 Then
                iTotalParts = iTotalParts + dr.RMARQD_PRICE
            End If
        Next
        dvRepairQuotedDetail.RowFilter = ""

        '報價零件加總金額
        Me.uiLbl_Repair_PartsTotal.Text = iTotalParts.ToString()
        Me.uiTxt_Repair_PartsTotal.Text = iTotalParts.ToString()


        '計算Service Charge
        '人工維修費用(Service Charge 金額 * 人工每小時單價) -->已不計算人工每小時單價了
        'Dim iServiceCharge As Double = Convert.ToDouble(Me.uiTxt_Repair_ManHour.Text) * Convert.ToDouble(Me.uiTxt_Repair_LABORPrice.Text)
        Dim iServiceCharge As Double = 0

        Select Case Me.UI_flowCase.Text
            Case "01"
                iServiceCharge = ServiceChargeRule_Exception(Me.UI_CU_SERVICE_CHG.Text.Trim()).ToString()

            Case "02"
                iServiceCharge = ServiceChargeRule_Exception(Me.uiTxt_Repair_ManHour.Text.Trim()).ToString()

            Case Else
                iServiceCharge = ServiceChargeRule_Exception(Me.UI_CU_SERVICE_CHG.Text.Trim()).ToString()
        End Select
        Me.uiTxt_Repair_ManHour.Text = iServiceCharge.ToString()
        Me.uiLbl_Repair_LaborCost.Text = iServiceCharge.ToString()

        '總金額 (Service Charge + 報價零件加總金額)
        Me.uiLbl_Repair_Total.Text = (iServiceCharge + iTotalParts).ToString()
    End Sub

#End Region

    ''' <summary>
    ''' 將畫面DataList的值儲存至暫存Table(_dtRepairQuotedDetail)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Keep_RepairQuotedDetail_Data()
        Dim i As Integer = 0
        Dim dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView

        For i = 0 To Me.UI_dvRepairDetail.Items.Count - 1
            Dim chkParts As CheckBox = Me.UI_dvRepairDetail.Items(i).FindControl("chkParts")
            Dim chhWaive As CheckBox = Me.UI_dvRepairDetail.Items(i).FindControl("chhWaive")
            Dim chkOption As CheckBox = Me.UI_dvRepairDetail.Items(i).FindControl("chkOption")
            Dim lblRMARQDID As Label = Me.UI_dvRepairDetail.Items(i).FindControl("lblRMARQDID")
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
            Dim UI_txtRMARQD_PRICE As TextBox = Me.UI_dvRepairDetail.Items(i).FindControl("UI_txtRMARQD_PRICE")
            'Dim txtPrice As Label = Me.UI_dvRepairDetail.Items(i).FindControl("txtPrice")

            dvRepairQuotedDetail.RowFilter = "RMARQD_ID='" & lblRMARQDID.Text.Trim() & "'"
            If dvRepairQuotedDetail.Count > 0 Then
                Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dvRepairQuotedDetail(0).Row
                Dim RMARQD_WAIVE As Integer = 0
                Dim RMARQD_OPTION As Integer = 0
                Dim RMARQD_ACC As Integer = 0

                If chkParts.Checked = True Then RMARQD_ACC = 1
                If chhWaive.Checked = True Then RMARQD_WAIVE = 1
                If chkOption.Checked = True Then RMARQD_OPTION = 1

                dr.RMARQD_ACC = RMARQD_ACC
                dr.RMARQD_WAIVE = RMARQD_WAIVE
                dr.RMARQD_OPTION = RMARQD_OPTION

                dr.RMARQD_NPARTNO = txtNewPart.Text.Trim()
                dr.RMARQD_NSERIALNO = txtNewSerial.Text.Trim()
                dr.RMARQD_OPARTNO = txtOldPart.Text.Trim()
                dr.RMARQD_OSERIALNO = txtOldSerial.Text.Trim()

                dr.RMARQD_DESC = txtDescription.Text.Trim()
                dr.RMARQD_LOCATION = txtLocation.Text.Trim()
                dr.RMARQD_IMPROPERUSAGE = cboImproper.SelectedValue
                dr.RMARQD_DEFECTIVE = cboDefective.SelectedValue

                dr.RMARQD_QTY = txtQty.Text.Trim()
                dr.RMARQD_MATERIALCOST = UI_txtMaterialCost.Text.Trim()
                dr.RMARQD_PRICE = PartsRule_Exception(dr.RMARQD_QTY, dr.RMARQD_MATERIALCOST, dr.RMARQD_WAIVE, dr.RMARQD_ACC)
                addLog("PARTNO= " & txtNewPart.Text.Trim() & " MCOST= " & UI_txtMaterialCost.Text.Trim() & " WAIVE= " & dr.RMARQD_WAIVE & " PRICE=  " & dr.RMARQD_PRICE & " PartAccount=  " & dr.RMARQD_ACC)
            End If
        Next

        ReCalcReset_RepairQuotedDetail_Data(dtRepairQuotedDetail)

        Session("_dtRepairQuotedDetail") = dtRepairQuotedDetail
    End Sub

    Private Sub Delete(ByVal RMARED_ID As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Call Keep_RepairQuotedDetail_Data()

        Dim dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable = Session("_dtRepairQuotedDetail")
        Dim dvRepair As DataView = dtRepairQuotedDetail.DefaultView

        dvRepair.RowFilter = "RMARQD_ID='" & RMARED_ID.ToString().Trim() & "'"
        If dvRepair.Count > 0 Then
            dvRepair.Item(0)("RMARQD_MARK") = "1"
        End If
        dvRepair.RowFilter = ""

        Session("_dtRepairQuotedDetail") = dtRepairQuotedDetail
        Call RepairQuotedDetail_DataBind(dtRepairQuotedDetail, 0)
    End Sub

    ''' <summary>
    ''' Reseller/End user報價單通知信
    ''' </summary>
    ''' <returns></returns>
    Private Function SalesConfirmed_SendMail() As Boolean
        Dim blnFlag As Boolean = False
        Dim sMsg As String = ""
        Dim sMailCC As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try

            dtCustomer = oCustomer.QueryUser(Me.UI_CUNO.Text.Trim(), Me.UI_ACCOUNTID.Text.Trim(), "")
            If dtCustomer.Rows.Count > 0 Then
                Dim mailTo As String = Me.UI_RMA_MAIL.Text.Trim()

                If dtCustomer.Rows(0)("CU_NO").ToString().Trim() = "CA010" Or dtCustomer.Rows(0)("CU_NO").ToString().Trim() = "BG53" Or dtCustomer.Rows(0)("CU_NO").ToString().Trim() = "R000081" Then
                    mailTo = dtCustomer.Rows(0)("CU_EMAIL").ToString().Trim()
                End If

                'Dim repair_center As Common.enmRepairCenter = [Enum].Parse(GetType(Common.enmRepairCenter), "AU_LAPTOP_KINGS")

                '澳洲維修中心 系統通知信與收件人調整 modify by buck 20250715
                If dtCustomer.Rows(0)("CU_COUNTRYID").ToString().Trim() = GetDescriptionText(enmRepairCenter.AU) Then
                    Dim dtAdminTemp = oAdmin.Query("", "1,2,4")
                    Dim AURepairCenter = From x In dtAdminTemp
                                         Where x.AD_REPAIRCENTER = GetDescriptionText(enmRepairCenter.CL_AU_Service_Center)
                                         Select x

                    AURepairCenter.ToList().ForEach(Sub(item)
                                                        mailTo += "," & item.AD_EMAIL
                                                    End Sub)
                End If

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
                    If dtAdmin.Rows.Count > 0 Then
                        MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                        SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '

                        If MailSales.Trim() <> "" Then
                            If sMailCC.Trim() <> "" Then
                                sMailCC = sMailCC & ","
                            End If
                            sMailCC = sMailCC & MailSales.Trim()
                        End If

                    End If
                End If

                Dim MailAssistant As String = ""
                Dim AssistantName As String = ""
                If CU_ASSISTANTID.Trim() <> "" Then
                    dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                    If dtAdmin.Rows.Count > 0 Then
                        MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                        AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '

                        If MailAssistant.Trim() <> "" Then
                            If sMailCC.Trim() <> "" Then
                                sMailCC = sMailCC & ","
                            End If
                            sMailCC = sMailCC & MailAssistant.Trim()
                        End If

                    End If
                End If

                Dim oLoginInfo As New ctlLoginInfo
                Dim LanguageID As String = oLoginInfo.getLanguageID("Customer", Me.UI_ACCOUNTID.Text.Trim())

                Dim sSubject As String = _oLanguage.getMailText("Mail", "023", ctlLanguage.eumType.Mail, LanguageID)
                Dim sBody As String = _oLanguage.getMailText("Mail", "024", ctlLanguage.eumType.Mail, LanguageID)
                Dim sEmailURL As String = _oLanguage.getMailText("Mail", "005", ctlLanguage.eumType.Mail, LanguageID)

                If mailTo.Trim() <> "" Then
                    If sMailCC.Trim() <> "" Then
                        mailTo = mailTo & "," & sMailCC
                    End If

                    sSubject = sSubject.Replace("[$RMA No$]", Me.RMA_NO.Trim())

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

                    sBody = sBody.Replace("[$Customer User Name$]", Me.UI_RMA_APPLICANT.Text)
                    sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                    sBody = sBody.Replace("[$RMA Request No$]", Me.RMA_NO.Trim())
                    sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                    Dim oAttachmentFile As New Collection
                    oAttachmentFile.Add(Me.ViewState("_AttachmentFile").ToString())
                    'addLog("Send mail Quoting" & mailTo)
                    'oMail.SendMail(sSubject, sBody, "isaac.yeh@cipherlab.com.tw,hugh.wang@cipherlab.com.tw", "", oAttachmentFile)

                    'mailTo = "ted@icat-tech.com.tw"
                    'mailTo = mailTo & "," & "yijen.lo@cipherlab.com.tw"
                    Dim ctlRMARES As New ctlRMA.Requested
                    Dim dtEndUseres As DataTable
                    dtEndUseres = ctlRMARES.IsEndUser(Me.UI_CUNO.Text.Trim())

                    '如果是enduser
                    If (dtEndUseres.Rows.Count > 0) Then
                        '新增銀行帳 20230710
                        Dim folderPath As String = Server.MapPath("~/FILE/Sample/")
                        Dim account As New Collection()
                        oAttachmentFile.Add(folderPath + "Transfer_account.pdf")
                        sBody = sBody.Replace(" Please confirm this quotation soon.", " You can choose between Paypal or a bank transfer to make the payment. Once we have confirmed your payment, we will proceed to the next stage.")
                    End If

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        _MailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    blnFlag = oMail.SendMail(sSubject, sBody, mailTo, _MailCC, oAttachmentFile)
                    addLog("Send mail Quoting to isaac:" & mailTo)

                End If
            End If

        Catch ex As Exception
            blnFlag = False
            sMsg = ex.Message

        Finally
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 設定RMADID
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMADID</returns>
    ''' <remarks></remarks>
    Public Property RMADID() As String
        Get
            Return Me.ViewState("_RMADID").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMADID") = nNewValue
        End Set
    End Property
    ''' <summary>
    ''' 設定RMA_NO
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 RMA_NO</returns>
    ''' <remarks></remarks>
    Public Property RMA_NO() As String
        Get
            Return Me.ViewState("_RMA_NO").ToString().Trim()
        End Get

        Set(ByVal nNewValue As String)
            Me.ViewState("_RMA_NO") = nNewValue
        End Set
    End Property


#Region "產生 維修報價 檔案"
    Private Sub qryRepairQuotaionRPT()
        Dim i As Integer = 0
        Dim ctlClient As New ctlRMA.Client

        Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
        Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
        Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable

        Dim sRMANO As String = Me.RMA_NO.Trim()

        dtClient_SalesQuoted_Head = ctlClient.QryClient_SalesQuoted_Head(sRMANO)
        dtClient_SalesQuoted_SN = ctlClient.QryClient_SalesQuoted_SN_001(sRMANO)
        dtClient_SalesQuoted_Part = ctlClient.QryClient_SalesQuoted_Part(sRMANO)

        Dim sRMA_COMPNO As String = ""
        If dtClient_SalesQuoted_Head.Rows.Count > 0 Then
            sRMA_COMPNO = dtClient_SalesQuoted_Head.Rows(0)("RMA_COMPNO").ToString().Trim()
        End If

        Dim dvPart As DataView = dtClient_SalesQuoted_Part.DefaultView
        For i = 0 To dtClient_SalesQuoted_SN.Rows.Count - 1
            Dim j As Integer = 0
            Dim drSN As RmaDTO.VWRPTCLIENT_SALESQUOTED_SNRow = dtClient_SalesQuoted_SN.Rows(i)
            drSN.SEQSN = (i + 1).ToString()

            '非正常使用: 0.No, 1.Yes
            Dim RMARQ_IMPROPERUSAGE_Text As String = "N"
            If drSN.RMARQ_IMPROPERUSAGE = 1 Then
                RMARQ_IMPROPERUSAGE_Text = "Y"
            End If
            drSN.RMARQ_IMPROPERUSAGE_TEXT = RMARQ_IMPROPERUSAGE_Text


            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            Dim RMAD_ISWARRANTY_Text As String = "N"
            If drSN.IsRMAD_ISWARRANTYNull = False Then
                If drSN.RMAD_ISWARRANTY = 1 Then
                    RMAD_ISWARRANTY_Text = "Y"
                End If
            End If
            drSN.RMAD_ISWARRANTY_TEXT = RMAD_ISWARRANTY_Text

            '是否要維修: 1.Accept, 2.Reject
            Dim RMARQ_Reject As String = ""
            If drSN.IsRMARQ_ACCEPTNull = False Then
                If drSN.RMARQ_ACCEPT = 2 Then
                    RMARQ_Reject = "Y"
                End If
            End If
            drSN.RMARQ_ACCEPT_TEXT = RMARQ_Reject


            dvPart.RowFilter = "RMAD_ID='" & drSN.RMAD_ID.Trim() & "'"
            For j = 0 To dvPart.Count - 1
                dvPart(j)("SEQPART") = (j + 1).ToString()
            Next
        Next
        dvPart.RowFilter = ""


        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtClient_SalesQuoted_Head)
        oDsReport.Tables.Add(dtClient_SalesQuoted_SN)
        oDsReport.Tables.Add(dtClient_SalesQuoted_Part)

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMANO("Customer", sRMANO)

        Call Print(oDsReport, sRMA_COMPNO, LanguageID)

    End Sub

    Private Sub Print(ByVal oDsReport As DataSet, ByVal sRMA_COMPNO As String, ByVal sLanguageID As String)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sRMA_COMPNO.ToLower() = "CL_CHINA".ToLower() Then
            ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_CHINA.rpt"))
        Else
            If sLanguageID = "003" Then
                ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_jp.rpt"))
            Else
                '新增paypal 打印文件特別備註 2023/7/5 ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                Dim oRequested As New ctlRMA.Requested
                Dim dt As DataTable = oRequested.IsEndUser(Me.UI_ACCOUNTID.Text.Trim(), "X0091")
                If (dt.Rows.Count > 0) Then
                    ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted_X0091.rpt"))
                Else
                    ReportDoc.Load(Server.MapPath("Report\rptClient_SalesQuoted.rpt"))
                End If
            End If
        End If

        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        'oCommon.OpenPdf(Me, _ReportToPDF) '修改報價完只要匯出PDF不用開啟報表 by buck modify 20251103
        Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
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

    'Public Sub ConfigureExportToPdf()
    '    Me.ViewState("_AttachmentFile") = _Reoprt_FilePath & _ReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = Me.ViewState("_AttachmentFile").ToString().Trim()
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()
    'End Sub

    Private Sub addLog(ByVal LogValue As String)
        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_LOG"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
            oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try
    End Sub

#End Region

    Public Function GetData(ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String) As DataTable
        If OperationCenter <> "CL_CHINA" Then
            OperationCenter = "CLHQ"
        End If

        Dim oExport As New ctlRMA.Export
        'Dim sEWEnd As String = oExport.getWarranty(RMAD_SERIALNO, "")
        Dim sEWEnd As String = oExport.getMaxWarranty(RMAD_SERIALNO, "", UI_lblRepairNo.Text.Trim())
        Dim sCWEnd As String = oExport.getWarrantyCW(RMAD_SERIALNO, "")
        Dim sSWEnd As String = oExport.getWarrantySW(RMAD_SERIALNO, "")
        Dim sWarDate As String = oExport.getWarrantyStart(RMAD_SERIALNO)
        Dim sWarVersion As String = String.Empty

        Dim sWar_id As String = ""
        Dim oWarranty As New ctlWarranty
        Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(RMAD_SERIALNO)
        If dtPur.Rows.Count > 0 Then
            sWar_id = dtPur.Rows(0)("wati_ver").ToString()
            'sWarDate = DateTime.Parse(dtPur.Rows(0)("waty_date").ToString()).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
            If sCWEnd.Trim() <> "" Then
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "CW", "", "Y", "WAR_VERSION")
            Else
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "EW", "", "Y", "WAR_VERSION")
            End If

            If dtEwVer.Rows.Count > 0 Then
                If dtEwVer.Rows(0)("WAR_VERSION").ToString().Trim().Equals("0") Then
                    sWar_id = dtEwVer.Rows(0)("WAR_ID").ToString()
                End If

                '20200217 wisely modify 抓訂單細目的版本
                sWarVersion = oWarranty.QueryWARVERSION(RMAD_SERIALNO)
                If sWarVersion <> String.Empty Then
                    Dim find_rows As DataRow() = dtEwVer.Select("WAR_VERSION='" + sWarVersion + "'")
                    If find_rows.Length > 0 Then
                        sWar_id = find_rows(0)("WAR_ID").ToString()
                    End If
                End If

            End If
        End If
        If sWarDate <> "" Then
            sWarDate = DateTime.Parse(sWarDate).ToString("yyyy/MM/dd")
        End If


        If sWar_id = "" Then
            sWar_id = "123$321"
        End If

        Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        'sWar_id = "9700CW00125"
        dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")
        dtWarrParts.Columns.Add("PODate")
        dtWarrParts.Columns.Add("WarrEndDate")

        Dim i As Integer
        For i = 0 To dtWarrParts.Rows.Count - 1
            dtWarrParts.Rows(i)("PODate") = sWarDate
            If sWarDate <> "" Then
                dtWarrParts.Rows(i)("WarrEndDate") = DateTime.Parse(sWarDate).AddMonths(Double.Parse(dtWarrParts.Rows(i)("WAP_MON").ToString()) + Double.Parse(dtWarrParts.Rows(i)("WAP_EMON").ToString())).AddDays(-1).ToString("yyyy/MM/dd")
            End If
        Next

        Return dtWarrParts
    End Function

    Private Sub QueryData(ByVal EXPORT_SERIALNO_String As String)
        Try
            total_loss_Panel.Visible = False


            If EXPORT_SERIALNO_String <> "" Then

                Me.Shipped_Quantity_Label.Text = _oLanguage.getText("RMA_Repair", "966", ctlLanguage.eumType.Tag)
                Me.Accumulated_Repair_Quantity_Label.Text = _oLanguage.getText("RMA_Repair", "967", ctlLanguage.eumType.Tag)
                Me.Repair_Rate_Label.Text = _oLanguage.getText("RMA_Repair", "968", ctlLanguage.eumType.Tag)


                'total loss
                total_loss_Panel.Visible = True
                Dim ctlExtend_count As New ctlExtend
                Dim myDataTable As New DataTable
                myDataTable = ctlExtend_count.QryRMA_RMADETAIL(EXPORT_SERIALNO_String)

                Dim total As String = ""
                Dim Exceed As String = ""

                Exceed = myDataTable.Rows.Count.ToString().Trim()

                For Each item As DataRow In myDataTable.Rows
                    total = item("TOTAL").ToString().Trim()
                Next

                Shipping_Lab.Text = total
                Repair_Lab.Text = Exceed

                Dim percentage As Double = (Convert.ToInt32(Exceed) / Convert.ToInt32(total)) * 100

                Exceed_Lab.Text = percentage.ToString() & "%"

            End If


        Catch ex As Exception

        End Try


        If Me.Shipping_Lab.Text.Trim() = "" Then
            total_loss_Panel.Visible = False
        Else
            total_loss_Panel.Visible = True

        End If

    End Sub

    Protected Sub rblBI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblBI.SelectedIndexChanged

        If rblBI.SelectedItem.Value = 1 Then

            plBI.Visible = True

        Else
            plBI.Visible = False
        End If

    End Sub

    '需求新增:BI保固 By buck Add 20250902 begin
    Protected Sub UI_opgApplyBatteryInsurance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UI_opgApplyBatteryInsurance.SelectedIndexChanged

        fdt_Warranty.Visible = False
        UI_lblWarrantyBI.Visible = False
        If UI_opgApplyBatteryInsurance.SelectedItem.Value = 1 Then
            fdt_Warranty.Visible = True
            UI_lblWarrantyBI.Visible = True
        End If
    End Sub
    '需求新增:BI保固 By buck Add 20250902 end
End Class

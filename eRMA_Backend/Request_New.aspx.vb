Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Partial Class Request_New
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _RedirectURL As String = "Client_Worklist.aspx"
    Dim _Address As String = ""
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")

    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_Upload_FilePath")
    Dim _Requested_ExcelSample_VisualPath As String = ConfigurationSettings.AppSettings("Requested_ExcelSample_VisualPath")
    Dim _Requested_Default_FarFarcNO As String = ConfigurationSettings.AppSettings("Requested_Default_FarFarcNO")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _RepairEmailCC As String = ConfigurationSettings.AppSettings("RepairEmailCC")
    Dim _RequestNew_JP_BYTE_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_JP_BYTE_EmailCC")
    Dim _RequestNew_US_CL_MPLUS_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_US_CL_MPLUS_EmailCC")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                _RedirectURL = "Client_FlowCase01_Worklist.aspx"
                Exit For
            End If
        Next

        '用客戶申請時表單的維修中心判斷是否要執行 flow case 02
        Dim arrRepairNo_flowCase02() As String = _RepairNo_flowCase02.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase02.Length - 1
            If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase02(i).ToString().Trim()) <> -1 Then
                _RedirectURL = "Client_FlowCase01_Worklist.aspx"
                Exit For
            End If
        Next


        If Me.IsPostBack = False Then
            Call setControls()
            Call getModelData()
            Session("_dtRMADetail") = Nothing
            Session("_PickCostomer_Submit") = False

            Me.ViewState("_dtAddress") = Nothing
            Me.ViewState("_eumCommand") = eumCommand.AddNew
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()

                If Me.UI_lblPreviousPage_RMAID.Text.Trim() <> "" And Me.UI_lblPreviousPage_RMANO.Text.Trim() <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If
            End If


            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    Call QueryData_Head()

                Case eumCommand.UPDATE
                    Call QueryData_Head()
                    Call QueryData_RMA()
                    Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
            End Select
            Call cmdButton_visible()

            '20210721 wisely add  NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
            If (UI_lblRepairCenterValue.Text = "NZ_PB_TECH" Or UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS") Then
                uiLblDesc01.Visible = False
            Else
                uiLblDesc01.Visible = True
            End If

        End If


    End Sub
#End Region

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Me.IsPostBack = True Then
            'Repair Bom 查回來後新增
            If Session("_PickCostomer_Submit") = True Then
                Call QueryData_Head()
            End If
            Session("_PickCostomer_Submit") = False
        End If

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_cmdExcelFile.NavigateUrl = "https://e-rma.cipherlab.com.tw" & _Requested_ExcelSample_VisualPath & "Sample.csv"

        Dim sClientID As String = Me.UI_cmdPick.ClientID & "," & Me.UI_cmdAdd.ClientID
        'Me.ucProgressStatus.NotpostBackElement = sClientID

        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(False, Me.UI_cboRepairCenter, sRepairText)
        Dim oListItem As New ListItem
        Me.UI_cboRepairCenter.SelectedValue = "-1"
        oListItem = Me.UI_cboRepairCenter.SelectedItem
        Me.UI_cboRepairCenter.Items.Remove(oListItem)



        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblUserID.Text = _oLanguage.getText("RMA", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblProductInformation.Text = _oLanguage.getText("RMA", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblPleaseTittle.Text = _oLanguage.getText("RMA", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblMail.Text = _oLanguage.getText("RMA", "044", ctlLanguage.eumType.Tag)
        Me.UI_lblFile.Text = _oLanguage.getText("RMA", "024", ctlLanguage.eumType.Tag)

        Me.UI_cmdFileAdd.Text = _oLanguage.getText("RMA", "211", ctlLanguage.eumType.Command)
        Me.UI_cmdExcelFile.Text = _oLanguage.getText("RMA", "212", ctlLanguage.eumType.Tag)
        Me.UI_UploadFileDesc.Text = _oLanguage.getText("RMA", "186", ctlLanguage.eumType.Tag)
        Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        Me.uiLblDesc01.Text = _oLanguage.getText("RMA", "411", ctlLanguage.eumType.Tag)


        Me.rfv_txtTEL.ErrorMessage = _oLanguage.getText("RMA", "207", ctlLanguage.eumType.Validator)
        Me.rfv_txtAdress.ErrorMessage = _oLanguage.getText("RMA", "047", ctlLanguage.eumType.Validator)
        Me.rfv_txtApplicant.ErrorMessage = _oLanguage.getText("RMA", "048", ctlLanguage.eumType.Validator)
        'Me.rfv_txtModelNo.ErrorMessage = _oLanguage.getText("RMA", "191", ctlLanguage.eumType.Validator)
        Me.cvModelNo_Serial.ErrorMessage = _oLanguage.getText("RMA", "210", ctlLanguage.eumType.Validator)
        Me.cvFileUpLoad.ErrorMessage = _oLanguage.getText("RMA", "219", ctlLanguage.eumType.Validator)
        Me.rfv_txtAccountID.ErrorMessage = _oLanguage.getText("RMA", "220", ctlLanguage.eumType.Validator)

        Me.revEMail_Empty.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)
        'Me.revEMail.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)


        Me.UI_cmdCust_Search.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdAdressPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)

        Me.UI_cmdTmpSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdModify.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)

        Me.UI_lblEUCompany.Text = _oLanguage.getText("RMA", "459", ctlLanguage.eumType.Tag)
        Me.UI_lblEUTel.Text = _oLanguage.getText("RMA", "455", ctlLanguage.eumType.Tag)
        Me.UI_lblEUName.Text = _oLanguage.getText("RMA", "456", ctlLanguage.eumType.Tag)
        Me.UI_lblEUMail.Text = _oLanguage.getText("RMA", "457", ctlLanguage.eumType.Tag)
        Me.UI_lblEUAddress.Text = _oLanguage.getText("RMA", "458", ctlLanguage.eumType.Tag)
        '設定 Enter 鍵觸發
        Me.UI_txtSerial.Attributes.Add("OnKeypress", "return clickButton(event,'" & Me.UI_cmdAdd.ClientID & "')")

        Me.chkWarrantyMsg.Text = _oLanguage.getText("RMA", "209", ctlLanguage.eumType.Validator)

        Me.lb_PartsRequest.Text = _oLanguage.getText("Transfer", "010", ctlLanguage.eumType.Word)

        If Session("_Comp_Admin").ToString().ToUpper() = "MPLUS" Then
            Me.UI_lblModel.Visible = False
            Me.UI_cboModel.Visible = False
        End If

        '20210708 wisely add 若是 JP_BYTE_MPLUS 不秀補料選項
        'If Session("_RepairID").ToString().Trim() = "JP_BYTE_MPLUS" Then
        '    lb_PartsRequest.Visible = False
        '    UI_PartsRequest.Visible = False
        'End If



        Me.UI_lblAccountIDText.Visible = True
        Me.UI_txtAccountIDText.Visible = False
        Me.UI_txtAccountIDText.ReadOnly = False
        Me.UI_cmdCust_Search.Visible = False
        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            Me.UI_lblAccountIDText.Visible = False

            Me.UI_txtAccountIDText.Visible = True
            Me.UI_txtAccountIDText.ReadOnly = True
            Me.UI_cmdCust_Search.Visible = True
        End If

    End Sub

    ''' <summary>
    ''' 控制按鈕的出現 時機
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cmdButton_visible()
        Me.UI_cmdTmpSave.Visible = False
        Me.UI_cmdSubmit.Visible = False
        Me.UI_cmdModify.Visible = False
        Me.UI_cmdCancel.Visible = False

        If Session("_dtRMADetail") Is Nothing Then
            Exit Sub
        End If

        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        dvRMADetail.RowFilter = "RMAD_MARK=0"
        If dvRMADetail.Count > 0 Then
            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    Me.UI_cmdTmpSave.Visible = True
                    Me.UI_cmdSubmit.Visible = True

                Case eumCommand.UPDATE
                    Dim RMA_STATUS As Integer = Convert.ToInt16(Me.UI_RMA_STATUS.Text.Trim())
                    Select Case RMA_STATUS
                        Case 0
                            Me.UI_cmdTmpSave.Visible = True
                            Me.UI_cmdSubmit.Visible = True
                            Me.UI_cmdCancel.Visible = True

                        Case 10
                            'Me.UI_cmdModify.Visible = True
                            Me.UI_cmdSubmit.Visible = True
                    End Select
            End Select
        End If

        dvRMADetail.RowFilter = ""
    End Sub

    ''' <summary>
    ''' 取得Model 的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getModelData()
        Dim i As Integer = 0
        Dim oListItem As ListItem

        Dim oWarranty As New ctlWarranty
        Dim dtModel As New WarrantyDTO.vwPrdGroupDataTable
        dtModel = oWarranty.QueryPrdGroup("", "")

        Me.UI_cboModel.Items.Clear()
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "208", ctlLanguage.eumType.Tag)
        oListItem.Value = ""
        Me.UI_cboModel.Items.Add(oListItem)

        For i = 0 To dtModel.Rows.Count - 1
            Dim tGroupNO As String
            tGroupNO = dtModel.Rows(i).Item("GroupNo").ToString()
            '依需求單 : 因為部分EOL機種已過保修期 . 麻煩請協助將RMA系統客戶申請開單畫面由Model下拉式選單移除EOL(Model) 
            'MODI BY Angel ON 202160829
            If tGroupNO <> "1200" And tGroupNO <> "0520" And tGroupNO <> "07AC" And tGroupNO <> "0711" And tGroupNO <> "93AC" And tGroupNO <> "9300" And tGroupNO <> "94AC" And tGroupNO <> "9400" And tGroupNO <> "95AC" And tGroupNO <> "9500" Then
                oListItem = New ListItem
                oListItem.Text = dtModel.Rows(i).Item("GroupName").ToString().Trim()
                oListItem.Value = dtModel.Rows(i).Item("GroupNo").ToString().Trim()
                If tGroupNO = "102X" Then
                    oListItem.Text = "1023 / 1023 Reader"
                End If
                Me.UI_cboModel.Items.Add(oListItem)
            End If

        Next

    End Sub

    ''' <summary>
    ''' 新增 RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_Head()
        Dim i As Integer = 0
        '' ''Dim oListItem As ListItem
        Dim dtAddress As New DataTable
        dtAddress.Columns.Add("CU_Address")


        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustome = oCustomer.QueryByCompany(Me.UI_txtAccountIDText.Text.Trim)
            End If
        Else
            dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
        End If

        Dim sTel As String = ""
        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            Me.UI_lblAccountIDText.Text = dr.CU_NO.ToString().Trim()
            Me.UI_lblAccountNameText.Text = dr.CU_NAME.ToString().Trim()

            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
            Me.UI_lblRepairCenterValue.Text = dr.COMP_NO.ToString().Trim()
            Me.UI_cboRepairCenter.SelectedValue = dr.COMP_NO.ToString().Trim()

            Me.UI_lblRepairCenterText.Visible = False
            Me.UI_cboRepairCenter.Visible = False
            If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                Me.UI_cboRepairCenter.Visible = True
            Else
                If dr.CU_ISCHOICE = "1" Then
                    Me.UI_cboRepairCenter.Visible = True
                Else
                    Me.UI_lblRepairCenterText.Visible = True
                End If
            End If


            If dr.IsCU_TELNull = False Then sTel = dr.CU_TEL.ToString().Trim()
            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_txtApplicant.Text = dr.CU_CONTACTPERSON.ToString().Trim()
            If dr.IsCU_EMAILNull = False Then Me.UI_txtMail.Text = dr.CU_EMAIL.ToString().Trim()

            If dr.IsCU_ADDRESS1Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS1.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS2Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS2.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS3Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS3.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS4Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS4.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            '20211223 wisely 若為Enduer 不允許輸入EndUser 欄位
            Dim oRequested As New ctlRMA.Requested
            Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblAccountIDText.Text.Trim())
            If (dt.Rows.Count > 0) Then
                trCompany.Visible = False
                trEUName.Visible = False
                trEUAddress.Visible = False
            Else
                trCompany.Visible = True
                trEUName.Visible = True
                trEUAddress.Visible = True
            End If

        End If


        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustomerUser = oCustomerUser.QueryUser(Me.UI_txtAccountIDText.Text.Trim, "")
            End If
        Else
            dtCustomerUser = oCustomerUser.QueryUser(Session("_CustomerID").ToString(), Session("_UserID").ToString())
        End If

        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            Me.UI_lblUserIDText.Text = dr.CUUS_ACCOUNTID.ToString().Trim()
            If dr.IsCUUS_TELNull = False Then sTel = dr.CUUS_TEL.ToString().Trim()

            If dr.IsCUUS_ADDRESSNull = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CUUS_ADDRESS.Trim()
                dtAddress.Rows.Add(oRow)
            End If


            '' ''If dr.IsCUUS_ADDRESSNull = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CUUS_ADDRESS.Trim()
            '' ''    oListItem.Value = dr.CUUS_ADDRESS.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If
        Next


        Me.UI_txtTel.Text = sTel.Trim()
        If dtAddress.Rows.Count > 0 And Convert.ToInt16(Me.ViewState("_eumCommand")) = eumCommand.AddNew Then
            Me.UI_txtAddress.Text = dtAddress.Rows(0)("CU_Address").ToString().Trim()
        End If
        Me.ViewState("_dtAddress") = dtAddress
    End Sub

    ''' <summary>
    ''' 取得要修改 RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMA()
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        Dim oRMA As New ctlRMA.Requested
        Dim oRMADetail As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable

        Dim sRMAID As String = Me.UI_lblPreviousPage_RMAID.Text.ToString().Trim()
        Dim sRMANO As String = Me.UI_lblPreviousPage_RMANO.Text.Trim()

        dtRMA = oRMA.QueryByRMAHead(sRMANO)
        If dtRMA.Rows.Count > 0 Then
            Me.UI_lblUserIDText.Text = dtRMA.Rows(0)("RMA_ACCOUNTID").ToString().Trim()
            Me.UI_txtApplicant.Text = dtRMA.Rows(0)("RMA_APPLICANT").ToString().Trim()
            Me.UI_txtTel.Text = dtRMA.Rows(0)("RMA_TEL").ToString().Trim()
            Me.UI_txtMail.Text = dtRMA.Rows(0)("RMA_MAIL").ToString().Trim()
            '' ''Me.UI_cboAddress.SelectedValue = dtRMA.Rows(0)("RMA_ADDRESS").ToString().Trim()

            dtCompany = oCompany.QueryByPrimaryKey(dtRMA.Rows(0)("RMA_COMPNO").ToString().Trim())
            If dtCompany.Rows.Count > 0 Then
                Me.UI_lblRepairCenterText.Text = dtCompany.Rows(0)("COMP_NAME").ToString().Trim()
            End If
            Me.UI_lblRepairCenterValue.Text = dtRMA.Rows(0)("RMA_COMPNO").ToString().Trim()
            Me.UI_cboRepairCenter.SelectedValue = dtRMA.Rows(0)("RMA_COMPNO").ToString().Trim()

            Me.UI_txtAddress.Text = dtRMA.Rows(0)("RMA_ADDRESS").ToString().Trim()
            Me.UI_RMA_STATUS.Text = dtRMA.Rows(0)("RMA_STATUS").ToString().Trim()
            Me.UI_txtRemark.Text = dtRMA.Rows(0)("RMA_Remark").ToString().Trim()

            Me.UI_txtEUCompany.Text = dtRMA.Rows(0)("RMA_EUCOMPANY").ToString().Trim()
            Me.UI_txtEUName.Text = dtRMA.Rows(0)("RMA_EUNAME").ToString().Trim()
            Me.UI_txtEUAddress.Text = dtRMA.Rows(0)("RMA_EUADDRESS").ToString().Trim()
            Me.UI_txtEUTel.Text = dtRMA.Rows(0)("RMA_EUTEL").ToString().Trim()
            Me.UI_txtEUMail.Text = dtRMA.Rows(0)("RMA_EUMAIL").ToString().Trim()
        End If

        dtRMADetail = oRMA.QueryByRMADetail(sRMANO, "")
        Call ArrangementData(dtRMADetail)
        Session("_dtRMADetail") = dtRMADetail

        Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
        Call RMADetail_DataBind(iPageIndex)
    End Sub

    Private Sub RMADetail_DataBind(ByVal iPageIndex As Integer)
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        dtRMADetail = Session("_dtRMADetail")

        Call ArrangementData(dtRMADetail)
        Call cmdButton_visible()

        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        dvRMADetail.RowFilter = "RMAD_MARK=0"

        Me.UI_dvRMADetail.PageSize = _PageSize
        Me.UI_dvRMADetail.PageIndex = iPageIndex
        Me.UI_dvRMADetail.DataSource = dvRMADetail
        Me.UI_dvRMADetail.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then
            dtRMADetail.Columns.Add("SeqID")
            dtRMADetail.Columns.Add("CWEndWarr")
            dtRMADetail.Columns.Add("SWEndWarr")
        End If

        For i = 0 To dtRMADetail.Rows.Count - 1
            dtRMADetail.Rows(i)("SeqID") = i + 1

            'dtRMADetail.Rows(i)("RMAD_sWARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            'If IsDate(dtRMADetail.Rows(i)("RMAD_WARRANTY")) = True Then
            'dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(dtRMADetail.Rows(i)("RMAD_WARRANTY")).ToShortDateString
            'End If

            Dim oExport As New ctlRMA.Export
            'Dim sEWEnd As String = oExport.getWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            Dim sEWEnd As String = ""

            If Me.UI_cboRepairCenter.Visible = False Then
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
            Else
                sEWEnd = oExport.getMaxWarranty(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
            End If
            If sEWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(sEWEnd).ToShortDateString()
            End If

            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If

            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If
        Next
    End Sub

    Protected Sub UI_dvRMADetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRMADetail.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Transfer", "011", ctlLanguage.eumType.Word)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)

            'EW
            e.Row.Cells(5).Text = _oLanguage.getText("Transfer", "029", ctlLanguage.eumType.Word)
            'CW
            e.Row.Cells(6).Text = _oLanguage.getText("Transfer", "030", ctlLanguage.eumType.Word)

            e.Row.Cells(7).Text = _oLanguage.getText("Transfer", "012", ctlLanguage.eumType.Word)
            e.Row.Cells(8).Text = _oLanguage.getText("RMA", "016", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("RMA", "017", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")

            Dim UI_ISFILL As Label = e.Row.FindControl("UI_ISFILL")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdWarrDetail As ImageButton = e.Row.FindControl("UI_cmdWarrDetail")

            '是否已填寫問題:0.否, 1.是
            UI_cmdEdit.Text = _oLanguage.getText("Common", "025", ctlLanguage.eumType.Command)
            If UI_ISFILL.Text.Trim() = "1" Then
                UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(5).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(5).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(5).ForeColor = Drawing.Color.Red
                    'UI_cmdWarrDetail.Visible = False
                End If
            End If

            If IsDate(e.Row.Cells(6).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(6).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(6).ForeColor = Drawing.Color.Red
                    'UI_cmdWarrDetail.Visible = False
                End If
            End If

            'If IsDate(e.Row.Cells(7).Text.Trim()) = True Then
            '    If Convert.ToDateTime(e.Row.Cells(7).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
            '        e.Row.Cells(7).ForeColor = Drawing.Color.Red
            '    End If
            'End If
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

    Protected Sub UI_dvRMADetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRMADetail.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtRMADetail") Is Nothing Then
            Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")

            Call RMADetail_DataBind(iPageIndex)
        Else
            Call RMADetail_DataBind(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvRMADetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRMADetail.RowCommand
        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow
        Dim oExport As New ctlRMA.Export

        If e.CommandName = "cmdEdit" Then
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            'Me.UcPopProblem.show(UI_RMADID.Text.Trim(), True, "")
            Me.UcPopProblem.show(UI_RMADID.Text.Trim(), True, UI_lblRepairCenterValue.Text.Trim(), UI_lblUserIDText.Text.Trim())
        End If

        If e.CommandName = "cmdWarrDetail" Then
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMAD_MODELNO As Label = row.FindControl("UI_RMAD_MODELNO")
            Dim UI_RMAD_SERIALNO As Label = row.FindControl("UI_RMAD_SERIALNO")

            Dim sModelNo As String = oExport.getCModelNo(UI_RMAD_MODELNO.Text.Trim(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
            'Me.UcWarrantyPartsView.show(True, UI_RMAD_SERIALNO.Text.Trim(), UI_RMAD_MODELNO.Text.Trim(), UI_cboRepairCenter.SelectedValue)
            Me.UcWarrantyPartsView.show(True, UI_RMAD_SERIALNO.Text.Trim(), sModelNo, UI_lblRepairCenterValue.Text.Trim())
        End If

        If e.CommandName = "cmdDel" Then
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Call Delete(UI_RMADID.Text.ToString())
        End If
    End Sub

    ''' <summary>
    ''' 查詢客戶基本資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdCust_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("_PickCostomer_Submit") = False
        Me.ucCustomer_pick.showByNewRequest(True)
    End Sub

    ''' <summary>
    ''' 挑選Adress資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdressPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdressPick.Click
        Dim dtAddress As DataTable = Me.ViewState("_dtAddress")
        Me.ucCustAddress.show(dtAddress, True)
    End Sub

    ''' <summary>
    ''' 序號確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirm.Click
        Dim i As Integer = 0
        Dim oExport As New ctlRMA.Export

        Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim().ToUpper())
        If sModelNo.Trim() = "" Then
            Me.UI_cboModel.SelectedValue = "OTHER"

        Else
            Me.UI_cboModel.SelectedValue = "OTHER"
            For i = 0 To Me.UI_cboModel.Items.Count - 1
                If Me.UI_cboModel.Items(i).Value.ToLower().Trim() = sModelNo.ToLower().Trim() Then
                    Me.UI_cboModel.SelectedValue = sModelNo
                    Exit For
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' 查詢Model的資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdPick.Click
        'Dim sModel As String = Me.UI_txtModel.Text.ToString().Trim()
        'Me.ucModel.show(sModel, True)
    End Sub

    ''' <summary>
    ''' 新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd.Click
        Dim oExport As New ctlRMA.Export
        Dim oRequested As New ctlRMA.Requested
        Dim ctlReport As New ctlReport
        Dim sMessage As String = ""
        Dim retval As Boolean = True

        Try
            '20180424 Isaac Add 出貨的產品必須由原出貨的顧客送回，若不在Export資料表內一樣可送修
            'Me.UI_txtSerial.Text = Me.UI_txtSerial.Text.Trim().ToUpper() '轉大寫
            'Dim dtExport As New RmaDTO.ExportDataTable
            'dtExport = oRequested.QueryByExport(Me.UI_txtSerial.Text)

            'If dtExport.Rows.Count() > 0 Then
            '    Dim bool_Cust As Boolean = False
            '    For Each dr As DataRow In dtExport
            '        Dim CustNo As String = dr("EXPORT_CUSTNO").ToString()
            '        If Session("_CustomerID").ToString().ToUpper().Trim() = CustNo Then
            '            bool_Cust = True
            '            Exit For
            '        End If
            '    Next

            '    If bool_Cust = False Then
            '        Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator) 'Wait
            '        Throw New ArgumentException(sErrorMsg & "1")
            '    End If
            'End If

            If UI_txtSerial.Text = "" Then
                Dim sErrorMsg As String = _oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator) 'Wait
                Throw New ArgumentException(sErrorMsg)
            End If

            '判斷是否為零件序號
            UI_txtSerialParts.Text = ""
            Dim sProductSn As String = ""

            Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
            'dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), Me.UI_txtSerial.Text.Trim().ToUpper())
            dtReport = ctlReport.QueryRMAWarranty("", Me.UI_txtSerial.Text.Trim().ToUpper())
            If dtReport.Rows.Count > 0 Then
                sProductSn = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
            End If
            'Throw New ArgumentException(sProductSn)
            If sProductSn <> "" Then
                If sProductSn <> Me.UI_txtSerial.Text.Trim().ToUpper() Then
                    UI_txtSerialParts.Text = Me.UI_txtSerial.Text.Trim().ToUpper()
                    Me.UI_txtSerial.Text = sProductSn
                Else
                    sProductSn = ""
                End If
            End If

            '20180424 Isaac Add EndUser只能送修全保
            'Dim bool_EndUser As Boolean = oRequested.IsEndUser(Session("_CustomerID").ToString().Trim())
            'Dim dt As DataTable = oRequested.IsEndUser(Session("_CustomerID").ToString().Trim())
            Dim dt As DataTable = oRequested.IsEndUser(UI_lblUserIDText.Text.ToString().Trim())
            'If bool_EndUser Then
            If (dt.Rows.Count > 0) Then
                '20220303 wisely Enduser 不可以送修零件
                If UI_txtSerialParts.Text.ToString() <> "" Then
                    Me.UI_txtSerial.Text = UI_txtSerialParts.Text
                    Dim sErrorMsg As String = _oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator) 'Wait
                    Throw New ArgumentException(sErrorMsg)
                End If

                'Check ISCW
                Dim sCWEnd As String = oExport.getWarrantyCW(Me.UI_txtSerial.Text.Trim().ToUpper(), "")
                If sCWEnd.Trim() <> "" Then
                    If Convert.ToDateTime(sCWEnd) < Date.Now Then '如果保固日期小於Today不可送修
                        Dim sErrorMsg As String = _oLanguage.getText("RMA", "242", ctlLanguage.eumType.Validator) 'Wait
                        Throw New ArgumentException(sErrorMsg)
                    End If
                Else
                    Dim sErrorMsg As String = _oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator) 'Wait
                    Throw New ArgumentException(sErrorMsg)
                End If
            End If

            Dim dtRMADetail As New RmaDTO.RMADetailDataTable

            If IsNothing(Session("_dtRMADetail")) = True Then
                dtRMADetail = AddRMADetail(dtRMADetail)
            Else
                dtRMADetail = Session("_dtRMADetail")
                dtRMADetail = AddRMADetail(dtRMADetail)
            End If

            Session("_dtRMADetail") = dtRMADetail
            Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
            Call RMADetail_DataBind(iPageIndex)

            'Me.UI_txtModel.Text = ""
            Me.UI_txtSerial.Text = ""
            Me.UI_cboModel.SelectedValue = ""

            Dim oScriptManager As ScriptManager = Me.Master.FindControl("ScriptManager")
            oScriptManager.SetFocus(Me.UI_txtSerial)
        Catch ex As Exception
            sMessage = ex.Message
            retval = False

        Finally

            If retval = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As RmaDTO.RMADetailDataTable
        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim sWarranty As String = "" '_oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        Dim oExport As New ctlRMA.Export

        If Me.UI_txtSerial.Text.Trim() <> "" Then
            dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + Me.UI_txtSerial.Text.Trim().ToUpper() + "' AND RMAD_MARK<>'1'"
            If dtRMADetail.DefaultView.Count > 0 Then
                Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                Throw New ArgumentException(sErrorMsg)
            End If
        End If

        Try
            'If Me.UI_txtSerial.Text.ToString().Trim() <> "" Then
            '    'Dim sWarrantyDate As String = oExport.getWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString())
            '    Dim sWarrantyDate As String = ""
            '    If Me.UI_cboRepairCenter.Visible = False Then
            '        sWarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
            '    Else
            '        sWarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
            '    End If

            '    If sWarrantyDate.Trim() <> "" Then
            '        sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
            '    End If
            'End If


            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""

            '' ''dr.RMAD_MODELNO = Me.UI_txtModel.Text.Trim()
            'dr.RMAD_MODELNO = Me.UI_cboModel.SelectedValue
            If Me.UI_txtSerial.Text.Trim() <> "" Then
                Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim().ToUpper(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
                If sModelNo.Trim() = "" Then
                    dr.RMAD_MODELNO = "OTHER"
                Else
                    dr.RMAD_MODELNO = sModelNo
                End If
            Else
                If Me.UI_cboModel.SelectedValue.Trim() <> "" Then
                    dr.RMAD_MODELNO = Me.UI_cboModel.SelectedValue
                End If
            End If

            'Mod by Isaac 保固日期看抓全保、一般保的最大日期
            If Me.UI_txtSerial.Text.ToString().Trim() <> "" Then
                'Dim WarrantyDate As String = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString())
                Dim WarrantyDate As String = ""
                If Me.UI_cboRepairCenter.Visible = False Then
                    WarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                Else
                    WarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                End If

                If WarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If

                If WarrantyDate.Trim() <> "" Then
                    dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                End If
            End If

            dr.RMAD_SERIALNO = Me.UI_txtSerial.Text.ToString().Trim().ToUpper()
            dr.RMAD_CUSNAME = ""                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty          'Warranty 字串格式
            'If IsDate(sWarranty) = True Then
            'dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
            'End If



            'dr.RMAD_FARFARCNO = "-1"
            'dr.RMAD_FARNO = "-1"
            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = ""
            dr.RMAD_STATUS = 0

            dr.RMAD_FARFARCNO = _oLanguage.getText("RMA", "233", ctlLanguage.eumType.Tag)
            dr.RMAD_FARNO = _oLanguage.getText("RMA", "234", ctlLanguage.eumType.Tag)


            dr.RMAD_PARTSN = UI_txtSerialParts.Text.Trim().ToUpper()
            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try
        Return dtRMADetail
    End Function

    ''' <summary>
    ''' 品項刪除
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal sID As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        dvRMADetail.RowFilter = "RMAD_ID='" & sID.ToString().Trim() & "'"
        If dvRMADetail.Count > 0 Then
            dvRMADetail(0)("RMAD_MARK") = 1
        End If
        dvRMADetail.RowFilter = ""

        Session("_dtRMADetail") = dtRMADetail

        Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageIndex
        RMADetail_DataBind(iPageIndex)
    End Sub

    ''' <summary>
    ''' 儲存之前的檢核 -- 品項
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chkRMA() As Boolean
        Dim retval As Boolean = True
        Dim sMessage As String = ""


        Try
            '20170502 Isaac Add
            If Not (UI_txtEUCompany.Text.Trim() = "") Or Not (UI_txtEUTel.Text.Trim() = "") Or Not (UI_txtEUName.Text.Trim() = "") Or Not (UI_txtEUMail.Text.Trim() = "") Or Not (UI_txtEUAddress.Text.Trim() = "") Then
                If UI_txtEUCompany.Text = "" Then
                    sMessage = _oLanguage.getText("Transfer", "039", ctlLanguage.eumType.Word) '"End User Company Name is null"
                    retval = False
                ElseIf UI_txtEUTel.Text = "" Then
                    sMessage = _oLanguage.getText("Transfer", "040", ctlLanguage.eumType.Word)  '"End User Tel No. is null"
                    retval = False
                ElseIf UI_txtEUName.Text = "" Then
                    sMessage = _oLanguage.getText("Transfer", "041", ctlLanguage.eumType.Word)  '"End User Contact Person is null"
                    retval = False
                ElseIf UI_txtEUMail.Text = "" Then
                    sMessage = _oLanguage.getText("Transfer", "042", ctlLanguage.eumType.Word)  '"End User Email is null"
                    retval = False
                ElseIf UI_txtEUAddress.Text = "" Then
                    sMessage = _oLanguage.getText("Transfer", "043", ctlLanguage.eumType.Word)  '"End User Address is null"
                    retval = False
                End If
            End If

            If Not Session("_dtRMADetail") Is Nothing Then
                Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
                Dim dvRMADetail As DataView = dtRMADetail.DefaultView
                dvRMADetail.RowFilter = "RMAD_MARK=0"
                If dvRMADetail.Count = 0 Then
                    sMessage = _oLanguage.getText("RMA", "194", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If

                '是否已填寫問題:0.否, 1.是
                dvRMADetail.RowFilter = "RMAD_ISFILL=0 and RMAD_MARK=0"
                If dvRMADetail.Count > 0 Then
                    sMessage = _oLanguage.getText("RMA", "195", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If
            End If

        Catch ex As Exception
            sMessage = ex.Message
            retval = False

        Finally

            If retval = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try



        Return retval
    End Function

    ''' <summary>
    ''' 取消 RMA 單維修單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2011/08/04 START</remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdCancel.Click
        'RMAD_STATUS:0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim ctlClient As New ctlRMA.Client
        Dim dtClient As New RmaDTO.Client_CancelDataTable

        Try
            Dim dr As RmaDTO.Client_CancelRow = dtClient.NewClient_CancelRow
            dr.RMA_NO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
            dr.RMA_CLIENTAD = Session("_UserID")
            dr.RMA_CLIENTADNAME = Session("_UserName")
            dr.RMA_CLIENTDATE = Date.Now

            dtClient.Rows.Add(dr)

            Call ctlClient.Client_Cancel(dtClient)
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, _RedirectURL)
            End If
        End Try


    End Sub

    ''' <summary>
    ''' 儲存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdTmpSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdTmpSave.Click
        Dim blnFlag As Boolean = False
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If chkRMA() = True Then
            Dim iStatus As Integer = 0      '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Save(iStatus)
        End If

    End Sub

    ''' <summary>
    ''' 將RMA單Submit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If chkRMA() = True Then
            Dim iStatus As Integer = 10  '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Save(iStatus)
        End If

        '_oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
        'Dim sMsg As String = "Please refer to Warranty Details for valid expiration date"
        'Me.ucMessage.showMessageByAlert(sMsg)

    End Sub

    ''' <summary>
    ''' 將RMA單 Modify
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdModify.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If chkRMA() = True Then
            Dim iStatus As Integer = 10  '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Save(iStatus)
        End If

    End Sub
    '''' <summary>
    '''' 儲存LOG
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub addLog(ByVal NoValue As String, ByVal LogValue As String)
    '    Dim oConn As New ICAT_OracleDAO.Connection
    '    oConn.Open()
    '    Dim oCommand As OracleCommand = oConn.Command
    '    Try
    '        oCommand.CommandText = "SP_ADD_LOGN"
    '        oCommand.CommandType = System.Data.CommandType.StoredProcedure
    '        oCommand.Parameters.Add("vNo", OracleType.NVarChar).Value = NoValue
    '        oCommand.Parameters("vNo").Direction = ParameterDirection.Input
    '        oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
    '        oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
    '        oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
    '        oCommand.Parameters("vResult").Direction = ParameterDirection.Output
    '        oCommand.ExecuteNonQuery()
    '        oCommand.Parameters.Clear()
    '        oCommand.CommandText = ""
    '        oCommand.CommandType = CommandType.Text
    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        oCommand.Dispose()
    '    End Try
    'End Sub

    ''' <summary>
    ''' 儲存RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Save(ByVal RMA_Status As Integer)
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim sRMAID As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oAdmin As New ctlAdmin

        Try
            'RMA Table
            dtRMA = Save_RMA(RMA_Status, True)

            'RMADetail Table
            dtRMADetail = Save_RMADetail(RMA_Status)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    sRMAID = oRequested.SaveByAddNew(dtRMA, dtRMADetail)

                Case eumCommand.UPDATE
                    oRequested.SaveByEdit(Me.UI_lblPreviousPage_RMANO.Text.Trim(), dtRMA, dtRMADetail)
                    sRMAID = Me.UI_lblPreviousPage_RMAID.Text.Trim()
            End Select
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If RMA_Status = 10 Then
                'addLog(DirectCast(dtRMA.Rows(0), DataService.RmaDTO.RMARow).RMA_NO, "User : " + Session("_UserID") + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
            Else
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Save ReqNew: " & sRMAID.Trim())
            End If

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)

            Else

                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " STATUS: " & RMA_Status)
                If RMA_Status = 10 Then
                    Dim bool_ISCW As Boolean = oRequested.chkISCWarrantyFee(sRMAID)
                    Dim _Crypto As New SecurityCrypt.Crypto
                    Dim sParm_01 As String = _Crypto.Encrypt(sRMAID.Trim, "")
                    Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
                    Dim sRedirectURL As String = "Request_Print.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02

                    '20210721 wisely add NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
                    If (UI_lblRepairCenterValue.Text = "NZ_PB_TECH" OrElse UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS") Then
                        bool_ISCW = False
                    End If

                    'getRequestForm(sRMAID.Trim())
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    Dim isSendMail As Boolean = False
                    If UI_PartsRequest.Checked = True Then
                        isSendMail = SendMail_Parts(Me.UI_lblUserIDText.Text.ToString().Trim(), Me.UI_txtApplicant.Text.ToString().Trim(), sRMAID.Trim(), bool_ISCW)
                    Else
                        isSendMail = SendMail(Me.UI_lblUserIDText.Text.ToString().Trim(), Me.UI_txtApplicant.Text.ToString().Trim(), sRMAID.Trim(), bool_ISCW)
                    End If

                    oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " endMail")


                    '則pop window 顯示警示文字
                    '判斷是否需運費 chkISCWarrantyFee (P0/PB/CW only)
                    If bool_ISCW = True And UI_PartsRequest.Checked = False Then
                        Me.ucMessage.showMessageBySuccess("<font color='red'>" & _oLanguage.getText("RMA", "411", ctlLanguage.eumType.Tag) & "</font>", ascx_ucMessage.eumTransferURL.Redirect, sRedirectURL)
                    Else
                        Response.Redirect(sRedirectURL)
                    End If
                Else
                    Dim sMsg As String = ""
                    Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                        Case eumCommand.AddNew
                            sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                            sMsg = _oLanguage.getText("Transfer", "044", ctlLanguage.eumType.Word) '"Please refer to Warranty Details for valid expiration date"
                        Case eumCommand.UPDATE
                            sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                    End Select

                    Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, _RedirectURL)

                End If

            End If
        End Try


    End Sub

    Private Function Save_RMA(ByVal RMA_Status As Integer, ByVal bool_ISCW As Boolean) As RmaDTO.RMADataTable
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oGuid As Guid = Guid.NewGuid

        Try
            Dim dr As RmaDTO.RMARow = dtRMA.NewRMARow

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    dr.RMA_ID = oGuid.ToString
                    dr.RMA_NO = ""

                Case eumCommand.UPDATE
                    dr.RMA_ID = Me.UI_lblPreviousPage_RMAID.Text.Trim()
                    dr.RMA_NO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
            End Select

            dr.RMA_CUNO = Me.UI_lblAccountIDText.Text.ToString().Trim()
            dr.RMA_ACCOUNTID = Me.UI_lblUserIDText.Text.ToString().Trim()
            dr.RMA_APPLICANT = Me.UI_txtApplicant.Text.ToString().Trim()
            dr.RMA_TEL = Me.UI_txtTel.Text.ToString().Trim()
            '' ''dr.RMA_ADDRESS = Me.UI_cboAddress.SelectedItem.Text.ToString().Trim()
            dr.RMA_ADDRESS = Me.UI_txtAddress.Text.Trim()

            If Me.UI_cboRepairCenter.Visible = False Then
                dr.RMA_COMPNO = Me.UI_lblRepairCenterValue.Text.ToString().Trim()
            Else
                dr.RMA_COMPNO = Me.UI_cboRepairCenter.SelectedValue
            End If


            dr.RMA_STATUS = RMA_Status
            dr.RMA_MAIL = Me.UI_txtMail.Text.Trim()
            dr.RMA_Remark = Me.UI_txtRemark.Text.Trim()

            dr.RMA_AD = Session("_UserID")
            dr.RMA_ADNAME = Session("_UserName")
            dr.RMA_CSTMP = Date.Now
            dr.RMA_LUAD = Session("_UserID")
            dr.RMA_LUADNAME = Session("_UserName")
            dr.RMA_LUSTMP = Date.Now
            dr.RMA_MARK = 0

            If bool_ISCW Then
                dr.RMA_EUCOMPANY = Me.UI_txtEUCompany.Text.Trim()
                dr.RMA_EUTEL = Me.UI_txtEUTel.Text.Trim()
                dr.RMA_EUNAME = Me.UI_txtEUName.Text.Trim()
                dr.RMA_EUMAIL = Me.UI_txtEUMail.Text.Trim()
                dr.RMA_EUADDRESS = Me.UI_txtEUAddress.Text.Trim()
            Else
                dr.RMA_EUCOMPANY = ""
                dr.RMA_EUTEL = ""
                dr.RMA_EUNAME = ""
                dr.RMA_EUMAIL = ""
                dr.RMA_EUADDRESS = ""
            End If

            If UI_PartsRequest.Checked Then
                dr.RMA_PARTSREQUEST = 1 'Part’s request MAIL
            Else
                dr.RMA_PARTSREQUEST = 0
            End If

            dtRMA.AddRMARow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMA
    End Function

    Private Function Save_RMADetail(ByVal RMA_Status As Integer) As RmaDTO.RMADetailDataTable
        Dim i As Integer = 0
        Dim dtTMP As New RmaDTO.RMADetailDataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oExport As New ctlRMA.Export


        Try
            dtTMP = Session("_dtRMADetail")
            Dim SNTemp As String = ""
            For i = 0 To UI_dvRMADetail.Rows.Count - 1
                Dim UI_UI_SERIALNO As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_SERIALNO")
                Dim UI_RMADID As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_RMADID")

                If UI_UI_SERIALNO.Text.ToString() <> "" Then
                    dtTMP.DefaultView.RowFilter = "RMAD_ID<>'" + UI_RMADID.Text.Trim + "' AND RMAD_SERIALNO='" + UI_UI_SERIALNO.Text + "' AND RMAD_MARK<>'1'"
                    If dtTMP.DefaultView.Count > 0 Then
                        Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                    End If

                    If SNTemp.IndexOf(UI_UI_SERIALNO.Text.Trim() + "~@~;") >= 0 Then
                        Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                    Else
                        dtTMP.DefaultView.RowFilter = "RMAD_ID='" + UI_RMADID.Text.Trim + "'"
                        If dtTMP.DefaultView.Count > 0 Then
                            dtTMP.DefaultView(0)("RMAD_SERIALNO") = UI_UI_SERIALNO.Text.Trim()
                        End If
                        SNTemp += UI_UI_SERIALNO.Text.Trim() + "~@~;"
                    End If
                Else
                    Throw New ArgumentException(_oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator))
                End If
            Next
            dtTMP.DefaultView.RowFilter = ""
            dtTMP.AcceptChanges()




            For i = 0 To dtTMP.Rows.Count - 1
                Dim drTMP As RmaDTO.RMADetailRow = dtTMP.Rows(i)
                Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow

                Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                    Case eumCommand.AddNew
                        Dim oGuid As Guid = Guid.NewGuid
                        dr.RMAD_ID = oGuid.ToString
                        dr.RMAD_RMANO = ""

                    Case eumCommand.UPDATE
                        dr.RMAD_ID = drTMP.RMAD_ID
                        dr.RMAD_RMANO = drTMP.RMAD_RMANO
                        'dr.RMAD_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
                End Select

                dr.RMAD_SEQ = i + 1
                If drTMP.IsRMAD_SERIALNONull = False Then dr.RMAD_SERIALNO = drTMP.RMAD_SERIALNO
                If drTMP.IsRMAD_CUSNAMENull = False Then dr.RMAD_CUSNAME = drTMP.RMAD_CUSNAME
                If drTMP.IsRMAD_PARTSNNull = False Then dr.RMAD_PARTSN = drTMP.RMAD_PARTSN
                '==================
                'MODEL NO
                '==================
                If drTMP.IsRMAD_MODELNONull = False Then dr.RMAD_MODELNO = drTMP.RMAD_MODELNO
                If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                    Dim sModelNo As String = oExport.getModelNo(drTMP.RMAD_SERIALNO.ToString().Trim())
                    If sModelNo.Trim() = "" Then
                        dr.RMAD_MODELNO = "OTHER"
                    Else
                        dr.RMAD_MODELNO = sModelNo
                    End If
                End If


                '==================
                'WarrantyDate
                '==================
                'If drTMP.IsRMAD_WARRANTYNull = False Then dr.RMAD_WARRANTY = drTMP.RMAD_WARRANTY
                Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                    'Dim sWarrantyDate As String = oExport.getWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString())
                    'Dim sWarrantyDate As String = ""
                    'If Me.UI_cboRepairCenter.Visible = False Then
                    '    sWarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                    'Else
                    '    sWarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                    'End If

                    'Dim WarrantyDate As String = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString())
                    Dim WarrantyDate As String = ""
                    If Me.UI_cboRepairCenter.Visible = False Then
                        WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                    Else
                        WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                    End If

                    'If sWarrantyDate.Trim() <> "" Then
                    '    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                    'End If
                    If WarrantyDate.Trim() <> "" Then
                        dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                    End If
                End If
                'If IsDate(sWarranty) = True Then
                'dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
                'End If





                If drTMP.IsRMAD_FARFARCNONull = False Then
                    If drTMP.RMAD_FARFARCNO.Trim() <> "-1" Then
                        dr.RMAD_FARFARCNO = drTMP.RMAD_FARFARCNO
                    End If
                End If

                If drTMP.IsRMAD_FARNONull = False Then
                    If drTMP.RMAD_FARNO.Trim() <> "-1" Then
                        dr.RMAD_FARNO = drTMP.RMAD_FARNO
                    End If
                End If

                If drTMP.IsRMAD_UPLOADFILENull = False Then dr.RMAD_UPLOADFILE = drTMP.RMAD_UPLOADFILE

                If drTMP.IsRMAD_PRODUCTDESCNull = False Then dr.RMAD_PRODUCTDESC = drTMP.RMAD_PRODUCTDESC
                If drTMP.IsRMAD_PROBLEMDESCNull = False Then dr.RMAD_PROBLEMDESC = drTMP.RMAD_PROBLEMDESC

                dr.RMAD_STATUS = RMA_Status
                dr.RMAD_ISFILL = drTMP.RMAD_ISFILL
                dr.RMAD_RECEVSTATUS = 0

                dr.RMAD_AD = Session("_UserID")
                dr.RMAD_ADNAME = Session("_UserName")
                dr.RMAD_CSTMP = Date.Now
                dr.RMAD_LUAD = Session("_UserID")
                dr.RMAD_LUADNAME = Session("_UserName")
                dr.RMAD_LUSTMP = Date.Now
                dr.RMAD_MARK = drTMP.RMAD_MARK

                dtRMADetail.AddRMADetailRow(dr)
            Next

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMADetail
    End Function

    Private Function SendMail_Parts(ByVal UserID As String, ByVal Applicant As String, ByVal RMAID As String, ByVal bool_ISCW As Boolean) As Boolean
        Dim blnFlag As Boolean = False
        Dim oAdmin As New ctlAdmin
        Dim dtPartMail As New AccountDTO.MailSetDataTable
        Dim oRequested As New ctlRMA.Requested
        Dim oMail As New ctlMail
        Try
            Dim RMA_NO As String = oRequested.getRMANo(RMAID.Trim())
            Dim sRequestDate As String = Date.Now.ToShortDateString()
            Dim MailUser As String = Me.UI_txtMail.Text.Trim() '申請人Mail

            If Not (UI_txtEUCompany.Text.Trim() = "") Then
                getRequestForm(RMAID, True) '附件檔產生
            Else
                getRequestForm(RMAID, False) '附件檔產生
            End If

            Dim oAttachmentFile As New Collection
            If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
            End If

            '額外寄給其他人
            dtPartMail = oAdmin.GetMailSet()
            dtPartMail.DefaultView.RowFilter = "MAIL_ID='Part'"
            If dtPartMail.DefaultView.Count > 0 Then
                If dtPartMail.DefaultView(0)("MAIL_ADDRESS") <> "" Then
                    MailUser = MailUser & "," & dtPartMail.DefaultView(0)("MAIL_ADDRESS")
                End If
            End If

            Dim sSubject_User As String = _oLanguage.getText("Mail", "037", ctlLanguage.eumType.Mail)
            Dim sBody_User As String = _oLanguage.getText("Mail", "010", ctlLanguage.eumType.Mail)
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            If MailUser <> "" Then
                sSubject_User = sSubject_User.Replace("[$Request No$]", RMA_NO.Trim())
                sSubject_User = sSubject_User.Replace("[$Request Date$]", sRequestDate)

                sBody_User = sBody_User.Replace("[$Applicant Name$]", Applicant.Trim())
                sBody_User = sBody_User.Replace("[$Request Date$]", sRequestDate)
                sBody_User = sBody_User.Replace("[$RMA No$]", RMA_NO.Trim())
                sBody_User = sBody_User.Replace("[$Email URL$]", sEmailURL)

                '20200319 wisely add 日本要收到
                If UI_cboRepairCenter.SelectedValue.ToUpper() = "JP_BYTE" Then
                    _MailCC += "," + _RequestNew_JP_BYTE_EmailCC
                End If

                '20210702 wisely add 特殊人員要收到
                If UI_cboRepairCenter.SelectedValue.ToUpper() = "US_CL_MPLUS" Then
                    _MailCC += "," + _RequestNew_US_CL_MPLUS_EmailCC
                End If

                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    MailUser = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject_User, sBody_User, MailUser, _MailCC, oAttachmentFile)
                If blnFlag = False Then
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " parts mail faild.")
                    '   Exit Try
                End If

            End If

        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
        Return blnFlag
    End Function

    Private Function SendMail(ByVal UserID As String, ByVal Applicant As String, ByVal RMAID As String, ByVal bool_ISCW As Boolean) As Boolean
        Dim blnFlag As Boolean = False

        Dim j As Integer = 0
        Dim ctlRMA As New ctlRMA
        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try
            Dim RMA_NO As String = oRequested.getRMANo(RMAID.Trim())
            Dim CU_NAME As String = ""
            Dim MailSales As String = ""
            Dim MailAssistant As String = ""
            dtCustomer = oCustomer.QueryUser(UserID)
            If dtCustomer.Rows.Count > 0 And RMA_NO.Trim() <> "" Then
                oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendMail")
                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(申請人)
                '================================================================================================================================================================================================================

                Dim sRequestDate As String = Date.Now.ToShortDateString()
                'Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()
                Dim MailUser As String = Me.UI_txtMail.Text.Trim()
                Dim sSubject_User As String = _oLanguage.getText("Mail", "009", ctlLanguage.eumType.Mail)
                Dim sBody_User As String = _oLanguage.getText("Mail", "010", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
                Try
                    If MailUser <> "" Then
                        oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendRequester")
                        sSubject_User = sSubject_User.Replace("[$Request No$]", RMA_NO.Trim())
                        sSubject_User = sSubject_User.Replace("[$Request Date$]", sRequestDate)

                        sBody_User = sBody_User.Replace("[$Applicant Name$]", Applicant.Trim())
                        sBody_User = sBody_User.Replace("[$Request Date$]", sRequestDate)
                        sBody_User = sBody_User.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody_User = sBody_User.Replace("[$Email URL$]", sEmailURL)

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            MailUser = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            MailUser = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject_User, sBody_User, MailUser, _MailCC)
                        If blnFlag = False Then
                            oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & "cust mail faild.")
                            '   Exit Try
                        End If

                    End If
                Catch ex As Exception
                    ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 begin
                    'oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to customer fail.", "sunny@cipherlab.com.tw", "ryan.lee@cipherlab.com.tw")
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to customer fail.", "sunny@cipherlab.com.tw")
                    ' 2025.09.12 移除離職人員信箱。ryan.lee@cipherlab.com.tw by buck modify 20250912 end
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to sales fail.")

                Finally


                End Try

                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(業務和助理)
                '================================================================================================================================================================================================================
                Try
                    CU_NAME = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                    Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                    Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                    'Dim MailSales As String = ""
                    Dim SalesName As String = ""
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " sales : " & CU_SALESID.Trim())
                    If CU_SALESID.Trim() <> "" Then
                        dtAdmin = oAdmin.Query(CU_SALESID, "")
                        MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                        SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If

                    ' Dim MailAssistant As String = ""
                    Dim AssistantName As String = ""
                    If CU_ASSISTANTID.Trim() <> "" Then
                        dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                        MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                        AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If

                    Dim sSubject As String = _oLanguage.getText("Mail", "011", ctlLanguage.eumType.Mail)
                    Dim sBody As String = _oLanguage.getText("Mail", "012", ctlLanguage.eumType.Mail)
                    If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                        Dim sMailTo As String = ""
                        sSubject = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())
                        sSubject = sSubject.Replace("[$Request No$]", RMA_NO.Trim())
                        sSubject = sSubject.Replace("[$Request Date$]", sRequestDate.Trim())

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

                        sBody = sBody.Replace("[$Request Date$]", sRequestDate.Trim())
                        sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                        sBody = sBody.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                        '20200319 wisely add 日本要收到
                        If UI_cboRepairCenter.SelectedValue.ToUpper() = "JP_BYTE" Then
                            _MailCC += "," + _RequestNew_JP_BYTE_EmailCC
                        End If

                        '20210702 wisely add 特殊人員要收到
                        If UI_cboRepairCenter.SelectedValue.ToUpper() = "US_CL_MPLUS" Then
                            _MailCC += "," + _RequestNew_US_CL_MPLUS_EmailCC
                        End If


                        oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendSales")

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            sMailTo = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)

                    End If
                Catch ex As Exception

                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = "sunny@cipherlab.com.tw"
                    Dim mailCC As String = "yijen.lo@cipherlab.com.tw"
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        mailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to sales fail.", mailTo, mailCC)
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to sales fail.")

                Finally


                End Try
            End If


            '================================================================================================================================================================================================================
            '顧客申請確認-->對象(維修中心維修人員)
            '================================================================================================================================================================================================================
            If bool_ISCW = True Then
                Try
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendbyCW")
                    Dim sRequestDate As String = Date.Now.ToShortDateString()

                    If Not (UI_txtEUCompany.Text.Trim() = "") Then
                        getRequestForm(RMAID, True) '附件檔產生
                    Else
                        getRequestForm(RMAID, False) '附件檔產生
                    End If

                    Dim oAttachmentFile As New Collection
                    If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
                    End If

                    Dim sSubject_Repair As String = _oLanguage.getText("Mail", "424", ctlLanguage.eumType.Mail)
                    Dim sBody_Repair As String = _oLanguage.getText("Mail", "425", ctlLanguage.eumType.Mail)
                    Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                    sSubject_Repair = sSubject_Repair.Replace("[$Customer's Name$]", CU_NAME.Trim())
                    sSubject_Repair = sSubject_Repair.Replace("[$Request No$]", RMA_NO.Trim())
                    sSubject_Repair = sSubject_Repair.Replace("[$Request Date$]", sRequestDate.Trim())

                    Dim Repaire_MailTo As String = ""
                    Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                    Dim Repaire_Name As String = ""                    '維修人員
                    Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RMA(RMA_NO)
                    For j = 0 To arrRepaire.Count - 1
                        Dim arrList() As String = arrRepaire(j)

                        Repaire_Name = arrList(0).Trim()
                        Repaire_EMAIL = arrList(1).Trim()

                        'If Repaire_Name.Trim <> "" Then
                        '    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", Repaire_Name)
                        'Else
                        '    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "")
                        'End If

                        If Repaire_MailTo.IndexOf(Repaire_EMAIL.Trim()) = -1 Then
                            If Repaire_MailTo.Trim <> "" Then
                                Repaire_MailTo = Repaire_MailTo & ","
                            End If

                            Repaire_MailTo = Repaire_MailTo & Repaire_EMAIL.Trim()
                        End If
                    Next

                    'Isaac Add 180326 加入業務業管Mail通知
                    If MailSales.Trim() <> "" Then '業務Mail
                        Repaire_MailTo = Repaire_MailTo & "," & MailSales.Trim()
                    End If

                    If MailAssistant.Trim() <> "" Then '助理Mail
                        Repaire_MailTo = Repaire_MailTo & "," & MailAssistant.Trim()
                    End If


                    'Repaire_MailTo = "isaac.yeh@cipherlab.com.tw,sunny@cipherlab.com.tw"
                    If Repaire_MailTo.Trim <> "" Then
                        sBody_Repair = sBody_Repair.Replace("[$Request Date$]", sRequestDate.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            Repaire_MailTo = ConfigurationManager.AppSettings("MailTo")
                            _RepairEmailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_MailTo, _RepairEmailCC, oAttachmentFile)
                    End If

                Catch ex As Exception
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = "sunny@cipherlab.com.tw"
                    Dim mailCC As String = "yijen.lo@cipherlab.com.tw"
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        mailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to RMA center fail.", mailTo, mailCC)
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to RMA fail.")

                Finally


                End Try

            End If

        Catch ex As Exception


        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' Excel檔案==新增Serial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdFileAdd_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdFileAdd.Click
        Dim blnFlag As Boolean
        Dim sMessage As String = ""
        Dim i As Integer = 0

        Dim dt As New DataTable
        Dim oExport As New ctlRMA.Export
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable


        Try
            Dim sFullFileName As String = UpLoadFile()
            If sFullFileName = "" Then
                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
            End If

            Dim dir As String = _Requested_Upload_FilePath

            Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
            Dim TextLine As String

            If System.IO.File.Exists(FILE_NAME) = True Then
                Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                Do While objReader.Peek() <> -1
                    TextLine = objReader.ReadLine()
                    Dim arrText() As String = TextLine.Split(",")

                    If arrText.Length > 0 Then
                        If arrText(0).ToString().Trim().ToLower <> "Serial Number".ToLower() Then
                            Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                            Dim sModel As String = arrText(1).ToString().Trim().ToUpper()            'Model No
                            Dim sProductDesc As String = arrText(2).ToString().Trim()      '產品敘述
                            Dim sCusName As String = arrText(3).ToString().Trim()         'Customer Product Name
                            Dim sFarFarcNo As String = arrText(4).ToString().Trim()       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
                            Dim sFarNo As String = arrText(5).ToString().Trim()           '關聯 FailureReasons.FAR_NO-->不良原因代碼
                            Dim sProblemDesc As String = arrText(6).ToString().Trim()     'Problem Description

                            Dim sWarranty As String = ""        '保固日期
                            Dim sPartSn As String = ""        '零件序號

                            Dim sQuerySN As String = ""
                            Dim ctlReport As New ctlReport
                            Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable
                            dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), sSerial)
                            If dtReport.Rows.Count > 0 Then
                                sQuerySN = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
                            End If

                            If sQuerySN <> "" Then
                                If sQuerySN <> sSerial Then
                                    sPartSn = sSerial '輸入的零件序號
                                    sSerial = sQuerySN '查詢得到主機序號
                                End If
                            End If


                            '檢核 Serial No 是否有重複
                            If sSerial.Trim() <> "" Then
                                dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + sSerial.Trim() + "' AND RMAD_MARK<>'1'"
                                If dtRMADetail.DefaultView.Count > 0 Then
                                    Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                                    Throw New ArgumentException(sErrorMsg)
                                End If
                            End If


                            'Warranty
                            If sSerial.ToString().Trim() <> "" Then
                                'Dim sWarrantyDate As String = oExport.getWarranty(sSerial, Session("_CustomerID").ToString())
                                Dim sWarrantyDate As String = ""
                                If Me.UI_cboRepairCenter.Visible = False Then
                                    sWarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                                Else
                                    sWarrantyDate = oExport.getMaxWarranty(Me.UI_txtSerial.Text.ToUpper(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                                End If

                                If sWarrantyDate.Trim() <> "" Then
                                    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                                End If
                            Else
                                sWarranty = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                            End If

                            'ModelNo
                            If sSerial.Trim() <> "" Then
                                'Dim sModelNo As String = oExport.getModelNo(sSerial.Trim())
                                Dim sModelNo As String = oExport.getModelNo(sSerial.Trim(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
                                If sModelNo.Trim() = "" Then
                                    sModel = "OTHER"
                                Else
                                    sModel = sModelNo
                                End If
                            Else
                                If sModel.Trim() = "" Then
                                    sModel = "OTHER"
                                Else
                                    Dim oModel As New ctlRMA.Model
                                    If oModel.chkModel(sModel.Trim()) = False Then
                                        sModel = "OTHER"
                                    End If
                                End If
                            End If

                            'FarFarcNO
                            If sFarFarcNo.Trim() = "" Then
                                sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                sFarNo = -1

                            Else
                                Dim oFailure As New ctlFailure

                                If sFarFarcNo.Trim() <> "" And sFarNo.Trim() <> "" Then
                                    If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim(), sFarNo.Trim()) = False Then
                                        sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                        sFarNo = -1
                                    End If
                                Else

                                    If sFarFarcNo.Trim() <> "" And sFarNo.Trim() = "" Then
                                        If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim()) = False Then
                                            sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                            sFarNo = -1
                                        End If
                                    End If
                                End If

                            End If

                            If IsNothing(Session("_dtRMADetail")) = True Then
                                dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn)
                            Else
                                dtRMADetail = Session("_dtRMADetail")
                                dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn)
                            End If

                        End If
                    End If
                Loop
                objReader.Close()
            End If

            Session("_dtRMADetail") = dtRMADetail
            Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
            Call RMADetail_DataBind(iPageIndex)

            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Dim sScript As String = "<script type=""text/javascript"">alert(""" & sMessage.Substring(0, 10) & """)</script>"
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                'Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try



        '' ''Dim oExport As New ctlRMA.Export
        '' ''Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        '' ''Dim i As Integer = 0
        '' ''Dim sSerial As String = ""          'Serial No
        '' ''Dim sModel As String = ""           'Model No
        '' ''Dim sCusName As String = ""         'Customer Product Name
        '' ''Dim sWarranty As String = ""        '保固日期
        '' ''Dim sFarFarcNo As String = ""       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
        '' ''Dim sFarNo As String = ""           '關聯 FailureReasons.FAR_NO-->不良原因代碼
        '' ''Dim sProblemDesc As String = ""     'Problem Description
        '' ''Dim sProductDesc As String = ""     '產品敘述

        '' ''For i = 0 To 2
        '' ''    sSerial = "P0001"
        '' ''    sModel = "M0002"
        '' ''    sCusName = "111111"
        '' ''    'sFarFarcNo = "001"
        '' ''    'sFarNo = "101"
        '' ''    sProblemDesc = "sProblemDesc"
        '' ''    sProductDesc = "sProductDesc"

        '' ''    'Warranty
        '' ''    If sSerial.ToString().Trim() <> "" Then
        '' ''        Dim sWarrantyDate As String = oExport.getWarranty(sSerial, Session("_CustomerID").ToString())
        '' ''        If sWarrantyDate.Trim() <> "" Then
        '' ''            sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
        '' ''        End If
        '' ''    Else
        '' ''        sWarranty = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        '' ''    End If

        '' ''    'ModelNo
        '' ''    If sSerial.Trim() <> "" Then
        '' ''        Dim sModelNo As String = oExport.getModelNo(sSerial.Trim())
        '' ''        If sModelNo.Trim() = "" Then
        '' ''            sModel = "OTHER"
        '' ''        Else
        '' ''            sModel = sModelNo
        '' ''        End If
        '' ''    Else
        '' ''        If sModel.Trim() = "" Then
        '' ''            sModel = "OTHER"
        '' ''        End If
        '' ''    End If

        '' ''    'FarFarcNO
        '' ''    If sFarFarcNo.Trim() = "" Then
        '' ''        sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
        '' ''    End If


        '' ''    If IsNothing(Session("_dtRMADetail")) = True Then
        '' ''        dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc)
        '' ''    Else
        '' ''        dtRMADetail = Session("_dtRMADetail")
        '' ''        dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc)
        '' ''    End If
        '' ''Next


        '' ''Session("_dtRMADetail") = dtRMADetail
        '' ''Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
        '' ''Call RMADetail_DataBind(iPageIndex)
    End Sub

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <param name="sSerial"></param>
    ''' <param name="sModel"></param>
    ''' <param name="sCusName"></param>
    ''' <param name="sWarranty"></param>
    ''' <param name="sFarFarcNo"></param>
    ''' <param name="sFarNo"></param>
    ''' <param name="sProblemDesc"></param>
    ''' <param name="sProductDesc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Execl_AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable, ByVal sSerial As String, ByVal sModel As String, ByVal sCusName As String, ByVal sWarranty As String,
                                       ByVal sFarFarcNo As String, ByVal sFarNo As String, ByVal sProblemDesc As String, ByVal sProductDesc As String, ByVal sPartSN As String) As RmaDTO.RMADetailDataTable

        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim oExport As New ctlRMA.Export

        Try
            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""
            dr.RMAD_MODELNO = sModel.Trim()
            dr.RMAD_SERIALNO = sSerial.Trim()
            dr.RMAD_CUSNAME = sCusName.Trim()                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty                       'Warranty 字串格式
            'dr.RMAD_WARRANTY = oExport.getMaxWarranty(sSerial.Trim(), Session("_CustomerID").ToString())
            If Me.UI_cboRepairCenter.Visible = False Then
                dr.RMAD_WARRANTY = oExport.getMaxWarranty(sSerial.Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
            Else
                dr.RMAD_WARRANTY = oExport.getMaxWarranty(sSerial.Trim(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
            End If

            'If IsDate(sWarranty) = True Then
            'dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
            'End If

            dr.RMAD_FARFARCNO = sFarFarcNo.Trim()
            dr.RMAD_FARNO = sFarNo.Trim()
            dr.RMAD_PARTSN = sPartSN.Trim()
            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = sProductDesc.Trim()
            dr.RMAD_PROBLEMDESC = sProblemDesc.Trim()
            dr.RMAD_STATUS = 0

            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMADetail
    End Function

    Private Function UpLoadFile() As String
        Dim retval As String = ""

        Try
            Dim txtFileName As String = Me.html_FileUpload.FileName

            '****** 取得檔名 **********
            Dim FileSplit() As String = Split(txtFileName, "\")
            Dim FileName As String = FileSplit(FileSplit.Length - 1)

            '****** 取得副檔名 **********
            Dim auxFileSplit() As String = Split(FileName, ".")
            Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
            Dim sFileNameChange As String = ""
            Dim oCommon As New Common
            sFileNameChange = oCommon.GetRandomizeNum()
            sFileNameChange = sFileNameChange & "." & auxFileName

            '***** 檔案(原檔名,亂數檔名) *****
            Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

            Me.html_FileUpload.SaveAs(_Requested_Upload_FilePath & sFileNameChange)
            'Me.html_FileUpload.MoveTo(_Requested_Upload_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
            retval = sFileNameChange

        Catch ex As Exception
            Throw ex
            retval = ""

        End Try

        Return retval
    End Function

    Private Sub getRequestForm(ByVal sRMAID As String, ByVal EndUser As Boolean)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)


        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)

        'dtRequest = oRMARequest.getReport(Session("_LanguageID").ToString().Trim(), sRMAID)

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

            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next

        Call Print(dtReport, EndUser, LanguageID)
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal LanguageID As String)
        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        '取得客戶的語系
        Dim sCust As String = UI_txtAccountIDText.Text
        Dim oLoginInfo As New ctlLoginInfo
        Dim sLanguageID As String = oLoginInfo.getLanguageID("Customer", sCust)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If (UI_PartsRequest.Checked) Then
            If (sLanguageID = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts_jp.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_Parts.rpt"))
            End If

        ElseIf (EndUser) Then
            If (sLanguageID = "003") Then
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            Else
                ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))
            End If
        Else
            If (sLanguageID = "003") Then
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

    '    Return retval
    'End Function

    ''Dim conString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='text;HDR=Yes;FMT=Delimited';Data Source=" & dir & ";"
    'Dim conString As String = "Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=" & dir & ";Extensions=asc,csv,tab,txt;Persist Security Info=False"
    'Dim conn As New Odbc.OdbcConnection(conString)
    'Dim da As New Odbc.OdbcDataAdapter("Select * from " & sFullFileName, conn)

    'da.Fill(dt)

    'For i = 0 To dt.Rows.Count - 1
    '    Dim sSerial As String = dt.Rows(i)(0).ToString().Trim()        'Serial No
    '    Dim sModel As String = dt.Rows(i)(1).ToString().Trim()            'Model No
    '    Dim sProductDesc As String = dt.Rows(i)(2).ToString().Trim()      '產品敘述
    '    Dim sCusName As String = dt.Rows(i)(3).ToString().Trim()         'Customer Product Name
    '    Dim sFarFarcNo As String = dt.Rows(i)(4).ToString().Trim()       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
    '    Dim sFarNo As String = dt.Rows(i)(5).ToString().Trim()           '關聯 FailureReasons.FAR_NO-->不良原因代碼
    '    Dim sProblemDesc As String = dt.Rows(i)(6).ToString().Trim()     'Problem Description

    '    Dim sWarranty As String = ""        '保固日期

    'Next

    Protected Sub UI_cmdMail_Click(sender As Object, e As EventArgs) Handles UI_cmdMail.Click
        SendMail("DT618", "詹佳真", "2c880659-fbf1-48f9-883b-966cd0e4df5f", True)
    End Sub

End Class

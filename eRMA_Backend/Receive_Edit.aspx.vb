Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Receive_Edit
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _RedirectURL As String = "Receive_WorkList.aspx"
    Dim _Address As String = ""
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    'Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Session("_dtRMADetail") = Nothing
            Me.ViewState("_dtAddress") = Nothing

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Dim UI_lblPreviousPage_CUNO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_CUNO")

                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()
                Me.UI_lblPreviousPage_CUNO.Text = UI_lblPreviousPage_CUNO.Text.Trim()

                Me.UI_lblRMANumberText.Text = Me.UI_lblPreviousPage_RMANO.Text

                Dim hsSelectID As New Hashtable
                Me.ViewState("hsSelectID") = hsSelectID
            End If

            Call getModelData()
            Call QueryData_Head()
            Call QueryData_RMA()

            Call setControls()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim sClientID As String = Me.UI_cmdPick.ClientID '& "," & Me.UI_cmdAdd.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANumber.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblUserID.Text = _oLanguage.getText("RMA", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblProductInformation.Text = _oLanguage.getText("RMA", "010", ctlLanguage.eumType.Tag)
        'Me.UI_lblPleaseTittle.Text = _oLanguage.getText("RMA", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblMail.Text = _oLanguage.getText("RMA", "044", ctlLanguage.eumType.Tag)
        Me.UI_lblSerialNo.Text = _oLanguage.getText("RMA", "043", ctlLanguage.eumType.Tag)


        Me.rfv_txtTEL.ErrorMessage = _oLanguage.getText("RMA", "207", ctlLanguage.eumType.Validator)
        Me.rfv_txtAdress.ErrorMessage = _oLanguage.getText("RMA", "047", ctlLanguage.eumType.Validator)
        Me.rfv_txtApplicant.ErrorMessage = _oLanguage.getText("RMA", "048", ctlLanguage.eumType.Validator)
        'Me.rfv_txtModelNo.ErrorMessage = _oLanguage.getText("RMA", "191", ctlLanguage.eumType.Validator)
        Me.cvModelNo_Serial.ErrorMessage = _oLanguage.getText("RMA", "210", ctlLanguage.eumType.Validator)

        Me.revEMail_Empty.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)
        'Me.revEMail.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)

        '設定 Enter 鍵觸發
        Me.UI_txtSerialNo.Attributes.Add("OnKeypress", "return clickButton(event,'" & Me.UI_cmdQuery.ClientID & "')")

        Me.UI_cmdAdressPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdQuery.Value = _oLanguage.getText("Common", "022", ctlLanguage.eumType.Command)
        Me.UI_cmdPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)

        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.UI_cmdDelete.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        Me.UI_cmdTmpSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        'Me.UI_cmdSubmit.Enabled = False
        Me.UI_cmdSubmit.Attributes.Add("disabled", "true")
        Me.UI_cmdDelete.Attributes.Add("disabled", "true")
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

    '''' <summary>
    '''' 取得Model 的資料
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub getModelData()
    '    Dim i As Integer = 0
    '    Dim oListItem As ListItem
    '    Dim oModel As New ctlRMA.Model
    '    Dim dtModel As New RmaDTO.ModelDataTable

    '    dtModel = oModel.Query("")

    '    Me.UI_cboModel.Items.Clear()
    '    oListItem = New ListItem
    '    oListItem.Text = _oLanguage.getText("RMA", "208", ctlLanguage.eumType.Tag)
    '    oListItem.Value = ""
    '    Me.UI_cboModel.Items.Add(oListItem)

    '    For i = 0 To dtModel.Rows.Count - 1
    '        oListItem = New ListItem
    '        oListItem.Text = dtModel.Rows(i).Item("MODELNAME").ToString().Trim()
    '        oListItem.Value = dtModel.Rows(i).Item("MODELNO").ToString().Trim()
    '        Me.UI_cboModel.Items.Add(oListItem)
    '    Next

    'End Sub

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

        dtCustome = oCustomer.QueryByCompany(Me.UI_lblPreviousPage_CUNO.Text)
        Dim sTel As String = ""

        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            Me.UI_lblAccountIDText.Text = dr.CU_NO.ToString().Trim()
            Me.UI_lblAccountNameText.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
            Me.UI_lblRepairCenterValue.Text = dr.COMP_NO.ToString().Trim()

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

            '' ''If dr.IsCU_ADDRESS1Null = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CU_ADDRESS1.Trim()
            '' ''    oListItem.Value = dr.CU_ADDRESS1.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If

            '' ''If dr.IsCU_ADDRESS2Null = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CU_ADDRESS2.Trim()
            '' ''    oListItem.Value = dr.CU_ADDRESS2.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If

            '' ''If dr.IsCU_ADDRESS3Null = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CU_ADDRESS3.Trim()
            '' ''    oListItem.Value = dr.CU_ADDRESS3.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If

            '' ''If dr.IsCU_ADDRESS4Null = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CU_ADDRESS4.Trim()
            '' ''    oListItem.Value = dr.CU_ADDRESS4.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If
        End If

        dtCustomerUser = oCustomerUser.QueryUser(Me.UI_lblPreviousPage_CUNO.Text, "")
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

            Me.UI_txtAddress.Text = dtRMA.Rows(0)("RMA_ADDRESS").ToString().Trim()
            Me.UI_lblPreviousPage_RMASTATUS.Text = dtRMA.Rows(0)("RMA_STATUS").ToString().Trim()

            Me.UI_txtEUCompany.Text = dtRMA.Rows(0)("RMA_EUCOMPANY").ToString().Trim()
            Me.UI_txtEUTel.Text = dtRMA.Rows(0)("RMA_EUTEL").ToString().Trim()
            Me.UI_txtEUName.Text = dtRMA.Rows(0)("RMA_EUNAME").ToString().Trim()
            Me.UI_txtEUMail.Text = dtRMA.Rows(0)("RMA_EUMAIL").ToString().Trim()
            Me.UI_txtEUAddress.Text = dtRMA.Rows(0)("RMA_EUADDRESS").ToString().Trim()
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
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        dvRMADetail.RowFilter = "RMAD_MARK=0"

        Call ArrangementData(dtRMADetail)

        'Me.UI_dvRMADetail.PageSize = _PageSize
        'Me.UI_dvRMADetail.PageIndex = iPageIndex
        Me.UI_dvRMADetail.DataSource = dvRMADetail
        Me.UI_dvRMADetail.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then

            dtRMADetail.Columns.Add("SeqID")
            dtRMADetail.Columns.Add("RecvDATE")
            dtRMADetail.Columns.Add("RecvDelDATE")
            dtRMADetail.Columns.Add("CWEndWarr")
            dtRMADetail.Columns.Add("SWEndWarr")
            dtRMADetail.Columns.Add("RepairCnt")

            Dim oColumn As New DataColumn
            oColumn.ColumnName = "isCheck"
            oColumn.DataType = GetType(System.String)
            oColumn.DefaultValue = "0"
            dtRMADetail.Columns.Add(oColumn)
        End If

        For i = 0 To dtRMADetail.Rows.Count - 1
            dtRMADetail.Rows(i)("SeqID") = i + 1

            If IsDate(dtRMADetail.Rows(i)("RMAD_WARRANTY")) = False Then
                'dtRMADetail.Rows(i)("RMAD_sWARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRMADetail.Rows(i)("RMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        'dtRMADetail.Rows(i)("RMAD_sWARRANTY") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        'dtRMADetail.Rows(i)("RMAD_sWARRANTY") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select

            Else
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(dtRMADetail.Rows(i)("RMAD_WARRANTY")).ToShortDateString
            End If

            Dim oExport As New ctlRMA.Export
            Dim sCWEnd As String = oExport.getWarrantyCW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sCWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("CWEndWarr") = Convert.ToDateTime(sCWEnd).ToShortDateString()
            End If
            Dim sSWEnd As String = oExport.getWarrantySW(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            If sSWEnd.Trim() <> "" Then
                dtRMADetail.Rows(i)("SWEndWarr") = Convert.ToDateTime(sSWEnd).ToShortDateString()
            End If

            dtRMADetail.Rows(i)("RepairCnt") = oExport.getRepairCnt(dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString(), "")
            'dtRMADetail.Rows(i)("RepairCnt") = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString()

            Dim RMAD_RECVDATE As String = ""
            If dtRMADetail.Rows(i)("RMAD_RECVDATE").ToString().Trim() <> "" Then
                RMAD_RECVDATE = Convert.ToDateTime(dtRMADetail.Rows(i)("RMAD_RECVDATE")).ToShortDateString()
            End If

            '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            dtRMADetail.Rows(i)("RecvDATE") = ""
            dtRMADetail.Rows(i)("RecvDelDATE") = ""
            Select Case Convert.ToInt16(dtRMADetail.Rows(i)("RMAD_RECEVSTATUS"))
                Case 1
                    dtRMADetail.Rows(i)("RecvDATE") = RMAD_RECVDATE

                Case 2
                    dtRMADetail.Rows(i)("RecvDelDATE") = RMAD_RECVDATE
            End Select

            '202105004 轉換model
            Dim sModelNo As String = oExport.getMModelNo(dtRMADetail.Rows(i)("RMAD_MODELNO").ToString().Trim(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
            dtRMADetail.Rows(i)("RMAD_MODELNO") = sModelNo

        Next
    End Sub

    Protected Sub UI_dvRMADetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRMADetail.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
            'e.Row.Cells(4).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)

            'e.Row.Cells(4).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            'e.Row.Cells(5).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)
            'e.Row.Cells(6).Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)


            e.Row.Cells(12).Text = _oLanguage.getText("RMA", "016", ctlLanguage.eumType.Tag)
            e.Row.Cells(13).Text = _oLanguage.getText("RMA", "042", ctlLanguage.eumType.Tag)
            e.Row.Cells(14).Text = _oLanguage.getText("RMA", "017", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMAcheck As CheckBox = e.Row.FindControl("UI_RMAcheck")
            Dim UI_RMADWARRANTY As Label = e.Row.FindControl("UI_RMADWARRANTY")
            Dim UI_RECEVSTATUS As Label = e.Row.FindControl("UI_RECEVSTATUS")
            Dim UI_ISFILL As Label = e.Row.FindControl("UI_ISFILL")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_Delcheck As CheckBox = e.Row.FindControl("UI_Delcheck")
            Dim imgDel As ImageButton = e.Row.FindControl("imgDel")
            Dim UI_PARTNUMBER As Label = e.Row.FindControl("UI_PARTNUMBER")

            Dim RMAD_SERIALNO As TextBox = e.Row.FindControl("RMAD_SERIALNO")
            Dim UI_SHOWSERIAL As Label = e.Row.FindControl("UI_SHOWSERIAL")
            If UI_PARTNUMBER.Text.Trim() <> "" Then '如果是送Parts
                UI_SHOWSERIAL.Text = UI_PARTNUMBER.Text.Trim()
            Else
                UI_SHOWSERIAL.Text = RMAD_SERIALNO.Text.Trim()
            End If

            If UI_RMADWARRANTY.Text.Trim() <> "" Then
                Dim RMAD_WARRANTY As DateTime = Convert.ToDateTime(UI_RMADWARRANTY.Text.Trim())
                If RMAD_WARRANTY < Date.Now Then
                    e.Row.Cells(5).ForeColor = Drawing.Color.Red
                End If
            End If

            If UI_RECEVSTATUS.Text.Trim() = "2" Then
                UI_Delcheck.Visible = False
                imgDel.Visible = False
            End If

            If UI_RECEVSTATUS.Text.Trim() = "1" Or UI_RECEVSTATUS.Text.Trim() = "2" Then
                UI_RMAcheck.Visible = False
            End If

            '是否已填寫問題:0.否, 1.是
            UI_cmdEdit.Text = _oLanguage.getText("Common", "025", ctlLanguage.eumType.Command)
            If UI_ISFILL.Text.Trim() = "1" Then
                UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            Else
                '未填寫問題, 不可以收貨
                UI_RMAcheck.Visible = False
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
            Call keepCheck()
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Me.UcPopProblem.show(UI_RMADID.Text.Trim(), True, Me.UI_lblRepairCenterValue.Text.Trim(), Me.UI_lblAccountIDText.Text.Trim())
            Me.UcPopProblem.isModified = True
        End If

        If e.CommandName = "cmdDel" Then
            Call keepCheck()
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim UI_RMADNO As Label = row.FindControl("UI_RMADNO")
            Call Delete(UI_RMADID.Text.ToString(), UI_RMADNO.Text.Trim())
        End If

        If e.CommandName = "cmdWarrDetail" Then
            iIndex = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMAD_SERIALNO As Label = row.FindControl("UI_RMAD_SERIALNO")

            Dim UI_RMAD_MODELNO As Label = row.FindControl("UI_RMAD_MODELNO")
            Dim sModelNo As String = oExport.getCModelNo(UI_RMAD_MODELNO.Text.Trim(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
            'Me.UcWarrantyPartsView.show(True, UI_RMAD_SERIALNO.Text.Trim(), UI_RMAD_MODELNO.Text.Trim(), UI_lblRepairCenterValue.Text.Trim())
            Me.UcWarrantyPartsView.show(True, UI_RMAD_SERIALNO.Text.Trim(), sModelNo, UI_lblRepairCenterValue.Text.Trim())
        End If

        If e.CommandName = "cmdDetail" Then
            iIndex = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Dim RMA_NO As String = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()
            Me.ucClientDetailPur.show(UI_RMADID.Text.ToString().Trim(), RMA_NO.Trim(), True)
        End If

        If e.CommandName = "cmdSWDetail" Then
            iIndex = Convert.ToInt16(e.CommandArgument)
            row = Me.UI_dvRMADetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")
            Me.ucSpecialSetting.show(UI_RMADID.Text.ToString().Trim(), True)
        End If
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

        Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim())
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
        Dim ctlReport As New ctlReport
        Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable

        If UI_txtSerial.Text = "" Then
            Dim sErrorMsg As String = _oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator) 'Wait
            Throw New ArgumentException(sErrorMsg)
        End If

        UI_txtSerialParts.Text = ""
        Dim sProductSn As String = ""
        dtReport = ctlReport.QueryRMAWarranty("", Me.UI_txtSerial.Text.Trim().ToUpper())
        If dtReport.Rows.Count > 0 Then
            sProductSn = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
        End If
        If sProductSn <> "" Then
            If sProductSn <> Me.UI_txtSerial.Text.Trim().ToUpper() Then
                UI_txtSerialParts.Text = Me.UI_txtSerial.Text.Trim().ToUpper()
                Me.UI_txtSerial.Text = sProductSn
            Else
                sProductSn = ""
            End If
        End If

        Dim dtRMADetail As New RmaDTO.RMADetailDataTable

        Dim oScriptManager As ScriptManager = Me.Master.FindControl("ScriptManager")
        oScriptManager.SetFocus(Me.UI_txtSerialNo)

        If IsNothing(Session("_dtRMADetail")) = True Then
            dtRMADetail = AddRMADetail(dtRMADetail)
        Else
            dtRMADetail = Session("_dtRMADetail")
            dtRMADetail = AddRMADetail(dtRMADetail)
        End If
        Session("_dtRMADetail") = dtRMADetail

        Call keepCheck()

        Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
        Call RMADetail_DataBind(iPageIndex)

        Me.UI_txtSerial.Text = ""
        Me.UI_cboModel.SelectedValue = ""
    End Sub

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As RmaDTO.RMADetailDataTable
        Dim oRequested As New ctlRMA.Requested
        Dim oReceived As New ctlRMA.Received
        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        Dim oExport As New ctlRMA.Export

        If Me.UI_txtSerial.Text.Trim() <> "" Then
            dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + Me.UI_txtSerial.Text.Trim() + "' AND RMAD_MARK<>'1'"
            If dtRMADetail.DefaultView.Count > 0 Then
                Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                Throw New ArgumentException(sErrorMsg)
            End If
        End If

        Try
            If Me.UI_txtSerial.Text.ToString().Trim() <> "" Then
                'Dim sWarrantyDate As String = oExport.getWarranty(Me.UI_txtSerial.Text, Me.UI_lblPreviousPage_CUNO.Text)
                Dim sWarrantyDate As String = oExport.getMaxWarranty(Me.UI_txtSerial.Text, Me.UI_lblPreviousPage_CUNO.Text, Me.UI_lblRepairCenterValue.Text.Trim())
                If sWarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                End If
            End If

            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = dtRMADetail.Rows.Count + 1
            dr.RMAD_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()

            '' ''dr.RMAD_MODELNO = Me.UI_txtModel.Text.Trim()
            'dr.RMAD_MODELNO = Me.UI_cboModel.SelectedValue
            If Me.UI_txtSerial.Text.Trim() <> "" Then
                Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim())
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

            dr.RMAD_SERIALNO = Me.UI_txtSerial.Text.ToString().Trim()
            dr.RMAD_CUSNAME = ""                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty          'Warranty 字串格式
            If IsDate(sWarranty) = True Then
                dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
            End If

            'dr.RMAD_FARFARCNO = "-1"
            'dr.RMAD_FARNO = "-1"
            dr.RMAD_FARFARCNO = _oLanguage.getText("RMA", "233", ctlLanguage.eumType.Tag)
            dr.RMAD_FARNO = _oLanguage.getText("RMA", "234", ctlLanguage.eumType.Tag)
            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = " "
            'dr.RMAD_PROBLEMDESC = ""
            dr.RMAD_STATUS = 10

            dr.RMAD_PARTSN = UI_txtSerialParts.Text.Trim().ToUpper()
            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0
            dr.RMAD_RECEVSTATUS = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr("isCheck") = "1"
            dtRMADetail.AddRMADetailRow(dr)

            oRequested.AddNew_RMADetail(dr)        '新增品項到資料庫去
            oReceived.UpdateRmaStatus(Me.UI_lblPreviousPage_RMANO.Text.Trim(), Session("_UserID"), Session("_UserName"))
        Catch ex As Exception
            Throw ex

        End Try
        Return dtRMADetail
    End Function

    ''' <summary>
    ''' 品項刪除
    ''' </summary>
    ''' <param name="RMAD_ID"></param>
    ''' <param name="RMAD_No"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal RMAD_ID As String, ByVal RMAD_NO As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        dvRMADetail.RowFilter = "RMAD_ID='" & RMAD_ID.ToString().Trim() & "'"
        If dvRMADetail.Count > 0 Then
            dvRMADetail(0)("RMAD_MARK") = 1
            dvRMADetail(0)("RMAD_RECVDATE") = Date.Now.ToShortDateString()
            dvRMADetail(0)("RMAD_RECEVSTATUS") = 2
        End If
        dvRMADetail.RowFilter = ""

        Session("_dtRMADetail") = dtRMADetail

        '=====================================================================================================================================================================
        'Receive Delete
        '=====================================================================================================================================================================
        Dim oReceived As New ctlRMA.Received
        Dim dtReceive_Del As New RmaDTO.tmpRecvStatusDataTable
        Dim dr As RmaDTO.tmpRecvStatusRow = dtReceive_Del.NewRow

        '是屬與修改資料才執行刪除機制, 新增的品項不需要處理
        If RMAD_NO.Trim <> "" Then
            dr.RMAD_ID = RMAD_ID
            dr.RMAD_NO = RMAD_NO
            dr.RMA_RecvAD = Session("_UserID")
            dr.RMA_RecvName = Session("_UserName")
            dr.RMA_RecvStatus = "2"         '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            dtReceive_Del.AddtmpRecvStatusRow(dr)

            oReceived.DeleteRMADetail(dtReceive_Del)
        End If

        Dim oRMA As New ctlRMA.Requested
        dtRMADetail = oRMA.QueryByRMADetail(RMAD_NO, "")
        Call ArrangementData(dtRMADetail)
        Session("_dtRMADetail") = dtRMADetail
        Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageCount
        Call RMADetail_DataBind(iPageIndex)

        'Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageIndex
        'Call RMADetail_DataBind(iPageIndex)
    End Sub

    ''' <summary>
    ''' RMA單 品項全刪除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim oReceived As New ctlRMA.Received
        Dim dtReceive_Del As New RmaDTO.tmpRecvStatusDataTable

        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        For i = 0 To UI_dvRMADetail.Rows.Count - 1
            If UI_dvRMADetail.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_RMADID As Label = UI_dvRMADetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMADNO As Label = UI_dvRMADetail.Rows(i).FindControl("UI_RMADNO")
                Dim UI_Delcheck As CheckBox = UI_dvRMADetail.Rows(i).FindControl("UI_Delcheck")

                If UI_Delcheck.Checked = True Then
                    dvRMADetail.RowFilter = "RMAD_ID='" & UI_RMADID.Text.ToString().Trim() & "'"
                    If dvRMADetail.Count > 0 Then
                        'dvRMADetail(0)("RMAD_MARK") = 1
                        dvRMADetail(0)("RMAD_RECVDATE") = Date.Now.ToShortDateString()
                        dvRMADetail(0)("RMAD_RECEVSTATUS") = 2
                    End If

                    '=====================================================================================================================================================================
                    'Receive Delete
                    '=====================================================================================================================================================================
                    Dim dr As RmaDTO.tmpRecvStatusRow = dtReceive_Del.NewRow

                    '是屬與修改資料才執行刪除機制, 新增的品項不需要處理
                    If UI_RMADNO.Text.Trim <> "" Then
                        dr.RMAD_ID = UI_RMADID.Text.Trim()
                        dr.RMAD_NO = UI_RMADNO.Text.Trim()
                        dr.RMA_RecvAD = Session("_UserID")
                        dr.RMA_RecvName = Session("_UserName")
                        dr.RMA_RecvStatus = "2"         '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                        dtReceive_Del.AddtmpRecvStatusRow(dr)
                    End If

                End If
            End If
        Next

        If dtReceive_Del.Rows.Count > 0 Then
            oReceived.DeleteRMADetail(dtReceive_Del)
        End If

        dvRMADetail.RowFilter = ""
        Session("_dtRMADetail") = dtRMADetail
        Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageIndex
        Call RMADetail_DataBind(iPageIndex)
    End Sub

    ''' <summary>
    ''' 存檔前整理 RMA 是否有做收貨 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Arrangement_ReceivedData()
        Dim i As Integer = 0
        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        For i = 0 To Me.UI_dvRMADetail.Rows.Count - 1
            If Me.UI_dvRMADetail.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_RMAcheck As CheckBox = Me.UI_dvRMADetail.Rows(i).FindControl("UI_RMAcheck")
                Dim UI_RMADID As Label = Me.UI_dvRMADetail.Rows(i).FindControl("UI_RMADID")

                If UI_RMAcheck.Checked = True Then
                    dvRMADetail.RowFilter = "RMAD_ID='" & UI_RMADID.Text.Trim() & "'"
                    If dvRMADetail.Count > 0 Then
                        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                        dvRMADetail(0)("RMAD_STATUS") = "20"
                        dvRMADetail(0)("RMAD_RECVAD") = Session("_UserID")
                        dvRMADetail(0)("RMAD_RECVADNAME") = Session("_UserName")
                        dvRMADetail(0)("RMAD_RECVDATE") = Date.Now
                        dvRMADetail(0)("RMAD_RECEVSTATUS") = "1"       '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                    End If
                End If
            End If
        Next

        dvRMADetail.RowFilter = ""
        Session("_dtRMADetail") = dtRMADetail
    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        '讓 Progress 等候一下, 再開始處理資料
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call UI_cmdTmpSave_Click(sender, e)
        Call Arrangement_ReceivedData()
        Save()
    End Sub

    ''' <summary>
    ''' 儲存-收貨RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Save()
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim oReceived As New ctlRMA.Received
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim dtReceive As New RmaDTO.tmpRecvStatusDataTable


        Try
            'RMA Table
            dtRMA = Save_RMA()

            '收貨
            dtReceive = Save_ReceivedData()

            oReceived.SaveByReceived(dtRMA, dtReceive)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByAlert(sMessage)
            Else

                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Receive_WorkList.aspx")
            End If
        End Try


    End Sub

    Private Function Save_RMA() As RmaDTO.RMADataTable
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oGuid As Guid = Guid.NewGuid

        Try
            Dim dr As RmaDTO.RMARow = dtRMA.NewRMARow

            dr.RMA_ID = Me.UI_lblPreviousPage_RMAID.Text.Trim()
            dr.RMA_NO = Me.UI_lblPreviousPage_RMANO.Text.Trim()

            dr.RMA_CUNO = Me.UI_lblAccountIDText.Text.ToString().Trim()
            dr.RMA_ACCOUNTID = Me.UI_lblUserIDText.Text.ToString().Trim()
            dr.RMA_APPLICANT = Me.UI_txtApplicant.Text.ToString().Trim()
            dr.RMA_TEL = Me.UI_txtTel.Text.ToString().Trim()
            '' ''dr.RMA_ADDRESS = Me.UI_cboAddress.SelectedItem.Text.ToString().Trim()
            dr.RMA_ADDRESS = Me.UI_txtAddress.Text.Trim()
            dr.RMA_COMPNO = Me.UI_lblRepairCenterValue.Text.ToString().Trim()

            dr.RMA_STATUS = Convert.ToInt16(Me.UI_lblPreviousPage_RMASTATUS.Text)
            dr.RMA_MAIL = Me.UI_txtMail.Text.Trim()

            dr.RMA_AD = Session("_UserID")
            dr.RMA_ADNAME = Session("_UserName")
            dr.RMA_CSTMP = Date.Now
            dr.RMA_LUAD = Session("_UserID")
            dr.RMA_LUADNAME = Session("_UserName")
            dr.RMA_LUSTMP = Date.Now
            dr.RMA_MARK = 0

            dr.RMA_EUCOMPANY = Me.UI_txtEUCompany.Text.Trim()
            dr.RMA_EUTEL = Me.UI_txtEUTel.Text.Trim()
            dr.RMA_EUNAME = Me.UI_txtEUName.Text.Trim()
            dr.RMA_EUMAIL = Me.UI_txtEUMail.Text.Trim()
            dr.RMA_EUADDRESS = Me.UI_txtEUAddress.Text.Trim()

            'dr.RMA_EUCOMPANY = ""
            'dr.RMA_EUTEL = ""
            'dr.RMA_EUNAME = ""
            'dr.RMA_EUMAIL = ""
            'dr.RMA_EUADDRESS = ""

            dtRMA.AddRMARow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMA
    End Function

    Private Function Save_ReceivedData() As RmaDTO.tmpRecvStatusDataTable
        Dim i As Integer = 0
        Dim oReceived As New ctlRMA.Received
        Dim dtReceive As New RmaDTO.tmpRecvStatusDataTable

        For i = 0 To Me.UI_dvRMADetail.Rows.Count - 1
            If Me.UI_dvRMADetail.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_RMADID As Label = UI_dvRMADetail.Rows(i).FindControl("UI_RMADID")
                Dim UI_RMADNO As Label = UI_dvRMADetail.Rows(i).FindControl("UI_RMADNO")
                Dim UI_RMAcheck As CheckBox = UI_dvRMADetail.Rows(i).FindControl("UI_RMAcheck")

                If UI_RMAcheck.Checked = True Then
                    Dim dr As RmaDTO.tmpRecvStatusRow = dtReceive.NewRow

                    dr.RMAD_ID = UI_RMADID.Text.Trim()
                    dr.RMAD_NO = UI_RMADNO.Text.Trim()
                    dr.RMA_RecvAD = Session("_UserID")
                    dr.RMA_RecvName = Session("_UserName")
                    dr.RMA_RecvStatus = "1"         '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

                    dtReceive.AddtmpRecvStatusRow(dr)
                End If
            End If
        Next

        Return dtReceive
    End Function

    ''' <summary>
    ''' Temp Save To
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdTmpSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim bSelect As Boolean = False

        Dim dtTMP As New RmaDTO.RMADetailDataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oExport As New ctlRMA.Export


        Me.UI_cmdSubmit.Enabled = False

        Try
            System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
            dtTMP = Session("_dtRMADetail")

            Dim SNTemp As String = ""
            For i = 0 To UI_dvRMADetail.Rows.Count - 1
                Dim RMAD_SERIALNO As TextBox = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_SERIALNO")
                Dim RMAD_PARTNUMBER As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_PARTNUMBER")
                Dim UI_RMADID As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_RMADID")
                Dim UI_RMAcheck As CheckBox = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_RMAcheck")
                If UI_RMAcheck.Checked Then
                    bSelect = True
                End If

                If RMAD_SERIALNO.Text.ToString() <> "" Then
                    dtTMP.DefaultView.RowFilter = "RMAD_ID<>'" + UI_RMADID.Text.Trim + "' AND RMAD_SERIALNO='" + RMAD_SERIALNO.Text + "' AND RMAD_PARTSN='" + RMAD_PARTNUMBER.Text + "'"
                    If dtTMP.DefaultView.Count > 0 Then
                        Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                    End If

                    If SNTemp.IndexOf(RMAD_SERIALNO.Text.Trim() + RMAD_PARTNUMBER.Text.Trim() + "~@~;") >= 0 Then
                        Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                    Else
                        dtTMP.DefaultView.RowFilter = "RMAD_ID='" + UI_RMADID.Text.Trim + "'"
                        If dtTMP.DefaultView.Count > 0 Then
                            dtTMP.DefaultView(0)("RMAD_SERIALNO") = RMAD_SERIALNO.Text.Trim()
                            dtTMP.DefaultView(0)("isCheck") = "0"
                            If UI_RMAcheck.Checked Then
                                dtTMP.DefaultView(0)("isCheck") = "1"
                            End If
                        End If

                        SNTemp += RMAD_SERIALNO.Text.Trim() + RMAD_PARTNUMBER.Text.Trim() + "~@~;"
                    End If
                Else
                    Throw New ArgumentException(_oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator))
                End If
            Next
            dtTMP.DefaultView.RowFilter = ""
            dtTMP.AcceptChanges()
            Session("_dtRMADetail") = dtTMP

            Dim oRequested As New ctlRMA.Requested
            For i = 0 To dtTMP.Rows.Count - 1
                Dim drTMP As RmaDTO.RMADetailRow = dtTMP.Rows(i)

                If drTMP.IsRMAD_PRODUCTDESCNull = True Then drTMP.RMAD_PRODUCTDESC = ""
                If drTMP.RMAD_RECEVSTATUS = 2 Then drTMP.RMAD_STATUS = "91"

                '==================
                'MODEL NO
                '==================
                If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                    Dim sModelNo As String = oExport.getCModelNo(drTMP.RMAD_MODELNO.ToString().Trim(), Me.UI_lblRepairCenterValue.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.ToString().Trim())
                    If sModelNo.Trim() = "" Then
                        drTMP.RMAD_MODELNO = "OTHER"
                    Else
                        drTMP.RMAD_MODELNO = sModelNo
                    End If
                End If


                '==================
                'WarrantyDate
                '==================
                Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                    'Dim sWarrantyDate As String = oExport.getWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Me.UI_lblAccountIDText.Text.Trim())
                    Dim sWarrantyDate As String = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Me.UI_lblAccountIDText.Text.Trim(), Me.UI_lblRepairCenterValue.Text.Trim())
                    If sWarrantyDate.Trim() <> "" Then
                        sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                    End If
                End If
                If IsDate(sWarranty) = True Then
                    drTMP.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
                Else
                    drTMP.SetRMAD_WARRANTYNull()
                End If

                oRequested.Edit_RMADetail(drTMP)
            Next

            Dim iPageIndex As Integer = Me.UI_dvRMADetail.PageIndex
            Call RMADetail_DataBind(iPageIndex)

            If bSelect = True Then
                Me.UI_cmdSubmit.Enabled = True
            End If
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Private Sub keepCheck()
        Dim i As Integer = 0
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable

        dtRMADetail = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        For i = 0 To Me.UI_dvRMADetail.Rows.Count - 1
            If Me.UI_dvRMADetail.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_RMAcheck As CheckBox = Me.UI_dvRMADetail.Rows(i).FindControl("UI_RMAcheck")
                Dim UI_RMADID As Label = Me.UI_dvRMADetail.Rows(i).FindControl("UI_RMADID")

                dvRMADetail.RowFilter = "RMAD_ID='" & UI_RMADID.Text.ToString().Trim() & "'"
                If dvRMADetail.Count > 0 Then
                    dvRMADetail(0)("isCheck") = "0"
                    If UI_RMAcheck.Checked = True Then
                        dvRMADetail(0)("isCheck") = "1"
                    End If
                End If

            End If
        Next
        dvRMADetail.RowFilter = ""
        Session("_dtRMADetail") = dtRMADetail
    End Sub

End Class

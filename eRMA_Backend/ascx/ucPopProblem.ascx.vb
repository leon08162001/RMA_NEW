Imports System.Data
Imports DataService
Imports DefLanguage
Imports SecurityCrypt



Partial Class ascx_ucPopProblem
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto


    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Requested_FilePath As String = ConfigurationSettings.AppSettings("Requested_FilePath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_isModified") = False
            Me.ViewState("_show") = False
            Me.ViewState("_key") = ""

            Call getModelData()
            Call setControls()
        End If
    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()

        Dim sKey As String = "File=" & Me.html_File.ClientID
        sKey = sKey & "&FullFile=" & Me.html_FullFile.ClientID
        sKey = sKey & "&FilePath=Requested_FilePath"

        sKey = _Crypto.Encrypt(sKey, "")
        Me.ifrmFileUpload.Attributes.Add("src", "FileUpload.aspx?ID=" & sKey)

        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sFarcNameText)

        '取得Tag Text
        Me.UI_lblProblemTittle.Text = _oLanguage.getText("RMA", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        Me.UI_lblProductDesc.Text = _oLanguage.getText("RMA", "196", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerName.Text = _oLanguage.getText("RMA", "014", ctlLanguage.eumType.Tag)

        Me.UI_lblWarranty.Text = _oLanguage.getText("RMA", "015", ctlLanguage.eumType.Tag)

        'Me.UI_radWarranty.Items(0).Text = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_1.ClientID & ")")
        Me.UI_chkWarranty_1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_0.ClientID & ")")

        Me.UI_chkWarranty_C0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_C1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_C0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_C1.ClientID & ")")
        Me.UI_chkWarranty_C1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_C0.ClientID & ")")

        Me.UI_chkWarranty_S0.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_S1.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
        Me.UI_chkWarranty_S0.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_S1.ClientID & ")")
        Me.UI_chkWarranty_S1.Attributes.Add("onclick", "chkWarranty(" & Me.UI_chkWarranty_S0.ClientID & ")")

        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblFile.Text = _oLanguage.getText("RMA", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("RMA", "025", ctlLanguage.eumType.Tag)

        Me.lblFileUpload_Title.Text = _oLanguage.getText("RMA", "048", ctlLanguage.eumType.Tag)
        Me.UI_UploadFileDesc.Text = _oLanguage.getText("RMA", "186", ctlLanguage.eumType.Tag)

        Me.rfv_ProductDesc.ErrorMessage = _oLanguage.getText("RMA", "198", ctlLanguage.eumType.Validator)
        Me.rfv_ProblemDesc.ErrorMessage = _oLanguage.getText("RMA", "199", ctlLanguage.eumType.Validator)
        Me.cv_FailureClass.ErrorMessage = _oLanguage.getText("RMA", "078", ctlLanguage.eumType.Validator)
        Me.cv_Failure.ErrorMessage = _oLanguage.getText("RMA", "079", ctlLanguage.eumType.Validator)

        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirmed.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)
        Me.html_FileUpload_Clear.Text = _oLanguage.getText("Common", "046", ctlLanguage.eumType.Command)
        Me.UI_butDownLoad.Text = _oLanguage.getText("Common", "052", ctlLanguage.eumType.Command)

        Me.UI_WEBURL.Text = _WEBURL
        Me.UI_VisualPath.Text = _Requested_VisualPath


        '開放 是否保固日期內 項目
        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        Me.UI_trWarranty.Visible = False
        Me.UI_trCWarranty.Visible = False
        Me.UI_trSWarranty.Visible = False
        If Session("_Role").ToString().IndexOf("1") <> -1 Then
            Me.UI_trWarranty.Visible = True
            Me.UI_trCWarranty.Visible = True
            Me.UI_trSWarranty.Visible = True
        End If

    End Sub

    ''' <summary>
    ''' 不良原因代碼(下拉式)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboFailureClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFailureClass.SelectedIndexChanged

        If Me.html_FullFile.Text.Trim() <> "" Then
            Dim arrFullFile() As String = Me.html_FullFile.Text.Trim().Split(",")
            Me.html_File.Text = arrFullFile(0)
        End If


        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
		
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)

        Call show("", True, Me.UI_lblRepairNo.Text, Me.UI_lblAccountID.Text.Trim())
    End Sub


    ''' <summary>
    ''' 取得Model 的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getModelData()
        Dim oWarranty As New ctlWarranty
        Dim dtModel As New WarrantyDTO.vwPrdGroupDataTable
        dtModel = oWarranty.QueryPrdGroup("", "")

        Me.UI_cboModel.DataTextField = "GroupName"
        Me.UI_cboModel.DataValueField = "GroupNo"
        Me.UI_cboModel.DataSource = dtModel
        Me.UI_cboModel.DataBind()

        Dim oListItem As ListItem
        oListItem = New ListItem
        oListItem.Text = "OTHER"
        oListItem.Value = "OTHER"
        UI_cboModel.Items.Insert(0, oListItem)

    End Sub




    ''' <summary>
    ''' QueryData
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData()
        Dim i As Integer = 0
        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        Dim oExport As New ctlRMA.Export
        Dim sModelNo As String = ""

        dvRMADetail.RowFilter = "RMAD_ID='" & Me.ViewState("_key").ToString().Trim() & "'"

        If dvRMADetail.Count > 0 Then
            Me.UI_txtSerial.Text = dvRMADetail.Item(0)("RMAD_SERIALNO").ToString().Trim()
            If dvRMADetail.Item(0)("RMAD_PARTSN").ToString().Trim() <> "" Then
                Me.UI_SHOWSERIAL.Text = dvRMADetail.Item(0)("RMAD_PARTSN").ToString().Trim()
            Else
                Me.UI_SHOWSERIAL.Text = UI_txtSerial.Text
            End If

            sModelNo = oExport.getCModelNo(dvRMADetail.Item(0)("RMAD_MODELNO").ToString().Trim(), Me.UI_lblRepairNo.Text.Trim(), Me.UI_lblAccountID.Text)

            If dvRMADetail.Item(0)("RMAD_MODELNO").ToString().Trim() <> "" Then
                Dim oListItem As ListItem = Me.UI_cboModel.Items.FindByValue(sModelNo)
                If oListItem Is Nothing Then
                        Me.UI_cboModel.SelectedValue = "OTHER"
                    Else
                    Me.UI_cboModel.SelectedValue = sModelNo
                End If
                End If

                Me.UI_txtCustomerName.Text = dvRMADetail.Item(0)("RMAD_CUSNAME").ToString().Trim()

                Me.UI_txtProductDesc.Text = dvRMADetail.Item(0)("RMAD_PRODUCTDESC").ToString().Trim()
                Me.UI_txtDescription.Text = dvRMADetail.Item(0)("RMAD_PROBLEMDESC").ToString().Trim()

                If dvRMADetail.Item(0)("RMAD_FARFARCNO").ToString().Trim() <> "" Then
                    Me.UI_cboFailureClass.SelectedValue = dvRMADetail.Item(0)("RMAD_FARFARCNO").ToString().Trim()

                    Call UI_cboFailureClass_SelectedIndexChanged(Me.UI_cboFailure, System.EventArgs.Empty)

                    If dvRMADetail.Item(0)("RMAD_FARNO").ToString().Trim() <> "" And dvRMADetail.Item(0)("RMAD_FARNO").ToString().Trim() <> "NT" Then
                        Me.UI_cboFailure.SelectedValue = dvRMADetail.Item(0)("RMAD_FARNO").ToString().Trim()
                    End If

                Else

                    Me.UI_cboFailureClass.SelectedValue = "-1"
                    Call UI_cboFailureClass_SelectedIndexChanged(Me.UI_cboFailure, System.EventArgs.Empty)
                End If

                Me.UI_chkWarranty_0.Checked = False
                Me.UI_chkWarranty_1.Checked = False
                If dvRMADetail.Item(0)("RMAD_ISWARRANTY").ToString().Trim() = "0" Then
                    Me.UI_chkWarranty_0.Checked = True
                End If
                If dvRMADetail.Item(0)("RMAD_ISWARRANTY").ToString().Trim() = "1" Then
                    Me.UI_chkWarranty_1.Checked = True
                End If

                Me.UI_chkWarranty_C0.Checked = False
                Me.UI_chkWarranty_C1.Checked = False
                If dvRMADetail.Item(0)("rmad_iscw").ToString().Trim() = "0" Then
                    Me.UI_chkWarranty_C0.Checked = True
                End If
                If dvRMADetail.Item(0)("rmad_iscw").ToString().Trim() = "1" Then
                    Me.UI_chkWarranty_C1.Checked = True
                End If

                Me.UI_chkWarranty_S0.Checked = False
                Me.UI_chkWarranty_S1.Checked = False
                If dvRMADetail.Item(0)("rmad_issw").ToString().Trim() = "0" Then
                    Me.UI_chkWarranty_S0.Checked = True
                End If
                If dvRMADetail.Item(0)("rmad_issw").ToString().Trim() = "1" Then
                    Me.UI_chkWarranty_S1.Checked = True
                End If

                '檔案上傳
                Me.html_File.Text = ""
                Me.html_FullFile.Text = ""
                If dvRMADetail.Item(0)("RMAD_UPLOADFILE").ToString().Trim() <> "" Then
                    Me.html_FullFile.Text = dvRMADetail.Item(0)("RMAD_UPLOADFILE").ToString().Trim()
                    Dim arrFileName() As String = Me.html_FullFile.Text.ToString().Split(",")
                    Me.html_File.Text = arrFileName(0)
                    'Me.UI_linkFile.Text = arrFileName(0)
                    'Me.UI_linkFile.NavigateUrl = _WEBURL & _Requested_VisualPath & "\" & arrFileName(1)
                End If
            End If

            dvRMADetail.RowFilter = ""
    End Sub





    ''' <summary>
    ''' 修改Confirmed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirmed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirmed.Click
        Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)

        Dim oRequested As New ctlRMA.Requested
        Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView
        Dim oExport As New ctlRMA.Export

        dvRMADetail.RowFilter = "RMAD_ID='" & Me.ViewState("_key").ToString().Trim() & "'"
        If dvRMADetail.Count > 0 Then
            Dim dr As RmaDTO.RMADetailRow = dvRMADetail(0).Row()

            dr.RMAD_MODELNO = oExport.getMModelNo(Me.UI_cboModel.SelectedValue.Trim(), Me.UI_lblRepairNo.Text.Trim(), Me.UI_lblAccountID.Text)  'Me.UI_cboModel.SelectedValue.Trim()
            dr.RMAD_SERIALNO = Me.UI_txtSerial.Text.ToString().Trim()
            dr.RMAD_CUSNAME = Me.UI_txtCustomerName.Text.ToString().Trim()
            dr.RMAD_FARFARCNO = Me.UI_cboFailureClass.SelectedValue.ToString().Trim()
            dr.RMAD_FARNO = Me.UI_cboFailure.SelectedValue.ToString().Trim()

            '檔案上傳
            dr.RMAD_UPLOADFILE = Me.html_FullFile.Text.Trim
            dr.RMAD_PRODUCTDESC = Me.UI_txtProductDesc.Text.Trim()
            If Me.UI_txtDescription.Text.ToString().Trim() <> "" Then
                dr.RMAD_PROBLEMDESC = Me.UI_txtDescription.Text.ToString().Trim()
            End If

            dr.RMAD_ISFILL = "1"                '是否已填寫問題:0.否, 1.是

            'Warranty
            If Me.UI_txtSerial.Text.ToString().Trim() <> "" Then
                'Dim oExport As New ctlRMA.Export
                Dim sWarrantyDate As String = oExport.getMaxWarranty(Me.UI_txtSerial.Text, Session("_CustomerID").ToString(), Session("_RepairID").ToString())
                If sWarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                End If
            End If

            dr.RMAD_sWARRANTY = sWarranty          'Warranty 字串格式
            If IsDate(sWarranty) = True Then
                dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
            Else
                dr.SetRMAD_WARRANTYNull()
            End If

            dr.SetRMAD_ISWARRANTYNull()
            If Me.UI_chkWarranty_0.Checked = True Then
                dr.RMAD_ISWARRANTY = 0
            End If
            If Me.UI_chkWarranty_1.Checked = True Then
                dr.RMAD_ISWARRANTY = 1
            End If

            dr.Setrmad_iscwNull()
            If Me.UI_chkWarranty_C0.Checked = True Then
                dr.rmad_iscw = 0
            End If
            If Me.UI_chkWarranty_C1.Checked = True Then
                dr.rmad_iscw = 1
            End If

            dr.Setrmad_isswNull()
            If Me.UI_chkWarranty_S0.Checked = True Then
                dr.rmad_issw = 0
            End If
            If Me.UI_chkWarranty_S1.Checked = True Then
                dr.rmad_issw = 1
            End If
            '=========================================================================================================================
            '直接修改資料庫內容
            '前有用到的是 收貨人員修改品項時
            '=========================================================================================================================
            If Convert.ToBoolean(Me.ViewState("_isModified")) = True Then
                oRequested.Edit_RMADetail(dr)
            End If

        End If
        dvRMADetail.RowFilter = ""
        Session("_dtRMADetail") = dtRMADetail


        '=========================================================================================================================
        'UI_dvRMADetail DataBind
        '=========================================================================================================================
        dvRMADetail.RowFilter = "RMAD_MARK=0"
        Dim UI_dvRMADetail As GridView = Me.Parent.FindControl("UI_dvRMADetail")
        dtRMADetail = Session("_dtRMADetail")
        UI_dvRMADetail.DataSource = dvRMADetail
        UI_dvRMADetail.DataBind()

        Me.ViewState("_isModified") = False
        Call show("", False, Me.UI_lblRepairNo.Text, Me.UI_lblAccountID.Text.Trim())
    End Sub











    Public Sub show(ByVal Key As String, ByVal isShow As Boolean, ByVal sRepairNo As String, ByVal sAccountID As String)
        Me.ViewState("_show") = isShow

        Me.UI_flowCase.Text = ""
        Me.UI_lblRepairNo.Text = sRepairNo
        Me.UI_lblAccountID.Text = sAccountID
        If Me.UI_lblRepairNo.Text.Trim <> "" Then
            Call chkFlowCase01()
        End If
        Call setFlowCase01()


        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            If Key.Trim <> "" Then
                Me.ViewState("_key") = Key
                Call QueryData()
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
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
            If Me.UI_lblRepairNo.Text.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
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
        Me.UI_chkWarranty_0.Enabled = True
        Me.UI_chkWarranty_1.Enabled = True
        Me.UI_chkWarranty_C0.Enabled = True
        Me.UI_chkWarranty_C1.Enabled = True
        Me.UI_chkWarranty_S0.Enabled = True
        Me.UI_chkWarranty_S1.Enabled = True

        If Me.UI_flowCase.Text = "01" Then
            Me.UI_chkWarranty_0.Enabled = False
            Me.UI_chkWarranty_1.Enabled = False

            Me.UI_chkWarranty_C0.Enabled = False
            Me.UI_chkWarranty_C1.Enabled = False

            Me.UI_chkWarranty_S0.Enabled = False
            Me.UI_chkWarranty_S1.Enabled = False
        End If

    End Sub











    ''' <summary>
    ''' 設定是否要直接修改資料庫內容
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 Boolean</returns>
    ''' <remarks>
    ''' 目前有用到的是 收貨人員修改品項時
    ''' </remarks>
    Public Property isModified() As Boolean
        Get
            Return Convert.ToBoolean(Me.ViewState("_isModified"))
        End Get

        Set(ByVal nNewValue As Boolean)
            Me.ViewState("_isModified") = nNewValue
        End Set
    End Property






End Class

Imports DataService
Imports System.Data
Imports DefLanguage


Partial Class ascx_ucCustomerAdmin
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")


    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Session("_dtCustomerUser") = Nothing

            Dim UI_lblPreviousPage_CuNo As Label = Me.Parent.FindControl("UI_lblPreviousPage_CuNo")
            Dim UI_lblPreviousPage_Role As Label = Me.Parent.FindControl("UI_lblPreviousPage_Role")

            Me.ViewState("_CuNo") = UI_lblPreviousPage_CuNo.Text.Trim()
            Me.ViewState("_Role") = UI_lblPreviousPage_Role.Text.Trim()

            Me.ViewState("_eumCommand") = eumCommand.AddNew
            If Me.ViewState("_CuNo").ToString().Trim() <> "" Then
                Me.ViewState("_eumCommand") = eumCommand.UPDATE
            End If

            Call setControls()

            If Me.ViewState("_eumCommand") = eumCommand.UPDATE Then
                Call QueryByCustomer()
                Call QueryByUser(0)
            End If
        End If

    End Sub
#End Region


    Private Sub setControls()

        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getCountryByDropDownList(Me.UI_cboCountry, sRepairText)
        oCommon.getRepairCenteryByDropDownList(False, Me.UI_cboRepairCenter, sRepairText)


        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Customer", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerName.Text = _oLanguage.getText("Customer", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerID.Text = _oLanguage.getText("Customer", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblCountry.Text = _oLanguage.getText("Customer", "013", ctlLanguage.eumType.Tag)

        Me.UI_lblSalesID.Text = _oLanguage.getText("Customer", "014", ctlLanguage.eumType.Tag)
        Me.UI_lblAssistantID.Text = _oLanguage.getText("Customer", "015", ctlLanguage.eumType.Tag)

        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Customer", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblContactPerson.Text = _oLanguage.getText("Customer", "016", ctlLanguage.eumType.Tag)

        Me.UI_lblisChoice.Text = _oLanguage.getText("Customer", "042", ctlLanguage.eumType.Tag)
        Me.UI_radisChoice.Items(0).Text = _oLanguage.getText("Customer", "028", ctlLanguage.eumType.Tag)
        Me.UI_radisChoice.Items(1).Text = _oLanguage.getText("Customer", "027", ctlLanguage.eumType.Tag)

        '新增服務費折讓開關功能 by buck Add 20260427 begin
        Me.UI_CU_SERVICE_CHG_DISCOUNT.Text = _oLanguage.getText("Customer", "044", ctlLanguage.eumType.Tag)
        Me.UITxt_CU_SERVICE_CHG_DISCOUNT.Items(0).Text = _oLanguage.getText("Customer", "045", ctlLanguage.eumType.Tag)
        Me.UITxt_CU_SERVICE_CHG_DISCOUNT.Items(1).Text = _oLanguage.getText("Customer", "046", ctlLanguage.eumType.Tag)
        '新增服務費折讓開關功能 by buck Add 20260427 end

        Me.UI_lblTEL.Text = _oLanguage.getText("Customer", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress1.Text = _oLanguage.getText("Customer", "017", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress2.Text = _oLanguage.getText("Customer", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress3.Text = _oLanguage.getText("Customer", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress4.Text = _oLanguage.getText("Customer", "020", ctlLanguage.eumType.Tag)
        Me.UI_lblEMail.Text = _oLanguage.getText("Customer", "021", ctlLanguage.eumType.Tag)
        Me.UI_lblFinanceEMail.Text = _oLanguage.getText("Customer", "043", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Customer", "022", ctlLanguage.eumType.Tag)

        Me.UI_opgStatus.Items(0).Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
        Me.UI_opgStatus.Items(1).Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)

        Me.UI_lblInformationTittle.Text = _oLanguage.getText("Customer", "023", ctlLanguage.eumType.Tag)

        Me.UI_cmdSubmit_Company.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "009", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)

        Me.rfvCusName.ErrorMessage = _oLanguage.getText("Customer", "030", ctlLanguage.eumType.Validator)
        Me.rfvCustomerID.ErrorMessage = _oLanguage.getText("Customer", "031", ctlLanguage.eumType.Validator)
        Me.rfvSalesID.ErrorMessage = _oLanguage.getText("Customer", "032", ctlLanguage.eumType.Validator)
        Me.cvCountry.ErrorMessage = _oLanguage.getText("Customer", "033", ctlLanguage.eumType.Validator)
        Me.cvRepairCenter.ErrorMessage = _oLanguage.getText("Customer", "034", ctlLanguage.eumType.Validator)
        'Me.revEMail.ErrorMessage = _oLanguage.getText("Customer", "035", ctlLanguage.eumType.Validator)

        Select Case Me.ViewState("_Role").ToString().Trim().ToLower()
            Case "Admin".ToLower()
                Me.UI_txtCusName.Visible = True
                Me.UI_txtCustomerID.Visible = True
                Me.UI_trSalesID1.Visible = True
                Me.UI_trSalesID2.Visible = True
                Me.UI_trAssistantID1.Visible = True
                Me.UI_trAssistantID2.Visible = True
                Me.UI_cboRepairCenter.Visible = True
                Me.UI_opgStatus.Visible = True

            Case "Manager".ToLower()
                Me.UI_lblCustomerNameText.Visible = True
                Me.UI_lblCustomerIDText.Visible = True
                Me.UI_trSalesID1.Visible = False
                Me.UI_trSalesID2.Visible = False
                Me.UI_trAssistantID1.Visible = False
                Me.UI_trAssistantID2.Visible = False
                Me.UI_lblRepairCenterText.Visible = True
                Me.UI_lblStatusText.Visible = True

                Me.cvRepairCenter.Visible = False
        End Select

        Me.UI_txtCustomerID.Visible = True
        Me.UI_lblCustomerIDText.Visible = False
        Me.UI_trInformationTittle.Visible = False
        Me.UI_trCustomerUser.Visible = False
        Me.UI_trSubmit.Visible = False
        Me.UITxt_CU_TIPTOP_ID.ReadOnly = False

        If Me.ViewState("_eumCommand") = eumCommand.UPDATE Then
            Me.UI_txtCustomerID.Visible = False
            Me.UI_lblCustomerIDText.Visible = True
            Me.UI_trInformationTittle.Visible = True
            Me.UI_trCustomerUser.Visible = True
            Me.UI_trSubmit.Visible = True
            Me.rfvCustomerID.Visible = False
        End If

    End Sub



    Private Sub QueryByCustomer()
        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.CustomerDataTable

        dtCustomer = oCustomer.QueryByPrimaryKey(Me.ViewState("_CuNo").ToString().Trim())
        If dtCustomer.Count > 0 Then
            Dim dr As CustomerDTO.CustomerRow = dtCustomer.Rows(0)

            Me.UI_lblCustomerNameText.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_txtCusName.Text = dr.CU_NAME.ToString().Trim()

            Me.UI_lblCustomerIDText.Text = dr.CU_NO.ToString().Trim()
            Me.UI_txtCustomerID.Text = dr.CU_NO.ToString().Trim()

            Me.UI_cboCountry.SelectedValue = dr.CU_COUNTRYID.ToString().Trim()

            Me.UI_cboRepairCenter.SelectedValue = dr.CU_COMPNO.ToString().Trim()
            Me.UI_lblRepairCenterValue.Text = dr.CU_COMPNO.ToString().Trim()
            Me.UI_lblRepairCenterText.Text = Me.UI_cboRepairCenter.Items(Me.UI_cboRepairCenter.SelectedIndex).Text.Trim()

            Me.UI_txtSalesID.Text = dr.CU_SALESID.ToString().Trim()

            Me.UI_txtAssistantID.Text = ""
            If dr.IsCU_ASSISTANTIDNull = False Then Me.UI_txtAssistantID.Text = dr.CU_ASSISTANTID.ToString().Trim()

            Me.UI_txtContactPerson.Text = ""
            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_txtContactPerson.Text = dr.CU_CONTACTPERSON.ToString().Trim()

            Me.UI_txtTEL.Text = ""
            If dr.IsCU_TELNull = False Then Me.UI_txtTEL.Text = dr.CU_TEL.ToString().Trim()

            '增加Discount off & service charge MODI BY Angel ON 2016/03/23
            Me.UITxt_CU_SERVICE_CHG.Text = ""
            If dr.IsCU_SERVICE_CHGNull = False Then Me.UITxt_CU_SERVICE_CHG.Text = dr.CU_SERVICE_CHG.ToString().Trim()
            Me.UITxt_CU_SERVICE_CHG_DISCOUNT.SelectedValue = dr.CU_SERVICE_CHG_DISCOUNT.ToString().Trim() '新增服務費折讓開關功能 by buck Add 20260427 begin

            Me.UITxt_CU_DISCOUNT_OFF.Text = ""
            If dr.IsCU_DISCOUNT_OFFNull = False Then Me.UITxt_CU_DISCOUNT_OFF.Text = dr.CU_DISCOUNT_OFF.ToString().Trim()

            Me.UI_txtAddress1.Text = ""
            If dr.IsCU_ADDRESS1Null = False Then Me.UI_txtAddress1.Text = dr.CU_ADDRESS1.ToString().Trim()

            Me.UI_txtAddress2.Text = ""
            If dr.IsCU_ADDRESS2Null = False Then Me.UI_txtAddress2.Text = dr.CU_ADDRESS2.ToString().Trim()

            Me.UI_txtAddress3.Text = ""
            If dr.IsCU_ADDRESS3Null = False Then Me.UI_txtAddress3.Text = dr.CU_ADDRESS3.ToString().Trim()

            Me.UI_txtAddress4.Text = ""
            If dr.IsCU_ADDRESS4Null = False Then Me.UI_txtAddress4.Text = dr.CU_ADDRESS4.ToString().Trim()

            Me.UI_txtEMail.Text = ""
            If dr.IsCU_EMAILNull = False Then Me.UI_txtEMail.Text = dr.CU_EMAIL.ToString().Trim()
            Me.UI_txtFinanceEMail.Text = ""
            If dr.IsCU_FINANCEEMAILNull = False Then Me.UI_txtFinanceEMail.Text = dr.CU_FINANCEEMAIL.ToString().Trim()

            Me.UI_opgStatus.SelectedValue = dr.CU_STATUS.ToString().Trim()
            Me.UI_lblStatusValue.Text = dr.CU_STATUS.ToString().Trim()
            Me.UI_lblStatusText.Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)   'Close
            If dr.CU_STATUS.ToString().Trim() = "1" Then        'Open
                Me.UI_lblStatusText.Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
            End If

            Me.UI_radisChoice.SelectedValue = dr.CU_ISCHOICE.ToString().Trim()

            '20151208移除dr.CU_TIPTOP_ID存入UITxt_CU_TIPTOP_ID
            'If dr.IsCU_TIPTOP_IDNull = False Then Me.UITxt_CU_TIPTOP_ID.Text = dr.CU_TIPTOP_ID.ToString().Trim()
            '20151216 顯示TiptopID,但不能修改
            Try
                Me.UITxt_CU_TIPTOP_ID.Text = dr.CU_TIPTOP_ID.ToString().Trim()
            Catch ex As Exception
                Me.UITxt_CU_TIPTOP_ID.Text = ""
            End Try


        End If

    End Sub




    Private Sub QueryByUser(ByVal iPageIndex As Integer)
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        dtCustomerUser = oCustomerUser.Query(Me.ViewState("_CuNo").ToString().Trim())
        Me.UI_cmdAdd.Visible = True
        If dtCustomerUser.Rows.Count > 0 Then
            Me.UI_cmdAdd.Visible = False
        End If

        Call CustomerUser_DataBind(dtCustomerUser, iPageIndex)

    End Sub

    Private Sub CustomerUser_DataBind(ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtCustomerUser)

        Session("_dtCustomerUser") = dtCustomerUser

        Me.UI_CustomerUser.DataSource = dtCustomerUser.DefaultView
        Me.UI_CustomerUser.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable)
        Dim i As Integer = 0

        If dtCustomerUser.Columns("SeqID") Is Nothing Then
            dtCustomerUser.Columns.Add("SeqID")
        End If

        For i = 0 To dtCustomerUser.Rows.Count - 1
            dtCustomerUser.Rows(i)("SeqID") = i + 1
        Next
    End Sub

    Protected Sub UI_CustomerUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_CustomerUser.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Customer", "024", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Customer", "025", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Customer", "011", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Customer", "021", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Customer", "005", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Customer", "041", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_oldAccountID As Label = e.Row.FindControl("UI_oldAccountID")
            Dim UI_AccountID As TextBox = e.Row.FindControl("UI_AccountID")
            Dim UI_lblAccountID As Label = e.Row.FindControl("UI_lblAccountID")
            Dim UI_EMail As TextBox = e.Row.FindControl("UI_EMail")
            Dim UI_FinanceEMail As TextBox = e.Row.FindControl("UI_FinanceEMail")


            If UI_oldAccountID.Text.Trim() <> "" Then
                UI_AccountID.Visible = False
                UI_lblAccountID.Visible = True
            Else
                UI_AccountID.Visible = True
                UI_lblAccountID.Visible = False
            End If

            Dim UI_ISStatus As Label = e.Row.FindControl("UI_ISStatus")
            Dim UI_Status As DropDownList = e.Row.FindControl("UI_Status")
            UI_Status.Items(0).Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
            UI_Status.Items(1).Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)
            UI_Status.SelectedValue = UI_ISStatus.Text.Trim()

            Dim UI_ISMANAGER As Label = e.Row.FindControl("UI_ISMANAGER")
            Dim UI_Manager As DropDownList = e.Row.FindControl("UI_Manager")
            UI_Manager.Items(0).Text = _oLanguage.getText("Customer", "027", ctlLanguage.eumType.Tag)
            UI_Manager.Items(1).Text = _oLanguage.getText("Customer", "028", ctlLanguage.eumType.Tag)
            'UI_Manager.SelectedValue = UI_ISMANAGER.Text.Trim()
            UI_Manager.SelectedValue = 0

            Dim UI_lblManager As Label = e.Row.FindControl("UI_lblManager")
            If UI_ISMANAGER.Text.Trim() = "1" Then
                UI_lblManager.Text = _oLanguage.getText("Customer", "027", ctlLanguage.eumType.Tag)
            Else
                UI_lblManager.Text = _oLanguage.getText("Customer", "028", ctlLanguage.eumType.Tag)
            End If


            Dim UI_Pwd As TextBox = e.Row.FindControl("UI_Pwd")
            Dim rfv_AccountID As RequiredFieldValidator = e.Row.FindControl("rfv_AccountID")
            Dim rfv_Password As RequiredFieldValidator = e.Row.FindControl("rfv_Password")
            Dim revUIEMail_1 As RegularExpressionValidator = e.Row.FindControl("revUIEMail_1")
            Dim revUIEMail_2 As RequiredFieldValidator = e.Row.FindControl("revUIEMail_2")

            rfv_AccountID.ControlToValidate = UI_AccountID.ID
            rfv_Password.ControlToValidate = UI_Pwd.ID
            revUIEMail_1.ControlToValidate = UI_EMail.ID
            revUIEMail_2.ControlToValidate = UI_EMail.ID

            rfv_AccountID.ErrorMessage = _oLanguage.getText("Customer", "036", ctlLanguage.eumType.Validator)
            rfv_Password.ErrorMessage = _oLanguage.getText("Customer", "037", ctlLanguage.eumType.Validator)
            revUIEMail_1.ErrorMessage = _oLanguage.getText("Customer", "035", ctlLanguage.eumType.Validator)
            revUIEMail_2.ErrorMessage = _oLanguage.getText("Customer", "035", ctlLanguage.eumType.Validator)
        End If

    End Sub


    ''' <summary>
    ''' 紀錄 CustomerUser 裡的資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Keep_CustomerUser_Data()
        Dim i As Integer = 0
        Dim dtCustomerUser As CustomerDTO.CustomerUserDataTable = Session("_dtCustomerUser")

        Dim dvCustomerUser As DataView = dtCustomerUser.DefaultView
        For i = 0 To Me.UI_CustomerUser.Rows.Count - 1

            If Me.UI_CustomerUser.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_SeqID As Label = Me.UI_CustomerUser.Rows(i).FindControl("UI_SeqID")

                Dim UI_AccountID As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_AccountID")
                Dim UI_Pwd As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_Pwd")
                Dim UI_Tel As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_Tel")
                Dim UI_EMail As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_EMail")
                Dim UI_FinanceEMail As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_FinanceEMail")

                Dim UI_Status As DropDownList = Me.UI_CustomerUser.Rows(i).FindControl("UI_Status")

                Dim UI_Manager As DropDownList = Me.UI_CustomerUser.Rows(i).FindControl("UI_Manager")

                dvCustomerUser.RowFilter = "SeqID='" & UI_SeqID.Text.Trim() & "'"
                If dvCustomerUser.Count > 0 Then
                    Dim dr As CustomerDTO.CustomerUserRow = dvCustomerUser(0).Row

                    dr.CUUS_ACCOUNTID = UI_AccountID.Text.Trim()
                    dr.CUUS_PWD = UI_Pwd.Text.Trim()
                    dr.CUUS_TEL = UI_Tel.Text.Trim()
                    dr.CUUS_EMAIL = UI_EMail.Text.Trim()

                    dr.CUUS_STATUS = UI_Status.SelectedValue              '帳號狀態 (1:開啟 , 0:關閉)
                    dr.CUUS_ISMANAGER = UI_Manager.SelectedValue           '是否為管理人(0.否,1.是)
                End If
            End If
        Next
        dvCustomerUser.RowFilter = ""
        Session("_dtCustomerUser") = dtCustomerUser
    End Sub

    



    ''' <summary>
    ''' 公司確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Company_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit_Company.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.CustomerDataTable

        Try
            Dim dr As CustomerDTO.CustomerRow = dtCustomer.NewCustomerRow


            dr.CU_NO = Me.UI_txtCustomerID.Text.Trim()
            dr.CU_NAME = Me.UI_txtCusName.Text.Trim()
            dr.CU_COUNTRYID = Me.UI_cboCountry.SelectedValue.Trim()


            dr.CU_TEL = Me.UI_txtTEL.Text.Trim()
            '增加Discount off & service charge MODI BY Angel ON 2016/03/23
            If String.IsNullOrEmpty(Me.UITxt_CU_SERVICE_CHG.Text) Then
                dr.CU_SERVICE_CHG = 0
            Else
                dr.CU_SERVICE_CHG = Convert.ToDouble(Me.UITxt_CU_SERVICE_CHG.Text.Trim())
            End If
            dr.CU_SERVICE_CHG_DISCOUNT = Me.UITxt_CU_SERVICE_CHG_DISCOUNT.SelectedValue '新增服務費折讓開關功能 by buck Add 20260427 begin

            If String.IsNullOrEmpty(Me.UITxt_CU_DISCOUNT_OFF.Text) Then
                dr.CU_DISCOUNT_OFF = 0
            Else
                'dr.CU_DISCOUNT_OFF = Convert.ToInt16(Me.UITxt_CU_DISCOUNT_OFF.Text.Trim())
                dr.CU_DISCOUNT_OFF = Math.Truncate(Convert.ToDecimal(Me.UITxt_CU_DISCOUNT_OFF.Text.Trim()) * 100D) / 100D
            End If


            dr.CU_ADDRESS1 = Me.UI_txtAddress1.Text.Trim()
            dr.CU_ADDRESS2 = Me.UI_txtAddress2.Text.Trim()
            dr.CU_ADDRESS3 = Me.UI_txtAddress3.Text.Trim()
            dr.CU_ADDRESS4 = Me.UI_txtAddress4.Text.Trim()
            dr.CU_EMAIL = Me.UI_txtEMail.Text.Trim()
            dr.CU_FINANCEEMAIL = Me.UI_txtFinanceEMail.Text.Trim()
            dr.CU_CONTACTPERSON = Me.UI_txtContactPerson.Text.Trim()

            dr.CU_SALESID = Me.UI_txtSalesID.Text.Trim()
            dr.CU_ASSISTANTID = Me.UI_txtAssistantID.Text.Trim()

            Select Case Me.ViewState("_Role").ToString().Trim().ToLower()
                Case "admin".ToLower()
                    dr.CU_COMPNO = Me.UI_cboRepairCenter.SelectedValue.Trim()           'admin
                    dr.CU_STATUS = Me.UI_opgStatus.SelectedValue

                Case "Manager".ToLower()
                    dr.CU_COMPNO = Me.UI_lblRepairCenterValue.Text.ToString().Trim()    'Manager
                    dr.CU_STATUS = Me.UI_lblStatusValue.Text.ToString().Trim()           'Manager   
            End Select

            dr.CU_ISCHOICE = Me.UI_radisChoice.SelectedValue.Trim()
            dr.CU_TIPTOP_ID = Me.UITxt_CU_TIPTOP_ID.Text.Trim()

            dr.CU_AD = Session("_UserID")
            dr.CU_ADNAME = Session("_UserName")
            dr.CU_CSTMP = Date.Now
            dr.CU_LUAD = Session("_UserID")
            dr.CU_LUADNAME = Session("_UserName")
            dr.CU_LUSTMP = Date.Now


            dtCustomer.AddCustomerRow(dr)

            If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
                If (String.IsNullOrEmpty(dr.CU_TIPTOP_ID)) Then
                    dr.CU_TIPTOP_ID = "ME363"
                End If
                oCustomer.SaveAdd(dtCustomer)
            Else
                oCustomer.SaveEdit(dtCustomer)
            End If


            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
                    Me.ucMessage.showMessageByAlert(oCommon.getMessage(Common.enmMessage.AddOK))
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                    Me.ViewState("_CuNo") = Me.UI_txtCustomerID.Text.Trim()
                    Call setControls()
                    Call QueryByCustomer()
                Else
                    Me.ucMessage.showMessageByAlert(oCommon.getMessage(Common.enmMessage.EditOK))
                End If
            End If
        End Try
    End Sub



    ''' <summary>
    ''' 新增使用者
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_dtCustomerUser") Is Nothing = True Then
            Session("_dtCustomerUser") = dtCustomerUser
        Else
            dtCustomerUser = Session("_dtCustomerUser")
        End If


        Call Keep_CustomerUser_Data()

        Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.NewCustomerUserRow

        dr.CUUS_CUID = Me.ViewState("_CuNo").ToString().Trim()
        dr.CUUS_ACCOUNTID = ""
        dr.CUUS_oldACCOUNTID = ""
        dr.CUUS_PWD = ""
        dr.CUUS_TEL = ""
        dr.CUUS_ADDRESS = ""
        dr.CUUS_STATUS = 1              '帳號狀態 (1:開啟 , 0:關閉)
        dr.CUUS_ISMANAGER = 0           '是否為管理人(0.否,1.是)

        dr.CUUS_AD = Session("_UserID")
        dr.CUUS_ADNAME = Session("_UserName")
        dr.CUUS_CSTMP = Date.Now
        dr.CUUS_LUAD = Session("_UserID")
        dr.CUUS_LUADNAME = Session("_UserName")
        dr.CUUS_LUSTMP = Date.Now

        dtCustomerUser.AddCustomerUserRow(dr)

        Call CustomerUser_DataBind(dtCustomerUser, 0)
    End Sub

    ''' <summary>
    ''' 取消
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Call setControls()
        Call QueryByCustomer()
        Call QueryByUser(0)
    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oCustomerUser As New ctlCustomer.CustomerUser

        Try
            Call Keep_CustomerUser_Data()


            If Not Session("_dtCustomerUser") Is Nothing Then
                Dim dtCustomerUser As CustomerDTO.CustomerUserDataTable = Session("_dtCustomerUser")
                oCustomerUser.Save(dtCustomerUser)
            End If
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Me.ucMessage.showMessageByAlert(oCommon.getMessage(Common.enmMessage.EditOK))
                Call QueryByUser(0)
            End If
        End Try



    End Sub





End Class

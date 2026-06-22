Imports DataService
Imports System.Data
Imports DefLanguage


Partial Class ascx_ucCustomerUser
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")


#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtCustomerUser_User") = Nothing

            Dim UI_lblPreviousPage_CuNo As Label = Me.Parent.FindControl("UI_lblPreviousPage_CuNo")
            Dim UI_lblPreviousPage_CuusID As Label = Me.Parent.FindControl("UI_lblPreviousPage_CuusID")

            Me.UI_lblPreviousPage_CuNo.Text = UI_lblPreviousPage_CuNo.Text.ToString().Trim()
            Me.UI_lblPreviousPage_CuusID.Text = UI_lblPreviousPage_CuusID.Text.ToString().Trim()

            Call setControls()
            Call QueryByCustomer()
            Call QueryByUser(0)
        End If
    End Sub
#End Region


    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Customer", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerName.Text = _oLanguage.getText("Customer", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomerID.Text = _oLanguage.getText("Customer", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblCountry.Text = _oLanguage.getText("Customer", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("Customer", "009", ctlLanguage.eumType.Tag)
        Me.UI_lblContactPerson.Text = _oLanguage.getText("Customer", "016", ctlLanguage.eumType.Tag)
        Me.UI_lblTEL.Text = _oLanguage.getText("Customer", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress1.Text = _oLanguage.getText("Customer", "017", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress2.Text = _oLanguage.getText("Customer", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress3.Text = _oLanguage.getText("Customer", "019", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress4.Text = _oLanguage.getText("Customer", "020", ctlLanguage.eumType.Tag)
        Me.UI_lblEMail.Text = _oLanguage.getText("Customer", "021", ctlLanguage.eumType.Tag)
        Me.UI_lblStatus.Text = _oLanguage.getText("Customer", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblInformationTittle.Text = _oLanguage.getText("Customer", "023", ctlLanguage.eumType.Tag)

        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryByCustomer()
        Dim oCountry As New ctlCountry
        Dim oCompany As New ctlCompany


        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.CustomerDataTable

        dtCustomer = oCustomer.QueryByPrimaryKey(Me.UI_lblPreviousPage_CuNo.Text.Trim())
        If dtCustomer.Count > 0 Then
            Dim dr As CustomerDTO.CustomerRow = dtCustomer.Rows(0)

            Me.UI_lblCustomerNameText.Text = dr.CU_NAME.ToString().Trim()

            Me.UI_lblCustomerIDText.Text = dr.CU_NO.ToString().Trim()

            '國家名稱
            Me.UI_lblCountryText.Text = oCountry.getCountryName(dr.CU_COUNTRYID.ToString().Trim())

            'RepairCenter
            Me.UI_lblRepairCenterText.Text = oCompany.getRepairName(dr.CU_COMPNO.ToString().Trim())

            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_lblContactPersonText.Text = dr.CU_CONTACTPERSON.ToString().Trim()

            If dr.IsCU_TELNull = False Then Me.UI_lblTELText.Text = dr.CU_TEL.ToString().Trim()

            If dr.IsCU_ADDRESS1Null = False Then Me.UI_lblAddress1Text.Text = dr.CU_ADDRESS1.ToString().Trim()

            If dr.IsCU_ADDRESS2Null = False Then Me.UI_lblAddress2Text.Text = dr.CU_ADDRESS2.ToString().Trim()

            If dr.IsCU_ADDRESS3Null = False Then Me.UI_lblAddress3Text.Text = dr.CU_ADDRESS3.ToString().Trim()

            If dr.IsCU_ADDRESS4Null = False Then Me.UI_lblAddress4Text.Text = dr.CU_ADDRESS4.ToString().Trim()

            If dr.IsCU_EMAILNull = False Then Me.UI_lblEMailText.Text = dr.CU_EMAIL.ToString().Trim()


            Me.UI_lblStatusText.Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)   'Close
            If dr.CU_STATUS.ToString().Trim() = "1" Then        'Open
                Me.UI_lblStatusText.Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
            End If

        End If
    End Sub




    Private Sub QueryByUser(ByVal iPageIndex As Integer)
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        dtCustomerUser = oCustomerUser.QueryUser(Me.UI_lblPreviousPage_CuNo.Text.Trim(), Me.UI_lblPreviousPage_CuusID.Text.Trim())

        Call CustomerUser_DataBind(dtCustomerUser, iPageIndex)

    End Sub

    Private Sub CustomerUser_DataBind(ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtCustomerUser)

        Session("_dtCustomerUser_User") = dtCustomerUser

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
            Dim UI_ISMANAGER As Label = e.Row.FindControl("UI_ISMANAGER")
            Dim UI_EMail As TextBox = e.Row.FindControl("UI_EMail")

            If UI_oldAccountID.Text.Trim() <> "" Then
                UI_AccountID.Visible = False
                UI_lblAccountID.Visible = True
            Else
                UI_AccountID.Visible = True
                UI_lblAccountID.Visible = False
            End If

            Dim UI_lblStatus As Label = e.Row.FindControl("UI_lblStatus")
            Dim UI_ISStatus As Label = e.Row.FindControl("UI_ISStatus")
            Dim UI_Status As DropDownList = e.Row.FindControl("UI_Status")
            UI_Status.Items(0).Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
            UI_Status.Items(1).Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)
            UI_Status.SelectedValue = UI_ISStatus.Text.Trim()

            '是否為管理人(0.否,1.是)
            If UI_ISMANAGER.Text.Trim = "1" Then
                UI_Status.Visible = True
            Else
                UI_lblStatus.Visible = True
                If UI_ISStatus.Text.Trim() = "0" Then
                    UI_lblStatus.Text = _oLanguage.getText("Customer", "007", ctlLanguage.eumType.Tag)
                Else
                    UI_lblStatus.Text = _oLanguage.getText("Customer", "008", ctlLanguage.eumType.Tag)
                End If
            End If

            '是否為管理人(0.否,1.是)
            Dim UI_Manager As Label = e.Row.FindControl("UI_Manager")
            If UI_ISMANAGER.Text.Trim = "1" Then
                UI_Manager.Text = _oLanguage.getText("Customer", "027", ctlLanguage.eumType.Tag)
            Else
                UI_Manager.Text = _oLanguage.getText("Customer", "028", ctlLanguage.eumType.Tag)
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
        Dim dtCustomerUser As CustomerDTO.CustomerUserDataTable = Session("_dtCustomerUser_User")

        Dim dvCustomerUser As DataView = dtCustomerUser.DefaultView
        For i = 0 To Me.UI_CustomerUser.Rows.Count - 1

            If Me.UI_CustomerUser.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_SeqID As Label = Me.UI_CustomerUser.Rows(i).FindControl("UI_SeqID")

                Dim UI_AccountID As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_AccountID")
                Dim UI_Pwd As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_Pwd")
                Dim UI_Tel As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_Tel")
                Dim UI_EMail As TextBox = Me.UI_CustomerUser.Rows(i).FindControl("UI_EMail")

                Dim UI_Status As DropDownList = Me.UI_CustomerUser.Rows(i).FindControl("UI_Status")

                dvCustomerUser.RowFilter = "SeqID='" & UI_SeqID.Text.Trim() & "'"
                If dvCustomerUser.Count > 0 Then
                    Dim dr As CustomerDTO.CustomerUserRow = dvCustomerUser(0).Row

                    dr.CUUS_ACCOUNTID = UI_AccountID.Text.Trim()
                    dr.CUUS_PWD = UI_Pwd.Text.Trim()
                    dr.CUUS_TEL = UI_Tel.Text.Trim()
                    dr.CUUS_EMAIL = UI_EMail.Text.Trim()
                    dr.CUUS_STATUS = UI_Status.SelectedValue              '帳號狀態 (1:開啟 , 0:關閉)
                End If
            End If
        Next
        dvCustomerUser.RowFilter = ""
        Session("_dtCustomerUser_User") = dtCustomerUser
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
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oCustomerUser As New ctlCustomer.CustomerUser

        Try
            Call Keep_CustomerUser_Data()

            If Not Session("_dtCustomerUser_User") Is Nothing Then
                Dim dtCustomerUser As CustomerDTO.CustomerUserDataTable = Session("_dtCustomerUser_User")
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

Imports System.IO
Imports DataService
Imports DefLanguage

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim _oComm As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage
    Dim _ctlLanguage As New ctlLanguage

    Dim _isDebug = ConfigurationManager.AppSettings("isDebug")
    Dim _isDebugSIT = ConfigurationManager.AppSettings("isDebugSIT")

    ''' <summary>
    ''' 取得語系資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryLanguage()

        Try


            Dim dtLanguage As New LanguageDTO.DEFLANGUAGEDataTable
            dtLanguage = _ctlLanguage.QueryByDefLanguage()

            Me.UI_cboLanguage.DataTextField = "DFL_NAME"
            Me.UI_cboLanguage.DataValueField = "DFL_NO"

            Me.UI_cboLanguage.DataSource = dtLanguage.DefaultView
            Me.UI_cboLanguage.DataBind()

            Dim Language_load As New ListItem
            Language_load.Text = "請選擇語系"
            Language_load.Value = 0
            Me.UI_cboLanguage.Items.Add(Language_load)
            Me.UI_cboLanguage.SelectedValue = 0

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            Me.UI_txtPassword.Attributes.Add("AutoComplete", "Off")

            Call LoginOut()
            Call QueryLanguage()

            If Convert.ToBoolean(Session("_isTimeOut")) = True Then
                Me.UI_lblMessage.Visible = True
                Me.UI_lblMessage.Text = _oLanguage.getText("Common", "050", ctlLanguage.eumType.Tag)
            End If

            Me.UI_linkForget.Text = _oLanguage.getText("Common", "053", ctlLanguage.eumType.Tag)
            Me.UI_linkRigister.PostBackUrl = "https://e-rma.cipherlab.com.tw/RMA/View/RMA/RegisterUser.aspx" 'JW/shaili/20150810
            'Me.CheckBox1.Checked = True

            If _isDebug = True Then
                UI_txtAccountID.Text = "admin"
                UI_txtPassword.Text = "AdminRMB"
                CheckBox1.Checked = True

            End If
        End If

    End Sub

    Protected Sub UI_linkEndUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_linkEndUser.Click
        Dim buffer As Byte() = File.ReadAllBytes(Server.MapPath("~/FILE/Sample/EndUser_UserGuide_2015.pdf"))
        Dim ms As New MemoryStream(buffer)
        ms.Position = 0
        ms.Read(buffer, 0, Integer.Parse(ms.Length.ToString()))
        Response.Clear()
        Response.AddHeader("Content-Disposition", ("attachment;filename=End User Guide.pdf"))
        Response.AddHeader("Content-Length", ms.Length.ToString())
        Response.ContentType = "application/pdf"
        ms.Close()
        Response.BinaryWrite(buffer)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
    End Sub

    ''' <summary>
    ''' Distributor Guide JW/shaili/20150907
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_linkDist_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_linkDist.Click
        Dim buffer As Byte() = File.ReadAllBytes(Server.MapPath("~/FILE/Sample/EndUser_RMA_New_account_Registration_User_Guide.pdf"))
        Dim ms As New MemoryStream(buffer)
        ms.Position = 0
        ms.Read(buffer, 0, Integer.Parse(ms.Length.ToString()))
        Response.Clear()
        Response.AddHeader("Content-Disposition", ("attachment;filename=Distributor Guide.pdf"))
        Response.AddHeader("Content-Length", ms.Length.ToString())
        Response.ContentType = "application/pdf"
        ms.Close()
        Response.BinaryWrite(buffer)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
    End Sub

    ''' <summary>
    ''' 登入
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 登入時先比對客戶資料,比對無客戶資料再比對管理者資料
    Protected Sub UI_cmdLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Token As String = Guid.NewGuid().ToString().Trim()

        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim RepairID As String = ""
        Dim Repair_NAME As String = ""
        Dim Repair_TEL As String = ""
        Dim Comp_Admin As String = ""
        Dim CountryID As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable

        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim ctlLanguage As New ctlLanguage


        'START
        '原本是紀錄客戶編號, 已改成紀錄 登入型態:1.Customer, 2.Manager
        'Dim sCompanyID As String = Me.UI_txtCompanyID.Text.ToString().Trim()
        Dim sLoginType As String = "Customer"
        Dim Identity As String = "1" 'JW/shaili/20150524
        'END

        Dim sAccountID As String = Me.UI_txtAccountID.Text.ToString().Trim()
        Dim sPassword As String = Me.UI_txtPassword.Text.ToString().Trim()

        Try
            sPassword = _Crypto.Encrypt(sPassword, "")
            '_Crypto.Decrypt("6E17A43D91B298A9", "")
            If oCustomerUser.Login(sLoginType, sAccountID, sPassword) = True Then
                dtCustomerUser = oCustomerUser.getLoginData()
                If dtCustomerUser.Rows.Count > 0 Then
                    Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(0)

                    dtCustomer = oCustomer.QueryByCompany(dr.CUUS_CUID)
                    If dtCustomer.Rows.Count > 0 Then
                        RepairID = dtCustomer.Rows(0)("CU_COMPNO").ToString().Trim()
                        Repair_NAME = dtCustomer.Rows(0)("COMP_ADDRESS").ToString().Trim()
                        Repair_TEL = dtCustomer.Rows(0)("COMP_TEL").ToString().Trim()
                        Comp_Admin = dtCustomer.Rows(0)("COMP_ADMIN").ToString().Trim()
                        CountryID = dtCustomer.Rows(0)("CU_COUNTRYID").ToString().Trim()
                        If dtCustomer.Rows(0)("CU_ISENDUSER").ToString().Trim() = "Y" Then 'JW/shaili/20150524
                            sLoginType = "EndUser"
                            Identity = "3"
                        End If
                    End If

                    oLoginInfo.Save(sLoginType, sAccountID)
                    Dim LanguageID As String = oLoginInfo.getLanguageID(sLoginType, sAccountID)
                    ctlLanguage.reLoad(LanguageID)

                    Call setSession(Identity, dr.CUUS_CUID, RepairID, Repair_NAME, Repair_TEL, dr.CUUS_ACCOUNTID, dr.CUUS_ACCOUNTID, "", dr.CUUS_ISMANAGER, "", 0, 0, LanguageID, Comp_Admin, CountryID) 'JW/shaili/20150524
                    blnFlag = True
                End If

            Else
                If oAdmin.Login(sAccountID, sPassword) = True Then
                    dtAdmin = oAdmin.getLoginData()
                    If dtAdmin.Rows.Count > 0 Then
                        Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

                        '一次性密鑰 開始
                        Dim AccountID As String = sAccountID
                        Dim CheckToken As String = "0"

                        Dim ctAddress_Admin As New ctAddress
                        ctAddress_Admin.Insert_Token_Logoin(AccountID, Token, CheckToken)
                        '一次性密鑰 結束

                        sLoginType = "Manager"
                        oLoginInfo.Save(sLoginType, sAccountID)
                        Dim LanguageID As String = oLoginInfo.getLanguageID(sLoginType, sAccountID)
                        ctlLanguage.reLoad(LanguageID)

                        '取得維修中心的資料
                        If dr.AD_REPAIRCENTER.Trim() <> "" Then
                            Dim arrRepairCenter() As String = dr.AD_REPAIRCENTER.Split(",")
                            Dim oCompany As New ctlCompany
                            Dim dtCompany As New CompanyDTO.CompanyDataTable

                            dtCompany = oCompany.QueryByRepairName(arrRepairCenter(0).Trim())
                            If dtCompany.Rows.Count > 0 Then
                                Dim drCompany As CompanyDTO.CompanyRow = dtCompany.Rows(0)
                                Repair_NAME = drCompany.COMP_NAME.Trim()
                                If drCompany.IsCOMP_TELNull = False Then Repair_TEL = drCompany.COMP_TEL.Trim()
                                Comp_Admin = drCompany.COMP_ADMIN.ToString()
                                CountryID = drCompany.COMP_COUNTRYID.ToString()
                            End If
                        End If

                        Dim sDeptNo As String = oAdmin.getLoginDeptNO(dr.AD_ID)

                        Call setSession("2", "", "", Repair_NAME, Repair_TEL, dr.AD_ID, dr.AD_NAME, sDeptNo, "1", dr.AD_REPAIRCENTER, dr.AD_ROLE, dr.AD_AUTHORITYLEVEL, LanguageID, Comp_Admin, CountryID)
                        blnFlag = True
                    End If
                End If
            End If

            If sAccountID <> "0261" And sAccountID <> "admin" Then
                If Comp_Admin.ToUpper().Trim() = "MPLUS" Then
                    sMessage = "You have no Cipherlab account !"
                    blnFlag = False
                End If
            End If

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            If blnFlag = False Then
                '登入有誤
                Me.UI_lblMessage.Visible = True
                If sMessage.Trim() = "" Then
                    sMessage = _oLanguage.getText("Common", "051", ctlLanguage.eumType.Tag)
                End If
                Me.UI_lblMessage.Text = sMessage

            Else
                Me.UI_lblMessage.Visible = False
                Me.UI_lblMessage.Text = ""

                If Me.CheckBox1.Checked = True Then

                    Dim Path As String = _oComm.getWorkListPage()

                    'Dim request As HttpRequest = Page.Request
                    'Dim port As Integer = request.Url.Port
                    'Session("_ClientPort") = port

                    If Session("_Identity").ToString() = "2" Then
                        '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 begin
                        Dim _WEBURL As String = ConfigurationManager.AppSettings("WEBURL_Admin")
                        Dim request As HttpRequest = Page.Request
                        Dim scheme As String = request.Url.Scheme      ' http 或 https
                        Dim host As String = request.Url.Host          ' 網域或 IP
                        Dim port As Integer = request.Url.Port         ' port
                        Dim logurl As String = String.Format("/Token_Login.aspx?Token={0}", Token) & If(HttpContext.Current.Request.IsLocal OrElse (_isDebug = True And _isDebugSIT = false), String.Format("&ClientPort={0}", port), "") '
                        Dim url As String = If(HttpContext.Current.Request.IsLocal OrElse (_isDebug = True And _isDebugSIT = False), String.Format("{0}://{1}:{2}", scheme, host, "50996"), _WEBURL)
                        Session("ClientPort") = Page.Request.Url.Port
                        LogHelper.WriteLog(Page.Request.Url.Port)
                        LogHelper.WriteLog(url & logurl)
                        Response.Redirect(url & logurl)
                        'Response.Redirect("https://e-rma-admin.cipherlab.com.tw/Token_Login.aspx?Token=" & Token)
                        'Response.Redirect("http://localhost:50996/Token_Login.aspx?Token=" & Token)
                        '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 end
                    End If

                    Response.Redirect(Path)


                Else
                    Me.UI_lblMessage.Visible = True

                    Me.UI_lblMessage.Text = "Please Check True By signing up CipherLab E-RMA system, you acknowledge that you consent to CipherLab is Privacy Policy You can withdraw your consent at any time."

                End If



            End If
        End Try

        If Me.UI_lblMessage.Visible = True Then
            Dim sScript As String = "<script type=""text/javascript"">alert('" & Me.UI_lblMessage.Text & "')</script>"
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "Msg", sScript)
        End If


    End Sub

    ''' <summary>
    ''' 設定登入後的 Session 資料
    ''' </summary>
    ''' <param name="Identity">身分:1.客戶, 2.公司(維修中心)</param>
    ''' <param name="CustomerID">客戶代號</param>
    ''' <param name="UserID">帳號</param>
    ''' <param name="UserName">姓名</param>
    ''' <param name="isManager">是否為管理人(0.否,1.是)</param>
    ''' <param name="Role">角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin</param>
    ''' <param name="AuthorityLevel">權限範圍等級:0.By Center、1.All</param>
    ''' <param name="LanguageID">語系</param>
    ''' <remarks></remarks>
    Private Sub setSession(ByVal Identity As String, ByVal CustomerID As String,
        ByVal RepairID As String, ByVal CustomerName As String, ByVal CustomerTEL As String,
        ByVal UserID As String, ByVal UserName As String, ByVal DeptNO As String, ByVal isManager As String,
        ByVal RepairCenter As String, ByVal Role As String, ByVal AuthorityLevel As String, ByVal LanguageID As String, ByVal sComp_Admin As String, ByVal CountryID As String)

        Session("_Identity") = Identity.Trim()
        Session("_CustomerID") = CustomerID.Trim()
        Session("_RepairID") = RepairID.Trim()
        Session("_RepairName") = CustomerName.Trim()
        Session("_RepairTEL") = CustomerTEL.Trim()
        Session("_Comp_Admin") = sComp_Admin
        Session("_CountryID") = CountryID.Trim()

        Session("_UserID") = UserID.Trim()
        Session("_UserName") = UserName.Trim()
        Session("_DeptNO") = DeptNO.Trim()
        Session("_isManager") = isManager.Trim()

        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        Session("_Role") = Role.Trim()
        Session("_OperationRole") = ""
        Session("_OperationRoleName") = ""

        Dim arrRole() As String = Session("_Role").ToString().Split(",")
        If arrRole.Length > 0 Then
            Session("_OperationRole") = arrRole(0).Trim()

            Select Case arrRole(0).Trim()
                Case "1"
                    Session("_OperationRoleName") = _oLanguage.getText("Common", "057", ctlLanguage.eumType.Tag)

                Case "2"
                    Session("_OperationRoleName") = _oLanguage.getText("Common", "058", ctlLanguage.eumType.Tag)

                Case "3"
                    Session("_OperationRoleName") = _oLanguage.getText("Common", "059", ctlLanguage.eumType.Tag)

            End Select
        End If
        Session("_isShowNotFound") = _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag)

        Session("_RepairCenter") = RepairCenter.Trim()
        Session("_AuthorityLevel") = AuthorityLevel.Trim()
        Session("_LanguageID") = LanguageID.Trim()

        If Me.UI_cboLanguage.SelectedValue = 0 Then

        Else
            '換語系
            Session("_LanguageID") = Me.UI_cboLanguage.SelectedValue
            _ctlLanguage.reLoad(Session("_LanguageID"))

        End If

        Dim oCountry As New ctlCountry
        Dim oCompany As New ctlCompany


        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.CustomerDataTable

        dtCustomer = oCustomer.QueryByPrimaryKey(CustomerID)

        If dtCustomer.Count > 0 Then
            Dim dr As CustomerDTO.CustomerRow = dtCustomer.Rows(0)

            Try
                Session("CU_CONTACTPERSON") = dr.CU_CONTACTPERSON.ToString().Trim()
            Catch ex As Exception
                Session("CU_CONTACTPERSON") = UserID
            End Try


        End If


    End Sub

    ''' <summary>
    ''' 登出清除Session
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoginOut()
        Session("_Identity") = ""
        Session("_CustomerID") = ""
        Session("_RepairName") = ""
        Session("_RepairTEL") = ""

        Session("_UserID") = ""
        Session("_UserName") = ""
        Session("_DeptNO") = ""
        Session("_isManager") = ""

        Session("_Role") = ""
        Session("_OperationRole") = ""
        Session("_OperationRoleName") = ""

        Session("_RepairCenter") = ""
        Session("_AuthorityLevel") = ""
        Session("_LanguageID") = "001"
        Session("_isPolicy") = False
        Session("_isTimeOut") = False
        Session("_dtLanguage") = Nothing
        Session("_isShowNotFound") = ""
        Session("CU_CONTACTPERSON") = ""
        Session("_Comp_Admin") = ""

    End Sub

End Class

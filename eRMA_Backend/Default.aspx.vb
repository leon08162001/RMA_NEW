Imports DataService
Imports System.Data
Imports SecurityCrypt
Imports DefLanguage
Imports System.IO


Partial Class _Default
    Inherits System.Web.UI.Page
    Dim _oComm As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call LoginOut()

            If Convert.ToBoolean(Session("_isTimeOut")) = True Then
                Me.UI_lblMessage.Visible = True
                Me.UI_lblMessage.Text = _oLanguage.getText("Common", "050", ctlLanguage.eumType.Tag)
            End If

            Me.UI_linkForget.Text = _oLanguage.getText("Common", "053", ctlLanguage.eumType.Tag)
            Me.UI_linkRigister.PostBackUrl = "https://e-rma.cipherlab.com.tw/RMA/View/RMA/RegisterUser.aspx" 'JW/shaili/20150810
        End If

    End Sub


    
    ''' <summary>
    ''' End User註冊 JW/shaili/20150524
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    'Protected Sub UI_linkRigister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_linkRigister.Click
    '    'Me.Form.Action = "http://" + Request.Url.Authority + "/RMA/View/RMA/RegisterUser.aspx"
    '    Me.Form.Action = "http://websvr2.cipherlab.com.tw:80/View/RMA/RegisterUser.aspx"
    'End Sub
    
    ''' <summary>
    ''' End User Guide JW/shaili/20150907
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
    Protected Sub UI_cmdLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdLogin.Click
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim RepairID As String = ""
        Dim Repair_NAME As String = ""
        Dim Repair_TEL As String = ""
        Dim Comp_Admin As String = ""

        Dim oCustomer As New ctlCustomer.Customer
        Dim dtCustomer As New CustomerDTO.vwCustomerDataTable

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
                        If dtCustomer.Rows(0)("CU_ISENDUSER").ToString().Trim() = "Y" Then 'JW/shaili/20150524
                            sLoginType = "EndUser"
                            Identity = "3"
                        End If
                    End If

                    oLoginInfo.Save(sLoginType, sAccountID)
                    Dim LanguageID As String = oLoginInfo.getLanguageID(sLoginType, sAccountID)
                    ctlLanguage.reLoad(LanguageID)

                    Call setSession(Identity, dr.CUUS_CUID, RepairID, Repair_NAME, Repair_TEL, dr.CUUS_ACCOUNTID, dr.CUUS_ACCOUNTID, "", dr.CUUS_ISMANAGER, "", 0, 0, LanguageID, Comp_Admin) 'JW/shaili/20150524
                    blnFlag = True
                End If

            Else
                If oAdmin.Login(sAccountID, sPassword) = True Then
                    dtAdmin = oAdmin.getLoginData()
                    If dtAdmin.Rows.Count > 0 Then
                        Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

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
                            End If
                        End If

                        Dim sDeptNo As String = oAdmin.getLoginDeptNO(dr.AD_ID)

                        Call setSession("2", "", "", Repair_NAME, Repair_TEL, dr.AD_ID, dr.AD_NAME, sDeptNo, "1", dr.AD_REPAIRCENTER, dr.AD_ROLE, dr.AD_AUTHORITYLEVEL, LanguageID, Comp_Admin)
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

				'Response.Write(HttpContext.Current.Session("_Identity").ToString())
				'Response.Write(HttpContext.Current.Session("_RepairID").ToString.Trim())				
				'Response.End()

                '登入成攻, 導入到 待處理工作頁面
                Response.Redirect(_oComm.getWorkListPage())

            End If
        End Try

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
        ByVal RepairCenter As String, ByVal Role As String, ByVal AuthorityLevel As String, ByVal LanguageID As String, ByVal sComp_Admin As String)

        Session("_Identity") = Identity.Trim()
        Session("_CustomerID") = CustomerID.Trim()
        Session("_RepairID") = RepairID.Trim()
        Session("_RepairName") = CustomerName.Trim()
        Session("_RepairTEL") = CustomerTEL.Trim()
        Session("_Comp_Admin") = sComp_Admin

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

        Session("_Comp_Admin") = ""

    End Sub





End Class

Imports System.Data
Imports DataService
Imports DefLanguage
Imports RMA_Common

Partial Class Token_Login
    Inherits System.Web.UI.Page
    Dim _oComm As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then



            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""

            Dim RepairID As String = ""
            Dim Repair_NAME As String = ""
            Dim Repair_TEL As String = ""
            Dim Comp_Admin As String = ""

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
            Dim ctAddress As New DataService.ctlAdmin

            If Not Request.Params("Token") Is Nothing Then
                LogHelper.WriteLog(Request.Params("ClientPort").ToString())
                Dim Token As String = Request.Params("Token").ToString().Trim()
                Dim myDataTable As New DataTable
                myDataTable = ctAddress.Select_Token_Logoin(Token)

                Dim sAccountID As String = ""

                If (myDataTable.Rows.Count > 0) Then
                    sAccountID = myDataTable.Rows(0)("AccountID").ToString().Trim()
                End If


                Try

                    If oAdmin.LoginToken(sAccountID) = True Then
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


                    Else

                        'Response.Write(HttpContext.Current.Session("_Identity").ToString())
                        'Response.Write(HttpContext.Current.Session("_RepairID").ToString.Trim())				
                        'Response.End()

                        '登入成攻, 導入到 待處理工作頁面
                        Response.Redirect(_oComm.getWorkListPage())

                    End If
                End Try



            End If
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
        '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 begin
        Session("_ClientPort") = If(Not Request.Params("ClientPort") Is Nothing, Request.Params("ClientPort"), Nothing)
        '修改本機登出跳轉到正式機，直接抓本機的port組出URL。 by buck modify 20250917 end
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

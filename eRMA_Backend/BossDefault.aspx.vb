Imports DataService
Imports DefLanguage


Partial Class BossDefault
    Inherits System.Web.UI.Page
    Dim _oComm As New Common
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            If Convert.ToBoolean(Session("_isTimeOut")) = True Then
                Me.UI_lblMessage.Visible = True
                Me.UI_lblMessage.Text = _oLanguage.getText("Common", "050", ctlLanguage.eumType.Tag)
            End If

            If IsNothing(Request("ID")) = False Then
                Me.UI_lblPreviousPage_RMASMID.Text = _Crypto.Decrypt(Request("ID").Trim(), "")
            End If

            'Me.UI_cmdLogin.Visible = False
            'If Me.UI_lblPreviousPage_RMASMID.Text <> "" Then
            '    Me.UI_cmdLogin.Visible = True
            'End If

            Call LoginOut()
        End If

    End Sub

    ''' <summary>
    ''' 登入
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdLogin.Click
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim Repair_NAME As String = ""
        Dim Repair_TEL As String = ""
        Dim oLoginInfo As New ctlLoginInfo
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        'START
        '原本是紀錄客戶編號, 已改成紀錄 登入型態:1.Customer, 2.Manager
        'Dim sCompanyID As String = Me.UI_txtCompanyID.Text.ToString().Trim()
        Dim sLoginType As String = "Manager"
        'END

        Dim sAccountID As String = Me.UI_txtAccountID.Text.ToString().Trim()
        Dim sPassword As String = Me.UI_txtPassword.Text.ToString().Trim()

        Try
            sPassword = _Crypto.Encrypt(sPassword, "")

            If oAdmin.Login(sAccountID, sPassword) = True Then
                dtAdmin = oAdmin.getLoginData()
                If dtAdmin.Rows.Count > 0 Then
                    Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

                    oLoginInfo.Save(sLoginType, sAccountID)
                    Dim LanguageID As String = oLoginInfo.getLanguageID(sLoginType, sAccountID)

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
                        End If
                    End If

                    Call setSession("2", "", Repair_NAME, Repair_TEL, dr.AD_ID, dr.AD_NAME, "1", dr.AD_REPAIRCENTER, dr.AD_ROLE, dr.AD_AUTHORITYLEVEL, LanguageID)
                    blnFlag = True
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

                '登入成攻, 導入到 待處理工作頁面
                'Response.Redirect("BossShipment_Notice.aspx")
                Server.Transfer("BossShipment_Notice.aspx")
                'Server.Transfer("Shipment_Detail.aspx")

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
    Private Sub setSession(ByVal Identity As String, ByVal CustomerID As String, ByVal CustomerName As String, ByVal CustomerTEL As String,
        ByVal UserID As String, ByVal UserName As String, ByVal isManager As String,
        ByVal RepairCenter As String, ByVal Role As String, ByVal AuthorityLevel As String, ByVal LanguageID As String)

        Session("_Identity") = Identity.Trim()
        Session("_CustomerID") = CustomerID.Trim()
        Session("_RepairName") = CustomerName.Trim()
        Session("_RepairTEL") = CustomerTEL.Trim()

        Session("_UserID") = UserID.Trim()
        Session("_UserName") = UserName.Trim()
        Session("_isManager") = isManager.Trim()

        Session("_Role") = Role.Trim()
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
        Session("_isManager") = ""

        Session("_Role") = ""
        Session("_RepairCenter") = ""
        Session("_AuthorityLevel") = ""
        Session("_LanguageID") = ""
        Session("_isPolicy") = False
        Session("_isTimeOut") = False
    End Sub

End Class

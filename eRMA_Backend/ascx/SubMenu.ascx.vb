Imports DefLanguage
Imports SecurityCrypt


Partial Class ascx_SubMenu
    Inherits System.Web.UI.UserControl
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Dim _RepairExtend_Name1 As String = ConfigurationSettings.AppSettings("RepairExtend_Name1")
    Dim _RepairExtend_URL1 As String = ConfigurationSettings.AppSettings("RepairExtend_URL1")
    Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0
        Dim blnReports As Boolean = False   '紀錄是否執行報表功能

        Me.UI_tbUser.Visible = False
        Me.UI_tbAdmin.Visible = False
        Me.UI_tbProcess.Visible = False
        Me.UI_trShipment.Visible = False
        Me.UI_trShipping.Visible = False
        UI_tbWarrantyOrder.Visible = False
        UI_tbCost.Visible = False
        UI_tbAssistant.Visible = False

        '判斷是否是報表查詢機制
        Dim qryParm As String = Request.Url.Query.Trim()
        If Not qryParm.Equals("") Then
            If qryParm.Substring(1).IndexOf("=") = -1 Then
                If _Crypto.Decrypt(qryParm.Substring(1), "").ToLower = "Reports=true".ToLower Then
                    blnReports = True
                End If
            End If
        End If        
        
        'If Session("_Identity").ToString() = "3" Then 'JW/shaili/20150524
        '    Me.UI_linCustomer.Target = "_blank"
        '    Me.UI_linCustomer.NavigateUrl = "~/GoToRMA_Account.aspx"
        'End If
        
		'Response.Write(blnReports)
		'Response.Write(Session("_Role").ToString())
        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        If Session("_Role").ToString() = "0" Then
            Me.UI_tbUser.Visible = True
            If blnReports = True Then
                Call setControlsRPT(qryParm)
            End If


        Else
            If blnReports = True Then
                Call setControlsRPT(qryParm)
            Else
                Me.UI_tbReports.Visible = False

                If Session("_Role").ToString().IndexOf("3") <> -1 Then
                    Me.UI_tbProcess.Visible = True
                    Me.UI_trShipment.Visible = False
                    Me.UI_trExtend_1.Visible = False
                    Me.UI_ChargeQUOTED.Visible = True
                End If

                If Session("_Role").ToString().IndexOf("4") <> -1 Then
                    Me.UI_tbProcess.Visible = True
                    Me.UI_trShipping.Visible = False
                    Me.UI_trExtend_1.Visible = False
                End If

                If Session("_Role").ToString().IndexOf("2") <> -1 Then
                    Me.UI_tbProcess.Visible = True
                    Me.UI_trExtend_1.Visible = True
                    Me.UI_linExtend_1.Text = _RepairExtend_Name1
                    Me.UI_linExtend_1.NavigateUrl = _RepairExtend_URL1
                    Me.UI_HQRepairList.Visible = True
                    '月對帳單 只有維修人員及Admin可以看到
                    UI_Maintenance_Statement.Visible = True

                    '判斷是否要執行 開放 UI_HQQuote
                    Dim arrRepairCenter() As String = Session("_RepairCenter").ToString().Trim().Split(",")
                    For i = 0 To arrRepairCenter.Length - 1
                        If _HQRepairNo.Trim() = arrRepairCenter(i).ToString().Trim() Then
                            Me.UI_HQQuote.Visible = True
                            Exit For
                        End If
                    Next
                End If

                If Session("_Role").ToString().IndexOf("6") <> -1 Then
                    Me.UI_tbWarrantySetting.Visible = True
                End If

                If Session("_Role").ToString().IndexOf("7") <> -1 Then
                    Me.UI_tbWarrantyOrder.Visible = True
                End If

                If Session("_Role").ToString().IndexOf("9") <> -1 Then
                    Me.UI_tbAdmin.Visible = True
                    Me.UI_tbProcess.Visible = True
                    Me.UI_trAccount_Process.Visible = True
                    Me.UI_HQRepairList.Visible = True
                    '月對帳單 只有維修人員及Admin可以看到
                    UI_Maintenance_Statement.Visible = True
                Else
                    Me.UI_tbProcess.Visible = True
                    Me.UI_trAccount_Process.Visible = True
                End If

            End If
        End If


        '如果符合 flow case 01的維修中心, 關閉 UI_trExtend_1, UI_trExtend_2, UI_HQRepairList
        '符合法國 維修中心
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        For i = 0 To arrRepairNo_flowCase01.Length - 1
            If Session("_RepairCenter").ToString().Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                Me.UI_trExtend_1.Visible = False
                Me.UI_trExtend_2.Visible = False

                If Session("_Role").ToString().IndexOf("9") = -1 Then
                    Me.UI_HQRepairList.Visible = False
                End If

                Exit For
            End If
        Next

        'Sunny說某次改版這兩個被拿掉 20171219 Isaac Mod
        If Session("_Role").ToString().IndexOf("9") <> -1 Then
            Me.UI_trExtend_1.Visible = True
            Me.UI_trExtend_2.Visible = True
        End If

        '新增特殊權限
        If Session("_Role").ToString().IndexOf("C") <> -1 Then
            UI_tbCost.Visible = True
        End If

        If Session("_Role").ToString().IndexOf("A") <> -1 Then
            UI_tbAssistant.Visible = True
        End If


        Me.UI_trShipping.Visible = False


        If Me.IsPostBack = False Then
            Call setControls()
        End If

    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblRGAccount.Text = _oLanguage.getText("Common", "035", ctlLanguage.eumType.Tag)
        Me.UI_lblRepair.Text = _oLanguage.getText("Common", "036", ctlLanguage.eumType.Tag)

        Me.UI_lblAccountText.Text = Session("_UserID").ToString()
        If Session("_OperationRoleName").ToString.Trim <> "" Then
            Me.UI_lblRoleNameText.Text = _oLanguage.getText("Common", "067", ctlLanguage.eumType.Tag) & ":" & Session("_OperationRoleName")
        Else
            Me.UI_lblRoleNameText.Text = ""
        End If

        Me.UI_linHQQuote.Text = _oLanguage.getText("RMA", "424", ctlLanguage.eumType.Tag)
        Me.UI_linRepairList.Text = _oLanguage.getText("RMA", "425", ctlLanguage.eumType.Tag)

        Me.UI_linChargeQUOTED.Text = _oLanguage.getText("RMA", "426", ctlLanguage.eumType.Tag)

        Me.UI_lblCUNAME.Text = Session("_RepairName").ToString()
        Me.UI_lblCUTEL.Text = Session("_RepairTEL").ToString()
    End Sub


    Private Sub setControlsRPT(ByVal qryParm As String)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Me.UI_tbReports.Visible = True

        Me.ui_HrefReport1.Text = _oLanguage.getText("Report", "001", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport1.NavigateUrl = "~/Report1_Search.aspx" & qryParm

        Me.ui_HrefReport2.Text = _oLanguage.getText("Report", "002", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport2.NavigateUrl = "~/Report2_Search.aspx" & qryParm

        Me.ui_HrefReport3.Text = _oLanguage.getText("Report", "003", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport3.NavigateUrl = "~/Report3_Search.aspx" & qryParm

        Me.ui_HrefReport4.Text = _oLanguage.getText("Report", "004", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport4.NavigateUrl = "~/Report4_Search.aspx" & qryParm


        Me.ui_HrefReport5.Text = _oLanguage.getText("Report", "005", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport5.NavigateUrl = "~/Report5_Search.aspx" & qryParm

        Me.ui_HrefReport6.Text = _oLanguage.getText("Report", "006", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport6.NavigateUrl = "~/Report6_Search.aspx" & qryParm

        Me.ui_HrefReport7.Text = _oLanguage.getText("Report", "007", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport7.NavigateUrl = "~/Report7_Search.aspx" & qryParm

        Me.ui_HrefReport8.Text = _oLanguage.getText("Report", "008", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport8.NavigateUrl = "~/Report8_Search.aspx" & qryParm

        Me.ui_HrefReport9.Text = _oLanguage.getText("Report", "009", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport9.NavigateUrl = "~/Report9_Search.aspx" & qryParm

        Me.ui_HrefReport10.Text = _oLanguage.getText("Report", "010", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport10.NavigateUrl = "~/Report10_Search.aspx" & qryParm

        Me.ui_HrefReport11.Text = _oLanguage.getText("Report", "H_001", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport11.NavigateUrl = "~/Report11_Search.aspx" & qryParm

        Me.ui_HrefReport12.Text = _oLanguage.getText("Report", "H_014", ctlLanguage.eumType.Tag)
        Me.ui_HrefReport12.NavigateUrl = "~/Report12_Search.aspx" & qryParm

	
        '身分:1.客戶, 2.公司(維修中心)	
		'Response.Write(Session("_Identity").ToString().Trim())
		'Response.Write(Me.UI_tbReports.Visible)
        If Session("_Identity").ToString().Trim() = "1" Then
            Me.trRPT2.Visible = False
            Me.trRPT3.Visible = True
            Me.trRPT4.Visible = False
            Me.trRPT5.Visible = False

            Me.trRPT11.Visible = False
            Me.trRPT12.Visible = True
        Else
            Me.trRPT2.Visible = False
            Me.trRPT3.Visible = True
            Me.trRPT4.Visible = True
            Me.trRPT5.Visible = False

            Me.trRPT11.Visible = True
            Me.trRPT12.Visible = True		
        End If


        'Dim isReports_First As Boolean = True
        '如果符合 flow case 01的維修中心, 關閉 UI_trExtend_1, UI_trExtend_2, UI_HQRepairList
        '符合法國 維修中心
        Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
        Dim arrRepairCenter() As String = Session("_RepairCenter").ToString().Trim().Split(",")
        If arrRepairCenter.Length = 1 Then
            Dim sRepairCenter As String = arrRepairCenter(0).ToString().Trim()

            For j = 0 To arrRepairNo_flowCase01.Length - 1
                If sRepairCenter.IndexOf(arrRepairNo_flowCase01(j).ToString().Trim()) <> -1 Then
				    'Response.Write(Session("_Role").ToString())
                    If Session("_Role").ToString().IndexOf("2") <> -1 Then
                        '如果是CEAT的話, 維修人員只看到這兩個 (rpt3, rpt12)
                        Me.trRPT4.Visible = False
                        Me.trRPT11.Visible = False
                    Else
                        'marked by Hugh 191124, 除CEAT維修人員外, 其它報表應該仍開放, 與HQ一致
                        'Me.UI_tbReports.Visible = False
                    End If

                    Exit For
                End If
            Next
        End If

    End Sub



    Protected Sub UI_imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UI_imgSearch.Click

        Dim sParm As String = Me.UI_globalRMAID.Text.Trim()

        '身分:1.客戶, 2.公司(維修中心) ,3.End User
        'New Request
        If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
            Response.Redirect("Client_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
        Else

            '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                Response.Redirect("Sales_Status_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
            Else
                Response.Redirect("Request_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
            End If

        End If

    End Sub



End Class

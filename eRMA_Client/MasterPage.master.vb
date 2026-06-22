Imports System.Data
Imports Common
Imports DataService
Imports DefLanguage

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim _oLanguage As New ctlLanguage
    Public _alert As String
    Public _oLanguage_js As String

    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _WEBURL As String = ConfigurationManager.AppSettings("WEBURL")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Protected Sub UI_imgSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_imgSearch.Click
        LogHelper.WriteLog("UI_imgSearch_Click")
        Try
            Dim sParm As String = Me.UI_globalRMAID.Text.Trim()

            Dim request As HttpRequest = Page.Request
            Dim scheme As String = request.Url.Scheme      ' http 或 https
            Dim host As String = request.Url.Host          ' 網域或 IP
            Dim port As Integer = request.Url.Port         ' port
            Dim logurl As String = ""
            Dim url As String = If(HttpContext.Current.Request.IsLocal OrElse _isDebug = True, String.Format("{0}://{1}:{2}", scheme, host, port), _WEBURL)

            '身分:1.客戶, 2.公司(維修中心) ,3.End User
            'New Request  _Crypto.Encrypt(sParm, "")
            If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
                'Response.Redirect("Client_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
                logurl = String.Format("/Client_Search.aspx?RMANO={0}", _Crypto.Encrypt(sParm, ""), False)
            Else

                '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
                If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                    'Response.Redirect("Sales_Status_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
                    logurl = String.Format("/Sales_Status_Search.aspx?RMANO={0}", _Crypto.Encrypt(sParm, ""), False)
                Else
                    'Response.Redirect("Request_Search.aspx?RMANO=" & _Crypto.Encrypt(sParm, ""))
                    logurl = String.Format("/Request_Search.aspx?RMANO={0}", _Crypto.Encrypt(sParm, ""), False)
                End If
            End If

            Dim script As String = "window.location='" & url & logurl & "';"
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        Catch ex As Exception
            LogHelper.WriteLog(ex.Message)
        End Try
    End Sub

    Dim _ctlLanguage As New ctlLanguage
    ''' <summary>
    ''' 取得語系資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryLanguage()
        Dim dtLanguage As New LanguageDTO.DEFLANGUAGEDataTable
        dtLanguage = _ctlLanguage.QueryByDefLanguage()

        Me.UI_cboLanguage.DataTextField = "DFL_NAME"
        Me.UI_cboLanguage.DataValueField = "DFL_NO"

        Me.UI_cboLanguage.DataSource = dtLanguage.DefaultView
        Me.UI_cboLanguage.DataBind()
        Me.UI_cboLanguage.SelectedValue = Session("_LanguageID").ToString().Trim()

        _ctlLanguage.reLoad(Session("_LanguageID"))


    End Sub
    ''' <summary>
    ''' 查詢客戶
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryClient()

        _alert = ""

        Try
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

            Dim oClient As New ctlRMA.Client



            If Session("Query_Alert") IsNot Nothing Then
                dtRequest = Session("Query_Alert")
            Else
                dtRequest = oClient.Query_Alert(Session("_CustomerID"))
                Session("Query_Alert") = dtRequest
            End If

            For a = 0 To dtRequest.Rows.Count - 1

                Dim RMAD_RMANO As String = dtRequest.Rows(a)("RMAD_RMANO").ToString()
                Dim RMAD_WARRANTY As String = dtRequest.Rows(a)("RMAD_WARRANTY").ToString()
                Dim RMAD_SERIALNO As String = dtRequest.Rows(a)("RMAD_SERIALNO").ToString()
                Dim RMAD_STATUS As String = dtRequest.Rows(a)("RMAD_STATUS").ToString()
                Dim RMAD_RMANO_Crypto As String = _Crypto.Encrypt(RMAD_RMANO, "").Trim()

                '" & _Crypto.Encrypt(RMAD_RMANO, "")
                _alert += "    <div Class='erma-alert-list1'  id='" & RMAD_RMANO_Crypto & "'   onclick='Serch_RMAD_RMANO(this)'  > "
                _alert += "    <div Class='borders'> "
                _alert += "    <div Class='erma-msg'> "
                _alert += "    <span Class='nortext'>RMA No. </span> <span class='boldtext'>" & RMAD_RMANO & "</span> <span class='nortext'>"

                _alert += "     </br> 序號:" & RMAD_SERIALNO
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoted, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled';

                If RMAD_STATUS = "10" Then
                    _alert += "   已請求 "
                ElseIf RMAD_STATUS = "0" Then
                    _alert += "    已儲存 "
                ElseIf RMAD_STATUS = "20" Then
                    _alert += "    已收到 "
                ElseIf RMAD_STATUS = "30" Then
                    _alert += "    維修報價 "
                ElseIf RMAD_STATUS = "35" Then
                    _alert += "    銷售報價 "
                ElseIf RMAD_STATUS = "40" Then
                    _alert += "    銷售報價確認 "
                ElseIf RMAD_STATUS = "50" Then
                    _alert += "   客戶確認 "
                ElseIf RMAD_STATUS = "60" Then
                    _alert += "    已修復 "
                ElseIf RMAD_STATUS = "70" Then
                    _alert += "    已出貨 "
                ElseIf RMAD_STATUS = "90" Then
                    _alert += "    關閉 "
                ElseIf RMAD_STATUS = "91" Then
                    _alert += "    取消 "
                End If

                _alert += "    </span> "
                _alert += "    </div> "

                If RMAD_WARRANTY <> "" Then
                    _alert += "    <h2 Class='hh2'>" & Convert.ToDateTime(RMAD_WARRANTY).ToString("yyyy/MM/dd") & "</h2> "
                End If

                _alert += "    </div> "
                _alert += "    <div Class='erma-circle'></div> "
                _alert += "    </div>"

            Next

        Catch ex As Exception

        End Try

    End Sub

    Public Class SnData
        Public Property sn As String
        Public Property CustomerID As String
    End Class

    'alert
    Private Sub RMA_DataBind(ByVal RMA_CUNO As String)


        Dim ctlRMA_ As New ctlRMA.Requested
        Dim dt As New DataTable
        dt = ctlRMA_.QueryByRMAROWNUM(RMA_CUNO)

        For i = 0 To dt.Rows.Count - 1

            '接受前臺傳的num
            Dim Sn_no As String
            Sn_no = dt.Rows(i)("RMA_NO").ToString().Trim()

            Dim oClient As New ctlRMA.Client
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable

            Dim Requested As New ctlRMA.Requested
            Dim RMADetailDataTable As New RmaDTO.RMADetailDataTable

            Dim oRMAStatus As New ctlRMA.RMAStatus
            Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
            Dim CustomerID As String = ""
            Dim sRMANo As String = ""
            Dim sModelNo As String = ""
            Dim sSerialNo As String = ""
            Dim Status As String = "-1"
            Dim fdate As String = Date.Now.AddMonths(_QueryDate_Month).ToShortDateString()
            Dim edate As String = Date.Now.ToShortDateString()

            If (Sn_no = "") Then
                Context.Response.Write("")
                Return
            End If

            sRMANo = Sn_no
            CustomerID = RMA_CUNO

            ' 新增查詢訂單是否過期 by buck modify 20251118 begin 
            'dtRequest = oClient.Query(sRMANo, "", sModelNo, sSerialNo, "-1", Status, fdate, edate, "", "RMAD_RMANO desc")
            dtRequest = oClient.Query(sRMANo, "", sModelNo, sSerialNo, "-1", Status, fdate, edate, "", "", "RMAD_RMANO desc")
            ' 新增查詢訂單是否過期 by buck modify 20251118 end 

            Dim RMA_ID As String = dtRequest.Rows(0)(1).ToString().Trim()
            RMADetailDataTable = Requested.QueryByRMADetail(RMA_ID, "")
            Dim RMADetailDataTable_RMAD_ID As String = RMADetailDataTable.Rows(0)(0).ToString().Trim()

            dtStatusPoint = oRMAStatus.QueryPointByDetail(RMADetailDataTable_RMAD_ID)
            'Dim _SnData As String = JsonConvert.SerializeObject(dtStatusPoint)

            For a = 0 To dtStatusPoint.Rows.Count - 1

                'RECEIVED_DATE 建立
                'REPAIRQUOTED_DATE 收貨
                'REPAIRQUOTED_DATE  報價
                'REPAIRED_DATE  維修
                'CLOSE_DATE   寄貨

                Dim RECEIVED_DATE As String = dtStatusPoint.Rows(a)("RECEIVED_DATE").ToString().Trim()
                Dim REPAIRQUOTED_DATE As String = dtStatusPoint.Rows(a)("REPAIRQUOTED_DATE").ToString().Trim()
                Dim CLOSE_DATE As String = dtStatusPoint.Rows(a)("CLOSE_DATE").ToString().Trim()

            Next


            'Me.UI_dvRMAListView.DataSource = dvRMA_tmp
            'Me.UI_dvRMAListView.DataBind()
        Next

    End Sub

    ''' <summary>
    ''' 異動語系
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboLanguage.SelectedIndexChanged
        Dim oLoginInfo As New ctlLoginInfo

        Dim log = ""

        Select Case Session("_Identity").ToString()
            Case "1"
                oLoginInfo.ChangeLanguage("Customer", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
                Session("_LanguageID") = oLoginInfo.getLanguageID("Customer", Session("_UserID").ToString())
                log = "C"
            Case "2"
                oLoginInfo.ChangeLanguage("Manager", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
                Session("_LanguageID") = oLoginInfo.getLanguageID("Manager", Session("_UserID").ToString())
                log = "C"

            Case "3"
                oLoginInfo.ChangeLanguage("Manager", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
                Session("_LanguageID") = Me.UI_cboLanguage.SelectedItem.Value.ToString().Trim()
                log = "C"
        End Select
        _ctlLanguage.reLoad(Session("_LanguageID"))

        Dim sURL As String = Me.Page.AppRelativeVirtualPath.Replace("~/", "")

        If log = "" Then
            Dim javascript As String = "<script>alert('使用者不能切換語系');window.location.href=window.location.href;</script>"
            'Dim javascript As String = "<script>window.document.location.href='" & sURL & "';</script>"
            Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType, "locationParent", javascript)
        Else
            Dim javascript As String = "<script>window.location.href=window.location.href;</script>"
            'Dim javascript As String = "<script>window.document.location.href='" & sURL & "';</script>"
            Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType, "locationParent", javascript)
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Page.Title = Session("_Title").ToString().Trim()

        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then

            '存菜單
            'Dim myCookie As HttpCookie = New HttpCookie("UserSettings")
            'myCookie("meuse") = System.IO.Path.GetFileName(Request.PhysicalPath)
            'myCookie.Expires = Now.AddDays(1)
            'Response.Cookies.Add(myCookie)



            '身分:1.客戶, 2.公司(維修中心)
            Select Case Session("_Identity").ToString().Trim()
                Case "1"
                    Me.UI_LoginRole.Text = _oLanguage.getText("Common", "065", ctlLanguage.eumType.Tag) & " - "
                Case "2"
                    Me.UI_LoginRole.Text = _oLanguage.getText("Common", "036", ctlLanguage.eumType.Tag) & " - "
            End Select
            Me.UI_LoginName.Text = Me.UI_LoginName.Text & Session("_UserName").ToString().Trim()

            Call setControls()
            Call QueryLanguage()

            '身分:1.客戶, 2.公司(維修中心) ,3.End User New Request
            If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
                Admin_Server.Visible = False
            Else
                Admin_Server.Visible = False
            End If

            'Call QueryClient()
            Call set_left_Controls()

        End If

        If Session("_LanguageID").ToString() = "003" Then
            Image21.ImageUrl = ResolveUrl("~/Images/pic_21_j.gif")
        Else
            Image21.ImageUrl = ResolveUrl("~/Images/pic_21.gif")
        End If

        If Session("_LanguageID").ToString() = "002" Then
            _oLanguage_js = "<script src='/js/hDate.js'></script>"
        ElseIf Session("_LanguageID").ToString() = "001" Then
            _oLanguage_js = "<script src='/js/hDate_En.js'></script>"
        ElseIf Session("_LanguageID").ToString() = "003" Then
            _oLanguage_js = "<script src='/js/hDate_Jp.js'></script>"
        End If


        Call setScript_Processing()

        'Call RMA_DataBind(Session("_CustomerID").ToString().Trim())

        '首頁通知訊息 
        '2025/6/16-2025/7/31: AU、NZ的維修人工費要調漲至$120美金		
        If Session("_Alerted") Is Nothing AndAlso Date.Today >= Date.Parse("2025/6/13") AndAlso Date.Today <= Date.Parse("2025/7/31") AndAlso (Session("_RepairID") = "AU_LAPTOP_KINGS" Or Session("_RepairID") = "NZ_PB_TECH") Then
            UI_Add_RMA_panel_iframe.Attributes.Add("src", "AU_NZ_MA_PRICE.html")
            UI_Add_RMA_panel_ModalPopupExtender.Show()
            Session("_Alerted") = "True"
        End If

        '2025/9/1 ~ : EU、UK、JP的維修人工費要調漲至$120美金		
        '2025/9/1 ~ : TW的維修人工費要調漲至$50美金	
        '2025/9/1 ~ : TW以外的維修人工費要調漲至$100美金	
        '修改共用寫法 by buck modify 2025/08/29
        '加入澳洲中心 by buck modify 2025/09/16
        Dim url As String = String.Format("../Announcements/Price_Adjustment.html?repair={0}&country={1}", Session("_RepairID"), Session("_CountryID"))
        If Session("_Alerted") Is Nothing AndAlso Date.Today >= Date.Parse("2025/8/1") AndAlso
            (
                Session("_RepairID") = GetDescriptionText(enmRepairCenter.CL_EU_Service_Center) Or
                Session("_RepairID") = GetDescriptionText(enmRepairCenter.CL_UK_Service_Centre) Or
                Session("_RepairID") = GetDescriptionText(enmRepairCenter.CL_JP_Service_Center) Or
                Session("_RepairID") = GetDescriptionText(enmRepairCenter.CL_TW_Service_Center) Or
                Session("_RepairID").IndexOf(GetDescriptionText(enmRepairCenter.AU)) > -1
            ) Then
            UI_Add_RMA_panel_iframe.Attributes.Add("src", url)
            UI_Add_RMA_panel_ModalPopupExtender.Show()
            Session("_Alerted") = "True"
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim i As Integer = 0
        Dim flagRole As Boolean = False

        '新網站
        Me.UI_lblRole_Master_Text.Text = _oLanguage.getText("Common", "067", ctlLanguage.eumType.Tag)
        Me.CustomerBtn.Text = _oLanguage.getText("RMA2", "001", ctlLanguage.eumType.Tag)
        Me.New_Request_Btn.Text = _oLanguage.getText("RMA2", "002", ctlLanguage.eumType.Tag)
        Me.AlertLabel.Text = _oLanguage.getText("RMA2", "093", ctlLanguage.eumType.Tag)

        '設定 角色 的工作項目
        Me.UI_butReceive_WorkList.Text = _oLanguage.getText("Common", "057", ctlLanguage.eumType.Tag)
        Me.UI_butRepair_WorkList.Text = _oLanguage.getText("Common", "058", ctlLanguage.eumType.Tag)
        Me.UI_butSale_WorkList.Text = _oLanguage.getText("Common", "059", ctlLanguage.eumType.Tag)
        Me.UI_butShip_ShippingList.Text = _oLanguage.getText("Common", "080", ctlLanguage.eumType.Tag)

        Me.UI_butReceive_WorkList.Visible = False
        Me.UI_butRepair_WorkList.Visible = False
        Me.UI_butSale_WorkList.Visible = False
        Me.UI_butShip_ShippingList.Visible = False

        Me.UI_butReceive_WorkList.CssClass = "worklist"
        Me.UI_butRepair_WorkList.CssClass = "worklist"
        Me.UI_butSale_WorkList.CssClass = "worklist"
        Me.UI_butShip_ShippingList.CssClass = "worklist"

        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        Dim arrRole() As String = Session("_Role").ToString().Split(",")
        If arrRole.Length > 1 Then
            For i = 0 To arrRole.Length - 1
                Select Case arrRole(i).Trim()
                    Case "1"
                        Me.UI_butReceive_WorkList.Visible = True

                    Case "2"
                        Me.UI_butRepair_WorkList.Visible = True

                    Case "3"
                        Me.UI_butSale_WorkList.Visible = True
                    Case "4"
                        Me.UI_butShip_ShippingList.Visible = True
                End Select
            Next
        End If

        Me.UI_butReceive_WorkList.CssClass = "btn btn-success"
        Me.UI_butRepair_WorkList.CssClass = "btn btn-danger"
        Me.UI_butSale_WorkList.CssClass = "btn btn-warning"
        Me.UI_butShip_ShippingList.CssClass = "btn btn-info"

        '一人會有多角色, 希望在介面上可以清楚的知道目前是在操作哪各角色-->利用介面上的切換角色的按鈕用顏色來判斷
        Dim arrOperationRole() As String = Session("_OperationRole").ToString().Split(",")
        If arrOperationRole.Length > 0 Then
            Select Case arrOperationRole(0).ToString.Trim()
                Case "1"
                    Me.UI_butReceive_WorkList.CssClass = "btn btn-primary"

                Case "2"
                    Me.UI_butRepair_WorkList.CssClass = "btn btn-primary"

                Case "3"
                    Me.UI_butSale_WorkList.CssClass = "btn btn-primary"
                Case "4"
                    Me.UI_butShip_ShippingList.CssClass = "btn btn-primary"
            End Select
        End If

        Me.Official_Website_Label.Text = _oLanguage.getText("RMA2", "071", ctlLanguage.eumType.Tag)
        Me.UI_globalRMAID.Attributes.Add("placeholder", _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag))

    End Sub

    Private Sub setScript_Processing()
        '取得Processing Text
        Dim sScript As String = ""
        sScript = sScript & "<script type=""text/javascript"">" & vbCrLf
        sScript = sScript & "var delMsg=""" & _oLanguage.getText("Processing", "001", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var CancelItemMsg=""" & _oLanguage.getText("Processing", "015", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var BarCodeMsg=""" & _oLanguage.getText("Processing", "016", ctlLanguage.eumType.Processing) & """;" & vbCrLf

        sScript = sScript & "var Progress_SaveMsg=""" & _oLanguage.getText("Processing", "002", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var Progress_LoadMsg=""" & _oLanguage.getText("Processing", "003", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var Progress_DelMsg=""" & _oLanguage.getText("Processing", "004", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var Progress_ExportMsg=""" & _oLanguage.getText("Processing", "005", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var Progress_ProcessMsg=""" & _oLanguage.getText("Processing", "006", ctlLanguage.eumType.Processing) & """;" & vbCrLf
        sScript = sScript & "var Progress_CancelMsg=""" & _oLanguage.getText("Processing", "013", ctlLanguage.eumType.Processing) & """;" & vbCrLf

        sScript = sScript & "var doubleConfirmMsg=""" & _oLanguage.getText("RMA", "237", ctlLanguage.eumType.Validator) & """;" & vbCrLf

        sScript = sScript & "</script>" & vbCrLf
        Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "delMsg", sScript)
    End Sub

    ''' <summary>
    ''' 收貨
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butReceive_WorkList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butReceive_WorkList.Click
        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        Session("_OperationRole") = "1"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "057", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Receive_WorkList.aspx")
    End Sub

    ''' <summary>
    ''' 維修
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butRepair_WorkList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butRepair_WorkList.Click
        Session("_OperationRole") = "2"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "058", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Repair_WorkList.aspx")
    End Sub

    ''' <summary>
    ''' 業務
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butSale_WorkList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butSale_WorkList.Click
        Session("_OperationRole") = "3"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "059", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Sales_WorkList.aspx")
    End Sub

    Protected Sub UI_butShip_ShippingList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_butShip_ShippingList.Click
        Session("_OperationRole") = "4"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "080", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Shipping_List.aspx")
    End Sub

#Region "右邊主板頁面"
    Protected Sub UI_butReceive_Drop_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UI_butReceive_Drop.SelectedIndexChanged

        If UI_butReceive_Drop.SelectedItem.Value = "--Select--" Then

        End If

        If UI_butReceive_Drop.SelectedItem.Value = "Received" Then
            Call UI_butReceive_WorkList_Click()
        End If

        If UI_butReceive_Drop.SelectedItem.Value = "Repaired" Then
            Call UI_butRepair_WorkList_Click()
        End If

        If UI_butReceive_Drop.SelectedItem.Value = "Sales" Then
            Call UI_butSale_WorkList_Click()
        End If

        If UI_butReceive_Drop.SelectedItem.Value = "Shipping" Then
            Call UI_butShip_ShippingList_Click()
        End If

    End Sub

    ''' <summary>
    ''' 收貨
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butReceive_WorkList_Click()
        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        Session("_OperationRole") = "1"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "057", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Receive_WorkList.aspx")
    End Sub

    ''' <summary>
    ''' 維修
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butRepair_WorkList_Click()
        Session("_OperationRole") = "2"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "058", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Repair_WorkList.aspx")
    End Sub

    ''' <summary>
    ''' 業務
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butSale_WorkList_Click()
        Session("_OperationRole") = "3"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "059", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Sales_WorkList.aspx")
    End Sub

    ''' <summary>
    ''' 出貨
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_butShip_ShippingList_Click()
        Session("_OperationRole") = "4"
        Session("_OperationRoleName") = _oLanguage.getText("Common", "080", ctlLanguage.eumType.Tag)
        Me.Server.Transfer("Shipping_List.aspx")
    End Sub

    Protected Sub New_Request_Btn_Click(sender As Object, e As EventArgs) Handles New_Request_Btn.Click
        '初始化
        Session("_dtRMADetail") = Nothing
        UI_Add_RMA_panel_iframe.Attributes.Add("src", "ProductInformation_01.aspx")
        UI_Add_RMA_panel_ModalPopupExtender.Show()
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub set_left_Controls()
        '取得Tag Text

        Me.UI_lblRepair.Text = _oLanguage.getText("Common", "036", ctlLanguage.eumType.Tag)
        Me.UI_lblAccount_ID_Text.Text = Session("_UserID").ToString()

        If Not (Session("CU_CONTACTPERSON") Is Nothing) Then
            Me.UI_lblAccountText.Text = Session("CU_CONTACTPERSON").ToString()
        Else
            Me.UI_lblAccountText.Text = Session("_UserID").ToString()
        End If


        If Session("_OperationRoleName").ToString.Trim <> "" Then
            Me.UI_lblRoleNameText.Text = _oLanguage.getText("Common", "067", ctlLanguage.eumType.Tag) & ":" & Session("_OperationRoleName")
        Else
            Me.UI_lblRoleNameText.Text = ""
        End If

        Me.UI_lblCUNAME.Text = Session("_RepairName").ToString()
        Me.UI_lblCUTEL.Text = Session("_RepairTEL").ToString()

        '身分:1.客戶, 2.公司(維修中心) ,3.End User
        If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
            Me.UI_tr_butReceive_Drop.Visible = False
            Me.UI_NewRequestPanel.Visible = True

            If Me.UI_lblCUTEL.Text = "+886286471166" Then
                AddressPanel1.Visible = True
            End If
        Else
            Me.UI_tr_butReceive_Drop.Visible = True
            Me.UI_NewRequestPanel.Visible = False
            Dim Received As String = _oLanguage.getText("Common", "057", ctlLanguage.eumType.Tag)
            Dim Repaired As String = _oLanguage.getText("Common", "058", ctlLanguage.eumType.Tag)
            Dim Sales As String = _oLanguage.getText("Common", "059", ctlLanguage.eumType.Tag)
            Dim Shipping As String = _oLanguage.getText("Common", "080", ctlLanguage.eumType.Tag)

            Dim Select_myListItem As New ListItem()
            Select_myListItem.Value = "--Select--"
            Select_myListItem.Text = "--Select--"

            Dim Received_myListItem As New ListItem()
            Received_myListItem.Value = "Received"
            Received_myListItem.Text = Received

            Dim Repaired_myListItem As New ListItem()
            Repaired_myListItem.Value = "Repaired"
            Repaired_myListItem.Text = Repaired

            Dim Sales_myListItem As New ListItem()
            Sales_myListItem.Value = "Sales"
            Sales_myListItem.Text = Sales

            Dim Shipping_myListItem As New ListItem()
            Shipping_myListItem.Value = "Shipping"
            Shipping_myListItem.Text = Shipping

            '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            Dim arrRole() As String = Session("_Role").ToString().Split(",")
            If arrRole.Length > 1 Then
                UI_butReceive_Drop.Items.Add(Select_myListItem)
                For i = 0 To arrRole.Length - 1
                    Select Case arrRole(i).Trim()
                        Case "1"
                            UI_butReceive_Drop.Items.Add(Received_myListItem)
                        Case "2"
                            UI_butReceive_Drop.Items.Add(Repaired_myListItem)
                        Case "3"
                            UI_butReceive_Drop.Items.Add(Sales_myListItem)
                        Case "4"
                            UI_butReceive_Drop.Items.Add(Shipping_myListItem)
                    End Select
                Next

            Else

            End If

        End If


    End Sub
#End Region
    Protected Sub CustomerBtn_Click(sender As Object, e As EventArgs) Handles CustomerBtn.Click
        Response.Redirect("Customer.aspx")
    End Sub

End Class


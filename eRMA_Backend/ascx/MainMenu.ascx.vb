Imports DataService
Imports DefLanguage
Imports RMA_Model

Partial Class ascx_MainMenu
    Inherits System.Web.UI.UserControl
    Dim _oComm As New Common
    Dim _ctlLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim helper As New Utility.LanguageHelper()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call QueryLanguage()


            '身分:1.客戶, 2.公司(維修中心)
            'New Request
            Me.UI_linButton06.Visible = False
            Me.UI_linButton01.NavigateUrl = "~/Request_Policy.aspx"
            Me.UI_linButton08.Visible = False 'JW/shaili/20150524
            Me.UI_linButton08.Target = "_blank" 'JW/shaili/20150602
            Me.UI_linButton08.NavigateUrl = "~/GoToRMA.aspx" 'JW/shaili/20150524

            Me.UI_linButton09.Visible = False '新增逾期查詢功能按鈕 by buck modify 20251120 

            If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
                Me.UI_linButton03.NavigateUrl = "~/Client_Status_List.aspx"
                Me.UI_linButton08.Visible = True 'JW/shaili/20150524

            Else
                If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                    Me.UI_linButton01.Visible = True
                Else
                    Me.UI_linButton01.Visible = False
                End If

                Me.UI_linButton03.NavigateUrl = "~/Request_Search.aspx"

                If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                    Me.UI_linButton03.NavigateUrl = "~/Sales_Status_Search.aspx"
                End If
            End If

            If Session("_Identity") = "3" Then 'JW/shaili/20150604
                Me.UI_linButton01.Visible = True
            End If

            '取得各角色的 待處理工作連結的 頁面
            'Wait For Processing
            '取消 SALES 的Wait For Processing功能150817
            If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                Me.UI_linButton02.Visible = False
            End If
            Me.UI_linButton02.NavigateUrl = "~/" & _oComm.getWorkListPage()


            '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            If Session("_Role").ToString().IndexOf("9") <> -1 Then
                Me.UI_linButton05.NavigateUrl = "~/MaintainAccount_Search.aspx"
                Me.UI_linButton08.Visible = True 'JW/shaili/20150524

                Me.UI_linButton09.NavigateUrl = "~/ExpiredQuery.aspx"
                Me.UI_linButton09.Visible = True '新增逾期查詢功能按鈕 by buck modify 20251120
            Else
                If Session("_Role").ToString().IndexOf("0") <> -1 Then
                    Me.UI_linButton05.NavigateUrl = "~/Customer.aspx"
                Else
                    Me.UI_linButton05.NavigateUrl = "~/MaintainAccount_Edit.aspx"
                    If Session("_Role").ToString().IndexOf("3") <> -1 Then 'JW/shaili/20150524
                        Me.UI_linButton08.Visible = True
                    End If
                End If
            End If

            'Temp data upload
            If Session("_Role").ToString().IndexOf("8") <> -1 Then
                Me.UI_linButton06.Visible = True
                Me.UI_linButton06.NavigateUrl = "~/Upload_List.aspx"
            End If


            '報表
            Dim sParm As String = "Reports=true"
            Me.UI_linButton04.NavigateUrl = "~/Report1_Search.aspx?" & _Crypto.Encrypt(sParm, "")
            If Session("_Identity") = "1" Then
                'Me.UI_linButton04.Visible = False
            End If


            If Session("_Role").ToString().IndexOf("7") <> -1 Then
                Me.UI_linButton07.Visible = True
                Me.UI_linButton07.NavigateUrl = "~/Warranty_SellingafterOrder.aspx"
            Else
                Me.UI_linButton07.Visible = False
            End If


            If Session("_LanguageID").ToString() = "003" Then
                Me.UI_linButton01.ImageUrl = "~/images/pic_04_j.gif"
                Me.UI_linButton02.ImageUrl = "~/images/pic_05_j.gif"
                Me.UI_linButton03.ImageUrl = "~/images/pic_06_j.gif"
                Me.UI_linButton04.ImageUrl = "~/images/pic_07_j.gif"
                Me.UI_linButton05.ImageUrl = "~/images/pic_08_j.gif"
                Me.UI_linButton06.ImageUrl = "~/images/pic_24_j.gif"
                Me.UI_linButton07.ImageUrl = "~/images/pic_menu_06_j.gif"
                Me.UI_linButton08.ImageUrl = "~/images/pic_menu_07_j.gif"
                Me.UI_linButton09.ImageUrl = "~/images/pic_10ExpiredQuery.gif"
            Else
                Me.UI_linButton01.ImageUrl = "~/images/pic_04.gif"
                Me.UI_linButton02.ImageUrl = "~/images/pic_05.gif"
                Me.UI_linButton03.ImageUrl = "~/images/pic_06.gif"
                Me.UI_linButton04.ImageUrl = "~/images/pic_07.gif"
                Me.UI_linButton05.ImageUrl = "~/images/pic_08.gif"
                Me.UI_linButton06.ImageUrl = "~/images/pic_24.gif"
                Me.UI_linButton07.ImageUrl = "~/images/pic_menu_06.gif"
                Me.UI_linButton08.ImageUrl = "~/images/pic_menu_07.gif"
                Me.UI_linButton09.ImageUrl = "~/images/pic_10ExpiredQuery.gif"
            End If

            If Session("_Comp_Admin").ToString().ToUpper() = "MPLUS" Then
                UI_linButton07.Visible = False
                UI_linButton08.Visible = False
            End If

        End If

    End Sub


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

        '調整多語系 by buck add 20251201
        helper.Apply(Me, Session("_LanguageID").ToString())

    End Sub



    ''' <summary>
    ''' 異動語系
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboLanguage.SelectedIndexChanged
        Dim oLoginInfo As New ctlLoginInfo

        Select Case Session("_Identity").ToString()
            Case "1"
                oLoginInfo.ChangeLanguage("Customer", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
                Session("_LanguageID") = oLoginInfo.getLanguageID("Customer", Session("_UserID").ToString())

            Case "2"
                oLoginInfo.ChangeLanguage("Manager", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
                Session("_LanguageID") = oLoginInfo.getLanguageID("Manager", Session("_UserID").ToString())
        End Select
        _ctlLanguage.reLoad(Session("_LanguageID"))

        Dim sURL As String = Me.Page.AppRelativeVirtualPath.Replace("~/", "")
        Dim javascript As String = "<script>window.location.href=window.location.href;</script>"
        'Dim javascript As String = "<script>window.document.location.href='" & sURL & "';</script>"
        Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType, "locationParent", javascript)
    End Sub



End Class

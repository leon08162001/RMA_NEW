Imports DataService
Imports DefLanguage


Partial Class ascx_MainMenu
    Inherits System.Web.UI.UserControl
    Dim _oComm As New Common
    Dim _ctlLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim _oLanguage As New ctlLanguage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then


            Dim userSettings As String = System.IO.Path.GetFileName(Request.PhysicalPath)
            If userSettings = "Client_FlowCase01_Worklist.aspx" Then

                Me.UI_linButton00.CssClass = "Main_a_Save"
                Query_Area_Lab.CssClass = "Main_a"

                ' Me.Unprocessed_li.Style("border-bottom") = "2px solid #7693F5"

            Else

                    Me.UI_linButton00.CssClass = "Main_a"
                    Query_Area_Lab.CssClass = "Main_a_Save"

                End If




            Me.UI_li_linChargeQUOTED.Visible = False
            Me.UI_li_tbWarrantyOrder.Visible = False
            Me.UI_li_linWarrantyBatch.Visible = False
            Me.UI_li_linSellingafterOrder.Visible = False

            If Session("_Role").ToString().IndexOf("3") <> -1 Then

            End If

            If Session("_Role").ToString().IndexOf("4") <> -1 Then

            End If

            If Session("_Role").ToString().IndexOf("2") <> -1 Then

            End If

            If Session("_Role").ToString().IndexOf("6") <> -1 Then

            End If

            If Session("_Role").ToString().IndexOf("7") <> -1 Then
                Me.UI_li_linChargeQUOTED.Visible = True
                Me.UI_li_tbWarrantyOrder.Visible = True
                Me.UI_li_linWarrantyBatch.Visible = True
                Me.UI_li_linSellingafterOrder.Visible = True
            End If

            If Session("_Role").ToString().IndexOf("9") <> -1 Then

            Else

            End If



            '身分:1.客戶, 2.公司(維修中心)
            'New Request
            Me.UI_linButton06.Visible = False
            Me.UI_linButton01.NavigateUrl = "~/Request_Policy.aspx"
            Me.UI_linButton08.Visible = False 'JW/shaili/20150524
            Me.UI_linButton08.Target = "_blank" 'JW/shaili/20150602
            Me.UI_linButton08.NavigateUrl = "~/GoToRMA.aspx" 'JW/shaili/20150524

            If Session("_Identity") = "1" Or Session("_Identity") = "3" Then
                Me.UI_linButton03.NavigateUrl = "~/Client_Status_List.aspx"
                Me.UI_linButton08.Visible = True 'JW/shaili/20150524
                Me.Unprocessed_li.Visible = True

                Me.UI_linButton09.NavigateUrl = "~/WarrantyQuery.aspx"
                Me.UI_linButton09.Text = _oLanguage.getText("Report", "003", ctlLanguage.eumType.Tag)
                Me.UI_linButton09.Visible = True

                Me.UI_linButton10.NavigateUrl = "~/RMA_Detail_Report.aspx"
                Me.UI_linButton10.Text = _oLanguage.getText("Report", "004", ctlLanguage.eumType.Tag)
                Me.UI_linButton10.Visible = True
            Else

                Me.Unprocessed_li.Visible = False
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

                'Me.UI_linButton01.ImageUrl = "~/images/pic_04_j.gif"
                'Me.UI_linButton02.ImageUrl = "~/images/pic_05_j.gif"
                'Me.UI_linButton03.ImageUrl = "~/images/pic_06_j.gif"
                'Me.UI_linButton04.ImageUrl = "~/images/pic_07_j.gif"
                'Me.UI_linButton05.ImageUrl = "~/images/pic_08_j.gif"
                'Me.UI_linButton06.ImageUrl = "~/images/pic_24_j.gif"
                'Me.UI_linButton07.ImageUrl = "~/images/pic_menu_06_j.gif"
                'Me.UI_linButton08.ImageUrl = "~/images/pic_menu_07_j.gif"
                Me.UI_linButton00.Text = Query_Language_Img("pic_u_05.gif")

                Me.UI_linButton01.Text = Query_Language_Img("pic_04_j.gif")
                Me.UI_linButton02.Text = Query_Language_Img("pic_05_j.gif")
                Me.UI_linButton03.Text = Query_Language_Img("pic_06_j.gif")
                Me.UI_linButton04.Text = Query_Language_Img("pic_07_j.gif")
                Me.UI_linButton05.Text = Query_Language_Img("pic_08_j.gif")
                Me.UI_linButton06.Text = Query_Language_Img("pic_24_j.gif")
                Me.UI_linButton07.Text = Query_Language_Img("pic_menu_06_j.gif")
                Me.UI_linButton08.Text = Query_Language_Img("pic_menu_07_j.gif")

                'Me.UI_linButton01_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                ''Me.UI_linButton02_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'Me.UI_linButton03_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'Me.UI_linButton04_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'Me.UI_linButton05_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'Me.UI_linButton06_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'Me.UI_linButton07_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                ''Me.UI_linButton08_Lab.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

            Else
                'Me.UI_linButton01.ImageUrl = "~/images/pic_04.gif"
                'Me.UI_linButton02.ImageUrl = "~/images/pic_05.gif"
                'Me.UI_linButton03.ImageUrl = "~/images/pic_06.gif"
                'Me.UI_linButton04.ImageUrl = "~/images/pic_07.gif"
                'Me.UI_linButton05.ImageUrl = "~/images/pic_08.gif"
                'Me.UI_linButton06.ImageUrl = "~/images/pic_24.gif"
                'Me.UI_linButton07.ImageUrl = "~/images/pic_menu_06.gif"
                'Me.UI_linButton08.ImageUrl = "~/images/pic_menu_07.gif"
                Me.UI_linButton00.Text = Query_Language_Img("pic_u_05.gif")

                Me.UI_linButton01.Text = Query_Language_Img("pic_04.gif")
                Me.UI_linButton02.Text = Query_Language_Img("pic_05.gif")
                Me.UI_linButton03.Text = Query_Language_Img("pic_06.gif")
                Me.UI_linButton04.Text = Query_Language_Img("pic_07.gif")
                Me.UI_linButton05.Text = Query_Language_Img("pic_08.gif")
                Me.UI_linButton06.Text = Query_Language_Img("pic_24.gif")
                Me.UI_linButton07.Text = Query_Language_Img("pic_menu_06.gif")
                Me.UI_linButton08.Text = Query_Language_Img("pic_menu_07.gif")



                'Me.UI_linButton01_Lab.Text = ""
                'Me.UI_linButton02_Lab.Text = ""
                'Me.UI_linButton03_Lab.Text = ""
                'Me.UI_linButton04_Lab.Text = ""
                'Me.UI_linButton05_Lab.Text = ""
                'Me.UI_linButton06_Lab.Text = ""
                'Me.UI_linButton07_Lab.Text = ""
                'Me.UI_linButton08_Lab.Text = ""

            End If

            If Session("_Comp_Admin").ToString().ToUpper() = "MPLUS" Then
                UI_linButton07.Visible = False
                UI_linButton08.Visible = False
            End If

            If Me.UI_linButton01.Text = "" Or Me.UI_linButton01.Visible = False Then
                Me.UI_linButton01_li.Visible = False
            End If

            If Me.UI_linButton02.Text = "" Or Me.UI_linButton02.Visible = False Then
                Me.UI_linButton02_li.Visible = False
            End If

            If Me.UI_linButton03.Text = "" Or Me.UI_linButton03.Visible = False Then
                Me.UI_linButton03_li.Visible = False
            End If

            If Me.UI_linButton04.Text = "" Or Me.UI_linButton04.Visible = False Then
                Me.UI_linButton04_li.Visible = False
            End If

            If Me.UI_linButton05.Text = "" Or Me.UI_linButton05.Visible = False Then
                Me.UI_linButton05_li.Visible = False
            End If

            If Me.UI_linButton06.Text = "" Or Me.UI_linButton06.Visible = False Then
                Me.UI_linButton06_li.Visible = False
            End If

            If Me.UI_linButton07.Text = "" Or Me.UI_linButton07.Visible = False Then
                Me.UI_linButton07_li.Visible = False
            End If

            If Me.UI_linButton08.Text = "" Or Me.UI_linButton08.Visible = False Then
                Me.UI_linButton08_li.Visible = False
            End If

            If Me.UI_linButton09.Text = "" Or Me.UI_linButton09.Visible = False Then
                Me.UI_linButton09_li.Visible = False
            End If

            If Me.UI_linButton10.Text = "" Or Me.UI_linButton10.Visible = False Then
                Me.UI_linButton10_li.Visible = False
            End If

			Me.UI_linButton01_li.Visible = False
			Me.UI_linButton02_li.Visible = False
			Me.UI_linButton05_li.Visible = False

            '不要顯示
            Me.UI_linButton10_li.Visible = False
        End If

        Call setControls()

    End Sub


    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Me.UI_linButton00.Text = _oLanguage.getText("RMA2", "003", ctlLanguage.eumType.Tag)
        Me.Query_Area_Lab.Text = _oLanguage.getText("RMA2", "004", ctlLanguage.eumType.Tag)
        Me.UI_linButton03.Text = _oLanguage.getText("RMA2", "005", ctlLanguage.eumType.Tag)
        Me.UI_linButton04.Text = _oLanguage.getText("RMA2", "006", ctlLanguage.eumType.Tag)
        Me.UI_linButton09.Text = _oLanguage.getText("RMA2", "007", ctlLanguage.eumType.Tag)
        Me.UI_linButton10.Text = _oLanguage.getText("RMA2", "008", ctlLanguage.eumType.Tag)
    End Sub

    '''' <summary>
    '''' 語系換選單 輸入圖片 => 文字
    '''' </summary>
    '''' <remarks></remarks>
    Public Function Query_Language_Img(ByVal url As String) As String
        Dim context As String = ""


        Select Case url
            Case "pic_u_menu_06.gif"
                context = "WARRANTY SELLING"
            Case "pic_u_24.gif"
                context = "IMPORT"
            Case "pic_u_24_j.gif"
                context = "輸入する"
            Case "pic_u_09.gif"
                context = "GLOBAL WEB"
            Case "pic_u_08.gif"
                context = ""    'SETTING  不顯示
            Case "pic_u_08_j.gif"
                context = "設 定"
            Case "pic_u_07.gif"
                context = "Part's Query"
            Case "pic_u_07_j.gif"
                context = "レポート"
            Case "pic_u_06.gif"
                context = "Status Query"
            Case "pic_u_06_j.gif"
                context = "進捗状況"
            Case "pic_u_05.gif"
                context = "WAIT FOR PROCESSING"
            Case "pic_u_05_j.gif"
                context = "見積状況"
            Case "pic_u_04.gif"
                context = ""   'New REQUEST 不顯示
            Case "pic_u_04_j.gif"
                context = "新しいリクエスト"
            Case "pic_menu_07.gif"
                context = ""  'REGISTRATION 不顯示
            Case "pic_menu_06.gif"
                context = "WARRANTY SELLING"
            Case "pic_24.gif"
                context = "IMPORT"
            Case "pic_24_j.gif"
                context = "輸入する"
            Case "pic_09.gif"
                context = "GLOBAL WEB"
            Case "pic_08.gif"
                context = ""   'SETTING  不顯示
            Case "pic_08_j.gif"
                context = "設定"
            Case "pic_07.gif"
                context = "Part's Query"
            Case "pic_07_j1.gif"
                context = "レポート"
            Case "pic_07_j.gif"
                context = "レポート"
            Case "pic_06.gif"
                context = "Status Query"
            Case "pic_06_j.gif"
                context = "進捗状況"
            Case "pic_05.gif"
                context = "" 'WAIT FOR PROCESSING 不顯示
            Case "pic_05_j.gif"
                context = "見積状況"
            Case "pic_04.gif"
                context = ""  'New REQUEST 不顯示
            Case "pic_04_j.gif"
                context = "依頼登録"
            Case Else
                context = ""
                Exit Select
        End Select


        Return context

    End Function

    '''' <summary>
    '''' 取得語系資料
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub QueryLanguage()
    '    Dim dtLanguage As New LanguageDTO.DEFLANGUAGEDataTable
    '    dtLanguage = _ctlLanguage.QueryByDefLanguage()

    '    Me.UI_cboLanguage.DataTextField = "DFL_NAME"
    '    Me.UI_cboLanguage.DataValueField = "DFL_NO"

    '    Me.UI_cboLanguage.DataSource = dtLanguage.DefaultView
    '    Me.UI_cboLanguage.DataBind()
    '    Me.UI_cboLanguage.SelectedValue = Session("_LanguageID").ToString().Trim()

    '    _ctlLanguage.reLoad(Session("_LanguageID"))


    'End Sub



    '''' <summary>
    '''' 異動語系
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Protected Sub UI_cboLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboLanguage.SelectedIndexChanged
    '    Dim oLoginInfo As New ctlLoginInfo

    '    Select Case Session("_Identity").ToString()
    '        Case "1"
    '            oLoginInfo.ChangeLanguage("Customer", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
    '            Session("_LanguageID") = oLoginInfo.getLanguageID("Customer", Session("_UserID").ToString())

    '        Case "2"
    '            oLoginInfo.ChangeLanguage("Manager", Session("_UserID").ToString(), Me.UI_cboLanguage.SelectedValue)
    '            Session("_LanguageID") = oLoginInfo.getLanguageID("Manager", Session("_UserID").ToString())
    '    End Select
    '    _ctlLanguage.reLoad(Session("_LanguageID"))

    '    Dim sURL As String = Me.Page.AppRelativeVirtualPath.Replace("~/", "")
    '    Dim javascript As String = "<script>window.location.href=window.location.href;</script>"
    '    'Dim javascript As String = "<script>window.document.location.href='" & sURL & "';</script>"
    '    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType, "locationParent", javascript)
    'End Sub



End Class

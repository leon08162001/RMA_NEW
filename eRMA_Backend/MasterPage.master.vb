Imports System.Drawing
Imports DefLanguage

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim _oLanguage As New ctlLanguage
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        'Me.Page.Title = Session("_Title").ToString().Trim()


        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            '身分:1.客戶, 2.公司(維修中心)
            Select Case Session("_Identity").ToString().Trim()
                Case "1"
                    Me.UI_LoginRole.Text = _oLanguage.getText("Common", "065", ctlLanguage.eumType.Tag) & " - "
                Case "2"
                    Me.UI_LoginRole.Text = _oLanguage.getText("Common", "036", ctlLanguage.eumType.Tag) & " - "
            End Select
            Me.UI_LoginName.Text = Me.UI_LoginName.Text & Session("_UserName").ToString().Trim()

            If _isDebug = True Then
                Me.UI_Model.Text = "測試模式"
                Me.UI_Model.ForeColor = Color.Red
                Me.UI_Model.Font.Size = 36
                Me.UI_Model.Font.Bold = True
                Me.UI_Model.Visible = True
            Else
                Me.UI_Model.Visible = False
            End If

            Call setControls()
        End If

        If Session("_LanguageID").ToString() = "003" Then
            Image21.ImageUrl = ResolveUrl("~/Images/pic_21_j.gif")
        Else
            Image21.ImageUrl = ResolveUrl("~/Images/pic_21.gif")
        End If


        Call setScript_Processing()

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim i As Integer = 0
        Dim flagRole As Boolean = False

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


        '一人會有多角色, 希望在介面上可以清楚的知道目前是在操作哪各角色-->利用介面上的切換角色的按鈕用顏色來判斷
        Dim arrOperationRole() As String = Session("_OperationRole").ToString().Split(",")
        If arrOperationRole.Length > 0 Then
            Select Case arrOperationRole(0).ToString.Trim()
                Case "1"
                    Me.UI_butReceive_WorkList.CssClass = "butOperation"

                Case "2"
                    Me.UI_butRepair_WorkList.CssClass = "butOperation"

                Case "3"
                    Me.UI_butSale_WorkList.CssClass = "butOperation"
                Case "4"
                    Me.UI_butShip_ShippingList.CssClass = "butOperation"
            End Select
        End If

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
End Class


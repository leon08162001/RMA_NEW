Partial Class Customer
    Inherits System.Web.UI.Page

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then

            '是否為管理人(0.否,1.是)
            If Session("_isManager") = "1" Then
                '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
                If Session("_Role").ToString().IndexOf("9") <> -1 Then
                    Me.UI_lblPreviousPage_Role.Text = "Admin"

                    If Not Me.PreviousPage Is Nothing Then
                        Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                        Dim UI_lblPreviousPage_CuNo As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_CuNo")
                        Me.UI_lblPreviousPage_CuNo.Text = UI_lblPreviousPage_CuNo.Text.ToString().Trim()
                    End If

                Else
                    Me.UI_lblPreviousPage_Role.Text = "Manager"
                    Me.UI_lblPreviousPage_CuNo.Text = Session("_CustomerID").ToString().Trim()

                End If
                Me.ucCustomerAdmin.Visible = True

            Else
                Me.UI_lblPreviousPage_CuNo.Text = Session("_CustomerID").ToString().Trim()
                Me.UI_lblPreviousPage_CuusID.Text = Session("_UserID").ToString().Trim()
                Me.ucCustomerUser.Visible = True
            End If



        End If
    End Sub
#End Region

End Class

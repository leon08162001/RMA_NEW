Imports DataService
Imports DefLanguage

Partial Class Warranty
    Inherits System.Web.UI.Page
    Dim _oLanguage As New ctlLanguage

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call QueryData()
            Call setControls()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyTittle.Text = _oLanguage.getText("Warranty", "002", ctlLanguage.eumType.Tag)

        Me.UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData()
        Dim oWarranty As New ctlWarranty
        Dim dtWarranty As New WarrantyDTO.WarrantyDataTable

        dtWarranty = oWarranty.Query("")

        Me.UI_Warranty.DataSource = dtWarranty
        Me.UI_Warranty.DataBind()
    End Sub

    Protected Sub UI_Warranty_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_Warranty.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Warranty", "003", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Warranty", "004", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Warranty", "005", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Warranty", "006", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Warranty", "007", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_WarrVisible As DropDownList = e.Row.FindControl("UI_WarrVisible")
            Dim UI_WarrUnit As DropDownList = e.Row.FindControl("UI_WarrUnit")
            UI_WarrVisible.Items(0).Text = _oLanguage.getText("Warranty", "011", ctlLanguage.eumType.Tag)
            UI_WarrVisible.Items(1).Text = _oLanguage.getText("Warranty", "012", ctlLanguage.eumType.Tag)
            UI_WarrUnit.Items(0).Text = _oLanguage.getText("Warranty", "008", ctlLanguage.eumType.Tag)
            UI_WarrUnit.Items(1).Text = _oLanguage.getText("Warranty", "009", ctlLanguage.eumType.Tag)
            UI_WarrUnit.Items(2).Text = _oLanguage.getText("Warranty", "010", ctlLanguage.eumType.Tag)

            Dim UI_Unit As Label = e.Row.FindControl("UI_Unit")
            Dim UI_Visible As Label = e.Row.FindControl("UI_Visible")
            Dim oDropWarrUnit As DropDownList = e.Row.FindControl("UI_WarrUnit")
            Dim oDropVisible As DropDownList = e.Row.FindControl("UI_WarrVisible")

            oDropWarrUnit.SelectedValue = UI_Unit.Text.Trim()
            oDropVisible.SelectedValue = UI_Visible.Text.Trim()

            Dim UI_WarrNumber As TextBox = e.Row.FindControl("UI_WarrNumber")
            Dim rfv_WarrNumber As RequiredFieldValidator = e.Row.FindControl("rfv_WarrNumber")
            rfv_WarrNumber.ControlToValidate = UI_WarrNumber.ID
            Call setValidationMessage(rfv_WarrNumber)
        End If
    End Sub

    ''' <summary>
    ''' 設定 Validation 的錯誤訊息
    ''' </summary>
    ''' <param name="oValidator"></param>
    ''' <remarks></remarks>
    Private Sub setValidationMessage(ByVal oValidator As Object)
        Dim sErrorMessage As String = ""
        sErrorMessage = _oLanguage.getText("Warranty", "013", ctlLanguage.eumType.Validator)
        oValidator.ErrorMessage = sErrorMessage.ToString().Trim()
    End Sub

    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oGridView As GridView = Me.UI_Warranty
        Dim oCommon As New Common
        Dim oWarranty As New ctlWarranty
        Dim dtWarranty As New WarrantyDTO.WarrantyDataTable

        Try
            Dim i As Integer = 0
            For i = 0 To oGridView.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrantyRow = dtWarranty.NewWarrantyRow

                Dim UI_WarrID As Label = oGridView.Rows(i).Cells(0).Controls(1).FindControl("UI_WarrID")
                Dim UI_WarrNumber As TextBox = oGridView.Rows(i).Cells(1).FindControl("UI_WarrNumber")
                Dim UI_WarrUnit As DropDownList = oGridView.Rows(i).Cells(2).FindControl("UI_WarrUnit")
                Dim UI_WarrVisible As DropDownList = oGridView.Rows(i).Cells(5).FindControl("UI_WarrVisible")

                dr.WARR_ID = UI_WarrID.Text.ToString().Trim()
                dr.WARR_NUMBER = Convert.ToInt32(UI_WarrNumber.Text.ToString().Trim())
                dr.WARR_UNIT = Convert.ToInt32(UI_WarrUnit.SelectedValue.ToString().Trim())
                dr.WARR_TYPE = 1
                dr.WARR_VISIBLE = Convert.ToInt32(UI_WarrVisible.SelectedValue.ToString().Trim())
                dr.WARR_AD = Session("_UserID").ToString().Trim()                '資料建立人帳號
                dr.WARR_ADNAME = Session("_UserName").ToString().Trim()          '資料建立人姓名
                dr.WARR_CSTMP = Date.Now                                         '資料建立時間
                dr.WARR_LUAD = Session("_UserID").ToString().Trim()              '最後修改人
                dr.WARR_LUADNAME = Session("_UserName").ToString().Trim()        '最後修改日期
                dr.WARR_LUSTMP = Date.Now                                        '最後修改時間
                dr.WARR_MARK = 0

                dtWarranty.AddWarrantyRow(dr)
            Next

            oWarranty.SaveEdit(dtWarranty)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
    End Sub

End Class

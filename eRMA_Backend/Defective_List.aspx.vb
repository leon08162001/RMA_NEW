Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class Defective_List
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_DFLNO") = ""
            Me.ViewState("_DefectiveName") = ""
            Me.ViewState("_SortExpression") = "DEFECTIVE_NO"
            Me.ViewState("_SortDirection") = "asc"

            Session("_dtDefective") = Nothing

            Call setDefault()
            Call QueryData(0)
        End If
    End Sub
#End Region

    Private Sub setDefault()
        '語系
        oCommon.getDefLaguageByDropDownList(Me.UI_cboLanguage, "")
        oCommon.getDefLaguageByDropDownList(Me.UI_cboLanguageAdd, "")

        Me.rfvDefectiveNo.ErrorMessage = _oLanguage.getText("Defective", "015", ctlLanguage.eumType.Validator)
        Me.rfvDefectiveName.ErrorMessage = _oLanguage.getText("Defective", "013", ctlLanguage.eumType.Validator)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Defective", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblDefectiveTittle.Text = _oLanguage.getText("Defective", "002", ctlLanguage.eumType.Tag)

        Me.UI_lblLanguage.Text = _oLanguage.getText("Defective", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblDefectiveName.Text = _oLanguage.getText("Defective", "004", ctlLanguage.eumType.Tag)

        Me.UI_lblLanguageAdd.Text = _oLanguage.getText("Defective", "010", ctlLanguage.eumType.Tag)
        Me.UI_lblDefectiveNoAdd.Text = _oLanguage.getText("Defective", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblDefectiveNameAdd.Text = _oLanguage.getText("Defective", "004", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)

        Me.UI_dvDefective.Columns(0).HeaderText = _oLanguage.getText("Defective", "101", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(1).HeaderText = _oLanguage.getText("Defective", "003", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(2).HeaderText = _oLanguage.getText("Defective", "004", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(3).HeaderText = _oLanguage.getText("Defective", "005", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(4).HeaderText = _oLanguage.getText("Defective", "008", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(5).HeaderText = _oLanguage.getText("Defective", "009", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(6).HeaderText = _oLanguage.getText("Defective", "016", ctlLanguage.eumType.Tag)
        Me.UI_dvDefective.Columns(7).HeaderText = _oLanguage.getText("Defective", "017", ctlLanguage.eumType.Tag)

    End Sub

    ''' <summary>
    ''' Show
    ''' </summary>
    ''' <param name="iPageIndex"></param>
    ''' <remarks></remarks>
    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oDefective As New ctlDefective
        Dim dtDefective As New DefectiveDTO.vwDefectiveDataTable
        Dim sDFLNO As String = Me.ViewState("_DFLNO")
        Dim sDefectiveName As String = Me.ViewState("_DefectiveName")

        dtDefective = oDefective.QueryByKey(sDFLNO, "", sDefectiveName)

        Call ArrangementData(dtDefective)
        Session("_dtDefective") = dtDefective

        Dim dvDefective As DataView = dtDefective.DefaultView
        dvDefective.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call Defective_DataBind(dvDefective, iPageIndex)
    End Sub

    Private Sub Defective_DataBind(ByVal dvDefective As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvDefective.PageSize = _PageSize
        Me.UI_dvDefective.PageIndex = iPageIndex
        Me.UI_dvDefective.DataSource = dvDefective
        Me.UI_dvDefective.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtDefective As DefectiveDTO.vwDefectiveDataTable)
        Dim i As Integer = 0
        Dim iValue As String = ""

        dtDefective.Columns.Add("Status")
        dtDefective.Columns.Add("SeqID")
        dtDefective.Columns.Add("LUSTMP")

        For i = 0 To dtDefective.Rows.Count - 1
            dtDefective.Rows(i)("SeqID") = i + 1

            '狀態
            iValue = dtDefective.Rows(i).Item("DEFECTIVE_VISIBLE").ToString.Trim()
            If iValue.ToString.Trim() = "1" Then
                dtDefective.Rows(i).Item("Status") = _oLanguage.getText("Defective", "006", ctlLanguage.eumType.Tag)
            Else
                dtDefective.Rows(i).Item("Status") = _oLanguage.getText("Defective", "007", ctlLanguage.eumType.Tag)
            End If

            dtDefective.Rows(i)("LUSTMP") = Convert.ToDateTime(dtDefective.Rows(i).Item("DEFECTIVE_LUSTMP").ToString.Trim()).ToShortDateString()
        Next
    End Sub

    Protected Sub UI_dvDefective_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvDefective.RowCommand
        If e.CommandName <> "cmdEdit" And e.CommandName <> "cmdSave" And e.CommandName <> "cmdDel" Then
            Exit Sub
        End If

        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow
        row = Me.UI_dvDefective.Rows(iIndex)

        Dim UI_DefectiveDFLNO As Label = row.FindControl("UI_DefectiveDFLNO")
        Dim UI_DefectiveNo As Label = row.FindControl("UI_DefectiveNo")
        Dim UI_lblVisible As Label = row.FindControl("UI_lblVisible")
        Dim UI_lblDefecticeName As Label = row.FindControl("UI_lblDefecticeName")
        Dim UI_txtDefecticeName As TextBox = row.FindControl("UI_txtDefecticeName")
        Dim UI_lblDefecticeStatus As Label = row.FindControl("UI_lblDefecticeStatus")
        Dim UI_cboStatus As DropDownList = row.FindControl("UI_cboStatus")
        Dim UI_cmdEdit As Button = row.FindControl("UI_cmdEdit")
        Dim UI_cmdSave As Button = row.FindControl("UI_cmdSave")
        Dim UI_cmdDel As Button = row.FindControl("UI_cmdDel")

        Select Case e.CommandName
            Case "cmdEdit"
                UI_lblDefecticeName.Visible = False
                UI_lblDefecticeStatus.Visible = False
                UI_cmdEdit.Visible = False
                UI_cmdDel.Enabled = False

                UI_txtDefecticeName.Visible = True
                UI_cboStatus.Visible = True
                UI_cmdSave.Visible = True

                UI_cboStatus.SelectedValue = UI_lblVisible.Text.ToString().Trim()

            Case "cmdSave"
                UI_lblDefecticeName.Visible = True
                UI_lblDefecticeStatus.Visible = True
                UI_cmdEdit.Visible = True
                UI_cmdDel.Enabled = True

                UI_txtDefecticeName.Visible = False
                UI_cboStatus.Visible = False
                UI_cmdSave.Visible = False

                Call Save(UI_DefectiveNo.Text.Trim(), UI_txtDefecticeName.Text.Trim(), UI_cboStatus.SelectedValue.ToString().Trim(), UI_DefectiveDFLNO.Text.Trim())

            Case "cmdDel"
                Call Del(UI_DefectiveNo.Text.ToString().Trim(), UI_DefectiveDFLNO.Text.Trim())

        End Select

    End Sub

    Protected Sub UI_dvDefective_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvDefective.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim UI_txtDefecticeName As TextBox = e.Row.FindControl("UI_txtDefecticeName")
            Dim rfv_DefecticeName As RequiredFieldValidator = e.Row.FindControl("rfv_DefecticeName")
            Dim UI_cboStatus As DropDownList = e.Row.FindControl("UI_cboStatus")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdSave As Button = e.Row.FindControl("UI_cmdSave")
            Dim UI_cmdDel As Button = e.Row.FindControl("UI_cmdDel")

            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
            UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
            UI_cboStatus.Items(0).Text = _oLanguage.getText("Defective", "006", ctlLanguage.eumType.Tag)
            UI_cboStatus.Items(1).Text = _oLanguage.getText("Defective", "007", ctlLanguage.eumType.Tag)

            rfv_DefecticeName.ControlToValidate = UI_txtDefecticeName.ID
            rfv_DefecticeName.ErrorMessage = _oLanguage.getText("Defective", "013", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If
    End Sub

    Protected Sub UI_dvDefective_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvDefective.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtDefective") Is Nothing Then
            Dim dtDefective As DefectiveDTO.vwDefectiveDataTable = Session("_dtDefective")
            Dim dvDefective As DataView = dtDefective.DefaultView
            Call Defective_DataBind(dvDefective, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvDefective_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvDefective.Sorting

        If Me.ViewState("_SortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_SortDirection") = "asc"
        Else
            If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_SortDirection") = "desc"
            Else
                Me.ViewState("_SortDirection") = "asc"
            End If
        End If
        Me.ViewState("_SortExpression") = e.SortExpression

        If IsNothing(Session("_dtDefective")) = False Then
            Dim dtDefective As DataTable = Session("_dtDefective")
            Dim dvDefective As DataView = dtDefective.DefaultView
            dvDefective.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call Defective_DataBind(dvDefective, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvDefective.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvDefective.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvDefective.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvDefective.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvDefective.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvDefective.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Me.ViewState("_DFLNO") = Me.UI_cboLanguage.SelectedValue.ToString().Trim()
        Me.ViewState("_DefectiveName") = Me.UI_txtDefectiveName.Text.ToString().Trim()

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oDefective As New ctlDefective
        Dim dtDefective As New DefectiveDTO.vwDefectiveDataTable

        Try
            Dim dr As DefectiveDTO.vwDefectiveRow = dtDefective.NewvwDefectiveRow

            dr.DEFECTIVE_NO = Me.UI_txtDefectiveNoAdd.Text.ToString().Trim()
            dr.DEFECTIVE_NAME = Me.UI_txtDefectiveNameAdd.Text.ToString().Trim()
            dr.DEFECTIVE_VISIBLE = 1
            dr.DEFECTIVE_AD = Session("_UserID")
            dr.DEFECTIVE_ADNAME = Session("_UserName")
            dr.DEFECTIVE_CSTMP = Date.Now
            dr.DEFECTIVE_LUAD = Session("_UserID")
            dr.DEFECTIVE_LUADNAME = Session("_UserName")
            dr.DEFECTIVE_LUSTMP = Date.Now
            dr.DEFECTIVE_DFLNO = Me.UI_cboLanguageAdd.SelectedValue.ToString().Trim()

            dtDefective.AddvwDefectiveRow(dr)
            oDefective.SaveAdd(dtDefective)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                Me.ucMessage.showMessageByAlert(sMsg)

                Call QueryData(0)
                Me.UI_txtDefectiveNoAdd.Text = ""
                Me.UI_txtDefectiveNameAdd.Text = ""
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="DefectiveNo"></param>
    ''' <param name="DefecticeName"></param>
    ''' <param name="Status"></param>
    ''' <remarks></remarks>
    Private Sub Save(ByVal DefectiveNo As String, ByVal DefecticeName As String, ByVal Status As String, ByVal DefectiveDFLNo As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim oDefective As New ctlDefective
        Dim dtDefective As New DefectiveDTO.vwDefectiveDataTable

        Try
            Dim dr As DefectiveDTO.vwDefectiveRow = dtDefective.NewvwDefectiveRow

            dr.DEFECTIVE_NO = DefectiveNo.Trim()
            dr.DEFECTIVE_NAME = DefecticeName.Trim()
            dr.DEFECTIVE_VISIBLE = Convert.ToInt32(Status.Trim())
            dr.DEFECTIVE_AD = Session("_UserID")
            dr.DEFECTIVE_ADNAME = Session("_UserName")
            dr.DEFECTIVE_CSTMP = Date.Now
            dr.DEFECTIVE_LUAD = Session("_UserID")
            dr.DEFECTIVE_LUADNAME = Session("_UserName")
            dr.DEFECTIVE_LUSTMP = Date.Now
            dr.DEFECTIVE_DFLNO = DefectiveDFLNo.Trim()

            dtDefective.AddvwDefectiveRow(dr)
            oDefective.SaveEdit(dtDefective)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageByAlert(sMsg)

                Call QueryData(0)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="DefectiveNo"></param>
    ''' <remarks></remarks>
    Private Sub Del(ByVal DefectiveNo As String, ByVal DefectiveDFLNo As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oDefective As New ctlDefective
        Try
            oDefective.SaveDel(DefectiveNo, DefectiveDFLNo)

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.DelOK)
                Me.ucMessage.showMessageByAlert(sMsg)

                Call QueryData(0)
            End If
        End Try
    End Sub

End Class

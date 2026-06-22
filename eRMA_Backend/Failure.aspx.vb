Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Failure
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtFailure") = Nothing
            Me.ViewState("_SortExpression") = "FAR_NO"
            Me.ViewState("_SortDirection") = "asc"

            Call setControls()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_FARCNO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_FARCNO")
                Dim UI_lblPreviousPage_DFLNO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_DFLNO")

                Me.UI_lblPreviousPage_FARCNO.Text = UI_lblPreviousPage_FARCNO.Text.ToString().Trim()
                Me.UI_lblPreviousPage_DFLNO.Text = UI_lblPreviousPage_DFLNO.Text.ToString().Trim()

                Call QueryFailureHead()
                Call QueryFailureItemTemplate(0)
            End If
        End If
    End Sub
#End Region

    Private Sub setControls()
        Me.rfv_txtFailureNo.ErrorMessage = _oLanguage.getText("Failure", "020", ctlLanguage.eumType.Validator)
        Me.rfv_txtFailureName.ErrorMessage = _oLanguage.getText("Failure", "021", ctlLanguage.eumType.Validator)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Failure", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCategory.Text = _oLanguage.getText("Failure", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblLanguageClass.Text = _oLanguage.getText("Failure", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureNoClass.Text = _oLanguage.getText("Failure", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureNameClass.Text = _oLanguage.getText("Failure", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblStatusClass.Text = _oLanguage.getText("Failure", "015", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureTittle.Text = _oLanguage.getText("Failure", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureNo.Text = _oLanguage.getText("Failure", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureName.Text = _oLanguage.getText("Failure", "005", ctlLanguage.eumType.Tag)

        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)

        Me.UI_dvFailureReason.Columns(1).HeaderText = _oLanguage.getText("Failure", "004", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(2).HeaderText = _oLanguage.getText("Failure", "005", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(3).HeaderText = _oLanguage.getText("Failure", "015", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(4).HeaderText = _oLanguage.getText("Failure", "013", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(5).HeaderText = _oLanguage.getText("Failure", "014", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(6).HeaderText = _oLanguage.getText("Failure", "027", ctlLanguage.eumType.Tag)
        Me.UI_dvFailureReason.Columns(7).HeaderText = _oLanguage.getText("Failure", "029", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryFailureHead()
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim dtFailure As New FailureDTO.tmpFailureReasonsDataTable

        dtFailure = oFailure.QueryKey(Me.UI_lblPreviousPage_DFLNO.Text.ToString().Trim(), Me.UI_lblPreviousPage_FARCNO.Text.ToString().Trim())

        If dtFailure.Rows.Count > 0 Then
            Dim drFailure As FailureDTO.tmpFailureReasonsRow = dtFailure.Rows(0)

            Me.UI_lblLanguageClassText.Text = drFailure.DFL_NAME.ToString().Trim()
            Me.UI_lblFailureNoClassText.Text = drFailure.FARC_NO.ToString().Trim()
            Me.UI_FailureNameClassText.Text = drFailure.FARC_NAME.ToString().Trim()

            If drFailure.FARC_VISIBLE.ToString().Trim() = "1" Then
                Me.UI_lblStatusClassText.Text = _oLanguage.getText("Failure", "016", ctlLanguage.eumType.Tag)
            Else
                Me.UI_lblStatusClassText.Text = _oLanguage.getText("Failure", "017", ctlLanguage.eumType.Tag)
            End If
        End If

    End Sub

    Private Sub QueryFailureItemTemplate(ByVal iPageIndex As Integer)
        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailure As New FailureDTO.tmpFailureReasonsDataTable

        dtFailure = oFailure.QueryALL(Me.UI_lblPreviousPage_DFLNO.Text, Me.UI_lblPreviousPage_FARCNO.Text.ToString().Trim())

        Session("_dtFailure") = dtFailure
        Call ArrangementData(dtFailure)
        Dim dvFailure As DataView = dtFailure.DefaultView
        dvFailure.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call FailureReasons_DataBind(dvFailure, iPageIndex)
    End Sub

    Private Sub FailureReasons_DataBind(ByVal dvFailure As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvFailureReason.PageSize = _PageSize
        Me.UI_dvFailureReason.PageIndex = iPageIndex
        Me.UI_dvFailureReason.DataSource = dvFailure
        Me.UI_dvFailureReason.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtFailure As FailureDTO.tmpFailureReasonsDataTable)
        Dim i As Integer = 0

        If dtFailure.Columns("SeqID") Is Nothing Then
            dtFailure.Columns.Add("SeqID")
            dtFailure.Columns.Add("Status")
            dtFailure.Columns.Add("LUSTMP")
        End If

        For i = 0 To dtFailure.Rows.Count - 1
            dtFailure.Rows(i)("SeqID") = i + 1

            If dtFailure.Rows(i).Item("FAR_VISIBLE").ToString().Trim() = "1" Then
                dtFailure.Rows(i)("Status") = _oLanguage.getText("Failure", "016", ctlLanguage.eumType.Tag)
            Else
                dtFailure.Rows(i)("Status") = _oLanguage.getText("Failure", "017", ctlLanguage.eumType.Tag)
            End If

            dtFailure.Rows(i)("LUSTMP") = Convert.ToDateTime(dtFailure.Rows(i).Item("FAR_LUSTMP").ToString()).ToShortDateString()
        Next
    End Sub

    Protected Sub UI_dvFailureReason_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvFailureReason.RowCommand
        If e.CommandName <> "cmdEdit" And e.CommandName <> "cmdSave" And e.CommandName <> "cmdDel" Then
            Exit Sub
        End If

        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow
        row = Me.UI_dvFailureReason.Rows(iIndex)

        Dim FarcDFLNo As String = Me.UI_lblPreviousPage_DFLNO.Text.ToString().Trim()
        Dim FarcN0 As String = Me.UI_lblPreviousPage_FARCNO.Text.ToString().Trim()
        Dim UI_FAR_NO As Label = row.FindControl("UI_FAR_NO")
        Dim UI_FAR_VISIBLE As Label = row.FindControl("UI_FAR_VISIBLE")
        Dim UI_lblFarName As Label = row.FindControl("UI_lblFarName")
        Dim UI_txtFarName As TextBox = row.FindControl("UI_txtFarName")
        Dim UI_lblStatus As Label = row.FindControl("UI_lblStatus")
        Dim UI_cboStatus As DropDownList = row.FindControl("UI_cboStatus")
        Dim UI_cmdEdit As Button = row.FindControl("UI_cmdEdit")
        Dim UI_cmdSave As Button = row.FindControl("UI_cmdSave")
        Dim UI_cmdDel As Button = row.FindControl("UI_cmdDel")

        Select Case e.CommandName
            Case "cmdEdit"
                UI_lblFarName.Visible = False
                UI_lblStatus.Visible = False
                UI_cmdEdit.Visible = False
                UI_cmdDel.Enabled = False

                UI_txtFarName.Visible = True
                UI_cboStatus.Visible = True
                UI_cmdSave.Visible = True

                UI_cboStatus.SelectedValue = UI_FAR_VISIBLE.Text.ToString().Trim()

            Case "cmdSave"
                UI_lblFarName.Visible = True
                UI_lblStatus.Visible = True
                UI_cmdEdit.Visible = True
                UI_cmdDel.Enabled = True

                UI_txtFarName.Visible = False
                UI_cboStatus.Visible = False
                UI_cmdSave.Visible = False

                Call Save(FarcDFLNo, FarcN0.Trim(), UI_FAR_NO.Text.Trim(), UI_txtFarName.Text.Trim(), UI_cboStatus.SelectedValue.Trim())

            Case "cmdDel"

                Call Del(FarcDFLNo, FarcN0.Trim(), UI_FAR_NO.Text.Trim())
        End Select

    End Sub

    Protected Sub UI_dvFailureReason_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvFailureReason.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_dvFailureReason.PageIndex * Me.UI_dvFailureReason.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_txtFarName As TextBox = e.Row.FindControl("UI_txtFarName")
            Dim rfvFailureName As RequiredFieldValidator = e.Row.FindControl("rfvFailureName")
            Dim UI_FAR_VISIBLE As Label = e.Row.FindControl("UI_FAR_VISIBLE")
            Dim UI_cboStatus As DropDownList = e.Row.FindControl("UI_cboStatus")
            Dim UI_cmdSave As Button = e.Row.FindControl("UI_cmdSave")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdDel As Button = e.Row.FindControl("UI_cmdDel")

            UI_cboStatus.Items(0).Text = _oLanguage.getText("Failure", "016", ctlLanguage.eumType.Tag)
            UI_cboStatus.Items(1).Text = _oLanguage.getText("Failure", "017", ctlLanguage.eumType.Tag)
            UI_cboStatus.SelectedValue = UI_FAR_VISIBLE.Text.ToString().Trim()
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
            UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)

            rfvFailureName.ControlToValidate = UI_txtFarName.ID
            rfvFailureName.ErrorMessage = _oLanguage.getText("Failure", "021", ctlLanguage.eumType.Validator)
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If

                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "DataControlPagerLinkButton".ToLower() Then
                    Dim oLinkButton As LinkButton = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLinkButton.Attributes.Add("onclick", "onProgress('Process')")
                End If

            Next
        End If

    End Sub

    Protected Sub UI_dvFailureReason_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvFailureReason.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtFailure") Is Nothing Then
            Dim dtFailure As FailureDTO.tmpFailureReasonsDataTable = Session("_dtFailure")
            Dim dvFailure As DataView = dtFailure.DefaultView
            Call FailureReasons_DataBind(dvFailure, iPageIndex)

        Else
            Call QueryFailureItemTemplate(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvFailureReason_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvFailureReason.Sorting

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

        If IsNothing(Session("_dtFailure")) = False Then
            Dim dtFailure As DataTable = Session("_dtFailure")
            Dim dvFailure As DataView = dtFailure.DefaultView
            dvFailure.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call FailureReasons_DataBind(dvFailure, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvFailureReason.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvFailureReason.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvFailureReason.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvFailureReason.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvFailureReason.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvFailureReason.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    Private Sub Save(ByVal FarDFLNo As String, ByVal FarcNo As String, ByVal FarNo As String, ByVal FarName As String, ByVal Status As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailure As New FailureDTO.FailureReasonsDataTable
        Dim dr As FailureDTO.FailureReasonsRow = dtFailure.NewFailureReasonsRow

        Try
            dr.FAR_DFLNO = FarDFLNo.Trim()
            dr.FAR_FARCNO = FarcNo.Trim()
            dr.FAR_NO = FarNo.Trim()
            dr.FAR_REASON = FarName.Trim()
            dr.FAR_VISIBLE = Convert.ToInt32(Status.Trim())
            dr.FAR_AD = Session("_UserID")
            dr.FAR_ADNAME = Session("_UserName")
            dr.FAR_CSTMP = Date.Now
            dr.FAR_LUAD = Session("_UserID")
            dr.FAR_LUADNAME = Session("_UserName")
            dr.FAR_LUSTMP = Date.Now

            dtFailure.AddFailureReasonsRow(dr)
            oFailure.SaveEdit(dtFailure)

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

                Call QueryFailureHead()
                Call QueryFailureItemTemplate(0)
            End If
        End Try

    End Sub

    Private Sub Del(ByVal FarDFLNo As String, ByVal FarcN0 As String, ByVal FarNo As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasons

        Try
            oFailure.SaveDel(FarDFLNo, FarcN0, FarNo)

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

                Call QueryFailureHead()
                Call QueryFailureItemTemplate(0)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 不良原因代碼-新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAdd.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailure As New FailureDTO.FailureReasonsDataTable
        Dim dr As FailureDTO.FailureReasonsRow = dtFailure.NewFailureReasonsRow

        Try
            dr.FAR_DFLNO = Me.UI_lblPreviousPage_DFLNO.Text.ToString().Trim()
            dr.FAR_FARCNO = Me.UI_lblPreviousPage_FARCNO.Text.ToString().Trim()
            dr.FAR_NO = Me.UI_txtFailureNo.Text.ToString().Trim()
            dr.FAR_REASON = Me.UI_txtFailureName.Text.ToString().Trim()
            dr.FAR_VISIBLE = 1
            dr.FAR_AD = Session("_UserID")
            dr.FAR_ADNAME = Session("_UserName")
            dr.FAR_CSTMP = Date.Now
            dr.FAR_LUAD = Session("_UserID")
            dr.FAR_LUADNAME = Session("_UserName")
            dr.FAR_LUSTMP = Date.Now

            dtFailure.AddFailureReasonsRow(dr)
            oFailure.SaveAdd(dtFailure)

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

                Call QueryFailureHead()
                Call QueryFailureItemTemplate(0)

                Me.UI_txtFailureNo.Text = ""
                Me.UI_txtFailureName.Text = ""
            End If
        End Try
    End Sub

End Class

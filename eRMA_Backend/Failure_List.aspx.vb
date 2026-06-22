Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Failure_List
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtFailureClass") = Nothing
            Me.ViewState("_FARC_DFLNO") = ""
            Me.ViewState("_FARC_NO") = "-1"

            Me.ViewState("_SortExpression") = "FARC_NO"
            Me.ViewState("_SortDirection") = "asc"

            Call setControls()
            Call QueryData(0)
        End If
    End Sub
#End Region

    Private Sub setControls()
        Dim sClientID As String = Me.UI_cboLanguage_Search.ClientID & "," & Me.UI_cboLanguage.ClientID
        Me.ucProgressStatus.NotpostBackElement = sClientID

        '語系
        Dim Category1Text As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getDefLaguageByDropDownList(Me.UI_cboLanguage_Search, Category1Text)
        oCommon.getDefLaguageByDropDownList(Me.UI_cboLanguage, Category1Text)

        Me.ViewState("_FARC_DFLNO") = Me.UI_cboLanguage_Search.SelectedValue
        Me.ViewState("_FARC_NO") = "-1"

        Call UI_cboLanguage_Search_SelectedIndexChanged(Me.UI_cboLanguage_Search, System.EventArgs.Empty)

        Me.rfvFarNo.ErrorMessage = _oLanguage.getText("Failure", "018", ctlLanguage.eumType.Validator)
        Me.rfvFarcName.ErrorMessage = _oLanguage.getText("Failure", "019", ctlLanguage.eumType.Validator)

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("Failure", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblCategory.Text = _oLanguage.getText("Failure", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblFailureTittle.Text = _oLanguage.getText("Failure", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblLanguage_Search.Text = _oLanguage.getText("Failure", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblLanguage.Text = _oLanguage.getText("Failure", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblFarcNo.Text = _oLanguage.getText("Failure", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblFarcName.Text = _oLanguage.getText("Failure", "005", ctlLanguage.eumType.Tag)

        Me.UI_cmdAddClass.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)

        Me.UI_FailureReason.Columns(1).HeaderText = _oLanguage.getText("Failure", "024", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(2).HeaderText = _oLanguage.getText("Failure", "004", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(3).HeaderText = _oLanguage.getText("Failure", "005", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(4).HeaderText = _oLanguage.getText("Failure", "015", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(5).HeaderText = _oLanguage.getText("Failure", "013", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(6).HeaderText = _oLanguage.getText("Failure", "014", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(7).HeaderText = _oLanguage.getText("Failure", "027", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(8).HeaderText = _oLanguage.getText("Failure", "028", ctlLanguage.eumType.Tag)
        Me.UI_FailureReason.Columns(9).HeaderText = _oLanguage.getText("Failure", "029", ctlLanguage.eumType.Tag)

    End Sub

    ''' <summary>
    ''' 查詢語系 Changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboLanguage_Search_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Me.UI_cboLanguage_Search.SelectedValue, Me.UI_cboFarcNameSearch, sFarcNameText)

    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        '        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable

        Dim FARC_DFLNO As String = Me.ViewState("_FARC_DFLNO")
        Dim FARC_NO As String = Me.ViewState("_FARC_NO")
        dtFailure = oFailure.QueryFailureClass(FARC_DFLNO, FARC_NO)

        Call ArrangementData(dtFailure)
        Session("_dtFailureClass") = dtFailure
        Dim dvFailure As DataView = dtFailure.DefaultView
        dvFailure.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call FailureReasons_DataBind(dvFailure, iPageIndex)
    End Sub

    Private Sub FailureReasons_DataBind(ByVal dvFailure As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_FailureReason.PageSize = _PageSize
        Me.UI_FailureReason.PageIndex = iPageIndex
        Me.UI_FailureReason.DataSource = dvFailure
        Me.UI_FailureReason.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtFailure As FailureDTO.FailureReasonsClassDataTable)
        Dim i As Integer = 0

        Dim oLanguage As New ctlLanguage
        Dim dtLanguage As New LanguageDTO.DEFLANGUAGEDataTable
        dtLanguage = oLanguage.QueryByDefLanguage()
        Dim dvLanguage As DataView = dtLanguage.DefaultView()

        If dtFailure.Columns("SeqID") Is Nothing Then
            dtFailure.Columns.Add("SeqID")
            dtFailure.Columns.Add("Status")
            dtFailure.Columns.Add("LvName")
        End If

        For i = 0 To dtFailure.Rows.Count - 1
            dtFailure.Rows(i)("SeqID") = i + 1

            dvLanguage.RowFilter = "DFL_NO='" & dtFailure.Rows(i)("FARC_DFLNO").ToString().Trim() & "'"
            If dvLanguage.Count > 0 Then
                dtFailure.Rows(i)("LvName") = dvLanguage(0)("DFL_NAME").ToString().Trim()
            End If

            If dtFailure.Rows(i).Item("FARC_VISIBLE").ToString().Trim() = "1" Then
                dtFailure.Rows(i)("Status") = _oLanguage.getText("Failure", "016", ctlLanguage.eumType.Tag)
            Else
                dtFailure.Rows(i)("Status") = _oLanguage.getText("Failure", "017", ctlLanguage.eumType.Tag)
            End If
        Next
    End Sub

    Protected Sub UI_FailureReason_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_FailureReason.RowCommand

        If e.CommandName <> "cmdEdit" And e.CommandName <> "cmdSave" And e.CommandName <> "cmdDetail" And e.CommandName <> "cmdDel" Then
            Exit Sub
        End If

        Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
        Dim row As GridViewRow

        Dim UI_FARC_DFLNO As New Label
        Dim UI_FARC_NO As New Label
        Dim UI_FARC_VISIBLE As New Label
        Dim UI_lblFarcName As New Label
        Dim UI_txtFarcName As New TextBox
        Dim UI_lblFarcStatus As New Label
        Dim UI_Status As New DropDownList
        Dim UI_cmdEdit As New Button
        Dim UI_cmdSave As New Button
        Dim UI_cmdDetail As New Button
        Dim UI_cmdDel As New Button


        If e.CommandName = "cmdEdit" Or e.CommandName = "cmdSave" Or e.CommandName = "cmdDetail" Or e.CommandName = "cmdDel" Then
            row = Me.UI_FailureReason.Rows(iIndex)

            UI_FARC_DFLNO = row.FindControl("UI_FARC_DFLNO")
            UI_FARC_NO = row.FindControl("UI_FARC_NO")
            UI_FARC_VISIBLE = row.FindControl("UI_FARC_VISIBLE")
            UI_lblFarcName = row.FindControl("UI_lblFarcName")
            UI_txtFarcName = row.FindControl("UI_txtFarcName")
            UI_lblFarcStatus = row.FindControl("UI_lblFarcStatus")
            UI_Status = row.FindControl("UI_Status")
            UI_cmdEdit = row.FindControl("UI_cmdEdit")
            UI_cmdSave = row.FindControl("UI_cmdSave")
            UI_cmdDetail = row.FindControl("UI_cmdDetail")
            UI_cmdDel = row.FindControl("UI_cmdDel")
        End If

        Select Case e.CommandName
            Case "cmdEdit"
                UI_lblFarcName.Visible = False
                UI_lblFarcStatus.Visible = False
                UI_cmdEdit.Visible = False
                UI_cmdDetail.Enabled = False
                UI_cmdDel.Enabled = False

                UI_txtFarcName.Visible = True
                UI_Status.Visible = True
                UI_cmdSave.Visible = True

                UI_Status.SelectedValue = UI_FARC_VISIBLE.Text.ToString().Trim()

            Case "cmdSave"
                UI_lblFarcName.Visible = True
                UI_lblFarcStatus.Visible = True
                UI_cmdEdit.Visible = True
                UI_cmdDetail.Enabled = True
                UI_cmdDel.Enabled = True

                UI_txtFarcName.Visible = False
                UI_Status.Visible = False
                UI_cmdSave.Visible = False

                Call Save(UI_FARC_DFLNO.Text.Trim(), UI_FARC_NO.Text.Trim(), UI_txtFarcName.Text.Trim(), UI_Status.SelectedValue.Trim())

            Case "cmdDetail"
                UI_lblFarcName.Visible = True
                UI_lblFarcStatus.Visible = True
                UI_cmdEdit.Visible = True

                UI_txtFarcName.Visible = False
                UI_Status.Visible = False
                UI_cmdSave.Visible = False

                Me.UI_lblPreviousPage_FARCNO.Text = UI_FARC_NO.Text.ToString().Trim()
                Me.UI_lblPreviousPage_DFLNO.Text = UI_FARC_DFLNO.Text.Trim()
            Case "cmdDel"

                Call Del(UI_FARC_DFLNO.Text.Trim(), UI_FARC_NO.Text.Trim())
        End Select


    End Sub

    Protected Sub UI_FailureReason_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_FailureReason.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_FailureReason.PageIndex * Me.UI_FailureReason.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_txtFarcName As TextBox = e.Row.FindControl("UI_txtFarcName")
            Dim rfv_FarcName As RequiredFieldValidator = e.Row.FindControl("rfv_FarcName")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            Dim UI_cmdSave As Button = e.Row.FindControl("UI_cmdSave")
            Dim UI_cmdDetail As Button = e.Row.FindControl("UI_cmdDetail")
            Dim UI_cmdDel As Button = e.Row.FindControl("UI_cmdDel")
            Dim UI_Status As DropDownList = e.Row.FindControl("UI_Status")

            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
            UI_cmdDetail.Text = _oLanguage.getText("Common", "049", ctlLanguage.eumType.Command)
            UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
            UI_Status.Items(0).Text = _oLanguage.getText("Failure", "016", ctlLanguage.eumType.Tag)
            UI_Status.Items(1).Text = _oLanguage.getText("Failure", "017", ctlLanguage.eumType.Tag)

            rfv_FarcName.ControlToValidate = UI_txtFarcName.ID
            rfv_FarcName.ErrorMessage = _oLanguage.getText("Failure", "019", ctlLanguage.eumType.Validator)
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

    Protected Sub UI_FailureReason_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_FailureReason.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtFailureClass") Is Nothing Then
            Dim dtFailure As FailureDTO.FailureReasonsClassDataTable = Session("_dtFailureClass")
            Dim dvFailure As DataView = dtFailure.DefaultView
            Call FailureReasons_DataBind(dvFailure, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_FailureReason_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_FailureReason.Sorting

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

        If IsNothing(Session("_dtFailureClass")) = False Then
            Dim dtFailure As DataTable = Session("_dtFailureClass")
            Dim dvFailure As DataView = dtFailure.DefaultView
            dvFailure.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call FailureReasons_DataBind(dvFailure, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_FailureReason.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_FailureReason.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_FailureReason.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_FailureReason.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_FailureReason.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_FailureReason.Columns(i).HeaderText = sHeaderText
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
        Me.ViewState("_FARC_DFLNO") = Me.UI_cboLanguage_Search.SelectedValue
        Me.ViewState("_FARC_NO") = Me.UI_cboFarcNameSearch.SelectedValue

        Call QueryData(0)
    End Sub

    ''' <summary>
    ''' 新增類別
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdAddClass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdAddClass.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
        Dim dr As FailureDTO.FailureReasonsClassRow = dtFailure.NewFailureReasonsClassRow

        Try
            dr.FARC_DFLNO = Me.UI_cboLanguage.SelectedValue.ToString().Trim()
            dr.FARC_NO = Me.UI_txtFarcNo.Text.ToString().Trim()
            dr.FARC_NAME = Me.UI_txtFarcName.Text.ToString().Trim()
            dr.FARC_VISIBLE = 1
            dr.FARC_AD = Session("_UserID")
            dr.FARC_ADNAME = Session("_UserName")
            dr.FARC_CSTMP = Date.Now
            dr.FARC_LUAD = Session("_UserID")
            dr.FARC_LUADNAME = Session("_UserName")
            dr.FARC_LUSTMP = Date.Now

            dtFailure.AddFailureReasonsClassRow(dr)
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

                Me.UI_cboLanguage_Search.SelectedValue = Me.UI_cboLanguage.SelectedValue
                Call UI_cboLanguage_Search_SelectedIndexChanged(Me.UI_cboLanguage_Search, System.EventArgs.Empty)
                Me.ViewState("_FARC_DFLNO") = Me.UI_cboLanguage_Search.SelectedValue
                Me.ViewState("_FARC_NO") = "-1"

                Call QueryData(0)

                Me.UI_txtFarcNo.Text = ""
                Me.UI_txtFarcName.Text = ""
            End If
        End Try

    End Sub

    Private Sub Save(ByVal DFLNO As String, ByVal FarcNo As String, ByVal FarcName As String, ByVal Status As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
        Dim dr As FailureDTO.FailureReasonsClassRow = dtFailure.NewFailureReasonsClassRow

        Try
            dr.FARC_DFLNO = DFLNO.Trim()
            dr.FARC_NO = FarcNo.Trim()
            dr.FARC_NAME = FarcName.Trim()
            dr.FARC_VISIBLE = Convert.ToInt32(Status.Trim())
            dr.FARC_AD = Session("_UserID")
            dr.FARC_ADNAME = Session("_UserName")
            dr.FARC_CSTMP = Date.Now
            dr.FARC_LUAD = Session("_UserID")
            dr.FARC_LUADNAME = Session("_UserName")
            dr.FARC_LUSTMP = Date.Now

            dtFailure.AddFailureReasonsClassRow(dr)
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

                Call QueryData(Me.UI_FailureReason.PageIndex)
            End If
        End Try

    End Sub

    Private Sub Del(ByVal DFLNO As String, ByVal FarcNo As String)
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim oFailure As New ctlFailure.FailureReasonsClass

        Try
            oFailure.SaveDel(DFLNO, FarcNo)

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

Imports DataService
Imports DefLanguage

Partial Class Warranty_GroupSetting_add
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
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_GroupNo As Label = oContentPlaceHolder.FindControl("lblPreviousPage_GroupNo")
                Me.UI_lblPreviousPage_GroupNo.Text = UI_lblPreviousPage_GroupNo.Text.ToString().Trim()

                UI_txtGroupNo.Text = ""
                UI_txtGroupName.Text = ""

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.UI_lblPreviousPage_GroupNo.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call QueryGroup()
                Call QueryWarrPartsType(0)
            End If
        End If
    End Sub
#End Region

    Private Sub QueryGroup()
        Dim oWarranty As New ctlWarranty
        Dim dtGroup As New WarrantyDTO.WarrGroupTypeDataTable

        Dim sGroupNoTmp As String = Me.UI_lblPreviousPage_GroupNo.Text
        If sGroupNoTmp.Trim().Equals("") Then
            sGroupNoTmp = "$"
        End If
        dtGroup = oWarranty.QueryWarrGroup(sGroupNoTmp, "")
        If dtGroup.Rows.Count > 0 Then
            Dim dr As WarrantyDTO.WarrGroupTypeRow = dtGroup.Rows(0)

            Me.UI_txtGroupNo.Text = dr.GROUP_NO.ToString().Trim()
            Me.UI_txtGroupName.Text = dr.GROUP_NAME.ToString().Trim()
        End If
    End Sub

    ''' <summary>
    ''' RepairCenter - 儲存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim oWarranty As New ctlWarranty

        Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
            Case eumCommand.AddNew
                blnFlag = oWarranty.chkWarrGroupIsExist(Me.UI_txtGroupNo.Text.Trim(), "")

            Case eumCommand.UPDATE
                blnFlag = oWarranty.chkWarrGroupIsExist(Me.UI_txtGroupNo.Text.Trim(), Me.UI_lblPreviousPage_GroupNo.Text.Trim())
        End Select

        If blnFlag = True Then
            sMessage = "Group No Error"
            Me.ucMessage.showMessageByFailed(sMessage)

        Else
            Call Save()
        End If

    End Sub

    Private Sub Save()
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        Dim oWarranty As New ctlWarranty
        Dim dtGroup As New WarrantyDTO.WarrGroupTypeDataTable

        Try
            If UI_txtGroupNo.Text.Trim().Equals("") Then
                Throw New Exception("Please key in Group No. & Group Name")
            End If

            Dim dtWarrGroupParts As New WarrantyDTO.WarrGroupPartsDataTable
            Dim i As Integer = 0
            For i = 0 To UI_GroupParts.Rows.Count - 1
                If UI_GroupParts.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim chkType As CheckBox = UI_GroupParts.Rows(i).FindControl("chkType")
                    Dim UI_TypeNo As Label = UI_GroupParts.Rows(i).FindControl("UI_TypeNo")

                    If chkType.Checked Then
                        Dim dr1 As WarrantyDTO.WarrGroupPartsRow = dtWarrGroupParts.NewRow
                        dr1.GRPT_GNO = Me.UI_txtGroupNo.Text.Trim()
                        dr1.GRPT_TNO = UI_TypeNo.Text.Trim()
                        dr1.GRPT_AD = Session("_UserID")
                        dr1.GRPT_ADNAME = Session("_UserName")
                        dr1.GRPT_CSTMP = Date.Now
                        dr1.GRPT_LUAD = Session("_UserID")
                        dr1.GRPT_LUADNAME = Session("_UserName")
                        dr1.GRPT_LUSTMP = Date.Now
                        dtWarrGroupParts.AddWarrGroupPartsRow(dr1)
                    End If
                    'dtRow.
                End If

            Next


            Dim dr As WarrantyDTO.WarrGroupTypeRow = dtGroup.NewRow

            dr.GROUP_NO = Me.UI_txtGroupNo.Text.Trim()
            dr.GROUP_NAME = Me.UI_txtGroupName.Text.Trim()

            dr.GROUP_AD = Session("_UserID")
            dr.GROUP_ADNAME = Session("_UserName")
            dr.GROUP_CSTMP = Date.Now
            dr.GROUP_LUAD = Session("_UserID")
            dr.GROUP_LUADNAME = Session("_UserName")
            dr.GROUP_LUSTMP = Date.Now

            dtGroup.AddWarrGroupTypeRow(dr)

            Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                Case eumCommand.AddNew
                    oWarranty.SaveWarrGroupAdd(dtGroup)
                    oWarranty.SaveWarrGroupPartsAdd(dtWarrGroupParts, Me.UI_txtGroupNo.Text.Trim())
                Case eumCommand.UPDATE
                    oWarranty.SaveWarrGroupEdit(dtGroup, Me.UI_lblPreviousPage_GroupNo.Text)
                    oWarranty.SaveWarrGroupPartsAdd(dtWarrGroupParts, Me.UI_txtGroupNo.Text.Trim())
            End Select
            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else

                Dim sMsg As String = ""
                Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                    Case eumCommand.AddNew
                        sMsg = oCommon.getMessage(Common.enmMessage.AddOK)

                    Case eumCommand.UPDATE
                        sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                End Select
                Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_GroupSetting.aspx")

                Me.ViewState("_eumCommand") = eumCommand.UPDATE
                Me.UI_lblPreviousPage_GroupNo.Text = Me.UI_txtGroupNo.Text.Trim()
            End If
        End Try
    End Sub

    Private Sub QueryWarrPartsType(ByVal iPageIndex As Integer)
        Dim oWarranty As New ctlWarranty
        Dim dtWarrPartsType As New WarrantyDTO.WarrPartsTypeDataTable

        dtWarrPartsType = oWarranty.QueryWarrPartsType(Session("_LanguageID").ToString(), "", "")
        Call WarrPartsType_DataBind(dtWarrPartsType, iPageIndex)
    End Sub

    Private Sub WarrPartsType_DataBind(ByVal dtWarrPartsType As WarrantyDTO.WarrPartsTypeDataTable, ByVal iPageIndex As Integer)
        Call ArrangementData(dtWarrPartsType)

        Session("_dtWarrPartsType") = dtWarrPartsType

        If Me.ViewState("_eumCommandPart") = eumCommand.UPDATE Then
            Me.UI_GroupParts.AllowPaging = True
            Me.UI_GroupParts.PageSize = _PageSize
            Me.UI_GroupParts.PageIndex = iPageIndex
        Else
            Me.UI_GroupParts.AllowPaging = False
        End If

        Me.UI_GroupParts.DataSource = dtWarrPartsType.DefaultView
        Me.UI_GroupParts.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtWarrPartsType As WarrantyDTO.WarrPartsTypeDataTable)
        Dim i As Integer = 0

        If dtWarrPartsType.Columns("SeqID") Is Nothing Then
            dtWarrPartsType.Columns.Add("SeqID")
        End If

        If dtWarrPartsType.Columns("Checked") Is Nothing Then
            dtWarrPartsType.Columns.Add("Checked")
        End If

        Dim oWarranty As New ctlWarranty
        Dim dtWarrGroupParts As New WarrantyDTO.WarrGroupPartsDataTable

        Dim sGroupTemp As String = UI_lblPreviousPage_GroupNo.Text.Trim
        dtWarrGroupParts = oWarranty.QueryWarrGroupParts(sGroupTemp, "")
        For i = 0 To dtWarrPartsType.Rows.Count - 1
            dtWarrPartsType.Rows(i)("SeqID") = i + 1
            Dim sTypeNo As String = dtWarrPartsType.Rows(i)("TYPE_NO").ToString().Trim()

            dtWarrGroupParts.DefaultView.RowFilter = "GRPT_TNO=" + "'" + sTypeNo + "'"
            If dtWarrGroupParts.DefaultView.Count > 0 Then
                dtWarrPartsType.Rows(i)("Checked") = "Y"
            Else
                dtWarrPartsType.Rows(i)("Checked") = "N"
            End If
        Next
    End Sub

    Protected Sub UI_GroupParts_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_GroupParts.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkType As CheckBox = e.Row.FindControl("chkType")
            Dim UI_TypeNo As Label = e.Row.FindControl("UI_TypeNo")
            Dim UI_Check As Label = e.Row.FindControl("UI_Check")
            If UI_Check.Text.Trim().Equals("Y") Then
                chkType.Checked = True
            Else
                chkType.Checked = False
            End If
        End If
    End Sub
End Class

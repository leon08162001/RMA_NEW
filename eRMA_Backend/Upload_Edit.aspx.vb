Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class Upload_Edit
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_FilePath As String = ConfigurationSettings.AppSettings("Repair_FilePath")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")
    Public _PageSize As String = ConfigurationSettings.AppSettings("PageSize")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            Call setControls()

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMAID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMAID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")

                Me.UI_lblPreviousPage_RMAID.Text = UI_lblPreviousPage_RMAID.Text.ToString().Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.ToString().Trim()

                Call RepairHead()
                Call RepairDetail(0)
            End If
        End If
    End Sub
#End Region

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "114", ctlLanguage.eumType.Tag)
        Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicantID.Text = _oLanguage.getText("RMA", "045", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        'Me.UI_lblProductTittle.Text = _oLanguage.getText("RMA", "101", ctlLanguage.eumType.Tag)
        Me.UI_lblRMANo.Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)

        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "038", ctlLanguage.eumType.Command)

    End Sub

#Region "RepairHead"
    Private Sub RepairHead()
        Dim oRepair As New ctlRMA.Upload
        Dim dtRepairHead As New ImpRmaDTO.dtImpRmaDataTable

        dtRepairHead = oRepair.QueryRmaImport("", Me.UI_lblPreviousPage_RMAID.Text, "", "", "", "", "", "", "")
        If dtRepairHead.Rows.Count > 0 Then
            Dim dr As ImpRmaDTO.dtImpRmaRow = dtRepairHead.Rows(0)
            If dr.IsIRMA_NONull = False Then Me.UI_lblRMANoText.Text = dr.IRMA_NO.ToString().Trim()
            If dr.IsIRMA_CUNONull = False Then Me.UI_lblAccountIDText.Text = dr.IRMA_CUNO.ToString().Trim()
            If dr.Iscu_nameNull = False Then Me.UI_lblAccountNameText.Text = dr.cu_name.ToString().Trim()
            If dr.IsIRMA_ACCOUNTIDNull = False Then Me.UI_lblApplicantIDText.Text = dr.IRMA_ACCOUNTID.ToString().Trim()
            If dr.IsIRMA_APPLICANTNull = False Then Me.UI_lblApplicantText.Text = dr.IRMA_APPLICANT.ToString().Trim()
            If dr.IsIRMA_TELNull = False Then Me.UI_lblTelText.Text = dr.IRMA_TEL.ToString().Trim()
            If dr.IsIRMA_ADDRESSNull = False Then Me.UI_lblAddressText.Text = dr.IRMA_ADDRESS.ToString().Trim()
        End If
    End Sub
#End Region

#Region "RepairDetail"
    Private Sub RepairDetail(ByVal iPageIndex As Integer)
        Dim oRepair As New ctlRMA.Upload
        Dim dtRepairDetail As New ImpRmaDTO.dtImpRmaDetailQryDataTable

        dtRepairDetail = oRepair.QueryRmaDetail(Session("_LanguageID").ToString().Trim(), "", Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim(), "")
        Call ReplaceData(dtRepairDetail)

        Session("_dtUploadRepairDetail") = dtRepairDetail
        Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView

        Call RepairDetail_DataBind(dvRepairDetail, iPageIndex)
    End Sub

    Private Sub RepairDetail_DataBind(ByVal dvRepairDetail As DataView, ByVal iPageIndex As Integer)
        Me.UI_dvRetailDetail.PageSize = _PageSize
        Me.UI_dvRetailDetail.PageIndex = iPageIndex
        Me.UI_dvRetailDetail.DataSource = dvRepairDetail
        Me.UI_dvRetailDetail.DataBind()
    End Sub

    Private Sub ReplaceData(ByVal dtRepairDetail As ImpRmaDTO.dtImpRmaDetailQryDataTable)
        Dim i As Integer = 0

        dtRepairDetail.Columns.Add("Warranty")
        dtRepairDetail.Columns.Add("FarcName")
        For i = 0 To dtRepairDetail.Rows.Count - 1
            Dim dr As ImpRmaDTO.dtImpRmaDetailQryRow = dtRepairDetail.Rows(i)
            Dim sFarcName As String = ""

            'If dr.IsIRMAR_FARCNONull = False And dr.IsRMAR_FARCNAMENull = False Then
            '    sFarcName = dr.RMAR_FARCNAME.ToString().Trim()
            'Else
            '    If dr.IsRMAD_FARFARCNONull = False And dr.IsFARC_NAMENull = False Then
            '        sFarcName = dr.FARC_NAME.ToString().Trim()
            '    End If
            'End If
            dtRepairDetail.Rows(i).Item("FarcName") = sFarcName.ToString().Trim()

            '保固日期
            If dr.IsIRMAD_WARRANTYNull = False Then
                dtRepairDetail.Rows(i).Item("Warranty") = Convert.ToDateTime(dr.IRMAD_WARRANTY.ToString().Trim()).ToShortDateString()

            Else
                dtRepairDetail.Rows(i).Item("Warranty") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                Select Case dtRepairDetail.Rows(i)("IRMAD_ISWARRANTY").ToString().Trim()
                    Case "0"
                        dtRepairDetail.Rows(i)("Warranty") = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
                    Case "1"
                        dtRepairDetail.Rows(i)("Warranty") = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
                End Select
            End If

        Next
    End Sub

    Protected Sub UI_dvRetailDetail_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_dvRetailDetail.PageIndexChanged

    End Sub

    Protected Sub UI_dvRetailDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvRetailDetail.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_dvRetailDetail.Rows(iIndex)
            Dim RMAD_ID As Label = row.FindControl("UI_RMADID")
            Dim RMA_NO As String = Me.UI_lblPreviousPage_RMANO.Text.ToString().Trim()
            Me.ucRepairDetail.show(RMAD_ID.Text.ToString().Trim(), Me.UI_lblAccountIDText.Text.Trim(), True)
        End If

        If e.CommandName = "cmdEditSn" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_dvRetailDetail.Rows(iIndex)
            Dim RMAD_ID As Label = row.FindControl("UI_RMADID")
            Me.UI_lblPreviousPage_RMADID.Text = RMAD_ID.Text.Trim()
        End If

        If e.CommandName = "cmdDelete" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow
            row = Me.UI_dvRetailDetail.Rows(iIndex)
            Dim UI_RMADID As Label = row.FindControl("UI_RMADID")

            Call Delete(UI_RMADID.Text.ToString().Trim())
            Call RepairDetail(0)
        End If

    End Sub

    ''' <summary>
    ''' 刪除 RMA Detail 單
    ''' </summary>
    ''' <param name="sRMADID"></param>
    ''' <remarks></remarks>
    Private Sub Delete(ByVal sRMADID As String)
        Dim oReceived As New ctlRMA.Upload
        Dim dtReceiveList As New ImpRmaDTO.dtImpRmaDataTable

        oReceived.DeleteRMAD_FromIMP_RMADetail(sRMADID, Session("_UserID").ToString(), Session("_UserName").ToString())


    End Sub
    Protected Sub UI_dvRetailDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvRetailDetail.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("Upload", "004", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("Upload", "016", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("Upload", "017", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("Upload", "018", ctlLanguage.eumType.Tag)
            'e.Row.Cells(5).Text = _oLanguage.getText("Upload", "018", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("Upload", "019", ctlLanguage.eumType.Tag)
            e.Row.Cells(6).Text = _oLanguage.getText("Upload", "020", ctlLanguage.eumType.Tag)
            e.Row.Cells(7).Text = _oLanguage.getText("Upload", "021", ctlLanguage.eumType.Tag)
            e.Row.Cells(8).Text = _oLanguage.getText("Upload", "012", ctlLanguage.eumType.Tag)
            e.Row.Cells(9).Text = _oLanguage.getText("Upload", "022", ctlLanguage.eumType.Tag)
            e.Row.Cells(10).Text = _oLanguage.getText("Upload", "023", ctlLanguage.eumType.Tag)
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_RMADCSTMP As Label = e.Row.FindControl("UI_RMADCSTMP")
            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)

            '若申請日期超過保固日期,保固日期以紅色表示
            If IsDate(e.Row.Cells(4).Text.Trim()) = True Then
                If Convert.ToDateTime(e.Row.Cells(4).Text) < Convert.ToDateTime(UI_RMADCSTMP.Text.Trim()) Then
                    e.Row.Cells(4).ForeColor = Drawing.Color.Red
                End If
            End If

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

    Protected Sub UI_dvRetailDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvRetailDetail.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtUploadRepairDetail") Is Nothing Then
            Dim dtRepairDetail As ImpRmaDTO.dtImpRmaDetailQryDataTable = Session("_dtUploadRepairDetail")
            Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView
            Call RepairDetail_DataBind(dvRepairDetail, iPageIndex)

        Else
            Call RepairDetail(iPageIndex)
        End If
    End Sub

#End Region
End Class

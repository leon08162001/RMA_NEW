Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucSpecialSetting
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim sUploadPath As String = ConfigurationSettings.AppSettings("EmailAttachFolder")
    Public sUploadUrl As String = ConfigurationSettings.AppSettings("UploadUrl")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub
    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Call oCommon.getCostCenterByDropDownList(False, Me.UI_cboOperationCenter, "")
        Call oCommon.getWarrantyTypeByDropDownList(False, Me.UI_cboWarrantyType, "")
        Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "035", ctlLanguage.eumType.Tag)
        Me.UI_lblOperationCenter.Text = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyType.Text = _oLanguage.getText("Warranty", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblExtraMonths.Text = _oLanguage.getText("Warranty", "031", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyName.Text = _oLanguage.getText("Warranty", "029", ctlLanguage.eumType.Tag)
        Me.UI_lblYears.Text = _oLanguage.getText("Warranty", "032", ctlLanguage.eumType.Tag)
        Me.UI_lblVersion.Text = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("Warranty", "036", ctlLanguage.eumType.Tag)
        Me.UI_lblUnitPrice.Text = _oLanguage.getText("Warranty", "037", ctlLanguage.eumType.Tag)

        lblPartsInform.Text = _oLanguage.getText("Warranty", "043", ctlLanguage.eumType.Tag)
        lblPartAllPart.Text = _oLanguage.getText("Warranty", "047", ctlLanguage.eumType.Tag)
        lblPartAllDesc.Text = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        lblPartOKPart.Text = _oLanguage.getText("Warranty", "047", ctlLanguage.eumType.Tag)
        lblPartOKDesc.Text = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        lblPartOKYears.Text = _oLanguage.getText("Warranty", "032", ctlLanguage.eumType.Tag)
        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)

        pnlFile.Visible = False
        UI_txtVersion.Enabled = False
    End Sub

    Private Sub QueryData(ByVal sRMADID As String)
        Dim oWarranty As New ctlWarranty
        Dim dtSwSet As New WarrantyDTO.SWSETDataTable

        Dim oExport As New ctlRMA.Export
        Dim sSwID As String = oExport.getSpecialWarrantyID(sRMADID)


        dtSwSet = oWarranty.QuerySWSet(sSwID, "", "", -1, "", "")
        Dim i As Integer = 0
        If dtSwSet.Count > 0 Then
            Dim dr As WarrantyDTO.SWSETRow = dtSwSet.Rows(0)

            UI_cboOperationCenter.SelectedValue = dr.SW_COMPNO.ToString().Trim()
            UI_cboWarrantyType.SelectedValue = dr.SW_TYPE01.ToString().Trim()
            UI_cboWarrantyTypeoth.SelectedValue = dr.SW_TYPE02.ToString().Trim()

            If dr.SW_TYPE01.ToString() = "1" Then
                UI_cboWarrantyTypeoth.Visible = True
                UI_lblDescription.Visible = True
                UI_txtDescription.Visible = True
                pnDesc.Visible = False
            Else
                UI_cboWarrantyTypeoth.Visible = False
                UI_lblDescription.Visible = False
                UI_txtDescription.Visible = False
                pnDesc.Visible = True
            End If

            UI_ExtraMonths.Text = dr.SW_EXTMM.ToString().Trim()
            UI_txtProductGroup.Text = dr.SW_GROUP.ToString().Trim()
            UI_txtWarrantyName.Text = dr.SW_NAME.ToString().Trim()
            UI_Years.Text = dr.SW_STDYY.ToString().Trim()
            UI_txtVersion.Text = dr.SW_VERSION.ToString("000").Trim()
            If Not dr.IsSW_DESCNull Then
                UI_txtDescription.Text = dr.SW_DESC.ToString().Trim()
                txtDescription.Text = dr.SW_DESC.ToString().Trim()
            End If
            If Not dr.IsSW_ExpdateNull Then
                txtExpDate.Text = dr.SW_Expdate.ToString("yyyy/MM/dd")
            End If
            UI_UnitPrice.Text = dr.SW_PRICE.ToString().Trim()

            UI_cboOperationCenter.Enabled = False
            UI_txtProductGroup.Enabled = False
            UI_txtVersion.Enabled = False
            UI_cboWarrantyType.Enabled = False
            UI_cboWarrantyTypeoth.Enabled = False
            UI_Years.Enabled = False
            UI_ExtraMonths.Enabled = False

            Dim dtSWFile As New WarrantyDTO.SWFILEDataTable
            dtSWFile = oWarranty.QuerySWFile(sSwID, "", "")
            lstFile.DataSource = dtSWFile
            lstFile.DataBind()


            If UI_cboWarrantyType.SelectedValue <> 1 Then
                pnParts.Visible = False
            Else
                pnParts.Visible = True
            End If

        End If

        Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
        pnlFile.Visible = True
        dtSWParts = oWarranty.QuerySWParts(sSwID, "", 1, "")
        lstOKPartNo.DataSource = dtSWParts
        lstOKPartNo.DataBind()

        dtSWParts = oWarranty.QuerySWParts(sSwID, "", 0, "")
        lstAllPartNo.DataSource = dtSWParts
        lstAllPartNo.DataBind()
    End Sub
    Public Sub show(ByVal RMADID As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Me.ViewState("_sRMADID") = RMADID

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Call QueryData(RMADID)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub
End Class

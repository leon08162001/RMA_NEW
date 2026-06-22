Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucImportRepairDetail
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto


    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Requested_FilePath As String = ConfigurationSettings.AppSettings("Requested_FilePath")
    Dim _Requested_VisualPath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_isModified") = False
            Me.ViewState("_show") = False
            Me.ViewState("_key") = ""
            Me.ViewState("_CUNO") = ""

            Call setControls()
        End If
    End Sub



    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsClassByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass, sFarcNameText)

        '取得Tag Text
        Me.UI_lblProblemTittle.Text = _oLanguage.getText("RMA", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)


        Me.UI_lblFailure.Text = _oLanguage.getText("RMA", "023", ctlLanguage.eumType.Tag)

        Me.rfv_ProductDesc.ErrorMessage = _oLanguage.getText("RMA", "198", ctlLanguage.eumType.Validator)
        Me.rfv_ProblemDesc.ErrorMessage = _oLanguage.getText("RMA", "199", ctlLanguage.eumType.Validator)
        Me.cv_FailureClass.ErrorMessage = _oLanguage.getText("RMA", "078", ctlLanguage.eumType.Validator)
        Me.cv_Failure.ErrorMessage = _oLanguage.getText("RMA", "079", ctlLanguage.eumType.Validator)

        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdConfirmed.Text = _oLanguage.getText("Common", "015", ctlLanguage.eumType.Command)

        Me.UI_WEBURL.Text = _WEBURL
        Me.UI_VisualPath.Text = _Requested_VisualPath

    End Sub

    ''' <summary>
    ''' 不良原因代碼(下拉式)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cboFailureClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cboFailureClass.SelectedIndexChanged
        Dim sFarcNameText As String = _oLanguage.getText("Failure", "007", ctlLanguage.eumType.Tag)
        oCommon.getFailureReasonsByDropDownList(Session("_LanguageID"), Me.UI_cboFailureClass.SelectedValue, Me.UI_cboFailure, sFarcNameText)

        Call show("", "", True)
    End Sub
    ''' <summary>
    ''' QueryData
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData()
        Dim i As Integer = 0
        Dim dtRMADetail As ImpRmaDTO.dtImpRmaDetailQryDataTable = Session("_dtUploadRepairDetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        dvRMADetail.RowFilter = "IRMAD_ID='" & Me.ViewState("_key").ToString().Trim() & "'"

        If dvRMADetail.Count > 0 Then
            Me.UI_txtSerial.Text = dvRMADetail.Item(0)("IRMAD_SERIALNO").ToString().Trim()
            Me.UI_txtDescription.Text = dvRMADetail.Item(0)("IRMAD_PROBLEMDESC").ToString().Trim()
            If dvRMADetail.Item(0)("IRMAD_FARFARCNO").ToString().Trim() <> "" Then
                Me.UI_cboFailureClass.SelectedValue = dvRMADetail.Item(0)("IRMAD_FARFARCNO").ToString().Trim()
                Call UI_cboFailureClass_SelectedIndexChanged(Me.UI_cboFailure, System.EventArgs.Empty)
                If dvRMADetail.Item(0)("IRMAD_FARNO").ToString().Trim() <> "" Then
                    Me.UI_cboFailure.SelectedValue = dvRMADetail.Item(0)("IRMAD_FARNO").ToString().Trim()
                End If
            Else
                Me.UI_cboFailureClass.SelectedValue = "-1"
                Call UI_cboFailureClass_SelectedIndexChanged(Me.UI_cboFailure, System.EventArgs.Empty)
            End If
        End If

        dvRMADetail.RowFilter = ""
    End Sub
    ''' <summary>
    ''' 修改Confirmed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdConfirmed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdConfirmed.Click
        Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)

        Dim oExport As New ctlRMA.Export
        Dim oRequested As New ctlRMA.Upload
        Dim dtRMADetail As ImpRmaDTO.dtImpRmaDetailQryDataTable = Session("_dtUploadRepairDetail")
        Dim dvRMADetail As DataView = dtRMADetail.DefaultView

        dvRMADetail.RowFilter = "IRMAD_ID='" & Me.ViewState("_key").ToString().Trim() & "'"
        If dvRMADetail.Count > 0 Then
            Dim dr As ImpRmaDTO.dtImpRmaDetailQryRow = dvRMADetail(0).Row()

            dr.IRMAD_SERIALNO = Me.UI_txtSerial.Text.ToString().Trim()
            dr.IRMAD_FARFARCNO = Me.UI_cboFailureClass.SelectedValue.ToString().Trim()
            dr.farc_name = Me.UI_cboFailureClass.SelectedItem.Text.Trim()
            dr.IRMAD_FARNO = Me.UI_cboFailure.SelectedValue.ToString().Trim()
            dr.far_reason = Me.UI_cboFailure.SelectedItem.Text.Trim()


            If Me.UI_txtSerial.Text.Trim() <> "" Then
                Dim sModelNo As String = oExport.getModelNo(Me.UI_txtSerial.Text.Trim())
                If sModelNo.Trim() = "" Then
                    dr.IRMAD_MODELNO = "OTHER"
                Else
                    dr.IRMAD_MODELNO = sModelNo
                End If

                'Dim sWarrantyDate As String = oExport.getWarranty(Me.UI_txtSerial.Text, Me.ViewState("_CUNO").ToString().Trim())
                Dim sWarrantyDate As String = oExport.getMaxWarranty(Me.UI_txtSerial.Text, Me.ViewState("_CUNO").ToString().Trim(), Session("_RepairID").ToString())
                If sWarrantyDate.Trim() <> "" Then
                    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                    If IsDate(sWarranty) = True Then
                        dr.IRMAD_WARRANTY = Convert.ToDateTime(sWarranty)
                        'dr.SetIRMAD_WARRANTYNull()
                    End If
                End If
            End If


            dr.IRMAD_PROBLEMDESC = Me.UI_txtDescription.Text.ToString().Trim()
            dr.IRMAD_LUAD = Session("_UserID").ToString()
            dr.IRMAD_LUADNAME = Session("_UserName").ToString()
            dr.IRMAD_LUSTMP = Date.Now
            dr.IRMAD_STATUS = 20
            oRequested.Edit_RMADetail_FromIMP_RMA(dr)
        End If

        dvRMADetail.RowFilter = ""
        Session("_dtUploadRepairDetail") = dtRMADetail


        '=========================================================================================================================
        'UI_dvRMADetail DataBind
        '=========================================================================================================================
        dvRMADetail.RowFilter = "IRMAD_MARK=0"
        Dim UI_dvRetailDetail As GridView = Me.Parent.FindControl("UI_dvRetailDetail")
        dtRMADetail = Session("_dtUploadRepairDetail")
        UI_dvRetailDetail.DataSource = dvRMADetail
        UI_dvRetailDetail.DataBind()

        Me.ViewState("_isModified") = False
        Call show("", "", False)
    End Sub

    Public Sub show(ByVal Key As String, ByVal CUNO As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            If Key.Trim <> "" Then
                Me.ViewState("_key") = Key
                Me.ViewState("_CUNO") = CUNO
                Call QueryData()
            End If
            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

    ''' <summary>
    ''' 設定是否要直接修改資料庫內容
    ''' </summary>
    ''' <value></value>
    ''' <returns>回傳 Boolean</returns>
    ''' <remarks>
    ''' 目前有用到的是 收貨人員修改品項時
    ''' </remarks>
    Public Property isModified() As Boolean
        Get
            Return Convert.ToBoolean(Me.ViewState("_isModified"))
        End Get

        Set(ByVal nNewValue As Boolean)
            Me.ViewState("_isModified") = nNewValue
        End Set
    End Property

End Class

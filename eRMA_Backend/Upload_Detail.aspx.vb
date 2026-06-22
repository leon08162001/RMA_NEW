Imports DataService
Imports System.Data
Imports DefLanguage

Partial Class Upload_Detail
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _Repair_FilePath As String = ConfigurationSettings.AppSettings("Repair_FilePath")
    Dim _Repair_VisualPath As String = ConfigurationSettings.AppSettings("Repair_VisualPath")

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtRepairDetail") = Nothing

            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim UI_lblPreviousPage_RMADID As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMADID")
                Dim UI_lblPreviousPage_RMANO As Label = oContentPlaceHolder.FindControl("UI_lblPreviousPage_RMANO")
                Me.UI_lblPreviousPage_RMADID.Text = UI_lblPreviousPage_RMADID.Text.Trim()
                Me.UI_lblPreviousPage_RMANO.Text = UI_lblPreviousPage_RMANO.Text.Trim()

                Call setControls()
                Call QueryDataByHead()
                Call QueryDataByDetail()
            End If
        End If
    End Sub
#End Region

    Private Sub setControls()

        '¨ú±oTag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "111", ctlLanguage.eumType.Tag)
        Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblRepair.Text = _oLanguage.getText("Report", "122", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairDate.Text = _oLanguage.getText("Report", "124", ctlLanguage.eumType.Tag)

        Me.UI_lblInformationTittle.Text = _oLanguage.getText("RMA", "082", ctlLanguage.eumType.Tag)
        Me.UI_cmdBack.Value = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryDataByHead()
        Dim sText As String = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)

        Dim oRepair As New ctlRMA.Upload
        Dim dtRepair As New ImpRmaDTO.IRMARepairDataTable
        Dim dtRmaDetail As New ImpRmaDTO.dtImpRmaDetailQryDataTable

        dtRepair = oRepair.QueryByRmaRepair_FromIMP_RMAREPAIR(Me.UI_lblPreviousPage_RMADID.Text, "")
        If dtRepair.Rows.Count > 0 Then
            Dim dr As ImpRmaDTO.IRMARepairRow = dtRepair.Rows(0)

            If dr.IsIRMAR_REPAIRADNAMENull = False Then Me.UI_txtRepair.Text = dr.IRMAR_REPAIRAD.ToString().Trim()
            If dr.IsIRMAR_REPAIRDATENull = False Then Me.UI_txtRepairDate.Text = Date.Parse(dr.IRMAR_REPAIRDATE.ToString()).ToString("yyy-MM-dd")
        End If

        dtRmaDetail = oRepair.QueryRmaDetail(Session("_LanguageID").ToString().Trim(), Me.UI_lblPreviousPage_RMADID.Text, "", "")
        If dtRmaDetail.Rows.Count > 0 Then
            Dim dr As ImpRmaDTO.dtImpRmaDetailQryRow = dtRmaDetail.Rows(0)
            Me.UI_lblSerialText.Text = dr.IRMAD_SERIALNO.ToString().Trim()
        End If

    End Sub

#Region "QueryDataByDetail"

#End Region

    Private Sub QueryDataByDetail()
        Dim oRepair As New ctlRMA.Upload
        Dim dtRepairDetail As New ImpRmaDTO.IRMARepair_DetailDataTable

        dtRepairDetail = oRepair.QueryByDetail_FromIRMARepair_Detail(Session("_LanguageID").ToString().Trim(), Me.UI_lblPreviousPage_RMADID.Text)
        Dim oRow As DataRow = dtRepairDetail.NewRow()
        oRow("IRMARED_ID") = -1
        oRow("IRMARED_RMADID") = Me.UI_lblPreviousPage_RMADID.Text
        oRow("IRMARED_MARK") = 0
        dtRepairDetail.Rows.Add(oRow)

        Call RepairDetail_DataBind(dtRepairDetail, 0)
    End Sub

    Private Sub RepairDetail_DataBind(ByVal dtRepairDetail As ImpRmaDTO.IRMARepair_DetailDataTable, ByVal iPageIndex As Integer)
        Session("_dtRepairDetail") = dtRepairDetail

        Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
        Me.UI_dvRepairDetail.EditItemIndex = dtRepairDetail.DefaultView.Count - 1
        Me.UI_dvRepairDetail.DataBind()
    End Sub
    Protected Sub UI_dvRepairDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvRepairDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHNewPart As Label = e.Item.FindControl("lblHNewPart")
            Dim lblHNewSerial As Label = e.Item.FindControl("lblHNewSerial")
            Dim lblHPart As Label = e.Item.FindControl("lblHPart")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")

            Dim lblHDescription As Label = e.Item.FindControl("lblHDescription")
            Dim lblHImproperUsage As Label = e.Item.FindControl("lblHImproperUsage")
            Dim lblHDefectReason As Label = e.Item.FindControl("lblHDefectReason")
            Dim lblHEdit As Label = e.Item.FindControl("lblHEdit")
            Dim lblHDelete As Label = e.Item.FindControl("lblHDelete")
            Dim UI_cboImproperUsage As DropDownList = e.Item.FindControl("UI_cboImproperUsage")
            Dim UI_cboDefectReason As DropDownList = e.Item.FindControl("UI_cboDefectReason")

            lblHNewPart.Text = _oLanguage.getText("RMA", "184", ctlLanguage.eumType.Tag)
            lblHNewSerial.Text = _oLanguage.getText("RMA", "185", ctlLanguage.eumType.Tag)
            lblHPart.Text = _oLanguage.getText("RMA", "083", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "098", ctlLanguage.eumType.Tag)
            lblHDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
            lblHImproperUsage.Text = _oLanguage.getText("RMA", "064", ctlLanguage.eumType.Tag)
            lblHDefectReason.Text = _oLanguage.getText("RMA", "102", ctlLanguage.eumType.Tag)
            lblHEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            lblHDelete.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        End If

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim UI_cmdEdit As Button = e.Item.FindControl("UI_cmdEdit")
            Dim UI_cmdDel As Button = e.Item.FindControl("UI_cmdDel")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
            UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        End If
        If e.Item.ItemType = ListItemType.EditItem Then
            Dim UI_cmdSave As Button = e.Item.FindControl("UI_cmdSave")
            'Dim UI_cmdDel As Button = e.Item.FindControl("UI_cmdDel")
            Dim lblIRMARED_IMPROPERUSAGE As Label = e.Item.FindControl("lblIRMARED_IMPROPERUSAGE")
            Dim lblIRMARED_DEFECTIVE As Label = e.Item.FindControl("lblIRMARED_DEFECTIVE")

            Dim UI_cboImproperUsage As DropDownList = e.Item.FindControl("UI_cboImproperUsage")
            Dim UI_cboDefectReason As DropDownList = e.Item.FindControl("UI_cboDefectReason")

            UI_cboImproperUsage.Items.Clear()
            UI_cboImproperUsage.Items.Add(New ListItem("Y", "1"))
            UI_cboImproperUsage.Items.Add(New ListItem("N", "0"))
            UI_cboImproperUsage.SelectedValue = lblIRMARED_IMPROPERUSAGE.Text.Trim()

            oCommon.getDefectiveByDropDownList(Session("_LanguageID"), UI_cboDefectReason)
            UI_cboDefectReason.SelectedValue = lblIRMARED_DEFECTIVE.Text.Trim()

            UI_cmdSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
            'UI_cmdDel.Text = _oLanguage.getText("Common", "007", ctlLanguage.eumType.Command)
        End If

    End Sub

    Protected Sub UI_dvRepairDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles UI_dvRepairDetail.ItemCommand

        Dim iIndex As Integer = e.Item.ItemIndex
        If e.CommandName = "cmdEdit" Then
            Dim dtRepairDetail As New ImpRmaDTO.IRMARepair_DetailDataTable
            dtRepairDetail = Session("_dtRepairDetail")

            Me.UI_dvRepairDetail.DataSource = dtRepairDetail.DefaultView()
            Me.UI_dvRepairDetail.EditItemIndex = iIndex
            Me.UI_dvRepairDetail.DataBind()
        End If

        If e.CommandName = "cmdDel" Then
            Dim lblIRMARED_ID As Label = e.Item.FindControl("lblIRMARED_ID")

            Call Delete(lblIRMARED_ID.Text.ToString().Trim())
            QueryDataByDetail()
        End If

        If e.CommandName = "cmdSave" Then
            Dim oRepair As New ctlRMA.Upload
            Dim dtRepairDetail As New ImpRmaDTO.IRMARepair_DetailDataTable
            Dim lblIRMARED_ID As Label = e.Item.FindControl("lblIRMARED_ID")
            Dim lblNewPart As TextBox = e.Item.FindControl("lblNewPart")
            Dim lblNewSerial As TextBox = e.Item.FindControl("lblNewSerial")
            Dim lblDescription As TextBox = e.Item.FindControl("lblDescription")
            Dim lblPart As TextBox = e.Item.FindControl("lblPart")
            Dim lblSerial As TextBox = e.Item.FindControl("lblSerial")
            Dim UI_cboImproperUsage As DropDownList = e.Item.FindControl("UI_cboImproperUsage")
            Dim UI_cboDefectReason As DropDownList = e.Item.FindControl("UI_cboDefectReason")

            dtRepairDetail = Session("_dtRepairDetail")

            dtRepairDetail.DefaultView.RowFilter = "IRMARED_ID='" + lblIRMARED_ID.Text.Trim() + "'"
            Dim dvRepairDetail As ImpRmaDTO.IRMARepair_DetailRow = dtRepairDetail.DefaultView(0).Row()

            dvRepairDetail.IRMARED_ID = lblIRMARED_ID.Text.Trim()
            dvRepairDetail.IRMARED_NPARTNO = lblNewPart.Text.Trim()
            dvRepairDetail.IRMARED_NSERIALNO = lblNewSerial.Text.Trim()
            dvRepairDetail.IRMARED_OPARTNO = lblPart.Text.Trim()
            dvRepairDetail.IRMARED_OSERIALNO = lblSerial.Text.Trim()
            dvRepairDetail.IRMARED_DESC = lblDescription.Text.Trim()
            dvRepairDetail.IRMARED_IMPROPERUSAGE = UI_cboImproperUsage.SelectedValue()
            dvRepairDetail.IRMARED_DEFECTIVE = UI_cboDefectReason.SelectedValue()
            dvRepairDetail.IRMARED_MARK = 0.0
            dvRepairDetail.IRMARED_LUAD = Session("_UserID").ToString()
            dvRepairDetail.IRMARED_LUADNAME = Session("_UserName").ToString()
            dvRepairDetail.IRMARED_LUSTMP = Date.Now

            If lblIRMARED_ID.Text.Trim() = "-1" Then
                dvRepairDetail.IRMARED_QTY = 1
                dvRepairDetail.IRMARED_MATERIALCOST = 0.0
                dvRepairDetail.IRMARED_PRICE = 0.0
                dvRepairDetail.IRMARED_CURRENCYCODE = "NTD"
                dvRepairDetail.IRMARED_CURRENCYRATE = 0.0
                dvRepairDetail.IRMARED_ASSIGEPRICE = 0.0
                dvRepairDetail.IRMARED_ASSIGECURRENCYCODE = "NTD"
                dvRepairDetail.IRMARED_ASSIGECURRENCYRATE = 0.0

                dvRepairDetail.IRMARED_AD = Session("_UserID").ToString()
                dvRepairDetail.IRMARED_ADNAME = Session("_UserName").ToString()
                dvRepairDetail.IRMARED_CSTMP = Date.Now

                oRepair.InsertRMAREPAIR_DETAIL_FromIMP_RMA(dvRepairDetail, UI_lblPreviousPage_RMANO.Text.Trim())
            Else
                oRepair.EditRMAREPAIR_DETAIL(dvRepairDetail, UI_lblPreviousPage_RMANO.Text.Trim())
            End If

            Call QueryDataByDetail()
        End If

    End Sub

    Private Sub Delete(ByVal sIRMARED_ID As String)
        Dim oRepair As New ctlRMA.Upload
        Dim dtRepairDetail As New ImpRmaDTO.IRMARepair_DetailDataTable

        oRepair.DeleteRMAREPAIR_DETAIL(sIRMARED_ID, Session("_UserID").ToString(), Session("_UserName").ToString())
    End Sub

End Class

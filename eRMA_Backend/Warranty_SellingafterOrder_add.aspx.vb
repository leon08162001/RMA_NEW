
Imports System.Data
Imports System.Data.OracleClient
Imports System.IO
Imports System.Xml
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports iTextSharp.text
Imports iTextSharp.text.pdf    '140428 by cipherlab.fairy

Partial Class Warranty_SellingafterOrder_add
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim sUploadPath As String = ConfigurationManager.AppSettings("EmailAttachFolder")
    Dim _pdfSample As String = ConfigurationManager.AppSettings("Requested_ExcelSample_VisualPath")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions
    Dim _WEBURL As String = ConfigurationManager.AppSettings("WEBURL")

    'fairy
    Dim strAD As String
    Dim strCenter As String

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")
    Dim sFontPath As String = ConfigurationSettings.AppSettings("FontPath")
    Dim _WaterMarkPic As String = ConfigurationSettings.AppSettings("WaterMarkPic")

    Dim _ReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"
    Shared conNumberWordLess20 As String() = {"", "ONE", "TWO", "THREE", "FOUR", "FIVE",
     "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
     "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
     "EIGHTEEN", "NINETEEN"}
    Shared conNumberWordTen As String() = {"", "", "TWENTY", "THIRTY", "FORTY", "FIFTY",
     "SIXTY", "SEVENTY", "EIGHTY", "NINETY"}

    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write(Session("_DeptNO"))

        If Me.IsPostBack = False Then
            If Not Me.PreviousPage Is Nothing Then
                Dim oContentPlaceHolder As ContentPlaceHolder = Me.PreviousPage.Master.FindControl("ContentPlaceHolder")
                Dim lblPreviousPage_SwID As Label = oContentPlaceHolder.FindControl("lblPreviousPage_SwID")
                Dim lblPreviousPage_Compno As Label = oContentPlaceHolder.FindControl("lblPreviousPage_Compno")
                Me.txtWarrantyNo.Text = lblPreviousPage_SwID.Text.ToString().Trim()
                'Me.cboOperationCenter.SelectedValue = lblPreviousPage_Compno.Text.ToString().Trim()
                'txtCurrency.Enabled = False
                cboWarrantyType.Enabled = False
                txtSales.Enabled = False
                txtSalesName.Enabled = False

                txtDiffCnt.Text = "0"

                txtPurDate.Text = DateTime.Now.ToString("yyyy/MM/dd")
                txtFlow.Text = "N"

                Me.ViewState("_eumCommand") = eumCommand.AddNew
                If Me.txtWarrantyNo.Text.Trim <> "" Then
                    Me.ViewState("_eumCommand") = eumCommand.UPDATE
                End If

                Call setControls()

                Call QueryData(lblPreviousPage_Compno.Text.ToString().Trim())

                Dim oWarranty As New ctlWarranty
                Dim dtCwVer As New WarrantyDTO.WARRSETDataTable
                dtCwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), "CW", txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
                dtCwVer.DefaultView.RowFilter = "WAR_STATUS=2"
                cboVersion.DataTextField = "war_name"
                cboVersion.DataValueField = "war_id"
                cboVersion.DataSource = dtCwVer.DefaultView
                cboVersion.DataBind()

                txtWarrantyNo.Enabled = False
                txtFlow.Enabled = False
                txtErpNo.Enabled = False

                oCommon.getWarrsetTypeByDropDownList(Me.rdoType, "", 1, txtCustomer.Text)
            End If
        End If
    End Sub

    Private Sub setControls()
        'Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "035", ctlLanguage.eumType.Tag)
        'Me.UI_lblSubTittle.Text = _oLanguage.getText("Warranty", "038", ctlLanguage.eumType.Tag)
        Call oCommon.getCostCenterByDropDownList(False, Me.cboOperationCenter, "")
        lblWarrantyNo.Text = "Warranty No"
        lblOperationCenter.Text = "Operation Center"
        lblSales.Text = "Sales"
        lblCustomer.Text = "Customer"
        lblOrderType.Text = "Order Type"
        lblERPNo.Text = "Invoice No"
        lblPurDate.Text = "Purchase Date"
        lblCurrency.Text = "Currency"
        lblFlow.Text = "Status"
        cmdPickCustomer.Text = "Select"
        txtErpNo.ReadOnly = True

        Me.UI_cmdSave.Text = _oLanguage.getText("Warranty", "039", ctlLanguage.eumType.Tag)
        pnlFile.Visible = False
        pnAdd.Visible = False
    End Sub

    Private Sub QueryData(ByVal pComNo As String)
        If Me.ViewState("_eumCommand") = eumCommand.AddNew Then
            Exit Sub
        End If


        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.WARRANTYORDDataTable
        Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
        UI_cmdSave.Visible = False
        ' btnPI.Visible = False

        dtData = oWarranty.QueryWorder(sSwID, pComNo, "", "", "", "")
        Dim i As Integer = 0
        If dtData.Count > 0 Then
            Dim dr As WarrantyDTO.WARRANTYORDRow = dtData.Rows(0)

            txtWarrantyNo.Text = dr.WATY_NO
            cboOperationCenter.SelectedValue = dr.WATY_COMPNO.ToString().Trim()
            cboWarrantyType.SelectedValue = dr.WATY_ORDERTYPE.ToString().Trim()
            cboOperationCenter.Enabled = False
            txtCurrency.Enabled = False

            txtCustomer.Text = dr.WATY_CUST.ToString().Trim()
            txtCustomerName.Text = dr.CU_NAME.ToString().Trim()

            txtCustomerName.Width = 100
            If Not dr.IsWATY_SALESIDNull Then
                txtSales.Text = dr.WATY_SALESID
            End If

            If Not dr.IsWATY_CUST_PONull Then
                txtCustPo.Text = dr.WATY_CUST_PO
            End If
            txtCustPo.Enabled = False

            'fairy
            strAD = dr.WATY_AD
            strCenter = dr.WATY_COMPNO

            If Not dr.Isgen02Null Then
                txtSalesName.Text = dr.gen02
            End If

            txtSales.Enabled = False
            txtSalesName.Enabled = False

            txtCustomer.Enabled = False
            txtCustomerName.Enabled = False
            cmdPickCustomer.Visible = False
            If Not dr.IsWATY_ERPNONull Then txtErpNo.Text = dr.WATY_ERPNO.ToString().Trim()
            If Not dr.IsWATY_DATENull Then txtPurDate.Text = dr.WATY_DATE.ToString("yyyy/MM/dd")
            If Not dr.IsWATY_CURRNull Then txtCurrency.Text = dr.WATY_CURR.ToString().Trim()

            txtFlow.Text = dr.WATY_FLOW.ToString().Trim()
            txtConfirm.Text = dr.ISConfirm.ToString().Trim()
            txtWarrantyNo.Enabled = False
            pnlFile.Visible = True
            pnAdd.Visible = True

            QueryItm()

            If txtConfirm.Text.Trim() = "Y" Then
                btnDelete.Visible = False
                btnSave.Visible = False
                btnSubmit.Visible = False
                pnAdd.Visible = False
                If txtFlow.Text.Trim() = "Y" Then
                    btnPI.Visible = True
                End If
            End If
        End If

    End Sub
    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSave.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim sMessage As String = ""
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim sRepair As String = ""
        Dim sRole As String = ""
        Dim sSwID As String = txtWarrantyNo.Text.ToString().Trim()

        Dim oWarranty As New ctlWarranty
        Dim dtWorder As New WarrantyDTO.WARRANTYORDDataTable

        Try
            Dim dr As WarrantyDTO.WARRANTYORDRow = dtWorder.NewWARRANTYORDRow

            dr.WATY_COMPNO = cboOperationCenter.SelectedValue.Trim()
            dr.WATY_CUST = txtCustomer.Text.ToString().Trim()
            dr.WATY_DATE = Convert.ToDateTime(txtPurDate.Text)
            dr.WATY_ORDERTYPE = cboWarrantyType.SelectedValue.Trim()
            dr.WATY_CURR = txtCurrency.Text
            dr.WATY_ERPNO = txtErpNo.Text
            dr.WATY_FLOW = txtFlow.Text
            dr.WATY_SALESID = txtSales.Text
            dr.WATY_CUST_PO = txtCustPo.Text

            dr.WATY_AD = Session("_UserID")
            dr.WATY_NO = ""
            dr.WATY_ADNAME = Session("_UserName")
            dr.WATY_CSTMP = Date.Now
            dr.WATY_LUAD = Session("_UserID")
            dr.WATY_LUADNAME = Session("_UserName")
            dr.WATY_LUSTMP = Date.Now
            dr.WATY_MARK = 0
            dr.ISFLOW = "N"
            dr.ISConfirm = "N"
            dtWorder.AddWARRANTYORDRow(dr)

            txtWarrantyNo.Text = oWarranty.SaveAddWarrantyOrd(dtWorder)
            txtWarrantyNo.Enabled = False
            blnFlag = True
            pnlFile.Visible = True
            pnAdd.Visible = True
            UI_cmdSave.Visible = False
            btnDelete.Visible = False
            txtCustomer.Enabled = False
            txtCustomerName.Enabled = False
            cmdPickCustomer.Visible = False
            txtCurrency.Enabled = False

            oCommon.getWarrsetTypeByDropDownList(Me.rdoType, "", 1, txtCustomer.Text)

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
            End If
        End Try
    End Sub

    Protected Sub cmdPickCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPickCustomer.Click
        If cboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.cboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        Me.ucWarrantyOrderCust.show(True, Me.ViewState("_OperationCenter").ToString.Trim)
    End Sub

    Protected Sub rdoType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoType.SelectedIndexChanged
        Dim oWarranty As New ctlWarranty
        txtPurchaseYear.Enabled = True
        'If rdoType.SelectedValue.ToString().Trim() = "CW" Then
        '    Dim dtCwVer As New WarrantyDTO.WARRSETDataTable
        '    dtCwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), "CW", txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
        '    dtCwVer.DefaultView.RowFilter = "WAR_STATUS=2"
        '    cboVersion.DataTextField = "war_name"
        '    cboVersion.DataValueField = "war_id"
        '    cboVersion.DataSource = dtCwVer.DefaultView
        '    cboVersion.DataBind()
        'End If
        'If rdoType.SelectedValue.ToString().Trim() = "EW" Then
        '    Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
        '    dtEwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), "EW", txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
        '    dtEwVer.DefaultView.RowFilter = "WAR_STATUS=2"
        '    cboVersion.DataTextField = "war_name"
        '    cboVersion.DataValueField = "war_id"
        '    cboVersion.DataSource = dtEwVer.DefaultView
        '    cboVersion.DataBind()
        'End If

        Dim dtCwVer As New WarrantyDTO.WARRSETDataTable
        dtCwVer = oWarranty.QueryWarrSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), rdoType.SelectedValue.ToString().Trim(), txtCustomer.Text.Trim(), "Y", "a.WAR_GROUP,a.WAR_VERSION")
        dtCwVer.DefaultView.RowFilter = "WAR_STATUS=2"
        cboVersion.DataTextField = "war_name"
        cboVersion.DataValueField = "war_id"
        cboVersion.DataSource = dtCwVer.DefaultView
        cboVersion.DataBind()

        If rdoType.SelectedValue.ToString().Trim() = "SW" Then
            Dim dtSwVer As New WarrantyDTO.SWSETDataTable
            dtSwVer = oWarranty.QuerySWSet("", cboOperationCenter.SelectedValue, txtModel.Text.Trim(), -1, "Y", "")
            dtSwVer.DefaultView.RowFilter = "SW_STATUS=2"
            cboVersion.DataTextField = "sw_name"
            cboVersion.DataValueField = "sw_id"
            cboVersion.DataSource = dtSwVer.DefaultView
            cboVersion.DataBind()
            SetYear()
        End If
    End Sub
    Protected Sub cboVersion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboVersion.SelectedIndexChanged
        If rdoType.SelectedValue.ToString().Trim() = "SW" Then
            SetYear()
        End If
    End Sub

    Protected Sub btnSaveOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveOrder.Click
        System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False

        If (rdoType.SelectedValue = "-1" OrElse txtPurchaseYear.Text = "") Then
            sMessage = "Warranty Type Not choose OR Purchase Year Not keyin!"
            Me.ucMessage.showMessageByFailed(sMessage)
            Exit Sub
        End If

        Dim oWarranty As New ctlWarranty
        Dim dtWarrantyItem As New WarrantyDTO.WARRANTYITEMDataTable

        Try
            If txtOrderNo.Text.Trim().Equals("") And txtSKU.Text.Trim().Equals("") Then
                Throw New Exception("Please input query condition first!!")
            End If

            Dim WatiName As String = oWarranty.QueryWarrantyName(cboVersion.SelectedValue.Trim(), DateTime.Now.AddDays(1), DateTime.Now.AddYears(txtPurchaseYear.Text))
            'WatiName += "Type " + txtModel.Text.Trim()
            'If rdoType.SelectedValue.Trim() = "CW" Then
            '    WatiName += " Com W."
            'End If
            'If rdoType.SelectedValue.Trim() = "EW" Then
            '    WatiName += " Ext W."
            '    '如果選擇SW,OrderNo不可以空白  20140804
            '    'EW 可以訂單資料空白 - Frank Chen 151118
            '    'If txtOrderNo.Text.Trim().Equals("") Then
            '    'Throw New Exception("Order No, can not be null, please input again!")
            '    'End If
            'End If
            If rdoType.SelectedValue.Trim() = "SW" Then
                WatiName += "Type " + txtModel.Text.Trim()
                WatiName += " Spe W."
            End If

            If txtOrderNo.Text.Trim().Equals("") Then
                Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.NewWARRANTYITEMRow
                dr.WATI_WATYNO = txtWarrantyNo.Text
                dr.WATI_ORDER = ""
                dr.WATI_SKUNO = txtSKU.Text.Trim()
                dr.wati_model = txtModel.Text
                dr.wati_name = WatiName
                dr.WATI_SKUDESC = txtDescription.Text.Trim()
                dr.WATI_TYPE = rdoType.SelectedValue.Trim()
                dr.WATI_VER = cboVersion.SelectedValue.Trim()
                dr.WATI_DESC = ""
                dr.WATI_YEAR = txtPurchaseYear.Text
                dr.WATI_QTY = 0
                dr.WATI_PRICE = 0.0
                dr.WATI_BASE = 0.0
                dr.WATI_AD = Session("_UserID")
                dr.WATI_ADNAME = Session("_UserName")
                dr.WATI_CSTMP = Date.Now
                dr.WATI_LUAD = Session("_UserID")
                dr.WATI_LUADNAME = Session("_UserName")



                dr.WATI_LUSTMP = Date.Now
                dr.WATI_MARK = 0
                dtWarrantyItem.AddWARRANTYITEMRow(dr)
            Else
                Dim oCustomer As New ctlCustomer.Customer
                Dim dtOrder As New DataTable
                If Me.ViewState("_OperationCenter") = "CL_CHINA" Then
                    dtOrder = oCustomer.QryOrderSH(txtCustomer.Text.Trim(), txtOrderNo.Text.Trim(), txtOrderSeq.Text.Trim(), txtSKU.Text.Trim())
                Else
                    dtOrder = oCustomer.QryOrder(txtCustomer.Text.Trim(), txtOrderNo.Text.Trim(), txtOrderSeq.Text.Trim(), txtSKU.Text.Trim())
                End If
                '  For i = 0 To dtOrder.Rows.Count - 1

                Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.NewWARRANTYITEMRow
                dr.WATI_WATYNO = txtWarrantyNo.Text
                dr.WATI_ORDER = dtOrder.Rows(i)("OEA01").ToString()
                dr.WATI_ORDSEQ = dtOrder.Rows(i)("OEB03").ToString()
                dr.WATI_SKUNO = dtOrder.Rows(i)("OEB04").ToString()
                dr.WATI_SKUDESC = dtOrder.Rows(i)("OEB06").ToString()
                dr.wati_model = txtModel.Text
                dr.wati_name = WatiName
                dr.WATI_TYPE = rdoType.SelectedValue.Trim()
                dr.WATI_VER = cboVersion.SelectedValue.Trim()
                dr.WATI_DESC = ""
                dr.WATI_YEAR = txtPurchaseYear.Text
                dr.WATI_QTY = dtOrder.Rows(i)("OEB12").ToString()
                '
                dr.WATI_PRICE = 0.0
                dr.WATI_BASE = 0.0


                dr.WATI_AD = Session("_UserID")
                dr.WATI_ADNAME = Session("_UserName")
                dr.WATI_CSTMP = Date.Now
                dr.WATI_LUAD = Session("_UserID")
                dr.WATI_LUADNAME = Session("_UserName")
                dr.WATI_LUSTMP = Date.Now
                dr.WATI_MARK = 0
                dtWarrantyItem.AddWARRANTYITEMRow(dr)
                '  Next
            End If

            oWarranty.SaveAddWarrantyItem(dtWarrantyItem)

            blnFlag = True
            QueryItm()

            txtOrderNo.Text = ""
            txtOrderSeq.Text = ""
            txtSKU.Text = ""
            txtSKU.Enabled = True
            btnSKUSel.Visible = True
            txtModel.Text = ""
            txtDescription.Text = ""
            txtPurchaseYear.Text = ""

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
            End If
        End Try
    End Sub

    Protected Sub btnSKUSel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSKUSel.Click
        If cboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.cboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        Me.ucWarrantyOrderSKU.show(txtCustomer.Text.Trim(), True, Me.ViewState("_OperationCenter").ToString.Trim)
    End Sub

    Protected Sub btnOrderSel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrderSel.Click

        If cboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.cboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        Me.ucWarrantyOrderOrder.show(txtCustomer.Text, True, Me.ViewState("_OperationCenter").ToString.Trim)
    End Sub

    Public Sub QueryItm()
        Dim sOrderNo As String = txtWarrantyNo.Text.ToString().Trim()
        btnSubmit.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        btnSubmit.Text = "Confirm"
        txtDiffCnt.Text = "0"

        If sOrderNo <> "" Then
            Dim oWarranty As New ctlWarranty
            Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
            dtData = oWarranty.QueryWarrantyItem(sOrderNo, -1, "")

            For i = 0 To dtData.Rows.Count - 1
                Dim wat_ver_act As String = dtData.Rows(i)("WATI_VER_ACT").ToString().Trim()
                If wat_ver_act.Length > 0 Then
                    Dim wat_ver_act_last As String = Mid(wat_ver_act, wat_ver_act.Length - 1, 1)
                    If IsNumeric(wat_ver_act_last) = True Then
                        If wat_ver_act_last = "1" Or wat_ver_act_last = "R" Or wat_ver_act_last = "r" Then

                        Else
                            dtData.Rows(i)("WATI_VER_ACT") = Mid(wat_ver_act, 1, wat_ver_act.Length - 2) & "1" & Mid(wat_ver_act, wat_ver_act.Length, 1)
                        End If
                    End If
                End If
            Next

            dvWarrantyItem.DataSource = dtData
            dvWarrantyItem.DataBind()

            If dtData.Rows.Count > 0 Then
                btnSubmit.Visible = True
                btnSave.Visible = True
                btnDelete.Visible = True
            End If
        End If
    End Sub

    Protected Sub dvWarrantyItem_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dvWarrantyItem.RowCommand

        If e.CommandName = "cmdAdd" Or e.CommandName = "cmdDetail" Or e.CommandName = "cmdDel" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.dvWarrantyItem.Rows(iIndex)

            Dim lblwati_seq As Label = row.FindControl("lblwati_seq")
            Dim lblOrder As Label = row.FindControl("lblOrder")
            Dim lblQty As Label = row.FindControl("lblQty")

            If e.CommandName = "cmdAdd" Then
                Dim nStdQty As Integer = Convert.ToInt32(lblQty.Text)
                If lblOrder.Text.Trim() = "" Then
                    nStdQty = -1
                End If

                Me.ucWarrantyOrderSNAdd.show(True, txtWarrantyNo.Text.Trim(), lblwati_seq.Text.Trim(), nStdQty)
            End If
            If e.CommandName = "cmdDel" Then
                Dim oWarranty As New ctlWarranty
                oWarranty.SaveDelWarrantyItem(txtWarrantyNo.Text.Trim(), Convert.ToInt32(lblwati_seq.Text), Session("_UserID"), Session("_UserName"))
                QueryItm()
            End If
            If e.CommandName = "cmdDetail" Then
                Me.ucWarrantyOrderSNView.show(True, txtWarrantyNo.Text.Trim(), lblwati_seq.Text.Trim())
            End If
        End If

    End Sub

    Protected Sub dvWarrantyItem_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dvWarrantyItem.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnSerial As Button = e.Row.FindControl("btnSerial")
            Dim btnDelete As Button = e.Row.FindControl("btnDelete")
            Dim btnView As Button = e.Row.FindControl("btnView")
            Dim lblwati_base As Label = e.Row.FindControl("lblwati_base")
            Dim txtPrice As TextBox = e.Row.FindControl("txtPrice")

            If txtConfirm.Text.Trim() = "Y" Then
                btnSerial.Visible = False
                btnView.Visible = True
                btnDelete.Visible = False
            Else
                btnSerial.Visible = True
                btnView.Visible = False
                btnDelete.Visible = True
            End If

            If Convert.ToDouble(txtPrice.Text) < Convert.ToDouble(lblwati_base.Text) Then
                txtDiffCnt.Text = Convert.ToInt32(txtDiffCnt.Text) + 1
                btnSubmit.Text = "Flow"
            End If

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Warranty_SellingafterOrder.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim oWarranty As New ctlWarranty
            Dim dtWorder As New WarrantyDTO.WARRANTYORDDataTable
            Dim dr As WarrantyDTO.WARRANTYORDRow = dtWorder.NewWARRANTYORDRow
            dr.WATY_NO = txtWarrantyNo.Text
            dr.ISConfirm = "Y"
            dr.WATY_LUAD = Session("_UserID")
            dr.WATY_LUADNAME = Session("_UserName")
            dr.WATY_LUSTMP = Date.Now
            dr.WATY_MARK = 1
            dtWorder.AddWARRANTYORDRow(dr)
            oWarranty.SaveEditWarrantyOrd(dtWorder, False)
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                sMessage = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageBySuccess(sMessage, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SellingafterOrder.aspx")
            End If
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        'Dim sTitle As String = txtSubmit.Text
        Try
            btnSubmit.Text = txtSubmit.Text
            Dim oWarranty As New ctlWarranty
            Dim dtWarrantyItem As New WarrantyDTO.WARRANTYITEMDataTable

            Dim i As Integer = 0
            For i = 0 To dvWarrantyItem.Rows.Count - 1
                If dvWarrantyItem.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim lblwati_seq As Label = dvWarrantyItem.Rows(i).FindControl("lblwati_seq")
                    Dim txtPrice As TextBox = dvWarrantyItem.Rows(i).FindControl("txtPrice")

                    Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.NewWARRANTYITEMRow
                    dr.WATI_WATYNO = txtWarrantyNo.Text
                    dr.wati_seq = lblwati_seq.Text
                    dr.WATI_PRICE = txtPrice.Text

                    dr.WATI_AD = Session("_UserID")
                    dr.WATI_ADNAME = Session("_UserName")
                    dr.WATI_CSTMP = Date.Now
                    dr.WATI_LUAD = Session("_UserID")
                    dr.WATI_LUADNAME = Session("_UserName")
                    dr.WATI_LUSTMP = Date.Now
                    dr.WATI_MARK = 0
                    dtWarrantyItem.AddWARRANTYITEMRow(dr)
                End If
            Next
            oWarranty.SaveEditWarrantyItem(dtWarrantyItem)
            blnFlag = True
            QueryItm()
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                sMessage = oCommon.getMessage(Common.enmMessage.EditOK)
                Me.ucMessage.showMessageByAlert(sMessage)
            End If
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click


        Dim msg As String = ""


        '檢查數量
        Try
            'Total Loss		
            Dim Total_Loss_Qty As String = ""
            If 1 = 1 Then
                Dim i As Integer = 0

                Dim Warranty_TypeSetting_add_oWarranty As New ctlWarranty
                Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                EXPORT_EXPORT_ORDERNUMBER = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYITEM_WATI_WATYNO(txtWarrantyNo.Text.Trim())

                For i = 0 To EXPORT_EXPORT_ORDERNUMBER.Rows.Count - 1

                    If EXPORT_EXPORT_ORDERNUMBER.Rows(i)(0).ToString() = "OK" Then
                        Total_Loss_Qty = "YES"
                    End If

                    If EXPORT_EXPORT_ORDERNUMBER.Rows(i)(0).ToString() = "NO" Then

                        If EXPORT_EXPORT_ORDERNUMBER.Rows(i)(1).ToString() = "" Then

                        Else
                            msg += "第" & (i + 1).ToString & "筆 MOQ  Total Loss 最少購買 " & EXPORT_EXPORT_ORDERNUMBER.Rows(i)(1).ToString() & "個</br>"
                        End If

                    End If

                Next

                If msg <> "" Then
                    Me.ucMessage.showMessageByFailed(msg)
                End If
            End If

            '最低起批量
            If Total_Loss_Qty <> "YES" Then
                Dim i As Integer = 0

                Dim Warranty_TypeSetting_add_oWarranty As New ctlWarranty
                Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                EXPORT_EXPORT_ORDERNUMBER = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYITEM_WATI_WATYNO_QTY_ALL(txtWarrantyNo.Text.Trim())

                For i = 0 To EXPORT_EXPORT_ORDERNUMBER.Rows.Count - 1
                    If EXPORT_EXPORT_ORDERNUMBER.Rows(i)(0).ToString() = "NO" Then

                        If EXPORT_EXPORT_ORDERNUMBER.Rows(i)(1).ToString() = "" Then

                        Else
                            msg += "第" & (i + 1).ToString & "筆 MOQ  最少購買 " & EXPORT_EXPORT_ORDERNUMBER.Rows(i)(1).ToString() & "個</br>"
                        End If

                    End If

                Next

                If msg <> "" Then
                    Me.ucMessage.showMessageByFailed(msg)
                End If
            End If

        Catch ex As Exception

        End Try

        If msg = "" Then
            Dim sMessage As String = ""
            Dim blnFlag As Boolean = False
            Dim bFlow As Boolean = False
            Dim sOrderNo As String = txtWarrantyNo.Text.ToString().Trim()
            Try
                Dim oWarranty As New ctlWarranty
                Dim dtWarrantyItem As New WarrantyDTO.WARRANTYITEMDataTable

                Dim i As Integer = 0
                For i = 0 To dvWarrantyItem.Rows.Count - 1
                    If dvWarrantyItem.Rows(i).RowType = DataControlRowType.DataRow Then
                        Dim lblwati_seq As Label = dvWarrantyItem.Rows(i).FindControl("lblwati_seq")
                        Dim txtPrice As TextBox = dvWarrantyItem.Rows(i).FindControl("txtPrice")
                        Dim txtBase As TextBox = dvWarrantyItem.Rows(i).FindControl("txtBase")

                        If Convert.ToDouble(txtPrice.Text) < Convert.ToDouble(txtBase.Text) Then
                            bFlow = True
                        End If

                        Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.NewWARRANTYITEMRow
                        dr.WATI_WATYNO = txtWarrantyNo.Text
                        dr.wati_seq = lblwati_seq.Text
                        dr.WATI_PRICE = txtPrice.Text

                        dr.WATI_AD = Session("_UserID")
                        dr.WATI_ADNAME = Session("_UserName")
                        dr.WATI_CSTMP = Date.Now
                        dr.WATI_LUAD = Session("_UserID")
                        dr.WATI_LUADNAME = Session("_UserName")
                        dr.WATI_LUSTMP = Date.Now
                        dr.WATI_MARK = 0
                        dtWarrantyItem.AddWARRANTYITEMRow(dr)
                    End If
                Next
                oWarranty.SaveEditWarrantyItem(dtWarrantyItem)

                Dim dtWorder As New WarrantyDTO.WARRANTYORDDataTable
                Dim dro As WarrantyDTO.WARRANTYORDRow = dtWorder.NewWARRANTYORDRow
                dro.WATY_NO = txtWarrantyNo.Text
                dro.ISConfirm = "Y"
                dro.WATY_ORDERTYPE = cboWarrantyType.SelectedValue.Trim()
                dro.WATY_CURR = txtCurrency.Text
                dro.WATY_LUAD = Session("_UserID")
                dro.WATY_LUADNAME = Session("_UserName")
                dro.WATY_LUSTMP = Date.Now
                dro.WATY_MARK = 0
                dtWorder.AddWARRANTYORDRow(dro)

                '2025/01/06 新增 電池保固 開始
                If 1 = 1 Then

                    Dim WAP_WID As String = "" 'WAP_WID  保固號碼    
                    Dim WAP_RULE As String = ""  'WAP_RULE   保固電池類型

                    Dim WATS_WATYNO As String = ""
                    Dim WATS_WATYSEQ As Integer = 0
                    Dim WATS_SN As String = ""               '保固序號
                    Dim WATS_WARRNSTART As Date = Nothing    '保固開始日

                    Dim WATS_WARRN_ORDER_14 As Date = Nothing
                    Dim WATS_WARRN_18 As Date = Nothing
                    Dim WATS_WARRN_ORDER_32 As Date = Nothing
                    Dim WATS_WARRN_36 As Date = Nothing

                    Dim WATS_AD As String = ""
                    Dim WATS_ADNAME As String = ""
                    Dim WATS_CSTMP As Date = Nothing

                    WAP_WID = "1" 'WAP_WID  保固號碼    
                    WAP_RULE = "1"  'WAP_RULE   保固電池類型

                    If WAP_WID <> "" And WAP_RULE <> "" Then
                        Dim Warranty_TypeSetting_add_oWarranty As New ctlWarranty
                        Dim EXPORT_EXPORT_ORDERNUMBER As New DataTable
                        EXPORT_EXPORT_ORDERNUMBER = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYSERIAL_BI(txtWarrantyNo.Text.Trim())

                        For i = 0 To EXPORT_EXPORT_ORDERNUMBER.Rows.Count - 1



                            WATS_WATYNO = EXPORT_EXPORT_ORDERNUMBER.Rows(i)("WATS_WATYNO").ToString()
                            WATS_WATYSEQ = Convert.ToInt32(EXPORT_EXPORT_ORDERNUMBER.Rows(i)("WATS_WATYSEQ").ToString())
                            WATS_SN = EXPORT_EXPORT_ORDERNUMBER.Rows(i)("WATS_SN").ToString()
                            WATS_WARRNSTART = Convert.ToDateTime(EXPORT_EXPORT_ORDERNUMBER.Rows(i)("WATS_WARRNSTART").ToString())

                            Dim Warranty_TypeSetting_add_oWarranty_BI As New ctlWarranty

                            If WAP_RULE = "18" Then
                                WATS_WARRN_ORDER_14 = WATS_WARRNSTART.AddMonths(14)
                                WATS_WARRN_18 = WATS_WARRNSTART.AddMonths(18)
                            Else
                                WATS_WARRN_ORDER_14 = WATS_WARRNSTART.AddMonths(14)
                                WATS_WARRN_18 = WATS_WARRNSTART.AddMonths(18)
                                WATS_WARRN_ORDER_32 = WATS_WARRNSTART.AddMonths(28)
                                WATS_WARRN_36 = WATS_WARRNSTART.AddMonths(36)

                            End If

                            WATS_AD = Session("_UserID")
                            WATS_ADNAME = Session("_UserName")
                            WATS_CSTMP = Date.Now


                            If 1 = 1 Then

                                Dim EXPORT_EXPORT_ORDERNUMBER_ As New DataTable
                                EXPORT_EXPORT_ORDERNUMBER_ = Warranty_TypeSetting_add_oWarranty.Select_WARRANTYITEM_BI(EXPORT_EXPORT_ORDERNUMBER.Rows(i)("WATI_VER").ToString())

                                For a = 0 To EXPORT_EXPORT_ORDERNUMBER_.Rows.Count - 1

                                    WAP_WID = EXPORT_EXPORT_ORDERNUMBER_.Rows(a)("WAP_WID").ToString() 'WAP_WID  保固號碼    
                                    WAP_RULE = EXPORT_EXPORT_ORDERNUMBER_.Rows(a)("WAP_RULE").ToString()  'WAP_RULE   保固電池類型
                                    Warranty_TypeSetting_add_oWarranty_BI.WARRANTYSERIAL_BI_Insert(WATS_WATYNO, WATS_WATYSEQ, WATS_SN, WAP_WID, WAP_RULE, WATS_WARRN_ORDER_14, WATS_WARRN_18, WATS_WARRN_ORDER_32, WATS_WARRN_36, WATS_AD, WATS_ADNAME, WATS_CSTMP)
                                Next
                            End If

                        Next


                    End If


                    If msg <> "" Then
                        Me.ucMessage.showMessageByFailed(msg)
                    End If
                End If
                '2025/01/06 新增 電池保固 結束


                'Throw New Exception(txtSubmit.Text)
                'If txtSubmit.Text.Trim() = "Flow" Then
                If bFlow Then
                    '140428 by cipherlab.fairy----(S)
                    Dim FlowWS As New WorkflowService.WorkflowServiceService()
                    Dim formOID As String = FlowWS.findFormOIDsOfProcess("WarrantyOrder")
                    Dim formXML As New XmlDocument()
                    formXML.LoadXml(FlowWS.getFormFieldTemplate(formOID))
                    formXML.DocumentElement.SelectSingleNode("waty_no").InnerText = txtWarrantyNo.Text
                    formXML.DocumentElement.SelectSingleNode("waty_custid").InnerText = txtCustomer.Text
                    formXML.DocumentElement.SelectSingleNode("waty_custname").InnerText = txtCustomerName.Text
                    formXML.DocumentElement.SelectSingleNode("waty_compno").InnerText = strCenter
                    formXML.DocumentElement.SelectSingleNode("waty_date").InnerText = txtPurDate.Text
                    formXML.DocumentElement.SelectSingleNode("waty_curr").InnerText = txtCurrency.Text
                    'formXML.DocumentElement.SelectSingleNode("waty_ad").InnerText = strAD
                    formXML.DocumentElement.SelectSingleNode("waty_salesid").InnerText = txtSales.Text
                    formXML.DocumentElement.SelectSingleNode("waty_salesname").InnerText = txtSalesName.Text
                    'Throw New Exception(FlowWS.getFormFieldTemplate(formOID))
                    'Throw New Exception(strCenter)
                    'DataSet(oDS)  '撈單身資料
                    Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
                    dtData = oWarranty.QueryWarrantyItem(sOrderNo, -1, "")

                    '設定datagrid
                    Dim xnl As XmlNode
                    xnl = formXML.DocumentElement.SelectSingleNode("wogrid/records")
                    Dim xn As XmlNode
                    xn = xnl.FirstChild.Clone()
                    xnl.RemoveAll()
                    xn.Attributes("id").InnerText = "wogrid_0"
                    xn.SelectSingleNode("//item[@id='wati_seq']").InnerText = dtData.Rows(0)("wati_seq").ToString
                    xn.SelectSingleNode("//item[@id='wati_type']").InnerText = dtData.Rows(0)("wati_type").ToString
                    xn.SelectSingleNode("//item[@id='wati_skuno']").InnerText = dtData.Rows(0)("wati_skuno").ToString
                    xn.SelectSingleNode("//item[@id='wati_skudesc']").InnerText = dtData.Rows(0)("wati_skudesc").ToString
                    xn.SelectSingleNode("//item[@id='wati_year']").InnerText = dtData.Rows(0)("wati_year").ToString
                    xn.SelectSingleNode("//item[@id='wati_qty']").InnerText = dtData.Rows(0)("wati_qty").ToString
                    xn.SelectSingleNode("//item[@id='wati_price']").InnerText = dtData.Rows(0)("wati_price").ToString
                    xn.SelectSingleNode("//item[@id='wati_base']").InnerText = dtData.Rows(0)("wati_base").ToString
                    xnl.AppendChild(xn)
                    'Throw New Exception(dvWarrantyItem.Rows.Count)
                    For i = 1 To dtData.Rows.Count - 1 Step 1
                        xn = xnl.FirstChild.Clone()
                        xn.Attributes("id").InnerText = "wogrid_" + i.ToString
                        xn.SelectSingleNode("//item[@id='wati_seq']").InnerText = dtData.Rows(i)("wati_seq").ToString
                        xn.SelectSingleNode("//item[@id='wati_type']").InnerText = dtData.Rows(i)("wati_type").ToString
                        xn.SelectSingleNode("//item[@id='wati_skuno']").InnerText = dtData.Rows(i)("wati_skuno").ToString
                        xn.SelectSingleNode("//item[@id='wati_skudesc']").InnerText = dtData.Rows(i)("wati_skudesc").ToString
                        xn.SelectSingleNode("//item[@id='wati_year']").InnerText = dtData.Rows(i)("wati_year").ToString
                        xn.SelectSingleNode("//item[@id='wati_qty']").InnerText = dtData.Rows(i)("wati_qty").ToString
                        xn.SelectSingleNode("//item[@id='wati_price']").InnerText = dtData.Rows(i)("wati_price").ToString
                        xn.SelectSingleNode("//item[@id='wati_base']").InnerText = dtData.Rows(i)("wati_base").ToString
                        xnl.AppendChild(xn)
                    Next

                    'Throw New Exception(formXML.InnerXml)
                    'strAD = "0231"
                    'Dim strDept As String = "30840"
                    Dim loginAD = Session("_UserID")
                    Dim loginDept As String = Session("_DeptNO")

                    Dim processID As String
                    processID = FlowWS.invokeProcess("WarrantyOrder", loginAD, loginDept, formOID, formXML.InnerXml, "WRMA-保固訂單" + txtWarrantyNo.Text)
                    'Dim sMessage As String = processID
                    '140428 by cipherlab.fairy----(E)

                    oWarranty.SaveEditWarrantyOrd(dtWorder, True)           '更新waty_flow='F'

                Else
                    oWarranty.SaveEditWarrantyOrd(dtWorder, False)
                End If
                blnFlag = True
            Catch ex As Exception
                sMessage = ex.Message
                blnFlag = False
            Finally
                If blnFlag = False Then
                    Me.ucMessage.showMessageByFailed(sMessage)
                Else
                    sMessage = oCommon.getMessage(Common.enmMessage.EditOK)
                    Me.ucMessage.showMessageBySuccess(sMessage, ascx_ucMessage.eumTransferURL.Redirect, "Warranty_SellingafterOrder.aspx")
                End If
            End Try
        End If
    End Sub
    Protected Sub btnPI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPI.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim doc As New Document()
        Try
            Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtData As New DataTable
            dtData = oWarranty.QueryPIPrint(sSwID)

            If dtData.Rows.Count > 0 Then

                Dim ms As New MemoryStream()

                Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
                writer.CloseStream = False
                doc.Open()

                Dim cb As PdfContentByte = writer.DirectContent
                Dim bf As BaseFont = BaseFont.CreateFont(sFontPath & "\msjhbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
                Dim ftStd As New iTextSharp.text.Font(bf, 6, 0)   '字體
                Dim ftBold As New iTextSharp.text.Font(bf, 10, 0)

                Dim jpeg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath("PdfLogo.jpg"))
                doc.Add(jpeg)

                Dim pg1 As New Paragraph("CIPHERLAB Co., Ltd.", ftBold)
                pg1.Alignment = 1
                doc.Add(pg1)
                Dim pgr As New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                Dim pg2 As New Paragraph("Proforma Invoice", ftStd)
                pg2.Alignment = 1
                doc.Add(pg2)

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                Dim sStr As String = ""
                sStr = "COMPANY ADDRESS :" + dtData.Rows(0)("COMP_ADDRESS").ToString()
                pg2 = New Paragraph(sStr, ftStd)
                pg2.Alignment = 1
                doc.Add(pg2)

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                doc.Add(pgr)

                Dim t01 As New PdfPTable(6)
                t01.TotalWidth = 523
                t01.LockedWidth = True
                t01.SetWidths(New Integer() {1, 2, 1, 2, 1, 2})

                'Row 1
                sStr = "DATE PRINTED :"
                Dim cell As New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.FixedHeight = 15.0F
                cell.BorderWidth = 0
                t01.AddCell(cell)

                sStr = dtData.Rows(0)("SYSDATE").ToString()
                If sStr <> "" Then
                    sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                End If
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.BorderWidth = 0
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                t01.AddCell(cell)

                sStr = "TEL. :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.BorderWidth = 0
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                t01.AddCell(cell)

                sStr = dtData.Rows(0)("COMP_TEL").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.BorderWidth = 0
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                t01.AddCell(cell)


                sStr = "DATE :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.BorderWidth = 0
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                t01.AddCell(cell)

                sStr = dtData.Rows(0)("WATY_DATE").ToString()
                If sStr <> "" Then
                    sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                End If
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.BorderWidth = 0
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                t01.AddCell(cell)
                doc.Add(t01)

                Dim t02 As New PdfPTable(4)
                t02.TotalWidth = 523
                t02.LockedWidth = True
                t02.SetWidths(New Integer() {1, 2, 1, 2})

                'Row 1
                sStr = "CUSTOMER:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.FixedHeight = 15.0F
                cell.Border = PdfPCell.TOP_BORDER
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OCC18").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 3
                cell.Border = PdfPCell.TOP_BORDER
                t02.AddCell(cell)

                'Row 2
                sStr = "ATTN:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OCC29").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.Colspan = 3
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                'Row 3
                sStr = "TEL:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OCC261").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = "PI. NO:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("WATY_NO").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                'Row 4
                sStr = "FAX:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OCC271").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.Colspan = 3
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                'Row 4
                sStr = "PAYMENT TERM:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OAG02").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 3
                cell.BorderWidth = 0
                t02.AddCell(cell)

                'Row 5
                sStr = "PRICE TERM:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t02.AddCell(cell)

                sStr = dtData.Rows(0)("OAH02").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 3
                cell.Border = PdfPCell.BOTTOM_BORDER
                t02.AddCell(cell)

                'sStr = "PAGE:"
                'cell = New PdfPCell(New Phrase(sStr, ftStd))
                'cell.VerticalAlignment = Element.ALIGN_MIDDLE
                'cell.BorderWidth = 0
                't02.AddCell(cell)

                'sStr = " 1 "
                'cell = New PdfPCell(New Phrase(sStr, ftStd))
                'cell.VerticalAlignment = Element.ALIGN_MIDDLE
                't02.AddCell(cell)
                'cell.BorderWidth = 0
                doc.Add(t02)

                Dim t03 As New PdfPTable(7)
                t03.TotalWidth = 523
                t03.LockedWidth = True
                t03.SetWidths(New Integer() {1, 2, 1, 1, 1, 1, 1})

                'Row 1
                sStr = "ITEM NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 20.0F
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t03.AddCell(cell)

                sStr = "PRODUCT NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t03.AddCell(cell)

                sStr = "DELIVERY DATE"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t03.AddCell(cell)

                sStr = "QUANTITY"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.HorizontalAlignment = 1
                t03.AddCell(cell)

                sStr = "UNIT PRICE"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.HorizontalAlignment = 1
                t03.AddCell(cell)

                sStr = "CURRENCY"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t03.AddCell(cell)

                sStr = "AMOUNT"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.HorizontalAlignment = 2
                t03.AddCell(cell)

                Dim i As Integer = 0
                Dim dTotQty As Integer = 0.0
                Dim dTotAmt As Double = 0.0
                For i = 0 To dtData.Rows.Count - 1

                    sStr = dtData.Rows(i)("wati_seq").ToString().Trim()
                    cell.FixedHeight = 15.0F
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = dtData.Rows(i)("wati_skuno").ToString().Trim()
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = dtData.Rows(i)("WATY_DATE").ToString().Trim()
                    If sStr <> "" Then
                        sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                    End If
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = dtData.Rows(i)("wati_qty").ToString().Trim() + "  PCS"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.HorizontalAlignment = 1
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    Dim dPrice As Double = Convert.ToDouble(dtData.Rows(i)("wati_price").ToString())
                    Dim dQty As Double = Convert.ToDouble(dtData.Rows(i)("wati_qty").ToString())
                    sStr = dtData.Rows(i)("wati_price").ToString().Trim()
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.HorizontalAlignment = 1
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = dtData.Rows(i)("WATY_CURR").ToString().Trim()
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = (dPrice * dQty).ToString("N2")
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.HorizontalAlignment = 2
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = ""
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.FixedHeight = 15.0F
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t03.AddCell(cell)

                    sStr = dtData.Rows(i)("wati_name").ToString().Trim()
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.BorderWidth = 0
                    cell.Colspan = 6
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    t03.AddCell(cell)

                    dTotQty += dQty
                    dTotAmt += dPrice * dQty

                Next

                'TOTAL QTY
                sStr = ""
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.FixedHeight = 15.0F
                cell.Colspan = 2
                cell.BorderWidth = 0
                t03.AddCell(cell)

                sStr = "TOTAL QTY :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t03.AddCell(cell)

                sStr = dTotQty.ToString("N0")
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t03.AddCell(cell)

                sStr = "TOTAL AMT  :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.BorderWidth = 0
                t03.AddCell(cell)

                sStr = dTotAmt.ToString("N2")
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 2
                cell.BorderWidth = 0
                t03.AddCell(cell)

                'SAY TOTAL :
                sStr = "SAY TOTAL:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.FixedHeight = 15.0F
                cell.Border = PdfPCell.TOP_BORDER
                t03.AddCell(cell)

                sStr = GetNumberWord(dTotAmt)
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 6
                cell.Border = PdfPCell.TOP_BORDER
                t03.AddCell(cell)

                'SAY TOTAL :
                sStr = "REMARK :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = " "
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Colspan = 6
                cell.BorderWidth = 0
                t03.AddCell(cell)

                '空白一行 
                sStr = ""
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 1
                cell.BorderWidth = 0
                cell.Colspan = 7
                cell.FixedHeight = 30.0F
                t03.AddCell(cell)

                'RETURNED
                sStr = "RETURNED GOODS ,  7 DAYS AFTER THE GOODS IS RECEIVED"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.Colspan = 7
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                'BANK
                sStr = "BANK :"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.Colspan = 2
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = dtData.Rows(0)("NMT02").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.Colspan = 5
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                'SWIFT CODE 
                sStr = "SWIFT CODE:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.Colspan = 2
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = dtData.Rows(0)("OCJ02").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.Colspan = 5
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                'ACCOUNT 
                sStr = "ACCOUNT NO. OF BENEFICIARY:"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.Colspan = 2
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = dtData.Rows(0)("OCJ03").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.Colspan = 5
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                '空白一行 
                sStr = ""
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 1
                cell.BorderWidth = 0
                cell.Colspan = 7
                cell.FixedHeight = 30.0F
                t03.AddCell(cell)

                'SINCERELY 
                sStr = "SINCERELY YOURS"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 2
                cell.BorderWidth = 0
                cell.Colspan = 5
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = ""
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 0
                cell.BorderWidth = 0
                cell.Colspan = 2
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                'SINCERELY 
                sStr = dtData.Rows(0)("OCC29").ToString() + "           SIGNATURE"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 2
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = "___________________________"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 0
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = "CIPHERLAB CO., LTD."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 2
                cell.BorderWidth = 0
                cell.Colspan = 3
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                sStr = "___________________________"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 0
                cell.BorderWidth = 0
                cell.Colspan = 2
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                ' 空白
                sStr = ""
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 1
                cell.BorderWidth = 0
                cell.Colspan = 7
                cell.Rowspan = 3
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                'OCC29
                sStr = dtData.Rows(0)("OCC29").ToString()
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.BorderWidth = 0
                cell.FixedHeight = 15.0F
                't03.AddCell(cell)

                sStr = "SIGNATURE"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 1
                cell.BorderWidth = 0
                cell.Colspan = 6
                cell.FixedHeight = 15.0F
                't03.AddCell(cell)

                'Last
                sStr = "(END)"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_TOP
                cell.HorizontalAlignment = 2
                cell.BorderWidth = 0
                cell.Colspan = 7
                cell.FixedHeight = 15.0F
                t03.AddCell(cell)

                doc.Add(t03)


                doc.Close()
                Dim buffer As Byte() = New Byte(ms.Length - 1) {}
                ms.Position = 0
                ms.Read(buffer, 0, CInt(ms.Length))
                Response.Clear()
                Response.AddHeader("Content-Disposition", "attachment;filename=" & sSwID & ".pdf")
                Response.AddHeader("Content-Length", ms.Length.ToString())
                Response.ContentType = "application/pdf"
                ms.Close()

                Response.BinaryWrite(buffer)
                Response.OutputStream.Flush()
                Response.OutputStream.Close()
            End If
            blnFlag = True
        Catch ex As Exception
            doc.Close()
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    Protected Sub RunPrint(ByVal WatyNo As String)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtReport As New DataTable

        Dim oWarranty As New ctlWarranty
        dtReport = oWarranty.QueryPIPrint(WatyNo)

        Call Print(dtReport)
    End Sub

    Private Sub Print(ByVal dtReport As DataTable)
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        ReportDoc.Load(Server.MapPath("Report\PI.rpt"))
        ReportDoc.SetDataSource(oReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, _ReportToPDF)
        oCommon.OpenPdf(Me, _ReportToPDF)
        'ExportSetup()
        'ConfigureExportToPdf()
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
    End Sub

    'Public Sub ExportSetup()
    '    If Not System.IO.Directory.Exists(_Reoprt_FilePath) Then
    '        System.IO.Directory.CreateDirectory(_Reoprt_FilePath)
    '    End If

    '    myDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    myExportOptions = ReportDoc.ExportOptions
    '    ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
    'End Sub

    'Public Sub ConfigureExportToPdf()
    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & _ReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Dim sScript As String = ""
    '    sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    sScript = sScript & "</script>" & vbCrLf
    '    Response.Write(sScript)
    'End Sub

    '取得額外規格於PDF上顯示 MODI BY ANGEL ON 20160118
    Private Function runFnGetImc04_1(ByVal SKUNO_IN As String, ByVal CUSTID_IN As String) As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        ' Throw New Exception(SKUNO_IN)
        Try

            oCommand.CommandText = "FnGetImc04_1"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("vima01", OracleType.NVarChar).Value = SKUNO_IN
            oCommand.Parameters("vima01").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vocc01", OracleType.NVarChar).Value = CUSTID_IN
            oCommand.Parameters("vocc01").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 4000)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

    Protected Sub btnService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnService.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim doc As New Document()
        Try
            Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtMData As New DataTable
            dtMData = oWarranty.QueryPIPrint(sSwID)

            If dtMData.Rows.Count > 0 Then


                'Dim ms As New MemoryStream()

                Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.Create))

                writer.CloseStream = False
                doc.Open()

                Dim cb As PdfContentByte = writer.DirectContent
                Dim bf As BaseFont = BaseFont.CreateFont(sFontPath & "\msjhbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
                'Add New Barcode Fount , you need modify the font name from msjhbd.ttf to ....，你可以使用 code39或code93碼，字體沒有我可以給你
                '或者從你們公司找合適的字體
                Dim bfBar As BaseFont = BaseFont.CreateFont(sFontPath & "\FRE3OF9X.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)

                Dim ftStd As New iTextSharp.text.Font(bf, 8, 0)   '改成8 fairy
                'Add New Barcode Format
                '此地方的字體用6號，如果條碼太小你可以調大
                Dim ftStdBar As New iTextSharp.text.Font(bfBar, 24, 0)
                Dim ftBold As New iTextSharp.text.Font(bf, 10, 0)
                Dim ftBold1 As New iTextSharp.text.Font(bf, 14, 0)

                Dim sStr As String = dtMData.Rows(0)("SYSDATE").ToString()
                If sStr <> "" Then
                    sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                End If

                Dim pg0 As New Paragraph("Print Date: " + sStr, ftStd)
                pg0.Alignment = 2
                doc.Add(pg0)

                Dim jpeg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath("PdfLogo.jpg"))
                doc.Add(jpeg)

                Dim pg1 As New Paragraph("CipherLab Co., Ltd.", ftBold1)
                pg1.Alignment = 1
                doc.Add(pg1)
                Dim pgr As New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                Dim pg2 As New Paragraph("Service Advantage", ftBold)
                pg2.Alignment = 1
                doc.Add(pg2)

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr) '此處加入 這是加一行的意思嗎？

                'Add New Slip Number In Here,you need modify OCC18 to 你的欄位（哈哈，你竟然也用漢語拼音）
                Dim pg2Barcode As New Paragraph("*" + dtMData.Rows(0)("SlipNo").ToString() + "*", ftStdBar)
                pg2Barcode.Alignment = 2 '靠右對其
                doc.Add(pg2Barcode) '此處加入

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)
                Dim t00 As New PdfPTable(2)
                t00.TotalWidth = 523
                t00.LockedWidth = True
                t00.DefaultCell.BorderWidth = 0
                t00.SetWidths(New Integer() {1, 1})

                sStr = "CUSTOMER: " + dtMData.Rows(0)("OCC18").ToString()
                Dim cell As New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t00.AddCell(cell)

                sStr = "No: " + sSwID
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.HorizontalAlignment = 2
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t00.AddCell(cell)
                doc.Add(t00)

                Dim t01 As New PdfPTable(5)
                t01.TotalWidth = 523
                t01.LockedWidth = True
                t01.DefaultCell.BorderWidth = 0
                t01.SetWidths(New Integer() {2, 3, 1, 2, 4})

                sStr = "ITEM NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t01.AddCell(cell)

                sStr = "DESCRIPTION"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                sStr = "Q’TY"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.HorizontalAlignment = 1
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                sStr = "SERIAL NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                sStr = "WARRANTY DETAIL"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
                dtData = oWarranty.QueryWarrantyItem(sSwID, -1, "")

                Dim i As Integer = 0
                For i = 0 To dtData.Rows.Count - 1
                    Dim dr As WarrantyDTO.WARRANTYITEMRow = dtData.Rows(i)

                    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                    dtWarrantySerial = oWarranty.QueryWarrantySerial(dr.WATI_WATYNO, dr.wati_seq, "", "wats_sn")

                    sStr = dr.WATI_SKUNO
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = runFnGetImc04_1(dr.WATI_SKUNO, dtMData.Rows(0)("OCC18").ToString())
                    '  If Not dr.IsWATI_SKUDESCNull Then
                    'sStr = dr.WATI_SKUDESC
                    'End If
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t01.AddCell(cell)

                    sStr = dr.WATI_QTY
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.HorizontalAlignment = 1
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t01.AddCell(cell)

                    sStr = ""
                    If dtWarrantySerial.Rows.Count > 0 Then
                        sStr = dtWarrantySerial.Rows(0)("wats_sn").ToString()
                    End If
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t01.AddCell(cell)

                    sStr = ""
                    If Not dr.Iswati_nameNull Then
                        sStr = dr.wati_name
                    End If
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    t01.AddCell(cell)

                    Dim j As Integer = 1
                    For j = 1 To dtWarrantySerial.Rows.Count - 1
                        Dim drs As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(j)

                        sStr = ""
                        cell = New PdfPCell(New Phrase(sStr, ftStd))
                        cell.FixedHeight = 18.0F
                        cell.BorderWidth = 0
                        t01.AddCell(cell)

                        sStr = ""
                        cell = New PdfPCell(New Phrase(sStr, ftStd))
                        cell.BorderWidth = 0
                        t01.AddCell(cell)

                        sStr = ""
                        cell = New PdfPCell(New Phrase(sStr, ftStd))
                        cell.BorderWidth = 0
                        t01.AddCell(cell)

                        sStr = drs.WATS_SN
                        cell = New PdfPCell(New Phrase(sStr, ftStd))
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE
                        cell.BorderWidth = 0
                        t01.AddCell(cell)

                        sStr = ""
                        cell = New PdfPCell(New Phrase(sStr, ftStd))
                        cell.BorderWidth = 0
                        t01.AddCell(cell)

                    Next

                Next
                doc.Add(t01)
                '保持空白
                For i = 0 To 76 - dtData.Rows.Count * 3
                    pgr = New Paragraph(" ")
                    pgr.SetLeading(0.0F, 0.5F)
                    doc.Add(pgr)
                Next

                ' 空白
                Dim pg110 As New Paragraph("------------------------------------------------------------------------------------------------------------------------------------------------------", ftStd)
                pg110.Alignment = 1
                doc.Add(pg110)

                Dim pg111 As New Paragraph("Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C.", ftStd)
                pg111.Alignment = 0
                doc.Add(pg111)

                Dim pg112 As New Paragraph("Tel: +886 2 86471166      Fax: +886 2 87323300      e-mail: salesadmin@cipherlab.com.tw", ftStd)
                pg112.Alignment = 0
                doc.Add(pg112)

                Dim reader As PdfReader
                Dim NewPage As PdfImportedPage
                Dim iPageNum As Integer
                Dim k As Integer

                reader = New PdfReader(Server.MapPath(_pdfSample) + "CipherLab_Warranty_Policy_Terms_EN.pdf")
                iPageNum = reader.NumberOfPages
                For k = 1 To iPageNum
                    doc.NewPage()
                    NewPage = writer.GetImportedPage(reader, k)

                    cb.AddTemplate(NewPage, 0, 0)
                Next

                reader = New PdfReader(Server.MapPath(_pdfSample) + "CipherLab_Warranty_Policy_Terms_TC.pdf")
                iPageNum = reader.NumberOfPages
                For k = 1 To iPageNum
                    doc.NewPage()
                    NewPage = writer.GetImportedPage(reader, k)

                    cb.AddTemplate(NewPage, 0, 0)
                Next
                writer.CloseStream = True
                doc.Close()
                writer.Close()

                'System.Threading.Thread.Sleep(100)
                WatermarkAdd(_Reoprt_FilePath + sSwID + ".pdf", _Reoprt_FilePath + sSwID + "N.pdf", _WaterMarkPic)

                'Dim f As New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.open)
                'ms.SetLength(f.Length)
                'f.Read(ms.GetBuffer(), 0, CInt(f.Length))
                'ms.Flush()
                'f.Close()

                Dim sScript As String = ""
                sScript = sScript & "<script language=""javascript"">" & vbCrLf
                sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & sSwID & "N.pdf" & "','','');" & vbCrLf
                sScript = sScript & "</script>" & vbCrLf
                Response.Write(sScript)

                'Dim buffer As Byte() = New Byte(ms.Length - 1) {}
                'ms.Position = 0
                'ms.Read(buffer, 0, CInt(ms.Length))
                'Response.Clear()
                'Response.AddHeader("Content-Disposition", "attachment;filename=" & sSwID & ".pdf")
                'Response.AddHeader("Content-Length", ms.Length.ToString())
                'Response.ContentType = "application/pdf"
                'ms.Close()

                'Response.BinaryWrite(buffer)
                'Response.OutputStream.Flush()
                'Response.OutputStream.Close()
                blnFlag = True
            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            doc.Close()
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub
    Public Shared Function GetNumberWord(ByVal dValue As Double) As String
        Dim sValue As String = dValue.ToString("#.00")
        Dim sValueBeforeDot As String = sValue.Substring(0, sValue.IndexOf("."))
        Dim sValueAfterDot As String = sValue.Substring(sValue.IndexOf(".") + 1, sValue.Length - sValue.IndexOf(".") - 1)
        Dim sResult As String
        If sValueAfterDot = "00" Then
            sResult = Get2NumberWord(sValueBeforeDot) & " ONLY"
        Else
            sResult = Get2NumberWord(sValueBeforeDot) & " AND CENTS " & Get2NumberWord(sValueAfterDot) & " ONLY"
        End If
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = sResult.Replace("  ", " ")
        sResult = "SAY US DOLLAR " & sResult
        Return (sResult)
    End Function
    Protected Shared Function Get2NumberWord(ByVal sNumber As String) As String
        If sNumber = "" Then
            sNumber = "0"
        End If
        Dim iLength As Integer = sNumber.Length
        Dim iNumber As Integer = Convert.ToInt32(sNumber)
        If iLength < 3 AndAlso iLength > 0 Then
            If iNumber = 0 Then
                Return ("")
            ElseIf iNumber < 20 Then
                Return (conNumberWordLess20(iNumber))
            Else
                Dim iTenNumber As Integer, iOneNumber As Integer
                iTenNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 1))
                iOneNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 1, 1))
                Return (conNumberWordTen(iTenNumber) + "-" + conNumberWordLess20(iOneNumber))
            End If
        ElseIf iLength = 3 Then
            Dim iHundredNumber As Integer, iBelowHundredNumber As Integer
            iHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 1))
            iBelowHundredNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 2, 2))
            If iBelowHundredNumber = 0 Then
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED")
            ElseIf iHundredNumber = 0 Then
                Return ("AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            Else
                Return (conNumberWordLess20(iHundredNumber) + " HUNDRED AND " & Get2NumberWord(iBelowHundredNumber.ToString()))
            End If
        ElseIf iLength > 3 And iLength < 7 Then
            Dim iThousandNumber As Integer, iBelowThousandNumber As Integer
            iThousandNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 3))
            iBelowThousandNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 3, 3))
            Return (Get2NumberWord(iThousandNumber.ToString()) & " THOUNSAND " & IIf(iBelowThousandNumber < 100 And iBelowThousandNumber > 0, "AND ", "") & Get2NumberWord(iBelowThousandNumber.ToString()))
        ElseIf iLength > 6 AndAlso iLength < 10 Then
            Dim iMillionNumber As Integer, iBelowMillionNumber As Integer
            iMillionNumber = Convert.ToInt32(sNumber.Substring(0, sNumber.Length - 6))
            iBelowMillionNumber = Convert.ToInt32(sNumber.Substring(sNumber.Length - 6, 6))
            Return (Get2NumberWord(iMillionNumber.ToString()) & " MILLION " & IIf(iBelowMillionNumber < 100000 And iBelowMillionNumber > 0, "AND ", "") & Get2NumberWord(iBelowMillionNumber.ToString()))
        Else
            Return ("")
        End If

    End Function

    Public Sub SetYear()
        Dim sSWID As String = ""
        If cboVersion.SelectedIndex > -1 Then
            sSWID = cboVersion.SelectedValue.Trim()

            Dim dtSwVer As New WarrantyDTO.SWSETDataTable
            Dim oWarranty As New ctlWarranty
            dtSwVer = oWarranty.QuerySWSet(sSWID, "", "", -1, "")
            If dtSwVer.Rows.Count > 0 Then
                txtPurchaseYear.Text = dtSwVer.Rows(0)("SW_STDYY").ToString()
                txtPurchaseYear.Enabled = False
            End If
        End If
    End Sub
    Public Sub WatermarkAdd(ByVal inputPath As String, ByVal outputPath As String, ByVal watermarkPath As String)
        Try
            Dim pdfReader As New PdfReader(inputPath)
            Dim numberOfPages As Integer = pdfReader.NumberOfPages
            Dim outputStream As New FileStream(outputPath, FileMode.Create)
            Dim pdfStamper As New PdfStamper(pdfReader, outputStream)
            Dim waterMarkContent As PdfContentByte

            Dim watermarkimagepath As String = watermarkPath
            Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(watermarkimagepath)

            image.SetAbsolutePosition(50, 120)
            For i As Integer = 1 To numberOfPages
                If i = 1 Then
                    waterMarkContent = pdfStamper.GetUnderContent(i)
                    waterMarkContent.AddImage(image)
                End If
            Next
            pdfStamper.Close()
            pdfReader.Close()
        Catch ex As Exception
            'WriteLog.Log(ex.ToString())
            Throw ex
        End Try
    End Sub

    Protected Sub btnReMoCloud_Click(sender As Object, e As EventArgs) Handles btnReMoCloud.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Dim doc As New Document()
        Try
            Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
            Dim oWarranty As New ctlWarranty
            Dim dtMData As New DataTable
            dtMData = oWarranty.QueryPIPrint(sSwID)

            If dtMData.Rows.Count > 0 Then


                'Dim ms As New MemoryStream()

                Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.Create))

                writer.CloseStream = False
                doc.Open()

                Dim cb As PdfContentByte = writer.DirectContent
                Dim bf As BaseFont = BaseFont.CreateFont(sFontPath & "\msjhbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
                'Add New Barcode Fount , you need modify the font name from msjhbd.ttf to ....，你可以使用 code39或code93碼，字體沒有我可以給你
                '或者從你們公司找合適的字體
                Dim bfBar As BaseFont = BaseFont.CreateFont(sFontPath & "\FRE3OF9X.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)

                Dim ftStd As New iTextSharp.text.Font(bf, 8, 0)   '改成8 fairy
                'Add New Barcode Format
                '此地方的字體用6號，如果條碼太小你可以調大
                Dim ftStdBar As New iTextSharp.text.Font(bfBar, 24, 0)
                Dim ftBold As New iTextSharp.text.Font(bf, 10, 0)
                Dim ftBold1 As New iTextSharp.text.Font(bf, 14, 0)

                Dim sStr As String = dtMData.Rows(0)("SYSDATE").ToString()
                If sStr <> "" Then
                    sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
                End If

                Dim pg0 As New Paragraph("Print Date: " + sStr, ftStd)
                pg0.Alignment = 2  '靠右
                doc.Add(pg0)

                Dim jpeg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath("PdfLogo.jpg"))
                doc.Add(jpeg)

                Dim pg1 As New Paragraph("CipherLab Co., Ltd.", ftBold1)
                pg1.Alignment = 1
                doc.Add(pg1)

                'pg.SetAlignment("Justify"); //左右對齊
                'pg.FirstLineIndent = 20.0F;   //段落句首縮排
                'pg.SetLeading(0.0F, 2.0F);  //設定行距

                Dim pgr As New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                Dim pg2 As New Paragraph("ReMoCloud User Licenses", ftBold)
                pg2.Alignment = 1
                doc.Add(pg2)

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr) '此處加入 這是加一行的意思嗎？

                ''Add New Slip Number In Here,you need modify OCC18 to 你的欄位（哈哈，你竟然也用漢語拼音）
                'Dim pg2Barcode As New Paragraph("*" + dtMData.Rows(0)("SlipNo").ToString() + "*", ftStdBar)
                'pg2Barcode.Alignment = 2 '靠右對其
                'doc.Add(pg2Barcode) '此處加入

                pgr = New Paragraph(" ")
                pgr.SetLeading(0.0F, 0.5F)
                doc.Add(pgr)

                Dim t00 As New PdfPTable(2)
                t00.TotalWidth = 523
                t00.LockedWidth = True
                t00.DefaultCell.BorderWidth = 0
                t00.SetWidths(New Integer() {1, 1})

                sStr = "CUSTOMER: " + dtMData.Rows(0)("OCC18").ToString()
                Dim cell As New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t00.AddCell(cell)

                sStr = "No: " + sSwID
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.HorizontalAlignment = 2
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t00.AddCell(cell)
                doc.Add(t00)

                Dim t01 As New PdfPTable(4)
                t01.TotalWidth = 523
                t01.LockedWidth = True
                t01.DefaultCell.BorderWidth = 0
                t01.SetWidths(New Integer() {1, 3, 1, 1})

                sStr = "ITEM NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                cell.FixedHeight = 20.0F
                t01.AddCell(cell)

                sStr = "DESCRIPTION"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                sStr = "Q’TY"
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.HorizontalAlignment = 1
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.HorizontalAlignment = Element.ALIGN_BOTTOM
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                sStr = "SERIAL NO."
                cell = New PdfPCell(New Phrase(sStr, ftStd))
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Border = PdfPCell.BOTTOM_BORDER
                t01.AddCell(cell)

                Dim iItem As Integer = 0
                For iItem = 0 To 0
                    sStr = "A904RMCS00001"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = "REMOCLOUD-STANDARD-1 YEAR"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = "4"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.HorizontalAlignment = Element.ALIGN_BOTTOM
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)


                    sStr = "See below"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)



                    sStr = "FW120C1001009"
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = ""
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = ""
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                    sStr = ""
                    cell = New PdfPCell(New Phrase(sStr, ftStd))
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.BorderWidth = 0
                    cell.FixedHeight = 18.0F
                    t01.AddCell(cell)

                Next



                'Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
                'dtData = oWarranty.QueryWarrantyItem(sSwID, -1, "")

                'Dim i As Integer = 0
                'For i = 0 To dtData.Rows.Count - 1
                '    Dim dr As WarrantyDTO.WARRANTYITEMRow = dtData.Rows(i)

                '    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                '    dtWarrantySerial = oWarranty.QueryWarrantySerial(dr.WATI_WATYNO, dr.wati_seq, "", "wats_sn")

                '    sStr = dr.WATI_SKUNO
                '    cell = New PdfPCell(New Phrase(sStr, ftStd))
                '    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '    cell.BorderWidth = 0
                '    cell.FixedHeight = 18.0F
                '    t01.AddCell(cell)

                '    sStr = runFnGetImc04_1(dr.WATI_SKUNO, dtMData.Rows(0)("OCC18").ToString())
                '    '  If Not dr.IsWATI_SKUDESCNull Then
                '    'sStr = dr.WATI_SKUDESC
                '    'End If
                '    cell = New PdfPCell(New Phrase(sStr, ftStd))
                '    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '    cell.BorderWidth = 0
                '    t01.AddCell(cell)

                '    sStr = dr.WATI_QTY
                '    cell = New PdfPCell(New Phrase(sStr, ftStd))
                '    cell.HorizontalAlignment = 1
                '    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '    cell.BorderWidth = 0
                '    t01.AddCell(cell)

                '    sStr = ""
                '    If dtWarrantySerial.Rows.Count > 0 Then
                '        sStr = dtWarrantySerial.Rows(0)("wats_sn").ToString()
                '    End If
                '    cell = New PdfPCell(New Phrase(sStr, ftStd))
                '    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '    cell.BorderWidth = 0
                '    t01.AddCell(cell)

                '    sStr = ""
                '    If Not dr.Iswati_nameNull Then
                '        sStr = dr.wati_name
                '    End If
                '    cell = New PdfPCell(New Phrase(sStr, ftStd))
                '    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '    cell.BorderWidth = 0
                '    t01.AddCell(cell)

                '    Dim j As Integer = 1
                '    For j = 1 To dtWarrantySerial.Rows.Count - 1
                '        Dim drs As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(j)

                '        sStr = ""
                '        cell = New PdfPCell(New Phrase(sStr, ftStd))
                '        cell.FixedHeight = 18.0F
                '        cell.BorderWidth = 0
                '        t01.AddCell(cell)

                '        sStr = ""
                '        cell = New PdfPCell(New Phrase(sStr, ftStd))
                '        cell.BorderWidth = 0
                '        t01.AddCell(cell)

                '        sStr = ""
                '        cell = New PdfPCell(New Phrase(sStr, ftStd))
                '        cell.BorderWidth = 0
                '        t01.AddCell(cell)

                '        sStr = drs.WATS_SN
                '        cell = New PdfPCell(New Phrase(sStr, ftStd))
                '        cell.VerticalAlignment = Element.ALIGN_MIDDLE
                '        cell.BorderWidth = 0
                '        t01.AddCell(cell)

                '        sStr = ""
                '        cell = New PdfPCell(New Phrase(sStr, ftStd))
                '        cell.BorderWidth = 0
                '        t01.AddCell(cell)

                '    Next

                'Next


                doc.Add(t01)
                '保持空白
                For i = 0 To 76 - 1 * 3
                    pgr = New Paragraph(" ")
                    pgr.SetLeading(0.0F, 0.5F)
                    doc.Add(pgr)
                Next

                ' 空白
                Dim pg110 As New Paragraph("------------------------------------------------------------------------------------------------------------------------------------------------------", ftStd)
                pg110.Alignment = 1
                doc.Add(pg110)

                Dim pg111 As New Paragraph("Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C.", ftStd)
                pg111.Alignment = 0
                doc.Add(pg111)

                Dim pg112 As New Paragraph("Tel: +886 2 86471166      Fax: +886 2 87323300      e-mail: salesadmin@cipherlab.com.tw", ftStd)
                pg112.Alignment = 0
                doc.Add(pg112)

                Dim reader As PdfReader
                Dim NewPage As PdfImportedPage
                Dim iPageNum As Integer
                Dim k As Integer

                reader = New PdfReader(Server.MapPath(_pdfSample) + "ReMoCloud_EULA.PDF")
                iPageNum = reader.NumberOfPages
                For k = 1 To iPageNum
                    doc.NewPage()
                    NewPage = writer.GetImportedPage(reader, k)

                    cb.AddTemplate(NewPage, 0, 0)
                Next

                reader = New PdfReader(Server.MapPath(_pdfSample) + "ReMoCloud_Terms_and_Conditions.pdf")
                iPageNum = reader.NumberOfPages
                For k = 1 To iPageNum
                    doc.NewPage()
                    NewPage = writer.GetImportedPage(reader, k)

                    cb.AddTemplate(NewPage, 0, 0)
                Next
                writer.CloseStream = True
                doc.Close()
                writer.Close()

                'System.Threading.Thread.Sleep(100)
                WatermarkAdd(_Reoprt_FilePath + sSwID + ".pdf", _Reoprt_FilePath + sSwID + "N.pdf", "d:\\eRMAtest\\BasePIC.jpg")

                'Dim f As New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.open)
                'ms.SetLength(f.Length)
                'f.Read(ms.GetBuffer(), 0, CInt(f.Length))
                'ms.Flush()
                'f.Close()

                Dim sScript As String = ""
                sScript = sScript & "<script language=""javascript"">" & vbCrLf
                sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & sSwID & "N.pdf" & "','','');" & vbCrLf
                sScript = sScript & "</script>" & vbCrLf
                Response.Write(sScript)

                'Dim buffer As Byte() = New Byte(ms.Length - 1) {}
                'ms.Position = 0
                'ms.Read(buffer, 0, CInt(ms.Length))
                'Response.Clear()
                'Response.AddHeader("Content-Disposition", "attachment;filename=" & sSwID & ".pdf")
                'Response.AddHeader("Content-Length", ms.Length.ToString())
                'Response.ContentType = "application/pdf"
                'ms.Close()

                'Response.BinaryWrite(buffer)
                'Response.OutputStream.Flush()
                'Response.OutputStream.Close()
                blnFlag = True
            End If
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            doc.Close()
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try
    End Sub

    Protected Sub btnNewService_Click(sender As Object, e As EventArgs) Handles btnNewService.Click
        Dim oWarranty As New ctlWarranty
        Dim sSwID As String = Me.txtWarrantyNo.Text.ToString().Trim()
        Dim iFontSize As Integer = 9
        Dim iIndex As Integer = 0

        Dim doc As Document = New Document(PageSize.A4, 20, 20, 140, 40)
        Dim ms As New MemoryStream()
        'Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(_Reoprt_FilePath + sSwID + ".pdf", FileMode.Create))
        Dim PageEventHandler As PdfPageEventPG = New PdfPageEventPG()

        writer.CloseStream = False

        Dim dtMData As DataTable = oWarranty.QueryPIPrint(sSwID)
        Dim oDSGift As DataTable = oWarranty.GetRMCGift(sSwID)


        writer.PageEvent = PageEventHandler
        PageEventHandler.SetHeader("This Is Hearder")
        writer.PdfVersion = PdfWriter.VERSION_1_7
        writer.SetPdfVersion(PdfName.VERSION)

        Dim sStr As String = dtMData.Rows(0)("SYSDATE").ToString()
        If sStr <> "" Then
            sStr = Convert.ToDateTime(sStr).ToString("yyyy/MM/dd")
            PageEventHandler.PrintDate = sStr
        End If

        PageEventHandler.SwID = sSwID
        PageEventHandler.ShipNO = dtMData.Rows(0)("SlipNo").ToString()
        PageEventHandler.CustomerNO = dtMData.Rows(0)("OCC18").ToString()
        PageEventHandler.Addr = "Address: 12F, 333 Dunhua S. Rd., Sec. 2, Taipei, Taiwan 106, R.O.C."
        PageEventHandler.Tel = "Tel: +886 2 86471166  Fax: +886 2 87323300   e - mail: salesadmin @cipherlab.com.tw"

        doc.Open()

        Dim cb As PdfContentByte = writer.DirectContent
        Dim ct As ColumnText = New ColumnText(cb)

        Dim Bfont As BaseFont = BaseFont.CreateFont(sFontPath + "\msjh.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
        Dim font9 As Font = New Font(Bfont, 9, 0)

        'Phrase helloPhrase = New Phrase();            
        'Font helloFont = New iTextSharp.text.Font(chBaseFont, 14);

        Dim t01 As PdfPTable = New PdfPTable(4)
        t01.TotalWidth = 523
        t01.LockedWidth = True
        t01.DefaultCell.BorderWidth = 0
        t01.SetWidths(New Integer() {1, 2, 1, 2})

        sStr = "ITEM NO."
        'Dim TitleCell As PdfPCell = New PdfPCell(New Phrase(sStr, New Font(Font.FontFamily.HELVETICA, iFontSize)))
        Dim TitleCell As PdfPCell = New PdfPCell(New Phrase(sStr, font9))
        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
        TitleCell.Border = PdfPCell.BOTTOM_BORDER
        TitleCell.FixedHeight = 20.0F
        t01.AddCell(TitleCell)

        sStr = "DESCRIPTION" '+ Chr(13) + "aaaAAA123"
        TitleCell = New PdfPCell(New Phrase(sStr, font9))
        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
        TitleCell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(TitleCell)

        sStr = "Q’TY"
        TitleCell = New PdfPCell(New Phrase(sStr, font9))
        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
        TitleCell.HorizontalAlignment = Element.ALIGN_CENTER
        TitleCell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(TitleCell)

        sStr = "WARRANTY DETAIL"
        TitleCell = New PdfPCell(New Phrase(sStr, font9))
        TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
        TitleCell.Border = PdfPCell.BOTTOM_BORDER
        t01.AddCell(TitleCell)

        'doc.Add(t01)

        Dim t02 As PdfPTable = New PdfPTable(4) 'SerialNO
        Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
        dtData = oWarranty.QueryWarrantyItem(sSwID, -1, "")

        Dim MVPdf As Boolean = False
        Dim BatteryPdf As Boolean = False
        Dim SpecPdf As Boolean = False

        Dim i As Integer = 0
        For i = 0 To dtData.Rows.Count - 1
            Dim dr As WarrantyDTO.WARRANTYITEMRow = dtData.Rows(i)

            If (dr.WATI_TYPE = "PB" Or dr.WATI_TYPE = "EB") Then
                BatteryPdf = True
            End If
            If dr.WATI_TYPE = "M0" Then
                MVPdf = True
            End If

            Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
            dtWarrantySerial = oWarranty.QueryWarrantySerial(dr.WATI_WATYNO, dr.wati_seq, "", "wats_sn")

            sStr = dr.WATI_SKUNO
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.BorderWidth = 0
            TitleCell.FixedHeight = 18.0F
            t01.AddCell(TitleCell)

            sStr = runFnGetImc04_1(dr.WATI_SKUNO, dtMData.Rows(0)("OCC18").ToString())
            '  If Not dr.IsWATI_SKUDESCNull Then
            'sStr = dr.WATI_SKUDESC
            'End If
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.BorderWidth = 0
            t01.AddCell(TitleCell)

            sStr = dr.WATI_QTY
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.HorizontalAlignment = 1
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.BorderWidth = 0
            t01.AddCell(TitleCell)

            'sStr = ""
            'If dtWarrantySerial.Rows.Count > 0 Then
            '    sStr = dtWarrantySerial.Rows(0)("wats_sn").ToString()
            'End If
            'cell = New PdfPCell(New Phrase(sStr, ftStd))
            'cell.VerticalAlignment = Element.ALIGN_MIDDLE
            'cell.BorderWidth = 0
            't01.AddCell(cell)

            sStr = ""
            If Not dr.Iswati_nameNull Then
                sStr = dr.wati_name
            End If
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.BorderWidth = 0
            t01.AddCell(TitleCell)

            '空白一行 
            sStr = " "
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.HorizontalAlignment = 1
            TitleCell.BorderWidth = 0
            TitleCell.Colspan = 4
            TitleCell.FixedHeight = 10.0F
            t01.AddCell(TitleCell)

            Dim j As Integer = 0
            For j = 0 To dtWarrantySerial.Rows.Count - 1
                Dim drs As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(j)

                Using drsn As DataTable = oWarranty.QueryWarrantySerial(drs.WATS_SN, dr.WATI_TYPE)
                    sStr = drsn.Rows(0)("wats_sn").ToString
                    TitleCell = New PdfPCell(New Phrase(sStr, font9))
                    TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                    TitleCell.BorderWidth = 0
                    t02.AddCell(TitleCell)
                End Using

            Next

            For j = 1 To 4 - (dtWarrantySerial.Rows.Count Mod 4)
                sStr = " "
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.BorderWidth = 0
                t02.AddCell(TitleCell)
            Next

            '空白一行 
            sStr = " "
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
            TitleCell.BorderWidth = 0
            TitleCell.Colspan = 4
            TitleCell.FixedHeight = 10.0F
            t02.AddCell(TitleCell)

            '特殊簽呈
            If Not dr.IsWAR_SPEC_DESCNull Then
                sStr = " "
                sStr = dr.WAR_SPEC_DESC
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_TOP
                TitleCell.BorderWidth = 0
                TitleCell.Colspan = 4
                t02.AddCell(TitleCell)

                Dim dt As DataTable = oWarranty.QrySpecWarranty(dr.WATI_VER)
                If dt.Rows.Count > 0 Then
                    If (dt.Rows(0)("WAP_NAME") = "Losstop%") Then
                        SpecPdf = True
                    End If

                End If
            End If
            ' 保固卡加入Warranty Card Content by buck add 20260128 begin
            Dim dtWARRSET = oWarranty.QueryWARRSETALL()
            Dim t03 As PdfPTable = New PdfPTable(1)
            sStr = dtWARRSET.AsEnumerable.
                            Where(Function(x) x.Field(Of String)("war_id") = dr.WATI_VER).
                            Select(Function(y) y.Field(Of String)("WAR_Card_Content")).First()
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.BorderWidth = 0
            t03.AddCell(TitleCell)

            '空白一行 
            sStr = " "
            TitleCell = New PdfPCell(New Phrase(sStr, font9))
            TitleCell.VerticalAlignment = Element.ALIGN_TOP
            TitleCell.HorizontalAlignment = 1
            TitleCell.BorderWidth = 0
            TitleCell.Colspan = 4
            TitleCell.FixedHeight = 10.0F
            t03.AddCell(TitleCell)
            ' 保固卡加入Warranty Card Content by buck add 20260128 end

            '贈送RMC
            Dim t04 As PdfPTable = New PdfPTable(1)
            t04.DefaultCell.Border = Rectangle.NO_BORDER
            t04.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
            t04.TotalWidth = 523
            t04.LockedWidth = True

            iIndex = 0
            Dim drGifts As DataRow() = oDSGift.Select("OGB01='" + dr.WATI_WATYNO + "' AND OGB03='" + dr.wati_seq.ToString() + "'")

            For iIndex = 0 To drGifts.Count - 1
                Dim drGift As DataRow = drGifts(iIndex)

                'RMA 保固贈送連結取消 by buck modify 20251204 begin
                If Date.Parse("2026/1/1") > Date.Parse(txtPurDate.Text) Then
                    Dim link As Font = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLUE)
                    Dim click As Anchor = New Anchor(drGift("AKEY_IMA01").ToString() + "  Register RemoCloud " + drGift("quantity").ToString() + " PCS ", link)
                    click.Reference = "http://ackey.cipherlab.com.tw/ackey_generate18/index18?pra=" + drGift("ENCRYPT_KEY").ToString()
                    Dim p1 As Paragraph = New Paragraph()
                    p1.Add(click)
                    t04.AddCell(p1)
                End If
                'RMA 保固贈送連結取消 by buck modify 20251204 end
                '空白一行 
                sStr = " "
                TitleCell = New PdfPCell(New Phrase(sStr, font9))
                TitleCell.VerticalAlignment = Element.ALIGN_MIDDLE
                TitleCell.BorderWidth = 0
                TitleCell.FixedHeight = 25.0F
                t04.AddCell(TitleCell)
            Next

            doc.Add(t01)
            doc.Add(t02)
            doc.Add(t03)
            If (oDSGift.Rows.Count > 0) Then
                doc.Add(t04)
            End If
            t01.FlushContent()
            t02.FlushContent()
            t03.FlushContent()
            t04.FlushContent()

        Next

        writer.CloseStream = True

        doc.Close()
        writer.Close()

        '合併PDF
        Dim PDFFiles As List(Of String) = New List(Of String)()
        PDFFiles.Add(_Reoprt_FilePath + sSwID + ".pdf")
        PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_Warranty_Policy_EN.pdf")
        PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_Warranty_Policy_TC.pdf")
        If BatteryPdf Then

            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Battery_Insurance_Policy_EN.pdf")
            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Battery_Insurance_Policy_TC.pdf")


            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab Battery Insurance Policy-220621_C.PDF")
            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab Battery Insurance Policy-220621_E.PDF")

        End If
        If MVPdf Then
            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_of_Material_Warranty_Policy_EN.pdf")
            PDFFiles.Add(Server.MapPath(_pdfSample) + "CipherLab_Service_Advantage_of_Material_Warranty_Policy_TC.pdf")
        End If
        If SpecPdf Then
            PDFFiles.Add(Server.MapPath(_pdfSample) + "TotalLossFinal_EN.pdf")
            PDFFiles.Add(Server.MapPath(_pdfSample) + "TotalLossFinal_TC.pdf")
        End If

        Dim sFileName As String = sSwID + "N.pdf"

        iTextSharpPdfMerge(PDFFiles, _Reoprt_FilePath + "\\" + sFileName)

        If File.Exists(_Reoprt_FilePath + sSwID + ".pdf") Then
            File.Delete(_Reoprt_FilePath + sSwID + ".pdf")
        End If

        'PageEventHandler.bPDFPage = writer.PageNumber  '為了寫頁首頁尾

        ''For i = 0 To 100
        ''    doc.Add(New Paragraph("the first page" + i.ToString()))
        ''Next

        'Dim reader As PdfReader
        'Dim NewPage As PdfImportedPage
        'Dim iPageNum As Integer
        'Dim k As Integer

        'reader = New PdfReader(Server.MapPath(_pdfSample) + "CipherLab_Warranty_Policy_Terms_EN.pdf")
        'iPageNum = reader.NumberOfPages
        'For k = 1 To iPageNum
        '    doc.NewPage()
        '    NewPage = writer.GetImportedPage(reader, k)

        '    cb.AddTemplate(NewPage, 0, 0)
        'Next

        'reader = New PdfReader(Server.MapPath(_pdfSample) + "CipherLab_Warranty_Policy_Terms_TC.pdf")
        'iPageNum = reader.NumberOfPages
        'For k = 1 To iPageNum
        '    doc.NewPage()
        '    NewPage = writer.GetImportedPage(reader, k)

        '    cb.AddTemplate(NewPage, 0, 0)
        'Next
        'writer.CloseStream = True

        'doc.Close()
        'writer.Close()


        Dim sScript As String = ""
        sScript = sScript & "<script language=""javascript"">" & vbCrLf
        sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & sSwID & "N.pdf" & "','','');" & vbCrLf
        sScript = sScript & "</script>" & vbCrLf
        Response.Write(sScript)

    End Sub

    Private Sub iTextSharpPdfMerge(ByVal inFiles As List(Of String), ByVal outFile As String)
        Using stream = New FileStream(outFile, FileMode.Create)

            Using doc = New Document()

                Using pdf = New PdfCopy(doc, stream)
                    doc.Open()
                    For Each file As String In inFiles
                        Dim reader = New PdfReader(file)

                        For i As Integer = 0 To reader.NumberOfPages - 1
                            Dim page = pdf.GetImportedPage(reader, i + 1)
                            pdf.AddPage(page)
                        Next

                        pdf.FreeReader(reader)
                        reader.Close()
                    Next

                End Using
            End Using
        End Using
    End Sub

End Class

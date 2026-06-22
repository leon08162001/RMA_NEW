Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class BossShipment_Notice
    Inherits System.Web.UI.Page
    Dim _Crypto As New SecurityCrypt.Crypto
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim i As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True
            Response.Redirect("BossDefault.aspx")
        End If

        If Me.IsPostBack = False Then
            Call setDefault()

            Dim UI_lblPreviousPage_RMASMID As Label = Me.PreviousPage.FindControl("UI_lblPreviousPage_RMASMID")
            Me.UI_lblPreviousPage_RMASMID.Text = UI_lblPreviousPage_RMASMID.Text.ToString().Trim()

            Call QueryData_RMAShipping()        'show表頭
        End If

    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "147", ctlLanguage.eumType.Tag)
        Me.UI_lblShipmentInformation.Text = _oLanguage.getText("RMA", "148", ctlLanguage.eumType.Tag)
        Me.UI_lblNotice.Text = _oLanguage.getText("RMA", "144", ctlLanguage.eumType.Tag)
        Me.UI_lblDate.Text = _oLanguage.getText("RMA", "097", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingOrders.Text = _oLanguage.getText("RMA", "149", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingNumber.Text = _oLanguage.getText("RMA", "150", ctlLanguage.eumType.Tag)
        Me.UI_lblMemo.Text = _oLanguage.getText("RMA", "151", ctlLanguage.eumType.Tag)
        Me.UI_lblShippingTittle.Text = _oLanguage.getText("RMA", "152", ctlLanguage.eumType.Tag)

        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub

    ''' <summary>
    ''' 顯示單頭資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShipping()
        Dim oShipping As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.ShipmentDataTable
        Dim sShipmentID As String = Me.UI_lblPreviousPage_RMASMID.Text.ToString().Trim()

        dtShipping = oShipping.QueryShipmentByBossConfirm(sShipmentID)

        If dtShipping.Rows.Count > 0 Then
            Dim dr As RmaDTO.ShipmentRow = dtShipping.Rows(0)

            Me.UI_lblNoticeText.Text = dr.RMASM_PACKINGNO.ToString().Trim()
            Me.UI_lblDateText.Text = Convert.ToDateTime(dr.RMASM_CSTMP.ToString.Trim()).ToShortDateString()
            Me.UI_lblCustomerName.Text = dr.CU_NAME.ToString().Trim()
            Me.UI_lblCustomerID.Text = dr.RMASM_CUNO.ToString().Trim()
            Me.UI_lblCurrencyCode.Text = dr.RMARSM_CURRENCYCODE.ToString().Trim()
            Me.UI_lblCurrencyRate.Text = dr.RMARSM_CURRENCYRATE.ToString().Trim()
            If dr.RMASM_ISSHIP.ToString().Trim() = "1" Then
                Me.UI_lblShippingOrdersText.Text = _oLanguage.getText("RMA", "065", ctlLanguage.eumType.Tag)
            Else
                Me.UI_lblShippingOrdersText.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            End If

            If dr.IsRMASM_SHIPNONull = False Then Me.UI_lblShippingNumberText.Text = dr.RMASM_SHIPNO.ToString().Trim()
            If dr.IsRMASM_SHIPMEMONull = False Then Me.UI_lblMemoText.Text = dr.RMASM_SHIPMEMO.ToString().Trim()

            Call QueryData_RMAShippingDetail()  'show表身

        Else
            Me.UI_tbShipmentNotice.Visible = False

            Dim sMessage As String = _oLanguage.getText("RMA", "205", ctlLanguage.eumType.Tag)
            Me.ucMessage.showMessageBySuccess(sMessage, ascx_ucMessage.eumTransferURL.Redirect, "BossDefault.aspx")

        End If

    End Sub

    ''' <summary>
    ''' 顯示單身資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_RMAShippingDetail()
        Dim oShipment As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
        Dim sShipmentID As String = Me.UI_lblPreviousPage_RMASMID.Text.ToString().Trim()

        dtShipping = oShipment.QueryByRMA_ShipmentDetail(sShipmentID)

        Call Shipping_DataBind(dtShipping)
    End Sub

    Private Sub Shipping_DataBind(ByVal dtShipping As RmaDTO.Shipment_DetailDataTable)
        Session("_dtRMAShipping") = dtShipping
        Me.UI_dvShipping.ShowFooter = False

        Dim dvShipping As DataView = dtShipping.DefaultView()
        If dvShipping.Count > 0 Then
            Me.UI_dvShipping.ShowFooter = True
        End If

        Me.UI_dvShipping.DataSource = dtShipping.DefaultView()
        Me.UI_dvShipping.DataBind()
    End Sub

    Protected Sub UI_dvShipping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles UI_dvShipping.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHNo As Label = e.Item.FindControl("lblHNo")
            Dim lblHRMA As Label = e.Item.FindControl("lblHRMA")
            Dim lblHSerial As Label = e.Item.FindControl("lblHSerial")
            Dim lblHModel As Label = e.Item.FindControl("lblHModel")
            Dim lblHLabor As Label = e.Item.FindControl("lblHLabor")
            Dim lblHMaterial As Label = e.Item.FindControl("lblHMaterial")
            Dim lblHAmount As Label = e.Item.FindControl("lblHAmount")
            Dim lblHDetail As Label = e.Item.FindControl("lblHDetail")

            lblHNo.Text = _oLanguage.getText("RMA", "066", ctlLanguage.eumType.Tag)
            lblHRMA.Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
            lblHSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            lblHModel.Text = _oLanguage.getText("RMA", "020", ctlLanguage.eumType.Tag)
            lblHLabor.Text = _oLanguage.getText("RMA", "178", ctlLanguage.eumType.Tag)
            lblHMaterial.Text = _oLanguage.getText("RMA", "059", ctlLanguage.eumType.Tag)
            lblHAmount.Text = _oLanguage.getText("RMA", "180", ctlLanguage.eumType.Tag)
            lblHDetail.Text = _oLanguage.getText("RMA", "038", ctlLanguage.eumType.Tag)
        End If

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblSaleLabor As Label = e.Item.FindControl("lblSaleLabor")
            Dim lblSaleMaterial As Label = e.Item.FindControl("lblSaleMaterial")
            Dim lblSaleQuoted As Label = e.Item.FindControl("lblSaleQuoted")

            Dim lblLabor As Label = e.Item.FindControl("lblLabor")
            Dim lblQuoted As Label = e.Item.FindControl("lblQuoted")
            lblLabor.Text = _oLanguage.getText("RMA", "094", ctlLanguage.eumType.Tag)
            lblQuoted.Text = _oLanguage.getText("RMA", "179", ctlLanguage.eumType.Tag)

            '==========================================================================================================================================================
            '表尾的資料(總金額)
            '==========================================================================================================================================================
            If Me.UI_lblLaborTotal.Text.ToString().Trim() <> "" And lblSaleLabor.Text.ToString().Trim() <> "" Then
                Me.UI_lblLaborTotal.Text = Convert.ToDouble(Me.UI_lblLaborTotal.Text.ToString().Trim()) + Convert.ToDouble(lblSaleLabor.Text.ToString().Trim())
            End If
            If Me.UI_lblMaterialTotal.Text.ToString().Trim() <> "" And lblSaleMaterial.Text.ToString().Trim() <> "" Then
                Me.UI_lblMaterialTotal.Text = Convert.ToDouble(Me.UI_lblMaterialTotal.Text.ToString().Trim()) + Convert.ToDouble(lblSaleMaterial.Text.ToString().Trim())
            End If

            If Me.UI_lblQuotedTotal.Text.ToString().Trim() <> "" And lblSaleQuoted.Text.ToString().Trim() <> "" Then
                Me.UI_lblQuotedTotal.Text = Convert.ToDouble(Me.UI_lblQuotedTotal.Text.ToString().Trim()) + Convert.ToDouble(lblSaleQuoted.Text.ToString().Trim())

                Dim lblLOWESTDISCOUNT As Label = e.Item.FindControl("lblLOWESTDISCOUNT")
                Dim lblRepairQuoted As Label = e.Item.FindControl("lblRepairQuoted")

                Dim iLowestDisCount As Double = Convert.ToDouble(lblLOWESTDISCOUNT.Text.Trim)
                If iLowestDisCount > 0 Then
                    Dim iRepairQuoted As Double = Convert.ToDouble(lblRepairQuoted.Text.Trim())
                    Dim iDisCountAMT As Double = iRepairQuoted * (iLowestDisCount * 0.1)  '維修後折扣金額

                    If Convert.ToDouble(lblSaleQuoted.Text.Trim()) <= iDisCountAMT Then
                        Dim oTable As Table = e.Item.FindControl("oTableRow2")
                        Dim oTableRow0 As TableRow = oTable.Rows(0)
                        Dim oTableRow1 As TableRow = oTable.Rows(1)

                        oTableRow0.BackColor = Drawing.Color.LightSkyBlue
                        oTableRow1.BackColor = Drawing.Color.LightSkyBlue
                    End If
                End If
            End If
        End If

        If e.Item.ItemType = ListItemType.Footer Then
            Dim lblFLabor As Label = e.Item.FindControl("lblFLabor")
            Dim lblFMaterial As Label = e.Item.FindControl("lblFMaterial")
            Dim lblFAmount As Label = e.Item.FindControl("lblFAmount")
            Dim lblFLaborTotal As Label = e.Item.FindControl("lblFLaborTotal")
            Dim lblFMaterialTotal As Label = e.Item.FindControl("lblFMaterialTotal")
            Dim lblFQuotedTotal As Label = e.Item.FindControl("lblFQuotedTotal")
            Dim lblFCurrnecyCode As Label = e.Item.FindControl("lblFCurrnecyCode")

            lblFLabor.Text = _oLanguage.getText("RMA", "178", ctlLanguage.eumType.Tag)
            lblFMaterial.Text = _oLanguage.getText("RMA", "059", ctlLanguage.eumType.Tag)
            lblFAmount.Text = _oLanguage.getText("RMA", "088", ctlLanguage.eumType.Tag)

            lblFLaborTotal.Text = Convert.ToDecimal(Me.UI_lblLaborTotal.Text.ToString().Trim()).ToString("N")
            lblFMaterialTotal.Text = Convert.ToDecimal(Me.UI_lblMaterialTotal.Text.ToString().Trim()).ToString("N")
            lblFQuotedTotal.Text = Convert.ToDecimal(Me.UI_lblQuotedTotal.Text.ToString().Trim()).ToString("N")
            lblFCurrnecyCode.Text = Me.UI_lblCurrencyCode.Text.ToString().Trim()
        End If

    End Sub

    Protected Sub UI_dvShipping_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles UI_dvShipping.ItemCommand
        If e.CommandName = "cmdDetail" Then
            Dim lblRMANO As Label = e.Item.FindControl("lblRMANO")
            Dim lblRMADID As Label = e.Item.FindControl("lblRMADID")
            Me.ucRepairDetail.show(lblRMADID.Text.ToString().Trim(), lblRMANO.Text.Trim(), True)
        End If

    End Sub

    ''' <summary>
    ''' 確認送出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""

        Dim oShipping As New ctlRMA.Shipment
        Dim dtShipping As New RmaDTO.RMA_ShippingDataTable

        Try
            oShipping.Save_BossConfirm(Me.UI_lblPreviousPage_RMASMID.Text, Session("_UserID").ToString(), Session("_UserName").ToString())

            blnFlag = True

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            Else
                sMessage = oCommon.getMessage(Common.enmMessage.ConfirmOK)
                Me.ucMessage.showMessageBySuccess(sMessage, ascx_ucMessage.eumTransferURL.Redirect, "BossDefault.aspx")
            End If
        End Try


    End Sub

End Class

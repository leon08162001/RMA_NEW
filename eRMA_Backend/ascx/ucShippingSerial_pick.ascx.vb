Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ascx_ucShippingSerial_pick
    Inherits System.Web.UI.UserControl
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_show") = False
            Me.ViewState("_CuNo") = ""
            Me.ViewState("_CuName") = ""
            Me.ViewState("_CompNo") = ""

            Call setDefault()
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        Me.UI_lblShippingSerial.Text = _oLanguage.getText("RMA", "204", ctlLanguage.eumType.Tag)
        Me.UI_lblCtnNo.Text = _oLanguage.getText("RMA", "163", ctlLanguage.eumType.Tag)
        Me.UI_lblDescription.Text = _oLanguage.getText("RMA", "099", ctlLanguage.eumType.Tag)
        Me.UI_lblNetWeight.Text = _oLanguage.getText("RMA", "165", ctlLanguage.eumType.Tag)
        Me.UI_lblGrossWeigh.Text = _oLanguage.getText("RMA", "166", ctlLanguage.eumType.Tag)
        Me.UI_lblMeasurement.Text = _oLanguage.getText("RMA", "167", ctlLanguage.eumType.Tag)
        Me.UI_lblCustomer.Text = _oLanguage.getText("RMA", "030", ctlLanguage.eumType.Tag)

        Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub

    ''' <summary>
    ''' 群組勾選
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_CheckGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Me.UI_cmdSubmit.Enabled = False

        For i = 0 To Me.UI_dvShipping.Rows.Count - 1
            If Me.UI_dvShipping.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvShipping.Rows(i).FindControl("UI_Check")
                Dim UI_Mark As Label = Me.UI_dvShipping.Rows(i).FindControl("UI_Mark")
                UI_Check.Checked = sender.Checked

                If UI_Check.Checked = True And UI_Mark.Text.Trim() <> "9" Then
                    Me.UI_cmdSubmit.Enabled = True
                End If
            End If
        Next
        Me.ajModalProgress.Show()
    End Sub

    Protected Sub UI_check_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0
        Me.UI_cmdSubmit.Enabled = False

        For i = 0 To Me.UI_dvShipping.Rows.Count - 1
            If Me.UI_dvShipping.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvShipping.Rows(i).FindControl("UI_Check")

                If UI_Check.Checked = True Then
                    Me.UI_cmdSubmit.Enabled = True
                End If
            End If
        Next
        Me.ajModalProgress.Show()
    End Sub



    Private Sub QueryDataShipment(ByVal iPageIndex As Integer)
        Dim oShipping As New ctlRMA.Shipping
        Dim _dtShippingDetail As New RmaDTO.tmpShipping_DetailDataTable
        Dim dtShipping As New RmaDTO.Shipping_DetailDataTable
        Dim dtShipment As New RmaDTO.tmpShipment_DetailDataTable
        Dim dvShipping As DataView
      
        '已被選取到的資料
        If IsNothing(Session("_dtShippingDetail")) = False Then
            _dtShippingDetail = Session("_dtShippingDetail")
        End If

        dtShipping = oShipping.getQueryByShipment(Me.ViewState("_CuNo"))
        If dtShipping.Rows.Count > 0 Then
            '取業務已確認出貨的資料
            dtShipment = oShipping.QueryByShipment(Me.ViewState("_CuNo"))

            '組合資料填值
            dtShipping = getShipmentTable(dtShipping, dtShipment)

            '比對資料(排除已選取到的資料)
            dtShipping = getShipmentShow(dtShipping, _dtShippingDetail)

            '增加已刪除後的出貨單子
            dtShipping = getShipmentAdd(dtShipping, _dtShippingDetail, dtShipment)

            '無資料
            If Not dtShipping.Rows.Count > 0 Then
                dtShipping = AddSerial(dtShipping)
                dvShipping = dtShipping.DefaultView
                dvShipping.RowFilter = "oldMark = '9'"
            End If
        Else
            '無資料
            dtShipping = AddSerial(dtShipping)
            dvShipping = dtShipping.DefaultView
            dvShipping.RowFilter = "oldMark = '9'"
        End If


        Me.UI_dvShipping.PageSize = _PageSize
        Me.UI_dvShipping.PageIndex = iPageIndex
        Me.UI_dvShipping.DataSource = dtShipping
        Me.UI_dvShipping.DataBind()
    End Sub

    ''' <summary>
    ''' 組合資料填值
    ''' </summary>
    ''' <param name="dtShipping"></param>
    ''' <param name="dtShipment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getShipmentTable(ByVal dtShipping As RmaDTO.Shipping_DetailDataTable, ByVal dtShipment As RmaDTO.tmpShipment_DetailDataTable) As RmaDTO.Shipping_DetailDataTable
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim dvShipping As DataView = dtShipping.DefaultView
        Dim dvShipment As DataView = dtShipment.DefaultView

        For i = 0 To dvShipping.Count - 1
            Dim PackingNo As String = dvShipping.Item(i)("RMASM_PACKINGNO").ToString().Trim()
            Dim RMANo As String = dvShipping.Item(i)("RMASMD_RMANO").ToString().Trim()
            Dim ShipNo As String = ""
            Dim SerialNo As String = ""
            Dim iTagCount As Integer = 0

            dvShipment.RowFilter = "RMASM_PACKINGNO='" & PackingNo.Trim() & "' AND RMASMD_RMANO='" & RMANo.Trim() & "'"
            For j = 0 To dvShipment.Count - 1
                iTagCount = iTagCount + 1

                '是否隨貨:0.否, 1.是
                If dvShipment.Item(j)("RMASM_ISSHIP").ToString() = "1" Then
                    ShipNo = dvShipment.Item(j)("RMASM_SHIPNO").ToString()
                End If

                'SerialNo
                If SerialNo.Trim <> "" Then
                    If iTagCount = 10 Then
                        iTagCount = 0
                        SerialNo = SerialNo & ",<br>"
                    Else
                        SerialNo = SerialNo & ","
                    End If
                End If

                SerialNo = SerialNo & dvShipment.Item(j)("RMASMD_SERIALNO").ToString().Trim()
            Next

            dvShipping.Item(i)("RMASM_SHIPNO") = ShipNo.Trim()
            dvShipping.Item(i)("RMASMD_SERIALNO") = SerialNo.Trim()
            dvShipping.Item(i)("oldMark") = "0"
            dvShipment.RowFilter = ""
        Next

        Return dtShipping
    End Function

    ''' <summary>
    ''' 比對資料(排除已選取到的資料)
    ''' </summary>
    ''' <param name="dtShipping"></param>
    ''' <param name="_dtShippingDetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getShipmentShow(ByVal dtShipping As RmaDTO.Shipping_DetailDataTable, ByVal _dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable) As RmaDTO.Shipping_DetailDataTable
        Dim i As Integer = 0
        Dim dvShipping As DataView = dtShipping.DefaultView
        Dim sWhere As String = ""

        For i = 0 To _dtShippingDetail.Rows.Count - 1
            Dim dr As RmaDTO.tmpShipping_DetailRow = _dtShippingDetail.Rows(i)
            Dim PackingNo As String = ""
            Dim RMANO As String = ""

            '組合排除已選取的資料
            If dr.IsRMASHD_RMANONull = False And dr.IsRMASHD_RMASMPACKINGNONull = False And dr.RMASHD_oldMark <> "2" Then
                If sWhere.Trim() <> "" Then
                    sWhere = sWhere & " AND "
                End If

                sWhere = sWhere & "( "
                sWhere = sWhere & " RMASM_PACKINGNO <> '" & dr.RMASHD_RMASMPACKINGNO.ToString().Trim() & "'"
                sWhere = sWhere & " OR RMASMD_RMANO <> '" & dr.RMASHD_RMANO.ToString().Trim() & "'"
                sWhere = sWhere & " )"
            End If
        Next

        dvShipping.RowFilter = sWhere.Trim()

        Return dtShipping
    End Function

    ''' <summary>
    ''' 增加已刪除後的出貨單子
    ''' </summary>
    ''' <param name="dtShipping"></param>
    ''' <param name="_dtShippingDetail"></param>
    ''' <param name="dtShipment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getShipmentAdd(ByVal dtShipping As RmaDTO.Shipping_DetailDataTable, ByVal _dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal dtShipment As RmaDTO.tmpShipment_DetailDataTable) As RmaDTO.Shipping_DetailDataTable
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim oShipping As New ctlRMA.Shipping
        Dim dtShippingDetail As New RmaDTO.Shipping_DetailDataTable
        Dim dvShipment As DataView = dtShipment.DefaultView
        Dim _dvShippingDetail As DataView = _dtShippingDetail.DefaultView()

        _dvShippingDetail.RowFilter = "RMASHD_oldMark ='2'"

        For i = 0 To _dvShippingDetail.Count - 1
            Dim _dr As RmaDTO.tmpShipping_DetailRow = _dvShippingDetail(i).Row
            Dim PACKINGNO As String = _dr.RMASHD_RMASMPACKINGNO.ToString().Trim()
            Dim RMANO As String = _dr.RMASHD_RMANO.ToString().Trim()
            Dim ShipNo As String = ""
            Dim SerialNo As String = ""

            '挑選已出貨的單子
            dtShippingDetail = oShipping.getQueryByShipping(PACKINGNO.Trim(), RMANO.Trim())
            If dtShippingDetail.Count > 0 Then
                Dim drShippingDetail As RmaDTO.Shipping_DetailRow = dtShippingDetail.Rows(0)
                Dim drAdd As RmaDTO.Shipping_DetailRow = dtShipping.NewShipping_DetailRow

                dvShipment.RowFilter = "RMASM_PACKINGNO='" & PACKINGNO.Trim() & "' AND RMASMD_RMANO='" & RMANO.Trim() & "'"
                For j = 0 To dvShipment.Count - 1
                    '是否隨貨:0.否, 1.是
                    If dvShipment.Item(j)("RMASM_ISSHIP").ToString() = "1" Then
                        ShipNo = dvShipment.Item(j)("RMASM_SHIPNO").ToString()
                    End If

                    'SerialNo
                    If SerialNo.Trim <> "" Then
                        SerialNo = SerialNo & ","
                    End If
                    SerialNo = SerialNo & dvShipment.Item(j)("RMASMD_SERIALNO").ToString().Trim()
                Next

                drAdd.RMASM_PACKINGNO = PACKINGNO.ToString().Trim()
                drAdd.RMASMD_RMANO = RMANO.ToString().Trim()
                drAdd.Qty = drShippingDetail.Qty.ToString().Trim()
                drAdd.RMASM_SHIPNO = ShipNo.ToString().Trim()
                drAdd.RMASMD_SERIALNO = SerialNo.ToString().Trim()
                drAdd.oldMark = "1"

                dtShipping.AddShipping_DetailRow(drAdd)
                dvShipment.RowFilter = ""
            End If

        Next

        _dvShippingDetail.RowFilter = ""
        Return dtShipping
    End Function

    Protected Sub UI_dvShipping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvShipping.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "145", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "189", ctlLanguage.eumType.Tag)
            e.Row.Cells(4).Text = _oLanguage.getText("RMA", "190", ctlLanguage.eumType.Tag)
            e.Row.Cells(5).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_Mark As Label = e.Row.FindControl("UI_Mark")
            If UI_Mark.Text.Trim = "9" Then
                e.Row.Visible = False
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

    Protected Sub UI_dvShipping_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvShipping.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryDataShipment(iPageIndex)
        Me.ajModalProgress.Show()
    End Sub

    ''' <summary>
    ''' 無資料時建立一筆虛擬資料,作用是秀出表頭
    ''' </summary>
    ''' <param name="dtShipping"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial(ByVal dtShipping As RmaDTO.Shipping_DetailDataTable) As RmaDTO.Shipping_DetailDataTable
        Dim drShipping As RmaDTO.Shipping_DetailRow = dtShipping.NewShipping_DetailRow
        Try
            drShipping.oldMark = "9"

            dtShipping.AddShipping_DetailRow(drShipping)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtShipping
    End Function





    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim dtShippingDetail As New RmaDTO.tmpShipping_DetailDataTable
        Dim dvShippingDetail As DataView
        Dim CtnNo As String = Me.UI_txtCtnNo.Text.ToString().Trim()
        Dim Description As String = Me.UI_txtDescription.Text.ToString().Trim()
        Dim Netweight As String = Me.UI_txtNetWeight.Text.ToString().Trim()
        Dim Grossweigh As String = Me.UI_txtGrossWeigh.Text.ToString().Trim()
        Dim Measurement As String = Me.UI_txtMeasurement.Text.ToString().Trim()

        '已被選取到的資料
        If IsNothing(Session("_dtShippingDetail")) = False Then
            dtShippingDetail = Session("_dtShippingDetail")
        End If
        dvShippingDetail = dtShippingDetail.DefaultView()

        For i = 0 To Me.UI_dvShipping.Rows.Count - 1
            Dim oCheck As CheckBox = Me.UI_dvShipping.Rows(i).FindControl("UI_Check")
            Dim PACKINGNO As Label = Me.UI_dvShipping.Rows(i).FindControl("UI_PACKINGNO")
            Dim RMANO As Label = Me.UI_dvShipping.Rows(i).FindControl("UI_RMANO")
            Dim QUANTITY As Label = Me.UI_dvShipping.Rows(i).FindControl("UI_QUANTITY")
            Dim UI_Mark As Label = Me.UI_dvShipping.Rows(i).FindControl("UI_Mark")

            If oCheck.Checked = True Then
                If UI_Mark.Text = "1" Then
                    '修改原出貨資料已被刪除的值,並修正為已被選取資料
                    dvShippingDetail.RowFilter = "RMASHD_RMASMPACKINGNO='" & PACKINGNO.Text.ToString().Trim() & "' AND RMASHD_RMANO='" & RMANO.Text.ToString().Trim() & "'"
                    If dvShippingDetail.Count > 0 Then
                        dvShippingDetail(0)("RMASHD_CTNNO") = CtnNo.Trim()
                        dvShippingDetail(0)("RMASHD_DESCRIPTION") = Description.Trim()
                        dvShippingDetail(0)("RMASHD_QUANTITY") = Convert.ToDecimal(QUANTITY.Text.ToString().Trim())
                        dvShippingDetail(0)("RMASHD_NETWEIGHT") = Convert.ToDecimal(Netweight.Trim())
                        dvShippingDetail(0)("RMASHD_GROSSWEIGH") = Grossweigh.Trim()
                        dvShippingDetail(0)("RMASHD_MEASUREMENT") = Measurement.Trim()
                        dvShippingDetail(0)("RMASHD_oldMark") = "1"
                    End If
                    dvShippingDetail.RowFilter = ""
                Else
                    Dim dr As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.NewtmpShipping_DetailRow
                    Dim oGuid As Guid = Guid.NewGuid

                    dr.RMASHD_ID = oGuid.ToString()
                    dr.RMASHD_RMASHID = oGuid.ToString()
                    dr.RMASHD_CTNNO = CtnNo.Trim()
                    dr.RMASHD_DESCRIPTION = Description.Trim()
                    dr.RMASHD_SHIPMENTNO = PACKINGNO.Text.ToString().Trim()
                    dr.RMASHD_QUANTITY = Convert.ToDecimal(QUANTITY.Text.ToString().Trim())
                    dr.RMASHD_NETWEIGHT = Convert.ToDecimal(Netweight.Trim())
                    dr.RMASHD_GROSSWEIGH = Grossweigh.Trim()
                    dr.RMASHD_MEASUREMENT = Measurement.Trim()
                    dr.RMASHD_AD = Session("_UserID")
                    dr.RMASHD_ADNAME = Session("_UserName")
                    dr.RMASHD_CSTMP = Date.Now
                    dr.RMASHD_LUAD = Session("_UserID")
                    dr.RMASHD_LUADNAME = Session("_UserName")
                    dr.RMASHD_LUSTMP = Date.Now
                    dr.RMASHD_RMANO = RMANO.Text.ToString().Trim()
                    dr.RMASHD_RMASMPACKINGNO = PACKINGNO.Text.ToString().Trim()

                    If UI_Mark.Text = "1" Then
                        dr.RMASHD_oldMark = "1"
                    Else
                        dr.RMASHD_oldMark = "0"
                    End If


                    dtShippingDetail.AddtmpShipping_DetailRow(dr)
                End If

            End If
        Next

        '控制按鈕功能
        Call setButton(dtShippingDetail)

        Session("_dtShippingDetail") = dtShippingDetail
        Dim UI_dvShippingList As GridView = Me.Parent.FindControl("UI_dvShippingList")
        UI_dvShippingList.DataSource = dtShippingDetail.DefaultView
        UI_dvShippingList.DataBind()
    End Sub

    ''' <summary>
    ''' 控制按鈕功能
    ''' </summary>
    ''' <param name="dtShippingDetail"></param>
    ''' <remarks></remarks>
    Private Sub setButton(ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
        Dim UI_cmdPrint As Button = Me.Parent.FindControl("UI_cmdPrint")
        Dim UI_cmdSave As Button = Me.Parent.FindControl("UI_cmdSave")
        Dim UI_cmdSubmit As Button = Me.Parent.FindControl("UI_cmdSubmit")
        UI_cmdPrint.Enabled = False
        UI_cmdSave.Enabled = False
        UI_cmdSubmit.Enabled = False

        Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView()
        dvShippingDetail.RowFilter = "RMASHD_oldMark='0' OR RMASHD_oldMark='1'"
        If dvShippingDetail.Count > 0 Then
            UI_cmdPrint.Enabled = True
            UI_cmdSave.Enabled = True
            UI_cmdSubmit.Enabled = True
        End If

    End Sub






    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="CuNo">客戶代號</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="CompNo">客戶ID</param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal CuNo As String, ByVal CuName As String, ByVal CompNo As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Call ClearValue()

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.ViewState("_CuNo") = CuNo.Trim()
            Me.ViewState("_CuName") = CuName.Trim()
            Me.ViewState("_CompNo") = CompNo.Trim()

            Me.UI_txtCustomer.Text = CuName.Trim()
            Call QueryDataShipment(0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

    ''' <summary>
    ''' 清畫面的欄位值
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearValue()
        Me.ViewState("_CuNo") = ""
        Me.ViewState("_CuName") = ""
        Me.ViewState("_CompNo") = ""

        Me.UI_txtCustomer.Text = ""
        Me.UI_txtCtnNo.Text = ""
        Me.UI_txtDescription.Text = ""
        Me.UI_txtNetWeight.Text = "0"
        Me.UI_txtGrossWeigh.Text = ""
        Me.UI_txtMeasurement.Text = ""
    End Sub
  




   
End Class

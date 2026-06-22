Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ascx_ucRMASerial_Pick
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtSerial") = Nothing
            Call setDefault()
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblShippingSerial.Text = _oLanguage.getText("RMA", "177", ctlLanguage.eumType.Tag)

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

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
                Dim UI_Mark As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_Mark")
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

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            If Me.UI_dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")

                If UI_Check.Checked = True Then
                    Me.UI_cmdSubmit.Enabled = True
                End If
            End If
        Next
        Me.ajModalProgress.Show()
    End Sub




    Private Sub QueryDataSerial(ByVal iPageIndex As Integer)
        Dim oSerial As New ctlRMA.Shipment
        Dim _dtRMAShipping As New RmaDTO.Shipment_DetailDataTable
        Dim dtSerial As New RmaDTO.Shipment_DetailDataTable
        Dim sCuID As String = Me.UI_lblCuID.Text.ToString().Trim()
        Dim dvSerial As DataView
        Dim _dvRMAShipping As DataView

        '排除已選的資料()
        Dim i As Integer = 0
        Dim sRMADID As String = ""
        If IsNothing(Session("_dtRMAShipping")) = False Then
            _dtRMAShipping = Session("_dtRMAShipping")
            _dvRMAShipping = _dtRMAShipping.DefaultView
            For i = 0 To _dvRMAShipping.Count - 1
                If sRMADID.Trim <> "" Then
                    sRMADID = sRMADID & " AND "
                End If
                sRMADID = sRMADID & "RMASMD_RMADID <> '" & _dvRMAShipping.Item(i)("RMASMD_RMADID").ToString().Trim() & "'"
            Next
        End If


        dtSerial = oSerial.QueryCustomerByRMADetail(sCuID, Session("_UserID").ToString())
        If Not dtSerial.Rows.Count > 0 Then
            dtSerial = AddSerial(dtSerial)
            dvSerial = dtSerial.DefaultView
            dvSerial.RowFilter = "RMASMD_oldMark = '9'"
        Else
            If sRMADID.Trim() <> "" Then        '排除已選的資料
                dvSerial = dtSerial.DefaultView
                dvSerial.RowFilter = sRMADID.ToString().Trim()
            Else
                dvSerial = dtSerial.DefaultView
            End If
        End If

        Session("_dtSerial") = dtSerial             '將所有資料(dtSerial)暫存到_dtSerial(暫存資料)
        Me.UI_dvSerial.PageSize = _PageSize
        Me.UI_dvSerial.PageIndex = iPageIndex
        Me.UI_dvSerial.DataSource = dvSerial
        Me.UI_dvSerial.DataBind()
        dvSerial.RowFilter = ""
    End Sub

    Protected Sub UI_dvSerial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSerial.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "046", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
            e.Row.Cells(3).Text = _oLanguage.getText("RMA", "021", ctlLanguage.eumType.Tag)
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

    Protected Sub UI_dvSerial_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSerial.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryDataSerial(iPageIndex)
        Me.ajModalProgress.Show()
    End Sub

    ''' <summary>
    ''' 無資料時建立一筆虛擬資料,作用是秀出表頭
    ''' </summary>
    ''' <param name="dtSerial"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial(ByVal dtSerial As RmaDTO.Shipment_DetailDataTable) As RmaDTO.Shipment_DetailDataTable
        Dim drSerial As RmaDTO.Shipment_DetailRow = dtSerial.NewShipment_DetailRow
        Dim oGuid As Guid = Guid.NewGuid
        Try
            drSerial.RMASMD_oldMark = "9"
            drSerial.RMASMD_LOWESTDISCOUNT = 0

            dtSerial.AddShipment_DetailRow(drSerial)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtSerial
    End Function





    ''' <summary>
    ''' Submit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim dtSerial As New RmaDTO.Shipment_DetailDataTable

        Dim _dtSerial As RmaDTO.Shipment_DetailDataTable = Session("_dtSerial")
        Dim _dvSerial As DataView = _dtSerial.DefaultView
        Dim _dtRMAShipping As New RmaDTO.Shipment_DetailDataTable
        Dim _dvRMAShipping As DataView

        If IsNothing(Session("_dtRMAShipping")) = False Then
            _dtRMAShipping = Session("_dtRMAShipping")
        End If
        _dvRMAShipping = _dtRMAShipping.DefaultView

        For i = 0 To Me.UI_dvSerial.Rows.Count - 1
            Dim UI_Check As CheckBox = Me.UI_dvSerial.Rows(i).FindControl("UI_Check")
            Dim UI_RMADID As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_RMADID")
            Dim UI_Mark As Label = Me.UI_dvSerial.Rows(i).FindControl("UI_Mark")      '旗標,9:虛擬資料

            '選取到的資料
            If UI_Check.Checked = True And UI_Mark.Text.ToString().Trim() <> "9" Then

                _dvSerial.RowFilter = "RMASMD_RMADID='" & UI_RMADID.Text.ToString().Trim() & "'"
                If _dvSerial.Count > 0 Then
                    '檢查選取到的資料是否重複(_dvRMAShipping)
                    _dvRMAShipping.RowFilter = "RMASMD_RMADID='" & UI_RMADID.Text.ToString().Trim() & "'"
                    If _dvRMAShipping.Count > 0 Then
                        Dim sMark As String = _dvRMAShipping.Item(0)("RMASMD_oldMark").ToString().Trim()
                        If sMark.Trim() = "2" Then           '將被刪除狀態(2)改為修改狀態(1)
                            _dvRMAShipping(0)("RMASMD_oldMark") = "1"
                        End If
                    Else
                        If IsNothing(Session("_dtRMAShipping")) = True Then
                            dtSerial = AddSerial(_dvSerial, dtSerial)
                        Else
                            dtSerial = Session("_dtRMAShipping")
                            dtSerial = AddSerial(_dvSerial, dtSerial)
                        End If
                    End If
                End If
                _dvRMAShipping.RowFilter = ""
                _dvSerial.RowFilter = ""
            End If
        Next

        Dim UI_dvShipping As DataList = Me.Parent.FindControl("UI_dvShipping")
        Dim UI_cmdSave As Button = Me.Parent.FindControl("UI_cmdSave")
        Dim UI_cmdSubmit As Button = Me.Parent.FindControl("UI_cmdSubmit")
        Dim UI_cmdPrint As Button = Me.Parent.FindControl("UI_cmdPrint")
        Dim UI_cmdCustomerSearch As Button = Me.Parent.FindControl("UI_cmdCustomerSearch")
        Dim UI_txtCustomer As TextBox = Me.Parent.FindControl("UI_txtCustomer")
        UI_dvShipping.ShowFooter = False
        UI_cmdSave.Enabled = False
        UI_cmdSubmit.Enabled = False
        UI_cmdPrint.Enabled = False
        UI_cmdCustomerSearch.Enabled = True
        UI_txtCustomer.Enabled = True

        '如果沒新增任何一筆資料,就取Session("_dtRMAShipping")的資料
        If Not dtSerial.Rows.Count > 0 Then
            If IsNothing(Session("_dtRMAShipping")) = False Then
                dtSerial = Session("_dtRMAShipping")
            End If
        End If

        If dtSerial.Rows.Count > 0 Then
            '至少選取一筆資料才秀出功能鍵
            UI_dvShipping.ShowFooter = True
            UI_cmdSave.Enabled = True
            UI_cmdSubmit.Enabled = True
            UI_cmdPrint.Enabled = True
            UI_cmdCustomerSearch.Enabled = False
            UI_txtCustomer.Enabled = False
        End If

        Session("_dtRMAShipping") = dtSerial
        UI_dvShipping.DataSource = dtSerial
        UI_dvShipping.DataBind()
    End Sub

    ''' <summary>
    ''' 新增選取的資料到dtSerial
    ''' </summary>
    ''' <param name="_dvSerial"></param>
    ''' <param name="dtSerial"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddSerial(ByVal _dvSerial As DataView, ByVal dtSerial As RmaDTO.Shipment_DetailDataTable) As RmaDTO.Shipment_DetailDataTable
        Dim drSerial As RmaDTO.Shipment_DetailRow = dtSerial.NewShipment_DetailRow

        Try
            drSerial.RMASMD_RMASMID = _dvSerial.Item(0)("RMASMD_RMASMID").ToString().Trim()
            drSerial.RMASMD_ID = _dvSerial.Item(0)("RMASMD_ID").ToString().Trim()
            drSerial.RMASMD_RMANO = _dvSerial.Item(0)("RMASMD_RMANO").ToString().Trim()
            drSerial.RMASMD_RMADID = _dvSerial.Item(0)("RMASMD_RMADID").ToString().Trim()
            drSerial.RMASMD_SERIALNO = _dvSerial.Item(0)("RMASMD_SERIALNO").ToString().Trim()
            drSerial.RMASMD_PARTNO = _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim()

            If _dvSerial.Item(0)("RMASMD_MODELNO").ToString().Trim() <> "" Then
                drSerial.RMASMD_MODELNO = _dvSerial.Item(0)("RMASMD_MODELNO").ToString().Trim()
            End If
            
            If _dvSerial.Item(0)("RMARSD_LABORCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_LABORCOST = _dvSerial.Item(0)("RMARSD_LABORCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_MATERIALCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_MATERIALCOST = _dvSerial.Item(0)("RMARSD_MATERIALCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_QUOTE").ToString().Trim() <> "" Then
                drSerial.RMARSD_QUOTE = _dvSerial.Item(0)("RMARSD_QUOTE").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim() <> "" Then
                drSerial.RMASMD_PARTNO = _dvSerial.Item(0)("RMASMD_PARTNO").ToString().Trim()
            End If

            If _dvSerial.Item(0)("RMARSD_CURRENCYCODE").ToString().Trim() <> "" Then
                drSerial.RMARSD_CURRENCYCODE = _dvSerial.Item(0)("RMARSD_CURRENCYCODE").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_CURRENCYRATE").ToString().Trim() <> "" Then
                drSerial.RMARSD_CURRENCYRATE = _dvSerial.Item(0)("RMARSD_CURRENCYRATE").ToString().Trim()
            End If


            If _dvSerial.Item(0)("RMASMD_oldRMAID").ToString().Trim() <> "" Then
                drSerial.RMASMD_oldRMAID = _dvSerial.Item(0)("RMASMD_oldRMAID").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldLABORCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldLABORCOST = _dvSerial.Item(0)("RMARSD_oldLABORCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldMATERIALCOST").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldMATERIALCOST = _dvSerial.Item(0)("RMARSD_oldMATERIALCOST").ToString().Trim()
            End If
            If _dvSerial.Item(0)("RMARSD_oldQUOTE").ToString().Trim() <> "" Then
                drSerial.RMARSD_oldQUOTE = _dvSerial.Item(0)("RMARSD_oldQUOTE").ToString().Trim()
            End If

            drSerial.RMASMD_LOWESTDISCOUNT = _dvSerial.Item(0)("RMASMD_LOWESTDISCOUNT").ToString().Trim()


            drSerial.RMASMD_oldMark = "0"

            dtSerial.AddShipment_DetailRow(drSerial)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtSerial
    End Function







    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="sCuID"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sCuID As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow
        Session("_dtSerial") = Nothing

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.UI_lblCuID.Text = sCuID.Trim()
            Call QueryDataSerial(0)

            Me.ajModalProgress.Show()

        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub




End Class

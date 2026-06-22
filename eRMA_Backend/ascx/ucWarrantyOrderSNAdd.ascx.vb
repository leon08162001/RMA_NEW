Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
Imports NPOI.HSSF.UserModel

Partial Class ascx_ucWarrantyOrderSNAdd
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    'Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _PageSize As String = "15"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub

    ''' <summary>
    ''' 砞﹚北兜
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '眔Tag Text
        'Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        'Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub

    Private Sub QueryData(ByVal dtWarrantySerial As DataTable, ByVal iPageIndex As Integer)
        Me.dvSerial.PageSize = _PageSize
        Me.dvSerial.PageIndex = iPageIndex
        Me.dvSerial.DataSource = dtWarrantySerial.DefaultView
        Me.dvSerial.DataBind()
    End Sub

    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dvSerial.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSn As Label = e.Row.FindControl("lblSn")
            Dim lblwats_mark As Label = e.Row.FindControl("lblwats_mark")
            If lblwats_mark.Text.Trim().Equals("1") Then
                lblSn.Font.Bold = True
                lblSn.ForeColor = Drawing.Color.Red
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

    Protected Sub UI_dvAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dvSerial.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Dim dtWarrantySerial As DataTable = Me.ViewState("_dtWarrantySerial")
        Call QueryData(dtWarrantySerial, iPageIndex)
        Me.ajModalProgress.Show()
    End Sub
    Protected Sub btnReload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReload.Click
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        lblMsg.Text = ""
        Try
            Dim oWarranty As New ctlWarranty
            oWarranty.ReloadWarrantyItemSN(lblWoNo.Text, Convert.ToInt16(lblOrderSeq.Text))
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                lblMsg.Text = sMessage
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                lblMsg.Text = sMsg
            End If
        End Try

        show(True, lblWoNo.Text, Convert.ToInt32(lblOrderSeq.Text), ViewState("_wats_qty"))
    End Sub

    Protected Sub btnSnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSnSave.Click
        Dim sMessage As String = ""
        lblMsg.Text = ""
        Dim blnFlag As Boolean = False
        Dim oWarranty As New ctlWarranty
        Dim dtWarrantySN As DataTable = Me.ViewState("_dtWarrantySerial")
        Try
            If txtStart.Text.Trim() = "" Then
                Throw New Exception("Serial Number Can Not Empty!")
            End If

            If txtStart.Text <> "" Then
                If txtEnd.Text <> "" Then
                    Dim sStartBarcodeNo As String = txtStart.Text
                    Dim sEndBarcodeNo As String = txtEnd.Text
                    Dim nNum As Integer = 0
                    Dim nStart As Integer = 0
                    Dim nEnd As Integer = 0
                    Dim sFormat As String = ""

                    For i As Integer = 0 To sStartBarcodeNo.Length - 1
                        If Not sStartBarcodeNo.Substring(i, 1).Equals(sEndBarcodeNo.Substring(i, 1).ToString()) Then
                            nNum = i
                            Exit For
                        End If
                    Next
                    For i As Integer = 0 To (sStartBarcodeNo.Length - nNum) - 1
                        sFormat += "0"
                    Next
                    If nNum > 0 Then
                        Try
                            nStart = Integer.Parse(sStartBarcodeNo.Substring(nNum, sStartBarcodeNo.Length - nNum))
                            nEnd = Integer.Parse(sEndBarcodeNo.Substring(nNum, sStartBarcodeNo.Length - nNum))
                        Catch
                            Throw New Exception("SN End Err!")
                        End Try
                    End If

                    If ViewState("_wats_qty") > 0 Then
                        If dtWarrantySN.Rows.Count + 1 + (nEnd - nStart) - ViewState("_wats_qty") > 0 Then Throw New Exception("腹计秖璹虫计秖!")
                    End If

                    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                    For nI As Integer = nStart To nEnd
                        Dim sNextBarcodeNo As String = sStartBarcodeNo.Substring(0, nNum) & nI.ToString(sFormat)
                        Dim dr1 As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                        dr1.WATS_WATYNO = lblWoNo.Text
                        dr1.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                        dr1.WATS_SN = sNextBarcodeNo

                        dr1.WATS_AD = Session("_UserID")
                        dr1.WATS_ADNAME = Session("_UserName")
                        dr1.WATS_CSTMP = Date.Now
                        dr1.WATS_LUAD = Session("_UserID")
                        dr1.WATS_LUADNAME = Session("_UserName")
                        dr1.WATS_LUSTMP = Date.Now
                        dr1.WATS_MARK = 0
                        dtWarrantySerial.AddWARRANTYSERIALRow(dr1)
                    Next
                    oWarranty.SaveWarrantySerial(dtWarrantySerial)
                Else
                    If ViewState("_wats_qty") > 0 Then
                        If dtWarrantySN.Rows.Count + 1 - ViewState("_wats_qty") > 0 Then Throw New Exception("腹计秖璹虫计秖!")
                    End If

                    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                    Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                    dr.WATS_WATYNO = lblWoNo.Text
                    dr.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                    dr.WATS_SN = txtStart.Text

                    dr.WATS_AD = Session("_UserID")
                    dr.WATS_ADNAME = Session("_UserName")
                    dr.WATS_CSTMP = Date.Now
                    dr.WATS_LUAD = Session("_UserID")
                    dr.WATS_LUADNAME = Session("_UserName")
                    dr.WATS_LUSTMP = Date.Now
                    dr.WATS_MARK = 1
                    dtWarrantySerial.AddWARRANTYSERIALRow(dr)

                    oWarranty.SaveWarrantySerial(dtWarrantySerial)
                End If
            End If
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                lblMsg.Text = sMessage
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                lblMsg.Text = sMsg
            End If
        End Try
        UpdateParent()
        show(True, lblWoNo.Text, Convert.ToInt32(lblOrderSeq.Text), ViewState("_wats_qty"))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim sMessage As String = ""
        lblMsg.Text = ""
        Dim blnFlag As Boolean = False
        Dim bSelect As Boolean = False
        Dim oWarranty As New ctlWarranty
        Try
            Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable

            Dim i As Integer = 0
            For i = 0 To dvSerial.Rows.Count - 1
                If dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim chk As CheckBox = dvSerial.Rows(i).FindControl("chk")
                    Dim lblSn As Label = dvSerial.Rows(i).FindControl("lblSn")

                    If chk.Checked Then
                        bSelect = True
                        Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                        dr.WATS_WATYNO = lblWoNo.Text
                        dr.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                        dr.WATS_SN = lblSn.Text
                        dr.WATS_LUAD = Session("_UserID")
                        dr.WATS_LUADNAME = Session("_UserName")
                        dr.WATS_LUSTMP = Date.Now
                        dr.WATS_MARK = 1
                        dtWarrantySerial.AddWARRANTYSERIALRow(dr)
                    End If

                End If
            Next
		
            If Not bSelect Then
                Throw New Exception("Please select Serial Number first !")
            End If
            oWarranty.DeleteWarrantySerial(dtWarrantySerial)
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                lblMsg.Text = sMessage
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.DelOK)
                lblMsg.Text = sMsg
            End If
        End Try
        UpdateParent()
        show(True, lblWoNo.Text, Convert.ToInt32(lblOrderSeq.Text), ViewState("_wats_qty"))
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim sMessage As String = ""
        lblMsg.Text = ""
        Dim blnFlag As Boolean = False
        Dim bSelect As Boolean = False
        Dim oWarranty As New ctlWarranty
        Try
            Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable

            Dim i As Integer = 0
            For i = 0 To dvSerial.Rows.Count - 1
                If dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim chk As CheckBox = dvSerial.Rows(i).FindControl("chk")
                    Dim lblSn As Label = dvSerial.Rows(i).FindControl("lblSn")

                    If chk.Checked Then
                        bSelect = True
                        Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                        dr.WATS_WATYNO = lblWoNo.Text
                        dr.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                        dr.WATS_SN = lblSn.Text
                        dr.WATS_LUAD = Session("_UserID")
                        dr.WATS_LUADNAME = Session("_UserName")
                        dr.WATS_LUSTMP = Date.Now
                        dr.WATS_MARK = 0
                        dtWarrantySerial.AddWARRANTYSERIALRow(dr)
                    End If

                End If
            Next

            If Not bSelect Then
                Throw New Exception("Please select Serial Number first !")
            End If
            oWarranty.EditWarrantySerial(dtWarrantySerial)
            blnFlag = True
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally
            If blnFlag = False Then
                lblMsg.Text = sMessage
            Else
                Dim sMsg As String = ""
                sMsg = oCommon.getMessage(Common.enmMessage.DelOK)
                lblMsg.Text = sMsg
            End If
        End Try
        UpdateParent()
        show(True, lblWoNo.Text, Convert.ToInt32(lblOrderSeq.Text), ViewState("_wats_qty"))
    End Sub

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sMessage As String = ""
        Dim blnFlag As Boolean = False
        Try
            Dim i As Integer = 0
            Dim bCheck As Boolean = False
            Dim chkAll As CheckBox = dvSerial.HeaderRow.FindControl("chkAll")
            bCheck = chkAll.Checked

            For i = 0 To dvSerial.Rows.Count - 1
                If dvSerial.Rows(i).RowType = DataControlRowType.DataRow Then
                    Dim chk As CheckBox = dvSerial.Rows(i).FindControl("chk")
                    chk.Checked = bCheck
                End If
            Next
        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False
        Finally
            If blnFlag = False Then
                lblMsg.Text = sMessage
            End If
        End Try
        Me.ajModalProgress.Show()
    End Sub

    Public Sub UpdateParent()
        Dim dvWarrantyItem As GridView = Me.Parent.FindControl("dvWarrantyItem")
        Dim txtOrderSeq As TextBox = Me.Parent.FindControl("txtOrderSeq")
        Dim sOrderNo As String = lblWoNo.Text.ToString().Trim()
        If sOrderNo <> "" Then
            Dim oWarranty As New ctlWarranty
            Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
            dtData = oWarranty.QueryWarrantyItem(sOrderNo, -1, "")
            dvWarrantyItem.DataSource = dtData
            dvWarrantyItem.DataBind()
        End If
    End Sub

    Public Sub show(ByVal isShow As Boolean, ByVal wats_watyno As String, ByVal wats_watyseq As Integer, ByVal wats_qty As Integer)
        Me.ViewState("_show") = isShow
        Me.ViewState("_wats_qty") = wats_qty

        lblWoNo.Text = wats_watyno
        lblOrderSeq.Text = wats_watyseq

        txtStart.Text = ""
        txtEnd.Text = ""
        If wats_qty > 0 Then
            btnReload.Visible = True
            pnSerial.Visible = False
        Else
            btnReload.Visible = False
            pnSerial.Visible = True
        End If

        'lblMsg.Text = ""
        Dim oWarranty As New ctlWarranty
        Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
        dtWarrantySerial = oWarranty.QueryWarrantySerial(wats_watyno, wats_watyseq, "", "wats_sn")
        If dtWarrantySerial.Rows.Count > 0 Then
            btnDelete.Visible = True
        Else
            btnDelete.Visible = False
        End If

        Me.ViewState("_dtWarrantySerial") = dtWarrantySerial

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Call QueryData(dtWarrantySerial, 0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub
	
	
	'20240531 02:25 excel

    Protected Sub Excel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Excel_btn.Click

        Dim savePath As String = "D:\eRMANew\FILE\UpLoad\"

        If (FileUpload1.HasFile) Then
            Dim fileName As String = FileUpload1.FileName

            savePath = savePath & fileName
            FileUpload1.SaveAs(savePath)

            Dim fs = New FileStream(savePath, FileMode.Open, FileAccess.Read) 

            Label1.Text =  fileName
            Dim workbook As HSSFWorkbook = New HSSFWorkbook(fs)  
            Dim u_sheet As HSSFSheet = workbook.GetSheetAt(0)  
            Dim D_table As DataTable = New DataTable()
            Dim headerRow As HSSFRow = u_sheet.GetRow(0)  

            For k As Integer = headerRow.FirstCellNum To (headerRow.LastCellNum - 1)  
              
                If Not headerRow.GetCell(k) Is Nothing Then
                    Dim D_column As DataColumn = New DataColumn(headerRow.GetCell(k).StringCellValue)
                    D_table.Columns.Add(D_column)
                End If
            Next

            '矪瞶cell穦Τ戈拜肈 by buck modify 20251001 begin
            'For i As Integer = (u_sheet.FirstRowNum + 1) To u_sheet.LastRowNum   
            '    Dim row As HSSFRow = u_sheet.GetRow(i)  
            '    Dim D_dataRow As DataRow = D_table.NewRow()

            '    For j As Integer = row.FirstCellNum To (row.LastCellNum - 1)  
            '        If Not row.GetCell(j) Is Nothing Then
            '            D_dataRow(j) = row.GetCell(j).ToString() 
            '        End If
            '    Next
            '    D_table.Rows.Add(D_dataRow)
            'Next
            For i As Integer = (u_sheet.FirstRowNum + 1) To u_sheet.LastRowNum
                Dim row As HSSFRow = u_sheet.GetRow(i)
                If row Is Nothing Then
                    Continue For
                End If
                Dim D_dataRow As DataRow = D_table.NewRow()
                Dim hasValue As Boolean = False

                For j As Integer = row.FirstCellNum To (row.LastCellNum - 1)
                    Dim cell = row.GetCell(j)
                    If cell Is Nothing Then
                        Continue For
                    End If

                    Dim cellValue As Object = Nothing
                    Select Case cell.CellType
                        Case HSSFCellType.STRING
                            If String.IsNullOrWhiteSpace(cell.StringCellValue) Then Continue For
                            cellValue = cell.StringCellValue

                        Case HSSFCellType.NUMERIC
                            If HSSFDateUtil.IsCellDateFormatted(cell) Then
                                cellValue = cell.DateCellValue.ToString("yyyy/MM/dd")
                            Else
                                cellValue = cell.NumericCellValue
                            End If

                        Case HSSFCellType.BOOLEAN
                            cellValue = cell.BooleanCellValue

                        Case Else
                            If String.IsNullOrWhiteSpace(cell.ToString()) Then Continue For
                            cellValue = cell.ToString()
                    End Select

                    D_dataRow(j) = cellValue
                    hasValue = True
                Next

                If hasValue Then
                    D_table.Rows.Add(D_dataRow)
                End If
            Next
            '矪瞶cell穦Τ戈拜肈 by buck modify 20251001 end

            workbook = Nothing
            u_sheet = Nothing
           
            Dim D_View As DataView = New DataView(D_table)

            'GridView1.DataSource = D_View
            'GridView1.DataBind()
            Try

                For l = 0 To D_table.Rows.Count - 1

                 
               

                        txtStart.Text = D_table.Rows(l)(0).ToString().Trim()

                        Dim sMessage As String = ""
                        lblMsg.Text = ""
                        Dim blnFlag As Boolean = False
                        Dim oWarranty As New ctlWarranty
                        Dim dtWarrantySN As DataTable = Me.ViewState("_dtWarrantySerial")
                        Try
                            If txtStart.Text.Trim() = "" Then
                                Throw New Exception("Serial Number Can Not Empty!")
                            End If

                            If txtStart.Text <> "" Then
                                If txtEnd.Text <> "" Then
                                    Dim sStartBarcodeNo As String = txtStart.Text
                                    Dim sEndBarcodeNo As String = txtEnd.Text
                                    Dim nNum As Integer = 0
                                    Dim nStart As Integer = 0
                                    Dim nEnd As Integer = 0
                                    Dim sFormat As String = ""

                                    For i As Integer = 0 To sStartBarcodeNo.Length - 1
                                        If Not sStartBarcodeNo.Substring(i, 1).Equals(sEndBarcodeNo.Substring(i, 1).ToString()) Then
                                            nNum = i
                                            Exit For
                                        End If
                                    Next
                                    For i As Integer = 0 To (sStartBarcodeNo.Length - nNum) - 1
                                        sFormat += "0"
                                    Next
                                    If nNum > 0 Then
                                        Try
                                            nStart = Integer.Parse(sStartBarcodeNo.Substring(nNum, sStartBarcodeNo.Length - nNum))
                                            nEnd = Integer.Parse(sEndBarcodeNo.Substring(nNum, sStartBarcodeNo.Length - nNum))
                                        Catch
                                            Throw New Exception("SN End Err!")
                                        End Try
                                    End If

                                    If ViewState("_wats_qty") > 0 Then
                                        If dtWarrantySN.Rows.Count + 1 + (nEnd - nStart) - ViewState("_wats_qty") > 0 Then Throw New Exception("序?數量大於?單數量!")
                                    End If

                                    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                                    For nI As Integer = nStart To nEnd
                                        Dim sNextBarcodeNo As String = sStartBarcodeNo.Substring(0, nNum) & nI.ToString(sFormat)
                                        Dim dr1 As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                                        dr1.WATS_WATYNO = lblWoNo.Text
                                        dr1.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                                        dr1.WATS_SN = sNextBarcodeNo

                                        dr1.WATS_AD = Session("_UserID")
                                        dr1.WATS_ADNAME = Session("_UserName")
                                        dr1.WATS_CSTMP = Date.Now
                                        dr1.WATS_LUAD = Session("_UserID")
                                        dr1.WATS_LUADNAME = Session("_UserName")
                                        dr1.WATS_LUSTMP = Date.Now
                                        dr1.WATS_MARK = 0
                                        dtWarrantySerial.AddWARRANTYSERIALRow(dr1)
                                    Next
                                    oWarranty.SaveWarrantySerial(dtWarrantySerial)
                                Else
                                    If ViewState("_wats_qty") > 0 Then
                                        If dtWarrantySN.Rows.Count + 1 - ViewState("_wats_qty") > 0 Then Throw New Exception("序?數量大於?單數量!")
                                    End If

                                    Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
                                    Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.NewWARRANTYSERIALRow

                                    dr.WATS_WATYNO = lblWoNo.Text
                                    dr.wats_watyseq = Convert.ToInt32(lblOrderSeq.Text)
                                    dr.WATS_SN = txtStart.Text

                                    dr.WATS_AD = Session("_UserID")
                                    dr.WATS_ADNAME = Session("_UserName")
                                    dr.WATS_CSTMP = Date.Now
                                    dr.WATS_LUAD = Session("_UserID")
                                    dr.WATS_LUADNAME = Session("_UserName")
                                    dr.WATS_LUSTMP = Date.Now
                                    dr.WATS_MARK = 1
                                    dtWarrantySerial.AddWARRANTYSERIALRow(dr)

                                    oWarranty.SaveWarrantySerial(dtWarrantySerial)
                                End If
                            End If
                            blnFlag = True
                        Catch ex As Exception
                            sMessage = ex.Message
                            blnFlag = False

                        Finally
                            If blnFlag = False Then
                                lblMsg.Text = sMessage
                            Else
                                Dim sMsg As String = ""
                                sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                                lblMsg.Text = sMsg
                            End If
                        End Try
                        UpdateParent()
                        show(True, lblWoNo.Text, Convert.ToInt32(lblOrderSeq.Text), ViewState("_wats_qty"))

                 
        


                Next
            Catch ex As Exception

            Finally

            End Try
         
        End If

    End Sub
	
End Class

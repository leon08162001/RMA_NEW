Imports System.Data
Imports System.IO
Imports System.Xml
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage
Imports RMA_Common

Partial Class ProductInformation_04_01
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    Dim _Comms As New Commons
    Dim _oLanguage As New ctlLanguage
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")

    Dim _RedirectURL As String = "Client_Worklist.aspx"
    Dim _Address As String = ""
    Dim oWAR_TYPE As String = "" '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02
    Dim _ProgressSleepBySave As String = ConfigurationSettings.AppSettings("ProgressSleepBySave")
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")

    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_Upload_FilePath")
    Dim _Requested_ExcelSample_VisualPath As String = ConfigurationSettings.AppSettings("Requested_ExcelSample_VisualPath")
    Dim _Requested_Default_FarFarcNO As String = ConfigurationSettings.AppSettings("Requested_Default_FarFarcNO")
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _RepairEmailCC As String = ConfigurationSettings.AppSettings("RepairEmailCC")
    Dim _RequestNew_JP_BYTE_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_JP_BYTE_EmailCC")
    Dim _RequestNew_US_CL_MPLUS_EmailCC As String = ConfigurationSettings.AppSettings("RequestNew_US_CL_MPLUS_EmailCC")

    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Enum eumCommand
        AddNew = 1
        UPDATE = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True

            Response.Redirect("~/Default.aspx")
        End If

        If Me.IsPostBack = False Then


            Dim RMA_APPLICANT As String = ""
            Dim RMA_TEL As String = ""
            Dim RMA_ADDRESS As String = ""
            Dim RMA_MAIL As String = ""
            Dim RMA_REMARK As String = ""
            Dim ddlDiv_string As String = ""
            Dim ddlDiv_intdex As Integer = 0


            Try
                '序號明細帶入
                Dim dtTMP As New RmaDTO.RMADetailDataTable
                dtTMP = Session("_dtRMADetail")
                UI_dvRMADetail.DataSource = dtTMP
                UI_dvRMADetail.DataBind()

                'Dim myctAddress As New ctAddress

                'If Not Request.Params("RMANO") Is Nothing Then

                '    Dim _Crypto As New SecurityCrypt.Crypto
                '    Dim sRMANO As String = _Crypto.Decrypt(Request.Params("RMANO").ToString().Trim(), "")

                '    Dim mytable As New DataTable
                '    mytable = myctAddress.Select_RMA(sRMANO)

                '    For i = 0 To mytable.Rows.Count - 1

                '        RMA_APPLICANT = mytable.Rows(i)("RMA_APPLICANT").ToString().Trim()
                '        RMA_TEL = mytable.Rows(i)("RMA_TEL").ToString().Trim()
                '        RMA_ADDRESS = mytable.Rows(i)("RMA_ADDRESS").ToString().Trim()
                '        RMA_MAIL = mytable.Rows(i)("RMA_MAIL").ToString().Trim()
                '        RMA_REMARK = mytable.Rows(i)("RMA_REMARK").ToString().Trim()

                '        Dim RMA_ADDRESS_List As String() = RMA_ADDRESS.Split(" ")

                '        If RMA_ADDRESS_List.Length > 1 Then
                '            ddlDiv_string = RMA_ADDRESS.Split(" ")(1).ToString().Trim()
                '        End If

                '    Next

                'End If

                '修改部分
                If Not Request.Params("RMANO") Is Nothing Then
                    Dim _Crypto As New SecurityCrypt.Crypto
                    Dim RMANO As String = _Crypto.Decrypt(Request.Params("RMANO").ToString().Trim(), "")

                    Dim dtRMA As New RmaDTO.RMADataTable
                    Dim oRMA As New ctlRMA.Requested
                    dtRMA = oRMA.QueryByRMAHead(RMANO)
                    Me.UI_lblPreviousPage_RMAID.Text = dtRMA.Rows(0)("RMA_ID").ToString().Trim()
                    Me.UI_lblPreviousPage_RMANO.Text = RMANO
                End If

            Catch ex As ArgumentException

            End Try

            Try
                Call setControls()
                Call QueryData_Head()

                ddlDiv.Items.Clear()

                Dim saveDir As String = "\Uploads\"
                Dim appPath As String = Request.PhysicalApplicationPath

                Dim FileNames As String = ""

                Dim language_String As String = Session("_LanguageID")

                If language_String = "002" Then
                    FileNames = "global_cn.xml"
                Else
                    FileNames = "global_en.xml"
                End If


                Dim Path As String = appPath & saveDir & FileNames

                Dim xmldoc As New XmlDataDocument()
                Dim xmlnode As XmlNodeList
                Dim i As Integer
                Dim fs As New FileStream(Path, FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)
                xmlnode = xmldoc.GetElementsByTagName("CountryRegion")

                Dim List_First_xml As New ListItem
                List_First_xml.Text = "-" & _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag) & "-"
                List_First_xml.Value = "-Select-"
                ddlDiv.Items.Add(List_First_xml)

                For i = 0 To xmlnode.Count - 1



                    Dim xe As XmlElement = CType(xmlnode(i), XmlElement)
                    Dim List_xml As New ListItem
                    List_xml.Text = xe.GetAttribute("Name")
                    List_xml.Value = xe.GetAttribute("Code")

                    If ddlDiv_string = xe.GetAttribute("Name") Then
                        ddlDiv_intdex = i
                    End If


                    ddlDiv.Items.Add(List_xml)

                Next

                If Not Request.Params("RMANO") Is Nothing Then

                    Me.ddlDiv.SelectedIndex = ddlDiv_intdex + 1
                    Me.UI_txtApplicant.Text = RMA_APPLICANT
                    Me.UI_txtTel.Text = RMA_TEL
                    Me.UI_txtMail.Text = RMA_MAIL
                    Me.UI_txtRemark.Text = RMA_REMARK

                    Dim RMA_ADDRESS_List As String() = RMA_ADDRESS.Split(" ")

                    If RMA_ADDRESS_List.Length > 0 Then
                        Me.UI_txtCode.Text = RMA_ADDRESS.Split(" ")(0).ToString().Trim()
                    End If

                    If RMA_ADDRESS_List.Length > 2 Then
                        Me.UI_txtAddress.Text = RMA_ADDRESS.Split(" ")(2).ToString().Trim()
                    End If

                Else
                    Me.UI_txtApplicant.Text = ""
                    Me.UI_txtTel.Text = ""
                    Me.UI_txtMail.Text = ""
                    Me.UI_txtRemark.Text = ""
                    Me.UI_txtCode.Text = ""
                    Me.UI_txtAddress.Text = ""

                End If

            Catch ex As ArgumentException

            End Try
        End If

    End Sub

    Protected Sub ddlDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDiv.SelectedIndexChanged

        Try

            ddlVil.Items.Clear()
            ddlCityarea.Items.Clear()
            Dim index As Int32 = 0

            Dim saveDir As String = "\Uploads\"
            Dim appPath As String = Request.PhysicalApplicationPath

            Dim FileNames As String = ""

            Dim language_String As String = Session("_LanguageID")

            If language_String = "002" Then
                FileNames = "global_cn.xml"
            Else
                FileNames = "global_en.xml"
            End If

            Dim Path As String = appPath & saveDir & FileNames

            Dim xmldoc As New XmlDataDocument()
            Dim xmlnode As XmlNodeList
            Dim i As Integer
            Dim fs As New FileStream(Path, FileMode.Open, FileAccess.Read)
            xmldoc.Load(fs)
            xmlnode = xmldoc.GetElementsByTagName("CountryRegion")

            If xmlnode.Count > 0 Then
                Dim List_First_xml As New ListItem
                List_First_xml.Text = "-Select-"
                List_First_xml.Value = "-Select-"
                ddlVil.Items.Add(List_First_xml)
            End If

            For i = 0 To xmlnode.Count - 1

                Dim ddlVil_String As String = ddlDiv.SelectedItem.Value.ToString().Trim()

                Dim xe As XmlElement = CType(xmlnode(i), XmlElement)

                If ddlVil_String = xe.GetAttribute("Code") Then

                    Dim xnf1 As XmlNodeList = xmlnode(i).ChildNodes


                    Dim xn2 As XmlNode
                    For Each xn2 In xnf1

                        Dim xe_ As XmlElement = CType(xn2, XmlElement)

                        If xe_ Is Nothing Then

                        Else

                            Dim List_xml As New ListItem
                            List_xml.Text = xe_.GetAttribute("Name")
                            List_xml.Value = xe_.GetAttribute("Code")
                            ddlVil.Items.Add(List_xml)

                            If List_xml.Text = "" Then

                            Else
                                index = index + 1
                            End If

                        End If




                    Next xn2

                End If



            Next

            If index = 0 Then

                For i = 0 To xmlnode.Count - 1

                    Dim ddlVil_String As String = ddlDiv.SelectedItem.Value.ToString().Trim()

                    Dim xe As XmlElement = CType(xmlnode(i), XmlElement)

                    If ddlVil_String = xe.GetAttribute("Code") Then

                        Dim xnf1 As XmlNodeList = xmlnode(i).ChildNodes


                        Dim xn2 As XmlNode
                        For Each xn2 In xnf1

                            Dim ddlCityarea_String As String = ddlVil.SelectedItem.Value.ToString().Trim()
                            Dim xn2_xe_ As XmlElement = CType(xn2, XmlElement)


                            For Each xn3 In xn2

                                Dim xn3_xe_ As XmlElement = CType(xn3, XmlElement)

                                'Dim List_xml As New ListItem
                                'List_xml.Text = xn3_xe_.GetAttribute("Name")
                                'List_xml.Value = xn3_xe_.GetAttribute("Code")
                                'ddlCityarea.Items.Add(List_xml)

                            Next xn3




                        Next xn2

                    End If



                Next

            End If

            If index > 1 Then

            Else
                ddlVil.Items.Clear()
            End If

            '如果取不到值

        Catch ex As ArgumentException

        End Try

    End Sub

    Protected Sub ddlVil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVil.SelectedIndexChanged

        Try


            ddlCityarea.Items.Clear()

            Dim saveDir As String = "\Uploads\"
            Dim appPath As String = Request.PhysicalApplicationPath

            Dim FileNames As String = ""

            Dim language_String As String = Session("_LanguageID")

            If language_String = "002" Then
                FileNames = "global_cn.xml"
            Else
                FileNames = "global_en.xml"
            End If


            Dim Path As String = appPath & saveDir & FileNames

            Dim xmldoc As New XmlDataDocument()
            Dim xmlnode As XmlNodeList
            Dim i As Integer
            Dim fs As New FileStream(Path, FileMode.Open, FileAccess.Read)
            xmldoc.Load(fs)
            xmlnode = xmldoc.GetElementsByTagName("CountryRegion")

            For i = 0 To xmlnode.Count - 1

                Dim ddlVil_String As String = ddlDiv.SelectedItem.Value.ToString().Trim()

                Dim xe As XmlElement = CType(xmlnode(i), XmlElement)

                If ddlVil_String = xe.GetAttribute("Code") Then

                    Dim xnf1 As XmlNodeList = xmlnode(i).ChildNodes


                    Dim xn2 As XmlNode
                    For Each xn2 In xnf1

                        Dim ddlCityarea_String As String = ddlVil.SelectedItem.Value.ToString().Trim()
                        Dim xn2_xe_ As XmlElement = CType(xn2, XmlElement)

                        If ddlCityarea_String = xn2_xe_.GetAttribute("Code") Then

                            For Each xn3 In xn2

                                Dim xn3_xe_ As XmlElement = CType(xn3, XmlElement)

                                Dim List_xml As New ListItem
                                List_xml.Text = xn3_xe_.GetAttribute("Name")
                                List_xml.Value = xn3_xe_.GetAttribute("Code")
                                ddlCityarea.Items.Add(List_xml)

                            Next xn3

                        End If


                    Next xn2

                End If



            Next

        Catch ex As ArgumentException

        End Try

    End Sub

    Protected Sub SubmitBtn_Click(sender As Object, e As EventArgs) Handles SubmitBtn.Click
        'System.Threading.Thread.Sleep(Convert.ToInt32(_ProgressSleepBySave))

        If chkRMA() = True Then
            Dim iStatus As Integer = 10  '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Save(iStatus)
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()
        'Me.UI_cmdExcelFile.NavigateUrl = _WEBURL & _Requested_ExcelSample_VisualPath & "Sample.csv"

        'Dim sClientID As String = Me.UI_cmdPick.ClientID & "," & Me.UI_cmdAdd.ClientID
        'Me.ucProgressStatus.NotpostBackElement = sClientID

        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(False, Me.UI_cboRepairCenter, sRepairText)
        Dim oListItem As New ListItem
        Me.UI_cboRepairCenter.SelectedValue = "-1"
        oListItem = Me.UI_cboRepairCenter.SelectedItem
        Me.UI_cboRepairCenter.Items.Remove(oListItem)

        Me.BackBtn.Text = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)
        Me.SubmitBtn.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        Me.ErmaButtonBackBackBtn.Text = _oLanguage.getText("Common", "006", ctlLanguage.eumType.Command)

        '取得Tag Text
        'Me.UI_lblTittle.Text = _oLanguage.getText("RMA", "001", ctlLanguage.eumType.Tag)
        'Me.UI_lblClientInformation.Text = _oLanguage.getText("RMA", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountID.Text = _oLanguage.getText("RMA", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblAccountName.Text = _oLanguage.getText("RMA", "004", ctlLanguage.eumType.Tag)
        Me.UI_lblUserID.Text = _oLanguage.getText("RMA", "005", ctlLanguage.eumType.Tag)
        Me.UI_lblApplicant.Text = _oLanguage.getText("RMA", "006", ctlLanguage.eumType.Tag)
        Me.UI_lblTel.Text = _oLanguage.getText("RMA", "007", ctlLanguage.eumType.Tag)
        'Me.UI_lblAddress.Text = _oLanguage.getText("RMA", "008", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("RMA", "009", ctlLanguage.eumType.Tag)
        'Me.UI_lblProductInformation.Text = _oLanguage.getText("RMA", "010", ctlLanguage.eumType.Tag)
        'Me.UI_lblPleaseTittle.Text = _oLanguage.getText("RMA", "011", ctlLanguage.eumType.Tag)
        'Me.UI_lblModel.Text = _oLanguage.getText("RMA", "012", ctlLanguage.eumType.Tag)
        'Me.UI_lblSerial.Text = _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag)
        Me.UI_lblMail.Text = _oLanguage.getText("RMA", "044", ctlLanguage.eumType.Tag)
        'Me.UI_lblFile.Text = _oLanguage.getText("RMA", "024", ctlLanguage.eumType.Tag)

        'Me.UI_cmdFileAdd.Text = _oLanguage.getText("RMA", "211", ctlLanguage.eumType.Command)
        'Me.UI_cmdExcelFile.Text = _oLanguage.getText("RMA", "212", ctlLanguage.eumType.Tag)
        'Me.UI_UploadFileDesc.Text = _oLanguage.getText("RMA", "186", ctlLanguage.eumType.Tag)
        Me.UI_lblRemark.Text = _oLanguage.getText("RMA", "134", ctlLanguage.eumType.Tag)
        'Me.uiLblDesc01.Text = _oLanguage.getText("RMA", "411", ctlLanguage.eumType.Tag)


        'Me.rfv_txtTEL.ErrorMessage = _oLanguage.getText("RMA", "207", ctlLanguage.eumType.Validator)
        'Me.rfv_txtAdress.ErrorMessage = _oLanguage.getText("RMA", "047", ctlLanguage.eumType.Validator)
        'Me.rfv_txtApplicant.ErrorMessage = _oLanguage.getText("RMA", "048", ctlLanguage.eumType.Validator)
        ''Me.rfv_txtModelNo.ErrorMessage = _oLanguage.getText("RMA", "191", ctlLanguage.eumType.Validator)
        'Me.cvModelNo_Serial.ErrorMessage = _oLanguage.getText("RMA", "210", ctlLanguage.eumType.Validator)
        'Me.cvFileUpLoad.ErrorMessage = _oLanguage.getText("RMA", "219", ctlLanguage.eumType.Validator)
        'Me.rfv_txtAccountID.ErrorMessage = _oLanguage.getText("RMA", "220", ctlLanguage.eumType.Validator)
        'Me.revEMail_Empty.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)
        'Me.revEMail.ErrorMessage = _oLanguage.getText("RMA", "049", ctlLanguage.eumType.Validator)


        Me.UI_cmdCust_Search.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        'Me.UI_cmdAdressPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)

        'Me.UI_cmdPick.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        'Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        'Me.UI_cmdConfirm.Text = _oLanguage.getText("Common", "016", ctlLanguage.eumType.Command)
        'Me.UI_cmdTmpSave.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        'Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
        'Me.UI_cmdModify.Text = _oLanguage.getText("Common", "002", ctlLanguage.eumType.Command)
        'Me.UI_cmdCancel.Text = _oLanguage.getText("Common", "070", ctlLanguage.eumType.Command)
        'Me.UI_lblEUCompany.Text = _oLanguage.getText("RMA", "459", ctlLanguage.eumType.Tag)
        'Me.UI_lblEUTel.Text = _oLanguage.getText("RMA", "455", ctlLanguage.eumType.Tag)
        'Me.UI_lblEUName.Text = _oLanguage.getText("RMA", "456", ctlLanguage.eumType.Tag)
        'Me.UI_lblEUMail.Text = _oLanguage.getText("RMA", "457", ctlLanguage.eumType.Tag)
        'Me.UI_lblEUAddress.Text = _oLanguage.getText("RMA", "458", ctlLanguage.eumType.Tag)
        ''設定 Enter 鍵觸發
        'Me.UI_txtSerial.Attributes.Add("OnKeypress", "return clickButton(event,'" & Me.UI_cmdAdd.ClientID & "')")
        'Me.chkWarrantyMsg.Text = _oLanguage.getText("RMA", "209", ctlLanguage.eumType.Validator)
        'Me.lb_PartsRequest.Text = _oLanguage.getText("Transfer", "010", ctlLanguage.eumType.Word)

        If Session("_Comp_Admin").ToString().ToUpper() = "MPLUS" Then
            'Me.UI_lblModel.Visible = False
            'Me.UI_cboModel.Visible = False
        End If

        '20210708 wisely add 若是 JP_BYTE_MPLUS 不秀補料選項
        'If Session("_RepairID").ToString().Trim() = "JP_BYTE_MPLUS" Then
        '    lb_PartsRequest.Visible = False
        '    UI_PartsRequest.Visible = False
        'End If



        Me.UI_lblAccountIDText.Visible = True
        Me.UI_txtAccountIDText.Visible = False
        Me.UI_txtAccountIDText.ReadOnly = False
        Me.UI_cmdCust_Search.Visible = False
        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            Me.UI_lblAccountIDText.Visible = False

            Me.UI_txtAccountIDText.Visible = True
            Me.UI_txtAccountIDText.ReadOnly = True
            Me.UI_cmdCust_Search.Visible = True
        End If

        Me.UI_txtCountry.Attributes.Add("Placeholder", _oLanguage.getText("RMA2", "087", ctlLanguage.eumType.Tag))
        Me.UI_txtCode.Attributes.Add("Placeholder", _oLanguage.getText("RMA2", "083", ctlLanguage.eumType.Tag))
        Me.UI_txtAddress.Attributes.Add("Placeholder", _oLanguage.getText("Customer", "026", ctlLanguage.eumType.Tag))
        Me.ComprehensiveWarrantyLabel.Text = _oLanguage.getText("RMA2", "088", ctlLanguage.eumType.Tag)
        Me.UI_lblAddress.Text = _oLanguage.getText("Customer", "026", ctlLanguage.eumType.Tag)
        Me.lb_PartsRequest.Text = _oLanguage.getText("RMA2", "033", ctlLanguage.eumType.Tag)
    End Sub

    ''' <summary>
    ''' 新增 RMA單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QueryData_Head()
        Dim i As Integer = 0
        '' ''Dim oListItem As ListItem
        Dim dtAddress As New DataTable
        dtAddress.Columns.Add("CU_Address")


        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustome = oCustomer.QueryByCompany(Me.UI_txtAccountIDText.Text.Trim)
            End If
        Else
            dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
        End If

        Dim sTel As String = ""
        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)
            'Me.UI_lblAccountIDText.Text = dr.CU_NO.ToString().Trim()
            Me.UI_lblAccountIDText.Text = ""
            Me.UI_lblAccountNameText.Text = ""
            Me.UI_lblRepairCenterText.Text = dr.COMP_NAME.ToString().Trim()
            Me.UI_lblRepairCenterValue.Text = dr.COMP_NO.ToString().Trim()
            Me.UI_cboRepairCenter.SelectedValue = dr.COMP_NO.ToString().Trim()


            '資料 開始 20231229
            Me.UI_lblAccountNameText_HiddenField.Value = dr.CU_NO.ToString().Trim()
            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_txtApplicant_HiddenField.Value = dr.CU_CONTACTPERSON.ToString().Trim()
            '資料 結束 20231229

            Me.UI_lblRepairCenterText.Visible = False
            Me.UI_cboRepairCenter.Visible = False
            If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                Me.UI_cboRepairCenter.Visible = True
            Else
                If dr.CU_ISCHOICE = "1" Then
                    Me.UI_cboRepairCenter.Visible = True
                Else
                    Me.UI_lblRepairCenterText.Visible = True
                End If
            End If


            If dr.IsCU_TELNull = False Then sTel = dr.CU_TEL.ToString().Trim()
            If dr.IsCU_CONTACTPERSONNull = False Then Me.UI_txtApplicant.Text = dr.CU_CONTACTPERSON.ToString().Trim()
            If dr.IsCU_EMAILNull = False Then Me.UI_txtMail.Text = dr.CU_EMAIL.ToString().Trim()
            Me.UI_txtAddress_HiddenField.Value = dr.CU_ADDRESS1.Trim()


            If dr.IsCU_ADDRESS1Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS1.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS2Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS2.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS3Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS3.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            If dr.IsCU_ADDRESS4Null = False Then
                Dim oRow As DataRow = dtAddress.NewRow()
                oRow("CU_Address") = dr.CU_ADDRESS4.Trim()
                dtAddress.Rows.Add(oRow)
            End If

            '20211223 wisely 若為Enduer 不允許輸入EndUser 欄位
            Dim oRequested As New ctlRMA.Requested
            Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
            If (dt.Rows.Count > 0) Then
                'trCompany.Visible = False
                'trEUName.Visible = False
                'trEUAddress.Visible = False
            Else
                'trCompany.Visible = True
                'trEUName.Visible = True
                'trEUAddress.Visible = True
            End If

        End If

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustomerUser = oCustomerUser.QueryUser(Me.UI_txtAccountIDText.Text.Trim, "")
            End If
        Else
            dtCustomerUser = oCustomerUser.QueryUser(Session("_CustomerID").ToString(), Session("_UserID").ToString())
        End If

        For i = 0 To dtCustomerUser.Rows.Count - 1
            Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

            Me.UI_lblUserIDText.Text = dr.CUUS_ACCOUNTID.ToString().Trim()
            If dr.IsCUUS_TELNull = False Then sTel = dr.CUUS_TEL.ToString().Trim()

            'If dr.IsCUUS_ADDRESSNull = False Then
            '    Dim oRow As DataRow = dtAddress.NewRow()
            '    oRow("CU_Address") = dr.CUUS_ADDRESS.Trim()
            '    dtAddress.Rows.Add(oRow)

            '    Dim ADDRESSData_List As New List(Of String)
            '    'ADDRESSData_List = GetData(Me.UI_lblAccountIDText.Text.ToString().Trim())

            '    For t As Integer = 0 To ADDRESSData_List.Count - 1
            '        Dim Address As String = ADDRESSData_List(t).ToString().Trim()
            '        dtAddress.Rows.Add(Address)
            '    Next

            'End If


            '' ''If dr.IsCUUS_ADDRESSNull = False Then
            '' ''    oListItem = New ListItem
            '' ''    oListItem.Text = dr.CUUS_ADDRESS.Trim()
            '' ''    oListItem.Value = dr.CUUS_ADDRESS.Trim()
            '' ''    Me.UI_cboAddress.Items.Add(oListItem)
            '' ''End If

            '資料 開始 20231229
            If sTel.Trim() IsNot Nothing Then
                Me.UI_txtTel_HiddenField.Value = sTel.Trim()
            End If

            If dr.CUUS_EMAIL IsNot Nothing Then
                Me.UI_txtMail_HiddenField.Value = dr.CUUS_EMAIL.Trim()
            End If
            '資料 結束 20231229

        Next

        Me.ViewState("_dtAddress") = dtAddress
    End Sub

#Region "儲存RMA單 "
    ' code goes here

    ''' <summary>
    ''' 儲存RMA單 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Save(ByVal RMA_Status As Integer)
        Dim blnFlag As Boolean = False
        Dim sMessage As String = ""
        Dim sRMAID As String = ""

        Dim oRequested As New ctlRMA.Requested
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oAdmin As New ctlAdmin

        Try

            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    'RMA Table
                    dtRMA = Save_RMA(RMA_Status, False)
                Case eumCommand.UPDATE
                    dtRMA = Save_RMA(RMA_Status, True)
            End Select

            'RMADetail Table
            dtRMADetail = Save_RMADetail(RMA_Status)
            Dim dvRMADetail_View As DataView = dtRMADetail.DefaultView
            dvRMADetail_View.RowFilter = "RMAD_MARK=0"

            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    sRMAID = oRequested.SaveByAddNew(dtRMA, dtRMADetail)
                    '新增要補 RMAD_ID 資料
                    '客戶編號
                    Dim myctAddress As New ctAddress
                    For i = 0 To dtRMADetail.Rows.Count - 1
                        Try

                            Dim RMAD_RMANO_string As String = sRMAID
                            Dim RMAD_SERIALNO_string As String = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()
                            Dim CUSTOMER_PRODUCT_NUMBER_string As String = ""
                            If dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER") Is Nothing Then


                                CUSTOMER_PRODUCT_NUMBER_string = ""
                            Else

                                CUSTOMER_PRODUCT_NUMBER_string = dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
                            End If

                            myctAddress.Up_RMADETAIL_New(RMAD_RMANO_string, RMAD_SERIALNO_string, CUSTOMER_PRODUCT_NUMBER_string)
                        Catch ex As Exception

                        End Try

                    Next
                Case eumCommand.UPDATE
                    oRequested.SaveByEdit(Me.UI_lblPreviousPage_RMANO.Text.Trim(), dtRMA, dtRMADetail)
                    sRMAID = Me.UI_lblPreviousPage_RMAID.Text.Trim()

                    '客戶編號
                    Dim myctAddress As New ctAddress
                    For i = 0 To dtRMADetail.Rows.Count - 1
                        Try

                            Dim RMAD_RMANO_string As String = dtRMADetail.Rows(i)("RMAD_ID").ToString().Trim()
                            Dim RMAD_SERIALNO_string As String = dtRMADetail.Rows(i)("RMAD_SERIALNO").ToString().Trim()
                            Dim CUSTOMER_PRODUCT_NUMBER_string As String = ""
                            If dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER") Is Nothing Then


                                CUSTOMER_PRODUCT_NUMBER_string = ""
                            Else

                                CUSTOMER_PRODUCT_NUMBER_string = dtRMADetail.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
                            End If

                            myctAddress.Up_RMADETAIL(RMAD_RMANO_string, RMAD_SERIALNO_string, CUSTOMER_PRODUCT_NUMBER_string)
                        Catch ex As Exception

                        End Try

                    Next


            End Select
            blnFlag = True


            Dim dtRequest As New RmaDTO.RequestReportDataTable
            Dim oRMARequest As New ctlRMA.Requested
            Dim oLoginInfo As New ctlLoginInfo
            Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)
            dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)

            Dim RMA_NO_ As String = ""

            For i = 0 To dtRequest.Rows.Count - 1
                Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
                RMA_NO_ = dr.RMA_NO.ToString().Trim()
            Next

            Dim myctAddress_Log As New ctAddress
            myctAddress_Log.Insert_OrderLog(System.Guid.NewGuid.ToString(), Session("_CustomerID").ToString().Trim(), RMA_NO_)

        Catch ex As Exception
            sMessage = ex.Message
            blnFlag = False

        Finally

            '20250226 part修改
            Dim UP_PARTSREQUEST As New ctlRMA.Requested
            UP_PARTSREQUEST.UPDATE_RMA_RMA_PARTSREQUEST(UI_PartsRequest.Checked, sRMAID.Trim)

            If RMA_Status = 10 Then
                'addLog(DirectCast(dtRMA.Rows(0), DataService.RmaDTO.RMARow).RMA_NO, "User :  " + Session("_UserID") + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Finally ReqNew Sumbit: " & sRMAID.Trim())
            Else
                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " Save ReqNew: " & sRMAID.Trim())
            End If

            If blnFlag = False Then
                Me.ucMessage.showMessageByFailed(sMessage)

            Else

                oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " STATUS: " & RMA_Status)
                If RMA_Status = 10 Then
                    '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 begin
                    'Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(sRMAID)
                    Dim oWarrantyData As DataTable = oRequested.chkISCWarrantyData(sRMAID)
                    oWAR_TYPE = oWarrantyData.AsEnumerable().Select(Function(x) x("WAR_TYPE").ToString()).FirstOrDefault()
                    Dim bool_ISCW As Boolean = oWarrantyData.AsEnumerable().
                                                Select(Function(x)
                                                           Dim v = x("RMAD_ISCW")
                                                           If IsDBNull(v) Then Return False

                                                           Select Case v.ToString()
                                                               Case "1"
                                                                   Return True
                                                               Case Else
                                                                   Return False
                                                           End Select
                                                       End Function).
                                                FirstOrDefault()
                    '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 end
                    Dim _Crypto As New SecurityCrypt.Crypto
                    Dim sParm_01 As String = _Crypto.Encrypt(sRMAID.Trim, "")
                    Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
                    Dim sRedirectURL As String = "ProductInformation_05_001.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02

                    '20210721 wisely add NZ_PB_TECH,AU_LAPTOP_KINGS 沒有全保功能
                    '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 begin
                    'If (UI_lblRepairCenterValue.Text = "NZ_PB_TECH" OrElse UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS") Then
                    '    bool_ISCW = False
                    'End If
                    If (((oWAR_TYPE = "CW" OrElse oWAR_TYPE = "P0") And
                        (UI_lblRepairCenterValue.Text = "JP_BYTE" OrElse
                        UI_lblRepairCenterValue.Text = "AU" OrElse
                        UI_lblRepairCenterValue.Text = "NZ_PB_TECH" OrElse
                        UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS")) OrElse
                        oWAR_TYPE = "E0") Then
                        bool_ISCW = False
                    End If
                    '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 end

                    'getRequestForm(sRMAID.Trim())
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    Dim isSendMail As Boolean = False
                    If UI_PartsRequest.Checked = True Then
                        isSendMail = SendMail_Parts(Me.UI_lblUserIDText.Text.ToString().Trim(), Me.UI_txtApplicant.Text.ToString().Trim(), sRMAID.Trim(), bool_ISCW)
                    Else
                        isSendMail = SendMail(Me.UI_lblUserIDText.Text.ToString().Trim(), Me.UI_txtApplicant.Text.ToString().Trim(), sRMAID.Trim(), bool_ISCW)
                    End If

                    oAdmin.addLog(oRequested.getRMANo(sRMAID.Trim()), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + sRMAID.Trim() & " endMail")
                    '客人送修, 按Submit時, 判斷若其中有一筆為全保, RMADETAIL.RMAD_ISCW = 1
                    '則pop window 顯示警示文字
                    bool_ISCW = oRequested.chkISCWarrantyFee(sRMAID)
                    If bool_ISCW = True And UI_PartsRequest.Checked = False Then

                        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 開始
                        If (UI_lblRepairCenterValue.Text = "JP_BYTE" OrElse UI_lblRepairCenterValue.Text = "AU" OrElse UI_lblRepairCenterValue.Text = "NZ_PB_TECH" OrElse UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS") Then
                            Response.Redirect(sRedirectURL)
                        Else
                            Me.ucMessage.showMessageBySuccess("<font color='red'>" & _oLanguage.getText("RMA2", "123", ctlLanguage.eumType.Tag) & "</font>", ascx_ucMessage.eumTransferURL.Redirect, sRedirectURL)
                        End If
                        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 結束

                    Else
                        Response.Redirect(sRedirectURL)
                    End If
                Else
                    Dim sMsg As String = ""
                    Select Case Convert.ToInt16(Me.ViewState("_eumCommand"))
                        Case eumCommand.AddNew
                            sMsg = oCommon.getMessage(Common.enmMessage.AddOK)
                            sMsg = _oLanguage.getText("Transfer", "044", ctlLanguage.eumType.Word) '"Please refer to Warranty Details for valid expiration date"
                        Case eumCommand.UPDATE
                            sMsg = oCommon.getMessage(Common.enmMessage.EditOK)
                    End Select
                    Me.ucMessage.showMessageBySuccess(sMsg, ascx_ucMessage.eumTransferURL.Redirect, _RedirectURL)

                End If

            End If
        End Try

        If blnFlag = False Then
            Dim sMsg As String = "Repeat sent"
            Me.ucMessage.showMessageByAlert(sMsg)
        Else

            Dim bool_ISCW As Boolean = oRequested.chkISCWarranty(sRMAID)
            Dim _Crypto As New SecurityCrypt.Crypto
            Dim sParm_01 As String = _Crypto.Encrypt(sRMAID.Trim, "")
            Dim sParm_02 As String = _Crypto.Encrypt(bool_ISCW.ToString(), "")
            Dim sRedirectURL As String = "ProductInformation_05_001.aspx?sRMAID=" & sParm_01 & "&ISCW=" & sParm_02
            bool_ISCW = oRequested.chkISCWarrantyFee(sRMAID)
            If bool_ISCW = True And UI_PartsRequest.Checked = False Then

                'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 開始
                If (UI_lblRepairCenterValue.Text = "JP_BYTE" OrElse UI_lblRepairCenterValue.Text = "AU" OrElse UI_lblRepairCenterValue.Text = "NZ_PB_TECH" OrElse UI_lblRepairCenterValue.Text = "AU_LAPTOP_KINGS") Then
                    Response.Redirect(sRedirectURL)
                Else
                    Me.ucMessage.showMessageBySuccess("<font color='red'>" & _oLanguage.getText("RMA2", "123", ctlLanguage.eumType.Tag) & "</font>", ascx_ucMessage.eumTransferURL.Redirect, sRedirectURL)
                End If
                'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 結束

            Else
                Response.Redirect(sRedirectURL)
            End If


        End If


    End Sub

    Protected Function GetUserEmail() As String

        Dim oCustomer As New ctlCustomer.Customer
        Dim oCustomerUser As New ctlCustomer.CustomerUser
        Dim dtCustome As New CustomerDTO.VWCUSTOMERDataTable
        Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        If Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
            If Me.UI_txtAccountIDText.Text.Trim <> "" Then
                dtCustome = oCustomer.QueryByCompany(Me.UI_txtAccountIDText.Text.Trim)
            End If
        Else
            dtCustome = oCustomer.QueryByCompany(Session("_CustomerID").ToString())
        End If

        If dtCustome.Count > 0 Then
            Dim dr As CustomerDTO.VWCUSTOMERRow = dtCustome.Rows(0)

            Return dr.CU_EMAIL.ToString().Trim()
        End If


    End Function

    Private Function Save_RMA(ByVal RMA_Status As Integer, ByVal bool_ISCW As Boolean) As RmaDTO.RMADataTable
        Dim dtRMA As New RmaDTO.RMADataTable
        Dim oGuid As Guid = Guid.NewGuid

        Try
            Dim dr As RmaDTO.RMARow = dtRMA.NewRMARow
            Select Case Convert.ToInt16(Session("_eumCommand"))
                Case eumCommand.AddNew
                    dr.RMA_ID = oGuid.ToString
                    dr.RMA_NO = ""

                Case eumCommand.UPDATE
                    dr.RMA_ID = Me.UI_lblPreviousPage_RMAID.Text.Trim()
                    dr.RMA_NO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
            End Select


            '地址編輯 開始
            Dim add As String

            add += Me.UI_txtCode.Text.Trim()


            If ddlDiv.Items.Count > 0 Then

                If ddlDiv.SelectedItem.Value.ToString().Trim() = "-Select-" Then

                Else
                    add += " " & ddlDiv.SelectedItem.Text.ToString().Trim()

                End If

            End If


            If Me.UI_txtCountry.Text.Trim() = "" Then
            Else
                add += " " & Me.UI_txtCountry.Text.Trim()
            End If


            add += " " & Me.UI_txtAddress.Text.Trim()
            '地址編輯 結束

            dr.RMA_CUNO = Me.UI_lblAccountNameText_HiddenField.Value.ToString().Trim()
            dr.RMA_ACCOUNTID = Session("_UserID")
            dr.RMA_APPLICANT = Me.UI_txtApplicant_HiddenField.Value.ToString().Trim()

            '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 開始
            Dim Email_ As String = GetUserEmail()


            dr.RMA_MAIL = Email_

            '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 結束


            dr.RMA_TEL = Me.UI_txtTel_HiddenField.Value.ToString().Trim()
            dr.RMA_ADDRESS = Me.UI_txtAddress_HiddenField.Value.ToString().Trim()

            If Me.UI_cboRepairCenter.Visible = False Then
                dr.RMA_COMPNO = Me.UI_lblRepairCenterValue.Text.ToString().Trim()
            Else
                dr.RMA_COMPNO = Me.UI_cboRepairCenter.SelectedValue
            End If


            dr.RMA_STATUS = RMA_Status
            dr.RMA_Remark = Me.UI_txtRemark.Text.Trim()

            dr.RMA_AD = Session("_UserID")
            dr.RMA_ADNAME = Session("_UserName")
            dr.RMA_CSTMP = Date.Now
            dr.RMA_LUAD = Session("_UserID")
            dr.RMA_LUADNAME = Session("_UserName")
            dr.RMA_LUSTMP = Date.Now
            dr.RMA_MARK = 0

            bool_ISCW = True

            If bool_ISCW Then
                dr.RMA_EUCOMPANY = Me.UI_lblAccountNameText.Text.Trim()
                dr.RMA_EUNAME = Me.UI_txtApplicant.Text.Trim()
                dr.RMA_EUTEL = Me.UI_txtTel.Text.Trim()
                dr.RMA_EUMAIL = Me.UI_txtMail.Text.Trim()
                dr.RMA_EUADDRESS = add
            Else
                dr.RMA_EUCOMPANY = ""
                dr.RMA_EUTEL = ""
                dr.RMA_EUNAME = ""
                dr.RMA_EUMAIL = ""
                dr.RMA_EUADDRESS = ""
            End If

            If UI_PartsRequest.Checked Then
                dr.RMA_PARTSREQUEST = 1 'Part’s request MAIL
            Else
                dr.RMA_PARTSREQUEST = 0
            End If


            If dr.RMA_TEL = "+886286471166" Then

                Try
                    Dim ctlCustomer As New ctlCustomer.Customer
                    Dim myCustomerDataTable As New CustomerDTO.CustomerDataTable
                    myCustomerDataTable = ctlCustomer.QueryByPrimaryKey(Session("_UserID"))
                    dr.RMA_TEL = myCustomerDataTable.Rows(0)("CU_TEL").ToString().Trim()
                Catch ex As Exception

                End Try
            End If



            dtRMA.AddRMARow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMA
    End Function
    Private Function Save_RMADetail(ByVal RMA_Status As Integer) As RmaDTO.RMADetailDataTable
        Dim i As Integer = 0
        Dim dtTMP As New RmaDTO.RMADetailDataTable
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable
        Dim oExport As New ctlRMA.Export


        If UI_dvRMADetail.Rows.Count = 0 Then

        Else

            Try
                dtTMP = Session("_dtRMADetail")
                Dim SNTemp As String = ""
                For i = 0 To UI_dvRMADetail.Rows.Count - 1
                    Dim UI_UI_SERIALNO As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_SERIALNO")
                    Dim UI_RMADID As Label = UI_dvRMADetail.Rows(i).Cells(1).FindControl("UI_RMADID")

                    If UI_UI_SERIALNO.Text.ToString() <> "" Then
                        dtTMP.DefaultView.RowFilter = "RMAD_ID<>'" + UI_RMADID.Text.Trim + "' AND RMAD_SERIALNO='" + UI_UI_SERIALNO.Text + "' AND RMAD_MARK<>'1'"
                        If dtTMP.DefaultView.Count > 0 Then
                            '系統無法找到資料 更換讀取條件 
                            Throw New ArgumentException(_oLanguage.getText("RMA", "084", ctlLanguage.eumType.Validator))
                            'Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                        End If

                        If SNTemp.IndexOf(UI_UI_SERIALNO.Text.Trim() + "~@~;") >= 0 Then
                            '系統無法找到資料 更換讀取條件
                            Throw New ArgumentException(_oLanguage.getText("RMA", "084", ctlLanguage.eumType.Validator))
                            'Throw New ArgumentException(_oLanguage.getText("RMA", "232", ctlLanguage.eumType.Validator))
                        Else
                            dtTMP.DefaultView.RowFilter = "RMAD_ID='" + UI_RMADID.Text.Trim + "'"
                            If dtTMP.DefaultView.Count > 0 Then
                                dtTMP.DefaultView(0)("RMAD_SERIALNO") = UI_UI_SERIALNO.Text.Trim()
                            End If
                            SNTemp += UI_UI_SERIALNO.Text.Trim() + "~@~;"
                        End If
                    Else
                        Throw New ArgumentException(_oLanguage.getText("RMA", "231", ctlLanguage.eumType.Validator))
                    End If
                Next
                dtTMP.DefaultView.RowFilter = ""
                dtTMP.AcceptChanges()




                For i = 0 To dtTMP.Rows.Count - 1
                    Dim drTMP As RmaDTO.RMADetailRow = dtTMP.Rows(i)
                    Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow

                    Select Case Convert.ToInt16(Session("_eumCommand"))
                        Case eumCommand.AddNew
                            Dim oGuid As Guid = Guid.NewGuid
                            dr.RMAD_ID = oGuid.ToString
                            dr.RMAD_RMANO = ""

                        Case eumCommand.UPDATE
                            dr.RMAD_ID = drTMP.RMAD_ID
                            dr.RMAD_RMANO = drTMP.RMAD_RMANO
                            'dr.RMAD_RMANO = Me.UI_lblPreviousPage_RMANO.Text.Trim()
                    End Select

                    dr.RMAD_SEQ = i + 1
                    If drTMP.IsRMAD_SERIALNONull = False Then dr.RMAD_SERIALNO = drTMP.RMAD_SERIALNO
                    If drTMP.IsRMAD_CUSNAMENull = False Then
                        dr.RMAD_CUSNAME = drTMP.RMAD_CUSNAME
                    Else
                        dr.RMAD_CUSNAME = ""
                    End If
                    If drTMP.IsRMAD_PARTSNNull = False Then

                        dr.RMAD_PARTSN = drTMP.RMAD_PARTSN
                    Else
                        dr.RMAD_PARTSN = ""
                    End If
                    '==================
                    'MODEL NO
                    '==================
                    If drTMP.IsRMAD_MODELNONull = False Then dr.RMAD_MODELNO = drTMP.RMAD_MODELNO
                    If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                        Dim sModelNo As String = oExport.getModelNo(drTMP.RMAD_SERIALNO.ToString().Trim())
                        If sModelNo.Trim() = "" Then
                            dr.RMAD_MODELNO = "OTHER"
                        Else
                            dr.RMAD_MODELNO = sModelNo
                        End If
                    End If


                    '==================
                    'WarrantyDate
                    '==================
                    'If drTMP.IsRMAD_WARRANTYNull = False Then dr.RMAD_WARRANTY = drTMP.RMAD_WARRANTY
                    Dim sWarranty As String = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                    If drTMP.RMAD_SERIALNO.ToString().Trim() <> "" Then
                        'Dim sWarrantyDate As String = oExport.getWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString())
                        'Dim sWarrantyDate As String = ""
                        'If Me.UI_cboRepairCenter.Visible = False Then
                        '    sWarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                        'Else
                        '    sWarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                        'End If

                        'Dim WarrantyDate As String = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString())
                        Dim WarrantyDate As String = ""
                        If Me.UI_cboRepairCenter.Visible = False Then
                            WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_lblRepairCenterValue.Text.ToString().Trim())
                        Else
                            WarrantyDate = oExport.getMaxWarranty(drTMP.RMAD_SERIALNO.ToString().Trim(), Session("_CustomerID").ToString(), Me.UI_cboRepairCenter.SelectedValue)
                        End If

                        'If sWarrantyDate.Trim() <> "" Then
                        '    sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                        'End If
                        If WarrantyDate.Trim() <> "" Then
                            dr.RMAD_WARRANTY = Convert.ToDateTime(WarrantyDate).ToShortDateString()
                        End If
                    End If
                    'If IsDate(sWarranty) = True Then
                    'dr.RMAD_WARRANTY = Convert.ToDateTime(sWarranty)
                    'End If





                    If drTMP.IsRMAD_FARFARCNONull = False Then
                        If drTMP.RMAD_FARFARCNO.Trim() <> "-1" Then
                            dr.RMAD_FARFARCNO = drTMP.RMAD_FARFARCNO
                        End If
                    End If

                    If drTMP.IsRMAD_FARNONull = False Then
                        If drTMP.RMAD_FARNO.Trim() <> "-1" Then
                            dr.RMAD_FARNO = drTMP.RMAD_FARNO
                        End If
                    End If

                    If drTMP.IsRMAD_UPLOADFILENull = False Then
                        dr.RMAD_UPLOADFILE = drTMP.RMAD_UPLOADFILE
                    Else
                        dr.RMAD_UPLOADFILE = ""
                    End If


                    If dtTMP.Rows(i)("RMAD_PRODUCTDESC") Is Nothing Then
                    Else
                        dr.RMAD_PROBLEMDESC = dtTMP.Rows(i)("RMAD_PRODUCTDESC").ToString().Trim()
                        dr.RMAD_PRODUCTDESC = dtTMP.Rows(i)("RMAD_PRODUCTDESC").ToString().Trim()
                    End If

                    If dtTMP.Rows(i)("CUSTOMER_PRODUCT_NUMBER") Is Nothing Then
                        dr.CUSTOMER_PRODUCT_NUMBER = ""
                    Else
                        dr.CUSTOMER_PRODUCT_NUMBER = dtTMP.Rows(i)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
                    End If


                    dr.RMAD_STATUS = RMA_Status
                    dr.RMAD_ISFILL = drTMP.RMAD_ISFILL
                    dr.RMAD_RECEVSTATUS = 0

                    dr.RMAD_AD = Session("_UserID")
                    dr.RMAD_ADNAME = Session("_UserName")
                    dr.RMAD_CSTMP = Date.Now
                    dr.RMAD_LUAD = Session("_UserID")
                    dr.RMAD_LUADNAME = Session("_UserName")
                    dr.RMAD_LUSTMP = Date.Now
                    dr.RMAD_MARK = drTMP.RMAD_MARK

                    dtRMADetail.AddRMADetailRow(dr)
                Next

            Catch ex As Exception
                Throw ex

            End Try

        End If

        Return dtRMADetail
    End Function
    Private Function SendMail_Parts(ByVal UserID As String, ByVal Applicant As String, ByVal RMAID As String, ByVal bool_ISCW As Boolean) As Boolean
        Dim blnFlag As Boolean = False
        Dim oAdmin As New ctlAdmin
        Dim dtPartMail As New AccountDTO.MailSetDataTable
        Dim oRequested As New ctlRMA.Requested
        Dim oMail As New ctlMail
        Try
            Dim RMA_NO As String = oRequested.getRMANo(RMAID.Trim())
            Dim sRequestDate As String = Date.Now.ToShortDateString()
            Dim MailUser As String = Me.UI_txtMail.Text.Trim() '申請人Mail

            '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 開始
            Dim Email_ As String = GetUserEmail()

            If Email_ = Me.UI_txtMail.Text.Trim() Then
                MailUser = Me.UI_txtMail.Text.Trim()
            Else
                MailUser = Me.UI_txtMail.Text.Trim() & "," & Email_
            End If
            '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 結束

            'End User 
            getRequestForm(RMAID, False) '附件檔產生


            Dim oAttachmentFile As New Collection
            If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
            End If

            '額外寄給其他人
            dtPartMail = oAdmin.GetMailSet()
            dtPartMail.DefaultView.RowFilter = "MAIL_ID='Part'"
            If dtPartMail.DefaultView.Count > 0 Then
                If dtPartMail.DefaultView(0)("MAIL_ADDRESS") <> "" Then
                    MailUser = MailUser & "," & dtPartMail.DefaultView(0)("MAIL_ADDRESS")
                End If
            End If

            Dim sSubject_User As String = _oLanguage.getText("Mail", "037", ctlLanguage.eumType.Mail)
            Dim sBody_User As String = _oLanguage.getText("Mail", "010", ctlLanguage.eumType.Mail)
            Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

            If MailUser <> "" Then
                sSubject_User = sSubject_User.Replace("[$Request No$]", RMA_NO.Trim())
                sSubject_User = sSubject_User.Replace("[$Request Date$]", sRequestDate)

                sBody_User = sBody_User.Replace("[$Applicant Name$]", Applicant.Trim())
                sBody_User = sBody_User.Replace("[$Request Date$]", sRequestDate)
                sBody_User = sBody_User.Replace("[$RMA No$]", RMA_NO.Trim())
                sBody_User = sBody_User.Replace("[$Email URL$]", sEmailURL)

                '20200319 wisely add 日本要收到
                If UI_cboRepairCenter.SelectedValue.ToUpper() = "JP_BYTE" Then
                    _MailCC += "," + _RequestNew_JP_BYTE_EmailCC
                End If

                '20210702 wisely add 特殊人員要收到
                If UI_cboRepairCenter.SelectedValue.ToUpper() = "US_CL_MPLUS" Then
                    _MailCC += "," + _RequestNew_US_CL_MPLUS_EmailCC
                End If


                '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                If _isDebug = True Then
                    MailUser = ConfigurationManager.AppSettings("MailTo")
                    _MailCC = ConfigurationManager.AppSettings("MailCC")
                End If
                blnFlag = oMail.SendMail(sSubject_User, sBody_User, MailUser, _MailCC, oAttachmentFile)
                If blnFlag = False Then
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " parts mail faild.")
                    '   Exit Try
                End If

            End If

        Catch ex As Exception

        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If
        End Try
        Return blnFlag
    End Function
    Private Function SendMail(ByVal UserID As String, ByVal Applicant As String, ByVal RMAID As String, ByVal bool_ISCW As Boolean) As Boolean
        Dim blnFlag As Boolean = False

        Dim j As Integer = 0
        Dim ctlRMA As New ctlRMA
        Dim oRequested As New ctlRMA.Requested
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oCustomer As New ctlCustomer.CustomerUser
        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
        Dim oMail As New ctlMail

        Try
            Dim RMA_NO As String = oRequested.getRMANo(RMAID.Trim())
            Dim CU_NAME As String = ""
            Dim MailSales As String = ""
            Dim MailAssistant As String = ""
            dtCustomer = oCustomer.QueryUser(UserID)
            If dtCustomer.Rows.Count > 0 And RMA_NO.Trim() <> "" Then
                oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendMail")
                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(申請人)
                '================================================================================================================================================================================================================

                Dim sRequestDate As String = Date.Now.ToShortDateString()
                'Dim MailUser As String = dtCustomer.Rows(0)("CUUS_EMAIL").ToString().Trim()
                Dim MailUser As String = ""

                '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 開始
                Dim Email_ As String = GetUserEmail()

                '澳洲維修中心 系統通知信與收件人調整 modify by buck 20250909
                '紐西蘭 系統通知信與收件人調整 modify by buck 20251217
                If dtCustomer.Rows(0)("CU_COUNTRYID").ToString().Trim() = oCommon.GetDescriptionText(oCommon.enmRepairCenter.AU) Then
                    Dim dtAdminTemp = oAdmin.Query("", "1,2,4")
                    Dim aryRepairCenter As String() = {oCommon.GetDescriptionText(oCommon.enmRepairCenter.CL_AU_Service_Center)}
                    Dim AURepairCenter = From x In dtAdminTemp
                                         Where x.AD_REPAIRCENTER = String.Join(",", aryRepairCenter)
                                         Select x

                    AURepairCenter.ToList().ForEach(Sub(item)
                                                        Email_ += "," & item.AD_EMAIL
                                                    End Sub)

                ElseIf oCommon.GetDescriptionText(oCommon.enmRepairCenter.CL_NZ_Service_Center).Contains(dtCustomer.Rows(0)("CU_COUNTRYID").ToString().Trim()) Then

                    Dim dtAdminTemp = oAdmin.Query("", "1,2,4")
                    Dim enmRepairCenter As New Common.enmRepairCenter
                    Dim aryRepairCenter As String() = {oCommon.GetDescriptionText(oCommon.enmRepairCenter.CL_NZ_Service_Center)}
                    Dim AURepairCenter = From x In dtAdminTemp
                                         Where x.AD_REPAIRCENTER = String.Join(",", aryRepairCenter) And x.AD_ID = "CLNZ"
                                         Select x

                    AURepairCenter.ToList().ForEach(Sub(item)
                                                        Email_ += "," & item.AD_EMAIL
                                                    End Sub)

                End If
                '紐西蘭 系統通知信與收件人調整 modify by buck 20251217
                '澳洲維修中心 系統通知信與收件人調整 modify by buck 20250909

                If Email_ = Me.UI_txtMail.Text.Trim() Then
                    MailUser = Me.UI_txtMail.Text.Trim()
                Else
                    MailUser = Me.UI_txtMail.Text.Trim() & "," & Email_
                End If
                '新增原始客戶 EMAIL 與 本次輸入 EMAIL , 為區隔條件 結束

                Dim sSubject_User As String = _oLanguage.getText("Mail", "009", ctlLanguage.eumType.Mail)
                Dim sBody_User As String = _oLanguage.getText("Mail", "010", ctlLanguage.eumType.Mail)
                Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
                Try
                    If MailUser <> "" Then
                        oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendRequester")
                        sSubject_User = sSubject_User.Replace("[$Request No$]", RMA_NO.Trim())
                        sSubject_User = sSubject_User.Replace("[$Request Date$]", sRequestDate)

                        sBody_User = sBody_User.Replace("[$Applicant Name$]", Applicant.Trim())
                        sBody_User = sBody_User.Replace("[$Request Date$]", sRequestDate)
                        sBody_User = sBody_User.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody_User = sBody_User.Replace("[$Email URL$]", sEmailURL)

                        '20240605 終端顧客也要接受到附件 分辨是否有下條件 開始
                        getRequestForm_001(RMAID, True) '附件檔產生
                        Dim oAttachmentFile As New Collection
                        If Me.ViewState("_AttachmentFile_01_001").ToString().Trim() <> "" Then
                            oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01_001").ToString())
                        End If
                        sBody_User = sBody_User.Replace("[$Email", sEmailURL)
                        sBody_User = sBody_User.Replace("URL$]", "")

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            MailUser = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject_User, sBody_User, MailUser, _MailCC, oAttachmentFile)

                        '20240605 終端顧客也要接受到附件 分辨是否有下條件 結束

                        If blnFlag = False Then
                            oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & "cust mail faild.")
                            '   Exit Try
                        End If

                    End If
                Catch ex As Exception
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = "sunny@cipherlab.com.tw"
                    Dim mailCC As String = "yijen.lo@cipherlab.com.tw"
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        mailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to customer fail.", mailTo, mailCC)
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to sales fail.")

                Finally


                End Try

                '================================================================================================================================================================================================================
                '顧客申請確認-->對象(業務和助理)
                '================================================================================================================================================================================================================
                Try
                    CU_NAME = dtCustomer.Rows(0)("CU_NAME").ToString().Trim()
                    Dim CU_SALESID As String = dtCustomer.Rows(0)("CU_SALESID").ToString().Trim()
                    Dim CU_ASSISTANTID As String = dtCustomer.Rows(0)("CU_ASSISTANTID").ToString().Trim()

                    'Dim MailSales As String = ""
                    Dim SalesName As String = ""
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " sales : " & CU_SALESID.Trim())
                    If CU_SALESID.Trim() <> "" Then
                        dtAdmin = oAdmin.Query(CU_SALESID, "")
                        MailSales = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '業務Mail
                        SalesName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If

                    ' Dim MailAssistant As String = ""
                    Dim AssistantName As String = ""
                    If CU_ASSISTANTID.Trim() <> "" Then
                        dtAdmin = oAdmin.Query(CU_ASSISTANTID, "")
                        MailAssistant = dtAdmin.Rows(0)("AD_EMAIL").ToString().Trim()       '助理Mail
                        AssistantName = dtAdmin.Rows(0)("AD_Name").ToString().Trim()        '
                    End If

                    Dim sSubject As String = _oLanguage.getText("Mail", "011", ctlLanguage.eumType.Mail)
                    Dim sBody As String = _oLanguage.getText("Mail", "012", ctlLanguage.eumType.Mail)
                    If MailSales.Trim() <> "" Or MailAssistant.Trim() <> "" Then
                        Dim sMailTo As String = ""
                        sSubject = sSubject.Replace("[$Customer's Name$]", CU_NAME.Trim())
                        sSubject = sSubject.Replace("[$Request No$]", RMA_NO.Trim())
                        sSubject = sSubject.Replace("[$Request Date$]", sRequestDate.Trim())

                        If MailSales.Trim() = "" Then
                            sBody = sBody.Replace("[$Sales Name$]", "")
                            sBody = sBody.Replace("/", "")
                        Else
                            sBody = sBody.Replace("[$Sales Name$]", SalesName)
                            sMailTo = MailSales.Trim()
                        End If

                        If MailAssistant.Trim() = "" Then
                            sBody = sBody.Replace("[$Assistant Name$]", "")
                            sBody = sBody.Replace("/", "")
                        Else
                            sBody = sBody.Replace("[$Assistant Name$]", AssistantName)
                            If sMailTo.Trim <> "" Then
                                sMailTo = sMailTo & ","
                            End If
                            sMailTo = sMailTo & MailAssistant.Trim()
                        End If

                        sBody = sBody.Replace("[$Request Date$]", sRequestDate.Trim())
                        sBody = sBody.Replace("[$Customer Name$]", CU_NAME.Trim())
                        sBody = sBody.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody = sBody.Replace("[$Email URL$]", sEmailURL)

                        '20200319 wisely add 日本要收到
                        If UI_cboRepairCenter.SelectedValue.ToUpper() = "JP_BYTE" Then
                            _MailCC += "," + _RequestNew_JP_BYTE_EmailCC
                        End If

                        '20210702 wisely add 特殊人員要收到
                        If UI_cboRepairCenter.SelectedValue.ToUpper() = "US_CL_MPLUS" Then
                            _MailCC += "," + _RequestNew_US_CL_MPLUS_EmailCC
                        End If

                        '強制新增日期
                        sBody.Replace("[$Request", sRequestDate.Trim())
                        sBody.Replace("Date$]", "")

                        oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendSales")

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            sMailTo = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)

                    End If
                Catch ex As Exception
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = "sunny@cipherlab.com.tw"
                    Dim mailCC As String = "yijen.lo@cipherlab.com.tw"
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        mailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to sales fail.", mailTo, mailCC)
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to sales fail.")

                Finally


                End Try
            End If


            '================================================================================================================================================================================================================
            '顧客申請確認-->對象(維修中心維修人員)
            '================================================================================================================================================================================================================
            If bool_ISCW = True Then
                Try
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " SendbyCW")
                    Dim sRequestDate As String = Date.Now.ToShortDateString()

                    '20230907  true 等於能填入名字
                    getRequestForm(RMAID, False) '附件檔產生

                    Dim oAttachmentFile As New Collection
                    If Me.ViewState("_AttachmentFile_01").ToString().Trim() <> "" Then
                        oAttachmentFile.Add(Me.ViewState("_AttachmentFile_01").ToString())
                    End If

                    Dim sSubject_Repair As String = _oLanguage.getText("Mail", "424", ctlLanguage.eumType.Mail)
                    Dim sBody_Repair As String = _oLanguage.getText("Mail", "425", ctlLanguage.eumType.Mail)

                    'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知
                    If (Session("_RepairID").ToString() = "JP_BYTE" OrElse Session("_RepairID").ToString() = "AU" OrElse Session("_RepairID").ToString() = "NZ_PB_TECH" OrElse Session("_RepairID").ToString() = "AU_LAPTOP_KINGS") Then
                        sBody_Repair = _oLanguage.getText("RMA2", "129", ctlLanguage.eumType.Tag)

                    Else

                    End If
                    'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知


                    Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)

                    sSubject_Repair = sSubject_Repair.Replace("[$Customer's Name$]", CU_NAME.Trim())
                    sSubject_Repair = sSubject_Repair.Replace("[$Request No$]", RMA_NO.Trim())
                    sSubject_Repair = sSubject_Repair.Replace("[$Request Date$]", sRequestDate.Trim())

                    Dim Repaire_MailTo As String = ""
                    Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                    Dim Repaire_Name As String = ""                    '維修人員
                    Dim arrRepaire As ArrayList = ctlRMA.getRepaireMail_RMA(RMA_NO)
                    For j = 0 To arrRepaire.Count - 1
                        Dim arrList() As String = arrRepaire(j)

                        Repaire_Name = arrList(0).Trim()
                        Repaire_EMAIL = arrList(1).Trim()

                        'If Repaire_Name.Trim <> "" Then
                        '    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", Repaire_Name)
                        'Else
                        '    sBody_Repair = sBody_Repair.Replace("[$Repaire Name$]", "")
                        'End If

                        If Repaire_MailTo.IndexOf(Repaire_EMAIL.Trim()) = -1 Then
                            If Repaire_MailTo.Trim <> "" Then
                                Repaire_MailTo = Repaire_MailTo & ","
                            End If

                            Repaire_MailTo = Repaire_MailTo & Repaire_EMAIL.Trim()
                        End If
                    Next

                    'Isaac Add 180326 加入業務業管Mail通知
                    If MailSales.Trim() <> "" Then '業務Mail
                        Repaire_MailTo = Repaire_MailTo & "," & MailSales.Trim()
                    End If

                    If MailAssistant.Trim() <> "" Then '助理Mail
                        Repaire_MailTo = Repaire_MailTo & "," & MailAssistant.Trim()
                    End If



                    'Repaire_MailTo = "isaac.yeh@cipherlab.com.tw,sunny@cipherlab.com.tw"
                    If Repaire_MailTo.Trim <> "" Then
                        sBody_Repair = sBody_Repair.Replace("[$Request Date$]", sRequestDate.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$Customer Name$]", CU_NAME.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$RMA No$]", RMA_NO.Trim())
                        sBody_Repair = sBody_Repair.Replace("[$Email URL$]", sEmailURL)

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            Repaire_MailTo = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject_Repair, sBody_Repair, Repaire_MailTo, _RepairEmailCC, oAttachmentFile)
                    End If

                Catch ex As Exception
                    '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                    Dim mailTo As String = "sunny@cipherlab.com.tw"
                    Dim mailCC As String = "yijen.lo@cipherlab.com.tw"
                    If _isDebug = True Then
                        mailTo = ConfigurationManager.AppSettings("MailTo")
                        mailCC = ConfigurationManager.AppSettings("MailCC")
                    End If
                    oMail.SendMail(RMA_NO.Trim() & " RMA New Request Sand mail fail.", RMA_NO.Trim() & " mail to RMA center fail.", mailTo, mailCC)
                    oAdmin.addLog(RMA_NO.Trim(), "User : " + Session("_UserID") + " Name: " + Session("_UserName") + " " + RMA_NO.Trim() & " mail to RMA fail.")

                Finally


                End Try

            End If

        Catch ex As Exception


        Finally
            Dim sMsg As String = ""
            If blnFlag = False Then
                sMsg = _oLanguage.getText("Mail", "008", ctlLanguage.eumType.Tag)
                Me.ucMessage.showMessageByAlert(sMsg)
            End If

        End Try

        Return blnFlag
    End Function
    Private Sub getRequestForm(ByVal sRMAID As String, ByVal EndUser As Boolean)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)

        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)
        EndUser = oRMARequest.chkISCWarrantyFee(sRMAID)

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
            Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow

            If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
            If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
            If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
            If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
            If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
            If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
            If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
            If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

            If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
            If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
            If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
            If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
            If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()

            '20240308 客戶編號 開始
            Dim myctAddress As New ctAddress
            Dim CUSTOMER_PRODUCT_NUMBER As String = myctAddress.Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(dr.RMA_NO.ToString(), dr.RMAD_SERIALNO.ToString())
            drReport.RMAD_PRODUCTDESC = CUSTOMER_PRODUCT_NUMBER
            '20240308 客戶編號 結束

            '20221205 wisely add 若為Enduser送修 
            'Dim oRequested As New ctlRMA.Requested
            'Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblAccountIDText.Text.Trim())
            If (EndUser AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                'drReport.RMA_EUMAIL = dt.Rows(0)("cu_email").ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If


            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next


        Call Print(dtReport, EndUser, LanguageID)
    End Sub

    Private Sub getRequestForm_001(ByVal sRMAID As String, ByVal EndUser As Boolean)
        Dim i As Integer = 0
        Dim oRMARequest As New ctlRMA.Requested
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim dtReport As New RmaDTO.RequestReportDataTable

        Dim oLoginInfo As New ctlLoginInfo
        Dim LanguageID As String = oLoginInfo.getLanguageIDRMAID("Customer", sRMAID)

        dtRequest = oRMARequest.getReport(LanguageID.ToString().Trim(), sRMAID)
        EndUser = oRMARequest.chkISCWarrantyFee(sRMAID)

        For i = 0 To dtRequest.Rows.Count - 1
            Dim dr As RmaDTO.RequestReportRow = dtRequest.Rows(i)
            Dim drReport As RmaDTO.RequestReportRow = dtReport.NewRequestReportRow

            If dr.IsRMA_NONull = False Then drReport.RMA_NO = dr.RMA_NO.ToString().Trim()
            If dr.IsRMA_IDNull = False Then drReport.RMA_ID = dr.RMA_ID.ToString().Trim()
            If dr.IsRMA_CUNONull = False Then drReport.RMA_CUNO = dr.RMA_CUNO.ToString().Trim()
            If dr.IsRMA_ACCOUNTIDNull = False Then drReport.RMA_ACCOUNTID = dr.RMA_ACCOUNTID.ToString().Trim()
            If dr.IsRMA_APPLICANTNull = False Then drReport.RMA_APPLICANT = dr.RMA_APPLICANT.ToString().Trim()
            If dr.IsRMA_TELNull = False Then drReport.RMA_TEL = dr.RMA_TEL.ToString().Trim()
            If dr.IsRMA_ADDRESSNull = False Then drReport.RMA_ADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            If dr.IsRMA_CSTMPNull = False Then drReport.RMA_CSTMP = Convert.ToDateTime(dr.RMA_CSTMP.ToString().Trim()).ToShortDateString()
            If dr.IsCU_NAMENull = False Then drReport.CU_NAME = dr.CU_NAME.ToString().Trim()
            If dr.IsCOMP_NAMENull = False Then drReport.COMP_NAME = dr.COMP_NAME.ToString().Trim()
            If dr.IsNoticeDescNull = False Then drReport.NoticeDesc = dr.NoticeDesc.ToString().Trim()
            If dr.IsRMAD_SERIALNONull = False Then drReport.RMAD_SERIALNO = dr.RMAD_SERIALNO.ToString().Trim()
            If dr.IsRMAD_PARTSNNull = False Then '如果是送PART就顯示PART序號
                drReport.RMAD_SERIALNO = dr.RMAD_PARTSN.Trim()
            End If
            If dr.IsRMAD_MODELNONull = False Then drReport.RMAD_MODELNO = dr.RMAD_MODELNO.ToString().Trim()
            If dr.IsRMAD_CUSNAMENull = False Then drReport.RMAD_CUSNAME = dr.RMAD_CUSNAME.ToString().Trim()
            If dr.IsRMAD_WARRANTYNull = False Then drReport.RMAD_WARRANTY = Convert.ToDateTime(dr.RMAD_WARRANTY.ToString().Trim()).ToShortDateString()
            If dr.IsFARC_NAMENull = False Then drReport.FARC_NAME = dr.FARC_NAME.ToString().Trim()

            If dr.IsRMAD_PRODUCTDESCNull = False Then drReport.RMAD_PRODUCTDESC = dr.RMAD_PRODUCTDESC.ToString().Trim()
            If dr.IsCOMP_ADDRESSNull = False Then drReport.COMP_ADDRESS = dr.COMP_ADDRESS.ToString().Trim()
            If dr.IsCOMP_TELNull = False Then drReport.COMP_TEL = dr.COMP_TEL.ToString().Trim()
            If dr.IsCW_EDATENull = False Then drReport.CW_EDATE = Convert.ToDateTime(dr.CW_EDATE.ToString().Trim()).ToShortDateString()

            If dr.IsRMA_EUCOMPANYNull = False Then drReport.RMA_EUCOMPANY = dr.RMA_EUCOMPANY.ToString().Trim()
            If dr.IsRMA_EUNAMENull = False Then drReport.RMA_EUNAME = dr.RMA_EUNAME.ToString().Trim()
            If dr.IsRMA_EUMAILNull = False Then drReport.RMA_EUMAIL = dr.RMA_EUMAIL.ToString().Trim()
            If dr.IsRMA_EUTELNull = False Then drReport.RMA_EUTEL = dr.RMA_EUTEL.ToString().Trim()
            If dr.IsRMA_EUADDRESSNull = False Then drReport.RMA_EUADDRESS = dr.RMA_EUADDRESS.ToString().Trim()

            '20221205 wisely add 若為Enduser送修 
            'Dim oRequested As New ctlRMA.Requested
            'Dim dt As DataTable = oRequested.IsEndUser(Me.UI_lblAccountIDText.Text.Trim())
            If (EndUser AndAlso drReport.IsRMA_EUCOMPANYNull) Then
                drReport.RMA_EUCOMPANY = drReport.CU_NAME.ToString().Trim()
                drReport.RMA_EUTEL = drReport.RMA_TEL.ToString().Trim()
                drReport.RMA_EUNAME = dr.RMA_APPLICANT.ToString().Trim()
                'drReport.RMA_EUMAIL = dt.Rows(0)("cu_email").ToString().Trim()
                drReport.RMA_EUADDRESS = dr.RMA_ADDRESS.ToString().Trim()
            End If


            drReport.SeqID = i + 1

            dtReport.AddRequestReportRow(drReport)
        Next


        Call Print_001(dtReport, EndUser, LanguageID)
    End Sub

    Private Sub Print_001(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal sLanguageID As String)
        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        Dim check As Boolean = False

        Dim dtEndUser As DataTable
        Dim ctlRMAR As New ctlRMA.Requested
        dtEndUser = ctlRMAR.IsEndUser(Session("_UserID"))

        If (dtEndUser.Rows.Count > 0) Then
            check = True
        End If

        ''取得客戶的語系
        'Dim sCust As String = UI_txtAccountIDText.Text
        'Dim oLoginInfo As New ctlLoginInfo
        'Dim sLanguageID As String = oLoginInfo.getLanguageID("Customer", sCust)

        EndUser = True
        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument

        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知
        Dim Server_MapPath_rpt As String = ""
        Dim check_bool As Boolean = False
        If (Session("_RepairID").ToString() = "JP_BYTE" OrElse Session("_RepairID").ToString() = "AU" OrElse Session("_RepairID").ToString() = "NZ_PB_TECH" OrElse Session("_RepairID").ToString() = "AU_LAPTOP_KINGS") Then
            check_bool = True
            Panel1.Visible = False
        Else

        End If
        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 結束

        'ReportDoc.Load(Server.MapPath("Report\rptRequest_EndUser.rpt"))

        If (EndUser) Then
            If sLanguageID = "003" Then
                If check = True Then
                    If (check_bool) Then
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_jp_Del_Notice.rpt")
                    Else
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_jp_Del.rpt")
                    End If
                Else
                    If (check_bool) Then
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_jp_Notice.rpt")
                    Else
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_jp.rpt")
                    End If
                End If
            Else
                If check = True Then
                    If (check_bool) Then
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_Del_Notice.rpt")
                    Else
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_Del.rpt")
                    End If
                Else
                    If (check_bool) Then
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_Notice.rpt")
                    Else
                        Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser.rpt")
                    End If
                End If
            End If

        Else
            If sLanguageID = "003" Then
                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_jp_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_jp.rpt")
                End If
            Else
                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02.rpt")
                End If
            End If
        End If

        'ReportDoc.Load(Server_MapPath_rpt)
        'ReportDoc.SetDataSource(oReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        '修改Export函式。因本機開發環境vs2022所以Crystal Report需用v13，並且環境無法新舊版共存。
        '所以將舊版封裝成exe檔案，用呼叫exe方式呼叫舊版元件 by buck modify 20251106 begin
        Dim exportPath As String = IO.Path.Combine(_Reoprt_FilePath, sReportToPDF)
        _Comms.ExportSetup_New(Server_MapPath_rpt, exportPath, oReport)
        'oCommon.ExportSetup(ReportDoc, sReportToPDF)
        '修改Export函式。因本機開發環境vs2022所以Crystal Report需用v13，並且環境無法新舊版共存。
        '所以將舊版封裝成exe檔案，用呼叫exe方式呼叫舊版元件 by buck modify 20251106 end
        Me.ViewState("_AttachmentFile_01_001") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_01_001") = ConfigureExportToPdf(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

        ReportDoc.Close()
    End Sub

    Private Sub Print(ByVal dtReport As RmaDTO.RequestReportDataTable, ByVal EndUser As Boolean, ByVal LanguageID As String)
        Dim sReportToPDF As String = "Request_" & oCommon.GetRandomizeNum() & ".pdf"

        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================
        Dim oReport As New DataSet
        oReport.Tables.Add(dtReport)

        EndUser = True

        '取得客戶的語系
        Dim sCust As String = UI_txtAccountIDText.Text
        Dim oLoginInfo As New ctlLoginInfo
        Dim sLanguageID As String = oLoginInfo.getLanguageID("Customer", sCust)

        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知
        Dim Server_MapPath_rpt As String = ""
        '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 begin
        'Dim check_bool As Boolean = False
        'If (Session("_RepairID").ToString() = "JP_BYTE" OrElse Session("_RepairID").ToString() = "AU" OrElse Session("_RepairID").ToString() = "NZ_PB_TECH" OrElse Session("_RepairID").ToString() = "AU_LAPTOP_KINGS") Then
        '    check_bool = True
        'Else

        'End If
        Dim check_bool As Boolean = False
        If (((oWAR_TYPE = "CW" OrElse oWAR_TYPE = "P0") And
            (Session("_RepairID").ToString() = "JP_BYTE" OrElse
            Session("_RepairID").ToString() = "AU" OrElse
            Session("_RepairID").ToString() = "NZ_PB_TECH" OrElse
            Session("_RepairID").ToString() = "AU_LAPTOP_KINGS")) OrElse
            oWAR_TYPE = "E0") Then
            check_bool = True
        Else

        End If
        '修改保固資訊後面可以判斷，並且修改為回傳datatable(取件通知問題) by buck modify 2026/01/02 end
        'JP、AU、NZ維修中心沒有取件服務，系統當初有特別做調整，所以如果是這三個維修中心的保內CW / P0，都不會跳取件相關通知 結束

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If (UI_PartsRequest.Checked) Then
            If (sLanguageID = "003") Then

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_Parts_jp_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_Parts_jp.rpt")
                End If

            Else

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_Parts_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_Parts.rpt")
                End If
            End If

        ElseIf (EndUser) Then
            If (sLanguageID = "003") Then

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser.rpt")
                End If

            Else

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_EndUser.rpt")
                End If
            End If
        Else
            If (sLanguageID = "003") Then

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_jp_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_jp.rpt")
                End If

            Else

                If (check_bool) Then
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02_Notice.rpt")
                Else
                    Server_MapPath_rpt = Server.MapPath("Report\rptRequest_02.rpt")
                End If

            End If

        End If

        'ReportDoc.Load(Server_MapPath_rpt)
        'ReportDoc.SetDataSource(oReport)

        'CrystalReportViewer1.ReportSource = ReportDoc
        'Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        '修改Export函式。因本機開發環境vs2022所以Crystal Report需用v13，並且環境無法新舊版共存。
        '所以將舊版封裝成exe檔案，用呼叫exe方式呼叫舊版元件 by buck modify 20251106 begin
        Dim exportPath As String = IO.Path.Combine(_Reoprt_FilePath, sReportToPDF)
        _Comms.ExportSetup_New(Server_MapPath_rpt, exportPath, oReport)
        'oCommon.ExportSetup(ReportDoc, sReportToPDF)
        '修改Export函式。因本機開發環境vs2022所以Crystal Report需用v13，並且環境無法新舊版共存。
        '所以將舊版封裝成exe檔案，用呼叫exe方式呼叫舊版元件 by buck modify 20251106 end
        Me.ViewState("_AttachmentFile_01") = _Reoprt_FilePath & sReportToPDF
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_01") = ConfigureExportToPdf(sReportToPDF)
        '修改Export PDF共用函式 by buck modify 20250828 end

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

    'Public Function ConfigureExportToPdf(ByVal sReportToPDF As String) As String
    '    Dim retval As String = _Reoprt_FilePath & sReportToPDF

    '    '===================================================================================================================
    '    '傳PDF檔
    '    '===================================================================================================================
    '    myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
    '    myDiskFileDestinationOptions.DiskFileName = _Reoprt_FilePath & sReportToPDF
    '    myExportOptions.DestinationOptions = myDiskFileDestinationOptions
    '    ReportDoc.Export()

    '    Return retval
    'End Function

    ''' <summary>
    ''' 儲存之前的檢核 -- 品項
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chkRMA() As Boolean
        Dim retval As Boolean = True
        Dim sMessage As String = ""


        Try
            '20170502 Isaac Add
            'If Not (UI_txtEUCompany.Text.Trim() = "") Or Not (UI_txtEUTel.Text.Trim() = "") Or Not (UI_txtEUName.Text.Trim() = "") Or Not (UI_txtEUMail.Text.Trim() = "") Or Not (UI_txtEUAddress.Text.Trim() = "") Then
            '    If UI_txtEUCompany.Text = "" Then
            '        sMessage = _oLanguage.getText("Transfer", "039", ctlLanguage.eumType.Word) '"End User Company Name is null"
            '        retval = False
            '    ElseIf UI_txtEUTel.Text = "" Then
            '        sMessage = _oLanguage.getText("Transfer", "040", ctlLanguage.eumType.Word)  '"End User Tel No. is null"
            '        retval = False
            '    ElseIf UI_txtEUName.Text = "" Then
            '        sMessage = _oLanguage.getText("Transfer", "041", ctlLanguage.eumType.Word)  '"End User Contact Person is null"
            '        retval = False
            '    ElseIf UI_txtEUMail.Text = "" Then
            '        sMessage = _oLanguage.getText("Transfer", "042", ctlLanguage.eumType.Word)  '"End User Email is null"
            '        retval = False
            '    ElseIf UI_txtEUAddress.Text = "" Then
            '        sMessage = _oLanguage.getText("Transfer", "043", ctlLanguage.eumType.Word)  '"End User Address is null"
            '        retval = False
            '    End If
            'End If

            If Not Session("_dtRMADetail") Is Nothing Then
                Dim dtRMADetail As RmaDTO.RMADetailDataTable = Session("_dtRMADetail")
                Dim dvRMADetail As DataView = dtRMADetail.DefaultView
                dvRMADetail.RowFilter = "RMAD_MARK=0"
                If dvRMADetail.Count = 0 Then
                    sMessage = _oLanguage.getText("RMA", "194", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If

                '是否已填寫問題:0.否, 1.是
                dvRMADetail.RowFilter = "RMAD_ISFILL=0 and RMAD_MARK=0"
                If dvRMADetail.Count > 0 Then
                    sMessage = _oLanguage.getText("RMA", "195", ctlLanguage.eumType.Validator)
                    retval = False
                    Exit Try
                End If
            End If

        Catch ex As Exception
            sMessage = ex.Message
            retval = False

        Finally

            If retval = False Then
                Me.ucMessage.showMessageByFailed(sMessage)
            End If
        End Try



        Return retval
    End Function

#End Region

    Protected Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click

        Response.Redirect("ProductInformation_04.aspx")

    End Sub
    Protected Sub ErmaButtonBackBackBtn_Click(sender As Object, e As EventArgs) Handles ErmaButtonBackBackBtn.Click
        Response.Redirect("ProductInformation_04.aspx")
    End Sub

    'Alert 開始
    Protected Function CheckingAlert(ByVal index As String) As String
        Dim Context As String
        Context = _oLanguage.getText("RMA2", index, ctlLanguage.eumType.Tag)
        Return Context
    End Function
    'Alert 結束
End Class

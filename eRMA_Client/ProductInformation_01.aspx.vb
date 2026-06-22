Imports System.Data
Imports DataService
Imports DefLanguage

Partial Class ProductInformation_01
    Inherits System.Web.UI.Page

    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_Upload_FilePath")
    Dim _Requested_ExcelSample_VisualPath As String = ConfigurationSettings.AppSettings("Requested_ExcelSample_VisualPath")
    Dim _Requested_Default_FarFarcNO As String = ConfigurationSettings.AppSettings("Requested_Default_FarFarcNO")
    Dim _oLanguage As New ctlLanguage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            Me.UI_cmdDisagree.Text = _oLanguage.getText("Common", "023", ctlLanguage.eumType.Command)
            Me.UI_cmdAgree.Text = _oLanguage.getText("Common", "024", ctlLanguage.eumType.Command)
        End If



        If Session("_UserID") = "" Then
            Session("_isTimeOut") = True

            Response.Redirect("~/Default.aspx")
        End If

        If Me.IsPostBack = False Then

            Call setDefault()
            Call QueryData()

            If Convert.ToBoolean(Session("_isPolicy")) = True Then
                Me.Panel1.Visible = False
                Me.Panel2.Visible = True
            Else
                Me.Panel1.Visible = True
                Me.Panel2.Visible = False

            End If

        End If

    End Sub

    Private Sub QueryData()
        Dim oPolicy As New ctlPolicy
        Dim dtPolicy As New PolicyDTO.PolicyDataTable

        'Session("_LanguageID") = "002"
        dtPolicy = oPolicy.Query(Session("_LanguageID").ToString(), Session("_Comp_Admin").ToString())

        If dtPolicy.Rows.Count > 0 Then
            Me.UI_lblPolicy.Text = dtPolicy.Rows(0).Item("POLICY_TEXT").ToString().Trim()
        End If


    End Sub
    Private Sub setDefault()
        Me.AddBtn.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.Product_Information_Lab.Text = _oLanguage.getText("RMA2", "056", ctlLanguage.eumType.Tag)
        Me.ContextLab.Text = _oLanguage.getText("RMA2", "057", ctlLanguage.eumType.Tag)
        Me.SerialLab.Text = _oLanguage.getText("RMA2", "058", ctlLanguage.eumType.Tag)
        Me.SerialNumberTxt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "059", ctlLanguage.eumType.Tag))
        Me.UploadFileLabel.Text = _oLanguage.getText("RMA2", "020", ctlLanguage.eumType.Tag)
        'Me.PleaseNumberTxt.Attributes.Add("placeholder", _oLanguage.getText("RMA2", "063", ctlLanguage.eumType.Tag))
        Me.SelectFileLabe.Text = _oLanguage.getText("RMA2", "021", ctlLanguage.eumType.Tag)
        Me.Upload_File_SizeLabel.Text = _oLanguage.getText("RMA2", "060", ctlLanguage.eumType.Tag)
        Me.Upload_File_CSV_SizeLabel.Text = _oLanguage.getText("RMA2", "061", ctlLanguage.eumType.Tag)
        Me.UploadFileLabel.Text = _oLanguage.getText("RMA2", "020", ctlLanguage.eumType.Tag)
        Me.Sample_hyperlink.Text = _oLanguage.getText("RMA2", "062", ctlLanguage.eumType.Tag)
    End Sub

    Protected Sub AddBtn_Click(sender As Object, e As EventArgs) Handles AddBtn.Click

        Session("_dtRMADetail") = Nothing
        '檢查 24小時 序號 開始
        Dim chk_CW As Boolean = False

        If SerialNumberTxt.Text.Trim() = "" Then
            'EXCEL
            '批次輸入
            Dim blnFlag As Boolean
            Dim sMessage As String = ""
            Dim i As Integer = 0

            Dim dt As New DataTable
            Dim oExport As New ctlRMA.Export
            Dim dtRMADetail As New RmaDTO.RMADetailDataTable


            Try
                Dim sFullFileName As String = UpLoadFile()

                '需檢查通過 才可以下一個階段 開始
                '判斷條件
                Dim lstWriteBits As List(Of String) = New List(Of String)()
                Dim lstWriteBits_New As List(Of String) = New List(Of String)()

                If sFullFileName <> "over" Then
                    If sFullFileName <> "format" Then

                        If sFullFileName = "" Then
                            Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                        End If

                        Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                        If txtFileName_Check = "" Then


                        Else




                            Dim dir As String = _Requested_Upload_FilePath
                            Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                            Dim TextLine As String
                            '客戶設定檔案
                            Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                            Dim ctlCustomer_Customer As New ctlCustomer.Customer
                            dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                            If System.IO.File.Exists(FILE_NAME) = True Then
                                Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)

                                Do While objReader.Peek() <> -1
                                    TextLine = objReader.ReadLine()
                                    Dim arrText() As String = TextLine.Split(",")
                                    Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                    lstWriteBits.Add(sSerial)

                                Loop
                                objReader.Close()
                            End If



                        End If
                    End If
                End If

                '過濾重複序號
                lstWriteBits_New = lstWriteBits.Distinct().ToList
                Dim Check_SN_List As List(Of String) = New List(Of String)()
                Check_SN_List = lstWriteBits.Intersect(lstWriteBits_New).ToList
                '需檢查通過 才可以下一個階段 結束
                Dim chk_sn_list As String = ""

                '比較數量是否減少
                If lstWriteBits.Count = lstWriteBits_New.Count Then
                    If sFullFileName <> "over" Then
                        If sFullFileName <> "format" Then

                            If sFullFileName = "" Then
                                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                            End If

                            Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                            If txtFileName_Check = "" Then
                                Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag) & "');  </script>"
                                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                            Else

                                Dim dir As String = _Requested_Upload_FilePath

                                Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                Dim TextLine As String

                                '客戶設定檔案
                                Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                If System.IO.File.Exists(FILE_NAME) = True Then
                                    Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                                    Do While objReader.Peek() <> -1
                                        TextLine = objReader.ReadLine()
                                        Dim arrText() As String = TextLine.Split(",")

                                        If arrText.Length > 0 Then
                                            If arrText(0).ToString().Trim().ToLower <> "Serial Number".ToLower() Then

                                                Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                                chk_sn_list = chk_sn_list & sSerial & ","
                                            End If
                                        End If

                                    Loop
                                    objReader.Close()
                                End If

                            End If

                        Else


                        End If
                    Else


                    End If
                Else
                    Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA2", "124", ctlLanguage.eumType.Tag) & "</br>" & Check_SN_List(1).ToString())
                End If

                Dim ctAddress As New ctAddress
                Dim ctAddress_dt As New DataTable
                chk_sn_list = chk_sn_list.Remove(chk_sn_list.Length - 1, 1)

                Dim chk_sn_list_Split() As String = Split(chk_sn_list, ",")
                Dim index_ As Integer = 0

                For i = 0 To chk_sn_list_Split.Length - 1

                    ctAddress_dt = ctAddress.chk_CW(chk_sn_list_Split(i).ToString().Trim())
                    If ctAddress_dt.Rows.Count > 0 Then
                        index_ = index_ + 1
                    Else

                    End If
                Next

                If index_ = chk_sn_list_Split.Length Then
                    chk_CW = True
                End If


            Catch ex As Exception

            Finally

            End Try
        Else
            '輸入框
            Dim ctAddress As New ctAddress
            Dim ctAddress_dt As New DataTable
            ctAddress_dt = ctAddress.chk_CW(SerialNumberTxt.Text.Trim())

            If ctAddress_dt.Rows.Count > 0 Then

                '批次輸入
                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim i As Integer = 0

                Dim dt As New DataTable
                Dim oExport As New ctlRMA.Export
                Dim dtRMADetail As New RmaDTO.RMADetailDataTable
                chk_CW = True
            Else

            End If

        End If

        '只要不是終端客戶 都是不需要檢查CW
        If 1 = 1 Then
            Dim oRequested As New ctlRMA.Requested
            Dim dt As DataTable = oRequested.IsEndUser(Session("_UserID"), "X0091")
            If (dt.Rows.Count > 0) Then
                chk_CW = True
            Else
                chk_CW = True
            End If
        End If

        If chk_CW = True Then
            If SerialNumberTxt.Text.Trim() = "" Then

                '批次輸入
                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim i As Integer = 0

                Dim dt As New DataTable
                Dim oExport As New ctlRMA.Export
                Dim dtRMADetail As New RmaDTO.RMADetailDataTable


                Try
                    Dim sFullFileName As String = UpLoadFile()

                    '需檢查通過 才可以下一個階段 開始
                    '判斷條件
                    Dim lstWriteBits As List(Of String) = New List(Of String)()
                    Dim lstWriteBits_New As List(Of String) = New List(Of String)()

                    If sFullFileName <> "over" Then
                        If sFullFileName <> "format" Then

                            If sFullFileName = "" Then
                                Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                            End If

                            Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                            If txtFileName_Check = "" Then


                            Else




                                Dim dir As String = _Requested_Upload_FilePath
                                Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                Dim TextLine As String
                                '客戶設定檔案
                                Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                If System.IO.File.Exists(FILE_NAME) = True Then
                                    Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)

                                    Do While objReader.Peek() <> -1
                                        TextLine = objReader.ReadLine()
                                        Dim arrText() As String = TextLine.Split(",")
                                        Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No
                                        lstWriteBits.Add(sSerial)

                                    Loop
                                    objReader.Close()
                                End If



                            End If
                        End If
                    End If

                    '過濾重複序號
                    lstWriteBits_New = lstWriteBits.Distinct().ToList
                    Dim Check_SN_List As List(Of String) = New List(Of String)()
                    Check_SN_List = lstWriteBits.Intersect(lstWriteBits_New).ToList
                    '需檢查通過 才可以下一個階段 結束

                    '比較數量是否減少
                    If lstWriteBits.Count = lstWriteBits_New.Count Then
                        If sFullFileName <> "over" Then
                            If sFullFileName <> "format" Then

                                If sFullFileName = "" Then
                                    Throw New ArgumentException(_oLanguage.getText("Common", "042", ctlLanguage.eumType.Tag))
                                End If

                                Dim txtFileName_Check As String = Me.html_FileUpload.FileName

                                If txtFileName_Check = "" Then
                                    Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("Common", "077", ctlLanguage.eumType.Tag) & "');  </script>"
                                    Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                                Else

                                    Dim dir As String = _Requested_Upload_FilePath

                                    Dim FILE_NAME As String = _Requested_Upload_FilePath & sFullFileName
                                    Dim TextLine As String

                                    '客戶設定檔案
                                    Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable

                                    Dim ctlCustomer_Customer As New ctlCustomer.Customer
                                    dtvwCustomer = ctlCustomer_Customer.QueryByCompany(Session("_CustomerID"))

                                    If System.IO.File.Exists(FILE_NAME) = True Then
                                        Dim objReader As New System.IO.StreamReader(FILE_NAME, System.Text.Encoding.Default)
                                        Do While objReader.Peek() <> -1
                                            TextLine = objReader.ReadLine()
                                            Dim arrText() As String = TextLine.Split(",")

                                            If arrText.Length > 0 Then
                                                If arrText(0).ToString().Trim().ToLower <> "Serial Number".ToLower() Then

                                                    Dim sSerial As String = arrText(0).ToString().Trim().ToUpper()        'Serial No


                                                    Dim sModel As String = arrText(1).ToString().Trim().ToUpper()            'Model No

                                                    '20240411  會儲存但不呈現 下依序-2-->excel sample中可否直接拉掉那兩欄，不然怕客戶填錯 開始
                                                    'Dim sProductDesc As String = arrText(2).ToString().Trim()      '產品敘述
                                                    'Dim sCusName As String = arrText(3).ToString().Trim()         'Customer Product Name
                                                    Dim sProductDesc As String = ""      '產品敘述
                                                    Dim sCusName As String = ""         'Customer Product Name
                                                    Dim sFarFarcNo As String = arrText(2).ToString().Trim()       '關聯 FailureReasons.RMAD_FAR_FARCNO-->不良原因類別代碼
                                                    Dim sFarNo As String = arrText(3).ToString().Trim()           '關聯 FailureReasons.FAR_NO-->不良原因代碼
                                                    Dim sProblemDesc As String = arrText(4).ToString().Trim()     'Problem Description
                                                    Dim CUSTOMER_PRODUCT_NUMBER As String = arrText(5).ToString().Trim()
                                                    '20240411  會儲存但不呈現-->excel sample中可否直接拉掉那兩欄，不然怕客戶填錯 結束


                                                    Dim sWarranty As String = ""        '保固日期
                                                    Dim sPartSn As String = ""        '零件序號

                                                    Dim sQuerySN As String = ""
                                                    Dim ctlReport As New ctlReport
                                                    Dim dtReport As New ReportDTO.Rpt_RMAWarrantyDataTable


                                                    dtReport = ctlReport.QueryRMAWarranty(Session("_CustomerID").ToString().Trim(), sSerial)

                                                    If dtReport.Rows.Count > 0 Then
                                                        sQuerySN = dtReport.Rows(0)("EXPORT_SERIALNO").ToString()
                                                    End If

                                                    If sQuerySN <> "" Then
                                                        If sQuerySN <> sSerial Then
                                                            sPartSn = sSerial '輸入的零件序號
                                                            sSerial = sQuerySN '查詢得到主機序號
                                                        End If
                                                    End If


                                                    '檢核 Serial No 是否有重複
                                                    If sSerial.Trim() <> "" Then
                                                        dtRMADetail.DefaultView.RowFilter = "RMAD_SERIALNO='" + sSerial.Trim() + "' AND RMAD_MARK<>'1'"
                                                        If dtRMADetail.DefaultView.Count > 0 Then
                                                            Dim sErrorMsg As String = _oLanguage.getText("RMA", "230", ctlLanguage.eumType.Validator)
                                                            Throw New ArgumentException(sErrorMsg)
                                                            'Continue Do
                                                        End If
                                                    Else
                                                        Dim sErrorMsg As String = _oLanguage.getText("RMA", "108", ctlLanguage.eumType.Validator)
                                                        Throw New ArgumentException(sErrorMsg)
                                                    End If



                                                    'Warranty
                                                    If sSerial.ToString().Trim() <> "" Then
                                                        'Dim sWarrantyDate As String = oExport.getWarranty(sSerial, Session("_CustomerID").ToString())
                                                        Dim sWarrantyDate As String = ""
                                                        If dtvwCustomer.Rows(0)("CU_ISCHOICE").ToString().Trim() = "1" Then
                                                            sWarrantyDate = oExport.getMaxWarranty(sSerial.ToUpper(), Session("_CustomerID").ToString(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim())
                                                        Else
                                                            sWarrantyDate = oExport.getMaxWarranty(sSerial.ToUpper(), Session("_CustomerID").ToString(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim())
                                                        End If

                                                        If sWarrantyDate.Trim() <> "" Then
                                                            sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                                                        End If
                                                    Else
                                                        sWarranty = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                                                    End If

                                                    'ModelNo
                                                    If sSerial.Trim() <> "" Then
                                                        'Dim sModelNo As String = oExport.getModelNo(sSerial.Trim())
                                                        Dim sModelNo As String = oExport.getModelNo(sSerial.Trim(), dtvwCustomer.Rows(0)("CU_CSTMP").ToString().Trim(), Session("_CustomerID").ToString())
                                                        If sModelNo.Trim() = "" Then
                                                            sModel = "OTHER"
                                                        Else
                                                            sModel = sModelNo
                                                        End If
                                                    Else
                                                        If sModel.Trim() = "" Then
                                                            sModel = "OTHER"
                                                        Else
                                                            Dim oModel As New ctlRMA.Model
                                                            If oModel.chkModel(sModel.Trim()) = False Then
                                                                sModel = "OTHER"
                                                            End If
                                                        End If
                                                    End If

                                                    'FarFarcNO
                                                    If sFarFarcNo.Trim() = "" Then
                                                        sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                        sFarNo = -1

                                                    Else
                                                        Dim oFailure As New ctlFailure

                                                        If sFarFarcNo.Trim() <> "" And sFarNo.Trim() <> "" Then
                                                            If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim(), sFarNo.Trim()) = False Then
                                                                sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                                sFarNo = -1
                                                            End If
                                                        Else

                                                            If sFarFarcNo.Trim() <> "" And sFarNo.Trim() = "" Then
                                                                If oFailure.chkFailure(Session("_LanguageID"), sFarFarcNo.Trim()) = False Then
                                                                    sFarFarcNo = _Requested_Default_FarFarcNO.Trim()
                                                                    sFarNo = -1
                                                                End If
                                                            End If
                                                        End If

                                                    End If

                                                    If IsNothing(Session("_dtRMADetail")) = True Then
                                                        dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn, CUSTOMER_PRODUCT_NUMBER)
                                                    Else
                                                        dtRMADetail = Session("_dtRMADetail")
                                                        dtRMADetail = Execl_AddRMADetail(dtRMADetail, sSerial, sModel, sCusName, sWarranty, sFarFarcNo, sFarNo, sProblemDesc, sProductDesc, sPartSn, CUSTOMER_PRODUCT_NUMBER)
                                                    End If

                                                End If
                                            End If
                                        Loop
                                        objReader.Close()
                                    End If
                                    Session("_eumCommand") = 1
                                    Session("_dtRMADetail") = dtRMADetail

                                    blnFlag = True
                                    '直接新增維修單明細
                                    Dim sScript As String = "<script type=""text/javascript"">   window.location = 'ProductInformation_03.aspx'; window.parent.Close_windows(); window.parent.Show_windows(); </script>"
                                    Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                                End If

                            Else
                                Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "118", ctlLanguage.eumType.Tag) & "');  </script>"
                                Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                            End If
                        Else
                            Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "118", ctlLanguage.eumType.Tag) & "');  </script>"
                            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)

                        End If
                    Else
                        Me.ucMessage.showMessageByFailed(_oLanguage.getText("RMA2", "124", ctlLanguage.eumType.Tag) & "</br>" & Check_SN_List(1).ToString())
                    End If

                Catch ex As Exception
                    sMessage = ex.Message
                    blnFlag = False
                    Dim sScript As String = "<script type=""text/javascript"">   alert('" & sMessage & "');  </script>"
                    Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)
                Finally

                End Try


            Else

                Dim Requestedes As New ctlRMA.Requested
                Dim dt As New DataTable
                dt = Requestedes.QueryByRMA_SERIALNO(SerialNumberTxt.Text.Trim())
                Dim index As Integer = Convert.ToInt32(dt.Rows(0)(0).ToString().Trim())
                '24小時序號 關閉
                index = 0
                If index <= 0 Then

                    Response.Redirect("ProductInformation_02.aspx?SerialNumber=" & SerialNumberTxt.Text.Trim())
                Else

                    Dim sScript As String = "<script type=""text/javascript"">   alert('" & _oLanguage.getText("RMA2", "113", ctlLanguage.eumType.Tag).Replace("XXXXXXXXXXXXX", SerialNumberTxt.Text.Trim()) & "');  </script>"
                    Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType(), "alertMsg", sScript)
                End If
            End If
        Else
            Me.ucMessage.showMessageByFailed("<span  style='color:red;' >" & _oLanguage.getText("RMA", "241", ctlLanguage.eumType.Validator) & "</span>")
        End If
        '檢查 24小時 序號 結束
    End Sub

    Private Function UpLoadFile() As String
        Dim fileSize As Integer = html_FileUpload.PostedFile.ContentLength
        Dim maxRequestLength As Integer = 5242880 'Number Of Bytes (5MB)

        Dim ext As String = System.IO.Path.GetExtension(html_FileUpload.FileName)
        Dim retval As String = ""
        If ext = ".csv" Then

            If fileSize < maxRequestLength Then

                Try
                    Dim txtFileName As String = Me.html_FileUpload.FileName

                    '****** 取得檔名 **********
                    Dim FileSplit() As String = Split(txtFileName, "\")
                    Dim FileName As String = FileSplit(FileSplit.Length - 1)

                    '****** 取得副檔名 **********
                    Dim auxFileSplit() As String = Split(FileName, ".")
                    Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
                    Dim sFileNameChange As String = ""
                    Dim oCommon As New Common
                    sFileNameChange = oCommon.GetRandomizeNum()
                    sFileNameChange = sFileNameChange & "." & auxFileName

                    '***** 檔案(原檔名,亂數檔名) *****
                    Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

                    Me.html_FileUpload.SaveAs(_Requested_Upload_FilePath & sFileNameChange)
                    'Me.html_FileUpload.MoveTo(_Requested_Upload_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
                    retval = sFileNameChange

                Catch ex As Exception
                    Throw ex
                    retval = ""

                End Try
            Else
                retval = "over"
            End If

        Else
            retval = "format"
        End If
        Return retval
    End Function

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <param name="sSerial"></param>
    ''' <param name="sModel"></param>
    ''' <param name="sCusName"></param>
    ''' <param name="sWarranty"></param>
    ''' <param name="sFarFarcNo"></param>
    ''' <param name="sFarNo"></param>
    ''' <param name="sProblemDesc"></param>
    ''' <param name="sProductDesc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Execl_AddRMADetail(ByVal dtRMADetail As RmaDTO.RMADetailDataTable, ByVal sSerial As String, ByVal sModel As String, ByVal sCusName As String, ByVal sWarranty As String,
                                       ByVal sFarFarcNo As String, ByVal sFarNo As String, ByVal sProblemDesc As String, ByVal sProductDesc As String, ByVal sPartSN As String, ByVal CUSTOMER_PRODUCT_NUMBER As String) As RmaDTO.RMADetailDataTable

        Dim dr As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow
        Dim oExport As New ctlRMA.Export

        Try
            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            dr.RMAD_ID = sGUID.ToString().Trim()
            dr.RMAD_SEQ = 0
            dr.RMAD_RMANO = ""
            dr.RMAD_MODELNO = sModel.Trim()
            dr.RMAD_SERIALNO = sSerial.Trim()
            dr.RMAD_CUSNAME = sCusName.Trim()                   'Customer Product Name
            dr.RMAD_sWARRANTY = sWarranty                       'Warranty 字串格式

            dr.RMAD_FARFARCNO = sFarFarcNo.Trim()
            dr.RMAD_FARNO = sFarNo.Trim()
            dr.RMAD_PARTSN = sPartSN.Trim()
            dr.RMAD_UPLOADFILE = ""
            dr.RMAD_PRODUCTDESC = sProblemDesc.Trim()
            dr.RMAD_PROBLEMDESC = sProductDesc.Trim()
            dr.RMAD_STATUS = 0

            dr.RMAD_AD = Session("_UserID")
            dr.RMAD_ADNAME = Session("_UserName")
            dr.RMAD_CSTMP = Date.Now
            dr.RMAD_LUAD = Session("_UserID")
            dr.RMAD_LUADNAME = Session("_UserName")
            dr.RMAD_LUSTMP = Date.Now
            dr.RMAD_MARK = 0

            dr.RMAD_ISFILL = 1             '是否已填寫問題:0.否, 1.是
            dr.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            If CUSTOMER_PRODUCT_NUMBER.Length > 20 Then
                dr.CUSTOMER_PRODUCT_NUMBER = CUSTOMER_PRODUCT_NUMBER.Trim().Substring(0, 20)
            Else
                dr.CUSTOMER_PRODUCT_NUMBER = CUSTOMER_PRODUCT_NUMBER.Trim()
            End If

            dtRMADetail.AddRMADetailRow(dr)

        Catch ex As Exception
            Throw ex

        End Try

        Return dtRMADetail
    End Function

    Protected Sub UI_cmdAgree_Click(sender As Object, e As EventArgs) Handles UI_cmdAgree.Click

        Try
            Me.Panel1.Visible = False
            Me.Panel2.Visible = True
            Session("_isPolicy") = True     '紀錄是否有顯示過Policy
        Catch ex As Exception
            Throw ex

        End Try

    End Sub
End Class

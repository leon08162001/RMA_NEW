Imports Newtonsoft.Json

Partial Class CheckingReport
    Inherits System.Web.UI.Page

    Dim _Requested_Upload_FilePath As String = ConfigurationSettings.AppSettings("Requested_VisualPath")
    Dim WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")
    Public canvas As String = ""

    Public Class UpData

        Private iPath As String

        Public Property Path() As String
            Get
                ' Gets the property value.
                Return iPath
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iPath = Value
            End Set
        End Property
    End Class


    Public Class Details

        Private iconfirm As String

        Public Property confirm() As String
            Get
                ' Gets the property value.
                Return iconfirm
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iconfirm = Value
            End Set
        End Property

        Private ichoose As String

        Public Property choose() As String
            Get
                ' Gets the property value.
                Return ichoose
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                ichoose = Value
            End Set
        End Property

        Private iRemark As String

        Public Property Remark() As String
            Get
                ' Gets the property value.
                Return iRemark
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iRemark = Value
            End Set
        End Property

    End Class


    Public Class Test_OK_Report

        Private iCustomerID As String

        Public Property CustomerID() As String
            Get
                ' Gets the property value.
                Return iCustomerID
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iCustomerID = Value
            End Set
        End Property


        Private iCustomerName As String

        Public Property CustomerName() As String
            Get
                ' Gets the property value.
                Return iCustomerName
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iCustomerName = Value
            End Set
        End Property

        Private iRMA_Number As String

        Public Property RMA_Number() As String
            Get
                ' Gets the property value.
                Return iRMA_Number
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iRMA_Number = Value
            End Set
        End Property

        Private iSerial_Number As String

        Public Property Serial_Number() As String
            Get
                ' Gets the property value.
                Return iSerial_Number
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iSerial_Number = Value
            End Set
        End Property

        Private iPower_on As Details

        Public Property Power_on() As Details
            Get
                ' Gets the property value.
                Return iPower_on
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iPower_on = Value
            End Set
        End Property

        Private iCharger_Test As Details

        Public Property Charger_Test() As Details
            Get
                ' Gets the property value.
                Return iCharger_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iCharger_Test = Value
            End Set
        End Property

        Private iDisplay_Test As Details

        Public Property Display_Test() As Details
            Get
                ' Gets the property value.
                Return iDisplay_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iDisplay_Test = Value
            End Set
        End Property

        Private iIndicator_light_Test As Details

        Public Property Indicator_light_Test() As Details
            Get
                ' Gets the property value.
                Return iIndicator_light_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iIndicator_light_Test = Value
            End Set
        End Property

        Private iKeypad_Test As Details

        Public Property Keypad_Test() As Details
            Get
                ' Gets the property value.
                Return iKeypad_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iKeypad_Test = Value
            End Set
        End Property

        Private iReader_Test As Details

        Public Property Reader_Test() As Details
            Get
                ' Gets the property value.
                Return iReader_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iReader_Test = Value
            End Set
        End Property

        Private iMemory_Test As Details

        Public Property Memory_Test() As Details
            Get
                ' Gets the property value.
                Return iMemory_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iMemory_Test = Value
            End Set
        End Property

        Private iCamera_Test As Details

        Public Property Camera_Test() As Details
            Get
                ' Gets the property value.
                Return iCamera_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iCamera_Test = Value
            End Set
        End Property

        Private iSound_Test As Details

        Public Property Sound_Test() As Details
            Get
                ' Gets the property value.
                Return iSound_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iSound_Test = Value
            End Set
        End Property

        Private iInterface_Test As Details

        Public Property Interface_Test() As Details
            Get
                ' Gets the property value.
                Return iInterface_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iInterface_Test = Value
            End Set
        End Property

        Private iSD_Card_Test As Details

        Public Property SD_Card_Test() As Details
            Get
                ' Gets the property value.
                Return iSD_Card_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iSD_Card_Test = Value
            End Set
        End Property

        Private iNFC_Test As Details

        Public Property NFC_Test() As Details
            Get
                ' Gets the property value.
                Return iNFC_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iNFC_Test = Value
            End Set
        End Property

        Private iRFID_Test As Details

        Public Property RFID_Test() As Details
            Get
                ' Gets the property value.
                Return iRFID_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iRFID_Test = Value
            End Set
        End Property

        Private iGMS_Test As Details
        Public Property GMS_Test() As Details
            Get
                ' Gets the property value.
                Return iGMS_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iGMS_Test = Value
            End Set
        End Property

        Private iWireless_Test As Details
        Public Property Wireless_Test() As Details
            Get
                ' Gets the property value.
                Return iWireless_Test
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iWireless_Test = Value
            End Set
        End Property

        Private iFirmware_Version As Details

        Public Property Firmware_Version() As Details
            Get
                ' Gets the property value.
                Return iFirmware_Version
            End Get
            Set(ByVal Value As Details)
                ' Sets the property value.
                iFirmware_Version = Value
            End Set
        End Property

        Private iTest_Method As String

        Public Property Test_Method() As String
            Get
                ' Gets the property value.
                Return iTest_Method
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iTest_Method = Value
            End Set
        End Property

        Private iOther As String

        Public Property Other() As String
            Get
                ' Gets the property value.
                Return iOther
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iOther = Value
            End Set
        End Property

        Private iMaintenance As String

        Public Property Maintenance() As String
            Get
                ' Gets the property value.
                Return iMaintenance
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iMaintenance = Value
            End Set
        End Property

        Private iDate As String

        Public Property Date_() As String
            Get
                ' Gets the property value.
                Return iDate
            End Get
            Set(ByVal Value As String)
                ' Sets the property value.
                iDate = Value
            End Set
        End Property

        Private iUpData As List(Of UpData)

        Public Property UpData() As List(Of UpData)
            Get
                ' Gets the property value.
                Return iUpData
            End Get
            Set(ByVal Value As List(Of UpData))
                ' Sets the property value.
                iUpData = Value
            End Set
        End Property

    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Call Initialization()

        End If

    End Sub

    ''' <summary>
    ''' 上傳資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdFileAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdFileAdd.Click

        Dim path As String = UpLoadFile()
        File_HiddenField.Value = path & "," & path
        html_File_.Value = path & "," & path
        Dim img As String = ""
        img += "<div Class='col-lg-4 mb-4 mb-lg-0'>"
        img += "<div Class='bg-image hover-overlay ripple shadow-1-strong rounded' data-ripple-color='light' >"

        img += "<img src ='" & WEBURL & _Requested_Upload_FilePath & path & "'         class='w-100' >"
        img += "<a href ='#!' data-mdb-toggle='modal' data-mdb-target='#exampleModal2'>"
        img += "<div Class='mask' style='background-color: rgba(251, 251, 251, 0.2);'></div>"
        img += "</a>"
        img += "</div>"
        img += "</div>"
        Me.UploadLabel.Text = Me.UploadLabel.Text & img
        canvas = Me.UploadLabel.Text

        Me.UploadLabel.Visible = False

        FileHiddenField.Value = FileHiddenField.Value & path & "|"


    End Sub

    Public Function DisplayStr() As String
        Return Me.UploadLabel.Text
    End Function

    Public Function Initialization()

        Me.Date_Lab.Text = DateTime.Now.ToString("yyyy/MM/dd")

        If Session("_UserName") Is Nothing Then

        Else
            Me.Maintenance_Lab.Text = Session("_UserName").ToString().Trim()
        End If

    End Function

    Private Function UpLoadFile() As String
        Dim retval As String = ""

        Try
            Dim txtFileName As String = Me.FileUpload1.FileName

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

            Me.FileUpload1.SaveAs(Server.MapPath(_Requested_Upload_FilePath) & sFileNameChange)
            'Me.html_FileUpload.MoveTo(_Requested_Upload_FilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)
            retval = sFileNameChange

        Catch ex As Exception
            Throw ex
            retval = ""

        End Try

        Return retval
    End Function

    Protected Sub SaveBtn_Click(sender As Object, e As EventArgs) Handles SaveBtn.Click

        Try

            Dim Test_OK_Report_List As New Test_OK_Report

            'Power_on
            Dim Power_on_Details As New Details
            Power_on_Details.confirm = Me.Power_on_check.Enabled.ToString().Trim()
            If Me.Power_on_Y.Enabled = True & Me.Power_on_N.Enabled = False Then
                Power_on_Details.choose = "Y"
            End If
            If Me.Power_on_Y.Enabled = False & Me.Power_on_N.Enabled = True Then
                Power_on_Details.choose = "N"
            End If
            Power_on_Details.Remark = Me.Power_on_note.Text.Trim()
            Test_OK_Report_List.Power_on = Power_on_Details

            'Charger_Test
            Dim Charger_Test_Details As New Details
            Charger_Test_Details.confirm = Me.Charger_Test_check.Enabled.ToString().Trim()
            If Me.Charger_Test_Y.Enabled = True & Me.Charger_Test_N.Enabled = False Then
                Charger_Test_Details.choose = "Y"
            End If
            If Me.Charger_Test_Y.Enabled = False & Me.Charger_Test_N.Enabled = True Then
                Charger_Test_Details.choose = "N"
            End If
            Charger_Test_Details.Remark = Me.Charger_Test_note.Text.Trim()
            Test_OK_Report_List.Charger_Test = Charger_Test_Details


            'Display_Test
            Dim Display_Test_Details As New Details
            Display_Test_Details.confirm = Me.Display_Test_check.Enabled.ToString().Trim()
            If Me.Display_Test_Y.Enabled = True & Me.Display_Test_N.Enabled = False Then
                Display_Test_Details.choose = "Y"
            End If
            If Me.Display_Test_Y.Enabled = False & Me.Display_Test_N.Enabled = True Then
                Display_Test_Details.choose = "N"
            End If
            Display_Test_Details.Remark = Me.Display_Test_note.Text.Trim()
            Test_OK_Report_List.Display_Test = Display_Test_Details

            'Indicator_light_Test
            Dim Indicator_light_Test_Details As New Details
            Indicator_light_Test_Details.confirm = Me.Indicator_light_Test_check.Enabled.ToString().Trim()
            If Me.Indicator_light_Test_Y.Enabled = True & Me.Indicator_light_Test_N.Enabled = False Then
                Indicator_light_Test_Details.choose = "Y"
            End If
            If Me.Indicator_light_Test_Y.Enabled = False & Me.Indicator_light_Test_N.Enabled = True Then
                Indicator_light_Test_Details.choose = "N"
            End If
            Indicator_light_Test_Details.Remark = Me.Indicator_light_Test_note.Text.Trim()
            Test_OK_Report_List.Display_Test = Indicator_light_Test_Details

            'Keypad_Test
            Dim Keypad_Test_Details As New Details
            Keypad_Test_Details.confirm = Me.Keypad_Test_check.Enabled.ToString().Trim()
            If Me.Keypad_Test_Y.Enabled = True & Me.Keypad_Test_N.Enabled = False Then
                Keypad_Test_Details.choose = "Y"
            End If
            If Me.Keypad_Test_Y.Enabled = False & Me.Keypad_Test_N.Enabled = True Then
                Keypad_Test_Details.choose = "N"
            End If
            Keypad_Test_Details.Remark = Me.Keypad_Test_note.Text.Trim()
            Test_OK_Report_List.Keypad_Test = Keypad_Test_Details


            'Reader_Test
            Dim Reader_Test_Details As New Details
            Reader_Test_Details.confirm = Me.Reader_Test_check.Enabled.ToString().Trim()
            If Me.Reader_Test_Y.Enabled = True & Me.Reader_Test_N.Enabled = False Then
                Reader_Test_Details.choose = "Y"
            End If
            If Me.Reader_Test_Y.Enabled = False & Me.Reader_Test_N.Enabled = True Then
                Reader_Test_Details.choose = "N"
            End If
            Reader_Test_Details.Remark = Me.Reader_Test_note.Text.Trim()
            Test_OK_Report_List.Reader_Test = Reader_Test_Details


            'Memory_Test
            Dim Memory_Test_Details As New Details
            Memory_Test_Details.confirm = Me.Memory_Test_check.Enabled.ToString().Trim()
            If Me.Memory_Test_Y.Enabled = True & Me.Memory_Test_N.Enabled = False Then
                Memory_Test_Details.choose = "Y"
            End If
            If Me.Memory_Test_Y.Enabled = False & Me.Memory_Test_N.Enabled = True Then
                Memory_Test_Details.choose = "N"
            End If
            Memory_Test_Details.Remark = Me.Memory_Test_note.Text.Trim()
            Test_OK_Report_List.Memory_Test = Memory_Test_Details

            'Camera_Test
            Dim Camera_Test_Details As New Details
            Camera_Test_Details.confirm = Me.Camera_Test_check.Enabled.ToString().Trim()
            If Me.Camera_Test_Y.Enabled = True & Me.Camera_Test_N.Enabled = False Then
                Camera_Test_Details.choose = "Y"
            End If
            If Me.Camera_Test_Y.Enabled = False & Me.Camera_Test_N.Enabled = True Then
                Camera_Test_Details.choose = "N"
            End If
            Camera_Test_Details.Remark = Me.Camera_Test_note.Text.Trim()
            Test_OK_Report_List.Camera_Test = Camera_Test_Details


            'Sound_Test
            Dim Sound_Test_Details As New Details
            Sound_Test_Details.confirm = Me.Sound_Test_check.Enabled.ToString().Trim()
            If Me.Sound_Test_Y.Enabled = True & Me.Sound_Test_N.Enabled = False Then
                Sound_Test_Details.choose = "Y"
            End If
            If Me.Sound_Test_Y.Enabled = False & Me.Sound_Test_N.Enabled = True Then
                Sound_Test_Details.choose = "N"
            End If
            Sound_Test_Details.Remark = Me.Sound_Test_note.Text.Trim()
            Test_OK_Report_List.Sound_Test = Sound_Test_Details

            'Interface_Test
            Dim Interface_Test_Details As New Details
            Interface_Test_Details.confirm = Me.Interface_Test_check.Enabled.ToString().Trim()
            Interface_Test_Details.choose = Me.Interface_Test_check.Enabled.ToString().Trim()
            If Me.Interface_Test_Y.Enabled = True & Me.Interface_Test_N.Enabled = False Then
                Interface_Test_Details.choose = "Y"
            End If
            If Me.Interface_Test_Y.Enabled = False & Me.Interface_Test_N.Enabled = True Then
                Interface_Test_Details.choose = "N"
            End If
            Interface_Test_Details.Remark = Me.Interface_Test_note.Text.Trim()
            Test_OK_Report_List.Interface_Test = Interface_Test_Details


            'SD_Card_Test
            Dim SD_Card_Test_Details As New Details
            SD_Card_Test_Details.confirm = Me.SD_Card_Test_check.Enabled.ToString().Trim()
            SD_Card_Test_Details.choose = Me.SD_Card_Test_check.Enabled.ToString().Trim()
            If Me.SD_Card_Test_Y.Enabled = True & Me.SD_Card_Test_N.Enabled = False Then
                SD_Card_Test_Details.choose = "Y"
            End If
            If Me.SD_Card_Test_Y.Enabled = False & Me.SD_Card_Test_N.Enabled = True Then
                SD_Card_Test_Details.choose = "N"
            End If
            SD_Card_Test_Details.Remark = Me.SD_Card_Test_note.Text.Trim()
            Test_OK_Report_List.SD_Card_Test = SD_Card_Test_Details


            'NFC_Test
            Dim NFC_Test_Details As New Details
            NFC_Test_Details.confirm = Me.NFC_Test_check.Enabled.ToString().Trim()
            NFC_Test_Details.choose = Me.NFC_Test_check.Enabled.ToString().Trim()
            If Me.NFC_Test_Y.Enabled = True & Me.NFC_Test_N.Enabled = False Then
                NFC_Test_Details.choose = "Y"
            End If
            If Me.NFC_Test_Y.Enabled = False & Me.NFC_Test_N.Enabled = True Then
                NFC_Test_Details.choose = "N"
            End If
            NFC_Test_Details.Remark = Me.NFC_Test_note.Text.Trim()
            Test_OK_Report_List.NFC_Test = NFC_Test_Details


            'RFID_Test
            Dim RFID_Test_Details As New Details
            RFID_Test_Details.confirm = Me.RFID_Test_check.Enabled.ToString().Trim()
            RFID_Test_Details.choose = Me.RFID_Test_check.Enabled.ToString().Trim()
            If Me.RFID_Test_Y.Enabled = True & Me.RFID_Test_N.Enabled = False Then
                RFID_Test_Details.choose = "Y"
            End If
            If Me.RFID_Test_Y.Enabled = False & Me.RFID_Test_N.Enabled = True Then
                RFID_Test_Details.choose = "N"
            End If
            RFID_Test_Details.Remark = Me.RFID_Test_note.Text.Trim()
            Test_OK_Report_List.RFID_Test = RFID_Test_Details


            'GMS_Test
            Dim GMS_Test_Details As New Details
            GMS_Test_Details.confirm = Me.GMS_Test_check.Enabled.ToString().Trim()
            GMS_Test_Details.choose = Me.GMS_Test_check.Enabled.ToString().Trim()
            If Me.GMS_Test_Y.Enabled = True & Me.GMS_Test_N.Enabled = False Then
                GMS_Test_Details.choose = "Y"
            End If
            If Me.GMS_Test_Y.Enabled = False & Me.GMS_Test_N.Enabled = True Then
                GMS_Test_Details.choose = "N"
            End If
            GMS_Test_Details.Remark = Me.GMS_Test_note.Text.Trim()
            Test_OK_Report_List.GMS_Test = GMS_Test_Details

            'Wireless_Test
            Dim Wireless_Test_Details As New Details
            Wireless_Test_Details.confirm = Me.Wireless_Test_check.Enabled.ToString().Trim()
            Wireless_Test_Details.choose = Me.Wireless_Test_check.Enabled.ToString().Trim()
            If Me.Wireless_Test_Y.Enabled = True & Me.Wireless_Test_N.Enabled = False Then
                Wireless_Test_Details.choose = "Y"
            End If
            If Me.Wireless_Test_Y.Enabled = False & Me.Wireless_Test_N.Enabled = True Then
                Wireless_Test_Details.choose = "N"
            End If
            Wireless_Test_Details.Remark = Me.Wireless_Test_note.Text.Trim()
            Test_OK_Report_List.Wireless_Test = Wireless_Test_Details

            'Firmware_Version
            Dim Firmware_Version_Details As New Details
            Firmware_Version_Details.confirm = Me.Firmware_Version_check.Enabled.ToString().Trim()
            Firmware_Version_Details.choose = Me.Firmware_Version_check.Enabled.ToString().Trim()
            If Me.Firmware_Version_Y.Enabled = True & Me.Firmware_Version_N.Enabled = False Then
                Firmware_Version_Details.choose = "Y"
            End If
            If Me.Firmware_Version_Y.Enabled = False & Me.Firmware_Version_N.Enabled = True Then
                Firmware_Version_Details.choose = "N"
            End If
            Firmware_Version_Details.Remark = Me.Firmware_Version_note.Text.Trim()
            Test_OK_Report_List.Firmware_Version = Firmware_Version_Details

            Test_OK_Report_List.Other = Me.Other_Txt.Text.Trim()

            Test_OK_Report_List.Test_Method = Me.Test_Method_Txt.Text.Trim()

            Test_OK_Report_List.Maintenance = Me.Maintenance_Lab.Text.Trim()
            Test_OK_Report_List.Date_ = Me.Date_Lab.Text.Trim()

            Dim file_string As String() = FileHiddenField.Value.Split("|")

            Dim UpData_List As New List(Of UpData)

            For Each s As String In file_string
                Dim myUpData As New UpData
                myUpData.Path = s

                UpData_List.Add(myUpData)
            Next

            Test_OK_Report_List.UpData = UpData_List

            Dim _SnData As String = JsonConvert.SerializeObject(Test_OK_Report_List)

        Catch ex As Exception
            Throw ex


        End Try

    End Sub

    Protected Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click

        Try


        Catch ex As Exception
            Throw ex


        End Try



    End Sub

End Class

Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage

Public Class Common
    Dim _oLanguage As New ctlLanguage
    Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
    Dim _RepairNo_flowCase02 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase02")
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    Enum enmRMAstatus As Integer
        RequestedTmp = 0
        Requested = 10
        inProgress = 20
        Closed = 90
        Canceled = 91
    End Enum
    Enum enmRMADstatus As Integer
        RequestedTmp = 0
        Requested = 10
        Received = 20
        RepairQuoted = 30
        SalesQuoted = 35
        SalesConfirmed = 40
        CustomerConfirmed = 50
        Repaired = 60
        Shipped = 70
        Closed = 90
        Canceled = 91
    End Enum

    Enum eumType As Integer
        Tag = 1
        Validator = 2
    End Enum

    Enum enmMessage As Integer
        AddOK = 1
        EditOK = 2
        DelOK = 3
        MailOK = 4
        ConfirmOK = 5
        ChangePassWordOK = 6
        ProcessOK = 7
    End Enum
    Enum enmRole As Integer
        View = 0
        Receiver = 1
        Repair = 2
        Sales = 3
        Shipping = 4
        Admin = 9
    End Enum

    ''' <summary>
    ''' 列舉-維修中心(Comp_No)
    ''' </summary>
    Enum enmRepairCenter
        <Description("AU")>
        AU
        <Description("AUS")>
        AUS
        <Description("AU_LAPTOP_KINGS")>
        CL_AU_Service_Center
        <Description("AC_GMBH")>
        CL_GMBH_Service_Center
        <Description("CLHQ")>
        CL_TW_Service_Center
        <Description("CEAT")>
        CL_EU_Service_Center
        <Description("CL_CHINA")>
        CL_CN_Service_Center
        <Description("CL_USA")>
        CL_US_Service_center
        <Description("UK_FALA")>
        CL_UK_Service_Centre
        <Description("JP_BYTE")>
        CL_JP_Service_Center
        <Description("JP_BYTE_MPLUS")>
        MPlus_JP_Service_Center
        <Description("NZ_PB_TECH")>
        CL_NZ_Service_Center
        <Description("US_CL_MPLUS")>
        MPlus_US_Service_Center
        <Description("US_CL_CLUSA")>
        CU_US_Service_center
        <Description("JRC")>
        CL_JRC_Service_Center
    End Enum

    ''' <summary>
    ''' 列舉轉換 Description
    ''' </summary>
    ''' <returns></returns>

    Public Shared Function GetDescriptionText(Of T)(source As T) As String
        Dim fi As FieldInfo = source.GetType().GetField(source.ToString())
        Dim attributes As DescriptionAttribute() = CType(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        If attributes.Length > 0 Then
            Return attributes(0).Description
        Else
            Return source.ToString()
        End If
    End Function

    ''' <summary>
    ''' 轉換 RMA 單頭 Status 文字格式
    ''' </summary>
    ''' <param name="Status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertToStatusText(ByVal Status As Integer) As String
        Dim retval As String = ""

        '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
        Select Case Status
            Case 0
                retval = _oLanguage.getText("Common", "062", ctlLanguage.eumType.Status)
            Case 10
                retval = _oLanguage.getText("Common", "026", ctlLanguage.eumType.Status)
            Case 20
                retval = _oLanguage.getText("Common", "034", ctlLanguage.eumType.Status)
            Case 90
                retval = _oLanguage.getText("Common", "032", ctlLanguage.eumType.Status)
            Case 91
                retval = _oLanguage.getText("Common", "033", ctlLanguage.eumType.Status)
        End Select

        Return retval
    End Function

    ''' <summary>
    ''' 轉換 RMA 單身 Status 文字格式
    ''' </summary>
    ''' <param name="Status">RMA 單身狀態</param>
    ''' <param name="RMAD_ID">RMA 單身編號</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertToItemStatusText(ByVal Status As Integer, ByVal RMAD_ID As String) As String
        Dim retval As String = ""

        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
        Select Case Status
            Case 0
                retval = ""
            Case 10
                retval = _oLanguage.getText("Common", "026", ctlLanguage.eumType.Status) 'Requested
            Case 20
                retval = _oLanguage.getText("Common", "027", ctlLanguage.eumType.Status) 'Received

            Case 30 'WIP
                retval = _oLanguage.getText("Common", "066", ctlLanguage.eumType.Status) 'WIP

            Case 35
                retval = _oLanguage.getText("Common", "028", ctlLanguage.eumType.Status) 'Quoted

            Case 40
                retval = _oLanguage.getText("Common", "029", ctlLanguage.eumType.Status) 'Sales Quoted

            Case 50

                ' 檢核是否是 業務帶客戶確認
                Dim ctlSale As New ctlRMA.Sale
                If ctlSale.isSalesByClientConfirm(RMAD_ID.Trim()) = True Then
                    retval = _oLanguage.getText("Common", "072", ctlLanguage.eumType.Status)    '業務帶客戶確認 Sales Quoted
                Else
                    retval = _oLanguage.getText("Common", "071", ctlLanguage.eumType.Status)    '客戶確認 'Customer Confirmed
                End If


            Case 60
                retval = _oLanguage.getText("Common", "031", ctlLanguage.eumType.Status) 'Repaired

            Case 70
                retval = _oLanguage.getText("Common", "060", ctlLanguage.eumType.Status) 'Noticed

            Case 90
                retval = _oLanguage.getText("Common", "032", ctlLanguage.eumType.Status) 'Closed

            Case 91

                '2011/08/04 START
                'RMAD_RECEVSTATUS -->註解	是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                ''沒收貨刪除:Deleted, 已收貨刪除:Canceled
                Dim ctlReceived As New ctlRMA.Received
                If ctlReceived.isDeleted(RMAD_ID) = True Then
                    retval = _oLanguage.getText("Common", "082", ctlLanguage.eumType.Status)    'Deleted
                Else
                    retval = _oLanguage.getText("Common", "033", ctlLanguage.eumType.Status)    'Canceled
                End If
                '2011/08/04 END

        End Select

        Return retval
    End Function

    ''' <summary>
    ''' 取得FQA大類資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getFQAClassByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0

        Dim oListItem As ListItem
        Dim oFAQ As New ctlFAQ.FAQClass()
        Dim dtFAQClass As New FaqDTO.FAQClassDataTable

        dtFAQClass = oFAQ.QueryByEffective()
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtFAQClass.Rows.Count - 1
            Dim dr As FaqDTO.FAQClassRow = dtFAQClass.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.FAQC_CLASSNAME.ToString().Trim()
            oListItem.Value = dr.FAQC_ID.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得FQA小類資料
    ''' </summary>
    ''' <param name="FAQCID">傳入FAQ大類參考號</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getFQASubClassByDropDownList(ByVal FAQCID As String, ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0

        Dim oListItem As ListItem
        Dim oFAQ As New ctlFAQ.FAQSubClass
        Dim dtFAQSubClass As New FaqDTO.FAQSubClassDataTable

        dtFAQSubClass = oFAQ.QueryBYClass(FAQCID.ToString().Trim())
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtFAQSubClass.Rows.Count - 1
            Dim dr As FaqDTO.FAQSubClassRow = dtFAQSubClass.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.FAQSC_CLASSNAME.ToString().Trim()
            oListItem.Value = dr.FAQSC_ID.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得Program Type資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getProgramTypeByDropDownList(ByRef oDropList As DropDownList, ByVal WarrantyType As String, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        dtCompany = oCompany.QueryProgramType(WarrantyType)

        oDropList.Items.Clear()

        If TagText <> "" Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If


        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As DataRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr("PROGRAM_TYPE_NAME").ToString().Trim()
            oListItem.Value = dr("PROGRAM_TYPE").ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得Price Ver資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getPriceVerByDropDownList(ByRef oDropList As DropDownList, ByVal WarrantyType As String, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        dtCompany = oCompany.QueryPriceVer(WarrantyType)

        oDropList.Items.Clear()

        If TagText <> "" Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As DataRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr("PRICE_VER_NAME").ToString().Trim()
            oListItem.Value = dr("PRICE_VER").ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得Item Type資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getItemTypeByDropDownList(ByRef oDropList As DropDownList, ByVal WarrantyType As String, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        dtCompany = oCompany.QueryItemType(WarrantyType)

        oDropList.Items.Clear()

        If TagText <> "" Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As DataRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr("ITEM_TYPE_NAME").ToString().Trim()
            oListItem.Value = dr("ITEM_TYPE").ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得WARRSET Type資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getWarrsetTypeByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String, Optional ByVal IsAdd As Integer = 1, Optional ByVal sCustNO As String = "")
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        If IsAdd = 1 Then
            dtCompany = oCompany.QueryWarrsetType(True, sCustNO)
        Else
            dtCompany = oCompany.QueryWarrsetType(False, sCustNO)
        End If


        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As DataRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr("WARRSET_TYPE_NAME").ToString().Trim()
            oListItem.Value = dr("WARRSET_TYPE").ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得Warranty Status資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getWarrantyStatusByDropDownList(ByRef oDropList As DropDownList)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        oListItem = New ListItem
        oListItem.Text = "-Select-"
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "Open"
        oListItem.Value = "0"
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "Flow"
        oListItem.Value = "1"
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "Confirmed"
        oListItem.Value = "2"
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "Invalid"
        oListItem.Value = "3"
        oDropList.Items.Add(oListItem)

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得保固品名
    ''' </summary>
    ''' <param name="War_Group"></param>
    ''' <param name="WARRSET_TYPE"></param>
    ''' <param name="Program_Type"></param>
    ''' <param name="Item_Type"></param>
    ''' <param name="Price_Ver"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWarrsetPartNM(ByVal War_Group As String, ByVal WARRSET_TYPE As String, ByVal Program_Type As String, ByVal Item_Type As String, ByVal Price_Ver As String) As String
        Dim i As Integer = 0
        Dim oCompany As New ctlCompany

        Dim sPart_nm As String = oCompany.GetWarrsetPartNM(War_Group, WARRSET_TYPE, Program_Type, Item_Type, Price_Ver)

        Return sPart_nm

    End Function

    ''' <summary>
    ''' 取得RepairCenter資料
    ''' </summary>
    ''' <param name="isEffective">只取有效資料</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getRepairCenteryByDropDownList(ByVal isEffective As Boolean, ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        If isEffective = True Then
            dtCompany = oCompany.QueryByEffective()
        Else
            dtCompany = oCompany.QueryAll()
        End If

        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.COMP_NAME.ToString().Trim()
            oListItem.Value = dr.COMP_NO.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    Public Sub getCostCenterByDropDownList(ByVal isEffective As Boolean, ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oWarranty As New ctlCompany
        Dim dtCostCenter As New CompanyDTO.CompanyDataTable
        dtCostCenter = oWarranty.QueryByEffective("")

        oDropList.Items.Clear()
        If TagText <> "" Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If


        For i = 0 To dtCostCenter.Rows.Count - 1
            Dim dr As CompanyDTO.CompanyRow = dtCostCenter.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.COMP_NAME.ToString().Trim()
            oListItem.Value = dr.COMP_NO.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        If TagText = "" Then
            oDropList.SelectedValue = "CLHQ"
            For i = 0 To oDropList.Items.Count - 1
                If oDropList.Items(i).Value.Trim() = "CLHQ" Then
                    oDropList.Items(i).Selected = True
                End If
            Next
        End If


        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得RepairCenter資料
    ''' </summary>
    ''' <param name="RepairCenter">維修點</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText"></param>
    ''' <param name="isSelectALL">設定是否要有 "請選擇" 項目</param>
    ''' <remarks></remarks>
    Public Sub getRepairCenteryByDropDownList(ByVal RepairCenter As String, ByRef oDropList As DropDownList, ByVal TagText As String, ByVal isSelectALL As Boolean)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        dtCompany = oCompany.QueryAll()

        oDropList.Items.Clear()

        If isSelectALL = True Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If

        Dim RCenter As String() = RepairCenter.Split(",")

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

            For j = 0 To RCenter.Count - 1
                If RCenter(j).Trim() = dr.COMP_NO.Trim() Then
                    oListItem = New ListItem
                    oListItem.Text = dr.COMP_NAME.ToString().Trim()
                    oListItem.Value = dr.COMP_NO.ToString().Trim()
                    oDropList.Items.Add(oListItem)
                End If
            Next

        Next


        'For i = 0 To dtCompany.Rows.Count - 1
        '    Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

        '    If RepairCenter.Trim().IndexOf(dr.COMP_NO.Trim()) Then
        '        oListItem = New ListItem
        '        oListItem.Text = dr.COMP_NAME.ToString().Trim()
        '        oListItem.Value = dr.COMP_NO.ToString().Trim()
        '        oDropList.Items.Add(oListItem)
        '    End If
        'Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得RepairCenter資料
    ''' </summary>
    ''' <param name="oCheckBoxList">傳入CheckBoxList物件</param>
    ''' <remarks></remarks>
    Public Sub getRepairCenteryByCheckBoxList(ByRef oCheckBoxList As CheckBoxList)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable

        dtCompany = oCompany.QueryAll()
        oCheckBoxList.Items.Clear()

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.COMP_NAME.ToString().Trim()
            oListItem.Value = dr.COMP_NO.ToString().Trim()
            oCheckBoxList.Items.Add(oListItem)
        Next

        oCheckBoxList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得國家資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getCountryByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCountry As New ctlCountry
        Dim dtCountry As New CountryDTO.CountryDataTable

        dtCountry = oCountry.Query("1")
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtCountry.Rows.Count - 1
            Dim dr As CountryDTO.CountryRow = dtCountry.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.COUNTRY_NAME.ToString().Trim()
            oListItem.Value = dr.COUNTRY_ID.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得幣別資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getCurrencyByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oExchange As New ctlExchangeRate
        Dim dtExchange As New CurrencyDTO.CurrencyDataTable

        dtExchange = oExchange.Query("1")
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = "-Select-"
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtExchange.Rows.Count - 1
            Dim dr As CurrencyDTO.CurrencyRow = dtExchange.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.CURRENCY_CODE.ToString().Trim()
            oListItem.Value = dr.CURRENCY_CODE.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得不良原因類別資料
    ''' </summary>
    ''' <param name="DFLNO">傳入語系</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getFailureReasonsClassByDropDownList(ByVal DFLNO As String, ByVal oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0

        Dim oListItem As ListItem
        Dim oFailure As New ctlFailure.FailureReasonsClass
        Dim dtFailure As New DataTable

        dtFailure = oFailure.QueryByLanguage(DFLNO.ToString().Trim())
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        For i = 0 To dtFailure.Rows.Count - 1
            Dim dr As DataRow = dtFailure.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.Item(1).Trim()
            oListItem.Value = dr.Item(0).ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得不良原因資料
    ''' </summary>
    ''' <param name="DFLNO">傳入語系</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="FARC_NO">傳入不良原因類別代碼</param>
    ''' <param name="TagText"></param>
    ''' <remarks></remarks>
    Public Sub getFailureReasonsByDropDownList(ByVal DFLNO As String, ByVal FARC_NO As String, ByVal oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0

        Dim sValue As String = oDropList.SelectedValue
        Dim oListItem As ListItem
        Dim oFailure As New ctlFailure.FailureReasons
        Dim dtFailure As New FailureDTO.vwFailureReasonsDataTable

        dtFailure = oFailure.Query(DFLNO, FARC_NO)
        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = "-1"
        oDropList.Items.Add(oListItem)

        Dim bOK As Boolean = False

        For i = 0 To dtFailure.Rows.Count - 1
            Dim dr As FailureDTO.vwFailureReasonsRow = dtFailure.Rows(i)

            If dr.FAR_VISIBLE = 1 Then
                oListItem = New ListItem
                oListItem.Text = dr.FAR_REASON.ToString().Trim()
                oListItem.Value = dr.FAR_NO.ToString().Trim()
                If dr.FAR_NO.ToString().Trim() = sValue Then
                    bOK = True
                End If
                oDropList.Items.Add(oListItem)
            End If
        Next

        If bOK Then
            oDropList.SelectedValue = sValue
        End If

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 Defective 資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getDefectiveByDropDownList(ByVal DFLNO As String, ByRef oDropList As DropDownList)
        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oDuty As New ctlDefective
        Dim dtDuty As New DefectiveDTO.DefectiveDataTable

        dtDuty = oDuty.QueryByEffective(DFLNO)
        oDropList.Items.Clear()

        'oListItem = New ListItem
        'oListItem.Text = TagText
        'oListItem.Value = "-1"
        'oDropList.Items.Add(oListItem)

        For i = 0 To dtDuty.Rows.Count - 1
            Dim dr As DefectiveDTO.DefectiveRow = dtDuty.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.DEFECTIVE_NAME.ToString().Trim()
            oListItem.Value = dr.DEFECTIVE_NO.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 單頭 狀態下拉物件
    ''' </summary>
    ''' <param name="cboStatus"></param>
    ''' <remarks></remarks>
    Public Sub getStatus(ByVal cboStatus As DropDownList)
        Dim i As Integer = 0

        Dim oListItem As ListItem

        cboStatus.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oListItem.Value = "-1"
        cboStatus.Items.Add(oListItem)

        '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "026", ctlLanguage.eumType.Status)
        oListItem.Value = "10"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "034", ctlLanguage.eumType.Status)
        oListItem.Value = "20"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "032", ctlLanguage.eumType.Status)
        oListItem.Value = "90"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "033", ctlLanguage.eumType.Status)
        oListItem.Value = "91"
        cboStatus.Items.Add(oListItem)

        cboStatus.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 單身 狀態下拉物件
    ''' </summary>
    ''' <param name="cboStatus"></param>
    ''' <remarks></remarks>
    Public Sub getItemStatus(ByVal cboStatus As DropDownList)
        Dim i As Integer = 0

        Dim oListItem As ListItem

        cboStatus.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oListItem.Value = "-1"
        cboStatus.Items.Add(oListItem)

        '10.Requested, 20.Received, 30.Quoted, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 90.Closed, 91.Canceled
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "026", ctlLanguage.eumType.Status)
        oListItem.Value = "10"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "027", ctlLanguage.eumType.Status)
        oListItem.Value = "20"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "028", ctlLanguage.eumType.Status)
        oListItem.Value = "30"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "029", ctlLanguage.eumType.Status)
        oListItem.Value = "40"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "030", ctlLanguage.eumType.Status)
        oListItem.Value = "50"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "031", ctlLanguage.eumType.Status)
        oListItem.Value = "60"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "069", ctlLanguage.eumType.Status)
        oListItem.Value = "70"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "032", ctlLanguage.eumType.Status)
        oListItem.Value = "90"
        cboStatus.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "033", ctlLanguage.eumType.Status)
        oListItem.Value = "91"
        cboStatus.Items.Add(oListItem)

        cboStatus.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 單身 狀態下拉物件
    ''' </summary>
    ''' <param name="cboStatus"></param>
    ''' <param name="arrList">定義要取得哪些狀態的資料</param>
    ''' <remarks></remarks>
    Public Sub getItemStatus(ByVal cboStatus As DropDownList, ByVal arrList As Array)
        Dim i As Integer = 0
        Dim oListItem As ListItem

        cboStatus.Items.Clear()

        For i = 0 To arrList.Length - 1
            Dim sValue As String = arrList(i).ToString().Trim()
            oListItem = New ListItem

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
            '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            Select Case sValue
                Case "-1"
                    oListItem.Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
                    oListItem.Value = "-1"

                Case "10"   'Requested
                    oListItem.Text = _oLanguage.getText("Common", "026", ctlLanguage.eumType.Status)
                    oListItem.Value = "10"

                Case "20"   'Received
                    oListItem.Text = _oLanguage.getText("Common", "027", ctlLanguage.eumType.Status)
                    oListItem.Value = "20"

                Case "30"   'Repair Quoted
                    oListItem.Text = _oLanguage.getText("Common", "028", ctlLanguage.eumType.Status)
                    oListItem.Value = "30"

                Case "35"   'Sale Quoting
                    'oListItem.Text = _oLanguage.getText("Common", "029", ctlLanguage.eumType.Status)
                    'oListItem.Value = "35"

                Case "40"   'Sales Confirmed
                    oListItem.Text = _oLanguage.getText("Common", "029", ctlLanguage.eumType.Status)
                    oListItem.Value = "40"

                Case "50"   'Client Confirmed
                    oListItem.Text = _oLanguage.getText("Common", "030", ctlLanguage.eumType.Status)
                    oListItem.Value = "50"

                Case "60"   'Repaired
                    oListItem.Text = _oLanguage.getText("Common", "031", ctlLanguage.eumType.Status)
                    oListItem.Value = "60"

                Case "70"   'Repaired
                    oListItem.Text = _oLanguage.getText("Common", "069", ctlLanguage.eumType.Status)
                    oListItem.Value = "70"

                Case "90"   'Closed
                    oListItem.Text = _oLanguage.getText("Common", "032", ctlLanguage.eumType.Status)
                    oListItem.Value = "90"

                Case "91"   'Canceled
                    oListItem.Text = _oLanguage.getText("Common", "033", ctlLanguage.eumType.Status)
                    oListItem.Value = "91"
            End Select
            cboStatus.Items.Add(oListItem)
        Next

        cboStatus.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 Warranty 狀態下拉物件
    ''' </summary>
    ''' <param name="cboWarranty"></param>
    ''' <remarks></remarks>
    Public Sub getWarranty(ByVal cboWarranty As DropDownList)
        Dim i As Integer = 0

        Dim oListItem As ListItem

        cboWarranty.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oListItem.Value = "-1"
        cboWarranty.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Report", "050", ctlLanguage.eumType.Tag)
        oListItem.Value = "1"
        cboWarranty.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Report", "051", ctlLanguage.eumType.Tag)
        oListItem.Value = "0"
        cboWarranty.Items.Add(oListItem)

        cboWarranty.Dispose()
    End Sub

    Public Sub getQuery_ClientStatus(ByVal cboStatus As DropDownList)
        Dim i As Integer = 0

        Dim oListItem As ListItem

        cboStatus.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("RMA", "026", ctlLanguage.eumType.Tag)
        oListItem.Value = "-1"
        cboStatus.Items.Add(oListItem)

        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
        '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled

        'New Request
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "062", ctlLanguage.eumType.Status)
        oListItem.Value = "10"
        cboStatus.Items.Add(oListItem)

        'Processing
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "061", ctlLanguage.eumType.Status)
        oListItem.Value = "20"
        cboStatus.Items.Add(oListItem)

        'Closed
        oListItem = New ListItem
        oListItem.Text = _oLanguage.getText("Common", "060", ctlLanguage.eumType.Status)
        oListItem.Value = "90"
        cboStatus.Items.Add(oListItem)


        cboStatus.Dispose()
    End Sub

    ''' <summary>
    ''' 取得 原型機種 資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getQuery_PrimalModelNoByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String, ByVal USERID As String)
        Dim i As Integer = 0
        Dim oListItem As ListItem

        Dim oExport As New ctlRMA.Export_Primal
        Dim dtPrimal As New RmaDTO.EXPORT_PRIMALSNDataTable
        Dim denso() As String = {"CJ052", "CJ052D", "CJ052U", "DT439", "CJ052K", "CJ052S", "CJ052T", "CJ052I", "CJ052A"}

        If Array.IndexOf(denso, USERID) <> -1 Then
            oListItem = New ListItem
            oListItem.Text = "RS31"
            oListItem.Value = "RS31"
            oDropList.Items.Add(oListItem)
        Else
            dtPrimal = oExport.QueryAll()

            oDropList.Items.Clear()

            'oListItem = New ListItem
            'oListItem.Text = TagText
            'oListItem.Value = "-1"
            'oDropList.Items.Add(oListItem)

            For i = 0 To dtPrimal.Rows.Count - 1
                Dim dr As RmaDTO.EXPORT_PRIMALSNRow = dtPrimal.Rows(i)

                oListItem = New ListItem
                oListItem.Text = dr.EXPAR_PRIMALSN.ToString().Trim()
                oListItem.Value = dr.EXPAR_PRIMALSN.ToString().Trim()
                oDropList.Items.Add(oListItem)
            Next
        End If

        oDropList.Dispose()
    End Sub

#Region "取得日期下拉物件"
    Public Sub getYear_DropDownList(ByVal cboYear As DropDownList, Optional ByVal SelectedValue As String = "")
        Dim i As Integer
        Dim nYear As Integer = Date.Now.Year()

        Dim oListItem As ListItem
        cboYear.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = "-- Year --"
        oListItem.Value = "-1"
        cboYear.Items.Add(oListItem)

        nYear = nYear - 5
        For i = 0 To 9
            Dim nValue As Integer = 0
            nValue = nYear + i
            oListItem = New ListItem
            oListItem.Text = Convert.ToString(nValue).Trim()
            oListItem.Value = Convert.ToInt32(nValue)

            If SelectedValue.Trim() <> "" And nValue.ToString() = SelectedValue.Trim() Then
                oListItem.Selected = True
            End If

            cboYear.Items.Add(oListItem)
        Next

        cboYear.Dispose()
    End Sub

    Public Sub getMonth_DropDownList(ByVal cboMonth As DropDownList, Optional ByVal SelectedValue As String = "")
        Dim i As Integer
        Dim oListItem As ListItem
        cboMonth.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = "-- Month --"
        oListItem.Value = "-1"
        cboMonth.Items.Add(oListItem)

        For i = 1 To 12
            oListItem = New ListItem
            Dim sMonth As String = ""

            Select Case i
                Case 1
                    'sMonth = "January"
                    sMonth = _oLanguage.getText("Transfer", "016", ctlLanguage.eumType.Word)
                Case 2
                    'sMonth = "February"
                    sMonth = _oLanguage.getText("Transfer", "017", ctlLanguage.eumType.Word)
                Case 3
                    'sMonth = "March"
                    sMonth = _oLanguage.getText("Transfer", "018", ctlLanguage.eumType.Word)
                Case 4
                    'sMonth = "April"
                    sMonth = _oLanguage.getText("Transfer", "019", ctlLanguage.eumType.Word)
                Case 5
                    'sMonth = "May"
                    sMonth = _oLanguage.getText("Transfer", "020", ctlLanguage.eumType.Word)
                Case 6
                    'sMonth = "June"
                    sMonth = _oLanguage.getText("Transfer", "021", ctlLanguage.eumType.Word)
                Case 7
                    'sMonth = "July"
                    sMonth = _oLanguage.getText("Transfer", "022", ctlLanguage.eumType.Word)
                Case 8
                    'sMonth = "August"
                    sMonth = _oLanguage.getText("Transfer", "023", ctlLanguage.eumType.Word)
                Case 9
                    'sMonth = "September"
                    sMonth = _oLanguage.getText("Transfer", "024", ctlLanguage.eumType.Word)
                Case 10
                    'sMonth = "October"
                    sMonth = _oLanguage.getText("Transfer", "025", ctlLanguage.eumType.Word)
                Case 11
                    'sMonth = "November"
                    sMonth = _oLanguage.getText("Transfer", "026", ctlLanguage.eumType.Word)
                Case Else
                    'sMonth = "December"
                    sMonth = _oLanguage.getText("Transfer", "027", ctlLanguage.eumType.Word)
            End Select

            oListItem.Text = Convert.ToString(sMonth).Trim()
            oListItem.Value = Convert.ToInt32(i)

            If SelectedValue.Trim() <> "" And i.ToString() = SelectedValue.Trim() Then
                oListItem.Selected = True
            End If

            cboMonth.Items.Add(oListItem)
        Next

        cboMonth.Dispose()
    End Sub

    Public Sub getDay_DropDownList(ByVal cboDay As DropDownList, Optional ByVal SelectedValue As String = "")
        Dim i As Integer
        Dim oListItem As ListItem
        cboDay.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = "-- Day --"
        oListItem.Value = "-1"
        cboDay.Items.Add(oListItem)

        For i = 1 To 31
            oListItem = New ListItem
            oListItem.Text = Convert.ToString(i).Trim()
            oListItem.Value = Convert.ToInt32(i)

            If SelectedValue.Trim() <> "" And i.ToString() = SelectedValue.Trim() Then
                oListItem.Selected = True
            End If

            cboDay.Items.Add(oListItem)
        Next

        cboDay.Dispose()
    End Sub

#End Region

#Region "getMessage()"
    Public Function getMessage(ByVal enmMessage As enmMessage) As String
        Dim sMessage As String = ""

        Select Case enmMessage
            Case Common.enmMessage.AddOK
                sMessage = _oLanguage.getText("Processing", "007", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.EditOK
                sMessage = _oLanguage.getText("Processing", "008", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.DelOK
                sMessage = _oLanguage.getText("Processing", "009", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.MailOK
                sMessage = _oLanguage.getText("Processing", "010", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.ConfirmOK
                sMessage = _oLanguage.getText("Processing", "011", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.ChangePassWordOK
                sMessage = _oLanguage.getText("Processing", "012", ctlLanguage.eumType.Processing)

            Case Common.enmMessage.ProcessOK
                sMessage = _oLanguage.getText("Processing", "014", ctlLanguage.eumType.Processing)
        End Select

        Return sMessage
    End Function
#End Region

    ''' <summary>
    ''' 匯率轉換
    ''' </summary>
    ''' <param name="iSourceRate">傳入指派的維修中心 - 兌美金匯率</param>
    ''' <param name="iSourceQuote">傳入費用加總(報價)</param>
    ''' <param name="iTargetRate">傳入被指派的維修中心 - 兌美金匯率</param>
    ''' <returns>傳回指派的維修中心 --> 費用加總(報價)</returns>
    ''' <remarks></remarks>
    Public Function ConvertCurrency(ByVal iSourceRate As Double, ByVal iSourceQuote As Double, ByVal iTargetRate As Double) As Double
        Dim retvalAMT As Double

        retvalAMT = Math.Round((iSourceQuote / iSourceRate) * iTargetRate, 2)

        Return retvalAMT
    End Function

    ''' <summary>
    ''' 取得DefLaguage資料
    ''' </summary>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <remarks></remarks>
    Public Sub getDefLaguageByDropDownList(ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim i As Integer = 0

        Dim oListItem As ListItem
        Dim oLanguage As New ctlLanguage
        Dim dtDefLaguage As New LanguageDTO.DEFLANGUAGEDataTable

        dtDefLaguage = oLanguage.QueryByDefLanguage()
        oDropList.Items.Clear()

        'oListItem = New ListItem
        'oListItem.Text = TagText
        'oListItem.Value = "-1"
        'oDropList.Items.Add(oListItem)

        For i = 0 To dtDefLaguage.Rows.Count - 1
            Dim dr As LanguageDTO.DEFLANGUAGERow = dtDefLaguage.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr.DFL_NAME.ToString().Trim()
            oListItem.Value = dr.DFL_NO.ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()
    End Sub

    ''' <summary>
    ''' 取得各角色的 待處理工作連結的 頁面
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getWorkListPage() As String
        Dim i As Integer = 0
        Dim isFlowCase01 As Boolean = False 'JW/shaili
        Dim retval As String = ""

        '身分:1.客戶, 2.公司(維修中心)
        Select Case HttpContext.Current.Session("_Identity").ToString()
            Case "1"
                '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
                Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
                For i = 0 To arrRepairNo_flowCase01.Length - 1
                    If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                        retval = "Client_FlowCase01_Worklist.aspx"
                        Exit For
                    End If
                Next

                '用客戶申請時表單的維修中心判斷是否要執行 flow case 02
                Dim arrRepairNo_flowCase02() As String = _RepairNo_flowCase02.Trim().Split(",")
                For i = 0 To arrRepairNo_flowCase02.Length - 1
                    If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase02(i).ToString().Trim()) <> -1 Then
                        retval = "Client_FlowCase01_Worklist.aspx"
                        Exit For
                    End If
                Next

                If retval = "" Then
                    retval = "Client_Worklist.aspx"
                End If

            Case "2"
                '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
                If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("1") <> -1 Then
                    retval = "Receive_WorkList.aspx"
                    Exit Select
                End If

                If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("2") <> -1 Then
                    retval = "Repair_WorkList.aspx"
                    Exit Select
                End If

                If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("3") <> -1 Then
                    retval = "Sales_WorkList.aspx"
                    Exit Select
                End If

                If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("4") <> -1 Then
                    retval = "Shipping_List.aspx"
                    Exit Select
                End If

                If HttpContext.Current.Session("_OperationRole").ToString().IndexOf("9") <> -1 Then
                    retval = "MaintainAccount_Search.aspx"
                    Exit Select
                End If

            Case "3" 'JW/shaili/20150527
                '用客戶申請時表單的維修中心判斷是否要執行 flow case 01
                Dim arrRepairNo_flowCase01() As String = _RepairNo_flowCase01.Trim().Split(",")
                For i = 0 To arrRepairNo_flowCase01.Length - 1
                    If HttpContext.Current.Session("_RepairID").ToString.Trim().IndexOf(arrRepairNo_flowCase01(i).ToString().Trim()) <> -1 Then
                        isFlowCase01 = True
                        Exit For
                    End If
                Next
                'If isFlowCase01 = True Then
                retval = "Client_FlowCase01_Worklist.aspx"
                'Else
                'retval = "Client_Worklist.aspx"
                'End If
        End Select

        Return retval
    End Function

#Region "GetRandomizeNum:產生亂數檔名"
    '產生亂數檔名
    Public Function GetRandomizeNum() As String
        Dim strFileKey As String
        Dim i As Integer
        Randomize()
        strFileKey = Mid(CStr(Year(Now)), 3) & fnADD("0", Month(Now), 2) & fnADD("0", Day(Now), 2) & fnADD("0", Hour(Now), 2) & fnADD("0", Minute(Now), 2) & fnADD("0", Second(Now), 2)
        For i = 1 To 5
            strFileKey = strFileKey & Chr(Int(Rnd(1) * 26) + 65)
        Next
        Return strFileKey
    End Function

    Public Function fnADD(ByVal pITEM As String, ByVal pVALUE As String, ByVal pNUM As Integer) As String
        Dim i As Integer
        Dim pLong As Long
        pVALUE = CStr(pVALUE)
        pLong = Len(pVALUE) + 1
        For i = pLong To pNUM
            pVALUE = pITEM & pVALUE
        Next
        fnADD = pVALUE
    End Function
#End Region

    ''' <summary>
    ''' 取得Warranty Type資料
    ''' </summary>
    ''' <param name="isEffective">只取有效資料</param>
    ''' <param name="oDropList">傳入下拉式選項物件</param>
    ''' <param name="TagText">傳入要下拉的文字敘述</param>
    ''' <remarks></remarks>
    Public Sub getWarrantyTypeByDropDownList(ByVal isEffective As Boolean, ByRef oDropList As DropDownList, ByVal TagText As String)
        Dim oListItem As ListItem

        oDropList.Items.Clear()

        If TagText <> "" Then
            oListItem = New ListItem
            oListItem.Text = TagText
            oListItem.Value = "-1"
            oDropList.Items.Add(oListItem)
        End If


        oListItem = New ListItem
        oListItem.Text = "1.Component"
        oListItem.Value = 1
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "2.SW"
        oListItem.Value = 2
        oDropList.Items.Add(oListItem)

        oListItem = New ListItem
        oListItem.Text = "3.Additional Service"
        oListItem.Value = 3
        oDropList.Items.Add(oListItem)

        oDropList.Dispose()
    End Sub

    Public Sub getExtensionWarrantyTypeByDropDownList(ByVal isEffective As Boolean, ByRef oDropList As DropDownList, ByVal TagText As String)
        'Dim oListItem As ListItem

        'oDropList.Items.Clear()

        'If TagText <> "" Then
        '    oListItem = New ListItem
        '    oListItem.Text = TagText
        '    oListItem.Value = ""
        '    oDropList.Items.Add(oListItem)
        'End If


        'oListItem = New ListItem
        'oListItem.Text = "CW"
        'oListItem.Value = "CW"
        'oDropList.Items.Add(oListItem)

        'oListItem = New ListItem
        'oListItem.Text = "EW"
        'oListItem.Value = "EW"
        'oDropList.Items.Add(oListItem)

        'oDropList.Dispose()

        Dim i As Integer = 0
        Dim oListItem As ListItem
        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        dtCompany = oCompany.QueryWarrsetType(isEffective)

        oDropList.Items.Clear()

        oListItem = New ListItem
        oListItem.Text = TagText
        oListItem.Value = ""
        oDropList.Items.Add(oListItem)

        For i = 0 To dtCompany.Rows.Count - 1
            Dim dr As DataRow = dtCompany.Rows(i)

            oListItem = New ListItem
            oListItem.Text = dr("WARRSET_TYPE_NAME").ToString().Trim()
            oListItem.Value = dr("WARRSET_TYPE").ToString().Trim()
            oDropList.Items.Add(oListItem)
        Next

        oDropList.Dispose()

    End Sub

    ''' <summary>
    ''' 由 判斷是否為DBNull
    ''' </summary>
    ''' <param name="data">來源data</param>
    ''' <remarks></remarks>
    Public Function CheckDBNull(ByVal data As Object) As String
        If IsDBNull(data) Then
            Return ""
        Else
            Return data
        End If
    End Function

    Public Sub ExportSetup(reportDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument, ReportToPDF As String)
        If Not System.IO.Directory.Exists(ConfigurationSettings.AppSettings("Report_FilePath")) Then
            System.IO.Directory.CreateDirectory(ConfigurationSettings.AppSettings("Report_FilePath"))
        End If

        myDiskFileDestinationOptions = New DiskFileDestinationOptions()
        myExportOptions = reportDoc.ExportOptions
        reportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
        myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
        myDiskFileDestinationOptions.DiskFileName = ConfigurationManager.AppSettings("Report_FilePath") & ReportToPDF
        myExportOptions.DestinationOptions = myDiskFileDestinationOptions
        reportDoc.Export()
    End Sub

    ''' <summary>
    ''' 後端呼叫開啟 PDF，前端瀏覽器自動打開新視窗
    ''' PDF 可以位於後端專案的實體路徑
    ''' </summary>
    ''' <param name="page">當前 Page 實例</param>
    ''' <param name="pdfFileName">PDF 檔案名稱</param>
    Public Sub OpenPdf(page As Page, pdfFileName As String)
        Dim _WEBURL As String = ConfigurationManager.AppSettings("WEBURL")
        Dim filePath As String = ConfigurationManager.AppSettings("Report_FilePath")
        Dim fullFilePath As String = Path.Combine(filePath, pdfFileName)

        If page Is Nothing Then
            Throw New ArgumentNullException("page")
        End If

        If String.IsNullOrEmpty(fullFilePath) OrElse Not File.Exists(fullFilePath) Then
            Throw New FileNotFoundException("找不到 PDF 檔案", fullFilePath)
        End If

        If String.IsNullOrEmpty(pdfFileName) Then
            Throw New ArgumentNullException("pdfFileName")
        End If

        ' 從當前 Request 自動抓取後端 URL 與 port
        Dim request As HttpRequest = page.Request
        Dim scheme As String = request.Url.Scheme      ' http 或 https
        Dim host As String = request.Url.Host          ' 網域或 IP
        Dim port As Integer = request.Url.Port         ' port

        ' 生成 PDF URL，透過 Handler 提供 HTTP 訪問
        Dim pdfUrl As String = If(HttpContext.Current.Request.IsLocal, scheme & "://" & host & ":" & port, _WEBURL) & "/ashx/ExportPdfHandler.ashx?file=" & pdfFileName

        ' 註冊到 ScriptManager 或 ClientScript
        Dim sm As ScriptManager = ScriptManager.GetCurrent(page)
        If sm IsNot Nothing AndAlso sm.IsInAsyncPostBack Then
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openPdf", "window.open('" & pdfUrl & "');", True)
        Else
            page.ClientScript.RegisterStartupScript(page.GetType(), "openPdf", "window.open('" & pdfUrl & "');", True)
        End If
    End Sub


End Class

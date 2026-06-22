Imports System.Data
Imports System.IO
Imports DataService
Imports DefLanguage
'202307140929 exe TO excel
Imports NPOI.HSSF.UserModel

Partial Class Warranty_TypeSetting
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
    Dim _WEBURL As String = ConfigurationSettings.AppSettings("WEBURL")

    '20230714 09:27 產生excel
    Protected Sub Excel_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Excel_btn.Click

        '取保固單明細 20230714
        Dim oWarranty As New ctlWarranty
        Dim WARRANTYITEMDataTable_CELL As New DataTable
        WARRANTYITEMDataTable_CELL = oWarranty.QueryWARRSETALL()





        Dim MS_Date_yyyyMMddhhmmss As String = DateTime.Now.ToString("yyyyMMddhhmmss")
        Dim folderPath As String = Server.MapPath("~/FILE/Sample/")

        If Not WARRANTYITEMDataTable_CELL Is Nothing Then
            Dim dtTmp As DataTable = WARRANTYITEMDataTable_CELL


            Dim workbook As HSSFWorkbook = New HSSFWorkbook()
            Dim sheet1 As HSSFSheet = workbook.CreateSheet("sheet1")
            Dim row As HSSFRow
            Dim cell As HSSFCell

            For i = 0 To dtTmp.Rows.Count - 1

                row = sheet1.CreateRow(i)

                If i = 0 Then



                    Dim WAR_ID As String = "WAR_ID"
                    Dim WAR_ID_cell As HSSFCell = row.CreateCell(0)
                    WAR_ID_cell.SetCellValue(WAR_ID)

                    Dim WAR_NAME As String = "WAR_NAME"
                    Dim WAR_NAME_cell As HSSFCell = row.CreateCell(1)
                    WAR_NAME_cell.SetCellValue(WAR_NAME)

                    Dim WAR_COMPNO As String = "WAR_COMPNO"
                    Dim WAR_COMPNO_cell As HSSFCell = row.CreateCell(2)
                    WAR_COMPNO_cell.SetCellValue(WAR_COMPNO)

                    Dim WAR_GROUP As String = "WAR_GROUP"
                    Dim WAR_GROUP_cell As HSSFCell = row.CreateCell(3)
                    WAR_GROUP_cell.SetCellValue(WAR_GROUP)

                    Dim WAR_VERSION As String = "WAR_VERSION"
                    Dim WAR_VERSION_cell As HSSFCell = row.CreateCell(4)
                    WAR_VERSION_cell.SetCellValue(WAR_VERSION)

                    Dim WAR_TYPE As String = "WAR_TYPE"
                    Dim WAR_TYPE_cell As HSSFCell = row.CreateCell(5)
                    WAR_TYPE_cell.SetCellValue(WAR_TYPE)

                    Dim WAR_DISCOUNT As String = "WAR_DISCOUNT"
                    Dim WAR_DISCOUNT_cell As HSSFCell = row.CreateCell(6)
                    WAR_DISCOUNT_cell.SetCellValue(WAR_DISCOUNT)

                    Dim WAR_EXTMM As String = "WAR_EXTMM"
                    Dim WAR_EXTMM_cell As HSSFCell = row.CreateCell(7)
                    WAR_EXTMM_cell.SetCellValue(WAR_EXTMM)

                    Dim WAR_STDYY As String = "WAR_STDYY"
                    Dim WAR_STDYY_cell As HSSFCell = row.CreateCell(8)
                    WAR_STDYY_cell.SetCellValue(WAR_STDYY)

                    Dim WAR_LONGYY As String = "WAR_LONGYY"
                    Dim WAR_LONGYY_cell As HSSFCell = row.CreateCell(9)
                    WAR_LONGYY_cell.SetCellValue(WAR_LONGYY)

                    Dim WAR_DESC As String = "WAR_DESC"
                    Dim WAR_DESC_cell As HSSFCell = row.CreateCell(10)
                    WAR_DESC_cell.SetCellValue(WAR_DESC)

                    Dim WAR_ISALL As String = "WAR_ISALL"
                    Dim WAR_ISALL_cell As HSSFCell = row.CreateCell(11)
                    WAR_ISALL_cell.SetCellValue(WAR_ISALL)

                    Dim WAR_STATUS As String = "WAR_STATUS"
                    Dim WAR_STATUS_cell As HSSFCell = row.CreateCell(12)
                    WAR_STATUS_cell.SetCellValue(WAR_STATUS)

                    Dim WAR_AD As String = "WAR_AD"
                    Dim WAR_AD_cell As HSSFCell = row.CreateCell(13)
                    WAR_AD_cell.SetCellValue(WAR_AD)

                    Dim WAR_ADNAME As String = "WAR_ADNAME"
                    Dim WAR_ADNAME_cell As HSSFCell = row.CreateCell(14)
                    WAR_ADNAME_cell.SetCellValue(WAR_ADNAME)

                    Dim WAR_CSTMP As String = "WAR_CSTMP"
                    Dim WAR_CSTMP_cell As HSSFCell = row.CreateCell(15)
                    WAR_CSTMP_cell.SetCellValue(WAR_CSTMP)

                    Dim WAR_LUAD As String = "WAR_LUAD"
                    Dim WAR_LUAD_cell As HSSFCell = row.CreateCell(16)
                    WAR_LUAD_cell.SetCellValue(WAR_LUAD)

                    Dim WAR_LUADNAME As String = "WAR_LUADNAME"
                    Dim WAR_LUADNAME_cell As HSSFCell = row.CreateCell(17)
                    WAR_LUADNAME_cell.SetCellValue(WAR_LUADNAME)

                    Dim WAR_LUSTMP As String = "WAR_LUSTMP"
                    Dim WAR_LUSTMP_cell As HSSFCell = row.CreateCell(18)
                    WAR_LUSTMP_cell.SetCellValue(WAR_LUSTMP)

                    Dim WAR_MARK As String = "WAR_MARK"
                    Dim WAR_MARK_cell As HSSFCell = row.CreateCell(19)
                    WAR_MARK_cell.SetCellValue(WAR_MARK)

                    Dim WAR_EF_ID As String = "WAR_EF_ID"
                    Dim WAR_EF_ID_cell As HSSFCell = row.CreateCell(20)
                    WAR_EF_ID_cell.SetCellValue(WAR_EF_ID)

                    Dim WAR_COPYCN As String = "WAR_COPYCN"
                    Dim WAR_COPYCN_cell As HSSFCell = row.CreateCell(21)
                    WAR_COPYCN_cell.SetCellValue(WAR_COPYCN)




                Else



                    Dim WAR_ID As String = dtTmp.Rows(i - 1)("WAR_ID").ToString()
                    Dim WAR_ID_cell As HSSFCell = row.CreateCell(0)
                    WAR_ID_cell.SetCellValue(WAR_ID)

                    Dim WAR_NAME As String = dtTmp.Rows(i - 1)("WAR_NAME").ToString()
                    Dim WAR_NAME_cell As HSSFCell = row.CreateCell(1)
                    WAR_NAME_cell.SetCellValue(WAR_NAME)

                    Dim WAR_COMPNO As String = dtTmp.Rows(i - 1)("WAR_COMPNO").ToString()
                    Dim WAR_COMPNO_cell As HSSFCell = row.CreateCell(2)
                    WAR_COMPNO_cell.SetCellValue(WAR_COMPNO)

                    Dim WAR_GROUP As String = dtTmp.Rows(i - 1)("WAR_GROUP").ToString()
                    Dim WAR_GROUP_cell As HSSFCell = row.CreateCell(3)
                    WAR_GROUP_cell.SetCellValue(WAR_GROUP)

                    Dim WAR_VERSION As String = dtTmp.Rows(i - 1)("WAR_VERSION").ToString()
                    Dim WAR_VERSION_cell As HSSFCell = row.CreateCell(4)
                    WAR_VERSION_cell.SetCellValue(WAR_VERSION)

                    Dim WAR_TYPE As String = dtTmp.Rows(i - 1)("WAR_TYPE").ToString()
                    Dim WAR_TYPE_cell As HSSFCell = row.CreateCell(5)
                    WAR_TYPE_cell.SetCellValue(WAR_TYPE)

                    Dim WAR_DISCOUNT As String = dtTmp.Rows(i - 1)("WAR_DISCOUNT").ToString()
                    Dim WAR_DISCOUNT_cell As HSSFCell = row.CreateCell(6)
                    WAR_DISCOUNT_cell.SetCellValue(WAR_DISCOUNT)

                    Dim WAR_EXTMM As String = dtTmp.Rows(i - 1)("WAR_EXTMM").ToString()
                    Dim WAR_EXTMM_cell As HSSFCell = row.CreateCell(7)
                    WAR_EXTMM_cell.SetCellValue(WAR_EXTMM)

                    Dim WAR_STDYY As String = dtTmp.Rows(i - 1)("WAR_STDYY").ToString()
                    Dim WAR_STDYY_cell As HSSFCell = row.CreateCell(8)
                    WAR_STDYY_cell.SetCellValue(WAR_STDYY)

                    Dim WAR_LONGYY As String = dtTmp.Rows(i - 1)("WAR_LONGYY").ToString()
                    Dim WAR_LONGYY_cell As HSSFCell = row.CreateCell(9)
                    WAR_LONGYY_cell.SetCellValue(WAR_LONGYY)

                    Dim WAR_DESC As String = dtTmp.Rows(i - 1)("WAR_DESC").ToString()
                    Dim WAR_DESC_cell As HSSFCell = row.CreateCell(10)
                    WAR_DESC_cell.SetCellValue(WAR_DESC)

                    Dim WAR_ISALL As String = dtTmp.Rows(i - 1)("WAR_ISALL").ToString()
                    Dim WAR_ISALL_cell As HSSFCell = row.CreateCell(11)
                    WAR_ISALL_cell.SetCellValue(WAR_ISALL)

                    Dim WAR_STATUS As String = dtTmp.Rows(i - 1)("WAR_STATUS").ToString()
                    Dim WAR_STATUS_cell As HSSFCell = row.CreateCell(12)
                    WAR_STATUS_cell.SetCellValue(WAR_STATUS)

                    Dim WAR_AD As String = dtTmp.Rows(i - 1)("WAR_AD").ToString()
                    Dim WAR_AD_cell As HSSFCell = row.CreateCell(13)
                    WAR_AD_cell.SetCellValue(WAR_AD)

                    Dim WAR_ADNAME As String = dtTmp.Rows(i - 1)("WAR_ADNAME").ToString()
                    Dim WAR_ADNAME_cell As HSSFCell = row.CreateCell(14)
                    WAR_ADNAME_cell.SetCellValue(WAR_ADNAME)

                    Dim WAR_CSTMP As String = dtTmp.Rows(i - 1)("WAR_CSTMP").ToString()
                    Dim WAR_CSTMP_cell As HSSFCell = row.CreateCell(15)
                    WAR_CSTMP_cell.SetCellValue(WAR_CSTMP)

                    Dim WAR_LUAD As String = dtTmp.Rows(i - 1)("WAR_LUAD").ToString()
                    Dim WAR_LUAD_cell As HSSFCell = row.CreateCell(16)
                    WAR_LUAD_cell.SetCellValue(WAR_LUAD)

                    Dim WAR_LUADNAME As String = dtTmp.Rows(i - 1)("WAR_LUADNAME").ToString()
                    Dim WAR_LUADNAME_cell As HSSFCell = row.CreateCell(17)
                    WAR_LUADNAME_cell.SetCellValue(WAR_LUADNAME)

                    Dim WAR_LUSTMP As String = dtTmp.Rows(i - 1)("WAR_LUSTMP").ToString()
                    Dim WAR_LUSTMP_cell As HSSFCell = row.CreateCell(18)
                    WAR_LUSTMP_cell.SetCellValue(WAR_LUSTMP)

                    Dim WAR_MARK As String = dtTmp.Rows(i - 1)("WAR_MARK").ToString()
                    Dim WAR_MARK_cell As HSSFCell = row.CreateCell(19)
                    WAR_MARK_cell.SetCellValue(WAR_MARK)

                    Dim WAR_EF_ID As String = dtTmp.Rows(i - 1)("WAR_EF_ID").ToString()
                    Dim WAR_EF_ID_cell As HSSFCell = row.CreateCell(20)
                    WAR_EF_ID_cell.SetCellValue(WAR_EF_ID)

                    Dim WAR_COPYCN As String = dtTmp.Rows(i - 1)("WAR_COPYCN").ToString()
                    Dim WAR_COPYCN_cell As HSSFCell = row.CreateCell(21)
                    WAR_COPYCN_cell.SetCellValue(WAR_COPYCN)


                End If
            Next


            Dim MS As New FileStream(Path.Combine(folderPath, MS_Date_yyyyMMddhhmmss & ".xls"), FileMode.Create)
            workbook.Write(MS)
            MS.Close()
            Download_Lab.Text = "<a href ='" & "FILE/Sample/" & MS_Date_yyyyMMddhhmmss & ".xls" & "' download>下載" & MS_Date_yyyyMMddhhmmss & ".xls" & "</a>"

        Else
            Download_Lab.Text = "請再次點擊Searh"
        End If

    End Sub

    Public Sub ConfigureExportToPdf(ByVal href As String)
        '===================================================================================================================
        '傳PDF檔
        '===================================================================================================================

        Dim sScript As String = ""
        sScript = sScript & "<script language=""javascript"">"
        sScript = sScript & "window.open('" & href & "','','');"
        'sScript = sScript & "window.location.href='" & Me.UI_RedirectURL.Text & "';" & vbCrLf
        sScript = sScript & "</script>"
        Response.Write(sScript)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Me.ViewState("_SortExpression") = "Version"
            Me.ViewState("_SortDirection") = "desc"

            Me.ViewState("_OperationCenter") = ""
            Me.ViewState("_ProductGroup") = ""
            Me.ViewState("_WarrantyType") = ""
            Session("_dtTmp") = Nothing

            Dim oWarranty As New ctlWarranty
            Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
            dtProductGroup = oWarranty.QueryPrdGroup("", "")
            ViewState("_dtProductGroup") = dtProductGroup

            Call setControls()
            Call QueryData(0)
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setControls()

        Call oCommon.getCostCenterByDropDownList(False, Me.UI_CboOperationCenter, "All")
        Call oCommon.getExtensionWarrantyTypeByDropDownList(False, Me.UI_CboWarrantyType, "All")

        '取得Tag Text
        'Me.UI_lblTittle.Text = _oLanguage.getText("Warranty", "021", ctlLanguage.eumType.Tag)
        Me.UI_lblOperationCenter.Text = _oLanguage.getText("Warranty", "022", ctlLanguage.eumType.Tag)
        Me.UI_lblProductGroup.Text = _oLanguage.getText("Warranty", "023", ctlLanguage.eumType.Tag)
        Me.UI_lblWarrantyType.Text = _oLanguage.getText("Warranty", "024", ctlLanguage.eumType.Tag)
        Me.UI_lblPriceList.Text = _oLanguage.getText("Warranty", "061", ctlLanguage.eumType.Tag)


        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getPriceVerByDropDownList(Me.UI_cboPriceVer, "", sRepairText)

        Dim sProjectTypeText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)
        oCommon.getProgramTypeByDropDownList(Me.UI_cboProgramType, "", sRepairText)
        'setBattaryService(sRepairText)

        'oCommon.getWarrsetTypeByDropDownList(Me.UI_CboWarrantyType, sRepairText)

        oCommon.getItemTypeByDropDownList(Me.UI_cboItemType, "", sRepairText)

        oCommon.getWarrantyStatusByDropDownList(Me.UI_cboStatus)

        UI_lblItemtype.Text = _oLanguage.getText("Warranty", "082", ctlLanguage.eumType.Tag)
        UI_lblPriceVer.Text = _oLanguage.getText("Warranty", "083", ctlLanguage.eumType.Tag)
        UI_lblProgramType.Text = _oLanguage.getText("Warranty", "084", ctlLanguage.eumType.Tag)

        UI_lblCust.Text = _oLanguage.getText("Warranty", "076", ctlLanguage.eumType.Tag)
        UI_lblWarrantyName.Text = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        UI_lblStatus.Text = _oLanguage.getText("Warranty", "007", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "003", ctlLanguage.eumType.Command)
        Me.UI_cmdProductGroupPick.Text = _oLanguage.getText("Warranty", "040", ctlLanguage.eumType.Tag)


        Me.UI_dvSales.Columns(1).HeaderText = _oLanguage.getText("Warranty", "026", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(2).HeaderText = _oLanguage.getText("Warranty", "027", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(3).HeaderText = _oLanguage.getText("Warranty", "030", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(4).HeaderText = _oLanguage.getText("Warranty", "084", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(5).HeaderText = _oLanguage.getText("Warranty", "082", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(6).HeaderText = _oLanguage.getText("Warranty", "083", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(7).HeaderText = _oLanguage.getText("Customer", "003", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(8).HeaderText = _oLanguage.getText("Customer", "004", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(9).HeaderText = _oLanguage.getText("Warranty", "028", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(10).HeaderText = _oLanguage.getText("Warranty", "048", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(11).HeaderText = _oLanguage.getText("Warranty", "050", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(12).HeaderText = _oLanguage.getText("Warranty", "051", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(13).HeaderText = _oLanguage.getText("Warranty", "049", ctlLanguage.eumType.Tag)
        Me.UI_dvSales.Columns(14).HeaderText = _oLanguage.getText("Warranty", "034", ctlLanguage.eumType.Tag)

        Dim dtTmp As New DataTable
        dtTmp.Columns.Add("SEQID")
        dtTmp.Columns.Add("war_id")
        dtTmp.Columns.Add("OperationCenter")
        dtTmp.Columns.Add("ProductGroup")
        dtTmp.Columns.Add("WarrantyType")
        dtTmp.Columns.Add("Program_Type")
        dtTmp.Columns.Add("Item_Type")
        dtTmp.Columns.Add("Price_Ver")
        dtTmp.Columns.Add("cu_no")
        dtTmp.Columns.Add("cu_name")
        dtTmp.Columns.Add("Version")
        dtTmp.Columns.Add("WarrantyName")
        dtTmp.Columns.Add("ExtraMonths")
        dtTmp.Columns.Add("StdYears")
        dtTmp.Columns.Add("LongestYears")
        dtTmp.Columns.Add("Discount")
        dtTmp.Columns.Add("Status")
        dtTmp.Columns.Add("COMP_NAME")

        Session("_dtTmp") = dtTmp
    End Sub

    '維修中心查詢
    Private Sub QueryData_for(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.WARRSETDataTable

        Dim sOperationCenter As String = Me.ViewState("_OperationCenter").ToString().Trim()
        Dim sProductGroup As String = Me.ViewState("_ProductGroup").ToString().Trim()
        Dim sWarrantyType As String = Me.ViewState("_WarrantyType").ToString().Trim()
        Dim sPriceVer As String = IIf(Me.UI_cboPriceVer.SelectedValue = "-1", "", Me.UI_cboPriceVer.SelectedValue)
        Dim sProgramType As String = IIf(Me.UI_cboProgramType.SelectedValue = "-1", "", Me.UI_cboProgramType.SelectedValue)
        Dim sItemType As String = IIf(Me.UI_cboItemType.SelectedValue = "-1", "", Me.UI_cboItemType.SelectedValue)
        Dim sWarrantyName As String = UI_txtWarrantyName.Text
        Dim sCust As String = UI_txtCust.Text
        Dim sStatus As String = IIf(Me.UI_cboStatus.SelectedValue = "-1", "", Me.UI_cboStatus.SelectedValue)

        dtData = oWarranty.QueryWarrSetNew(sOperationCenter, sProductGroup, sWarrantyType, sItemType, sPriceVer, sProgramType, sWarrantyName, sCust, sStatus, "")

        Call ArrangementData(dtData)

        Dim dtTmp As DataTable = Session("_dtTmp")

        '創造 DataTable
        Dim vrTmp As New DataTable
        vrTmp.Columns.Add("war_id", GetType(String))
        vrTmp.Columns.Add("Status", GetType(String))
        vrTmp.Columns.Add("COMP_NAME", GetType(String))
        vrTmp.Columns.Add("ProductGroup", GetType(String))
        vrTmp.Columns.Add("WarrantyType", GetType(String))
        vrTmp.Columns.Add("Program_Type", GetType(String))
        vrTmp.Columns.Add("Item_Type", GetType(String))
        vrTmp.Columns.Add("Price_Ver", GetType(String))
        vrTmp.Columns.Add("cu_no", GetType(String))
        vrTmp.Columns.Add("cu_name", GetType(String))
        vrTmp.Columns.Add("Version", GetType(String))
        vrTmp.Columns.Add("WarrantyName", GetType(String))
        vrTmp.Columns.Add("StdYears", GetType(String))
        vrTmp.Columns.Add("LongestYears", GetType(String))
        vrTmp.Columns.Add("Discount", GetType(String))
        vrTmp.Columns.Add("SEQID", GetType(String))


        '資料二次過濾
        Dim list As New ArrayList

        Dim oCompany As New ctlCompany
        Dim dtCompany As New DataTable

        Dim sCustNO As String = Me.UI_txtCust.Text
        dtCompany = oCompany.QueryWarrsetType(False, sCustNO)

        '主資料表
        For a = 0 To dtTmp.Rows.Count - 1
            '比較
            Dim check_Warranty_Type As String = "N"

            Dim dr As DataRow = dtTmp.Rows(a)
            Dim WarrantyType As String = dr("WarrantyType").ToString().Trim()

            '先轉代碼

            '維修中心資料表
            For i = 0 To dtCompany.Rows.Count - 1
                Dim dr_ As DataRow = dtCompany.Rows(i)

                '代碼比對維修中心 是否需要顯示
                Dim WARRSET_TYPE As String = dr_("WARRSET_TYPE").ToString().Trim()

                '是否一樣
                If WarrantyType = WARRSET_TYPE Then
                    check_Warranty_Type = "Y"
                End If
                '是否一樣

            Next

            '比較 結果
            If check_Warranty_Type = "Y" Then
                Dim drTmp As DataRow = vrTmp.NewRow

                drTmp("SEQID") = dr("SEQID").ToString().Trim()
                drTmp("war_id") = dr("war_id").ToString().Trim()
                drTmp("Status") = dr("Status").ToString().Trim()
                drTmp("COMP_NAME") = dr("COMP_NAME").ToString().Trim()
                drTmp("ProductGroup") = dr("ProductGroup").ToString().Trim()
                drTmp("WarrantyType") = dr("WarrantyType").ToString().Trim()
                drTmp("Program_Type") = dr("Program_Type").ToString().Trim()
                drTmp("Item_Type") = dr("Item_Type").ToString().Trim()
                drTmp("Price_Ver") = dr("Price_Ver").ToString().Trim()
                drTmp("cu_no") = dr("cu_no").ToString().Trim()
                drTmp("cu_name") = dr("cu_name").ToString().Trim()
                drTmp("Version") = dr("Version").ToString().Trim()
                drTmp("WarrantyName") = dr("WarrantyName").ToString().Trim()
                drTmp("StdYears") = dr("StdYears").ToString().Trim()
                drTmp("LongestYears") = dr("LongestYears").ToString().Trim()
                drTmp("Discount") = dr("Discount").ToString().Trim()
                drTmp("Status") = dr("Status").ToString().Trim()

                vrTmp.Rows.Add(drTmp)

            End If
            '比較 結果


        Next



        Dim dvTmp As DataView = vrTmp.DefaultView

        Call RMA_DataBind(dvTmp, iPageIndex)
    End Sub
    '維修中心查詢

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim i As Integer = 0
        Dim oWarranty As New ctlWarranty
        Dim dtData As New WarrantyDTO.WARRSETDataTable

        Dim sOperationCenter As String = Me.ViewState("_OperationCenter").ToString().Trim()
        Dim sProductGroup As String = Me.ViewState("_ProductGroup").ToString().Trim()
        Dim sWarrantyType As String = Me.ViewState("_WarrantyType").ToString().Trim()
        Dim sPriceVer As String = IIf(Me.UI_cboPriceVer.SelectedValue = "-1", "", Me.UI_cboPriceVer.SelectedValue)
        Dim sProgramType As String = IIf(Me.UI_cboProgramType.SelectedValue = "-1", "", Me.UI_cboProgramType.SelectedValue)
        Dim sItemType As String = IIf(Me.UI_cboItemType.SelectedValue = "-1", "", Me.UI_cboItemType.SelectedValue)
        Dim sWarrantyName As String = UI_txtWarrantyName.Text
        Dim sCust As String = UI_txtCust.Text
        Dim sStatus As String = IIf(Me.UI_cboStatus.SelectedValue = "-1", "", Me.UI_cboStatus.SelectedValue)

        dtData = oWarranty.QueryWarrSetNew(sOperationCenter, sProductGroup, sWarrantyType, sItemType, sPriceVer, sProgramType, sWarrantyName, sCust, sStatus, "")

        Call ArrangementData(dtData)

        Dim dtTmp As DataTable = Session("_dtTmp")
        Dim dvTmp As DataView = dtTmp.DefaultView

        Call RMA_DataBind(dvTmp, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dtData As WarrantyDTO.WARRSETDataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim iCount As Integer = 0
        Dim sRMANo As String = ""

        Dim dtTmp As DataTable = Session("_dtTmp")
        dtTmp.Rows.Clear()
        Dim dvTmp As DataView = dtTmp.DefaultView
        For i = 0 To dtData.Rows.Count - 1
            Dim dr As WarrantyDTO.WARRSETRow = dtData.Rows(i)
            Dim war_id As String = dr.WAR_ID.ToString().Trim()

            iCount = iCount + 1
            Dim drTmp As DataRow = dtTmp.NewRow
            drTmp("SEQID") = iCount
            drTmp("war_id") = dr.WAR_ID
            drTmp("OperationCenter") = dr.WAR_COMPNO
            drTmp("ProductGroup") = dr.WAR_GROUP
            drTmp("Version") = dr.WAR_VERSION.ToString("000")
            drTmp("WarrantyName") = dr.WAR_NAME
            'If dr.WAR_TYPE = "CW" Then
            '    drTmp("WarrantyType") = "CW"
            'Else
            '    drTmp("WarrantyType") = "EW"
            'End If
            drTmp("WarrantyType") = dr.WAR_TYPE

            If Not dr.IsWAR_PROGRAM_TYPENull Then
                drTmp("Program_Type") = dr.WAR_PROGRAM_TYPE
            End If

            If Not dr.IsWAR_ITEM_TYPENull Then
                drTmp("Item_Type") = dr.WAR_ITEM_TYPE
            End If

            If Not dr.IsWAR_PRICE_VERNull Then
                drTmp("Price_Ver") = dr.WAR_PRICE_VER
            End If

            If Not dr.Iscu_noNull Then
                drTmp("cu_no") = dr.cu_no
            End If

            If Not dr.Iscu_nameNull Then
                drTmp("cu_name") = dr.cu_name
            End If

            drTmp("ExtraMonths") = dr.WAR_EXTMM
            drTmp("StdYears") = dr.WAR_STDYY
            drTmp("LongestYears") = dr.WAR_LONGYY
            drTmp("Discount") = dr.WAR_DISCOUNT
            drTmp("COMP_NAME") = dr.COMP_NAME
            If dr.WAR_STATUS = 1 Then
                drTmp("Status") = "Flow"
            ElseIf dr.WAR_STATUS = 2 Then
                drTmp("Status") = "Confirmed"
            ElseIf dr.WAR_STATUS = 3 Then
                drTmp("Status") = "Invaild"
            Else
                drTmp("Status") = "Open"
            End If
            dtTmp.Rows.Add(drTmp)
        Next

        Session("_dtTmp") = dtTmp
    End Sub

    Private Sub RMA_DataBind(ByVal dvTmp As DataView, ByVal iPageIndex As Integer)
        dvTmp.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_dvSales.PageSize = _PageSize
        Me.UI_dvSales.PageIndex = iPageIndex
        Me.UI_dvSales.DataSource = dvTmp
        Me.UI_dvSales.DataBind()
    End Sub

    Protected Sub UI_dvSales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvSales.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            UI_SeqID.Text = (Me.UI_dvSales.PageIndex * Me.UI_dvSales.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)

            If lblStatus.Text.Trim() <> "Open" Then
                UI_cmdEdit.Text = "Detail"
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
    Protected Sub UI_dvSales_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvSales.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtTmp") Is Nothing Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, iPageIndex)

        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_dvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_dvSales.RowCommand

        If e.CommandName = "cmdDetail" Or e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim lblSwID As Label = row.FindControl("lblSwID")

            Me.lblPreviousPage_SwID.Text = lblSwID.Text.Trim
        End If

        If e.CommandName = "cmdCopy" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)

            Dim row As GridViewRow = Me.UI_dvSales.Rows(iIndex)
            Dim lblSwID As Label = row.FindControl("lblSwID")

            Dim oWarranty As New ctlWarranty
            Dim sWar_id As String = oWarranty.SaveCopyWarrSet(lblSwID.Text, Session("_UserID"), Session("_UserName"))

            Me.lblPreviousPage_SwID.Text = sWar_id
            Me.lblPreviousPage_Type.Text = "C"
        End If

    End Sub

    Protected Sub UI_dvSales_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_dvSales.Sorting

        If Me.ViewState("_SortExpression").ToString().ToLower() <> e.SortExpression.ToString().ToLower() Then
            Me.ViewState("_SortDirection") = "asc"
        Else
            If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                Me.ViewState("_SortDirection") = "desc"
            Else
                Me.ViewState("_SortDirection") = "asc"
            End If
        End If
        Me.ViewState("_SortExpression") = e.SortExpression

        If IsNothing(Session("_dtTmp")) = False Then
            Dim dtTmp As DataTable = Session("_dtTmp")
            Dim dvTmp As DataView = dtTmp.DefaultView
            Call RMA_DataBind(dvTmp, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_dvSales.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_dvSales.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_dvSales.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_dvSales.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_dvSales.Columns(i).HeaderText = sHeaderText
            End If
        Next
    End Sub

    ''' <summary>
    ''' 按下查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        If UI_CboOperationCenter.SelectedValue.Trim() <> "-1" Then
            Me.ViewState("_OperationCenter") = Me.UI_CboOperationCenter.SelectedValue.Trim()
        Else
            Me.ViewState("_OperationCenter") = ""
        End If
        Me.ViewState("_WarrantyType") = Me.UI_CboWarrantyType.SelectedValue.Trim()
        Me.ViewState("_ProductGroup") = Me.UI_txtProductGroup.Text.Trim()

        '202307200 維修中心過濾
        If Me.UI_CheckBox_Cust.Checked = False Then
            Call QueryData(0)
        Else

            If Me.UI_txtCust.Text <> "" Then
                QueryData_for(0)
            Else
                Me.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('勾選 Show Warranty Customer Can Purchase 必填入 Customer 客戶編號')", True)
            End If
        End If
        '202307200 維修中心過濾

    End Sub
    Protected Sub UI_cmdProductGroupPick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdProductGroupPick.Click
        Dim dtProductGroup As DataTable = Me.ViewState("_dtProductGroup")
        Me.ucProductGroup.show(dtProductGroup, True)
    End Sub

    Protected Sub UI_cboWarrantyType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UI_CboWarrantyType.SelectedIndexChanged
        GetWarrsetData(UI_CboWarrantyType.SelectedValue)
    End Sub

    Private Sub GetWarrsetData(WarrsetType As String)
        Dim sRepairText As String = _oLanguage.getText("Customer", "006", ctlLanguage.eumType.Tag)

        oCommon.getPriceVerByDropDownList(Me.UI_cboPriceVer, WarrsetType, sRepairText)
        oCommon.getProgramTypeByDropDownList(Me.UI_cboProgramType, WarrsetType, sRepairText)
        oCommon.getItemTypeByDropDownList(Me.UI_cboItemType, WarrsetType, sRepairText)

    End Sub

End Class


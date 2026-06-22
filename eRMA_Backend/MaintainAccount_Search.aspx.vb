Imports System.Data
Imports CrystalDecisions.Shared
Imports DataService
Imports DefLanguage


Partial Class MaintainAccount_Search
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "3"
    Private ReportDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private myDiskFileDestinationOptions As DiskFileDestinationOptions
    Private myExportOptions As ExportOptions

    '所有產出報表的位置
    Dim _Reoprt_FilePath As String = ConfigurationSettings.AppSettings("Report_FilePath")
    Dim _Report_VisualPath As String = ConfigurationSettings.AppSettings("Report_VisualPath")

    Shared conNumberWordLess20 As String() = {"", "ONE", "TWO", "THREE", "FOUR", "FIVE",
     "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
     "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
     "EIGHTEEN", "NINETEEN"}

    Shared conNumberWordTen As String() = {"", "", "TWENTY", "THIRTY", "FORTY", "FIFTY",
     "SIXTY", "SEVENTY", "EIGHTY", "NINETY"}

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Session("_dtAdmin") = Nothing
            Call setControls()
            Call QueryData(0)
        End If
        'Call Print_Inovice("SHP-2015120045", "CLHQ", "ARBA-151200020")   'fairy
    End Sub
#End Region

    Private Sub Print_Inovice(ByVal ShippingNo As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String)
        Dim i As Integer = 0
        Dim ctlReport As New ctlReport
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable

        Dim iRMARQ_QUOTE As Double = 0.0
        'Throw New Exception(sRepairCenter.Trim())    'fairy
        dtRptINVOICE = ctlReport.qryRPTInovice(ShippingNo.Trim(), sRepairCenter.Trim(), sRMA_ARNO)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("RMAD_SEQ") = (i + 1).ToString()
            iRMARQ_QUOTE = iRMARQ_QUOTE + Convert.ToDouble(dtRptINVOICE.Rows(i)("RMARQ_QUOTE"))
        Next

        Dim SayTotal As String = GetNumberWord(iRMARQ_QUOTE)
        For i = 0 To dtRptINVOICE.Rows.Count - 1
            dtRptINVOICE.Rows(i)("SayTotal") = SayTotal
        Next

        Dim oDsReport As New DataSet
        oDsReport.Tables.Add(dtRptINVOICE)

        Dim LanguageID As String = ""
        If dtRptINVOICE.Rows.Count > 0 Then
            Dim oLoginInfo As New ctlLoginInfo
            LanguageID = oLoginInfo.getLanguageIDRMANO("Customer", dtRptINVOICE.Rows(0)("rmad_rmano").ToString())
        End If


        Call Print_Inovice(oDsReport, LanguageID)
    End Sub

    Private Sub Print_Inovice(ByVal oDsReport As DataSet, ByVal sLanguageID As String)
        Dim sReportToPDF_Invoice As String = "Invoice_" & oCommon.GetRandomizeNum() & ".pdf"
        '===================================================================================================================
        '產生Report檔
        '===================================================================================================================

        ReportDoc = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If sLanguageID = "003" Then
            ReportDoc.Load(Server.MapPath("Report\rptInovice_jp.rpt"))
        Else
            ReportDoc.Load(Server.MapPath("Report\rptInovice.rpt"))
        End If
        ReportDoc.SetDataSource(oDsReport)

        CrystalReportViewer1.ReportSource = ReportDoc
        Me.CrystalReportViewer1.DataBind()

        '修改Export PDF共用函式 by buck modify 20250828 begin
        oCommon.ExportSetup(ReportDoc, sReportToPDF_Invoice)
        Me.ViewState("_AttachmentFile_01") = _Reoprt_FilePath & sReportToPDF_Invoice
        'ExportSetup()
        'Me.ViewState("_AttachmentFile_01") = ConfigureExportToPdf(sReportToPDF_Invoice)
        '修改Export PDF共用函式 by buck modify 20250828 end

        Me.CrystalReportViewer1.Visible = False
        ReportDoc.Close()
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

    '    ' '' ''Dim sScript As String = ""
    '    ' '' ''sScript = sScript & "<script language=""javascript"">" & vbCrLf
    '    ' '' ''sScript = sScript & "window.open('" & _WEBURL & _Report_VisualPath & _ReportToPDF & "','','');" & vbCrLf
    '    '' '' ''sScript = sScript & "window.location.href='Shipping_List.aspx';" & vbCrLf
    '    ' '' ''sScript = sScript & "</script>" & vbCrLf
    '    ' '' ''Response.Write(sScript)

    '    Return retval
    'End Function

    Private Sub setControls()
        Dim sRepairText As String = _oLanguage.getText("MaintainAccount", "004", ctlLanguage.eumType.Tag)
        oCommon.getRepairCenteryByDropDownList(False, Me.UI_cboRepairCenter, sRepairText)

        Me.UI_cboRepairCenter.Items.Remove(Me.UI_cboRepairCenter.Items.FindByValue("-1"))

        '登入者有開維修中心權限
        Dim oAdmin As New ctlAdmin
        Dim dtAdmin As New AccountDTO.ADMINDataTable
        Dim sAdmin As String = Context.Session("_UserID").ToString()
        dtAdmin = oAdmin.Query(sAdmin, "")
        Dim oExtend As New ctlExtend
        Dim _CompanyData As String = ""

        '舊 維修中心
        Dim dt As DataTable = New DataTable
        dt = oExtend.QryCompanyData()

        '新 維修中心
        Dim dt_ As DataTable = New DataTable
        dt_ = oExtend.QryCompanyData()

        If dtAdmin.Count > 0 Then


            Dim AD_REPAIRCENTER As String() = dtAdmin(0)("AD_REPAIRCENTER").ToString().Split(",")

            For i = 0 To dt.Rows.Count - 1

                Dim index As Integer = 0

                '第一層過濾
                For a = 0 To AD_REPAIRCENTER.Length - 1
                    If (dt.Rows(i)(0).ToString() = AD_REPAIRCENTER(a).ToString()) Then
                        index = index + 1
                    End If
                Next

                If index > 0 Then

                Else
                    Dim row As DataRow = dt_.AsEnumerable().SingleOrDefault(Function(r) r("COMP_NO") = dt.Rows(i)(0).ToString())
                    If Not row Is Nothing Then
                        dt_.Rows.Remove(row)
                    End If
                End If

            Next

            Me.UI_cboRepairCenter.Items.Clear()

            For i = 0 To dt_.Rows.Count - 1
                Dim myListItem As ListItem = New ListItem
                myListItem.Value = dt_(i)(0).ToString()
                myListItem.Text = dt_(i)(1).ToString()
                Me.UI_cboRepairCenter.Items.Add(myListItem)
            Next

        End If



        'Me.UI_cboRepairCenter.SelectedItem.Value = Session("_RepairName").ToString()

        '取得Tag Text
        Me.UI_lblTittle.Text = _oLanguage.getText("MaintainAccount", "001", ctlLanguage.eumType.Tag)
        Me.UI_lblRepairCenter.Text = _oLanguage.getText("MaintainAccount", "002", ctlLanguage.eumType.Tag)
        Me.UI_lblUserTittle.Text = _oLanguage.getText("MaintainAccount", "003", ctlLanguage.eumType.Tag)
        Me.UI_lblTittle0.Text = _oLanguage.getText("MaintainAccount", "011", ctlLanguage.eumType.Tag)
        Me.UI_lblTittle1.Text = _oLanguage.getText("MaintainAccount", "012", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdAdd.Text = _oLanguage.getText("Common", "012", ctlLanguage.eumType.Command)

        Me.UI_MaintainAccount.Columns(1).HeaderText = _oLanguage.getText("MaintainAccount", "005", ctlLanguage.eumType.Tag)
        Me.UI_MaintainAccount.Columns(2).HeaderText = _oLanguage.getText("MaintainAccount", "006", ctlLanguage.eumType.Tag)
        Me.UI_MaintainAccount.Columns(3).HeaderText = _oLanguage.getText("MaintainAccount", "007", ctlLanguage.eumType.Tag)
        Me.UI_MaintainAccount.Columns(4).HeaderText = _oLanguage.getText("MaintainAccount", "008", ctlLanguage.eumType.Tag)
        Me.UI_MaintainAccount.Columns(5).HeaderText = _oLanguage.getText("MaintainAccount", "009", ctlLanguage.eumType.Tag)
        Me.UI_MaintainAccount.Columns(6).HeaderText = _oLanguage.getText("MaintainAccount", "010", ctlLanguage.eumType.Tag)
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oAdmin As New ctlAdmin

        Dim oCompany As New ctlCompany

        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim sRepairCenter As String = Me.UI_cboRepairCenter.SelectedValue.ToString().Trim()

        dtCompany = oCompany.QueryAll()

        If sRepairCenter.Trim <> "-1" Then
            dtAdmin = oAdmin.QueryByRepairCenter(sRepairCenter)
        Else
            dtAdmin = oAdmin.QueryAll()
        End If

        Call ArrangementData(dtAdmin, dtCompany)

        Session("_dtAdmin") = dtAdmin

        Dim dvAdmin As DataView = dtAdmin.DefaultView
        Me.ViewState("_SortExpression") = "AD_ID"
        Me.ViewState("_SortDirection") = "asc"
        dvAdmin.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()

        Call MaintainAccount_DataBind(dvAdmin, iPageIndex)
    End Sub

    Private Sub ArrangementData(ByVal dtAdmin As AccountDTO.ADMINDataTable, ByVal dtCompany As CompanyDTO.CompanyDataTable)
        Dim dvCompany As DataView
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim sLevel As String = ""
        Dim arrRoles() As String
        Dim arrRepairCenter() As String
        Dim sStatus As String = ""

        dvCompany = dtCompany.DefaultView

        dtAdmin.Columns.Add("Level")
        dtAdmin.Columns.Add("Roles")
        dtAdmin.Columns.Add("RepairCenter")
        dtAdmin.Columns.Add("Status")
        For i = 0 To dtAdmin.Rows.Count - 1
            '權限範圍等級:0.By Center、1.All
            sLevel = dtAdmin.Rows(i).Item("AD_AUTHORITYLEVEL").ToString.Trim()
            If sLevel.ToString.Trim() = "1" Then
                dtAdmin.Rows(i).Item("Level") = "All"
            Else
                dtAdmin.Rows(i).Item("Level") = "By Center"
            End If

            '角色:1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
            arrRoles = dtAdmin.Rows(i).Item("AD_ROLE").ToString.Trim().Split(",")
            Dim strRole As String = ""
            Dim sRole As String = ""
            For j = 0 To arrRoles.Length - 1
                Select Case arrRoles(j).Trim()
                    Case "1"
                        strRole = "Receiver"
                    Case "2"
                        strRole = "Repair center"
                    Case "3"
                        strRole = "Sales"
                    Case "4"
                        strRole = "Shipping"
                    Case "9"
                        strRole = "Admin"
                    Case "7"
                        strRole = "Warranty Selling"
                    Case "6"
                        strRole = "Warranty Setting"
                End Select

                If sRole.Trim = "" Then
                    sRole = strRole
                Else
                    sRole = sRole & "," & strRole
                End If
            Next
            dtAdmin.Rows(i).Item("Roles") = sRole

            '帳號狀態:0.Close、1.Open
            sStatus = dtAdmin.Rows(i).Item("AD_VISIBLE").ToString.Trim()
            If sStatus.ToString.Trim() = "1" Then
                dtAdmin.Rows(i).Item("Status") = "Open"
            Else
                dtAdmin.Rows(i).Item("Status") = "Close"
            End If

            '維修中心代碼
            arrRepairCenter = dtAdmin.Rows(i).Item("AD_REPAIRCENTER").ToString.Trim().Split(",")
            Dim sCompNo As String = ""
            Dim sRepairCenter As String = ""
            For j = 0 To arrRepairCenter.Length - 1
                sCompNo = arrRepairCenter(j).Trim()
                dvCompany.RowFilter = "COMP_NO='" & sCompNo.ToString().Trim() & "'"

                If dvCompany.Count > 0 Then
                    If sRepairCenter.Trim = "" Then
                        sRepairCenter = dvCompany(0)("COMP_NAME").ToString().Trim()
                    Else
                        sRepairCenter = sRepairCenter & "," & dvCompany(0)("COMP_NAME").ToString().Trim()
                    End If
                End If

            Next
            dtAdmin.Rows(i).Item("RepairCenter") = sRepairCenter
        Next
    End Sub

    Private Sub MaintainAccount_DataBind(ByVal dvAdmin As DataView, ByVal iPageIndex As Integer)
        Call CreateKeyPoint(Me.ViewState("_SortExpression").ToString(), Me.ViewState("_SortDirection").ToString())

        Me.UI_MaintainAccount.PageSize = _PageSize
        Me.UI_MaintainAccount.PageIndex = iPageIndex
        Me.UI_MaintainAccount.DataSource = dvAdmin
        Me.UI_MaintainAccount.DataBind()
    End Sub

    Protected Sub UI_MaintainAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_MaintainAccount.RowDataBound
        Dim i As Integer = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UI_SeqID As Label = e.Row.FindControl("UI_SeqID")
            UI_SeqID.Text = (Me.UI_MaintainAccount.PageIndex * Me.UI_MaintainAccount.PageSize) + (e.Row.RowIndex + 1).ToString()

            Dim UI_cmdEdit As Button = e.Row.FindControl("UI_cmdEdit")
            UI_cmdEdit.Text = _oLanguage.getText("Common", "005", ctlLanguage.eumType.Command)
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

    Protected Sub UI_MaintainAccount_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles UI_MaintainAccount.RowCommand

        If e.CommandName = "cmdEdit" Then
            Dim iIndex As Integer = Convert.ToInt16(e.CommandArgument)
            Dim row As GridViewRow

            row = Me.UI_MaintainAccount.Rows(iIndex)

            Dim UI_adID As Label = row.FindControl("UI_adID")
            Me.UI_lblPreviousPage_adID.Text = UI_adID.Text.Trim
        End If

    End Sub

    Protected Sub UI_MaintainAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_MaintainAccount.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        If Not Session("_dtAdmin") Is Nothing Then
            Dim dtAdmin As AccountDTO.ADMINDataTable = Session("_dtAdmin")
            Dim dvAdmin As DataView = dtAdmin.DefaultView
            dvAdmin.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call MaintainAccount_DataBind(dvAdmin, iPageIndex)
        Else
            Call QueryData(iPageIndex)
        End If
    End Sub

    Protected Sub UI_MaintainAccount_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles UI_MaintainAccount.Sorting

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

        If IsNothing(Session("_dtAdmin")) = False Then
            Dim dtAdmin As DataTable = Session("_dtAdmin")
            Dim dvAdmin As DataView = dtAdmin.DefaultView
            dvAdmin.Sort = Me.ViewState("_SortExpression").ToString() & " " & Me.ViewState("_SortDirection").ToString()
            Call MaintainAccount_DataBind(dvAdmin, 0)
        End If
    End Sub

    Private Sub CreateKeyPoint(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim i As Integer = 0
        Dim sKeyPoint_ASC As String = "▲"    '遞增(小->大)
        Dim sKeyPoint_Desc As String = "▼"   '遞減(大->小)

        For i = 0 To Me.UI_MaintainAccount.Columns.Count - 1
            Dim sHeaderText As String = Me.UI_MaintainAccount.Columns(i).HeaderText.ToString().Trim()
            sHeaderText = sHeaderText.Replace(sKeyPoint_ASC, "").Replace(sKeyPoint_Desc, "")

            Dim dgSort As String = Me.UI_MaintainAccount.Columns(i).SortExpression.ToLower().Trim()
            If dgSort.ToLower() = SortExpression.ToLower() Then
                If Me.ViewState("_SortDirection").ToString().ToLower = "asc".ToLower() Then
                    Me.UI_MaintainAccount.Columns(i).HeaderText = sKeyPoint_ASC & sHeaderText
                Else
                    Me.UI_MaintainAccount.Columns(i).HeaderText = sKeyPoint_Desc & sHeaderText
                End If

            Else
                Me.UI_MaintainAccount.Columns(i).HeaderText = sHeaderText
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
        Call QueryData(0)
    End Sub

End Class

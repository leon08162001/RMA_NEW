<%@ WebHandler Language="VB" Class="GetRMADataHandler" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data
Imports System.IO

Public Class GetRMADataHandler : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState

    Public Class test
        Public Property a As String
        Public Property b As String
    End Class

    Public Class SnData
        Public Property sn As String
        Public Property CustomerID As String
    End Class


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "application/json"

        Dim octAddress As New ctAddress
        Dim oClient As New ctlRMA.Client
        Dim oDataTable As New DataTable

        Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable


        Dim Requested As New ctlRMA.Requested
        Dim RMADetailDataTable As New RmaDTO.RMADetailDataTable

        Dim oRMAStatus As New ctlRMA.RMAStatus
        Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
        Dim _QueryDate_Month As String = ConfigurationSettings.AppSettings("QueryDate_Month")
        Dim CustomerID As String = ""
        Dim sRMANo As String = ""
        Dim sModelNo As String = ""
        Dim sSerialNo As String = ""
        Dim Status As String = "-1"
        Dim fdate As String = Date.Now.AddMonths(-480).ToShortDateString()
        Dim edate As String = Date.Now.ToShortDateString()

        '接受前臺傳的num
        Dim Sn_no As String

        Using reader As StreamReader = New StreamReader(context.Request.InputStream)
            Sn_no = reader.ReadToEnd()
        End Using

        If (Sn_no = "") Then
            context.Response.Write("")
            Return
        End If

        Dim p As SnData = JsonConvert.DeserializeObject(Of SnData)(Sn_no)

        sRMANo = p.sn.Trim()
        CustomerID = p.CustomerID.Trim()

        dtRequest = oClient.Query_Not_Like(sRMANo, "", sModelNo, sSerialNo, "-1", Status, fdate, edate, "", "RMAD_RMANO desc")
        oDataTable = octAddress.select_RMADETAIL(sRMANo)

        If oDataTable.Rows.Count > 0 Then

            '先抓到RMA單目前最前面狀態(排序 數字最小的放第一筆取出) 然後抓時間表 把時間填上去
            Dim RMADetailDataTable_RMAD_ID As String = oDataTable.Rows(0)(0).ToString().Trim()
            dtStatusPoint = oRMAStatus.QueryPointByDetail(RMADetailDataTable_RMAD_ID)

            If dtStatusPoint.Rows.Count = 0 Then

                If dtRequest.Rows(0)("RMAD_STATUS") <> 0 Then

                    If dtRequest.Rows.Count > 0 Then
                        Dim dtStatusPointNew As DataRow = dtStatusPoint.NewRow
                        dtStatusPointNew("RMAD_ID") = Convert.ToDateTime(dtRequest.Rows(0)("RMAD_CSTMP")).ToShortDateString()
                        dtStatusPoint.Rows.Add(dtStatusPointNew)
                    End If

                End If

            Else
                dtStatusPoint.Rows(0)("RMAD_ID") = Convert.ToDateTime(dtRequest.Rows(0)("RMAD_CSTMP")).ToShortDateString()
            End If

            If dtStatusPoint.Rows.Count > 0 Then
                Dim _SnData As String = JsonConvert.SerializeObject(dtStatusPoint)
                context.Response.Write(_SnData)
            Else
                context.Response.Write("")

            End If

        Else
            context.Response.Write("")
        End If

    End Sub

    Private Sub QueryDataByStatusPoint(ByVal RMANo As String)
        Dim oRMAStatus As New ctlRMA.RMAStatus
        Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable


        Dim UI_lblReceivedUser_Text_String As String = ""
        Dim UI_lblReceivedDate_Text_String As String = ""
        Dim UI_lblRepairQuotedUser_Text_String As String = ""
        Dim UI_lblRepairQuotedDate_Text_String As String = ""
        Dim UI_lblSalesUser_Text_String As String = ""
        Dim UI_lblSalesDate_Text_String As String = ""
        Dim UI_lblClientUser_Text_String As String = ""
        Dim UI_lblClientDate_Text_String As String = ""
        Dim UI_lblRepairedUser_Text_String As String = ""
        Dim UI_lblRepairedDate_Text_String As String = ""
        Dim UI_lblCloseUser_Text_String As String = ""
        Dim UI_lblCloseDate_Text_String As String = ""
        Dim UI_lblCancelUser_Text_String As String = ""
        Dim UI_lblCancelDate_Text_String As String = ""



        dtStatusPoint = oRMAStatus.QueryPointByDetail(RMANo)
        If dtStatusPoint.Rows.Count > 0 Then
            Dim dr As RmaDTO.vwStatusPoint_DetailRow = dtStatusPoint.Rows(0)

            If dr.IsRECEIVED_ADNull = False Then UI_lblReceivedUser_Text_String = dr.RECEIVED_AD.Trim()
            If dr.IsRECEIVED_DATENull = False Then UI_lblReceivedDate_Text_String = dr.RECEIVED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsREPAIRQUOTED_ADNull = False Then UI_lblRepairQuotedUser_Text_String = dr.REPAIRQUOTED_AD.Trim()
            If dr.IsREPAIRQUOTED_DATENull = False Then UI_lblRepairQuotedDate_Text_String = dr.REPAIRQUOTED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsSALES_ADNull = False Then UI_lblSalesUser_Text_String = dr.SALES_AD.Trim()
            If dr.IsSALES_DATENull = False Then UI_lblSalesDate_Text_String = dr.SALES_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLIENT_CONFIRMNull = False Then
                '1.客戶自行確認, 2.業務帶客戶確認
                If dr.CLIENT_CONFIRM = 1 Then
                    If dr.IsCLIENT_ADNull = False Then UI_lblClientUser_Text_String = dr.CLIENT_AD.Trim()
                    If dr.IsCLIENT_DATENull = False Then UI_lblClientDate_Text_String = dr.CLIENT_DATE.ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    If dr.IsSALES_ADNull = False Then UI_lblClientUser_Text_String = dr.SALES_AD.Trim()
                    If dr.IsSALES_DATENull = False Then UI_lblClientDate_Text_String = dr.SALES_DATE.ToString("yyyy/MM/dd HH:mm:ss")
                End If
            End If

            If dr.IsREPAIRED_ADNull = False Then UI_lblRepairedUser_Text_String = dr.REPAIRED_AD.Trim()
            If dr.IsREPAIRED_DATENull = False Then UI_lblRepairedDate_Text_String = dr.REPAIRED_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCLOSE_ADNull = False Then UI_lblCloseUser_Text_String = dr.CLOSE_AD.Trim()
            If dr.IsCLOSE_DATENull = False Then UI_lblCloseDate_Text_String = dr.CLOSE_DATE.ToString("yyyy/MM/dd HH:mm:ss")

            If dr.IsCANCEL_ADNull = False Then UI_lblCancelUser_Text_String = dr.CANCEL_AD.Trim()
            If dr.IsCANCEL_DATENull = False Then UI_lblCancelDate_Text_String = dr.CANCEL_DATE.ToString("yyyy/MM/dd HH:mm:ss")

        End If

    End Sub


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
<%@ WebHandler Language="VB" Class="GetRMACostHandler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Newtonsoft.Json
Imports DataService
Imports System.Data

Public Class GetRMACostHandler : Implements IHttpHandler

    Partial Public Class RMA_Cost_Params
        Public Property sDate As String
        Public Property eDate As String
    End Class

    Partial Public Class RMA_Cost
        Public Property RMA_NO As String
        Public Property cust_no As String
        Public Property repair_no As String
        Public Property SALE_NO As String
        Public Property SALE_NM As String
        Public Property material_amt As Decimal
        Public Property laborcost_amt As Decimal
        Public Property repair_amt As Decimal
        Public Property receivable_amt As Decimal
        Public Property sale_quoted_amt As Decimal
        Public Property shipping_date As String
        Public Property year As String
        Public Property month As String
        Public Property day As String
    End Class



    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        Dim GetStr As String
        Dim sParams As RMA_Cost_Params

        Using reader As StreamReader = New StreamReader(context.Request.InputStream, Encoding.UTF8)
            'sParams = JsonConvert.DeserializeObject(Of MaintenanceStatement_Params)(reader.ReadToEnd())
            GetStr = reader.ReadToEnd()
            Dim decodeStr As String = HttpUtility.UrlDecode(GetStr)
            sParams = JsonConvert.DeserializeObject(Of RMA_Cost_Params)(GetStr)
        End Using

        Dim _RMA_CostData As String = JsonConvert.SerializeObject(GetMaintenanceStatementData(sParams.sDate, sParams.eDate))

        If _RMA_CostData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_RMA_CostData)
    End Sub

    Function GetMaintenanceStatementData(ByVal sDate As String, ByVal eDate As String) As Object
        Dim oExtend As New ctlExtend
        Dim dRMA_CostData As DataTable = oExtend.QryRMACostData(sDate, eDate)
        Dim lstOfString As List(Of Country) = New List(Of Country)

        lstOfString.Add(New Country("台灣", "TW"))
        lstOfString.Add(New Country("中國", "CN"))
        lstOfString.Add(New Country("香港", "HK"))
        lstOfString.Add(New Country("澳門", "MO"))
        lstOfString.Add(New Country("澳大利亞(澳洲)", "AU"))
        lstOfString.Add(New Country("巴林", "BH"))
        lstOfString.Add(New Country("不丹", "BT"))
        lstOfString.Add(New Country("汶萊", "BN"))
        lstOfString.Add(New Country("柬埔寨", "KH"))
        lstOfString.Add(New Country("斐濟", "FJ"))
        lstOfString.Add(New Country("關島", "GU"))
        lstOfString.Add(New Country("印度", "IN"))
        lstOfString.Add(New Country("印尼", "ID"))
        lstOfString.Add(New Country("伊朗", "IR"))
        lstOfString.Add(New Country("以色列", "IL"))
        lstOfString.Add(New Country("日本", "JP"))
        lstOfString.Add(New Country("約旦", "JO"))
        lstOfString.Add(New Country("科威特", "KW"))
        lstOfString.Add(New Country("韓國", "KR"))
        lstOfString.Add(New Country("寮國", "LA"))
        lstOfString.Add(New Country("馬來西亞", "MY"))
        lstOfString.Add(New Country("諾魯", "NR"))
        lstOfString.Add(New Country("紐西蘭", "NZ"))
        lstOfString.Add(New Country("阿曼", "OM"))
        lstOfString.Add(New Country("巴基斯坦", "PK"))
        lstOfString.Add(New Country("巴布亞紐幾內亞", "PG"))
        lstOfString.Add(New Country("菲律賓", "PH"))
        lstOfString.Add(New Country("卡達", "QA"))
        lstOfString.Add(New Country("新加坡", "SG"))
        lstOfString.Add(New Country("沙烏地阿拉伯", "SA"))
        lstOfString.Add(New Country("索羅門群島", "SB"))
        lstOfString.Add(New Country("斯里蘭卡", "LK"))
        lstOfString.Add(New Country("敘利亞", "SY"))
        lstOfString.Add(New Country("泰國", "TH"))
        lstOfString.Add(New Country("土耳其", "TR"))
        lstOfString.Add(New Country("阿拉伯聯合大公國", "AE"))
        lstOfString.Add(New Country("越南", "VN"))
        lstOfString.Add(New Country("葉門", "YE"))
        lstOfString.Add(New Country("亞美尼亞", "AM"))
        lstOfString.Add(New Country("奧地利", "AT"))
        lstOfString.Add(New Country("亞塞拜然", "AZ"))
        lstOfString.Add(New Country("白俄羅斯", "BY"))
        lstOfString.Add(New Country("比利時", "BE"))
        lstOfString.Add(New Country("保加利亞", "BG"))
        lstOfString.Add(New Country("塞普勒斯", "CY"))
        lstOfString.Add(New Country("捷克", "CZ"))
        lstOfString.Add(New Country("丹麥", "DK"))
        lstOfString.Add(New Country("愛沙尼亞", "EE"))
        lstOfString.Add(New Country("芬蘭", "FI"))
        lstOfString.Add(New Country("法國", "FR"))
        lstOfString.Add(New Country("喬治亞", "GE"))
        lstOfString.Add(New Country("德國", "DE"))
        lstOfString.Add(New Country("英國", "GB"))
        lstOfString.Add(New Country("希臘", "GR"))
        lstOfString.Add(New Country("匈牙利", "HU"))
        lstOfString.Add(New Country("愛爾蘭", "IE"))
        lstOfString.Add(New Country("義大利", "IT"))
        lstOfString.Add(New Country("哈薩克", "KZ"))
        lstOfString.Add(New Country("吉爾吉斯", "KG"))
        lstOfString.Add(New Country("拉脫維亞", "LV"))
        lstOfString.Add(New Country("立陶宛", "LT"))
        lstOfString.Add(New Country("盧森堡", "LU"))
        lstOfString.Add(New Country("馬耳他", "MT"))
        lstOfString.Add(New Country("摩爾多瓦", "MD"))
        lstOfString.Add(New Country("荷蘭", "NL"))
        lstOfString.Add(New Country("挪威", "NO"))
        lstOfString.Add(New Country("波蘭", "PL"))
        lstOfString.Add(New Country("葡萄牙", "PT"))
        lstOfString.Add(New Country("羅馬尼亞", "ES"))
        lstOfString.Add(New Country("俄羅斯", "RU"))
        lstOfString.Add(New Country("斯洛伐克", "SK"))
        lstOfString.Add(New Country("斯洛維尼亞", "SI"))
        lstOfString.Add(New Country("西班牙", "ES"))
        lstOfString.Add(New Country("瑞士", "CH"))
        lstOfString.Add(New Country("瑞典", "SE"))
        lstOfString.Add(New Country("塔吉克", "TJ"))
        lstOfString.Add(New Country("土庫曼", "TM"))
        lstOfString.Add(New Country("烏克蘭", "UA"))
        lstOfString.Add(New Country("烏茲別克", "UZ"))
        lstOfString.Add(New Country("加拿大", "CA"))
        lstOfString.Add(New Country("美國", "US"))
        lstOfString.Add(New Country("阿根廷", "AR"))
        lstOfString.Add(New Country("巴貝多", "BB"))
        lstOfString.Add(New Country("玻利維亞", "BO"))
        lstOfString.Add(New Country("巴西", "BR"))
        lstOfString.Add(New Country("智利", "CL"))
        lstOfString.Add(New Country("哥倫比亞", "CO"))
        lstOfString.Add(New Country("哥斯大黎加", "CR"))
        lstOfString.Add(New Country("厄瓜多", "EC"))
        lstOfString.Add(New Country("薩爾瓦多", "SV"))
        lstOfString.Add(New Country("瓜地馬拉", "GT"))
        lstOfString.Add(New Country("宏都拉斯", "HN"))
        lstOfString.Add(New Country("牙買加", "JM"))
        lstOfString.Add(New Country("墨西哥", "MX"))
        lstOfString.Add(New Country("巴拿馬", "PA"))
        lstOfString.Add(New Country("巴拉圭", "PY"))
        lstOfString.Add(New Country("秘魯", "PE"))
        lstOfString.Add(New Country("波多黎各", "PR"))
        lstOfString.Add(New Country("烏拉圭", "UY"))
        lstOfString.Add(New Country("委內瑞拉", "VE"))
        lstOfString.Add(New Country("貝南", "BJ"))
        lstOfString.Add(New Country("象牙海岸", "CI"))
        lstOfString.Add(New Country("吉布地", "DJ"))
        lstOfString.Add(New Country("埃及", "EG"))
        lstOfString.Add(New Country("衣索比亞", "ET"))
        lstOfString.Add(New Country("迦納", "GH"))
        lstOfString.Add(New Country("肯亞", "KE"))
        lstOfString.Add(New Country("賴索托", "LS"))
        lstOfString.Add(New Country("馬達加斯加", "MG"))
        lstOfString.Add(New Country("馬拉威", "MW"))
        lstOfString.Add(New Country("馬利", "ML"))
        lstOfString.Add(New Country("模里西斯", "MU"))
        lstOfString.Add(New Country("莫三比克", "MZ"))
        lstOfString.Add(New Country("尼日", "NE"))
        lstOfString.Add(New Country("奈及利亞", "NG"))
        lstOfString.Add(New Country("塞內加爾", "SN"))
        lstOfString.Add(New Country("獅子山", "SL"))
        lstOfString.Add(New Country("南非", "ZA"))
        lstOfString.Add(New Country("蘇丹", "SD"))
        lstOfString.Add(New Country("坦尚尼亞", "TZ"))
        lstOfString.Add(New Country("多哥", "TG"))
        lstOfString.Add(New Country("英國", "UK"))

        For i = 0 To dRMA_CostData.Rows.Count - 1
            dRMA_CostData(i)("CU_COUNTRYID") = dRMA_CostData(i)("CU_COUNTRYID")

            Dim result = From v In lstOfString Where v.country_Lastname = dRMA_CostData(i)("CU_COUNTRYID")

            For Each value As Country In result
                dRMA_CostData(i)("CU_COUNTRYID") = value.country_Firstname
            Next

        Next

        Return dRMA_CostData

    End Function


    Public Structure Country

        Public Property country_Firstname As String
        Public Property country_Lastname As String

        Sub New(firstname As String, lastname As String)
            Me.country_Firstname = firstname
            Me.country_Lastname = lastname
        End Sub

    End Structure

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
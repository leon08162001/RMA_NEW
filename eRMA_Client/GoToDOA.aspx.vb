Imports System.Data
Imports System.Data.OracleClient

Partial Class GoToDOA
    Inherits System.Web.UI.Page

    Dim oCommon As New Common
    'Dim _oLanguage As New ctlLanguage

    Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize")
    'Dim _PageSize As String = "5"

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '登入者ID
        Dim str_UserID As String = Session("_UserID").ToString().Trim()
        Dim str_loginId As String = QueryDoALoginId(str_UserID)
        '身分:1.客戶, 2.公司(維修中心)
        Dim str_Identity As String = Session("_Identity").ToString().Trim()


        If str_loginId.Equals("") Then
            '沒有帳號顯示查無帳號
            If str_Identity.Equals("1") Then
                Response.Write("<script language=javascript>alert('「" & str_UserID & "」- DOA system no such account ! Please contact your sales !')</script>")
            Else
                Response.Write("<script language=javascript>alert('「" & str_UserID & "」- DOA system no such account !')</script>")
            End If

            Response.Write("<script>function func(){window.close();}</script>")
        Else
            '如果有帳號轉到DOA 開單都往對外的網頁
            Dim str_act As String = "https://websvr2.cipherlab.com.tw/View/Account/Login.aspx?ID=" & str_UserID & "&AD=" & str_loginId
            '客戶的ID與AD相同
            If str_Identity.Equals("1") Then
                str_act = "https://websvr2.cipherlab.com.tw/View/Account/Login.aspx?ID=" & str_loginId & "&AD=" & str_loginId
            End If
            'Response.Write("<script language=javascript>alert('str_passAct=「" & str_passAct & "」!')</script>")
            Me.Form.Action = str_act
            Response.Write("<script language=javascript>function func(){document.form1.submit();}</script>")
        End If

    End Sub
#End Region

    Private Function QueryDoALoginId(ByVal strParam As String)

        Dim strReturn As String = ""

        '身分:1.客戶, 2.公司(維修中心)
        Dim str_Identity As String = Session("_Identity").ToString().Trim()
        'Response.Write("<script language=javascript>alert('1.客戶, 2.公司 ---str_Identity=「" & str_Identity & "」')</script>")
        'Response.Write("<script language=javascript>alert('str_UserID=「" & str_UserID & "」')</script>")


        Dim oracle_connect As New OracleConnection
        Dim oracle_command As New OracleCommand
        Dim dr As OracleDataReader

        '取得 Web.config 檔的資料連接設定
        'oracle_connect.ConnectionString =  ConfigurationManager.ConnectionStrings("ConnectionString1").ConnectionString
        oracle_connect.ConnectionString = "Data Source=192.168.7.20:1521/topprod;Persist Security Info=True;User ID=rma;Password=4321rma;Unicode=True"
        oracle_command.Connection = oracle_connect
        oracle_command.CommandType = CommandType.StoredProcedure
        oracle_command.CommandText = "SP_GetDOALoginId"

        oracle_command.Parameters.Clear()

        '設定參數為 OracleType.VarChar資料型別(對應 Oracle資料庫的 varchar2)，SIZE是1
        oracle_command.Parameters.Add("vType", OracleType.VarChar, 1)
        oracle_command.Parameters("vType").Value = str_Identity

        oracle_command.Parameters.Add("vData1", OracleType.VarChar, 50)
        oracle_command.Parameters("vData1").Value = strParam

        '設定 result為 輸出參數
        oracle_command.Parameters.Add("result", OracleType.Cursor, 5000)
        oracle_command.Parameters("result").Direction = ParameterDirection.Output

        oracle_connect.Open()
        dr = oracle_command.ExecuteReader

        '不用迴圈,僅取第1筆資料
        If dr.Read() Then
            strReturn = dr.GetValue(0).ToString()
        End If
        'Response.Write("<script language=javascript>alert('strReturn=「" & strReturn & "」')</script>")
        dr.Close()
        oracle_connect.Close()


        Return strReturn
    End Function

End Class

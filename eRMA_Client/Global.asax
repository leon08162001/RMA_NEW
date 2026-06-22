<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 應用程式啟動時執行的程式碼
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 應用程式關閉時執行的程式碼
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' 發生未處理錯誤時執行的程式碼
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 啟動新工作階段時執行的程式碼
        Session("_Title") = "RMA System"

        Session("_Identity") = ""           '身分:1.客戶, 2.公司(維修中心)
        Session("_CustomerID") = ""         '客戶代碼
        Session("_RepairID") = ""           '客戶-維修中心代碼(如:001,002,003)
        Session("_RepairName") = ""         '維修中心名稱
        Session("_RepairTEL") = ""          '維修中心電話

        Session("_UserID") = ""             '帳號
        Session("_UserName") = ""           '姓名
        Session("_DeptNO") = ""             '部門
        Session("_isManager") = ""          '是否為管理人(0.否,1.是)

        '角色:0.View、1.Receiver、2.Repair center、3.Sales、4.Shipping、9.Admin
        'ps:給身分2使用
        Session("_Role") = ""

        '目前正在使用的角色
        Session("_OperationRole") = ""
        Session("_OperationRoleName") = ""

        Session("_RepairCenter") = ""       '維修中心人員, 可維護哪些維修中心(維修中心代碼-->如:001,002,003)

        '權限範圍等級:0.By Center、1.All
        'ps:給身分2使用
        Session("_AuthorityLevel") = ""
        Session("_LanguageID") = "001"         '語系

        Session("_isPolicy") = False        '紀錄是否有顯示過Policy
        Session("_isTimeOut") = False
        Session("_isShowNotFound") = ""

        Session("UCError") = ""             '紀錄如果使用控制項有錯誤資料時,要處理
        Session("UCOK") = False             '紀錄如果使用控制項有錯誤資料時,要處理



        'Session("_UserID") = "admin"
        'Session("_UserName") = "系統管理員"
        'Session("_LanguageID") = "002"

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 工作階段結束時執行的程式碼。 
        ' 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        ' 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        ' 或 SQLServer，就不會引發這個事件。
    End Sub
</script>
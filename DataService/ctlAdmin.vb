Imports System.Configuration
Imports System.Data.OracleClient
Imports ICAT_OracleDAO




Public Class ctlAdmin
    Dim _dtadmin As New AccountDTO.ADMINDataTable

    Public Function Insert_Token_Logoin(ByVal AccountID As String, ByVal Token As String, ByVal CheckToken As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command


        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " INSERT INTO Token_Logoin(AccountID,Token,CheckToken) VALUES (:AccountID,:Token,:CheckToken) "
            oCommand.Parameters.AddWithValue(":AccountID", AccountID)
            oCommand.Parameters.AddWithValue(":Token", Token)
            oCommand.Parameters.AddWithValue(":CheckToken", CheckToken)
            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    Public Function Select_Token_Logoin(ByVal Token As String) As DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try


            oQuery.addWHERE("Token", ":Token", Token.Trim(), OracleType.VarChar)
            sCondition = sCondition & "  Token=:Token "
            sSQL = " select * from Token_Logoin where " & sCondition & "  and CHECKTOKEN = '0' "

            dt = oQuery.ExecuteDT(sSQL)

            If dt.Rows.Count > 0 Then
                Up_Token_Logoin(Token)
            End If


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function Up_Token_Logoin(ByVal Token As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command


        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " UPDATE Token_Logoin SET CheckToken = '1' WHERE Token =:Token "
            oCommand.Parameters.AddWithValue(":Token", Token)
            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    Public Function LoginToken(ByVal AD_ID As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim dt As New DataTable
        Dim dtadmin As New AccountDTO.ADMINDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try

            'ToLower
            'oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID.ToLower(), OracleType.VarChar)
            'sCondition = sCondition & " AND lower(AD_ID)=:AD_ID"

            oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID, OracleType.VarChar)
            sCondition = sCondition & " AND AD_ID=:AD_ID"


            '帳號狀態:0.Close、1.Open
            oQuery.addWHERE("AD_VISIBLE", ":AD_VISIBLE", "1", OracleType.VarChar)
            sCondition = sCondition & " AND AD_VISIBLE=:AD_VISIBLE"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtadmin)
            If dtadmin.Rows.Count > 0 Then
                _dtadmin = dtadmin.Copy
                blnFlag = True
            End If

        Catch ex As Exception
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return blnFlag
    End Function
    ''' <summary>
    ''' 儲存LOG
    ''' </summary>
    ''' <param name="NoValue">傳入單號</param>
    ''' <param name="LogValue">傳入LogData</param>
    ''' <remarks></remarks>
    Public Sub addLog(ByVal NoValue As String, ByVal LogValue As String)
        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_LOGN"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vNo", OracleType.NVarChar).Value = NoValue
            oCommand.Parameters("vNo").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
            oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            'Throw ex

        Finally
            oCommand.Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' 登入
    ''' </summary>
    ''' <param name="AD_ID">傳入帳號</param>
    ''' <param name="AD_PASSWORD">傳入密碼</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:失敗, True:成功</remarks>
    Public Function Login(ByVal AD_ID As String, ByVal AD_PASSWORD As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim dt As New DataTable
        Dim dtadmin As New AccountDTO.ADMINDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try

            'ToLower
            'oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID.ToLower(), OracleType.VarChar)
            'sCondition = sCondition & " AND lower(AD_ID)=:AD_ID"

            oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID, OracleType.VarChar)
            sCondition = sCondition & " AND AD_ID=:AD_ID"

            oQuery.addWHERE("AD_PASSWORD", ":AD_PASSWORD", AD_PASSWORD, OracleType.VarChar)
            sCondition = sCondition & " AND AD_PASSWORD=:AD_PASSWORD"

            '帳號狀態:0.Close、1.Open
            oQuery.addWHERE("AD_VISIBLE", ":AD_VISIBLE", "1", OracleType.VarChar)
            sCondition = sCondition & " AND AD_VISIBLE=:AD_VISIBLE"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtadmin)
            If dtadmin.Rows.Count > 0 Then
                _dtadmin = dtadmin.Copy
                blnFlag = True
            End If

        Catch ex As Exception
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return blnFlag
    End Function

    ''' <summary>
    ''' 取得登入成功的admin資料
    ''' </summary>
    ''' <returns>傳回 adminDataTable</returns>
    ''' <remarks></remarks>
    Public Function getLoginData() As AccountDTO.ADMINDataTable
        Return _dtadmin
    End Function

    ''' <summary>
    ''' 取得維修中心的帳號資料
    ''' </summary>
    ''' <param name="ad_RepairCenter">公司代碼(維修中心)</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByRepairCenter(ByVal ad_RepairCenter As String, Optional ByVal OrderBY As String = "") As AccountDTO.ADMINDataTable
        Dim dt As New DataTable
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " AD_CSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            'ad_RepairCenter = "%" & ad_RepairCenter & "%"
            'oQuery.addWHERE("ad_RepairCenter", ":ad_RepairCenter", ad_RepairCenter, OracleType.VarChar)
            'sCondition = sCondition & " AND ad_RepairCenter like :ad_RepairCenter"
            sCondition = sCondition & " AND regexp_like(ad_RepairCenter, '(^|\s|\W)" + ad_RepairCenter + "($|\s|\W)')"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtAdmin)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtAdmin


    End Function

    ''' <summary>
    ''' 取得維修中心的帳號資料
    ''' </summary>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryAll(Optional ByVal OrderBY As String = "") As AccountDTO.ADMINDataTable
        Dim dt As New DataTable
        Dim dtAdmin As New AccountDTO.ADMINDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " AD_CSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            Dim sSQL As String = "SELECT * FROM ADMIN " & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtAdmin)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtAdmin


    End Function

    ''' <summary>
    ''' 取得admin資料
    ''' </summary>
    ''' <param name="AD_ID">帳號</param>
    ''' <param name="sRole">角色</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 FAQClassDataTable</remarks>
    Public Function Query(ByVal AD_ID As String, ByVal sRole As String, Optional ByVal OrderBY As String = "") As AccountDTO.ADMINDataTable
        Dim dt As New DataTable
        Dim dtAdmin As New AccountDTO.ADMINDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " AD_CSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If AD_ID.ToString().Trim() <> "" Then
                oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND AD_ID=:AD_ID"
            End If

            If sRole.ToString().Trim() <> "" Then
                oQuery.addWHERE("AD_ROLE", ":AD_ROLE", sRole, OracleType.VarChar)
                sCondition = sCondition & " AND AD_ROLE=:AD_ROLE"
            End If

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtAdmin)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtAdmin
    End Function

    ''' <summary>
    ''' 取得 直屬主管 mail
    ''' </summary>
    ''' <param name="AD_ID">帳號</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getUpperSuperMaill(ByVal AD_ID As String) As String
        Dim retval As String = ""

        Dim dt As New DataTable
        Dim sCondition As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID, OracleType.VarChar)
            sCondition = sCondition & " AND AD_ID=:AD_ID"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition
            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("AD_UPPERSUPERMIAL").ToString().Trim()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 取得 登入者部門
    ''' </summary>
    ''' <param name="AD_ID">帳號</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getLoginDeptNO(ByVal AD_ID As String) As String
        Dim retval As String = ""

        Dim dt As New DataTable
        Dim sCondition As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("gen01", ":gen01", AD_ID, OracleType.VarChar)
            sCondition = sCondition & " AND gen01=:gen01"

            Dim sSQL As String = "SELECT gen03 FROM cipherlab.gen_file WHERE 1=1 " & sCondition
            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("gen03").ToString().Trim()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 取得 mail
    ''' </summary>
    ''' <param name="AD_ID">帳號</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getMail(ByVal AD_ID As String) As String
        Dim retval As String = ""

        Dim dt As New DataTable
        Dim sCondition As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("AD_ID", ":AD_ID", AD_ID, OracleType.VarChar)
            sCondition = sCondition & " AND AD_ID=:AD_ID"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1 " & sCondition
            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("AD_EMAIL").ToString().Trim()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' admin-新增
    ''' </summary>
    ''' <param name="dtadmin">傳入ADMINDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveAdd(ByVal dtadmin As AccountDTO.ADMINDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim dr As AccountDTO.ADMINRow = dtadmin.Rows(i)

            If chkadmin(dr.AD_ID) = True Then
                Throw New ArgumentException("帳號已重複, 請確認...!!")
            End If

            Dim AD_PASSWORD As String = oCrypto.Encrypt(dr.AD_PASSWORD.Trim(), "")

            oExecute.addParameter("AD_ID", dr.AD_ID, OracleType.VarChar)
            oExecute.addParameter("AD_NAME", dr.AD_NAME, OracleType.NVarChar)
            oExecute.addParameter("AD_PASSWORD", AD_PASSWORD, OracleType.VarChar)

            oExecute.addParameter("AD_REPAIRCENTER", dr.AD_REPAIRCENTER, OracleType.VarChar)
            oExecute.addParameter("AD_ROLE", dr.AD_ROLE, OracleType.VarChar)
            oExecute.addParameter("AD_VISIBLE", dr.AD_VISIBLE, OracleType.Int16)
            oExecute.addParameter("AD_AUTHORITYLEVEL", dr.AD_AUTHORITYLEVEL, OracleType.Int16)
            oExecute.addParameter("AD_UPPERSUPERMIAL", dr.AD_UPPERSUPERMIAL, OracleType.VarChar)

            If dr.IsAD_EMAILNull = False Then oExecute.addParameter("AD_EMAIL", dr.AD_EMAIL, OracleType.VarChar)

            oExecute.addParameter("AD_AD", dr.AD_AD, OracleType.NVarChar)
            oExecute.addParameter("AD_ADNAME", dr.AD_ADNAME, OracleType.NVarChar)
            oExecute.addParameter("AD_CSTMP", dr.AD_CSTMP, OracleType.DateTime)
            oExecute.addParameter("AD_LUAD", dr.AD_LUAD, OracleType.NVarChar)
            oExecute.addParameter("AD_LUADNAME", dr.AD_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("AD_LUSTMP", dr.AD_LUSTMP, OracleType.DateTime)

            oExecute.Command("ADMIN", Execute.eumCommandType.AddNew)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' Admin-修改
    ''' </summary>
    ''' <param name="dtAdmin">傳入ADMINDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtAdmin As AccountDTO.ADMINDataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dr As AccountDTO.ADMINRow = dtAdmin.Rows(0)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            Dim AD_PASSWORD As String = oCrypto.Encrypt(dr.AD_PASSWORD.Trim(), "")

            oExecute.addParameter("AD_NAME", dr.AD_NAME, OracleType.NVarChar)
            oExecute.addParameter("AD_PASSWORD", AD_PASSWORD, OracleType.VarChar)

            oExecute.addParameter("AD_REPAIRCENTER", dr.AD_REPAIRCENTER, OracleType.VarChar)
            oExecute.addParameter("AD_ROLE", dr.AD_ROLE, OracleType.VarChar)
            oExecute.addParameter("AD_VISIBLE", dr.AD_VISIBLE, OracleType.Int16)
            oExecute.addParameter("AD_AUTHORITYLEVEL", dr.AD_AUTHORITYLEVEL, OracleType.Int16)
            oExecute.addParameter("AD_UPPERSUPERMIAL", dr.AD_UPPERSUPERMIAL, OracleType.VarChar)

            If dr.IsAD_EMAILNull = False Then oExecute.addParameter("AD_EMAIL", dr.AD_EMAIL, OracleType.VarChar)

            oExecute.addParameter("AD_AD", dr.AD_AD, OracleType.NVarChar)
            oExecute.addParameter("AD_ADNAME", dr.AD_ADNAME, OracleType.NVarChar)
            oExecute.addParameter("AD_CSTMP", dr.AD_CSTMP, OracleType.DateTime)
            oExecute.addParameter("AD_LUAD", dr.AD_LUAD, OracleType.NVarChar)
            oExecute.addParameter("AD_LUADNAME", dr.AD_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("AD_LUSTMP", dr.AD_LUSTMP, OracleType.DateTime)

            oExecute.addWHERE("AD_ID", dr.AD_ID.ToString().Trim(), OracleType.VarChar)
            oExecute.Command("Admin", Execute.eumCommandType.UPDATE)

            blnFlag = True

        Catch ex As Exception
            blnFlag = False
            Throw ex

        Finally
            If blnFlag = False Then
                oConn.Rollback()
            Else
                oConn.Commit()
            End If

            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' 變更密碼
    ''' </summary>
    ''' <param name="ADID">傳入帳號</param>
    ''' <param name="Password">傳入密碼</param>
    ''' <param name="UpperMail">UpperMail</param>
    ''' <param name="EMail">Mail</param>
    ''' <param name="AD_LUAD">最後修改人帳號</param>
    ''' <param name="AD_LUADNAME">最後修改人姓名</param>
    ''' <remarks></remarks>
    Public Sub ChangePassword(ByVal ADID As String, ByVal Password As String, ByVal UpperMail As String, ByVal EMail As String, _
        ByVal AD_LUAD As String, ByVal AD_LUADNAME As String)

        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            Dim AD_PASSWORD As String = oCrypto.Encrypt(Password, "")
            oExecute.addParameter("AD_PASSWORD", AD_PASSWORD, OracleType.VarChar)
            oExecute.addParameter("AD_UPPERSUPERMIAL", UpperMail.Trim(), OracleType.VarChar)
            oExecute.addParameter("AD_EMAIL", EMail.Trim(), OracleType.VarChar)

            oExecute.addParameter("AD_LUAD", AD_LUAD.Trim(), OracleType.NVarChar)
            oExecute.addParameter("AD_LUADNAME", AD_LUADNAME.Trim(), OracleType.NVarChar)
            oExecute.addParameter("AD_LUSTMP", Date.Now, OracleType.DateTime)

            oExecute.addWHERE("AD_ID", ADID.Trim(), OracleType.VarChar)
            oExecute.Command("Admin", Execute.eumCommandType.UPDATE)

            blnFlag = True

        Catch ex As Exception
            blnFlag = False

        Finally
            If blnFlag = False Then
                oConn.Rollback()
            Else
                oConn.Commit()
            End If

            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' 檢核admin是否存在
    ''' </summary>
    ''' <param name="ad_ID">帳號</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Private Function chkadmin(ByVal ad_ID As String) As Boolean
        Dim retval As Boolean = False
        Dim dt As New DataTable
        Dim dtadmin As New AccountDTO.ADMINDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("AD_ID", ":AD_ID", ad_ID, OracleType.VarChar)
            sCondition = sCondition & " AND AD_ID=:AD_ID"

            Dim sSQL As String = "SELECT * FROM ADMIN WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtadmin)

            If dtadmin.Rows.Count > 0 Then
                retval = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    Public Function GetMailSet() As AccountDTO.MailSetDataTable
        Dim dtMailSet As New AccountDTO.MailSetDataTable
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()

        Try
            Dim sSQL As String = "SELECT * FROM MailSet WHERE 1=1"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtMailSet)
        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dtMailSet
    End Function

    Public Sub SaveMailSet(ByVal dtMailSet As AccountDTO.MailSetDataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()
            For i = 0 To dtMailSet.Rows.Count - 1
                Dim dr As AccountDTO.MailSetRow = dtMailSet.Rows(i)
                oExecute.addParameter("MAIL_ADDRESS", dr.Mail_Address, OracleType.NVarChar)
                oExecute.addWHERE("MAIL_ID", dr.Mail_ID, OracleType.VarChar)
                oExecute.Command("MAILSET", Execute.eumCommandType.UPDATE)
            Next
            blnFlag = True
        Catch ex As Exception
            blnFlag = False
            Throw ex

        Finally
            If blnFlag = False Then
                oConn.Rollback()
            Else
                oConn.Commit()
            End If

            oConn.Close()
            oConn.Dispose()
        End Try
    End Sub

End Class


Public Class ctlLoginInfo
    Dim _LanguageID As String = ConfigurationSettings.AppSettings("LanguageID")



    ''' <summary>
    ''' 檢核admin是否存在
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Private Function chkIsExist(ByVal CompanyID As String, ByVal AccountID As String) As Boolean
        Dim retval As Boolean = False
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            'ToLower
            'oQuery.addWHERE("LOGIN_CUNO", ":LOGIN_CUNO", AccountID.ToLower(), OracleType.VarChar)
            'sCondition = sCondition & " AND lower(LOGIN_CUNO)=:LOGIN_CUNO"

            'oQuery.addWHERE("LOGIN_ACCOUNTID", ":LOGIN_ACCOUNTID", AccountID.ToLower(), OracleType.VarChar)
            'sCondition = sCondition & " AND lower(LOGIN_ACCOUNTID)=:LOGIN_ACCOUNTID"

            oQuery.addWHERE("LOGIN_CUNO", ":LOGIN_CUNO", CompanyID, OracleType.VarChar)
            sCondition = sCondition & " AND LOGIN_CUNO=:LOGIN_CUNO"

            oQuery.addWHERE("LOGIN_ACCOUNTID", ":LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            sCondition = sCondition & " AND LOGIN_ACCOUNTID=:LOGIN_ACCOUNTID"

            Dim sSQL As String = "SELECT * FROM LoginInfo WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function



    ''' <summary>
    ''' admin-新增
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <remarks></remarks>
    Public Sub Save(ByVal CompanyID As String, ByVal AccountID As String)
        Dim i As Integer = 0

        Try
            If chkIsExist(CompanyID, AccountID) = True Then
                Call SaveEdit(CompanyID.Trim(), AccountID.Trim())
            Else
                Call SaveAdd(CompanyID.Trim(), AccountID.Trim())
            End If
        Catch ex As Exception
            Throw ex

        Finally

        End Try

    End Sub


    ''' <summary>
    ''' LoginInfo-新增
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <remarks></remarks>
    Private Sub SaveAdd(ByVal CompanyID As String, ByVal AccountID As String)
        Dim i As Integer = 0

        Dim dtLoginInfo As New AccountDTO.LoginInfoDataTable
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            oExecute.addParameter("LOGIN_CUNO", CompanyID, OracleType.VarChar)
            oExecute.addParameter("LOGIN_ACCOUNTID", AccountID, OracleType.NVarChar)
            oExecute.addParameter("LOGIN_DFLNO", _LanguageID, OracleType.VarChar)
            oExecute.addParameter("LOGIN_CSTMP", Date.Now, OracleType.DateTime)

            oExecute.Command("LoginInfo", Execute.eumCommandType.AddNew)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' LoginInfo-修改
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <remarks></remarks>
    Private Sub SaveEdit(ByVal CompanyID As String, ByVal AccountID As String)
        Dim i As Integer = 0

        Dim dtLoginInfo As New AccountDTO.LoginInfoDataTable
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            oExecute.addParameter("LOGIN_CSTMP", Date.Now, OracleType.DateTime)

            oExecute.addWHERE("LOGIN_CUNO", CompanyID, OracleType.VarChar)
            oExecute.addWHERE("LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            oExecute.Command("LoginInfo", Execute.eumCommandType.UPDATE)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' LoginInfo-變更語系
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <param name="LOGIN_DFLNO">變更語系代碼</param>
    ''' <remarks></remarks>
    Public Sub ChangeLanguage(ByVal CompanyID As String, ByVal AccountID As String, ByVal LOGIN_DFLNO As String)
        Dim dtLoginInfo As New AccountDTO.LoginInfoDataTable
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()


            oExecute.addParameter("LOGIN_DFLNO", LOGIN_DFLNO, OracleType.VarChar)
            oExecute.addParameter("LOGIN_CSTMP", Date.Now, OracleType.DateTime)

            oExecute.addWHERE("LOGIN_CUNO", CompanyID, OracleType.VarChar)
            oExecute.addWHERE("LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            oExecute.Command("LoginInfo", Execute.eumCommandType.UPDATE)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' 取得使用者目前的 語系
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="AccountID">帳號</param>
    ''' <returns>回傳語系</returns>
    ''' <remarks></remarks>
    Public Function getLanguageID(ByVal CompanyID As String, ByVal AccountID As String) As String
        Dim retval As String = _LanguageID
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("LOGIN_CUNO", ":LOGIN_CUNO", CompanyID, OracleType.VarChar)
            sCondition = sCondition & " AND LOGIN_CUNO=:LOGIN_CUNO"

            oQuery.addWHERE("LOGIN_ACCOUNTID", ":LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            sCondition = sCondition & " AND LOGIN_ACCOUNTID=:LOGIN_ACCOUNTID"

            Dim sSQL As String = "SELECT * FROM LoginInfo WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("LOGIN_DFLNO").ToString().Trim()
            Else
                retval = "001"
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 取得客戶目前的 語系
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="sRMA_NO">RMA單號</param>
    ''' <returns>回傳語系</returns>
    ''' <remarks></remarks>
    Public Function getLanguageIDRMANO(ByVal CompanyID As String, ByVal sRMA_NO As String)
        Dim retval As String = _LanguageID
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("LOGIN_CUNO", ":LOGIN_CUNO", CompanyID, OracleType.VarChar)
            sCondition = sCondition & " AND a.LOGIN_CUNO=:LOGIN_CUNO"

            'oQuery.addWHERE("LOGIN_ACCOUNTID", ":LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            'sCondition = sCondition & " AND a.LOGIN_ACCOUNTID=:LOGIN_ACCOUNTID"

            oQuery.addWHERE("RMA_NO", ":RMA_NO", sRMA_NO, OracleType.VarChar)

            Dim sSQL As String = "SELECT a.* FROM LoginInfo a JOIN RMA b ON b.RMA_NO=:RMA_NO AND b.RMA_ACCOUNTID = a.LOGIN_ACCOUNTID WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("LOGIN_DFLNO").ToString().Trim()
            Else
                retval = "001"
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' 取得客戶目前的 語系
    ''' </summary>
    ''' <param name="CompanyID">公司代碼 或 管理人員代碼</param>
    ''' <param name="sRMA_ID">RMA ID</param>
    ''' <returns>回傳語系</returns>
    ''' <remarks></remarks>
    Public Function getLanguageIDRMAID(ByVal CompanyID As String, ByVal sRMA_ID As String)
        Dim retval As String = _LanguageID
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("LOGIN_CUNO", ":LOGIN_CUNO", CompanyID, OracleType.VarChar)
            sCondition = sCondition & " AND a.LOGIN_CUNO=:LOGIN_CUNO"

            'oQuery.addWHERE("LOGIN_ACCOUNTID", ":LOGIN_ACCOUNTID", AccountID, OracleType.VarChar)
            'sCondition = sCondition & " AND a.LOGIN_ACCOUNTID=:LOGIN_ACCOUNTID"

            oQuery.addWHERE("RMA_ID", ":RMA_ID", sRMA_ID, OracleType.VarChar)

            Dim sSQL As String = "SELECT a.* FROM LoginInfo a JOIN RMA b ON b.RMA_ID=:RMA_ID AND b.RMA_ACCOUNTID = a.LOGIN_ACCOUNTID WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("LOGIN_DFLNO").ToString().Trim()
            Else
                retval = "001"
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retval
    End Function


End Class
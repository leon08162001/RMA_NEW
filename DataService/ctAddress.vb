Imports System.Data.OracleClient
Imports ICAT_OracleDAO
Imports Microsoft.VisualBasic.CompilerServices


Public Class ctAddress
    Public Function Insert_Token_Logoin(ByVal AccountID As String, ByVal Token As String, ByVal CheckToken As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " INSERT INTO Token_Logoin(AccountID,Token,CheckToken) VALUES (:AccountID,:Token,:CheckToken) "
            oracleCommand.Parameters.AddWithValue(":AccountID", AccountID)
            oracleCommand.Parameters.AddWithValue(":Token", Token)
            oracleCommand.Parameters.AddWithValue(":CheckToken", CheckToken)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Select_Token_Logoin(ByVal Token As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim dataTable As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            query.addWHERE("Token", ":Token", Token.Trim(), OracleType.VarChar)
            text2 += "  Token=:Token "
            text = " select * from Token_Logoin where " & text2 & "  and CHECKTOKEN = '0' "
            dataTable = query.ExecuteDT(text)

            If dataTable.Rows.Count > 0 Then
                Up_Token_Logoin(Token)
            End If

        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return dataTable
    End Function

    Public Function ProductInformation_03_Delete(ByVal RMAD_ID As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = "      DELETE FROM RMADETAIL  WHERE RMAD_ID =:RMAD_ID "
            oracleCommand.Parameters.AddWithValue(":RMAD_ID", RMAD_ID)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Up_Token_Logoin(ByVal Token As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " UPDATE Token_Logoin SET CheckToken = '1' WHERE Token =:Token "
            oracleCommand.Parameters.AddWithValue(":Token", Token)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function select_RMADETAIL(ByVal sRMANo As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(sRMANo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMAD_RMANO", ":RMAD_RMANO", sRMANo.Trim(), OracleType.VarChar)
                text2 += " AND RMAD_RMANO=:RMAD_RMANO"
            End If

            text = " select RMAD_ID from RMADETAIL where 1 = 1 " & text2 & " order by RMAD_STATUS "
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function chk_CW(ByVal EXPORT_SERIALNO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(EXPORT_SERIALNO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO.Trim(), OracleType.VarChar)
                text2 += " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"
            End If

            text = " SELECT EXPORT_SERIALNO " & vbCrLf & "            FROM EXPORT Where 1 = 1" & vbCrLf & "            AND ( NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'CW' OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'EB' OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'E0'  OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'P0'  OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'PB') " & vbCrLf & "            AND ( CW_EDATE > sysdate)  " & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function chk_CW_List(ByVal EXPORT_SERIALNO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(EXPORT_SERIALNO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO.Trim(), OracleType.VarChar)
                text2 += " AND EXPORT_SERIALNO in (:EXPORT_SERIALNO)"
            End If

            text = " SELECT EXPORT_SERIALNO " & vbCrLf & "            FROM EXPORT Where 1 = 1" & vbCrLf & "            AND ( NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'CW' OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'EB' OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'E0'  OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'P0'  OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'PB') " & vbCrLf & "            AND ( CW_EDATE > sysdate)  " & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function QueryByExport_List(ByVal EXPORT_SERIALNO As String) As RmaDTO.ExportDataTable
        Dim flag As Boolean = False
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim exportDataTable As RmaDTO.ExportDataTable = New RmaDTO.ExportDataTable()
        Dim dataTable As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            query.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO, OracleType.VarChar)
            text2 = " AND EXPORT_SERIALNO in (:EXPORT_SERIALNO)"
            text = "SELECT * FROM EXPORT WHERE 1=1 " & text2
            dataTable = query.ExecuteDT(text)
            Dim dtSource As DataTable = dataTable
            Dim dtTarget As DataTable = exportDataTable
            Common.TransferDataTable(dtSource, dtTarget)
            exportDataTable = CType(dtTarget, RmaDTO.ExportDataTable)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return exportDataTable
    End Function

    Public Function Query_RMA_Cu_No(ByVal Cu_No As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(Cu_No.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMA_CUNO", ":RMA_CUNO", Cu_No.Trim(), OracleType.VarChar)
                text2 += " AND RMA_CUNO=:RMA_CUNO"
            End If

            text = " SELECT RMA_NO  FROM ( SELECT RMA_NO  FROM RMA WHERE 1=1" & text2 & "  order by RMA_CSTMP desc ) WHERE ROWNUM = 1"
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Query_EXPORT(ByVal Cu_No As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(Cu_No.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", Cu_No.Trim(), OracleType.VarChar)
                text2 += " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"
            End If

            text = "SELECT EXPORT_PARTNO,EXPORT_SERIALNO,EXPORT_WAR_ID" & vbCrLf & "                ,EW_SDATE            " & vbCrLf & "                ,CW_SDATE             " & vbCrLf & "                ,CW_EDATE              " & vbCrLf & "                ,SW_SDATE             " & vbCrLf & "                ,SW_EDATE         " & vbCrLf & "                FROM EXPORT WHERE 1=1" & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Query_EXPORT_EXPORT_WAR_ID(ByVal Cu_No As String, ByVal EXPORT_WAR_ID As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(Cu_No.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", Cu_No.Trim(), OracleType.VarChar)
                text2 += " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"
            End If

            If Operators.CompareString(EXPORT_WAR_ID.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                EXPORT_WAR_ID = "%" & EXPORT_WAR_ID.Trim() & "%"
                query.addWHERE("EXPORT_WAR_ID", ":EXPORT_WAR_ID", EXPORT_WAR_ID, OracleType.NVarChar)
                text2 += " AND EXPORT_WAR_ID like :EXPORT_WAR_ID"
            End If

            text = "SELECT EXPORT_PARTNO,EXPORT_SERIALNO,EXPORT_WAR_ID" & vbCrLf & "                ,EW_SDATE            " & vbCrLf & "                ,CW_SDATE             " & vbCrLf & "                ,CW_EDATE              " & vbCrLf & "                ,SW_SDATE             " & vbCrLf & "                ,SW_EDATE         " & vbCrLf & "                FROM EXPORT WHERE 1=1" & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Query_Address(ByVal Cu_No As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(Cu_No.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("CU_NO", ":CU_NO", Cu_No.Trim(), OracleType.VarChar)
                text2 += " AND CU_NO=:CU_NO"
            End If

            text = "SELECT CU_ADDRESS FROM CADDRESS WHERE 1=1" & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function DELETE_Address(ByVal Cu_No As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " DELETE FROM CADDRESS WHERE CU_NO  = :CU_NO "
            oracleCommand.Parameters.AddWithValue(":CU_NO", Cu_No)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Insert_Address(ByVal Cu_No As String, ByVal CU_ADDRESS_NO As Integer, ByVal CU_ADDRESS As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " insert into  CADDRESS(Cu_No, CU_ADDRESS_NO, CU_ADDRESS) values(:CU_NO,:CU_ADDRESS_NO,:CU_ADDRESS)  "
            oracleCommand.Parameters.AddWithValue(":CU_NO", Cu_No)
            oracleCommand.Parameters.AddWithValue(":CU_ADDRESS_NO", CU_ADDRESS_NO)
            oracleCommand.Parameters.AddWithValue(":CU_ADDRESS", CU_ADDRESS)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Up_RMADETAIL(ByVal RMAD_ID As String, ByVal RMAD_SERIALNO As String, ByVal CUSTOMER_PRODUCT_NUMBER As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " UPDATE RMADETAIL SET CUSTOMER_PRODUCT_NUMBER =:CUSTOMER_PRODUCT_NUMBER WHERE RMAD_ID =:RMAD_ID   "
            oracleCommand.Parameters.AddWithValue(":RMAD_ID", RMAD_ID)
            oracleCommand.Parameters.AddWithValue(":CUSTOMER_PRODUCT_NUMBER", CUSTOMER_PRODUCT_NUMBER)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Up_RMADETAIL_New(ByVal RMAD_RMANO As String, ByVal RMAD_SERIALNO As String, ByVal CUSTOMER_PRODUCT_NUMBER As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " UPDATE RMADETAIL SET CUSTOMER_PRODUCT_NUMBER =:CUSTOMER_PRODUCT_NUMBER WHERE RMAD_RMANO in (select RMA_NO from RMA where RMA_ID =:RMA_ID) and  RMAD_SERIALNO =:RMAD_SERIALNO  "
            oracleCommand.Parameters.AddWithValue(":RMA_ID", RMAD_RMANO)
            oracleCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)
            oracleCommand.Parameters.AddWithValue(":CUSTOMER_PRODUCT_NUMBER", CUSTOMER_PRODUCT_NUMBER)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Select_RMA_COMPNO(ByVal RMA_COMPNO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " select COMP_ADDRESS from COMPANY  where   COMP_NO =:COMP_NO   "
            query.addWHERE("COMP_NO", ":COMP_NO", RMA_COMPNO.Trim(), OracleType.VarChar)
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Select_Count_CheckingReport(ByVal RMA_NO As String, ByVal RMAD_SERIALNO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " select count(RMA_NO) as A from CHECKINGREPORT  where   RMA_NO =:RMA_NO  and RMAD_SERIALNO =:RMAD_SERIALNO "
            query.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
            query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO.Trim(), OracleType.VarChar)
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Insert_CheckingReport(ByVal ID As String, ByVal CU_NO As String, ByVal RMA_NO As String, ByVal RMAD_SERIALNO As String, ByVal CONTEXT As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " insert into  CHECKINGREPORT(ID, CU_NO, RMA_NO,RMAD_SERIALNO,CONTEXT) values(:ID, :CU_NO, :RMA_NO,:RMAD_SERIALNO,:CONTEXT)   "
            oracleCommand.Parameters.AddWithValue(":ID", ID)
            oracleCommand.Parameters.AddWithValue(":CU_NO", CU_NO)
            oracleCommand.Parameters.AddWithValue(":RMA_NO", RMA_NO)
            oracleCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)
            oracleCommand.Parameters.AddWithValue(":CONTEXT", CONTEXT)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Select_CheckingReport(ByVal CU_NO As String, ByVal RMA_NO As String, ByVal RMAD_SERIALNO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " select * from CHECKINGREPORT  where  CU_NO =:CU_NO and RMA_NO =:RMA_NO  and RMAD_SERIALNO =:RMAD_SERIALNO "
            query.addWHERE("CU_NO", ":CU_NO", CU_NO.Trim(), OracleType.VarChar)
            query.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
            query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO.Trim(), OracleType.VarChar)
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Up_CheckingReport(ByVal RMA_NO As String, ByVal RMAD_SERIALNO As String, ByVal CONTEXT As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " UPDATE  CHECKINGREPORT Set CONTEXT =:CONTEXT  where  RMA_NO =:RMA_NO and  RMAD_SERIALNO =:RMAD_SERIALNO "
            oracleCommand.Parameters.AddWithValue(":RMA_NO", RMA_NO)
            oracleCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)
            oracleCommand.Parameters.AddWithValue(":CONTEXT", CONTEXT)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Del_CheckingReport(ByVal RMA_NO As String, ByVal RMAD_SERIALNO As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " DELETE FROM  CHECKINGREPORT  where  RMA_NO =:RMA_NO and  RMAD_SERIALNO =:RMAD_SERIALNO "
            oracleCommand.Parameters.AddWithValue(":RMA_NO", RMA_NO)
            oracleCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Del_OrderLog(ByVal RMA_NO As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " DELETE FROM OrderLog WHERE RMA_NO =:RMA_NO   "
            oracleCommand.Parameters.AddWithValue(":RMA_NO", RMA_NO)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Insert_OrderLog(ByVal ID As String, ByVal CU_NO As String, ByVal RMA_NO As String) As Object
        Dim connection As Connection = New Connection()
        Dim oracleCommand As OracleCommand = connection.Command()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try
            text = " insert into  OrderLog(ID, CU_NO, RMA_NO) values(:ID,:CU_NO,:RMA_NO)   "
            oracleCommand.Parameters.AddWithValue(":ID", ID)
            oracleCommand.Parameters.AddWithValue(":CU_NO", CU_NO)
            oracleCommand.Parameters.AddWithValue(":RMA_NO", RMA_NO)
            oracleCommand.CommandText = text
            oracleCommand.ExecuteNonQuery()
            Dim result As Object = Nothing
            Return result
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Function

    Public Function Select_OrderLog(ByVal CU_NO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(CU_NO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("CU_NO", ":CU_NO", CU_NO.Trim(), OracleType.VarChar)
                text2 += " where CU_NO=:CU_NO"
            End If

            text = "select " & vbCrLf & "A.RMA_NO," & vbCrLf & "RMA_EUCOMPANY," & vbCrLf & "RMA_EUNAME," & vbCrLf & "RMA_EUTEL from ORDERLOG A left join RMA B on  A.RMA_NO  = B.RMA_NO " & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Select_RMA(ByVal RMA_NO As String) As DataTable
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim result As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(RMA_NO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
                text2 += " AND RMA_NO=:RMA_NO"
            End If

            text = "SELECT * FROM RMA WHERE 1=1" & text2
            result = query.ExecuteDT(text)
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Get_CUSTOMER_PRODUCT_NUMBER(ByVal RMARQD_RMADID As String) As String
        Dim result As String = ""
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim dataTable As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(RMARQD_RMADID.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMAD_ID", ":RMAD_ID", RMARQD_RMADID.Trim(), OracleType.VarChar)
                text2 += " AND RMAD_ID=:RMAD_ID"
            End If

            text = "SELECT CUSTOMER_PRODUCT_NUMBER FROM RMADETAIL WHERE 1=1" & text2
            dataTable = query.ExecuteDT(text)
            result = dataTable.Rows(0)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function

    Public Function Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(ByVal RMAD_RMANO As String, ByVal RMAD_SERIALNO As String) As String
        Dim result As String = ""
        Dim connection As Connection = New Connection()
        Dim query As Query = New Query(connection)
        Dim dataTable As DataTable = New DataTable()
        Dim text As String = ""
        Dim text2 As String = ""
        connection.Open()

        Try

            If Operators.CompareString(RMAD_RMANO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO.Trim(), OracleType.VarChar)
                text2 += " AND RMAD_RMANO=:RMAD_RMANO"
            End If

            If Operators.CompareString(RMAD_SERIALNO.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO.Trim(), OracleType.VarChar)
                text2 += " AND RMAD_SERIALNO=:RMAD_SERIALNO"
            End If

            text = "  SELECT CUSTOMER_PRODUCT_NUMBER FROM RMADETAIL  WHERE 1=1" & text2
            dataTable = query.ExecuteDT(text)
            result = (If((dataTable.Rows.Count <> 0), dataTable.Rows(0)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim(), ""))
        Catch ex As Exception
            ProjectData.SetProjectError(ex)
            Dim ex2 As Exception = ex
            Throw ex2
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return result
    End Function
End Class

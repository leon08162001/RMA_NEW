Imports System.Data.OracleClient
Imports DefLanguage
Imports ICAT_OracleDAO
Public Class ctlCustomer

#Region "Class:Customer"
    Public Class Customer
        Dim _oLanguage As New ctlLanguage

        Public Function QueryALL() As DataTable


            Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                '將對應業務、業管email加入現有報表 by buck modify 20251030 begin
                'sSQL = "SELECT * FROM CUSTOMER"
                sSQL = "
                            SELECT cu.*,ads.AD_EMAIL AS CU_SALES_EMAIL,ada.AD_EMAIL AS CU_ASSISTANT_EMAIL
                            FROM CUSTOMER cu
                            LEFT JOIN admin ads ON cu.CU_SALESID = ads.AD_ID
                            LEFT JOIN admin ada ON cu.CU_ASSISTANTID = ada.AD_ID
                        "
                '將對應業務、業管email加入現有報表 by buck modify 20251030 end
                dt = oQuery.ExecuteDT(sSQL)


            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' 取得Customer資料
        ''' </summary>
        ''' <param name="Cu_No">客戶編號</param>
        ''' <param name="Cu_Name">客戶名稱</param>
        ''' <param name="Cu_Status">帳號狀態 (1:開啟 , 0:關閉)</param>
        ''' <param name="Cu_CompNo">公司代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 CustomerDataTable</returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal Cu_No As String, ByVal Cu_Name As String, ByVal Cu_Status As Integer,
                ByVal Cu_CompNo As String, ByVal sCountry As String, Optional ByVal OrderBY As String = "") As CustomerDTO.VWCUSTOMERDataTable

            Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If Cu_No.ToString().Trim() <> "" Then
                    oQuery.addWHERE("CU_NO", ":CU_NO", Cu_No.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND CU_NO=:CU_NO"
                End If

                If Cu_Name.ToString().Trim() <> "" Then
                    Cu_Name = "%" & Cu_Name & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", Cu_Name.Trim().ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Cu_CompNo.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("CU_COMPNO", ":CU_COMPNO", Cu_CompNo, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_COMPNO=:CU_COMPNO"
                End If

                If Cu_Status.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("CU_STATUS", ":CU_STATUS", Cu_Status, OracleType.Int16)
                    sCondition = sCondition & " AND CU_STATUS=:CU_STATUS"
                End If

                If sCountry.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("CU_COUNTRYID", ":CU_COUNTRYID", sCountry, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_COUNTRYID=:CU_COUNTRYID"
                End If

                sSQL = "SELECT * FROM vwCustomer WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        Public Function Query01(ByVal Cu_No As String, ByVal Cu_Name As String, ByVal Cu_Status As Integer,
                ByVal Cu_CompNo As String, ByVal Cu_Sales As String, Optional ByVal OrderBY As String = "") As CustomerDTO.VWCUSTOMERDataTable

            Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If Cu_No.ToString().Trim() <> "" Then
                    oQuery.addWHERE("CU_NO", ":CU_NO", Cu_No.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND CU_NO=:CU_NO"
                End If

                If Cu_Name.ToString().Trim() <> "" Then
                    Cu_Name = "%" & Cu_Name & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", Cu_Name.Trim().ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Cu_CompNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("CU_COMPNO", ":CU_COMPNO", Cu_CompNo, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_COMPNO=:CU_COMPNO"
                End If

                If Cu_Sales.ToString().Trim() <> "" Then
                    oQuery.addWHERE("Cu_Sales", ":Cu_Sales", Cu_Sales, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_SALESID=:Cu_Sales"
                End If

                If Cu_Status.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("CU_STATUS", ":CU_STATUS", Cu_Status, OracleType.Int16)
                    sCondition = sCondition & " AND CU_STATUS=:CU_STATUS"
                End If

                sSQL = "SELECT * FROM vwCustomer WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        Public Function QrySKU(ByVal CustNo As String, ByVal sSKU As String) As DataTable
            Dim dtData As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                If CustNo.ToString().Trim() <> "" Then
					CustNo = "%" & CustNo & "%"
                    'oQuery.addWHERE("OEA03", ":OEA03", CustNo, OracleType.VarChar)
                    oQuery.addWHERE("OCI11", ":OCI11", CustNo, OracleType.VarChar)
                    oQuery.addWHERE("OCC01", ":OCC01", CustNo, OracleType.VarChar)
                    'sCondition = sCondition & " AND OEA03 = :OEA03"
                    'sCondition = sCondition & " AND decode(OEA03,'BD01','BD007',OEA03) = :OEA03"
                    '20191231 wisely modify 
                    sCondition = sCondition & " AND OEA03 IN (SELECT oci01 FROM cipherlab.oci_file WHERE oci03='60' AND oci02='10' "
                    sCondition = sCondition & "               AND OCI11 like :OCI11"
                    sCondition = sCondition & "               UNION "
                    sCondition = sCondition & "               SELECT occ01 FROM cipherlab.occ_file WHERE 1 = 1 "
                    sCondition = sCondition & "               AND OCC01 like :OCC01"
                    sCondition = sCondition & "              )"
                End If
                If sSKU.ToString().Trim() <> "" Then
                    sSKU = "%" & sSKU & "%"
                    oQuery.addWHERE("OEB04", ":OEB04", sSKU, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB04 LIKE :OEB04"
                End If

                sSQL = "SELECT OEB04,IMA021 AS OEB06 FROM cipherlab.oea_file a,cipherlab.oeb_file b , cipherlab.IMA_FILE where IMA_FILE.IMA01 = b.oeb04 AND a.oea01=b.oeb01 " & sCondition & " and oeb04 not like 'MISC%' group by OEB04,IMA021"

                dtData = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
            Return dtData
        End Function

        Public Function QrySKU_SH(ByVal CustNo As String, ByVal sSKU As String) As DataTable
            Dim dtData As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                If CustNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("OEA03", ":OEA03", CustNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEA03 = :OEA03"
                End If
                If sSKU.ToString().Trim() <> "" Then
                    sSKU = "%" & sSKU & "%"
                    oQuery.addWHERE("OEB04", ":OEB04", sSKU, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB04 LIKE :OEB04"
                End If

                sSQL = "SELECT OEB04,IMA021 AS OEB06 FROM ciphersh.oea_file a,ciphersh.oeb_file b , cipherlab.IMA_FILE   where IMA_FILE.IMA01 = b.oeb04 AND a.oea01=b.oeb01 " & sCondition & " and oeb04 not like 'MISC%' group by OEB04,IMA021"

                dtData = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
            Return dtData
        End Function

        Public Function QryOrder(ByVal CustNo As String, ByVal OrderNo As String, ByVal OrderSeq As String, ByVal PartNo As String) As DataTable
            Dim dtData As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                If CustNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("OEA03", ":OEA03", CustNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEA03 = :OEA03"
                End If
                If OrderNo.ToString().Trim() <> "" Then
                    OrderNo = "%" & OrderNo & "%"
                    oQuery.addWHERE("OEA01", ":OEA01", OrderNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEA01 like :OEA01"
                End If

                If OrderSeq.ToString().Trim() <> "" Then
                    oQuery.addWHERE("OEB03", ":OEB03", OrderSeq, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB03= :OEB03"
                End If

                If PartNo.ToString().Trim() <> "" Then
                    PartNo = "%" & PartNo & "%"
                    oQuery.addWHERE("OEB04", ":OEB04", PartNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB04 like :OEB04"
                End If

                If OrderNo.Trim() = "" AndAlso PartNo.Trim() = "" Then
                    sSQL = "SELECT OEA01,OEB03,OEB04,IMA021 AS OEB06,OEB12, IMA021 AS OEB06 FROM cipherlab.oea_file a,cipherlab.oeb_file b , cipherlab.IMA_FILE where  IMA_FILE.IMA01 = b.oeb04 AND TRUNC(SYSDATE-a.OEA02) <= 365  AND a.oea01=b.oeb01 " & sCondition & " order by oeb01,oeb03"
                Else
                    sSQL = "SELECT OEA01,OEB03,OEB04,IMA021 AS OEB06,OEB12, IMA021 AS OEB06 FROM cipherlab.oea_file a,cipherlab.oeb_file b , cipherlab.IMA_FILE where  IMA_FILE.IMA01 = b.oeb04   AND a.oea01=b.oeb01 " & sCondition & " order by oeb01,oeb03"
                End If


                dtData = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
            Return dtData
        End Function

        Public Function QryOrderSH(ByVal CustNo As String, ByVal OrderNo As String, ByVal OrderSeq As String, ByVal PartNo As String) As DataTable
            Dim dtData As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                If CustNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("OEA03", ":OEA03", CustNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEA03 = :OEA03"
                End If
                If OrderNo.ToString().Trim() <> "" Then
                    OrderNo = "%" & OrderNo & "%"
                    oQuery.addWHERE("OEA01", ":OEA01", OrderNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEA01 like :OEA01"
                End If

                If OrderSeq.ToString().Trim() <> "" Then
                    oQuery.addWHERE("OEB03", ":OEB03", OrderSeq, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB03= :OEB03"
                End If

                If PartNo.ToString().Trim() <> "" Then
                    PartNo = "%" & PartNo & "%"
                    oQuery.addWHERE("OEB04", ":OEB04", PartNo, OracleType.VarChar)
                    sCondition = sCondition & " AND OEB04 like :OEB04"
                End If

                sSQL = "SELECT OEA01,OEB03,OEB04,IMA021 AS OEB06,OEB12,IMA021 AS OEB06 FROM ciphersh.oea_file a,ciphersh.oeb_file b , cipherlab.IMA_FILE  where IMA_FILE.IMA01 = b.oeb04 AND a.oea01=b.oeb01 " & sCondition & " order by oeb01,oeb03"

                dtData = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
            Return dtData
        End Function

        ''' <summary>
        ''' 取得Customer資料
        ''' </summary>
        ''' <param name="KeyWord">關鍵字</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 vwCustomerDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryKeyWord(ByVal KeyWord As String, Optional ByVal OrderBY As String = "") As CustomerDTO.VWCUSTOMERDataTable

            Dim dtCustomer As New CustomerDTO.VWCUSTOMERDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If KeyWord.ToString().Trim() <> "" Then
                    KeyWord = "%" & KeyWord & "%"

                    oQuery.addWHERE("CU_NO", ":CU_NO", KeyWord.Trim().ToLower, OracleType.VarChar)
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", KeyWord.Trim().ToLower(), OracleType.NVarChar)

                    sCondition = sCondition & " AND (lower(CU_NO) like :CU_NO"
                    sCondition = sCondition & " OR lower(CU_NAME) like :CU_NAME)"
                End If


                sSQL = "SELECT * FROM vwCustomer WHERE CU_STATUS=1 " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 取得 Customer Repair 資料
        ''' </summary>
        ''' <param name="CuNo">傳入客戶編號</param>
        ''' <returns>傳回 vwCustomerDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByCompany(ByVal CuNo As String) As CustomerDTO.VWCUSTOMERDataTable
            Dim dtvwCustomer As New CustomerDTO.VWCUSTOMERDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("CU_NO", ":CU_NO", CuNo, OracleType.VarChar)
                sCondition = sCondition & " AND CU_NO=:CU_NO"

                sSQL = "SELECT * FROM vwCUSTOMER WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtvwCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtvwCustomer
        End Function

        ''' <summary>
        ''' 取得 客戶代碼 資料
        ''' </summary>
        ''' <param name="CU_No">客戶代碼</param>
        ''' <returns>傳回 CompanyDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByPrimaryKey(ByVal CU_No As String) As CustomerDTO.CustomerDataTable
            Dim sSQL As String = ""

            Dim dtCustomer As New CustomerDTO.CustomerDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("CU_No", ":CU_No", CU_No, OracleType.VarChar)

                sSQL = "SELECT * FROM Customer WHERE CU_No=:CU_No"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 取得 業務的 客戶資料
        ''' </summary>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="SalesID">業務代碼及業務助理代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>tmpCustomerBySaleDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryBySale(ByVal CuName As String, ByVal SalesID As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpCustomerBySaleDataTable
            Dim sCondition As String = ""
            Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '業務代碼及業務助理代碼
                'Dim sCuAssistantID As String = sCuSalesID.Trim()

                oQuery.addWHERE("CU_SALESID", ":CU_SALESID", SalesID.ToLower().Trim(), OracleType.VarChar)
                oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", SalesID.ToLower().Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND (lower(CU_SALESID)=:CU_SALESID OR lower(CU_ASSISTANTID)=:CU_ASSISTANTID)"

                'sCondition = sCondition & " AND CU_SALESID=:CU_SALESID"


                '客戶名稱
                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CU_STATUS", ":CU_STATUS", 1, OracleType.Int16)
                sCondition = sCondition & " AND (CU_STATUS=:CU_STATUS)"


                Dim sSQL As String = " SELECT CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID, "
                sSQL = sSQL & " CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 "
                sSQL = sSQL & " FROM CUSTOMER "
                sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " INNER JOIN CURRENCY ON COMPANY.COMP_CURRENCYCODE = CURRENCY.CURRENCY_CODE "
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 取得 維修中心的 客戶資料
        ''' </summary>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="SalesID">業務代碼及業務助理代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>tmpCustomerBySaleDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRepair(ByVal CuName As String, ByVal SalesID As String, ByVal CompNo As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpCustomerBySaleDataTable
            Dim sCondition As String = ""
            Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY


                '客戶名稱
                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CU_STATUS", ":CU_STATUS", 1, OracleType.Int16)
                sCondition = sCondition & " AND (CU_STATUS=:CU_STATUS)"

                '客戶維修中心點
                oQuery.addWHERE("CU_COMPNO", ":CU_COMPNO", CompNo, OracleType.VarChar)
                sCondition = sCondition & " AND (CU_COMPNO=:CU_COMPNO)"

                Dim sSQL As String = " SELECT CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID, "
                sSQL = sSQL & " CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 "
                sSQL = sSQL & " FROM CUSTOMER "
                sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " INNER JOIN CURRENCY ON COMPANY.COMP_CURRENCYCODE = CURRENCY.CURRENCY_CODE "
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 取得 mail
        ''' </summary>
        ''' <param name="CU_NO">客戶編號</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMail(ByVal CU_NO As String) As String
            Dim retval As String = ""

            Dim dt As New DataTable
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("CU_NO", ":CU_NO", CU_NO, OracleType.VarChar)
                sCondition = sCondition & " AND CU_NO=:CU_NO"

                Dim sSQL As String = "SELECT * FROM Customer WHERE 1=1 " & sCondition
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("CU_EMAIL").ToString().Trim()
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
        ''' 取得客戶代碼是否存在
        ''' </summary>
        ''' <param name="new_CUNO">新的客戶代碼</param>
        ''' <param name="old_CUNO">舊的客戶代碼</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:不存在, True:存在</remarks>
        Public Function chkIsExist(ByVal new_CUNO As String, Optional ByVal old_CUNO As String = "") As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If old_CUNO.Trim() <> "" Then
                    oQuery.addWHERE("CU_NO", ":old_CompNo", old_CUNO, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_NO<>:old_CUNO"
                End If

                oQuery.addWHERE("CU_NO", ":new_CUNO", new_CUNO, OracleType.VarChar)
                Dim sSQL As String = "SELECT * FROM Customer WHERE CU_NO=:new_CUNO " & sCondition

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
        ''' Customer-新增
        ''' </summary>
        ''' <param name="dtCustomer">傳入dtCustomer</param>
        ''' <remarks></remarks>
        ''' Add CU_WH TO F1 MODI BY ANGEL ON 20151202
        ''' ADD CU_TIPTOP_ID MODI BY ANGEL ON 20151216
        Public Sub SaveAdd(ByVal dtCustomer As CustomerDTO.CustomerDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                Dim dr As CustomerDTO.CustomerRow = dtCustomer.Rows(0)
                If chkIsExist(dr.CU_NO, "") = True Then
                    Throw New ArgumentException(_oLanguage.getText("Customer", "029", ctlLanguage.eumType.Validator))
                End If

                oConn.BeginTransaction()

                oExecute.addParameter("CU_NO", dr.CU_NO, OracleType.VarChar)
                oExecute.addParameter("CU_NAME", dr.CU_NAME, OracleType.NVarChar)
                oExecute.addParameter("CU_COUNTRYID", dr.CU_COUNTRYID, OracleType.VarChar)
                oExecute.addParameter("CU_COMPNO", dr.CU_COMPNO, OracleType.VarChar)
                oExecute.addParameter("CU_TEL", dr.CU_TEL, OracleType.VarChar)
                oExecute.addParameter("CU_ADDRESS1", dr.CU_ADDRESS1, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS2", dr.CU_ADDRESS2, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS3", dr.CU_ADDRESS3, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS4", dr.CU_ADDRESS4, OracleType.NVarChar)
                oExecute.addParameter("CU_EMAIL", dr.CU_EMAIL, OracleType.VarChar)
                oExecute.addParameter("CU_FinanceEMAIL", dr.CU_FINANCEEMAIL, OracleType.NVarChar)
                oExecute.addParameter("CU_SALESID", dr.CU_SALESID, OracleType.VarChar)
                oExecute.addParameter("CU_ASSISTANTID", dr.CU_ASSISTANTID, OracleType.VarChar)
                oExecute.addParameter("CU_STATUS", dr.CU_STATUS, OracleType.Int16)
                oExecute.addParameter("CU_AD", dr.CU_AD, OracleType.NVarChar)
                oExecute.addParameter("CU_ADNAME", dr.CU_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("CU_CSTMP", dr.CU_CSTMP, OracleType.DateTime)
                oExecute.addParameter("CU_LUAD", dr.CU_LUAD, OracleType.NVarChar)
                oExecute.addParameter("CU_LUADNAME", dr.CU_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("CU_LUSTMP", dr.CU_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("CU_CONTACTPERSON", dr.CU_CONTACTPERSON, OracleType.NVarChar)
                oExecute.addParameter("CU_ISCHOICE", dr.CU_ISCHOICE, OracleType.NVarChar)
                oExecute.addParameter("CU_TIPTOP_ID", dr.CU_TIPTOP_ID, OracleType.VarChar)

                oExecute.addParameter("CU_SERVICE_CHG", dr.CU_SERVICE_CHG, OracleType.Number)
                oExecute.addParameter("CU_DISCOUNT_OFF", dr.CU_DISCOUNT_OFF, OracleType.Number)
                '新增服務費折讓開關功能 by buck Add 20260427 begin
                oExecute.addParameter("CU_SERVICE_CHG_DISCOUNT", dr.CU_SERVICE_CHG_DISCOUNT, OracleType.Number)
                '新增服務費折讓開關功能 by buck Add 20260427 end

                oExecute.addParameter("CU_WH", "F1", OracleType.VarChar)

                oExecute.Command("CUSTOMER", Execute.eumCommandType.AddNew)
                oConn.Commit()

            Catch ex As Exception
                oConn.Commit()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Customer-修改
        ''' </summary>
        ''' <param name="dtCustomer">傳入dtCustomer</param>
        ''' <remarks></remarks>
        ''' Add CU_WH TO F1 MODI BY ANGEL ON 20151202
        Public Sub SaveEdit(ByVal dtCustomer As CustomerDTO.CustomerDataTable)
            Dim i As Integer = 0
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            Dim dr As CustomerDTO.CustomerRow = dtCustomer.Rows(0)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("CU_NAME", dr.CU_NAME, OracleType.NVarChar)
                oExecute.addParameter("CU_COUNTRYID", dr.CU_COUNTRYID, OracleType.VarChar)
                oExecute.addParameter("CU_COMPNO", dr.CU_COMPNO, OracleType.VarChar)
                oExecute.addParameter("CU_TEL", dr.CU_TEL, OracleType.VarChar)
                oExecute.addParameter("CU_ADDRESS1", dr.CU_ADDRESS1, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS2", dr.CU_ADDRESS2, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS3", dr.CU_ADDRESS3, OracleType.NVarChar)
                oExecute.addParameter("CU_ADDRESS4", dr.CU_ADDRESS4, OracleType.NVarChar)
                oExecute.addParameter("CU_EMAIL", dr.CU_EMAIL, OracleType.VarChar)
                oExecute.addParameter("CU_FinanceEMAIL", dr.CU_FINANCEEMAIL, OracleType.NVarChar)
                oExecute.addParameter("CU_SALESID", dr.CU_SALESID, OracleType.VarChar)
                oExecute.addParameter("CU_ASSISTANTID", dr.CU_ASSISTANTID, OracleType.VarChar)
                oExecute.addParameter("CU_STATUS", dr.CU_STATUS, OracleType.Int16)
                oExecute.addParameter("CU_AD", dr.CU_AD, OracleType.NVarChar)
                oExecute.addParameter("CU_ADNAME", dr.CU_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("CU_CSTMP", dr.CU_CSTMP, OracleType.DateTime)
                oExecute.addParameter("CU_LUAD", dr.CU_LUAD, OracleType.NVarChar)
                oExecute.addParameter("CU_LUADNAME", dr.CU_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("CU_LUSTMP", dr.CU_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("CU_CONTACTPERSON", dr.CU_CONTACTPERSON, OracleType.NVarChar)
                oExecute.addParameter("CU_ISCHOICE", dr.CU_ISCHOICE, OracleType.NVarChar)
                oExecute.addParameter("CU_TIPTOP_ID", dr.CU_TIPTOP_ID, OracleType.VarChar)
                oExecute.addParameter("CU_WH", "F1", OracleType.VarChar)

                oExecute.addParameter("CU_SERVICE_CHG", dr.CU_SERVICE_CHG, OracleType.Number)
                oExecute.addParameter("CU_DISCOUNT_OFF", dr.CU_DISCOUNT_OFF, OracleType.Number)
                '新增服務費折讓開關功能 by buck Add 20260427 begin
                oExecute.addParameter("CU_SERVICE_CHG_DISCOUNT", dr.CU_SERVICE_CHG_DISCOUNT, OracleType.Number)
                '新增服務費折讓開關功能 by buck Add 20260427 end

                oExecute.addWHERE("CU_NO", dr.CU_NO.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("CUSTOMER", Execute.eumCommandType.UPDATE)

                blnFlag = True

            Catch ex As Exception
                blnFlag = False
                Throw New Exception(ex.ToString())
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
#End Region


#Region "Class:CustomerUser"
    Public Class CustomerUser
        Dim _oLanguage As New ctlLanguage
        Dim _dtCustomerUser As New CustomerDTO.CustomerUserDataTable

        ''' <summary>
        ''' 登入
        ''' </summary>
        ''' <param name="CUUS_CUID">傳入公司代碼</param>
        ''' <param name="CUUS_ACCOUNTID">傳入帳號</param>
        ''' <param name="CUUS_PWD">傳入密碼</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:失敗, True:成功</remarks>
        Public Function Login(ByVal CUUS_CUID As String, ByVal CUUS_ACCOUNTID As String, ByVal CUUS_PWD As String) As Boolean
            Dim blnFlag As Boolean = False
            Dim dt As New DataTable
            Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                'oQuery.addWHERE("CUUS_CUID", ":CUUS_CUID", CUUS_CUID, OracleType.VarChar)
                'sCondition = sCondition & " AND CUUS_CUID=:CUUS_CUID"

                oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID, OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_ACCOUNTID=:CUUS_ACCOUNTID"

                oQuery.addWHERE("CUUS_PWD", ":CUUS_PWD", CUUS_PWD, OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_PWD=:CUUS_PWD"

                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CUUS_STATUS", ":CUUS_STATUS", "1", OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_STATUS=:CUUS_STATUS"

                Dim sSQL As String = "SELECT * FROM CUSTOMERUSER WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser)
                If dtCustomerUser.Rows.Count > 0 Then
                    _dtCustomerUser = dtCustomerUser.Copy
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
        ''' 取得登入成功的CustomerUser資料
        ''' </summary>
        ''' <returns>傳回 CustomerUserDataTable</returns>
        ''' <remarks></remarks>
        Public Function getLoginData() As CustomerDTO.CustomerUserDataTable
            Return _dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得CustomerUser資料
        ''' </summary>
        ''' <param name="CUUS_CUID">傳入公司代碼</param>
        ''' <param name="CUUS_ACCOUNTID">傳入帳號</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:失敗, True:成功</remarks>
        Public Function QueryUser(ByVal CUUS_CUID As String, ByVal CUUS_ACCOUNTID As String) As CustomerDTO.CustomerUserDataTable
            Dim blnFlag As Boolean = False
            Dim dt As New DataTable
            Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("CUUS_ACCOUNTID") = "CUUS_oldACCOUNTID"

                Dim HasExceColumn_Decrypt As New Hashtable
                HasExceColumn_Decrypt("CUUS_PWD") = "CUUS_PWD"

                oQuery.addWHERE("CUUS_CUID", ":CUUS_CUID", CUUS_CUID, OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_CUID=:CUUS_CUID"

                If CUUS_ACCOUNTID.Trim <> "" Then
                    oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID, OracleType.VarChar)
                    sCondition = sCondition & " AND CUUS_ACCOUNTID=:CUUS_ACCOUNTID"
                End If

                Dim sSQL As String = "SELECT * FROM CUSTOMERUSER WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser, HasExceColumn, HasExceColumn_Decrypt)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得CustomerUser資料
        ''' </summary>
        ''' <param name="CUUS_ACCOUNTID">傳入帳號</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryUser(ByVal CUUS_ACCOUNTID As String) As CustomerDTO.VWCUSTOMERUSERDataTable

            Dim blnFlag As Boolean = False
            Dim dt As New DataTable
            Dim dtCustomerUser As New CustomerDTO.VWCUSTOMERUSERDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable

                Dim HasExceColumn_Decrypt As New Hashtable
                HasExceColumn_Decrypt("CUUS_PWD") = "CUUS_PWD"

                oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_ACCOUNTID=:CUUS_ACCOUNTID"

                Dim sSQL As String = "SELECT * FROM vwCustomerUser WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser, HasExceColumn, HasExceColumn_Decrypt)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得CustomerUser資料
        ''' </summary>
        ''' <param name="CUUS_CUID">傳入公司代碼</param>
        ''' <param name="CUUS_ACCOUNTID">傳入帳號</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryUser(ByVal CUUS_CUID As String, ByVal CUUS_ACCOUNTID As String, ByVal OrderBY As String) As CustomerDTO.VWCUSTOMERUSERDataTable

            Dim blnFlag As Boolean = False
            Dim dt As New DataTable
            Dim dtCustomerUser As New CustomerDTO.VWCUSTOMERUSERDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable

                Dim HasExceColumn_Decrypt As New Hashtable
                HasExceColumn_Decrypt("CUUS_PWD") = "CUUS_PWD"

                oQuery.addWHERE("CUUS_CUID", ":CUUS_CUID", CUUS_CUID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_CUID=:CUUS_CUID"

                oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_ACCOUNTID=:CUUS_ACCOUNTID"

                Dim sSQL As String = "SELECT * FROM vwCustomerUser WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser, HasExceColumn, HasExceColumn_Decrypt)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得CustomerUser 忘記密碼的 資料
        ''' </summary>
        ''' <param name="CUUS_ACCOUNTID">傳入帳號</param>
        ''' <param name="CUUS_EMAIL">傳入Mail</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByForgetPassword(ByVal CUUS_ACCOUNTID As String, ByVal CUUS_EMAIL As String) As CustomerDTO.VWCUSTOMERUSERDataTable

            Dim blnFlag As Boolean = False
            Dim dt As New DataTable
            Dim dtCustomerUser As New CustomerDTO.VWCUSTOMERUSERDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable

                Dim HasExceColumn_Decrypt As New Hashtable
                HasExceColumn_Decrypt("CUUS_PWD") = "CUUS_PWD"

                'oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID.ToLower().Trim(), OracleType.VarChar)
                'sCondition = sCondition & " AND lower(CUUS_ACCOUNTID)=:CUUS_ACCOUNTID"

                oQuery.addWHERE("CUUS_ACCOUNTID", ":CUUS_ACCOUNTID", CUUS_ACCOUNTID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CUUS_ACCOUNTID=:CUUS_ACCOUNTID"

                CUUS_EMAIL = "%" & CUUS_EMAIL.ToLower().Trim() & "%"
                oQuery.addWHERE("CUUS_EMAIL", ":CUUS_EMAIL", CUUS_EMAIL, OracleType.VarChar)
                sCondition = sCondition & " AND lower(CUUS_EMAIL) like :CUUS_EMAIL"

                Dim sSQL As String = "SELECT * FROM vwCustomerUser WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser, HasExceColumn, HasExceColumn_Decrypt)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得CustomerUser資料
        ''' </summary>
        ''' <param name="CU_ID">客戶編號</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 CustomerUserDataTable</returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal CU_ID As String, Optional ByVal OrderBY As String = "") As CustomerDTO.CustomerUserDataTable
            Dim dtCustomerUser As New CustomerDTO.CustomerUserDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("CUUS_ACCOUNTID") = "CUUS_oldACCOUNTID"

                Dim HasExceColumn_Decrypt As New Hashtable
                HasExceColumn_Decrypt("CUUS_PWD") = "CUUS_PWD"

                If OrderBY.Trim = "" Then
                    OrderBY = " CUUS_ACCOUNTID asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If CU_ID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("CUUS_CUID", ":CUUS_CUID", CU_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND CUUS_CUID=:CUUS_CUID"
                End If

                sSQL = "SELECT * FROM CUSTOMERUSER WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomerUser, HasExceColumn, HasExceColumn_Decrypt)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerUser
        End Function

        ''' <summary>
        ''' 取得客戶代碼是否存在
        ''' </summary>
        ''' <param name="CUUS_CUID">客戶代碼</param>
        ''' <param name="new_ACCOUNTID">新的帳號</param>
        ''' <param name="old_ACCOUNTID">舊的帳號</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:不存在, True:存在</remarks>
        Public Function chkIsExist(ByVal CUUS_CUID As String, ByVal new_ACCOUNTID As String, Optional ByVal old_ACCOUNTID As String = "") As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("CUUS_CUID", ":CUUS_CUID", CUUS_CUID, OracleType.VarChar)
                oQuery.addWHERE("CUUS_ACCOUNTID", ":new_ACCOUNTID", new_ACCOUNTID, OracleType.VarChar)

                If old_ACCOUNTID.Trim() <> "" Then
                    oQuery.addWHERE("CUUS_ACCOUNTID", ":old_ACCOUNTID", old_ACCOUNTID, OracleType.VarChar)
                    sCondition = sCondition & " AND CUUS_ACCOUNTID<>:old_ACCOUNTID"
                End If

                Dim sSQL As String = "SELECT * FROM CustomerUser " &
                  " WHERE CUUS_CUID=:CUUS_CUID AND CUUS_ACCOUNTID=:new_ACCOUNTID " & sCondition

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
        ''' 存檔
        ''' </summary>
        ''' <param name="dtCustomerUser">傳入CustomerUserDataTable</param>
        ''' <remarks></remarks>
        Public Sub Save(ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable)

            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New Query(oConn)

            Dim dtCustomerUser_Add As New CustomerDTO.CustomerUserDataTable
            Dim dtCustomerUser_Edit As New CustomerDTO.CustomerUserDataTable

            oConn.Open()

            Try

                '====================================================================================================================================================================================
                '處理 Customer User
                '====================================================================================================================================================================================
                dtCustomerUser_Add = dtCustomerUser.Copy
                dtCustomerUser_Edit = dtCustomerUser.Copy

                Dim dvCustomerUser_Add As DataView = dtCustomerUser_Add.DefaultView()
                dvCustomerUser_Add.RowFilter = "CUUS_oldACCOUNTID<>''"
                Do While dvCustomerUser_Add.Count > 0
                    dvCustomerUser_Add.Delete(0)
                Loop

                Dim dvCustomerUser_Edit As DataView = dtCustomerUser_Edit.DefaultView()
                dvCustomerUser_Edit.RowFilter = "CUUS_oldACCOUNTID=''"
                Do While dvCustomerUser_Edit.Count > 0
                    dvCustomerUser_Edit.Delete(0)
                Loop

                '新增時檢核資料是否存在
                For i = 0 To dtCustomerUser_Add.Rows.Count - 1
                    Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser_Add.Rows(i)
                    If chkIsExist(dr.CUUS_CUID, dr.CUUS_ACCOUNTID, "") = True Then
                        Throw New ArgumentException(_oLanguage.getText("Customer", "040", ctlLanguage.eumType.Validator))
                    End If
                Next

                '新增時檢核資料是否存在
                For i = 0 To dtCustomerUser_Edit.Rows.Count - 1
                    Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser_Edit.Rows(i)
                    If chkIsExist(dr.CUUS_CUID, dr.CUUS_ACCOUNTID, dr.CUUS_oldACCOUNTID) = True Then
                        Throw New ArgumentException(_oLanguage.getText("Customer", "040", ctlLanguage.eumType.Validator))
                    End If
                Next

                '====================================================================================================================================================================================
                '處理 CustomerUser
                '====================================================================================================================================================================================

                oConn.BeginTransaction()

                If dtCustomerUser_Add.Rows.Count > 0 Then
                    Call SaveAdd(oExecute, dtCustomerUser_Add)
                End If

                If dtCustomerUser_Edit.Rows.Count > 0 Then
                    Call SaveEdit(oExecute, dtCustomerUser_Edit)
                End If

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
        ''' CustomerUser-新增
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtCustomerUser">傳入dtCustomerUser</param>
        ''' <remarks></remarks>
        Private Sub SaveAdd(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable)
            Dim i As Integer = 0
            Dim oCrypto As New SecurityCrypt.Crypto


            Try

                For i = 0 To dtCustomerUser.Rows.Count - 1
                    Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

                    Dim CUUS_PWD As String = oCrypto.Encrypt(dr.CUUS_PWD.Trim(), "")

                    oExecute.addParameter("CUUS_ACCOUNTID", dr.CUUS_ACCOUNTID, OracleType.VarChar)
                    oExecute.addParameter("CUUS_CUID", dr.CUUS_CUID, OracleType.VarChar)
                    oExecute.addParameter("CUUS_PWD", CUUS_PWD, OracleType.VarChar)

                    oExecute.addParameter("CUUS_TEL", dr.CUUS_TEL, OracleType.VarChar)
                    oExecute.addParameter("CUUS_EMAIL", dr.CUUS_EMAIL, OracleType.VarChar)
                    oExecute.addParameter("CUUS_STATUS", dr.CUUS_STATUS, OracleType.Int16)
                    oExecute.addParameter("CUUS_ISMANAGER", dr.CUUS_ISMANAGER, OracleType.Int16)

                    oExecute.addParameter("CUUS_AD", dr.CUUS_AD, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_ADNAME", dr.CUUS_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_CSTMP", dr.CUUS_CSTMP, OracleType.DateTime)

                    oExecute.addParameter("CUUS_LUAD", dr.CUUS_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_LUADNAME", dr.CUUS_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_LUSTMP", dr.CUUS_LUSTMP, OracleType.DateTime)
                    'oExecute.addParameter("CU_TIPTOP_ID", dr.CU_TIPTOP_ID, OracleType.VarChar)


                    oExecute.Command("CUSTOMERUSER", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' CustomerUser-修改
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtCustomerUser">傳入dtCustomerUser</param>
        ''' <remarks></remarks>
        Private Sub SaveEdit(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtCustomerUser As CustomerDTO.CustomerUserDataTable)
            Dim i As Integer = 0
            Dim oCrypto As New SecurityCrypt.Crypto

            Try

                For i = 0 To dtCustomerUser.Rows.Count - 1
                    Dim dr As CustomerDTO.CustomerUserRow = dtCustomerUser.Rows(i)

                    Dim CUUS_PWD As String = oCrypto.Encrypt(dr.CUUS_PWD.Trim(), "")

                    oExecute.addParameter("CUUS_PWD", CUUS_PWD, OracleType.VarChar)
                    oExecute.addParameter("CUUS_TEL", dr.CUUS_TEL, OracleType.VarChar)
                    oExecute.addParameter("CUUS_EMAIL", dr.CUUS_EMAIL, OracleType.VarChar)
                    oExecute.addParameter("CUUS_STATUS", dr.CUUS_STATUS, OracleType.Int16)
                    oExecute.addParameter("CUUS_ISMANAGER", dr.CUUS_ISMANAGER, OracleType.Int16)

                    'oExecute.addParameter("CUUS_AD", dr.CUUS_AD, OracleType.NVarChar)
                    'oExecute.addParameter("CUUS_ADNAME", dr.CUUS_ADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("CUUS_CSTMP", dr.CUUS_CSTMP, OracleType.DateTime)

                    oExecute.addParameter("CUUS_LUAD", dr.CUUS_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_LUADNAME", dr.CUUS_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("CUUS_LUSTMP", dr.CUUS_LUSTMP, OracleType.DateTime)
                    'oExecute.addParameter("CU_TIPTOP_ID", dr.CU_TIPTOP_ID, OracleType.VarChar)

                    oExecute.addWHERE("CUUS_ACCOUNTID", dr.CUUS_ACCOUNTID.ToString().Trim(), OracleType.VarChar)
                    oExecute.addWHERE("CUUS_CUID", dr.CUUS_CUID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("CUSTOMERUSER", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 客戶端 經銷商 or EndUser
        ''' </summary>

        Public Sub Update_Dealer_EndUser(ByVal check As Boolean, ByVal CU_NO As String)
            Dim i As Integer = 0
            Dim sSQL As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                sSQL = "  UPDATE CUSTOMER SET   CU_ISENDUSER =:CU_ISENDUSER where CU_NO =:CU_NO   "
                Dim oCommand As OracleCommand = oConn.Command()

                If check = True Then
                    oCommand.Parameters.AddWithValue("CU_ISENDUSER", "Y")
                Else
                    oCommand.Parameters.AddWithValue("CU_ISENDUSER", "")
                End If

                oCommand.Parameters.AddWithValue("CU_NO", CU_NO)

                oCommand.CommandText = sSQL
                oCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End Sub

    End Class
#End Region

End Class

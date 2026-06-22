Imports System.Data.OracleClient
Imports ICAT_OracleDAO

Public Class ctlExtend

    Public Function DelCompanyCountry(company_no As String, country_id As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        sSQL = "DELETE FROM COMPANYCOUNTRY WHERE COMP_NO = :COMP_NO AND COUNTRY_ID = :COUNTRY_ID "

        Try
            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("COMP_NO", company_no))
                oCommand.Parameters.Add(New OracleParameter("COUNTRY_ID", country_id))

                oCommand.ExecuteNonQueryAsync()
            End Using


        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function InsertCompanyCountry(company_no As String, country_id As String, user As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        sSQL = "INSERT INTO COMPANYCOUNTRY(COMP_NO,COUNTRY_ID,UPD_USER,UPD_DATE) VALUES(:COMP_NO,:COUNTRY_ID,:UPD_USER,sysdate) "

        Try

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("COMP_NO", company_no))
                oCommand.Parameters.Add(New OracleParameter("COUNTRY_ID", country_id))
                oCommand.Parameters.Add(New OracleParameter("UPD_USER", user))

                oCommand.ExecuteNonQueryAsync()
            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function QryCounrtyData(Repair_no As String)

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL += "SELECT COUNTRY_ID,COUNTRY_NAME FROM COUNTRY "

            If Repair_no <> "" Then
                sSQL += "WHERE COUNTRY_ID NOT IN (SELECT COUNTRY_ID FROM COMPANYCOUNTRY WHERE COMP_NO = :COMP_NO) "
            End If

            sSQL += "ORDER BY COUNTRY_ID"

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                If Repair_no <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("COMP_NO", Repair_no))
                End If

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryCompanyCountryData(Repair_no As String) As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT a.COMP_NO,a.COUNTRY_ID,b.COUNTRY_NAME FROM COMPANYCOUNTRY a
                    JOIN COUNTRY b ON b.COUNTRY_ID = a.COUNTRY_ID 
                    WHERE 1= 1 "

            If Repair_no <> "" Then
                sSQL += "AND a.COMP_NO = :COMP_NO "
            End If

            sSQL += "ORDER BY a.COMP_NO,a.COUNTRY_ID"

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                If Repair_no <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("COMP_NO", Repair_no))
                End If

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryCompanyData()

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = ""

            sSQL += "SELECT COMP_NO,COMP_NAME FROM COMPANY
                     WHERE COMP_VISIBLE='1'
                     ORDER BY COMP_NO "

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryRMAData(sDate As String, eDate As String, rma_no As String) As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT to_char(a.RMA_CSTMP,'YYYY/MM/DD') RMA_CSTMP,a.RMA_NO,a.RMA_CUNO,c.CU_NAME,a.RMA_ADDRESS,a.RMA_COMPNO,a.RMA_STATUS,a.RMA_MARK,a.RMA_INVNO,a.RMA_ARNO,
                    b.RMAD_MODELNO,b.RMAD_SERIALNO,b.RMAD_WARRANTY,b.RMAD_FARNO,d.FAR_REASON,b.RMAD_FARFARCNO,e.FARC_NAME,
                    b.RMAD_PROBLEMDESC,b.RMAD_PRODUCTDESC,b.RMAD_STATUS,b.RMAD_RECEVSTATUS,b.RMAD_ISWARRANTY,b.RMAD_ISCW,b.RMAD_PARTSN,
                    f.RMARQ_IMPROPERUSAGE,f.RMARQ_LABORHOUR,f.RMARQ_LABORPRICE,f.RMARQ_MATERIALCOST,f.RMARQ_QUOTE,
                    f.RMARQ_ASSIGLABORCOST,f.RMARQ_ASSIGMATERIALCOST,f.RMARQ_ASSIGEQUOTE,g.RMARQD_OPARTNO,g.RMARQD_NPARTNO,g.RMARQD_OSERIALNO,g.RMARQD_NSERIALNO,
                    h.RMASQ_LABORCOST,h.RMASQ_MATERIALCOST,h.RMASQ_QUOTE,h.RMASQ_CURRENCYCODE,h.RMASQ_CURRENCYRATE,
                    j.RMASM_SHIPMEMO,j.RMASM_SHIPNO,l.RMASH_PACKINGLIST,l.RMASH_EXPRESSCO,l.RMASH_TRACKINGNO,l.RMASH_MEMO
                    FROM RMA a
                    JOIN RMADETAIL b ON b.RMAD_RMANO = a.RMA_NO
                    LEFT JOIN CUSTOMER c ON c.cu_no = a.RMA_CUNO
                    LEFT JOIN FAILUREREASONS d ON d.FAR_NO = b.RMAD_FARNO AND d.FAR_VISIBLE='1' AND d.FAR_DFLNO='001'
                    LEFT JOIN FAILUREREASONSCLASS e ON e.FARC_VISIBLE='1' AND e.FARC_DFLNO='001' AND e.FARC_NO = b.RMAD_FARFARCNO AND e.FARC_NO = d.FAR_FARCNO AND e.FARC_DFLNO = d.FAR_DFLNO
                    LEFT JOIN RMAREPAIR_QUOTED f ON f.RMARQ_RMADID = b.RMAD_ID
                    LEFT JOIN RMAREPAIR_QUOTED_DETAIL g ON g.RMARQD_MARK='1' AND g.RMARQD_RMADID = b.RMAD_ID
                    LEFT JOIN RMASALE_QUOTED h ON h.RMASQ_RMADID = b.RMAD_ID
                    LEFT JOIN RMA_SHIPMENTDETAIL i ON i.RMASMD_RMANO = a.RMA_NO
                    LEFT JOIN RMA_SHIPMENT j ON j.RMASM_ID = i.RMASMD_RMASMID
                    LEFT JOIN RMA_SHIPPINGDETAIL k ON k.RMASHD_RMANO = a.RMA_NO
                    LEFT JOIN RMA_SHIPPING l ON l.RMASH_ID = k.RMASHD_RMASHID
                    WHERE 1=1 "

            If rma_no <> "" Then
                sSQL += "AND a.RMA_NO = :RMA_NO "
            End If
            If sDate <> "" Then
                sSQL += "AND to_char(a.RMA_CSTMP,'YYYY/MM/DD') BETWEEN :sDate AND :eDate  "
            End If

            sSQL += "ORDER BY a.RMA_NO "

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                If sDate <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("sDate", sDate))
                    oCommand.Parameters.Add(New OracleParameter("eDate", eDate))
                End If

                If rma_no <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("RMA_NO", rma_no))
                End If


                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryRMA_RMADETAIL(ByVal SERIALNO As String) As DataTable
        Dim sErr As String = ""
        Dim dt_ As New DataTable
        If String.IsNullOrEmpty(SERIALNO) Then Return dt_

        Dim oConn As New Connection
        oConn.Open()

        Try
            Dim sSQL As String = "
                                    WITH TargetExports AS (
                                        SELECT A.EXPORT_SERIALNO, A.CW_SDATE, A.CW_EDATE 
                                        FROM EXPORT A 
                                        INNER JOIN ( 
                                            SELECT DISTINCT EXPORT_SALESNUMBER FROM EXPORT WHERE EXPORT_SERIALNO = :SERIALNO 
                                        ) B ON A.EXPORT_SALESNUMBER = B.EXPORT_SALESNUMBER 
                                        WHERE EXISTS (SELECT 1 FROM WARRSPECS W WHERE W.WAP_WID = A.EXPORT_WAR_ID) 
                                    ), 
                                    ExportInfo AS (
                                        SELECT COUNT(*) OVER() as total_count, EXPORT_SERIALNO, CW_SDATE, CW_EDATE FROM TargetExports
                                    ) 
                                    SELECT R.RMAD_RMANO, R.RMAD_SERIALNO, R.RMAD_STATUS, E.total_count AS total 
                                    FROM RMADETAIL R 
                                    INNER JOIN ExportInfo E ON R.RMAD_SERIALNO = E.EXPORT_SERIALNO 
                                    WHERE R.RMAD_STATUS = 90 
                                    AND (
                                        E.CW_SDATE IS NULL OR 
                                        R.RMAD_CSTMP BETWEEN TO_DATE(TO_CHAR(E.CW_SDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') 
                                                        AND TO_DATE(TO_CHAR(E.CW_EDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD')
                                    ) 
                                "

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)
                oCommand.Parameters.Add(New OracleParameter("SERIALNO", OracleType.VarChar)).Value = SERIALNO

                Using sda = New OracleDataAdapter(oCommand)
                    sda.Fill(dt_)
                End Using
            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt_
    End Function

    'Public Function QryRMA_RMADETAIL(SERIALNO As String) As DataTable

    '    Dim dt As DataTable = New DataTable
    '    Dim dt_ As DataTable = New DataTable

    '    Dim sSQL As String = ""
    '    Dim sErr As String = ""

    '    Dim sDate As String = ""
    '    Dim eDate As String = ""

    '    Dim RMAD_SERIALNO As String = "("



    '    '存資料
    '    Try
    '        sSQL = ""
    '        Dim oConn As New Connection
    '        oConn.Open()
    '        Try
    '            'sSQL = " select * from EXPORT where EXPORT_WAR_ID in (select WAP_WID from WARRSPECS) and  EXPORT_SALESNUMBER in (  select EXPORT_SALESNUMBER from EXPORT  where EXPORT_SERIALNO =:EXPORT_SERIALNO)  "
    '            sSQL = "
    '                    SELECT A.*
    '                    FROM EXPORT A
    '                    INNER JOIN (
    '                        SELECT DISTINCT EXPORT_SALESNUMBER 
    '                        FROM EXPORT 
    '                        WHERE EXPORT_SERIALNO = :EXPORT_SERIALNO
    '                    ) B ON A.EXPORT_SALESNUMBER = B.EXPORT_SALESNUMBER
    '                    WHERE EXISTS (
    '                        SELECT 1 FROM WARRSPECS W WHERE W.WAP_WID = A.EXPORT_WAR_ID
    '                    )
    '                    "
    '            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

    '                If SERIALNO <> "" Then
    '                    oCommand.Parameters.Add(New OracleParameter("EXPORT_SERIALNO", SERIALNO))
    '                End If

    '                Using sda2 = New OracleDataAdapter(oCommand)
    '                    sda2.Fill(dt)
    '                End Using

    '            End Using

    '        Catch ex As Exception
    '            sErr = ex.Message
    '        Finally
    '            oConn.Close()
    '            oConn.Dispose()
    '        End Try

    '    Catch ex As Exception
    '        sErr = ex.Message
    '    Finally

    '    End Try
    '    '放參數
    '    Try

    '        For Each item As DataRow In dt.Rows

    '            RMAD_SERIALNO += "'" & item("EXPORT_SERIALNO").ToString().Trim() & "',"
    '            sDate = item("CW_SDATE").ToString().Trim()
    '            eDate = item("CW_EDATE").ToString().Trim()

    '        Next

    '    Catch ex As Exception
    '        sErr = ex.Message
    '    Finally
    '        RMAD_SERIALNO += "'0')"
    '    End Try


    '    Try
    '        sSQL = ""
    '        Dim oConn As New Connection
    '        oConn.Open()

    '        Try

    '            sSQL = "select
    '            RMAD_RMANO,
    '            RMAD_SERIALNO,
    '            RMAD_STATUS,
    '            '" & dt.Rows.Count.ToString() & "'     as total
    '            from RMADETAIL
    '            WHERE 1=1 and RMAD_STATUS = 90  "

    '            If sDate <> "" Then
    '                sSQL += "AND to_char(RMAD_CSTMP,'YYYY/MM/DD') BETWEEN :sDate AND :eDate  "
    '            End If

    '            sSQL += " and  RMAD_SERIALNO in " & RMAD_SERIALNO

    '            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

    '                If sDate <> "" Then
    '                    oCommand.Parameters.Add(New OracleParameter("sDate", sDate))
    '                    oCommand.Parameters.Add(New OracleParameter("eDate", eDate))
    '                End If

    '                Using sda2 = New OracleDataAdapter(oCommand)
    '                    sda2.Fill(dt_)
    '                End Using

    '            End Using

    '        Catch ex As Exception
    '            sErr = ex.Message
    '        Finally
    '            oConn.Close()
    '            oConn.Dispose()
    '        End Try



    '    Catch ex As Exception
    '        sErr = ex.Message
    '    Finally

    '    End Try

    '    Return dt_

    'End Function

    Public Function QryCustomerData() As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT cu_no,cu_name,cu_salesid,cu_assistantid FROM customer ORDER BY cu_no "

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    ''' <summary>
    ''' 退回程式 此程式只能退沒有折讓時 才可以退回
    ''' 60->10 60->20 60->30 
    ''' 50(40)->30 50(40)->20 50(40)->10
    ''' 30 -> 20 30-> 10
    ''' 20 -> 10
    ''' </summary>
    ''' <param name="sRMA_NO">RMA_NO</param>
    ''' <returns>回傳 Err</returns>
    ''' <remarks></remarks>
    Public Function UpdateRMAStatus(sRMA_NO As String, Serial_no As String, Start_Status As String, End_Status As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sSQL10 As String = ""
        Dim sSQL20 As String = ""
        Dim sSQL30 As String = ""
        Dim sSQLRepairData As String = ""
        Dim sSQLSaleQuoted As String = ""
        Dim sSQLRepairQuoted As String = ""
        Dim sErr As String = ""

        If Start_Status = End_Status Then
            Return sErr
        End If



        sSQL10 = "UPDATE RMA SET RMA_STATUS=10 WHERE RMA_NO = :RMA_NO1; "
        sSQL10 += "UPDATE RMADETAIL SET RMAD_STATUS=10,RMAD_RECEVSTATUS=0 "
        sSQL10 += "WHERE RMAD_RMANO = :RMA_NO2 AND RMAD_SERIALNO = :SERIALNO2; "


        sSQL20 = "UPDATE RMADETAIL Set RMAD_STATUS=20 WHERE RMAD_RMANO =:RMA_NO3 AND RMAD_SERIALNO = :SERIALNO3; "

        sSQL30 = "UPDATE RMADETAIL SET RMAD_STATUS=30 WHERE RMAD_RMANO =:RMA_NO4 AND RMAD_SERIALNO = :SERIALNO4; "

        '刪除維修資料
        sSQLRepairData = "DELETE FROM RMAREPAIR WHERE RMAR_RMADID IN ( "
        sSQLRepairData += " SELECT RMAD_ID FROM RMADetail WHERE RMAD_RMANO =:RMA_NO5 "
        sSQLRepairData += " AND RMAD_SERIALNO = :SERIALNO5); "
        sSQLRepairData += " DELETE FROM RMAREPAIR_DETAIL WHERE RMARED_RMADID IN ( "
        sSQLRepairData += " SELECT RMAD_ID FROM RMADetail WHERE RMAD_RMANO =:RMA_NO6 "
        sSQLRepairData += " AND RMAD_SERIALNO = :SERIALNO6); "

        '刪除業務報價資料
        sSQLSaleQuoted += "DELETE FROM RMASALE_QUOTED WHERE RMASQ_RMADID IN ( "
        sSQLSaleQuoted += "SELECT RMAD_ID FROM RMADetail WHERE RMAD_RMANO =:RMA_NO7 "
        sSQLSaleQuoted += "AND RMAD_SERIALNO = :SERIALNO7); "

        '刪除維修人員報價資料
        sSQLRepairQuoted += "DELETE FROM RMAREPAIR_QUOTED WHERE RMARQ_RMADID IN ( "
        sSQLRepairQuoted += "SELECT RMAD_ID FROM RMADetail WHERE RMAD_RMANO =:RMA_NO8 "
        sSQLRepairQuoted += "AND RMAD_SERIALNO = :SERIALNO8); "
        sSQLRepairQuoted += "DELETE FROM RMAREPAIR_QUOTED_DETAIL WHERE RMARQD_RMADID IN ( "
        sSQLRepairQuoted += "SELECT RMAD_ID FROM RMADetail WHERE RMAD_RMANO =:RMA_NO9 "
        sSQLRepairQuoted += "AND RMAD_SERIALNO = :SERIALNO9); "

        oConn.Open()

        sSQL = "BEGIN "

        Select Case End_Status
            Case "10" '退到10 尚未接收
                sSQL += sSQLRepairQuoted + sSQLSaleQuoted + sSQLRepairData + sSQL10

            Case "20" '退到20 已接收
                sSQL += sSQLRepairQuoted + sSQLSaleQuoted + sSQLRepairData + sSQL20

            Case "30" '退到30 報價階段
                sSQL += sSQLRepairData + sSQLSaleQuoted + sSQL30

            Case Else
                sSQL = "SELECT sysdate FROM dual;"
        End Select

        sSQL += "END; "


        Try
            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                Select Case End_Status
                    Case "10" '退到10 尚未接收
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO8", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO8", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO9", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO9", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO7", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO7", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO5", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO5", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO6", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO6", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO1", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO2", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO2", Serial_no))

                    Case "20" '退到20 已接收
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO8", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO8", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO9", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO9", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO7", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO7", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO5", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO5", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO6", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO6", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO3", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO3", Serial_no))

                    Case "30" '退到30 報價階段
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO5", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO5", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO6", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO6", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO7", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO7", Serial_no))
                        oCommand.Parameters.Add(New OracleParameter("RMA_NO4", sRMA_NO))
                        oCommand.Parameters.Add(New OracleParameter("SERIALNO4", Serial_no))
                    Case Else
                        'sSQL = "SELECT sysdate FROM dual;"
                End Select

                oCommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function QryRMAStatus(sRma_no As String) As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT a.RMA_NO,b.RMAD_SERIALNO,b.RMAD_STATUS 
                    FROM RMA a
                    JOIN RMADETAIL b ON b.RMAD_RMANO = a.RMA_NO 
                    LEFT JOIN RMACHARGE_QUOTED c ON c.RMACQ_RMANO = a.RMA_NO
                    WHERE b.RMAD_STATUS <= '60' AND b.RMAD_STATUS <> '0' AND a.RMA_STATUS <='20' 
                    AND c.RMACQ_RMANO IS NULL                    
                    AND a.RMA_NO = :RMA_NO 
                    ORDER BY a.RMA_NO,b.RMAD_SERIALNO "

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("RMA_NO", sRma_no))

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryDuplicateSNData(cust_no As String, serial_no As String) As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT a.RMA_NO,TO_CHAR(a.RMA_CSTMP, 'YYYY/MM/DD') Request_Date,CU_NAME,b.RMAD_SERIALNO,b.RMAD_MODELNO,TO_CHAR(b.RMAD_WARRANTY, 'YYYY/MM/DD') RMAD_WARRANTY_DATE,
                       CASE RMAD_ISWARRANTY WHEN 1 THEN 'Yes' ELSE 'No' END WARRANTY,CASE RMARQ_IMPROPERUSAGE WHEN 1 THEN 'Yes' ELSE 'No' END Improper_Usage,
                       CASE RMAD_STATUS
                         WHEN 10 THEN 'Requested'
                         WHEN 20 THEN 'Received'
                         WHEN 30 THEN 'Repair Quoted'
                         WHEN 35 THEN 'Sale Quoting'
                         WHEN 40 THEN 'Sales Confirmed'
                         WHEN 50 THEN 'Client Confirmed'
                         WHEN 60 THEN 'Repaired'
                         WHEN 70 THEN 'Shipped'
                         WHEN 90 THEN 'Closed'
                         WHEN 91 THEN 'Canceled'
                      END STATUS,
                      RMASQ_LABORCOST,RMASQ_MATERIALCOST,RMASQ_QUOTE,RMACQ_DISCOUNT RMACQ_DISCOUNT_AMT,RMARQ_ASSIGLABORCOST,RMARQ_ASSIGMATERIALCOST,RMARQ_ASSIGEQUOTE
                      FROM rma a
                      JOIN rmadetail b ON b.rmad_rmano = a.rma_no 
                      JOIN (SELECT RMAD_SERIALNO FROM rmadetail
                            GROUP BY RMAD_SERIALNO
                            HAVING COUNT(*) > 1) a1 ON a1.RMAD_SERIALNO = b.RMAD_SERIALNO                          
                      LEFT JOIN customer c ON c.cu_no = a.rma_cuno
                      LEFT JOIN RMAREPAIR_QUOTED d ON d.RMARQ_RMADID = b.RMAD_ID
                      LEFT JOIN (  SELECT RMASQ_RMADID,
                                   SUM (RMASQ_LABORCOST)  RMASQ_LABORCOST,
                                   SUM (RMASQ_MATERIALCOST) RMASQ_MATERIALCOST,
                                   SUM (RMASQ_QUOTE)      RMASQ_QUOTE
                                   FROM RMASALE_QUOTED
                                   GROUP BY RMASQ_RMADID) r ON RMAD_ID = RMASQ_RMADID
                      LEFT JOIN RMACHARGE_QUOTED ON RMA_NO = RMACQ_RMANO
                      WHERE 1=1
                      AND b.RMAD_STATUS <> 0 "

            If (cust_no <> "") Then
                sSQL += " AND (UPPER(c.CU_TIPTOP_ID) = :CU_NO OR UPPER(CU_NO) = :CU_NO) "
            End If


            If (serial_no <> "") Then
                sSQL += " AND UPPER(b.RMAD_SERIALNO) = :RMAD_SERIALNO "
            End If

            sSQL += " ORDER BY b.RMAD_SERIALNO,a.RMA_NO DESC "

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)
                'oCommand.BindByName = True

                If cust_no <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("CU_NO", cust_no))
                End If
                If serial_no <> "" Then
                    oCommand.Parameters.Add(New OracleParameter("RMAD_SERIALNO", serial_no))
                End If

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function InsertSALES_RELATE(sales_id As String, head_id As String, asst_id As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        sSQL = "INSERT INTO CIPHERLAB.SALES_RELATE(SALES_ID,HEAD_ID,ASST_ID) VALUES(:SALES_ID,:HEAD_ID,:ASST_ID) "

        Try

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("SALES_ID", sales_id))
                oCommand.Parameters.Add(New OracleParameter("ASST_ID", asst_id))
                oCommand.Parameters.Add(New OracleParameter("HEAD_ID", head_id))

                oCommand.ExecuteNonQueryAsync()
            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function UpdateSALES_RELATE(sales_id As String, head_id As String, asst_id As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        sSQL = "UPDATE CIPHERLAB.SALES_RELATE SET HEAD_ID=:HEAD_ID ,ASST_ID=:ASST_ID  
                WHERE SALES_ID = :SALES_ID"

        Try
            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("SALES_ID", sales_id))
                oCommand.Parameters.Add(New OracleParameter("ASST_ID", asst_id))
                oCommand.Parameters.Add(New OracleParameter("HEAD_ID", head_id))

                oCommand.ExecuteNonQueryAsync()
            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function DelSALES_RELATE(ByVal Sale_id As String) As String
        Dim oConn As New Connection
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        sSQL = "DELETE FROM CIPHERLAB.SALES_RELATE WHERE SALES_ID = :SALES_ID "

        Try
            Using oCommand = New OracleCommand(sSQL, oConn.Connection)

                oCommand.Parameters.Add(New OracleParameter("SALES_ID", Sale_id))

                oCommand.ExecuteNonQueryAsync()
            End Using


        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return sErr

    End Function

    Public Function QrySALES_RELATEData(sale_id As String) As DataTable
        Dim oConn As New Connection
        Dim dt As DataTable = New DataTable
        Dim sSQL As String = ""
        Dim sErr As String = ""

        oConn.Open()

        Try
            sSQL = "SELECT SALES_ID,HEAD_ID,ASST_ID FROM CIPHERLAB.SALES_RELATE "
            If sale_id <> "" Then
                sSQL += " WHERE SALES_ID = :SALES_ID "
            End If

            Using oCommand = New OracleCommand(sSQL, oConn.Connection)
                'oCommand.BindByName = True

                If sale_id <> "" Then
                    'oCommand.Parameters.Add("SALES_ID", OracleType.VarChar).Value = sale_id
                    oCommand.Parameters.Add(New OracleParameter("SALES_ID", sale_id))
                End If

                Using sda2 = New OracleDataAdapter(oCommand)
                    sda2.Fill(dt)
                End Using

            End Using

        Catch ex As Exception
            sErr = ex.Message
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function GetWarrantyTypeData()

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = ""

            sSQL += "SELECT WARRSET_TYPE,WARRSET_TYPE_NAME FROM WARRSET_TYPE WHERE WARRSET_SEQ > 0 "

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryWarrantyData(ByVal sDate As String, ByVal eDate As String, ByVal sCust As String,
                                    ByVal sInvNo As String, ByVal sSaleID As String, ByVal sType As String,
                                    ByVal sModel As String, ByVal sSN As String)

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = ""

            sSQL = "SELECT a.WATY_NO,a.WATY_CUST,d.OCC02 CUST_NM,a.WATY_DATE,a.WATY_CURR,a.WATY_ERPNO,a.WATY_SALESID,e.AD_NAME SALE_NM,a.WATY_CUST_PO,
                     b.WATI_SEQ,b.WATI_ORDER,b.WATI_ORDSEQ,b.WATI_SKUNO,b.WATI_SKUDESC,b.WATI_TYPE,b.WATI_MODEL,b.WATI_VER,b.WATI_VER_ACT,
                     b.WATI_QTY,b.WATI_YEAR,b.WATI_PRICE,c.WATS_SN,c.WATS_WARRNSTART,c.WATS_WARRNEND FROM WARRANTYORD a
                     JOIN WARRANTYITEM b ON b.WATI_WATYNO = a.WATY_NO AND b.WATI_MARK='0'
                     JOIN WARRANTYSERIAL c ON c.WATS_WATYNO = b.WATI_WATYNO AND c.WATS_WATYSEQ = b.WATI_SEQ
                     LEFT JOIN cipherlab.occ_file d ON d.OCC01 = a.WATY_CUST
                     LEFT JOIN ADMIN e ON e.AD_ID = a.WATY_SALESID
                     WHERE a.WATY_MARK='0' AND a.ISCONFIRM ='Y' "

            If sDate <> "" Then
                sSQL += "AND to_char(a.WATY_DATE,'YYYY/MM/DD') BETWEEN :sDate AND :eDate  "
                oQuery.addWHERE("sDate", ":sDate", sDate, OracleType.VarChar)
                oQuery.addWHERE("eDate", ":eDate", eDate, OracleType.VarChar)
            End If

            If sCust <> "" Then
                sSQL += "AND a.WATY_CUST = :sCust "
                oQuery.addWHERE("sCust", ":sCust", sCust, OracleType.VarChar)
            End If

            If sInvNo <> "" Then
                sSQL += "AND a.WATY_ERPNO LIKE :sInvNo "
                oQuery.addWHERE("sInvNo", ":sInvNo", "%" + sInvNo + "%", OracleType.VarChar)
            End If

            If sSaleID <> "" Then
                sSQL += "AND a.WATY_SALESID = :sSaleID "
                oQuery.addWHERE("sSaleID", ":sSaleID", sSaleID, OracleType.VarChar)
            End If

            If sType <> "" Then
                sSQL += "AND b.WATI_TYPE = :sType "
                oQuery.addWHERE("sType", ":sType", sType, OracleType.VarChar)
            End If

            If sModel <> "" Then
                sSQL += "AND b.WATI_MODEL LIKE :sModel "
                oQuery.addWHERE("sModel", ":sModel", "%" + sModel + "%", OracleType.VarChar)
            End If

            If sSN <> "" Then
                sSQL += "AND c.WATS_SN LIKE :sSN "
                oQuery.addWHERE("sSN", ":sSN", "%" + sSN + "%", OracleType.VarChar)
            End If

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    Public Function QryRMACostData(sDate As String, eDate As String)
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = ""

            sSQL = "SELECT a.RMA_NO,a.RMA_CUNO cust_no,a.RMA_COMPNO repair_no,
                       CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)),2) 
                                                         ELSE NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0) END receivable_amt,
                       CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',SUM(c.RMASQ_LABORCOST)),2) 
                                                         ELSE SUM(c.RMASQ_LABORCOST) END laborcost_amt,
                       CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END)),2) 
                                                         ELSE SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END) END material_amt,
                       CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',SUM(c.RMASQ_LABORCOST)+SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END)),2) 
                                                         ELSE SUM(c.RMASQ_LABORCOST)+SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END) END repair_amt,
                       CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)-SUM(c.RMASQ_LABORCOST)-SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END)),2) 
                                                         ELSE NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)-SUM(c.RMASQ_LABORCOST)-SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END) END sale_quoted_amt,
                       MAX(e.RMASH_LUSTMP) shipping_date,to_char(MAX(e.RMASH_LUSTMP),'YYYY') year,to_char(MAX(e.RMASH_LUSTMP),'MM') month,to_char(MAX(e.RMASH_LUSTMP),'DD') day,
                       g.AD_ID SALE_NO,g.AD_NAME SALE_NM ,f.CU_NAME,f.CU_COUNTRYID 
                       FROM RMA a
                       JOIN RMADETAIL b ON b.RMAD_RMANO = a.RMA_NO AND b.RMAD_STATUS LIKE '9%'  
                       JOIN RMASALE_QUOTED c ON c.RMASQ_RMADID = b.RMAD_ID
                       JOIN (SELECT RMASHD_RMANO,MAX(RMASH_LUSTMP) RMASH_LUSTMP FROM RMA_SHIPPING a
                             JOIN RMA_SHIPPINGDETAIL b ON b.RMASHD_RMASHID =a.RMASH_ID
                             WHERE to_char(a.RMASH_LUSTMP,'YYYY/MM/DD') BETWEEN :sDate AND :eDate
                             GROUP BY RMASHD_RMANO) e ON e.RMASHD_RMANO= a.RMA_NO
                       LEFT JOIN RMACHARGE_QUOTED d ON d.RMACQ_RMANO = a.RMA_NO
                       LEFT JOIN CUSTOMER f ON f.CU_NO = a.RMA_CUNO
                       LEFT JOIN ADMIN g ON g.AD_ID = f.CU_SALESID 
                        WHERE a.RMA_STATUS = '90' --AND d.RMACQ_RMANO IS NOT NULL
                        --AND a.RMA_NO='ARMA-2021120037'  
                        GROUP BY a.RMA_NO,a.RMA_CUNO,a.RMA_COMPNO,d.RMACQ_DISCOUNT,g.AD_ID,g.AD_NAME ,f.CU_NAME,f.CU_COUNTRYID 
                        HAVING NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)+SUM(c.RMASQ_LABORCOST)+SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END) <> 0
                        UNION --折讓 當月折 上面處理了  之後再折 怎麼處理  若查詢多月...
                        SELECT a.RMA_NO,a.RMA_CUNO cust_no,a.RMA_COMPNO repair_no,0 receivable_amt,0 laborcost_amt,0 material_amt,0 repair_amt, 
                        CASE WHEN a.RMA_COMPNO='CL_CHINA' THEN ROUND(cipherlab.getCurUSD(MAX(e.RMASH_LUSTMP),'RMB','1',NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)-SUM(c.RMASQ_LABORCOST)-SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END)),2) 
                                                          ELSE NVL(SUM(c.RMASQ_QUOTE),0)-NVL(d.RMACQ_DISCOUNT,0)-SUM(c.RMASQ_LABORCOST)-SUM(CASE WHEN b.RMAD_STATUS='91' THEN 0 ELSE c.RMASQ_MATERIALCOST END) END sale_quoted_amt,
                        MAX(e.RMASH_LUSTMP) shipping_date,to_char(MAX(e.RMASH_LUSTMP),'YYYY') year,to_char(MAX(e.RMASH_LUSTMP),'MM') month,to_char(MAX(e.RMASH_LUSTMP),'DD') day,
                        g.AD_ID SALE_NO,g.AD_NAME SALE_NM ,f.CU_NAME,f.CU_COUNTRYID 
                        FROM RMA a
                        JOIN RMADETAIL b ON b.RMAD_RMANO = a.RMA_NO AND b.RMAD_STATUS LIKE '9%'  
                        JOIN RMASALE_QUOTED c ON c.RMASQ_RMADID = b.RMAD_ID
                        JOIN RMACHARGE_QUOTED d ON d.RMACQ_RMANO = a.RMA_NO AND to_char(RMACQ_CHARGEDATE,'YYYY/MM/DD') BETWEEN :sDate AND :eDate AND NVL(RMACQ_ARNO,' ') <> ' '
                        JOIN (SELECT RMASHD_RMANO,MAX(RMASH_LUSTMP) RMASH_LUSTMP FROM RMA_SHIPPING a
                             JOIN RMA_SHIPPINGDETAIL b ON b.RMASHD_RMASHID =a.RMASH_ID
                             WHERE to_char(a.RMASH_LUSTMP,'YYYY/MM/DD') NOT BETWEEN :sDate AND :eDate
                             GROUP BY RMASHD_RMANO) e ON e.RMASHD_RMANO= a.RMA_NO
                         LEFT JOIN CUSTOMER f ON f.CU_NO = a.RMA_CUNO
                         LEFT JOIN ADMIN g ON g.AD_ID = f.CU_SALESID 
                        GROUP BY a.RMA_NO,a.RMA_CUNO,a.RMA_COMPNO,d.RMACQ_DISCOUNT,g.AD_ID,g.AD_NAME ,f.CU_NAME,f.CU_COUNTRYID 
                        ORDER BY shipping_date,RMA_NO,cust_no  "

            oQuery.addWHERE("sDate", ":sDate", sDate, OracleType.VarChar)
            oQuery.addWHERE("eDate", ":eDate", eDate, OracleType.VarChar)

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

End Class

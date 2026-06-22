Imports DefLanguage
Imports ICAT_OracleDAO

Public Class ctlOverTime

    Public Function getMailContent() As LanguageDTO.LANGUAGEDataTable
        Dim dt As New DataTable
        Dim dtLanguage As New LanguageDTO.LANGUAGEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            Dim sSQL As String = "SELECT * FROM LANGUAGE WHERE FUNCTIONNAME='Mail' and (Key='005' OR Key='031' OR Key='032') and TYPE='Mail'"
            dt = oQuery.ExecuteDT(sSQL)
            TransferDataTable(dt, dtLanguage)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtLanguage

    End Function

    ''' <summary>
    ''' 取得 逾時發送Mail 定義檔
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Query_ConfigLimit() As OverTimeDTO.ConfigLimitDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.ConfigLimitDataTable

        oConn.Open()
        Try

            Dim sSQL As String = "SELECT * from CONFIGLIMIT"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[收貨]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryReceive(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO,  RMA_CSTMP ,  AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(RMA_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & " LastComfirmDate," & _
                " to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(RMA_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=10 AND RMAD_RECEVSTATUS=0" & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ','   like '%,' || TRIM(RMA.RMA_COMPNO) || ',%'" & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '1') > 0" & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null" & _
                "    AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(RMA_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & _
                " order by RMA_NO, LastComfirmDate "

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[維修報價]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryRepairQuote(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO, AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(RMAD_RECVDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & "  LastComfirmDate, " & _
                " to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(RMAD_RECVDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays " & _
                " FROM RMA " & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO " & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO " & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0  " & _
                "    AND RMAD_STATUS=20 AND RMAD_RECEVSTATUS=1 " & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ','   like '%,' || TRIM(RMA.RMA_COMPNO) || ',%' " & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '2') > 0 " & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null " & _
                "    AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(RMAD_RECVDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay & _
                " order by RMA_NO, LastComfirmDate "

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[業務報價]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySalesQuote(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO , AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & " LastComfirmDate," & _
                "  to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=30 AND RMAD_RECEVSTATUS=1" & _
                "  INNER JOIN RMAREPAIR_QUOTED ON rmadetail.RMAD_ID = rmarepair_quoted.RMARQ_RMADID" & _
                "  INNER JOIN admin ON  AD_VISIBLE= 1 AND (CUSTOMER.CU_SALESID = admin.AD_ID OR CUSTOMER.CU_ASSISTANTID = admin.AD_ID)" & _
                " WHERE RMA_MARK=0 AND AD_EMAIL is not null" & _
                "    AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & _
                " order by RMA_NO, LastComfirmDate "

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[維修人員]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryRepair(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO, AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(DECODE(RMASQ_CLIENTCONFIRM, 1,   RMASQ_CLIENTDATE ,2, RMASQ_SALEDATE, null),'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & "  LastComfirmDate," & _
                "  to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(DECODE(RMASQ_CLIENTCONFIRM, 1,   RMASQ_CLIENTDATE ,2, RMASQ_SALEDATE, null),'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=50 AND RMAD_RECEVSTATUS=1" & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ','   like '%,' || TRIM(RMA.RMA_COMPNO) || ',%'" & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '2') > 0" & _
                "  INNER JOIN rmasale_quoted ON rmadetail.RMAD_ID = rmasale_quoted.RMASQ_RMADID" & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null" & _
                "  AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(DECODE(RMASQ_CLIENTCONFIRM, 1,   RMASQ_CLIENTDATE ,2, RMASQ_SALEDATE, null),'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString()

            sSQL = sSQL & " union all "

            sSQL = "SELECT DISTINCT  RMA_NO, AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & "  LastComfirmDate," & _
                "  to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=60 AND RMAD_RECEVSTATUS=1" & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ',' like '%,' || TRIM(RMA.RMA_COMPNO) || ',%'" & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '2') > 0" & _
                "  INNER JOIN rmarepair_quoted ON rmadetail.RMAD_ID = rmarepair_quoted.RMARQ_RMADID" & _
                "  LEFT OUTER JOIN rmarepair ON rmadetail.RMAD_ID = rmarepair.RMAR_RMADID" & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null AND rmarepair.RMAR_REPAIRAD is null" & _
                "    AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(RMARQ_CSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & _
                " order by RMA_NO, LastComfirmDate "

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[Shipment]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryShipment(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO, AD_ID, AD_Name, AD_EMAIL, " & _
                " to_date(to_char(RMAR_REPAIRDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & " LastComfirmDate," & _
                "  to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(RMAR_REPAIRDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=60 AND RMAD_RECEVSTATUS=1" & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ','   like '%,' || TRIM(RMA.RMA_COMPNO) || ',%'" & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '3') > 0" & _
                "  INNER JOIN rmarepair ON rmadetail.RMAD_ID = rmarepair.RMAR_RMADID AND rmarepair.RMAR_REPAIRAD is not null" & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null " & _
                "   AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(RMAR_REPAIRDATE,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & _
                " order by RMA_NO, LastComfirmDate "



            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

    ''' <summary>
    ''' 取得逾時[Shipping]資料
    ''' </summary>
    ''' <param name="LimitDay">可逾時天數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryShipping(ByVal LimitDay As Integer) As OverTimeDTO.dtOverListDataTable
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim dtRetval As New OverTimeDTO.dtOverListDataTable

        oConn.Open()
        Try
            'LimitDay = LimitDay + 1

            sSQL = "SELECT DISTINCT  RMA_NO, AD_ID, AD_Name, AD_EMAIL, " & _
                "  to_date(to_char(vwShipment.RMASM_LUSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & " LastComfirmDate," & _
                "  to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') - (to_date(to_char(vwShipment.RMASM_LUSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & ") OverDays" & _
                " FROM RMA" & _
                "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & _
                "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" & _
                "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 " & _
                "    AND RMAD_STATUS=70 AND RMAD_RECEVSTATUS=1" & _
                "  INNER JOIN admin ON  ',' || TRIM(admin.AD_REPAIRCENTER) || ','   like '%,' || TRIM(RMA.RMA_COMPNO) || ',%'" & _
                "    AND AD_VISIBLE= 1 AND INSTR(AD_ROLE, '4') > 0" & _
                "  INNER JOIN " & _
                "  (" & _
                "    SELECT * FROM RMA_SHIPMENT INNER JOIN RMA_SHIPMENTDETAIL ON RMASM_ID = RMASMD_RMASMID" & _
                "   and RMASM_ISSUBMIT=1" & _
                "   ) vwShipment on rmadetail.RMAD_ID = vwShipment.RMASMD_RMADID" & _
                " WHERE RMA_MARK=0  AND AD_EMAIL is not null " & _
                "    AND to_date(to_char(sysdate,'YYYY/MM/DD'),'YYYY/MM/DD') > to_date(to_char(vwShipment.RMASM_LUSTMP,'YYYY/MM/DD'),'YYYY/MM/DD') + " & LimitDay.ToString() & _
                " order by RMA_NO, LastComfirmDate "


            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRetval)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return dtRetval
    End Function

End Class

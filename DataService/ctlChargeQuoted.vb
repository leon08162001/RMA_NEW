Imports System.Data.OracleClient
Imports ICAT_OracleDAO


Public Class ctlChargeQuoted

    Public Function Del_RMACHARGE_QUOTED(ByVal RMACQ_RMANO As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try


            sSQL = "  DELETE FROM  RMACHARGE_QUOTED where   RMACQ_RMANO =:RMACQ_RMANO   "

            oCommand.Parameters.AddWithValue(":RMACQ_RMANO", RMACQ_RMANO)

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    '先新增一筆暫時的
    Public Function Insert_RMACHARGE_QUOTED(ByVal dtRMACharge_QUOTED As ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable)
        Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTEDRow = dtRMACharge_QUOTED.Rows(0)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " INSERT INTO RMACHARGE_QUOTED ( RMACQ_ID, RMACQ_RMANO, RMACQ_CHARGEDATE, RMACQ_SALEQUOTE, RMACQ_DISCOUNT, RMACQ_CHARGEQUOTE, RMACQ_CURRENCYCODE, RMACQ_CURRENCYRATE, RMACQ_QUOTE_ORIGINAL, RMACQ_CURRENCYCODE_ORIGINAL,RMACQ_CURRENCYRATE_ORIGINAL,RMACQ_AD,RMACQ_ADNAME,RMACQ_CSTMP ,RMACQ_LUAD ,RMACQ_LUADNAME ,RMACQ_LUSTMP) VALUES   (     :RMACQ_ID, :RMACQ_RMANO, :RMACQ_CHARGEDATE, :RMACQ_SALEQUOTE, :RMACQ_DISCOUNT, :RMACQ_CHARGEQUOTE, :RMACQ_CURRENCYCODE, :RMACQ_CURRENCYRATE, :RMACQ_QUOTE_ORIGINAL, :RMACQ_CURRENCYCODE_ORIGINAL,:RMACQ_CURRENCYRATE_ORIGINAL,:RMACQ_AD,:RMACQ_ADNAME,:RMACQ_CSTMP ,:RMACQ_LUAD ,:RMACQ_LUADNAME ,:RMACQ_LUSTMP)  "

            oCommand.Parameters.AddWithValue("RMACQ_ID", dr.RMACQ_ID)
            oCommand.Parameters.AddWithValue("RMACQ_RMANO", dr.RMACQ_RMANO)
            oCommand.Parameters.AddWithValue("RMACQ_CHARGEDATE", dr.RMACQ_CHARGEDATE)

            oCommand.Parameters.AddWithValue("RMACQ_SALEQUOTE", dr.RMACQ_SALEQUOTE)
            oCommand.Parameters.AddWithValue("RMACQ_DISCOUNT", dr.RMACQ_DISCOUNT)
            oCommand.Parameters.AddWithValue("RMACQ_CHARGEQUOTE", dr.RMACQ_CHARGEQUOTE)
            oCommand.Parameters.AddWithValue("RMACQ_CURRENCYCODE", dr.RMACQ_CURRENCYCODE)
            oCommand.Parameters.AddWithValue("RMACQ_CURRENCYRATE", dr.RMACQ_CURRENCYRATE)

            oCommand.Parameters.AddWithValue("RMACQ_QUOTE_ORIGINAL", dr.RMACQ_QUOTE_ORIGINAL)
            oCommand.Parameters.AddWithValue("RMACQ_CURRENCYCODE_ORIGINAL", dr.RMACQ_CURRENCYCODE_ORIGINAL)
            oCommand.Parameters.AddWithValue("RMACQ_CURRENCYRATE_ORIGINAL", dr.RMACQ_CURRENCYRATE_ORIGINAL)

            oCommand.Parameters.AddWithValue("RMACQ_AD", dr.RMACQ_AD)
            oCommand.Parameters.AddWithValue("RMACQ_ADNAME", dr.RMACQ_ADNAME)


            oCommand.Parameters.AddWithValue("RMACQ_CSTMP", dr.RMACQ_CSTMP)
            oCommand.Parameters.AddWithValue("RMACQ_LUAD", dr.RMACQ_LUAD)
            oCommand.Parameters.AddWithValue("RMACQ_LUADNAME", dr.RMACQ_LUADNAME)
            oCommand.Parameters.AddWithValue("RMACQ_LUSTMP", dr.RMACQ_LUSTMP)

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    '暫存折讓單價格
    Public Function Insert_RMACHARGE_QUOTED(ByVal RMACQ_EF_ID As String,
        ByVal RMACQ_SALEQUOTE As String,
        ByVal RMACQ_DISCOUNT As String,
        ByVal RMACHARGE_QUOTEDDATATABLE As String,
        ByVal RMACHARGE_QUOTED_SNDATATABLE As String,
        ByVal RMACHARGE_QUOTED_PARTDATATABLE As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try


            sSQL = " INSERT INTO RMACQ_EF_TABLE (RMACQ_EF_ID,RMACQ_SALEQUOTE,RMACQ_DISCOUNT,RMACHARGE_QUOTEDDATATABLE,RMACHARGE_QUOTED_SNDATATABLE,RMACHARGE_QUOTED_PARTDATATABLE) VALUES   (:RMACQ_EF_ID,:RMACQ_SALEQUOTE,:RMACQ_DISCOUNT,:RMACHARGE_QUOTEDDATATABLE,:RMACHARGE_QUOTED_SNDATATABLE,:RMACHARGE_QUOTED_PARTDATATABLE)  "

            oCommand.Parameters.AddWithValue("RMACQ_EF_ID", RMACQ_EF_ID)
            oCommand.Parameters.AddWithValue("RMACQ_SALEQUOTE", RMACQ_SALEQUOTE)
            oCommand.Parameters.AddWithValue("RMACQ_DISCOUNT", RMACQ_DISCOUNT)
            oCommand.Parameters.AddWithValue("RMACHARGE_QUOTEDDATATABLE", RMACHARGE_QUOTEDDATATABLE)
            oCommand.Parameters.AddWithValue("RMACHARGE_QUOTED_SNDATATABLE", RMACHARGE_QUOTED_SNDATATABLE)
            oCommand.Parameters.AddWithValue("RMACHARGE_QUOTED_PARTDATATABLE", RMACHARGE_QUOTED_PARTDATATABLE)

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    '查暫存折讓單價格
    Public Function Select_RMACHARGE_QUOTED(ByVal RMACQ_EF_ID As String) As DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            If RMACQ_EF_ID.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMACQ_EF_ID", ":RMACQ_EF_ID", RMACQ_EF_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMACQ_EF_ID=:RMACQ_EF_ID"
            End If


            sSQL = "SELECT * FROM RMACQ_EF_TABLE WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    '查暫存折讓單價格_001
    Public Function Select_RMACHARGE_QUOTED_RMACQ_RMANO(ByVal RMACQ_RMANO As String) As DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            If RMACQ_RMANO.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMACQ_RMANO", ":RMACQ_RMANO", RMACQ_RMANO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMACQ_RMANO=:RMACQ_RMANO"
            End If


            sSQL = "SELECT * FROM RMACHARGE_QUOTED WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    '刪除
    Public Function del_RMACQ_EF_TABLE(ByVal RMACQ_EF_ID As String)


        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = "  DELETE FROM  RMACQ_EF_TABLE where   RMACQ_EF_ID =:RMACQ_EF_ID   "

            oCommand.Parameters.AddWithValue("RMACQ_EF_ID", RMACQ_EF_ID)
            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


    End Function

    ''' <summary>
    ''' 檢核是否 審核通過
    ''' </summary>
    ''' <param name="RMA_NO"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 判斷,如果存在於charge quoted並且 approve NOT 2 的時候, 不可以出貨
    ''' </remarks>
    Public Function chkChargeQuoted_Approve(ByVal RMA_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim oQuery1 As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            'approve 是否審核通過: 0.新增, 1.送簽中, 2.已確認
            Dim RMACQ_APPROVAL As String = "2"

            oQuery.addWHERE("RMACQ_RMANO", ":RMACQ_RMANO", RMA_NO.Trim(), OracleType.VarChar)
            oQuery.addWHERE("RMACQ_APPROVAL", ":RMACQ_APPROVAL", RMACQ_APPROVAL.Trim(), OracleType.VarChar)

            sSQL = sSQL & " SELECT RMACQ_RMANO FROM RMACHARGE_QUOTED "
            sSQL = sSQL & " WHERE RMACQ_RMANO=:RMACQ_RMANO AND RMACQ_APPROVAL<>:RMACQ_APPROVAL"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = True
            End If

            '2023/01/18 wisely add 判斷折讓金額沒有回寫Sale_quoted
            If Not retval Then
                oQuery1.addWHERE("RMACQ_RMANO", ":RMACQ_RMANO", RMA_NO.Trim(), OracleType.VarChar)

                sSQL = "  SELECT RMA_NO,NVL(sum(RMASQ_QUOTE),0)-NVL(RMACQ_DISCOUNT,0),RMACQ_CHARGEQUOTE "
                sSQL = sSQL & " FROM RMADETAIL,RMASALE_QUOTED,RMA "
                sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"
                sSQL = sSQL & " WHERE RMA_NO = RMAD_RMANO "
                sSQL = sSQL & " AND RMASQ_RMADID = RMAD_ID "
                sSQL = sSQL & " AND RMA_NO = :RMACQ_RMANO "
                sSQL = sSQL & " GROUP BY RMA_NO,RMACQ_DISCOUNT,RMACQ_CHARGEQUOTE"
                sSQL = sSQL & " HAVING NVL(sum(RMASQ_QUOTE),0)- NVL(RMACQ_DISCOUNT,0) <> RMACQ_CHARGEQUOTE"

                dt = oQuery1.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = True
                End If
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
    ''' 檢核  RMACHARGE_QUOTED 是否存在
    ''' </summary>
    ''' <param name="RMA_NO"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function chkIsExist(ByVal RMA_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("RMACQ_RMANO", ":RMACQ_RMANO", RMA_NO.Trim(), OracleType.VarChar)

            sSQL = sSQL & " SELECT RMACQ_RMANO FROM RMACHARGE_QUOTED "
            sSQL = sSQL & " WHERE RMACQ_RMANO=:RMACQ_RMANO "

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
    ''' 檢核  RMACHARGE_QUOTED_SN 是否存在
    ''' </summary>
    ''' <param name="RMA_NO"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function chkIsExistSN(ByVal RMA_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)

            sSQL = sSQL & " SELECT RMAD_ID FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO "
            sSQL = sSQL & " INNER JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID = RMADETAIL.RMAD_ID"

            sSQL = sSQL & " WHERE RMA_NO=:RMA_NO "

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
    ''' 檢核  RMACHARGE_QUOTED_PART 是否存在
    ''' </summary>
    ''' <param name="RMA_NO"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function chkIsExistPART(ByVal RMA_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)

            sSQL = sSQL & " SELECT RMAD_ID FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO "
            sSQL = sSQL & " INNER JOIN RMACHARGE_QUOTED_PART ON RMACHARGE_QUOTED_PART.RMACQPT_RMADID = RMADETAIL.RMAD_ID"

            sSQL = sSQL & " WHERE RMA_NO=:RMA_NO "

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
    ''' 取得 要業務折讓的RMA資料
    ''' </summary>
    ''' <param name="COMPNO">維修中心代碼</param>
    ''' <param name="SALESID">業務代碼</param>
    ''' <param name="RMANo">RMA No</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>vwClient_WorkListDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryByChargeQuotedList(ByVal COMPNo As String, ByVal SALESID As String, ByVal RMANo As String, ByVal fdate As String, ByVal edate As String,
                Optional ByVal OrderBY As String = "") As ChargeQuotedDTO.vwChargeQuotedListDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedList As New ChargeQuotedDTO.vwChargeQuotedListDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_CSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If (SALESID <> "admin" AndAlso SALESID <> "0261" AndAlso SALESID <> "0210") Then
                oQuery.addWHERE("CU_SALESID", ":CU_SALESID", SALESID, OracleType.VarChar)
                oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", SALESID, OracleType.VarChar)
                sCondition = sCondition & " AND (CU_SALESID =:CU_SALESID OR CU_ASSISTANTID =:CU_ASSISTANTID)"
            End If


            If RMANo.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RMA_NO"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)

                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            'RMA_STATUS	   -->0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            'RMAD_STATUS      -->0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '2015/10/21 Fairy要求-->要列出rmad_status=91資料

            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_ID, RMA_NO, RMA_STATUS, RMA_CSTMP, RMA_APPLICANT, QTY, RMASQ_QUOTE, RMASQ_CURRENCYCODE,"
            sSQL = sSQL & "     RMACQ_CURRENCYCODE, RMACQ_SALEQUOTE, RMACQ_DISCOUNT, RMACQ_CHARGEQUOTE, RMACQ_CURRENCYRATE, "
            sSQL = sSQL & "     RMACQ_CURRENCYCODE_ORIGINAL, RMACQ_QUOTE_ORIGINAL, RMACQ_CURRENCYRATE_ORIGINAL,"
            sSQL = sSQL & "     RMACQ_APPROVAL, RMACQ_APPROVALDATE,CU_NAME "
            sSQL = sSQL & " FROM RMA"

            sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO"

            sSQL = sSQL & " INNER JOIN"
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as QTY "
            sSQL = sSQL & "     FROM RMADETAIL "
            'sSQL = sSQL & "     WHERE (RMAD_STATUS=40 OR RMAD_STATUS=50 OR RMAD_STATUS=60 OR RMAD_STATUS=70 OR RMAD_STATUS=90) AND RMAD_RECEVSTATUS<>2 AND RMADETAIL.RMAD_MARK=0"
            sSQL = sSQL & "     WHERE (RMAD_STATUS=40 OR RMAD_STATUS=50 OR RMAD_STATUS=60 OR RMAD_STATUS=70 OR RMAD_STATUS=90 OR RMAD_STATUS=91) AND RMAD_RECEVSTATUS<>2 AND RMADETAIL.RMAD_MARK=0"
            sSQL = sSQL & "     GROUP BY RMAD_RMANO"
            sSQL = sSQL & " ) vwRMADETAIL "
            sSQL = sSQL & " ON RMA.RMA_NO = vwRMADETAIL.RMAD_RMANO"

            sSQL = sSQL & " INNER JOIN"
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAD_RMANO, sum(RMASQ_QUOTE) as RMASQ_QUOTE, RMASQ_CURRENCYCODE as RMASQ_CURRENCYCODE"
            sSQL = sSQL & "     FROM RMADETAIL "
            'sSQL = sSQL & "     INNER JOIN RMASALE_QUOTED  ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID AND RMAD_STATUS<>91 "
            sSQL = sSQL & "     INNER JOIN RMASALE_QUOTED  ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID"
            sSQL = sSQL & "     GROUP BY RMAD_RMANO, RMASQ_CURRENCYCODE"
            sSQL = sSQL & " ) vwSALEQUOTED"
            sSQL = sSQL & " ON RMA.RMA_NO = vwSALEQUOTED.RMAD_RMANO"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"

            sSQL = sSQL & " WHERE (RMA_STATUS=10 OR RMA_STATUS=20 OR RMA_STATUS=90 OR RMA_STATUS=91)" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedList)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedList
    End Function

    Public Function QueryByChargeQuotedHead(ByVal RMANo As String) As ChargeQuotedDTO.vwChargeQuotedHeadDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedHead As New ChargeQuotedDTO.vwChargeQuotedHeadDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            'RMAD_STATUS : 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '2015/10/21 Fairy要求-->要列出rmad_status=91資料

            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
            sCondition = sCondition & " RMA_NO=:RMA_NO"

            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_NO, RMA_ID, RMA_STATUS, RMA_CSTMP, RMA_Remark,"
            sSQL = sSQL & "     CU_NO, CU_NAME, CUUS_ACCOUNTID, RMA_APPLICANT, CU_TIPTOP_ID, "
            sSQL = sSQL & "     COMP_NO, COMP_NAME,"
            sSQL = sSQL & "     RMASQ_CURRENCYCODE, RMASQ_QUOTE, RMASQ_CURRENCYRATE, RMASQ_SALEAD, RMASQ_SALEADNAME, "

            sSQL = sSQL & "     RMACQ_CURRENCYCODE, RMACQ_SALEQUOTE, RMACQ_DISCOUNT, RMACQ_CHARGEQUOTE, RMACQ_CURRENCYRATE, "
            sSQL = sSQL & "     RMACQ_CURRENCYCODE_ORIGINAL, RMACQ_QUOTE_ORIGINAL, RMACQ_CURRENCYRATE_ORIGINAL, "
            sSQL = sSQL & "     RMACQ_APPROVAL, RMACQ_APPROVALDATE "
            sSQL = sSQL & " FROM RMA "

            sSQL = sSQL & " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO"
            sSQL = sSQL & " INNER JOIN CUSTOMERUSER  ON RMA.RMA_ACCOUNTID = CUSTOMERUSER.CUUS_ACCOUNTID"
            sSQL = sSQL & " INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO "

            sSQL = sSQL & " INNER JOIN"
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAD_RMANO, sum(RMASQ_QUOTE) as RMASQ_QUOTE, RMASQ_CURRENCYCODE as RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, RMASQ_SALEAD, RMASQ_SALEADNAME"
            sSQL = sSQL & "     FROM RMADETAIL "
            'sSQL = sSQL & "     INNER JOIN RMASALE_QUOTED  ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID AND RMAD_STATUS<>91 "
            sSQL = sSQL & "     INNER JOIN RMASALE_QUOTED  ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
            sSQL = sSQL & "     GROUP BY RMAD_RMANO, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, RMASQ_SALEAD, RMASQ_SALEADNAME"
            sSQL = sSQL & " ) vwSALEQUOTED"
            sSQL = sSQL & " ON RMA.RMA_NO = vwSALEQUOTED.RMAD_RMANO "

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"

            sSQL = sSQL & " WHERE " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedHead)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedHead
    End Function

    Public Function QueryByChargeQuotedSN(ByVal LanguageID As String, ByVal RMANo As String, ByVal sRMAD_ID As String) As ChargeQuotedDTO.vwChargeQuotedSNDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
            sCondition = sCondition & " RMA_NO=:RMA_NO"

            If sRMAD_ID.Trim() <> "" Then
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", sRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"
            End If

            'RMA_STATUS	   -->0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            'RMAD_STATUS      -->0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '2015/10/21 Fairy要求-->要列出rmad_status=91資料

            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_NO, RMA_ID, RMA_STATUS, RMA_CSTMP, RMA_APPLICANT, "
            sSQL = sSQL & "     COMP_NO, COMP_NAME, "
            sSQL = sSQL & "     CU_TIPTOP_ID, "

            sSQL = sSQL & "     RMAD_ID, RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_WARRANTY, RMAD_CSTMP, "
            sSQL = sSQL & "     RMARQ_IMPROPERUSAGE,"
            sSQL = sSQL & "     FARC1.FARC_NAME FARC_NAME1, vwRMAREPAIR.FARC_NAME2,"

            sSQL = sSQL & "     RMASQ_LABORCOST, "
            'sSQL = sSQL & "     RMASQ_MATERIALCOST,"
            sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE RMASQ_MATERIALCOST END as RMASQ_MATERIALCOST, "
            sSQL = sSQL & "     RMASQ_QUOTE,"

            sSQL = sSQL & "     RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, RMARQ_ACCEPT, "

            sSQL = sSQL & "     RMACQSN_LABORCOST, RMACQSN_MATERIALCOST, RMACQSN_QUOTE, RMACQSN_DISCOUNTAMOUNT, RMACQSN_CURRENCYCODE, RMACQSN_CURRENCYRATE, "
            sSQL = sSQL & "     RMACQ_CHARGEDATE, RMACQ_APPROVAL, RMACQ_APPROVALDATE,CUSTOMER.CU_SALESID "
            sSQL = sSQL & "     ,GEN_FILE.GEN03"
            sSQL = sSQL & " FROM RMA "

            sSQL = sSQL & " INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO"
            sSQL = sSQL & " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO"

            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND RMAD_RECEVSTATUS=1 AND RMAD_MARK=0"
            'sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID AND RMARQ_ACCEPT=1"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID"
            sSQL = sSQL & " INNER JOIN RMASALE_QUOTED ON RMASALE_QUOTED.RMASQ_RMADID = RMADETAIL.RMAD_ID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID =  RMADETAIL.RMAD_ID"
            sSQL = sSQL & " LEFT OUTER JOIN FAILUREREASONSCLASS FARC1 ON RMADETAIL.rmad_farfarcno = FARC1.FARC_NO AND FARC1.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " LEFT OUTER JOIN "
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAR_RMADID, RMAR_FARCNO, FARC2.FARC_NAME FARC_NAME2 ,RMAR_PROBLEMDESC, RMAR_REPAIRDESC, RMAR_REPAIRMEMO"
            sSQL = sSQL & "     FROM  RMAREPAIR LEFT OUTER JOIN FAILUREREASONSCLASS FARC2"
            sSQL = sSQL & "     ON RMAREPAIR.RMAR_FARCNO = FARC2.FARC_NO AND FARC2.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " ) vwRMAREPAIR"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwRMAREPAIR.RMAR_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"
            sSQL = sSQL & " LEFT OUTER JOIN CIPHERLAB.GEN_FILE ON GEN_FILE.GEN01=CUSTOMER.CU_SALESID"

            sSQL = sSQL & " WHERE " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedSN)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedSN
    End Function

    Public Function QueryByBondChargeQuotedSN(ByVal LanguageID As String, ByVal RMANo As String, ByVal sRMAD_ID As String) As ChargeQuotedDTO.vwChargeQuotedSNDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
            sCondition = sCondition & " RMA_NO=:RMA_NO"

            If sRMAD_ID.Trim() <> "" Then
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", sRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"
            End If

            'RMA_STATUS	   -->0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            'RMAD_STATUS      -->0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '2015/10/21 Fairy要求-->要列出rmad_status=91資料

            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_NO, RMA_ID, RMA_STATUS, RMA_CSTMP, RMA_APPLICANT, "
            sSQL = sSQL & "     COMP_NO, COMP_NAME, "
            sSQL = sSQL & "     CU_TIPTOP_ID, "

            sSQL = sSQL & "     RMAD_ID, RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_WARRANTY, RMAD_CSTMP, "
            sSQL = sSQL & "     RMARQ_IMPROPERUSAGE,"
            sSQL = sSQL & "     FARC1.FARC_NAME FARC_NAME1, vwRMAREPAIR.FARC_NAME2,"

            sSQL = sSQL & "     RMASQ_LABORCOST, "
            'sSQL = sSQL & "     RMASQ_MATERIALCOST,"
            sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE RMASQ_MATERIALCOST END as RMASQ_MATERIALCOST, "
            sSQL = sSQL & "     RMASQ_QUOTE,"

            sSQL = sSQL & "     RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, RMARQ_ACCEPT, "

            sSQL = sSQL & "     RMACQSN_LABORCOST, RMACQSN_MATERIALCOST, RMACQSN_QUOTE, RMACQSN_DISCOUNTAMOUNT, RMACQSN_CURRENCYCODE, RMACQSN_CURRENCYRATE, "
            sSQL = sSQL & "     RMACQ_CHARGEDATE, RMACQ_APPROVAL, RMACQ_APPROVALDATE,CUSTOMER.CU_SALESID "
            sSQL = sSQL & "     ,GEN_FILE.GEN03"
            sSQL = sSQL & " FROM RMA "

            sSQL = sSQL & " INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO"
            sSQL = sSQL & " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO"

            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND RMAD_RECEVSTATUS=1 AND RMAD_MARK=0"
            'sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID AND RMARQ_ACCEPT=1"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID"
            sSQL = sSQL & " INNER JOIN RMASALE_QUOTED ON RMASALE_QUOTED.RMASQ_RMADID = RMADETAIL.RMAD_ID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID =  RMADETAIL.RMAD_ID"
            sSQL = sSQL & " LEFT OUTER JOIN FAILUREREASONSCLASS FARC1 ON RMADETAIL.rmad_farfarcno = FARC1.FARC_NO AND FARC1.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " LEFT OUTER JOIN "
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAR_RMADID, RMAR_FARCNO, FARC2.FARC_NAME FARC_NAME2 ,RMAR_PROBLEMDESC, RMAR_REPAIRDESC, RMAR_REPAIRMEMO"
            sSQL = sSQL & "     FROM  RMAREPAIR LEFT OUTER JOIN FAILUREREASONSCLASS FARC2"
            sSQL = sSQL & "     ON RMAREPAIR.RMAR_FARCNO = FARC2.FARC_NO AND FARC2.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " ) vwRMAREPAIR"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwRMAREPAIR.RMAR_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"
            sSQL = sSQL & " LEFT OUTER JOIN CIPHERBOND.GEN_FILE ON GEN_FILE.GEN01=CUSTOMER.CU_SALESID"

            sSQL = sSQL & " WHERE " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedSN)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedSN
    End Function

    Public Function QueryBySHChargeQuotedSN(ByVal LanguageID As String, ByVal RMANo As String, ByVal sRMAD_ID As String) As ChargeQuotedDTO.vwChargeQuotedSNDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedSN As New ChargeQuotedDTO.vwChargeQuotedSNDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
            sCondition = sCondition & " RMA_NO=:RMA_NO"

            If sRMAD_ID.Trim() <> "" Then
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", sRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"
            End If

            'RMA_STATUS	   -->0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
            'RMAD_RECEVSTATUS -->是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
            'RMAD_STATUS      -->0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            '2015/10/21 Fairy要求-->要列出rmad_status=91資料

            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_NO, RMA_ID, RMA_STATUS, RMA_CSTMP, RMA_APPLICANT, "
            sSQL = sSQL & "     COMP_NO, COMP_NAME, "
            sSQL = sSQL & "     CU_TIPTOP_ID, "

            sSQL = sSQL & "     RMAD_ID, RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_WARRANTY, RMAD_CSTMP, "
            sSQL = sSQL & "     RMARQ_IMPROPERUSAGE,"
            sSQL = sSQL & "     FARC1.FARC_NAME FARC_NAME1, vwRMAREPAIR.FARC_NAME2,"

            sSQL = sSQL & "     RMASQ_LABORCOST, "
            'sSQL = sSQL & "     RMASQ_MATERIALCOST,"
            sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE RMASQ_MATERIALCOST END as RMASQ_MATERIALCOST, "
            sSQL = sSQL & "     RMASQ_QUOTE,"

            sSQL = sSQL & "     RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, RMARQ_ACCEPT, "

            sSQL = sSQL & "     RMACQSN_LABORCOST, RMACQSN_MATERIALCOST, RMACQSN_QUOTE, RMACQSN_DISCOUNTAMOUNT, RMACQSN_CURRENCYCODE, RMACQSN_CURRENCYRATE, "
            sSQL = sSQL & "     RMACQ_CHARGEDATE, RMACQ_APPROVAL, RMACQ_APPROVALDATE,CUSTOMER.CU_SALESID "
            sSQL = sSQL & "     ,GEN_FILE.GEN03"
            sSQL = sSQL & " FROM RMA "

            sSQL = sSQL & " INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO"
            sSQL = sSQL & " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO"

            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND RMAD_RECEVSTATUS=1 AND RMAD_MARK=0"
            'sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID AND RMARQ_ACCEPT=1"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADETAIL.RMAD_ID"
            sSQL = sSQL & " INNER JOIN RMASALE_QUOTED ON RMASALE_QUOTED.RMASQ_RMADID = RMADETAIL.RMAD_ID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID =  RMADETAIL.RMAD_ID"
            sSQL = sSQL & " LEFT OUTER JOIN FAILUREREASONSCLASS FARC1 ON RMADETAIL.rmad_farfarcno = FARC1.FARC_NO AND FARC1.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " LEFT OUTER JOIN "
            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMAR_RMADID, RMAR_FARCNO, FARC2.FARC_NAME FARC_NAME2 ,RMAR_PROBLEMDESC, RMAR_REPAIRDESC, RMAR_REPAIRMEMO"
            sSQL = sSQL & "     FROM  RMAREPAIR LEFT OUTER JOIN FAILUREREASONSCLASS FARC2"
            sSQL = sSQL & "     ON RMAREPAIR.RMAR_FARCNO = FARC2.FARC_NO AND FARC2.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " ) vwRMAREPAIR"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwRMAREPAIR.RMAR_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO"
            sSQL = sSQL & " LEFT OUTER JOIN CIPHERSH.GEN_FILE ON GEN_FILE.GEN01=CUSTOMER.CU_SALESID"

            sSQL = sSQL & " WHERE " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedSN)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedSN
    End Function

    Public Function QueryByChargeQuotedPART(ByVal sRMA_NO As String, ByVal sRMAD_ID As String) As ChargeQuotedDTO.vwChargeQuotedPARTDataTable

        Dim sCondition As String = ""
        Dim dtChargeQuotedPART As New ChargeQuotedDTO.vwChargeQuotedPARTDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("RMA_NO", ":RMA_NO", sRMA_NO, OracleType.VarChar)
            sCondition = sCondition & " RMA_NO=:RMA_NO"

            If sRMAD_ID.Trim() <> "" Then
                oQuery.addWHERE("RMARQD_RMADID", ":RMARQD_RMADID", sRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARQD_RMADID=:RMARQD_RMADID"
            End If

            'RMARQD_WAIVE：表示此零件是我方吸收必修，維修收費價格會是0；
            'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
            Dim sSQL As String = ""
            sSQL = sSQL & " SELECT RMA_NO, RMARQD_ID, RMARQD_RMADID, RMARQD_NPARTNO, RMARQD_DESC, "
            sSQL = sSQL & "     RMARQD_QTY, RMARQD_MATERIALCOST, RMARQD_PRICE, RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, "
            sSQL = sSQL & "     RMARQD_WAIVE, RMARQD_OPTION,  RMARQD_OPTIONCLIENT,"

            sSQL = sSQL & "     RMACQPT_QTY, RMACQPT_MATERIALCOST, RMACQPT_PRICE, RMACQPT_RECHARGE_PRICE, RMACQPT_CURRENCYCODE, RMACQPT_CURRENCYRATE,"
            sSQL = sSQL & "     RMACQPT_QTY_ORIGINAL, RMACQPT_MATERIALCOST_ORIGINAL, RMACQPT_PRICE_ORIGINAL, RMACQPT_CURRENCYCODE_ORIGINAL, RMACQPT_CURRENCYRATE_ORIGINAL"
            sSQL = sSQL & " FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND RMAD_RECEVSTATUS=1 AND RMAD_MARK=0"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED_DETAIL ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED_DETAIL.RMARQD_RMADID "
            sSQL = sSQL & "     AND RMAREPAIR_QUOTED_DETAIL.RMARQD_MARK=0 AND RMAREPAIR_QUOTED_DETAIL.RMARQD_OPTIONCLIENT<>1"

            sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_PART ON RMACHARGE_QUOTED_PART.RMACQPT_RMARQD_ID = RMAREPAIR_QUOTED_DETAIL.RMARQD_ID  "
            sSQL = sSQL & "     AND RMACHARGE_QUOTED_PART.RMACQPT_RMADID = RMAREPAIR_QUOTED_DETAIL.RMARQD_RMADID"

            sSQL = sSQL & " WHERE " & sCondition
            sSQL = sSQL & " ORDER BY rmarqd_cstmp asc, RMARQD_ID asc"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtChargeQuotedPART)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtChargeQuotedPART
    End Function

    Public Sub saveChargeQUOTED(ByVal dtRMACharge_QUOTED As ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable, ByVal dtChargeQUOTED_SN As ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable,
            ByVal dtRMACHARGE_QUOTED_PART As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable)

        Dim sSQL As String = ""
        Dim i As Integer = 0
        Dim isChargePART As Boolean = False

        Dim dtQuery As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            Dim dr As ChargeQuotedDTO.RMACHARGE_QUOTEDRow = dtRMACharge_QUOTED.Rows(0)
            Dim sRMA_NO As String = dr.RMACQ_RMANO.Trim()


            '==================================================================================================================================================================================================================================================
            'RMACHARGE_QUOTED Table
            '==================================================================================================================================================================================================================================================
            sSQL = ""
            oQuery.addWHERE("RMA_NO", ":RMA_NO", sRMA_NO, OracleType.VarChar)

            sSQL = sSQL & " SELECT * FROM RMA  "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO"
            sSQL = sSQL & " INNER JOIN RMASALE_QUOTED_CLOG ON RMADETAIL.RMAD_ID = RMASALE_QUOTED_CLOG.RMASQCLOG_RMADID"
            sSQL = sSQL & " WHERE RMA_NO=:RMA_NO"
            dtQuery = oQuery.ExecuteDT(sSQL)
            If dtQuery.Rows.Count = 0 Then
                'add RMASALE_QUOTED_CLOG Table
                addNewRMASALE_QUOTED_CLOG(oConn, sRMA_NO)

                'add RMAREPAIR_QUOTED_CLOG Table
                addNewRMAREPAIR_QUOTED_CLOG(oConn, sRMA_NO)

                'add RMAREPAIR_QUOTED_DETAIL_CLOG Table
                addNewRMAREPAIR_QUOTED_DETAIL_CLOG(oConn, sRMA_NO)
            End If




            '==================================================================================================================================================================================================================================================
            'RMACHARGE_QUOTED Table
            '==================================================================================================================================================================================================================================================

            oQuery.addWHERE("RMACQ_RMANO", ":RMACQ_RMANO", sRMA_NO, OracleType.VarChar)
            sSQL = "SELECT * FROM RMACHARGE_QUOTED WHERE RMACQ_RMANO=:RMACQ_RMANO "
            dtQuery = oQuery.ExecuteDT(sSQL)
            If dtQuery.Rows.Count = 0 Then
                'add
                oExecute.addParameter("RMACQ_ID", dr.RMACQ_ID, OracleType.VarChar)
                oExecute.addParameter("RMACQ_RMANO", dr.RMACQ_RMANO, OracleType.VarChar)
                oExecute.addParameter("RMACQ_CHARGEDATE", dr.RMACQ_CHARGEDATE, OracleType.DateTime)

                oExecute.addParameter("RMACQ_SALEQUOTE", dr.RMACQ_SALEQUOTE, OracleType.Double)
                oExecute.addParameter("RMACQ_DISCOUNT", dr.RMACQ_DISCOUNT, OracleType.Double)
                oExecute.addParameter("RMACQ_CHARGEQUOTE", dr.RMACQ_CHARGEQUOTE, OracleType.Double)
                oExecute.addParameter("RMACQ_CURRENCYCODE", dr.RMACQ_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("RMACQ_CURRENCYRATE", dr.RMACQ_CURRENCYRATE, OracleType.Double)

                oExecute.addParameter("RMACQ_QUOTE_ORIGINAL", dr.RMACQ_QUOTE_ORIGINAL, OracleType.Double)
                oExecute.addParameter("RMACQ_CURRENCYCODE_ORIGINAL", dr.RMACQ_CURRENCYCODE_ORIGINAL, OracleType.VarChar)
                oExecute.addParameter("RMACQ_CURRENCYRATE_ORIGINAL", dr.RMACQ_CURRENCYRATE_ORIGINAL, OracleType.Double)

                If dr.IsRMACQ_APPROVALNull = False Then oExecute.addParameter("RMACQ_APPROVAL", dr.RMACQ_APPROVAL, OracleType.Int16)
                If dr.IsRMACQ_APPROVALDATENull = False Then oExecute.addParameter("RMACQ_APPROVALDATE", dr.RMACQ_APPROVALDATE, OracleType.DateTime)

                oExecute.addParameter("RMACQ_AD", dr.RMACQ_AD, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_ADNAME", dr.RMACQ_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_CSTMP", dr.RMACQ_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMACQ_LUAD", dr.RMACQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_LUADNAME", dr.RMACQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_LUSTMP", dr.RMACQ_LUSTMP, OracleType.DateTime)

                oExecute.Command("RMACHARGE_QUOTED", Execute.eumCommandType.AddNew)
            Else

                'edit
                oExecute.addParameter("RMACQ_CHARGEDATE", dr.RMACQ_CHARGEDATE, OracleType.DateTime)

                oExecute.addParameter("RMACQ_SALEQUOTE", dr.RMACQ_SALEQUOTE, OracleType.Double)
                oExecute.addParameter("RMACQ_DISCOUNT", dr.RMACQ_DISCOUNT, OracleType.Double)
                oExecute.addParameter("RMACQ_CHARGEQUOTE", dr.RMACQ_CHARGEQUOTE, OracleType.Double)
                oExecute.addParameter("RMACQ_CURRENCYCODE", dr.RMACQ_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("RMACQ_CURRENCYRATE", dr.RMACQ_CURRENCYRATE, OracleType.Double)

                If dr.IsRMACQ_APPROVALNull = False Then oExecute.addParameter("RMACQ_APPROVAL", dr.RMACQ_APPROVAL, OracleType.Int16)
                If dr.IsRMACQ_APPROVALDATENull = False Then oExecute.addParameter("RMACQ_APPROVALDATE", dr.RMACQ_APPROVALDATE, OracleType.DateTime)

                oExecute.addParameter("RMACQ_LUAD", dr.RMACQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_LUADNAME", dr.RMACQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMACQ_LUSTMP", dr.RMACQ_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("RMACQ_RMANO", dr.RMACQ_RMANO, OracleType.VarChar)
                oExecute.Command("RMACHARGE_QUOTED", Execute.eumCommandType.UPDATE)
            End If


            '==================================================================================================================================================================================================================================================
            '有異動 RMACHARGE_QUOTED_PART
            '==================================================================================================================================================================================================================================================
            If dtRMACHARGE_QUOTED_PART.Rows.Count > 0 Then
                isChargePART = True
                For i = 0 To dtRMACHARGE_QUOTED_PART.Rows.Count - 1
                    Dim drPART As ChargeQuotedDTO.RMACHARGE_QUOTED_PARTRow = dtRMACHARGE_QUOTED_PART.Rows(i)
                    Dim RMACQPT_RMARQD_ID As String = drPART.RMACQPT_RMARQD_ID.Trim()
                    Dim RMACQPT_RMADID As String = drPART.RMACQPT_RMADID.Trim()

                    oQuery.addWHERE("RMACQPT_RMARQD_ID", ":RMACQPT_RMARQD_ID", RMACQPT_RMARQD_ID, OracleType.VarChar)
                    oQuery.addWHERE("RMACQPT_RMADID", ":RMACQPT_RMADID", RMACQPT_RMADID, OracleType.VarChar)
                    sSQL = "SELECT * FROM RMACHARGE_QUOTED_PART WHERE RMACQPT_RMARQD_ID=:RMACQPT_RMARQD_ID AND RMACQPT_RMADID=:RMACQPT_RMADID"
                    dtQuery = oQuery.ExecuteDT(sSQL)
                    If dtQuery.Rows.Count = 0 Then
                        'add
                        oExecute.addParameter("RMACQPT_ID", drPART.RMACQPT_ID, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_RMARQD_ID", drPART.RMACQPT_RMARQD_ID, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_RMADID", drPART.RMACQPT_RMADID, OracleType.NVarChar)

                        oExecute.addParameter("RMACQPT_QTY", drPART.RMACQPT_QTY, OracleType.Double)
                        oExecute.addParameter("RMACQPT_MATERIALCOST", drPART.RMACQPT_MATERIALCOST, OracleType.Double)
                        oExecute.addParameter("RMACQPT_PRICE", drPART.RMACQPT_PRICE, OracleType.Double)
                        oExecute.addParameter("RMACQPT_RECHARGE_PRICE", drPART.RMACQPT_RECHARGE_PRICE, OracleType.Double)
                        oExecute.addParameter("RMACQPT_CURRENCYCODE", drPART.RMACQPT_CURRENCYCODE, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_CURRENCYRATE", drPART.RMACQPT_CURRENCYRATE, OracleType.Double)

                        oExecute.addParameter("RMACQPT_QTY_ORIGINAL", drPART.RMACQPT_QTY_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQPT_MATERIALCOST_ORIGINAL", drPART.RMACQPT_MATERIALCOST_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQPT_PRICE_ORIGINAL", drPART.RMACQPT_PRICE_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQPT_CURRENCYCODE_ORIGINAL", drPART.RMACQPT_CURRENCYCODE_ORIGINAL, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_CURRENCYRATE_ORIGINAL", drPART.RMACQPT_CURRENCYRATE_ORIGINAL, OracleType.Double)

                        oExecute.addParameter("RMACQPT_AD", drPART.RMACQPT_AD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_ADNAME", drPART.RMACQPT_ADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_CSTMP", drPART.RMACQPT_CSTMP, OracleType.DateTime)
                        oExecute.addParameter("RMACQPT_LUAD", drPART.RMACQPT_LUAD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_LUADNAME", drPART.RMACQPT_LUADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_LUSTMP", drPART.RMACQPT_LUSTMP, OracleType.DateTime)

                        oExecute.Command("RMACHARGE_QUOTED_PART", Execute.eumCommandType.AddNew)
                    Else

                        'edit
                        oExecute.addParameter("RMACQPT_RECHARGE_PRICE", drPART.RMACQPT_RECHARGE_PRICE, OracleType.Double)

                        oExecute.addParameter("RMACQPT_LUAD", drPART.RMACQPT_LUAD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_LUADNAME", drPART.RMACQPT_LUADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQPT_LUSTMP", drPART.RMACQPT_LUSTMP, OracleType.DateTime)

                        oExecute.addWHERE("RMACQPT_RMARQD_ID", RMACQPT_RMARQD_ID, OracleType.VarChar)
                        oExecute.addWHERE("RMACQPT_RMADID", RMACQPT_RMADID, OracleType.VarChar)
                        oExecute.Command("RMACHARGE_QUOTED_PART", Execute.eumCommandType.UPDATE)
                    End If


                    '==================================================================================================================================================================================================================================================
                    '1. 需重新整理 RMAREPAIR_QUOTED_DETAIL Table
                    '==================================================================================================================================================================================================================================================
                    oExecute.addParameter("RMARQD_PRICE", drPART.RMACQPT_RECHARGE_PRICE, OracleType.Double)

                    'oExecute.addParameter("RMARQD_LUAD", drPART.RMACQPT_LUAD, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQD_LUADNAME", drPART.RMACQPT_LUADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQD_LUSTMP", drPART.RMACQPT_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMARQD_ID", RMACQPT_RMARQD_ID, OracleType.VarChar)
                    oExecute.addWHERE("RMARQD_RMADID", RMACQPT_RMADID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_QUOTED_DETAIL", Execute.eumCommandType.UPDATE)

                Next
            End If



            '==================================================================================================================================================================================================================================================
            '有異動 RMACHARGE_QUOTED_SN
            '==================================================================================================================================================================================================================================================
            If dtChargeQUOTED_SN.Rows.Count > 0 Then
                For i = 0 To dtChargeQUOTED_SN.Rows.Count - 1
                    Dim drSN As ChargeQuotedDTO.RMACHARGE_QUOTED_SNRow = dtChargeQUOTED_SN.Rows(i)
                    Dim RMACQSN_RMADID As String = drSN.RMACQSN_RMADID.Trim()

                    oQuery.addWHERE("RMACQSN_RMADID", ":RMACQSN_RMADID", RMACQSN_RMADID, OracleType.VarChar)
                    sSQL = "SELECT * FROM RMACHARGE_QUOTED_SN WHERE RMACQSN_RMADID=:RMACQSN_RMADID "
                    dtQuery = oQuery.ExecuteDT(sSQL)
                    If dtQuery.Rows.Count = 0 Then
                        'add
                        oExecute.addParameter("RMACQSN_ID", drSN.RMACQSN_ID, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_RMADID", drSN.RMACQSN_RMADID, OracleType.NVarChar)

                        oExecute.addParameter("RMACQSN_LABORCOST", drSN.RMACQSN_LABORCOST, OracleType.Double)
                        oExecute.addParameter("RMACQSN_MATERIALCOST", drSN.RMACQSN_MATERIALCOST, OracleType.Double)
                        oExecute.addParameter("RMACQSN_QUOTE", drSN.RMACQSN_QUOTE, OracleType.Double)

                        oExecute.addParameter("RMACQSN_DISCOUNTAMOUNT", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)

                        oExecute.addParameter("RMACQSN_CURRENCYCODE", drSN.RMACQSN_CURRENCYCODE, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_CURRENCYRATE", drSN.RMACQSN_CURRENCYRATE, OracleType.Double)

                        oExecute.addParameter("RMACQSN_LABORCOST_ORIGINAL", drSN.RMACQSN_LABORCOST_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQSN_MATERIALCOST_ORIGINAL", drSN.RMACQSN_MATERIALCOST_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQSN_QUOTE_ORIGINAL", drSN.RMACQSN_QUOTE_ORIGINAL, OracleType.Double)
                        oExecute.addParameter("RMACQSN_CURRENCYCODE_ORIGINAL", drSN.RMACQSN_CURRENCYCODE_ORIGINAL, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_CURRENCYRATE_ORIGINAL", drSN.RMACQSN_CURRENCYRATE_ORIGINAL, OracleType.Double)

                        oExecute.addParameter("RMACQSN_AD", drSN.RMACQSN_AD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_ADNAME", drSN.RMACQSN_ADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_CSTMP", drSN.RMACQSN_CSTMP, OracleType.DateTime)
                        oExecute.addParameter("RMACQSN_LUAD", drSN.RMACQSN_LUAD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_LUADNAME", drSN.RMACQSN_LUADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_LUSTMP", drSN.RMACQSN_LUSTMP, OracleType.DateTime)

                        oExecute.Command("RMACHARGE_QUOTED_SN", Execute.eumCommandType.AddNew)

                    Else
                        'edit
                        'oExecute.addParameter("RMACQSN_LABORCOST", drSN.RMACQSN_LABORCOST, OracleType.Double)
                        'oExecute.addParameter("RMACQSN_MATERIALCOST", drSN.RMACQSN_MATERIALCOST, OracleType.Double)
                        'oExecute.addParameter("RMACQSN_QUOTE", drSN.RMACQSN_QUOTE, OracleType.Double)
                        oExecute.addParameter("RMACQSN_DISCOUNTAMOUNT", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)
                        'oExecute.addParameter("RMACQSN_CURRENCYCODE", drSN.RMACQSN_CURRENCYCODE, OracleType.NVarChar)
                        'oExecute.addParameter("RMACQSN_CURRENCYRATE", drSN.RMACQSN_CURRENCYRATE, OracleType.Double)

                        oExecute.addParameter("RMACQSN_LUAD", drSN.RMACQSN_LUAD, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_LUADNAME", drSN.RMACQSN_LUADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMACQSN_LUSTMP", drSN.RMACQSN_LUSTMP, OracleType.DateTime)

                        oExecute.addWHERE("RMACQSN_RMADID", drSN.RMACQSN_RMADID, OracleType.VarChar)
                        oExecute.Command("RMACHARGE_QUOTED_SN", Execute.eumCommandType.UPDATE)
                    End If



                    '==================================================================================================================================================================================================================================================
                    '1. 需重新整理 RMAREPAIR_QUOTED Table
                    '==================================================================================================================================================================================================================================================
                    '有異動 零件的費用, 所以維修報價也必須異動 零件的費用 及 費用總金額
                    If isChargePART = True Then
                        If drSN.IsCharge_MaterialCostNull = False Then
                            oExecute.addParameter("RMARQ_MATERIALCOST", drSN.Charge_MaterialCost, OracleType.Double)
                        Else
                            oExecute.addParameter("RMARQ_MATERIALCOST", drSN.RMACQSN_MATERIALCOST, OracleType.Double)
                        End If
                        oExecute.addParameter("RMARQ_QUOTE", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)

                    Else
                        oExecute.addParameter("RMARQ_QUOTE", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)
                    End If

                    'oExecute.addParameter("RMARQ_LUAD", drSN.RMACQSN_LUAD, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQ_LUADNAME", drSN.RMACQSN_LUADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQ_LUSTMP", drSN.RMACQSN_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMARQ_RMADID", drSN.RMACQSN_RMADID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_QUOTED", Execute.eumCommandType.UPDATE)


                    '==================================================================================================================================================================================================================================================
                    '2. 需重新整理 RMASALE_QUOTED Table
                    '==================================================================================================================================================================================================================================================
                    '有異動 零件的費用, 所以業務報價也必須異動 零件的費用 及 費用總金額
                    If isChargePART = True Then
                        'oExecute.addParameter("RMASQ_MATERIALCOST", drSN.RMACQSN_MATERIALCOST, OracleType.Double)
                        'oExecute.addParameter("RMASQ_QUOTE", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)
                        If drSN.IsCharge_MaterialCostNull = False Then
                            oExecute.addParameter("RMASQ_MATERIALCOST", drSN.Charge_MaterialCost, OracleType.Double)
                        Else
                            oExecute.addParameter("RMASQ_MATERIALCOST", drSN.RMACQSN_MATERIALCOST, OracleType.Double)
                        End If
                        oExecute.addParameter("RMASQ_QUOTE", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)
                    Else
                        oExecute.addParameter("RMASQ_QUOTE", drSN.RMACQSN_DISCOUNTAMOUNT, OracleType.Double)
                    End If

                    oExecute.addParameter("RMASQ_SALEAD", drSN.RMACQSN_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_SALEADNAME", drSN.RMACQSN_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_SALEDATE", drSN.RMACQSN_LUSTMP, OracleType.DateTime)

                    'oExecute.addParameter("RMASQ_LUAD", drSN.RMACQSN_LUAD, OracleType.NVarChar)
                    'oExecute.addParameter("RMASQ_LUADNAME", drSN.RMACQSN_LUADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("RMASQ_LUSTMP", drSN.RMACQSN_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMASQ_RMADID", drSN.RMACQSN_RMADID, OracleType.VarChar)
                    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)
                Next
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
    ''' add New RMASALE_QUOTED_CLOG Table
    ''' </summary>
    ''' <param name="oConn"></param>
    ''' <param name="sRMA_NO"></param>
    ''' <remarks></remarks>
    Private Sub addNewRMASALE_QUOTED_CLOG(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sRMA_NO As String)
        Dim sSQL As String = ""

        Dim oCommand As OracleCommand = oConn.Command

        Try
            sSQL = sSQL & " INSERT INTO RMASALE_QUOTED_CLOG( "
            sSQL = sSQL & "   RMASQCLOG_ID, RMASQCLOG_RMADID, "
            sSQL = sSQL & "   RMASQCLOG_LABORCOST, RMASQCLOG_MATERIALCOST, RMASQCLOG_QUOTE, RMASQCLOG_CURRENCYCODE, RMASQCLOG_CURRENCYRATE, "
            sSQL = sSQL & "   RMASQCLOG_DESC, "
            sSQL = sSQL & "   RMASQCLOG_AD, RMASQCLOG_ADNAME, RMASQCLOG_CSTMP, RMASQCLOG_LUAD, RMASQCLOG_LUADNAME, RMASQCLOG_LUSTMP, "
            sSQL = sSQL & "   RMASQCLOG_SALEAD, RMASQCLOG_SALEADNAME, RMASQCLOG_SALEDATE, RMASQCLOG_CLIENTAD, RMASQCLOG_CLIENTADNAME, RMASQCLOG_CLIENTDATE, RMASQCLOG_CLIENTCONFIRM"
            sSQL = sSQL & " )"
            sSQL = sSQL & " SELECT "
            sSQL = sSQL & "   RMASQ_ID, RMASQ_RMADID, "
            sSQL = sSQL & "   RMASQ_LABORCOST, RMASQ_MATERIALCOST, RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, "
            sSQL = sSQL & "   RMASQ_DESC, "
            sSQL = sSQL & "   RMASQ_AD, RMASQ_ADNAME, RMASQ_CSTMP, RMASQ_LUAD, RMASQ_LUADNAME, RMASQ_LUSTMP, "
            sSQL = sSQL & "   RMASQ_SALEAD, RMASQ_SALEADNAME, RMASQ_SALEDATE, RMASQ_CLIENTAD, RMASQ_CLIENTADNAME, RMASQ_CLIENTDATE, RMASQ_CLIENTCONFIRM"
            sSQL = sSQL & " FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO"
            sSQL = sSQL & " INNER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID"

            sSQL = sSQL & " WHERE RMA.RMA_NO ='" & sRMA_NO & "'"

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex

        Finally
        End Try
    End Sub

    ''' <summary>
    ''' add new RMAREPAIR_QUOTED_CLOG Table
    ''' </summary>
    ''' <param name="oConn"></param>
    ''' <param name="sRMA_NO"></param>
    ''' <remarks></remarks>
    Private Sub addNewRMAREPAIR_QUOTED_CLOG(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sRMA_NO As String)
        Dim sSQL As String = ""

        Dim oCommand As OracleCommand = oConn.Command

        Try
            sSQL = sSQL & " INSERT INTO RMAREPAIR_QUOTED_CLOG("
            sSQL = sSQL & "   RMARQCLOG_ID, RMARQCLOG_RMADID, RMARQCLOG_COMPNO, RMARQCLOG_IMPROPERUSAGE, "
            sSQL = sSQL & "   RMARQCLOG_LABORHOUR, RMARQCLOG_LABORPRICE, RMARQCLOG_MATERIALCOST, RMARQCLOG_QUOTE, RMARQCLOG_CURRENCYCODE, RMARQCLOG_CURRENCYRATE, "
            sSQL = sSQL & "   RMARQCLOG_ASSIGLABORCOST, RMARQCLOG_ASSIGMATERIALCOST, RMARQCLOG_ASSIGEQUOTE, RMARQCLOG_ASSIGECURRENCYCODE, RMARQCLOG_ASSIGECURRENCYRATE, "
            sSQL = sSQL & "   RMARQCLOG_AD, RMARQCLOG_ADNAME, RMARQCLOG_CSTMP, RMARQCLOG_LUAD, RMARQCLOG_LUADNAME, RMARQCLOG_LUSTMP, RMARQCLOG_ACCEPT  "
            sSQL = sSQL & " )"
            sSQL = sSQL & " SELECT "
            sSQL = sSQL & "   RMARQ_ID, RMARQ_RMADID, RMARQ_COMPNO, RMARQ_IMPROPERUSAGE, "
            sSQL = sSQL & "   RMARQ_LABORHOUR, RMARQ_LABORPRICE, RMARQ_MATERIALCOST, RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_CURRENCYRATE, "
            sSQL = sSQL & "   RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, RMARQ_ASSIGECURRENCYRATE, "
            sSQL = sSQL & "   RMARQ_AD, RMARQ_ADNAME, RMARQ_CSTMP, RMARQ_LUAD, RMARQ_LUADNAME, RMARQ_LUSTMP, RMARQ_ACCEPT"
            sSQL = sSQL & " FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID"

            sSQL = sSQL & " WHERE RMA.RMA_NO ='" & sRMA_NO & "'"

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex

        Finally
        End Try
    End Sub

    ''' <summary>
    ''' add new RMAREPAIR_QUOTED_DETAIL_CLOG Table
    ''' </summary>
    ''' <param name="oConn"></param>
    ''' <param name="sRMA_NO"></param>
    ''' <remarks></remarks>
    Private Sub addNewRMAREPAIR_QUOTED_DETAIL_CLOG(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sRMA_NO As String)
        Dim sSQL As String = ""

        Dim oCommand As OracleCommand = oConn.Command

        Try

            sSQL = sSQL & " INSERT INTO RMAREPAIR_QUOTED_DETAIL_CLOG("
            sSQL = sSQL & "   RMARQDCLOG_ID, RMARQDCLOG_RMADID, "
            sSQL = sSQL & "   RMARQDCLOG_NPARTNO, RMARQDCLOG_NSERIALNO, RMARQDCLOG_NWARRANTY, "
            sSQL = sSQL & "   RMARQDCLOG_OPARTNO, RMARQDCLOG_OSERIALNO, RMARQDCLOG_OWARRANTY, "
            sSQL = sSQL & "   RMARQDCLOG_DESC, RMARQDCLOG_LOCATION, RMARQDCLOG_IMPROPERUSAGE, RMARQDCLOG_DEFECTIVE, "
            sSQL = sSQL & "   RMARQDCLOG_QTY, RMARQDCLOG_MATERIALCOST, RMARQDCLOG_PRICE, RMARQDCLOG_CURRENCYCODE, RMARQDCLOG_CURRENCYRATE, "
            sSQL = sSQL & "   RMARQDCLOG_ASSIGEPRICE, RMARQDCLOG_ASSIGECURRENCYCODE, RMARQDCLOG_ASSIGECURRENCYRATE, "
            sSQL = sSQL & "   RMARQDCLOG_AD, RMARQDCLOG_ADNAME, RMARQDCLOG_CSTMP, RMARQDCLOG_LUAD, RMARQDCLOG_LUADNAME, RMARQDCLOG_LUSTMP, "
            sSQL = sSQL & "   RMARQDCLOG_MARK, RMARQDCLOG_WAIVE, RMARQDCLOG_OPTION, RMARQDCLOG_OPTIONCLIENT"
            sSQL = sSQL & " )"
            sSQL = sSQL & " SELECT "
            sSQL = sSQL & "   RMARQD_ID, RMARQD_RMADID, "
            sSQL = sSQL & "   RMARQD_NPARTNO, RMARQD_NSERIALNO, RMARQD_NWARRANTY, "
            sSQL = sSQL & "   RMARQD_OPARTNO, RMARQD_OSERIALNO, RMARQD_OWARRANTY, "
            sSQL = sSQL & "   RMARQD_DESC, RMARQD_LOCATION, RMARQD_IMPROPERUSAGE, RMARQD_DEFECTIVE, "
            sSQL = sSQL & "   RMARQD_QTY, RMARQD_MATERIALCOST, RMARQD_PRICE, RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, "
            sSQL = sSQL & "   RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE, "
            sSQL = sSQL & "   RMARQD_AD, RMARQD_ADNAME, RMARQD_CSTMP, RMARQD_LUAD, RMARQD_LUADNAME, RMARQD_LUSTMP, "
            sSQL = sSQL & "   RMARQD_MARK, RMARQD_WAIVE, RMARQD_OPTION, RMARQD_OPTIONCLIENT"
            sSQL = sSQL & " FROM RMA "
            sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO"
            sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED_DETAIL ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED_DETAIL.RMARQD_RMADID"

            sSQL = sSQL & " WHERE RMARQD_MARK=0 AND RMA.RMA_NO ='" & sRMA_NO & "'"

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex

        Finally
        End Try
    End Sub

End Class

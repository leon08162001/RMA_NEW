Imports System.Data.OracleClient
Imports System.Linq
Imports DataService.ReportDTO
Imports DefLanguage
Imports ICAT_OracleDAO

Public Class ctlReport
    Dim _oLanguage As New ctlLanguage
    Dim _SQLCollection As New Collection

    Public Function GetShipRmaNo(ByVal SHIPPINGNO As String) As DataTable
        Dim sCondition As String = ""
        Dim strSQL As String
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("RMASH_SHIPPINGNO", ":RMASH_SHIPPINGNO", SHIPPINGNO, OracleType.VarChar)
            sCondition = sCondition & " AND RMASH_SHIPPINGNO=:RMASH_SHIPPINGNO"
            strSQL = "SELECT DISTINCT RMASHD_RMANO " &
                     " FROM RMA_SHIPPING JOIN RMA_SHIPPINGDETAIL ON RMASH_ID = RMASHD_RMASHID " &
                     " WHERE 1=1 "
            strSQL = strSQL & sCondition

            dt = oQuery.ExecuteDT(strSQL)
        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function GetInvoiceData(ByVal RMANO As String) As RmaDTO.rptPrintInvoiceDataTable
        Dim sCondition As String = ""
        Dim strSQL As String
        Dim dtInvoice As New RmaDTO.rptPrintInvoiceDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANO, OracleType.VarChar)
            sCondition = sCondition & " AND RMA_NO=:RMA_NO"
            strSQL = "SELECT RMA_NO,OMA02,OMA01,OMA10,OMA54,OMA54X, " &
                     "          OMA54T,OFA01,OCC01,OCC18,OCC11,OCC231 " &
                     "    FROM RMA " &
                     "         JOIN CIPHERLAB.OMA_FILE ON OMA01 = RMA_ARNO " &
                     "          JOIN CIPHERLAB.OFA_FILE ON OMA67 = OFA01 " &
                     "         JOIN CIPHERLAB.OCC_FILE ON OCC01 = OMA03 " &
                     "   WHERE 1=1 "
            strSQL = strSQL & sCondition
            dt = oQuery.ExecuteDT(strSQL)
            Common.TransferDataTable(dt, dtInvoice)
        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtInvoice
    End Function

    ''' <summary>
    ''' 取得 Spare BOM by Part's No 報表
    ''' </summary>
    ''' <param name="PartsNo">Part's No</param>
    ''' <param name="RepairBOM_No">取得 BOM 維修中心代碼</param>
    ''' <param name="RequestDate">日期</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySpare_PartsNo(ByVal PartsNo As String, ByVal RepairBOM_No As String, ByVal RequestDate As String) As ReportDTO.Rpt_BOMDataTable
        Dim i As Integer
        Dim j As Integer = 0
        Dim sCondition_Lower As String = ""
        Dim sCondition_Upper As String = ""
        Dim sSQL As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New Rpt_BOMDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            Dim OrderBY As String = " ORDER BY bmb_file.bmb01 asc, bmb_file.bmb03 asc"



            'bmb01: 上階料件
            'bmb03: 下階料件
            'sl_ima02: "Y"-->是否為 維修料件
            'Substitute1: "1"-->替代
            'Substitute2: "2"-->取代
            ' "AND sl_ima_file.sl_ima02='Y'" & _
            '增加料號成本欄位 by Hugh, 並一併修改 ReportDTO.xsd, 增加 RPBOM_MATERIALCOST
            '規格抓imc_file的資料
            sSQL = "select vwAllData.* , imc04 RPBOM_DESC , tc_oae240 EXPORT_MODELNO , sl_ima_file.sl_ima02  from " &
                        "( SELECT DISTINCT level, bmb_file.bmb01, bmb_file.bmb03/*, vwREPAIRBOM.RPBOM_DESC*/, RPBOM_MATERIALCOST " &
                         " FROM bmb_file" &
                         " LEFT OUTER join " &
                         " (" &
                         "   SELECT RPBOM_PARTNO/*, RPBOM_DESC*/, RPBOM_MATERIALCOST FROM REPAIRBOM WHERE RPBOM_COMPNO='" & RepairBOM_No & "'" &
                         "   GROUP BY RPBOM_PARTNO, RPBOM_DESC, RPBOM_MATERIALCOST " &
                         " ) vwREPAIRBOM ON bmb_file.bmb03 = vwREPAIRBOM.RPBOM_PARTNO" &
                         " WHERE to_char(bmb_file.bmb04,'YYYYMMDD') <=to_char(TO_DATE('" & RequestDate & "','YYYY/MM/DD'),'YYYYMMDD')" &
                         " AND (to_char(bmb_file.bmb05,'YYYYMMDD') >= to_char(TO_DATE('" & RequestDate & "','YYYY/MM/DD'),'YYYYMMDD') or bmb_file.bmb05 is null)" &
                         " START WITH bmb_file.bmb01 IN ('" & PartsNo & "')" &
                         " CONNECT BY PRIOR bmb_file.bmb03 = bmb_file.bmb01) vwAllData " &
                    " INNER join sl_ima_file ON vwAllData.bmb03 = sl_ima_file.sl_ima01" &
                    " left join cipherlab.tc_oae_file ON tc_oae010 = vwAllData.bmb03" &
                    " LEFT JOIN CIPHERLAB.IMC_FILE ON imc01 = bmb03" &
                    " where imc02 = '0000' and imc03 = 1 and sl_ima02='Y'" &
                    " order by bmb03, bmb01"


            '            sSQL = "select * from " & _
            '            "( SELECT DISTINCT level, EXPORT.EXPORT_MODELNO, bmb_file.bmb01, bmb_file.bmb03, vwREPAIRBOM.RPBOM_DESC, RPBOM_MATERIALCOST, " & _
            '             " sl_ima_file.sl_ima02" & _
            '             " FROM bmb_file" & _
            '             " LEFT OUTER join sl_ima_file ON bmb_file.bmb03 = sl_ima_file.sl_ima01" & _
            '             " LEFT OUTER join EXPORT on bmb_file.bmb03 = EXPORT.EXPORT_PARTNO" & _
            '            " LEFT OUTER join " & _
            '           " (" & _
            '          "   SELECT RPBOM_PARTNO, RPBOM_DESC, RPBOM_MATERIALCOST FROM REPAIRBOM WHERE RPBOM_COMPNO='" & RepairBOM_No & "'" & _
            '         "   GROUP BY RPBOM_PARTNO, RPBOM_DESC, RPBOM_MATERIALCOST " & _
            '        " ) vwREPAIRBOM ON bmb_file.bmb03 = vwREPAIRBOM.RPBOM_PARTNO" & _
            '       " WHERE to_char(bmb_file.bmb04,'YYYYMMDD') <=to_char(TO_DATE('" & RequestDate & "','YYYY/MM/DD'),'YYYYMMDD')" & _
            '      " AND (to_char(bmb_file.bmb05,'YYYYMMDD') >= to_char(TO_DATE('" & RequestDate & "','YYYY/MM/DD'),'YYYYMMDD') or bmb_file.bmb05 is null)" & _
            '     " START WITH bmb_file.bmb01 IN ('" & PartsNo & "')" & _
            '    " CONNECT BY PRIOR bmb_file.bmb03 = bmb_file.bmb01) vwAllData " & _
            '" where sl_ima02='Y'" & _
            '" order by bmb03, bmb01"

            _SQLCollection.Add(sSQL)
            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

            For i = 0 To dtReport.Rows.Count - 1
                If sCondition_Lower.Trim <> "" Then
                    sCondition_Lower = sCondition_Lower & ","
                End If
                If sCondition_Upper.Trim <> "" Then
                    sCondition_Upper = sCondition_Upper & ","
                End If

                sCondition_Lower = sCondition_Lower & "'" & dtReport.Rows(i)("bmb03").ToString().Trim() & "'"
                sCondition_Upper = sCondition_Upper & "'" & dtReport.Rows(i)("bmb01").ToString().Trim() & "'"

                dtReport.Rows(i)("imgfile") = dtReport.Rows(i)("bmb03").ToString().Trim() & ".jpg"
            Next
            If sCondition_Lower.Trim = "" Then
                sCondition_Lower = "''"
            End If
            If sCondition_Upper.Trim = "" Then
                sCondition_Upper = "''"
            End If


            'bmd02: "1"-->替代, "2"-->取代 
            Dim dvReport As DataView = dtReport.DefaultView
            If sCondition_Lower.Trim <> "" Then
                'sSQL = "SELECT DISTINCT bmd01, bmd02 , bmd08 FROM bmd_file " & _
                ' " WHERE bmd01 IN (" & sCondition & ")" & _
                ' "  AND bmd02 = '2' and (bmd08 = 'ALL' or bmd08 ='" & PartsNo.Trim() & "') "

                'sSQL = "SELECT DISTINCT bmd01, bmd02 , bmd08 FROM bmd_file " & _
                ' " WHERE bmd01 IN (" & sCondition_Lower & ")" & _
                ' "  AND (bmd08 = 'ALL' or bmd08 IN ('" & PartsNo.Trim() & "'))"

                sSQL = "SELECT DISTINCT bmd01, bmd02 , bmd08 FROM bmd_file " &
                 " WHERE bmd01 IN (" & sCondition_Lower & ")" &
                 "  AND (bmd08 = 'ALL' or bmd08 IN (" & sCondition_Upper & "))"
                _SQLCollection.Add(sSQL)

                dtTmp = oQuery.ExecuteDT(sSQL)

                For i = 0 To dtTmp.Rows.Count - 1
                    Dim bmd01 As String = dtTmp.Rows(i)("bmd01").ToString().Trim()
                    Dim bmd02 As String = dtTmp.Rows(i)("bmd02").ToString().Trim()

                    dvReport.RowFilter = "bmb03='" & bmd01 & "'"
                    For j = 0 To dvReport.Count - 1
                        If bmd02.Trim() = "1" Then
                            dvReport(j)("Substitute1") = "1"
                        End If
                        If bmd02.Trim() = "2" Then
                            dvReport(j)("Substitute2") = "2"
                        End If
                    Next
                Next
            End If
            dvReport.RowFilter = ""


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Spare BOM by 原型機種 報表
    ''' </summary>
    ''' <param name="PrimalSN">原型機種</param>
    ''' <param name="RepairBOM_No">BOM 維修中心代碼</param>
    ''' <param name="PartsNo">ByRef Part's No</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySpare_PrimalSN(ByVal PrimalSN As String, ByVal RepairBOM_No As String, ByRef PartsNo As String) As ReportDTO.Rpt_BOMDataTable
        Dim i As Integer = 0

        Dim sCondition As String = ""
        Dim sSQL As String = ""
        Dim iTop As String = ""

        Dim dtTmp_Parent As New DataTable
        Dim dtReport As New Rpt_BOMDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)


        oConn.Open()
        Try
            sCondition = sCondition & " AND rownum<2"

            oQuery.addWHERE("EXPAR_PRIMALSN", ":EXPAR_PRIMALSN", PrimalSN.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND (EXPAR_PRIMALSN=:EXPAR_PRIMALSN)"


            sSQL = "SELECT EXPAR_PARTSNO, EXPAR_PRIMALSN FROM EXPORT_PRIMALSN" &
                    " where 1=1 " & sCondition

            _SQLCollection.Add(PrimalSN.Trim())
            _SQLCollection.Add(sSQL)
            dtTmp_Parent = oQuery.ExecuteDT(sSQL)


            If dtTmp_Parent.Rows.Count > 0 Then
                PartsNo = dtTmp_Parent.Rows(0)("EXPAR_PARTSNO").ToString().Trim()
                Dim tDate As Date = Date.Now
                dtReport = QuerySpare_PartsNo(PartsNo, RepairBOM_No, tDate.ToShortDateString())
            End If


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Spare BOM by Product SerialNo 報表
    ''' </summary>
    ''' <param name="Product_SerialNo">Product Serial No</param>
    ''' <param name="RepairBOM_No">取得 BOM 維修中心代碼</param>
    ''' <param name="PartsNo">ByRef Part's No</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySpare_ProductSerialNo(ByVal Product_SerialNo As String, ByVal RepairBOM_No As String, ByRef PartsNo As String) As ReportDTO.Rpt_BOMDataTable
        Dim i As Integer = 0

        Dim sCondition1 As String = ""
        Dim sCondition2 As String = ""
        Dim sSQL As String = ""
        Dim iTop As String = ""

        Dim dtTmp_Parent As New DataTable
        Dim dtReport As New Rpt_BOMDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)


        oConn.Open()
        Try
            Dim OrderBY As String = " ORDER BY EXPORT_SERIALNO asc , EXPORT_SHIPPING_TIME desc"

            sCondition1 = sCondition1 & " AND rownum<2"
            sCondition2 = sCondition2 & " AND rownum<2"

            oQuery.addWHERE("EXPAR_D_SN", ":EXPAR_D_SN", Product_SerialNo.Trim(), OracleType.VarChar)
            sCondition1 = sCondition1 & " AND (EXPAR_D_SN=:EXPAR_D_SN)"




            sSQL = "SELECT EXPORT_PARTNO, EXPORT_SERIALNO, EXPAR_D_ITEMNO, EXPAR_D_SN, " &
                    " EXPORT_SHIPPING_TIME, EXPORT_WARRANTY_DATE, EXPORT_CUSTOMERNAME" &
                    " FROM EXPORT_PARTS INNER JOIN EXPORT" &
                    " ON EXPORT_PARTS.EXPAR_M_SN = EXPORT.EXPORT_SERIALNO" &
                    " where (EXPAR_EFFECT=1 or EXPAR_EFFECT is null)" & sCondition1 & OrderBY


            'sCondition1 = sCondition1 & " AND (to_char(EXPORT_SHIPPING_TIME,'YYYYMMDD') = to_char(TO_DATE('" & ShippingDate & "','YYYY/MM/DD'),'YYYYMMDD'))"


            '先以 detail 序號為 Key 查詢 master 的序號再 join 到出貨資料show master SN 和最後一筆出貨日期。
            '若查不到再以 master sn 為 key 查詢出最後一筆出貨日期。
            sSQL = "SELECT EXPORT_PARTNO, EXPORT_SERIALNO, EXPAR_D_ITEMNO, EXPAR_D_SN, " &
                    " EXPORT_SHIPPING_TIME, EXPORT_WARRANTY_DATE, EXPORT_CUSTOMERNAME" &
                    " FROM EXPORT_PARTS INNER JOIN EXPORT" &
                    " ON EXPORT_PARTS.EXPAR_M_SN = EXPORT.EXPORT_SERIALNO" &
                    " where (EXPAR_EFFECT=1 or EXPAR_EFFECT is null)" & sCondition1 & OrderBY
            dtTmp_Parent = oQuery.ExecuteDT(sSQL)

            If dtTmp_Parent.Rows.Count = 0 Then
                oQuery.Dispose()
                oQuery.addWHERE("EXPAR_M_SN", ":EXPAR_M_SN", Product_SerialNo.Trim(), OracleType.VarChar)
                sCondition2 = sCondition2 & " AND (EXPAR_M_SN=:EXPAR_M_SN)"
                'sCondition2 = sCondition2 & " AND (to_char(EXPORT_SHIPPING_TIME,'YYYYMMDD') = to_char(TO_DATE('" & ShippingDate & "','YYYY/MM/DD'),'YYYYMMDD'))"

                sSQL = "SELECT DISTINCT EXPORT_PARTNO, EXPORT_SERIALNO, EXPAR_D_ITEMNO, EXPAR_D_SN, " &
                        " EXPORT_SHIPPING_TIME, EXPORT_WARRANTY_DATE, EXPORT_CUSTOMERNAME" &
                        " FROM EXPORT_PARTS INNER JOIN EXPORT" &
                        " ON EXPORT_PARTS.EXPAR_M_SN = EXPORT.EXPORT_SERIALNO" &
                        " where (EXPAR_EFFECT=1 or EXPAR_EFFECT is null) " & sCondition2 & OrderBY
                dtTmp_Parent = oQuery.ExecuteDT(sSQL)
            End If


            If dtTmp_Parent.Rows.Count > 0 Then
                PartsNo = dtTmp_Parent.Rows(0)("EXPORT_PARTNO").ToString().Trim()
                Dim ShippingDate As Date = dtTmp_Parent.Rows(0)("EXPORT_SHIPPING_TIME")
                dtReport = QuerySpare_PartsNo(PartsNo, RepairBOM_No, ShippingDate.ToShortDateString())

                'Dim sPartNo As String = ""
                'For i = 0 To dtTmp_Parent.Rows.Count - 1
                '    If sPartNo.Trim <> "" Then
                '        sPartNo = sPartNo & ","
                '    End If
                '    sPartNo = sPartNo & dtTmp_Parent.Rows(i)("EXPORT_PARTNO").ToString().Trim()
                'Next

                'If sPartNo.Trim <> "" Then
                '    sPartNo = sPartNo.Replace(",", "','")
                '    dtReport = QuerySpare_PartsNo(sPartNo, RepairBOM_No, ShippingDate)
                'End If
            End If



        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 取替代 關係
    ''' </summary>
    ''' <param name="iSubstitute">"1"-->替代, "2"-->取代</param>
    ''' <param name="upper_PartsNo">上階料件</param>
    ''' <param name="lower_PartsNo">下階料件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySubstitute(ByVal iSubstitute As Integer, ByVal upper_PartsNo As String, ByVal lower_PartsNo As String, ByVal RepairBOM_No As String) As ReportDTO.Rpt_SubstituteDataTable
        Dim sSQL As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New Rpt_SubstituteDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            Dim OrderBY As String = " ORDER BY bmd04 asc"

            'bmd01: 下階料件
            'bmd02: "1"-->替代, "2"-->取代
            'bmd04: 取替代料件
            'bmd08: 上階料件
            sSQL = "SELECT bmd01, bmd02, bmd04, bmd08, vwREPAIRBOM.RPBOM_DESC" &
                " FROM bmd_file " &
                " LEFT OUTER join " &
                " (" &
                "   SELECT RPBOM_PARTNO, RPBOM_DESC FROM REPAIRBOM WHERE RPBOM_COMPNO='" & RepairBOM_No & "'" &
                "   GROUP BY RPBOM_PARTNO, RPBOM_DESC" &
                " ) vwREPAIRBOM ON bmd_file.bmd01 = vwREPAIRBOM.RPBOM_PARTNO" &
                " WHERE bmd_file.bmd02 = '" & iSubstitute & "' and bmd_file.bmd01='" & lower_PartsNo.Trim() & "'" &
                " AND (bmd08 = 'ALL' or bmd08 ='" & upper_PartsNo.Trim() & "') " & OrderBY

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Repair Frequency Query
    ''' </summary>
    ''' <param name="SerialNo">Serial No</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_FrequencyDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryFrequency(ByVal SerialNo As String, ByVal fdate As String, ByVal edate As String,
        Optional ByVal OrderBY As String = "") As vwRpt_FrequencyDataTable

        Dim i As Integer = 0
        Dim sSQL As String
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New vwRpt_FrequencyDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMAD_SERIALNO asc, RMAD_MODELNO asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If SerialNo.ToString().Trim() <> "" Then
                SerialNo = "%" & SerialNo.Trim() & "%"
                oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_SERIALNO) like :RMAD_SERIALNO"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            sSQL = "SELECT RMAD_SERIALNO  SerialNo , RMAD_MODELNO ModelNo, Count(*) iCount " &
                    " FROM rma , rmadetail" &
                    " WHERE rma.RMA_NO = RMAD_RMANO and RMA_MARK=0 and RMA_STATUS in (10,20,90)" &
                    "   and rmad_mark=0 and RMAD_STATUS>0 and RMAD_RECEVSTATUS <> 2" & sCondition &
                    " group by RMAD_SERIALNO, RMAD_MODELNO" & OrderBY

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    Public Function RMA_Detail_Report(ByVal RmaNO As String, ByVal CuNo As String, ByVal CuName As String, ByVal ModelNo As String, ByVal Warranty As String,
        ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal SDfdate As String, ByVal SDedate As String, ByVal RepairCenter As String,
        ByVal iLanguageID As String, Optional ByVal OrderBY As String = "") As VWRPT_RMADETAILDataTable

        Dim i As Integer
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New VWRPT_RMADETAILDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_NO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY


            '維修中心
            Dim sCondition_Repair As String = ""
            sCondition = sCondition & " AND ("
            Dim arrRepair() As String = RepairCenter.Split(",")
            For i = 0 To arrRepair.Length - 1
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                If sCondition_Repair.Trim <> "" Then
                    sCondition_Repair = sCondition_Repair & " OR "
                End If
                sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
            Next
            sCondition = sCondition & sCondition_Repair & ")"


            If RmaNO.Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RmaNO", RmaNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RmaNO"
            End If

            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            If ModelNo.ToString().Trim() <> "" Then
                ModelNo = "%" & ModelNo.Trim() & "%"
                oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
            End If

            'Warranty
            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            If Warranty.Trim() <> "-1" Then
                oQuery.addWHERE("RMAD_ISWARRANTY", ":RMAD_ISWARRANTY", Warranty, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_ISWARRANTY=:RMAD_ISWARRANTY"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO"
                sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
            End If

            '狀態
            If Status.Trim() <> "-1" Then
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
            End If


            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            If SDfdate.Trim <> "" And SDedate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(SDfdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(SDedate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("ShippingDate", ":ShippingDate1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("ShippingDate", ":ShippingDate2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (ShippingDate >=:ShippingDate1 AND ShippingDate <=:ShippingDate2)"
            End If

            'Dim sSQL As String = "SELECT * FROM vwRpt_RMADetail " & _
            '        " WHERE 1=1 " & sCondition & OrderBY
            '" (SELECT DISTINCT EXPORT_PARTNO FROM EXPORT WHERE EXPORT_SERIALNO = RMADETAIL.RMAD_SERIALNO) EXPORT_PARTNO " &

            sSQL = "SELECT DISTINCT RMA_NO, RMA_CUNO, RMA_ACCOUNTID, RMA_APPLICANT, RMA_COMPNO, RMA_STATUS, RMA_CSTMP, RMA_LUSTMP, " &
            " CU_NO, CU_NAME, CU_SALESID, CU_ASSISTANTID, COMP_NAME,COUNTRY_NAME, " &
            " RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_CUSNAME, RMAD_WARRANTY, RMAD_ISWARRANTY, RMAD_FARFARCNO, RMAD_FARNO, RMAD_STATUS, " &
            " RMAD_PRODUCTDESC, RMAD_CSTMP, RMAD_LUSTMP, RMAD_RECVAD, RMAD_RECVADNAME, RMAD_RECVDATE, RMAD_RECEVSTATUS, " &
            " NVL(FARC_2.FAR_REASON, FARC_1.FAR_REASON) FAR_REASON, " &
            " RMARQ_IMPROPERUSAGE, " &
            " RMAR_COMPNO, RMAR_DUTYNO, RMAR_FARCNO, RMAR_FARNO, RMAR_PROBLEMDESC, " &
            " RMAR_LABORHOUR, RMAR_LABORPRICE, RMAR_LABORCOST, RMAR_MATERIALCOST, RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_CURRENCYRATE, " &
            " RMAR_ASSIGELABORCOST, RMAR_ASSIGEMATERIALCOST, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_ASSIGECURRENCYRATE, " &
            " RMAR_REPAIR_ISFILL, RMAR_REPAIRDESC, " &
            " RMARQ_COMPNO, RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, RMARQ_ASSIGECURRENCYRATE, " &
            " RMASQ_LABORCOST, RMASQ_MATERIALCOST, RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, " &
            " RMASQ_SALEAD, RMASQ_SALEADNAME, RMASQ_SALEDATE, RMASQ_CLIENTAD, RMASQ_CLIENTADNAME, RMASQ_CLIENTDATE, RMASQ_CLIENTCONFIRM, " &
            " RMASM_PACKINGNO, RMASM_LUAD, RMASM_LUADNAME, NoticedDate, " &
            " RMARSD_LABORCOST, RMARSD_MATERIALCOST, RMARSD_QUOTE, RMARSD_CURRENCYCODE, RMARSD_CURRENCYRATE, " &
            " RMASH_SHIPPINGNO, RMASH_LUAD, RMASH_LUADNAME, ShippingDate, " &
            " RMAD_RECVAD Received_AD, RMAD_RECVADNAME Received_Name , to_char(RMAD_RECVDATE,'yyyy/mm/dd HH24:MI:SS') Received_Date, " &
            " RMAR_REPAIRAD Repaired_AD, RMAR_REPAIRADNAME Repaired_Name, to_char(RMAR_REPAIRDATE,'yyyy/mm/dd HH24:MI:SS') Repaired_Date, " &
            " RMARQ_LUAD RepairQuoted_AD, RMARQ_LUADNAME RepairQuoted_Name, to_char(RMARQ_LUSTMP,'yyyy/mm/dd HH24:MI:SS') RepairQuoted_Date," &
            " RMASQ_SALEAD Sales_AD, RMASQ_SALEADNAME Sales_Name, to_char(RMASQ_SALEDATE,'yyyy/mm/dd HH24:MI:SS') Sales_Date, " &
            " RMASQ_CLIENTAD Client_AD, RMASQ_CLIENTADNAME Client_Name, to_char(RMASQ_CLIENTDATE,'yyyy/mm/dd HH24:MI:SS') Client_Date, RMASQ_CLIENTCONFIRM Client_Confirm,RMAD_ISCW, " &
            " (SELECT MIN(EXPORT_SHIPPING_TIME) FROM EXPORT WHERE EXPORT_SERIALNO = RMADETAIL.RMAD_SERIALNO) EXPORT_SHIPPING_DATE,RMACHARGE_QUOTED.RMACQ_DISCOUNT,RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT " &
            "  " &
            " FROM RMA " &
            "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO " &
            "  INNER JOIN COUNTRY ON COUNTRY.COUNTRY_ID = CUSTOMER.CU_COUNTRYID " &
            "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO " &
            "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 AND RMAD_RECEVSTATUS<>2 " &
            "  LEFT OUTER JOIN RMAREPAIR ON RMAREPAIR.RMAR_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN RMASALE_QUOTED ON RMASALE_QUOTED.RMASQ_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN " &
            " ( " &
            "  SELECT FAR_FARCNO,FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS  " &
            "  WHERE FAILUREREASONSCLASS.FARC_NO = FAILUREREASONS.FAR_FARCNO AND FAR_DFLNO = FARC_DFLNO " &
            "  AND FARC_DFLNO='" & iLanguageID.ToString().Trim() & "'" &
            "  ) FARC_1 ON RMADETAIL.RMAD_FARNO = FARC_1.FAR_NO AND RMADETAIL.RMAD_FARFARCNO = FARC_1.FAR_FARCNO " &
            " LEFT OUTER JOIN " &
            " ( " &
            "  SELECT FAR_FARCNO,FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS  " &
            "  WHERE FAILUREREASONSCLASS.FARC_NO = FAILUREREASONS.FAR_FARCNO AND FAR_DFLNO = FARC_DFLNO " &
            "  AND FARC_DFLNO='" & iLanguageID.ToString().Trim() & "'" &
            "  ) FARC_2 ON RMAREPAIR.RMAR_FARNO = FARC_2.FAR_NO AND RMADETAIL.RMAD_FARFARCNO = FARC_2.FAR_FARCNO " &
            "  LEFT OUTER JOIN " &
            " (" &
            "  select RMASM_PACKINGNO, RMASM_LUAD, RMASM_LUADNAME, RMASM_LUSTMP as NoticedDate,  " &
            "   RMASMD_RMANO ,  RMASMD_RMADID,  " &
            "   RMARSD_LABORCOST , RMARSD_MATERIALCOST, RMARSD_QUOTE, RMARSD_CURRENCYCODE, RMARSD_CURRENCYRATE " &
            "  from RMA_SHIPMENT inner join RMA_SHIPMENTDETAIL " &
            "  on RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID " &
            " ) vwSHIPMENT ON vwSHIPMENT.RMASMD_RMADID =  RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN " &
            " ( " &
            " SELECT rmashd_rmano, rmasmd_rmadid, RMASH_SHIPPINGNO, RMASH_LUAD, RMASH_LUADNAME, RMASH_LUSTMP as ShippingDate, RMASHD_RMASMPACKINGNO " &
            " FROM RMA_SHIPMENT " &
            "  inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID " &
            "  inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO " &
            "  inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID " &
            " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA_NO " &
            " LEFT JOIN RMACHARGE_QUOTED ON RMACHARGE_QUOTED.RMACQ_RMANO = RMA.RMA_NO " &
            " LEFT JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID = rmadetail.RMAD_ID  " &
            " WHERE RMA_MARK=0 AND RMAD_STATUS>0 " & sCondition & OrderBY



            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 RMA Detail 報表
    ''' </summary>
    ''' <param name="RmaNO">RMA 單號</param>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="ModelNo">Model No</param>
    ''' <param name="Warranty">是否保固日期內:null.未定(Unidentified), 0.否, 1.是</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="Status">Status</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="RepairCenter">登入者維修中心代碼</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_RMADetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryRMADetail(ByVal RmaNO As String, ByVal CuNo As String, ByVal CuName As String, ByVal ModelNo As String, ByVal Warranty As String,
        ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal SDfdate As String, ByVal SDedate As String, ByVal RepairCenter As String,
        ByVal iLanguageID As String, Optional ByVal OrderBY As String = "") As VWRPT_RMADETAILDataTable

        Dim i As Integer
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New VWRPT_RMADETAILDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_NO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY


            '維修中心
            Dim sCondition_Repair As String = ""
            sCondition = sCondition & " AND ("
            Dim arrRepair() As String = RepairCenter.Split(",")
            For i = 0 To arrRepair.Length - 1
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                If sCondition_Repair.Trim <> "" Then
                    sCondition_Repair = sCondition_Repair & " OR "
                End If
                sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
            Next
            sCondition = sCondition & sCondition_Repair & ")"


            If RmaNO.Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RmaNO", RmaNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RmaNO"
            End If

            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            If ModelNo.ToString().Trim() <> "" Then
                ModelNo = "%" & ModelNo.Trim() & "%"
                oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
            End If

            'Warranty
            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            If Warranty.Trim() <> "-1" Then
                oQuery.addWHERE("RMAD_ISWARRANTY", ":RMAD_ISWARRANTY", Warranty, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_ISWARRANTY=:RMAD_ISWARRANTY"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO"
                sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
            End If

            '狀態
            If Status.Trim() <> "-1" Then
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
            End If


            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            If SDfdate.Trim <> "" And SDedate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(SDfdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(SDedate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("ShippingDate", ":ShippingDate1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("ShippingDate", ":ShippingDate2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (ShippingDate >=:ShippingDate1 AND ShippingDate <=:ShippingDate2)"
            End If

            'Dim sSQL As String = "SELECT * FROM vwRpt_RMADetail " & _
            '        " WHERE 1=1 " & sCondition & OrderBY
            '" (SELECT DISTINCT EXPORT_PARTNO FROM EXPORT WHERE EXPORT_SERIALNO = RMADETAIL.RMAD_SERIALNO) EXPORT_PARTNO " &

            sSQL = "SELECT DISTINCT RMA_NO, RMA_CUNO, RMA_ACCOUNTID, RMA_APPLICANT, RMA_COMPNO, RMA_STATUS, RMA_CSTMP, RMA_LUSTMP, " &
            " CU_NO, CU_NAME, CU_SALESID, CU_ASSISTANTID, COMP_NAME,COUNTRY_NAME, " &
            " RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_CUSNAME, RMAD_WARRANTY, RMAD_ISWARRANTY, RMAD_FARFARCNO, RMAD_FARNO, RMAD_STATUS, " &
            " RMAD_PRODUCTDESC, RMAD_CSTMP, RMAD_LUSTMP, RMAD_RECVAD, RMAD_RECVADNAME, RMAD_RECVDATE, RMAD_RECEVSTATUS, " &
            " NVL(FARC_2.FAR_REASON, FARC_1.FAR_REASON) FAR_REASON, " &
            " RMARQ_IMPROPERUSAGE, " &
            " RMAR_COMPNO, RMAR_DUTYNO, RMAR_FARCNO, RMAR_FARNO, RMAR_PROBLEMDESC, " &
            " RMAR_LABORHOUR, RMAR_LABORPRICE, RMAR_LABORCOST, RMAR_MATERIALCOST, RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_CURRENCYRATE, " &
            " RMAR_ASSIGELABORCOST, RMAR_ASSIGEMATERIALCOST, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_ASSIGECURRENCYRATE, " &
            " RMAR_REPAIR_ISFILL, RMAR_REPAIRDESC, " &
            " RMARQ_COMPNO, RMARQ_ASSIGLABORCOST, RMARQ_ASSIGMATERIALCOST, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, RMARQ_ASSIGECURRENCYRATE, " &
            " RMASQ_LABORCOST, RMASQ_MATERIALCOST, RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE, " &
            " RMASQ_SALEAD, RMASQ_SALEADNAME, RMASQ_SALEDATE, RMASQ_CLIENTAD, RMASQ_CLIENTADNAME, RMASQ_CLIENTDATE, RMASQ_CLIENTCONFIRM, " &
            " RMASM_PACKINGNO, RMASM_LUAD, RMASM_LUADNAME, NoticedDate, " &
            " RMARSD_LABORCOST, RMARSD_MATERIALCOST, RMARSD_QUOTE, RMARSD_CURRENCYCODE, RMARSD_CURRENCYRATE, " &
            " RMASH_SHIPPINGNO, RMASH_LUAD, RMASH_LUADNAME, ShippingDate, " &
            " RMAD_RECVAD Received_AD, RMAD_RECVADNAME Received_Name , to_char(RMAD_RECVDATE,'yyyy/mm/dd HH24:MI:SS') Received_Date, " &
            " RMAR_REPAIRAD Repaired_AD, RMAR_REPAIRADNAME Repaired_Name, to_char(RMAR_REPAIRDATE,'yyyy/mm/dd HH24:MI:SS') Repaired_Date, " &
            " RMARQ_LUAD RepairQuoted_AD, RMARQ_LUADNAME RepairQuoted_Name, to_char(RMARQ_LUSTMP,'yyyy/mm/dd HH24:MI:SS') RepairQuoted_Date," &
            " RMASQ_SALEAD Sales_AD, RMASQ_SALEADNAME Sales_Name, to_char(RMASQ_SALEDATE,'yyyy/mm/dd HH24:MI:SS') Sales_Date, " &
            " RMASQ_CLIENTAD Client_AD, RMASQ_CLIENTADNAME Client_Name, to_char(RMASQ_CLIENTDATE,'yyyy/mm/dd HH24:MI:SS') Client_Date, RMASQ_CLIENTCONFIRM Client_Confirm,RMAD_ISCW, " &
            " (SELECT MIN(EXPORT_SHIPPING_TIME) FROM EXPORT WHERE EXPORT_SERIALNO = RMADETAIL.RMAD_SERIALNO) EXPORT_SHIPPING_DATE,RMACHARGE_QUOTED.RMACQ_DISCOUNT,RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT, " &
            " EXPORT_PARTNO " &
            " FROM RMA " &
            "  INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO " &
            "  INNER JOIN COUNTRY ON COUNTRY.COUNTRY_ID = CUSTOMER.CU_COUNTRYID " &
            "  INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO " &
            "  INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 AND RMAD_RECEVSTATUS<>2 " &
            "  LEFT OUTER JOIN RMAREPAIR ON RMAREPAIR.RMAR_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN RMASALE_QUOTED ON RMASALE_QUOTED.RMASQ_RMADID = RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN " &
            " ( " &
            "  SELECT FAR_FARCNO,FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS  " &
            "  WHERE FAILUREREASONSCLASS.FARC_NO = FAILUREREASONS.FAR_FARCNO AND FAR_DFLNO = FARC_DFLNO " &
            "  AND FARC_DFLNO='" & iLanguageID.ToString().Trim() & "'" &
            "  ) FARC_1 ON RMADETAIL.RMAD_FARNO = FARC_1.FAR_NO AND RMADETAIL.RMAD_FARFARCNO = FARC_1.FAR_FARCNO " &
            " LEFT OUTER JOIN " &
            " ( " &
            "  SELECT FAR_FARCNO,FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS  " &
            "  WHERE FAILUREREASONSCLASS.FARC_NO = FAILUREREASONS.FAR_FARCNO AND FAR_DFLNO = FARC_DFLNO " &
            "  AND FARC_DFLNO='" & iLanguageID.ToString().Trim() & "'" &
            "  ) FARC_2 ON RMAREPAIR.RMAR_FARNO = FARC_2.FAR_NO AND RMAREPAIR.RMAR_FARCNO = FARC_2.FAR_FARCNO " &
            "  LEFT OUTER JOIN " &
            " (" &
            "  select RMASM_PACKINGNO, RMASM_LUAD, RMASM_LUADNAME, RMASM_LUSTMP as NoticedDate,  " &
            "   RMASMD_RMANO ,  RMASMD_RMADID,  " &
            "   RMARSD_LABORCOST , RMARSD_MATERIALCOST, RMARSD_QUOTE, RMARSD_CURRENCYCODE, RMARSD_CURRENCYRATE " &
            "  from RMA_SHIPMENT inner join RMA_SHIPMENTDETAIL " &
            "  on RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID " &
            " ) vwSHIPMENT ON vwSHIPMENT.RMASMD_RMADID =  RMADetail.RMAD_ID " &
            "  LEFT OUTER JOIN " &
            " ( " &
            " SELECT rmashd_rmano, rmasmd_rmadid, RMASH_SHIPPINGNO, RMASH_LUAD, RMASH_LUADNAME, RMASH_LUSTMP as ShippingDate, RMASHD_RMASMPACKINGNO " &
            " FROM RMA_SHIPMENT " &
            "  inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID " &
            "  inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO " &
            "  inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID " &
            " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA_NO " &
            " LEFT JOIN RMACHARGE_QUOTED ON RMACHARGE_QUOTED.RMACQ_RMANO = RMA.RMA_NO " &
            " LEFT JOIN RMACHARGE_QUOTED_SN ON RMACHARGE_QUOTED_SN.RMACQSN_RMADID = rmadetail.RMAD_ID  " &
            " LEFT JOIN VWEXPORTPARTNO ON EXPORT_SERIALNO = RMAD_SERIALNO " &
            " WHERE RMA_MARK=0 AND RMAD_STATUS>0 " & sCondition & OrderBY



            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 RMA Warranty 報表
    ''' </summary>
    ''' <param name="CustID">CustID</param>
    ''' <param name="Product_SerialNo">Product Serial No</param>
    ''' <returns></returns>
    ''' <remarks>優化SQL語法 by buck modify 20260320</remarks>
    Public Function QueryRMAWarranty(ByVal CustID As String, ByVal Product_SerialNo As String) As Rpt_RMAWarrantyDataTable
        Dim sCondition1 As String = ""
        Dim sCondition2 As String = ""
        Dim sCondition3 As String = ""

        Dim sSQL As String = ""
        Dim iTop As String = ""

        Dim dtTmp_Parent As New DataTable
        Dim dtTmp_Child As New DataTable
        Dim dtReport As New Rpt_RMAWarrantyDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            Dim OrderBY As String = " ORDER BY EXPORT_SERIALNO asc , EXPORT_SHIPPING_TIME desc, EXPORT_CSTMP desc"

            If CustID.Trim <> "" Then
                'sCondition1 = sCondition1 & " AND rownum<2"
                'sCondition2 = sCondition2 & " AND rownum<2"
                sCondition1 = sCondition1 & " AND CU_NO='" & CustID & "'"
                sCondition2 = sCondition2 & " AND CU_NO='" & CustID & "'"
            End If

            oQuery.addWHERE("SERIALNO", ":SERIALNO", Product_SerialNo.Trim(), OracleType.VarChar)
            'sCondition1 = sCondition1 & " AND (EXPAR_D_SN=:EXPAR_D_SN or RPD.RMARED_NSERIALNO = :RMARED_NSERIALNO)"

            '先以 detail 序號為 Key 查詢 master 的序號再 join 到出貨資料show master SN 和最後一筆出貨日期。
            '若查不到再以 master sn 為 key 查詢出最後一筆出貨日期。
            '需求新增:BI保固 By buck Add 20250902 begin
            sSQL = "
                    SELECT DISTINCT 
                        EXPORT_PARTNO, EXPORT_SERIALNO, EXPORT_SHIPPING_TIME, EXPORT_WARRANTY_DATE, 
                        EXPORT_CUSTOMERNAME, '0' AS isDetail, CW_EDATE,
                        NVL(WAR_TYPE,'') AS WAR_TYPE, 
                        NVL(EXPORT_WAR_ID,'') AS EXPORT_WAR_ID,
                        NVL(PROGRAM_TYPE_NAME,'') AS WAR_PROGRAM_TYPE,
                        NVL(ITEM_TYPE_NAME,'') AS WAR_ITEM_TYPE,
                        NVL(PRICE_VER_NAME,'') AS WAR_PRICE_VER, 
                        EXPORT_CSTMP
                    FROM (
                        SELECT ep.*, e.* 
                        FROM EXPORT_PARTS ep
                        INNER JOIN EXPORT e ON ep.EXPAR_M_SN = e.EXPORT_SERIALNO
                        WHERE (ep.EXPAR_EFFECT = 1 OR ep.EXPAR_EFFECT IS NULL)
                          AND ep.EXPAR_D_SN = :SERIALNO

                        UNION
    
                        SELECT ep.*, e.*
                        FROM EXPORT_PARTS ep
                        INNER JOIN EXPORT e ON ep.EXPAR_M_SN = e.EXPORT_SERIALNO
                        INNER JOIN RMAREPAIR_Detail RPD ON ep.EXPAR_D_ITEMNO = RPD.RMARED_NPARTNO 
                                                       AND ep.EXPAR_D_SN = RPD.RMARED_OSERIALNO
                        WHERE (ep.EXPAR_EFFECT = 1 OR ep.EXPAR_EFFECT IS NULL)
                          AND RPD.RMARED_NSERIALNO = :SERIALNO
                    ) t
                    LEFT JOIN CUSTOMER ON t.EXPORT_CUSTNO = CUSTOMER.CU_TIPTOP_ID
                    LEFT JOIN WARRSET a ON a.WAR_ID = t.EXPORT_WAR_ID
                    LEFT JOIN WARRSET_TYPE e_type ON e_type.WARRSET_TYPE = a.WAR_TYPE
                    LEFT JOIN WARRSET_ITEM_TYPE b ON b.WARRSET_TYPE = a.WAR_TYPE AND b.ITEM_TYPE = a.WAR_ITEM_TYPE
                    LEFT JOIN WARRSET_PROGRAM_TYPE c ON c.WARRSET_TYPE = a.WAR_TYPE AND c.PROGRAM_TYPE = a.WAR_PROGRAM_TYPE
                    LEFT JOIN WARRSET_PRICE_VER d ON d.WARRSET_TYPE = a.WAR_TYPE AND d.PRICE_VER = a.WAR_PRICE_VER
                    WHERE 1=1
                    " & sCondition1 & OrderBY
            '需求新增:BI保固 By buck Add 20250902 end
            dtTmp_Parent = oQuery.ExecuteDT(sSQL)

            If dtTmp_Parent.Rows.Count = 0 Then
                oQuery.Dispose()
                oQuery.addWHERE("EXPAR_M_SN", ":EXPAR_M_SN", Product_SerialNo.Trim(), OracleType.VarChar)
                sCondition2 = sCondition2 & " AND EXPAR_M_SN=:EXPAR_M_SN"
                '需求新增:BI保固 By buck Add 20250902 begin
                sSQL = "
                        SELECT DISTINCT 
                            e.EXPORT_PARTNO, 
                            e.EXPORT_SERIALNO, 
                            e.EXPORT_SHIPPING_TIME, 
                            e.EXPORT_WARRANTY_DATE, 
                            e.EXPORT_CUSTOMERNAME, 
                            '0' AS isDetail, 
                            e.CW_EDATE,
                            NVL(a.WAR_TYPE, '') AS WAR_TYPE, 
                            NVL(e.EXPORT_WAR_ID, '') AS EXPORT_WAR_ID,
                            NVL(wpt.PROGRAM_TYPE_NAME, '') AS WAR_PROGRAM_TYPE,
                            NVL(wit.ITEM_TYPE_NAME, '') AS WAR_ITEM_TYPE,
                            NVL(wpv.PRICE_VER_NAME, '') AS WAR_PRICE_VER, 
                            e.EXPORT_CSTMP
                        FROM EXPORT_PARTS ep
                        INNER JOIN EXPORT e 
                            ON ep.EXPAR_M_SN = e.EXPORT_SERIALNO
                        LEFT JOIN CUSTOMER c 
                            ON e.EXPORT_CUSTNO = c.CU_TIPTOP_ID
                        LEFT JOIN WARRSET a 
                            ON a.WAR_ID = e.EXPORT_WAR_ID
                        LEFT JOIN WARRSET_TYPE ws 
                            ON ws.WARRSET_TYPE = a.WAR_TYPE
                        LEFT JOIN WARRSET_ITEM_TYPE wit 
                            ON wit.WARRSET_TYPE = a.WAR_TYPE AND wit.ITEM_TYPE = a.WAR_ITEM_TYPE
                        LEFT JOIN WARRSET_PROGRAM_TYPE wpt 
                            ON wpt.WARRSET_TYPE = a.WAR_TYPE AND wpt.PROGRAM_TYPE = a.WAR_PROGRAM_TYPE
                        LEFT JOIN WARRSET_PRICE_VER wpv 
                            ON wpv.WARRSET_TYPE = a.WAR_TYPE AND wpv.PRICE_VER = a.WAR_PRICE_VER
                        WHERE (ep.EXPAR_EFFECT = 1 OR ep.EXPAR_EFFECT IS NULL)
                        " & sCondition2 & OrderBY
                '需求新增:BI保固 By buck Add 20250902 end
                dtTmp_Parent = oQuery.ExecuteDT(sSQL)
            End If

            '直接抓出貨資料 
            If dtTmp_Parent.Rows.Count = 0 Then
                oQuery.Dispose()

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", Product_SerialNo.Trim(), OracleType.VarChar)
                sCondition3 = " EXPORT_SERIALNO=:EXPORT_SERIALNO "

                If CustID.Trim <> "" Then
                    sCondition3 = sCondition3 & " AND CU_NO='" & CustID & "'"
                End If
                '需求新增:BI保固 By buck Add 20250902 begin
                sSQL = "
                        SELECT DISTINCT 
                            e.EXPORT_PARTNO, 
                            e.EXPORT_CSTMP, 
                            e.EXPORT_SERIALNO, 
                            e.EXPORT_SHIPPING_TIME, 
                            e.EXPORT_WARRANTY_DATE, 
                            e.EXPORT_CUSTOMERNAME, 
                            '0' AS isDetail, 
                            e.CW_EDATE,
                            NVL(wt.WARRSET_TYPE_NAME, '') AS WAR_TYPE,
                            NVL(e.EXPORT_WAR_ID, '') AS EXPORT_WAR_ID,
                            NVL(wpt.PROGRAM_TYPE_NAME, '') AS WAR_PROGRAM_TYPE,
                            NVL(wit.ITEM_TYPE_NAME, '') AS WAR_ITEM_TYPE,
                            NVL(wpv.PRICE_VER_NAME, '') AS WAR_PRICE_VER
                        FROM EXPORT e
                        LEFT JOIN CUSTOMER c 
                            ON e.EXPORT_CUSTNO = c.CU_TIPTOP_ID
                        LEFT JOIN WARRSET a 
                            ON a.WAR_ID = e.EXPORT_WAR_ID
                        LEFT JOIN WARRSET_TYPE wt 
                            ON wt.WARRSET_TYPE = a.WAR_TYPE
                        LEFT JOIN WARRSET_ITEM_TYPE wit 
                            ON wit.WARRSET_TYPE = a.WAR_TYPE AND wit.ITEM_TYPE = a.WAR_ITEM_TYPE
                        LEFT JOIN WARRSET_PROGRAM_TYPE wpt 
                            ON wpt.WARRSET_TYPE = a.WAR_TYPE AND wpt.PROGRAM_TYPE = a.WAR_PROGRAM_TYPE
                        LEFT JOIN WARRSET_PRICE_VER wpv 
                            ON wpv.WARRSET_TYPE = a.WAR_TYPE AND wpv.PRICE_VER = a.WAR_PRICE_VER
                        WHERE
                        " & sCondition3 & OrderBY
                '需求新增:BI保固 By buck Add 20250902 end
                dtTmp_Parent = oQuery.ExecuteDT(sSQL)
            End If

            If dtTmp_Parent.Rows.Count > 0 Then
                Common.TransferDataTable(dtTmp_Parent, dtReport)

                Dim DetailTitle As String = Replace(_oLanguage.getText("Report", "160", ctlLanguage.eumType.Tag), "'", "''")

                Dim EXPORT_SERIALNO As String = dtTmp_Parent.Rows(0)("EXPORT_SERIALNO").ToString().Trim()
                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO.Trim(), OracleType.VarChar)
                'BI保固需求修正 by buck modify 20260114 begin
                sSQL = "
                        WITH Titel AS (
                            SELECT '" & DetailTitle & "' EXPORT_PARTNO, '' EXPORT_SERIALNO, '' EXPORT_SHIPPING_TIME, '' EXPORT_WARRANTY_DATE, '' EXPORT_CUSTOMERNAME , '2' isDetail,  '' WAR_TYPE,'' EXPORT_WAR_ID,'' WAR_PROGRAM_TYPE,'' WAR_ITEM_TYPE,'' WAR_PRICE_VER  
                            from dual 
                        ), PARTS AS (
                            SELECT EXPAR_D_ITEMNO EXPORT_PARTNO, EXPAR_D_SN EXPORT_SERIALNO, '' EXPORT_SHIPPING_TIME, '' EXPORT_WARRANTY_DATE, '' EXPORT_CUSTOMERNAME , '1' isDetail, '' WAR_TYPE,'' EXPORT_WAR_ID,'' WAR_PROGRAM_TYPE,'' WAR_ITEM_TYPE,'' WAR_PRICE_VER
                            FROM EXPORT_PARTS
                            where (EXPAR_EFFECT=1 or EXPAR_EFFECT is null) 
                            AND EXPAR_M_SN = (    
                                SELECT EXPAR_M_SN 
                                FROM EXPORT_PARTS PARTS           
                                LEFT JOIN RMAREPAIR_Detail RPD ON PARTS.EXPAR_D_ITEMNO = RPD.RMARED_NPARTNO AND PARTS.EXPAR_D_SN = RPD.RMARED_OSERIALNO
                                WHERE (PARTS.EXPAR_M_SN = :EXPORT_SERIALNO OR PARTS.EXPAR_D_SN = :EXPORT_SERIALNO OR RPD.RMARED_NSERIALNO = :EXPORT_SERIALNO)
                                GROUP BY EXPAR_M_SN    
                            )
                        ), PARTS_REPAIRDetail AS (
                            SELECT PARTS.EXPORT_PARTNO,PARTS.EXPORT_SERIALNO,PARTS.EXPORT_SHIPPING_TIME, PARTS.EXPORT_WARRANTY_DATE,
                            CASE
                                WHEN RPD.RMARED_OSERIALNO IS NOT NULL THEN '被取代'
                                ELSE ''
                            END AS EXPORT_CUSTOMERNAME, PARTS.isDetail, PARTS.WAR_TYPE, PARTS.EXPORT_WAR_ID, PARTS.WAR_PROGRAM_TYPE, PARTS.WAR_ITEM_TYPE, PARTS.WAR_PRICE_VER--, EXPAR_M_SN  
                            FROM PARTS
                            LEFT JOIN RMAREPAIR_Detail RPD ON PARTS.EXPORT_PARTNO = RPD.RMARED_NPARTNO AND PARTS.EXPORT_SERIALNO = RPD.RMARED_OSERIALNO 
                            ORDER BY (CASE WHEN RPD.RMARED_OSERIALNO IS NOT NULL THEN '被取代' ELSE '' END) desc  
                        ), PARTS_RMAD AS (
                            SELECT PARTS.EXPORT_PARTNO,RPD.RMARED_NSERIALNO AS EXPORT_SERIALNO, PARTS.EXPORT_SHIPPING_TIME, PARTS.EXPORT_WARRANTY_DATE,   
                            D.RMAD_RMANO AS EXPORT_CUSTOMERNAME, PARTS.isDetail, PARTS.WAR_TYPE, PARTS.EXPORT_WAR_ID, PARTS.WAR_PROGRAM_TYPE, PARTS.WAR_ITEM_TYPE, PARTS.WAR_PRICE_VER--, EXPAR_M_SN  
                            FROM PARTS
                            JOIN RMAREPAIR_Detail RPD ON PARTS.EXPORT_PARTNO = RPD.RMARED_NPARTNO AND PARTS.EXPORT_SERIALNO = RPD.RMARED_OSERIALNO
                            LEFT JOIN RMADETAIL D ON RPD.RMARED_RMADID = D.RMAD_ID
                        )

                        SELECT *
                        FROM Titel
                        UNION ALL 
                        SELECT *
                        FROM PARTS_REPAIRDetail
                        UNION ALL
                        SELECT *
                        FROM PARTS_RMAD
                    "
                
                'BI保固需求修正 by buck modify 20260114 end
                dtTmp_Child = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dtTmp_Child, dtReport)
            End If


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Failure Reason Report 報表
    ''' </summary>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="ModelNo">Model No</param>
    ''' <param name="Warranty">是否保固日期內:null.未定(Unidentified), 0.否, 1.是</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="Status">Status</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_RMADetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryFailureReason(ByVal CuNo As String, ByVal ModelNo As String, ByVal Warranty As String,
        ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String,
        Optional ByVal OrderBY As String = "") As vwRtp_FailureDataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New vwRtp_FailureDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMAD_MODELNO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_CUNO=:RMA_CUNO"
            End If

            If ModelNo.ToString().Trim() <> "" Then
                ModelNo = "%" & ModelNo.Trim() & "%"
                oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
            End If

            'Warranty
            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            If Warranty.Trim() <> "-1" Then
                oQuery.addWHERE("RMAD_ISWARRANTY", ":RMAD_ISWARRANTY", Warranty, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_ISWARRANTY=:RMAD_ISWARRANTY"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_COMPNO=:RMA_COMPNO"
            End If

            '狀態
            If Status.Trim() <> "-1" Then
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
            End If


            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            sSQL = "SELECT RMAD_MODELNO , MODELNAME, " &
                    " NVL(vwFailure_2.FAR_NO, vwFailure_1.FAR_NO) FAR_NO, " &
                    " NVL(vwFailure_2.FAR_REASON, vwFailure_1.FAR_REASON) FAR_REASON," &
                    " count(*) iCount" &
                    " FROM RMA" &
                    " INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 AND RMAD_RECEVSTATUS<>2 AND RMAD_STATUS>0 " &
                    " INNER JOIN MODEL ON MODEL.MODELNO = rmadetail.RMAD_MODELNO" &
                    " LEFT OUTER JOIN " &
                    " (" &
                    "   SELECT FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS " &
                    "   WHERE FARC_DFLNO=1 AND FAILUREREASONSCLASS.FARC_NO = FAR_FARCNO" &
                    " ) vwFailure_1 ON RMADETAIL.RMAD_FARNO = vwFailure_1.FAR_NO" &
                    " LEFT OUTER JOIN RMAREPAIR ON RMAREPAIR.RMAR_RMADID = RMADetail.RMAD_ID" &
                    " LEFT OUTER JOIN " &
                    " (" &
                    "   SELECT FAR_NO, FAR_REASON FROM FAILUREREASONSCLASS, FAILUREREASONS " &
                    "   WHERE FARC_DFLNO=1 AND FAILUREREASONSCLASS.FARC_NO = FAR_FARCNO" &
                    " ) vwFailure_2 ON RMAREPAIR.RMAR_FARNO = vwFailure_2.FAR_NO" &
                    " WHERE RMA_MARK=0 " & sCondition &
                    " group by RMAD_MODELNO, MODELNAME," &
                    " NVL(vwFailure_2.FAR_NO, vwFailure_1.FAR_NO), " &
                    " NVL(vwFailure_2.FAR_REASON, vwFailure_1.FAR_REASON)" &
                    OrderBY

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Customers Request 報表
    ''' </summary>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="Warranty">是否保固日期內:null.未定(Unidentified), 0.否, 1.是</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_CustomersRequestDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryCustomersRequest(ByVal CuNo As String, ByVal CuName As String, ByVal Warranty As String,
        ByVal Repair As String, ByVal fdate As String, ByVal edate As String,
        Optional ByVal OrderBY As String = "") As vwRpt_CustomersRequestDataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New vwRpt_CustomersRequestDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_CUNO asc, TO_CHAR(RMA_CSTMP,'yyyy-MM') asc"
            End If
            OrderBY = " ORDER BY " & OrderBY




            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            'Warranty
            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            If Warranty.Trim() <> "-1" Then
                oQuery.addWHERE("RMAD_ISWARRANTY", ":RMAD_ISWARRANTY", Warranty, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_ISWARRANTY=:RMAD_ISWARRANTY"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                'oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO)"
                'sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If

            sSQL = "SELECT RMA_CUNO, CU_NAME, TO_CHAR(RMA_CSTMP,'yyyy-MM') RequestYM," &
                " count(rmadetail.RMAD_ID) RequestQTY, count(vwClosed.RMAD_ID) ClosedQTY, count(vwCanceled.RMAD_ID) CanceledQTY" &
                " FROM RMA" &
                " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" &
                " INNER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" &
                " INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 AND RMAD_STATUS>0" &
                " LEFT OUTER JOIN " &
                " (" &
                "   SELECT RMAD_ID, RMAD_RMANO FROM rmadetail WHERE RMAD_MARK=0 AND RMAD_STATUS=90" &
                " ) vwClosed ON vwClosed.RMAD_RMANO = RMA.RMA_NO AND vwClosed.RMAD_ID = rmadetail.RMAD_ID" &
                " LEFT OUTER JOIN " &
                " (" &
                "   SELECT RMAD_ID, RMAD_RMANO FROM rmadetail WHERE RMAD_MARK=0 AND RMAD_STATUS=91" &
                " ) vwCanceled ON vwCanceled.RMAD_RMANO = RMA.RMA_NO AND vwCanceled.RMAD_ID = rmadetail.RMAD_ID" &
                " WHERE RMA_MARK=0 " & sCondition &
                " GROUP BY RMA_CUNO, CU_NAME, TO_CHAR(RMA_CSTMP,'yyyy-MM')" & OrderBY

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 Repair Center Monthly By Repairer 維修人員 報表
    ''' </summary>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="Warranty">是否保固日期內:null.未定(Unidentified), 0.否, 1.是</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_CustomersRequestDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryRepairCenterMonthly(ByVal CuNo As String, ByVal CuName As String, ByVal Warranty As String,
        ByVal Repair As String, ByVal fdate As String, ByVal edate As String,
        Optional ByVal OrderBY As String = "") As vwRpt_RCMonthlyByRepairerDataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim dtReport As New vwRpt_RCMonthlyByRepairerDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_COMPNO asc, TO_CHAR(RMA_CSTMP,'yyyy-MM') asc, RMAR_REPAIRAD asc"
            End If
            OrderBY = " ORDER BY " & OrderBY


            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            'Warranty
            '是否保固日期內:null.未定(Unidentified), 0.否, 1.是
            If Warranty.Trim() <> "-1" Then
                oQuery.addWHERE("RMAD_ISWARRANTY", ":RMAD_ISWARRANTY", Warranty, OracleType.Int16)
                sCondition = sCondition & " AND RMAD_ISWARRANTY=:RMAD_ISWARRANTY"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                'oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO)"
                'sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
            End If


            sSQL = " SELECT RMA_COMPNO, COMP_NAME,  TO_CHAR(RMA_CSTMP,'yyyy-MM') RequestYM, RMAR_REPAIRAD, RMAR_REPAIRADNAME," &
                " count(vwRequest.RMAD_ID) RequestQTY," &
                " count(vwCanceled.RMAD_ID) CanceledQTY, " &
                " count(vwReceived.RMAD_ID) ReceivedQTY, " &
                " count(vwQuoted.RMAD_ID) QuotedQTY ," &
                " count(rmadetail.RMAD_ID) RepairedQTY," &
                " count(rmadetail.RMAD_ID) / (count(vwRequest.RMAD_ID)  -  count(vwCanceled.RMAD_ID)) PerformedQTY, " &
                " count(vwWarranty.RMAD_ID) WarrantyQTY, " &
                " count(vwWarrantyOut.RMAD_ID) WarrantyOutQTY" &
                " FROM RMA " &
                "   INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" &
                "   INNER JOIN COMPANY ON RMA_COMPNO = COMP_NO" &
                "   INNER JOIN rmadetail ON rmadetail.RMAD_RMANO = RMA.RMA_NO and RMAD_MARK=0 AND RMAD_STATUS>0" &
                "   INNER JOIN RMAREPAIR ON RMAR_RMADID = RMAD_ID  AND RMAR_REPAIRAD is not null" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "        SELECT RMAD_ID, RMAD_RMANO FROM rmadetail  WHERE RMAD_MARK=0 AND RMAD_STATUS>0 " &
                "   ) vwRequest ON vwRequest.RMAD_RMANO = RMA.RMA_NO AND vwRequest.RMAD_ID = rmadetail.RMAD_ID" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "        SELECT RMAD_ID, RMAD_RMANO FROM rmadetail  WHERE RMAD_MARK=0 AND RMAD_STATUS=91" &
                "    ) vwCanceled ON vwCanceled.RMAD_RMANO = RMA.RMA_NO AND vwCanceled.RMAD_ID = rmadetail.RMAD_ID" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "       SELECT RMAD_ID, RMAD_RMANO FROM rmadetail WHERE RMAD_MARK=0 AND RMAD_STATUS>0 AND RMAD_RECEVSTATUS=1" &
                "   ) vwReceived ON vwReceived.RMAD_RMANO = RMA.RMA_NO AND vwReceived.RMAD_ID = rmadetail.RMAD_ID" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "       SELECT RMAD_ID, RMAD_RMANO FROM rmadetail , RMASALE_QUOTED" &
                "       WHERE RMAD_ID = RMASQ_RMADID   AND RMAD_MARK=0 AND RMAD_STATUS>30 AND RMAD_STATUS<>91 " &
                "   ) vwQuoted ON vwQuoted.RMAD_RMANO = RMA.RMA_NO AND vwQuoted.RMAD_ID = rmadetail.RMAD_ID" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "       SELECT RMAD_ID, RMAD_RMANO FROM rmadetail WHERE RMAD_MARK=0 AND RMAD_STATUS>0 AND RMAD_STATUS<>91 AND RMAD_ISWARRANTY = 1" &
                "   ) vwWarranty ON vwWarranty.RMAD_RMANO = RMA.RMA_NO AND vwWarranty.RMAD_ID = rmadetail.RMAD_ID" &
                "   LEFT OUTER JOIN " &
                "   (" &
                "       SELECT RMAD_ID, RMAD_RMANO FROM rmadetail WHERE RMAD_MARK=0 AND RMAD_STATUS>0 AND RMAD_STATUS<>91 AND RMAD_ISWARRANTY <>1" &
                "   ) vwWarrantyOut ON vwWarrantyOut.RMAD_RMANO = RMA.RMA_NO AND vwWarrantyOut.RMAD_ID = rmadetail.RMAD_ID" &
                " WHERE RMA_MARK=0 " & sCondition &
                " GROUP BY RMA_COMPNO, COMP_NAME,  TO_CHAR(RMA_CSTMP,'yyyy-MM') , RMAR_REPAIRAD, RMAR_REPAIRADNAME " & OrderBY

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 取得 對帳單 報表
    ''' </summary>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="RMANo">RmaNo</param>
    ''' <param name="ModelNo">Model No</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="RepairCenter">登入者維修中心代碼</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_RMADetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryRMABill(ByVal CuNo As String, ByVal CuName As String, ByVal RMANo As String, ByVal ModelNo As String,
        ByVal Repair As String, ByVal fdate As String, ByVal edate As String, ByVal RepairCenter As String,
        Optional ByVal OrderBY As String = "") As Rpt_BillDataDataTable

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        Dim sCondition1 As String = ""
        Dim sCondition2 As String = ""
        Dim sCondition3 As String = ""

        Dim dtTmpA As New DataTable
        Dim dtTmpB As New DataTable
        Dim dtReport As New Rpt_BillDataDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_NO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY


            '維修中心
            Dim sCondition_Repair As String = ""
            sCondition = sCondition & " AND ("
            Dim arrRepair() As String = RepairCenter.Split(",")
            For i = 0 To arrRepair.Length - 1
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                If sCondition_Repair.Trim <> "" Then
                    sCondition_Repair = sCondition_Repair & " OR "
                End If
                sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString
            Next
            sCondition = sCondition & sCondition_Repair & ")"


            If RMANo.Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RMA_NO"
            End If

            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            If ModelNo.ToString().Trim() <> "" Then
                Dim sModelNo As String = "%" & ModelNo.Trim() & "%"
                oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", sModelNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                sCondition1 = sCondition1 & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_COMPNO=:RMA_COMPNO"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim RMASH_CSTMP1 = Convert.ToDateTime(fdate)
                Dim RMASH_CSTMP2 = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP1", RMASH_CSTMP1, OracleType.DateTime)
                oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP2", RMASH_CSTMP2, OracleType.DateTime)
                sCondition1 = sCondition1 & " AND (RMASH_CSTMP >=:RMASH_CSTMP1 AND RMASH_CSTMP <=:RMASH_CSTMP2)"
            End If

            'sCondition3 = sCondition3 & " AND (RMAD_SERIALNO like 'EF%' or RMAD_SERIALNO like 'EK%') "

            sSQL = "SELECT RMA_NO, RMA_ID, cu_no, cu_name, RMA_CSTMP RequestedData" &
                " , COMP_NO, COMP_NAME, nvl(vwRMADetail.ReceivedCount,0) ReceivedCount, nvl(vwSHIPPING.shippingCount,0) shippingCount" &
                " ,NULL ShippedDate, bill_date" &
                " FROM rma INNER JOIN CUSTOMER " &
                " ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=90) " &
                " INNER JOIN rmadetail ON RMA.RMA_NO = rmadetail.rmad_rmano AND rmadetail.RMAD_RECEVSTATUS=1" &
                " INNER JOIN COMPANY ON COMPANY.COMP_NO = RMA.RMA_COMPNO" &
                " INNER JOIN " &
                " (" &
                "   SELECT rmad_rmano, Count(*) ReceivedCount FROM rmadetail " &
                "   WHERE rmad_mark=0 " & sCondition3 &
                "   GROUP BY rmad_rmano" &
                " ) vwRMADetail ON RMA.RMA_NO = vwRMADetail.rmad_rmano" &
                "" &
                " INNER JOIN " &
                " (" &
                "   SELECT RMASMD_RMANO, count(*) shippingCount" &
                "   FROM RMA_SHIPPING " &
                "   INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID" &
                "   INNER JOIN RMA_SHIPMENT ON RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO" &
                "   INNER JOIN RMA_SHIPMENTDETAIL ON  RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID  " &
                "       AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO" &
                "   INNER JOIN rmadetail ON RMASMD_RMANO = rmadetail.rmad_rmano AND rmadetail.RMAD_RECEVSTATUS=1 " &
                "       AND RMASMD_RMADID = RMAD_ID" &
                "   WHERE 1=1" & sCondition1 & sCondition3 &
                "   GROUP BY RMASMD_RMANO" &
                " ) vwSHIPPING ON vwSHIPPING.RMASMD_RMANO = rmadetail.rmad_rmano" &
                "" &
                " LEFT OUTER JOIN RMA_BILLDATA ON RMA_BILLDATA.bill_rmano = RMA.RMA_NO " &
                "" &
                " WHERE 1=1" & sCondition & sCondition3 &
                "" &
                " GROUP BY RMA_NO, RMA_ID, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, COMP_NO, COMP_NAME" &
                "   , nvl(vwRMADetail.ReceivedCount,0)  , nvl(vwSHIPPING.shippingCount,0)  , bill_date" & OrderBY

            dtTmpA = oQuery.ExecuteDT(sSQL)


            For i = 0 To dtTmpA.Rows.Count - 1
                Dim sRMANO As String = dtTmpA.Rows(i)("RMA_NO").ToString().Trim()

                If ModelNo.ToString().Trim() <> "" Then
                    Dim sModelNo As String = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", sModelNo.ToLower(), OracleType.VarChar)
                    sCondition2 = sCondition2 & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If
                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMASH_CSTMP1 = Convert.ToDateTime(fdate)
                    Dim RMASH_CSTMP2 = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP1", RMASH_CSTMP1, OracleType.DateTime)
                    oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP2", RMASH_CSTMP2, OracleType.DateTime)
                    sCondition2 = sCondition2 & " AND (RMASH_CSTMP >=:RMASH_CSTMP1 AND RMASH_CSTMP <=:RMASH_CSTMP2)"
                End If


                sSQL = " SELECT RMASMD_RMANO,  RMASH_CSTMP ShippedDate" &
                    " FROM RMA_SHIPPING " &
                    " INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID" &
                    " INNER JOIN RMA_SHIPMENT ON RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO" &
                    " INNER JOIN RMA_SHIPMENTDETAIL ON  RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID" &
                    "   AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO" &
                    " INNER JOIN rmadetail ON RMASMD_RMANO = rmadetail.rmad_rmano AND rmadetail.RMAD_RECEVSTATUS=1" &
                    "   AND RMASMD_RMADID = RMAD_ID" &
                    " WHERE RMASMD_RMANO ='" & sRMANO.Trim() & "'" & sCondition2 &
                    "  GROUP BY RMASMD_RMANO,  RMASH_CSTMP" &
                    " ORDER BY RMASH_CSTMP desc"

                dtTmpB = oQuery.ExecuteDT(sSQL)

                Dim sShippedDate As String = ""
                For j = 0 To dtTmpB.Rows.Count - 1
                    Dim ShippedDate As String = dtTmpB.Rows(j)("ShippedDate").ToString().Trim()
                    If ShippedDate.Trim <> "" Then
                        If sShippedDate.Trim <> "" Then
                            sShippedDate = sShippedDate & vbCrLf
                        End If
                        sShippedDate = sShippedDate & Convert.ToDateTime(ShippedDate).ToShortDateString()
                    End If
                    dtTmpA.Rows(i)("ShippedDate") = sShippedDate
                Next
            Next

            Common.TransferDataTable(dtTmpA, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    ''' <summary>
    ''' 給 對帳單報表用
    ''' </summary>
    ''' <param name="CuNo">客戶編碼</param>
    ''' <param name="CuName">客戶名稱</param>
    ''' <param name="RMANo">RmaNo</param>
    ''' <param name="ModelNo">Model No</param>
    ''' <param name="Repair">維修中心代碼</param>
    ''' <param name="fdate">開始日期</param>
    ''' <param name="edate">結束日期</param>
    ''' <param name="RepairCenter">登入者維修中心代碼</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>傳回 vwRpt_RMADetailDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryRMAShippingByBill(ByVal CuNo As String, ByVal CuName As String, ByVal RMANo As String, ByVal ModelNo As String,
        ByVal Repair As String, ByVal fdate As String, ByVal edate As String, ByVal RepairCenter As String,
        Optional ByVal OrderBY As String = "") As ReportDTO.vwRpt_ShippingDetailDataTable

        Dim i As Integer = 0
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        Dim sCondition3 As String = ""

        Dim dtTmpA As New DataTable
        Dim dtReport As New vwRpt_ShippingDetailDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_NO, RMAD_ID desc"
            End If
            OrderBY = " ORDER BY " & OrderBY


            '維修中心
            Dim sCondition_Repair As String = ""
            sCondition = sCondition & " AND ("
            Dim arrRepair() As String = RepairCenter.Split(",")
            For i = 0 To arrRepair.Length - 1
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                If sCondition_Repair.Trim <> "" Then
                    sCondition_Repair = sCondition_Repair & " OR "
                End If
                sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString
            Next
            sCondition = sCondition & sCondition_Repair & ")"


            If RMANo.Trim() <> "" Then
                Dim sCondition_RMANo As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRMANo() As String = RMANo.Split(",")
                For i = 0 To arrRMANo.Length - 1
                    oQuery.addWHERE("RMA_NO", ":RMA_NO" & i.ToString, arrRMANo(i).Trim(), OracleType.VarChar)

                    If sCondition_RMANo.Trim <> "" Then
                        sCondition_RMANo = sCondition_RMANo & " OR "
                    End If
                    sCondition_RMANo = sCondition_RMANo & " RMA_NO =:RMA_NO" & i.ToString
                Next
                sCondition = sCondition & sCondition_RMANo & ")"
            End If


            If CuNo.Trim() <> "" Then
                oQuery.addWHERE("CU_No", ":CU_No", CuNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND CU_No=:CU_No"
            End If

            If CuName.ToString().Trim() <> "" Then
                CuName = "%" & CuName.Trim() & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
            End If

            If ModelNo.ToString().Trim() <> "" Then
                Dim sModelNo As String = "%" & ModelNo.Trim() & "%"
                oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", sModelNo.ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
            End If

            '維修點
            If Repair.Trim() <> "-1" Then
                oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_COMPNO=:RMA_COMPNO"
            End If

            If fdate.Trim <> "" And edate.Trim <> "" Then
                Dim ShippedDate1 = Convert.ToDateTime(fdate)
                Dim ShippedDate2 = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                oQuery.addWHERE("ShippedDate", ":ShippedDate1", ShippedDate1, OracleType.DateTime)
                oQuery.addWHERE("ShippedDate", ":ShippedDate2", ShippedDate2, OracleType.DateTime)
                sCondition = sCondition & " AND (ShippedDate >=:ShippedDate1 AND ShippedDate <=:ShippedDate2)"
            End If

            sCondition3 = sCondition3 & " AND (RMAD_SERIALNO like 'EF%' or RMAD_SERIALNO like 'EK%') "

            '搭被匯出格式1 的方式
            'sSQL = " SELECT RMA_ID, RMA_NO, RMAD_ID, cu_no, cu_name , RMAD_SERIALNO, RMAD_MODELNO, RMAD_ISWARRANTY, RMAD_WARRANTY" & _
            '    " , vwSHIPPING.ShippedDate, RMARED_NPARTNO" & _
            '    " , RMAR_REPAIRAD, RMAR_REPAIRADNAME, RMAR_REPAIRDATE" & _
            '    " FROM rma " & _
            '    " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=90) " & _
            '    " INNER JOIN rmadetail ON RMA.RMA_NO = rmadetail.rmad_rmano AND rmadetail.RMAD_RECEVSTATUS=1" & _
            '    " INNER JOIN RMAREPAIR ON rmadetail.rmad_id = RMAR_RMADID AND RMAR_REPAIRAD is not null" & _
            '    " INNER JOIN" & _
            '    "" & _
            '    " (" & _
            '    "   SELECT RMASMD_RMANO, RMASMD_RMADID, RMASH_CSTMP ShippedDate" & _
            '    "   FROM RMA_SHIPPING " & _
            '    "   INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID" & _
            '    "   INNER JOIN RMA_SHIPMENT ON RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO" & _
            '    "   INNER JOIN RMA_SHIPMENTDETAIL ON  RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID  " & _
            '    "       AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO" & _
            '    " ) vwSHIPPING ON vwSHIPPING.RMASMD_RMANO = rmadetail.rmad_rmano AND vwSHIPPING.RMASMD_RMADID = rmadetail.RMAD_ID" & _
            '    "" & _
            '    " LEFT OUTER JOIN RMAREPAIR_DETAIL on rmadetail.rmad_id = RMAREPAIR_DETAIL.RMARED_RMADID AND RMARED_MARK=0" & _
            '    " WHERE 1=1" & sCondition & sCondition3 & OrderBY


            sSQL = " SELECT RMA_ID, RMA_NO, RMAD_ID, cu_no, cu_name , RMAD_SERIALNO, RMAD_MODELNO, RMAD_ISWARRANTY, RMAD_WARRANTY" &
                " , vwSHIPPING.ShippedDate" &
                " , RMAR_REPAIRAD, RMAR_REPAIRADNAME, RMAR_REPAIRDATE" &
                " FROM rma " &
                " INNER JOIN CUSTOMER  ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=90) " &
                " INNER JOIN rmadetail ON RMA.RMA_NO = rmadetail.rmad_rmano AND rmadetail.RMAD_RECEVSTATUS=1" &
                " INNER JOIN RMAREPAIR ON rmadetail.rmad_id = RMAR_RMADID AND RMAR_REPAIRAD is not null" &
                " INNER JOIN" &
                "" &
                " (" &
                "   SELECT RMASMD_RMANO, RMASMD_RMADID, RMASH_CSTMP ShippedDate" &
                "   FROM RMA_SHIPPING " &
                "   INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID" &
                "   INNER JOIN RMA_SHIPMENT ON RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO" &
                "   INNER JOIN RMA_SHIPMENTDETAIL ON  RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID  " &
                "       AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO" &
                " ) vwSHIPPING ON vwSHIPPING.RMASMD_RMANO = rmadetail.rmad_rmano AND vwSHIPPING.RMASMD_RMADID = rmadetail.RMAD_ID" &
                " WHERE 1=1" & sCondition & sCondition3 & OrderBY

            dtTmpA = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmpA, dtReport)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtReport
    End Function

    Public Function qryRPTInovice(ByVal rmash_shippingno As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String) As RmaDTO.RPTINVOICEDataTable
        Dim i As Integer = 0
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        Dim dtRptINVOICE As New RmaDTO.RPTINVOICEDataTable
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        dtCompany = oCompany.QueryByPrimaryKey(sRepairCenter)
        Dim sCOMP_ADMIN As String = dtCompany.Rows(0)("COMP_ADMIN").ToString().Trim()

        oConn.Open()
        Try

            '150323 by fairy
            '維修中心
            'Dim sCondition_RMANO As String = ""
            'Dim arrRMANO() As String = sRMANO.Split(",")
            'For i = 0 To arrRMANO.Length - 1
            '    oQuery.addWHERE("rmad_rmano", ":rmad_rmano" & i.ToString, arrRMANO(i).Trim(), OracleType.VarChar)
            '    If sCondition_RMANO.Trim <> "" Then
            '        sCondition_RMANO = sCondition_RMANO & " OR "
            '    End If
            '    sCondition_RMANO = sCondition_RMANO & " rmad_rmano =:rmad_rmano" & i.ToString
            'Next
            'sCondition = sCondition & "(" & sCondition_RMANO & ")"

            'sSQL = sSQL & "select rmad_seq,rmad_rmano,rmad_serialno,export_partno,'1' as quantity,rmarq_quote"
            'sSQL = sSQL & ",occ01 as cust_id"
            'sSQL = sSQL & ",occ18 as company"
            'sSQL = sSQL & ",occ29 as attn"
            'sSQL = sSQL & ",occ261 as tel"
            'sSQL = sSQL & ",occ271 as fax "
            'sSQL = sSQL & ",occ45 as payment_id"
            'sSQL = sSQL & ",b.oag02 as Payment_Term "
            'sSQL = sSQL & ",occ44 as price_term_id"
            'sSQL = sSQL & ",c.oah02 as price_term"
            'sSQL = sSQL & ",occ231 as ship_1"
            'sSQL = sSQL & ",occ232 as ship_2"
            'sSQL = sSQL & ",occ233 as ship_3"
            'sSQL = sSQL & ",occ241 as Invoice_1"
            'sSQL = sSQL & ",occ242 as Invoice_2"
            'sSQL = sSQL & ",occ243 as Invoice_3"
            'sSQL = sSQL & ",occ47 as shipping_type_id"
            'sSQL = sSQL & ",d.ged02 as shipping_type"
            'sSQL = sSQL & ",sysdate as Shipping_on"
            'sSQL = sSQL & ",occ48 as from_id"
            'sSQL = sSQL & ",e.oac02 as fromx"
            'sSQL = sSQL & ",occ49 as to_id"
            'sSQL = sSQL & ",f.oac02 as tox"
            'sSQL = sSQL & ",g.comp_address as from_of_fixcenter"

            'sSQL = sSQL & " from rma z"
            'sSQL = sSQL & " left join rmadetail m on z.rma_no=m.rmad_rmano"
            'sSQL = sSQL & " left join export n on m.rmad_serialno=n.export_serialno"
            'sSQL = sSQL & " left join RMAREPAIR_QUOTED o on m.rmad_id=o.RMARQ_RMADID"
            'sSQL = sSQL & " left join cipherlab.occ_file a on z.rma_cuno=a.occ02"
            'sSQL = sSQL & " left join cipherlab.oag_file b on a.occ45=B.OAG01"
            'sSQL = sSQL & " left join cipherlab.oah_file c  on a.occ44=c.oah01"
            'sSQL = sSQL & " left join cipherlab.ged_file d on a.occ47=D.GED01"
            'sSQL = sSQL & " left join cipherlab.oac_file e on a.occ48=e.oac01"
            'sSQL = sSQL & " left join cipherlab.oac_file f on a.occ49=f.oac01"
            'sSQL = sSQL & " left join company g on z.RMA_COMPNO=g.comp_no"
            'sSQL = sSQL & " where " & sCondition
            'sSQL = sSQL & " order by m.rmad_seq "

            'sSQL = sSQL & " select * from RPTINVOICE"

            oQuery.addWHERE("rmash_shippingno", ":rmash_shippingno", rmash_shippingno, OracleType.VarChar)
            sCondition = sCondition & " rmash_shippingno=:rmash_shippingno"

            '150625 by fairy
            'sSQL = sSQL & "select ofb03 as rmad_seq,substr(ofb06,1,15) as rmad_rmano,substr(ofb06,17,9) as rmad_serialno,'' as export_partno,'1' as quantity,ofb14 as rmarq_quote"
            'sSQL = sSQL & ",ofa01 as rmash_invno"      '150324 add by fairy
            'sSQL = sSQL & ",ofa03 as cust_id"
            'sSQL = sSQL & ",occ18 as company"
            'sSQL = sSQL & ",occ29 as attn"
            'sSQL = sSQL & ",occ261 as tel"
            'sSQL = sSQL & ",occ271 as fax "
            'sSQL = sSQL & ",occ45 as payment_id"
            'sSQL = sSQL & ",oag02 as Payment_Term "
            'sSQL = sSQL & ",ofa31 as price_term_id"
            'sSQL = sSQL & ",oah02 as price_term"
            'sSQL = sSQL & ",occ231 as ship_1"
            'sSQL = sSQL & ",occ232 as ship_2"
            'sSQL = sSQL & ",occ233 as ship_3"
            'sSQL = sSQL & ",occ241 as Invoice_1"
            'sSQL = sSQL & ",occ242 as Invoice_2"
            'sSQL = sSQL & ",occ243 as Invoice_3"
            'sSQL = sSQL & ",occ47 as shipping_type_id"
            'sSQL = sSQL & ",ged02 as shipping_type"
            'sSQL = sSQL & ",sysdate as Shipping_on"
            'sSQL = sSQL & ",occ48 as from_id"
            'sSQL = sSQL & ",a.oac02 as fromx"
            'sSQL = sSQL & ",occ49 as to_id"
            'sSQL = sSQL & ",b.oac02 as tox"
            'sSQL = sSQL & ",comp_address as from_of_fixcenter"
            'sSQL = sSQL & " from rma_shipping"
            'sSQL = sSQL & " INNER JOIN company on comp_no = RMASH_COMPNO"
            'sSQL = sSQL & " INNER JOIN cipherlab.ofa_file on ofa01=RMASH_INVNO"
            'sSQL = sSQL & " INNER JOIN cipherlab.ofb_file on ofb01=ofa01"
            'sSQL = sSQL & " INNER JOIN cipherlab.occ_file on occ01=ofa03 "
            'sSQL = sSQL & " left join cipherlab.oag_file on ofa32=OAG01"
            'sSQL = sSQL & " left join cipherlab.oah_file on ofa31=oah01"
            'sSQL = sSQL & " left join cipherlab.ged_file on occ47=GED01"
            'sSQL = sSQL & " left join cipherlab.oac_file a on occ48=a.oac01"
            'sSQL = sSQL & " left join cipherlab.oac_file b on occ49=b.oac01"
            'sSQL = sSQL & " where " & sCondition
            'sSQL = sSQL & " order by ofb03"

            sSQL = sSQL & "Select ofb03 as rmad_seq "
            sSQL = sSQL & ",RMA_NO as rmad_rmano "
            sSQL = sSQL & ",'' as  rmad_serialno "
            sSQL = sSQL & ",'' as export_partno "
            sSQL = sSQL & ",(Select COUNT(*) From RMA_ShippingDetail Where RMA_ShippingDetail.RMASHD_RMANO=RMA_NO) as quantity "
            sSQL = sSQL & ",ofb14 as rmarq_quote "
            sSQL = sSQL & ",ofa01 as rmash_invno"
            'sSQL = sSQL & ",ofa03 as cust_id "
            'sSQL = sSQL & ",occ18 as company "
            'sSQL = sSQL & ",occ29 as attn "
            'sSQL = sSQL & ",occ261 as tel "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN c.CU_NO ELSE ofa03 END as cust_id  "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN c.CU_NAME ELSE cast(occ18 as nvarchar2(200)) END as company "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN RMA.RMA_APPLICANT ELSE cast(occ29 as nvarchar2(50)) END AS attn "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN RMA.RMA_TEL ELSE occ261 END  as tel "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', c.CU_NO, ofa03) as cust_id  "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', c.CU_NAME, cast(occ18 as nvarchar2(200))) as company "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', RMA.RMA_APPLICANT, cast(occ29 as nvarchar2(50))) AS attn "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', RMA.RMA_TEL, occ261)  as tel "
            sSQL = sSQL & ",occ271 as fax "
            sSQL = sSQL & ",occ45 as payment_id "
            sSQL = sSQL & ",oag02 as Payment_Term "
            sSQL = sSQL & ",ofa31 as price_term_id "
            sSQL = sSQL & ",oah02 as price_term "
            'sSQL = sSQL & ",occ231 as ship_1 "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN RMA.RMA_ADDRESS ELSE cast(occ231 as nvarchar2(200)) END as ship_1 "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', RMA.RMA_ADDRESS, cast(occ231 as nvarchar2(200))) as ship_1 "
            sSQL = sSQL & ",occ232 as ship_2 "
            sSQL = sSQL & ",occ233 as ship_3 "
            'sSQL = sSQL & ",occ241 as Invoice_1 "
            'sSQL = sSQL & ",CASE WHEN ofa03='X0091' THEN RMA.RMA_ADDRESS ELSE cast(occ241 as nvarchar2(200)) END as Invoice_1 "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', RMA.RMA_ADDRESS, cast(occ241 as nvarchar2(200))) as Invoice_1 "
            sSQL = sSQL & ",occ242 as Invoice_2 "
            sSQL = sSQL & ",occ243 as Invoice_3 "
            sSQL = sSQL & ",occ47 as shipping_type_id "
            sSQL = sSQL & ",ged02 as shipping_type "
            sSQL = sSQL & ",sysdate as Shipping_on "
            sSQL = sSQL & ",occ48 as from_id "
            'sSQL = sSQL & ",a.oac02 as fromx "
            'sSQL = sSQL & ",occ49 as to_id "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', 'TAIWAN R.O.C.', a.oac02) as fromx "
            sSQL = sSQL & ",occ49 as to_id "
            sSQL = sSQL & ",DECODE(ofa03, 'X0091', d.COUNTRY_NAME, b.oac02) as tox "
            sSQL = sSQL & ",comp_address as from_of_fixcenter "

            If sCOMP_ADMIN.ToUpper() = "CHINA" Then
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & "Inner Join Ciphersh.ofa_file On RMA.RMA_INVNO=ofa01 "
                sSQL = sSQL & "Inner Join Ciphersh.ofb_file On ofb01=ofa01 "
                sSQL = sSQL & "Inner Join Ciphersh.occ_file On ofa03=occ01 "
                sSQL = sSQL & "Inner Join COMPANY On RMA_COMPNO=COMP_NO "
                sSQL = sSQL & "Left join Ciphersh.oag_file on ofa32=oag01 "
                sSQL = sSQL & "Left join Ciphersh.oah_file on ofa31=oah01 "
                sSQL = sSQL & "Left join Ciphersh.ged_file on occ47=GED01 "
                sSQL = sSQL & "Left join Ciphersh.oac_file a on occ48=a.oac01 "
                sSQL = sSQL & "Left join Ciphersh.oac_file b on occ49=b.oac01 "
                sSQL = sSQL & "LEFT JOIN customer c ON c.CU_NO =RMA.RMA_CUNO AND c.CU_TIPTOP_ID = ofa03 "
                sSQL = sSQL & "JOIN country d ON d.COUNTRY_ID = c.CU_COUNTRYID "
            ElseIf sCOMP_ADMIN.ToUpper() = "MPLUS" Then
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & "Inner Join CIPHERBOND.ofa_file On RMA.RMA_INVNO=ofa01 "
                sSQL = sSQL & "Inner Join CIPHERBOND.ofb_file On ofb01=ofa01 "
                sSQL = sSQL & "Inner Join CIPHERBOND.occ_file On ofa03=occ01 "
                sSQL = sSQL & "Inner Join COMPANY On RMA_COMPNO=COMP_NO "
                sSQL = sSQL & "Left join CIPHERBOND.oag_file on ofa32=oag01 "
                sSQL = sSQL & "Left join CIPHERBOND.oah_file on ofa31=oah01 "
                sSQL = sSQL & "Left join CIPHERBOND.ged_file on occ47=GED01 "
                sSQL = sSQL & "Left join CIPHERBOND.oac_file a on occ48=a.oac01 "
                sSQL = sSQL & "Left join CIPHERBOND.oac_file b on occ49=b.oac01 "
                sSQL = sSQL & "LEFT JOIN customer c ON c.CU_NO =RMA.RMA_CUNO AND c.CU_TIPTOP_ID = ofa03 "
                sSQL = sSQL & "JOIN country d ON d.COUNTRY_ID = c.CU_COUNTRYID "
            Else
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & "Inner Join Cipherlab.ofa_file On RMA.RMA_INVNO=ofa01 "
                sSQL = sSQL & "Inner Join Cipherlab.ofb_file On ofb01=ofa01 "
                sSQL = sSQL & "Inner Join Cipherlab.occ_file On ofa03=occ01 "
                sSQL = sSQL & "Inner Join COMPANY On RMA_COMPNO=COMP_NO "
                sSQL = sSQL & "Left join Cipherlab.oag_file on ofa32=oag01 "
                sSQL = sSQL & "Left join Cipherlab.oah_file on ofa31=oah01 "
                sSQL = sSQL & "Left join Cipherlab.ged_file on occ47=GED01 "
                sSQL = sSQL & "Left join Cipherlab.oac_file a on occ48=a.oac01 "
                sSQL = sSQL & "Left join Cipherlab.oac_file b on occ49=b.oac01 "
                sSQL = sSQL & "LEFT JOIN customer c ON c.CU_NO =RMA.RMA_CUNO AND c.CU_TIPTOP_ID = ofa03 "
                sSQL = sSQL & "JOIN country d ON d.COUNTRY_ID = c.CU_COUNTRYID "
            End If

            sSQL = sSQL & "Where Exists(Select * From RMA_Shipping,RMA_ShippingDetail "
            sSQL = sSQL & "              Where RMA_Shipping.RMASH_ID=RMA_ShippingDetail.RMASHD_RMASHID "
            sSQL = sSQL & "                And " & sCondition
            sSQL = sSQL & "                And RMA_ShippingDetail.RMASHD_RMANO=RMA.RMA_NO) "

            sSQL = sSQL & " AND RMA.RMA_ARNO='" & sRMA_ARNO.Trim() & "'"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRptINVOICE)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRptINVOICE
    End Function

    Public Function qryRPTAD(ByVal rmash_shippingno As String, ByVal sRepairCenter As String, ByVal sRMA_ARNO As String) As RmaDTO.RPTADDataTable
        Dim i As Integer = 0
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        Dim dtRptAD As New RmaDTO.RPTADDataTable
        Dim oCompany As New ctlCompany
        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        dtCompany = oCompany.QueryByPrimaryKey(sRepairCenter)
        Dim sCOMP_ADMIN As String = dtCompany.Rows(0)("COMP_ADMIN").ToString().Trim()

        oConn.Open()
        Try
            oQuery.addWHERE("rmash_shippingno", ":rmash_shippingno", rmash_shippingno, OracleType.VarChar)
            sCondition = sCondition & " rmash_shippingno=:rmash_shippingno"

            'sSQL = sSQL & "select omb03 as SEQ, rmash_shippingno "
            'sSQL = sSQL & ",oma01 as 帳款編號 "
            'sSQL = sSQL & ",to_char(sysdate,'yyyy/mm/dd') as 帳款日期 "
            'sSQL = sSQL & ",oma03 as 客戶編號 "
            'sSQL = sSQL & ",occ02 as 簡稱 "
            'sSQL = sSQL & ",oma10 as 發票號碼 "
            'sSQL = sSQL & ",to_char(oma09,'yyyy/mm/dd')  as 發票日期 "
            'sSQL = sSQL & ",oma32 as 收款方式 "
            'sSQL = sSQL & ",to_char(oma11,'yyyy/mm/dd') as 應收款日 "
            'sSQL = sSQL & ",to_char(oma12,'yyyy/mm/dd')  as  容許票到期日 "
            'sSQL = sSQL & ",oma23 as 幣別 "
            'sSQL = sSQL & ",oma24 as 立帳匯率 "
            'sSQL = sSQL & ",oma21 as 稅別 "
            'sSQL = sSQL & ",oma211 as 稅率 "
            'sSQL = sSQL & ",decode(oma25,'2','EXP','LOC') as 銷售類別  "
            'sSQL = sSQL & ",oma14 as 帳款人員 "
            'sSQL = sSQL & ",100 as 折扣率"
            'sSQL = sSQL & ",substr(omb06,1,15) as RMA單號 "
            ''sSQL = sSQL & ",rmashd_shipmentno"
            'sSQL = sSQL & ",omb14 as 應收金額 "
            'sSQL = sSQL & ",substr(omb06,17,9) as rmad_serialno "
            '' sSQL = sSQL & "j.export_partno"
            'sSQL = sSQL & ",oma03 as 終端客戶編號"
            'sSQL = sSQL & ",occ02 as 終端客戶名稱"
            'sSQL = sSQL & ",occ241 as 終端客戶地址"
            'sSQL = sSQL & ",occ261 as 終端客戶電話"
            'sSQL = sSQL & ",occ29 as 聯絡人"
            'sSQL = sSQL & ",omb14 as 終端應收 "
            'sSQL = sSQL & " from rma_shipping "
            'sSQL = sSQL & " INNER JOIN cipherlab.oma_file on oma01=RMASH_ARNO  "
            'sSQL = sSQL & " INNER JOIN cipherlab.omb_file on omb01=oma01 "
            'sSQL = sSQL & " INNER JOIN cipherlab.occ_file on occ01=oma03 "
            'sSQL = sSQL & " INNER JOIN customer on CU_TIPTOP_ID = occ01  "
            'sSQL = sSQL & "  WHERE " & sCondition
            'sSQL = sSQL & " order by rmash_shippingno"

            'sSQL = sSQL & " select * from RPTAD"


            sSQL = sSQL & "Select omb03 as SEQ "
            sSQL = sSQL & ",'" & rmash_shippingno.Trim() & "' as rmash_shippingno "
            sSQL = sSQL & ",oma01 as 帳款編號 "
            sSQL = sSQL & ",to_char(sysdate,'yyyy/mm/dd') as 帳款日期 "
            sSQL = sSQL & ",DECODE (CUSTOMER.CU_TIPTOP_ID, 'X0091', ENDUSER.EU_NO, oma03) as 客戶編號 "
            sSQL = sSQL & ",DECODE (CUSTOMER.CU_TIPTOP_ID, 'X0091', ENDUSER.EU_NAME, occ02) as 簡稱 "
            sSQL = sSQL & ",oma10 as 發票號碼 "
            sSQL = sSQL & ",to_char(oma09,'yyyy/mm/dd')  as 發票日期 "
            sSQL = sSQL & ",oma32 as 收款方式 "
            sSQL = sSQL & ",to_char(oma11,'yyyy/mm/dd') as 應收款日 "
            sSQL = sSQL & ",to_char(oma12,'yyyy/mm/dd')  as  容許票到期日 "
            sSQL = sSQL & ",oma23 as 幣別 "
            sSQL = sSQL & ",oma24 as 立帳匯率 "
            sSQL = sSQL & ",oma21 as 稅別 "
            sSQL = sSQL & ",oma211 as 稅率 "
            sSQL = sSQL & ",decode(oma25,'2','EXP','LOC') as 銷售類別 "
            sSQL = sSQL & ",oma14 as 帳款人員 "
            sSQL = sSQL & ",100 as 折扣率 "
            'sSQL = sSQL & ",substr(omb06,1,15) as RMA單號 "
            sSQL = sSQL & ",RMA.RMA_NO as RMA單號 "
            'sSQL = sSQL & ",rmashd_shipmentno "
            sSQL = sSQL & ",omb14 as 應收金額 "
            sSQL = sSQL & ",substr(omb06,17,9) as rmad_serialno "
            'sSQL = sSQL & " j.export_partno "
            'sSQL = sSQL & ",oma03 as 終端客戶編號 "
            'sSQL = sSQL & ",occ02 as 終端客戶名稱 "
            'sSQL = sSQL & ",occ241 as 終端客戶地址 "
            'sSQL = sSQL & ",occ261 as 終端客戶電話 "
            'sSQL = sSQL & ",occ29 as 聯絡人 "
            sSQL = sSQL & ",DECODE(CUSTOMER.CU_ISENDUSER,'Y',ENDUSER.EU_NO,oma03) as 終端客戶編號"
            sSQL = sSQL & ",DECODE(CUSTOMER.CU_ISENDUSER,'Y',ENDUSER.EU_NAME,occ02) as 終端客戶名稱"
            sSQL = sSQL & ",DECODE(CUSTOMER.CU_ISENDUSER,'Y',ENDUSER.EU_ADDRESS,occ241) as 終端客戶地址"
            sSQL = sSQL & ",DECODE(CUSTOMER.CU_ISENDUSER,'Y',ENDUSER.EU_TEL,occ261) as 終端客戶電話"
            sSQL = sSQL & ",DECODE(CUSTOMER.CU_ISENDUSER,'Y',ENDUSER.EU_CONTACT,occ29) as 聯絡人"
            sSQL = sSQL & ",omb14 as 終端應收 "
            'sSQL = sSQL & ",(Select SUM(RMASALE_QUOTED.RMASQ_QUOTE) From RMADetail,RMASALE_QUOTED Where RMADetail.RMAD_ID=RMASALE_QUOTED.RMASQ_RMADID And RMADetail.RMAD_RMANO=RMA.RMA_NO) as Actual_Amount "

            If sCOMP_ADMIN.ToUpper() = "CHINA" Then
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & " INNER JOIN ciphersh.oma_file ON RMA_ARNO=oma01 "
                sSQL = sSQL & " INNER JOIN ciphersh.omb_file ON omb01=oma01 "
                sSQL = sSQL & " INNER JOIN ciphersh.occ_file ON occ01=oma03 "
                ' sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01"
                sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01 and CU_NO=RMA.RMA_CUNO "
                sSQL = sSQL & " Left JOIN ENDUSER ON CUSTOMER.CU_NO=ENDUSER.EU_NO "

            ElseIf sCOMP_ADMIN.ToUpper() = "MPLUS" Then
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & " INNER JOIN CIPHERBOND.oma_file ON RMA_ARNO=oma01 "
                sSQL = sSQL & " INNER JOIN CIPHERBOND.omb_file ON omb01=oma01 "
                sSQL = sSQL & " INNER JOIN CIPHERBOND.occ_file ON occ01=oma03 "
                ' sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01"
                sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01 and CU_NO=RMA.RMA_CUNO "
                sSQL = sSQL & " Left JOIN ENDUSER ON CUSTOMER.CU_NO=ENDUSER.EU_NO "
            Else
                sSQL = sSQL & "From RMA "
                sSQL = sSQL & " INNER JOIN cipherlab.oma_file ON RMA_ARNO=oma01 "
                sSQL = sSQL & " INNER JOIN cipherlab.omb_file ON omb01=oma01 "
                sSQL = sSQL & " INNER JOIN cipherlab.occ_file ON occ01=oma03 "
                ' sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01"
                sSQL = sSQL & " INNER JOIN customer ON CU_TIPTOP_ID = occ01 and CU_NO=RMA.RMA_CUNO "
                sSQL = sSQL & " Left JOIN ENDUSER ON CUSTOMER.CU_NO=ENDUSER.EU_NO "
            End If

            sSQL = sSQL & " Where Exists(Select * From RMA_Shipping,RMA_ShippingDetail "
            sSQL = sSQL & "              Where RMA_Shipping.RMASH_ID=RMA_ShippingDetail.RMASHD_RMASHID"
            sSQL = sSQL & "                And " & sCondition
            sSQL = sSQL & "                And RMA_ShippingDetail.RMASHD_RMANO=RMA.RMA_NO)"

            sSQL = sSQL & " AND RMA.RMA_ARNO='" & sRMA_ARNO.Trim() & "'"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRptAD)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRptAD
    End Function

    ''' <summary>
    ''' MPLUS 要寄 NOTICES
    ''' </summary>
    ''' <param name="sRMANO"></param>
    ''' <returns></returns>
    Public Function qryNotices(ByVal sRMANO As String) As RmaDTO.RequestReportDataTable
        Dim i As Integer = 0
        Dim sCondition As String = ""
        Dim sSQL As String = ""
        Dim dtTmp As New DataTable
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()
        Try
            If sRMANO.Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RMA_NO", sRMANO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASH_SHIPPINGNO=:RMASH_SHIPPINGNO "
            End If
            sSQL = "SELECT RMA_NO,RMA_ID,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_CSTMP,CU_NAME,COMP_NAME, " &
                   "COMP_ADDRESS,COMP_TEL,'' AS NoticeDesc,'' CW_EDATE,RMA_EUCOMPANY,RMA_EUNAME,RMA_EUADDRESS,RMA_EUTEL,RMA_EUMAIL " &
                   "FROM RMA " &
                   "LEFT JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO " &
                   "LEFT JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO " &
                   "WHERE RMA_NO=:RMA_NO " &
                   "AND RMA_MARK = '0' " &
                   "ORDER BY CW_EDATE,RMA_NO"


            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtRequest)
        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRequest
    End Function

    ''' <summary>
    ''' Shpping時若有全保需要寄PACKING LIST
    ''' </summary>
    ''' <param name="LanguageID"></param>
    ''' <param name="ShippingNo"></param>
    ''' <returns></returns>
    Public Function qryShpRmaNo(ByVal LanguageID As String, ByVal ShippingNo As String) As RmaDTO.RequestReportDataTable
        Dim i As Integer = 0
        Dim sCondition As String = ""
        Dim sSQL As String = ""
        Dim dtTmp As New DataTable
        Dim dtRequest As New RmaDTO.RequestReportDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()
        Try
            If ShippingNo.Trim() <> "" Then
                oQuery.addWHERE("RMASH_SHIPPINGNO", ":RMASH_SHIPPINGNO", ShippingNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASH_SHIPPINGNO=:RMASH_SHIPPINGNO "
            End If
            sSQL = "SELECT RMA_NO,RMA_ID,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_CSTMP,CU_NAME,COMP_NAME, " &
                   "      COMP_ADDRESS,COMP_TEL,'' AS NoticeDesc,RMAD_SERIALNO,RMAD_MODELNO,RMAD_CUSNAME,RMAD_WARRANTY,RMAD_PRODUCTDESC,FARC_NAME,'' AS SeqID," &
                   "      MAX (CW_EDATE) CW_EDATE,RMA_EUCOMPANY,RMA_EUNAME,RMA_EUADDRESS,RMA_EUTEL,RMA_EUMAIL,RMASH_SHIPPINGNO,vShipping.*" &
                   " FROM (SELECT DISTINCT RMASH_SHIPPINGNO," &
                   "                       RMASHD_RMANO," &
                   "                       RMASH_TRACKINGNO," &
                   "                       TO_CHAR (RMASH_CSTMP, 'yyyy/mm/dd') RMASH_CSTMP," &
                   "                       RMASMD_SERIALNO" &
                   "         FROM RMA_SHIPPING" &
                   "              INNER JOIN RMA_SHIPPINGDETAIL ON RMASH_ID = RMASHD_RMASHID" &
                   "              INNER JOIN RMA_SHIPMENT ON RMASM_PACKINGNO = RMASHD_SHIPMENTNO" &
                   "              INNER JOIN RMA_SHIPMENTDETAIL ON RMASM_ID = RMASMD_RMASMID)" &
                   "      vShipping" &
                   "      INNER JOIN RMA ON RMASHD_RMANO = RMA_NO" &
                   "      LEFT JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" &
                   "      LEFT JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO" &
                   "      LEFT JOIN" &
                   "      (SELECT *" &
                   "         FROM RMADETAIL" &
                   "              LEFT JOIN FAILUREREASONSCLASS" &
                   "                 ON     RMADETAIL.RMAD_FARFARCNO =" &
                   "                           FAILUREREASONSCLASS.FARC_NO" &
                   "                    AND FAILUREREASONSCLASS.FARC_DFLNO ='" & LanguageID.Trim() & "') vwRMADETAIL" &
                   "         ON     RMA.RMA_NO = vwRMADETAIL.RMAD_RMANO" &
                   "            AND RMAD_SERIALNO = RMASMD_SERIALNO" &
                   "      LEFT JOIN export ON EXPORT_SERIALNO = RMASMD_SERIALNO" &
                   " WHERE     RMA_MARK = '0'" &
                   "      AND RMAD_MARK = '0' " & sCondition &
                   " GROUP BY RMA_NO,RMA_ID,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_CSTMP,CU_NAME,COMP_NAME,COMP_ADDRESS,COMP_TEL,RMAD_SERIALNO," &
                   "        RMAD_MODELNO,RMAD_CUSNAME,RMAD_WARRANTY,RMAD_PRODUCTDESC,FARC_NAME,RMA_EUCOMPANY,RMA_EUNAME,RMA_EUADDRESS,RMA_EUTEL,RMA_EUMAIL," &
                   "  RMASH_SHIPPINGNO, RMASHD_RMANO, RMASH_TRACKINGNO, RMASH_CSTMP, RMASMD_SERIALNO" &
                 " ORDER BY CW_EDATE,RMA_NO"

            dtTmp = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dtTmp, dtRequest)
        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRequest
    End Function

    ''' <summary>
    ''' 取得月對帳單
    ''' </summary>
    ''' <param name="repair_no"></param>
    ''' <param name="sDate"></param>
    ''' <param name="eDate"></param>
    ''' <returns></returns>
    Public Function qryMaintenanceStatement(ByVal repair_no As String, ByVal sDate As String, ByVal eDate As String) As DataTable
        Dim dt As DataTable
        Dim sSQL As String = ""
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()
        Try
            sSQL = "SELECT NVL (a.RMASH_TRACKINGNO, '') TrackingNo,
                RMASM_ADNAME  RMAD_RECVAD,
                       --a.RMASH_ID,
                    ( vwSHIPMENT.RMAD_MODELNO) MODELNO,
                    (vwSHIPMENT.RMAD_WARRANTY) WARRANTY,
                       d.RMA_COMPNO,
                       TO_CHAR (c.RMASHD_LUSTMP, 'yyyy/mm/dd') RMASHD_LUSTMP,
                       TO_CHAR (a.RMASH_CSTMP, 'yyyy/mm/dd') Shipped_Date,
                       TO_CHAR (a.RMASH_CSTMP, 'yyyy') Shipped_Year,
                       TO_CHAR (a.RMASH_CSTMP, 'mm') Shipped_Month,
                       TO_CHAR (a.RMASH_CSTMP, 'dd') Shipped_Day,
                       d.RMA_CUNO,
                       b.CU_NAME,
                       b.CU_COUNTRYID,
                       g.COUNTRY_NAME,
                       c.RMASHD_RMANO RMA_NO,
                       vwSHIPMENT.RMAD_SERIALNO serial_no,
                       --RMASM_CSTMP   NoticeDate,
                       SUBSTR(f.EXPORT_WAR_ID,5,2) WARRANTY_TYPE,
                       CASE WHEN vwSHIPMENT.RMAD_ISWARRANTY='1' THEN 'IW' ELSE 'OW' END Warranty_Kind,
                       CASE WHEN vwSHIPMENT.RMAD_RECEVSTATUS='2' THEN 'NTF'
                            WHEN (vwSHIPMENT.RMAD_STATUS = 91 AND vwSHIPMENT.RMAD_RECEVSTATUS = 1) THEN 'Not repaired' ELSE 'Repaired' END FINAL_STATUS
                       FROM RMA_SHIPPING a
                       INNER JOIN CUSTOMER b ON b.CU_NO = a.RMASH_CUNO
                       INNER JOIN RMA_SHIPPINGDETAIL c ON c.RMASHD_RMASHID = a.RMASH_ID
                       INNER JOIN RMA d ON d.RMA_NO = c.RMASHD_RMANO
                       --JOIN RMADETAIL e ON e.RMAD_RMANO = d.RMA_NO
                       INNER JOIN (SELECT RMASMD_RMANO,RMASM_PACKINGNO,TO_CHAR (RMASM_CSTMP, 'yyyy/mm/dd') AS RMASM_CSTMP,RMASM_ADNAME,
                                   RMASMD_SERIALNO,RMAD_STATUS,RMAD_RECEVSTATUS,RMAD_PARTSN,RMAD_SERIALNO,RMAD_MODELNO,RMAD_WARRANTY,
                                   RMAD_ISWARRANTY
                                   FROM RMA_SHIPMENT
                                   INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID
                                   INNER JOIN RMADETAIL ON RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMADETAIL.RMAD_ID) vwSHIPMENT 
                                 ON c.RMASHD_RMANO = vwSHIPMENT.RMASMD_RMANO
                                    AND c.RMASHD_RMASMPACKINGNO = vwSHIPMENT.RMASM_PACKINGNO
                       LEFT JOIN (SELECT MAX(CW_EDATE) CW_EDATE,EXPORT_SERIALNO, max(EXPORT_WAR_ID) EXPORT_WAR_ID
                                  FROM EXPORT 
                                  GROUP BY EXPORT_SERIALNO) f ON f.EXPORT_SERIALNO = vwSHIPMENT.RMAD_SERIALNO --AND f.EXPORT_CUSTNO = d.RMA_CUNO
                       LEFT JOIN COUNTRY g ON g.COUNTRY_ID = b.CU_COUNTRYID                     
                       WHERE 1 = 1
                       --AND RMASHD_RMANO ='ARMA-2022050013'
                       --AND e.RMAD_SERIALNO='DQI004456'
                       AND d.RMA_COMPNO=:RMA_COMPNO
                       AND (TO_CHAR (a.RMASH_LUSTMP, 'yyyy/mm/dd') >= :sDate
                       AND TO_CHAR (a.RMASH_LUSTMP, 'yyyy/mm/dd') <= :eDate)
                       ORDER BY a.RMASH_CSTMP,vwSHIPMENT.RMAD_SERIALNO    
                      "
            oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", repair_no, OracleType.VarChar)
            oQuery.addWHERE("RMASH_LUSTMP", ":sDate", sDate, OracleType.VarChar)
            oQuery.addWHERE("RMASH_LUSTMP", ":eDate", eDate, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

            Return dt

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    Public Function Get_RMA_COMPNO_RMAR_REPAIRADNAME(ByVal RMA_COMPNO As String) As DataTable

        Dim myDataTable As New DataTable
        Dim sSQL As String = ""
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()
        Try
            sSQL = "   select 
                       U.RMAD_RMANO
                       ,U.RMAD_SERIALNO
                       , K.RMAR_REPAIRADNAME from RMADETAIL U
                       LEFT JOIN RMAREPAIR K ON K.RMAR_RMADID = U.RMAD_ID   
                       where  RMAD_RMANO in ( select RMA_NO from RMA where RMA_COMPNO =:RMA_COMPNO  ) 
                      "
            oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", RMA_COMPNO, OracleType.VarChar)
            myDataTable = oQuery.ExecuteDT(sSQL)

            Return myDataTable

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try



    End Function

    Public ReadOnly Property getSQL() As Collection
        Get
            Return _SQLCollection
        End Get
    End Property

End Class

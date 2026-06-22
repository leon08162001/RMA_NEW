Imports ICAT_OracleDAO

Public Class QueryWatyBI_DAO

    ''' <summary>
    ''' 從來源資料庫撈出要匯入資料
    ''' </summary>
    Public Function GetSourceData() As List(Of WarrantyBIRsp)
        Dim lsWatyBIRsp As List(Of WarrantyBIRsp)
        Dim dt As New DataTable()
        Dim conn As New Connection()
        Dim oQuery As New Query(conn)
        conn.Open()

        Try
            Dim sql As String = "
                SELECT DISTINCT 'RMA' AS BI_SOURCE, waty_no AS BI_ORDERNO, waty_date AS BI_WATY_DATE, waty_cust AS BI_CUNO, occ02 AS BI_CUNAME,
                wati_seq AS BI_ORDERSEQ, wati_skuno AS BI_PRODUCTNO, wati_qty AS BI_ORDERQTY, war_type AS BI_WATY_TYPE, wati_ver AS BI_WATY_VER,
                wati_ver_act AS BI_WATYNO, decode(wati_year, 3, wati_qty, 5, wati_qty*2) AS BI_BATTERYQTY,
                to_char(wats_warrnstart, 'yyyy/MM/dd') AS BI_WATY_SDATE, to_char(wats_warrnend, 'yyyy/MM/dd') AS BI_WATY_EDATE
                FROM warrantyord 
                JOIN warrantyitem ON waty_no = wati_watyno 
                JOIN warrantyserial ON wati_watyno = wats_watyno AND wati_seq=wats_watyseq
                JOIN warrset ON war_id = wati_ver
                JOIN cipherlab.occ_file ON occ01 = waty_cust
                WHERE war_type in ('PB', 'EB') 
                AND isconfirm = 'Y'
                AND wati_mark = 0 and wats_mark = 0
                UNION 
                SELECT DISTINCT 'AXBA', oea01, oea02, oea03, oea032, oeb03, oeb04, oeb24, decode(ta_oeb080, 5, 'EB', 6, 'PB') warr_type, ta_oeb084,
                FN_GetWarrantyPNOByWARRNO(ta_oeb084, 1, ta_oeb082) warr_pno, decode(ta_oeb082, 3, oeb24, 5, oeb24*2) batt_qty,
                to_char(cw_sdate, 'yyyy/MM/dd'), to_char(cw_edate, 'yyyy/MM/dd') 
                FROM cipherlab.oea_file 
                JOIN cipherlab.oeb_file ON oea01 = oeb01
                JOIN export ON export_ordernumber = oea01 || '-' || oeb03
                WHERE ta_oeb080 in (5, 6) --電池保
                AND oea00 = 1 --1:訂單, 0:合約
                AND oeb24 > 0 --已交數量
                AND oeaconf = 'Y' --已確認
            "
            dt = oQuery.ExecuteDT(sql)

            lsWatyBIRsp = New Common().DataTableToModelList(Of WarrantyBIRsp)(dt)
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return lsWatyBIRsp
    End Function

End Class

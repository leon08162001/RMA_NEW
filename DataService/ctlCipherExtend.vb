Imports DefLanguage
Imports ICAT_OracleDAO

Public Class ctlCipherExtend

#Region "Class:Quote:報價單"
    Public Class Quote
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得要列印的報價單
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="sRMAID">傳入RMA_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回QuoteTable</returns>
        ''' <remarks></remarks>
        Public Function getQuoteReportData(ByVal LanguageID As String, ByVal sRMAID As String, Optional ByVal OrderBY As String = "") As CipherExtendDTO.QuotaReportDataTable
            Dim sCondition As String = ""
            Dim dtRequest As New CipherExtendDTO.QuotaReportDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                If OrderBY.Trim = "" Then
                    OrderBY = " RMADETAIL.RMAD_RMANO, RMADETAIL.RMAD_SEQ "
                End If

                OrderBY = " ORDER BY " & OrderBY

                sCondition = " Where RMA.RMA_NO='" & sRMAID & "' AND (RMADETAIL.RMAD_STATUS <>91 or RMADETAIL.RMAD_RECEVSTATUS <>2) "

                Dim sSQL As String = "SELECT"
                sSQL = sSQL & " RMA.RMA_CUNO as CU_CUNO, "
                sSQL = sSQL & " CUSTOMER.CU_NAME as CU_NAME, "
                sSQL = sSQL & " CUSTOMER.CU_CONTACTPERSON as CU_ContactPerson, "
                sSQL = sSQL & " CUSTOMER.CU_TEL as CU_TelFax, "
                sSQL = sSQL & " CUSTOMER.CU_ADDRESS1 as CU_Address, "
                sSQL = sSQL & " RMA.RMA_NO as CU_RMANO,  "
                sSQL = sSQL & " to_char(RMA.RMA_CSTMP,'YYYY-MM-DD') as CU_SendDate, "
                sSQL = sSQL & " to_char(RMAREPAIR_QUOTED.RMARQ_LUSTMP,'YYYY-MM-DD') as CU_QuoteDate, "
                sSQL = sSQL & " RMADETAIL.RMAD_SEQ as IT_SEQ, "
                sSQL = sSQL & " RMADETAIL.RMAD_MODELNO as IT_Model, "
                sSQL = sSQL & " RMADETAIL.RMAD_SERIALNO as IT_SerialNO, "
                sSQL = sSQL & " case when RMADETAIL.RMAD_STATUS=91 then 'N' else '' end as IT_Status, "
                sSQL = sSQL & " RMAREPAIR.RMAR_PROBLEMDESC as IT_ProblemDesc, "
                sSQL = sSQL & " RMAREPAIR.RMAR_REPAIRDESC as IT_RepairDesc, "
                'sSQL = sSQL & " RMAREPAIR_DETAIL.RMARED_NPARTNO as IT_PartNo, "
                'sSQL = sSQL & " RMAREPAIR_QUOTED.RMARQ_QUOTE as IT_RepairQuote, "
                'sSQL = sSQL & " RMASALE_QUOTED.RMASQ_QUOTE as IT_SalesQuote, "
                sSQL = sSQL & " case  when RMASALE_QUOTED.RMASQ_QUOTE >=0 then  RMASALE_QUOTED.RMASQ_QUOTE else RMAREPAIR_QUOTED.RMARQ_QUOTE end as IT_Quote "
                sSQL = sSQL & " FROM "
                sSQL = sSQL & " RMA INNER JOIN RMADETAIL ON (RMA.RMA_NO=RMADETAIL.RMAD_RMANO) "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR ON (RMADETAIL.RMAD_ID=RMAREPAIR.RMAR_RMADID)"
                'sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_DETAIL ON (RMADETAIL.RMAD_ID=RMAREPAIR_DETAIL.RMARED_RMADID)"
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON (RMADETAIL.RMAD_ID=RMAREPAIR_QUOTED.RMARQ_RMADID)"
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON (RMADETAIL.RMAD_ID=RMASALE_QUOTED.RMASQ_RMADID)"
                sSQL = sSQL & " LEFT OUTER JOIN CUSTOMER on (RMA.RMA_CUNO=CUSTOMER.CU_NO)"

                sSQL = sSQL & sCondition
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)

                Common.TransferDataTable(dt, dtRequest)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRequest
        End Function

    End Class
#End Region

End Class

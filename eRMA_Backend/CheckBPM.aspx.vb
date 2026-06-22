Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports Newtonsoft.Json

Partial Class CheckBPM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim myctlChargeQuoted As New ctlChargeQuoted
            Dim myDataTable As New DataTable
            Dim dtRMACharge_QUOTED As New ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable
            Dim dtChargeQUOTED_SN As New ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable
            Dim dtRMACHARGE_QUOTED_PART As New ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable

            myDataTable = myctlChargeQuoted.Select_RMACHARGE_QUOTED(Request.Params("RMACQ_EF_ID").ToString())

            If myDataTable.Rows.Count > 0 Then

                myDataTable(0)("RMACQ_EF_ID").ToString().Trim()
                Dim RMACQ_SALEQUOTE As Decimal = myDataTable(0)("RMACQ_SALEQUOTE").ToString().Trim()
                Dim RMACQ_DISCOUNT As Decimal = myDataTable(0)("RMACQ_DISCOUNT").ToString().Trim()


                dtRMACharge_QUOTED = JsonConvert.DeserializeObject(Of ChargeQuotedDTO.RMACHARGE_QUOTEDDataTable)(myDataTable(0)("RMACHARGE_QUOTEDDATATABLE"))
                dtChargeQUOTED_SN = JsonConvert.DeserializeObject(Of ChargeQuotedDTO.RMACHARGE_QUOTED_SNDataTable)(myDataTable(0)("RMACHARGE_QUOTED_SNDATATABLE"))
                dtRMACHARGE_QUOTED_PART = JsonConvert.DeserializeObject(Of ChargeQuotedDTO.RMACHARGE_QUOTED_PARTDataTable)(myDataTable(0)("RMACHARGE_QUOTED_PARTDATATABLE"))

                myctlChargeQuoted.saveChargeQUOTED(dtRMACharge_QUOTED, dtChargeQUOTED_SN, dtRMACHARGE_QUOTED_PART)

                '修改結案
                Dim RMACQ_RMANO As String = dtRMACharge_QUOTED(0)("RMACQ_RMANO").ToString().Trim()
                Dim RMACQ_EF_ID As String = myDataTable(0)("RMACQ_EF_ID").ToString().Trim()

                '塞入Flow回傳的流程序號及更改狀態(UPDATE RMACHARGE_QUOTED SET RMACQ_APPROVAL=1,RMACQ_EF_ID=sRMACQ_EF_ID)
                Dim sRMACQ_RMANO As String = RMACQ_RMANO
                Dim sRMACQ_EF_ID As String = RMACQ_EF_ID
                Dim retMessage As String = runSP_UPD_RMACQ(sRMACQ_RMANO, 2, sRMACQ_EF_ID)

                '刪除log
                myctlChargeQuoted.del_RMACQ_EF_TABLE(RMACQ_EF_ID)

            End If





        Catch ex As Exception

        Finally

        End Try


    End Sub

    Public Function runSP_UPD_RMACQ(ByVal RMACQ_RMANO_IN As String, ByVal RMACQ_APPROVAL_IN As Integer, ByVal RMACQ_EF_ID_IN As String) As String
        Dim retval As String = ""

        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()

        Dim oCommand As OracleCommand = oConn.Command
        Try

            oCommand.CommandText = "SP_UPD_RMACQ"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure

            oCommand.Parameters.Add("RMACQ_RMANO_IN", OracleType.NVarChar).Value = RMACQ_RMANO_IN
            oCommand.Parameters("RMACQ_RMANO_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RMACQ_APPROVAL_IN", OracleType.Int16).Value = RMACQ_APPROVAL_IN
            oCommand.Parameters("RMACQ_APPROVAL_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("RMACQ_EF_ID_IN", OracleType.NVarChar).Value = RMACQ_EF_ID_IN
            oCommand.Parameters("RMACQ_EF_ID_IN").Direction = ParameterDirection.Input

            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output

            oCommand.ExecuteNonQuery()

            retval = oCommand.Parameters("vResult").Value

            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text

        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try

        Return retval
    End Function

End Class

Imports System.Data.OracleClient
Imports ICAT_OracleDAO

Public Class ctlPolicy

    ''' <summary>
    ''' 取得Policy資料
    ''' </summary>
    ''' <param name="DEFL_NO">語系代號</param>
    ''' <returns>傳回 FAQDataTable</returns>
    ''' <remarks></remarks>
    Public Function Query(ByVal DEFL_NO As String, ByVal COMP_NO As String) As PolicyDTO.PolicyDataTable
        Dim sCondition As String = ""
        Dim stPolicy As New PolicyDTO.PolicyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("POLICY_DFLNO", ":POLICY_DFLNO", DEFL_NO, OracleType.VarChar)
            sCondition = sCondition & " AND POLICY_DFLNO=:POLICY_DFLNO"
            oQuery.addWHERE("POLICY_COMPNO", ":POLICY_COMPNO", COMP_NO, OracleType.VarChar)
            sCondition = sCondition & " AND POLICY_COMPNO=:POLICY_COMPNO"

            Dim sSQL As String = "SELECT * FROM POLICY WHERE 1=1" & sCondition
            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, stPolicy)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return stPolicy
    End Function

    ''' <summary>
    ''' Policy-新增
    ''' </summary>
    ''' <param name="dtPolicy">傳入POLICYDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveAdd(ByVal dtPolicy As PolicyDTO.POLICYDataTable)
        Dim oGuid As Guid = Guid.NewGuid

        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dr As PolicyDTO.POLICYRow = dtPolicy.Rows(0)

            oExecute.addParameter("POLICY_ID", oGuid.ToString, OracleType.VarChar)
            oExecute.addParameter("POLICY_TEXT", dr.POLICY_TEXT, OracleType.NVarChar)

            oExecute.addParameter("POLICY_AD", dr.POLICY_AD, OracleType.NVarChar)
            oExecute.addParameter("POLICY_ADNAME", dr.POLICY_ADNAME, OracleType.NVarChar)
            oExecute.addParameter("POLICY_CSTMP", dr.POLICY_CSTMP, OracleType.DateTime)
            oExecute.addParameter("POLICY_LUAD", dr.POLICY_LUAD, OracleType.NVarChar)
            oExecute.addParameter("POLICY_LUADNAME", dr.POLICY_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("POLICY_LUSTMP", dr.POLICY_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("POLICY_DFLNO", dr.POLICY_DFLNO, OracleType.VarChar)

            oExecute.Command("POLICY", Execute.eumCommandType.AddNew)

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
    ''' Policy-修改
    ''' </summary>
    ''' <param name="dtPolicy">傳入POLICYDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtPolicy As PolicyDTO.POLICYDataTable)
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()
            Dim dr As PolicyDTO.POLICYRow = dtPolicy.Rows(0)

            oExecute.addParameter("POLICY_TEXT", dr.POLICY_TEXT, OracleType.NVarChar)
            oExecute.addParameter("POLICY_AD", dr.POLICY_AD, OracleType.NVarChar)
            oExecute.addParameter("POLICY_ADNAME", dr.POLICY_ADNAME, OracleType.NVarChar)
            oExecute.addParameter("POLICY_CSTMP", dr.POLICY_CSTMP, OracleType.DateTime)
            oExecute.addParameter("POLICY_LUAD", dr.POLICY_LUAD, OracleType.NVarChar)
            oExecute.addParameter("POLICY_LUADNAME", dr.POLICY_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("POLICY_LUSTMP", dr.POLICY_LUSTMP, OracleType.DateTime)

            oExecute.addWHERE("POLICY_ID", dr.POLICY_ID, OracleType.VarChar)
            oExecute.addWHERE("POLICY_DFLNO", dr.POLICY_DFLNO, OracleType.VarChar)

            oExecute.Command("POLICY", Execute.eumCommandType.UPDATE)
            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

End Class

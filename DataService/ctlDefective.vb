Imports System.Data.OracleClient
Imports DefLanguage
Imports ICAT_OracleDAO

Public Class ctlDefective
    Dim _oLanguage As New ctlLanguage

    ''' <summary>
    ''' 取得 Defective 資料
    ''' </summary>
    ''' <param name="DFLNO">語系</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryAll(ByVal DFLNO As String, Optional ByVal OrderBY As String = "") As DefectiveDTO.DEFECTIVEDataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtDuty As New DefectiveDTO.DEFECTIVEDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " Defective_NO asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            sSQL = "SELECT * FROM Defective where DEFECTIVE_DFLNO='" & DFLNO.Trim() & "'" & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtDuty)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtDuty
    End Function

    ''' <summary>
    ''' 取得有效的 Defective 資料
    ''' </summary>
    ''' <param name="DFLNO">語系</param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByEffective(ByVal DFLNO As String, Optional ByVal OrderBY As String = "") As DefectiveDTO.DEFECTIVEDataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtDuty As New DefectiveDTO.DEFECTIVEDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " Defective_NO asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            sSQL = "SELECT * FROM Defective WHERE Defective_VISIBLE=1 AND DEFECTIVE_DFLNO='" & DFLNO.Trim() & "'" & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtDuty)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtDuty
    End Function

    ''' <summary>
    ''' 取得 Defective 資料
    ''' </summary>
    ''' <param name="DefectiveNo">Duty代碼</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByKey(ByVal DFLNO As String, ByVal DefectiveNo As String, ByVal DefectiveName As String, Optional ByVal OrderBY As String = "") As DefectiveDTO.vwDefectiveDataTable
        Dim sCondition As String = ""

        Dim dtDuty As New DefectiveDTO.vwDefectiveDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            If OrderBY.Trim = "" Then
                OrderBY = " DEFECTIVE_NO asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            '關聯 DefLanguage.DFL_NO
            If DFLNO.Trim() <> "" Then
                oQuery.addWHERE("DEFECTIVE_DFLNO", ":DEFECTIVE_DFLNO", DFLNO, OracleType.VarChar)
                sCondition = sCondition & " AND DEFECTIVE_DFLNO=:DEFECTIVE_DFLNO"
            End If

            'Duty代碼
            If DefectiveNo.Trim() <> "" Then
                oQuery.addWHERE("DEFECTIVE_NO", ":DEFECTIVE_NO", DefectiveNo, OracleType.VarChar)
                sCondition = sCondition & " AND DEFECTIVE_NO=:DEFECTIVE_NO"
            End If

            'Duty名稱
            If DefectiveName.Trim() <> "" Then
                DefectiveName = "%" & DefectiveName.Trim() & "%"
                oQuery.addWHERE("DEFECTIVE_NAME", ":DEFECTIVE_NAME", DefectiveName.Trim().ToLower(), OracleType.VarChar)
                sCondition = sCondition & " AND lower(DEFECTIVE_NAME) like :DEFECTIVE_NAME"
            End If

            Dim sSQL As String = " SELECT DEFECTIVE.*,DFL_NAME "
            sSQL = sSQL & " FROM DEFECTIVE "
            sSQL = sSQL & " INNER JOIN DEFLANGUAGE ON DEFECTIVE.DEFECTIVE_DFLNO = DEFLANGUAGE.DFL_NO "
            sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtDuty)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtDuty
    End Function

    ''' <summary>
    ''' Defective-新增
    ''' </summary>
    ''' <param name="dtDefective">傳入dtDefective</param>
    ''' <remarks></remarks>
    Public Sub SaveAdd(ByVal dtDefective As DefectiveDTO.vwDefectiveDataTable)
        Dim i As Integer = 0

        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim dr As DefectiveDTO.vwDefectiveRow = dtDefective.Rows(0)

            oExecute.addParameter("DEFECTIVE_NO", dr.DEFECTIVE_NO, OracleType.VarChar)
            oExecute.addParameter("DEFECTIVE_NAME", dr.DEFECTIVE_NAME, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_VISIBLE", dr.DEFECTIVE_VISIBLE, OracleType.Int16)
            oExecute.addParameter("DEFECTIVE_AD", dr.DEFECTIVE_AD, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_ADNAME", dr.DEFECTIVE_ADNAME, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_CSTMP", dr.DEFECTIVE_CSTMP, OracleType.DateTime)
            oExecute.addParameter("DEFECTIVE_LUAD", dr.DEFECTIVE_LUAD, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_LUADNAME", dr.DEFECTIVE_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_LUSTMP", dr.DEFECTIVE_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("DEFECTIVE_DFLNO", dr.DEFECTIVE_DFLNO, OracleType.VarChar)

            oExecute.Command("DEFECTIVE", Execute.eumCommandType.AddNew)

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
    ''' Defective-修改
    ''' </summary>
    ''' <param name="dtDefective">傳入dtDefective</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtDefective As DefectiveDTO.vwDefectiveDataTable)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            Dim dr As DefectiveDTO.vwDefectiveRow = dtDefective.Rows(0)

            oExecute.addParameter("DEFECTIVE_NAME", dr.DEFECTIVE_NAME, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_VISIBLE", dr.DEFECTIVE_VISIBLE, OracleType.Int16)
            oExecute.addParameter("DEFECTIVE_LUAD", dr.DEFECTIVE_LUAD, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_LUADNAME", dr.DEFECTIVE_LUADNAME, OracleType.NVarChar)
            oExecute.addParameter("DEFECTIVE_LUSTMP", dr.DEFECTIVE_LUSTMP, OracleType.DateTime)

            oExecute.addWHERE("DEFECTIVE_NO", dr.DEFECTIVE_NO.Trim(), OracleType.VarChar)
            oExecute.addWHERE("DEFECTIVE_DFLNO", dr.DEFECTIVE_DFLNO.Trim(), OracleType.VarChar)
            oExecute.Command("DEFECTIVE", Execute.eumCommandType.UPDATE)

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
    ''' Defective-刪除
    ''' </summary>
    ''' <param name="DefectiveNo">Duty代碼</param>
    ''' <remarks></remarks>
    Public Sub SaveDel(ByVal DefectiveNo As String, ByVal DefectiveDFLNo As String)
        Dim blnFlag As Boolean = False
        Dim i As Integer = 0
        Dim sMessage As String = ""
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            blnFlag = chkIsExist(DefectiveNo)

            If blnFlag = True Then
                sMessage = _oLanguage.getText("Defective", "014", ctlLanguage.eumType.Validator)
                Throw New ArgumentException(sMessage)
            End If

            oExecute.addWHERE("DEFECTIVE_NO", DefectiveNo.Trim(), OracleType.VarChar)
            oExecute.addWHERE("DEFECTIVE_DFLNO", DefectiveDFLNo.Trim(), OracleType.VarChar)
            oExecute.Command("DEFECTIVE", Execute.eumCommandType.Delete)

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
    ''' 檢核Duty代碼是否存在RMA單裡
    ''' </summary>
    ''' <param name="DefectiveNo">Duty代碼</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function chkIsExist(ByVal DefectiveNo As String) As Boolean
        Dim blnFlag As Boolean = False
        Dim sCondition As String = ""

        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("RMAR_DUTYNO", ":RMAR_DUTYNO", DefectiveNo.Trim(), OracleType.VarChar)
            oQuery.addWHERE("RMARED_DEFECTIVE", ":RMARED_DEFECTIVE", DefectiveNo.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND (RMAR_DUTYNO=:RMAR_DUTYNO OR RMARED_DEFECTIVE=:RMARED_DEFECTIVE) "

            Dim sSQL As String = " SELECT RMAR_DUTYNO,RMARED_DEFECTIVE"
            sSQL = sSQL & " FROM RMADETAIL "
            sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
            sSQL = sSQL & " INNER JOIN RMAREPAIR_DETAIL ON RMADETAIL.RMAD_ID = RMAREPAIR_DETAIL.RMARED_RMADID "
            sSQL = sSQL & " where NOT(RMAR_DUTYNO is null AND RMARED_DEFECTIVE is null) "
            sSQL = sSQL & sCondition

            dt = oQuery.ExecuteDT(sSQL)

            If dt.Rows.Count > 0 Then
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

End Class

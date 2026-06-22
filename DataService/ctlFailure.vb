Imports System.Data.OracleClient
Imports DefLanguage
Imports ICAT_OracleDAO


Public Class ctlFailure

#Region "Class:FailureReasonsClass"
    Public Class FailureReasonsClass
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得FailureReasons資料
        ''' </summary>
        ''' <param name="FARC_DFLNO">語系代碼</param>
        ''' <param name="FARC_NO">不良原因類別代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 vwFailureReasonsDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryFailureClass(ByVal FARC_DFLNO As String, ByVal FARC_NO As String, Optional ByVal OrderBY As String = "") As FailureDTO.FailureReasonsClassDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FARC_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If FARC_DFLNO.Trim <> "" Then
                    oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"
                End If

                If FARC_NO.Trim <> "-1" Then
                    oQuery.addWHERE("FARC_NO", ":FARC_NO", FARC_NO.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND FARC_NO=:FARC_NO"
                End If

                Dim sSQL As String = "SELECT * FROM FAILUREREASONSCLASS " &
                        " WHERE 1=1 AND FARC_VISIBLE='1' " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        ''' <summary>
        ''' 取得不良原因類別By語系資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 DataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByLanguage(ByVal LanguageID As String, Optional ByVal OrderBY As String = "") As FailureDTO.FailureReasonsClassDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FARC_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"
                oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", LanguageID, OracleType.VarChar)

                Dim sSQL As String = "SELECT * FROM FAILUREREASONSCLASS WHERE 1=1 AND FARC_VISIBLE='1' " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        ''' <summary>
        ''' 取得FailureReasons資料
        ''' </summary>
        ''' <param name="FARC_NO">不良原因類別代碼</param>
        ''' <returns>傳回 vwFailureReasonsDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryKey(ByVal FARC_DFLNO As String, ByVal FARC_NO As String) As FailureDTO.tmpFailureReasonsDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.tmpFailureReasonsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"

                oQuery.addWHERE("FARC_NO", ":FARC_NO", FARC_NO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FARC_NO=:FARC_NO"

                Dim sSQL As String = " SELECT FARC_DFLNO, FARC_NO, FARC_NAME, FARC_VISIBLE, "
                sSQL = sSQL & " FARC_LUAD, FARC_LUADNAME, FARC_LUSTMP, DFL_NAME "
                sSQL = sSQL & " FROM FAILUREREASONSCLASS "
                sSQL = sSQL & " INNER JOIN DEFLANGUAGE ON FAILUREREASONSCLASS.FARC_DFLNO = DEFLANGUAGE.DFL_NO "
                sSQL = sSQL & " WHERE 1=1 AND FAILUREREASONSCLASS.FARC_VISIBLE='1' " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        ''' <summary>
        ''' 不良原因類別-新增
        ''' </summary>
        ''' <param name="dtFailure"></param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtFailure As FailureDTO.FailureReasonsClassDataTable)
            Dim blnFlag As Boolean = False
            Dim oFailure As New ctlFailure

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                Dim dr As FailureDTO.FailureReasonsClassRow = dtFailure.Rows(0)
                If oFailure.chkFailure(dr.FARC_DFLNO, dr.FARC_NO) = True Then
                    Throw New ArgumentException(_oLanguage.getText("Failure", "025", ctlLanguage.eumType.Validator))
                End If

                oConn.BeginTransaction()

                oExecute.addParameter("FARC_NO", dr.FARC_NO, OracleType.VarChar)
                oExecute.addParameter("FARC_NAME", dr.FARC_NAME, OracleType.NVarChar)
                oExecute.addParameter("FARC_VISIBLE", dr.FARC_VISIBLE, OracleType.Int16)
                oExecute.addParameter("FARC_AD", dr.FARC_AD, OracleType.NVarChar)
                oExecute.addParameter("FARC_ADNAME", dr.FARC_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("FARC_CSTMP", dr.FARC_CSTMP, OracleType.DateTime)
                oExecute.addParameter("FARC_LUAD", dr.FARC_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FARC_LUADNAME", dr.FARC_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FARC_LUSTMP", dr.FARC_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("FARC_DFLNO", dr.FARC_DFLNO, OracleType.VarChar)

                oExecute.Command("FAILUREREASONSCLASS", Execute.eumCommandType.AddNew)
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
        ''' 不良原因類別-修改
        ''' </summary>
        ''' <param name="dtFailure"></param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtFailure As FailureDTO.FailureReasonsClassDataTable)
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                Dim dr As FailureDTO.FailureReasonsClassRow = dtFailure.Rows(0)
                oConn.BeginTransaction()

                oExecute.addParameter("FARC_NAME", dr.FARC_NAME, OracleType.NVarChar)
                oExecute.addParameter("FARC_VISIBLE", dr.FARC_VISIBLE, OracleType.Int16)
                oExecute.addParameter("FARC_LUAD", dr.FARC_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FARC_LUADNAME", dr.FARC_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FARC_LUSTMP", dr.FARC_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("FARC_DFLNO", dr.FARC_DFLNO.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FARC_NO", dr.FARC_NO.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("FAILUREREASONSCLASS", Execute.eumCommandType.UPDATE)
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
        ''' 不良原因類別-刪除
        ''' </summary>
        ''' <param name="DFLNO">關聯 DefLanguage.DFL_NO</param>
        ''' <param name="FarcNo">不良原因類別代碼</param>
        ''' <remarks></remarks>
        Public Sub SaveDel(ByVal DFLNO As String, ByVal FarcNo As String)
            Dim blnFlag As Boolean = False
            Dim sMessage As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '檢核不良原因是否存在RMA單裡
                blnFlag = chkIsExist_FailureClass(FarcNo)
                If blnFlag = True Then
                    sMessage = _oLanguage.getText("Failure", "030", ctlLanguage.eumType.Validator)
                    Throw New ArgumentException(sMessage)
                End If

                oExecute.addWHERE("FARC_DFLNO", DFLNO.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FARC_NO", FarcNo.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("FAILUREREASONSCLASS", Execute.eumCommandType.Delete)

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
        ''' 檢核不良原因類別是否存在RMA單裡
        ''' </summary>
        ''' <param name="FarcNo">不良原因類別代碼</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkIsExist_FailureClass(ByVal FarcNo As String) As Boolean
            Dim blnFlag As Boolean = False
            Dim sCondition As String = ""
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_FARFARCNO", ":RMAD_FARFARCNO", FarcNo.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMAR_FARCNO", ":RMAR_FARCNO", FarcNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND (RMAD_FARFARCNO=:RMAD_FARFARCNO OR RMAR_FARCNO=:RMAR_FARCNO) "

                Dim sSQL As String = " SELECT RMAD_FARFARCNO, RMAR_FARCNO "
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " WHERE NOT(RMAD_FARFARCNO is null AND RMAR_FARCNO is null) "
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
#End Region

#Region "Class:FailureReasons"
    Public Class FailureReasons
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得FailureReasons資料
        ''' </summary>
        ''' <param name="FAR_NO">不良原因類別代碼</param>
        ''' <param name="FAR_DFLNO">語系代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 vwFailureReasonsDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryALL(ByVal FAR_DFLNO As String, ByVal FAR_NO As String, Optional ByVal OrderBY As String = "") As FailureDTO.tmpFailureReasonsDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.tmpFailureReasonsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try

                If OrderBY.Trim = "" Then
                    OrderBY = " FAR_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAR_DFLNO", ":FAR_DFLNO", FAR_DFLNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FAR_DFLNO=:FAR_DFLNO"

                oQuery.addWHERE("FARC_NO", ":FARC_NO", FAR_NO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FARC_NO=:FARC_NO"

                Dim sSQL As String = " SELECT FAILUREREASONS.*, DFL_NAME "
                sSQL = sSQL & " FROM FAILUREREASONS "
                sSQL = sSQL & " INNER JOIN FAILUREREASONSCLASS ON FAILUREREASONS.FAR_FARCNO = FAILUREREASONSCLASS.FARC_NO "
                sSQL = sSQL & "     AND FAILUREREASONS.FAR_DFLNO = FAILUREREASONSCLASS.FARC_DFLNO AND FAILUREREASONSCLASS.FARC_VISIBLE='1' "
                sSQL = sSQL & " INNER JOIN DEFLANGUAGE ON FAILUREREASONSCLASS.FARC_DFLNO = DEFLANGUAGE.DFL_NO "
                sSQL = sSQL & " WHERE 1=1 AND FAILUREREASONS.FAR_VISIBLE='1' " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        ''' <summary>
        ''' 取得FailureReasons資料
        ''' </summary>
        ''' <param name="FARC_DFLNO">語系代碼</param>
        ''' <param name="FARC_NO">不良原因類別代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 vwFailureReasonsDataTable</returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal FARC_DFLNO As String, ByVal FARC_NO As String, Optional ByVal OrderBY As String = "") As FailureDTO.vwFailureReasonsDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.vwFailureReasonsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("FAR_NO") = "FAR_oldNO"

                If OrderBY.Trim = "" Then
                    OrderBY = " FAR_FARCNO asc, FAR_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"

                If FARC_NO.Trim <> "" Then
                    oQuery.addWHERE("FAR_FARCNO", ":FAR_FARCNO", FARC_NO.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND FAR_FARCNO=:FAR_FARCNO"
                End If

                Dim sSQL As String = "SELECT * FROM vwFailureReasons " &
                        " WHERE 1=1 " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure, HasExceColumn)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        ''' <summary>
        ''' 取得FailureReasons資料
        ''' </summary>
        ''' <param name="FARC_DFLNO">語系代碼</param>
        ''' <param name="FARC_NO">不良原因類別代碼</param>
        ''' <param name="FAR_NO">不良原因類別代碼</param>
        ''' <returns>傳回 vwFailureReasonsDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByFailure(ByVal FARC_DFLNO As String, ByVal FARC_NO As String, ByVal FAR_NO As String) As FailureDTO.vwFailureReasonsDataTable
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.vwFailureReasonsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("FAR_NO") = "FAR_oldNO"

                oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"

                oQuery.addWHERE("FAR_FARCNO", ":FAR_FARCNO", FARC_NO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FAR_FARCNO=:FAR_FARCNO"

                oQuery.addWHERE("FAR_NO", ":FAR_NO", FAR_NO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND FAR_NO=:FAR_NO"

                Dim sSQL As String = "SELECT * FROM vwFailureReasons " &
                        " WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFailure, HasExceColumn)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFailure
        End Function

        Public Sub Save(ByVal dtFailureReasons As FailureDTO.vwFailureReasonsDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New Query(oConn)

            Dim dtFailureReasons_Add As New FailureDTO.vwFailureReasonsDataTable
            Dim dtFailureReasons_Edit As New FailureDTO.vwFailureReasonsDataTable

            oConn.Open()

            Try
                dtFailureReasons_Add = dtFailureReasons.Copy
                dtFailureReasons_Edit = dtFailureReasons.Copy

                Dim dvFailureReasons_Add As DataView = dtFailureReasons_Add.DefaultView()
                dvFailureReasons_Add.RowFilter = "FAR_oldNO<>''"
                Do While dvFailureReasons_Add.Count > 0
                    dvFailureReasons_Add.Delete(0)
                Loop

                Dim dvFailureReasons_Edit As DataView = dtFailureReasons_Edit.DefaultView()
                dvFailureReasons_Edit.RowFilter = "FAR_oldNO=''"
                Do While dvFailureReasons_Edit.Count > 0
                    dvFailureReasons_Edit.Delete(0)
                Loop

                '新增時檢核資料是否存在
                For i = 0 To dtFailureReasons_Add.Rows.Count - 1
                    Dim dr As FailureDTO.vwFailureReasonsRow = dtFailureReasons_Add.Rows(i)
                    If chkIsExist(dr.FAR_DFLNO, dr.FAR_FARCNO, dr.FAR_NO) = True Then
                        Throw New ArgumentException(_oLanguage.getText("Failure", "026", ctlLanguage.eumType.Validator))
                    End If
                Next

                '修改時檢核資料是否存在
                For i = 0 To dtFailureReasons_Edit.Rows.Count - 1
                    Dim dr As FailureDTO.vwFailureReasonsRow = dtFailureReasons_Edit.Rows(i)
                    If chkIsExist(dr.FAR_DFLNO, dr.FAR_FARCNO, dr.FAR_NO, dr.FAR_oldNO) = True Then
                        Throw New ArgumentException(_oLanguage.getText("Failure", "026", ctlLanguage.eumType.Validator))
                    End If
                Next

                oConn.BeginTransaction()

                If dtFailureReasons_Add.Rows.Count > 0 Then
                    Call SaveAdd(oExecute, dtFailureReasons_Add)
                End If

                If dtFailureReasons_Edit.Rows.Count > 0 Then
                    Call SaveEdit(oExecute, dtFailureReasons_Edit)
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
        ''' FailureReasons-新增
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtFailure">傳入vwFailureReasonsDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtFailure As FailureDTO.vwFailureReasonsDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtFailure.Rows.Count - 1
                    Dim dr As FailureDTO.vwFailureReasonsRow = dtFailure.Rows(i)

                    oExecute.addParameter("FAR_NO", dr.FAR_NO, OracleType.VarChar)
                    oExecute.addParameter("FAR_FARCNO", dr.FAR_FARCNO, OracleType.VarChar)
                    oExecute.addParameter("FAR_REASON", dr.FAR_REASON, OracleType.NVarChar)
                    oExecute.addParameter("FAR_VISIBLE", dr.FAR_VISIBLE, OracleType.Int16)

                    oExecute.addParameter("FAR_AD", dr.FAR_AD, OracleType.NVarChar)
                    oExecute.addParameter("FAR_ADNAME", dr.FAR_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAR_CSTMP", dr.FAR_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("FAR_LUAD", dr.FAR_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAR_LUADNAME", dr.FAR_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAR_LUSTMP", dr.FAR_LUSTMP, OracleType.DateTime)

                    oExecute.Command("FAILUREREASONS", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' FailureReasons-修改
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtFailure">傳入vwFailureReasonsDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtFailure As FailureDTO.vwFailureReasonsDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtFailure.Rows.Count - 1
                    Dim dr As FailureDTO.vwFailureReasonsRow = dtFailure.Rows(i)

                    oExecute.addParameter("FAR_REASON", dr.FAR_REASON, OracleType.NVarChar)
                    oExecute.addParameter("FAR_VISIBLE", dr.FAR_VISIBLE, OracleType.Int16)
                    oExecute.addParameter("FAR_LUAD", dr.FAR_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAR_LUADNAME", dr.FAR_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAR_LUSTMP", dr.FAR_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("FAR_FARCNO", dr.FAR_FARCNO.ToString().Trim(), OracleType.VarChar)
                    oExecute.addWHERE("FAR_NO", dr.FAR_NO.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("FAILUREREASONS", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex
            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 不良原因類別-新增
        ''' </summary>
        ''' <param name="dtFailure"></param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtFailure As FailureDTO.FailureReasonsDataTable)
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                Dim dr As FailureDTO.FailureReasonsRow = dtFailure.Rows(0)
                If chkIsExist(dr.FAR_DFLNO, dr.FAR_FARCNO, dr.FAR_NO) = True Then
                    Throw New ArgumentException(_oLanguage.getText("Failure", "026", ctlLanguage.eumType.Validator))
                End If

                oConn.BeginTransaction()

                oExecute.addParameter("FAR_DFLNO", dr.FAR_DFLNO, OracleType.VarChar)
                oExecute.addParameter("FAR_FARCNO", dr.FAR_FARCNO, OracleType.VarChar)
                oExecute.addParameter("FAR_NO", dr.FAR_NO, OracleType.VarChar)
                oExecute.addParameter("FAR_REASON", dr.FAR_REASON, OracleType.NVarChar)
                oExecute.addParameter("FAR_VISIBLE", dr.FAR_VISIBLE, OracleType.Int16)
                oExecute.addParameter("FAR_AD", dr.FAR_AD, OracleType.NVarChar)
                oExecute.addParameter("FAR_ADNAME", dr.FAR_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAR_CSTMP", dr.FAR_CSTMP, OracleType.DateTime)
                oExecute.addParameter("FAR_LUAD", dr.FAR_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FAR_LUADNAME", dr.FAR_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAR_LUSTMP", dr.FAR_LUSTMP, OracleType.DateTime)

                oExecute.Command("FAILUREREASONS", Execute.eumCommandType.AddNew)
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
        ''' 不良原因-修改
        ''' </summary>
        ''' <param name="dtFailure">傳入vwFailureReasonsDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtFailure As FailureDTO.FailureReasonsDataTable)
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                Dim dr As FailureDTO.FailureReasonsRow = dtFailure.Rows(0)
                oConn.BeginTransaction()

                oExecute.addParameter("FAR_REASON", dr.FAR_REASON, OracleType.NVarChar)
                oExecute.addParameter("FAR_VISIBLE", dr.FAR_VISIBLE, OracleType.Int16)
                oExecute.addParameter("FAR_LUAD", dr.FAR_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FAR_LUADNAME", dr.FAR_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAR_LUSTMP", dr.FAR_LUSTMP, OracleType.DateTime)


                oExecute.addWHERE("FAR_DFLNO", dr.FAR_DFLNO.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FAR_FARCNO", dr.FAR_FARCNO.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FAR_NO", dr.FAR_NO.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("FAILUREREASONS", Execute.eumCommandType.UPDATE)
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
        ''' 不良原因-刪除
        ''' </summary>
        ''' <param name="FarcN0">關聯 FailureReasonsClass.FARC_NO</param>
        ''' <param name="FarNo">不良原因代碼</param>
        ''' <remarks></remarks>
        Public Sub SaveDel(ByVal FarDFLNo As String, ByVal FarcN0 As String, ByVal FarNo As String)
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '檢核不良原因是否存在RMA單裡
                blnFlag = chkIsExist_Failure(FarNo)
                If blnFlag = True Then
                    Dim sDelMessage As String = _oLanguage.getText("Failure", "031", ctlLanguage.eumType.Validator)
                    Throw New ArgumentException(sDelMessage)
                End If

                oExecute.addWHERE("FAR_DFLNO", FarDFLNo.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FAR_FARCNO", FarcN0.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("FAR_NO", FarNo.ToString().Trim(), OracleType.VarChar)

                oExecute.Command("FAILUREREASONS", Execute.eumCommandType.Delete)

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
        ''' 檢核不良原因是否存在RMA單裡
        ''' </summary>
        ''' <param name="FarNo">不良原因代碼</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkIsExist_Failure(ByVal FarNo As String) As Boolean
            Dim blnFlag As Boolean = False
            Dim sCondition As String = ""
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_FARNO", ":RMAD_FARNO", FarNo.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMAR_FARNO", ":RMAR_FARNO", FarNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND (RMAD_FARNO=:RMAD_FARNO OR RMAR_FARNO=:RMAR_FARNO) "

                Dim sSQL As String = " SELECT RMAD_FARNO, RMAR_FARNO "
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " WHERE NOT(RMAD_FARNO is null AND RMAR_FARNO is null) "
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

        ''' <summary>
        ''' 檢核FailureReasons是否存在
        ''' </summary>
        ''' <param name="FAR_FARCNO">類別代碼</param>
        ''' <param name="newFAR_NO">新的不良原因代碼</param>
        ''' <param name="oldFAR_NO">舊的不良原因代碼</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:不存在, True:存在</remarks>
        Private Function chkIsExist(ByVal FAR_DFLNO As String, ByVal FAR_FARCNO As String, ByVal newFAR_NO As String, Optional ByVal oldFAR_NO As String = "") As Boolean
            Dim retval As Boolean = False
            Dim dt As New DataTable
            Dim dtFailure As New FailureDTO.vwFailureReasonsDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("FAR_DFLNO", ":FAR_DFLNO", FAR_DFLNO.Trim(), OracleType.VarChar)

                oQuery.addWHERE("FAR_FARCNO", ":FAR_FARCNO", FAR_FARCNO.Trim(), OracleType.VarChar)
                oQuery.addWHERE("FAR_NO", ":newFAR_NO", newFAR_NO.Trim(), OracleType.VarChar)

                If oldFAR_NO.Trim() <> "" Then
                    oQuery.addWHERE("FAR_NO", ":oldFAR_NO", oldFAR_NO.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND FAR_NO<>:oldFAR_NO"
                End If

                Dim sSQL As String = "SELECT * FROM FAILUREREASONS " &
                    " WHERE FAR_DFLNO=:FAR_DFLNO AND FAR_FARCNO=:FAR_FARCNO AND FAR_NO=:newFAR_NO" & sCondition

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

    End Class
#End Region

    ''' <summary>
    ''' 檢核FailureReasonsClass是否存在
    ''' </summary>
    ''' <param name="FARC_DFLNO">語系代碼</param>
    ''' <param name="FARC_NO">不良原因類別代碼參考號</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Public Function chkFailure(ByVal FARC_DFLNO As String, ByVal FARC_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim dt As New DataTable
        Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"

            oQuery.addWHERE("FARC_NO", ":FARC_NO", FARC_NO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND FARC_NO=:FARC_NO"

            Dim sSQL As String = "SELECT * FROM FAILUREREASONSCLASS WHERE 1=1 AND FARC_VISIBLE='1' " & sCondition

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
    ''' 檢核FailureReasonsClass是否存在
    ''' </summary>
    ''' <param name="FARC_DFLNO">語系代碼</param>
    ''' <param name="FARC_NO">不良原因類別代碼參考號</param>
    ''' <param name="FAR_NO">不良原因代碼</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Public Function chkFailure(ByVal FARC_DFLNO As String, ByVal FARC_NO As String, ByVal FAR_NO As String) As Boolean
        Dim retval As Boolean = False
        Dim dt As New DataTable
        Dim dtFailure As New FailureDTO.FailureReasonsClassDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", FARC_DFLNO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO"

            oQuery.addWHERE("FARC_NO", ":FARC_NO", FARC_NO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND FARC_NO=:FARC_NO"

            oQuery.addWHERE("FAR_NO", ":FAR_NO", FAR_NO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND FAR_NO=:FAR_NO"

            Dim sSQL As String = "SELECT * FROM FAILUREREASONSCLASS, FAILUREREASONS WHERE 1=1 " &
                " AND FAILUREREASONS.FAR_FARCNO = FAILUREREASONSCLASS.FARC_NO" & sCondition

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

End Class

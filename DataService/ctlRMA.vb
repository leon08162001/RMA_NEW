Imports System.Configuration
Imports System.Data.OracleClient
Imports System.Runtime.CompilerServices
Imports DefLanguage
Imports ICAT_OracleDAO
Imports Microsoft.VisualBasic.CompilerServices
Imports RMA_Model

Public Class ctlRMA
#Region "Class:RepairBOM:維修中心零件費用檔"
    Public Class RepairBOM
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得RepairBOM資料
        ''' </summary>
        ''' <param name="RpBOM_CompNo">公司代碼</param>
        ''' <param name="PartNo">PartNo</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 RepairBOMDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByPartNo(ByVal RpBOM_CompNo As String, ByVal PartNo As String, Optional ByVal OrderBY As String = "") As RmaDTO.RepairBOMDataTable
            Dim dtRepairBom As New RmaDTO.RepairBOMDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RPBOM_PARTNO") = "RPBOM_oldPARTNO"

                If OrderBY.Trim = "" Then
                    OrderBY = " RPBOM_PARTNO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If PartNo.Trim <> "" Then
                    PartNo = "%" & PartNo.Trim() & "%"
                    oQuery.addWHERE("RPBOM_PARTNO", ":RPBOM_PARTNO", PartNo, OracleType.VarChar)
                    sCondition = sCondition & " AND RPBOM_PARTNO like :RPBOM_PARTNO"
                End If

                oQuery.addWHERE("RPBOM_COMPNO", ":RPBOM_COMPNO", RpBOM_CompNo, OracleType.VarChar)
                sCondition = sCondition & " AND RPBOM_COMPNO=:RPBOM_COMPNO"

                sSQL = "SELECT * FROM REPAIRBOM WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairBom, HasExceColumn)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairBom
        End Function

        ''' <summary>
        ''' 取得RepairBOM資料
        ''' </summary>
        ''' <param name="Comp_No">公司代碼</param>
        ''' <param name="PartNo">PartNo</param>
        ''' <param name="Location">定義排序</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 RepairBOMDataTable</returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal Comp_No As String, ByVal PartNo As String, ByVal Location As String, Optional ByVal OrderBY As String = "") As RmaDTO.RepairBOMDataTable
            Dim dtRepairBom As New RmaDTO.RepairBOMDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RPBOM_PARTNO") = "RPBOM_oldPARTNO"

                If OrderBY.Trim = "" Then
                    OrderBY = " RPBOM_PARTNO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RPBOM_COMPNO", ":RPBOM_COMPNO", Comp_No.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RPBOM_COMPNO=:RPBOM_COMPNO"

                oQuery.addWHERE("RPBOM_PARTNO", ":RPBOM_PARTNO", PartNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RPBOM_PARTNO =:RPBOM_PARTNO"

                If Location.Trim <> "" Then
                    oQuery.addWHERE("RPBOM_LOCATION", ":RPBOM_LOCATION", Location.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND RPBOM_LOCATION =:RPBOM_LOCATION"
                End If

                sSQL = "SELECT * FROM REPAIRBOM WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairBom, HasExceColumn)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairBom
        End Function

        ''' <summary>
        ''' 取得RepairBOM 的 Parts 資料
        ''' </summary>
        ''' <param name="RepairBOM_No">BOM 維修中心代碼</param>
        ''' <param name="KeyWord">公司代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 RepairBOMDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairPart(ByVal RepairBOM_No As String, ByVal KeyWord As String, Optional ByVal OrderBY As String = "") As RmaDTO.RepairBOMDataTable
            Dim dtRepairBom As New RmaDTO.RepairBOMDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RPBOM_PARTNO") = "RPBOM_oldPARTNO"


                If OrderBY.Trim = "" Then
                    OrderBY = "RPBOM_PARTNO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If KeyWord.Trim <> "" Then
                    KeyWord = "%" & KeyWord.Trim().ToLower() & "%"
                    oQuery.addWHERE("RPBOM_PARTNO", ":RPBOM_PARTNO", KeyWord, OracleType.VarChar)
                    oQuery.addWHERE("rpbom_desc", ":rpbom_desc", KeyWord, OracleType.VarChar)

                    sCondition = sCondition & " AND (lower(RPBOM_PARTNO) like :RPBOM_PARTNO"
                    sCondition = sCondition & " OR lower(rpbom_desc) like :rpbom_desc"
                    sCondition = sCondition & " )"
                End If

                sSQL = "SELECT REPAIRBOM.* FROM sl_ima_file,REPAIRBOM " &
                 " WHERE sl_ima01 = rpbom_partno AND sl_ima03 = 'Y' AND rpbom_compno = '" & RepairBOM_No.Trim() & "' " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairBom, HasExceColumn)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairBom
        End Function

        ''' <summary>
        ''' 取得公司代碼是否存在
        ''' </summary>
        ''' <param name="RPBOM_CompNO">公司代碼</param>
        ''' <param name="new_PartNo">新的零件代碼</param>
        ''' <param name="old_PartNo">舊的零件代碼</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:不存在, True:存在</remarks>
        Private Function chkIsExist(ByVal RPBOM_CompNo As String, ByVal new_PartNo As String, Optional ByVal old_PartNo As String = "") As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RPBOM_COMPNO", ":RPBOM_COMPNO", RPBOM_CompNo.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RPBOM_PARTNO", ":new_PartNo", new_PartNo.Trim(), OracleType.VarChar)

                If old_PartNo.Trim() <> "" Then
                    oQuery.addWHERE("RPBOM_PARTNO", ":old_PartNo", old_PartNo.Trim(), OracleType.VarChar)
                    sCondition = sCondition & " AND RPBOM_PARTNO<>:old_PartNo"
                End If

                Dim sSQL As String = "SELECT * FROM REPAIRBOM " &
                        " WHERE RPBOM_COMPNO=:RPBOM_COMPNO AND RPBOM_PARTNO=:new_PartNo " & sCondition

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
        ''' RepairBOM-存檔
        ''' </summary>
        ''' <param name="dtRepairBOM"></param>
        ''' <remarks></remarks>
        Public Sub Save(ByVal dtRepairBOM As RmaDTO.RepairBOMDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New Query(oConn)

            Dim dtRepairBOM_Add As New RmaDTO.RepairBOMDataTable
            Dim dtRepairBOM_Edit As New RmaDTO.RepairBOMDataTable

            oConn.Open()

            Try
                dtRepairBOM_Add = dtRepairBOM.Copy
                dtRepairBOM_Edit = dtRepairBOM.Copy

                Dim dvRepairBOM_Add As DataView = dtRepairBOM_Add.DefaultView()
                dvRepairBOM_Add.RowFilter = "RPBOM_oldPARTNO<>''"
                Do While dvRepairBOM_Add.Count > 0
                    dvRepairBOM_Add.Delete(0)
                Loop

                Dim dvRepairBOM_Edit As DataView = dtRepairBOM_Edit.DefaultView()
                dvRepairBOM_Edit.RowFilter = "RPBOM_oldPARTNO=''"
                Do While dvRepairBOM_Edit.Count > 0
                    dvRepairBOM_Edit.Delete(0)
                Loop

                '新增時檢核資料是否存在
                For i = 0 To dtRepairBOM_Add.Rows.Count - 1
                    Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM_Add.Rows(i)
                    If chkIsExist(dr.RPBOM_COMPNO, dr.RPBOM_PARTNO) = True Then
                        Throw New ArgumentException(_oLanguage.getText("RepairCenter", "043", ctlLanguage.eumType.Validator))
                    End If
                Next

                '修改時檢核資料是否存在
                For i = 0 To dtRepairBOM_Edit.Rows.Count - 1
                    Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM_Edit.Rows(i)
                    If chkIsExist(dr.RPBOM_COMPNO, dr.RPBOM_PARTNO, dr.RPBOM_oldPARTNO) = True Then
                        Throw New ArgumentException(_oLanguage.getText("RepairCenter", "043", ctlLanguage.eumType.Validator))
                    End If
                Next

                oConn.BeginTransaction()

                If dtRepairBOM_Add.Rows.Count > 0 Then
                    Call SaveAdd(oExecute, dtRepairBOM_Add)
                End If

                If dtRepairBOM_Edit.Rows.Count > 0 Then
                    Call SaveEdit(oExecute, dtRepairBOM_Edit)
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
        ''' RepairBOM-新增
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRepairBOM">傳入要新增的資料</param>
        ''' <remarks></remarks>
        Private Sub SaveAdd(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRepairBOM As RmaDTO.RepairBOMDataTable)
            Dim i As Integer = 0
            Dim PartNo As String = ""


            Try
                For i = 0 To dtRepairBOM.Rows.Count - 1
                    Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM.Rows(i)

                    oExecute.addParameter("RPBOM_COMPNO", dr.RPBOM_COMPNO, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_PARTNO", dr.RPBOM_PARTNO, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_LOCATION", dr.RPBOM_LOCATION, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_DESC", dr.RPBOM_DESC, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_MATERIALCOST", dr.RPBOM_MATERIALCOST, OracleType.Double)

                    oExecute.addParameter("RPBOM_AD", dr.RPBOM_AD, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_ADNAME", dr.RPBOM_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_CSTMP", dr.RPBOM_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("RPBOM_LUAD", dr.RPBOM_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_LUADNAME", dr.RPBOM_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_LUSTMP", dr.RPBOM_LUSTMP, OracleType.DateTime)

                    oExecute.Command("REPAIRBOM", Execute.eumCommandType.AddNew)
                Next


            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' RepairBOM-修改
        ''' </summary>
        ''' <param name="oExecute">傳入ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRepairBOM">傳入dtRepairBOM</param>
        ''' <remarks></remarks>
        Private Sub SaveEdit(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRepairBOM As RmaDTO.RepairBOMDataTable)
            Dim i As Integer = 0


            Try

                For i = 0 To dtRepairBOM.Count - 1
                    Dim dr As RmaDTO.RepairBOMRow = dtRepairBOM.Rows(i)

                    oExecute.addParameter("RPBOM_PARTNO", dr.RPBOM_PARTNO, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_LOCATION", dr.RPBOM_LOCATION, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_DESC", dr.RPBOM_DESC, OracleType.VarChar)
                    oExecute.addParameter("RPBOM_MATERIALCOST", dr.RPBOM_MATERIALCOST, OracleType.Double)

                    'oExecute.addParameter("RPBOM_AD", dr.RPBOM_AD, OracleType.NVarChar)
                    'oExecute.addParameter("RPBOM_ADNAME", dr.RPBOM_ADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("RPBOM_CSTMP", dr.RPBOM_CSTMP, OracleType.DateTime)

                    oExecute.addParameter("RPBOM_LUAD", dr.RPBOM_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_LUADNAME", dr.RPBOM_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RPBOM_LUSTMP", dr.RPBOM_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RPBOM_COMPNO", dr.RPBOM_COMPNO.ToString().Trim(), OracleType.VarChar)
                    oExecute.addWHERE("RPBOM_PARTNO", dr.RPBOM_oldPARTNO.ToString().Trim(), OracleType.VarChar)

                    oExecute.Command("REPAIRBOM", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' RepairBOM-刪除
        ''' </summary>
        ''' <param name="RPBOM_COMPNO">傳入公司代碼</param>
        ''' <param name="RPBOM_PARTNO">傳入零件代碼</param>
        ''' <remarks></remarks>
        Public Sub Delete(ByVal RPBOM_COMPNO As String, ByVal RPBOM_PARTNO As String)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addWHERE("RPBOM_COMPNO", RPBOM_COMPNO.ToString().Trim(), OracleType.VarChar)
                oExecute.addWHERE("RPBOM_PARTNO", RPBOM_PARTNO.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("REPAIRBOM", Execute.eumCommandType.Delete)
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
#End Region

#Region "Class:Export:出貨資料"
    Public Class Export
        Dim _isDebug As String = ConfigurationSettings.AppSettings("isDebug")

        '' <summary>
        '' 取得 Warranty 資料  -- 已不用 2020/09/15
        '' </summary>
        '' <param name="SerialNo">Serial No</param>
        '' <param name="CustNo">客戶代碼-->已不使用了</param>
        '' <returns></returns>
        '' <remarks></remarks>
        'Public Function getWarranty(ByVal SerialNo As String, ByVal CustNo As String) As String
        '    Dim retval As String = ""
        '    Dim sSQL As String = ""
        '    Dim sCondition As String = ""

        '    Dim oConn As New Connection
        '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        '    Dim dt As New DataTable

        '    oConn.Open()
        '    Try

        '        oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
        '        sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"

        '        'oQuery.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", CustNo.Trim(), OracleType.VarChar)
        '        'sCondition = sCondition & " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"

        '        'Dim sSQL As String = "SELECT EXPORT_SERIALNO, max(EXPORT_WARRANTY_DATE) EXPORT_WARRANTY_DATE " & _
        '        '        " FROM EXPORT WHERE 1=1" & sCondition & _
        '        '        " group by EXPORT_SERIALNO"


        '        sSQL = " SELECT * FROM " &
        '            "(" &
        '            "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
        '            "       FROM EXPORT WHERE 1=1" & sCondition &
        '            "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
        '            ") WHERE ROWNUM < 2"

        '        dt = oQuery.ExecuteDT(sSQL)
        '        If dt.Rows.Count > 0 Then
        '            retval = dt.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
        '        End If

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        oConn.Close()
        '        oConn.Dispose()
        '    End Try


        '    Return retval
        'End Function

        Public Function getEXPORT(ByVal SerialNo As String) As String
            Dim retval As String = ""
            Dim sSQL As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim dt_D As New DataTable
            Dim dt_CW As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)

                sSQL = " SELECT EXPORT_WAR_ID FROM EXPORT where 1 = 1 AND EXPORT_SERIALNO=:EXPORT_SERIALNO"

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("EXPORT_WAR_ID").ToString()
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
        ''' 取得保固最大日期
        ''' </summary>
        ''' <param name="SerialNo"></param>
        ''' <returns>最大保固日</returns>
        Public Function getMaxWarranty(ByVal SerialNo As String, ByVal CustNo As String, ByVal Company_No As String) As String
            Dim retval As String = ""
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim dt_D As New DataTable
            Dim dt_CW As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"

                sSQL = " SELECT MAX(CW_EDATE) CW_EDATE FROM " &
                    "(" &
                    "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE , CW_EDATE " &
                    "       FROM EXPORT WHERE 1=1" & sCondition &
                    "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                    ")"
                dt_CW = oQuery.ExecuteDT(sSQL)
                If dt_CW.Rows(0)("CW_EDATE").ToString().Trim() <> "" Then
                    retval = dt_CW.Rows(0)("CW_EDATE").ToString()
                Else
                    '20200910 wisely 若有DC07賽弗萊 出貨須判斷維修中心是否CL_CHINA 若是則抓最大 若不是則抓原始DC07出貨那張
                    oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                    sSQL = "SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE FROM EXPORT WHERE EXPORT_SERIALNO =:EXPORT_SERIALNO AND EXPORT_CUSTNO ='DC07' "
                    dt_D = oQuery.ExecuteDT(sSQL)
                    If dt_D.Rows.Count > 0 Then
                        If Company_No <> "CL_CHINA" Then
                            retval = dt_D.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                        Else
                            oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                            sCondition = " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"
                            sSQL = " SELECT * FROM " &
                                   "(" &
                                   "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                                   "       FROM EXPORT WHERE 1=1" & sCondition &
                                   "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                                   ") WHERE ROWNUM < 2"

                            dt = oQuery.ExecuteDT(sSQL)
                            If dt.Rows.Count > 0 Then
                                retval = dt.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                            End If
                        End If
                    Else
                        oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                        sCondition = " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"
                        sSQL = " SELECT * FROM " &
                               "(" &
                               "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                               "       FROM EXPORT WHERE 1=1" & sCondition &
                               "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                               ") WHERE ROWNUM < 2"

                        dt = oQuery.ExecuteDT(sSQL)
                        If dt.Rows.Count > 0 Then
                            retval = dt.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                        End If
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
        ''' 取得保固最大日期
        ''' </summary>
        ''' <param name="SerialNo"></param>
        ''' <returns>最大保固日</returns>
        Public Function getMaxWarranty(ByVal PartNo As String, ByVal SerialNo As String, ByVal CustNo As String, ByVal Company_No As String) As String
            Dim retval As String = ""
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim dt_D As New DataTable
            Dim dt_CW As New DataTable

            oConn.Open()
            Try
                PartNo = "%" + PartNo.Trim() + "%"
                SerialNo = "%" + SerialNo.Trim() + "%"
                oQuery.addWHERE("EXPORT_PARTNO", ":EXPORT_PARTNO", PartNo.Trim(), OracleType.VarChar)
                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_PARTNO LIKE :EXPORT_PARTNO AND  EXPORT_SERIALNO LIKE :EXPORT_SERIALNO "

                sSQL = " SELECT MAX(CW_EDATE) CW_EDATE FROM " &
                    "(" &
                    "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE , CW_EDATE " &
                    "       FROM EXPORT WHERE 1=1" & sCondition &
                    "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                    ")"
                dt_CW = oQuery.ExecuteDT(sSQL)
                If dt_CW.Rows(0)("CW_EDATE").ToString().Trim() <> "" Then
                    retval = dt_CW.Rows(0)("CW_EDATE").ToString()
                Else
                    '20200910 wisely 若有DC07賽弗萊 出貨須判斷維修中心是否CL_CHINA 若是則抓最大 若不是則抓原始DC07出貨那張
                    oQuery.addWHERE("EXPORT_PARTNO", ":EXPORT_PARTNO", PartNo.Trim(), OracleType.VarChar)
                    oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                    sSQL = "SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE FROM  EXPORT WHERE EXPORT_PARTNO LIKE :EXPORT_PARTNO AND EXPORT_SERIALNO LIKE :EXPORT_SERIALNO AND EXPORT_CUSTNO ='DC07' "

                    dt_D = oQuery.ExecuteDT(sSQL)
                    If dt_D.Rows.Count > 0 Then
                        If Company_No <> "CL_CHINA" Then
                            retval = dt_D.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                        Else
                            oQuery.addWHERE("EXPORT_PARTNO", ":EXPORT_PARTNO", PartNo.Trim(), OracleType.VarChar)
                            oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                            sCondition = " AND (EXPORT_PARTNO LIKE :EXPORT_PARTNO OR EXPORT_SERIALNO LIKE :EXPORT_SERIALNO)"

                            sSQL = " SELECT * FROM " &
                                   "(" &
                                   "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                                   "       FROM EXPORT WHERE 1=1" & sCondition &
                                   "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                                   ") WHERE ROWNUM < 2"

                            dt = oQuery.ExecuteDT(sSQL)
                            If dt.Rows.Count > 0 Then
                                retval = dt.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                            End If
                        End If
                    Else
                        oQuery.addWHERE("EXPORT_PARTNO", ":EXPORT_PARTNO", PartNo.Trim(), OracleType.VarChar)
                        oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                        sCondition = " AND EXPORT_PARTNO LIKE :EXPORT_PARTNO AND EXPORT_SERIALNO LIKE :EXPORT_SERIALNO "

                        sSQL = " SELECT * FROM " &
                               "(" &
                               "   SELECT EXPORT_SERIALNO, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                               "       FROM EXPORT WHERE 1=1" & sCondition &
                               "   ORDER BY EXPORT_SHIPPING_TIME desc , EXPORT_WARRANTY_DATE desc" &
                               ") WHERE ROWNUM < 2"

                        dt = oQuery.ExecuteDT(sSQL)
                        If dt.Rows.Count > 0 Then
                            retval = dt.Rows(0)("EXPORT_WARRANTY_DATE").ToString()
                        End If
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

        '''' <summary>
        '''' 取得 Warranty 資料      2020/09/15 已不用
        '''' </summary>
        '''' <param name="PartNo">Part No</param>
        '''' <param name="SerialNo">Serial No</param>
        '''' <param name="CustNo">客戶代碼</param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function getWarranty(ByVal PartNo As String, ByVal SerialNo As String, ByVal CustNo As String) As String
        '    Dim retval As String = ""
        '    Dim sCondition As String = ""

        '    Dim oConn As New Connection
        '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        '    Dim dtEXPORT As New DataTable

        '    oConn.Open()
        '    Try
        '        'oQuery.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", CustNo.Trim(), OracleType.VarChar)
        '        'sCondition = sCondition & " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"


        '        oQuery.addWHERE("EXPORT_PARTNO", ":EXPORT_PARTNO", PartNo.ToLower().Trim(), OracleType.VarChar)

        '        If SerialNo.Trim() <> "" Then
        '            oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.ToLower().Trim(), OracleType.VarChar)
        '            sCondition = sCondition & " AND ( lower(EXPORT_PARTNO)=:EXPORT_PARTNO OR lower(EXPORT_SERIALNO)=:EXPORT_SERIALNO)"
        '        Else
        '            sCondition = sCondition & " AND  lower(EXPORT_PARTNO)=:EXPORT_PARTNO"
        '        End If


        '        Dim sSQL As String = "SELECT * FROM EXPORT WHERE 1=1" & sCondition

        '        dtEXPORT = oQuery.ExecuteDT(sSQL)
        '        Dim dvEXPORT As DataView = dtEXPORT.DefaultView()
        '        dvEXPORT.RowFilter = "EXPORT_PARTNO='" & PartNo.Trim() & "' and EXPORT_SERIALNO='" & SerialNo.Trim() & "'"
        '        If dvEXPORT.Count > 0 Then
        '            retval = dvEXPORT(0)("EXPORT_WARRANTY_DATE").ToString()
        '            Exit Try
        '        End If

        '        dvEXPORT.RowFilter = "EXPORT_PARTNO='" & PartNo.Trim() & "'"
        '        If dvEXPORT.Count > 0 Then
        '            retval = dvEXPORT(0)("EXPORT_WARRANTY_DATE").ToString()
        '            Exit Try
        '        End If

        '        dvEXPORT.RowFilter = "EXPORT_SERIALNO='" & SerialNo.Trim() & "'"
        '        If dvEXPORT.Count > 0 Then
        '            retval = dvEXPORT(0)("EXPORT_WARRANTY_DATE").ToString()
        '            Exit Try
        '        End If

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        oConn.Close()
        '        oConn.Dispose()
        '    End Try


        '    Return retval
        'End Function

        Public Function getWarrantyCW(ByVal SerialNo As String, ByVal CustNo As String) As String
            Dim retval As String = ""
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"

                'oQuery.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", CustNo.Trim(), OracleType.VarChar)
                'sCondition = sCondition & " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"

                'sSQL = "SELECT EXPORT_SERIALNO, max(EXPORT_WARRANTY_DATE) EXPORT_WARRANTY_DATE " &
                '        " FROM EXPORT WHERE 1=1" & sCondition &
                '        " group by EXPORT_SERIALNO"

                sSQL = " SELECT MAX(cw_edate) cw_edate FROM " &
                    "(" &
                    "   SELECT EXPORT_SERIALNO,cw_edate, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                    "       FROM EXPORT join WARRSET on export_war_id = war_id WHERE 1=1 " & sCondition &
                    "   ORDER BY EXPORT_SHIPPING_TIME , cw_edate desc" &
                    ")"

                'sSQL = " SELECT MAX(cw_edate) cw_edate FROM " &
                '    "(" &
                '    "   SELECT EXPORT_SERIALNO,cw_edate, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                '    "       FROM EXPORT WHERE 1=1 AND NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') <> 'M0' " & sCondition &
                '    "   ORDER BY EXPORT_SHIPPING_TIME , cw_edate desc" &
                '    ")"

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("cw_edate").ToString()
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return retval
        End Function

        Public Function getRepairCnt(ByVal SerialNo As String, ByVal CustNo As String) As String
            Dim retval As String = "0"
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("rmad_serialno", ":rmad_serialno", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND rmad_serialno=:rmad_serialno"

                sSQL = "select nvl(count(1),0) Cnt from rmadetail where rmad_status<>91 and rmad_mark=0" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("Cnt").ToString()
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return retval
        End Function

        Public Function getSpecialWarrantyID(ByVal RMADID As String) As String
            Dim retval As String = "$A$"
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMADID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND e.RMAD_ID=:RMAD_ID"

                sSQL = "SELECT a.sw_id"
                sSQL = sSQL & " FROM SWSET a,PORECORD b,WARRANTYITEM c,WARRANTYSERIAL d,RMADetail e"
                sSQL = sSQL & " WHERE(b.POR_SN = d.WATS_SN And c.WATI_VER = a.SW_ID)"
                sSQL = sSQL & " and c.WATI_WATYNO= d.WATS_WATYNO "
                sSQL = sSQL & " and c.WATI_SEQ =d.WATS_WATYSEQ"
                sSQL = sSQL & " and e.RMAD_SERIALNO=d.WATS_SN"
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("sw_id").ToString()
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retval
        End Function

        Public Function getWarrantySW(ByVal SerialNo As String, ByVal CustNo As String) As String
            Dim retval As String = ""
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"

                'oQuery.addWHERE("EXPORT_CUSTNO", ":EXPORT_CUSTNO", CustNo.Trim(), OracleType.VarChar)
                'sCondition = sCondition & " AND EXPORT_CUSTNO=:EXPORT_CUSTNO"

                'Dim sSQL As String = "SELECT EXPORT_SERIALNO, max(EXPORT_WARRANTY_DATE) EXPORT_WARRANTY_DATE " & _
                '        " FROM EXPORT WHERE 1=1" & sCondition & _
                '        " group by EXPORT_SERIALNO"


                sSQL = " SELECT * FROM " &
                    "(" &
                    "   SELECT EXPORT_SERIALNO,sw_edate, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE" &
                    "       FROM EXPORT WHERE 1=1" & sCondition &
                    "   ORDER BY EXPORT_SHIPPING_TIME desc" &
                    ") WHERE ROWNUM < 2"

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("sw_edate").ToString()
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
        ''' 取得 Warranty 資料
        ''' </summary>
        ''' <param name="SerialNo">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getWarrantyStart(ByVal SerialNo As String) As String
            Dim retval As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dtEXPORT As New DataTable

            oConn.Open()
            Try
                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo, OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO "

                Dim sSQL As String = "SELECT * FROM EXPORT WHERE 1=1 " & sCondition & "ORDER BY EXPORT_WARRANTY_DATE DESC"

                dtEXPORT = oQuery.ExecuteDT(sSQL)
                Dim dvEXPORT As DataView = dtEXPORT.DefaultView()
                If dvEXPORT.Count > 0 Then
                    retval = dvEXPORT(0)("EXPORT_SHIPPING_TIME").ToString()
                    Exit Try
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
        ''' 用 Serial No 取得 Model No
        ''' </summary>
        ''' <param name="SerialNo">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getModelNo(ByVal SerialNo As String) As String
            Dim retval As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try

                oQuery.addWHERE("export_serialno", ":export_serialno", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND export_serialno=:export_serialno"

                Dim sTableName As String = "cipherlab.tc_oae_file"
                'If _isDebug = True Then
                '    sTableName = "tc_oae_file"
                'End If
                Dim sSQL As String = "select  tc_oae240  from export," & sTableName & " where export_partno=tc_oae010" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("tc_oae240").ToString()
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
        ''' 用 Serial No,Comp_no,CUNO 取得 Model No
        ''' </summary>
        ''' <param name="SerialNo">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getModelNo(ByVal SerialNo As String, ByVal sComp_no As String, ByVal sCu_no As String) As String
            Dim retval As String = ""
            Dim sCondition As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try
                oQuery.addWHERE("RMM_COMPNO", ":RMM_COMPNO", sComp_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMM_CUNO", ":RMM_CUNO", sCu_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("export_serialno", ":export_serialno", SerialNo.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND a.export_serialno=:export_serialno"


                Dim sSQL As String = "SELECT DECODE(NVL(c.RMM_MMODELNO,' '),' ',b.tc_oae240,c.RMM_MMODELNO) MODELNO  from export a " & vbNewLine
                sSQL += "JOIN cipherlab.tc_oae_file b ON a.export_partno=b.tc_oae010 " & vbNewLine
                sSQL += "LEFT JOIN RMAMODELMAPPING c ON c.RMM_COMPNO=:RMM_COMPNO AND c.RMM_CUNO=:RMM_CUNO AND c.RMM_CMODELNO = b.tc_oae240 " & vbNewLine
                sSQL += " WHERE 1=1 " & vbNewLine
                sSQL += sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("MODELNO").ToString()
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
        ''' 用 Comp_no,CUNO,CMODELNO 取得 Mplus Model No
        ''' </summary>
        ''' <param name="ModelNO">Serial No</param>
        ''' <param name="sComp_no">Serial No</param>
        ''' <param name="sCu_no">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMModelNo(ByVal ModelNO As String, ByVal sComp_no As String, ByVal sCu_no As String) As String
            Dim retval As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try
                oQuery.addWHERE("RMM_COMPNO", ":RMM_COMPNO", sComp_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMM_CUNO", ":RMM_CUNO", sCu_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMM_CMODELNO", ":RMM_CMODELNO", ModelNO.Trim(), OracleType.VarChar)


                Dim sSQL As String = "SELECT RMM_MMODELNO FROM RMAMODELMAPPING " & vbNewLine
                sSQL += "WHERE RMM_CMODELNO=:RMM_CMODELNO AND RMM_COMPNO=:RMM_COMPNO AND RMM_CUNO=:RMM_CUNO" & vbNewLine

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("RMM_MMODELNO").ToString()
                Else
                    retval = ModelNO
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
        ''' 用 Comp_no,CUNO,CMODELNO 取得 Cippher Model No
        ''' </summary>
        ''' <param name="ModelNO">Serial No</param>
        ''' <param name="sComp_no">Serial No</param>
        ''' <param name="sCu_no">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getCModelNo(ByVal ModelNO As String, ByVal sComp_no As String, ByVal sCu_no As String) As String
            Dim retval As String = ""

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try
                oQuery.addWHERE("RMM_COMPNO", ":RMM_COMPNO", sComp_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMM_CUNO", ":RMM_CUNO", sCu_no.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMM_MMODELNO", ":RMM_MMODELNO", ModelNO.Trim(), OracleType.VarChar)

                Dim sSQL As String = "SELECT RMM_CMODELNO FROM RMAMODELMAPPING " & vbNewLine
                sSQL += "WHERE RMM_MMODELNO=:RMM_MMODELNO AND RMM_COMPNO=:RMM_COMPNO AND RMM_CUNO=:RMM_CUNO" & vbNewLine

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("RMM_CMODELNO").ToString()
                Else
                    retval = ModelNO
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
        ''' 富士通_MOMO專案RMA專屬檢核
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getSerialControl() As DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable

            oConn.Open()
            Try
                Dim sSQL = "
                            WITH cipher_oeafile AS(
                                SELECT oea01
                                FROM cipherlab.oea_file 
                                WHERE OEA03 = 'DT728' AND UPPER(ta_oea040) LIKE UPPER('%MOMO%')
                            )

                            SELECT EXPORT_SERIALNO
                            FROM export e 
                            WHERE  e.export_custno = 'DT728'
                            AND SUBSTR(e.export_ordernumber,1,14) in (select oea01 from cipher_oeafile)
                            GROUP BY EXPORT_SERIALNO
                        "

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
#End Region

#Region "Class:Export_Primal:原型機種  資料"
    Public Class Export_Primal

        ''' <summary>
        ''' 取得 原型機種 資料
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryAll() As RmaDTO.EXPORT_PRIMALSNDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim dtPrimal As New RmaDTO.EXPORT_PRIMALSNDataTable

            oConn.Open()
            Try

                Dim sSQL As String = "SELECT EXPAR_PARTSNO, EXPAR_PRIMALSN " &
                        " FROM EXPORT_PRIMALSN " &
                        " ORDER BY EXPAR_PRIMALSN"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtPrimal)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return dtPrimal
        End Function
    End Class
#End Region

#Region "Class:Model"
    Public Class Model

        ''' <summary>
        ''' 取得 Model資料
        ''' </summary>
        ''' <param name="Model">Model No,Model Name</param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal Model As String, Optional ByVal OrderBY As String = "") As RmaDTO.ModelDataTable
            Dim dtModel As New RmaDTO.ModelDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim sModelName As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " MODELNO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If Model.ToString().Trim() <> "" Then
                    sModelName = "%" & Model & "%"
                    oQuery.addWHERE("MODELNO", ":MODELNO", Model, OracleType.VarChar)
                    oQuery.addWHERE("MODELNAME", ":MODELNAME", sModelName, OracleType.VarChar)
                    sCondition = sCondition & " AND (MODELNO =:MODELNO OR MODELNAME like :MODELNAME)"
                End If

                sSQL = "SELECT * FROM MODEL WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtModel)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtModel
        End Function

        ''' <summary>
        ''' 檢核Model是否存在
        ''' </summary>
        ''' <param name="Model">Model</param>
        ''' <returns>傳回 Boolean</returns>
        ''' <remarks>False:不存在, True:存在</remarks>
        Public Function chkModel(ByVal Model As String) As Boolean
            Dim retval As Boolean = False
            Dim sSQL As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("MODELNO", ":MODELNO", Model, OracleType.VarChar)
                sCondition = sCondition & " AND MODELNO =:MODELNO"

                sSQL = "SELECT * FROM MODEL WHERE 1=1" & sCondition

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

#Region "Class:Requested:需求單"
    Public Class Requested
        Dim _oLanguage As New ctlLanguage

        ''' <summary>
        ''' 取得 RMA_REMARK
        ''' </summary>
        Public Function QueryByRMA_REMARK(ByVal RMA_NO As String) As String

            Dim context As String = ""


            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sSQL = " SELECT  t.RMA_REMARK  FROM ( SELECT RMA_REMARK FROM RMA   where RMA_NO =:RMA_NO  ) t WHERE ROWNUM < 2  "
                dt = oQuery.ExecuteDT(sSQL)
                context = dt.Rows(0).Item("RMA_REMARK").ToString().Trim()

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return context

        End Function

        ''' <summary>
        ''' 取得SerialNO 
        ''' </summary>
        ''' <param name="RMAD_SERIALNO">SerialNumber</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_SERIALNO(ByVal RMAD_SERIALNO As String) As DataTable
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            Dim result As DataTable = New DataTable()
            Dim text As String = ""
            Dim text2 As String = ""
            connection.Open()

            Try
                query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO, OracleType.VarChar)
                text = " SELECT COUNT( RMAD_SERIALNO ) FROM RMADETAIL   where RMAD_SERIALNO =:RMAD_SERIALNO AND RMAD_CSTMP > SYSDATE  - 1 AND RMAD_CSTMP < SYSDATE  + 1  "
                result = query.ExecuteDT(text)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return result
        End Function

        ''' <summary>
        ''' 取得客戶目前 最新十筆 RMA 
        ''' </summary>
        Public Function QueryByRMAROWNUM(ByVal RMA_CUNO As String) As DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", RMA_CUNO, OracleType.VarChar)
                sSQL = " SELECT  t.RMA_NO  FROM ( SELECT RMA_NO FROM RMA   where RMA_CUNO =:RMA_CUNO ORDER BY RMA_CSTMP desc ) t WHERE ROWNUM < 10  "
                dt = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt

        End Function

        ''' <summary>
        ''' 取得 RMA 單頭資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMAHead(ByVal RMANo As String) As RmaDTO.RMADataTable

            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO =:RMA_NO"

                sSQL = "SELECT * FROM RMA WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMA)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRMA

        End Function

        ''' <summary>
        ''' 取得 RMA 單身資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>RMADetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRMADetail(ByVal RMANo As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMADetailDataTable
            Dim dtRMADetail As New RmaDTO.RMADetailDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO =:RMAD_RMANO"

                'sSQL = "SELECT * FROM RMADetail WHERE 1=1" & sCondition & OrderBY
                sSQL = "SELECT * FROM RMADetail WHERE RMAD_MARK=0 " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMADetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRMADetail

        End Function

        ''' <summary>
        ''' 取得 RMA 單身資料
        ''' </summary>
        ''' <param name="RMADID">RMAD ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>RMADetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByDetail(ByVal RMADID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMADetailDataTable
            Dim i As Integer = 0
            Dim dtRMADetail As New RmaDTO.RMADetailDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY


                Dim sCondition_RMADID As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRMADID() As String = RMADID.Split(",")
                For i = 0 To arrRMADID.Length - 1
                    oQuery.addWHERE("RMAD_ID", ":RMAD_ID" & i.ToString, arrRMADID(i).Trim(), OracleType.NVarChar)

                    If sCondition_RMADID.Trim <> "" Then
                        sCondition_RMADID = sCondition_RMADID & " OR "
                    End If
                    sCondition_RMADID = sCondition_RMADID & " RMAD_ID =:RMAD_ID" & i.ToString
                Next
                sCondition = sCondition & sCondition_RMADID & ")"

                sSQL = "SELECT * FROM RMADetail WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMADetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRMADetail

        End Function

        ''' <summary>
        ''' 抓取序號的保固資料
        ''' </summary>
        ''' <param name="EXPORT_SERIALNO"></param>
        ''' <returns></returns>
        Public Function QueryByExport(ByVal EXPORT_SERIALNO As String) As RmaDTO.ExportDataTable
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dtExport As New RmaDTO.ExportDataTable
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO, OracleType.VarChar)
                sCondition = " AND EXPORT_SERIALNO =:EXPORT_SERIALNO"

                sSQL = "SELECT * FROM EXPORT WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                TransferDataTable(dt, dtExport)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtExport
        End Function

        ''' <summary>
        ''' 判斷是否為EndUser
        ''' </summary>
        ''' <param name="CU_NO">客戶代號</param>
        ''' <returns></returns>
        Public Function IsEndUser(ByVal CU_NO As String, Optional ByVal Paypal As String = "") As DataTable
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("CU_NO", ":CU_NO", CU_NO, OracleType.VarChar)
                sCondition = sCondition & " AND CU_NO =:CU_NO"
                If Paypal <> "" Then
                    oQuery.addWHERE("CU_TIPTOP_ID", ":Paypal", Paypal, OracleType.VarChar)
                    sCondition = sCondition & " AND CU_TIPTOP_ID =:Paypal"
                End If

                sSQL = " SELECT * " &
                       " FROM customer a INNER JOIN enduser b ON b.EU_NO = a.CU_NO " &
                       " WHERE 1 = 1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                'If dt.Rows.Count > 0 Then
                '    retval = True
                'End If

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return dt

        End Function

        ''' <summary>
        ''' 取得Enduser要維修資訊
        ''' </summary>
        ''' <param name="Transation_id">ID</param>
        ''' <returns></returns>
        Public Function GetRepairData(ByVal Transation_id As String) As DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                sSQL = " SELECT a.* FROM RMA_AHEAD a " &
                       " JOIN CIPHERLABPAY.ORDERH b ON b.TRANSATION_ID=:TRANSATION_ID AND b.PO_NO = a.RMA_NO "

                oQuery.addWHERE("TRANSATION_ID", ":TRANSATION_ID", Transation_id, OracleType.VarChar)


                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return dt

        End Function

        ''' <summary>
        ''' 判斷若其中有一筆為全保,RMADETAIL.RMAD_ISCW=1
        ''' 加入保固資訊後面可以判斷 by buck modify 2026/01/02
        ''' </summary>
        ''' <param name="RMA_ID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Function chkISCWarrantyData(ByVal RMA_ID As String) As DataTable
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID =:RMA_ID"

                sSQL = sSQL & " SELECT * "
                sSQL = sSQL & " FROM RMA "
                sSQL = sSQL & " INNER JOIN RMADetail ON RMA.RMA_NO = RMADetail.RMAD_RMANO"
                sSQL = sSQL & "     AND RMAD_ISCW=1 AND RMAD_MARK=0 "
                '加入保固資訊後面可以判斷 by buck modify 2026/01/02 begin
                sSQL = sSQL & " LEFT JOIN Export on RMADetail.RMAD_SERIALNO = Export.EXPORT_SERIALNO "
                sSQL = sSQL & " LEFT JOIN warrset on export_war_id = war_id "
                '加入保固資訊後面可以判斷 by buck modify 2026/01/02 end
                sSQL = sSQL & " WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt

        End Function

        ''' <summary>
        ''' 判斷若其中有一筆為全保,RMADETAIL.RMAD_ISCW=1
        ''' </summary>
        ''' <param name="RMA_ID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Function chkISCWarranty(ByVal RMA_ID As String) As Boolean
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID =:RMA_ID"

                sSQL = sSQL & " SELECT * "
                sSQL = sSQL & " FROM RMA "
                sSQL = sSQL & " INNER JOIN RMADetail ON RMA.RMA_NO = RMADetail.RMAD_RMANO"
                sSQL = sSQL & "     AND RMAD_ISCW=1 AND RMAD_MARK=0 "
                sSQL = sSQL & " WHERE 1=1" & sCondition

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
        ''' 判斷全保是否免運
        ''' </summary>
        ''' <param name="RMA_ID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Function chkISCWarrantyFee(ByVal RMA_ID As String) As Boolean
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID =:RMA_ID"

                'modi by hugh 20231108, 修改 CW_EDATE需在保固內, 才會付全保運費
                'modi by Hugh 20240126, 修改 war_type的抓法, 改到 warrset 裡抓, 而不是從保固單字串裡抓
                sSQL = sSQL & " SELECT * "
                sSQL = sSQL & " FROM RMA "
                sSQL = sSQL & " INNER JOIN RMADetail ON RMA.RMA_NO = RMADetail.RMAD_RMANO"
                sSQL = sSQL & "     AND RMAD_ISCW=1 AND RMAD_MARK=0 "
                sSQL = sSQL & " JOIN (SELECT EXPORT_SERIALNO "
                sSQL = sSQL & "       FROM EXPORT join warrset on export_war_id = war_id"
                sSQL = sSQL & "       where 1=1 "
                'sSQL = sSQL & "  AND EXPORT_SERIALNO=:EXPORT_SERIALNO "
                sSQL = sSQL & "  AND war_type in ('P0', 'PB', 'CW') "
                sSQL = sSQL & "  AND ( CW_EDATE > sysdate) ) b ON b.EXPORT_SERIALNO =RMADetail.RMAD_SERIALNO "

                sSQL = sSQL & " WHERE 1=1" & sCondition

                '判斷發全保信
                '           Select Case EXPORT_SERIALNO,cw_edate, EXPORT_SHIPPING_TIME , EXPORT_WARRANTY_DATE
                '    From EXPORT Where 1 = 1
                '    And EXPORT_SERIALNO='FW12230006094' 
                '    And (NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'P0' OR NVL(SUBSTR(EXPORT_WAR_ID,5,2),' ') = 'CW')
                'ORDER BY EXPORT_SHIPPING_TIME , cw_edate desc

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
        ''' 取得 RMA 單號
        ''' </summary>
        ''' <param name="RMA_ID">RMA No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRMANo(ByVal RMA_ID As String) As String
            Dim retval As String = ""
            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID =:RMA_ID"

                sSQL = "SELECT * FROM RMA WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("RMA_NO").ToString().Trim()
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
        ''' 存檔-新增
        ''' </summary>
        ''' <param name="dtRMA">RMADataTable</param>
        ''' <param name="dtRMADetail">RMADetailDataTable</param>
        ''' <remarks></remarks>
        Public Function SaveByAddNew(ByVal dtRMA As RmaDTO.RMADataTable, ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As String
            Dim RMA_ID As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                'mod by Isaac 建單時可能會重複送出，增加檢核，一小時內相同序號無法重複送出
                Dim iCount As Integer = ChkRMARepeat(dtRMADetail)
                If iCount > 0 Then
                    Throw New ArgumentException(_oLanguage.getText("Common", "084", ctlLanguage.eumType.Validator))
                End If

                Dim sBill As String = AddNew_RMA(oExecute, dtRMA, RMA_ID)
                If sBill = "1" Then
                    Throw New ArgumentException(_oLanguage.getText("Common", "019", ctlLanguage.eumType.Validator))
                End If
                Call AddNew_RMADetail(oExecute, sBill, dtRMADetail)

                oConn.Commit()

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return RMA_ID
        End Function

        ''' <summary>
        ''' 存檔-修改
        ''' </summary>
        ''' <param name="RMANo">RMA NO</param>
        ''' <param name="dtRMA">RMADataTable</param>
        ''' <param name="dtRMADetail">RMADetailDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveByEdit(ByVal RMANo As String, ByVal dtRMA As RmaDTO.RMADataTable, ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            Dim dtTMPDetail_Add As New RmaDTO.RMADetailDataTable
            Dim dtTMPDetail_Edit As New RmaDTO.RMADetailDataTable

            oConn.Open()
            Try

                dtTMPDetail_Add = dtRMADetail.Copy
                dtTMPDetail_Edit = dtRMADetail.Copy

                Dim dvTMPDetail_Add As DataView = dtTMPDetail_Add.DefaultView()
                dvTMPDetail_Add.RowFilter = "RMAD_RMANO<>''"
                Do While dvTMPDetail_Add.Count > 0
                    dvTMPDetail_Add.Delete(0)
                Loop

                Dim dvTMPDetail_Edit As DataView = dtTMPDetail_Edit.DefaultView()
                dvTMPDetail_Edit.RowFilter = "RMAD_RMANO=''"
                Do While dvTMPDetail_Edit.Count > 0
                    dvTMPDetail_Edit.Delete(0)
                Loop

                oConn.BeginTransaction()
                Call Edit_RMA(oExecute, dtRMA, False)

                Call AddNew_RMADetail(oExecute, RMANo, dtTMPDetail_Add)
                Call Edit_RMADetail(oExecute, dtTMPDetail_Edit)

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
        ''' 
        ''' </summary>
        ''' <param name="dtRMADetail"></param>
        ''' <returns></returns>
        Public Function ChkRMARepeat(ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As Integer
            Dim i As Integer = 0
            Dim dvRMADetail As DataView = dtRMADetail.DefaultView
            Dim x As Integer = 0
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                sCondition += "and RMAD_SERIALNO in ( "
                For i = 0 To dvRMADetail.Count - 1
                    Dim dr As RmaDTO.RMADetailRow = dvRMADetail(i).Row
                    If dr.RMAD_MARK <> 1 Then
                        oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO" & i, dr.RMAD_SERIALNO, OracleType.VarChar)
                        If x = 0 Then
                            sCondition += ":RMAD_SERIALNO" & i
                            'sCondition += "'" & dr.RMAD_SERIALNO & "'"
                        Else
                            sCondition += "," & ":RMAD_SERIALNO" & i
                            'sCondition += ",'" & dr.RMAD_SERIALNO & "'"
                        End If
                        x += 1
                    End If
                Next
                sCondition += " ) "

                sSQL = "SELECT COUNT(*) cnt FROM RMADetail WHERE RMAD_MARK=0 and TRUNC((SYSDATE-RMAD_CSTMP)*24) < 1 " & sCondition
                'oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo, OracleType.VarChar)
                'sCondition = sCondition & " AND RMAD_RMANO =:RMAD_RMANO"

                'sSQL = "SELECT * FROM RMADetail WHERE 1=1" & sCondition & OrderBY
                'sSQL = "SELECT * FROM RMADetail WHERE RMAD_MARK=0 " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)

                'Common.TransferDataTable(dt, dtRMADetail)

            Catch ex As Exception
                Throw New Exception(sSQL)
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return Convert.ToInt32(dt.Rows(0)("cnt").ToString().Trim())

        End Function

        ''' <summary>
        ''' 新增 RMA 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRMA">RMADataTable</param>
        ''' <param name="RMA_ID">RMA ID</param>
        ''' <remarks></remarks>
        Private Function AddNew_RMA(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMA As RmaDTO.RMADataTable, ByRef RMA_ID As String) As String
            Dim i As Integer = 0
            Dim retval As String = ""

            Dim oGuid As Guid = Guid.NewGuid
            Dim oBill As New AutoDocumentNo.Bill

            Try
                Dim dr As RmaDTO.RMARow = dtRMA.Rows(0)

                Dim sGUID As String = oGuid.ToString
                Dim sBill As String = oBill.GetBillNo(oExecute.Connection, dr.RMA_COMPNO.Trim(), dr.RMA_AD, dr.RMA_ADNAME)
                RMA_ID = sGUID

                oExecute.addParameter("RMA_ID", sGUID, OracleType.VarChar)      '系統自動產生唯一識別碼
                oExecute.addParameter("RMA_NO", sBill, OracleType.VarChar)          'RMA 編號

                oExecute.addParameter("RMA_CUNO", dr.RMA_CUNO, OracleType.VarChar)
                oExecute.addParameter("RMA_ACCOUNTID", dr.RMA_ACCOUNTID, OracleType.VarChar)
                oExecute.addParameter("RMA_APPLICANT", dr.RMA_APPLICANT, OracleType.NVarChar)
                oExecute.addParameter("RMA_TEL", dr.RMA_TEL, OracleType.VarChar)
                oExecute.addParameter("RMA_ADDRESS", dr.RMA_ADDRESS, OracleType.NVarChar)
                oExecute.addParameter("RMA_COMPNO", dr.RMA_COMPNO, OracleType.VarChar)
                oExecute.addParameter("RMA_STATUS", dr.RMA_STATUS, OracleType.Int16)
                oExecute.addParameter("RMA_MAIL", dr.RMA_MAIL, OracleType.VarChar)
                oExecute.addParameter("RMA_Remark", dr.RMA_Remark, OracleType.NVarChar)

                oExecute.addParameter("RMA_AD", dr.RMA_AD, OracleType.NVarChar)
                oExecute.addParameter("RMA_ADNAME", dr.RMA_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMA_CSTMP", dr.RMA_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMA_LUAD", dr.RMA_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUADNAME", dr.RMA_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUSTMP", dr.RMA_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMA_MARK", dr.RMA_MARK, OracleType.Int16)

                oExecute.addParameter("RMA_EUCOMPANY", dr.RMA_EUCOMPANY, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUNAME", dr.RMA_EUNAME, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUADDRESS", dr.RMA_EUADDRESS, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUTEL", dr.RMA_EUTEL, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUMAIL", dr.RMA_EUMAIL, OracleType.NVarChar)
                oExecute.addParameter("RMA_PARTSREQUEST", dr.RMA_PARTSREQUEST, OracleType.Int16)

                oExecute.Command("RMA", Execute.eumCommandType.AddNew)
                retval = sBill

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 新增RMA 品項
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="RMA_No"></param>
        ''' <param name="dtRMADetail"></param>
        ''' <remarks></remarks>
        Private Sub AddNew_RMADetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMA_No As String, ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
            Dim i As Integer = 0
            Dim dvRMADetail As DataView = dtRMADetail.DefaultView

            Try
                dvRMADetail.RowFilter = "RMAD_MARK=0"
                For i = 0 To dvRMADetail.Count - 1
                    Dim dr As RmaDTO.RMADetailRow = dvRMADetail(i).Row

                    Dim oGuid As Guid = Guid.NewGuid

                    oExecute.addParameter("RMAD_ID", oGuid.ToString, OracleType.VarChar)        '系統自動產生唯一識別碼
                    oExecute.addParameter("RMAD_RMANO", RMA_No, OracleType.VarChar)             '關聯 RMA.RMA_No

                    oExecute.addParameter("RMAD_SEQ", dr.RMAD_SEQ, OracleType.Int16)

                    oExecute.addParameter("RMAD_MODELNO", dr.RMAD_MODELNO, OracleType.VarChar)
                    oExecute.addParameter("RMAD_SERIALNO", dr.RMAD_SERIALNO, OracleType.VarChar)
                    oExecute.addParameter("RMAD_CUSNAME", dr.RMAD_CUSNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_PARTSN", dr.RMAD_PARTSN, OracleType.NVarChar)

                    If dr.IsRMAD_WARRANTYNull = False Then
                        oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                        '顧客Request Submit之同時, 倘若保固期限大於或等於Submit, 自動記錄 is warranty
                        'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                        If dr.RMAD_WARRANTY > Date.Now Then
                            oExecute.addParameter("RMAD_ISWARRANTY", 1, OracleType.Int16)
                        Else
                            oExecute.addParameter("RMAD_ISWARRANTY", 0, OracleType.Int16)
                        End If
                    End If


                    'Check ISCW
                    'by Hugh 2023/11/3, CWEndData不為空有可能為E0/EB, 不包去程運費
                    Dim oExport As New ctlRMA.Export
                    Dim sCWEnd As String = oExport.getWarrantyCW(dr.RMAD_SERIALNO, "")
                    If sCWEnd.Trim() <> "" Then
                        If Convert.ToDateTime(sCWEnd) > Date.Now Then
                            oExecute.addParameter("RMAD_ISCW", 1, OracleType.Int16)
                        End If
                    End If


                    'Check ISSW
                    Dim sSWEnd As String = oExport.getWarrantySW(dr.RMAD_SERIALNO, "")
                    If sSWEnd.Trim() <> "" Then
                        If Convert.ToDateTime(sSWEnd) > Date.Now Then
                            oExecute.addParameter("RMAD_ISSW", 1, OracleType.Int16)
                        End If
                    End If
                    'If dr.IsRMAD_WARRANTYNull = False Then
                    '    oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                    '    '顧客Request Submit之同時, 倘若保固期限大於或等於Submit, 自動記錄 is warranty
                    '    'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                    '    If dr.RMAD_WARRANTY > Date.Now Then
                    '        oExecute.addParameter("RMAD_ISWARRANTY", 1, OracleType.Int16)
                    '    End If
                    'End If

                    'If dr.IsRMAD_WARRANTYNull = False Then
                    '    oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                    '    '顧客Request Submit之同時, 倘若保固期限大於或等於Submit, 自動記錄 is warranty
                    '    'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                    '    If dr.RMAD_WARRANTY > Date.Now Then
                    '        oExecute.addParameter("RMAD_ISWARRANTY", 1, OracleType.Int16)
                    '    End If
                    'End If

                    If dr.IsRMAD_FARFARCNONull = False Then oExecute.addParameter("RMAD_FARFARCNO", dr.RMAD_FARFARCNO, OracleType.VarChar)
                    If dr.IsRMAD_FARNONull = False Then oExecute.addParameter("RMAD_FARNO", dr.RMAD_FARNO, OracleType.VarChar)

                    oExecute.addParameter("RMAD_UPLOADFILE", dr.RMAD_UPLOADFILE, OracleType.VarChar)
                    If dr.IsRMAD_PRODUCTDESCNull = False Then oExecute.addParameter("RMAD_PRODUCTDESC", dr.RMAD_PRODUCTDESC, OracleType.NVarChar)
                    If dr.IsRMAD_PROBLEMDESCNull = False Then oExecute.addParameter("RMAD_PROBLEMDESC", dr.RMAD_PROBLEMDESC, OracleType.NVarChar)

                    oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)
                    oExecute.addParameter("RMAD_ISFILL", dr.RMAD_ISFILL, OracleType.Int16)      '是否已填寫問題:0.否, 1.是
                    oExecute.addParameter("RMAD_RECEVSTATUS", 0, OracleType.Int16) '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

                    oExecute.addParameter("RMAD_AD", dr.RMAD_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_ADNAME", dr.RMAD_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_LUAD", dr.RMAD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_MARK", dr.RMAD_MARK, OracleType.Int16)

                    oExecute.Command("RMADETAIL", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改 RMA 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRMA">RMADataTable</param>
        ''' <param name="isReceived">是否收貨</param>
        ''' <remarks></remarks>
        Public Sub Edit_RMA(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMA As RmaDTO.RMADataTable, ByVal isReceived As Boolean)

            Try
                Dim dr As RmaDTO.RMARow = dtRMA.Rows(0)

                oExecute.addParameter("RMA_CUNO", dr.RMA_CUNO, OracleType.VarChar)
                oExecute.addParameter("RMA_ACCOUNTID", dr.RMA_ACCOUNTID, OracleType.VarChar)
                oExecute.addParameter("RMA_APPLICANT", dr.RMA_APPLICANT, OracleType.NVarChar)
                oExecute.addParameter("RMA_TEL", dr.RMA_TEL, OracleType.VarChar)
                oExecute.addParameter("RMA_ADDRESS", dr.RMA_ADDRESS, OracleType.NVarChar)

                If isReceived = True Then
                    oExecute.addParameter("RMA_STATUS", 20, OracleType.Int16)
                Else
                    oExecute.addParameter("RMA_COMPNO", dr.RMA_COMPNO, OracleType.VarChar)
                    oExecute.addParameter("RMA_STATUS", dr.RMA_STATUS, OracleType.Int16)
                End If

                oExecute.addParameter("RMA_MAIL", dr.RMA_MAIL, OracleType.VarChar)
                If dr.IsRMA_RemarkNull = False Then oExecute.addParameter("RMA_Remark", dr.RMA_Remark, OracleType.NVarChar)

                oExecute.addParameter("RMA_LUAD", dr.RMA_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUADNAME", dr.RMA_LUADNAME, OracleType.NVarChar)

                oExecute.addParameter("RMA_CSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMA_MARK", dr.RMA_MARK, OracleType.Int16)

                oExecute.addParameter("RMA_EUCOMPANY", dr.RMA_EUCOMPANY, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUNAME", dr.RMA_EUNAME, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUADDRESS", dr.RMA_EUADDRESS, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUTEL", dr.RMA_EUTEL, OracleType.NVarChar)
                oExecute.addParameter("RMA_EUMAIL", dr.RMA_EUMAIL, OracleType.NVarChar)

                oExecute.addWHERE("RMA_ID", dr.RMA_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("RMA", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally

            End Try
        End Sub

        ''' <summary>
        ''' 修改RMA 品項
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="dtRMADetail"></param>
        ''' <remarks></remarks>
        Private Sub Edit_RMADetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
            Dim i As Integer = 0
            Dim sMessage As String = ""
            Dim oReceived As New ctlRMA.Received

            Try

                For i = 0 To dtRMADetail.Rows.Count - 1
                    Dim dr As RmaDTO.RMADetailRow = dtRMADetail.Rows(i)

                    '=======================================================================================================================================
                    '檢核是否已收貨
                    '=======================================================================================================================================
                    If oReceived.isReceived(oExecute, dr.RMAD_ID.ToString().Trim()) = True Then
                        If dr.IsRMAD_MODELNONull = False Then
                            sMessage = sMessage & "[" & dr.RMAD_MODELNO.Trim() & "]"
                        End If
                        sMessage = sMessage & _oLanguage.getText("RMA", "203", ctlLanguage.eumType.Validator)
                        Throw New ArgumentException(sMessage)
                    End If

                    oExecute.addParameter("RMAD_SEQ", dr.RMAD_SEQ, OracleType.Int16)

                    If dr.IsRMAD_MODELNONull = False Then oExecute.addParameter("RMAD_MODELNO", dr.RMAD_MODELNO, OracleType.VarChar)
                    If dr.IsRMAD_SERIALNONull = False Then oExecute.addParameter("RMAD_SERIALNO", dr.RMAD_SERIALNO, OracleType.VarChar)
                    If dr.IsRMAD_CUSNAMENull = False Then oExecute.addParameter("RMAD_CUSNAME", dr.RMAD_CUSNAME, OracleType.NVarChar)

                    If dr.IsRMAD_WARRANTYNull = False Then
                        oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                        '顧客Request Submit之同時, 倘若保固期限大於或等於Submit, 自動記錄 is warranty
                        'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                        If dr.RMAD_WARRANTY > Date.Now Then
                            oExecute.addParameter("RMAD_ISWARRANTY", 1, OracleType.Int16)
                        Else
                            oExecute.addParameter("RMAD_ISWARRANTY", 0, OracleType.Int16)
                        End If
                    Else
                        oExecute.addParameter("RMAD_WARRANTY", System.Convert.DBNull, OracleType.DateTime)
                        oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                    End If


                    If dr.IsRMAD_FARFARCNONull = False Then oExecute.addParameter("RMAD_FARFARCNO", dr.RMAD_FARFARCNO, OracleType.VarChar)
                    If dr.IsRMAD_FARNONull = False Then oExecute.addParameter("RMAD_FARNO", dr.RMAD_FARNO, OracleType.VarChar)

                    If dr.IsRMAD_UPLOADFILENull = False Then oExecute.addParameter("RMAD_UPLOADFILE", dr.RMAD_UPLOADFILE, OracleType.VarChar)
                    If dr.IsRMAD_PRODUCTDESCNull = False Then oExecute.addParameter("RMAD_PRODUCTDESC", dr.RMAD_PRODUCTDESC, OracleType.NVarChar)
                    If dr.IsRMAD_PROBLEMDESCNull = False Then oExecute.addParameter("RMAD_PROBLEMDESC", dr.RMAD_PROBLEMDESC, OracleType.NVarChar)

                    If dr.IsRMAD_PARTSNNull = False Then oExecute.addParameter("RMAD_PARTSN", dr.RMAD_PARTSN, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)
                    oExecute.addParameter("RMAD_ISFILL", dr.RMAD_ISFILL, OracleType.Int16)      '是否已填寫問題:0.否, 1.是

                    oExecute.addParameter("RMAD_LUAD", dr.RMAD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_MARK", dr.RMAD_MARK, OracleType.Int16)

                    oExecute.addWHERE("RMAD_ID", dr.RMAD_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMADETAIL", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 收貨人員 - 新增RMA 品項
        ''' </summary>
        ''' <param name="dr">dtRMADetail</param>
        ''' <remarks></remarks>
        Public Sub AddNew_RMADetail(ByVal dr As RmaDTO.RMADetailRow)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)                    '系統自動產生唯一識別碼
                oExecute.addParameter("RMAD_RMANO", dr.RMAD_RMANO, OracleType.VarChar)              '關聯 RMA.RMA_No

                oExecute.addParameter("RMAD_SEQ", dr.RMAD_SEQ, OracleType.Int16)

                oExecute.addParameter("RMAD_MODELNO", dr.RMAD_MODELNO, OracleType.VarChar)
                oExecute.addParameter("RMAD_SERIALNO", dr.RMAD_SERIALNO, OracleType.VarChar)
                oExecute.addParameter("RMAD_CUSNAME", dr.RMAD_CUSNAME, OracleType.NVarChar)

                If dr.IsRMAD_WARRANTYNull = False Then oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                If dr.IsRMAD_FARFARCNONull = False Then oExecute.addParameter("RMAD_FARFARCNO", dr.RMAD_FARFARCNO, OracleType.VarChar)
                If dr.IsRMAD_FARNONull = False Then oExecute.addParameter("RMAD_FARNO", dr.RMAD_FARNO, OracleType.VarChar)

                oExecute.addParameter("RMAD_UPLOADFILE", dr.RMAD_UPLOADFILE, OracleType.VarChar)
                oExecute.addParameter("RMAD_PRODUCTDESC", dr.RMAD_PRODUCTDESC, OracleType.NVarChar)
                If dr.IsRMAD_PROBLEMDESCNull = False Then oExecute.addParameter("RMAD_PROBLEMDESC", dr.RMAD_PROBLEMDESC, OracleType.NVarChar)

                oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)
                oExecute.addParameter("RMAD_ISFILL", dr.RMAD_ISFILL, OracleType.Int16)      '是否已填寫問題:0.否, 1.是

                If dr.IsRMAD_PARTSNNull = False Then oExecute.addParameter("RMAD_PARTSN", dr.RMAD_PARTSN, OracleType.NVarChar)

                oExecute.addParameter("RMAD_AD", dr.RMAD_AD, OracleType.NVarChar)
                oExecute.addParameter("RMAD_ADNAME", dr.RMAD_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAD_CSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMAD_LUAD", dr.RMAD_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMAD_MARK", dr.RMAD_MARK, OracleType.Int16)

                If dr.IsRMAD_RECVADNull = False Then oExecute.addParameter("RMAD_RECVAD", dr.RMAD_RECVAD, OracleType.NVarChar)
                If dr.IsRMAD_RECVADNAMENull = False Then oExecute.addParameter("RMAD_RECVADNAME", dr.RMAD_RECVADNAME, OracleType.NVarChar)
                If dr.IsRMAD_RECVDATENull = False Then oExecute.addParameter("RMAD_RECVDATE", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMAD_RECEVSTATUS", dr.RMAD_RECEVSTATUS, OracleType.Int16)

                oExecute.Command("RMADETAIL", Execute.eumCommandType.AddNew)

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
        ''' 收貨人員 - 直接修改RMA 品項
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <remarks></remarks>
        Public Sub Edit_RMADetail(ByVal dr As RmaDTO.RMADetailRow)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                If dr.IsRMAD_MODELNONull = False Then oExecute.addParameter("RMAD_MODELNO", dr.RMAD_MODELNO, OracleType.VarChar)
                If dr.IsRMAD_SERIALNONull = False Then oExecute.addParameter("RMAD_SERIALNO", dr.RMAD_SERIALNO, OracleType.VarChar)
                If dr.IsRMAD_CUSNAMENull = False Then oExecute.addParameter("RMAD_CUSNAME", dr.RMAD_CUSNAME, OracleType.NVarChar)

                If dr.IsRMAD_WARRANTYNull = False Then
                    oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                Else
                    oExecute.addParameter("RMAD_WARRANTY", System.Convert.DBNull, OracleType.DateTime)
                End If

                If dr.IsRMAD_FARFARCNONull = False Then oExecute.addParameter("RMAD_FARFARCNO", dr.RMAD_FARFARCNO, OracleType.VarChar)
                If dr.IsRMAD_FARNONull = False Then oExecute.addParameter("RMAD_FARNO", dr.RMAD_FARNO, OracleType.VarChar)

                If dr.IsRMAD_UPLOADFILENull = False Then oExecute.addParameter("RMAD_UPLOADFILE", dr.RMAD_UPLOADFILE, OracleType.VarChar)

                oExecute.addParameter("RMAD_PRODUCTDESC", dr.RMAD_PRODUCTDESC, OracleType.NVarChar)
                If dr.IsRMAD_PROBLEMDESCNull = False Then oExecute.addParameter("RMAD_PROBLEMDESC", dr.RMAD_PROBLEMDESC, OracleType.NVarChar)

                oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)
                oExecute.addParameter("RMAD_ISFILL", dr.RMAD_ISFILL, OracleType.Int16)      '是否已填寫問題:0.否, 1.是


                If dr.IsRMAD_ISWARRANTYNull = False Then
                    oExecute.addParameter("RMAD_ISWARRANTY", dr.RMAD_ISWARRANTY, OracleType.Int16)
                Else
                    oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                End If

                If dr.Isrmad_iscwNull = False Then
                    oExecute.addParameter("rmad_iscw", dr.rmad_iscw, OracleType.Int16)
                Else
                    oExecute.addParameter("rmad_iscw", System.Convert.DBNull, OracleType.Int16)
                End If

                If dr.Isrmad_isswNull = False Then
                    oExecute.addParameter("rmad_issw", dr.rmad_issw, OracleType.Int16)
                Else
                    oExecute.addParameter("rmad_issw", System.Convert.DBNull, OracleType.Int16)
                End If

                'If dr.IsRMAD_WARRANTYNull = False Then
                '    oExecute.addParameter("RMAD_WARRANTY", dr.RMAD_WARRANTY, OracleType.DateTime)
                '    '顧客Request Submit之同時, 倘若保固期限大於或等於Submit, 自動記錄 is warranty
                '    'RMAD_ISWARRANTY:是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                '    If dr.RMAD_WARRANTY > Date.Now Then
                '        oExecute.addParameter("RMAD_ISWARRANTY", 1, OracleType.Int16)
                '    Else
                '        oExecute.addParameter("RMAD_ISWARRANTY", 0, OracleType.Int16)
                '    End If
                'Else
                '    oExecute.addParameter("RMAD_WARRANTY", System.Convert.DBNull, OracleType.DateTime)
                '    oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                'End If


                If dr.IsRMAD_PARTSNNull = False Then oExecute.addParameter("RMAD_PARTSN", dr.RMAD_PARTSN, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUAD", dr.RMAD_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUSTMP", dr.RMAD_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMAD_MARK", dr.RMAD_MARK, OracleType.Int16)

                oExecute.addWHERE("RMAD_ID", dr.RMAD_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("RMADETAIL", Execute.eumCommandType.UPDATE)

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
        ''' 取得要列印的需求單
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="sRMAID">傳入RMA_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回vwRepair_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function getReport(ByVal LanguageID As String, ByVal sRMAID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RequestReportDataTable
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.RequestReportDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMA_ID", ":RMA_ID", sRMAID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID=:RMA_ID"

                Dim sSQL As String = "SELECT RMA_NO,RMA_ID,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_CSTMP, "
                sSQL = sSQL & " CU_NAME,COMP_NAME, COMP_ADDRESS, COMP_TEL, '' as NoticeDesc, "
                sSQL = sSQL & " RMAD_SERIALNO,RMAD_PARTSN,RMAD_MODELNO,RMAD_CUSNAME,RMAD_WARRANTY, RMAD_PRODUCTDESC ,FARC_NAME,'' as SeqID, "
                sSQL = sSQL & " CW_EDATE, RMA_EUCOMPANY, RMA_EUNAME, RMA_EUADDRESS, RMA_EUTEL, RMA_EUMAIL, RMA_PARTSREQUEST,RMA_COMPNO,RMAD_ISCW"
                sSQL = sSQL & " FROM RMA "
                sSQL = sSQL & " LEFT OUTER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " LEFT OUTER JOIN COMPANY ON RMA.RMA_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & " SELECT * FROM RMADETAIL "
                sSQL = sSQL & " LEFT OUTER JOIN FAILUREREASONSCLASS ON RMADETAIL.RMAD_FARFARCNO = FAILUREREASONSCLASS.FARC_NO AND FAILUREREASONSCLASS.FARC_DFLNO='" & LanguageID.Trim() & "'"
                sSQL = sSQL & " LEFT OUTER JOIN (  SELECT EXPORT_SERIALNO, MAX (CW_EDATE) CW_EDATE FROM EXPORT "
                sSQL = sSQL & " GROUP BY EXPORT_SERIALNO) export On export.EXPORT_SERIALNO = RMADETAIL.RMAD_SERIALNO"
                sSQL = sSQL & " ) vwRMADETAIL "
                sSQL = sSQL & " On RMA.RMA_NO = vwRMADETAIL.RMAD_RMANO "
                sSQL = sSQL & " WHERE RMA_MARK ='0' AND RMAD_MARK='0' "
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

        ''' <summary>
        ''' 取得 提RMA單的需求人員Mail, 姓名 及 品項內容
        ''' </summary>
        ''' <param name="RMADID">RMADID</param>
        ''' <returns>回傳 ArrayList(RMA_NO,RMA_APPLICANT,RMA_MAIL,RMAD_SERIALNO,RMAD_PRODUCTDESC)</returns>
        ''' <remarks></remarks>
        Public Function getApplicantMail(ByVal RMADID As String) As ArrayList
            Dim retArrList As New ArrayList
            Dim i As Integer = 0

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                Dim sCondition_RMADID As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRMADID() As String = RMADID.Split(",")
                For i = 0 To arrRMADID.Length - 1
                    oQuery.addWHERE("RMAD_ID", ":RMAD_ID" & i.ToString, arrRMADID(i).Trim(), OracleType.NVarChar)

                    If sCondition_RMADID.Trim <> "" Then
                        sCondition_RMADID = sCondition_RMADID & " OR "
                    End If
                    sCondition_RMADID = sCondition_RMADID & " RMAD_ID =:RMAD_ID" & i.ToString
                Next
                sCondition = sCondition & sCondition_RMADID & ")"

                sSQL = "SELECT RMA_NO, RMA_APPLICANT, RMA_MAIL, RMAD_SERIALNO, RMAD_PRODUCTDESC " &
                    " FROM RMA , RMADetail WHERE RMA.RMA_NO = RMADetail.RMAD_RMANO  " & sCondition &
                    " GROUP BY RMA_NO, RMA_APPLICANT, RMA_MAIL, RMAD_SERIALNO, RMAD_PRODUCTDESC" &
                    " ORDER BY RMA_NO"

                dt = oQuery.ExecuteDT(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    Dim arrLiat(4) As String
                    arrLiat(0) = dt.Rows(i)("RMA_NO").ToString().Trim()
                    arrLiat(1) = dt.Rows(i)("RMA_APPLICANT").ToString().Trim()
                    arrLiat(2) = dt.Rows(i)("RMA_MAIL").ToString().Trim()
                    arrLiat(3) = dt.Rows(i)("RMAD_SERIALNO").ToString().Trim()
                    arrLiat(4) = dt.Rows(i)("RMAD_PRODUCTDESC").ToString().Trim()

                    retArrList.Add(arrLiat)
                Next

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retArrList
        End Function

        ''' <summary>
        ''' 取得 提RMA單的END USER Mail
        ''' </summary>
        ''' <param name="RMADID">RMADID</param>
        ''' <returns>mail</returns>
        ''' <remarks></remarks>
        Public Function getEndUserMail(ByVal RMADID As String) As String
            Dim Mail As String
            Dim i As Integer = 0

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                Dim sCondition_RMADID As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRMADID() As String = RMADID.Split(",")
                For i = 0 To arrRMADID.Length - 1
                    oQuery.addWHERE("RMAD_ID", ":RMAD_ID" & i.ToString, arrRMADID(i).Trim(), OracleType.NVarChar)

                    If sCondition_RMADID.Trim <> "" Then
                        sCondition_RMADID = sCondition_RMADID & " OR "
                    End If
                    sCondition_RMADID = sCondition_RMADID & " RMAD_ID =:RMAD_ID" & i.ToString
                Next
                sCondition = sCondition & sCondition_RMADID & ")"

                sSQL = " SELECT RMA_NO, RMA_EUCOMPANY, RMA_EUNAME, RMA_EUADDRESS, RMA_EUTEL, RMA_EUMAIL, RMAD_SERIALNO " &
                       "    FROM RMA, RMADetail   " &
                       "   WHERE RMA.RMA_NO = RMADetail.RMAD_RMANO  " &
                       "         AND RMAD_ISCW = 1 AND RMAD_MARK = 0 " & sCondition &
                       " ORDER BY RMA_NO "

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    Mail = dt.Rows(0)("RMA_EUMAIL").ToString()
                End If
            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return Mail
        End Function

        ''' <summary>
        ''' 修改 RMA單的PartsRequest
        ''' </summary>
        ''' <param name="RMA_PARTSREQUEST">RMA_PartsRequest</param>
        ''' <param name="RMA_ID">RMA ID</param>
        ''' <returns>mail</returns>
        ''' <remarks></remarks>
        Public Function UPDATE_RMA_RMA_PARTSREQUEST(ByVal RMA_PARTSREQUEST As Boolean, ByVal RMA_ID As String) As Boolean
            Dim connection As Connection = New Connection()
            Dim text As String = ""
            Dim text2 As String = ""
            connection.Open()
            text = (If((Not RMA_PARTSREQUEST), "UPDATE RMA SET RMA_PARTSREQUEST=0 WHERE RMA_ID =:RMA_ID ", "UPDATE RMA SET RMA_PARTSREQUEST=1 WHERE RMA_ID =:RMA_ID "))

            Try
                Dim obj As Object = New OracleCommand(text, connection.Connection())

                Try
                    NewLateBinding.LateCall(NewLateBinding.LateGet(obj, Nothing, "Parameters", New Object(-1) {}, Nothing, Nothing, Nothing), Nothing, "Add", New Object(0) {New OracleParameter("RMA_ID", RMA_ID)}, Nothing, Nothing, Nothing, IgnoreReturn:=True)
                    NewLateBinding.LateCall(obj, Nothing, "ExecuteNonQueryAsync", New Object(-1) {}, Nothing, Nothing, Nothing, IgnoreReturn:=True)
                Finally

                    If obj IsNot Nothing Then
                        CType(obj, IDisposable).Dispose()
                    End If
                End Try

            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                text2 = ex2.Message
                ProjectData.ClearProjectError()
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return RMA_PARTSREQUEST
        End Function

    End Class
#End Region

#Region "Class:Received:收貨"
    Public Class Received

        ''' <summary>
        ''' 取得 待工作項目
        ''' </summary>
        ''' <param name="COMPNO">維修中心代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWork(ByVal COMPNo As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.vwReceiveListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtReceiveList As New RmaDTO.vwReceiveListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = COMPNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim().ToLower() & "%"
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMA_NO) like :RMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim().ToLower() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim().ToLower() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND (lower(RMAD_SERIALNO) like :RMAD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_SERIALNO)"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.Trim().ToLower() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Status <> -1 Then
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    oQuery.addWHERE("RMA_STATUS", ":RMA_STATUS", Status, OracleType.Int16)
                    If Status = 10 Then
                        sCondition = sCondition & " AND RMA_STATUS<=:RMA_STATUS"
                    Else
                        sCondition = sCondition & " AND RMA_STATUS=:RMA_STATUS"
                    End If
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If

                Dim sSQL As String = "select RMA_NO, RMA_ID, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(TotalCount,0) TotalCount, nvl(RecvCount,0) RecvCount"
                sSQL = sSQL & " from rma inner join CUSTOMER"
                sSQL = sSQL & " ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=10 OR RMA_STATUS=20)"
                sSQL = sSQL & " inner join rmadetail"
                sSQL = sSQL & " ON RMA.RMA_NO = rmadetail.rmad_rmano"
                sSQL = sSQL & " inner join "
                sSQL = sSQL & "  ("
                sSQL = sSQL & "    select rmad_rmano, Count(*) TotalCount from rmadetail"
                sSQL = sSQL & "    where rmad_mark=0 group BY rmad_rmano"
                sSQL = sSQL & "  ) vwRMADetail"
                sSQL = sSQL & " ON RMA.RMA_NO = vwRMADetail.rmad_rmano"
                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "   select rmad_rmano, Count(*) RecvCount from rmadetail"
                sSQL = sSQL & "   where rmad_mark=0 AND (RMAD_STATUS=20 OR RMAD_RECEVSTATUS<>0) AND RMAD_STATUS <> 91 group BY rmad_rmano"
                sSQL = sSQL & "  ) vwRecv"
                sSQL = sSQL & " ON RMA.RMA_NO = vwRecv.rmad_rmano"

                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "   select rmad_rmano, Count(*) DelCount from rmadetail"
                sSQL = sSQL & "   where rmad_mark=0 AND RMAD_STATUS = 91 group BY rmad_rmano"
                sSQL = sSQL & "  ) vwDel"
                sSQL = sSQL & " ON RMA.RMA_NO = vwDel.rmad_rmano"

                sSQL = sSQL & " WHERE nvl(TotalCount,0) <> (nvl(RecvCount,0) + nvl(DelCount,0)) " & sCondition
                sSQL = sSQL & " group by RMA_NO, RMA_ID, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(TotalCount,0), nvl(RecvCount,0)"
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtReceiveList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtReceiveList
        End Function

        ''' <summary>
        ''' 檢核是否已收貨
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="RMAD_ID">RMAD ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isReceived(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMAD_ID As String) As Boolean
            Dim retval As Boolean = False
            Dim dt As New DataTable

            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)

            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
            oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
            Dim sCondition As String = " AND RMAD_ID =:RMAD_ID"

            Dim sSQL As String = "SELECT * FROM RMADetail WHERE RMAD_STATUS>=20 " & sCondition
            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = True
            End If

            Return retval
        End Function

        ''' <summary>
        ''' 檢核 沒收貨刪除
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isDeleted(ByVal RMAD_ID As String) As Boolean
            Dim retval As Boolean = False
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)


            oConn.Open()
            Try
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                Dim sCondition As String = " AND RMAD_ID =:RMAD_ID"

                'RMAD_RECEVSTATUS -->註解	是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                Dim sSQL As String = "SELECT * FROM RMADetail WHERE RMAD_STATUS>=20 AND RMAD_RECEVSTATUS=2 " & sCondition
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
        ''' 刪除 RMA 單 RMA_STATUS=Canceled
        ''' </summary>
        ''' <param name="RMA_ID">RMA ID</param>
        ''' <param name="RMA_NO">RMA NO</param>
        ''' <param name="RecvAD">收貨人帳號</param>
        ''' <param name="RecvNAME">收貨人姓名</param>
        ''' <remarks></remarks>
        Public Sub DeleteRMA(ByVal RMA_ID As String, ByVal RMA_NO As String,
                ByVal RecvAD As String, ByVal RecvNAME As String)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                '===========================================================================================================================================================================
                'UPDATE  RMA_STATUS = Canceled
                '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                oExecute.addParameter("RMA_STATUS", "91", OracleType.Int16)

                oExecute.addParameter("RMA_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMA_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("RMA_ID", RMA_ID.Trim(), OracleType.VarChar)
                oExecute.addWHERE("RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
                oExecute.Command("RMA", Execute.eumCommandType.UPDATE)


                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                oExecute.addParameter("RMAD_STATUS", "91", OracleType.Int16)

                oExecute.addParameter("RMAD_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addParameter("RMAD_RECVAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMAD_RECVADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("RMAD_RECVDATE", Date.Now, OracleType.DateTime)

                '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                oExecute.addParameter("RMAD_RECEVSTATUS", 2, OracleType.Int16)

                oExecute.addWHERE("RMAD_RMANO", RMA_NO.Trim(), OracleType.VarChar)
                oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
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
        ''' 刪除 RMA單品項 RMA_STATUS=Canceled
        ''' </summary>
        ''' <param name="dtReceive">傳入要處理的 dtReceive</param>
        ''' <remarks></remarks>
        Public Sub DeleteRMADetail(ByVal dtReceive As RmaDTO.tmpRecvStatusDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)


            oConn.Open()
            Try
                oConn.BeginTransaction()
                Call Save_RecvStatus(oExecute, dtReceive)
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
        ''' 檢核所以項目是否已全部刪除, 是的話 單頭狀態改為 Canceled
        ''' </summary>
        ''' <param name="sRMA_NO"></param>
        Public Sub UpdateRmaStatus(ByVal sRMA_NO As String, ByVal sRecvAD As String, ByVal sRecvName As String)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim dt As New DataTable

            oConn.Open()
            Try
                oConn.BeginTransaction()
                '===========================================================================================================================================================================
                '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", sRMA_NO, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO =:RMAD_RMANO"

                Dim sSQL As String = "SELECT * FROM RMADetail WHERE RMAD_MARK=0 AND RMAD_STATUS<>91" & sCondition
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count = 0 Then
                    oExecute.addParameter("RMA_STATUS", "91", OracleType.Int16)
                Else
                    oExecute.addParameter("RMA_STATUS", "20", OracleType.Int16)
                End If
                oExecute.addParameter("RMA_LUAD", sRecvAD, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUADNAME", sRecvName, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("RMA_NO", sRMA_NO, OracleType.VarChar)
                oExecute.Command("RMA", Execute.eumCommandType.UPDATE)

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
        ''' 儲存 RMA 單的狀態
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="dtReceive">傳入要處理的 dtReceive</param>
        ''' <remarks></remarks>
        Private Sub Save_RecvStatus(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtReceive As RmaDTO.tmpRecvStatusDataTable)
            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim sRMA_NO As String = ""
            Dim sRecvAD As String = ""
            Dim sRecvName As String = ""

            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)
            Dim dt As New DataTable

            Try


                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                For i = 0 To dtReceive.Rows.Count - 1
                    Dim dr As RmaDTO.tmpRecvStatusRow = dtReceive.Rows(i)

                    sRMA_NO = dr.RMAD_NO.Trim()
                    sRecvAD = dr.RMA_RecvAD.Trim()
                    sRecvName = dr.RMA_RecvName.Trim()

                    '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                    Select Case dr.RMA_RecvStatus.Trim()
                        Case "1"
                            oExecute.addParameter("RMAD_STATUS", "20", OracleType.Int16)
                            oExecute.addParameter("RMAD_RECEVSTATUS", 1, OracleType.Int16)

                        Case "2"
                            oExecute.addParameter("RMAD_STATUS", "91", OracleType.Int16)
                            oExecute.addParameter("RMAD_RECEVSTATUS", 2, OracleType.Int16)
                    End Select

                    oExecute.addParameter("RMAD_LUAD", dr.RMA_RecvAD.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", dr.RMA_RecvName.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addParameter("RMAD_RECVAD", dr.RMA_RecvAD.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_RECVADNAME", dr.RMA_RecvName.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_RECVDATE", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("RMAD_ID", dr.RMAD_ID.Trim(), OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                Next

                '===========================================================================================================================================================================
                '檢核所以項目是否已全部刪除, 是的話 單頭狀態改為 Canceled
                '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", sRMA_NO, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO =:RMAD_RMANO"

                Dim sSQL As String = "SELECT * FROM RMADetail WHERE RMAD_MARK=0 AND RMAD_STATUS<>91" & sCondition
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count = 0 Then
                    oExecute.addParameter("RMA_STATUS", "91", OracleType.Int16)
                Else
                    oExecute.addParameter("RMA_STATUS", "20", OracleType.Int16)
                End If
                oExecute.addParameter("RMA_LUAD", sRecvAD, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUADNAME", sRecvName, OracleType.NVarChar)
                oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("RMA_NO", sRMA_NO, OracleType.VarChar)
                oExecute.Command("RMA", Execute.eumCommandType.UPDATE)


            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 收貨
        ''' </summary>
        ''' <param name="dtRMA"></param>
        ''' <param name="dtReceive"></param>
        ''' <remarks></remarks>
        Public Sub SaveByReceived(ByVal dtRMA As RmaDTO.RMADataTable, ByVal dtReceive As RmaDTO.tmpRecvStatusDataTable)
            Dim oRequested As New ctlRMA.Requested
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)


            oConn.Open()
            Try
                oConn.BeginTransaction()

                Call Save_RecvStatus(oExecute, dtReceive)
                oRequested.Edit_RMA(oExecute, dtRMA, True)
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
#End Region

#Region "Class:Repair:維修"
    Public Class Repair

        ''' <summary>
        ''' 取得 待工作項目Group by 單頭
        ''' </summary>
        ''' <param name="COMPNO">維修中心代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="arrStatus">要取的項目的目前狀態</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="isQuery">查詢方式:0.all，1.Quoting，2.Repairing</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWorkGroup(ByVal COMPNo As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal arrStatus() As String, ByVal fdate As String, ByVal edate As String, ByVal isQuery As Integer,
            Optional ByVal OrderBY As String = "") As RmaDTO.vwRepair_WorkListGroupDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRepairList As New RmaDTO.vwRepair_WorkListGroupDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_NO Desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sInRepairCenter As String = ""
                Dim sInRepairCenterFirst As String = ""
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = COMPNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    sInRepairCenterFirst = arrRepair(i).Trim()

                    If sInRepairCenter <> "" Then
                        sInRepairCenter = sInRepairCenter + ","
                    End If
                    sInRepairCenter = sInRepairCenter + "'" + arrRepair(i).Trim() + "'"

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMA_NO) like :RMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    oQuery.addWHERE("RMAD_PARTSN", ":RMAD_PARTSN", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND (lower(RMAD_SERIALNO) like :RMAD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_PARTSN)"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If


                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
                '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                Dim iLoop As Integer = 0
                Dim sCondition_Status As String = ""
                For i = 0 To arrStatus.Length - 1
                    Dim Status As String = arrStatus(i).ToString().Trim()
                    If Status.Trim <> "" And Convert.ToInt16(Status) > 10 Then
                        iLoop = iLoop + 1

                        If sCondition_Status.Trim <> "" Then
                            sCondition_Status = sCondition_Status & " OR "
                        End If
                        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS" & iLoop.ToString, Convert.ToInt16(Status), OracleType.Int16)
                        sCondition_Status = sCondition_Status & " RMAD_STATUS=:RMAD_STATUS" & iLoop.ToString
                    End If
                Next

                If sCondition_Status.Trim <> "" Then
                    sCondition = sCondition & " AND (" & sCondition_Status & ") "
                End If

                'If Status <> -1 Then
                '    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
                '    '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled

                '    If Convert.ToInt16(Status) >= 20 And Convert.ToInt16(Status) < 40 Then
                '        '等待報價 或 已報價
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "20", OracleType.Int16)
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "40", OracleType.Int16)
                '        sCondition = sCondition & " AND (RMAD_STATUS>=:RMAD_STATUS1 AND RMAD_STATUS<:RMAD_STATUS2)"
                '    Else
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                '        sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
                '    End If
                'End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If


                If isQuery = 2 Then
                    'oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                    'oQuery.addWHERE("RMARED_RMADID", ":RMARED_RMADID", SerialNo, OracleType.VarChar)
                    'sCondition = sCondition & " AND RMARED_RMADID is null"
                    sCondition = sCondition & " AND RMAR_REPAIRAD is null"
                End If

                Dim sSQL As String = "SELECT RMA_ID,RMA_NO,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_COMPNO ,RMA_MAIL,RMA_STATUS,RMA_AD,RMA_ADNAME,RMA_CSTMP"
                sSQL += ",RMA_LUAD,RMA_LUADNAME,RMA_LUSTMP,RMA_MARK"

                'sSQL += ",NVL(b.TotalCount,0) TotalCount,NVL(c.RecvCount,0) RecvCount"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " ),0) TotalCount"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " AND (t.RMAD_STATUS=20 OR t.RMAD_RECEVSTATUS<>0) ),0) RecvCount"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " AND t.RMAD_STATUS IN('20')),0) Received"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " AND t.RMAD_STATUS IN('30')),0) WIP"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s"
                sSQL += " on t.rmad_id=s.rmar_rmadid WHERE a.RMA_NO=t.RMAD_RMANO AND t.RMAD_STATUS IN('50','60')"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " and s.RMAR_REPAIRAD is null),0) Repairing"

                sSQL += ",NVL((SELECT COUNT(1) FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO AND t.RMAD_STATUS IN('60')"
                sSQL += " AND (NVL(s.RMAR_COMPNO,'') IN(" + sInRepairCenter + ") or a.RMA_COMPNO IN(" + sInRepairCenter + "))"
                sSQL += " and s.RMAR_REPAIRAD is not null),0) Repaired"

                sSQL += ",NVL((SELECT MAX('Y') FROM RMADETAIL t LEFT OUTER JOIN RMAREPAIR z ON t.RMAD_ID = z.RMAR_RMADID"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO"
                If sInRepairCenter <> "" Then
                    sSQL += " AND NVL(z.RMAR_COMPNO,'" + sInRepairCenterFirst + "') IN(" + sInRepairCenter + ")"
                End If
                sSQL += " AND t.RMAD_STATUS IN('20','30')),'N') ShowQuote"

                sSQL += ",NVL((SELECT t.RMAD_ID FROM RMADETAIL t LEFT OUTER JOIN RMAREPAIR z ON t.RMAD_ID = z.RMAR_RMADID"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO AND ROWNUM=1"
                If sInRepairCenter <> "" Then
                    sSQL += " AND NVL(z.RMAR_COMPNO,'" + sInRepairCenterFirst + "') IN(" + sInRepairCenter + ")"
                End If
                sSQL += " AND t.RMAD_STATUS IN('20','30')),'N') ShowQuoteSN"

                sSQL += ",NVL((SELECT MAX('Y') FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += " WHERE a.RMA_NO=t.RMAD_RMANO AND t.RMAD_STATUS IN('50','60')"
                If sInRepairCenter <> "" Then
                    sSQL += " AND NVL(s.RMAR_COMPNO,'" + sInRepairCenterFirst + "') IN(" + sInRepairCenter + ")"
                End If
                'sSQL += "  and s.RMAR_REPAIRAD is null"
                sSQL += "),'N') ShowRepair"

                sSQL += ",NVL((SELECT t.RMAD_ID FROM RMADETAIL t left outer join rmarepair s on t.rmad_id=s.rmar_rmadid"
                sSQL += "  WHERE a.RMA_NO=t.RMAD_RMANO AND ROWNUM=1 AND t.RMAD_STATUS IN('50','60')"
                If sInRepairCenter <> "" Then
                    sSQL += " AND NVL(s.RMAR_COMPNO,'" + sInRepairCenterFirst + "') IN(" + sInRepairCenter + ")"
                End If
                'sSQL += "  and s.RMAR_REPAIRAD is null"
                sSQL += "),'N') ShowRepairSN"

                sSQL += ",MAX(rmad_id) rmad_id,MAX(rmad_seq) rmad_seq"
                sSQL += ",MAX(rmad_rmano) rmad_rmano,MAX(rmad_modelno) rmad_modelno"
                sSQL += ",MAX(rmad_serialno) rmad_serialno,MAX(rmad_cusname) rmad_cusname"
                sSQL += ",MAX(rmad_warranty) rmad_warranty,MAX(rmad_farfarcno) rmad_farfarcno"
                sSQL += ",MAX(rmad_farno) rmad_farno,MAX(rmad_uploadfile) rmad_uploadfile"
                sSQL += ",MAX(rmad_productdesc) rmad_productdesc,MAX(rmad_problemdesc) rmad_problemdesc"
                sSQL += ",MAX(rmad_status) rmad_status,MAX(rmad_isfill) rmad_isfill"
                sSQL += ",MAX(rmad_recvad) rmad_recvad,MAX(rmad_recvadname) rmad_recvadname"
                sSQL += ",MAX(rmad_recvdate) rmad_recvdate,MAX(rmad_recevstatus) rmad_recevstatus"
                sSQL += ",MAX(rmad_ad) rmad_ad,MAX(rmad_adname) rmad_adname"
                sSQL += ",MAX(rmad_cstmp) rmad_cstmp,MAX(rmad_luad) rmad_luad"
                sSQL += ",MAX(rmad_luadname) rmad_luadname,MAX(rmad_lustmp) rmad_lustmp"
                sSQL += ",MAX(rmad_mark) rmad_mark,MAX(rmad_iswarranty) rmad_iswarranty"
                sSQL += ",MAX(cu_no) cu_no,MAX(cu_name) cu_name"
                sSQL += ",MAX(rmar_compno) rmar_compno,MAX(comp_name) comp_name"
                sSQL += ",MAX(rmar_repair_isfill) rmar_repair_isfill,MAX(rmar_repairad) rmar_repairad"
                sSQL += ",MAX(rmared_rmadid) rmared_rmadid,MAX(rmarq_assigecurrencycode) rmarq_assigecurrencycode"
                sSQL += ",MAX(rmarq_assigequote) rmarq_assigequote,MAX(rmarq_currencycode) rmarq_currencycode"
                sSQL += ",MAX(rmarq_quote) rmarq_quote"
                sSQL += "  FROM VWREPAIR_WORKLIST a"
                sSQL = sSQL & " inner join "
                sSQL = sSQL & "  ("
                sSQL = sSQL & "    select rmad_rmano B_NO, Count(*) TotalCount from rmadetail"
                sSQL = sSQL & "    where rmad_mark=0 group BY rmad_rmano"
                sSQL = sSQL & "  ) b"
                sSQL = sSQL & " ON a.RMA_NO = b.B_NO"
                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "   select rmad_rmano C_NO, Count(*) RecvCount from rmadetail"
                sSQL = sSQL & "   where rmad_mark=0 AND (RMAD_STATUS=20 OR RMAD_RECEVSTATUS<>0) group BY rmad_rmano"
                sSQL = sSQL & "  ) c"
                sSQL = sSQL & " ON a.RMA_NO = c.C_NO"

                sSQL += " WHERE 1=1" & sCondition
                sSQL += " GROUP BY RMA_ID,RMA_NO,RMA_CUNO,RMA_ACCOUNTID,RMA_APPLICANT,RMA_TEL,RMA_ADDRESS,RMA_COMPNO"
                sSQL += " ,RMA_MAIL,RMA_STATUS,RMA_AD,RMA_ADNAME,RMA_CSTMP,RMA_LUAD,RMA_LUADNAME,RMA_LUSTMP,RMA_MARK"
                sSQL += ",NVL(b.TotalCount,0),NVL(c.RecvCount,0)"
                sSQL += OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairList
        End Function

        Public Function QueryByWorkGroup_speed() As DataTable
            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRepairList As New RmaDTO.vwRepair_WorkListGroupDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = " select   
                RMA_ID,
                RMA_NO,
                RMA_NO as RMAD_RMANO,
                RMA_NO as ShowRepairSN,
                RMA_NO as ShowQuoteSN,
                RMA_CUNO,
                RMA_ACCOUNTID,
                RMA_APPLICANT,
                RMA_TEL,
                RMA_COMPNO ,
                RMA_MAIL,
                RMA_STATUS,
                RMA_AD,
                RMA_ADNAME,
                RMA_CSTMP,
                RMA_LUAD,
                RMA_LUADNAME,   
                RMA_LUSTMP,    
                RMA_MARK 
                from  RMA   where RMA_NO in ( select    DISTINCT RMAD_RMANO from  RMADETAIL  where    RMAD_STATUS in ( 20,30,50,60))  and RMA_STATUS = 20  order by  RMA_CSTMP desc  "
                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        Public Function QueryByWorkGroup_RMAD_ID(ByVal RMAD_RMANO As String) As DataTable
            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRepairList As New RmaDTO.vwRepair_WorkListGroupDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = " select   
                RMAD_ID
                from  RMADETAIL  where  RMAD_RMANO =:RMAD_RMANO   and RMAD_SEQ = 1"
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO, OracleType.VarChar)

                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' 取得 待工作項目
        ''' </summary>
        ''' <param name="COMPNO">維修中心代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="arrStatus">要取的項目的目前狀態</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="isQuery">查詢方式:0.all，1.Quoting，2.Repairing</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWork(ByVal COMPNo As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal arrStatus() As String, ByVal fdate As String, ByVal edate As String, ByVal isQuery As Integer,
            Optional ByVal OrderBY As String = "") As RmaDTO.VWREPAIR_WORKLISTDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRepairList As New RmaDTO.VWREPAIR_WORKLISTDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = COMPNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMA_NO) like :RMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_SERIALNO) like :RMAD_SERIALNO"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If


                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
                '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                Dim iLoop As Integer = 0
                Dim sCondition_Status As String = ""
                For i = 0 To arrStatus.Length - 1
                    Dim Status As String = arrStatus(i).ToString().Trim()
                    If Status.Trim <> "" And Convert.ToInt16(Status) > 10 Then
                        iLoop = iLoop + 1

                        If sCondition_Status.Trim <> "" Then
                            sCondition_Status = sCondition_Status & " OR "
                        End If
                        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS" & iLoop.ToString, Convert.ToInt16(Status), OracleType.Int16)
                        sCondition_Status = sCondition_Status & " RMAD_STATUS=:RMAD_STATUS" & iLoop.ToString
                    End If
                Next

                If sCondition_Status.Trim <> "" Then
                    sCondition = sCondition & " AND (" & sCondition_Status & ") "
                End If

                'If Status <> -1 Then
                '    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 
                '    '40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled

                '    If Convert.ToInt16(Status) >= 20 And Convert.ToInt16(Status) < 40 Then
                '        '等待報價 或 已報價
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "20", OracleType.Int16)
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "40", OracleType.Int16)
                '        sCondition = sCondition & " AND (RMAD_STATUS>=:RMAD_STATUS1 AND RMAD_STATUS<:RMAD_STATUS2)"
                '    Else
                '        oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                '        sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
                '    End If
                'End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If


                If isQuery = 2 Then
                    'oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                    'oQuery.addWHERE("RMARED_RMADID", ":RMARED_RMADID", SerialNo, OracleType.VarChar)
                    'sCondition = sCondition & " AND RMARED_RMADID is null"
                    sCondition = sCondition & " AND RMAR_REPAIRAD is null"
                End If

                Dim sSQL As String = "SELECT * FROM VWREPAIR_WORKLIST WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairList
        End Function

        ''' <summary>
        ''' 取得已維修 品項的資料
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>RMADetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByDetail(ByVal RMAD_ID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMARepair_DetailDataTable
            Dim sCondition As String = ""
            Dim dtRepairDetail As New RmaDTO.RMARepair_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RMARED_ID") = "RMARED_oldID"

                If OrderBY.Trim = "" Then
                    OrderBY = " RMARED_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMARED_RMADID", ":RMARED_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARED_RMADID=:RMARED_RMADID"

                Dim sSQL As String = ""
                sSQL = sSQL & " SELECT RMARED_ID, RMARED_RMADID, "
                sSQL = sSQL & "     RMARED_NPARTNO, RMARED_NSERIALNO, RMARED_NWARRANTY, "
                sSQL = sSQL & "     RMARED_OPARTNO, RMARED_OSERIALNO, RMARED_OWARRANTY, "
                sSQL = sSQL & "     RMARED_DESC, RMARED_LOCATION, RMARED_IMPROPERUSAGE, RMARED_DEFECTIVE, "
                sSQL = sSQL & "     RMARED_QTY, RMARED_MATERIALCOST, RMARED_PRICE, RMARED_CURRENCYCODE, RMARED_CURRENCYRATE, "
                sSQL = sSQL & "     RMARED_ASSIGEPRICE, RMARED_ASSIGECURRENCYCODE, RMARED_ASSIGECURRENCYRATE, "
                sSQL = sSQL & "     RMARED_AD, RMARED_ADNAME, RMARED_CSTMP, RMARED_LUAD, RMARED_LUADNAME, RMARED_LUSTMP, RMARED_MARK, "
                sSQL = sSQL & "     nvl(RMARED_WAIVE,0) as RMARED_WAIVE, nvl(RMARED_OPTION,0) as RMARED_OPTION, nvl(RMARED_ISSOURCE, 1) as RMARED_ISSOURCE"
                sSQL = sSQL & " FROM RMARepair_Detail"
                sSQL = sSQL & " WHERE RMARED_MARK=0 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairDetail, HasExceColumn)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairDetail
        End Function

        ''' <summary>
        ''' 取得維修詳細資料(單頭)
        ''' </summary>
        ''' <param name="RMA_ID">傳入RMAID</param>
        ''' <returns>傳回vwRepair_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairHead(ByVal RMA_ID As String) As RmaDTO.vwRepair_HeadDataTable
            Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID=:RMA_ID"

                sSQL = "SELECT * FROM VWREPAIR_HEAD WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairHead)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairHead
        End Function

        ''' <summary>
        ''' 取得維修詳細資料(單頭)
        ''' </summary>
        ''' <param name="RMA_ID">傳入RMAID</param>
        ''' <returns>傳回vwRepair_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByIRepairHead(ByVal RMA_ID As String) As RmaDTO.vwRepair_HeadDataTable
            Dim dtRepairHead As New RmaDTO.vwRepair_HeadDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_ID", ":RMA_ID", RMA_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ID=:RMA_ID"

                sSQL = "SELECT * FROM VWIREPAIR_HEAD WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairHead)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairHead
        End Function

        ''' <summary>
        ''' 取得所有要維修品項(單身)
        ''' </summary>
        ''' <param name="RMA_NO">傳入RMA_NO</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回tmpRepair_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryAllByRMADetail(ByVal LanguageID As String, ByVal RMA_NO As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpRepair_DetailDataTable
            Dim sCondition As String = ""
            Dim dtRepairDetail As New RmaDTO.tmpRepair_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO=:RMAD_RMANO"

                Dim sSQL As String = "SELECT RMAD_ID, RMAD_SERIALNO, RMAD_MODELNO, RMAD_PRODUCTDESC, RMAD_CUSNAME, RMAD_WARRANTY, RMAD_CSTMP, RMAD_ISWARRANTY, "
                sSQL = sSQL & " RMAD_RECEVSTATUS, RMAD_FARFARCNO, FAILUREREASONSCLASS.FARC_NAME,"
                sSQL = sSQL & " vwRMAREPAIR.RMAR_FARCNO RMAR_FARCNO, vwRMAREPAIR.FARC_NAME RMAR_FARCNAME"

                sSQL = sSQL & " FROM RMADETAIL LEFT OUTER JOIN FAILUREREASONSCLASS"
                sSQL = sSQL & " ON RMADETAIL.rmad_farfarcno = FAILUREREASONSCLASS.FARC_NO AND FAILUREREASONSCLASS.FARC_DFLNO='" & LanguageID.Trim() & "'"
                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & " SELECT * FROM RMAREPAIR LEFT OUTER JOIN FAILUREREASONSCLASS"
                sSQL = sSQL & " ON RMAREPAIR.RMAR_FARCNO = FAILUREREASONSCLASS.FARC_NO AND FAILUREREASONSCLASS.FARC_DFLNO='" & LanguageID.Trim() & "'"
                sSQL = sSQL & " ) vwRMAREPAIR"
                sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwRMAREPAIR.RMAR_RMADID"
                sSQL = sSQL & " WHERE RMAD_RECEVSTATUS='1'"
                sSQL = sSQL & sCondition
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairDetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairDetail
        End Function

        ''' <summary>
        ''' 取得維修資料(單頭)
        ''' </summary>
        ''' <param name="RMAD_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRepair(ByVal RMAD_ID As String) As RmaDTO.RMARepairDataTable
            Dim dtRMARepair As New RmaDTO.RMARepairDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMAR_RMADID", ":RMAR_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAR_RMADID=:RMAR_RMADID"

                sSQL = "SELECT * FROM RMARepair WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMARepair)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try


            Return dtRMARepair
        End Function

        ''' <summary>
        ''' 取得維修詳細資料
        ''' </summary>
        ''' <param name="sRMADID">傳入RMAD_ID</param>
        ''' <returns>傳回vwRepair_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRMARepairDetail(ByVal sLanguageID As String, ByVal sRMADID As String) As RmaDTO.vwRepair_DetailDataTable
            Dim sCondition As String = ""
            Dim dtRMARepairDetail As New RmaDTO.vwRepair_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", sRMADID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID "
                oQuery.addWHERE("FARC_DFLNO", ":FARC_DFLNO", sLanguageID, OracleType.VarChar)
                sCondition = sCondition & " AND FARC_DFLNO=:FARC_DFLNO "

                Dim sSQL As String = "SELECT * FROM vwRepair_Detail WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMARepairDetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRMARepairDetail
        End Function

        ''' <summary>
        ''' 取得維修上傳檔案資料 - 群組方式
        ''' </summary>
        ''' <param name="RMA_NO">RMA NO</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByUpload_Group(ByVal RMA_NO As String) As RmaDTO.tmpRepairUploadDataTable
            Dim i As Integer = 0
            Dim iColumn As Integer = 3
            Dim iLoopCount As Integer = 0
            Dim dtRepairUpload As New RmaDTO.RMAREPAIR_UPLOADDataTable
            Dim dttmpRepair As New RmaDTO.tmpRepairUploadDataTable


            dtRepairUpload = QueryByUpload(RMA_NO)

            Dim dr As DataRow = dttmpRepair.NewRow
            For i = 0 To dtRepairUpload.Rows.Count - 1
                Dim drRepairUpload As RmaDTO.RMAREPAIR_UPLOADRow = dtRepairUpload.Rows(i)

                iLoopCount = iLoopCount + 1
                If iLoopCount = 1 And i > 0 Then
                    dr = dttmpRepair.NewRow
                End If

                dr("SeqID_" & iLoopCount.ToString()) = i + 1
                dr("RMARU_UPLOADFILE_" & iLoopCount.ToString()) = drRepairUpload.RMARU_UPLOADFILE.ToString().Trim()

                dr("RMARU_DESC_" & iLoopCount.ToString()) = ""
                If drRepairUpload.IsRMARU_DESCNull = False Then
                    dr("RMARU_DESC_" & iLoopCount.ToString()) = drRepairUpload.RMARU_DESC.ToString().Trim()
                End If

                If iLoopCount = iColumn Then
                    dttmpRepair.Rows.Add(dr)
                    iLoopCount = 0
                Else
                    If i = dtRepairUpload.Rows.Count - 1 Then
                        dttmpRepair.Rows.Add(dr)
                    End If
                End If
            Next

            Return dttmpRepair
        End Function

        ''' <summary>
        ''' 取得維修上傳檔案資料
        ''' </summary>
        ''' <param name="RMA_NO">傳入RMA_NO</param>
        ''' <returns>傳回RMARepair_UploadDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByUpload(ByVal RMA_NO As String) As RmaDTO.RMAREPAIR_UPLOADDataTable
            Dim dtRepairUpload As New RmaDTO.RMAREPAIR_UPLOADDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMARU_RMANO", ":RMARU_RMANO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " AND RMARU_RMANO=:RMARU_RMANO"

                sSQL = "SELECT * FROM RMAREPAIR_UPLOAD WHERE 1=1" & sCondition & " ORDER BY RMARU_LUSTMP desc"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairUpload)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairUpload
        End Function

        ''' <summary>
        ''' 儲存 RMA 單的狀態 RMA_STATUS(60)=維修
        ''' </summary>
        ''' <param name="dtRepairStatus">傳入dtRepairStatus</param>
        ''' <remarks></remarks>
        Public Sub Save_RepairStatus(ByVal dtRepairStatus As RmaDTO.tmpRepairStatusDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                Call Save_Status(oExecute, dtRepairStatus, 60)
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
        ''' 儲存 RMA 單的狀態
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRepairStatus">傳入dtRepairStatus</param>
        ''' <param name="RMAD_STATUS">傳入RMAD_STATUS</param>
        ''' <remarks></remarks>
        Private Sub Save_Status(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRepairStatus As RmaDTO.tmpRepairStatusDataTable, ByVal RMAD_STATUS As Integer)
            Dim i As Integer = 0
            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)
            Dim dt As New DataTable

            Try

                For i = 0 To dtRepairStatus.Rows.Count - 1
                    Dim dr As RmaDTO.tmpRepairStatusRow = dtRepairStatus.Rows(i)
                    oExecute.addParameter("RMAD_STATUS", Convert.ToInt16(dr.Repair_Status), OracleType.Int16)

                    oExecute.addWHERE("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try


        End Sub

        ''' <summary>
        ''' 檢核  RMAR_RMADID 是否存在
        ''' </summary>
        ''' <param name="RMAR_RMADID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkIsExist(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMAR_RMADID As String) As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAR_RMADID", ":RMAR_RMADID", RMAR_RMADID.Trim(), OracleType.VarChar)

                Dim sSQL As String = "SELECT * FROM RMAREPAIR WHERE RMAR_RMADID=:RMAR_RMADID "

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
        ''' 檢核是否已填寫過 維修報價單
        ''' </summary>
        ''' <param name="RMAR_RMADID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function chkQuoted_IsFill(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMAR_RMADID As String) As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)

            Try

                oQuery.addWHERE("RMAR_RMADID", ":RMAR_RMADID", RMAR_RMADID.Trim(), OracleType.VarChar)
                oQuery.addWHERE("RMAR_REPAIR_ISFILL", ":RMAR_REPAIR_ISFILL", 1, OracleType.Int16)

                Dim sSQL As String = "SELECT * FROM RMAREPAIR " &
                        " WHERE RMAR_RMADID=:RMAR_RMADID AND RMAR_REPAIR_ISFILL=:RMAR_REPAIR_ISFILL"

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = True
                End If

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 儲存維修項目
        ''' </summary>
        ''' <param name="dtRMARepair"></param>
        ''' <param name="dtRepairDetail"></param>
        ''' <remarks></remarks>
        Public Sub Save(ByVal dtRMARepair As RmaDTO.RMARepairDataTable, ByVal dtRepairDetail As RmaDTO.RMARepair_DetailDataTable, ByVal isRepair As Boolean)
            Dim i As Integer = 0
            Dim blnRepair As Boolean = False

            Dim oRepair As New ctlRMA.Repair

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)


            oConn.Open()
            Try
                oConn.BeginTransaction()

                '檢核是否有 維修單頭(RMAREPAIR Table)資料 , true:有, false:無
                Dim drRepair As RmaDTO.RMARepairRow = dtRMARepair.Rows(0)
                blnRepair = oRepair.chkIsExist(oExecute, drRepair.RMAR_RMADID)

                If blnRepair = True Then
                    'update RMAREPAIR
                    Call Edit_RMARepair(oExecute, drRepair, False, isRepair)
                Else
                    'addnew RMAREPAIR
                    Call AddNew_RMARepair(oExecute, drRepair, False, isRepair)
                End If

                'add RMARepairDetail
                Call Delete_RMARepairDetail(oExecute, drRepair.RMAR_RMADID)
                Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView
                dvRepairDetail.RowFilter = "RMARED_MARK=0"
                Call AddNew_RMARepairDetail(oExecute, dvRepairDetail)


                ' '' ''add RMARepairDetail
                '' ''Dim dvRepairDetail As DataView = dtRepairDetail.DefaultView
                '' ''dvRepairDetail.RowFilter = "RMARED_oldID='' and RMARED_MARK=0"
                '' ''Call AddNew_RMARepairDetail(oExecute, dvRepairDetail)

                ' '' ''update RMARepairDetail
                '' ''dvRepairDetail.RowFilter = "RMARED_oldID<>''"
                '' ''Call Edit_RMARepairDetail(oExecute, dvRepairDetail)

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
        ''' 新增 RMARepair 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="drRMARepair">RMARepairRow</param>
        ''' <param name="isRepairQuoted">是否為維修報價機制</param>
        ''' <param name="isRepair">是否為維修機制</param>
        ''' <remarks></remarks>
        Public Sub AddNew_RMARepair(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal drRMARepair As RmaDTO.RMARepairRow, ByVal isRepairQuoted As Boolean, ByVal isRepair As Boolean)
            Dim oGuid As Guid = Guid.NewGuid

            Try
                Dim dr As RmaDTO.RMARepairRow = drRMARepair

                Dim sGUID As String = oGuid.ToString
                oExecute.addParameter("RMAR_ID", sGUID, OracleType.VarChar)

                oExecute.addParameter("RMAR_RMADID", dr.RMAR_RMADID, OracleType.VarChar)
                oExecute.addParameter("RMAR_COMPNO", dr.RMAR_COMPNO, OracleType.VarChar)

                If dr.IsRMAR_DUTYNONull = False Then oExecute.addParameter("RMAR_DUTYNO", dr.RMAR_DUTYNO, OracleType.VarChar)
                If dr.IsRMAR_FARCNONull = False Then oExecute.addParameter("RMAR_FARCNO", dr.RMAR_FARCNO, OracleType.VarChar)
                If dr.IsRMAR_FARNONull = False Then oExecute.addParameter("RMAR_FARNO", dr.RMAR_FARNO, OracleType.VarChar)

                If dr.IsRMAR_PROBLEMDESCNull = False Then oExecute.addParameter("RMAR_PROBLEMDESC", dr.RMAR_PROBLEMDESC, OracleType.NVarChar)
                If dr.IsRMAR_REPAIRDESCNull = False Then oExecute.addParameter("RMAR_REPAIRDESC", dr.RMAR_REPAIRDESC, OracleType.NVarChar)
                If dr.IsRMAR_REPAIRMEMONull = False Then oExecute.addParameter("RMAR_REPAIRMEMO", dr.RMAR_REPAIRMEMO, OracleType.NVarChar)


                '是否已填寫維修報價單:0.否, 1.是
                oExecute.addParameter("RMAR_REPAIR_ISFILL", dr.RMAR_REPAIR_ISFILL, OracleType.Int16)

                If isRepairQuoted = False And isRepair = True Then
                    If dr.IsRMAR_LABORHOURNull = False Then oExecute.addParameter("RMAR_LABORHOUR", dr.RMAR_LABORHOUR, OracleType.Double)
                    If dr.IsRMAR_LABORPRICENull = False Then oExecute.addParameter("RMAR_LABORPRICE", dr.RMAR_LABORPRICE, OracleType.Double)
                    If dr.IsRMAR_LABORCOSTNull = False Then oExecute.addParameter("RMAR_LABORCOST", dr.RMAR_LABORCOST, OracleType.Double)
                    If dr.IsRMAR_MATERIALCOSTNull = False Then oExecute.addParameter("RMAR_MATERIALCOST", dr.RMAR_MATERIALCOST, OracleType.Double)
                    If dr.IsRMAR_QUOTENull = False Then oExecute.addParameter("RMAR_QUOTE", dr.RMAR_QUOTE, OracleType.Double)

                    If dr.IsRMAR_CURRENCYCODENull = False Then oExecute.addParameter("RMAR_CURRENCYCODE", dr.RMAR_CURRENCYCODE, OracleType.VarChar)
                    If dr.IsRMAR_CURRENCYRATENull = False Then oExecute.addParameter("RMAR_CURRENCYRATE", dr.RMAR_CURRENCYRATE, OracleType.Double)

                    If dr.IsRMAR_ASSIGELABORCOSTNull = False Then oExecute.addParameter("RMAR_ASSIGELABORCOST", dr.RMAR_ASSIGELABORCOST, OracleType.Double)
                    If dr.IsRMAR_ASSIGEMATERIALCOSTNull = False Then oExecute.addParameter("RMAR_ASSIGEMATERIALCOST", dr.RMAR_ASSIGEMATERIALCOST, OracleType.Double)
                    If dr.IsRMAR_ASSIGEQUOTENull = False Then oExecute.addParameter("RMAR_ASSIGEQUOTE", dr.RMAR_ASSIGEQUOTE, OracleType.Double)
                    If dr.IsRMAR_ASSIGECURRENCYCODENull = False Then oExecute.addParameter("RMAR_ASSIGECURRENCYCODE", dr.RMAR_ASSIGECURRENCYCODE, OracleType.VarChar)
                    If dr.IsRMAR_ASSIGECURRENCYRATENull = False Then oExecute.addParameter("RMAR_ASSIGECURRENCYRATE", dr.RMAR_ASSIGECURRENCYRATE, OracleType.Double)

                    oExecute.addParameter("RMAR_REPAIRAD", dr.RMAR_REPAIRAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAR_REPAIRADNAME", dr.RMAR_REPAIRADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAR_REPAIRDATE", Date.Now, OracleType.DateTime)
                End If

                oExecute.addParameter("RMAR_AD", dr.RMAR_AD, OracleType.NVarChar)
                oExecute.addParameter("RMAR_ADNAME", dr.RMAR_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAR_CSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("RMAR_LUAD", dr.RMAR_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMAR_LUADNAME", dr.RMAR_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAR_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.Command("RMAREPAIR", Execute.eumCommandType.AddNew)


                '========================================================================================================
                '變更RMA 品項狀態
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '========================================================================================================
                If isRepairQuoted = True And isRepair = False Then
                    oExecute.addParameter("RMAD_STATUS", 30, OracleType.Int16)
                    oExecute.addWHERE("RMAD_ID", dr.RMAR_RMADID, OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                End If

                If isRepairQuoted = False And isRepair = True Then
                    oExecute.addParameter("RMAD_STATUS", 60, OracleType.Int16)
                    oExecute.addWHERE("RMAD_ID", dr.RMAR_RMADID, OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                End If


            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改 RMARepair 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="drRMARepair">RMARepairRow</param>
        ''' <param name="isRepairQuoted">是否為維修報價機制</param>
        ''' <param name="isRepair">是否為維修機制</param>
        ''' <remarks></remarks>
        Public Sub Edit_RMARepair(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal drRMARepair As RmaDTO.RMARepairRow, ByVal isRepairQuoted As Boolean, ByVal isRepair As Boolean)

            Try
                Dim dr As RmaDTO.RMARepairRow = drRMARepair

                oExecute.addParameter("RMAR_COMPNO", dr.RMAR_COMPNO, OracleType.VarChar)

                If dr.IsRMAR_DUTYNONull = False Then oExecute.addParameter("RMAR_DUTYNO", dr.RMAR_DUTYNO, OracleType.VarChar)
                If dr.IsRMAR_FARCNONull = False Then oExecute.addParameter("RMAR_FARCNO", dr.RMAR_FARCNO, OracleType.VarChar)
                If dr.IsRMAR_FARNONull = False Then oExecute.addParameter("RMAR_FARNO", dr.RMAR_FARNO, OracleType.VarChar)

                If dr.IsRMAR_PROBLEMDESCNull = False Then oExecute.addParameter("RMAR_PROBLEMDESC", dr.RMAR_PROBLEMDESC, OracleType.NVarChar)
                If dr.IsRMAR_REPAIRDESCNull = False Then oExecute.addParameter("RMAR_REPAIRDESC", dr.RMAR_REPAIRDESC, OracleType.NVarChar)
                If dr.IsRMAR_REPAIRMEMONull = False Then oExecute.addParameter("RMAR_REPAIRMEMO", dr.RMAR_REPAIRMEMO, OracleType.NVarChar)


                '防止填寫維修項目時, 異動到此欄位資料
                If isRepair = False Then
                    '是否已填寫維修報價單:0.否, 1.是
                    oExecute.addParameter("RMAR_REPAIR_ISFILL", dr.RMAR_REPAIR_ISFILL, OracleType.Int16)
                End If

                oExecute.addParameter("RMAR_LUAD", dr.RMAR_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMAR_LUADNAME", dr.RMAR_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAR_LUSTMP", Date.Now, OracleType.DateTime)

                If dr.IsRMAR_LABORHOURNull = False Then oExecute.addParameter("RMAR_LABORHOUR", dr.RMAR_LABORHOUR, OracleType.Double)
                If dr.IsRMAR_LABORPRICENull = False Then oExecute.addParameter("RMAR_LABORPRICE", dr.RMAR_LABORPRICE, OracleType.Double)
                If dr.IsRMAR_LABORCOSTNull = False Then oExecute.addParameter("RMAR_LABORCOST", dr.RMAR_LABORCOST, OracleType.Double)
                If dr.IsRMAR_MATERIALCOSTNull = False Then oExecute.addParameter("RMAR_MATERIALCOST", dr.RMAR_MATERIALCOST, OracleType.Double)
                If dr.IsRMAR_QUOTENull = False Then oExecute.addParameter("RMAR_QUOTE", dr.RMAR_QUOTE, OracleType.Double)

                If dr.IsRMAR_CURRENCYCODENull = False Then oExecute.addParameter("RMAR_CURRENCYCODE", dr.RMAR_CURRENCYCODE, OracleType.VarChar)
                If dr.IsRMAR_CURRENCYRATENull = False Then oExecute.addParameter("RMAR_CURRENCYRATE", dr.RMAR_CURRENCYRATE, OracleType.Double)

                If dr.IsRMAR_ASSIGELABORCOSTNull = False Then oExecute.addParameter("RMAR_ASSIGELABORCOST", dr.RMAR_ASSIGELABORCOST, OracleType.Double)
                If dr.IsRMAR_ASSIGEMATERIALCOSTNull = False Then oExecute.addParameter("RMAR_ASSIGEMATERIALCOST", dr.RMAR_ASSIGEMATERIALCOST, OracleType.Double)
                If dr.IsRMAR_ASSIGEQUOTENull = False Then oExecute.addParameter("RMAR_ASSIGEQUOTE", dr.RMAR_ASSIGEQUOTE, OracleType.Double)
                If dr.IsRMAR_ASSIGECURRENCYCODENull = False Then oExecute.addParameter("RMAR_ASSIGECURRENCYCODE", dr.RMAR_ASSIGECURRENCYCODE, OracleType.VarChar)
                If dr.IsRMAR_ASSIGECURRENCYRATENull = False Then oExecute.addParameter("RMAR_ASSIGECURRENCYRATE", dr.RMAR_ASSIGECURRENCYRATE, OracleType.Double)

                If isRepairQuoted = False And isRepair = True Then

                    oExecute.addParameter("RMAR_REPAIRAD", dr.RMAR_REPAIRAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAR_REPAIRADNAME", dr.RMAR_REPAIRADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAR_REPAIRDATE", Date.Now, OracleType.DateTime)
                End If

                oExecute.addWHERE("RMAR_RMADID", dr.RMAR_RMADID, OracleType.VarChar)
                oExecute.Command("RMAREPAIR", Execute.eumCommandType.UPDATE)


                '========================================================================================================
                '變更RMA 品項狀態
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '========================================================================================================
                '品項狀態=20, 才可以變更
                Dim oRMAStatus As New ctlRMA.RMAStatus
                Dim iStatus As Integer = oRMAStatus.getItemStatus(oExecute, dr.RMAR_RMADID)
                If iStatus = 20 Then
                    If isRepairQuoted = True And isRepair = False Then
                        oExecute.addParameter("RMAD_STATUS", 30, OracleType.Int16)
                        oExecute.addWHERE("RMAD_ID", dr.RMAR_RMADID, OracleType.VarChar)
                        oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                    End If
                End If


                If isRepairQuoted = False And isRepair = True Then
                    oExecute.addParameter("RMAD_STATUS", 60, OracleType.Int16)
                    oExecute.addWHERE("RMAD_ID", dr.RMAR_RMADID, OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                End If

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 新增 RMARepair Detail 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dvRepairDetail">RMARepair_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub AddNew_RMARepairDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dvRepairDetail As DataView)
            Dim i As Integer = 0

            Try

                For i = 0 To dvRepairDetail.Count - 1
                    Dim oGuid As Guid = Guid.NewGuid
                    Dim sGUID As String = oGuid.ToString

                    Dim dr As RmaDTO.RMARepair_DetailRow = dvRepairDetail(i).Row

                    oExecute.addParameter("RMARED_ID", sGUID, OracleType.VarChar)
                    'oExecute.addParameter("RMARED_ID", dr.RMARED_ID, OracleType.VarChar)
                    oExecute.addParameter("RMARED_RMADID", dr.RMARED_RMADID, OracleType.VarChar)

                    oExecute.addParameter("RMARED_NPARTNO", dr.RMARED_NPARTNO, OracleType.VarChar)
                    oExecute.addParameter("RMARED_NSERIALNO", dr.RMARED_NSERIALNO, OracleType.VarChar)
                    If dr.IsRMARED_NWARRANTYNull = False Then oExecute.addParameter("RMARED_NWARRANTY", dr.RMARED_NWARRANTY, OracleType.DateTime)

                    If dr.IsRMARED_OPARTNONull = False Then oExecute.addParameter("RMARED_OPARTNO", dr.RMARED_OPARTNO, OracleType.VarChar)
                    If dr.IsRMARED_OSERIALNONull = False Then oExecute.addParameter("RMARED_OSERIALNO", dr.RMARED_OSERIALNO, OracleType.VarChar)
                    If dr.IsRMARED_OWARRANTYNull = False Then oExecute.addParameter("RMARED_OWARRANTY", dr.RMARED_OWARRANTY, OracleType.DateTime)

                    oExecute.addParameter("RMARED_IMPROPERUSAGE", dr.RMARED_IMPROPERUSAGE, OracleType.Int16)
                    If dr.IsRMARED_DESCNull = False Then oExecute.addParameter("RMARED_DESC", dr.RMARED_DESC, OracleType.NVarChar)
                    If dr.IsRMARED_LOCATIONNull = False Then oExecute.addParameter("RMARED_LOCATION", dr.RMARED_LOCATION, OracleType.VarChar)
                    If dr.IsRMARED_DEFECTIVENull = False Then oExecute.addParameter("RMARED_DEFECTIVE", dr.RMARED_DEFECTIVE, OracleType.NVarChar)

                    oExecute.addParameter("RMARED_QTY", dr.RMARED_QTY, OracleType.Int16)
                    oExecute.addParameter("RMARED_MATERIALCOST", dr.RMARED_MATERIALCOST, OracleType.Double)
                    oExecute.addParameter("RMARED_PRICE", dr.RMARED_PRICE, OracleType.Double)

                    oExecute.addParameter("RMARED_AD", dr.RMARED_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_ADNAME", dr.RMARED_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARED_LUAD", dr.RMARED_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_LUADNAME", dr.RMARED_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARED_MARK", dr.RMARED_MARK, OracleType.Double)

                    oExecute.addParameter("RMARED_CURRENCYCODE", dr.RMARED_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARED_CURRENCYRATE", dr.RMARED_CURRENCYRATE, OracleType.Double)

                    oExecute.addParameter("RMARED_ASSIGECURRENCYCODE", dr.RMARED_ASSIGECURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARED_ASSIGECURRENCYRATE", dr.RMARED_ASSIGECURRENCYRATE, OracleType.Double)
                    oExecute.addParameter("RMARED_ASSIGEPRICE", dr.RMARED_ASSIGEPRICE, OracleType.Double)

                    If dr.IsRMARED_WAIVENull = False Then oExecute.addParameter("RMARED_WAIVE", dr.RMARED_WAIVE, OracleType.NVarChar)
                    If dr.IsRMARED_OPTIONNull = False Then oExecute.addParameter("RMARED_OPTION", dr.RMARED_OPTION, OracleType.NVarChar)
                    If dr.IsRMARED_ISSOURCENull = False Then oExecute.addParameter("RMARED_ISSOURCE", dr.RMARED_ISSOURCE, OracleType.NVarChar)

                    oExecute.Command("RMAREPAIR_DETAIL", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try



        End Sub

        ''' <summary>
        ''' 新增 RMARepair Detail 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dvRepairDetail">RMARepair_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub Edit_RMARepairDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dvRepairDetail As DataView)
            Dim i As Integer = 0

            Try

                For i = 0 To dvRepairDetail.Count - 1
                    Dim dr As RmaDTO.RMARepair_DetailRow = dvRepairDetail(i).Row

                    oExecute.addParameter("RMARED_NPARTNO", dr.RMARED_NPARTNO, OracleType.VarChar)
                    If dr.IsRMARED_NSERIALNONull = False Then oExecute.addParameter("RMARED_NSERIALNO", dr.RMARED_NSERIALNO, OracleType.VarChar)
                    If dr.IsRMARED_NWARRANTYNull = False Then oExecute.addParameter("RMARED_NWARRANTY", dr.RMARED_NWARRANTY, OracleType.DateTime)

                    If dr.IsRMARED_OPARTNONull = False Then oExecute.addParameter("RMARED_OPARTNO", dr.RMARED_OPARTNO, OracleType.VarChar)
                    If dr.IsRMARED_OSERIALNONull = False Then oExecute.addParameter("RMARED_OSERIALNO", dr.RMARED_OSERIALNO, OracleType.VarChar)
                    If dr.IsRMARED_OWARRANTYNull = False Then oExecute.addParameter("RMARED_OWARRANTY", dr.RMARED_OWARRANTY, OracleType.DateTime)

                    oExecute.addParameter("RMARED_IMPROPERUSAGE", dr.RMARED_IMPROPERUSAGE, OracleType.Int16)
                    If dr.IsRMARED_DESCNull = False Then oExecute.addParameter("RMARED_DESC", dr.RMARED_DESC, OracleType.NVarChar)
                    If dr.IsRMARED_LOCATIONNull = False Then oExecute.addParameter("RMARED_LOCATION", dr.RMARED_LOCATION, OracleType.VarChar)
                    If dr.IsRMARED_DEFECTIVENull = False Then oExecute.addParameter("RMARED_DEFECTIVE", dr.RMARED_DEFECTIVE, OracleType.NVarChar)

                    oExecute.addParameter("RMARED_QTY", dr.RMARED_QTY, OracleType.Int16)
                    oExecute.addParameter("RMARED_MATERIALCOST", dr.RMARED_MATERIALCOST, OracleType.Double)
                    oExecute.addParameter("RMARED_PRICE", dr.RMARED_PRICE, OracleType.Double)

                    oExecute.addParameter("RMARED_LUAD", dr.RMARED_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_LUADNAME", dr.RMARED_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARED_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARED_MARK", dr.RMARED_MARK, OracleType.Double)

                    oExecute.addParameter("RMARED_CURRENCYCODE", dr.RMARED_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARED_CURRENCYRATE", dr.RMARED_CURRENCYRATE, OracleType.Double)

                    oExecute.addParameter("RMARED_ASSIGECURRENCYCODE", dr.RMARED_ASSIGECURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARED_ASSIGECURRENCYRATE", dr.RMARED_ASSIGECURRENCYRATE, OracleType.Double)
                    oExecute.addParameter("RMARED_ASSIGEPRICE", dr.RMARED_ASSIGEPRICE, OracleType.Double)

                    oExecute.addWHERE("RMARED_ID", dr.RMARED_ID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_DETAIL", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try



        End Sub

        ''' <summary>
        ''' Delete RMARepair Detail 單頭
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="RMARED_RMADID"></param>
        ''' <remarks></remarks>
        Private Sub Delete_RMARepairDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMARED_RMADID As String)
            Dim i As Integer = 0

            Try
                'oExecute.addWHERE("RMARED_RMADID", RMARED_RMADID, OracleType.VarChar)
                'oExecute.Command("RMAREPAIR_DETAIL", Execute.eumCommandType.Delete)


                oExecute.addParameter("RMARED_MARK", 1, OracleType.Int16)

                oExecute.addWHERE("RMARED_RMADID", RMARED_RMADID, OracleType.VarChar)
                oExecute.Command("RMAREPAIR_DETAIL", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally

            End Try



        End Sub

        ''' <summary>
        ''' RMARepair_Upload-新增
        ''' </summary>
        ''' <param name="dtRepairUpload">傳入要新增的資料</param>
        ''' <remarks></remarks>
        Public Sub SaveUpload(ByVal dtRepairUpload As RmaDTO.RMAREPAIR_UPLOADDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()
                Dim dr As RmaDTO.RMAREPAIR_UPLOADRow = dtRepairUpload.Rows(0)

                oExecute.addParameter("RMARU_ID", dr.RMARU_ID, OracleType.VarChar)
                oExecute.addParameter("RMARU_RMANO", dr.RMARU_RMANO, OracleType.VarChar)
                oExecute.addParameter("RMARU_UPLOADFILE", dr.RMARU_UPLOADFILE, OracleType.VarChar)
                oExecute.addParameter("RMARU_DESC", dr.RMARU_DESC, OracleType.NVarChar)

                oExecute.addParameter("RMARU_AD", dr.RMARU_AD, OracleType.NVarChar)
                oExecute.addParameter("RMARU_ADNAME", dr.RMARU_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMARU_CSTMP", dr.RMARU_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMARU_LUAD", dr.RMARU_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMARU_LUADNAME", dr.RMARU_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMARU_LUSTMP", dr.RMARU_LUSTMP, OracleType.DateTime)

                oExecute.Command("RMAREPAIR_UPLOAD", Execute.eumCommandType.AddNew)
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
        ''' RMARepair_Upload-刪除
        ''' </summary>
        ''' <param name="RMARU_ID">維修上傳檔案識別碼</param>
        ''' <remarks></remarks>
        Public Sub DeleteUpload(ByVal RMARU_ID As String)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                oExecute.addWHERE("RMARU_ID", RMARU_ID.Trim(), OracleType.VarChar)
                oExecute.Command("RMAREPAIR_UPLOAD", Execute.eumCommandType.Delete)
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
#End Region

#Region "Class:Repair:維修報價"
    Public Class Repair_Quoting
        Dim _RepairNo_flowCase01 As String = ConfigurationSettings.AppSettings("RepairNo_flowCase01")
        Dim _HQRepairNo As String = ConfigurationSettings.AppSettings("HQRepairNo")

        ''' <summary>
        ''' 取得 維修報價 資料
        ''' </summary>
        ''' <param name="RMAD_ID">傳入 RMAD ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal RMAD_ID As String, ByVal NOTIN_RMAD_STATUS As String) As RmaDTO.vwRepair_QuotingDataTable
            Dim sCondition As String = ""
            Dim dtRepairQuoting As New RmaDTO.vwRepair_QuotingDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"

                If NOTIN_RMAD_STATUS <> "" Then
                    sCondition = sCondition & " AND RMAD_STATUS NOT IN(" + NOTIN_RMAD_STATUS + ")"
                End If

                Dim sSQL As String = "SELECT * FROM vwRepair_Quoting  WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuoting)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairQuoting
        End Function

        ''' <summary>
        ''' 取得已維修報價表頭 的資料
        ''' </summary>
        ''' <param name="RMARQ_RMADID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairQuoted(ByVal RMARQ_RMADID As String) As RmaDTO.RMARepair_QuotedDataTable
            Dim sCondition As String = ""
            Dim dtRepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMARQ_RMADID", ":RMARQ_RMADID", RMARQ_RMADID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARQ_RMADID=:RMARQ_RMADID"

                Dim sSQL As String = "SELECT * FROM RMAREPAIR_QUOTED WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuoted)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairQuoted
        End Function

        ''' <summary>
        ''' 取得已維修報價 零件的資料
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>RMADetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairQuotedDetail(ByVal RMAD_ID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim sCondition As String = ""
            Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RMARQD_ID") = "RMARQD_oldID"

                If OrderBY.Trim = "" Then
                    OrderBY = " RMARQD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMARQD_RMADID", ":RMARQD_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARQD_RMADID=:RMARQD_RMADID"

                Dim sSQL As String = "SELECT RMARQD_ID,"
                sSQL = sSQL & " RMARQD_RMADID, RMARQD_NPARTNO, RMARQD_NSERIALNO, RMARQD_NWARRANTY, "
                sSQL = sSQL & " RMARQD_OPARTNO, RMARQD_OSERIALNO, RMARQD_OWARRANTY, "
                sSQL = sSQL & " RMARQD_DESC, RMARQD_LOCATION, RMARQD_IMPROPERUSAGE, RMARQD_DEFECTIVE, "
                sSQL = sSQL & " RMARQD_QTY, RMARQD_MATERIALCOST, RMARQD_PRICE, RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, "
                sSQL = sSQL & " RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE, "
                sSQL = sSQL & " RMARQD_AD, RMARQD_ADNAME, RMARQD_CSTMP, RMARQD_LUAD, RMARQD_LUADNAME, RMARQD_LUSTMP, RMARQD_MARK,RMARQD_ACC, "
                sSQL = sSQL & " NVL(RMARQD_WAIVE,0) as RMARQD_WAIVE, NVL(RMARQD_OPTION,0) as RMARQD_OPTION, NVL(RMARQD_OPTIONCLIENT,2) as RMARQD_OPTIONCLIENT"
                sSQL = sSQL & " FROM RMAREPAIR_QUOTED_DETAIL WHERE RMARQD_MARK=0"
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuotedDetail, HasExceColumn)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairQuotedDetail
        End Function

        ''' <summary>
        ''' 客戶端 取得已維修報價 零件的資料
        ''' </summary>
        ''' <param name="RMAD_ID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairQuotedDetail_Client(ByVal RMAD_ID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim sCondition As String = ""
            Dim dtRepairQuotedDetail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("RMARQD_ID") = "RMARQD_oldID"

                If OrderBY.Trim = "" Then
                    OrderBY = " RMARQD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMARQD_RMADID", ":RMARQD_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARQD_RMADID=:RMARQD_RMADID"

                Dim sSQL As String = "SELECT RMARQD_ID,"
                sSQL = sSQL & " RMARQD_RMADID, RMARQD_NPARTNO, RMARQD_NSERIALNO, RMARQD_NWARRANTY, "
                sSQL = sSQL & " RMARQD_OPARTNO, RMARQD_OSERIALNO, RMARQD_OWARRANTY, "

                sSQL = sSQL & " RMARQD_DESC, "
                sSQL = sSQL & " RMARQD_LOCATION, RMARQD_IMPROPERUSAGE, RMARQD_DEFECTIVE, "

                sSQL = sSQL & " RMARQD_QTY, "
                sSQL = sSQL & " RMARQD_MATERIALCOST, "
                sSQL = sSQL & " RMARQD_PRICE, RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, "

                sSQL = sSQL & " RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE, "
                sSQL = sSQL & " RMARQD_AD, RMARQD_ADNAME, RMARQD_CSTMP, RMARQD_LUAD, RMARQD_LUADNAME, RMARQD_LUSTMP, RMARQD_MARK, "
                sSQL = sSQL & " NVL(RMAREPAIR_QUOTED_DETAIL.RMARQD_WAIVE,0) as RMARQD_WAIVE, NVL(RMAREPAIR_QUOTED_DETAIL.RMARQD_OPTION,0) as RMARQD_OPTION ,NVL (RMAREPAIR_QUOTED_DETAIL.RMARQD_ACC, 0) AS RMARQD_ACC, RMAREPAIR_QUOTED_DETAIL.RMARQD_OPTIONCLIENT "

                sSQL = sSQL & " FROM RMAREPAIR_QUOTED "

                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED_DETAIL "
                sSQL = sSQL & "     ON RMARQ_RMADID = RMARQD_RMADID"

                sSQL = sSQL & " LEFT OUTER JOIN REPAIRBOM "
                sSQL = sSQL & " ON RMARQ_COMPNO = RPBOM_COMPNO AND RPBOM_PARTNO = RMARQD_NPARTNO"

                sSQL = sSQL & " WHERE RMARQD_MARK=0 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuotedDetail, HasExceColumn)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairQuotedDetail
        End Function

        ''' <summary>
        ''' 取得已維修報價 零件的資料 HQ維修人重新更新
        ''' </summary>
        ''' <param name="RMAD_ID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRepairQuotedDetail_HQ(ByVal RMAD_ID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMAREPAIR_QUOTED_DETAIL_HQDataTable
            Dim sCondition As String = ""
            Dim dtRepairQuotedDetail_HQ As New RmaDTO.RMAREPAIR_QUOTED_DETAIL_HQDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                If OrderBY.Trim = "" Then
                    OrderBY = " RMARQD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMARQD_RMADID", ":RMARQD_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMARQD_RMADID=:RMARQD_RMADID"

                Dim sSQL As String = "SELECT RMARQD_ID,"
                sSQL = sSQL & " RMARQD_RMADID, RMARQD_NPARTNO, RMARQD_NSERIALNO, RMARQD_NWARRANTY, "
                sSQL = sSQL & " RMARQD_OPARTNO, RMARQD_OSERIALNO, RMARQD_OWARRANTY, "

                sSQL = sSQL & " NVL(RMARQD_DESC, RPBOM_DESC) as RMARQD_DESC,"

                sSQL = sSQL & " RMARQD_LOCATION, RMARQD_IMPROPERUSAGE, RMARQD_DEFECTIVE, "
                sSQL = sSQL & " RMARQD_QTY, "

                sSQL = sSQL & " CASE WHEN RMARQD_MATERIALCOST =0  THEN NVL(RPBOM_MATERIALCOST,0) ELSE NVL(RMARQD_MATERIALCOST,0) END  as RMARQD_MATERIALCOST, "
                sSQL = sSQL & " RMARQD_PRICE, RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, "

                sSQL = sSQL & " RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE, "
                sSQL = sSQL & " RMARQD_AD, RMARQD_ADNAME, RMARQD_CSTMP, RMARQD_LUAD, RMARQD_LUADNAME, RMARQD_LUSTMP, RMARQD_MARK, "
                sSQL = sSQL & " NVL(RMARQD_WAIVE,0) as RMARQD_WAIVE, NVL(RMARQD_OPTION,0) as RMARQD_OPTION , RMARQD_OPTIONCLIENT, "

                sSQL = sSQL & "CASE WHEN RMARQD_MATERIALCOST =0  THEN 1 "
                sSQL = sSQL & "     ELSE "
                sSQL = sSQL & "         CASE WHEN RMARQD_DESC IS NULL  THEN 1 ELSE 0 END"
                sSQL = sSQL & " END as isAbnormal"

                sSQL = sSQL & " FROM RMAREPAIR_QUOTED "

                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED_DETAIL "
                sSQL = sSQL & "     ON RMARQ_RMADID = RMARQD_RMADID"

                sSQL = sSQL & " LEFT OUTER JOIN REPAIRBOM "
                sSQL = sSQL & " ON RMARQ_COMPNO = RPBOM_COMPNO AND RPBOM_PARTNO = RMARQD_NPARTNO"

                sSQL = sSQL & " WHERE RMARQD_MARK=0 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuotedDetail_HQ)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairQuotedDetail_HQ
        End Function

        ''' <summary>
        ''' 取得可以需要讓 HQ維修人重新更新, 將未定義的維修報價零件的 規格或單價 補齊
        ''' </summary>
        ''' <param name="COMPNo">維修中心代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="fdate"></param>
        ''' <param name="edate"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryHQRepairQuote(ByVal COMPNo As String, ByVal RMANo As String, ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.VWHQREPAIRQUOTEDataTable

            Dim i As Integer = 0
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtHQRepairQuote As New RmaDTO.VWHQREPAIRQUOTEDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = COMPNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim().ToLower() & "%"
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMA_NO) like :RMA_NO"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.Trim().ToLower() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 
                '70.Shipped, 90.Closed, 91.Canceled
                sSQL = sSQL & " SELECT RMA_NO, RMA_ID, RMA_COMPNO, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(TotalCount,0) TotalCount, nvl(RecvCount,0) RecvCount, nvl(WIPCount,0) WIPCount, nvl(RepairingCount,0) RepairingCount"
                sSQL = sSQL & " FROM rma inner join CUSTOMER"
                sSQL = sSQL & "     ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=10 OR RMA_STATUS=20)"
                sSQL = sSQL & " INNER JOIN rmadetail ON RMA.RMA_NO = rmadetail.rmad_rmano"
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmad_rmano, Count(*) TotalCount FROM rmadetail"
                sSQL = sSQL & "     WHERE rmad_mark=0 group BY rmad_rmano"
                sSQL = sSQL & " ) vwRMADetail"
                sSQL = sSQL & " ON RMA.RMA_NO = vwRMADetail.rmad_rmano"

                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED on RMAD_ID = RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED_DETAIL on RMARQD_RMADID = RMAD_ID and RMARQD_MARK=0"

                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmad_rmano, Count(*) RecvCount FROM rmadetail"
                sSQL = sSQL & "     WHERE rmad_mark=0 AND (RMAD_STATUS=20 OR RMAD_RECEVSTATUS<>0) AND RMAD_STATUS <> 91 group BY rmad_rmano"
                sSQL = sSQL & " ) vwRecv"
                sSQL = sSQL & " ON RMA.RMA_NO = vwRecv.rmad_rmano"

                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmad_rmano, Count(*) DelCount FROM rmadetail"
                sSQL = sSQL & "     WHERE rmad_mark=0 AND RMAD_STATUS = 91 group BY rmad_rmano"
                sSQL = sSQL & " ) vwDel"
                sSQL = sSQL & " ON RMA.RMA_NO = vwDel.rmad_rmano"

                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmad_rmano, Count(*) WIPCount FROM rmadetail"
                sSQL = sSQL & "     WHERE rmad_mark=0 AND RMAD_STATUS IN('30') group BY rmad_rmano"
                sSQL = sSQL & " ) vwWIP"
                sSQL = sSQL & " ON RMA.RMA_NO = vwWIP.rmad_rmano"

                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmad_rmano, Count(*) RepairingCount FROM rmadetail"
                sSQL = sSQL & "     LEFT OUTER JOIN rmarepair on rmad_id= rmar_rmadid"
                sSQL = sSQL & "     WHERE rmad_mark=0 AND RMAD_STATUS IN('50','60') and RMAR_REPAIRAD is null group BY rmad_rmano"
                sSQL = sSQL & " ) vwRepairing"
                sSQL = sSQL & " ON RMA.RMA_NO = vwRepairing.rmad_rmano"

                sSQL = sSQL & " WHERE (RMARQ_ID IS NULL OR nvl(RMARQD_DESC,' ')=' '  OR nvl(RMARQD_PRICE,0)=0) AND (RMAD_STATUS>20 AND RMAD_STATUS<40) " & sCondition
                sSQL = sSQL & " group by RMA_NO, RMA_ID, RMA_COMPNO, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(TotalCount,0), nvl(RecvCount,0), nvl(WIPCount,0), nvl(RepairingCount,0)"
                sSQL = sSQL & OrderBY


                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtHQRepairQuote)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtHQRepairQuote
        End Function

        ''' <summary>
        ''' 檢核  RMAR_RMADID 是否存在
        ''' </summary>
        ''' <param name="RMARQ_RMADID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkIsExist(ByVal RMARQ_RMADID As String) As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMARQ_RMADID", ":RMARQ_RMADID", RMARQ_RMADID.Trim(), OracleType.VarChar)

                Dim sSQL As String = "SELECT * FROM RMAREPAIR_QUOTED WHERE RMARQ_RMADID=:RMARQ_RMADID "

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
        ''' 儲存-報價單
        ''' </summary>
        ''' <param name="dtRMARepair"></param>
        ''' <param name="dtRMARepairQuoted"></param>
        ''' <param name="dtRepairQuotedDetail"></param>
        ''' <param name="isRepairQuoted">是否為維修報價機制</param>
        ''' <param name="isWarranty">是否保固日期內:null.未定(Unidentified), 0.否, 1.是</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="isFlowCase">要執行哪種流程, 用客戶申請時表單的維修中心判斷是否要執行 flow case 01</param>
        ''' <remarks></remarks>
        Public Sub Save(ByVal dtRMARepair As RmaDTO.RMARepairDataTable, ByVal dtRMARepairQuoted As RmaDTO.RMARepair_QuotedDataTable, ByVal dtRepairQuotedDetail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable,
            ByVal isRepairQuoted As Boolean, ByVal isWarranty As String, ByVal isCWarranty As String, ByVal isSWarranty As String, ByVal ModelNo As String, ByVal SerialNo As String, ByVal bIncludeTop As Boolean,
            ByVal isFlowCase As String, ByVal sRMANo As String, Optional ByVal Apply_BI As String = "0")

            Dim i As Integer = 0
            Dim oExport As New ctlRMA.Export
            Dim oLanguage As New ctlLanguage

            Dim blnRepair As Boolean = False
            Dim blnQuoted As Boolean = False

            Dim oRepair As New ctlRMA.Repair

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)


            oConn.Open()
            Try
                '檢核是否有 維修單頭(RMAREPAIR Table)資料 , true:有, false:無
                Dim drRepair As RmaDTO.RMARepairRow = dtRMARepair.Rows(0)
                blnRepair = oRepair.chkIsExist(oExecute, drRepair.RMAR_RMADID)

                '檢核是否有 維修報價(RMAREPAIR_QUOTED Table)資料 , true:有, false:無
                Dim drQuote As RmaDTO.RMARepair_QuotedRow = dtRMARepairQuoted.Rows(0)
                blnQuoted = chkIsExist(drQuote.RMARQ_RMADID)

                '檢核是否有 維修報價零件(ooxx Table)資料 , true:有, false:無


                oConn.BeginTransaction()

                If blnRepair = True Then
                    'update RMAREPAIR
                    Call oRepair.Edit_RMARepair(oExecute, drRepair, isRepairQuoted, False)
                Else
                    'addnew RMAREPAIR
                    Call oRepair.AddNew_RMARepair(oExecute, drRepair, isRepairQuoted, False)
                End If

                If blnQuoted = True Then
                    'update RMAREPAIR_QUOTED
                    Call Edit_Quoted(oExecute, dtRMARepairQuoted)
                Else
                    'addnew RMAREPAIR_QUOTED
                    Call AddNew_Quoted(oExecute, dtRMARepairQuoted)
                End If


                '================================================================================================================================================================================================
                '修改品項的 是否保固期內
                '================================================================================================================================================================================================
                Dim dr As RmaDTO.RMARepairRow = dtRMARepair.Rows(0)
                If isWarranty.Trim() = "" Then
                    oExecute.addParameter("RMAD_ISWARRANTY", System.Convert.DBNull, OracleType.Int16)
                Else
                    oExecute.addParameter("RMAD_ISWARRANTY", isWarranty, OracleType.Int16)
                End If
                If isCWarranty.Trim() = "" Then
                    oExecute.addParameter("RMAD_ISCW", System.Convert.DBNull, OracleType.Int16)
                Else
                    oExecute.addParameter("RMAD_ISCW", isCWarranty, OracleType.Int16)
                End If
                If isSWarranty.Trim() = "" Then
                    oExecute.addParameter("RMAD_ISSW", System.Convert.DBNull, OracleType.Int16)
                Else
                    oExecute.addParameter("RMAD_ISSW", isSWarranty, OracleType.Int16)
                End If

                If Apply_BI = "1" Then
                    oExecute.addParameter("RMAD_APPLY_BI", Apply_BI, OracleType.Int16)
                Else
                    oExecute.addParameter("RMAD_APPLY_BI", "0", OracleType.Int16)
                End If

                '2012/04/25
                'If ModelNo.Trim() <> "" Then
                '    oExecute.addParameter("RMAD_MODELNO", ModelNo.Trim(), OracleType.NVarChar)
                'End If

                If SerialNo <> "" Then
                    oExecute.addParameter("RMAD_SERIALNO", SerialNo.Trim(), OracleType.NVarChar)
                End If

                If bIncludeTop = True Then
                    '==================
                    'MODEL NO
                    '==================
                    'Dim sModelNo As String = oExport.getModelNo(SerialNo.Trim())
                    'If sModelNo.Trim() = "" Then
                    '    sModelNo = "OTHER"
                    'End If
                    'oExecute.addParameter("RMAD_MODELNO", sModelNo.Trim(), OracleType.NVarChar)

                    '==================
                    'WarrantyDate
                    '==================
                    Dim sWarranty As String = oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
                    Dim sWarrantyDate As String = oExport.getMaxWarranty(SerialNo.Trim(), dr.RMAR_COMPNO.Trim(), dr.RMAR_COMPNO)
                    If sWarrantyDate.Trim() <> "" Then
                        sWarranty = Convert.ToDateTime(sWarrantyDate).ToShortDateString()
                    End If
                    If IsDate(sWarranty) = True Then
                        oExecute.addParameter("RMAD_WARRANTY", Convert.ToDateTime(sWarranty), OracleType.DateTime)
                    Else
                        oExecute.addParameter("RMAD_WARRANTY", System.Convert.DBNull, OracleType.DateTime)
                    End If
                End If

                oExecute.addWHERE("RMAD_ID", dr.RMAR_RMADID, OracleType.VarChar)
                oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)




                '================================================================================================================================================================================================
                '維修報價 零件檔
                '================================================================================================================================================================================================
                'add RMARepairDetail
                Call Delete_RepairQuotedDetail(oExecute, drRepair.RMAR_RMADID)
                Dim dvRepairQuotedDetail As DataView = dtRepairQuotedDetail.DefaultView
                dvRepairQuotedDetail.RowFilter = "RMARQD_MARK=0"
                Call AddNew_RepairQuotedDetail(oExecute, dvRepairQuotedDetail)


                '================================================================================================================================================================================================
                '如果是 flow case 01 的狀況, RMA報價中，若其中有一筆零件規格空白或單價為0時，按confirm，零件異常訊息。
                '符合上敘條件, 如底於 維修中心訂定的維修金額也不用直接送修
                '================================================================================================================================================================================================
                Dim isRepairing As Boolean = True
                If isFlowCase = "01" Or isFlowCase = "02" Then
                    dvRepairQuotedDetail.RowFilter = "(RMARQD_DESC='' OR RMARQD_MATERIALCOST=0) AND RMARQD_MARK=0"
                    If dvRepairQuotedDetail.Count > 0 Then
                        isRepairing = False
                    End If
                End If


                '================================================================================================================================================================================================
                '檢核 底於 維修中心訂定的維修金額, 直接送修
                '================================================================================================================================================================================================
                If isRepairQuoted = True And isRepairing = True Then
                    Call LessApprovAmountToRepair(oExecute, dtRMARepairQuoted)
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
        ''' 客戶端 修改 RMARQ_ACCEPT: 是否要維修: 1.Accept, 2.Reject BY Paypal 暫存
        ''' </summary>
        ''' <param name="tmpRMAREPAIR_QUOTED_Client"></param>
        ''' <remarks></remarks>
        Public Sub Update_RepairQuoted_Client_Payment(ByVal tmpRMAREPAIR_QUOTED_Client As DataTable)
            Dim i As Integer = 0
            Dim sSQL As String = "BEGIN "
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                sSQL += "DELETE FROM RMA_AHEAD WHERE RMA_NO=:RMA_NO_O; "
                Dim oCommand As OracleCommand = oConn.Command()
                oCommand.Parameters.Add(New OracleParameter("RMA_NO_O", tmpRMAREPAIR_QUOTED_Client.Rows(0)("RMA_NO")))
                For i = 0 To tmpRMAREPAIR_QUOTED_Client.Rows.Count - 1
                    Dim dr As DataRow = tmpRMAREPAIR_QUOTED_Client.Rows(i)
                    sSQL += "INSERT INTO RMA_AHEAD (RMA_NO,RMA_SERIALNO,RMAD_ID,RMA_ACCEPT,RMA_AMT,USER_ID,USER_NAME) values "
                    sSQL += "(:RMA_NO" + i.ToString() + ",:RMA_SERIALNO" + i.ToString() + ",:RMAD_ID" + i.ToString() + ",:RMA_ACCEPT" + i.ToString() + ",:RMA_AMT" + i.ToString() + ",:USER_ID" + i.ToString() + ",:USER_NAME" + i.ToString() + ");"

                    oCommand.Parameters.Add(New OracleParameter(":RMA_NO" + i.ToString(), dr("RMA_NO")))
                    oCommand.Parameters.Add(New OracleParameter(":RMA_SERIALNO" + i.ToString(), dr("RMA_SERIALNO")))
                    oCommand.Parameters.Add(New OracleParameter(":RMAD_ID" + i.ToString(), dr("RMAD_ID")))
                    oCommand.Parameters.Add(New OracleParameter(":RMA_ACCEPT" + i.ToString(), dr("RMA_ACCEPT")))
                    oCommand.Parameters.Add(New OracleParameter(":RMA_AMT" + i.ToString(), dr("RMA_AMT")))
                    oCommand.Parameters.Add(New OracleParameter(":USER_ID" + i.ToString(), dr("USER_ID")))
                    oCommand.Parameters.Add(New OracleParameter(":USER_NAME" + i.ToString(), dr("USER_NAME")))

                    'OracleParameter param1 = New OracleParameter("Field1", "test");
                    'cmd.Parameters.Add("pDate", OracleDbType.Date).Value = date
                Next

                sSQL += "END;"
                'oCommand.Parameters.Add("RMA_NO", OracleString) = "111"
                oCommand.CommandText = sSQL
                oCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End Sub

        ''' <summary>
        ''' 客戶端 修改 RMARQ_ACCEPT: 是否要維修: 1.Accept, 2.Reject
        ''' </summary>
        ''' <param name="tmpRMAREPAIR_QUOTED_Client"></param>
        ''' <remarks></remarks>
        Public Sub Update_RepairQuoted_Client(ByVal tmpRMAREPAIR_QUOTED_Client As RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable)
            Dim i As Integer = 0

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction() ' bug修正 by buck modify 20251201
                For Each dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow In tmpRMAREPAIR_QUOTED_Client

                    If dr.IsRMARQ_LABORHOURNull = False Then
                        oExecute.addParameter("RMARQ_LABORHOUR", dr.RMARQ_LABORHOUR, OracleType.Number)
                    End If

                    If dr.IsRMARQ_QUOTENull = False Then
                        oExecute.addParameter("RMARQ_QUOTE", dr.RMARQ_QUOTE, OracleType.Number)
                    End If

                    '是否要維修: 1.Accept, 2.Reject
                    oExecute.addParameter("RMARQ_ACCEPT", dr.RMARQ_ACCEPT, OracleType.Int16)

                    'oExecute.addParameter("RMARQ_LUAD", dr.RMARQ_LUAD, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQ_LUADNAME", dr.RMARQ_LUADNAME, OracleType.NVarChar)
                    'oExecute.addParameter("RMARQ_LUSTMP", dr.RMARQ_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMARQ_ID", dr.RMARQ_ID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_QUOTED", Execute.eumCommandType.UPDATE)
                Next

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
        ''' 過期訂單執行Reject
        ''' </summary>
        ''' <param name="model"></param>
        ''' <remarks></remarks>
        ''' 逾期超過1個月的RMA報價單進行強制取消作業 by buck add 20251201
        <Obsolete>
        Public Function UpdateRMAQuotedRejectData(model As List(Of RMADetailReq)) As Result
            Dim result As New Result
            Dim oConn As New Connection
            oConn.Open()
            oConn.BeginTransaction()
            Try
                Using cmd As OracleCommand = oConn.Command()
                    For Each items As RMADetailReq In model
                        ' ========================
                        ' 更新 RMADetail
                        ' ========================

                        cmd.CommandText = "
                                            UPDATE RMADetail 
                                            SET RMAD_STATUS = :RMAD_STATUS,
                                                RMAD_LUAD = :RMAD_LUAD,
                                                RMAD_LUADNAME = :RMAD_LUADNAME,
                                                RMAD_LUSTMP = :RMAD_LUSTMP,
                                                RMAD_REJAD = :RMAD_REJAD,
                                                RMAD_REJADNAME = :RMAD_REJADNAME,
                                                RMAD_REJSTMP = :RMAD_REJSTMP
                                            WHERE RMAD_ID = :RMAD_ID
                                        "
                        cmd.Parameters.Clear()
                        cmd.Parameters.Add("RMAD_STATUS", OracleType.VarChar).Value = items.RMAD_STATUS
                        cmd.Parameters.Add("RMAD_LUAD", OracleType.VarChar).Value = items.RMAD_LUAD
                        cmd.Parameters.Add("RMAD_LUADNAME", OracleType.VarChar).Value = items.RMAD_LUADNAME
                        cmd.Parameters.Add("RMAD_LUSTMP", OracleType.DateTime).Value = items.RMAD_LUSTMP
                        cmd.Parameters.Add("RMAD_REJAD", OracleType.VarChar).Value = items.RMAD_REJAD
                        cmd.Parameters.Add("RMAD_REJADNAME", OracleType.VarChar).Value = items.RMAD_REJADNAME
                        cmd.Parameters.Add("RMAD_REJSTMP", OracleType.DateTime).Value = items.RMAD_REJSTMP
                        cmd.Parameters.Add("RMAD_ID", OracleType.VarChar).Value = items.RMAD_ID

                        cmd.ExecuteNonQuery()

                        ' ========================
                        ' 更新 RMARepair_Quoted
                        ' ========================
                        For Each rq As RMARepairQuotedReq In items.RepairQuoted
                            cmd.CommandText = "
                                                UPDATE RMAREPAIR_QUOTED
                                                SET RMARQ_ACCEPT = :RMARQ_ACCEPT,
                                                    RMARQ_QUOTE = :RMARQ_QUOTE,
                                                    RMARQ_LUSTMP = :RMARQ_LUSTMP
                                                WHERE RMARQ_ID = :RMARQ_ID
                                            "
                            'cmd.CommandText = "
                            '                    UPDATE RMAREPAIR_QUOTED
                            '                    SET RMARQ_ACCEPT = :RMARQ_ACCEPT,
                            '                        RMARQ_QUOTE = :RMARQ_QUOTE,
                            '                        RMARQ_LUAD = :RMARQ_LUAD,
                            '                        RMARQ_LUADNAME = :RMARQ_LUADNAME,
                            '                        RMARQ_LUSTMP = :RMARQ_LUSTMP
                            '                    WHERE RMARQ_ID = :RMARQ_ID
                            '                "
                            cmd.Parameters.Clear()
                            cmd.Parameters.Add("RMARQ_ACCEPT", OracleType.Number).Value = rq.RMARQ_ACCEPT
                            cmd.Parameters.Add("RMARQ_QUOTE", OracleType.Number).Value =
                                If(String.IsNullOrWhiteSpace(rq.RMARQ_QUOTE), DBNull.Value, Convert.ToDouble(rq.RMARQ_QUOTE))
                            'cmd.Parameters.Add("RMARQ_LUAD", OracleType.VarChar).Value = items.RMAD_LUAD
                            'cmd.Parameters.Add("RMARQ_LUADNAME", OracleType.VarChar).Value = items.RMAD_LUADNAME
                            cmd.Parameters.Add("RMARQ_LUSTMP", OracleType.DateTime).Value = items.RMAD_LUSTMP
                            cmd.Parameters.Add("RMARQ_ID", OracleType.VarChar).Value = rq.RMARQ_ID
                            cmd.ExecuteNonQuery()
                        Next

                        ' ========================
                        ' 更新 RMASale_Quoted
                        ' ========================
                        For Each sq As RMASaleQuotedReq In items.SaleQuoted
                            cmd.CommandText = "
                                                UPDATE RMASALE_QUOTED
                                                SET RMASQ_QUOTE = :RMASQ_QUOTE,
                                                    RMASQ_LUAD = :RMASQ_LUAD,
                                                    RMASQ_LUADNAME = :RMASQ_LUADNAME,
                                                    RMASQ_LUSTMP = :RMASQ_LUSTMP,
                                                    RMASQ_CLIENTAD = :RMASQ_CLIENTAD,
                                                    RMASQ_CLIENTADNAME = :RMASQ_CLIENTADNAME,
                                                    RMASQ_CLIENTDATE = :RMASQ_CLIENTDATE
                                                WHERE RMASQ_RMADID = :RMASQ_RMADID
                                            "
                            cmd.Parameters.Clear()
                            cmd.Parameters.Add("RMASQ_QUOTE", OracleType.Number).Value =
                                If(String.IsNullOrWhiteSpace(sq.RMASQ_QUOTE), DBNull.Value, Convert.ToDouble(sq.RMASQ_QUOTE))
                            cmd.Parameters.Add("RMASQ_LUAD", OracleType.VarChar).Value = items.RMAD_LUAD
                            cmd.Parameters.Add("RMASQ_LUADNAME", OracleType.VarChar).Value = items.RMAD_LUADNAME
                            cmd.Parameters.Add("RMASQ_LUSTMP", OracleType.DateTime).Value = items.RMAD_LUSTMP
                            cmd.Parameters.Add("RMASQ_CLIENTAD", OracleType.VarChar).Value = items.RMAD_LUAD
                            cmd.Parameters.Add("RMASQ_CLIENTADNAME", OracleType.VarChar).Value = items.RMAD_LUADNAME
                            cmd.Parameters.Add("RMASQ_CLIENTDATE", OracleType.DateTime).Value = items.RMAD_LUSTMP
                            cmd.Parameters.Add("RMASQ_RMADID", OracleType.VarChar).Value = items.RMAD_ID
                            cmd.ExecuteNonQuery()
                        Next
                    Next
                End Using
                oConn.Commit()
                result.IsSuccess = True
                result.Message = ""
            Catch ex As Exception
                oConn.Rollback()
                result.IsSuccess = False
                result.Message = ex.Message
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return result
        End Function

        ''' <summary>
        ''' 客戶端 修改 RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修
        ''' </summary>
        ''' <param name="tmpRMAREPAIR_QUOTED_DETAIL_Client"></param>
        ''' <remarks></remarks>
        Public Sub Update_RepairQuotedDetail_Client(ByVal tmpRMAREPAIR_QUOTED_DETAIL_Client As RmaDTO.tmpRMAREPAIR_QUOTED_DETAIL_ClientDataTable)
            Dim i As Integer = 0

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                For i = 0 To tmpRMAREPAIR_QUOTED_DETAIL_Client.Rows.Count - 1
                    Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_DETAIL_ClientRow = tmpRMAREPAIR_QUOTED_DETAIL_Client.Rows(i)
                    'dr.RMARQD_WAIVE        '表示此零件是我方吸收必修，在客人確認畫面不會顯示，維修收費價格會是0；
                    'dr.RMARQD_OPTION       '如果RMARQD_WAIVE=0, RMARQD_OPTION=1 讓客戶決定是否要修
                    'dr.RMARQD_OPTIONCLIENT 'RMARQD_OPTIONCLIENT: 客戶可選擇是否要修-->1.不修, 2.要修

                    If dr.RMARQD_WAIVE = 1 Then
                        oExecute.addParameter("RMARQD_OPTIONCLIENT", 2, OracleType.Int16)
                    Else
                        oExecute.addParameter("RMARQD_OPTIONCLIENT", dr.RMARQD_OPTIONCLIENT, OracleType.Int16)
                    End If

                    oExecute.addParameter("RMARQD_LUAD", dr.RMARQD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_LUADNAME", dr.RMARQD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_LUSTMP", dr.RMARQD_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMARQD_ID", dr.RMARQD_ID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_QUOTED_DETAIL", Execute.eumCommandType.UPDATE)
                Next
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
        ''' 新增報價單 RMAREPAIR_QUOTED
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRMARepairQuoted">傳入RMARepair_QuotedDataTable</param>
        ''' <remarks></remarks>
        Public Sub AddNew_Quoted(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMARepairQuoted As RmaDTO.RMARepair_QuotedDataTable)
            Dim oGuid As Guid = Guid.NewGuid

            Try
                Dim dr As RmaDTO.RMARepair_QuotedRow = dtRMARepairQuoted.Rows(0)


                Dim sGUID As String = oGuid.ToString
                oExecute.addParameter("RMARQ_ID", sGUID, OracleType.VarChar)

                oExecute.addParameter("RMARQ_RMADID", dr.RMARQ_RMADID, OracleType.VarChar)
                oExecute.addParameter("RMARQ_COMPNO", dr.RMARQ_COMPNO, OracleType.VarChar)
                oExecute.addParameter("RMARQ_IMPROPERUSAGE", dr.RMARQ_IMPROPERUSAGE, OracleType.Int16)

                If dr.IsRMARQ_LABORHOURNull = False Then oExecute.addParameter("RMARQ_LABORHOUR", dr.RMARQ_LABORHOUR, OracleType.Double)
                If dr.IsRMARQ_LABORPRICENull = False Then oExecute.addParameter("RMARQ_LABORPRICE", dr.RMARQ_LABORPRICE, OracleType.Double)
                If dr.IsRMARQ_MATERIALCOSTNull = False Then oExecute.addParameter("RMARQ_MATERIALCOST", dr.RMARQ_MATERIALCOST, OracleType.Double)

                If dr.IsRMARQ_QUOTENull = False Then
                    oExecute.addParameter("RMARQ_QUOTE", dr.RMARQ_QUOTE, OracleType.Double)
                End If

                If dr.IsRMARQ_CURRENCYCODENull = False Then oExecute.addParameter("RMARQ_CURRENCYCODE", dr.RMARQ_CURRENCYCODE, OracleType.VarChar)
                If dr.IsRMARQ_CURRENCYRATENull = False Then oExecute.addParameter("RMARQ_CURRENCYRATE", dr.RMARQ_CURRENCYRATE, OracleType.Double)

                If dr.IsRMARQ_ASSIGLABORCOSTNull = False Then oExecute.addParameter("RMARQ_ASSIGLABORCOST", dr.RMARQ_ASSIGLABORCOST, OracleType.Double)
                If dr.IsRMARQ_ASSIGMATERIALCOSTNull = False Then oExecute.addParameter("RMARQ_ASSIGMATERIALCOST", dr.RMARQ_ASSIGMATERIALCOST, OracleType.Double)

                If dr.IsRMARQ_ASSIGEQUOTENull = False Then oExecute.addParameter("RMARQ_ASSIGEQUOTE", dr.RMARQ_ASSIGEQUOTE, OracleType.Double)
                If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then oExecute.addParameter("RMARQ_ASSIGECURRENCYCODE", dr.RMARQ_ASSIGECURRENCYCODE, OracleType.VarChar)
                If dr.IsRMARQ_ASSIGECURRENCYRATENull = False Then oExecute.addParameter("RMARQ_ASSIGECURRENCYRATE", dr.RMARQ_ASSIGECURRENCYRATE, OracleType.Double)

                If dr.IsRMARQ_ACCEPTNull = False Then
                    oExecute.addParameter("RMARQ_ACCEPT", dr.RMARQ_ACCEPT, OracleType.Int16)
                End If

                oExecute.addParameter("RMARQ_AD", dr.RMARQ_AD, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_ADNAME", dr.RMARQ_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_CSTMP", dr.RMARQ_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMARQ_LUAD", dr.RMARQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_LUADNAME", dr.RMARQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_LUSTMP", dr.RMARQ_LUSTMP, OracleType.DateTime)
                If dr.IsRMARQ_EXPIRED_DATENull = False Then oExecute.addParameter("RMARQ_EXPIRED_DATE", dr.RMARQ_EXPIRED_DATE, OracleType.DateTime) '新增訂單逾期日 by buck modify 20251118

                oExecute.Command("RMAREPAIR_QUOTED", Execute.eumCommandType.AddNew)

            Catch ex As Exception
                Throw ex

            Finally
            End Try

        End Sub

        ''' <summary>
        ''' 修改報價單 RMAREPAIR_QUOTED
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRMARepairQuoted">傳入RMARepair_QuotedDataTable</param>
        ''' <remarks></remarks>
        Public Sub Edit_Quoted(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMARepairQuoted As RmaDTO.RMARepair_QuotedDataTable)
            Dim oGuid As Guid = Guid.NewGuid

            Try
                Dim dr As RmaDTO.RMARepair_QuotedRow = dtRMARepairQuoted.Rows(0)

                oExecute.addParameter("RMARQ_RMADID", dr.RMARQ_RMADID, OracleType.VarChar)
                oExecute.addParameter("RMARQ_COMPNO", dr.RMARQ_COMPNO, OracleType.VarChar)
                oExecute.addParameter("RMARQ_IMPROPERUSAGE", dr.RMARQ_IMPROPERUSAGE, OracleType.Int16)

                If dr.IsRMARQ_LABORHOURNull = False Then
                    oExecute.addParameter("RMARQ_LABORHOUR", dr.RMARQ_LABORHOUR, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_LABORHOUR", System.Convert.DBNull, OracleType.Double)
                End If
                If dr.IsRMARQ_LABORPRICENull = False Then
                    oExecute.addParameter("RMARQ_LABORPRICE", dr.RMARQ_LABORPRICE, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_LABORPRICE", System.Convert.DBNull, OracleType.Double)
                End If
                If dr.IsRMARQ_MATERIALCOSTNull = False Then
                    oExecute.addParameter("RMARQ_MATERIALCOST", dr.RMARQ_MATERIALCOST, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_MATERIALCOST", System.Convert.DBNull, OracleType.Double)
                End If

                If dr.IsRMARQ_QUOTENull = False Then
                    oExecute.addParameter("RMARQ_QUOTE", dr.RMARQ_QUOTE, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_QUOTE", System.Convert.DBNull, OracleType.Double)
                End If
                If dr.IsRMARQ_CURRENCYCODENull = False Then
                    oExecute.addParameter("RMARQ_CURRENCYCODE", dr.RMARQ_CURRENCYCODE, OracleType.VarChar)
                Else
                    oExecute.addParameter("RMARQ_CURRENCYCODE", System.Convert.DBNull, OracleType.VarChar)
                End If
                If dr.IsRMARQ_CURRENCYRATENull = False Then
                    oExecute.addParameter("RMARQ_CURRENCYRATE", dr.RMARQ_CURRENCYRATE, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_CURRENCYRATE", System.Convert.DBNull, OracleType.Double)
                End If

                If dr.IsRMARQ_ASSIGLABORCOSTNull = False Then
                    oExecute.addParameter("RMARQ_ASSIGLABORCOST", dr.RMARQ_ASSIGLABORCOST, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_ASSIGLABORCOST", System.Convert.DBNull, OracleType.Double)
                End If
                If dr.IsRMARQ_ASSIGMATERIALCOSTNull = False Then
                    oExecute.addParameter("RMARQ_ASSIGMATERIALCOST", dr.RMARQ_ASSIGMATERIALCOST, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_ASSIGMATERIALCOST", System.Convert.DBNull, OracleType.Double)
                End If
                If dr.IsRMARQ_ASSIGEQUOTENull = False Then
                    oExecute.addParameter("RMARQ_ASSIGEQUOTE", dr.RMARQ_ASSIGEQUOTE, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_ASSIGEQUOTE", System.Convert.DBNull, OracleType.Double)
                End If

                If dr.IsRMARQ_ASSIGECURRENCYCODENull = False Then
                    oExecute.addParameter("RMARQ_ASSIGECURRENCYCODE", dr.RMARQ_ASSIGECURRENCYCODE, OracleType.VarChar)
                Else
                    oExecute.addParameter("RMARQ_ASSIGECURRENCYCODE", System.Convert.DBNull, OracleType.VarChar)
                End If
                If dr.IsRMARQ_ASSIGECURRENCYRATENull = False Then
                    oExecute.addParameter("RMARQ_ASSIGECURRENCYRATE", dr.RMARQ_ASSIGECURRENCYRATE, OracleType.Double)
                Else
                    oExecute.addParameter("RMARQ_ASSIGECURRENCYRATE", System.Convert.DBNull, OracleType.Double)
                End If

                If dr.IsRMARQ_ACCEPTNull = False Then
                    oExecute.addParameter("RMARQ_ACCEPT", dr.RMARQ_ACCEPT, OracleType.Int16)
                End If

                oExecute.addParameter("RMARQ_LUAD", dr.RMARQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_LUADNAME", dr.RMARQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMARQ_LUSTMP", dr.RMARQ_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("RMARQ_RMADID", dr.RMARQ_RMADID, OracleType.VarChar)
                oExecute.Command("RMAREPAIR_QUOTED", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally
            End Try

        End Sub

        ''' <summary>
        ''' Delete RMAREPAIR_QUOTED_DETAIL
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="RMARQD_RMADID"></param>
        ''' <remarks></remarks>
        Private Sub Delete_RepairQuotedDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMARQD_RMADID As String)
            Dim i As Integer = 0

            Try
                oExecute.addParameter("RMARQD_MARK", 1, OracleType.Int16)

                oExecute.addWHERE("RMARQD_RMADID", RMARQD_RMADID, OracleType.VarChar)
                oExecute.Command("RMAREPAIR_QUOTED_DETAIL", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally

            End Try



        End Sub

        ''' <summary>
        ''' 新增 RMAREPAIR_QUOTED_DETAIL 
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dvRepairQuotedDetail">DataView</param>
        ''' <remarks></remarks>
        Private Sub AddNew_RepairQuotedDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dvRepairQuotedDetail As DataView)
            Dim i As Integer = 0

            Try

                For i = 0 To dvRepairQuotedDetail.Count - 1
                    Dim oGuid As Guid = Guid.NewGuid
                    Dim sGUID As String = oGuid.ToString
                    Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dvRepairQuotedDetail(i).Row

                    oExecute.addParameter("RMARQD_ID", sGUID, OracleType.VarChar)
                    'oExecute.addParameter("RMARQD_ID", dr.RMARQD_ID, OracleType.VarChar)
                    oExecute.addParameter("RMARQD_RMADID", dr.RMARQD_RMADID, OracleType.VarChar)

                    oExecute.addParameter("RMARQD_NPARTNO", dr.RMARQD_NPARTNO, OracleType.VarChar)
                    oExecute.addParameter("RMARQD_NSERIALNO", dr.RMARQD_NSERIALNO, OracleType.VarChar)
                    If dr.IsRMARQD_NWARRANTYNull = False Then oExecute.addParameter("RMARQD_NWARRANTY", dr.RMARQD_NWARRANTY, OracleType.DateTime)

                    If dr.IsRMARQD_OPARTNONull = False Then oExecute.addParameter("RMARQD_OPARTNO", dr.RMARQD_OPARTNO, OracleType.VarChar)
                    If dr.IsRMARQD_OSERIALNONull = False Then oExecute.addParameter("RMARQD_OSERIALNO", dr.RMARQD_OSERIALNO, OracleType.VarChar)
                    If dr.IsRMARQD_OWARRANTYNull = False Then oExecute.addParameter("RMARQD_OWARRANTY", dr.RMARQD_OWARRANTY, OracleType.DateTime)

                    oExecute.addParameter("RMARQD_IMPROPERUSAGE", dr.RMARQD_IMPROPERUSAGE, OracleType.Int16)
                    If dr.IsRMARQD_DESCNull = False Then oExecute.addParameter("RMARQD_DESC", dr.RMARQD_DESC, OracleType.NVarChar)
                    If dr.IsRMARQD_LOCATIONNull = False Then oExecute.addParameter("RMARQD_LOCATION", dr.RMARQD_LOCATION, OracleType.VarChar)
                    If dr.IsRMARQD_DEFECTIVENull = False Then oExecute.addParameter("RMARQD_DEFECTIVE", dr.RMARQD_DEFECTIVE, OracleType.NVarChar)

                    oExecute.addParameter("RMARQD_QTY", dr.RMARQD_QTY, OracleType.Int16)
                    oExecute.addParameter("RMARQD_MATERIALCOST", dr.RMARQD_MATERIALCOST, OracleType.Double)
                    oExecute.addParameter("RMARQD_PRICE", dr.RMARQD_PRICE, OracleType.Double)

                    oExecute.addParameter("RMARQD_AD", dr.RMARQD_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_ADNAME", dr.RMARQD_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARQD_LUAD", dr.RMARQD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_LUADNAME", dr.RMARQD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMARQD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARQD_MARK", dr.RMARQD_MARK, OracleType.Double)

                    oExecute.addParameter("RMARQD_CURRENCYCODE", dr.RMARQD_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARQD_CURRENCYRATE", dr.RMARQD_CURRENCYRATE, OracleType.Double)

                    oExecute.addParameter("RMARQD_ASSIGECURRENCYCODE", dr.RMARQD_ASSIGECURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARQD_ASSIGECURRENCYRATE", dr.RMARQD_ASSIGECURRENCYRATE, OracleType.Double)
                    oExecute.addParameter("RMARQD_ASSIGEPRICE", dr.RMARQD_ASSIGEPRICE, OracleType.Double)

                    If dr.IsRMARQD_ACCNull = False Then oExecute.addParameter("RMARQD_ACC", dr.RMARQD_ACC, OracleType.Int16)
                    If dr.IsRMARQD_WAIVENull = False Then oExecute.addParameter("RMARQD_WAIVE", dr.RMARQD_WAIVE, OracleType.Int16)
                    If dr.IsRMARQD_OPTIONNull = False Then oExecute.addParameter("RMARQD_OPTION", dr.RMARQD_OPTION, OracleType.Int16)
                    If dr.IsRMARQD_OPTIONCLIENTNull = False Then oExecute.addParameter("RMARQD_OPTIONCLIENT", dr.RMARQD_OPTIONCLIENT, OracleType.Int16)

                    oExecute.Command("RMAREPAIR_QUOTED_DETAIL", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try



        End Sub

        ''' <summary>
        ''' 修改 RMAREPAIR_QUOTED_DETAIL 
        ''' </summary>
        ''' <param name="dtRepairQuoted_Detail">DataView</param>
        ''' <remarks></remarks>
        Public Sub Edit_RepairQuotedDetail(ByVal RMARQ_ID As String, ByVal RMARQ_MATERIALCOST As Double, ByVal RMARQ_QUOTE As Double,
            ByVal RMARQ_ASSIGLABORCOST As Double, ByVal RMARQ_ASSIGMATERIALCOST As Double, ByVal RMARQ_ASSIGEQUOTE As Double,
            ByVal dtRepairQuoted_Detail As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable)

            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                'update RMAREPAIR_QUOTED_DETAIL
                For i = 0 To dtRepairQuoted_Detail.Count - 1
                    Dim dr As RmaDTO.RMAREPAIR_QUOTED_DETAILRow = dtRepairQuoted_Detail.Rows(i)

                    If dr.IsRMARQD_DESCNull = False Then oExecute.addParameter("RMARQD_DESC", dr.RMARQD_DESC, OracleType.NVarChar)

                    oExecute.addParameter("RMARQD_MATERIALCOST", dr.RMARQD_MATERIALCOST, OracleType.Double)     '零件費用
                    oExecute.addParameter("RMARQD_PRICE", dr.RMARQD_PRICE, OracleType.Double)                   'Price
                    oExecute.addParameter("RMARQD_ASSIGEPRICE", dr.RMARQD_ASSIGEPRICE, OracleType.Double)       '轉換成 指派的維修中心 --> Price

                    If dr.IsRMARQD_OPTIONCLIENTNull = False Then oExecute.addParameter("RMARQD_OPTIONCLIENT", dr.RMARQD_OPTIONCLIENT, OracleType.Int16)

                    oExecute.addWHERE("RMARQD_ID", dr.RMARQD_ID, OracleType.VarChar)
                    oExecute.Command("RMAREPAIR_QUOTED_DETAIL", Execute.eumCommandType.UPDATE)
                Next

                'update RMAREPAIR_QUOTED
                oExecute.addParameter("RMARQ_MATERIALCOST", RMARQ_MATERIALCOST, OracleType.Double)
                oExecute.addParameter("RMARQ_QUOTE", RMARQ_QUOTE, OracleType.Double)

                oExecute.addParameter("RMARQ_ASSIGLABORCOST", RMARQ_ASSIGLABORCOST, OracleType.Double)
                oExecute.addParameter("RMARQ_ASSIGMATERIALCOST", RMARQ_ASSIGMATERIALCOST, OracleType.Double)
                oExecute.addParameter("RMARQ_ASSIGEQUOTE", RMARQ_ASSIGEQUOTE, OracleType.Double)

                oExecute.addWHERE("RMARQ_ID", RMARQ_ID, OracleType.VarChar)
                oExecute.Command("RMAREPAIR_QUOTED", Execute.eumCommandType.UPDATE)
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
        ''' 檢核 底於 維修中心訂定的維修金額, 直接送修
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtRMARepairQuoted">傳入RMARepair_QuotedDataTable</param>
        ''' <remarks></remarks>
        Private Sub LessApprovAmountToRepair(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtRMARepairQuoted As RmaDTO.RMARepair_QuotedDataTable)
            Dim oCompany As New ctlCompany

            Dim dr As RmaDTO.RMARepair_QuotedRow = dtRMARepairQuoted.Rows(0)
            Dim ApprovAmount As String = oCompany.getApprovAmount(dr.RMARQ_COMPNO)

            If IsNumeric(ApprovAmount) = True And dr.IsRMARQ_QUOTENull = False Then
                If Convert.ToDouble(ApprovAmount) >= dr.RMARQ_QUOTE Then
                    oExecute.addParameter("RMAD_STATUS", 60, OracleType.Int16)
                    oExecute.addWHERE("RMAD_ID", dr.RMARQ_RMADID, OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                End If
            End If

        End Sub

        ''' <summary>
        ''' 檢核 底於 維修中心訂定的維修金額, 直接送修
        ''' MODI BY Angel ON 20160128 公HQuote用
        ''' </summary>        
        ''' <param name="dtRMARepairQuoted">傳入RMARepair_QuotedDataTable</param>
        ''' <remarks></remarks>
        Public Sub LessApprovAmountToRepairHQ(ByVal dtRMARepairQuoted As RmaDTO.RMARepair_QuotedDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                LessApprovAmountToRepair(oExecute, dtRMARepairQuoted)
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
        ''' 檢核是否已全部報完價
        ''' </summary>
        ''' <param name="RMA_No">傳入RMA No</param>
        ''' <returns>已全部報完價回傳True, 反之則False</returns>
        ''' <remarks></remarks>
        Function CHKAllItem_isRepairQuoted(ByVal RMA_No As String) As Boolean
            Dim retval As Boolean = False
            Dim sCondition As String = ""
            Dim sSQL As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'RMAD_STATUS --> 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled

                oQuery.addWHERE("RMADetail.RMAD_RMANO", ": RMAD_RMANO", RMA_No.Trim(), OracleType.VarChar)

                sSQL = "SELECT RMADetail.RMAD_RMANO, NVL(vwRequest.Request_QTY,0) Request_QTY , NVL(vwRepairQuoted.RepairQuoted_QTY,0) RepairQuoted_QTY" &
                " FROM  RMADetail INNER JOIN" &
                "   (" &
                "       SELECT RMAD_RMANO, Count(*) as Request_QTY FROM RMADetail" &
                "       WHERE RMAD_MARK=0 and RMAD_RECEVSTATUS<>2 and RMAD_STATUS<>91 and RMAD_STATUS<>0" &
                "       GROUP BY RMAD_RMANO" &
                "   ) vwRequest ON vwRequest.RMAD_RMANO = RMADetail.RMAD_RMANO" &
                "   LEFT OUTER JOIN" &
                "   (" &
                "       SELECT RMAD_RMANO, Count(*) as RepairQuoted_QTY FROM RMADetail" &
                "       WHERE RMAD_MARK=0 and RMAD_RECEVSTATUS=1 and RMAD_STATUS>20 and RMAD_STATUS<>91" &
                "       GROUP BY RMAD_RMANO" &
                "   ) vwRepairQuoted  ON vwRepairQuoted.RMAD_RMANO = RMADetail.RMAD_RMANO" &
                " WHERE RMADetail.RMAD_RMANO=:RMAD_RMANO" &
                " GROUP BY RMADetail.RMAD_RMANO, Request_QTY, RepairQuoted_QTY"

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    Dim Request_QTY As Integer = Convert.ToInt16(dt.Rows(0)("Request_QTY").ToString.Trim())
                    Dim RepairQuoted_QTY As Integer = Convert.ToInt16(dt.Rows(0)("RepairQuoted_QTY").ToString.Trim())
                    If Request_QTY = RepairQuoted_QTY Then
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
        ''' 檢核 RMA單裡的 維修報價 零件檔 規格及單價 是否有無值狀況
        ''' </summary>
        ''' <param name="RMA_No">傳入RMA No</param>
        ''' <returns>規格及單價 有無值的狀況回傳True, 反之則False</returns>
        ''' <remarks></remarks>
        Function FlowCase01_CHKQuotedItem_Abnormal(ByVal RMA_No As String) As Boolean

            Dim retval As Boolean = False
            Dim sCondition As String = ""
            Dim sSQL As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'RMAD_STATUS --> 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("rmadetail.RMAD_RMANO", ":RMAD_RMANO", RMA_No.Trim(), OracleType.VarChar)

                sSQL = sSQL & " SELECT RMAD_SERIALNO, RMARQ_ID, nvl(RMARQD_DESC,' ') as RMARQD_DESC, nvl(RMARQD_MATERIALCOST,0) as RMARQD_MATERIALCOST, RMARQD_NPARTNO, RMAD_STATUS"
                sSQL = sSQL & " FROM rmadetail "

                sSQL = sSQL & "     left outer join RMAREPAIR_QUOTED on RMAD_ID = RMARQ_RMADID "
                sSQL = sSQL & "     left outer join RMAREPAIR_QUOTED_DETAIL on RMARQD_RMADID = RMAD_ID and RMARQD_MARK=0"

                'sSQL = sSQL & " INNER JOIN "
                'sSQL = sSQL & " ("
                'sSQL = sSQL & "     SELECT * FROM RMAREPAIR_QUOTED"
                'sSQL = sSQL & "     INNER JOIN RMAREPAIR_QUOTED_DETAIL "
                'sSQL = sSQL & "     ON RMAREPAIR_QUOTED.RMARQ_RMADID = RMAREPAIR_QUOTED_DETAIL.RMARQD_RMADID AND RMAREPAIR_QUOTED_DETAIL.RMARQD_MARK=0"
                'sSQL = sSQL & " ) vwRMAREPAIR_QUOTED"
                'sSQL = sSQL & " ON RMAD_ID = RMARQ_RMADID"

                sSQL = sSQL & " WHERE RMAD_MARK=0 AND (RMAD_STATUS=10 OR RMAD_STATUS=20 OR RMAD_STATUS=30)"
                sSQL = sSQL & "     AND (RMAREPAIR_QUOTED.RMARQ_ID is null OR RMARQD_RMADID is not null)"               '此條件是解決 rma quote無更換零件,只有service charge
                sSQL = sSQL & "     AND (RMARQ_ID IS NULL OR nvl(RMARQD_DESC,' ')=' ' OR nvl(RMARQD_MATERIALCOST,0)=0)"
                sSQL = sSQL & "     AND rmadetail.RMAD_RMANO=:RMAD_RMANO"


                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = True   '有無值的狀況
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
        ''' 取得 RMA單裡的 維修報價 零件檔 規格及單價 是否有異常(無值)狀況
        ''' </summary>
        ''' <param name="RMA_No"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function QryQuotedItem_Abnormal_01(ByVal RMA_No As String) As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim retval As Boolean = False
            Dim sCondition As String = ""
            Dim sSQL As String = ""

            Dim dtRepairQuoted_Detail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                'RMAD_STATUS --> 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("rmadetail.RMAD_RMANO", ":RMAD_RMANO", RMA_No.Trim(), OracleType.VarChar)

                sSQL = sSQL & " SELECT RMAREPAIR_QUOTED_DETAIL.*"
                sSQL = sSQL & " FROM rmadetail "

                sSQL = sSQL & "     left outer join RMAREPAIR_QUOTED on RMAD_ID = RMARQ_RMADID "
                sSQL = sSQL & "     left outer join RMAREPAIR_QUOTED_DETAIL on RMARQD_RMADID = RMAD_ID and RMARQD_MARK=0"

                sSQL = sSQL & " WHERE RMAD_MARK=0 AND (RMAD_STATUS=10 OR RMAD_STATUS=20 OR RMAD_STATUS=30)"
                sSQL = sSQL & "     AND (RMARQ_ID IS NULL OR nvl(RMARQD_DESC,' ')=' ' OR nvl(RMARQD_MATERIALCOST,0)=0)"
                sSQL = sSQL & "     AND rmadetail.RMAD_RMANO=:RMAD_RMANO"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuoted_Detail)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairQuoted_Detail

        End Function

        ''' <summary>
        ''' 取得 RMA單裡的 維修報價資料
        ''' </summary>
        ''' <param name="RMA_No"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function qryRepair_Quoted(ByVal RMA_No As String) As RmaDTO.RMARepair_QuotedDataTable
            Dim sSQL As String = ""

            Dim dtRepairQuoted As New RmaDTO.RMARepair_QuotedDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'RMAD_STATUS --> 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("rmadetail.RMAD_RMANO", ":RMAD_RMANO", RMA_No.Trim(), OracleType.VarChar)

                sSQL = sSQL & " SELECT RMAREPAIR_QUOTED.*"
                sSQL = sSQL & " FROM rmadetail "
                sSQL = sSQL & "     INNER JOIN RMAREPAIR_QUOTED on RMAD_ID = RMARQ_RMADID "
                sSQL = sSQL & " WHERE rmadetail.RMAD_RMANO=:RMAD_RMANO AND RMAD_STATUS<50"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuoted)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairQuoted
        End Function

        ''' <summary>
        ''' 取得 RMA單裡的 維修報價零件檔 資料
        ''' </summary>
        ''' <param name="RMARQD_RMADID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function qryRepair_Quoted_Detail(ByVal RMARQD_RMADID As String) As RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim sSQL As String = ""

            Dim dtRepairQuoted_Detail As New RmaDTO.RMAREPAIR_QUOTED_DETAILDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'RMAD_STATUS --> 0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMARQD_RMADID", ":RMARQD_RMADID", RMARQD_RMADID.Trim(), OracleType.VarChar)

                sSQL = sSQL & " SELECT RMAREPAIR_QUOTED_DETAIL.*"
                sSQL = sSQL & " FROM RMAREPAIR_QUOTED_DETAIL "
                sSQL = sSQL & " WHERE RMARQD_RMADID=:RMARQD_RMADID AND RMARQD_MARK=0"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairQuoted_Detail)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRepairQuoted_Detail
        End Function
    End Class
#End Region

#Region "Class:RMAStatus"
    Public Class RMAStatus
        ''' <summary>
        ''' 取得品項各階段的 資料
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <returns>回傳vwStatusPoint_DetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryPointByDetail(ByVal RMAD_ID As String) As RmaDTO.vwStatusPoint_DetailDataTable
            Dim sCondition As String = ""
            Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"

                Dim sSQL As String = "SELECT * FROM vwStatusPoint_Detail WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtStatusPoint)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtStatusPoint
        End Function

        ''' <summary>
        ''' 變更 RMADetail 單的狀態
        ''' </summary>
        ''' <param name="dtStatus">傳入RMADetailStatusDataTable</param>
        ''' <remarks></remarks>
        Public Sub ChangeStatus(ByVal dtStatus As RmaDTO.RMADetailStatusDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                For i = 0 To dtStatus.Rows.Count - 1
                    Dim dr As RmaDTO.RMADetailStatusRow = dtStatus.Rows(i)

                    If dr.IsRMAD_IDNull = False Then
                        oExecute.addParameter("RMAD_STATUS", Convert.ToInt16(dr.RMAD_STATUS), OracleType.Int16)

                        '目前 91.Cancel 會紀錄是誰處理的
                        If dr.IsRMAD_ADNull = False Then oExecute.addParameter("RMAD_LUAD", dr.RMAD_AD.Trim(), OracleType.NVarChar)
                        If dr.IsRMAD_ADNAMENull = False Then oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_ADNAME.Trim(), OracleType.NVarChar)
                        If dr.IsRMAD_ADNull = False And dr.IsRMAD_ADNAMENull = False Then
                            oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                        End If

                        oExecute.addWHERE("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)
                        oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                    End If
                Next

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
        ''' 變更 RMADetail 單的狀態
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtStatus">傳入RMADetailStatusDataTable</param>
        ''' <remarks></remarks>
        Public Sub ChangeStatus(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtStatus As RmaDTO.RMADetailStatusDataTable)
            Dim i As Integer = 0

            Try
                For i = 0 To dtStatus.Rows.Count - 1
                    Dim dr As RmaDTO.RMADetailStatusRow = dtStatus.Rows(i)

                    If dr.IsRMAD_IDNull = False Then
                        oExecute.addParameter("RMAD_STATUS", Convert.ToInt16(dr.RMAD_STATUS), OracleType.Int16)

                        '目前 91.Cancel 會紀錄是誰處理的
                        If dr.IsRMAD_ADNull = False Then oExecute.addParameter("RMAD_LUAD", dr.RMAD_AD.Trim(), OracleType.NVarChar)
                        If dr.IsRMAD_ADNAMENull = False Then oExecute.addParameter("RMAD_LUADNAME", dr.RMAD_ADNAME.Trim(), OracleType.NVarChar)
                        If dr.IsRMAD_ADNull = False And dr.IsRMAD_ADNAMENull = False Then
                            oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                        End If

                        oExecute.addWHERE("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)
                        oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                    End If
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 取得目前 RMADetail 的狀態
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <remarks></remarks>
        Public Function getItemStatus(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMAD_ID As String) As Integer
            Dim retval As Integer = 0
            Dim sCondition As String = ""
            Dim dtStatusPoint As New RmaDTO.vwStatusPoint_DetailDataTable
            Dim dt As New DataTable

            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)

            Try

                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"

                Dim sSQL As String = "SELECT * FROM RMADETAIL WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    retval = Convert.ToInt16(dt.Rows(0)("RMAD_STATUS"))
                End If

            Catch ex As Exception
                Throw ex
            Finally

            End Try

            Return retval
        End Function

        '''' <summary>
        '''' 檢核 業務帶客戶確認 報價
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function chkSalesConfirm() As Boolean

        'End Function
    End Class
#End Region

#Region "Class:Sale:業務"
    Public Class Sale

        ''' <summary>
        ''' 取得業務報價資料
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <returns>RMASale_QuotedDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryBySaleQuoted(ByVal RMAD_ID As String) As RmaDTO.RMASALE_QUOTEDDataTable
            Dim sCondition As String = ""
            Dim dtRepairDetail As New RmaDTO.RMASALE_QUOTEDDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMASQ_RMADID", ":RMASQ_RMADID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " RMASQ_RMADID=:RMASQ_RMADID"

                Dim sSQL As String = "SELECT * FROM RMASALE_QUOTED WHERE " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairDetail)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairDetail
        End Function

        ''' <summary>
        ''' 取得 待工作項目
        ''' </summary>
        ''' <param name="COMPNO">維修中心代碼</param>
        ''' <param name="SALESID">業務代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWork(ByVal COMPNo As String, ByVal SALESID As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.vwSale_WorkListDataTable

            Dim sCondition As String = ""
            Dim dtSalseList As New RmaDTO.vwSale_WorkListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY


                '業務對應到的客戶
                If SALESID <> "admin" Then
                    COMPNo = "%" & COMPNo & "%"
                    oQuery.addWHERE("CU_SALESID", ":CU_SALESID", SALESID, OracleType.VarChar)
                    oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", SALESID, OracleType.VarChar)
                    sCondition = sCondition & " AND (CU_SALESID =:CU_SALESID OR CU_ASSISTANTID =:CU_ASSISTANTID)"
                End If


                If RMANo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMA_NO=:RMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_MODELNO=:RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_SERIALNO=:RMAD_SERIALNO"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND CU_NAME like :CU_NAME"
                End If

                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "30", OracleType.Int16)
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "50", OracleType.Int16)
                sCondition = sCondition & " AND (RMAD_STATUS>=:RMAD_STATUS1 AND RMAD_STATUS<:RMAD_STATUS2)"


                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sSQL As String = "SELECT * FROM VWSALE_WORKLIST WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtSalseList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtSalseList
        End Function

        ''' <summary>
        ''' 取得業務自己的RMA Detail 清單, 含已報價資料
        ''' </summary>
        ''' <param name="COMPNO">維修中心代碼</param>
        ''' <param name="SALESID">業務代碼</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByDetail(ByVal COMPNo As String, ByVal SALESID As String, ByVal RMANo As String, Optional ByVal OrderBY As String = "") As RmaDTO.vwSale_WorkListDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtSalseList As New RmaDTO.vwSale_WorkListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY


                '業務對應到的客戶
                If SALESID <> "admin" Then
                    COMPNo = "%" & COMPNo & "%"
                    oQuery.addWHERE("CU_SALESID", ":CU_SALESID", SALESID, OracleType.VarChar)
                    oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", SALESID, OracleType.VarChar)
                    sCondition = sCondition & " AND (CU_SALESID =:CU_SALESID OR CU_ASSISTANTID =:CU_ASSISTANTID)"
                End If


                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RMA_NO"


                sSQL = "SELECT " &
                        " RMA.RMA_ID, RMA.RMA_NO, RMA.RMA_CUNO, RMA.RMA_ACCOUNTID, RMA.RMA_APPLICANT," &
                        " RMA.RMA_TEL, RMA.RMA_ADDRESS, RMA.RMA_COMPNO, RMA.RMA_MAIL, RMA.RMA_STATUS," &
                        " RMA.RMA_AD, RMA.RMA_ADNAME, RMA.RMA_CSTMP," &
                        " RMA.RMA_LUAD, RMA.RMA_LUADNAME, RMA.RMA_LUSTMP, RMA.RMA_MARK, " &
                        "" &
                        " RMADETAIL.RMAD_ID, RMADETAIL.RMAD_SEQ, RMADETAIL.RMAD_RMANO," &
                        " RMADETAIL.RMAD_MODELNO, RMADETAIL.RMAD_SERIALNO, RMADETAIL.RMAD_CUSNAME," &
                        " RMADETAIL.RMAD_WARRANTY, RMADETAIL.RMAD_FARFARCNO, RMADETAIL.RMAD_FARNO," &
                        " RMADETAIL.RMAD_UPLOADFILE, RMADETAIL.RMAD_PRODUCTDESC, RMADETAIL.RMAD_PROBLEMDESC," &
                        " RMADETAIL.RMAD_STATUS, RMADETAIL.RMAD_ISFILL," &
                        " RMADETAIL.RMAD_RECVAD, RMADETAIL.RMAD_RECVADNAME, RMADETAIL.RMAD_RECVDATE, RMADETAIL.RMAD_RECEVSTATUS," &
                        " RMADETAIL.RMAD_AD, RMADETAIL.RMAD_ADNAME, RMADETAIL.RMAD_CSTMP," &
                        " RMADETAIL.RMAD_LUAD, RMADETAIL.RMAD_LUADNAME, RMADETAIL.RMAD_LUSTMP, RMADETAIL.RMAD_MARK, " &
                        "" &
                        " CU_NO, CU_NAME, CU_SALESID, CU_ASSISTANTID, " &
                        " RMAR_COMPNO, COMP_NAME, RMAR_REPAIR_ISFILL, " &
                        " RMARQ_ASSIGECURRENCYCODE , RMARQ_ASSIGEQUOTE, RMARQ_CURRENCYCODE, RMARQ_QUOTE, " &
                        " RMASQ_LABORCOST, RMASQ_MATERIALCOST, RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE,RMASQ_ID" &
                        " FROM RMA " &
                        " INNER JOIN RMADETAIL" &
                        "   ON RMA.Rma_no  = RMADETAIL.rmad_rmano AND RMAD_RECEVSTATUS=1 AND RMADETAIL.RMAD_MARK=0 AND (RMAD_STATUS<50 OR RMAD_STATUS=91)" &
                        " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" &
                        " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID" &
                        " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID" &
                        " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID" &
                        " LEFT OUTER JOIN COMPANY ON RMAREPAIR.RMAR_COMPNO = COMPANY.COMP_NO" &
                        " WHERE 1=1" & sCondition & OrderBY


                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtSalseList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtSalseList
        End Function

        ''' <summary>
        ''' 檢核是否是 業務帶客戶確認
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isSalesByClientConfirm(ByVal RMAD_ID As String) As Boolean
            Dim retval As Boolean = False
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMASQ_RMADID", ":RMASQ_RMADID", RMAD_ID, OracleType.VarChar)
                Dim sCondition As String = " AND RMASQ_RMADID =:RMASQ_RMADID"

                'RMASQ_CLIENTCONFIRM: 1.客戶確認, 2.業務帶客戶確認
                Dim sSQL As String = "SELECT RMASQ_CLIENTCONFIRM FROM RMASALE_QUOTED WHERE RMASQ_CLIENTCONFIRM=2 " & sCondition
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
        ''' 業務報價-新增
        ''' </summary>
        ''' <param name="dtSale">傳入RMASALE_QUOTEDDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtSale As RmaDTO.RMASALE_QUOTEDDataTable)
            Dim blnFlag As Boolean = False
            Dim dr As RmaDTO.RMASALE_QUOTEDRow = dtSale.Rows(0)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("RMASQ_ID", dr.RMASQ_ID, OracleType.VarChar)
                oExecute.addParameter("RMASQ_RMADID", dr.RMASQ_RMADID, OracleType.VarChar)
                oExecute.addParameter("RMASQ_LABORCOST", dr.RMASQ_LABORCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_MATERIALCOST", dr.RMASQ_MATERIALCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_QUOTE", dr.RMASQ_QUOTE, OracleType.Number)
                oExecute.addParameter("RMASQ_CURRENCYCODE", dr.RMASQ_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("RMASQ_CURRENCYRATE", dr.RMASQ_CURRENCYRATE, OracleType.Number)
                oExecute.addParameter("RMASQ_AD", dr.RMASQ_AD, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_ADNAME", dr.RMASQ_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_CSTMP", dr.RMASQ_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMASQ_LUAD", dr.RMASQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUADNAME", dr.RMASQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUSTMP", dr.RMASQ_LUSTMP, OracleType.DateTime)

                If dr.IsRMASQ_DESCNull = False Then oExecute.addParameter("RMASQ_DESC", dr.RMASQ_DESC, OracleType.NVarChar)
                If dr.IsRMASQ_SALEADNull = False Then oExecute.addParameter("RMASQ_SALEAD", dr.RMASQ_SALEAD, OracleType.NVarChar)
                If dr.IsRMASQ_SALEADNAMENull = False Then oExecute.addParameter("RMASQ_SALEADNAME", dr.RMASQ_SALEADNAME, OracleType.NVarChar)
                If dr.IsRMASQ_SALEDATENull = False Then oExecute.addParameter("RMASQ_SALEDATE", dr.RMASQ_SALEDATE, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTADNull = False Then oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTADNAMENull = False Then oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTDATENull = False Then oExecute.addParameter("RMASQ_CLIENTDATE", dr.RMASQ_CLIENTDATE, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTCONFIRMNull = False Then oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.AddNew)


                '=============================================================================================================
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '變更RMADetail的狀態(RMAD_STATUS==>35.Sale Quoting)
                '=============================================================================================================
                Dim oRMAStatus As New ctlRMA.RMAStatus
                Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow
                drStatus.RMAD_ID = dr.RMASQ_RMADID
                drStatus.RMAD_STATUS = 35
                dtStatus.AddRMADetailStatusRow(drStatus)

                Call oRMAStatus.ChangeStatus(oExecute, dtStatus)

                blnFlag = True

            Catch ex As Exception
                blnFlag = False

            Finally
                If blnFlag = False Then
                    oConn.Rollback()
                Else
                    oConn.Commit()
                End If

                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' 業務報價-修改
        ''' </summary>
        ''' <param name="dtSale"></param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtSale As RmaDTO.RMASALE_QUOTEDDataTable)
            Dim i As Integer = 0
            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim dr As RmaDTO.RMASALE_QUOTEDRow = dtSale.Rows(0)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("RMASQ_LABORCOST", dr.RMASQ_LABORCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_MATERIALCOST", dr.RMASQ_MATERIALCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_QUOTE", dr.RMASQ_QUOTE, OracleType.Number)
                oExecute.addParameter("RMASQ_LUAD", dr.RMASQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUADNAME", dr.RMASQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUSTMP", dr.RMASQ_LUSTMP, OracleType.DateTime)

                If dr.IsRMASQ_DESCNull = False Then oExecute.addParameter("RMASQ_DESC", dr.RMASQ_DESC, OracleType.NVarChar)
                If dr.IsRMASQ_SALEADNull = False Then oExecute.addParameter("RMASQ_SALEAD", dr.RMASQ_SALEAD, OracleType.NVarChar)
                If dr.IsRMASQ_SALEADNAMENull = False Then oExecute.addParameter("RMASQ_SALEADNAME", dr.RMASQ_SALEADNAME, OracleType.NVarChar)
                If dr.IsRMASQ_SALEDATENull = False Then oExecute.addParameter("RMASQ_SALEDATE", dr.RMASQ_SALEDATE, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTADNull = False Then oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTADNAMENull = False Then oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTDATENull = False Then oExecute.addParameter("RMASQ_CLIENTDATE", dr.RMASQ_CLIENTDATE, OracleType.NVarChar)
                If dr.IsRMASQ_CLIENTCONFIRMNull = False Then oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                oExecute.addWHERE("RMASQ_ID", dr.RMASQ_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)

                '=============================================================================================================
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '變更RMADetail的狀態(RMAD_STATUS==>35.Sale Quoting), 如果 狀態=30 才可以變更
                '=============================================================================================================
                Dim oRMAStatus As New ctlRMA.RMAStatus
                Dim iStatus As Integer = oRMAStatus.getItemStatus(oExecute, dr.RMASQ_RMADID)
                If iStatus = 30 Then
                    Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                    Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow
                    drStatus.RMAD_ID = dr.RMASQ_RMADID
                    drStatus.RMAD_STATUS = 35
                    dtStatus.AddRMADetailStatusRow(drStatus)

                    Call oRMAStatus.ChangeStatus(oExecute, dtStatus)
                End If

                blnFlag = True

            Catch ex As Exception
                blnFlag = False

            Finally
                If blnFlag = False Then
                    oConn.Rollback()
                Else
                    oConn.Commit()
                End If

                oConn.Close()
                oConn.Dispose()
            End Try

        End Sub

        ''' <summary>
        ''' 業務報價確認
        ''' </summary>
        ''' <param name="dtConfirmed">傳入SalesQuoted_ConfirmedDataTable</param>
        ''' <remarks></remarks>
        Public Sub SalesQuoted_Confirmed(ByVal dtConfirmed As RmaDTO.SalesQuoted_ConfirmedDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                For i = 0 To dtConfirmed.Rows.Count - 1
                    Dim dr As RmaDTO.SalesQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                    oExecute.addParameter("RMASQ_SALEAD", dr.RMASQ_SALEAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_SALEADNAME", dr.RMASQ_SALEADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_SALEDATE", Date.Now, OracleType.DateTime)

                    If dr.IsRMASQ_CLIENTCONFIRMNull = False Then oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                    oExecute.addWHERE("RMASQ_ID", dr.RMASQ_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)
                Next


                Dim oRMAStatus As New ctlRMA.RMAStatus
                Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                For i = 0 To dtConfirmed.Rows.Count - 1
                    Dim dr As RmaDTO.SalesQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow
                    drStatus.RMAD_ID = dr.RMAD_ID
                    drStatus.RMAD_STATUS = dr.RMAD_STATUS
                    dtStatus.AddRMADetailStatusRow(drStatus)
                Next
                If dtStatus.Rows.Count > 0 Then
                    Call oRMAStatus.ChangeStatus(oExecute, dtStatus)
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
        ''' 業務報價 並 確認
        ''' </summary>
        ''' <param name="dtSale">傳入RMASALE_QUOTEDDataTable</param>
        ''' <remarks></remarks>
        Public Sub SaveAdd_SalesConfirmed(ByVal dtSale As RmaDTO.RMASALE_QUOTEDDataTable)
            Dim i As Integer = 0
            Dim blnFlag As Boolean = False

            Dim dtSalesQuoted As New DataTable
            Dim dr As RmaDTO.RMASALE_QUOTEDRow = dtSale.Rows(0)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oQuery.addWHERE("RMASQ_RMADID", ":RMASQ_RMADID", dr.RMASQ_RMADID, OracleType.VarChar)
                Dim sCondition As String = " RMASQ_RMADID =:RMASQ_RMADID"

                'RMASQ_CLIENTCONFIRM: 1.客戶確認, 2.業務帶客戶確認
                Dim sSQL As String = "SELECT * FROM RMASALE_QUOTED WHERE " & sCondition
                dtSalesQuoted = oQuery.ExecuteDT(sSQL)

                If dtSalesQuoted.Rows.Count > 0 Then
                    'edit RMASALE_QUOTED
                    oExecute.addParameter("RMASQ_RMADID", dr.RMASQ_RMADID, OracleType.VarChar)
                    oExecute.addParameter("RMASQ_LABORCOST", dr.RMASQ_LABORCOST, OracleType.Number)
                    oExecute.addParameter("RMASQ_MATERIALCOST", dr.RMASQ_MATERIALCOST, OracleType.Number)
                    oExecute.addParameter("RMASQ_QUOTE", dr.RMASQ_QUOTE, OracleType.Number)
                    oExecute.addParameter("RMASQ_CURRENCYCODE", dr.RMASQ_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMASQ_CURRENCYRATE", dr.RMASQ_CURRENCYRATE, OracleType.Number)

                    oExecute.addParameter("RMASQ_AD", dr.RMASQ_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_ADNAME", dr.RMASQ_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_CSTMP", dr.RMASQ_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("RMASQ_LUAD", dr.RMASQ_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_LUADNAME", dr.RMASQ_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_LUSTMP", dr.RMASQ_LUSTMP, OracleType.DateTime)

                    If dr.IsRMASQ_DESCNull = False Then oExecute.addParameter("RMASQ_DESC", dr.RMASQ_DESC, OracleType.NVarChar)

                    If dr.IsRMASQ_SALEADNull = False Then oExecute.addParameter("RMASQ_SALEAD", dr.RMASQ_SALEAD, OracleType.NVarChar)
                    If dr.IsRMASQ_SALEADNAMENull = False Then oExecute.addParameter("RMASQ_SALEADNAME", dr.RMASQ_SALEADNAME, OracleType.NVarChar)
                    If dr.IsRMASQ_SALEDATENull = False Then oExecute.addParameter("RMASQ_SALEDATE", dr.RMASQ_SALEDATE, OracleType.DateTime)

                    If dr.IsRMASQ_CLIENTADNull = False Then oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                    If dr.IsRMASQ_CLIENTADNAMENull = False Then oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                    If dr.IsRMASQ_CLIENTDATENull = False Then oExecute.addParameter("RMASQ_CLIENTDATE", dr.RMASQ_CLIENTDATE, OracleType.DateTime)

                    If dr.IsRMASQ_CLIENTCONFIRMNull = False Then oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                    oExecute.addWHERE("RMASQ_RMADID", dr.RMASQ_RMADID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)

                Else
                    'add new RMASALE_QUOTED
                    oExecute.addParameter("RMASQ_ID", dr.RMASQ_ID, OracleType.VarChar)

                    oExecute.addParameter("RMASQ_RMADID", dr.RMASQ_RMADID, OracleType.VarChar)
                    oExecute.addParameter("RMASQ_LABORCOST", dr.RMASQ_LABORCOST, OracleType.Number)
                    oExecute.addParameter("RMASQ_MATERIALCOST", dr.RMASQ_MATERIALCOST, OracleType.Number)
                    oExecute.addParameter("RMASQ_QUOTE", dr.RMASQ_QUOTE, OracleType.Number)
                    oExecute.addParameter("RMASQ_CURRENCYCODE", dr.RMASQ_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMASQ_CURRENCYRATE", dr.RMASQ_CURRENCYRATE, OracleType.Number)

                    oExecute.addParameter("RMASQ_AD", dr.RMASQ_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_ADNAME", dr.RMASQ_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_CSTMP", dr.RMASQ_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("RMASQ_LUAD", dr.RMASQ_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_LUADNAME", dr.RMASQ_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASQ_LUSTMP", dr.RMASQ_LUSTMP, OracleType.DateTime)

                    If dr.IsRMASQ_DESCNull = False Then oExecute.addParameter("RMASQ_DESC", dr.RMASQ_DESC, OracleType.NVarChar)

                    If dr.IsRMASQ_SALEADNull = False Then oExecute.addParameter("RMASQ_SALEAD", dr.RMASQ_SALEAD, OracleType.NVarChar)
                    If dr.IsRMASQ_SALEADNAMENull = False Then oExecute.addParameter("RMASQ_SALEADNAME", dr.RMASQ_SALEADNAME, OracleType.NVarChar)
                    If dr.IsRMASQ_SALEDATENull = False Then oExecute.addParameter("RMASQ_SALEDATE", dr.RMASQ_SALEDATE, OracleType.DateTime)

                    If dr.IsRMASQ_CLIENTADNull = False Then oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                    If dr.IsRMASQ_CLIENTADNAMENull = False Then oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                    If dr.IsRMASQ_CLIENTDATENull = False Then oExecute.addParameter("RMASQ_CLIENTDATE", dr.RMASQ_CLIENTDATE, OracleType.DateTime)

                    If dr.IsRMASQ_CLIENTCONFIRMNull = False Then oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.AddNew)
                End If


                '=============================================================================================================
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '變更RMADetail的狀態(RMAD_STATUS==>35.Sale Quoting)
                '=============================================================================================================
                Dim oRMAStatus As New ctlRMA.RMAStatus
                Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow
                drStatus.RMAD_ID = dr.RMASQ_RMADID
                drStatus.RMAD_STATUS = 40
                dtStatus.AddRMADetailStatusRow(drStatus)

                Call oRMAStatus.ChangeStatus(oExecute, dtStatus)

                blnFlag = True

            Catch ex As Exception
                blnFlag = False

            Finally
                If blnFlag = False Then
                    oConn.Rollback()
                Else
                    oConn.Commit()
                End If

                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

        Public Sub Edit_SalesQuoted(ByVal RMASQ_RMADID As String, ByVal RMASQ_LABORCOST As Decimal, ByVal RMASQ_MATERIALCOST As Decimal, ByVal RMASQ_QUOTE As Decimal,
            ByVal RMASQ_LUAD As String, ByVal RMASQ_LUADNAME As String, ByVal RMASQ_LUSTMP As DateTime)

            Dim blnFlag As Boolean = False
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("RMASQ_LABORCOST", RMASQ_LABORCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_MATERIALCOST", RMASQ_MATERIALCOST, OracleType.Number)
                oExecute.addParameter("RMASQ_QUOTE", RMASQ_QUOTE, OracleType.Number)

                oExecute.addParameter("RMASQ_LUAD", RMASQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUADNAME", RMASQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASQ_LUSTMP", RMASQ_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("RMASQ_RMADID", RMASQ_RMADID, OracleType.VarChar)
                oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)
                blnFlag = True

            Catch ex As Exception
                blnFlag = False

            Finally
                If blnFlag = False Then
                    oConn.Rollback()
                Else
                    oConn.Commit()
                End If

                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub
    End Class
#End Region

#Region "Class:Shipment:業務出貨確認單"
    Public Class Shipment
        Dim _oLanguage As New ctlLanguage
        Dim _PrimaryKey As String = ""

        ''' <summary>
        ''' 取得要 Shipment 的客戶EndUser, 只能挑選 維修中心 的客戶
        ''' </summary>
        ''' <param name="SCuNO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByShipment_CustomerEU(ByVal SCuNO As String) As DataTable
            Dim sCondition As String = ""
            Dim dtCustomerEU As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", SCuNO, OracleType.VarChar)
                sCondition = sCondition & " AND rma.RMA_CUNO=:RMA_CUNO " + vbNewLine

                'RMAD_RECEVSTATUS -->註解	是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                Dim sSQL As String = ""
                sSQL = "SELECT DISTINCT RMA.RMA_EUCOMPANY CU_NO,RMA.RMA_EUCOMPANY CU_NAME,RMA.RMA_EUADDRESS" + vbNewLine
                sSQL += "FROM RMADETAIL " + vbNewLine
                sSQL += "INNER Join RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO" + vbNewLine
                sSQL += "INNER Join Company ON RMA.RMA_COMPNO = Company.COMP_NO" + vbNewLine
                sSQL += "INNER JOIN (SELECT * From RMA INNER Join CUSTOMER On RMA.RMA_CUNO = CUSTOMER.CU_NO) vwCustomer ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO" + vbNewLine
                sSQL += "INNER Join RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMA_SHIPMENT ON RMA_SHIPMENTDETAIL.RMASMD_RMASMID = RMA_SHIPMENT.RMASM_ID" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPINGDETAIL.RMASHD_SHIPMENTNO = RMA_SHIPMENT.RMASM_PACKINGNO" + vbNewLine
                sSQL += "LEFT OUTER JOIN RMA_SHIPPING ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID" + vbNewLine
                sSQL += "WHERE RMA.RMA_MARK = '0'" + vbNewLine
                sSQL += "And RMAD_MARK = '0'" + vbNewLine
                '修改也要帶出來 marked by hugh 2024/9/27
                'sSQL += "And RMASMD_RMADID Is NULL" + vbNewLine                
                '修改 by hugh 2024/9/27, 只抓未結案/Saved 的部份
                'sSQL += "AND ( (RMAD_STATUS = '60' AND RMAR_REPAIRAD IS NOT NULL)  Or (RMAD_STATUS = '91' AND RMAD_RECEVSTATUS = 1)) " + vbNewLine
                sSQL += "AND ( (RMAD_STATUS = '60' AND RMAR_REPAIRAD IS NOT NULL AND (RMASH_ISSUBMIT = 0 or RMASH_ISSUBMIT is null) )  Or (RMAD_STATUS = '91' AND RMAD_RECEVSTATUS = 1 AND (RMASH_ISSUBMIT = 0 or RMASH_ISSUBMIT is null) )) " + vbNewLine
                'sSQL += "AND rma.RMA_CUNO = 'BU046' "+ vbNewLine 
                sSQL = sSQL & sCondition
                If SCuNO.Trim() = "BU046" Then
                    sSQL += "AND NVL(RMA.RMA_EUCOMPANY,' ') <> ' ' " + vbNewLine
                Else
                    sSQL += "AND ( NVL(RMA.RMA_EUCOMPANY,' ') <> ' ') " + vbNewLine
                End If
                sSQL += "ORDER BY RMA.RMA_EUCOMPANY DESC " + vbNewLine

                dtCustomerEU = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomerEU
        End Function

        ''' <summary>
        ''' 取得要 Shipment 的客戶, 只能挑選 維修中心 的客戶
        ''' </summary>
        ''' <param name="RepairCenter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByShipment_Customer(ByVal RepairCenter As String) As DataTable
            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim OrderBY As String = ""

            Dim dtCustomer As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'OrderBY = " ORDER BY CU_NO asc" & OrderBY
                OrderBY = " ORDER BY CU_Name asc" & OrderBY

                '維修中心
                Dim arrRepairCenter() As String = RepairCenter.Split(",")
                For i = 0 To arrRepairCenter.Length - 1
                    If sCondition.Trim <> "" Then
                        sCondition = sCondition & " OR "
                    End If
                    sCondition = sCondition & " comp_no = '" & arrRepairCenter(i).Trim() & "'"
                Next
                If sCondition.Trim <> "" Then
                    sCondition = " AND (" & sCondition & ")"
                End If


                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CU_STATUS", ":CU_STATUS", 1, OracleType.Int16)
                sCondition = sCondition & " AND (CU_STATUS=:CU_STATUS)"


                'Dim sSQL As String = " SELECT CU_NO,CU_NAME "
                'sSQL = sSQL & " FROM CUSTOMER "
                'sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                'sSQL = sSQL & " INNER JOIN RMA ON RMA.RMA_CUNO = CUSTOMER.CU_NO"
                'sSQL = sSQL & " INNER JOIN RMADETAIL ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                'sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID"
                ''sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID and RMAR_REPAIRAD is not null"
                'sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID"
                'sSQL = sSQL & " WHERE RMA_MARK ='0' AND RMAD_MARK='0' AND ((RMAD_STATUS='60' AND RMAR_REPAIRAD is not null) OR (RMAD_STATUS='91' AND RMAD_RECEVSTATUS=1)) AND RMASMD_ID IS NULL " & sCondition
                'sSQL = sSQL & " GROUP BY CU_NO,CU_NAME " & OrderBY

                Dim sSQL As String = "SELECT CU_NO, CU_NAME " &
                                     "   FROM CUSTOMER " &
                                     "        INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO" &
                                     "        INNER JOIN RMA ON RMA.RMA_CUNO = CUSTOMER.CU_NO" &
                                     "        INNER JOIN RMADETAIL ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO" &
                                     "        INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID" &
                                     "        LEFT OUTER JOIN RMA_SHIPMENTDETAIL" &
                                     "           ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID" &
                                     "        LEFT OUTER JOIN RMA_SHIPPINGDETAIL" &
                                     "           ON RMA.RMA_NO = RMA_SHIPPINGDETAIL.RMASHD_RMANO" &
                                     "        LEFT OUTER JOIN RMA_SHIPPING" &
                                     "           ON RMA_SHIPPINGDETAIL.RMASHD_RMASHID = RMA_SHIPPING.RMASH_ID" &
                                     "  WHERE     RMA_MARK = '0'" &
                                     "        AND RMAD_MARK = '0'" &
                                     "        AND (   (RMAD_STATUS = '60' AND RMAR_REPAIRAD IS NOT NULL)" &
                                     "             OR (RMAD_STATUS = '91' AND RMAD_RECEVSTATUS = 1))" &
                                      "       AND (RMASMD_ID IS NULL OR (RMAD_STATUS <= 60 AND RMASH_ISSUBMIT = 0))" & sCondition
                sSQL = sSQL & " GROUP BY CU_NO,CU_NAME " & OrderBY

                dtCustomer = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 取得要 Shipment 的客戶, 只能挑選 業務 負責的客戶
        ''' </summary>
        ''' <param name="SalesID">業務代碼及業務助理代碼</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>tmpCustomerBySaleDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByShipment_Customer(ByVal SalesID As String, ByVal CuName As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpCustomerBySaleDataTable
            Dim sCondition As String = ""
            Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    'OrderBY = " CU_NO asc"
                    OrderBY = " CU_Name asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '業務代碼及業務助理代碼
                'Dim sCuAssistantID As String = sCuSalesID.Trim()

                oQuery.addWHERE("CU_SALESID", ":CU_SALESID", SalesID.ToLower().Trim(), OracleType.VarChar)
                oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", SalesID.ToLower().Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND (lower(CU_SALESID)=:CU_SALESID OR lower(CU_ASSISTANTID)=:CU_ASSISTANTID)"

                'sCondition = sCondition & " AND CU_SALESID=:CU_SALESID"


                '客戶名稱
                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CU_STATUS", ":CU_STATUS", 1, OracleType.Int16)
                sCondition = sCondition & " AND (CU_STATUS=:CU_STATUS)"


                Dim sSQL As String = " SELECT CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID, "
                sSQL = sSQL & " CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 "
                sSQL = sSQL & " FROM CUSTOMER "
                sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " INNER JOIN CURRENCY ON COMPANY.COMP_CURRENCYCODE = CURRENCY.CURRENCY_CODE "
                sSQL = sSQL & " INNER JOIN RMA ON RMA.RMA_CUNO = CUSTOMER.CU_NO"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                'sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID and RMAR_REPAIRAD is not null"
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " WHERE RMA_MARK ='0' AND RMAD_MARK='0' AND ((RMAD_STATUS='60' AND RMAR_REPAIRAD is not null) OR (RMAD_STATUS='91' AND RMAD_RECEVSTATUS=1)) " & sCondition
                sSQL = sSQL & " GROUP BY CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID, CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 " & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 挑選要出貨的單子
        ''' </summary>
        ''' <param name="sCuID"></param>
        ''' <param name="sCuSalesID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryCustomerByRMADetail(ByVal sCuID As String, ByVal sCuSalesID As String, Optional ByVal OrderBY As String = "") As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA.RMA_NO desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", sCuID, OracleType.VarChar)
                sCondition = sCondition & " AND rma.RMA_CUNO=:RMA_CUNO "

                If sCuSalesID.Trim <> "" Then
                    oQuery.addWHERE("RMA_EUCOMPANY", ":RMA_EUCOMPANY", sCuSalesID.Replace("'", "^"), OracleType.VarChar)

                    '修改寫法如果有單引號字元會出錯 by buck modify 2025.08.20
                    'Mail:FRMA-2025080008 無法結案
                    'sCondition = sCondition & " AND rma.RMA_EUCOMPANY=:RMA_EUCOMPANY "
                    sCondition = sCondition & " AND REPLACE(rma.RMA_EUCOMPANY, '''', '^')=:RMA_EUCOMPANY "
                End If

                'RMAD_RECEVSTATUS -->註解	是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                Dim sSQL As String = "SELECT "
                sSQL = sSQL & " RMASMD_RMASMID, RMASMD_ID, RMA.RMA_NO as RMASMD_RMANO, RMAD_ID as RMASMD_RMADID, "
                'sSQL = sSQL & " RMAD_MODELNO as RMASMD_MODELNO, RMAD_SERIALNO as RMASMD_SERIALNO, RMAD_PARTSN, "
                sSQL = sSQL & " FN_GETMMODELNO(RMAD_MODELNO,vwCustomer.RMA_COMPNO,vwCustomer.RMA_ACCOUNTID) as RMASMD_MODELNO, RMAD_SERIALNO as RMASMD_SERIALNO, RMAD_PARTSN, "
                sSQL = sSQL & " RMASQ_LABORCOST as RMARSD_LABORCOST, RMASQ_MATERIALCOST as RMARSD_MATERIALCOST, RMASQ_QUOTE as RMARSD_QUOTE, "
                sSQL = sSQL & " RMASQ_CURRENCYCODE as RMARSD_CURRENCYCODE, RMASQ_CURRENCYRATE as RMARSD_CURRENCYRATE, "
                sSQL = sSQL & " RMAR_ASSIGELABORCOST as RMARSD_oldLABORCOST, RMAR_ASSIGEMATERIALCOST as RMARSD_oldMATERIALCOST, RMAR_ASSIGEQUOTE as RMARSD_oldQUOTE, "
                sSQL = sSQL & " rma.RMA_ID as RMASMD_oldRMAID, COMP_LOWESTDISCOUNT as RMASMD_LOWESTDISCOUNT"
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN Company ON RMA.RMA_COMPNO = Company.COMP_NO"
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID"
                'sSQL = sSQL & " AND RMAR_REPAIRAD is not null " 
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " WHERE RMA.RMA_MARK ='0' AND RMAD_MARK='0' AND RMASMD_RMADID IS NULL"

                '========================================================================================================================================================================================================================================================================================
                '2011/08/04 START
                '原Code
                '========================================================================================================================================================================================================================================================================================
                '針對有維修的項目
                'sSQL = sSQL & " AND (RMAD_STATUS='60') AND RMAR_REPAIRAD is not null"

                '可以納入有收貨不維修的項目
                sSQL = sSQL & " AND ((RMAD_STATUS='60' AND RMAR_REPAIRAD is not null) OR (RMAD_STATUS='91' AND RMAD_RECEVSTATUS=1))"
                '========================================================================================================================================================================================================================================================================================
                '2011/08/04 END
                '========================================================================================================================================================================================================================================================================================

                '20210624 加入只抓不是寄全保的  BU046只要沒有EndUser 全抓
                If sCuSalesID.Trim = "" Then
                    If sCuID.Trim() = "BU046" Then
                        sSQL = sSQL & "AND NVL(RMA.RMA_EUCOMPANY,' ') = ' ' "
                    Else
                        sSQL = sSQL & "AND NOT ( NVL(RMA.RMA_EUCOMPANY,' ') <> ' ') "
                    End If
                End If



                sSQL = sSQL & sCondition & OrderBY


                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' By RMA_DETAIL 的ID查詢資料
        ''' </summary>
        ''' <param name="RMAD_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryCustomerByRMADetailID(ByVal RMAD_ID As String) As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMADETAIL.RMAD_ID=:RMAD_ID"

                Dim sSQL As String = "SELECT "
                sSQL = sSQL & " RMASMD_RMASMID, RMASMD_ID, RMA.RMA_NO as RMASMD_RMANO, RMAD_ID as RMASMD_RMADID, "
                sSQL = sSQL & " RMAD_MODELNO as RMASMD_MODELNO, RMAD_SERIALNO as RMASMD_SERIALNO, RMAD_PARTSN, "
                sSQL = sSQL & " RMASQ_LABORCOST as RMARSD_LABORCOST, RMASQ_MATERIALCOST as RMARSD_MATERIALCOST, RMASQ_QUOTE as RMARSD_QUOTE, "
                sSQL = sSQL & " nvl(RMASQ_CURRENCYCODE,' ') as RMARSD_CURRENCYCODE, nvl(RMASQ_CURRENCYRATE,0) as RMARSD_CURRENCYRATE, "

                sSQL = sSQL & " RMAR_ASSIGELABORCOST as RMARSD_oldLABORCOST, RMAR_ASSIGEMATERIALCOST as RMARSD_oldMATERIALCOST, RMAR_ASSIGEQUOTE as RMARSD_oldQUOTE, "
                sSQL = sSQL & " nvl(RMAR_ASSIGECURRENCYCODE,' ') as RMARSD_oldCURRENCYCODE, nvl(RMAR_ASSIGECURRENCYRATE,0) as RMARSD_oldCURRENCYRATE,"

                sSQL = sSQL & " rma.RMA_ID as RMASMD_oldRMAID, RMAD_STATUS, RMAD_RECEVSTATUS, "
                sSQL = sSQL & " COMP_LOWESTDISCOUNT as RMASMD_LOWESTDISCOUNT"
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN Company ON RMA.RMA_COMPNO = Company.COMP_NO"
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                'sSQL = sSQL & " AND RMAR_REPAIRAD is not null"
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " WHERE RMA.RMA_MARK ='0' AND RMAD_MARK='0'"
                'sSQL = sSQL & " AND RMASMD_RMADID IS NULL"
                sSQL = sSQL & " AND (RMAD_STATUS='60' OR (RMAD_STATUS='91' AND RMAD_RECEVSTATUS=1))"
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' By RMA_DETAIL 的ID查詢資料 給 ShippingNotice save 用的
        ''' </summary>
        ''' <param name="RMAD_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryCustomerByRMADetailID_Save(ByVal RMAD_ID As String) As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMADETAIL.RMAD_ID=:RMAD_ID"

                Dim sSQL As String = "SELECT "
                sSQL = sSQL & " RMASMD_RMASMID, RMASMD_ID, RMA.RMA_NO as RMASMD_RMANO, RMAD_ID as RMASMD_RMADID, "
                sSQL = sSQL & " RMAD_MODELNO as RMASMD_MODELNO, RMAD_SERIALNO as RMASMD_SERIALNO, "
                sSQL = sSQL & " RMASQ_LABORCOST as RMARSD_LABORCOST, RMASQ_MATERIALCOST as RMARSD_MATERIALCOST, RMASQ_QUOTE as RMARSD_QUOTE, "
                sSQL = sSQL & " nvl(RMASQ_CURRENCYCODE,' ') as RMARSD_CURRENCYCODE, nvl(RMASQ_CURRENCYRATE,0) as RMARSD_CURRENCYRATE, "

                sSQL = sSQL & " RMAR_ASSIGELABORCOST as RMARSD_oldLABORCOST, RMAR_ASSIGEMATERIALCOST as RMARSD_oldMATERIALCOST, RMAR_ASSIGEQUOTE as RMARSD_oldQUOTE, "
                sSQL = sSQL & " nvl(RMAR_ASSIGECURRENCYCODE,' ') as RMARSD_oldCURRENCYCODE, nvl(RMAR_ASSIGECURRENCYRATE,0) as RMARSD_oldCURRENCYRATE,"

                sSQL = sSQL & " rma.RMA_ID as RMASMD_oldRMAID, RMAD_STATUS, RMAD_RECEVSTATUS, "
                sSQL = sSQL & " COMP_LOWESTDISCOUNT as RMASMD_LOWESTDISCOUNT"
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN Company ON RMA.RMA_COMPNO = Company.COMP_NO"
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                'sSQL = sSQL & " AND RMAR_REPAIRAD is not null"
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " WHERE RMA.RMA_MARK ='0' AND RMAD_MARK='0'"
                'sSQL = sSQL & " AND RMASMD_RMADID IS NULL"
                sSQL = sSQL & " AND (RMAD_STATUS='60' OR RMAD_STATUS='90' OR (RMAD_STATUS='91' AND RMAD_RECEVSTATUS=1))"
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 取得 RMA_Shipping
        ''' </summary>
        ''' <param name="sRMANo">RMA No</param>
        ''' <param name="sShipmentID">Shipment ID</param>
        ''' <param name="sCuSalesID">sCuSalesID</param>
        ''' <param name="Notice">Notice</param>
        ''' <param name="Tracking">Tracking</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="Serial">Serial</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>回傳ShipmentDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_Shipping(ByVal sRMANo As String, ByVal sShipmentID As String, ByVal sCuSalesID As String, ByVal Notice As String, ByVal Tracking As String,
            ByVal CuName As String, ByVal Serial As String, ByVal fdate As String, ByVal edate As String,
             Optional ByVal OrderBY As String = "") As RmaDTO.ShipmentDataTable

            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.ShipmentDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMASM_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If sRMANo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMASMD_RMANO", ":RMASMD_RMANO", sRMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMASMD_RMANO=:RMASMD_RMANO"
                End If

                If sShipmentID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMASM_ID", ":RMASM_ID", sShipmentID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMASM_ID=:RMASM_ID"
                Else
                    '列表List使用的條件,排除RMADetail單狀態大於等於'90'
                    oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", "90", OracleType.Int16)
                    sCondition = sCondition & " AND RMAD_STATUS <:RMAD_STATUS"
                End If

                '業務代碼OR業務助理代碼
                oQuery.addWHERE("CU_SALESID", ":CU_SALESID", sCuSalesID, OracleType.VarChar)
                oQuery.addWHERE("CU_ASSISTANTID", ":CU_ASSISTANTID", sCuSalesID, OracleType.VarChar)
                sCondition = sCondition & " AND (CU_SALESID=:CU_SALESID OR CU_ASSISTANTID=:CU_ASSISTANTID)"

                If Notice.ToString().Trim() <> "" Then
                    Notice = "%" & Notice.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMASM_PACKINGNO", ":RMASM_PACKINGNO", Notice, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMASM_PACKINGNO) like :RMASM_PACKINGNO"
                End If

                If Tracking.ToString().Trim() <> "" Then
                    Tracking = "%" & Tracking.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMASH_SHIPPINGNO", ":RMASH_SHIPPINGNO", Tracking, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMASH_SHIPPINGNO) like :RMASH_SHIPPINGNO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Serial.ToString().Trim() <> "" Then
                    Serial = "%" & Serial.ToLower.Trim() & "%"
                    oQuery.addWHERE("RMASMD_SERIALNO", ":RMASMD_SERIALNO", Serial, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMASMD_SERIALNO) like :RMASMD_SERIALNO"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMASM_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMASM_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMASM_CSTMP", ":RMASM_CSTMP", RMASM_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMASM_LUSTMP", ":RMASM_LUSTMP", RMASM_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMASM_CSTMP >=:RMASM_CSTMP AND RMASM_LUSTMP <=:RMASM_LUSTMP)"
                End If

                'Dim sSQL As String = " SELECT RMA_SHIPMENT.*, CU_NAME "
                'sSQL = sSQL & " FROM RMA_SHIPMENT "
                'sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPMENT.RMASM_CUNO = CUSTOMER.CU_NO "
                'sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                'sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

                Dim sSQL As String = " SELECT RMASMD_RMANO, RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM, RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPMENT.RMASM_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA_SHIPPINGDETAIL "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPING ON RMA_SHIPPINGDETAIL.RMASHD_RMASHID = RMA_SHIPPING.RMASH_ID "
                sSQL = sSQL & " ) vwRMASHIPPINGDETAIL "
                sSQL = sSQL & " ON RMA_SHIPMENT.RMASM_PACKINGNO = vwRMASHIPPINGDETAIL.RMASHD_RMASMPACKINGNO "
                sSQL = sSQL & " AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = vwRMASHIPPINGDETAIL.RMASHD_RMANO "
                sSQL = sSQL & " WHERE 1=1" & sCondition
                sSQL = sSQL & " GROUP BY RMASMD_RMANO, RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM,RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' By Shipment ID,Get Shipment No
        ''' </summary>
        ''' <param name="sShipmentID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryShipmentByShipmentID(ByVal sShipmentID As String) As RmaDTO.ShipmentDataTable

            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.ShipmentDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If sShipmentID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMASM_ID", ":RMASM_ID", sShipmentID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMASM_ID=:RMASM_ID"
                Else
                    '列表List使用的條件,排除RMADetail單狀態大於等於'90'
                    oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", "90", OracleType.Int16)
                    sCondition = sCondition & " AND RMAD_STATUS <:RMAD_STATUS"
                End If

                Dim sSQL As String = " SELECT RMASMD_RMANO, RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM, RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPMENT.RMASM_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA_SHIPPINGDETAIL "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPING ON RMA_SHIPPINGDETAIL.RMASHD_RMASHID = RMA_SHIPPING.RMASH_ID "
                sSQL = sSQL & " ) vwRMASHIPPINGDETAIL "
                sSQL = sSQL & " ON RMA_SHIPMENT.RMASM_PACKINGNO = vwRMASHIPPINGDETAIL.RMASHD_RMASMPACKINGNO "
                sSQL = sSQL & " AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = vwRMASHIPPINGDETAIL.RMASHD_RMANO "
                sSQL = sSQL & " WHERE 1=1" & sCondition
                sSQL = sSQL & " GROUP BY RMASMD_RMANO, RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM,RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        Public Function QueryShipmentByShipingID(ByVal sShipingtID As String) As RmaDTO.ShipmentDataTable

            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.ShipmentDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If sShipingtID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMASH_ID", ":RMASH_ID", sShipingtID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMASH_ID=:RMASH_ID"
                End If

                Dim sSQL As String = " SELECT c.* FROM RMA_SHIPPING a,RMA_SHIPPINGDETAIL b,RMA_SHIPMENT c"
                sSQL += " WHERE a.RMASH_ID=b.RMASHD_RMASHID AND b.RMASHD_SHIPMENTNO=c.RMASM_PACKINGNO " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 取得 Notice No
        ''' </summary>
        ''' <param name="RMASM_ID">RMASM_ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getNoticeNo(ByVal RMASM_ID As String) As String
            Dim retval As String = ""
            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try
                oQuery.addWHERE("RMASM_ID", ":RMASM_ID", RMASM_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_ID=:RMASM_ID"

                sSQL = "SELECT * FROM RMA_SHIPMENT WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    retval = dt.Rows(0)("RMASM_PACKINGNO").ToString().Trim()
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
        ''' 取得 Shipment 裡的維修人員Mail及姓名
        ''' </summary>
        ''' <param name="RMASM_ID">RMASM_ID</param>
        ''' <returns>回傳 ArrayList</returns>
        ''' <remarks></remarks>
        Public Function getRepaireMail(ByVal RMASM_ID As String) As ArrayList
            Dim retArrList As New ArrayList
            Dim i As Integer = 0
            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""


            oConn.Open()
            Try
                oQuery.addWHERE("RMASMD_RMASMID", ":RMASMD_RMASMID", RMASM_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMASMID=:RMASMD_RMASMID"

                sSQL = "SELECT RMAR_REPAIRADNAME, AD_EMAIL FROM RMA_SHIPMENTDETAIL, RMAREPAIR, admin " &
                    " WHERE RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMAREPAIR.RMAR_RMADID" &
                    " AND admin.AD_ID = RMAREPAIR.RMAR_REPAIRAD" & sCondition &
                    " GROUP BY RMAR_REPAIRADNAME, AD_EMAIL"

                dt = oQuery.ExecuteDT(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    Dim arrLiat(1) As String
                    arrLiat(0) = dt.Rows(i)("RMAR_REPAIRADNAME").ToString().Trim()
                    arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                    retArrList.Add(arrLiat)
                Next

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retArrList
        End Function

        ''' <summary>
        ''' 取得 Shipment 裡的出貨人員Mail及姓名
        ''' </summary>
        ''' <param name="RMASM_ID">RMASM_ID</param>
        ''' <returns>回傳 ArrayList</returns>
        ''' <remarks></remarks>
        Public Function getShippingMail(ByVal RMASM_ID As String) As ArrayList
            Dim retArrList As New ArrayList
            Dim i As Integer = 0
            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""


            oConn.Open()
            Try
                oQuery.addWHERE("RMASMD_RMASMID", ":RMASMD_RMASMID", RMASM_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMASMID=:RMASMD_RMASMID"

                sSQL = "SELECT AD_ID, AD_Name, AD_EMAIL FROM RMA_SHIPMENTDETAIL, RMADetail, RMA, admin" &
                        " WHERE RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMADetail.RMAD_ID" &
                        " AND RMADetail.RMAD_RMANO = RMA.RMA_NO" &
                        " AND admin.AD_REPAIRCENTER like RMA.RMA_COMPNO" &
                        " AND AD_ROLE like ('%4%') AND AD_EMAIL is not null " & sCondition &
                        " GROUP BY AD_ID, AD_Name, AD_EMAIL"

                dt = oQuery.ExecuteDT(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    Dim arrLiat(1) As String
                    arrLiat(0) = dt.Rows(i)("AD_Name").ToString().Trim()
                    arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                    retArrList.Add(arrLiat)
                Next

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retArrList
        End Function

        ''' <summary>
        ''' 取得 Shipment 裡的 轉單後的 出貨人員Mail及姓名
        ''' </summary>
        ''' <param name="RMASM_ID">RMASM_ID</param>
        ''' <returns>回傳 ArrayList</returns>
        ''' <remarks></remarks>
        Public Function getShippingMailByAssign(ByVal RMASM_ID As String) As ArrayList
            Dim retArrList As New ArrayList
            Dim i As Integer = 0
            Dim dtRMA As New RmaDTO.RMADataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""


            oConn.Open()
            Try
                oQuery.addWHERE("RMASMD_RMASMID", ":RMASMD_RMASMID", RMASM_ID.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMASMID=:RMASMD_RMASMID"

                sSQL = "SELECT AD_ID, AD_Name, AD_EMAIL FROM RMA_SHIPMENTDETAIL, RMAREPAIR, admin" &
                    " WHERE RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMAREPAIR.RMAR_RMADID" &
                    " AND AD_REPAIRCENTER like RMAR_COMPNO AND AD_ROLE like ('%4%') AND AD_EMAIL is not null " & sCondition &
                    " GROUP BY AD_ID, AD_Name, AD_EMAIL"

                dt = oQuery.ExecuteDT(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    Dim arrLiat(1) As String
                    arrLiat(0) = dt.Rows(i)("AD_Name").ToString().Trim()
                    arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                    retArrList.Add(arrLiat)
                Next

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retArrList
        End Function

        ''' <summary>
        ''' 取得 主管要審核的 Shipment 單
        ''' </summary>
        ''' <param name="RMASM_ID">傳入RMASM_ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryShipmentByBossConfirm(ByVal RMASM_ID As String) As RmaDTO.ShipmentDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.ShipmentDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMASM_ID", ":RMASM_ID", RMASM_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_ID=:RMASM_ID"

                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", "90", OracleType.Int16)
                sCondition = sCondition & " AND RMAD_STATUS <:RMAD_STATUS"

                Dim sSQL As String = " SELECT RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM, RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPMENT.RMASM_CUNO = CUSTOMER.CU_NO AND RMASM_ISBOSSCONFIRM=1"
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA_SHIPPINGDETAIL "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPING ON RMA_SHIPPINGDETAIL.RMASHD_RMASHID = RMA_SHIPPING.RMASH_ID "
                sSQL = sSQL & " ) vwRMASHIPPINGDETAIL "
                sSQL = sSQL & " ON RMA_SHIPMENT.RMASM_PACKINGNO = vwRMASHIPPINGDETAIL.RMASHD_RMASMPACKINGNO "
                sSQL = sSQL & " AND RMA_SHIPMENTDETAIL.RMASMD_RMANO = vwRMASHIPPINGDETAIL.RMASHD_RMANO "
                sSQL = sSQL & " WHERE 1=1" & sCondition
                sSQL = sSQL & " GROUP BY RMASM_ID, RMASM_PACKINGNO, RMASM_CSTMP, RMASM_LUSTMP, "
                sSQL = sSQL & " RMASM_CUNO, RMARSM_CURRENCYCODE, RMARSM_CURRENCYRATE, RMASM_ISSHIP, "
                sSQL = sSQL & " RMASM_SHIPNO, RMASM_SHIPMEMO, RMASM_ISBOSSCONFIRM,RMASM_ISSUBMIT,"
                sSQL = sSQL & " CU_NAME, RMASH_SHIPPINGNO, RMASH_CSTMP "

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 取得 RMA_ShipmentDetail
        ''' </summary>
        ''' <param name="sShipmentID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_ShipmentDetail(ByVal sShipmentID As String, Optional ByVal OrderBY As String = "") As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMASMD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMASMD_RMASMID", ":RMASMD_RMASMID", sShipmentID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMASMID=:RMASMD_RMASMID"

                Dim sSQL As String = " SELECT RMA_SHIPMENTDETAIL.* , '1' as RMASMD_oldMark, "
                sSQL = sSQL & " RMAR_ASSIGELABORCOST as RMARSD_oldLABORCOST, RMAR_ASSIGEMATERIALCOST as RMARSD_oldMATERIALCOST, RMAR_ASSIGEQUOTE as RMARSD_oldQUOTE,"
                sSQL = sSQL & " RMASMD_LOWESTDISCOUNT"
                sSQL = sSQL & " FROM RMA_SHIPMENTDETAIL "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMAREPAIR.RMAR_RMADID "
                'sSQL = sSQL & " INNER JOIN RMA ON RMA_SHIPMENTDETAIL.RMASMD_RMANO  = RMA.RMA_NO"
                'sSQL = sSQL & " INNER JOIN company ON RMA.rma_compno = company.comp_no"
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 從ShippingDetail 取得 RMA_ShipmentDetail 的資料
        ''' </summary>
        ''' <param name="sShippingID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query_ShippingToShipmentDetail(ByVal sShippingID As String, Optional ByVal OrderBY As String = "") As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""

            Dim dtShipping As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMASMD_RMANO desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMASHD_RMASHID", ":RMASHD_RMASHID", sShippingID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASHD_RMASHID=:RMASHD_RMASHID"

                Dim sSQL As String = " select * "
                sSQL = sSQL & " from RMA_SHIPPINGDETAIL "
                sSQL = sSQL & " inner JOIN RMA_SHIPMENT on RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO"
                sSQL = sSQL & " inner JOIN RMA_SHIPMENTDETAIL on RMA_SHIPMENTDETAIL.RMASMD_RMASMID = RMA_SHIPMENT.RMASM_ID"
                sSQL = sSQL & " inner JOIN RMA ON RMA.RMA_NO = RMASMD_RMANO"
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 存檔-新增
        ''' </summary>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <param name="dtShipment">ShipmentDataTable</param>
        ''' <param name="dtShipmentDetail">Shipment_DetailDataTable</param>
        ''' <returns>回傳 RMASM_ID</returns>
        ''' <remarks>
        ''' sType-->Save:不需改RMA狀態,Submit:改RMA狀態
        ''' </remarks>
        Public Function SaveByAddNew(ByVal sType As String, ByVal dtShipment As RmaDTO.ShipmentDataTable, ByVal dtShipmentDetail As RmaDTO.Shipment_DetailDataTable) As String
            Dim i As Integer = 0
            Dim RMASM_ID As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                RMASM_ID = AddNew_RMAShipment(oExecute, dtShipment, sType)

                Call AddNew_RMAShipmentDetail(oExecute, RMASM_ID, dtShipmentDetail)

                '修改RMA單的狀態
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    Call Edit_RMADetail(oExecute, dtShipmentDetail)
                End If

                oConn.Commit()

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return RMASM_ID
        End Function

        ''' <summary>
        ''' 存檔-新增 給 Shipping 用
        ''' </summary>
        ''' <param name="oConn"></param>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <param name="dtShipment">ShipmentDataTable</param>
        ''' <param name="dtShipmentDetail">Shipment_DetailDataTable</param>
        ''' <returns>回傳 RMASM_ID</returns>
        ''' <remarks>
        ''' sType-->Save:不需改RMA狀態,Submit:改RMA狀態
        ''' </remarks>        
        Public Function SaveByAddNew_Shipping(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sType As String, ByVal dtShipment As RmaDTO.ShipmentDataTable, ByVal dtShipmentDetail As RmaDTO.Shipment_DetailDataTable,
                ByRef RMASM_PACKINGNO As String) As String

            Dim i As Integer = 0
            Dim RMASM_ID As String = ""
            Dim oExecute As New Execute(oConn)

            Try

                RMASM_ID = AddNewShipment_Shipping(oExecute, dtShipment, sType, RMASM_PACKINGNO)

                Call AddNew_RMAShipmentDetail_Shipping(oExecute, RMASM_ID, dtShipmentDetail)

                '修改RMA單的狀態
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    Call Edit_RMADetail(oExecute, dtShipmentDetail)
                End If


            Catch ex As Exception
                Throw ex

            Finally
            End Try

            Return RMASM_ID
        End Function
        Public Function ShipmentDelete(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMASM_ID As String) As String
            Dim retval As String = "OK"
            'Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            Try
                'oConn.Open()
                oExecute.addWHERE("RMASMD_RMASMID", RMASM_ID.Trim(), OracleType.VarChar)
                oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.Delete)

                oExecute.addWHERE("RMASM_ID", RMASM_ID, OracleType.NVarChar)
                oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.Delete)
                'oConn.Close()
            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 存檔-修改
        ''' </summary>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <param name="sShipmentID">KEY</param>
        ''' <param name="dtShipping">ShipmentDataTable</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <remarks>
        ''' sType-->Save:不需改RMA狀態,Submit:改RMA狀態
        ''' </remarks>
        Public Sub SaveByEdit(ByVal sType As String, ByVal sShipmentID As String, ByVal dtShipping As RmaDTO.ShipmentDataTable, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            Dim dtShippingDetail_Add As New RmaDTO.Shipment_DetailDataTable
            Dim dtShippingDetail_Edit As New RmaDTO.Shipment_DetailDataTable
            Dim dtShippingDetail_Del As New RmaDTO.Shipment_DetailDataTable

            oConn.Open()
            Try
                'RMASMD_oldMark==>0:新增,1:修改,2:刪除
                dtShippingDetail_Add = dtShippingDetail.Copy
                dtShippingDetail_Edit = dtShippingDetail.Copy
                dtShippingDetail_Del = dtShippingDetail.Copy

                '新增
                Dim dvShippingDetail_Add As DataView = dtShippingDetail_Add.DefaultView()
                dvShippingDetail_Add.RowFilter = "RMASMD_oldMark<>'0'"
                Do While dvShippingDetail_Add.Count > 0
                    dvShippingDetail_Add.Delete(0)
                Loop

                '修改
                Dim dvShippingDetail_Edit As DataView = dtShippingDetail_Edit.DefaultView()
                dvShippingDetail_Edit.RowFilter = "RMASMD_oldMark<>'1'"
                Do While dvShippingDetail_Edit.Count > 0
                    dvShippingDetail_Edit.Delete(0)
                Loop

                '刪除
                Dim dvShippingDetail_Del As DataView = dtShippingDetail_Del.DefaultView()
                dvShippingDetail_Del.RowFilter = "RMASMD_oldMark<>'2'"
                Do While dvShippingDetail_Del.Count > 0
                    dvShippingDetail_Del.Delete(0)
                Loop

                oConn.BeginTransaction()

                Call Edit_RMAShipping(oExecute, dtShipping, sType)

                Call AddNew_RMAShipmentDetail(oExecute, sShipmentID, dtShippingDetail_Add)
                Call Edit_RMAShippingDetail(oExecute, dtShippingDetail_Edit)
                Call Del_RMAShippingDetail(oExecute, dtShippingDetail_Del)


                '修改RMA單的狀態
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    Call Edit_RMADetail(oExecute, dtShippingDetail_Add)
                    Call Edit_RMADetail(oExecute, dtShippingDetail_Edit)
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
        ''' 新增 RMA_SHIPMENT 單頭 給 Shipping 用的
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShipping">ShipmentDataTable</param>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AddNewShipment_Shipping(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShipping As RmaDTO.ShipmentDataTable, ByVal sType As String, ByRef RMASM_PACKINGNO As String) As String
            Dim i As Integer = 0
            Dim retval As String = ""

            Dim oGuid As Guid = Guid.NewGuid
            Dim oBill As New AutoDocumentNo.Bill

            Try
                Dim dr As RmaDTO.ShipmentRow = dtShipping.Rows(0)
                Dim sGUID As String = oGuid.ToString

                RMASM_PACKINGNO = oBill.GetBillNo(oExecute.Connection, "SHM", dr.RMASM_AD, dr.RMASM_ADNAME)

                oExecute.addParameter("RMASM_PACKINGNO", RMASM_PACKINGNO, OracleType.VarChar)            'RMA 編號
                oExecute.addParameter("RMASM_ID", sGUID, OracleType.VarChar)                                '系統自動產生唯一識別碼
                oExecute.addParameter("RMASM_CUNO", dr.RMASM_CUNO, OracleType.VarChar)
                oExecute.addParameter("RMASM_ISSHIP", dr.RMASM_ISSHIP, OracleType.Int16)

                If dr.IsRMASM_SHIPNONull = False Then oExecute.addParameter("RMASM_SHIPNO", dr.RMASM_SHIPNO, OracleType.VarChar)
                If dr.IsRMASM_SHIPMEMONull = False Then oExecute.addParameter("RMASM_SHIPMEMO", dr.RMASM_SHIPMEMO, OracleType.VarChar)

                oExecute.addParameter("RMASM_AD", dr.RMASM_AD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_ADNAME", dr.RMASM_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASM_CSTMP", dr.RMASM_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMASM_LUAD", dr.RMASM_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUADNAME", dr.RMASM_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUSTMP", dr.RMASM_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMARSM_LABORCOST", dr.RMARSM_LABORCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_MATERIALCOST", dr.RMARSM_MATERIALCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_QUOTE", dr.RMARSM_QUOTE, OracleType.Double)
                oExecute.addParameter("RMARSM_CURRENCYCODE", dr.RMARSM_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("RMARSM_CURRENCYRATE", dr.RMARSM_CURRENCYRATE, OracleType.Double)


                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", dr.RMASM_ISBOSSCONFIRM, OracleType.Int16)
                    oExecute.addParameter("RMASM_ISSUBMIT", 1, OracleType.Int16)
                Else
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", 0, OracleType.Int16)   '是否需主管審核:0.否, 1.是, 2.已審核
                    oExecute.addParameter("RMASM_ISSUBMIT", 0, OracleType.Int16)        '是否已Submit: 0.否, 1.是 
                End If

                oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.AddNew)
                retval = sGUID

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 新增 RMA_SHIPMENT 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShipping">ShipmentDataTable</param>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AddNew_RMAShipment(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShipping As RmaDTO.ShipmentDataTable, ByVal sType As String) As String
            Dim i As Integer = 0
            Dim retval As String = ""

            Dim oGuid As Guid = Guid.NewGuid
            Dim oBill As New AutoDocumentNo.Bill

            Try
                Dim dr As RmaDTO.ShipmentRow = dtShipping.Rows(0)
                Dim sGUID As String = oGuid.ToString

                Dim RMASM_PACKINGNO As String = oBill.GetBillNo(oExecute.Connection, "SHM", dr.RMASM_AD, dr.RMASM_ADNAME)

                oExecute.addParameter("RMASM_PACKINGNO", RMASM_PACKINGNO, OracleType.VarChar)            'RMA 編號
                oExecute.addParameter("RMASM_ID", sGUID, OracleType.VarChar)                                '系統自動產生唯一識別碼
                oExecute.addParameter("RMASM_CUNO", dr.RMASM_CUNO, OracleType.VarChar)
                oExecute.addParameter("RMASM_ISSHIP", dr.RMASM_ISSHIP, OracleType.Int16)

                If dr.IsRMASM_SHIPNONull = False Then oExecute.addParameter("RMASM_SHIPNO", dr.RMASM_SHIPNO, OracleType.VarChar)
                If dr.IsRMASM_SHIPMEMONull = False Then oExecute.addParameter("RMASM_SHIPMEMO", dr.RMASM_SHIPMEMO, OracleType.VarChar)

                oExecute.addParameter("RMASM_AD", dr.RMASM_AD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_ADNAME", dr.RMASM_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASM_CSTMP", dr.RMASM_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMASM_LUAD", dr.RMASM_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUADNAME", dr.RMASM_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUSTMP", dr.RMASM_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMARSM_LABORCOST", dr.RMARSM_LABORCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_MATERIALCOST", dr.RMARSM_MATERIALCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_QUOTE", dr.RMARSM_QUOTE, OracleType.Double)
                oExecute.addParameter("RMARSM_CURRENCYCODE", dr.RMARSM_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("RMARSM_CURRENCYRATE", dr.RMARSM_CURRENCYRATE, OracleType.Double)


                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", dr.RMASM_ISBOSSCONFIRM, OracleType.Int16)
                    oExecute.addParameter("RMASM_ISSUBMIT", 1, OracleType.Int16)
                Else
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", 0, OracleType.Int16)   '是否需主管審核:0.否, 1.是, 2.已審核
                    oExecute.addParameter("RMASM_ISSUBMIT", 0, OracleType.Int16)        '是否已Submit: 0.否, 1.是 
                End If

                oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.AddNew)
                retval = sGUID

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 修改 RMA_Shipping 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShipping">ShipmentDataTable</param>
        ''' <param name="sType">表單狀態:Print,Save,Submit</param>
        ''' <remarks></remarks>
        Private Sub Edit_RMAShipping(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShipping As RmaDTO.ShipmentDataTable, ByVal sType As String)

            Try
                Dim dr As RmaDTO.ShipmentRow = dtShipping.Rows(0)

                oExecute.addParameter("RMASM_ISSHIP", dr.RMASM_ISSHIP, OracleType.Int16)
                oExecute.addParameter("RMASM_LUAD", dr.RMASM_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUADNAME", dr.RMASM_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASM_LUSTMP", dr.RMASM_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMARSM_LABORCOST", dr.RMARSM_LABORCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_MATERIALCOST", dr.RMARSM_MATERIALCOST, OracleType.Double)
                oExecute.addParameter("RMARSM_QUOTE", dr.RMARSM_QUOTE, OracleType.Double)

                If dr.IsRMASM_SHIPNONull = False Then oExecute.addParameter("RMASM_SHIPNO", dr.RMASM_SHIPNO, OracleType.VarChar)
                If dr.IsRMASM_SHIPMEMONull = False Then oExecute.addParameter("RMASM_SHIPMEMO", dr.RMASM_SHIPMEMO, OracleType.VarChar)

                '是否需主管審核:0.否, 1.是, 2.已審核
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", dr.RMASM_ISBOSSCONFIRM, OracleType.Int16)
                    oExecute.addParameter("RMASM_ISSUBMIT", 1, OracleType.Int16)
                End If

                oExecute.addWHERE("RMASM_ID", dr.RMASM_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' Shipment 主管審核
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save_BossConfirm(ByVal RMASM_ID As String, ByVal AD As String, ByVal ADName As String)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                oExecute.addParameter("RMASM_BOSSAD", AD, OracleType.NVarChar)
                oExecute.addParameter("RMASM_BOSSADNAME", ADName, OracleType.NVarChar)
                oExecute.addParameter("RMASM_BOSSDATE", Date.Now, OracleType.DateTime)

                '是否需主管審核:0.否, 1.是, 2.已審核
                oExecute.addParameter("RMASM_ISBOSSCONFIRM", 2, OracleType.Int16)

                oExecute.addWHERE("RMASM_ID", RMASM_ID.Trim(), OracleType.VarChar)
                oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.UPDATE)

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
        ''' 列印ShippingReport
        ''' </summary>
        ''' <param name="RMASMID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByReport(ByVal RMASMID As String) As RmaDTO.ShippingSaleRoportDataTable
            Dim sCondition As String = ""
            Dim dtShippingReport As New RmaDTO.ShippingSaleRoportDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMASM_ID", ":RMASM_ID", RMASMID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_ID=:RMASM_ID"

                Dim sSQL As String = " SELECT "
                sSQL = sSQL & " RMA_CUNO, CUSTOMER.CU_NAME, RMA_ACCOUNTID, RMA_APPLICANT, RMA_TEL, RMA_ADDRESS, "
                sSQL = sSQL & " COMP_NAME, RMAD_PRODUCTDESC, RMAD_WARRANTY, RMAD_CSTMP, "
                sSQL = sSQL & " RMASMD_RMANO, RMASMD_SERIALNO, RMASMD_MODELNO, RMASM_CSTMP, RMAD_ISWARRANTY "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMADETAIL.RMAD_ID "
                sSQL = sSQL & " INNER JOIN RMA ON RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPMENT.RMASM_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM CUSTOMER "
                sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " ) vwCompany "
                sSQL = sSQL & " ON RMA_SHIPMENT.RMASM_CUNO = vwCompany.CU_NO "
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShippingReport)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShippingReport
        End Function

        ''' <summary>
        ''' 新增 RMA_SHIPMENTDETAIL 出貨項 給 shipping 用的
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="ShipmentID">關聯 RMASM_SHIPMENT.RMASM_ID-->RMA Shipment 編號</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub AddNew_RMAShipmentDetail_Shipping(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal ShipmentID As String, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0
            Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView

            Dim dtQuery As New DataTable
            Dim oQuery As New ICAT_OracleDAO.Query(oExecute.Connection)

            Try
                dvShippingDetail.RowFilter = "RMASMD_oldMark=0"

                For i = 0 To dvShippingDetail.Count - 1
                    Dim dr As RmaDTO.Shipment_DetailRow = dvShippingDetail(i).Row

                    oQuery.addWHERE("RMASMD_RMADID", ":RMASMD_RMADID", dr.RMASMD_RMADID.ToString().Trim(), OracleType.VarChar)
                    Dim sSQL As String = "SELECT RMASMD_RMADID FROM RMA_SHIPMENTDETAIL WHERE RMASMD_RMADID=:RMASMD_RMADID "
                    dtQuery = oQuery.ExecuteDT(sSQL)

                    'If dtQuery.Rows.Count > 0 Then
                    '    Dim abd As String = ""
                    'End If

                    If dtQuery.Rows.Count = 0 Then
                        Dim oGuid As Guid = Guid.NewGuid

                        oExecute.addParameter("RMASMD_RMASMID", ShipmentID, OracleType.VarChar)         'RMA Shipment 編號
                        oExecute.addParameter("RMASMD_ID", oGuid.ToString, OracleType.VarChar)          '系統自動產生唯一識別碼
                        oExecute.addParameter("RMASMD_RMANO", dr.RMASMD_RMANO, OracleType.VarChar)
                        oExecute.addParameter("RMASMD_RMADID", dr.RMASMD_RMADID, OracleType.VarChar)

                        If dr.IsRMASMD_MODELNONull = False Then oExecute.addParameter("RMASMD_MODELNO", dr.RMASMD_MODELNO, OracleType.VarChar)
                        If dr.IsRMASMD_SERIALNONull = False Then oExecute.addParameter("RMASMD_SERIALNO", dr.RMASMD_SERIALNO, OracleType.VarChar)
                        If dr.IsRMASMD_PARTNONull = False Then oExecute.addParameter("RMASMD_PARTNO", dr.RMASMD_PARTNO, OracleType.VarChar)

                        oExecute.addParameter("RMARSD_LABORCOST", dr.RMARSD_LABORCOST, OracleType.Double)
                        oExecute.addParameter("RMARSD_MATERIALCOST", dr.RMARSD_MATERIALCOST, OracleType.Double)
                        oExecute.addParameter("RMARSD_QUOTE", dr.RMARSD_QUOTE, OracleType.Double)
                        oExecute.addParameter("RMARSD_CURRENCYCODE", dr.RMARSD_CURRENCYCODE, OracleType.VarChar)
                        oExecute.addParameter("RMARSD_CURRENCYRATE", dr.RMARSD_CURRENCYRATE, OracleType.Double)

                        oExecute.addParameter("RMASMD_LOWESTDISCOUNT", dr.RMASMD_LOWESTDISCOUNT, OracleType.Double)

                        oExecute.addParameter("RMASMD_AD", dr.RMASMD_AD, OracleType.NVarChar)
                        oExecute.addParameter("RMASMD_ADNAME", dr.RMASMD_ADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMASMD_CSTMP", dr.RMASMD_CSTMP, OracleType.DateTime)
                        oExecute.addParameter("RMASMD_LUAD", dr.RMASMD_LUAD, OracleType.NVarChar)
                        oExecute.addParameter("RMASMD_LUADNAME", dr.RMASMD_LUADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMASMD_LUSTMP", dr.RMASMD_LUSTMP, OracleType.DateTime)

                        oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.AddNew)
                    End If
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 新增 RMA_SHIPMENTDETAIL 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="ShipmentID">關聯 RMASM_SHIPMENT.RMASM_ID-->RMA Shipment 編號</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub AddNew_RMAShipmentDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal ShipmentID As String, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0
            Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView

            Try
                dvShippingDetail.RowFilter = "RMASMD_oldMark=0"

                '檢核  RMASMD_RMADID 是否存在
                Call chkIsExist(dtShippingDetail)

                For i = 0 To dvShippingDetail.Count - 1
                    Dim dr As RmaDTO.Shipment_DetailRow = dvShippingDetail(i).Row

                    Dim oGuid As Guid = Guid.NewGuid

                    oExecute.addParameter("RMASMD_RMASMID", ShipmentID, OracleType.VarChar)         'RMA Shipment 編號
                    oExecute.addParameter("RMASMD_ID", oGuid.ToString, OracleType.VarChar)          '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASMD_RMANO", dr.RMASMD_RMANO, OracleType.VarChar)
                    oExecute.addParameter("RMASMD_RMADID", dr.RMASMD_RMADID, OracleType.VarChar)

                    If dr.IsRMASMD_MODELNONull = False Then oExecute.addParameter("RMASMD_MODELNO", dr.RMASMD_MODELNO, OracleType.VarChar)
                    If dr.IsRMASMD_SERIALNONull = False Then oExecute.addParameter("RMASMD_SERIALNO", dr.RMASMD_SERIALNO, OracleType.VarChar)
                    If dr.IsRMASMD_PARTNONull = False Then oExecute.addParameter("RMASMD_PARTNO", dr.RMASMD_PARTNO, OracleType.VarChar)

                    oExecute.addParameter("RMARSD_LABORCOST", dr.RMARSD_LABORCOST, OracleType.Double)
                    oExecute.addParameter("RMARSD_MATERIALCOST", dr.RMARSD_MATERIALCOST, OracleType.Double)
                    oExecute.addParameter("RMARSD_QUOTE", dr.RMARSD_QUOTE, OracleType.Double)
                    oExecute.addParameter("RMARSD_CURRENCYCODE", dr.RMARSD_CURRENCYCODE, OracleType.VarChar)
                    oExecute.addParameter("RMARSD_CURRENCYRATE", dr.RMARSD_CURRENCYRATE, OracleType.Double)

                    oExecute.addParameter("RMASMD_LOWESTDISCOUNT", dr.RMASMD_LOWESTDISCOUNT, OracleType.Double)

                    oExecute.addParameter("RMASMD_AD", dr.RMASMD_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_ADNAME", dr.RMASMD_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_CSTMP", dr.RMASMD_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("RMASMD_LUAD", dr.RMASMD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUADNAME", dr.RMASMD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUSTMP", dr.RMASMD_LUSTMP, OracleType.DateTime)

                    oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.AddNew)

                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub Edit_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim dr As RmaDTO.Shipment_DetailRow = dtShippingDetail.Rows(i)

                    oExecute.addParameter("RMARSD_LABORCOST", dr.RMARSD_LABORCOST, OracleType.Double)
                    oExecute.addParameter("RMARSD_MATERIALCOST", dr.RMARSD_MATERIALCOST, OracleType.Double)
                    oExecute.addParameter("RMARSD_QUOTE", dr.RMARSD_QUOTE, OracleType.Double)

                    oExecute.addParameter("RMASMD_LOWESTDISCOUNT", dr.RMASMD_LOWESTDISCOUNT, OracleType.Double)

                    oExecute.addParameter("RMASMD_LUAD", dr.RMASMD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUADNAME", dr.RMASMD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUSTMP", dr.RMASMD_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMASMD_ID", dr.RMASMD_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 刪除 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub Del_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim dr As RmaDTO.Shipment_DetailRow = dtShippingDetail.Rows(i)

                    oExecute.addWHERE("RMASMD_ID", dr.RMASMD_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.Delete)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改RMADetail的狀態
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub Edit_RMADetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0

            For i = 0 To dtShippingDetail.Rows.Count - 1
                Dim dr As RmaDTO.Shipment_DetailRow = dtShippingDetail.Rows(i)

                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '===========================================================================================================================================================================
                oExecute.addParameter("RMAD_STATUS", "70", OracleType.Int16)
                oExecute.addParameter("RMAD_LUAD", dr.RMASMD_AD, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUADNAME", dr.RMASMD_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("RMAD_ID", dr.RMASMD_RMADID, OracleType.VarChar)
                oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
            Next

        End Sub

        ''' <summary>
        ''' 檢核  RMASMD_RMADID 是否存在
        ''' </summary>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub chkIsExist(ByVal dtShippingDetail As RmaDTO.Shipment_DetailDataTable)
            Dim i As Integer = 0
            Dim sMessage As String = ""
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                For i = 0 To dtShippingDetail.Count - 1
                    Dim dr As RmaDTO.Shipment_DetailRow = dtShippingDetail.Rows(i)

                    oQuery.addWHERE("RMASMD_RMADID", ":RMASMD_RMADID", dr.RMASMD_RMADID.ToString().Trim(), OracleType.VarChar)

                    Dim sSQL As String = "SELECT RMASMD_RMADID FROM RMA_SHIPMENTDETAIL WHERE RMASMD_RMADID=:RMASMD_RMADID "

                    dt = oQuery.ExecuteDT(sSQL)
                    If dt.Rows.Count > 0 Then
                        If sMessage.Trim() <> "" Then
                            sMessage = sMessage & "<br>"
                        End If
                        sMessage = sMessage & dr.RMASMD_RMANO.ToString().Trim() & "  (" & dr.RMASMD_SERIALNO.ToString().Trim() & ")"
                    End If
                Next

                If sMessage.Trim() <> "" Then
                    sMessage = _oLanguage.getText("RMA", "197", ctlLanguage.eumType.Validator) & "<BR>" & sMessage
                    Throw New ArgumentException(sMessage)
                End If

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' 取得 PrimaryKey
        ''' </summary>
        ''' <value>回傳em_rfnbr</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property getPrimaryKey() As String
            Get
                Return _PrimaryKey
            End Get
        End Property
    End Class
#End Region

#Region "Class:Shipping:出貨確認單"
    Public Class Shipping
        Dim _oLanguage As New ctlLanguage
        Dim _PrimaryKey As String = ""

        ''' <summary>
        ''' 取得要 Shipping 的客戶, 只能挑選 維修中心 負責的客戶
        ''' </summary>
        ''' <param name="CompNo">客戶維修中心代碼</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>tmpCustomerBySaleDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByShipping_Customer(ByVal CompNo As String, ByVal CuName As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpCustomerBySaleDataTable
            Dim sCondition As String = ""
            Dim dtCustomer As New RmaDTO.tmpCustomerBySaleDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " CU_NO asc"
                End If
                OrderBY = " ORDER BY " & OrderBY


                '客戶名稱
                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '帳號狀態 (1:開啟 , 0:關閉)
                oQuery.addWHERE("CU_STATUS", ":CU_STATUS", 1, OracleType.Int16)
                sCondition = sCondition & " AND (CU_STATUS=:CU_STATUS)"

                '客戶維修中心點
                oQuery.addWHERE("CU_COMPNO", ":CU_COMPNO", CompNo, OracleType.VarChar)
                sCondition = sCondition & " AND (CU_COMPNO=:CU_COMPNO)"

                Dim sSQL As String = " SELECT CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID, "
                sSQL = sSQL & " CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 "
                sSQL = sSQL & " FROM CUSTOMER "
                sSQL = sSQL & " INNER JOIN COMPANY ON CUSTOMER.CU_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " INNER JOIN CURRENCY ON COMPANY.COMP_CURRENCYCODE = CURRENCY.CURRENCY_CODE "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENT ON RMASM_CUNO = CU_NO"
                sSQL = sSQL & " WHERE NOT EXISTS (SELECT RMASHD_ID FROM RMA_SHIPPINGDETAIL WHERE RMASHD_RMASMPACKINGNO = RMASM_PACKINGNO) "
                sSQL = sSQL & sCondition
                sSQL = sSQL & " GROUP BY CU_NO,CU_NAME,CU_SALESID,CU_ASSISTANTID,CURRENCY_CODE, CURRENCY_RATE, CU_ADDRESS1 " & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtCustomer)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtCustomer
        End Function

        ''' <summary>
        ''' 挑選要出貨的單子
        ''' </summary>
        ''' <param name="CuNo">客戶代碼</param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getQueryByShipment(ByVal CuNo As String, Optional ByVal OrderBY As String = "") As RmaDTO.Shipping_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipping_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection

            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_SHIPMENTDETAIL.RMASMD_RMANO desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMASM_CUNO", ":RMASM_CUNO", CuNo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_CUNO=:RMASM_CUNO"

                Dim sSQL As String = " SELECT RMA_SHIPMENT.RMASM_PACKINGNO, RMA_SHIPMENTDETAIL.RMASMD_RMANO, count(*) Qty "
                sSQL = sSQL & " FROM RMA_SHIPMENTDETAIL"
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENT ON RMA_SHIPMENTDETAIL.RMASMD_RMASMID = RMA_SHIPMENT.RMASM_ID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO "
                sSQL = sSQL & " AND RMA_SHIPMENT.RMASM_PACKINGNO = RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO "
                sSQL = sSQL & " WHERE RMA_SHIPPINGDETAIL.RMASHD_RMANO is  null and (RMASM_ISBOSSCONFIRM=0 OR RMASM_ISBOSSCONFIRM=2)"
                sSQL = sSQL & sCondition
                sSQL = sSQL & " GROUP BY RMA_SHIPMENT.RMASM_PACKINGNO,RMA_SHIPMENTDETAIL.RMASMD_RMANO"
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 挑選要跟出貨比對的單子
        ''' </summary>
        ''' <param name="CompNo">維修點</param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByShipment(ByVal CompNo As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpShipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.tmpShipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMASMD_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMASM_CUNO", ":RMASM_CUNO", CompNo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_CUNO=:RMASM_CUNO"

                Dim sSQL As String = " SELECT RMASM_PACKINGNO, RMASMD_RMANO, RMASM_SHIPNO, RMASM_ISSHIP, RMASMD_SERIALNO"
                sSQL = sSQL & " FROM RMA_SHIPMENTDETAIL "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENT ON RMA_SHIPMENTDETAIL.RMASMD_RMASMID = RMA_SHIPMENT.RMASM_ID "
                sSQL = sSQL & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 挑選已出貨的單子
        ''' </summary>
        ''' <param name="PACKINGNO"></param>
        ''' <param name="RMANO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getQueryByShipping(ByVal PACKINGNO As String, ByVal RMANO As String) As RmaDTO.Shipping_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.Shipping_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMASM_PACKINGNO", ":RMASM_PACKINGNO", PACKINGNO, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_PACKINGNO=:RMASM_PACKINGNO"

                oQuery.addWHERE("RMASMD_RMANO", ":RMASMD_RMANO", RMANO, OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMANO=:RMASMD_RMANO"

                Dim sSQL As String = " SELECT RMA_SHIPMENT.RMASM_PACKINGNO, RMA_SHIPMENTDETAIL.RMASMD_RMANO, count(*) Qty "
                sSQL = sSQL & " FROM RMA_SHIPMENTDETAIL"
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENT ON RMA_SHIPMENTDETAIL.RMASMD_RMASMID = RMA_SHIPMENT.RMASM_ID "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPMENTDETAIL.RMASMD_RMANO = RMA_SHIPPINGDETAIL.RMASHD_RMANO "
                sSQL = sSQL & " AND RMA_SHIPMENT.RMASM_PACKINGNO = RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO "
                sSQL = sSQL & " WHERE RMA_SHIPPINGDETAIL.RMASHD_RMANO is not null"
                sSQL = sSQL & sCondition
                sSQL = sSQL & " GROUP BY RMA_SHIPMENT.RMASM_PACKINGNO,RMA_SHIPMENTDETAIL.RMASMD_RMANO"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 由SHP單反查是否有全保
        ''' </summary>
        ''' <param name="RMASH_SHIPPINGNO"></param>
        ''' <returns></returns>
        Public Function chkShpISCWarranty(ByVal RMASH_SHIPPINGNO As String) As Boolean
            Dim retval As Boolean = False

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""

            oConn.Open()
            Try

                oQuery.addWHERE("RMASH_SHIPPINGNO", ":RMASH_SHIPPINGNO", RMASH_SHIPPINGNO, OracleType.VarChar)
                sCondition = sCondition & " AND RMASH_SHIPPINGNO =:RMASH_SHIPPINGNO"

                sSQL = "SELECT DISTINCT RMASH_SHIPPINGNO, " &
                       "                 RMASHD_RMANO," &
                       "                 RMASH_TRACKINGNO," &
                       "                 TO_CHAR (RMASH_CSTMP, 'yyyy/mm/dd') RMASH_CSTMP," &
                       "                 RMASMD_SERIALNO" &
                       "   FROM RMA_SHIPPING" &
                       "        INNER JOIN RMA_SHIPPINGDETAIL ON RMASH_ID = RMASHD_RMASHID" &
                       "        INNER JOIN RMA_SHIPMENT ON RMASM_PACKINGNO = RMASHD_SHIPMENTNO" &
                       "        INNER JOIN RMA_SHIPMENTDETAIL ON RMASM_ID = RMASMD_RMASMID" &
                       "        INNER JOIN RMADETAIL" &
                       "           ON RMAD_RMANO = RMASHD_RMANO AND RMASMD_SERIALNO = RMAD_SERIALNO" &
                       "  WHERE     RMAD_ISCW = 1" &
                       "        AND RMAD_MARK = 0 " & sCondition

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

        '''' <summary>
        '''' 取得 RMA_Shipping
        '''' </summary>
        '''' <param name="sShipmentID">sShipmentID</param>
        '''' <param name="sCuSalesID">sCuSalesID</param>
        '''' <param name="Notice">Notice</param>
        '''' <param name="Tracking">Tracking</param>
        '''' <param name="CuName">客戶名稱</param>
        '''' <param name="Serial">Serial</param>
        '''' <param name="fdate">開始日期</param>
        '''' <param name="edate">結束日期</param>
        '''' <param name="OrderBY">定義排序</param>
        '''' <returns>ShipmentDataTable</returns>
        '''' <remarks></remarks>
        'Public Function QueryByRMA_Shipping(ByVal sShippingID As String, ByVal Notice As String, ByVal Tracking As String, _
        'ByVal CuName As String, ByVal Serial As String, ByVal fdate As String, ByVal edate As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMA_ShippingDataTable
        '    Dim sCondition As String = ""
        '    Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
        '    Dim dt As New DataTable

        '    Dim oConn As New Connection
        '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        '    oConn.Open()
        '    Try
        '        If OrderBY.Trim = "" Then
        '            OrderBY = " RMASH_CSTMP desc"
        '        End If
        '        OrderBY = " ORDER BY " & OrderBY

        '        If sShippingID.ToString().Trim() <> "" Then
        '            oQuery.addWHERE("RMASH_ID", ":RMASH_ID", sShippingID, OracleType.VarChar)
        '            sCondition = sCondition & " AND RMASH_ID=:RMASH_ID"
        '        End If


        '        If Notice.ToString().Trim() <> "" Then
        '            oQuery.addWHERE("RMASHD_RMASMPACKINGNO", ":RMASHD_RMASMPACKINGNO", Notice, OracleType.VarChar)
        '            sCondition = sCondition & " AND RMASHD_RMASMPACKINGNO=:RMASHD_RMASMPACKINGNO"
        '        End If


        '        'If Tracking.ToString().Trim() <> "" Then
        '        '    oQuery.addWHERE("RMASM_PACKINGNO", ":RMASM_PACKINGNO", Tracking, OracleType.VarChar)
        '        '    sCondition = sCondition & " AND RMASM_PACKINGNO=:RMASM_PACKINGNO"
        '        'End If




        '        If CuName.ToString().Trim() <> "" Then
        '            CuName = "%" & CuName & "%"
        '            oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
        '            sCondition = sCondition & " AND CU_NAME like :CU_NAME"
        '        End If

        '        'If Serial.ToString().Trim() <> "" Then
        '        '    oQuery.addWHERE("RMASMD_SERIALNO", ":RMASMD_SERIALNO", Serial, OracleType.VarChar)
        '        '    sCondition = sCondition & " AND RMASMD_SERIALNO=:RMASMD_SERIALNO"
        '        'End If

        '        If fdate.Trim <> "" And edate.Trim <> "" Then
        '            Dim RMASH_CSTMP = Convert.ToDateTime(fdate)
        '            Dim RMASH_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

        '            oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP", RMASH_CSTMP, OracleType.DateTime)
        '            oQuery.addWHERE("RMASH_LUSTMP", ":RMASH_LUSTMP", RMASH_LUSTMP, OracleType.DateTime)

        '            sCondition = sCondition & " AND (RMASH_CSTMP >=:RMASH_CSTMP AND RMASH_LUSTMP <=:RMASH_LUSTMP)"
        '        End If

        '        Dim sSQL As String = " SELECT RMA_SHIPPING.*, RMASH_TRACKINGNO "
        '        sSQL = sSQL & " CU_NAME,vwRMA_SHIPPING.RMASHD_RMASMPACKINGNO,RMASM_CSTMP "
        '        sSQL = sSQL & " FROM RMA_SHIPPING "
        '        sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPPING.RMASH_CUNO = CUSTOMER.CU_NO "
        '        sSQL = sSQL & " INNER JOIN "
        '        sSQL = sSQL & " ( "
        '        sSQL = sSQL & " SELECT RMASHD_RMASMPACKINGNO,RMASH_ID "
        '        sSQL = sSQL & " FROM RMA_SHIPPING "
        '        sSQL = sSQL & " INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID "
        '        sSQL = sSQL & " GROUP BY RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO , RMA_SHIPPING.RMASH_ID "
        '        sSQL = sSQL & " ) vwRMA_SHIPPING "
        '        sSQL = sSQL & " ON RMA_SHIPPING.RMASH_ID = vwRMA_SHIPPING.RMASH_ID "
        '        sSQL = sSQL & " INNER JOIN RMA_SHIPMENT ON vwRMA_SHIPPING.RMASHD_RMASMPACKINGNO = RMA_SHIPMENT.RMASM_PACKINGNO "
        '        sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY

        '        dt = oQuery.ExecuteDT(sSQL)
        '        Common.TransferDataTable(dt, dtShipping)

        '    Catch ex As Exception
        '        Throw ex

        '    Finally
        '        oConn.Close()
        '        oConn.Dispose()
        '    End Try

        '    Return dtShipping
        'End Function

        ''' <summary>
        ''' 取得 RMA_Shipping
        ''' </summary>
        ''' <param name="sShippingID">sShippingID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>ShipmentDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_Shipping(ByVal sShippingID As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMA_ShippingDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMASH_ID", ":RMASH_ID", sShippingID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASH_ID=:RMASH_ID"

                Dim sSQL As String = " SELECT RMA_SHIPPING.*,CU_NAME "
                sSQL = sSQL & " FROM RMA_SHIPPING"
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPPING.RMASH_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 取得 RMA_Shipping
        ''' </summary>
        ''' <param name="CompNo">客戶維修中心代碼</param>
        ''' <param name="sShippingID"></param>
        ''' <param name="RMANO">RMA No</param>
        ''' <param name="Tracking">Tracking</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="Serial">Serial No</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_ShippingList(ByVal CompNo As String, ByVal sShippingID As String, ByVal RMANO As String, ByVal Tracking As String,
        ByVal CuName As String, ByVal Serial As String, ByVal fdate As String, ByVal edate As String, Optional ByVal OrderBY As String = "") As DataTable

            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.RMA_ShippingDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                'If OrderBY.Trim = "" Then
                '    OrderBY = " RMASH_CSTMP desc"
                'End If
                'OrderBY = " ORDER BY " & OrderBY

                '維修中心
                If CompNo.ToString().Trim() <> "" Then
                    Dim i As Integer = 0
                    Dim sCondition_Repair As String = ""
                    sCondition = sCondition & " AND ("
                    Dim arrRepair() As String = CompNo.Split(",")
                    For i = 0 To arrRepair.Length - 1
                        'oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                        If sCondition_Repair.Trim <> "" Then
                            sCondition_Repair = sCondition_Repair & " OR "
                        End If
                        'sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString
                        sCondition_Repair = sCondition_Repair & " RMA_COMPNO = '" & arrRepair(i).Trim() & "'"
                    Next
                    sCondition = sCondition & sCondition_Repair & ")"
                End If


                If sShippingID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMASH_ID", ":RMASH_ID", sShippingID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMASH_ID=:RMASH_ID"
                End If


                If RMANO.ToString().Trim() <> "" Then
                    RMANO = "%" & RMANO.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMASHD_RMANO", ":RMASHD_RMANO", RMANO, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMASHD_RMANO) like :RMASHD_RMANO"
                End If

                If Tracking.ToString().Trim() <> "" Then
                    Tracking = "%" & Tracking.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMASH_TRACKINGNO", ":RMASH_TRACKINGNO", Tracking, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMASH_TRACKINGNO) like :RMASH_TRACKINGNO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.ToLower().Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Serial.ToString().Trim() <> "" Then
                    Serial = "%" & Serial.ToLower().Trim() & "%"
                    oQuery.addWHERE("RMASMD_SERIALNO", ":RMASMD_SERIALNO", Serial, OracleType.VarChar)
                    oQuery.addWHERE("RMASMD_SERIALNO", ":RMAD_PARTSN", Serial, OracleType.VarChar)
                    sCondition = sCondition & " AND (lower(RMASMD_SERIALNO) like :RMASMD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_PARTSN)"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMASH_CSTMP As DateTime = Convert.ToDateTime(fdate)
                    Dim RMASH_LUSTMP As DateTime = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    'oQuery.addWHERE("RMASH_CSTMP", ":RMASH_CSTMP", RMASH_CSTMP, OracleType.DateTime)
                    'oQuery.addWHERE("RMASH_LUSTMP", ":RMASH_LUSTMP", RMASH_LUSTMP, OracleType.DateTime)

                    'sCondition = sCondition & " AND (RMASH_CSTMP >=:RMASH_CSTMP AND RMASH_LUSTMP <=:RMASH_LUSTMP)"
                    sCondition = sCondition & " AND (to_char(RMASH_CSTMP,'yyyy/mm/dd') >='" & RMASH_CSTMP.ToString("yyyy/MM/dd") & "' AND to_char(RMASH_LUSTMP,'yyyy/mm/dd') <='" & RMASH_LUSTMP.ToString("yyyy/MM/dd") & "')"
                End If

                sSQL = "SELECT "
                sSQL = sSQL & " nvl(RMASH_TRACKINGNO,'') TrackingNo, RMASH_ID, to_char(RMASHD_LUSTMP,'yyyy/mm/dd') RMASHD_LUSTMP, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate, "
                sSQL = sSQL & " CU_NAME, RMASHD_RMANO, RMASM_CSTMP NoticeDate, RMA_COMPNO, "
                sSQL = sSQL & " CASE  "
                sSQL = sSQL & "     WHEN RMAD_STATUS <= 60 AND RMASH_ISSUBMIT = 0  THEN 1"
                sSQL = sSQL & "     WHEN RMAD_STATUS = 91 AND RMAD_RECEVSTATUS =1 AND RMASH_ISSUBMIT = 0  THEN 1"
                sSQL = sSQL & "     WHEN RMAD_STATUS = 91 AND RMAD_RECEVSTATUS =1 AND RMASH_ISSUBMIT = 1  THEN 0  "
                sSQL = sSQL & "     WHEN RMAD_STATUS = 90  THEN 0 "
                sSQL = sSQL & " ELSE -1  END as isEdit"

                sSQL = sSQL & " FROM RMA_SHIPPING "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPPING.RMASH_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT RMASMD_RMANO, RMASM_PACKINGNO, to_char(RMASM_CSTMP,'yyyy/mm/dd') as RMASM_CSTMP, RMASMD_SERIALNO, "
                sSQL = sSQL & " RMAD_STATUS, RMAD_RECEVSTATUS, RMAD_PARTSN "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " INNER JOIN RMADETAIL ON  RMA_SHIPMENTDETAIL.RMASMD_RMADID = RMADETAIL.RMAD_ID "
                sSQL = sSQL & " ) vwSHIPMENT "
                sSQL = sSQL & " ON RMA_SHIPPINGDETAIL.RMASHD_RMANO = vwSHIPMENT.RMASMD_RMANO "
                sSQL = sSQL & " AND RMA_SHIPPINGDETAIL.RMASHD_RMASMPACKINGNO = vwSHIPMENT.RMASM_PACKINGNO "
                sSQL = sSQL & " INNER JOIN RMA ON RMA.rma_no = RMA_SHIPPINGDETAIL.RMASHD_RMANO"
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY
                sSQL = sSQL & " GROUP BY RMASH_TRACKINGNO, RMASH_ID,to_char(RMASHD_LUSTMP,'yyyy/mm/dd'), to_char(RMASH_CSTMP,'yyyy/mm/dd'),CU_NAME,RMASHD_RMANO, RMASM_CSTMP, RMA_COMPNO"

                sSQL = sSQL & " ,CASE  "
                sSQL = sSQL & "     WHEN RMAD_STATUS <= 60 AND RMASH_ISSUBMIT = 0  THEN 1"
                sSQL = sSQL & "     WHEN RMAD_STATUS = 91 AND RMAD_RECEVSTATUS =1 AND RMASH_ISSUBMIT = 0  THEN 1"
                sSQL = sSQL & "     WHEN RMAD_STATUS = 91 AND RMAD_RECEVSTATUS =1 AND RMASH_ISSUBMIT = 1  THEN 0  "
                sSQL = sSQL & "     WHEN RMAD_STATUS = 90  THEN 0 "
                sSQL = sSQL & " ELSE -1 END"

                dt = oQuery.ExecuteDT(sSQL)
                'Common.TransferData(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' 取得 RMA_ShippingDetail
        ''' </summary>
        ''' <param name="sShipmentID"></param>
        ''' <param name="OrderBY"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMA_ShippingDetail(ByVal sShipmentID As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpShipping_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipping As New RmaDTO.tmpShipping_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMASHD_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMASHD_RMASHID", ":RMASHD_RMASHID", sShipmentID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASHD_RMASHID=:RMASHD_RMASHID"

                Dim sSQL As String = " SELECT a.*, '1' as RMASHD_oldMark,b.RMASM_ID,c.RMASMD_RMADID FROM RMA_SHIPPINGDETAIL a,RMA_SHIPMENT b,RMA_SHIPMENTDETAIL c "
                sSQL = sSQL & " WHERE a.RMASHD_SHIPMENTNO=b.RMASM_PACKINGNO AND b.RMASM_ID=c.RMASMD_RMASMID" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipping)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipping
        End Function

        ''' <summary>
        ''' 取得 RMAD_ID 給 Shipping
        ''' </summary>
        ''' <param name="PackingNo">RMA Shipment 編號</param>
        ''' <param name="RMANo">關聯 RMA.RMA_No-->RMA 編號</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMADID_Shipping(ByVal oConn As ICAT_OracleDAO.Connection, ByVal PackingNo As String, ByVal RMANo As String) As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            Try

                oQuery.addWHERE("RMASM_PACKINGNO", ":RMASM_PACKINGNO", PackingNo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_PACKINGNO=:RMASM_PACKINGNO"

                oQuery.addWHERE("RMASMD_RMANO", ":RMASMD_RMANO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMANO=:RMASMD_RMANO"

                Dim sSQL As String = " SELECT RMA_SHIPMENTDETAIL.* "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipment)

            Catch ex As Exception
                Throw ex

            Finally
            End Try

            Return dtShipment
        End Function

        ''' <summary>
        ''' 取得 RMAD_ID
        ''' </summary>
        ''' <param name="PackingNo">RMA Shipment 編號</param>
        ''' <param name="RMANo">關聯 RMA.RMA_No-->RMA 編號</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMADID(ByVal PackingNo As String, ByVal RMANo As String) As RmaDTO.Shipment_DetailDataTable
            Dim sCondition As String = ""
            Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMASM_PACKINGNO", ":RMASM_PACKINGNO", PackingNo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASM_PACKINGNO=:RMASM_PACKINGNO"

                oQuery.addWHERE("RMASMD_RMANO", ":RMASMD_RMANO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMASMD_RMANO=:RMASMD_RMANO"

                Dim sSQL As String = " SELECT RMA_SHIPMENTDETAIL.* "
                sSQL = sSQL & " FROM RMA_SHIPMENT "
                sSQL = sSQL & " INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPMENT.RMASM_ID = RMA_SHIPMENTDETAIL.RMASMD_RMASMID "
                sSQL = sSQL & " WHERE 1=1" & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShipment)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShipment
        End Function

        ''' <summary>
        ''' 判斷RMADETAIL單的狀態 給 Shipping 用的
        ''' </summary>
        ''' <param name="RMANo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMANO_Shipping(ByVal oConn As ICAT_OracleDAO.Connection, ByVal RMANo As String) As Boolean
            Dim blnFlag As Boolean = False
            Dim sCondition As String = ""
            Dim dt As New DataTable

            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            Try

                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO=:RMAD_RMANO"

                Dim sSQL As String = " SELECT RMADETAIL.*"
                sSQL = sSQL & " FROM RMADETAIL"
                sSQL = sSQL & " WHERE RMAD_STATUS < '90' AND RMAD_MARK = 0" & sCondition

                dt = oQuery.ExecuteDT(sSQL)

                If Not dt.Rows.Count > 0 Then
                    blnFlag = True
                End If

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return blnFlag
        End Function

        ''' <summary>
        ''' 判斷RMADETAIL單的狀態
        ''' </summary>
        ''' <param name="RMANo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByRMANO(ByVal RMANo As String) As Boolean
            Dim blnFlag As Boolean = False
            Dim sCondition As String = ""
            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO=:RMAD_RMANO"

                Dim sSQL As String = " SELECT RMADETAIL.*"
                sSQL = sSQL & " FROM RMADETAIL"
                sSQL = sSQL & " WHERE RMAD_STATUS < '90'" & sCondition

                dt = oQuery.ExecuteDT(sSQL)

                If Not dt.Rows.Count > 0 Then
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
        ''' 列印ShippingReport
        ''' </summary>
        ''' <param name="RMASHID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryByReport(ByVal RMASHID As String) As RmaDTO.ShippingReportDataTable
            Dim sCondition As String = ""
            Dim dtShippingReport As New RmaDTO.ShippingReportDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oQuery.addWHERE("RMASHD_RMASHID", ":RMASHD_RMASHID", RMASHID, OracleType.VarChar)
                sCondition = sCondition & " AND RMASHD_RMASHID=:RMASHD_RMASHID"

                Dim sSQL As String = " SELECT "
                sSQL = sSQL & " RMASH_SHIPPINGNO, RMASH_PACKINGLIST, RMASH_FROM, CU_NAME, CU_TEL, RMASH_ADDRESS, RMASH_CSTMP, RMASH_LUADNAME, "
                sSQL = sSQL & " RMASHD_RMASHID, RMASHD_CTNNO, RMASHD_DESCRIPTION, RMASHD_QUANTITY, RMASHD_NETWEIGHT, RMASHD_GROSSWEIGH, RMASHD_MEASUREMENT "
                sSQL = sSQL & " FROM RMA_SHIPPING "
                sSQL = sSQL & " INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_SHIPPING.RMASH_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtShippingReport)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtShippingReport
        End Function

        ''' <summary>
        ''' Call SP -->SP_SHP_INS_AR(傳入SHP單號,幣別,執行人員代號)
        ''' </summary>
        ''' <param name="CustNo"></param>
        ''' <param name="RMASH_SHIPPINGNO"></param>
        ''' <param name="CurrencyCode"></param>
        ''' <param name="sUserAD"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function runSP_SHP_INS_AR(ByVal CustNo As String, ByVal RMASH_SHIPPINGNO As String, ByVal CurrencyCode As String,
            ByVal sUserAD As String, ByVal sRepairCenter As String) As String

            Dim retval As String = ""

            Dim oConn As New Connection
            'Dim oExecute As New Execute(oConn)
            'Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '=============================================================================================================================================================================================================================================================
                'Call SP -->SP_SHP_INS_AR(傳入SHP單號,幣別,執行人員代號)
                '=============================================================================================================================================================================================================================================================
                Dim oCommand As OracleCommand = oConn.Command()
                'oCommand.CommandText = "SP_SHP_INS_AR"
                'oCommand.CommandType = System.Data.CommandType.StoredProcedure
                'oCommand.Parameters.Add("vCUSTNO", OracleType.NVarChar).Value = CustNo
                'oCommand.Parameters.Add("vSHPNO", OracleType.NVarChar).Value = RMASH_SHIPPINGNO
                'oCommand.Parameters.Add("vCurr", OracleType.NVarChar).Value = CurrencyCode
                'oCommand.Parameters.Add("vUserNo", OracleType.NVarChar).Value = sUserAD
                'oCommand.Parameters.Add("vCompno", OracleType.NVarChar).Value = sRepairCenter
                'oCommand.ExecuteNonQuery()

                oCommand.CommandText = "SP_SHP_INS_AR"
                oCommand.CommandType = System.Data.CommandType.StoredProcedure

                oCommand.Parameters.Add("vCUSTNO", OracleType.NVarChar).Value = CustNo
                oCommand.Parameters("vCUSTNO").Direction = ParameterDirection.Input

                oCommand.Parameters.Add("vSHPNO", OracleType.NVarChar).Value = RMASH_SHIPPINGNO
                oCommand.Parameters("vSHPNO").Direction = ParameterDirection.Input

                oCommand.Parameters.Add("vCurr", OracleType.NVarChar).Value = CurrencyCode
                oCommand.Parameters("vCurr").Direction = ParameterDirection.Input

                oCommand.Parameters.Add("vUserNo", OracleType.NVarChar).Value = sUserAD
                oCommand.Parameters("vUserNo").Direction = ParameterDirection.Input

                oCommand.Parameters.Add("vCompno", OracleType.NVarChar).Value = sRepairCenter
                oCommand.Parameters("vCompno").Direction = ParameterDirection.Input

                oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
                oCommand.Parameters("vResult").Direction = ParameterDirection.Output

                oCommand.ExecuteNonQuery()

                retval = oCommand.Parameters("vResult").Value

                oCommand.Parameters.Clear()
                oCommand.CommandText = ""
                oCommand.CommandType = CommandType.Text

                oConn.Commit()

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 存檔-新增
        ''' </summary>
        ''' <param name="sType">Save:不需改RMA狀態,Submit:改RMA狀態</param>
        ''' <param name="dtShipping">RMA_ShippingDataTable</param>
        ''' <param name="dtShippingDetail">tmpShipping_DetailDataTable</param>
        ''' <param name="isSumbit">判斷是否確認送出</param>
        ''' <remarks></remarks>
        Public Function SaveByAddNew(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sType As String, ByVal dtShipping As RmaDTO.RMA_ShippingDataTable,
            ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal isSumbit As Boolean,
            ByVal CustNo As String, ByRef retvalSHIPPINGNO As String, ByVal CurrencyCode As String, ByVal sUserAD As String) As String

            Dim i As Integer = 0
            Dim j As Integer = 0
            'Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim RMASH_ID As String = ""

            'oConn.Open()
            Try
                'oConn.BeginTransaction()

                RMASH_ID = AddNew_RMAShipping(oExecute, dtShipping, isSumbit, retvalSHIPPINGNO)

                Call AddNew_RMAShippingDetail(oExecute, RMASH_ID, dtShippingDetail, True)

                '================================================================================================================================
                '修改RMA及RMADetail的狀態(Closed)
                '================================================================================================================================
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    Call Edit_RMADetail(oExecute, dtShippingDetail)
                End If

                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    '===========================================================================================================================================================================
                    'UPDATE  RMA_STATUS = Closed
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    '===========================================================================================================================================================================
                    Call EditByRMA_Shipping(oConn, dtShippingDetail)
                End If

                'oConn.Commit()


            Catch ex As Exception
                'oConn.Rollback()
                Throw ex

            Finally
                'oConn.Close()
                'oConn.Dispose()
            End Try

            Return RMASH_ID
        End Function

        ''' <summary>
        ''' 存檔-修改
        ''' </summary>
        ''' <param name="sType">Save:不需改RMA狀態,Submit:改RMA狀態</param>
        ''' <param name="sShipmentID">KEY</param>
        ''' <param name="dtShipping">ShipmentDataTable</param>
        ''' <param name="dtShippingDetail">Shipment_DetailDataTable</param>
        ''' <param name="isSumbit">判斷是否確認送出</param>
        ''' <remarks></remarks>
        Public Sub SaveByEdit(ByVal oConn As ICAT_OracleDAO.Connection, ByVal sType As String, ByVal sShipmentID As String, ByVal dtShipping As RmaDTO.RMA_ShippingDataTable,
            ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal isSumbit As Boolean,
            ByVal CustNo As String, ByVal RMASH_SHIPPINGNO As String, ByVal CurrencyCode As String, ByVal sUserAD As String)

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim RMASH_ID As String = ""

            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            Dim dtShippingDetail_Add As New RmaDTO.tmpShipping_DetailDataTable
            Dim dtShippingDetail_Edit As New RmaDTO.tmpShipping_DetailDataTable
            Dim dtShippingDetail_Del As New RmaDTO.tmpShipping_DetailDataTable

            Try
                'RMASMD_oldMark==>0:新增,1:修改,2:刪除
                dtShippingDetail_Add = dtShippingDetail.Copy
                dtShippingDetail_Edit = dtShippingDetail.Copy
                dtShippingDetail_Del = dtShippingDetail.Copy

                '新增
                Dim dvShippingDetail_Add As DataView = dtShippingDetail_Add.DefaultView()
                dvShippingDetail_Add.RowFilter = "RMASHD_oldMark<>'0'"
                Do While dvShippingDetail_Add.Count > 0
                    dvShippingDetail_Add.Delete(0)
                Loop

                '修改
                Dim dvShippingDetail_Edit As DataView = dtShippingDetail_Edit.DefaultView()
                dvShippingDetail_Edit.RowFilter = "RMASHD_oldMark<>'1'"
                Do While dvShippingDetail_Edit.Count > 0
                    dvShippingDetail_Edit.Delete(0)
                Loop

                '刪除
                Dim dvShippingDetail_Del As DataView = dtShippingDetail_Del.DefaultView()
                dvShippingDetail_Del.RowFilter = "RMASHD_oldMark<>'2'"
                Do While dvShippingDetail_Del.Count > 0
                    dvShippingDetail_Del.Delete(0)
                Loop


                '========================================================================================================================================================================================================================================================================================
                '2011/08/04 START
                '原Code
                '========================================================================================================================================================================================================================================================================================
                'Call Edit_RMAShipping(oExecute, dtShipping, isSumbit)
                'Call AddNew_RMAShippingDetail(oExecute, sShipmentID, dtShippingDetail_Add)
                'Call Edit_RMAShippingDetail(oExecute, dtShippingDetail_Edit)
                'Call Del_RMAShippingDetail(oExecute, dtShippingDetail_Del)


                '========================================================================================================================================================================================================================================================================================
                '改成
                '========================================================================================================================================================================================================================================================================================
                If dtShipping.Rows.Count > 0 Then
                    RMASH_ID = dtShipping.Rows(0)("RMASH_ID").ToString().Trim()

                    Call Edit_RMAShipping(oExecute, dtShipping, isSumbit)
                    Call Del_RMAShippingDetail(oExecute, RMASH_ID)
                    Call AddNew_RMAShippingDetail(oExecute, RMASH_ID, dtShippingDetail, False)
                End If
                '========================================================================================================================================================================================================================================================================================
                '2011/08/04 END
                '========================================================================================================================================================================================================================================================================================

                '================================================================================================================================
                '修改RMA及RMADetail的狀態(Closed)
                '================================================================================================================================
                If sType.ToLower().Trim() = "Submit".ToLower().Trim() Then
                    Call Edit_RMADetail(oExecute, dtShippingDetail_Add)
                    Call Edit_RMADetail(oExecute, dtShippingDetail_Edit)
                    Call EditByRMA(oConn, dtShippingDetail)    'MaggieChen
                End If

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally


            End Try

        End Sub

        ''' <summary>
        ''' 新增 RMA_Shipping 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShipping">RMA_ShippingDataTable</param>
        ''' <param name="isSumbit">判斷是否確認送出</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AddNew_RMAShipping(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShipping As RmaDTO.RMA_ShippingDataTable, ByVal isSumbit As Boolean, ByRef retvalSHIPPINGNO As String) As String
            Dim i As Integer = 0
            Dim retval As String = ""

            Dim oGuid As Guid = Guid.NewGuid
            Dim oBill As New AutoDocumentNo.Bill

            Try
                Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.Rows(0)
                Dim sGUID As String = oGuid.ToString

                Dim RMASH_SHIPPINGNO As String = oBill.GetBillNo(oExecute.Connection, "SHP", dr.RMASH_AD, dr.RMASH_ADNAME)

                retvalSHIPPINGNO = RMASH_SHIPPINGNO
                oExecute.addParameter("RMASH_SHIPPINGNO", RMASH_SHIPPINGNO, OracleType.VarChar)
                oExecute.addParameter("RMASH_ID", sGUID, OracleType.VarChar)                '系統自動產生唯一識別碼

                If dr.IsRMASH_SHIPPINGBYNull = False Then oExecute.addParameter("RMASH_SHIPPINGBY", dr.RMASH_SHIPPINGBY, OracleType.NVarChar)
                If dr.IsRMASH_ADDRESSNull = False Then oExecute.addParameter("RMASH_ADDRESS", dr.RMASH_ADDRESS, OracleType.NVarChar)
                If dr.IsRMASH_EXPRESSCONull = False Then oExecute.addParameter("RMASH_EXPRESSCO", dr.RMASH_EXPRESSCO, OracleType.NVarChar)
                If dr.IsRMASH_MEMONull = False Then oExecute.addParameter("RMASH_MEMO", dr.RMASH_MEMO, OracleType.NVarChar)

                oExecute.addParameter("RMASH_PACKINGLIST", dr.RMASH_PACKINGLIST, OracleType.NVarChar)
                oExecute.addParameter("RMASH_FROM", dr.RMASH_FROM, OracleType.NVarChar)
                oExecute.addParameter("RMASH_CUNO", dr.RMASH_CUNO, OracleType.VarChar)
                oExecute.addParameter("RMASH_TRACKINGNO", dr.RMASH_TRACKINGNO, OracleType.VarChar)
                oExecute.addParameter("RMASH_AD", dr.RMASH_AD, OracleType.NVarChar)
                oExecute.addParameter("RMASH_ADNAME", dr.RMASH_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASH_CSTMP", dr.RMASH_CSTMP, OracleType.DateTime)
                oExecute.addParameter("RMASH_LUAD", dr.RMASH_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASH_LUADNAME", dr.RMASH_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASH_LUSTMP", dr.RMASH_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("RMASH_COMPNO", dr.RMASH_COMPNO, OracleType.VarChar)

                If isSumbit = True Then
                    oExecute.addParameter("RMASH_ISSUBMIT", 1, OracleType.Int16)
                Else
                    oExecute.addParameter("RMASH_ISSUBMIT", 0, OracleType.Int16)
                End If

                oExecute.Command("RMA_SHIPPING", Execute.eumCommandType.AddNew)
                retval = sGUID

            Catch ex As Exception
                Throw ex

            Finally

            End Try

            Return retval
        End Function

        ''' <summary>
        ''' 修改 RMA_Shipping 單頭
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShipping">RMA_ShippingDataTable</param>
        ''' <param name="isSumbit">判斷是否確認送出</param>
        ''' <remarks></remarks>
        Private Sub Edit_RMAShipping(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShipping As RmaDTO.RMA_ShippingDataTable, ByVal isSumbit As Boolean)

            Try
                Dim dr As RmaDTO.RMA_ShippingRow = dtShipping.Rows(0)

                oExecute.addParameter("RMASH_PACKINGLIST", dr.RMASH_PACKINGLIST, OracleType.NVarChar)
                oExecute.addParameter("RMASH_TRACKINGNO", dr.RMASH_TRACKINGNO, OracleType.VarChar)
                oExecute.addParameter("RMASH_LUAD", dr.RMASH_LUAD, OracleType.NVarChar)
                oExecute.addParameter("RMASH_LUADNAME", dr.RMASH_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("RMASH_LUSTMP", dr.RMASH_LUSTMP, OracleType.DateTime)

                If dr.IsRMASH_ADDRESSNull = False Then oExecute.addParameter("RMASH_ADDRESS", dr.RMASH_ADDRESS, OracleType.NVarChar)
                If dr.IsRMASH_EXPRESSCONull = False Then oExecute.addParameter("RMASH_EXPRESSCO", dr.RMASH_EXPRESSCO, OracleType.NVarChar)
                If dr.IsRMASH_MEMONull = False Then oExecute.addParameter("RMASH_MEMO", dr.RMASH_MEMO, OracleType.NVarChar)

                If isSumbit = True Then
                    oExecute.addParameter("RMASH_ISSUBMIT", 1, OracleType.Int16)
                End If

                oExecute.addWHERE("RMASH_ID", dr.RMASH_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("RMA_SHIPPING", Execute.eumCommandType.UPDATE)

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改RMA狀態(Closed)
        ''' </summary>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub EditByRMA(ByVal oConn As ICAT_OracleDAO.Connection, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0
            Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            Try
                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim drShippingDetail As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)
                    Dim RMANo As String = drShippingDetail.RMASHD_RMANO.ToString().Trim()

                    '===========================================================================================================================================================================
                    'UPDATE  RMA_STATUS = Closed
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    '===========================================================================================================================================================================
                    Dim blnFlag As Boolean = False

                    'blnFlag = QueryByRMANO(RMANo)
                    blnFlag = QueryByRMANO_Shipping(oConn, RMANo)
                    If blnFlag = True Then
                        oExecute.addParameter("RMA_STATUS", "90", OracleType.Int16)

                        oExecute.addParameter("RMA_LUAD", drShippingDetail.RMASHD_AD, OracleType.NVarChar)
                        oExecute.addParameter("RMA_LUADNAME", drShippingDetail.RMASHD_ADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)

                        oExecute.addWHERE("RMA_NO", RMANo.Trim(), OracleType.VarChar)
                        oExecute.Command("RMA", Execute.eumCommandType.UPDATE)
                    End If
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try
        End Sub

        ''' <summary>
        ''' 修改RMA狀態(Closed) 給 Shpping 用的
        ''' </summary>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub EditByRMA_Shipping(ByVal oConn As ICAT_OracleDAO.Connection, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0
            Dim dtShipment As New RmaDTO.Shipment_DetailDataTable
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            Try
                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim drShippingDetail As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)
                    Dim RMANo As String = drShippingDetail.RMASHD_RMANO.ToString().Trim()

                    '===========================================================================================================================================================================
                    'UPDATE  RMA_STATUS = Closed
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    '===========================================================================================================================================================================
                    Dim blnFlag As Boolean = False

                    blnFlag = QueryByRMANO_Shipping(oConn, RMANo)
                    If blnFlag = True Then
                        oExecute.addParameter("RMA_STATUS", "90", OracleType.Int16)

                        oExecute.addParameter("RMA_LUAD", drShippingDetail.RMASHD_AD, OracleType.NVarChar)
                        oExecute.addParameter("RMA_LUADNAME", drShippingDetail.RMASHD_ADNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)

                        oExecute.addWHERE("RMA_NO", RMANo.Trim(), OracleType.VarChar)
                        oExecute.Command("RMA", Execute.eumCommandType.UPDATE)
                    End If
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 新增 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="ShipmentID">關聯 RMA_SHIPPING.RMASHD_RMASHID-->關聯 RMA_SHIPPING.RMASH_ID</param>
        ''' <param name="dtShippingDetail">tmpShipping_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub AddNew_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal ShipmentID As String, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable, ByVal isAdd As Boolean)
            Dim i As Integer = 0
            Dim dvShippingDetail As DataView = dtShippingDetail.DefaultView

            Try
                dvShippingDetail.RowFilter = "RMASHD_oldMark=0"

                '檢核 RMASHD_RMANO及RMASHD_RMASMPACKINGNO 是否存在
                If isAdd = True Then
                    Call chkIsExist(dtShippingDetail)
                End If

                For i = 0 To dvShippingDetail.Count - 1
                    Dim dr As RmaDTO.tmpShipping_DetailRow = dvShippingDetail(i).Row

                    Dim oGuid As Guid = Guid.NewGuid

                    oExecute.addParameter("RMASHD_RMASHID", ShipmentID, OracleType.VarChar)         'RMA Shipment 編號
                    oExecute.addParameter("RMASHD_ID", oGuid.ToString, OracleType.VarChar)          '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASHD_SHIPMENTNO", dr.RMASHD_SHIPMENTNO, OracleType.VarChar)

                    If dr.IsRMASHD_CTNNONull = False Then oExecute.addParameter("RMASHD_CTNNO", dr.RMASHD_CTNNO, OracleType.VarChar)
                    If dr.IsRMASHD_DESCRIPTIONNull = False Then oExecute.addParameter("RMASHD_DESCRIPTION", dr.RMASHD_DESCRIPTION, OracleType.NVarChar)
                    If dr.IsRMASHD_QUANTITYNull = False Then oExecute.addParameter("RMASHD_QUANTITY", dr.RMASHD_QUANTITY, OracleType.Double)
                    If dr.IsRMASHD_NETWEIGHTNull = False Then oExecute.addParameter("RMASHD_NETWEIGHT", dr.RMASHD_NETWEIGHT, OracleType.Double)
                    If dr.IsRMASHD_GROSSWEIGHNull = False Then oExecute.addParameter("RMASHD_GROSSWEIGH", dr.RMASHD_GROSSWEIGH, OracleType.NVarChar)
                    If dr.IsRMASHD_MEASUREMENTNull = False Then oExecute.addParameter("RMASHD_MEASUREMENT", dr.RMASHD_MEASUREMENT, OracleType.NVarChar)

                    oExecute.addParameter("RMASHD_AD", dr.RMASHD_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_ADNAME", dr.RMASHD_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_CSTMP", dr.RMASHD_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("RMASHD_LUAD", dr.RMASHD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUADNAME", dr.RMASHD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUSTMP", dr.RMASHD_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("RMASHD_RMANO", dr.RMASHD_RMANO, OracleType.VarChar)
                    oExecute.addParameter("RMASHD_RMASMPACKINGNO", dr.RMASHD_RMASMPACKINGNO, OracleType.VarChar)

                    oExecute.Command("RMA_SHIPPINGDETAIL", Execute.eumCommandType.AddNew)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShippingDetail">tmpShipping_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub Edit_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim dr As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)

                    If dr.IsRMASHD_CTNNONull = False Then oExecute.addParameter("RMASHD_CTNNO", dr.RMASHD_CTNNO, OracleType.VarChar)
                    If dr.IsRMASHD_DESCRIPTIONNull = False Then oExecute.addParameter("RMASHD_DESCRIPTION", dr.RMASHD_DESCRIPTION, OracleType.NVarChar)
                    If dr.IsRMASHD_QUANTITYNull = False Then oExecute.addParameter("RMASHD_QUANTITY", dr.RMASHD_QUANTITY, OracleType.Double)
                    If dr.IsRMASHD_NETWEIGHTNull = False Then oExecute.addParameter("RMASHD_NETWEIGHT", dr.RMASHD_NETWEIGHT, OracleType.Double)
                    If dr.IsRMASHD_GROSSWEIGHNull = False Then oExecute.addParameter("RMASHD_GROSSWEIGH", dr.RMASHD_GROSSWEIGH, OracleType.NVarChar)
                    If dr.IsRMASHD_MEASUREMENTNull = False Then oExecute.addParameter("RMASHD_MEASUREMENT", dr.RMASHD_MEASUREMENT, OracleType.NVarChar)

                    oExecute.addParameter("RMASHD_SHIPMENTNO", dr.RMASHD_SHIPMENTNO, OracleType.VarChar)
                    oExecute.addParameter("RMASHD_LUAD", dr.RMASHD_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUADNAME", dr.RMASHD_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUSTMP", dr.RMASHD_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("RMASHD_ID", dr.RMASHD_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMA_SHIPPINGDETAIL", Execute.eumCommandType.UPDATE)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 刪除 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="dtShippingDetail">tmpShipping_DetailDataTable</param>
        ''' <remarks></remarks>
        Private Sub Del_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0

            Try

                For i = 0 To dtShippingDetail.Rows.Count - 1
                    Dim dr As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)

                    oExecute.addWHERE("RMASHD_ID", dr.RMASHD_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("RMA_SHIPPINGDETAIL", Execute.eumCommandType.Delete)
                Next

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 刪除 RMA_ShippingDetail 出貨項
        ''' </summary>
        ''' <param name="oExecute">ICAT_OracleDAO.Execute</param>
        ''' <param name="RMASHD_ID">RMASHD_ID</param>
        ''' <remarks></remarks>
        Private Sub Del_RMAShippingDetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal RMASHD_ID As String)
            Dim i As Integer = 0

            Try
                oExecute.addWHERE("RMASHD_RMASHID", RMASHD_ID, OracleType.VarChar)
                oExecute.Command("RMA_SHIPPINGDETAIL", Execute.eumCommandType.Delete)

            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Sub

        ''' <summary>
        ''' 修改RMADetail的狀態
        ''' </summary>
        ''' <param name="oExecute"></param>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub Edit_RMADetail(ByVal oExecute As ICAT_OracleDAO.Execute, ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim dtShipment As New RmaDTO.Shipment_DetailDataTable

            For i = 0 To dtShippingDetail.Rows.Count - 1
                Dim drShippingDetail As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)
                Dim PackingNo As String = drShippingDetail.RMASHD_RMASMPACKINGNO.ToString().Trim()
                Dim RMANo As String = drShippingDetail.RMASHD_RMANO.ToString().Trim()

                '取RMAD_ID修改RMA單身的狀態
                dtShipment = QueryByRMADID_Shipping(oExecute.Connection, PackingNo, RMANo)
                For j = 0 To dtShipment.Rows.Count - 1
                    Dim drShipment As RmaDTO.Shipment_DetailRow = dtShipment.Rows(j)
                    Dim RMAD_ID As String = drShipment.RMASMD_RMADID.ToString().Trim()    '取RMAD_ID

                    '===========================================================================================================================================================================
                    'UPDATE  RMAD_STATUS = Closed
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    '===========================================================================================================================================================================
                    oExecute.addParameter("RMAD_STATUS", "90", OracleType.Int16)
                    oExecute.addParameter("RMAD_LUAD", drShippingDetail.RMASHD_AD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", drShippingDetail.RMASHD_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("RMAD_ID", RMAD_ID, OracleType.VarChar)
                    oExecute.addWHERE("RMAD_STATUS", "60", OracleType.VarChar)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                Next
            Next

        End Sub

        ''' <summary>
        ''' 檢核 RMASHD_RMANO及RMASHD_RMASMPACKINGNO 是否存在
        ''' </summary>
        ''' <param name="dtShippingDetail"></param>
        ''' <remarks></remarks>
        Public Sub chkIsExist(ByVal dtShippingDetail As RmaDTO.tmpShipping_DetailDataTable)
            Dim i As Integer = 0
            Dim sMessage As String = ""
            Dim sCondition As String = ""

            Dim dt As New DataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                For i = 0 To dtShippingDetail.Count - 1
                    Dim dr As RmaDTO.tmpShipping_DetailRow = dtShippingDetail.Rows(i)

                    oQuery.addWHERE("RMASHD_RMANO", ":RMASHD_RMANO", dr.RMASHD_RMANO.ToString().Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMASHD_RMASMPACKINGNO", ":RMASHD_RMASMPACKINGNO", dr.RMASHD_RMASMPACKINGNO.ToString().Trim(), OracleType.VarChar)

                    Dim sSQL As String = "SELECT RMASHD_RMANO,RMASHD_RMASMPACKINGNO FROM RMA_SHIPPINGDETAIL "
                    sSQL = sSQL & " WHERE RMASHD_RMANO=:RMASHD_RMANO AND RMASHD_RMASMPACKINGNO=:RMASHD_RMASMPACKINGNO"

                    dt = oQuery.ExecuteDT(sSQL)
                    If dt.Rows.Count > 0 Then
                        If sMessage.Trim() <> "" Then
                            sMessage = sMessage & "<br>"
                        End If
                        sMessage = sMessage & dr.RMASHD_RMANO.ToString().Trim() & " (" & dr.RMASHD_RMASMPACKINGNO.ToString().Trim() & ")"
                    End If
                Next

                If sMessage.Trim() <> "" Then
                    sMessage = _oLanguage.getText("RMA", "197", ctlLanguage.eumType.Validator) & "<BR>" & sMessage
                    Throw New ArgumentException(sMessage)
                End If

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' 取得 PrimaryKey
        ''' </summary>
        ''' <value>回傳em_rfnbr</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property getPrimaryKey() As String
            Get
                Return _PrimaryKey
            End Get
        End Property
    End Class
#End Region

#Region "Class:Client:客戶"
    Public Class Client
        ''' 查詢 RMADETAIL 資料
        Public Function QryRMADETAIL(ByVal RMAD_ID As String) As DataTable

            Dim dt As New DataTable

            Dim sCondition As String = ""
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)



            oConn.Open()
            Try


                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_ID=:RMAD_ID"

                Dim sSQL As String = " SELECT RMAD_SERIALNO from RMADETAIL  WHERE 1=1 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' 查詢 Export 資料
        ''' </summary>
        ''' <param name="Export_Serialno">Serial No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryExport(ByVal Export_Serialno As String) As RmaDTO.dtExportDataTable

            Dim dtRequest As New RmaDTO.dtExportDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = "Select a.export_partno,a.export_fd_sr e_sr,
                                        a.export_fd_01 e_01,
                                        a.export_fd_02 e_02,
                                        a.export_fd_03 e_03,
                                        a.export_fd_04 e_04,
                                        a.export_fd_05 e_05,
                                        a.export_fd_06 e_06,
                                        a.export_fd_07 e_07,
                                        a.export_fd_08 e_08,
                                        a.export_fd_09 e_09,
                                        a.export_fd_10 e_10,
                                        a.export_fd_11 e_11,
                                        a.export_fd_12 e_12,
                                        a.export_fd_13 e_13,
                                        a.export_fd_14 e_14,
                                        a.export_fd_15 e_15,
                                        a.export_fd_16 e_16,
                                        a.export_fd_17 e_17,
                                        a.export_fd_18 e_18,
                                        B.FD_01 n_01,
                                        B.FD_02 n_02,
                                        B.FD_03 n_03,
                                        B.FD_04 n_04,
                                        B.FD_05 n_05,
                                        B.FD_06 n_06,
                                        B.FD_07 n_07,
                                        B.FD_08 n_08,
                                        B.FD_09 n_09,
                                        B.FD_10 n_10,
                                        B.FD_11 n_11,
                                        B.FD_12 n_12,
                                        B.FD_13 n_13,
                                        B.FD_14 n_14,
                                        B.FD_15 n_15,
                                        B.FD_16 n_16,
                                        B.FD_17 n_17,
                                        B.FD_18 n_18,
                                        case When (Select ima021 from cipherlab.ima_file where ima01=a.export_fd_01) Is null Then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_01) end m_01,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_02) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_02) end m_02,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_03) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_03) end m_03,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_04) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_04) end m_04,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_05) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_05) end m_05,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_06) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_06) end m_06,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_07) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_07) end m_07,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_08) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_08) end m_08,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_09) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_09) end m_09,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_10) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_10) end m_10,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_11) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_11) end m_11,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_12) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_12) end m_12,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_13) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_13) end m_13,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_14) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_14) end m_14,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_15) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_15) end m_15,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_16) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_16) end m_16,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_17) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_17) end m_17,
                                        case when (select ima021 from cipherlab.ima_file where ima01=a.export_fd_18) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.export_fd_18) end m_18
                                        from export a join cipherlab.SRVerName b on A.EXPORT_FD_SR = B.SR
                                        where export_serialno is not null
                                    "

                If Export_Serialno <> "" Then
                    sSQL += " AND export_serialno='" & Export_Serialno & "'"
                End If

                sSQL += " ORDER BY Export_Serialno"
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

        ''' <summary>
        ''' 查詢 Export 資料
        ''' </summary>
        ''' <param name="TC_OAE010">Part No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryTcOaeFile(ByVal TC_OAE010 As String) As RmaDTO.dtTcOaeFileDataTable

            Dim dtRequest As New RmaDTO.dtTcOaeFileDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = " select a.TC_OAE010,a.tc_oae460, "
                sSQL = sSQL & "a.FD_01, "
                sSQL = sSQL & "a.FD_02,"
                sSQL = sSQL & "a.FD_03,"
                sSQL = sSQL & "a.FD_04,"
                sSQL = sSQL & "a.FD_05,"
                sSQL = sSQL & "a.FD_06,"
                sSQL = sSQL & "a.FD_07,"
                sSQL = sSQL & "a.FD_08,"
                sSQL = sSQL & "a.FD_09,"
                sSQL = sSQL & "a.FD_10,"
                sSQL = sSQL & "a.FD_11,"
                sSQL = sSQL & "a.FD_12,"
                sSQL = sSQL & "a.FD_13,"
                sSQL = sSQL & "a.FD_14,"
                sSQL = sSQL & "a.FD_15,"
                sSQL = sSQL & "a.FD_16,"
                sSQL = sSQL & "a.FD_17,"
                sSQL = sSQL & "a.FD_18,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_01) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_01) end m_01,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_02) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_02) end m_02,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_03) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_03) end m_03,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_04) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_04) end m_04,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_05) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_05) end m_05,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_06) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_06) end m_06,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_07) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_07) end m_07,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_08) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_08) end m_08,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_09) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_09) end m_09,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_10) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_10) end m_10,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_11) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_11) end m_11,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_12) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_12) end m_12,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_13) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_13) end m_13,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_14) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_14) end m_14,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_15) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_15) end m_15,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_16) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_16) end m_16,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_17) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_17) end m_17,"
                sSQL = sSQL & "case when (select ima021 from cipherlab.ima_file where ima01=a.FD_18) is null then '' else (select ima021 from cipherlab.ima_file where ima01=a.FD_18) end m_18"
                sSQL = sSQL & " from cipherlab.TC_OAE_FILE a"
                sSQL = sSQL & " where a.TC_OAE010 is not null"

                If TC_OAE010 <> "" Then
                    sSQL += " AND a.TC_OAE010='" & TC_OAE010 & "'"
                End If

                sSQL += " ORDER BY a.TC_OAE010"
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

        ''' <summary>
        ''' 查詢 SrVerBank 資料
        ''' </summary>
        ''' <param name="SR">SR No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QrySrVerBank(ByVal SR As String) As RmaDTO.dtSrVerBankDataTable

            Dim dtRequest As New RmaDTO.dtSrVerBankDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = " SELECT "
                sSQL = sSQL & "a.SR,a.FD_Name,a.VER_ITEM,b.IMA02,b.IMA021 FROM cipherlab.SRVerBank a,cipherlab.IMA_FILE b WHERE a.VER_ITEM=b.IMA01 AND a.ACTI='Y' "
                If SR <> "" Then
                    sSQL += " AND SR='" & SR & "'"
                End If

                sSQL += " ORDER BY VER_ITEM"
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

        ''' <summary>
        ''' 查詢 SR 資料
        ''' </summary>
        ''' <param name="SR">SR No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QrySRVerName(ByVal SR As String) As RmaDTO.dtSRVerNameDataTable

            Dim dtRequest As New RmaDTO.dtSRVerNameDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim sSQL As String = " SELECT "
                sSQL = sSQL & " SR, FD_01, FD_02, FD_03, FD_04, FD_05, FD_06, FD_07,"
                sSQL = sSQL & " FD_08, FD_09,FD_10,FD_11,FD_12,FD_13,FD_14,FD_15,FD_16,FD_17,FD_18 "
                sSQL = sSQL & " FROM cipherlab.SRVerName "
                sSQL = sSQL & " WHERE SR IS NOT NULL "
                If SR <> "" Then
                    sSQL += " AND SR='" & SR & "'"
                End If

                sSQL += " ORDER BY SR"
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

        ''' <summary>
        ''' 變更 Serial Number 版本
        ''' </summary>
        ''' <param name="dtExport">dtExportUpdateDataTable</param>
        ''' <param name="dtRmaVerLog">dtRmaVerLogInsertDataTable</param>
        ''' <remarks></remarks>
        Public Sub Update_ExportVer(ByVal dtExport As RmaDTO.dtExportUpdateDataTable, ByVal dtRmaVerLog As RmaDTO.dtRmaVerLogInsertDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                Dim i As Integer
                For i = 0 To dtExport.Count - 1
                    Dim dr As RmaDTO.dtExportUpdateRow = dtExport(i)
                    oExecute.addParameter("EXPORT_FD_01", dr.EXPORT_FD_01, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_02", dr.EXPORT_FD_02, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_03", dr.EXPORT_FD_03, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_04", dr.EXPORT_FD_04, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_05", dr.EXPORT_FD_05, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_06", dr.EXPORT_FD_06, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_07", dr.EXPORT_FD_07, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_08", dr.EXPORT_FD_08, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_09", dr.EXPORT_FD_09, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_10", dr.EXPORT_FD_10, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_11", dr.EXPORT_FD_11, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_12", dr.EXPORT_FD_12, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_13", dr.EXPORT_FD_13, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_14", dr.EXPORT_FD_14, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_15", dr.EXPORT_FD_15, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_16", dr.EXPORT_FD_16, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_17", dr.EXPORT_FD_17, OracleType.VarChar)
                    oExecute.addParameter("EXPORT_FD_18", dr.EXPORT_FD_18, OracleType.VarChar)

                    oExecute.addWHERE("export_serialno", dr.export_serialno.Trim(), OracleType.VarChar)
                    oExecute.Command("Export", Execute.eumCommandType.UPDATE)
                Next

                For i = 0 To dtRmaVerLog.Count - 1
                    Dim dr As RmaDTO.dtRmaVerLogInsertRow = dtRmaVerLog(i)
                    oExecute.addParameter("RMAVL_SERIALNO", dr.RMAVL_SERIALNO, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_DATETIME", dr.RMAVL_DATETIME, OracleType.DateTime)
                    oExecute.addParameter("RMAVL_PNO", dr.RMAVL_PNO, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_SR", dr.RMAVL_SR, OracleType.VarChar)

                    oExecute.addParameter("RMAVL_OLD_FD_01", dr.RMAVL_OLD_FD_01, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_02", dr.RMAVL_OLD_FD_02, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_03", dr.RMAVL_OLD_FD_03, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_04", dr.RMAVL_OLD_FD_04, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_05", dr.RMAVL_OLD_FD_05, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_06", dr.RMAVL_OLD_FD_06, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_07", dr.RMAVL_OLD_FD_07, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_08", dr.RMAVL_OLD_FD_08, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_09", dr.RMAVL_OLD_FD_09, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_10", dr.RMAVL_OLD_FD_10, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_11", dr.RMAVL_OLD_FD_11, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_12", dr.RMAVL_OLD_FD_12, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_13", dr.RMAVL_OLD_FD_13, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_14", dr.RMAVL_OLD_FD_14, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_15", dr.RMAVL_OLD_FD_15, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_16", dr.RMAVL_OLD_FD_16, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_17", dr.RMAVL_OLD_FD_17, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_OLD_FD_18", dr.RMAVL_OLD_FD_18, OracleType.VarChar)

                    oExecute.addParameter("RMAVL_NEW_FD_01", dr.RMAVL_NEW_FD_01, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_02", dr.RMAVL_NEW_FD_02, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_03", dr.RMAVL_NEW_FD_03, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_04", dr.RMAVL_NEW_FD_04, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_05", dr.RMAVL_NEW_FD_05, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_06", dr.RMAVL_NEW_FD_06, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_07", dr.RMAVL_NEW_FD_07, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_08", dr.RMAVL_NEW_FD_08, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_09", dr.RMAVL_NEW_FD_09, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_10", dr.RMAVL_NEW_FD_10, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_11", dr.RMAVL_NEW_FD_11, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_12", dr.RMAVL_NEW_FD_12, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_13", dr.RMAVL_NEW_FD_13, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_14", dr.RMAVL_NEW_FD_14, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_15", dr.RMAVL_NEW_FD_15, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_16", dr.RMAVL_NEW_FD_16, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_17", dr.RMAVL_NEW_FD_17, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_NEW_FD_18", dr.RMAVL_NEW_FD_18, OracleType.VarChar)

                    oExecute.addParameter("RMAVL_AD", dr.RMAVL_AD, OracleType.VarChar)
                    oExecute.addParameter("RMAVL_ADNAME", dr.RMAVL_ADNAME, OracleType.VarChar)

                    oExecute.Command("RMA_VERLOG", Execute.eumCommandType.AddNew)
                Next
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
        ''' 取得 待客戶報價確認單
        ''' </summary>
        ''' <param name="CU_NO">客戶編號</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwClient_WorkListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWork(ByVal CU_NO As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.vwClient_WorkListDataTable

            Dim sCondition As String = ""
            Dim dtClientList As New RmaDTO.vwClient_WorkListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMA_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '客戶編號
                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", CU_NO, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_CUNO=:RMA_CUNO"

                If RMANo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMA_NO=:RMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_MODELNO=:RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_SERIALNO=:RMAD_SERIALNO"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If


                '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                oQuery.addWHERE("RMA_STATUS", ":RMA_STATUS1", "10", OracleType.Int16)
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "40", OracleType.Int16)

                oQuery.addWHERE("RMA_STATUS", ":RMA_STATUS2", "20", OracleType.Int16)
                oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "20", OracleType.Int16)
                sCondition = sCondition & " AND ((RMA_STATUS>:RMA_STATUS1 AND RMAD_STATUS=:RMAD_STATUS1) OR (RMA_STATUS<:RMA_STATUS2 AND RMAD_STATUS<:RMAD_STATUS2))"

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sSQL As String = "SELECT * " &
                    " FROM vwClient_WorkList WHERE 1=1" & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClientList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClientList
        End Function

        ''' <summary>
        ''' 取得 待客戶報價確認單
        ''' </summary>
        ''' <param name="CU_NO">客戶編號</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwClient_WorkListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWorkCUSTOMER_PRODUCT_NUMBER(ByVal CU_NO As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String, ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String, ByVal Optional OrderBY As String = "") As RmaDTO.vwClient_WorkListDataTable
            Dim text As String = ""
            Dim vwClient_WorkListDataTable As RmaDTO.vwClient_WorkListDataTable = New RmaDTO.vwClient_WorkListDataTable()
            Dim dataTable As DataTable = New DataTable()
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            connection.Open()

            Try

                If Operators.CompareString(OrderBY.Trim(), "", TextCompare:=False) = 0 Then
                    OrderBY = " RMA_CSTMP desc"
                End If

                OrderBY = " ORDER BY " & OrderBY
                query.addWHERE("RMA_CUNO", ":RMA_CUNO", CU_NO, OracleType.VarChar)
                text += " AND RMA_CUNO=:RMA_CUNO"

                If Operators.CompareString(RMANo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    text += " AND  CUSTOMER_PRODUCT_NUMBER=:RMA_NO    "
                End If

                If Operators.CompareString(ModelNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    text += " AND RMAD_MODELNO=:RMAD_MODELNO"
                End If

                If Operators.CompareString(SerialNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    text += " AND RMAD_SERIALNO=:RMAD_SERIALNO"
                End If

                If Operators.CompareString(CU_NAME.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    CU_NAME = "%" & CU_NAME.Trim() & "%"
                    query.addWHERE("CU_NAME", ":CU_NAME", CU_NAME.ToLower(), OracleType.NVarChar)
                    text += " AND lower(CU_NAME) like :CU_NAME"
                End If

                query.addWHERE("RMA_STATUS", ":RMA_STATUS1", "10", OracleType.Int16)
                query.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "40", OracleType.Int16)
                query.addWHERE("RMA_STATUS", ":RMA_STATUS2", "20", OracleType.Int16)
                query.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "20", OracleType.Int16)
                text += " AND ((RMA_STATUS>:RMA_STATUS1 AND RMAD_STATUS=:RMAD_STATUS1) OR (RMA_STATUS<:RMA_STATUS2 AND RMAD_STATUS<:RMAD_STATUS2))"

                If (Operators.CompareString(fdate.Trim(), "", TextCompare:=False) <> 0) And (Operators.CompareString(edate.Trim(), "", TextCompare:=False) <> 0) Then
                    Dim obj As Object = Convert.ToDateTime(fdate)
                    Dim obj2 As Object = Convert.ToDateTime(edate).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RuntimeHelpers.GetObjectValue(obj), OracleType.DateTime)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RuntimeHelpers.GetObjectValue(obj2), OracleType.DateTime)
                    text += " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sQLStatement As String = "    SELECT " & vbCrLf & "                RMA.RMA_ID" & vbTab & "as" & vbTab & " RMA_ID," & vbCrLf & "                RMA.RMA_NO" & vbTab & "as" & vbTab & "    RMA_NO," & vbCrLf & "                RMA.RMA_CUNO" & vbTab & "as" & vbTab & "    RMA_CUNO," & vbCrLf & "                RMA.RMA_ACCOUNTID" & vbTab & "as" & vbTab & "    RMA_ACCOUNTID," & vbCrLf & "                RMA.RMA_APPLICANT" & vbTab & "as" & vbTab & "    RMA_APPLICANT," & vbCrLf & "                RMA.RMA_TEL" & vbTab & "as" & vbTab & "    RMA_TEL," & vbCrLf & "                RMA.RMA_ADDRESS" & vbTab & "as" & vbTab & "    RMA_ADDRESS," & vbCrLf & "                RMA.RMA_COMPNO" & vbTab & "as" & vbTab & "    RMA_COMPNO," & vbCrLf & "                RMA.RMA_MAIL" & vbTab & "as" & vbTab & "    RMA_MAIL," & vbCrLf & "                RMA.RMA_STATUS" & vbTab & "as" & vbTab & "    RMA_STATUS," & vbCrLf & "                RMA.RMA_AD" & vbTab & "as" & vbTab & "    RMA_AD," & vbCrLf & "                RMA.RMA_ADNAME" & vbTab & "as" & vbTab & "    RMA_ADNAME," & vbCrLf & "                RMA.RMA_CSTMP" & vbTab & "as" & vbTab & "    RMA_CSTMP," & vbCrLf & "                RMA.RMA_LUAD" & vbTab & "as" & vbTab & "    RMA_LUAD," & vbCrLf & "                RMA.RMA_LUADNAME" & vbTab & "as" & vbTab & "    RMA_LUADNAME," & vbCrLf & "                RMA.RMA_LUSTMP" & vbTab & "as" & vbTab & "    RMA_LUSTMP," & vbCrLf & "                RMA.RMA_MARK" & vbTab & "as" & vbTab & "    RMA_MARK," & vbCrLf & "                RMA_REMARK" & vbTab & "as" & vbTab & "    RMA_REMARK," & vbCrLf & "                RMADETAIL.RMAD_ID" & vbTab & "as" & vbTab & "    RMAD_ID," & vbCrLf & "                RMADETAIL.RMAD_SEQ" & vbTab & "as" & vbTab & "    RMAD_SEQ," & vbCrLf & "                RMADETAIL.RMAD_RMANO" & vbTab & "as" & vbTab & "    RMAD_RMANO," & vbCrLf & "                RMADETAIL.RMAD_MODELNO" & vbTab & "as" & vbTab & "    RMAD_MODELNO," & vbCrLf & "                RMADETAIL.RMAD_SERIALNO" & vbTab & "as" & vbTab & "    RMAD_SERIALNO," & vbCrLf & "                RMADETAIL.RMAD_CUSNAME" & vbTab & "as" & vbTab & "    RMAD_CUSNAME," & vbCrLf & "                RMADETAIL.RMAD_WARRANTY" & vbTab & "as" & vbTab & "    RMAD_WARRANTY," & vbCrLf & "                RMADETAIL.RMAD_FARFARCNO" & vbTab & "as" & vbTab & "    RMAD_FARFARCNO," & vbCrLf & "                RMADETAIL.RMAD_FARNO" & vbTab & "as" & vbTab & "    RMAD_FARNO," & vbCrLf & "                RMADETAIL.RMAD_UPLOADFILE" & vbTab & "as" & vbTab & "    RMAD_UPLOADFILE," & vbCrLf & "                RMADETAIL.RMAD_PROBLEMDESC" & vbTab & "as" & vbTab & "    RMAD_PROBLEMDESC," & vbCrLf & "                RMADETAIL.RMAD_STATUS" & vbTab & "as" & vbTab & "    RMAD_STATUS," & vbCrLf & "                RMADETAIL.RMAD_ISFILL" & vbTab & "as" & vbTab & "    RMAD_ISFILL," & vbCrLf & "                RMADETAIL.RMAD_RECVAD" & vbTab & "as" & vbTab & "    RMAD_RECVAD," & vbCrLf & "                RMADETAIL.RMAD_RECVADNAME" & vbTab & "as" & vbTab & "    RMAD_RECVADNAME," & vbCrLf & "                RMADETAIL.RMAD_RECVDATE" & vbTab & "as" & vbTab & "    RMAD_RECVDATE," & vbCrLf & "                RMADETAIL.RMAD_RECEVSTATUS" & vbTab & "as" & vbTab & "    RMAD_RECEVSTATUS," & vbCrLf & "                RMADETAIL.RMAD_AD" & vbTab & "as" & vbTab & "    RMAD_AD," & vbCrLf & "                RMADETAIL.RMAD_ADNAME" & vbTab & "as" & vbTab & "    RMAD_ADNAME," & vbCrLf & "                RMADETAIL.RMAD_CSTMP" & vbTab & "as" & vbTab & "    RMAD_CSTMP," & vbCrLf & "                RMADETAIL.RMAD_LUAD" & vbTab & "as" & vbTab & "    RMAD_LUAD," & vbCrLf & "                RMADETAIL.RMAD_LUADNAME" & vbTab & "as" & vbTab & "    RMAD_LUADNAME," & vbCrLf & "                RMADETAIL.RMAD_LUSTMP" & vbTab & "as" & vbTab & "    RMAD_LUSTMP," & vbCrLf & "                RMADETAIL.RMAD_MARK" & vbTab & "as" & vbTab & "    RMAD_MARK," & vbCrLf & "                RMADETAIL.RMAD_PRODUCTDESC" & vbTab & "as" & vbTab & "    RMAD_PRODUCTDESC," & vbCrLf & "                RMADETAIL.RMAD_ISWARRANTY" & vbTab & "as" & vbTab & "    RMAD_ISWARRANTY," & vbCrLf & "                CU_NO" & vbTab & "as" & vbTab & "    CU_NO," & vbCrLf & "                CU_NAME" & vbTab & "as" & vbTab & "    CU_NAME," & vbCrLf & "                CU_SALESID" & vbTab & "as" & vbTab & "    CU_SALESID," & vbCrLf & "                CU_ASSISTANTID" & vbTab & "as" & vbTab & "    CU_ASSISTANTID," & vbCrLf & "                RMAR_COMPNO" & vbTab & "as" & vbTab & "    RMAR_COMPNO," & vbCrLf & "                COMP_NAME" & vbTab & "as" & vbTab & "    COMP_NAME," & vbCrLf & "                RMAR_REPAIR_ISFILL" & vbTab & "as" & vbTab & "    RMAR_REPAIR_ISFILL," & vbCrLf & "                RMARQ_ASSIGECURRENCYCODE" & vbTab & "as" & vbTab & "    RMARQ_ASSIGECURRENCYCODE," & vbCrLf & "                RMARQ_ASSIGEQUOTE" & vbTab & "as" & vbTab & "    RMARQ_ASSIGEQUOTE," & vbCrLf & "                RMARQ_CURRENCYCODE" & vbTab & "as" & vbTab & "    RMARQ_CURRENCYCODE," & vbCrLf & "                RMARQ_QUOTE" & vbTab & "as" & vbTab & "    RMARQ_QUOTE," & vbCrLf & "                RMASQ_LABORCOST" & vbTab & "as" & vbTab & "    RMASQ_LABORCOST," & vbCrLf & "                RMASQ_MATERIALCOST" & vbTab & "as" & vbTab & "    RMASQ_MATERIALCOST," & vbCrLf & "                RMASQ_QUOTE" & vbTab & "as" & vbTab & "    RMASQ_QUOTE," & vbCrLf & "                RMASQ_CURRENCYCODE" & vbTab & "as" & vbTab & "    RMASQ_CURRENCYCODE," & vbCrLf & "                RMASQ_CURRENCYRATE" & vbTab & "as" & vbTab & "    RMASQ_CURRENCYRATE," & vbCrLf & "                RMASQ_ID" & vbTab & "as" & vbTab & "    RMASQ_ID," & vbCrLf & "                CUSTOMER_PRODUCT_NUMBER" & vbTab & "as" & vbTab & "    CUSTOMER_PRODUCT_NUMBER" & vbCrLf & vbCrLf & "                FROM RMA" & vbCrLf & "                INNER JOIN RMADETAIL" & vbCrLf & "                ON     RMA.Rma_no = RMADETAIL.rmad_rmano" & vbCrLf & "                AND RMADETAIL.RMAD_MARK = 0" & vbCrLf & "                AND RMAD_RECEVSTATUS <> 2" & vbCrLf & "                AND (   (RMA_STATUS = 20 AND RMAD_STATUS = 40)" & vbCrLf & "                OR (RMA_STATUS = 0 AND RMAD_STATUS = 0))" & vbCrLf & "                INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & vbCrLf & "                LEFT OUTER JOIN RMAREPAIR" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID" & vbCrLf & "                LEFT OUTER JOIN RMAREPAIR_QUOTED" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID" & vbCrLf & "                LEFT OUTER JOIN RMASALE_QUOTED" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID" & vbCrLf & "                LEFT OUTER JOIN COMPANY ON RMAREPAIR.RMAR_COMPNO = COMPANY.COMP_NO   WHERE 1=1" & text & OrderBY
                dataTable = query.ExecuteDT(sQLStatement)
                Dim dtSource As DataTable = dataTable
                Dim dtTarget As DataTable = vwClient_WorkListDataTable
                Common.TransferDataTable(dtSource, dtTarget)
                vwClient_WorkListDataTable = CType(dtTarget, RmaDTO.vwClient_WorkListDataTable)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return vwClient_WorkListDataTable
        End Function

        ''' <summary>
        ''' 取得 待客戶報價確認單
        ''' </summary>
        ''' <param name="CU_NO">客戶編號</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwClient_WorkListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByWorkProductAll(ByVal CU_NO As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String, ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String, ByVal Optional OrderBY As String = "") As RmaDTO.vwClient_WorkListDataTable
            Dim text As String = ""
            Dim vwClient_WorkListDataTable As RmaDTO.vwClient_WorkListDataTable = New RmaDTO.vwClient_WorkListDataTable()
            Dim dataTable As DataTable = New DataTable()
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            connection.Open()

            Try

                If Operators.CompareString(OrderBY.Trim(), "", TextCompare:=False) = 0 Then
                    OrderBY = " RMA_CSTMP desc"
                End If

                OrderBY = " ORDER BY " & OrderBY
                query.addWHERE("RMA_CUNO", ":RMA_CUNO", CU_NO, OracleType.VarChar)
                text += " AND RMA_CUNO=:RMA_CUNO"

                If Operators.CompareString(RMANo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
                    text += " AND ( RMA_NO=:RMA_NO   or   CUSTOMER_PRODUCT_NUMBER=:RMA_NO )  "
                End If

                If Operators.CompareString(ModelNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo, OracleType.VarChar)
                    text += " AND RMAD_MODELNO=:RMAD_MODELNO"
                End If

                If Operators.CompareString(SerialNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    text += " AND RMAD_SERIALNO=:RMAD_SERIALNO"
                End If

                If Operators.CompareString(CU_NAME.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    CU_NAME = "%" & CU_NAME.Trim() & "%"
                    query.addWHERE("CU_NAME", ":CU_NAME", CU_NAME.ToLower(), OracleType.NVarChar)
                    text += " AND lower(CU_NAME) like :CU_NAME"
                End If

                query.addWHERE("RMA_STATUS", ":RMA_STATUS1", "10", OracleType.Int16)
                query.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", "40", OracleType.Int16)
                query.addWHERE("RMA_STATUS", ":RMA_STATUS2", "20", OracleType.Int16)
                query.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", "20", OracleType.Int16)
                text += " AND ((RMA_STATUS>:RMA_STATUS1 AND RMAD_STATUS=:RMAD_STATUS1) OR (RMA_STATUS<:RMA_STATUS2 AND RMAD_STATUS<:RMAD_STATUS2))"

                If (Operators.CompareString(fdate.Trim(), "", TextCompare:=False) <> 0) And (Operators.CompareString(edate.Trim(), "", TextCompare:=False) <> 0) Then
                    Dim obj As Object = Convert.ToDateTime(fdate)
                    Dim obj2 As Object = Convert.ToDateTime(edate).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RuntimeHelpers.GetObjectValue(obj), OracleType.DateTime)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RuntimeHelpers.GetObjectValue(obj2), OracleType.DateTime)
                    text += " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sQLStatement As String = "    SELECT " & vbCrLf & "                RMA.RMA_ID" & vbTab & "as" & vbTab & " RMA_ID," & vbCrLf & "                RMA.RMA_NO" & vbTab & "as" & vbTab & "    RMA_NO," & vbCrLf & "                RMA.RMA_CUNO" & vbTab & "as" & vbTab & "    RMA_CUNO," & vbCrLf & "                RMA.RMA_ACCOUNTID" & vbTab & "as" & vbTab & "    RMA_ACCOUNTID," & vbCrLf & "                RMA.RMA_APPLICANT" & vbTab & "as" & vbTab & "    RMA_APPLICANT," & vbCrLf & "                RMA.RMA_TEL" & vbTab & "as" & vbTab & "    RMA_TEL," & vbCrLf & "                RMA.RMA_ADDRESS" & vbTab & "as" & vbTab & "    RMA_ADDRESS," & vbCrLf & "                RMA.RMA_COMPNO" & vbTab & "as" & vbTab & "    RMA_COMPNO," & vbCrLf & "                RMA.RMA_MAIL" & vbTab & "as" & vbTab & "    RMA_MAIL," & vbCrLf & "                RMA.RMA_STATUS" & vbTab & "as" & vbTab & "    RMA_STATUS," & vbCrLf & "                RMA.RMA_AD" & vbTab & "as" & vbTab & "    RMA_AD," & vbCrLf & "                RMA.RMA_ADNAME" & vbTab & "as" & vbTab & "    RMA_ADNAME," & vbCrLf & "                RMA.RMA_CSTMP" & vbTab & "as" & vbTab & "    RMA_CSTMP," & vbCrLf & "                RMA.RMA_LUAD" & vbTab & "as" & vbTab & "    RMA_LUAD," & vbCrLf & "                RMA.RMA_LUADNAME" & vbTab & "as" & vbTab & "    RMA_LUADNAME," & vbCrLf & "                RMA.RMA_LUSTMP" & vbTab & "as" & vbTab & "    RMA_LUSTMP," & vbCrLf & "                RMA.RMA_MARK" & vbTab & "as" & vbTab & "    RMA_MARK," & vbCrLf & "                RMA_REMARK" & vbTab & "as" & vbTab & "    RMA_REMARK," & vbCrLf & "                RMADETAIL.RMAD_ID" & vbTab & "as" & vbTab & "    RMAD_ID," & vbCrLf & "                RMADETAIL.RMAD_SEQ" & vbTab & "as" & vbTab & "    RMAD_SEQ," & vbCrLf & "                RMADETAIL.RMAD_RMANO" & vbTab & "as" & vbTab & "    RMAD_RMANO," & vbCrLf & "                RMADETAIL.RMAD_MODELNO" & vbTab & "as" & vbTab & "    RMAD_MODELNO," & vbCrLf & "                RMADETAIL.RMAD_SERIALNO" & vbTab & "as" & vbTab & "    RMAD_SERIALNO," & vbCrLf & "                RMADETAIL.RMAD_CUSNAME" & vbTab & "as" & vbTab & "    RMAD_CUSNAME," & vbCrLf & "                RMADETAIL.RMAD_WARRANTY" & vbTab & "as" & vbTab & "    RMAD_WARRANTY," & vbCrLf & "                RMADETAIL.RMAD_FARFARCNO" & vbTab & "as" & vbTab & "    RMAD_FARFARCNO," & vbCrLf & "                RMADETAIL.RMAD_FARNO" & vbTab & "as" & vbTab & "    RMAD_FARNO," & vbCrLf & "                RMADETAIL.RMAD_UPLOADFILE" & vbTab & "as" & vbTab & "    RMAD_UPLOADFILE," & vbCrLf & "                RMADETAIL.RMAD_PROBLEMDESC" & vbTab & "as" & vbTab & "    RMAD_PROBLEMDESC," & vbCrLf & "                RMADETAIL.RMAD_STATUS" & vbTab & "as" & vbTab & "    RMAD_STATUS," & vbCrLf & "                RMADETAIL.RMAD_ISFILL" & vbTab & "as" & vbTab & "    RMAD_ISFILL," & vbCrLf & "                RMADETAIL.RMAD_RECVAD" & vbTab & "as" & vbTab & "    RMAD_RECVAD," & vbCrLf & "                RMADETAIL.RMAD_RECVADNAME" & vbTab & "as" & vbTab & "    RMAD_RECVADNAME," & vbCrLf & "                RMADETAIL.RMAD_RECVDATE" & vbTab & "as" & vbTab & "    RMAD_RECVDATE," & vbCrLf & "                RMADETAIL.RMAD_RECEVSTATUS" & vbTab & "as" & vbTab & "    RMAD_RECEVSTATUS," & vbCrLf & "                RMADETAIL.RMAD_AD" & vbTab & "as" & vbTab & "    RMAD_AD," & vbCrLf & "                RMADETAIL.RMAD_ADNAME" & vbTab & "as" & vbTab & "    RMAD_ADNAME," & vbCrLf & "                RMADETAIL.RMAD_CSTMP" & vbTab & "as" & vbTab & "    RMAD_CSTMP," & vbCrLf & "                RMADETAIL.RMAD_LUAD" & vbTab & "as" & vbTab & "    RMAD_LUAD," & vbCrLf & "                RMADETAIL.RMAD_LUADNAME" & vbTab & "as" & vbTab & "    RMAD_LUADNAME," & vbCrLf & "                RMADETAIL.RMAD_LUSTMP" & vbTab & "as" & vbTab & "    RMAD_LUSTMP," & vbCrLf & "                RMADETAIL.RMAD_MARK" & vbTab & "as" & vbTab & "    RMAD_MARK," & vbCrLf & "                RMADETAIL.RMAD_PRODUCTDESC" & vbTab & "as" & vbTab & "    RMAD_PRODUCTDESC," & vbCrLf & "                RMADETAIL.RMAD_ISWARRANTY" & vbTab & "as" & vbTab & "    RMAD_ISWARRANTY," & vbCrLf & "                CU_NO" & vbTab & "as" & vbTab & "    CU_NO," & vbCrLf & "                CU_NAME" & vbTab & "as" & vbTab & "    CU_NAME," & vbCrLf & "                CU_SALESID" & vbTab & "as" & vbTab & "    CU_SALESID," & vbCrLf & "                CU_ASSISTANTID" & vbTab & "as" & vbTab & "    CU_ASSISTANTID," & vbCrLf & "                RMAR_COMPNO" & vbTab & "as" & vbTab & "    RMAR_COMPNO," & vbCrLf & "                COMP_NAME" & vbTab & "as" & vbTab & "    COMP_NAME," & vbCrLf & "                RMAR_REPAIR_ISFILL" & vbTab & "as" & vbTab & "    RMAR_REPAIR_ISFILL," & vbCrLf & "                RMARQ_ASSIGECURRENCYCODE" & vbTab & "as" & vbTab & "    RMARQ_ASSIGECURRENCYCODE," & vbCrLf & "                RMARQ_ASSIGEQUOTE" & vbTab & "as" & vbTab & "    RMARQ_ASSIGEQUOTE," & vbCrLf & "                RMARQ_CURRENCYCODE" & vbTab & "as" & vbTab & "    RMARQ_CURRENCYCODE," & vbCrLf & "                RMARQ_QUOTE" & vbTab & "as" & vbTab & "    RMARQ_QUOTE," & vbCrLf & "                RMASQ_LABORCOST" & vbTab & "as" & vbTab & "    RMASQ_LABORCOST," & vbCrLf & "                RMASQ_MATERIALCOST" & vbTab & "as" & vbTab & "    RMASQ_MATERIALCOST," & vbCrLf & "                RMASQ_QUOTE" & vbTab & "as" & vbTab & "    RMASQ_QUOTE," & vbCrLf & "                RMASQ_CURRENCYCODE" & vbTab & "as" & vbTab & "    RMASQ_CURRENCYCODE," & vbCrLf & "                RMASQ_CURRENCYRATE" & vbTab & "as" & vbTab & "    RMASQ_CURRENCYRATE," & vbCrLf & "                RMASQ_ID" & vbTab & "as" & vbTab & "    RMASQ_ID," & vbCrLf & "                CUSTOMER_PRODUCT_NUMBER" & vbTab & "as" & vbTab & "    CUSTOMER_PRODUCT_NUMBER" & vbCrLf & vbCrLf & "                FROM RMA" & vbCrLf & "                INNER JOIN RMADETAIL" & vbCrLf & "                ON     RMA.Rma_no = RMADETAIL.rmad_rmano" & vbCrLf & "                AND RMADETAIL.RMAD_MARK = 0" & vbCrLf & "                AND RMAD_RECEVSTATUS <> 2" & vbCrLf & "                AND (   (RMA_STATUS = 20 AND RMAD_STATUS = 40)" & vbCrLf & "                OR (RMA_STATUS = 0 AND RMAD_STATUS = 0))" & vbCrLf & "                INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO" & vbCrLf & "                LEFT OUTER JOIN RMAREPAIR" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID" & vbCrLf & "                LEFT OUTER JOIN RMAREPAIR_QUOTED" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID" & vbCrLf & "                LEFT OUTER JOIN RMASALE_QUOTED" & vbCrLf & "                ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID" & vbCrLf & "                LEFT OUTER JOIN COMPANY ON RMAREPAIR.RMAR_COMPNO = COMPANY.COMP_NO   WHERE 1=1" & text & OrderBY
                dataTable = query.ExecuteDT(sQLStatement)
                Dim dtSource As DataTable = dataTable
                Dim dtTarget As DataTable = vwClient_WorkListDataTable
                Common.TransferDataTable(dtSource, dtTarget)
                vwClient_WorkListDataTable = CType(dtTarget, RmaDTO.vwClient_WorkListDataTable)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return vwClient_WorkListDataTable
        End Function

        ''' <summary>
        ''' 查詢 Request RMA 資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="InStatus">InStatus</param>
        ''' <param name="RepairCenter">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query01(ByVal RMANo As String, ByVal NoRMADID As String, ByVal CuName As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal Repair As String, ByVal InStatus As String, ByVal NOTIN_RMAD_STATUS As String, ByVal RepairCenter As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                Dim sInRepairCenter As String = ""
                Dim sInRepairCenterFirst As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = RepairCenter.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                    sInRepairCenterFirst = arrRepair(i).Trim()
                    If sInRepairCenter <> "" Then
                        sInRepairCenter = sInRepairCenter + ","
                    End If
                    sInRepairCenter = sInRepairCenter + "'" + arrRepair(i).Trim() + "'"

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA.RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If NoRMADID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("NoRMADID", ":NoRMADID", NoRMADID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_ID!=:NoRMADID"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_SERIALNO) like :RMAD_SERIALNO"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA.RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                Dim sSQL As String = " SELECT "
                sSQL = sSQL & " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_PARTSN,"
                sSQL = sSQL & " CU_NO, CU_NAME, RMA.RMA_ID, " '需求新增:BI保固 By buck Add 20250902
                'sSQL = sSQL & " CU_NAME, RMA.RMA_ID, "
                sSQL = sSQL & " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD, "
                sSQL = sSQL & " RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE, "
                sSQL = sSQL & " TrackingNo, ShippedDate,RMAREPAIR.RMAR_COMPNO,RMA.RMA_COMPNO"
                sSQL = sSQL & " ,'0' CHECK_FLAG,COMP_NAME,RMAD_PARTSN, RMAD_APPLY_BI, CU_TIPTOP_ID " 'RMA報價價格調整-抓取TIPTOP CutomerID By Buck modify 20251013
                'sSQL = sSQL & " ,'0' CHECK_FLAG,COMP_NAME,RMAD_PARTSN, RMAD_APPLY_BI " '需求新增:BI保固 By buck Add 20250902
                'sSQL = sSQL & " ,'0' CHECK_FLAG,COMP_NAME,RMAD_PARTSN "
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN COMPANY ON RMAREPAIR.RMAR_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate  "
                sSQL = sSQL & "     FROM RMA_SHIPMENT "
                sSQL = sSQL & "     inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID "
                sSQL = sSQL & "     inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO"
                sSQL = sSQL & "     inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID"
                sSQL = sSQL & " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO "
                sSQL = sSQL & " WHERE rmad_mark=0 AND RMAD_RECEVSTATUS=1 "
                If sInRepairCenter <> "" Then
                    sSQL += " AND (NVL(RMAREPAIR.RMAR_COMPNO,'" + "" + "') IN(" + sInRepairCenter + ") OR  rma.rma_compno in (" + sInRepairCenter + "))"
                End If
                If InStatus.Trim() <> "" Then
                    sSQL = sSQL & " AND RMAD_STATUS IN(" + InStatus + ")"
                End If
                If NOTIN_RMAD_STATUS.Trim() <> "" Then
                    sSQL = sSQL & " AND RMAD_STATUS NOT IN(" + NOTIN_RMAD_STATUS + ")"
                End If

                sSQL = sSQL & sCondition & OrderBY
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

        ''' <summary>
        ''' 查詢 Request RMA 資料 包含未收件
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="InStatus">InStatus</param>
        ''' <param name="RepairCenter">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query02(ByVal RMANo As String, ByVal NoRMADID As String, ByVal CuName As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal Repair As String, ByVal InStatus As String, ByVal NOTIN_RMAD_STATUS As String, ByVal RepairCenter As String,
            Optional ByVal OrderBY As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                Dim sInRepairCenter As String = ""
                Dim sInRepairCenterFirst As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = RepairCenter.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                    sInRepairCenterFirst = arrRepair(i).Trim()
                    If sInRepairCenter <> "" Then
                        sInRepairCenter = sInRepairCenter + ","
                    End If
                    sInRepairCenter = sInRepairCenter + "'" + arrRepair(i).Trim() + "'"

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA.RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If NoRMADID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("NoRMADID", ":NoRMADID", NoRMADID, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_ID!=:NoRMADID"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_SERIALNO) like :RMAD_SERIALNO"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA.RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                Dim sSQL As String = " SELECT "
                sSQL = sSQL & " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY,"
                sSQL = sSQL & " CU_NAME, RMA.RMA_ID, "
                sSQL = sSQL & " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD, "
                sSQL = sSQL & " RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE, "
                sSQL = sSQL & " TrackingNo, ShippedDate,RMAREPAIR.RMAR_COMPNO,RMA.RMA_COMPNO"
                sSQL = sSQL & " ,'0' CHECK_FLAG,COMP_NAME"
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT * FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN COMPANY ON RMAREPAIR.RMAR_COMPNO = COMPANY.COMP_NO "
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate  "
                sSQL = sSQL & "     FROM RMA_SHIPMENT "
                sSQL = sSQL & "     inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID "
                sSQL = sSQL & "     inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO"
                sSQL = sSQL & "     inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID"
                sSQL = sSQL & " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO "
                sSQL = sSQL & " WHERE rmad_mark=0 AND RMAD_RECEVSTATUS<>2 "

                If sInRepairCenter <> "" Then
                    sSQL += " AND (NVL(RMAREPAIR.RMAR_COMPNO,'" + "" + "') IN(" + sInRepairCenter + ") OR  rma.rma_compno in (" + sInRepairCenter + "))"
                End If
                If InStatus.Trim() <> "" Then
                    sSQL = sSQL & " AND RMAD_STATUS IN(" + InStatus + ")"
                End If
                If NOTIN_RMAD_STATUS.Trim() <> "" Then
                    sSQL = sSQL & " AND RMAD_STATUS NOT IN(" + NOTIN_RMAD_STATUS + ")"
                End If

                sSQL = sSQL & sCondition & OrderBY
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

        ''' <summary>
        ''' 查詢 Request RMA 資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="RepairCenter">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal RMANo As String, ByVal CuName As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal RepairCenter As String, Optional ExpireStatus As String = "",
            Optional ByVal OrderBY As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                If RepairCenter.Trim <> "" Then
                    Dim sCondition_Repair As String = ""
                    Dim arrRepair() As String = RepairCenter.Split(",")
                    For i = 0 To arrRepair.Length - 1
                        oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                        oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                        If sCondition_Repair.Trim <> "" Then
                            sCondition_Repair = sCondition_Repair & " OR "
                        End If
                        sCondition_Repair = sCondition_Repair & " RMA.RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                    Next
                    If sCondition_Repair.Trim <> "" Then
                        sCondition = sCondition & " AND (" & sCondition_Repair & ")"
                    End If
                End If


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND (lower(RMAD_SERIALNO) like :RMAD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_SERIALNO OR lower(RMARED_NSERIALNO) like :RMAD_SERIALNO OR lower(RMARED_OSERIALNO) like :RMAD_SERIALNO)"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA.RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                '狀態
                If Status <> "-1" Then
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                    sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA.RMA_CSTMP >=:RMAD_CSTMP1 AND RMA.RMA_CSTMP <=:RMAD_CSTMP2)"

                    'oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    'oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)
                    'sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If

                ' 新增查詢訂單是否過期 by buck modify 20251118 begin 
                If ExpireStatus <> "" Then
                    oQuery.addWHERE("RMARQ_EXPIRED_DATE", ":RMARQ_EXPIRED_DATE", ExpireStatus, OracleType.Int16)
                    sCondition = sCondition & " AND (
                                                    CASE 
                                                        WHEN SYSDATE > RMARQ_EXPIRED_DATE THEN 1 
                                                        ELSE 0 
                                                    END
                                                ) = :RMARQ_EXPIRED_DATE
                                            "
                End If
                ' 新增查詢訂單是否過期 by buck modify 20251118 end

                '需求新增:BI保固 By buck Add 20250902 begin
                ' 新增查詢訂單是否過期 by buck modify 20251118 begin 
                ' 逾期超過1個月的RMA報價單進行強制取消作業 by buck modify 20251201
                Dim sSQL As String = " SELECT DISTINCT 
                                        RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_RECEVSTATUS,
                                        CU_NO,CU_NAME, RMA.RMA_ID,RMARQ_ID,
                                        RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE,
                                        RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD,
                                        RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE,
                                        TrackingNo, ShippedDate,RMAD_PARTSN,RMA.RMA_COMPNO,RMA.RMA_ACCOUNTID
                                        ,RMARQ_EXPIRED_DATE,
                                        CASE 
                                            WHEN SYSDATE > RMARQ_EXPIRED_DATE THEN 1 
                                            ELSE 0 
                                        END AS EXPIRE_STATUS
                                        FROM RMADETAIL
                                        INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO
                                        INNER JOIN
                                        (
                                            SELECT * FROM RMA
                                            INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO
                                        ) vwCustomer
                                        ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO
                                        LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID
                                        LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID
                                        LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID
                                        LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID
                                        LEFT OUTER JOIN
                                        (
                                            SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate
                                            FROM RMA_SHIPMENT 
                                            inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID 
                                            inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO
                                            inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID
                                        ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO 
                                        LEFT OUTER JOIN RMARepair_Detail ON RMADETAIL.RMAD_ID = RMARepair_Detail.RMARED_RMADID  
                                        WHERE rmad_mark=0
                                    " & sCondition & OrderBY
                'Dim sSQL As String = " SELECT "
                'sSQL = sSQL & " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_RECEVSTATUS,"
                'sSQL = sSQL & " CU_NAME, RMA.RMA_ID, "
                'sSQL = sSQL & " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                'sSQL = sSQL & " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD, "
                'sSQL = sSQL & " RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE, "
                'sSQL = sSQL & " TrackingNo, ShippedDate,RMAD_PARTSN,RMA.RMA_COMPNO,RMA.RMA_ACCOUNTID "
                'sSQL = sSQL & " FROM RMADETAIL "
                'sSQL = sSQL & " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                'sSQL = sSQL & " INNER JOIN "
                'sSQL = sSQL & " ( "
                'sSQL = sSQL & " SELECT * FROM RMA "
                'sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                'sSQL = sSQL & " ) vwCustomer "
                'sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                'sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                'sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                'sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                'sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                'sSQL = sSQL & " LEFT OUTER JOIN"
                'sSQL = sSQL & " ("
                'sSQL = sSQL & "     SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate  "
                'sSQL = sSQL & "     FROM RMA_SHIPMENT "
                'sSQL = sSQL & "     inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID "
                'sSQL = sSQL & "     inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO"
                'sSQL = sSQL & "     inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID"
                'sSQL = sSQL & " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO "
                'sSQL = sSQL & " WHERE rmad_mark=0 " & sCondition & OrderBY
                ' 新增查詢訂單是否過期 by buck modify 20251118 end
                '需求新增:BI保固 By buck Add 20250902 end

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

        ''' <summary>
        ''' 查詢 Request RMA 資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="RepairCenter">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExpiredQuery(ByVal RMANo As String, ByVal CuName As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal Repair As String, ByVal fdate As String, ByVal edate As String, ByVal RepairCenter As String, Optional ExpireStatus As String = "",
            Optional ByVal OrderBY As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                If RepairCenter.Trim <> "" Then
                    Dim sCondition_Repair As String = ""
                    Dim arrRepair() As String = RepairCenter.Split(",")
                    For i = 0 To arrRepair.Length - 1
                        oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                        oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                        If sCondition_Repair.Trim <> "" Then
                            sCondition_Repair = sCondition_Repair & " OR "
                        End If
                        sCondition_Repair = sCondition_Repair & " RMA.RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                    Next
                    If sCondition_Repair.Trim <> "" Then
                        sCondition = sCondition & " AND (" & sCondition_Repair & ")"
                    End If
                End If


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND (lower(RMAD_SERIALNO) like :RMAD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_SERIALNO OR lower(RMARED_NSERIALNO) like :RMAD_SERIALNO OR lower(RMARED_OSERIALNO) like :RMAD_SERIALNO)"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA.RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA.RMA_CSTMP >=:RMAD_CSTMP1 AND RMA.RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                If ExpireStatus <> "" Then
                    oQuery.addWHERE("RMARQ_EXPIRED_DATE", ":RMARQ_EXPIRED_DATE", ExpireStatus, OracleType.Int16)
                    sCondition = sCondition & " AND (
                                                    CASE 
                                                        WHEN SYSDATE > RMARQ_EXPIRED_DATE THEN 1 
                                                        ELSE 0 
                                                    END
                                                ) = :RMARQ_EXPIRED_DATE
                                            "
                End If

                Dim sSQL As String = " SELECT DISTINCT 
                                        RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_RECEVSTATUS,
                                        CU_NO,CU_NAME, RMA.RMA_ID,RMARQ_ID,
                                        RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE,
                                        RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD,
                                        RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE,
                                        TrackingNo, ShippedDate,RMAD_PARTSN,RMA.RMA_COMPNO,RMA.RMA_ACCOUNTID
                                        ,RMARQ_EXPIRED_DATE,
                                        CASE 
                                            WHEN SYSDATE > RMARQ_EXPIRED_DATE THEN 1 
                                            ELSE 0 
                                        END AS EXPIRE_STATUS
                                        FROM RMADETAIL
                                        INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO
                                        INNER JOIN
                                        (
                                            SELECT * FROM RMA
                                            INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO
                                        ) vwCustomer
                                        ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO
                                        LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID
                                        LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID
                                        LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID
                                        LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID
                                        LEFT OUTER JOIN
                                        (
                                            SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate
                                            FROM RMA_SHIPMENT 
                                            inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID 
                                            inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO
                                            inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID
                                        ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO 
                                        LEFT OUTER JOIN RMARepair_Detail ON RMADETAIL.RMAD_ID = RMARepair_Detail.RMARED_RMADID  
                                        WHERE RMAD_STATUS='40' AND rmad_mark=0
                                    " & sCondition & OrderBY

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

        ''' <summary>
        ''' 查詢 Request RMA 資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">CuName</param>
        ''' <param name="ModelNo">ModelNo</param>
        ''' <param name="SerialNo">SerialNo</param>
        ''' <param name="Repair">Repair</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">fdate</param>
        ''' <param name="edate">edate</param>
        ''' <param name="RepairCenter">RepairCenter</param>
        ''' <param name="OrderBY">OrderBY</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Query_Not_Like(ByVal RMANo As String, ByVal CuName As String, ByVal ModelNo As String, ByVal SerialNo As String, ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal RepairCenter As String, ByVal Optional OrderBY As String = "") As RmaDTO.tmpRequest_ListDataTable
            Dim num As Integer = 0
            Dim text As String = ""
            Dim tmpRequest_ListDataTable As RmaDTO.tmpRequest_ListDataTable = New RmaDTO.tmpRequest_ListDataTable()
            Dim dataTable As DataTable = New DataTable()
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            connection.Open()

            Try

                If Operators.CompareString(OrderBY.Trim(), "", TextCompare:=False) = 0 Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If

                OrderBY = " ORDER BY " & OrderBY

                If Operators.CompareString(RepairCenter.Trim(), "", TextCompare:=False) <> 0 Then
                    Dim text2 As String = ""
                    Dim array As String() = RepairCenter.Split(","c)
                    Dim num2 As Integer = array.Length - 1

                    For num = 0 To num2
                        query.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & num, array(num).Trim(), OracleType.VarChar)
                        query.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & num, array(num).Trim(), OracleType.VarChar)

                        If Operators.CompareString(text2.Trim(), "", TextCompare:=False) <> 0 Then
                            text2 += " OR "
                        End If

                        text2 = text2 & " RMA.RMA_COMPNO =:RMA_COMPNO" & num & " OR RMAR_COMPNO =:RMAR_COMPNO" & num
                    Next

                    If Operators.CompareString(text2.Trim(), "", TextCompare:=False) <> 0 Then
                        text = text & " AND (" & text2 & ")"
                    End If
                End If

                If Operators.CompareString(RMANo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    RMANo = RMANo.Trim()
                    query.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    text += " AND lower(RMAD_RMANO) =:RMAD_RMANO"
                End If

                If Operators.CompareString(CuName.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    CuName = "%" & CuName.Trim() & "%"
                    query.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    text += " AND lower(CU_NAME) like :CU_NAME"
                End If

                If Operators.CompareString(ModelNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    query.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    text += " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If Operators.CompareString(SerialNo.ToString().Trim(), "", TextCompare:=False) <> 0 Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    query.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    text += " AND (lower(RMAD_SERIALNO) like :RMAD_SERIALNO OR lower(RMAD_PARTSN) like :RMAD_SERIALNO)"
                End If

                If Operators.CompareString(Repair, "-1", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    query.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)
                    text += " AND (RMA.RMA_COMPNO=:RMA_COMPNO"
                    text += " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                If Operators.CompareString(Status, "-1", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                    text += " AND RMAD_STATUS=:RMAD_STATUS"
                End If

                If (Operators.CompareString(fdate.Trim(), "", TextCompare:=False) <> 0) And (Operators.CompareString(edate.Trim(), "", TextCompare:=False) <> 0) Then
                    Dim obj As Object = Convert.ToDateTime(fdate)
                    Dim obj2 As Object = Convert.ToDateTime(edate).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RuntimeHelpers.GetObjectValue(obj), OracleType.DateTime)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RuntimeHelpers.GetObjectValue(obj2), OracleType.DateTime)
                    text += " AND (RMA.RMA_CSTMP >=:RMAD_CSTMP1 AND RMA.RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim text3 As String = " SELECT "
                text3 += " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY, RMAD_RECEVSTATUS,"
                text3 += " CU_NAME, RMA.RMA_ID, "
                text3 += " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                text3 += " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, RMAR_REPAIRAD, "
                text3 += " RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMARSD_QUOTE, RMARSD_CURRENCYCODE, "
                text3 += " TrackingNo, ShippedDate,RMAD_PARTSN,RMA.RMA_COMPNO,RMA.RMA_ACCOUNTID "
                text3 += " FROM RMADETAIL "
                text3 += " INNER JOIN RMA ON RMADETAIL.RMAD_RMANO = RMA.RMA_NO "
                text3 += " INNER JOIN "
                text3 += " ( "
                text3 += " SELECT * FROM RMA "
                text3 += " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                text3 += " ) vwCustomer "
                text3 += " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                text3 += " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                text3 += " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                text3 += " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                text3 += " LEFT OUTER JOIN RMA_SHIPMENTDETAIL ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID "
                text3 += " LEFT OUTER JOIN"
                text3 += " ("
                text3 += "     SELECT rmashd_rmano, rmasmd_rmadid,  RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate  "
                text3 += "     FROM RMA_SHIPMENT "
                text3 += "     inner join RMA_SHIPMENTDETAIL on RMASM_ID = RMASMD_RMASMID "
                text3 += "     inner join RMA_SHIPPINGDETAIL on rmashd_shipmentno = RMASM_PACKINGNO"
                text3 += "     inner join RMA_SHIPPING on RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID"
                text3 += " ) vwSHIPPING ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID and rmashd_rmano=RMA.RMA_NO "
                text3 = text3 & " WHERE rmad_mark=0 " & text & OrderBY
                dataTable = query.ExecuteDT(text3)
                Dim dtSource As DataTable = dataTable
                Dim dtTarget As DataTable = tmpRequest_ListDataTable
                Common.TransferDataTable(dtSource, dtTarget)
                tmpRequest_ListDataTable = CType(dtTarget, RmaDTO.tmpRequest_ListDataTable)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return tmpRequest_ListDataTable
        End Function

        Public Function Query_Alert(ByVal CU_NO As String) As DataTable
            Dim num As Integer = 0
            Dim text As String = ""
            Dim tmpRequest_ListDataTable As RmaDTO.tmpRequest_ListDataTable = New RmaDTO.tmpRequest_ListDataTable()
            Dim result As DataTable = New DataTable()
            Dim val As Connection = New Connection()
            Dim val2 As Query = New Query(val)
            val.Open()

            Try

                If Operators.CompareString(CU_NO, "", TextCompare:=False) <> 0 Then
                    val2.addWHERE("RMA_CUNO", ":RMA_CUNO", CObj(CU_NO), OracleType.Char)
                    text += " AND RMA_CUNO=:RMA_CUNO"
                End If

                Dim text2 As String = " SELECT ROWNUM, t.* FROM (SELECT  RMAD_RMANO,RMAD_LUSTMP as RMA_CSTMP, RMAD_SERIALNO, RMAD_STATUS,Query_Alert_Read from RMADETAIL  left join RMA on RMA.RMA_NO = RMADETAIL.RMAD_RMANO  WHERE  1 = 1 " & text & "   order by RMAD_LUSTMP desc ) t WHERE ROWNUM <= 15 "
                result = val2.ExecuteDT(text2)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                val.Close()
                val.Dispose()
            End Try

            Return result
        End Function
        'Public Function Query_Alert(ByVal CU_NO As String) As RmaDTO.tmpRequest_ListDataTable

        '    Dim i As Integer = 0
        '    Dim sCondition As String = ""
        '    Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
        '    Dim dt As New DataTable

        '    Dim oConn As New Connection
        '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        '    oConn.Open()
        '    Try

        '        If CU_NO <> "" Then
        '            oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", CU_NO, OracleType.Char)
        '            sCondition = sCondition & " AND RMA_CUNO=:RMA_CUNO"
        '        End If

        '        Dim sSQL As String = " SELECT "
        '        sSQL = sSQL & " RMAD_RMANO, RMAD_WARRANTY, RMAD_SERIALNO, RMAD_STATUS from RMADETAIL  left join RMA on RMA.RMA_NO = RMADETAIL.RMAD_RMANO "
        '        sSQL = sSQL & " WHERE rmad_mark=0 " & sCondition & " order by RMAD_WARRANTY desc "

        '        dt = oQuery.ExecuteDT(sSQL)
        '        Common.TransferDataTable(dt, dtRequest)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        oConn.Close()
        '        oConn.Dispose()
        '    End Try

        '    Return dtRequest
        'End Function

        ''' <summary>
        ''' 查詢 Request RMA 資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="CompNo">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryRMA(ByVal RMANo As String, ByVal CuName As String,
            ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal CompNo As String,
            Optional ByVal OrderBY As String = "", Optional ByRef SQLText As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = CompNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                '狀態
                If Status <> "-1" Then
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    oQuery.addWHERE("RMA_STATUS", ":RMA_STATUS", Status, OracleType.Int16)
                    If Status < 20 Then
                        sCondition = sCondition & " AND RMA_STATUS<=:RMA_STATUS"
                    Else
                        sCondition = sCondition & " AND RMA_STATUS=:RMA_STATUS"
                    End If
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sSQL As String = " SELECT a.RMA_NO, a.RMA_ID, a.RMA_STATUS, "
                sSQL = sSQL & " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY,"
                sSQL = sSQL & " CU_NAME, a.RMA_APPLICANT, a.RMA_CSTMP, "
                sSQL = sSQL & " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMASQ_QUOTE, RMASQ_CURRENCYCODE, b.RMARSD_QUOTE, b.RMARSD_CURRENCYCODE "
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA a ON RMADETAIL.RMAD_RMANO = a.RMA_NO "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT RMA_NO,CU_NAME FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL b ON RMADETAIL.RMAD_ID = b.RMASMD_RMADID "
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY


                ' SQLText = sSQL
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

        ''' <summary>
        ''' 查詢 Request RMA 已維修的資料
        ''' </summary>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="CuName">客戶名稱</param>
        ''' <param name="Repair">維修中心代碼</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="CompNo">登入者維修中心代碼</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QueryRMAByRepairList(ByVal RMANo As String, ByVal CuName As String,
            ByVal Repair As String, ByVal Status As String, ByVal fdate As String, ByVal edate As String, ByVal CompNo As String,
            Optional ByVal OrderBY As String = "", Optional ByRef SQLText As String = "") As RmaDTO.tmpRequest_ListDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRequest As New RmaDTO.tmpRequest_ListDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_CSTMP DESC"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                Dim sCondition_Repair As String = ""
                sCondition = sCondition & " AND ("
                Dim arrRepair() As String = CompNo.Split(",")
                For i = 0 To arrRepair.Length - 1
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)

                    If sCondition_Repair.Trim <> "" Then
                        sCondition_Repair = sCondition_Repair & " OR "
                    End If
                    sCondition_Repair = sCondition_Repair & " RMA_COMPNO =:RMA_COMPNO" & i.ToString & " OR RMAR_COMPNO =:RMAR_COMPNO" & i.ToString
                Next
                sCondition = sCondition & sCondition_Repair & ")"


                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_RMANO) like :RMAD_RMANO"
                End If

                If CuName.ToString().Trim() <> "" Then
                    CuName = "%" & CuName.Trim() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CuName.ToLower(), OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                '維修點
                If Repair <> "-1" Then
                    oQuery.addWHERE("RMA_COMPNO", ":RMA_COMPNO", Repair, OracleType.VarChar)
                    oQuery.addWHERE("RMAR_COMPNO", ":RMAR_COMPNO", Repair, OracleType.VarChar)

                    sCondition = sCondition & " AND (RMA_COMPNO=:RMA_COMPNO"
                    sCondition = sCondition & " OR RMAR_COMPNO=:RMAR_COMPNO)"
                End If

                '狀態
                If Status <> "-1" Then
                    '0.Requested 暫存階段,  10.Requested, 20. in Progress, 90.Closed, 91.Canceled
                    oQuery.addWHERE("RMA_STATUS", ":RMA_STATUS", Status, OracleType.Int16)
                    If Status < 20 Then
                        sCondition = sCondition & " AND RMA_STATUS<=:RMA_STATUS"
                    Else
                        sCondition = sCondition & " AND RMA_STATUS=:RMA_STATUS"
                    End If
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim sSQL As String = " SELECT a.RMA_NO, a.RMA_ID, a.RMA_STATUS, "
                sSQL = sSQL & " RMAD_RMANO, RMAD_ID, RMAD_MODELNO, RMAD_SERIALNO, RMAD_WARRANTY, RMAD_CSTMP, RMAD_STATUS, RMAD_ISWARRANTY,"
                sSQL = sSQL & " CU_NAME, a.RMA_APPLICANT, a.RMA_CSTMP, "
                sSQL = sSQL & " RMARQ_QUOTE, RMARQ_CURRENCYCODE, RMARQ_ASSIGEQUOTE, RMARQ_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMAR_QUOTE, RMAR_CURRENCYCODE, RMAR_ASSIGEQUOTE, RMAR_ASSIGECURRENCYCODE, "
                sSQL = sSQL & " RMAR_REPAIRAD"
                'sSQL = sSQL & " , RMASQ_QUOTE, RMASQ_CURRENCYCODE, b.RMARSD_QUOTE, b.RMARSD_CURRENCYCODE "
                sSQL = sSQL & " FROM RMADETAIL "
                sSQL = sSQL & " INNER JOIN RMA a ON RMADETAIL.RMAD_RMANO = a.RMA_NO "
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ( "
                sSQL = sSQL & " SELECT RMA_NO,CU_NAME FROM RMA "
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO "
                sSQL = sSQL & " ) vwCustomer "
                sSQL = sSQL & " ON RMADETAIL.RMAD_RMANO = vwCustomer.RMA_NO "
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID "
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMADETAIL.RMAD_ID = RMAREPAIR.RMAR_RMADID "
                sSQL = sSQL & " INNER JOIN RMASALE_QUOTED ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID "
                'sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL b ON RMADETAIL.RMAD_ID = b.RMASMD_RMADID "
                sSQL = sSQL & " WHERE 1=1" & sCondition & OrderBY


                ' SQLText = sSQL
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

        ''' <summary>
        ''' 取得給客戶看的RMA詳細資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMAD_ID">傳入RMAD_ID</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        Public Function QueryByClientDetail(ByVal LanguageID As String, ByVal RMAD_ID As String) As RmaDTO.tmpClientDetailDataTable
            Dim sCondition As String = ""
            Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " RMAD_ID=:RMAD_ID"

                Dim sSQL As String = getQueryByClient_SQL(LanguageID, sCondition)

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClientDetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClientDetail
        End Function

        ''' <summary>
        ''' 取得給客戶看的RMA詳細資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMA_NO">傳入RMAD_NO</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        Public Function QueryByClient(ByVal LanguageID As String, ByVal RMA_NO As String) As RmaDTO.tmpClientDetailDataTable
            Dim sCondition As String = ""
            Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMAD_RMANO=:RMAD_RMANO"

                Dim sSQL As String = getQueryByClient_SQL(LanguageID, sCondition)

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClientDetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClientDetail
        End Function

        ''' <summary>
        ''' 取得要Reject清單資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMA_NO_List">傳入RMAD_NO</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        ''' 逾期超過1個月的RMA報價單進行強制取消作業 by buck add 20251201
        Public Function QueryRejectListData(ByVal LanguageID As String, ByVal RMA_NO_List As String) As DataTable
            Dim sCondition As String = ""
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                sCondition = String.Format(" RMAD_RMANO in ({0})", RMA_NO_List)

                Dim sSQL As String = getQueryByClient_SQL(LanguageID, sCondition)

                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' 取得給客戶看的RMA 列表資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMA_CUNO">客戶編號</param>
        ''' <param name="RMA_ACCOUNTID">客戶帳號</param>
        ''' <param name="RMA_NO">傳入RMAD_NO</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        Public Function QueryStatus(ByVal LanguageID As String, ByVal RMA_CUNO As String, ByVal RMA_ACCOUNTID As String,
            ByVal RMA_NO As String, ByVal Status As Integer,
            ByVal fdate As String, ByVal edate As String, Optional ByVal OrderBY As String = "") As RmaDTO.tmpClientDetailDataTable

            Dim sCondition As String = ""
            Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                sCondition = " 1=1 "

                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", RMA_CUNO, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_CUNO =:RMA_CUNO"

                oQuery.addWHERE("RMA_ACCOUNTID", ":RMA_ACCOUNTID", RMA_ACCOUNTID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ACCOUNTID =:RMA_ACCOUNTID"

                If RMA_NO.Trim <> "" Then
                    oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMA_NO, OracleType.VarChar)
                    sCondition = sCondition & " AND RMAD_RMANO=:RMAD_RMANO"
                End If

                If Status <> -1 Then
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                    '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled

                    '10:New Request, 20:Processing, 70:Shipped
                    Select Case Status
                        Case 10
                            oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                            sCondition = sCondition & " AND RMAD_STATUS<=:RMAD_STATUS"

                        Case 20
                            oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS1", 20, OracleType.Int16)
                            oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS2", 70, OracleType.Int16)
                            sCondition = sCondition & " AND RMAD_STATUS>=:RMAD_STATUS1 AND RMAD_STATUS<=:RMAD_STATUS2"

                        Case 90
                            oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                            sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
                    End Select
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If


                Dim sSQL As String = getQueryByClient_SQL(LanguageID, sCondition)
                sSQL = sSQL & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClientDetail)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClientDetail
        End Function

        ''' <summary>
        ''' 取得給客戶看的RMA 列表資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMA_CUNO">客戶編號</param>
        ''' <param name="RMA_ACCOUNTID">客戶帳號</param>
        ''' <param name="RMA_NO">傳入RMAD_NO</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        Public Function QueryStatus_ALL(ByVal LanguageID As String, ByVal RMA_CUNO As String, ByVal RMA_ACCOUNTID As String, ByVal RMA_NO As String, ByVal Status As Integer, ByVal fdate As String, ByVal edate As String, ByVal Optional OrderBY As String = "") As RmaDTO.tmpClientDetailDataTable
            Dim text As String = ""
            Dim tmpClientDetailDataTable As RmaDTO.tmpClientDetailDataTable = New RmaDTO.tmpClientDetailDataTable()
            Dim dataTable As DataTable = New DataTable()
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            connection.Open()

            Try

                If Operators.CompareString(OrderBY.Trim(), "", TextCompare:=False) = 0 Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If

                OrderBY = " ORDER BY " & OrderBY
                text = " 1=1 "
                query.addWHERE("RMA_CUNO", ":RMA_CUNO", RMA_CUNO, OracleType.VarChar)
                text += " AND RMA_CUNO =:RMA_CUNO"
                query.addWHERE("RMA_ACCOUNTID", ":RMA_ACCOUNTID", RMA_ACCOUNTID, OracleType.VarChar)
                text += " AND RMA_ACCOUNTID =:RMA_ACCOUNTID"

                If Operators.CompareString(RMA_NO.Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMA_NO, OracleType.VarChar)
                    text += " AND ( CUSTOMER_PRODUCT_NUMBER=:RMAD_RMANO  or   RMAD_RMANO=:RMAD_RMANO )  "
                End If

                If Status <> -1 Then
                    query.addWHERE("RMA_STATUS", ":RMA_STATUS", Status, OracleType.Int16)
                    text += " AND RMA_STATUS=:RMA_STATUS"
                End If

                If (Operators.CompareString(fdate.Trim(), "", TextCompare:=False) <> 0) And (Operators.CompareString(edate.Trim(), "", TextCompare:=False) <> 0) Then
                    Dim obj As Object = Convert.ToDateTime(fdate)
                    Dim obj2 As Object = Convert.ToDateTime(edate).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RuntimeHelpers.GetObjectValue(obj), OracleType.DateTime)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RuntimeHelpers.GetObjectValue(obj2), OracleType.DateTime)
                    text += " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim queryByClient_SQL As String = getQueryByClient_SQL(LanguageID, text)
                queryByClient_SQL += OrderBY
                dataTable = query.ExecuteDT(queryByClient_SQL)
                Dim dtSource As DataTable = dataTable
                Dim dtTarget As DataTable = tmpClientDetailDataTable
                Common.TransferDataTable(dtSource, dtTarget)
                tmpClientDetailDataTable = CType(dtTarget, RmaDTO.tmpClientDetailDataTable)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return tmpClientDetailDataTable
        End Function

        ''' <summary>
        ''' 取得給客戶看的RMA 列表資料
        ''' </summary>
        ''' <param name="LanguageID">語系代碼</param>
        ''' <param name="RMA_CUNO">客戶編號</param>
        ''' <param name="RMA_ACCOUNTID">客戶帳號</param>
        ''' <param name="RMA_NO">傳入RMAD_NO</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>回傳tmpClientDetail</returns>
        ''' <remarks></remarks>
        Public Function QueryStatus_Customer_Number(ByVal LanguageID As String, ByVal RMA_CUNO As String, ByVal RMA_ACCOUNTID As String, ByVal RMA_NO As String, ByVal Status As Integer, ByVal fdate As String, ByVal edate As String, ByVal Optional OrderBY As String = "") As RmaDTO.tmpClientDetailDataTable
            Dim text As String = ""
            Dim tmpClientDetailDataTable As RmaDTO.tmpClientDetailDataTable = New RmaDTO.tmpClientDetailDataTable()
            Dim dataTable As DataTable = New DataTable()
            Dim connection As Connection = New Connection()
            Dim query As Query = New Query(connection)
            connection.Open()

            Try

                If Operators.CompareString(OrderBY.Trim(), "", TextCompare:=False) = 0 Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If

                OrderBY = " ORDER BY " & OrderBY
                text = " 1=1 "
                query.addWHERE("RMA_CUNO", ":RMA_CUNO", RMA_CUNO, OracleType.VarChar)
                text += " AND RMA_CUNO =:RMA_CUNO"
                query.addWHERE("RMA_ACCOUNTID", ":RMA_ACCOUNTID", RMA_ACCOUNTID, OracleType.VarChar)
                text += " AND RMA_ACCOUNTID =:RMA_ACCOUNTID"

                If Operators.CompareString(RMA_NO.Trim(), "", TextCompare:=False) <> 0 Then
                    query.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMA_NO, OracleType.VarChar)
                    text += " AND CUSTOMER_PRODUCT_NUMBER=:RMAD_RMANO"
                End If

                If Status <> -1 Then
                    query.addWHERE("RMA_STATUS", ":RMA_STATUS", Status, OracleType.Int16)
                    text += " AND RMA_STATUS=:RMA_STATUS"
                End If

                If (Operators.CompareString(fdate.Trim(), "", TextCompare:=False) <> 0) And (Operators.CompareString(edate.Trim(), "", TextCompare:=False) <> 0) Then
                    Dim obj As Object = Convert.ToDateTime(fdate)
                    Dim obj2 As Object = Convert.ToDateTime(edate).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RuntimeHelpers.GetObjectValue(obj), OracleType.DateTime)
                    query.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RuntimeHelpers.GetObjectValue(obj2), OracleType.DateTime)
                    text += " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"
                End If

                Dim queryByClient_SQL As String = getQueryByClient_SQL(LanguageID, text)
                queryByClient_SQL += OrderBY
                dataTable = query.ExecuteDT(queryByClient_SQL)
                Dim dtSource As DataTable = dataTable
                Dim dtTarget As DataTable = tmpClientDetailDataTable
                Common.TransferDataTable(dtSource, dtTarget)
                tmpClientDetailDataTable = CType(dtTarget, RmaDTO.tmpClientDetailDataTable)
            Catch ex As Exception
                ProjectData.SetProjectError(ex)
                Dim ex2 As Exception = ex
                Throw ex2
            Finally
                connection.Close()
                connection.Dispose()
            End Try

            Return tmpClientDetailDataTable
        End Function

        Private Function getQueryByClient_SQL(ByVal LanguageID As String, ByVal sCondition As String) As String
            Dim sSQL As String = "SELECT RMA_NO, RMA_ID, RMA_CSTMP, RMA_APPLICANT, RMA_Remark, "

            sSQL = sSQL & " RMAD_ID, RMAD_RMANO ,RMAD_SERIALNO,RMAD_MODELNO, RMAD_RECEVSTATUS,RMAD_UPLOADFILE, RMAD_STATUS, RMAD_WARRANTY, RMAD_ISWARRANTY, RMAD_CSTMP, "
            '需求新增:BI保固 By buck Add 20250902 begin
            'sSQL = sSQL & " RMAD_FARFARCNO, FARC1.FARC_NAME FARC_NAME1, RMAD_PRODUCTDESC, RMAD_PROBLEMDESC, "
            sSQL = sSQL & " RMAD_FARFARCNO, FARC1.FARC_NAME FARC_NAME1, RMAD_PRODUCTDESC, RMAD_PROBLEMDESC, RMAD_APPLY_BI, "
            '需求新增:BI保固 By buck Add 20250902 end

            sSQL = sSQL & " RMAR_FARCNO, FARC_NAME2, RMAR_PROBLEMDESC, RMAR_REPAIRDESC, RMAR_REPAIRMEMO,"

            sSQL = sSQL & " RMARQ_ID, RMARQ_IMPROPERUSAGE, "

            sSQL = sSQL & " RMASQ_LABORCOST, RMASQ_MATERIALCOST, RMASQ_QUOTE, RMASQ_CURRENCYCODE, RMASQ_CURRENCYRATE,"

            sSQL = sSQL & " RMARSD_LABORCOST, RMARSD_MATERIALCOST, RMARSD_QUOTE, RMARSD_CURRENCYCODE, RMARSD_CURRENCYRATE,"
            sSQL = sSQL & " TrackingNo, ShippedDate,RMAD_PARTSN,DECODE(NVL(RMARQ_COMPNO,' '),' ',RMA_COMPNO,RMARQ_COMPNO) RMARQ_COMPNO,RMA_ACCOUNTID "
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
            sSQL = sSQL & " ,CUSTOMER.CU_SERVICE_CHG_DISCOUNT"
            '新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
            sSQL = sSQL & " FROM "
            sSQL = sSQL & " RMA INNER JOIN RMADETAIL"
            sSQL = sSQL & " ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND RMA_MARK=0 AND RMAD_MARK=0"

            sSQL = sSQL & " LEFT OUTER JOIN FAILUREREASONSCLASS FARC1"
            sSQL = sSQL & " ON RMADETAIL.rmad_farfarcno = FARC1.FARC_NO AND FARC1.FARC_DFLNO='" & LanguageID.Trim() & "'"

            sSQL = sSQL & " LEFT OUTER JOIN "
            sSQL = sSQL & " ( "
            sSQL = sSQL & "     SELECT RMAR_RMADID, RMAR_FARCNO, FARC2.FARC_NAME FARC_NAME2 ,RMAR_PROBLEMDESC, RMAR_REPAIRDESC, RMAR_REPAIRMEMO"
            sSQL = sSQL & "     FROM  RMAREPAIR LEFT OUTER JOIN FAILUREREASONSCLASS FARC2"
            sSQL = sSQL & "     ON RMAREPAIR.RMAR_FARCNO = FARC2.FARC_NO AND FARC2.FARC_DFLNO='" & LanguageID.Trim() & "'"
            sSQL = sSQL & " ) vwRMAREPAIR"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwRMAREPAIR.RMAR_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN RMAREPAIR_QUOTED"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = RMAREPAIR_QUOTED.RMARQ_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = RMASALE_QUOTED.RMASQ_RMADID and RMASQ_SALEAD is not null"

            sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID"

            sSQL = sSQL & " LEFT OUTER JOIN"
            'sSQL = sSQL & " ("
            'sSQL = sSQL & "     SELECT RMASMD_RMANO, RMASMD_RMADID, RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate"
            'sSQL = sSQL & "     FROM RMA_SHIPPING "
            'sSQL = sSQL & "     INNER JOIN RMA_SHIPPINGDETAIL ON RMA_SHIPPING.RMASH_ID = RMA_SHIPPINGDETAIL.RMASHD_RMASHID "
            'sSQL = sSQL & "     INNER JOIN RMA_SHIPMENTDETAIL ON RMA_SHIPPINGDETAIL.RMASHD_RMANO = RMA_SHIPMENTDETAIL.RMASMD_RMANO"
            'sSQL = sSQL & " ) vwSHIPPING"

            sSQL = sSQL & " ("
            sSQL = sSQL & "     SELECT RMASMD_RMANO, RMASMD_RMADID, RMASH_TRACKINGNO TrackingNo, to_char(RMASH_CSTMP,'yyyy/mm/dd') ShippedDate"
            sSQL = sSQL & "     FROM RMA_SHIPMENTDETAIL "
            sSQL = sSQL & "     inner join RMA_SHIPMENT on RMASMD_RMASMID = RMASM_ID "
            sSQL = sSQL & "     inner JOIN RMA_SHIPPINGDETAIL on RMASHD_RMANO = RMASMD_RMANO AND RMASHD_RMASMPACKINGNO = RMASM_PACKINGNO"
            sSQL = sSQL & "     inner join RMA_SHIPPING on RMASHD_RMASHID = RMASH_ID "
            sSQL = sSQL & " ) vwSHIPPING"
            sSQL = sSQL & " ON RMADETAIL.RMAD_ID = vwSHIPPING.RMASMD_RMADID"
            ''新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin
            sSQL = sSQL & " Left Join CUSTOMER on RMA.RMA_CUNO = CUSTOMER.CU_NO"
            ''新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end
            sSQL = sSQL & " WHERE " & sCondition

            Return sSQL
        End Function

        ''' <summary>
        ''' 取得給客戶看的RMA 列表資料
        ''' </summary>
        ''' <param name="RMA_CUNO">客戶編號</param>
        ''' <param name="RMA_ACCOUNTID">客戶帳號</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="Status">Status</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwRMADataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByClientLst(ByVal RMA_CUNO As String, ByVal RMA_ACCOUNTID As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
                ByVal Status As Integer, ByVal fdate As String, ByVal edate As String,
                 Optional ByVal OrderBY As String = "") As RmaDTO.tmpClientListDataTable

            Dim dtRMA As New RmaDTO.tmpClientListDataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim dt As New DataTable
            Dim sCondition As String = ""

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " RMAD_RMANO asc, RMAD_SEQ asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("RMA_CUNO", ":RMA_CUNO", RMA_CUNO, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_CUNO =:RMA_CUNO"

                oQuery.addWHERE("RMA_ACCOUNTID", ":RMA_ACCOUNTID", RMA_ACCOUNTID, OracleType.VarChar)
                sCondition = sCondition & " AND RMA_ACCOUNTID =:RMA_ACCOUNTID"

                'ToLower
                If RMANo.Trim <> "" Then
                    RMANo = "%" & RMANo.Trim() & "%"
                    oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMA_NO) like :RMA_NO"
                End If

                If ModelNo.Trim <> "" Then
                    ModelNo = "%" & ModelNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_MODELNO", ":RMAD_MODELNO", ModelNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_MODELNO) like :RMAD_MODELNO"
                End If

                If SerialNo.Trim <> "" Then
                    SerialNo = "%" & SerialNo.Trim() & "%"
                    oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", SerialNo.ToLower(), OracleType.VarChar)
                    sCondition = sCondition & " AND lower(RMAD_SERIALNO) like :RMAD_SERIALNO"
                End If


                If Status <> -1 Then
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    oQuery.addWHERE("RMAD_STATUS", ":RMAD_STATUS", Status, OracleType.Int16)
                    If Status = 10 Then
                        sCondition = sCondition & " AND RMAD_STATUS<=:RMAD_STATUS"
                    Else
                        sCondition = sCondition & " AND RMAD_STATUS=:RMAD_STATUS"
                    End If
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP1", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("RMA_CSTMP", ":RMAD_CSTMP2", RMAD_LUSTMP, OracleType.DateTime)
                    sCondition = sCondition & " AND (RMA_CSTMP >=:RMAD_CSTMP1 AND RMA_CSTMP <=:RMAD_CSTMP2)"

                    'oQuery.addWHERE("RMAD_CSTMP", ":RMAD_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    'oQuery.addWHERE("RMAD_LUSTMP", ":RMAD_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)
                    'sCondition = sCondition & " AND (RMAD_CSTMP >=:RMAD_CSTMP AND RMAD_LUSTMP <=:RMAD_LUSTMP)"
                End If

                Dim sSQL As String = "SELECT RMA_ID, RMA_NO, RMA_CUNO, RMA_ACCOUNTID, RMA_APPLICANT, "
                sSQL = sSQL & " RMA_TEL, RMA_ADDRESS, RMA_COMPNO, RMA_STATUS, RMA_MAIL, "
                sSQL = sSQL & " RMA_AD, RMA_ADNAME, RMA_CSTMP, RMA_LUAD, RMA_LUADNAME, RMA_LUSTMP, RMA_MARK, "
                sSQL = sSQL & " RMAD_ID, RMAD_RMANO, RMAD_MODELNO, RMAD_SERIALNO, RMAD_CUSNAME, RMAD_WARRANTY, "
                sSQL = sSQL & " RMAD_FARFARCNO, RMAD_FARNO, RMAD_UPLOADFILE, RMAD_PROBLEMDESC, RMAD_STATUS, RMAD_ISFILL, "
                sSQL = sSQL & " RMAD_RECVAD, RMAD_RECVADNAME, RMAD_RECVDATE, RMAD_RECEVSTATUS, "
                sSQL = sSQL & " RMAD_AD, RMAD_ADNAME, RMAD_CSTMP, RMAD_LUAD, RMAD_LUADNAME, RMAD_LUSTMP, RMAD_MARK, RMAD_ISWARRANTY,"
                sSQL = sSQL & " CU_NO, CU_NAME, RMASQ_QUOTE, RMARSD_QUOTE,CUSTOMER_PRODUCT_NUMBER"
                sSQL = sSQL & " FROM RMA INNER JOIN RMADETAIL "
                sSQL = sSQL & " ON RMA.RMA_NO = RMADETAIL.RMAD_RMANO AND rma_mark=0 AND rmad_mark=0"
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA.RMA_CUNO = CUSTOMER.CU_NO"
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED"
                sSQL = sSQL & " ON rmad_id = RMASALE_QUOTED.rmasq_rmadid AND RMASQ_SALEAD is not null"
                sSQL = sSQL & " LEFT OUTER JOIN RMA_SHIPMENTDETAIL"
                sSQL = sSQL & " ON RMAD_RMANO = RMA_SHIPMENTDETAIL.RMASMD_RMANO AND RMAD_ID = RMA_SHIPMENTDETAIL.RMASMD_RMADID"
                sSQL = sSQL & " WHERE 1=1 " & sCondition & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRMA)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRMA

        End Function

        '20240702 客戶確認維修 
        Public Sub Bank_ClientQuoted_Confirmed(ByVal dtConfirmed As RmaDTO.ClientQuoted_ConfirmedDataTable)
            Dim i As Integer = 0
            'Dim sqlstr As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '========================================================================================================================================================================================================================================================================================
                '2011/09/16 START
                '調整項目:
                '1.客戶端 wait for processing 按下報價確認按鈕時, 只針對該 RMA單有任一筆item是需要客戶報價確認的item做確認.
                '原Code
                '========================================================================================================================================================================================================================================================================================
                'For i = 0 To dtConfirmed.Rows.Count - 1
                '    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                '    oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                '    oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                '    oExecute.addParameter("RMASQ_CLIENTDATE", Date.Now, OracleType.DateTime)
                '    oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                '    oExecute.addWHERE("RMASQ_ID", dr.RMASQ_ID.ToString().Trim(), OracleType.VarChar)
                '    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)
                'Next

                'Dim oRMAStatus As New ctlRMA.RMAStatus
                'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                'For i = 0 To dtConfirmed.Rows.Count - 1
                '    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                '    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '    Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow

                '    drStatus.RMAD_ID = dr.RMAD_ID
                '    drStatus.RMAD_STATUS = dr.RMAD_STATUS

                '    dtStatus.AddRMADetailStatusRow(drStatus)
                'Next
                'If dtStatus.Rows.Count > 0 Then
                '    Call oRMAStatus.ChangeStatus(oExecute, dtStatus)
                'End If


                '========================================================================================================================================================================================================================================================================================
                '改成 
                '========================================================================================================================================================================================================================================================================================
                For i = 0 To dtConfirmed.Rows.Count - 1
                    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                    'RMASQ_CLIENTCONFIRM --> 1.客戶自行確認, 2.業務帶客戶確認
                    Dim sqlstr As String = ""
                    sqlstr = sqlstr & " UPDATE RMASALE_QUOTED SET "
                    sqlstr = sqlstr & "     RMASQ_CLIENTAD ='" & dr.RMASQ_CLIENTAD.Trim() & "',"
                    sqlstr = sqlstr & "     RMASQ_CLIENTADNAME ='" & dr.RMASQ_CLIENTADNAME.Trim() & "',"

                    If dr.IsRMASQ_QUOTENull = False Then
                        sqlstr = sqlstr & " RMASQ_QUOTE = " & dr.RMASQ_QUOTE & ","
                    End If

                    If dr.IsRMASQ_CLIENTCONFIRMNull = False Then
                        sqlstr = sqlstr & " RMASQ_CLIENTCONFIRM=" & dr.RMASQ_CLIENTCONFIRM & ","
                    End If

                    sqlstr = sqlstr & "     RMASQ_CLIENTDATE =sysdate"
                    sqlstr = sqlstr & " WHERE RMASQ_RMADID IN "
                    sqlstr = sqlstr & " ("
                    sqlstr = sqlstr & "   SELECT RMAD_ID FROM RMADetail "
                    sqlstr = sqlstr & "   WHERE RMAD_ID='" & dr.RMAD_ID.Trim() & "' AND RMAD_STATUS=40 "
                    sqlstr = sqlstr & " )"


                    Dim oCommand As OracleCommand = oConn.Command()
                    oCommand.CommandText = sqlstr
                    oCommand.ExecuteNonQuery()


                    'oExecute.addParameter("RMAD_STATUS", 50, OracleType.Int16)
                    oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)

                    oExecute.addParameter("RMAD_LUAD", dr.RMASQ_CLIENTAD.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", dr.RMASQ_CLIENTADNAME.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)

                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                Next
                '========================================================================================================================================================================================================================================================================================
                '2011/09/16 END
                '========================================================================================================================================================================================================================================================================================


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
        ''' 客戶報價確認
        ''' </summary>
        ''' <param name="dtConfirmed">傳入ClientQuoted_ConfirmedDataTable</param>
        ''' <remarks></remarks>
        Public Sub ClientQuoted_Confirmed(ByVal dtConfirmed As RmaDTO.ClientQuoted_ConfirmedDataTable)
            Dim i As Integer = 0
            'Dim sqlstr As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                '========================================================================================================================================================================================================================================================================================
                '2011/09/16 START
                '調整項目:
                '1.客戶端 wait for processing 按下報價確認按鈕時, 只針對該 RMA單有任一筆item是需要客戶報價確認的item做確認.
                '原Code
                '========================================================================================================================================================================================================================================================================================
                'For i = 0 To dtConfirmed.Rows.Count - 1
                '    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                '    oExecute.addParameter("RMASQ_CLIENTAD", dr.RMASQ_CLIENTAD, OracleType.NVarChar)
                '    oExecute.addParameter("RMASQ_CLIENTADNAME", dr.RMASQ_CLIENTADNAME, OracleType.NVarChar)
                '    oExecute.addParameter("RMASQ_CLIENTDATE", Date.Now, OracleType.DateTime)
                '    oExecute.addParameter("RMASQ_CLIENTCONFIRM", dr.RMASQ_CLIENTCONFIRM, OracleType.Int16)

                '    oExecute.addWHERE("RMASQ_ID", dr.RMASQ_ID.ToString().Trim(), OracleType.VarChar)
                '    oExecute.Command("RMASALE_QUOTED", Execute.eumCommandType.UPDATE)
                'Next

                'Dim oRMAStatus As New ctlRMA.RMAStatus
                'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable
                'For i = 0 To dtConfirmed.Rows.Count - 1
                '    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                '    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                '    Dim drStatus As RmaDTO.RMADetailStatusRow = dtStatus.NewRMADetailStatusRow

                '    drStatus.RMAD_ID = dr.RMAD_ID
                '    drStatus.RMAD_STATUS = dr.RMAD_STATUS

                '    dtStatus.AddRMADetailStatusRow(drStatus)
                'Next
                'If dtStatus.Rows.Count > 0 Then
                '    Call oRMAStatus.ChangeStatus(oExecute, dtStatus)
                'End If


                '========================================================================================================================================================================================================================================================================================
                '改成 
                '========================================================================================================================================================================================================================================================================================
                For i = 0 To dtConfirmed.Rows.Count - 1
                    Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.Rows(i)

                    'RMASQ_CLIENTCONFIRM --> 1.客戶自行確認, 2.業務帶客戶確認
                    Dim sqlstr As String = ""
                    sqlstr = sqlstr & " UPDATE RMASALE_QUOTED SET "
                    sqlstr = sqlstr & "     RMASQ_CLIENTAD ='" & dr.RMASQ_CLIENTAD.Trim() & "',"
                    sqlstr = sqlstr & "     RMASQ_CLIENTADNAME ='" & dr.RMASQ_CLIENTADNAME.Trim() & "',"

                    If dr.IsRMASQ_LABORCOSTNull = False Then
                        sqlstr = sqlstr & " RMASQ_LABORCOST = " & dr.RMASQ_LABORCOST & ","
                    End If

                    If dr.IsRMASQ_QUOTENull = False Then
                        sqlstr = sqlstr & " RMASQ_QUOTE = " & dr.RMASQ_QUOTE & ","
                    End If

                    If dr.IsRMASQ_CLIENTCONFIRMNull = False Then
                        sqlstr = sqlstr & " RMASQ_CLIENTCONFIRM=" & dr.RMASQ_CLIENTCONFIRM & ","
                    End If

                    sqlstr = sqlstr & "     RMASQ_CLIENTDATE =sysdate"
                    sqlstr = sqlstr & " WHERE RMASQ_RMADID IN "
                    sqlstr = sqlstr & " ("
                    sqlstr = sqlstr & "   SELECT RMAD_ID FROM RMADetail "
                    sqlstr = sqlstr & "   WHERE RMAD_ID='" & dr.RMAD_ID.Trim() & "' AND RMAD_STATUS=40 "
                    sqlstr = sqlstr & " )"


                    Dim oCommand As OracleCommand = oConn.Command()
                    oCommand.CommandText = sqlstr
                    oCommand.ExecuteNonQuery()


                    'oExecute.addParameter("RMAD_STATUS", 50, OracleType.Int16)
                    oExecute.addParameter("RMAD_STATUS", dr.RMAD_STATUS, OracleType.Int16)

                    oExecute.addParameter("RMAD_LUAD", dr.RMASQ_CLIENTAD.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", dr.RMASQ_CLIENTADNAME.Trim(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("RMAD_ID", dr.RMAD_ID, OracleType.VarChar)
                    oExecute.addWHERE("RMAD_STATUS", 40, OracleType.Int16)
                    oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)
                Next
                '========================================================================================================================================================================================================================================================================================
                '2011/09/16 END
                '========================================================================================================================================================================================================================================================================================


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
        ''' 客戶整批取消維修
        ''' </summary>
        ''' <param name="dtCancel"></param>
        ''' <remarks></remarks>
        Public Sub Client_Cancel(ByVal dtCancel As RmaDTO.Client_CancelDataTable)
            Dim i As Integer = 0
            Dim sqlstr As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                For i = 0 To dtCancel.Rows.Count - 1
                    Dim dr As RmaDTO.Client_CancelRow = dtCancel.Rows(i)
                    'RMADetail Table
                    'oExecute.addParameter("RMAD_STATUS", 91, OracleType.Int16)

                    'oExecute.addParameter("RMAD_LUAD", dr.RMA_CLIENTAD.Trim(), OracleType.NVarChar)
                    'oExecute.addParameter("RMAD_LUADNAME", dr.RMA_CLIENTADNAME.Trim(), OracleType.NVarChar)
                    'oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)

                    'oExecute.addWHERE("RMAD_RMANO", dr.RMA_NO, OracleType.VarChar)
                    'oExecute.addWHERE("RMAD_STATUS", 40, OracleType.Int16)
                    'oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)

                    sqlstr = "UPDATE RMADetail SET " &
                            " RMAD_STATUS=91," &
                            " RMAD_LUAD = '" & dr.RMA_CLIENTAD.Trim() & "'," &
                            " RMAD_LUADNAME = '" & dr.RMA_CLIENTADNAME.Trim() & "'," &
                            " RMAD_LUSTMP = sysdate" &
                            " WHERE RMAD_RMANO ='" & dr.RMA_NO & "' AND (RMAD_STATUS=40 OR RMAD_STATUS=0)"

                    Dim oCommand As OracleCommand = oConn.Command()
                    oCommand.CommandText = sqlstr
                    oCommand.ExecuteNonQuery()


                    'RMA Table
                    oExecute.addParameter("RMA_STATUS", 91, OracleType.Int16)
                    oExecute.addWHERE("RMA_NO", dr.RMA_NO, OracleType.VarChar)
                    oExecute.Command("RMA", Execute.eumCommandType.UPDATE)
                Next
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
        ''' 取得報價表頭 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <remarks></remarks>
        Public Function QryClient_SalesQuoted_AM0180200003439(ByVal RMA_NO As String) As RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try


                sSQL = sSQL & " SELECT RMA_NO, RMA_APPLICANT, RMA_TEL, RMA_ADDRESS, CU_NAME, RMA_COMPNO, TO_CHAR(RMAD_RECVDATE, 'YYYY/MM/DD') as RMAD_RECVDATE,  TO_CHAR(sysdate, 'YYYY/MM/DD HH24:MI') as PrintDate, "
                sSQL = sSQL & " vwSalesQuoted.RMASQ_QUOTE, TO_CHAR(vwSalesQuoted.SALES_DATE, 'YYYY/MM/DD') as SALES_DATE, "
                sSQL = sSQL & "     nvl(AcceptCount,0) as AcceptCount, nvl(RejectCount,0) as RejectCount, "
                sSQL = sSQL & " RMACHARGE_QUOTED.RMACQ_DISCOUNT, RMACHARGE_QUOTED.RMACQ_CHARGEQUOTE, RMACHARGE_QUOTED.RMACQ_QUOTE_ORIGINAL,vwSalesQuoted.RMASQ_CURRENCYCODE AS Currency_NO "

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_CUNO = CU_NO"

                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, min(RMAD_RECVDATE) as RMAD_RECVDATE"
                sSQL = sSQL & "     FROM RMADETAIL WHERE RMAD_MARK=0 AND RMAD_RECEVSTATUS=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwRecDate"
                sSQL = sSQL & " ON RMAD_RMANO = RMA_NO"

                ''是否要維修: 1.Accept, 2.Reject
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO,RMASQ_CURRENCYCODE, sum(RMASQ_QUOTE) as RMASQ_QUOTE, min(RMASQ_SALEDATE) as SALES_DATE"
                sSQL = sSQL & "     FROM RMADETAIL, RMASALE_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMASQ_RMADID " 'AND RMAD_STATUS<>91
                sSQL = sSQL & "     GROUP BY RMAD_RMANO,RMASQ_CURRENCYCODE "
                sSQL = sSQL & " ) vwSalesQuoted"
                sSQL = sSQL & " ON RMA_NO = vwSalesQuoted.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as AcceptCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwAccept"
                sSQL = sSQL & " ON RMA_NO = vwAccept.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as RejectCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=2"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwReject"
                sSQL = sSQL & " ON RMA_NO = vwReject.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED "
                sSQL = sSQL & " ON RMACHARGE_QUOTED.RMACQ_RMANO = RMA.RMA_NO "



                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClient_SalesQuoted_Head)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClient_SalesQuoted_Head
        End Function

        ''' <summary>
        ''' 取得報價表頭 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <remarks></remarks>
        Public Function QryClient_SalesQuoted_Head(ByVal RMA_NO As String) As RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtClient_SalesQuoted_Head As New RmaDTO.VWRPTCLIENT_SALESQUOTED_HEADDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                sSQL = sSQL & " SELECT RMA_NO, RMA_APPLICANT, RMA_TEL, RMA_ADDRESS, CU_NAME, RMA_COMPNO, TO_CHAR(RMAD_RECVDATE, 'YYYY/MM/DD') as RMAD_RECVDATE,  TO_CHAR(sysdate, 'YYYY/MM/DD HH24:MI') as PrintDate, "
                sSQL = sSQL & " vwSalesQuoted.RMASQ_QUOTE, TO_CHAR(vwSalesQuoted.SALES_DATE, 'YYYY/MM/DD') as SALES_DATE, "
                sSQL = sSQL & "     nvl(AcceptCount,0) as AcceptCount, nvl(RejectCount,0) as RejectCount, "
                sSQL = sSQL & " RMACHARGE_QUOTED.RMACQ_DISCOUNT, RMACHARGE_QUOTED.RMACQ_CHARGEQUOTE, RMACHARGE_QUOTED.RMACQ_QUOTE_ORIGINAL,vwSalesQuoted.RMASQ_CURRENCYCODE AS Currency_NO "

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_CUNO = CU_NO"

                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, min(RMAD_RECVDATE) as RMAD_RECVDATE"
                sSQL = sSQL & "     FROM RMADETAIL WHERE RMAD_MARK=0 AND RMAD_RECEVSTATUS=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwRecDate"
                sSQL = sSQL & " ON RMAD_RMANO = RMA_NO"

                ''是否要維修: 1.Accept, 2.Reject
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO,RMASQ_CURRENCYCODE, sum(RMASQ_QUOTE) as RMASQ_QUOTE, min(RMASQ_SALEDATE) as SALES_DATE"
                sSQL = sSQL & "     FROM RMADETAIL, RMASALE_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMASQ_RMADID " 'AND RMAD_STATUS<>91
                sSQL = sSQL & "     GROUP BY RMAD_RMANO,RMASQ_CURRENCYCODE "
                sSQL = sSQL & " ) vwSalesQuoted"
                sSQL = sSQL & " ON RMA_NO = vwSalesQuoted.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as AcceptCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwAccept"
                sSQL = sSQL & " ON RMA_NO = vwAccept.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as RejectCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=2"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwReject"
                sSQL = sSQL & " ON RMA_NO = vwReject.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED "
                sSQL = sSQL & " ON RMACHARGE_QUOTED.RMACQ_RMANO = RMA.RMA_NO "

                sSQL = sSQL & " WHERE " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClient_SalesQuoted_Head)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClient_SalesQuoted_Head
        End Function

        ''' <summary>
        ''' 取得報價 SN 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryClient_SalesQuoted_SN_001(ByVal RMA_NO As String) As RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                'sSQL = sSQL & " SELECT RMA_NO, RMAD_ID , RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, "
                sSQL = sSQL & " SELECT RMA_NO, RMAD_ID ,"

                sSQL = sSQL & " Case WHEN RMAD_PARTSN = ''  THEN RMAD_SERIALNO"
                sSQL = sSQL & " WHEN RMAD_PARTSN Is Null THEN RMAD_SERIALNO"
                sSQL = sSQL & "  Else"
                sSQL = sSQL & "  RMAD_PARTSN"
                sSQL = sSQL & "   End  RMAD_SERIALNO"

                sSQL = sSQL & ", FN_GETMMODELNO(RMAD_MODELNO,RMA_COMPNO,RMA_ACCOUNTID) RMAD_MODELNO, RMAD_STATUS, "
                sSQL = sSQL & "     RMAR_PROBLEMDESC, RMAR_REPAIRDESC,"
                sSQL = sSQL & "     RMARQ_IMPROPERUSAGE, null as RMARQ_IMPROPERUSAGE_Text, "
                sSQL = sSQL & "     RMAD_ISWARRANTY, null as RMAD_ISWARRANTY_Text,"
                sSQL = sSQL & "     RMARQ_ACCEPT, null as RMARQ_ACCEPT,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE "
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_LABORCOST END"
                'sSQL = sSQL & "     END RMASQ_LABORCOST,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE"
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_MATERIALCOST END"
                'sSQL = sSQL & "     END RMASQ_MATERIALCOST, "

                'sSQL = sSQL & "     CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_QUOTE END RMASQ_QUOTE, "

                'sSQL = sSQL & "     nvl(RMASQ_LABORCOST,0) as RMASQ_LABORCOST, "
                ''sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_LABORCOST,0) END RMASQ_LABORCOST, "
                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_MATERIALCOST,0) END RMASQ_MATERIALCOST, "
                ''sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_QUOTE,0) END RMASQ_QUOTE, "
                'sSQL = sSQL & "     nvl(RMASQ_QUOTE,0) as RMASQ_QUOTE, "

                sSQL = sSQL & "     CASE WHEN RMACQSN_LABORCOST is not null THEN RMACQSN_LABORCOST ELSE nvl(RMASQ_LABORCOST,0) END as RMASQ_LABORCOST,"
                sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE "
                sSQL = sSQL & "         CASE WHEN RMACQSN_MATERIALCOST is not null THEN RMACQSN_MATERIALCOST  ELSE nvl(RMASQ_MATERIALCOST,0) END END as RMASQ_MATERIALCOST,"
                sSQL = sSQL & "     CASE WHEN RMACQSN_QUOTE is not null THEN RMACQSN_QUOTE ELSE nvl(RMASQ_QUOTE,0) END as RMASQ_QUOTE,"

                sSQL = sSQL & "     nvl(RMASQ_CURRENCYCODE,' ') as RMASQ_CURRENCYCODE, "
                sSQL = sSQL & "     nvl(RMASQ_CURRENCYRATE , 0) as RMASQ_CURRENCYRATE, "

                sSQL = sSQL & "     nvl(RMACHARGE_QUOTED_SN.RMACQSN_QUOTE , 0) - nvl(RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT , 0) as chargeQuoted , "
                sSQL = sSQL & "     RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT "
                'sSQL = sSQL & "     nvl(RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT,0) as RMACQSN_DISCOUNTAMOUNT "

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0"
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMAD_ID=RMAR_RMADID"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID"

                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMAD_ID = RMASQ_RMADID"
                sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMAD_ID = RMACHARGE_QUOTED_SN.RMACQSN_RMADID"

                sSQL = sSQL & " WHERE " & sCondition
                sSQL = sSQL & " ORDER BY RMARQ_CSTMP asc"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClient_SalesQuoted_SN)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClient_SalesQuoted_SN
        End Function

        ''' <summary>
        ''' 取得報價 SN 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryClient_SalesQuoted_SN(ByVal RMA_NO As String) As RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtClient_SalesQuoted_SN As New RmaDTO.VWRPTCLIENT_SALESQUOTED_SNDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                'sSQL = sSQL & " SELECT RMA_NO, RMAD_ID , RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, "
                sSQL = sSQL & " SELECT RMA_NO, RMAD_ID , RMAD_SERIALNO, FN_GETMMODELNO(RMAD_MODELNO,RMA_COMPNO,RMA_ACCOUNTID) RMAD_MODELNO, RMAD_STATUS, "
                sSQL = sSQL & "     RMAR_PROBLEMDESC, RMAR_REPAIRDESC,"
                sSQL = sSQL & "     RMARQ_IMPROPERUSAGE, null as RMARQ_IMPROPERUSAGE_Text, "
                sSQL = sSQL & "     RMAD_ISWARRANTY, null as RMAD_ISWARRANTY_Text,"
                sSQL = sSQL & "     RMARQ_ACCEPT, null as RMARQ_ACCEPT,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE "
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_LABORCOST END"
                'sSQL = sSQL & "     END RMASQ_LABORCOST,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE"
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_MATERIALCOST END"
                'sSQL = sSQL & "     END RMASQ_MATERIALCOST, "

                'sSQL = sSQL & "     CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_QUOTE END RMASQ_QUOTE, "

                'sSQL = sSQL & "     nvl(RMASQ_LABORCOST,0) as RMASQ_LABORCOST, "
                ''sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_LABORCOST,0) END RMASQ_LABORCOST, "
                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_MATERIALCOST,0) END RMASQ_MATERIALCOST, "
                ''sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_QUOTE,0) END RMASQ_QUOTE, "
                'sSQL = sSQL & "     nvl(RMASQ_QUOTE,0) as RMASQ_QUOTE, "

                sSQL = sSQL & "     CASE WHEN RMACQSN_LABORCOST is not null THEN RMACQSN_LABORCOST ELSE nvl(RMASQ_LABORCOST,0) END as RMASQ_LABORCOST,"
                sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE "
                sSQL = sSQL & "         CASE WHEN RMACQSN_MATERIALCOST is not null THEN RMACQSN_MATERIALCOST  ELSE nvl(RMASQ_MATERIALCOST,0) END END as RMASQ_MATERIALCOST,"
                sSQL = sSQL & "     CASE WHEN RMACQSN_QUOTE is not null THEN RMACQSN_QUOTE ELSE nvl(RMASQ_QUOTE,0) END as RMASQ_QUOTE,"

                sSQL = sSQL & "     nvl(RMASQ_CURRENCYCODE,' ') as RMASQ_CURRENCYCODE, "
                sSQL = sSQL & "     nvl(RMASQ_CURRENCYRATE , 0) as RMASQ_CURRENCYRATE, "

                sSQL = sSQL & "     nvl(RMACHARGE_QUOTED_SN.RMACQSN_QUOTE , 0) - nvl(RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT , 0) as chargeQuoted , "
                sSQL = sSQL & "     RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT "
                'sSQL = sSQL & "     nvl(RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT,0) as RMACQSN_DISCOUNTAMOUNT "

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0"
                sSQL = sSQL & " INNER JOIN RMAREPAIR ON RMAD_ID=RMAR_RMADID"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID"

                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMAD_ID = RMASQ_RMADID"
                sSQL = sSQL & " LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMAD_ID = RMACHARGE_QUOTED_SN.RMACQSN_RMADID"

                sSQL = sSQL & " WHERE " & sCondition
                sSQL = sSQL & " ORDER BY RMARQ_CSTMP asc"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClient_SalesQuoted_SN)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClient_SalesQuoted_SN
        End Function

        ''' <summary>
        ''' 取得報價 Part 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryClient_SalesQuoted_Part(ByVal RMA_NO As String) As RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtClient_SalesQuoted_Part As New RmaDTO.VWRPTCLIENT_SALESQUOTED_PARTDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                sSQL = sSQL & " SELECT RMA_NO, RMAD_ID, RMARQD_NPARTNO, RMARQD_DESC, RMARQD_QTY, "
                sSQL = sSQL & "     CASE WHEN RMACQPT_PRICE is not null THEN RMACQPT_PRICE ELSE RMARQD_PRICE END as RMARQD_PRICE, "
                sSQL = sSQL & "     RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE,"
                sSQL = sSQL & "     RMARQD_WAIVE, RMARQD_OPTION, RMARQD_OPTIONCLIENT, "
                sSQL = sSQL & "     vwRMAREPAIR_QUOTED_DETAIL.RMACQPT_RECHARGE_PRICE"

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT<>2"

                'sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED_DETAIL ON RMAD_ID = RMARQD_RMADID AND RMARQD_MARK=0"

                sSQL = sSQL & " INNER JOIN"
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT * FROM RMAREPAIR_QUOTED_DETAIL "
                sSQL = sSQL & "     LEFT OUTER JOIN RMACHARGE_QUOTED_PART ON RMARQD_ID = RMACQPT_RMARQD_ID and RMARQD_RMADID = RMACQPT_RMADID"
                sSQL = sSQL & "     WHERE RMARQD_MARK=0"
                sSQL = sSQL & " ) vwRMAREPAIR_QUOTED_DETAIL ON RMAD_ID = RMARQD_RMADID"

                sSQL = sSQL & " WHERE " & sCondition
                sSQL = sSQL & " ORDER BY rmarqd_cstmp asc, RMARQD_ID asc"

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtClient_SalesQuoted_Part)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtClient_SalesQuoted_Part
        End Function

        ''' <summary>
        ''' 取得 維修清單 表頭 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <remarks></remarks>
        Public Function QryRpt_RepairList_Head(ByVal RMA_NO As String) As RmaDTO.vwRptRepairList_HeadDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtRptRepairList_Head As New RmaDTO.vwRptRepairList_HeadDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                sSQL = sSQL & " SELECT RMA_NO, CU_NAME, TO_CHAR(RMAD_RECVDATE, 'YYYY/MM/DD') as RMAD_RECVDATE,  TO_CHAR(sysdate, 'YYYY/MM/DD HH24:MI') as PrintDate, vwSalesQuoted.RMASQ_QUOTE, "
                sSQL = sSQL & "     nvl(AcceptCount,0) as AcceptCount, nvl(RejectCount,0) as RejectCount"
                sSQL = sSQL & "     , COMP_NAME, vwRepair.RMAR_REPAIRADNAME , TO_CHAR(vwRepair.RMAR_REPAIRDATE, 'YYYY/MM/DD HH24:MI') as RMAR_REPAIRDATE"
                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN CUSTOMER ON RMA_CUNO = CU_NO"
                sSQL = sSQL & " INNER JOIN COMPANY ON RMA_COMPNO = COMP_NO"

                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, min(RMAD_RECVDATE) as RMAD_RECVDATE"
                sSQL = sSQL & "     FROM RMADETAIL WHERE RMAD_MARK=0 AND RMAD_RECEVSTATUS=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwRecDate"
                sSQL = sSQL & " ON RMAD_RMANO = RMA_NO"

                ''是否要維修: 1.Accept, 2.Reject
                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, sum(RMASQ_QUOTE) as RMASQ_QUOTE"
                sSQL = sSQL & "     FROM RMADETAIL, RMASALE_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMASQ_RMADID"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwSalesQuoted"
                sSQL = sSQL & " ON RMA_NO = vwSalesQuoted.RMAD_RMANO"

                sSQL = sSQL & " INNER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, RMAR_REPAIRAD, RMAR_REPAIRADNAME, RMAR_REPAIRDATE"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR "
                sSQL = sSQL & "     WHERE RMAD_ID = RMAR_RMADID AND RMAR_REPAIRAD IS NOT NULL"
                sSQL = sSQL & "         AND ROWNUM <= 1"
                sSQL = sSQL & "         AND  RMAD_RMANO= '" & RMA_NO & "'"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO, RMAR_REPAIRAD, RMAR_REPAIRADNAME, RMAR_REPAIRDATE"
                sSQL = sSQL & " ) vwRepair"
                sSQL = sSQL & " ON RMA_NO = vwRepair.RMAD_RMANO "

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as AcceptCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=1"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwAccept"
                sSQL = sSQL & " ON RMA_NO = vwAccept.RMAD_RMANO"

                sSQL = sSQL & " LEFT OUTER JOIN "
                sSQL = sSQL & " ("
                sSQL = sSQL & "     SELECT RMAD_RMANO, COUNT(*) as RejectCount"
                sSQL = sSQL & "     FROM RMADETAIL, RMAREPAIR_QUOTED "
                sSQL = sSQL & "     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=2"
                sSQL = sSQL & "     GROUP BY RMAD_RMANO"
                sSQL = sSQL & " ) vwReject"
                sSQL = sSQL & " ON RMA_NO = vwReject.RMAD_RMANO"

                sSQL = sSQL & " WHERE " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRptRepairList_Head)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRptRepairList_Head
        End Function

        ''' <summary>
        ''' 取得 維修清單 SN 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryRpt_RepairList_SN(ByVal RMA_NO As String) As RmaDTO.vwRptRepairList_SNDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtRptRepairList_SN As New RmaDTO.vwRptRepairList_SNDataTable
            Dim dt As New DataTable


            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                sSQL = sSQL & " SELECT RMA_NO, RMAD_ID , RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, "
                sSQL = sSQL & "     RMARQ_IMPROPERUSAGE, null as RMARQ_IMPROPERUSAGE_Text, "
                sSQL = sSQL & "     RMAD_ISWARRANTY, null as RMAD_ISWARRANTY_Text,"
                sSQL = sSQL & "     RMARQ_ACCEPT, null as RMARQ_ACCEPT,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE "
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_LABORCOST END"
                'sSQL = sSQL & "     END RMASQ_LABORCOST,"

                'sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 "
                'sSQL = sSQL & "         ELSE"
                'sSQL = sSQL & "             CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_MATERIALCOST END"
                'sSQL = sSQL & "     END RMASQ_MATERIALCOST, "

                'sSQL = sSQL & "     CASE WHEN RMAD_STATUS=60 THEN 0 ELSE RMASQ_QUOTE END RMASQ_QUOTE, "

                sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_LABORCOST,0) END RMASQ_LABORCOST, "
                sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_MATERIALCOST,0) END RMASQ_MATERIALCOST, "
                sSQL = sSQL & "     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE nvl(RMASQ_QUOTE,0) END RMASQ_QUOTE, "

                sSQL = sSQL & "     nvl(RMASQ_CURRENCYCODE,' ') as RMASQ_CURRENCYCODE, "
                sSQL = sSQL & "     nvl(RMASQ_CURRENCYRATE , 0) as RMASQ_CURRENCYRATE"

                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID"
                sSQL = sSQL & " LEFT OUTER JOIN RMASALE_QUOTED ON RMAD_ID = RMASQ_RMADID"

                sSQL = sSQL & " WHERE " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRptRepairList_SN)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRptRepairList_SN
        End Function

        ''' <summary>
        ''' 取得報價 Part 資料
        ''' </summary>
        ''' <param name="RMA_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QryRpt_RepairList_Part(ByVal RMA_NO As String) As RmaDTO.vwRptRepairList_PartDataTable
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            Dim dtRptRepairList_Part As New RmaDTO.vwRptRepairList_PartDataTable

            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO, OracleType.VarChar)
                sCondition = sCondition & " RMA_NO=:RMA_NO"

                sSQL = sSQL & " SELECT RMA_NO, RMAD_ID, RMARED_NPARTNO, RMARED_DESC, RMARED_QTY, "
                sSQL = sSQL & "     RMARED_PRICE, RMARED_CURRENCYCODE, RMARED_CURRENCYRATE, RMARED_ASSIGEPRICE, RMARED_ASSIGECURRENCYCODE, RMARED_ASSIGECURRENCYRATE, "
                sSQL = sSQL & "     RMARED_WAIVE, RMARED_OPTION, RMARED_ISSOURCE "
                sSQL = sSQL & " FROM RMA"
                sSQL = sSQL & " INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT<>2"
                sSQL = sSQL & " INNER JOIN RMAREPAIR_DETAIL ON RMARQ_RMADID = RMARED_RMADID AND RMARED_MARK=0 "

                sSQL = sSQL & " WHERE " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRptRepairList_Part)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRptRepairList_Part
        End Function
    End Class
#End Region

    ''' <summary>
    ''' 儲存出帳日
    ''' </summary>
    ''' <param name="arrRMANo"></param>
    ''' <param name="UserID"></param>
    ''' <param name="UserName"></param>
    ''' <remarks></remarks>
    Public Sub Save_BillData(ByVal arrRMANo() As String, ByVal UserID As String, ByVal UserName As String)
        Dim i As Integer = 0
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtTmp As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            Dim sCondition_RMANo As String = ""
            sCondition = sCondition & " WHERE ("
            For i = 0 To arrRMANo.Length - 1
                oQuery.addWHERE("BILL_RMANO", ":BILL_RMANO" & i.ToString, arrRMANo(i).Trim(), OracleType.VarChar)

                If sCondition_RMANo.Trim <> "" Then
                    sCondition_RMANo = sCondition_RMANo & " OR "
                End If
                sCondition_RMANo = sCondition_RMANo & " BILL_RMANO =:BILL_RMANO" & i.ToString
            Next
            sCondition = sCondition & sCondition_RMANo & ")"

            sSQL = "SELECT BILL_RMANO, '1' as isEdit FROM RMA_BILLDATA" & sCondition
            dtTmp = oQuery.ExecuteDT(sSQL)

            Dim dvTmp As DataView = dtTmp.DefaultView
            For i = 0 To arrRMANo.Length - 1
                Dim sRMANo As String = arrRMANo(i).ToString().Trim()
                Dim isEdit As String = 0

                dvTmp.RowFilter = "BILL_RMANO='" & sRMANo.Trim() & "'"
                If dvTmp.Count = 0 Then
                    Dim dr As DataRow = dtTmp.NewRow
                    dr("BILL_RMANO") = sRMANo
                    dr("isEdit") = 0
                    dtTmp.Rows.Add(dr)
                End If
            Next

            oConn.BeginTransaction()

            'addnew
            dvTmp.RowFilter = "isEdit=0"
            For i = 0 To dvTmp.Count - 1
                Dim BILL_RMANO As String = dvTmp(i)("BILL_RMANO").ToString().Trim()
                oExecute.addParameter("BILL_RMANO", BILL_RMANO, OracleType.NVarChar)
                oExecute.addParameter("BILL_DATE", Date.Now, OracleType.DateTime)

                oExecute.addParameter("BILL_AD", UserID.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_ADNAME", UserName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_CSTMP", Date.Now, OracleType.DateTime)

                oExecute.addParameter("BILL_LUAD", UserID.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUADNAME", UserName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.Command("RMA_BILLDATA", Execute.eumCommandType.AddNew)
            Next


            'edit
            dvTmp.RowFilter = "isEdit=1"
            For i = 0 To dvTmp.Count - 1
                Dim BILL_RMANO As String = dvTmp(i)("BILL_RMANO").ToString().Trim()

                oExecute.addParameter("BILL_DATE", Date.Now, OracleType.DateTime)

                oExecute.addParameter("BILL_LUAD", UserID.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUADNAME", UserName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("BILL_RMANO", BILL_RMANO.Trim(), OracleType.NVarChar)
                oExecute.Command("RMA_BILLDATA", Execute.eumCommandType.UPDATE)
            Next

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
    ''' Clear 出帳日
    ''' </summary>
    ''' <param name="arrRMANo"></param>
    ''' <param name="UserID"></param>
    ''' <param name="UserName"></param>
    ''' <remarks></remarks>
    Public Sub Clear_BillData(ByVal arrRMANo() As String, ByVal UserID As String, ByVal UserName As String)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try


            oConn.BeginTransaction()

            For i = 0 To arrRMANo.Length - 1
                Dim BILL_RMANO As String = arrRMANo(i).Trim()

                oExecute.addParameter("BILL_DATE", DBNull.Value.ToString(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUAD", UserID.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUADNAME", UserName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("BILL_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("BILL_RMANO", BILL_RMANO.Trim(), OracleType.NVarChar)
                oExecute.Command("RMA_BILLDATA", Execute.eumCommandType.UPDATE)
            Next

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
    ''' 檢核是否還可以再維修報價
    ''' </summary>
    ''' <param name="COMP_NO">登入系統的維修中心代碼</param>
    ''' <param name="RMAD_ID">RMA 品項代碼</param>
    ''' <returns>回傳True:可以, False:不可以</returns>
    ''' <remarks>
    ''' 登入系統維修中心 跟 被指派維修中心 不一樣時 
    ''' AND 被指派維修中心 已填寫維修金額, 指派維修中心就不可以再修改(進入Quoting 項目).
    ''' OR 品項狀態 大於 40.Sales Confirmed，維修人員就不可以再修改(進入Quoting 項目) - 更新報價
    ''' </remarks>
    Public Function isRepairQuoted(ByVal COMP_NO As String, ByVal RMAD_ID As String) As Boolean
        Dim retval As Boolean = True
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()
        Try

            sSQL = "SELECT RMA_NO, RMA_COMPNO, RMA_STATUS, " &
                " RMAD_ID, RMAD_STATUS, RMAD_MODELNO, RMAD_SERIALNO, " &
                " RMAR_COMPNO, RMAR_REPAIR_ISFILL" &
                " FROM rma,rmadetail, RMAREPAIR" &
                " WHERE RMA.RMA_NO = rmadetail.RMAD_RMANO AND rmadetail.RMAD_RECEVSTATUS=1 " &
                " AND RMA.RMA_MARK=0 AND rmadetail.RMAD_MARK=0" &
                " AND rmadetail.RMAD_ID = RMAREPAIR.rmar_rmadid" &
                " AND rmadetail.RMAD_MARK=0 " &
                " AND" &
                " (" &
                "   rmadetail.RMAD_STATUS >=40" &
                "   OR (RMAR_COMPNO<>'" & COMP_NO.Trim() & "' and RMAREPAIR.RMAR_REPAIR_ISFILL=1)" &
                " )" &
                " AND RMAD_ID='" & RMAD_ID.Trim() & "'"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                '找到符合條件資料, 就不可以在報價了
                retval = False
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
    ''' 取得 RMA 單的 維修中心 最低折扣
    ''' </summary>
    ''' <param name="RMA_NO"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getLowestDisCount(ByVal RMA_NO As String) As Double
        Dim retval As Double = 0
        Dim sSQL As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()
        Try

            sSQL = "SELECT RMA_NO, COMP_LOWESTDISCOUNT "
            sSQL = sSQL & " FROM RMA INNER JOIN company ON RMA.rma_compno = company.comp_no"
            sSQL = sSQL & " AND RMA.RMA_NO='" & RMA_NO.Trim() & "'"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COMP_LOWESTDISCOUNT")
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
    ''' 取得單一RMA 相關的加總筆數
    ''' </summary>
    ''' <param name="RMANo">RMA No</param>
    ''' <returns>RMACountDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryCountBySingular(ByVal RMANo As String) As RmaDTO.RMACountDataTable
        Dim i As Integer = 0
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim dtRMACount As New RmaDTO.RMACountDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            RMANo = RMANo.Trim().ToLower()
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMANo, OracleType.VarChar)
            sCondition = sCondition & " AND lower(RMA_NO) =:RMA_NO"


            Dim sSQL As String = "select RMA_NO, RMA_ID, nvl(RequestQTY,0) RequestQTY, nvl(ReceivedQTY,0) ReceivedQTY"
            sSQL = sSQL & " from rma inner join CUSTOMER"
            sSQL = sSQL & " ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=10 OR RMA_STATUS=20)"
            sSQL = sSQL & " inner join "
            sSQL = sSQL & "  ("
            sSQL = sSQL & "    select rmad_rmano, Count(*) RequestQTY from rmadetail"
            sSQL = sSQL & "    where rmad_mark=0 group BY rmad_rmano"
            sSQL = sSQL & "  ) vwRMADetail"
            sSQL = sSQL & " ON RMA.RMA_NO = vwRMADetail.rmad_rmano"
            sSQL = sSQL & " LEFT OUTER JOIN"
            sSQL = sSQL & " ("
            sSQL = sSQL & "   select rmad_rmano, Count(*) ReceivedQTY from rmadetail"
            sSQL = sSQL & "   where rmad_mark=0 AND RMAD_RECEVSTATUS=1 group BY rmad_rmano"
            sSQL = sSQL & "  ) vwRecv"
            sSQL = sSQL & " ON RMA.RMA_NO = vwRecv.rmad_rmano"
            sSQL = sSQL & " WHERE 1=1 " & sCondition
            sSQL = sSQL & " group by RMA_NO, RMA_ID, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(RequestQTY,0), nvl(ReceivedQTY,0)"


            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRMACount)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRMACount
    End Function

    ''' <summary>
    ''' 取得多個RMA 相關的加總筆數
    ''' </summary>
    ''' <param name="arrRMANo"></param>
    ''' <param name="OrderBY">定義排序</param>
    ''' <returns>RMACountDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryCountByPlural(ByVal arrRMANo() As String, Optional ByVal OrderBY As String = "") As RmaDTO.RMACountDataTable
        Dim i As Integer = 0
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim dtRMACount As New RmaDTO.RMACountDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " RMA_NO asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            For i = 0 To arrRMANo.Length - 1
                oQuery.addWHERE("RMA_NO", ":RMA_NO" & i.ToString, arrRMANo(i).Trim(), OracleType.VarChar)
                If sCondition.Trim <> "" Then
                    sCondition = sCondition & " OR "
                End If
                sCondition = sCondition & " RMA_NO =:RMA_NO" & i.ToString
            Next

            Dim sSQL As String = "select RMA_NO, RMA_ID, nvl(RequestQTY,0) RequestQTY, nvl(ReceivedQTY,0) ReceivedQTY"
            sSQL = sSQL & " from rma inner join CUSTOMER"
            sSQL = sSQL & " ON RMA.RMA_CUNO = CUSTOMER.CU_NO and rma_mark=0 and (RMA_STATUS=10 OR RMA_STATUS=20)"
            sSQL = sSQL & " inner join "
            sSQL = sSQL & "  ("
            sSQL = sSQL & "    select rmad_rmano, Count(*) RequestQTY from rmadetail"
            sSQL = sSQL & "    where rmad_mark=0 group BY rmad_rmano"
            sSQL = sSQL & "  ) vwRMADetail"
            sSQL = sSQL & " ON RMA.RMA_NO = vwRMADetail.rmad_rmano"
            sSQL = sSQL & " LEFT OUTER JOIN"
            sSQL = sSQL & " ("
            sSQL = sSQL & "   select rmad_rmano, Count(*) ReceivedQTY from rmadetail"
            sSQL = sSQL & "   where rmad_mark=0 AND RMAD_RECEVSTATUS=1 group BY rmad_rmano"
            sSQL = sSQL & "  ) vwRecv"
            sSQL = sSQL & " ON RMA.RMA_NO = vwRecv.rmad_rmano"
            sSQL = sSQL & " WHERE " & sCondition
            sSQL = sSQL & " group by RMA_NO, RMA_ID, cu_no, cu_name, RMA_STATUS, RMA_CSTMP, nvl(RequestQTY,0), nvl(ReceivedQTY,0)"
            sSQL = sSQL & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtRMACount)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtRMACount
    End Function

    ''' <summary>
    ''' 取得 RMA 單的 維修人員 Mail
    ''' </summary>
    ''' <param name="RMA_NO">RMA_NO</param>
    ''' <returns>回傳 ArrayList</returns>
    ''' <remarks></remarks>
    Public Function getRepaireMail_RMA(ByVal RMA_NO As String) As ArrayList
        Dim retArrList As New ArrayList
        Dim i As Integer = 0
        Dim dtRMA As New RmaDTO.RMADataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""


        oConn.Open()
        Try
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND RMA_NO=:RMA_NO"

            sSQL = "SELECT AD_NAME, AD_EMAIL FROM RMA, company, admin" &
                " WHERE RMA.rma_compno = company.COMP_NO" &
                " and AD_ROLE like '%2%' and AD_VISIBLE=1 " &
                " and instr(admin.ad_repaircenter, company.COMP_NO)>0 " &
                sCondition &
                " GROUP BY AD_NAME, AD_EMAIL"

            '                " and admin.ad_repaircenter in (company.COMP_NO) " & _

            dt = oQuery.ExecuteDT(sSQL)
            For i = 0 To dt.Rows.Count - 1
                Dim arrLiat(1) As String
                arrLiat(0) = dt.Rows(i)("AD_NAME").ToString().Trim()
                arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                retArrList.Add(arrLiat)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retArrList
    End Function

    ''' <summary>
    ''' 取得 RMA 單的 Shipping Mail
    ''' </summary>
    ''' <param name="RMA_NO">RMA_NO</param>
    ''' <returns>回傳 ArrayList</returns>
    ''' <remarks></remarks>
    Public Function getShippingMail_RMA(ByVal RMA_NO As String) As ArrayList
        Dim retArrList As New ArrayList
        Dim i As Integer = 0
        Dim dtRMA As New RmaDTO.RMADataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""


        oConn.Open()
        Try
            oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND RMA_NO=:RMA_NO"

            sSQL = "SELECT AD_NAME, AD_EMAIL FROM RMA, company, admin" &
                " WHERE RMA.rma_compno = company.COMP_NO" &
                " and AD_ROLE like '%4%' and AD_VISIBLE=1 " &
                " and instr(admin.ad_repaircenter, company.COMP_NO)>0 " &
                sCondition &
                " GROUP BY AD_NAME, AD_EMAIL"

            '" and admin.ad_repaircenter in (company.COMP_NO) " & _

            dt = oQuery.ExecuteDT(sSQL)
            For i = 0 To dt.Rows.Count - 1
                Dim arrLiat(1) As String
                arrLiat(0) = dt.Rows(i)("AD_NAME").ToString().Trim()
                arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                retArrList.Add(arrLiat)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retArrList
    End Function

    ''' <summary>
    ''' 取得 某各維修中心的 維修人員 Mail
    ''' </summary>
    ''' <param name="sRepairCenter">某各維修中心 ID</param>
    ''' <returns>回傳 ArrayList</returns>
    ''' <remarks></remarks>
    Public Function getRepaireMail_RepairCenter(ByVal sRepairCenter As String) As ArrayList
        Dim retArrList As New ArrayList
        Dim i As Integer = 0
        Dim dtRMA As New RmaDTO.RMADataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sCondition As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " SELECT AD_NAME, AD_EMAIL FROM admin"
            sSQL = sSQL & " WHERE AD_ROLE like '%2%' AND AD_VISIBLE=1 "
            sSQL = sSQL & " AND instr(admin.ad_repaircenter, '" & sRepairCenter & "')>0"
            sSQL = sSQL & " GROUP BY AD_NAME, AD_EMAIL"

            dt = oQuery.ExecuteDT(sSQL)
            For i = 0 To dt.Rows.Count - 1
                Dim arrLiat(1) As String
                arrLiat(0) = dt.Rows(i)("AD_NAME").ToString().Trim()
                arrLiat(1) = dt.Rows(i)("AD_EMAIL").ToString().Trim()
                retArrList.Add(arrLiat)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return retArrList
    End Function

    ''' <summary>
    ''' 取得 RMA單 的業務人員與業助 Mail
    ''' </summary>
    ''' <param name="sRMANo">RMA No</param>
    ''' <returns>回傳 ArrayList</returns>
    ''' <remarks></remarks>
    Public Function getRMA_SalesMail(ByVal sRMANo As String) As RmaDTO.VWSALESBYRMADataTable
        Dim retArrList As New ArrayList
        Dim i As Integer = 0
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sCondition As String = ""


        oConn.Open()
        Try
            Dim arrRMANo() As String = sRMANo.Split(",")
            For i = 0 To arrRMANo.Length - 1
                oQuery.addWHERE("RMA_NO", ":RMA_NO" & i.ToString, arrRMANo(i).Trim(), OracleType.VarChar)

                If sCondition.Trim <> "" Then
                    sCondition = sCondition & " OR "
                End If
                sCondition = sCondition & " RMA_NO=:RMA_NO" & i.ToString
            Next
            sCondition = " AND (" & sCondition & ")"

            Dim sSQL As String = " SELECT CU_SALESID as SalesID, vwSales.AD_EMAIL as SalesEmail, vwSales.AD_Name as SalesName, "
            sSQL = sSQL & " CU_ASSISTANTID as AssistantID, vwAssistantSales.AD_EMAIL as AssistantEmail , vwAssistantSales.AD_Name as AssistantName"
            sSQL = sSQL & " FROM RMA"
            sSQL = sSQL & " INNER JOIN CUSTOMER "
            sSQL = sSQL & "     ON RMA_CUNO = CU_NO"

            sSQL = sSQL & " INNER JOIN CUSTOMERUSER"
            sSQL = sSQL & "     ON CUUS_CUID = CU_NO AND RMA_CUNO=CUUS_CUID  AND RMA_ACCOUNTID=CUUS_ACCOUNTID"

            sSQL = sSQL & " LEFT OUTER JOIN ADMIN vwSales"
            sSQL = sSQL & "     ON vwSales.AD_ID = CU_SALESID AND vwSales.AD_VISIBLE=1"

            sSQL = sSQL & " LEFT OUTER JOIN ADMIN vwAssistantSales"
            sSQL = sSQL & "     ON vwAssistantSales.AD_ID = CU_ASSISTANTID AND vwAssistantSales.AD_VISIBLE=1"

            sSQL = sSQL & " WHERE 1=1 " & sCondition

            sSQL = sSQL & " GROUP BY  CU_SALESID, vwSales.AD_EMAIL , vwSales.AD_Name,"
            sSQL = sSQL & "     CU_ASSISTANTID, vwAssistantSales.AD_EMAIL , vwAssistantSales.AD_Name"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtSales)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtSales
    End Function

    ''' <summary>
    ''' 取得 RMA單 的業務人員與業助 Mail
    ''' </summary>
    ''' <param name="sRMA_ARNO">RMA_ARNO</param>
    ''' <returns>回傳 ArrayList</returns>
    ''' <remarks></remarks>
    Public Function getSalesMail_ARNO(ByVal sRMA_ARNO As String) As RmaDTO.VWSALESBYRMADataTable
        Dim retArrList As New ArrayList
        Dim i As Integer = 0
        Dim dtSales As New RmaDTO.VWSALESBYRMADataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sCondition As String = ""


        oConn.Open()
        Try
            'Dim arrRMANo() As String = sRMANo.Split(",")
            'For i = 0 To arrRMANo.Length - 1
            '    oQuery.addWHERE("RMA_NO", ":RMA_NO" & i.ToString, arrRMANo(i).Trim(), OracleType.VarChar)

            '    If sCondition.Trim <> "" Then
            '        sCondition = sCondition & " OR "
            '    End If
            '    sCondition = sCondition & " RMA_NO=:RMA_NO" & i.ToString
            'Next
            'sCondition = " AND (" & sCondition & ")"
            sCondition = sCondition & " AND RMA_ARNO = '" & sRMA_ARNO.Trim() & "'"

            Dim sSQL As String = " SELECT RMA_INVNO, RMA_ARNO, CU_SALESID as SalesID, vwSales.AD_EMAIL as SalesEmail, vwSales.AD_Name as SalesName, "
            sSQL = sSQL & " CU_ASSISTANTID as AssistantID, vwAssistantSales.AD_EMAIL as AssistantEmail , vwAssistantSales.AD_Name as AssistantName,CUSTOMER.CU_NAME as CU_NAME,CUSTOMER.CU_FINANCEEMAIL as CU_FINANCEEMAIL "
            sSQL = sSQL & " FROM RMA"
            sSQL = sSQL & " INNER JOIN CUSTOMER "
            sSQL = sSQL & "     ON RMA_CUNO = CU_NO"

            sSQL = sSQL & " INNER JOIN CUSTOMERUSER"
            sSQL = sSQL & "     ON CUUS_CUID = CU_NO AND RMA_CUNO=CUUS_CUID  AND RMA_ACCOUNTID=CUUS_ACCOUNTID"

            sSQL = sSQL & " LEFT OUTER JOIN ADMIN vwSales"
            sSQL = sSQL & "     ON vwSales.AD_ID = CU_SALESID AND vwSales.AD_VISIBLE=1"

            sSQL = sSQL & " LEFT OUTER JOIN ADMIN vwAssistantSales"
            sSQL = sSQL & "     ON vwAssistantSales.AD_ID = CU_ASSISTANTID AND vwAssistantSales.AD_VISIBLE=1"

            sSQL = sSQL & " WHERE 1=1 " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtSales)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtSales
    End Function

    ''' <summary>
    ''' 取得 SDC 資料
    ''' </summary>
    ''' <param name="sRMA_SERIAL">RMA_SERIAL</param>
    ''' <returns>回傳 DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetSDCData(ByVal sRMA_SERIAL As String) As DataTable
        Dim dt As DataTable
        Dim oConn As New Connection
        Dim sSQL As Text.StringBuilder = New Text.StringBuilder
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = String.Empty

        oConn.Open()
        Try
            'oQuery.addWHERE("SERIAL", ":SERIAL", sRMA_SERIAL.Trim(), OracleType.VarChar)
            'sCondition = sCondition & " AND SERIAL=:SERIAL"
            oQuery.addWHERE("EXPAR_M_SN", ":EXPAR_M_SN", sRMA_SERIAL.Trim(), OracleType.VarChar)
            oQuery.addWHERE("EXPAR_D_SN", ":EXPAR_D_SN", sRMA_SERIAL.Trim(), OracleType.VarChar)
            sCondition = sCondition & " And ep.EXPAR_M_SN = :EXPAR_M_SN or ep.EXPAR_D_SN = :EXPAR_D_SN"

            'sSQL.AppendLine("SELECT a.AKEY,b.AST_TYPENAME,c.ima02 part_nm,c.ima021 part_desc")
            sSQL.AppendLine("SELECT DISTINCT a.AKEY,b.AST_TYPENAME,c.ima02 part_nm,c.ima021 part_desc")
            sSQL.AppendLine("FROM CIPHERLAB.AKEY_DETAIL_FILE a")
            sSQL.AppendLine("JOIN CIPHERLAB.AKEY_SOFTWARE_TYPE b ON b.AST_TYPE = a.AD_AKEY_TYPE")
            sSQL.AppendLine("LEFT JOIN (SELECT a.OEB01 || '-' || a.OEB03 orderno,b.ima01,b.ima02,b.ima021 FROM CIPHERLAB.oeb_file a ")
            sSQL.AppendLine("           JOIN CIPHERLAB.ima_file b ON b.IMA01 = a.oeb04")
            sSQL.AppendLine("          ) c ON c.orderno=a.AD_ORDERNO ")
            sSQL.AppendLine("LEFT JOIN EXPORT e on e.EXPORT_M_SN = a.SERIAL ")
            sSQL.AppendLine("LEFT JOIN EXPORT_PARTS ep ON e.EXPORT_M_SN = ep.EXPAR_M_SN ")
            sSQL.AppendLine("WHERE 1=1 ")
            sSQL.AppendLine(sCondition)

            dt = oQuery.ExecuteDT(sSQL.ToString())

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

    ''' <summary>
    ''' 取得 SDC 資料
    ''' </summary>
    ''' <param name="sRMA_SERIAL">RMA_SERIAL</param>
    ''' <returns>回傳 DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetWarrantyData(ByVal sRMA_SERIAL As String) As DataTable
        Dim dt As DataTable
        Dim oConn As New Connection
        Dim sSQL As Text.StringBuilder = New Text.StringBuilder
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = String.Empty

        oConn.Open()
        Try
            'sSQL.AppendLine("SELECT a.WAR_ID,a.WAR_TYPE,b.ITEM_TYPE_NAME,c.PROGRAM_TYPE_NAME,d.PRICE_VER_NAME,a.WAR_SPEC_DESC ")
            sSQL.AppendLine("SELECT DISTINCT a.WAR_ID,a.WAR_TYPE,b.ITEM_TYPE_NAME,c.PROGRAM_TYPE_NAME,d.PRICE_VER_NAME,a.WAR_SPEC_DESC ")
            sSQL.AppendLine("FROM WARRSET a")
            sSQL.AppendLine("JOIN WARRSET_ITEM_TYPE b ON b.WARRSET_TYPE = a.WAR_TYPE And b.ITEM_TYPE = a.WAR_ITEM_TYPE ")
            sSQL.AppendLine("JOIN WARRSET_PROGRAM_TYPE c ON c.WARRSET_TYPE = a.WAR_TYPE And c.PROGRAM_TYPE = a.WAR_PROGRAM_TYPE ")
            sSQL.AppendLine("JOIN WARRSET_PRICE_VER d ON d.WARRSET_TYPE = a.WAR_TYPE And d.PRICE_VER = a.WAR_PRICE_VER")
            'sSQL.AppendLine("JOIN EXPORT e ON e.EXPORT_WAR_ID = a.WAR_ID And e.EXPORT_SERIALNO=:EXPORT_SERIALNO ")
            sSQL.AppendLine("JOIN EXPORT e ON e.EXPORT_WAR_ID = a.WAR_ID ")
            sSQL.AppendLine("JOIN EXPORT_PARTS ep ON e.EXPORT_M_SN = ep.EXPAR_M_SN ")
            sSQL.AppendLine("WHERE ep.EXPAR_M_SN = :EXPAR_M_SN or ep.EXPAR_D_SN = :EXPAR_D_SN ")

            'oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", sRMA_SERIAL.Trim(), OracleType.VarChar)
            oQuery.addWHERE("EXPAR_M_SN", ":EXPAR_M_SN", sRMA_SERIAL.Trim(), OracleType.VarChar)
            oQuery.addWHERE("EXPAR_D_SN", ":EXPAR_D_SN", sRMA_SERIAL.Trim(), OracleType.VarChar)

            dt = oQuery.ExecuteDT(sSQL.ToString())

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt

    End Function

#Region "新增需求:BI保固"
    '需求新增:BI保固 By buck Add 20250902 begin
    Public Class WarrantyBI
        Public Sub UpdRMADetail_Apply_BI(model As RMADetailReq)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()

                oExecute.addParameter("RMAD_APPLY_BI", model.RMAD_APPLY_BI, OracleType.NVarChar)
                oExecute.addWHERE("RMAD_ID", model.RMAD_ID, OracleType.VarChar)
                oExecute.Command("RMADetail", Execute.eumCommandType.UPDATE)

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
    '需求新增:BI保固 By buck Add 20250902 end
#End Region
    '' begin 從ctIRMA合併過來 by buck 2025.06.27
    Public Class Upload
#Region "Class:Upload:上傳"
        ''' <summary>
        ''' 取得 Excel Upload Master Data
        ''' </summary>
        ''' <param name="RMAID">RMA ID</param>
        ''' <param name="RMANo">RMA No</param>
        ''' <param name="ModelNo">Model No</param>
        ''' <param name="SerialNo">Serial No</param>
        ''' <param name="CU_NAME">客戶名稱</param>
        ''' <param name="fdate">開始日期</param>
        ''' <param name="edate">結束日期</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>vwReceiveListDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryRmaImport(ByVal COMPNo As String, ByVal RMAID As String, ByVal RMANo As String, ByVal ModelNo As String, ByVal SerialNo As String,
            ByVal CU_NAME As String, ByVal fdate As String, ByVal edate As String,
            Optional ByVal OrderBY As String = "") As ImpRmaDTO.dtImpRmaDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRmaList As New ImpRmaDTO.dtImpRmaDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " IRMA_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                '維修中心
                If COMPNo.Trim() <> "" Then
                    Dim sCondition_Repair As String = ""
                    sCondition = sCondition & " AND ("
                    Dim arrRepair() As String = COMPNo.Split(",")
                    For i = 0 To arrRepair.Length - 1
                        oQuery.addWHERE("IRMA_COMPNO", ":IRMA_COMPNO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                        If sCondition_Repair.Trim <> "" Then
                            sCondition_Repair = sCondition_Repair & " OR "
                        End If
                        sCondition_Repair = sCondition_Repair & " IRMA_COMPNO =:IRMA_COMPNO" & i.ToString
                    Next
                    sCondition = sCondition & sCondition_Repair & ")"
                End If

                If RMAID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("IRMA_ID", ":IRMA_ID", RMAID, OracleType.VarChar)
                    sCondition = sCondition & " AND IRMA_ID=:IRMA_ID"
                End If

                If RMANo.ToString().Trim() <> "" Then
                    RMANo = "%" & RMANo.Trim().ToLower() & "%"
                    oQuery.addWHERE("IRMA_NO", ":IRMA_NO", RMANo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(IRMA_NO) like :IRMA_NO"
                End If

                If ModelNo.ToString().Trim() <> "" Then
                    ModelNo = "%" & ModelNo.Trim().ToLower() & "%"
                    oQuery.addWHERE("IRMAD_MODELNO", ":IRMAD_MODELNO", ModelNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(IRMAD_MODELNO) like :IRMAD_MODELNO"
                End If

                If SerialNo.ToString().Trim() <> "" Then
                    SerialNo = "%" & SerialNo.Trim().ToLower() & "%"
                    oQuery.addWHERE("IRMAD_SERIALNO", ":IRMAD_SERIALNO", SerialNo, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(IRMAD_SERIALNO) like :IRMAD_SERIALNO"
                End If

                If CU_NAME.ToString().Trim() <> "" Then
                    CU_NAME = "%" & CU_NAME.Trim().ToLower() & "%"
                    oQuery.addWHERE("CU_NAME", ":CU_NAME", CU_NAME, OracleType.NVarChar)
                    sCondition = sCondition & " AND lower(CU_NAME) like :CU_NAME"
                End If

                If fdate.Trim <> "" And edate.Trim <> "" Then
                    Dim RMAD_CSTMP = Convert.ToDateTime(fdate)
                    Dim RMAD_LUSTMP = Convert.ToDateTime(edate).AddHours(23).AddMinutes(59).AddSeconds(59)

                    oQuery.addWHERE("IRMA_CSTMP", ":IRMA_CSTMP", RMAD_CSTMP, OracleType.DateTime)
                    oQuery.addWHERE("IRMA_LUSTMP", ":IRMA_LUSTMP", RMAD_LUSTMP, OracleType.DateTime)

                    sCondition = sCondition & " AND (IRMA_CSTMP >=:IRMA_CSTMP AND IRMA_LUSTMP <=:IRMA_LUSTMP)"
                End If

                Dim sSQL As String = "SELECT irma_id, "
                sSQL = sSQL & "irma_no, "
                sSQL = sSQL & "irma_cuno, "
                sSQL = sSQL & "irma_accountid, "
                sSQL = sSQL & "irma_applicant, "
                sSQL = sSQL & "irma_tel, "
                sSQL = sSQL & "irma_address, "
                sSQL = sSQL & "irma_compno, "
                sSQL = sSQL & "irma_mail, "
                sSQL = sSQL & "irma_file, "
                sSQL = sSQL & "substr(irma_file,1,15)||'....' irma_file_st, "
                sSQL = sSQL & "irma_error, "
                sSQL = sSQL & "substr(irma_error,1,15)||'....' irma_error_st, "
                sSQL = sSQL & "DECODE(IRMA_STATUS,10,'Excel Upload',20,'Temp Saved',30,'Confirmed','Cancel') irma_status, "
                sSQL = sSQL & "irma_ad, "
                sSQL = sSQL & "irma_adname, "
                sSQL = sSQL & "irma_cstmp, "
                sSQL = sSQL & "irma_luad, "
                sSQL = sSQL & "irma_luadname, "
                sSQL = sSQL & "irma_lustmp, "
                sSQL = sSQL & "irma_mark, "
                sSQL = sSQL & "irma_remark,"
                sSQL = sSQL & "cu_name"
                sSQL = sSQL & " FROM"
                sSQL = sSQL & " IMP_RMA LEFT OUTER JOIN CUSTOMER ON IMP_RMA.IRMA_CUNO=CUSTOMER.CU_NO"
                sSQL = sSQL & " LEFT OUTER JOIN IMP_RMADETAIL ON IMP_RMA.IRMA_NO=IMP_RMADETAIL.IRMAD_RMANO"
                sSQL = sSQL & " WHERE irma_mark=0 and IRMA_STATUS in(10,20)" & sCondition
                sSQL = sSQL & " GROUP BY irma_id, "
                sSQL = sSQL & "irma_no, "
                sSQL = sSQL & "irma_cuno, "
                sSQL = sSQL & "irma_accountid, "
                sSQL = sSQL & "irma_applicant, "
                sSQL = sSQL & "irma_tel, "
                sSQL = sSQL & "irma_address, "
                sSQL = sSQL & "irma_compno, "
                sSQL = sSQL & "irma_mail, "
                sSQL = sSQL & "irma_file, "
                sSQL = sSQL & "substr(irma_file,1,15)||'....',"
                sSQL = sSQL & "irma_error, "
                sSQL = sSQL & "substr(irma_error,1,15)||'....',"
                sSQL = sSQL & "DECODE(IRMA_STATUS,10,'Excel Upload',20,'Temp Saved',30,'Confirmed','Cancel'), "
                sSQL = sSQL & "irma_ad, "
                sSQL = sSQL & "irma_adname, "
                sSQL = sSQL & "irma_cstmp, "
                sSQL = sSQL & "irma_luad, "
                sSQL = sSQL & "irma_luadname, "
                sSQL = sSQL & "irma_lustmp, "
                sSQL = sSQL & "irma_mark, "
                sSQL = sSQL & "irma_remark,"
                sSQL = sSQL & "cu_name "

                sSQL = sSQL & OrderBY
                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRmaList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRmaList
        End Function

        Public Function QueryRmaDetail(ByVal LanguageID As String, ByVal IRMAD_ID As String, ByVal IRMAD_RMANO As String, Optional ByVal OrderBY As String = "") As ImpRmaDTO.dtImpRmaDetailQryDataTable

            Dim i As Integer = 0
            Dim sCondition As String = ""
            Dim dtRmaList As New ImpRmaDTO.dtImpRmaDetailQryDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " irmad_seq"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If IRMAD_ID.ToString().Trim() <> "" Then
                    oQuery.addWHERE("IRMAD_ID", ":IRMAD_ID", IRMAD_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND IRMAD_ID=:IRMAD_ID"
                End If

                If IRMAD_RMANO.ToString().Trim() <> "" Then
                    IRMAD_RMANO = "%" & IRMAD_RMANO.Trim().ToLower() & "%"
                    oQuery.addWHERE("IRMAD_RMANO", ":IRMAD_RMANO", IRMAD_RMANO, OracleType.VarChar)
                    sCondition = sCondition & " AND lower(IRMAD_RMANO) like :IRMAD_RMANO"
                End If

                Dim sSQL As String = "SELECT irmad_id, "
                sSQL = sSQL & "irmad_id, "
                sSQL = sSQL & "irmad_seq, "
                sSQL = sSQL & "irmad_rmano, "
                sSQL = sSQL & "irmad_modelno, "
                sSQL = sSQL & "irmad_serialno, "
                sSQL = sSQL & "irmad_cusname, "
                sSQL = sSQL & "irmad_warranty, "
                sSQL = sSQL & "irmad_farfarcno, "
                sSQL = sSQL & "irmad_farno, "
                sSQL = sSQL & "irmad_uploadfile, "
                sSQL = sSQL & "irmad_problemdesc, "
                sSQL = sSQL & "irmad_status, "
                sSQL = sSQL & "DECODE(irmad_status,10,'Excel Upload',20,'Temp Saved',30,'Confirmed','Cancel') irmad_status_name, "
                sSQL = sSQL & "irmad_isfill, "
                sSQL = sSQL & "irmad_recvad, "
                sSQL = sSQL & "irmad_recvadname, "
                sSQL = sSQL & "irmad_recvdate, "
                sSQL = sSQL & "irmad_recevstatus, "
                sSQL = sSQL & "irmad_ad, "
                sSQL = sSQL & "irmad_adname, "
                sSQL = sSQL & "irmad_cstmp, "
                sSQL = sSQL & "irmad_luad, "
                sSQL = sSQL & "irmad_luadname, "
                sSQL = sSQL & "irmad_lustmp, "
                sSQL = sSQL & "irmad_mark, "
                sSQL = sSQL & "irmad_productdesc, "
                sSQL = sSQL & "irmad_iswarranty, "
                sSQL = sSQL & "export_modelno, "
                sSQL = sSQL & "export_warranty_date,"
                sSQL = sSQL & "farc_name, "
                sSQL = sSQL & "far_reason"

                sSQL = sSQL & " FROM"
                sSQL = sSQL & " IMP_RMADETAIL LEFT OUTER JOIN IMP_RMA ON IMP_RMADETAIL.irmad_rmano=IMP_RMA.irma_no"
                sSQL = sSQL & " LEFT OUTER JOIN EXPORT ON IMP_RMADETAIL.irmad_serialno=EXPORT.export_serialno and IMP_RMA.irma_cuno=EXPORT.export_custno"
                sSQL = sSQL & " LEFT OUTER JOIN failurereasonsclass ON IMP_RMADETAIL.IRMAD_FARFARCNO=failurereasonsclass.farc_no"
                sSQL = sSQL & " AND failurereasonsclass.farc_dflno='" & LanguageID.Trim() & "'"

                sSQL = sSQL & "  LEFT OUTER JOIN failurereasons ON IMP_RMADETAIL.IRMAD_FARNO=failurereasons.far_no"
                sSQL = sSQL & " AND failurereasons.far_dflno='" & LanguageID.Trim() & "'"

                sSQL = sSQL & " WHERE irmad_mark=0 and irmad_status in(10,20)" & sCondition

                sSQL = sSQL & OrderBY
                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRmaList)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtRmaList
        End Function

        ''' <summary>
        ''' 取得已維修 品項的資料From IRMARepair_Detail
        ''' </summary>
        ''' <param name="IRMAD_ID">IRMAD_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>RMADetailDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByDetail_FromIRMARepair_Detail(ByVal LanguageID As String, ByVal IRMAD_ID As String, Optional ByVal OrderBY As String = "") As ImpRmaDTO.IRMARepair_DetailDataTable
            Dim sCondition As String = ""
            Dim dtRepairDetail As New ImpRmaDTO.IRMARepair_DetailDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                Dim HasExceColumn As New Hashtable
                HasExceColumn("IRMARED_ID") = "IRMARED_oldID"

                If OrderBY.Trim = "" Then
                    OrderBY = " IRMARED_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("IRMARED_RMADID", ":IRMARED_RMADID", IRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND IRMARED_RMADID=:IRMARED_RMADID"

                Dim sSQL As String = "SELECT IMP_RMAREPAIR_DETAIL.*,defective.defective_name,DECODE(IRMARED_IMPROPERUSAGE,0,'N','Y') IRMARED_IMPROPERUSAGE_NAME"
                sSQL = sSQL & " FROM IMP_RMAREPAIR_DETAIL"
                sSQL = sSQL & " left outer join defective on IMP_RMAREPAIR_DETAIL.IRMARED_DEFECTIVE=defective.defective_no"
                sSQL = sSQL & " AND defective.defective_dflno='" & LanguageID.Trim() & "'"

                sSQL = sSQL & " WHERE IRMARED_MARK=0 " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairDetail, HasExceColumn)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairDetail
        End Function

        ''' <summary>
        ''' 取得已維修 From IMP_RMARepair
        ''' </summary>
        ''' <param name="IRMAD_ID">IRMAD_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>IMP_RMAREPAIR</returns>
        ''' <remarks></remarks>
        Public Function QueryByRmaRepair_FromIMP_RMAREPAIR(ByVal IRMAD_ID As String, Optional ByVal OrderBY As String = "") As ImpRmaDTO.IRMARepairDataTable
            Dim sCondition As String = ""
            Dim dtRepairDetail As New ImpRmaDTO.IRMARepairDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " IRMAR_CSTMP asc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("IRMAR_RMADID", ":IRMAR_RMADID", IRMAD_ID, OracleType.VarChar)
                sCondition = sCondition & " AND IRMAR_RMADID=:IRMAR_RMADID"

                Dim sSQL As String = "SELECT * FROM IMP_RMAREPAIR WHERE IRMAR_ID IS NOT NULL " & sCondition

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtRepairDetail)

            Catch ex As Exception
                Throw ex

            End Try

            Return dtRepairDetail
        End Function

        ''' <summary>
        ''' 刪除 RMA 單 RMA_STATUS=Canceled From IMP_RMA
        ''' </summary>
        ''' <param name="RMA_ID">RMA ID</param>
        ''' <param name="RMA_NO">RMA NO</param>
        ''' <param name="RecvAD">收貨人帳號</param>
        ''' <param name="RecvNAME">收貨人姓名</param>
        ''' <remarks></remarks>
        Public Sub DeleteRMA_FromIMP_RMA(ByVal RMA_ID As String, ByVal RMA_NO As String,
                ByVal RecvAD As String, ByVal RecvNAME As String)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                '===========================================================================================================================================================================
                'UPDATE  RMA_STATUS = Canceled
                ' 10:Added,20:Updated,30:Closed,40:Cancel
                '===========================================================================================================================================================================
                oExecute.addParameter("IRMA_STATUS", "40", OracleType.Int16)
                oExecute.addParameter("IRMA_MARK", "1", OracleType.Int16)
                oExecute.addParameter("IRMA_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("IRMA_ID", RMA_ID.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMA", Execute.eumCommandType.UPDATE)


                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                ' 10:Added,20:Updated,30:Closed,40:Cancel
                '===========================================================================================================================================================================
                oExecute.addParameter("IRMAD_STATUS", "40", OracleType.Int16)
                oExecute.addParameter("IRMAD_MARK", "1", OracleType.Int16)
                oExecute.addParameter("IRMAD_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addParameter("IRMAD_RECVAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVDATE", Date.Now, OracleType.DateTime)

                '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                oExecute.addParameter("IRMAD_RECEVSTATUS", 2, OracleType.Int16)

                oExecute.addWHERE("IRMAD_RMANO", RMA_NO.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMADetail", Execute.eumCommandType.UPDATE)
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
        ''' 轉入 RMA 單 RMA_STATUS=Closed
        ''' </summary>
        ''' <param name="RMA_ID">RMA ID</param>
        ''' <param name="RMA_NO">RMA NO</param>
        ''' <param name="RecvAD">收貨人帳號</param>
        ''' <param name="RecvNAME">收貨人姓名</param>
        ''' <remarks></remarks>
        Public Function ImportRMA(ByVal RMA_ID As String, ByVal RMA_NO As String,
                ByVal RecvAD As String, ByVal RecvNAME As String) As String

            Dim sReturn As String = ""
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                Dim oGuid As Guid = Guid.NewGuid
                Dim oBill As New AutoDocumentNo.Bill
                Dim sRMA_ID As String = oGuid.ToString
                Dim sRMA_NO As String = ""
                Dim dt As New DataTable
                Dim dtRepair As New DataTable
                Dim dtRepairDetail As New DataTable
                Dim i As Integer
                Dim j As Integer
                Dim k As Integer
                Dim RMASM_ID As String = System.Guid.NewGuid().ToString()
                Dim RMASM_PACKINGNO As String = oBill.GetBillNo(oExecute.Connection, "SHM", RecvAD, RecvNAME)
                Dim RMASH_ID As String = System.Guid.NewGuid().ToString()
                Dim RMASH_SHIPPINGNO As String = oBill.GetBillNo(oExecute.Connection, "SHP", RecvAD, RecvNAME)
                '===========================================================================================================================================================================
                'Insert Into RMA
                '===========================================================================================================================================================================
                Dim sSQL As String = "SELECT * FROM IMP_RMA WHERE IRMA_ID='" & RMA_ID & "'"
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("IRMA_CUNO").ToString().Trim() = "" Then Throw New Exception("RMA_CUNO Can Not Empty!")
                    If dt.Rows(0)("IRMA_COMPNO").ToString().Trim() = "" Then Throw New Exception("RMA_COMPNO Can Not Empty!")

                    'sRMA_NO = "Z" + oBill.GetBillNo(oExecute.Connection, dt.Rows(0)("IRMA_COMPNO").ToString(), RecvAD, RecvNAME)
                    sRMA_NO = oBill.GetBillNo(oExecute.Connection, "import", RecvAD, RecvNAME)
                    sReturn = sRMA_NO

                    oExecute.addParameter("RMA_ID", sRMA_ID, OracleType.VarChar)      '系統自動產生唯一識別碼
                    oExecute.addParameter("RMA_NO", sRMA_NO, OracleType.VarChar)          'RMA 編號

                    oExecute.addParameter("RMA_CUNO", dt.Rows(0)("IRMA_CUNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMA_ACCOUNTID", RecvAD, OracleType.VarChar)
                    oExecute.addParameter("RMA_APPLICANT", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMA_TEL", dt.Rows(0)("IRMA_TEL").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMA_ADDRESS", dt.Rows(0)("IRMA_ADDRESS").ToString(), OracleType.NVarChar)
                    oExecute.addParameter("RMA_COMPNO", dt.Rows(0)("IRMA_COMPNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMA_STATUS", 90, OracleType.Int16)
                    oExecute.addParameter("RMA_MAIL", dt.Rows(0)("IRMA_MAIL").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMA_Remark", dt.Rows(0)("IRMA_REMARK").ToString(), OracleType.NVarChar)

                    oExecute.addParameter("RMA_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMA_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMA_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMA_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMA_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMA_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMA_MARK", 0, OracleType.Int16)
                    oExecute.Command("RMA", Execute.eumCommandType.AddNew)

                    'Insert Into RMA_SHIPMENT
                    oExecute.addParameter("RMASM_PACKINGNO", RMASM_PACKINGNO, OracleType.VarChar)            'RMA 編號
                    oExecute.addParameter("RMASM_ID", RMASM_ID, OracleType.VarChar)                                '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASM_CUNO", dt.Rows(0)("IRMA_CUNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMASM_ISSHIP", 1, OracleType.Int16)
                    oExecute.addParameter("RMASM_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASM_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASM_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASM_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASM_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASM_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMARSM_LABORCOST", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSM_MATERIALCOST", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSM_QUOTE", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSM_CURRENCYCODE", "NTD", OracleType.VarChar)
                    oExecute.addParameter("RMARSM_CURRENCYRATE", 0.0, OracleType.Double)
                    oExecute.addParameter("RMASM_ISBOSSCONFIRM", 2, OracleType.Int16)
                    oExecute.addParameter("RMASM_ISSUBMIT", 1, OracleType.Int16)
                    oExecute.Command("RMA_SHIPMENT", Execute.eumCommandType.AddNew)

                    'Insert Into RMA_SHIPPING
                    oExecute.addParameter("RMASH_SHIPPINGNO", RMASH_SHIPPINGNO, OracleType.VarChar)          'RMA 編號
                    oExecute.addParameter("RMASH_ID", RMASH_ID, OracleType.VarChar)                '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASH_PACKINGLIST", RMASH_SHIPPINGNO, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_FROM", RMASH_SHIPPINGNO, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_CUNO", dt.Rows(0)("IRMA_CUNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMASH_TRACKINGNO", RMASH_SHIPPINGNO, OracleType.VarChar)
                    oExecute.addParameter("RMASH_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASH_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASH_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASH_COMPNO", dt.Rows(0)("IRMA_COMPNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMASH_ISSUBMIT", 1, OracleType.Int16)
                    oExecute.Command("RMA_SHIPPING", Execute.eumCommandType.AddNew)

                    'Insert Into RMA_SHIPPINGDETAIL
                    Dim RMASHD_ID As String = System.Guid.NewGuid().ToString()
                    oExecute.addParameter("RMASHD_RMASHID", RMASH_ID, OracleType.VarChar)         'RMA Shipment 編號
                    oExecute.addParameter("RMASHD_ID", RMASHD_ID, OracleType.VarChar)          '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASHD_SHIPMENTNO", RMASM_PACKINGNO, OracleType.VarChar)
                    oExecute.addParameter("RMASHD_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASHD_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASHD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASHD_RMANO", sRMA_NO, OracleType.VarChar)
                    oExecute.addParameter("RMASHD_RMASMPACKINGNO", RMASM_PACKINGNO, OracleType.VarChar)

                    oExecute.Command("RMA_SHIPPINGDETAIL", Execute.eumCommandType.AddNew)
                End If
                '===========================================================================================================================================================================
                'Insert Into RMA Detail
                '===========================================================================================================================================================================
                sSQL = "SELECT * FROM IMP_RMADETAIL WHERE IRMAD_RMANO='" & RMA_NO & "' AND IRMAD_MARK=0"
                dt = oQuery.ExecuteDT(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    'RMADETAIL
                    Dim sRMAD_ID As String = System.Guid.NewGuid().ToString()
                    oExecute.addParameter("RMAD_ID", sRMAD_ID, OracleType.VarChar)                    '系統自動產生唯一識別碼
                    oExecute.addParameter("RMAD_RMANO", sRMA_NO, OracleType.VarChar)              '關聯 RMA.RMA_No

                    oExecute.addParameter("RMAD_SEQ", i + 1, OracleType.Int16)

                    oExecute.addParameter("RMAD_MODELNO", dt.Rows(i)("IRMAD_MODELNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMAD_SERIALNO", dt.Rows(i)("IRMAD_SERIALNO").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMAD_CUSNAME", dt.Rows(i)("IRMAD_CUSNAME").ToString(), OracleType.NVarChar)
                    If dt.Rows(i)("IRMAD_WARRANTY").ToString().Trim() <> "" Then
                        oExecute.addParameter("RMAD_WARRANTY", DateTime.Parse(dt.Rows(i)("IRMAD_WARRANTY").ToString()), OracleType.DateTime)
                    End If
                    oExecute.addParameter("RMAD_FARFARCNO", dt.Rows(i)("IRMAD_FARFARCNO").ToString(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_FARNO", dt.Rows(i)("IRMAD_FARNO").ToString(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_UPLOADFILE", dt.Rows(i)("IRMAD_UPLOADFILE").ToString(), OracleType.VarChar)
                    oExecute.addParameter("RMAD_PRODUCTDESC", dt.Rows(i)("IRMAD_PRODUCTDESC").ToString(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_PROBLEMDESC", dt.Rows(i)("IRMAD_PROBLEMDESC").ToString(), OracleType.NVarChar)
                    oExecute.addParameter("RMAD_STATUS", 90, OracleType.Int16)
                    oExecute.addParameter("RMAD_ISFILL", 1, OracleType.Int16)      '是否已填寫問題:0.否, 1.是
                    oExecute.addParameter("RMAD_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMAD_MARK", 0, OracleType.Int16)
                    oExecute.addParameter("RMAD_RECVAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_RECVADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMAD_RECEVSTATUS", 1, OracleType.Int16)
                    oExecute.Command("RMADETAIL", Execute.eumCommandType.AddNew)

                    'Insert Into RMA_SHIPMENTDETAIL
                    Dim RMASMD_ID As String = System.Guid.NewGuid().ToString()
                    oExecute.addParameter("RMASMD_RMASMID", RMASM_ID, OracleType.VarChar)         'RMA Shipment 編號
                    oExecute.addParameter("RMASMD_ID", RMASMD_ID, OracleType.VarChar)          '系統自動產生唯一識別碼
                    oExecute.addParameter("RMASMD_RMANO", sRMA_NO, OracleType.VarChar)
                    oExecute.addParameter("RMASMD_RMADID", sRMAD_ID, OracleType.VarChar)
                    oExecute.addParameter("RMARSD_LABORCOST", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSD_MATERIALCOST", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSD_QUOTE", 0.0, OracleType.Double)
                    oExecute.addParameter("RMARSD_CURRENCYCODE", "NTD", OracleType.VarChar)
                    oExecute.addParameter("RMARSD_CURRENCYRATE", 0.0, OracleType.Double)
                    oExecute.addParameter("RMASMD_LOWESTDISCOUNT", 0.0, OracleType.Double)
                    oExecute.addParameter("RMASMD_AD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_ADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_CSTMP", Date.Now, OracleType.DateTime)
                    oExecute.addParameter("RMASMD_LUAD", RecvAD, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUADNAME", RecvNAME, OracleType.NVarChar)
                    oExecute.addParameter("RMASMD_LUSTMP", Date.Now, OracleType.DateTime)
                    oExecute.Command("RMA_SHIPMENTDETAIL", Execute.eumCommandType.AddNew)

                    '===========================================================================================================================================================================
                    'Insert Into RMA Repair
                    '===========================================================================================================================================================================
                    sSQL = "SELECT * FROM IMP_RMAREPAIR WHERE IRMAR_RMADID='" & dt.Rows(i)("IRMAD_ID").ToString() & "'"
                    dtRepair = oQuery.ExecuteDT(sSQL)
                    For j = 0 To dtRepair.Rows.Count - 1
                        Dim sRMAR_ID As String = System.Guid.NewGuid().ToString()
                        oExecute.addParameter("RMAR_ID", sRMAR_ID, OracleType.VarChar)
                        oExecute.addParameter("RMAR_RMADID", sRMAD_ID, OracleType.VarChar)
                        oExecute.addParameter("RMAR_COMPNO", dtRepair.Rows(j)("IRMAR_COMPNO").ToString(), OracleType.VarChar)

                        oExecute.addParameter("RMAR_DUTYNO", dtRepair.Rows(j)("IRMAR_DUTYNO").ToString(), OracleType.VarChar)
                        oExecute.addParameter("RMAR_FARCNO", dtRepair.Rows(j)("IRMAR_FARCNO").ToString(), OracleType.VarChar)
                        oExecute.addParameter("RMAR_FARNO", dtRepair.Rows(j)("IRMAR_FARNO").ToString(), OracleType.VarChar)

                        oExecute.addParameter("RMAR_PROBLEMDESC", dtRepair.Rows(j)("IRMAR_PROBLEMDESC").ToString(), OracleType.NVarChar)
                        oExecute.addParameter("RMAR_REPAIRDESC", dtRepair.Rows(j)("IRMAR_REPAIRDESC").ToString(), OracleType.NVarChar)
                        oExecute.addParameter("RMAR_REPAIRMEMO", dtRepair.Rows(j)("IRMAR_REPAIRMEMO").ToString(), OracleType.NVarChar)


                        '是否已填寫維修報價單:0.否, 1.是
                        oExecute.addParameter("RMAR_REPAIR_ISFILL", 1, OracleType.Int16)

                        oExecute.addParameter("RMAR_LABORHOUR", Double.Parse(dtRepair.Rows(j)("IRMAR_LABORHOUR").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_LABORPRICE", Double.Parse(dtRepair.Rows(j)("IRMAR_LABORPRICE").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_LABORCOST", Double.Parse(dtRepair.Rows(j)("IRMAR_LABORCOST").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_MATERIALCOST", Double.Parse(dtRepair.Rows(j)("IRMAR_MATERIALCOST").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_QUOTE", Double.Parse(dtRepair.Rows(j)("IRMAR_QUOTE").ToString()), OracleType.Double)

                        oExecute.addParameter("RMAR_CURRENCYCODE", dtRepair.Rows(j)("IRMAR_CURRENCYCODE").ToString(), OracleType.VarChar)
                        oExecute.addParameter("RMAR_CURRENCYRATE", Double.Parse(dtRepair.Rows(j)("IRMAR_CURRENCYRATE").ToString()), OracleType.Double)

                        oExecute.addParameter("RMAR_ASSIGELABORCOST", Double.Parse(dtRepair.Rows(j)("IRMAR_ASSIGELABORCOST").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_ASSIGEMATERIALCOST", Double.Parse(dtRepair.Rows(j)("IRMAR_ASSIGEMATERIALCOST").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_ASSIGEQUOTE", Double.Parse(dtRepair.Rows(j)("IRMAR_ASSIGEQUOTE").ToString()), OracleType.Double)
                        oExecute.addParameter("RMAR_ASSIGECURRENCYCODE", dtRepair.Rows(j)("IRMAR_ASSIGECURRENCYCODE").ToString(), OracleType.VarChar)
                        oExecute.addParameter("RMAR_ASSIGECURRENCYRATE", Double.Parse(dtRepair.Rows(j)("IRMAR_ASSIGECURRENCYRATE").ToString()), OracleType.Double)

                        oExecute.addParameter("RMAR_REPAIRAD", dtRepair.Rows(j)("IRMAR_REPAIRAD").ToString(), OracleType.NVarChar)
                        oExecute.addParameter("RMAR_REPAIRADNAME", dtRepair.Rows(j)("IRMAR_REPAIRADNAME").ToString(), OracleType.NVarChar)
                        oExecute.addParameter("RMAR_REPAIRDATE", Date.Parse(dtRepair.Rows(j)("IRMAR_REPAIRDATE").ToString()), OracleType.DateTime)

                        oExecute.addParameter("RMAR_AD", RecvAD, OracleType.NVarChar)
                        oExecute.addParameter("RMAR_ADNAME", RecvNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMAR_CSTMP", Date.Now, OracleType.DateTime)
                        oExecute.addParameter("RMAR_LUAD", RecvAD, OracleType.NVarChar)
                        oExecute.addParameter("RMAR_LUADNAME", RecvNAME, OracleType.NVarChar)
                        oExecute.addParameter("RMAR_LUSTMP", Date.Now, OracleType.DateTime)

                        oExecute.Command("RMAREPAIR", Execute.eumCommandType.AddNew)

                        '

                        sSQL = "SELECT * FROM IMP_RMAREPAIR_DETAIL WHERE IRMARED_RMADID='" & dt.Rows(i)("IRMAD_ID").ToString() & "'"
                        dtRepairDetail = oQuery.ExecuteDT(sSQL)
                        For k = 0 To dtRepairDetail.Rows.Count - 1
                            Dim sRMARED_ID As String = System.Guid.NewGuid().ToString()
                            oExecute.addParameter("RMARED_ID", sRMARED_ID, OracleType.VarChar)
                            oExecute.addParameter("RMARED_RMADID", sRMAD_ID, OracleType.VarChar)

                            oExecute.addParameter("RMARED_NPARTNO", dtRepairDetail.Rows(k)("IRMARED_NPARTNO").ToString(), OracleType.VarChar)
                            oExecute.addParameter("RMARED_NSERIALNO", dtRepairDetail.Rows(k)("IRMARED_NSERIALNO").ToString(), OracleType.VarChar)
                            If dtRepairDetail.Rows(k)("IRMARED_NWARRANTY").ToString().Trim() <> "" Then oExecute.addParameter("RMARED_NWARRANTY", Date.Parse(dtRepairDetail.Rows(k)("IRMARED_NWARRANTY").ToString()), OracleType.DateTime)

                            oExecute.addParameter("RMARED_OPARTNO", dtRepairDetail.Rows(k)("IRMARED_OPARTNO").ToString(), OracleType.VarChar)
                            oExecute.addParameter("RMARED_OSERIALNO", dtRepairDetail.Rows(k)("IRMARED_OSERIALNO").ToString(), OracleType.VarChar)

                            If dtRepairDetail.Rows(k)("IRMARED_OWARRANTY").ToString().Trim() <> "" Then oExecute.addParameter("RMARED_OWARRANTY", Date.Parse(dtRepairDetail.Rows(k)("IRMARED_OWARRANTY").ToString()), OracleType.DateTime)

                            oExecute.addParameter("RMARED_IMPROPERUSAGE", dtRepairDetail.Rows(k)("IRMARED_IMPROPERUSAGE").ToString(), OracleType.Int16)
                            oExecute.addParameter("RMARED_DESC", dtRepairDetail.Rows(k)("IRMARED_DESC").ToString(), OracleType.NVarChar)
                            oExecute.addParameter("RMARED_LOCATION", dtRepairDetail.Rows(k)("IRMARED_LOCATION").ToString(), OracleType.VarChar)
                            oExecute.addParameter("RMARED_DEFECTIVE", dtRepairDetail.Rows(k)("IRMARED_DEFECTIVE").ToString(), OracleType.NVarChar)

                            oExecute.addParameter("RMARED_QTY", dtRepairDetail.Rows(k)("IRMARED_QTY").ToString(), OracleType.Int16)
                            oExecute.addParameter("RMARED_MATERIALCOST", Double.Parse(dtRepairDetail.Rows(k)("IRMARED_MATERIALCOST").ToString()), OracleType.Double)
                            oExecute.addParameter("RMARED_PRICE", Double.Parse(dtRepairDetail.Rows(k)("IRMARED_PRICE").ToString()), OracleType.Double)

                            oExecute.addParameter("RMARED_AD", RecvAD, OracleType.NVarChar)
                            oExecute.addParameter("RMARED_ADNAME", RecvNAME, OracleType.NVarChar)
                            oExecute.addParameter("RMARED_CSTMP", Date.Now, OracleType.DateTime)
                            oExecute.addParameter("RMARED_LUAD", RecvAD, OracleType.NVarChar)
                            oExecute.addParameter("RMARED_LUADNAME", RecvNAME, OracleType.NVarChar)
                            oExecute.addParameter("RMARED_LUSTMP", Date.Now, OracleType.DateTime)
                            oExecute.addParameter("RMARED_MARK", 0, OracleType.Double)

                            oExecute.addParameter("RMARED_CURRENCYCODE", dtRepairDetail.Rows(k)("IRMARED_CURRENCYCODE").ToString(), OracleType.VarChar)
                            oExecute.addParameter("RMARED_CURRENCYRATE", Double.Parse(dtRepairDetail.Rows(k)("IRMARED_CURRENCYRATE").ToString()), OracleType.Double)

                            oExecute.addParameter("RMARED_ASSIGECURRENCYCODE", dtRepairDetail.Rows(k)("IRMARED_ASSIGECURRENCYCODE").ToString(), OracleType.VarChar)
                            oExecute.addParameter("RMARED_ASSIGECURRENCYRATE", Double.Parse(dtRepairDetail.Rows(k)("IRMARED_ASSIGECURRENCYRATE").ToString()), OracleType.Double)
                            oExecute.addParameter("RMARED_ASSIGEPRICE", Double.Parse(dtRepairDetail.Rows(k)("IRMARED_ASSIGEPRICE").ToString()), OracleType.Double)

                            oExecute.Command("RMAREPAIR_DETAIL", Execute.eumCommandType.AddNew)

                        Next
                    Next
                Next


                '===========================================================================================================================================================================
                'UPDATE  RMA_STATUS = Canceled
                ' 10:Added,20:Updated,30:Closed,40:Cancel
                '===========================================================================================================================================================================
                oExecute.addParameter("IRMA_STATUS", "30", OracleType.Int16)
                oExecute.addParameter("IRMA_RMANO", sRMA_NO.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUSTMP", Date.Now, OracleType.DateTime)
                oExecute.addWHERE("IRMA_ID", RMA_ID.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMA", Execute.eumCommandType.UPDATE)

                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                ' 10:Added,20:Updated,30:Closed,40:Cancel
                '===========================================================================================================================================================================
                oExecute.addParameter("IRMAD_STATUS", "30", OracleType.Int16)
                oExecute.addParameter("IRMAD_LUAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("IRMAD_RECVAD", RecvAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVADNAME", RecvNAME.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVDATE", Date.Now, OracleType.DateTime)
                '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                oExecute.addParameter("IRMAD_RECEVSTATUS", 1, OracleType.Int16)
                oExecute.addWHERE("IRMAD_RMANO", RMA_NO.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMADetail", Execute.eumCommandType.UPDATE)
                oConn.Commit()

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
            Return sReturn
        End Function

        ''' <summary>
        ''' 刪除 RMA 單身 RMAD_STATUS=Canceled From IMP_RMADetail
        ''' </summary>
        ''' <param name="RMAD_ID">RMAD_ID</param>
        ''' <param name="UpdateAD">收貨人帳號</param>
        ''' <param name="UpdateName">收貨人姓名</param>
        ''' <remarks></remarks>
        Public Sub DeleteRMAD_FromIMP_RMADetail(ByVal RMAD_ID As String, ByVal UpdateAD As String, ByVal UpdateName As String)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()
                '===========================================================================================================================================================================
                'UPDATE  RMAD_STATUS = Canceled
                ' 10:Added,20:Updated,30:Closed,40:Cancel
                '===========================================================================================================================================================================
                oExecute.addParameter("IRMAD_STATUS", "40", OracleType.Int16)
                oExecute.addParameter("IRMAD_MARK", "1", OracleType.Int16)
                oExecute.addParameter("IRMAD_LUAD", UpdateAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUADNAME", UpdateName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addParameter("IRMAD_RECVAD", UpdateAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVADNAME", UpdateName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMAD_RECVDATE", Date.Now, OracleType.DateTime)

                '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除
                oExecute.addParameter("IRMAD_RECEVSTATUS", 2, OracleType.Int16)

                oExecute.addWHERE("IRMAD_ID", RMAD_ID.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMADetail", Execute.eumCommandType.UPDATE)
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
        ''' 刪除 RMA 單身 RMAD_STATUS=Canceled From IMP_RMAREPAIR_DETAIL
        ''' </summary>
        ''' <param name="IRMARED_ID">IRMARED_ID</param>
        ''' <param name="UpdateAD">UpdateAD</param>
        ''' <param name="UpdateName">UpdateName</param>
        ''' <remarks></remarks>
        Public Sub DeleteRMAREPAIR_DETAIL(ByVal IRMARED_ID As String, ByVal UpdateAD As String, ByVal UpdateName As String)

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try

                oConn.BeginTransaction()

                oExecute.addParameter("IRMARED_MARK", 1, OracleType.Int16)
                oExecute.addParameter("IRMARED_LUAD", UpdateAD.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUADNAME", UpdateName.Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("IRMARED_ID", IRMARED_ID.Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMAREPAIR_DETAIL", Execute.eumCommandType.UPDATE)


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
        ''' 編輯 RMA 單身 RMAD_STATUS=Canceled From IMP_RMAREPAIR_DETAIL
        ''' </summary>
        ''' <param name="dr">IRMARepair_DetailRow</param>
        ''' <param name="RMA_NO">RMA NO</param>
        ''' <remarks></remarks>
        Public Sub EditRMAREPAIR_DETAIL(ByVal dr As ImpRmaDTO.IRMARepair_DetailRow, ByVal RMA_NO As String)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()


                oExecute.addParameter("IRMARED_NPARTNO", dr.IRMARED_NPARTNO, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_NSERIALNO", dr.IRMARED_NSERIALNO, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_OPARTNO", dr.IRMARED_OPARTNO, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_OSERIALNO", dr.IRMARED_OSERIALNO, OracleType.NVarChar)

                oExecute.addParameter("IRMARED_IMPROPERUSAGE", dr.IRMARED_IMPROPERUSAGE.ToString().Trim(), OracleType.Int16)
                oExecute.addParameter("IRMARED_DEFECTIVE", dr.IRMARED_DEFECTIVE.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("IRMARED_DESC", dr.IRMARED_DESC.ToString().Trim(), OracleType.NVarChar)

                oExecute.addParameter("IRMARED_LUAD", dr.IRMARED_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUADNAME", dr.IRMARED_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUSTMP", dr.IRMARED_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("IRMARED_ID", dr.IRMARED_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMAREPAIR_DETAIL", Execute.eumCommandType.UPDATE)

                oExecute.addParameter("IRMA_STATUS", 20, OracleType.Int16)
                oExecute.addParameter("IRMA_LUAD", dr.IRMARED_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUADNAME", dr.IRMARED_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUSTMP", dr.IRMARED_LUSTMP, OracleType.DateTime)
                oExecute.addWHERE("IRMA_NO", RMA_NO, OracleType.VarChar)
                oExecute.Command("IMP_RMA", Execute.eumCommandType.UPDATE)
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
        ''' Insert RMAREPAIR_DETAI From IMP_RMA
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <remarks></remarks>
        Public Sub InsertRMAREPAIR_DETAIL_FromIMP_RMA(ByVal dr As ImpRmaDTO.IRMARepair_DetailRow, ByVal RMA_NO As String)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()
                Dim oGuid As Guid = Guid.NewGuid


                oExecute.addParameter("IRMARED_ID", oGuid.ToString(), OracleType.VarChar)
                oExecute.addParameter("IRMARED_RMADID", dr.IRMARED_RMADID, OracleType.VarChar)
                oExecute.addParameter("IRMARED_NPARTNO", dr.IRMARED_NPARTNO, OracleType.VarChar)
                oExecute.addParameter("IRMARED_NSERIALNO", dr.IRMARED_NSERIALNO, OracleType.VarChar)
                'oExecute.addParameter("IRMARED_NWARRANTY", dr.IRMARED_NWARRANTY, OracleType.DateTime)

                oExecute.addParameter("IRMARED_OPARTNO", dr.IRMARED_OPARTNO, OracleType.VarChar)
                oExecute.addParameter("IRMARED_OSERIALNO", dr.IRMARED_OSERIALNO, OracleType.VarChar)
                'oExecute.addParameter("IRMARED_OWARRANTY", dr.IRMARED_OWARRANTY, OracleType.DateTime)

                oExecute.addParameter("IRMARED_DESC", dr.IRMARED_DESC, OracleType.VarChar)
                'oExecute.addParameter("IRMARED_LOCATION", dr.IRMARED_LOCATION, OracleType.VarChar)
                oExecute.addParameter("IRMARED_IMPROPERUSAGE", dr.IRMARED_IMPROPERUSAGE, OracleType.Int16)
                oExecute.addParameter("IRMARED_DEFECTIVE", dr.IRMARED_DEFECTIVE, OracleType.VarChar)
                oExecute.addParameter("IRMARED_QTY", dr.IRMARED_QTY, OracleType.Int16)
                oExecute.addParameter("IRMARED_MATERIALCOST", dr.IRMARED_MATERIALCOST, OracleType.Double)
                oExecute.addParameter("IRMARED_PRICE", dr.IRMARED_PRICE, OracleType.Double)
                oExecute.addParameter("IRMARED_CURRENCYCODE", dr.IRMARED_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("IRMARED_ASSIGEPRICE", dr.IRMARED_ASSIGEPRICE, OracleType.Double)
                oExecute.addParameter("IRMARED_ASSIGECURRENCYCODE", dr.IRMARED_ASSIGECURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("IRMARED_ASSIGECURRENCYRATE", dr.IRMARED_ASSIGECURRENCYRATE, OracleType.Double)

                oExecute.addParameter("IRMARED_AD", dr.IRMARED_AD, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_ADNAME", dr.IRMARED_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_CSTMP", dr.IRMARED_CSTMP, OracleType.DateTime)
                oExecute.addParameter("IRMARED_LUAD", dr.IRMARED_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUADNAME", dr.IRMARED_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMARED_LUSTMP", dr.IRMARED_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("IRMARED_MARK", dr.IRMARED_MARK, OracleType.Double)

                oExecute.Command("IMP_RMAREPAIR_DETAIL", Execute.eumCommandType.AddNew)

                oExecute.addParameter("IRMA_STATUS", 20, OracleType.Int16)
                oExecute.addParameter("IRMA_LUAD", dr.IRMARED_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUADNAME", dr.IRMARED_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUSTMP", dr.IRMARED_LUSTMP, OracleType.DateTime)
                oExecute.addWHERE("IRMA_NO", RMA_NO, OracleType.VarChar)
                oExecute.Command("IMP_RMA", Execute.eumCommandType.UPDATE)

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
        ''' Modify RMADetail From IMP_RMA
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <remarks></remarks>
        Public Sub Edit_RMADetail_FromIMP_RMA(ByVal dr As ImpRmaDTO.dtImpRmaDetailQryRow)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                If dr.IsIRMAD_MODELNONull = False Then oExecute.addParameter("IRMAD_MODELNO", dr.IRMAD_MODELNO, OracleType.VarChar)
                If dr.IsIRMAD_WARRANTYNull = False Then oExecute.addParameter("IRMAD_WARRANTY", dr.IRMAD_WARRANTY, OracleType.DateTime)

                If dr.IsIRMAD_SERIALNONull = False Then oExecute.addParameter("IRMAD_SERIALNO", dr.IRMAD_SERIALNO, OracleType.VarChar)
                If dr.IsIRMAD_CUSNAMENull = False Then oExecute.addParameter("IRMAD_CUSNAME", dr.IRMAD_CUSNAME, OracleType.NVarChar)

                If dr.IsIRMAD_FARFARCNONull = False Then oExecute.addParameter("IRMAD_FARFARCNO", dr.IRMAD_FARFARCNO, OracleType.VarChar)
                If dr.IsIRMAD_FARNONull = False Then oExecute.addParameter("IRMAD_FARNO", dr.IRMAD_FARNO, OracleType.VarChar)
                'oExecute.addParameter("IRMAD_PRODUCTDESC", dr.IRMAD_PRODUCTDESC, OracleType.NVarChar)
                If dr.IsIRMAD_PROBLEMDESCNull = False Then oExecute.addParameter("IRMAD_PROBLEMDESC", dr.IRMAD_PROBLEMDESC, OracleType.NVarChar)

                oExecute.addParameter("IRMAD_STATUS", dr.IRMAD_STATUS, OracleType.Int16)
                oExecute.addParameter("IRMAD_LUAD", dr.IRMAD_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUADNAME", dr.IRMAD_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMAD_LUSTMP", dr.IRMAD_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("IRMAD_ID", dr.IRMAD_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMADETAIL", Execute.eumCommandType.UPDATE)

                oExecute.addParameter("IRMA_STATUS", 20, OracleType.Int16)
                oExecute.addParameter("IRMA_LUAD", dr.IRMAD_LUAD, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUADNAME", dr.IRMAD_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("IRMA_LUSTMP", dr.IRMAD_LUSTMP, OracleType.DateTime)
                oExecute.addWHERE("IRMA_NO", dr.IRMAD_RMANO.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("IMP_RMA", Execute.eumCommandType.UPDATE)

                oConn.Commit()

            Catch ex As Exception
                oConn.Rollback()
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End Sub

        Public Function GetExpotrData1() As OracleDataReader
            Dim oConn_string As String = "DATA SOURCE=192.168.7.20:1521/topprod;USER ID=rma;PASSWORD=4321rma;PERSIST SECURITY INFO=True;"
            Dim conn As New OracleConnection(oConn_string)

            Dim sSQL = "
select distinct * from (
SELECT 
    EXPORT.*,'' AS ORDER_NO
FROM EXPORT 
WHERE SUBSTR(EXPORT_ORDERNUMBER, 1, 14) IN (
    'AXBA-251000028',
    'AXBA-251000051',
    'AXBA-251000099',
    'AXBA-251000121',
    'AXBA-251000130',
    'AXBA-251000148',
    'AXBA-251000194',
    'AXBA-251000329',
    'AXBA-251000478',
    'AXBA-251000587',
    'AXBA-251000589',
    'AXBA-251000591',
    'AXBA-251000599',
    'AXBA-251100028',
    'AXBA-251100038',
    'AXBA-251100064',
    'AXBA-251100104',
    'AXBA-251100155',
    'AXBA-251100170',
    'AXBA-251100397',
    'AXBA-251100399',
    'AXBA-251100435',
    'AXBA-251100439',
    'AXBA-251100466',
    'AXBA-251200129',
    'AXBA-251200160',
    'AXBA-251200260',
    'AXBA-251200270',
    'AXBA-251200343',
    'AXBA-251200345',
    'AXBA-251200442',
    'AXBA-251200494',
    'AXBA-251200496',
    'AXBA-260100036',
    'AXBA-260100051',
    'AXBA-260100061',
    'AXBA-260100066',
    'AXBA-260100103',
    'AXBA-260100246',
    'AXBA-260100505',
    'AXBA-260100528',
    'AXBA-260200061',
    'AXBA-260200125',
    'AXBA-260200196',
    'AXBA-260200429',
    'AXBA-260300049',
    'AXBA-260300075',
    'AXBA-260300184',
    'AXBA-260300222',
    'AXBA-260300286',
    'AXBA-260300292',
    'AXBA-260300449',
    'AXBA-260300550'
)
UNION ALL

SELECT 
    export.*,WATY_NO AS ORDER_NO
FROM WARRANTYORD 
LEFT JOIN WARRANTYSERIAL ON WATS_WATYNO = WATY_NO
LEFT JOIN export ON EXPORT_SERIALNO = WATS_SN
WHERE WATY_NO IN (
    'WRMA-2025100001',
    'WRMA-2025100002',
    'WRMA-2025100003',
    'WRMA-2025100008',
    'WRMA-2025100010',
    'WRMA-2025100011',
    'WRMA-2025100012',
    'WRMA-2025100013',
    'WRMA-2025100014',
    'WRMA-2025100015',
    'WRMA-2025100016',
    'WRMA-2025100017',
    'WRMA-2025100018',
    'WRMA-2025100020',
    'WRMA-2025100021',
    'WRMA-2025100022',
    'WRMA-2025100023',
    'WRMA-2025100024',
    'WRMA-2025100025',
    'WRMA-2025100026',
    'WRMA-2025100027',
    'WRMA-2025100030',
    'WRMA-2025100032',
    'WRMA-2025100033',
    'WRMA-2025100034',
    'WRMA-2025100035',
    'WRMA-2025100036',
    'WRMA-2025100037',
    'WRMA-2025100038',
    'WRMA-2025100039',
    'WRMA-2025100040',
    'WRMA-2025100041',
    'WRMA-2025100042',
    'WRMA-2025100044',
    'WRMA-2025100045',
    'WRMA-2025100046',
    'WRMA-2025100047',
    'WRMA-2025100048',
    'WRMA-2025100050',
    'WRMA-2025100051',
    'WRMA-2025100052',
    'WRMA-2025100053',
    'WRMA-2025100054',
    'WRMA-2025100055',
    'WRMA-2025100056',
    'WRMA-2025100057',
    'WRMA-2025100058',
    'WRMA-2025100059',
    'WRMA-2025100060',
    'WRMA-2025100061',
    'WRMA-2025100062',
    'WRMA-2025100063',
    'WRMA-2025100064',
    'WRMA-2025100065',
    'WRMA-2025100066',
    'WRMA-2025100067',
    'WRMA-2025100068',
    'WRMA-2025100070',
    'WRMA-2025100071',
    'WRMA-2025100072',
    'WRMA-2025100074',
    'WRMA-2025100075',
    'WRMA-2025100077',
    'WRMA-2025100078',
    'WRMA-2025100079',
    'WRMA-2025100080',
    'WRMA-2025100081',
    'WRMA-2025100082',
    'WRMA-2025100083',
    'WRMA-2025100084',
    'WRMA-2025100085',
    'WRMA-2025100087',
    'WRMA-2025100088',
    'WRMA-2025100090',
    'WRMA-2025100091',
    'WRMA-2025100092',
    'WRMA-2025110002',
    'WRMA-2025110003',
    'WRMA-2025110004',
    'WRMA-2025110005',
    'WRMA-2025110008',
    'WRMA-2025110009',
    'WRMA-2025110010',
    'WRMA-2025110011',
    'WRMA-2025110013',
    'WRMA-2025110014',
    'WRMA-2025110015',
    'WRMA-2025110020',
    'WRMA-2025110021',
    'WRMA-2025110022',
    'WRMA-2025110023',
    'WRMA-2025110024',
    'WRMA-2025110025',
    'WRMA-2025110026',
    'WRMA-2025110027',
    'WRMA-2025110028',
    'WRMA-2025110029',
    'WRMA-2025110030',
    'WRMA-2025110031',
    'WRMA-2025110032',
    'WRMA-2025110033',
    'WRMA-2025110034',
    'WRMA-2025110035',
    'WRMA-2025110036',
    'WRMA-2025110037',
    'WRMA-2025110038',
    'WRMA-2025110039',
    'WRMA-2025110040',
    'WRMA-2025110041',
    'WRMA-2025110043',
    'WRMA-2025110045',
    'WRMA-2025110048',
    'WRMA-2025110049',
    'WRMA-2025110051',
    'WRMA-2025110053',
    'WRMA-2025110056',
    'WRMA-2025110057',
    'WRMA-2025110058',
    'WRMA-2025110059',
    'WRMA-2025110060',
    'WRMA-2025110061',
    'WRMA-2025110062',
    'WRMA-2025110063',
    'WRMA-2025110064',
    'WRMA-2025110065',
    'WRMA-2025110068',
    'WRMA-2025110069',
    'WRMA-2025110070',
    'WRMA-2025110071',
    'WRMA-2025110072',
    'WRMA-2025110073',
    'WRMA-2025120001',
    'WRMA-2025120003',
    'WRMA-2025120004',
    'WRMA-2025120005',
    'WRMA-2025120007',
    'WRMA-2025120009',
    'WRMA-2025120010',
    'WRMA-2025120011',
    'WRMA-2025120012',
    'WRMA-2025120013',
    'WRMA-2025120014',
    'WRMA-2025120015',
    'WRMA-2025120016',
    'WRMA-2025120017',
    'WRMA-2025120018',
    'WRMA-2025120019',
    'WRMA-2025120020',
    'WRMA-2025120021',
    'WRMA-2025120022',
    'WRMA-2025120023',
    'WRMA-2025120025',
    'WRMA-2025120026',
    'WRMA-2025120027',
    'WRMA-2025120028',
    'WRMA-2025120029',
    'WRMA-2025120030',
    'WRMA-2025120031',
    'WRMA-2025120033',
    'WRMA-2025120034',
    'WRMA-2025120035',
    'WRMA-2025120036',
    'WRMA-2025120038',
    'WRMA-2025120039',
    'WRMA-2025120040',
    'WRMA-2025120041',
    'WRMA-2025120042',
    'WRMA-2025120043',
    'WRMA-2025120044',
    'WRMA-2025120046',
    'WRMA-2025120047',
    'WRMA-2025120048',
    'WRMA-2025120049',
    'WRMA-2025120050',
    'WRMA-2025120051',
    'WRMA-2025120052',
    'WRMA-2025120053',
    'WRMA-2025120054',
    'WRMA-2025120055',
    'WRMA-2025120056',
    'WRMA-2025120057',
    'WRMA-2025120058',
    'WRMA-2025120059',
    'WRMA-2025120060',
    'WRMA-2025120061',
    'WRMA-2025120062',
    'WRMA-2025120063',
    'WRMA-2025120064',
    'WRMA-2025120065',
    'WRMA-2025120066',
    'WRMA-2025120067',
    'WRMA-2025120068',
    'WRMA-2025120069',
    'WRMA-2025120070',
    'WRMA-2026010001',
    'WRMA-2026010002',
    'WRMA-2026010003',
    'WRMA-2026010004',
    'WRMA-2026010005',
    'WRMA-2026010006',
    'WRMA-2026010007',
    'WRMA-2026010009',
    'WRMA-2026010010',
    'WRMA-2026010011',
    'WRMA-2026010012',
    'WRMA-2026010013',
    'WRMA-2026010014',
    'WRMA-2026010015',
    'WRMA-2026010016',
    'WRMA-2026010017',
    'WRMA-2026010018',
    'WRMA-2026010019',
    'WRMA-2026010020',
    'WRMA-2026010021',
    'WRMA-2026010022',
    'WRMA-2026010023',
    'WRMA-2026010024',
    'WRMA-2026010025',
    'WRMA-2026010027',
    'WRMA-2026010028',
    'WRMA-2026010029',
    'WRMA-2026010030',
    'WRMA-2026010032',
    'WRMA-2026010033',
    'WRMA-2026010034',
    'WRMA-2026010035',
    'WRMA-2026010036',
    'WRMA-2026010037',
    'WRMA-2026010038',
    'WRMA-2026020001',
    'WRMA-2026020002',
    'WRMA-2026020003',
    'WRMA-2026020004',
    'WRMA-2026020005',
    'WRMA-2026020006',
    'WRMA-2026020007',
    'WRMA-2026020008',
    'WRMA-2026020009',
    'WRMA-2026020010',
    'WRMA-2026020011',
    'WRMA-2026020012',
    'WRMA-2026020013',
    'WRMA-2026020014',
    'WRMA-2026020016',
    'WRMA-2026020017',
    'WRMA-2026020018',
    'WRMA-2026020019',
    'WRMA-2026020020',
    'WRMA-2026020021',
    'WRMA-2026020022',
    'WRMA-2026020023',
    'WRMA-2026020024',
    'WRMA-2026020026',
    'WRMA-2026020027',
    'WRMA-2026020028',
    'WRMA-2026020029',
    'WRMA-2026020030',
    'WRMA-2026020031',
    'WRMA-2026020032',
    'WRMA-2026020033',
    'WRMA-2026020034',
    'WRMA-2026020035',
    'WRMA-2026020036',
    'WRMA-2026020037',
    'WRMA-2026020038',
    'WRMA-2026020039',
    'WRMA-2026020040',
    'WRMA-2026030002',
    'WRMA-2026030003',
    'WRMA-2026030004',
    'WRMA-2026030005',
    'WRMA-2026030006',
    'WRMA-2026030007',
    'WRMA-2026030008',
    'WRMA-2026030009',
    'WRMA-2026030010',
    'WRMA-2026030011',
    'WRMA-2026030012',
    'WRMA-2026030013',
    'WRMA-2026030014',
    'WRMA-2026030015',
    'WRMA-2026030016',
    'WRMA-2026030017',
    'WRMA-2026030018',
    'WRMA-2026030020',
    'WRMA-2026030021',
    'WRMA-2026030022',
    'WRMA-2026030023',
    'WRMA-2026030024',
    'WRMA-2026030025',
    'WRMA-2026030026',
    'WRMA-2026030027',
    'WRMA-2026030028',
    'WRMA-2026030031',
    'WRMA-2026030032',
    'WRMA-2026030033',
    'WRMA-2026030034'
)

)
                        "
            Dim cmd As New OracleCommand(sSQL, conn)

            Try
                conn.Open()
                ' 使用 CloseConnection 確保後續 reader.Close() 時連線一併關閉
                Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Catch ex As Exception
                If conn.State = ConnectionState.Open Then conn.Close()
                Throw
            End Try

        End Function
#End Region
    End Class
    '' end 從ctIRMA合併過來 by buck 2025.06.27

End Class

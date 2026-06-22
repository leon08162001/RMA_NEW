Imports System.Configuration
Imports System.Data.OracleClient
Imports System.Linq
Imports System.Text
Imports ICAT_OracleDAO
Imports RMA_Model

Public Class ctlWarranty

    Dim _isDebug As String = ConfigurationSettings.AppSettings("isDebug")

    Public Function Select_RMA(ByVal RMA_NO As String) As DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            If RMA_NO.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMA_NO", ":RMA_NO", RMA_NO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMA_NO=:RMA_NO"
            End If


            sSQL = "SELECT * FROM RMA WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function Get_CUSTOMER_PRODUCT_NUMBER_RMAD_RMANO(ByVal RMAD_RMANO As String, ByVal RMAD_SERIALNO As String) As String

        Dim CUSTOMER_PRODUCT_NUMBER As String = ""


        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            If RMAD_RMANO.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_RMANO=:RMAD_RMANO"
            End If
            If RMAD_SERIALNO.ToString().Trim() <> "" Then
                oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND RMAD_SERIALNO=:RMAD_SERIALNO"
            End If


            sSQL = "  SELECT CUSTOMER_PRODUCT_NUMBER FROM RMADETAIL  WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count = 0 Then
                CUSTOMER_PRODUCT_NUMBER = ""
            Else
                CUSTOMER_PRODUCT_NUMBER = dt.Rows(0)("CUSTOMER_PRODUCT_NUMBER").ToString().Trim()
            End If


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return CUSTOMER_PRODUCT_NUMBER
    End Function


    ''' WARRANTYSERIAL
    Public Function WARRANTYSERIALQuery(ByVal WATS_WATYNO As String) As DataTable


        Dim check As String = ""

        Dim dt As New DataTable
        Dim dtWarranty As New WarrantyDTO.WarrantyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try


            If WATS_WATYNO.ToString().Trim() <> "" Then
                oQuery.addWHERE("WATS_WATYNO", ":WATS_WATYNO", WATS_WATYNO.Trim(), OracleType.VarChar)
                sCondition = sCondition & " WHERE WATS_WATYNO=:WATS_WATYNO"
            End If

            Dim sSQL As String = "SELECT * FROM WARRANTYSERIAL " & sCondition

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function


    ''' WARRANTYSERIAL
    Public Function WARRANTYSERIALQuery_WATS_SN(ByVal WATS_SN As String, ByVal WATS_WATYNO As String) As DataTable


        Dim check As String = ""

        Dim dt As New DataTable
        Dim dtWarranty As New WarrantyDTO.WarrantyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try


            If WATS_SN.ToString().Trim() <> "" Then
                oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN.Trim(), OracleType.VarChar)
                sCondition = sCondition & " WHERE WATS_SN=:WATS_SN"
            End If

            If WATS_WATYNO.ToString().Trim() <> "" Then
                oQuery.addWHERE("WATS_WATYNO", ":WATS_WATYNO", WATS_SN.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND WATS_SN<>:WATS_WATYNO"
            End If

            Dim sSQL As String = "SELECT * FROM WARRANTYSERIAL " & sCondition

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
    ''' 取得Warranty資料
    ''' </summary>
    ''' <param name="Visible">是否顯示(1:顯示 , 0:不顯示, "":全部)</param>
    ''' <param name="OrderBY">排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 WarrantyDataTable</remarks>
    Public Function Query(ByVal Visible As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrantyDataTable

        Dim dt As New DataTable
        Dim dtWarranty As New WarrantyDTO.WarrantyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " WARR_LUSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If Visible.ToString().Trim() <> "" Then
                oQuery.addWHERE("WARR_VISIBLE", ":WARR_VISIBLE", Visible.Trim(), OracleType.Int16)
                sCondition = sCondition & " AND WARR_VISIBLE=:WARR_VISIBLE"
            End If

            oQuery.addWHERE("WARR_MARK", ":WARR_MARK", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WARRANTY WHERE WARR_MARK=:WARR_MARK " & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarranty)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarranty
    End Function



    ''' <summary>
    ''' Warranty - 修改
    ''' </summary>
    ''' <param name="dtWarranty">傳入WarrantyDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtWarranty As WarrantyDTO.WarrantyDataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            For i = 0 To dtWarranty.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrantyRow = dtWarranty.Rows(i)

                oExecute.addParameter("WARR_NUMBER", dr.WARR_NUMBER.ToString().Trim(), OracleType.VarChar)
                oExecute.addParameter("WARR_UNIT", dr.WARR_UNIT.ToString().Trim(), OracleType.Int16)
                oExecute.addParameter("WARR_TYPE", dr.WARR_TYPE.ToString().Trim(), OracleType.Int16)
                oExecute.addParameter("WARR_VISIBLE", dr.WARR_VISIBLE.ToString().Trim(), OracleType.Int16)

                'oExecute.addParameter("WARR_AD", dr.WARR_AD.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("WARR_ADNAME", dr.WARR_ADNAME.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("WARR_CSTMP", dr.WARR_CSTMP, OracleType.DateTime)

                oExecute.addParameter("WARR_LUAD", dr.WARR_LUAD, OracleType.NVarChar)
                oExecute.addParameter("WARR_LUADNAME", dr.WARR_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("WARR_LUSTMP", dr.WARR_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("WARR_MARK", dr.WARR_MARK.ToString().Trim(), OracleType.Int16)

                oExecute.addWHERE("WARR_ID", dr.WARR_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("WARRANTY", Execute.eumCommandType.UPDATE)
            Next

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
    ''' Part Query
    ''' </summary>
    ''' <param name="PartNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryParts(ByVal PartNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.vwPartsDataTable

        Dim dt As New DataTable
        Dim dtParts As New WarrantyDTO.vwPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " PartNo desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If PartNo.ToString().Trim() <> "" Then
                oQuery.addWHERE("sfb05", ":sfb05", PartNo, OracleType.VarChar)
                sCondition = sCondition & " AND sfb05=:sfb05"
            End If


            Dim sSQL As String = "select sfb05 PartNo from sfb_file group by sfb05" & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtParts
    End Function

    ''' <summary>
    ''' QueryWarrGroup Query
    ''' </summary>
    ''' <param name="GroupNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWarrGroup(ByVal GroupNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrGroupTypeDataTable

        Dim dt As New DataTable
        Dim dtwWarrGroup As New WarrantyDTO.WarrGroupTypeDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " GROUP_NO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If GroupNo.ToString().Trim() <> "" Then
                'GroupNo = "%" & GroupNo & "%"
                oQuery.addWHERE("GROUP_NO", ":GROUP_NO", GroupNo, OracleType.VarChar)
                'sCondition = sCondition & " AND GROUP_NO LIKE:GROUP_NO"
                sCondition = sCondition & " AND GROUP_NO = :GROUP_NO"
            End If


            Dim sTableName As String = "WarrGroupType"
            If _isDebug = True Then
                sTableName = "WarrGroupType"
            End If
            Dim sSQL As String = "SELECT GROUP_NO,GROUP_NAME,GROUP_AD,GROUP_ADNAME,GROUP_CSTMP,GROUP_LUAD,GROUP_LUADNAME,GROUP_LUSTMP FROM " & sTableName & " WHERE 1=1" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtwWarrGroup)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtwWarrGroup
    End Function

    ''' <summary>
    ''' 取得公司代碼是否存在
    ''' </summary>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Public Function chkWarrGroupIsExist(ByVal new_GroupNo As String, Optional ByVal old_GroupNo As String = "") As Boolean
        Dim retval As Boolean = False
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If old_GroupNo.Trim() <> "" Then
                oQuery.addWHERE("GROUP_NO", ":old_GroupNo", old_GroupNo, OracleType.VarChar)
                sCondition = sCondition & " AND GROUP_NO<>:old_GroupNo"
            End If

            oQuery.addWHERE("GROUP_NO", ":new_GroupNo", new_GroupNo, OracleType.VarChar)
            Dim sSQL As String = "SELECT * FROM WarrGroupType WHERE GROUP_NO=:new_GroupNo " & sCondition

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
    ''' 
    ''' </summary>
    ''' <param name="dtGroup"></param>
    ''' <remarks></remarks>
    Public Sub SaveWarrGroupAdd(ByVal dtGroup As WarrantyDTO.WarrGroupTypeDataTable)
        Dim i As Integer = 0

        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            For i = 0 To dtGroup.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrGroupTypeRow = dtGroup.Rows(i)

                oExecute.addParameter("GROUP_NO", dr.GROUP_NO, OracleType.VarChar)
                oExecute.addParameter("GROUP_NAME", dr.GROUP_NAME, OracleType.NVarChar)

                oExecute.addParameter("GROUP_AD", dr.GROUP_AD, OracleType.NVarChar)
                oExecute.addParameter("GROUP_ADNAME", dr.GROUP_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("GROUP_CSTMP", dr.GROUP_CSTMP, OracleType.DateTime)
                oExecute.addParameter("GROUP_LUAD", dr.GROUP_LUAD, OracleType.NVarChar)
                oExecute.addParameter("GROUP_LUADNAME", dr.GROUP_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("GROUP_LUSTMP", dr.GROUP_LUSTMP, OracleType.DateTime)

                oExecute.Command("WarrGroupType", Execute.eumCommandType.AddNew)
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
    ''' 
    ''' </summary>
    ''' <param name="dtGroup"></param>
    ''' <param name="oldGroup_No"></param>
    ''' <remarks></remarks>
    Public Sub SaveWarrGroupEdit(ByVal dtGroup As WarrantyDTO.WarrGroupTypeDataTable, ByVal oldGroup_No As String)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            For i = 0 To dtGroup.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrGroupTypeRow = dtGroup.Rows(i)

                oExecute.addParameter("GROUP_NO", dr.GROUP_NO.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("GROUP_NAME", dr.GROUP_NAME.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("GROUP_LUAD", dr.GROUP_LUAD, OracleType.NVarChar)
                oExecute.addParameter("GROUP_LUADNAME", dr.GROUP_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("GROUP_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("GROUP_NO", oldGroup_No.Trim(), OracleType.VarChar)
                oExecute.Command("WarrGroupType", Execute.eumCommandType.UPDATE)
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
    ''' QueryWarrGroupParts
    ''' </summary>
    ''' <param name="GroupNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function QueryWarrGroupParts(ByVal GroupNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrGroupPartsDataTable

        Dim dt As New DataTable
        Dim dtwWarrGroupParts As New WarrantyDTO.WarrGroupPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " GRPT_GNO desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If GroupNo.ToString().Trim() <> "" Then
                GroupNo = "%" & GroupNo & "%"
                oQuery.addWHERE("GRPT_GNO", ":GRPT_GNO", GroupNo, OracleType.VarChar)
                sCondition = sCondition & " AND GRPT_GNO LIKE:GRPT_GNO"
            End If


            Dim sTableName As String = "WarrGroupParts"
            If _isDebug = True Then
                sTableName = "WarrGroupParts"
            End If
            Dim sSQL As String = "SELECT GRPT_GNO,GRPT_TNO,GRPT_AD,GRPT_ADNAME,GRPT_CSTMP,GRPT_LUAD,GRPT_LUADNAME,GRPT_LUSTMP FROM " & sTableName & " WHERE 1=1" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtwWarrGroupParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtwWarrGroupParts
    End Function

    ''' <summary>
    ''' SaveWarrGroupPartsAdd
    ''' </summary>
    ''' <param name="dtGroup"></param>
    ''' <remarks></remarks>

    Public Sub SaveWarrGroupPartsAdd(ByVal dtGroup As WarrantyDTO.WarrGroupPartsDataTable, ByVal GROUP_NO As String)
        Dim i As Integer = 0

        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            oExecute.addWHERE("GRPT_GNO", GROUP_NO, OracleType.VarChar)
            oExecute.Command("WarrGroupParts", Execute.eumCommandType.Delete)

            For i = 0 To dtGroup.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrGroupPartsRow = dtGroup.Rows(i)

                oExecute.addParameter("GRPT_GNO", dr.GRPT_GNO, OracleType.VarChar)
                oExecute.addParameter("GRPT_TNO", dr.GRPT_TNO, OracleType.NVarChar)

                oExecute.addParameter("GRPT_AD", dr.GRPT_AD, OracleType.NVarChar)
                oExecute.addParameter("GRPT_ADNAME", dr.GRPT_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("GRPT_CSTMP", dr.GRPT_CSTMP, OracleType.DateTime)
                oExecute.addParameter("GRPT_LUAD", dr.GRPT_LUAD, OracleType.NVarChar)
                oExecute.addParameter("GRPT_LUADNAME", dr.GRPT_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("GRPT_LUSTMP", dr.GRPT_LUSTMP, OracleType.DateTime)

                oExecute.Command("WarrGroupParts", Execute.eumCommandType.AddNew)
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
    ''' ProductGroup Query
    ''' </summary>
    ''' <param name="GroupNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function QueryPrdGroup(ByVal GroupNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.vwPrdGroupDataTable

        Dim dt As New DataTable
        Dim dtProductGroup As New WarrantyDTO.vwPrdGroupDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " tc_oad020 desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If GroupNo.ToString().Trim() <> "" Then
                GroupNo = "%" & GroupNo & "%"
                oQuery.addWHERE("tc_oad020", ":tc_oad020", GroupNo, OracleType.VarChar)
                sCondition = sCondition & " AND tc_oad020 LIKE:tc_oad020"
            End If


            Dim sTableName As String = "cipherlab.tc_oad_file"
            'If _isDebug = True Then
            'sTableName = "tc_oad_file"
            'End If
            Dim sSQL As String = "select tc_oad020 GroupNo,tc_oad030 GroupName from " & sTableName & " where tc_oad010='23'" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtProductGroup)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtProductGroup
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PartNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryPrdGroupByPartNo(ByVal PartNo As String, Optional ByVal OrderBY As String = "") As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " tc_oae240 desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If PartNo.ToString().Trim() <> "" Then
                PartNo = "%" & PartNo & "%"
                oQuery.addWHERE("tc_oae010", ":tc_oae010", PartNo, OracleType.VarChar)
                sCondition = sCondition & " AND tc_oae010 LIKE:tc_oae010"
            End If


            Dim sSQL As String = "select tc_oae240 tc_oae020 from cipherlab.tc_oae_file where 1=1" & sCondition & OrderBY

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
    ''' QueryCostCenter
    ''' </summary>
    ''' <param name="CenterNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryCostCenter(ByVal CenterNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.vwCostCenterDataTable

        Dim dt As New DataTable
        Dim dtProductGroup As New WarrantyDTO.vwCostCenterDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " CenterNo desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If CenterNo.ToString().Trim() <> "" Then
                oQuery.addWHERE("CenterNo", ":CenterNo", CenterNo, OracleType.VarChar)
                sCondition = sCondition & " AND CenterNo=:CenterNo"
            End If


            Dim sSQL As String = "select CenterNo,CenterName from CostCenter " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtProductGroup)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtProductGroup
    End Function


    ''' <summary>
    ''' Special設定主檔
    ''' </summary>
    ''' <param name="sw_id"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySWSet(ByVal sw_id As String, ByVal OperationCenter As String, ByVal ProductGroup As String, ByVal WarrantyType As Integer, ByVal IsValid As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.SWSETDataTable

        Dim dt As New DataTable
        Dim dtSWSET As New WarrantyDTO.SWSETDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " a.sw_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If sw_id.ToString().Trim() <> "" Then
                oQuery.addWHERE("sw_id", ":sw_id", sw_id, OracleType.VarChar)
                sCondition = sCondition & " AND a.sw_id=:sw_id"
            End If

            If OperationCenter.ToString().Trim() <> "" Then
                oQuery.addWHERE("OperationCenter", ":OperationCenter", OperationCenter, OracleType.VarChar)
                sCondition = sCondition & " AND a.SW_COMPNO=:OperationCenter"
            End If

            If ProductGroup.ToString().Trim() <> "" Then
                oQuery.addWHERE("ProductGroup", ":ProductGroup", ProductGroup, OracleType.VarChar)
                sCondition = sCondition & " AND a.sw_group=:ProductGroup"
            End If

            If WarrantyType > 0 Then
                oQuery.addWHERE("WarrantyType", ":WarrantyType", WarrantyType, OracleType.Number)
                sCondition = sCondition & " AND SW_TYPE01=:WarrantyType"
            End If

            If IsValid = "Y" Then
                sCondition = sCondition & " AND nvl(a.SW_Expdate,sysdate)>=sysdate"
            End If

            oQuery.addWHERE("sw_mark", ":sw_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT a.*,b.COMP_NAME FROM SWSet a,COMPANY b WHERE a.SW_COMPNO=b.COMP_NO AND a.sw_mark=:sw_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtSWSET)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtSWSET
    End Function

    ''' <summary>
    ''' Special文件檔
    ''' </summary>
    ''' <param name="swf_id"></param>
    ''' <param name="swf_file"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySWFile(ByVal swf_id As String, ByVal swf_file As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.SWFILEDataTable

        Dim dt As New DataTable
        Dim dtSWFile As New WarrantyDTO.SWFILEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " swf_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If swf_id.Trim() <> "" Then
                oQuery.addWHERE("SWF_ID", ":SWF_ID", swf_id, OracleType.VarChar)
                sCondition = sCondition & " AND SWF_ID=:SWF_ID"
            End If

            If swf_file.Trim() <> "" Then
                oQuery.addWHERE("swf_file", ":swf_file", swf_file, OracleType.VarChar)
                sCondition = sCondition & " AND swf_file=:swf_file"
            End If

            oQuery.addWHERE("swf_mark", ":swf_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM SWFile WHERE swf_mark=:swf_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtSWFile)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtSWFile
    End Function

    ''' <summary>
    ''' Special料號資料檔
    ''' </summary>
    ''' <param name="swp_id"></param>
    ''' <param name="swp_partno"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuerySWParts(ByVal swp_id As String, ByVal swp_partno As String, ByVal SWP_FLAG As Integer, Optional ByVal OrderBY As String = "") As WarrantyDTO.SWPARTSDataTable

        Dim dt As New DataTable
        Dim dtSWParts As New WarrantyDTO.SWPARTSDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " swp_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If swp_id.Trim() <> "" Then
                oQuery.addWHERE("swp_id", ":swp_id", swp_id, OracleType.VarChar)
                sCondition = sCondition & " AND swp_id=:swp_id"
            End If

            If swp_partno.Trim() <> "" Then
                oQuery.addWHERE("swp_partno", ":swp_partno", swp_partno, OracleType.VarChar)
                sCondition = sCondition & " AND swp_partno=:swp_partno"
            End If

            If SWP_FLAG > -1 Then
                oQuery.addWHERE("SWP_FLAG", ":SWP_FLAG", SWP_FLAG, OracleType.Int16)
                sCondition = sCondition & " AND SWP_FLAG=:SWP_FLAG"
            End If

            oQuery.addWHERE("swp_mark", ":swp_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM SWParts WHERE swp_mark=:swp_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtSWParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtSWParts
    End Function


    ''' <summary>
    ''' 取得保固版本-版號
    ''' </summary>
    ''' <param name="sSerial_no"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWARVERSION(ByVal sSerial_no As String) As String

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If sSerial_no.Trim() <> "" Then
                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", sSerial_no, OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO = :EXPORT_SERIALNO"
            End If

            Dim sSQL As StringBuilder = New StringBuilder()

            sSQL.AppendLine("SELECT DISTINCT b.ta_oeb081 FROM export a ")
            sSQL.AppendLine("JOIN cipherlab.oeb_file b ON b.OEB01 || '-' || b.OEB03 = a.EXPORT_ORDERNUMBER")
            sSQL.AppendLine("WHERE 1 = 1 ")
            sSQL.AppendLine(sCondition)

            dt = oQuery.ExecuteDT(sSQL.ToString())
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("ta_oeb081").ToString()
            Else
                Return String.Empty
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function


    ''' <summary>
    ''' QueryWARRSETALL 保固設定檔單頭
    ''' </summary>
    Public Function QueryWARRSETALL() As DataTable

        Dim dt As New DataTable
        Dim dtWarrSet As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            ' 保固卡加入Warranty Card Content by buck add 20260128 begin
            'Dim sSQL As String = " select WAR_ID	,WAR_NAME	,WAR_COMPNO,	WAR_GROUP,	WAR_VERSION	, WAR_TYPE	,WAR_DISCOUNT	,WAR_EXTMM	,WAR_STDYY	,WAR_LONGYY	,WAR_DESC	,WAR_ISALL	,WAR_STATUS	,WAR_AD	,WAR_ADNAME	,WAR_CSTMP	,WAR_LUAD	,WAR_LUADNAME	,WAR_LUSTMP	,WAR_MARK	,WAR_EF_ID	,WAR_COPYCN from WARRSET "
            Dim sSQL As String = " select WAR_ID	,WAR_NAME	,WAR_COMPNO,	WAR_GROUP,	WAR_VERSION	, WAR_TYPE	,WAR_DISCOUNT	,WAR_EXTMM	,WAR_STDYY	,WAR_LONGYY	,WAR_DESC	,WAR_ISALL	,WAR_STATUS	,WAR_AD	,WAR_ADNAME	,WAR_CSTMP	,WAR_LUAD	,WAR_LUADNAME	,WAR_LUSTMP	,WAR_MARK	,WAR_EF_ID	,WAR_COPYCN ,WAR_CARD_CONTENT from WARRSET "
            ' 保固卡加入Warranty Card Content by buck add 20260128 end
            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    '優化效能 by buck modify 20260323 begin
    ''' <summary>
    ''' WarrSet 保固設定檔單頭
    ''' </summary>
    ''' <param name="war_id"></param>
    ''' <param name="OperationCenter"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>	
    Public Function QueryWarrSet(ByVal war_id As String, ByVal OperationCenter As String, ByVal ProductGroup As String, ByVal WarrantyType As String, ByVal CustNo As String, ByVal IsValid As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRSETDataTable

        Dim dt As New DataTable
        Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If String.IsNullOrEmpty(OrderBY.Trim()) Then
                OrderBY = " a.war_cstmp DESC"
            End If

            If Not String.IsNullOrEmpty(war_id.Trim()) Then
                oQuery.addWHERE("war_id", ":war_id", war_id, OracleType.VarChar)
                sCondition &= " AND a.war_id = :war_id"
            End If

            If Not String.IsNullOrEmpty(OperationCenter.Trim()) Then
                oQuery.addWHERE("OperationCenter", ":OperationCenter", OperationCenter, OracleType.VarChar)
                sCondition &= " AND a.war_compno = :OperationCenter"
            End If

            If Not String.IsNullOrEmpty(ProductGroup.Trim()) Then
                oQuery.addWHERE("ProductGroup", ":ProductGroup", ProductGroup, OracleType.VarChar)
                sCondition &= " AND a.war_group = :ProductGroup"
            End If

            If Not String.IsNullOrEmpty(WarrantyType) Then
                If WarrantyType = "CW" Then
                    sCondition &= " AND a.war_type IN ('CW','E0','P0','PB','EB','M0')"
                Else
                    oQuery.addWHERE("WarrantyType", ":WarrantyType", WarrantyType, OracleType.VarChar)
                    sCondition &= " AND a.war_type = :WarrantyType"
                End If
            End If

            If Not String.IsNullOrEmpty(CustNo) Then
                oQuery.addWHERE("CustNo", ":CustNo", CustNo, OracleType.VarChar)
                sCondition &= " AND (a.WAR_ISALL = 1 OR a.WAR_ID IN(SELECT WC_CLID FROM WCLIENT WHERE WC_CLNO=:CustNo))"
            End If

            If IsValid = "Y" Then
                sCondition &= " AND (a.WAR_Expdate IS NULL OR a.WAR_Expdate >= CURRENT_DATE)"
            End If

            oQuery.addWHERE("war_mark", ":war_mark", "0", OracleType.VarChar)

            Dim sSQL As String = "
                                    SELECT a.*, b.COMP_NAME 
                                    FROM WarrSet a
                                    INNER JOIN COMPANY b ON a.WAR_COMPNO = b.COMP_NO 
                                    WHERE a.war_mark = :war_mark {0}
                                    ORDER BY {1}
                                "
            sSQL = String.Format(sSQL, sCondition, OrderBY)

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrSet)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrSet
    End Function
    'Public Function QueryWarrSet(ByVal war_id As String, ByVal OperationCenter As String, ByVal ProductGroup As String, ByVal WarrantyType As String, ByVal CustNo As String, ByVal IsValid As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRSETDataTable

    '    Dim dt As New DataTable
    '    Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
    '    Dim oConn As New Connection
    '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)
    '    Dim sCondition As String = ""

    '    oConn.Open()
    '    Try
    '        If OrderBY.Trim = "" Then
    '            OrderBY = " war_cstmp desc"
    '        End If
    '        OrderBY = " ORDER BY " & OrderBY

    '        If war_id.Trim() <> "" Then
    '            oQuery.addWHERE("war_id", ":war_id", war_id, OracleType.VarChar)
    '            sCondition = sCondition & " AND a.war_id=:war_id"
    '        End If

    '        If OperationCenter.Trim() <> "" Then
    '            oQuery.addWHERE("OperationCenter", ":OperationCenter", OperationCenter, OracleType.VarChar)
    '            sCondition = sCondition & " AND a.war_compno=:OperationCenter"
    '        End If

    '        If ProductGroup.Trim() <> "" Then
    '            oQuery.addWHERE("ProductGroup", ":ProductGroup", ProductGroup, OracleType.VarChar)
    '            sCondition = sCondition & " AND a.war_group=:ProductGroup"
    '        End If

    '        If WarrantyType <> "" Then
    '            If (WarrantyType = "CW") Then 'CW=CW/E0/P0/PB/EB/M0
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeA", "CW", OracleType.VarChar)
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeB", "E0", OracleType.VarChar)
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeC", "P0", OracleType.VarChar)
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeD", "PB", OracleType.VarChar)
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeE", "EB", OracleType.VarChar)
    '                oQuery.addWHERE("WarrantyType", ":WarrantyTypeF", "M0", OracleType.VarChar)
    '                sCondition = sCondition & " AND (a.war_type=:WarrantyTypeA or a.war_type=:WarrantyTypeB or a.war_type=:WarrantyTypeC or a.war_type=:WarrantyTypeD or a.war_type=:WarrantyTypeE or a.war_type=:WarrantyTypeF)"
    '            Else ''CW/E0/P0/PB/EB/M0 單獨
    '                oQuery.addWHERE("WarrantyType", ":WarrantyType", WarrantyType, OracleType.VarChar)
    '                sCondition = sCondition & " AND a.war_type=:WarrantyType"
    '            End If

    '        End If


    '        If CustNo <> "" Then
    '            oQuery.addWHERE("CustNo", ":CustNo", CustNo, OracleType.VarChar)
    '            sCondition = sCondition & " AND (a.WAR_ISALL=1 OR a.WAR_ID IN(SELECT WC_CLID FROM WCLIENT WHERE WC_CLNO=:CustNo))"
    '        End If

    '        If IsValid = "Y" Then
    '            sCondition = sCondition & " AND nvl(a.WAR_Expdate,sysdate)>=sysdate"
    '        End If

    '        oQuery.addWHERE("war_mark", ":war_mark", "0", OracleType.Int16)

    '        Dim sSQL As String = "SELECT a.*,b.COMP_NAME FROM WarrSet a,COMPANY b WHERE a.WAR_COMPNO=b.COMP_NO AND a.war_mark=:war_mark " & sCondition & OrderBY

    '        dt = oQuery.ExecuteDT(sSQL)
    '        Common.TransferDataTable(dt, dtWarrSet)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        oConn.Close()
    '        oConn.Dispose()
    '    End Try

    '    Return dtWarrSet
    'End Function
    '優化效能 by buck modify 20260323 end

    ''' <summary>
    ''' WarrSet 保固設定檔單頭New
    ''' </summary>
    ''' <param name="OperationCenter"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWarrSetNew(ByVal OperationCenter As String, ByVal ProductGroup As String, ByVal WarrantyType As String, ByVal sItemType As String, ByVal sPriceVer As String, ByVal sProgramType As String, ByVal sWarrantyName As String, ByVal CustNo As String, ByVal sStatus As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRSETDataTable

        Dim dt As New DataTable
        Dim dtWarrSet As New WarrantyDTO.WARRSETDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " war_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            'If war_id.Trim() <> "" Then
            '    oQuery.addWHERE("war_id", ":war_id", war_id, OracleType.VarChar)
            '    sCondition = sCondition & " AND a.war_id=:war_id"
            'End If

            If OperationCenter.Trim() <> "" Then
                oQuery.addWHERE("OperationCenter", ":OperationCenter", OperationCenter, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_compno=:OperationCenter"
            End If

            If ProductGroup.Trim() <> "" Then
                oQuery.addWHERE("ProductGroup", ":ProductGroup", ProductGroup, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_group=:ProductGroup"
            End If

            If WarrantyType <> "" Then
                oQuery.addWHERE("WarrantyType", ":WarrantyType", WarrantyType, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_type=:WarrantyType"
            End If

            If sItemType <> "" Then
                oQuery.addWHERE("ItemType", ":ItemType", sItemType, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_item_type=:ItemType"
            End If

            If sProgramType <> "" Then
                oQuery.addWHERE("ProgramType", ":ProgramType", sProgramType, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_program_type=:ProgramType"
            End If

            If sPriceVer <> "" Then
                oQuery.addWHERE("PriceVer", ":PriceVer", sPriceVer, OracleType.VarChar)
                sCondition = sCondition & " AND a.war_price_ver=:PriceVer"
            End If

            If sWarrantyName <> "" Then
                oQuery.addWHERE("WAR_NAME", ":WAR_NAME", "%" + sWarrantyName + "%", OracleType.VarChar)
                sCondition = sCondition & " AND a.WAR_NAME LIKE :WAR_NAME"
            End If

            'If CustNo <> "" Then
            '    oQuery.addWHERE("CustNo", ":CustNo", "%" + CustNo + "%", OracleType.VarChar)
            '    'sCondition = sCondition & " AND (a.WAR_ISALL=1 OR a.WAR_ID IN (SELECT WC_CLID FROM WCLIENT WHERE WC_CLNO=:CustNo))"
            '    sCondition = sCondition & " AND (a.WAR_ISALL=1 OR a.WAR_ID IN (SELECT WC_CLID FROM WCLIENT a JOIN customer b ON b.CU_NO = a.WC_CLNO And (b.CU_NO Like :CustNo OR b.CU_NAME LIKE :CustNo)))"
            'End If

            If sStatus <> "" Then
                oQuery.addWHERE("WAR_STATUS", ":WAR_STATUS", sStatus, OracleType.VarChar)
                sCondition = sCondition & " AND a.WAR_STATUS= :WAR_STATUS"
            End If

            oQuery.addWHERE("war_mark", ":war_mark", "0", OracleType.Int16)

            Dim sSQL As String = ""
            If CustNo <> "" Then
                oQuery.addWHERE("CustNo", ":CustNo", "%" + CustNo + "%", OracleType.VarChar)

                sSQL = "SELECT a.*,b.COMP_NAME,d.cu_no,d.cu_name FROM WarrSet a JOIN COMPANY b ON a.WAR_COMPNO=b.COMP_NO "
                sSQL += "JOIN WCLIENT c ON c.WC_CLID = a.WAR_ID "
                sSQL += "JOIN CUSTOMER d ON d.cu_no = c.WC_CLNO AND (d.CU_NO Like :CustNo OR d.CU_NAME LIKE :CustNo)"
            Else
                sSQL = "SELECT a.*,b.COMP_NAME,'' cu_no, '' cu_name FROM WarrSet a JOIN COMPANY b ON a.WAR_COMPNO=b.COMP_NO "
            End If

            sSQL += "WHERE a.war_mark=:war_mark " + sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrSet)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrSet
    End Function

    ''' <summary>
    ''' WarrPrice  保固設定檔單身
    ''' </summary>
    ''' <param name="wp_id"></param>
    ''' <param name="wp_compno"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWPrice(ByVal wp_id As String, ByVal wp_compno As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WPRICEDataTable

        Dim dt As New DataTable
        Dim dtWPrice As New WarrantyDTO.WPRICEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " wp_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If wp_id.Trim() <> "" Then
                oQuery.addWHERE("wp_id", ":wp_id", wp_id, OracleType.VarChar)
                sCondition = sCondition & " AND wp_id=:wp_id"
            End If

            If wp_compno.Trim() <> "" Then
                oQuery.addWHERE("wp_compno", ":wp_compno", wp_compno, OracleType.VarChar)
                sCondition = sCondition & " AND wp_compno=:wp_compno"
            End If

            oQuery.addWHERE("wp_mark", ":wp_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WPrice WHERE wp_mark=:wp_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWPrice)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWPrice
    End Function

    ''' <summary>
    ''' 保固客戶檔
    ''' </summary>
    ''' <param name="WC_CLID"></param>
    ''' <param name="WC_CLNO"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWClient(ByVal WC_CLID As String, ByVal WC_CLNO As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WCLIENTDataTable

        Dim dt As New DataTable
        Dim dtWClient As New WarrantyDTO.WCLIENTDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " WC_CSTMP desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WC_CLID.Trim() <> "" Then
                oQuery.addWHERE("WC_CLID", ":WC_CLID", WC_CLID, OracleType.VarChar)
                sCondition = sCondition & " AND WC_CLID=:WC_CLID"
            End If

            If WC_CLNO.Trim() <> "" Then
                oQuery.addWHERE("WC_CLNO", ":WC_CLNO", WC_CLNO, OracleType.VarChar)
                sCondition = sCondition & " AND WC_CLNO=:WC_CLNO"
            End If

            oQuery.addWHERE("WC_MARK", ":WC_MARK", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WCLIENT WHERE WC_MARK=:WC_MARK " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWClient)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWClient
    End Function

    ''' <summary>
    ''' 價格變動記錄檔
    ''' </summary>
    ''' <param name="wpl_seq"></param>
    ''' <param name="wpl_id"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWPriceLog(ByVal wpl_seq As Integer, ByVal wpl_id As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WPRICELOGDataTable

        Dim dt As New DataTable
        Dim dtWPriceLog As New WarrantyDTO.WPRICELOGDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " wp_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If wpl_seq > 0 Then
                oQuery.addWHERE("wpl_seq", ":wpl_seq", wpl_seq, OracleType.Number)
                sCondition = sCondition & " AND wpl_seq=:wpl_seq"
            End If

            If wpl_id.Trim() <> "" Then
                oQuery.addWHERE("wpl_id", ":wpl_id", wpl_id, OracleType.VarChar)
                sCondition = sCondition & " AND wpl_id=:wpl_id"
            End If

            oQuery.addWHERE("wpl_mark", ":wpl_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WPriceLog WHERE wpl_mark=:wpl_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWPriceLog)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWPriceLog
    End Function

    ''' <summary>
    ''' 保固訂單單頭檔
    ''' MODI BY Angel ON 20151114  增加CL_CHINA
    ''' </summary>
    ''' <param name="WoNo"></param>
    ''' <param name="WoComp"></param>
    ''' <param name="WoCust"></param>
    ''' <param name="WoStart"></param>
    ''' <param name="WoEnd"></param>
    ''' <param name="WoErpNo"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWorder(ByVal WoNo As String, ByVal WoComp As String, ByVal WoCust As String, ByVal WoStart As String, ByVal WoEnd As String, ByVal WoErpNo As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRANTYORDDataTable

        Dim dt As New DataTable
        Dim dtWorder As New WarrantyDTO.WARRANTYORDDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sSQL As String = ""
        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " a.waty_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WoNo.Trim() <> "" Then
                oQuery.addWHERE("wo_no", ":wo_no", WoNo, OracleType.VarChar)
                sCondition = sCondition & " AND a.waty_no=:wo_no"
            End If

            If WoComp.Trim() <> "" Then
                oQuery.addWHERE("wo_comp", ":wo_comp", WoComp, OracleType.VarChar)
                sCondition = sCondition & " AND a.waty_compno=:wo_comp"
            End If

            If WoCust.Trim() <> "" Then
                oQuery.addWHERE("wo_cust", ":wo_cust", WoCust, OracleType.VarChar)
                sCondition = sCondition & " AND a.waty_cust=:wo_cust"
            End If

            If WoErpNo.Trim() <> "" Then
                oQuery.addWHERE("wo_erpno", ":wo_erpno", WoErpNo, OracleType.VarChar)
                sCondition = sCondition & " AND a.waty_erpno=:wo_erpno"
            End If

            If WoStart.Trim() <> "" Then
                oQuery.addWHERE("WoStart", ":WoStart", Convert.ToDateTime(WoStart), OracleType.DateTime)
                sCondition = sCondition & " AND a.waty_date >=:WoStart"
            End If

            If WoEnd.Trim() <> "" Then
                oQuery.addWHERE("WoEnd", ":WoEnd", Convert.ToDateTime(WoEnd), OracleType.DateTime)
                sCondition = sCondition & " AND a.waty_date <=:WoEnd"
            End If


            oQuery.addWHERE("waty_mark", ":waty_mark", "0", OracleType.Int16)

            If WoComp = "CL_CHINA" Then 'MODI BY Angel ON 20151114  增加CL_CHINA
                sSQL = "SELECT a.*,b.occ02 CU_NAME,b.occ04,c.gen02 FROM WarrantyOrd a,ciphersh.occ_file b,ciphersh.gen_file c WHERE a.waty_cust=b.occ01 and a.WATY_SALESID=c.gen01(+) and a.waty_mark=:waty_mark " & sCondition & OrderBY
            Else
                sSQL = "SELECT a.*,b.occ02 CU_NAME,b.occ04,c.gen02 FROM WarrantyOrd a,cipherlab.occ_file b,cipherlab.gen_file c WHERE a.waty_cust=b.occ01 and a.WATY_SALESID=c.gen01(+) and a.waty_mark=:waty_mark " & sCondition & OrderBY

            End If

            'Dim sSQL As String = "SELECT a.*,b.occ02 CU_NAME,b.occ04,c.gen02 FROM WarrantyOrd a,cipherlab.occ_file b,cipherlab.gen_file c WHERE a.waty_cust=b.occ01 and a.WATY_SALESID=c.gen01(+) and a.waty_mark=:waty_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWorder)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWorder
    End Function

    ''' <summary>
    ''' 保固訂單單身檔
    ''' </summary>
    ''' <param name="wod_no"></param>
    ''' <param name="wod_item"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWodetail(ByVal wod_no As String, ByVal wod_item As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WODETAILDataTable

        Dim dt As New DataTable
        Dim dtWodetail As New WarrantyDTO.WODETAILDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " wod_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If wod_no.Trim() <> "" Then
                oQuery.addWHERE("wod_no", ":wod_no", wod_no, OracleType.VarChar)
                sCondition = sCondition & " AND wod_no=:wod_no"
            End If

            If wod_item.Trim() <> "" Then
                oQuery.addWHERE("wod_item", ":wod_item", wod_item, OracleType.VarChar)
                sCondition = sCondition & " AND wod_item=:wod_item"
            End If

            oQuery.addWHERE("wod_mark", ":wod_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM Wodetail WHERE wod_mark=:wod_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWodetail)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWodetail
    End Function

    ''' <summary>
    ''' 保固訂單序號檔
    ''' </summary>
    ''' <param name="wos_no"></param>
    ''' <param name="wos_item"></param>
    ''' <param name="wos_sn"></param>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWoSerial(ByVal wos_no As String, ByVal wos_item As String, ByVal wos_sn As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WOSERIALDataTable

        Dim dt As New DataTable
        Dim dtWoSerial As New WarrantyDTO.WOSERIALDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " wod_cstmp desc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If wos_no.Trim() <> "" Then
                oQuery.addWHERE("wos_no", ":wos_no", wos_no, OracleType.VarChar)
                sCondition = sCondition & " AND wos_no=:wos_no"
            End If

            If wos_item.Trim() <> "" Then
                oQuery.addWHERE("wos_item", ":wos_item", wos_item, OracleType.VarChar)
                sCondition = sCondition & " AND wos_item=:wos_item"
            End If

            If wos_sn.Trim() <> "" Then
                oQuery.addWHERE("wos_sn", ":wos_sn", wos_sn, OracleType.VarChar)
                sCondition = sCondition & " AND wos_sn=:wos_sn"
            End If


            oQuery.addWHERE("wos_mark", ":wos_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WoSerial WHERE wos_mark=:wos_mark " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWoSerial)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWoSerial
    End Function


    ''' <summary>
    ''' QueryExport
    ''' </summary>
    ''' <param name="SerialNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryExport(ByVal SerialNo As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If SerialNo.Trim() <> "" Then
                oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", SerialNo, OracleType.VarChar)
                sCondition = sCondition & " AND EXPORT_SERIALNO=:EXPORT_SERIALNO"
            End If

            'Isaac Mod Orderby
            'by Hugh 20231106, 去除RRMA的單號, 抓取最後出貨的記錄(多筆出貨記錄時)
            Dim sSQL As String = "SELECT a.*, b.ima021,NVL(c.WAR_TYPE,'') WAR_TYPE,NVL(a.EXPORT_WAR_ID,'') EXPORT_WAR_ID,NVL(c.PROGRAM_TYPE_NAME,'') WAR_PROGRAM_TYPE,NVL(c.ITEM_TYPE_NAME,'') WAR_ITEM_TYPE,NVL(c.PRICE_VER_NAME,'') WAR_PRICE_VER " &
                                 " FROM Export a " &
                                 " join cipherlab.ima_file b on export_partno=ima01 " &
                                 " LEFT JOIN (SELECT a.WAR_ID,a.WAR_TYPE,b.ITEM_TYPE_NAME,c.PROGRAM_TYPE_NAME,d.PRICE_VER_NAME FROM WARRSET a " &
                                 "            JOIN WARRSET_ITEM_TYPE b ON b.WARRSET_TYPE = a.WAR_TYPE AND b.ITEM_TYPE = a.WAR_ITEM_TYPE " &
                                 "            JOIN WARRSET_PROGRAM_TYPE c ON c.WARRSET_TYPE = a.WAR_TYPE AND c.PROGRAM_TYPE = a.WAR_PROGRAM_TYPE " &
                                 "            JOIN WARRSET_PRICE_VER d ON d.WARRSET_TYPE = a.WAR_TYPE AND d.PRICE_VER = a.WAR_PRICE_VER ) c ON c.WAR_ID = a.EXPORT_WAR_ID " &
                                 " WHERE EXPORT_SERIALNO Is Not NULL " & sCondition & " and export_salesnumber not like 'RRMA%' ORDER BY EXPORT_CSTMP DESC, EXPORT_CSTMP DESC"

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function QueryWarrantyPO(ByVal SerialNo As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If SerialNo.Trim() <> "" Then
                sCondition = sCondition & " And c.por_sn='" & SerialNo & "'"
            End If

            'by Hugh 20231103, 排序抓最新一筆
            'RMA加購保固
            '優化效能 by buck modify 20260323 begin
            'Dim sSQL As String = "SELECT a.waty_date,a.waty_no,b.wati_type,b.wati_name,c.por_sdate wats_warrnstart,c.por_edate wats_warrnend,b.wati_skuno,b.wati_model,c.por_sn wats_sn,b.wati_ver"
            'sSQL = sSQL & " FROM WarrantyOrd a,WarrantyItem b,PORECORD c"
            'sSQL = sSQL & " WHERE a.waty_no=b.wati_watyno and b.wati_watyno=c.por_order and b.wati_seq=c.por_ordseq" & sCondition
            'sSQL = sSQL & " AND a.ISConfirm='Y' order by waty_date desc"
            Dim sSQL = "
                        SELECT a.waty_date,a.waty_no,b.wati_type,b.wati_name,c.por_sdate wats_warrnstart,c.por_edate wats_warrnend,b.wati_skuno,b.wati_model,c.por_sn wats_sn,b.wati_ver
                        FROM WarrantyOrd a,WarrantyItem b,PORECORD c
                        WHERE a.waty_no=b.wati_watyno and b.wati_watyno=c.por_order and b.wati_seq=c.por_ordseq {0}
                        AND a.ISConfirm='Y' order by waty_date desc
                        "
            sSQL = String.Format(sSQL, sCondition)
            '優化效能 by buck modify 20260323 end

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function QueryWarrantyVer_TP(ByVal SerialNo As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If SerialNo.Trim() <> "" Then
                sCondition = sCondition & " And export_serialno ='" & SerialNo & "'"
            End If

            '抓tiptop隨貨出保固版號
            If (dt.Rows.Count = 0) Then
                Dim sSQL As String = "select export_war_id wati_ver "
                sSQL = sSQL & " from export"
                sSQL = sSQL & " where 1=1" & sCondition
                sSQL = sSQL & " order by export_cstmp desc  "
                dt = oQuery.ExecuteDT(sSQL)
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    ''' <summary>
    ''' SWSet--Add
    ''' </summary>
    ''' <param name="dtSWSet">傳入SWFILEDataTable</param>
    ''' <remarks></remarks>
    Public Function SaveAddSWSet(ByVal dtSWSet As WarrantyDTO.SWSETDataTable) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sSwID As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dr As WarrantyDTO.SWSETRow = dtSWSet.Rows(i)
            Dim dt As New DataTable
            Dim nVersionNo As Integer = 0
            Dim sSQL As String = "SELECT NVL(MAX(sw_version),-1) VersionNo FROM SWSet WHERE SW_GROUP='" & dr.SW_GROUP & "'"
            dt = oQuery.ExecuteDT(sSQL)
            nVersionNo = Convert.ToInt16(dt.Rows(0)("VersionNo").ToString()) + 1
            sSwID = dr.SW_GROUP & "SW" & nVersionNo.ToString("000") + dr.SW_STDYY.ToString()

            oExecute.addParameter("sw_id", sSwID, OracleType.VarChar)
            oExecute.addParameter("sw_name", dr.SW_NAME, OracleType.VarChar)
            oExecute.addParameter("sw_compno", dr.SW_COMPNO, OracleType.VarChar)
            oExecute.addParameter("sw_group", dr.SW_GROUP, OracleType.VarChar)
            oExecute.addParameter("sw_version", nVersionNo, OracleType.Number)
            oExecute.addParameter("sw_type01", dr.SW_TYPE01, OracleType.Number)
            oExecute.addParameter("sw_type02", dr.SW_TYPE02, OracleType.Number)
            oExecute.addParameter("sw_extmm", dr.SW_EXTMM, OracleType.Number)
            oExecute.addParameter("sw_stdyy", dr.SW_STDYY, OracleType.Number)
            oExecute.addParameter("sw_desc", dr.SW_DESC, OracleType.VarChar)
            oExecute.addParameter("sw_price", dr.SW_PRICE, OracleType.Number)
            oExecute.addParameter("sw_status", dr.SW_STATUS, OracleType.Number)
            If Not dr.IsSW_ExpdateNull Then
                oExecute.addParameter("SW_Expdate", dr.SW_Expdate, OracleType.DateTime)
            End If


            oExecute.addParameter("sw_ad", dr.SW_AD, OracleType.VarChar)
            oExecute.addParameter("sw_adname", dr.SW_ADNAME, OracleType.VarChar)
            oExecute.addParameter("sw_cstmp", dr.SW_CSTMP, OracleType.DateTime)
            oExecute.addParameter("sw_luad", dr.SW_LUAD, OracleType.VarChar)
            oExecute.addParameter("sw_luadname", dr.SW_LUADNAME, OracleType.VarChar)
            oExecute.addParameter("sw_lustmp", dr.SW_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("sw_mark", dr.SW_MARK, OracleType.Int16)

            oExecute.Command("SWSet", Execute.eumCommandType.AddNew)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sSwID
    End Function

    ''' <summary>
    ''' SWSet-修改
    ''' </summary>
    ''' <param name="dtSWSet">傳入SWFILEDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEditSWSet(ByVal dtSWSet As WarrantyDTO.SWSETDataTable)
        Dim i As Integer = 0
        Dim blnFlag As Boolean = False
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dr As WarrantyDTO.SWSETRow = dtSWSet.Rows(0)

        oConn.Open()
        Try
            oConn.BeginTransaction()


            oExecute.addParameter("sw_name", dr.SW_NAME, OracleType.VarChar)
            oExecute.addParameter("sw_compno", dr.SW_COMPNO, OracleType.VarChar)
            oExecute.addParameter("sw_group", dr.SW_GROUP, OracleType.VarChar)
            oExecute.addParameter("sw_version", dr.SW_VERSION, OracleType.VarChar)

            oExecute.addParameter("sw_type01", dr.SW_TYPE01, OracleType.Number)
            oExecute.addParameter("sw_type02", dr.SW_TYPE02, OracleType.Number)
            oExecute.addParameter("sw_extmm", dr.SW_EXTMM, OracleType.Number)
            oExecute.addParameter("sw_stdyy", dr.SW_STDYY, OracleType.Number)
            oExecute.addParameter("sw_desc", dr.SW_DESC, OracleType.VarChar)
            oExecute.addParameter("sw_price", dr.SW_PRICE, OracleType.Number)
            oExecute.addParameter("sw_status", dr.SW_STATUS, OracleType.Number)
            If Not dr.IsSW_ExpdateNull Then
                oExecute.addParameter("SW_Expdate", dr.SW_Expdate, OracleType.DateTime)
            End If

            oExecute.addParameter("sw_luad", dr.SW_LUAD, OracleType.VarChar)
            oExecute.addParameter("sw_luadname", dr.SW_LUADNAME, OracleType.VarChar)
            oExecute.addParameter("sw_lustmp", dr.SW_LUSTMP, OracleType.DateTime)

            oExecute.addWHERE("sw_id", dr.SW_ID, OracleType.VarChar)
            oExecute.Command("SWSet", Execute.eumCommandType.UPDATE)

            blnFlag = True

        Catch ex As Exception
            blnFlag = False
            Throw ex

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



    Public Sub SaveInvalidSWSet(ByVal sSWID As String, ByVal sAd As String, ByVal sAdName As String, ByVal nStatus As Integer)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()
            'Update SWSet
            oExecute.addParameter("SW_STATUS", nStatus, OracleType.Number)
            oExecute.addParameter("SW_LUAD", sAd, OracleType.VarChar)
            oExecute.addParameter("SW_LUADNAME", sAdName, OracleType.VarChar)
            oExecute.addParameter("SW_LUSTMP", Date.Now, OracleType.DateTime)
            oExecute.addWHERE("SW_ID", sSWID, OracleType.VarChar)
            oExecute.Command("SWSet", Execute.eumCommandType.UPDATE)
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
    ''' SWParts Add and Update
    ''' </summary>
    ''' <param name="dtSWParts">傳入SWPARTSDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveSWParts(ByVal dtSWParts As WarrantyDTO.SWPARTSDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            For i = 0 To dtSWParts.Rows.Count - 1

                Dim dr As WarrantyDTO.SWPARTSRow = dtSWParts.Rows(i)
                Dim dt As New DataTable

                Dim sSQL As String = "SELECT swp_id FROM SWParts WHERE swp_id='" & dr.SWP_ID & "' AND swp_partno='" & dr.SWP_PARTNO & "'"
                dt = oQuery.ExecuteDT(sSQL)

                If dt.Rows.Count > 0 Then
                    oExecute.addParameter("swp_luad", dr.SWP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("swp_luadname", dr.SWP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("swp_lustmp", dr.SWP_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("swp_mark", dr.SWP_MARK, OracleType.Int16)
                    oExecute.addParameter("SWP_FLAG", dr.SWP_FLAG, OracleType.Int16)
                    oExecute.addParameter("swp_desc", dr.SWP_DESC, OracleType.VarChar)
                    oExecute.addParameter("swp_year", dr.SWP_YEAR, OracleType.VarChar)

                    oExecute.addWHERE("swp_id", dr.SWP_ID, OracleType.VarChar)
                    oExecute.addWHERE("swp_partno", dr.SWP_PARTNO, OracleType.VarChar)
                    oExecute.Command("SWParts", Execute.eumCommandType.UPDATE)

                Else
                    oExecute.addParameter("swp_id", dr.SWP_ID, OracleType.VarChar)
                    oExecute.addParameter("swp_partno", dr.SWP_PARTNO, OracleType.VarChar)
                    oExecute.addParameter("swp_desc", dr.SWP_DESC, OracleType.VarChar)
                    oExecute.addParameter("swp_year", dr.SWP_YEAR, OracleType.VarChar)


                    oExecute.addParameter("swp_ad", dr.SWP_AD, OracleType.VarChar)
                    oExecute.addParameter("swp_adname", dr.SWP_ADNAME, OracleType.VarChar)
                    oExecute.addParameter("swp_cstmp", dr.SWP_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("swp_luad", dr.SWP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("swp_luadname", dr.SWP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("swp_lustmp", dr.SWP_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("swp_mark", dr.SWP_MARK, OracleType.Int16)
                    oExecute.addParameter("SWP_FLAG", dr.SWP_FLAG, OracleType.Int16)

                    oExecute.Command("SWParts", Execute.eumCommandType.AddNew)
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
    ''' SWParts--Edit
    ''' </summary>
    ''' <param name="dtSWParts">傳入SWPARTSDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveEditSWParts(ByVal dtSWParts As WarrantyDTO.SWPARTSDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()

        Try
            oConn.BeginTransaction()
            For i = 0 To dtSWParts.Rows.Count - 1
                Dim dr As WarrantyDTO.SWPARTSRow = dtSWParts.Rows(i)
                oExecute.addParameter("swp_year", dr.SWP_YEAR, OracleType.VarChar)
                oExecute.addParameter("swp_luad", dr.SWP_LUAD, OracleType.VarChar)
                oExecute.addParameter("swp_luadname", dr.SWP_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("swp_lustmp", dr.SWP_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("swp_id", dr.SWP_ID, OracleType.VarChar)
                oExecute.addWHERE("swp_partno", dr.SWP_PARTNO, OracleType.VarChar)
                oExecute.addWHERE("swp_mark", 0, OracleType.VarChar)
                oExecute.Command("SWParts", Execute.eumCommandType.UPDATE)
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
    ''' SWParts--Del
    ''' </summary>
    ''' <param name="dtSWParts">傳入SWPARTSDataTable</param>
    ''' <remarks></remarks>
    Public Sub SaveDelSWParts(ByVal dtSWParts As WarrantyDTO.SWPARTSDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        oConn.Open()

        Try
            oConn.BeginTransaction()
            For i = 0 To dtSWParts.Rows.Count - 1
                Dim dr As WarrantyDTO.SWPARTSRow = dtSWParts.Rows(i)
                oExecute.addParameter("SWP_FLAG", 0, OracleType.Number)
                oExecute.addParameter("swp_luad", dr.SWP_LUAD, OracleType.VarChar)
                oExecute.addParameter("swp_luadname", dr.SWP_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("swp_lustmp", dr.SWP_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("swp_id", dr.SWP_ID, OracleType.VarChar)
                oExecute.addWHERE("swp_partno", dr.SWP_PARTNO, OracleType.VarChar)
                oExecute.addWHERE("swp_mark", 0, OracleType.Number)
                oExecute.Command("SWParts", Execute.eumCommandType.UPDATE)
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
    ''' WarrSet--Add
    ''' </summary>
    ''' <param name="dtSWSet">傳入WARRSETDataTable</param>
    ''' <remarks></remarks>
    Public Function SaveAddWarrSet(ByVal dtSWSet As WarrantyDTO.WARRSETDataTable) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sSwID As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dr As WarrantyDTO.WARRSETRow = dtSWSet.Rows(i)
            Dim dt As New DataTable
            Dim nVersionNo As Integer = -1
            'Dim sSQL As String = "SELECT NVL(MAX(war_version),-1) VersionNo FROM WarrSet WHERE WAR_TYPE='" & dr.WAR_TYPE & "' AND war_group='" & dr.WAR_GROUP & "'"
            Dim sSQL As String = "SELECT NVL(MAX(war_version),-1) VersionNo FROM WarrSet WHERE WAR_TYPE=:WAR_TYPE AND WAR_GROUP=:WAR_GROUP AND WAR_ITEM_TYPE=:WAR_ITEM_TYPE "
            sSQL += " And WAR_PRICE_VER =: WAR_PRICE_VER And WAR_PROGRAM_TYPE=:WAR_PROGRAM_TYPE "

            oQuery.addWHERE("WAR_TYPE", ":WAR_TYPE", dr.WAR_TYPE, OracleType.VarChar)
            oQuery.addWHERE("WAR_GROUP", ":WAR_GROUP", dr.WAR_GROUP, OracleType.VarChar)
            oQuery.addWHERE("WAR_ITEM_TYPE", ":WAR_ITEM_TYPE", dr.WAR_ITEM_TYPE, OracleType.VarChar)
            oQuery.addWHERE("WAR_PRICE_VER", ":WAR_PRICE_VER", dr.WAR_PRICE_VER, OracleType.VarChar)
            oQuery.addWHERE("WAR_PROGRAM_TYPE", ":WAR_PROGRAM_TYPE", dr.WAR_PROGRAM_TYPE, OracleType.VarChar)

            dt = oQuery.ExecuteDT(sSQL)
            nVersionNo = Convert.ToInt16(dt.Rows(0)("VersionNo").ToString()) + 1

            '20221205 EW 也改為13碼
            'If dr.WAR_TYPE = "EW" Then
            '    sSwID = dr.WAR_GROUP & dr.WAR_TYPE & nVersionNo.ToString("000") + dr.WAR_STDYY.ToString() + dr.WAR_LONGYY.ToString()
            'Else
            '    sSwID = dr.WAR_GROUP & dr.WAR_TYPE & dr.WAR_PROGRAM_TYPE & dr.WAR_ITEM_TYPE & dr.WAR_PRICE_VER & nVersionNo.ToString("000") + dr.WAR_STDYY.ToString() + dr.WAR_LONGYY.ToString()
            'End If
            sSwID = dr.WAR_GROUP & dr.WAR_TYPE & dr.WAR_PROGRAM_TYPE & dr.WAR_ITEM_TYPE & dr.WAR_PRICE_VER & nVersionNo.ToString("000") + dr.WAR_STDYY.ToString() + dr.WAR_LONGYY.ToString()

            oExecute.addParameter("war_id", sSwID, OracleType.VarChar)
            oExecute.addParameter("war_name", dr.WAR_NAME, OracleType.VarChar)
            oExecute.addParameter("war_compno", dr.WAR_COMPNO, OracleType.VarChar)
            oExecute.addParameter("war_group", dr.WAR_GROUP, OracleType.VarChar)
            oExecute.addParameter("war_version", nVersionNo, OracleType.Number)
            oExecute.addParameter("war_type", dr.WAR_TYPE, OracleType.VarChar)
            oExecute.addParameter("war_discount", dr.WAR_DISCOUNT, OracleType.Number)
            oExecute.addParameter("war_extmm", dr.WAR_EXTMM, OracleType.Number)
            oExecute.addParameter("war_stdyy", dr.WAR_STDYY, OracleType.Number)
            oExecute.addParameter("war_longyy", dr.WAR_LONGYY, OracleType.Number)
            oExecute.addParameter("war_desc", dr.WAR_DESC, OracleType.VarChar)
            oExecute.addParameter("war_isall", dr.WAR_ISALL, OracleType.Number)
            oExecute.addParameter("war_status", dr.WAR_STATUS, OracleType.Number)
            If Not dr.IsWAR_ExpdateNull Then
                oExecute.addParameter("WAR_Expdate", dr.WAR_Expdate, OracleType.DateTime)
            End If

            oExecute.addParameter("war_ad", dr.WAR_AD, OracleType.VarChar)
            oExecute.addParameter("war_adname", dr.WAR_ADNAME, OracleType.VarChar)
            oExecute.addParameter("war_cstmp", dr.WAR_CSTMP, OracleType.DateTime)
            oExecute.addParameter("war_luad", dr.WAR_LUAD, OracleType.VarChar)
            oExecute.addParameter("war_luadname", dr.WAR_LUADNAME, OracleType.VarChar)
            oExecute.addParameter("war_lustmp", dr.WAR_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("war_mark", dr.WAR_MARK, OracleType.Int16)
            oExecute.addParameter("war_copycn", dr.WAR_COPYCN, OracleType.Int16)
            oExecute.addParameter("WAR_ITEM_TYPE", dr.WAR_ITEM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PROGRAM_TYPE", dr.WAR_PROGRAM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PRICE_VER", dr.WAR_PRICE_VER, OracleType.VarChar)
            oExecute.addParameter("WAR_SPEC_DESC", dr.WAR_SPEC_DESC, OracleType.VarChar)

            'If Not dr.IsWAR_REPAIR_NONull Then
            '    oExecute.addParameter("WAR_REPAIR_NO", dr.WAR_REPAIR_NO, OracleType.VarChar)
            'End If
            'If Not dr.IsWAR_BATTERY_SERIVCENull Then
            '    oExecute.addParameter("WAR_BATTERY_SERIVCE", dr.WAR_BATTERY_SERIVCE, OracleType.VarChar)
            'End If

            oExecute.Command("WarrSet", Execute.eumCommandType.AddNew)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sSwID
    End Function

    Public Sub SaveAddWarrSetPart(ByVal sWAR_ID As String)
        Dim oConn As New Connection
        Dim sSQL As String = ""

        oConn.Open()

        Dim retvalRate As String
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_Tranding_PartNO"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vWar_id", OracleType.VarChar).Value = sWAR_ID
            oCommand.Parameters("vWar_id").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.VarChar, 300)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            retvalRate = oCommand.Parameters("vResult").Value
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        'Try
        '    sSQL = " BEGIN "
        '    sSQL += "DELETE FROM WARR_PART WHERE WAR_PART_ID LIKE :WAR_IDDel; "

        '    sSQL += "INSERT INTO WARR_PART(WAR_PART_ID, WAR_PART_NO, WAR_PART_YEAR) "
        '    sSQL += "SELECT a.wp_id war_part_id,CASE WHEN WAR_TYPE='EW' THEN SUBSTR(a.wp_id,1,9) ELSE SUBSTR(a.wp_id,1,11) END||'11' war_part_no,1 war_part_year "
        '    sSQL += "FROM wprice a "
        '    sSQL += "JOIN warrset b ON b.WAR_ID = a.wp_id AND b.WAR_VERSION='0' "
        '    sSQL += "WHERE a.wp_id=:WAR_ID "
        '    sSQL += "AND a.wp_styy=1 AND CASE WHEN b.WAR_TYPE='EW' THEN a.wp_yy02 ELSE a.wp_yy01 END  <> 0; "

        '    sSQL += "INSERT INTO WARR_PART(WAR_PART_ID, WAR_PART_NO, WAR_PART_YEAR) "
        '    sSQL += "SELECT wp_id war_part_id,CASE WHEN WAR_TYPE='EW' THEN SUBSTR(a.wp_id,1,9) ELSE SUBSTR(a.wp_id,1,11) END||'12' war_part_no,2 war_part_year "
        '    sSQL += "FROM wprice "
        '    sSQL += "WHERE wp_id=:WAR_ID "
        '    sSQL += "AND wp_styy=1 AND wp_yy02 <> 0; "

        '    sSQL += "INSERT INTO WARR_PART(WAR_PART_ID, WAR_PART_NO, WAR_PART_YEAR) "
        '    sSQL += "SELECT wp_id war_part_id,CASE WHEN WAR_TYPE='EW' THEN SUBSTR(a.wp_id,1,9) ELSE SUBSTR(a.wp_id,1,11) END||'13' war_part_no,3 war_part_year "
        '    sSQL += "FROM wprice "
        '    sSQL += "WHERE wp_id=:WAR_ID "
        '    sSQL += "AND wp_styy=1 AND wp_yy03 <> 0; "

        '    sSQL += "INSERT INTO WARR_PART(WAR_PART_ID, WAR_PART_NO, WAR_PART_YEAR) "
        '    sSQL += "SELECT wp_id war_part_id,CASE WHEN WAR_TYPE='EW' THEN SUBSTR(a.wp_id,1,9) ELSE SUBSTR(a.wp_id,1,11) END||'14' war_part_no,4 war_part_year "
        '    sSQL += "FROM wprice "
        '    sSQL += "WHERE wp_id=:WAR_ID "
        '    sSQL += "AND wp_styy=1 AND wp_yy04 <> 0; "

        '    sSQL += "INSERT INTO WARR_PART(WAR_PART_ID, WAR_PART_NO, WAR_PART_YEAR) "
        '    sSQL += "SELECT wp_id war_part_id,CASE WHEN WAR_TYPE='EW' THEN SUBSTR(a.wp_id,1,9) ELSE SUBSTR(a.wp_id,1,11) END||'15' war_part_no,5 war_part_year "
        '    sSQL += "FROM wprice "
        '    sSQL += "WHERE wp_id=:WAR_ID "
        '    sSQL += "AND wp_styy=1 AND wp_yy05 <> 0; "
        '    sSQL += "END; "

        '    oCommand.Parameters.Add("WAR_IDDel", OracleType.NVarChar).Value = sWAR_ID.Substring(0, 11) + "%"
        '    oCommand.Parameters.Add("WAR_ID", OracleType.NVarChar).Value = sWAR_ID
        '    oCommand.CommandText = sSQL.ToString()
        '    oCommand.ExecuteNonQuery()

        'Catch ex As Exception
        '    Throw ex

        'Finally
        '    oConn.Close()
        '    oConn.Dispose()
        'End Try

    End Sub

    Public Function SaveEditWarrSet(ByVal dtSWSet As WarrantyDTO.WARRSETDataTable) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim sSwID As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dr As WarrantyDTO.WARRSETRow = dtSWSet.Rows(i)

            oExecute.addParameter("war_name", dr.WAR_NAME, OracleType.VarChar)
            oExecute.addParameter("war_compno", dr.WAR_COMPNO, OracleType.VarChar)
            oExecute.addParameter("war_group", dr.WAR_GROUP, OracleType.VarChar)
            oExecute.addParameter("war_version", dr.WAR_VERSION, OracleType.Number)
            oExecute.addParameter("war_type", dr.WAR_TYPE, OracleType.VarChar)
            oExecute.addParameter("war_discount", dr.WAR_DISCOUNT, OracleType.Number)
            oExecute.addParameter("war_extmm", dr.WAR_EXTMM, OracleType.Number)
            oExecute.addParameter("war_stdyy", dr.WAR_STDYY, OracleType.Number)
            oExecute.addParameter("war_longyy", dr.WAR_LONGYY, OracleType.Number)
            oExecute.addParameter("war_desc", dr.WAR_DESC, OracleType.VarChar)
            oExecute.addParameter("war_isall", dr.WAR_ISALL, OracleType.Number)
            oExecute.addParameter("war_status", dr.WAR_STATUS, OracleType.Number)
            oExecute.addParameter("war_luad", dr.WAR_LUAD, OracleType.VarChar)
            oExecute.addParameter("war_luadname", dr.WAR_LUADNAME, OracleType.VarChar)
            oExecute.addParameter("war_lustmp", dr.WAR_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("war_mark", dr.WAR_MARK, OracleType.Int16)
            oExecute.addParameter("war_copycn", dr.WAR_COPYCN, OracleType.Int16)
            If Not dr.IsWAR_ExpdateNull Then
                oExecute.addParameter("WAR_Expdate", dr.WAR_Expdate, OracleType.DateTime)
            End If
            oExecute.addParameter("WAR_ITEM_TYPE", dr.WAR_ITEM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PROGRAM_TYPE", dr.WAR_PROGRAM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PRICE_VER", dr.WAR_PRICE_VER, OracleType.VarChar)
            oExecute.addParameter("WAR_SPEC_DESC", dr.WAR_SPEC_DESC, OracleType.VarChar)
            oExecute.addParameter("WAR_CARD_CONTENT", dr.WAR_CARD_CONTENT, OracleType.VarChar) ' 加入Warranty Card Content欄位 by buck add 20260128
            'If Not dr.IsWAR_REPAIR_NONull Then
            '    oExecute.addParameter("WAR_REPAIR_NO", dr.WAR_REPAIR_NO, OracleType.VarChar)
            'End If
            'If Not dr.IsWAR_BATTERY_SERIVCENull Then
            '    oExecute.addParameter("WAR_BATTERY_SERIVCE", dr.WAR_BATTERY_SERIVCE, OracleType.VarChar)
            'End If

            oExecute.addWHERE("war_id", dr.WAR_ID, OracleType.VarChar)
            oExecute.Command("WarrSet", Execute.eumCommandType.UPDATE)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sSwID
    End Function

    Public Sub SaveInvalidWarrSet(ByVal sWarrID As String, ByVal sAd As String, ByVal sAdName As String, ByVal nStatus As Integer)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()
            'Update SWSet
            oExecute.addParameter("WAR_STATUS", nStatus, OracleType.Number)
            oExecute.addParameter("WAR_LUAD", sAd, OracleType.VarChar)
            oExecute.addParameter("WAR_LUADNAME", sAdName, OracleType.VarChar)
            oExecute.addParameter("WAR_LUSTMP", Date.Now, OracleType.DateTime)
            oExecute.addWHERE("WAR_ID", sWarrID, OracleType.VarChar)
            oExecute.Command("WarrSet", Execute.eumCommandType.UPDATE)
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
    ''' SaveCopyWarrSet
    ''' </summary>
    ''' <param name="WAR_ID"></param>
    ''' <param name="AdID"></param>
    ''' <param name="AdName"></param>SaveAddWarrSet
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveCopyWarrSet(ByVal WAR_ID As String, ByVal AdID As String, ByVal AdName As String) As String
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sSwID As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dt As New DataTable
            Dim sSQL As String = "SELECT * FROM WarrSet WHERE WAR_ID='" & WAR_ID & "'"
            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count < 1 Then
                Return WAR_ID
            End If

            Dim WAR_NAME As String = dt.Rows(0)("WAR_NAME").ToString().Trim()
            Dim WAR_COMPNO As String = dt.Rows(0)("WAR_COMPNO").ToString().Trim()
            Dim WAR_GROUP As String = dt.Rows(0)("WAR_GROUP").ToString().Trim()
            Dim WAR_TYPE As String = dt.Rows(0)("WAR_TYPE").ToString().Trim()
            Dim WAR_DISCOUNT As String = dt.Rows(0)("WAR_DISCOUNT").ToString().Trim()
            Dim WAR_EXTMM As String = dt.Rows(0)("WAR_EXTMM").ToString().Trim()
            Dim WAR_STDYY As String = dt.Rows(0)("WAR_STDYY").ToString().Trim()
            Dim WAR_LONGYY As String = dt.Rows(0)("WAR_LONGYY").ToString().Trim()
            Dim WAR_DESC As String = dt.Rows(0)("WAR_DESC").ToString().Trim()
            'Dim WAR_ISALL As String = dt.Rows(0)("WAR_ISALL").ToString().Trim()
            Dim WAR_ITEM_TYPE As String = dt.Rows(0)("WAR_ITEM_TYPE").ToString().Trim()
            Dim WAR_PROGRAM_TYPE As String = dt.Rows(0)("WAR_PROGRAM_TYPE").ToString().Trim()
            Dim WAR_PRICE_VER As String = dt.Rows(0)("WAR_PRICE_VER").ToString().Trim()

            Dim nVersionNo As Integer = -1
            sSQL = "SELECT NVL(MAX(war_version),-1) VersionNo FROM WarrSet WHERE WAR_TYPE='" & WAR_TYPE & "' AND war_group='" & WAR_GROUP & "'"
            dt = oQuery.ExecuteDT(sSQL)
            nVersionNo = Convert.ToInt16(dt.Rows(0)("VersionNo").ToString()) + 1
            'sSwID = WAR_GROUP & WAR_TYPE & nVersionNo.ToString("000") + WAR_STDYY.ToString() + WAR_LONGYY.ToString()
            sSwID = WAR_GROUP & WAR_TYPE & WAR_PROGRAM_TYPE & WAR_ITEM_TYPE & WAR_PRICE_VER & nVersionNo.ToString("000") + WAR_STDYY.ToString() + WAR_LONGYY.ToString()

            oExecute.addParameter("war_id", sSwID, OracleType.VarChar)
            oExecute.addParameter("war_name", WAR_NAME, OracleType.VarChar)
            oExecute.addParameter("war_compno", WAR_COMPNO, OracleType.VarChar)
            oExecute.addParameter("war_group", WAR_GROUP, OracleType.VarChar)
            oExecute.addParameter("war_version", nVersionNo, OracleType.Number)
            oExecute.addParameter("war_type", WAR_TYPE, OracleType.VarChar)
            oExecute.addParameter("war_discount", WAR_DISCOUNT, OracleType.Number)
            oExecute.addParameter("war_extmm", WAR_EXTMM, OracleType.Number)
            oExecute.addParameter("war_stdyy", WAR_STDYY, OracleType.Number)
            oExecute.addParameter("war_longyy", WAR_LONGYY, OracleType.Number)
            oExecute.addParameter("war_desc", WAR_DESC, OracleType.VarChar)
            oExecute.addParameter("war_isall", 1, OracleType.Number)
            oExecute.addParameter("war_status", 0, OracleType.Number)

            oExecute.addParameter("war_ad", AdID, OracleType.VarChar)
            oExecute.addParameter("war_adname", AdName, OracleType.VarChar)
            oExecute.addParameter("war_cstmp", DateTime.Now, OracleType.DateTime)
            oExecute.addParameter("war_luad", AdID, OracleType.VarChar)
            oExecute.addParameter("war_luadname", AdName, OracleType.VarChar)
            oExecute.addParameter("war_lustmp", DateTime.Now, OracleType.DateTime)
            oExecute.addParameter("war_mark", 0, OracleType.Int16)

            oExecute.addParameter("WAR_ITEM_TYPE", WAR_ITEM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PROGRAM_TYPE", WAR_PROGRAM_TYPE, OracleType.VarChar)
            oExecute.addParameter("WAR_PRICE_VER", WAR_PRICE_VER, OracleType.VarChar)

            oExecute.Command("WarrSet", Execute.eumCommandType.AddNew)

            sSQL = "SELECT * FROM WPRICE WHERE WP_ID='" & WAR_ID & "'"
            dt = oQuery.ExecuteDT(sSQL)
            For i = 0 To dt.Rows.Count - 1
                oExecute.addParameter("wp_id", sSwID, OracleType.VarChar)
                oExecute.addParameter("wp_compno", dt.Rows(i)("WP_COMPNO").ToString(), OracleType.VarChar)
                oExecute.addParameter("wp_styy", dt.Rows(i)("WP_STYY").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy01", dt.Rows(i)("WP_YY01").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy02", dt.Rows(i)("WP_YY02").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy03", dt.Rows(i)("WP_YY03").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy04", dt.Rows(i)("WP_YY04").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy05", dt.Rows(i)("WP_YY05").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy06", dt.Rows(i)("WP_YY06").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy07", dt.Rows(i)("WP_YY07").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy08", dt.Rows(i)("WP_YY08").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy09", dt.Rows(i)("WP_YY09").ToString(), OracleType.Number)
                oExecute.addParameter("wp_yy10", dt.Rows(i)("WP_YY10").ToString(), OracleType.Number)
                oExecute.addParameter("wp_ad", AdID, OracleType.VarChar)
                oExecute.addParameter("wp_adname", AdName, OracleType.VarChar)
                oExecute.addParameter("wp_cstmp", DateTime.Now, OracleType.DateTime)
                oExecute.addParameter("wp_luad", AdID, OracleType.VarChar)
                oExecute.addParameter("wp_luadname", AdName, OracleType.VarChar)
                oExecute.addParameter("wp_lustmp", DateTime.Now, OracleType.DateTime)
                oExecute.addParameter("wp_mark", 0, OracleType.Int16)
                oExecute.Command("WPrice", Execute.eumCommandType.AddNew)
            Next

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sSwID
    End Function

    Public Sub SaveAddWPrice(ByVal dtWPrice As WarrantyDTO.WPRICEDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim nSeq As Integer = 0
            Dim sSQL As String = ""
            If (dtWPrice.Rows.Count > 0) Then
                sSQL = "SELECT NVL(MAX(wpl_seq),0) wpl_seq FROM WPriceLog WHERE wpl_id='" & dtWPrice.Rows(0)("WP_ID").ToString().Trim() & "'"
                dt = oQuery.ExecuteDT(sSQL)
                nSeq = Convert.ToInt16(dt.Rows(0)("wpl_seq").ToString())
            End If

            For i = 0 To dtWPrice.Rows.Count - 1
                Dim dr As WarrantyDTO.WPRICERow = dtWPrice.Rows(i)

                Dim nVersionNo As Integer = 1
                sSQL = "SELECT wp_id,WP_COMPNO,WP_STYY,WP_YY01,WP_YY02,WP_YY03,WP_YY04,WP_YY05,WP_YY06,WP_YY07,WP_YY08,WP_YY09,WP_YY10 FROM WPrice WHERE wp_id='" & dr.WP_ID & "' AND wp_styy=" & dr.WP_STYY
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then

                    If (dr.WP_YY01 <> Decimal.Parse(dt.Rows(0)("WP_YY01").ToString()) Or dr.WP_YY02 <> Decimal.Parse(dt.Rows(0)("WP_YY02").ToString()) Or dr.WP_YY03 <> Decimal.Parse(dt.Rows(0)("WP_YY03").ToString()) Or dr.WP_YY04 <> Decimal.Parse(dt.Rows(0)("WP_YY04").ToString()) Or dr.WP_YY05 <> Decimal.Parse(dt.Rows(0)("WP_YY05").ToString()) Or dr.WP_YY06 <> Decimal.Parse(dt.Rows(0)("WP_YY06").ToString()) Or dr.WP_YY07 <> Decimal.Parse(dt.Rows(0)("WP_YY07").ToString()) Or dr.WP_YY08 <> Decimal.Parse(dt.Rows(0)("WP_YY08").ToString()) Or dr.WP_YY09 <> Decimal.Parse(dt.Rows(0)("WP_YY09").ToString()) Or dr.WP_YY10 <> Decimal.Parse(dt.Rows(0)("WP_YY10").ToString())) Then
                        nSeq = nSeq + 1
                        oExecute.addParameter("wpl_seq", nSeq, OracleType.Number)
                        oExecute.addParameter("wpl_id", dr.WP_ID, OracleType.VarChar)
                        oExecute.addParameter("wpl_compno", dt.Rows(0)("WP_COMPNO").ToString(), OracleType.VarChar)
                        oExecute.addParameter("wpl_styy", dt.Rows(0)("WP_STYY").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy01", dt.Rows(0)("WP_YY01").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy02", dt.Rows(0)("WP_YY02").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy03", dt.Rows(0)("WP_YY03").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy04", dt.Rows(0)("WP_YY04").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy05", dt.Rows(0)("WP_YY05").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy06", dt.Rows(0)("WP_YY06").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy07", dt.Rows(0)("WP_YY07").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy08", dt.Rows(0)("WP_YY08").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy09", dt.Rows(0)("WP_YY09").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_yy10", dt.Rows(0)("WP_YY10").ToString(), OracleType.Number)
                        oExecute.addParameter("wpl_ad", dr.WP_LUAD, OracleType.VarChar)
                        oExecute.addParameter("wpl_adname", dr.WP_LUADNAME, OracleType.VarChar)
                        oExecute.addParameter("wpl_cstmp", Date.Now, OracleType.DateTime)
                        oExecute.addParameter("wpl_luad", dr.WP_LUAD, OracleType.VarChar)
                        oExecute.addParameter("wpl_luadname", dr.WP_LUADNAME, OracleType.VarChar)
                        oExecute.addParameter("wpl_lustmp", Date.Now, OracleType.DateTime)
                        oExecute.addParameter("wpl_mark", 0, OracleType.Int16)
                        oExecute.Command("WPriceLog", Execute.eumCommandType.AddNew)
                    End If


                    oExecute.addParameter("wp_compno", dr.WP_COMPNO, OracleType.VarChar)
                    oExecute.addParameter("wp_styy", dr.WP_STYY, OracleType.Number)
                    oExecute.addParameter("wp_yy01", dr.WP_YY01, OracleType.Number)
                    oExecute.addParameter("wp_yy02", dr.WP_YY02, OracleType.Number)
                    oExecute.addParameter("wp_yy03", dr.WP_YY03, OracleType.Number)
                    oExecute.addParameter("wp_yy04", dr.WP_YY04, OracleType.Number)
                    oExecute.addParameter("wp_yy05", dr.WP_YY05, OracleType.Number)
                    oExecute.addParameter("wp_yy06", dr.WP_YY06, OracleType.Number)
                    oExecute.addParameter("wp_yy07", dr.WP_YY07, OracleType.Number)
                    oExecute.addParameter("wp_yy08", dr.WP_YY08, OracleType.Number)
                    oExecute.addParameter("wp_yy09", dr.WP_YY09, OracleType.Number)
                    oExecute.addParameter("wp_yy10", dr.WP_YY10, OracleType.Number)
                    oExecute.addParameter("wp_luad", dr.WP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("wp_luadname", dr.WP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("wp_lustmp", dr.WP_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("wp_id", dr.WP_ID, OracleType.VarChar)
                    oExecute.addWHERE("wp_styy", dr.WP_STYY, OracleType.VarChar)
                    oExecute.Command("WPrice", Execute.eumCommandType.UPDATE)
                Else
                    oExecute.addParameter("wp_id", dr.WP_ID, OracleType.VarChar)
                    oExecute.addParameter("wp_compno", dr.WP_COMPNO, OracleType.VarChar)
                    oExecute.addParameter("wp_styy", dr.WP_STYY, OracleType.Number)
                    oExecute.addParameter("wp_yy01", dr.WP_YY01, OracleType.Number)
                    oExecute.addParameter("wp_yy02", dr.WP_YY02, OracleType.Number)
                    oExecute.addParameter("wp_yy03", dr.WP_YY03, OracleType.Number)
                    oExecute.addParameter("wp_yy04", dr.WP_YY04, OracleType.Number)
                    oExecute.addParameter("wp_yy05", dr.WP_YY05, OracleType.Number)
                    oExecute.addParameter("wp_yy06", dr.WP_YY06, OracleType.Number)
                    oExecute.addParameter("wp_yy07", dr.WP_YY07, OracleType.Number)
                    oExecute.addParameter("wp_yy08", dr.WP_YY08, OracleType.Number)
                    oExecute.addParameter("wp_yy09", dr.WP_YY09, OracleType.Number)
                    oExecute.addParameter("wp_yy10", dr.WP_YY10, OracleType.Number)
                    oExecute.addParameter("wp_ad", dr.WP_AD, OracleType.VarChar)
                    oExecute.addParameter("wp_adname", dr.WP_ADNAME, OracleType.VarChar)
                    oExecute.addParameter("wp_cstmp", dr.WP_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("wp_luad", dr.WP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("wp_luadname", dr.WP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("wp_lustmp", dr.WP_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("wp_mark", dr.WP_MARK, OracleType.Int16)
                    oExecute.Command("WPrice", Execute.eumCommandType.AddNew)
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

    Public Sub SaveInvalidWPrice(ByVal sWarID As String, ByVal sAd As String, ByVal sAdName As String)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dtWPrice As WarrantyDTO.WPRICEDataTable
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim nSeq As Integer = 0
            Dim sSQL As String = "SELECT NVL(MAX(wpl_seq),0) wpl_seq FROM WPriceLog WHERE wpl_id='" & sWarID & "'"
            dt = oQuery.ExecuteDT(sSQL)
            nSeq = Convert.ToInt16(dt.Rows(0)("wpl_seq").ToString())

            dtWPrice = QueryWPrice(sWarID, "", "")
            For i = 0 To dtWPrice.Rows.Count - 1
                Dim dr As WarrantyDTO.WPRICERow = dtWPrice.Rows(i)
                nSeq = nSeq + 1
                oExecute.addParameter("wpl_seq", nSeq, OracleType.Number)
                oExecute.addParameter("wpl_id", dr.WP_ID, OracleType.VarChar)
                oExecute.addParameter("wpl_compno", dr.WP_COMPNO, OracleType.VarChar)
                oExecute.addParameter("wpl_styy", dr.WP_STYY, OracleType.Number)
                oExecute.addParameter("wpl_yy01", dr.WP_YY01, OracleType.Number)
                oExecute.addParameter("wpl_yy02", dr.WP_YY02, OracleType.Number)
                oExecute.addParameter("wpl_yy03", dr.WP_YY03, OracleType.Number)
                oExecute.addParameter("wpl_yy04", dr.WP_YY04, OracleType.Number)
                oExecute.addParameter("wpl_yy05", dr.WP_YY05, OracleType.Number)
                oExecute.addParameter("wpl_yy06", dr.WP_YY06, OracleType.Number)
                oExecute.addParameter("wpl_yy07", dr.WP_YY07, OracleType.Number)
                oExecute.addParameter("wpl_yy08", dr.WP_YY08, OracleType.Number)
                oExecute.addParameter("wpl_yy09", dr.WP_YY09, OracleType.Number)
                oExecute.addParameter("wpl_yy10", dr.WP_YY10, OracleType.Number)
                oExecute.addParameter("wpl_ad", sAd, OracleType.VarChar)
                oExecute.addParameter("wpl_adname", sAdName, OracleType.VarChar)
                oExecute.addParameter("wpl_cstmp", Date.Now, OracleType.DateTime)
                oExecute.addParameter("wpl_luad", sAd, OracleType.VarChar)
                oExecute.addParameter("wpl_luadname", sAdName, OracleType.VarChar)
                oExecute.addParameter("wpl_lustmp", Date.Now, OracleType.DateTime)
                oExecute.addParameter("wpl_mark", dr.WP_MARK, OracleType.Int16)
                oExecute.Command("WPriceLog", Execute.eumCommandType.AddNew)
            Next

            oExecute.addParameter("wp_yy01", 0, OracleType.Number)
            oExecute.addParameter("wp_yy02", 0, OracleType.Number)
            oExecute.addParameter("wp_yy03", 0, OracleType.Number)
            oExecute.addParameter("wp_yy04", 0, OracleType.Number)
            oExecute.addParameter("wp_yy05", 0, OracleType.Number)
            oExecute.addParameter("wp_yy06", 0, OracleType.Number)
            oExecute.addParameter("wp_yy07", 0, OracleType.Number)
            oExecute.addParameter("wp_yy08", 0, OracleType.Number)
            oExecute.addParameter("wp_yy09", 0, OracleType.Number)
            oExecute.addParameter("wp_yy10", 0, OracleType.Number)
            oExecute.addParameter("wp_luad", sAd, OracleType.VarChar)
            oExecute.addParameter("wp_luadname", sAdName, OracleType.VarChar)
            oExecute.addParameter("wp_lustmp", Date.Now, OracleType.DateTime)

            oExecute.addWHERE("wp_id", sWarID, OracleType.VarChar)
            oExecute.Command("WPrice", Execute.eumCommandType.UPDATE)

            oConn.Commit()
        Catch ex As Exception
            oConn.Rollback()
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    Public Sub SaveAddWClient(ByVal dtWPrice As WarrantyDTO.WCLIENTDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()
            If dtWPrice.Rows.Count > 0 Then
                oExecute.addParameter("WC_MARK", 1, OracleType.Number)

                oExecute.addWHERE("WC_CLID", dtWPrice.Rows(0)("WC_CLID").ToString(), OracleType.VarChar)
                oExecute.Command("WCLIENT", Execute.eumCommandType.UPDATE)
            End If

            For i = 0 To dtWPrice.Rows.Count - 1
                Dim dr As WarrantyDTO.WCLIENTRow = dtWPrice.Rows(i)

                Dim nVersionNo As Integer = 1
                Dim sSQL As String = "SELECT WC_CLID FROM WCLIENT WHERE WC_CLID='" & dr.WC_CLID & "' AND WC_CLNO='" & dr.WC_CLNO + "'"
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    oExecute.addParameter("WC_CLNAME", dr.WC_CLNAME, OracleType.VarChar)
                    oExecute.addParameter("WC_MARK", dr.WC_MARK, OracleType.Number)
                    oExecute.addParameter("WC_LUAD", dr.WC_LUAD, OracleType.VarChar)
                    oExecute.addParameter("WC_LUADNAME", dr.WC_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("WC_LUSTMP", dr.WC_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("WC_CLID", dr.WC_CLID, OracleType.VarChar)
                    oExecute.addWHERE("WC_CLNO", dr.WC_CLNO, OracleType.VarChar)
                    oExecute.Command("WCLIENT", Execute.eumCommandType.UPDATE)
                Else
                    oExecute.addParameter("WC_CLID", dr.WC_CLID, OracleType.VarChar)
                    oExecute.addParameter("WC_CLNO", dr.WC_CLNO, OracleType.VarChar)
                    oExecute.addParameter("WC_CLNAME", dr.WC_CLNAME, OracleType.VarChar)
                    oExecute.addParameter("WC_AD", dr.WC_AD, OracleType.VarChar)
                    oExecute.addParameter("WC_ADNAME", dr.WC_ADNAME, OracleType.VarChar)
                    oExecute.addParameter("WC_CSTMP", dr.WC_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("WC_LUAD", dr.WC_LUAD, OracleType.VarChar)
                    oExecute.addParameter("WC_LUADNAME", dr.WC_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("WC_LUSTMP", dr.WC_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("WC_MARK", dr.WC_MARK, OracleType.Int16)
                    oExecute.Command("WCLIENT", Execute.eumCommandType.AddNew)
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
    ''' SaveAddWorder
    ''' </summary>
    ''' <param name="dtWorder"></param>
    ''' <remarks></remarks>
    Public Sub SaveAddWorder(ByVal dtWorder As WarrantyDTO.WORDERDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWorder.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WORDERRow = dtWorder.Rows(i)
                oExecute.addParameter("wo_no", dr.WO_NO, OracleType.VarChar)
                oExecute.addParameter("wo_comp", dr.WO_COMP, OracleType.VarChar)
                oExecute.addParameter("wo_cust", dr.WO_CUST, OracleType.VarChar)
                oExecute.addParameter("wo_date", dr.WO_DATE, OracleType.DateTime)
                oExecute.addParameter("wo_ordertype", dr.WO_ORDERTYPE, OracleType.VarChar)
                oExecute.addParameter("wo_curr", dr.WO_CURR, OracleType.VarChar)
                oExecute.addParameter("wo_erpno", dr.WO_ERPNO, OracleType.VarChar)
                oExecute.addParameter("wo_flow", dr.WO_FLOW, OracleType.VarChar)


                oExecute.addParameter("wo_ad", dr.WO_AD, OracleType.VarChar)
                oExecute.addParameter("wo_adname", dr.WO_ADNAME, OracleType.VarChar)
                oExecute.addParameter("wo_cstmp", dr.WO_CSTMP, OracleType.DateTime)
                oExecute.addParameter("wo_luad", dr.WO_LUAD, OracleType.VarChar)
                oExecute.addParameter("wo_luadname", dr.WO_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("wo_lustmp", dr.WO_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("wo_mark", dr.WO_MARK, OracleType.Int16)

                oExecute.Command("Worder", Execute.eumCommandType.AddNew)

                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' SaveAddWodetail
    ''' </summary>
    ''' <param name="dtWodetail"></param>
    ''' <remarks></remarks>
    Public Sub SaveAddWodetail(ByVal dtWodetail As WarrantyDTO.WODETAILDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            For i = 0 To dtWodetail.Rows.Count - 1

                Dim dr As WarrantyDTO.WODETAILRow = dtWodetail.Rows(i)

                oExecute.addParameter("wod_no", dr.WOD_NO, OracleType.VarChar)
                oExecute.addParameter("wod_item", dr.WOD_ITEM, OracleType.Number)
                oExecute.addParameter("wod_orderno", dr.WOD_ORDERNO, OracleType.VarChar)
                oExecute.addParameter("wo_warrid", dr.WO_WARRID, OracleType.VarChar)
                oExecute.addParameter("wod_year", dr.WOD_YEAR, OracleType.Number)
                oExecute.addParameter("wod_skuno", dr.WOD_SKUNO, OracleType.VarChar)
                oExecute.addParameter("wod_skudesc", dr.WOD_SKUDESC, OracleType.VarChar)
                oExecute.addParameter("wod_type", dr.WOD_TYPE, OracleType.VarChar)
                oExecute.addParameter("wod_ver", dr.WOD_VER, OracleType.VarChar)
                oExecute.addParameter("wod_desc", dr.WOD_DESC, OracleType.VarChar)
                oExecute.addParameter("wod_qty", dr.WOD_QTY, OracleType.Number)
                oExecute.addParameter("wod_price", dr.WOD_PRICE, OracleType.Number)
                oExecute.addParameter("wod_base", dr.WOD_BASE, OracleType.Number)

                oExecute.addParameter("wod_ad", dr.WOD_AD, OracleType.VarChar)
                oExecute.addParameter("wod_adname", dr.WOD_ADNAME, OracleType.VarChar)
                oExecute.addParameter("wod_cstmp", dr.WOD_CSTMP, OracleType.DateTime)
                oExecute.addParameter("wod_luad", dr.WOD_LUAD, OracleType.VarChar)
                oExecute.addParameter("wod_luadname", dr.WOD_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("wod_lustmp", dr.WOD_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("wod_mark", dr.WOD_MARK, OracleType.Int16)

                oExecute.Command("Wodetail", Execute.eumCommandType.AddNew)
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
    ''' SaveAddWoSerial
    ''' </summary>
    ''' <param name="dtWoSerial"></param>
    ''' <remarks></remarks>
    Public Sub SaveAddWoSerial(ByVal dtWoSerial As WarrantyDTO.WOSERIALDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            For i = 0 To dtWoSerial.Rows.Count - 1

                Dim dr As WarrantyDTO.WOSERIALRow = dtWoSerial.Rows(i)

                oExecute.addParameter("wos_no", dr.WOS_NO, OracleType.VarChar)
                oExecute.addParameter("wos_item", dr.WOS_ITEM, OracleType.Number)
                oExecute.addParameter("wos_sn", dr.WOS_SN, OracleType.VarChar)

                oExecute.addParameter("wos_ad", dr.WOS_AD, OracleType.VarChar)
                oExecute.addParameter("wos_adname", dr.WOS_ADNAME, OracleType.VarChar)
                oExecute.addParameter("wos_cstmp", dr.WOS_CSTMP, OracleType.DateTime)
                oExecute.addParameter("wos_luad", dr.WOS_LUAD, OracleType.VarChar)
                oExecute.addParameter("wos_luadname", dr.WOS_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("wos_lustmp", dr.WOS_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("wos_mark", dr.WOS_MARK, OracleType.Int16)

                oExecute.Command("WoSerial", Execute.eumCommandType.AddNew)
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
    ''' SaveAddWarrantyOrd
    ''' </summary>
    ''' <param name="dtWarrantyOrd"></param>
    ''' <remarks></remarks>
    Public Function SaveAddWarrantyOrd(ByVal dtWarrantyOrd As WarrantyDTO.WARRANTYORDDataTable) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sWatyNo As String = ""
        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWarrantyOrd.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WARRANTYORDRow = dtWarrantyOrd.Rows(i)

                Dim dt As New DataTable
                Dim nNumber As Integer = 1
                Dim sPrefix As String = "WRMA-" + DateTime.Now.ToString("yyyyMM")
                Dim sSQL As String = "SELECT NVL(MAX(waty_no),0) waty_no FROM WarrantyOrd WHERE substr(waty_no,1,11)='" & sPrefix & "'"
                dt = oQuery.ExecuteDT(sSQL)
                nNumber = Convert.ToInt16(dt.Rows(0)("waty_no").ToString().Replace(sPrefix, "")) + 1
                sWatyNo = sPrefix & nNumber.ToString("0000")

                oExecute.addParameter("waty_no", sWatyNo, OracleType.VarChar)
                oExecute.addParameter("waty_compno", dr.WATY_COMPNO, OracleType.VarChar)
                oExecute.addParameter("waty_cust", dr.WATY_CUST, OracleType.VarChar)
                oExecute.addParameter("waty_date", dr.WATY_DATE, OracleType.DateTime)
                oExecute.addParameter("waty_ordertype", dr.WATY_ORDERTYPE, OracleType.VarChar)
                oExecute.addParameter("waty_curr", dr.WATY_CURR, OracleType.VarChar)
                oExecute.addParameter("waty_erpno", dr.WATY_ERPNO, OracleType.VarChar)
                oExecute.addParameter("waty_flow", dr.WATY_FLOW, OracleType.VarChar)

                If Not dr.IsWATY_SALESIDNull Then
                    oExecute.addParameter("WATY_SALESID", dr.WATY_SALESID, OracleType.VarChar)
                End If

                oExecute.addParameter("waty_ad", dr.WATY_AD, OracleType.VarChar)
                oExecute.addParameter("waty_adname", dr.WATY_ADNAME, OracleType.VarChar)
                oExecute.addParameter("waty_cstmp", dr.WATY_CSTMP, OracleType.DateTime)
                oExecute.addParameter("waty_luad", dr.WATY_LUAD, OracleType.VarChar)
                oExecute.addParameter("waty_luadname", dr.WATY_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("waty_lustmp", dr.WATY_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("waty_mark", dr.WATY_MARK, OracleType.Int16)
                oExecute.addParameter("ISFLOW", dr.ISFLOW, OracleType.VarChar)
                oExecute.addParameter("ISConfirm", dr.ISConfirm, OracleType.VarChar)
                oExecute.addParameter("waty_cust_po", dr.WATY_CUST_PO, OracleType.VarChar)

                oExecute.Command("WarrantyOrd", Execute.eumCommandType.AddNew)

                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sWatyNo
    End Function

    ''' <summary>
    ''' SaveEditWarrantyOrd
    ''' </summary>
    ''' <param name="dtWarrantyOrd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveEditWarrantyOrd(ByVal dtWarrantyOrd As WarrantyDTO.WARRANTYORDDataTable, ByVal bFlow As Boolean) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sWatyNo As String = ""
        Dim sSQL As String = ""
        Dim dt As DataTable
        oConn.Open()

        Try
            oConn.BeginTransaction()
            If dtWarrantyOrd.Rows.Count > 0 Then
                Dim drt As WarrantyDTO.WARRANTYORDRow = dtWarrantyOrd.Rows(0)
                sSQL = "SELECT a.wati_watyno,a.wati_seq,a.wati_qty"
                sSQL = sSQL & ",nvl((select count(t.wats_sn) from WarrantySerial t where a.wati_watyno=t.wats_watyno and a.wati_seq=t.wats_watyseq and t.wats_mark=0),0) SnQty"
                sSQL = sSQL & " FROM WarrantyItem a"
                sSQL = sSQL & " where a.wati_watyno='" & drt.WATY_NO & "'"
                sSQL = sSQL & " and a.wati_mark=0"
                sSQL = sSQL & " and (a.wati_qty<>nvl((select count(t.wats_sn) from WarrantySerial t where a.wati_watyno=t.wats_watyno and a.wati_seq=t.wats_watyseq and t.wats_mark=0),0)"
                sSQL = sSQL & " or a.wati_qty<1)"
                dt = oQuery.ExecuteDT(sSQL)

                If dt.Rows.Count > 0 Then
                    Throw New Exception("please check SerialNo!")
                End If
            End If

            If dtWarrantyOrd.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WARRANTYORDRow = dtWarrantyOrd.Rows(i)

                oExecute.addParameter("ISConfirm", dr.ISConfirm, OracleType.VarChar)
                oExecute.addParameter("waty_luad", dr.WATY_LUAD, OracleType.VarChar)
                oExecute.addParameter("waty_luadname", dr.WATY_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("waty_lustmp", dr.WATY_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("waty_mark", dr.WATY_MARK, OracleType.Int16)
                If bFlow Then
                    '需要流程簽核
                    oExecute.addParameter("waty_flow", "F", OracleType.VarChar)
                Else
                    '不需要流程簽核
                    oExecute.addParameter("waty_flow", "Y", OracleType.VarChar)
                End If
                oExecute.addWHERE("waty_no", dr.WATY_NO, OracleType.VarChar)
                oExecute.Command("WarrantyOrd", Execute.eumCommandType.UPDATE)

                '如果不需要簽核，則自動生效,調用 SP tx_po_appv,產生AR
                If Not bFlow Then
                    If dr.WATY_MARK = 0 Then
                        'Update Export SP
                        Dim cmd As OracleCommand = oConn.Command
                        cmd.CommandText = "tx_po_appv"
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Clear()

                        Dim p0 As OracleParameter = New OracleParameter("vWatyNo", OracleType.VarChar)
                        p0.Value = dr.WATY_NO
                        p0.Direction = System.Data.ParameterDirection.Input
                        cmd.Parameters.Add(p0)

                        Dim p1 As OracleParameter = New OracleParameter("vUserNo", OracleType.VarChar)
                        p1.Value = dr.WATY_LUAD
                        p1.Direction = System.Data.ParameterDirection.Input
                        cmd.Parameters.Add(p1)

                        'Dim pp2 As OracleParameter = New OracleParameter("vResult", OracleType.VarChar, 3000)
                        'pp2.Direction = System.Data.ParameterDirection.Output
                        'cmd.Parameters.Add(pp2)
                        Dim nResult As Integer = cmd.ExecuteNonQuery()

                        'Dim sResult As String = pp2.Value.ToString()
                        'If sResult <> "" Then
                        'Throw New Exception(sResult)
                        'End If
                    End If
                End If
                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sWatyNo
    End Function

    ''' <summary>
    ''' SaveAddWarrantyItem
    ''' </summary>
    ''' <param name="dtWarrantyItem"></param>
    ''' <remarks></remarks>
    Public Sub SaveAddWarrantyItem(ByVal dtWarrantyItem As WarrantyDTO.WARRANTYITEMDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""
        Dim nResult As Integer = 0

        oConn.Open()

        Try
            oConn.BeginTransaction()
            '

            For i = 0 To dtWarrantyItem.Rows.Count - 1
                Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.Rows(i)
                'Check UnConfirm Order
                If dr.WATI_ORDER.Trim() <> "" Then
                    sSQL = "SELECT a.wati_seq FROM WarrantyItem a,WarrantyOrd b WHERE a.wati_watyno=b.waty_no and a.wati_order='" & dr.WATI_ORDER & "' AND a.wati_ordseq=" & dr.WATI_ORDSEQ
                    sSQL = sSQL & " AND a.wati_mark=0 AND b.waty_mark=0 AND b.waty_flow!='Y'"
                    sSQL = sSQL & " AND a.wati_type='" & dr.WATI_TYPE & "'"
                    dt = oQuery.ExecuteDT(sSQL)
                    If dt.Rows.Count > 0 Then
                        Throw New Exception("This order item had been purchase,please confirm!")
                    End If
                End If

                'Insert into WarrantyItem
                Dim nWatySeq As Integer = 1
                sSQL = "SELECT NVL(MAX(wati_seq),0) wati_seq FROM WarrantyItem WHERE wati_watyno='" & dr.WATI_WATYNO & "'"
                dt = oQuery.ExecuteDT(sSQL)
                nWatySeq = Convert.ToInt16(dt.Rows(0)("wati_seq").ToString()) + 1

                oExecute.addParameter("wati_watyno", dr.WATI_WATYNO, OracleType.VarChar)
                oExecute.addParameter("wati_seq", nWatySeq, OracleType.VarChar)
                oExecute.addParameter("wati_order", dr.WATI_ORDER, OracleType.VarChar)
                If Not dr.IsWATI_ORDSEQNull Then
                    oExecute.addParameter("wati_ordseq", dr.WATI_ORDSEQ, OracleType.Number)
                End If

                oExecute.addParameter("wati_skuno", dr.WATI_SKUNO, OracleType.VarChar)
                oExecute.addParameter("wati_model", dr.wati_model, OracleType.VarChar)
                oExecute.addParameter("wati_name", dr.wati_name, OracleType.VarChar)
                oExecute.addParameter("wati_skudesc", dr.WATI_SKUDESC, OracleType.VarChar)
                oExecute.addParameter("wati_type", dr.WATI_TYPE, OracleType.VarChar)
                oExecute.addParameter("wati_ver", dr.WATI_VER, OracleType.VarChar)
                oExecute.addParameter("wati_desc", dr.WATI_DESC, OracleType.VarChar)

                oExecute.addParameter("wati_year", dr.WATI_YEAR, OracleType.Number)
                oExecute.addParameter("wati_qty", dr.WATI_QTY, OracleType.Number)
                oExecute.addParameter("wati_price", 0, OracleType.Number)
                oExecute.addParameter("wati_base", 0, OracleType.Number)
                oExecute.addParameter("wati_ad", dr.WATI_AD, OracleType.VarChar)
                oExecute.addParameter("wati_adname", dr.WATI_ADNAME, OracleType.VarChar)
                oExecute.addParameter("wati_cstmp", dr.WATI_CSTMP, OracleType.DateTime)
                oExecute.addParameter("wati_luad", dr.WATI_LUAD, OracleType.VarChar)
                oExecute.addParameter("wati_luadname", dr.WATI_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("wati_lustmp", dr.WATI_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("WATI_MARK", dr.WATI_MARK, OracleType.Int16)
                oExecute.Command("WarrantyItem", Execute.eumCommandType.AddNew)


                Dim cmd As OracleCommand = oConn.Command
                If dr.WATI_ORDER.Trim() <> "" Then
                    'Update Erp No
                    oExecute.addParameter("waty_erpno", dr.WATI_ORDER, OracleType.VarChar)
                    oExecute.addWHERE("waty_no", dr.WATI_WATYNO, OracleType.VarChar)
                    oExecute.Command("WarrantyOrd", Execute.eumCommandType.UPDATE)

                    'Get Packing System Serial Number(SP By Yu)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Clear()
                    cmd.CommandText = "tx_get_sn"

                    Dim p0 As OracleParameter = New OracleParameter("vWatyNo", OracleType.VarChar)
                    p0.Value = dr.WATI_WATYNO
                    p0.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(p0)

                    Dim p2 As OracleParameter = New OracleParameter("vWatiSeq", OracleType.Number)
                    p2.Value = nWatySeq
                    p2.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(p2)

                    Dim p3 As OracleParameter = New OracleParameter("vUserNo", OracleType.VarChar)
                    p3.Value = dr.WATI_LUAD
                    p3.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(p3)

                    Dim p4 As OracleParameter = New OracleParameter("vUserName", OracleType.VarChar)
                    p4.Value = dr.WATI_LUADNAME
                    p4.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(p4)

                    Dim pp2 As OracleParameter = New OracleParameter("vResult", OracleType.VarChar, 3000)
                    pp2.Direction = System.Data.ParameterDirection.Output
                    cmd.Parameters.Add(pp2)

                    nResult = cmd.ExecuteNonQuery()
                    Dim sResult As String = pp2.Value.ToString()
                    If sResult <> "" Then
                        Throw New Exception(sResult)
                    End If
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

    Public Sub SaveDelWarrantyItem(ByVal sWatyNo As String, ByVal nWatiSeq As Integer, ByVal sAd As String, ByVal sAdName As String)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()
            'Update WarrantyItem
            oExecute.addParameter("wati_mark", 1, OracleType.Number)
            oExecute.addParameter("wati_luad", sAd, OracleType.VarChar)
            oExecute.addParameter("wati_luadname", sAdName, OracleType.VarChar)
            oExecute.addParameter("wati_lustmp", Date.Now, OracleType.DateTime)
            oExecute.addWHERE("wati_watyno", sWatyNo, OracleType.VarChar)
            oExecute.addWHERE("wati_seq", nWatiSeq, OracleType.Number)
            oExecute.Command("WarrantyItem", Execute.eumCommandType.UPDATE)

            'Update WarrantyItem
            oExecute.addParameter("wats_mark", 1, OracleType.Number)
            oExecute.addParameter("wats_luad", sAd, OracleType.VarChar)
            oExecute.addParameter("wats_luadname", sAdName, OracleType.VarChar)
            oExecute.addParameter("wats_lustmp", Date.Now, OracleType.DateTime)
            oExecute.addWHERE("wats_watyno", sWatyNo, OracleType.VarChar)
            oExecute.addWHERE("wats_watyseq", nWatiSeq, OracleType.Number)
            oExecute.Command("WarrantySerial", Execute.eumCommandType.UPDATE)
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
    ''' ReloadWarrantyItemSN
    ''' </summary>
    ''' <param name="wati_watyno"></param>
    ''' <param name="wati_seq"></param>
    ''' <remarks></remarks>
    Public Sub ReloadWarrantyItemSN(ByVal wati_watyno As String, ByVal wati_seq As Integer)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            '
            Dim oWarranty As New ctlWarranty
            Dim dtData As New WarrantyDTO.WARRANTYITEMDataTable
            dtData = oWarranty.QueryWarrantyItem(wati_watyno, wati_seq, "")
            If dtData.Rows.Count < 1 Then Throw New Exception("Data Not find!!")

            Dim dr As WarrantyDTO.WARRANTYITEMRow = dtData(0)

            '調用SP,抓訂單序號并寫入
            Dim cmd As OracleCommand = oConn.Command
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            cmd.CommandText = "tx_get_sn"

            Dim p0 As OracleParameter = New OracleParameter("vWatyNo", OracleType.VarChar)
            p0.Value = dr.WATI_WATYNO
            p0.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(p0)

            Dim p2 As OracleParameter = New OracleParameter("vWatiSeq", OracleType.Number)
            p2.Value = dr.wati_seq
            p2.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(p2)

            Dim p3 As OracleParameter = New OracleParameter("vUserNo", OracleType.VarChar)
            p3.Value = dr.WATI_LUAD
            p3.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(p3)

            Dim p4 As OracleParameter = New OracleParameter("vUserName", OracleType.VarChar)
            p4.Value = dr.WATI_LUADNAME
            p4.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(p4)

            Dim pp2 As OracleParameter = New OracleParameter("vResult", OracleType.VarChar, 3000)
            pp2.Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(pp2)

            Dim nResult As Integer = cmd.ExecuteNonQuery()
            Dim sResult As String = pp2.Value.ToString()
            If sResult <> "" Then
                Throw New Exception(sResult)
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
    ''' SaveEditWarrantyItem
    ''' </summary>
    ''' <param name="dtWarrantyItem"></param>
    ''' <remarks></remarks>
    Public Sub SaveEditWarrantyItem(ByVal dtWarrantyItem As WarrantyDTO.WARRANTYITEMDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWarrantyItem.Rows.Count > 0 Then
                Dim drt As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.Rows(0)
                sSQL = "SELECT a.wati_watyno,a.wati_seq,a.wati_qty"
                sSQL = sSQL & ",nvl((select count(t.wats_sn) from WarrantySerial t where a.wati_watyno=t.wats_watyno and a.wati_seq=t.wats_watyseq and t.wats_mark=0),0) SnQty"
                sSQL = sSQL & " FROM WarrantyItem a"
                sSQL = sSQL & " where a.wati_watyno='" & drt.WATI_WATYNO & "'"
                sSQL = sSQL & " and a.wati_mark=0"
                sSQL = sSQL & " and a.wati_qty<>nvl((select count(t.wats_sn) from WarrantySerial t where a.wati_watyno=t.wats_watyno and a.wati_seq=t.wats_watyseq and t.wats_mark=0),0)"
                dt = oQuery.ExecuteDT(sSQL)

                If dt.Rows.Count > 0 Then
                    Throw New Exception("please check SerialNo!")
                End If
            End If

            For i = 0 To dtWarrantyItem.Rows.Count - 1
                Dim dr As WarrantyDTO.WARRANTYITEMRow = dtWarrantyItem.Rows(i)

                oExecute.addParameter("wati_price", dr.WATI_PRICE, OracleType.Number)
                oExecute.addParameter("wati_luad", dr.WATI_LUAD, OracleType.VarChar)
                oExecute.addParameter("wati_luadname", dr.WATI_LUADNAME, OracleType.VarChar)
                oExecute.addParameter("wati_lustmp", dr.WATI_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("WATI_MARK", dr.WATI_MARK, OracleType.Int16)

                oExecute.addWHERE("wati_watyno", dr.WATI_WATYNO, OracleType.VarChar)
                oExecute.addWHERE("wati_seq", dr.wati_seq, OracleType.Number)
                oExecute.Command("WarrantyItem", Execute.eumCommandType.UPDATE)
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

    Public Function QueryWarrantyName(ByVal War_ver As String, ByVal sDate As DateTime, ByVal eDate As DateTime) As String

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = "SELECT NVL(FN_GET_WARRSET_PERIOD(:War_ver,:sDate,:eDate),'') WarName FROM Dual"

            oQuery.addWHERE("War_ver", ":War_ver", War_ver, OracleType.VarChar)
            oQuery.addWHERE("sDate", ":sDate", sDate, OracleType.DateTime)
            oQuery.addWHERE("eDate", ":eDate", eDate, OracleType.DateTime)

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt.Rows(0)("WarName").ToString()

    End Function


    Public Function QrySpecWarranty(ByVal wati_ver As String) As DataTable
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim sSQL As String = " SELECT * FROM Warrspecs WHERE wap_wid= :wap_wid "
            oQuery.addWHERE("wap_wid", ":wap_wid", wati_ver, OracleType.VarChar)

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function QueryWarrantyItem(ByVal wati_watyno As String, ByVal wati_seq As Integer, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRANTYITEMDataTable

        Dim dt As New DataTable
        Dim dtWarrantyItem As New WarrantyDTO.WARRANTYITEMDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = ""
            End If
            'OrderBY = " ORDER BY " & OrderBY

            If wati_watyno.Trim() <> "" Then
                oQuery.addWHERE("wati_watyno", ":wati_watyno", wati_watyno, OracleType.VarChar)
                sCondition = sCondition & " AND a.wati_watyno=:wati_watyno"
            End If

            If wati_seq > 0 Then
                oQuery.addWHERE("wati_seq", ":wati_seq", wati_seq, OracleType.Number)
                sCondition = sCondition & " AND a.wati_seq=:wati_seq"
            End If


            oQuery.addWHERE("a.wati_mark", ":wati_mark", "0", OracleType.Int16)

            'Dim sSQL As String = "SELECT a.*,b.WAR_NAME,b.WAR_VERSION FROM WarrantyItem a,WarrSet b WHERE a.wati_ver=b.WAR_ID AND a.wati_type IN('CW','EW') AND a.wati_mark=:wati_mark " & sCondition
            'sSQL = sSQL + " UNION ALL"
            'sSQL = sSQL + " SELECT a.*,b.SW_NAME,b.SW_VERSION FROM WarrantyItem a,SWSet b WHERE a.wati_ver=b.SW_ID AND a.wati_type IN('SW') AND a.wati_mark=:wati_mark " & sCondition
            'sSQL = sSQL & OrderBY
            Dim sSQL As String = "SELECT a.*,b.WAR_NAME,b.WAR_VERSION,b.WAR_SPEC_DESC FROM WarrantyItem a,WarrSet b WHERE a.wati_ver=b.WAR_ID AND a.wati_mark=:wati_mark " & sCondition
            sSQL = sSQL & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrantyItem)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrantyItem
    End Function


    Public Function QueryWarrantySerial(ByVal wats_watyno As String, ByVal wats_watyseq As Integer, ByVal wats_sn As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WARRANTYSERIALDataTable

        Dim dt As New DataTable
        Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " wats_cstmp"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If wats_watyno.Trim() <> "" Then
                oQuery.addWHERE("wats_watyno", ":wats_watyno", wats_watyno, OracleType.VarChar)
                sCondition = sCondition & " AND wats_watyno=:wats_watyno"
            End If

            If wats_watyseq > 0 Then
                oQuery.addWHERE("wats_watyseq", ":wats_watyseq", wats_watyseq, OracleType.Number)
                sCondition = sCondition & " AND wats_watyseq=:wats_watyseq"
            End If

            If wats_sn.Trim() <> "" Then
                oQuery.addWHERE("wats_sn", ":wats_sn", wats_sn, OracleType.VarChar)
                sCondition = sCondition & " AND wats_sn=:wats_sn"
            End If


            'oQuery.addWHERE("wats_mark", ":wats_mark", "0", OracleType.Int16)

            Dim sSQL As String = "SELECT * FROM WarrantySerial WHERE 1=1 " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrantySerial)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrantySerial
    End Function

    Public Function QueryWarrantySerial(ByVal wats_sn As String, ByVal wats_type As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If wats_sn.Trim() <> "" Then
                oQuery.addWHERE("wats_sn", ":wats_sn", wats_sn, OracleType.VarChar)
                'sCondition = sCondition & " AND wats_watyno=:wats_watyno"
            End If

            If wats_type <> "" Then
                oQuery.addWHERE("wats_type", ":wats_type", wats_type, OracleType.VarChar)
                'sCondition = sCondition & " AND wats_watyseq=:wats_watyseq"
            End If


            Dim sSQL As String = "SELECT FN_OGB_SN_T(:wats_sn,:wats_type) wats_sn FROM dual "

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
    ''' SaveAddWarrantySerial
    ''' </summary>
    ''' <param name="dtWarrantySerial"></param>
    ''' <remarks></remarks>
    Public Sub SaveWarrantySerial(ByVal dtWarrantySerial As WarrantyDTO.WARRANTYSERIALDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWarrantySerial.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(0)
                Dim sSQL As String = ""
                Dim dt As New DataTable
                For i = 0 To dtWarrantySerial.Rows.Count - 1
                    dr = dtWarrantySerial.Rows(i)

                    oConn.Command.CommandType = CommandType.Text
                    oConn.Command.Parameters.Clear()
                    sSQL = "SELECT wats_sn FROM WarrantySerial WHERE wats_watyno='" & dr.WATS_WATYNO & "' AND wats_watyseq=" & dr.wats_watyseq & " AND wats_sn='" & dr.WATS_SN & "'"
                    dt = oQuery.ExecuteDT(sSQL)

                    'Insert Or Update
                    If dt.Rows.Count > 0 Then
                        'oExecute.addParameter("wats_mark", dr.WATS_MARK, OracleType.VarChar)
                        'oExecute.addParameter("wats_luad", dr.WATS_LUAD, OracleType.VarChar)
                        'oExecute.addParameter("wats_luadname", dr.WATS_LUADNAME, OracleType.VarChar)
                        'oExecute.addParameter("wats_lustmp", dr.WATS_LUSTMP, OracleType.DateTime)

                        'oExecute.addWHERE("wats_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                        'oExecute.addWHERE("wats_watyseq", dr.wats_watyseq, OracleType.VarChar)
                        'oExecute.addWHERE("wats_sn", dr.WATS_SN, OracleType.VarChar)
                        'oExecute.Command("WarrantySerial", Execute.eumCommandType.UPDATE)

                    Else
                        'oExecute.addParameter("wats_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                        'oExecute.addParameter("wats_watyseq", dr.wats_watyseq, OracleType.VarChar)
                        'oExecute.addParameter("wats_sn", dr.WATS_SN, OracleType.VarChar)

                        'oExecute.addParameter("wats_ad", dr.WATS_AD, OracleType.VarChar)
                        'oExecute.addParameter("wats_adname", dr.WATS_ADNAME, OracleType.VarChar)
                        'oExecute.addParameter("wats_cstmp", dr.WATS_CSTMP, OracleType.DateTime)
                        'oExecute.addParameter("wats_luad", dr.WATS_LUAD, OracleType.VarChar)
                        'oExecute.addParameter("wats_luadname", dr.WATS_LUADNAME, OracleType.VarChar)
                        'oExecute.addParameter("wats_lustmp", dr.WATS_LUSTMP, OracleType.DateTime)
                        'oExecute.addParameter("wats_mark", dr.WATS_MARK, OracleType.Int16)

                        'oExecute.Command("WarrantySerial", Execute.eumCommandType.AddNew)
                    End If

                    'Caculate Warranty Date
                    Dim cmd As OracleCommand = oConn.Command
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Clear()
                    cmd.CommandText = "tx_chk_sn"
                    Dim pp0 As OracleParameter = New OracleParameter("vWatyNo", OracleType.VarChar)
                    pp0.Value = dr.WATS_WATYNO
                    pp0.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(pp0)

                    Dim pp1 As OracleParameter = New OracleParameter("vWatiSeq", OracleType.Number)
                    pp1.Value = dr.wats_watyseq
                    pp1.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(pp1)

                    Dim pp3 As OracleParameter = New OracleParameter("vSN", OracleType.VarChar)
                    pp3.Value = dr.WATS_SN
                    pp3.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(pp3)

                    Dim pp4 As OracleParameter = New OracleParameter("vUserNo", OracleType.VarChar)
                    pp4.Value = dr.WATS_LUAD
                    pp4.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(pp4)

                    Dim pp5 As OracleParameter = New OracleParameter("vUserName", OracleType.VarChar)
                    pp5.Value = dr.WATS_LUADNAME
                    pp5.Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Add(pp5)

                    Dim pp2 As OracleParameter = New OracleParameter("vResult", OracleType.VarChar, 3000)
                    pp2.Direction = System.Data.ParameterDirection.Output
                    cmd.Parameters.Add(pp2)
                    Dim nResult As Integer = cmd.ExecuteNonQuery()

                    Dim sResult As String = pp2.Value.ToString()
                    If sResult <> "" Then
                        Throw New Exception(sResult)
                    End If
                Next

                'Update WarrantyItem Qty
                oConn.Command.CommandType = CommandType.Text
                oConn.Command.Parameters.Clear()
                sSQL = "SELECT b.wati_order,nvl(count(a.wats_sn),0) CountQty FROM WarrantySerial a,WarrantyItem b WHERE a.wats_watyno=b.wati_watyno and a.wats_watyseq=b.wati_seq and a.wats_mark=0 and a.wats_watyno='" & dr.WATS_WATYNO & "' AND a.wats_watyseq=" & dr.wats_watyseq & " group by b.wati_order"
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("wati_order").ToString().Trim() = "" Then
                        Dim nQty As Integer = Convert.ToInt32(dt.Rows(0)("CountQty").ToString())

                        oExecute.addParameter("wati_qty", nQty, OracleType.Number)
                        oExecute.addWHERE("wati_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                        oExecute.addWHERE("wati_seq", dr.wats_watyseq, OracleType.VarChar)
                        oExecute.Command("WarrantyItem", Execute.eumCommandType.UPDATE)
                    End If
                End If

                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' EditWarrantySerial
    ''' </summary>
    ''' <param name="dtWarrantySerial"></param>
    ''' <remarks></remarks>
    Public Sub EditWarrantySerial(ByVal dtWarrantySerial As WarrantyDTO.WARRANTYSERIALDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWarrantySerial.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(0)

                'Update WarrantySerial
                For i = 0 To dtWarrantySerial.Rows.Count - 1
                    dr = dtWarrantySerial.Rows(i)
                    oExecute.addParameter("wats_mark", dr.WATS_MARK, OracleType.Number)
                    oExecute.addParameter("wats_luad", dr.WATS_LUAD, OracleType.VarChar)
                    oExecute.addParameter("wats_luadname", dr.WATS_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("wats_lustmp", dr.WATS_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("wats_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                    oExecute.addWHERE("wats_watyseq", dr.wats_watyseq, OracleType.Number)
                    oExecute.addWHERE("wats_sn", dr.WATS_SN, OracleType.VarChar)
                    oExecute.Command("WarrantySerial", Execute.eumCommandType.UPDATE)
                Next

                'Update WarrantyItem Qty
                Dim sSQL As String = "SELECT b.wati_order,nvl(count(a.wats_sn),0) CountQty FROM WarrantySerial a,WarrantyItem b WHERE a.wats_watyno=b.wati_watyno and a.wats_watyseq=b.wati_seq and a.wats_mark=0 and a.wats_watyno='" & dr.WATS_WATYNO & "' AND a.wats_watyseq=" & dr.wats_watyseq & " group by b.wati_order"
                Dim dt As DataTable = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("wati_order").ToString().Trim() = "" Then
                        Dim nQty As Integer = Convert.ToInt32(dt.Rows(0)("CountQty").ToString())

                        oExecute.addParameter("wati_qty", nQty, OracleType.Number)
                        oExecute.addWHERE("wati_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                        oExecute.addWHERE("wati_seq", dr.wats_watyseq, OracleType.VarChar)
                        oExecute.Command("WarrantyItem", Execute.eumCommandType.UPDATE)
                    End If
                End If
                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    Public Sub DeleteWarrantySerial(ByVal dtWarrantySerial As WarrantyDTO.WARRANTYSERIALDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            If dtWarrantySerial.Rows.Count > 0 Then
                Dim dr As WarrantyDTO.WARRANTYSERIALRow = dtWarrantySerial.Rows(0)

                'Update WarrantySerial
                For i = 0 To dtWarrantySerial.Rows.Count - 1
                    dr = dtWarrantySerial.Rows(i)
                    'oExecute.addParameter("wats_mark", dr.WATS_MARK, OracleType.Number)
                    'oExecute.addParameter("wats_luad", dr.WATS_LUAD, OracleType.VarChar)
                    'oExecute.addParameter("wats_luadname", dr.WATS_LUADNAME, OracleType.VarChar)
                    'oExecute.addParameter("wats_lustmp", dr.WATS_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("wats_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                    oExecute.addWHERE("wats_watyseq", dr.wats_watyseq, OracleType.Number)
                    oExecute.addWHERE("wats_sn", dr.WATS_SN, OracleType.VarChar)
                    oExecute.Command("WarrantySerial", Execute.eumCommandType.Delete)
                Next

                'Update WarrantyItem Qty
                Dim sSQL As String = "SELECT b.wati_order,nvl(count(a.wats_sn),0) CountQty FROM WarrantySerial a,WarrantyItem b WHERE a.wats_watyno=b.wati_watyno and a.wats_watyseq=b.wati_seq and a.wats_mark=0 and a.wats_watyno='" & dr.WATS_WATYNO & "' AND a.wats_watyseq=" & dr.wats_watyseq & " group by b.wati_order"
                Dim dt As DataTable = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    'If dt.Rows(0)("wati_order").ToString().Trim() = "" Then
                    Dim nQty As Integer = Convert.ToInt32(dt.Rows(0)("CountQty").ToString())

                    oExecute.addParameter("wati_qty", nQty, OracleType.Number)
                    oExecute.addWHERE("wati_watyno", dr.WATS_WATYNO, OracleType.VarChar)
                    oExecute.addWHERE("wati_seq", dr.wats_watyseq, OracleType.VarChar)
                    oExecute.Command("WarrantyItem", Execute.eumCommandType.UPDATE)
                    'End If
                End If
                oConn.Commit()

            End If
        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    Public Function QueryCustomer(ByVal Cu_No As String, ByVal Cu_Name As String, ByVal Cu_Status As String,
            ByVal Cu_CompNo As String, ByVal Cu_Sales As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.OCC_FILEDataTable

        Dim dtCustomer As New WarrantyDTO.OCC_FILEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " a.occ01 asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If Cu_No.ToString().Trim() <> "" Then
                Cu_No = "%" & Cu_No & "%"
                oQuery.addWHERE("CU_NO", ":CU_NO", Cu_No.Trim(), OracleType.VarChar)
                sCondition = sCondition & " AND a.occ01 like:CU_NO"
            End If

            If Cu_Name.ToString().Trim() <> "" Then
                Cu_Name = "%" & Cu_Name & "%"
                oQuery.addWHERE("CU_NAME", ":CU_NAME", Cu_Name.Trim().ToLower(), OracleType.NVarChar)
                sCondition = sCondition & " AND (lower(a.occ02) like :CU_NAME or lower(a.occ01) like :CU_NAME)"
            End If

            If Cu_Sales.ToString().Trim() <> "" Then
                Cu_Sales = "%" & Cu_Sales & "%"
                oQuery.addWHERE("Cu_Sales", ":Cu_Sales", Cu_Sales, OracleType.VarChar)
                sCondition = sCondition & " AND a.occ04 like:Cu_Sales"
            End If

            If Cu_CompNo = "CL_CHINA" Then 'MODI BY Angel ON 20151214 增加CH_CHINA
                sSQL = "select a.occ01 CU_NO,a.occ02 CU_NAME,occ261 CU_TEL,a.occ04||' / '||b.gen02 CU_SALESID,a.occ42,a.occ43,a.occ04,b.gen02 from ciphersh.occ_file a,ciphersh.gen_file b WHERE a.occ04=b.gen01" & sCondition & OrderBY
            Else
                sSQL = "select a.occ01 CU_NO,a.occ02 CU_NAME,occ261 CU_TEL,a.occ04||' / '||b.gen02 CU_SALESID,a.occ42,a.occ43,a.occ04,b.gen02 from cipherlab.occ_file a,cipherlab.gen_file b WHERE a.occ04=b.gen01" & sCondition & OrderBY

            End If
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
    ''' SWFile--Add
    ''' </summary>
    ''' <param name="dtSWFile">傳入SWFILEDataTable</param>
    ''' <remarks></remarks>
    Public Function SaveAddSWFile(ByVal dtSWFile As WarrantyDTO.SWFILEDataTable) As String
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sSwID As String = ""

        oConn.Open()

        Try
            oConn.BeginTransaction()
            Dim dr As WarrantyDTO.SWFILERow = dtSWFile.Rows(i)

            oExecute.addParameter("SWF_ID", dr.SWF_ID, OracleType.VarChar)
            oExecute.addParameter("SWF_FILE", dr.SWF_FILE, OracleType.VarChar)

            oExecute.addParameter("swf_ad", dr.SWF_AD, OracleType.VarChar)
            oExecute.addParameter("swf_adname", dr.SWF_ADNAME, OracleType.VarChar)
            oExecute.addParameter("swf_cstmp", dr.SWF_CSTMP, OracleType.DateTime)
            oExecute.addParameter("swf_luad", dr.SWF_LUAD, OracleType.VarChar)
            oExecute.addParameter("swf_luadname", dr.SWF_LUADNAME, OracleType.VarChar)
            oExecute.addParameter("swf_lustmp", dr.SWF_LUSTMP, OracleType.DateTime)
            oExecute.addParameter("swf_mark", dr.SWF_MARK, OracleType.Int16)

            oExecute.Command("SWFile", Execute.eumCommandType.AddNew)

            oConn.Commit()

        Catch ex As Exception
            oConn.Rollback()
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return sSwID
    End Function

    Public Function QueryPIPrint(ByVal WatyNo As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim cmd As OracleCommand = oConn.Command
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            cmd.CommandText = "sp_qry_pi"
            Dim pp0 As OracleParameter = New OracleParameter("vWatyNo", OracleType.VarChar)
            pp0.Value = WatyNo
            pp0.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(pp0)

            Dim pr As OracleParameter = New OracleParameter("result", OracleType.Cursor)
            pr.Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(pr)

            Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function GetRMCGift(ByVal WatyNo As String) As DataTable
        Dim dt As DataTable
        Dim oConn As New Connection
        Dim sSQL As Text.StringBuilder = New Text.StringBuilder
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = String.Empty

        oConn.Open()
        Try
            oQuery.addWHERE("WATI_WATYNO", ":WATI_WATYNO", WatyNo.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND a.WATI_WATYNO =:WATI_WATYNO"

            sSQL.AppendLine("SELECT b.OGB01,b.OGB03,b.orderno,b.quantity,b.ENCRYPT_KEY,c.AKEY_IMA01 ")
            sSQL.AppendLine("FROM WARRANTYITEM a ")
            sSQL.AppendLine("JOIN CIPHERLAB.AKEY_LOGIN_GIFT b ON b.OGB01 = a.WATI_WATYNO And b.OGB03 = a.WATI_SEQ ")
            sSQL.AppendLine("JOIN CIPHERLAB.AKEY_IMA_FILE c ON c.AKEY_TYPE = b.SOFTWARETYPE AND c.AKEY_TYPE_YEAR = a.WATI_YEAR AND c.AKEY_TYPE_FREE='Y'")
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

    Public Function QueryServicesPrint(ByVal WatyNo As String) As DataTable

        Dim dt As DataTable
        Dim oConn As New Connection
        Dim sSQL As Text.StringBuilder = New Text.StringBuilder
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = String.Empty

        oConn.Open()
        Try
            oQuery.addWHERE("WarrantyCard_NO", ":WarrantyCard_NO", WatyNo.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND a.WarrantyCard_NO=:WarrantyCard_NO"

            sSQL.AppendLine("SELECT a.*,b.OCC18,sysdate ")
            sSQL.AppendLine("From WarrantyCard1 a ")
            sSQL.AppendLine("JOIN cipherlab.occ_file b ON b.occ01 = a.WarrantyCard_CUST ")
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

    Public Function QueryServicesSN(ByVal WarrantyCard_NO As String, ByVal WarrantyCard_SKUNO As String) As DataTable

        Dim dt As DataTable
        Dim oConn As New Connection
        Dim sSQL As Text.StringBuilder = New Text.StringBuilder
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = String.Empty

        oConn.Open()
        Try
            oQuery.addWHERE("WarrantyCard_NO", ":WarrantyCard_NO", WarrantyCard_NO.Trim(), OracleType.VarChar)
            oQuery.addWHERE("WarrantyCard_SKUNO", ":WarrantyCard_SKUNO", WarrantyCard_SKUNO.Trim(), OracleType.VarChar)

            sSQL.AppendLine("SELECT * FROM WarrantyCard2 ")
            sSQL.AppendLine("WHERE WarrantyCard_NO = :WarrantyCard_NO AND WarrantyCard_SKUNO =:WarrantyCard_SKUNO ")
            sSQL.AppendLine("ORDER BY WarrantyCard_SEQ ")
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


    Public Function QuerySlipNoDecrypt(ByVal SlipNo As String) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            Dim cmd As OracleCommand = oConn.Command
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            cmd.CommandText = "SP_QRY_SlipNoDecrypt"
            Dim pp0 As OracleParameter = New OracleParameter("vSlipNo", OracleType.VarChar)
            pp0.Value = SlipNo
            pp0.Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(pp0)

            Dim pr As OracleParameter = New OracleParameter("result", OracleType.Cursor)
            pr.Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(pr)

            Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function getGen02(ByVal gen01 As String) As String
        Dim retval As String = ""
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()
        Try

            oQuery.addWHERE("gen01", ": gen01", gen01.Trim(), OracleType.VarChar)
            sCondition = sCondition & " AND gen01=:gen01"

            sSQL = "select gen03 from cipherlab.gen_file where 1=1"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("gen03").ToString()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return retval
    End Function

    Public Function QueryWarrPartsType(ByVal LANGUAGE_NO As String, ByVal TYPE_NAME As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrPartsTypeDataTable

        Dim dt As New DataTable
        Dim dtWarrPartsType As New WarrantyDTO.WarrPartsTypeDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " TYPE_SEQ ASC"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If TYPE_NAME.ToString().Trim() <> "" Then
                TYPE_NAME = "%" & TYPE_NAME & "%"
                oQuery.addWHERE("TYPE_NAME", ":TYPE_NAME", TYPE_NAME, OracleType.VarChar)
                sCondition = sCondition & " AND TYPE_NAME LIKE:TYPE_NAME"
            End If


            Dim sTableName As String = "WarrPartsType"
            If _isDebug = True Then
                sTableName = "WarrPartsType"
            End If
            Dim sSQL As String = "select TYPE_NO,TYPE_NAME,TYPE_SEQ from " & sTableName & " where 1=1" & sCondition & OrderBY

            If LANGUAGE_NO = "002" Then
                sSQL = "select TYPE_NO,TYPE_NAME_CH TYPE_NAME,TYPE_SEQ from " & sTableName & " where 1=1" & sCondition & OrderBY
            End If


            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrPartsType)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrPartsType
    End Function

    Public Function QueryWarrSpecsType(ByVal LANGUAGE_NO As String, ByVal SPEC_NAME As String, Optional ByVal OrderBY As String = "") As DataTable

        Dim dt As New DataTable
        'Dim dtWarrPartsType As New WarrantyDTO.WarrPartsTypeDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " SPEC_SEQ ASC"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If SPEC_NAME.ToString().Trim() <> "" Then
                SPEC_NAME = "%" & SPEC_NAME & "%"
                oQuery.addWHERE("SPEC_NAME", ":SPEC_NAME", SPEC_NAME, OracleType.VarChar)
                sCondition = sCondition & " AND SPEC_NAME LIKE :SPEC_NAME"
            End If


            Dim sTableName As String = "WarrSPECSType"
            If _isDebug = True Then
                sTableName = "WarrSPECSType"
            End If
            Dim sSQL As String = "SELECT SPEC_NO,SPEC_NAME,SPEC_SEQ from " & sTableName & " where 1=1" & sCondition & OrderBY

            If LANGUAGE_NO = "002" Then
                sSQL = "select SPEC_NO,SPEC_NAME_CH SPEC_NAME,SPEC_SEQ from " & sTableName & " where 1=1" & sCondition & OrderBY
            End If


            dt = oQuery.ExecuteDT(sSQL)
            'Common.TransferDataTable(dt, dtWarrPartsType)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    '優化效能 by buck modify 20260323 begin
    Public Function QueryWarrParts(ByVal LANGUAGE_NO As String, ByVal WAP_WID As String, ByVal SERIAL_NO As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrPartsDataTable

        Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""

        If SERIAL_NO.Trim() <> "" Then
            sWarrGroup = getSerialNumberWarrGroup(SERIAL_NO)
        End If

        oConn.Open()
        Try
            If String.IsNullOrEmpty(OrderBY.Trim()) Then
                OrderBY = " A.WAP_SEQ ASC"
            End If

            If Not String.IsNullOrEmpty(WAP_WID.Trim()) Then
                oQuery.addWHERE("WAP_WID", ":WAP_WID", WAP_WID, OracleType.VarChar)
                sCondition &= " AND A.WAP_WID = :WAP_WID"
            End If

            Dim sSQL As String = "
                                    SELECT A.WAP_WID, A.WAP_SEQ, A.WAP_NAME, {0},
                                    A.WAP_MON, A.WAP_EMON, A.WAP_DESC, A.WAP_AD, A.WAP_ADNAME, 
                                    A.WAP_CSTMP, A.WAP_LUAD, A.WAP_LUADNAME, A.WAP_LUSTMP
                                    FROM WarrParts A
                                    INNER JOIN WarrPartsType B ON A.WAP_NAME = B.TYPE_NO
                                "
            sSQL = String.Format(sSQL, If(LANGUAGE_NO = "002", "B.TYPE_NAME_CH AS TYPE_NAME", "B.TYPE_NAME"))

            If Not String.IsNullOrEmpty(sWarrGroup.Trim()) Then
                oQuery.addWHERE("GRPT_GNO", ":GRPT_GNO", sWarrGroup, OracleType.VarChar)
                sSQL &= "INNER JOIN (SELECT DISTINCT GRPT_TNO FROM WarrGroupParts WHERE GRPT_GNO = :GRPT_GNO) G ON B.TYPE_NO = G.GRPT_TNO "
            End If

            sSQL &= " WHERE 1=1 " & sCondition
            sSQL &= " ORDER BY " & OrderBY

            Dim dt As DataTable = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtWarrParts
    End Function

    'Public Function QueryWarrParts(ByVal LANGUAGE_NO As String, ByVal WAP_WID As String, ByVal SERIAL_NO As String, Optional ByVal OrderBY As String = "") As WarrantyDTO.WarrPartsDataTable

    '    Dim dt As New DataTable
    '    Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
    '    Dim oConn As New Connection
    '    Dim oQuery As New ICAT_OracleDAO.Query(oConn)
    '    Dim sCondition As String = ""

    '    Dim sWarrGroup As String = ""
    '    If SERIAL_NO.Trim() <> "" Then
    '        sWarrGroup = getSerialNumberWarrGroup(SERIAL_NO)
    '    End If

    '    oConn.Open()
    '    Try
    '        If OrderBY.Trim = "" Then
    '            OrderBY = " WAP_SEQ ASC"
    '        End If
    '        OrderBY = " ORDER BY " & OrderBY

    '        If WAP_WID.ToString().Trim() <> "" Then
    '            'WAP_WID = "%" & WAP_WID & "%"
    '            oQuery.addWHERE("WAP_WID", ":WAP_WID", WAP_WID, OracleType.VarChar)
    '            sCondition = sCondition & " AND WAP_WID =:WAP_WID"
    '        End If

    '        If sWarrGroup.Trim() <> "" Then
    '            sCondition = sCondition & " and exists(select '*' from WarrGroupParts where TYPE_NO=GRPT_TNO and GRPT_GNO='" + sWarrGroup + "')"
    '        End If


    '        Dim sTableName As String = "WarrParts"
    '        If _isDebug = True Then
    '            sTableName = "WarrParts"
    '        End If
    '        Dim sSQL As String = "select WAP_WID,WAP_SEQ,WAP_NAME,TYPE_NAME,WAP_MON,WAP_EMON,WAP_DESC,WAP_AD,WAP_ADNAME,WAP_CSTMP,WAP_LUAD,WAP_LUADNAME"
    '        sSQL = sSQL + ",WAP_LUSTMP from WarrParts,WarrPartsType where WAP_NAME=TYPE_NO" & sCondition & OrderBY

    '        If LANGUAGE_NO = "002" Then
    '            sSQL = "select WAP_WID,WAP_SEQ,WAP_NAME,TYPE_NAME_CH TYPE_NAME,WAP_MON,WAP_EMON,WAP_DESC,WAP_AD,WAP_ADNAME,WAP_CSTMP,WAP_LUAD,WAP_LUADNAME"
    '            sSQL = sSQL + ",WAP_LUSTMP from WarrParts,WarrPartsType where WAP_NAME=TYPE_NO" & sCondition & OrderBY
    '        End If
    '        'Throw New Exception(sSQL)

    '        dt = oQuery.ExecuteDT(sSQL)
    '        'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
    '        Common.TransferDataTable(dt, dtWarrParts)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        oConn.Close()
    '        oConn.Dispose()
    '    End Try

    '    Return dtWarrParts
    'End Function
    '優化效能 by buck modify 20260323 end

    Public Function QueryWarrSpecs(ByVal LANGUAGE_NO As String, ByVal WAP_WID As String, ByVal SERIAL_NO As String, Optional ByVal OrderBY As String = "") As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        Dim sWarrGroup As String = ""
        If SERIAL_NO.Trim() <> "" Then
            sWarrGroup = getSerialNumberWarrGroup(SERIAL_NO)
        End If

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " WAP_SEQ ASC"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WAP_WID.ToString().Trim() <> "" Then
                'WAP_WID = "%" & WAP_WID & "%"
                oQuery.addWHERE("WAP_WID", ":WAP_WID", WAP_WID, OracleType.VarChar)
                sCondition = sCondition & " AND WAP_WID =:WAP_WID"
            End If

            If sWarrGroup.Trim() <> "" Then
                sCondition = sCondition & " and exists(select '*' from WarrGroupParts where TYPE_NO=GRPT_TNO and GRPT_GNO='" + sWarrGroup + "')"
            End If


            Dim sTableName As String = "WarrSpecs"
            If _isDebug = True Then
                sTableName = "WarrSpecs"
            End If
            Dim sSQL As String = "select WAP_WID,WAP_SEQ,WAP_NAME,SPEC_NAME,WAP_RULE,WAP_AD,WAP_ADNAME,WAP_CSTMP "
            sSQL = sSQL + "FROM WarrSpecs,WarrSpecsType where WAP_NAME=SPEC_NO" & sCondition & OrderBY

            If LANGUAGE_NO = "002" Then
                sSQL = "select WAP_WID,WAP_SEQ,WAP_NAME,SPEC_NAME_CH TYPE_NAME,WAP_RULE,WAP_AD,WAP_ADNAME,WAP_CSTMP "
                sSQL = sSQL + "FROM WarrSpecs,WarrSpecsType where WAP_NAME=SPEC_NO" & sCondition & OrderBY
            End If
            'Throw New Exception(sSQL)

            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

#Region "RMA_TotalLose_Record"

    Public Function Select_WARRANTYITEM(ByVal WATS_SN As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select * from  RMA.WARRANTYITEM
            where
            WATI_WATYNO    in (select WATS_WATYNO from  WARRANTYSERIAL where  WATS_SN =:WATS_SN)    and
            WATI_SEQ    in ( select WATS_WATYSEQ from  WARRANTYSERIAL where  WATS_SN =:WATS_SN)  and WATI_MARK = 0      
            "
            oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function Select_WARRANTYSERIAL(ByVal WATS_SN As String) As Boolean

        Dim check As Boolean = False

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            'Dim sSQL As String = "  select (select  WATY_ERPNO from  WARRANTYORD  where WATY_NO in (select WATS_WATYNO from ( select WATS_WATYNO from  WARRANTYSERIAL where  WATS_SN =:WATS_SN  order  by WATS_CSTMP desc ) where rownum = 1) ) WATS_SN from  WARRANTYSERIAL where  WATS_SN  =:WATS_SN and WATS_MARK = 0  "
            Dim sSQL = "
                            SELECT 
                                MAX(O.WATY_ERPNO) KEEP (DENSE_RANK LAST ORDER BY S.WATS_CSTMP ASC) AS WATS_SN
                            FROM WARRANTYSERIAL S
                            JOIN WARRANTYORD O ON S.WATS_WATYNO = O.WATY_NO
                            WHERE S.WATS_SN = :WATS_SN 
                              AND S.WATS_MARK = 0
                        "
            oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

            check = (dt IsNot Nothing) AndAlso
                        dt.AsEnumerable().Any(Function(x) Not String.IsNullOrEmpty(x("WATS_SN").ToString().Trim()))

            'If Not dt Is Nothing Then
            '    If dt.Rows.Count > 0 Then
            '        check = True
            '    End If
            'Else

            'End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return check
    End Function

    Public Function EXPORT_EXPORT_ORDERNUMBER(ByVal EXPORT_SERIALNO As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            'Dim sSQL As String = " select EXPORT_ORDERNUMBER ,  ( Select  WAP_RULE from WARRSPECS where WAP_NAME = 'Losstop%' and WAP_WID = EXPORT_WAR_ID ) Losstop, ( Select  WAP_RULE from WARRSPECS where WAP_NAME = 'MOQ' and WAP_WID = EXPORT_WAR_ID ) MOQ from EXPORT where  EXPORT_SERIALNO =:EXPORT_SERIALNO "
            Dim sSQL = "
                            SELECT 
                                E.EXPORT_ORDERNUMBER,
                                MAX(CASE WHEN W.WAP_NAME LIKE 'Losstop%' THEN W.WAP_RULE END) AS Losstop,
                                MAX(CASE WHEN W.WAP_NAME = 'MOQ' THEN W.WAP_RULE END) AS MOQ
                            FROM EXPORT E
                            LEFT JOIN WARRSPECS W ON E.EXPORT_WAR_ID = W.WAP_WID
                            WHERE E.EXPORT_SERIALNO in (
                                SELECT EXPAR_M_SN FROM EXPORT_PARTS WHERE EXPAR_M_SN = :EXPORT_SERIALNO
                                UNION
                                SELECT EXPAR_M_SN FROM EXPORT_PARTS WHERE EXPAR_D_SN = :EXPORT_SERIALNO
                            )
                            GROUP BY E.EXPORT_ORDERNUMBER, E.EXPORT_WAR_ID
                        "
            oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function Insert_axmt410_axmt400(ByVal ID As String, ByVal Project_No As String, ByVal Project_Qty As String, ByVal Order_No As String, ByVal Order_Qty As String, ByVal Total_Loss_Insurance As String, ByVal RMAD_RMANO As String, ByVal RMAD_SEQ As String, ByVal RMAD_SERIALNO As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command


        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " INSERT INTO RMA_TotalLose_Record(ID,Project_No,Project_Qty,Order_No,Order_Qty,Total_Loss_Insurance,RMAD_RMANO,RMAD_SEQ,RMAD_SERIALNO) VALUES (:ID,:Project_No,:Project_Qty,:Order_No,:Order_Qty,:Total_Loss_Insurance,:RMAD_RMANO,:RMAD_SEQ,:RMAD_SERIALNO) "

            oCommand.Parameters.AddWithValue(":ID", ID)
            oCommand.Parameters.AddWithValue(":Project_No", Project_No)
            oCommand.Parameters.AddWithValue(":Project_Qty", Project_Qty)
            oCommand.Parameters.AddWithValue(":Order_No", Order_No)
            oCommand.Parameters.AddWithValue(":Order_Qty", Order_Qty)
            oCommand.Parameters.AddWithValue(":Total_Loss_Insurance", Total_Loss_Insurance)
            oCommand.Parameters.AddWithValue(":RMAD_RMANO", RMAD_RMANO)
            oCommand.Parameters.AddWithValue(":RMAD_SEQ", RMAD_SEQ)
            oCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)


            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    'tiptop保固資料
    Public Function SELECT_axmt410_axmt400(ByVal Project_No As String, ByVal Project_Qty As String, ByVal Order_No As String, ByVal Order_Qty As String, ByVal TOTAL_LOSS_INSURANCE As String)


        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select * from RMA_TotalLose_Record
            where
            PROJECT_NO      =:PROJECT_NO          
            and PROJECT_QTY =:PROJECT_QTY          
            and ORDER_NO    =:ORDER_NO                 
            and ORDER_QTY   =:ORDER_QTY            
            and TOTAL_LOSS_INSURANCE  =:TOTAL_LOSS_INSURANCE     "

            oQuery.addWHERE("Project_No", ":Project_No", Project_No, OracleType.VarChar)
            oQuery.addWHERE("Project_Qty", ":Project_Qty", Project_Qty, OracleType.VarChar)
            oQuery.addWHERE("Order_No", ":Order_No", Order_No, OracleType.VarChar)
            oQuery.addWHERE("Order_Qty", ":Order_Qty", Order_Qty, OracleType.VarChar)
            oQuery.addWHERE("TOTAL_LOSS_INSURANCE", ":TOTAL_LOSS_INSURANCE", TOTAL_LOSS_INSURANCE, OracleType.VarChar)


            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    'RMA保固資料
    Public Function SELECT_axmt410_axmt400_RMA(ByVal Order_No As String, ByVal Order_Qty As String, ByVal TOTAL_LOSS_INSURANCE As String)


        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select * from RMA_TotalLose_Record
            where
            ORDER_NO    =:ORDER_NO                 
            and ORDER_QTY   =:ORDER_QTY            
            and TOTAL_LOSS_INSURANCE  =:TOTAL_LOSS_INSURANCE     "

            oQuery.addWHERE("Order_No", ":Order_No", Order_No, OracleType.VarChar)
            oQuery.addWHERE("Order_Qty", ":Order_Qty", Order_Qty, OracleType.VarChar)
            oQuery.addWHERE("TOTAL_LOSS_INSURANCE", ":TOTAL_LOSS_INSURANCE", TOTAL_LOSS_INSURANCE, OracleType.VarChar)


            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function select_Project_No_RMAD_SERIALNO(ByVal RMAD_RMANO As String, ByVal RMAD_SERIALNO As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select * from RMA_TotalLose_Record
            where
            RMAD_RMANO      =:RMAD_RMANO          
            and RMAD_SERIALNO =:RMAD_SERIALNO          
           "

            oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO, OracleType.VarChar)
            oQuery.addWHERE("RMAD_SERIALNO", ":RMAD_SERIALNO", RMAD_SERIALNO, OracleType.VarChar)


            'Throw New Exception(sSQL)

            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function DELETE_axmt410_axmt400(ByVal RMAD_RMANO As String, ByVal RMAD_SERIALNO As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command

        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " DELETE From RMA_TotalLose_Record Where RMAD_RMANO =:RMAD_RMANO and RMAD_SERIALNO =:RMAD_SERIALNO "

            oCommand.Parameters.AddWithValue(":RMAD_RMANO", RMAD_RMANO)
            oCommand.Parameters.AddWithValue(":RMAD_SERIALNO", RMAD_SERIALNO)


            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    Public Function EXPORT_axmt410_axmt400(ByVal OEB01 As String, ByVal OEB03 As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " SELECT c Project_No ,( select oeb12  from CIPHERLAB.OEB_FILE where OEB01 = c and OEB03 = N02  ) Project_Qty ,N01 Order_No ,oeb12 Order_Qty,N01,oeb24,oeb04 FROM    ( select  (select oea12 from CIPHERLAB.OEA_FILE where OEA01 = OEB01) c ,OEB03 N02,oeb12 ,OEB01  N01    ,oeb24 , oeb04 from CIPHERLAB.OEB_FILE where OEB01 =:OEB01 and OEB03 =:OEB03 ) "

            oQuery.addWHERE("OEB01", ":OEB01", OEB01, OracleType.VarChar)
            oQuery.addWHERE("OEB03", ":OEB03", OEB03, OracleType.VarChar)

            'Throw New Exception(sSQL)

            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    '判斷可用數量
    Public Function Check_axmt410_axmt400(ByVal Order_No As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select count(ORDER_NO) as ORDER_NO from RMA_TotalLose_Record where  ORDER_NO =:ORDER_NO "

            oQuery.addWHERE("ORDER_NO", ":ORDER_NO", Order_No, OracleType.VarChar)

            'Throw New Exception(sSQL)

            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function Select_WARRSET_WAR_SPEC_DESC(ByVal EXPORT_SERIALNO As String) As DataTable

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select WAR_SPEC_DESC from WARRSET where WAR_ID in ( select EXPORT_WAR_ID from  EXPORT where EXPORT_SERIALNO =:EXPORT_SERIALNO ) "

            oQuery.addWHERE("EXPORT_SERIALNO", ":EXPORT_SERIALNO", EXPORT_SERIALNO, OracleType.VarChar)

            'Throw New Exception(sSQL)

            dt = oQuery.ExecuteDT(sSQL)
            'Throw New Exception(dt.Rows.Count.ToString() + "-SERIAL_NO " + SERIAL_NO + "-WAP_WID " + WAP_WID + "-" + sSQL)
            'Common.TransferDataTable(dt, dtWarrParts)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    Public Function Select_WARRANTYITEM_WATI_WATYNO(ByVal WATI_WATYNO As String) As DataTable

        Dim check As Boolean = False

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select  
            CASE WHEN WATI_QTY >= ( 
            
            select   CASE WHEN WAP_RULE is Null THEN 0 
            ELSE
            CAST(WAP_RULE AS integer)
         
            END  
            from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER
            
 )   and  (select count(WAP_WID)  from WARRSPECS where WAP_NAME =  'Losstop%' and WAP_WID =  WATI_VER) > 0   THEN 'OK'

            ELSE
            'NO'
            END  QTY
             , ( select WAP_RULE from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER )
             ,WATI_QTY
            from WARRANTYITEM where WATI_MARK = 0 and WATI_WATYNO =:WATI_WATYNO "
            oQuery.addWHERE("WATI_WATYNO", ":WATI_WATYNO", WATI_WATYNO, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function


    Public Function Select_WARRANTYITEM_WATI_WATYNO_ALL(ByVal WATI_WATYNO As String) As DataTable

        Dim check As Boolean = False

        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""


        oConn.Open()
        Try

            Dim sSQL As String = " select  
            CASE WHEN SUM(WATI_QTY) >= ( 
            
            select   CASE WHEN WAP_RULE is Null THEN 0 
            ELSE
            CAST(WAP_RULE AS integer)
         
            END  
            from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER
            
 )    and  (select count(WAP_WID)  from WARRSPECS where WAP_NAME =  'Losstop%' and WAP_WID =  WATI_VER) > 0   THEN 'OK'

            ELSE
            'NO'
            END  QTY
             , ( select WAP_RULE from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER )
             ,SUM(WATI_QTY)
            from WARRANTYITEM where WATI_MARK = 0 and WATI_WATYNO =:WATI_WATYNO group by WATI_VER "
            oQuery.addWHERE("WATI_WATYNO", ":WATI_WATYNO", WATI_WATYNO, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

#Region "2025/01/03 最低購買數量"
    Public Function Select_WARRANTYITEM_WATI_WATYNO_QTY_ALL(ByVal WATI_WATYNO As String) As DataTable

        Dim check As Boolean = False
        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""
        oConn.Open()
        Try

            Dim sSQL As String = " select  
            CASE WHEN SUM(WATI_QTY) >= (   
            select   CASE WHEN WAP_RULE is Null THEN 0 
            ELSE
            CAST(WAP_RULE AS integer)
            END  
            from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER
            )   and  (select count(WAP_WID)  from WARRSPECS where WAP_NAME =  'Losstop%' and WAP_WID =  WATI_VER) = 0  THEN 'OK'
            ELSE
            'NO'
            END  QTY
            , ( select WAP_RULE from WARRSPECS where WAP_NAME =  'MOQ' and WAP_WID =  WATI_VER )
            ,SUM(WATI_QTY)
            from WARRANTYITEM where WATI_MARK = 0 and WATI_WATYNO =:WATI_WATYNO group by WATI_VER "
            oQuery.addWHERE("WATI_WATYNO", ":WATI_WATYNO", WATI_WATYNO, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

#End Region



#Region "2025/01/06 BI 電池保固"

    '電池保序號查詢
    Public Function Query_WATS_SN(ByVal WATS_SN As String) As DataTable

        Dim dt As New DataTable
        Dim RMAD_RMANO As String = ""
        Dim RMAD_SEQ As String = ""
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt_ As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " 
            select		
            B.WATS_WATYNO                ,
            B.WATS_WATYSEQ               ,
            Y.CU_NO                      ,
            Y.CU_NAME                    , 
            E.EXPORT_PARTNO              ,
            TO_CHAR(   L.WATS_WARRNSTART   , 'YYYY-MM-DD')   WATS_WARRNSTART     ,
            TO_CHAR(   L.WATS_WARRNEND   , 'YYYY-MM-DD')     WATS_WARRNEND   ,
            (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) + (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO =  B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ and WAP_RULE = '36') Total,
            (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_36 is not null  ))  +  (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_18 is not null  ))   Consumed_Amount,
            (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) + (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO =  B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ and WAP_RULE = '36')  -  ((select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_36 is not null  ))  +  (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_18 is not null  ))) Remaining_Consumption,
            TO_CHAR(B.WATS_WARRN_ORDER_14 , 'YYYY-MM-DD')      WATS_WARRN_ORDER_14  ,
            TO_CHAR(B.WATS_WARRN_18 , 'YYYY-MM-DD')      WATS_WARRN_18  ,
            TO_CHAR( B.WATS_WARRN_ORDER_32, 'YYYY-MM-DD')    WATS_WARRN_ORDER_32    ,
            TO_CHAR(B.WATS_WARRN_36, 'YYYY-MM-DD')        WATS_WARRN_36,
            B.EXPORT_ORDERNUMBER_18      ,
            B.EXPORT_ORDERNUMBER_SEQ_18  ,
            B.EXPORT_ORDERNUMBER_36      ,
            B.EXPORT_ORDERNUMBER_SEQ_36 
            from WARRANTYSERIAL_BI B 
            LEFT JOIN
            (
            select 
            O.WATY_NO         ,
            C.CU_NO           ,
            C.CU_NAME    
            from WARRANTYORD O 
            left join CUSTOMER C on O.WATY_CUST = C.CU_NO
            ) Y on B.WATS_WATYNO = Y.WATY_NO
            LEFT JOIN WARRANTYSERIAL L on L.WATS_WATYNO = B.WATS_WATYNO and L.WATS_WATYSEQ = B.WATS_WATYSEQ and  L.WATS_SN =  B.WATS_SN
            LEFT JOIN EXPORT E on  E.EXPORT_SERIALNO =  B.WATS_SN
            where B.WATS_SN =:WATS_SN "
                oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN, OracleType.VarChar)
                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        Return dt
    End Function

    'RMA單查消耗
    Public Function Query_WATS_ALL(ByVal RMAD_RMANO As String) As DataTable

        Dim dt As New DataTable
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt_ As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " 
         select RMAD_RMANO,to_char(RMAD_SEQ) RMAD_SEQ,(select count(WATS_SN) from WARRANTYSERIAL_BI where  (  RMAD_RMANO_18 = RMAD_RMANO and  RMAD_SEQ_18 = RMAD_SEQ ) or    (  RMAD_RMANO_36 = RMAD_RMANO and  RMAD_SEQ_36 = RMAD_SEQ )) Remaining_Consumption,WATS_WATYNO,to_char(WATS_WATYSEQ) WATS_WATYSEQ
                from
                (

                select distinct
                (select RMAD_RMANO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_RMANO,
                (select RMAD_SEQ from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_SEQ,
                (select RMAD_SERIALNO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_SERIALNO, 
                Total,WATS_WATYNO,WATS_WATYSEQ from WARRANTYITEM A
                left JOIN
                (
                select		
                RMAD_RMANO_18,
                RMAD_SEQ_18,
                B.WATS_WATYNO                ,
                B.WATS_WATYSEQ               ,
                Y.CU_NO                      ,
                Y.CU_NAME                    , 
                E.EXPORT_PARTNO              ,
                TO_CHAR(   L.WATS_WARRNSTART   , 'YYYY-MM-DD')   WATS_WARRNSTART     ,
                TO_CHAR(   L.WATS_WARRNEND   , 'YYYY-MM-DD')     WATS_WARRNEND   ,
                (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) - (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_18 is null  or WATS_WARRN_18 is null)) Total,

                TO_CHAR(B.WATS_WARRN_ORDER_14 , 'YYYY-MM-DD')      WATS_WARRN_ORDER_14  ,
                TO_CHAR(B.WATS_WARRN_18 , 'YYYY-MM-DD')      WATS_WARRN_18  ,
                TO_CHAR( B.WATS_WARRN_ORDER_32, 'YYYY-MM-DD')    WATS_WARRN_ORDER_32    ,
                TO_CHAR(B.WATS_WARRN_36, 'YYYY-MM-DD')        WATS_WARRN_36,
                B.EXPORT_ORDERNUMBER_18      ,
                B.EXPORT_ORDERNUMBER_SEQ_18  ,
                B.EXPORT_ORDERNUMBER_36      ,
                B.EXPORT_ORDERNUMBER_SEQ_36 
                from WARRANTYSERIAL_BI B 
                LEFT JOIN
                (
                select 
                O.WATY_NO         ,
                C.CU_NO           ,
                C.CU_NAME    
                from WARRANTYORD O 
                left join CUSTOMER C on O.WATY_CUST = C.CU_NO
                ) Y on B.WATS_WATYNO = Y.WATY_NO
                LEFT JOIN WARRANTYSERIAL L on L.WATS_WATYNO = B.WATS_WATYNO and L.WATS_WATYSEQ = B.WATS_WATYSEQ and  L.WATS_SN =  B.WATS_SN
                LEFT JOIN EXPORT E on  E.EXPORT_SERIALNO =  B.WATS_SN where RMAD_RMANO_18 is not Null 
                ) B on A.WATI_WATYNO =  B.WATS_WATYNO and A.WATI_SEQ =  B.WATS_WATYSEQ where WATI_WATYNO in (select distinct WATS_WATYNO from WARRANTYSERIAL_BI)  

                union all

                select distinct
                (select RMAD_RMANO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_RMANO,
                (select RMAD_SEQ from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_SEQ,
                (select RMAD_SERIALNO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_SERIALNO, 
                Total,WATS_WATYNO,WATS_WATYSEQ from WARRANTYITEM A
                left JOIN
                (
                select		
                RMAD_RMANO_36,
                RMAD_SEQ_36,
                B.WATS_WATYNO                ,
                B.WATS_WATYSEQ               ,
                Y.CU_NO                      ,
                Y.CU_NAME                    , 
                E.EXPORT_PARTNO              ,
                TO_CHAR(   L.WATS_WARRNSTART   , 'YYYY-MM-DD')   WATS_WARRNSTART     ,
                TO_CHAR(   L.WATS_WARRNEND   , 'YYYY-MM-DD')     WATS_WARRNEND   ,
                (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) - (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_36 is null  or WATS_WARRN_36 is null))  Total,
                TO_CHAR(B.WATS_WARRN_ORDER_14 , 'YYYY-MM-DD')      WATS_WARRN_ORDER_14  ,
                TO_CHAR(B.WATS_WARRN_18 , 'YYYY-MM-DD')      WATS_WARRN_18  ,
                TO_CHAR( B.WATS_WARRN_ORDER_32, 'YYYY-MM-DD')    WATS_WARRN_ORDER_32    ,
                TO_CHAR(B.WATS_WARRN_36, 'YYYY-MM-DD')        WATS_WARRN_36,
                B.EXPORT_ORDERNUMBER_18      ,
                B.EXPORT_ORDERNUMBER_SEQ_18  ,
                B.EXPORT_ORDERNUMBER_36      ,
                B.EXPORT_ORDERNUMBER_SEQ_36 
                from WARRANTYSERIAL_BI B 
                LEFT JOIN
                (
                select 
                O.WATY_NO         ,
                C.CU_NO           ,
                C.CU_NAME    
                from WARRANTYORD O 
                left join CUSTOMER C on O.WATY_CUST = C.CU_NO
                ) Y on B.WATS_WATYNO = Y.WATY_NO
                LEFT JOIN WARRANTYSERIAL L on L.WATS_WATYNO = B.WATS_WATYNO and L.WATS_WATYSEQ = B.WATS_WATYSEQ and  L.WATS_SN =  B.WATS_SN
                LEFT JOIN EXPORT E on  E.EXPORT_SERIALNO =  B.WATS_SN where RMAD_RMANO_36 is not Null 
                ) B on A.WATI_WATYNO =  B.WATS_WATYNO and A.WATI_SEQ =  B.WATS_WATYSEQ where WATI_WATYNO in (select distinct WATS_WATYNO from WARRANTYSERIAL_BI)  

                )
                where RMAD_RMANO =:RMAD_RMANO
                group by RMAD_RMANO,RMAD_SEQ,WATS_WATYNO,WATS_WATYSEQ
                "
                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO, OracleType.VarChar)
                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        Return dt
    End Function
    '查消耗ALL
    Public Function Query_WATS_ALL() As DataTable

        Dim dt As New DataTable
        Dim RMAD_RMANO As String = ""
        Dim RMAD_SEQ As String = ""
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt_ As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " 
        select RMAD_RMANO,to_char(RMAD_SEQ) RMAD_SEQ,(select count(WATS_SN) from WARRANTYSERIAL_BI where  (  RMAD_RMANO_18 = RMAD_RMANO and  RMAD_SEQ_18 = RMAD_SEQ ) or    (  RMAD_RMANO_36 = RMAD_RMANO and  RMAD_SEQ_36 = RMAD_SEQ )) Remaining_Consumption,WATS_WATYNO,to_char(WATS_WATYSEQ) WATS_WATYSEQ
            from
            (

            select distinct
            (select RMAD_RMANO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_RMANO,
            (select RMAD_SEQ from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_SEQ,
            (select RMAD_SERIALNO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_18 and RMAD_SEQ = RMAD_SEQ_18) ) RMAD_SERIALNO, 
            Total,WATS_WATYNO,WATS_WATYSEQ from WARRANTYITEM A
            left JOIN
            (
            select		
            RMAD_RMANO_18,
            RMAD_SEQ_18,
            B.WATS_WATYNO                ,
            B.WATS_WATYSEQ               ,
            Y.CU_NO                      ,
            Y.CU_NAME                    , 
            E.EXPORT_PARTNO              ,
            TO_CHAR(   L.WATS_WARRNSTART   , 'YYYY-MM-DD')   WATS_WARRNSTART     ,
            TO_CHAR(   L.WATS_WARRNEND   , 'YYYY-MM-DD')     WATS_WARRNEND   ,
            (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) - (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_18 is null  or WATS_WARRN_18 is null)) Total,

            TO_CHAR(B.WATS_WARRN_ORDER_14 , 'YYYY-MM-DD')      WATS_WARRN_ORDER_14  ,
            TO_CHAR(B.WATS_WARRN_18 , 'YYYY-MM-DD')      WATS_WARRN_18  ,
            TO_CHAR( B.WATS_WARRN_ORDER_32, 'YYYY-MM-DD')    WATS_WARRN_ORDER_32    ,
            TO_CHAR(B.WATS_WARRN_36, 'YYYY-MM-DD')        WATS_WARRN_36,
            B.EXPORT_ORDERNUMBER_18      ,
            B.EXPORT_ORDERNUMBER_SEQ_18  ,
            B.EXPORT_ORDERNUMBER_36      ,
            B.EXPORT_ORDERNUMBER_SEQ_36 
            from WARRANTYSERIAL_BI B 
            LEFT JOIN
            (
            select 
            O.WATY_NO         ,
            C.CU_NO           ,
            C.CU_NAME    
            from WARRANTYORD O 
            left join CUSTOMER C on O.WATY_CUST = C.CU_NO
            ) Y on B.WATS_WATYNO = Y.WATY_NO
            LEFT JOIN WARRANTYSERIAL L on L.WATS_WATYNO = B.WATS_WATYNO and L.WATS_WATYSEQ = B.WATS_WATYSEQ and  L.WATS_SN =  B.WATS_SN
            LEFT JOIN EXPORT E on  E.EXPORT_SERIALNO =  B.WATS_SN where RMAD_RMANO_18 is not Null 
            ) B on A.WATI_WATYNO =  B.WATS_WATYNO and A.WATI_SEQ =  B.WATS_WATYSEQ where WATI_WATYNO in (select distinct WATS_WATYNO from WARRANTYSERIAL_BI)  

            union all

            select distinct
            (select RMAD_RMANO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_RMANO,
            (select RMAD_SEQ from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_SEQ,
            (select RMAD_SERIALNO from RMA.RMADETAIL where (RMAD_RMANO = RMAD_RMANO_36 and RMAD_SEQ = RMAD_SEQ_36) ) RMAD_SERIALNO, 
            Total,WATS_WATYNO,WATS_WATYSEQ from WARRANTYITEM A
            left JOIN
            (
            select		
            RMAD_RMANO_36,
            RMAD_SEQ_36,
            B.WATS_WATYNO                ,
            B.WATS_WATYSEQ               ,
            Y.CU_NO                      ,
            Y.CU_NAME                    , 
            E.EXPORT_PARTNO              ,
            TO_CHAR(   L.WATS_WARRNSTART   , 'YYYY-MM-DD')   WATS_WARRNSTART     ,
            TO_CHAR(   L.WATS_WARRNEND   , 'YYYY-MM-DD')     WATS_WARRNEND   ,
            (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  ) - (select count(WATS_SN) from WARRANTYSERIAL_BI where WATS_WATYNO = B.WATS_WATYNO and WATS_WATYSEQ  = B.WATS_WATYSEQ  and ( RMAD_RMANO_36 is null  or WATS_WARRN_36 is null))  Total,
            TO_CHAR(B.WATS_WARRN_ORDER_14 , 'YYYY-MM-DD')      WATS_WARRN_ORDER_14  ,
            TO_CHAR(B.WATS_WARRN_18 , 'YYYY-MM-DD')      WATS_WARRN_18  ,
            TO_CHAR( B.WATS_WARRN_ORDER_32, 'YYYY-MM-DD')    WATS_WARRN_ORDER_32    ,
            TO_CHAR(B.WATS_WARRN_36, 'YYYY-MM-DD')        WATS_WARRN_36,
            B.EXPORT_ORDERNUMBER_18      ,
            B.EXPORT_ORDERNUMBER_SEQ_18  ,
            B.EXPORT_ORDERNUMBER_36      ,
            B.EXPORT_ORDERNUMBER_SEQ_36 
            from WARRANTYSERIAL_BI B 
            LEFT JOIN
            (
            select 
            O.WATY_NO         ,
            C.CU_NO           ,
            C.CU_NAME    
            from WARRANTYORD O 
            left join CUSTOMER C on O.WATY_CUST = C.CU_NO
            ) Y on B.WATS_WATYNO = Y.WATY_NO
            LEFT JOIN WARRANTYSERIAL L on L.WATS_WATYNO = B.WATS_WATYNO and L.WATS_WATYSEQ = B.WATS_WATYSEQ and  L.WATS_SN =  B.WATS_SN
            LEFT JOIN EXPORT E on  E.EXPORT_SERIALNO =  B.WATS_SN where RMAD_RMANO_36 is not Null 
            ) B on A.WATI_WATYNO =  B.WATS_WATYNO and A.WATI_SEQ =  B.WATS_WATYSEQ where WATI_WATYNO in (select distinct WATS_WATYNO from WARRANTYSERIAL_BI)  

            )
            group by RMAD_RMANO,RMAD_SEQ,WATS_WATYNO,WATS_WATYSEQ
            "
                dt = oQuery.ExecuteDT(sSQL)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        Return dt
    End Function


    Public Function Check_Standard_Battery(ByVal RMAD_ID As String) As DataTable

        Dim dt As New DataTable
        Dim RMAD_RMANO As String = ""
        Dim RMAD_SEQ As String = ""
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt_ As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " select * from RMADETAIL where RMAD_ID = :RMAD_ID "
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                dt_ = oQuery.ExecuteDT(sSQL)
                RMAD_RMANO = dt_.Rows(0)("RMAD_RMANO").ToString().Trim()
                RMAD_SEQ = dt_.Rows(0)("RMAD_SEQ").ToString().Trim()

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        If 1 = 1 Then

            Dim check As Boolean = False

            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = "
            select count(WATS_SN) from WARRANTYSERIAL_BI where RMAD_RMANO_18 =:RMAD_RMANO and RMAD_SEQ_18 =:RMAD_SEQ 
            UNION ALL
            select count(WATS_SN) from WARRANTYSERIAL_BI where RMAD_RMANO_36 =:RMAD_RMANO and RMAD_SEQ_36 =:RMAD_SEQ   and WAP_RULE = '36' 
            "

                oQuery.addWHERE("RMAD_RMANO", ":RMAD_RMANO", RMAD_RMANO, OracleType.VarChar)
                oQuery.addWHERE("RMAD_SEQ", ":RMAD_SEQ", RMAD_SEQ, OracleType.VarChar)
                dt = oQuery.ExecuteDT(sSQL)


            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If
        Return dt
    End Function

    Public Function Select_WARRANTYITEM_BI(ByVal WATI_WATYNO As String) As DataTable

        Dim check As Boolean = False
        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""
        oConn.Open()
        Try

            Dim sSQL As String = " select * from WARRSPECS_BI where WAP_WID = :WAP_WIDR "
            oQuery.addWHERE("WAP_WIDR", ":WAP_WIDR", WATI_WATYNO, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function
    '計算目前使用電池數量
    Public Function WARRANTYSERIAL_BI_Repaired_QTY(ByVal WATS_SN As String) As DataTable

        Dim check As Boolean = False
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""
        oConn.Open()
        Try

            Dim sSQL As String = " select
            WATS_WATYNO, WATS_WATYSEQ,  WATS_SN ,  'RMAD_RMANO_18'  item ,
            CASE WHEN RMAD_RMANO_18 is not Null  or  EXPORT_ORDERNUMBER_18 is not Null  THEN 'NO'
            ELSE
            'OK'
            END  RMAD_RMANO_18
            FROM   WARRANTYSERIAL_BI  where WATS_WATYNO in ( select distinct WATS_WATYNO from WARRANTYSERIAL_BI where WATS_SN =:WATS_SN )
            UNION ALL
            select
            WATS_WATYNO, WATS_WATYSEQ,  WATS_SN ,  'RMAD_RMANO_36'  item ,
            CASE WHEN RMAD_RMANO_36 is not Null  or  EXPORT_ORDERNUMBER_36 is not Null  THEN 'NO'
            ELSE
            'OK'
            END  RMAD_RMANO_36
            FROM   WARRANTYSERIAL_BI   where WATS_WATYNO in ( select distinct WATS_WATYNO from WARRANTYSERIAL_BI where WATS_SN =:WATS_SN  and WAP_RULE = '36' ) "
            oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    '需求新增:BI保固 By buck Add 20250902 begin
    '電池購買保固總表
    Public Function WARRANTY_BI_List() As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New Query(oConn)

        oConn.Open()
        Try
            'Dim sSQL As String = "
            '    WITH RMA_ACCT AS 
            '    (
            '        SELECT DISTINCT RMA_ACCOUNTID FROM RMA 
            '        WHERE RMA_NO in (SELECT RMAD_RMANO FROM RMADETAIL WHERE RMAD_SERIALNO = :BI_WATYNO)
            '    ), WATYORD AS
            '    (
            '        SELECT a.*,b.occ02 CU_NAME,b.occ04,c.gen02 
            '        FROM WarrantyOrd a,cipherlab.occ_file b,cipherlab.gen_file c 
            '        WHERE a.waty_cust=b.occ01 
            '        and a.WATY_SALESID=c.gen01(+) 
            '        and a.waty_mark='0'
            '        AND a.WATY_CUST in (SELECT RMA_ACCOUNTID FROM RMA_ACCT)
            '    )

            '    SELECT * 
            '    FROM WARRANTY_BI wbi
            '    LEFT JOIN WARRANTY_BI_EXPEND wbie ON wbi.BI_ORDERNO = wbie.BE_ORDERNO
            '    JOIN WATYORD wod ON wbie.BE_REFNO = wod.WATY_NO
            '    WHERE BE_PRODSERIAL = :BI_WATYNO
            '"
            Dim sSQL As String = "
                        SELECT 
                            BI_ID,
                            BI_SOURCE,
                            BI_ORDERNO,
                            BI_WATY_DATE,
                            BI_CUNO,
                            BI_CUNAME,
                            BI_ORDERSEQ,
                            BI_PRODUCTNO,
                            BI_ORDERQTY,
                            BI_WATY_TYPE,
                            BI_WATY_VER,
                            BI_WATYNO,
                            BI_BATTERYQTY,
                            BI_WATY_SDATE,
                            BI_WATY_EDATE,
                            BI_AD,
                            BI_ADNAME,
                            BI_CSTMP,
                            BI_LUAD,
                            BI_LUADNAME,
                            BI_LUSTMP
                        FROM WARRANTY_BI
                    "
            'Dim sSQL As String = "
            '            --隨保+續購 (不含序號)
            '            SELECT DISTINCT 'RMA' warr_source, waty_no, waty_date, waty_cust, occ02 waty_custname, wati_seq, wati_skuno, wati_qty, war_type, wati_ver, wati_ver_act waty_pno, decode(wati_year, 3, wati_qty, 5, wati_qty*2) batt_qty, to_char(wats_warrnstart, 'yyyy/MM/dd'), to_char(wats_warrnend, 'yyyy/MM/dd') 
            '            FROM warrantyord 
            '            JOIN warrantyitem ON waty_no = wati_watyno 
            '            JOIN warrantyserial ON wati_watyno = wats_watyno AND wati_seq=wats_watyseq
            '            JOIN warrset ON war_id = wati_ver
            '            JOIN cipherlab.occ_file ON occ01 = waty_cust
            '            WHERE war_type in ('PB', 'EB') 
            '            AND isconfirm = 'Y'
            '            AND wati_mark = 0 and wats_mark = 0
            '            UNION 
            '            SELECT DISTINCT 'AXBA', oea01, oea02, oea03, oea032, oeb03, oeb04, oeb24, decode(ta_oeb080, 5, 'EB', 6, 'PB') warr_type, ta_oeb084, FN_GetWarrantyPNOByWARRNO(ta_oeb084, 1, ta_oeb082) warr_pno, decode(ta_oeb082, 3, oeb24, 5, oeb24*2) batt_qty, to_char(cw_sdate, 'yyyy/MM/dd'), to_char(cw_edate, 'yyyy/MM/dd') 
            '            FROM cipherlab.oea_file 
            '            JOIN cipherlab.oeb_file ON oea01 = oeb01
            '            JOIN export ON export_ordernumber = oea01 || '-' || oeb03
            '            WHERE ta_oeb080 in (5, 6) --電池保
            '            AND oea00 = 1 --1:訂單, 0:合約
            '            AND oeb24 > 0 --已交數量
            '            AND oeaconf = 'Y' --已確認
            '        "

            '"
            'UNION
            'SELECT DISTINCT 
            '    'RMA1' AS warr_source,
            '    'WRMA-2022080099' AS waty_no,
            '    TO_DATE('2022/08/01', 'yyyy/mm/dd') AS waty_date,
            '    'BD006' AS waty_cust,
            '    'EET' AS waty_custname,
            '    9 AS wati_seq,
            '    'W4T34GDFG' AS wati_skuno,
            '    10 AS wati_qty,
            '    'PB' AS war_type,
            '    'RK95PBP3T00005' AS wati_ver,
            '    '' AS waty_pno,
            '    2 AS batt_qty,
            '    TO_CHAR(TO_DATE('2022/02/12', 'yyyy/mm/dd'), 'yyyy/MM/dd') AS warr_start,
            '    TO_CHAR(TO_DATE('2025/02/11', 'yyyy/mm/dd'), 'yyyy/MM/dd') AS warr_end
            'FROM dual
            '"

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    '電池購買保固明細
    Public Function WARRANTY_BI_Detail_List(model As RMADetailReq) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command
        Dim oQuery As New Query(oConn)

        oConn.Open()
        Try
            'Dim sSQL As String = "
            '            WITH WATY_BI AS (
            '                --隨保+續購 (含序號)
            '                SELECT 
            '                    'RMA' warr_source, 
            '                    waty_no, waty_date, waty_cust, occ02 waty_custname,
            '                    wati_seq, wati_skuno, wati_qty, war_type,
            '                    wati_ver, wati_ver_act waty_pno, wati_year,
            '                    decode(wati_year, 3, wati_qty, 5, wati_qty*2) batt_qty, 
            '                    wats_sn,
            '                    wats_warrnstart warr_start, 
            '                    wats_warrnend   warr_end
            '                FROM warrantyord 
            '                JOIN warrantyitem ON waty_no = wati_watyno 
            '                JOIN warrantyserial ON wati_watyno = wats_watyno AND wati_seq = wats_watyseq
            '                JOIN warrset ON war_id = wati_ver
            '                JOIN cipherlab.occ_file ON occ01 = waty_cust
            '                WHERE war_type IN ('PB', 'EB') 
            '                  AND isconfirm = 'Y'
            '                  AND wati_mark = 0 
            '                  AND wats_mark = 0

            '                UNION ALL

            '                SELECT 
            '                    'AXBA', oea01, oea02, oea03, oea032, oeb03,
            '                    oeb04, oeb24, decode(ta_oeb080, 5, 'EB', 6, 'PB') warr_type, 
            '                    ta_oeb084,
            '                    FN_GetWarrantyPNOByWARRNO(ta_oeb084, 1, ta_oeb082) warr_pno, ta_oeb082,
            '                    decode(ta_oeb082, 3, oeb24, 5, oeb24*2) batt_qty, 
            '                    export_serialno,
            '                    cw_sdate, 
            '                    cw_edate
            '                FROM cipherlab.oea_file 
            '                JOIN cipherlab.oeb_file ON oea01 = oeb01
            '                JOIN export ON export_ordernumber = oea01 || '-' || oeb03
            '                WHERE ta_oeb080 IN (5, 6)
            '                  AND oea00 = 1
            '                  AND oeb24 > 0
            '                  AND oeaconf = 'Y'
            '            )
            '            SELECT distinct warr_source, waty_no AS Order_No, waty_date,
            '                   waty_cust, waty_custname,
            '                   wati_seq, wati_skuno, wati_qty AS Order_Qty, war_type,
            '                   wati_year AS BI_Year,
            '                   wati_ver, waty_pno, batt_qty AS Replaceable_Qty, wats_sn,
            '                   to_char(warr_start, 'yyyy/MM/dd') warr_start,
            '                   to_char(warr_end,   'yyyy/MM/dd') warr_end
            '                    ,'' AS Qty_replaced
            '            FROM WATY_BI wbi
            '            LEFT JOIN EXPORT_PARTS prs on wbi.WATS_SN = prs.EXPAR_M_SN
            '            WHERE 1=1 
            '            AND prs.EXPAR_M_SN = :WATS_SN or prs.EXPAR_D_SN = :WATS_SN
            '        "
            Dim sSQL = "
                            WITH WATY_BI AS (
                            SELECT 
                                'RMA' AS warr_source, waty_no, waty_date, waty_cust, occ02 AS waty_custname,
                                wati_seq, wati_skuno, wati_qty, war_type, wati_ver, 
                                wati_ver_act AS waty_pno, wati_year,
                                DECODE(wati_year, 3, wati_qty, 5, wati_qty * 2) AS batt_qty, 
                                wats_sn, wats_warrnstart AS warr_start, wats_warrnend AS warr_end
                            FROM warrantyord 
                            JOIN warrantyitem ON waty_no = wati_watyno 
                            JOIN warrantyserial ON wati_watyno = wats_watyno AND wati_seq = wats_watyseq
                            JOIN warrset ON war_id = wati_ver
                            JOIN cipherlab.occ_file ON occ01 = waty_cust
                            WHERE war_type IN ('PB', 'EB') 
                              AND isconfirm = 'Y'
                              AND wati_mark = 0 AND wats_mark = 0
                              AND wats_sn = :WATS_SN

                            UNION ALL

                            SELECT 
                                'AXBA' AS warr_source, oea01, oea02, oea03, oea032, oeb03,
                                oeb04, oeb24, DECODE(ta_oeb080, 5, 'EB', 6, 'PB') AS warr_type, 
                                ta_oeb084,
                                FN_GetWarrantyPNOByWARRNO(ta_oeb084, 1, ta_oeb082) AS warr_pno, 
                                ta_oeb082,
                                DECODE(ta_oeb082, 3, oeb24, 5, oeb24 * 2) AS batt_qty, 
                                export_serialno, cw_sdate, cw_edate
                            FROM cipherlab.oea_file 
                            JOIN cipherlab.oeb_file ON oea01 = oeb01
                            JOIN export ON export_ordernumber = oea01 || '-' || oeb03
                            WHERE ta_oeb080 IN (5, 6)
                              AND oea00 = 1 AND oeb24 > 0 AND oeaconf = 'Y'
                              AND export_serialno = :WATS_SN
                        )
                        SELECT DISTINCT 
                            warr_source, waty_no AS Order_No, waty_date, waty_cust, waty_custname,
                            wati_seq, wati_skuno, wati_qty AS Order_Qty, war_type, wati_year AS BI_Year,
                            wati_ver, waty_pno, batt_qty AS Replaceable_Qty, wats_sn,
                            TO_CHAR(warr_start, 'yyyy/MM/dd') AS warr_start,
                            TO_CHAR(warr_end, 'yyyy/MM/dd') AS warr_end,
                            '' AS Qty_replaced
                        FROM WATY_BI wbi
                        WHERE EXISTS (
                            SELECT 1 FROM EXPORT_PARTS prs 
                            WHERE prs.EXPAR_M_SN = wbi.wats_sn OR prs.EXPAR_D_SN = wbi.wats_sn
                        )
                        OR wbi.wats_sn = :WATS_SN
                    "
            oCommand.Parameters.AddWithValue(":WATS_SN", model.RMAD_SERIALNO)

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function

    '需求新增:BI保固 By buck Add 20250902 end

    '需求新增:BI保固 By buck Add 20250902 begin
    '電池耗用紀錄表
    Public Function WARRANTY_BatExpend_Record(model As RMADetailReq) As DataTable

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command
        Dim oQuery As New Query(oConn)

        oConn.Open()
        Try
            Dim sSQL As String = "
                WITH PARTS_SN AS 
                (
                    SELECT EXPAR_M_SN 
                    FROM EXPORT_PARTS PARTS
                    LEFT JOIN RMAREPAIR_Detail RPD ON PARTS.EXPAR_D_ITEMNO = RPD.RMARED_NPARTNO AND PARTS.EXPAR_D_SN = RPD.RMARED_OSERIALNO
                    WHERE (PARTS.EXPAR_M_SN = :BI_WATYNO OR PARTS.EXPAR_D_SN = :BI_WATYNO OR RPD.RMARED_NSERIALNO = :BI_WATYNO)
                    GROUP BY EXPAR_M_SN
                ),RMA_ACCT AS 
                (
                    SELECT DISTINCT RMA_ACCOUNTID FROM RMA 
                    WHERE RMA_NO in (
                        SELECT RMAD_RMANO 
                        FROM RMADETAIL 
                        WHERE RMAD_STATUS = '90' 
                        AND RMAD_SERIALNO = (SELECT EXPAR_M_SN FROM PARTS_SN)
                    )
                ), WATYORD AS
                (
                    SELECT a.*,b.occ02 CU_NAME,b.occ04,c.gen02 
                    FROM WarrantyOrd a,cipherlab.occ_file b,cipherlab.gen_file c 
                    WHERE a.waty_cust=b.occ01 
                    and a.WATY_SALESID=c.gen01(+) 
                    and a.waty_mark='0'
                    AND a.WATY_CUST in (SELECT RMA_ACCOUNTID FROM RMA_ACCT)
                )

                SELECT * 
                FROM WARRANTY_BI_EXPEND wbie
                JOIN WATYORD wod ON wbie.BE_REFNO = wod.WATY_NO
            "

            oCommand.Parameters.AddWithValue(":BI_WATYNO", model.RMAD_SERIALNO)

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function
    '需求新增:BI保固 By buck Add 20250902 end

    '需求新增:BI保固 By buck Add 20250902 begin
    <Obsolete>
    Public Function InsertWarrantyBIExpen(watyBIExpendReqList As List(Of WarrantyBIExpendReq)) As Result
        Dim result As New Result
        Dim oConn As New Connection
        oConn.Open()
        oConn.BeginTransaction()

        Try
            Using cmd As OracleCommand = oConn.Command()
                cmd.CommandText = "
                    INSERT INTO WARRANTY_BI_EXPEND
                    (
                            BE_ID,
                            BE_ORDERNO,
                            BE_TYPE,
                            BE_REFNO,
                            BE_ORDERSEQ,
                            BE_PRODSERIAL,
                            BE_BATSERIAL_OLD,
                            BE_BATSERIAL_NEW,
                            BE_USEQTY,
                            BE_AD,
                            BE_ADNAME,
                            BE_CSTMP,
                            BE_LUAD,
                            BE_LUADNAME,
                            BE_LUSTMP
                    )
                    VALUES
                    (
                            :BE_ID,
                            :BE_ORDERNO,
                            :BE_TYPE,
                            :BE_REFNO,
                            :BE_ORDERSEQ,
                            :BE_PRODSERIAL,
                            :BE_BATSERIAL_OLD,
                            :BE_BATSERIAL_NEW,
                            :BE_USEQTY,
                            :BE_AD,
                            :BE_ADNAME,
                            :BE_CSTMP,
                            :BE_LUAD,
                            :BE_LUADNAME,
                            :BE_LUSTMP
                    )
                "

                For Each item As WarrantyBIExpendReq In watyBIExpendReqList
                    cmd.Parameters.Clear()

                    cmd.Parameters.Add(":BE_ID", OracleType.VarChar).Value = item.BE_ID
                    cmd.Parameters.Add(":BE_ORDERNO", OracleType.VarChar).Value = item.BE_ORDERNO
                    cmd.Parameters.Add(":BE_TYPE", OracleType.VarChar).Value = item.BE_TYPE
                    cmd.Parameters.Add(":BE_REFNO", OracleType.VarChar).Value = item.BE_REFNO
                    cmd.Parameters.Add(":BE_ORDERSEQ", OracleType.VarChar).Value = item.BE_ORDERSEQ
                    cmd.Parameters.Add(":BE_PRODSERIAL", OracleType.VarChar).Value = item.BE_PRODSERIAL
                    cmd.Parameters.Add(":BE_BATSERIAL_OLD", OracleType.VarChar).Value = item.BE_BATSERIAL_OLD
                    cmd.Parameters.Add(":BE_BATSERIAL_NEW", OracleType.VarChar).Value = item.BE_BATSERIAL_NEW
                    cmd.Parameters.Add(":BE_USEQTY", OracleType.VarChar).Value = item.BE_USEQTY
                    cmd.Parameters.Add(":BE_AD", OracleType.VarChar).Value = item.BE_AD
                    cmd.Parameters.Add(":BE_ADNAME", OracleType.VarChar).Value = item.BE_ADNAME
                    cmd.Parameters.Add(":BE_CSTMP", OracleType.DateTime).Value = item.BE_CSTMP
                    cmd.Parameters.Add(":BE_LUAD", OracleType.VarChar).Value = item.BE_LUAD
                    cmd.Parameters.Add(":BE_LUADNAME", OracleType.VarChar).Value = item.BE_LUADNAME
                    cmd.Parameters.Add(":BE_LUSTMP", OracleType.DateTime).Value = item.BE_LUSTMP

                    cmd.ExecuteNonQuery()
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
    '需求新增:BI保固 By buck Add 20250902 end

    Public Function Select_WARRANTYSERIAL_BI(ByVal WATS_WATYNO As String) As DataTable

        Dim check As Boolean = False
        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""
        oConn.Open()
        Try

            Dim sSQL As String = " 
            select WATS_WATYNO,WATS_WATYSEQ,WATS_SN,WATS_WARRNSTART,WATI_VER from WARRANTYSERIAL S  
            left join  WARRANTYORD O on O.WATY_NO = S.WATS_WATYNO and  O.WATY_MARK = 0
            left join  WARRANTYITEM I on I.WATI_WATYNO = S.WATS_WATYNO  and  I.WATI_SEQ = S.WATS_WATYSEQ  and I.WATI_MARK = 0
            where  S.WATS_WATYNO  =:WATS_WATYNO  and  WATY_MARK = 0  "
            oQuery.addWHERE("WATS_WATYNO", ":WATS_WATYNO", WATS_WATYNO, OracleType.VarChar)

            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function
    Public Function WARRSPECS_BI_Insert(ByVal WAP_WID As String, ByVal WAP_SEQ As String, ByVal WAP_NAME As String, ByVal WAP_RULE As String, ByVal WAP_AD As String, ByVal WAP_ADNAME As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command


        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " INSERT INTO WARRSPECS_BI(WAP_WID,WAP_SEQ,WAP_NAME,WAP_RULE,WAP_AD,WAP_ADNAME,WAP_CSTMP) VALUES (:WAP_WID,:WAP_SEQ,:WAP_NAME,:WAP_RULE,:WAP_AD,:WAP_ADNAME,SYSDATE) "
            oCommand.Parameters.AddWithValue(":WAP_WID", WAP_WID)
            oCommand.Parameters.AddWithValue(":WAP_SEQ", WAP_SEQ)
            oCommand.Parameters.AddWithValue(":WAP_NAME", WAP_NAME)
            oCommand.Parameters.AddWithValue(":WAP_RULE", WAP_RULE)
            oCommand.Parameters.AddWithValue(":WAP_AD", WAP_AD)
            oCommand.Parameters.AddWithValue(":WAP_ADNAME", WAP_ADNAME)

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function
    Public Function WARRANTYSERIAL_BI_Insert(ByVal WATS_WATYNO As String, ByVal WATS_WATYSEQ As String, ByVal WATS_SN As String, ByVal WAP_WID As String, ByVal WAP_RULE As String, ByVal WATS_WARRN_ORDER_14 As Date, ByVal WATS_WARRN_18 As Date, ByVal WATS_WARRN_ORDER_32 As Date, ByVal WATS_WARRN_36 As Date, ByVal WATS_AD As String, ByVal WATS_ADNAME As String, ByVal WATS_CSTMP As Date)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command


        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try

            sSQL = " INSERT INTO WARRANTYSERIAL_BI( WATS_WATYNO,WATS_WATYSEQ,WATS_SN,WAP_WID,WAP_RULE,WATS_WARRN_ORDER_14,WATS_WARRN_18,WATS_WARRN_ORDER_32,WATS_WARRN_36,WATS_AD,WATS_ADNAME,WATS_CSTMP ) VALUES ( :WATS_WATYNO,:WATS_WATYSEQ,:WATS_SN,:WAP_WID,:WAP_RULE,:WATS_WARRN_ORDER_14,:WATS_WARRN_18,:WATS_WARRN_ORDER_32,:WATS_WARRN_36,:WATS_AD,:WATS_ADNAME,:WATS_CSTMP ) "

            oCommand.Parameters.AddWithValue(":WATS_WATYNO", WATS_WATYNO)
            oCommand.Parameters.AddWithValue(":WATS_WATYSEQ", Convert.ToInt32(WATS_WATYSEQ))
            oCommand.Parameters.AddWithValue(":WATS_SN", WATS_SN)
            oCommand.Parameters.AddWithValue(":WAP_WID", WAP_WID)
            oCommand.Parameters.AddWithValue(":WAP_RULE", Convert.ToInt32(WAP_RULE))
            oCommand.Parameters.AddWithValue(":WATS_WARRN_ORDER_14", WATS_WARRN_ORDER_14)
            oCommand.Parameters.AddWithValue(":WATS_WARRN_18", WATS_WARRN_18)
            oCommand.Parameters.AddWithValue(":WATS_WARRN_ORDER_32", WATS_WARRN_ORDER_32)
            oCommand.Parameters.AddWithValue(":WATS_WARRN_36", WATS_WARRN_36)
            oCommand.Parameters.AddWithValue(":WATS_AD", WATS_AD)
            oCommand.Parameters.AddWithValue(":WATS_ADNAME", WATS_ADNAME)
            oCommand.Parameters.AddWithValue(":WATS_CSTMP", WATS_CSTMP)


            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function
    Public Function Select_Repaired_WARRANTYSERIAL_BI(ByVal WATS_SN As String) As DataTable

        Dim check As Boolean = False
        Dim dt As New DataTable
        'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""
        Dim sWarrGroup As String = ""
        oConn.Open()
        Try

            Dim sSQL As String = " select * from WARRANTYSERIAL_BI where WATS_SN = :WATS_SN "
            oQuery.addWHERE("WATS_SN", ":WATS_SN", WATS_SN, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)


        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try
        Return dt
    End Function
    Public Function Up_Repaired_RMAD_RMANO_18__WARRANTYSERIAL_BI(ByVal RMAD_RMANO_18 As String, ByVal RMAD_SEQ_18 As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        oConn.Open()

        Try

            sSQL = " UPDATE WARRANTYSERIAL_BI  SET RMAD_RMANO_18  = Null , RMAD_SEQ_18 = Null where RMAD_RMANO_18  =:RMAD_RMANO_18  and  RMAD_SEQ_18 =:RMAD_SEQ_18 "
            oCommand.Parameters.AddWithValue(":RMAD_RMANO_18", RMAD_RMANO_18)
            oCommand.Parameters.AddWithValue(":RMAD_SEQ_18", Convert.ToInt32(RMAD_SEQ_18))

            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function
    Public Function Up_Repaired_RMAD_RMANO_36__WARRANTYSERIAL_BI(ByVal RMAD_RMANO_36 As String, ByVal RMAD_SEQ_36 As String)

        Dim oConn As New Connection
        Dim oCommand As OracleCommand = oConn.Command
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        oConn.Open()

        Try

            sSQL = " UPDATE WARRANTYSERIAL_BI  SET RMAD_RMANO_36  = Null , RMAD_SEQ_36 = Null where RMAD_RMANO_36  =:RMAD_RMANO_36  and  RMAD_SEQ_36 =:RMAD_SEQ_36 "
            oCommand.Parameters.AddWithValue(":RMAD_RMANO_36", RMAD_RMANO_36)
            oCommand.Parameters.AddWithValue(":RMAD_SEQ_36", Convert.ToInt32(RMAD_SEQ_36))


            oCommand.CommandText = sSQL
            oCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Function

    Public Function Up_Repaired_WATS_SN_WARRANTYSERIAL_BI(ByVal RMAD_ID As String)

        Dim RMAD_RMANO As String = ""
        Dim RMAD_SEQ As String = ""
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " select * from RMADETAIL where RMAD_ID = :RMAD_ID "
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                dt = oQuery.ExecuteDT(sSQL)
                RMAD_RMANO = dt.Rows(0)("RMAD_RMANO").ToString().Trim()
                RMAD_SEQ = dt.Rows(0)("RMAD_SEQ").ToString().Trim()

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        '還原
        If 1 = 1 Then
            Call Up_Repaired_RMAD_RMANO_18__WARRANTYSERIAL_BI(RMAD_RMANO, RMAD_SEQ)
            Call Up_Repaired_RMAD_RMANO_36__WARRANTYSERIAL_BI(RMAD_RMANO, RMAD_SEQ)
        End If

    End Function

    Public Function Up_Repaired_RMAD_RMANO_WARRANTYSERIAL_BI(ByVal WATS_SN As String, ByVal RMAD_ID As String, ByVal Item As String)

        Dim RMAD_RMANO As String = ""
        Dim RMAD_SEQ As String = ""
        '查資料
        If 1 = 1 Then

            Dim check As Boolean = False
            Dim dt As New DataTable
            'Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)
            Dim sCondition As String = ""
            Dim sWarrGroup As String = ""
            oConn.Open()
            Try

                Dim sSQL As String = " select * from RMADETAIL where RMAD_ID = :RMAD_ID "
                oQuery.addWHERE("RMAD_ID", ":RMAD_ID", RMAD_ID, OracleType.VarChar)
                dt = oQuery.ExecuteDT(sSQL)
                RMAD_RMANO = dt.Rows(0)("RMAD_RMANO").ToString().Trim()
                RMAD_SEQ = dt.Rows(0)("RMAD_SEQ").ToString().Trim()

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

        '紀錄使用
        If 1 = 1 Then

            Dim oConn As New Connection
            Dim oCommand As OracleCommand = oConn.Command
            Dim sSQL As String = ""
            Dim sCondition As String = ""
            oConn.Open()

            Try

                If Item = "RMAD_RMANO_18" Then
                    sSQL = " UPDATE WARRANTYSERIAL_BI  SET RMAD_RMANO_18  =:RMAD_RMANO , RMAD_SEQ_18 =:RMAD_SEQ where WATS_SN  =:WATS_SN   "
                Else
                    sSQL = " UPDATE WARRANTYSERIAL_BI  SET RMAD_RMANO_36  =:RMAD_RMANO , RMAD_SEQ_36 =:RMAD_SEQ where WATS_SN  =:WATS_SN   "

                End If
                oCommand.Parameters.AddWithValue(":WATS_SN", WATS_SN)
                oCommand.Parameters.AddWithValue(":RMAD_RMANO", RMAD_RMANO)
                oCommand.Parameters.AddWithValue(":RMAD_SEQ", Convert.ToInt32(RMAD_SEQ))

                oCommand.CommandText = sSQL
                oCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

        End If

    End Function

#End Region

#End Region

    Public Function getSerialNumberWarrGroup(ByVal SerialNo As String) As String
        Dim retval As String = ""
        Dim sSQL As String = ""
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
            'sTableName = "tc_oae_file"
            'End If
            sSQL = "select  tc_oae250  from export," & sTableName & " where export_partno=tc_oae010" & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("tc_oae250").ToString()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


        Return retval
    End Function

    Public Sub SaveAddWarrParts(ByVal dtWarrParts As WarrantyDTO.WarrPartsDataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim nSeq As Integer = 0
            Dim sSQL As String = ""
            If (dtWarrParts.Rows.Count > 0) Then
                sSQL = "SELECT NVL(MAX(WAP_SEQ),0) WAP_SEQ FROM WarrParts WHERE WAP_WID='" & dtWarrParts.Rows(0)("WAP_WID").ToString().Trim() & "'"
                dt = oQuery.ExecuteDT(sSQL)
                nSeq = Convert.ToInt16(dt.Rows(0)("WAP_SEQ").ToString())
            End If

            For i = 0 To dtWarrParts.Rows.Count - 1
                Dim dr As WarrantyDTO.WarrPartsRow = dtWarrParts.Rows(i)

                Dim nVersionNo As Integer = 1
                sSQL = "SELECT WAP_WID FROM WarrParts WHERE WAP_WID='" & dr.WAP_WID & "' AND WAP_SEQ=" & dr.WAP_SEQ
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    oExecute.addParameter("WAP_NAME", dr.WAP_NAME, OracleType.VarChar)
                    oExecute.addParameter("WAP_MON", dr.WAP_MON, OracleType.Number)
                    oExecute.addParameter("WAP_EMON", dr.WAP_EMON, OracleType.Number)
                    oExecute.addParameter("WAP_DESC", dr.WAP_DESC, OracleType.VarChar)
                    oExecute.addParameter("WAP_LUAD", dr.WAP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("WAP_LUADNAME", dr.WAP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("WAP_LUSTMP", dr.WAP_LUSTMP, OracleType.DateTime)

                    oExecute.addWHERE("WAP_WID", dr.WAP_WID, OracleType.VarChar)
                    oExecute.addWHERE("WAP_SEQ", dr.WAP_SEQ, OracleType.VarChar)
                    oExecute.Command("WarrParts", Execute.eumCommandType.UPDATE)
                Else
                    oExecute.addParameter("WAP_WID", dr.WAP_WID, OracleType.VarChar)
                    oExecute.addParameter("WAP_SEQ", dr.WAP_SEQ, OracleType.VarChar)
                    oExecute.addParameter("WAP_NAME", dr.WAP_NAME, OracleType.VarChar)
                    oExecute.addParameter("WAP_MON", dr.WAP_MON, OracleType.Number)
                    oExecute.addParameter("WAP_EMON", dr.WAP_EMON, OracleType.Number)
                    oExecute.addParameter("WAP_DESC", dr.WAP_DESC, OracleType.VarChar)

                    oExecute.addParameter("WAP_LUAD", dr.WAP_LUAD, OracleType.VarChar)
                    oExecute.addParameter("WAP_LUADNAME", dr.WAP_LUADNAME, OracleType.VarChar)
                    oExecute.addParameter("WAP_LUSTMP", dr.WAP_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("WAP_AD", dr.WAP_AD, OracleType.VarChar)
                    oExecute.addParameter("WAP_ADNAME", dr.WAP_ADNAME, OracleType.VarChar)
                    oExecute.addParameter("WAP_CSTMP", dr.WAP_CSTMP, OracleType.DateTime)

                    oExecute.Command("WarrParts", Execute.eumCommandType.AddNew)
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

    Public Sub SaveAddWarrSpecs(ByVal dtWarrSpecs As DataTable)
        Dim i As Integer = 0
        Dim oCrypto As New SecurityCrypt.Crypto
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable

        oConn.Open()

        Try
            oConn.BeginTransaction()

            Dim nSeq As Integer = 0
            Dim sSQL As String = ""
            If (dtWarrSpecs.Rows.Count > 0) Then
                sSQL = "SELECT NVL(MAX(WAP_SEQ),0) WAP_SEQ FROM WarrSpecs WHERE WAP_WID='" & dtWarrSpecs.Rows(0)("WAP_WID").ToString().Trim() & "'"
                dt = oQuery.ExecuteDT(sSQL)
                nSeq = Convert.ToInt16(dt.Rows(0)("WAP_SEQ").ToString())
            End If

            For i = 0 To dtWarrSpecs.Rows.Count - 1
                Dim dr As DataRow = dtWarrSpecs.Rows(i)

                Dim nVersionNo As Integer = 1
                sSQL = "SELECT WAP_WID FROM WarrSpecs WHERE WAP_WID='" & dr("WAP_WID") & "' AND WAP_SEQ=" & dr("WAP_SEQ")
                dt = oQuery.ExecuteDT(sSQL)
                If dt.Rows.Count > 0 Then
                    oExecute.addParameter("WAP_NAME", dr("WAP_NAME"), OracleType.VarChar)
                    oExecute.addParameter("WAP_RULE", dr("WAP_RULE"), OracleType.Number)
                    oExecute.addParameter("WAP_AD", dr("WAP_AD"), OracleType.VarChar)
                    oExecute.addParameter("WAP_ADNAME", dr("WAP_ADNAME"), OracleType.VarChar)
                    oExecute.addParameter("WAP_CSTMP", dr("WAP_CSTMP"), OracleType.DateTime)

                    oExecute.addWHERE("WAP_WID", dr("WAP_WID"), OracleType.VarChar)
                    oExecute.addWHERE("WAP_SEQ", dr("WAP_SEQ"), OracleType.VarChar)
                    oExecute.Command("WarrSpecs", Execute.eumCommandType.UPDATE)
                Else
                    oExecute.addParameter("WAP_WID", dr("WAP_WID"), OracleType.VarChar)
                    oExecute.addParameter("WAP_SEQ", dr("WAP_SEQ"), OracleType.VarChar)
                    oExecute.addParameter("WAP_NAME", dr("WAP_NAME"), OracleType.VarChar)
                    oExecute.addParameter("WAP_RULE", dr("WAP_RULE"), OracleType.Number)

                    oExecute.addParameter("WAP_AD", dr("WAP_AD"), OracleType.VarChar)
                    oExecute.addParameter("WAP_ADNAME", dr("WAP_ADNAME"), OracleType.VarChar)
                    oExecute.addParameter("WAP_CSTMP", dr("WAP_CSTMP"), OracleType.DateTime)

                    oExecute.Command("WarrSpecs", Execute.eumCommandType.AddNew)
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

    Public Function GetWarrantData(ByVal SN As String) As DataTable
        Dim sSQL As String = ""
        Dim dt As DataTable
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()

        Try
            '" SELECT DISTINCT '1' type,a.EXPORT_SERIALNO,'Comprehensive Coverage' TypeName, d.PROGRAM_TYPE_DESC, " &

            sSQL = " SELECT DISTINCT '0' type,a.EXPORT_PARTNO,'' TypeName, '' PROGRAM_TYPE_DESC, " &
                   " TO_CHAR(b.EXPORT_SHIPPING_TIME,'dd-mm-yyyy') sDate ,TO_CHAR(ADD_MONTHS(b.EXPORT_SHIPPING_TIME,NVL(c.WAR_STDYY,1) * 12),'dd-mm-yyyy') eDate " &
                   " From EXPORT a" &
                   " JOIN (SELECT EXPORT_SERIALNO,MIN(EXPORT_SHIPPING_TIME) EXPORT_SHIPPING_TIME FROM EXPORT " &
                   "       WHERE EXPORT_SERIALNO =:SERIALNO " &
                   "       GROUP BY EXPORT_SERIALNO) b On b.EXPORT_SERIALNO = a.EXPORT_SERIALNO And b.EXPORT_SHIPPING_TIME = a.EXPORT_SHIPPING_TIME " &
                   " LEFT JOIN WARRSET c ON c.WAR_TYPE=a.EXPORT_MODELNO AND c.WAR_VERSION='0' " &
                   " UNION " &
                   " SELECT DISTINCT '1' type,a.EXPORT_SERIALNO,CASE WHEN NVL(e.WARRSET_TYPE_DESC,' ') <> ' ' THEN e.WARRSET_TYPE_DESC " &
                   " WHEN months_between(a.EXPORT_WARRANTY_DATE,a.EXPORT_SHIPPING_TIME) > 13 THEN 'Ext.Warranty Effective' " &
                   " WHEN NVL (TO_CHAR (CW_SDATE, 'yyyy'), ' ') <> ' ' " &
                   " THEN NVL(e.WARRSET_TYPE_DESC,'Premium Comprehensive Warranty') ELSE '' END TypeName, d.PROGRAM_TYPE_DESC, " &
                   " CASE WHEN NVL(to_char(CW_SDATE,'yyyy'),' ') <> ' ' THEN TO_CHAR(CW_SDATE,'dd-mm-yyyy') ELSE TO_CHAR(b.EXPORT_SHIPPING_TIME,'dd-mm-yyyy') END sDate," &
                   " CASE WHEN NVL(to_char(CW_SDATE,'yyyy'),' ') <> ' ' THEN TO_CHAR(CW_EDATE,'dd-mm-yyyy') ELSE TO_CHAR(EXPORT_WARRANTY_DATE,'dd-mm-yyyy') END eDate " &
                   " FROM EXPORT a " &
                   " JOIN (SELECT EXPORT_SERIALNO,MAX(EXPORT_SHIPPING_TIME) EXPORT_SHIPPING_TIME FROM EXPORT " &
                   "       WHERE EXPORT_SERIALNO=:SERIALNO " &
                   "       GROUP BY EXPORT_SERIALNO) b ON b.EXPORT_SERIALNO = a.EXPORT_SERIALNO And b.EXPORT_SHIPPING_TIME = a.EXPORT_SHIPPING_TIME " &
                   " LEFT JOIN WARRSET c ON c.WAR_ID = a.EXPORT_WAR_ID " &
                   " LEFT JOIN WARRSET_PROGRAM_TYPE d ON d.WARRSET_TYPE = c.WAR_TYPE AND d.PROGRAM_TYPE = c.war_program_type " &
                   " LEFT JOIN WARRSET_TYPE e ON e.WARRSET_TYPE = c.WAR_TYPE " &
                   " UNION " &
                   " SELECT DISTINCT '2' type,a.serial,c.AST_TYPENAME,'' PROGRAM_TYPE_DESC,null sDate,null eDate FROM CIPHERLAB.AKEY_DETAIL_FILE a " &
                   " JOIN CIPHERLAB.AKEY_LOGIN_FILE b ON b.AKEY_OGB01 = a.AKEY_OGB01 And b.AKEY_OGB03 = a.AKEY_OGB03 " &
                   " JOIN CIPHERLAB.AKEY_SOFTWARE_TYPE c ON c.AST_TYPE = b.SOFTWARETYPE " &
                   " WHERE a.SERIAL=:SERIALNO"

            oQuery.addWHERE("EXPORT_SERIALNO", ":SERIALNO", SN, OracleType.VarChar)
            oQuery.addWHERE("SERIAL", ":SERIALNO", SN, OracleType.VarChar)
            dt = oQuery.ExecuteDT(sSQL)

            Return dt
        Catch ex As Exception
            Throw ex

        Finally
            oConn.Close()
            oConn.Dispose()
        End Try


    End Function

End Class

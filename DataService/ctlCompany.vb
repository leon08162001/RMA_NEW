Imports System.Data.OracleClient
Imports ICAT_OracleDAO

Public Class ctlCompany

    ''' <summary>
    ''' 取得WARRSET TYPE資料
    ''' </summary>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryWarrsetType(Optional ByVal TYPE_USE As Boolean = True, Optional ByVal sCustNo As String = "") As DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            'If OrderBY.Trim = "" Then
            '    OrderBY = " WARRSET_SEQ asc"
            'End If
            'OrderBY = " ORDER BY " & OrderBY
            If sCustNo = "" Then
                sSQL = "SELECT WARRSET_TYPE, WARRSET_TYPE_NAME FROM WARRSET_TYPE WHERE WARRSET_SEQ >= 1 "
                If TYPE_USE Then
                    sSQL += "And WARRSET_TYPE_USE='Y' "
                End If
                sSQL += " ORDER BY WARRSET_SEQ asc"
            Else
                sSQL = "SELECT WARRSET_RMA_TYPE WARRSET_TYPE,WARRSET_RMA_NAME WARRSET_TYPE_NAME FROM TABLE(FN_SP_GetRMAWarrantyType('" + sCustNo + "')) "
                'oQuery.addWHERE("CustNO", ":CustNO", sCustNo, OracleType.VarChar)
            End If


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
    ''' 取得Item Type資料
    ''' </summary>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryItemType(ByVal WarrsetType As String, Optional ByVal OrderBY As String = "") As DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " ITEM_TYPE asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WarrsetType <> "" Then
                oQuery.addWHERE("WARRSET_TYPE", ":WARRSET_TYPE", WarrsetType, OracleType.VarChar)
                sSQL = "SELECT ITEM_TYPE, ITEM_TYPE_NAME FROM WARRSET_ITEM_TYPE WHERE WARRSET_TYPE =:WARRSET_TYPE " & OrderBY
            Else
                sSQL = "SELECT DISTINCT ITEM_TYPE, ITEM_TYPE_NAME FROM WARRSET_ITEM_TYPE " & OrderBY
            End If



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
    ''' 取得Price Type資料
    ''' </summary>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryPriceVer(ByVal WarrsetType As String, Optional ByVal OrderBY As String = "") As DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " PRICE_VER asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WarrsetType <> "" Then
                oQuery.addWHERE("WARRSET_TYPE", ":WARRSET_TYPE", WarrsetType, OracleType.VarChar)
                sSQL = "SELECT PRICE_VER, PRICE_VER_NAME FROM WARRSET_PRICE_VER WHERE WARRSET_TYPE =:WARRSET_TYPE " & OrderBY
            Else
                sSQL = "SELECT DISTINCT PRICE_VER, PRICE_VER_NAME FROM WARRSET_PRICE_VER " & OrderBY
            End If


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
    ''' 取得Project Type資料
    ''' </summary>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryProgramType(ByVal WarrsetType As String, Optional ByVal OrderBY As String = "") As DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " PROGRAM_TYPE asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If WarrsetType <> "" Then
                oQuery.addWHERE("WARRSET_TYPE", ":WARRSET_TYPE", WarrsetType, OracleType.VarChar)
                sSQL = "SELECT PROGRAM_TYPE, PROGRAM_TYPE_NAME FROM WARRSET_PROGRAM_TYPE WHERE WARRSET_TYPE =:WARRSET_TYPE " & OrderBY

            Else
                sSQL = "SELECT DISTINCT PROGRAM_TYPE, PROGRAM_TYPE_NAME FROM WARRSET_PROGRAM_TYPE " & OrderBY
            End If

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
    ''' 取得保固品名
    ''' </summary>
    ''' <param name="War_Group"></param>
    ''' <param name="WARRSET_TYPE"></param>
    ''' <param name="Program_Type"></param>
    ''' <param name="Item_Type"></param>
    ''' <param name="Price_Ver"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWarrsetPartNM(ByVal War_Group As String, ByVal WARRSET_TYPE As String, ByVal Program_Type As String, ByVal Item_Type As String, ByVal Price_Ver As String) As String
        Dim sSQL As String = ""
        Dim sCondition As String = ""
        Dim retval As String = ""
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            oQuery.addWHERE("War_Group", ":War_Group", War_Group, OracleType.VarChar)
            oQuery.addWHERE("WARRSET_TYPE", ":WARRSET_TYPE", WARRSET_TYPE, OracleType.VarChar)
            oQuery.addWHERE("Program_Type", ":Program_Type", Program_Type, OracleType.VarChar)
            oQuery.addWHERE("Item_Type", ":Item_Type", Item_Type, OracleType.VarChar)
            oQuery.addWHERE("Price_Ver", ":Price_Ver", Price_Ver, OracleType.VarChar)
            sSQL = "SELECT FN_GET_WARRSET_PARTNM(:War_Group,:WARRSET_TYPE,:Program_Type,:Item_Type,:Price_Ver) part_nm FROM dual "

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("part_nm").ToString().Trim()
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
    ''' 取得Company資料
    ''' </summary>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryAll(Optional ByVal OrderBY As String = "") As CompanyDTO.CompanyDataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " Comp_No asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            sSQL = "SELECT * FROM COMPANY " & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtCompany)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtCompany
    End Function

    ''' <summary>
    ''' 取得有效的Company資料
    ''' </summary>
    ''' <param name="OrderBY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByEffective(Optional ByVal OrderBY As String = "") As CompanyDTO.CompanyDataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " Comp_No asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            oQuery.addWHERE("COMP_VISIBLE", ":COMP_VISIBLE", "1", OracleType.Int16)
            sCondition = sCondition & " AND COMP_VISIBLE=:COMP_VISIBLE"

            sSQL = "SELECT * FROM COMPANY WHERE 1=1" & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtCompany)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtCompany
    End Function

    ''' <summary>
    ''' 取得 Company 及幣別相關資料
    ''' </summary>
    ''' <param name="Comp_No"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByCurrency(ByVal Comp_No As String) As CompanyDTO.vwCompany_CurrencyDataTable
        Dim dtCompany As New CompanyDTO.vwCompany_CurrencyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            Dim sSQL As String = "SELECT * FROM vwCompany_Currency WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtCompany)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtCompany
    End Function

    ''' <summary>
    ''' 取得 Company 是否扣庫存
    ''' </summary>
    ''' <param name="Comp_No"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryByStock(ByVal Comp_No As String) As String
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim retval As String = "N"

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            Dim sSQL As String = "SELECT COMP_STOCKMANAGER FROM COMPANY WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COMP_STOCKMANAGER").ToString().Trim()
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
    ''' 取得 公司代碼 資料
    ''' </summary>
    ''' <param name="Comp_No">公司代碼</param>
    ''' <returns>傳回 CompanyDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryByPrimaryKey(ByVal Comp_No As String) As CompanyDTO.CompanyDataTable
        Dim sSQL As String = ""

        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim dt As New DataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            sSQL = "SELECT * FROM COMPANY WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtCompany)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtCompany
    End Function

    ''' <summary>
    ''' 取得 維修中心名稱 資料
    ''' </summary>
    ''' <param name="Comp_No">公司代碼</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getRepairName(ByVal Comp_No As String) As String
        Dim retval As String = ""
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            Dim sSQL As String = "SELECT * FROM COMPANY WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COMP_NAME").ToString().Trim()
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
    ''' 取得 維修中心名稱 資料
    ''' </summary>
    ''' <param name="COMPNo">公司代碼</param>
    ''' <returns>回傳 CompanyDataTable</returns>
    ''' <remarks></remarks>
    Public Function QueryByRepairName(ByVal COMPNo As String) As CompanyDTO.CompanyDataTable
        Dim i As Integer = 0
        Dim dt As New DataTable
        Dim dtCompany As New CompanyDTO.CompanyDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)


        oConn.Open()
        Try
            Dim sCondition As String = ""
            Dim sCondition_Repair As String = ""
            sCondition = sCondition & " AND ("

            Dim arrRepair() As String = COMPNo.Split(",")
            For i = 0 To arrRepair.Length - 1
                oQuery.addWHERE("COMP_NO", ":COMP_NO" & i.ToString, arrRepair(i).Trim(), OracleType.VarChar)
                If sCondition_Repair.Trim <> "" Then
                    sCondition_Repair = sCondition_Repair & " OR "
                End If
                sCondition_Repair = sCondition_Repair & " COMP_NO =:COMP_NO" & i.ToString
            Next
            sCondition = sCondition & sCondition_Repair & ")"

            Dim sSQL As String = "SELECT * FROM COMPANY WHERE 1=1 " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtCompany)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtCompany
    End Function

    ''' <summary>
    ''' 取得 同意維修金額 資料
    ''' </summary>
    ''' <param name="Comp_No">公司代碼</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getApprovAmount(ByVal Comp_No As String) As String
        Dim retval As String = ""
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            Dim sSQL As String = "SELECT * FROM COMPANY WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COMP_APPROVALAMOUNT").ToString().Trim()
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
    ''' 取得 工時單價 資料
    ''' </summary>
    ''' <param name="Comp_No">公司代碼</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getLaborCost(ByVal Comp_No As String) As String
        Dim retval As String = ""
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            oQuery.addWHERE("COMP_NO", ":COMP_NO", Comp_No, OracleType.VarChar)

            Dim sSQL As String = "SELECT * FROM COMPANY WHERE COMP_NO=:COMP_NO"

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COMP_LABORCOST").ToString().Trim()
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
    ''' 取得公司代碼是否存在
    ''' </summary>
    ''' <param name="new_CompNo">新的公司代碼</param>
    ''' <param name="old_CompNo">舊的公司代碼</param>
    ''' <returns>傳回 Boolean</returns>
    ''' <remarks>False:不存在, True:存在</remarks>
    Public Function chkIsExist(ByVal new_CompNo As String, Optional ByVal old_CompNo As String = "") As Boolean
        Dim retval As Boolean = False
        Dim sCondition As String = ""

        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            If old_CompNo.Trim() <> "" Then
                oQuery.addWHERE("COMP_NO", ":old_CompNo", old_CompNo, OracleType.VarChar)
                sCondition = sCondition & " AND COMP_NO<>:old_CompNo"
            End If

            oQuery.addWHERE("COMP_NO", ":new_CompNo", new_CompNo, OracleType.VarChar)
            Dim sSQL As String = "SELECT * FROM COMPANY WHERE COMP_NO=:new_CompNo " & sCondition

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
    ''' Company-新增
    ''' </summary>
    ''' <param name="dtCompany">傳入dtCompany</param>
    ''' <remarks></remarks>
    Public Sub SaveAdd(ByVal dtCompany As CompanyDTO.CompanyDataTable)
        Dim i As Integer = 0

        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)
        Dim oQuery As New Query(oConn)

        oConn.Open()

        Try
            oConn.BeginTransaction()

            For i = 0 To dtCompany.Rows.Count - 1
                Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

                oExecute.addParameter("COMP_NO", dr.COMP_NO, OracleType.VarChar)
                oExecute.addParameter("COMP_NAME", dr.COMP_NAME, OracleType.NVarChar)
                'oExecute.addParameter("COMP_INVOICE", dr.COMP_INVOICE, OracleType.VarChar)              '統一編號

                oExecute.addParameter("COMP_TEL", dr.COMP_TEL, OracleType.VarChar)
                oExecute.addParameter("COMP_ADDRESS", dr.COMP_ADDRESS, OracleType.NVarChar)
                oExecute.addParameter("COMP_COUNTRYID", dr.COMP_COUNTRYID, OracleType.VarChar)
                oExecute.addParameter("COMP_CURRENCYCODE", dr.COMP_CURRENCYCODE, OracleType.VarChar)

                'oExecute.addParameter("COMP_URL", dr.COMP_URL, OracleType.VarChar)                     '公司URL
                oExecute.addParameter("COMP_ISROLE", 1, OracleType.Int16)                               '公司角色:1.維修中心
                oExecute.addParameter("COMP_ISREPAIR", 1, OracleType.Int16)                             '是否附設維修中心:0.否, 1.是
                oExecute.addParameter("COMP_VISIBLE", dr.COMP_VISIBLE, OracleType.Int16)

                oExecute.addParameter("COMP_EXPRESSCO", dr.COMP_EXPRESSCO, OracleType.NVarChar)
                oExecute.addParameter("COMP_EXPRESSURL", dr.COMP_EXPRESSURL, OracleType.VarChar)
                oExecute.addParameter("COMP_REMARK", dr.COMP_REMARK, OracleType.NVarChar)
                oExecute.addParameter("COMP_STOCKMANAGER", dr.COMP_STOCKMANAGER, OracleType.NVarChar)

                oExecute.addParameter("COMP_AD", dr.COMP_AD, OracleType.NVarChar)
                oExecute.addParameter("COMP_ADNAME", dr.COMP_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("COMP_CSTMP", dr.COMP_CSTMP, OracleType.DateTime)
                oExecute.addParameter("COMP_LUAD", dr.COMP_LUAD, OracleType.NVarChar)
                oExecute.addParameter("COMP_LUADNAME", dr.COMP_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("COMP_LUSTMP", dr.COMP_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("COMP_LABORCOST", dr.COMP_LABORCOST, OracleType.Int16)
                oExecute.addParameter("COMP_APPROVALAMOUNT", dr.COMP_APPROVALAMOUNT, OracleType.Int16)
                oExecute.addParameter("COMP_LOWESTDISCOUNT", dr.COMP_LOWESTDISCOUNT, OracleType.Int16)

                oExecute.Command("COMPANY", Execute.eumCommandType.AddNew)
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
    ''' Company-修改
    ''' </summary>
    ''' <param name="dtCompany">傳入dtCompany</param>
    ''' <param name="oldComp_No">舊的公司代碼</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtCompany As CompanyDTO.CompanyDataTable, ByVal oldComp_No As String)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            For i = 0 To dtCompany.Rows.Count - 1
                Dim dr As CompanyDTO.CompanyRow = dtCompany.Rows(i)

                oExecute.addParameter("COMP_NO", dr.COMP_NO.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("COMP_NAME", dr.COMP_NAME.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("COMP_TEL", dr.COMP_TEL.ToString().Trim(), OracleType.VarChar)
                oExecute.addParameter("COMP_ADDRESS", dr.COMP_ADDRESS.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("COMP_COUNTRYID", dr.COMP_COUNTRYID, OracleType.VarChar)
                oExecute.addParameter("COMP_CURRENCYCODE", dr.COMP_CURRENCYCODE, OracleType.VarChar)
                oExecute.addParameter("COMP_VISIBLE", dr.COMP_VISIBLE.ToString().Trim(), OracleType.Int16)

                oExecute.addParameter("COMP_EXPRESSCO", dr.COMP_EXPRESSCO.ToString().Trim(), OracleType.NVarChar)
                oExecute.addParameter("COMP_EXPRESSURL", dr.COMP_EXPRESSURL, OracleType.VarChar)
                oExecute.addParameter("COMP_REMARK", dr.COMP_REMARK, OracleType.NVarChar)
                oExecute.addParameter("COMP_STOCKMANAGER", dr.COMP_STOCKMANAGER, OracleType.NVarChar)

                oExecute.addParameter("COMP_LUAD", dr.COMP_LUAD, OracleType.NVarChar)
                oExecute.addParameter("COMP_LUADNAME", dr.COMP_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("COMP_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addParameter("COMP_LABORCOST", dr.COMP_LABORCOST, OracleType.Double)
                oExecute.addParameter("COMP_APPROVALAMOUNT", dr.COMP_APPROVALAMOUNT, OracleType.Double)
                oExecute.addParameter("COMP_LOWESTDISCOUNT", dr.COMP_LOWESTDISCOUNT, OracleType.Double)

                oExecute.addWHERE("COMP_NO", oldComp_No.Trim(), OracleType.VarChar)
                oExecute.Command("COMPANY", Execute.eumCommandType.UPDATE)
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

End Class

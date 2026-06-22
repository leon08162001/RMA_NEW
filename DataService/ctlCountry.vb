Imports System.Data.OracleClient
Imports ICAT_OracleDAO

Public Class ctlCountry

    ''' <summary>
    ''' 取得國家資料
    ''' </summary>
    ''' <param name="OrderBY">排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 CountryDataTable</remarks>
    Public Function QueryAll(Optional ByVal OrderBY As String = "") As CountryDTO.CountryDataTable
        Dim dt As New DataTable
        Dim dtFAQClass As New CountryDTO.CountryDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " COUNTRY_ID asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            Dim sSQL As String = "SELECT * FROM COUNTRY " & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtFAQClass)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtFAQClass
    End Function

    ''' <summary>
    ''' 取得國家資料
    ''' </summary>
    ''' <param name="Visible">是否顯示(1:顯示 , 0:不顯示, "":全部)</param>
    ''' <param name="OrderBY">排序</param>
    ''' <returns></returns>
    ''' <remarks>傳回 CountryDataTable</remarks>
    Public Function Query(ByVal Visible As String, Optional ByVal OrderBY As String = "") As CountryDTO.CountryDataTable
        Dim dt As New DataTable
        Dim dtFAQClass As New CountryDTO.CountryDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try
            If OrderBY.Trim = "" Then
                OrderBY = " COUNTRY_ID asc"
            End If
            OrderBY = " ORDER BY " & OrderBY

            If Visible.ToString().Trim() <> "" Then
                oQuery.addWHERE("COUNTRY_VISIBLE", ":COUNTRY_VISIBLE", Visible, OracleType.Int16)
                sCondition = sCondition & " AND COUNTRY_VISIBLE=:COUNTRY_VISIBLE"
            End If

            Dim sSQL As String = "SELECT * FROM COUNTRY WHERE 1=1 " & sCondition & OrderBY

            dt = oQuery.ExecuteDT(sSQL)
            Common.TransferDataTable(dt, dtFAQClass)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtFAQClass
    End Function

    ''' <summary>
    ''' 取得國家名稱
    ''' </summary>
    ''' <param name="COUNTRY_ID">國家代碼</param>
    ''' <returns></returns>
    ''' <remarks>傳回 CountryDataTable</remarks>
    Public Function getCountryName(ByVal COUNTRY_ID As String) As String
        Dim retval As String = ""
        Dim dt As New DataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim sCondition As String = ""

        oConn.Open()
        Try

            oQuery.addWHERE("COUNTRY_ID", ":COUNTRY_ID", COUNTRY_ID, OracleType.VarChar)
            sCondition = sCondition & " AND COUNTRY_ID=:COUNTRY_ID"

            Dim sSQL As String = "SELECT * FROM COUNTRY WHERE 1=1 " & sCondition

            dt = oQuery.ExecuteDT(sSQL)
            If dt.Rows.Count > 0 Then
                retval = dt.Rows(0)("COUNTRY_NAME").ToString.Trim()
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
    ''' 國家資料 - 修改
    ''' </summary>
    ''' <param name="dtCountry">傳入要修改國家的資料</param>
    ''' <remarks></remarks>
    Public Sub SaveEdit(ByVal dtCountry As CountryDTO.CountryDataTable)
        Dim i As Integer = 0
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()
        Try
            oConn.BeginTransaction()

            For i = 0 To dtCountry.Rows.Count - 1
                Dim dr As CountryDTO.CountryRow = dtCountry.Rows(i)

                oExecute.addParameter("COUNTRY_NAME", dr.COUNTRY_NAME.ToString().Trim(), OracleType.VarChar)
                oExecute.addParameter("COUNTRY_VISIBLE", dr.COUNTRY_VISIBLE.ToString().Trim(), OracleType.Int16)

                'oExecute.addParameter("COUNTRY_AD", dr.COUNTRY_AD.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("COUNTRY_ADNAME", dr.COUNTRY_ADNAME.ToString().Trim(), OracleType.VarChar)
                'oExecute.addParameter("COUNTRY_CSTMP", dr.COUNTRY_CSTMP, OracleType.DateTime)

                oExecute.addParameter("COUNTRY_LUAD", dr.COUNTRY_LUAD, OracleType.NVarChar)
                oExecute.addParameter("COUNTRY_LUADNAME", dr.COUNTRY_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("COUNTRY_LUSTMP", dr.COUNTRY_LUSTMP, OracleType.DateTime)

                oExecute.addWHERE("COUNTRY_ID", dr.COUNTRY_ID.ToString().Trim(), OracleType.VarChar)
                oExecute.Command("COUNTRY", Execute.eumCommandType.UPDATE)
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

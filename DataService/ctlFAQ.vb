Imports System.Data.OracleClient
Imports DataService.FaqDTO
Imports ICAT_OracleDAO

Public Class ctlFAQ

#Region "Class:FAQClass:FAQ大類"
    Public Class FAQClass

        ''' <summary>
        ''' 取得FAQ大類資料
        ''' </summary>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks>傳回 FAQClassDataTable</remarks>
        Public Function QueryAll(Optional ByVal OrderBY As String = "") As FAQClassDataTable
            Dim dt As New DataTable
            Dim dtFAQClass As New FAQClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQC_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAQC_MARK", ":FAQC_MARK", "0", OracleType.Int16)

                Dim sSQL As String = "SELECT * FROM FAQCLASS " &
                        " WHERE FAQC_MARK=:FAQC_MARK " & OrderBY

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
        ''' 取得有效的FAQ大類資料
        ''' </summary>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks>傳回 FAQClassDataTable</remarks>
        Public Function QueryByEffective(Optional ByVal OrderBY As String = "") As FAQClassDataTable
            Dim dt As New DataTable
            Dim dtFAQClass As New FAQClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQC_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAQC_VISIBLE", ":FAQC_VISIBLE", "1", OracleType.Int16)
                oQuery.addWHERE("FAQC_MARK", ":FAQC_MARK", "0", OracleType.Int16)

                Dim sSQL As String = "SELECT * FROM FAQCLASS " &
                        " WHERE FAQC_VISIBLE=:FAQC_VISIBLE AND FAQC_MARK=:FAQC_MARK " & OrderBY

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
        ''' FAQ大類-新增
        ''' </summary>
        ''' <param name="dtFAQCLASS"></param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtFAQCLASS As FaqDTO.FAQClassDataTable)
            Dim i As Integer = 0
            Dim oGuid As Guid = Guid.NewGuid
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                For i = 0 To dtFAQCLASS.Rows.Count - 1
                    Dim dr As FaqDTO.FAQClassRow = dtFAQCLASS.Rows(i)
                    Dim sGUID As String = oGuid.ToString

                    oExecute.addParameter("FAQC_ID", sGUID, OracleType.VarChar)
                    oExecute.addParameter("FAQC_CLASSNAME", dr.FAQC_CLASSNAME, OracleType.VarChar)
                    oExecute.addParameter("FAQC_TYPE", dr.FAQC_TYPE, OracleType.Int16)
                    oExecute.addParameter("FAQC_VISIBLE", dr.FAQC_VISIBLE, OracleType.Int16)

                    oExecute.addParameter("FAQC_AD", dr.FAQC_AD, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_ADNAME", dr.FAQC_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_CSTMP", dr.FAQC_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("FAQC_LUAD", dr.FAQC_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_LUADNAME", dr.FAQC_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_LUSTMP", dr.FAQC_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("FAQC_MARK", dr.FAQC_MARK, OracleType.Int16)

                    oExecute.Command("FAQCLASS", Execute.eumCommandType.AddNew)
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
        ''' FAQ大類-修改
        ''' </summary>
        ''' <param name="dtFAQCLASS"></param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtFAQCLASS As FaqDTO.FAQClassDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                For i = 0 To dtFAQCLASS.Rows.Count - 1
                    Dim dr As FaqDTO.FAQClassRow = dtFAQCLASS.Rows(i)

                    oExecute.addParameter("FAQC_CLASSNAME", dr.FAQC_CLASSNAME, OracleType.VarChar)
                    oExecute.addParameter("FAQC_VISIBLE", dr.FAQC_VISIBLE, OracleType.Int16)
                    oExecute.addParameter("FAQC_TYPE", dr.FAQC_TYPE, OracleType.Int16)

                    oExecute.addParameter("FAQC_LUAD", dr.FAQC_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_LUADNAME", dr.FAQC_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQC_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("FAQC_ID", dr.FAQC_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("FAQCLASS", Execute.eumCommandType.UPDATE)
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
#End Region

#Region "Class:FAQSubClass:FAQ小類"
    Public Class FAQSubClass

        ''' <summary>
        ''' 取得FAQ大類所屬的小類資料
        ''' </summary>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks>傳回 FAQSubClassDataTable</remarks>
        Public Function QueryBYClass(ByVal FAQC_ID As String, Optional ByVal OrderBY As String = "") As FAQSubClassDataTable
            Dim dt As New DataTable
            Dim dtFAQSubClass As New FAQSubClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQSC_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAQSC_VISIBLE", ":FAQSC_VISIBLE", "1", OracleType.Int16)
                oQuery.addWHERE("FAQSC_MARK", ":FAQSC_MARK", "0", OracleType.Int16)
                oQuery.addWHERE("FAQSC_FAQCID", ":FAQSC_FAQCID", FAQC_ID.Trim(), OracleType.VarChar)

                Dim sSQL As String = "SELECT * FROM FAQSUBCLASS " &
                        " WHERE FAQSC_VISIBLE=:FAQSC_VISIBLE AND FAQSC_MARK=:FAQSC_MARK AND FAQSC_FAQCID=:FAQSC_FAQCID " & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFAQSubClass)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFAQSubClass
        End Function

        ''' <summary>
        ''' 取得全部小類資料
        ''' </summary>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns></returns>
        ''' <remarks>傳回 FAQSubClassDataTable</remarks>
        Public Function QueryAll(Optional ByVal OrderBY As String = "") As vwFAQClassDataTable
            Dim dt As New DataTable
            Dim dtFAQSubClass As New vwFAQClassDataTable
            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQSC_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAQC_VISIBLE", ":FAQC_VISIBLE", "1", OracleType.Int16)
                oQuery.addWHERE("FAQC_MARK", ":FAQC_MARK", "0", OracleType.Int16)
                oQuery.addWHERE("FAQSC_MARK", ":FAQSC_MARK", "0", OracleType.Int16)

                Dim sSQL As String = "SELECT * FROM vwFAQClass " &
                        " WHERE FAQC_VISIBLE=:FAQC_VISIBLE AND FAQSC_MARK=:FAQSC_MARK AND FAQC_MARK=:FAQC_MARK" & OrderBY

                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFAQSubClass)

            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFAQSubClass
        End Function



        ''' <summary>
        ''' FAQ小類-新增
        ''' </summary>
        ''' <param name="dtFAQSubClass"></param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtFAQSubClass As FaqDTO.FAQSubClassDataTable)
            Dim i As Integer = 0
            Dim oGuid As Guid = Guid.NewGuid


            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                For i = 0 To dtFAQSubClass.Rows.Count - 1
                    Dim dr As FaqDTO.FAQSubClassRow = dtFAQSubClass.Rows(i)
                    Dim sGUID As String = oGuid.ToString

                    oExecute.addParameter("FAQSC_ID", sGUID, OracleType.VarChar)
                    oExecute.addParameter("FAQSC_FAQCID", dr.FAQSC_FAQCID, OracleType.VarChar)
                    oExecute.addParameter("FAQSC_CLASSNAME", dr.FAQSC_CLASSNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_VISIBLE", dr.FAQSC_VISIBLE, OracleType.Int16)

                    oExecute.addParameter("FAQSC_AD", dr.FAQSC_AD, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_ADNAME", dr.FAQSC_ADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_CSTMP", dr.FAQSC_CSTMP, OracleType.DateTime)
                    oExecute.addParameter("FAQSC_LUAD", dr.FAQSC_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_LUADNAME", dr.FAQSC_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_LUSTMP", dr.FAQSC_LUSTMP, OracleType.DateTime)
                    oExecute.addParameter("FAQSC_MARK", dr.FAQSC_MARK, OracleType.Int16)

                    oExecute.Command("FAQSUBCLASS", Execute.eumCommandType.AddNew)
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
        ''' FAQ小類-修改
        ''' </summary>
        ''' <param name="dtFAQSubClass"></param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtFAQSubClass As FaqDTO.FAQSubClassDataTable)
            Dim i As Integer = 0
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                For i = 0 To dtFAQSubClass.Rows.Count - 1
                    Dim dr As FaqDTO.FAQSubClassRow = dtFAQSubClass.Rows(i)

                    oExecute.addParameter("FAQSC_FAQCID", dr.FAQSC_FAQCID, OracleType.VarChar)
                    oExecute.addParameter("FAQSC_CLASSNAME", dr.FAQSC_CLASSNAME, OracleType.VarChar)
                    oExecute.addParameter("FAQSC_VISIBLE", dr.FAQSC_VISIBLE, OracleType.Int16)

                    oExecute.addParameter("FAQSC_LUAD", dr.FAQSC_LUAD, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_LUADNAME", dr.FAQSC_LUADNAME, OracleType.NVarChar)
                    oExecute.addParameter("FAQSC_LUSTMP", Date.Now, OracleType.DateTime)

                    oExecute.addWHERE("FAQSC_ID", dr.FAQSC_ID.ToString().Trim(), OracleType.VarChar)
                    oExecute.Command("FAQSUBCLASS", Execute.eumCommandType.UPDATE)
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
#End Region

#Region "Class:FAQ"
    Public Class FAQ

        ''' <summary>
        ''' 取得FAQ資料
        ''' </summary>
        ''' <param name="FAQC_ID">FAQCLASS.FAQC_ID</param>
        ''' <param name="FAQSC_ID">FAQSUBCLASS.FAQSC_ID</param>
        ''' <param name="Question">問題 Display</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 FAQDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryDisplay(ByVal FAQC_ID As String, ByVal FAQSC_ID As String, ByVal Question As String, Optional ByVal OrderBY As String = "") As FaqDTO.FAQDataTable
            Dim sCondition As String = ""
            Dim dtFAQ As New FaqDTO.FAQDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQ_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If FAQC_ID.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("FAQ_FAQCID", ":FAQ_FAQCID", FAQC_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND FAQ_FAQCID=:FAQ_FAQCID"
                End If

                If FAQSC_ID.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("FAQ_FAQSCID", ":FAQ_FAQSCID", FAQSC_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND FAQ_FAQSCID=:FAQ_FAQSCID"
                End If

                If Question.ToString().Trim() <> "" Then
                    Question = "%" & Question.ToLower().Trim() & "%"
                    oQuery.addWHERE("FAQ_QUESTION", ":FAQ_QUESTION", Question, OracleType.VarChar)
                    sCondition = " AND lower(FAQ_QUESTION) like :FAQ_QUESTION"
                End If


                Dim sSQL As String = "SELECT * FROM FAQ WHERE   FAQ_VISIBLE = 1   and FAQ_MARK = 0 " & sCondition & OrderBY
                dt = oQuery.ExecuteDT(sSQL)

                'Dim myView As DataView = dt.DefaultView
                'myView.RowFilter = "FAQ_MARK=0"
                'If myView.Count > 0 Then


                'End If

                Common.TransferDataTable(dt, dtFAQ)


            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFAQ
        End Function


        ''' <summary>
        ''' 取得FAQ資料
        ''' </summary>
        ''' <param name="FAQC_ID">FAQCLASS.FAQC_ID</param>
        ''' <param name="FAQSC_ID">FAQSUBCLASS.FAQSC_ID</param>
        ''' <param name="Question">問題 KeyWord</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 FAQDataTable</returns>
        ''' <remarks></remarks>
        Public Function Query(ByVal FAQC_ID As String, ByVal FAQSC_ID As String, ByVal Question As String, Optional ByVal OrderBY As String = "") As FaqDTO.FAQDataTable
            Dim sCondition As String = ""
            Dim dtFAQ As New FaqDTO.FAQDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()
            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQ_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                If FAQC_ID.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("FAQ_FAQCID", ":FAQ_FAQCID", FAQC_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND FAQ_FAQCID=:FAQ_FAQCID"
                End If

                If FAQSC_ID.ToString().Trim() <> "-1" Then
                    oQuery.addWHERE("FAQ_FAQSCID", ":FAQ_FAQSCID", FAQSC_ID, OracleType.VarChar)
                    sCondition = sCondition & " AND FAQ_FAQSCID=:FAQ_FAQSCID"
                End If

                If Question.ToString().Trim() <> "" Then
                    Question = "%" & Question.ToLower().Trim() & "%"
                    oQuery.addWHERE("FAQ_QUESTION", ":FAQ_QUESTION", Question, OracleType.VarChar)
                    sCondition = " AND lower(FAQ_QUESTION) like :FAQ_QUESTION"
                End If

                oQuery.addWHERE("FAQ_MARK", ":FAQ_MARK", "0", OracleType.Int16)

                Dim sSQL As String = "SELECT * FROM FAQ WHERE FAQ_MARK=:FAQ_MARK" & sCondition & OrderBY
                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFAQ)


            Catch ex As Exception
                Throw ex
            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFAQ
        End Function

        ''' <summary>
        ''' 取得FAQ資料
        ''' </summary>
        ''' <param name="FAQ_ID">傳入FAQ_ID</param>
        ''' <param name="OrderBY">定義排序</param>
        ''' <returns>傳回 FAQDataTable</returns>
        ''' <remarks></remarks>
        Public Function QueryByPrimaryKey(ByVal FAQ_ID As String, Optional ByVal OrderBY As String = "") As FaqDTO.FAQDataTable
            Dim dtFAQ As New FaqDTO.FAQDataTable
            Dim dt As New DataTable

            Dim oConn As New Connection
            Dim oQuery As New ICAT_OracleDAO.Query(oConn)

            oConn.Open()

            Try
                If OrderBY.Trim = "" Then
                    OrderBY = " FAQ_CSTMP desc"
                End If
                OrderBY = " ORDER BY " & OrderBY

                oQuery.addWHERE("FAQ_ID", ":FAQ_ID", FAQ_ID, OracleType.VarChar)

                Dim sSQL As String = "SELECT * FROM FAQ WHERE FAQ_ID=:FAQ_ID " & OrderBY
                dt = oQuery.ExecuteDT(sSQL)
                Common.TransferDataTable(dt, dtFAQ)

            Catch ex As Exception
                Throw ex

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try

            Return dtFAQ
        End Function



        ''' <summary>
        ''' FAQ-新增
        ''' </summary>
        ''' <param name="dtFAQ"></param>
        ''' <remarks></remarks>
        Public Sub SaveAdd(ByVal dtFAQ As FaqDTO.FAQDataTable)
            Dim oGuid As Guid = Guid.NewGuid

            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()
                Dim dr As FaqDTO.FAQRow = dtFAQ.Rows(0)

                oExecute.addParameter("FAQ_ID", oGuid.ToString, OracleType.VarChar)
                oExecute.addParameter("FAQ_FAQCID", dr.FAQ_FAQCID, OracleType.VarChar)
                oExecute.addParameter("FAQ_FAQSCID", dr.FAQ_FAQSCID, OracleType.VarChar)
                oExecute.addParameter("FAQ_QUESTION", dr.FAQ_QUESTION, OracleType.VarChar)
                oExecute.addParameter("FAQ_ANSWER", dr.FAQ_ANSWER, OracleType.VarChar)
                oExecute.addParameter("FAQ_ISSUEDATE", dr.FAQ_ISSUEDATE, OracleType.DateTime)
                oExecute.addParameter("FAQ_VISIBLE", dr.FAQ_VISIBLE, OracleType.Int16)

                oExecute.addParameter("FAQ_AD", dr.FAQ_AD, OracleType.NVarChar)
                oExecute.addParameter("FAQ_ADNAME", dr.FAQ_ADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAQ_CSTMP", dr.FAQ_CSTMP, OracleType.DateTime)
                oExecute.addParameter("FAQ_LUAD", dr.FAQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUADNAME", dr.FAQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUSTMP", dr.FAQ_LUSTMP, OracleType.DateTime)
                oExecute.addParameter("FAQ_MARK", dr.FAQ_MARK, OracleType.Int16)

                oExecute.Command("FAQ", Execute.eumCommandType.AddNew)

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
        ''' FAQ-修改
        ''' </summary>
        ''' <param name="dtFAQ"></param>
        ''' <remarks></remarks>
        Public Sub SaveEdit(ByVal dtFAQ As FaqDTO.FAQDataTable)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()
            Try
                oConn.BeginTransaction()
                Dim dr As FaqDTO.FAQRow = dtFAQ.Rows(0)

                oExecute.addParameter("FAQ_FAQCID", dr.FAQ_FAQCID, OracleType.VarChar)
                oExecute.addParameter("FAQ_FAQSCID", dr.FAQ_FAQSCID, OracleType.VarChar)
                oExecute.addParameter("FAQ_QUESTION", dr.FAQ_QUESTION, OracleType.VarChar)
                oExecute.addParameter("FAQ_ANSWER", dr.FAQ_ANSWER, OracleType.VarChar)
                oExecute.addParameter("FAQ_ISSUEDATE", dr.FAQ_ISSUEDATE, OracleType.DateTime)
                oExecute.addParameter("FAQ_VISIBLE", dr.FAQ_VISIBLE, OracleType.Int16)

                oExecute.addParameter("FAQ_LUAD", dr.FAQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUADNAME", dr.FAQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUSTMP", Date.Now, OracleType.DateTime)

                oExecute.addWHERE("FAQ_ID", dr.FAQ_ID, OracleType.NVarChar)

                oExecute.Command("FAQ", Execute.eumCommandType.UPDATE)
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
        ''' FAQ-刪除
        ''' </summary>
        ''' <param name="FAQ_ID">Key</param>
        ''' <param name="FAQ_LUAD">最後修改人帳號</param>
        ''' <param name="FAQ_LUADNAME">最後修改人姓名</param>
        ''' <remarks></remarks>
        Public Sub Delete(ByVal FAQ_ID As String, ByVal FAQ_LUAD As String, ByVal FAQ_LUADNAME As String)
            Dim oConn As New Connection
            Dim oExecute As New Execute(oConn)

            oConn.Open()

            Try
                oConn.BeginTransaction()

                oExecute.addParameter("FAQ_LUAD", FAQ_LUAD, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUADNAME", FAQ_LUADNAME, OracleType.NVarChar)
                oExecute.addParameter("FAQ_LUSTMP", Date.Now, OracleType.DateTime)
                oExecute.addParameter("FAQ_MARK", 1, OracleType.Int16)

                oExecute.addWHERE("FAQ_ID", FAQ_ID.ToString().Trim(), OracleType.NVarChar)
                oExecute.Command("FAQ", Execute.eumCommandType.UPDATE)
                oConn.Commit()

            Catch ex As Exception
                Throw ex
                oConn.Rollback()

            Finally
                oConn.Close()
                oConn.Dispose()
            End Try
        End Sub

    End Class
#End Region

End Class

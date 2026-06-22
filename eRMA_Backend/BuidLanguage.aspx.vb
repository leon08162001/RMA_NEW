Imports System.Data
Imports System.Data.OracleClient
Imports System.IO
Imports System.Xml
Imports DefLanguage.LanguageDTO
Imports ICAT_OracleDAO


Partial Class BuidLanguage
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim OrderBY As String = " ORDER BY FunctionName,TYPE , Key"

        Dim dt As New DataTable
        Dim dtLanguage As New LANGUAGEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            Dim sSQL As String = "SELECT * FROM LANGUAGE " & OrderBY
            dt = oQuery.ExecuteDT(sSQL)

            Dim oDataSet As New DataSet
            oDataSet.Tables.Add(dt)

            Dim xmlDoc As XmlDataDocument = New XmlDataDocument(dt.DataSet)
            Dim sInternalXML As String = xmlDoc.OuterXml

            Dim sXmlFile As String = Server.MapPath("bin\EnglishTMP.xml")
            Dim ofs As New StreamWriter(sXmlFile, False)
            ofs.WriteLine(sInternalXML)
            ofs.Close()

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim i As Integer = 0

        Dim sSQL As String = ""
        Dim oConn As New Connection
        Dim oExecute As New Execute(oConn)

        oConn.Open()

        Try
            Dim dsLanguage As New DataSet
            dsLanguage.ReadXml(Me.UI_XML.Text)

            Dim dtLanguage As New DataTable
            dtLanguage = dsLanguage.Tables(0)


            If dtLanguage.Rows.Count > 0 Then
                oConn.BeginTransaction()

                sSQL = "DELETE FROM LANGUAGE"
                oExecute.Command("LANGUAGE", Execute.eumCommandType.Delete)

                For i = 0 To dtLanguage.Rows.Count - 1
                    oExecute.addParameter("FUNCTIONNAME", dtLanguage.Rows(i)("FUNCTIONNAME").ToString().Trim(), OracleType.VarChar)
                    oExecute.addParameter("KEY", dtLanguage.Rows(i)("KEY").ToString().Trim(), OracleType.VarChar)
                    oExecute.addParameter("TYPE", dtLanguage.Rows(i)("TYPE").ToString().Trim(), OracleType.VarChar)

                    Dim VALUE As String = " "
                    If dtLanguage.Rows(i)("VALUE").ToString().Trim() <> "" Then
                        VALUE = dtLanguage.Rows(i)("VALUE").ToString().Trim()
                    End If
                    oExecute.addParameter("VALUE", VALUE, OracleType.VarChar)

                    oExecute.Command("LANGUAGE", Execute.eumCommandType.AddNew)
                Next

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

End Class

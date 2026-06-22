Imports DefLanguage.LanguageDTO
Imports ICAT_OracleDAO


Public Class ctlLanguage
    Inherits System.Web.UI.Page

    Enum eumType As Integer
        Tag = 1
        Validator = 2
        Command = 3
        Processing = 4
        Status = 5
        Mail = 6
        Word = 7
    End Enum

    ''' <summary>
    ''' 取得各屬性的語系定義的對照表
    ''' </summary>
    ''' <param name="FunctionName">功能</param>
    ''' <param name="Key">Key</param>
    ''' <param name="eumType">型態定義</param>
    ''' <returns>回傳訊息值</returns>
    ''' <remarks></remarks>
    Public Function getText(ByVal FunctionName As String, ByVal Key As String, ByVal eumType As eumType) As String
        Dim retval As String = ""
        Dim sType As String = ""

        Dim dtLanguage As New LANGUAGEDataTable

        Try
            'Session("_LanguageID")
            If IsNothing(Session("_dtLanguage")) = True Then
                'dtLanguage = QueryAll()
                dtLanguage = LoadXML(Session("_LanguageID").ToString().Trim())
                Session("_dtLanguage") = dtLanguage
            Else
                dtLanguage = Session("_dtLanguage")
            End If

            Dim dvLanguage As DataView = dtLanguage.DefaultView

            Select Case eumType
                Case ctlLanguage.eumType.Tag
                    sType = "Tag"

                Case ctlLanguage.eumType.Validator
                    sType = "Validator"

                Case ctlLanguage.eumType.Command
                    sType = "Command"

                Case ctlLanguage.eumType.Processing
                    sType = "Processing"

                Case ctlLanguage.eumType.Status
                    sType = "Status"

                Case ctlLanguage.eumType.Mail
                    sType = "Mail"
                Case ctlLanguage.eumType.Word
                    sType = "Word"
            End Select

            dvLanguage.RowFilter = "FunctionName='" & FunctionName.Trim() & "' AND Key='" & Key.Trim() & "' AND TYPE='" & sType & "'"
            If dvLanguage.Count > 0 Then
                'Throw New ArgumentException("")
                retval = dvLanguage(0)("VALUE").ToString().Trim()
            End If


        Catch ex As Exception
            Throw ex
        Finally

        End Try

        Return retval
    End Function

    Public Function getMailText(ByVal FunctionName As String, ByVal Key As String, ByVal eumType As eumType, ByVal sLanguageID As String) As String
        Dim retval As String = ""
        Dim sType As String = ""

        Dim dtLanguage As New LANGUAGEDataTable

        If sLanguageID = "" Then
            sLanguageID = "001"
        End If

        Try
            If (IsNothing(Session("_dtMailLanguage")) = True OrElse IsNothing(Session("_MailLanguageID")) = True) Then
                'dtLanguage = QueryAll()
                dtLanguage = LoadXML(sLanguageID.Trim())
                Session("_dtMailLanguage") = dtLanguage
                Session("_MailLanguageID") = sLanguageID
            Else
                If Session("_MailLanguageID") <> sLanguageID Then
                    dtLanguage = LoadXML(sLanguageID.Trim())
                    Session("_dtMailLanguage") = dtLanguage
                    Session("_MailLanguageID") = sLanguageID
                Else
                    dtLanguage = Session("_dtMailLanguage")
                End If

            End If

            Dim dvLanguage As DataView = dtLanguage.DefaultView

            Select Case eumType
                Case ctlLanguage.eumType.Tag
                    sType = "Tag"

                Case ctlLanguage.eumType.Validator
                    sType = "Validator"

                Case ctlLanguage.eumType.Command
                    sType = "Command"

                Case ctlLanguage.eumType.Processing
                    sType = "Processing"

                Case ctlLanguage.eumType.Status
                    sType = "Status"

                Case ctlLanguage.eumType.Mail
                    sType = "Mail"
                Case ctlLanguage.eumType.Word
                    sType = "Word"
            End Select

            dvLanguage.RowFilter = "FunctionName='" & FunctionName.Trim() & "' AND Key='" & Key.Trim() & "' AND TYPE='" & sType & "'"
            If dvLanguage.Count > 0 Then
                'Throw New ArgumentException("")
                retval = dvLanguage(0)("VALUE").ToString().Trim()
            End If


        Catch ex As Exception
            Throw ex
        Finally

        End Try

        Return retval
    End Function

    Public Sub reLoad(ByVal LanguageID As String)
        Dim dtLanguage As New LANGUAGEDataTable
        'dtLanguage = QueryAll()
        dtLanguage = LoadXML(LanguageID)
        Session("_dtLanguage") = dtLanguage

    End Sub

    Private Function QueryAll() As LANGUAGEDataTable
        Dim retval As String = ""
        Dim OrderBY As String = " ORDER BY FunctionName,TYPE , Key"

        Dim dt As New DataTable
        Dim dtLanguage As New LANGUAGEDataTable
        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try

            Dim sSQL As String = "SELECT * FROM LANGUAGE " & OrderBY
            dt = oQuery.ExecuteDT(sSQL)

            'Dim oDataSet As New DataSet
            'oDataSet.Tables.Add(dt)

            'Dim xmlDoc As XmlDataDocument = New XmlDataDocument(dt.DataSet)
            'Dim sInternalXML As String = xmlDoc.OuterXml

            'Dim sXmlFile As String = Server.MapPath("bin\English.xml")
            'Dim ofs As New StreamWriter(sXmlFile, False)
            'ofs.WriteLine(sInternalXML)
            'ofs.Close()

            TransferDataTable(dt, dtLanguage)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtLanguage
    End Function

    Public Function LoadXML(ByVal LanguageID As String) As LANGUAGEDataTable
        Dim oDataSet As New DataSet
        Dim dt As New DataTable
        Dim dtDefLanguage As DataTable
        Dim dtLanguage As New LANGUAGEDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)

        oConn.Open()
        Try
            Dim sSQL As String = "SELECT * FROM DEFLANGUAGE where DFL_NO = '" & LanguageID & "'"
            dtDefLanguage = oQuery.ExecuteDT(sSQL)
            Dim FileName As String = dtDefLanguage.Rows(0)("FILENAME").ToString.Trim()

            Dim XMLFile As String = "bin\" & FileName

            oDataSet.ReadXml(Server.MapPath(XMLFile))
            dt = oDataSet.Tables(0)
            TransferDataTable(dt, dtLanguage)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtLanguage
    End Function

    ''' <summary>
    ''' 取得 語系定義檔 資料
    ''' </summary>
    ''' <remarks>傳回 LanguageDataTable</remarks>
    Public Function QueryByDefLanguage() As LanguageDTO.DEFLANGUAGEDataTable
        Dim dtLanguage As New LanguageDTO.DEFLANGUAGEDataTable

        Dim oConn As New Connection
        Dim oQuery As New ICAT_OracleDAO.Query(oConn)
        Dim dt As New DataTable
        Dim sSQL As String = ""
        Dim sCondition As String = ""

        oConn.Open()
        Try
            sSQL = "SELECT * FROM DEFLANGUAGE WHERE DFL_ISOPEN='1'"

            dt = oQuery.ExecuteDT(sSQL)
            TransferDataTable(dt, dtLanguage)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dtLanguage
    End Function

    ''' <summary>
    ''' 由 DataTable 轉換到 另一各 DTO DataTable 
    ''' </summary>
    ''' <param name="dtSource">來源資料表</param>
    ''' <param name="dtTarget">目的地資料表</param>
    ''' <remarks></remarks>
    Private Sub TransferDataTable(ByVal dtSource As DataTable, ByRef dtTarget As DataTable)
        Dim i As Integer = 0
        Dim j As Integer = 0

        For i = 0 To dtSource.Rows.Count - 1
            Dim dr As DataRow = dtTarget.NewRow

            For j = 0 To dtSource.Columns.Count - 1
                Dim sColumnName As String = dtSource.Columns(j).ColumnName
                dr(sColumnName) = dtSource.Rows(i)(sColumnName)
            Next
            dtTarget.Rows.Add(dr)

        Next


    End Sub

End Class

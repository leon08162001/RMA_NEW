Imports DefLanguage

Partial Class FileUpload
    Inherits System.Web.UI.Page
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage
    Dim _Crypto As New SecurityCrypt.Crypto

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0

        If Me.IsPostBack = False Then
            Me.butOK.Text = _oLanguage.getText("Common", "037", ctlLanguage.eumType.Command)
            Me.rfvhtmlFullFile.ErrorMessage = _oLanguage.getText("RMA", "121", ctlLanguage.eumType.Validator)

            Dim sKey As String = Request.QueryString("ID")
            sKey = _Crypto.Decrypt(sKey, "")

            Dim arrKey() As String = sKey.Split("&")
            For i = 0 To arrKey.Length - 1
                Dim arrUniqueKey() As String = arrKey(i).Split("=")
                If arrUniqueKey(0).ToString().ToLower() = "File".ToLower() Then
                    Me.html_File.Text = arrUniqueKey(1).ToString()
                End If

                If arrUniqueKey(0).ToString().ToLower() = "FullFile".ToLower() Then
                    Me.html_FullFile.Text = arrUniqueKey(1).ToString()
                End If

                If arrUniqueKey(0).ToString().ToLower() = "FilePath".ToLower() Then
                    Me.html_FilePath.Text = arrUniqueKey(1).ToString()
                End If
            Next
        End If


    End Sub

    Protected Sub butOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butOK.Click
        Dim sFilePath As String = ConfigurationManager.AppSettings(Me.html_FilePath.Text)

        Dim txtFileName As String = Me.html_FileUpload.FileName

        '****** 取得檔名 **********
        Dim FileSplit() As String = Split(txtFileName, "\")
        Dim FileName As String = FileSplit(FileSplit.Length - 1)

        '****** 取得副檔名 **********
        Dim auxFileSplit() As String = Split(FileName, ".")
        Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
        Dim sFileNameChange As String = ""
        Dim oCommon As New Common
        sFileNameChange = oCommon.GetRandomizeNum()
        sFileNameChange = sFileNameChange & "." & auxFileName

        '***** 檔案(原檔名,亂數檔名) *****
        Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

        Me.html_FileUpload.MoveTo(sFilePath & sFileNameChange, Brettle.Web.NeatUpload.MoveToOptions.Overwrite)

        Dim sScript As String = "<script type=""text/javascript"">" & vbCrLf
        sScript = sScript & "var html_File = window.parent.document.getElementById(""" & Me.html_File.Text & """);" & vbCrLf
        sScript = sScript & "var html_FullFile = window.parent.document.getElementById(""" & Me.html_FullFile.Text & """);" & vbCrLf

        sScript = sScript & "html_File.value=""" & FileName & """" & vbCrLf
        sScript = sScript & "html_FullFile.value=""" & sFullFileName & """" & vbCrLf
        sScript = sScript & "</script>" & vbCrLf

        Me.lblScript.Text = sScript





        'Dim sErrMsg As String = ""
        'Dim sFilePath As String = ConfigurationManager.AppSettings(Me.html_FilePath.Text)

        'If Me.html_FileUpload.HasFile = True Then
        '    If Me.html_FileUpload.FileContent.Length <> 0 Then
        '        Dim txtFileName As String = Me.html_FileUpload.FileName

        '        '****** 取得檔名 **********
        '        Dim FileSplit() As String = Split(txtFileName, "\")
        '        Dim FileName As String = FileSplit(FileSplit.Length - 1)

        '        '****** 取得副檔名 **********
        '        Dim auxFileSplit() As String = Split(FileName, ".")
        '        Dim auxFileName As String = auxFileSplit(auxFileSplit.Length - 1)
        '        Dim sFileNameChange As String = ""
        '        Dim oCommon As New Common
        '        sFileNameChange = oCommon.GetRandomizeNum()
        '        sFileNameChange = sFileNameChange & "." & auxFileName

        '        '***** 檔案(原檔名,亂數檔名) *****
        '        Dim sFullFileName As String = FileName.Trim & "," & sFileNameChange.Trim

        '        'Me.html_FileUpload.SaveAs(sFilePath & sFileNameChange)

        '        Dim sScript As String = "<script type=""text/javascript"">" & vbCrLf
        '        sScript = sScript & "var html_File = window.parent.document.getElementById(""" & Me.html_File.Text & """);" & vbCrLf
        '        sScript = sScript & "var html_FullFile = window.parent.document.getElementById(""" & Me.html_FullFile.Text & """);" & vbCrLf

        '        sScript = sScript & "html_File.value=""" & FileName & """" & vbCrLf
        '        sScript = sScript & "html_FullFile.value=""" & sFullFileName & """" & vbCrLf
        '        sScript = sScript & "</script>" & vbCrLf

        '        Me.lblScript.Text = sScript
        '    End If
        'End If
    End Sub

End Class

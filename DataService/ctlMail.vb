Imports System.Configuration
Imports System.Data.OracleClient
Imports System.IO
Imports System.Net.Mail
Imports System.Web
Imports ICAT_OracleDAO
Imports RMA_Model

Public Class ctlMail

    Dim _SMTPServer As String = ConfigurationSettings.AppSettings("SMTPServer")
    Dim _UserName As String = ConfigurationSettings.AppSettings("UserName")
    Dim _Password As String = ConfigurationSettings.AppSettings("Password")
    Dim _MailForm As String = ConfigurationSettings.AppSettings("MailForm")
    Dim _NickName As String = ConfigurationSettings.AppSettings("MailForm_NickName")

    'EMAIL
    Public Function Select_RMACHARGE_QUOTED(ByVal RMAD_RMANO As String) As DataTable

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

            sSQL = "select RMA_CUNO,CU_NAME,RMAD_SERIALNO,RMARQ_ACCEPT from RMADETAIL D 
            left join RMAREPAIR_QUOTED  A on D.RMAD_ID = A.RMARQ_RMADID  
            left join RMA  R on D.RMAD_RMANO = R.RMA_NO  
            left join  CUSTOMER C on C.CU_NO = R.RMA_CUNO
            WHERE 1=1" & sCondition

            dt = oQuery.ExecuteDT(sSQL)

        Catch ex As Exception
            Throw ex
        Finally
            oConn.Close()
            oConn.Dispose()
        End Try

        Return dt
    End Function

    Public Function SendMailReBool(ByVal sSubject As String, ByVal sBody As String, ByVal sMailTo As String, Optional ByVal sMailBcc As String = "", Optional ByVal AttachmentFile As Collection = Nothing) As Result
        Dim result As New Result
        Dim i As Integer

        Try
            Dim myMailForm As New MailAddress(_MailForm.Trim, _NickName.Trim)

            '==================================================================================================================================
            '定義Mail 內容
            '==================================================================================================================================
            Dim myMessage As New MailMessage
            myMessage.From = myMailForm
            'Mail To
            If sMailTo.Trim() <> "" Then
                Dim arrMailTo() As String = sMailTo.Split(",")
                For i = 0 To arrMailTo.Length - 1
                    If arrMailTo(i).Trim() <> "" Then
                        Dim myMailTo As New MailAddress(arrMailTo(i))
                        myMessage.To.Add(myMailTo)
                    End If
                Next
            End If

            'Mail Bcc
            If sMailBcc.Trim() <> "" Then
                Dim arrMailBcc() As String = sMailBcc.Split(",")
                For i = 0 To arrMailBcc.Length - 1
                    If arrMailBcc(i).Trim() <> "" Then
                        Dim myMailBcc As New MailAddress(arrMailBcc(i))
                        myMessage.Bcc.Add(myMailBcc)
                    End If
                Next
            End If


            myMessage.Subject = sSubject
            'myMessage.SubjectEncoding = Encoding.UTF8
            myMessage.Body = sBody.Replace("\n", "<br>")
            'myMessage.BodyEncoding = Encoding.UTF8

            myMessage.IsBodyHtml = True
            'myMessage.Priority = MailPriority.Normal
            If IsNothing(AttachmentFile) = False Then
                For i = 1 To AttachmentFile.Count
                    Dim file As String = AttachmentFile(i).ToString()
                    Dim oAttachment As New Attachment(file)
                    myMessage.Attachments.Add(oAttachment)
                Next
            End If

            '==================================================================================================================================
            '定義 SMTP 及 Send Mail
            '==================================================================================================================================
            Dim SMTPServer As New SmtpClient(_SMTPServer.Trim)
            SMTPServer.UseDefaultCredentials = True
            'SMTPServer.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis  '是否用IIS SMTP 傳送
            If _Password.ToString.Trim <> "" Then
                SMTPServer.Credentials = New System.Net.NetworkCredential(_UserName.Trim, _Password.Trim)  '認證
            End If

            Try
                SMTPServer.Send(myMessage)

                result.IsSuccess = True
                result.Message = ""
            Catch ex As Exception
                result.IsSuccess = False
                result.Message = "Message:" & ex.Message & "，InnerException:" & ex.InnerException.ToString() & ",Source:" & ex.Source
                'Throw ex
            End Try


        Catch ex As Exception
            'retval = False
            'Throw ex
            result.IsSuccess = False
            result.Message = "Message:" & ex.Message & "，InnerException:" & ex.InnerException.ToString() & ",Source:" & ex.Source
        Finally

        End Try
        Return result
    End Function

    Public Function SendMail(ByVal sSubject As String, ByVal sBody As String, ByVal sMailTo As String, Optional ByVal sMailBcc As String = "", Optional ByVal AttachmentFile As Collection = Nothing) As Boolean
        Dim i As Integer
        Dim retval As Boolean = False

        Try
            Dim myMailForm As New MailAddress(_MailForm.Trim, _NickName.Trim)


            '==================================================================================================================================
            '定義Mail 內容
            '==================================================================================================================================
            Dim myMessage As New MailMessage
            myMessage.From = myMailForm

            'Mail To
            If sMailTo.Trim() <> "" Then
                Dim arrMailTo() As String = sMailTo.Split(",")
                For i = 0 To arrMailTo.Length - 1
                    If arrMailTo(i).Trim() <> "" Then
                        Dim myMailTo As New MailAddress(arrMailTo(i))
                        myMessage.To.Add(myMailTo)
                    End If
                Next
            End If


            'Mail Bcc
            If sMailBcc.Trim() <> "" Then
                Dim arrMailBcc() As String = sMailBcc.Split(",")
                For i = 0 To arrMailBcc.Length - 1
                    If arrMailBcc(i).Trim() <> "" Then
                        Dim myMailBcc As New MailAddress(arrMailBcc(i))
                        myMessage.Bcc.Add(myMailBcc)
                    End If
                Next
            End If


            myMessage.Subject = sSubject
            'myMessage.SubjectEncoding = Encoding.UTF8
            myMessage.Body = sBody.Replace("\n", "<br>")
            'myMessage.BodyEncoding = Encoding.UTF8

            myMessage.IsBodyHtml = True
            'myMessage.Priority = MailPriority.Normal

            If IsNothing(AttachmentFile) = False Then
                For i = 1 To AttachmentFile.Count
                    Dim file As String = AttachmentFile(i).ToString()
                    Dim oAttachment As New Attachment(file)
                    myMessage.Attachments.Add(oAttachment)
                Next
            End If


            '==================================================================================================================================
            '定義 SMTP 及 Send Mail
            '==================================================================================================================================
            Dim SMTPServer As New SmtpClient(_SMTPServer.Trim)
            SMTPServer.UseDefaultCredentials = True
            'SMTPServer.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis  '是否用IIS SMTP 傳送
            If _Password.ToString.Trim <> "" Then
                SMTPServer.Credentials = New System.Net.NetworkCredential(_UserName.Trim, _Password.Trim)  '認證
            End If

            Try
                SMTPServer.Send(myMessage)
                retval = True

            Catch ex As Exception
                retval = False
                Throw ex
            End Try


        Catch ex As Exception
            retval = False
            Throw ex

        Finally

        End Try

        Return retval
    End Function

    Public Function SendMail_MailMessage(ByVal myMessage As MailMessage) As Boolean
        Dim i As Integer
        Dim retval As Boolean = False

        Try
            Dim myMailForm As New MailAddress(_MailForm.Trim, _NickName.Trim)

            '==================================================================================================================================
            '定義 SMTP 及 Send Mail
            '==================================================================================================================================
            Dim SMTPServer As New SmtpClient(_SMTPServer.Trim)
            SMTPServer.UseDefaultCredentials = True
            'SMTPServer.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis  '是否用IIS SMTP 傳送
            If _Password.ToString.Trim <> "" Then
                SMTPServer.Credentials = New System.Net.NetworkCredential(_UserName.Trim, _Password.Trim)  '認證
            End If

            Try
                SMTPServer.Send(myMessage)
                retval = True

            Catch ex As Exception
                retval = False
                Throw ex
            End Try


        Catch ex As Exception
            retval = False
            Throw ex

        Finally

        End Try

        Return retval
    End Function

End Class

<%@ WebHandler Language="VB" Class="GetAlertRMADataHandler" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data
Imports System.IO
Imports DefLanguage
Public Class GetAlertRMADataHandler : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState

    Dim dtRequest_New As New DataTable
    Dim RMAD_RMANO As String
    Dim RMA_CSTMP As String
    Dim RMAD_SERIALNO As String
    Dim RMAD_STATUS As String
    Dim Query_Alert_Read As String

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim _oLanguage As New ctlLanguage

        Dim dtRequest As New DataTable


        dtRequest_New.Columns.Add("RMAD_RMANO", GetType(String))
        dtRequest_New.Columns.Add("RMA_CSTMP", GetType(String))
        dtRequest_New.Columns.Add("RMAD_SERIALNO", GetType(String))
        dtRequest_New.Columns.Add("RMAD_STATUS", GetType(Integer))
        dtRequest_New.Columns.Add("Query_Alert_Read", GetType(String))

        Dim oClient As New ctlRMA.Client
        Dim _alert As String = ""
        Dim _Crypto As New SecurityCrypt.Crypto

        dtRequest = oClient.Query_Alert(HttpContext.Current.Session.Item("_CustomerID"))

        Dim ctr = From c In dtRequest Select c.Item(1)
                  Distinct


        For a = 0 To ctr.Count - 1
            Dim RMA_NO As String = ctr(a).ToString().Trim()

            Dim dvRMAClient As DataView = dtRequest.DefaultView
            dvRMAClient.RowFilter = "RMAD_RMANO='" & RMA_NO & "'"

            'dvRMAClient.Count
            For i = 0 To dvRMAClient.Count - 1
                RMAD_RMANO = dvRMAClient(i)("RMAD_RMANO").ToString()
                RMA_CSTMP = dvRMAClient(i)("RMA_CSTMP").ToString()
                RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                RMAD_STATUS = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
            Next

            dvRMAClient.Sort = "RMAD_STATUS"
            Dim index As Integer = 0

            For i = 0 To dvRMAClient.Count - 1
                index = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
            Next


            If index >= 91 Then
                For i = 0 To dvRMAClient.Count - 1
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 91 Then
                        RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
						 Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                        Call AddRow()
                    End If
                Next
            End If



            If index >= 90 Then
                For i = 0 To dvRMAClient.Count - 1
			  	    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 90 And BigIndex <> 91 Then
                        RMAD_STATUS = "90"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 70 Then
                For i = 0 To dvRMAClient.Count - 1
				    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 70 And BigIndex <> 91 Then
                        RMAD_STATUS = "70"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 60 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 60 And BigIndex <> 91 Then
                        RMAD_STATUS = "60"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 50 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 50 And BigIndex <> 91 Then
                        RMAD_STATUS = "50"
                        Call AddRow()
                    End If
                Next
            End If

            If index >= 40 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                     Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 40 And BigIndex <> 91 Then
                        RMAD_STATUS = "40"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 35 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 35 And BigIndex <> 91 Then
                        RMAD_STATUS = "35"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 30 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 30 And BigIndex <> 91 Then
                        RMAD_STATUS = "30"
                        Call AddRow()
                    End If
                Next
            End If

            If index >= 20 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    If BigIndex >= 20 Then
                        RMAD_STATUS = "20"
                        Call AddRow()
                    End If
                Next
            End If

            If index >= 10 Then
                For i = 0 To dvRMAClient.Count - 1
                    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
                    Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    If BigIndex >= 10 Then
                        RMAD_STATUS = "10"
                        Call AddRow()
                    End If

                Next
            End If

            If index >= 0 Then
                For i = 0 To dvRMAClient.Count - 1
				    RMAD_SERIALNO = dvRMAClient(i)("RMAD_SERIALNO").ToString()
                    Dim BigIndex As Integer = Convert.ToUInt32(dvRMAClient(i)("RMAD_STATUS"))
			        Query_Alert_Read = dvRMAClient(i)("Query_Alert_Read").ToString()
                    If BigIndex = 0 Then
					    RMAD_STATUS = "0"
                        Call AddRow()
                    End If
                Next
            End If


        Next





        For a = 0 To dtRequest_New.Rows.Count - 1

            Dim RMAD_RMANO As String = dtRequest_New.Rows(a)("RMAD_RMANO").ToString()
            Dim RMAD_WARRANTY As String = dtRequest_New.Rows(a)("RMA_CSTMP").ToString()
            Dim RMAD_SERIALNO As String = dtRequest_New.Rows(a)("RMAD_SERIALNO").ToString()
            Dim RMAD_STATUS As String = dtRequest_New.Rows(a)("RMAD_STATUS").ToString()
            Dim RMAD_RMANO_Crypto As String = _Crypto.Encrypt(RMAD_RMANO, "").Trim()

            Dim RMAD_SERIALNO_Crypto As String = _Crypto.Encrypt(RMAD_SERIALNO, "").Trim()


            '" & _Crypto.Encrypt(RMAD_RMANO, "")
            _alert += "    <div Class='erma-alert-list1'  id='" & RMAD_RMANO_Crypto & "_" & RMAD_SERIALNO_Crypto & "'   onclick='Serch_RMAD_RMANO(this)'  > "
            _alert += "    <div Class='borders'> "
            _alert += "    <div Class='erma-msg'> "
            _alert += "    <span Class='nortext'>" & _oLanguage.getText("RMA", "029", ctlLanguage.eumType.Tag) & " </span> <span class='boldtext'>" & RMAD_RMANO & "</span> <span class='nortext'>"

            _alert += "     </br> " & _oLanguage.getText("RMA", "013", ctlLanguage.eumType.Tag) & ":" & RMAD_SERIALNO
            '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoted, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled';


            Dim NotSent As String = _oLanguage.getText("RMA2", "108", ctlLanguage.eumType.Tag)
            Dim Requested As String = _oLanguage.getText("RMA2", "072", ctlLanguage.eumType.Tag)
            Dim Received As String = _oLanguage.getText("RMA2", "073", ctlLanguage.eumType.Tag)
            Dim Repair_Quoted As String = _oLanguage.getText("RMA2", "074", ctlLanguage.eumType.Tag)
            Dim Sale_Quoted As String = _oLanguage.getText("RMA2", "075", ctlLanguage.eumType.Tag)
            Dim Sales_Confirmed As String = _oLanguage.getText("RMA2", "076", ctlLanguage.eumType.Tag)
            Dim Client_Confirmed As String = _oLanguage.getText("RMA2", "077", ctlLanguage.eumType.Tag)
            Dim Repaired As String = _oLanguage.getText("RMA2", "078", ctlLanguage.eumType.Tag)
            Dim Shipped As String = _oLanguage.getText("RMA2", "079", ctlLanguage.eumType.Tag)
            Dim Closed As String = _oLanguage.getText("RMA2", "080", ctlLanguage.eumType.Tag)
            Dim Canceled As String = _oLanguage.getText("RMA2", "081", ctlLanguage.eumType.Tag)


            If RMAD_STATUS = "0" Then
                _alert += "   " & NotSent & " "
            ElseIf RMAD_STATUS = "10" Then
                _alert += "    " & Requested & " "
            ElseIf RMAD_STATUS = "20" Then
                _alert += "    " & Received & " "
            ElseIf RMAD_STATUS = "30" Then
                _alert += "    " & Repair_Quoted & " "
            ElseIf RMAD_STATUS = "35" Then
                _alert += "    " & Sale_Quoted & " "
            ElseIf RMAD_STATUS = "40" Then
                _alert += "    " & Sales_Confirmed & " "
            ElseIf RMAD_STATUS = "50" Then
                _alert += "   " & Client_Confirmed & " "
            ElseIf RMAD_STATUS = "60" Then
                _alert += "    " & Repaired & " "
            ElseIf RMAD_STATUS = "70" Then
                _alert += "     " & Shipped & " "
            ElseIf RMAD_STATUS = "90" Then
                _alert += "    " & Closed & " "
            ElseIf RMAD_STATUS = "91" Then
                _alert += "     " & Canceled & " "
            End If

            _alert += "    </span> "
            _alert += "    </div> "

            If RMAD_WARRANTY <> "" Then
                _alert += "    <h2 Class='hh2'>" & Convert.ToDateTime(RMAD_WARRANTY).ToString("yyyy/MM/dd") & "</h2> "
            End If

            _alert += "    </div> "

         If IsDBNull(dtRequest_New.Rows(a)("QUERY_ALERT_READ")) Or dtRequest_New.Rows(a)("QUERY_ALERT_READ") = "" Then

                _alert += "   <div Class='erma-circle'></div>"

            Else


            End If


            _alert += "    </div>"

        Next

        context.Response.ContentType = "text/plain"
        context.Response.Write(_alert)

    End Sub

    Public Sub AddRow()

        Dim R As DataRow = dtRequest_New.NewRow
        R("RMAD_RMANO") = RMAD_RMANO
        R("RMA_CSTMP") = RMA_CSTMP
        R("RMAD_SERIALNO") = RMAD_SERIALNO
        R("RMAD_STATUS") = RMAD_STATUS
        R("Query_Alert_Read") = Query_Alert_Read
        dtRequest_New.Rows.Add(R)

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
Imports System.Data
Imports System.Data.OracleClient
Imports DataService
Imports DefLanguage

Partial Class _SendMailEndUser_Accounting
    Inherits System.Web.UI.Page

    Dim _oLanguage As New ctlLanguage
    Dim _MailCC As String = ConfigurationSettings.AppSettings("MailCC")
    Dim _isDebug As String = ConfigurationManager.AppSettings("isDebug")
    Private Function addLog(ByVal LogValue As String)



        Dim oConn As New ICAT_OracleDAO.Connection
        oConn.Open()
        Dim oCommand As OracleCommand = oConn.Command
        Try
            oCommand.CommandText = "SP_ADD_LOG"
            oCommand.CommandType = System.Data.CommandType.StoredProcedure
            oCommand.Parameters.Add("vLOG", OracleType.NVarChar).Value = LogValue
            oCommand.Parameters("vLOG").Direction = ParameterDirection.Input
            oCommand.Parameters.Add("vResult", OracleType.NVarChar, 200)
            oCommand.Parameters("vResult").Direction = ParameterDirection.Output
            oCommand.ExecuteNonQuery()
            oCommand.Parameters.Clear()
            oCommand.CommandText = ""
            oCommand.CommandType = CommandType.Text
        Catch ex As Exception
            Throw ex

        Finally
            oCommand.Dispose()
        End Try
    End Function

    Public Function CheckOracleTransaction(ByVal PO_NO As String) As Boolean

        Dim check As Boolean = False


        Dim myConnection As New OracleConnection("DATA SOURCE=192.168.7.20:1521/topprod;USER ID=CIPHERLABPAY;PASSWORD=ciph1R@pay;PERSIST SECURITY INFO=True;")
        myConnection.Open()

        Dim myCommand As OracleCommand = myConnection.CreateCommand()
        Dim myTrans As OracleTransaction

        ' Start a local transaction
        myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted)
        ' Assign transaction object for a pending local transaction
        myCommand.Transaction = myTrans

        Try
            myCommand.CommandText = "  select count(PO_NO) from ORDERH where  PO_NO =:PO_NO  and PAY_TYPE = 'BankTransfer'  "

            myCommand.Parameters.AddWithValue("PO_NO", PO_NO)



            Dim Read As OracleDataReader = myCommand.ExecuteReader
            While Read.Read()
                If Read(0).ToString().Trim() = "0" Then
                    check = True
                End If
            End While

            myTrans.Commit()
            Console.WriteLine("Both records are written to database.")
        Catch e As Exception
            myTrans.Rollback()
            Console.WriteLine(e.ToString())
            Console.WriteLine("Neither record was written to database.")
        Finally
            myConnection.Close()
        End Try

        Return check
    End Function

    Public Function RunOracleTransaction(ByVal PO_NO As String, ByVal PO_AMT As String, ByVal CREATEUSER As String)
        Dim myConnection As New OracleConnection("DATA SOURCE=192.168.7.20:1521/topprod;USER ID=CIPHERLABPAY;PASSWORD=ciph1R@pay;PERSIST SECURITY INFO=True;")
        myConnection.Open()

        Dim myCommand As OracleCommand = myConnection.CreateCommand()
        Dim myTrans As OracleTransaction

        ' Start a local transaction
        myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted)
        ' Assign transaction object for a pending local transaction
        myCommand.Transaction = myTrans

        Try
            myCommand.CommandText = "  insert into ORDERH (PO_NO,PO_TYPE,PAY_TYPE ,CURRENCY_CODE ,PO_AMT,PO_NET_AMT,PO_FEE_AMT,ORDER_ID,RETURN_URL,PO_STATUS,CREATEUSER,CREATEDATE ) values(:PO_NO,:PO_TYPE,:PAY_TYPE ,:CURRENCY_CODE ,:PO_AMT,:PO_NET_AMT,:PO_FEE_AMT,:ORDER_ID,:RETURN_URL,:PO_STATUS,:CREATEUSER,:CREATEDATE)  "

            myCommand.Parameters.AddWithValue("PO_NO", PO_NO)
            myCommand.Parameters.AddWithValue("PO_TYPE", "RMA")
            myCommand.Parameters.AddWithValue("PAY_TYPE", "BankTransfer")
            myCommand.Parameters.AddWithValue("CURRENCY_CODE", "USD")
            myCommand.Parameters.AddWithValue("PO_AMT", Convert.ToDouble(PO_AMT))
            myCommand.Parameters.AddWithValue("PO_NET_AMT", 0)
            myCommand.Parameters.AddWithValue("PO_FEE_AMT", 0)
            myCommand.Parameters.AddWithValue("ORDER_ID", System.Guid.NewGuid.ToString())

            myCommand.Parameters.AddWithValue("RETURN_URL", "http://e-rma.cipherlab.com.tw/Client_FlowCase01_Worklist.aspx")
            myCommand.Parameters.AddWithValue("PO_STATUS", 1)
            myCommand.Parameters.AddWithValue("CREATEUSER", CREATEUSER)
            myCommand.Parameters.AddWithValue("CREATEDATE", DateTime.Now)

            myCommand.ExecuteNonQuery()

            myTrans.Commit()
            Console.WriteLine("Both records are written to database.")

        Catch e As Exception
            myTrans.Rollback()
            Console.WriteLine(e.ToString())
            Console.WriteLine("Neither record was written to database.")
        Finally
            myConnection.Close()
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim check As Boolean = False

        If Me.IsPostBack = False Then
            Dim RMASQ_ID As String = Request.QueryString.Item("RMASQ_ID")
            check = CheckOracleTransaction(RMASQ_ID) '確認按鈕是否已按過, check=True 尚未按過
        End If

        If check = True Then

            If Me.IsPostBack = False Then

                Dim i As Integer = 0
                Dim blnFlag As Boolean
                Dim sMessage As String = ""
                Dim item_Accept As String = ""
                Dim item_Reject As String = ""

                Dim dtRepairQuoted_Client As New RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable
                Dim ctlRMA As New ctlRMA.Repair_Quoting
                Dim ctlRMAStatus As New ctlRMA.RMAStatus
                Dim ctlClient As New ctlRMA.Client
                Dim sAmt As Double = Request.QueryString.Item("sAmt")
                Dim dtEndUser As DataTable
                Dim ctlRMAR As New ctlRMA.Requested
                Dim query As String = Request.QueryString.Item("SendMailEndUser_Accounting")

                Dim RMASQ_ID As String = Request.QueryString.Item("RMASQ_ID")
                Dim RMASQ_CLIENTAD As String = Request.QueryString.Item("RMASQ_CLIENTAD")
                Dim RMASQ_CLIENTADNAME As String = Request.QueryString.Item("RMASQ_CLIENTADNAME")
                Dim Customer As String = Request.QueryString.Item("Customer")

                addLog("ctMail EndUserMail : " & RMASQ_ID & " 點選確認之會計員工編號: " & RMASQ_CLIENTAD & " 時間: " & Date.Now.ToString())

                dtRepairQuoted_Client = Newtonsoft.Json.JsonConvert.DeserializeObject(Of RmaDTO.tmpRMAREPAIR_QUOTED_ClientDataTable)(query)

                Try

                    Dim RMARQ_ACCEPT_1 As String = ""
                    Dim RMARQ_ACCEPT_2 As String = ""
                    Dim RMARQ_LUADNAME As String = ""

                    '客戶端 修改 RMARQ_ACCEPT: 是否要維修: 1.Accept, 2.Reject
                    ctlRMA.Update_RepairQuoted_Client(dtRepairQuoted_Client)

                    '取得拒絕/接受的Serial 清單, 用於Mail中
                    For i = 0 To dtRepairQuoted_Client.Rows.Count - 1
                        Dim dr As RmaDTO.tmpRMAREPAIR_QUOTED_ClientRow = dtRepairQuoted_Client.Rows(i)

                        RMARQ_LUADNAME = dr.RMARQ_LUADNAME.ToString().Trim()

                        If dr.RMARQ_ACCEPT = 1 Then 'Accept

                            Dim oRMA As New ctlRMA.Client
                            Dim dtClientDetail As New DataTable

                            dtClientDetail = oRMA.QryRMADETAIL(dr.RMARQ_RMADID.ToString())

                            For a = 0 To dtClientDetail.Rows.Count - 1
                                RMARQ_ACCEPT_1 += dtClientDetail.Rows(a)("RMAD_SERIALNO").ToString() & ","
                            Next

                        ElseIf dr.RMARQ_ACCEPT = 2 Then 'Reject

                            Dim oRMA As New ctlRMA.Client
                            Dim dtClientDetail As New DataTable

                            dtClientDetail = oRMA.QryRMADETAIL(dr.RMARQ_RMADID.ToString())

                            For a = 0 To dtClientDetail.Rows.Count - 1
                                RMARQ_ACCEPT_2 += dtClientDetail.Rows(a)("RMAD_SERIALNO").ToString() & ","
                            Next

                        End If


                    Next

                    '查客戶資料
                    Dim Customer_ As New ctlCustomer.Customer
                    Dim CustomerDataTable_ As New CustomerDTO.CustomerDataTable
                    CustomerDataTable_ = Customer_.QueryByPrimaryKey(RMARQ_LUADNAME)

                    For u = 0 To CustomerDataTable_.Rows.Count - 1
                        RMARQ_LUADNAME = CustomerDataTable_.Rows(u)("CU_NAME").ToString()
                    Next

                    Dim dvRMAREPAIR_QUOTED_Client As DataView = dtRepairQuoted_Client.DefaultView()

                    '=============================================================================================================================================================================================================================
                    '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                    'Reject 的要 cancel
                    '=============================================================================================================================================================================================================================
                    'Dim dtStatus As New RmaDTO.RMADetailStatusDataTable


                    '拒絕的項目
                    Dim dtReject As New RmaDTO.ClientQuoted_ConfirmedDataTable
                    dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=2"

                    For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                        Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtReject.NewClientQuoted_ConfirmedRow
                        Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()
                        Dim RMARQ_QUOTE As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_QUOTE").ToString().Trim()
                        Dim iServiceCharge As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_LABORHOUR").ToString().Trim()

                        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                        '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                        dr.RMAD_ID = RMARQ_RMADID.Trim()
                        'dr.RMASQ_QUOTE = Convert.ToDouble(iServiceCharge)
                        dr.RMASQ_QUOTE = Convert.ToDouble(RMARQ_QUOTE)
                        dr.RMAD_STATUS = 91

                        dr.RMASQ_ID = RMASQ_ID
                        dr.RMASQ_CLIENTAD = Customer
                        dr.RMASQ_CLIENTADNAME = RMASQ_CLIENTADNAME
                        dr.RMASQ_CLIENTDATE = Date.Now

                        dtReject.AddClientQuoted_ConfirmedRow(dr)
                    Next

                    '壓回
                    If dtReject.Rows.Count > 0 Then
                        ctlClient.Bank_ClientQuoted_Confirmed(dtReject)
                    End If


                    '=============================================================================================================================================================================================================================
                    '修改RMAD狀態(50)-->客戶自行確認, 報價
                    '=============================================================================================================================================================================================================================

                    '接受的項目
                    Dim dtConfirmed As New RmaDTO.ClientQuoted_ConfirmedDataTable
                    dvRMAREPAIR_QUOTED_Client.RowFilter = "RMARQ_ACCEPT=1"
                    For i = 0 To dvRMAREPAIR_QUOTED_Client.Count() - 1
                        Dim dr As RmaDTO.ClientQuoted_ConfirmedRow = dtConfirmed.NewClientQuoted_ConfirmedRow
                        Dim RMARQ_RMADID As String = dvRMAREPAIR_QUOTED_Client(i)("RMARQ_RMADID").ToString().Trim()

                        '0.Requested 暫存階段, 10.Requested, 20.Received, 30.Repair Quoted, 35.Sale Quoting, 40.Sales Confirmed, 
                        '50.Client Confirmed, 60.Repaired, 70.Shipped, 90.Closed, 91.Canceled
                        dr.RMAD_ID = RMARQ_RMADID.Trim()
                        dr.RMAD_STATUS = 50

                        dr.RMASQ_ID = RMASQ_ID
                        dr.RMASQ_CLIENTAD = Customer
                        dr.RMASQ_CLIENTADNAME = RMASQ_CLIENTADNAME
                        dr.RMASQ_CLIENTDATE = Date.Now
                        dr.RMASQ_CLIENTCONFIRM = 1     '1.客戶自行確認, 2.業務帶客戶確認

                        dtConfirmed.AddClientQuoted_ConfirmedRow(dr)
                    Next

                    '壓回
                    If dtConfirmed.Rows.Count > 0 Then
                        ctlClient.Bank_ClientQuoted_Confirmed(dtConfirmed)
                    End If

                    Dim sRMA As String = RMASQ_ID
                    Dim _CustomerID As String = RMASQ_CLIENTAD
                    Dim _UserID As String = RMASQ_CLIENTAD


                    If 1 = 1 Then

                        Label1.Text = "第一階段"

                        blnFlag = False
                        Dim oMail As New ctlMail
                        Dim sMsg As String = ""
                        Dim sSubject As String = "RMA NO." & sRMA & " ’s payment has been confirmed by " & RMASQ_CLIENTADNAME
                        Dim sEmailURL As String = _oLanguage.getText("Mail", "005", ctlLanguage.eumType.Mail)
                        Dim sMailTo As String = "sunny@cipherlab.com.tw,Miko@cipherlab.com.tw,"

                        Dim CU_SALESID As String
                        Dim CU_ASSISTANTID As String
                        Dim oCustomer As New ctlCustomer.CustomerUser
                        Dim dtCustomer As New CustomerDTO.VWCUSTOMERUSERDataTable
                        Dim oAdmin As New ctlAdmin
                        Dim dtAdmin As New AccountDTO.ADMINDataTable


                        Dim oRMA As New ctlRMA
                        Dim Repaire_EMAIL As String = ""                   '維修人員 EMail
                        Dim Repaire_Name As String = ""                    '維修人員

                        Dim arrRepaire As ArrayList = oRMA.getRepaireMail_RMA(sRMA)

                        For j = 0 To arrRepaire.Count - 1
                            Dim arrList() As String = arrRepaire(j)
                            Repaire_Name = arrList(0).Trim()
                            Repaire_EMAIL = arrList(1).Trim()
                            sMailTo += Repaire_EMAIL + ","
                        Next

                        '存入paypal平台log 開始
                        Dim CREATEUSER_ID As String = ""    '客戶ID
                        Dim CREATEUSER_Name As String = "" '客戶名稱
                        For u = 0 To CustomerDataTable_.Rows.Count - 1
                            CREATEUSER_ID = CustomerDataTable_.Rows(u)("CU_NO").ToString()
                            CREATEUSER_Name = CustomerDataTable_.Rows(u)("CU_NAME").ToString()
                        Next

                        Call RunOracleTransaction(sRMA, sAmt, CREATEUSER_ID & "-" & CREATEUSER_Name)
                        '存入paypal平台log 結束


                        Dim oRMA_ As New ctlRMA.Client
                        Dim dtClientDetail As New RmaDTO.tmpClientDetailDataTable

                        Dim sBody As String = ""
                        sBody += "Dear " & Repaire_Name & "," & "</br>"
                        sBody += "This notice is your customer " & RMARQ_LUADNAME

                        If RMASQ_CLIENTAD = "admin" Then
                            sBody += "Paypal"
                        Else

                        End If

                        sBody += " payment has been confirmed by " & RMASQ_CLIENTADNAME & "(" & _UserID & ")" & " and get into repair status." & "</br>"
                        sBody += "The RMA No. " & sRMA & " is getting into the repair status." & "</br></br>"

                        If RMARQ_ACCEPT_1.Length > 0 Then
                            sBody += "Accept Item: " & RMARQ_ACCEPT_1.Remove(RMARQ_ACCEPT_1.Length - 1, 1) & "</br></br>"
                        Else
                            sBody += "Accept Item:  </br></br>"
                        End If

                        If RMARQ_ACCEPT_2.Length > 0 Then
                            sBody += "Cancel Item: " & RMARQ_ACCEPT_2.Remove(RMARQ_ACCEPT_2.Length - 1, 1) & "</br></br>"
                        Else
                            sBody += "Cancel Item:  </br></br>"
                        End If

                        sBody += "To check the application status, or to see detailed application information, please visit " & " web: <a href='http://e-rma.cipherlab.com.tw'>http://e-rma.cipherlab.com.tw</a> and status query page." & "</br>"
                        sBody += "Thank you for using the RMA system." & "</br>"

                        sBody += "== This is an automated system notification, please do not reply. ==" & "</br>"
                        sBody += "== For any system operation issues, please contact Cipherlab E-RMA Administrator: e-rma@cipherlab.com.tw ==" & "</br></br>"

                        sBody += "Best Regards," & "</br>"
                        sBody += "Cipherlab E-RMA Service Center" & "</br>"

                        If RMASQ_CLIENTAD = "admin" Then
                            sMailTo += "Fanny@cipherlab.com.tw" & "," & "Vivian@cipherlab.com.tw" & "," & "Teresa.Cheng@cipherlab.com.tw"
                        End If

                        '修改寄信前判斷目前是否debug模式，是：改成config裡設定MailTo人員；否：正式環境人員。 by buck modify 20250703
                        If _isDebug = True Then
                            sMailTo = ConfigurationManager.AppSettings("MailTo")
                            _MailCC = ConfigurationManager.AppSettings("MailCC")
                        End If
                        blnFlag = oMail.SendMail(sSubject, sBody, sMailTo, _MailCC)
                        Label1.Text = "已寄信"

                        If RMASQ_CLIENTAD = "admin" Then
                            Response.Redirect("Client_FlowCase01_Worklist.aspx")
                        End If

                    End If


                    blnFlag = True

                Catch ex As Exception
                    sMessage = ex.Message
                    blnFlag = False

                Finally

                    If blnFlag = False Then

                    Else

                    End If

                End Try


            End If


            If Me.IsPostBack = False Then

                '紀錄
                Dim RMASQ_ID_String As String = Request.QueryString.Item("RMASQ_ID")
                Dim sAmt As Double = Convert.ToDouble(Request.QueryString.Item("sAmt"))


                If RMASQ_ID_String <> "" And sAmt <> 0 Then
                    Dim oConn_string As String = "DATA SOURCE=192.168.7.20:1521/topprod;USER ID=CIPHERLABPAY;PASSWORD=ciph1R@pay;PERSIST SECURITY INFO=True;"
                    'Dim oConn_string As String = "DATA SOURCE=192.168.7.20:1521/topprod;USER ID=CIPHERLABPAY;PASSWORD=ciph1R@pay;PERSIST SECURITY INFO=True;"
                    Dim myOracleConnection As New OracleConnection(oConn_string)
                    '第一步 修改RMA PO_NO 為TRANSATION_ID
                    Try
                        myOracleConnection.Open()
                        Dim myOracleCommand As OracleCommand = myOracleConnection.CreateCommand
                        Dim dt As New DataTable
                        Dim sSQL As String = ""
                        sSQL = sSQL & " UPDATE ORDERH SET TRANSATION_ID =:TRANSATION_ID WHERE PO_NO =:PO_NO  "
                        myOracleCommand.CommandText = sSQL
                        myOracleCommand.Parameters.Add(":PO_NO", RMASQ_ID_String)
                        myOracleCommand.Parameters.Add(":TRANSATION_ID", RMASQ_ID_String)
                        myOracleCommand.ExecuteNonQuery()
                    Catch ex As Exception

                    Finally
                        myOracleConnection.Close()
                    End Try

                    '第二步 沖帳 新增
                    Try
                        myOracleConnection.Open()
                        Dim myOracleCommand As OracleCommand = myOracleConnection.CreateCommand
                        Dim dt As New DataTable
                        Dim sSQL As String = ""
                        sSQL = sSQL & " DECLARE  "
                        sSQL = sSQL & " BEGIN "
                        sSQL = sSQL & " SP_SHP_INS_TR_Bank_Transfer(:TRANSATION_ID);"
                        sSQL = sSQL & " END;"
                        myOracleCommand.CommandText = sSQL
                        myOracleCommand.Parameters.Add(":TRANSATION_ID", RMASQ_ID_String)
                        myOracleCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim err As String = ex.Message
                    Finally
                        myOracleConnection.Close()
                    End Try
                End If

            End If

        End If

    End Sub

End Class

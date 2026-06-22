<%@ WebHandler Language="VB" Class="GetSnDataHandlerNew" %>

Imports System
Imports System.Web
Imports Newtonsoft.Json
Imports DataService
Imports System.Data
Imports System.IO
Imports System.Data.SqlClient

Public Class GetSnDataHandlerNew : Implements IHttpHandler

    Public Class test
        Public Property a As String
        Public Property b As String
    End Class

    Public Class SnData
        Public Property sn As String
    End Class


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "application/json"
        '接受前臺傳的num
        Dim Sn_no As String
        Using reader As StreamReader = New StreamReader(context.Request.InputStream)
            Sn_no = reader.ReadToEnd()
        End Using

        If (Sn_no = "") Then
            context.Response.Write("")
            Return
        End If

        Dim p As SnData = JsonConvert.DeserializeObject(Of SnData)(Sn_no)

        Dim _SnData As String = JsonConvert.SerializeObject(GetSnData(p.sn))

        If _SnData = "[]" Then
            context.Response.Write("")
        End If
        context.Response.Write(_SnData)

    End Sub


    Private Function GetSnData(ByVal DistNumber As String) As Object
        Dim ConnectionString As String = "Data Source = 10.1.1.154; Initial Catalog =CipherLabs_0419_PRD; User ID = data_query; Password = DaQu#0103;"

        '
        Dim myDataSet As New DataSet

        Try

            Dim sql As String = "select ItemCode, Dscription,convert(varchar, GrnStart, 111) GrnStart, convert(varchar,GrntExp_Cal, 111)GrntExp_Cal  from ( " &
                "select DocDate, ShipTo, ItemCode, Dscription, DistNumber,  " &
                "case when GrnStart is null then DocDate else GrnStart end GrnStart,  " &
                "GrntExp, year1, year2, " &
                "case when GrntExp is null  " &
                "then case when SUBSTRING(ItemCode, 1, 1) = 'M'  " &
                "then DateAdd(year, year1+year2, GrnStart)  " &
                "else case when SUBSTRING(ItemCode, 1, 1) = 'A'  " &
                "then DateAdd(year, 1, GrnStart)  " &
                "else null  " &
                "end   " &
                "end    " &
                "else GrntExp  " &
                "end GrntExp_Cal      " &
                "from  " &
                "(" &
                "SELECT a.DocEntry, a.DocNum, CONVERT(CHAR(10), a.DocDate, 126) DocDate, a.DocDueDate, a.CardCode, a.CardName, a.Address2 ShipTo, a.Comments, " &
                "b.ItemCode, b.Dscription, b.Quantity, b.ShipDate, b.U_RN CustPO#, " &
                "upper(e.DistNumber) DistNumber, case when e.GrntStart is null then a.DocDate else e.GrntStart end GrnStart, e.GrntExp, " &
                "case when ISNUMERIC(SUBSTRING(b.ItemCode, 7, 1))=1 then Cast(SUBSTRING(b.ItemCode, 7, 1) as INT) else 0 end year1, " &
                "case when ISNUMERIC(SUBSTRING(b.ItemCode, 9, 1))=1 then Cast(SUBSTRING(b.ItemCode, 9, 1) as INT) else 0 end year2 " &
                "FROM [CipherLabs_0419_PRD].[dbo].[ODLN] a " &
                "inner join [CipherLabs_0419_PRD].[dbo].[DLN1] b ON a.DocEntry = b.DocEntry   " &
                "inner join [CipherLabs_0419_PRD].[dbo].[OITL] c on b.DocEntry = c.ApplyEntry and b.LineNum = c.ApplyLine and c.ApplyType = 15 " &
                "inner join [CipherLabs_0419_PRD].[dbo].[ITL1] d on c.LogEntry = d.LogEntry " &
                "inner join [CipherLabs_0419_PRD].[dbo].[OSRN] e on d.ItemCode = e.ItemCode and d.MdAbsEntry =e.AbsEntry " &
                "where  " &
                "a.CANCELED = 'N' and a.DocStatus = 'C' and a.InvntSttus = 'C' " &
                "and upper(e.DistNumber) = @DistNumber " &
                ") warranty  " &
                ") warranty_final " &
                "order by GrntExp_Cal desc"



            Using cn As New SqlConnection(ConnectionString)
                Try
                    Dim cmd As SqlCommand = New SqlCommand
                    With cmd
                        .Connection = cn
                        .Connection.Open()
                        .CommandText = sql
                        .Parameters.Add("@DistNumber", SqlDbType.NVarChar)
                        .Parameters("@DistNumber").Value = DistNumber
                    End With
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(myDataSet)
                    cmd.Dispose()

                Catch ex As Exception
                    Dim err As String = ex.Message

                End Try
            End Using


        Catch ex As Exception

        End Try

        Return myDataSet.Tables(0)

    End Function



    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
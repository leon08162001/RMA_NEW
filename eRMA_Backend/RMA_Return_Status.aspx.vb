Imports System.Web.Services
Imports DataService
Imports Newtonsoft.Json

Partial Class RMA_Return_Status
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("_Role").ToString().IndexOf("C") = -1 Then
            Session("_isTimeOut") = True
            Response.Redirect("Default.aspx")
        End If

    End Sub

    <WebMethod>
    Public Shared Function GetRMAStatus(sRMA_NO As String) As Object

        Dim oExtend As New ctlExtend

        Dim _RMAStatus As String = JsonConvert.SerializeObject(oExtend.QryRMAStatus(sRMA_NO))

        Return _RMAStatus

    End Function

    <WebMethod>
    Public Shared Function UpdateRMAStatus(objStatus As String) As String

        Dim oExtend As New ctlExtend

        Dim i, j As Int32
        Dim sErr As String = "ok"

        Dim StatusArray() As String = objStatus.Split(",")
        For i = 0 To StatusArray.Length - 1
            If StatusArray(i).Trim() <> "" Then
                Dim SerialNoArray() As String = StatusArray(i).Trim().Split("@")
                sErr = oExtend.UpdateRMAStatus(SerialNoArray(0).Trim(), SerialNoArray(1).Trim(), SerialNoArray(2).Trim(), SerialNoArray(3).Trim())
                If sErr <> "" Then
                    Return sErr
                End If
            End If
        Next



        Return sErr

    End Function

End Class

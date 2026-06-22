Imports System.ComponentModel

Public Enum ResultStatus
    <Description("The request was successful")>
    Success = 1
    <Description("Request failed")>
    Fail = 0
    <Description("request exception")>
    [Error] = -1
End Enum

Public Class ResponseResult(Of T)

    Public Property Status As ResultStatus

    Public Property Message As String

    Public Property Data As T

End Class


Imports RMA_Model.Enums
''' <summary>
''' Initializes a new instance of the <see cref="APException"/> class.
''' APException
''' </summary>
Public Class APException
    Inherits ApplicationException

    ''' <summary>
    ''' Initializes a new instance of the <see cref="APException"/> class.
    ''' </summary>
    ''' <param name="statusCode">1</param>
    ''' <param name="msg">2</param>
    Public Sub New(statusCode As ResultCodeEnum, msg As String)
        MyBase.New(msg)
        Result.ResultCode = statusCode
        Result.Message = msg
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="APException"/> class.
    ''' </summary>
    ''' <param name="result">result</param>
    Public Sub New(result As Result)
        Me.Result = result
    End Sub

    ''' <summary>
    ''' Gets or sets Result
    ''' </summary>
    Public Property Result As Result = New Result()

End Class

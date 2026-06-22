Imports System.ComponentModel
Imports RMA_Model.Enums

''' <summary>
''' Result
''' </summary>
Public Class Result

    Private _resultCode As ResultCodeEnum = ResultCodeEnum.OK
    Private _msg As String = Nothing
    Private _errors As New List(Of String)()

    '加入回傳boolean by buck add 20251201 being
    Public Property IsSuccess As Boolean
        Get
            Return Me.ResultCode = ResultCodeEnum.OK
        End Get
        Set(value As Boolean)
            If value Then
                Me.ResultCode = ResultCodeEnum.OK
            Else
                '❗ 你可以改成預設錯誤代碼，例如：ResultCodeEnum.FAIL
                Me.ResultCode = ResultCodeEnum.FAIL
            End If
        End Set
    End Property
    '加入回傳boolean by buck add 20251201 end

    ''' <summary>
    ''' Gets or Sets ResultCode
    ''' </summary>
    <Description("結果代碼")>
    Public Property ResultCode As ResultCodeEnum
        Get
            Return _resultCode
        End Get
        Set(value As ResultCodeEnum)
            If _resultCode <> value Then
                _resultCode = value
                _msg = Nothing
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets Message
    ''' </summary>
    <Description("結果訊息")>
    Public Property Message As String
        Get
            If _msg Is Nothing Then
                _msg = My.Resources.ResourceManager.GetString(ResultCode.ToString(), My.Resources.Culture)
            End If
            Return _msg
        End Get
        Set(value As String)
            _msg = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets Errors
    ''' </summary>
    <Description("錯誤說明，若沒有錯誤則為空陣列")>
    Public Property Errors As List(Of String)
        Get
            If _errors.Count = 0 AndAlso ResultCode <> ResultCodeEnum.OK Then
                _errors.Add(Message)
            End If
            Return _errors.Distinct().ToList()
        End Get
        Set(value As List(Of String))
            _errors = value
        End Set
    End Property

    ''' <summary>
    ''' 複數錯誤訊息陣列
    ''' </summary>
    <Description("複數錯誤訊息陣列")>
    Public Property ErrorMessages As List(Of String)

    ''' <summary>
    ''' Validate
    ''' </summary>
    ''' <returns>Result</returns>
    ''' <exception cref="APException">exception</exception>
    Public Overridable Function Validate() As Result
        If ResultCode = ResultCodeEnum.OK Then
            Return Me
        Else
            Throw New APException(Me)
        End If
    End Function

End Class

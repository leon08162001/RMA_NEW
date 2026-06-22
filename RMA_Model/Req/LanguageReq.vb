Imports System.Web.UI


Public Class LanguageReq
    ''' <summary>
    ''' 目標 GridView 的資料列
    ''' </summary>
    Public Property TargetControl As Control
    ''' <summary>
    ''' 命名空間名稱
    ''' </summary>
    Public Property NamespaceName As String
    ''' <summary>
    ''' 類別分類名稱
    ''' </summary>
    Public Property Category As String
    ''' <summary>
    ''' 類別名稱
    ''' </summary>
    Public Property ClassName As String
    ''' <summary>
    ''' 所屬區段（部分）
    ''' </summary>
    Public Property Section As String
    ''' <summary>
    ''' 語系代碼對應表 (Key = ID, Value = 語系文字)
    ''' </summary>
    Public Property mapping As Dictionary(Of Integer, String)
    ''' <summary>
    ''' 語系代碼 (例如：EN、ZH-TW)
    ''' </summary>
    Public Property LangCode As String

End Class

Imports System.IO

Public Class ProgressBar
    Inherits System.Web.UI.Page

    Private Property MaxValue() As Integer
        Get
            If ViewState("MaxValue") Is Nothing Then
                Return 0
            Else
                Return Convert.ToInt32(ViewState("MaxValue"))
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("ThisValue") = Value
        End Set
    End Property

    Private Property ThisValue() As Integer
        Get
            If ViewState("ThisValue") Is Nothing Then
                Return 0
            Else
                Return Convert.ToInt32(ViewState("ThisValue"))
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("ThisValue") = Value
        End Set
    End Property

    Dim m_page As System.Web.UI.Page
    '/ <summary>
    '/ 功能描述:构造函数
    '/ 作　　者:huangzh
    '/ 创建日期:2016-05-06 11:54:34
    '/ 任务编号:
    '/ </summary>
    '/ <param name="page">当前页面</param>
    Sub New(ByVal page As System.Web.UI.Page)
        m_page = page
    End Sub

    '/ <summary>
    '/ 功能描述:初始化进度条
    '/ 作　　者:huangzh
    '/ 创建日期:2016-05-06 11:55:26
    '/ 任务编号:
    '/ </summary>
    Public Sub InitProgress()
        '根据ProgressBar.htm显示进度条界面
        Dim templateFileName As String = AppDomain.CurrentDomain.BaseDirectory + "ProgressBar.html"
        'Dim reader As StreamReader = New StreamReader(templateFileName, System.Text.Encoding.GetEncoding("GB2312"))
        Dim reader As StreamReader = New StreamReader(templateFileName, System.Text.Encoding.GetEncoding("utf-8"))
        Dim strhtml As String = reader.ReadToEnd()
        reader.Close()
        m_page.Response.Write(strhtml)
        m_page.Response.Flush()
    End Sub

    Public Sub SetMaxValue(ByVal intMaxValue As Integer)
        MaxValue = intMaxValue
    End Sub

    '/ <summary>
    '/ 功能描述:设置标题
    '/ 作　　者:huangzh
    '/ 创建日期:2016-05-06 11:55:36
    '/ 任务编号:
    '/ </summary>
    '/ <param name="strTitle">strTitle</param>
    Public Sub SetTitle(ByVal strTitle As String)
        Dim strjsBlock As String = "<script>SetTitle('" + strTitle + "'); </script>"

        m_page.Response.Write(strjsBlock)
        m_page.Response.Flush()
    End Sub

    '/ <summary>
    '/ 功能描述:设置进度
    '/ 作　　者:huangzh
    '/ 创建日期:2016-05-06 11:55:45
    '/ 任务编号:
    '/ </summary>
    '/ <param name="percent">percent</param>
    Public Sub AddProgress(ByVal intpercent As Integer)
        ThisValue = ThisValue + intpercent
        Dim dblstep As Double = (CType(ThisValue / CType(MaxValue, Double), Double)) * 100

        Dim strjsBlock As String = "<script>SetPorgressBar('" + dblstep.ToString("0.00") + "'); </script>"

        m_page.Response.Write(strjsBlock)
        m_page.Response.Flush()
    End Sub

    Public Sub DisponseProgress()
        Dim strjsBlock As String = "<script>SetCompleted();</script>"
        m_page.Response.Write(strjsBlock)
        m_page.Response.Flush()
    End Sub

End Class


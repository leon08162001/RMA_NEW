Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Newtonsoft.Json.Linq

Namespace Utility

    Public Class LanguageHelper
        Private Shared _langCache As JObject = Nothing
        Private Class DummyContainer
            Inherits Control
        End Class
        ' ================================
        ' 語系代碼對照表（加入你要求的 001 → en 等）
        ' ================================
        Private Shared ReadOnly _langMapping As New Dictionary(Of String, String) From {
            {"001", "en"},
            {"002", "zh"},
            {"003", "jp"}
        }

        ' 將外部語系代碼轉成實際語系（en/zh/jp）
        Private Shared Function GetCurrentLang(lang As String) As String
            If String.IsNullOrWhiteSpace(lang) Then
                Return "en"   ' 預設語系，可自行調整
            End If

            ' 001 → en / 002 → zh / 003 → jp
            If _langMapping.ContainsKey(lang) Then
                Return _langMapping(lang)
            End If

            ' 傳入 en/zh/jp → 直接使用
            Return lang.ToLower()
        End Function

        ' ===== 讀取內嵌 JSON (Embedded Resource) =====
        Private Shared Function LoadLangJson() As JObject
            If _langCache IsNot Nothing Then Return _langCache

            Dim asm As Assembly = Assembly.GetExecutingAssembly()

            Dim resourceName = asm.GetManifestResourceNames().
                FirstOrDefault(Function(n) n.EndsWith("language.json", StringComparison.OrdinalIgnoreCase))

            If resourceName Is Nothing Then
                Throw New Exception("找不到內嵌資源 language.json，請確認 Build Action = Embedded Resource")
            End If

            Using stream As Stream = asm.GetManifestResourceStream(resourceName)
                Using reader As New StreamReader(stream)
                    Dim json As String = reader.ReadToEnd()
                    _langCache = JObject.Parse(json)
                End Using
            End Using

            Return _langCache
        End Function

        Private Shared Function GetText(token As JToken, key As String, lang As String, Optional fallback As Boolean = True) As String
            If token Is Nothing OrElse String.IsNullOrWhiteSpace(key) Then
                Return If(fallback, "[" & key & "]", Nothing)
            End If

            Dim result As String = Nothing
            FindKeyRecursive(token, key, lang, result)

            If result Is Nothing AndAlso fallback Then
                Return "[" & key & "]"
            End If

            Return result
        End Function

        Private Shared Sub FindKeyRecursive(token As JToken, key As String, lang As String, ByRef result As String)
            If token.Type = JTokenType.Object Then
                For Each prop As JProperty In token.Children(Of JProperty)()
                    If prop.Name.Equals("GRID", StringComparison.OrdinalIgnoreCase) Then
                        Continue For
                    End If

                    If prop.Name = key AndAlso prop.Value.Type = JTokenType.Object AndAlso prop.Value(lang) IsNot Nothing Then

                        result = prop.Value(lang).ToString()
                        Exit Sub
                    End If

                    FindKeyRecursive(prop.Value, key, lang, result)
                    If result IsNot Nothing Then Exit Sub
                Next
            ElseIf token.Type = JTokenType.Array Then
                For Each item As JToken In token
                    FindKeyRecursive(item, key, lang, result)
                    If result IsNot Nothing Then Exit Sub
                Next
            End If
        End Sub

        Public Shared Sub Apply(ctrl As Control, strlang As String)
            Dim pageName As String = ""

            If TypeOf ctrl Is Page Then
                ' 取得頁面名稱，保留原始大小寫，去掉 .aspx
                Dim pg As Page = DirectCast(ctrl, Page)
                pageName = Path.GetFileName(pg.Request.Path)
                If pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) Then
                    pageName = pageName.Substring(0, pageName.Length - 5)
                End If
            Else
                ' UserControl: 取得原始路徑
                Dim uc As UserControl = TryCast(ctrl, UserControl)
                If uc IsNot Nothing AndAlso uc.AppRelativeVirtualPath IsNot Nothing Then
                    pageName = Path.GetFileName(uc.AppRelativeVirtualPath)
                    If pageName.EndsWith(".ascx", StringComparison.OrdinalIgnoreCase) Then
                        pageName = pageName.Substring(0, pageName.Length - 5)
                    End If
                Else
                    ' fallback
                    pageName = ctrl.GetType().Name
                End If
            End If

            Dim lang = GetCurrentLang(strlang)

            Dim json = LoadLangJson()
            Dim pageToken = json.SelectToken(pageName)
            If pageToken Is Nothing Then Exit Sub

            ApplyRecursive(ctrl, pageToken, lang)
        End Sub

        Private Shared Sub ApplyRecursive(ctrl As Control, pageToken As JToken, lang As String)
            ' ===== Label / Button / LinkButton / ImageButton / TextBox =====
            If TypeOf ctrl Is ITextControl _
               AndAlso Not TypeOf ctrl Is DropDownList _
               AndAlso Not TypeOf ctrl Is RadioButtonList _
               AndAlso Not TypeOf ctrl Is CheckBoxList _
                AndAlso Not TypeOf ctrl Is TextBox Then

                Dim t = DirectCast(ctrl, ITextControl)
                If Not String.IsNullOrEmpty(ctrl.ID) Then
                    Dim v = GetText(pageToken, ctrl.ID, lang)
                    If v IsNot Nothing Then t.Text = v
                End If

            ElseIf TypeOf ctrl Is Button _
                OrElse TypeOf ctrl Is LinkButton _
                OrElse TypeOf ctrl Is ImageButton Then

                Dim id = ctrl.ID
                Dim t = GetText(pageToken, id, lang)
                If t.StartsWith("[") Then Exit Sub

                If TypeOf ctrl Is Button Then
                    DirectCast(ctrl, Button).Text = t
                ElseIf TypeOf ctrl Is LinkButton Then
                    DirectCast(ctrl, LinkButton).Text = t
                ElseIf TypeOf ctrl Is ImageButton Then
                    DirectCast(ctrl, ImageButton).AlternateText = t
                End If

                ' ===== DropDownList =====
            ElseIf TypeOf ctrl Is DropDownList Then
                Dim ddl = DirectCast(ctrl, DropDownList)

                For Each item As ListItem In ddl.Items
                    If String.IsNullOrWhiteSpace(item.Text) Then Continue For

                    Dim translated = GetText(pageToken, item.Text, lang)
                    If Not translated.StartsWith("[") Then
                        item.Text = translated
                    End If
                Next

                ' ===== RadioButtonList =====
            ElseIf TypeOf ctrl Is RadioButtonList Then
                Dim rbl = DirectCast(ctrl, RadioButtonList)

                For Each item As ListItem In rbl.Items
                    If String.IsNullOrWhiteSpace(item.Text) Then Continue For

                    Dim t = GetText(pageToken, item.Text, lang)
                    If Not t.StartsWith("[") Then
                        item.Text = t
                    End If
                Next

                ' ===== CheckBoxList =====
            ElseIf TypeOf ctrl Is CheckBoxList Then
                Dim cbl = DirectCast(ctrl, CheckBoxList)

                For Each item As ListItem In cbl.Items
                    If String.IsNullOrWhiteSpace(item.Text) Then Continue For

                    Dim t = GetText(pageToken, item.Text, lang)
                    If Not t.StartsWith("[") Then
                        item.Text = t
                    End If
                Next

            End If

            ' 遞迴子控制項
            For Each c As Control In ctrl.Controls
                ApplyRecursive(c, pageToken, lang)
            Next
        End Sub

        Public Shared Sub ApplyGridHeader(gv As GridView, pageName As String, gridKey As String, slang As String)
            Dim json = LoadLangJson()
            Dim lang = GetCurrentLang(slang)
            Dim gridToken = json.SelectToken(String.Format("{0}.UI.GRID.{1}", pageName, gridKey))
            If gridToken Is Nothing Then Exit Sub

            For Each col As DataControlField In gv.Columns
                Dim key As String = Nothing

                If TypeOf col Is BoundField Then
                    key = DirectCast(col, BoundField).DataField
                ElseIf TypeOf col Is TemplateField Then
                    key = col.HeaderText
                End If

                If String.IsNullOrWhiteSpace(key) Then Continue For

                Dim text = GetText(gridToken, key, lang, False)
                If text IsNot Nothing Then
                    col.HeaderText = text
                End If
            Next
        End Sub

    End Class


End Namespace
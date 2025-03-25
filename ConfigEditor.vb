Imports System
Imports System.IO
Imports System.Collections.Generic

Public Class ConfigEditor
    Private window_ As EditorWindow
    Private cursorX_ As Integer
    Private cursorY_ As Integer
    Private configData_ As Dictionary(Of String, String)
    Private configKeys_ As List(Of String)
    Private currentInputValue_ As String
    Private isEditingValue_ As Boolean

    Public Sub New(window As EditorWindow)
        Me.window_ = window
        Me.cursorX_ = 0
        Me.cursorY_ = 0
        Me.configData_ = New Dictionary(Of String, String)()
        Me.configKeys_ = New List(Of String)()
        Me.currentInputValue_ = ""
        Me.isEditingValue_ = False
    End Sub

    Public Sub Run()
        window_.SetTitle("Lexia Editor 3: Config Editor")
        While True
            window_.Clear()
            DrawConfig()
            window_.Draw()
            Dim input As Char = Console.ReadKey(True).KeyChar
            HandleInput(input)
            If input = "q"c Then Exit While
        End While
    End Sub

    Public Sub LoadConfig(filename As String)
        If Not File.Exists(filename) Then
            Console.Error.WriteLine("Error: Could not open config file: " & filename)
            Return
        End If

        configData_.Clear()
        configKeys_.Clear()
        Dim lines As String() = File.ReadAllLines(filename)
        For Each line As String In lines
            Dim parts As String() = line.Split("="c)
            If parts.Length = 2 Then
                configData_(parts(0)) = parts(1)
                configKeys_.Add(parts(0))
            End If
        Next
    End Sub

    Public Sub SaveConfig(filename As String)
        Using writer As New StreamWriter(filename)
            For Each key As String In configKeys_
                writer.WriteLine(key & "=" & configData_(key))
            Next
        End Using
    End Sub

    Private Sub HandleInput(input As Char)
        If isEditingValue_ Then
            If input = vbCr Then
                isEditingValue_ = False
            ElseIf input = vbBack Then
                If currentInputValue_.Length > 0 Then
                    currentInputValue_ = currentInputValue_.Substring(0, currentInputValue_.Length - 1)
                End If
            Else
                currentInputValue_ &= input
            End If
        Else
            Select Case input
                Case "w"c
                    If cursorY_ > 0 Then cursorY_ -= 1
                Case "s"c
                    If cursorY_ < configKeys_.Count - 1 Then cursorY_ += 1
                Case "e"c
                    If cursorY_ < configKeys_.Count Then
                        currentInputValue_ = configData_(configKeys_(cursorY_))
                        isEditingValue_ = True
                    End If
                Case "a"c
                    AddPair("new_key", "new_value")
            End Select
        End If

        If Not isEditingValue_ AndAlso configKeys_.Count > 0 AndAlso cursorY_ < configKeys_.Count Then
            configData_(configKeys_(cursorY_)) = currentInputValue_
        End If
    End Sub

    Private Sub AddPair(key As String, value As String)
        configData_(key) = value
        configKeys_.Add(key)
    End Sub

    Private Sub DrawConfig()
        Dim offsetX As Integer = 5
        Dim offsetY As Integer = 5

        For i As Integer = 0 To configKeys_.Count - 1
            Dim key As String = configKeys_(i)
            Dim value As String = configData_(key)
            If i = cursorY_ Then
                window_.SetPixel(offsetX, offsetY + i, "["c)
                window_.SetPixel(offsetX + key.Length + 1 + value.Length + 2, offsetY + i, "]"c)
            End If
            For x As Integer = 0 To key.Length - 1
                window_.SetPixel(offsetX + x + 1, offsetY + i, key(x))
            Next
            window_.SetPixel(offsetX + key.Length + 1, offsetY + i, "="c)
            For x As Integer = 0 To value.Length - 1
                window_.SetPixel(offsetX + key.Length + 2 + x, offsetY + i, value(x))
            Next
        Next

        If isEditingValue_ AndAlso configKeys_.Count > 0 AndAlso cursorY_ < configKeys_.Count Then
            Dim key As String = configKeys_(cursorY_)
            window_.SetPixel(offsetX + key.Length + 2 + configData_(key).Length, offsetY + cursorY_, "["c)
            window_.SetPixel(offsetX + key.Length + 2 + configData_(key).Length + currentInputValue_.Length + 1, offsetY + cursorY_, "]"c)
            For x As Integer = 0 To currentInputValue_.Length - 1
                window_.SetPixel(offsetX + key.Length + 2 + configData_(key).Length + 1 + x, offsetY + cursorY_, currentInputValue_(x))
            Next
        End If
    End Sub
End Class
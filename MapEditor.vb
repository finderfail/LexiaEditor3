Imports System
Imports System.Collections.Generic
Imports System.IO

Public Class MapEditor
    Private window_ As EditorWindow
    Private cursorX_ As Integer
    Private cursorY_ As Integer
    Private mapData_ As List(Of String)
    Private width_ As Integer
    Private height_ As Integer

    Public Sub New(window As EditorWindow)
        Me.window_ = window
        Me.cursorX_ = 0
        Me.cursorY_ = 0
        Me.mapData_ = New List(Of String)()
        Me.width_ = 0
        Me.height_ = 0
    End Sub

    Public Sub Run()
        window_.SetTitle("Map Editor [e - Place wall, r - Remove wall, q - Quit]")
        While True
            window_.Clear()
            DrawMap()
            window_.Draw()
            Dim input As Char = Console.ReadKey(True).KeyChar
            HandleInput(input)
            If input = "q"c Then Exit While
        End While
    End Sub

    Public Sub LoadMap(filename As String)
        If Not File.Exists(filename) Then
            Console.Error.WriteLine("Error: Could not open map file: " & filename)
            Return
        End If

        mapData_.Clear()
        Dim lines As String() = File.ReadAllLines(filename)
        For Each line As String In lines
            mapData_.Add(line)
        Next

        If mapData_.Count > 0 Then
            height_ = mapData_.Count
            width_ = mapData_(0).Length
            For Each row As String In mapData_
                If row.Length <> width_ Then
                    Console.Error.WriteLine("Error: inconsistent map width!")
                    Return
                End If
            Next
        Else
            Console.Error.WriteLine("Error: Map file is empty")
            width_ = 32
            height_ = 32
            For i As Integer = 0 To height_ - 1
                mapData_.Add(New String(" "c, width_))
            Next
        End If
    End Sub

    Public Sub SaveMap(filename As String)
        Using writer As New StreamWriter(filename)
            For Each line As String In mapData_
                writer.WriteLine(line)
            Next
        End Using
    End Sub

    Private Sub HandleInput(input As Char)
        Select Case input
            Case "w"c
                If cursorY_ > 0 Then cursorY_ -= 1
            Case "s"c
                If cursorY_ < height_ - 1 Then cursorY_ += 1
            Case "a"c
                If cursorX_ > 0 Then cursorX_ -= 1
            Case "d"c
                If cursorX_ < width_ - 1 Then cursorX_ += 1
            Case "e"c
                mapData_(cursorY_) = mapData_(cursorY_).Substring(0, cursorX_) & "#" & mapData_(cursorY_).Substring(cursorX_ + 1)
            Case "r"c
                mapData_(cursorY_) = mapData_(cursorY_).Substring(0, cursorX_) & " " & mapData_(cursorY_).Substring(cursorX_ + 1)
        End Select
    End Sub

    Private Sub DrawMap()
        Dim offsetX As Integer = (window_.GetWidth() - width_) / 2
        Dim offsetY As Integer = (window_.GetHeight() - height_) / 2
        For y As Integer = 0 To height_ - 1
            For x As Integer = 0 To width_ - 1
                If x = cursorX_ AndAlso y = cursorY_ Then
                    window_.SetPixel(offsetX + x, offsetY + y, "["c)
                    window_.SetPixel(offsetX + x + 1, offsetY + y, "]"c)
                    x += 1
                    Continue For
                End If
                window_.SetPixel(offsetX + x, offsetY + y, mapData_(y)(x))
            Next
        Next
    End Sub
End Class
Imports System
Imports System.IO

Public Class EditorWindow
    Private width_ As Integer
    Private height_ As Integer
    Private title_ As String
    Private buffer_ As Char()

    Public Sub New(width As Integer, height As Integer, title As String)
        Me.width_ = width
        Me.height_ = height
        Me.title_ = title
        Me.buffer_ = New Char(width_ * height_ - 1) {}
        Array.Clear(buffer_, 0, buffer_.Length)
    End Sub

    Public Sub Clear()
        Array.Clear(buffer_, 0, buffer_.Length)
    End Sub

    Public Sub Draw()
        Console.SetCursorPosition(0, 0)
        Console.WriteLine(title_)
        For y As Integer = 0 To height_ - 1
            For x As Integer = 0 To width_ - 1
                Console.Write(buffer_(y * width_ + x))
            Next
            Console.WriteLine()
        Next
    End Sub

    Public Sub SetTitle(title As String)
        Me.title_ = title
    End Sub

    Public Sub SetPixel(x As Integer, y As Integer, c As Char)
        If x >= 0 AndAlso x < width_ AndAlso y >= 0 AndAlso y < height_ Then
            buffer_(y * width_ + x) = c
        End If
    End Sub

    Public Function GetWidth() As Integer
        Return width_
    End Function

    Public Function GetHeight() As Integer
        Return height_
    End Function
End Class
Imports System
Imports System.IO
Imports System.Diagnostics

Public Class Compiler
    Public Sub New()
    End Sub

    Public Function CompileProject(projectDir As String) As Boolean
        Dim manager As New ProjectManager()

        Dim srcPath As String = "./" & projectDir & "/src"
        Dim buildPath As String = "./" & projectDir & "/build"
        Dim engineInc As String = "engine/include"
        Dim engineLib As String = "./lib/libengine.a"

        Dim command As String = ""
        If Environment.OSVersion.Platform = PlatformID.MacOSX Then
            command = "g++ " & srcPath & "/main.cpp " & engineLib & " -I" & engineInc & " -o " & buildPath & "/game" & " -F/Library/Frameworks -framework SDL2 -I/usr/local/Cellar/sdl2/2.30.11/include -L/usr/local/Cellar/sdl2/2.30.11/lib"
        ElseIf Environment.OSVersion.Platform = PlatformID.Unix Then
            command = "g++ " & srcPath & "/main.cpp " & engineLib & " -I" & engineInc & " -o " & buildPath & "/game" & " -lSDL2"
        End If

        Console.Error.WriteLine("Compilation command: " & command)
        Dim process As New Process()
        process.StartInfo.FileName = "sh"
        process.StartInfo.Arguments = "-c " & command
        process.StartInfo.RedirectStandardError = True
        process.StartInfo.UseShellExecute = False
        process.Start()

        Dim errorOutput As String = process.StandardError.ReadToEnd()
        process.WaitForExit()

        If process.ExitCode <> 0 Then
            Console.Error.WriteLine("Error: Compilation failed with exit code " & process.ExitCode)
            Console.Error.WriteLine(errorOutput)
            Return False
        End If

        Console.WriteLine("Success: Project has been compiled!")
        Return True
    End Function
End Class

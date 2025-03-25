Imports System
Imports System.Diagnostics
Imports System.IO

Module Main
    Sub Main()
        Dim screenWidth As Integer = 120
        Dim screenHeight As Integer = 30
        Dim window As New EditorWindow(screenWidth, screenHeight, "Lexia Editor 3")
        Dim projectManager As New ProjectManager()
        Dim compiler As New Compiler()
        Dim currentProjectDir As String = ""
        Dim input As String

        While True
            ClearScreen()
            Console.WriteLine("LexiaEditor 3")
            Console.WriteLine("Powered by Lexia Engine 2")
            Console.WriteLine("1. Create new project")
            Console.WriteLine("2. Load project")
            Console.WriteLine("3. Exit")
            Console.WriteLine("Enter your choice: ")

            input = Console.ReadLine()
            If input = "1" Then
                ClearScreen()
                Console.WriteLine("Enter the name of the new project:")
                Dim projectName As String = Console.ReadLine()
                If projectManager.CreateProject(projectName) Then
                    currentProjectDir = projectManager.GetCurrentProjectDir()
                    Console.WriteLine("Project created in " & currentProjectDir)
                    Exit While
                End If
            ElseIf input = "2" Then
                ClearScreen()
                Console.WriteLine("Enter the name of the project you want to load:")
                Dim projectName As String = Console.ReadLine()

                If Not File.Exists(projectName & "/project.conf") Then
                    Console.Error.WriteLine("Could not find project file!")
                    Continue While
                End If

                Dim line As String = File.ReadAllLines(projectName & "/project.conf")(0)

                If projectManager.LoadProject(line) Then
                    currentProjectDir = projectManager.GetCurrentProjectDir()
                    Console.WriteLine("Project loaded from " & currentProjectDir)
                    Exit While
                End If
            ElseIf input = "3" Then
                Return
            Else
                Console.Error.WriteLine("Invalid input!")
            End If
        End While

        While True
            ClearScreen()
            Console.WriteLine("1. Map Editor")
            Console.WriteLine("2. Config Editor")
            Console.WriteLine("3. Compile Project")
            Console.WriteLine("4. Run Project")
            Console.WriteLine("5. Back")
            Console.WriteLine("Enter your choice: ")
            input = Console.ReadLine()
            If input = "1" Then
                Dim mapEditor As New MapEditor(window)
                mapEditor.LoadMap(currentProjectDir & "/maps/map.txt")
                mapEditor.Run()
                mapEditor.SaveMap(currentProjectDir & "/maps/map.txt")
            ElseIf input = "2" Then
                Dim configEditor As New ConfigEditor(window)
                configEditor.LoadConfig(currentProjectDir & "/config/game.conf")
                configEditor.Run()
                configEditor.SaveConfig(currentProjectDir & "/config/game.conf")
            ElseIf input = "3" Then
                If Not compiler.CompileProject(currentProjectDir) Then
                    Console.Error.WriteLine("Error: Project did not compile!")
                End If
                Console.ReadKey()
            ElseIf input = "4" Then
                Dim gamePath As String = projectManager.GetGameBuildPath()
                Dim command As String = gamePath & "/game"
                Dim process As New Process()
                process.StartInfo.FileName = "sh"
                process.StartInfo.Arguments = "-c " & command
                process.StartInfo.RedirectStandardError = True
                process.StartInfo.UseShellExecute = False
                process.Start()

                Dim errorOutput As String = process.StandardError.ReadToEnd()
                process.WaitForExit()

                If process.ExitCode <> 0 Then
                    Console.Error.WriteLine("Error: Could not execute game!")
                    Console.Error.WriteLine(errorOutput)
                End If
                Console.ReadKey()
            ElseIf input = "5" Then
                Console.WriteLine("Editor will close.")
                Exit While
            Else
                Console.Error.WriteLine("Invalid input!")
            End If
        End While
    End Sub

    Sub ClearScreen()
        Console.Clear()
    End Sub
End Module
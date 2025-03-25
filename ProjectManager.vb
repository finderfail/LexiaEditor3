Imports System
Imports System.IO

Public Class ProjectManager
    Private currentProjectDir_ As String

    Public Sub New()
        Me.currentProjectDir_ = ""
    End Sub

    Public Function CreateProject(projectName As String) As Boolean
        currentProjectDir_ = "./" & projectName
        Dim gameSrcDir As String = currentProjectDir_ & "/src"
        Dim gameBuildDir As String = currentProjectDir_ & "/build"
        Dim mapDir As String = currentProjectDir_ & "/maps"
        Dim configDir As String = currentProjectDir_ & "/config"

        Console.Error.WriteLine("Creating root dir: " & currentProjectDir_)
        If Not Directory.Exists(currentProjectDir_) Then
            Directory.CreateDirectory(currentProjectDir_)
        End If

        Console.Error.WriteLine("Creating gameSrcDir: " & gameSrcDir)
        If Not Directory.Exists(gameSrcDir) Then
            Directory.CreateDirectory(gameSrcDir)
        End If

        Console.Error.WriteLine("Creating gameBuildDir: " & gameBuildDir)
        If Not Directory.Exists(gameBuildDir) Then
            Directory.CreateDirectory(gameBuildDir)
        End If

        Console.Error.WriteLine("Creating mapDir: " & mapDir)
        If Not Directory.Exists(mapDir) Then
            Directory.CreateDirectory(mapDir)
        End If

        Console.Error.WriteLine("Creating configDir: " & configDir)
        If Not Directory.Exists(configDir) Then
            Directory.CreateDirectory(configDir)
        End If

        SaveProjectConfig()

        Using mainFile As New StreamWriter(gameSrcDir & "/main.cpp")
            mainFile.WriteLine("#include ""../../engine/include/Screen.h""")
            mainFile.WriteLine("#include ""../../engine/include/Map.h""")
            mainFile.WriteLine("#include ""../../engine/include/Player.h""")
            mainFile.WriteLine("#include ""../../engine/include/Raycaster.h""")
            mainFile.WriteLine("#include ""../../engine/include/Config.h""")
            mainFile.WriteLine("#include <SDL.h>")
            mainFile.WriteLine("#define M_PI 3.14159265358979323846")
            mainFile.WriteLine("#include <iostream>")
            mainFile.WriteLine("#include <chrono>")
            mainFile.WriteLine("#include <thread>")
            mainFile.WriteLine("int main() {")
            mainFile.WriteLine("  Config config;")
            mainFile.WriteLine("  if (!config.load(""" & currentProjectDir_ & "/config/game.conf"")) {")
            mainFile.WriteLine("    return 1;")
            mainFile.WriteLine("  }")
            mainFile.WriteLine("  int screenWidth = config.getInt(""screenWidth"");")
            mainFile.WriteLine("  int screenHeight = config.getInt(""screenHeight"");")
            mainFile.WriteLine("  const char* gameName = ""Lexia Development Kit (LE2)"";")
            mainFile.WriteLine("  Screen screen(screenWidth, screenHeight, gameName);")
            mainFile.WriteLine("  Map map;")
            mainFile.WriteLine("  if (!map.load(""" & currentProjectDir_ & "/maps/map.txt"")) {")
            mainFile.WriteLine("    return 1;")
            mainFile.WriteLine("  }")
            mainFile.WriteLine("  float playerStartX = config.getInt(""playerStartX"");")
            mainFile.WriteLine("  float playerStartY = config.getInt(""playerStartY"");")
            mainFile.WriteLine("  float playerStartDir = config.getInt(""playerStartDir"") * M_PI / 180.0f;")
            mainFile.WriteLine("  Player player(playerStartX, playerStartY, playerStartDir);")
            mainFile.WriteLine("  Raycaster raycaster(map, screen);")
            mainFile.WriteLine("  bool isRunning = true;")
            mainFile.WriteLine("  SDL_Event event;")
            mainFile.WriteLine("  while (isRunning) {")
            mainFile.WriteLine("  while(SDL_PollEvent(&event))")
            mainFile.WriteLine("    {")
            mainFile.WriteLine("      if (event.type == SDL_QUIT)")
            mainFile.WriteLine("        isRunning = false;")
            mainFile.WriteLine("      if (event.type == SDL_KEYDOWN) {")
            mainFile.WriteLine("        float moveSpeed = 0.1f;")
            mainFile.WriteLine("        float rotateSpeed = 0.05f;")
            mainFile.WriteLine("        switch (event.key.keysym.sym) {")
            mainFile.WriteLine("          case SDLK_w:")
            mainFile.WriteLine("             player.move(moveSpeed, map);")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("          case SDLK_s:")
            mainFile.WriteLine("             player.move(-moveSpeed, map);")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("          case SDLK_a:")
            mainFile.WriteLine("             player.rotate(-rotateSpeed);")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("          case SDLK_d:")
            mainFile.WriteLine("             player.rotate(rotateSpeed);")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("          case SDLK_q:")
            mainFile.WriteLine("             isRunning = false;")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("          default:")
            mainFile.WriteLine("             break;")
            mainFile.WriteLine("        }")
            mainFile.WriteLine("      }")
            mainFile.WriteLine("  }")
            mainFile.WriteLine("  screen.clear(0x000000);")
            mainFile.WriteLine("  raycaster.castRays(player);")
            mainFile.WriteLine("  screen.render();")
            mainFile.WriteLine("  std::this_thread::sleep_for(std::chrono::milliseconds(16));")
            mainFile.WriteLine("      }")
            mainFile.WriteLine("  return 0;")
            mainFile.WriteLine("      }")
        End Using

        Using confFile As New StreamWriter(currentProjectDir_ & "/config/game.conf")
            confFile.WriteLine("screenWidth=1280")
            confFile.WriteLine("screenHeight=720")
            confFile.WriteLine("playerStartX=3.0")
            confFile.WriteLine("playerStartY=3.0")
            confFile.WriteLine("playerStartDir=90")
        End Using

        Using mapFile As New StreamWriter(currentProjectDir_ & "/maps/map.txt")
            mapFile.WriteLine("################################")
            mapFile.WriteLine("#                              #")
            mapFile.WriteLine("#                              #")
            mapFile.WriteLine("#                              #")
            mapFile.WriteLine("#  #####                       #")
            mapFile.WriteLine("#  #   #                       #")
            mapFile.WriteLine("#  #   #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("#      #                       #")
            mapFile.WriteLine("################################")
        End Using

        Return True
    End Function

    Public Function LoadProject(projectName As String) As Boolean
        currentProjectDir_ = projectName
        Return True
    End Function

    Public Sub SetCurrentProjectDir(dir As String)
        currentProjectDir_ = dir
    End Sub

    Public Function GetCurrentProjectDir() As String
        Return currentProjectDir_
    End Function

    Public Function GetGameSourcePath() As String
        Return currentProjectDir_ & "/src"
    End Function

    Public Function GetGameBuildPath() As String
        Return currentProjectDir_ & "/build"
    End Function

    Public Sub SaveProjectConfig()
        Using writer As New StreamWriter(currentProjectDir_ & "/project.conf")
            writer.WriteLine(currentProjectDir_)
        End Using
    End Sub
End Class
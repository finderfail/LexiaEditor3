#include "../../engine/include/Screen.h"
#include "../../engine/include/Map.h"
#include "../../engine/include/Player.h"
#include "../../engine/include/Raycaster.h"
#include "../../engine/include/Config.h"
#include <SDL.h>
#define M_PI 3.14159265358979323846
#include <iostream>
#include <chrono>
#include <thread>
int main() {
  Config config;
  if (!config.load("./Emae/config/game.conf")) {
    return 1;
  }
  int screenWidth = config.getInt("screenWidth");
  int screenHeight = config.getInt("screenHeight");
  const char* gameName = "Lexia Development Kit (LE2)";
  Screen screen(screenWidth, screenHeight, gameName);
  Map map;
  if (!map.load("./Emae/maps/map.txt")) {
    return 1;
  }
  float playerStartX = config.getInt("playerStartX");
  float playerStartY = config.getInt("playerStartY");
  float playerStartDir = config.getInt("playerStartDir") * M_PI / 180.0f;
  Player player(playerStartX, playerStartY, playerStartDir);
  Raycaster raycaster(map, screen);
  bool isRunning = true;
  SDL_Event event;
  while (isRunning) {
  while(SDL_PollEvent(&event))
    {
      if (event.type == SDL_QUIT)
        isRunning = false;
      if (event.type == SDL_KEYDOWN) {
        float moveSpeed = 0.1f;
        float rotateSpeed = 0.05f;
        switch (event.key.keysym.sym) {
          case SDLK_w:
             player.move(moveSpeed, map);
             break;
          case SDLK_s:
             player.move(-moveSpeed, map);
             break;
          case SDLK_a:
             player.rotate(-rotateSpeed);
             break;
          case SDLK_d:
             player.rotate(rotateSpeed);
             break;
          case SDLK_q:
             isRunning = false;
             break;
          default:
             break;
        }
      }
  }
  screen.clear(0x000000);
  raycaster.castRays(player);
  screen.render();
  std::this_thread::sleep_for(std::chrono::milliseconds(16));
      }
  return 0;
      }

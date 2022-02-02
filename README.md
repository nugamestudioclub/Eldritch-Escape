# blocks-horror
One of Northeastern Game Studio's submissions for Coding Blocks Game Jam 2022

## Game Description: 
You are a worker in a glass factory, who came to the office on an off-day to check up on a strange work request put in from one of your supervisors anonymously. You reach your office to check the request out when suddenly the lights turn off. Nearby, you hear the sound of glass shattering...

Wander through a maze of dark corridors and find the missing gems you need to unlock the door. And avoid crossing paths with the monster that lurks in the halls!

## Project Structure
- The following folders are all part of the project's **Assets** folder.
- The game runs on the Universal Render Pipeline for Unity, however no URP settings are included in this repository.
- Do keep in mind, this also uses the URP 2D Experimental rendering system, not the standard URP 3D games.

### Animations
- Animation controllers + Animation Objects used in the game

### Fog of War Package:
- Two shaders written in HLSL to manage the fog of war
  - Shaders make parts of objects not rendered when not lit by a 2D light.

### Font:
- Font used

### URP Assets:
- Pipeline assets used to configure the rendering pipeline.

### Prefabs:
- Scnene Objects used and spawned in throughout the game's level + world.

### Scenes:
- List of Scene assets used
  - Currently 2 scenes: Main menu and Level (Sample Scene)

### Scripts:
- Folder contains all scripts used in the game.
- Menu Script.cs is used for Main Menu UI
- NAV2D sub folder is never used.

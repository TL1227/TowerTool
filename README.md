# Tower Tool
<img src="README/TowerTool.png" alt="drawing" width="600"/>  

Everything you need to craft the perfect map for [TowerRPG](https://github.com/TL1227/TowerRPG)

## Features
- Map editor built using WPF
- Edit map files using a brush palette
- Save/Load files
- Launch the Tower RPG project directly from the editor
- Live Edit Mode, edit the map in real time while playing the game
- Player's in game position appears on your map in Live Edit Mode

## Map Format
Maps are stored as plain text files — one character per tile.  
I wanted the format to be human readable so you could use just a text editor to build levels.  
Sharing maps with friends would just be copy pasting into a chat window.  
Plus making a site that hosts custom maps would also be trivial. 

## Key
| Character | Tile  |
|-----------|-------|
| #         | Wall  |
| (space)   | Floor |
| c         | Chest |
| s         | Player Start Location |

Example:
```
      ###########
      #c        #
      #   ###   #
######          #
s               #
######          #
      #   ###   #
      #         #
      ###########

```
_This is the file shown in the screenshot above_

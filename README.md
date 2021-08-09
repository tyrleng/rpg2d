# What this is about
Attempt to recreate some functionality from 2d tactical RPGs like Fire Emblem.  
Not sure what the original games used, but I started with Unity and C# since those are the most accessible.

Done out of nostalgia and curiousity at how game development might be like.  

Currently on hold. Life in my day job picked up and I can't concentrate on this project.  

# Character Tile Movement System

I tried recreating the tile movement system from tactical RPGs. In such games, characters have only so many squares they can move over per turn (say 5 or 6).  

Before anything can be done, the first problem was determining which tile square the character is standing. It was surprisingly tough, since Unity tracks character positions based on screen pixel coordinates and not by tile position on the tilemap! Had to do some coordinate system mapping voodoo.  

The next problem was calculating all the valid squares that the character can move to. The valid squares would then be highlighted in the magenta colour you see. The term for this problem is floodfilling, though my algorithm implementation probably leaves much to be desired :upside_down_face:.  

The next problem is in determining the path to the square selected. This step doesn't even require an explicit algorithm. During the floodfilling tile finding process, we'd just need each tile to remember which other tile chose to explore the former tile. Based on this simple data, we can construct a valid path for the character.

The last problem is to allow the character to visually move to the selected square, provided the user clicks in a valid square. The problem here is with getting Unity to slow the action down. Unity's update loops are so fast that even if your character is following a path through all the squares, the character still seems to instantaneously teleport. Luckily, Unity offers coroutine features that really help out in this problem.  

![](https://media.giphy.com/media/Jt2v2hmRvtQN8GgI6h/giphy.gif)

## AI Decision Making

Wanted to design the AI system using Goal-Driven Agents.  
Unfortunately, pursuing even simplistic AI will take a lot of time I don't have during my full time day job.  

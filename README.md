# What this is about
Attempt to recreate some functionality from 2d tactical RPGs like Fire Emblem.

# Attempts made

## Character Tile Movement Controller

I tried recreating the tile movement system from tactical RPGs. Characters have only so many squares they can move per turn (say 5 or 6).
Based on this limit, the first problem was calculating all the valid squares that the character can move to, then highlighting those valid squares. The term for this problem is floodfilling, though my algorithm implementation probably leaves much to be desired :laugh.   
Determining which square the character is standing on can be tricky, because Unity tracks character positions based on screen pixel coordinates and not by tile position on the tilemap!  

The next problem is in determining the path to the square selected, although the logic can be combined with the floodfilling process.

The last problem is to allow the character to visually move to the selected square, provided the user clicks in a valid square. The problem here is with getting Unity to slow the action down. Unity's update loops are so fast that even if your character is following a path, it'll still seem to teleport. Luckily,Unity offers coroutine features that really help out.  

![](https://media.giphy.com/media/Jt2v2hmRvtQN8GgI6h/giphy.gif)

## AI Decision Making

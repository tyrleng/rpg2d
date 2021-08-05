using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CharacterControllers;

/*
* Given a point and a number of movement spaces, what are all the squares that this unit can reach?
* Given two points, how can a unit go from one point to another?
 */
public class TileMovement : MonoBehaviour
{
    private PlayerController _controller;
    public int halfRange;
    private Dictionary<Vector3Int, WorldTile> _traversableTiles;

    // input lag needed to prevent the update code from executing immediately
    // inspiration was from Ruby's Adventure, but still not sure if hard coding input lag is the way to solve the problem.
    private bool _inputLag;
    private float _inputLagTime = 0.25f;

    //Starter method that gets all the FloodFill methods running.
    private void OnMouseDown()
    {
        FloodFill();
    }    

    void Start()
    {
        _traversableTiles = new Dictionary<Vector3Int, WorldTile>();
        _controller = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (_inputLag)
        {
            _inputLagTime -= Time.deltaTime;
            if (_inputLagTime < 0.0f)
            {
                _inputLagTime = 0.25f;
                _inputLag = false;
            }
        }
        
        // input lag is needed to prevent this code block from being called the same time as FloodFill.
        // controller.selected is a custom variable checking whether this character had it's FloodFill method run.
        // GetMouseButtonDown checks whether the player has clicked a spot where the character might move to.
        if (!_inputLag && _controller.Selected && Input.GetMouseButtonDown(0))
        {
            bool result = CheckMouseClickInRange();
            if (result)
            {
                // proceed with movement
                FloorFillMove();
            }
            if (!result)
            {
                FloodFillClear();
            }
            // reset states
            foreach (var tileKey in _traversableTiles)
            {
                WorldTile tile = tileKey.Value;
                tile.MovementNumber = 0;
                tile.ExploredFrom = null;
            }
            // impt that this statement comes only after all the tiles have been reset!
            _traversableTiles = new Dictionary<Vector3Int, WorldTile>();
            if (!_controller.Moving)
            {
                //resetting the selected status, since any mouse click will meant the character is not selected anymore regardless what action happens. Will also help switch back animation
                _controller.Selected = false;
            }
           
        }
    }
    
    private void FloodFill()
        // Find all the squares that the character can traverse over
    {
        Queue<WorldTile> tilesToExplore = new Queue<WorldTile>();
       /*
        * Current tile begins with the range.
        * For each tile, get the position of the adjacent 4 tiles.
        * For each adjacent tile, calculate the current tile range less the adjacent tile cost.
        * If calculated range < 1, give up on the adjacent tile, the adjacent tile will not be traversable.
        * If the adjacent tile range not assigned or has range greater than calculated tile range, assign the calculated range to the adjacent tile.
        * If previous statement true, assign to the adjacent tile the current tile as the the tile travelled from
        */
       Vector3Int position = Vector3Int.FloorToInt(gameObject.transform.position);
        Debug.Log("Flood Fill: Character's position is " + position);
        WorldTile characterTile = null;
        characterTile = TilemapController.Tiles[position];

        // The +1 is to compensate for the later deduction
        characterTile.MovementNumber = halfRange + 1;
        
        tilesToExplore.Enqueue(characterTile);
        while (tilesToExplore.Count != 0)
        {
            WorldTile currentTileExplored = tilesToExplore.Dequeue();
            // checking if the tile has not been added to the traversable dictionary yet. If has been added before, don't try to re-add.
            if (!_traversableTiles.ContainsKey(currentTileExplored.WorldLocation))
            {
                _traversableTiles.Add(currentTileExplored.WorldLocation, currentTileExplored);
            }
            WorldTile[] adjacentTiles = GetAdjacentTiles(currentTileExplored.WorldLocation);
            for (int i = 0; i < 4; i++)
            {
                WorldTile adjacentTile = adjacentTiles[i];
                // Must check, because the adjacent tiles really can be null if near the border of the map.
                if (adjacentTile == null)
                    continue;
                int adjMovementNumber = currentTileExplored.MovementNumber - 1 - adjacentTile.Penalty;
                if (adjMovementNumber > adjacentTile.MovementNumber && adjMovementNumber > 0)
                {
                    adjacentTile.MovementNumber = adjMovementNumber;
                    adjacentTile.ExploredFrom = currentTileExplored;
                    tilesToExplore.Enqueue(adjacentTile);
                }
            }
        }
        // tinting the traversable tiles
        foreach (var tile in _traversableTiles)
        {
            Tilemap tilemap = tile.Value.Tilemap;
            tilemap.SetTileFlags(tile.Key, TileFlags.None);
            // Tint to shade of magenta
            // note that SetColor() operates on cell space, whilst you've been using WorldSpace. So be careful if WorldSpace and CellSpace are different.
            tilemap.SetColor(tile.Key, new Color(1,0,1,1));
        }
        _controller.Selected = true;
        _inputLag = true;
    }
    
    private void FloodFillClear()    
    {
        foreach (var tileKey in _traversableTiles)
        {
            //having no colour but being opaque will merely display the original sprite
            tileKey.Value.Tilemap.SetColor(tileKey.Key, new Color(0,0,0,1));
        }
    }

    private void FloorFillMove()
    {
        // Need to let the animator know that the character is moving.
        _controller.Moving = true;
        // Find the tile that the character is moving to
        Camera main = Camera.main;
        Vector3Int mousePos = Vector3Int.FloorToInt(main.ScreenToWorldPoint(Input.mousePosition));
        mousePos.z = 0;
        WorldTile selectedTile = _traversableTiles[mousePos];
        
        // Figure out the path to the selected tile
        Stack<WorldTile> tileMovementStack = new Stack<WorldTile>();
        WorldTile exploredFrom = selectedTile.ExploredFrom;
        while (exploredFrom != null)
        {
            tileMovementStack.Push(selectedTile);
            selectedTile = exploredFrom;
            exploredFrom = selectedTile.ExploredFrom;
        }
        
        // Make the character move to the selected tile
        StartCoroutine(StaggeredMove(tileMovementStack));
        
        // get rid of the tile tint
        foreach (var tileKey in _traversableTiles)
        {
            tileKey.Value.Tilemap.SetColor(tileKey.Key, new Color(1,1,1, 1));
        }
    }    

    private IEnumerator StaggeredMove(Stack<WorldTile> tileMovementStack)
    {
        while (tileMovementStack.Count != 0)
        {
            WorldTile targetTile = tileMovementStack.Pop();
            Vector3Int currentTilePos = Vector3Int.FloorToInt(gameObject.transform.position);
            Vector3Int movementVector = targetTile.WorldLocation - currentTilePos;
            _controller.MovementX = movementVector.x;
            _controller.MovementY = movementVector.y;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movementVector, 1);
            yield return new WaitForSeconds(0.25f);
        }
        // once movement is done, need to let the animator know.
        _controller.Moving = false;
    }

    private bool CheckMouseClickInRange()
    {
        Camera main = Camera.main;
        Debug.Log("Input Mouse Position in Screen Coor is " + Input.mousePosition);
        Vector3Int mousePos = Vector3Int.FloorToInt(main.ScreenToWorldPoint(Input.mousePosition));
        mousePos.z = 0;
        Debug.Log("Mouse Position in World Coor is " + mousePos);
        foreach (var tileKey in _traversableTiles)
        {
            if (mousePos.Equals(tileKey.Key))
            {
                return true;
            }
        }
        return false;
    }
    
    // NOTE: This method can throw KeyNotFound Exceptions from the Dictionary. The calling method must catch the exception.
    private WorldTile[] GetAdjacentTiles(Vector3Int currentTilePos)
    {

        WorldTile leftTile;
        WorldTile rightTile;
        WorldTile upTile;
        WorldTile downTile;
        
        try
        {
            Vector3Int leftTilePos = new Vector3Int(currentTilePos.x - 1, currentTilePos.y, currentTilePos.z);
             leftTile = TilemapController.Tiles[leftTilePos];
        }
        catch (KeyNotFoundException)
        {
            leftTile = null;
        }
        
        try
        {
            Vector3Int rightTilePos = new Vector3Int(currentTilePos.x + 1, currentTilePos.y, currentTilePos.z);
            rightTile = TilemapController.Tiles[rightTilePos];
        }
        catch (KeyNotFoundException)
        {
            rightTile = null;
        }

        try
        {
            Vector3Int upTilePos = new Vector3Int(currentTilePos.x, currentTilePos.y + 1, currentTilePos.z);
            upTile = TilemapController.Tiles[upTilePos];
        }
        catch (KeyNotFoundException)
        {
            upTile = null;
        }

        try
        {
            Vector3Int downTilePos = new Vector3Int(currentTilePos.x, currentTilePos.y - 1, currentTilePos.z);
            downTile = TilemapController.Tiles[downTilePos];
        }

        catch (KeyNotFoundException)
        {
            downTile = null;
        }
        return new WorldTile[]{leftTile,rightTile,upTile,downTile};
    }
    

    // private BoundsInt _traversableSizeDeprecated;
    // public Tilemap tilemapDeprecated;
    //
    // private void FloodFillBaseDepre(Color color)
    // {
    //     // Debug.Log("Entered Flood Fill Base");
    //     Vector3Int position = Vector3Int.FloorToInt(gameObject.transform.position);
    //     int midX = position.x;
    //     int midY = position.y;
    //     for (int x = midX - halfRange; x < midX + halfRange + 1; x++)
    //     {
    //         for (int y = midY - halfRange; y < midY + halfRange + 1; y++)
    //         {
    //             Vector3Int tilePosition = new Vector3Int(x,y,0);
    //             tilemapDeprecated.SetTileFlags(tilePosition, TileFlags.None);
    //             tilemapDeprecated.SetColor(tilePosition, color);
    //         }
    //     }
    //     Vector3Int minPos = new Vector3Int(midX - halfRange, midY - halfRange, 0);
    //     Vector3Int maxPos = new Vector3Int(midX + halfRange + 1, midY + halfRange + 1, 0);
    //     _traversableSizeDeprecated = new BoundsInt();
    //     _traversableSizeDeprecated.SetMinMax(minPos, maxPos);
    //     _selected = true;
    //     _inputLag = true;
    // }
    //
    // private bool CheckMouseClickInRangeDeprecated()
    // {
    //     bool result = false;
    //     Camera main = Camera.main;
    //     Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
    //     float mousePosX = mousePos.x;
    //     float mousePosY = mousePos.y;
    //     if (mousePosX - _traversableSizeDeprecated.xMin > 0.0f && mousePosY - _traversableSizeDeprecated.yMin > 0.0f)
    //     {
    //         if (mousePosX - _traversableSizeDeprecated.xMax < 0.0f && mousePosY - _traversableSizeDeprecated.yMax < 0.0f)
    //         {
    //             result = true;
    //         }
    //     }
    //     // Regardless whether the click is within the box, a click has been made, so the character will be deselected.
    //     return result;
    // }
}

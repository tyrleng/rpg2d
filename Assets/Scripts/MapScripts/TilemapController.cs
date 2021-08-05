using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

    public class TilemapController : MonoBehaviour
    {
        public List<Tilemap> tilemaps;

        public static Dictionary<Vector3Int, WorldTile> Tiles { get; private set; }

        private void Awake()
        {
            InitWorldTiles();
        }

        private void InitWorldTiles()
        {
            Tiles = new Dictionary<Vector3Int, WorldTile>();
            foreach (Tilemap tilemap in tilemaps)
            {
                foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
                {
                    Vector3Int cellPlace = new Vector3Int(pos.x, pos.y, pos.z);
                    if (!tilemap.HasTile(cellPlace))
                        continue;
                    WorldTile tile = new WorldTile
                    {
                        CellLocation = cellPlace,
                        // the FloorToInt method is really important. That's how you'll match a mouse click with the tile the mouse clicked on
                        WorldLocation = Vector3Int.FloorToInt(tilemap.CellToWorld(cellPlace)),
                        Tilemap = tilemap,
                        OrderInLayer = tilemap.GetComponent<Renderer>().sortingOrder
                    };
                    try
                    {
                        Tiles.Add(tile.WorldLocation, tile);
                    }
                    //A tile might already exist at the WorldLocation - a fault of the map designer.
                    //Deal with this by choosing the tile with highest order in layer to be rendered.
                    catch (ArgumentException argumentException)
                    {
                        WorldTile previousTile = Tiles[tile.WorldLocation];
                        if (tile.OrderInLayer > previousTile.OrderInLayer)
                            Tiles[tile.WorldLocation] = tile;
                    }
                }
            }
        }
    }

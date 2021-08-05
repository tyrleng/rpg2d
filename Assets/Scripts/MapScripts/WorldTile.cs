using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile 
{
    public enum PenaltyType
    {
        Normal = 0,
        Mud = 1,
        Sand = 2,
        Untraversable = int.MaxValue
    }
    public Vector3Int CellLocation { get; set; }

    public Vector3Int WorldLocation { get; set; }

    private Tilemap _tilemap;
    public Tilemap Tilemap
    {
        get => _tilemap;
        set
        {
            _tilemap = value;
            CalcPenalty();
        }
    }
    // how many movement range tokens traversing this tile will take.
    public int Penalty { get; private set; }
    
    public int MovementNumber { get; set; }

    public WorldTile ExploredFrom { get; set; }
    
    public int OrderInLayer { get; set; }

    private void CalcPenalty()
    {
        if (_tilemap.CompareTag("Untraversable"))
            Penalty = int.MaxValue;
        else Penalty = 0;
    }

}

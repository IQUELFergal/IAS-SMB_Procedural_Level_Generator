﻿using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Chunk
{
    Vector2Int size;
    protected TileBase[][,] tileGrid;
    private int tilemapCount;

    public TileBase[][,] TileGrid { get => tileGrid; set => tileGrid = value; }
    public int TilemapCount { get => tilemapCount; set => tilemapCount = value; }
    public Vector2Int Size { get => size; set => size = value; }

    public abstract void GenerateChunkGrid();
}

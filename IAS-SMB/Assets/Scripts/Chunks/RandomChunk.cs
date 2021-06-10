using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomChunk : Chunk
{
    public List<ChunkElement> chunkElements;
    public int startGroundHeight;
    public int endGroundHeight;
    

    public override void GenerateChunkGrid()
    {
        foreach (var gameElem in chunkElements)
        {
            for (int x = gameElem.Rect.x; x < gameElem.Rect.x + gameElem.Rect.width; x++)
            {
                for (int y = gameElem.Rect.y; y < gameElem.Rect.y + gameElem.Rect.height; y++)
                {
                    TileGrid[gameElem.TilemapIndex][x, y] = gameElem.Tile;
                }
            }
        }
    }

    public RandomChunk(Vector2Int size, int groundHeight, List<ChunkElement> list)
    {
        chunkElements = list;
        Size = size;
        startGroundHeight = endGroundHeight = groundHeight;
        int maxIndex = 0;
        for (int i = 0; i < chunkElements.Count; i++)
        {
            if (maxIndex < chunkElements[i].TilemapIndex) maxIndex = chunkElements[i].TilemapIndex;
        }
        TilemapCount = maxIndex + 1;
        TileGrid = new TileBase[TilemapCount][,];
        for (int i = 0; i < TileGrid.Length; i++)
        {
            TileGrid[i] = new TileBase[Size.x, Size.y];
        }
        GenerateChunkGrid();
    }

    public RandomChunk(Vector2Int size, int startGndHeight, int endGndHeight, List<ChunkElement> list)
    {
        chunkElements = list;
        startGroundHeight = startGndHeight;
        endGroundHeight = endGndHeight;
        Size = size;
        int maxIndex = 0;
        for (int i = 0; i < chunkElements.Count; i++)
        {
            if (maxIndex < chunkElements[i].TilemapIndex) maxIndex = chunkElements[i].TilemapIndex;
        }
        TilemapCount = maxIndex + 1;
        TileGrid = new TileBase[TilemapCount][,];
        for (int i = 0; i < TileGrid.Length; i++)
        {
            TileGrid[i] = new TileBase[Size.x, Size.y];
        }
        GenerateChunkGrid();
    }
}

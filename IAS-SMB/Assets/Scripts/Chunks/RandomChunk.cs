using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomChunk : Chunk
{
    public List<GameElement> gameElements;

    

    public override void GenerateChunkGrid()
    {
        foreach (var gameElem in gameElements)
        {
            for (int x = gameElem.rect.x; x < gameElem.rect.x + gameElem.rect.width; x++)
            {
                for (int y = gameElem.rect.y; y < gameElem.rect.y + gameElem.rect.height; y++)
                {
                    TileGrid[gameElem.tilemapIndex][x, y] = gameElem.data.Tile;
                }
            }
        }
    }

    public RandomChunk(Vector2Int size, List<GameElement> list)
    {
        gameElements = list;
        Size = size;
        int maxIndex = 0;
        for (int i = 0; i < gameElements.Count; i++)
        {
            if (maxIndex < gameElements[i].tilemapIndex) maxIndex = gameElements[i].tilemapIndex;
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

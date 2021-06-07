using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameElement
{
    public int tilemapIndex;
    public RectInt rect;
    public GameElementData data;

    public GameElement(int tileMapIndex, Vector2Int chunkSize, Vector2Int position, GameElementData ged)
    {
        tilemapIndex = tileMapIndex;
        data = ged;
        rect = new RectInt(position.x, position.y, Mathf.Clamp(ged.Size.x, 0, chunkSize.x - position.x), Mathf.Clamp(ged.Size.y, 0, chunkSize.y - position.y));       
    }

    //Creates a GameElement and override its size attribute
    public GameElement(int tileMapIndex, Vector2Int chunkSize, Vector2Int position, Vector2Int size, GameElementData ged)
    {
        tilemapIndex = tileMapIndex;
        data = ged;
        rect = new RectInt(position.x, position.y, Mathf.Clamp(size.x, 0, chunkSize.x - position.x), Mathf.Clamp(size.y, 0, chunkSize.y - position.y));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkElement
{
    static Vector2Int chunkSize;
    int tilemapIndex;
    TileBase tile;
    RectInt rect;

    public TileBase Tile { get => tile;}
    public RectInt Rect { get => rect;}
    public int TilemapIndex { get => tilemapIndex;}


    public ChunkElement(int tileMapIndex, TileBase t, Vector2Int position, Vector2Int size)
    {
        tilemapIndex = tileMapIndex;
        tile = t;
        rect = new RectInt(position.x, position.y, Mathf.Clamp(size.x, 0, chunkSize.x - position.x), Mathf.Clamp(size.y, 0, chunkSize.y - position.y));
    }

    public ChunkElement(int tileMapIndex, TileBase t, RectInt r)
    {
        tilemapIndex = tileMapIndex;
        tile = t;
        rect = new RectInt(r.x, r.y, Mathf.Clamp(r.width, 0, chunkSize.x - r.x), Mathf.Clamp(r.height, 0, chunkSize.y - r.y));
    }

    public static void SetChunkSize(Vector2Int cs) => chunkSize = cs;

    public bool isOverlapping(List<ChunkElement> elements)
    {
        foreach (var elem in elements)
        {
            if (rect.Overlaps(new RectInt(elem.Rect.x-1, elem.Rect.y, elem.Rect.width + 2, elem.Rect.height)))
            {
                return true;
            }
        }
        return false;
    }

    public bool isOverlapping(List<RectInt> rects)
    {
        foreach (var rect in rects)
        {
            if (rect.Overlaps(new RectInt(rect.x, rect.y, rect.width, rect.height)))
            {
                return true;
            }
        }
        return false;
    }

}

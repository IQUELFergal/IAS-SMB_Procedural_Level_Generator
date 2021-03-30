using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ColorToTileBase
{
    [SerializeField] Color color = new Color(56,56,56,0);
    [SerializeField] TileBase tile;

    public ColorToTileBase(Color c)
    {
        Color = c;
    }

    public ColorToTileBase(Color c, TileBase t)
    {
        Color = c;
        Tile = t;
    }

    public Color Color { get => color; set => color = value; }
    public TileBase Tile { get => tile; set => tile = value; }
}

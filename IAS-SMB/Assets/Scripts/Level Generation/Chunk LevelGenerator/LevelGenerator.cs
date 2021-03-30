using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] LevelStyle style;


    public LevelStyle Style { get => style; set => style = value; }

    public Color[,] GenerateLevel()
    {
        if (style != null)
        {
            int size;
            switch (style.levelSizeSelector)
            {
                default:
                    Debug.LogError("No level style found : creating empty level...");
                    return null;
                case LevelStyle.LevelSizeSelector.Fixed:
                    size = style.levelSize;
                    break;
                case LevelStyle.LevelSizeSelector.RandomRange:
                    size = Random.Range(style.minLevelSize,style.maxLevelSize); 
                    break;
            }
            Color[,] level = new Color[size * style.chunkSize.x, style.chunkSize.y];

            for (int i = 0; i < size; i++)
            {
                int randomIndex = Random.Range(0, style.randomChunkSprites.width / style.chunkSize.x);
                for (int x = 0; x < style.chunkSize.x; x++)
                {
                    for (int y = 0; y < style.chunkSize.y; y++)
                    {
                        if (style.useStartChunks && i == 0)
                        {
                            level[x + style.chunkSize.x * i, y] = style.startChunkSprites.GetPixel(x, y);
                        }
                        else if (style.useEndChunks && i == size - 1)
                        {
                            level[x + style.chunkSize.x * i, y] = style.endChunkSprites.GetPixel(x, y);
                        }
                        else level[x + style.chunkSize.x * i, y] = style.randomChunkSprites.GetPixel(x + randomIndex * style.chunkSize.x, y);
                    }
                }
            }
            return level;
        }
        else
        {
            Debug.LogError("No level style found.");
            return null;
        }
        
    }


    public TileBase[,] ToTileArray(Color[,] colors)
    {
        TileBase[,] level = new TileBase[colors.GetLength(0), colors.GetLength(1)];
        if (style != null)
        {
            for (int y = 0; y < level.GetLength(1); y++)
            {
                for (int x = 0; x < level.GetLength(0); x++)
                {
                    for (int i = 0; i < style.randomChunkTileLibrary.Length; i++)
                    {
                        if (colors[x, y] == style.randomChunkTileLibrary[i].Color)
                        {
                            level[x, y] = style.randomChunkTileLibrary[i].Tile;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No level style found : converting into an empty level...");
        }
        return level;
    }

}

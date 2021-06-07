using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SeedInitializer))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] LevelStyle style;
    [SerializeField] GameElementData platform;
    [SerializeField] GameElementData gap;
    [SerializeField] GameElementData cannon;

    Grid grid;
    SeedInitializer seedInitializer;

    public LevelStyle Style { get => style; set => style = value; }

    public Chunk[] level;

    public void CreateLevel()
    {
        seedInitializer = GetComponent<SeedInitializer>();
        seedInitializer.InitSeed();
        Debug.Log("Creating level with seed " + seedInitializer.Seed);
        int size;
        switch (style.levelSizeSelector)
        {
            default:
                return;
            case SizeSelector.Fixed:
                size = style.levelSize;
                break;
            case SizeSelector.RandomRange:
                size = Random.Range(style.minLevelSize, style.maxLevelSize);
                break;
        }

        level = new Chunk[size];
        for (int i = 0; i < level.Length; i++)
        {
            level[i] = CreateChunk(i);
        }

        //TilemapGenerator tilemapGenerator = new TilemapGenerator();
        TilemapGenerator tilemapGenerator = GetComponent<TilemapGenerator>();
        tilemapGenerator.GenerateTilemap(level, -style.chunkSize.x/2, -style.chunkSize.y/2);
    }


    public Chunk CreateChunk(int chunkIndex)
    {
        List<GameElement> gameElements = new List<GameElement>();
        var platformElement = new GameElement(0, style.chunkSize, new Vector2Int(0, 0), platform);
        gameElements.Add(platformElement);
        gameElements.Add(new GameElement(0, style.chunkSize, new Vector2Int(Random.Range(0, style.chunkSize.x), platformElement.rect.height), cannon));
        gameElements.Add(new GameElement(0, style.chunkSize, new Vector2Int(5, 0), gap));


        Chunk chunk = new RandomChunk(style.chunkSize, gameElements);
        return chunk;
    }

    
    public Color[,] GenerateLevel()
    {
        if (style != null)
        {
            int size;
            switch (style.levelSizeSelector)
            {
                default:
                    return null;
                case SizeSelector.Fixed:
                    size = style.levelSize;
                    break;
                case SizeSelector.RandomRange:
                    size = Random.Range(style.minLevelSize, style.maxLevelSize);
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
                        else
                        {
                            switch (style.levelChunkType)
                            {
                                default:
                                    break;
                                case LevelStyle.LevelChunkType.GenerateRandomChunk:

                                    break;
                                case LevelStyle.LevelChunkType.UseChunkAtlas:
                                    level[x + style.chunkSize.x * i, y] = style.randomChunkSprites.GetPixel(x + randomIndex * style.chunkSize.x, y);
                                    break;
                            }
                        }
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



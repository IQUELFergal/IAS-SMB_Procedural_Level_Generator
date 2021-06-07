using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelStyle", menuName = "LevelGenerator/Level Style", order = 1)]
public class LevelStyle : ScriptableObject
{
    public enum LevelChunkType { UseChunkAtlas, GenerateRandomChunk}

    public SizeSelector levelSizeSelector;
    public int tilemapCount;
    public int levelSize;
    public int minLevelSize;
    public int maxLevelSize;

    public Vector2Int chunkSize = Vector2Int.one;
    public LevelChunkType levelChunkType;

    //public bool useStartEndChunks = false;
    public bool useStartChunks = false;
    public bool useEndChunks = false;
    //public Texture2D startEndChunkSprites;
    //public ColorToTileBase[] startEndChunkTileLibrary = null;
    
    public Texture2D startChunkSprites;
    public ColorToTileBase[] startChunkTileLibrary = null;

    public Texture2D randomChunkSprites;
    public ColorToTileBase[] randomChunkTileLibrary = null;

    public Texture2D endChunkSprites;
    public ColorToTileBase[] endChunkTileLibrary = null;

    private void OnValidate()
    {
        if (randomChunkSprites != null && (randomChunkSprites.width % chunkSize.x != 0 || randomChunkSprites.height % chunkSize.y != 0))
        {
            Debug.LogError("The texture size is not matching with the chunk size given.");
        }

        if (chunkSize.x <=0)
        {
            chunkSize.x = 1;
        }

        if (chunkSize.y <= 0)
        {
            chunkSize.y = 1;
        }


        if (useStartChunks != useEndChunks)
        {
            if (levelSize < 1) levelSize = 1;
            if (minLevelSize < 1) minLevelSize = 1;
        }


        if (useStartChunks && useEndChunks)
        {
            if (levelSize < 2) levelSize = 2;
            if (minLevelSize < 2) minLevelSize = 2;
        }


        if (minLevelSize > maxLevelSize) maxLevelSize = minLevelSize + 1;


        updateLibraries();
    }

    public void updateLibraries()
    {

        if (startChunkSprites != null)
        {
            List<Color> colors = new List<Color>();
            for (int x = 0; x < startChunkSprites.width; x++)
            {
                for (int y = 0; y < startChunkSprites.height; y++)
                {
                    Color color = startChunkSprites.GetPixel(x, y);
                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                    }
                }
            }
            if (startChunkTileLibrary == null || startChunkTileLibrary.Length != colors.Count) // séparer ca dans un autre if pour garder la valeur des tiles si on augmente le nombre de couleurs
            {
                startChunkTileLibrary = new ColorToTileBase[colors.Count];
                for (int i = 0; i < colors.Count; i++)
                {
                    startChunkTileLibrary[i] = new ColorToTileBase(colors[i]);
                }
            }
        }
        else startChunkTileLibrary = null;





        if (randomChunkSprites != null)
        {
            List<Color> colors = new List<Color>();
            for (int x = 0; x < randomChunkSprites.width; x++)
            {
                for (int y = 0; y < randomChunkSprites.height; y++)
                {
                    Color color = randomChunkSprites.GetPixel(x, y);
                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                    }
                }
            }
            if (randomChunkTileLibrary == null || randomChunkTileLibrary.Length != colors.Count) // séparer ca dans un autre if pour garder la valeur des tiles si on augmente le nombre de couleurs
            {
                randomChunkTileLibrary = new ColorToTileBase[colors.Count];
                for (int i = 0; i < colors.Count; i++)
                {
                    randomChunkTileLibrary[i] = new ColorToTileBase(colors[i]);
                }
            }
        }
        else randomChunkTileLibrary = null;






        if (endChunkSprites != null)
        {
            List<Color> colors = new List<Color>();
            for (int x = 0; x < endChunkSprites.width; x++)
            {
                for (int y = 0; y < endChunkSprites.height; y++)
                {
                    Color color = endChunkSprites.GetPixel(x, y);
                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                    }
                }
            }
            if (endChunkTileLibrary == null || endChunkTileLibrary.Length != colors.Count) // séparer ca dans un autre if pour garder la valeur des tiles si on augmente le nombre de couleurs
            {
                endChunkTileLibrary = new ColorToTileBase[colors.Count];
                for (int i = 0; i < colors.Count; i++)
                {
                    endChunkTileLibrary[i] = new ColorToTileBase(colors[i]);
                }
            }
        }
        else endChunkTileLibrary = null;

    }


}

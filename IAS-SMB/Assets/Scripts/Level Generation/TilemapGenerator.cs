using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SeedInitializer))]
public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    SeedInitializer seedInitializer;


    void Start()
    {
        GenerateTilemap();
    }

    [ContextMenu("Generate tilemap")]
    public void GenerateTilemap()
    {
        seedInitializer = GetComponent<SeedInitializer>();
        seedInitializer.InitSeed();
        if (tilemap == null)
        {
            Debug.Log("No tilemap was found");
            return;
        }
        ClearTilemaps();
        LevelGenerator levelGenerator = GetComponent<LevelGenerator>();
        TileBase[,] level = levelGenerator.ToTileArray(levelGenerator.GenerateLevel());
        DrawTilemap(level, -levelGenerator.Style.chunkSize.x / 2, -levelGenerator.Style.chunkSize.y / 2);
    }



    public void DrawTilemap(TileBase[,] level, int xOffset = 0, int yOffset = 0)
    {
        Debug.Log("Drawing tilemap with seed " + seedInitializer.Seed); ;
        for (int y = 0; y < level.GetLength(1); y++)
        {
            for (int x = 0; x < level.GetLength(0); x++)
            {
                tilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), level[x, y]);
            }
        }
    }

    [ContextMenu("Get tilemap info")]
    void GetTilemapInformations()
    {
        Debug.Log("cellBounds :" + tilemap.cellBounds.ToString());
        Debug.Log("color :" + tilemap.color.ToString());
        Debug.Log("origin :" + tilemap.origin.ToString());
        Debug.Log("size :" + tilemap.size.ToString());
        Debug.Log("tileAnchor :" + tilemap.tileAnchor.ToString());
        Debug.Log("localBounds :" + tilemap.localBounds.ToString());
    }

    [ContextMenu("Clear Tilemap")]
    void ClearTilemaps()
    {
        tilemap.ClearAllTiles();
    }


}

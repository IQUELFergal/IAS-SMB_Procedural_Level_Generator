using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    Grid grid;
    Tilemap[] tilemaps;

    [SerializeField] Tilemap tilemap;

   

    [ContextMenu("Create tilemaps")]
    public void CreateTilemaps(int n)
    {
        if (grid != null)
        {
            DestroyImmediate(grid.gameObject);
        }
        GameObject obj = new GameObject("Level grid");
        obj.AddComponent<Grid>();
        grid = obj.GetComponent<Grid>();
        grid.cellSize = new Vector3(1, 1, 0);
        tilemaps = new Tilemap[n];
        for (int i = 0; i < n; i++)
        {
            obj = new GameObject("Tilemap" + i);
            obj.transform.SetParent(grid.transform);
            obj.transform.localPosition = new Vector3(0, 0, i * 5);
            obj.layer = LayerMask.NameToLayer("Ground");
            obj.AddComponent<Tilemap>();
            obj.AddComponent<TilemapRenderer>();
            obj.AddComponent<CompositeCollider2D>();
            obj.AddComponent<TilemapCollider2D>();
            obj.GetComponent<TilemapCollider2D>().usedByComposite = true;
            obj.AddComponent<Rigidbody2D>();
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tilemaps[i] = obj.GetComponent<Tilemap>();
        }
    }



    /*[ContextMenu("Generate tilemap")]
    public void GenerateTilemap()
    {
        if (tilemap == null)
        {
            Debug.Log("No tilemap was found");
            return;
        }
        ClearTilemaps();
        LevelGenerator levelGenerator = GetComponent<LevelGenerator>();
        TileBase[,] level = levelGenerator.ToTileArray(levelGenerator.GenerateLevel());
        DrawTilemap(level, -levelGenerator.Style.chunkSize.x / 2, -levelGenerator.Style.chunkSize.y / 2);
    }*/


    public void GenerateTilemap(Chunk[] level, int xOffset = 0, int yOffset = 0)
    {
        int maxTilemapCount = 0;
        for (int i = 0; i < level.Length; i++)
        {
            if (level[i].TilemapCount > maxTilemapCount) maxTilemapCount = level[i].TilemapCount;
        }
        CreateTilemaps(maxTilemapCount);
        DrawLevel(level, xOffset, yOffset);
    }



    /*public void DrawTilemap(TileBase[,] level, int xOffset = 0, int yOffset = 0)
    {
        for (int y = 0; y < level.GetLength(1); y++)
        {
            for (int x = 0; x < level.GetLength(0); x++)
            {
                tilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), level[x, y]);
            }
        }
    }*/

    public void DrawLevel(Chunk[] level, int xOffset = 0, int yOffset = 0)
    {
        for (int i = 0; i < level.Length; i++)
        {
            for (int j = 0; j < level[i].TileGrid.Length; j++)
            {
                for (int y = 0; y < level[i].TileGrid[j].GetLength(1); y++)
                {
                    for (int x = 0; x < level[i].TileGrid[j].GetLength(0); x++)
                    {
                        tilemaps[j].SetTile(new Vector3Int(x + xOffset + i* level[i].TileGrid[j].GetLength(0), y + yOffset, 0), level[i].TileGrid[j][x, y]);
                    }
                }
            }
        }
    }


    [ContextMenu("Clear Tilemap")]
    void ClearTilemaps()
    {
        for (int i = 0; i < tilemaps.Length; i++)
        {
            tilemaps[i].ClearAllTiles();
        }
    }


}

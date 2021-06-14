using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

class TilemapTraining : Training
{
	Tilemap tilemap;
    public override void Compile()
    {
		tilemap = GetComponent<Tilemap>();
		tilemap.CompressBounds();
		//GetTilemapInformations();
		width = tilemap.size.x;
		depth = tilemap.size.y;

		str_tile = new Dictionary<string, byte>();
		sample = new byte[width, depth];
		//int cnt = this.transform.childCount;
		tiles = new UnityEngine.Object[width * depth];
		RS = new int[width * depth];
		tiles[0] = null;
		RS[0] = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
				Vector3Int tilepos = tilemap.origin + new Vector3Int(i, j, 0);
				TileBase tile = tilemap.GetTile(tilepos);
				UnityEngine.Object fab = tile;
				#if UNITY_EDITOR
				fab = PrefabUtility.GetCorrespondingObjectFromSource(tile);
				if (fab == null)
				{
					fab = (GameObject)Resources.Load(tile.name);
					if (!fab)
					{
						fab = tile;
					}
				}

				tile.name = fab.name;
				#endif
				int X = tilepos.x / gridsize;
				int Y = tilepos.y / gridsize;
				int R = 0;
				if (!str_tile.ContainsKey(fab.name + R))
				{
					int index = str_tile.Count + 1;
					str_tile.Add(fab.name + R, (byte)index);
					tiles[index] = fab;
					RS[index] = R;
					sample[i, j] = str_tile[fab.name + R];
				}
				else
				{
					sample[i, j] = str_tile[fab.name + R];
				}
			}
		}
		tiles = tiles.SubArray(0, str_tile.Count + 1);
		RS = RS.SubArray(0, str_tile.Count + 1);
	}
	void GetTilemapInformations()
	{
		Debug.Log("cellBounds :" + tilemap.cellBounds.ToString());
		Debug.Log("color :" + tilemap.color.ToString());
		Debug.Log("origin :" + tilemap.origin.ToString());
		Debug.Log("size :" + tilemap.size.ToString());
		Debug.Log("tileAnchor :" + tilemap.tileAnchor.ToString());
		Debug.Log("localBounds :" + tilemap.localBounds.ToString());
	}

	public void ClearTilemap()
    {
		tilemap.ClearAllTiles();
    }

	void OnDrawGizmos()
    {
		Gizmos.color = Color.magenta;
        if (tilemap !=null)
        {
			Gizmos.DrawWireCube(tilemap.origin + new Vector3(tilemap.size.x / 2 + (tilemap.size.x % 2 == 0 ? 0 : 0.5f), tilemap.size.y / 2 + (tilemap.size.y % 2 == 0 ? 0 : 0.5f), 0), tilemap.size);
		}
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(TilemapTraining))]
public class TilemapTrainingEditor : Editor
{
	public override void OnInspectorGUI()
	{
		TilemapTraining training = (TilemapTraining)target;
		if (GUILayout.Button("Compile"))
		{
			training.Compile();
		}
		if (GUILayout.Button("Record neighbors"))
		{
			training.RecordNeighbors();
		}
		if (GUILayout.Button("Clear tilemap"))
		{
			training.ClearTilemap();
		}
		DrawDefaultInspector();
	}
}
#endif

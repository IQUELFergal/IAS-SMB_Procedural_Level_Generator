using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

class TilemapOverlapWFC : OverlapWFC
{
	
	Tilemap outputTilemap;
	TileBase[,] tilesRendering;
	public override void Generate()
    {
		if (training == null) { Debug.Log("Can't Generate: no designated Training component"); }
		if (training.sample == null)
		{
			training.Compile();
		}
		outputTilemap = GetComponent<Tilemap>();
		tilesRendering = new TileBase[width, depth];
		model = new OverlappingModel(training.sample, N, width, depth, periodicInput, periodicOutput, symmetry, foundation);
		undrawn = true;
	}

	public override void Draw()
	{
		outputTilemap = GetComponent<Tilemap>();
		if (outputTilemap == null) { return; }
		//if (group == null) { return; }
		undrawn = false;
		outputTilemap.ClearAllTiles();
		try
		{
			for (int y = 0; y < depth; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (tilesRendering[x, y] == null)
					{
						int v = (int)model.Sample(x, y);
						if (v != 99 && v < training.tiles.Length)
						{
							TileBase tile = training.tiles[v] as TileBase;
							if (tile != null)
							{
								outputTilemap.SetTile(tilemapOffset+new Vector3Int(x, y, 0), tile);
								tilesRendering[x, y] = tile;
							}
						}
						else
						{
							undrawn = true;
						}
					}
				}
			}
		}
		catch (IndexOutOfRangeException)
		{
			model = null;
			return;
		}
	}
	public void ClearTilemap()
	{
		outputTilemap = GetComponent<Tilemap>();
		outputTilemap.ClearAllTiles();
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(TilemapOverlapWFC))]
public class TilemapWFCGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		TilemapOverlapWFC generator = (TilemapOverlapWFC)target;
		if (generator.training != null)
		{
			if (GUILayout.Button("Clear tilemap"))
			{
				generator.ClearTilemap();
			}
			if (GUILayout.Button("Generate"))
			{
				generator.Generate();
			}
			if (generator.model != null)
			{
				if (GUILayout.Button("RUN"))
				{
					generator.Run();
				}
			}
		}
		DrawDefaultInspector();
	}
}
#endif

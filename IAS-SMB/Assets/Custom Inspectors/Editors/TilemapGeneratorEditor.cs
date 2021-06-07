/*using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TilemapGenerator))]
public class TilemapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TilemapGenerator tilemapGen = (TilemapGenerator)target;

        DrawDefaultInspector();
        GUILayout.Space(20);

        if (GUILayout.Button("Generate tilemap"))
        {
            tilemapGen.GenerateTilemap();
        }
    }
}*/

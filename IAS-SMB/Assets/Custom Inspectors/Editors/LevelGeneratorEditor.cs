using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator generator = (LevelGenerator)target;

        DrawDefaultInspector();
        GUILayout.Space(20);

        if (GUILayout.Button("Generate level"))
        {
            generator.CreateLevel();
        }
    }
}
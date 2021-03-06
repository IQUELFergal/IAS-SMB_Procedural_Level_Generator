using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelStyle))]
public class LevelStyleEditor : Editor
{
    bool displayStartLibrary = false;
    bool displayRandomLibrary = false;
    bool displayEndLibrary = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LevelStyle levelStyle = (LevelStyle)target;

        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Level style configuration", centeredStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(20);

        //Chunk size
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chunkSize"));

        //Level size
        levelStyle.levelSizeSelector = (SizeSelector)EditorGUILayout.EnumPopup("Level Size Selector", levelStyle.levelSizeSelector);
        
        switch (levelStyle.levelSizeSelector)
        {
            default:
                break;
            case SizeSelector.Fixed:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("levelSize"));
                break;
            case SizeSelector.RandomRange:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minLevelSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLevelSize"));
                break;
        }

        //UseStartEndChunks
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useStartChunks"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useEndChunks"));
        if(levelStyle.randomChunkSprites != null && (levelStyle.randomChunkSprites.width % levelStyle.chunkSize.x != 0 || levelStyle.randomChunkSprites.height % levelStyle.chunkSize.y != 0))
        {
            EditorGUILayout.HelpBox("Random chunks texture size does not match the chunk size given.", MessageType.Error);
            GUILayout.Space(20);
        }
        else if (levelStyle.startChunkSprites != null && levelStyle.useStartChunks && (levelStyle.startChunkSprites.width % levelStyle.chunkSize.x != 0 || levelStyle.startChunkSprites.height % levelStyle.chunkSize.y != 0))
        {
            EditorGUILayout.HelpBox("Start chunks texture size does not match the chunk size given.", MessageType.Error);
            GUILayout.Space(20);
        }
        else if (levelStyle.endChunkSprites != null && levelStyle.useEndChunks && (levelStyle.endChunkSprites.width % levelStyle.chunkSize.x != 0 || levelStyle.endChunkSprites.height % levelStyle.chunkSize.y != 0))
        {
            EditorGUILayout.HelpBox("End chunks texture size does not match the chunk size given.", MessageType.Error);
            GUILayout.Space(20);
        }
        else GUILayout.Space(40);


        

        //Random Chunk Tile Library
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Chunks configuration", centeredStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(20);



        //Start Chunk Tile Library
        if (((LevelStyle)target).useStartChunks)
        {
            displayStartLibrary = EditorGUILayout.Foldout(displayStartLibrary, "Start Chunk Tile Library", EditorStyles.foldoutHeader);
            if (displayStartLibrary)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("startChunkSprites"), new GUIContent("Chunk atlas"));
                GUILayout.Space(5);
                SerializedProperty arrayProp = serializedObject.FindProperty("startChunkTileLibrary");
                if (arrayProp.arraySize > 0) EditorGUILayout.LabelField("Colors to tiles");
                for (int i = 0; i < arrayProp.arraySize; ++i)
                {
                    SerializedProperty colorToTile = arrayProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(colorToTile, new GUIContent(((LevelStyle)target).startChunkTileLibrary[i].Color.ToString("F2")));
                }
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        GUILayout.Space(20);


        //Random Chunk Tile Library
        displayRandomLibrary = EditorGUILayout.Foldout(displayRandomLibrary, "Random Chunk Tile Library", EditorStyles.foldoutHeader);
        if (displayRandomLibrary)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            ((LevelStyle)target).levelChunkType = (LevelStyle.LevelChunkType)EditorGUILayout.EnumPopup("Chunk type", ((LevelStyle)target).levelChunkType);
            GUILayout.Space(5);
            
            switch (((LevelStyle)target).levelChunkType)
            {
                default:
                    break;

                case LevelStyle.LevelChunkType.UseChunkAtlas:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomChunkSprites"), new GUIContent("Chunk atlas"));
                    SerializedProperty arrayProp = serializedObject.FindProperty("randomChunkTileLibrary");
                    if (arrayProp.arraySize > 0) EditorGUILayout.LabelField("Colors to tiles");
                    for (int i = 0; i < arrayProp.arraySize; ++i)
                    {
                        SerializedProperty colorToTile = arrayProp.GetArrayElementAtIndex(i);
                        EditorGUILayout.PropertyField(colorToTile, new GUIContent(((LevelStyle)target).randomChunkTileLibrary[i].Color.ToString("F2")));
                    }
                    break;
                case LevelStyle.LevelChunkType.GenerateRandomChunk:
                    GUILayout.Label(" // Generation parameters // ", centeredStyle);
                    break;
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        GUILayout.Space(20);


        //End Chunk Tile Library
        if (((LevelStyle)target).useEndChunks)
        {
            displayEndLibrary = EditorGUILayout.Foldout(displayEndLibrary, "End Chunk Tile Library", EditorStyles.foldoutHeader);
            if (displayEndLibrary)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("endChunkSprites"), new GUIContent("Chunk atlas"));
                GUILayout.Space(5);
                SerializedProperty arrayProp = serializedObject.FindProperty("endChunkTileLibrary");
                if (arrayProp.arraySize > 0) EditorGUILayout.LabelField("Colors to tiles");
                for (int i = 0; i < arrayProp.arraySize; ++i)
                {
                    SerializedProperty colorToTile = arrayProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(colorToTile, new GUIContent(((LevelStyle)target).endChunkTileLibrary[i].Color.ToString("F2")));
                }
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }




   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    bool displayPlatformSettings;
    bool displayTubeSettings;
    bool displayCannonSettings;
    bool displayGapsSettings;
    bool displayBlockSettings;
    bool displayCoinSettings;
    bool displayEnemySettings;

    SerializedProperty chunkSize;
    SerializedProperty levelSize;
    SerializedProperty minLevelSize;
    SerializedProperty maxLevelSize;
    SerializedProperty useStartChunk;
    SerializedProperty useEndChunk;

    SerializedProperty platformTile;
    SerializedProperty platformMaxHeightDifference;
    SerializedProperty platformFixedCount;
    SerializedProperty platformMinCount;
    SerializedProperty platformMaxCount;
    SerializedProperty platformFixedHeight;
    SerializedProperty platformMinHeight;
    SerializedProperty platformMaxHeight;

    SerializedProperty gapTile;
    SerializedProperty numberOfGaps;
    SerializedProperty gapFixedWidth;
    SerializedProperty gapMinWidth;
    SerializedProperty gapMaxWidth;

    SerializedProperty tubeTile;
    SerializedProperty numberOfTubes;
    SerializedProperty tubeMaxIteration;
    SerializedProperty tubeFixedHeight;
    SerializedProperty tubeMinHeight;
    SerializedProperty tubeMaxHeight;

    SerializedProperty cannonTile;
    SerializedProperty numberOfCannons;
    SerializedProperty cannonMaxIteration;
    SerializedProperty cannonFixedHeight;
    SerializedProperty cannonMinHeight;
    SerializedProperty cannonMaxHeight;

    SerializedProperty blockTile;
    SerializedProperty yellowBlockTile;
    SerializedProperty blockHeightFromPlatform;
    SerializedProperty blockFixedWidth;
    SerializedProperty blockMinWidth;
    SerializedProperty blockMaxWidth;

    SerializedProperty coinTile;
    SerializedProperty extraCoinProbability;

    SerializedProperty goombaTile;
    SerializedProperty koopaTile;
    SerializedProperty enemyFixedCount;
    SerializedProperty enemyMinCount;
    SerializedProperty enemyMaxCount;

    private void OnEnable()
    {
        //Level config properties
        chunkSize = serializedObject.FindProperty("chunkSize");
        levelSize = serializedObject.FindProperty("levelSize");
        minLevelSize = serializedObject.FindProperty("minLevelSize");
        maxLevelSize = serializedObject.FindProperty("maxLevelSize");
        useStartChunk = serializedObject.FindProperty("useStartChunk");
        useEndChunk = serializedObject.FindProperty("useEndChunk");

        //Chunk config properties
        platformTile = serializedObject.FindProperty("platformTile");
        platformMaxHeightDifference = serializedObject.FindProperty("platformMaxHeightDifference");
        platformFixedCount = serializedObject.FindProperty("platformFixedCount");
        platformMinCount = serializedObject.FindProperty("platformMinCount");
        platformMaxCount = serializedObject.FindProperty("platformMaxCount");
        platformFixedHeight = serializedObject.FindProperty("platformFixedHeight");
        platformMinHeight = serializedObject.FindProperty("platformMinHeight");
        platformMaxHeight = serializedObject.FindProperty("platformMaxHeight");

        gapTile = serializedObject.FindProperty("gapTile");
        numberOfGaps = serializedObject.FindProperty("numberOfGaps");
        gapFixedWidth = serializedObject.FindProperty("gapFixedWidth");
        gapMinWidth = serializedObject.FindProperty("gapMinWidth");
        gapMaxWidth = serializedObject.FindProperty("gapMaxWidth");

        tubeTile = serializedObject.FindProperty("tubeTile");
        numberOfTubes = serializedObject.FindProperty("numberOfTubes");
        tubeMaxIteration = serializedObject.FindProperty("tubeMaxIteration");
        tubeFixedHeight = serializedObject.FindProperty("tubeFixedHeight");
        tubeMinHeight = serializedObject.FindProperty("tubeMinHeight");
        tubeMaxHeight = serializedObject.FindProperty("tubeMaxHeight");

        cannonTile = serializedObject.FindProperty("cannonTile");
        numberOfCannons = serializedObject.FindProperty("numberOfCannons");
        cannonMaxIteration = serializedObject.FindProperty("cannonMaxIteration");
        cannonFixedHeight = serializedObject.FindProperty("cannonFixedHeight");
        cannonMinHeight = serializedObject.FindProperty("cannonMinHeight");
        cannonMaxHeight = serializedObject.FindProperty("cannonMaxHeight");

        blockTile = serializedObject.FindProperty("blockTile");
        yellowBlockTile = serializedObject.FindProperty("yellowBlockTile");
        blockHeightFromPlatform = serializedObject.FindProperty("blockHeightFromPlatform");
        blockFixedWidth = serializedObject.FindProperty("blockFixedWidth");
        blockMinWidth = serializedObject.FindProperty("blockMinWidth");
        blockMaxWidth = serializedObject.FindProperty("blockMaxWidth");

        coinTile = serializedObject.FindProperty("coinTile");
        extraCoinProbability = serializedObject.FindProperty("extraCoinProbability");

        goombaTile = serializedObject.FindProperty("goombaTile");
        koopaTile = serializedObject.FindProperty("koopaTile");
        enemyFixedCount = serializedObject.FindProperty("enemyFixedCount");
        enemyMinCount = serializedObject.FindProperty("enemyMinCount");
        enemyMaxCount = serializedObject.FindProperty("enemyMaxCount");
    }

    public override void OnInspectorGUI()
    {
        LevelGenerator generator = (LevelGenerator)target;

        var titleStyle = GUI.skin.GetStyle("Label");
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.fontStyle = FontStyle.Bold;

        var headerStyle = GUI.skin.GetStyle("Label");
        headerStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Level configuration", titleStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(10);


        //Chunk size
        EditorGUILayout.PropertyField(chunkSize);

        //Level size
        generator.levelSizeSelector = (SizeSelector)EditorGUILayout.EnumPopup("Level Size Selector", generator.levelSizeSelector);

        switch (generator.levelSizeSelector)
        {
            default:
                break;
            case SizeSelector.Fixed:
                EditorGUILayout.PropertyField(levelSize);
                break;
            case SizeSelector.RandomRange:
                EditorGUILayout.PropertyField(minLevelSize);
                EditorGUILayout.PropertyField(maxLevelSize);
                break;
        }

        //UseStartEndChunks
        EditorGUILayout.PropertyField(useStartChunk);
        EditorGUILayout.PropertyField(useEndChunk);

        //DrawDefaultInspector();
        GUILayout.Space(20);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Chunk configuration", titleStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(10);

        displayPlatformSettings = EditorGUILayout.Foldout(displayPlatformSettings, "Platform Settings", EditorStyles.foldoutHeader);
        if (displayPlatformSettings)
        {
            EditorGUILayout.PropertyField(platformTile);
            EditorGUILayout.PropertyField(platformMaxHeightDifference);

            generator.platformCountSelector = (SizeSelector)EditorGUILayout.EnumPopup("Platform Count Selector", generator.platformCountSelector);

            switch (generator.platformCountSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(platformFixedCount);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(platformMinCount);
                    EditorGUILayout.PropertyField(platformMaxCount);
                    break;
            }

            generator.platformHeightSelector = (SizeSelector)EditorGUILayout.EnumPopup("Platform Height Selector", generator.platformHeightSelector);

            switch (generator.platformHeightSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(platformFixedHeight);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(platformMinHeight);
                    EditorGUILayout.PropertyField(platformMaxHeight);
                    break;
            }
        }

        GUILayout.Space(10);

        displayTubeSettings = EditorGUILayout.Foldout(displayTubeSettings, "Tube Settings", EditorStyles.foldoutHeader);
        if (displayTubeSettings)
        {
            EditorGUILayout.PropertyField(tubeTile);
            EditorGUILayout.PropertyField(numberOfTubes);
            EditorGUILayout.PropertyField(tubeMaxIteration);

            generator.tubeHeightSelector = (SizeSelector)EditorGUILayout.EnumPopup("Tube Height Selector", generator.tubeHeightSelector);

            switch (generator.tubeHeightSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(tubeFixedHeight);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(tubeMinHeight);
                    EditorGUILayout.PropertyField(tubeMaxHeight);
                    break;
            }
        }

        GUILayout.Space(10);

        displayCannonSettings = EditorGUILayout.Foldout(displayCannonSettings, "Cannon Settings", EditorStyles.foldoutHeader);
        if (displayCannonSettings)
        {
            EditorGUILayout.PropertyField(cannonTile);
            EditorGUILayout.PropertyField(numberOfCannons);
            EditorGUILayout.PropertyField(cannonMaxIteration);

            generator.cannonHeightSelector = (SizeSelector)EditorGUILayout.EnumPopup("Cannon Height Selector", generator.cannonHeightSelector);

            switch (generator.cannonHeightSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(cannonFixedHeight);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(cannonMinHeight);
                    EditorGUILayout.PropertyField(cannonMaxHeight);
                    break;
            }
        }

        GUILayout.Space(10);

        displayGapsSettings = EditorGUILayout.Foldout(displayGapsSettings, "Gap Settings", EditorStyles.foldoutHeader);
        if (displayGapsSettings)
        {
            EditorGUILayout.PropertyField(gapTile);
            EditorGUILayout.PropertyField(numberOfGaps);

            generator.gapWidthSelector = (SizeSelector)EditorGUILayout.EnumPopup("Gap Width Selector", generator.gapWidthSelector);

            switch (generator.gapWidthSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(gapFixedWidth);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(gapMinWidth);
                    EditorGUILayout.PropertyField(gapMaxWidth);
                    break;
            }
        }

        GUILayout.Space(10);

        displayBlockSettings = EditorGUILayout.Foldout(displayBlockSettings, "Block Settings", EditorStyles.foldoutHeader);
        if (displayBlockSettings)
        { 
            EditorGUILayout.PropertyField(blockTile);
            EditorGUILayout.PropertyField(yellowBlockTile);
            EditorGUILayout.PropertyField(blockHeightFromPlatform);

            generator.blockWidthSelector = (SizeSelector)EditorGUILayout.EnumPopup("Block Width Selector", generator.blockWidthSelector);

            switch (generator.blockWidthSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(blockFixedWidth);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(blockMinWidth);
                    EditorGUILayout.PropertyField(blockMaxWidth);
                    break;
            }
        }

        GUILayout.Space(10);

        displayCoinSettings = EditorGUILayout.Foldout(displayCoinSettings, "Coin Settings", EditorStyles.foldoutHeader);
        if (displayCoinSettings)
        { 
            EditorGUILayout.PropertyField(coinTile);
            EditorGUILayout.PropertyField(extraCoinProbability);
        }

        GUILayout.Space(10);

        displayEnemySettings = EditorGUILayout.Foldout(displayEnemySettings, "Enemy Settings", EditorStyles.foldoutHeader);
        if (displayEnemySettings)
        { 
            EditorGUILayout.PropertyField(goombaTile);
            EditorGUILayout.PropertyField(koopaTile);
            generator.enemyCountSelector = (SizeSelector)EditorGUILayout.EnumPopup("Platform Count Selector", generator.enemyCountSelector);

            switch (generator.enemyCountSelector)
            {
                default:
                    break;
                case SizeSelector.Fixed:
                    EditorGUILayout.PropertyField(enemyFixedCount);
                    break;
                case SizeSelector.RandomRange:
                    EditorGUILayout.PropertyField(enemyMinCount);
                    EditorGUILayout.PropertyField(enemyMaxCount);
                    break;
            }
        }

        GUILayout.Space(20);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Generate level"))
        {
            generator.CreateLevel();
        }

        GUILayout.Space(10);
    }
}
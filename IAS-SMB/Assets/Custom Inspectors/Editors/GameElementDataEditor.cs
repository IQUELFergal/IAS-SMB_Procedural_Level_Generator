using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameElementData))]
public class GameElementDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GameElementData gameElementData = (GameElementData)target;

        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("GameElement ("+ gameElementData.name+") configuration", centeredStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(20);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("tile"));

        //Size
        gameElementData.sizeSelector = (SizeSelector)EditorGUILayout.EnumPopup("Level Size Selector", gameElementData.sizeSelector);
        
        switch (gameElementData.sizeSelector)
        {
            default:
                break;
            case SizeSelector.Fixed:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedSize"));
                break;
            case SizeSelector.RandomRange:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSize"));
                break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }   
}

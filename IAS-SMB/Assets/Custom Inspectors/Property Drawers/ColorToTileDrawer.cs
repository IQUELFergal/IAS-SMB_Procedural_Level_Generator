using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(ColorToTileBase))]
public class ColorToTileDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect pos, SerializedProperty prop, UnityEngine.GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(pos, label, prop);
        
        // Draw label
        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty color = prop.FindPropertyRelative("color");
        SerializedProperty tile = prop.FindPropertyRelative("tile");

        EditorGUI.PropertyField(
            new Rect(pos.x , pos.y, pos.width / 2, pos.height),
            color, GUIContent.none);

        if ((TileBase)tile.objectReferenceValue != null)
        {
            //EditorGUI.DrawPreviewTexture(new Rect(pos.width / 2 + pos.x, pos.y, pos.height, pos.height), ((Tile)tile.objectReferenceValue).sprite.texture);
            if (tile.objectReferenceValue.GetType() == typeof(RuleTile))
            {
                GUI.DrawTexture(new Rect(pos.width / 2 + pos.x, pos.y, pos.height, pos.height), ((RuleTile)tile.objectReferenceValue).m_DefaultSprite.texture);
            }
            else GUI.DrawTexture(new Rect(pos.width / 2 + pos.x, pos.y, pos.height, pos.height), ((Tile)tile.objectReferenceValue).sprite.texture);
            
        }
        
        EditorGUI.PropertyField(
            new Rect(pos.height + pos.width / 2 + pos.x, pos.y, pos.width / 2- pos.height, pos.height),
            tile, GUIContent.none);
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

}

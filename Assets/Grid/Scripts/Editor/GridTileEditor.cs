using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GridNamespace;

[CustomEditor(typeof(GridTile))]
public class GridTileEditor : Editor
{
    GUIStyle normalStyle;



    public override void OnInspectorGUI()
    {
        if (normalStyle == null) {
            normalStyle = new GUIStyle(EditorStyles.label);
            normalStyle.fontStyle = FontStyle.Normal;
        }

        
        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Name: ", target.name, normalStyle);
        EditorStyles.label.fontStyle = FontStyle.Normal;

    }

}

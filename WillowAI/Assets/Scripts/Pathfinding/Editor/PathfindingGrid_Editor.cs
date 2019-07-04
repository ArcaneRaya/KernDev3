using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingGrid))]
public class PathfindingGrid_Editor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.LabelField("Only changable by PathfindingController in scene", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        base.OnInspectorGUI();
        GUI.enabled = true;
    }
}
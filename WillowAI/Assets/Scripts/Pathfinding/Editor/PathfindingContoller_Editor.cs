using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingController))]
public class PathfindingContoller_Editor : Editor {

    private SerializedProperty grid;
    //   private SerializedObject serializedGridObject;

    private SerializedProperty gridProperty_floor;
    private SerializedProperty gridProperty_obstacle;
    private SerializedProperty gridProperty_NodeSize;
    private SerializedProperty gridProperty_UnitHeight;


    void OnEnable() {
        grid = serializedObject.FindProperty("Grid");
        //serializedGridObject = new SerializedObject(grid.objectReferenceValue);

        gridProperty_floor = grid.FindPropertyRelative("Floor");
        gridProperty_obstacle = grid.FindPropertyRelative("Obstacle");
        gridProperty_NodeSize = grid.FindPropertyRelative("NodeSize");
        gridProperty_UnitHeight = grid.FindPropertyRelative("UnitHeight");

    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        GUI.enabled = false;
        base.OnInspectorGUI();
        GUI.enabled = true;
        EditorGUILayout.PropertyField(gridProperty_floor);
        EditorGUILayout.PropertyField(gridProperty_obstacle);
        EditorGUILayout.PropertyField(gridProperty_NodeSize);
        EditorGUILayout.PropertyField(gridProperty_UnitHeight);
        //if (EditorGUI.EndChangeCheck()) {
        //    if (grid.objectReferenceValue != null) {
        //        //serializedGridObject = new SerializedObject(grid.objectReferenceValue);

        //        gridProperty_floor = grid.FindPropertyRelative("Floor");
        //        gridProperty_obstacle = grid.FindPropertyRelative("Obstacle");
        //        gridProperty_NodeSize = grid.FindPropertyRelative("NodeSize");
        //        gridProperty_UnitHeight = grid.FindPropertyRelative("UnitHeight");
        //    } else {
        //        //serializedGridObject = null;
        //    }
        //}
        serializedObject.ApplyModifiedProperties();

        //if (serializedGridObject != null) {
        //    serializedGridObject.Update();
        //    EditorGUILayout.PropertyField(gridProperty_floor);
        //    EditorGUILayout.PropertyField(gridProperty_obstacle);
        //    EditorGUILayout.PropertyField(gridProperty_NodeSize);
        //    EditorGUILayout.PropertyField(gridProperty_UnitHeight);
        //    serializedGridObject.ApplyModifiedProperties();
        //}

    }
}

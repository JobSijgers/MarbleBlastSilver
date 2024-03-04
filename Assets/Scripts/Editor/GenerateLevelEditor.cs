using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateLevel))]

public class GenerateLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerateLevel generateLevel = (GenerateLevel)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Resources"))
        {
            generateLevel.LoadResources();
        }
        EditorGUILayout.EndHorizontal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using Codice.Client.Common.GameUI;

[CustomEditor(typeof(LevelSegment))]
public class LevelSegmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LevelSegment levelSegment = (LevelSegment)target;

        base.OnInspectorGUI();
        if (levelSegment.segmentReqGems)
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            levelSegment.barrier = (GameObject)EditorGUILayout.ObjectField("Barrier", levelSegment.barrier, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();  
            if (GUILayout.Button("Fetch Required Items"))
            {
                levelSegment.FetchRequiredItems();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            SerializedProperty objectList = serializedObject.FindProperty("requiredGems");
            EditorGUILayout.PropertyField(objectList);

            EditorGUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

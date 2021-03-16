using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlurPanel))]
public class BlurPanelEditor : ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("time"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}

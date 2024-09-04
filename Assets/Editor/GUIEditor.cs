using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GUIChangeObj)), CanEditMultipleObjects]
public class GUIEditor : Editor
{
    public bool enabled = true;
    public override void OnInspectorGUI()
    {
        var cachedColor = GUI.backgroundColor;

        if (GUILayout.Button("Select all Selectable"))
        {
            Selection.objects = FindObjectsOfType<GUIChangeObj>();
        }
        if (GUILayout.Button("Deselect all"))
        {
            Selection.objects = new Object[] { };
        }
        if (GUILayout.Button("Apply Enable/Disable Status"))
        {
            enabled = !enabled;
            if (enabled)
            {
                GUI.backgroundColor = Color.green;
                Debug.Log("a");
            } else
            {
                Debug.Log("b");
                GUI.backgroundColor = cachedColor;
            }
            foreach (GUIChangeObj obj in FindObjectsOfType<GUIChangeObj>(true))
            {
                obj.gameObject.SetActive(enabled);
            }
        }

        serializedObject.Update();
        var scale = serializedObject.FindProperty("scale");
        EditorGUILayout.PropertyField(scale);
        serializedObject.ApplyModifiedProperties();
        if (scale.floatValue > 3f)
        {
            EditorGUILayout.HelpBox("Too big!", MessageType.Warning);
        }
        else if (scale.floatValue < .5f)
        {
            EditorGUILayout.HelpBox("Too small!", MessageType.Warning);
        }
        else
        {
            foreach (GameObject currObj in Selection.objects) {
                currObj.gameObject.transform.localScale = new Vector3(scale.floatValue, scale.floatValue, scale.floatValue);
            }
        }

    }
}

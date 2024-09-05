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
    public static Object originalSelection = null;
    
    public override void OnInspectorGUI()
    {
        //On click, find and collect all 'GUIChangeObj' - Save original object to load on deselect
        if (GUILayout.Button("Select all Selectable"))
        {
            originalSelection = Selection.objects[0];
            Selection.objects = FindObjectsOfType<GUIChangeObj>();
        }
        if (GUILayout.Button("Deselect all"))
        {
            Object[] tempArray = {originalSelection};
            Selection.objects = tempArray;
            originalSelection = null;
        }

        //Set following button color by 'enabled' state
        if (enabled)
        {
            GUI.backgroundColor = Color.green;
            Debug.Log("a");
        }
        else
        {
            Debug.Log("b");
            GUI.backgroundColor = Color.gray;
        }
        if (GUILayout.Button("Enable/Disable All", GUILayout.Height(40)))
        {
            enabled = !enabled;
            
            foreach (GUIChangeObj obj in FindObjectsOfType<GUIChangeObj>(true))
            {
                obj.gameObject.SetActive(enabled);
            }
        }

        //Restore rest of UI to gray
        GUI.backgroundColor = Color.gray;
        //Update Properties List then Access the scale from the 'GUIChangeObj' script
        serializedObject.Update();
        var scale = serializedObject.FindProperty("scale");
        //Show to screen and update value in time
        EditorGUILayout.PropertyField(scale);
        serializedObject.ApplyModifiedProperties();


        //Check if in bounds, if not send appropiate error
        //If in bounds, set size based on single or multiple selections (two cases avoids cast error idky)
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
            if (Selection.objects.Length == 1)
            {
                foreach (GameObject currObj in Selection.objects)
                {
                    currObj.transform.localScale = new Vector3(scale.floatValue, scale.floatValue, scale.floatValue);
                }
            }
            else
            {
                foreach (GUIChangeObj currObj in Selection.objects)
                {
                    currObj.transform.localScale = new Vector3(scale.floatValue, scale.floatValue, scale.floatValue);
                }
            }
        }
    }
}

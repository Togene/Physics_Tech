using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(Isometric))]

public class IsometricInspector : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Isometric isoMap = (Isometric)target;
        
        if(GUILayout.Button("Generate"))
        {
            isoMap.createGrid();
        }
        else if (GUILayout.Button("Delete"))
        {
            isoMap.Delete();
        }
        //DrawDefaultInspector();
    }

}

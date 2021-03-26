using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GhostController))]
public class GhostControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GhostController ghost = (GhostController)target;

        if (GUILayout.Button("Valid"))
        {
            ghost.SetPlacementValid();
        }
        if (GUILayout.Button("Invalid"))
        {
            ghost.SetPlacementInvalid();
        }
        if (GUILayout.Button("Original"))
        {
            ghost.RemoveGhost();
        }
    }
}

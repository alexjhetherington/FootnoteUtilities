using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidEvent), editorForChildClasses: true)]
public class VoidEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        VoidEvent e = target as VoidEvent;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}

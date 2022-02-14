using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SoundEntry))]
public class SoundEntryPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var audioClip = (AudioClip)property.FindPropertyRelative("audioClip").objectReferenceValue;
        if (audioClip != null)
            Highlighter.HighlightIdentifier(position, audioClip.name);

        DrawHeader(position, property);
    }

    private bool DrawHeader(Rect rect, SerializedProperty property)
    {
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("audioClip"));

        property.isExpanded = EditorGUI.Foldout(
            rect,
            property.isExpanded,
            GUIContent.none,
            EditorStyles.foldout
        );

        if (property.isExpanded)
        {
            foreach (SerializedProperty child in property.GetChildren())
            {
                if (child.name != "audioClip" && child.name != "audioMixerGroup")
                    EditorGUILayout.PropertyField(child);
            }

            EditorGUILayout.Separator();
        }

        return property.isExpanded;
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundLibrary))]
public class SoundLibraryInspector : Editor
{
    string textSearch;
    HashSet<string> soundEntryNames = new HashSet<string>();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty soundEntriesProperty = serializedObject.FindProperty("soundEntries");

        textSearch = GUILayout.TextField(textSearch);
        if (GUILayout.Button("Collapse All"))
        {
            for (int i = 0; i < soundEntriesProperty.arraySize; i++)
            {
                SerializedProperty soundEntryProperty = soundEntriesProperty.GetArrayElementAtIndex(
                    i
                );
                soundEntryProperty.isExpanded = false;
            }
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("audioMixerGroup"));
        GUILayout.Space(40);

        ShowSoundEntries(soundEntriesProperty);
        serializedObject.ApplyModifiedProperties();
    }

    private void ShowSoundEntries(SerializedProperty soundEntriesProperty)
    {
        if (soundEntriesProperty.arraySize == 0)
            soundEntriesProperty.arraySize += 1;

        soundEntryNames.Clear();

        for (int i = 0; i < soundEntriesProperty.arraySize; i++)
        {
            SerializedProperty soundEntryProperty = soundEntriesProperty.GetArrayElementAtIndex(i);
            Object audioClip =
                soundEntryProperty.FindPropertyRelative("audioClip").objectReferenceValue;

            //Skip the entry if text search is active and entry does not have relevant alias
            if (!string.IsNullOrEmpty(textSearch) && audioClip != null)
            {
                SerializedProperty aliases = soundEntryProperty.FindPropertyRelative("aliases");
                HashSet<string> _aliases = new HashSet<string>();

                if (aliases != null)
                {
                    for (int j = 0; j < aliases.arraySize; j++)
                    {
                        SerializedProperty alias = aliases.GetArrayElementAtIndex(j);
                        _aliases.Add(alias.stringValue.ToLower());
                    }
                }

                _aliases.Add(audioClip.name);
                string lowerTextSearch = textSearch.ToLower();

                bool match = false;
                foreach (string alias in _aliases)
                {
                    if (alias.Contains(textSearch.ToLower()))
                    {
                        match = true;
                        break;
                    }
                }
                if (!match)
                    continue;
            }

            ShowSoundEntry(soundEntriesProperty, soundEntryProperty, i);

            //Remove entries that are null (unless it is the last one) or already added
            if (
                i < soundEntriesProperty.arraySize - 1
                && (
                    audioClip == null
                    || (audioClip != null && soundEntryNames.Contains(audioClip.name))
                )
            )
            {
                soundEntriesProperty.DeleteArrayElementAtIndex(i);
                i--; //Was questioning whether it is necessary. It is, otherwise the object reference window does not work properly
                continue;
            }

            if (audioClip != null)
                soundEntryNames.Add(audioClip.name);
        }

        //Add an entry if the final audioClip is not null
        SerializedProperty finalSoundEntry = soundEntriesProperty.GetArrayElementAtIndex(
            soundEntriesProperty.arraySize - 1
        );
        Object finalAudioClip =
            finalSoundEntry.FindPropertyRelative("audioClip").objectReferenceValue;
        if (finalAudioClip != null)
        {
            SoundLibrary soundLibrary = target as SoundLibrary;
            List<SoundEntry> soundEntries = soundLibrary.soundEntries;
            soundEntries.Add(new SoundEntry());
        }
    }

    private static void ShowSoundEntry(
        SerializedProperty soundEntriesProperty,
        SerializedProperty soundEntryProperty,
        int index
    )
    {
        EditorGUILayout.PropertyField(soundEntryProperty);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (
            index < soundEntriesProperty.arraySize - 1
            && GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(16))
        )
        {
            soundEntriesProperty.DeleteArrayElementAtIndex(index);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class SoundLibrary : ScriptableObject
{
    [SerializeField]
    public AudioMixerGroup audioMixerGroup;

    [SerializeField]
    public List<SoundEntry> soundEntries;

    public Dictionary<string, List<SoundEntry>> GetSoundEntries()
    {
        var dictionary = new Dictionary<string, List<SoundEntry>>(soundEntries.Count - 1);
        for (int i = 0; i < soundEntries.Count - 1; i++) //the last sound entry is null
        {
            SoundEntry soundEntry = soundEntries[i];
            soundEntry.audioMixerGroup = audioMixerGroup;

            if (soundEntry.audioClip == null)
            {
                Debug.LogError(
                    "Null AudioClip found in the sound library; not adding it to list of available sounds!",
                    this
                );
                continue;
            }

            if (!dictionary.ContainsKey(soundEntry.audioClip.name))
                dictionary[soundEntry.audioClip.name] = new List<SoundEntry>();

            dictionary[soundEntry.audioClip.name].Add(soundEntry);

            foreach (string alias in soundEntry.aliases)
            {
                if (!dictionary.ContainsKey(alias))
                    dictionary[alias] = new List<SoundEntry>();

                dictionary[alias].Add(soundEntry);
            }
        }
        return dictionary;
    }
}

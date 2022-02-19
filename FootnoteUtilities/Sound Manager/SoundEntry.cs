using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundEntry
{
    public SoundEntry()
    {
        volume = 1;
        spatialBlend = 0;
    }

    public AudioClip audioClip;

    [Range(0.0F, 1.0F)]
    public float volume = 1;

    [Range(0.0F, 1.0F)]
    public float spatialBlend = 1;

    [Range(0.0F, 0.25F)]
    public float pitchVariation = 0;

    public bool looping;

    public string[] aliases;

    public CustomFalloff customFalloff;

    public AudioMixerGroup audioMixerGroup;

    public int maxSimultaneousPlaying = 0;
}

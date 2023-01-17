using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTriggerSoundFade : MonoBehaviour
{
    public Tag triggerByTag;
    public bool compound = false;
    public AudioSource audioSource;

    public float targetVolume;

    public float fadeSpeed;

    public void OnTriggerEnter(Collider other)
    {
        if (compound)
            return;

        if (other.HasTag(triggerByTag))
        {
            SoundManager.FadeTo(audioSource, targetVolume, fadeSpeed);
        }
    }

    public void OnCompoundTriggerEnter(Collider other)
    {
        if (!compound)
            return;

        if (other.HasTag(triggerByTag))
        {
            SoundManager.FadeTo(audioSource, targetVolume, fadeSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInterface : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    public void Play()
    {
        SoundManager.PlaySound(audioClip);
    }

    public void Play(string name)
    {
        SoundManager.PlaySound(name);
    }
}

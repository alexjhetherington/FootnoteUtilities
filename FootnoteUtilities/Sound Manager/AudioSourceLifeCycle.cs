using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceLifeCycle : MonoBehaviour
{
    private Transform toFollow;
    private bool following = false;
    public float startTime;
    public AudioSource audioSource;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (!audioSource.isPlaying || (following && toFollow == null))
        {
            SoundManager.Instance.Release(audioSource);
        }

        if (toFollow != null)
            transform.position = toFollow.position;
    }

    public void SetFollowTarget(Transform target)
    {
        toFollow = target;
        following = true;
    }
}

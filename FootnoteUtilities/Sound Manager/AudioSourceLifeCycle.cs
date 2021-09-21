using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceLifeCycle : MonoBehaviour
{
    public Transform toFollow;
    public float startTime;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (toFollow != null)
            transform.position = toFollow.position;

        if (!audioSource.isPlaying)
            PoolManager.ReleaseObject(gameObject);
    }
}

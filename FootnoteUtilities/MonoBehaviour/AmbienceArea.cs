using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceArea : MonoBehaviour
{
    [Resolve]
    public BoxCollider area;

    public AmbienceSequnce[] ambienceSequences = new AmbienceSequnce[1];

    private float[] nextTimes;

    [System.Serializable]
    public struct AmbienceSequnce
    {
        public Vector2 timeBetweenPlay;
        public AudioClip[] audioClips;
    }

    void Awake()
    {
        nextTimes = new float[ambienceSequences.Length];

        for (int i = 0; i < ambienceSequences.Length; i++)
        {
            AssignRandomNextTime(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < ambienceSequences.Length; i++)
        {
            if (nextTimes[i] < Time.time)
            {
                SoundManager.PlaySound(
                    ambienceSequences[i].audioClips[
                        Random.Range(0, ambienceSequences[i].audioClips.Length)
                    ],
                    area.GetRandomPointInsideCollider()
                );

                AssignRandomNextTime(i);
            }
        }
    }

    private void AssignRandomNextTime(int i)
    {
        nextTimes[i] =
            Time.time
            + Random.Range(
                ambienceSequences[i].timeBetweenPlay.x,
                ambienceSequences[i].timeBetweenPlay.y
            );
    }
}

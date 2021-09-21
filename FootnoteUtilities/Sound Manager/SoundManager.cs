using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, List<SoundEntry>> groupedSoundEntries = new Dictionary<string, List<SoundEntry>>();
    private GameObject prototype;

    private void Init()
    {
        prototype = new GameObject("Sound Prototype");
        DontDestroyOnLoad(prototype);
        AudioSource audioSource = prototype.AddComponent<AudioSource>();
        AudioSourceLifeCycle audioSourceLifeCycle = prototype.AddComponent<AudioSourceLifeCycle>();
        audioSourceLifeCycle.audioSource = audioSource;
        prototype.SetActive(false);


        var soundLibrary2 = Resources.LoadAll<SoundLibrary>("SoundManager");
        Init(soundLibrary2);
    }

    private void Init(SoundLibrary[] soundLibrary)
    {
        if (soundLibrary.Length < 1)
            Debug.LogError("Sound manager could not find any sound libraries");
        else
        {
            foreach (SoundLibrary lib in soundLibrary)
            {
                foreach(var entry in lib.GetSoundEntries())
                {
                    if (!groupedSoundEntries.ContainsKey(entry.Key))
                        groupedSoundEntries[entry.Key] = entry.Value;
                    else
                        groupedSoundEntries[entry.Key].AddRange(entry.Value);
                }
            }
        }
    }

    public AudioSource PlaySoundInternal(AudioClip audioClip, Vector3 position, Transform followTarget, float pitchOverride) { return PlaySoundInternal(audioClip.name, position, followTarget, pitchOverride);  }
    public AudioSource PlaySoundInternal(string name, Vector3 position, Transform followTarget, float pitchOverride) {
        List<SoundEntry> soundEntries;

        if(!groupedSoundEntries.TryGetValue(name, out soundEntries))
        {
            Debug.LogError("Sound was not found in library: " + name);
            foreach(var entry in groupedSoundEntries.Keys)
            {
                Debug.Log(entry);
            }
            return null;
        }

        SoundEntry soundEntry = soundEntries[Random.Range(0, soundEntries.Count)];

        var go = PoolManager.SpawnObject(prototype);
        DontDestroyOnLoad(go);
        go.name = "(Sound) " + soundEntry.audioClip.name;

        var audioSource = go.GetComponent<AudioSource>();
        var audioSourceLifecycle = go.GetComponent<AudioSourceLifeCycle>();

        go.transform.position = position;
        
        audioSource.clip = soundEntry.audioClip;
        audioSource.volume = soundEntry.volume;
        audioSource.loop = soundEntry.looping;
        audioSource.spatialBlend = soundEntry.spatialBlend;
        audioSource.outputAudioMixerGroup = soundEntry.audioMixerGroup;

        if (soundEntry.pitchVariation != 0)
            audioSource.pitch = 1 + Random.Range(-soundEntry.pitchVariation, soundEntry.pitchVariation);
        else
            audioSource.pitch = 1;

        if (pitchOverride != 0)
            audioSource.pitch = pitchOverride;
        
        audioSourceLifecycle.toFollow = followTarget;

        audioSource.Play();
        return audioSource;
    }

    //===Singleton Related things=== 

    public static AudioSource PlaySound(AudioClip audioClip, float pitchOverride)
    {
        return Instance.PlaySoundInternal(audioClip.name, new Vector3(), null, pitchOverride);
    }
    public static AudioSource PlaySound(AudioClip audioClip, Vector3 position = new Vector3(), Transform followTarget = null, float pitchOverride = 0)
    {
        return Instance.PlaySoundInternal(audioClip.name, position, followTarget, pitchOverride);
    }
    public static AudioSource PlaySound(string name, float pitchOverride)
    {
        return Instance.PlaySoundInternal(name, new Vector3(), null, pitchOverride);
    }
    public static AudioSource PlaySound(string name, Vector3 position = new Vector3(), Transform followTarget = null, float pitchOverride = 0)
    {
        return Instance.PlaySoundInternal(name, position, followTarget, pitchOverride);
    }

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            // Do not modify _instance here. It will be assigned in awake
            return new GameObject("(singleton) SoundManager").AddComponent<SoundManager>();
        }
    }

    void Awake()
    {
        // Only one instance of SoundManager at a time!
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }
}

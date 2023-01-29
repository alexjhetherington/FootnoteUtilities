using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, List<SoundEntry>> groupedSoundEntries = new Dictionary<
        string,
        List<SoundEntry>
    >();

    private Dictionary<string, List<AudioSource>> currentSounds = new Dictionary<
        string,
        List<AudioSource>
    >();

    private GameObject prototype;

    private AudioSource music;

    #region Init
    private void Init()
    {
        prototype = new GameObject("Sound Prototype");
        DontDestroyOnLoad(prototype);
        AudioSource audioSource = prototype.AddComponent<AudioSource>();
        AudioSourceLifeCycle audioSourceLifeCycle = prototype.AddComponent<AudioSourceLifeCycle>();
        audioSourceLifeCycle.audioSource = audioSource;

        prototype.SetActive(false);

        var soundLibrary2 = Resources.LoadAll<SoundLibrary>("SoundLibraries");
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
                foreach (var entry in lib.GetSoundEntries())
                {
                    if (!groupedSoundEntries.ContainsKey(entry.Key))
                        groupedSoundEntries[entry.Key] = entry.Value;
                    else
                        groupedSoundEntries[entry.Key].AddRange(entry.Value);
                }
            }
        }
    }
    #endregion

    #region Update
    void Update()
    {
        foreach (List<AudioSource> clipGroup in currentSounds.Values)
        {
            if (clipGroup.SortByDistanceIfUnordered(activeAudioListener.transform.position))
            {
                LimitMaxSounds(clipGroup[0].clip.name);
            }
        }
    }
    #endregion

    #region FX
    public AudioSource PlaySoundInternal(
        AudioClip audioClip,
        Vector3 position,
        Transform followTarget,
        float pitchOverride
    )
    {
        return PlaySoundInternal(audioClip.name, position, followTarget, pitchOverride);
    }
    public AudioSource PlaySoundInternal(
        string name,
        Vector3 position,
        Transform followTarget,
        float pitchOverride
    )
    {
        List<SoundEntry> soundEntries;

        if (!groupedSoundEntries.TryGetValue(name, out soundEntries))
        {
            Debug.LogError("Sound was not found in library: " + name);
            foreach (var entry in groupedSoundEntries.Keys)
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
        audioSource.panStereo = 0;
        audioSource.outputAudioMixerGroup = soundEntry.audioMixerGroup;

        var audioSourceLowPass = go.GetOrAddComponent<AudioLowPassFilter>();
        audioSourceLowPass.enabled = false;

        if (soundEntry.customFalloff != null)
        {
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.maxDistance =
                soundEntry.customFalloff.volumeFalloff[
                    soundEntry.customFalloff.volumeFalloff.length - 1
                ].time;

            audioSource.SetCustomCurve(
                AudioSourceCurveType.CustomRolloff,
                soundEntry.customFalloff.volumeFalloff
            );

            //We scale the animation curve so you can draw it in absolute values 22,000khz to 0khz, plotted against units
            if (soundEntry.customFalloff.useLowPass)
            {
                audioSourceLowPass.enabled = true;

                var lowPassCurveScaled = new AnimationCurve();
                Keyframe[] points = new Keyframe[soundEntry.customFalloff.lowPassFalloff.length];
                for (int i = 0; i < soundEntry.customFalloff.lowPassFalloff.length; i++)
                {
                    var key = soundEntry.customFalloff.lowPassFalloff[i];
                    key.time = key.time / audioSource.maxDistance;
                    key.value = key.value / 22000;
                    key.inTangent = key.inTangent / audioSource.maxDistance; //Don't know why this works
                    key.outTangent = key.outTangent / audioSource.maxDistance; //Don't know why this works
                    points[i] = key;
                }
                lowPassCurveScaled.keys = points;

                audioSourceLowPass.customCutoffCurve = lowPassCurveScaled;
            }
        }
        else
        {
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.maxDistance = 500; //Default
        }

        if (soundEntry.pitchVariation != 0)
            audioSource.pitch =
                1 + Random.Range(-soundEntry.pitchVariation, soundEntry.pitchVariation);
        else
            audioSource.pitch = 1;

        if (pitchOverride != 0)
            audioSource.pitch = pitchOverride;

        if (followTarget != null)
            audioSourceLifecycle.SetFollowTarget(followTarget);

        if (soundEntry.maxSimultaneousPlaying > 0)
        {
            var existing = currentSounds.SafeGet(soundEntry.audioClip.name);
            existing.Add(audioSource);

            if (existing.Count > soundEntry.maxSimultaneousPlaying)
            {
                audioSource.volume = 0; // Revisit if audio fade in becomes a problem for new sounds
            }
        }

        audioSource.Play();
        return audioSource;
    }

    public void Release(AudioSource audioSource)
    {
        if (currentSounds.ContainsKey(audioSource.clip.name))
        {
            currentSounds[audioSource.clip.name].Remove(audioSource);
            LimitMaxSounds(audioSource.clip.name); //In case a volume 0 sound should now be faded in
        }

        SoundManager.CancelExistingFade(audioSource);
        audioSource.Stop();
        PoolManager.ReleaseObject(audioSource.gameObject);
    }

    //TODO make sure this is the correct way round
    private void LimitMaxSounds(string audioClipName)
    {
        //Audioclip name should only have one entry
        var soundEntry = groupedSoundEntries[audioClipName][0];
        var audioSources = currentSounds[audioClipName];

        if (soundEntry.maxSimultaneousPlaying == 0)
        {
            Debug.LogWarning(
                "Tried to limit the number of sounds for an unlimited entry: " + audioClipName,
                this
            );
            return;
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i < soundEntry.maxSimultaneousPlaying)
            {
                //Play the top N sounds
                FadeTo(audioSources[i], soundEntry.volume, 5f);
            }
            else
            {
                //Mute the rest
                FadeTo(audioSources[i], 0, 5f);
            }
        }
    }

    #endregion

    #region Music
    public void PlayMusicInternal(string name, float fadeTime)
    {
        if (music == null)
        {
            var go = new GameObject("Music AudioSource");
            DontDestroyOnLoad(go);
            AudioSource audioSource = go.AddComponent<AudioSource>();
            music = audioSource;
        }

        List<SoundEntry> soundEntries;
        if (!groupedSoundEntries.TryGetValue(name, out soundEntries))
        {
            Debug.LogError("Sound was not found in library: " + name);
            foreach (var entry in groupedSoundEntries.Keys)
            {
                Debug.Log(entry);
            }
            return;
        }
        SoundEntry soundEntry = soundEntries[0];

        StartCoroutine(FadeNextMusic(soundEntry, fadeTime));
    }

    public void StopMusicInternal(float fadeTime)
    {
        if (music != null)
            StartCoroutine(FadeOut_ManualCoroutine(music, fadeTime));
    }

    private IEnumerator FadeNextMusic(SoundEntry soundEntry, float fadeTime)
    {
        yield return StartCoroutine(FadeOut_ManualCoroutine(music, fadeTime));
        music.clip = soundEntry.audioClip;
        music.loop = soundEntry.looping;
        music.outputAudioMixerGroup = soundEntry.audioMixerGroup;
        music.Play();
    }
    #endregion

    #region Fading FX
    private static Dictionary<AudioSource, Coroutine> fadingAudioSources = new Dictionary<
        AudioSource,
        Coroutine
    >();

    public static void FadeOut(AudioSource audioSource, float fadeTime)
    {
        CancelExistingFade(audioSource);
        fadingAudioSources.Add(
            audioSource,
            Coroutiner.Instance.StartCoroutine(_FadeOut(audioSource, fadeTime))
        );
    }
    //Useful if you want to do something after the sound fades out - but will not be halted if you try to fade it in again!
    public static IEnumerator FadeOut_ManualCoroutine(AudioSource audioSource, float fadeTime)
    {
        CancelExistingFade(audioSource);
        return _FadeOut(audioSource, fadeTime);
    }
    private static IEnumerator _FadeOut(AudioSource audioSource, float FadeTime)
    {
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }

    public static void FadeIn(AudioSource audioSource, float fadeTime)
    {
        CancelExistingFade(audioSource);
        fadingAudioSources.Add(
            audioSource,
            Coroutiner.Instance.StartCoroutine(_FadeIn(audioSource, fadeTime))
        );
    }
    private static IEnumerator _FadeIn(AudioSource audioSource, float FadeTime)
    {
        float targetVol = audioSource.volume;
        audioSource.volume = 0;

        audioSource.Stop();
        audioSource.Play();

        while (audioSource.volume < targetVol)
        {
            audioSource.volume = Mathf.Min(
                targetVol,
                audioSource.volume + Time.deltaTime / FadeTime
            );

            yield return null;
        }
    }

    public static void FadeTo(AudioSource audioSource, float targetVolume, float speed)
    {
        CancelExistingFade(audioSource);
        fadingAudioSources.Add(
            audioSource,
            Coroutiner.Instance.StartCoroutine(_FadeTo(audioSource, targetVolume, speed))
        );
    }
    private static IEnumerator _FadeTo(AudioSource audioSource, float targetVolume, float speed)
    {
        while (audioSource.volume != targetVolume)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                targetVolume,
                speed * Time.deltaTime
            );

            yield return null;
        }
    }

    private static void CancelExistingFade(AudioSource audioSource)
    {
        if (fadingAudioSources.ContainsKey(audioSource))
        {
            if (fadingAudioSources[audioSource] != null)
                Coroutiner.Instance.StopCoroutine(fadingAudioSources[audioSource]);

            fadingAudioSources.Remove(audioSource);
        }
    }
    #endregion

    #region Public Static Interface
    public static AudioSource PlaySound(AudioClip audioClip, float pitchOverride = 0)
    {
        if (audioClip == null)
            return null;

        return Instance.PlaySoundInternal(audioClip.name, new Vector3(), null, pitchOverride);
    }
    public static AudioSource PlaySound(
        AudioClip audioClip,
        Vector3 position,
        float pitchOverride = 0
    )
    {
        if (audioClip == null)
            return null;

        return Instance.PlaySoundInternal(audioClip.name, position, null, pitchOverride);
    }
    public static AudioSource PlaySound(
        AudioClip audioClip,
        Transform followTarget,
        float pitchOverride = 0
    )
    {
        if (audioClip == null)
            return null;

        return Instance.PlaySoundInternal(
            audioClip.name,
            new Vector3(),
            followTarget,
            pitchOverride
        );
    }

    public static AudioSource PlaySound(string name, float pitchOverride = 0)
    {
        return Instance.PlaySoundInternal(name, new Vector3(), null, pitchOverride);
    }
    public static AudioSource PlaySound(
        string name,
        Transform followTarget,
        float pitchOverride = 0
    )
    {
        return Instance.PlaySoundInternal(name, new Vector3(), followTarget, pitchOverride);
    }
    public static AudioSource PlaySound(string name, Vector3 position, float pitchOverride = 0)
    {
        return Instance.PlaySoundInternal(name, position, null, pitchOverride);
    }

    public static void PlayMusic(AudioClip audioClip, float fadeTime)
    {
        if (audioClip == null)
            return;

        Instance.PlayMusicInternal(audioClip.name, fadeTime);
    }
    public static void PlayMusic(string name, float fadeTime)
    {
        Instance.PlayMusicInternal(name, fadeTime);
    }
    public static void StopMusic(float fadeTime)
    {
        Instance.StopMusicInternal(fadeTime);
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

    private static AudioListener _activeAudioListener;
    public static AudioListener activeAudioListener
    {
        get
        {
            if (!_activeAudioListener || !_activeAudioListener.isActiveAndEnabled)
            {
                Debug.Log("Finding a new audio listener");
                _activeAudioListener = FindObjectOfType<AudioListener>();
            }

            return _activeAudioListener;
        }
        // The above approach should work for most games
        // If you switch audio listeners mid scene, consider
        // manually setting the audio listener for better performance
        set { _activeAudioListener = value; }
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
    #endregion
}

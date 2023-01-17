using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableAnimationController : MonoBehaviour
{
    [SerializeField]
    private int maxClips = 20;

    private PlayableGraph _playableGraph;
    private AnimationMixerPlayable _animationMixerPlayable;
    private PlayableOutput _playableOutput;

    private Animator _animator;

    private int nextIndex;
    private Dictionary<AnimationClip, int> clipIndex;
    private ClipInfo[] clipInfos;

    private void Awake()
    {
        clipIndex = new Dictionary<AnimationClip, int>(maxClips);
        clipInfos = new ClipInfo[maxClips];

        _animator = this.GetOrAddComponent<Animator>();
        _animator.runtimeAnimatorController = null;

        _playableGraph = PlayableGraph.Create();
        _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        _animationMixerPlayable = AnimationMixerPlayable.Create(_playableGraph, maxClips);

        _playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator);
        _playableOutput.SetSourcePlayable(_animationMixerPlayable);

        _playableGraph.Play();
    }

    private void OnDestroy()
    {
        _playableGraph.Destroy();
    }

    public void Play(AnimationClip animationClip)
    {
        int index = GetIndex(animationClip);
        ResetClips();
        _animationMixerPlayable.SetInputWeight(index, 1);
        _animationMixerPlayable.GetInput(index).SetTime(0);
        _playableGraph.Play();
    }

    public void SetSpeed(AnimationClip animationClip, float speed)
    {
        int index = GetIndex(animationClip);
        _animationMixerPlayable.GetInput(index).SetSpeed(speed);
    }

    public void CrossFade(AnimationClip animationClip, float fadeTime)
    {
        int index = GetIndex(animationClip);

        for (int i = 0; i < clipInfos.Length; i++)
        {
            if (i == index)
            {
                if (!clipInfos[i].isFadeIn || !clipInfos[i].clip.isLooping)
                {
                    if (clipInfos[i].lerpingCoroutine != null)
                        StopCoroutine(clipInfos[i].lerpingCoroutine);

                    clipInfos[i].lerpingCoroutine = StartCoroutine(FadeClip(i, true, fadeTime));
                }
            }
            else
            {
                if (clipInfos[i].isFadeIn || clipInfos[i].weight == 1)
                {
                    if (clipInfos[i].lerpingCoroutine != null)
                        StopCoroutine(clipInfos[i].lerpingCoroutine);

                    clipInfos[i].lerpingCoroutine = StartCoroutine(FadeClip(i, false, fadeTime));
                }
            }
        }
    }

    public void Stop()
    {
        ResetClips();
        _playableGraph.Stop();
    }

    private int GetIndex(AnimationClip animationClip)
    {
        int index;
        if (clipIndex.ContainsKey(animationClip))
        {
            index = clipIndex[animationClip];
        }
        else
        {
            //Debug.Log("Animation not found, adding to the playable graph");
            var clipPlayable = AnimationClipPlayable.Create(_playableGraph, animationClip);
            index = nextIndex++;
            clipIndex.Add(animationClip, index);
            _animationMixerPlayable.ConnectInput(index, clipPlayable, 0);
            clipInfos[index].clip = animationClip;
        }
        return index;
    }

    private IEnumerator FadeClip(int clipIndex, bool fadeIn, float fadeTime)
    {
        if (fadeTime > clipInfos[clipIndex].clip.length)
            Debug.LogWarning(
                string.Format(
                    "Fade time [{0}] is greater than clip length [{1}] for clip [{2}]",
                    fadeTime,
                    clipInfos[clipIndex].clip.length,
                    clipInfos[clipIndex].clip.name
                )
            );

        if (fadeIn)
            _animationMixerPlayable.GetInput(clipIndex).SetTime(0);

        clipInfos[clipIndex].isFadeIn = fadeIn;
        float endWeight = fadeIn ? 1 : 0;

        float progress = clipInfos[clipIndex].weight;
        if (!fadeIn)
            progress = 1 - progress;

        while (progress < 1)
        {
            progress = Mathf.Min(1, progress + Time.deltaTime / fadeTime);
            var weight = Mathf.Lerp(fadeIn ? 0 : 1, fadeIn ? 1 : 0, progress);
            SetClipWeight(clipIndex, weight);

            if (progress == 1)
                clipInfos[clipIndex].lerpingCoroutine = null;

            yield return null;
        }
    }

    private void SetClipWeight(int index, float weight)
    {
        clipInfos[index].weight = weight;
        _animationMixerPlayable.SetInputWeight(index, weight);
    }

    private void ResetClips()
    {
        for (int i = 0; i < clipInfos.Length; i++)
        {
            SetClipWeight(i, 0);
            clipInfos[i].isFadeIn = false;

            if (clipInfos[i].lerpingCoroutine != null)
            {
                StopCoroutine(clipInfos[i].lerpingCoroutine);
                clipInfos[i].lerpingCoroutine = null;
            }
        }
    }

    private struct ClipInfo
    {
        public AnimationClip clip;

        public float weight;
        public Coroutine lerpingCoroutine;
        public bool isFadeIn;
    }
}

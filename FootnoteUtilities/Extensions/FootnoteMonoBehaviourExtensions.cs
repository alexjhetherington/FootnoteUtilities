using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteMonoBehaviourExtensions
{
    public static Coroutine DoAfter(this MonoBehaviour monoBehaviour, float wait, Action action)
    {
        return monoBehaviour.StartCoroutine(DoAfter(wait, action));
    }

    private static IEnumerator DoAfter(float wait, Action action)
    {
        yield return new WaitForSeconds(wait);
        action.Invoke();
    }

    public static Coroutine DoOnProgress(
        this MonoBehaviour monoBehaviour,
        float length,
        Action<float> action
    )
    {
        return monoBehaviour.StartCoroutine(DoOnProgress(length, action));
    }

    private static IEnumerator DoOnProgress(float length, Action<float> action)
    {
        float progress = 0;

        while (progress < 1)
        {
            yield return null;
            progress = Mathf.Min(1, progress + Time.deltaTime / length);
            action.Invoke(progress);
        }
    }

    public static Coroutine DoAtEndOfFrame(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(DoAtEndOfFrame(action));
    }

    private static IEnumerator DoAtEndOfFrame(Action action)
    {
        yield return new WaitForEndOfFrame();
        action.Invoke();
    }
}

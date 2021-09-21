using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteMonoBehaviourExtensions
{
    public static void DoAfter(this MonoBehaviour monoBehaviour, float wait, Action action) =>
        monoBehaviour.StartCoroutine(DoAfter(wait, action));

    private static IEnumerator DoAfter(float wait, Action action)
    {
        yield return new WaitForSeconds(wait);
        action.Invoke();
    }

    public static void DoOnProgress(this MonoBehaviour monoBehaviour, float length, Action<float> action) =>
        monoBehaviour.StartCoroutine(DoOnProgress(length, action));

    private static IEnumerator DoOnProgress(float length, Action<float> action)
    {
        float progress = 0;

        while(progress < 1)
        {
            yield return null;
            progress = Mathf.Min(1, progress + Time.deltaTime);
            action.Invoke(progress);
        }
    }
}

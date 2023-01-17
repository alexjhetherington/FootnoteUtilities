using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTransition : MonoBehaviour, Transition
{
    public float fadeOutTime;
    public float fadeInTime;
    public float obscuredTime;
    public Image image;

    public void Obscure(Action onScreenObscured)
    {
        StartCoroutine(_Obscure(onScreenObscured));
    }

    private IEnumerator _Obscure(Action onScreenObscured)
    {
        yield return StartCoroutine(Fade(1f, fadeOutTime));
        onScreenObscured.Invoke();
    }

    public void Unobscure(Action onScreenUnobscured)
    {
        StartCoroutine(_Unobscure(onScreenUnobscured));
    }

    private IEnumerator _Unobscure(Action onScreenUnobscured)
    {
        yield return new WaitForSecondsRealtime(obscuredTime);

        //Little bit of a hack but basically forces the transition to happen after a few frames
        yield return null;
        yield return new WaitForSecondsRealtime(0.2f);

        yield return StartCoroutine(Fade(0, fadeInTime));
        onScreenUnobscured.Invoke();
    }

    private IEnumerator Fade(float targetAlpha, float fadeTime)
    {
        while (image.color.a != targetAlpha)
        {
            var newAlpha = Mathf.MoveTowards(
                image.color.a,
                targetAlpha,
                Time.unscaledDeltaTime / fadeTime
            );
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }
    }
}

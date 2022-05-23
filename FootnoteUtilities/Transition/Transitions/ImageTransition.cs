using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTransition : MonoBehaviour, Transition
{
    public float fadeTime;
    public float obscuredTime;
    public Image image;

    public void Obscure(Action onScreenObscured)
    {
        StartCoroutine(_Obscure(onScreenObscured));
    }

    private IEnumerator _Obscure(Action onScreenObscured)
    {
        yield return StartCoroutine(Fade(1f));
        onScreenObscured.Invoke();
    }

    public void Unobscure(Action onScreenUnobscured)
    {
        StartCoroutine(_Unobscure(onScreenUnobscured));
    }

    private IEnumerator _Unobscure(Action onScreenUnobscured)
    {
        yield return new WaitForSecondsRealtime(obscuredTime);
        yield return StartCoroutine(Fade(0));
        onScreenUnobscured.Invoke();
    }

    private IEnumerator Fade(float targetAlpha)
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

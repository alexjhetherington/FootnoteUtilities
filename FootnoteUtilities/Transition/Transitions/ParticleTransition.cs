using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTransition : MonoBehaviour, Transition
{
    public float waitTime;
    public ParticleSystem ps;

    public void Obscure(Action onScreenObscured)
    {
        ps.Play();
        StartCoroutine(_Obscure(onScreenObscured));
    }

    private IEnumerator _Obscure(Action onScreenObscured)
    {
        yield return new WaitForSeconds(waitTime);
        onScreenObscured.Invoke();
    }

    public void Unobscure(Action onScreenUnobscured)
    {
        ps.Stop();
        onScreenUnobscured.Invoke();
    }
}

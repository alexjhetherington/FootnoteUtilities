using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAnimation : MonoBehaviour
{
    public AnimationClip defaultAnimation;

    [Resolve]
    public PlayableAnimationController animationController;

    void Start()
    {
        animationController.Play(defaultAnimation);
    }
}

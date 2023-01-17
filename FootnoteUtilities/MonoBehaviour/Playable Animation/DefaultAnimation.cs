using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAnimation : MonoBehaviour
{
    public AnimationClip defaultAnimation;

    public float speed = 1f;

    [Resolve]
    public PlayableAnimationController animationController;

    void Start()
    {
        animationController.SetSpeed(defaultAnimation, speed);
        animationController.Play(defaultAnimation);
    }

    [Button]
    void UpdateSpeed()
    {
        animationController.SetSpeed(defaultAnimation, speed);
    }
}

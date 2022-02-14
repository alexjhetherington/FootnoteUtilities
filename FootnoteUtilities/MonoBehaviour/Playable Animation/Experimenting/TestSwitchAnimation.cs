using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitchAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationClip clip1;
    [SerializeField]
    private AnimationClip clip2;

    [SerializeField]
    private PlayableAnimationController animator1;
    [SerializeField]
    private Animator animator2;

    [Button]
    private void First()
    {
        if (animator1 != null)
            animator1.CrossFade(clip1, 1f);

        if (animator2 != null)
            animator2.CrossFade(clip1.name, 1f);
    }

    [Button]
    private void Second()
    {
        if (animator1 != null)
            animator1.CrossFade(clip2, 1f);

        if (animator2 != null)
            animator2.CrossFade(clip2.name, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomFalloff : ScriptableObject
{
    public AnimationCurve volumeFalloff;
    public bool useLowPass;
    public AnimationCurve lowPassFalloff;
}

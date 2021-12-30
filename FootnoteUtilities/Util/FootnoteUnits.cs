using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteUnits
{
    public static float linearToDecibels(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 10f);
        return 20f * Mathf.Log10(linear);
    }
    public static float decibelsToLinear(float decibels)
    {
        decibels = Mathf.Clamp(decibels, -80f, 20f);
        return Mathf.Pow(10f, decibels / 20f);
    }
}

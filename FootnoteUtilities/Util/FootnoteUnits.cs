using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteUnits
{
    public static float linearToDecibels(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 15f) / 10;
        return 40f * Mathf.Log10(linear);
    }
    public static float decibelsToLinear(float decibels)
    {
        return Mathf.Pow(10f, decibels / 40f) * 10;
    }
}

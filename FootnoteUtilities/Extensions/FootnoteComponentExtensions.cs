using System;
using System.Collections;
using UnityEngine;

/// <summary>
///     Extension methods for UnityEngine.Component.
///     Ref: https://github.com/mminer/unity-extensions/blob/master/ComponentExtensions.cs
/// </summary>
public static class FootnoteComponentExtensions
{
    public static T AddComponent<T>(this UnityEngine.Component component)
        where T : UnityEngine.Component => component.gameObject.AddComponent<T>();

    public static bool HasComponent<T>(this UnityEngine.Component component)
        where T : UnityEngine.Component => component.GetComponent<T>() != null;

    public static T GetOrAddComponent<T>(this UnityEngine.Component component)
        where T : UnityEngine.Component
    {
        T other = component.GetComponent<T>();

        if (other != null)
            return other;

        return component.AddComponent<T>();
    }
}

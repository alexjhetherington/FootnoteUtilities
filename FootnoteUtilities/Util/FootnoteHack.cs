using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteHack
{
    //Deprecated by Unity 2020 which allows calls to FindObjectOfType with a bool flag to specify whether
    //inactive objects should be included
    public static T[] FindObjectsOfTypeIncludingDisabled<T>()
    {
        var ActiveScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var RootObjects = ActiveScene.GetRootGameObjects();
        var MatchObjects = new List<T>();

        foreach (var ro in RootObjects)
        {
            var Matches = ro.GetComponentsInChildren<T>(true);
            MatchObjects.AddRange(Matches);
        }

        return MatchObjects.ToArray();
    }

    public static Component[] FindObjectsOfTypeIncludingDisabled(Type type)
    {
        var ActiveScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var RootObjects = ActiveScene.GetRootGameObjects();
        var MatchObjects = new List<Component>();

        foreach (var ro in RootObjects)
        {
            var Matches = ro.GetComponentsInChildren(type, true);
            MatchObjects.AddRange(Matches);
        }

        return MatchObjects.ToArray();
    }

    public static T[] FindObjectsOfTypeIncludingDisabled<T>(GameObject[] rootObjects)
    {
        var MatchObjects = new List<T>();

        foreach (var ro in rootObjects)
        {
            var Matches = ro.GetComponentsInChildren<T>(true);
            MatchObjects.AddRange(Matches);
        }

        return MatchObjects.ToArray();
    }
}

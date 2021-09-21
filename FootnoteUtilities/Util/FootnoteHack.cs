using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteHack
{
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
}

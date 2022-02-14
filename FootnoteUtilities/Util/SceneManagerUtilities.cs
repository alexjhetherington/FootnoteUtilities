using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtilities
{
    public static int GetBuildIndexByName(string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            if (path.Contains(name))
                return i;
        }

        throw new System.Exception("Scene does not exist: " + name);
    }
}

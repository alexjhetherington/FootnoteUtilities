using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public static class StaticSceneHelper
{
    public enum LoadType
    {
        Sync,
        Async
    };

    private static Dictionary<int, CoroutineResult<GameObject[]>> sceneRootObjects = new Dictionary<
        int,
        CoroutineResult<GameObject[]>
    >();
    private static Dictionary<int, Dictionary<Type, Object>> sceneTypedObjects = new Dictionary<
        int,
        Dictionary<Type, Object>
    >();

    //When Domain Loading is switched off
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        sceneRootObjects.Clear();
        sceneTypedObjects.Clear();
    }

    public static void LoadSceneAsStatic(LoadType loadType, int sceneIndex)
    {
        CoroutineResult<GameObject[]> result;
        if (loadType == LoadType.Sync)
            result = new CoroutineResult<GameObject[]>(UnpackSync_GameObjects(sceneIndex));
        else
            result = new CoroutineResult<GameObject[]>(UnpackAsync_GameObjects(sceneIndex));

        if (!sceneRootObjects.ContainsKey(sceneIndex))
            sceneRootObjects.Add(sceneIndex, result);

        if (!sceneTypedObjects.ContainsKey(sceneIndex))
            sceneTypedObjects.Add(sceneIndex, new Dictionary<Type, Object>());
    }

    public static CoroutineResult<GameObject[]> GetGameObjectsFromSceneAsStatic(
        LoadType loadType,
        int sceneIndex
    )
    {
        LoadSceneAsStatic(loadType, sceneIndex);
        return sceneRootObjects[sceneIndex];
    }

    public static CoroutineResult<T> GetComponentFromSceneAsStatic<T>(
        LoadType loadType,
        int sceneIndex
    )
    {
        LoadSceneAsStatic(loadType, sceneIndex);

        if (!sceneTypedObjects[sceneIndex].ContainsKey(typeof(T)))
        {
            var result = new CoroutineResult<T>(ExtractTypedObject<T>(sceneIndex));
            sceneTypedObjects[sceneIndex].Add(typeof(T), result);
            return result;
        }
        else
        {
            return (CoroutineResult<T>)sceneTypedObjects[sceneIndex][typeof(T)];
        }
    }

    private static IEnumerator UnpackSync_GameObjects(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();

        Debug.Log("Scene loaded");
        GameObject[] gos = DoNotDestroyAndUnloadScene(sceneIndex);
        yield return gos;
    }

    private static IEnumerator UnpackAsync_GameObjects(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        GameObject[] gos = DoNotDestroyAndUnloadScene(sceneIndex);
        yield return gos;
    }

    private static IEnumerator ExtractTypedObject<T>(int sceneIndex)
    {
        var gosResult = sceneRootObjects[sceneIndex];
        yield return gosResult;
        var gos = gosResult.value;

        bool found = false;
        foreach (GameObject go in gos)
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                found = true;
                yield return component;
            }
        }

        if (!found)
            throw new Exception(
                "Object of type: " + typeof(T) + " not found in scene: " + sceneIndex
            );
    }

    private static GameObject[] DoNotDestroyAndUnloadScene(int sceneIndex)
    {
        GameObject[] gos = SceneManager.GetSceneByBuildIndex(sceneIndex).GetRootGameObjects();

        foreach (GameObject go in gos)
            UnityEngine.Object.DontDestroyOnLoad(go);

        SceneManager.UnloadSceneAsync(sceneIndex);

        return gos;
    }
}

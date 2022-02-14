using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public static class ScenePack
{
    private static Dictionary<int, GameObject[]> sceneRootObjects = new Dictionary<
        int,
        GameObject[]
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

    public static void UnpackScene(int sceneIndex, System.Action<GameObject[]> unpacked)
    {
        if (!sceneRootObjects.ContainsKey(sceneIndex))
            Coroutiner.Instance.StartCoroutine(_UnpackScene(sceneIndex, unpacked));
        else
            unpacked.Invoke(sceneRootObjects[sceneIndex]);
    }

    private static IEnumerator _UnpackScene(int sceneIndex, Action<GameObject[]> unpacked)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();

        GameObject[] gos = SaveObjectsAndUnloadScene(sceneIndex);
        unpacked.Invoke(gos);
    }

    public static void UnpackScene<T>(int sceneIndex, Action<T> unpacked)
    {
        if (!sceneTypedObjects.ContainsKey(sceneIndex))
            Coroutiner.Instance.StartCoroutine(_UnpackScene(sceneIndex, unpacked));
        else if (!sceneTypedObjects[sceneIndex].ContainsKey(typeof(T)))
            unpacked.Invoke(PutTypedObject<T>(sceneIndex, sceneRootObjects[sceneIndex]));
        else
            unpacked.Invoke((T)sceneTypedObjects[sceneIndex][typeof(T)]);
    }

    private static IEnumerator _UnpackScene<T>(int sceneIndex, Action<T> unpacked)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        GameObject[] gos = SaveObjectsAndUnloadScene(sceneIndex);
        T component = PutTypedObject<T>(sceneIndex, gos);
        unpacked.Invoke(component);
    }

    private static GameObject[] SaveObjectsAndUnloadScene(int sceneIndex)
    {
        GameObject[] gos = SceneManager.GetSceneByBuildIndex(sceneIndex).GetRootGameObjects();

        foreach (GameObject go in gos)
            UnityEngine.Object.DontDestroyOnLoad(go);

        sceneRootObjects.Add(sceneIndex, gos);
        sceneTypedObjects.Add(sceneIndex, new Dictionary<Type, Object>());

        SceneManager.UnloadSceneAsync(sceneIndex);

        return gos;
    }

    private static T PutTypedObject<T>(int sceneIndex, GameObject[] gos)
    {
        foreach (GameObject go in gos)
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                sceneTypedObjects[sceneIndex].Add(typeof(T), component);
                return component;
            }
        }

        throw new Exception("Object of type: " + typeof(T) + " not found in scene: " + sceneIndex);
    }

    public static void Unpack<T>(this Scene scene, Action<T> unpacked)
    {
        UnpackScene(scene.buildIndex, unpacked);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[DefaultExecutionOrder(int.MinValue)]
public class LinkedScene : MonoBehaviour
{
    public static event System.Action OnAllScenesLoadedAwake;
    public static event System.Action OnAllScenesLoadedStart;
    private static Coroutine notifier;

    [SerializeField]
    private SceneReference mainScene;

    [SerializeField]
    private SceneReference[] additiveScenes;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (mainScene != null && !string.IsNullOrEmpty(mainScene.SceneName))
            SceneManager.LoadScene(mainScene.SceneName, LoadSceneMode.Single);

        foreach (SceneReference scene in additiveScenes)
            if (!SceneManager.GetSceneByName(scene.SceneName).isLoaded)
                SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Additive);

        notifier = Coroutiner.Instance.StartCoroutine(NotifyAllLoaded());
        Destroy(gameObject);
    }

    IEnumerator NotifyAllLoaded()
    {
        if (notifier != null)
        {
            Coroutiner.Instance.StopCoroutine(notifier);
        }

        yield return null;
        //Debug.Log("Notify!");
        OnAllScenesLoadedAwake?.Invoke();
        OnAllScenesLoadedStart?.Invoke();
    }

#if UNITY_EDITOR

    [Button]
    void LoadAllScenesEditor()
    {
        if (mainScene != null)
            EditorSceneManager.OpenScene(mainScene.ScenePath, OpenSceneMode.Single);

        foreach (SceneReference scene in additiveScenes)
            EditorSceneManager.OpenScene(scene.ScenePath, OpenSceneMode.Additive);
    }

    [Button]
    void LoadAdditiveScenesEditor()
    {
        foreach (SceneReference scene in additiveScenes)
            EditorSceneManager.OpenScene(scene.ScenePath, OpenSceneMode.Additive);
    }
#endif
}

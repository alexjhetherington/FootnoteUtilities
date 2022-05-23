using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class StaticScene : ScriptableObject
{
    public SceneReference sceneReference;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BootstrapScences()
    {
        var staticScenes = Resources.LoadAll<StaticScene>("StaticScenes");
        foreach (StaticScene staticScene in staticScenes)
        {
            StaticSceneHelper.LoadSceneAsStatic(
                StaticSceneHelper.LoadType.Sync,
                SceneManagerUtilities.GetBuildIndexByName(staticScene.sceneReference.SceneName)
            );
        }
    }
}

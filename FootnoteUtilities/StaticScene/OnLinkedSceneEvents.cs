using UnityEngine;
using UnityEngine.Events;

public class OnLinkedSceneEvents : MonoBehaviour
{
    public UnityEvent AllScenesAwake;
    public UnityEvent AllScenesStart;

    // Start is called before the first frame update
    void Awake()
    {
        LinkedScene.OnAllScenesLoadedAwake += InvokeAllScenesAwake;
        LinkedScene.OnAllScenesLoadedStart += InvokeAllScenesStart;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        LinkedScene.OnAllScenesLoadedAwake -= InvokeAllScenesAwake;
        LinkedScene.OnAllScenesLoadedStart -= InvokeAllScenesStart;
    }

    void InvokeAllScenesAwake()
    {
        AllScenesAwake.Invoke();
    }

    void InvokeAllScenesStart()
    {
        AllScenesStart.Invoke();
    }
}

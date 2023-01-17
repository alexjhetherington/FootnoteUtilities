using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class OnAwakeEarlyEvent : MonoBehaviour
{
    public UnityEvent unityEvent;

    void Awake()
    {
        unityEvent.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartNextFrameEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent unityEvent;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        unityEvent.Invoke();
    }
}

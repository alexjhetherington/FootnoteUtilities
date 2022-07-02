using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerTagEnter : MonoBehaviour
{
    public Tag checkTag;
    public UnityEvent unityEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.HasTag(checkTag))
            unityEvent.Invoke();
    }
}

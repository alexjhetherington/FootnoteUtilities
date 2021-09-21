using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Useful when you want a trigger prefab where the trigger might be very different shapes
public class ColliderProxyEvents : MonoBehaviour
{
    public ColliderEvent OnTriggerEnterProxy;
    public ColliderEvent OnTriggerStayProxy;
    public ColliderEvent OnTriggerExitProxy;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterProxy.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayProxy.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitProxy.Invoke(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseVoidEventOnDisable : MonoBehaviour
{
    public VoidEvent voidEvent;

    void OnDisable()
    {
        Debug.Log("Raising Event: " + voidEvent.name);
        voidEvent.Raise();
    }
}

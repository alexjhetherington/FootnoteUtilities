using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringListener : MonoBehaviour
{
    public StringVariable variableReference;
    public StringEvent variableEvent;

    public void OnChanged(string newValue)
    {
        variableEvent.Invoke(newValue);
    }

    private void Awake()
    {
        variableReference.RegisterListener(OnChanged);
    }

    private void OnDestroy()
    {
        variableReference.UnregisterListener(OnChanged);
    }
}

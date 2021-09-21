using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanListener : MonoBehaviour
{
    public BooleanVariable variableReference;
    public BooleanEvent variableEvent;

    public void OnChanged(bool newValue)
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

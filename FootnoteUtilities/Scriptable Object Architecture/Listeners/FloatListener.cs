using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatListener : MonoBehaviour
{
    public FloatVariable variableReference;
    public FloatEvent variableEvent;

    public void OnChanged(float newValue)
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

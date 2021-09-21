using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    public List<VoidEvent> references;
    public UnityEvent action;

    public void Invoke()
    {
        action.Invoke();
    }

    private void OnEnable()
    {
        foreach (VoidEvent voidEvent in references)
            voidEvent.RegisterListener(Invoke);
    }

    private void OnDisable()
    {
        foreach (VoidEvent voidEvent in references)
            voidEvent.UnregisterListener(Invoke);
    }
}

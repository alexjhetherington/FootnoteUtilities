using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on https://github.com/roboryantron/Unite2017
[CreateAssetMenu]
public class VoidEvent : ScriptableObject
{
    private readonly List<Action> eventListener = new List<Action>();

    public void Raise()
    {
        for (int i = eventListener.Count - 1; i >= 0; i--)
            eventListener[i].Invoke();
    }

    public void RegisterListener(Action listener)
    {
        if (!eventListener.Contains(listener))
            eventListener.Add(listener);
    }

    public void UnregisterListener(Action listener)
    {
        if (eventListener.Contains(listener))
            eventListener.Remove(listener);
    }
}

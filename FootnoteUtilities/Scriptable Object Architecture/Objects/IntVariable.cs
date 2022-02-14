using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on https://github.com/roboryantron/Unite2017
[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int _value;

    public int Value
    {
        get { return _value; }
        set
        {
            //Not a good idea. Reseting values to their 'default' on scene load will not trigger an event
            //if the value was at the default in the previous scene
            //if (_value == value) return;

            _value = value;

            for (int i = variableChangedListeners.Count - 1; i >= 0; i--)
                variableChangedListeners[i].Invoke(_value);
        }
    }

    private readonly List<Action<int>> variableChangedListeners = new List<Action<int>>();

    public void RegisterListener(Action<int> listener)
    {
        if (!variableChangedListeners.Contains(listener))
            variableChangedListeners.Add(listener);
    }

    public void UnregisterListener(Action<int> listener)
    {
        if (variableChangedListeners.Contains(listener))
            variableChangedListeners.Remove(listener);
    }
}

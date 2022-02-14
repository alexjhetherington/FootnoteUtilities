using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on https://github.com/roboryantron/Unite2017
[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float _value;

    public float Value
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

    private readonly List<Action<float>> variableChangedListeners = new List<Action<float>>();

    public void RegisterListener(Action<float> listener)
    {
        if (!variableChangedListeners.Contains(listener))
            variableChangedListeners.Add(listener);
    }

    public void UnregisterListener(Action<float> listener)
    {
        if (variableChangedListeners.Contains(listener))
            variableChangedListeners.Remove(listener);
    }
}

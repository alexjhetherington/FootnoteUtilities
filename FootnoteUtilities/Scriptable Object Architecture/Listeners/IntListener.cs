﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntListener : MonoBehaviour
{
    public IntVariable variableReference;
    public IntEvent variableEvent;

    public void OnChanged(int newValue)
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

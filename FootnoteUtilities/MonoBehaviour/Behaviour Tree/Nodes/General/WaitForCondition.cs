using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForCondition : Node
{
    private Func<bool> condition;
    private Brain root;
    private float checkFrequency;

    private Coroutine waitingCoroutine;

    public WaitForCondition(Brain root, float checkFrequency, Func<bool> condition)
    {
        this.root = root;
        this.checkFrequency = checkFrequency;
        this.condition = condition;
    }

    public void Cancel()
    {
        if (waitingCoroutine != null)
            root.StopCoroutine(waitingCoroutine);
    }

    public void Run(ParentNode parent)
    {
        waitingCoroutine = root.StartCoroutine(Check_Coroutine(parent));
    }

    private IEnumerator Check_Coroutine(ParentNode parent)
    {
        while (!condition.Invoke())
        {
            yield return new WaitForSeconds(checkFrequency);
        }

        parent.HandleChildComplete();
    }
}

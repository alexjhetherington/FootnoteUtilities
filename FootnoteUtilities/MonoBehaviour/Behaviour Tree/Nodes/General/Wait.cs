using System;
using System.Collections;
using UnityEngine;

public class Wait : Node
{
    private Brain root;
    private float waitTime;

    private float lowerWaitTime;
    private float upperWaitTime;

    private Coroutine currentWaiting;

    public Wait(Brain root)
    {
        this.root = root;
        waitTime = float.MaxValue;
    }

    public Wait(Brain root, float waitTime)
    {
        this.root = root;
        this.waitTime = waitTime;
    }

    public Wait(Brain root, float lowerWaitTime, float upperWaitTime)
    {
        this.root = root;
        this.lowerWaitTime = lowerWaitTime;
        this.upperWaitTime = upperWaitTime;
    }

    public void Cancel()
    {
        if (currentWaiting != null)
            root.StopCoroutine(currentWaiting);
    }

    public void Run(ParentNode parent)
    {
        currentWaiting = root.StartCoroutine(Wait_Coroutine(parent));
    }

    private IEnumerator Wait_Coroutine(ParentNode parent)
    {
        if (upperWaitTime <= 0)
            yield return new WaitForSeconds(waitTime);
        else
            yield return new WaitForSeconds(UnityEngine.Random.Range(lowerWaitTime, upperWaitTime));
        parent.HandleChildComplete();
    }
}

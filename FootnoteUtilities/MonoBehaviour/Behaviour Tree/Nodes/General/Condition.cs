using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : Node
{
    private Func<bool> condition;

    public Condition(Func<bool> condition)
    {
        this.condition = condition;
    }

    public void Cancel()
    {
        return; //Nothing
    }

    public void Run(ParentNode parent)
    {
        if (condition.Invoke())
        {
            parent.HandleChildComplete();
        }
        else
        {
            parent.HandleChildFailed();
        }
    }
}

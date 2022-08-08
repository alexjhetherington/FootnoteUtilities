using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalRun : Node, ParentNode
{
    private Node child;
    private ParentNode parent;
    private Func<bool> condition;

    private ConditionalRun(Func<bool> condition, Node child)
    {
        this.child = child;
        this.condition = condition;
    }

    public void Cancel()
    {
        child.Cancel();
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;

        if (condition.Invoke())
        {
            child.Run(this);
        }
        else
        {
            parent.HandleChildComplete();
        }
    }

    public void HandleChildComplete()
    {
        parent.HandleChildComplete();
    }

    public void HandleChildFailed()
    {
        parent.HandleChildFailed();
    }

    public void HandleChildInterrupt(Node child)
    {
        parent.HandleChildInterrupt(this);
    }
}
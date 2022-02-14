using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node, ParentNode
{
    private Node child;
    private ParentNode parent;

    public Inverter(Node child)
    {
        this.child = child;
    }

    public void Cancel()
    {
        child.Cancel();
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;
        child.Run(this);
    }

    public void HandleChildComplete()
    {
        parent.HandleChildFailed();
    }

    public void HandleChildFailed()
    {
        parent.HandleChildComplete();
    }

    public void HandleChildInterrupt(Node child)
    {
        parent.HandleChildInterrupt(this);
    }
}

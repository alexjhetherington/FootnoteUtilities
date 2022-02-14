using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : Node, ParentNode
{
    private Node child;
    private ParentNode parent;

    private string name;

    public DebugNode(string name, Node child)
    {
        this.name = name;
        this.child = child;
    }

    public void Cancel()
    {
        child.Cancel();
    }

    public void Run(ParentNode parent)
    {
        Debug.Log(name + " run at " + Time.time);
        this.parent = parent;
        child.Run(this);
    }

    public void HandleChildComplete()
    {
        Debug.Log(name + " completed at " + Time.time);
        parent.HandleChildComplete();
    }

    public void HandleChildFailed()
    {
        Debug.Log(name + " failed at " + Time.time);
        parent.HandleChildFailed();
    }

    public void HandleChildInterrupt(Node child)
    {
        parent.HandleChildInterrupt(this);
    }
}

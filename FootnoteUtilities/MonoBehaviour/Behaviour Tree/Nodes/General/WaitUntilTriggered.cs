using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUntilTriggered : Node
{
    private ParentNode parent;

    public void Cancel()
    {
        parent = null;
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;
    }

    public void Trigger()
    {
        if (parent != null)
            parent.HandleChildComplete();

        parent = null;
    }
}

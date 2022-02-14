using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstToFinish : Node, ParentNode
{
    private List<Node> nodes = new List<Node>();
    private ParentNode parent;

    private bool receivedStatus;

    private void Add(Node node)
    {
        nodes.Add(node);
    }

    public void Run(ParentNode parent)
    {
        receivedStatus = false;

        this.parent = parent;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (receivedStatus) //possible for nodes that return instantly
            {
                break;
            }

            Run(i);
        }
    }

    private void Run(int index)
    {
        nodes[index].Run(this);
    }

    public void Cancel()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Cancel();
        }
    }

    public void HandleChildComplete()
    {
        receivedStatus = true;
        Cancel();
        parent.HandleChildComplete();
    }

    public void HandleChildFailed()
    {
        receivedStatus = true;
        Cancel();
        parent.HandleChildFailed();
    }

    public void HandleChildInterrupt(Node child)
    {
        Debug.LogError(
            "FirstToFinish node does not support interruption. Failed children are cancelled, so should not be possible. Unless someone added an option for FirstToFinish to only cancel+report to parent when receiving a complete signal"
        );
        throw new NotImplementedException();
    }

    public FirstToFinishBuilder Builder()
    {
        return new FirstToFinishBuilder(this);
    }

    public class FirstToFinishBuilder
    {
        private FirstToFinish composite;
        public FirstToFinishBuilder(FirstToFinish composite)
        {
            this.composite = composite;
        }

        public FirstToFinish Build()
        {
            return composite;
        }

        public FirstToFinishBuilder Add(Node node)
        {
            composite.Add(node);
            return this;
        }
    }
}

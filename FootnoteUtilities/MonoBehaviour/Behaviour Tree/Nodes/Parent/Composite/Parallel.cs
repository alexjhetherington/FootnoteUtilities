using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Node, ParentNode
{
    private List<Node> nodes;

    private int recieved = 0;
    private ParentNode parent;

    private void Add(Node node)
    {
        nodes.Add(node);
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Run(this);
        }
    }

    public void Cancel()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Cancel();
        }
        recieved = 0;
    }

    public void HandleChildComplete()
    {
        recieved++;
        if (recieved >= nodes.Count)
        {
            recieved = 0;
            parent.HandleChildComplete();
        }
    }

    public void HandleChildFailed()
    {
        Cancel();
        parent.HandleChildFailed();
    }

    public void HandleChildInterrupt(Node child)
    {
        Debug.LogError(
            "Parallel node does not support interruption. Failed children are cancelled, so should not be possible. Unless someone added an option for Parallel to only cancel+report to parent when receiving a complete signal"
        );
        throw new NotImplementedException();
    }

    public ParallelBuilder Builder()
    {
        return new ParallelBuilder(this);
    }

    public class ParallelBuilder
    {
        private Parallel composite;
        public ParallelBuilder(Parallel composite)
        {
            this.composite = composite;
        }

        public Parallel Build()
        {
            return composite;
        }

        public ParallelBuilder Add(Node node)
        {
            composite.Add(node);
            return this;
        }
    }
}

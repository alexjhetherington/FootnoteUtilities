using System;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node, ParentNode
{
    private List<Node> nodes = new List<Node>();
    private int index = 0;
    private ParentNode parent;

    private void Add(Node node)
    {
        nodes.Add(node);
    }

    public void Run(ParentNode parent)
    {
        index = 0;
        this.parent = parent;
        Run(index);
    }

    private void Run(int index)
    {
        nodes[index].Run(this);
    }

    //Optimisation - only cancel up to the current node?
    public void Cancel()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Cancel();
        }
    }

    public void HandleChildComplete()
    {
        index++;
        if (index >= nodes.Count)
        {
            Cancel();
            parent.HandleChildComplete();
        }
        else
        {
            Run(index);
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
            "Sequence node does not support interruption. Failed children are cancelled, so should not be possible"
        );
        throw new NotImplementedException();
    }

    public SequenceBuilder Builder()
    {
        return new SequenceBuilder(this);
    }

    public class SequenceBuilder
    {
        private Sequence composite;
        public SequenceBuilder(Sequence composite)
        {
            this.composite = composite;
        }

        public Sequence Build()
        {
            return composite;
        }

        public SequenceBuilder Add(Node node)
        {
            composite.Add(node);
            return this;
        }
    }
}

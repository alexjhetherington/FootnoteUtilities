using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node, ParentNode
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

    //Optimisation - only cancel up to current node?
    public void Cancel()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Cancel();
        }
    }

    private void Cancel(int firstIndex)
    {
        for (int i = firstIndex; i < nodes.Count; i++)
        {
            nodes[i].Cancel();
        }
    }

    public void HandleChildComplete()
    {
        Cancel();
        parent.HandleChildComplete();
    }

    public void HandleChildFailed()
    {
        index++;
        if (index >= nodes.Count)
        {
            Cancel();
            parent.HandleChildFailed();
        }
        else
        {
            Run(index);
        }
    }

    //If a child node interrupts, cancel everything after that child, then run from that child
    public void HandleChildInterrupt(Node child)
    {
        index = nodes.IndexOf(child);
        Cancel(index + 1);
        Run(index);
    }

    public SelectorBuilder Builder()
    {
        return new SelectorBuilder(this);
    }

    public class SelectorBuilder
    {
        private Selector composite;
        public SelectorBuilder(Selector composite)
        {
            this.composite = composite;
        }

        public Selector Build()
        {
            return composite;
        }

        public SelectorBuilder Add(Node node)
        {
            composite.Add(node);
            return this;
        }
    }
}

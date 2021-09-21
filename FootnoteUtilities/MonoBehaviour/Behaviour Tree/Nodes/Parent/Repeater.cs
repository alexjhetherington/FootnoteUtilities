using System;
using System.Collections;
using UnityEngine;

public class Repeater : Node, ParentNode
{
    private Node node;
    private Brain brain;
    
    private Coroutine deferredRun;

    public Repeater(Brain brain, Node node)
    {
        this.node = node;
        this.brain = brain;
    }

    public void Cancel()
    {
        if (deferredRun != null)
            brain.StopCoroutine(deferredRun);

        node.Cancel();
    }

    public void Run(ParentNode parent)
    {
        node.Run(this);
    }

    private IEnumerator DeferredRepeat()
    {
        Cancel();
        yield return new WaitForEndOfFrame();
        Run(this);
    }

    public void HandleChildComplete()
    {
        brain.StartCoroutine(DeferredRepeat());
    }

    public void HandleChildFailed()
    {
        brain.StartCoroutine(DeferredRepeat());
    }
    
    public void HandleChildInterrupt(Node child)
    {
        Debug.LogError("Repeater node does not support interruption. Failed children are cancelled, so should not be possible");
        throw new NotImplementedException();
    }
}

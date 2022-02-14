using System;
using System.Collections;
using UnityEngine;

public class Repeater : Node, ParentNode
{
    private Node node;
    private Brain brain;
    private ParentNode parent;

    private Coroutine deferredRun;

    private bool untilSuccess = false;

    private Repeater(Brain brain, Node node, bool untilSuccess)
    {
        this.node = node;
        this.brain = brain;
        this.untilSuccess = untilSuccess;
    }

    public Repeater(Brain brain, Node node)
    {
        this.node = node;
        this.brain = brain;
    }

    public static Repeater UntilSuccess(Brain brain, Node node)
    {
        return new Repeater(brain, node, true);
    }

    public void Cancel()
    {
        if (deferredRun != null)
            brain.StopCoroutine(deferredRun);

        node.Cancel();
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;
        InternalRun();
    }

    private void InternalRun()
    {
        node.Run(this);
    }

    private IEnumerator DeferredRepeat()
    {
        Cancel();
        yield return null;
        InternalRun();
    }

    public void HandleChildComplete()
    {
        if (untilSuccess)
            parent.HandleChildComplete();
        else
            brain.StartCoroutine(DeferredRepeat());
    }

    public void HandleChildFailed()
    {
        brain.StartCoroutine(DeferredRepeat());
    }

    public void HandleChildInterrupt(Node child)
    {
        Debug.LogError(
            "Repeater node does not support interruption. Failed children are cancelled, so should not be possible"
        );
        throw new NotImplementedException();
    }
}

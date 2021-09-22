using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalRun : Node, ParentNode
{
    private Brain brain;

    private Node child;
    private ParentNode parent;

    private Func<bool> condition;
    private bool reevaluate;

    private Coroutine reevaluationCoroutine;

    private ConditionalRun(Brain brain, Func<bool> condition, Node child, bool reevaluate)
    {
        this.brain = brain;
        this.child = child;
        this.condition = condition;
        this.reevaluate = reevaluate;
    }

    public ConditionalRun(Func<bool> condition, Node child) : this(null, condition, child, false) { }
    public static ConditionalRun WithReevaluate(Brain brain, Func<bool> condition, Node child) { return new ConditionalRun(brain, condition, child, true); }

    public void Cancel()
    {
        if (reevaluationCoroutine != null)
            brain.StopCoroutine(reevaluationCoroutine);
                
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
            parent.HandleChildFailed();

            if(reevaluate && brain != null)
                brain.StartCoroutine(Reevaluate_Coroutine(parent));
        }
    }

    private IEnumerator Reevaluate_Coroutine(ParentNode parent)
    {
        while (true)
        {
            if (condition.Invoke())
                break;

            yield return null;
        }
        
        parent.HandleChildInterrupt(this);
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
        Cancel();
        parent.HandleChildInterrupt(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileCondition : Node, ParentNode
{
    private Brain brain;

    private Node child;
    private ParentNode parent;

    private Func<bool> condition;
    private bool reevaluate;

    private Coroutine checkBecameTrue;
    private Coroutine checkBecameFalse;

    private WhileCondition(Brain brain, Func<bool> condition, Node child, bool reevaluate)
    {
        this.brain = brain;
        this.child = child;
        this.condition = condition;
        this.reevaluate = reevaluate;
    }

    public WhileCondition(Brain brain, Func<bool> condition, Node child)
        : this(null, condition, child, false) { }
    public static WhileCondition WithPriority(Brain brain, Func<bool> condition, Node child)
    {
        return new WhileCondition(brain, condition, child, true);
    }

    public void Cancel()
    {
        ResetCoroutines();
        child.Cancel();
    }

    public void ResetCoroutines()
    {
        if (checkBecameTrue != null)
            brain.StopCoroutine(checkBecameTrue);

        if (checkBecameFalse != null)
            brain.StopCoroutine(checkBecameFalse);
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;

        if (condition.Invoke())
        {
            checkBecameFalse = brain.StartCoroutine(CheckBecameFalse_Coroutine(parent));
            child.Run(this);
        }
        else
        {
            HandleChildFailed();
        }
    }

    private IEnumerator CheckBecameTrue_Coroutine(ParentNode parent)
    {
        while (true)
        {
            if (condition.Invoke())
                break;

            yield return new WaitForSeconds(0.1f);
        }

        parent.HandleChildInterrupt(this);
    }

    private IEnumerator CheckBecameFalse_Coroutine(ParentNode parent)
    {
        while (true)
        {
            if (!condition.Invoke())
                break;

            yield return new WaitForSeconds(0.1f);
        }

        child.Cancel();
        HandleChildFailed();
    }

    public void HandleChildComplete()
    {
        ResetCoroutines();
        parent.HandleChildComplete();
    }

    public void HandleChildFailed()
    {
        ResetCoroutines();
        parent.HandleChildFailed();

        if (reevaluate && brain != null)
        {
            checkBecameTrue = brain.StartCoroutine(CheckBecameTrue_Coroutine(parent));
        }
    }

    public void HandleChildInterrupt(Node child)
    {
        ResetCoroutines();
        parent.HandleChildInterrupt(this);
    }
}

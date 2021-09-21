using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootAt : MonoBehaviour, Node
{
    protected Brain brain;
    protected string targetKey;

    public void Init(Brain brain, string targetKey)
    {
        this.brain = brain;
        this.targetKey = targetKey;
    }

    public abstract void Cancel();
    public abstract void Run(ParentNode parent);
}

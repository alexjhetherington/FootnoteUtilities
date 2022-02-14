using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : MonoBehaviour, ParentNode
{
    public Blackboard Blackboard = new Blackboard();

    protected Node root;

    public abstract Node GetTree();

    public virtual void Awake()
    {
        root = GetTree();
    }

    public virtual void OnDisable()
    {
        if (root != null)
            root.Cancel();
    }

    public virtual void OnEnable()
    {
        if (root != null)
            root.Run(this);
    }

    public void Kill()
    {
        if (root != null)
            root.Cancel();
    }

    protected Action DrawLine(string key)
    {
        return () =>
        {
            Vector3 destination;

            if (Blackboard.TryGetTypedValue(key, out destination))
            {
                Debug.DrawLine(transform.position, destination, Color.blue, 5);
            }
            else
            {
                Debug.LogWarning("Debug draw line failed due to missing key: " + key);
            }
        };
    }

    protected bool IsCloseToTransform(Transform player, float distance)
    {
        bool condition = Vector3.Distance(player.position, transform.position) < distance;
        return condition;
    }

    public void HandleChildComplete()
    {
        Debug.LogWarning(
            "Behaviour finished with Complete signal. Are you sure you wanted the behaviour to finish?",
            this
        );
    }

    public void HandleChildFailed()
    {
        Debug.LogWarning(
            "Behaviour finished with Failed signal. Are you sure you wanted the behaviour to finish?",
            this
        );
    }

    public void HandleChildInterrupt(Node child)
    {
        Debug.LogWarning(
            "Behaviour finished with Interrupt signal. Are you sure you wanted the behaviour to finish?",
            this
        );
    }
}

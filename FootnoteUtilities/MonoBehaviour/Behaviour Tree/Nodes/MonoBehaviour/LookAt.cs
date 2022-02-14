using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour, Node
{
    [SerializeField]
    private float speed;

    private Brain brain;
    private string targetKey;
    private bool continuous;

    private Component targetComponent;
    private Vector3? targetVector;
    private ParentNode parent;

    public LookAt Init(Brain brain, string targetKey, bool continuous = false)
    {
        this.brain = brain;
        this.targetKey = targetKey;
        this.continuous = continuous;
        return this;
    }

    public LookAt Init(Brain brain, string targetKey, float speed, bool continuous = false)
    {
        this.brain = brain;
        this.targetKey = targetKey;
        this.continuous = continuous;
        this.speed = speed;
        return this;
    }

    public void LateUpdate()
    {
        if (targetVector == null && targetComponent == null)
            return;

        Vector3 lookPos =
            (targetComponent != null ? targetComponent.transform.position : targetVector.Value)
            - transform.position;
        lookPos.y = 0;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(lookPos),
            Time.deltaTime * speed
        );

        if (
            !continuous
            && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(lookPos)) < 0.5
        )
        {
            parent.HandleChildComplete();
            targetVector = null;
            targetComponent = null;
        }
    }

    public void Run(ParentNode parent)
    {
        this.parent = parent;

        if (brain.Blackboard.TryGetTypedValue(targetKey, out targetComponent))
        {
            return;
        }
        else if (brain.Blackboard.TryGetTypedValue(targetKey, out targetVector))
        {
            return;
        }

        Debug.Log("LookAt could not find Component or Vector3 in blackboard with key:" + targetKey);
        parent.HandleChildFailed();
    }

    public void Cancel()
    {
        targetComponent = null;
        targetVector = null;
    }
}

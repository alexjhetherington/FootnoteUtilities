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

    private Transform target;
    private ParentNode parent;
    
    public LookAt Init(Brain brain, string targetKey, bool continuous = false)
    {
        this.brain = brain;
        this.targetKey = targetKey;
        this.continuous = continuous;
        return this;
    }

    public void LateUpdate()
    {
        if (target != null)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * speed);

            if (!continuous && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(lookPos)) < 10)
            {
                parent.HandleChildComplete();
                target = null;
            }
        }
    }

    public void Run(ParentNode parent)
    {
        if (!brain.Blackboard.TryGetTypedValue(targetKey, out target))
        {
            Debug.Log("LookAt could not find Transform in blackboard with key:" + targetKey);
            parent.HandleChildFailed();
        }
        else
        {
            this.parent = parent;
        }
    }

    public void Cancel()
    {
        target = null;
    }
}

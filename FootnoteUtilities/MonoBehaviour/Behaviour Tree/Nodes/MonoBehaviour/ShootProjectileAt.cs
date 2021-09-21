using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileAt : ShootAt
{
    [SerializeField] private float horizontalSpray = default;
    [SerializeField] private float verticalSpray = default;

    [SerializeField] private GameObject projectilePrefab = default;
    public override void Cancel()
    {
        return;
    }

    public override void Run(ParentNode parent)
    {
        Transform target;
        bool foundTarget = brain.Blackboard.TryGetTypedValue(targetKey, out target);

        if (foundTarget)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction = Quaternion.Euler(0, UnityEngine.Random.Range(-horizontalSpray / 2, horizontalSpray / 2), 0) * direction;
            direction = Quaternion.Euler(UnityEngine.Random.Range(-verticalSpray / 2, verticalSpray / 2), 0, 0) * direction;
            GameObject go = Instantiate(projectilePrefab, new Vector3(transform.position.x, 1.3f, transform.position.z) + (direction * 1f), Quaternion.LookRotation(direction, Vector3.up));
            parent.HandleChildComplete();
        }
        else
        {
            parent.HandleChildFailed();
        }
    }
}

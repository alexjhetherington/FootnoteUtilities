using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHitscanAt : ShootAt
{
    [SerializeField] public float range = default;
    [SerializeField] private int damage = default;
    [SerializeField] private float horizontalSpray = default;
    [SerializeField] private float verticalSpray = default;

    public event Action<Vector3> bulletReached;

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
            Vector3 origin = transform.position + Vector3.up;

            direction = Quaternion.Euler(0, UnityEngine.Random.Range(-horizontalSpray / 2, horizontalSpray / 2), 0) * direction;
            direction = Quaternion.Euler(UnityEngine.Random.Range(-verticalSpray / 2, verticalSpray / 2), 0, 0) * direction;

            RaycastHit raycastHit;
            if (Physics.Raycast(origin, direction, out raycastHit, range))
            {
                bulletReached?.Invoke(raycastHit.point);

                Health health = raycastHit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.Value -= damage;
                }
            }
            else
            {
                bulletReached?.Invoke(origin + direction * range);
            }

            parent.HandleChildComplete();
        }
        else
        {
            parent.HandleChildFailed();
        }
    }
}
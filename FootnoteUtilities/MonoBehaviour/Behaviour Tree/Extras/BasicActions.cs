using System;
using UnityEngine;

public static class BasicActions
{
    public static Action AssignPointBehind(
        Blackboard blackboard,
        string key,
        Transform source,
        Transform target,
        float maxDistance,
        int layerMask
    )
    {
        return () =>
        {
            RaycastHit hit;
            Vector3 dir = target.position - source.position;
            dir.y = 0;
            Physics.Raycast(target.position, dir, out hit);

            Vector3 newTarget;
            if (hit.collider == null || hit.distance > maxDistance)
                newTarget = source.position + (dir.normalized * maxDistance);
            else
                newTarget = hit.point;

            blackboard[key] = Footnote3D.GetPointBehindTargetWithRaycast(
                source.position,
                target.position,
                maxDistance,
                layerMask
            );
        };
    }

    internal static Action AssignTargetDiagonalTowards(
        Blackboard blackboard,
        string key,
        Transform source,
        Transform transform,
        float angle,
        float minDistance,
        float maxDistance
    )
    {
        return () =>
        {
            float rangle = UnityEngine.Random.value >= 0.5 ? angle : -angle;
            Vector3 towards = transform.position - source.position;
            Vector3 angled = Quaternion.AngleAxis(rangle, Vector3.up) * towards;
            Vector3 distanced =
                angled.normalized * UnityEngine.Random.Range(minDistance, maxDistance);
            blackboard[key] = source.position + distanced;
        };
    }

    public static Action AssignRandomCloseToTransform(
        Blackboard blackboard,
        string key,
        Transform transform
    )
    {
        return () =>
        {
            Vector2 circle = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(5, 15);
            Vector3 newTarget = new Vector3(circle.x, 0, circle.y) + transform.position;
            blackboard[key] = newTarget;
        };
    }

    public static Action AssignRandomCloseToTransformWithinLos(
        Blackboard blackboard,
        string key,
        Transform transform,
        int layerMask
    )
    {
        return () =>
        {
            Vector2 circle;
            Vector3 newTarget;

            do
            {
                circle = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(12, 17);
                newTarget =
                    new Vector3(circle.x, transform.position.y, circle.y) + transform.position;
            } while (
                Physics.Linecast(newTarget + Vector3.up, transform.position + Vector3.up, layerMask)
            );

            blackboard[key] = newTarget;
        };
    }
}

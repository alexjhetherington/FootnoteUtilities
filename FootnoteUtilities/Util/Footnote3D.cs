using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Footnote3D
{
    //Get closest point on line a->b to arbitrary point p, where the line is infinite
    public static Vector3 GetPoint(Vector3 p, Vector3 a, Vector3 b)
    {
        return a + Vector3.Project(p - a, b - a);
    }

    public static int indexOfClosestPoint(Vector3[] points, Vector3 point)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(point, points[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // Calculate distance between a point and a line.
    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }

    // Project /point/ onto a line.
    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 relativePoint = point - lineStart;
        Vector3 lineDirection = lineEnd - lineStart;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > .000001f)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        dot = Mathf.Clamp(dot, 0.0F, length);

        return lineStart + normalizedLineDirection * dot;
    }

    public static RaycastHit GetClosestHitInSphere(
        Vector3 origin,
        float maxDistance,
        float inverseResolution
    )
    {
        Ray ray = new Ray();
        ray.origin = origin;
        Vector3 direction = Vector3.right;
        int steps = Mathf.FloorToInt(360f / inverseResolution);
        Quaternion xRotation = Quaternion.Euler(Vector3.right * inverseResolution);
        Quaternion yRotation = Quaternion.Euler(Vector3.up * inverseResolution);
        Quaternion zRotation = Quaternion.Euler(Vector3.forward * inverseResolution);

        RaycastHit closest = new RaycastHit();

        for (int x = 0; x < steps / 2; x++)
        {
            direction = zRotation * direction;
            for (int y = 0; y < steps; y++)
            {
                direction = xRotation * direction;
                ray.direction = direction;
                //Debug.DrawLine(ray.origin, ray.origin + direction, Color.red, 10); // for science
                RaycastHit candidate;
                Physics.Raycast(ray, out candidate, maxDistance);
                if (
                    candidate.collider != null
                    && (closest.collider == null || closest.distance > candidate.distance)
                )
                {
                    closest = candidate;
                }
            }
        }

        return closest;
    }

    public static Vector3 GetPointBehindTargetWithRaycast(
        Vector3 source,
        Vector3 target,
        float maxDistance,
        int layerMask
    )
    {
        RaycastHit hit;
        Vector3 dir = target - source;
        dir.y = 0;
        Physics.Raycast(source, dir, out hit, maxDistance, layerMask);

        if (hit.collider == null || hit.distance > maxDistance)
            return source + (dir.normalized * maxDistance);
        else
            return hit.point;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteColliderExtensions
{
    public static Vector3 GetRandomPointInsideCollider(this BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point =
            new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z)
            ) + boxCollider.center;
        return boxCollider.transform.TransformPoint(point);
    }
}

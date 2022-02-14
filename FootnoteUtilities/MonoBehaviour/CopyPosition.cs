using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}

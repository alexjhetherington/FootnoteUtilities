using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private Vector3[] waypoints = null;

    public Vector3[] Get()
    {
        if (waypoints == null)
        {
            waypoints = new Vector3[transform.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = transform.GetChild(i).position;
            }
        }

        return waypoints;
    }

    void OnDrawGizmos()
    {
        if (transform.childCount == 0)
            return;

        Vector3 startPosition = transform.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        Gizmos.color = Color.blue;
        foreach (Transform waypoint in transform)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourTreeHelpers
{
    public static void AddWaypointsToBlackboard(
        Brain brain,
        Vector3[] waypoints,
        string vector3QueueKey
    )
    {
        List<Vector3> queue = new List<Vector3>(waypoints.Length);

        foreach (Vector3 waypoint in waypoints)
            queue.Add(waypoint);

        brain.Blackboard[vector3QueueKey] = queue;
    }
}

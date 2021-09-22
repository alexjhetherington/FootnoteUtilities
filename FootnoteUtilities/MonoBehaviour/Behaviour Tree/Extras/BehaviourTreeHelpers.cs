using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourTreeHelpers
{
    public static void AddWaypointsToBlackboard(Brain brain, Vector3[] waypoints, string vector3QueueKey)
    {
        Queue<Vector3> queue = new Queue<Vector3>(waypoints.Length);

        foreach (Vector3 waypoint in waypoints)
            queue.Enqueue(waypoint);

        brain.Blackboard[vector3QueueKey] = queue;
    }
}

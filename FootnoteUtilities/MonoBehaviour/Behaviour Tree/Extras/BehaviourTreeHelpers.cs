using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourTreeHelpers
{
    public static void AddWaypointsToBlackboard(Brain brain, BehaviourTreeWaypoint[] waypoints, string vector3QueueKey)
    {
        Queue<Vector3> queue = new Queue<Vector3>(waypoints.Length);

        foreach (BehaviourTreeWaypoint waypoint in waypoints)
            queue.Enqueue(waypoint.transform.position);

        brain.Blackboard[vector3QueueKey] = queue;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class SubTreeLibrary
{
    public static Node FireBurst(Brain brain, ShootAt shootAt, int burstAmount, float waitBetween)
    {
        Sequence root = new Sequence();
        for(int i = 0; i < burstAmount; i++)
        {
            root.Add(shootAt);
            root.Add(new Wait(brain, waitBetween));
        }
        return root;
    }

    public static Node Patrol(Brain brain, string vector3QueueKey, string vector3SelectedKey)
    {
        Repeater root = new Repeater(brain, new Sequence().Builder()
                                .Add(new RunAction(() =>
                                {
                                    Queue<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                                    Vector3 next = waypoints.Dequeue();
                                    waypoints.Enqueue(next);
                                    brain.Blackboard[vector3SelectedKey] = next;
                                }))
                                .Add(new NavTo(brain, vector3SelectedKey, brain.GetComponent<NavMeshAgent>()))
                            .Build());

        return root;
    }
}

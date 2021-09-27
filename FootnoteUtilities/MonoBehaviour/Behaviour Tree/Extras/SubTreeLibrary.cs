using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class SubTreeLibrary
{
    public static Node FireBurst(Brain brain, ShootAt shootAt, int burstAmount, float waitBetween)
    {
        var root = new Sequence().Builder();
        for(int i = 0; i < burstAmount; i++)
        {
            root.Add(shootAt);
            root.Add(new Wait(brain, waitBetween));
        }
        return root.Build();
    }

    public static Node Patrol(Brain brain, string vector3QueueKey, string vector3SelectedKey)
    {
        Repeater root = new Repeater(brain, new Sequence().Builder()
                                .Add(new RunAction(() =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                                    Vector3 next = waypoints[0];
                                    waypoints.RemoveAt(0);


                                    waypoints.Add(next);
                                    brain.Blackboard[vector3SelectedKey] = next;
                                }))
                                .Add(new NavTo(brain, vector3SelectedKey, brain.GetComponent<NavMeshAgent>()))
                            .Build());

        return root;
    }

    public static Node Waypoints(Brain brain, string vector3QueueKey, string vector3SelectedKey)
    {
        Repeater root = Repeater.UntilSuccess(brain, new Sequence().Builder()
                                .Add(new RunAction(() =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                                    Vector3 next = waypoints[0];
                                    brain.Blackboard[vector3SelectedKey] = next;
                                }))
                                .Add(new FirstToFinish().Builder()
                                    .Add(new NavTo(brain, vector3SelectedKey, brain.GetComponent<NavMeshAgent>()))
                                    .Add(new WaitForCondition(brain, 0.1f, () =>
                                    {
                                        Vector3 dest = brain.Blackboard.Get<Vector3>(vector3SelectedKey);
                                        return Vector3.Distance(dest, brain.transform.position) < 1.5f;
                                    }))
                                    .Build())
                                .Add(new RunAction(() =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);
                                    waypoints.RemoveAt(0);
                                }))
                                .Add(new Condition(() =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                                    if (waypoints.Count == 0)
                                        Debug.Log("Got to end of waypoints");

                                    return waypoints.Count == 0;
                                }))
                            .Build());

        return root;
    }
}

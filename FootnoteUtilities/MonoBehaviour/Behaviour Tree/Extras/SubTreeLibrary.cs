using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class SubTreeLibrary
{
    public static Node FireBurst(
        Brain brain,
        string targetKey,
        int burstAmount,
        float waitBetween,
        Shoot shoot
    )
    {
        var root = new Sequence().Builder();

        root.Add(new Condition(() => brain.Blackboard.ContainsKey(targetKey)));

        for (int i = 0; i < burstAmount; i++)
        {
            root.Add(
                new RunAction(
                    () => shoot.Shoot(brain.Blackboard.Get<Transform>(targetKey).position)
                )
            );
            root.Add(new Wait(brain, waitBetween));
        }
        return root.Build();
    }

    public static Node Patrol(Brain brain, string vector3QueueKey, string vector3SelectedKey)
    {
        Repeater root = new Repeater(
            brain,
            new Sequence()
                .Builder()
                .Add(
                    new RunAction(
                        () =>
                        {
                            List<Vector3> waypoints;
                            brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                            Vector3 next = waypoints[0];
                            waypoints.RemoveAt(0);

                            waypoints.Add(next);
                            brain.Blackboard[vector3SelectedKey] = next;
                        }
                    )
                )
                .Add(new NavTo(brain, vector3SelectedKey, brain.GetComponent<NavMeshAgent>()))
                .Build()
        );

        return root;
    }

    public static Node Waypoints(Brain brain, string vector3QueueKey, string vector3SelectedKey)
    {
        Node root = new Sequence()
            .Builder()
            //Early fail if no waypoints
            .Add(new Inverter(IsOutOfWayPoints(brain, vector3QueueKey)))
            .Add(
                Repeater.UntilSuccess(
                    brain,
                    new Sequence()
                        .Builder()
                        .Add(
                            new RunAction(
                                () =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(
                                        vector3QueueKey,
                                        out waypoints
                                    );

                                    Vector3 next = waypoints[0];
                                    brain.Blackboard[vector3SelectedKey] = next;
                                }
                            )
                        )
                        .Add(
                            new FirstToFinish()
                                .Builder()
                                .Add(
                                    new NavTo(
                                        brain,
                                        vector3SelectedKey,
                                        brain.GetComponent<NavMeshAgent>()
                                    )
                                )
                                .Add(
                                    new WaitForCondition(
                                        brain,
                                        0.1f,
                                        () =>
                                        {
                                            Vector3 dest;
                                            if (
                                                brain.Blackboard.TryGetTypedValue(
                                                    vector3SelectedKey,
                                                    out dest
                                                )
                                            )
                                                return Vector3.Distance(
                                                        dest,
                                                        brain.transform.position
                                                    ) < 1.5f;
                                            else
                                                return false;
                                        }
                                    )
                                )
                                .Build()
                        )
                        .Add(
                            new RunAction(
                                () =>
                                {
                                    List<Vector3> waypoints;
                                    brain.Blackboard.TryGetTypedValue(
                                        vector3QueueKey,
                                        out waypoints
                                    );
                                    waypoints.RemoveAt(0);
                                }
                            )
                        )
                        .Add(IsOutOfWayPoints(brain, vector3QueueKey))
                        .Build()
                )
            )
            .Build();

        return root;
    }

    private static Node IsOutOfWayPoints(Brain brain, string vector3QueueKey)
    {
        return new Condition(
            () =>
            {
                List<Vector3> waypoints;
                brain.Blackboard.TryGetTypedValue(vector3QueueKey, out waypoints);

                return waypoints.Count == 0;
            }
        );
    }
}

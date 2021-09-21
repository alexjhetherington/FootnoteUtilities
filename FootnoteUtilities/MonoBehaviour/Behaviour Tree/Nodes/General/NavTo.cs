using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTo : Node
{
    private NavMeshAgent navMeshAgent;
    private string destinationKey;
    private Brain root;

    private bool debug;

    //Can be null if blackboard variable is a vector3
    //Vector3 is preferrable if possible, because it means nav mesh agent pathing does not update every frame
    private Transform targetTransform;

    private Coroutine currentNavTo;

    public NavTo(Brain root, string destinationKey, NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;
        this.root = root;
        this.destinationKey = destinationKey;
    }

    public NavTo(Brain root, string destinationKey, NavMeshAgent navMeshAgent, bool debug)
    {
        this.navMeshAgent = navMeshAgent;
        this.root = root;
        this.destinationKey = destinationKey;
        this.debug = debug;
    }

    public void Cancel()
    {
        if (debug)
            Debug.Log("Cancelled: " + Environment.StackTrace);

        if(navMeshAgent.isActiveAndEnabled)
            navMeshAgent.isStopped = true;

        if (currentNavTo != null)
            root.StopCoroutine(currentNavTo);
    }

    public void Run(ParentNode parent)
    {
        Vector3 location;
        if (root.Blackboard.TryGetTypedValue(destinationKey, out location))
        {
            NavMeshHit hit;
            bool foundPosition = NavMesh.SamplePosition(location, out hit, 5, NavMesh.AllAreas);

            if (foundPosition)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(hit.position);
                currentNavTo = root.StartCoroutine(WaitForDestination_Coroutine(parent));
            }
            else
            {
                Debug.LogWarning("NavTo tried to locate to nav to an area that was not near the navmesh: " + destinationKey + ", vector3: " + location);
                parent.HandleChildFailed();
            }
        }
        else if (root.Blackboard.TryGetTypedValue(destinationKey, out targetTransform))
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetTransform.position);

            currentNavTo = root.StartCoroutine(WaitForDestination_Coroutine(parent));
        }
        else
        {
            Debug.Log("NavTo could not find Vector3 or Transform in blackboard with key:" + destinationKey);
            parent.HandleChildFailed();
        }
    }

    private IEnumerator WaitForDestination_Coroutine(ParentNode parent)
    {
        while (!navMeshAgent.HasArrived())
        {
            if (targetTransform != null)
                navMeshAgent.SetDestination(targetTransform.position);

            yield return new WaitForEndOfFrame();
        }

        parent.HandleChildComplete();
    }
}

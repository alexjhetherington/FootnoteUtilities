using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonoBehaviourRuntimeSet : RuntimeSet<MonoBehaviour>
{
    //TODO bad allocation
    public IEnumerable<T> GetAs<T>() where T : MonoBehaviour
    {
        foreach (MonoBehaviour monoBehaviour in Items)
        {
            if (monoBehaviour is T)
                yield return (T)monoBehaviour;
            else
                yield return monoBehaviour.GetComponent<T>();
        }
    }
}

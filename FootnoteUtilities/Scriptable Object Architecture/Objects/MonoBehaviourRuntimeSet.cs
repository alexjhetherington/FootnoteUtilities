using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonoBehaviourRuntimeSet : RuntimeSet<MonoBehaviour>
{
    //TODO bad allocation?
    public IEnumerable<T> GetAs<T>() where T : MonoBehaviour
    {
        foreach (MonoBehaviour monoBehaviour in Items)
        {
            if (monoBehaviour is T)
            {
                yield return (T)monoBehaviour;
            }
            else
            {
                T component = monoBehaviour.GetComponentInChildren<T>();
                if (component != null)
                {
                    yield return component;
                }
                else
                {
                    Debug.LogError(
                        "Looks like you incorrectly set group ["
                            + this.name
                            + "] on object ["
                            + monoBehaviour.name
                            + "]"
                    );
                    continue;
                }
            }
        }
    }

    public T GetFirstActiveAs<T>() where T : MonoBehaviour
    {
        foreach (MonoBehaviour monoBehaviour in Items)
        {
            if (monoBehaviour.gameObject.activeInHierarchy && monoBehaviour.isActiveAndEnabled)
            {
                if (monoBehaviour is T)
                    return (T)monoBehaviour;

                T component = monoBehaviour.GetComponentInChildren<T>();
                if (component != null)
                {
                    return component;
                }
                Debug.LogError(
                    "Looks like you incorrectly set group ["
                        + this.name
                        + "] on object ["
                        + monoBehaviour.name
                        + "]"
                );
                continue;
            }
        }
        throw new Exception(
            "No live gameobjects of type [" + typeof(T) + "] in runtime set [" + name + "]"
        );
    }
}

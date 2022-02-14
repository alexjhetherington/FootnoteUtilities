using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfMonoBehaviourRuntimeSet : MonoBehaviour
{
    [SerializeField]
    private bool excludeWhenDisabled;
    [SerializeField]
    private MonoBehaviourRuntimeSet set;

    public virtual void OnEnable()
    {
        if (excludeWhenDisabled)
            set.Add(this);
    }

    public virtual void OnDisable()
    {
        if (excludeWhenDisabled)
            set.Remove(this);
    }

    public virtual void Awake()
    {
        if (!excludeWhenDisabled)
            set.Add(this);
    }

    public virtual void OnDestroy()
    {
        if (!excludeWhenDisabled)
            set.Remove(this);
    }
}

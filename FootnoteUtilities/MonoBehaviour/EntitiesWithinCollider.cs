using System.Collections.Generic;
using UnityEngine;

public abstract class EntitiesWithinCollider<T> : MonoBehaviour where T : MonoBehaviour
{
    List<T> _entitiesWithinCollider = new List<T>();

    //TODO Bad Allocation
    public IEnumerable<T> Get()
    {
        for (int i = 0; i < _entitiesWithinCollider.Count; i++)
        {
            if (_entitiesWithinCollider[i] != null)
            {
                yield return _entitiesWithinCollider[i];
            }
            else
            {
                _entitiesWithinCollider.RemoveAt(i);
                i--;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        T component = other.GetComponent<T>();
        if (component != null)
            _entitiesWithinCollider.Add(component);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        T component = other.GetComponent<T>();
        if (component != null)
            _entitiesWithinCollider.Remove(component);
    }
}

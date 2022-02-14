using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : Dictionary<string, object>
{
    public T Get<T>(string key)
    {
        return (T)this[key];
    }

    public bool TryGetTypedValue<T>(string key, out T val)
    {
        object temp;
        bool success = TryGetValue(key, out temp);

        if (!success || !(temp is T))
        {
            val = default;
            return false;
        }

        val = (T)temp;
        return success;
    }
}

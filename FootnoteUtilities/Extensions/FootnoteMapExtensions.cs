using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteMapExtensions
{
    public static List<T> SafeGet<K, T>(this Dictionary<K, List<T>> map, K key)
    {
        if (!map.ContainsKey(key))
        {
            Type listType = typeof(List<T>);
            List<T> created = (List<T>)Activator.CreateInstance(listType);

            map[key] = created;
        }

        return map[key];
    }
}

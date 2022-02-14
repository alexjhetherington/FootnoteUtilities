using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Add(T thing)
    {
        if (!Items.Contains(thing))
        {
            Items.Add(thing);
            for (int i = itemAddedListeners.Count - 1; i >= 0; i--)
                itemAddedListeners[i].Invoke(thing);
        }
    }

    public void Remove(T thing)
    {
        if (Items.Contains(thing))
        {
            Items.Remove(thing);
            for (int i = itemRemovedListeners.Count - 1; i >= 0; i--)
                itemRemovedListeners[i].Invoke(thing);
        }
    }

    private readonly List<Action<T>> itemAddedListeners = new List<Action<T>>();

    public void RegisterItemAddedListener(Action<T> listener)
    {
        if (!itemAddedListeners.Contains(listener))
            itemAddedListeners.Add(listener);
    }

    public void UnregisterItemAddedListener(Action<T> listener)
    {
        if (itemAddedListeners.Contains(listener))
            itemAddedListeners.Remove(listener);
    }

    private readonly List<Action<T>> itemRemovedListeners = new List<Action<T>>();

    public void RegisterItemRemovedListener(Action<T> listener)
    {
        if (!itemRemovedListeners.Contains(listener))
            itemRemovedListeners.Add(listener);
    }

    public void UnregisterItemRemovedListener(Action<T> listener)
    {
        if (itemRemovedListeners.Contains(listener))
            itemRemovedListeners.Remove(listener);
    }
}

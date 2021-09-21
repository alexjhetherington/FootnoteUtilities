using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedArray<T> {

    private struct TimedItem
    {
        public float timeLeft;
        public T item;

        public TimedItem(float timeLeft, T item)
        {
            this.timeLeft = timeLeft;
            this.item = item;
        }
    }

    private List<TimedItem> _timedArray = new List<TimedItem>();

    private int FindItem(T item)
    {
        for(int i = 0; i < _timedArray.Count; i++)
        {
            T currentItem = _timedArray[i].item;
            if (currentItem.Equals(item))
            {
                return i;
            }
        }

        return -1;
    }

    public void Insert(float time, T item)
    {
        //Debug.Log("Add: " + item);
        int index = FindItem(item);
        if(index >= 0)
        {
            _timedArray[index] = new TimedItem(time, item);
        }
        else
        {
            _timedArray.Add(new TimedItem(time, item));
        }
    }

    public void Tick(float deltaTime)
    {
        for (int i = _timedArray.Count - 1; i >= 0; i--)
        {
            TimedItem currentItem = _timedArray[i];
            float newTime = currentItem.timeLeft - deltaTime;
            if (newTime > 0)
            {
                _timedArray[i] = new TimedItem(newTime, currentItem.item);
            }
            else
            {
                //Debug.Log("Remove: " + _timedArray[i].item);
                _timedArray.RemoveAt(i);
            }
            
        }
    }

    public bool Exists(T item)
    {
        int index = FindItem(item);
        if (index >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

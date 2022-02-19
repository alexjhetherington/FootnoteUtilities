using System;
using System.Collections.Generic;

public class TimedArray<T>
{
    public struct TimedItem
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

    public Tuple<TimedItem, TimedItem> AfterAndBefore(float timeLeft)
    {
        for (int i = 0; i < _timedArray.Count; i++)
        {
            TimedItem currentItem = _timedArray[i];
            if (currentItem.timeLeft > timeLeft)
            {
                if (i < 0)
                {
                    return Tuple.Create(currentItem, _timedArray[i - 1]);
                }
                return Tuple.Create(currentItem, currentItem);
            }
        }

        return Tuple.Create(_timedArray[_timedArray.Count - 1], _timedArray[_timedArray.Count - 1]);
    }

    private int FindFirst(T item)
    {
        for (int i = 0; i < _timedArray.Count; i++)
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
        _timedArray.Add(new TimedItem(time, item));
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
        int index = FindFirst(item);
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

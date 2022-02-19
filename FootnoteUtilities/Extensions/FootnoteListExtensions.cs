using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteListExtensions
{
    public static bool SortByDistanceIfUnordered<T>(this List<T> components, Vector3 point)
        where T : Component
    {
        return components.SortIfUnordered(
            (item1, item2) =>
            {
                float dist1 = Vector3.Distance(point, item1.transform.position);
                float dist2 = Vector3.Distance(point, item2.transform.position);

                if (dist1 < dist2)
                    return -1;
                else if (dist1 == dist2)
                    return 0;
                else
                    return 1;
            }
        );
    }

    public static bool SortIfUnordered<T>(this List<T> list, Comparison<T> comparison)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (comparison.Invoke(list[i], list[i + 1]) > 0)
            {
                list.Sort(comparison);
                return true;
            }
        }
        return false;
    }
}

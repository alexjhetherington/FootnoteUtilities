using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteListExtensions
{
    public static void SortByDistance<T>(this List<T> components, Vector3 point) where T : Component
    {
        components.Sort(
            (item1, item2) =>
            {
                float dist1 = Vector3.Distance(point, item1.transform.position);
                float dist2 = Vector3.Distance(point, item1.transform.position);

                if (dist1 < dist2)
                    return -1;
                else if (dist1 == dist2)
                    return 0;
                else
                    return 1;
            }
        );
    }
}

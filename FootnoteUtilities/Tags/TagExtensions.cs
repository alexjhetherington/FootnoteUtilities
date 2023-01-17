using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagExtensions
{
    public static bool HasTag(this Component component, Tag tag)
    {
        var tags = component.GetComponent<Tags>();
        if (tags != null)
            return tags.tags.Contains(tag);

        return false;
    }
}

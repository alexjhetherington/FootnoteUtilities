using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagExtensions
{
    public static bool HasTag(this Component component, Tag tag)
    {
        var tags = component.GetOrAddComponent<Tags>();
        return tags.tags.Contains(tag);
    }
}

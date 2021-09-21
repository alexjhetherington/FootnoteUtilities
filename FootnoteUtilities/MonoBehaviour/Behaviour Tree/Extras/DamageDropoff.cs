using System;
using UnityEngine;

class DamageDropoff
{
    public static int calculate(RangeToDamage[] rangesToDamage, Transform transform, Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return calculate(rangesToDamage, distance);
    }

    public static int calculate(RangeToDamage[] rangesToDamage, float distance)
    {
        foreach (RangeToDamage rangeToDamage in rangesToDamage)
        {
            if (distance < rangeToDamage.range) return rangeToDamage.damage;
        }
        return 0;
    }
}

[Serializable]
public struct RangeToDamage
{
    public float range;
    public int damage;
}

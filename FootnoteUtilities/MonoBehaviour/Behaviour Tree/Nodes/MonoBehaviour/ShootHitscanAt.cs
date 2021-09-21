using System;
using UnityEngine;

public class ShootHitscanAt : ShootAt
{
    [SerializeField] private RangeToDamage[] rangesToDamage = default;
    [SerializeField] private AudioClip shootSound = default;
    [SerializeField] private ParticleSystem muzzleFlash = default;
    [SerializeField] private int blockingLayerMask = default;

    public override void Cancel()
    {
        return;
    }

    public override void Run(ParentNode parent)
    {
        Transform target;
        bool foundTarget = brain.Blackboard.TryGetTypedValue(targetKey, out target);

        if (!foundTarget)
        {
            parent.HandleChildFailed();
            return;
        }
        
        if (shootSound != null) SoundManager.PlaySound(shootSound);
        if (muzzleFlash != null) muzzleFlash.Play();

        if (!targetBlocked(target))
        {
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.Value -= DamageDropoff.calculate(rangesToDamage, transform, target);
            }
        }
        parent.HandleChildComplete();
    }

    protected bool targetBlocked(Transform player)
    {
        return Physics.Linecast(transform.position, player.position, blockingLayerMask);
    }
}
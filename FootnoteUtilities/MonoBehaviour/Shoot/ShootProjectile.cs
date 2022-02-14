using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour, Shoot
{
    [SerializeField]
    private float horizontalSpray = default;
    [SerializeField]
    private float verticalSpray = default;

    [SerializeField]
    private GameObject projectilePrefab = default;

    public void Shoot(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction =
            Quaternion.Euler(
                0,
                UnityEngine.Random.Range(-horizontalSpray / 2, horizontalSpray / 2),
                0
            ) * direction;
        direction =
            Quaternion.Euler(UnityEngine.Random.Range(-verticalSpray / 2, verticalSpray / 2), 0, 0)
            * direction;
        PoolManager.SpawnObject(
            projectilePrefab,
            transform.position + Vector3.up + (direction * 1f),
            Quaternion.LookRotation(direction, Vector3.up)
        );
    }
}

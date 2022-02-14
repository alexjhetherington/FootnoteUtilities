using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDiedReplaceWithPrefab : MonoBehaviour, OnDiedHandler
{
    [SerializeField]
    private GameObject prefab;

    public void OnDied()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

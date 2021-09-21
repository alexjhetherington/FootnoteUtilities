using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(int.MaxValue)]
public class DisableAfterStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}

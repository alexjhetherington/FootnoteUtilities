using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(int.MaxValue)]
public class DisableAfterAwake : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}

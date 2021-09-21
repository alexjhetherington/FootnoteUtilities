using UnityEngine;

public class VerticalBob : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 2;
    [SerializeField] private float bobDistance = 0.2f;

    private float lastSin = 0f;

    void Update()
    {
        float sin = Mathf.Sin(Time.time * bobSpeed) * bobDistance;

        transform.localPosition += Vector3.up * (sin - lastSin);

        lastSin = sin;
    }
}
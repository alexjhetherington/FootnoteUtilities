using UnityEngine;

public class VerticalBob : MonoBehaviour
{
    public float bobSpeed = 2;
    public float bobDistance = 0.2f;
    public bool randomisePhaseOnAwake = true;

    private float lastSin = 0f;
    private float offset = 0;

    void Start()
    {
        if (randomisePhaseOnAwake)
            offset = Random.Range(0, 2 * Mathf.PI);
    }

    void Update()
    {
        float sin = Mathf.Sin(offset + (Time.time * bobSpeed)) * bobDistance;

        transform.localPosition += Vector3.up * (sin - lastSin);

        lastSin = sin;
    }
}

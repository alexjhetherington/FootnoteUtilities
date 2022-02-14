using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    [SerializeField]
    private float xScrollSpeed = 0f;
    [SerializeField]
    private float yScrollSpeed = 0.1f;
    private Renderer r;
    void Start()
    {
        r = GetComponent<Renderer>();
    }

    void Update()
    {
        r.material.SetTextureOffset(
            "_MainTex",
            new Vector2(xScrollSpeed * Time.time, yScrollSpeed * Time.time)
        );
    }
}

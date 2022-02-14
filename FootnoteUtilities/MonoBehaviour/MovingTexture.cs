using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    [SerializeField]
    private float xScrollSpeed = 0f;
    [SerializeField]
    private float yScrollSpeed = 0.1f;
    private Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.material.SetTextureOffset(
            "_MainTex",
            new Vector2(xScrollSpeed * Time.time, yScrollSpeed * Time.time)
        );
    }
}

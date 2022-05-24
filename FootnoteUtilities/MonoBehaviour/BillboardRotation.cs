using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotation : MonoBehaviour
{
    [SerializeField]
    private bool rotateAtEdgeOfScreen;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null)
            return;
        if (rotateAtEdgeOfScreen)
        {
            var lookPos = transform.position - cam.transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
        else
        {
            var lookPos = -camera.transform.forward;
            lookPos.y = 0;
            transform.forward = lookPos;
        }
    }
}

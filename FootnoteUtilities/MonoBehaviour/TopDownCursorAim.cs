using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCursorAim : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var lookPos = hit.point - transform.position;
            lookPos.y = 0;

            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}

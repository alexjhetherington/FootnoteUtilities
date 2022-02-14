using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDown3dController : MonoBehaviour, Mover
{
    [SerializeField]
    private float speed = 1;

    private CharacterController characterController;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horz, 0, vert).normalized;
        Vector3 velocity = dir * speed;

        characterController.Move(velocity * Time.deltaTime);
    }
}

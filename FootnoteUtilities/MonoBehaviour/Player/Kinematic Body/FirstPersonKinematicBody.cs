using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonKinematicBody : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    private float speed = default;
    [SerializeField]
    private float gravity = default;
    [SerializeField]
    private float mouseSensitivityVert = default;
    [SerializeField]
    private float mouseSensitivityHorz = default;

    [Header("References")]
    [SerializeField]
    private Transform camUpDown = default;
    [SerializeField]
    private Transform orientation = default;

    private KinematicBody characterController;
    private float vertical;
    private bool lockMovement;

    private float verticalInput = 0;
    private float horzInput = 0;

    private float cameraVerticalAngle = 0;

    void Awake()
    {
        characterController = GetComponent<KinematicBody>();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float turnLeftRight = Input.GetAxisRaw("Mouse X") * mouseSensitivityHorz;
        float lookUpDown = Input.GetAxisRaw("Mouse Y") * mouseSensitivityVert;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle - lookUpDown, -90, 90);
        camUpDown.transform.localRotation = Quaternion.Euler(cameraVerticalAngle, 0, 0);

        orientation.Rotate(0, turnLeftRight, 0);

        verticalInput = Input.GetAxisRaw("Vertical");
        horzInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (!lockMovement)
        {
            vertical = characterController.isGrounded ? -10 : vertical - gravity * Time.deltaTime;

            characterController.Move(
                orientation.forward * verticalInput * speed * Time.deltaTime
                    + orientation.right * horzInput * speed * Time.deltaTime
                    + new Vector3(0, vertical, 0) * Time.deltaTime
            );
        }
    }

    public void LockMovement(bool lockMovement)
    {
        this.lockMovement = lockMovement;
    }
}

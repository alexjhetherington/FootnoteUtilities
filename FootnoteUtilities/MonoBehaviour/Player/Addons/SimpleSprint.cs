using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSprint : MonoBehaviour
{
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float sprintTime;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float rechargeTime;

    [SerializeField]
    private KeyCode sprintButton;
    [SerializeField]
    private FloatVariable stamina;

    private float cooldownRemaining;

    private Mover mover;
    private bool isSprinting;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        stamina.Value = 1;
        mover.SetSpeed(normalSpeed);
    }

    private void Update()
    {
        isSprinting = Input.GetKey(sprintButton);

        if (stamina.Value <= 0)
        {
            isSprinting = false;
            stamina.Value = 0;
        }

        mover.SetSpeed(isSprinting ? sprintSpeed : normalSpeed);

        if (isSprinting)
        {
            cooldownRemaining = cooldown;
            stamina.Value -= Time.deltaTime / sprintTime;
        }
        {
            cooldownRemaining -= Time.deltaTime;

            if (cooldownRemaining <= 0)
            {
                stamina.Value += Time.deltaTime / rechargeTime;
                stamina.Value = Mathf.Min(1, stamina.Value);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private ActiveWeapon activeWeapon;

    private void Awake()
    {
        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        playerInput = GetComponentInParent<PlayerInput>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();

        Debug.Log(playerControls);
        Debug.Log(playerController);
        Debug.Log(playerInput);
        Debug.Log(activeWeapon);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void MouseFollowWithOffset()
    {
        if (playerInput.currentControlScheme == playerControls.KBMScheme.name)
        {
            Vector2 mousePosition = playerControls.Movement.Aim.ReadValue<Vector2>();
            Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(
                playerController.transform.position
            );

            // float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            if (mousePosition.x < playerScreenPoint.x)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (playerInput.currentControlScheme == playerControls.GamepadScheme.name)
        {
            Vector2 aimVector = playerControls.Movement.Aim.ReadValue<Vector2>();
            if (aimVector.x < -0.1f)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (aimVector.x > 0.1f)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
    }
}

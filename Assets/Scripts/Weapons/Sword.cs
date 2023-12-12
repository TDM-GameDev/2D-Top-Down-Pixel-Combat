using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimationPrefab;

    [SerializeField] private Transform slashAnimationSpawnPoint;

    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackCooldown = 0.45f;
    private PlayerControls playerControls;
    private Animator animator;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnimation;
    private bool attackButtonDown, isAttacking = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        playerInput = GetComponentInParent<PlayerInput>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
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
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
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
                weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
                weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (playerInput.currentControlScheme == playerControls.GamepadScheme.name)
        {
            Vector2 aimVector = playerControls.Movement.Aim.ReadValue<Vector2>();
            if (aimVector.x < -0.1f)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, 0);
                weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (aimVector.x > 0.1f)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
                weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
            weaponCollider.gameObject.SetActive(true);

            slashAnimation = Instantiate(
                slashAnimationPrefab,
                slashAnimationSpawnPoint.position,
                Quaternion.identity
            );
            slashAnimation.transform.parent = transform.parent;
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void SwingUpFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);
        if (playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void DoneAttackingAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        // Testing git changes
        if (playerInput.currentControlScheme == playerControls.KBMScheme.name) {
            Vector2 mousePosition = playerControls.Movement.Aim.ReadValue<Vector2>();
            Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            if (mousePosition.x < playerScreenPoint.x) {
                spriteRenderer.flipX = true;
            }
            else {
                spriteRenderer.flipX = false;
            }
        }
        else if (playerInput.currentControlScheme == playerControls.GamepadScheme.name) {
            Vector2 aimVector = playerControls.Movement.Aim.ReadValue<Vector2>();
            if (aimVector.x < -0.1f) {
                spriteRenderer.flipX = true;
            }
            else if (aimVector.x > 0.1f) {
                spriteRenderer.flipX = false;
            }
        }
    }
}

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
    public static PlayerController Instance;
    public bool FacingLeft
    {
        get { return _facingLeft; }
        set { _facingLeft = value; }
    }

    [SerializeField]
    private float moveSpeed = 4f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;

    private bool _facingLeft = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (playerInput.currentControlScheme == playerControls.KBMScheme.name)
        {
            Vector2 mousePosition = playerControls.Movement.Aim.ReadValue<Vector2>();
            Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            if (mousePosition.x < playerScreenPoint.x)
            {
                spriteRenderer.flipX = true;
                _facingLeft = true;
            }
            else
            {
                spriteRenderer.flipX = false;
                _facingLeft = false;
            }
        }
        else if (playerInput.currentControlScheme == playerControls.GamepadScheme.name)
        {
            Vector2 aimVector = playerControls.Movement.Aim.ReadValue<Vector2>();
            if (aimVector.x < -0.1f)
            {
                spriteRenderer.flipX = true;
                _facingLeft = true;
            }
            else if (aimVector.x > 0.1f)
            {
                spriteRenderer.flipX = false;
                _facingLeft = false;
            }
        }
    }
}

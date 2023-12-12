using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private KnockBack knockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (knockBack.GettingKnockedBack)
        {
            return;
        }
        rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
    }

    public void SetDirection(Vector2 newMoveDirection)
    {
        moveDirection = newMoveDirection;
    }
}

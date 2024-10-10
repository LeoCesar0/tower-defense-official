using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private Animator animator;

    public bool isJumping = false;
    public bool isFalling = false;
    public bool isWalking = false;
    public bool isAttacking = false;
    public bool attack1 = false;
    public bool attack2 = false;
    public bool isDead = false;
    public bool isIdle = true;
    public bool isDashing = false;
    public bool isGrounded = false;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleFalling();
        HandleGrounded();
        HandleIdle();

    }

    // COLLISIONS

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SetJumping(false);
            SetFalling(false);
            SetGrounded(true);
        }
    }


    // HANDLERS

    private void HandleIdle()
    {
        if (!isJumping & !isFalling & !isWalking & !isAttacking & !isDead & !isDashing)
        {
            SetIdle(true);
        }
        else
        {
            SetIdle(false);
        }
    }
    private void HandleGrounded()
    {
        if (!isJumping & !isFalling)
        {
            SetGrounded(true);
        }
        else
        {
            SetGrounded(false);
        }
    }

    private void HandleFalling()
    {
        bool isGoingDown = rb.velocity.y < 0;

        if (isJumping && isGoingDown)
        {
            SetJumping(false);
            SetFalling(true);
        }
    }


    // FUNCTIONS

    public void SetJumping(bool value)
    {
        if (isJumping != value)
        {
            isJumping = value;
            animator.SetBool("isJumping", value);
        }
    }
    public void SetGrounded(bool value)
    {
        if (isGrounded != value)
        {
            isGrounded = value;
        }
    }


    public void SetFalling(bool value)
    {
        if (isFalling != value)
        {
            isFalling = value;
            animator.SetBool("isFalling", value);
        }
    }

    public void SetDashing(bool value)
    {
        if (isDashing != value)
        {
            isDashing = value;
            animator.SetBool("isDashing", value);
        }
    }

    public void SetWalking(bool value)
    {
        if (isWalking != value)
        {
            isWalking = value;
            animator.SetBool("isWalking", value);
        }
    }

    public void SetAttacking(bool value)
    {
        if (isAttacking != value)
        {
            isAttacking = value;
            animator.SetBool("isAttacking", value);
        }
    }

    public void SetAttack1(bool value)
    {
        if (attack1 != value)
        {
            attack1 = value;
            animator.SetBool("attack1", value);
        }
    }

    public void SetAttack2(bool value)
    {
        if (attack2 != value)
        {
            attack2 = value;
            animator.SetBool("attack2", value);
        }
    }

    public void SetDead(bool value)
    {
        if (isDead != value)
        {
            isDead = value;
            animator.SetBool("isDead", value);
        }
    }

    public void SetIdle(bool value)
    {
        if (isIdle != value)
        {
            isIdle = value;
            animator.SetBool("isIdle", value);
        }
    }


}
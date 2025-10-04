using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f; // Walking speed
    public float jumpForce = 5f; // Jump force (increased for more wild jumping)
    public float dashSpeed = 10f; // Speed when dashing
    public float dashDuration = 0.2f; // How long the dash lasts

    // Private variables for internal use
    private Rigidbody2D rb;
    private float jumpTimeCounter; // To control how long the player holds the jump
    private float dashTime; // To track how long the player is dashing
    private Vector2 dashDirection; // To store the direction of the dash

    private PlayerStateMachine playerStateMachine;

    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
    }

    // Function to handle basic movement and dashing
    void HandleMovement()
    {
        if (playerStateMachine.GetCurrentState() is DashingState) return; // Skip regular movement if player is dashing

        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0 && IsGrounded())
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * moveX, transform.localScale.y, transform.localScale.z);
            playerStateMachine.TryTransitionToWalking();
        }
        else if (moveX == 0)
        {
            playerStateMachine.TryTransitionToIdle();
        }
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }


    // Function to handle jumping with more control over height
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            playerStateMachine.TryTransitionToJumping();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !(playerStateMachine.GetCurrentState() is DashingState))
        {
            playerStateMachine.TryTransitionToDashing();
            dashTime = dashDuration;

            float moveX = Input.GetAxisRaw("Horizontal");
            dashDirection = new Vector2(moveX != 0 ? moveX : transform.localScale.x, 0); // Dash in direction of movement
        }

        if (playerStateMachine.GetCurrentState() is DashingState)
        {
            rb.velocity = dashDirection * dashSpeed;

            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                playerStateMachine.TryTransitionToIdle(); // End dash
            }
        }
    }
    
    // Helper method to check if grounded
    private bool IsGrounded()
    {
        // Simple ground check - you can improve this with raycasting
        return Mathf.Abs(rb.velocity.y) < 0.1f;
    }

    // Check if the player is grounded to reset jumping and dashing

}

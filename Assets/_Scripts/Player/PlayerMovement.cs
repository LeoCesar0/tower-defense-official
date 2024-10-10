using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 8f; // Walking speed
    public float jumpForce = 5f; // Jump force (increased for more wild jumping)
    public float dashSpeed = 10f; // Speed when dashing
    public float dashDuration = 0.2f; // How long the dash lasts

    // Private variables for internal use
    private Rigidbody2D rb;
    private float jumpTimeCounter; // To control how long the player holds the jump
    private float dashTime; // To track how long the player is dashing
    private Vector2 dashDirection; // To store the direction of the dash

    private PlayerStateController playerState;

    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerStateController>();
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
        if (playerState.isDashing) return; // Skip regular movement if player is dashing

        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0 && playerState.isGrounded)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * moveX, transform.localScale.y, transform.localScale.z);
            playerState.SetWalking(true);
        }
        else
        {
            playerState.SetWalking(false);
        }
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }


    // Function to handle jumping with more control over height
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && playerState.isGrounded)
        {
            playerState.SetJumping(true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !playerState.isDashing)
        {
            playerState.SetDashing(true);
            dashTime = dashDuration;

            float moveX = Input.GetAxisRaw("Horizontal");
            dashDirection = new Vector2(moveX != 0 ? moveX : transform.localScale.x, 0); // Dash in direction of movement
        }

        if (playerState.isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;

            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                playerState.SetDashing(false); // End dash
            }
        }
    }

    // Check if the player is grounded to reset jumping and dashing

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 5f; // Walking speed
    public float jumpForce = 10f; // Jump force (increased for more wild jumping)
    public float maxJumpTime = 0.35f; // Time the jump button can be held
    public float dashSpeed = 10f; // Speed when dashing
    public float dashDuration = 0.2f; // How long the dash lasts

    // Private variables for internal use
    private Rigidbody2D rb;
    private bool isGrounded = true; // Check if player is on the ground
    private float jumpTimeCounter; // To control how long the player holds the jump
    private bool isJumping; // Track if player is in the process of jumping
    private bool isDashing = false; // Track if player is dashing
    private float dashTime; // To track how long the player is dashing
    private Vector2 dashDirection; // To store the direction of the dash

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (isDashing) return; // Skip regular movement if player is dashing

        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    // Function to handle jumping with more control over height
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    // Function to handle dashing
    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;

            float moveX = Input.GetAxisRaw("Horizontal");
            dashDirection = new Vector2(moveX != 0 ? moveX : transform.localScale.x, 0); // Dash in direction of movement
        }

        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;

            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false; // End dash
            }
        }
    }

    // Check if the player is grounded to reset jumping and dashing
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLISION ENTER");
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("GROUND ENTER");
            isGrounded = true;
            isJumping = false;
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Increases fall speed for realistic gravity
    public float lowJumpMultiplier = 2f; // Increases gravity for short jumps
    public float coyoteTime = 0.2f; // Duration player can still jump after leaving the ground

    [Header("Components")]
    public Rigidbody rb;
    public Transform spriteTransform;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private float coyoteTimeCounter;
    private bool isFacingRight = true;
    private bool isJumping;

    private void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyJumpModifiers();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(moveInput * moveSpeed, velocity.y, velocity.z);

        // Flip the sprite direction
        if (moveInput > 0 && !isFacingRight)
        {
            FlipSprite();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            FlipSprite();
        }
    }

    private void HandleJump()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
            isJumping = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isJumping = true;
            coyoteTimeCounter = 0f; // Prevent multiple jumps during coyote time
        }

        // Prevent sticking to walls by resetting vertical velocity when colliding horizontally
        if (Physics.Raycast(transform.position, Vector3.right, 0.6f, groundLayer) ||
            Physics.Raycast(transform.position, Vector3.left, 0.6f, groundLayer))
        {
            if (!isGrounded && rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
    }

    private void ApplyJumpModifiers()
    {
        if (rb.velocity.y < 0)
        {
            // Apply fall multiplier for faster falling
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Apply low jump multiplier when jump button is released early
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = spriteTransform.localScale;
        scale.x *= -1;
        spriteTransform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
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

    [Header("Double Jump")]
    public bool hasDoubleJump = false; // Enables double jump

    private float coyoteTimeCounter;
    private bool isFacingRight = true;
    private bool isJumping;
    private bool canDoubleJump;

    [Header("Fall Through Platform")]
    public LayerMask fallThroughPlatformLayer;
    private Collider playerCollider;

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyJumpModifiers();

        if (Input.GetKeyDown(KeyCode.S))
        {
            DropThroughPlatform();
        }
    }

    private void DropThroughPlatform()
    {
        Collider[] platforms = Physics.OverlapBox(
            transform.position,
            new Vector3(0.5f, 0.5f, 0.5f), // Adjust size to fit your player/platform dimensions
            Quaternion.identity,
            fallThroughPlatformLayer
        );

        foreach (Collider platform in platforms)
        {
            Physics.IgnoreCollision(playerCollider, platform, true);
            StartCoroutine(ReenableCollision(platform));
        }
    }

    private System.Collections.IEnumerator ReenableCollision(Collider platform)
    {
        // Re-enable collision after a short delay
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(playerCollider, platform, false);
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(moveInput * moveSpeed, velocity.y, velocity.z);

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
            canDoubleJump = true; // Reset double jump when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                isJumping = true;
                coyoteTimeCounter = 0f; // Prevent multiple jumps during coyote time
            }
            else if (hasDoubleJump && canDoubleJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                canDoubleJump = false; // Disable double jump after use
                isJumping = true; // Ensure double jump is recognized as a jump
            }
        }
    }

    private void ApplyJumpModifiers()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Apply fall multiplier for faster falling
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Apply low jump multiplier when jump button is released early
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
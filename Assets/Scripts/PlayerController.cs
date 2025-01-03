using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Components")]
    public Rigidbody rb;
    public Transform spriteTransform;
    private Collider playerCollider;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Double Jump")]
    public bool hasDoubleJump = false;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private float dashTime;
    private float dashCooldownTime;

    private bool isFacingRight = true;
    private bool isJumping;
    private bool canDoubleJump;

    [Header("Fall Through Platform")]
    public LayerMask fallThroughPlatformLayer;

    private bool isInvincible = false; // Tracks invincibility state
    private bool canMove = true; // Check if the player can move (Cannot when attacked or damaged due to stun)
    private float stunTime = 0.2f;
    private float stunTimer;

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        stunTimer = stunTime;
    }

    private void Update()
    {
        if (!canMove) stunTimer -= Time.deltaTime;
        else stunTimer = stunTime;

        if (stunTimer <= 0) SetCanMove(true);

        if (isDashing)
        {
            Dash();
        }
        else
        {
            HandleMovement();
            HandleJump();
            ApplyJumpModifiers();

            if (Input.GetKeyDown(KeyCode.S))
            {
                DropThroughPlatform();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= dashCooldownTime)
            {
                StartDash();
            }
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        dashCooldownTime = Time.time + dashCooldown;

        // Start invincibility during dash
        StartCoroutine(EnableInvincibility(dashDuration));
    }

    private void Dash()
    {
        float dashDirection = isFacingRight ? 1f : -1f;
        Vector3 dashVelocity = new Vector3(dashDirection * dashSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        rb.linearVelocity = dashVelocity;

        if (Time.time >= dashTime)
        {
            isDashing = false;
        }

        // Check for collision with walls
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right * dashDirection, out hit, 1f, groundLayer))
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
            isDashing = false;
        }
    }

    private System.Collections.IEnumerator EnableInvincibility(float duration)
    {
        isInvincible = true;
        Debug.Log("Player is now invincible.");

        // Wait for the dash duration
        yield return new WaitForSeconds(duration);

        isInvincible = false;
        Debug.Log("Player is no longer invincible.");
    }

    private void DropThroughPlatform()
    {
        Collider[] platforms = Physics.OverlapBox(
            transform.position,
            new Vector3(0.5f, 0.5f, 0.5f),
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
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(playerCollider, platform, false);
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (canMove)
        {
            rb.linearVelocity = new Vector3(moveInput * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

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
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            canDoubleJump = true;
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
                coyoteTimeCounter = 0f;
            }
            else if (hasDoubleJump && canDoubleJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                canDoubleJump = false;
                isJumping = true;
            }
        }
    }

    private void ApplyJumpModifiers()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
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

    public void SetCanMove(bool value) => canMove = value;
    public float GetSpeed => moveSpeed;
    public void SetMoveSpeed(float value) => moveSpeed = value;

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

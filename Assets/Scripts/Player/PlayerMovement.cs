using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private StatsHandler statsHandler;

    [Space(15)]
    [Header("CHECKS")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [Space(10)]
    [SerializeField] private LayerMask groundLayer;
    [Space(10)]
    [Header("CROUCH")]
    [SerializeField] private CapsuleCollider standingCollider;
    [SerializeField] private CapsuleCollider crouchCollider;

    private Rigidbody playerRB;

    private bool isGrounded;
    private bool isJumpCut;
    private bool hasAirJump = true;
    private bool canAirJump = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isDashing = false;
    private bool canDash;
    private bool isFacingRight = true;
    private bool isStun = false;
    private bool isFiringDiagonal = false;

    //TIMERS
    // Coyote time is the timer after jumping off a ledge
    //Jump Buffer timer is after space is pressed
    private float jumpBufferTimer;
    private float coyoteTimer;
    private float dashStopTimer;
    private float dashCooldownTimer;
    private float stunTimer;

    private float horizontalInput;
    private float verticalInput;
    private float gravityScale;
    private float lastInputX;
    #endregion

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        hasAirJump = true;
        canAirJump = false;
        dashCooldownTimer = playerDataSO.dashCooldown * statsHandler.dashCooldownMultiplier;
        standingCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    private void Start()
    {
        gravityScale = playerDataSO.gravityScale * statsHandler.gravityMultiplier;
    }

    private void Update()
    {
        #region Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCooldownTimer <= 0 && canDash) Dash();
        }
        if (Input.GetKeyDown(KeyCode.Space)) jumpBufferTimer = playerDataSO.jumpBufferTime;
        if (Input.GetKeyUp(KeyCode.Space)) isJumpCut = isJumping;
        #endregion

        isFiringDiagonal = Input.GetMouseButton(0) && horizontalInput != 0 && verticalInput != 0;
        isGrounded = IsGrounded();

        //TIMERS
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
        dashStopTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        if (stunTimer > 0) stunTimer -= Time.deltaTime;
        else if (stunTimer <= 0) isStun = false;

        if (isGrounded)
        {
            coyoteTimer = playerDataSO.coyoteTime;
            isJumpCut = false;
            isFalling = false;
            if (canAirJump) hasAirJump = true;
            canDash = true;
            if (jumpBufferTimer > 0 && !isJumping)
            {
                isJumping = true;
                isJumpCut = false;
            }

            if (verticalInput < 0)
            {
                crouchCollider.enabled = true;
                standingCollider.enabled = false;
            }
            else
            {
                crouchCollider.enabled = false;
                standingCollider.enabled = true;
            }
        }
        else if (hasAirJump && jumpBufferTimer > 0 && !isDashing)
        {
            isJumpCut = false;
            isFalling = false;
            hasAirJump = false;
            if (!isJumping)
            {
                isJumping = true;
                isJumpCut = false;
            }
            Jump(playerDataSO.airJumpForceMultiplier);
        }

        //Flip Sprite in the direciton of motion
        if (horizontalInput > 0) isFacingRight = true;
        else if (horizontalInput < 0) isFacingRight = false;
        FlipSprite();

        if (!isDashing && !isStun && !isFiringDiagonal)
        {
            Move();
        }

        if (isJumping && playerRB.linearVelocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
        }

        if (coyoteTimer > 0 && jumpBufferTimer > 0 && !isDashing)
        {
            Jump(1);
        }

        if (playerRB.linearVelocity.y < 0)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, Mathf.Max(playerRB.linearVelocity.y, -playerDataSO.maxFallSpeed), playerRB.linearVelocity.z);
        }

        if (isDashing && dashStopTimer <= 0)
        {
            isDashing = false;
            gravityScale = playerDataSO.gravityScale;
            playerRB.linearVelocity = Vector2.zero;
            dashCooldownTimer = playerDataSO.dashCooldown * statsHandler.dashCooldownMultiplier;
            playerRB.linearVelocity = Vector2.zero;
        }

        #region GRAVITY
        if (isDashing)
        {
            gravityScale = 0;
        }
        else if (isJumpCut)
        {
            gravityScale = playerDataSO.gravityScale * playerDataSO.jumpCutGravityMultiplier * statsHandler.gravityMultiplier;
        }
        else if ((isJumping || isFalling) && Mathf.Abs(playerRB.linearVelocity.y) < playerDataSO.jumpHangTimeThreshold)
        {
            gravityScale = playerDataSO.gravityScale * playerDataSO.jumpHangGravityMultiplier * statsHandler.gravityMultiplier;
        }
        else gravityScale = playerDataSO.gravityScale * statsHandler.gravityMultiplier;
        #endregion
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void Move()
    {
        float moveSpeed = (isGrounded && verticalInput < 0) ? playerDataSO.crouchMoveSpeed : playerDataSO.moveSpeed;
        playerRB.linearVelocity = new Vector3(moveSpeed * horizontalInput * statsHandler.moveSpeedMultiplier, playerRB.linearVelocity.y, playerRB.linearVelocity.z);
    }

    private void Jump(float jumpForceMultiplier)
    {
        jumpBufferTimer = 0;
        coyoteTimer = 0;
        isJumping = true;

        float force = playerDataSO.jumpForce * jumpForceMultiplier;
        force -= playerRB.linearVelocity.y;
        force *= playerRB.mass;
        playerRB.AddForce(Vector2.up * force, ForceMode.Impulse);
    }

    private void ApplyGravity()
    {
        playerRB.AddForce(Physics.gravity * gravityScale, ForceMode.Force);
    }

    private void Dash()
    {
        isDashing = true;
        canDash = false;
        dashStopTimer = playerDataSO.dashTime;
        playerRB.linearVelocity = new Vector2(horizontalInput * playerDataSO.dashVelocity, 0);
    }

    private void FlipSprite()
    {
        if (!isFacingRight) transform.eulerAngles = new Vector3(0, 180, 0);
        else transform.eulerAngles = Vector3.zero;
    }

    private bool IsGrounded()
        => Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

    public void SetStun(float stunTime)
    {
        isStun = true;
        stunTimer = stunTime;
    }

    public void CanDoubleJump(bool condition) => canAirJump = condition;
    public Vector2 GetMovementDirection() => new Vector2(horizontalInput, verticalInput);
    public bool GetIsGrounded() => isGrounded;

    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}

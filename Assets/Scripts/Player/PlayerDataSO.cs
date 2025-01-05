using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private StatsHandler statsHandler;
    [Space(5)]
    [Header("Gravity")]
    [HideInInspector] public float gravity; //TO BE CALCULATED IN ONVALIDATE
    [HideInInspector] public float gravityScale;
    [Space(5)]
    public float fallGravityMultiplier;
    public float maxFallSpeed;
    [Space(5)]
    public float fastFallMultiplier;

    [Header("Move")]
    public float moveSpeed;

    [Space(20)]
    [Header("Jump")]
    public float jumpHeight;
    public float jumpReachTime;
    public float jumpForce;
    [Range(0f, 1f)] public float airJumpForceMultiplier;

    [Space(10)]
    [Header("Air Control Values")]
    public float jumpCutGravityMultiplier;
    [Space(1)]
    [Range(0f, 1f)] public float jumpHangGravityMultiplier;
    public float jumpHangTimeThreshold;
    public float jumpHangAccelerationMultiplier;
    public float jumpHangMoveSpeedMultiplier;

    [Space(20)]
    [Header("Dash")]
    public float dashCooldown;
    public float dashDistanceTravel;
    public float dashAcceleration;
    public float dashVelocity;
    public float dashTime;

    [Space(20)]
    [Header("Assists")]
    [Range(0f, 0.5f)] public float jumpBufferTime;
    [Range(0f, 0.5f)] public float coyoteTime;

    private void OnValidate()
    {
        gravity = -(2 * jumpHeight) / (jumpReachTime * jumpReachTime);
        gravityScale = gravity / Physics.gravity.y;

        jumpForce = Mathf.Abs(gravity) * jumpReachTime;
        dashVelocity = Mathf.Sqrt(2 * dashAcceleration * (dashDistanceTravel * statsHandler.dashDistanceMultiplier));
        dashTime = dashVelocity / dashAcceleration;
    }
}

using UnityEngine;

public class OverWorldPlayerScript : MonoBehaviour
{
    public float speed = 10f; // Maximum movement speed
    public float acceleration = 20f; // How quickly the player reaches max speed
    public float deceleration = 15f; // How quickly the player slows down
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private SpriteRenderer sr;

    private Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // Handle Ground Detection
        HandleGroundDetection();

        // Handle Movement
        HandleMovement();
    }

    private void HandleGroundDetection()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1f; // Adjust the starting position of the raycast

        if (Physics.Raycast(castPos, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider != null)
            {
                // Smoothly adjust the player's Y position to match the ground
                float targetY = hit.point.y + groundCheckDistance;
                Vector3 targetPosition = new Vector3(rb.position.x, targetY, rb.position.z);
                rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, 0.2f)); // Smooth interpolation
            }
        }
    }

    private void HandleMovement()
    {
        // Get Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        // Calculate target velocity
        Vector3 targetVelocity = inputDir * speed;

        // Smoothly accelerate or decelerate to target velocity
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, 
            (inputDir.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);

        // Apply movement
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);

        // Flip sprite based on movement direction
        if (x > 0)
            sr.flipX = false;
        else if (x < 0)
            sr.flipX = true;
    }
}

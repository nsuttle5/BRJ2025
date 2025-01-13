using UnityEngine;

public class DiskMovement : MonoBehaviour {
    public Transform diskCenterTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float diskRadius = 5f;
    
    private float currentAngle;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set initial position
        transform.position = diskCenterTransform.position + new Vector3(diskRadius, transform.position.y, 0);
    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal");
        currentAngle += direction * moveSpeed * Time.fixedDeltaTime;
    
        // Calculate target position, maintaining the original Y position
        float targetX = diskCenterTransform.position.x + diskRadius * Mathf.Cos(currentAngle);
        float targetZ = diskCenterTransform.position.z + diskRadius * Mathf.Sin(currentAngle);
        Vector3 targetPosition = new Vector3(targetX, diskCenterTransform.position.y, targetZ);
    
        Vector3 moveDirection = (targetPosition - rb.position);
        // Apply velocity but maintain any existing Y velocity for jumping/falling if needed
        rb.linearVelocity = new Vector3(moveDirection.x / Time.fixedDeltaTime, 
            rb.linearVelocity.y,  // Keep existing Y velocity
            moveDirection.z / Time.fixedDeltaTime);
    }
    
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(diskCenterTransform.position, Vector3.up, diskRadius);
    }
}
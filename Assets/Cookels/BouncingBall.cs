using UnityEngine;

public class BouncingBall : MonoBehaviour {
    public float radius = 5f;        
    public float moveSpeed = 2f;     
    public bool clockwise = true;    
    public float initialHeightOffset = 20f;
    public float bounceEverySeconds = 2f;
    public float bounceForce = 10f;
    public Transform stageCenterTransform;
    
    private float currentAngle;
    private float timeSinceLastBounce;
    private Rigidbody rb;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Rigidbody component missing from the ball!");
            return;
        }
        
        // Constrain rotation and movement on X and Z axes
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        transform.position = stageCenterTransform.position + new Vector3(radius, initialHeightOffset, 0);
        timeSinceLastBounce = 0f;
    }
    
    public void Initialize(GameObject stageCenter) {
        stageCenterTransform = stageCenter.transform;
    }
    
    void FixedUpdate() {
        if (stageCenterTransform == null) return;
        
        HandleMovementAndRotation();
        HandleBouncing();
    }

    void HandleMovementAndRotation() {
        float direction = clockwise ? -1f : 1f;
        currentAngle += direction * moveSpeed * Time.fixedDeltaTime;
        
        // Calculate new position on circle
        float newX = stageCenterTransform.position.x + radius * Mathf.Cos(currentAngle);
        float newZ = stageCenterTransform.position.z + radius * Mathf.Sin(currentAngle);
        
        // Create the new position vector, keeping the current Y position
        Vector3 newPosition = new Vector3(newX, rb.position.y, newZ);
        
        // Set the rigidbody position directly for X and Z, letting physics handle Y
        rb.MovePosition(newPosition);
        
        // Optional: Zero out any horizontal velocity to prevent drift
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, velocity.y, 0f);
    }
    
    void HandleBouncing() {
        timeSinceLastBounce += Time.fixedDeltaTime;
        
        if (timeSinceLastBounce >= bounceEverySeconds) {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            timeSinceLastBounce = 0f;
        }
    }
    
    void OnDrawGizmos() {
        if (stageCenterTransform != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(stageCenterTransform.position, radius);
        }
    }
}
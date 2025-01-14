using UnityEngine;

public class CircularMovement : MonoBehaviour 
{
    public float radius = 5f;        // Radius of the circle
    public float moveSpeed = 2f;     // Speed in radians per second
    public bool clockwise = true;    // Direction of movement
    public float initialHeightOffset = 1f;
    public Transform centerTransform;

    private float currentAngle;
    
    void Start()
    {
        // Set initial position
        transform.position = centerTransform.position + new Vector3(radius, initialHeightOffset, 0);
    }
    
    void Update()
    {
        if (centerTransform == null) return;

        // Update the angle (clockwise or counter-clockwise)
        float direction = clockwise ? -1f : 1f;
        currentAngle += direction * moveSpeed * Time.deltaTime;
        
        // Calculate new position on X and Z axes
        float newX = centerTransform.position.x + radius * Mathf.Cos(currentAngle);
        float newZ = centerTransform.position.z + radius * Mathf.Sin(currentAngle);
        
        // Update sprite position, maintaining its Y position
        transform.position = new Vector3(newX, transform.position.y, newZ);

        // Rotate sprite to face movement direction (around Y axis for 2.5D)
        float angle = Mathf.Atan2(newZ - centerTransform.position.z, newX - centerTransform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle - 90, 0);
    }
    
    void OnDrawGizmos()
    {
        if (centerTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centerTransform.position, radius);
        }
    }
}
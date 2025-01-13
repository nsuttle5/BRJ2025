using UnityEngine;

public class CookelsBycicleAttack : MonoBehaviour {
    public float radius = 5f;        // Radius of the circle
    public float moveSpeed = 2f;     // Speed in radians per second
    public bool clockwise = true;    // Direction of movement
    public bool spriteFacesMovementDirection;
    public float initialHeightOffset = 1f;
    public Transform stageCenterTransform;
    
    private float currentAngle;
    private bool isEnabled;
    
    // ToDo: add animations

    public void Enable() {
        if (isEnabled) return;
        // Set initial position, maybe transition slowly instead of instant snap lol
        transform.position = stageCenterTransform.position + new Vector3(radius, initialHeightOffset, 0);
        isEnabled = true;
    }

    public void Disable() {
        isEnabled = false;
    }
    
    void Update() {
        if (!isEnabled) return;
        if (stageCenterTransform == null) return;
        
        HandleMovementAndRotation();
        // ToDo: implement crashing of environment and damage to player if hits are detected
    }

    void HandleMovementAndRotation() {
        // Update the angle (clockwise or counter-clockwise)
        float direction = clockwise ? -1f : 1f;
        currentAngle += direction * moveSpeed * Time.deltaTime;
        
        // Calculate new position on X and Z axes
        float newX = stageCenterTransform.position.x + radius * Mathf.Cos(currentAngle);
        float newZ = stageCenterTransform.position.z + radius * Mathf.Sin(currentAngle);
        
        // Update sprite position, maintaining its Y position
        transform.position = new Vector3(newX, transform.position.y, newZ);
        
        // Rotate sprite to face movement direction (around Y axis for 2.5D)
        if (spriteFacesMovementDirection) {
            float angle = Mathf.Atan2(newZ - stageCenterTransform.position.z, newX - stageCenterTransform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle - 90, 0);
        }
    }
    
    void OnDrawGizmos()
    {
        if (stageCenterTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(stageCenterTransform.position, radius);
        }
    }
}

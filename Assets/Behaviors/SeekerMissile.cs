using UnityEngine;

public class SeekerMissile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;                    // Base movement speed
    [SerializeField] private float rotationSpeed = 200f;          // How fast the missile rotates
    [SerializeField] private float serpentineFrequency = 2f;     // How fast the serpentine pattern oscillates
    [SerializeField] private float serpentineAmplitude = 1f;     // How wide the serpentine pattern is

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;          // Explosion effect prefab
    [SerializeField] private float explosionRadius = 2f;         // Explosion damage radius
    [SerializeField] private float explosionForce = 500f;        // Force applied to rigidbodies
    [SerializeField] private float damage = 50f;                 // Damage dealt by explosion

    private Transform player;                                     // Reference to player transform
    private float serpentineOffset;                              // Current offset in the serpentine pattern
    private Vector3 previousPosition;                            // Previous frame position for direction
    private Rigidbody rb;                                       // Missile's rigidbody

    private void Start()
    {
        // Find the player (you might want to pass this reference instead)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;

        // Freeze rotation and Z-axis movement
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationY |
                        RigidbodyConstraints.FreezePositionZ;

        // Set drag to prevent unwanted physics behavior
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Calculate direction to player (ignoring Z axis)
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.z = 0; // Ensure no Z movement
        Vector3 directionToPlayer = toPlayer.normalized;

        // Update serpentine offset
        serpentineOffset += Time.fixedDeltaTime * serpentineFrequency;

        // Calculate perpendicular vector for serpentine movement
        Vector3 perpendicularDir = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0);

        // Combine forward movement with serpentine pattern
        Vector3 movement = directionToPlayer + perpendicularDir * Mathf.Sin(serpentineOffset) * serpentineAmplitude;
        movement.Normalize();

        // Apply movement using Rigidbody
        rb.linearVelocity = movement * speed;

        // Update rotation to face movement direction (only around Z axis)
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        rb.MoveRotation(Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        ));

        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        // Spawn explosion effect if prefab is assigned
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Find all nearby colliders
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Apply explosion force to rigidbodies (force only in XY plane)
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (nearbyObject.transform.position - transform.position);
                direction.z = 0; // Ensure force is only in XY plane
                direction.Normalize();
                rb.AddForce(direction * explosionForce);
            }

            // Apply damage to objects with health component
            IHealthSystem healthSystem = nearbyObject.GetComponent<IHealthSystem>();
            if (healthSystem != null)
            {
                // Calculate damage falloff based on distance (ignoring Z axis)
                Vector3 toTarget = nearbyObject.transform.position - transform.position;
                toTarget.z = 0;
                float distance = toTarget.magnitude;
                float damageMultiplier = 1f - (distance / explosionRadius);
                float finalDamage = damage * Mathf.Max(0, damageMultiplier);

                healthSystem.TakeDamage(finalDamage);
            }
        }

        // Destroy the missile
        Destroy(gameObject);
    }

    // Optional: Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

// Interface for objects that can take damage, for now, we should modify later?
public interface IHealthSystem
{
    void TakeDamage(float amount);
}
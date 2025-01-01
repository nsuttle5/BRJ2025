using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab
    public Transform firePoint;         // The transform of the FirePoint GameObject
    public float projectileSpeedMultiplier = 1f; // Multiplier for the projectile speed
    public float projectileSpeed = 10f; // Speed of the projectile
    public float projectileLifetime = 5f; // Time before the projectile is destroyed


    void Start()
    {
        projectileSpeed *= projectileSpeedMultiplier;
    }

    void Update()
    {
        Aim();
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Shoot();
        }
    }

    void Aim()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's on the same plane as the player

        // Rotate the firePoint to face the mouse
        Vector3 aimDirection = (mousePos - firePoint.position).normalized;
        firePoint.right = aimDirection;
    }

    void Shoot()
    {
        // Instantiate the projectile at the firePoint position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Apply velocity to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }

        // Destroy the projectile after a set lifetime
        Destroy(projectile, projectileLifetime);
    }
}

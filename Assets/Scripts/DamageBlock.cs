using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage inflicted

    private void OnCollisionEnter(Collision collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

        if (player != null)
        {
            // Calculate knockback direction
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;

            // Ensure the knockback has a slight upward angle to work in both grounded and airborne cases
            if (knockbackDirection.y <= 0.1f) // Minimal upward knockback if grounded
            {
                knockbackDirection.y = 0.5f;
            }

            // Apply damage and knockback
            player.TakeDamage(damageAmount, knockbackDirection);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage inflicted

    private PlayerManager player;
    List<(int, Vector3)> damageQuene;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out player))
        {
            // Calculate knockback direction
            damageQuene = player.GetDamageQuene();
            Rigidbody rb = player.GetPlayerRigidbody();
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            //Vector3 knockbackDirection = -rb.linearVelocity;

            // Ensure the knockback has a slight upward angle to work in both grounded and airborne cases
            if (knockbackDirection.y <= 0.1f) // Minimal upward knockback if grounded
            {
                knockbackDirection.y = 0.5f;
            }

            // Apply damage and knockback
            damageQuene.Add((damageAmount, knockbackDirection));
            //player.TakeDamage(damageAmount, knockbackDirection);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (damageQuene.Count > 0)
        {
            damageQuene.RemoveAt(damageQuene.Count - 1);
        }
    }
}

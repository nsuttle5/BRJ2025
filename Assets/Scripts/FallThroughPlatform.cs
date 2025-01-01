using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    private Collider platformCollider;

    private void Start()
    {
        // Reference to the main (non-trigger) collider of the platform
        platformCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Disable collision only if the player is moving upwards
                if (playerRigidbody.linearVelocity.y > 0)
                {
                    Physics.IgnoreCollision(other, platformCollider, true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ensure collisions are re-enabled when the player exits the platform
            Physics.IgnoreCollision(other, platformCollider, false);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int maxHealth = 5; // Maximum health the player can have
    public int currentHealth; // Player's current health

    [Header("UI Elements")]
    public Slider healthBar; // Health bar slider to represent player's health

    [Header("Damage Settings")]
    public float invincibilityDuration = 1f; // Time the player is invincible after taking damage
    public float knockbackForce = 5f;       // Knockback force applied to the player

    private bool isInvincible = false; // Tracks if the player is invincible
    private Rigidbody rb;             // Reference to the player's Rigidbody

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerManager: Rigidbody component missing!");
        }
    }

    public void TakeDamage(int damage, Vector3 knockbackDirection)
    {
        if (isInvincible) return;

        // Apply damage
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Apply knockback
            if (rb != null)
            {
                rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
            }

            // Start invincibility period
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add death logic (e.g., respawn, game over screen)
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}

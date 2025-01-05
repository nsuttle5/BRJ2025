using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Settings")]
    public int maxHealth = 4; // Maximum health the player can have
    public int currentHealth; // Player's current health

    [Header("UI Elements")]
    [SerializeField] private Transform playerHealthUI;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform firePoint;
    public Sprite emptyHeart; // Sprite for empty heart
    public Sprite fullHeart;  // Sprite for full heart

    [Header("Damage Settings")]
    public float invincibilityDuration = 0.2f; // Time the player is invincible after taking damage
    public float knockbackForce = 5f;       // Knockback force applied to the player

    public bool isInvincible = false; // Tracks if the player is invincible
    private Rigidbody rb;             // Reference to the player's Rigidbody
    private PlayerMovement playerMovement;

    private List<(int damage, Vector3 knockback)> damageQueue = new List<(int, Vector3)>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Get the Rigidbody component
        if (rb == null)
        {
            Debug.LogError("PlayerManager: Rigidbody component missing!");
        }
    }

    private void Update()
    {
        if (damageQueue.Count > 0 && !isInvincible)
        {
            Debug.Log("DOING DAMAGE");
            int damage = damageQueue[0].damage;
            Vector3 knockback = damageQueue[0].knockback;
            TakeDamage(damage, knockback);
            damageQueue.RemoveAt(0);
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
                playerMovement.SetStun(0.2f);
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
        foreach (Transform child in playerHealthUI.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < maxHealth; i++)
        {
            Transform heart = Instantiate(heartPrefab, playerHealthUI, false).transform;
            if (i < currentHealth)
            {
                heart.GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                heart.GetComponent<Image>().sprite = emptyHeart;
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add death logic (e.g., respawn, game over screen)
    }

    public void IncreaseMaxHealth()
    {
        if (currentHealth == maxHealth) currentHealth++;
        maxHealth++;
        UpdateHealthUI();
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public Rigidbody GetPlayerRigidbody() => rb;
    public PlayerMovement GetPlayerMovement() => playerMovement;
    public List<(int, Vector3)> GetDamageQuene() => damageQueue;
    public Transform GetFirePoint() => firePoint;
}

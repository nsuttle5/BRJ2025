using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 4; // Maximum health the player can have
    [SerializeField] private int currentHealth; // Player's current health

    [Header("UI Elements")]
    [SerializeField] private Transform playerHealthUI;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite emptyHeart; // Sprite for empty heart
    [SerializeField] private Sprite fullHeart;  // Sprite for full heart

    [Header("Damage Settings")]
    [SerializeField] private float invincibilityDuration = 0.2f; // Time the player is invincible after taking damage
    [SerializeField] private float knockbackForce = 5f;       // Knockback force applied to the player

    private bool isInvincible = false; // Tracks if the player is invincible
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
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        //Give damage that was stored
        if (damageQueue.Count > 0 && !isInvincible)
        {
            int damage = damageQueue[0].damage;
            Vector3 knockback = damageQueue[0].knockback;
            TakeDamage(damage, knockback, invincibilityDuration);
            damageQueue.RemoveAt(0);
        }
    }

    public void TakeDamage(int damage, Vector3 knockbackDirection, float stunTime)
    {
        if (isInvincible) return;

        // Apply damage
        currentHealth -= damage;
        UpdateHealthUI();
        
        if (currentHealth > 0)
        {
            playerMovement.SetStun(stunTime);
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);

            // Start invincibility period
            StartCoroutine(InvincibilityCoroutine(stunTime));
        }
    }

    private void UpdateHealthUI()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        if (currentHealth <= 0)
        {
            Die();
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
    public void SetCurrentHealth(int amount)
    {
        currentHealth = amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
    public int GetCurrentHealth() => currentHealth;
    public void SetMaxHealth(int amount)
    {
        maxHealth = amount;
        UpdateHealthUI();
    }
    public int GetMaxHealth() => maxHealth;
    public void SetInvincibility(bool condition) => isInvincible = condition;
    public Rigidbody GetPlayerRigidbody() => rb;
    public PlayerMovement GetPlayerMovement() => playerMovement;
    public List<(int, Vector3)> GetDamageQuene() => damageQueue;
    public Transform GetFirePoint() => firePoint;

    private System.Collections.IEnumerator InvincibilityCoroutine(float invincibilityDuration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}

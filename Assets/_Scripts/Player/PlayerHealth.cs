using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 120;
    public int currentHealth;
    public float healthRegenRate = 2f; // Health per second
    public float healthRegenDelay = 3f; // Delay before regen starts after taking damage
    
    [Header("Death Settings")]
    public float deathDelay = 2f;
    public GameObject deathEffect;
    public AudioClip deathSound;
    
    [Header("Damage Effects")]
    public GameObject damageEffect;
    public AudioClip damageSound;
    public float invulnerabilityTime = 0.5f;
    
    // Private variables
    private PlayerStats stats;
    private PlayerStateMachine stateMachine;
    private AudioSource audioSource;
    private float lastDamageTime;
    private bool isInvulnerable = false;
    private bool isDead = false;
    
    // Events
    public System.Action<int, int> OnHealthChanged; // current, max
    public System.Action OnPlayerDeath;
    public System.Action<float> OnDamageTaken; // damage amount
    
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        audioSource = GetComponent<AudioSource>();
        
        // Initialize health from stats if available
        if (stats.maxHp > 0)
        {
            maxHealth = stats.maxHp;
            currentHealth = stats.hp;
        }
        else
        {
            currentHealth = maxHealth;
        }
        
        // Start health regeneration coroutine
        StartCoroutine(HealthRegeneration());
    }
    
    void Update()
    {
        // Update invulnerability state
        if (isInvulnerable && Time.time >= lastDamageTime + invulnerabilityTime)
        {
            isInvulnerable = false;
        }
    }
    
    public void TakeDamage(int damage, DamageType damageType = DamageType.Physical)
    {
        if (isDead || isInvulnerable) return;
        
        // Calculate actual damage after armor
        int actualDamage = CalculateDamage(damage, damageType);
        
        currentHealth -= actualDamage;
        lastDamageTime = Time.time;
        isInvulnerable = true;
        
        // Update stats
        if (stats.maxHp > 0)
        {
            stats.hp = currentHealth;
        }
        
        // Trigger events
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke(actualDamage);
        
        // Play damage effects
        if (damageSound && audioSource)
            audioSource.PlayOneShot(damageSound);
            
        if (damageEffect)
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        
        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    int CalculateDamage(int baseDamage, DamageType damageType)
    {
        int armor = 0;
        
        switch (damageType)
        {
            case DamageType.Physical:
                armor = stats.physicalArmor;
                break;
            case DamageType.Magical:
                armor = stats.magicalArmor;
                break;
        }
        
        // Simple armor calculation: reduce damage by armor value
        int finalDamage = Mathf.Max(1, baseDamage - armor);
        
        // Check for dodge
        if (Random.Range(0f, 1f) < stats.dodgeChance)
        {
            finalDamage = 0;
            Debug.Log("Player dodged the attack!");
        }
        
        return finalDamage;
    }
    
    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        
        // Update stats
        if (stats.maxHp > 0)
        {
            stats.hp = currentHealth;
        }
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        // Update stats
        if (stats.maxHp > 0)
        {
            stats.maxHp = maxHealth;
            stats.hp = currentHealth;
        }
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        currentHealth = 0;
        stateMachine.TryTransitionToDead();
        
        // Play death effects
        if (deathSound && audioSource)
            audioSource.PlayOneShot(deathSound);
            
        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        
        // Trigger death event
        OnPlayerDeath?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Disable player controls
        StartCoroutine(HandleDeath());
    }
    
    IEnumerator HandleDeath()
    {
        // Disable movement and attack
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        
        yield return new WaitForSeconds(deathDelay);
        
        // Here you could respawn the player or trigger game over
        // For now, we'll just log the death
        Debug.Log("Player has died!");
        
        // You might want to call a GameManager to handle respawn or game over
        // GameManager.Instance.PlayerDied();
    }
    
    IEnumerator HealthRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            // Only regenerate if not dead, not at full health, and enough time has passed since last damage
            if (!isDead && currentHealth < maxHealth && Time.time >= lastDamageTime + healthRegenDelay)
            {
                int regenAmount = Mathf.RoundToInt(stats.hpRegen);
                if (regenAmount > 0)
                {
                    Heal(regenAmount);
                }
            }
        }
    }
    
    // Public getters
    public bool IsDead() => isDead;
    public bool IsInvulnerable() => isInvulnerable;
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    
    // Method to revive player (useful for respawn system)
    public void Revive(int healthAmount = -1)
    {
        isDead = false;
        isInvulnerable = false;
        
        if (healthAmount == -1)
            healthAmount = maxHealth;
            
        currentHealth = Mathf.Min(maxHealth, healthAmount);
        
        // Update stats
        if (stats.maxHp > 0)
        {
            stats.hp = currentHealth;
        }
        
        // State will be managed by the state machine
        
        // Re-enable player controls
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}

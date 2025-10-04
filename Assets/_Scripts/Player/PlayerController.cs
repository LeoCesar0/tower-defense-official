using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public PlayerClass playerClass = PlayerClass.Knight;
    
    [Header("Experience Settings")]
    public int experienceGainRate = 10;
    
    // Component references
    private PlayerStats stats;
    private PlayerStateController stateController;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;
    
    // Events
    public System.Action<PlayerStats> OnStatsChanged;
    public System.Action<int> OnLevelUp;
    public System.Action<int> OnExperienceGained;
    
    void Start()
    {
        InitializePlayer();
        SetupEventListeners();
    }
    
    void InitializePlayer()
    {
        // Get or add required components
        stateController = GetComponent<PlayerStateController>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        
        // Initialize stats based on class
        stats = InitializeStatsForClass(playerClass);
        
        // Apply stats to components
        ApplyStatsToComponents();
        
        // Notify about initial stats
        OnStatsChanged?.Invoke(stats);
    }
    
    PlayerStats InitializeStatsForClass(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Knight:
                return PlayerStats.CreateKnightStats();
            default:
                return PlayerStats.CreateKnightStats();
        }
    }
    
    void ApplyStatsToComponents()
    {
        // Apply movement stats
        if (movement)
        {
            movement.moveSpeed = stats.moveSpeed;
            movement.jumpForce = stats.jumpForce;
            movement.dashSpeed = stats.dashSpeed;
        }
        
        // Apply health stats
        if (health)
        {
            health.maxHealth = stats.maxHp;
            health.currentHealth = stats.hp;
            health.healthRegenRate = stats.hpRegen;
        }
        
    }
    
    void SetupEventListeners()
    {
        // Listen to health changes
        if (health)
        {
            health.OnHealthChanged += OnHealthChanged;
            health.OnPlayerDeath += OnPlayerDeath;
        }
        
    }
    
    void Update()
    {
        HandleInput();
        UpdateStats();
    }
    
    void HandleInput()
    {
        // Level up test (remove in production)
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExperience(100);
        }
        
        // Heal test (remove in production)
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (health)
                health.Heal(20);
        }
        
    }
    
    void UpdateStats()
    {
        // Sync stats with components
        if (health)
        {
            stats.hp = health.GetCurrentHealth();
            stats.maxHp = health.GetMaxHealth();
        }
        
    }
    
    // Experience and Leveling
    public void GainExperience(int amount)
    {
        stats.experience += amount;
        OnExperienceGained?.Invoke(amount);
        
        // Check for level up
        while (stats.experience >= stats.experienceToNextLevel)
        {
            LevelUp();
        }
        
        OnStatsChanged?.Invoke(stats);
    }
    
    void LevelUp()
    {
        stats = stats.LevelUp();
        OnLevelUp?.Invoke(stats.level);
        
        // Apply new stats to components
        ApplyStatsToComponents();
        
        // Full heal on level up
        if (health)
        {
            health.Heal(health.GetMaxHealth());
        }
        
        
        OnStatsChanged?.Invoke(stats);
        
        Debug.Log($"Level Up! Now level {stats.level}");
    }
    
    // Event handlers
    void OnHealthChanged(int current, int max)
    {
        stats.hp = current;
        stats.maxHp = max;
        OnStatsChanged?.Invoke(stats);
    }
    
    
    void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
        // Handle player death - could trigger respawn, game over, etc.
    }
    
    // Public getters
    public PlayerStats GetStats() => stats;
    public PlayerClass GetPlayerClass() => playerClass;
    public int GetLevel() => stats.level;
    public int GetExperience() => stats.experience;
    public int GetExperienceToNextLevel() => stats.experienceToNextLevel;
    
    // Public setters for external systems
    public void SetPlayerClass(PlayerClass newClass)
    {
        playerClass = newClass;
        stats = InitializeStatsForClass(playerClass);
        ApplyStatsToComponents();
        OnStatsChanged?.Invoke(stats);
    }
    
    public void ModifyStat(string statName, int value)
    {
        // This could be expanded to modify specific stats
        // For now, we'll just trigger a stats update
        OnStatsChanged?.Invoke(stats);
    }
    
    void OnDestroy()
    {
        // Clean up event listeners
        if (health)
        {
            health.OnHealthChanged -= OnHealthChanged;
            health.OnPlayerDeath -= OnPlayerDeath;
        }
        
    }
}

// Player class enum
public enum PlayerClass
{
    Knight,
    Mage,
    Archer,
    Rogue
}

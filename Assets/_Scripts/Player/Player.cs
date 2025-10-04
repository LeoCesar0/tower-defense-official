using System.Collections;
using UnityEngine;

/// <summary>
/// Main Player script that automatically manages all player components.
/// Just add this one script to your player GameObject and everything will be set up automatically!
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    public PlayerClass playerClass = PlayerClass.Knight;
    public int experienceGainRate = 10;
    
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;
    public float comboWindow = 0.3f;
    
    [Header("Health Settings")]
    public int maxHealth = 120;
    public float healthRegenRate = 2f;
    public float healthRegenDelay = 3f;
    public float invulnerabilityTime = 0.5f;
    
    [Header("Knight Abilities")]
    public float shieldBashRange = 2f;
    public float shieldBashDamage = 40f;
    public float shieldBashCooldown = 8f;
    public float defensiveStanceDuration = 5f;
    public float defensiveStanceCooldown = 15f;
    public float defensiveStanceArmorBonus = 10f;
    public float whirlwindRange = 3f;
    public float whirlwindDamage = 30f;
    public float whirlwindCooldown = 12f;
    
    [Header("Effects")]
    public GameObject attackEffect;
    public GameObject damageEffect;
    public GameObject deathEffect;
    public GameObject shieldBashEffect;
    public GameObject defensiveStanceEffect;
    public GameObject whirlwindEffect;
    
    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip shieldBashSound;
    public AudioClip defensiveStanceSound;
    public AudioClip whirlwindSound;
    
    // Component references (automatically managed)
    private PlayerStats stats;
    private PlayerStateMachine stateMachine;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;
    private KnightAbilities abilities;
    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    
    // Events
    public System.Action<PlayerStats> OnStatsChanged;
    public System.Action<int> OnLevelUp;
    public System.Action<int> OnExperienceGained;
    public System.Action<int, int> OnHealthChanged;
    public System.Action OnPlayerDeath;
    
    void Awake()
    {
        // Automatically add required components if they don't exist
        SetupComponents();
        
        // Initialize player
        InitializePlayer();
    }
    
    void Start()
    {
        // Setup event listeners
        SetupEventListeners();
    }
    
    void Update()
    {
        HandleInput();
        UpdateStats();
    }
    
    void SetupComponents()
    {
        // Get or add Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
            Debug.LogWarning("No Animator found. Added one automatically. Please assign an Animator Controller.");
        }
        
        // Get or add Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true; // Prevent rotation
            rb.gravityScale = 3f; // Adjust gravity as needed
        }
        
        // Get or add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Get or add PlayerStateMachine
        stateMachine = GetComponent<PlayerStateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<PlayerStateMachine>();
        }
        
        // Get or add PlayerMovement
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            movement = gameObject.AddComponent<PlayerMovement>();
        }
        
        // Get or add PlayerAttack
        attack = GetComponent<PlayerAttack>();
        if (attack == null)
        {
            attack = gameObject.AddComponent<PlayerAttack>();
        }
        
        // Get or add PlayerHealth
        health = GetComponent<PlayerHealth>();
        if (health == null)
        {
            health = gameObject.AddComponent<PlayerHealth>();
        }
        
        // Get or add KnightAbilities (only for Knight class)
        if (playerClass == PlayerClass.Knight)
        {
            abilities = GetComponent<KnightAbilities>();
            if (abilities == null)
            {
                abilities = gameObject.AddComponent<KnightAbilities>();
            }
        }
        
        // Configure components with our settings
        ConfigureComponents();
    }
    
    void ConfigureComponents()
    {
        // Configure PlayerMovement
        movement.moveSpeed = moveSpeed;
        movement.jumpForce = jumpForce;
        movement.dashSpeed = dashSpeed;
        movement.dashDuration = dashDuration;
        
        // Configure PlayerAttack
        attack.attackPoint = attackPoint != null ? attackPoint : transform;
        attack.attackRange = attackRange;
        attack.enemyLayers = enemyLayers;
        attack.attackCooldown = attackCooldown;
        attack.comboWindow = comboWindow;
        attack.attackEffect = attackEffect;
        attack.attackSound = attackSound;
        
        // Configure PlayerHealth
        health.maxHealth = maxHealth;
        health.healthRegenRate = healthRegenRate;
        health.healthRegenDelay = healthRegenDelay;
        health.invulnerabilityTime = invulnerabilityTime;
        health.damageEffect = damageEffect;
        health.deathEffect = deathEffect;
        health.damageSound = damageSound;
        health.deathSound = deathSound;
        
        // Configure KnightAbilities
        if (abilities != null)
        {
            abilities.shieldBashRange = shieldBashRange;
            abilities.shieldBashDamage = shieldBashDamage;
            abilities.shieldBashCooldown = shieldBashCooldown;
            abilities.defensiveStanceDuration = defensiveStanceDuration;
            abilities.defensiveStanceCooldown = defensiveStanceCooldown;
            abilities.defensiveStanceArmorBonus = defensiveStanceArmorBonus;
            abilities.whirlwindRange = whirlwindRange;
            abilities.whirlwindDamage = whirlwindDamage;
            abilities.whirlwindCooldown = whirlwindCooldown;
            abilities.shieldBashEffect = shieldBashEffect;
            abilities.defensiveStanceEffect = defensiveStanceEffect;
            abilities.whirlwindEffect = whirlwindEffect;
            abilities.shieldBashSound = shieldBashSound;
            abilities.defensiveStanceSound = defensiveStanceSound;
            abilities.whirlwindSound = whirlwindSound;
        }
    }
    
    void InitializePlayer()
    {
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
        
        // Listen to state machine changes
        if (stateMachine)
        {
            stateMachine.OnStateChanged += OnStateChanged;
        }
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
    
    void OnStateChanged(PlayerState newState)
    {
        // You can add logic here to respond to state changes
        // For example, update UI, play sounds, etc.
    }
    
    // Public getters
    public PlayerStats GetStats() => stats;
    public PlayerClass GetPlayerClass() => playerClass;
    public int GetLevel() => stats.level;
    public int GetExperience() => stats.experience;
    public int GetExperienceToNextLevel() => stats.experienceToNextLevel;
    public PlayerState GetCurrentState() => stateMachine?.GetCurrentState();
    
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
        // For example, for equipment bonuses
        OnStatsChanged?.Invoke(stats);
    }
    
    // Method to apply equipment bonuses
    public void ApplyEquipmentBonus(float attackDamageBonus, float magicDamageBonus, 
                                  float attackSpeedBonus, int physicalArmorBonus, 
                                  int magicArmorBonus, float criticalChanceBonus)
    {
        stats = stats.ApplyEquipmentBonus(attackDamageBonus, magicDamageBonus, 
                                        attackSpeedBonus, physicalArmorBonus, 
                                        magicArmorBonus, criticalChanceBonus);
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
        
        if (stateMachine)
        {
            stateMachine.OnStateChanged -= OnStateChanged;
        }
    }
    
    // Visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
        // Draw ability ranges if Knight
        if (playerClass == PlayerClass.Knight)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, shieldBashRange);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, whirlwindRange);
        }
    }
}

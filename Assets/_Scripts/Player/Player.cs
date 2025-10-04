using UnityEngine;

/// <summary>
/// Simplified Player script using the new data architecture.
/// Just add this one script to your player GameObject and assign a CharacterPreset!
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    [Tooltip("Assign a CharacterPreset to configure the player")]
    public CharacterPreset playerPreset;
    
    // Component references
    private PlayerCharacter playerCharacter;
    private CharacterStateMachine stateMachine;
    
    // Events (for UI integration)
    public System.Action<CharacterStats> OnStatsChanged;
    public System.Action<int> OnLevelUp;
    public System.Action<int, int> OnHealthChanged;
    public System.Action OnPlayerDeath;
    public System.Action<CharacterState> OnStateChanged;
    
    void Awake()
    {
        SetupComponents();
    }
    
    void Start()
    {
        InitializePlayer();
        SetupEventListeners();
    }
    
    void SetupComponents()
    {
        // Get or add PlayerCharacter (the main character component)
        playerCharacter = GetComponent<PlayerCharacter>();
        if (playerCharacter == null)
        {
            playerCharacter = gameObject.AddComponent<PlayerCharacter>();
        }
        
        // Get or add CharacterStateMachine
        stateMachine = GetComponent<CharacterStateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<CharacterStateMachine>();
        }
        
        // Ensure we have required components
        if (GetComponent<Animator>() == null)
        {
            gameObject.AddComponent<Animator>();
            Debug.LogWarning("No Animator found. Added one automatically. Please assign an Animator Controller.");
        }
        
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.gravityScale = 3f;
        }
        
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }
    
    void InitializePlayer()
    {
        if (playerPreset != null)
        {
            playerCharacter.InitializeFromPreset(playerPreset);
            Debug.Log($"Player initialized with preset: {playerPreset.presetName}");
        }
        else
        {
            Debug.LogError("No player preset assigned! Please assign a CharacterPreset in the inspector.");
        }
    }
    
    void SetupEventListeners()
    {
        if (playerCharacter)
        {
            playerCharacter.OnHealthChanged += HandleHealthChanged;
            playerCharacter.OnCharacterDeath += HandlePlayerDeath;
            playerCharacter.OnStatsChanged += OnStatsChanged;
            playerCharacter.OnStateChanged += HandleStateChanged;
        }
    }
    
    // Public getters for external systems
    public CharacterStats GetStats() => playerCharacter?.GetStats() ?? new CharacterStats();
    public CharacterClass GetPlayerClass() => playerCharacter?.GetCharacterClass() ?? CharacterClass.Knight;
    public int GetLevel() => playerCharacter?.GetLevel() ?? 1;
    public int GetExperience() => playerCharacter?.GetExperience() ?? 0;
    public int GetExperienceToNextLevel() => playerCharacter?.GetExperienceToNextLevel() ?? 100;
    public CharacterState GetCurrentState() => stateMachine?.GetCurrentState() ?? CharacterState.Idle;
    public CharacterPreset GetPreset() => playerPreset;
    
    // Public methods for external systems
    public void SetPlayerPreset(CharacterPreset newPreset)
    {
        playerPreset = newPreset;
        if (playerCharacter && newPreset != null)
        {
            playerCharacter.InitializeFromPreset(newPreset);
        }
    }
    
    public void ApplyEquipmentBonus(EquipmentBonus bonus)
    {
        if (playerCharacter)
        {
            playerCharacter.ApplyEquipmentBonus(bonus);
        }
    }
    
    public void GainExperience(int amount)
    {
        if (playerCharacter)
        {
            playerCharacter.GainExperience(amount);
        }
    }
    
    public void Heal(int amount)
    {
        if (playerCharacter)
        {
            playerCharacter.Heal(amount);
        }
    }
    
    // Event handlers
    void HandleHealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }
    
    void HandlePlayerDeath()
    {
        Debug.Log("Player has died!");
        OnPlayerDeath?.Invoke();
    }
    
    void HandleStateChanged(CharacterState newState)
    {
        OnStateChanged?.Invoke(newState);
    }
    
    void OnDestroy()
    {
        // Clean up event listeners
        if (playerCharacter)
        {
            playerCharacter.OnHealthChanged -= HandleHealthChanged;
            playerCharacter.OnCharacterDeath -= HandlePlayerDeath;
            playerCharacter.OnStatsChanged -= OnStatsChanged;
            playerCharacter.OnStateChanged -= HandleStateChanged;
        }
    }
    
    // Visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        if (playerCharacter && playerCharacter.GetCharacterData())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerCharacter.GetCharacterData().attackRange);
        }
    }
}

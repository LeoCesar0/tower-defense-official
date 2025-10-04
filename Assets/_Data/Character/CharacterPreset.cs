using UnityEngine;

/// <summary>
/// ScriptableObject that defines a complete character preset with all data
/// </summary>
[CreateAssetMenu(fileName = "New Character Preset", menuName = "Character/Character Preset")]
public class CharacterPreset : ScriptableObject
{
    [Header("Character Info")]
    public string presetName;
    public string description;
    public CharacterType characterType;
    public CharacterClass characterClass;
    
    [Header("Character Data")]
    public CharacterData characterData;
    
    [Header("Abilities")]
    public AbilityData[] abilities;
    
    [Header("Starting Equipment")]
    public EquipmentData[] startingEquipment;
    
    [Header("AI Settings (for NPCs/Enemies)")]
    public AIData aiData;
    
    [Header("Prefab Reference")]
    public GameObject characterPrefab;
    
    /// <summary>
    /// Create a character instance from this preset
    /// </summary>
    public GameObject CreateCharacterInstance(Transform parent = null)
    {
        if (characterPrefab == null)
        {
            Debug.LogError($"Character prefab is null for preset: {presetName}");
            return null;
        }
        
        GameObject instance = Instantiate(characterPrefab, parent);
        
        // Apply character data
        var characterComponent = instance.GetComponent<ICharacter>();
        if (characterComponent != null)
        {
            characterComponent.InitializeFromPreset(this);
        }
        
        return instance;
    }
    
    /// <summary>
    /// Validate that this preset has all required data
    /// </summary>
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(presetName))
        {
            Debug.LogError("Character preset name is empty");
            return false;
        }
        
        if (characterData == null)
        {
            Debug.LogError($"Character data is null for preset: {presetName}");
            return false;
        }
        
        if (characterPrefab == null)
        {
            Debug.LogError($"Character prefab is null for preset: {presetName}");
            return false;
        }
        
        return true;
    }
}

/// <summary>
/// AI data for NPCs and enemies
/// </summary>
[System.Serializable]
public struct AIData
{
    [Header("Behavior")]
    public AIBehaviorType behaviorType;
    public float detectionRange;
    public float attackRange;
    public float moveSpeed;
    
    [Header("Combat")]
    public bool canAttack;
    public float attackCooldown;
    public bool canUseAbilities;
    public int[] abilityIndices;
    
    [Header("Movement")]
    public bool canPatrol;
    public Transform[] patrolPoints;
    public float patrolWaitTime;
    public bool canChase;
    public float chaseRange;
    
    [Header("Aggro")]
    public bool isAggressive;
    public float aggroRange;
    public float aggroDuration;
    public bool canLoseAggro;
}

/// <summary>
/// AI behavior types
/// </summary>
public enum AIBehaviorType
{
    Passive,
    Aggressive,
    Defensive,
    Patrol,
    Guard,
    Boss
}

/// <summary>
/// Main character interface that combines all character behaviors
/// </summary>
public interface ICharacter : IDamageable, IAttacker, IMovable, IAbilityUser, IStatHolder, ITargetable, IBuffable, IEquippable, ILevelable, IStateful, IAnimatable
{
    void InitializeFromPreset(CharacterPreset preset);
    CharacterPreset GetPreset();
    CharacterData GetCharacterData();
    void UpdateCharacter();
}

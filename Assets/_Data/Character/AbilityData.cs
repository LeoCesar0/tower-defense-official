using UnityEngine;

/// <summary>
/// ScriptableObject for ability data that can be reused across characters
/// </summary>
[CreateAssetMenu(fileName = "New Ability Data", menuName = "Character/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("Basic Info")]
    public string abilityName;
    public string description;
    public AbilityType abilityType;
    public CharacterClass requiredClass;
    
    [Header("Ability Settings")]
    public float cooldown = 5f;
    public float duration = 0f; // 0 = instant
    public float range = 1f;
    public float damage = 0f;
    public DamageType damageType = DamageType.Physical;
    public int manaCost = 0;
    
    [Header("Effects")]
    public GameObject abilityEffect;
    public AudioClip abilitySound;
    public float effectDuration = 1f;
    
    [Header("Animation")]
    public string animationTrigger;
    public float animationDuration = 0.5f;
    
    [Header("Special Properties")]
    public bool hasKnockback = false;
    public float knockbackForce = 0f;
    public bool isAreaOfEffect = false;
    public float aoeRadius = 0f;
    public bool appliesBuff = false;
    public BuffData buffData;
    
    /// <summary>
    /// Check if a character can use this ability
    /// </summary>
    public bool CanCharacterUse(CharacterClass characterClass, int currentMana)
    {
        if (requiredClass != CharacterClass.None && requiredClass != characterClass)
            return false;
            
        if (manaCost > currentMana)
            return false;
            
        return true;
    }
}

/// <summary>
/// Types of abilities
/// </summary>
public enum AbilityType
{
    Attack,
    Buff,
    Debuff,
    Movement,
    Defensive,
    Ultimate
}

/// <summary>
/// ScriptableObject for buff/debuff data
/// </summary>
[CreateAssetMenu(fileName = "New Buff Data", menuName = "Character/Buff Data")]
public class BuffData : ScriptableObject
{
    [Header("Basic Info")]
    public string buffName;
    public string description;
    public BuffType buffType;
    
    [Header("Buff Settings")]
    public float duration = 5f;
    public bool isStackable = false;
    public int maxStacks = 1;
    
    [Header("Stat Modifications")]
    public float attackDamageModifier = 0f;
    public float magicDamageModifier = 0f;
    public float attackSpeedModifier = 0f;
    public int physicalArmorModifier = 0;
    public int magicArmorModifier = 0;
    public float moveSpeedModifier = 0f;
    public float criticalChanceModifier = 0f;
    
    [Header("Effects")]
    public GameObject buffEffect;
    public AudioClip buffSound;
    public Color buffColor = Color.white;
    
    /// <summary>
    /// Apply this buff to character stats
    /// </summary>
    public CharacterStats ApplyBuff(CharacterStats baseStats, int stackCount = 1)
    {
        var modifiedStats = baseStats;
        float multiplier = isStackable ? stackCount : 1f;
        
        modifiedStats.attackDamage += attackDamageModifier * multiplier;
        modifiedStats.magicDamage += magicDamageModifier * multiplier;
        modifiedStats.attackSpeed += attackSpeedModifier * multiplier;
        modifiedStats.physicalArmor += Mathf.RoundToInt(physicalArmorModifier * multiplier);
        modifiedStats.magicArmor += Mathf.RoundToInt(magicArmorModifier * multiplier);
        modifiedStats.moveSpeed += moveSpeedModifier * multiplier;
        modifiedStats.criticalChance += criticalChanceModifier * multiplier;
        
        return modifiedStats;
    }
}

/// <summary>
/// Types of buffs
/// </summary>
public enum BuffType
{
    Buff,
    Debuff,
    Temporary
}

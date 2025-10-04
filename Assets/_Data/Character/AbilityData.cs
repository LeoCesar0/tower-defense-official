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
    
    [Header("Buff/Debuff")]
    public bool appliesBuff = false;
    public BuffData buffData;
    
    [Header("Targeting")]
    public bool requiresTarget = false;
    public bool isAreaOfEffect = false;
    public float aoeRadius = 0f;
    
    /// <summary>
    /// Check if this ability can be used by the given character
    /// </summary>
    public bool CanBeUsedBy(CharacterClass characterClass)
    {
        return requiredClass == CharacterClass.None || requiredClass == characterClass;
    }
    
    /// <summary>
    /// Get the effective damage for this ability
    /// </summary>
    public float GetEffectiveDamage(float characterDamage)
    {
        return damage > 0 ? damage : characterDamage;
    }
}
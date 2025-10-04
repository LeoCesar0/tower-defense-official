using UnityEngine;

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

using UnityEngine;

/// <summary>
/// ScriptableObject for equipment data (weapons, armor, accessories)
/// </summary>
[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Character/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    [Header("Basic Info")]
    public string equipmentName;
    public string description;
    public EquipmentType equipmentType;
    public EquipmentSlot equipmentSlot;
    public CharacterClass requiredClass = CharacterClass.None;
    public int requiredLevel = 1;
    
    [Header("Visual")]
    public Sprite equipmentIcon;
    public GameObject equipmentModel;
    public RuntimeAnimatorController animatorOverride;
    
    [Header("Stat Bonuses")]
    public EquipmentBonus statBonuses;
    
    [Header("Special Properties")]
    public AbilityData[] grantedAbilities;
    public BuffData[] passiveBuffs;
    
    [Header("Audio")]
    public AudioClip equipSound;
    public AudioClip unequipSound;
    
    [Header("Economy")]
    public int goldCost = 100;
    public bool isSellable = true;
    public int sellValue = 50;
    
    /// <summary>
    /// Check if this equipment can be used by the given character
    /// </summary>
    public bool CanBeUsedBy(CharacterClass characterClass, int characterLevel)
    {
        if (requiredClass != CharacterClass.None && requiredClass != characterClass)
            return false;
        
        if (characterLevel < requiredLevel)
            return false;
        
        return true;
    }
    
    /// <summary>
    /// Get the effective sell value
    /// </summary>
    public int GetSellValue()
    {
        return isSellable ? sellValue : 0;
    }
}
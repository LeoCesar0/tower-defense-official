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
    
    /// <summary>
    /// Check if a character can equip this item
    /// </summary>
    public bool CanCharacterEquip(CharacterClass characterClass, int characterLevel)
    {
        if (requiredClass != CharacterClass.None && requiredClass != characterClass)
            return false;
            
        if (requiredLevel > characterLevel)
            return false;
            
        return true;
    }
}

/// <summary>
/// Types of equipment
/// </summary>
public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Consumable
}

/// <summary>
/// Equipment slots
/// </summary>
public enum EquipmentSlot
{
    Weapon,
    Helmet,
    Chest,
    Legs,
    Boots,
    Gloves,
    Ring,
    Necklace,
    Consumable
}

/// <summary>
/// Equipment set data for set bonuses
/// </summary>
[CreateAssetMenu(fileName = "New Equipment Set", menuName = "Character/Equipment Set")]
public class EquipmentSetData : ScriptableObject
{
    [Header("Set Info")]
    public string setName;
    public string description;
    
    [Header("Set Pieces")]
    public EquipmentData[] setPieces;
    
    [Header("Set Bonuses")]
    public EquipmentBonus[] setBonuses; // Index corresponds to number of pieces equipped
    
    /// <summary>
    /// Get the bonus for having a certain number of set pieces equipped
    /// </summary>
    public EquipmentBonus GetSetBonus(int equippedPieces)
    {
        if (equippedPieces <= 0 || equippedPieces > setBonuses.Length)
            return new EquipmentBonus();
            
        return setBonuses[equippedPieces - 1];
    }
}

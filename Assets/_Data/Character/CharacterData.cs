using UnityEngine;

/// <summary>
/// Base ScriptableObject for all character data (Player, Enemy, NPC)
/// </summary>
[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public CharacterType characterType;
    public CharacterClass characterClass;
    
    [Header("Core Stats")]
    public int level = 1;
    public int maxHealth = 100;
    public int maxMana = 50;
    
    [Header("Combat Stats")]
    public float attackDamage = 20f;
    public float magicDamage = 10f;
    public float attackSpeed = 1.0f;
    public float attackRange = 1.5f;
    public float criticalChance = 0.05f;
    public float criticalMultiplier = 1.5f;
    
    [Header("Defense Stats")]
    public int physicalArmor = 5;
    public int magicArmor = 3;
    public float dodgeChance = 0.02f;
    public float blockChance = 0.1f;
    
    [Header("Movement Stats")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float dashSpeed = 10f;
    public float dashCooldown = 2f;
    
    [Header("Regeneration")]
    public float healthRegen = 1f;
    public float manaRegen = 0.5f;
    public float regenDelay = 2f;
    
    [Header("Visual & Audio")]
    public Sprite characterSprite;
    public RuntimeAnimatorController animatorController;
    public AudioClip[] footstepSounds;
    public AudioClip[] attackSounds;
    public AudioClip[] damageSounds;
    public AudioClip[] deathSounds;
    
    [Header("Effects")]
    public GameObject[] attackEffects;
    public GameObject[] damageEffects;
    public GameObject[] deathEffects;
    
    /// <summary>
    /// Create a CharacterStats struct from this data
    /// </summary>
    public CharacterStats ToCharacterStats()
    {
        return new CharacterStats
        {
            level = this.level,
            maxHp = this.maxHealth,
            hp = this.maxHealth,
            maxMana = this.maxMana,
            mana = this.maxMana,
            attackDamage = this.attackDamage,
            magicDamage = this.magicDamage,
            attackSpeed = this.attackSpeed,
            attackRange = this.attackRange,
            criticalChance = this.criticalChance,
            criticalMultiplier = this.criticalMultiplier,
            physicalArmor = this.physicalArmor,
            magicArmor = this.magicArmor,
            dodgeChance = this.dodgeChance,
            blockChance = this.blockChance,
            moveSpeed = this.moveSpeed,
            jumpForce = this.jumpForce,
            dashSpeed = this.dashSpeed,
            dashCooldown = this.dashCooldown,
            hpRegen = this.healthRegen,
            manaRegen = this.manaRegen
        };
    }
}

/// <summary>
/// Character types for different categories
/// </summary>
public enum CharacterType
{
    Player,
    Enemy,
    NPC,
    Boss
}

/// <summary>
/// Character classes for different specializations
/// </summary>
public enum CharacterClass
{
    Knight,
    Mage,
    Archer,
    Rogue,
    Warrior,
    Assassin,
    Paladin,
    Necromancer
}

/// <summary>
/// Runtime character stats (used during gameplay)
/// </summary>
[System.Serializable]
public struct CharacterStats
{
    public int level;
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public float attackDamage;
    public float magicDamage;
    public float attackSpeed;
    public float attackRange;
    public float criticalChance;
    public float criticalMultiplier;
    public int physicalArmor;
    public int magicArmor;
    public float dodgeChance;
    public float blockChance;
    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashCooldown;
    public float hpRegen;
    public float manaRegen;
    
    /// <summary>
    /// Apply equipment bonuses to stats
    /// </summary>
    public CharacterStats ApplyEquipmentBonus(EquipmentBonus bonus)
    {
        var newStats = this;
        newStats.attackDamage += bonus.attackDamageBonus;
        newStats.magicDamage += bonus.magicDamageBonus;
        newStats.attackSpeed += bonus.attackSpeedBonus;
        newStats.physicalArmor += bonus.physicalArmorBonus;
        newStats.magicArmor += bonus.magicArmorBonus;
        newStats.criticalChance += bonus.criticalChanceBonus;
        newStats.criticalChance = Mathf.Clamp(newStats.criticalChance, 0f, 0.5f);
        return newStats;
    }
    
    /// <summary>
    /// Level up the character
    /// </summary>
    public CharacterStats LevelUp()
    {
        var newStats = this;
        newStats.level++;
        newStats.attackDamage += 3f;
        newStats.magicDamage += 1f;
        newStats.attackSpeed += 0.05f;
        newStats.physicalArmor += 1;
        newStats.magicArmor += 1;
        newStats.criticalChance += 0.01f;
        newStats.maxHp += 15;
        newStats.hp = newStats.maxHp; // Full heal on level up
        newStats.maxMana += 5;
        newStats.mana = newStats.maxMana;
        return newStats;
    }
}

/// <summary>
/// Equipment bonus data
/// </summary>
[System.Serializable]
public struct EquipmentBonus
{
    public float attackDamageBonus;
    public float magicDamageBonus;
    public float attackSpeedBonus;
    public int physicalArmorBonus;
    public int magicArmorBonus;
    public float criticalChanceBonus;
}

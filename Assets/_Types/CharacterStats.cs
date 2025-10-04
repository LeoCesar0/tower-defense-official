using UnityEngine;

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

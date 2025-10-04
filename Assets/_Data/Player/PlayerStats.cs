
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    // Core Stats
    public int level;
    public int experience;
    public int experienceToNextLevel;
    
    // Health
    public int hp;
    public int maxHp;
    public int hpRegen;
    
    // Combat Stats (as specified in requirements)
    public float attackDamage;        // Physical damage
    public float magicDamage;         // Magic damage
    public float attackSpeed;
    public int physicalArmor;
    public int magicArmor;
    public float criticalChance;
    public float criticalMultiplier;
    
    // Additional Combat Stats
    public float attackRange;
    public float dodgeChance;
    public float blockChance;
    
    // Movement Stats
    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashCooldown;
    
    // Constructor for Knight class
    public static PlayerStats CreateKnightStats()
    {
        return new PlayerStats
        {
            level = 1,
            experience = 0,
            experienceToNextLevel = 100,
            
            hp = 120,
            maxHp = 120,
            hpRegen = 2,
            
            attackDamage = 25f,
            magicDamage = 5f,
            attackSpeed = 1.2f,
            attackRange = 1.5f,
            criticalChance = 0.05f,
            criticalMultiplier = 1.5f,
            
            physicalArmor = 8,
            magicArmor = 3,
            dodgeChance = 0.02f,
            blockChance = 0.1f,
            
            moveSpeed = 8f,
            jumpForce = 5f,
            dashSpeed = 12f,
            dashCooldown = 2f
        };
    }
    
    // Method to level up
    public PlayerStats LevelUp()
    {
        var newStats = this;
        newStats.level++;
        newStats.experience -= experienceToNextLevel;
        newStats.experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f);
        
        // Stat increases per level (simplified)
        newStats.attackDamage += 3f;
        newStats.magicDamage += 1f;
        newStats.attackSpeed += 0.05f;
        newStats.physicalArmor += 1;
        newStats.magicArmor += 1;
        newStats.criticalChance += 0.01f;
        
        // Health increase
        newStats.maxHp += 15;
        newStats.hp = newStats.maxHp; // Full heal on level up
        
        return newStats;
    }
    
    // Method to apply weapon/item bonuses
    public PlayerStats ApplyEquipmentBonus(float attackDamageBonus, float magicDamageBonus, 
                                         float attackSpeedBonus, int physicalArmorBonus, 
                                         int magicArmorBonus, float criticalChanceBonus)
    {
        var newStats = this;
        newStats.attackDamage += attackDamageBonus;
        newStats.magicDamage += magicDamageBonus;
        newStats.attackSpeed += attackSpeedBonus;
        newStats.physicalArmor += physicalArmorBonus;
        newStats.magicArmor += magicArmorBonus;
        newStats.criticalChance += criticalChanceBonus;
        
        // Clamp critical chance to reasonable values
        newStats.criticalChance = Mathf.Clamp(newStats.criticalChance, 0f, 0.5f);
        
        return newStats;
    }
}

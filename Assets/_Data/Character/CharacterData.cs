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
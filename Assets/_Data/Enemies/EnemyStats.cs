using UnityEngine;

[System.Serializable]
public struct EnemyStats
{
    // Health
    public int hp;
    public int maxHp;

    // Movement
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float dashForce;

    // Defense
    public int armor;
    public int magicArmor;

    // Attack
    public float damage;
    public DamageType damageType;
    public float atkSpeed;
    public float attackRange;

    // Constructor to initialize all fields
    public EnemyStats(int hp, int maxHp, float walkSpeed, float runSpeed, float jumpForce, float dashForce, int armor, int magicArmor, float atkSpeed, float attackRange, float damage, DamageType damageType)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.jumpForce = jumpForce;
        this.dashForce = dashForce;
        this.armor = armor;
        this.magicArmor = magicArmor;
        this.atkSpeed = atkSpeed;
        this.attackRange = attackRange;
        this.damage = damage;
        this.damageType = damageType;
    }
}

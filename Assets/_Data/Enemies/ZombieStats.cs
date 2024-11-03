public static class ZombieStats
{
    public static EnemyStats Stats = new EnemyStats(
        hp: 100,
        maxHp: 100,
        walkSpeed: 1.5f,
        runSpeed: 2.5f,
        jumpForce: 3.0f,
        dashForce: 5.0f,
        armor: 8,
        magicArmor: 2,
        atkSpeed: 1.5f,
        attackRange: 1f,
        damage: 1f,
        damageType: DamageType.Physical
    );
}

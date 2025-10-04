using UnityEngine;

/// <summary>
/// Interface for any character that can attack
/// </summary>
public interface IAttacker
{
    void Attack(IDamageable target);
    void Attack(Vector3 position, float range);
    bool CanAttack();
    float GetAttackDamage();
    float GetAttackRange();
    float GetAttackSpeed();
}

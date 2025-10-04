/// <summary>
/// Interface for any character that can take damage
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage, DamageType damageType);
    void Heal(float amount);
    bool IsDead();
    float GetHealthPercentage();
    int GetCurrentHealth();
    int GetMaxHealth();
}

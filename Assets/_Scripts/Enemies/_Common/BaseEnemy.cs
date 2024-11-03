using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    // Reference to EnemyStats
    public EnemyStats stats;

    protected float lastAttackTime;

    protected virtual void Start()
    {
        lastAttackTime = 0;
        InitializeStats();
    }

    protected virtual void InitializeStats()
    {
        Debug.Log($"{gameObject.name}: Base InitializeStats ran. HP set to max ({stats.hp}).");
    }


    // Method for taking damage
    public virtual void TakeDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(damage - stats.armor, 0);
        stats.hp -= effectiveDamage;
        stats.hp = Mathf.Max(0, stats.hp);
        Debug.Log($"{gameObject.name} took {effectiveDamage} damage. Current HP: {stats.hp}");

        if (stats.hp <= 0)
        {
            Die();
        }
    }

    // Method for basic movement
    public virtual void Move(Vector3 direction)
    {
        Vector3 movement = direction.normalized * stats.walkSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    // Attack logic with attack speed check
    public virtual void Attack()
    {
        if (Time.time >= lastAttackTime + stats.atkSpeed)
        {
            Debug.Log($"{gameObject.name} attacks with speed {stats.atkSpeed}.");
            lastAttackTime = Time.time;
        }
    }

    // Method for death
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}

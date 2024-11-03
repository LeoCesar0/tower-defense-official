using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class EnemyAttack : MonoBehaviour
{
    private BaseEnemy baseEnemy; // Reference to the BaseEnemy component
    private Transform target; // Target to attack
    private float lastAttackTime = 0f; // Time of the last attack

    private void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>(); // Get the BaseEnemy component to access stats
    }

    private void Update()
    {
        if (target != null)
        {
            // Check if target is within attack range
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= baseEnemy.stats.attackRange && Time.time >= lastAttackTime + baseEnemy.stats.atkSpeed)
            {
                Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Set target when an object with the PlayerLike or Target tag enters the attack range
        if (other.CompareTag("PlayerLike") || other.CompareTag("Target"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove target when it leaves the attack range
        if (other.transform == target)
        {
            target = null;
        }
    }

    private void Attack()
    {
        // Attack logic (e.g., deal damage to target)
        Debug.Log($"{gameObject.name} attacks {target.name} for {baseEnemy.stats} damage.");
        lastAttackTime = Time.time; // Update last attack time
    }
}

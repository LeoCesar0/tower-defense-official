using UnityEngine;

[RequireComponent(typeof(EnemyCharacter))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float stopDistance = 1.5f;
    
    private EnemyCharacter enemyCharacter;
    private ITargetable currentTarget;
    private float lastAttackTime;
    
    private void Start()
    {
        enemyCharacter = GetComponent<EnemyCharacter>();
        if (enemyCharacter == null)
        {
            Debug.LogError("EnemyAI requires EnemyCharacter component!");
            enabled = false;
            return;
        }
        
        float attackCooldown = GetAttackCooldown();
        lastAttackTime = -attackCooldown;
    }
    
    private float GetAttackCooldown()
    {
        if (enemyCharacter != null && enemyCharacter.GetAttackSpeed() > 0)
        {
            return 1f / enemyCharacter.GetAttackSpeed();
        }
        return 1f;
    }
    
    private void Update()
    {
        if (enemyCharacter.IsDead()) return;
        
        UpdateTarget();
        
        if (currentTarget != null && currentTarget.IsValidTarget())
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.GetTargetPosition());
            
            if (distanceToTarget <= enemyCharacter.GetAttackRange())
            {
                TryAttack();
            }
            else if (distanceToTarget <= detectionRange)
            {
                MoveTowardsTarget();
            }
            else
            {
                currentTarget = null;
            }
        }
    }
    
    private void UpdateTarget()
    {
        if (currentTarget != null && currentTarget.IsValidTarget())
        {
            return;
        }
        
        currentTarget = FindPlayerTarget();
    }
    
    private ITargetable FindPlayerTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        ITargetable closestPlayer = null;
        float closestDistance = float.MaxValue;
        
        foreach (var collider in colliders)
        {
            var targetable = collider.GetComponent<ITargetable>();
            if (targetable != null && targetable.IsValidTarget() && targetable.GetCharacterType() == CharacterType.Player)
            {
                float distance = Vector3.Distance(transform.position, targetable.GetTargetPosition());
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = targetable;
                }
            }
        }
        
        return closestPlayer;
    }
    
    private void MoveTowardsTarget()
    {
        if (currentTarget == null) return;
        
        Vector3 targetPosition = currentTarget.GetTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        float distance = Vector3.Distance(transform.position, targetPosition);
        
        if (distance > stopDistance)
        {
            enemyCharacter.Move(new Vector2(direction.x, 0));
        }
        else
        {
            enemyCharacter.Move(Vector2.zero);
        }
    }
    
    private void TryAttack()
    {
        if (currentTarget == null) return;
        
        float attackCooldown = GetAttackCooldown();
        if (Time.time >= lastAttackTime + attackCooldown && enemyCharacter.CanAttack())
        {
            var damageable = currentTarget as IDamageable;
            if (damageable != null)
            {
                enemyCharacter.Attack(damageable);
                lastAttackTime = Time.time;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        if (enemyCharacter != null)
        {
            Gizmos.DrawWireSphere(transform.position, enemyCharacter.GetAttackRange());
        }
    }
}

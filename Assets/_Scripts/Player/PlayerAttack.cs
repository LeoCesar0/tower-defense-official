using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;
    
    [Header("Attack Timing")]
    public float attackCooldown = 0.5f;
    public float comboWindow = 0.3f;
    
    [Header("Attack Effects")]
    public GameObject attackEffect;
    public AudioClip attackSound;
    
    // Private variables
    private PlayerStats stats;
    private PlayerStateMachine stateMachine;
    private AudioSource audioSource;
    private float lastAttackTime;
    private int currentCombo = 0;
    private float comboResetTime;
    
    // Attack types
    public enum AttackType
    {
        LightAttack,
        HeavyAttack,
        SpecialAttack
    }
    
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        audioSource = GetComponent<AudioSource>();
        
        if (attackPoint == null)
            attackPoint = transform;
    }
    
    void Update()
    {
        HandleAttackInput();
        UpdateComboTimer();
    }
    
    void HandleAttackInput()
    {
        // Light attack (left mouse button or X key)
        if (Input.GetButtonDown("Fire1") && CanAttack())
        {
            PerformAttack(AttackType.LightAttack);
        }
        
        // Heavy attack (right mouse button or C key)
        if (Input.GetButtonDown("Fire2") && CanAttack())
        {
            PerformAttack(AttackType.HeavyAttack);
        }
        
        // Special attack (Q key)
        if (Input.GetKeyDown(KeyCode.Q) && CanAttack())
        {
            PerformAttack(AttackType.SpecialAttack);
        }
    }
    
    bool CanAttack()
    {
        return !(stateMachine.GetCurrentState() is AttackingState) && 
               !(stateMachine.GetCurrentState() is DeadState) && 
               Time.time >= lastAttackTime + attackCooldown;
    }
    
    
    void PerformAttack(AttackType attackType)
    {
        lastAttackTime = Time.time;
        stateMachine.TryTransitionToAttacking();
        
        // Set specific attack animation
        switch (attackType)
        {
            case AttackType.LightAttack:
                stateMachine.GetAnimator()?.SetBool("attack1", true);
                StartCoroutine(ExecuteAttack(attackType, 0.2f));
                break;
            case AttackType.HeavyAttack:
                stateMachine.GetAnimator()?.SetBool("attack2", true);
                StartCoroutine(ExecuteAttack(attackType, 0.4f));
                break;
            case AttackType.SpecialAttack:
                stateMachine.GetAnimator()?.SetBool("attack1", true);
                StartCoroutine(ExecuteAttack(attackType, 0.6f));
                break;
        }
        
        // Play attack sound
        if (attackSound && audioSource)
            audioSource.PlayOneShot(attackSound);
        
        // Update combo
        UpdateCombo(attackType);
    }
    
    IEnumerator ExecuteAttack(AttackType attackType, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        float damage = CalculateDamage(attackType);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            // Apply damage to enemy
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage, DamageType.Physical);
            }
            
            // Create attack effect
            if (attackEffect)
            {
                Instantiate(attackEffect, enemy.transform.position, Quaternion.identity);
            }
        }
        
        // Reset attack state
        yield return new WaitForSeconds(0.1f);
        stateMachine.GetAnimator()?.SetBool("attack1", false);
        stateMachine.GetAnimator()?.SetBool("attack2", false);
        stateMachine.TryTransitionToIdle();
    }
    
    float CalculateDamage(AttackType attackType)
    {
        float baseDamage = stats.attackDamage;
        float multiplier = 1f;
        
        switch (attackType)
        {
            case AttackType.LightAttack:
                multiplier = 1f;
                break;
            case AttackType.HeavyAttack:
                multiplier = 1.5f;
                break;
            case AttackType.SpecialAttack:
                multiplier = 2f;
                break;
        }
        
        // Apply combo bonus
        if (currentCombo > 1)
        {
            multiplier += (currentCombo - 1) * 0.1f;
        }
        
        // Check for critical hit
        if (Random.Range(0f, 1f) < stats.criticalChance)
        {
            multiplier *= stats.criticalMultiplier;
        }
        
        return baseDamage * multiplier;
    }
    
    void UpdateCombo(AttackType attackType)
    {
        if (Time.time <= comboResetTime)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 1;
        }
        
        comboResetTime = Time.time + comboWindow;
    }
    
    void UpdateComboTimer()
    {
        if (Time.time > comboResetTime && currentCombo > 0)
        {
            currentCombo = 0;
        }
    }
    
    // Visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    // Public methods for external access
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    
    public float GetAttackCooldownRemaining()
    {
        return Mathf.Max(0, (lastAttackTime + attackCooldown) - Time.time);
    }
    
    public bool IsAttacking()
    {
        return stateMachine.GetCurrentState() is AttackingState;
    }
}

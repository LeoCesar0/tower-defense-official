using System.Collections;
using UnityEngine;

public class KnightAbilities : MonoBehaviour
{
    [Header("Knight Abilities")]
    public float shieldBashRange = 2f;
    public float shieldBashDamage = 40f;
    public float shieldBashCooldown = 8f;
    public float defensiveStanceDuration = 5f;
    public float defensiveStanceCooldown = 15f;
    public float defensiveStanceArmorBonus = 10f;
    
    public float whirlwindRange = 3f;
    public float whirlwindDamage = 30f;
    public float whirlwindCooldown = 12f;
    
    [Header("Ability Effects")]
    public GameObject shieldBashEffect;
    public GameObject defensiveStanceEffect;
    public GameObject whirlwindEffect;
    public AudioClip shieldBashSound;
    public AudioClip defensiveStanceSound;
    public AudioClip whirlwindSound;
    
    // Private variables
    private PlayerController playerController;
    private PlayerStateMachine stateMachine;
    private AudioSource audioSource;
    
    // Cooldown tracking
    private float lastShieldBashTime;
    private float lastDefensiveStanceTime;
    private float lastWhirlwindTime;
    
    // Active ability states
    private bool isDefensiveStanceActive = false;
    private float defensiveStanceEndTime;
    private float originalArmor;
    
    // Events
    public System.Action<string> OnAbilityUsed; // ability name
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        stateMachine = GetComponent<PlayerStateMachine>();
        audioSource = GetComponent<AudioSource>();
        
        // Store original armor value
        originalArmor = playerController.GetStats().physicalArmor;
    }
    
    void Update()
    {
        HandleAbilityInput();
        UpdateDefensiveStance();
    }
    
    void HandleAbilityInput()
    {
        // Shield Bash (E key)
        if (Input.GetKeyDown(KeyCode.E) && CanUseShieldBash())
        {
            UseShieldBash();
        }
        
        // Defensive Stance (R key)
        if (Input.GetKeyDown(KeyCode.R) && CanUseDefensiveStance())
        {
            UseDefensiveStance();
        }
        
        // Whirlwind (T key)
        if (Input.GetKeyDown(KeyCode.T) && CanUseWhirlwind())
        {
            UseWhirlwind();
        }
    }
    
    // Shield Bash Ability
    bool CanUseShieldBash()
    {
        return Time.time >= lastShieldBashTime + shieldBashCooldown &&
               !(stateMachine.GetCurrentState() is AttackingState) &&
               !(stateMachine.GetCurrentState() is DeadState);
    }
    
    void UseShieldBash()
    {
        lastShieldBashTime = Time.time;
        stateMachine.TryTransitionToAttacking();
        
        // Play sound
        if (shieldBashSound && audioSource)
            audioSource.PlayOneShot(shieldBashSound);
        
        // Create effect
        if (shieldBashEffect)
            Instantiate(shieldBashEffect, transform.position, transform.rotation);
        
        // Execute shield bash
        StartCoroutine(ExecuteShieldBash());
        
        OnAbilityUsed?.Invoke("Shield Bash");
    }
    
    IEnumerator ExecuteShieldBash()
    {
        yield return new WaitForSeconds(0.3f);
        
        // Find enemies in range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, shieldBashRange);
        
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    // Deal damage and knockback
                    enemyController.TakeDamage(shieldBashDamage, DamageType.Physical);
                    
                    // Apply knockback
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb)
                    {
                        enemyRb.AddForce(knockbackDirection * 500f, ForceMode2D.Impulse);
                    }
                }
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        stateMachine.TryTransitionToIdle();
    }
    
    // Defensive Stance Ability
    bool CanUseDefensiveStance()
    {
        return Time.time >= lastDefensiveStanceTime + defensiveStanceCooldown &&
               !isDefensiveStanceActive &&
               !(stateMachine.GetCurrentState() is DeadState);
    }
    
    void UseDefensiveStance()
    {
        lastDefensiveStanceTime = Time.time;
        isDefensiveStanceActive = true;
        defensiveStanceEndTime = Time.time + defensiveStanceDuration;
        
        // Increase armor
        var stats = playerController.GetStats();
        stats.physicalArmor += (int)defensiveStanceArmorBonus;
        
        // Play sound
        if (defensiveStanceSound && audioSource)
            audioSource.PlayOneShot(defensiveStanceSound);
        
        // Create effect
        if (defensiveStanceEffect)
        {
            GameObject effect = Instantiate(defensiveStanceEffect, transform);
            Destroy(effect, defensiveStanceDuration);
        }
        
        OnAbilityUsed?.Invoke("Defensive Stance");
    }
    
    void UpdateDefensiveStance()
    {
        if (isDefensiveStanceActive && Time.time >= defensiveStanceEndTime)
        {
            EndDefensiveStance();
        }
    }
    
    void EndDefensiveStance()
    {
        isDefensiveStanceActive = false;
        
        // Restore original armor
        var stats = playerController.GetStats();
        stats.physicalArmor = originalArmor;
    }
    
    // Whirlwind Ability
    bool CanUseWhirlwind()
    {
        return Time.time >= lastWhirlwindTime + whirlwindCooldown &&
               !(stateMachine.GetCurrentState() is AttackingState) &&
               !(stateMachine.GetCurrentState() is DeadState);
    }
    
    void UseWhirlwind()
    {
        lastWhirlwindTime = Time.time;
        stateMachine.TryTransitionToAttacking();
        
        // Play sound
        if (whirlwindSound && audioSource)
            audioSource.PlayOneShot(whirlwindSound);
        
        // Create effect
        if (whirlwindEffect)
            Instantiate(whirlwindEffect, transform.position, Quaternion.identity);
        
        // Execute whirlwind
        StartCoroutine(ExecuteWhirlwind());
        
        OnAbilityUsed?.Invoke("Whirlwind");
    }
    
    IEnumerator ExecuteWhirlwind()
    {
        // Multiple hits over time
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            
            // Find enemies in range
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, whirlwindRange);
            
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.TakeDamage(whirlwindDamage, DamageType.Physical);
                    }
                }
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        stateMachine.TryTransitionToIdle();
    }
    
    // Public getters for UI
    public float GetShieldBashCooldownRemaining()
    {
        return Mathf.Max(0, (lastShieldBashTime + shieldBashCooldown) - Time.time);
    }
    
    public float GetDefensiveStanceCooldownRemaining()
    {
        return Mathf.Max(0, (lastDefensiveStanceTime + defensiveStanceCooldown) - Time.time);
    }
    
    public float GetWhirlwindCooldownRemaining()
    {
        return Mathf.Max(0, (lastWhirlwindTime + whirlwindCooldown) - Time.time);
    }
    
    public bool IsDefensiveStanceActive() => isDefensiveStanceActive;
    
    public float GetDefensiveStanceTimeRemaining()
    {
        if (!isDefensiveStanceActive) return 0;
        return Mathf.Max(0, defensiveStanceEndTime - Time.time);
    }
    
    // Visualize ability ranges in editor
    void OnDrawGizmosSelected()
    {
        // Shield Bash range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shieldBashRange);
        
        // Whirlwind range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, whirlwindRange);
    }
}

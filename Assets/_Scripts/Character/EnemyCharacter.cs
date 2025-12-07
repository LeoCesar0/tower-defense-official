using UnityEngine;

public class EnemyCharacter : BaseCharacter
{
    [Header("Enemy Settings")]
    public float detectionRange = 10f;
    
    private EnemyAI enemyAI;
    private float lastAttackTime;
    private float attackStateDuration = 0.3f;
    private float attackStateStartTime;
    
    #region Unity Lifecycle
    
    protected override void Start()
    {
        base.Start();
        
        enemyAI = GetComponent<EnemyAI>();
        if (enemyAI == null)
        {
            enemyAI = gameObject.AddComponent<EnemyAI>();
        }
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (currentState == CharacterState.Attacking && Time.time >= attackStateStartTime + attackStateDuration)
        {
            ChangeState(CharacterState.Idle);
        }
    }
    
    #endregion
    
    #region Movement Implementation
    
    public override void Move(Vector2 direction)
    {
        if (isDead || currentState == CharacterState.Attacking) return;
        
        if (direction.magnitude > 0.1f)
        {
            ChangeState(CharacterState.Moving);
            
            Vector2 velocity = rb.velocity;
            velocity.x = direction.x * currentStats.moveSpeed;
            rb.velocity = velocity;
            
            if (direction.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            ChangeState(CharacterState.Idle);
        }
    }
    
    public override void Jump()
    {
        if (isDead || !IsGrounded() || currentState == CharacterState.Attacking) return;
        
        ChangeState(CharacterState.Jumping);
        rb.velocity = new Vector2(rb.velocity.x, currentStats.jumpForce);
    }
    
    public override void Dash(Vector2 direction)
    {
        if (isDead || currentState == CharacterState.Dashing) return;
        
        ChangeState(CharacterState.Dashing);
    }
    
    public override bool IsGrounded()
    {
        return Mathf.Abs(rb.velocity.y) < 0.1f;
    }
    
    public override float GetMoveSpeed() => currentStats.moveSpeed;
    
    public override Vector2 GetVelocity() => rb.velocity;
    
    #endregion
    
    #region Attack Implementation
    
    public override void Attack(IDamageable target)
    {
        if (!CanAttack() || target == null) return;
        
        float attackCooldown = GetAttackCooldown();
        if (Time.time < lastAttackTime + attackCooldown) return;
        
        ChangeState(CharacterState.Attacking);
        lastAttackTime = Time.time;
        attackStateStartTime = Time.time;
        
        float baseDamage = GetAttackDamage();
        bool isCritical = Random.Range(0f, 1f) < currentStats.criticalChance;
        float damage = baseDamage;
        
        if (isCritical)
        {
            damage *= currentStats.criticalMultiplier;
        }
        
        string targetName = (target as MonoBehaviour)?.gameObject.name ?? "Unknown";
        string critText = isCritical ? " CRITICAL!" : "";
        
        Debug.Log($"[ATTACK] Enemy {gameObject.name} attacks {targetName} for {damage:F1} damage (Base: {baseDamage:F1}){critText}");
        
        target.TakeDamage(damage, DamageType.Physical);
        
        SetAnimationTrigger("attack");
        PlayAttackEffects();
    }
    
    private float GetAttackCooldown()
    {
        if (GetAttackSpeed() > 0)
        {
            return 1f / GetAttackSpeed();
        }
        return 1f;
    }
    
    public override void Attack(Vector3 position, float range)
    {
        if (!CanAttack()) return;
        
        ChangeState(CharacterState.Attacking);
        
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, range);
        int hitCount = 0;
        
        foreach (var target in targets)
        {
            var damageable = target.GetComponent<IDamageable>();
            if (damageable != null && damageable != this)
            {
                var targetable = damageable as ITargetable;
                if (targetable != null && targetable.GetCharacterType() == CharacterType.Player)
                {
                    Attack(damageable);
                    hitCount++;
                    break;
                }
            }
        }
        
        if (hitCount > 0)
        {
            Debug.Log($"[ATTACK] Enemy {gameObject.name} area attack hit {hitCount} target(s) at range {range:F1}");
        }
    }
    
    public override bool CanAttack()
    {
        if (isDead || currentState == CharacterState.Attacking || currentState == CharacterState.Casting)
            return false;
        
        float attackCooldown = GetAttackCooldown();
        return Time.time >= lastAttackTime + attackCooldown;
    }
    
    public override float GetAttackDamage() => currentStats.attackDamage;
    
    public override float GetAttackRange() => currentStats.attackRange;
    
    public override float GetAttackSpeed() => currentStats.attackSpeed;
    
    #endregion
    
    #region Ability Implementation
    
    public override void UseAbility(AbilityData ability)
    {
    }
    
    public override void UseAbility(int abilityIndex)
    {
    }
    
    public override bool CanUseAbility(AbilityData ability)
    {
        return false;
    }
    
    public override float GetAbilityCooldown(AbilityData ability)
    {
        return 0f;
    }
    
    public override AbilityData[] GetAvailableAbilities()
    {
        return new AbilityData[0];
    }
    
    #endregion
    
    #region Targetable Implementation
    
    public override Transform GetTransform() => transform;
    
    public override Vector3 GetTargetPosition() => transform.position;
    
    public override bool IsValidTarget() => !isDead;
    
    public override CharacterType GetCharacterType() => CharacterType.Enemy;
    
    #endregion
    
    #region Leveling Implementation
    
    public override void GainExperience(int amount)
    {
    }
    
    public override int GetExperience() => 0;
    
    public override int GetExperienceToNextLevel() => 0;
    
    public override bool CanLevelUp() => false;
    
    public override void OnLevelUp()
    {
    }
    
    #endregion
    
    #region Helper Methods
    
    private void PlayAttackEffects()
    {
        if (characterPreset?.characterData?.attackSounds != null && characterPreset.characterData.attackSounds.Length > 0)
        {
            var randomSound = characterPreset.characterData.attackSounds[Random.Range(0, characterPreset.characterData.attackSounds.Length)];
            if (audioSource && randomSound)
            {
                audioSource.PlayOneShot(randomSound);
            }
        }
        
        if (characterPreset?.characterData?.attackEffects != null && characterPreset.characterData.attackEffects.Length > 0)
        {
            var randomEffect = characterPreset.characterData.attackEffects[Random.Range(0, characterPreset.characterData.attackEffects.Length)];
            if (randomEffect)
            {
                Instantiate(randomEffect, transform.position, Quaternion.identity);
            }
        }
    }
    
    #endregion
}

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Player character implementation using the new data architecture
/// </summary>
public class PlayerCharacter : BaseCharacter, IControllable
{
    [Header("Player Settings")]
    public bool inputEnabled = true;
    
    [Header("Experience")]
    public int experience = 0;
    public int experienceToNextLevel = 100;
    
    // Input handling
    private Vector2 inputDirection;
    private bool jumpInput;
    private bool dashInput;
    private bool attackInput;
    private bool[] abilityInputs = new bool[4]; // E, R, T, Q
    
    // Ability system
    private AbilityData[] availableAbilities;
    private float[] abilityCooldowns;
    
    // Movement
    private float dashTime;
    private Vector2 dashDirection;
    
    #region Unity Lifecycle
    
    protected override void Start()
    {
        base.Start();
        
        // Initialize ability system
        if (characterPreset?.abilities != null)
        {
            availableAbilities = characterPreset.abilities;
            abilityCooldowns = new float[availableAbilities.Length];
        }
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (inputEnabled)
        {
            HandleInput();
        }
        
        HandleMovement();
        HandleAbilities();
    }
    
    #endregion
    
    #region Input Handling
    
    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }
    
    public bool IsInputEnabled() => inputEnabled;
    
    public void HandleInput()
    {
        // Movement input
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        
        // Attack input
        attackInput = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2");
        
        // Ability input
        abilityInputs[0] = Input.GetKeyDown(KeyCode.E); // Shield Bash
        abilityInputs[1] = Input.GetKeyDown(KeyCode.R); // Defensive Stance
        abilityInputs[2] = Input.GetKeyDown(KeyCode.T); // Whirlwind
        abilityInputs[3] = Input.GetKeyDown(KeyCode.Q); // Special Attack
        
        // Debug input
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExperience(100);
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20);
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
            
            // Apply movement
            Vector2 velocity = rb.velocity;
            velocity.x = direction.x * currentStats.moveSpeed;
            rb.velocity = velocity;
            
            // Flip sprite
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
        dashTime = currentStats.dashCooldown;
        dashDirection = direction.magnitude > 0 ? direction.normalized : Vector2.right * transform.localScale.x;
    }
    
    public override bool IsGrounded()
    {
        // Simple ground check - you can improve this with raycasting
        return Mathf.Abs(rb.velocity.y) < 0.1f;
    }
    
    public override float GetMoveSpeed() => currentStats.moveSpeed;
    
    public override Vector2 GetVelocity() => rb.velocity;
    
    #endregion
    
    #region Attack Implementation
    
    public override void Attack(IDamageable target)
    {
        if (!CanAttack()) return;
        
        ChangeState(CharacterState.Attacking);
        
        float damage = GetAttackDamage();
        if (Random.Range(0f, 1f) < currentStats.criticalChance)
        {
            damage *= currentStats.criticalMultiplier;
        }
        
        target.TakeDamage(damage, DamageType.Physical);
        
        // Play attack animation and effects
        SetAnimationTrigger("attack");
        PlayAttackEffects();
    }
    
    public override void Attack(Vector3 position, float range)
    {
        if (!CanAttack()) return;
        
        ChangeState(CharacterState.Attacking);
        
        // Find targets in range
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, range);
        
        foreach (var target in targets)
        {
            var damageable = target.GetComponent<IDamageable>();
            if (damageable != null && damageable != this)
            {
                Attack(damageable);
            }
        }
    }
    
    public override bool CanAttack()
    {
        return !isDead && currentState != CharacterState.Attacking && currentState != CharacterState.Casting;
    }
    
    public override float GetAttackDamage() => currentStats.attackDamage;
    
    public override float GetAttackRange() => currentStats.attackRange;
    
    public override float GetAttackSpeed() => currentStats.attackSpeed;
    
    #endregion
    
    #region Ability Implementation
    
    public override void UseAbility(AbilityData ability)
    {
        if (!CanUseAbility(ability)) return;
        
        // Find ability index
        int abilityIndex = System.Array.IndexOf(availableAbilities, ability);
        if (abilityIndex >= 0)
        {
            UseAbility(abilityIndex);
        }
    }
    
    public override void UseAbility(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= availableAbilities.Length) return;
        
        var ability = availableAbilities[abilityIndex];
        if (!CanUseAbility(ability)) return;
        
        // Set cooldown
        abilityCooldowns[abilityIndex] = ability.cooldown;
        
        // Use mana if required
        if (ability.manaCost > 0)
        {
            currentStats.mana = Mathf.Max(0, currentStats.mana - ability.manaCost);
        }
        
        // Execute ability
        ExecuteAbility(ability);
    }
    
    public override bool CanUseAbility(AbilityData ability)
    {
        if (ability == null || isDead) return false;
        
        // Check class requirement
        if (ability.requiredClass != GetCharacterClass())
            return false;
        
        // Check mana cost
        if (ability.manaCost > currentStats.mana)
            return false;
        
        // Check cooldown
        int abilityIndex = System.Array.IndexOf(availableAbilities, ability);
        if (abilityIndex >= 0 && abilityCooldowns[abilityIndex] > 0)
            return false;
        
        return true;
    }
    
    public override float GetAbilityCooldown(AbilityData ability)
    {
        int abilityIndex = System.Array.IndexOf(availableAbilities, ability);
        if (abilityIndex >= 0)
        {
            return abilityCooldowns[abilityIndex];
        }
        return 0f;
    }
    
    public override AbilityData[] GetAvailableAbilities() => availableAbilities;
    
    #endregion
    
    #region Leveling Implementation
    
    public override void GainExperience(int amount)
    {
        experience += amount;
        
        while (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    
    public override int GetExperience() => experience;
    
    public override int GetExperienceToNextLevel() => experienceToNextLevel;
    
    public override bool CanLevelUp() => experience >= experienceToNextLevel;
    
    public override void OnLevelUp()
    {
        experience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f);
        
        // Full heal on level up
        Heal(currentStats.maxHp);
        currentStats.mana = currentStats.maxMana;
        
        Debug.Log($"Level Up! Now level {currentStats.level}");
    }
    
    #endregion
    
    #region Targetable Implementation
    
    public override Transform GetTransform() => transform;
    
    public override Vector3 GetTargetPosition() => transform.position;
    
    public override bool IsValidTarget() => !isDead;
    
    public override CharacterType GetCharacterType() => CharacterType.Player;
    
    #endregion
    
    #region Helper Methods
    
    private void HandleMovement()
    {
        if (inputEnabled)
        {
            Move(inputDirection);
            
            if (jumpInput)
            {
                Jump();
            }
            
            if (dashInput)
            {
                Dash(inputDirection);
            }
        }
        
        // Handle dash
        if (currentState == CharacterState.Dashing)
        {
            rb.velocity = dashDirection * currentStats.dashSpeed;
            dashTime -= Time.deltaTime;
            
            if (dashTime <= 0)
            {
                ChangeState(CharacterState.Idle);
            }
        }
        
        // Handle falling
        if (rb.velocity.y < 0 && currentState == CharacterState.Jumping)
        {
            ChangeState(CharacterState.Falling);
        }
        
        // Handle landing
        if (IsGrounded() && (currentState == CharacterState.Falling || currentState == CharacterState.Jumping))
        {
            ChangeState(CharacterState.Idle);
        }
    }
    
    private void HandleAbilities()
    {
        // Update cooldowns
        for (int i = 0; i < abilityCooldowns.Length; i++)
        {
            if (abilityCooldowns[i] > 0)
            {
                abilityCooldowns[i] -= Time.deltaTime;
            }
        }
        
        // Handle ability input
        if (inputEnabled)
        {
            for (int i = 0; i < abilityInputs.Length && i < availableAbilities.Length; i++)
            {
                if (abilityInputs[i])
                {
                    UseAbility(i);
                }
            }
            
            if (attackInput)
            {
                Attack(transform.position, currentStats.attackRange);
            }
        }
    }
    
    private void ExecuteAbility(AbilityData ability)
    {
        // Play animation
        if (!string.IsNullOrEmpty(ability.animationTrigger))
        {
            SetAnimationTrigger(ability.animationTrigger);
        }
        
        // Play sound
        if (ability.abilitySound && audioSource)
        {
            audioSource.PlayOneShot(ability.abilitySound);
        }
        
        // Create effect
        if (ability.abilityEffect)
        {
            Instantiate(ability.abilityEffect, transform.position, Quaternion.identity);
        }
        
        // Execute ability logic based on type
        switch (ability.abilityType)
        {
            case AbilityType.Attack:
                Attack(transform.position, ability.range);
                break;
            case AbilityType.Buff:
                if (ability.appliesBuff && ability.buffData)
                {
                    ApplyBuff(ability.buffData);
                }
                break;
            // Add more ability types as needed
        }
    }
    
    private void PlayAttackEffects()
    {
        // Play attack sound
        if (characterPreset?.characterData?.attackSounds != null && characterPreset.characterData.attackSounds.Length > 0)
        {
            var randomSound = characterPreset.characterData.attackSounds[Random.Range(0, characterPreset.characterData.attackSounds.Length)];
            if (audioSource && randomSound)
            {
                audioSource.PlayOneShot(randomSound);
            }
        }
        
        // Create attack effect
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

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base character class that implements all character interfaces
/// This serves as the foundation for all character types
/// </summary>
public abstract class BaseCharacter : MonoBehaviour, ICharacter
{
    [Header("Character Data")]
    [SerializeField] protected CharacterPreset characterPreset;
    [SerializeField] protected CharacterStats currentStats;
    [SerializeField] protected List<BuffData> activeBuffs = new List<BuffData>();
    [SerializeField] protected Dictionary<EquipmentSlot, EquipmentData> equippedItems = new Dictionary<EquipmentSlot, EquipmentData>();
    
    // Component references
    protected Animator animator;
    protected Rigidbody2D rb;
    protected AudioSource audioSource;
    protected CharacterStateMachine stateMachine;
    
    // Events
    public System.Action<CharacterStats> OnStatsChanged;
    public System.Action<int, int> OnHealthChanged;
    public System.Action OnCharacterDeath;
    public System.Action<CharacterState> OnStateChanged;
    public System.Action<BuffData> OnBuffApplied;
    public System.Action<BuffData> OnBuffRemoved;
    
    // Current state
    protected CharacterState currentState = CharacterState.Idle;
    protected bool isDead = false;
    protected float lastDamageTime;
    protected bool isInvulnerable = false;
    
    #region Unity Lifecycle
    
    protected virtual void Awake()
    {
        SetupComponents();
    }
    
    protected virtual void Start()
    {
        if (characterPreset != null)
        {
            InitializeFromPreset(characterPreset);
        }
    }
    
    protected virtual void Update()
    {
        UpdateCharacter();
        UpdateBuffs();
        UpdateInvulnerability();
    }
    
    #endregion
    
    #region Component Setup
    
    protected virtual void SetupComponents()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        stateMachine = GetComponent<CharacterStateMachine>();
        
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<CharacterStateMachine>();
        }
    }
    
    #endregion
    
    #region ICharacter Implementation
    
    public virtual void InitializeFromPreset(CharacterPreset preset)
    {
        characterPreset = preset;
        currentStats = preset.characterData.ToCharacterStats();
        
        // Apply starting equipment
        if (preset.startingEquipment != null)
        {
            foreach (var equipment in preset.startingEquipment)
            {
                EquipItem(equipment);
            }
        }
        
        // Initialize abilities
        InitializeAbilities(preset.abilities);
        
        OnStatsChanged?.Invoke(currentStats);
    }
    
    public virtual CharacterPreset GetPreset() => characterPreset;
    public virtual CharacterData GetCharacterData() => characterPreset?.characterData;
    
    public virtual void UpdateCharacter()
    {
        // Override in derived classes for specific behavior
    }
    
    #endregion
    
    #region IDamageable Implementation
    
    public virtual void TakeDamage(float damage, DamageType damageType)
    {
        if (isDead || isInvulnerable) return;
        
        float actualDamage = CalculateDamage(damage, damageType);
        currentStats.hp = Mathf.Max(0, currentStats.hp - Mathf.RoundToInt(actualDamage));
        lastDamageTime = Time.time;
        isInvulnerable = true;
        
        OnHealthChanged?.Invoke(currentStats.hp, currentStats.maxHp);
        
        if (currentStats.hp <= 0)
        {
            Die();
        }
    }
    
    public virtual void Heal(float amount)
    {
        if (isDead) return;
        
        currentStats.hp = Mathf.Min(currentStats.maxHp, currentStats.hp + Mathf.RoundToInt(amount));
        OnHealthChanged?.Invoke(currentStats.hp, currentStats.maxHp);
    }
    
    public virtual bool IsDead() => isDead;
    public virtual float GetHealthPercentage() => (float)currentStats.hp / currentStats.maxHp;
    public virtual int GetCurrentHealth() => currentStats.hp;
    public virtual int GetMaxHealth() => currentStats.maxHp;
    
    protected virtual void Die()
    {
        isDead = true;
        currentStats.hp = 0;
        ChangeState(CharacterState.Dead);
        OnCharacterDeath?.Invoke();
    }
    
    #endregion
    
    #region IStatHolder Implementation
    
    public virtual CharacterStats GetStats() => currentStats;
    
    public virtual void ModifyStats(CharacterStats newStats)
    {
        currentStats = newStats;
        OnStatsChanged?.Invoke(currentStats);
    }
    
    public virtual void ApplyEquipmentBonus(EquipmentBonus bonus)
    {
        currentStats = currentStats.ApplyEquipmentBonus(bonus);
        OnStatsChanged?.Invoke(currentStats);
    }
    
    public virtual void LevelUp()
    {
        currentStats = currentStats.LevelUp();
        OnStatsChanged?.Invoke(currentStats);
    }
    
    public virtual int GetLevel() => currentStats.level;
    public virtual CharacterClass GetCharacterClass() => characterPreset?.characterClass ?? CharacterClass.Knight;
    
    #endregion
    
    #region IStateful Implementation
    
    public virtual CharacterState GetCurrentState() => currentState;
    
    public virtual void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;
        
        OnStateExit(currentState);
        currentState = newState;
        OnStateEnter(currentState);
        OnStateChanged?.Invoke(currentState);
    }
    
    public virtual bool CanTransitionTo(CharacterState targetState)
    {
        // Override in derived classes for specific transition rules
        return true;
    }
    
    public virtual void OnStateEnter(CharacterState state)
    {
        // Override in derived classes
    }
    
    public virtual void OnStateExit(CharacterState state)
    {
        // Override in derived classes
    }
    
    #endregion
    
    #region IBuffable Implementation
    
    public virtual void ApplyBuff(BuffData buff, int stackCount = 1)
    {
        if (buff == null) return;
        
        // Check if buff already exists
        int existingIndex = activeBuffs.FindIndex(b => b == buff);
        if (existingIndex >= 0)
        {
            if (buff.isStackable)
            {
                // Increase stack count (you might want to store stack counts separately)
                OnBuffApplied?.Invoke(buff);
            }
        }
        else
        {
            activeBuffs.Add(buff);
            OnBuffApplied?.Invoke(buff);
        }
    }
    
    public virtual void RemoveBuff(BuffData buff)
    {
        if (activeBuffs.Remove(buff))
        {
            OnBuffRemoved?.Invoke(buff);
        }
    }
    
    public virtual void RemoveAllBuffs()
    {
        activeBuffs.Clear();
    }
    
    public virtual bool HasBuff(BuffData buff) => activeBuffs.Contains(buff);
    
    public virtual int GetBuffStackCount(BuffData buff)
    {
        // Simplified - you might want to track stack counts separately
        return activeBuffs.Contains(buff) ? 1 : 0;
    }
    
    public virtual BuffData[] GetActiveBuffs() => activeBuffs.ToArray();
    
    #endregion
    
    #region IEquippable Implementation
    
    public virtual void EquipItem(EquipmentData equipment)
    {
        if (equipment == null || !CanEquipItem(equipment)) return;
        
        // Unequip existing item in the same slot
        UnequipItem(equipment.equipmentSlot);
        
        // Equip new item
        equippedItems[equipment.equipmentSlot] = equipment;
        
        // Apply stat bonuses
        ApplyEquipmentBonus(equipment.statBonuses);
        
        // Play equip sound
        if (equipment.equipSound && audioSource)
        {
            audioSource.PlayOneShot(equipment.equipSound);
        }
    }
    
    public virtual void UnequipItem(EquipmentSlot slot)
    {
        if (equippedItems.TryGetValue(slot, out EquipmentData equipment))
        {
            // Remove stat bonuses
            var negativeBonus = new EquipmentBonus
            {
                attackDamageBonus = -equipment.statBonuses.attackDamageBonus,
                magicDamageBonus = -equipment.statBonuses.magicDamageBonus,
                attackSpeedBonus = -equipment.statBonuses.attackSpeedBonus,
                physicalArmorBonus = -equipment.statBonuses.physicalArmorBonus,
                magicArmorBonus = -equipment.statBonuses.magicArmorBonus,
                criticalChanceBonus = -equipment.statBonuses.criticalChanceBonus
            };
            ApplyEquipmentBonus(negativeBonus);
            
            // Play unequip sound
            if (equipment.unequipSound && audioSource)
            {
                audioSource.PlayOneShot(equipment.unequipSound);
            }
            
            equippedItems.Remove(slot);
        }
    }
    
    public virtual EquipmentData GetEquippedItem(EquipmentSlot slot)
    {
        equippedItems.TryGetValue(slot, out EquipmentData equipment);
        return equipment;
    }
    
    public virtual EquipmentData[] GetAllEquippedItems()
    {
        var items = new EquipmentData[equippedItems.Count];
        equippedItems.Values.CopyTo(items, 0);
        return items;
    }
    
    public virtual bool CanEquipItem(EquipmentData equipment)
    {
        if (equipment == null) return false;
        
        return equipment.CanCharacterEquip(GetCharacterClass(), GetLevel());
    }
    
    #endregion
    
    #region IAnimatable Implementation
    
    public virtual Animator GetAnimator() => animator;
    
    public virtual void SetAnimationTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
    
    public virtual void SetAnimationBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }
    
    public virtual void SetAnimationFloat(string parameterName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }
    
    public virtual void SetAnimationInt(string parameterName, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(parameterName, value);
        }
    }
    
    #endregion
    
    #region Helper Methods
    
    protected virtual float CalculateDamage(float baseDamage, DamageType damageType)
    {
        int armor = damageType == DamageType.Physical ? currentStats.physicalArmor : currentStats.magicArmor;
        float finalDamage = Mathf.Max(1, baseDamage - armor);
        
        // Check for dodge
        if (Random.Range(0f, 1f) < currentStats.dodgeChance)
        {
            finalDamage = 0;
        }
        
        return finalDamage;
    }
    
    protected virtual void UpdateBuffs()
    {
        // Update buff durations and effects
        // This is a simplified implementation
    }
    
    protected virtual void UpdateInvulnerability()
    {
        if (isInvulnerable && Time.time >= lastDamageTime + 0.5f)
        {
            isInvulnerable = false;
        }
    }
    
    protected virtual void InitializeAbilities(AbilityData[] abilities)
    {
        // Override in derived classes to set up abilities
    }
    
    #endregion
    
    #region Abstract Methods
    
    // These must be implemented by derived classes
    public abstract void Attack(IDamageable target);
    public abstract void Attack(Vector3 position, float range);
    public abstract bool CanAttack();
    public abstract float GetAttackDamage();
    public abstract float GetAttackRange();
    public abstract float GetAttackSpeed();
    
    public abstract void Move(Vector2 direction);
    public abstract void Jump();
    public abstract void Dash(Vector2 direction);
    public abstract bool IsGrounded();
    public abstract float GetMoveSpeed();
    public abstract Vector2 GetVelocity();
    
    public abstract void UseAbility(AbilityData ability);
    public abstract void UseAbility(int abilityIndex);
    public abstract bool CanUseAbility(AbilityData ability);
    public abstract float GetAbilityCooldown(AbilityData ability);
    public abstract AbilityData[] GetAvailableAbilities();
    
    public abstract Transform GetTransform();
    public abstract Vector3 GetTargetPosition();
    public abstract bool IsValidTarget();
    public abstract CharacterType GetCharacterType();
    
    public abstract void GainExperience(int amount);
    public abstract int GetExperience();
    public abstract int GetExperienceToNextLevel();
    public abstract bool CanLevelUp();
    public abstract void OnLevelUp();
    
    #endregion
}

using UnityEngine;

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

/// <summary>
/// Interface for any character that can move
/// </summary>
public interface IMovable
{
    void Move(Vector2 direction);
    void Jump();
    void Dash(Vector2 direction);
    bool IsGrounded();
    float GetMoveSpeed();
    Vector2 GetVelocity();
}

/// <summary>
/// Interface for any character that can use abilities
/// </summary>
public interface IAbilityUser
{
    void UseAbility(AbilityData ability);
    void UseAbility(int abilityIndex);
    bool CanUseAbility(AbilityData ability);
    float GetAbilityCooldown(AbilityData ability);
    AbilityData[] GetAvailableAbilities();
}

/// <summary>
/// Interface for any character that can be controlled
/// </summary>
public interface IControllable
{
    void SetInputEnabled(bool enabled);
    bool IsInputEnabled();
    void HandleInput();
}

/// <summary>
/// Interface for any character that has stats
/// </summary>
public interface IStatHolder
{
    CharacterStats GetStats();
    void ModifyStats(CharacterStats newStats);
    void ApplyEquipmentBonus(EquipmentBonus bonus);
    void LevelUp();
    int GetLevel();
    CharacterClass GetCharacterClass();
}

/// <summary>
/// Interface for any character that can be targeted
/// </summary>
public interface ITargetable
{
    Transform GetTransform();
    Vector3 GetTargetPosition();
    bool IsValidTarget();
    CharacterType GetCharacterType();
}

/// <summary>
/// Interface for any character that can have buffs/debuffs
/// </summary>
public interface IBuffable
{
    void ApplyBuff(BuffData buff, int stackCount = 1);
    void RemoveBuff(BuffData buff);
    void RemoveAllBuffs();
    bool HasBuff(BuffData buff);
    int GetBuffStackCount(BuffData buff);
    BuffData[] GetActiveBuffs();
}

/// <summary>
/// Interface for any character that can equip items
/// </summary>
public interface IEquippable
{
    void EquipItem(EquipmentData equipment);
    void UnequipItem(EquipmentSlot slot);
    EquipmentData GetEquippedItem(EquipmentSlot slot);
    EquipmentData[] GetAllEquippedItems();
    bool CanEquipItem(EquipmentData equipment);
}

/// <summary>
/// Interface for any character that can experience and level up
/// </summary>
public interface ILevelable
{
    void GainExperience(int amount);
    int GetExperience();
    int GetExperienceToNextLevel();
    bool CanLevelUp();
    void OnLevelUp();
}

/// <summary>
/// Interface for any character that can be in different states
/// </summary>
public interface IStateful
{
    CharacterState GetCurrentState();
    void ChangeState(CharacterState newState);
    bool CanTransitionTo(CharacterState targetState);
    void OnStateEnter(CharacterState state);
    void OnStateExit(CharacterState state);
}

/// <summary>
/// Base character state enum
/// </summary>
public enum CharacterState
{
    Idle,
    Moving,
    Attacking,
    Casting,
    Stunned,
    Dead,
    Dashing,
    Jumping,
    Falling
}

/// <summary>
/// Interface for any character that can be animated
/// </summary>
public interface IAnimatable
{
    Animator GetAnimator();
    void SetAnimationTrigger(string triggerName);
    void SetAnimationBool(string parameterName, bool value);
    void SetAnimationFloat(string parameterName, float value);
    void SetAnimationInt(string parameterName, int value);
}

# Player System Documentation

## Overview

This player system provides a comprehensive RPG-like character system for your tower defense game, featuring modular components that work together to create a robust and scalable player experience.

## Scripts Overview

### Core Scripts

#### 1. **PlayerController.cs** - Main Coordinator

- **Purpose**: Central hub that coordinates all player systems
- **Features**:
  - Manages player class selection (Knight, Mage, Archer, Rogue)
  - Handles experience gain and leveling system
  - Coordinates between all player components
  - Provides events for UI integration
- **Key Methods**:
  - `GainExperience(int amount)` - Add experience points
  - `GetStats()` - Get current player stats
  - `SetPlayerClass(PlayerClass newClass)` - Change player class

#### 2. **PlayerStats.cs** - Data Structure

- **Purpose**: Contains all player statistics and progression data
- **Features**:
  - RPG-style stats (Strength, Agility, Intelligence, Vitality)
  - Health, Mana, and regeneration systems
  - Combat stats (damage, armor, critical chance)
  - Level progression with automatic stat scaling
- **Key Methods**:
  - `CreateKnightStats()` - Initialize Knight class stats
  - `LevelUp()` - Handle level progression

#### 3. **PlayerStateController.cs** - Animation & State Management

- **Purpose**: Manages player states and animation triggers
- **Features**:
  - Handles all player states (idle, walking, jumping, attacking, etc.)
  - Integrates with Unity Animator
  - Manages state transitions and validation
- **Key Methods**:
  - `SetWalking(bool)`, `SetAttacking(bool)`, etc. - State setters
  - All setters include animation triggers

#### 4. **PlayerMovement.cs** - Movement System

- **Purpose**: Handles player movement, jumping, and dashing
- **Features**:
  - Horizontal movement with speed control
  - Jump mechanics with gravity handling
  - Dash ability with cooldown
  - Sprite flipping based on movement direction
- **Key Methods**:
  - Movement is handled automatically in Update()
  - Stats are applied from PlayerController

#### 5. **PlayerAttack.cs** - Combat System

- **Purpose**: Manages player combat mechanics
- **Features**:
  - Light, Heavy, and Special attacks
  - Combo system with timing windows
  - Critical hit calculations
  - Area-of-effect damage detection
  - Mana cost for special attacks
- **Key Methods**:
  - `PerformAttack(AttackType)` - Execute specific attack
  - `GetCurrentCombo()` - Get current combo count

#### 6. **PlayerHealth.cs** - Health & Damage System

- **Purpose**: Manages player health, damage, and death
- **Features**:
  - Health regeneration system
  - Armor and damage type calculations
  - Invulnerability frames after damage
  - Death handling with respawn support
- **Key Methods**:
  - `TakeDamage(int, DamageType)` - Apply damage
  - `Heal(int)` - Restore health
  - `Revive(int)` - Resurrect player

#### 7. **KnightAbilities.cs** - Class-Specific Abilities

- **Purpose**: Implements Knight class special abilities
- **Features**:
  - Shield Bash: Close-range damage with knockback
  - Defensive Stance: Temporary armor boost
  - Whirlwind: Area-of-effect damage attack
  - Cooldown management for all abilities
- **Key Methods**:
  - Abilities are triggered by key presses (E, R, T)
  - Each ability has mana cost and cooldown

## Setup Instructions

### 1. GameObject Setup

1. Create an empty GameObject for your player
2. Add a SpriteRenderer component
3. Add a Rigidbody2D component
4. Add a Collider2D component (BoxCollider2D recommended)
5. Add an Animator component
6. Add an AudioSource component

### 2. Script Components

Add these scripts to your player GameObject in this order:

1. **PlayerController** (main coordinator)
2. **PlayerStateController** (state management)
3. **PlayerMovement** (movement)
4. **PlayerAttack** (combat)
5. **PlayerHealth** (health system)
6. **KnightAbilities** (class abilities)

### 3. Configuration

- Set up your Animator with the following boolean parameters:
  - `isIdle`, `isWalking`, `isJumping`, `isFalling`
  - `isAttacking`, `attack1`, `attack2`
  - `isDead`, `isDashing`
- Configure Input Manager for:
  - Horizontal axis (A/D or Arrow Keys)
  - Jump (Space)
  - Fire1 (Left Mouse Button)
  - Fire2 (Right Mouse Button)
  - Dash (Left Shift)

### 4. Layer Setup

- Create an "Enemy" layer for enemies
- Set the `enemyLayers` in PlayerAttack to include the Enemy layer

## Input Controls

### Movement

- **A/D or Arrow Keys**: Move left/right
- **Space**: Jump
- **Left Shift**: Dash

### Combat

- **Left Mouse Button**: Light Attack
- **Right Mouse Button**: Heavy Attack
- **Q**: Special Attack

### Knight Abilities

- **E**: Shield Bash
- **R**: Defensive Stance
- **T**: Whirlwind

### Debug/Testing (Remove in production)

- **L**: Gain 100 experience
- **H**: Heal 20 HP

## Events System

The player system uses C# events for loose coupling:

```csharp
// Subscribe to events
playerController.OnStatsChanged += UpdateUI;
playerController.OnLevelUp += ShowLevelUpEffect;
playerHealth.OnHealthChanged += UpdateHealthBar;
```

## Extensibility

### Adding New Classes

1. Create a new ability script (e.g., `MageAbilities.cs`)
2. Add the new class to the `PlayerClass` enum
3. Create a `CreateMageStats()` method in `PlayerStats.cs`
4. Update the `InitializeStatsForClass()` method in `PlayerController.cs`

### Adding New Abilities

1. Add ability parameters to the class ability script
2. Implement the ability logic in a coroutine
3. Add input handling in `HandleAbilityInput()`
4. Create visual and audio effects

### Modifying Stats

- All stats are centralized in `PlayerStats.cs`
- Stats automatically sync with components
- Level progression is handled automatically

## Best Practices

1. **Always use the PlayerController** as the main interface
2. **Subscribe to events** for UI updates rather than polling
3. **Use the state system** for animation triggers
4. **Test abilities** with the debug keys before implementing UI
5. **Keep stats balanced** - the system is designed to be easily tweaked

## Troubleshooting

### Common Issues

1. **Animations not playing**: Check Animator parameters match script calls
2. **Abilities not working**: Verify mana costs and cooldowns
3. **Stats not updating**: Ensure PlayerController is coordinating properly
4. **Movement issues**: Check Rigidbody2D settings and collision layers

### Performance Tips

1. Use object pooling for attack effects
2. Limit the number of active coroutines
3. Cache component references in Start()
4. Use events instead of Update() polling where possible

This system provides a solid foundation for your tower defense game and can be easily extended as your project grows!

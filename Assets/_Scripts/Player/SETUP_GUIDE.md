# Player Setup Guide - Single Script Solution

## 🎯 **Super Simple Setup**

### **Step 1: Add the Player Script**

1. Create your player GameObject
2. Add **ONLY** the `Player.cs` script to it
3. That's it! Everything else is automatic!

### **Step 2: Configure (Optional)**

The Player script will automatically add all necessary components, but you can customize:

#### **Required Components (Auto-Added):**

- ✅ Animator
- ✅ Rigidbody2D
- ✅ AudioSource
- ✅ PlayerStateMachine
- ✅ PlayerMovement
- ✅ PlayerAttack
- ✅ PlayerHealth
- ✅ KnightAbilities (if Knight class)

#### **Manual Setup (If Needed):**

1. **Animator Controller**: Assign your player's Animator Controller
2. **Attack Point**: Set the attack point transform (defaults to player transform)
3. **Enemy Layer**: Set the enemy layer mask for attacks
4. **Sprites**: Assign your player sprite to the SpriteRenderer

### **Step 3: Configure Settings**

All settings are exposed in the Player script inspector:

#### **Movement Settings:**

- Move Speed: How fast the player walks
- Jump Force: How high the player jumps
- Dash Speed: How fast the player dashes
- Dash Duration: How long the dash lasts

#### **Attack Settings:**

- Attack Range: How far the player can attack
- Attack Cooldown: Time between attacks
- Combo Window: Time to chain attacks

#### **Health Settings:**

- Max Health: Starting health
- Health Regen: Health restored per second
- Regen Delay: Delay before regen starts

#### **Knight Abilities:**

- Shield Bash: Range, damage, cooldown
- Defensive Stance: Duration, armor bonus, cooldown
- Whirlwind: Range, damage, cooldown

#### **Effects & Audio:**

- Assign particle effects and sound clips

## 🎮 **Controls**

### **Movement:**

- **A/D or Arrow Keys**: Move left/right
- **Space**: Jump
- **Left Shift**: Dash

### **Combat:**

- **Left Mouse Button**: Light Attack
- **Right Mouse Button**: Heavy Attack
- **Q**: Special Attack

### **Knight Abilities:**

- **E**: Shield Bash
- **R**: Defensive Stance
- **T**: Whirlwind

### **Debug (Remove in Production):**

- **L**: Gain 100 experience
- **H**: Heal 20 HP

## 🔧 **Advanced Usage**

### **Events:**

```csharp
Player player = GetComponent<Player>();

// Listen to events
player.OnStatsChanged += UpdateUI;
player.OnLevelUp += ShowLevelUpEffect;
player.OnHealthChanged += UpdateHealthBar;
player.OnPlayerDeath += HandleGameOver;
```

### **Equipment System:**

```csharp
// Apply weapon bonuses
player.ApplyEquipmentBonus(
    attackDamageBonus: 10f,
    magicDamageBonus: 5f,
    attackSpeedBonus: 0.1f,
    physicalArmorBonus: 3,
    magicArmorBonus: 2,
    criticalChanceBonus: 0.05f
);
```

### **Getting Player Info:**

```csharp
PlayerStats stats = player.GetStats();
int level = player.GetLevel();
PlayerState currentState = player.GetCurrentState();
```

## 🎨 **Animator Setup**

Your Animator Controller should have these boolean parameters:

- `isIdle`
- `isWalking`
- `isJumping`
- `isFalling`
- `isAttacking`
- `attack1`
- `attack2`
- `isDead`
- `isDashing`

## 🚀 **Benefits**

1. **One Script**: Only need to add Player.cs
2. **Auto-Setup**: All components added automatically
3. **Easy Configuration**: All settings in one place
4. **Event-Driven**: Easy UI integration
5. **Modular**: Can still access individual components if needed
6. **Scalable**: Easy to add new classes and abilities

## 🐛 **Troubleshooting**

### **Common Issues:**

1. **No Animator Controller**: Assign one in the Animator component
2. **No Attack Effects**: Assign particle effect prefabs
3. **No Sound**: Assign audio clips
4. **Enemies Not Taking Damage**: Set the enemy layer mask correctly

### **Performance Tips:**

1. Use object pooling for effects
2. Limit the number of active coroutines
3. Cache component references
4. Use events instead of Update() polling

This setup gives you a complete, professional player system with just one script!

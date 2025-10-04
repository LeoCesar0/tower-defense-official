# Character Data Architecture - Setup Guide

## 🎯 **New Architecture Overview**

The new character system uses **ScriptableObjects**, **Interfaces**, and **Data Types** to create a modular, reusable character system that works for players, enemies, and NPCs.

## 📁 **File Structure**

```
Assets/
├── _Data/
│   └── Character/
│       ├── CharacterData.cs          # Base character data ScriptableObject
│       ├── AbilityData.cs            # Ability data ScriptableObject
│       ├── EquipmentData.cs          # Equipment data ScriptableObject
│       ├── CharacterPreset.cs        # Complete character preset
│       └── CharacterFactory.cs       # Factory for creating characters
├── _Types/
│   └── CharacterInterfaces.cs        # All character interfaces
└── _Scripts/
    └── Character/
        ├── BaseCharacter.cs          # Base character implementation
        ├── PlayerCharacter.cs        # Player-specific implementation
        ├── CharacterStateMachine.cs  # Simple state machine
        └── EnemyCharacter.cs         # Enemy implementation (to be created)
```

## 🚀 **Quick Setup**

### **Step 1: Create Character Presets**

1. **Right-click in Project** → Create → Character → Character Preset
2. **Name it** (e.g., "Knight_Player")
3. **Configure the preset:**
   - Set Character Type: Player
   - Set Character Class: Knight
   - Assign Character Data
   - Add Abilities
   - Set Starting Equipment

### **Step 2: Create Character Data**

1. **Right-click in Project** → Create → Character → Character Data
2. **Name it** (e.g., "Knight_Data")
3. **Configure stats:**
   - Health, Mana, Attack Damage, etc.
   - Movement speeds
   - Defense stats
   - Assign sprites and sounds

### **Step 3: Create Abilities**

1. **Right-click in Project** → Create → Character → Ability Data
2. **Create abilities like:**
   - "Shield_Bash"
   - "Defensive_Stance"
   - "Whirlwind"
3. **Configure each ability:**
   - Cooldown, damage, range
   - Effects and sounds
   - Animation triggers

### **Step 4: Setup Player**

1. **Add PlayerNew script** to your player GameObject
2. **Assign the CharacterPreset** you created
3. **Done!** Everything else is automatic

## 🎮 **Usage Examples**

### **Creating a Player:**

```csharp
// In your game manager or spawner
CharacterPreset knightPreset = Resources.Load<CharacterPreset>("Knight_Player");
GameObject player = CharacterFactory.CreateCharacter(knightPreset, spawnPosition);
```

### **Creating an Enemy:**

```csharp
CharacterPreset zombiePreset = Resources.Load<CharacterPreset>("Zombie_Enemy");
GameObject zombie = CharacterFactory.CreateCharacter(zombiePreset, enemySpawnPoint);
```

### **Using the Character System:**

```csharp
// Get character component
var character = player.GetComponent<ICharacter>();

// Access stats
CharacterStats stats = character.GetStats();
int level = character.GetLevel();

// Use abilities
var abilities = character.GetAvailableAbilities();
character.UseAbility(abilities[0]); // Use first ability

// Apply equipment
EquipmentBonus bonus = new EquipmentBonus { attackDamageBonus = 10f };
character.ApplyEquipmentBonus(bonus);
```

## 🔧 **Creating Custom Characters**

### **1. Create Character Data:**

- Set base stats (health, damage, speed, etc.)
- Assign sprites and sounds
- Configure movement and combat parameters

### **2. Create Abilities:**

- Define ability effects and cooldowns
- Set damage, range, and special properties
- Assign visual and audio effects

### **3. Create Equipment:**

- Define stat bonuses
- Set equipment requirements
- Create equipment sets for bonuses

### **4. Create Character Preset:**

- Combine character data, abilities, and equipment
- Set AI behavior (for NPCs/enemies)
- Assign prefab reference

## 🎯 **Benefits of This System**

### **✅ Reusability:**

- Same data works for players, enemies, and NPCs
- Abilities can be shared between characters
- Equipment system works for all character types

### **✅ Modularity:**

- Each system is separate and focused
- Easy to add new character types
- Simple to modify existing characters

### **✅ Data-Driven:**

- Designers can create characters without coding
- Easy to balance and tweak values
- Version control friendly

### **✅ Extensibility:**

- Easy to add new interfaces
- Simple to create new character types
- Flexible ability and equipment systems

## 🐛 **Troubleshooting**

### **Common Issues:**

1. **"No player preset assigned"**

   - Make sure you've assigned a CharacterPreset in the PlayerNew script

2. **"Character data is null"**

   - Check that your CharacterPreset has CharacterData assigned

3. **"Animator is null"**

   - Make sure your character has an Animator component
   - Assign an Animator Controller

4. **"No abilities available"**
   - Check that your CharacterPreset has abilities assigned
   - Verify ability data is properly configured

### **Performance Tips:**

1. **Use Object Pooling** for frequently spawned characters
2. **Cache Component References** in Start() or Awake()
3. **Limit Active Abilities** to prevent performance issues
4. **Use Events** instead of Update() polling where possible

## 📚 **Next Steps**

1. **Create your first character preset**
2. **Test the system** with a simple player
3. **Add abilities** and test them
4. **Create enemy presets** for your tower defense game
5. **Implement equipment system** for progression

This architecture gives you a professional, scalable character system that's easy to use and extend!

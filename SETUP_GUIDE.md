# Game Setup Guide

## Quick Start - Making the Game Playable

### Step 1: Create Character Presets

1. In Unity Editor, go to **Game Setup > Create Default Character Presets**
2. This will create:
   - `Assets/_Data/Character/Presets/PlayerData.asset` (CharacterData)
   - `Assets/_Data/Character/Presets/DefaultPlayerPreset.asset` (CharacterPreset)
   - `Assets/_Data/Character/Presets/EnemyData.asset` (CharacterData)
   - `Assets/_Data/Character/Presets/DefaultEnemyPreset.asset` (CharacterPreset)

**Important Notes:**

- The presets created don't require a prefab reference for basic gameplay
- The `BaseCharacter` class automatically sets up all required components when you assign the preset
- If you see a validation error about missing prefab, you can ignore it for now (it's only needed if using `CreateCharacterInstance()`)

### Step 2: Setup Player GameObject

1. Create a new GameObject in your scene
2. Name it "Player"
3. Add the `GameSetupHelper` component to it
4. Right-click the component and select **"Setup Player GameObject"**
5. In the `Player` component, assign the `DefaultPlayerPreset` to the `playerPreset` field

### Step 3: Setup Enemy GameObject

1. Create a new GameObject in your scene
2. Name it "Enemy"
3. Add the `GameSetupHelper` component to it
4. Right-click the component and select **"Setup Enemy GameObject"**
   - This automatically adds `EnemyCharacter`, `EnemyAI`, and all required components
5. In the `EnemyCharacter` component, assign the `DefaultEnemyPreset` to the `characterPreset` field

### Step 4: Configure Tags (Automatic)

Tags are automatically set by the `GameSetupHelper`:

- Player GameObject gets "Player" tag
- Enemy GameObject gets "Enemy" tag

If tags don't exist, Unity will prompt you to create them. You can also create them manually:

1. Go to **Edit > Project Settings > Tags and Layers**
2. Add "Player" and "Enemy" tags if they don't exist

### Step 5: Test the Game

1. Press Play
2. Use **Arrow Keys** or **WASD** to move
3. Use **Left Click** or **Right Click** to attack
4. The enemy should automatically seek and attack you when in range

## Controls

- **Movement**: Arrow Keys or WASD (Horizontal)
- **Jump**: Space
- **Dash**: Left Shift
- **Attack**: Left Click or Right Click
- **Abilities**: E, R, T, Q (if configured)
- **Debug**: L (Gain Experience), H (Heal)

## Character Stats

### Default Player Stats

- Health: 100
- Attack Damage: 25
- Attack Speed: 1.5 attacks/second
- Attack Range: 1.5 units
- Move Speed: 6 units/second
- Physical Armor: 10

### Default Enemy Stats

- Health: 50
- Attack Damage: 15
- Attack Speed: 1 attack/second
- Attack Range: 1.2 units
- Move Speed: 4 units/second
- Physical Armor: 5
- Detection Range: 10 units

## Troubleshooting

### Enemy Not Attacking

- Check that enemy has `EnemyAI` component
- Check that enemy has `EnemyCharacter` component
- Verify enemy has a `CharacterPreset` assigned
- Check that player has `PlayerCharacter` component

### Player Can't Move

- Check that player has `Rigidbody2D` component
- Verify `Player` component has a preset assigned
- Check Input Manager settings (Edit > Project Settings > Input Manager)

### Attacks Not Working

- Ensure both characters have `Collider2D` components
- Check that characters are on the same physics layer
- Verify attack range is appropriate (check in CharacterPreset)

### Enemy Not Detecting Player

- Check detection range in `EnemyAI` component (default: 10 units)
- Verify player has `ITargetable` interface (via `PlayerCharacter`)
- Check that player's `CharacterType` is set to `Player`

## Next Steps

Once basic gameplay is working:

1. Add sprites to character presets
2. Create animations
3. Add sound effects
4. Create more enemy types
5. Add abilities and equipment

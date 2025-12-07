#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class CharacterPresetCreator
{
    [MenuItem("Game Setup/Create Default Character Presets")]
    public static void CreateDefaultPresets()
    {
        CreatePlayerPreset();
        CreateEnemyPreset();
        Debug.Log("Default character presets created successfully!");
    }
    
    private static void CreatePlayerPreset()
    {
        CharacterData playerData = ScriptableObject.CreateInstance<CharacterData>();
        playerData.characterName = "Player";
        playerData.characterType = CharacterType.Player;
        playerData.characterClass = CharacterClass.Knight;
        
        playerData.level = 1;
        playerData.maxHealth = 100;
        playerData.maxMana = 50;
        
        playerData.attackDamage = 25f;
        playerData.magicDamage = 10f;
        playerData.attackSpeed = 1.5f;
        playerData.attackRange = 1.5f;
        playerData.criticalChance = 0.1f;
        playerData.criticalMultiplier = 2f;
        
        playerData.physicalArmor = 10;
        playerData.magicArmor = 5;
        playerData.dodgeChance = 0.05f;
        playerData.blockChance = 0.1f;
        
        playerData.moveSpeed = 6f;
        playerData.jumpForce = 10f;
        playerData.dashSpeed = 12f;
        playerData.dashCooldown = 2f;
        
        playerData.healthRegen = 1f;
        playerData.manaRegen = 0.5f;
        playerData.regenDelay = 2f;
        
        string playerDataPath = "Assets/_Data/Character/Presets/PlayerData.asset";
        EnsureDirectoryExists(playerDataPath);
        AssetDatabase.CreateAsset(playerData, playerDataPath);
        
        CharacterPreset playerPreset = ScriptableObject.CreateInstance<CharacterPreset>();
        playerPreset.presetName = "Default Player";
        playerPreset.description = "Default player character preset";
        playerPreset.characterType = CharacterType.Player;
        playerPreset.characterClass = CharacterClass.Knight;
        playerPreset.characterData = playerData;
        playerPreset.abilities = new AbilityData[0];
        playerPreset.startingEquipment = new EquipmentData[0];
        
        string playerPresetPath = "Assets/_Data/Character/Presets/DefaultPlayerPreset.asset";
        EnsureDirectoryExists(playerPresetPath);
        AssetDatabase.CreateAsset(playerPreset, playerPresetPath);
        AssetDatabase.SaveAssets();
    }
    
    private static void CreateEnemyPreset()
    {
        CharacterData enemyData = ScriptableObject.CreateInstance<CharacterData>();
        enemyData.characterName = "Enemy";
        enemyData.characterType = CharacterType.Enemy;
        enemyData.characterClass = CharacterClass.None;
        
        enemyData.level = 1;
        enemyData.maxHealth = 50;
        enemyData.maxMana = 20;
        
        enemyData.attackDamage = 15f;
        enemyData.magicDamage = 5f;
        enemyData.attackSpeed = 1f;
        enemyData.attackRange = 1.2f;
        enemyData.criticalChance = 0.05f;
        enemyData.criticalMultiplier = 1.5f;
        
        enemyData.physicalArmor = 5;
        enemyData.magicArmor = 3;
        enemyData.dodgeChance = 0.02f;
        enemyData.blockChance = 0.05f;
        
        enemyData.moveSpeed = 4f;
        enemyData.jumpForce = 8f;
        enemyData.dashSpeed = 8f;
        enemyData.dashCooldown = 3f;
        
        enemyData.healthRegen = 0f;
        enemyData.manaRegen = 0f;
        enemyData.regenDelay = 0f;
        
        string enemyDataPath = "Assets/_Data/Character/Presets/EnemyData.asset";
        EnsureDirectoryExists(enemyDataPath);
        AssetDatabase.CreateAsset(enemyData, enemyDataPath);
        
        CharacterPreset enemyPreset = ScriptableObject.CreateInstance<CharacterPreset>();
        enemyPreset.presetName = "Default Enemy";
        enemyPreset.description = "Default enemy character preset";
        enemyPreset.characterType = CharacterType.Enemy;
        enemyPreset.characterClass = CharacterClass.None;
        enemyPreset.characterData = enemyData;
        enemyPreset.abilities = new AbilityData[0];
        enemyPreset.startingEquipment = new EquipmentData[0];
        
        AIData aiData = new AIData();
        aiData.behaviorType = AIBehaviorType.Aggressive;
        aiData.detectionRange = 10f;
        aiData.attackRange = 1.5f;
        aiData.moveSpeed = 4f;
        aiData.canAttack = true;
        aiData.attackCooldown = 1f;
        aiData.canUseAbilities = false;
        aiData.isAggressive = true;
        aiData.aggroRange = 10f;
        aiData.aggroDuration = 5f;
        aiData.canLoseAggro = false;
        enemyPreset.aiData = aiData;
        
        string enemyPresetPath = "Assets/_Data/Character/Presets/DefaultEnemyPreset.asset";
        EnsureDirectoryExists(enemyPresetPath);
        AssetDatabase.CreateAsset(enemyPreset, enemyPresetPath);
        AssetDatabase.SaveAssets();
    }
    
    private static void EnsureDirectoryExists(string filePath)
    {
        string directory = System.IO.Path.GetDirectoryName(filePath);
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }
    }
}
#endif

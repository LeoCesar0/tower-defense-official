using UnityEngine;

/// <summary>
/// Factory class for creating characters from presets
/// </summary>
public static class CharacterFactory
{
    /// <summary>
    /// Create a character from a preset
    /// </summary>
    public static GameObject CreateCharacter(CharacterPreset preset, Vector3 position, Transform parent = null)
    {
        if (preset == null)
        {
            Debug.LogError("Character preset is null");
            return null;
        }
        
        if (!preset.IsValid())
        {
            Debug.LogError($"Character preset is invalid: {preset.presetName}");
            return null;
        }
        
        GameObject character = preset.CreateCharacterInstance(parent);
        if (character != null)
        {
            character.transform.position = position;
            
            // Initialize the character
            var characterComponent = character.GetComponent<ICharacter>();
            if (characterComponent != null)
            {
                characterComponent.InitializeFromPreset(preset);
            }
        }
        
        return character;
    }
    
    /// <summary>
    /// Create a character from character data (for runtime creation)
    /// </summary>
    public static GameObject CreateCharacter(CharacterData characterData, Vector3 position, Transform parent = null)
    {
        if (characterData == null)
        {
            Debug.LogError("Character data is null");
            return null;
        }
        
        // Create a basic character GameObject
        GameObject character = new GameObject(characterData.characterName);
        if (parent != null)
            character.transform.SetParent(parent);
        
        character.transform.position = position;
        
        // Add required components based on character type
        AddRequiredComponents(character, characterData);
        
        // Initialize the character
        var characterComponent = character.GetComponent<ICharacter>();
        if (characterComponent != null)
        {
            // Create a temporary preset for initialization
            var tempPreset = ScriptableObject.CreateInstance<CharacterPreset>();
            tempPreset.presetName = characterData.characterName;
            tempPreset.characterData = characterData;
            tempPreset.characterType = characterData.characterType;
            tempPreset.characterClass = characterData.characterClass;
            
            characterComponent.InitializeFromPreset(tempPreset);
        }
        
        return character;
    }
    
    /// <summary>
    /// Add required components based on character data
    /// </summary>
    private static void AddRequiredComponents(GameObject character, CharacterData characterData)
    {
        // Add basic components
        character.AddComponent<SpriteRenderer>();
        character.AddComponent<Rigidbody2D>();
        character.AddComponent<Collider2D>();
        character.AddComponent<AudioSource>();
        
        // Add animator if controller is provided
        if (characterData.animatorController != null)
        {
            var animator = character.AddComponent<Animator>();
            animator.runtimeAnimatorController = characterData.animatorController;
        }
        
        // Add character-specific components based on type
        switch (characterData.characterType)
        {
            case CharacterType.Player:
                character.AddComponent<PlayerCharacter>();
                break;
            case CharacterType.Enemy:
                character.AddComponent<EnemyCharacter>();
                break;
            case CharacterType.NPC:
                character.AddComponent<NPCCharacter>();
                break;
            case CharacterType.Boss:
                character.AddComponent<BossCharacter>();
                break;
        }
        
        // Add state machine
        character.AddComponent<CharacterStateMachine>();
    }
    
    /// <summary>
    /// Create a random character of a specific type
    /// </summary>
    public static GameObject CreateRandomCharacter(CharacterType characterType, Vector3 position, Transform parent = null)
    {
        // This would load a random preset of the specified type
        // For now, we'll create a basic character
        var characterData = ScriptableObject.CreateInstance<CharacterData>();
        characterData.characterType = characterType;
        characterData.characterName = $"Random {characterType}";
        
        return CreateCharacter(characterData, position, parent);
    }
}

/// <summary>
/// Character spawner component for easy character creation in scenes
/// </summary>
public class CharacterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public CharacterPreset[] characterPresets;
    public Transform spawnPoint;
    public bool spawnOnStart = false;
    public float spawnDelay = 0f;
    
    [Header("Spawn Limits")]
    public int maxCharacters = 10;
    public float spawnRadius = 5f;
    
    private int currentCharacterCount = 0;
    
    void Start()
    {
        if (spawnOnStart)
        {
            if (spawnDelay > 0)
                Invoke(nameof(SpawnRandomCharacter), spawnDelay);
            else
                SpawnRandomCharacter();
        }
    }
    
    /// <summary>
    /// Spawn a random character from the available presets
    /// </summary>
    public void SpawnRandomCharacter()
    {
        if (characterPresets == null || characterPresets.Length == 0)
        {
            Debug.LogWarning("No character presets available for spawning");
            return;
        }
        
        if (currentCharacterCount >= maxCharacters)
        {
            Debug.LogWarning("Maximum character count reached");
            return;
        }
        
        var randomPreset = characterPresets[Random.Range(0, characterPresets.Length)];
        Vector3 spawnPosition = GetSpawnPosition();
        
        GameObject character = CharacterFactory.CreateCharacter(randomPreset, spawnPosition, transform);
        if (character != null)
        {
            currentCharacterCount++;
            
            // Listen for character death to update count
            var damageable = character.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // You could set up a death event listener here
            }
        }
    }
    
    /// <summary>
    /// Spawn a specific character preset
    /// </summary>
    public void SpawnCharacter(CharacterPreset preset)
    {
        if (preset == null)
        {
            Debug.LogWarning("Character preset is null");
            return;
        }
        
        if (currentCharacterCount >= maxCharacters)
        {
            Debug.LogWarning("Maximum character count reached");
            return;
        }
        
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject character = CharacterFactory.CreateCharacter(preset, spawnPosition, transform);
        if (character != null)
        {
            currentCharacterCount++;
        }
    }
    
    /// <summary>
    /// Get a random spawn position within the spawn radius
    /// </summary>
    private Vector3 GetSpawnPosition()
    {
        Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
        
        if (spawnRadius > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            basePosition += new Vector3(randomOffset.x, randomOffset.y, 0);
        }
        
        return basePosition;
    }
    
    /// <summary>
    /// Called when a character dies to update the count
    /// </summary>
    public void OnCharacterDied()
    {
        currentCharacterCount = Mathf.Max(0, currentCharacterCount - 1);
    }
    
    void OnDrawGizmosSelected()
    {
        if (spawnRadius > 0)
        {
            Gizmos.color = Color.green;
            Vector3 center = spawnPoint != null ? spawnPoint.position : transform.position;
            Gizmos.DrawWireSphere(center, spawnRadius);
        }
    }
}

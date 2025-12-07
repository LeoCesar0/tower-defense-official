using UnityEngine;

public class GameSetupHelper : MonoBehaviour
{
    [ContextMenu("Setup Player GameObject")]
    public void SetupPlayerGameObject()
    {
        GameObject player = gameObject;
        
        if (player.GetComponent<Player>() == null)
        {
            player.AddComponent<Player>();
        }
        
        if (player.GetComponent<PlayerCharacter>() == null)
        {
            player.AddComponent<PlayerCharacter>();
        }
        
        if (player.GetComponent<CharacterStateMachine>() == null)
        {
            player.AddComponent<CharacterStateMachine>();
        }
        
        if (player.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.gravityScale = 3f;
        }
        
        if (player.GetComponent<Collider2D>() == null)
        {
            CapsuleCollider2D collider = player.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.8f, 1.6f);
            collider.offset = new Vector2(0f, 0.8f);
        }
        
        if (player.GetComponent<AudioSource>() == null)
        {
            player.AddComponent<AudioSource>();
        }
        
        player.tag = "Player";
        
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = player.AddComponent<SpriteRenderer>();
            spriteRenderer.color = Color.blue;
        }
        
        Debug.Log($"Player GameObject '{player.name}' setup complete!");
    }
    
    [ContextMenu("Setup Enemy GameObject")]
    public void SetupEnemyGameObject()
    {
        GameObject enemy = gameObject;
        
        if (enemy.GetComponent<EnemyCharacter>() == null)
        {
            enemy.AddComponent<EnemyCharacter>();
        }
        
        if (enemy.GetComponent<EnemyAI>() == null)
        {
            enemy.AddComponent<EnemyAI>();
        }
        
        if (enemy.GetComponent<CharacterStateMachine>() == null)
        {
            enemy.AddComponent<CharacterStateMachine>();
        }
        
        if (enemy.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.gravityScale = 3f;
        }
        
        if (enemy.GetComponent<Collider2D>() == null)
        {
            CapsuleCollider2D collider = enemy.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.8f, 1.6f);
            collider.offset = new Vector2(0f, 0.8f);
        }
        
        if (enemy.GetComponent<AudioSource>() == null)
        {
            enemy.AddComponent<AudioSource>();
        }
        
        enemy.tag = "Enemy";
        
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = enemy.AddComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
        }
        
        Debug.Log($"Enemy GameObject '{enemy.name}' setup complete!");
    }
}

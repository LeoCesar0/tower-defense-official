using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class EnemyMovement : MonoBehaviour
{
    private BaseEnemy baseEnemy; // Reference to the BaseEnemy component
    private float currentSpeed; // Stores the current speed based on enemy stats

    [Header("Target Settings")]
    public string targetTag = "Target"; // Tag for main target to follow
    public string playerLikeTag = "PlayerLike"; // Tag for secondary targets to attack if encountered

    private Transform target; // Main target to follow
    private Transform playerLikeTarget; // Secondary target if encountered

    private EnemyStats stats;
    private Rigidbody2D rb;

    private void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>(); // Get the BaseEnemy component to access stats
        rb = GetComponent<Rigidbody2D>();
        stats = baseEnemy.stats; // Initialize speed from enemy stats

        // Find main target by tag
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }

    private void Update()
    {
        currentSpeed = stats.walkSpeed;
        // Check for PlayerLike object in range
        CheckForPlayerLike();

        // Move towards the appropriate target
        if (playerLikeTarget != null)
        {
            MoveTowards(playerLikeTarget);
        }
        else if (target != null)
        {
            MoveTowards(target);
        }
    }

    // Function to move towards a specific target
    private void MoveTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;
        transform.LookAt(target); // Face the target
    }

    // Check if a PlayerLike object is in the enemy's path
    private void CheckForPlayerLike()
    {
        // This example uses a simple range check to find the closest PlayerLike target.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(playerLikeTag))
            {
                playerLikeTarget = hitCollider.transform;
                currentSpeed = baseEnemy.stats.runSpeed; // Switch to runSpeed from stats if chasing
                return;
            }
        }
        // Reset to walkSpeed from stats if no PlayerLike is in range
        playerLikeTarget = null;
        currentSpeed = baseEnemy.stats.walkSpeed;
    }
}

using UnityEngine;

public class SkeletonController : BaseEnemy
{
    protected override void InitializeStats()
    {
        base.InitializeStats();
        stats = SkeletonStats.Stats;
        Debug.Log("Skeleton initialized with unique stats.");
    }
}

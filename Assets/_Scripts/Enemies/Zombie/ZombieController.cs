using UnityEngine;

public class ZombieController : BaseEnemy
{
    protected override void InitializeStats()
    {
        base.InitializeStats();
        stats = ZombieStats.Stats;
        Debug.Log("Zombie initialized with unique stats.");
    }

}

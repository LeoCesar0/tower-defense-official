using Unity.Entities;
using Unity.Mathematics;

public struct DashComponent : IComponentData
{
    public bool isDashing;
    public float dashSpeed;
    public float dashTime;
    public float dashDuration;
    public float2 dashDirection;
}

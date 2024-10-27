using Unity.Entities;
using Unity.Mathematics;

public struct MovementComponent : IComponentData
{
    public float2 velocity;
    public float moveSpeed;
    public float jumpForce;
}

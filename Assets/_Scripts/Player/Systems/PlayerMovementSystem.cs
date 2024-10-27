using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref MovementComponent movement, in PlayerInputComponent input, in PlayerStateComponent state) =>
        {
            // Handle horizontal movement
            if (!state.isDashing)
            {
                movement.velocity.x = input.moveDirectionX * movement.moveSpeed;
            }

        }).Run();
    }
}

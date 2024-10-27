using Unity.Entities;

public partial class PlayerStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PlayerStateComponent state, in MovementComponent movement) =>
        {
            // Update state based on velocity and grounded status
            state.isWalking = movement.velocity.x != 0;
            state.isFalling = movement.velocity.y < 0 && !state.isGrounded;
            state.isIdle = !state.isWalking && !state.isJumping && !state.isFalling && !state.isDashing;

        }).Run();
    }
}

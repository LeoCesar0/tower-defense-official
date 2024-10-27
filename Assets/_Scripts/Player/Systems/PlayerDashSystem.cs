using Unity.Entities;
using Unity.Mathematics;

public partial class PlayerDashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref DashComponent dash, ref MovementComponent movement, ref PlayerStateComponent state, in PlayerInputComponent input) =>
        {
            if (input.dashPressed && !dash.isDashing)
            {
                dash.isDashing = true;
                dash.dashTime = dash.dashDuration;
                dash.dashDirection = new float2(input.moveDirectionX, 0);
            }

            if (dash.isDashing)
            {
                movement.velocity = dash.dashDirection * dash.dashSpeed;
                dash.dashTime -= deltaTime;

                if (dash.dashTime <= 0)
                {
                    dash.isDashing = false;
                }
            }
        }).Run();
    }
}

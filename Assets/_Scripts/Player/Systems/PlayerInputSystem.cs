using Unity.Entities;
using UnityEngine;

public partial class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        bool jump = Input.GetButtonDown("Jump");
        bool dash = Input.GetKeyDown(KeyCode.LeftShift);
        float moveX = Input.GetAxisRaw("Horizontal");

        Entities.ForEach((ref PlayerInputComponent input) =>
        {
            input.jumpPressed = jump;
            input.dashPressed = dash;
            input.moveDirectionX = moveX;
        }).Run();
    }
}

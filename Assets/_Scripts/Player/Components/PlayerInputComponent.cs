using Unity.Entities;

public struct PlayerInputComponent : IComponentData
{
    public bool jumpPressed;
    public bool dashPressed;
    public float moveDirectionX;
}

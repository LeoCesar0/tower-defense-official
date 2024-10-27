using Unity.Entities;

public struct PlayerStateComponent : IComponentData
{
    public bool isWalking;
    public bool isJumping;
    public bool isFalling;
    public bool isAttacking;
    public bool isIdle;
    public bool isDashing;
    public bool isGrounded;
    public bool isDead;
}

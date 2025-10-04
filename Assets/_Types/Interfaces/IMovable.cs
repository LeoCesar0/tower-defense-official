using UnityEngine;

/// <summary>
/// Interface for any character that can move
/// </summary>
public interface IMovable
{
    void Move(Vector2 direction);
    void Jump();
    void Dash(Vector2 direction);
    bool IsGrounded();
    float GetMoveSpeed();
    Vector2 GetVelocity();
}

using UnityEngine;

/// <summary>
/// Interface for any character that can be targeted
/// </summary>
public interface ITargetable
{
    Transform GetTransform();
    Vector3 GetTargetPosition();
    bool IsValidTarget();
    CharacterType GetCharacterType();
}

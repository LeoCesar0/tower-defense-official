/// <summary>
/// Interface for any character that can be controlled by player input
/// </summary>
public interface IControllable
{
    void SetInputEnabled(bool enabled);
    bool IsInputEnabled();
    void HandleInput();
}

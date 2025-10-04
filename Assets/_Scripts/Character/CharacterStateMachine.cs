using UnityEngine;

/// <summary>
/// Simple state machine for character states
/// </summary>
public class CharacterStateMachine : MonoBehaviour
{
    [Header("State Machine")]
    public CharacterState currentState = CharacterState.Idle;
    public CharacterState previousState = CharacterState.Idle;
    
    // Events
    public System.Action<CharacterState> OnStateChanged;
    
    void Start()
    {
        // Initialize state
        OnStateChanged?.Invoke(currentState);
    }
    
    /// <summary>
    /// Change to a new state
    /// </summary>
    public void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;
        
        previousState = currentState;
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }
    
    /// <summary>
    /// Get the current state
    /// </summary>
    public CharacterState GetCurrentState() => currentState;
    
    /// <summary>
    /// Get the previous state
    /// </summary>
    public CharacterState GetPreviousState() => previousState;
    
    /// <summary>
    /// Check if currently in a specific state
    /// </summary>
    public bool IsInState(CharacterState state) => currentState == state;
    
    /// <summary>
    /// Check if currently in any of the specified states
    /// </summary>
    public bool IsInAnyState(params CharacterState[] states)
    {
        foreach (var state in states)
        {
            if (currentState == state) return true;
        }
        return false;
    }
}

using System.Collections;
using UnityEngine;

// Alternative State Machine Approach
public class PlayerStateMachine : MonoBehaviour
{
    [Header("State Machine Settings")]
    public float stateTransitionTime = 0.1f;
    
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    
    // Current state
    private PlayerState currentState;
    private PlayerState previousState;
    
    // State instances
    private IdleState idleState;
    private WalkingState walkingState;
    private JumpingState jumpingState;
    private FallingState fallingState;
    private AttackingState attackingState;
    private DashingState dashingState;
    private DeadState deadState;
    
    // Events
    public System.Action<PlayerState> OnStateChanged;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        
        // Initialize states
        InitializeStates();
        
        // Start with idle state
        ChangeState(idleState);
    }
    
    void Update()
    {
        currentState?.Update();
    }
    
    void InitializeStates()
    {
        idleState = new IdleState(this);
        walkingState = new WalkingState(this);
        jumpingState = new JumpingState(this);
        fallingState = new FallingState(this);
        attackingState = new AttackingState(this);
        dashingState = new DashingState(this);
        deadState = new DeadState(this);
    }
    
    public void ChangeState(PlayerState newState)
    {
        if (newState == currentState) return;
        
        previousState = currentState;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
        
        OnStateChanged?.Invoke(currentState);
    }
    
    // Public getters for states
    public PlayerState GetCurrentState() => currentState;
    public PlayerState GetPreviousState() => previousState;
    
    // Component getters
    public Animator GetAnimator() => animator;
    public PlayerMovement GetPlayerMovement() => playerMovement;
    public PlayerAttack GetPlayerAttack() => playerAttack;
    public Rigidbody2D GetRigidbody() => rb;
    
    // State transition methods
    public void TryTransitionToIdle()
    {
        if (CanTransitionTo(idleState))
            ChangeState(idleState);
    }
    
    public void TryTransitionToWalking()
    {
        if (CanTransitionTo(walkingState))
            ChangeState(walkingState);
    }
    
    public void TryTransitionToJumping()
    {
        if (CanTransitionTo(jumpingState))
            ChangeState(jumpingState);
    }
    
    public void TryTransitionToFalling()
    {
        if (CanTransitionTo(fallingState))
            ChangeState(fallingState);
    }
    
    public void TryTransitionToAttacking()
    {
        if (CanTransitionTo(attackingState))
            ChangeState(attackingState);
    }
    
    public void TryTransitionToDashing()
    {
        if (CanTransitionTo(dashingState))
            ChangeState(dashingState);
    }
    
    public void TryTransitionToDead()
    {
        if (CanTransitionTo(deadState))
            ChangeState(deadState);
    }
    
    private bool CanTransitionTo(PlayerState targetState)
    {
        return currentState?.CanTransitionTo(targetState) ?? true;
    }
}

// Base State Class
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Animator animator;
    protected PlayerMovement playerMovement;
    protected PlayerAttack playerAttack;
    protected Rigidbody2D rb;
    
    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.animator = stateMachine.GetAnimator();
        this.playerMovement = stateMachine.GetPlayerMovement();
        this.playerAttack = stateMachine.GetPlayerAttack();
        this.rb = stateMachine.GetRigidbody();
    }
    
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual bool CanTransitionTo(PlayerState targetState) => true;
}

// Concrete State Implementations
public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isIdle", true);
        animator?.SetBool("isWalking", false);
    }
    
    public override void Update()
    {
        // Check for transitions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            stateMachine.TryTransitionToWalking();
        }
        else if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            stateMachine.TryTransitionToJumping();
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            stateMachine.TryTransitionToAttacking();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.TryTransitionToDashing();
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isIdle", false);
    }
    
    private bool IsGrounded()
    {
        // Implement ground check logic
        return true; // Simplified for example
    }
}

public class WalkingState : PlayerState
{
    public WalkingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isWalking", true);
        animator?.SetBool("isIdle", false);
    }
    
    public override void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            stateMachine.TryTransitionToIdle();
        }
        else if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            stateMachine.TryTransitionToJumping();
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            stateMachine.TryTransitionToAttacking();
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isWalking", false);
    }
    
    private bool IsGrounded()
    {
        return true; // Simplified for example
    }
}

public class JumpingState : PlayerState
{
    public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isJumping", true);
        animator?.SetBool("isFalling", false);
    }
    
    public override void Update()
    {
        if (rb.velocity.y < 0)
        {
            stateMachine.TryTransitionToFalling();
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isJumping", false);
    }
}

public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isFalling", true);
        animator?.SetBool("isJumping", false);
    }
    
    public override void Update()
    {
        if (IsGrounded())
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                stateMachine.TryTransitionToWalking();
            }
            else
            {
                stateMachine.TryTransitionToIdle();
            }
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isFalling", false);
    }
    
    private bool IsGrounded()
    {
        return true; // Simplified for example
    }
}

public class AttackingState : PlayerState
{
    private float attackDuration = 0.5f;
    private float attackTimer;
    
    public AttackingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isAttacking", true);
        attackTimer = attackDuration;
    }
    
    public override void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            stateMachine.TryTransitionToIdle();
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isAttacking", false);
    }
    
    public override bool CanTransitionTo(PlayerState targetState)
    {
        // Can't transition while attacking
        return false;
    }
}

public class DashingState : PlayerState
{
    private float dashDuration = 0.2f;
    private float dashTimer;
    
    public DashingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isDashing", true);
        dashTimer = dashDuration;
    }
    
    public override void Update()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            stateMachine.TryTransitionToIdle();
        }
    }
    
    public override void Exit()
    {
        animator?.SetBool("isDashing", false);
    }
}

public class DeadState : PlayerState
{
    public DeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        animator?.SetBool("isDead", true);
    }
    
    public override void Update()
    {
        // Dead state - no transitions allowed
    }
    
    public override bool CanTransitionTo(PlayerState targetState)
    {
        // Can only transition from dead state through respawn
        return false;
    }
}

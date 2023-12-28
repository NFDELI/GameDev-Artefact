using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    // State Machine States.
    PlayerBaseState currentState;
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerWalkingState WalkingState = new PlayerWalkingState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerCrouchState CrouchState = new PlayerCrouchState();
    public PlayerDeathState DeathState = new PlayerDeathState();
    public PlayerRegularAttackState RegularAttackState = new PlayerRegularAttackState();
    public PlayerSpecialAttackState PlayerSpecialAttackState = new PlayerSpecialAttackState();

    // Variables
    public Vector2 movementInput;
    public Rigidbody2D rb;
    public Rigidbody2D bossRb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isLanded = true;
    public bool spriteFlip = false;
    public int attackCounter = 1;
    public bool canAttackChain = false;

    // Player Attributes
    public float health = 100.0f;
    public float movementSpeed = 1.0f;
    public float jumpForce = 2.0f;
    public float diagonalJumpForce = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentState = IdleState;

        currentState.EnterState(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        if(rb.position.x < bossRb.position.x)
        {
            // Player is on the left of the boss.
            spriteFlip = true;
        }
        else
        {
            // Player is on the right of the boss.
            spriteFlip = false;
        }

        if(health <= 0 && currentState != DeathState)
        {
            // Player loses all HP and Dies.
            currentState = DeathState;
            currentState.EnterState(this);

            // Boss wins one round.
        }

        // Debugging
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            health = 0;
        }
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void FlagAttackAnimationFinished()
    {
        // This function is used in the Animator.
        currentState = IdleState;
        currentState.EnterState(this);
    }

    public void FlagAttackAnimationFinishedCrouched()
    {
        // This function is used in the Animator.
        currentState = CrouchState;
        currentState.EnterState(this);
    }

    public void FlagAttackAnimationChain()
    {
        // This function is used in the Animator to cancel attack animation quickly when attacks lands.
        canAttackChain = true;
    }
}

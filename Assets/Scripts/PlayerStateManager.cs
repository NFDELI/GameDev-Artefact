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

    // Variables
    public Vector2 movementInput;
    public Rigidbody2D rb;
    public Rigidbody2D bossRb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float movementSpeed = 1.0f;
    public float jumpForce = 2.0f;
    public float diagonalJumpForce = 2.0f;
    public bool isLanded = true;
    public bool spriteFlip = false;

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
}

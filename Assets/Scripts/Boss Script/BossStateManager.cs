using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossStateManager : MonoBehaviour
{
    // State Machine States.
    BossBaseState currentState;
    public BossIdleState IdleState = new BossIdleState();
    public BossWalkingState WalkingState = new BossWalkingState();
    public BossJumpingState JumpingState = new BossJumpingState();
    public BossCrouchState CrouchState = new BossCrouchState();
    public BossDeathState DeathState = new BossDeathState();
    public BossRegularAttackState RegularAttackState = new BossRegularAttackState();
    public BossSpecialAttackState BossSpecialAttackState = new BossSpecialAttackState();
    public BossHitReactionState HitReactionState = new BossHitReactionState();
    
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
    public int forceDirection = 1;
    public bool canGetUp = false;

    // Boss Attributes
    public float health = 100.0f;
    public float movementSpeed = 1.0f;
    public float jumpForce = 2.0f;
    public float diagonalJumpForce = 2.0f;

    // Variables for Hit Reaction. (These variables should be set from the boss's Script)
    public int nextBossHitReaction = 0;
    public float nextBossHitStunDuration = 1.0f;
    public float nextBossDamageReceived = 0f;
    public Vector2 nextBossForceReceived = new Vector2(0, 0);

    // Boss Attack Variables.
    public bool isSpinnigKickForce = false;
    public float spinningKickSpeed = 1f;

    // Reference to Boss's fireball.
    public FireballScript fireballScript;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        if(rb.position.x < bossRb.position.x)
        {
            // Boss is on the left of the boss.
            spriteFlip = true;
        }
        else
        {
            // Boss is on the right of the boss.
            spriteFlip = false;
        }

        // Need to Move this Health check during damage check.
        if(health <= 0 && currentState != DeathState)
        {
            // Boss loses all HP and Dies.
            currentState = DeathState;
            currentState.EnterState(this);

            // Boss wins one round.
        }

        // Ensures that the attack force is applied in the correct Direction.
        if (!spriteRenderer.flipX)
        {
            forceDirection = -1;
        }
        else
        {
            forceDirection = 1;
        }

        // Debugging
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            health = 0;
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            currentState = HitReactionState;
            nextBossHitReaction = 0;
            currentState.EnterState(this);
        }
    }

    public void SwitchState(BossBaseState state)
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

    public void FlagDragonPunchForce()
    {
        // Used in the Animator, this function adds a force to push the player character upwards.
        rb.AddForce(new Vector2(1 * forceDirection, 6), ForceMode2D.Impulse);
    }

    public void FlagSpinningKickStart()
    {
        isSpinnigKickForce = true;  
    }

    public void FlagSpinningKickEnd()
    {
        isSpinnigKickForce = false;
    }
    
    public void FlagAllowBossToGetUp()
    {
        canGetUp = true;
    }

    public void SpawnFireball()
    {
        fireballScript.rb.position = new Vector2(rb.position.x + (0.8f * forceDirection), rb.position.y + 0.4f);
        fireballScript.FireballSpawned();
    }
}

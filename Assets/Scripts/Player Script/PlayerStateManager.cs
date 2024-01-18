using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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
    public PlayerSpecialAttackState SpecialAttackState = new PlayerSpecialAttackState();
    public PlayerHitReactionState HitReactionState = new PlayerHitReactionState();
    public PlayerParryAttemptState ParryAttemptState = new PlayerParryAttemptState();

    // Audio Script
    public AudioScript audioScript;
    public int nextPlayerHitSoundIndex; // When the player GETS hit.
    
    // Variables
    public Vector2 movementInput;
    public Rigidbody2D rb;
    public Rigidbody2D bossRb;
    public BoxCollider2D attackBoxCollider;
    public BoxCollider2D playerBoxCollider2D;
    public BoxCollider2D playerLandCollider2D;
    public BoxCollider2D playerAirCollider2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isLanded = true;
    public bool spriteFlip = false;
    public int attackCounter = 1;
    public bool canAttackChain = false;
    public int forceDirection = 1;
    public bool canGetUp = false;
    public bool isInvincible = false;
    public bool wasBlocking = false;

    // Player Attributes
    public float health = 100.0f;
    public float postureDefault;
    public float postureCurrent;
    public float movementSpeed = 1.0f;
    public float jumpForce = 2.0f;
    public float diagonalJumpForce = 2.0f;

    // Variables for Hit Reaction. (These variables should be set from the boss's Script)
    public int nextPlayerHitReaction = 0;
    public float nextPlayerHitStunDuration = 1.0f;
    public float nextPlayerDamageReceived = 0f;
    public Vector2 nextPlayerForceReceived = new Vector2(0, 0);

    // Player Attack Variables.
    public bool isSpinnigKickForce = false;
    public float spinningKickSpeed = 0.75f;

    // Reference to Player's fireball.
    public FireballScript fireballScript;

    // Reference to Boss's State Manager.
    public BossStateManager bossStateManager;

    // New Input System Varaibles.
    private PlayerInputActions input = null;

    public bool isLanding = false;
    public bool jumpDirection = false;
    public bool isCloseToWallLeft = false;
    public bool isCloseToWallRight = false;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Parry.performed += OnParryPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Parry.performed -= OnParryPerformed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentState = IdleState;

        postureCurrent = postureDefault;

        currentState.EnterState(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(this, collision);
        if(collision.tag == "BossRangeCheck")
        {
            // The player is in range of boss's attack.
            bossStateManager.hasReachedPlayer = true;
            bossStateManager.animator.SetBool("isWalkTowards", false);
            bossStateManager.animator.SetBool("isWalkBackwards", false);
            bossStateManager.SwitchState(bossStateManager.RegularAttackState);
        }
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

        // Ensures that the attack force is applied in the correct Direction.
        if(!spriteRenderer.flipX)
        {
            forceDirection = -1;
            playerBoxCollider2D.offset = new Vector2(-0.053f, -0.78f);
        }
        else
        {
            forceDirection = 1;
            playerBoxCollider2D.offset = new Vector2(0.053f, -0.78f);
        }

        // Debugging
        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    health = 0;
        //}
        //
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    currentState = HitReactionState;
        //    nextPlayerHitReaction = 0;
        //    currentState.EnterState(this);
        //}
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
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
        rb.AddForce(new Vector2(2f * forceDirection, 6), ForceMode2D.Impulse);
    }

    public void FlagSpinningKickStart()
    {
        isSpinnigKickForce = true;  
    }

    public void FlagSpinningKickEnd()
    {
        isSpinnigKickForce = false;
    }
    
    public void FlagAllowPlayerToGetUp()
    {
        canGetUp = true;
    }

    public void SpawnFireball()
    {
        fireballScript.rb.position = new Vector2(rb.position.x + (0.8f * forceDirection), rb.position.y + 0.4f);
        audioScript.PlayFireballShotSound();
        fireballScript.FireballSpawned();
    }

    public void TurnOnAttackBoxCollider()
    {
        if(spriteRenderer.flipX)
        {
            attackBoxCollider.offset = new Vector2(0.95f, 0f);
        }
        else
        {
            attackBoxCollider.offset = new Vector2(0f, 0f);
        }

        attackBoxCollider.enabled = true;
    }

    public void TurnOffAttackBoxCollider()
    {
        // Teleport the Collision box to another place.
        attackBoxCollider.offset = new Vector2(999f, 999f);
        attackBoxCollider.enabled = false;
    }

    public void TurnOninvincibility()
    {
        isInvincible = true;
    }

    public void TurnOffinvincibility()
    {
        isInvincible = false;
    }

    public void AttackHitProperty(float damage, Vector2 force, int hitreactionId, float stunduration, int hitsoundId)
    {
        bossStateManager.nextBossDamageReceived = damage;
        bossStateManager.nextBossForceReceived = force;
        bossStateManager.nextBossForceReceived.x *= forceDirection;
        bossStateManager.nextBossHitReaction = hitreactionId;
        bossStateManager.nextBossHitStunDuration = stunduration;
        bossStateManager.nextBossHitSoundIndex = hitsoundId;
    }

    public void AttackHitPropertySelf(float damage, Vector2 force, int hitreactionId, float stunduration, int hitsoundId)
    {
        nextPlayerDamageReceived = damage;
        nextPlayerForceReceived = force * (forceDirection);
        nextPlayerHitReaction = hitreactionId;
        nextPlayerHitStunDuration = stunduration;
        nextPlayerHitSoundIndex = hitsoundId;
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void OnParryPerformed(InputAction.CallbackContext value)
    {
        currentState.OnParryPerformed(this);
    }

    public void PlayerLandingTrue()
    {
        //if(spriteFlip)
        //{
        //    jumpDirection = true;
        //}
        //else
        //{
        //    jumpDirection = false;
        //}

        isLanding = true;
        playerLandCollider2D.enabled = true;
        playerBoxCollider2D.enabled = true;
    }

    public void PlayerLandingFalse()
    {
        if (spriteFlip)
        {
            jumpDirection = true;
        }
        else
        {
            jumpDirection = false;
        }

        isLanding = false;
        playerLandCollider2D.enabled = false;
        playerBoxCollider2D.enabled = false;
    }

    public void TakePostureDamage(float postureDamage)
    {
        // Player takes Posture damage.
        postureCurrent -= postureDamage;
        Debug.Log("Posture Reduced!");

        if (postureCurrent <= 0)
        {
            nextPlayerHitStunDuration = 4;
            nextPlayerHitReaction = 8;
            nextPlayerDamageReceived = 0;
            nextPlayerForceReceived = new Vector2(1.5f * -forceDirection, 0f);
            SwitchState(HitReactionState);
        }
    }

    public void IsPostureBroken()
    {
        // This function is called in the animator.
        if (postureCurrent <= 0)
        {
            nextPlayerHitReaction = 9;
            nextPlayerHitStunDuration = 4;
            SwitchState(HitReactionState);
            postureCurrent = postureDefault;
            attackBoxCollider.enabled = false;
        }
    }

    public void FellOnGroundSound()
    {
        audioScript.PlayFellToGroundSound();
    }

    public void TurnOnAttackBoxOffset()
    {
        if(spriteFlip)
        {
            attackBoxCollider.offset = new Vector2(0.95f, 0.6f);
        }
        else
        {
            attackBoxCollider.offset = new Vector2(0f, 0.6f);
        }
    }
}

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

    // Audio Script
    public AudioScript audioScript;
    public int nextBossSwingSoundIndex;
    public int nextBossHitSoundIndex; // When the boss GETS hit.

    // Variables
    public Vector2 movementInput;
    public Rigidbody2D rb;
    public Rigidbody2D playerRb;
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
    public float postureDefault;
    public float postureCurrent = 3.0f;
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
    public BossFireballScript fireballScript;

    // Attack Collision Boxes.
    public BoxCollider2D attackHighBoxCollider2D;
    public BoxCollider2D attackLowBoxCollider2D;

    public BoxCollider2D bossBoxCollider2D;
    public BoxCollider2D bossAirBoxCollider2D;

    public PlayerStateManager playerStateManager;
    public FireballScript playerFireballScript;

    // Boss AI Variables.
    public float aiDecisionTimer = 2f;
    public float defaultAiDecisionTimer = 2f;

    // When this number goes to 0, boss will start parrying everything until it is reset.
    public int blocksUntilParry = 3;
    public int blocksUntilParryDefault = 3;

    // Regular Attack State Variables.
    public bool hasReachedPlayer = false;
    public BoxCollider2D rangeCheckBox;

    public bool isVulnerable = true;
    public bool shouldResetAiTimer = true;

    public bool isLaunched = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossBoxCollider2D = GetComponent<BoxCollider2D>();

        postureCurrent = postureDefault;

        currentState = IdleState;

        //nextBossHitStunDuration = 9;
        //nextBossHitReaction = 9;
        //currentState = HitReactionState;

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

        if(rb.position.x < playerRb.position.x)
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
            rangeCheckBox.offset = new Vector2(-0.3f, 0f);
            bossBoxCollider2D.offset = new Vector2(-0.05f, -0.78f);
        }
        else
        {
            forceDirection = 1;
            rangeCheckBox.offset = new Vector2(0.61f, 0f);
            bossBoxCollider2D.offset = new Vector2(0.05f, -0.78f);
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
        fireballScript.rb.position = new Vector2(rb.position.x + (0.8f * forceDirection), rb.position.y + 0.6f);
        audioScript.PlayFireballShotSound();
        fireballScript.FireballSpawned();
    }

    public void TurnOnAttackBoxCollider()
    {
        if (spriteRenderer.flipX)
        {
            attackHighBoxCollider2D.offset = new Vector2(0.60f, 0f);
        }
        else
        {
            attackHighBoxCollider2D.offset = new Vector2(-0.34f, 0f);
        }

        attackHighBoxCollider2D.enabled = true;
    }

    public void TurnOffAttackBoxCollider()
    {
        attackHighBoxCollider2D.offset = new Vector2(999f, 999f);

        attackHighBoxCollider2D.enabled = false;
    }

    public void TurnOnLowAttackBoxCollider()
    {
        if (spriteRenderer.flipX)
        {
            attackLowBoxCollider2D.offset = new Vector2(0.77f, -0.37f);
        }
        else
        {
            attackLowBoxCollider2D.offset = new Vector2(-0.33f, -0.37f);
        }

        attackLowBoxCollider2D.enabled = true;
    }

    public void TurnOffLowAttackBoxCollider()
    {
        attackLowBoxCollider2D.offset = new Vector2(999f, 999f);

        attackLowBoxCollider2D.enabled = false;
    }

    public void AttackHitProperty(float damage, Vector2 force, int hitreactionId, float stunduration, int hitsoundId)
    {
        playerStateManager.nextPlayerDamageReceived = damage;
        playerStateManager.nextPlayerForceReceived = force * forceDirection;
        playerStateManager.nextPlayerHitReaction = hitreactionId;
        playerStateManager.nextPlayerHitStunDuration = stunduration;
        playerStateManager.nextPlayerHitSoundIndex = hitsoundId;
        
        //Player Swing sound here?
    }

    public void AttackHitPropertySelf(float damage, Vector2 force, int hitreactionId, float stunduration, int hitsoundId)
    {
        nextBossDamageReceived = damage;
        nextBossForceReceived = force * (forceDirection);
        nextBossHitReaction = hitreactionId;
        nextBossHitStunDuration = stunduration;
        nextBossHitSoundIndex = hitsoundId;
    }

    public void PlayAttackSwingSound()
    {
        audioScript.SoundIndexPlay(nextBossSwingSoundIndex);
    }

    public void TakePostureDamage(float postureDamage)
    {
        // Boss takes Posture damage.
        postureCurrent -= postureDamage;
        //Debug.Log("Posture Reduced!");

        if (postureCurrent <= 0)
        {
            isVulnerable = true;
            nextBossHitStunDuration = 4;
            nextBossHitReaction = 8;
            nextBossDamageReceived = 0;
            nextBossForceReceived = new Vector2(1.5f * -forceDirection, 0f);
            SwitchState(HitReactionState);
        }
    }

    public void IsPostureBroken()
    {
        // This function is called in the animator.
        if(postureCurrent <= 0 && isVulnerable)
        {
            nextBossHitReaction = 9;
            nextBossHitStunDuration = 4;
            SwitchState(HitReactionState);
            postureCurrent = postureDefault;
            attackHighBoxCollider2D.enabled = false;
            attackLowBoxCollider2D.enabled = false;
            isVulnerable = false;
        }
    }

    public void FellOnGroundSound()
    {
        // Audio Played in Animator.
        audioScript.PlayFellToGroundSound();
    }

    public void FlashBlue()
    {
        StartCoroutine(FlashBlueCoroutine());
    }

    IEnumerator FlashBlueCoroutine()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void FallGravityOn()
    {
        //0.2f
        rb.gravityScale = 1f;
    }

    public void FallGravityOff()
    {
        rb.gravityScale = 2f;
    }

    public void SetIsLaunchedWithDelay(float delay)
    {
        Invoke("TurnOnIsLaunched", delay);
    }

    public void TurnOnIsLaunched()
    {
        isLaunched = true;
    }
}

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
    public PlayerIntroductionState IntroductionState = new PlayerIntroductionState();

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
    public bool isUnblockableCounter = false;

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
    public PlayerInputActions input = null;

    public bool isLanding = false;
    public bool jumpDirection = false;
    public bool isCloseToWallLeft = false;
    public bool isCloseToWallRight = false;
    public bool isLaunched = false;

    public GameObject canvasReference;
    public PlayerInput playerInput;

    public GameObject DarkenEffect;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Parry.performed += OnParryPerformed;
        input.Player.NormalAttack.performed += OnRegularAttackPerformed;
        input.Player.SpecialAttack.performed += OnSpecialAttackPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Parry.performed -= OnParryPerformed;
        input.Player.NormalAttack.performed -= OnRegularAttackPerformed;
        input.Player.SpecialAttack.performed -= OnSpecialAttackPerformed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // currentState = IdleState;
        currentState = IntroductionState;

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
            if(!isInvincible)
            {
                // The player is in range of boss's attack.
                bossStateManager.hasReachedPlayer = true;
                bossStateManager.animator.SetBool("isWalkTowards", false);
                bossStateManager.animator.SetBool("isWalkBackwards", false);
                bossStateManager.SwitchState(bossStateManager.RegularAttackState);
            }
        }
        if(collision.tag == "BossAntiAirCheck")
        {
            // Boss does Dragon Punch.
            bossStateManager.nextAttackPatternChoice = 120;
            bossStateManager.animator.SetBool("isWalkTowards", false);
            bossStateManager.animator.SetBool("isWalkBackwards", false);

            // Prevent the player from constantly being comboed.
            bossStateManager.bossAntiAirBoxCollider2D.enabled = false;
            bossStateManager.SwitchState(bossStateManager.RegularAttackState);

            // Re-enable the Anti-Air Box after the player landed.
            bossStateManager.canAntiAirAgain = false;
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
        isUnblockableCounter = true;
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
        isUnblockableCounter = false;
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

    private void OnRegularAttackPerformed(InputAction.CallbackContext value)
    {
        currentState.OnRegularAttackPerformed(this);
    }
    
    private void OnSpecialAttackPerformed(InputAction.CallbackContext value)
    {
        currentState.OnSpecialAttackPerformed(this);
    }

    public void PlayerLandingTrue()
    {
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
            postureCurrent = 0;
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

    public void FallGravityOn()
    {
        //0.2f //1f
        rb.gravityScale = 1.5f;
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

    public void FireballVoice()
    {
        audioScript.PlayRyuFireballVoice();
    }

    public void DragonPunchVoice()
    {
        audioScript.PlayRyuDragonPunchVoice();
    }

    public void TurnOffPlayerCollisionBoxes()
    {
        // This function is used when the player falls down.
        playerBoxCollider2D.enabled = false;
    }

    public void RyuIntroVoice()
    {
        // Randomize between 2 Voice Lines.
        audioScript.PlayRyuIntroductionVoice();
    }

    public void RyuLoseVoice()
    {
        audioScript.PlayRyuLoseVoice();
    }

    public void RyuTransitionVoice()
    {
        audioScript.PlayRyuTransitionVoice();
    }

    public void GoIdleWithDelay(float delay)
    {
        animator.SetTrigger("triggerIdle");
        Invoke("GoIdle", delay);
    }

    public void GoIdle()
    {
        SwitchState(IdleState);
        canvasReference.SetActive(true);
    }

    public void PlayBossWinAnimationWithDelay(float delay)
    {
        Invoke("PlayBossWinAnimation", delay);
    }

    private void PlayBossWinAnimation()
    {
        bossStateManager.animator.SetTrigger("triggerWin");
    }

    public void GainPosture()
    {
        postureCurrent++;
        if(postureCurrent > postureDefault)
        {
            // Prevent Overflow of Posture.
            postureCurrent = postureDefault;
        }
    }

    public void StopMovingAnimation()
    {
        animator.SetBool("isWalkTowards", false);
        animator.SetBool("isWalkBackwards", false);
    }

    public void StartPlayerPhaseTwoAnimationWithDelay(float delay)
    {
        Invoke("StartPlayerPhaseTwoAnimation", delay);
    }

    private void StartPlayerPhaseTwoAnimation()
    {
        animator.SetTrigger("triggerPhaseTwo");
    }

    public void RyuShinVoice()
    {
        audioScript.PlayRyuShinVoice();
    }

    public void RyuSuperShoryukenVoice()
    {
        audioScript.PlayRyuSuperShoryukenVoice();
    }

    public void ApplyHitPropertyShin1()
    {
        audioScript.PlaySuperAttackSound();
        AttackHitProperty(3, new Vector2(0, 0), 14, 100f, 13);
    }

    public void ApplyHitPropertyShinFinal()
    {
        audioScript.PlaySuperAttackSound();
        AttackHitProperty(5, new Vector2(20, 90), 13, 999f, 14);
    }

    public void SuperTimeStart()
    {
        DarkenEffect.SetActive(true);
        bossStateManager.animator.speed = 0f;
        bossStateManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void SuperTimeStop()
    {
        DarkenEffect.SetActive(false);
        bossStateManager.animator.speed = 1;
        bossStateManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        bossStateManager.rb.constraints = RigidbodyConstraints2D.None;
    }
}

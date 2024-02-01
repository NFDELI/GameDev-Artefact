using UnityEngine;

public class BossIdleState : BossBaseState
{
    int attackDistanceChoice = -1;
    bool isAttackDistanceChosenClose = false;
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Idle State");
        boss.isLanded = true;
        boss.isLaunched = false;
        boss.rb.velocity = new Vector2(0, 0);
        boss.attackCounter = 1;
        boss.bossBoxCollider2D.enabled = true;

        // The boss is Idle, he is not in the air.
        boss.bossAirBoxCollider2D.enabled = false;

        boss.spriteRenderer.color = Color.white;

        // Reset decision making timer.
        boss.animator.SetTrigger("triggerIdle");

        if(boss.shouldResetAiTimer)
        {
            if(boss.isPhaseTwo)
            {
                boss.defaultAiDecisionTimer = 0.8f;
            }
            boss.aiDecisionTimer = boss.defaultAiDecisionTimer;
            boss.shouldResetAiTimer = true;
        }

        // Boss is not attempting to attack.
        boss.rangeCheckBox.enabled = false;

        // Close Range Attacks -> 1, 2, 3, 4, 5, 6, 7, 8.
        // Far Range Attacks -> 9, 10;
        attackDistanceChoice = Random.Range(1, 11);

        // Debugging Distance Choice.
        // attackDistanceChoice = 8;

        if (attackDistanceChoice < 9)
        {
            // Close Range Chosen.
            isAttackDistanceChosenClose = true;
        }
        else
        {
            // Far Range Chosen.
            isAttackDistanceChosenClose = false;
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (boss.health <= boss.phaseTwoHealthThreshold)
        { 
            boss.initiatePhaseTwo = true;
        }

        if (boss.initiatePhaseTwo && !boss.isPhaseTwo)
        {
            boss.isPhaseTwo = true;
            boss.aiDecisionTimer = boss.defaultAiDecisionTimer;
            boss.SwitchState(boss.IntroductionState);
        }

        if (boss.aiDecisionTimer <= 0)
        {
            boss.bossAntiAirBoxCollider2D.enabled = false;

            // Choose which action to do.
        
            // Do not engage if player is lying down on the floor.
            if(!boss.playerStateManager.isInvincible)
            {
                if(isAttackDistanceChosenClose)
                {
                    // Boss chooses to approach the player for close range attack.
                    boss.SwitchState(boss.WalkingState);
                }
                else
                {
                    // Boss chooses to attack the player from far range.
                    boss.nextAttackPatternChoice = 100;
                    boss.SwitchState(boss.RegularAttackState);
                }
            }
        
            // Boss chooses to do long range attack.
            //boss.SwitchState(boss.RegularAttackState);
        }
        else
        {
            // Wait, decrease timer.
            if(boss.isAiEnabled)
            {
                boss.aiDecisionTimer -= Time.deltaTime;
            }
        }

        // Sprite-Flip Check.
        if(boss.spriteFlip)
        {
            boss.spriteRenderer.flipX = true;
            boss.attackHighBoxCollider2D.offset = new Vector2(0.2685299f, 0);
        }
        else
        {
            boss.spriteRenderer.flipX = false;
            boss.attackHighBoxCollider2D.offset = new Vector2(-0.2685299f, 0);
        }

        // Boss is waiting for the player to Jump. (Anti-Air Check)
        if (boss.canAntiAirAgain)
        {
            boss.bossAntiAirBoxCollider2D.enabled = true;
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
        
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
        BossIdleDefense(boss, collision);
        //BossIdleOpen(boss, collision);
    }

    private void BossIdleDefense(BossStateManager boss, Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHigh")
        {
            if (boss.blocksUntilParry <= 0)
            {
                // Boss parries the incoming attack.
                boss.AttackHitPropertySelf(0, Vector2.zero, 6, 0, 10);
            }
            else
            {
                // Boss blocks the incoming attack.
                boss.AttackHitPropertySelf(boss.nextBossDamageReceived * 0.25f, Vector2.zero, 4, -1, 7);
            }

            // React to the player's Regular attack.
            boss.SwitchState(boss.HitReactionState);
        }
        if (collision.tag == "PlayerFireball")
        {
            // React to the player's Fireball attack.
            if (boss.blocksUntilParry <= 0)
            {
                // Boss parries the fireball attack.
                boss.AttackHitPropertySelf(0, Vector2.zero, 11, 0, 8);
            }
            else
            {
                // Boss blocks the fireball attack.
                boss.AttackHitPropertySelf(boss.playerFireballScript.damage * 0.25f, Vector2.zero, 4, boss.playerFireballScript.stunDuration, 11);
            }
            boss.SwitchState(boss.HitReactionState);
        }
    }

    private void BossIdleOpen(BossStateManager boss, Collider2D collision)
    {
        if(collision.tag == "PlayerAttackHigh")
        {
            boss.SwitchState(boss.HitReactionState);
        }
    }
}

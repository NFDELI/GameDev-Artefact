using UnityEngine;

public class BossIdleState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Idle State");
        boss.isLanded = true;
        boss.rb.velocity = new Vector2(0, 0);
        boss.attackCounter = 1;

        // Reset decision making timer.
        boss.animator.SetTrigger("triggerIdle");

        if(boss.shouldResetAiTimer)
        {
            boss.aiDecisionTimer = boss.defaultAiDecisionTimer;
            boss.shouldResetAiTimer = true;
        }

        boss.rangeCheckBox.enabled = false;

        // The boss is unmovable when Idle.
        boss.rb.mass = 1000;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.aiDecisionTimer <= 0)
        {
            // Choose which action to do.

            // Boss chooses to approach the player for close range attack.
            boss.SwitchState(boss.WalkingState);

            // Boss chooses to do long range attack.
            //boss.SwitchState(boss.RegularAttackState);
        }
        else
        {
            // Wait, decrease timer.
            boss.aiDecisionTimer -= Time.deltaTime;
        }
        
        // Check for Attack Input.
        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    boss.SwitchState(boss.RegularAttackState);
        //}
        //
        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    boss.SwitchState(boss.BossSpecialAttackState);
        //}

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
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
        
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHigh")
        {
            if(boss.blocksUntilParry <= 0)
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
            if(boss.blocksUntilParry <= 0)
            {
                // Boss parries the incoming attack.
                boss.AttackHitPropertySelf(0, Vector2.zero, 11, 0, 8);
            }
            else
            {
                // Boss blocks the incoming attack.
                boss.AttackHitPropertySelf(boss.playerFireballScript.damage * 0.25f, Vector2.zero, 4, boss.playerFireballScript.stunDuration, 11);
            }
            boss.SwitchState(boss.HitReactionState);
        }
    }
}

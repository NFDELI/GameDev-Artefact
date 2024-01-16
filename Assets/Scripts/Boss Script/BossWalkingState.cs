using UnityEngine;

public class BossWalkingState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Walking State");
        boss.hasReachedPlayer = false;
        boss.rangeCheckBox.enabled = true;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.hasReachedPlayer)
        {
            // Boss has reached the player to do close range attacks. (Change States)
            StopMovingAnimation(boss);
            boss.SwitchState(boss.RegularAttackState);
        }
        else 
        {
            // Boss not reached player yet. Go towards player.
            if (boss.rb.position.x < boss.playerRb.position.x)
            {
                // Move Right.
                if(boss.spriteFlip)
                {
                    boss.animator.SetBool("isWalkTowards", true);
                    boss.animator.SetBool("isWalkBackwards", false);
                }
                else
                {
                    boss.animator.SetBool("isWalkTowards", false);
                    boss.animator.SetBool("isWalkBackwards", true);
                }
                boss.rb.MovePosition(boss.rb.position + new Vector2(1, 0) * boss.movementSpeed * Time.fixedDeltaTime);

            }
            else if(boss.rb.position.x > boss.playerRb.position.x)
            {
                // Move Left.
                if(boss.spriteFlip) 
                {
                    boss.animator.SetBool("isWalkBackwards", true);
                    boss.animator.SetBool("isWalkTowards", false);
                }
                else
                {
                    boss.animator.SetBool("isWalkBackwards", false);
                    boss.animator.SetBool("isWalkTowards", true);
                }
                boss.rb.MovePosition(boss.rb.position + new Vector2(-1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
            }

            if(boss.movementInput.y > 0)
            {
                // Jump.
                StopMovingAnimation(boss);
                boss.SwitchState(boss.JumpingState);
            }
            else if(boss.movementInput.y < 0)
            {
                // Crouch.
                StopMovingAnimation(boss);
                boss.SwitchState(boss.CrouchState);
            }
        }

        // Check for Attack Input.
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    StopMovingAnimation(boss);
        //    boss.SwitchState(boss.RegularAttackState);
        //}
        //
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    StopMovingAnimation(boss);
        //    boss.SwitchState(boss.BossSpecialAttackState);
        //}
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
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

            // Player is Interrupting the boss, power through their attacks!
            boss.shouldResetAiTimer = false;

            // React to the player's Regular attack.
            boss.SwitchState(boss.HitReactionState);
        }
        if (collision.tag == "PlayerFireball")
        {
            // React to the player's Fireball attack.
            if (boss.blocksUntilParry <= 0)
            {
                // Boss parries the incoming attack.
                boss.AttackHitPropertySelf(0, Vector2.zero, 11, 0, 8);
            }
            else
            {
                // Boss blocks the incoming attack.
                boss.AttackHitPropertySelf(boss.playerFireballScript.damage * 0.25f, Vector2.zero, 4, boss.playerFireballScript.stunDuration, 11);
            }

            // Player is Interrupting the boss, power through their attacks!
            boss.shouldResetAiTimer = false;

            boss.SwitchState(boss.HitReactionState);
        }
    }

    public void MoveRight(BossStateManager boss)
    {
        boss.animator.SetBool("isWalkTowards", false);
        boss.animator.SetBool("isWalkBackwards", true);
        boss.rb.MovePosition(boss.rb.position + new Vector2(1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveLeft(BossStateManager boss) 
    {
        boss.animator.SetBool("isWalkBackwards", false);
        boss.animator.SetBool("isWalkTowards", true);
        boss.rb.MovePosition(boss.rb.position + new Vector2(-1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
    }

    private void StopMovingAnimation(BossStateManager boss)
    {
        boss.animator.SetBool("isWalkTowards", false);
        boss.animator.SetBool("isWalkBackwards", false);
    }
}

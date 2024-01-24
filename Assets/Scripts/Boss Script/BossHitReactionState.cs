using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossHitReactionState : BossBaseState
{
    private int hitReactionIndex;
    private float hitStunTime;
    private float hitDamage;
    private Vector2 hitForce;
    private bool timerStarted = false;
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Hit Reaction State");
        hitReactionIndex = boss.nextBossHitReaction;
        hitStunTime = boss.nextBossHitStunDuration;
        hitForce = boss.nextBossForceReceived;
        hitDamage = boss.nextBossDamageReceived;
        boss.spriteRenderer.color = Color.white;

        // Boss can takeknockback when getting hit.
        boss.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        boss.bossAntiAirBoxCollider2D.enabled = false;

        switch (boss.nextBossHitReaction)
        {
            case 0:
                // Get Hit High.
                boss.animator.SetTrigger("triggerHitReactionHigh");
                timerStarted = true;
                break;
            case 1:
                // Get Hit Low.
                boss.animator.SetTrigger("triggerHitReactionLow");
                timerStarted = true;
                break;
            case 2:
                // Get Hit then Fall.
                boss.animator.SetTrigger("triggerFall");
                break;
            case 3:
                // Grabbed.
                break;
            case 4:
                // High Block.
                boss.animator.SetTrigger("triggerBlockHigh");
                timerStarted= true;
                boss.blocksUntilParry--;
                break;
            case 5:
                // Low Block.
                boss.animator.SetTrigger("triggerBlockLow");
                timerStarted = true;
                break;
            case 6:
                // High Parry.
                boss.animator.SetTrigger("triggerParryHigh");
                boss.playerStateManager.TakePostureDamage(1f);
                boss.audioScript.PlayParryAttackSound();
                break;
            case 7:
                // Low Parry.
                boss.animator.SetTrigger("triggerParryLow");
                break;
            case 8:
                // Guard/Posture/Grab Break.
                boss.animator.SetTrigger("triggerPostureBreak");
                boss.audioScript.PlayBossPostureBreakVoice();
                boss.nextBossHitSoundIndex = -1;
                timerStarted = true;
                break;
            case 9:
                // Dazed/Stunned.
                boss.animator.SetTrigger("triggerStunned");
                timerStarted = true;
                break;
            case 10:
                // Get Up Animation.
                boss.animator.SetTrigger("triggerGetUp");
                break;
            case 11:
                // High Parry. (Fireball Parried)
                boss.animator.SetTrigger("triggerParryHigh");
                break;
            case 12:
                // Get Hit by Spinning Kick
                //boss.rb.velocity = Vector2.zero;
                boss.rb.totalForce = hitForce;
                boss.SetIsLaunchedWithDelay(0.4f);
                boss.animator.SetTrigger("triggerLaunched");

                // Boss Gets hit by Tatsu, allow player to recover quickly.
                boss.playerStateManager.animator.SetTrigger("triggerTatsuRecovery");
                break;
            case 13:
                // Boss Gets hit by Dragon Punch.
                //boss.rb.velocity = Vector2.zero;
                boss.rb.totalForce = hitForce;
                boss.SetIsLaunchedWithDelay(0.4f);
                boss.animator.SetTrigger("triggerLaunched");
                break;
            default:
                break;
        }

        // Apply Hit Attributes Here -Boss.
        boss.health -= hitDamage;
        boss.rb.AddForce(hitForce, ForceMode2D.Impulse);
        boss.audioScript.SoundIndexPlay(boss.nextBossHitSoundIndex);

        // Turn off Player's attack hitbox so that it can be re-registered for the next hit.
        boss.playerStateManager.attackBoxCollider.enabled = false;

        // Turn off the Boss's attack hitbox so that the player can properly punish the boss.
        boss.attackHighBoxCollider2D.enabled = false;
        boss.attackLowBoxCollider2D.enabled = false;
        boss.attackUnblockableBoxCollider2D.enabled = false;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.canGetUp && !(boss.health <= 0)) 
        {
            // To make the boss get up from falling.
            boss.canGetUp = false;
            boss.AttackHitPropertySelf(0, new Vector2(0 ,0), 10, 999f, -1);
            boss.bossBoxCollider2D.enabled = true;
            boss.bossAirBoxCollider2D.enabled = false;
            boss.SwitchState(boss.HitReactionState);
        }

        if(timerStarted)
        {
            hitStunTime -= Time.deltaTime;
            if(hitStunTime <= 0) 
            {
                StunFinished(boss);
            }
        }

        if (boss.isLaunched)
        {
            // The default boss's box collider is turned off so that the player's body does not push/collide with the airborne boss.
            boss.bossBoxCollider2D.enabled = false;
            boss.bossAirBoxCollider2D.enabled = true;

            // Boss's body has reached its peak.
            if (boss.rb.velocity.y < 0)
            {
                boss.animator.SetTrigger("triggerPeaked");
            }
        }
        else
        {
            boss.rb.gravityScale = 2f;
        }

        if(boss.health <= 0)
        {
            // Boss Dies.
            boss.SwitchState(boss.IntroductionState);
        }

    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
        //Debug.Log("Collision Detected");
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHigh")
        {
            boss.SwitchState(boss.HitReactionState);
        }
    }

    private void StunFinished(BossStateManager boss)
    {
        boss.animator.SetTrigger("triggerIdle");
        boss.SwitchState(boss.IdleState);
    }
}

using System;
using System.Collections;
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
        boss.spriteRenderer.color = Color.red;

        // Boss can takeknockback when getting hit.
        boss.rb.mass = 2f;

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
                boss.rb.velocity = Vector2.zero;
                boss.SetIsLaunchedWithDelay(0.2f);
                boss.animator.SetTrigger("triggerLaunched");

                // Boss Gets hit by Tatsu, allow player to recover quickly.
                boss.playerStateManager.animator.SetTrigger("triggerTatsuRecovery");
                break;
            case 13:
                // Boss Gets hit by Dragon Punch.
                boss.rb.velocity = Vector2.zero;
                boss.SetIsLaunchedWithDelay(0.2f);
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
    }

    public override void UpdateState(BossStateManager boss)
    {
        // Movement Input Detected.
        if((hitReactionIndex == 2 && boss.canGetUp && hitStunTime <= 0) && !(boss.health <= 0))
        {
            // To make the boss get up from falling.
            boss.nextBossHitReaction = 10;
            boss.canGetUp = false;
            boss.nextBossHitStunDuration = 1f;
            boss.nextBossHitSoundIndex = -1;
            boss.nextBossDamageReceived = 0;
            boss.nextBossForceReceived = Vector2.zero;
            boss.SwitchState(boss.HitReactionState);
        }

        if(timerStarted)
        {
            hitStunTime -= Time.deltaTime;
            if(hitStunTime <= 0) 
            {
                if (hitReactionIndex == 2)
                {
                    boss.canGetUp = true;
                }
                else
                {
                    StunFinished(boss);
                }
            }
        }

        if(boss.isLaunched)
        {
            boss.bossBoxCollider2D.enabled = false;
            boss.bossAirBoxCollider2D.enabled = true;

            // Boss's body has reached its peak.
            if(boss.rb.velocity.y < 0)
            {
                boss.animator.SetTrigger("triggerPeaked");
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(hitStunTime);
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

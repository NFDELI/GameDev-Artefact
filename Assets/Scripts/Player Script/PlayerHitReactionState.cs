using System;
using System.Collections;
using UnityEngine;

public class PlayerHitReactionState : PlayerBaseState
{
    private int hitReactionIndex;
    private float hitStunTime;
    private float hitDamage;
    private Vector2 hitForce;
    private bool timerStarted = false;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Hit Reaction State");

        hitReactionIndex = player.nextPlayerHitReaction;
        hitStunTime = player.nextPlayerHitStunDuration;
        hitForce = player.nextPlayerForceReceived;
        hitDamage = player.nextPlayerDamageReceived;
        //player.spriteRenderer.color = Color.red;

        switch (player.nextPlayerHitReaction)
        {
            case 0:
                // Get Hit High.
                player.animator.SetTrigger("triggerHitReactionHigh");
                timerStarted = true;
                break;
            case 1:
                // Get Hit Low.
                player.animator.SetTrigger("triggerHitReactionLow");
                timerStarted = true;
                break;
            case 2:
                // Get Hit then Fall.
                player.animator.SetTrigger("triggerFall");
                break;
            case 3:
                // Grabbed.
                break;
            case 4:
                // High Block.
                player.animator.SetTrigger("triggerBlockHigh");
                timerStarted= true;
                //player.spriteRenderer.color = Color.yellow;

                // Ensure that the player takes less damage while blocking and take less knockback.
                hitDamage = hitDamage / 4;
                hitForce = hitForce / 2;

                player.wasBlocking = true;
                break;
            case 5:
                // Low Block.
                player.animator.SetTrigger("triggerBlockLow");
                timerStarted = true;
                player.spriteRenderer.color = Color.yellow;

                // Ensure that the player takes less damage while blocking and take less knockback.
                hitDamage = hitDamage / 4;
                hitForce = hitForce / 2;
                break;
            case 6:
                // High Parry.
                player.animator.SetTrigger("triggerParryHigh");
                player.spriteRenderer.color = Color.blue;
                player.bossStateManager.TakePostureDamage(1f);
                break;
            case 7:
                // Low Parry.
                player.animator.SetTrigger("triggerParryLow");
                player.spriteRenderer.color = Color.blue;
                player.bossStateManager.TakePostureDamage(1f);
                break;
            case 8:
                // Guard/Posture/Grab Break.
                player.animator.SetTrigger("triggerPostureBreak");
                timerStarted = true;
                break;
            case 9:
                // Dazed/Stunned.
                player.animator.SetTrigger("triggerStunned");
                timerStarted = true;
                break;
            case 10:
                // Get Up Animation.
                player.animator.SetTrigger("triggerGetUp");
                break;
            case 11:
                // Get Hit by Fireball. (Fireball has special hit sounds and effects)
                player.animator.SetTrigger("triggerHitReactionHigh");
                timerStarted = true;
                player.nextPlayerHitSoundIndex = 12;
                hitForce = player.bossStateManager.fireballScript.knockbackForce;
                hitDamage = player.bossStateManager.fireballScript.damage;
                break;
            case 12:
                // Blocks Fireball Attack.
                player.animator.SetTrigger("triggerBlockHigh");
                timerStarted = true;
                player.nextPlayerHitSoundIndex = 11;
                hitForce = player.bossStateManager.fireballScript.knockbackForce / 2;
                hitDamage = player.bossStateManager.fireballScript.damage / 4;
                break;
            case 13:
                // Parry a Fireball. (Does not do posture damage to the boss)
                player.animator.SetTrigger("triggerParryHigh");
                player.spriteRenderer.color = Color.blue;
                break;
            default:
                break;
        }

        // Apply Hit Attributes Here.
        player.health -= hitDamage;
        player.rb.AddForce(hitForce, ForceMode2D.Impulse);
        player.audioScript.SoundIndexPlay(player.nextPlayerHitSoundIndex);

        // Turn off Boss's attack hitbox so that it can be re-registered for the next hit.
        player.bossStateManager.attackHighBoxCollider2D.enabled = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput != Vector2.zero)
        {
            // Movement Input Detected.
            if((hitReactionIndex == 2 && player.canGetUp) && !(player.health <= 0))
            {
                // To make the player get up from falling.
                player.nextPlayerHitReaction = 10;
                player.nextPlayerHitStunDuration = 1f;
                player.nextPlayerHitSoundIndex = -1;
                player.nextPlayerDamageReceived = 0;
                player.nextPlayerForceReceived = Vector2.zero;
                player.canGetUp = false;
                player.SwitchState(player.HitReactionState);
            }
        }

        if(hitReactionIndex != 2)
        {
            if (timerStarted)
            {
                hitStunTime -= Time.deltaTime;
                if (hitStunTime <= 0) { StunFinished(player); }
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(hitStunTime);
        }

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if(!player.isInvincible)
        {
            // Player gets hit by an attack again.
            if (collision.tag == "BossAttackHigh")
            {
                timerStarted = false;

                if(player.wasBlocking)
                {
                    // Ensures that the player goes into blocking state.
                    player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 4, player.nextPlayerHitStunDuration, 7);
                }

                player.SwitchState(player.HitReactionState);
            }

            if(collision.tag == "BossAttackLow")
            {
                timerStarted = false;
                player.SwitchState(player.HitReactionState);
            }
        }
    }

    private void StunFinished(PlayerStateManager player)
    {
        player.animator.SetTrigger("triggerIdle");
        player.SwitchState(player.IdleState);
    }
}

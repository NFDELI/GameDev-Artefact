using System;
using System.Collections;
using UnityEngine;

public class PlayerHitReactionState : PlayerBaseState
{
    private int hitReactionIndex;
    private float hitStunTime;
    private bool timerStarted = false;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Hit Reaction State");

        hitReactionIndex = player.nextPlayerHitReaction;
        hitStunTime = player.nextPlayerHitStunDuration;

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
                break;
            case 5:
                // Low Block.
                player.animator.SetTrigger("triggerBlockLow");
                timerStarted = true;
                break;
            case 6:
                // High Parry.
                player.animator.SetTrigger("triggerParryHigh");
                break;
            case 7:
                // Low Parry.
                player.animator.SetTrigger("triggerParryLow");
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
            default:
                break;
        }
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
        //Debug.Log("Collision Detected");
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if(!player.isInvincible)
        {
            if (collision.tag == "BossAttackHigh")
            {
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

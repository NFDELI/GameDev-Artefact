using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPostureBrokenState : PlayerBaseState
{
    float postureBrokenStunTime = 10f;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Posture Broken State");
        player.audioScript.PlayPlayerPostureBreakVoice();
        player.animator.SetTrigger("triggerPostureBreak");

        player.attackBoxCollider.enabled = false;

        player.bossStateManager.attackHighBoxCollider2D.enabled = false;
        player.bossStateManager.attackLowBoxCollider2D.enabled = false;
        player.bossStateManager.attackUnblockableBoxCollider2D.enabled = false;

        // Boss Punishes the player for being Posture Broken.
        player.bossStateManager.shouldResetAiTimer = false;
        player.bossStateManager.aiDecisionTimer = 0;
        player.bossStateManager.InstantAttackAIDelayed(0.3f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(postureBrokenStunTime > 0)
        {
            postureBrokenStunTime -= Time.deltaTime;
        }
        else
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        // Blocks Low Attacks While Crouching.
        if (collision.tag == "BossAttackLow")
        {
            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.HitReactionState);
        }
        
        if (collision.tag == "BossAttackHigh")
        {
            // Gets Hit by High Attacks.
            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.HitReactionState);
        }

        if (collision.tag == "BossAttackUnblockable")
        {
            player.SwitchState(player.HitReactionState);
        }

        if (collision.tag == "BossFireball")
        {
            // Gets hit by Fireball. (Cannot block fireball while crouching)
            player.nextPlayerHitReaction = 11;
            player.SwitchState(player.HitReactionState);
        }

    }
}

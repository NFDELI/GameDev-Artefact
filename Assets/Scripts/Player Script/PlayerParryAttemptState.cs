using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParryAttemptState : PlayerBaseState
{
    private bool isHighParryAttempt = false; 
    private bool isLowParryAttempt = false;

    public override void EnterState(PlayerStateManager player)
    {
        player.StopMovingAnimation();
        if (player.movementInput.y < 0)
        {
            AttemptLowParry(player);
            return;
        }
        AttemptHighParry(player);
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if (collision.tag == "BossAttackHigh")
        {
            ParryCheck(player, HitLevel.HIGH);
        }
        if (collision.tag == "BossAttackLow")
        {
            ParryCheck(player, HitLevel.LOW);
        }
        if (collision.tag == "BossFireball")
        {
            ParryCheck(player, HitLevel.FIREBALL);
        }
        // Unblockable attacks cannot be blocked or parried.
        player.SwitchState(player.HitReactionState);
    }

    private void AttemptLowParry(PlayerStateManager player)
    {
        isLowParryAttempt = true;
        isHighParryAttempt = false;
        player.animator.SetTrigger("triggerParryLowAttempt");
    }

    private void AttemptHighParry(PlayerStateManager player)
    {
        isLowParryAttempt = false;
        isHighParryAttempt = true;
        player.animator.SetTrigger("triggerParryHighAttempt");
    }

    private void ParryCheck(PlayerStateManager player, HitLevel hitLevel)
    {
        if (hitLevel == HitLevel.HIGH)
        {
            if (isHighParryAttempt) { player.RegularParryProperty(); return; }
            if (isLowParryAttempt) { player.RegularParryProperty(true); return; }
        }
        if(hitLevel == HitLevel.LOW)
        {
            if (isHighParryAttempt) { player.RegularParryProperty(true); return; }
            if (isLowParryAttempt) { player.PerfectLowParryProperty(); return; }
        }
        if(hitLevel == HitLevel.FIREBALL)
        {
            if (isHighParryAttempt) { player.FireBallParryProperty(); return; }
        }
    }

    private enum HitLevel
    {
        HIGH,
        LOW,
        FIREBALL,
    };
}
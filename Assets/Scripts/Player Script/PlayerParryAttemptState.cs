using UnityEngine;

public class PlayerParryAttemptState : PlayerBaseState
{
    private bool isHighParryAttempt = false; 
    private bool isLowParryAttempt = false; 
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Parry State");

        if (player.animator.GetBool("isCrouch"))
        {
            isLowParryAttempt = true;
            isHighParryAttempt = false;

            player.animator.SetTrigger("triggerParryLowAttempt");
        }
        else
        {
            isHighParryAttempt = true;
            isLowParryAttempt = false;
            player.animator.SetTrigger("triggerParryHighAttempt");
        }

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
            if (isHighParryAttempt)
            {
              // Ensures that the player goes into successful parrying state.
              player.AttackHitPropertySelf(0, player.nextPlayerForceReceived / 2, 6, player.nextPlayerHitStunDuration, 8);
            }

            // Parry success is also part of HitReactionState.
            player.SwitchState(player.HitReactionState);
        }
        else if (collision.tag == "BossAttackLow")
        {
            if (isLowParryAttempt)
            {
                // Ensures that the player goes into successful low state.
                player.AttackHitPropertySelf(0, player.nextPlayerForceReceived / 2, 7, player.nextPlayerHitStunDuration, 8);
            }
            player.SwitchState(player.HitReactionState);
        }

        if(collision.tag == "BossFireball")
        {
            if(isHighParryAttempt)
            {
                // Ensures that the player goes into successful parrying state.
                player.AttackHitPropertySelf(0, player.nextPlayerForceReceived / 2, 13, player.nextPlayerHitStunDuration, 8);
            }
            player.SwitchState(player.HitReactionState);
        }

        // Unblockable attacks cannot be blocked or parried.
        if (collision.tag == "BossAttackUnblockable")
        {
            player.SwitchState(player.HitReactionState);
        }
    }
}
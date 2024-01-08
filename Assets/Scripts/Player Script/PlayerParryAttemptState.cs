using UnityEngine;

public class PlayerParryAttemptState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Parry State");
        player.animator.SetTrigger("triggerParryHighAttempt");
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
            if (player.animator.GetBool("triggerParryHighAttempt"))
            {
                // Ensures that the player goes into blocking state.
                player.AttackHitPropertySelf(0, player.nextPlayerForceReceived / 2, 6, player.nextPlayerHitStunDuration, 7);
            }
            player.SwitchState(player.HitReactionState);
        }
    }
}
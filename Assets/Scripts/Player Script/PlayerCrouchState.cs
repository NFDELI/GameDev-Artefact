using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Crouch State");
        player.animator.SetBool("isCrouch", true);
        player.animator.SetTrigger("forceCrouch");
        player.attackCounter = 0;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput.y >= 0)
        {
            player.animator.SetBool("isCrouch", false);
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
            if (player.animator.GetBool("isCrouch"))
            {
                // Player's posture is broken.
                if (player.postureCurrent <= 0)
                {
                    player.nextPlayerHitReaction = 8;
                }

                // Ensures that the player goes into blocking state.
                player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 5, player.nextPlayerHitStunDuration, 7);
            }

            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.HitReactionState);
        }
        else if(collision.tag == "BossAttackHigh")
        {
            // Gets Hit by High Attacks.
            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.HitReactionState);
        }

        if (collision.tag == "BossFireball")
        {
            // Gets hit by Fireball. (Cannot block fireball while crouching)
            player.nextPlayerHitReaction = 11;
            player.SwitchState(player.HitReactionState);
        }

    }

    public override void OnParryPerformed(PlayerStateManager player)
    {
        player.animator.SetBool("isCrouch", false);
        player.SwitchState(player.ParryAttemptState);
    }
    public override void OnRegularAttackPerformed(PlayerStateManager player)
    {
        player.animator.SetBool("isCrouch", false);
        player.SwitchState(player.RegularAttackState);
    }

}

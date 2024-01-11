using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Crouch State");
        player.animator.SetBool("isCrouch", true);
        player.attackCounter = 0;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput.y >= 0)
        {
            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.IdleState);
        }

        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.SwitchState(player.RegularAttackState);
            //player.animator.SetBool("isCrouch", false);
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

    }
}

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
            player.animator.SetBool("isCrouch", false);
            player.shouldLowBlock = true;
            player.shouldHighBlock = false;
            player.shouldFireballBlock = false;
            player.SwitchState(player.BlockState);
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
    public override void OnSpecialAttackPerformed(PlayerStateManager player)
    {
        player.animator.SetBool("isCrouch", false);
        player.SwitchState(player.SpecialAttackState);
    }

}

using UnityEngine;

public class PlayerSpecialAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Special Attack, State");

        // Check which special move is used.
        if(player.movementInput != Vector2.zero)
        {
            // Right Special.
            if(player.movementInput.x > 0)
            {
                if(player.spriteFlip)
                {
                    // Dragon Punch.
                    player.animator.SetTrigger("triggerSpecialThree");
                    DragonPunchForce(player);
                }
                else
                {
                    // Tatsu. Spinning Kick.
                    player.animator.SetTrigger("triggerSpecialTwo");
                    SpinningKickForce(player);
                }
            }
            else
            {
                // Left Special.
                if (player.spriteFlip)
                {
                    // Tatsu. Spinning Kick.
                    player.animator.SetTrigger("triggerSpecialTwo");
                    SpinningKickForce(player);
                }
                else
                {
                    // Dragon Punch.
                    player.animator.SetTrigger("triggerSpecialThree");
                    DragonPunchForce(player);
                }
            }
        }
        else
        {
            // Neutral Direction. (Fireball)
            player.animator.SetTrigger("triggerSpecialOne");
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    private void DragonPunchForce(PlayerStateManager player)
    {
        player.rb.AddForce(new Vector2(1.5f * player.forceDirection, 5), ForceMode2D.Impulse);
    }

    private void SpinningKickForce(PlayerStateManager player)
    {
        player.rb.AddForce(new Vector2(1.5f * player.forceDirection, 0), ForceMode2D.Impulse);
    }
}

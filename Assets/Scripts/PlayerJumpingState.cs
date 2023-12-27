using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.isLanded = false;
        if(player.movementInput.x == 0)
        {
            // Neutral Jump.
            player.rb.AddForce(new Vector2(player.rb.velocity.x, player.jumpForce), ForceMode2D.Impulse);
            player.animator.SetTrigger("JumpNeutral");
        }
        else
        {
            if(player.movementInput.x > 0)
            {
                // Diagonal Right/Back Jump. (Back Diagonal Jump have same animation as Neutral Jump)
                player.rb.AddForce(new Vector2(player.diagonalJumpForce, player.jumpForce), ForceMode2D.Impulse);
                player.animator.SetTrigger("JumpNeutral");
            }
            else
            {
                // Diagonal Left/Forward Jump. 
                player.rb.AddForce(new Vector2(-player.diagonalJumpForce, player.jumpForce), ForceMode2D.Impulse);
                player.animator.SetTrigger("JumpDiagonal");
            }
        }
        Debug.Log("Entered Jumping State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.isLanded || (player.rb.velocity.y == 0))
        {
            // Player has Landed.
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

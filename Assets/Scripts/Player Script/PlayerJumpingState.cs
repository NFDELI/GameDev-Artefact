using UnityEditor;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.isLanded = false;
        player.playerBoxCollider2D.enabled = false;
        player.playerAirCollider2D.enabled = true;
        if(player.movementInput.x == 0)
        {
            // Neutral Jump.
            player.rb.AddForce(new Vector2(player.rb.linearVelocity.x, player.jumpForce), ForceMode2D.Impulse);
            player.animator.SetTrigger("JumpNeutral");
            player.audioScript.PlayJumpSound();
        }
        else
        {
            if(player.movementInput.x > 0)
            {
                // Diagonal Right/Back Jump. (Back Diagonal Jump have same animation as Neutral Jump)
                player.rb.AddForce(new Vector2(player.diagonalJumpForce, player.jumpForce), ForceMode2D.Impulse);
                if(player.shouldFlip)
                {
                    player.animator.SetTrigger("JumpDiagonal");
                }
                else
                {
                    player.animator.SetTrigger("JumpNeutral");
                }
            }
            else
            {
                // Diagonal Left/Forward Jump. 
                player.rb.AddForce(new Vector2(-player.diagonalJumpForce, player.jumpForce), ForceMode2D.Impulse);
                if(player.shouldFlip)
                {
                    player.animator.SetTrigger("JumpNeutral");
                }
                else
                {
                    player.animator.SetTrigger("JumpDiagonal");
                }
            }
        }
        Debug.Log("Entered Jumping State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.isLanded || (player.rb.linearVelocity.y == 0))
        {
            // Player has Landed.
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if(collision.tag == "BossAttackHigh")
        {
            player.rb.totalForce = Vector2.zero;
            player.rb.linearVelocity = Vector2.zero;

            // Player is already in the air, don't send him flying.
            player.nextPlayerForceReceived = new Vector2(player.nextPlayerForceReceived.x, player.nextPlayerForceReceived.y / 2);
            player.SwitchState(player.HitReactionState);
        }
        if (collision.tag == "BossAttackLow")
        {
            player.rb.totalForce = Vector2.zero;
            player.rb.linearVelocity = Vector2.zero;
            player.SwitchState(player.HitReactionState);
        }
        if (collision.tag == "BossFireball")
        {
            player.rb.totalForce = Vector2.zero;
            player.rb.linearVelocity = Vector2.zero;
            player.nextPlayerHitReaction = 11;
            player.SwitchState(player.HitReactionState);
        }
        if (collision.tag == "BossAttackUnblockable")
        {
            player.rb.totalForce = Vector2.zero;
            player.rb.linearVelocity = Vector2.zero;
            player.SwitchState(player.HitReactionState);
        }
    }

    public override void OnRegularAttackPerformed(PlayerStateManager player)
    {
        player.animator.SetTrigger("triggerJumpAttack");
        player.AttackHitProperty(15, new Vector2(15f, 0), 0, 1.5f, 5);
    }
}

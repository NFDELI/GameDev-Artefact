using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Idle State");
        player.animator.SetTrigger("triggerIdle");
        player.spriteRenderer.color = Color.white;
        player.isLanded = true;
        player.isInvincible = false;
        player.rb.velocity = new Vector2(0, 0);
        player.attackCounter = 1;
        player.wasBlocking = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput != Vector2.zero)
        {
            // Check for Movement Input.
            if(player.movementInput.y == 0)
            {
                // Walking.
                player.SwitchState(player.WalkingState);
            }
            else if(player.movementInput.y > 0 && player.isLanded)
            {
                // Jumping.
                player.SwitchState(player.JumpingState);
            }
            else if(player.movementInput.y < 0)
            {
                // Crouching.
                player.SwitchState(player.CrouchState);
            }
        }

        // Check for Attack Input.
        if(Input.GetKeyDown(KeyCode.U))
        {
            player.SwitchState(player.RegularAttackState);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            player.SwitchState(player.SpecialAttackState);
        }

        // Check for Parry Input.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.SwitchState(player.ParryAttemptState);
        }

        // Sprite-Flip Check.
        if(player.spriteFlip)
        {
            player.spriteRenderer.flipX = true;
            player.attackBoxCollider.offset = new Vector2(0.95f, 0);
        }
        else
        {
            player.spriteRenderer.flipX = false;
            player.attackBoxCollider.offset = new Vector2(0, 0);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if(collision.tag == "BossAttackHigh")
        {
            player.SwitchState(player.HitReactionState);
        }
    }
}

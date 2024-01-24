using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Idle State");
        player.animator.SetBool("isWalkTowards", false);
        player.animator.SetBool("isWalkBackwards", false);
        player.animator.SetBool("isCrouch", false);
        player.animator.SetTrigger("triggerIdle");
        player.playerAirCollider2D.enabled = false;
        player.spriteRenderer.color = Color.white;
        player.isLanded = true;
        player.isInvincible = false;
        player.rb.velocity = new Vector2(0, 0);
        player.attackCounter = 1;
        player.wasBlocking = false;

        player.playerBoxCollider2D.enabled = true;
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
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.SwitchState(player.ParryAttemptState);
        //}

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

        if(player.bossStateManager.health <= 0)
        {
            player.SwitchState(player.IntroductionState);
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
        if(collision.tag == "BossAttackLow")
        {
            player.SwitchState(player.HitReactionState);
        }
        if(collision.tag == "BossFireball")
        {
            player.nextPlayerHitReaction = 11;
            player.SwitchState(player.HitReactionState);
        }
        if(collision.tag == "BossAttackUnblockable")
        {
            player.SwitchState(player.HitReactionState);
        }
    }

    public override void OnParryPerformed(PlayerStateManager player)
    {
        player.SwitchState(player.ParryAttemptState);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Walking State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput == Vector2.zero)
        {
            // No Movement Input. (Movement Input Stopped)
            StopMovingAnimation(player);
            player.SwitchState(player.IdleState);
        }
        else 
        {
            // Movement Input Detected.
            if(player.movementInput.x > 0)
            {
                // Move Right.
                if(player.spriteFlip)
                {
                    player.animator.SetBool("isWalkTowards", true);
                    player.animator.SetBool("isWalkBackwards", false);
                }
                else
                {
                    player.animator.SetBool("isWalkTowards", false);
                    player.animator.SetBool("isWalkBackwards", true);
                }
                player.rb.MovePosition(player.rb.position + new Vector2(1, 0) * player.movementSpeed * Time.fixedDeltaTime);

            }
            else if(player.movementInput.x < 0)
            {
                // Move Left.
                if(player.spriteFlip) 
                {
                    player.animator.SetBool("isWalkBackwards", true);
                    player.animator.SetBool("isWalkTowards", false);
                }
                else
                {
                    player.animator.SetBool("isWalkBackwards", false);
                    player.animator.SetBool("isWalkTowards", true);
                }
                player.rb.MovePosition(player.rb.position + new Vector2(-1, 0) * player.movementSpeed * Time.fixedDeltaTime);
            }

            if(player.movementInput.y > 0)
            {
                // Jump.
                StopMovingAnimation(player);
                player.SwitchState(player.JumpingState);
            }
            else if(player.movementInput.y < 0)
            {
                // Crouch.
                StopMovingAnimation(player);
                player.SwitchState(player.CrouchState);
            }
        }

        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U))
        {
            StopMovingAnimation(player);
            player.SwitchState(player.RegularAttackState);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StopMovingAnimation(player);
            player.SwitchState(player.SpecialAttackState);
        }

        // Check for Parry Input.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SwitchState(player.ParryAttemptState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        //Debug.Log("Collision Detected while Walking");
        if(collision.tag == "BossAttackHigh")
        {
            if (player.animator.GetBool("isWalkBackwards"))
            {
                // Ensures that the player goes into blocking state.
                player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 4, player.nextPlayerHitStunDuration, 7);
            }
            player.SwitchState(player.HitReactionState);
        }
    }

    public void MoveRight(PlayerStateManager player)
    {
        player.animator.SetBool("isWalkTowards", false);
        player.animator.SetBool("isWalkBackwards", true);
        player.rb.MovePosition(player.rb.position + new Vector2(1, 0) * player.movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveLeft(PlayerStateManager player) 
    {
        player.animator.SetBool("isWalkBackwards", false);
        player.animator.SetBool("isWalkTowards", true);
        player.rb.MovePosition(player.rb.position + new Vector2(-1, 0) * player.movementSpeed * Time.fixedDeltaTime);
    }

    private void StopMovingAnimation(PlayerStateManager player)
    {
        player.animator.SetBool("isWalkTowards", false);
        player.animator.SetBool("isWalkBackwards", false);
    }
}

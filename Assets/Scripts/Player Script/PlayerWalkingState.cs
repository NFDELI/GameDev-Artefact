using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private float currentWalkingSpeed;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Walking State");
        StopMovingAnimation(player);
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
                    currentWalkingSpeed = 1f;
                }
                else
                {
                    player.animator.SetBool("isWalkTowards", false);
                    player.animator.SetBool("isWalkBackwards", true);
                    currentWalkingSpeed = 0.75f;
                }
                player.rb.MovePosition(player.rb.position + new Vector2(1 * currentWalkingSpeed, 0) * player.movementSpeed * Time.fixedDeltaTime);

            }
            else if(player.movementInput.x < 0)
            {
                // Move Left.
                if(player.spriteFlip) 
                {
                    player.animator.SetBool("isWalkBackwards", true);
                    player.animator.SetBool("isWalkTowards", false);
                    currentWalkingSpeed = 0.75f;
                }
                else
                {
                    player.animator.SetBool("isWalkBackwards", false);
                    player.animator.SetBool("isWalkTowards", true);
                    currentWalkingSpeed = 1f;
                }
                player.rb.MovePosition(player.rb.position + new Vector2(-1 * currentWalkingSpeed, 0) * player.movementSpeed * Time.fixedDeltaTime);
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
        // Blocks High Attacks.
        if(collision.tag == "BossAttackHigh")
        {
            if (player.animator.GetBool("isWalkBackwards"))
            {
                // Ensures that the player goes into blocking state.
                player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 4, player.nextPlayerHitStunDuration, 7);

                // Player's posture is broken by posture-chip damage.
                if (player.postureCurrent <= 0)
                {
                    player.nextPlayerHitReaction = 8;
                }
            }

            player.SwitchState(player.HitReactionState);
        }

        // Gets Hit by Low Attacks.
        if(collision.tag == "BossAttackLow")
        {
            player.SwitchState(player.HitReactionState);
        }

        if(collision.tag == "BossFireball")
        {
            // Fireball have special states/attributes.
            if (player.animator.GetBool("isWalkBackwards"))
            {
                if (player.postureCurrent <= 0)
                {
                    player.nextPlayerHitReaction = 8;
                }
                else
                {
                    player.nextPlayerHitReaction = 12;
                }
            }
            else
            {
                player.nextPlayerHitReaction = 11;
            }
            player.SwitchState(player.HitReactionState);
        }

        // Unblockable attacks cannot be blocked. (Posture Break the Player.)
        if (collision.tag == "BossAttackUnblockable")
        {
            // Player sacrificed Posture, so no damage is taken.
            player.postureCurrent = 0;
            player.nextPlayerDamageReceived = 0;
            player.nextPlayerHitReaction = 8;
            player.SwitchState(player.HitReactionState);
        }
    }

    private void StopMovingAnimation(PlayerStateManager player)
    {
        player.animator.SetBool("isWalkTowards", false);
        player.animator.SetBool("isWalkBackwards", false);
    }
}

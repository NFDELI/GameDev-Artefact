using UnityEditor.Build;
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
                    player.AttackHitProperty(3, new Vector2(20f, 85f), 13, 999, 5);
                    player.isUnblockableCounter = true;
                    player.rb.velocity = Vector2.zero;
                }
                else
                {
                    // Tatsu. Spinning Kick.
                    player.animator.SetTrigger("triggerSpecialTwo");

                    // The Y-Axis Velocity is Negative due to a bug. -Jan 17 2024.
                    player.AttackHitProperty(3, new Vector2(15f, 70f), 12, 999, 5);
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
                    player.AttackHitProperty(3, new Vector2(15f , 70f), 12, 999, 5);
                    SpinningKickForce(player);
                }
                else
                {
                    // Dragon Punch.
                    player.animator.SetTrigger("triggerSpecialThree");
                    player.AttackHitProperty(3, new Vector2(20f, 85f), 13, 999, 5);
                    player.isUnblockableCounter = true;
                    player.rb.velocity = Vector2.zero;
                }
            }
            if(player.movementInput.y < 0)
            {
                // Attempt to use Super Dragon Punch.
                if((player.superCurrent >= player.superMax) && !player.isUnblockableCounter)
                {
                    // Super Dragon Punch. (Hit Properties overriden in Animator)
                    player.audioScript.PlayUnblockableWarningSound();
                    player.bossStateManager.nextHitReceiveSuper = true;
                    player.animator.SetTrigger("triggerSuperSpecialOne");
                    player.isUnblockableCounter = true;
                    player.rb.velocity = Vector2.zero;
                    player.superCurrent = 0;
                }
                else
                {
                    // Super Bar not Ready.
                    player.audioScript.PlaySuperBarNotReadySound();
                    player.SwitchState(player.IdleState);
                }
            }
        }
        else
        {
            // Neutral Direction. (Fireball)
            if (player.fireballScript.isSpawned)
            {
                // Do not allow the player to spawn multiple fireballs at the scene.
                player.SwitchState(player.IdleState);
            }
            else
            {
                // Fireball is spawned from Animator.
                player.animator.SetTrigger("triggerSpecialOne");
                player.AttackHitProperty(3, new Vector2(5f, 0), 1, 0.5f, 12);
            }
            
            // Spawn Fireball. (Fireball is Spawned from the Animator)
            //player.SpawnFireball();
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.isSpinnigKickForce)
        {
            // Apply the x-velocity while doing a spinning kick.
            player.rb.MovePosition(player.rb.position + new Vector2(player.spinningKickSpeed * player.forceDirection, 0) * player.movementSpeed * Time.fixedDeltaTime);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if (collision.tag == "BossAttackHigh")
        {
            player.SwitchState(player.HitReactionState);
            player.attackBoxCollider.enabled = false;
            player.PlayerLandingTrue();
        }
        if (collision.tag == "BossAttackLow")
        {
            player.SwitchState(player.HitReactionState);
            player.attackBoxCollider.enabled = false;
            player.PlayerLandingTrue();
        }
        if (collision.tag == "BossFireball")
        {
            player.nextPlayerHitReaction = 11;
            player.SwitchState(player.HitReactionState);
            player.attackBoxCollider.enabled = false;
            player.PlayerLandingTrue();
        }
        if (collision.tag == "BossAttackUnblockable")
        {
            player.SwitchState(player.HitReactionState);
            player.attackBoxCollider.enabled = false;
            player.PlayerLandingTrue();
        }
    }

    private void DragonPunchForce(PlayerStateManager player)
    {
        player.rb.AddForce(new Vector2(1.5f * player.forceDirection, 5), ForceMode2D.Impulse);
    }

    private void SpinningKickForce(PlayerStateManager player)
    {
        player.rb.AddForce(new Vector2(1.5f * player.forceDirection, 0), ForceMode2D.Impulse);
    }
    public override void OnSpecialAttackPerformed(PlayerStateManager player)
    {
        // Check if player can do a supermove cancel.
        if (player.movementInput.y < 0 && !player.bossStateManager.nextHitReceiveSuper)
        {
            // Check if player has enough super metre to do the Super Move.
            if((player.superCurrent >= player.superMax) && !player.isUnblockableCounter)
            {
                player.FlagSpinningKickEnd();
                player.rb.totalForce = Vector2.zero;
                player.SwitchState(player.SpecialAttackState);
                player.superCurrent = 0;
            }
            else
            {
                player.audioScript.PlaySuperBarNotReadySound();
            }
        }
    }
}

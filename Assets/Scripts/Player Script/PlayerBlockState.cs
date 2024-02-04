using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockState : PlayerBaseState
{
    float blockSuperGain = 0.25f;
    float blockStunTime;
    Vector2 blockForce;
    float blockDamage;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Blocking State");

        player.StopMovingAnimation();
        player.TurnOffDarkenEffect();
        player.SuperTimeStop();

        if(player.shouldFireballBlock)
        {
            // Blocking Fireballs.
            player.audioScript.PlayFireballBlockSound();
            blockStunTime = 0.25f;
            blockForce = player.bossStateManager.fireballScript.knockbackForce / 2;
            blockDamage = player.bossStateManager.fireballScript.damage * 0.25f;
            player.postureCurrent -= 0.6f;
        }
        else
        {
            // Blocking Other Attacks.
            player.audioScript.PlayBlockAttackSound();
            blockStunTime = player.nextPlayerHitStunDuration / 2;
            blockForce = player.nextPlayerForceReceived;
            blockForce = new Vector2(blockForce.x / 2, blockForce.y / 3);
            blockDamage = player.nextPlayerDamageReceived / 2;
            player.postureCurrent -= 0.5f;
        }

        player.ChangeSuperAmount(blockSuperGain);

        if(player.postureCurrent <= 0)
        {
            player.SwitchState(player.PlayerPostureBroken);
        }

        player.spriteRenderer.color = Color.white;

        if(player.shouldHighBlock)
        {
            player.animator.SetTrigger("triggerBlockHigh");
        }
        else if(player.shouldLowBlock)
        {
            player.animator.SetTrigger("triggerBlockLow");
        }
        else
        {
            player.animator.SetTrigger("triggerBlockHigh");
        }

        // Apply Hit Attributes Here.
        player.health -= blockDamage;
        player.rb.AddForce(blockForce, ForceMode2D.Impulse);

        // Turn off Boss's attack hitbox so that it can be re-registered for the next hit.
        player.bossStateManager.attackHighBoxCollider2D.enabled = false;
        player.bossStateManager.attackLowBoxCollider2D.enabled = false;
        player.bossStateManager.attackUnblockableBoxCollider2D.enabled = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(blockStunTime > 0)
        {
            blockStunTime -= Time.deltaTime;
        }
        else
        {
            // Block stun finished.
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
            ResetBlockBooleans(player);
            if (player.shouldLowBlock)
            {
                player.SwitchState(player.BlockState);
            }
            else
            {
                player.SwitchState(player.HitReactionState);
            }
        }

        if (collision.tag == "BossAttackHigh")
        {
            ResetBlockBooleans(player);
            if (player.shouldHighBlock)
            {
                player.SwitchState(player.BlockState);
            }
            else
            {
                player.SwitchState(player.HitReactionState);
            }
        }

        if (collision.tag == "BossFireball")
        {
            ResetBlockBooleans(player);
            if (player.shouldHighBlock)
            {
                player.SwitchState(player.BlockState);
            }
            else
            {
                player.SwitchState(player.HitReactionState);
            }
        }

        if(player.postureCurrent <= 0)
        {
            ResetBlockBooleans(player);
            player.SwitchState(player.PlayerPostureBroken);
        }

    }

    private void ResetBlockBooleans(PlayerStateManager player)
    {
        player.shouldHighBlock = false;
        player.shouldLowBlock = false;
        player.shouldFireballBlock = false;

        Debug.LogError("RESET BOOLEANS CALLED");
    }
}

using System;
using System.Collections;
using UnityEngine;

public class PlayerHitReactionState : PlayerBaseState
{
    private int hitReactionIndex;
    private float hitStunTime;
    private float hitDamage;
    private float blockSuperGain = 0.25f;
    private float gettingHitSuperGain = 0.75f;
    private float parryingSuperGain = 0.5f;
    private Vector2 hitForce;
    private bool timerStarted = false;
    private bool successfulParry = false;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Hit Reaction State");

        player.StopMovingAnimation();
        player.TurnOffDarkenEffect();
        player.SuperTimeStop();

        hitReactionIndex = player.nextPlayerHitReaction;
        hitStunTime = player.nextPlayerHitStunDuration;
        hitForce = player.nextPlayerForceReceived;
        hitDamage = player.nextPlayerDamageReceived;

        player.spriteRenderer.color = Color.white;
        //player.spriteRenderer.color = Color.red;

        switch (hitReactionIndex)
        {
            case 0:
                // Get Hit High.
                player.animator.SetTrigger("triggerHitReactionHigh");
                timerStarted = true;
                player.ChangeSuperAmount(gettingHitSuperGain);

                Debug.LogError("TRIGGERED HIGH");
                break;
            case 1:
                // Get Hit Low.
                player.animator.SetTrigger("triggerHitReactionLow");
                timerStarted = true;
                player.ChangeSuperAmount(gettingHitSuperGain);
                Debug.LogError("TRIGGERED LOW");
                break;
            case 2:
                // Get Hit then Fall.
                player.animator.SetTrigger("triggerFall");
                break;
            case 3:
                // Grabbed.
                break;
            case 4:
                // High Block.
                player.animator.SetTrigger("triggerBlockHigh");
                timerStarted = true;
                //player.spriteRenderer.color = Color.yellow;

                if (hitStunTime == 999)
                {
                    hitStunTime = 0.9f;
                }

                // Ensure that the player takes less damage while blocking and take less knockback.
                hitDamage = hitDamage / 4;
                hitForce = new Vector2(hitForce.x / 2, hitForce.y / 3);
                //player.LosePosture(0.5f);

                player.postureCurrent -= 0.5f;

                player.wasBlocking = true;
                successfulParry = false;
                player.ChangeSuperAmount(blockSuperGain);
                break;
            case 5:
                // Low Block.
                player.animator.SetTrigger("triggerBlockLow");
                timerStarted = true;
                player.spriteRenderer.color = Color.yellow;

                // Ensure that the player takes less damage while blocking and take less knockback.
                hitDamage = hitDamage / 4;
                hitForce = new Vector2(hitForce.x / 2, hitForce.y / 3);
                //player.LosePosture(0.5f);
                player.postureCurrent -= 0.5f;
                player.ChangeSuperAmount(blockSuperGain);
                successfulParry = false;
                break;
            case 6:
                // High Parry.
                player.animator.SetTrigger("triggerParryHigh");
                player.spriteRenderer.color = Color.blue;
                player.bossStateManager.TakePostureDamage(1f);
                player.GainPosture();
                player.ChangeSuperAmount(parryingSuperGain);
                successfulParry = true;
                break;
            case 7:
                // Low Parry.
                player.animator.SetTrigger("triggerParryLow");
                player.spriteRenderer.color = Color.blue;
                player.bossStateManager.TakePostureDamage(1f);
                player.GainPosture();
                player.ChangeSuperAmount(parryingSuperGain);
                successfulParry = true;
                break;
            case 8:
                // Guard/Posture/Grab Break.
                player.wasBlocking = false;
                player.animator.SetTrigger("triggerPostureBreak");
                timerStarted = true;
                player.audioScript.PlayPlayerPostureBreakVoice();
                player.nextPlayerForceReceived = Vector2.zero;
                hitForce = Vector2.zero;

                hitDamage = 0;

                break;
            case 9:
                // Dazed/Stunned.
                player.wasBlocking = false;
                player.animator.SetTrigger("triggerStunned");
                timerStarted = true;

                // Boss should immediately punish the player.
                player.bossStateManager.shouldResetAiTimer = false;
                player.bossStateManager.aiDecisionTimer = 0;
                player.bossStateManager.InstantAttackAIDelayed(0.3f);
                player.nextPlayerHitSoundIndex = -1;
                Debug.LogError("Player Stunned!");
                break;
            case 10:
                // Get Up Animation.
                player.animator.SetTrigger("triggerGetUp");
                break;
            case 11:
                // Get Hit by Fireball. (Fireball has special hit sounds and effects)
                player.animator.SetTrigger("triggerHitReactionHigh");
                timerStarted = true;
                player.nextPlayerHitSoundIndex = 12;
                hitForce = player.bossStateManager.fireballScript.knockbackForce;
                hitDamage = player.bossStateManager.fireballScript.damage;
                hitStunTime = player.bossStateManager.fireballScript.stunDuration;

                player.ChangeSuperAmount(gettingHitSuperGain);
                successfulParry = false;
                break;
            case 12:
                // Blocks Fireball Attack.
                player.animator.SetTrigger("triggerBlockHigh");
                timerStarted = true;
                player.nextPlayerHitSoundIndex = 11;
                hitStunTime = 0.25f;
                hitForce = player.bossStateManager.fireballScript.knockbackForce / 2;
                hitDamage = player.bossStateManager.fireballScript.damage * 0.25f;
                //player.LosePosture(0.5f);

                player.postureCurrent -= 0.6f;
                player.wasBlocking = true;
                successfulParry = false;

                player.ChangeSuperAmount(blockSuperGain);
                break;
            case 13:
                // Parry a Fireball. (Does not do posture damage to the boss)
                player.animator.SetTrigger("triggerParryHigh");
                player.spriteRenderer.color = Color.blue;
                player.GainPosture();

                player.ChangeSuperAmount(parryingSuperGain);
                successfulParry = true;
                player.wasBlocking = false;
                break;
            case 14:
                // Player gets hit by Dragon Punch.
                player.animator.SetTrigger("triggerLaunched");
                player.rb.totalForce = Vector2.zero;
                player.SetIsLaunchedWithDelay(0.4f);

                player.ChangeSuperAmount(gettingHitSuperGain);
                Debug.LogError("TRIGGERED LAUNCHED");
                break;
            default:
                break;
        }

        // Apply Hit Attributes Here.
        player.health -= hitDamage;
        player.rb.AddForce(hitForce, ForceMode2D.Impulse);
        player.audioScript.SoundIndexPlay(player.nextPlayerHitSoundIndex);

        // Turn off Boss's attack hitbox so that it can be re-registered for the next hit.
        player.bossStateManager.attackHighBoxCollider2D.enabled = false;
        player.bossStateManager.attackLowBoxCollider2D.enabled = false;
        player.bossStateManager.attackUnblockableBoxCollider2D.enabled = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput != Vector2.zero)
        {
            // Movement Input Detected.
            if(player.canGetUp && !(player.health <= 0))
            {
                // To make the player get up from falling.
                player.nextPlayerHitReaction = 10;
                player.nextPlayerHitStunDuration = 999f;
                player.nextPlayerHitSoundIndex = -1;
                player.nextPlayerDamageReceived = 0;
                player.nextPlayerForceReceived = Vector2.zero;
                player.canGetUp = false;
                player.bossStateManager.canAntiAirAgain = true;
                player.SwitchState(player.HitReactionState);
            }
        }

        if(hitReactionIndex != 14)
        {
            if (timerStarted)
            {
                hitStunTime -= Time.deltaTime;
                if (hitStunTime <= 0) { StunFinished(player); }
            }
        }

        if(player.isLaunched)
        {
            player.playerBoxCollider2D.enabled = false;
            player.playerAirCollider2D.enabled = true;

            // Player's body has reached its peak.
            if (player.rb.velocity.y < 0)
            {
                player.animator.SetTrigger("triggerPeaked");
            }
        }

        if (player.health <= 0)
        {
            // Player is Dead
            player.SwitchState(player.IntroductionState);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(hitStunTime);
        }

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
        if(!player.isInvincible)
        {
            // Player gets hit by an attack again.
            if (collision.tag == "BossAttackHigh")
            {
                timerStarted = false;

                if(player.wasBlocking)
                {
                    // Ensures that the player goes into blocking state.
                    player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 4, player.nextPlayerHitStunDuration, 7);
                    if (player.postureCurrent <= 0)
                    {
                        player.nextPlayerDamageReceived = 0;
                        player.nextPlayerHitReaction = 8;
                        player.nextPlayerHitStunDuration = 4;
                    }
                }
                player.SwitchState(player.HitReactionState);
            }
            if(collision.tag == "BossAttackLow")
            {
                timerStarted = false;
                player.SwitchState(player.HitReactionState);
            }
            if (collision.tag == "BossAttackUnblockable")
            {
                player.SwitchState(player.HitReactionState);
            }
            if (collision.tag == "BossFireball")
            {
                if (player.wasBlocking && player.postureCurrent > 0)
                {
                    // Ensures that the player goes into blocking state.
                    player.AttackHitPropertySelf(player.nextPlayerDamageReceived, player.nextPlayerForceReceived, 12, player.nextPlayerHitStunDuration, 7);
                }
                else
                {
                    player.nextPlayerHitReaction = 11;
                }

                player.SwitchState(player.HitReactionState);
            }

        }
    }

    private void StunFinished(PlayerStateManager player)
    {
        if (player.movementInput != Vector2.zero)
        {
            if(player.movementInput.y == 0 && hitReactionIndex != 8)
            {
                // Change State to walking. (Gives access to block) 
                player.SwitchState(player.WalkingState);
            }
        }
        else
        {
            player.animator.SetTrigger("triggerIdle");
            player.SwitchState(player.IdleState);
        }

        //player.SwitchState(player.IdleState);
    }

    public override void OnParryPerformed(PlayerStateManager player)
    {
        if(successfulParry)
        {
            player.SwitchState(player.ParryAttemptState);
            Debug.LogWarning("ATTEMPTED SUCCESSFUL PARRY");
        }
    }

}

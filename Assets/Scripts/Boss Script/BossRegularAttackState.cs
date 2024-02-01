using System;
using UnityEngine;

public class BossRegularAttackState : BossBaseState
{
    int teleportDirection = 1;
    float teleportDuration;
    int attackPatternChoice;
    int currentAttackPatternIndex;
    int[] attackPatternChosen;


    int[] attackPatternOne = { 1, 2, 3, 4, 0 };  // The Player's Combo.
    int[] attackPatternTwo = { 6, 7, 5, 0 };    // Low kicks attack Combo.   
    int[] attackPatternThree = { 2, 3, 4, 0 }; // Mix-up ending High.
    int[] attackPatternFour = { 2, 3, 5, 0};  // Mix-up ending Low.
    int[] attackPatternFive = { 8, 0 }; // Unblockable-Delayed Punch.


    int[] attackPatternSix = { 20, 0 }; // Fireball.
    int[] attackPatternSeven = { 21, 0 }; // Dragon-Punch.
    int[] attackPatternEight = { 1, 2, 22, 1, 2, 3, 4, 0}; // Teleport Combo.


    int[] attackPatternNine = { 23, 20, 0 }; // Teleport Away from the player, then fireball. 
    int[] attackPatternTen = { 9, 0 }; // Long-Distance Unblockable.
    int[] attackPatternEleven = { 23, 9, 0 }; // Teleport away, then Long-Range Unblockable.

    int[] attackPatternTwelve = { 24, 0 }; // Anti Air - A faster Dragon Punch.

    int[] attackPatternSixFourteen = { 3, 10, 8, 0};

    // Phase-2 Attack Strings.
    int[] attackPatternSixThirteen = { 11, 10, 22, 6, 3, 5, 0 };
    int[] attackPatternSevenFifteen = { 11, 4, 23, 20, 0 };


    int[] relaxAttackPattern = { 0 }; // Do nothing, allow the player to poke.

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Regular Attack State");
        boss.canAttackChain = true;
        currentAttackPatternIndex = 0;
        boss.rangeCheckBox.enabled = false;
        boss.bossAntiAirBoxCollider2D.enabled = false;

        // Reset BlockUntilParry. (Give the player a chance to poke again.)
        boss.blocksUntilParry = boss.blocksUntilParryDefault;

        boss.ResetBlockUntilParry(2, 4);

        if (boss.nextAttackPatternChoice == -1)
        {
            // No attack pattern Forced.
            attackPatternChoice = UnityEngine.Random.Range(1, 9);
        }
        else
        {
            // An Attack pattern has already been chosen.
            attackPatternChoice = boss.nextAttackPatternChoice;
        }

        // Override Attack Pattern Choice for Debugging.
        attackPatternChoice = 2;

        // Choose a Random Attack Pattern.
        switch (attackPatternChoice)
        {
            case 1:
                attackPatternChosen = attackPatternOne;
                break;
            case 2:
                attackPatternChosen = attackPatternTwo;
                break;
            case 3:
                attackPatternChosen = attackPatternThree;
                break;
            case 4:
                attackPatternChosen = attackPatternFour;
                break;
            case 5:
                attackPatternChosen = attackPatternFive;
                break;
            case 6:
                // Never do Fireball on its own.
                //attackPatternChosen = attackPatternSix;

                if(boss.isPhaseTwo)
                {
                    attackPatternChosen = attackPatternSixThirteen;
                }
                else
                {
                    attackPatternChosen = attackPatternSixFourteen;
                }

                break;
            case 7:
                // Never do Dragon Punch on its own.
                //attackPatternChosen = attackPatternSeven;

                if(boss.isPhaseTwo)
                {
                    attackPatternChosen = attackPatternSevenFifteen;
                }
                else
                {
                    attackPatternChosen = relaxAttackPattern;
                }
                break;
            case 8:
                attackPatternChosen = attackPatternEight;
                break;
            case 9:
                attackPatternChosen = attackPatternNine;
                break;
            case 10:
                attackPatternChosen = attackPatternTen;
                break;
            case 100:
                // Long Range Mixup here!
                currentAttackPatternIndex = 0;
                int chance = UnityEngine.Random.Range(1, 3);
                if(chance == 1)
                {
                    if(boss.isNearPlayer)
                    {
                        // Teleport Back, then Fireball.
                        attackPatternChosen = attackPatternNine;
                    }
                    else
                    {
                        // Immediately do Fireball.
                        attackPatternChosen = attackPatternSix;
                    }
                }
                else
                {
                    if (boss.isNearPlayer)
                    {
                        // Teleport Back, then Unblockable Punch.
                        attackPatternChosen = attackPatternEleven;
                    }
                    else
                    {
                        // Immediately Unblockable Punch.
                        attackPatternChosen = attackPatternTen;
                    }
                }
                boss.nextAttackPatternChoice = -1;
                break;
            case 120:
                // Perform anti-Air.
                attackPatternChosen = attackPatternTwelve;
                break;
            default:
                break;
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (boss.canAttackChain)
        {
            if (attackPatternChosen[currentAttackPatternIndex] == 0)
            {
                // End of Attack Pattern.
                attackPatternChoice = -1;
                boss.shouldResetAiTimer = true;
                boss.SwitchState(boss.IdleState);
            }
            else
            {
                PerformAttack(boss, attackPatternChosen[currentAttackPatternIndex]);
                currentAttackPatternIndex++;
                boss.canAttackChain = false;
            }
        }

        if(boss.isTeleporting)
        {
            if(teleportDuration > 0)
            {
                // Boss is Teleporting.
                boss.rb.MovePosition(boss.rb.position + new Vector2(5 * teleportDirection, 0) * boss.movementSpeed * Time.fixedDeltaTime);
                // Boss still has time to move teleporting.
                teleportDuration -= Time.deltaTime;
            }
            else
            {
                // Stop Teleporting.
                boss.isTeleporting = false;
                boss.spriteRenderer.color = Color.white;
                boss.bossBoxCollider2D.enabled = true;
                //boss.SwitchState(boss.IdleState);
                boss.shouldResetAiTimer = false;
                boss.canAttackChain = true;
                boss.ReTrackPlayer();
            }
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHigh" || collision.tag == "Fireball")
        {
            if (boss.playerStateManager.isUnblockableCounter)
            {
                // This attack can be countered by Player Dragon Punch.
                boss.SwitchState(boss.HitReactionState);
            }
            else
            {
                // Make sure boss has armor.
                boss.audioScript.PlayArmorSound();
                boss.FlashBlue();

                boss.health -= boss.nextBossDamageReceived * 0.25f;

                // Boss cannot be launched while he is attacking.
                boss.nextBossForceReceived = new Vector2(boss.nextBossForceReceived.x, 0f);
                boss.rb.AddForce(boss.nextBossForceReceived * 0.5f, ForceMode2D.Impulse);
            }
        }
    }

    public void PerformAttack(BossStateManager boss, int attackId)
    {
        // The 'attackId' determines the next attack being performed. 
        switch (attackId)
        {
            case 0:
                boss.animator.SetTrigger("triggerAttackCrouch");
                boss.rb.AddForce(new Vector2(0.2f * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                break;
            case 1:
                boss.ReTrackPlayer();
                boss.animator.SetTrigger("triggerAttackOne");
                boss.rb.AddForce(new Vector2(3f * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.75f, 0), 0, 0.5f, 3);
                boss.nextBossSwingSoundIndex = 0;
                //boss.audioScript.PlayLightAttackSound();
                break;
            case 2:
                boss.animator.SetBool("isCrouch", false);
                boss.animator.SetTrigger("triggerAttackTwo");
                boss.rb.AddForce(new Vector2(2f * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(1f, 0), 0, 0.5f, 4);
                boss.nextBossSwingSoundIndex = 1;
                //boss.audioScript.PlayMediumAttackSound();
                break;
            case 3:
                boss.animator.SetTrigger("triggerAttackThree");
                boss.rb.AddForce(new Vector2(1.5f * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.25f, 0), 0, 0.25f, 4);
                boss.nextBossSwingSoundIndex = 2;
                //boss.audioScript.PlayMediumAttackSound();
                break;
            case 4:
                boss.animator.SetTrigger("triggerAttackFour");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.5f, 0), 14, 0.7f, 6);
                boss.nextBossSwingSoundIndex = 2;
                //boss.audioScript.PlayHeavyAttackSound();
                break;
            case 5:
                // This attack is a low attack.
                boss.animator.SetTrigger("triggerHeavyKickLow");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.5f, 0), 14, 0.7f, 5);
                boss.nextBossSwingSoundIndex = 2;
                break;
            case 6:
                // Light Low Kick.
                boss.ReTrackPlayer();
                boss.animator.SetTrigger("triggerLightKickLow");
                boss.rb.AddForce(new Vector2(3 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(5, new Vector2(0.5f, 0), 1, 0.25f, 3);
                boss.nextBossSwingSoundIndex = 0;
                break;
            case 7:
                // Medium Low Kick.
                boss.animator.SetTrigger("triggerMediumKickLow");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(8, new Vector2(0.5f, 0), 1, 0.15f, 4);
                boss.nextBossSwingSoundIndex = 1;
                break;
            case 8:
                // Medium High Punch. (Unblockable)
                boss.PlayUnblockableWarningSound();
                boss.animator.SetTrigger("triggerMediumPunchUnblockable");
                boss.rb.AddForce(new Vector2(3 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(15, new Vector2(1f, 0), 14, 0.5f, 6);
                boss.nextBossSwingSoundIndex = 2;
                boss.spriteRenderer.color = Color.red;
                break;
            case 9:
                // Heavy High Punch. (Unblockable)
                boss.PlayUnblockableWarningSound();
                boss.animator.SetTrigger("triggerHeavyPunchUnblockable");
                boss.rb.AddForce(new Vector2(6 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(15, new Vector2(3f, 0), 14, 0.5f, 6);
                boss.nextBossSwingSoundIndex = 2;
                boss.spriteRenderer.color = Color.red;
                break;
            case 10:
                // Heavy Kick High. (Player Style)
                boss.animator.SetTrigger("triggerHeavyKickHigh");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(2, new Vector2(3f, 2f), 14, 0.7f, 5);
                boss.nextBossSwingSoundIndex = 2;
                break;
            case 11:
                // Knee Kick Delayed.
                boss.animator.SetTrigger("triggerKneeDelayed");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(2, new Vector2(3f, 2f), 14, 0.7f, 5);
                boss.nextBossSwingSoundIndex = 2;
                break;
            case 20:
                // Boss performs a fireball Attack.
                boss.animator.SetTrigger("triggerSpecialOne");
                boss.audioScript.PlayBossFireballVoice();
                break;
            case 21:
                // Boss Performs a Dragon Punch Attack.
                boss.animator.SetTrigger("triggerSpecialThree");
                boss.AttackHitProperty(10, new Vector2(0.25f, 0), 0, 0.25f, 4);
                boss.nextBossSwingSoundIndex = 2;
                break;
            case 22:
                // Teleport Behind player Close.
                // Decide which direction to teleport.
                if (boss.spriteFlip)
                {
                    // Go Right.
                    teleportDirection = 1;
                }
                else
                {
                    // Go Left.
                    teleportDirection = -1;
                }
                CallTeleport(boss, 0.25f);
                break;
            case 23:
                // Teleport Away from the Player.
                if (boss.spriteFlip)
                {
                    // Go Left.
                    teleportDirection = -1;
                }
                else
                {
                    // Go Right.
                    teleportDirection = 1;
                }
                CallTeleport(boss, 0.5f);
                break;
            case 24:
                // Boss Performs a Dragon Punch Attack. (Anti-Air Version)
                boss.animator.SetTrigger("triggerAntiAir");
                boss.AttackHitProperty(15, new Vector2(1f, 4f), 14, 999, 5);
                boss.nextAttackPatternChoice = -1;
                break;
            default:
                break;
        }
    }

    private void TurnOffAllAttackBoxColliders(BossStateManager boss)
    {
        boss.attackHighBoxCollider2D.enabled = false;
        boss.attackLowBoxCollider2D.enabled = false;
        boss.attackUnblockableBoxCollider2D.enabled = false;
    }

    private void CallTeleport(BossStateManager boss, float newTeleportDuration)
    {
        // Teleport behind the player. 
        boss.animator.SetTrigger("triggerTeleportReady");
        teleportDuration = newTeleportDuration;

        // Make Boss Transparent.
        boss.spriteRenderer.color = new Color(1, 1, 1, 0.90f);

        // Turn off all collisions.
        boss.bossBoxCollider2D.enabled = false;
        TurnOffAllAttackBoxColliders(boss);
    }

}

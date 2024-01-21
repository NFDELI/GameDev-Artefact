using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossRegularAttackState : BossBaseState
{
    int attackPatternChoice;
    int currentAttackPatternIndex;
    int[] attackPatternChosen;
    int[] attackPatternOne = { 1, 2, 3, 4, 0 };  // The Player's Combo.
    int[] attackPatternTwo = { 6, 7, 5, 0 };    // Low kicks attack Combo.   
    int[] attackPatternThree = { 2, 3, 4, 0 }; // Mix-up ending High.
    int[] attackPatternFour = { 2, 3, 5, 0};  // Mix-up ending Low.
    int[] attackPatternFive = { 8, 0 }; // Unblockable-Delayed Punch.
    int[] attackPatternSix = { 20, 0 }; // Fireball.
    int[] attackPatternSeven = { 21, 0 }; // Tatsu.

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Regular Attack State");
        boss.canAttackChain = true;
        currentAttackPatternIndex = 0;
        boss.rangeCheckBox.enabled = false;

        // Reset BlockUntilParry. (Give the player a chance to poke again.)
        boss.blocksUntilParry = boss.blocksUntilParryDefault;

        if (boss.nextAttackPatternChoice == -1)
        {
            // No attack pattern Forced.
            attackPatternChoice = Random.Range(1, 8);
        }
        else
        {
            // An Attack pattern has already been chosen.
            attackPatternChoice = boss.nextAttackPatternChoice;
        }

        // Override Attack Pattern Choice for Debugging.
        attackPatternChoice = 5;

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
                attackPatternChosen = attackPatternSix;
                break;
            case 7:
                attackPatternChosen = attackPatternSeven;
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
                boss.SwitchState(boss.IdleState);
            }
            else
            {
                PerformAttack(boss, attackPatternChosen[currentAttackPatternIndex]);
                currentAttackPatternIndex++;
                boss.canAttackChain = false;
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
                boss.animator.SetTrigger("triggerAttackOne");
                boss.rb.AddForce(new Vector2(2f * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
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
                boss.AttackHitProperty(10, new Vector2(0.5f, 0), 14, 999f, 5);
                boss.nextBossSwingSoundIndex = 2;
                break;
            case 6:
                // Light Low Kick.
                boss.animator.SetTrigger("triggerLightKickLow");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(5, new Vector2(0.5f, 0), 1, 0.25f, 3);
                boss.nextBossSwingSoundIndex = 0;
                break;
            case 7:
                // Medium Low Kick.
                boss.animator.SetTrigger("triggerMediumKickLow");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(8, new Vector2(0.5f, 0), 1, 0.25f, 4);
                boss.nextBossSwingSoundIndex = 1;
                break;
            case 8:
                // Medium High Punch. (Unblockable)
                boss.animator.SetTrigger("triggerMediumPunchUnblockable");
                boss.rb.AddForce(new Vector2(3 * boss.forceDirection * boss.rb.mass, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(15, new Vector2(1f, 0), 14, 0.5f, 6);
                boss.nextBossSwingSoundIndex = 2;
                boss.spriteRenderer.color = Color.red;
                break;
            case 20:
                // Boss performs a fireball Attack.
                boss.animator.SetTrigger("triggerSpecialOne");
                boss.audioScript.PlayBossFireballVoice();
                break;
            case 21:
                // Boss Performs a Dragon Punch Attack.
                boss.animator.SetTrigger("triggerSpecialThree");
                boss.AttackHitProperty(15, new Vector2(1f, 4f), 14, 999, 5);
                break;
            default:
                break;
        }
    }
}

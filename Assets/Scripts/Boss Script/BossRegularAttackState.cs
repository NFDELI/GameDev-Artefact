using Unity.VisualScripting;
using UnityEngine;

public class BossRegularAttackState : BossBaseState
{
    int attackPatternChoice;
    int currentAttackPatternIndex;
    int[] attackPatternChosen;
    int[] attackPatternOne = { 1, 2, 3, 4, 0 };   //2, 3, 4, 0
    int[] attackPatternTwo = { 2, 3, 1, 4, 0 };   //3, 4, 1, 0
    int[] attackPatternThree = { 2, 3, 4, 0 }; //2, 3, 4
    int[] attackPatternFour = { 4, 4, 0 };
    int[] attackPatternFive = { 2, 3, 5, 0}; 


    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Regular Attack State");
        boss.canAttackChain = true;
        currentAttackPatternIndex = 0;
        boss.rangeCheckBox.enabled = false;

        attackPatternChoice = 5;
                              //Random.Range(1, 6);

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
            default:
                break;
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.canAttackChain)
        {
            if (attackPatternChosen[currentAttackPatternIndex] == 0)
            {
                // End of Attack Pattern.
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
        // Make sure boss has armor.
    }

    public void PerformAttack(BossStateManager boss, int attackId)
    {
        // The 'attackId' determines the next attack being performed. 
        switch (attackId)
        {
            case 0:
                boss.animator.SetTrigger("triggerAttackCrouch");
                boss.rb.AddForce(new Vector2(0.2f * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 1:
                boss.animator.SetTrigger("triggerAttackOne");
                boss.rb.AddForce(new Vector2(2f * boss.forceDirection, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.75f, 0), 0, 0.5f, 3);
                boss.nextBossSwingSoundIndex = 0;
                //boss.audioScript.PlayLightAttackSound();
                break;
            case 2:
                boss.animator.SetBool("isCrouch", false);
                boss.animator.SetTrigger("triggerAttackTwo");
                boss.rb.AddForce(new Vector2(2f * boss.forceDirection, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(1f, 0), 0, 0.5f, 4);
                boss.nextBossSwingSoundIndex = 1;
                //boss.audioScript.PlayMediumAttackSound();
                break;
            case 3:
                boss.animator.SetTrigger("triggerAttackThree");
                boss.rb.AddForce(new Vector2(1.5f * boss.forceDirection, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.25f, 0), 0, 0.25f, 4);
                boss.nextBossSwingSoundIndex = 2;
                //boss.audioScript.PlayMediumAttackSound();
                break;
            case 4:
                boss.animator.SetTrigger("triggerAttackFour");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.5f, 0), 2, 0.7f, 6);
                boss.nextBossSwingSoundIndex = 2;
                //boss.audioScript.PlayHeavyAttackSound();
                break;
            case 5:
                // This attack is a low attack.
                boss.animator.SetTrigger("triggerHeavyKickLow");
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection, 0), ForceMode2D.Impulse);
                boss.AttackHitProperty(10, new Vector2(0.5f, 0), 2, 0.7f, 5);
                boss.nextBossSwingSoundIndex = 2;
                break;
            default:
                break;
        }
    }
}

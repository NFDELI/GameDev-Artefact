using UnityEngine;

public class BossIntroductionState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Introduction State");
        if(boss.health <= 0)
        {
            // Boss Loses
            boss.rb.velocity = Vector2.zero;
            boss.isLaunched = false;
            boss.isPhaseTwo = false;
            boss.animator.SetTrigger("triggerLose");
            boss.attackHighBoxCollider2D.enabled = false;
            boss.attackLowBoxCollider2D.enabled = false;
            boss.attackUnblockableBoxCollider2D.enabled = false;
            boss.bossAntiAirBoxCollider2D.enabled = false;
            boss.rangeCheckBox.enabled = false;

            boss.isAiEnabled = false;

            boss.PlayPlayerWinAnimationWithDelay(2f);
        }
        else if(boss.isPhaseTwo)
        {
            // Play fall down animation, then get back up.
            boss.rb.velocity = Vector2.zero;
            boss.isLaunched = false;
            boss.animator.SetTrigger("triggerPhaseTwo");

            // Player Responded.
            boss.playerStateManager.SwitchState(boss.playerStateManager.IntroductionState);
        }
        else
        {
            boss.animator.SetTrigger("triggerIntroduction");
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
    }
}

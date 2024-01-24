using UnityEngine;

public class BossIntroductionState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Introduction State");
        boss.animator.SetTrigger("triggerIntroduction");
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

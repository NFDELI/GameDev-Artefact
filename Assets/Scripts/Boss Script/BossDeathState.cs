using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Death State");
        boss.animator.SetTrigger("triggerFall");
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            boss.health = 100.0f;
            boss.SwitchState(boss.IdleState);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {

    }

}

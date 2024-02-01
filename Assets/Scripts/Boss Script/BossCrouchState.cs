using UnityEngine;

public class BossCrouchState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Crouch State");
        boss.animator.SetBool("isCrouch", true);
        boss.attackCounter = 0;
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

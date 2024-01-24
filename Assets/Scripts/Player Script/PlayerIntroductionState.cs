using UnityEngine;

public class PlayerIntroductionState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager boss)
    {
        Debug.Log("Player Introduction State");
        boss.animator.SetTrigger("triggerIntroduction");
    }

    public override void UpdateState(PlayerStateManager boss)
    {
    }

    public override void OnCollisionEnter(PlayerStateManager boss, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager boss, Collider2D collision)
    {
    }
}

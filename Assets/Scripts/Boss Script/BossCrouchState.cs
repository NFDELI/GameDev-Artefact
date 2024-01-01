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
        if(boss.movementInput.y >= 0)
        {
            boss.animator.SetBool("isCrouch", false);
            boss.SwitchState(boss.IdleState);
        }

        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U))
        {
            boss.SwitchState(boss.RegularAttackState);
            //boss.animator.SetBool("isCrouch", false);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {

    }
}

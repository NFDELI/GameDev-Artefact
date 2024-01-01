using UnityEngine;

public class BossRegularAttackState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Regular Attack State");
        boss.canAttackChain = false;

        // The 'attackCounter' determines the next attack being performed. 
        switch (boss.attackCounter)
        {
            case 0:
                boss.animator.SetTrigger("triggerAttackCrouch");
                boss.attackCounter = 2;
                boss.rb.AddForce(new Vector2(0.5f * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 1:
                boss.animator.SetTrigger("triggerAttackOne");
                boss.attackCounter++;
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 2:
                boss.animator.SetBool("isCrouch", false);
                boss.animator.SetTrigger("triggerAttackTwo");
                boss.attackCounter++;
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 3:
                boss.animator.SetTrigger("triggerAttackThree");
                boss.attackCounter++;
                boss.rb.AddForce(new Vector2(1.5f * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 4:
                boss.animator.SetTrigger("triggerAttackFour");
                boss.attackCounter = 1;
                boss.rb.AddForce(new Vector2(2 * boss.forceDirection, 0), ForceMode2D.Impulse);
                break;
            default:
                break;
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U) && boss.canAttackChain)
        {
            boss.SwitchState(boss.RegularAttackState);
        }

        // Check for Special Attack Input.
        if (Input.GetKeyDown(KeyCode.I) && boss.canAttackChain)
        {
            boss.SwitchState(boss.BossSpecialAttackState);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {

    }
}

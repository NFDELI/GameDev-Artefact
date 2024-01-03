using UnityEngine;

public class BossIdleState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Idle State");
        boss.isLanded = true;
        boss.rb.velocity = new Vector2(0, 0);
        boss.attackCounter = 1;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.movementInput != Vector2.zero)
        {
            // Check for Movement Input.
            if(boss.movementInput.y == 0)
            {
                // Walking.
                boss.SwitchState(boss.WalkingState);
            }
            else if(boss.movementInput.y > 0 && boss.isLanded)
            {
                // Jumping.
                boss.SwitchState(boss.JumpingState);
            }
            else if(boss.movementInput.y < 0)
            {
                // Crouching.
                boss.SwitchState(boss.CrouchState);
            }
        }

        // Check for Attack Input.
        if(Input.GetKeyDown(KeyCode.K))
        {
            boss.SwitchState(boss.RegularAttackState);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            boss.SwitchState(boss.BossSpecialAttackState);
        }

        // Sprite-Flip Check.
        if(boss.spriteFlip)
        {
            boss.spriteRenderer.flipX = true;
            boss.attackOneCollider2D.offset = new Vector2(0.2685299f, 0);
        }
        else
        {
            boss.spriteRenderer.flipX = false;
            boss.attackOneCollider2D.offset = new Vector2(-0.2685299f, 0);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
        
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {

    }
}

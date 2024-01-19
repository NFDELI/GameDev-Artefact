using UnityEngine;

public class BossJumpingState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        boss.isLanded = false;
        if(boss.movementInput.x == 0)
        {
            // Neutral Jump.
            boss.rb.AddForce(new Vector2(boss.rb.velocity.x, boss.jumpForce), ForceMode2D.Impulse);
            boss.animator.SetTrigger("JumpNeutral");
        }
        else
        {
            if(boss.movementInput.x > 0)
            {
                // Diagonal Right/Back Jump. (Back Diagonal Jump have same animation as Neutral Jump)
                boss.rb.AddForce(new Vector2(boss.diagonalJumpForce, boss.jumpForce), ForceMode2D.Impulse);
                if(boss.spriteFlip)
                {
                    boss.animator.SetTrigger("JumpDiagonal");
                }
                else
                {
                    boss.animator.SetTrigger("JumpNeutral");
                }
            }
            else
            {
                // Diagonal Left/Forward Jump. 
                boss.rb.AddForce(new Vector2(-boss.diagonalJumpForce, boss.jumpForce), ForceMode2D.Impulse);
                if(boss.spriteFlip)
                {
                    boss.animator.SetTrigger("JumpNeutral");
                }
                else
                {
                    boss.animator.SetTrigger("JumpDiagonal");
                }
            }
        }
        Debug.Log("Boss Entered Jumping State");
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.isLanded || (boss.rb.velocity.y == 0))
        {
            // Boss has Landed.
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

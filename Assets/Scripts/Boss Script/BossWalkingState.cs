using UnityEngine;

public class BossWalkingState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Walking State");
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.movementInput == Vector2.zero)
        {
            // No Movement Input. (Movement Input Stopped)
            StopMovingAnimation(boss);
            boss.SwitchState(boss.IdleState);
        }
        else 
        {
            // Movement Input Detected.
            if(boss.movementInput.x > 0)
            {
                // Move Right.
                if(boss.spriteFlip)
                {
                    boss.animator.SetBool("isWalkTowards", true);
                    boss.animator.SetBool("isWalkBackwards", false);
                }
                else
                {
                    boss.animator.SetBool("isWalkTowards", false);
                    boss.animator.SetBool("isWalkBackwards", true);
                }
                boss.rb.MovePosition(boss.rb.position + new Vector2(1, 0) * boss.movementSpeed * Time.fixedDeltaTime);

            }
            else if(boss.movementInput.x < 0)
            {
                // Move Left.
                if(boss.spriteFlip) 
                {
                    boss.animator.SetBool("isWalkBackwards", true);
                    boss.animator.SetBool("isWalkTowards", false);
                }
                else
                {
                    boss.animator.SetBool("isWalkBackwards", false);
                    boss.animator.SetBool("isWalkTowards", true);
                }
                boss.rb.MovePosition(boss.rb.position + new Vector2(-1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
            }

            if(boss.movementInput.y > 0)
            {
                // Jump.
                StopMovingAnimation(boss);
                boss.SwitchState(boss.JumpingState);
            }
            else if(boss.movementInput.y < 0)
            {
                // Crouch.
                StopMovingAnimation(boss);
                boss.SwitchState(boss.CrouchState);
            }
        }

        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U))
        {
            StopMovingAnimation(boss);
            boss.SwitchState(boss.RegularAttackState);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StopMovingAnimation(boss);
            boss.SwitchState(boss.BossSpecialAttackState);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {
        //Debug.Log("Collision Detected while Walking");
    }

    public void MoveRight(BossStateManager boss)
    {
        boss.animator.SetBool("isWalkTowards", false);
        boss.animator.SetBool("isWalkBackwards", true);
        boss.rb.MovePosition(boss.rb.position + new Vector2(1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveLeft(BossStateManager boss) 
    {
        boss.animator.SetBool("isWalkBackwards", false);
        boss.animator.SetBool("isWalkTowards", true);
        boss.rb.MovePosition(boss.rb.position + new Vector2(-1, 0) * boss.movementSpeed * Time.fixedDeltaTime);
    }

    private void StopMovingAnimation(BossStateManager boss)
    {
        boss.animator.SetBool("isWalkTowards", false);
        boss.animator.SetBool("isWalkBackwards", false);
    }
}

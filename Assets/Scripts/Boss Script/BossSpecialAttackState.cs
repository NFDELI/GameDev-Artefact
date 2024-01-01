using UnityEngine;

public class BossSpecialAttackState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss Entered Special Attack, State");

        // Check which special move is used.
        if(boss.movementInput != Vector2.zero)
        {
            // Right Special.
            if(boss.movementInput.x > 0)
            {
                if(boss.spriteFlip)
                {
                    // Dragon Punch.
                    boss.animator.SetTrigger("triggerSpecialThree");
                    //DragonPunchForce(boss);
                }
                else
                {
                    // Tatsu. Spinning Kick.
                    boss.animator.SetTrigger("triggerSpecialTwo");
                    SpinningKickForce(boss);
                }
            }
            else
            {
                // Left Special.
                if (boss.spriteFlip)
                {
                    // Tatsu. Spinning Kick.
                    boss.animator.SetTrigger("triggerSpecialTwo");
                    SpinningKickForce(boss);
                }
                else
                {
                    // Dragon Punch.
                    boss.animator.SetTrigger("triggerSpecialThree");
                    //DragonPunchForce(boss);
                }
            }
        }
        else
        {
            // Neutral Direction. (Fireball)
            if (boss.fireballScript.isSpawned)
            {
                // Do not allow the boss to spawn multiple fireballs at the scene.
                boss.SwitchState(boss.IdleState);
            }
            else
            {
                // Fireball is spawned from Animator.
                boss.animator.SetTrigger("triggerSpecialOne");
            }
            
            // Spawn Fireball.
            //boss.SpawnFireball();
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(boss.isSpinnigKickForce)
        {
            // Apply the x-velocity while doing a spinning kick.
            boss.rb.MovePosition(boss.rb.position + new Vector2(boss.spinningKickSpeed * boss.forceDirection, 0) * boss.movementSpeed * Time.fixedDeltaTime);
        }
    }

    public override void OnCollisionEnter(BossStateManager boss, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(BossStateManager boss, Collider2D collision)
    {

    }

    private void DragonPunchForce(BossStateManager boss)
    {
        boss.rb.AddForce(new Vector2(1.5f * boss.forceDirection, 5), ForceMode2D.Impulse);
    }

    private void SpinningKickForce(BossStateManager boss)
    {
        boss.rb.AddForce(new Vector2(1.5f * boss.forceDirection, 0), ForceMode2D.Impulse);
    }
}

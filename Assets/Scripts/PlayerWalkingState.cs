using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Walking State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput == Vector2.zero)
        {
            // No Movement Input. (Movement Input Stopped)
            player.animator.SetBool("isWalkTowards", false);
            player.animator.SetBool("isWalkBackwards", false);
            player.SwitchState(player.IdleState);
        }
        else 
        {
            // Movement Input Detected.
            if(player.movementInput.x > 0)
            {
                // Move Right.
                if(player.spriteFlip)
                {
                    player.animator.SetBool("isWalkTowards", true);
                    player.animator.SetBool("isWalkBackwards", false);
                }
                else
                {
                    player.animator.SetBool("isWalkTowards", false);
                    player.animator.SetBool("isWalkBackwards", true);
                }
                player.rb.MovePosition(player.rb.position + new Vector2(1, 0) * player.movementSpeed * Time.fixedDeltaTime);

            }
            else if(player.movementInput.x < 0)
            {
                // Move Left.
                if(player.spriteFlip) 
                {
                    player.animator.SetBool("isWalkBackwards", true);
                    player.animator.SetBool("isWalkTowards", false);
                }
                else
                {
                    player.animator.SetBool("isWalkBackwards", false);
                    player.animator.SetBool("isWalkTowards", true);
                }
                player.rb.MovePosition(player.rb.position + new Vector2(-1, 0) * player.movementSpeed * Time.fixedDeltaTime);
            }

            if(player.movementInput.y > 0)
            {
                // Jump.
                player.animator.SetBool("isWalkTowards", false);
                player.animator.SetBool("isWalkBackwards", false);
                player.SwitchState(player.JumpingState);
            }
            else if(player.movementInput.y < 0)
            {
                // Crouch.
                player.animator.SetBool("isWalkTowards", false);
                player.animator.SetBool("isWalkBackwards", false);
                player.SwitchState(player.CrouchState);
            }
        }

        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.SwitchState(player.RegularAttackState);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            player.SwitchState(player.PlayerSpecialAttackState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public void MoveRight(PlayerStateManager player)
    {
        player.animator.SetBool("isWalkTowards", false);
        player.animator.SetBool("isWalkBackwards", true);
        player.rb.MovePosition(player.rb.position + new Vector2(1, 0) * player.movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveLeft(PlayerStateManager player) 
    {
        player.animator.SetBool("isWalkBackwards", false);
        player.animator.SetBool("isWalkTowards", true);
        player.rb.MovePosition(player.rb.position + new Vector2(-1, 0) * player.movementSpeed * Time.fixedDeltaTime);
    }
}

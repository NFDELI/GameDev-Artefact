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
            player.animator.SetBool("isWalkTowards", false);
            player.animator.SetBool("isWalkBackwards", false);
            player.SwitchState(player.IdleState);
        }
        else 
        {
            if(player.movementInput.x > 0)
            {
                //Move Right.
                player.animator.SetBool("isWalkTowards", false);
                player.animator.SetBool("isWalkBackwards", true);
                player.rb.MovePosition(player.rb.position + new Vector2(1, 0) * player.movementSpeed * Time.fixedDeltaTime);
                
            }
            else
            {
                //Move Left.
                player.animator.SetBool("isWalkBackwards", false);
                player.animator.SetBool("isWalkTowards", true);
                player.rb.MovePosition(player.rb.position + new Vector2(-1, 0) * player.movementSpeed * Time.fixedDeltaTime);
            }

            if(player.movementInput.y != 0)
            {
                player.animator.SetBool("isWalkTowards", false);
                player.animator.SetBool("isWalkBackwards", false);
                player.SwitchState(player.JumpingState);
            }
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

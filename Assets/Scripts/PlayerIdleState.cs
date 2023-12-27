using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Idle State");
        player.isLanded = true;
        player.rb.velocity = new Vector2(0, 0);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput != Vector2.zero)
        {
            if(player.movementInput.y == 0)
            {
                // Walking.
                player.SwitchState(player.WalkingState);
            }
            else if(player.movementInput.y > 0 && player.isLanded)
            {
                // Jumping.
                player.SwitchState(player.JumpingState);
            }
            else if(player.movementInput.y < 0)
            {
                // Crouching.
                player.SwitchState(player.CrouchState);
            }
            
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
        
    }
}

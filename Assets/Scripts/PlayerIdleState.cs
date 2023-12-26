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
                player.SwitchState(player.WalkingState);
            }
            else if(player.movementInput.y > 0 && player.isLanded)
            {
                player.SwitchState(player.JumpingState);
            }
            
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
        
    }
}

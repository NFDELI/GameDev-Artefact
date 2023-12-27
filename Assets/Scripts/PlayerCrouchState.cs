using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Crouch State");
        player.animator.SetBool("isCrouch", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.movementInput.y >= 0)
        {
            player.animator.SetBool("isCrouch", false);
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

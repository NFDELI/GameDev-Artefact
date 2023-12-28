using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Death State");
        player.animator.SetTrigger("triggerDeath");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            player.health = 100.0f;
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

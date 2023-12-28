using UnityEngine;

public class PlayerRegularAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Regular Attack State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

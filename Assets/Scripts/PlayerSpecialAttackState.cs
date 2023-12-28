using UnityEngine;

public class PlayerSpecialAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Special Attack, State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

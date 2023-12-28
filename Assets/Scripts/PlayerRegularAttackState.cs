using UnityEngine;

public class PlayerRegularAttackState : PlayerBaseState
{
    int forceDirection = 1;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Regular Attack State");
        player.canAttackChain = false;

        // Ensures that the attack force is applied in the correct Direction.
        if(!player.spriteRenderer.flipX)
        {
            forceDirection = -1;
        }
        else
        {
            forceDirection = 1;
        }

        // The 'attackCounter' determines the next attack being performed. 
        switch (player.attackCounter)
        {
            case 1:
                player.animator.SetTrigger("triggerAttackOne");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(2 * forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 2:
                player.animator.SetTrigger("triggerAttackTwo");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(2 * forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 3:
                player.animator.SetTrigger("triggerAttackThree");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(1.5f * forceDirection, 0), ForceMode2D.Impulse);
                break;
            case 4:
                player.animator.SetTrigger("triggerAttackFour");
                player.attackCounter = 1;
                player.rb.AddForce(new Vector2(2 * forceDirection, 0), ForceMode2D.Impulse);
                break;
            default:
                break;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Check for Attack Input.
        if (Input.GetKeyDown(KeyCode.U) && player.canAttackChain)
        {
            player.SwitchState(player.RegularAttackState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}

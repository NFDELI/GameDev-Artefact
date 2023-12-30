using UnityEngine;

public class PlayerHitReactionState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Hit Reaction State");

        switch (player.nextPlayerHitReaction)
        {
            case 0:
                // Get Hit High.
                player.animator.SetTrigger("triggerHitReactionHigh");
                break;
            case 1:
                // Get Hit Low.
                player.animator.SetTrigger("triggerHitReactionLow");
                break;
            case 2:
                // Get Hit then Fall.
                player.animator.SetTrigger("triggerFall");
                break;
            case 3:
                // Grabbed.
                player.animator.SetTrigger("triggerGetUp");
                break;
            case 4:
                // High Block.
                player.animator.SetTrigger("triggerBlockHigh");
                break;
            case 5:
                // Low Block.
                player.animator.SetTrigger("triggerBlockLow");
                break;
            case 6:
                // High Parry.
                player.animator.SetTrigger("triggerParryHigh");
                break;
            case 7:
                // Low Parry.
                player.animator.SetTrigger("triggerParryLow");
                break;
            case 8:
                // Guard/Posture/Grab Break.
                player.animator.SetTrigger("triggerPostureBreak");
                break;
            case 9:
                // Dazed/Stunned.
                player.animator.SetTrigger("triggerStunned");
                break;
            default:
                break;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
        //Debug.Log("Collision Detected");
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {

    }
}

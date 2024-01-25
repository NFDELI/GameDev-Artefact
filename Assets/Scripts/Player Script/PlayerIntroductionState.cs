using UnityEngine;

public class PlayerIntroductionState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player Introduction State");

        player.StopMovingAnimation();

        if (player.health <= 0)
        {
            player.rb.velocity = Vector2.zero;
            player.isLaunched = false;
            player.animator.SetTrigger("triggerLose");
            player.TurnOffPlayerCollisionBoxes();
            player.attackBoxCollider.enabled = false;

            // Stop the boss from attacking.
            player.bossStateManager.isAiEnabled = false;
            player.PlayBossWinAnimationWithDelay(2f);
        }
        else if(player.bossStateManager.isPhaseTwo && player.bossStateManager.health > 0)
        {
            player.StartPlayerPhaseTwoAnimationWithDelay(1f);
        }
        else if(!player.bossStateManager.initiatePhaseTwo && !player.bossStateManager.isPhaseTwo)
        {
            player.animator.SetTrigger("triggerIntroduction");
        }

    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {
    }
}

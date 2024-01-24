using UnityEngine;

public class PlayerIntroductionState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player Introduction State");
        if (player.health <= 0)
        {
            player.animator.SetTrigger("triggerLose");
            player.TurnOffPlayerCollisionBoxes();
            player.attackBoxCollider.enabled = false;

            // Stop the boss from attacking.
            player.bossStateManager.isAiEnabled = false;
            player.PlayBossWinAnimationWithDelay(2f);
        }
        else if (player.bossStateManager.health <= 0)
        {
            //player.animator.SetTrigger("triggerIntroduction");
        }
        else
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

using UnityEngine;

public class PlayerRegularAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Regular Attack State");
        player.canAttackChain = false;

        // The 'attackCounter' determines the next attack being performed. 
        switch (player.attackCounter)
        {
            case 0:
                player.animator.SetTrigger("triggerAttackCrouch");
                player.attackCounter = 2;
                player.rb.AddForce(new Vector2(0.5f * player.forceDirection, 0), ForceMode2D.Impulse);
                player.AttackHitProperty(2, new Vector2(0.2f, 0), 0, 0.2f, 3);
                player.audioScript.PlayLightAttackSound();
                break;
            case 1:
                player.animator.SetTrigger("triggerAttackOne");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(2 * player.forceDirection, 0), ForceMode2D.Impulse);
                player.AttackHitProperty(2, new Vector2(0.2f, 0), 0, 0.2f, 3);
                player.audioScript.PlayLightAttackSound();
                break;
            case 2:
                player.animator.SetBool("isCrouch", false);
                player.animator.SetTrigger("triggerAttackTwo");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(2 * player.forceDirection, 0), ForceMode2D.Impulse);
                player.AttackHitProperty(2, new Vector2(0.2f, 0), 0, 0.2f, 4);
                player.audioScript.PlayMediumAttackSound();
                break;
            case 3:
                player.animator.SetTrigger("triggerAttackThree");
                player.attackCounter++;
                player.rb.AddForce(new Vector2(1.5f * player.forceDirection, 0), ForceMode2D.Impulse);
                player.AttackHitProperty(2, new Vector2(0.2f, 0), 0, 0.2f, 4);
                player.audioScript.PlayMediumAttackSound();
                break;
            case 4:
                player.animator.SetTrigger("triggerAttackFour");
                player.attackCounter = 1;
                player.rb.AddForce(new Vector2(2 * player.forceDirection, 0), ForceMode2D.Impulse);
                player.audioScript.PlayHeavyAttackSound();
                player.AttackHitProperty(2, new Vector2(0.2f, 0), 0, 0.2f, 5);
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

        // Check for Special Attack Input.
        if (Input.GetKeyDown(KeyCode.I) && player.canAttackChain)
        {
            player.SwitchState(player.SpecialAttackState);
        }

        // Check for Parry Input.
        if (Input.GetKeyDown(KeyCode.Space) && player.canAttackChain)
        {
            player.SwitchState(player.ParryAttemptState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision)
    {

    }
}

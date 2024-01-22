using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboedScript : MonoBehaviour
{
    public PlayerStateManager player;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            if (player.isLaunched)
            {
                // Boss falls down then hitbox is disabled so that player cannot constantly combo.
                player.animator.SetTrigger("triggerLaunchedLanded");
                // Make Normal collider true!!
                player.playerBoxCollider2D.enabled = false;
                player.playerAirCollider2D.enabled = false;
                player.isLaunched = false;
                player.rb.gravityScale = 2f;

                // Make sure to turn on Hitbox when the player gets up.
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (player.isLaunched)
            {
                // Boss falls down then hitbox is disabled so that player cannot constantly combo.
                player.animator.SetTrigger("triggerLaunchedLanded");
                // Make Normal collider true!!
                player.playerBoxCollider2D.enabled = false;
                player.playerAirCollider2D.enabled = false;
                player.isLaunched = false;
                player.rb.gravityScale = 2f;

                // Make sure to turn on Hitbox when the player gets up.
            }
        }
    }
}

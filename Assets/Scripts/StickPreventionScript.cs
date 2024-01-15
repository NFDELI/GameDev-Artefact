using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPreventionScript : MonoBehaviour
{
    public PlayerStateManager playerStateManager;
    private bool timerStarted;
    public float timeToReset;
    private void Start()
    {
        timerStarted = false;
    }

    private void Update()
    {
        if(timerStarted)
        {
            timeToReset -= Time.deltaTime;
            if (timeToReset <= 0) { ResetPlayerCollider(); };
        }
    }

    private void ResetPlayerCollider()
    {
        playerStateManager.playerBoxCollider2D.enabled = true;
        timerStarted = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Boss")
        {
            // Prevent the player from landing on the boss.
            if (playerStateManager.jumpDirection && playerStateManager.isLanding)
            {
                if (!playerStateManager.isCloseToWallLeft && playerStateManager.isCloseToWallRight)
                {
                    // Player is close to wall right. So move left.
                    playerStateManager.rb.MovePosition(playerStateManager.rb.position + new Vector2(-0.125f, -0.125f));
                }
                else
                {
                    playerStateManager.rb.MovePosition(playerStateManager.rb.position + new Vector2(0.125f, -0.125f));
                }
            }
            else if (!playerStateManager.jumpDirection && playerStateManager.isLanding)
            {
                if(!playerStateManager.isCloseToWallRight && playerStateManager.isCloseToWallLeft)
                {   
                    // Player is close to wall left. So move right
                    playerStateManager.rb.MovePosition(playerStateManager.rb.position + new Vector2(0.125f, -0.125f));
                }
                else
                {
                    playerStateManager.rb.MovePosition(playerStateManager.rb.position + new Vector2(-0.125f, -0.125f));
                }
            }

            //timerStarted = true;
            //playerStateManager.playerBoxCollider2D.enabled = false;
        }
    }
}

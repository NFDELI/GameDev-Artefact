using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseToWallScripts : MonoBehaviour
{
    public bool isWallLeft;
    public PlayerStateManager playerStateManager;
    public float timeToResetDefault = 1f;
    public float timeToResetBool;

    // Start is called before the first frame update
    void Start()
    {
        timeToResetBool = timeToResetDefault;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player is close the wall, change the player's boolean.
        if(collision.tag == "Player")
        {
            if(isWallLeft)
            {
                // Player is close to Left Wall.
                playerStateManager.isCloseToWallLeft = true;
            }
            else
            {
                // Player is close to Right Wall.
                playerStateManager.isCloseToWallRight = true;
            }
            timeToResetBool = timeToResetDefault;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Invoke("ResetCloseToWalls", 1f);
        }
    }

    public void ResetCloseToWalls()
    {
        playerStateManager.isCloseToWallLeft = false;
        playerStateManager.isCloseToWallRight = false;
    }
}

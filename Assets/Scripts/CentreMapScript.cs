using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreMapScript : MonoBehaviour
{
    public PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.isCloseToWallLeft = false;
            player.isCloseToWallRight = false;
        }
    }
}

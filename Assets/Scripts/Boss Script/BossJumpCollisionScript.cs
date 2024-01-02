using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpCollisionScript : MonoBehaviour
{
    BoxCollider2D boxCollider;
    public BossStateManager stateManager;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Floor") 
        { 
            stateManager.isLanded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            stateManager.isLanded = false;
        }
    }
}

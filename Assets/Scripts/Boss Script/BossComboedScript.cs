using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossComboedScript : MonoBehaviour
{
    public BossStateManager boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            if (boss.isLaunched)
            {
                // Boss falls down then hitbox is disabled so that player cannot constantly combo.
                boss.animator.SetTrigger("triggerLaunchedLanded");
                // Make Normal collider true!!
                boss.bossBoxCollider2D.enabled = false;
                boss.bossAirBoxCollider2D.enabled = false;
                boss.isLaunched = false;
                boss.rb.gravityScale = 2f;

                // Make sure to turn on Hitbox when the boss gets up.
            }
        }
    }
}

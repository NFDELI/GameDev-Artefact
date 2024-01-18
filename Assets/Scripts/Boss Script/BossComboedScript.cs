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
                //boss.animator.ResetTrigger("triggerLaunchedLanded");
                boss.animator.SetTrigger("triggerLaunchedLanded");
                // Make Normal collider true!!
                boss.bossBoxCollider2D.enabled = false;
                boss.bossAirBoxCollider2D.enabled = false;
                boss.isLaunched = false;
                Debug.Log("CALLED");
            }
        }
    }
}

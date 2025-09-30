using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShortRangeCheck : MonoBehaviour
{
    private BossStateManager bossManager;
    private PlayerStateManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        bossManager = GetComponentInParent<BossStateManager>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerStateManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bossManager.isNearPlayer = true;

            if (!playerManager.isInvincible)
            {
                // The player is in range of boss's attack.
                bossManager.hasReachedPlayer = true;
                bossManager.animator.SetBool("isWalkTowards", false);
                bossManager.animator.SetBool("isWalkBackwards", false);
                bossManager.SwitchState(bossManager.RegularAttackState);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bossManager.isNearPlayer = false;
        }
    }
}

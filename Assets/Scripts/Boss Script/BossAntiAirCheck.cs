using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAntiAirCheck : MonoBehaviour
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
        if (collision.tag == "PlayerJumpCollider")
        {
            // Boss does Dragon Punch.
            bossManager.nextAttackPatternChoice = 120;
            bossManager.animator.SetBool("isWalkTowards", false);
            bossManager.animator.SetBool("isWalkBackwards", false);

            // Prevent the player from constantly being comboed.
            bossManager.bossAntiAirBoxCollider2D.enabled = false;
            bossManager.SwitchState(bossManager.RegularAttackState);

            // Re-enable the Anti-Air Box after the player landed.
            bossManager.canAntiAirAgain = false;
            Debug.Log("Anti Air Can!");
        }
    }

}

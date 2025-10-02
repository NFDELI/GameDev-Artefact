using Unity.VisualScripting;
using UnityEngine;

public abstract class BossBaseState
{
    protected GameManagerScript gameManagerScript;
    protected PlayerStateManager playerManager;
    protected BossStateManager bossManager;
    protected virtual void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        bossManager = GameObject.Find("Boss").GetComponent<BossStateManager>();

        if(playerManager == null)
        {
            Debug.LogWarning("Player Manager Can't be found");
        }

        if (bossManager == null)
        {
            Debug.LogWarning("Boss Manager Can't be found");
        }

        if (gameManagerScript == null)
        {
            Debug.LogWarning("Game Manager Can't be found");
        }
    }
    public abstract void EnterState(BossStateManager boss);
    public abstract void UpdateState(BossStateManager boss);
    public abstract void OnCollisionEnter(BossStateManager boss, Collision collision);
    public abstract void OnTriggerEnter2D(BossStateManager boss, Collider2D collision);

}

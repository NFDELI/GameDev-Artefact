using UnityEngine;

public abstract class BossBaseState
{
    public abstract void EnterState(BossStateManager boss);
    public abstract void UpdateState(BossStateManager boss);
    public abstract void OnCollisionEnter(BossStateManager boss, Collision collision);
    public abstract void OnTriggerEnter2D(BossStateManager boss, Collider2D collision);

}

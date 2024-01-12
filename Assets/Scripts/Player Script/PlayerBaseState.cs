using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionEnter(PlayerStateManager player, Collision collision);
    public abstract void OnTriggerEnter2D(PlayerStateManager player, Collider2D collision);
    public virtual void OnParryPerformed(PlayerStateManager player) { }
}

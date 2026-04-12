using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerAnimator))]
public class LocomotionBase : MonoBehaviour
{
    protected PlayerMovement Owner { get; private set; }
    protected PlayerControls Inputs { get; private set; }
    protected PlayerAnimator Animator { get; private set; }
    protected MovementState MoveState => Owner.MoveState;
    protected Rigidbody2D Rb => Owner.Rb;

    public virtual void Awake()
    {
        Owner = GetComponent<PlayerMovement>();
        Inputs = GetComponent<PlayerControls>();
        Animator = GetComponent<PlayerAnimator>();
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnLateUpdate() { }
}
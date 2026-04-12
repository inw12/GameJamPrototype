using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerControls))]
public class LocomotionBase : MonoBehaviour
{
    protected PlayerMovement Owner { get; private set; }
    protected PlayerControls Inputs { get; private set; }
    protected Rigidbody2D Rb => Owner.Rb;

    public virtual void Awake()
    {
        Owner = GetComponent<PlayerMovement>();
        Inputs = GetComponent<PlayerControls>();
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
}
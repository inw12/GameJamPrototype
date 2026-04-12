using UnityEngine;

public class OneLegLocomotion : LocomotionBase
{
    [Header("Movement")]
    [SerializeField] private float Speed;

    public override void OnUpdate()
    {
        //Owner.HandleJump();
    }

    private void OneLegHop()
    {

    }

    public override void OnFixedUpdate()
    {
        Owner.State = MovementState.Walk;

        Rb.linearVelocity = new Vector2(
            Inputs.MovePressed.x * Speed,
            Rb.linearVelocity.y
        );
    }
}

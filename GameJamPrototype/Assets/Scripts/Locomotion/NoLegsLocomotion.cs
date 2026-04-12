using UnityEngine;

public class NoLegsLocomotion : LocomotionBase
{
    [Header("Movement")]
    [SerializeField] private float Speed;
    [SerializeField] private float CrawlMultiplier = 0.3f;

    public override void OnEnter()
    {
        // TODO: trigger crawl animation layer / pose
    }

    public override void OnExit()
    {
        // TODO: restore normal animation layer
    }

    public override void OnUpdate()
    {
        // No jump – intentionally empty
    }

    public override void OnFixedUpdate()
    {
        Owner.State = MovementState.Walk;

        Rb.linearVelocity = new Vector2(
            Inputs.MovePressed.x * Speed * CrawlMultiplier,
            Rb.linearVelocity.y
        );
    }
}

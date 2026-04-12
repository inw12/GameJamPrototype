using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class BothLegsLocomotion : LocomotionBase
{
    [Header("Movement")]
    [SerializeField] private float Speed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private float JumpForce;

    [Header("Gravity/Ground/Jump")]
    [SerializeField] private PlatformEffector2D platforms;
    [SerializeField] private PlatformEffector2D stairs;

    private PlayerGrapple grapple;
    private bool isJumping;
    private bool dropDownTriggered;


    public override void OnUpdate()
    {
        //JumpLogic();
    }

    public override void OnFixedUpdate()
    {
        if (!Owner.CanMove) return;

        var targetSpeed = Inputs.SprintPressed ? SprintSpeed : Speed;

        Rb.linearVelocity = new Vector2(
            Inputs.MovePressed.x * targetSpeed,
            Rb.linearVelocity.y
        );
    }

    // private void JumpLogic()
    // {
    //     if (Owner.IsGrounded && Rb.linearVelocity.y <= 0)
    //         isJumping = false;

    //     //Drop through platform
    //     if (Inputs.MovePressed.y < 0f && !dropDownTriggered && Owner.LastGroundHit.collider.TryGetComponent(out PlatformEffector2D _))
    //     {
    //         isJumping = true;
    //         StartCoroutine(PlatformDropdown());
    //     }
    //     else if (Inputs.JumpPressed && !isJumping && Owner.IsGrounded)
    //     {
    //         isJumping = true;
    //         Rb.AddForce(JumpForce * Vector3.up, ForceMode2D.Impulse);
    //     }
    // }

    // private IEnumerator PlatformDropdown()
    // {
    //     dropDownTriggered = true;
    //     var playerLayer = 1 << gameObject.layer;
    //     platforms.colliderMask &= ~playerLayer;
    //     //stairs.colliderMask &= ~playerLayer;

    //     yield return new WaitForSeconds(0.35f);

    //     platforms.colliderMask |= playerLayer;
    //     //stairs.colliderMask |= playerLayer;
    //     dropDownTriggered = false;
    // }
}

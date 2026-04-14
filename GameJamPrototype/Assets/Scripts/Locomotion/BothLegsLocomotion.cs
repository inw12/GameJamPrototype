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
    private bool isJumping;
    private bool dropDownTriggered;


    public override void OnUpdate()
    {
        JumpLogic();
    }

    public override void OnFixedUpdate()
    {
        var targetSpeed = Inputs.SprintPressed ? SprintSpeed : Speed;

        Rb.linearVelocity = new Vector2(
            Inputs.MovePressed.x * targetSpeed,
            Rb.linearVelocity.y
        );
    }

    private void JumpLogic()
    {
        if (Owner.IsGrounded && Rb.linearVelocity.y <= 0 && !dropDownTriggered)
        {
            isJumping = false;
            Owner.dropdownTriggered = false;
        }
        //isJumping = (!Owner.IsGrounded || Rb.linearVelocity.y > 0) && isJumping;

        // Jump Input
        if (Inputs.JumpPressed && !isJumping && Owner.IsGrounded)
        {
            // drop through platform
            if (Inputs.MovePressed.y < 0f && !dropDownTriggered && Owner.LastGroundHit != null && Owner.LastGroundHit.TryGetComponent(out PlatformEffector2D p))
            {
                Owner.dropdownTriggered = true;
                isJumping = true;
                StartCoroutine(PlatformDropdown(p));
                return;
            }
            //normal jump
            else
            {
                isJumping = true;
                Rb.AddForce(JumpForce * Vector3.up, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator PlatformDropdown(PlatformEffector2D p)
    {
        dropDownTriggered = true;
        var playerLayer = 1 << gameObject.layer;
        p.colliderMask &= ~playerLayer;

        yield return new WaitForSeconds(0.5f);

        p.colliderMask |= playerLayer;
        dropDownTriggered = false;
    }
}

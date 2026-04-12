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
    [SerializeField] private float GroundRayLength;

    [Header("Gravity/Ground/Jump")]
    [SerializeField] private PlatformEffector2D platforms;
    [SerializeField] private PlatformEffector2D stairs;
    private bool IsGrounded;
    private LayerMask GroundLayer => LayerMask.GetMask("Ground");
    private LayerMask PlatformLayer => LayerMask.GetMask("Platform");
    private bool isJumping;
    private bool dropDownTriggered;
    private RaycastHit2D lastGroundHit;

    [Header("Debug Log")]
    public bool DebugLog;

    public override void OnUpdate()
    {
        GroundLogic();
        JumpLogic();
    }

    private void GroundLogic()
    {
        // Ground check
        var groundLayer = dropDownTriggered
            ? GroundLayer
            : (LayerMask)(GroundLayer | PlatformLayer);

        lastGroundHit = Physics2D.Raycast(Owner.transform.position, Vector2.down, GroundRayLength, groundLayer);
        IsGrounded = lastGroundHit;

        if (IsGrounded && Rb.linearVelocity.y <= 0)
            isJumping = false;
    }

    private void JumpLogic()
    {
        if (!Inputs.JumpPressed || !IsGrounded || isJumping) return;

        // Drop through platform
        if (Inputs.MovePressed.y < 0f && !dropDownTriggered
            && lastGroundHit.collider.TryGetComponent(out PlatformEffector2D _))
        {
            isJumping = true;
            StartCoroutine(PlatformDropdown());
        }
        else
        {
            isJumping = true;
            Rb.AddForce(JumpForce * Vector3.up, ForceMode2D.Impulse);
        }
    }

    private IEnumerator PlatformDropdown()
    {
        dropDownTriggered = true;
        var playerLayer = 1 << Owner.gameObject.layer;
        platforms.colliderMask &= ~playerLayer;
        stairs.colliderMask &= ~playerLayer;

        yield return new WaitForSeconds(0.35f);

        platforms.colliderMask |= playerLayer;
        stairs.colliderMask |= playerLayer;
        dropDownTriggered = false;
    }

    public override void OnFixedUpdate()
    {
        var targetSpeed = Inputs.SprintPressed ? SprintSpeed : Speed;
        Owner.State = Inputs.SprintPressed ? MovementState.Sprint : MovementState.Walk;

        Rb.linearVelocity = new Vector2(
            Inputs.MovePressed.x * targetSpeed,
            Rb.linearVelocity.y
        );
    }

    void OnDrawGizmos()
    {
        if (!DebugLog) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * GroundRayLength);
    }

    void OnGUI()
    {
        if (!DebugLog) return;
        GUILayout.Label($"Move: {Inputs?.MovePressed}");
    }
}

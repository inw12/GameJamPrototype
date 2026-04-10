using UnityEngine;
using System.Collections;
using System;
public enum MovementState
{
    Idle    = 0,
    Walk    = 1,
    Sprint  = 2
}
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float Speed; 
    [SerializeField] private float SprintSpeed; 
    [SerializeField] private float Acceleration; 
    private Rigidbody2D rb;
    private PlayerControls inputs;

    [Header("Gravity/Ground/Jump")]
    [SerializeField] private float GroundRayLength;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GravityForce;
    [SerializeField] private PlatformEffector2D platformEffector;

    private LayerMask GroundLayer => LayerMask.GetMask("Ground");
    private LayerMask PlatformLayer => LayerMask.GetMask("Platform");

    public bool IsGrounded = false;
    private bool isJumping;
    private bool dropDownTriggered; // down + jump input for falling through platforms

    [Header("DebugLog")]
    public bool DebugLog;
    [Header("Animation Control")]
    [SerializeField] private PlayerAnimationController animationController;

    // state machine
    private MovementState state;

    // character flipping
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = GravityForce;
        inputs = PlayerControls.Instance;

        state = MovementState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    void FixedUpdate()
    {
        // Apply Movement
        var targetSpeed = inputs.SprintPressed ? SprintSpeed : Speed;
        state = inputs.SprintPressed ? MovementState.Sprint : MovementState.Walk;
        rb.linearVelocity = new Vector2
        (
            inputs.MovePressed.x * targetSpeed,
            rb.linearVelocity.y
        );
    }

    void LateUpdate()
    {
        var animParameters = new PlayerAnimatorParameters
        {
            MovementAction = (int)state,
        };
        animationController.UpdateAnimator(animParameters);
    }

    void UpdateMove()
    {   
        // Ground 
        var groundLayer = dropDownTriggered ? GroundLayer : (LayerMask)(GroundLayer | PlatformLayer);
        var groundHit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, groundLayer);
        IsGrounded = groundHit;

        if (IsGrounded && rb.linearVelocity.y <= 0)
            isJumping = false;

        // Update State Machine
        state = inputs.MovePressed.sqrMagnitude == 0f ? MovementState.Idle : state;

        // Character Left/Right Flipping
        var x = inputs.GetMouseWorldPosition().x > transform.position.x ? 1f : -1f;
        if (x > 0f && !facingRight) FlipCharacter();
        if (x < 0f && facingRight) FlipCharacter();

        // Jump Action
        if (inputs.JumpPressed && IsGrounded && !isJumping)
        {
            // drop through platform
            if (inputs.MovePressed.y < 0f && !dropDownTriggered && groundHit.collider.TryGetComponent(out PlatformEffector2D p))
            {
                isJumping = true;
                StartCoroutine(PlatformDropdown());
            }
            // jump up
            else
            {
                isJumping = true;
                rb.AddForce(JumpForce * Vector3.up, ForceMode2D.Impulse);
            }
        }
    }
    
    private IEnumerator PlatformDropdown()
    {
        dropDownTriggered = true;
        var playerLayer = 1 << gameObject.layer;
        platformEffector.colliderMask &= ~playerLayer;

        yield return new WaitForSeconds(0.5f);

        platformEffector.colliderMask |= playerLayer;
        dropDownTriggered = false;
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
        GUILayout.Label($"Move: {inputs.MovePressed}");
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        var scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public bool IsFacingRight() => facingRight;
}

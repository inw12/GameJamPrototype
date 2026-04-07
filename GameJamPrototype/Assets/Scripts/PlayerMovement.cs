using UnityEngine;

public enum MovementState
{
    Idle    = 0,
    Walk    = 1,
    Sprint  = 2
}
[RequireComponent(typeof(PlayerControls))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float Speed; 
    [SerializeField] private float SprintSpeed; 
    [SerializeField] private float Acceleration; 
    private Rigidbody2D rb;
    private PlayerControls inputs;

    [Header("Gravity")]
    [SerializeField] private float GroundRayLength;
    public bool IsGrounded = false;
    private LayerMask groundLayer => LayerMask.GetMask("Ground");

    [Header("Debug")]
    public bool Debug;
    [Header("Animation Control")]
    [SerializeField] private PlayerAnimationController animationController;

    // state machine
    private MovementState state;

    // character flipping
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputs = PlayerControls.Instance;

        state = MovementState.Idle;

        animationController.Initialize();
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
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, groundLayer);

        // Update State Machine
        state = inputs.MovePressed.sqrMagnitude == 0f ? MovementState.Idle : state;

        // Character Left/Right Flipping
        var x = inputs.GetMouseWorldPosition().x > transform.position.x ? 1f : -1f;
        if (x > 0f && !facingRight) FlipCharacter();
        if (x < 0f && facingRight) FlipCharacter();
    }

    void OnDrawGizmos()
    {
        if (!Debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * GroundRayLength);
    }

    void OnGUI()
    {
        if (!Debug) return;
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

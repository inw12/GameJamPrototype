using UnityEngine;
public enum MovementState
{
    Idle    = 0,
    Walk    = 1,
    Sprint  = 2
}
public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerInputs inputs;
    public Vector2 MovePressed => inputs.General.Move.ReadValue<Vector2>();
    public bool JumpPressed => inputs.General.Jump.IsPressed();
    public bool SprintPressed => inputs.General.Sprint.IsPressed();

    [Header("Move")]
    [SerializeField] private float Speed; 
    [SerializeField] private float SprintSpeed; 
    [SerializeField] private float Acceleration; 
    private Rigidbody2D rb;

    [Header("Gravity")]
    [SerializeField] private float GroundRayLength;
    public bool IsGrounded = false;
    private LayerMask groundLayer => LayerMask.GetMask("Ground");

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Animation Control")]
    [SerializeField] private PlayerAnimationController animationController;
    public bool weaponEquipped;    // weapon equipped check

    // state machine
    private MovementState state;

    void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        var targetSpeed = SprintPressed ? SprintSpeed : Speed;
        state = SprintPressed ? MovementState.Sprint : MovementState.Walk;
        rb.linearVelocity = new Vector2
        (
            MovePressed.x * targetSpeed, 
            rb.linearVelocity.y
        );
    }

    void LateUpdate()
    {
        var animParameters = new PlayerAnimatorParameters
        {
            MovementAction = (int)state,
            WeaponEquipped = weaponEquipped
        };
        animationController.UpdateAnimator(animParameters);
    }

    void UpdateMove()
    {   
        // Ground 
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, groundLayer);

        // Update State Machine
        state = MovePressed.sqrMagnitude == 0f ? MovementState.Idle : state;

        // Sprite Flipping
        if (MovePressed.x > 0f)
            sprite.flipX = true;
        if (MovePressed.x < 0f)
            sprite.flipX = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * GroundRayLength);
    }

    void OnGUI()
    {
        GUILayout.Label($"Move: {MovePressed}");
    }
}

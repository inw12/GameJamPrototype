using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerInputs inputs;
    public Vector2 MovePressed => inputs.General.Move.ReadValue<Vector2>();
    public bool JumpPressed => inputs.General.Jump.IsPressed();

    [Header("Move")]
    [SerializeField] private float Speed; 
    [SerializeField] private float Acceleration; 
    private Rigidbody2D rb;

    [Header("Gravity")]
    [SerializeField] private float GroundRayLength;
    public bool IsGrounded = false;
    private LayerMask groundLayer => LayerMask.GetMask("Ground");

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;

    void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(MovePressed.x * Speed, rb.linearVelocity.y);
    }

    void UpdateMove()
    {   
        // Ground 
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, groundLayer);

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

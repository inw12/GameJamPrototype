using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] private float Speed; 
    [SerializeField] private float Acceleration; 
    private Rigidbody2D rb;
    private PlayerControls inputs;

    [Header("Gravity")]
    [SerializeField] private float GroundRayLength;
    public bool IsGrounded = false;
    private LayerMask groundLayer => LayerMask.GetMask("Ground");

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Debug")]
    public bool Debug;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputs = PlayerControls.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputs.MovePressed.x * Speed, rb.linearVelocity.y);
    }

    void UpdateMove()
    {   
        // Ground 
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, groundLayer);

        if (inputs.MovePressed.x > 0f)
            sprite.flipX = true;
        if (inputs.MovePressed.x < 0f)
            sprite.flipX = false;
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
}

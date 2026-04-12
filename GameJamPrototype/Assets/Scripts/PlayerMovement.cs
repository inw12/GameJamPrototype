using UnityEngine;
using System.Collections;

public enum MovementState { Idle = 0, Walk = 1, Sprint = 2, Crawl = 3 }

[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerLimbManager))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerMovement : MonoBehaviour
{
    // Public state
    public Rigidbody2D Rb { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool CanDropDown { get; private set; }
    public bool IsFacingRight { get; private set; }
    public RaycastHit2D LastGroundHit { get; private set; }
    public MovementState MoveState { get; private set; }

    // Private shared state
    [Min(1f)]
    [SerializeField] private float GravityScale;
    [SerializeField] private float GroundRayLength;
    private PlayerLimbManager _limbManager;
    private PlayerAnimator _animator;
    private LayerMask GroundMask => LayerMask.GetMask("Ground", "Platform");

    // Locomotion state machine 
    private BothLegsLocomotion _bothLegs;
    private OneLegLocomotion _oneLeg;
    private NoLegsLocomotion _noLegs;
    private LocomotionBase _active;
    private PlayerLimbManager.LegState _lastLegState;

    // Debug
    [Header("Debug Log")]
    public bool DebugLog;

    // Locomotion State Machine

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        _limbManager = GetComponent<PlayerLimbManager>();
        _animator = GetComponent<PlayerAnimator>();

        _bothLegs = GetComponent<BothLegsLocomotion>();
        _oneLeg = GetComponent<OneLegLocomotion>();
        _noLegs = GetComponent<NoLegsLocomotion>();

        SwitchLocomotion(_limbManager.CurrentLegState);

        IsFacingRight = true;

        Rb.gravityScale = GravityScale;
    }

    void Update()
    {
        var currentLegState = _limbManager.CurrentLegState;
        if (currentLegState != _lastLegState)
            SwitchLocomotion(currentLegState);

        UpdateShared();
        _active.OnUpdate();
    }

    void FixedUpdate()
    {
        _active.OnFixedUpdate();
    }

    void LateUpdate()
    {
        _active.OnLateUpdate();
    }

    private void SwitchLocomotion(PlayerLimbManager.LegState legState)
    {
        _active?.OnExit();
        _lastLegState = legState;

        _active = legState switch
        {
            PlayerLimbManager.LegState.BothLegs => _bothLegs,
            PlayerLimbManager.LegState.OneLeg => _oneLeg,
            PlayerLimbManager.LegState.NoLegs => _noLegs,
            _ => _bothLegs
        };

        _bothLegs.enabled = _active == _bothLegs;
        _oneLeg.enabled = _active == _oneLeg;
        _noLegs.enabled = _active == _noLegs;

        _active.OnEnter();
    }

    // Shared Behaviour
    private void UpdateShared()
    {
        // Update Movement State
        if (PlayerControls.Instance.MovePressed.sqrMagnitude == 0f)
            MoveState = MovementState.Idle;
        else 
            MoveState = PlayerControls.Instance.SprintPressed ? MovementState.Sprint : MovementState.Walk;

        // Ground check
        LastGroundHit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLength, GroundMask);
        IsGrounded = LastGroundHit;

        // Flip to face mouse cursor
        var x = PlayerControls.GetMouseWorldPosition().x > transform.position.x ? 1f : -1f;
        if (x > 0f && !IsFacingRight) FlipCharacter();
        if (x < 0f && IsFacingRight) FlipCharacter();

        // Update Animator
        _animator.UpdateGroundAnim(IsGrounded, MoveState);
    }

    // Helpers
    private void FlipCharacter()
    {
        IsFacingRight = !IsFacingRight;
        var scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
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
        GUILayout.Label($"Move: {PlayerControls.Instance.MovePressed}");
    }
}

using UnityEngine;

public enum MovementState
{
    Idle = 0,
    Walk = 1,
    Sprint = 2
}

[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerLimbManager))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerMovement : MonoBehaviour
{
    // Public state
    public Rigidbody2D Rb { get; private set; }
    public MovementState State { get; set; }
    public bool IsFacingRight => _facingRight;

    // Private shared state

    private PlayerAnimator _playerAnimator;
    private PlayerLimbManager _limbManager;
    private bool _facingRight;

    // Locomotion state machine 
    private BothLegsLocomotion _bothLegs;
    private OneLegLocomotion _oneLeg;
    private NoLegsLocomotion _noLegs;
    private LocomotionBase _active;
    private PlayerLimbManager.LegState _lastLegState;

    // Locomotion State Machine

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        _limbManager = GetComponent<PlayerLimbManager>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        State = MovementState.Idle;

        _bothLegs = GetComponent<BothLegsLocomotion>();
        _oneLeg = GetComponent<OneLegLocomotion>();
        _noLegs = GetComponent<NoLegsLocomotion>();

        SwitchLocomotion(_limbManager.CurrentLegState);
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
        _playerAnimator.UpdateLocAnim(State);
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
        // Idle override
        if (PlayerControls.Instance.MovePressed.sqrMagnitude == 0f)
            State = MovementState.Idle;

        // Flip to face mouse cursor
        var x = PlayerControls.GetMouseWorldPosition().x > transform.position.x ? 1f : -1f;
        if (x > 0f && !_facingRight) FlipCharacter();
        if (x < 0f && _facingRight) FlipCharacter();
    }

    // Helpers
    private void FlipCharacter()
    {
        _facingRight = !_facingRight;
        var scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }



}

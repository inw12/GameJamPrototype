using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerLimbManager))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon StartingWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;

    [Header("Pickup Interaction")]
    [SerializeField] private Transform pickupOrigin;
    [SerializeField] private float pickupRadius;

    // Events
    public event Action OnWeaponChange;

    // References
    private PlayerMovement playerMovement;
    private PlayerAnimator Animator;
    private PlayerLimbManager _limbManager;
    private Weapon _weapon;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponent<PlayerAnimator>();
        _limbManager = GetComponent<PlayerLimbManager>();

        if (StartingWeapon != null)
        {
            _weapon = Weapon.Equip(StartingWeapon, attachTo, true);
            _weapon.OnAttack += Animator.TriggerRanged;
        }
    }

    void Update()
    {
        if (!_weapon) return;
        _weapon.gameObject.SetActive(_limbManager.CanShoot);

        if (!_limbManager.CanShoot) return;
        AttackLoop();
        ChangeWeaponLoop();
    }

    void LateUpdate()
    {
        if (!_limbManager.CanShoot) return;
        UpdateArms();
    }

    private void AttackLoop()
    {
        // Read attack input
        if (PlayerControls.Instance.Mouse1)
        {
            _weapon.Attack();
        }
    }

    private void ChangeWeaponLoop()
    {
        // Scan for interactable objects
        Collider2D[] hits = Physics2D.OverlapCircleAll(pickupOrigin.position, pickupRadius);
        Collider2D[] nearbyWeapons = System.Array.FindAll(hits, h => h.GetComponent<Weapon>() != null);

        if (nearbyWeapons.Length > 0)
        {
            Collider2D closest = Helper.GetClosestCollider(transform.position, nearbyWeapons);

            if (closest.TryGetComponent(out Weapon newWeapon))
            {
                newWeapon.OnPlayerNearby();

                if (PlayerControls.Instance.InteractPressed)
                {
                    Animator.TriggerPickup();
                    ChangeWeapon(newWeapon);
                }
            }

        }
    }

    private void UpdateArms()
    {
        if (_weapon != null)
        {
            // position
            var mousePos = PlayerControls.GetMouseWorldPosition();
            backArmTarget.position = mousePos;

            // rotation
            var angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            angle = playerMovement.IsFacingRight ? angle + 180f : angle;
            backArmTarget.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void ChangeWeapon(Weapon newWeapon)
    {
        // Destroy current weapon instance
        if (_weapon != null)
        {
            _weapon.OnAttack -= Animator.TriggerRanged;
            Destroy(_weapon.gameObject);
            OnWeaponChange?.Invoke();
        }

        // Update current weapon
        _weapon = Weapon.Equip(newWeapon, attachTo, true);
        _weapon.OnAttack += Animator.TriggerRanged;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.orangeRed;
        Gizmos.DrawWireSphere(pickupOrigin.position, pickupRadius);
    }
}

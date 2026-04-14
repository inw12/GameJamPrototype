using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerLimbManager))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;

    [Header("Pickup Interaction")]
    [SerializeField] private Transform pickupOrigin;
    [SerializeField] private float pickupRadius;
    private LayerMask PickupLayer => LayerMask.GetMask("Pickup");

    // References
    private PlayerMovement playerMovement;
    private PlayerAnimator Animator;
    private PlayerLimbManager _limbManager;
    private GameObject _weaponObjectInstance;
    private Weapon _weapon;
    private bool _weaponEquipped;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponent<PlayerAnimator>();
        _limbManager = GetComponent<PlayerLimbManager>();

        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, attachTo);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();
            _weaponEquipped = CheckWeaponType(_weapon);
        }
    }

    void Update()
    {
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
        var item = Physics2D.OverlapCircle(pickupOrigin.position, pickupRadius, PickupLayer);
        if (item && item.TryGetComponent(out WeaponPickup weapon))
        {
            weapon.TogglePrompt();

            // weapon change interaction
            if (PlayerControls.Instance.InteractPressed)
            {
                ChangeWeapon(weapon.GetItem());
                weapon.DestroyObject();
            }
        }
    }

    private void UpdateArms()
    {
        if (_weaponEquipped)
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

    // * idk if we still need this... *
    private bool CheckWeaponType<T>(T weapon)
    {
        return weapon switch
        {
            RangedWeapon => true,
            MeleeWeapon => false,
            _ => false,
        };
    }

    private void ChangeWeapon(GameObject newWeapon)
    {
        // Destroy current weapon instance
        if (_weaponObjectInstance) Destroy(_weaponObjectInstance);

        // Update current weapon
        currentWeapon = newWeapon;
        _weaponObjectInstance = Instantiate(currentWeapon, attachTo);
        _weapon = _weaponObjectInstance.GetComponent<Weapon>();

        _weaponEquipped = CheckWeaponType(_weapon);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.orangeRed;
        Gizmos.DrawWireSphere(pickupOrigin.position, pickupRadius);
    }

}

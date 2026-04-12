using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;

    // References
    private PlayerMovement playerMovement;
    private PlayerAnimator Animator;
    private GameObject _weaponObjectInstance;
    private Weapon _weapon;
    private bool _weaponEquipped;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponent<PlayerAnimator>();

        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, transform);
            _weaponObjectInstance.transform.SetParent(attachTo);
            _weaponObjectInstance.transform.localPosition = Vector2.zero;
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();

            _weaponEquipped = CheckWeaponType(_weapon);
            Animator.UpdateWeaponAnim(_weaponEquipped);
        }
    }

    void Update()
    {
        bool grappleOn = PlayerControls.Instance.JumpPressed;
        _weapon.gameObject.SetActive(!grappleOn);

        // Read attack input
        if (PlayerControls.Instance.Mouse1)
        {
            _weapon.Attack();
        }
    }

    void LateUpdate()
    {
        // update weapon arms (if active)
        UpdateArms();
    }

    public void UpdateArms()
    {
        if (_weaponEquipped)
        {
            // position
            var mousePos = PlayerControls.GetMouseWorldPosition();
            backArmTarget.position = mousePos;

            // rotation
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            backArmTarget.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private bool CheckWeaponType<T>(T weapon)
    {
        return weapon switch
        {
            RangedWeapon => true,
            MeleeWeapon => false,
            _ => false,
        };
    }
}

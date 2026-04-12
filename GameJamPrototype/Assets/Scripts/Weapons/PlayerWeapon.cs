using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerAnimator animationController;
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;
    private PlayerMovement playerMovement;
    private PlayerControls playerControls;

    private GameObject _weaponObjectInstance;
    private Weapon _weapon;
    private bool _weaponEquipped;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerControls = GetComponent<PlayerControls>();

        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, transform);
            _weaponObjectInstance.transform.SetPositionAndRotation(attachTo.position, currentWeapon.transform.rotation);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();

            _weaponEquipped = CheckWeaponType(_weapon);
            //animationController.UpdateAnimator(_weaponEquipped);
        }
    }

    void Update()
    {
        // Read attack input
        if (PlayerControls.Instance.Mouse1)
        {
            _weapon.Attack(PlayerControls.GetMouseWorldPosition());
        }
    }

    void LateUpdate()
    {
        // update weapon position/rotation
        if (_weaponObjectInstance)
        {
            // position
            _weaponObjectInstance.transform.position = attachTo.position;

            if (_weaponEquipped)
            {
                // rotation
                var targetPosition = PlayerControls.GetMouseWorldPosition();
                var angle = Mathf.Atan2(targetPosition.y - attachTo.position.y, targetPosition.x - attachTo.position.x) * Mathf.Rad2Deg;
                angle = playerMovement.IsFacingRight ? angle : angle + 180f;
                _weaponObjectInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        // update weapon arms (if active)
        UpdateArms();
    }


    public void UpdateArms()
    {
        if (_weaponEquipped)
        {
            // position
            var mousePos = PlayerControls.GetMouseWorldPosition();
            frontArmTarget.position = mousePos;

            // rotation
            var angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            angle = playerMovement.IsFacingRight ? angle + 180f : angle;
            frontArmTarget.rotation = Quaternion.Euler(0f, 0f, angle);
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

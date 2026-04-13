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
            _weaponObjectInstance = Instantiate(currentWeapon, attachTo);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();

            _weaponEquipped = CheckWeaponType(_weapon);
        }
    }

    void Update()
    {
        // Read attack input
        if (PlayerControls.Instance.Mouse1)
        {
            _weapon.Attack();
        }
    }

    void LateUpdate()
    {
        // update weapon position/rotation
        // if (_weaponObjectInstance)
        // {
        //     // position
        //     _weaponObjectInstance.transform.position = attachTo.position;

        //     if (_weaponEquipped)
        //     {
        //         // rotation
        //         var targetPosition = PlayerControls.GetMouseWorldPosition();
        //         var angle = Mathf.Atan2(targetPosition.y - attachTo.position.y, targetPosition.x - attachTo.position.x) * Mathf.Rad2Deg;
        //         angle = playerMovement.IsFacingRight ? angle : angle + 180f;
        //         _weaponObjectInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //     }
        // }

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
            var angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            angle = playerMovement.IsFacingRight ? angle + 180f : angle;
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

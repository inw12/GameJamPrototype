using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private ArmAim armAim;

    private GameObject _weaponObjectInstance;
    private Weapon _weapon;
    private bool _weaponEquipped;

    void Start()
    {
        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, transform);
            _weaponObjectInstance.transform.SetPositionAndRotation(attachTo.position, currentWeapon.transform.rotation);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();

            _weaponEquipped = CheckWeaponType(_weapon);
            animationController.UpdateAnimator(_weaponEquipped);
            armAim.WeaponArmsActive(_weaponEquipped);
        }
    }

    void Update()
    {
        // Read input for attack
        if (PlayerControls.Instance)
        {
            var input = PlayerControls.Instance;

            // Read attack input
            if (input.Mouse1)
            {
                _weapon.Attack(input.GetMouseWorldPosition());
            }
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
                var targetPosition = PlayerControls.Instance.GetMouseWorldPosition();
                var angle = Mathf.Atan2(targetPosition.y - attachTo.position.y, targetPosition.x - attachTo.position.x) * Mathf.Rad2Deg;
                angle = playerMovement.IsFacingRight() ? angle : angle + 180f;
                _weaponObjectInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
        
        // update weapon arms (if active)
        armAim.UpdateArms();
    }

    public void SwitchWeapon(Weapon weapon)
    {

    }

    private bool CheckWeaponType<T>(T weapon)
    {
        return weapon switch
        {
            RangedWeapon    => true,
            MeleeWeapon     => false,
            _               => false,
        };
    }
}

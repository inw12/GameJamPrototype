using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private ArmAim armAim;

    private GameObject _weaponObjectInstance;
    private Weapon _weapon;

    void Start()
    {
        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, transform);
            _weaponObjectInstance.transform.SetPositionAndRotation(attachTo.position, currentWeapon.transform.rotation);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();

            animationController.UpdateAnimator(currentWeapon);
            armAim.WeaponArmsActive(currentWeapon);
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
        // update weapon positioning
        if (_weaponObjectInstance) _weaponObjectInstance.transform.position = attachTo.position;        
        
        // update weapon arms (if active)
        armAim.UpdateArms();
    }

    public void SwitchWeapon(Weapon weapon)
    {

    }
}

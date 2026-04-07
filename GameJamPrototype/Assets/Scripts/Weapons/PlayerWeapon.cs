using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;

    private GameObject _weaponObjectInstance;
    private Weapon _weapon;

    void Start()
    {
        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, transform);
            _weaponObjectInstance.transform.SetPositionAndRotation(attachTo.position, currentWeapon.transform.rotation);
            _weapon = _weaponObjectInstance.GetComponent<Weapon>();
        }
    }

    void Update()
    {

    }

    // update weapon positioning
    void LateUpdate()
    {
        if (_weaponObjectInstance) _weaponObjectInstance.transform.position = attachTo.position;

        animationController.UpdateAnimator(currentWeapon);
    }

    public void SwitchWeapon(Weapon weapon)
    {

    }
}

using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;

    private GameObject _weaponObjectInstance;
    private Weapon _weapon;

    private Vector2 _mousePos;

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
        // Read input for attack
        if (PlayerControls.Instance)
        {
            var input = PlayerControls.Instance;

            // Get mouse position
            Vector3 screenPos = input.Mouse;
            screenPos.z = -Camera.main.transform.position.z;
            _mousePos = Camera.main.ScreenToWorldPoint(screenPos);

            // Read attack input
            if (input.Mouse1)
            {
                _weapon.Attack(_mousePos);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_mousePos, 0.25f);
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

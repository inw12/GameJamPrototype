using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private Transform attachTo;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;

    private GameObject _weaponObjectInstance;

    void Start()
    {
        if (currentWeapon)
        {
            _weaponObjectInstance = Instantiate(currentWeapon, attachTo.position, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }

    // Used to update transform of weapon sprite
    void LateUpdate()
    {
        _weaponObjectInstance.transform.position = attachTo.position;
    }

    public void SwitchWeapon(Weapon weapon)
    {

    }
}
